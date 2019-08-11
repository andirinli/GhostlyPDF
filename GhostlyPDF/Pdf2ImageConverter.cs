using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GhostlyPDF
{
    /// <summary>
    /// https://www.opentechguides.com/how-to/article/tools/42/pdf-to-pnf.html
    /// https://www.biu.ac.il/os_site/documentation/gs/Use.htm
    /// https://bitbucket.org/brightertools/ghostscript/src/master/GhostScript.cs
    /// </summary>
    public class Pdf2ImageConverter : IDisposable
    {
        #region Private Fields
        private List<string> args = new List<string>();
        private bool _outputTypeSelected = false;
        private eOutputType _outputType = eOutputType.png256;
        private bool _outputResolutionSelected = false;
        private int _outputResolution = (int)eResolution.Standart;
        private int _startPage = 0;
        private int _endPage = 0;

        private string _outputPath = "";
        private string _outputTemplate = "";
        #endregion
        #region Constructor
        public Pdf2ImageConverter(string pdfFilePath)
        {
            if (!File.Exists(pdfFilePath))
                throw new FileNotFoundException();

            Guid = Guid.NewGuid();
            FilePath = pdfFilePath;

            init();
        }

        private void init()
        {
            #region Pdf Page Count Calculation
            using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            using (var sr = new StreamReader(fs))
            {
                var pdfStr = sr.ReadToEnd();
                Regex r = new Regex(@"/Type\s*/Page[^s]");
                var m = r.Matches(pdfStr);
                PdfPageCount = m.Count;
            }
            #endregion
            #region Default Parameters
            resetArgs();
            #endregion
        }

        #endregion
        #region Properties
        public Guid Guid { get; private set; }
        public string FilePath { get; }
        public int PdfPageCount { get; private set; }
        #endregion
        #region Converter Parameters
        public Pdf2ImageConverter SelectOutputType(eOutputType outputType)
        {
            _outputType = outputType;
            _outputTypeSelected = true;
            return this;
        }
        public Pdf2ImageConverter SelectResolution(eResolution resolution)
        {
            _outputResolution = (int)resolution;
            _outputResolutionSelected = true;
            return this;
        }
        public Pdf2ImageConverter SelectPageRange(int startPage, int endPage)
        {
            if (startPage <= 0 || endPage < startPage || endPage > PdfPageCount)
                throw new ArgumentOutOfRangeException("Please choose carrefuly startPage and endPage values..");

            _startPage = startPage;
            _endPage = endPage;

            return this;
        }
        public Pdf2ImageConverter SelectOutputPath(string path)
        {
            if (!string.IsNullOrEmpty(_outputPath))
                throw new Exception("you Already selected output path");

            if (!path.IsDirectory())
                throw new DirectoryNotFoundException("path is not directory or directory not exist");

            if (path.Equals("."))
                path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            else if (path.StartsWith(@".\"))
            {
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), path.Replace(".\\", ""));
            }


            _outputPath = path;

            return this;
        }

        /// <summary>
        /// Set OutputTemplate for convert process..
        /// </summary>
        /// <param name="template">
        /// %pX : Page Number and padding zero count.
        /// Example : Image_%p2 => Image_001
        /// 
        /// PS: Dont use extension.....
        /// </param>
        /// <returns></returns>
        public Pdf2ImageConverter SetOutputFileTemplate(string template)
        {
            if (!string.IsNullOrEmpty(_outputTemplate))
                throw new Exception("You already set output file template");
            _outputTemplate = template;
            return this;
        }

        #endregion
        #region Private Methods
        private void resetArgs()
        {
            args.Clear();

            args.Add("-q");
            args.Add("-dNOPAUSE");
            args.Add("-dBATCH");
            args.Add("-dSAFER");
            args.Add("-dPDFFitPage");
            if (_outputTypeSelected)
                args.Add(string.Format("-sDEVICE={0}", _outputType));    
            if(_outputResolutionSelected)
                args.Add(string.Format("-r{0}", _outputResolution));

            if(_startPage > 0 && _endPage > 0)
            {
                args.Add(string.Format("-dFirstPage={0}", _startPage));
                args.Add(string.Format("-dLastPage={0}", _endPage));
            }
        }
        private string tempFileTemplate()
        {
            return string.Format("{0}_%09d{1}", Guid.ToString(), _outputType.GetFileExtension());
        }
        private string[] getConvertedTempFiles()
        {
            return Directory.GetFiles(Path.GetTempPath(), tempFileTemplate().Replace("%09d", "*"));
        }
        private void convert()
        {
            if (!_outputTypeSelected)
                SelectOutputType(eOutputType.png256);
            if (!_outputResolutionSelected)
                SelectResolution(eResolution.Standart);

            var outPath = Path.Combine(Path.GetTempPath(), tempFileTemplate());

            args.Add(string.Format("-sOutputFile={0}", outPath));
            args.Add(FilePath);
            int cmdResult = GhostScriptApi.RunCommand(args.ToArray());
            if (cmdResult != 0)
                throw new Exception("Error on Convert");
        }
        #endregion

        #region Exports
        public void SaveAsFiles()
        {
            resetArgs();
            convert();

            if (_startPage == 0 && _endPage == 0)
            {
                _startPage = 1;
                _endPage = PdfPageCount;
            }

            int pageNumber = _startPage;
            foreach (var f in getConvertedTempFiles())
            {
                var targetFileName = string.Format("{0}{1}", _outputTemplate.GetTemplateFileName(pageNumber), _outputType.GetFileExtension());
                var target = Path.Combine(_outputPath, targetFileName);
                File.Copy(f, target);
                pageNumber++;
            }
        }
        public List<Bitmap> GetBitmaps()
        {
            resetArgs();
            convert();
            var result = new List<Bitmap>();
            foreach (var f in getConvertedTempFiles())
            {
                var bmp = new Bitmap(Image.FromFile(f))
                {
                    Tag = "GhostlyPDF Image"
                };
                result.Add(bmp);
            }
            return result;
        }
        #endregion
        #region Dispose
        public void Dispose()
        {
            ///Just for cleaning
            foreach (var f in getConvertedTempFiles())
            {
                try
                {
                    File.Delete(f);
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion
    }
}

