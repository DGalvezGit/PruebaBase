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
    public partial class CreaEstructura : Form
    {
        private Button btnSiguiente;
        private Button btnAtras;
        private Button btnCancelar;
        private uint numVista;
        private NombreDB v1;
        private Usuarios us;
        private Entidades ent;
        private Atributos atr;

        public CreaEstructura()
        {
            this.numVista = 1;
            this.btnSiguiente = new Button();
            this.btnAtras = new Button();
            this.btnCancelar = new Button();
            
            InitializeComponent();
        }

        private void CreaEstructura_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.Text = "Nueva Estructura";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = this.ClientSize;
            this.MaximumSize = this.ClientSize;
            this.cargaBotones();
            this.CancelButton = this.btnCancelar;
            this.cambiaVista();
            this.ResumeLayout();

        }

        private void cargaBotones()
        {
            Size s = new Size(this.ClientSize.Width, this.ClientSize.Height);
            int espacio = 10;

            this.btnSiguiente.Text = "Siguiente";
            this.btnAtras.Text = "Atras";
            this.btnCancelar.Text = "Cancelar";
            this.btnSiguiente.Location = new Point(s.Width - this.btnSiguiente.Width - espacio, s.Height - this.btnSiguiente.Height - espacio);
            this.btnAtras.Location = new Point(this.btnSiguiente.Location.X - this.btnAtras.Width - espacio, s.Height - this.btnAtras.Height - espacio);
            this.btnCancelar.Location = new Point(espacio, s.Height - this.btnCancelar.Height - espacio);
            this.Controls.Add(this.btnSiguiente);
            this.Controls.Add(this.btnAtras);
            this.Controls.Add(this.btnCancelar);
            this.btnSiguiente.Click += new EventHandler(buttonSiguiente_Click);
            this.btnAtras.Click += new EventHandler(buttonAtras_Click);
        }

        private void habilitaBotones(bool btnSig, bool btnAtras)
        {
            if (btnSig == true)
            {
                this.btnSiguiente.Text = "Siguiente";
            }
            else
            {
                this.btnSiguiente.Text = "Finalizar";
            }
            this.btnAtras.Enabled = btnAtras;
        }


        private void buttonSiguiente_Click(object sender, EventArgs e)
        {
            if (this.numVista < 4)
            {
                switch (this.numVista)
                {
                    case 1: //Crea un usuario
                        v1.muestraControles(false);
                        break;
                    case 2: //Agrega entidades
                        us.muestraControles(false);
                        break;
                    case 3: //Agrega Atributos
                        this.ent.muestraControles(false);
                        break;
                }
                this.numVista++;
                this.cambiaVista();
            }
            else
            {
                this.atr.agregaUltimosAtr();
                this.DialogResult = DialogResult.OK;
            }
        }

        public string nombreDB()
        {
            return this.v1 != null ? this.v1.nombreBD() : "";
        }

        public Usuario usuario()
        {
            return this.us.usuario();
        }

        public List<Entidad> entidades()
        {
            return this.atr.listEnt;
        }

        private void cambiaVista()
        {
            switch (this.numVista)
            {
                case 1: // Nombre de la base de datos
                    this.habilitaBotones(true, false);
                    if (this.v1 == null)
                    {
                        this.v1 = new NombreDB(this);
                    }
                    else
                    {
                        this.v1.muestraControles(true);
                    }
                    break;
                case 2: //Crea un usuario
                    this.habilitaBotones(true, true);
                    if (this.us == null)
                    {
                        this.us = new Usuarios(this);
                    }
                    else
                    {
                        this.us.muestraControles(true);
                    }
                    break;
                case 3: //Agrega entidades
                    this.habilitaBotones(true, true);
                    if (this.ent == null)
                    {
                        this.ent = new Entidades(this,'s');
                    }
                    else
                    {
                        this.ent.muestraControles(true);
                    }
                break;
                case 4: //Agrega Atributos
                    this.habilitaBotones(false, true);
                    if (this.atr == null)
                    {
                        this.atr = new Atributos(this,this.ent.entidades());
                    }
                    else
                    {
                        this.atr.actualizaEntidades();
                        this.atr.muestraControles(true);
                    }
                break;
            }
        }

        private void buttonAtras_Click(object sender, EventArgs e)
        {
            switch (numVista)
            {
                case 2:
                    this.us.muestraControles(false);
                break;
                case 3:
                    
                    this.ent.muestraControles(false);
                break;
                case 4:

                    this.atr.muestraControles(false);
                break;
            }
            this.numVista--;
            this.cambiaVista();
        }

        ~CreaEstructura()
        {
            this.v1 = null;
            this.us = null;
            this.ent = null;
            this.atr = null;
        }

        class NombreDB : Controles
        {
            
            public NombreDB(Form f)
            {
                base.label = new Label();
                base.controlPrincipal = new TextBox();
                this.inicializa(f);
            }

            private void inicializa(Form f)
            {
                base.label.Text = "Escribe un nombre";
                f.Controls.Add(base.label);
                f.Controls.Add(base.controlPrincipal);
                base.controlPrincipal.Focus();
                this.ubicacion(f.ClientSize.Width/2-this.tamControl.X/2,f.ClientSize.Height/2-this.tamControl.Y);
            }

            public void muestraControles(bool muestra)
            {
                base.label.Visible = muestra;
                base.controlPrincipal.Visible = muestra;
            }

            public string nombreBD()
            {
                return ((TextBox)this.controlPrincipal).Text;
            }

        }

        class Usuarios
        {
            private Label lNombre;
            private Label lContraseña;
            private TextBox tbNombre;
            private TextBox tbContraseña;
            private CheckBox chbAlta;
            private CheckBox chbBaja;
            private CheckBox chbModificacion;
            private CheckBox chbConsulta;
            private DateTimePicker dtpVigInicio;
            private DateTimePicker dtpVigFinal;
            private GroupBox gbPriv;

            public Usuarios(Form f)
            {
                this.lNombre = new Label();
                this.lContraseña = new Label();
                this.tbNombre = new TextBox();
                this.tbContraseña = new TextBox();
                this.chbAlta = new CheckBox();
                this.chbBaja = new CheckBox();
                this.chbModificacion = new CheckBox();
                this.chbConsulta = new CheckBox();
                this.dtpVigInicio = new DateTimePicker();
                this.dtpVigFinal = new DateTimePicker();
                this.gbPriv = new GroupBox();

                this.inicializaControles(f);
            }

            private void inicializaControles(Form f)
            {
                f.SuspendLayout();
                this.inicializaTextBox(f);
                this.inicializaGroupBox(f);

                this.dtpVigInicio.Location = new Point(this.gbPriv.Location.X-this.dtpVigInicio.Width-10,this.tbContraseña.Location.Y+this.tbContraseña.Height+10);
                this.dtpVigFinal.Location = new Point(this.dtpVigInicio.Location.X,this.dtpVigInicio.Location.Y+this.dtpVigInicio.Height+5);
                f.Controls.Add(this.dtpVigInicio);
                f.Controls.Add(this.dtpVigFinal);
                
                f.ResumeLayout();

            }

            private void inicializaTextBox(Form f)
            {
                this.lNombre.Text = "Usuario:";
                this.lContraseña.Text = "Contraseña";
                this.tbNombre.Location = new Point(f.ClientSize.Width / 2 - this.tbNombre.Width / 2, f.ClientSize.Height / 3 - this.tbNombre.Height);
                this.tbContraseña.Location = new Point(f.ClientSize.Width / 2 - this.tbNombre.Width / 2, this.tbNombre.Location.Y + this.tbNombre.Height + 5);
                this.lNombre.Size = TextRenderer.MeasureText(this.lNombre.Text, this.lNombre.Font);
                this.lNombre.Location = new Point(this.tbNombre.Location.X - this.lNombre.Width, f.ClientSize.Height / 3 - this.tbNombre.Height);
                this.lContraseña.Size = TextRenderer.MeasureText(this.lContraseña.Text, this.lContraseña.Font);
                this.lContraseña.Location = new Point(this.tbContraseña.Location.X - this.lContraseña.Width, this.tbNombre.Location.Y + this.tbNombre.Height + 5);
                this.tbContraseña.PasswordChar = '*';
                this.tbContraseña.UseSystemPasswordChar = true;
                f.Controls.Add(this.lNombre);
                f.Controls.Add(this.lContraseña);
                f.Controls.Add(this.tbNombre);
                f.Controls.Add(this.tbContraseña);
            }

            private void inicializaGroupBox(Form f)
            {
                int espacioEntreCont = 10;

                this.chbAlta.Text = "Alta";
                this.chbBaja.Text = "Baja";
                this.chbModificacion.Text = "Modificacion";
                this.chbConsulta.Text = "Consulta";
                this.gbPriv.Text = "Privilegios";
                this.chbAlta.Enabled = false;
                this.chbAlta.Checked = true;
                this.chbBaja.Enabled = false;
                this.chbBaja.Checked = true;
                this.chbModificacion.Enabled = false;
                this.chbModificacion.Checked = true;
                this.chbConsulta.Enabled = false;
                this.chbConsulta.Checked = true;
                this.gbPriv.Size = new Size(this.chbConsulta.Width+(espacioEntreCont*2),this.chbAlta.Height*3+espacioEntreCont*4);
                this.chbAlta.Location = new Point(15, 15);
                this.chbBaja.Location = new Point(15, this.chbAlta.Location.Y+this.chbAlta.Height+espacioEntreCont);
                this.chbModificacion.Location = new Point(15,this.chbBaja.Location.Y+this.chbBaja.Height+espacioEntreCont);
                this.chbConsulta.Location = new Point(15, this.chbModificacion.Location.Y + this.chbModificacion.Height + espacioEntreCont);
                this.gbPriv.Location = new Point(this.tbContraseña.Location.X + this.tbContraseña.Width + 15, f.ClientSize.Height / 3 - this.tbNombre.Height);
                f.Controls.Add(this.gbPriv);
                this.gbPriv.Controls.Add(this.chbAlta);
                this.gbPriv.Controls.Add(this.chbBaja);
                this.gbPriv.Controls.Add(this.chbModificacion);
                this.gbPriv.Controls.Add(this.chbConsulta);
            }

            public void muestraControles(bool muestra)
            {
                this.lNombre.Visible = muestra;
                this.lContraseña.Visible = muestra;
                this.tbNombre.Visible = muestra;
                this.tbContraseña.Visible = muestra;
                this.dtpVigInicio.Visible = muestra;
                this.dtpVigFinal.Visible = muestra;
                this.gbPriv.Visible = muestra;
            }

            public Usuario usuario()
            {
                return new Usuario(this.tbNombre.Text, this.tbContraseña.Text,new bool[] {chbAlta.Checked,chbBaja.Checked,chbModificacion.Checked,chbConsulta.Checked },dtpVigInicio.Value,dtpVigFinal.Value);
            }

            ~Usuarios()
            {
                this.lNombre.Dispose();
                this.lContraseña.Dispose();
                this.tbNombre.Dispose();
                this.tbContraseña.Dispose();
                this.chbAlta.Dispose();
                this.chbBaja.Dispose();
                this.chbConsulta.Dispose();
                this.dtpVigInicio.Dispose();
                this.dtpVigFinal.Dispose();
                this.gbPriv.Dispose();
            }
        }

        class Entidades : Controles
        {
            char tipo; //Tipo de estructura para las entidades
            public Entidades(Form f,char t):base()
            {
                this.tipo = t;
                base.controlPrincipal = new RichTextBox();
                this.inicializa(f);
            }

            private void inicializa(Form f)
            {
                base.label.Text = "Entidades";
                base.label.Size = TextRenderer.MeasureText(base.label.Text,base.label.Font);
                base.controlPrincipal.Size = new Size(125,178);
                base.label.Location = new Point(f.ClientSize.Width / 2 - this.controlPrincipal.Width / 2, ((f.ClientSize.Height / 2) - (base.controlPrincipal.Height / 2))- base.label.Height);
                base.controlPrincipal.Location = new Point(f.ClientSize.Width/2- base.controlPrincipal.Width/2, f.ClientSize.Height / 2 - base.controlPrincipal.Height / 2);
                f.Controls.Add(base.label);
                f.Controls.Add(controlPrincipal);
            }

            public void muestraControles(bool band)
            {
                base.label.Visible = band;
                base.controlPrincipal.Visible = band;
            }

            public List<Entidad> entidades()
            {
                List<string> listStr = ((RichTextBox)base.controlPrincipal).Text.Split('\n').ToList();
                List<Entidad> listEnt = new List<Entidad>();

                listStr.RemoveAll(a => a.Equals(""));

                foreach (string s in listStr)
                {
                    switch (this.tipo)
                    {
                        //En caso de que sea secuencial crea una entidad para la secuencial
                        case 's':
                        case 'S':
                            listEnt.Add(new EntSecuencial(s));
                        break;
                    }
                }
                
                return listEnt;
            }
        }

        class Atributos: Controles
        {
            ComboBox cbEntidades;
            int numEnt;
            private bool rowAddedActivate;
            private List<Entidad> ListEnt;
            public List<Entidad>listEnt
            {
                get { return this.ListEnt; }
            }

            public Atributos(Form f,List<Entidad> listEnt): base()
            {
                this.rowAddedActivate = true;
                this.cbEntidades = new ComboBox();
                base.controlPrincipal = new DataGridView();
                base.inicializaControl(f, "Atributos");
                this.inicializa(f,listEnt);
                
            }
            
            private void inicializa(Form f,List<Entidad> listEnt)
            {
                this.ListEnt = listEnt;
                this.actualizaEntidades();
                f.Controls.Add(this.cbEntidades);
                this.inicializaDataGrid(f.ClientSize.Width, f.ClientSize.Height);
                this.cbEntidades.Location = new Point(base.coordenadas.X+(base.tamControl.X/2)-this.cbEntidades.Width/2,base.coordenadas.Y-this.cbEntidades.Height-10);
                this.cbEntidades.SelectedIndexChanged += new EventHandler(this.seletedIndexChanged_Click);

                
            }

            private void inicializaDataGrid(int anchoVentana,int altoVentana)
            {
                ((DataGridView)base.controlPrincipal).BackgroundColor = SystemColors.Control;
                ((DataGridView)base.controlPrincipal).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                ((DataGridView)base.controlPrincipal).RowHeadersVisible = false;
                base.tamControl = new Point(450, 200);
                base.ubicacion((anchoVentana / 2 - (base.tamControl.X / 2)), (altoVentana / 2) - (base.tamControl.Y / 2));
                this.agregaColumnas();
                ((DataGridView)base.controlPrincipal).RowsAdded += new DataGridViewRowsAddedEventHandler(this.rowAdded_EventHandler);
                this.rowAdded_EventHandler(this,null);
            }

            private void rowAdded_EventHandler(object o, DataGridViewRowsAddedEventArgs e)
            {
                if (this.rowAddedActivate)
                {
                    foreach (DataGridViewRow r in ((DataGridView)base.controlPrincipal).Rows)
                    {
                        if (r.Cells["ColumnName"].Value == null)
                        {
                            r.Cells["ColumnKey"].Value = (r.Cells["ColumnKey"] as DataGridViewComboBoxCell).Items[1];
                            r.Cells["ColumnType"].Value = (r.Cells["ColumnType"] as DataGridViewComboBoxCell).Items[0];
                            r.Cells["ColumnCamp"].Value = (r.Cells["ColumnCamp"] as DataGridViewComboBoxCell).Items[0];
                        }
                    }
                }
            }

            public void seletedIndexChanged_Click(object o, EventArgs e)
            {
                Entidad aEnt = this.ListEnt.Find(a=>a.nombre.Equals(this.cbEntidades.Items[this.numEnt]));

                if (aEnt != null)
                {
                    aEnt.agregaAtributos(this.atributos());
                    ((DataGridView)base.controlPrincipal).Rows.Clear();
                    aEnt = this.ListEnt.Find(a => a.nombre.Equals(this.cbEntidades.Items[this.cbEntidades.SelectedIndex]));
                    if (aEnt.listAtr.Count > 0)
                    {
                        this.agregaAtributosDataGrid(aEnt, (DataGridView)base.controlPrincipal);
                    }
                }
                this.numEnt = this.cbEntidades.SelectedIndex;
            }
            

            private void agregaAtributosDataGrid(Entidad ent, DataGridView dgv)
            {
                this.rowAddedActivate = false;
                foreach (Atributo atr in ent.listAtr)
                {
                    dgv.Rows.Add(atr.llave.Equals('P')|| atr.llave.Equals('F')?"K"+ atr.llave:"None", atr.nombre, atr.tipo, atr.campo, atr.comentario);
                }
                this.rowAddedActivate = true;
            }

            public void actualizaEntidades()
            {
                this.cbEntidades.Items.Clear();

                foreach (Entidad ent in ListEnt)
                {
                    this.cbEntidades.Items.Add(ent.nombre);
                }
                if (this.ListEnt.Count > 0)
                {
                    this.cbEntidades.SelectedIndex = 0;
                    this.numEnt = 0;
                }

            }

            private void agregaColumnas()
            {
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                DataGridViewTextBoxColumn columnName = new DataGridViewTextBoxColumn();
                DataGridViewComboBoxColumn columnType = new DataGridViewComboBoxColumn();
                DataGridViewComboBoxColumn columnCamp = new DataGridViewComboBoxColumn();
                DataGridViewColumn columnComment = new DataGridViewTextBoxColumn();

                
                column.HeaderText = "Llave";
                column.Name = "ColumnKey";
                column.Items.AddRange(new string[] {"K"+Atributo.KP,"None"});
                column.Width = 50;
                column.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                columnName.HeaderText = "Nombre";
                columnName.Name = "ColumnName";
                columnName.Width = 75;
                columnName.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                columnType.HeaderText = "Tipo";
                columnType.Name = "ColumnType";
                columnType.Items.AddRange(new string[] { Atributo.entero, Atributo.flotante,Atributo.caracter,Atributo.cadena });
                columnType.Width = 50;
                columnType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                columnType.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                columnCamp.HeaderText = "Campo";
                columnCamp.Name = "ColumnCamp";
                columnCamp.Items.AddRange(new string[] { Atributo.CR, Atributo.NR });
                columnCamp.Width = 50;
                columnCamp.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                columnCamp.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                columnComment.HeaderText = "Comentario";
                columnComment.Name = "ColumnComment";
                columnComment.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                
                
                ((DataGridView)base.controlPrincipal).Columns.Add(column);

                ((DataGridView)base.controlPrincipal).Columns.Add(columnName);
                ((DataGridView)base.controlPrincipal).Columns.Add(columnType);
                ((DataGridView)base.controlPrincipal).Columns.Add(columnCamp);
                ((DataGridView)base.controlPrincipal).Columns.Add(columnComment);

            }

            public void muestraControles(bool band)
            {
                base.label.Visible = band;
                base.controlPrincipal.Visible = band;
                this.cbEntidades.Visible = band;
            }

            public List<Atributo> atributos()
            {
                List<Atributo> listAtr = new List<Atributo>();
                Atributo aAtr;

                try
                {
                    foreach (DataGridViewRow row in ((DataGridView)base.controlPrincipal).Rows)
                    {
                        if (row.Cells["ColumnName"].Value != null)
                        {

                            aAtr = new Atributo();
                            aAtr.llave = row.Cells[0].Value.ToString()=="KP"? Atributo.KP :Atributo.None;
                            aAtr.nombre = row.Cells[1].Value.ToString();
                            aAtr.tipo = row.Cells[2].Value.ToString();
                            aAtr.campo = row.Cells[3].Value.ToString();
                            if (row.Cells["ColumnComment"].Value != null)
                            {
                                aAtr.comentario = row.Cells[4].Value.ToString();
                            }
                            else
                            {
                                aAtr.comentario = "";
                            }
                            listAtr.Add(aAtr);
                        }
                    }
                }
                catch(NullReferenceException x)
                {
                    MessageBox.Show(x.Message);
                }

                return listAtr;
            }

            public void agregaUltimosAtr()
            {
                Entidad aEnt = this.ListEnt.Find(a => a.nombre.Equals(this.cbEntidades.Items[this.numEnt]));

                if (aEnt != null)
                {
                    aEnt.agregaAtributos(this.atributos());
                }
            }
        }
    }
}
