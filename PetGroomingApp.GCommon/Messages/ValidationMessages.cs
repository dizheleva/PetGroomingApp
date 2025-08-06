namespace PetGroomingApp.GCommon.Messages
{
    public static class Service
    {
        public const string NameRequiredMessage = "Name is required.";
        public const string NameMinLengthMessage = "Name must be at least {1} characters.";
        public const string NameMaxLengthMessage = "Name cannot exceed {1} characters.";

        public const string ImageUrlRequiredMessage = "Image URL is required.";
        public const string ImageUrlMaxLengthMessage = "Image URL cannot exceed {1} characters.";

        public const string DescriptionMinLengthMessage = "Description must be at least {1} characters.";
        public const string DescriptionMaxLengthMessage = "Description cannot exceed {1} characters.";

        public const string DurationRequiredMessage = "Duration is required.";

        public const string PriceRequiredMessage = "Price is required.";
        public const string PriceRangeMessage = "Price must be between {1} and {2}.";
    }

    public static class Groomer
    {
        public const string FirstNameRequiredMessage = "First name is required.";
        public const string FirstNameMinLengthMessage = "First name must be at least {1} characters.";
        public const string FirstNameMaxLengthMessage = "First name cannot exceed {1} characters.";

        public const string LastNameRequiredMessage = "Last name is required.";
        public const string LastNameMinLengthMessage = "Last name must be at least {1} characters.";
        public const string LastNameMaxLengthMessage = "Last name cannot exceed {1} characters.";

        public const string JobTitleRequiredMessage = "Job title is required.";
        public const string JobTitleMaxLengthMessage = "Job title cannot exceed {1} characters.";

        public const string ImageUrlRequiredMessage = "Image URL is required.";
        public const string ImageUrlMaxLengthMessage = "Image URL cannot exceed {1} characters.";

        public const string DescriptionMinLengthMessage = "Description must be at least {1} characters.";
        public const string DescriptionMaxLengthMessage = "Description cannot exceed {1} characters.";

        public const string PhoneNumberRequiredMessage = "Phone number is required.";
        public const string PhoneNumberInvalidMessage = "Phone number is not valid.";
    }

    public static class Pet
    {
        public const string NameRequiredMessage = "Name is required.";
        public const string NameMinLengthMessage = "Name must be at least {1} characters.";
        public const string NameMaxLengthMessage = "Name cannot exceed {1} characters.";

        public const string TypeRequiredMessage = "Pet type is required.";
        public const string TypeInvalidMessage = "Invalid pet type.";

        public const string BreedRequiredMessage = "Breed is required.";
        public const string BreedMinLengthMessage = "Breed must be at least {1} characters.";
        public const string BreedMaxLengthMessage = "Breed cannot exceed {1} characters.";

        public const string SizeRequiredMessage = "Pet size is required.";
        public const string SizeInvalidMessage = "Invalid pet size.";

        public const string GenderRequiredMessage = "Gender is required.";
        public const string GenderInvalidMessage = "Invalid gender.";

        public const string AgeRequiredMessage = "Age is required.";
        public const string AgeRangeMessage = "Age must be between {1} and {2}.";

        public const string ImageUrlMaxLengthMessage = "Image URL cannot exceed {1} characters.";

        public const string NotesMinLengthMessage = "Notes must be at least {1} characters.";
        public const string NotesMaxLengthMessage = "Notes cannot exceed {1} characters.";

        public const string OwnerIdInvalidMessage = "Invalid Owner ID format.";
    }

    public static class Appointment
    {
        public const string AppointmentTimeRequired = "Date and time is required.";
        public const string SelectedServiceIdsRequired = "Please select at least one service.";

        public const string NotesMaxLengthMessage = "Notes cannot exceed {1} characters.";

        public const string SelectedPetIdRequired = "Please select a pet.";
        public const string SelectedGroomerIdRequired = "Please select a groomer.";

        public const string TotalDurationRange = "Duration must be between {1} and {2} minutes.";
        public const string TotalPriceRange = "Price must be between {1} and {2}.";
    }
}
