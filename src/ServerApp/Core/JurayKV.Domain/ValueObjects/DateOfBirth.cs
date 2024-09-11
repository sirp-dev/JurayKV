using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.Primitives;

namespace JurayKV.Domain.ValueObjects;

[NotMapped]

public sealed class DateOfBirth : ValueObject
{
    private readonly DateTime _minDateOfBirth = DateTime.UtcNow.AddYears(-115);

    private readonly DateTime _maxDateOfBirth = DateTime.UtcNow.AddYears(-15);

    public DateOfBirth(DateTime value)
    {
        SetValue(value);
    }

    public DateTime Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private void SetValue(DateTime value)
    {
        if (value < _minDateOfBirth || value > _maxDateOfBirth)
        {
            throw new DomainValidationException("The minimum age has to be 15 years.");
        }

        Value = value;
    }
}

public class DateForSix
{
    public static DateTime GetTheDateBySix(DateTime date)
    {
        DateTime targetTime = new DateTime(date.Year, date.Month, date.Day, 6, 0, 0);

        DateTime resultDate = date;
        // Check if the current time is before 6am on the target date
        if (date < targetTime)
        {
            resultDate = date.Date.AddDays(-1);
        }
        return resultDate.Date;
    }

    //public static DateDto GetTheDateBySix(DateTime date)
    //{
    //    DateDto.
    //    DateTime today6AM = currentDate.Date.AddHours(6);
    //    return resultDate.Date;
    //}
}

//public class DateDto
//{
//    public DateTime CurrentSixAm { get;set;}
//    public DateTime NexSixAm { get;set; }
//}
