using System;
using WisdomPetMedicine.Hospital.Domain.Entities;
using WisdomPetMedicine.Hospital.Domain.Exceptions;
using WisdomPetMedicine.Hospital.Domain.ValueObjects;
using Xunit;

namespace WisdomPetMedicine.Hospital.Domain.Tests
{
    public class PatientTest
    {
        [Fact]
        public void PatientCannotBeAdmittedWithoutBloodTypeSet()
        {
            var patient = new Patient(PatientId.Create(Guid.NewGuid()));
            Assert.Throws<InvalidPatientStateException>(() =>
            {
                patient.AdmitPatient();
            });
        }

        [Fact]
        public void PatientCanBeAdmittedWithBloodTypeSet()
        {
            var patient = new Patient(PatientId.Create(Guid.NewGuid()));
            patient.SetBloodType(PatientBloodType.Create("DEA-1.1"));
            patient.AdmitPatient();
        }
    }
}
