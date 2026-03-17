namespace Bankampanya.Application.Common.Exceptions;

public sealed class DomainValidationException(string message) : Exception(message);
