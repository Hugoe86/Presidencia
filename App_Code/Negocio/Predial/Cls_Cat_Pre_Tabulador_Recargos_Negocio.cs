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
using System.Collections.Generic;
using Presidencia.Catalogo_Tabulador_Recargos.Datos;
using Presidencia.Constantes;

namespace Presidencia.Catalogo_Tabulador_Recargos.Negocio
{

    public class Cls_Cat_Pre_Tabulador_Recargos_Negocio
    {

        #region Variables Internas

        private String Recargo_ID;
        private String Anio_Tabulador;
        private String Anio;
        private String Enero;
        private String Febrero;
        private String Marzo;
        private String Abril;
        private String Mayo;
        private String Junio;
        private String Julio;
        private String Agosto;
        private String Septiembre;
        private String Octubre;
        private String Noviembre;
        private String Diciembre;
        private String Bimestre;
        private String Usuario;
        private DataTable Recargos;

        #endregion

        #region Variables Publicas
        
        public String P_Recargo_ID
        {
            get { return Recargo_ID; }
            set { Recargo_ID = value; }
        }

        public String P_Anio_Tabulador
        {
            get { return Anio_Tabulador; }
            set { Anio_Tabulador = value; }
        }
        
        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Enero
        {
            get { return Enero; }
            set { Enero = value; }
        }

        public String P_Febrero
        {
            get { return Febrero; }
            set { Febrero = value; }
        }

        public String P_Marzo
        {
            get { return Marzo; }
            set { Marzo = value; }
        }

        public String P_Abril
        {
            get { return Abril; }
            set { Abril = value; }
        }

        public String P_Mayo
        {
            get { return Mayo; }
            set { Mayo = value; }
        }

        public String P_Junio
        {
            get { return Junio; }
            set { Junio = value; }
        }

        public String P_Julio
        {
            get { return Julio; }
            set { Julio = value; }
        }

        public String P_Agosto
        {
            get { return Agosto; }
            set { Agosto = value; }
        }

        public String P_Septiembre
        {
            get { return Septiembre; }
            set { Septiembre = value; }
        }

        public String P_Octubre
        {
            get { return Octubre; }
            set { Octubre = value; }
        }

        public String P_Noviembre
        {
            get { return Noviembre; }
            set { Noviembre = value; }
        }

