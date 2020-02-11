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
using Presidencia.Control_Patrimonial.Cargar_Tipo_Movimiento.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Ope_Pat_Cargar_Tipo_Movimiento_Datos
/// </summary>
namespace Presidencia.Control_Patrimonial.Cargar_Tipo_Movimiento.Datos { 
    public class Cls_Ope_Pat_Cargar_Tipo_Movimiento_Datos {
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Vehiculos
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Parametros.  Contiene los parametros que se van a utilizar para
            ///                                             hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Diciembre/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Detalles_Vehiculos(Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Parametros)  {
                String Mi_SQL = null;
                DataSet Ds_Resultado = null;
                DataTable Dt_Resultado = new DataTable();
                try {
                    Mi_SQL = "SELECT PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS BIEN_ID";
                    Mi_SQL = Mi_SQL+ ", PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Estatus + " AS ESTATUS_BIEN";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " AS FECHA_INICIAL";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " AS FECHA_BAJA";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta + " AS MOV_ALTA";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Modificacion + " AS MOV_MODIF";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja + " AS MOV_BAJA";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + " PRINCIPAL";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS";
                    Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + " WHERE RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO'";
                    if (!String.IsNullOrEmpty(Parametros.P_Estatus)) Mi_SQL = Mi_SQL + " AND PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Estatus + " IN ('" + Parametros.P_Estatus + "')";
                    Mi_SQL = Mi_SQL + " ORDER BY RESGUARDOS." + Parametros.P_Order;
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Resultado != null) {
                        Dt_Resultado = Ds_Resultado.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Resultado;
            }
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Animales
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Parametros.  Contiene los parametros que se van a utilizar para
            ///                                             hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Diciembre/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Detalles_Animales(Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Parametros)  {
                String Mi_SQL = null;
                DataSet Ds_Resultado = null;
                DataTable Dt_Resultado = new DataTable();
                try {
                    Mi_SQL = "SELECT PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS BIEN_ID";
                    Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Estatus + " AS ESTATUS_BIEN";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " AS FECHA_INICIAL";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " AS FECHA_BAJA";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta + " AS MOV_ALTA";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Modificacion + " AS MOV_MODIF";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja + " AS MOV_BAJA";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + " PRINCIPAL";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS";
                    Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + " WHERE RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CEMOVIENTE'";
                    if (!String.IsNullOrEmpty(Parametros.P_Estatus)) Mi_SQL = Mi_SQL + " AND PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Estatus + " IN ('" + Parametros.P_Estatus + "')";
                    Mi_SQL = Mi_SQL + " ORDER BY RESGUARDOS." + Parametros.P_Order;
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Resultado != null) {
                        Dt_Resultado = Ds_Resultado.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Resultado;
            }
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_BM_Resguardos
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Parametros.  Contiene los parametros que se van a utilizar para
            ///                                             hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Diciembre/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Detalles_BM_Resguardos(Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Parametros)  {
                String Mi_SQL = null;
                DataSet Ds_Resultado = null;
                DataTable Dt_Resultado = new DataTable();
                try {
                    Mi_SQL = "SELECT PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                    Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " AS ESTATUS_BIEN";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " AS FECHA_INICIAL";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " AS FECHA_BAJA";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta + " AS MOV_ALTA";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Modificacion + " AS MOV_MODIF";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja + " AS MOV_BAJA";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " PRINCIPAL";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS";
                    Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + " WHERE RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                    if (!String.IsNullOrEmpty(Parametros.P_Estatus)) Mi_SQL = Mi_SQL + " AND PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " IN ('" + Parametros.P_Estatus + "')";
                    Mi_SQL = Mi_SQL + " ORDER BY RESGUARDOS." + Parametros.P_Order;
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Resultado != null) {
                        Dt_Resultado = Ds_Resultado.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Resultado;
            }
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_BM_Recibos
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Parametros.  Contiene los parametros que se van a utilizar para
            ///                                             hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Diciembre/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Detalles_BM_Recibos(Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Parametros)  {
                String Mi_SQL = null;
                DataSet Ds_Resultado = null;
                DataTable Dt_Resultado = new DataTable();
                try {
                    Mi_SQL = "SELECT PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                    Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " AS ESTATUS_BIEN";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + " AS BIEN_RESGUARDO_ID";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial + " AS FECHA_INICIAL";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final + " AS FECHA_BAJA";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Movimiento_Alta + " AS MOV_ALTA";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Movimiento_Modificacion + " AS MOV_MODIF";
                    Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Movimiento_Baja + " AS MOV_BAJA";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " PRINCIPAL";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + " RESGUARDOS";
                    Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + " WHERE RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                    if (!String.IsNullOrEmpty(Parametros.P_Estatus)) Mi_SQL = Mi_SQL + " AND PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " IN ('" + Parametros.P_Estatus + "')";
                    Mi_SQL = Mi_SQL + " ORDER BY RESGUARDOS." + Parametros.P_Order;
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Resultado != null) {
                        Dt_Resultado = Ds_Resultado.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Resultado;
            }
       
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Actualizar_Movimientos
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Parametros.  Contiene los parametros que se van a utilizar para
            ///                                             hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Diciembre/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Actualizar_Movimientos(Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Parametros)  {
                String Mi_SQL = null;
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                try {
                    foreach (DataRow Fila_Actual in Parametros.P_Aplicar_Movimiento.Rows) {
                        Mi_SQL = "UPDATE " + Fila_Actual["TABLA"].ToString().Trim();
                        Mi_SQL = Mi_SQL + " SET " + Fila_Actual["COLUMNA"].ToString().Trim() + " = '" + Fila_Actual["VALOR"].ToString().Trim() + "'";
                        Mi_SQL = Mi_SQL + " WHERE " + Fila_Actual["COLUMNA_NO_REGISTRO"].ToString().Trim() + " = '" + Fila_Actual["NO_REGISTRO"].ToString().Trim() + "'";
                        Mi_SQL = Mi_SQL + " AND " + Fila_Actual["COLUMNA_TIPO"].ToString().Trim() + " = '" + Fila_Actual["TIPO"].ToString().Trim() + "'";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                    Trans.Commit();
               } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else if (Ex.Code == 2627) {
                        if (Ex.Message.IndexOf("PRIMARY") != -1) {
                            Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                            Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        } else {
                            Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        }
                    } else if (Ex.Code == 547) {
                        Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else if (Ex.Code == 515) {
                        Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else {
                        Mensaje = "Error al intentar dar de Alta. Error: [" + Ex.Message + "][" + Mi_SQL + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Actualizacion_Dependencias
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Parametros.  Contiene los parametros que se van a utilizar para
            ///                                             hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Diciembre/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Actualizacion_Dependencias(Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Parametros)  {
                String Mi_SQL = null;
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                try {
                    Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RES SET RES." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = (SELECT EMP." + Cat_Empleados.Campo_Dependencia_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMP WHERE EMP." + Cat_Empleados.Campo_Empleado_ID + " = res." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + ") WHERE RES." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " IS NULL";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Mi_SQL = "UPDATE OPE_PAT_BIENES_RECIBOS RES SET RES.DEPENDENCIA_ID = (SELECT EMP.DEPENDENCIA_ID FROM CAT_EMPLEADOS EMP WHERE EMP.EMPLEADO_ID = res.empleado_recibo_id) WHERE RES.DEPENDENCIA_ID IS NULL";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
               } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else if (Ex.Code == 2627) {
                        if (Ex.Message.IndexOf("PRIMARY") != -1) {
                            Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                            Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        } else {
                            Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        }
                    } else if (Ex.Code == 547) {
                        Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else if (Ex.Code == 515) {
                        Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else {
                        Mensaje = "Error al intentar dar de Alta. Error: [" + Ex.Message + "][" + Mi_SQL + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Actualizacion_Observaciones
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Parametros.  Contiene los parametros que se van a utilizar para
            ///                                             hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Diciembre/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Actualizacion_Observaciones(Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Parametros)  {
                String Mi_SQL = null;
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                try {
                    Mi_SQL = "UPDATE OPE_PAT_BIENES_RESGUARDOS RES SET RES.OBSERVACIONES = (SELECT BM.OBSERVACIONES FROM OPE_PAT_BIENES_MUEBLES BM WHERE BM.BIEN_MUEBLE_ID = RES.BIEN_ID AND BM.OPERACION = 'RESGUARDO') WHERE RES.TIPO ='BIEN_MUEBLE' AND RES.OBSERVACIONES IS NULL";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Mi_SQL = "UPDATE OPE_PAT_BIENES_RECIBOS RES SET RES.OBSERVACIONES = (SELECT BM.OBSERVACIONES FROM OPE_PAT_BIENES_MUEBLES BM WHERE BM.BIEN_MUEBLE_ID = RES.BIEN_ID AND BM.OPERACION = 'RECIBO') WHERE RES.TIPO ='BIEN_MUEBLE' AND RES.OBSERVACIONES IS NULL";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Mi_SQL = "UPDATE OPE_PAT_BIENES_RESGUARDOS RES SET RES.OBSERVACIONES = (SELECT BM.OBSERVACIONES FROM OPE_PAT_VEHICULOS BM WHERE BM.VEHICULO_ID = RES.BIEN_ID) WHERE RES.TIPO ='VEHICULO' AND RES.OBSERVACIONES IS NULL";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Mi_SQL = "UPDATE OPE_PAT_BIENES_RESGUARDOS RES SET RES.OBSERVACIONES = (SELECT BM.OBSERVACIONES FROM OPE_PAT_CEMOVIENTES BM WHERE BM.CEMOVIENTE_ID = RES.BIEN_ID) WHERE RES.TIPO ='CEMOVIENTE' AND RES.OBSERVACIONES IS NULL";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
               } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else if (Ex.Code == 2627) {
                        if (Ex.Message.IndexOf("PRIMARY") != -1) {
                            Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                            Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        } else {
                            Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        }
                    } else if (Ex.Code == 547) {
                        Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else if (Ex.Code == 515) {
                        Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else {
                        Mensaje = "Error al intentar dar de Alta. Error: [" + Ex.Message + "][" + Mi_SQL + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Actualizacion_Estados
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Parametros.  Contiene los parametros que se van a utilizar para
            ///                                             hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Diciembre/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Actualizacion_Estados(Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Parametros)  {
                String Mi_SQL = null;
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                try {
                    Mi_SQL = "UPDATE OPE_PAT_BIENES_RESGUARDOS RES SET RES.ESTADO = (SELECT BM.ESTADO FROM OPE_PAT_BIENES_MUEBLES BM WHERE BM.BIEN_MUEBLE_ID = res.BIEN_ID AND BM.OPERACION = 'RESGUARDO') WHERE RES.TIPO ='BIEN_MUEBLE' AND RES.ESTADO IS NULL";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Mi_SQL = "UPDATE OPE_PAT_BIENES_RECIBOS RES SET RES.ESTADO = (SELECT BM.ESTADO FROM OPE_PAT_BIENES_MUEBLES BM WHERE BM.BIEN_MUEBLE_ID = res.BIEN_ID AND BM.OPERACION = 'RECIBO') WHERE RES.TIPO ='BIEN_MUEBLE' AND RES.ESTADO IS NULL";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
               } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else if (Ex.Code == 2627) {
                        if (Ex.Message.IndexOf("PRIMARY") != -1) {
                            Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                            Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        } else {
                            Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        }
                    } else if (Ex.Code == 547) {
                        Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else if (Ex.Code == 515) {
                        Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else {
                        Mensaje = "Error al intentar dar de Alta. Error: [" + Ex.Message + "][" + Mi_SQL + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Actualizacion_Estados_Alta
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Parametros.  Contiene los parametros que se van a utilizar para
            ///                                             hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Diciembre/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Actualizacion_Estados_Alta(Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Parametros)  {
                String Mi_SQL = null;
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                try {
                    Mi_SQL = "UPDATE OPE_PAT_BIENES_RESGUARDOS SET ESTADO = 'BUENO' WHERE MOVIMIENTO_ALTA = 'SI'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Mi_SQL = "UPDATE OPE_PAT_BIENES_RECIBOS SET ESTADO = 'BUENO' WHERE MOVIMIENTO_ALTA = 'SI'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
               } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else if (Ex.Code == 2627) {
                        if (Ex.Message.IndexOf("PRIMARY") != -1) {
                            Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                            Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        } else {
                            Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        }
                    } else if (Ex.Code == 547) {
                        Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else if (Ex.Code == 515) {
                        Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else {
                        Mensaje = "Error al intentar dar de Alta. Error: [" + Ex.Message + "][" + Mi_SQL + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Actualizacion_Estados
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Parametros.  Contiene los parametros que se van a utilizar para
            ///                                             hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Diciembre/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Actualizacion_Empleados_Antiguos(Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Parametros)  {
                String Mi_SQL = null;
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                try {
                    foreach (DataRow Fila_Actual in Parametros.P_Movimientos_Archivo.Rows)
                    {
                        Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos
                            + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = '" + Fila_Actual["DEPENDENCIA_ID"].ToString().Trim() + "'"
                            + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Fila_Actual["EMPLEADO_ID"].ToString().Trim() + "'"
                            + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " IS NULL";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos
                            + " SET " + Ope_Pat_Bienes_Recibos.Campo_Dependencia_ID + " = '" + Fila_Actual["DEPENDENCIA_ID"].ToString().Trim() + "'"
                            + " WHERE " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + " = '" + Fila_Actual["EMPLEADO_ID"].ToString().Trim() + "'"
                            + " AND " + Ope_Pat_Bienes_Recibos.Campo_Dependencia_ID + " IS NULL";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                    Trans.Commit();
               } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else if (Ex.Code == 2627) {
                        if (Ex.Message.IndexOf("PRIMARY") != -1) {
                            Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                            Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        } else {
                            Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                        }
                    } else if (Ex.Code == 547) {
                        Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else if (Ex.Code == 515) {
                        Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "][" + Mi_SQL + "]";
                    } else {
                        Mensaje = "Error al intentar dar de Alta. Error: [" + Ex.Message + "][" + Mi_SQL + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }
    }
}