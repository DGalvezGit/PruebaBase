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
    public partial class ClaveForanea : Form
    {
        Organizacion org;
        public ClaveForanea(Organizacion org)
        {
            this.org = org;
            InitializeComponent();
        }

        private void databases()
        {
            List<string> listStr = new List<string>();

            foreach(System.IO.FileInfo f in Archivo.informacionDirectorio(Archivo.path + '\\' + this.org.tipo).GetFiles())
            {
                if (f.Name.Substring(f.Name.Length - 4) != ".usr" && f.Name.Substring(f.Name.Length - 4) != ".rel")
                {
                    listStr.Add(f.Name);
                }
            }
            this.llenaComboBox(listStr, this.cbDB);
        }

        private void entidades(string nomOrg)
        {
            Organizacion orgAux;
            List<string> listEnt = new List<string>();

            orgAux = this.abreOrg(nomOrg);
            if (orgAux != null)
            {
                foreach (Entidad ent in orgAux.entidades())
                {
                    listEnt.Add(ent.nombre);
                }
                this.llenaComboBox(listEnt, cbEnt);
            }
        }

        private void atributos(string nomOrg,string nomEnt)
        {
            Organizacion orgAux;
            List<string> listAtr = new List<string>();

            orgAux = this.abreOrg(nomOrg);
            if (orgAux != null)
            {
                foreach (Atributo atr in orgAux.listaAtributos(nomEnt))
                {
                    if (atr.llave.Equals(Atributo.KP))
                    {
                        listAtr.Add(atr.nombre);
                    }
                }
                this.llenaComboBox(listAtr, cbAtr);
            }
        }

        private Organizacion abreOrg(string nomOrg)
        {
            Organizacion orgAux = null;
            string tipoOrg = nomOrg.Substring(nomOrg.Length - 4);
            switch (tipoOrg)
            {
                case ".scl":
                    orgAux = new Secuencial(nomOrg, this.org.usuario);
                    break;
                case ".mtl":

                    break;
            }

            return orgAux;
        }

        private void llenaComboBox(List<String> nombres,ComboBox cb)
        {
            cb.Items.Clear();
            cb.Items.AddRange(nombres.ToArray());
        }

        public string db()
        {
            return cbDB.SelectedItem.ToString();
        }
        public string ent()
        {
            return cbEnt.SelectedItem.ToString();
        }
        public string atr()
        {
            return cbAtr.SelectedItem.ToString();
        }

        private void cbDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.entidades(cbDB.SelectedItem.ToString());
        }

        private void ClaveForanea_Load(object sender, EventArgs e)
        {
            this.databases();
        }

        private void cbEnt_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.atributos(cbDB.SelectedItem.ToString(), cbEnt.SelectedItem.ToString());
        }
    }
}
