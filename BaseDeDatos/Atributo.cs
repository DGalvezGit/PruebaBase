using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDeDatos
{
    public class Atributo
    {
        public const char KP = 'P';
        public const char KF = 'F';
        public const char None = 'N';
        public const string entero = "int";
        public const string flotante = "float";
        public const string caracter = "char";
        public const string cadena = "string";
        private string Nombre;
        public string nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        private char Llave;
        public char llave
        {
            get { return Llave; }
            set { Llave = value; }
        }
        private string Tipo;
        public string tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        private string Campo;
        public string campo
        {
            get { return this.Campo; }
            set { this.Campo = value; }
        }
        private string Comentario;
        public string comentario
        {
            get { return this.obtenComentario(); }
            set
            {
                if (value.Length <= 100)
                {
                    Comentario = value;
                    for (int i = Comentario.Length; i < 100; i++)
                    {
                        Comentario += "~";
                    }
                }
            }
        }
        public string comentarioCompleto
        {
            get{ return Comentario; }
        }
        private long SigAtr;
        public long sigAtr
        {
            get{ return this.SigAtr;}
            set{ this.SigAtr = value; }
        }

        public Atributo()
        {
            this.SigAtr = -1;
        }

        public Atributo(string nombre,char llave,string tipo,string campo,string comentario):this()
        {
            this.nombre = nombre;
            this.llave = llave;
            this.tipo = tipo;
            this.campo = campo;
            this.comentario = comentario;
            
        }

        private string obtenComentario()
        {
            string coment = "";

            for (int i = 0; this.Comentario[i] != '~'; i++)
            {
                coment += this.Comentario[i];
            }

            return coment;
        }
    }
}
