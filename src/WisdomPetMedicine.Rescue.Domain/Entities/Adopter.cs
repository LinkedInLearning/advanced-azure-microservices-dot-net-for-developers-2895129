﻿using System;
using WisdomPetMedicine.Rescue.Domain.Events;
using WisdomPetMedicine.Rescue.Domain.Exceptions;
using WisdomPetMedicine.Rescue.Domain.ValueObjects;

namespace WisdomPetMedicine.Rescue.Domain.Entities;

public class Adopter(Guid id)
{
    public Guid Id { get; init; } = id;
    public AdopterName Name { get; private set; }
    public AdopterQuestionnaire Questionnaire { get; private set; }
    public AdopterAddress Address { get; private set; }
    public AdopterPhoneNumber PhoneNumber { get; private set; }

    public void SetName(AdopterName name)
    {
        Name = name;
    }

    public void SetQuestionnaire(AdopterQuestionnaire questionnaire)
    {
        Questionnaire = questionnaire;
    }

    public void SetAddress(AdopterAddress address)
    {
        Address = address;
    }

    public void SetPhoneNumber(AdopterPhoneNumber phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }

    public void RequestToAdopt(RescuedAnimalId petId)
    {
        ValidateStateForAdoption();

        DomainEvents.AdoptionRequestCreated.Publish(new AdoptionRequestCreated()
        {
            RescuedAnimalId = petId,
            AdopterId = Id
        });
    }

    private void ValidateStateForAdoption()
    {
        if (Name == null)
        {
            throw new InvalidAdopterStateException("Adopter name is missing");
        }

        if (Questionnaire == null)
        {
            throw new InvalidAdopterStateException("Adopter questionnaire is missing");
        }

        if (Address == null)
        {
            throw new InvalidAdopterStateException("Adopter address is missing");
        }

        if (PhoneNumber == null)
        {
            throw new InvalidAdopterStateException("Adopter phone number is missing");
        }
    }
}