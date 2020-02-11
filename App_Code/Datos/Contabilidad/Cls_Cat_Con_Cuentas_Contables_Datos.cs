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
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Cuentas_Contables.Negocio;

namespace Presidencia.Cuentas_Contables.Datos
{
    public class Cls_Cat_Con_Cuentas_Contables_Datos
    {
        #region (Métodos Operación)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Cuenta_Contable
            /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
            ///               2. Da de Alta la Cuenta Contable en la BD con los datos proporcionados 
            ///                  por elusuario
            /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Alta_Cuenta_Contable(Cls_Cat_Con_Cuentas_Contables_Negocio Datos)
            {
                String Mi_SQL;             //Variable de Consulta para la Alta de la Cuenta Contable
                Object Cuenta_Contable_ID; //Variable que contendrá el ID de la consulta

                try
                {
                    //Obtiene el ID del Nivel a dar de alta
                    Mi_SQL = "SELECT NVL(MAX (" + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + "),'00000') ";
                    Mi_SQL = Mi_SQL + "FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables;
                    Cuenta_Contable_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    if (Convert.IsDBNull(Cuenta_Contable_ID))
                    {
                        Datos.P_Cuenta_Contable_ID = "00001";
                    }
                    else
                    {
                        Datos.P_Cuenta_Contable_ID = String.Format("{0:00000}", Convert.ToInt32(Cuenta_Contable_ID) + 1);
                    }
                    //Da de Alta los datos de la Cuenta Contable con los datos proporcionados por el usuario
                    Mi_SQL = "INSERT INTO " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + " (";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Nivel_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Tipo_Balance_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Tipo_Resultado_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Descripcion + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Cuenta + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Afectable + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Comentarios + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Tipo_Cuenta + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Fecha_Creo + ") ";
                    Mi_SQL = Mi_SQL + "VALUES ('" + Datos.P_Cuenta_Contable_ID + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Nivel_ID + "', ";
                    if (!String.IsNullOrEmpty(Datos.P_Tipo_Balance_ID))
                    {
                        Mi_SQL = Mi_SQL + "'" + Datos.P_Tipo_Balance_ID + "', ";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + "NULL, ";
                    }
                    if (!String.IsNullOrEmpty(Datos.P_Tipo_Resultado_ID))
                    {
                        Mi_SQL = Mi_SQL + "'" + Datos.P_Tipo_Resultado_ID + "', '";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + "NULL, '";
                    }
                    Mi_SQL = Mi_SQL + Datos.P_Descripcion + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Cuenta + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Afectable + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Comentarios + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Tipo_Cuenta + "', SYSDATE)";

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Cuenta_Contable
            /// DESCRIPCION : Modifica los datos de la Cuenta Contable con lo que fueron introducidos 
            ///              por el usuario
            /// PARAMETROS  :  Datos: Datos que son enviados de la capa de Negocios y que fueron 
            ///                       proporcionados por el usuario y van a sustituir a los datos que se
            ///                       encuentran en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Modificar_Cuenta_Contable(Cls_Cat_Con_Cuentas_Contables_Negocio Datos)
            {
                String Mi_SQL; //Variable de Consulta para la Modificación de la Cuenta Contable
                try
                {
                    //Consulta para la modificación de la Cuenta Contable con los datos proporcionados por el usuario
                    Mi_SQL = "UPDATE " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + " SET ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Nivel_ID + " = '" + Datos.P_Nivel_ID + "', ";
                    if (!String.IsNullOrEmpty(Datos.P_Tipo_Balance_ID))
                    {
                        Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Tipo_Balance_ID + " = '" + Datos.P_Tipo_Balance_ID + "', ";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Tipo_Balance_ID + " = NULL, ";
                    }
                    if (!String.IsNullOrEmpty(Datos.P_Tipo_Resultado_ID))
                    {
                        Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Tipo_Resultado_ID + " = '" + Datos.P_Tipo_Resultado_ID + "', ";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Tipo_Resultado_ID + " = NULL, ";
                    }
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Cuenta + " = '" + Datos.P_Cuenta + "', ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Afectable + " = '" + Datos.P_Afectable + "', ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Tipo_Cuenta + " = '" + Datos.P_Tipo_Cuenta + "', ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                    Mi_SQL = Mi_SQL + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
            /// NOMBRE DE LA FUNCION: Eliminar_Cuenta_Contable
            /// DESCRIPCION : Elimina la Cuenta Contable que fue seleccionada por el usuario de la BD
            /// PARAMETROS  : Datos: Obtiene que Cuenta Contable desea eliminar de la BD
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Eliminar_Cuenta_Contable(Cls_Cat_Con_Cuentas_Contables_Negocio Datos)
            {
                String Mi_SQL; //Variable de Consulta para la Eliminación de la Cuenta Contable
                try
                {
                    Mi_SQL = "DELETE FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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

        #region (Métodos Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Cuentas_Contables
            /// DESCRIPCION : Consulta las Cuentas Contables que estan dadas de alta en la BD
            ///               con todos sus datos
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 10-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Datos_Cuentas_Contables(Cls_Cat_Con_Cuentas_Contables_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta de las Cuentas Contables
                Boolean Concatenacion_WHERE = true;

                try
                {
                    Mi_SQL = "SELECT " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + ".*, ";
                    Mi_SQL += Cat_Con_Niveles.Tabla_Cat_Con_Niveles + "." + Cat_Con_Niveles.Campo_Descripcion + " AS Nivel";
                    Mi_SQL += " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + " LEFT JOIN " + Cat_Con_Niveles.Tabla_Cat_Con_Niveles;
                    Mi_SQL += " ON " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Nivel_ID + " = " + Cat_Con_Niveles.Tabla_Cat_Con_Niveles + "." + Cat_Con_Niveles.Campo_Nivel_ID;

                    if (!string.IsNullOrEmpty(Datos.P_Cuenta_Contable_ID))
                    {
                        Mi_SQL += " WHERE " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + "='" + Datos.P_Cuenta_Contable_ID + "'";
                        Concatenacion_WHERE = false;
                    }

                    if (!string.IsNullOrEmpty(Datos.P_Descripcion))
                    {
                        if (Concatenacion_WHERE == true)
                        {
                            Mi_SQL += " WHERE (UPPER(" + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                            Mi_SQL += " OR " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " LIKE '%" + Datos.P_Descripcion + "%')";
                        }
                        else
                        {
                            Mi_SQL += " AND (UPPER(" + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                            Mi_SQL += " OR " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " LIKE '%" + Datos.P_Descripcion + "%')";
                        }
                    }

                    /////////////////////////////////////////////////////////
                    //Codigo comentado por Salvador Rea
                    /////////////////////////////////////////////////////////
                    //Mi_SQL = "SELECT " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + ".*, ";
                    //Mi_SQL += Cat_Con_Niveles.Tabla_Cat_Con_Niveles + "." + Cat_Con_Niveles.Campo_Descripcion + " AS Nivel";
                    //Mi_SQL += " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + ", " + Cat_Con_Niveles.Tabla_Cat_Con_Niveles;
                    //Mi_SQL += " WHERE " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Nivel_ID + " = " + Cat_Con_Niveles.Tabla_Cat_Con_Niveles + "." + Cat_Con_Niveles.Campo_Nivel_ID;

                    //if (!string.IsNullOrEmpty(Datos.P_Cuenta_Contable_ID))
                    //{
                    //    Mi_SQL += " AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + "='" + Datos.P_Cuenta_Contable_ID + "'";
                    //}

                    //if (!string.IsNullOrEmpty(Datos.P_Descripcion))
                    //{
                    //    Mi_SQL += " AND (UPPER(" + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                    //    Mi_SQL += " OR " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " LIKE '%" + Datos.P_Descripcion + "%')";
                    //}
                    //Mi_SQL += " ORDER BY " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta;

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
            /// NOMBRE DE LA FUNCION: Consulta_Cuentas_Contables
            /// DESCRIPCION : Consulta los Tipos de Poliza de las cuentas contables que estan 
            ///              dados de alta en la BD 
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 21-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Cuentas_Contables(Cls_Cat_Con_Cuentas_Contables_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta de las cuentas contables

                try
                {
                    //Consulta las Cuentas Contables que estan dados de alta en la base de datos
                    Mi_SQL = "SELECT " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + ", " + Cat_Con_Cuentas_Contables.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables;
                    if (Datos.P_Cuenta_Contable_ID != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "'";
                    }
                    if (Datos.P_Descripcion != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Cuentas_Contables.Campo_Descripcion + " LIKE '%" + Datos.P_Descripcion + "%'";
                    }
                    if (Datos.P_Afectable != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Cuentas_Contables.Campo_Afectable + " = '" + Datos.P_Afectable + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Con_Cuentas_Contables.Campo_Descripcion;
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Existencia_Cuenta_Contable
            /// DESCRIPCION : Consulta 
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 23-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Existencia_Cuenta_Contable(Cls_Cat_Con_Cuentas_Contables_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta de las cuentas contables

                try
                {
                    //Consulta si existe la cuenta contable que se desea dar de alta
                    Mi_SQL = "SELECT " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + ", " + Cat_Con_Cuentas_Contables.Campo_Cuenta;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables;
                    if (!String.IsNullOrEmpty(Datos.P_Cuenta))
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Cuentas_Contables.Campo_Cuenta + " = '" + Datos.P_Cuenta + "'";
                    }
                    if (!String.IsNullOrEmpty(Datos.P_Cuenta_Contable_ID))
                    {
                        if (Mi_SQL.Contains("WHERE"))
                        {
                            Mi_SQL = Mi_SQL + " AND " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "'";
                        }
                        else
                        {
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "'";
                        }

                    }
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
    }
}