using System;
using System.Xml.Serialization;

namespace Elisi02.Models
{
    [XmlType(TypeName = "command")]
    public class Command
    {
        [XmlIgnore]
        public DateTime TS
        {
            get
            {
                DateTime res;
                if (DateTime.TryParse(TimeStamp, out res))
                    return res;
                else
                    throw new InvalidOperationException("Неверный формат даты");
            }
            set
            {
                TimeStamp = value.ToString("dd.MM.yyyy HH:mm:ss");
            }
        }
        [XmlElement(ElementName = "value")]
        public CommandType Value { get; set; }
        [XmlElement(ElementName = "timestamp")]
        public string TimeStamp { get; set; }
    }

    public enum CommandType
    {
        On,
        Off
    }
}
