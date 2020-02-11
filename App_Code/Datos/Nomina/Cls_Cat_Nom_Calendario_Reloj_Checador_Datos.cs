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
using System.Text;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Calendario_Reloj_Checador.Negocio;

namespace Presidencia.Calendario_Reloj_Checador.Datos
{
    public class Cls_Cat_Nom_Calendario_Reloj_Checador_Datos
    {
        public Cls_Cat_Nom_Calendario_Reloj_Checador_Datos()
        {
        }
        #region (Metodos)
            #region(Metodos Operacion)
                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Alta_Modificacion_Calendario_Reloj_Checador
                /// DESCRIPCION : 1. Consulta que en el periodo nominal proporcionado por el usuario
                ///                  no se encuentre el registro en la base de datos
                ///               2. Si se encuentra algun registro entonces se actualiza los datos
                ///                  con la información proporcionada por el usuario
                ///               3. Si no se encontraron datos entonces da de alta el registro en
                ///                  la base de datos
                /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
                /// CREO        : Yazmin A Delgado Gómez
                /// FECHA_CREO  : 16-Agosto-2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                public static void Alta_Modificacion_Calendario_Reloj_Checador(Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio Datos)
                {
                    StringBuilder Mi_SQL = new StringBuilder(); //Contiene la consulta a realizar a la base de datos
                    DataTable Dt_Calendario;                    //Obtiene los datos del calendario si es que este ya fue dado de alta
                    try
                    {
                        foreach (DataRow Registro in Datos.P_Dt_Calendario_Reloj.Rows)
                        {
                            Mi_SQL.Length = 0;
                            //Consulta los datos generales del registro que se pretende modificar
                            Mi_SQL.Append("SELECT * FROM " + Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj);
                            Mi_SQL.Append(" WHERE " + Cat_Nom_Calendario_Reloj.Campo_Nomina_ID + " = '" + Registro["Nomina_ID"].ToString() + "'");
                            Mi_SQL.Append(" AND " + Cat_Nom_Calendario_Reloj.Campo_No_Nomina + " = " + Registro["No_Nomina"].ToString());
                            Dt_Calendario = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                            //Si se tienen ya registro del periodo nominal que se esta consultando entonces modifica los datos del registro
                            //con los datos que fueron proporcionados por el usuario
                            if (Dt_Calendario.Rows.Count > 0)
                            {
                                Mi_SQL.Length = 0;
                                //Actualiza las fechas del reloj checador de acuerdo al periodo nominal
                                Mi_SQL.Append("UPDATE " + Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj + " SET ");
                                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Registro["Fecha_Inicio"].ToString())) + "','DD/MM/YYYY'), ");
                                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Registro["Fecha_Fin"].ToString())) + "','DD/MM/YYYY'), ");
                                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Estatus + " = '" + Registro["Estatus"].ToString() + "', ");
                                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Fecha_Modifico + " = SYSDATE");
                                Mi_SQL.Append(" WHERE " + Cat_Nom_Calendario_Reloj.Campo_Nomina_ID + " = '" + Registro["Nomina_ID"].ToString() + "'");
                                Mi_SQL.Append(" AND " + Cat_Nom_Calendario_Reloj.Campo_No_Nomina + " = " + Registro["No_Nomina"].ToString());
                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                            }
                            //Si no exiten datos entonces da de alta el registro con los datos proporcionados por el usuario
                            else
                            {
                                Object Consecutivo; //Obtiene el Consecutivo con el cual va hacer guardado el nuevo registro en la base de datos

                                Mi_SQL.Length = 0;
                                //Consulta el último No. de Registro que fue agregado a la base de datos
                                Mi_SQL.Append("SELECT NVL(MAX(" + Cat_Nom_Calendario_Reloj.Campo_Consecutivo + "), 0)");
                                Mi_SQL.Append(" FROM " + Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj);
                                Consecutivo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                                if (Convert.IsDBNull(Consecutivo))
                                {
                                    Consecutivo = 1;
                                }
                                else
                                {
                                    Consecutivo = Convert.ToInt32(Consecutivo) + 1;
                                }
                                Mi_SQL.Length = 0;
                                //Da de alta las fechas del periodo nominal a considerar para el reloj checador
                                Mi_SQL.Append("INSERT INTO " + Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj + " (");
                                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Consecutivo + ", " + Cat_Nom_Calendario_Reloj.Campo_Nomina_ID + ", ");
                                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_No_Nomina + ", " + Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio + ", ");
                                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin + ", " + Cat_Nom_Calendario_Reloj.Campo_Estatus + ", ");
                                Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Campo_Usuario_Creo + ", " + Cat_Nom_Calendario_Reloj.Campo_Fecha_Modifico + ")");
                                Mi_SQL.Append(" VALUES (" + Consecutivo + ", '" + Registro["Nomina_ID"].ToString() + "', " + Registro["No_Nomina"].ToString() + ",");
                                Mi_SQL.Append(" TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Registro["Fecha_Inicio"].ToString())) + "','DD/MM/YYYY'),");
                                Mi_SQL.Append(" TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Registro["Fecha_Fin"].ToString())) + "','DD/MM/YYYY'),");
                                Mi_SQL.Append("'" + Registro["Estatus"].ToString() + "', '" + Datos.P_Nombre_Usuario + "', SYSDATE)");
                                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                            }
                        }
                    }
                    catch (OracleException Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }
                    catch (DBConcurrencyException Ex)
                    {
                        throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }
                    finally
                    {
                    }
                }
            #endregion
            #region (Consulta)
                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Consulta_Datos_Calendario_Nominal
                /// DESCRIPCION : Consulta los datos generales del calendario nominal de cuerdo
                ///               al año que fue proporcionado por el usuarop
                /// PARAMETROS  : Datos: Variable utilizado para el paso de parametros de la capa
                ///                      de Negocios
                /// CREO        : Yazmin A Delgado Gómez
                /// FECHA_CREO  : 01-Septiembre-2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                public static DataTable Consulta_Datos_Calendario_Nominal(Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio Datos)
                {
                    StringBuilder Mi_SQL = new StringBuilder(); //Contiene la cadena de Consulta de los datos hacia la capa de datos

                    try
                    {
                        //Consulta los datos generales del calendario nominal en base al año que fue proporcionado por el usuarios
                        Mi_SQL.Append("SELECT " + Cat_Nom_Calendario_Nominas.Campo_Anio + ", ");
                        Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + ", ");
                        Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Campo_Fecha_Inicio + ", ");
                        Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin);
                        Mi_SQL.Append(" FROM " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas);
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Calendario_Nominas.Campo_Anio + " = " + Datos.P_Anio);
                        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    }
                    catch (OracleException Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }

                    catch (DBConcurrencyException Ex)
                    {
                        throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }
                    finally
                    {
                    }
                }
                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Consulta_Calenadario_Reloj_Checador
                /// DESCRIPCION : Consulta los periodos nominales de la nómina y reloj checador que
                ///               pertenecen a la nomina ID qie se pretende consultar
                /// PARAMETROS  : Datos: Variable utilizada para el paso de parametros de la capa
                ///                      de negocios
                /// CREO        : Yazmin A Delgado Gómez
                /// FECHA_CREO  : 01-Septiembre-2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                public static DataTable Consulta_Calenadario_Reloj_Checador(Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio Datos)
                {
                    StringBuilder Mi_SQL = new StringBuilder();  //Contiene la cadena de consulta de los datos hacia la base de datos.                                                          
                    try
                    {
                            Mi_SQL.Length = 0;
                            //Consulta los datos generales del calendario nominal y del reloj checador
                            Mi_SQL.Append("SELECT " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID + ", ");
                            Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + ", ");
                            Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Estatus + ", ");
                            Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + " AS Fecha_Inicio_Nomina, ");
                            Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + " AS Fecha_Fin_Nomina, ");
                            Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj + "." + Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio + " AS Fecha_Inicio_Reloj, ");
                            Mi_SQL.Append(Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj + "." + Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin + " AS Fecha_Fin_Reloj");
                            Mi_SQL.Append(" FROM " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);
                            Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj);
                            Mi_SQL.Append(" ON " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);
                            Mi_SQL.Append(" = " + Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj + "." + Cat_Nom_Calendario_Reloj.Campo_Nomina_ID);
                            Mi_SQL.Append(" AND " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);
                            Mi_SQL.Append(" = " + Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj + "." + Cat_Nom_Calendario_Reloj.Campo_No_Nomina);
                            Mi_SQL.Append(" INNER JOIN " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas);
                            Mi_SQL.Append(" ON " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + " = '" + Datos.P_Nomina_ID + "'");
                            Mi_SQL.Append(" AND " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);
                            Mi_SQL.Append(" = " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);
                            Mi_SQL.Append(" ORDER BY " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);
                            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    }
                    catch (OracleException Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }

                    catch (DBConcurrencyException Ex)
                    {
                        throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }
                    finally
                    {
                    }
                }
                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Consulta_Fechas_Calendario_Reloj_Checador
                /// DESCRIPCION : Consulta las fecha de inicio y fin del periodo nominal que fue
                ///               seleccionado por el usuario
                /// PARAMETROS  : Datos: Variable utilizada para el paso de parametros de la capa
                ///                      de negocios
                /// CREO        : Yazmin A Delgado Gómez
                /// FECHA_CREO  : 05-Octubre-2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                public static DataTable Consulta_Fechas_Calendario_Reloj_Checador(Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio Datos)
                {
                    StringBuilder Mi_SQL= new StringBuilder(); //Variable que contendra la consulta a realizar para la obtención de los datos
                    try
                    {
                        Mi_SQL.Append("SELECT " + Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio + ", " + Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin);
                        Mi_SQL.Append(" FROM " + Cat_Nom_Calendario_Reloj.Tabla_Cat_Nom_Calendario_Reloj);
                        Mi_SQL.Append(" WHERE " + Cat_Nom_Calendario_Reloj.Campo_Nomina_ID + " = '" + Datos.P_Nomina_ID + "'");
                        Mi_SQL.Append(" AND " + Cat_Nom_Calendario_Reloj.Campo_No_Nomina + " = " + Datos.P_No_Nomina);
                        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    }
                    catch (OracleException Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }

                    catch (DBConcurrencyException Ex)
                    {
                        throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error: " + Ex.Message);
                    }
                    finally
                    {
                    }
                }
            #endregion
        #endregion
    }
}