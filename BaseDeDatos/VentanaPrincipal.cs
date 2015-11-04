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
    public partial class VentanaPrincipal : Form
    {
        private TreeviewArch controlArch;   //Clase que administra el label y el control TreeView que visualiza los archivos de las organizaciones
        private MenuCinta menu; //Clase que contiene la cinta menu que se encuentra en la parte superior de la ventana
        private LvEnt controlEnt;  //Clase que administra el label y el control DataGrid que visualiza las entidades
        private DgvAtr controlAtr;  //Clase que administra el label y el control DataGrid que visualiza los atributos
        private dgvDatos ControlDatos;
        public dgvDatos controlDatos
        {
            get { return this.ControlDatos; }
        }
        private uint sangriaIzq;
        private uint espacioEntreControles;
        private Organizacion Org;
        public Organizacion org
        {
            get { return this.Org; }
        }
        ToolStripStatusLabel labelComment;
        public bool orgAbierta
        {
            get { return this.Org != null ? true : false; }
        }
        

        public VentanaPrincipal()
        {
            this.sangriaIzq = 12;
            this.espacioEntreControles = 15;
            this.controlArch = new TreeviewArch(this);
            this.menu = new MenuCinta(this);
            this.controlEnt = new LvEnt(this);
            this.controlAtr = new DgvAtr(this);
            this.ControlDatos = new dgvDatos(this);
            this.Org = null;
            this.labelComment = new ToolStripStatusLabel();
            InitializeComponent();
        }

        private void VentanaPrincipal_Load(object sender, EventArgs e)
        {
            this.inicializaVentana();
            this.actualizaUbicacionControles();
            this.statusStrip1.Items.Add(this.labelComment);
        }

        private void inicializaVentana()
        {
            Rectangle screenSize = new Rectangle();

            screenSize = Screen.PrimaryScreen.Bounds;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(screenSize.Width*3/4,screenSize.Height*3/4);
            this.Location = new Point((screenSize.Width / 2) - this.Size.Width / 2, (screenSize.Height / 2 - this.Size.Height / 2));

        }

        private void redimencionaControles()
        {
            Point tamañoForma = new Point(this.ClientSize.Width,this.ClientSize.Height);

            this.SuspendLayout();
            tamañoForma.Y -= this.statusStrip1.Height;
            tamañoForma.Y -= this.menu.Height;
            tamañoForma.Y -= 4 * (int)this.espacioEntreControles;    // numero de controles multiplicado por el espacio que hay entre ellos
            this.controlArch.tamControl = new Point(tamañoForma.X * this.controlArch.TamañoPorcentaje().X / 100,
                                                    tamañoForma.Y * this.controlArch.TamañoPorcentaje().Y / 100);
            this.controlEnt.tamControl = new Point(tamañoForma.X * this.controlEnt.TamañoPorcentaje().X / 100,
                                                    tamañoForma.Y * this.controlEnt.TamañoPorcentaje().Y / 100);
            this.controlAtr.tamControl = new Point(tamañoForma.X * this.controlAtr.TamañoPorcentaje().X / 100,
                                                    tamañoForma.Y * this.controlAtr.TamañoPorcentaje().Y / 100);
            this.ControlDatos.tamControl = new Point(tamañoForma.X * this.ControlDatos.TamañoPorcentaje().X / 100,
                                                    tamañoForma.Y * this.ControlDatos.TamañoPorcentaje().Y / 100);
            this.actualizaUbicacionControles();

            this.ResumeLayout();
        }

        private void actualizaUbicacionControles()
        {
            int mayor =  this.controlAtr.coordenadas.Width;

            this.controlArch.ubicacion((int)this.sangriaIzq, this.menu.Height + (int)this.espacioEntreControles);
            this.controlEnt.ubicacion((int)this.sangriaIzq, this.controlArch.coordenadas.Height + (int)this.espacioEntreControles);
            this.controlAtr.ubicacion((int)this.sangriaIzq, this.controlEnt.coordenadas.Height + (int)this.espacioEntreControles);
            if (mayor < this.controlEnt.coordenadas.Width)
            {
                mayor = this.controlEnt.coordenadas.Width;
            }
            if (mayor < this.controlArch.coordenadas.Width)
            {
                mayor = this.controlArch.coordenadas.Width;
            }
            this.ControlDatos.ubicacion(mayor+(int)this.espacioEntreControles, this.menu.Height + (int)this.espacioEntreControles*3);
            
        }

        private void VentanaPrincipal_SizeChanged(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.redimencionaControles();
            this.ResumeLayout();
        }

        /// <summary>
        /// Este metodo se llama desde un evento cuando se da click en crear nueva estructura
        /// </summary>
        public void creaEstructura()
        {
            Usuario us;

            using (CreaEstructura creaEst = new CreaEstructura())
            {

                creaEst.ShowDialog();

                if (creaEst.DialogResult == DialogResult.OK)
                {
                    us = creaEst.usuario();
                    this.abreOrganizacion('s', creaEst.nombreDB(), creaEst.usuario(), creaEst.entidades(),true);
                    
                }
                creaEst.Close();
            }
        }

        /// <summary>
        /// Método que abre una organización 
        /// </summary>
        /// <param name="tipo">Tipo de organización</param>
        /// <param name="nombre">Nombre de la base de datos</param>
        /// <param name="us">Log in de usuario</param>
        /// <param name="listEnt">Lista de entidades a insertar si se creó una nueva organizacion</param>
        /// <param name="nueva">variable booleana para indicar si la organización es nueva,si es true crea un usuario,de lo contrario 
        /// inicia con uno previamente validado</param>
        public void abreOrganizacion(char tipo,string nombre,Usuario us,List<Entidad>listEnt,bool nueva)
        {
            
            
            switch (tipo)
            {
                case 's':
                case 'S':
                    this.Org = new Secuencial(nombre + ".scl", us);
                    if (nueva)
                    {
                        this.Org.altaUsuario(new Usuario("admin", "admin", new bool[] { true, true, true, true }, new DateTime(01,01,0001), new DateTime(01, 01,0001)));
                        this.Org.altaUsuario(us);
                    }
                    
                    if (this.Org != null)
                    {
                        if (listEnt != null)
                        {
                            ((Secuencial)this.Org).agregaEntidades(listEnt);
                        }
                        
                    }
                break;
                case 'm':
                case 'M':

                break;
            }
            this.actualizaControles(us);
        }

        public void actualizaControles(Usuario us)
        {
            this.ControlDatos.actualizaUsuario(us.nombre);
            if (us.consulta)
            {
                this.controlEnt.agregaEntidades(this.Org.entidades());
            }
            else
            {
                this.controlEnt.limpiaControl();
                this.controlDatos.limpiaControl();
                this.controlAtr.limpiaControl();
            }
            this.controlArch.RefreshArch();
        }

        public bool verificaUsuario(Usuario aUsr)
        {
            bool band = false;

            if ((aUsr.nombre.Equals("admin") && aUsr.vigFin.ToShortDateString() == "01/01/0001") || aUsr.vigFin.CompareTo(DateTime.Now) > 0)
            {
                band = true;
            }
            else
            {
                MessageBox.Show("El usuario no está vigente");
            }

            return band;
        }

        public void cierraOrg()
        {
            this.Org = null;
            this.controlEnt.limpiaControl();
            this.controlAtr.limpiaControl();
            this.ControlDatos.limpiaControl();
            this.ControlDatos.actualizaUsuario("");
        }

        /// <summary>
        /// Busca los atributos de cierta entidad y los agrega al control de los atributos para ser mostrados
        /// </summary>
        /// <param name="nomEnt"></param>
        public void agregaAcontrolAtributos(string nomEnt)
        {
            List<Atributo> listAtr;

            listAtr = this.Org.listaAtributos(nomEnt);
            this.controlAtr.agregaAtributos(listAtr);
            this.ControlDatos.agregaColumnas(listAtr);
        }

        public string entidadSeleccionada()
        {
            return this.controlEnt.entidadSeleccionada();
        }

        public void agregaAcinta(string nomAtr)
        {
            Entidad ent;
            Atributo atr;

            ent = this.Org.buscaEntidad(this.entidadSeleccionada());
            atr = this.Org.buscaAtributo(ent, nomAtr);
            this.labelComment.Text = atr.comentario;
        }

        public void cambiaClaveAtributo(string nomAtr,char claveNueva)
        {
            bool band;

            string nomEnt = this.entidadSeleccionada();
            band = this.Org.cambiaClaveAtributo(nomEnt,nomAtr,claveNueva);
            if (band)
            {
                this.controlAtr.agregaAtributos(this.Org.listaAtributos(nomEnt));
            }
            else
            {
                MessageBox.Show("Error al cambiar la clave");
            }
        }

        public Usuario pideUsuario(string path)
        {
            bool band = false;
            Usuario us = null;
            dUser dUsr = new dUser();

            dUsr.visualizaControles(false);
            dUsr.Text = "Sign In";
            while (!band)
            {
                dUsr.ShowDialog();
                if (dUsr.DialogResult == DialogResult.OK)
                {
                    if (!dUsr.nombre.Equals(""))
                    {
                        us = Usuario.buscaUsuario(path,dUsr.nombre);
                        if (us != null)
                        {
                            if (us.contraseña.Equals(dUsr.contraseña))
                            {
                                band = true;
                            }
                            else
                            {
                                ((TextBox)dUsr.Controls["tbContra"]).Clear();
                                MessageBox.Show("Contraseña incorrecta");
                                dUsr.DialogResult = DialogResult.None;
                                band = false;
                            }
                        }
                        else
                        {
                            ((TextBox)dUsr.Controls["tbNombre"]).Clear();
                            ((TextBox)dUsr.Controls["tbContra"]).Clear();
                            MessageBox.Show("Nombre de usuario incorrecto");
                            dUsr.DialogResult = DialogResult.None;
                            band = false;
                        }
                    }
                }
                else
                {
                    us = null;
                    band = true;
                }
            }
            dUsr.Dispose();

            return us;
        }

        public void agregaDatos(string nomEnt)
        {
            this.controlDatos.insertaDatosDataGrid(this.org.buscaEntidad(nomEnt));
        }
        
        




    }
}
