using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostlyPDF
{
    public enum eOutputType
    {
        /// <summary>
        /// PNG, Portable Network Graphics format, 
        /// 32-bit RGBA color with transparency 
        /// indicating pixel coverage.
        /// </summary>
        pngalpha,
        /// <summary>
        /// PNG, Portable Network Graphics format, 
        /// 24-bit RGB color.
        /// </summary>
        png16m,
        /// <summary>
        /// PNG, Portable Network Graphics format, 8-bit color.
        /// </summary>
        png256,
        /// <summary>
        /// PNG, Portable Network Graphics format, 4-bit color.
        /// </summary>
        png16,
        /// <summary>
        /// PNG, Portable Network Graphics format, grayscale.
        /// </summary>
        pnggray,
        /// <summary>
        /// PNG, Portable Network Graphics format, black-and-white.
        /// </summary>
        pngmono,

        /// <summary>
        /// JPEG File Interchange Format.
        /// </summary>
        jpeg,
        /// <summary>
        /// Grayscale JPEG File Interchange Format.
        /// </summary>
        jpeggray,

        /// <summary>
        /// BMP, MS Windows bitmap.
        /// </summary>
        bmp32b,
        /// <summary>
        /// BMP, MS Windows bitmap.
        /// </summary>
        bmp16m,
        /// <summary>
        /// BMP, MS Windows bitmap.
        /// </summary>
        bmp256,
        /// <summary>
        /// BMP, MS Windows bitmap.
        /// </summary>
        bmp16,
        /// <summary>
        /// BMP, MS Windows bitmap.
        /// </summary>
        bmpsep8,
        /// <summary>
        /// BMP, MS Windows bitmap.
        /// </summary>
        bmpgray,
        /// <summary>
        /// BMP, MS Windows bitmap.
        /// </summary>
        bmpmono,

    }
}
