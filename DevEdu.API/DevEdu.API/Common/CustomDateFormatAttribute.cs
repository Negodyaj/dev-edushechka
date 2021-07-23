using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DevEdu.API.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomDateFormatAttribute : ValidationAttribute
    {
        private const string _dateFormat = "dd.MM.yyyy";
        public override bool IsValid(object value)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("ru-RU");
            return DateTime.TryParseExact(value.ToString(), _dateFormat, cultureInfo, DateTimeStyles.None, out var date);
        }
    }
}