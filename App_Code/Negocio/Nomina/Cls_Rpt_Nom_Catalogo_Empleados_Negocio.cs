using System;
using System.Data;
using Presidencia.Reportes_Nomina_Catalogo_Empleados.Datos;

namespace Presidencia.Reportes_Nomina_Catalogo_Empleados.Negocio
{
    public class Cls_Rpt_Nom_Catalogo_Empleados_Negocio
    {
        #region VARIABLES LOCALES
        private String No_Empleado;
        private String Nombre_Empleado;
        private String Estatus_Empleado;
        private String RFC_Empleado;
        private String CURP_Empleado;
        private String Nomina;
        private String Filtro_Dinamico;
        private String Dependencia_ID;
        private String Tipo_Nomina_ID;
        private String Sindicato_ID;
        private String Percepcion_Deduccion_ID;
        #endregion VARIABLES LOCALES

        #region PROPIEDADES

        //get y set de P_Nombre_Empleado
        public String P_Nombre_Empleado
        {
            get { return Nombre_Empleado; }
            set { Nombre_Empleado = value; }
        }

        //get y set de P_Estatus_Empleado
        public String P_Estatus_Empleado
        {
            get { return Estatus_Empleado; }
            set { Estatus_Empleado = value; }
        }

        //get y set de P_RFC_Empleado
        public String P_RFC_Empleado
        {
            get { return RFC_Empleado; }
            set { RFC_Empleado = value; }
        }

        //get y set de P_CURP_Empleado
        public String P_CURP_Empleado
        {
            get { return CURP_Empleado; }
            set { CURP_Empleado = value; }
        }

        //get y set de P_No_Empleado
        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }

        //get y set de P_Nomina
        public String P_Nomina
        {
            get { return Nomina; }
            set { Nomina = value; }
        }

        //get y set de P_Filtro_Dinamico
        public String P_Filtro_Dinamico
        {
            get { return Filtro_Dinamico; }
            set { Filtro_Dinamico = value; }
        }

        //get y set de P_Tipo_Nomina_ID
        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }

        //get y set de P_Dependencia_ID
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        //get y set de P_Sindicato_ID
        public String P_Sindicato_ID
        {
            get { return Sindicato_ID; }
            set { Sindicato_ID = value; }
        }

        //get y set de P_Percepcion_Deduccion_ID
        public String P_Percepcion_Deduccion_ID
        {
            get { return Percepcion_Deduccion_ID; }
            set { Percepcion_Deduccion_ID = value; }
        }
        #endregion PROPIEDADES

        #region METODOS

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Catalogo_Empleados
        ///DESCRIPCIÓN          : Metodo para obtener los datos del catalogo de empleados
        ///PROPIEDADES          :
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 05-abr-2012
        ///*********************************************************************************************************
        public DataTable Consultar_Catalogo_Empleados()
        {
            return Cls_Rpt_Nom_Catalogo_Empleados_Datos.Consultar_Catalogo_Empleados(this);
        }
        #endregion METODOS
    }
}
