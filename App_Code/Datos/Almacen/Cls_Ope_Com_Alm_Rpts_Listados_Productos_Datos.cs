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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora_Eventos;
using Presidencia.Reportes_Listados_Productos.Negocio;


 

namespace Presidencia.Reportes_Listados_Productos.Datos
{
    public class Cls_Ope_Com_Alm_Rpts_Listados_Productos_Datos
    {
        public Cls_Ope_Com_Alm_Rpts_Listados_Productos_Datos()
        {
        }

        #region Metodos
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Listados_Productos
        ///DESCRIPCIÓN:          Método utilizado para consultar los listados de productos generados
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           07/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Listados_Productos(Cls_Ope_Com_Alm_Rpts_Listados_Productos_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataTable Dt_Consulta = new DataTable();
            Boolean where =true;

        try
        {
            Mi_SQL = " SELECT  LISTADO." + Ope_Com_Listado.Campo_Folio + "";
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Tipo + "";
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Estatus + "";
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Comentarios + "";
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Fecha_Construccion + "";
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_No_Requisicion_ID + "";
            Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Total + " as TOTAL_PRODUCTOS ";
            Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Empleados.Campo_Nombre + " ||' '|| " + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| ";
            Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Materno + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID + " = LISTADO." + Ope_Com_Listado.Campo_Empleado_Construccion_ID;
            Mi_SQL = Mi_SQL + ")  as EMPLEADO_CONSTRUCCION ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado + " LISTADO" ;
           

            if(Datos.P_Estatus !=null){

                Mi_SQL = Mi_SQL + " WHERE LISTADO." + Ope_Com_Listado.Campo_Estatus;
                Mi_SQL = Mi_SQL + " = '" + Datos.P_Estatus + "'";
                where = false;
            }

            if(Datos.P_Empleado_Creo !=null){
                 
                if(where == true){

                    Mi_SQL = Mi_SQL + " WHERE LISTADO." + Ope_Com_Listado.Campo_Empleado_Construccion_ID;
                    Mi_SQL = Mi_SQL + " = '" + Datos.P_Empleado_Creo+ "'";
                    where=false;
                }else{
                    Mi_SQL = Mi_SQL + " AND LISTADO." + Ope_Com_Listado.Campo_Empleado_Construccion_ID;
                    Mi_SQL = Mi_SQL + " = '" + Datos.P_Empleado_Creo + "'";
                }
            }

             if(Datos.P_Fecha_Inicial !=null){
                 
                if(where == true){
                    Mi_SQL = Mi_SQL + " WHERE TO_DATE(TO_CHAR(" + " LISTADO." + Ope_Com_Listado.Campo_Fecha_Generacion + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
                     " AND '" + Datos.P_Fecha_Final + "'";
                     where=false;
                }else{

                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + "LISTADO." + Ope_Com_Listado.Campo_Fecha_Generacion + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
                    " AND '" + Datos.P_Fecha_Final + "'";
                }
            }

             Mi_SQL = Mi_SQL + " ORDER BY LISTADO." + Ope_Com_Listado.Campo_Folio;


             if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
             {
                 Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
             }
         
             return Dt_Consulta;
        }
        catch (Exception Ex)
        {
            String Mensaje = "Error al intentar consultar los listados. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
           
      }



        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Ajustes_Listado
        ///DESCRIPCIÓN:          Método utilizado para consultar los ajustes de un listado
        ///                      determinado
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           09/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Ajustes_Listado(Cls_Ope_Com_Alm_Rpts_Listados_Productos_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataTable Dt_consulta = new DataTable();

            try
            {
                Mi_SQL = " SELECT  LISTADO." + Ope_Com_Listado.Campo_Folio + "";
                Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Tipo + "";
                Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Estatus + "";
                Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Empleados.Campo_Nombre + " ||' '|| " + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Materno + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID + " = LISTADO." + Ope_Com_Listado.Campo_Empleado_Construccion_ID;
                Mi_SQL = Mi_SQL + ")  as EMPLEADO_CONSTRUCCION ";
                Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Fecha_Construccion + "";
                Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_No_Requisicion_ID + "";
                Mi_SQL = Mi_SQL + ", LISTADO." + Ope_Com_Listado.Campo_Total + " as TOTAL_PRODUCTOS ";
                Mi_SQL = Mi_SQL + ", OBS_LISTADO." + Ope_Alm_Com_Obs_Listado.Campo_Estatus + " AS AJUSTE";
                Mi_SQL = Mi_SQL + ", OBS_LISTADO." + Ope_Alm_Com_Obs_Listado.Campo_Fecha_Creo + " as FECHA_AJUSTE";
                Mi_SQL = Mi_SQL + ", OBS_LISTADO." + Ope_Alm_Com_Obs_Listado.Campo_Comentario + " as COMENTARIOS";
                Mi_SQL = Mi_SQL + ", OBS_LISTADO." + Ope_Alm_Com_Obs_Listado.Campo_Usuario_Creo + " as USUARIO_AJUSTO";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado + " LISTADO, ";
                Mi_SQL = Mi_SQL + "" + Ope_Alm_Com_Obs_Listado.Tabla_Ope_Alm_Com_Obs_Listados + " OBS_LISTADO";
                Mi_SQL = Mi_SQL + " WHERE OBS_LISTADO." + Ope_Alm_Com_Obs_Listado.Campo_No_Listado_ID;
                Mi_SQL = Mi_SQL + " = LISTADO." + Ope_Com_Listado.Campo_Listado_ID;
                Mi_SQL = Mi_SQL + " AND OBS_LISTADO." + Ope_Alm_Com_Obs_Listado.Campo_No_Listado_ID + "=";
                Mi_SQL = Mi_SQL + Datos.P_No_Listado;

                // Ejecutar consulta
                Dt_consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_consulta;
             }
             catch (Exception Ex)
             {
                String Mensaje = "Error al intentar consultar los ajustes del listado. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
             }
        }



        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Empleados
        ///DESCRIPCIÓN:          Método utilizado para consultar los empleados que han realizado reportes
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           07/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Empleados(Cls_Ope_Com_Alm_Rpts_Listados_Productos_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataTable Dt_consulta = new DataTable();

            try
             {
                 Mi_SQL = "SELECT DISTINCT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ", ";
                 Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre;
                 Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno;
                 Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " AS EMPLEADO ";
                 Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                 Mi_SQL = Mi_SQL + ", " + Ope_Com_Listado.Tabla_Ope_Com_Listado;
                 Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                 Mi_SQL = Mi_SQL + " = " + Ope_Com_Listado.Tabla_Ope_Com_Listado + "." + Ope_Com_Listado.Campo_Empleado_Construccion_ID;
                 Mi_SQL = Mi_SQL + " ORDER BY EMPLEADO";
                
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Dt_consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
             }
             catch (Exception Ex)
             {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
             }
            return Dt_consulta;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Numeros_Listados
        ///DESCRIPCIÓN:          Método utilizado para consultar los Numeros de Listados y su Folio
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           07/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Numeros_Listados(Cls_Ope_Com_Alm_Rpts_Listados_Productos_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataTable Dt_consulta = new DataTable();

            try      
            {  
                Mi_SQL = "SELECT DISTINCT " + Ope_Com_Listado.Campo_Listado_ID + "";
                Mi_SQL = Mi_SQL + "," + Ope_Com_Listado.Campo_Folio + "";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado;
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Listado.Campo_Listado_ID;

                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Dt_consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_consulta;
        }
        #endregion
    }

}
