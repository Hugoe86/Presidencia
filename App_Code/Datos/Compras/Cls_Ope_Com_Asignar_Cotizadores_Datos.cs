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
using Presidencia.Asignar_Cotizadores.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;


/// <summary>
/// Summary description for Cls_Ope_Com_Asignar_Cotizadores_Datos
/// </summary>
namespace Presidencia.Asignar_Cotizadores.Datos
{

    public class Cls_Ope_Com_Asignar_Cotizadores_Datos
    {
        
        public Cls_Ope_Com_Asignar_Cotizadores_Datos()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region Altas
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Cotizadores_Asignados
        ///DESCRIPCIÓN: Metodo que da de alta los cotizadores que se asigna a cada Cotizacion 
        ///PARAMETROS: 1.- Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Alta_Cotizadores_Asignados(Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "";
            //Consultamos el listado de requisiciones correspondiente a esta cotizacion
            Mi_SQL = "SELECT " + Ope_Com_Cotizaciones.Campo_Lista_Requisiciones +
                " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion + 
                " ='" + Datos_Cotizacion.P_No_Cotizacion+ "'";
            DataTable Dt_Lista_Req = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            Datos_Cotizacion.P_Listado_Requisiciones = Dt_Lista_Req.Rows[0][Ope_Com_Cotizaciones.Campo_Lista_Requisiciones].ToString().Trim();

