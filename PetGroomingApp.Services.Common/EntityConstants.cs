namespace PetGroomingApp.Services.Common
{    
    public static class EntityConstants
    {
        public static class Service
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
            public const string NameRequiredMessage = "Service name is required.";
            public const string NameMinLengthMessage = "Service name must be at least {1} characters long.";
            public const string NameMaxLengthMessage = "Service name cannot exceed {1} characters.";

            public const int ImageUrlMaxLength = 2048;
            public const string ImageUrlRequiredMessage = "Image URL is required.";
            public const string ImageUrlMaxLengthMessage = "Image URL cannot exceed {1} characters.";

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 500;
            public const string DescriptionMinLengthMessage = "Description must be at least {1} characters long.";
            public const string DescriptionMaxLengthMessage = "Description cannot exceed {1} characters.";

            //public const int DurationMin = 5; // in minutes
            //public const int DurationMax = 480; // in minutes (8 hours)
            public const string DurationRequiredMessage = "Duration is required.";
            //public const string DurationRangeMessage = "Duration must be between {1} and {2} minutes.";

            public const string PriceMin = "0.01";
            public const string PriceMax = "1000.00";
            public const string PriceRequiredMessage = "Price is required.";
            public const string PriceRangeMessage = "Price must be between {1:C} and {2:C}.";
        }

        public static class  Groomer
        {
            public const int FirstNameMinLength = 3;
            public const int FirstNameMaxLength = 50;
            public const string FirstNameRequiredMessage = "Groomer first name is required.";
            public const string FirstNameMinLengthMessage = "Groomer first name must be at least {1} characters long.";
            public const string FirstNameMaxLengthMessage = "Groomer first name cannot exceed {1} characters.";

            public const int LastNameMinLength = 3;
            public const int LastNameMaxLength = 50;
            public const string LastNameRequiredMessage = "Groomer last name is required.";
            public const string LastNameMinLengthMessage = "Groomer last name must be at least {1} characters long.";
            public const string LastNameMaxLengthMessage = "Groomer last name cannot exceed {1} characters.";

            public const int JobTitleMaxLength = 100;
            public const string JobTitleRequiredMessage = "Job title is required.";
            public const string JobTitleMaxLengthMessage = "Job title cannot exceed {1} characters.";

            public const int ImageUrlMaxLength = 2048;
            public const string ImageUrlRequiredMessage = "Image URL is required.";
            public const string ImageUrlMaxLengthMessage = "Image URL cannot exceed {1} characters.";

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 500;
            public const string DescriptionMinLengthMessage = "Description must be at least {1} characters long.";
            public const string DescriptionMaxLengthMessage = "Description cannot exceed {1} characters.";

            public const string PhoneNumberRequiredMessage = "Phone number is required.";
            public const string PhoneNumberPattern = @"^\+?[0-9]{10,15}$"; // Example pattern for international phone numbers
            public const string PhoneNumberInvalidMessage = "Phone number must be between 10 and 15 digits long and can start with a '+' sign.";
        }

        public static class Pet
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
            public const string NameRequiredMessage = "Pet name is required.";
            public const string NameMinLengthMessage = "Pet name must be at least {1} characters long.";
            public const string NameMaxLengthMessage = "Pet name cannot exceed {1} characters.";

            public const string TypeRequiredMessage = "Pet type is required.";
            public const string TypeInvalidMessage = "Invalid pet type.";

            public const int BreedMinLength = 2;
            public const int BreedMaxLength = 50;
            public const string BreedRequiredMessage = "Pet breed is required.";
            public const string BreedMinLengthMessage = "Pet breed must be at least {1} characters long.";
            public const string BreedMaxLengthMessage = "Pet breed cannot exceed {1} characters.";

            public const string SizeRequiredMessage = "Pet size is required.";
            public const string SizeInvalidMessage = "Invalid pet size.";

            public const string GenderRequiredMessage = "Pet gender is required.";
            public const string GenderInvalidMessage = "Invalid pet gender.";

            public const int AgeMinValue = 0;
            public const int AgeMaxValue = 30;
            public const string AgeRequiredMessage = "Pet age is required.";
            public const string AgeRangeMessage = "Pet age must be between {1} and {2} years.";

            public const int ImageUrlMaxLength = 2048;
            public const string ImageUrlMaxLengthMessage = "Image URL cannot exceed {1} characters.";

            public const int NotesMinLength = 10;
            public const int NotesMaxLength = 500;
            public const string NotesMinLengthMessage = "Additional notes must be at least {1} characters long.";
            public const string NotesMaxLengthMessage = "Additional notes cannot exceed {1} characters.";
        }
    }
}
