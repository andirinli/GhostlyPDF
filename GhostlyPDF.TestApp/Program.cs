using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostlyPDF.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var file = Path.Combine(dir, "Example.pdf");

            using (var converter = new Pdf2ImageConverter(file))
            {
                var bitmaps = converter.SelectOutputType(eOutputType.bmp16m)
                                       .SelectPageRange(30, 35)
                                       .SelectResolution(eResolution.Medium)
                                       .GetBitmaps();

                converter.SelectOutputType(eOutputType.png16)
                         .SelectPageRange(1, 5)
                         .SelectResolution(eResolution.Standart)
                         .SelectOutputPath(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures))
                         .SaveAsFiles();

            }

            Console.WriteLine("Operation Complete");
            Console.Read();
        }
    }
}
