namespace PetGroomingApp.Data.Seeding.Utilities
{
    using System.Text.Json;

    public static class JsonSeederHelper
    {
        private static readonly string basePath = "C:\\Users\\Dilyana\\Documents\\GitHub\\PetGroomingApp\\PetGroomingApp.Data\\Seeding\\Input\\";
        public static List<T> LoadSeedData<T>(string relativePath)
        {
            var fullPath = Path.Combine(basePath, relativePath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"JSON file not found: {fullPath}");

            var json = File.ReadAllText(fullPath);
            return JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<T>();
        }
    }

}
