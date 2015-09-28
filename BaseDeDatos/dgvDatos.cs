using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatos
{
    class dgvDatos : Controles
    {

        public dgvDatos(Form f):base()
        {
            this.inicializa(f);
            base.tamañoPorcentajeX = 80;
            base.tamañoPorcentajeY = 100;
            
        }

        private void inicializa(Form f)
        {
            base.controlPrincipal = new DataGridView();
            base.inicializaControl(f,"");
            (base.controlPrincipal as DataGridView).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void limpiaControl()
        {
            ((DataGridView)base.controlPrincipal).Rows.Clear();
        }

        public void agregaColumnas(List<Atributo> listAtr)
        {
            (base.controlPrincipal as DataGridView).Columns.Clear();
            foreach (Atributo atr in listAtr)
            {
                (base.controlPrincipal as DataGridView).Columns.Add(atr.nombre,atr.nombre);
            }
        }


        public void actualizaUsuario(string nombre)
        {
            base.label.Text = nombre;
            base.label.Size = (TextRenderer.MeasureText(base.label.Text, base.label.Font));
            base.label.Location = new System.Drawing.Point(base.controlPrincipal.Location.X, base.controlPrincipal.Location.Y - base.label.Size.Height);
            
        }

    }
}
