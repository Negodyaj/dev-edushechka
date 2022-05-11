using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DevEdu.API.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomDateFormatAttribute : ValidationAttribute
    {
        private const string _dateFormat = "dd.MM.yyyy";
        private readonly int? _maxYears;
        private readonly int? _minYears;

        //public CustomDateFormatAttribute(int? maxYears = null, int? minYears = null)
        //{
        //    _maxYears = maxYears;
        //    _minYears = minYears;
        //}

        public CustomDateFormatAttribute() { }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("ru-RU");
            var parsed = DateTime.TryParseExact(value.ToString(), _dateFormat, cultureInfo, DateTimeStyles.None, out var date);

            if (!parsed || _maxYears.HasValue && date < DateTime.Now.AddYears(-_maxYears.Value) ||
                _minYears.HasValue && date > DateTime.Now.AddYears(-_minYears.Value))
                return false;

            return true;
        }
    }
}