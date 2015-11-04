using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatos
{
    static class Bloque
    {
        private static int tamCad = 100;

        public static byte[] creaBloque(List<Atributo> listAtr, DataGridViewRow registro)
        {
            byte[] bloque;
            int tamBloq;
            int pos = 0;
            byte[] apBloq = BitConverter.GetBytes(((long)-1)); //Apuntador a siguiente bloque
            string cad;

            tamBloq = calculaTamBloque(listAtr);
            bloque = new byte[tamBloq];
            agregaDatoBloque(apBloq, sizeof(long), bloque, ref pos);
            for (int i = 0; i < listAtr.Count; i++)
            {
                switch (listAtr[i].tipo)
                {
                    case Atributo.entero:
                        if (registro.Cells[i].Value == null)
                        {
                            registro.Cells[i].Value = 0;
                        }
                        agregaDatoBloque(BitConverter.GetBytes(int.Parse(registro.Cells[i].Value.ToString())), sizeof(int), bloque, ref pos);
                        break;
                    case Atributo.flotante:
                        if (registro.Cells[i].Value == null)
                        {
                            registro.Cells[i].Value = 0;
                        }
                        agregaDatoBloque(BitConverter.GetBytes(float.Parse(registro.Cells[i].Value.ToString())), sizeof(float), bloque, ref pos);
                        break;
                    case Atributo.caracter:
                        if (registro.Cells[i].Value == null)
                        {
                            registro.Cells[i].Value = '-';
                        }
                        agregaDatoBloque(BitConverter.GetBytes(char.Parse(registro.Cells[i].Value.ToString())), sizeof(char) / 2, bloque, ref pos);
                        break;
                    case Atributo.cadena:
                        if (registro.Cells[i].Value == null)
                        {
                            registro.Cells[i].Value = '-';
                        }
                        cad = registro.Cells[i].Value.ToString();
                        for (int j = cad.Length; j < tamCad; j++)
                        {
                            cad += '~';
                        }
                        agregaDatoBloque(convierteCadena(cad, (sizeof(char) / 2) * cad.Length), (sizeof(char) / 2) * cad.Length, bloque, ref pos);
                        break;
                }
            }

            return bloque;
        }

        private static byte[] convierteCadena(string cad, int tam)
        {
            byte[] b = new byte[tam];

            for (int i = 0; i < cad.Length; i++)
            {
                b[i] = BitConverter.GetBytes(cad[i])[0];
            }

            return b;
        }

        private static void agregaDatoBloque(byte[] dato, int tam, byte[] bloque, ref int pos)
        {
            for (int i = 0; i < tam; i++)
            {
                bloque[pos + i] = dato[i];
            }
            pos += tam;
        }

        public static int calculaTamBloque(List<Atributo> listAtr)
        {
            int tam = 0;

            foreach (Atributo atr in listAtr)
            {
                switch (atr.tipo)
                {
                    case Atributo.entero:
                        tam += sizeof(int);
                        break;
                    case Atributo.flotante:
                        tam += sizeof(float);
                        break;
                    case Atributo.caracter:
                        tam += sizeof(char) / 2;
                        break;
                    case Atributo.cadena:
                        tam += (sizeof(char) / 2) * tamCad;
                        break;
                }
            }
            tam += sizeof(long); // tamaño del apuntador al siguiente bloque

            return tam;
        }

        public static void convierteBloque(byte[] b, List<Atributo> listAtr)
        {
            int tam = 0;
            int entero;
            float flotante;
            char car;
            string cad = "";
            int pos = 8;

            for (int i = 0; i < listAtr.Count; i++)
            {
                switch (listAtr[i].tipo)
                {
                    case Atributo.entero:
                        entero = convierteEntero(b, pos, ref tam);
                        break;
                    case Atributo.flotante:

                        flotante = convierteFlotante(b, pos, ref tam);
                        break;
                    case Atributo.caracter:
                        car = convierteChar(b, pos, ref tam);
                        break;
                    case Atributo.cadena:
                        cad = convierteCadena(b, pos, ref tam);
                        break;
                }
                pos += tam;
            }
        }

        public static int convierteEntero(byte[] b, int pos, ref int tam)
        {
            tam = sizeof(int);
            byte[] bDato = new byte[tam];

            for (int j = 0; j < tam; j++)
            {
                bDato[j] = b[pos + j];
            }

            return BitConverter.ToInt32(bDato, 0);
        }

        public static float convierteFlotante(byte[] b, int pos, ref int tam)
        {
            tam = sizeof(float);
            byte[] bDato = new byte[tam];

            for (int j = 0; j < tam; j++)
            {
                bDato[j] = b[pos + j];
            }

            return BitConverter.ToSingle(bDato, 0);
        }

        public static char convierteChar(byte[] b, int pos, ref int tam)
        {
            tam = sizeof(char) / 2;
            char car = '\0';

            for (int j = 0; j < tam; j++)
            {
                car = Convert.ToChar(b[pos + j]);
            }

            return car;
        }

        public static string convierteCadena(byte[] b, int pos, ref int tam)
        {
            tam = (sizeof(char) / 2) * tamCad;
            char car;
            string cad = "";

            for (int j = 0; j < tam; j++)
            {
                car = Convert.ToChar(b[pos + j]);
                if (!car.Equals('~'))
                {
                    cad += car;
                }
                else
                {
                    break;
                }
            }

            return cad;
        }

        public static long leeApBloq(byte[] b)
        {
            long apSigBloq = 0;
            byte[] dato;

            dato = new byte[sizeof(long)];
            for (int j = 0; j < sizeof(long); j++)
            {
                dato[j] = b[j];
            }
            apSigBloq = BitConverter.ToInt32(dato, 0);

            return apSigBloq;
        }

        public static byte[] reescribeApSigBloq(long pos, byte[] bloque)
        {
            int posicion = 0;

            byte[] apBloq = BitConverter.GetBytes(pos);
            agregaDatoBloque(apBloq, sizeof(long), bloque, ref posicion);

            return bloque;
        }

        public static bool comparaDato(byte[] b,string dato, int posAtr,List<Atributo>listAtr)
        {
            bool band = false;

            int tam = 0;
            int entero;
            float flotante;
            char car;
            string cad = "";
            int pos = sizeof(long);

            for (int i = 0; i < listAtr.Count; i++)
            {
                switch (listAtr[i].tipo)
                {
                    case Atributo.entero:
                        entero = convierteEntero(b, pos, ref tam);
                        if (i == posAtr)
                        {
                            band = (entero.ToString() == dato);
                            return band;
                        }
                        break;
                    case Atributo.flotante:
                        flotante = convierteFlotante(b, pos, ref tam);
                        if (i == posAtr)
                        {
                            band = (flotante.ToString() == dato);
                            return band;
                        }
                        break;
                    case Atributo.caracter:
                        car = convierteChar(b, pos, ref tam);
                        if (i == posAtr)
                        {
                            band = (car.ToString() == dato);
                            return band;
                        }
                        break;
                    case Atributo.cadena:
                        cad = convierteCadena(b, pos, ref tam);
                        if (i == posAtr)
                        {
                            band = (cad == dato);
                            return band;
                        }
                        break;
                }
                pos += tam;        
            }
            
            return band;
        }

    }
}
