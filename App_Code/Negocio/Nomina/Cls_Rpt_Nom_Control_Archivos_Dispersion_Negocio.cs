using System;
using System.Data;
using Presidencia.Reportes_Nomina_Control_Archivos_Dispersion.Datos;

namespace Presidencia.Reportes_Nomina_Control_Archivos_Dispersion.Negocio
{
    public class Cls_Rpt_Nom_Control_Archivos_Dispersion_Negocio
    {
        #region VARIABLES LOCALES

        private String Numero_Nomina;
        private String Nomina_ID;

        #endregion VARIABLES LOCALES

        #region PROPIEDADES

        //get y set de P_Numero_Nomina
        public String P_Numero_Nomina
        {
            get { return Numero_Nomina; }
            set { Numero_Nomina = value; }
        }
        //get y set de P_Nomina_ID
        public String P_Nomina_ID
        {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }

        #endregion PROPIEDADES

        #region METODOS

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Depositos_Tipo_Nomina
        ///DESCRIPCIÓN          : Metodo para obtener los datos del catalogo de empleados
        ///PROPIEDADES          :
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 11-abr-2012
        ///*********************************************************************************************************
        public DataTable Consultar_Depositos_Tipo_Nomina()
        {
            return Cls_Rpt_Nom_Control_Archivos_Dispersion_Datos.Consultar_Depositos_Tipo_Nomina(this);
        }
        #endregion METODOS
    }
}
