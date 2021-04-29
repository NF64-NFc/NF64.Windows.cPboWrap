using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;


namespace NF64.Windows.cPboWrap.Propertiesa
{
    internal static class Application
    {
        private static Assembly CurrentAssembly { get; } = Assembly.GetEntryAssembly();


        internal static void Main(string[] args)
        {
            try
            {
                var p = new CPboParameter(args);
                var cmd = p.GetProcessStartInfo();
                Process.Start(cmd);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, CurrentAssembly.Location, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
