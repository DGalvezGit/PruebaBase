using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDeDatos
{
    public class Organizacion
    {
        private string Nombre;
        public string nombre
        {
            get { return this.Nombre; }
        }
        private Usuario Us;
        public Usuario usuario
        {
            get { return Us; }
        }
        private string Tipo;
        public string tipo
        {
            get { return this.Tipo; }
        }
        public string ruta
        {
            get { return Archivo.path + '\\' + this.tipo + '\\' + this.nombre; }
        }

        public Organizacion(string nombre, Usuario us, string tipo)
        {
            this.Nombre = nombre;
            this.Tipo = tipo;
            this.inicializa();
            this.Us = us;
        }

        private void inicializa()
        {
            Archivo.existeDirectorio(Archivo.path+'\\'+this.tipo);
            Archivo.inicializaArch(this.ruta);
        }

        public bool altaUsuario(Usuario us)
        {
            bool band = false;

            if (us != null)
            {
                band = us.altaUsuario(this.ruta.Remove(this.ruta.Length - 4) + ".usr");
            }

            return band;
        }

        public void cambiaUsuario(Usuario usr)
        {
            this.Us = usr;
        }

        #region ----------------------------------------ENTIDADES--------------------------------

        /// <summary>
        /// Verifica si no existe una entidad con el mismo nombre
        /// e inserta la entidad.
        /// </summary>
        /// <param name="nomEnt">nombre del archivo</param>
        /// <returns>si se insertó correctamente regresa true</returns>
        public bool altaEntidad(Entidad ent)
        {
            bool band = false;
            long pos;

            if (this.usuario.permisos[1] == true)
            {
                if (!this.existeEntidad(ent.nombre))
                {
                    pos = this.insertaEntidad(ent);
                    this.ligaEntidad(pos);
                    band = true;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Entidad ya existente");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No tienes permiso para altas");
            }

            return band;
        }

        /// <summary>
        /// Busca una entidad existente en el archivo
        /// </summary>
        /// <param name="nomEnt">Nombre de la entidad a buscar</param>
        /// <returns>Si encontró la entidad regresa true, de lo contrario regresa false</returns>
        public bool existeEntidad(string nomEnt)
        {
            bool band = false;
            long pos;
            Entidad aEnt;

            pos = Archivo.dameCab(this.ruta);
            if (pos != 0)
            {
                while (pos > 0 && !band)
                {
                    aEnt = this.leeEntidad(pos);
                    if (string.Equals(nomEnt, aEnt.nombre))
                    {
                        band = true;
                    }
                    else
                    {
                        pos = aEnt.sigEnt;
                    }
                }
            }

            return band;
        }

        public long buscaPosEntidad(string nomEnt)
        {
            long pos;
            Entidad aEnt = null;

            pos = Archivo.dameCab(this.ruta);
            if (pos != -1)
            {
                aEnt = this.leeEntidad(pos);
                while (aEnt.nombre != nomEnt && pos != -1)
                {
                    pos = aEnt.sigEnt;
                    if (pos != -1)
                    {
                        aEnt = this.leeEntidad(pos);
                    }
                }
            }

            return pos;
        }

        /// <summary>
        /// Busca una entidad existente en el archivo
        /// </summary>
        /// <param name="nomEnt">Nombre de la entidad a buscar</param>
        /// <returns>Si encontró la entidad regresa true, de lo contrario regresa false</returns>
        public Entidad buscaEntidad(string nomEnt)
        {
            bool band = false;
            long pos;
            Entidad aEnt = null;

            pos = Archivo.dameCab(this.ruta);
            if (pos != 0)
            {
                while (pos > 0 && !band)
                {
                    aEnt = this.leeEntidad(pos);
                    if (string.Equals(nomEnt, aEnt.nombre))
                    {
                        band = true;
                    }
                    else
                    {
                        pos = aEnt.sigEnt;
                    }
                }
            }

            return aEnt;
        }

        /// <summary>
        /// Liga la entidad a las otras entidades del archivo
        /// </summary>
        /// <param name="posEnt">posicion en la que se insertó</param>
        private void ligaEntidad(long posEnt)
        {
            long pos;
            Entidad aEnt;

            pos = Archivo.dameCab(this.ruta);

            if (pos == -1)
            {
                Archivo.reescribeCab(this.ruta, posEnt);
            }
            else
            {
                aEnt = this.leeEntidad(pos);
                while (aEnt.sigEnt != -1)
                {
                    pos = aEnt.sigEnt;
                    aEnt = this.leeEntidad(pos);
                }
                aEnt.sigEnt = posEnt;
                this.reescribeEntidad(aEnt, pos);
            }
        }

        protected virtual Entidad leeEntidad(long pos)
        {
            return null;
        }

        protected virtual long insertaEntidad(Entidad ent)
        {
            return -1;
        }

        protected virtual bool reescribeEntidad(Entidad ent, long pos)
        {
            return false;
        }

        public List<Entidad> entidades()
        {
            long pos;
            Entidad aEnt;
            List<Entidad> listEnt = new List<Entidad>();

            pos = Archivo.dameCab(this.ruta);
            while (pos != -1)
            {
                aEnt = this.leeEntidad(pos);
                listEnt.Add(aEnt);
                pos = aEnt.sigEnt;

            }
            
            return listEnt;
        }

        #endregion
        #region-----------------------ATRIBUTOS-------------------------------

        public bool cambiaClaveAtributo(string nomEnt,string nomAtr,char claveNueva)
        {
            bool band = false;
            Atributo atr;
            
            atr = this.buscaAtributo(this.buscaEntidad(nomEnt), nomAtr);
            
            switch (claveNueva)
            {
                case Atributo.KP:
                    if (claveNueva == atr.llave)
                    {
                        System.Windows.Forms.MessageBox.Show("Verificar que no tenga llaves foraneas o eliminarlas");
                        claveNueva = Atributo.None;
                    }
                    else
                    {
                        atr.llave = claveNueva;
                    }
                    band = true;
                break;
                case Atributo.KF:
                    if (atr.llave == Atributo.None)
                    {
                        using (ClaveForanea cf = new ClaveForanea(this))
                        {
                            cf.ShowDialog();
                            if (cf.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                atr.llave = claveNueva;
                                atr.comentario = atr.comentario + "\t:" + cf.db() + ':' + cf.ent() + ':' + cf.atr();
                                band = true;
                            }
                        }
                    }
                    else
                    {
                        atr.llave = Atributo.None;
                        atr.comentario = atr.comentario.Remove(atr.comentario.IndexOf('\t'));
                        band = true;
                    }
                break;
                case Atributo.None:
                    atr.llave = claveNueva;
                    atr.comentario = atr.comentario.Remove(atr.comentario.IndexOf('\t'));
                    band = true;
                break;
            }
            if (band)
            {
                this.reescribeAtributo(atr, this.buscaPosAtributo(nomEnt, atr.nombre));
            }

            return band;
        }

        public virtual bool altaAtributo(string nomEnt, Atributo atr, bool orden)
        {
            return false;
        }

        public virtual Atributo leeAtributo(long pos)
        {
            return null;
        }

        public virtual long insertaAtributo(Atributo atr)
        {
            return -1;
        }

        public virtual Atributo buscaAtributo(Entidad ent, string nomAtr)
        {
            return null;
        }

        public virtual long buscaPosAtributo(string nomEnt, string nomAtr)
        {
            return -1;
        }

        public virtual bool reescribeAtributo(Atributo atr, long pos)
        {
            return false;
        }

        public virtual List<Atributo> listaAtributos(string nomEnt)
        {
            return null;
        }

        #endregion

# region ---------------------------BLOQUE-----------------------------
        public virtual bool altaBloque(Entidad ent, byte[] b)
        {
            return false;
        }

        public virtual List<byte[]> listaBloques(Entidad ent)
        {
            return null;
        }

        public byte[] leeBloque(int tam,long pos)
        {
            return Archivo.leeBloque(this.ruta, tam, pos);
        }

#endregion
    }
}
