using System;
using System.Xml.Serialization;

namespace Elisi02.Models
{
    [XmlType(TypeName = "state")]
    public class PumpState
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
        [XmlElement(ElementName = "voltageIsAvailable")]
        public bool VoltageIsAvailable { get; set; }
        [XmlElement(ElementName = "pressureIsAvailable")]
        public bool PressureIsAvailable { get; set; }
        [XmlElement(ElementName = "statusIsOn")]
        public bool StatusIsOn { get; set; }
        [XmlElement(ElementName = "timestamp")]
        public string TimeStamp { get; set; }
    }
}
