using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace BaseDeDatos
{
    class TreeviewArch : Controles
    {
        VentanaPrincipal papi;

        public TreeviewArch(VentanaPrincipal f):base()
        {
            this.papi = f;
            this.inicializa(f);
            base.tamañoPorcentajeY = 20;
            base.tamañoPorcentajeX = 15;
        }

        private void inicializa(Form f)
        {
            base.controlPrincipal = new TreeView();
            base.inicializaControl(f,"Bases de Datos:");
            ((TreeView)base.controlPrincipal).NodeMouseDoubleClick += this.tvArch_NodeMouseDoubleClick;
            this.RefreshArch();
        }

        

        /// <summary>
        /// Actualiza el control arbol de archivos.
        /// </summary>
        public void RefreshArch()
        {
            DirectoryInfo df = new DirectoryInfo(Archivo.path);

            ((TreeView)base.controlPrincipal).Nodes.Clear();
            try
            {
                Archivo.existeDirectorio(Archivo.path);
                DirectoryInfo[] dirinfo = df.GetDirectories();

                for (int i = 0; i < dirinfo.Length; i++)
                {
                    ((TreeView)base.controlPrincipal).Nodes.Add(dirinfo[i].Name);
                    //if(dirinfo[i].GetFiles().Length > 0)
                    foreach (FileInfo f in dirinfo[i].GetFiles())
                    {
                        if (f.Name.Substring(f.Name.Length - 4) != ".usr" && f.Name.Substring(f.Name.Length - 4) != ".rel")
                        {
                            ((TreeView)base.controlPrincipal).Nodes[i].Nodes.Add(f.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Al dar doble click en el arbol de archivos
        /// abre la organización del archivo seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvArch_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
//            User usr, aUsr;
            TreeNode node = e.Node;
//            string name;

            if (node.Text != "Multilistas" && node.Text != "Secuencial")
            {
                this.abreOrganizacion(node.Parent.Text, node.Text.Remove(node.Text.Length - 4));
            }
        }

        /// <summary>
        /// Método que se llama cuando se va abrir una organización ya existente
        /// </summary>
        /// <param name="tipo">tipo de la organización a abrir</param>
        /// <param name="nombre"> nombre de la organización a abrir</param>
        private void abreOrganizacion(string tipo, string nombre)
        {
            Usuario aUsr;

            this.verificaOrgAbierta(nombre);
            if (!this.papi.orgAbierta)
            {
                aUsr = this.papi.pideUsuario(Archivo.path + '\\' + tipo + '\\' + nombre + ".usr");
                if (aUsr != null)
                {
                    if (this.papi.verificaUsuario(aUsr))
                    {
                        this.papi.abreOrganizacion(tipo[0], nombre, aUsr, null, false);
                    }
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

            if (this.papi.orgAbierta)
            {
                if (this.papi.org.nombre.Remove(this.papi.org.nombre.Length - 4) != orgAbrir)
                {
                    if (MessageBox.Show("¿Seguro que quieres cerrar esta organización?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        this.papi.cierraOrg();
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

    }
}
