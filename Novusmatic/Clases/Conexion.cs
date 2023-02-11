
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Novusmatic.Clases
{
    class Conexion
    {
        public static String NuevaConexion()
        {
            String connstring;
            Configuration conf = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            ConnectionStringsSection css = conf.ConnectionStrings;
            return connstring = css.ConnectionStrings["Novusmatic.Properties.Settings.ConnectionString"].ConnectionString;
        }
    }
}
