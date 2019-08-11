# GhostlyPDF
PDF to Image Converter for .NET



Example Usage : 

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

