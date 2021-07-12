using System;

namespace WisdomPetMedicine.Hospital.Infrastructure
{
    public class CosmosEventData
    {
        public Guid Id { get; set; }
        public string AggregateId { get; set; }
        public string EventName { get; set; }
        public string Data { get; set; }
        public string AssemblyQualifiedName { get; set; }
    }
}