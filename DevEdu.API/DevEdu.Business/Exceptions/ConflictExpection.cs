using System;

namespace DevEdu.Business.Exceptions
{
    public class ConflictExpection : Exception
    {
        public ConflictExpection(string message) : base(message) { }
    }
}
