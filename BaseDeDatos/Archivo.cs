using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BaseDeDatos
{
    static class Archivo
    {
        static string Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Base De Datos";
        public static string path
        {
            get { return Path; }
        }


        /// <summary>
        /// Verifica si existe el directorio de la organización
        /// si no existe el directorio lo crea
        /// </summary>
        /// <param name="path">ruta del directorio</param>
        public static void existeDirectorio(string dir)
        {
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }


        /// <summary>
        /// Crea un archivo con el nombre ingresado e
        /// inicializa la cabecera en -1
        /// </summary>
        /// <param name="directorio">nombre del archivo</param>
        public static void inicializaArch(string directorio)
        {
            try
            {
                if (!File.Exists(directorio))
                {
                    using (FileStream fils = new FileStream(directorio, FileMode.CreateNew))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fils))
                        {
                            bw.Write((long)-1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Lee la cabecera del archivo especificado
        /// </summary>
        /// <param name="directorio">directorio y nombre del archivo donde se encuentra</param>
        /// <returns></returns>
        public static long dameCab(string directorio)
        {
            long cab = 0;

            using (FileStream fs = new FileStream(directorio, FileMode.Open))
            {
                fs.Seek(0, SeekOrigin.Begin);
                using (BinaryReader br = new BinaryReader(fs))
                {
                    cab = br.ReadInt64();
                }
            }
            return cab;
        }

        /// <summary>
        /// Reescribe el apuntador a las entidades
        /// </summary>
        /// <param name="directorio">directorio del tipo de archivo y el nombre del archivo</param>
        /// <param name="dir">Direccion(del archivo) de la entidad a insertar</param>
        public static void reescribeCab(string directorio, long dir)
        {
            try
            {
                using (FileStream fs = new FileStream(directorio, FileMode.Open, FileAccess.Write))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write((long)dir);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public static DirectoryInfo informacionDirectorio(string path)
        {
            DirectoryInfo df = new DirectoryInfo(path);

            return df;
        }

        //-----------------------------------------------------USUARIOS-----------------------------------------------------
        //-----------------------------------------------------USUARIOS-----------------------------------------------------
        //-----------------------------------------------------USUARIOS-----------------------------------------------------
        //-----------------------------------------------------USUARIOS-----------------------------------------------------


        public static long altaUsuario(string path, Usuario us)
        {
            long pos = 0;

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Append))
                {
                    pos = fs.Position;
                    escribeUsuario(fs, us);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                pos = 0;
            }

            return pos;
        }

        private static bool escribeUsuario(FileStream fs, Usuario us)
        {
            bool band = false;
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(us.nombre);
                bw.Write(us.contraseña);
                foreach (bool p in us.permisos)
                {
                    bw.Write(p);
                }
                bw.Write(us.vigIni.ToShortDateString());
                bw.Write(us.vigFin.ToShortDateString());
                bw.Write(us.sigUs);
                band = true;
            }

            return band;
        }

        public static Usuario leeUsuario(string path, long pos)
        {
            string nombre;
            string psw;
            DateTime dateIni, dateFin;
            bool[] permisos = new bool[4];
            long sig;
            Usuario usr = null;

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    fs.Seek(pos, SeekOrigin.Begin);
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        nombre = br.ReadString();
                        psw = br.ReadString();
                        for (int i = 0; i < 4; i++)
                        {
                            permisos[i] = br.ReadBoolean();
                        }
                        dateIni = DateTime.Parse(br.ReadString());
                        dateFin = DateTime.Parse(br.ReadString());
                        sig = br.ReadInt64();
                    }
                }
                usr = new Usuario(nombre, psw, permisos, dateIni, dateFin);
                usr.sigUs = sig;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }


            return usr;
        }

        public static bool reescribeUsuario(string path, Usuario us, long pos)
        {
            bool band = false;

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Write))
                {
                    fs.Seek(pos, SeekOrigin.Begin);
                    band = escribeUsuario(fs, us);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                band = false;
            }

            return band;
        }

        //-----------------------------------------------------BLOQUE-----------------------------------------------------
        //-----------------------------------------------------BLOQUE-----------------------------------------------------
        //-----------------------------------------------------BLOQUE-----------------------------------------------------
        //-----------------------------------------------------BLOQUE-----------------------------------------------------

        public static byte[] leeBloque(string ruta, int tam, long pos)
        {
            byte[] bloq = new byte[tam];

            try
            {
                using (FileStream fs = new FileStream(ruta, FileMode.Open))
                {
                    fs.Seek(pos, SeekOrigin.Begin);
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        bloq = br.ReadBytes(tam);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                bloq = null;
            }


            return bloq;
        }


        public static long escribeBloque(string ruta, byte[] bloq)
        {
            long pos = 0;

            try
            {
                using (FileStream fs = new FileStream(ruta, FileMode.Append))
                {
                    pos = fs.Position;
                    altaBloque(fs,bloq);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                pos = 0;
            }

            return pos;
        }

        public static bool reescribeBloque(string ruta, byte[] bloq,long pos)
        {
            bool band = true;

            try
            {
                using (FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Write))
                {
                    fs.Seek(pos, SeekOrigin.Begin);
                    altaBloque(fs, bloq);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                band = false;
            }

            return band;
        }

        public static void altaBloque(FileStream fs,byte[] b)
        {
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(b);
            }
        }
    }

}
