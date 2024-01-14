namespace Framework.Server;

public static class ContentTypeIdentifier
{
    public static string GetContentType(string filePath)
    {
        switch (Path.GetExtension(filePath))
        {
            case ".html":
                return "text/html; charset=UTF-8";
            case ".json":
                return "application/json";
            case ".css":
                return "text/css; charset=UTF-8";
            case ".js":
                return "text/javascript";
            case ".png":
                return "image/png";
            case ".jpeg":
                return "image/jpeg";
            case ".svg":
                return "image/svg+xml";
            case ".ico":
                return "image/x-icon";
            default:
                return "text/plain";
        }
    } 
}
