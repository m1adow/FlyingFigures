using FlyingFigures.Model.Figures;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FlyingFigures.Model.Helpers.SerializationTools
{
    public class SerializationXML
    {
        public static void Serialize(List<Figure> figures, string path)
        {
            XmlSerializer serializer = new(typeof(List<Figure>));

            using (var writer = new StreamWriter(path))
                serializer.Serialize(writer, figures);
        }
    }
}
