using System;

namespace WisdomPetMedicine.Rescue.Domain.Exceptions
{
    public class InvalidAdopterStateException : Exception
    {
        public InvalidAdopterStateException(string message) : base(message)
        {
        }
    }
}