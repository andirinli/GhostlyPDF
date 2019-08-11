using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GhostlyPDF
{
    public static class GhostScriptApi
    {
        #region Private Fields
        private static object lockObj = new object();
        #endregion
        #region Constants
        const string Dll = "gsdll32.dll";
        #endregion
        #region Static Constructor
        static GhostScriptApi()
        {
            var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var requiredDll = Path.Combine(dir, Dll);

            if (!File.Exists(requiredDll))
                lock (lockObj)
                {
                    if (!File.Exists(requiredDll))
                    {
                        try
                        {
                            File.WriteAllBytes(requiredDll, Properties.Resources.gsdll32);
                        }
                        catch (Exception)
                        {
                            string ex = string.Format("File not found in path {0}", requiredDll);
                            throw new FileNotFoundException(ex);
                        }
                    }
                }

        }
        #endregion
        #region Native Imports
        [DllImport(Dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int gsapi_revision(ref sGSVersion version, int len);

        [DllImport(Dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int gsapi_new_instance(ref System.IntPtr pinstance, System.IntPtr handle);

        [DllImport(Dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int gsapi_init_with_args(IntPtr pInstance, int argc, [In, Out] string[] argv);

        [DllImport(Dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int gsapi_exit(IntPtr instance);

        [DllImport(Dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void gsapi_delete_instance(System.IntPtr pinstance);
        #endregion
        #region Methods
        public static sGhostScriptVersion GetVersion()
        {
            sGSVersion ver = new sGSVersion();
            var code = gsapi_revision(ref ver, Marshal.SizeOf(ver));
            if (code >= 0)
                return new sGhostScriptVersion(ver);
            return null;
        }

        public static int RunCommand(string[] args)
        {
            IntPtr instance = IntPtr.Zero;
            IntPtr handle = IntPtr.Zero;
            int code = gsapi_new_instance(ref instance, handle);
            if (code != 0) return code;
            code = gsapi_init_with_args(instance, args.Length, args);
            gsapi_exit(instance);
            gsapi_delete_instance(instance);
            return code;
        }
        #endregion
    }
}
