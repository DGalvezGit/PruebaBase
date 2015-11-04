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
        private List<int> filasNuevas; //Cuando se agregan nuevos registros y aún no se le da guardar
        private List<int> filasEliminar; //Cuando se agregan nuevos registros y aún no se le da guardar

        public dgvDatos(VentanaPrincipal f):base()
        {
            this.f = f;
            this.btnAgrega = new Button();
            this.btnGuardar = new Button();
            base.tamañoPorcentajeX = 80;
            base.tamañoPorcentajeY = 100;
            this.inicializa(f);
            this.filasNuevas = new List<int>();
            this.filasEliminar = new List<int>();
        }

        private void inicializa(Form f)
        {
            Size sz = TextRenderer.MeasureText(textBtnAgrega, this.btnAgrega.Font);

            base.controlPrincipal = new DataGridView();
            base.inicializaControl(f,"");
            (base.controlPrincipal as DataGridView).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            (base.controlPrincipal as DataGridView).AllowUserToAddRows = false;
            (base.controlPrincipal as DataGridView).CellValidating += DgvDatos_CellValidating;
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

        private void DgvDatos_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            bool band;

            if (this.filasNuevas.Count > 0)
            {
                if (this.filasNuevas.Exists(a => a.Equals(e.RowIndex)))
                {
                    band = this.validaNuevoRegistro((base.controlPrincipal as DataGridView).Columns[e.ColumnIndex].Name,e);
                    if (!band)
                    {
                        e.Cancel = true;
                        this.btnGuardar.Enabled = false;
                    }
                    else
                    {
                        e.Cancel = false;
                        (base.controlPrincipal as DataGridView).Rows[e.RowIndex].ErrorText = "";
                        this.btnGuardar.Enabled = true;
                    }
                }
            }
        }

        private bool validaNuevoRegistro(string nomAtr, DataGridViewCellValidatingEventArgs e)
        {
            bool band = true;
            int entero;
            float flotante;
            char car;
            Atributo atr;
            Entidad ent = this.f.org.buscaEntidad(this.f.entidadSeleccionada());

            ent.agregaAtributos(this.f.org.listaAtributos(ent.nombre));
            atr = ent.listAtr.Find(a=>a.nombre.Equals(nomAtr));
            if (atr != null)
            {
                if (atr.campo.Equals(Atributo.CR))
                {
                    if (e.FormattedValue.Equals(""))
                    {
                        (base.controlPrincipal as DataGridView).Rows[e.RowIndex].ErrorText = "Campo requerido";
                        band = false;
                    }
                }
                if (band && !e.FormattedValue.Equals(""))
                {
                    switch (atr.tipo)
                    {
                        case Atributo.entero:
                            band = int.TryParse(e.FormattedValue.ToString(), out entero);
                            break;
                        case Atributo.flotante:
                            band = float.TryParse(e.FormattedValue.ToString(), out flotante);
                            break;
                        case Atributo.caracter:
                            band = char.TryParse(e.FormattedValue.ToString(), out car);
                            if (band)
                            {
                                car = car.ToString().ToUpper()[0];
                            }
                            break;
                        default:
                            band = true;
                            break;
                    }
                    if (!band)
                    {
                        (base.controlPrincipal as DataGridView).Rows[e.RowIndex].ErrorText = "El dato debe ser: " + atr.tipo;
                    }
                    else
                    {
                        if (atr.llave == Atributo.KP)
                        {
                            if (this.checaRepetido(atr.nombre, e.FormattedValue.ToString()))
                            {
                                (base.controlPrincipal as DataGridView).Rows[e.RowIndex].ErrorText = "clave repetida";
                                band = false;
                            }
                        }
                        else if (atr.llave == Atributo.KF)
                        {
                            Relacion rel = atr.listRel.Find(a=>a.nomAtr.Equals(atr.nombre));
                            ent = this.f.org.buscaEntidad(rel.nomEnt,Archivo.path+'\\'+this.f.org.tipo+'\\'+rel.bd);

                            ent.agregaAtributos(this.f.org.listaAtributos(rel.nomEnt, Archivo.path + '\\' + this.f.org.tipo + '\\' + rel.bd));
                            if (null == this.f.org.buscaBloque(ent, ent.listAtr, rel.nomAtr, e.FormattedValue.ToString(), Archivo.path + '\\' + this.f.org.tipo + '\\' + rel.bd))
                            {

                                if (MessageBox.Show("No se encontro registro en la relacion.\nDeseas continuar", "Error", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    this.filasEliminar.Add(e.RowIndex);
                                    this.btnGuardar.Visible = false;
                                    this.filasNuevas.RemoveAt(this.filasNuevas.FindIndex(a => a.Equals(e.RowIndex)));
                                    band = true;
                                }
                                else
                                {
                                    band = false;
                                }
                                
                            }
                            
                        }
                    }
                }
                
            }

            return band;
        }

        /// <summary>
        /// checa si ya existe un bloque con con el mismo dato de llave principal
        /// </summary>
        /// /// <param name="nomAtr">nombre del atributo</param>
        /// <param name="dato">dato a comparar</param>
        /// <returns>true si está repetido de lo contrario false</returns>
        private bool checaRepetido(string nomAtr, string dato)
        {
            Entidad ent = this.f.org.buscaEntidad(this.f.entidadSeleccionada());

            return this.f.org.buscaBloque(ent, this.f.org.listaAtributos(ent.nombre), nomAtr, dato) != null ? true : false;
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
                        this.btnGuardar.Enabled = false;
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
                this.filasNuevas.Clear();
            }
            this.insertaDatosDataGrid(ent);
        }

        /// <summary>
        /// Recupera los datos guardados en el archivo y los pone en el control
        /// </summary>
        /// <param name="ent"></param>
        public void insertaDatosDataGrid(Entidad ent)//por ahora se va a tomar en cuenta una organizacion secuencial
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
                    this.insertaDatosDataGridFila(bloq, ent.listAtr);
                }
            }
        }

        private void insertaDatosDataGridFila(byte[] b,List<Atributo>listAtr)
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



    }

}
