namespace PetGroomingApp.Data.Seeding
{
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Seeding.Utilities;

    public static class DbSeeder
    {        
        public static List<Service> SeedServices()
        {
            return JsonSeederHelper.LoadSeedData<Service>("services.json");
        }

        public static List<Service> SeedGroomers()
        {
            return JsonSeederHelper.LoadSeedData<Service>("groomers.json");
        }

        public static List<Service> SeedPets()
        {
            return JsonSeederHelper.LoadSeedData<Service>("pets.json");
        }

        public static List<Service> SeedAppointments()
        {
            return JsonSeederHelper.LoadSeedData<Service>("appointments.json");
        }
    }

}
