using System;
using TheOneLib;

/*
    A simple .net console program to use TheOneService to obtain information on
    The Lord Of The Rings movies!
*/
class Program
{
    // Main for the console application.
    static void Main(string[] args)
    {
        // Instantiate TheOneService
        TheOneService theOneService = new TheOneService();
        // Get All Movies
        MovieContainer movieContainer = theOneService.getMovies();

        Console.WriteLine("-------------------------");
        Console.WriteLine("Movie Info Available For:");
        // Output Movies if available.
        if (movieContainer != null && movieContainer.movies != null) {
            foreach(Movie movie in movieContainer.movies) {
                Console.WriteLine(movie.name);
            }
        }
        Console.WriteLine("-------------------------");
    }
}

