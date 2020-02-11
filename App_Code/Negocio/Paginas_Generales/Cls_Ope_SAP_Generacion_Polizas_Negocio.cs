using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Generacion_Polizas.Datos;

namespace Presidencia.Generacion_Polizas.Negocio
{
    public class Cls_Ope_SAP_Generacion_Polizas_Negocio
    {
        private List<String> Polizas_Lst = new List<String>();  //Lista que contendra los elementos

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Cls_Ope_SAP_Generacion_Polizas_Negocio
        /// 	DESCRIPCIÓN: Constructor de la clase. Agrega los campos de la poliza a la lista Polizas_Lst
        /// 	            inicializando los valores con longitud y el caracter de escape "/"
        /// 	PARÁMETROS:
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 14-abr-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public Cls_Ope_SAP_Generacion_Polizas_Negocio()
        {
            //Campos de Datos Generales
            Polizas_Lst.Add("/".PadRight(4, ' '));          //0
            Polizas_Lst.Add("/".PadRight(8, ' '));          //1
            Polizas_Lst.Add("/".PadRight(2, ' '));          //2
            Polizas_Lst.Add("/".PadRight(2, ' '));          //3
            Polizas_Lst.Add("/".PadRight(16, ' '));         //4
            Polizas_Lst.Add("/".PadRight(25, ' '));         //5
                    //Campos Cargo y/o Abono
            Polizas_Lst.Add("/".PadRight(2, ' '));          //6
            Polizas_Lst.Add("/".PadRight(17, ' '));         //7
            Polizas_Lst.Add("/");                           //8
            Polizas_Lst.Add("/".PadRight(3, ' '));          //9
            Polizas_Lst.Add("/".PadRight(16, ' '));         //10
            Polizas_Lst.Add("/".PadRight(4, ' '));          //11
            Polizas_Lst.Add("/".PadRight(10, ' '));         //12
            Polizas_Lst.Add("/".PadRight(4, ' '));          //13
            Polizas_Lst.Add("/".PadRight(16, ' '));         //14
            Polizas_Lst.Add("/".PadRight(10, ' '));         //15
            Polizas_Lst.Add("/".PadRight(12, ' '));         //16
            Polizas_Lst.Add("/".PadRight(10, ' '));         //17
            Polizas_Lst.Add("/".PadRight(3, ' '));          //18
            Polizas_Lst.Add("/".PadRight(4, ' '));          //19
            Polizas_Lst.Add("/".PadRight(8, ' '));          //20
            Polizas_Lst.Add("/");                           //21
            Polizas_Lst.Add("/".PadRight(18, ' '));         //21
            Polizas_Lst.Add("/".PadRight(50, ' '));         //23
            Polizas_Lst.Add("/".PadRight(12, ' '));         //24
            Polizas_Lst.Add("/".PadRight(12, ' '));         //25
            Polizas_Lst.Add("/".PadRight(20, ' '));         //26
        }


/// --------------------------------------- Propiedades públicas ---------------------------------------
#region PROPIEDADES PUBLICAS

