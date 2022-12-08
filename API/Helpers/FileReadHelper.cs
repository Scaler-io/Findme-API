using Newtonsoft.Json;

namespace API.Helpers
{
    public class FileReadHelper<T> where T : class
    {
        public static List<T> SeederFileReader(string filename)
        {
            var data = File.ReadAllText($"./DataAccess/Seeders/{filename}.json");
            var jsonData = JsonConvert.DeserializeObject<List<T>>(data);
            return jsonData;
        }
    }
}
