using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BaseDeDatos
{
    class Secuencial : Organizacion
    {
        public Secuencial(string nombre, Usuario us) : base(nombre, us, "Secuencial")
        {

        }
        #region ----------------------------------ENTIDADES-----------------------------------------------------
        public bool agregaEntidades(List<Entidad> listEnt)
        {
            bool band = false;

            if (listEnt != null)
            {
                foreach (Entidad ent in listEnt)
                {
                    band = base.altaEntidad(ent);
                    if (band)
                    {
                        foreach (Atributo atr in ent.listAtr)
                        {
                            this.altaAtributo(ent, atr, false);
                        }
                    }
                }
            }


            return band;
        }

        /// <summary>
        /// Lee una entidad que se encuentra en una determinada posición del archivo
        /// </summary>
        /// <param name="pos">Posición de la entidad en el archivo</param>
        /// <returns>Regresa la entidad busacada</returns>
        protected override Entidad leeEntidad(long pos)
        {
            EntSecuencial aEnt = new EntSecuencial();

            try
            {
                using (FileStream fs = new FileStream(base.ruta, FileMode.Open))
                {
                    fs.Seek(pos, SeekOrigin.Begin);
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        aEnt.nombre = br.ReadString();
                        aEnt.apAtr = br.ReadInt64();
                        aEnt.apBloq = br.ReadInt64();
                        aEnt.apRef = br.ReadInt64();
                        aEnt.sigEnt = br.ReadInt64();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            return aEnt;
        }

        /// <summary>
        /// Lee una entidad que se encuentra en una determinada posición del archivo
        /// </summary>
        /// <param name="pos">Posición de la entidad en el archivo</param>
        /// <param name="path">Ruta del archivo</param>
        /// <returns>Regresa la entidad busacada</returns>
        protected override Entidad leeEntidad(long pos,string path)
        {
            EntSecuencial aEnt = new EntSecuencial();

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    fs.Seek(pos, SeekOrigin.Begin);
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        aEnt.nombre = br.ReadString();
                        aEnt.apAtr = br.ReadInt64();
                        aEnt.apBloq = br.ReadInt64();
                        aEnt.apRef = br.ReadInt64();
                        aEnt.sigEnt = br.ReadInt64();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            return aEnt;
        }


        /// <summary>
        /// Graba en el archivo la entidad
        /// </summary>
        /// <param name="path">Ruta del archivo</param>
        /// <param name="ent">Entidad a grabar en el archivo</param>
        /// <returns></returns>
        protected override long insertaEntidad(Entidad ent)
        {
            long pos = 0;

            try
            {
                using (FileStream fs = new FileStream(base.ruta, FileMode.Append))
                {
                    pos = fs.Position;
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(ent.nombre);
                        bw.Write((ent as EntSecuencial).apAtr);
                        bw.Write((ent as EntSecuencial).apBloq);
                        bw.Write((ent as EntSecuencial).apRef);
                        bw.Write(ent.sigEnt);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            return pos;
        }

        /// <summary>
        /// Reescribe la entidad en el archivo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ent"></param>
        /// <param name="pos"></param>
        protected override bool reescribeEntidad(Entidad ent, long pos)//  <=============Cuidado al reescribir el  nombre==========
        {
            bool band = false;

            try
            {
                using (FileStream fs = new FileStream(base.ruta, FileMode.Open, FileAccess.Write))
                {
                    fs.Seek(pos, SeekOrigin.Begin);
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(ent.nombre);
                        bw.Write((ent as EntSecuencial).apAtr);
                        bw.Write((ent as EntSecuencial).apBloq);
                        bw.Write((ent as EntSecuencial).apRef);
                        bw.Write(ent.sigEnt);
                    }
                }
                band = true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            return band;
        }



        #endregion

        #region-----------------------------------------------------ATRIBUTOS-----------------------------------------------------

        public override bool altaAtributo(string nomEnt, Atributo atr, bool orden)
        {
            bool band = false;
            Entidad aEnt;

            aEnt = this.buscaEntidad(nomEnt);
            this.altaAtributo(aEnt, atr, orden);

            return band;
        }

        /// <summary>
        /// Da de alta un atributo de una entidad
        /// </summary>
        /// <param name="nomEnt">Entidad en la que se va a crear el atributo</param>
        /// <param name="nombreAtr">nombre del atributo</param>
        /// <param name="tipo">tipo del atributo</param>
        /// <returns></returns>
        public bool altaAtributo(Entidad ent, Atributo atr, bool orden)
        {
            bool band = false;

            //            if (this.activeUser.permisos[1] == true)
            //            {
            if (this.buscaAtributo(ent, atr.nombre) == null)
            {
                if (orden)
                {
                    band = this.insertaAtrPrincipio(ent, atr);
                }
                else
                {
                    band = this.insertaAtrFinal(ent, atr);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Ya existe un atributo con el mismo nombre");
            }
            /*            }
                                    else
                                    {
                                        System.Windows.Forms.MessageBox.Show("No tienes permiso para altas");
                                    }
            */

            return band;
        }

        /// <summary>
        /// Lee un atributo del archivo
        /// </summary>
        /// <param name="pos">Posicion del atributo en el archivo</param>
        /// <returns>Regresa el atributo</returns>
        public override Atributo leeAtributo(long pos)
        {
            Atributo aAtr = new Atributo();

            try
            {
                using (FileStream fs = new FileStream(base.ruta, FileMode.Open))
                {
                    fs.Seek(pos, SeekOrigin.Begin);
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        aAtr.llave = br.ReadChar();
                        aAtr.nombre = br.ReadString();
                        aAtr.tipo = br.ReadString();
                        aAtr.campo = br.ReadString();
                        aAtr.comentario = br.ReadString();
                        aAtr.sigAtr = br.ReadInt64();
                        aAtr.sigRel = br.ReadInt64();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            this.recuperaRelaciones(aAtr);

            return aAtr;
        }

        public override Atributo leeAtributo(long pos,string path)
        {
            Atributo aAtr = new Atributo();

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    fs.Seek(pos, SeekOrigin.Begin);
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        aAtr.llave = br.ReadChar();
                        aAtr.nombre = br.ReadString();
                        aAtr.tipo = br.ReadString();
                        aAtr.campo = br.ReadString();
                        aAtr.comentario = br.ReadString();
                        aAtr.sigAtr = br.ReadInt64();
                        aAtr.sigRel = br.ReadInt64();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            this.recuperaRelaciones(aAtr);

            return aAtr;
        }



        /// <summary>
        /// Inserta al principio y liga el atributo
        /// </summary>
        /// <param name="ent"> Entidad que va a tener el atributo</param>
        /// <param name="atr"> atributo a insertar</param>
        /// <param name="atr"> Ruta del archivo</param>
        /// <returns>true si se insertó correctamente</returns>
        private bool insertaAtrPrincipio(Entidad ent, Atributo atr)
        {
            bool band = false;
            long posAtr, posEnt;


            atr.sigAtr = (ent as EntSecuencial).apAtr;
            posAtr = this.insertaAtributo(atr);
            (ent as EntSecuencial).apAtr = posAtr;
            posEnt = base.buscaPosEntidad(ent.nombre);
            if (posEnt != -1)
            {
                band = this.reescribeEntidad(ent, posEnt);
            }

            return band;
        }

        /// <summary>
        /// Inserta al final y liga el atributo
        /// </summary>
        /// <param name="ent">Entidad que va a tener el atributo</param>
        /// <param name="atr"> atributo a insertar</param>
        /// <param name="atr"> Ruta del archivo</param>
        /// <returns>true si se insertó correctamente</returns>
        private bool insertaAtrFinal(Entidad ent, Atributo atr)
        {
            bool band = false;
            long posAtr, posIt, posEnt;
            Atributo aAtr;

            posAtr = this.insertaAtributo(atr);
            if (posAtr != 0)
            {
                band = true;
            }
            if ((ent as EntSecuencial).apAtr == -1)
            {
                (ent as EntSecuencial).apAtr = posAtr;
                posEnt = this.buscaPosEntidad(ent.nombre);
                if (posEnt != -1)
                {
                    band = this.reescribeEntidad(ent, posEnt);
                }
            }
            else
            {
                posIt = (ent as EntSecuencial).apAtr;
                do
                {
                    aAtr = this.leeAtributo(posIt);
                    if (aAtr.sigAtr != -1)
                    {
                        posIt = aAtr.sigAtr;
                    }
                } while (aAtr.sigAtr != -1);
                aAtr.sigAtr = posAtr;
                band = this.reescribeAtributo(aAtr, posIt);
            }

            return band;
        }


        public override long insertaAtributo(Atributo atr)
        {
            long pos = 0;

            try
            {
                using (FileStream fs = new FileStream(base.ruta, FileMode.Append))
                {
                    pos = fs.Position;
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(atr.llave);
                        bw.Write(atr.nombre);
                        bw.Write(atr.tipo);
                        bw.Write(atr.campo);
                        bw.Write(atr.comentarioCompleto);
                        bw.Write(atr.sigAtr);
                        bw.Write(atr.sigRel);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                pos = 0;
            }

            return pos;
        }


        /// <summary>
        /// Busca un atributo existente en el archivo
        /// </summary>
        /// <param name="">Nombre de la entidad que contiene el atributo</param>
        /// <param name="">Nombre del atributo</param>
        /// <returns>Atributo.</returns>
        public override Atributo buscaAtributo(Entidad ent, string nomAtr)
        {
            Atributo aAtr = null;
            long pos;


            if (ent != null)
            {
                pos = (ent as EntSecuencial).apAtr;
                if (pos != -1)
                {
                    do
                    {
                        aAtr = this.leeAtributo(pos);
                        pos = aAtr.sigAtr;
                    }
                    while (!nomAtr.Equals(aAtr.nombre) && pos != -1);
                    if (!nomAtr.Equals(aAtr.nombre))
                    {
                        aAtr = null;
                    }
                }
            }

            return aAtr;
        }

        public override Atributo buscaAtributo(Entidad ent, string nomAtr,string path)
        {
            Atributo aAtr = null;
            long pos;


            if (ent != null)
            {
                pos = (ent as EntSecuencial).apAtr;
                if (pos != -1)
                {
                    do
                    {
                        aAtr = this.leeAtributo(pos,path);
                        pos = aAtr.sigAtr;
                    }
                    while (!nomAtr.Equals(aAtr.nombre) && pos != -1);
                    if (!nomAtr.Equals(aAtr.nombre))
                    {
                        aAtr = null;
                    }
                }
            }

            return aAtr;
        }

        public override long buscaPosAtributo(string nomEnt, string nomAtr)
        {
            Atributo aAtr = null;
            Entidad aEnt;
            long pos = -1;

            aEnt = this.buscaEntidad(nomEnt);
            if (aEnt != null)
            {
                pos = (aEnt as EntSecuencial).apAtr;
                if (pos != -1)
                {
                    do
                    {
                        aAtr = this.leeAtributo(pos);
                        if (aAtr.sigAtr != -1 && !aAtr.nombre.Equals(nomAtr))
                        {
                            pos = aAtr.sigAtr;
                        }
                    }
                    while (!nomAtr.Equals(aAtr.nombre));
                }
            }

            return pos;
        }

        public override long buscaPosAtributo(string nomEnt, string nomAtr,string path)
        {
            Atributo aAtr = null;
            Entidad aEnt;
            long pos = -1;

            aEnt = base.buscaEntidad(nomEnt,path);
            if (aEnt != null)
            {
                pos = (aEnt as EntSecuencial).apAtr;
                if (pos != -1)
                {
                    do
                    {
                        aAtr = this.leeAtributo(pos,path);
                        if (aAtr.sigAtr != -1 && !aAtr.nombre.Equals(nomAtr))
                        {
                            pos = aAtr.sigAtr;
                        }
                    }
                    while (!nomAtr.Equals(aAtr.nombre));
                }
            }

            return pos;
        }


        public override bool reescribeAtributo(Atributo atr, long pos)
        {
            bool band = true;

            try
            {
                using (FileStream fs = new FileStream(base.ruta, FileMode.Open, FileAccess.Write))
                {
                    fs.Seek(pos, SeekOrigin.Begin);
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(atr.llave);
                        bw.Write(atr.nombre);
                        bw.Write(atr.tipo);
                        bw.Write(atr.campo);
                        bw.Write(atr.comentarioCompleto);
                        bw.Write(atr.sigAtr);
                        bw.Write(atr.sigRel);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                band = false;
            }

            return band;
        }

        public override bool reescribeAtributo(Atributo atr, long pos,string path)
        {
            bool band = true;

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Write))
                {
                    fs.Seek(pos, SeekOrigin.Begin);
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(atr.llave);
                        bw.Write(atr.nombre);
                        bw.Write(atr.tipo);
                        bw.Write(atr.campo);
                        bw.Write(atr.comentarioCompleto);
                        bw.Write(atr.sigAtr);
                        bw.Write(atr.sigRel);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                band = false;
            }

            return band;
        }

        /// <summary>
        /// Regresa una lista de attributos con la entidad dada.
        /// </summary>
        /// <param name="nomEnt">Nombre de la entidad que contiene los atributos</param>
        /// <returns></returns>
        public override List<Atributo> listaAtributos(string nomEnt)
        {
            Entidad ent;
            Atributo aAtr;
            List<Atributo> listAtr = new List<Atributo>();
            long pos;

            ent = this.buscaEntidad(nomEnt);
            pos = (ent as EntSecuencial).apAtr;
            while (pos != -1)
            {
                aAtr = this.leeAtributo(pos);
                listAtr.Add(aAtr);
                pos = aAtr.sigAtr;
            }

            return listAtr;
        }

        /// <summary>
        /// Regresa una lista de attributos con la entidad dada.
        /// </summary>
        /// <param name="nomEnt">Nombre de la entidad que contiene los atributos</param>
        /// <returns></returns>
        public override List<Atributo> listaAtributos(string nomEnt, string path)
        {
            Entidad ent;
            Atributo aAtr;
            List<Atributo> listAtr = new List<Atributo>();
            long pos;

            ent = this.buscaEntidad(nomEnt, path);
            pos = (ent as EntSecuencial).apAtr;
            while (pos != -1)
            {
                aAtr = this.leeAtributo(pos, path);
                listAtr.Add(aAtr);
                pos = aAtr.sigAtr;
            }

            return listAtr;
        }



        /// <summary>
        /// guarda una relacion para un atributo que es KP
        /// </summary>
        /// <param name="nomEnt"></param>
        /// <param name="atr">atributo  que es KP</param>
        /// <param name="rel">relacion a guardar</param>
        /// <returns>true si se guardo false si hubo error</returns>
        public override bool altaRelacionEnKP(string nomEnt,Atributo atr,Relacion rel)
        {
            bool band = false;
            long pos, posAtr;

            //            if (this.activeUser.permisos[1] == true)
            //            {
            rel.apSigRef = atr.sigRel;
            pos = this.insertaRelacion(rel);
            if (pos != 0)
            {
                atr.sigRel = pos;
                posAtr = this.buscaPosAtributo(nomEnt, atr.nombre, Archivo.path +'\\'+ base.tipo + '\\' + rel.bd );
                band = this.reescribeAtributo(atr, posAtr,Archivo.path +'\\'+ base.tipo + '\\' + rel.bd);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error al guardar relacion");
            }
            
            
            /*            }
                                    else
                                    {
                                        System.Windows.Forms.MessageBox.Show("No tienes permiso para altas");
                                    }
            */

            return band;
        }

        /// <summary>
        /// guarda una relacion para un atributo que es KF, donde la nueva relacion debe tener la informacion de donde esta el atr KP
        /// </summary>
        /// <param name="nomEnt"></param>
        /// <param name="atr">atributo  que es KF</param>
        /// <param name="rel">relacion a guardar</param>
        /// <returns>true si se guardo false si hubo error</returns>
        public override bool altaRelacionEnKF(string nomEnt, Atributo atr, Relacion rel)
        {
            bool band = false;
            long pos, posAtr;

            //            if (this.activeUser.permisos[1] == true)
            //            {
            
            pos = this.insertaRelacion(rel);
            if (pos != 0)
            {
                atr.sigRel = pos;
                posAtr = this.buscaPosAtributo(nomEnt, atr.nombre);
                band = this.reescribeAtributo(atr, posAtr);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error al guardar relacion");
            }


            /*            }
                                    else
                                    {
                                        System.Windows.Forms.MessageBox.Show("No tienes permiso para altas");
                                    }
            */

            return band;
        }

        public long insertaRelacion(Relacion rel)
        {
            long pos = 0;

            try
            {
                using (FileStream fs = new FileStream(base.ruta, FileMode.Append))
                {
                    pos = fs.Position;
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(rel.bd);
                        bw.Write(rel.nomEnt);
                        bw.Write(rel.nomAtr);
                        bw.Write(rel.apSigRef);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                pos = 0;
            }

            return pos;
        }

        public void recuperaRelaciones(Atributo atr)
        {
            long apRel = atr.sigRel;
            Relacion rel;

            while (apRel != -1)
            {
                rel = this.leeRelacion(apRel);
                atr.agregaRelacion(rel);
                apRel = rel.apSigRef;
            }
        }

        public Relacion leeRelacion(long pos)
        {
            long sigRel = -1;
            string bd = "";
            string ent = "";
            string nomAtr = "";
            Relacion rel;

            try
            {
                using (FileStream fs = new FileStream(base.ruta, FileMode.Open))
                {
                    fs.Seek(pos, SeekOrigin.Begin);
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        bd = br.ReadString();
                        ent = br.ReadString();
                        nomAtr = br.ReadString();
                        sigRel = br.ReadInt64();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            rel = new Relacion(bd, ent, nomAtr, sigRel);

            return rel;
        }


        #endregion


        #region ----------------------------------BLOQUE-----------------------------------------------------

        public override bool altaBloque(Entidad ent, byte[] b)
        {
            long pos;
            bool band;

            pos = Archivo.escribeBloque(base.ruta, b);
            band = this.ligaBloque(ent, pos);

            return band;
        }

        private bool ligaBloque(Entidad ent, long pos)
        {
            bool band = false;
            long posEnt, posIt;
            byte[] bloq = null;
            long apSigBloq;

            if ((ent as EntSecuencial).apBloq == -1)
            {
                (ent as EntSecuencial).apBloq = pos;
                posEnt = this.buscaPosEntidad(ent.nombre);
                if (posEnt != -1)
                {
                    band = this.reescribeEntidad(ent, posEnt);
                }
            }
            else
            {
                posIt = (ent as EntSecuencial).apBloq;
                do
                {
                    bloq = Archivo.leeBloque(base.ruta, Bloque.calculaTamBloque(ent.listAtr), posIt);
                    apSigBloq = Bloque.leeApBloq(bloq);
                    if (apSigBloq != -1)
                    {
                        posIt = apSigBloq;
                    }
                } while (apSigBloq != -1);
                Bloque.reescribeApSigBloq(pos, bloq);
                band = Archivo.reescribeBloque(base.ruta, bloq, posIt);
            }

            return band;
        }

        /// <summary>
        /// regresa todos los registros de la entidad
        /// </summary>
        /// <param name="ent">Entidad con sus atributos</param>
        public override List<byte[]> listaBloques(Entidad ent)
        {
            List<byte[]> listBloq = new List<byte[]>();
            byte[] bloq;

            long pos = (ent as EntSecuencial).apBloq;
            int tamBloq = Bloque.calculaTamBloque(ent.listAtr);

            while (pos != -1)
            {
                bloq = Archivo.leeBloque(base.ruta, tamBloq, pos);
                listBloq.Add(bloq);
                pos = Bloque.leeApBloq(bloq);
            }

            return listBloq;
        }

        /// <summary>
        /// regresa todos los registros de la entidad
        /// </summary>
        /// <param name="ent">Entidad con sus atributos</param>
        public override List<byte[]> listaBloques(Entidad ent,string path)
        {
            List<byte[]> listBloq = new List<byte[]>();
            byte[] bloq;

            long pos = (ent as EntSecuencial).apBloq;
            int tamBloq = Bloque.calculaTamBloque(ent.listAtr);

            while (pos != -1)
            {
                bloq = Archivo.leeBloque(path, tamBloq, pos);
                listBloq.Add(bloq);
                pos = Bloque.leeApBloq(bloq);
            }

            return listBloq;
        }

        /// <summary>
        /// Busca un bloque que contenga el dato recibido como parametro
        /// </summary>
        /// <param name="ent">la entidad a la que pertenecen los datos</param>
        /// <param name="listAtr"> lista de atributos de la entidad</param>
        /// <param name="dato">dato a buscar en los bloques</param>
        /// <param name="nomAtr">nombre del atributo en el que se va a buscar la igualdad</param>
        /// <returns>si se encontro regresa el bloque en caso contrario regresa null</returns>
        public override byte[] buscaBloque(Entidad ent, List<Atributo> listAtr, string nomAtr, string dato)
        {
            ent.agregaAtributos(listAtr);
            List<byte[]> listBloq = this.listaBloques(ent);
            byte[] bloq = null;
            int posAtr = listAtr.FindIndex(a => a.nombre.Equals(nomAtr));

            foreach (byte[] b in listBloq)
            {
                if (Bloque.comparaDato(b,dato, posAtr,listAtr))
                {
                    bloq = b;
                    break;
                }
            }

            return bloq;
        }

        /// <summary>
        /// Busca un bloque que contenga el dato recibido como parametro
        /// </summary>
        /// <param name="ent">la entidad a la que pertenecen los datos</param>
        /// <param name="listAtr"> lista de atributos de la entidad</param>
        /// <param name="dato">dato a buscar en los bloques</param>
        /// <param name="nomAtr">nombre del atributo en el que se va a buscar la igualdad</param>
        /// <returns>si se encontro regresa el bloque en caso contrario regresa null</returns>
        public override byte[] buscaBloque(Entidad ent, List<Atributo> listAtr, string nomAtr, string dato,string path)
        {
            ent.agregaAtributos(listAtr);
            List<byte[]> listBloq = this.listaBloques(ent,path);
            byte[] bloq = null;
            int posAtr = listAtr.FindIndex(a => a.nombre.Equals(nomAtr));

            foreach (byte[] b in listBloq)
            {
                if (Bloque.comparaDato(b, dato, posAtr, listAtr))
                {
                    bloq = b;
                    break;
                }
            }

            return bloq;
        }


        public override bool eliminaRegistro(Entidad ent, byte[] bloq)
        {
            bool band = false;
            byte[] bloqAnt=null;
            List<byte[]> listBloq= this.listaBloques(ent);
            long pos = -1;

            for(int i=0;i<listBloq.Count && !band;i++)
            {
                band = Bloque.comparaBloque(listBloq[i], bloq,ent.listAtr);
                if (band)
                {
                    if (i == 0)
                    {
                        (ent as EntSecuencial).apBloq = Bloque.leeApBloq(listBloq[i]);
                        this.reescribeEntidad(ent, this.buscaPosEntidad(ent.nombre));
                    }
                    else
                    {
                        Bloque.reescribeApSigBloq(Bloque.leeApBloq(listBloq[i]), bloqAnt);
                        Archivo.reescribeBloque(base.ruta,bloqAnt, pos);
                        band = true;
                    }
                }
                if (i == 0)
                {
                    pos = (ent as EntSecuencial).apBloq;
                }
                else
                {
                    pos = Bloque.leeApBloq(bloqAnt);
                }
                bloqAnt = listBloq[i];
            }


            return band;

        }

        #endregion
    }


}
