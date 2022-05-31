using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SynCoolFinal
{
	[XmlRoot(ElementName = "root")]
	public class message_user:message.result
    {
		[XmlElement(ElementName = "Success")]
		public bool Success { get; set; }

		[XmlElement(ElementName = "user")]
		public User user { get; set; }

		[XmlRoot(ElementName = "user")]
		public class User
		{

			[XmlElement(ElementName = "pic")]
			public string Pic { get; set; }

			[XmlElement(ElementName = "nome")]
			public string Nome { get; set; }

			[XmlElement(ElementName = "cognome")]
			public string Cognome { get; set; }

			[XmlElement(ElementName = "email")]
			public string Email { get; set; }

			[XmlElement(ElementName = "dataNascita")]
			public DateTime DataNascita { get; set; }

			[XmlElement(ElementName = "descrizione")]
			public string Descrizione { get; set; }

			[XmlElement(ElementName = "isTutor")]
			public int IsTutor { get; set; }

			[XmlElement(ElementName = "citta")]
			public string Citta { get; set; }

			[XmlElement(ElementName = "username")]
			public string Username { get; set; }
		}
	}
}
