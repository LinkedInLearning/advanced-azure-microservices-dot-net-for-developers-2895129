using System;
using WisdomPetMedicine.Hospital.Domain.ValueObjects;

namespace WisdomPetMedicine.Hospital.Domain.Entities
{
    public class Procedure
    {
        public Guid Id { get; init; }
        public ProcedureName Name { get; private set; }
        internal Procedure(Guid id, string name)
        {
            Id = id;
            Name = ProcedureName.Create(name);
        }
        protected Procedure()
        {

        }

        public static Procedure Create(string name)
        {
            return new Procedure(Guid.NewGuid(), name);
        }
    }
}