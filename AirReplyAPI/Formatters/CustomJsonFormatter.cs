using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace AirReplyAPI.Formatters
{
    public class CustomJsonFormatter : InputFormatter
    {
        public CustomJsonFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            using (var reader = new StreamReader(context.HttpContext.Request.Body))
            {
                var content = await reader.ReadToEndAsync();
                var scope = context.HttpContext.RequestServices.CreateScope();
                var logger = scope.ServiceProvider.GetService<ILogger<CustomJsonFormatter>>();
                logger.LogInformation(content);

                return await InputFormatterResult.SuccessAsync(context.HttpContext.Request.Body);
            }
        }
    }
}
