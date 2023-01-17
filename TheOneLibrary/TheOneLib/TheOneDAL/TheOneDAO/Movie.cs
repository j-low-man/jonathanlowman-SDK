using System;

namespace TheOneLib.TheOneDAL.TheOneDAO
{
    [Serializable]
    public class Movie: TheOneBase
    {
        public String name { get; set; }
        public int runtimeInMinutes { get; set; }
        public Double budgetInMillions { get; set; }
        public Double boxOfficeRevenueInMillions { get; set; }
        public int academyAwardNominations { get; set; }
        public int academyAwardWins { get; set; }
        public Double rottenTomatoesScore { get; set; }

    }
}