using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDeDatos
{
    public class Usuario
    {
        private string Nombre;
        public string nombre
        {
            get { return Nombre; }
        }
        private string Contraseña;
        public string contraseña
        {
            get { return this.Contraseña; }
        }
        private bool[] Permisos;
        public bool []permisos
        {
            get { return Permisos; }
            set { this.Permisos = value; }
        }
        private DateTime VigIni;
        public DateTime vigIni
        {
            get { return this.VigIni; }
            set { this.VigIni = value; }
        }
        private DateTime VigFin;
        public DateTime vigFin
        {
            get { return this.VigFin; }
            set { this.VigFin = value; }
        }
        public long sigUs;
        
        public Usuario(string nombre, string contraseña,bool[]permisos,DateTime vigIni, DateTime vigFin)
        {
            this.Nombre = nombre;
            this.Contraseña = contraseña;
            this.Permisos = permisos;//[0]Alta, [1]Baja,[2]Modificacion, [3]Consulta
            this.VigIni = vigIni;
            this.VigFin = vigFin;
            this.sigUs = -1;
        }

        public bool altaUsuario(string path)
        {
            bool band = false;
            long pos;

            if (this != null)
            {
                Archivo.inicializaArch(path);

                if (buscaUsuario(path,this.nombre) == null)
                {
                    pos = Archivo.altaUsuario(path, this);
                    if (pos > 0)
                    {
                        band = this.ligaUsr(path, pos);
                    }
                }
            }

            return band;
        }

        private bool ligaUsr(string path, long posUsr)
        {
            bool band = false;

            long pos;
            Usuario aUsr;

            pos = Archivo.dameCab(path);

            if (pos == -1)
            {
                Archivo.reescribeCab(path, posUsr);
                band = true;
            }
            else
            {
                aUsr = Archivo.leeUsuario(path, pos);
                if (aUsr != null)
                {
                    while (aUsr.sigUs != -1)
                    {
                        pos = aUsr.sigUs;
                        aUsr = Archivo.leeUsuario(path, pos);
                    }
                    aUsr.sigUs = posUsr;
                    band = Archivo.reescribeUsuario(path, aUsr, pos);
                }
            }

            return band;
        }

        public static Usuario buscaUsuario(string path,string nombre)
        {
            Usuario aUs = null;
            long pos = 0;


            pos = Archivo.dameCab(path);
            if (pos != -1)
            {
                aUs = Archivo.leeUsuario(path, pos);
                while (pos != -1 && !aUs.nombre.Equals(nombre))
                {
                    pos = aUs.sigUs;
                    if (pos != -1)
                    {
                        pos = aUs.sigUs;
                        aUs = Archivo.leeUsuario(path, pos);
                    }
                }
                if (!aUs.nombre.Equals(nombre))
                {
                    aUs = null;
                }
            }

            return aUs;
        }

    }
}
