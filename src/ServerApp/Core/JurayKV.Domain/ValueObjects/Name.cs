﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.Primitives;

namespace JurayKV.Domain.ValueObjects;
[NotMapped]

public sealed class Name : ValueObject
{
    private const int _minLength = 2;
    private const int _maxLength = 50;

    public Name(string firstName, string lastName)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
    }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    private void SetFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new DomainValidationException("The FirstName cannot be null or empty.");
        }

        if (firstName.Length < _minLength || firstName.Length > _maxLength)
        {
            throw new DomainValidationException($"The FirstName must be in between {_minLength} to {_maxLength} characters.");
        }

        FirstName = firstName;
    }

    private void SetLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new DomainValidationException("The LastName cannot be null or empty.");
        }

        if (lastName.Length < _minLength || lastName.Length > _maxLength)
        {
            throw new DomainValidationException($"The LastName must be in between {_minLength} to {_maxLength} characters.");
        }

        LastName = lastName;
    }
}
