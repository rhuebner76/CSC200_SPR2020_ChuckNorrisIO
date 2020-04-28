using System.Linq;

namespace ChuckNorrisIO.Wpf.Services
{
    public class ChuckNorrisJokeClient
    {
        // return joke categories
        public System.Collections.Generic.IEnumerable<string> Categories()
        {
            string content = GetContent("https://api.chucknorris.io/jokes/categories");
            string[] result = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(content);

            return result;
        }

        // return random joke
        public System.Collections.Generic.IEnumerable<string> Random()
        {
            string content = GetContent("https://api.chucknorris.io/jokes/random");
            Joke result = Newtonsoft.Json.JsonConvert.DeserializeObject<Joke>(content);

            return new string[] { result.Value };
        }

        public System.Collections.Generic.IEnumerable<string> Random(string category)
        {
            string content = GetContent($"https://api.chucknorris.io/jokes/random?category={category}");
            Joke result = Newtonsoft.Json.JsonConvert.DeserializeObject<Joke>(content);

            return new string[] { result.Value };
        }

        public System.Collections.Generic.IEnumerable<string> Search(string query)
        {
            string content = GetContent($"https://api.chucknorris.io/jokes/search?query={query}");
            JokeCollection result = Newtonsoft.Json.JsonConvert.DeserializeObject<JokeCollection>(content);

            return result.Result.Select(item => item.Value);
        }

        private string GetContent(string uri)
        {
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response = httpClient.GetAsync(uri).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            httpClient.Dispose();

            return content;
        }

        private class JokeCollection
        {
            public int Total { get; set; }
            public Joke[] Result { get; set; }
        }

        private class Joke
        {
            public string[] Categories { get; set; }
            public string Value { get; set; }
        }
    }
}