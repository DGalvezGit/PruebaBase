using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDeDatos
{
    class EntSecuencial : Entidad
    {
        private long ApAtr;
        public long apAtr
        {
            get { return ApAtr; }
            set { this.ApAtr = value; }
        }
        private long ApBloq;
        public long apBloq
        {
            get { return ApBloq; }
            set { this.ApBloq = value; }
        }
        private long ApRef; // Indica donde tiene llaves foráneas
        public long apRef
        {
            get { return ApRef; }
            set { this.ApRef = value; }
        }

        public EntSecuencial():base()
        {
            this.ApAtr = -1;
            this.ApBloq = -1;
            this.ApRef = -1;

        }

        public EntSecuencial(string nombre) : base(nombre)
        {
            this.ApAtr = -1;
            this.ApBloq = -1;
            this.ApRef = -1;
        }

        public EntSecuencial(string nombre,List<Atributo>listAtr) : base(nombre , listAtr)
        {
            this.ApAtr = -1;
            this.ApBloq = -1;
            this.ApRef = -1;
        }
    }
}
