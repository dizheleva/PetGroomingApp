namespace PetGroomingApp.GCommon
{
    public class ExceptionMessages
    {
        public const string InterfaceNotFoundMessage = "Interface {0} not found for class {1}. " +
                                                      "Please ensure that the interface name matches the class name " +
                                                      "with the prefix 'I' and is defined in the same assembly.";
        
        public const string ServiceNotFoundMessage = "Service {0} not found. Please ensure it is registered in the service collection.";
        
        public const string RepositoryNotFoundMessage = "Repository {0} not found. Please ensure it is registered in the service collection.";
        
        public const string InvalidConfigurationMessage = "Invalid configuration for {0}. Please check your settings.";
    }
}
