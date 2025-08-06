namespace PetGroomingApp.GCommon.Constants
{
    public static class Service
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 100;

        public const int ImageUrlMaxLength = 2048;

        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 1000;

        public const string DurationDisplayName = "Duration (hh:mm)";

        public const string PriceMin = "0.01";
        public const string PriceMax = "1000.00";
    }

    public static class Groomer
    {
        public const int FirstNameMinLength = 2;
        public const int FirstNameMaxLength = 50;
        public const int LastNameMinLength = 2;
        public const int LastNameMaxLength = 50;
        public const int JobTitleMaxLength = 100;
        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 500;
        public const int ImageUrlMaxLength = 2048;
        public const string PhoneNumberPattern = @"^\+?\d{10,15}$";
    }

    public static class Pet
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 50;

        public const int BreedMinLength = 2;
        public const int BreedMaxLength = 50;

        public const int ImageUrlMaxLength = 2048;

        public const int NotesMinLength = 5;
        public const int NotesMaxLength = 500;

        public const int AgeMinValue = 0;
        public const int AgeMaxValue = 50;

        public const string OwnerIdRegexPattern = @"^[a-zA-Z0-9-]{36}$";
    }

    public static class Appointment
    {
        public const int NotesMaxLength = 500;

        public const int TotalDurationMin = 10;
        public const int TotalDurationMax = 240; 

        public const decimal TotalPriceMin = 0.00m;
        public const decimal TotalPriceMax = 10000.00m;
    }
}
