# jonathanlowman-SDK
## C# .Net Library SDK for The One API.

### Description

This is a SDK Library built in C# for .Net DLL consumption (or via NuGet).  The SDK will allow you to connect to and easily return objects for The One API.

#### Example Usage

        // Instantiate TheOneService
        TheOneService theOneService = new TheOneService();
        // Get All Movies
        MovieContainer movieContainer = theOneService.getMovies();

### How To Import

    This library can either be built and imported as a DLL to a .Net project, or obtained from NuGet located here:

    https://www.nuget.org/packages/TheOneLib/1.0.0

### Objects Returned

The main object that will be returned is TheOneContainer<>.  This object correlates to the returned meta data from The One API providing the following:

        int total
        int limit
        int offset
        int page
        int pages
        List<T> items
        Boolean error
        String errorMessage

If an error occurs during the retrieval of data, error will be marked as *true* and an error message will be provided.

The List of *items* will all provide a basic getId() and each will provide their own data that will correlate to The One API data.

Movie:

        String name
        int runtimeInMinutes
        Double budgetInMillions
        Double boxOfficeRevenueInMillions
        int academyAwardNominations
        int academyAwardWins
        Double rottenTomatoesScore

Quote:

        String dialog
        String movie_id
        String character_id


The publicly available functions to get a single Movie are slightly different and will simply return a singular Movie object (or empty Movie object if not found or an error occurs).  This is to simplify the return structure for a single Movie by id request.


The publicly available functions that can be used are as follows:

* TheOneContainer<Movie> getMovies()
* TheOneContainer<Movie> getMovies(Dictionary<String, String> urlParams)
* Movie getMovie(String movieId)
* Movie getMovie(String movieId, Dictionary<String, String> urlParams)
* TheOneContainer<Quote> getMovieQuotes(String movieId)
* TheOneContainer<Quote> getMovieQuotes(String movieId, Dictionary<String, String> urlParams)

In order to utilize params, create a Dictionary<String,String> and add in any key value pairs to pass along.  Any recognized items will be passed along to The One API.  Here are some examples:

Limit:
    * key = "limit", value = "100"

Page
    * key = "page", value = "2"

Offset
    * key = "offset", value = "3"

Sort:
    * key = "sort", value = "name:asc"

Exists:
    * key = "name", value = ""

Not Exists:
    * key = "!name", value = ""

Less Than, Greater Than, Equal
    * key = "budgetInMillions", value = "<100"
    * key = "academyAwardWins", value = ">0"
    * key = "name", value = "The Two Towers"