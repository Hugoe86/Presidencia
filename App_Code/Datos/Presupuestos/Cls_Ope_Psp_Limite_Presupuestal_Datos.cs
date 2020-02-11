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
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;
using Presidencia.Limite_Presupuestal.Negocio;
using Presidencia.Sessiones;

namespace Presidencia.Limite_Presupuestal.Datos
{
    public class Cls_Ope_Psp_Limite_Presupuestal_Datos
    {
        #region MÉTODOS
            ///*******************************************************************************
            // NOMBRE DE LA FUNCIÓN: Consultar_Unidades_Asignadas
            // DESCRIPCIÓN:  
            // RETORNA: 
            // CREO: Gustavo Angeles Cruz
            // FECHA_CREO: 30/Agosto/2010 
            // MODIFICO:
            // FECHA_MODIFICO
            // CAUSA_MODIFICACIÓN   
            // *******************************************************************************/
            public static DataTable Consultar_Unidades_Asignadas(Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio)
            {
                try
                {
                    String Mi_SQL = "SELECT " + Ope_Psp_Limite_presupuestal.Tabla_Ope_Psp_Limite_presupuestal + ".*," +
                    Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " ||' '|| " +
                    Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS CLAVE_NOMBRE, " +
                    Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + 
                    " FROM " + Ope_Psp_Limite_presupuestal.Tabla_Ope_Psp_Limite_presupuestal + " JOIN " +
                    Cat_Dependencias.Tabla_Cat_Dependencias + " ON " +
                    Ope_Psp_Limite_presupuestal.Tabla_Ope_Psp_Limite_presupuestal + "." + 
                    Ope_Psp_Limite_presupuestal.Campo_Dependencia_ID + " = " +
                    Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " WHERE " +
                    Ope_Psp_Limite_presupuestal.Tabla_Ope_Psp_Limite_presupuestal + "." + Ope_Psp_Limite_presupuestal.Campo_Anio_presupuestal +
                    " = '" + Negocio.P_Anio_Presupuestal +"' ORDER BY " + Cat_Dependencias.Campo_Clave +" ASC" ;

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
                catch (Exception Ex)
                {
                    Ex.ToString();
                    return null;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Capitulos_Asignados_A_Unidad_Asignada
            ///DESCRIPCIÓN          : consulta para obtener los capitulos de una unidad responsable
            ///PARAMETROS           1 Negocio conexion con la capa de negocio 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 08/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Capitulos_Asignados_A_Unidad_Asignada(Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT " + Ope_Psp_Detalle_Lim_Presup.Campo_Capitulo_ID);
                    Mi_Sql.Append(" FROM " + Ope_Psp_Detalle_Lim_Presup.Tabla_Ope_Psp_Limite_presupuestal);
                    Mi_Sql.Append(" WHERE " + Ope_Psp_Detalle_Lim_Presup.Campo_Dependencia_ID + " = '" + Negocio.P_Dependencia_ID + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Detalle_Lim_Presup.Campo_Anio_presupuestal + " = '" + Negocio.P_Anio_Presupuestal + "'");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    Ex.ToString();
                    return null;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Unidades_Responsables_Sin_Asignar
            ///DESCRIPCIÓN          : consulta para obtener las unidades responsables que no estan asignadas
            ///PARAMETROS           1 Negocio conexion con la capa de negocio 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 09/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Unidades_Responsables_Sin_Asignar(Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                DataTable Dt_Unidades_Sin_Asignar = new DataTable();
                DataTable Dt_Unidades_Asignadas = new DataTable();
                Int32 Contador_Fila;
                try
                {
                    //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                    Mi_Sql.Append("SELECT " + Cat_Dependencias.Campo_Clave + " ||' '|| " + Cat_Dependencias.Campo_Nombre + " AS NOMBRE, ");
                    Mi_Sql.Append(Cat_Dependencias.Campo_Dependencia_ID);
                    Mi_Sql.Append(" FROM " + Cat_Dependencias.Tabla_Cat_Dependencias);
                    Mi_Sql.Append(" WHERE " + Cat_Dependencias.Campo_Estatus + " = 'ACTIVO'");
                    Mi_Sql.Append(" ORDER BY " + Cat_Dependencias.Campo_Clave + " ASC");
                    
                    Dt_Unidades_Sin_Asignar = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                    if (Negocio.P_Accion.Equals("Sin_Asignar")) {
                        //OBTENEMOS LAS UNIDADES RESPONSABLES QUE YA ESTAN ASIGNADAS A UN AÑO PRESUPUESTAL
                        Mi_Sql = new StringBuilder();
                        Mi_Sql.Append("SELECT " + Ope_Psp_Limite_presupuestal.Campo_Dependencia_ID);
                        Mi_Sql.Append(" FROM " + Ope_Psp_Limite_presupuestal.Tabla_Ope_Psp_Limite_presupuestal);
                        Mi_Sql.Append(" WHERE " + Ope_Psp_Limite_presupuestal.Campo_Anio_presupuestal + " = '" + Negocio.P_Anio_Presupuestal + "'");

                        Dt_Unidades_Asignadas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                        //RECORREMOS EL DATATABLE DE LAS UNIDADES SIN ASIGNAR PARA ELIMINAR LOS REGISTROS DE LAS UNIDADES 
                        // QUE YA ESTAN ASIGNADAS
                        if (Dt_Unidades_Asignadas != null)
                        {
                            if (Dt_Unidades_Asignadas.Rows.Count > 0)
                            {
                                if (Dt_Unidades_Sin_Asignar != null)
                                {
                                    if (Dt_Unidades_Sin_Asignar.Rows.Count > 0)
                                    {
                                        foreach (DataRow Dr_Unidades_Asignadas in Dt_Unidades_Asignadas.Rows)
                                        {
                                            Contador_Fila = -1;
                                            foreach (DataRow Dr_Unidades_Sin_Asignar in Dt_Unidades_Sin_Asignar.Rows)
                                            {
                                                Contador_Fila++;
                                                if (Dr_Unidades_Asignadas["DEPENDENCIA_ID"].ToString().Equals(Dr_Unidades_Sin_Asignar["DEPENDENCIA_ID"].ToString()))
                                                {
                                                    Dt_Unidades_Sin_Asignar.Rows.RemoveAt(Contador_Fila);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return Dt_Unidades_Sin_Asignar;
                }
                catch (Exception Ex)
                {
                    Ex.ToString();
                    return null;
                }
            }

            ///*******************************************************************************
            // NOMBRE DE LA FUNCIÓN: Guardar_Limites
            // DESCRIPCIÓN:
            // RETORNA: 
            // CREO: Gustavo Angeles Cruz
            // FECHA_CREO: 30/Agosto/2010 
            // MODIFICO:
            // FECHA_MODIFICO
            // CAUSA_MODIFICACIÓN   
            // *******************************************************************************/
            public static String Guardar_Limites(Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio)
            {
                String Mensaje = "EXITO";
                String Fecha_Creo = DateTime.Now.ToString("dd/MM/yy").ToUpper();
                //String Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                String Mi_SQL = "";
                //INSERTAR LA REQUISICION   
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                try
                {
                    Mi_SQL = "INSERT INTO " + Ope_Psp_Limite_presupuestal.Tabla_Ope_Psp_Limite_presupuestal + " (" +
                        Ope_Psp_Limite_presupuestal.Campo_Dependencia_ID + "," +
                        Ope_Psp_Limite_presupuestal.Campo_Limite_Presupuestal + "," +
                        Ope_Psp_Limite_presupuestal.Campo_Anio_presupuestal + "," +
                        Ope_Psp_Limite_presupuestal.Campo_Fte_Financiamiento_ID + "," +
                        Ope_Psp_Limite_presupuestal.Campo_Proyecto_Programa_ID + "," +
                        Ope_Psp_Limite_presupuestal.Campo_Usuario_Creo + ") VALUES (" +
                        "'" + Negocio.P_Dependencia_ID + "'," +
                        Negocio.P_Limite_Presupuestal + "," +
                        Negocio.P_Anio_Presupuestal + ",'" +
                        Negocio.P_Fuente_Financiamiento_ID + "','" +
                        Negocio.P_Programa_ID + "'," +
                        "'" + Cls_Sessiones.Nombre_Empleado + "')";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    foreach (DataRow Renglon in Negocio.P_Dt_Capitulos.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Psp_Detalle_Lim_Presup.Tabla_Ope_Psp_Limite_presupuestal + " (" +
                            Ope_Psp_Detalle_Lim_Presup.Campo_Dependencia_ID + "," +
                            Ope_Psp_Detalle_Lim_Presup.Campo_Capitulo_ID + "," +
                            Ope_Psp_Limite_presupuestal.Campo_Anio_presupuestal + ") VALUES (" +
                            "'" + Negocio.P_Dependencia_ID + "'," +
                            "'" + Renglon["CAPITULO_ID"].ToString().Trim() + "'," +
                            Negocio.P_Anio_Presupuestal + ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                    Trans.Commit();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    Trans.Rollback();
                    Mensaje = "No se pudo guardar requisición, consulte a su administrador";
                }
                finally
                {
                    Cn.Close();
                }        
                return Mensaje;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Actualizar_Limites
            ///DESCRIPCIÓN          : consulta para actualizar los datos de una unidad responsable
            ///PARAMETROS           1 Negocio conexion con la capa de negocio 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 08/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static Boolean Actualizar_Limites(Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio)
            {
                Boolean Operacion_Exitosa = false;
                StringBuilder Mi_Sql = new StringBuilder();
                  
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                try
                {
                    //MODIFICAMOS LOS DATOS DE LA UNIDAD RESPONSABLE
                    Mi_Sql.Append("UPDATE " + Ope_Psp_Limite_presupuestal.Tabla_Ope_Psp_Limite_presupuestal);
                    Mi_Sql.Append(" SET " + Ope_Psp_Limite_presupuestal.Campo_Limite_Presupuestal + " = '" + Negocio.P_Limite_Presupuestal + "' ,");
                    Mi_Sql.Append(Ope_Psp_Limite_presupuestal.Campo_Proyecto_Programa_ID + " = '" + Negocio.P_Programa_ID + "' ,");
                    Mi_Sql.Append(Ope_Psp_Limite_presupuestal.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.No_Empleado + "' ,");
                    Mi_Sql.Append(Ope_Psp_Limite_presupuestal.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_Sql.Append(" WHERE " + Ope_Psp_Limite_presupuestal.Campo_Anio_presupuestal + " = '" + Negocio.P_Anio_Presupuestal + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Limite_presupuestal.Campo_Dependencia_ID + " = '" + Negocio.P_Dependencia_ID + "'");
                    Cmd.CommandText = Mi_Sql.ToString();
                    Cmd.ExecuteNonQuery();

                    Mi_Sql = new StringBuilder();
                    //ELIMINAMOS LOS CAPITULOS ASIGNADOS ANTERIORMETE A ESTA UNIDAD RESPONSABLE
                    Mi_Sql.Append(" DELETE " + Ope_Psp_Detalle_Lim_Presup.Tabla_Ope_Psp_Limite_presupuestal);
                    Mi_Sql.Append(" WHERE " + Ope_Psp_Detalle_Lim_Presup.Campo_Anio_presupuestal + " = '" + Negocio.P_Anio_Presupuestal + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Detalle_Lim_Presup.Campo_Dependencia_ID + " = '" + Negocio.P_Dependencia_ID + "'");
                    Cmd.CommandText = Mi_Sql.ToString();
                    Cmd.ExecuteNonQuery();

                    //INSETAMOS LOS NUEVOS CAPITULOS ASIGNADOS A LA UNIDAD RESPONSABLE
                    foreach (DataRow Renglon in Negocio.P_Dt_Capitulos.Rows)
                    {
                        Mi_Sql = new StringBuilder();
                        Mi_Sql.Append("INSERT INTO " + Ope_Psp_Detalle_Lim_Presup.Tabla_Ope_Psp_Limite_presupuestal + " (");
                        Mi_Sql.Append(Ope_Psp_Detalle_Lim_Presup.Campo_Dependencia_ID + ", ");
                        Mi_Sql.Append(Ope_Psp_Detalle_Lim_Presup.Campo_Capitulo_ID + ", ");
                        Mi_Sql.Append(Ope_Psp_Limite_presupuestal.Campo_Anio_presupuestal + ") VALUES (");
                        Mi_Sql.Append("'" + Negocio.P_Dependencia_ID + "', ");
                        Mi_Sql.Append("'" + Renglon["CAPITULO_ID"].ToString().Trim() + "',");
                        Mi_Sql.Append(Negocio.P_Anio_Presupuestal + ")");

                        Cmd.CommandText = Mi_Sql.ToString();
                        Cmd.ExecuteNonQuery();
                    }
                    Trans.Commit();
                    Operacion_Exitosa = true;
                }
                catch(Exception ex){
                    Operacion_Exitosa = false;
                    Trans.Rollback();
                    throw new Exception("Error al tratar de actualizar los datos de la unidad responsable Error[" + ex.Message + "]");
                }
                finally
                {
                    Cn.Close();
                }        
                return Operacion_Exitosa;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Eliminar_Limites
            ///DESCRIPCIÓN          : consulta para eliminar los datos de una unidad responsable
            ///PARAMETROS           1 Negocio conexion con la capa de negocio 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 09/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static Boolean Eliminar_Limites(Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio)
            {
                Boolean Operacion_Exitosa = false;
                StringBuilder Mi_Sql = new StringBuilder();

                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                try
                {
                    Mi_Sql = new StringBuilder();
                    //ELIMINAMOS LOS CAPITULOS ASIGNADOS ANTERIORMETE A ESTA UNIDAD RESPONSABLE
                    Mi_Sql.Append(" DELETE " + Ope_Psp_Detalle_Lim_Presup.Tabla_Ope_Psp_Limite_presupuestal);
                    Mi_Sql.Append(" WHERE " + Ope_Psp_Detalle_Lim_Presup.Campo_Anio_presupuestal + " = '" + Negocio.P_Anio_Presupuestal + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Detalle_Lim_Presup.Campo_Dependencia_ID + " = '" + Negocio.P_Dependencia_ID + "'");
                    Cmd.CommandText = Mi_Sql.ToString();
                    Cmd.ExecuteNonQuery();

                    Mi_Sql = new StringBuilder();
                    //ELIMINAMOS LOS DATOS DE LA UNIDAD RESPONSABLE
                    Mi_Sql.Append(" DELETE " + Ope_Psp_Limite_presupuestal.Tabla_Ope_Psp_Limite_presupuestal);
                    Mi_Sql.Append(" WHERE " + Ope_Psp_Limite_presupuestal.Campo_Anio_presupuestal + " = '" + Negocio.P_Anio_Presupuestal + "'");
                    Mi_Sql.Append(" AND " + Ope_Psp_Limite_presupuestal.Campo_Dependencia_ID + " = '" + Negocio.P_Dependencia_ID + "'");
                    Cmd.CommandText = Mi_Sql.ToString();
                    Cmd.ExecuteNonQuery();
                   
                    Trans.Commit();
                    Operacion_Exitosa = true;
                }
                catch (Exception ex)
                {
                    Operacion_Exitosa = false;
                    Trans.Rollback();
                    throw new Exception("Error al tratar de eliminar los datos de la unidad responsable Error[" + ex.Message + "]");
                }
                finally
                {
                    Cn.Close();
                }
                return Operacion_Exitosa;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Programas_Unidades_Responsables
            ///DESCRIPCIÓN          : consulta para obtener los programas de una unidad responsable
            ///PARAMETROS           1 Negocio conexion con la capa de negocio 
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 14/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Programa_Unidades_Responsables(Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                DataTable Dt_Programas = new DataTable();
                try
                {
                    //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                    Mi_Sql.Append("SELECT " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id + ", ");
                    Mi_Sql.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Clave + " || ' ' || ");
                    Mi_Sql.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Descripcion+ " AS NOMBRE");
                    Mi_Sql.Append(" FROM " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas);
                    Mi_Sql.Append(" INNER JOIN " + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia);
                    Mi_Sql.Append(" ON " +  Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + "." + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID);
                    Mi_Sql.Append(" = " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id);
                    Mi_Sql.Append(" WHERE " +  Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + "." + Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID);
                    Mi_Sql.Append(" = '" + Negocio.P_Dependencia_ID + "'");
                    Mi_Sql.Append(" AND " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Estatus + " = 'ACTIVO'");
                    Mi_Sql.Append(" ORDER BY " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas+ "." + Cat_Sap_Proyectos_Programas.Campo_Clave + " ASC");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0]; ;
                }
                catch (Exception Ex)
                {
                    Ex.ToString();
                    return null;
                }
            }
        #endregion
    }
}