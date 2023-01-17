using System;

namespace TheOneLib
{
    [Serializable]
    public class Movie
    {
        public String _id { get; set; }
        public String name { get; set; }
        public int runtimeInMinutes { get; set; }
        public Double budgetInMillions { get; set; }
        public Double boxOfficeRevenueInMillions { get; set; }
        public int academyAwardNominations { get; set; }
        public int academyAwardWins { get; set; }
        public Double rottenTomatoesScore { get; set; }

    }
}