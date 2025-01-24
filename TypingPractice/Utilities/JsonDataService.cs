using System.Text.Json;
using TypingPractice.Models;

namespace TypingPractice.Utilities
{
    public class JsonDataService
    {
        private string keywordsFilepath;
        private StreamReader jsonFileReader;
        private List<Keywords> keywordsFromJsonFile;

        public JsonDataService(string filepath)
        {
            keywordsFilepath = filepath;
            jsonFileReader = new StreamReader(keywordsFilepath);
        }

        public void ParseJsonDataFromFile(string filepath)
        {
            using (jsonFileReader)
            {
                string keywordsJson = jsonFileReader.ReadToEnd();
                if (keywordsJson != null)
                {
                    keywordsFromJsonFile = JsonSerializer.Deserialize<List<Keywords>>(keywordsJson);
                }
            }
        }

        public List<Keywords> GetAllCategoriesAndKeywords()
        {
            List<Keywords> keywords = keywordsFromJsonFile.Select(k => new Keywords
            {
                category = k.category,
                keywords = k.keywords
            }).ToList(); ;

            return keywords;
        }

        public List<string> GetKeywordsByCategory(string category)
        {
            if (keywordsFromJsonFile == null || !keywordsFromJsonFile.Any())
            {
                throw new InvalidOperationException($"There exists no JSON data in the variable keywordsFromJsonFile. The value is: '{keywordsFromJsonFile}'");
            }
            
            var matchedCategory = keywordsFromJsonFile.FirstOrDefault(k => k.category.Equals(category, StringComparison.OrdinalIgnoreCase));

            if (matchedCategory == null)
            {
                throw new KeyNotFoundException($"The category '{category}' was not found in the parameter variable keywordsList");
            }

            return matchedCategory.keywords.ToList();
        }
    }
}
