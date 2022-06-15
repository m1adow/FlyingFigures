using FlyingFigures.Model.Figures;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FlyingFigures.Model.Helpers.SerializationTools
{
    public class SerializationBIN
    {
        public static void Serialize(List<Figure> figures, string path)
        {
            using (var stream = File.Open(path, FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();

                binaryFormatter.Serialize(stream, figures);
            }
        }
    }
}
