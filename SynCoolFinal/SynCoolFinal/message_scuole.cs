using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SynCoolFinal
{	
	// using System.Xml.Serialization;
	// XmlSerializer serializer = new XmlSerializer(typeof(Root));
	// using (StringReader reader = new StringReader(xml))
	// {
	//    var test = (Root)serializer.Deserialize(reader);
	// }

	[XmlRoot(ElementName = "scuola")]
	public class Scuola
	{

		[XmlElement(ElementName = "ID")]
		public int ID { get; set; }

		[XmlElement(ElementName = "Nome")]
		public string Nome { get; set; }

		[XmlElement(ElementName = "Citta")]
		public string Citta { get; set; }

		[XmlElement(ElementName = "Descrizione")]
		public string Descrizione { get; set; }

		[XmlElement(ElementName = "CF")]
		public double CF { get; set; }
	}

	[XmlRoot(ElementName = "scuole")]
	public class Scuole
	{

		[XmlElement(ElementName = "scuola")]
		public List<Scuola> Scuola { get; set; }
	}

	[XmlRoot(ElementName = "root")]
	public class Root
	{

		[XmlElement(ElementName = "Success")]
		public bool Success { get; set; }

		[XmlElement(ElementName = "scuole")]
		public Scuole Scuole { get; set; }
	}


}
