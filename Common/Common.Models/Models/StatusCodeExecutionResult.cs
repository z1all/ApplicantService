namespace Common.Models.Models
{
    public enum StatusCodeExecutionResult
    {
        Ok = 200,
        NoContent = 204,
        BadRequest = 400,
        Unauthorized = 401,
        Forbid = 403,
        NotFound = 404,
        InternalServer = 500,
    }
}
