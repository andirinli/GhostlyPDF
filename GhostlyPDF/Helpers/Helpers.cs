using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostlyPDF
{
    public static class Helpers
    {
        public static bool IsDirectory(this string path)
        {
            try
            {
                FileAttributes attributes = File.GetAttributes(path);
                if (attributes.HasFlag(FileAttributes.Directory))
                    return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public static string GetFileExtension(this eOutputType type)
        {
            switch (type)
            {
                case eOutputType.pngalpha:
                case eOutputType.png16m:
                case eOutputType.png256:
                case eOutputType.png16:
                case eOutputType.pnggray:
                case eOutputType.pngmono:
                    return ".png";

                case eOutputType.jpeg:
                case eOutputType.jpeggray:
                    return ".jpg";

                case eOutputType.bmp32b:
                case eOutputType.bmp16m:
                case eOutputType.bmp256:
                case eOutputType.bmp16:
                case eOutputType.bmpsep8:
                case eOutputType.bmpgray:
                case eOutputType.bmpmono:
                    return ".bmp";

                default:
                    throw new NotSupportedException("Not supported output type");
            }
        }


        public static string GetTemplateFileName(this string template, int pageNumber)
        {
            if (string.IsNullOrEmpty(template))
                template = "Page_%p4";

            int paddingCount = 0;
            string result = template;

            var pIndex = template.IndexOf("%p");
            var pageStr = template.Substring(pIndex, 3);
            if (int.TryParse(pageStr.Substring(2), out paddingCount))
            {
                result = result.Replace(pageStr, string.Format("{0}", pageNumber.ToString().PadLeft(paddingCount, '0')));
            }
            else
            {
                result = result.Replace("%p", string.Format("{0}", pageNumber));
            }
            return result;
        }

        public static List<string> GetTempFiles()
        {
            return null;
        }

    }
}
