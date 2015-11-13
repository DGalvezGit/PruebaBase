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
        private ContextMenuStrip cmsMenu;

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
            
            this.inicializamenuContextual();
            base.controlPrincipal = new DataGridView();
            base.inicializaControl(f,"");
            (base.controlPrincipal as DataGridView).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            (base.controlPrincipal as DataGridView).AllowUserToAddRows = false;
            (base.controlPrincipal as DataGridView).CellValidating += DgvDatos_CellValidating;
            (base.controlPrincipal as DataGridView).CellMouseClick += DgvDatos_CellMouseClick;
            (base.controlPrincipal as DataGridView).ReadOnly = true;
          //  (base.controlPrincipal as DataGridView).EditMode = DataGridViewEditMode.EditProgrammatically;
            //(base.controlPrincipal as DataGridView).CellBeginEdit += DgvDatos_CellBeginEdit;
            (base.controlPrincipal as DataGridView).MultiSelect = false;
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
        /*
        private void DgvDatos_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            
        }
        */
        private void inicializamenuContextual()
        {
            ToolStripMenuItem modificaDato = new ToolStripMenuItem("Modificar dato", null, EventHAndler_modificaDato);
            ToolStripMenuItem eliminaRegistro = new ToolStripMenuItem("Elimina registro(Fila)", null, EventHAndler_eliminaRegistro);

            cmsMenu = new ContextMenuStrip();
            cmsMenu.Items.Add(modificaDato);
            cmsMenu.Items.Add(eliminaRegistro);
        }

        private void EventHAndler_modificaDato(object o, EventArgs e)
        {
            string dato = "";
            string nomAtr =(base.controlPrincipal as DataGridView).Columns[(base.controlPrincipal as DataGridView).CurrentCell.ColumnIndex].Name;
            Entidad ent = this.f.org.buscaEntidad(this.f.entidadSeleccionada());
            Atributo atr =this.f.org.buscaAtributo(ent,nomAtr);
            byte[] bloqNuevo,bloqViejo;

            if (!atr.llave.Equals(Atributo.KP) && !atr.llave.Equals(Atributo.KF))
            {
                
                using (DModificaDato d = new DModificaDato(nomAtr))
                {
                    d.ShowDialog();
                    if (d.DialogResult == DialogResult.OK)
                    {
                        dato = d.dato();
                        if (this.validaDato(atr, dato))
                        {
                            ent.agregaAtributos(this.f.org.listaAtributos(ent.nombre));
                            bloqViejo = Bloque.creaBloque(ent.listAtr, (base.controlPrincipal as DataGridView).Rows[(base.controlPrincipal as DataGridView).CurrentCell.RowIndex]);
                            (base.controlPrincipal as DataGridView).CurrentCell.Value = dato;
                            bloqNuevo = Bloque.creaBloque(ent.listAtr, (base.controlPrincipal as DataGridView).Rows[(base.controlPrincipal as DataGridView).CurrentCell.RowIndex]);
                            this.f.org.modificaBloque(ent,bloqViejo,bloqNuevo);
                        }
                        else
                        {
                            MessageBox.Show("Dato invalido");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Este atributo no se puede modificar");
            }
        }

        private void EventHAndler_eliminaRegistro(object o, EventArgs e)
        {
            bool band = false;
            Entidad ent;

            (base.controlPrincipal as DataGridView).Rows[(base.controlPrincipal as DataGridView).SelectedCells[0].RowIndex].Selected = true;
            if (this.f.org.usuario.baja)
            {
                if (MessageBox.Show("Seguro que quieres eliminar la fila?", "PELIGRO", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ent = this.f.org.buscaEntidad(this.f.entidadSeleccionada());
                    band = this.eliminaRegistro(ent);
                    if (band)
                    {
                        MessageBox.Show("Registro eliminado");
                        this.insertaDatosDataGrid(ent);
                    }
                    else
                    {
                        MessageBox.Show("Error al intentar eliminar registro");
                    }
                }
            }
        }

        private bool eliminaRegistro(Entidad ent)
        {
            bool band = false;
            byte[] bloq;

            ent.agregaAtributos(this.f.org.listaAtributos(ent.nombre));
            foreach (DataGridViewRow r in (base.controlPrincipal as DataGridView).SelectedRows)
            {
                if (!buscaRelacion(ent,r))
                {
                    bloq = Bloque.creaBloque(ent.listAtr, r);
                    band = this.f.org.eliminaRegistro(ent, bloq);

                }
            }

            return band;
        }

        private bool buscaRelacion(Entidad ent,DataGridViewRow r)
        {
            Atributo atr;
            bool band = false;
            string datoComp;

            atr = ent.listAtr.Find(a => a.llave.Equals(Atributo.KP));
            if (atr != null)
            {
                datoComp = (base.controlPrincipal as DataGridView)[atr.nombre, r.Index].Value.ToString();
                band = this.buscaRelacion(atr,datoComp);
            }

            return band;
        }

        private bool buscaRelacion(Atributo atr,string datoComp)
        {
            Entidad entAux;
            bool band = false;

            foreach (Relacion rel in atr.listRel)
            {
                entAux = this.f.org.buscaEntidad(rel.nomEnt);
                entAux.agregaAtributos(this.f.org.listaAtributos(entAux.nombre, Archivo.path + '\\' + this.f.org.tipo + '\\' + rel.bd));
                if (this.f.org.buscaBloque(entAux, entAux.listAtr, atr.nombre, datoComp) != null)
                {
                    band = true;
                    break;
                }
            }
            
            return band;
        }

        private void DgvDatos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
            {
                (base.controlPrincipal as DataGridView)[e.ColumnIndex, e.RowIndex].Selected = true;
                this.cmsMenu.Show(Cursor.Position);
            }
            
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
                    this.validaDato(atr,e.FormattedValue.ToString());
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

        private bool validaDato(Atributo atr,string dato)
        {
            bool band = true;
            int entero;
            float flotante;
            char car;

            switch (atr.tipo)
            {
                case Atributo.entero:
                    band = int.TryParse(dato, out entero);
                    break;
                case Atributo.flotante:
                    band = float.TryParse(dato, out flotante);
                    break;
                case Atributo.caracter:
                    band = char.TryParse(dato, out car);
                    if (band)
                    {
                        car = car.ToString().ToUpper()[0];
                    }
                    break;
                default:
                    band = true;
                    break;
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
