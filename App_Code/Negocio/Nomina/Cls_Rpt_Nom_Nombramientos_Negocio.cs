using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Reporte_Nombramientos.Datos;

namespace Presidencia.Reporte_Nombramientos.Negocio
{
    public class Cls_Rpt_Nom_Nombramientos_Negocio
    {
        public Cls_Rpt_Nom_Nombramientos_Negocio()
        {
        }

        #region (Variables Privadas)
        private String No_Empleado;
        private String Nombre;
        private String Dependencia_ID;
        private String Tipo_Nomina_ID;
        private String Estatus;
        private String Empleado_ID;
        #endregion

        #region (Variables Publicas)
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

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
        #endregion

        #region (Metodos)
        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Nombramientos
        /// DESCRIPCIÓN: Realizar la consulta de los nombramientos de los empleados con diversos criterios de busqueda
        /// PARÁMETROS:  
        /// CREO: Noe Mosqueda Valadez
        /// FECHA_CREO: 09/Abril/2012 21:41
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public DataTable Consulta_Nombramientos()
        {
            return Cls_Rpt_Nom_Nombramientos_Datos.Consulta_Nombramientos(this);
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Promociones_Empleado
        /// DESCRIPCIÓN: Realizar la consulta de las propociones de un empleado por orden descendente
        /// PARÁMETROS:  
        /// CREO: Noe Mosqueda Valadez
        /// FECHA_CREO: 09/Abril/2012 23:04
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public DataTable Consulta_Promociones_Empleado()
        {
            return Cls_Rpt_Nom_Nombramientos_Datos.Consulta_Promociones_Empleado(this);
        }
        #endregion
    }
}