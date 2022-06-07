using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SynCoolFinal
{
	[XmlRoot(ElementName = "root")]
	public class message_materie:message.result
    {
		[XmlElement(ElementName = "Success")]
		new public bool Success { get; set; }

		[XmlElement(ElementName = "materie")]
		public Materie materie { get; set; }

		[XmlRoot(ElementName = "materia")]
		public class Materia
		{

			[XmlElement(ElementName = "ID")]
			public int ID { get; set; }

			[XmlElement(ElementName = "Nome")]
			public string Nome { get; set; }

			public Materia() { }
			public Materia(int ID, string nome)
			{
				this.ID = ID;
				this.Nome = nome;
			}
		}

		[XmlRoot(ElementName = "materie")]
		public class Materie
		{
			[XmlElement(ElementName = "materia")]
			public List<Materia> Materia { get; set; }
		}

		

	}
}
