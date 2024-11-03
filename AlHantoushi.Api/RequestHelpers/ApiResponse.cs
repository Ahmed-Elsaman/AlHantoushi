namespace AlHantoushi.Api.RequestHelpers;
public class ApiResponse<TEntity>
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public TEntity Data { get; set; }


    public ApiResponse(int statusCode, TEntity data, string? message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDegaultMessageFromStatusCode(statusCode);
        Data = data;  
    }

    private string? GetDegaultMessageFromStatusCode(int statusCode)
    {
        return statusCode switch
        {
            200 => "OK",
            204 => "No Content",
            400 => "A bad request you have made",
            401 => "Auothrized, you have not",
            404 => "Resource was not found",
            500 => "Errors are the path to the dark side,Erros lead to anger, anger lead to hate , hate lead to shift career",
            _ => null
        };
    }
}
