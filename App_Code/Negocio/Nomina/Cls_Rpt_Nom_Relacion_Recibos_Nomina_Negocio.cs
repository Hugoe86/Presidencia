using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Relacion_Recibos_Nomina.Datos;

/// <summary>
/// Summary description for Cls_Rpt_Nom_Relacion_Recibos_Nomina_Negocio
/// </summary>
/// 
namespace Presidencia.Relacion_Recibos_Nomina.Negocio
{
    public class Cls_Rpt_Nom_Relacion_Recibos_Nomina_Negocio
    {

        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas
        private String Dependencia_ID;
        private String Nomina_ID;


        private String No_Nomina;

       


        #endregion

        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas

        public String P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }
       
        public String P_Nomina_ID
        {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        #endregion


        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos
        public DataTable Consultar_Unidades_Responsables()
        {
            return Cls_Rpt_Nom_Relacion_Recibos_Nomina_Datos.Consultar_Unidades_Responsables(this);

        }

        public DataTable Consultar_Recibos_Empleados()
        {
            return Cls_Rpt_Nom_Relacion_Recibos_Nomina_Datos.Consultar_Recibos_Empleados(this);
        }

        
        #endregion
    }
}