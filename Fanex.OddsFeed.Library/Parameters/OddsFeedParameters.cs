namespace Fanex.OddsFeed.Library.Parameters
{
    public class OddsFeedParameters
    {
        private bool _outRights = true;

        public string UserName { get; set; }

        public string Timestamp { get; set; }

        public string Domain { get; set; }

        public OddsFeedFilter Filter { get; set; }

        public bool IsSample { get; set; }

        public bool Outrights
        {
            get
            {
                return _outRights;
            }
            set
            {
                _outRights = value;
            }
        }
    }
}
