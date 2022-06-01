using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SynCoolFinal
{
	[XmlRoot(ElementName = "root")]
	public class message_base: message.result
	{

		[XmlElement(ElementName = "Success")]
		new public bool Success { get; set; }

		[XmlElement(ElementName = "Messaggio")]
		public string Messaggio { get; set; }
	}

}
