using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Vacaciones_Acumuladas_Por_Empleado.Datos;

/// <summary>
/// Summary description for Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Negocio
/// </summary>

namespace Presidencia.Vacaciones_Acumuladas_Por_Empleado.Negocio
{
    public class Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Negocio
    {

        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas
        private String Empleado_ID;
        private String Unidad_Responsable_ID;
        private String Tipo_Nomina_ID;
        private String Periodo_Vacacional;
        private String Anio;
        private String No_Empleado;


        #endregion

        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas

        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }

        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_Unidad_Responsable_ID
        {
            get { return Unidad_Responsable_ID; }
            set { Unidad_Responsable_ID = value; }
        }

        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }

        public String P_Periodo_Vacacional
        {
            get { return Periodo_Vacacional; }
            set { Periodo_Vacacional = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        #endregion

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        public DataTable Consultar_Unidades_Responsables()
        {
            return Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Datos.Consultar_Unidades_Responsables();

        }

        public DataTable Consultar_Tipo_Nomina()
        {
            return Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Datos.Consultar_Tipo_Nomina();

        }

        public DataTable Consultar_Empleado()
        {
            return Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Datos.Consultar_Empleado(this);

        }

        public DataTable Consultar_Vacaciones()
        {
            return Cls_Rpt_Nom_Vacaciones_Acumuladas_Por_Empleado_Datos.Consultar_Vacaciones(this);
        }



        #endregion
    }//fin del class
}