        public String P_Diciembre
        {
            get { return Diciembre; }
            set { Diciembre = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public DataTable DT_Recargos
        {
            get { return Recargos; }
            set { Recargos = value; }
        }

        public String P_Bimestre
        {
            get { return Bimestre; }
            set { Bimestre = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Recargo()
        {
            Cls_Cat_Pre_Tabulador_Recargos_Datos.Alta_Recargo(this);
        }

        public void Modificar_Recargo()
        {
            Cls_Cat_Pre_Tabulador_Recargos_Datos.Modificar_Recargo(this);
        }

        public int Eliminar_Recargo()
        {
            return Cls_Cat_Pre_Tabulador_Recargos_Datos.Eliminar_Recargo(this);
        }

        public DataTable Consultar_Anio() //Busqueda
        {
            return Cls_Cat_Pre_Tabulador_Recargos_Datos.Consultar_Anio(this);
        }

        public DataTable Consultar_Recargos()
        {
            return Cls_Cat_Pre_Tabulador_Recargos_Datos.Consultar_Recargos(this);
        }

        public DataTable Consultar_Anios()
        {
            return Cls_Cat_Pre_Tabulador_Recargos_Datos.Consultar_Anios();
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Tabulador_Recargos
        /// DESCRIPCIÓN: Obtener el tabulador de recargos en un Datatable para un mes entre un rango de anios
        /// PARÁMETROS:
        /// 		1. Mes: Numero de mes del que se obtendran los recargos
        /// 		2. Anio_Desde: limite inferior de anio a consultar
        /// 		3. Anio_Hasta: limite superior del anio a consultar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 04-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public DataTable Consultar_Tabulador_Recargos(Int32 Mes, Int32 Anio_Tabulador)
        {
            // convertir numero de anio en nombre de mes para consulta (nombre de campo en archivo de constantes)
            Dictionary<Int32, String> Meses = new Dictionary<int, string>()
            {
                {1, Cat_Pre_Recargos.Campo_Enero},
                {2, Cat_Pre_Recargos.Campo_Febrero},
                {3, Cat_Pre_Recargos.Campo_Marzo},
                {4, Cat_Pre_Recargos.Campo_Abril},
                {5, Cat_Pre_Recargos.Campo_Mayo},
                {6, Cat_Pre_Recargos.Campo_Junio},
                {7, Cat_Pre_Recargos.Campo_Julio},
                {8, Cat_Pre_Recargos.Campo_Agosto},
                {9, Cat_Pre_Recargos.Campo_Septiembre},
                {10, Cat_Pre_Recargos.Campo_Octubre},
                {11, Cat_Pre_Recargos.Campo_Noviembre},
                {12, Cat_Pre_Recargos.Campo_Diciembre},
            };
            if (Meses.ContainsKey(Mes))
            {
                return Cls_Cat_Pre_Tabulador_Recargos_Datos.Consultar_Tabulador_Recargos(Meses[Mes], Anio_Tabulador);
            }
            return null;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Tabulador_Recargos_Diccionario
        /// DESCRIPCIÓN: Obtener el tabulador de recargos para un mes entre un rango de anios
        ///             regresa un Diccionario<String,Decimal> con las tasas para recargos
        /// PARÁMETROS:
        /// 		1. Mes: Numero de mes del que se obtendran los recargos
        /// 		2. Anio_Tabulador: anio del tabulador a consultar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 04-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public Dictionary<String, Decimal> Consultar_Tabulador_Recargos_Diccionario(Int32 Mes, Int32 Anio_Tabulador)
        {
            Dictionary<String, Decimal> Tabulador_recargos = new Dictionary<String, Decimal>();
            DataTable Dt_Tabulador_Recargos;
            // convertir numero de anio en nombre de mes para consulta (nombre de campo en archivo de constantes)
            Dictionary<Int32, String> Meses = new Dictionary<int, string>()
            {
                {1, Cat_Pre_Recargos.Campo_Enero},
                {2, Cat_Pre_Recargos.Campo_Febrero},
                {3, Cat_Pre_Recargos.Campo_Marzo},
                {4, Cat_Pre_Recargos.Campo_Abril},
                {5, Cat_Pre_Recargos.Campo_Mayo},
                {6, Cat_Pre_Recargos.Campo_Junio},
                {7, Cat_Pre_Recargos.Campo_Julio},
                {8, Cat_Pre_Recargos.Campo_Agosto},
                {9, Cat_Pre_Recargos.Campo_Septiembre},
                {10, Cat_Pre_Recargos.Campo_Octubre},
                {11, Cat_Pre_Recargos.Campo_Noviembre},
                {12, Cat_Pre_Recargos.Campo_Diciembre},
            };

            Dt_Tabulador_Recargos = Cls_Cat_Pre_Tabulador_Recargos_Datos.Consultar_Tabulador_Recargos(Meses[Mes], Anio_Tabulador);

            foreach (DataRow Fila_Recargos in Dt_Tabulador_Recargos.Rows)
            {
                String Periodo = Fila_Recargos[Cat_Pre_Recargos.Campo_Bimestre].ToString()
                    + Fila_Recargos[Cat_Pre_Recargos.Campo_Anio].ToString();
                Decimal Tasa;

                // si el diccionario no contiene el periodo y se encuentra la tasa, agregar al diccionario
                if (!Tabulador_recargos.ContainsKey(Periodo) && Decimal.TryParse(Fila_Recargos["TASA"].ToString(), out Tasa))
                {
                    Tabulador_recargos.Add(Periodo, Tasa);
                }
            }

            return Tabulador_recargos;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Tabulador_Recargos_Diccionario
        /// DESCRIPCIÓN: Obtener el tabulador de recargos para un mes entre un rango de anios
        ///             regresa un Diccionario<String,Decimal> con las tasas para recargos
        /// PARÁMETROS:
        /// 		1. Mes: Numero de mes del que se obtendran los recargos
        /// 		2. Anio_Desde: limite inferior de anio a consultar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 04-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public Dictionary<String, Decimal> Consultar_Tabulador_Recargos_Diccionario(String Mes, String Anio_Tabulador)
        {
            Dictionary<String, Decimal> Tabulador_recargos = new Dictionary<String, Decimal>();
            DataTable Dt_Tabulador_Recargos;
            Int32 Anio = 0;

            Int32.TryParse(Anio_Tabulador, out Anio);

            Dt_Tabulador_Recargos = Cls_Cat_Pre_Tabulador_Recargos_Datos.Consultar_Tabulador_Recargos(Mes, Anio);

            foreach (DataRow Fila_Recargos in Dt_Tabulador_Recargos.Rows)
            {
                String Periodo = Fila_Recargos[Cat_Pre_Recargos.Campo_Bimestre].ToString()
                    + Fila_Recargos[Cat_Pre_Recargos.Campo_Anio].ToString();
                Decimal Tasa;

                // si el diccionario no contiene el periodo y se encuentra la tasa, agregar al diccionario
                if (!Tabulador_recargos.ContainsKey(Periodo) && Decimal.TryParse(Fila_Recargos["TASA"].ToString(), out Tasa))
                {
                    Tabulador_recargos.Add(Periodo, Tasa);
                }
            }

            return Tabulador_recargos;
        }

        #endregion

    }
}