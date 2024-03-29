﻿using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using Framework.Server.Routing.Attributes;

namespace Framework.Server.Routing;

public class TreeRoute
{
    private class Node
    {
        public string ArgName { get; set; }
        public ArgType ArgType { get; set; }

        public string PlainRoute { get; set; }

        public IDictionary<HttpMethods, MethodInfo> Methods { get; set; }
        public IDictionary<string, Node> PlainSubRoutes { get; private set; }
        public IDictionary<Type, Node> ArgSubRoutes { get; private set; }

        private Node()
        {
            Methods = new Dictionary<HttpMethods, MethodInfo>();
            PlainSubRoutes = new Dictionary<string, Node>();
            ArgSubRoutes = new Dictionary<Type, Node>();
        }

        public Node(string argName, ArgType argType)
            : this()
        {
            ArgName = argName;
            ArgType = argType;
        }

        public Node(string plainRoute)
            : this()
        {
            PlainRoute = plainRoute;
        }
    }

    private Node root = new("");

    private static string plainSegmentRegex = @"([a-zA-Z]+)";
    private static string argSegmentRegex = @"({[a-zA-Z]+})";
    private static string notEmptySegment = $"({plainSegmentRegex}|{argSegmentRegex})";
    private bool IsPlainSegment(string route)
        => Regex.IsMatch(route, $"^{plainSegmentRegex}$");
    private bool IsArgSegment(string route)
        => Regex.IsMatch(route, $"^{argSegmentRegex}$");
    private bool IsCorrectRoute(string route)
        => route == "" ||
        Regex.IsMatch(route, $"^({notEmptySegment}((/{notEmptySegment})*))$");

    public void AddRoute(HttpMethods methodType, string route, MethodInfo method,
        Dictionary<string, (int index, Type type)> methodArgumentNamesToOrderIndex = null!)
    {
        if (!IsCorrectRoute(route))
            throw new ArgumentException($"Неправильно задан маршрут: {route}");

        var curNode = root;
        var segments = route.Split('/');
        if (segments[0] != "")
            foreach (var segment in segments)
            {
                Node node = default!;
                if (IsArgSegment(segment))
                {
                    var argName = segment.Substring(1, segment.Length - 2);
                    if (methodArgumentNamesToOrderIndex is null ||
                        !methodArgumentNamesToOrderIndex.ContainsKey(argName))
                        throw new ArgumentException("В маршруте есть аргумент, но он не описан");
                    var argumentData = methodArgumentNamesToOrderIndex[argName];
                    var argType = ArgType.GetArgType(argumentData.type) ?? throw new NotImplementedException("Неизвестный тип :(");

                    node = new(argName, argType);
                    if (!curNode.ArgSubRoutes.ContainsKey(argType.Type))
                        curNode.ArgSubRoutes[argType.Type] = node;

                    curNode = curNode.ArgSubRoutes[argType.Type];
                }
                else if (IsPlainSegment(segment))
                {
                    var lowerSegment = segment.ToLower();
                    node = new(lowerSegment);
                    if (!curNode.PlainSubRoutes.ContainsKey(lowerSegment))
                        curNode.PlainSubRoutes[lowerSegment] = node;

                    curNode = curNode.PlainSubRoutes[lowerSegment];
                }
                else
                    throw new Exception("Переделай регулярку она глючная!!!");
            }

        curNode.Methods[methodType] = method;

        var controller = method.DeclaringType!;
    }

    private Dictionary<string, string> ParseAsQuery(string query)
        => query
            .Split('&')
            .Where(x => x.Contains('='))
            .Select(x => x.Split('='))
            .ToDictionary(x => x[0], x => x[1]);

    private object[] GetParameters(
        StreamReader inputStream, MethodInfo method,
        IDictionary<string, object> namesToValue,
        string queryString)
    {
        var parameters = method.GetParameters();
        var result = new object[parameters.Length];

        var inputStreamString = inputStream.ReadToEnd();
        var decodedString = HttpUtility.UrlDecode(inputStreamString);
        var fromInputStreamNameToValue = ParseAsQuery(decodedString);

        var fromQueryStringNameToValue = ParseAsQuery(queryString);

        for (int i = 0; i < parameters.Length; i++)
        {
            var name = parameters[i].Name;
            object value = default!;
            if (fromQueryStringNameToValue.TryGetValue(name, out var queryValue))
                value = queryValue;
            if (fromInputStreamNameToValue.TryGetValue(name, out var val))
                value = val;
            if (namesToValue.TryGetValue(name, out var obj))
                value = obj;
            try
            {
                if (parameters[i].ParameterType == typeof(bool))
                {
                    value = value?.ToString()?.ToLower() switch
                    {
                        "true" or "on" or "1" => true,
                        "false" or "off" or "0" or null => false
                    };
                }
                result[i] = Convert.ChangeType(value, parameters[i].ParameterType);
            }
            catch
            {
                result[i] = value;
            }
        }

        return result;
    }

    public bool TryNavigate(HttpMethods methodType, string route,
        Stream inputStream, Encoding requestEncoding,
        out MethodInfo method, out object[] parameters)
    {
        method = default!;
        parameters = default!;
        route = route.ToLower();

        if (route == "")
            return root.Methods.TryGetValue(methodType, out method!);

        if (!Regex.IsMatch(route, @"^(([\S]+(/[\S]+)*)|)$"))
            return false;

        Dictionary<string, object> parameterValuesFromRoutes = new();

        var curNode = root;
        string query = "";
        if (route.Contains('?'))
        {
            var queryStart = route.IndexOf('?');
            query = route.Substring(queryStart + 1);
            route = route.Substring(0, queryStart);
        }
        var segments = route.Split('/');
        for (int i = 0; i < segments.Length; i++)
        {
            var segment = segments[i];

            if (curNode.PlainSubRoutes.ContainsKey(segment))
                curNode = curNode.PlainSubRoutes[segment];

            else if (ArgType.TryParse(segment, out var val, out var type)
                && curNode.ArgSubRoutes.ContainsKey(type))
            {
                curNode = curNode.ArgSubRoutes[type];
                parameterValuesFromRoutes[curNode.ArgName] = val;
            }
            else
                return false;
        }
        if (!curNode.Methods.TryGetValue(methodType, out method!))
            return false;
        using (var stream = new StreamReader(inputStream, requestEncoding))
            parameters = GetParameters(stream, method, parameterValuesFromRoutes, query);
        return true;
    }
}
