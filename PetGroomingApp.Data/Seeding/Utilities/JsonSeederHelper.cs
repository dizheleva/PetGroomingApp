namespace PetGroomingApp.Data.Seeding.Utilities
{
    using System.Text.Json;

    public static class JsonSeederHelper
    {
        public static List<T> LoadSeedData<T>(string relativePath)
        {
            var basePath = AppContext.BaseDirectory;
            var fullPath = Path.Combine(basePath, "Data", "SeedData", relativePath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Seed file not found: {fullPath}");

            var json = File.ReadAllText(fullPath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
    }

}
