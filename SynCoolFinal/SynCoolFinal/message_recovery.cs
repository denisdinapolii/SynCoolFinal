using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SynCoolFinal
{
	[XmlRoot(ElementName = "root")]
	public class recovery
	{

		[XmlElement(ElementName = "Success")]
		public bool Success { get; set; }

		[XmlElement(ElementName = "Messaggio")]
		public string Messaggio { get; set; }
	}

}
