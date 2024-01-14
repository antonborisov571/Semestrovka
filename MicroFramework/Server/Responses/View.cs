using Framework.Server.Extensions;
using Framework.Server.Routing;

namespace Framework.Server.Responses;

public class View : IControllerResult
{
    const string Extension = ".tpl";

    string _viewPath;
    object _model;

    public View(string viewPath) => _viewPath = viewPath;

    public View(string viewPath, object model)
    {
        _viewPath = viewPath;
        _model = model;
    }

    public async void ExecuteResult(Context context)
    {
        var fileName = _viewPath;
        if (string.IsNullOrEmpty(Path.GetExtension(_viewPath))) 
        {
            fileName += Extension;
        }

        var path = Path.Combine(context.Settings.TemplateFolder, fileName);
        using (var file = new FileStream(path, FileMode.Open))
        {
            var engine = new TemplateEngine.TemplateEngine();
            var html = engine.GetHTMLInByte(file, _model);
            context.HttpContext.Response.WriteBody(html, "text/html");
        }
    }
}
