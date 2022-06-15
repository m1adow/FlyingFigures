using FlyingFigures.Model.Figures;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FlyingFigures.Model.Helpers.DeserializationTools
{
    public class DeserializationXML
    {
        public static void Deserialize(out List<Figure> figures, string path)
        {
            figures = new List<Figure>();

            XmlSerializer serializer = new(typeof(List<Figure>));

            using (var reader = new StreamReader(path))
                figures = (List<Figure>)serializer.Deserialize(reader);
        }
    }
}
