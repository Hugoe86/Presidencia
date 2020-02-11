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
using Presidencia.Bandeja_Pendientes_Atencion_Ciudadana.Datos;

/// <summary>
/// Summary description for Cls_Bandeja_Pendientes_Negocio
/// </summary>
/// 

namespace Presidencia.Bandeja_Pendientes_Atencion_Ciudadana.Negocios
{
    public class Cls_Bandeja_Pendientes_Negocio
    {
        #region Variables Internas
        private String Empleado_ID;
        private String Area_ID;
        private String Dependencia_ID;
        private String[] Areas;
        private String Rol_ID;

        
        #endregion

        #region Variables Públicas

        public String P_Rol_ID
        {
            get { return Rol_ID; }
            set { Rol_ID = value; }
        }

        public String[] P_Areas
        {
            get { return Areas; }
            set { Areas = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }


        public String P_Area_ID
        {
            get { return Area_ID; }
            set { Area_ID = value; }
        }

        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        #endregion

        #region Métodos
        public Cls_Bandeja_Pendientes_Negocio()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Parametros
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base de datos
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consultar_Parametros()
        {
            return Cls_Bandeja_Pendientes_Datos.Consultar_Parametros(this);
        }


        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Peticiones_Empleado
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base de datos
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consultar_Peticiones_Empleado() {
            return Cls_Bandeja_Pendientes_Datos.Consultar_Peticiones_Empleado(this);
        }


        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Area_Empleado
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base de datos
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consultar_Area_Empleado() {
            return Cls_Bandeja_Pendientes_Datos.Consultar_Area_Empleado(this);
        }


        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Peticiones_Jefe_Area
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base de datos
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consultar_Peticiones_Jefe_Area()
        {
            return Cls_Bandeja_Pendientes_Datos.Consultar_Peticiones_Jefe_Area(this);
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencia_Empleado
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base de datos
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************

        public DataSet Consultar_Dependencia_Empleado() {
            return Cls_Bandeja_Pendientes_Datos.Consultar_Dependencia_Empleado(this);
        }


        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Peticiones_Jefe_Dependencia
        ///DESCRIPCIÓN: Llama a la clase de datos que se encarga de la conexion a la base de datos
        ///PARAMETROS:
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public DataSet Consultar_Peticiones_Jefe_Dependencia() {
            return Cls_Bandeja_Pendientes_Datos.Consultar_Peticiones_Jefe_Dependencias(this);
        }
        #endregion 

    }
}
