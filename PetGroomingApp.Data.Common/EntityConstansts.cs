﻿namespace PetGroomingApp.Data.Common
{
    public static class EntityConstansts
    {
        public static class PetConstants
        {
            // Name stores text between 2 and 100 characters
            public const int PetNameMinLength = 2;
            public const int PetNameMaxLength = 100;

            // Breed stores text between 3 and 80 characters
            public const int BreedMinLength = 3;
            public const int BreedMaxLength = 50;

            // Age can be between 0 and 50 
            public const int PetAgeMin = 3;
            public const int PetAgeMax = 50;

            // Notes stores text between 10 and 1000 characters
            public const int NotesMinLength = 0;
            public const int NotesMaxLength = 1000;

            // Maximum allowed length for image URL
            public const int ImageUrlMaxLength = 2048;

            // Error messages
            public const string RequiredNameErrorMessage = "Name is required.";
            public const string PetNameMinLengthErrorMessage = "Name must be at least 2 characters.";
            public const string PetNameMaxLengthErrorMessage = "Name cannot exceed 100 characters.";

            public const string RequiredPetTypeErrorMessage = "Pet type is required.";

            public const string RequiredBreedErrorMessage = "Breed is required.";
            public const string BreedMinLengthErrorMessage = "Breed name must be at least 2 characters.";
            public const string BreedMaxLengthErrorMessage = "Breed name cannot exceed 80 characters.";

            public const string RequiredPetSizeErrorMessage = "Pet size is required.";

            public const string RequiredAgeErrorMessage = "Age is required.";
            public const string PetAgeErrorMessage = "Age must be between 0 and 50 minutes.";

            public const string NotesMaxLengthErrorMessage = "Notes cannot exceed 1000 characters.";

            public const string ImageUrlMaxLengthErrorMessage = "Image URL cannot exceed 2048 characters.";
        }

        public static class AppointmentConstants
        {
            public const int NotesMaxLength = 1000;
            public const int NotesMinLength = 0;
        }

        public static class GroomerConstants
        {
            public const int FirstNameMaxLength = 50;
            public const int FirstNameMinLength = 2;

            public const int LastNameMaxLength = 50;
            public const int LastNameMinLength = 2;

            public const int DescriptionMaxLength = 1000;
            public const int DescriptionMinLength = 0;

            public const int PhoneNumberMaxLength = 50;
            public const int PhoneNumberMinLength = 0;
        }

        public static class ServiceConstants
        {
            public const int ServiceNameMaxLength = 100;

            public const int ServiceDescriptionMaxLength = 1000;
            public const int ServiceDescriptionMinLength = 0;

            public const string ServicePriceSqlColumnType = "decimal(18,2)";
        }
    }
}
