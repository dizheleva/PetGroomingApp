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
}
