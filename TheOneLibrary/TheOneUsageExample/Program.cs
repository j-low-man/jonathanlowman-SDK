using System;
using System.Collections.Generic;
using TheOneLib;
using TheOneLib.TheOneDAL.TheOneDAO;

/*
    A simple .net console program to use TheOneService to obtain information on
    The Lord Of The Rings movies!
*/
class Program
{
    // Main for the console application.
    static void Main(string[] args)
    {
        String theTwoTowersId = String.Empty;

        // Instantiate TheOneService
        TheOneService theOneService = new TheOneService();
        // Get All Movies
        TheOneContainer<Movie> movieContainer = theOneService.getMovies();

        Console.WriteLine("-------------------------");
        Console.WriteLine("Movie Info Available For:");
        if (movieContainer.error)
        {

            Console.WriteLine(movieContainer.errorMessage);

        } 
        else
        {
            // Output Movies if available.
            // Traditional Brute-force... but works for our purposes at this time.
            if (movieContainer != null && movieContainer.items != null)
            {
                foreach (Movie movie in movieContainer.items)
                {
                    Console.WriteLine(movie.name);
                    if (movie.name.Trim().ToUpper().Equals("THE TWO TOWERS"))
                    {
                        theTwoTowersId = movie.getId();
                    }
                }
            }
        }
        Console.WriteLine("-------------------------");


        // Now that we have obtained all the Movies.
        // Let's get info on only one Movie.

        // We're going to pick The Two Towers, because it's an awesome movie.
        Console.WriteLine("-------------------------");
        Console.WriteLine("Individual Movie Info:");

        Movie tempMovie = theOneService.getMovie(theTwoTowersId);

        Console.WriteLine("Name:            " + tempMovie.name);
        Console.WriteLine("Runtime:         " + tempMovie.runtimeInMinutes);
        Console.WriteLine("Budget(M):       " + tempMovie.budgetInMillions);
        Console.WriteLine("Revenue(M):      " + tempMovie.boxOfficeRevenueInMillions);
        Console.WriteLine("Nominations(AA): " + tempMovie.academyAwardNominations);
        Console.WriteLine("Wins(AA):        " + tempMovie.academyAwardWins);
        Console.WriteLine("-------------------------");

        // Now that we have an awesome movie, let's get the quotes.

        TheOneContainer<Quote> quoteContainer = theOneService.getMovieQuotes(theTwoTowersId);

        Console.WriteLine("-------------------------");
        if (quoteContainer.error)
        {
            Console.WriteLine(quoteContainer.errorMessage);
        }
        else
        {
            Console.WriteLine("Quote Count:     " + quoteContainer.items.Count);
        }
        Console.WriteLine("-------------------------");


        // That's a lot of quotes, let's page that.

        Console.WriteLine("-------------------------");
        Dictionary<String, String> urlParams = new Dictionary<String, String>();
        urlParams.Add("limit", "10");

        quoteContainer = theOneService.getMovieQuotes(theTwoTowersId, urlParams);

        if (quoteContainer.error)
        {
            Console.WriteLine(quoteContainer.errorMessage);
        }
        else
        {
            foreach (Quote quote in quoteContainer.items)
            {
                // TODO: At some point, would be good to get the character from the api & display.
                // This would be a larger discussion of things like caching as only the id's come back.
                Console.WriteLine("Quote:");
                Console.WriteLine(quote.dialog);
                Console.WriteLine("=================");
            }

        }
        Console.WriteLine("-------------------------");


        // Now Lets use a Filter!
        urlParams.Clear();
        urlParams.Add("name", "");
        urlParams.Add("sort", "name:asc");
        urlParams.Add("budgetInMillions", "<100");


        movieContainer = theOneService.getMovies(urlParams);

        if (movieContainer.error)
        {
            Console.WriteLine(movieContainer.errorMessage);
        }
        else
        {
            movieContainer.items.ForEach(tmpMovie =>
            {
                Console.WriteLine("Name:            " + tmpMovie.name);
                Console.WriteLine("Runtime:         " + tmpMovie.runtimeInMinutes);
                Console.WriteLine("Budget(M):       " + tmpMovie.budgetInMillions);
                Console.WriteLine("Revenue(M):      " + tmpMovie.boxOfficeRevenueInMillions);
                Console.WriteLine("Nominations(AA): " + tmpMovie.academyAwardNominations);
                Console.WriteLine("Wins(AA):        " + tmpMovie.academyAwardWins);
                Console.WriteLine("=================");
            });
        }

        Console.WriteLine("-------------------------");

    }
}

