using MySql.Data.MySqlClient;
using Novusmatic.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Novusmatic
{
    public partial class frmCatalogo : Form
    {
        private bool isNew=true;
        private string id;

        String Ubicacion = "";
        bool subimg = false;
        byte[] img;
        public frmCatalogo()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmCatalogo_Load(object sender, EventArgs e)
        {
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            frmBuscarMoneda buscarmoneda = new frmBuscarMoneda();
            Program.buscado = 0;
            pAnverso.Image = null;
            pReverso.Image = null;
            buscarmoneda.ShowDialog();

            buscarmoneda.Location = new Point(0, 0);
            if (Program.buscado!=0)
            {
                //cargar datos del idmoneda buscada
                //MessageBox.Show("Se cargaran datos de moneda seleccionada");
                llenarDatos(Program.buscado);
                //bloquea
                txtCodigo.Enabled = false;
                txtAno.Enabled = false;
                txtCeca.Enabled = false;
                txtComent.Enabled = false;
                txtComp.Enabled = false;
                txtConservacion.Enabled = false;
                txtDenominacion.Enabled = false;
                txtDiametro.Enabled = false;
                txtEpoca.Enabled = false;
                txtKM.Enabled = false;
                txtPais.Enabled = false;
                txtPeso.Enabled = false;
                txtPrecio.Enabled = false;
                txtTirada.Enabled = false;
                btnGuardar.Enabled = false;
            }
        }

        private void llenarDatos(int buscado)
        {
            DataTable tabla = new DataTable();
            double PorcPagoIni = 0;
            double saldo = 0;
            double limite = 0;
            string peticion = "SELECT idmoneda,km,pais,peso,epoca,denominacion,ano,ceca,conservacion,diametro,composicion,tirada,precio,observaciones FROM moneda WHERE idmoneda=" + buscado + ";";
            //MOSTRAR DATOS
            using (MySqlConnection conexion = new MySqlConnection(Conexion.NuevaConexion()))
            {
                conexion.Open();
                MySqlDataAdapter adaptador = new MySqlDataAdapter(peticion, conexion);
                adaptador.Fill(tabla);
            }
            if (tabla.Rows.Count > 0)
            {
                txtCodigo.Text = tabla.Rows[0][0].ToString();
                txtKM.Text = tabla.Rows[0][1].ToString();
                txtPais.Text = tabla.Rows[0][2].ToString();
                txtPeso.Text = tabla.Rows[0][3].ToString();
                txtEpoca.Text = tabla.Rows[0][4].ToString();
                txtDenominacion.Text = tabla.Rows[0][5].ToString();
                txtAno.Text = tabla.Rows[0][6].ToString();
                txtCeca.Text = tabla.Rows[0][7].ToString();
                txtConservacion.Text = tabla.Rows[0][8].ToString();
                txtDiametro.Text = tabla.Rows[0][9].ToString();
                txtComp.Text = tabla.Rows[0][10].ToString();
                txtTirada.Text = tabla.Rows[0][11].ToString();
                txtPrecio.Text = tabla.Rows[0][12].ToString();
                txtComent.Text = tabla.Rows[0][13].ToString();
                btnEditar.Enabled = true;
                btnBorrar.Enabled = true;
                if (CargarImagen1() != null)
                {
                    pAnverso.Image = CargarImagen1();
                }
                if (CargarImagen2() != null)
                {
                    pReverso.Image = CargarImagen2();
                }
            }
        }
        private void pAnverso_Click(object sender, EventArgs e)
        {
            if(Program.buscado != 0)
            { 
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //DIRECTORIO POR DEFECTO DONDE SE BUSCARA LA IMAGEN
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.RestoreDirectory = true;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    //txtImagen.Text = openFileDialog1.FileName;
                    Ubicacion = openFileDialog1.FileName;
                    img = ImgABytes(Ubicacion);
                    pAnverso.Image = BytesAImg(img);
                        //subimg = true;
                        GuardarImagen1(BytesAImg(img));
                    }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: No se pudo encontrar el archivo: " + ex);
                }
            }
            else
            {
                //subimg = false;
            }
            }else
            {
                MessageBox.Show("Solo se pueden cargar imagenes de monedas que ya han sido registradas, registra una nueva moneda para cargar su imagen o busca una en el catalogo");
            }
        }

        private void pReverso_Click(object sender, EventArgs e)
        {
            if (Program.buscado != 0)
            {
                OpenFileDialog openFileDialog2 = new OpenFileDialog();
                //DIRECTORIO POR DEFECTO DONDE SE BUSCARA LA IMAGEN
                openFileDialog2.InitialDirectory = "c:\\";
                openFileDialog2.RestoreDirectory = true;
                DialogResult result = openFileDialog2.ShowDialog();
                if (result == DialogResult.OK)
                {
                    try
                    {
                        //txtImagen.Text = openFileDialog1.FileName;
                        Ubicacion = openFileDialog2.FileName;
                        img = ImgABytes(Ubicacion);
                        pReverso.Image = BytesAImg(img);
                        //subimg = true;
                        GuardarImagen2(BytesAImg(img));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: No se pudo encontrar el archivo: " + ex);
                    }
                }
                else
                {
                    //subimg = false;
                }
            }
            else
            {
                MessageBox.Show("Solo se pueden cargar imagenes de monedas que ya han sido registradas, registra una nueva moneda para cargar su imagen o busca una en el catalogo");
            }
        }

        //IMG A BYTES
        public Byte[] ImgABytes(String ruta)
        {
            FileStream img = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Byte[] arreglo = new Byte[img.Length];
            BinaryReader reader = new BinaryReader(img);
            arreglo = reader.ReadBytes(Convert.ToInt32(img.Length));
            img.Close();
            return arreglo;
        }
        //BYTES A IMG
        public Image BytesAImg(Byte[] ImgBytes)
        {
            Bitmap img = null;
            Byte[] bytes = (Byte[])(ImgBytes);
            MemoryStream ms = new MemoryStream(bytes);
            img = new Bitmap(ms);
            return img;
        }
        public void GuardarImagen1(Image imagen)
        {
            img = ImgABytes(Ubicacion);
            pAnverso.Image = BytesAImg(img);

            using (MemoryStream ms = new MemoryStream())
            {
                imagen.Save(ms, ImageFormat.Png);
                byte[] imgArr = ms.ToArray();
                using (MySqlConnection conn = new MySqlConnection(Conexion.NuevaConexion()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@imgArr", imgArr);
                        cmd.CommandText = "UPDATE moneda SET anverso = (@imgArr) WHERE idmoneda=" + Program.buscado + ";";
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                }
            }
        }
        public void GuardarImagen2(Image imagen)
        {
            img = ImgABytes(Ubicacion);
            pReverso.Image = BytesAImg(img);

            using (MemoryStream ms = new MemoryStream())
            {
                imagen.Save(ms, ImageFormat.Png);
                byte[] imgArr = ms.ToArray();
                using (MySqlConnection conn = new MySqlConnection(Conexion.NuevaConexion()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@imgArr", imgArr);
                        cmd.CommandText = "UPDATE moneda SET reverso = (@imgArr) WHERE idmoneda=" + Program.buscado + ";";
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                }
            }
        }
        public Image CargarImagen1()
        {
            using (MySqlConnection conn = new MySqlConnection(Conexion.NuevaConexion()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT anverso FROM moneda WHERE idmoneda=" + Program.buscado + ";";
                    try
                    {
                        byte[] imgArr = (byte[])cmd.ExecuteScalar();
                        using (var stream = new MemoryStream(imgArr))
                        {
                            Image img = Image.FromStream(stream);
                            return img;
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
        }
        public Image CargarImagen2()
        {
            using (MySqlConnection conn = new MySqlConnection(Conexion.NuevaConexion()))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT reverso FROM moneda WHERE idmoneda=" + Program.buscado + ";";
                    try
                    {
                        byte[] imgArr = (byte[])cmd.ExecuteScalar();
                        using (var stream = new MemoryStream(imgArr))
                        {
                            Image img = Image.FromStream(stream);
                            return img;
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
        }
        private void btnNueva_Click(object sender, EventArgs e)
        {
            Program.buscado = 0;
            Program.actualiza = 0;
            txtCodigo.Text = "";
            txtAno.Text = "";
            txtCeca.Text = "";
            txtComent.Text = "";
            txtComp.Text = "";
            txtConservacion.Text = "";
            txtDenominacion.Text = "";
            txtDiametro.Text = "";
            txtEpoca.Text = "";
            txtKM.Text = "";
            txtPais.Text = "";
            txtPeso.Text = "";
            txtPrecio.Text = "";
            txtTirada.Text = "";
            //
            //txtCodigo.Enabled = true;
            txtAno.Enabled = true;
            txtCeca.Enabled = true;
            txtComent.Enabled = true;
            txtComp.Enabled = true;
            txtConservacion.Enabled = true;
            txtDenominacion.Enabled = true;
            txtDiametro.Enabled = true;
            txtEpoca.Enabled = true;
            txtKM.Enabled = true;
            txtPais.Enabled = true;
            txtPeso.Enabled = true;
            txtPrecio.Enabled = true;
            txtTirada.Enabled = true;
            //
            btnGuardar.Enabled = true;
            btnBorrar.Enabled = false;
            btnEditar.Enabled = false;
            pAnverso.Image = null;
            pReverso.Image = null;

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //guarda            
            if (txtKM.Text == "")
            {
                MessageBox.Show("Captura el campo KM ", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtKM.Focus();
                return;
            }
            if (Program.actualiza == 1)
            {
                //idLimite = Convert.ToInt32(cbLimite.SelectedValue.ToString());
                //string peticion = "update clientes set Nombre='" + txtNombre.Text + "',ApePat='" + txtApePat.Text + "',ApeMat='" + txtApeMat.Text + "',Calle='" + txtCalle.Text + "',Colonia='" + cbColonia.Text + "',NumExt='" + txtNumExt.Text + "',NumInt='" + txtNumInt.Text + "',Ciudad='" + txtCiudad.Text + "',Telefono='" + txtTelefono.Text + "',RFC='" + txtRFC.Text + "',CURP='" + txtCURP.Text + "',Correo='" + txtCorreo.Text + "',DiaCobro='" + cbCobro.Text + "',Info='" + txtInfo.Text + "',Complemento='" + txtComplemento.Text + "',Identificador=1 WHERE ID_Cliente= '" + id + "';";
                string peticion = "update moneda set km='" + txtKM.Text + "',pais='" + txtPais.Text + "',epoca='" + txtEpoca.Text + "',denominacion='" + txtDenominacion.Text + "',ano='" + txtAno.Text + "',ceca='" + txtCeca.Text + "',conservacion='" + txtConservacion.Text + "',peso='" + txtPeso.Text + "',diametro='" + txtDiametro.Text + "',composicion='" + txtComp.Text + "',tirada='" + txtTirada.Text + "',precio='" + txtPrecio.Text + "',observaciones='" + txtComent.Text + "' WHERE idmoneda= '" + Program.buscado + "';";
                using (MySqlConnection cnn = new MySqlConnection(Conexion.NuevaConexion()))
                {
                    cnn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(peticion, cnn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Actualizado con éxito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Bloquear controles
                    Program.actualiza = 0;
                    txtAno.Enabled = false;
                    txtCeca.Enabled = false;
                    txtComent.Enabled = false;
                    txtComp.Enabled = false;
                    txtConservacion.Enabled = false;
                    txtDenominacion.Enabled = false;
                    txtDiametro.Enabled = false;
                    txtEpoca.Enabled = false;
                    txtKM.Enabled = false;
                    txtPais.Enabled = false;
                    txtPeso.Enabled = false;
                    txtPrecio.Enabled = false;
                    txtTirada.Enabled = false;
                    //
                    btnGuardar.Enabled = false;
                    btnEditar.Enabled = true;
                }
                //if (identificador == 0)//Cargar el limite Cuando es una busqueda normal
                //{
                //    if (nudSaldo.Value > 0)
                //    {
                //        if (nudSaldo.Value <= 50)
                //        {
                //            Calculo calculo = new Calculo();
                //            calculo.insertarSoloUnAbono(id, Convert.ToDouble(nudSaldo.Value));
                //        }
                //        else
                //        {
                //        int sem = cargarSemanasCliente(idLimite);
                //        Calculo calculo = new Calculo();
                //        calculo.calculaAbonoClienteMigrado(id, Convert.ToDouble(nudSaldo.Value), sem);
                //        }

                //    }
                //}
            }
            else
            {
                if (txtPais.Text == string.Empty)
                {
                    MessageBox.Show("Ingresa el pais.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPais.Focus();
                    return;
                }
                if (txtEpoca.Text == string.Empty)
                {
                    MessageBox.Show("Ingresa la epoca.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEpoca.Focus();
                    return;
                }
                if (txtDenominacion.Text == string.Empty)
                {
                    MessageBox.Show("Ingresa denominacion.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtDenominacion.Focus();
                    return;
                }
                if (txtAno.Text == string.Empty)
                {
                    MessageBox.Show("Ingresa el año.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAno.Focus();
                    return;
                }
                if (txtConservacion.Text == string.Empty)
                {
                    MessageBox.Show("Ingresa el conservacion.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtConservacion.Focus();
                    return;
                }
                if (txtPeso.Text == string.Empty)
                {
                    MessageBox.Show("Ingresa el peso.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPeso.Focus();
                    return;
                }
                if (txtDiametro.Text == string.Empty)
                {
                    MessageBox.Show("Ingresa el diametro.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtDiametro.Focus();
                    return;
                }
                if (txtComp.Text == string.Empty)
                {
                    MessageBox.Show("Ingresa el composicion.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtComp.Focus();
                    return;
                }
                if (txtTirada.Text == string.Empty)
                {
                    MessageBox.Show("Ingresa tirada.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTirada.Focus();
                    return;
                }
                if (txtPrecio.Text == string.Empty)
                {
                    MessageBox.Show("Ingresa el precio.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPrecio.Focus();
                    return;
                }
                if (txtComent.Text == string.Empty)
                {
                    MessageBox.Show("Ingresa el observaciones.", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtComent.Focus();
                    return;
                }
                string peticion = "insert into moneda(km,pais,peso,epoca,denominacion,ano,ceca,conservacion,diametro,composicion,tirada,precio,observaciones) values('" + txtKM.Text + "','" + txtPais.Text + "','" + txtPeso.Text + "','" + txtEpoca.Text + "','" + txtDenominacion.Text + "','" + txtAno.Text + "','" + txtCeca.Text + "','" + txtConservacion.Text + "','" + txtDiametro.Text + "','" + txtComp.Text + "','" + txtTirada.Text + "','" + txtPrecio.Text + "','" + txtComent.Text + "');";
                using (MySqlConnection conexion = new MySqlConnection(Conexion.NuevaConexion()))
                {
                    conexion.Open();
                    using (MySqlCommand cmmd = new MySqlCommand(peticion, conexion))
                    {
                        cmmd.ExecuteNonQuery();
                    }
                    //Bloquea controles
                    Program.actualiza = 0;
                    txtCodigo.Enabled = false;
                    txtAno.Enabled = false;
                    txtCeca.Enabled = false;
                    txtComent.Enabled = false;
                    txtComp.Enabled = false;
                    txtConservacion.Enabled = false;
                    txtDenominacion.Enabled = false;
                    txtDiametro.Enabled = false;
                    txtEpoca.Enabled = false;
                    txtKM.Enabled = false;
                    txtPais.Enabled = false;
                    txtPeso.Enabled = false;
                    txtPrecio.Enabled = false;
                    txtTirada.Enabled = false;
                    //
                    btnGuardar.Enabled = false;
                    btnEditar.Enabled = true;
                    btnNueva.PerformClick();
                    MessageBox.Show("Se a guardado tu moneda con éxito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
           }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            Program.actualiza = 0;
            try
            {
                //Borra
                String query = "DELETE FROM moneda WHERE idmoneda= '" + txtCodigo.Text + "';";
                using (MySqlConnection conexion = new MySqlConnection(Conexion.NuevaConexion()))
                {
                    conexion.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conexion);
                    {
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                        MessageBox.Show("Se borro la moneda con éxito", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //limpiar
                        btnNueva.PerformClick();                                             
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Program.actualiza = 1;
            txtAno.Enabled = true;
            txtCeca.Enabled = true;
            txtComent.Enabled = true;
            txtComp.Enabled = true;
            txtConservacion.Enabled = true;
            txtDenominacion.Enabled = true;
            txtDiametro.Enabled = true;
            txtEpoca.Enabled = true;
            txtKM.Enabled = true;
            txtPais.Enabled = true;
            txtPeso.Enabled = true;
            txtPrecio.Enabled = true;
            txtTirada.Enabled = true;
            //
            btnGuardar.Enabled = true;
            btnEditar.Enabled = false;            
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (Program.buscado != 0)
            {
                //int current bd= SELECT IDENT_CURRENT('customers');
                //SELECT MAX('id') FROM customers;
                DataTable tab = new DataTable();
                string petici = "SELECT idmoneda FROM moneda ORDER BY idmoneda ASC LIMIT 1";
                //MOSTRAR DATOS
                using (MySqlConnection conexion = new MySqlConnection(Conexion.NuevaConexion()))
                {
                    conexion.Open();
                    MySqlDataAdapter adaptador = new MySqlDataAdapter(petici, conexion);
                    adaptador.Fill(tab);
                }
                if (tab.Rows.Count > 0)
                {
                    int bot = Convert.ToInt32(tab.Rows[0][0]);
                    if(Program.buscado==bot)
                    { return; }
                    pAnverso.Image = null;
                    pReverso.Image = null;
                    bool resp=false;
                    do
                    {
                        //
                        Program.buscado = Program.buscado - 1;
                        //
                        DataTable tabx = new DataTable();
                        string peticix = "SELECT idmoneda FROM moneda Where idmoneda="+Program.buscado;
                        //MOSTRAR DATOS
                        using (MySqlConnection conexion = new MySqlConnection(Conexion.NuevaConexion()))
                        {
                            conexion.Open();
                            MySqlDataAdapter adaptador = new MySqlDataAdapter(peticix, conexion);
                            adaptador.Fill(tabx);
                        }
                        if (tabx.Rows.Count > 0)
                        {
                            resp = true;
                        }
                        else
                        {
                            resp = false;
                        }

                    } while (resp==false);
                    llenarDatos(Program.buscado);
                }            
            }
        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            pAnverso.Image = null;
            pReverso.Image = null;
            DataTable tabl = new DataTable();
            string peticio = "SELECT idmoneda FROM moneda ORDER BY idmoneda ASC LIMIT 1";
            //MOSTRAR DATOS
            using (MySqlConnection conexion = new MySqlConnection(Conexion.NuevaConexion()))
            {
                conexion.Open();
                MySqlDataAdapter adaptador = new MySqlDataAdapter(peticio, conexion);
                adaptador.Fill(tabl);
            }
            if (tabl.Rows.Count > 0)
            {
                //txtCodigo.Text = tabla.Rows[0][0].ToString();
                Program.buscado=Convert.ToInt32(tabl.Rows[0][0]);
                llenarDatos(Program.buscado);
            }
            
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            pAnverso.Image = null;
            pReverso.Image = null;
            DataTable tabl = new DataTable();
            string peticio = "SELECT idmoneda FROM moneda ORDER BY idmoneda DESC LIMIT 1";
            //MOSTRAR DATOS
            using (MySqlConnection conexion = new MySqlConnection(Conexion.NuevaConexion()))
            {
                conexion.Open();
                MySqlDataAdapter adaptador = new MySqlDataAdapter(peticio, conexion);
                adaptador.Fill(tabl);
            }
            if (tabl.Rows.Count > 0)
            {
                //txtCodigo.Text = tabla.Rows[0][0].ToString();
                Program.buscado = Convert.ToInt32(tabl.Rows[0][0]);
                llenarDatos(Program.buscado);
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (Program.buscado != 0)
            {
                //int current bd= SELECT IDENT_CURRENT('customers');
                //SELECT MAX('id') FROM customers;
                DataTable tab = new DataTable();
                string petici = "SELECT idmoneda FROM moneda ORDER BY idmoneda DESC LIMIT 1";
                //MOSTRAR DATOS
                using (MySqlConnection conexion = new MySqlConnection(Conexion.NuevaConexion()))
                {
                    conexion.Open();
                    MySqlDataAdapter adaptador = new MySqlDataAdapter(petici, conexion);
                    adaptador.Fill(tab);
                }
                if (tab.Rows.Count > 0)
                {
                    int top = Convert.ToInt32(tab.Rows[0][0]);
                    if (Program.buscado == top)
                    { return; }
                    pAnverso.Image = null;
                    pReverso.Image = null;
                    bool resp = false;
                    do
                    {
                        //
                        Program.buscado = Program.buscado + 1;
                        //
                        DataTable tabx = new DataTable();
                        string peticix = "SELECT idmoneda FROM moneda Where idmoneda=" + Program.buscado;
                        //MOSTRAR DATOS
                        using (MySqlConnection conexion = new MySqlConnection(Conexion.NuevaConexion()))
                        {
                            conexion.Open();
                            MySqlDataAdapter adaptador = new MySqlDataAdapter(peticix, conexion);
                            adaptador.Fill(tabx);
                        }
                        if (tabx.Rows.Count > 0)
                        {
                            resp = true;
                        }
                        else
                        {
                            resp = false;
                        }

                    } while (resp == false);
                    llenarDatos(Program.buscado);
                }
            }
        }

        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTirada_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
