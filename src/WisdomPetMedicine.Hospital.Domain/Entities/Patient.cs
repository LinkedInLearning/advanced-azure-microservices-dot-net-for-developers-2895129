using System;
using System.Collections.Generic;
using WisdomPetMedicine.Common;
using WisdomPetMedicine.Hospital.Domain.Events;
using WisdomPetMedicine.Hospital.Domain.Exceptions;
using WisdomPetMedicine.Hospital.Domain.ValueObjects;

namespace WisdomPetMedicine.Hospital.Domain.Entities
{
    public class Patient : AggregateRoot
    {
        private readonly List<Procedure> procedures = new();
        public Guid Id { get; private set; }
        public PatientBloodType BloodType { get; private set; }
        public PatientWeight Weight { get; private set; }
        public PatientStatus Status { get; private set; }
        public IReadOnlyCollection<Procedure> Procedures => procedures;

        public Patient(PatientId id)
        {
            ApplyDomainEvent(new PatientCreated()
            {
                Id = id
            });
        }

        public Patient()
        {

        }

        public void SetBloodType(PatientBloodType bloodType)
        {
            ApplyDomainEvent(new PatientBloodTypeUpdated()
            {
                Id = Id,
                BloodType = bloodType.Value
            });
        }

        public void SetWeight(PatientWeight weight)
        {
            ApplyDomainEvent(new PatientWeightUpdated()
            {
                Id = Id,
                Value = weight.Value
            });
        }

        public void AddProcedure(Procedure procedure)
        {
            ApplyDomainEvent(new PatientProcedureAdded()
            {
                PatientId = Id,
                Id = procedure.Id,
                ProcedureName = procedure.Name.Value
            });
        }

        public void AdmitPatient()
        {
            ApplyDomainEvent(new PatientAdmitted()
            {
                Id = Id
            });
        }

        public void DischargePatient()
        {
            ApplyDomainEvent(new PatientDischarged()
            {
                Id = Id
            });
        }

        protected override void ChangeStateByUsingDomainEvent(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case PatientCreated e:
                    Id = e.Id;
                    Status = PatientStatus.Pending;
                    break;
                case PatientBloodTypeUpdated e:
                    BloodType = new PatientBloodType(e.BloodType);
                    break;
                case PatientWeightUpdated e:
                    Weight = new PatientWeight(e.Value);
                    break;
                case PatientAdmitted:
                    Status = PatientStatus.Admitted;
                    break;
                case PatientDischarged:
                    Status = PatientStatus.Discharged;
                    break;
                case PatientProcedureAdded e:
                    var newProcedure = new Procedure(e.Id, e.ProcedureName);
                    procedures.Add(newProcedure);
                    break;
                default:
                    break;
            }
        }

        protected override void ValidateState()
        {
            var isValid = 
                Status switch
                {
                    PatientStatus.Admitted => BloodType != null && Weight != null,
                    _ => true
                };
            if (!isValid)
            {
                throw new InvalidPatientStateException("Invalid patient state");
            }
        }
    }
}