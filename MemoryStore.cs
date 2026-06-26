using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CybersecurityChatbott
{
    public class MemoryStore
    {
        public string UserName { get; set; }

        public string FavouriteTopic { get; set; }

        private Dictionary<string, string> _memory =
            new Dictionary<string, string>();


        public void Store(string key, string value)
        {
            _memory[key] = value;
        }

        public string Recall(string key)
        {
            if (_memory.ContainsKey(key))
            {
                return _memory[key];
            }

            return "";
        }

        public string GetPersonalisedOpener()
        {
            if (!string.IsNullOrEmpty(FavouriteTopic))
            {
                return "As someone interested in "
                    + FavouriteTopic + ", ";
            }

            return "";
        }

        // Persist memory to a JSON file
        public void SaveToFile(string path)
        {
            var dto = new MemoryDto
            {
                UserName = this.UserName,
                FavouriteTopic = this.FavouriteTopic,
                KeyValues = this._memory
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(dto, options);

            File.WriteAllText(path, json);
        }

        public bool LoadFromFile(string path)
        {
            if (!File.Exists(path))
                return false;

            var json = File.ReadAllText(path);
            try
            {
                var dto = JsonSerializer.Deserialize<MemoryDto>(json);
                if (dto != null)
                {
                    this.UserName = dto.UserName;
                    this.FavouriteTopic = dto.FavouriteTopic;
                    this._memory = dto.KeyValues ?? new Dictionary<string, string>();
                    return true;
                }
            }
            catch
            {
                // ignore errors and return false
            }

            return false;
        }

        private class MemoryDto
        {
            public string UserName { get; set; }
            public string FavouriteTopic { get; set; }
            public Dictionary<string, string> KeyValues { get; set; }
        }
    }
}