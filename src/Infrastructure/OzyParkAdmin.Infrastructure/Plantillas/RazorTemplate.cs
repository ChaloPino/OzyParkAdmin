using RazorEngineCore;
using System.Dynamic;

namespace OzyParkAdmin.Infrastructure.Plantillas;

/// <summary>
/// La plantilla de Razor que contiene el ViewBag además del model.
/// </summary>
/// <typeparam name="TModel"></typeparam>
public class RazorTemplate<TModel> : RazorEngineTemplateBase<TModel>
{
    /// <summary>
    /// El viewbag.
    /// </summary>
    public dynamic? ViewBag { get; set; } = new ExpandoObject();
}
