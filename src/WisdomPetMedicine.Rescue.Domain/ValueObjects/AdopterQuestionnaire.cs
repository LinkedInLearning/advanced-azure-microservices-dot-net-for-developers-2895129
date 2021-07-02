namespace WisdomPetMedicine.Rescue.Domain.ValueObjects
{
    public record AdopterQuestionnaire
    {
        public bool IsActivePerson { get; init; }
        public bool DoYouRent { get; init; }
        public bool HasFencedYard { get; init; }
        public bool HasChildren { get; init; }

        internal AdopterQuestionnaire(bool isActivePerson, bool doYouRent, bool hasFencedYard, bool hasChildren)
        {
            IsActivePerson = isActivePerson;
            DoYouRent = doYouRent;
            HasFencedYard = hasFencedYard;
            HasChildren = hasChildren;
        }

        public static AdopterQuestionnaire Create(bool isActivePerson, bool doYouRent, bool hasFencedYard, bool hasChildren)
        {
            return new AdopterQuestionnaire(isActivePerson, doYouRent, hasFencedYard, hasChildren);
        }
    }
}