using Shared;

namespace Api.Common;

public static class ToHttpResponse
{
    public static IResult Return (Result result)
    {
        return result switch
        {
            Success s => Results.Ok(s.ResponseMessage),
            Failure f => Results.BadRequest(f.Errors),
        };
    }
}