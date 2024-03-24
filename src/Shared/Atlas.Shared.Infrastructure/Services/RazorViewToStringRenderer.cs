using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Atlas.Shared.Infrastructure.Services;

/// <summary>
/// Renders Razor views to strings for further processing or inclusion in email templates.
/// Code from: https://github.com/aspnet/Entropy/blob/master/samples/Mvc.RenderViewToString/RazorViewToStringRenderer.cs
/// </summary>
/// <param name="viewEngine">The Razor view engine.</param>
/// <param name="tempDataProvider">The temporary data provider.</param>
/// <param name="serviceProvider">The service provider for dependency injection.</param>
public class RazorViewToStringRenderer(
    IRazorViewEngine viewEngine,
    ITempDataProvider tempDataProvider,
    IServiceProvider serviceProvider)
{
    /// <summary>
    /// Renders the specified Razor view to a string asynchronously.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to be used in the view.</typeparam>
    /// <param name="viewName">The name of the view to be rendered.</param>
    /// <param name="model">The model to be used in the view.</param>
    /// <returns>A string representation of the rendered Razor view.</returns>
    public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
    {
        ActionContext actionContext = GetActionContext();
        IView view = FindView(actionContext, viewName);

        await using var output = new StringWriter();
        var viewContext = new ViewContext(
            actionContext,
            view,
            new ViewDataDictionary<TModel>(
                metadataProvider: new EmptyModelMetadataProvider(),
                modelState: new ModelStateDictionary())
            {
                Model = model
            },
            new TempDataDictionary(
                actionContext.HttpContext,
                tempDataProvider),
            output,
            new HtmlHelperOptions());

        await view.RenderAsync(viewContext);

        return output.ToString();
    }

    /// <summary>
    /// Finds the specified Razor view using the provided action context and view name.
    /// </summary>
    /// <param name="actionContext">The action context for the current request.</param>
    /// <param name="viewName">The name of the view to be found.</param>
    /// <returns>The found Razor view.</returns>
    private IView FindView(ActionContext actionContext, string viewName)
    {
        ViewEngineResult getViewResult = viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
        if (getViewResult.Success)
        {
            return getViewResult.View;
        }

        ViewEngineResult findViewResult = viewEngine.FindView(actionContext, viewName, isMainPage: true);
        if (findViewResult.Success)
        {
            return findViewResult.View;
        }

        IEnumerable<string> searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
        string errorMessage = string.Join(
            Environment.NewLine,
            new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations));

        throw new InvalidOperationException(errorMessage);
    }

    /// <summary>
    /// Gets the action context for rendering Razor views.
    /// </summary>
    /// <returns>The action context for rendering Razor views.</returns>
    private ActionContext GetActionContext()
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };
        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }

}
