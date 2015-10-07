using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BaseDeDatos
{
    public class dgvDatos : Controles
    {
        private VentanaPrincipal f;
        private Button btnAgrega;
        private Button btnGuardar;
        private const string textBtnAgrega = "Agrega Registro";
        private const string textBtnGuardar= "Guardar";
        private List<int> filasNuevas;

        public dgvDatos(VentanaPrincipal f):base()
        {
            this.f = f;
            this.btnAgrega = new Button();
            this.btnGuardar = new Button();
            base.tamañoPorcentajeX = 80;
            base.tamañoPorcentajeY = 100;
            this.inicializa(f);
            this.filasNuevas = new List<int>();    
        }

        private void inicializa(Form f)
        {
            Size sz = TextRenderer.MeasureText(textBtnAgrega, this.btnAgrega.Font);

            base.controlPrincipal = new DataGridView();
            base.inicializaControl(f,"");
            (base.controlPrincipal as DataGridView).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            (base.controlPrincipal as DataGridView).AllowUserToAddRows = false;
            this.btnAgrega.Text = textBtnAgrega;
            this.btnGuardar.Text = textBtnGuardar;
            this.btnAgrega.Width = 100;
            this.btnAgrega.Click += BtnAgrega_Click;
            this.btnGuardar.Click += BtnGuardar_Click;
            this.btnGuardar.Visible = false;
            this.btnAgrega.Visible = false;
            f.Controls.Add(this.btnAgrega);
            f.Controls.Add(this.btnGuardar);

        }
        

        public void limpiaControl()
        {
            ((DataGridView)base.controlPrincipal).Rows.Clear();
            ((DataGridView)base.controlPrincipal).Columns.Clear();
            this.btnGuardar.Visible = false;
            this.btnAgrega.Visible = false;
            this.ubicaBotones();
        }

        public void agregaColumnas(List<Atributo> listAtr)
        {
            (base.controlPrincipal as DataGridView).Columns.Clear();
            foreach (Atributo atr in listAtr)
            {
                (base.controlPrincipal as DataGridView).Columns.Add(atr.nombre,atr.nombre);
            }
            if ((base.controlPrincipal as DataGridView).ColumnCount > 0)
            {
                this.btnAgrega.Visible = true;
            }
        }


        public void actualizaUsuario(string nombre)
        {
            base.label.Text = nombre;
            base.label.Size = (TextRenderer.MeasureText(base.label.Text, base.label.Font));
            base.label.Location = new Point(base.controlPrincipal.Location.X, base.controlPrincipal.Location.Y - base.label.Size.Height);
            
        }

        /// <summary>
        /// Asigna una nueva ubicación al control encapsulado(label y control)
        /// </summary>
        /// <param name="x">posicion en x</param>
        /// <param name="y">posición en y</param>
        public new void ubicacion(int x, int y)
        {
            base.ubicacion(x, y);
            this.ubicaBotones();
        }

        private void ubicaBotones()
        {
            this.btnGuardar.Location = new Point(base.controlPrincipal.Location.X + base.controlPrincipal.Width - this.btnGuardar.Width,
                                                base.controlPrincipal.Location.Y - this.btnGuardar.Height);
            if (!this.btnGuardar.Visible)
            {
                this.btnAgrega.Location = new Point(base.controlPrincipal.Location.X + base.controlPrincipal.Width - this.btnAgrega.Width,
                                                base.controlPrincipal.Location.Y - this.btnAgrega.Height);
            }
            else
            {
                this.btnAgrega.Location = new Point(this.btnGuardar.Location.X - this.btnAgrega.Width - 5, this.btnGuardar.Location.Y);
            }
        }

        private void BtnAgrega_Click(object sender, EventArgs e)
        {
            if (this.f.orgAbierta)
            {
                if (this.f.org.usuario.alta)
                {
                    try
                    {
                        (base.controlPrincipal as DataGridView).Rows.Add();
                        this.filasNuevas.Add((base.controlPrincipal as DataGridView).RowCount - 1);
                        this.btnGuardar.Visible = true;
                        this.ubicaBotones();
                    }
                    catch (InvalidOperationException x)
                    {
                        MessageBox.Show(x.Message);
                    }
                }
                else
                {
                    MessageBox.Show("No tienes suficientes privilegios");
                }
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            DataGridViewRow r;
            List<Atributo> listAtr = this.f.org.listaAtributos(this.f.entidadSeleccionada());

            if (this.filasNuevas.Count > 0)
            {
                foreach (int i in this.filasNuevas)
                {
                    r = (base.controlPrincipal as DataGridView).Rows[i];
                    Bloque.creaBloque(listAtr, r);
                }
            }
        }

    }
}
