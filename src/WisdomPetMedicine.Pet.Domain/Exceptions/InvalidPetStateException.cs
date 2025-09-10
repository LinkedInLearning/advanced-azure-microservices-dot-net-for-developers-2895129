using System;

namespace WisdomPetMedicine.Pet.Domain.Exceptions;

public class InvalidPetStateException(string message) : Exception(message)
{
}