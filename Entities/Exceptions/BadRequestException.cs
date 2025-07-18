﻿namespace Entities.Exceptions;

public abstract class BadRequestException(string message) : Exception(message)
{

}

public sealed class MaxAgeRangeBadRequestException : BadRequestException
{
    public MaxAgeRangeBadRequestException()
        : base("Max age can't be less than min age.")
    {
    }
}