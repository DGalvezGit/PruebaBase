using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatos
{
    public partial class dUser : Form
    {
        public string nombre;
        public string contraseña;
        public bool[] priv;
        public string vig;


        public dUser()
        {
            priv = new bool[4];
            InitializeComponent();
        }

        public void privilegios(bool band)
        {
            chbConsulta.Enabled = band;
            chbConsulta.Checked = !band;
            chbAlta.Enabled = band;
            chbAlta.Checked = !band;
            chbBaja.Enabled = band;
            chbBaja.Checked = !band;
            chbMod.Enabled = band;
            chbMod.Checked = !band;
            btnCancelar.Enabled = band;
        }

        public void visualizaControles(bool band)
        {
            lPrivilegios.Visible = band;
            chbConsulta.Visible = band;
            chbAlta.Visible = band;
            chbBaja.Visible = band;
            chbMod.Visible = band;
            dtpVig.Visible = band;
            lVigencia.Visible = band;
            btnCancelar.Enabled = !band;

        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.nombre = tbNombre.Text;
            this.contraseña = tbContra.Text;
            this.priv[0] = chbConsulta.Checked;
            this.priv[1] = chbAlta.Checked;
            this.priv[2] = chbBaja.Checked;
            this.priv[3] = chbMod.Checked;
            vig = dtpVig.Value.Date.ToString();
            this.DialogResult = DialogResult.OK;
        }

        private void dUser_Load(object sender, EventArgs e)
        {

        }


    }

}
