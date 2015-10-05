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

            tamBloq = calculaTamBloque(listAtr);
            bloque = new byte[tamBloq];
            agregaDatoBloque(apBloq, sizeof(long), bloque, ref pos);
            for (int i = 0; i < listAtr.Count; i++)
            {
                switch (listAtr[i].tipo)
                {
                    case Atributo.entero:
                        agregaDatoBloque(BitConverter.GetBytes((int)registro.Cells[i].Value), sizeof(int), bloque, ref pos);
                        break;
                    case Atributo.flotante:
                        agregaDatoBloque(BitConverter.GetBytes((float)registro.Cells[i].Value), sizeof(float), bloque, ref pos);
                        break;
                    case Atributo.caracter:
                        agregaDatoBloque(BitConverter.GetBytes((char)registro.Cells[i].Value), sizeof(char), bloque, ref pos);
                        break;
                    case Atributo.cadena:
                        agregaDatoBloque(convierteCadena(registro.Cells[i].Value.ToString()), sizeof(int), bloque, ref pos);
                        break;
                }
            }

            return bloque;
        }

        private static byte[] convierteCadena(string cad)
        {
            byte[] b = new byte[sizeof(char)*tamCad];

            for(int i=0;i<cad.Length;i++)
            {
                b.Concat(BitConverter.GetBytes(cad[i]));
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
                        tam += sizeof(char);
                    break;
                    case Atributo.cadena:
                        tam += sizeof(char)*tamCad;
                    break;
                }
            }
            tam += sizeof(long); // tamaño del apuntador al siguiente bloque

            return tam;
        }
        

    }
}
