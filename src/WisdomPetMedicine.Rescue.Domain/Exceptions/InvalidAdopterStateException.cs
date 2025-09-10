using System;

namespace WisdomPetMedicine.Rescue.Domain.Exceptions;

public class InvalidAdopterStateException(string message) : Exception(message)
{
}