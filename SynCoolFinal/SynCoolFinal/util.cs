using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SynCoolFinal
{
    public class util
    {
        public static message.result xmlDeserialization(Type type, string xml) 
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (StringReader reader = new StringReader(xml))
            {
                message.result test = (message.result)serializer.Deserialize(reader);
                return test;
            }
            
        }
    }
}
