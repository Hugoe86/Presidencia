using System;
using System.Data;
using Presidencia.Reportes_Nomina_Nomina_Por_Dependencia.Datos;

namespace Presidencia.Reportes_Nomina_Nomina_Por_Dependencia.Negocio
{
    public class Cls_Rpt_Nom_Nomina_Por_Dependencia_Negocio
    {
        #region VARIABLES LOCALES

        private String Nomina_ID;
        private String No_Nomina;
        private String Filtro_Dinamico;
        private String Dependencia_ID;
        private String Tipo_Nomina_ID;
        private String Tipo_Percepcion_Deduccion;

        #endregion VARIABLES LOCALES

        #region PROPIEDADES

        //get y set de P_Nomina_ID
        public String P_Nomina_ID
        {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }

        //get y set de P_No_Nomina
        public String P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }

        //get y set de P_Filtro_Dinamico
        public String P_Filtro_Dinamico
        {
            get { return Filtro_Dinamico; }
            set { Filtro_Dinamico = value; }
        }

        //get y set de P_Dependencia_ID
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        //get y set de P_Tipo_Nomina_ID
        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }

        //get y set de P_Tipo_Percepcion_Deduccion
        public String P_Tipo_Percepcion_Deduccion
        {
            get { return Tipo_Percepcion_Deduccion; }
            set { Tipo_Percepcion_Deduccion = value; }
        }

        #endregion PROPIEDADES

        #region METODOS

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Nomina
        ///DESCRIPCIÓN          : Metodo para obtener los datos de percepciones y deducciones de los recibos de nómina
        ///PROPIEDADES          :
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 09-abr-2012
        ///*********************************************************************************************************
        public DataTable Consultar_Nomina()
        {
            return Cls_Rpt_Nom_Nomina_Por_Dependencia_Datos.Consultar_Nomina(this);
        }
        #endregion METODOS
    }
}
