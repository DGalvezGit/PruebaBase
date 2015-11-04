using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatos
{
    class MenuCinta
    {
        MenuStrip menu;
        VentanaPrincipal ventanaPrinc;

        public int Height
        {
            get { return this.menu.Size.Height; }
        }

        public MenuCinta(VentanaPrincipal f)
        {
            this.ventanaPrinc = f;
            this.menu = new MenuStrip();
            f.Controls.Add(this.menu);
            this.agregaMenu();
        }

        private void agregaMenu()
        {
            ToolStripMenuItem[] subItems;
            ToolStripMenuItem usr = new ToolStripMenuItem("Usuarios");
            usr.DropDownItems.AddRange(new ToolStripMenuItem[] { new ToolStripMenuItem("Alta",null,agregaUsuarios_Click),
                                                                 new ToolStripMenuItem("Cambia usuario",null,cambiaUsr_Click)
                                                               }
                                      );

            // Create a MenuStrip control with a Estructuras subItems.
            #region
            subItems = new ToolStripMenuItem[]{ new ToolStripMenuItem("Crear", null, crearEstructura_Click) ,
                                                                     usr,
                                                                     new ToolStripMenuItem("Crear Afirmador",null,crearEstructura_Click),
                                                                     new ToolStripMenuItem("Crear Disparador",null,crearEstructura_Click)
                                                                    };
            
            ToolStripMenuItem estructurasMenu = new ToolStripMenuItem("Estructuras", null, subItems);
            #endregion
            // Create a MenuStrip control with a Mantenimiento subItems.
            #region
            subItems = new ToolStripMenuItem[]{ new ToolStripMenuItem("Alta de Datos", null, altaDatos_Click) ,
                                                                     new ToolStripMenuItem("Consultar", null, crearEstructura_Click),
                                                                     new ToolStripMenuItem("Eliminar", null, crearEstructura_Click),
                                                                     new ToolStripMenuItem("Modificar",null,crearEstructura_Click),
                                                                    };
            ToolStripMenuItem mantenimientoMenu = new ToolStripMenuItem("Mantenimiento",null,subItems);
            #endregion
            // Create a MenuStrip control with a sql subItems.
            #region
            subItems = new ToolStripMenuItem[]{ new ToolStripMenuItem("Sin novedades")};

            ToolStripMenuItem sqlMenu = new ToolStripMenuItem("SQL",null,subItems);
            
            #endregion
            
            ((ToolStripDropDownMenu)(estructurasMenu.DropDown)).ShowImageMargin = false;
            ((ToolStripDropDownMenu)(estructurasMenu.DropDown)).ShowCheckMargin = false;
            ((ToolStripDropDownMenu)(mantenimientoMenu.DropDown)).ShowImageMargin = false;
            ((ToolStripDropDownMenu)(mantenimientoMenu.DropDown)).ShowCheckMargin = false;
            ((ToolStripDropDownMenu)(sqlMenu.DropDown)).ShowImageMargin = false;
            ((ToolStripDropDownMenu)(sqlMenu.DropDown)).ShowCheckMargin = false;

            // Assign the ToolStripMenuItem that displays 
            // the list of child forms.
            // menu.MdiWindowListItem = windowMenu;-<---<------<-----<----<-------<-----<-----<---<---<-----<

            // Add the window ToolStripMenuItem to the MenuStrip.
            menu.Items.Add(estructurasMenu);
            menu.Items.Add(mantenimientoMenu);
            menu.Items.Add(sqlMenu);

            // Dock the MenuStrip to the top of the form.
            menu.Dock = DockStyle.Top;
        }

        void crearEstructura_Click(object sender, EventArgs e)
        {
            this.ventanaPrinc.creaEstructura();
        }

        void cambiaUsr_Click(object sender, EventArgs e)
        {
            string ruta = this.ventanaPrinc.org.ruta.Remove(this.ventanaPrinc.org.ruta.Length - 4) + ".usr";
            Usuario usr;

            if(this.ventanaPrinc.orgAbierta)
            {
                usr = this.ventanaPrinc.pideUsuario(ruta);
                if (usr != null)
                {
                    if (this.ventanaPrinc.verificaUsuario(usr))
                    {
                        this.ventanaPrinc.org.cambiaUsuario(usr);
                        this.ventanaPrinc.actualizaControles(usr);
                    }
                }
            }
            
            
        }

        void agregaUsuarios_Click(object sender, EventArgs e)
        {
            Usuario us = null;
            dUser dUsr;

            if (this.ventanaPrinc.orgAbierta)
            {
                dUsr = new dUser();
                dUsr.privilegios(true);
                dUsr.ShowDialog();
                if (dUsr.DialogResult == DialogResult.OK)
                {
                    if (!dUsr.nombre.Equals("") && !dUsr.contraseña.Equals(""))
                    {
                        us = new Usuario(dUsr.nombre, dUsr.contraseña, dUsr.priv, dUsr.vigIni, dUsr.vigFin);
                        if (this.ventanaPrinc.orgAbierta)
                        {
                            this.ventanaPrinc.org.altaUsuario(us);
                        }
                    }
                }
                dUsr.Dispose();
            }
        }

        void consultaEstructura_Click(object sender,EventArgs e)
        {

        }

        /// <summary>
        /// Agrega datos desde el menu cinta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void altaDatos_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No implementado");
        }

        ~MenuCinta()
        {
            this.menu.Dispose();
        }


    }


}
