namespace Fanex.OddsFeed.WebServiceLibrary
{
    using Common.DTO;

    public static class ErrorBuilder
    {
        public static Error GetUnAuthenticated()
        {
            return new Error
            {
                Id = 1,
                Message = "You are not authorized to take this action"
            };
        }

        public static Error GetParameterSufficent()
        {
            return new Error
            {
                Id = 2,
                Message = "Missing parameter!"
            };
        }

        public static Error GetWrongUrlFormat()
        {
            return new Error
            {
                Id = 3,
                Message = "Invalid URL"
            };
        }

        public static Error GetUMDownTime(string time)
        {
            return new Error
            {
                Id = 4,
                Message = string.Format("We are having server maintenance, website will be unavailable until {0} (HK Time GMT +8). Sorry for the inconveniences caused!", time)
            };
        }
    }
}
