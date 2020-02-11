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
using Presidencia.Operacion_Predial_Constancias.Negocio;
using Presidencia.Catalogo_Tipos_Constancias.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Operacion_Cierre_Definitivo_Dia.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Pre_Cierre_Definitivo_Dia_Datos
/// </summary>
/// 
namespace Presidencia.Operacion_Cierre_Definitivo_Dia.Datos
{
    public class Cls_Ope_Pre_Cierre_Definitivo_Dia_Datos
    {
        public Cls_Ope_Pre_Cierre_Definitivo_Dia_Datos()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajas
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///CREO: Christian Perez Ibarra
        ///FECHA_CREO: 21/Sept/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Impresion(Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio Datos)
        {
            DataTable tabla = new DataTable();
            try
            {
                for(int x=0;x<Datos .P_Dt_Cierre .Rows .Count ;x++){
                    String Mi_SQL = "SELECT " + Ope_Caj_Pagos .Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Corriente + ",";
                    Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Descripcion + ",";

                    Mi_SQL = Mi_SQL + "( Select " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Corriente + " ";
                    Mi_SQL = Mi_SQL + " From " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " inner join ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " on";
                     Mi_SQL =Mi_SQL + Cat_Pre_Modulos .Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos .Campo_Modulo_Id + "=";
                    Mi_SQL =Mi_SQL + Cat_Pre_Cajas .Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas .Campo_Modulo_Id + " inner join ";
                    Mi_SQL =Mi_SQL + Ope_Caj_Pagos .Tabla_Ope_Caj_Pagos + " on ";
                    Mi_SQL =Mi_SQL + Ope_Caj_Pagos .Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos .Campo_Caja_ID + " =" ;
                    Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_ID+ " ";
                    Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + "='";
                    Mi_SQL = Mi_SQL + Datos.P_Dt_Cierre.Rows[x]["Modulo_ID"].ToString().Trim() + "' and ";
                    Mi_SQL = Mi_SQL + " Extract ( Month From " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + ")=";
                    Mi_SQL = Mi_SQL + " EXTRACT(MONTH FROM SYSDATE)) AS Monto_Mes ";

                    Mi_SQL = Mi_SQL + "( Select " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Monto_Corriente + " ";
                    Mi_SQL = Mi_SQL + " From " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " inner join ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " on";
                    Mi_SQL = Mi_SQL + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + "=";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Modulo_Id + " inner join ";
                    Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " on ";
                    Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + " =";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_ID + " ";
                    Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + "='";
                    Mi_SQL = Mi_SQL + Datos.P_Dt_Cierre.Rows[x]["Modulo_ID"].ToString().Trim() + "' and ";
                    Mi_SQL = Mi_SQL + " Extract ( Year From " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + ")=";
                    Mi_SQL = Mi_SQL + " EXTRACT(YEAR FROM SYSDATE)) AS Monto_Anio ";

                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Modulos .Tabla_Cat_Pre_Modulo  + " inner join ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " on ";
                    Mi_SQL =Mi_SQL + Cat_Pre_Modulos .Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos .Campo_Modulo_Id + "=";
                    Mi_SQL =Mi_SQL + Cat_Pre_Cajas .Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas .Campo_Modulo_Id + " inner join ";
                    Mi_SQL =Mi_SQL + Ope_Caj_Pagos .Tabla_Ope_Caj_Pagos + " on ";
                    Mi_SQL =Mi_SQL + Ope_Caj_Pagos .Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos .Campo_Caja_ID + " =" ;
                    Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_ID + " inner join ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "on ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + "=";
                    Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Clave_Ingreso_ID + " ";
                    Mi_SQL =Mi_SQL + " Where " + Cat_Pre_Modulos .Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos .Campo_Modulo_Id + "='";
                    Mi_SQL =Mi_SQL + Datos .P_Dt_Cierre .Rows [x]["Modulo_ID"].ToString ().Trim ()+ "'";
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }
    


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajas
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///CREO: Christian Perez Ibarra
        ///FECHA_CREO: 21/Sept/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cajero(Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio Datos)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Caj_Turnos .Campo_Usuario_Creo + ",";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
                Mi_SQL = Mi_SQL + " Where " + Ope_Caj_Turnos.Campo_Caja_ID  + "='" + Datos.P_Caja_ID  + "'";
                //Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Modulos.Campo_Modulo_Id + " Desc";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajas
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///CREO: Christian Perez Ibarra
        ///FECHA_CREO: 21/Sept/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Llenar_Combo_Caja(Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio Datos)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Cajas .Campo_Caja_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Campo_Numero_De_Caja ;
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " Where " + Cat_Pre_Cajas.Campo_Modulo_Id + "='" + Datos.P_Modulo_ID+ "'";
                //Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Modulos.Campo_Modulo_Id + " Desc";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajas
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///CREO: Christian Perez Ibarra
        ///FECHA_CREO: 21/Sept/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Llenar_Combo_Modulos(Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio Datos)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Modulos.Campo_Modulo_Id + ",";
                Mi_SQL = Mi_SQL + Cat_Pre_Modulos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " ;
                Mi_SQL = Mi_SQL + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Modulos .Campo_Modulo_Id  + " Desc";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajas
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///CREO: Christian Perez Ibarra
        ///FECHA_CREO: 21/Sept/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cierres_Dia(Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio Datos)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Caj_Cierre_Dia.Tabla_Ope_Caj_Cierre_Dia + "." + Ope_Caj_Cierre_Dia.Campo_No_Cierre_Dia + ",";
                Mi_SQL =Mi_SQL + Ope_Caj_Cierre_Dia .Tabla_Ope_Caj_Cierre_Dia + "." +Ope_Caj_Cierre_Dia .Campo_Fecha_Cierre_Dia + ",";
                Mi_SQL =Mi_SQL + Ope_Caj_Cierre_Dia .Tabla_Ope_Caj_Cierre_Dia + "." +Ope_Caj_Cierre_Dia .Campo_Estatus  + ",";
                Mi_SQL =Mi_SQL + Ope_Caj_Cierre_Dia .Tabla_Ope_Caj_Cierre_Dia + "." + Ope_Caj_Cierre_Dia .Campo_Usuario_Creo + ",";
                Mi_SQL =Mi_SQL + Cat_Pre_Modulos .Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos .Campo_Clave + " as Modulo";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Caj_Cierre_Dia.Tabla_Ope_Caj_Cierre_Dia + " inner join ";
                Mi_SQL = Mi_SQL + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " on ";
                Mi_SQL = Mi_SQL + Ope_Caj_Cierre_Dia.Tabla_Ope_Caj_Cierre_Dia + "." + Ope_Caj_Cierre_Dia.Campo_Modulo_Id + "=";
                Mi_SQL = Mi_SQL + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id;
                Mi_SQL = Mi_SQL + " and ";
                //comparar con la fecha del dia de hoy
                Mi_SQL = Mi_SQL + Ope_Caj_Cierre_Dia.Tabla_Ope_Caj_Cierre_Dia + "." + Ope_Caj_Cierre_Dia.Campo_Fecha_Cierre_Dia + "='getdate()'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Caj_Cierre_Dia.Tabla_Ope_Caj_Cierre_Dia + "." + Ope_Caj_Cierre_Dia.Campo_No_Cierre_Dia + " Desc";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }
    }
}
