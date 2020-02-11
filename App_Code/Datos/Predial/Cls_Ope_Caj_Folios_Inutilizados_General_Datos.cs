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
using Presidencia;
using Presidencia.Folios_Inutilizados_General_Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;

namespace Presidencia.Folios_Inutilizados_General_Datos {
    public class Cls_Ope_Caj_Folios_Inutilizados_General_Datos
    {
            //*******************************************************************************
            //NOMBRE DE LA FUNCIÓN : Consultar_Modulos
            //DESCRIPCIÓN          : Metodo para consultar los modulos de las cajas
            //PARAMETROS           :   
            //CREO                 : Leslie González Vázquez
            //FECHA_CREO           : 21/octubre/2011 
            //MODIFICO             :
            //FECHA_MODIFICO       :
            //CAUSA_MODIFICACIÓN   :
            //*******************************************************************************
             public static DataTable Consultar_Modulos()
            {
                DataTable Tabla = new DataTable();
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Modulos.Campo_Modulo_Id  ;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Descripcion ;
                    Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo ;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Modulos.Campo_Estatus + " = 'VIGENTE'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Modulos.Campo_Descripcion;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        Tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de los modulos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Tabla;
            }

             //*******************************************************************************
             //NOMBRE DE LA FUNCIÓN : Consultar_Cajas
             //DESCRIPCIÓN          : Metodo para consultar las cajas de  los modulos 
             //PARAMETROS           : Folios_Negocio conexion con la capa de negocios  
             //CREO                 : Leslie González Vázquez
             //FECHA_CREO           : 21/octubre/2011 
             //MODIFICO             :
             //FECHA_MODIFICO       :
             //CAUSA_MODIFICACIÓN   :
             //*******************************************************************************
             public static DataTable Consultar_Cajas(Cls_Ope_Caj_Folios_Inutilizados_General_Negocio Folios_Negocio)
             {
                 DataTable Tabla = new DataTable();
                 try
                 {
                     String Mi_SQL = "SELECT " + Cat_Pre_Cajas.Campo_Caja_ID;
                     Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Numero_De_Caja;
                     Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja ;
                     Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cajas.Campo_Modulo_Id + " = '" + Folios_Negocio.P_Modulo_ID + "'";
                     Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cajas.Campo_Estatus + " = 'VIGENTE'";
                     Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Cajas.Campo_Numero_De_Caja;
                     DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                     if (dataset != null)
                     {
                         Tabla = dataset.Tables[0];
                     }
                 }
                 catch (Exception Ex)
                 {
                     String Mensaje = "Error al intentar consultar los registros de las cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                     throw new Exception(Mensaje);
                 }
                 return Tabla;
             }

             //*******************************************************************************
             //NOMBRE DE LA FUNCIÓN : Consultar_Folios
             //DESCRIPCIÓN          : Metodo para consultar los folios inutilizados
             //PARAMETROS           : Folios_Negocio conexion con la capa de negocios   
             //CREO                 : Leslie González Vázquez
             //FECHA_CREO           : 21/octubre/2011 
             //MODIFICO             :
             //FECHA_MODIFICO       :
             //CAUSA_MODIFICACIÓN   :
             //*******************************************************************************
             public static DataTable Consultar_Folios(Cls_Ope_Caj_Folios_Inutilizados_General_Negocio Folios_Negocio)
             {
                 DataTable Tabla = new DataTable();
                 String Mi_SQL = string.Empty;
                 try
                 {
                     Mi_SQL = "SELECT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo +", " ;
                     Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + ", ";
                     Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha+ ", ";
                     Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID + ", ";
                     Mi_SQL = Mi_SQL + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Observaciones + ", ";
                     Mi_SQL = Mi_SQL + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos  + "." + Ope_Caj_Turnos.Campo_Empleado_ID  + ", ";
                     Mi_SQL = Mi_SQL + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + ", ";
                     Mi_SQL = Mi_SQL + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos  + "." + Cat_Pre_Motivos.Campo_Nombre  + " AS MOTIVO, ";
                     Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "."  + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
                     Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "."  + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
                     Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "."  + Cat_Empleados.Campo_Apellido_Materno + " AS EMPLEADO";
                     Mi_SQL = Mi_SQL + " FROM " +  Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos ;
                     Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " +  Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
                     Mi_SQL = Mi_SQL + " ON " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Turno;
                     Mi_SQL = Mi_SQL + " = " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno;
                     Mi_SQL = Mi_SQL + " AND " +  Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID;
                     Mi_SQL = Mi_SQL + " = " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_ID;
                     Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                     Mi_SQL = Mi_SQL + " ON " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos  + "." + Ope_Caj_Pagos.Campo_Caja_ID;
                     Mi_SQL = Mi_SQL + " = " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_ID;
                     Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados;
                     Mi_SQL = Mi_SQL + " ON " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Empleado_ID;
                     Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                     Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos;
                     Mi_SQL = Mi_SQL + " ON " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID;
                     Mi_SQL = Mi_SQL + " = " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos + "." + Cat_Pre_Motivos.Campo_Motivo_ID ;
                     Mi_SQL = Mi_SQL + " WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Estatus + " = 'INUTILIZADO'";

                     if (!string.IsNullOrEmpty(Folios_Negocio.P_Caja_ID)){
                         Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + " = '"+Folios_Negocio.P_Caja_ID +"'";
                     }

                     if (!string.IsNullOrEmpty(Folios_Negocio.P_Empleado_ID))
                     {
                         Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos  + "." + Ope_Caj_Turnos.Campo_Empleado_ID  + " = '" + Folios_Negocio.P_Empleado_ID + "'";
                     }
                     if (!string.IsNullOrEmpty(Folios_Negocio.P_Modulo_ID))
                     {
                         Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja  + "." + Cat_Pre_Cajas.Campo_Modulo_Id + " = '" + Folios_Negocio.P_Modulo_ID+ "'";
                     }
                     if (!string.IsNullOrEmpty(Folios_Negocio.P_Fecha_Inicio) && !string.IsNullOrEmpty(Folios_Negocio.P_Fecha_Fin))
                     {
                         Mi_SQL = Mi_SQL + " AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Folios_Negocio.P_Fecha_Inicio)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')"+
                             " AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Folios_Negocio.P_Fecha_Fin)) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                     }
                     DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                     if (dataset != null)
                     {
                         Tabla = dataset.Tables[0];
                     }
                 }
                 catch (Exception Ex)
                 {
                     String Mensaje = "Error al intentar consultar los registros de los folios. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                     throw new Exception(Mensaje);
                 }
                 return Tabla;
             }
    }
}

