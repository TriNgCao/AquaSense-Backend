using Microsoft.AspNetCore.Http;

namespace AquaSense.Backend.Exceptions;

public abstract class AppException : Exception
{
    protected AppException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }
}

public sealed class NotFoundException : AppException
{
    public NotFoundException(string message = "Not found")
        : base(message, StatusCodes.Status404NotFound)
    {
    }
}

public sealed class ValidationException : AppException
{
    public ValidationException(string message = "Validation failed")
        : base(message, StatusCodes.Status400BadRequest)
    {
    }
}

public sealed class ConflictException : AppException
{
    public ConflictException(string message = "Conflict")
        : base(message, StatusCodes.Status409Conflict)
    {
    }
}

public sealed class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Unauthorized")
        : base(message, StatusCodes.Status401Unauthorized)
    {
    }
}

public sealed class ForbiddenException : AppException
{
    public ForbiddenException(string message = "Forbidden")
        : base(message, StatusCodes.Status403Forbidden)
    {
    }
}
