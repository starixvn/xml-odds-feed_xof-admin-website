namespace Fanex.OddsFeed.Common.DTO
{
    using System.Xml.Serialization;

    [XmlRoot()]
    public class Error
    {
        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlAttribute(AttributeName = "message")]
        public string Message { get; set; }
    }
}
