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
        private dgvDatos controlDatos;
        private uint sangriaIzq;
        private uint espacioEntreControles;
        private Organizacion org;
        ToolStripStatusLabel labelComment;
        

        public VentanaPrincipal()
        {
            this.sangriaIzq = 12;
            this.espacioEntreControles = 15;
            this.controlArch = new TreeviewArch(this);
            this.menu = new MenuCinta(this);
            this.controlEnt = new LvEnt(this);
            this.controlAtr = new DgvAtr(this);
            this.controlDatos = new dgvDatos(this);
            this.org = null;
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

            tamañoForma.Y -= this.statusStrip1.Height;
            tamañoForma.Y -= this.menu.Height;
            tamañoForma.Y -= 4 * (int)this.espacioEntreControles;    // numero de controles multiplicado por el espacio que hay entre ellos
            this.controlArch.tamControl = new Point(tamañoForma.X * this.controlArch.TamañoPorcentaje().X / 100,
                                                    tamañoForma.Y * this.controlArch.TamañoPorcentaje().Y / 100);
            this.controlEnt.tamControl = new Point(tamañoForma.X * this.controlEnt.TamañoPorcentaje().X / 100,
                                                    tamañoForma.Y * this.controlEnt.TamañoPorcentaje().Y / 100);
            this.controlAtr.tamControl = new Point(tamañoForma.X * this.controlAtr.TamañoPorcentaje().X / 100,
                                                    tamañoForma.Y * this.controlAtr.TamañoPorcentaje().Y / 100);
            this.controlDatos.tamControl = new Point(tamañoForma.X * this.controlDatos.TamañoPorcentaje().X / 100,
                                                    tamañoForma.Y * this.controlDatos.TamañoPorcentaje().Y / 100);
            this.actualizaUbicacionControles();

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
            this.controlDatos.ubicacion(mayor+(int)this.espacioEntreControles, this.menu.Height + (int)this.espacioEntreControles*3);
            
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
                    this.abreCreaOrganizacion('s', creaEst.nombreDB(), creaEst.usuario(), creaEst.entidades());
                    this.org.altaUsuario(us);
                    
                }
                creaEst.Close();
            }
        }

        private void abreCreaOrganizacion(char tipo,string nombre,Usuario us,List<Entidad>listEnt)
        {
            
            switch (tipo)
            {
                case 's':
                case 'S':
                    this.org = new Secuencial(nombre+".scl",us);
                    this.controlDatos.actualizaUsuario(us.nombre);
                    if (this.org != null)
                    {
                        if (listEnt != null)
                        {
                            ((Secuencial)this.org).agregaEntidades(listEnt);
                        }
                        this.controlEnt.agregaEntidades(this.org.entidades());
                    }
                break;
                case 'm':
                case 'M':

                break;
            }
            this.controlArch.RefreshArch();
        }


        public void abreOrganizacion(string tipo, string nombre)
        {
            Usuario aUsr;

            this.verificaOrgAbierta(nombre);
            if (this.org == null)
            {
                aUsr = this.pideUsuario(Archivo.path+'\\'+tipo+'\\'+nombre+".usr");
                if (aUsr != null)
                {
                    this.abreCreaOrganizacion(tipo[0], nombre, aUsr, null);
                }
            }
        }

        
        /// <summary>
        /// Verifica si existe una organizacion abierta. Si cumple,
        /// pregunta si se desea cambiar de organización
        /// </summary>
        /// <returns>true si se desea cambiar de organización, de lo contrario regresa false</returns>
        private bool verificaOrgAbierta(string orgAbrir)
        {
            bool band = false;

            if (this.org != null)
            {
                if (this.org.nombre != orgAbrir)
                {
                    if (MessageBox.Show("¿Seguro que quieres cerrar esta organización?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        this.org = null;
                        this.controlEnt.limpiaControl();
                        this.controlAtr.limpiaControl();
                        this.controlDatos.limpiaControl();
                        band = true;
                    }
                }
                else
                {
                    MessageBox.Show("Organización actualmente abierta");
                }
            }
            return band;
        }

        /// <summary>
        /// Busca los atributos de cierta entidad y los agrega al control de los atributos para ser mostrados
        /// </summary>
        /// <param name="nomEnt"></param>
        public void agregaAcontrolAtributos(string nomEnt)
        {
            List<Atributo> listAtr;

            listAtr = this.org.listaAtributos(nomEnt);
            this.controlAtr.agregaAtributos(listAtr);
            this.controlDatos.agregaColumnas(listAtr);
        }

        public string entidadSeleccionada()
        {
            return this.controlEnt.entidadSeleccionada();
        }

        public void agregaAcinta(string nomAtr)
        {
            Entidad ent;
            Atributo atr;

            ent = this.org.buscaEntidad(this.entidadSeleccionada());
            atr = this.org.buscaAtributo(ent, nomAtr);
            this.labelComment.Text = atr.comentario;
        }

        public void cambiaClaveAtributo(string nomAtr,char claveNueva)
        {
            bool band;

            string nomEnt = this.entidadSeleccionada();
            band = this.org.cambiaClaveAtributo(nomEnt,nomAtr,claveNueva);
            if (band)
            {
                this.controlAtr.agregaAtributos(this.org.listaAtributos(nomEnt));
            }
            else
            {
                MessageBox.Show("Error al cambiar la clave");
            }
        }

        private Usuario pideUsuario(string path)
        {
            bool band = false;
            Usuario us = null;
            dUser dUsr = new dUser();

            dUsr.visualizaControles(false);
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
        




    }
}
