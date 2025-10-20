using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Payment.Application.Results;

namespace Payment.API.Filters;



/// <summary>
/// Guarantees that all results are returned as IResult implementations.
/// </summary>
public class ApiResultFilterAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is OkObjectResult okObjectResult)
        {
            var isHandled = handleIResults(context, okObjectResult);
            if (!isHandled)
            {
                context.Result = new JsonResult(new BaseResult()
                {
                    Object = okObjectResult.Value,
                    StatusCode = okObjectResult.StatusCode
                });
            }
        }
        else if (context.Result is OkResult okResult)
        {
            context.Result = new JsonResult(new BaseResult()
            {
                StatusCode = okResult.StatusCode
            });
        }

        else if (context.Result is ObjectResult badRequestObjectResult && badRequestObjectResult.StatusCode == 400)
        {
            handleValidations(context, badRequestObjectResult);
        }
        else if (context.Result is ObjectResult notFoundObjectResult && notFoundObjectResult.StatusCode == 404)
        {
            string message = null;
            if (notFoundObjectResult.Value != null && !(notFoundObjectResult.Value is ProblemDetails))
                message = notFoundObjectResult.Value.ToString();

            var result = new BaseResult()
            {
                StatusCode = notFoundObjectResult.StatusCode
            };
            result.AddMessage(message, ResultTypes.NotFound);
            context.Result = new JsonResult(result.ClientJSON()) { StatusCode = notFoundObjectResult.StatusCode };
        }
        else if (context.Result is ContentResult contentResult)
        {
            var apiResult = new BaseResult() { StatusCode = contentResult.StatusCode, Object = contentResult.Content };
            context.Result = new JsonResult(apiResult) { StatusCode = contentResult.StatusCode };
        }
        else if (context.Result is ObjectResult objectResult && objectResult.StatusCode == null)
        {
            var apiResult = new BaseResult() { StatusCode = objectResult.StatusCode };
            context.Result = new JsonResult(apiResult) { StatusCode = objectResult.StatusCode };
        }

        base.OnResultExecuting(context);
    }

    private static void handleValidations(ResultExecutingContext context, ObjectResult badRequestObjectResult)
    {
        var result = new BaseResult()
        {
            StatusCode = badRequestObjectResult.StatusCode
        };
        var messageList = new List<string>();
        switch (badRequestObjectResult.Value)
        {
            case ValidationProblemDetails validationProblemDetails:
                messageList.AddRange(validationProblemDetails.Errors.SelectMany(p => p.Value).Distinct());

                break;
            case SerializableError errors:
                messageList.AddRange(errors.SelectMany(p => (string[])p.Value).Distinct());
                break;
            case var value when value != null && !(value is ProblemDetails):
                if (badRequestObjectResult.Value != null)
                    messageList.Add(badRequestObjectResult.Value.ToString());
                break;
        }
        result.AddMessageRange(messageList, ResultTypes.BadRequest);
        context.Result = new JsonResult(result) { StatusCode = badRequestObjectResult.StatusCode };
    }

    private static bool handleIResults(ResultExecutingContext context, OkObjectResult okObjectResult)
    {
        if (okObjectResult.Value != null &&
                        okObjectResult.Value is Application.Results.IResult)
        {
            var res = okObjectResult.Value as Application.Results.IResult;
            if (res != null)
            {
                context.Result = new JsonResult(res.ClientJSON());
                return true;
            }
            else
            {
                context.Result = new JsonResult(new BaseResult() { StatusCode = okObjectResult.StatusCode })
                { StatusCode = okObjectResult.StatusCode };
                return true;
            }

        }
        return false;
    }
}