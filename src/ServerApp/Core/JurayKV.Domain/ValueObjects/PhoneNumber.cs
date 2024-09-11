﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.Primitives;

namespace JurayKV.Domain.ValueObjects;
[NotMapped]

public sealed class PhoneNumber : ValueObject
{
    private const int _minLength = 10;
    private const int _maxLength = 20;

    public PhoneNumber(string value)
    {
        SetValue(value);
    }

    public string Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private void SetValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException("The PhoneNumber value cannot be null or empty.");
        }

        if (value.Length < _minLength || value.Length > _maxLength)
        {
            throw new DomainValidationException($"The PhoneNumber value must be in between {_minLength} && {_maxLength} characters.");
        }

        Value = value;
    }
}
