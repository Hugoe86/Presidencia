using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Text;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Xml;
using Presidencia.Sessiones;
using Presidencia.Plantillas_Word;
using System.IO;
using Presidencia.Constantes;
using openXML_Wp = DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.IO.Packaging;
using System.Text.RegularExpressions;
using System.Collections;

namespace Presidencia.Ayudante_Curp_Rfc
{
    public class Cls_Ayudante_Curp_Rfc
    {
        public String P_General;
        public String P_RFC;
        public String P_CURP;
        public String P_Nombre;
        public String P_Apellido_Paterno;
        public String P_Apellido_Materno;
        public String P_Entidad_Federativa;
        public String P_Sexo;
        public DateTime P_Fecha_Nacimiento;

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN    : Calcular
        ///DESCRIPCIÓN       : Calcula el RFC y CURP
        ///PARÁMETROS        :
        ///CREO              : Salvador Vazquez Camacho
        ///FECHA_CREO        : 20/Octubre/2012
        ///MODIFICÓ          : 
        ///FECHA_MODIFICÓ    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public void Calcular()
        {
            //Almacenara cada uno de los nombres
            String[] Nombres;
            String Fecha = "";

            try
            {
                //Cambiamos todo a mayúsculas
                P_Nombre = P_Nombre.ToUpper();
                P_Apellido_Paterno = P_Apellido_Paterno.ToUpper();
                P_Apellido_Materno = P_Apellido_Materno.ToUpper();
                P_Sexo = P_Sexo.ToUpper();
                P_Entidad_Federativa = P_Entidad_Federativa.ToUpper();
                //RFC que se regresará
                P_General = String.Empty;

                //Quitamos los espacios al principio y final del nombre y apellidos
                P_Nombre.Trim();
                P_Apellido_Paterno = P_Apellido_Paterno.Trim();
                P_Apellido_Materno = P_Apellido_Materno.Trim();

                //Quitamos los artículos del nombre y apellidos
                P_Nombre = QuitarArticulos(P_Nombre);
                P_Apellido_Paterno = QuitarArticulos(P_Apellido_Paterno);
                P_Apellido_Paterno = QuitarArticulos(P_Apellido_Paterno);

                //Obtenemos cada nombre en una posicion del arreglo
                Nombres = P_Nombre.Split(' ');

                //Verificamos si son dos o mas nombres
                if (Nombres.Length >= 2)
                {
                    // si el nombre es jose, maria o guadalupe, tomar segundo nombre
                    if (Nombres[0] == "JOSE" || Nombres[0] == "JOSÉ" ||
                        Nombres[0] == "MARIA" || Nombres[0] == "MARÍA" || Nombres[0] == "MA" ||
                        Nombres[0] == "GUADALUPE")
                    {
                        P_Nombre = Nombres[1];
                    }
                }

                //Agregamos el primer caracter del apellido paterno
                P_General = P_Apellido_Paterno.Substring(0, 1);

                //Buscamos y agregamos al rfc la primera vocal del primer apellido
                foreach (char Caracter in P_Apellido_Paterno)
                {
                    if (EsVocal(Caracter))
                    {
                        P_General += Caracter;
                        break;
                    }
                }

                //Agregamos el primer caracter del apellido materno
                P_General += P_Apellido_Materno.Substring(0, 1);

                //Agregamos el primer caracter del primer nombre
                P_General += P_Nombre.Substring(0, 1);

                Fecha = String.Format("{0:yyyy/MM/dd}", P_Fecha_Nacimiento);
                //agregamos la fecha yymmdd (por ejemplo: 680825, 25 de agosto de 1968 )
                P_General += Fecha.Substring(2, 2) + Fecha.Substring(5, 2) + Fecha.Substring(8, 2);

                P_CURP = P_RFC = P_General;

                //Le agregamos la homoclave al rfc 
                CalcularHomoclave(P_Apellido_Paterno + " " + P_Apellido_Materno + " " + P_Nombre, Fecha);

                //Calculamos y agregamos los digitos restantes al CURP
                CalcularCURP();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el metodo Calcular. Error: " + Ex.Message + "]");
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN    : CalcularCURP
        ///DESCRIPCIÓN       : Calcula los digitos restantes del CURP
        ///PARÁMETROS        :
        ///CREO              : Salvador Vazquez Camacho
        ///FECHA_CREO        : 20/Octubre/2012
        ///MODIFICÓ          : 
        ///FECHA_MODIFICÓ    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private void CalcularCURP()
        {
            //Cadena base de caracteres
            String Alfanumerico = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
            //Arreglo que almacena un valor para cada caracter del CURP
            int[] CURP_En_Numero;
            //Variable que almacena la sumatoria
            int Sumatoria = 0;
            //Digito verificador del CURP
            int Digito = 0;
            //Factor para calcular el digito verificador
            int Factor = 19;

            try
            {
                //Se agrega la inicial de acuerdo al Sexo
                if (P_Sexo == "HOMBRE" || P_Sexo == "MASCULINO" || P_Sexo == "H")
                    P_CURP += "H";
                if (P_Sexo == "MUJER" || P_Sexo == "FEMENINO" || P_Sexo == "F")
                    P_CURP += "M";

                //Se evalua el estado y se agrega la clave correspondiente
                switch (P_Entidad_Federativa)
                {
                    case "AGUASCALIENTES":
                        P_CURP += "AS";
                        break;
                    case "BAJA CALIFORNIA":
                        P_CURP += "BC";
                        break;
                    case "BAJA CALIFORNIA SUR":
                        P_CURP += "BS";
                        break;
                    case "CAMPECHE":
                        P_CURP += "CC";
                        break;
                    case "COAHUILA":
                        P_CURP += "CL";
                        break;
                    case "COLIMA":
                        P_CURP += "CM";
                        break;
                    case "CHIAPAS":
                        P_CURP += "CS";
                        break;
                    case "CHIHUAHUA":
                        P_CURP += "CH";
                        break;
                    case "DISTRITO FEDERAL":
                        P_CURP += "DF";
                        break;
                    case "DURANGO":
                        P_CURP += "DG";
                        break;
                    case "GUANAJUATO":
                        P_CURP += "GT";
                        break;
                    case "GUERRERO":
                        P_CURP += "GR";
                        break;
                    case "HIDALGO":
                        P_CURP += "HG";
                        break;
                    case "JALISCO":
                        P_CURP += "JC";
                        break;
                    case "MEXICO":
                        P_CURP += "MC";
                        break;
                    case "MICHOACAN":
                        P_CURP += "MN";
                        break;
                    case "MORELOS":
                        P_CURP += "MS";
                        break;
                    case "NAYARIT":
                        P_CURP += "NT";
                        break;
                    case "NUEVO LEON":
                        P_CURP += "NL";
                        break;
                    case "OAXACA":
                        P_CURP += "OC";
                        break;
                    case "PUEBLA":
                        P_CURP += "PL";
                        break;
                    case "QUERETARO":
                        P_CURP += "QT";
                        break;
                    case "QUINTANA ROO":
                        P_CURP += "QR";
                        break;
                    case "SAN LUIS POTOSI":
                        P_CURP += "SP";
                        break;
                    case "SINALOA":
                        P_CURP += "SL";
                        break;
                    case "SONORA":
                        P_CURP += "SR";
                        break;
                    case "TABASCO":
                        P_CURP += "TC";
                        break;
                    case "TAMAULIPAS":
                        P_CURP += "TS";
                        break;
                    case "TLAXCALA":
                        P_CURP += "TL";
                        break;
                    case "VERACRUZ":
                        P_CURP += "VZ";
                        break;
                    case "YUCATAN":
                        P_CURP += "YN";
                        break;
                    case "ZACATECAS":
                        P_CURP += "ZS";
                        break;
                    default:
                        P_CURP += "--";
                        break;
                }

                //Encontrar la Primer consonante del apellido paterno
                foreach (Char Caracter in P_Apellido_Paterno.Substring(1, P_Apellido_Paterno.Length - 1))
                {
                    if (!EsVocal(Caracter))
                    {
                        P_CURP += Caracter.ToString();
                        break;
                    }
                }

                //Encontrar la Primer consonante del apellido materno
                foreach (Char Caracter in P_Apellido_Materno.Substring(1, P_Apellido_Materno.Length - 1))
                {
                    if (!EsVocal(Caracter))
                    {
                        P_CURP += Caracter.ToString();
                        break;
                    }
                }

                //Encontrar la Primer consonante del Nombre
                foreach (Char Caracter in P_Nombre.Substring(1, P_Nombre.Length - 1))
                {
                    if (!EsVocal(Caracter))
                    {
                        P_CURP += Caracter.ToString();
                        break;
                    }
                }

                //Asignamos al arreglo el tamaño de la CURP
                CURP_En_Numero = new int[P_CURP.Length];

                //Recorremos cada caracter de la CURP
                for (int Cnt_CURP = 0; Cnt_CURP < P_CURP.Length; Cnt_CURP++)
                {
                    //Recorremos cada caracter de la cadena Alfanumerico
                    for (int C_Alfanumerico = 0; C_Alfanumerico < Alfanumerico.Length; C_Alfanumerico++)
                    {
                        //Comparamos cada caracter de la CURP con el del arreglo Alfanumericos
                        if (P_CURP[Cnt_CURP] == Alfanumerico[C_Alfanumerico])
                        {
                            //Si son iguales se almacena el valor de la posicion
                            CURP_En_Numero[Cnt_CURP] = C_Alfanumerico;
                            break;
                        }
                    }
                }

                //Se realiza la sumatoria en base a la formula indicada
                foreach (int Cnt_Sum in CURP_En_Numero)
                {
                    Factor--;
                    Sumatoria += (Factor * Cnt_Sum);
                }

                //Se calcula el digito verificador
                Digito = 10 - (Sumatoria % 10);

                //Se agrega el digito a la CURP
                P_CURP += Digito == 10 ? "00" : String.Format("{0:00}", Digito);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el metodo CalcularCURP. Error: " + Ex.Message + "]");
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN    : CalcularHomoclave
        ///DESCRIPCIÓN       : Calcula la Homoclave del RFC
        ///PARÁMETROS        :
        ///CREO              : Salvador Vazquez Camacho
        ///FECHA_CREO        : 20/Octubre/2012
        ///MODIFICÓ          : 
        ///FECHA_MODIFICÓ    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private void CalcularHomoclave(string nombreCompleto, string fecha)
        {
            int ModuloVerificador = 0;
            Hashtable TablaRFC1 = new Hashtable();
            Hashtable TablaRFC2 = new Hashtable();
            Hashtable TablaRFC3 = new Hashtable();
            int Division = 0;
            int Modulo = 0;
            int Indice = 0;
            String Homoclave = String.Empty;  //los dos primeros caracteres de la homoclave
            int RfcAnumeroSuma = 0;
            int SumaParcial = 0;
            StringBuilder NombreEnNumero = new StringBuilder();//   Guardara el nombre en su correspondiente numérico
            long valorSuma = 0;//La suma de la secuencia de números de nombreEnNumero

            try
            {
                #region Tablas para calcular la homoclave
                //Estas tablas realmente no se porque son como son
                //solo las copie de lo que encontré en internet

                #region TablaRFC 1
                TablaRFC1.Add("&", 10);
                TablaRFC1.Add("Ñ", 10);
                TablaRFC1.Add("A", 11);
                TablaRFC1.Add("B", 12);
                TablaRFC1.Add("C", 13);
                TablaRFC1.Add("D", 14);
                TablaRFC1.Add("E", 15);
                TablaRFC1.Add("F", 16);
                TablaRFC1.Add("G", 17);
                TablaRFC1.Add("H", 18);
                TablaRFC1.Add("I", 19);
                TablaRFC1.Add("J", 21);
                TablaRFC1.Add("K", 22);
                TablaRFC1.Add("L", 23);
                TablaRFC1.Add("M", 24);
                TablaRFC1.Add("N", 25);
                TablaRFC1.Add("O", 26);
                TablaRFC1.Add("P", 27);
                TablaRFC1.Add("Q", 28);
                TablaRFC1.Add("R", 29);
                TablaRFC1.Add("S", 32);
                TablaRFC1.Add("T", 33);
                TablaRFC1.Add("U", 34);
                TablaRFC1.Add("V", 35);
                TablaRFC1.Add("W", 36);
                TablaRFC1.Add("X", 37);
                TablaRFC1.Add("Y", 38);
                TablaRFC1.Add("Z", 39);
                TablaRFC1.Add("0", 0);
                TablaRFC1.Add("1", 1);
                TablaRFC1.Add("2", 2);
                TablaRFC1.Add("3", 3);
                TablaRFC1.Add("4", 4);
                TablaRFC1.Add("5", 5);
                TablaRFC1.Add("6", 6);
                TablaRFC1.Add("7", 7);
                TablaRFC1.Add("8", 8);
                TablaRFC1.Add("9", 9);
                #endregion

                #region TablaRFC 2
                TablaRFC2.Add(0, "1");
                TablaRFC2.Add(1, "2");
                TablaRFC2.Add(2, "3");
                TablaRFC2.Add(3, "4");
                TablaRFC2.Add(4, "5");
                TablaRFC2.Add(5, "6");
                TablaRFC2.Add(6, "7");
                TablaRFC2.Add(7, "8");
                TablaRFC2.Add(8, "9");
                TablaRFC2.Add(9, "A");
                TablaRFC2.Add(10, "B");
                TablaRFC2.Add(11, "C");
                TablaRFC2.Add(12, "D");
                TablaRFC2.Add(13, "E");
                TablaRFC2.Add(14, "F");
                TablaRFC2.Add(15, "G");
                TablaRFC2.Add(16, "H");
                TablaRFC2.Add(17, "I");
                TablaRFC2.Add(18, "J");
                TablaRFC2.Add(19, "K");
                TablaRFC2.Add(20, "L");
                TablaRFC2.Add(21, "M");
                TablaRFC2.Add(22, "N");
                TablaRFC2.Add(23, "P");
                TablaRFC2.Add(24, "Q");
                TablaRFC2.Add(25, "R");
                TablaRFC2.Add(26, "S");
                TablaRFC2.Add(27, "T");
                TablaRFC2.Add(28, "U");
                TablaRFC2.Add(29, "V");
                TablaRFC2.Add(30, "W");
                TablaRFC2.Add(31, "X");
                TablaRFC2.Add(32, "Y");
                #endregion

                #region TablaRFC 3
                TablaRFC3.Add("A", 10);
                TablaRFC3.Add("B", 11);
                TablaRFC3.Add("C", 12);
                TablaRFC3.Add("D", 13);
                TablaRFC3.Add("E", 14);
                TablaRFC3.Add("F", 15);
                TablaRFC3.Add("G", 16);
                TablaRFC3.Add("H", 17);
                TablaRFC3.Add("I", 18);
                TablaRFC3.Add("J", 19);
                TablaRFC3.Add("K", 20);
                TablaRFC3.Add("L", 21);
                TablaRFC3.Add("M", 22);
                TablaRFC3.Add("N", 23);
                TablaRFC3.Add("O", 25);
                TablaRFC3.Add("P", 26);
                TablaRFC3.Add("Q", 27);
                TablaRFC3.Add("R", 28);
                TablaRFC3.Add("S", 29);
                TablaRFC3.Add("T", 30);
                TablaRFC3.Add("U", 31);
                TablaRFC3.Add("V", 32);
                TablaRFC3.Add("W", 33);
                TablaRFC3.Add("X", 34);
                TablaRFC3.Add("Y", 35);
                TablaRFC3.Add("Z", 36);
                TablaRFC3.Add("0", 0);
                TablaRFC3.Add("1", 1);
                TablaRFC3.Add("2", 2);
                TablaRFC3.Add("3", 3);
                TablaRFC3.Add("4", 4);
                TablaRFC3.Add("5", 5);
                TablaRFC3.Add("6", 6);
                TablaRFC3.Add("7", 7);
                TablaRFC3.Add("8", 8);
                TablaRFC3.Add("9", 9);
                TablaRFC3.Add("", 24);
                TablaRFC3.Add(" ", 37);
                #endregion

                #endregion

                //agregamos un cero al inicio de la representación númerica del nombre
                NombreEnNumero.Append("0");

                //Recorremos el nombre y vamos convirtiendo las letras en 
                //su valor numérico
                foreach (char c in nombreCompleto)
                {
                    if (TablaRFC1.ContainsKey(c.ToString()))
                        NombreEnNumero.Append(TablaRFC1[c.ToString()].ToString());
                    else
                        NombreEnNumero.Append("00");
                }

                //Calculamos la suma de la secuencia de números 
                //calculados anteriormente
                //la formula es:
                //( (el caracter actual multiplicado por diez)
                //mas el valor del caracter siguiente )
                //(y lo anterior multiplicado por el valor del caracter siguiente)
                for (int Cnt_For = 0; Cnt_For < NombreEnNumero.Length - 1; Cnt_For++)
                {
                    valorSuma += ((Convert.ToInt32(NombreEnNumero[Cnt_For].ToString()) * 10) + Convert.ToInt32(NombreEnNumero[Cnt_For + 1].ToString())) * Convert.ToInt32(NombreEnNumero[Cnt_For + 1].ToString());
                }

                //Lo siguiente no se porque se calcula así, es parte del algoritmo.
                //Los magic numbers que aparecen por ahí deben tener algún origen matemático
                //relacionado con el algoritmo al igual que el proceso mismo de calcular el 
                //digito verificador.
                //Por esto no puedo añadir comentarios a lo que sigue, lo hice por acto de fe.

                Division = Convert.ToInt32(valorSuma) % 1000;
                Modulo = Division % 34;
                Division = (Division - Modulo) / 34;

                while (Indice <= 1)
                {
                    if (TablaRFC2.ContainsKey((Indice == 0) ? Division : Modulo))
                        Homoclave += TablaRFC2[(Indice == 0) ? Division : Modulo];
                    else
                        Homoclave += "Z";
                    Indice++;
                }

                //Agregamos al RFC los dos primeros caracteres de la homoclave
                P_RFC += Homoclave;

                //Aqui empieza el calculo del digito verificador basado en lo que tenemos del RFC
                //En esta parte tampoco conozco el origen matemático del algoritmo como para dar 
                //una explicación del proceso, así que ¡tengamos fe hermanos!.

                for (int i = 0; i < P_RFC.Length; i++)
                {
                    if (TablaRFC3.ContainsKey(P_RFC[i].ToString()))
                    {
                        RfcAnumeroSuma = Convert.ToInt32(TablaRFC3[P_RFC[i].ToString()]);
                        SumaParcial += (RfcAnumeroSuma * (14 - (i + 1)));
                    }
                }

                ModuloVerificador = SumaParcial % 11;
                if (ModuloVerificador == 0)
                    P_RFC += "0";
                else
                {
                    SumaParcial = 11 - ModuloVerificador;
                    if (SumaParcial == 10)
                        P_RFC += "A";
                    else
                        P_RFC += SumaParcial.ToString();
                }

                //en este punto la variable rfc pasada ya debe tener la homoclave
                //recuerda que la variable rfc se paso como "ref string" lo cual
                //hace que se modifique la original.
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el metodo CalcularHomoclave. Error: " + Ex.Message + "]");
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN    : EsVocal
        ///DESCRIPCIÓN       : Calcula la Homoclave del RFC
        ///PARÁMETROS        :
        ///CREO              : Salvador Vazquez Camacho
        ///FECHA_CREO        : 20/Octubre/2012
        ///MODIFICÓ          : 
        ///FECHA_MODIFICÓ    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        static private bool EsVocal(char Letra)
        {
            return Letra == 'a' || Letra == 'e' || Letra == 'i' || Letra == 'o' || Letra == 'u' ||
                   Letra == 'á' || Letra == 'é' || Letra == 'í' || Letra == 'ó' || Letra == 'ú' ||
                   Letra == 'A' || Letra == 'E' || Letra == 'I' || Letra == 'O' || Letra == 'U' ||
                   Letra == 'Á' || Letra == 'É' || Letra == 'Í' || Letra == 'Ó' || Letra == 'Ú';
        }

        /// ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN    : QuitarArticulos
        ///DESCRIPCIÓN       : Remplaza los artículos comúnes en los apellidos con caracter vacío
        ///PARÁMETROS        : Palabra: Palabra que se le quitaran los artículos
        ///CREO              : Salvador Vazquez Camacho
        ///FECHA_CREO        : 20/Octubre/2012
        ///MODIFICÓ          : 
        ///FECHA_MODIFICÓ    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        static private string QuitarArticulos(string Palabra)
        {
            return Palabra.Replace("DE LOS ", String.Empty).Replace("DEL ", String.Empty).Replace("LAS ", String.Empty).Replace("DE ", String.Empty).Replace("LA ", String.Empty).Replace("Y ", String.Empty);
        }

    }
}