            if(Datos_Cotizacion.P_Dt_Cotizadores.Rows.Count != 0)
            {
                //Damos de alta los cotizadores para cada cotizacion
                for (int i = 0; i < Datos_Cotizacion.P_Dt_Cotizadores.Rows.Count; i++)
                {
                    Mi_SQL = "INSERT INTO " + Ope_Com_Det_Cotizaciones.Tabla_Ope_Com_Det_Cotizaciones +
                        " (" + Ope_Com_Det_Cotizaciones.Campo_Giro_ID +
                        ", " + Ope_Com_Det_Cotizaciones.Campo_Empleado_ID +
                        ", " + Ope_Com_Det_Cotizaciones.Campo_No_Cotizacion +
                        ", " + Ope_Com_Det_Cotizaciones.Campo_Usuario_Creo +
                        ", " + Ope_Com_Det_Cotizaciones.Campo_Fecha_Creo + ")" +
                        " VALUES ('" + Datos_Cotizacion.P_Dt_Cotizadores.Rows[i][Ope_Com_Det_Cotizaciones.Campo_Giro_ID].ToString().Trim() +
                        "', '" + Datos_Cotizacion.P_Dt_Cotizadores.Rows[i][Ope_Com_Det_Cotizaciones.Campo_Empleado_ID].ToString().Trim() +
                        "', '" + Datos_Cotizacion.P_No_Cotizacion +
                        "', '" + Cls_Sessiones.Nombre_Empleado + "',SYSDATE)";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        
                            
                        

                    //Damos de alta los cotizadores en sus respectivos detalles de producto que les corresponde cotizar.
                        Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                            " SET " + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID +
                            " ='" + Datos_Cotizacion.P_Dt_Cotizadores.Rows[i][Ope_Com_Det_Cotizaciones.Campo_Empleado_ID].ToString().Trim() + "'" +
                            " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            " IN (" + Datos_Cotizacion.P_Listado_Requisiciones + ")" +
                            " AND " + Ope_Com_Req_Producto.Campo_Partida_ID +
                            " IN ( SELECT ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                            " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " ESP" +
                            " JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " GEN" +
                            " ON GEN." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + 
                            "= ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID +
                            " WHERE GEN." + Cat_SAP_Partida_Generica.Campo_Concepto_ID +" ='" +
                            Datos_Cotizacion.P_Dt_Cotizadores.Rows[i][Ope_Com_Det_Cotizaciones.Campo_Giro_ID].ToString().Trim() + "')";
                            

                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        
                }
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Baja_Cotizadores_Asignados
        ///DESCRIPCIÓN: Metodo que da de baja los cotizadores asignados a la Cotizacion 
        ///PARAMETROS: 1.- Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Baja_Cotizadores_Asignados(Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "";
            //Consultamos el listado de requisiciones correspondiente a esta cotizacion
            Mi_SQL = "SELECT " + Ope_Com_Cotizaciones.Campo_Lista_Requisiciones +
                " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                " ='" + Datos_Cotizacion.P_No_Cotizacion + "'";
            DataTable Dt_Lista_Req = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            Datos_Cotizacion.P_Listado_Requisiciones = Dt_Lista_Req.Rows[0][Ope_Com_Cotizaciones.Campo_Lista_Requisiciones].ToString().Trim();
            //eliminamos el los detalles de la cotizacion para dar de alta los nuevos 
            Mi_SQL = "DELETE FROM " + Ope_Com_Det_Cotizaciones.Tabla_Ope_Com_Det_Cotizaciones +
                " WHERE " + Ope_Com_Det_Cotizaciones.Campo_No_Cotizacion +
                "='" + Datos_Cotizacion.P_No_Cotizacion + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //Tambien eliminamos el cotizador_Id de la tabla de detalle de productos de la requisicion 
            Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                    " SET " + Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + "= NULL" +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                    " IN (" + Datos_Cotizacion.P_Listado_Requisiciones + ")";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Modificar_Cotizacion
        ///DESCRIPCIÓN: Metodo que modifica la cotizacion con respecto al estatus y manda llamar los metodos de alta y baja de cotizadores
        ///PARAMETROS: 1.- Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Cotizacion(Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion)
        {
           
            String Mi_SQL = "UPDATE " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                    " SET " + Ope_Com_Cotizaciones.Campo_Estatus  + 
                    "='" + Datos_Cotizacion.P_Estatus.Trim() + "'" +
                    " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                    " ='" + Datos_Cotizacion.P_No_Cotizacion + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //Damos de baja todos los detalles de giro 
            Baja_Cotizadores_Asignados(Datos_Cotizacion);
            //Damos de alta ahora los nuevos detalles
            Alta_Cotizadores_Asignados(Datos_Cotizacion);

        }


        #endregion Fin Altas

        #region Consultas

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Cotizaciones
        ///DESCRIPCIÓN: Metodo que las cotizaciones en existencia pero solo con estatus de generada o de Asignar Cotizador
        ///PARAMETROS: 1.- Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Cotizaciones(Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "SELECT " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                ", " + Ope_Com_Cotizaciones.Campo_Folio +
                ", " + Ope_Com_Cotizaciones.Campo_Tipo +
                ", " + Ope_Com_Cotizaciones.Campo_Estatus +
                ", TO_CHAR(" + Ope_Com_Cotizaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA" +
                ", " + Ope_Com_Cotizaciones.Campo_Total +
                " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                " WHERE " + Ope_Com_Cotizaciones.Campo_Estatus +
                " IN ('GENERADA','ASIGNAR COTIZADOR')" +
                " ORDER BY " + Ope_Com_Cotizaciones.Campo_No_Cotizacion;
            //COTIZADOR DEFINIDO
            if (Datos_Cotizacion.P_No_Cotizacion != null)
            {
                Mi_SQL = "SELECT " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                    ", " + Ope_Com_Cotizaciones.Campo_Folio +
                    ", " + Ope_Com_Cotizaciones.Campo_Tipo +
                    ", " + Ope_Com_Cotizaciones.Campo_Estatus +
                    ", " + Ope_Com_Cotizaciones.Campo_Condiciones +
                    ", TO_CHAR(" + Ope_Com_Cotizaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA" +
                    ", " + Ope_Com_Cotizaciones.Campo_Total +
                    " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                    " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                    "='" + Datos_Cotizacion.P_No_Cotizacion + "'";
            }//fin del IF
            if (Datos_Cotizacion.P_Folio != null)
            {
                Mi_SQL = "SELECT " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                ", " + Ope_Com_Cotizaciones.Campo_Folio +
                ", " + Ope_Com_Cotizaciones.Campo_Tipo +
                ", " + Ope_Com_Cotizaciones.Campo_Estatus +
                ", TO_CHAR(" + Ope_Com_Cotizaciones.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA" +
                ", " + Ope_Com_Cotizaciones.Campo_Total +
                " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                " WHERE " + Ope_Com_Cotizaciones.Campo_Estatus +
                " IN ('GENERADA','ASIGNAR COTIZADOR')" +
                " AND " + Ope_Com_Cotizaciones.Campo_Folio +
                " LIKE '%" + Datos_Cotizacion.P_Folio + "%'" +
                " ORDER BY " + Ope_Com_Cotizaciones.Campo_No_Cotizacion;
            }

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Conceptos
        ///DESCRIPCIÓN: Metodo que consulta los conceptos que contiene las requisicion de la cotizacion
        ///PARAMETROS: 1.- Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion, objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Conceptos(Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "SELECT * FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion + "='" + Datos_Cotizacion.P_No_Cotizacion.Trim()+ "'";
            DataTable  Dt_Cotizacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            
            Mi_SQL = "SELECT CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID +
                ", CONCEPTO." + Cat_Sap_Concepto.Campo_Clave +
                " ||''|| CONCEPTO." + Cat_Sap_Concepto.Campo_Descripcion +
                " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " CONCEPTO"  +
                " JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica +" PARTIDA_GEN" +
                " ON PARTIDA_GEN." + Cat_SAP_Partida_Generica.Campo_Concepto_ID + " = CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID +
                " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDA_ESP" +
                " ON PARTIDA_ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + "=" +
                "PARTIDA_GEN." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + 
                " JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " PRODUCTO_REQ" +
                " ON PRODUCTO_REQ." + Ope_Com_Req_Producto.Campo_Partida_ID + " = PARTIDA_ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                " WHERE PRODUCTO_REQ." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " IN (" +Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Lista_Requisiciones]+ ")" +
                " GROUP BY (CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID + ", CONCEPTO." + Cat_Sap_Concepto.Campo_Clave +
                ",CONCEPTO." + Cat_Sap_Concepto.Campo_Descripcion + ")" +
                " ORDER BY CONCEPTO." + Cat_Sap_Concepto.Campo_Clave;

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Detalle_Cotizaciones
        ///DESCRIPCIÓN: Metodo que consulta los detalles conrespecto al concepto y cotizador asignado a la cotizacion
        ///PARAMETROS: 1.- Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion, objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Detalle_Cotizaciones(Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "SELECT COT." + Ope_Com_Det_Cotizaciones.Campo_Giro_ID + " AS CONCEPTO_ID" +
                            ", (SELECT " + Cat_Sap_Concepto.Campo_Descripcion +
                            " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto +
                            " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID +
                            "= COT." + Ope_Com_Det_Cotizaciones.Campo_Giro_ID + ") AS DESCRIPCION_CONCEPTO" +
                            ", (SELECT " + Cat_Sap_Concepto.Campo_Clave +
                            " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto +
                            " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID +
                            "= COT." + Ope_Com_Det_Cotizaciones.Campo_Giro_ID + ") AS CLAVE_CONCEPTO" +
                            ", COT." + Ope_Com_Det_Cotizaciones.Campo_Empleado_ID +
                            ", (SELECT " + Cat_Empleados.Campo_Nombre +
                            " || ' ' || " + Cat_Empleados.Campo_Apellido_Paterno +
                            " || ' ' || " + Cat_Empleados.Campo_Apellido_Materno +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                            "= COT." + Ope_Com_Det_Cotizaciones.Campo_Empleado_ID + ") AS NOMBRE_EMPLEADO " +
                            " FROM " + Ope_Com_Det_Cotizaciones.Tabla_Ope_Com_Det_Cotizaciones + " COT" +
                            " WHERE COT." + Ope_Com_Det_Cotizaciones.Campo_No_Cotizacion +
                            " ='" + Datos_Cotizacion.P_No_Cotizacion + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Cotizadores
        ///DESCRIPCIÓN: Metodo que consulta los Cotizadores que cumplen con el concepto para esta cottizacion
        ///PARAMETROS: 1.- Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion, objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 09/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Cotizadores(Cls_Ope_Com_Asignar_Cotizadores_Negocio Datos_Cotizacion)
        {
            String Mi_SQL = "SELECT COT." + Cat_Com_Cotizadores.Campo_Empleado_ID +
                    ", (SELECT " + Cat_Empleados.Campo_Nombre +
                    " || ' ' || " + Cat_Empleados.Campo_Apellido_Paterno +
                    " || ' ' || " + Cat_Empleados.Campo_Apellido_Materno +
                    " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                    " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                    "= COT." + Ope_Com_Det_Cotizaciones.Campo_Empleado_ID + ")" +
                    " FROM " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores + " COT" +
                    " JOIN " + Cat_Com_Det_Cotizadores.Tabla_Cat_Com_Det_Cotizadores + " DET" +
                    " ON DET." + Cat_Com_Det_Cotizadores.Campo_Empleado_ID +
                    "= COT." + Cat_Com_Cotizadores.Campo_Empleado_ID +
                    " WHERE DET." + Cat_Com_Det_Cotizadores.Campo_Giro_ID +
                    "='" + Datos_Cotizacion.P_Concepto_ID + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
        #endregion Fin Consultas


    }//fin del class
}//fin de namespace