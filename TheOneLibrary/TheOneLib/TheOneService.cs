using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using TheOneLib.TheOneDAL.TheOneDAO;

namespace TheOneLib
{
    public class TheOneService
    {

        HttpClient client;

        // For now this is located with the running exe (not with TheOneLib.dll)
        // Ideally, the apikey would be held in a secure data store of some sort.
        const String apiFile = "./apikey.txt";
        const String idRef = "{id}";
        const String baseUrl = "https://the-one-api.dev/v2";
        const String moviesUrl = baseUrl + "/movie";
        const String movieUrl = moviesUrl + "/" + idRef;
        const String movieQuoteUrl = movieUrl + "/quote";
        List<String> acceptedAttributes = new List<String> { "limit", "page", "offset", "sort" };
        List<String> comparitors = new List<String> { "=", "!", ">", "<", ">=", "<=" };


        // Constructor
        public TheOneService() {
        }

        // Publicly available functions.


        // Get Movies and return the full container without Params.
        public TheOneContainer<Movie> getMovies()
        {
            return getMovies(new Dictionary<string, string>());
        }

        // Get Movies and return the full container with urlParams.
        public TheOneContainer<Movie> getMovies(Dictionary<String, String> urlParams) {
            // Call the API & return results.
            return Task.Run(() => callApi<TheOneContainer<Movie>,Movie>(moviesUrl, new Dictionary<string, string>(), urlParams)).Result;
        }

        // Get Movie Without Params.
        public Movie getMovie(String movieId)
        {
            return getMovie(movieId, new Dictionary<string, string>());
        }

        // Get the individual Movie, only returing it (the container meta data is unnecesary).
        // The user runs the risk of not knowing why an error might have occured.
        public Movie getMovie(String movieId, Dictionary<String, String> urlParams) {

            Movie movie = new Movie();

            // Set the ID in the path params.
            Dictionary<String, String> pathParameters = new Dictionary<string, string>();
            pathParameters.Add(idRef, movieId);

            // Call the API
            TheOneContainer<Movie> movieContainer = Task.Run(() => callApi<TheOneContainer<Movie>,Movie>(movieUrl, pathParameters, urlParams)).Result;
            if(movieContainer.items != null & movieContainer.items.Count > 0)
            {
                // Probably unnecessary, but better to be safe than sorry.
                movie = movieContainer.items.Find(movie => movie.getId() == movieId);
            }

            // Return the movie
            return movie;
        }

        // Get Quotes Without Params.
        public TheOneContainer<Quote> getMovieQuotes(String movieId)
        {
            return getMovieQuotes(movieId, new Dictionary<string, string>());
        }

        public TheOneContainer<Quote> getMovieQuotes(String movieId, Dictionary<String, String> urlParams)
        {
            // Set the ID in the path params.
            Dictionary<String, String> pathParameters = new Dictionary<string, string>();
            pathParameters.Add(idRef, movieId);
            TheOneContainer<Quote> quoteContainer = Task.Run(() => callApi<TheOneContainer<Quote>,Quote>(movieQuoteUrl, pathParameters, urlParams)).Result;

            return quoteContainer;
        }

        // Private Functions

        // Setup the Http Client & add in any headers/settings.
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

        // Generically call the api.
        private async Task<T> callApi<T, U>(String url, Dictionary<String, String> pathParameters, Dictionary<String, String> urlParameters) where T : TheOneContainer<U> where U : TheOneInterface {

            // Setup vars & client.
            T result = (T)Activator.CreateInstance(typeof(T));
            String json = String.Empty;
            prepareClient();

            // Apply any path params (denoted by {param}).
            // TODO: The same should probably be done header/body params.
            foreach (KeyValuePair<String, String> kvp in pathParameters)
            {
                if (url.Contains(kvp.Key)) {
                    url = url.Replace(kvp.Key, kvp.Value);
                }
            }

            // Use reflection to get the properties of TheOne Object we are searching.
            List<String> propNames = new List<String>();
            propNames.AddRange(acceptedAttributes);
            Type type = typeof(U);
            foreach (PropertyInfo prop in type.GetProperties())
            {
                propNames.Add(prop.Name);
            }

            // If any of the properties sent in are in the approved list, or are object properties, allow them to move forward.
            // This is a pass-through solution to allow flexibility of the api within the service.
            String urlParamString = String.Empty;
            List<String> urlParams = new List<String>();
            foreach (KeyValuePair<String, String> kvp in urlParameters)
            {
                // We have a valid key
                if (propNames.Contains(kvp.Key) || propNames.Contains(kvp.Key.Replace("!","")))
                {
                    String kvpValue = kvp.Value;
                    // Set the default param setup
                    String symbol = "=";

                    // We don't have a comparitor (exists/not exists)
                    if (String.IsNullOrEmpty(kvpValue))
                    {
                        symbol = "";
                    }

                    // Check to see if there's a comparitor in the value
                    comparitors.ForEach(sym => {
                        if (kvpValue.StartsWith(sym))
                        {
                            symbol = sym;
                            kvpValue = kvpValue.Replace(symbol, "");
                            return;
                        }
                    });

                    // Add the result to our url params list.
                    urlParams.Add(kvp.Key + symbol + kvpValue);
                }
            }

            if (urlParams.Count > 0)
            {
                // Convert the list to a proper query string part
                urlParamString = "?" + String.Join("&", urlParams);
            }

            url += urlParamString;

            // Set the URI from the resulting url.
            Uri uri = new Uri(url);
            try
            {
                // Go get the info!
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    // We've got the info, let's deseralize it into a usable object set!
                    string content = await response.Content.ReadAsStringAsync();
                    result = JsonSerializer.Deserialize<T>(content);
                }
                else
                {
                    // Respond with the failure result.
                    result.error = true;
                    result.errorMessage = response.ReasonPhrase;
                }

            }
            catch (Exception ex)
            {
                // Oh no!  Something went wrong.
                Debug.WriteLine("Error retrieving data from The One Api.", ex);

                // For now signal an error & report the exception.
                result.error = true;
                result.errorMessage = ex.Message;
            }

            // Let's return either our results!
            return result;
        }

    }
}
