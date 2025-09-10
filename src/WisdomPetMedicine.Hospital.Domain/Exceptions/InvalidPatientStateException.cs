using System;

namespace WisdomPetMedicine.Hospital.Domain.Exceptions;

public class InvalidPatientStateException(string message) : Exception(message)
{
}