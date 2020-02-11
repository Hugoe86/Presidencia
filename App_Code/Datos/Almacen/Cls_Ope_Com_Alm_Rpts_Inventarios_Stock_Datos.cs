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
using Presidencia.Reportes_Inventarios_Stock.Negocio;


/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Datos
/// </summary>
/// 

namespace Presidencia.Reportes_Inventarios_Stock.Datos
{
    public class Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Datos
    {
        public Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Datos()
        {
        }

        #region Metodos
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Reporte_Inventarios
        ///DESCRIPCIÓN:          Método utilizado para consultar un reporte de inventarios generados
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           03/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Inventarios_Stock(Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataSet Ds_Consulta = null;
            DataTable Dt_Consulta = new DataTable();
            Boolean where =true;

        try
        {
            Mi_SQL = " SELECT " + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario + "";
            Mi_SQL= Mi_SQL + ", " + Ope_Com_Cap_Inv_Stock.Campo_Usuario_Creo + "";
            Mi_SQL= Mi_SQL + ", " + Ope_Com_Cap_Inv_Stock.Campo_Fecha_Creo + "";
            Mi_SQL= Mi_SQL + ", " + Ope_Com_Cap_Inv_Stock.Campo_Estatus + "";
            Mi_SQL= Mi_SQL + ", " + Ope_Com_Cap_Inv_Stock.Campo_Tipo + " AS FILTRO_SELECCIONADO";
            Mi_SQL= Mi_SQL + ", " + Ope_Com_Cap_Inv_Stock.Campo_Observaciones + "";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock;

            if(Datos.P_Estatus !=null){
                 
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Cap_Inv_Stock.Campo_Estatus;
                Mi_SQL = Mi_SQL + " = '" + Datos.P_Estatus + "'";

                where=false;
            }

            if(Datos.P_Empleado_Creo !=null){
                 
                if(where == true){

                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Cap_Inv_Stock.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + " = '" + Datos.P_Empleado_Creo + "'";
                    where=false;
                }else{
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Cap_Inv_Stock.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + " = '" + Datos.P_Empleado_Creo + "'";
                }
            }

             if(Datos.P_Fecha_Inicial !=null){
                 
                if(where == true){
                     Mi_SQL = Mi_SQL + " WHERE TO_DATE(TO_CHAR(" + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock+ "." + Ope_Com_Cap_Inv_Stock.Campo_Fecha_Creo + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
                     " AND '" + Datos.P_Fecha_Final+ "'";
                     where=false;
                }else{

                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock + "." + Ope_Com_Cap_Inv_Stock.Campo_Fecha_Creo + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
                    " AND '" + Datos.P_Fecha_Final + "'";
                }
            }

             if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
             {
                 Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
             }
         
             return Dt_Consulta;
        }
        catch (Exception Ex)
        {
            String Mensaje = "Error al intentar consultar los inventarios. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
           
      }



        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Ajustes_Inventario
        ///DESCRIPCIÓN:          Método utilizado para consultar los ajustes de un inventario
        ///                      determinado
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           03/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Ajustes_Inventario(Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataTable Dt_consulta = new DataTable();

            try
            {
                Mi_SQL = " SELECT INV_STOCK."  + Ope_Com_Cap_Inv_Stock.Campo_Usuario_Creo + "";
                Mi_SQL = Mi_SQL + ", INV_STOCK."  + Ope_Com_Cap_Inv_Stock.Campo_Fecha_Creo + "";
                Mi_SQL = Mi_SQL + ", INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_Estatus + "";
                Mi_SQL = Mi_SQL + ", AJUSTES_INV_STOCK."  + Ope_Com_Ajustes_Inv_Stock.Campo_No_Inventario + "";
                Mi_SQL = Mi_SQL + ", AJUSTES_INV_STOCK." + Ope_Com_Ajustes_Inv_Stock.Campo_Usuario_Ajusto + "";
                Mi_SQL = Mi_SQL + ", AJUSTES_INV_STOCK."  + Ope_Com_Ajustes_Inv_Stock.Campo_Fecha + " AS FECHA_AJUSTE";
                Mi_SQL = Mi_SQL + ", AJUSTES_INV_STOCK."  + Ope_Com_Ajustes_Inv_Stock.Campo_Tipo_Ajuste + "";
                Mi_SQL = Mi_SQL + ", AJUSTES_INV_STOCK."  + Ope_Com_Ajustes_Inv_Stock.Campo_Justificacion + "";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ajustes_Inv_Stock.Tabla_Ope_Com_Ajustes_Inv_Stock + " AJUSTES_INV_STOCK, ";
                Mi_SQL = Mi_SQL + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock + " INV_STOCK ";
                Mi_SQL = Mi_SQL + " WHERE INV_STOCK." +  Ope_Com_Cap_Inv_Stock.Campo_No_Inventario + "='" + Datos.P_No_Inventario + "'";
                Mi_SQL = Mi_SQL + " AND AJUSTES_INV_STOCK." + Ope_Com_Ajustes_Inv_Stock.Campo_No_Inventario + "='" + Datos.P_No_Inventario + "'";
                
                // Ejecutar consulta
                Dt_consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_consulta;
             }
             catch (Exception Ex)
             {
                String Mensaje = "Error al intentar consultar los ajustes del inventario. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
             }
        }



        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Empleados
        ///DESCRIPCIÓN:          Método utilizado para consultar los empleados que han realizado reportes
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           03/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Empleados(Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataSet Ds_Consulta = null;
            DataTable Dt_consulta = new DataTable();

            try
             {
                 Mi_SQL = "SELECT DISTINCT " + Ope_Com_Cap_Inv_Stock.Campo_Usuario_Creo + "";
                 Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock;
                
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Consulta != null)
                {
                    Dt_consulta= Ds_Consulta.Tables[0];
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Numeros_Inventarios
        ///DESCRIPCIÓN:          Método utilizado para consultar los Numeros de Inventarios
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           03/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Numeros_Inventarios(Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataSet Ds_Consulta = null;
            DataTable Dt_consulta = new DataTable();

            try
            {
                Mi_SQL = "SELECT DISTINCT " + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario + "";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Cap_Inv_Stock.Campo_Estatus +  " <> 'PENDIENTE'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario;

                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Consulta != null)
                {
                    Dt_consulta = Ds_Consulta.Tables[0];
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
