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

            // Create a MenuStrip control with a Estructuras subItems.
            #region
            subItems = new ToolStripMenuItem[]{ new ToolStripMenuItem("Crear", null, crearEstructura_Click) ,
                                                                     new ToolStripMenuItem("Consultar", null, consultaEstructura_Click),
                                                                     new ToolStripMenuItem("Agregar Usuarios", null, crearEstructura_Click),
                                                                     new ToolStripMenuItem("Crear Afirmador",null,crearEstructura_Click),
                                                                     new ToolStripMenuItem("Crear Disparador",null,crearEstructura_Click)
                                                                    };
            ToolStripMenuItem estructurasMenu = new ToolStripMenuItem("Estructuras", null, subItems);
            #endregion
            // Create a MenuStrip control with a Mantenimiento subItems.
            #region
            subItems = new ToolStripMenuItem[]{ new ToolStripMenuItem("Alta de Datos", null, crearEstructura_Click) ,
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

        void consultaEstructura_Click(object sender,EventArgs e)
        {

        }

        ~MenuCinta()
        {
            this.menu.Dispose();
        }
    }


}
