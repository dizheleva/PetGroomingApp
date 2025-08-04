namespace PetGroomingApp.GCommon.Constants
{
    public static class GroomerConstants
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
