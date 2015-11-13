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
    public partial class DConsulta : Form
    {
        VentanaPrincipal f;
        Entidad selectedEnt;
        Atributo selectedAtr;

        public DConsulta()
        {
            InitializeComponent();
        }

        public DConsulta(VentanaPrincipal v)
        {
            this.f = v;
            InitializeComponent();
        }


        private void DConsulta_Load(object sender, EventArgs e)
        {
            foreach (Entidad ent in this.f.org.entidades())
            {
                this.cbEnt.Items.Add(ent.nombre);
            }
        }

        private void tbId_TextChanged(object sender, EventArgs e)
        {
            if (!this.tbId.Text.Equals(""))
            {
                if (cbRel.Items.Count > 0)
                {
                    this.consulta(this.tbId.Text.ToString());
                }
            }
        }

        private void consulta(string id)
        {
            Relacion rel = this.selectedAtr.listRel.Find(a => a.nomEnt.Equals(this.cbRel.SelectedItem.ToString()));
            Entidad entAux = this.f.org.buscaEntidad(rel.nomEnt, Archivo.path + '\\' + this.f.org.tipo + '\\' + rel.bd);
            entAux.agregaAtributos(this.f.org.listaAtributos(entAux.nombre, Archivo.path + '\\' + this.f.org.tipo + '\\' + rel.bd));

            dgvConsulta.Rows.Clear();
            dgvConsulta.Columns.Clear();

            foreach (Atributo atr in entAux.listAtr)
            {
                dgvConsulta.Columns.Add(atr.nombre, atr.nombre);
            }

            foreach (byte[] b in this.f.org.listaBloques(entAux, Archivo.path + '\\' + this.f.org.tipo + '\\' + rel.bd))
            {
                if (Bloque.comparaDato(b, this.tbId.Text.ToString(), entAux.listAtr.FindIndex(a=>a.nombre.Equals(this.selectedAtr.nombre)), entAux.listAtr))
                {
                    this.insertaDatosDataGridFila(b, entAux.listAtr);
                }
            }

        }

        private void cbEnt_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedEnt = this.f.org.buscaEntidad(this.cbEnt.SelectedItem.ToString());

            selectedEnt.agregaAtributos(this.f.org.listaAtributos(selectedEnt.nombre));
            selectedAtr = selectedEnt.listAtr.Find(a => a.llave.Equals(Atributo.KP));

            dgvConsulta.Rows.Clear();
            dgvConsulta.Columns.Clear();
            cbRel.Items.Clear();
            foreach (Relacion rel in selectedAtr.listRel)
            {
                cbRel.Items.Add(rel.nomEnt);
            }
            if (cbRel.Items.Count > 0)
            {
                cbRel.SelectedIndex = 0;
            }
        }

        private void insertaDatosDataGridFila(byte[] b, List<Atributo> listAtr)
        {
            int pos = 8;
            int indiceFila = dgvConsulta.Rows.Add();
            int tam = 0;

            for (int i = 0; i < listAtr.Count; i++)
            {
                switch (listAtr[i].tipo)
                {
                    case Atributo.entero:
                        dgvConsulta.Rows[indiceFila].Cells[i].Value = Bloque.convierteEntero(b, pos, ref tam);
                        break;
                    case Atributo.flotante:
                        dgvConsulta.Rows[indiceFila].Cells[i].Value = Bloque.convierteFlotante(b, pos, ref tam);
                        break;
                    case Atributo.caracter:
                        dgvConsulta.Rows[indiceFila].Cells[i].Value = Bloque.convierteChar(b, pos, ref tam);
                        break;
                    case Atributo.cadena:
                        dgvConsulta.Rows[indiceFila].Cells[i].Value = Bloque.convierteCadena(b, pos, ref tam);
                        break;
                }
                pos += tam;
            }
        }
    }
}
