using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GhostlyPDF
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct sGSVersion
    {
        public IntPtr product;
        public IntPtr copyright;
        public int revision;
        public int revisionDate;
    }

    public sealed class sGhostScriptVersion
    {
        #region Constructor
        internal sGhostScriptVersion(sGSVersion version)
        {
            Product = Marshal.PtrToStringAnsi(version.product);
            Copyright = Marshal.PtrToStringAnsi(version.copyright);
            Revision = version.revision;
            RevisionDate = version.revisionDate;
        }
        #endregion
        #region Properties
        public string Product { get; set; }
        public string Copyright { get; set; }
        public int Revision { get; set; }
        public int RevisionDate { get; set; }
        #endregion
    }
}
