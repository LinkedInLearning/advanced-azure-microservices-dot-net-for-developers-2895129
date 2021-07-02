using System;

namespace WisdomPetMedicine.Hospital.Api.Commands
{
    public class AddProcedureCommand
    {
        public Guid Id { get; set; }
        public string Procedure { get; set; }
    }
}