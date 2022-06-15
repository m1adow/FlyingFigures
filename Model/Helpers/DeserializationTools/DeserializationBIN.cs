using FlyingFigures.Model.Figures;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FlyingFigures.Model.Helpers.DeserializationTools
{
    public class DeserializationBIN
    {
        public static void Deserialize(out List<Figure> figures, string path)
        {
            figures = new List<Figure>();

            using (var stream = File.OpenRead(path))
            {
                var binaryFormatter = new BinaryFormatter();

                figures = (List<Figure>)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
