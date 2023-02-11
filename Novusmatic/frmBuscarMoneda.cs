using DevExpress.XtraReports.UI;
using MySql.Data.MySqlClient;
using Novusmatic.Clases;
using Novusmatic.Reportes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Novusmatic
{
    public partial class frmBuscarMoneda : Form
    {
        DataTable tablacat = new DataTable();
        public frmBuscarMoneda()
        {
            InitializeComponent();
        }

        private void btnBusca_Click(object sender, EventArgs e)
        {
            string peticion = string.Empty;
            //string peticion = "select ID_Cliente,NombreCompleto,LimiteCredito from clientes where ID_Cliente LIKE'%" + txtNombre.Text + "%' or NombreCompleto like'%" + txtNombre.Text + "%' LIMIT 0,18;";
            //string peticion = "SELECT clientes.ID_Cliente,clientes.NombreCompleto,limitedecredito.CantidadLimite FROM clientes INNER JOIN limitedecredito ON clientes.LimiteCredito = limitedecredito.ID_Limite WHERE clientes.ID_Cliente LIKE'%" + txtNombre.Text + "%' or clientes.ID_Cliente NOT LIKE '1' or clientes.Nombre REGEXP '^" + txtNombre.Text + "' or clientes.ApePat Regexp '^" + txtApePat.Text + "' or clientes.ApeMat regexp '^" + txtApeMat.Text + "' LIMIT 0,18;"; 
            peticion = "SELECT anverso,reverso,idmoneda, km, pais, peso, epoca, denominacion, ano, ceca, conservacion, diametro, composicion, tirada, precio, observaciones" +
                " FROM moneda WHERE idmoneda regexp '^" + txtCodigo.Text + 
                "' AND km regexp '^" + txtKM.Text + "' AND pais regexp '^" + txtPais.Text + 
                "' AND epoca regexp '^" + txtEpoca.Text + "' AND denominacion regexp '^" + txtDenominacion.Text + 
                "' AND conservacion regexp '^" + txtConservacion.Text + "' AND peso regexp '^" + txtPeso.Text + 
                "' AND diametro regexp '^" + txtDiametro.Text + "' AND ano regexp '^" + txtAno.Text + 
                "' AND ceca regexp '^" + txtCeca.Text + "' AND composicion regexp '^" + txtComposicion.Text + 
                "' AND precio regexp '^" + txtPrecio.Text + 
                "' ;"; // and clientes.ApeMat regexp '^" + txtApeMat.Text + "' LIMIT 0,5;";

            //if (txtCodigo.Text != string.Empty)
            //{
            //    peticion = "SELECT * FROM moneda  WHERE idmoneda LIKE'%" + txtCodigo.Text + "' or km LIKE'%" + txtKM.Text + "' or pais LIKE'%" + cbPais.Text + "';";// and clientes.ApeMat regexp '^" + txtApeMat.Text + "' LIMIT 0,5;";
            //}
            //else
            //{
            //    MessageBox.Show("No se encontro registro, prueba con otros parametros de busqueda");
            //    return;
            //}
            
            try
            {
                using (MySqlConnection conexion = new MySqlConnection(Conexion.NuevaConexion()))
                {
                    conexion.Open();
                     MySqlDataAdapter adaptador = new MySqlDataAdapter(peticion, conexion);
                    tablacat.Clear();
                    adaptador.Fill(tablacat);
                    dgv1.DataSource = tablacat;
                }
                if (tablacat.Rows.Count > 0)
                {
                    dgv1.Columns["anverso"].HeaderText = "Anverso";
                    dgv1.Columns["anverso"].Width = 30;
                    dgv1.Columns["reverso"].HeaderText = "Reverso";
                    dgv1.Columns["reverso"].Width = 60;
                    dgv1.Columns["idMoneda"].HeaderText = "Código";
                    dgv1.Columns["pais"].HeaderText = "País";
                    dgv1.Columns["km"].HeaderText = "KM";
                    dgv1.Columns["epoca"].HeaderText = "Época";
                    dgv1.Columns["denominacion"].HeaderText = "Denominación";
                    dgv1.Columns["Ano"].HeaderText = "Año";
                    dgv1.Columns["ceca"].HeaderText = "Ceca";
                    dgv1.Columns["conservacion"].HeaderText = "Conservación";
                    dgv1.Columns["peso"].HeaderText = "Peso";
                    dgv1.Columns["diametro"].HeaderText = "Diámetro";
                    dgv1.Columns["composicion"].HeaderText = "Composición";
                    dgv1.Columns["tirada"].HeaderText = "Tirada";
                    dgv1.Columns["precio"].HeaderText = "Precio";
                    dgv1.Columns["observaciones"].HeaderText = "Observaciones";
                    // dgv1.Columns["ID_Cliente"].Width = 50;
                    // dgv1.Columns["NombreCompleto"].HeaderText = "NOMBRE";
                    //dgv1.Columns["anverso"].Width = 120;
                    //dgv1.Columns["reverso"].Width = 120;
                    //dgv1.Columns["denominacion"].Width = 120;
                    //  //dgv1.Columns["NombreCompleto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    // dgv1.Columns["CantidadLimite"].HeaderText = "LIMITE DE CREDITO";
                    //  dgv1.Columns["PagoInicial"].HeaderText = "ENGANCHE";
                    //  dgv1.Columns["Saldo"].HeaderText = "SALDO";

                    //   dgv1.ClearSelection();
                    //   dgv1.Rows[0].Selected = true;
                    //   dgv1.Focus();
                    //   //txtNombre.Focus();
                }
            }
            catch (ConstraintException ex)
            {
                MessageBox.Show("No se pudo conectar verifique conexion con el servidor " + ex);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = "";
            txtKM.Text = "";
            txtAno.Text = "";
            txtCeca.Text = "";
            txtComposicion.Text = "";
            txtConservacion.Text = "";
            txtDenominacion.Text = "";
            txtDiametro.Text = "";
            txtEpoca.Text = "";
            txtPais.Text = "";
            txtPeso.Text = "";
            txtPrecio.Text = "";
            dgv1.DataSource = null;
        }

        private void dgv1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgv1.SelectedRows.Count > 0)
                {
                    //idenviar = Convert.ToInt32(dgv1.SelectedRows[0].Cells[0].Value);
                    //nombreenviar = dgv1.SelectedRows[0].Cells[1].Value.ToString();
                    //limiteenviar = dgv1.SelectedRows[0].Cells[2].Value.ToString();
                    //btnSeleccionar.PerformClick();
                    Program.buscado = Convert.ToInt32(dgv1.SelectedRows[0].Cells[2].Value);
                    this.Close();
                }
            }
            catch (ConstraintException)
            {
                return;
            }
        }

        private void txtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnBusca.PerformClick();
            }
        }

        private void cbPais_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnBusca.PerformClick();
            }
        }

        private void cbEpoca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnBusca.PerformClick();
            }
        }

        private void cbDenominacion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnBusca.PerformClick();
            }
        }

        private void txtKM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnBusca.PerformClick();
            }
        }

        private void cbConservacion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnBusca.PerformClick();
            }
        }

        private void cbPeso_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnBusca.PerformClick();
            }
        }

        private void cbDiametro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnBusca.PerformClick();
            }
        }

        private void cbAno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnBusca.PerformClick();
            }
        }

        private void cbCeca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnBusca.PerformClick();
            }
        }

        private void cbComposicion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnBusca.PerformClick();
            }
        }

        private void cbPrecio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnBusca.PerformClick();
            }
        }

        private void btnImpTodo_Click(object sender, EventArgs e)
        {

            frmProcesando process = new frmProcesando();
            process.Show();            
            XtraReportTodo report = new XtraReportTodo();            
            report.RequestParameters = false;
            ReportPrintTool pt = new ReportPrintTool(report);
            pt.AutoShowParametersPanel = false;           
            pt.ShowPreview();            
            process.Close();    
        }

        private void btnImpLista_Click(object sender, EventArgs e)
        {
            //peticion
            if (tablacat.Rows.Count > 0)
            {
                frmProcesando process = new frmProcesando();
                process.Show();
                XtraReportLista report1 = new XtraReportLista();
                report1.DataSource = tablacat;
                report1.DataMember = "moneda";
                report1.DataAdapter = tablacat;
                ReportPrintTool pt = new ReportPrintTool(report1);
                pt.AutoShowParametersPanel = false;
                report1.ShowPreview();
                process.Close();
            }
            else
            {
                MessageBox.Show("No hay lista para imprimir");
            }              
        }
    }
}
