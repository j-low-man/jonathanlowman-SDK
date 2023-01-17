using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TheOneLib
{
    public class TheOneService
    {
        HttpClient client;
        const String apiFile = "./apikey.txt";
        const String idRef = "{id}";
        const String baseUrl = "https://the-one-api.dev/v2";
        const String moviesUrl = baseUrl + "/movie";
        const String movieUrl = moviesUrl + "/" + idRef;
        const String movieQuoteUrl = movieUrl + "/quote";

        public TheOneService() {
        }

        public MovieContainer getMovies() {
            return Task.Run(() => callApi<MovieContainer>(moviesUrl, new Dictionary<string, string>())).Result;
        }

        public Movie getMovie(String id) {
            Dictionary<String, String> parameters = new Dictionary<string, string>();
            parameters.Add(idRef, id);
            return Task.Run(() => callApi<Movie>(moviesUrl, parameters)).Result;
        }

        private void prepareClient() {
            client = new HttpClient();
            String apiKey = File.ReadAllText(apiFile);
            if(!String.IsNullOrEmpty(apiKey)) {
                if(!apiKey.StartsWith("Bearer ")) {
                    apiKey = "Bearer " + apiKey;
                }
                client.DefaultRequestHeaders.Add("Authorization", apiKey);
            }
        }

        private async Task<T> callApi<T>(String url, Dictionary<String, String> urlparameters) {

            T result = default(T);
            String json = String.Empty;
            prepareClient();

            foreach(KeyValuePair<String,String> kvp in urlparameters)
            {
                if(url.Contains(kvp.Key)) {
                    url = url.Replace(kvp.Key, kvp.Value);
                }
            }

            Uri uri = new Uri(url);
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    result = JsonSerializer.Deserialize<T>(content);
                }

            }
            catch (Exception ex)
            {
                String message = "Error retrieving data from The One Api.";
                Debug.WriteLine(message, ex);
            }

            return result;
        }

    }
}
