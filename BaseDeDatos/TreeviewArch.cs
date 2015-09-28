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
                this.papi.abreOrganizacion(node.Parent.Text, node.Text.Remove(node.Text.Length - 4));
            }
        }


    }
}
