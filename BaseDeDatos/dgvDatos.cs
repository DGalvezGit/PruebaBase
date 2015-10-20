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
        
        /// <summary>
        /// Evento que se genera al presionar el botón guardar registro
        /// Crea el bloque de bytes y lo guarda en el archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            DataGridViewRow r;
            List<Atributo> listAtr = this.f.org.listaAtributos(this.f.entidadSeleccionada());
            Entidad ent = this.f.org.buscaEntidad(this.f.entidadSeleccionada());

            ent.agregaAtributos(this.f.org.listaAtributos(ent.nombre));
            if (this.filasNuevas.Count > 0)
            {
                foreach (int i in this.filasNuevas)
                {
                    r = (base.controlPrincipal as DataGridView).Rows[i];
                    this.f.org.altaBloque(ent, Bloque.creaBloque(listAtr, r));
                }
            }
        }


        public void insertaDatos(Entidad ent)//por ahora se va a tomar en cuenta una organizacion secuencial
        {

            long posIt = (ent as EntSecuencial).apBloq;
            byte[] bloq;
            int tam;

            (base.controlPrincipal as DataGridView).Rows.Clear();
            ent.agregaAtributos(this.f.org.listaAtributos(ent.nombre));
            tam = Bloque.calculaTamBloque(ent.listAtr);
            if (ent.listAtr.Count > 0)
            {
                while (posIt != -1)
                {
                    bloq = this.f.org.leeBloque(tam, posIt);
                    posIt = Bloque.leeApBloq(bloq);
                    this.insertaDatosFila(bloq, ent.listAtr);
                }
            }
        }

        private void insertaDatosFila(byte[] b,List<Atributo>listAtr)
        {
            int pos = 8;
            int indiceFila = (base.controlPrincipal as DataGridView).Rows.Add();
            int tam = 0;

            for (int i = 0; i < listAtr.Count; i++)
            {
                switch (listAtr[i].tipo)
                {
                    case Atributo.entero:
                        (base.controlPrincipal as DataGridView).Rows[indiceFila].Cells[i].Value = Bloque.convierteEntero(b, pos, ref tam);
                    break;
                    case Atributo.flotante:
                        (base.controlPrincipal as DataGridView).Rows[indiceFila].Cells[i].Value = Bloque.convierteFlotante(b, pos, ref tam);
                    break;
                    case Atributo.caracter:
                        (base.controlPrincipal as DataGridView).Rows[indiceFila].Cells[i].Value = Bloque.convierteChar(b, pos, ref tam);
                    break;
                    case Atributo.cadena:
                        (base.controlPrincipal as DataGridView).Rows[indiceFila].Cells[i].Value = Bloque.convierteCadena(b, pos, ref tam);
                        break;
                }
                pos += tam;
            }
        }

        /// <summary>
        /// Verifica que los campos requeridos contengan datos y los tipos de datos correspondan
        /// </summary>
        /// <param name="r">fila a verificar</param>
        /// <param name="listAtr">lista de atributos</param>
        /// <returns>true si es ta bien la fila de lo contrario regresa false</returns>
/*        private bool verificaFila(DataGridViewRow r,List<Atributo>listAtr)
        {
            bool band = true;

            for (int i = 0; i < listAtr.Count && band; i++)
            {
                if (listAtr[i].llave == Atributo.KP)//si es llave principal
                {

                                       if (this.verificaCelda(r.Cells[i].Value.ToString(), listAtr[i]))
                                       {
                                           guarda en el archivo
                                       }
                   
                }
                
                if (listAtr[i].campo == Atributo.NR)
                {
                    if (this.verificaCelda(r.Cells[i].Value.ToString(), listAtr[i]))
                    {
                        guarda en el archivo
                    }

                }
                else // Si el campo es requerido y el usuario no le dio un valor
                {
                    band = false;
                    MessageBox.Show("Fila: "+i+" ---El campo"+ listAtr[i].nombre +"es requerido");
                }
            }

            return band;
        }

        private bool verificaCelda(string dato, Atributo atr)
        {
            bool band = true;

            if (atr.llave == Atributo.KP)
            {
                verificar que no este repetido en el archivo
                 si esta repetido 
                 band=false;
                 
            }
            if(band==true)
            {
                verificar que el dato sea entero o letra
            }
            

            return band;
        }
*/
    }
}
