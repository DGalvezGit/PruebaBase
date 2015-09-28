using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDeDatos
{
    public class Entidad
    {
        private string Nombre;
        public string nombre
        {
            get { return Nombre; }
            set { this.Nombre = value; }
        }
        private long SigEnt;
        public long sigEnt
        {
            get { return SigEnt; }
            set { this.SigEnt = value; }
        }
        private List<Atributo> ListAtr;
        public List<Atributo> listAtr
        {
            get { return ListAtr; }
        }

//----------------------------------------------------------------------
        public Entidad()
        {
            this.sigEnt = -1;
            this.ListAtr = new List<Atributo>();
        }

        public Entidad(string nombre) : this()
        {
            this.Nombre = nombre;
        }

        public Entidad(string nombre, List<Atributo> listAtr)
        {
            this.Nombre = nombre;
            this.ListAtr = listAtr;
        }
//-------------------------------------------------------------------------

        public void agregaAtributos(List<Atributo> listAtr)
        {
            this.ListAtr = listAtr;
        }

        public void agregaAtributo(Atributo atr)
        {
            this.listAtr.Add(atr);
        }

    }
}
