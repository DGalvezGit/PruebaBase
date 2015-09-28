using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BaseDeDatos
{
    class LvEnt :Controles
    {
        VentanaPrincipal papi;

        public LvEnt(VentanaPrincipal f):base()
        {
            this.inicializa(f);
            base.tamañoPorcentajeY = 40;
            base.tamañoPorcentajeX = 10;
            papi = f;
        }

        private void inicializa(Form f)
        {
            base.controlPrincipal = new ListBox();
            base.inicializaControl(f, "Entidades:");
            ((ListBox)base.controlPrincipal).SelectedValueChanged += new EventHandler(this.selectedValueChanged);
        }

        public void limpiaControl()
        {
            ((ListBox)base.controlPrincipal).Items.Clear();
        }

        public void selectedValueChanged(object o, EventArgs e)
        {
            if ((o as ListBox).SelectedItem != null)
            {
                papi.agregaAcontrolAtributos(this.entidadSeleccionada());
            }
        }

        public void agregaEntidades(List<Entidad>listEnt)
        {
            ((ListBox)base.controlPrincipal).Items.Clear();
            foreach (Entidad ent in listEnt)
            {
                ((ListBox)base.controlPrincipal).Items.Add(ent.nombre);
            }
        }

        public void agregaEntidad(Entidad ent)
        {
            ((ListBox)base.controlPrincipal).Items.Add(ent.nombre);
        }


        public string entidadSeleccionada()
        {
            return ((ListBox)base.controlPrincipal).SelectedItem.ToString();
        }
    }
}
