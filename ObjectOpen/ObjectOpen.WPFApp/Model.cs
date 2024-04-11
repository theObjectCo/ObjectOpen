using System.IO;
using System.Reflection;
using System.Text.Json;

namespace ObjectOpen.WPFApp
{
    public static class Model
    {
        public static List<Employee> GetExistingEmployees()
        {
            string fileName = $"{typeof(App).Namespace}.ObjectEmployees.json";

            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName)
                ?? throw new InvalidOperationException($"{fileName} not found");

            using StreamReader reader = new StreamReader(stream);
            string jsonContent = reader.ReadToEnd();

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals
            };

            return JsonSerializer.Deserialize<List<Employee>>(jsonContent, options)
                ?? throw new InvalidOperationException($"Failed at deserialising {jsonContent}");
        }
    }
}
