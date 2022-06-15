using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FlyingFigures.Model.Helpers.SerializationTools
{
    public class SerializationJSON
    {
        public static void Serialize(List<Figure> figures, string path)
        {
            string json = JsonSerializer.Serialize((object)figures, new JsonSerializerOptions()
            {
                WriteIndented = true,
            });

            using (var writer = new StreamWriter(path))
                writer.Write(json);
        }
    }
}
