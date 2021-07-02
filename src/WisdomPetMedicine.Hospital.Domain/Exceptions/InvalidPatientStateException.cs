using System;

namespace WisdomPetMedicine.Hospital.Domain.Exceptions
{
    public class InvalidPatientStateException : Exception
    {
        public InvalidPatientStateException(string message) : base(message)
        {
        }
    }
}