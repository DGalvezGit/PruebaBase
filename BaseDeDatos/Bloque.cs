using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDeDatos
{
    static class Bloque
    {
        private static int tamCad = 100;

        public static byte[] creaBloque(List<Atributo>listAtr,System.Windows.Forms.DataGridViewRow registro)
        {
            byte[] bloque;
            int tamBloq;
            int pos =0;
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
                        agregaDatoBloque(BitConverter.GetBytes(int.Parse(registro.Cells[i].Value.ToString())), sizeof(int), bloque, ref pos);
                        break;
                    case Atributo.flotante:
                        agregaDatoBloque(BitConverter.GetBytes(float.Parse(registro.Cells[i].Value.ToString())), sizeof(float), bloque, ref pos);
                        break;
                    case Atributo.caracter:
                        agregaDatoBloque(BitConverter.GetBytes(char.Parse(registro.Cells[i].Value.ToString())), sizeof(char)/2, bloque, ref pos);
                        break;
                    case Atributo.cadena:
                        cad = registro.Cells[i].Value.ToString();
                        for (int j = cad.Length; j < tamCad; j++)
                        {
                            cad+='~';
                        }
                        agregaDatoBloque(convierteCadena(cad, (sizeof(char)/2) * cad.Length),(sizeof(char)/2)*cad.Length, bloque, ref pos);
                        break;
                }
            }

            return bloque;
        }

        private static byte[] convierteCadena(string cad,int tam)
        {
            byte[] b = new byte[tam];

            for(int i=0;i<cad.Length;i++)
            {
                b[i] = BitConverter.GetBytes(cad[i])[0];
            }

            return b;
        }

        private static void agregaDatoBloque(byte[] dato, int tam,byte[]bloque, ref int pos)
        {
            for (int i = 0; i < tam; i++)
            {
                bloque[pos + i] = dato[i];
            }
            pos += tam;
        }

        private static int calculaTamBloque(List<Atributo> listAtr)
        {
            int tam=0;
            
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
                        tam += sizeof(char)/2;
                    break;
                    case Atributo.cadena:
                        tam += (sizeof(char)/2)*tamCad;
                    break;
                }
            }
            tam += sizeof(long); // tamaño del apuntador al siguiente bloque

            return tam;
        }

        public static void leeBloque(byte[] b,List<Atributo>listAtr)
        {
            int tam=0;
            int entero;
            float flotante;
            byte[] bDato;
            char car;
            string cad="";
            int pos = 8;

            for (int i = 0; i < listAtr.Count; i++)
            {
                switch (listAtr[i].tipo)
                {
                    case Atributo.entero:
                        tam = sizeof(int);
                        bDato = new byte[tam];
                        for (int j = 0; j < tam; j++)
                        {
                            bDato[j] = b[pos + j];
                        }
                        entero = BitConverter.ToInt32(bDato,0);
                    break;
                    case Atributo.flotante:
                        tam = sizeof(float);
                        bDato = new byte[tam];
                        for (int j = 0; j < tam; j++)
                        {
                            bDato[j] = b[pos + j];
                        }
                        flotante = BitConverter.ToSingle(bDato, 0);
                    break;
                    case Atributo.caracter:
                        tam = sizeof(char)/2;
                        bDato = new byte[tam];
                        for (int j = 0; j < tam; j++)
                        {
                            bDato[j] = b[pos + j];
                        }
                        car = BitConverter.ToChar(bDato, 0);
                    break;
                    case Atributo.cadena:
                        tam = (sizeof(char)/2)*tamCad;
                        for (int j = 0; j < tam; j++)
                        {
                            cad += Convert.ToChar(b[pos + j]);
                        }
                    break;
                }
                pos += tam;
            }
        }

    }
}
