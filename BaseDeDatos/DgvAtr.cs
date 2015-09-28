using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BaseDeDatos
{
    class DgvAtr : Controles
    {
        VentanaPrincipal papi;
        ContextMenuStrip cmsClick;

        public DgvAtr(VentanaPrincipal f):base()
        {
            this.papi = f;
            this.inicializa(f);
            
            base.tamañoPorcentajeY = 40;
            base.tamañoPorcentajeX = 15;
            
        }

        private void inicializa(Form f)
        {
            base.controlPrincipal = new DataGridView();
            base.inicializaControl(f, "Atributos:");
            ((DataGridView)base.controlPrincipal).BackgroundColor = SystemColors.Control;
            ((DataGridView)base.controlPrincipal).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ((DataGridView)base.controlPrincipal).RowHeadersVisible = false;
            ((DataGridView)base.controlPrincipal).AllowUserToAddRows = false;
            ((DataGridView)base.controlPrincipal).AllowUserToDeleteRows = false;
            ((DataGridView)base.controlPrincipal).CellEnter += event_CellEnter;
            ((DataGridView)base.controlPrincipal).CellMouseClick += DgvAtr_CellMouseClick;

            this.agregaColumnas();
            
            this.inicializamenuContextual();
        }

        private void DgvAtr_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ((DataGridView)base.controlPrincipal).CurrentCell = ((DataGridView)base.controlPrincipal)[e.ColumnIndex, e.RowIndex];
                this.cmsClick.Show(Cursor.Position);
            }
            
        }

        private void inicializamenuContextual()
        {
            ToolStripMenuItem llavePrimaria = new ToolStripMenuItem("Clave Primaria", null, EventHAndler_cambiaClavePrimaria);
            ToolStripMenuItem llaveForanea = new ToolStripMenuItem("Clave Foranea", null, EventHAndler_cambiaClaveForanea);

            cmsClick = new ContextMenuStrip();
            

            cmsClick.Items.Add(llavePrimaria);
            cmsClick.Items.Add(llaveForanea);
            
        }
        
        /// <summary>
        /// Evento que se genera cuando se da click en la opcion del MenuContextSptrip
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void EventHAndler_cambiaClavePrimaria(object o, EventArgs e)
        {
            this.cambiaClave(Atributo.KP);
        }

        /// <summary>
        /// Evento que se genera cuando se da click en la opcion del MenuContextSptrip
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void EventHAndler_cambiaClaveForanea(object o, EventArgs e)
        {
            this.cambiaClave(Atributo.KF);
        }

        private void cambiaClave(char claveNueva)
        {
            int rowindex = (base.controlPrincipal as DataGridView).SelectedCells[0].RowIndex;

            string nomAtr = (base.controlPrincipal as DataGridView)["ColumnName", rowindex].Value.ToString();
            this.papi.cambiaClaveAtributo(nomAtr, claveNueva);
        }

        private void agregaColumnas()
        {
/*            DataGridViewImageColumn column = new DataGridViewImageColumn();
              column.HeaderText = "Llave";
              column.Name = "ColumnKey";
              ((DataGridView)base.controlPrincipal).Columns.Add(column);
*/
            ((DataGridView)base.controlPrincipal).Columns.Add("ColumnKey", "Llave"); //En lo que hay imagenes

            ((DataGridView)base.controlPrincipal).Columns.Add("ColumnName", "Nombre");
            ((DataGridView)base.controlPrincipal).Columns.Add("ColumnType", "Tipo");
            ((DataGridView)base.controlPrincipal).Columns.Add("ColumnCamp", "Campo");
        }

        public void limpiaControl()
        {
            ((DataGridView)base.controlPrincipal).Rows.Clear();
        }

        public void agregaAtributos(List<Atributo>listAtr)
        {
            ((DataGridView)base.controlPrincipal).Rows.Clear();

            foreach (Atributo atr in listAtr)
            {
                ((DataGridView)base.controlPrincipal).Rows.Add(new string[] 
                                                                { atr.llave!=Atributo.None?"K"+atr.llave.ToString():"",atr.nombre,
                                                                  atr.tipo,atr.campo});
            }
        }

        public void event_CellEnter(object o, DataGridViewCellEventArgs c)
        {
            papi.agregaAcinta(((DataGridView)base.controlPrincipal).Rows[c.RowIndex].Cells["ColumnName"].Value.ToString());
        }
    }
}
