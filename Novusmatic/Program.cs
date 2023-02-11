using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Novusmatic
{
    static class Program
    {

        public static int buscado = 0;
        public static int actualiza = 0;
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmNovusmatic());
        }
    }
}