        public String P_Sociedad
        {
            get { return Polizas_Lst[0]; }
            set
            {
                Polizas_Lst[0] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[0].Length, ' ').Substring(0, Polizas_Lst[0].Length) :
                    "/".PadRight(Polizas_Lst[0].Length, ' ');
            }
        }
        public String P_Fecha
        {
            get { return Polizas_Lst[1]; }
            set
            {
                Polizas_Lst[1] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[1].Length, ' ').Substring(0, Polizas_Lst[1].Length) :
                    "/".PadRight(Polizas_Lst[1].Length, ' ');
            }
        }
        public String P_Periodo
        {
            get { return Polizas_Lst[2]; }
            set
            {
                Polizas_Lst[2] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[2].Length, ' ').Substring(0, Polizas_Lst[2].Length) :
                    "/".PadRight(Polizas_Lst[2].Length, ' ');
            }
        }
        public String P_Clase_Docto
        {
            get { return Polizas_Lst[3]; }
            set
            {
                Polizas_Lst[3] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[3].Length, ' ').Substring(0, Polizas_Lst[3].Length) :
                    "/".PadRight(Polizas_Lst[3].Length, ' ');
            }
        }
        public String P_Referencia
        {
            get { return Polizas_Lst[4]; }
            set
            {
                Polizas_Lst[4] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[4].Length, ' ').Substring(0, Polizas_Lst[4].Length) :
                    "/".PadRight(Polizas_Lst[4].Length, ' ');
            }
        }
        public String P_Tecto_Cabecera
        {
            get { return Polizas_Lst[5]; }
            set
            {
                Polizas_Lst[5] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[5].Length, ' ').Substring(0, Polizas_Lst[5].Length) :
                    "/".PadRight(Polizas_Lst[5].Length, ' ');
            }
        }
        public String P_Clave
        {
            get { return Polizas_Lst[6]; }
            set
            {
                Polizas_Lst[6] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[6].Length, ' ').Substring(0, Polizas_Lst[6].Length) :
                    "/".PadRight(Polizas_Lst[6].Length, ' ');
            }
        }
        public String P_Cuenta
        {
            get { return Polizas_Lst[7]; }
            set
            {
                Polizas_Lst[7] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[7].Length, ' ').Substring(0, Polizas_Lst[7].Length) :
                    "/".PadRight(Polizas_Lst[7].Length, ' ');
            }
        }
        public String P_CME
        {
            get { return Polizas_Lst[8]; }
            set
            {
                Polizas_Lst[8] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[8].Length, ' ').Substring(0, Polizas_Lst[8].Length) :
                    "/".PadRight(Polizas_Lst[8].Length, ' ');
            }
        }
        public String P_Clase_Mov_Act_Fij
        {
            get { return Polizas_Lst[9]; }
            set
            {
                Polizas_Lst[9] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[9].Length, ' ').Substring(0, Polizas_Lst[9].Length) :
                    "/".PadRight(Polizas_Lst[9].Length, ' ');
            }
        }
        public String P_Importe
        {
            get { return Polizas_Lst[10]; }
            set
            {
                Polizas_Lst[10] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[10].Length, ' ').Substring(0, Polizas_Lst[10].Length) :
                    "/".PadRight(Polizas_Lst[10].Length, ' ');
            }
        }
        public String P_Cantidad_Activos
        {
            get { return Polizas_Lst[11]; }
            set
            {
                Polizas_Lst[11] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[11].Length, ' ').Substring(0, Polizas_Lst[11].Length) :
                    "/".PadRight(Polizas_Lst[11].Length, ' ');
            }
        }
        public String P_Unidad_Responsable
        {
            get { return Polizas_Lst[12]; }
            set
            {
                Polizas_Lst[12] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[12].Length, ' ').Substring(0, Polizas_Lst[12].Length) :
                    "/".PadRight(Polizas_Lst[12].Length, ' ');
            }
        }
        public String P_Area_Funcional
        {
            get { return Polizas_Lst[13]; }
            set
            {
                Polizas_Lst[13] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[13].Length, ' ').Substring(0, Polizas_Lst[13].Length) :
                    "/".PadRight(Polizas_Lst[13].Length, ' ');
            }
        }
        public String P_Elemento_PEP
        {
            get { return Polizas_Lst[14]; }
            set
            {
                Polizas_Lst[14] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[14].Length, ' ').Substring(0, Polizas_Lst[14].Length) :
                    "/".PadRight(Polizas_Lst[14].Length, ' ');
            }
        }
        public String P_Fondo
        {
            get { return Polizas_Lst[15]; }
            set
            {
                Polizas_Lst[15] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[15].Length, ' ').Substring(0, Polizas_Lst[15].Length) :
                    "/".PadRight(Polizas_Lst[15].Length, ' ');
            }
        }
        public String P_No_De_Orden
        {
            get { return Polizas_Lst[16]; }
            set
            {
                Polizas_Lst[16] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[16].Length, ' ').Substring(0, Polizas_Lst[16].Length) :
                    "/".PadRight(Polizas_Lst[16].Length, ' ');
            }
        }
        public String P_No_De_Reserva
        {
            get { return Polizas_Lst[17]; }
            set
            {
                Polizas_Lst[17] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[17].Length, ' ').Substring(0, Polizas_Lst[17].Length) :
                    "/".PadRight(Polizas_Lst[17].Length, ' ');
            }
        }
        public String P_Pos_De_Reserva
        {
            get { return Polizas_Lst[18]; }
            set
            {
                Polizas_Lst[18] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[18].Length, ' ').Substring(0, Polizas_Lst[18].Length) :
                    "/".PadRight(Polizas_Lst[18].Length, ' ');
            }
        }
        public String P_Division
        {
            get { return Polizas_Lst[19]; }
            set
            {
                Polizas_Lst[19] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[19].Length, ' ').Substring(0, Polizas_Lst[19].Length) :
                    "/".PadRight(Polizas_Lst[19].Length, ' ');
            }
        }
        public String P_Fecha_Base
        {
            get { return Polizas_Lst[20]; }
            set
            {
                Polizas_Lst[20] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[20].Length, ' ').Substring(0, Polizas_Lst[20].Length) :
                    "/".PadRight(Polizas_Lst[20].Length, ' ');
            }
        }
        public String P_Via_Pago
        {
            get { return Polizas_Lst[21]; }
            set
            {
                Polizas_Lst[21] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[21].Length, ' ').Substring(0, Polizas_Lst[21].Length) :
                    "/".PadRight(Polizas_Lst[21].Length, ' ');
            }
        }
        public String P_Asignacion
        {
            get { return Polizas_Lst[22]; }
            set
            {
                Polizas_Lst[22] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[22].Length, ' ').Substring(0, Polizas_Lst[22].Length) :
                    "/".PadRight(Polizas_Lst[22].Length, ' ');
            }
        }
        public String P_Texto_Posicion
        {
            get { return Polizas_Lst[23]; }
            set
            {
                Polizas_Lst[23] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[23].Length, ' ').Substring(0, Polizas_Lst[23].Length) :
                    "/".PadRight(Polizas_Lst[23].Length, ' ');
            }
        }
        public String P_Clave_Ref_1
        {
            get { return Polizas_Lst[24]; }
            set
            {
                Polizas_Lst[24] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[24].Length, ' ').Substring(0, Polizas_Lst[24].Length) :
                    "/".PadRight(Polizas_Lst[24].Length, ' ');
            }
        }
        public String P_Clave_Ref_2
        {
            get { return Polizas_Lst[25]; }
            set
            {
                Polizas_Lst[25] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[25].Length, ' ').Substring(0, Polizas_Lst[25].Length) :
                    "/".PadRight(Polizas_Lst[25].Length, ' ');
            }
        }
        public String P_Clave_Ref_3
        {
            get { return Polizas_Lst[26]; }
            set
            {
                Polizas_Lst[26] = value.Length > 0 ?
                    value.PadRight(Polizas_Lst[26].Length, ' ').Substring(0, Polizas_Lst[26].Length) :
                    "/".PadRight(Polizas_Lst[26].Length, ' ');
            }
        }


#endregion PROPIEDADES PUBLICAS

/// --------------------------------------- Metodos ---------------------------------------
#region METODOS

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Generar_Texto_Poliza
        /// 	DESCRIPCIÓN: Concatena el valor de todos los elementos de la Lista Polizas y lo regresa como String
        /// 	PARÁMETROS:
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 14-abr-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public String Generar_Texto_Poliza()
        {
            String Resultado = "";

            foreach (String Valor in Polizas_Lst)
            {
                Resultado += Valor;
            }
            return Resultado;
        }

        public DataTable Consultar_Datos_Poliza(String Orden_Compra)
        {
            return Cls_Ope_SAP_Generacion_Polizas_Datos.Consultar_Datos_Poliza(Orden_Compra);
        }

#endregion METODOS
    }

}///namespace