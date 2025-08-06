namespace PetGroomingApp.Services.Common
{    
    public static class Constants
    {
        public static class Service
        {           
            public const string DurationFormat = @"hh\:mm"; // TimeSpan format for display
            public const string DurationInvalidMessage = "Invalid duration format. Use HH:mm.";
        }

        public static class Pet
        {
            public const string DefaultPetUrl = "img/pet/defaultPet.png";

            public const string InvalidOperationMessage = "Pet not found or access denied.";
            public const string IdInvalidMessage = "Invalid ID format.";
            public const string PetNotFoundMessage = "Pet not found.";
            public const string NotEditableMessage = "Cannot edit pet.";
        }

        public static class  Appointment
        {
            public const string NotEditableMessage = "Cannot edit appointment.";
            public const string NotPastTimeMessage = "Appointment time cannot be in the past.";
            public const string AlreadyCompletedMessage = "Appointment is already completed.";
            public const string NotChosenPetName = "No pet";
        }
    }
}
