using FlyingFigures.Model.Figures;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FlyingFigures.Model.Helpers.DeserializationTools
{
    public class DeserializationJSON
    {
        public static void Deserialize(out List<Figure> figures, string path)
        {
            figures = new List<Figure>();

            using (var reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();

                figures = JsonSerializer.Deserialize<List<Figure>>(json);
            }
        }
    }
}
