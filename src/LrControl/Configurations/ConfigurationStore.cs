using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LrControl.Configurations.Models;
using Newtonsoft.Json;

namespace LrControl.Configurations
{
    public interface IConfigurationStore
    {
        Task<ConfigurationModel> LoadAsync(Stream stream);
        Task<Stream> SaveAsync(ConfigurationModel configuration);
    }
    
    public class ConfigurationStore : IConfigurationStore
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter>
            {
                new ControllerIdModelConverter()
            }
        };

        public async Task<ConfigurationModel> LoadAsync(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<ConfigurationModel>(json, JsonSerializerSettings);
            }
        }

        public Task<Stream> SaveAsync(ConfigurationModel configuration)
        {
            var json = JsonConvert.SerializeObject(configuration, JsonSerializerSettings);
            return Task.FromResult<Stream>(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }
    }
}