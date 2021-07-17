using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.InputModels
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateStringFormatAttribute : ValidationAttribute
    {
        private const string _dateFormat = "dd.MM.yyyy";
        public override bool IsValid(object value)
        {
            CultureInfo cultureRu = CultureInfo.CreateSpecificCulture("ru-RU");
            return DateTime.TryParseExact(value.ToString(), _dateFormat, cultureRu, DateTimeStyles.None, out var date);
        }
    }
}
