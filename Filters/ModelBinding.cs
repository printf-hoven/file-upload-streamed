using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Journal.Filters;

// the following taken from
// https://github.com/dotnet/AspNetCore.Docs/tree/2f0088ce754b123b924b7a23fced06e6fbb45fa9/aspnetcore/mvc/models/file-uploads/samples/3.x/SampleApp

// filter that disables form-model binding because we shall be writing
// our own code to read the multipart sections. 

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
{
  public void OnResourceExecuting(ResourceExecutingContext context)
  {
    var factories = context.ValueProviderFactories;
    factories.RemoveType<FormValueProviderFactory>();
    factories.RemoveType<FormFileValueProviderFactory>();
    factories.RemoveType<JQueryFormValueProviderFactory>();
  }

  public void OnResourceExecuted(ResourceExecutedContext context)
  {
  }

}
