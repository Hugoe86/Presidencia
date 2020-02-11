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
using Presidencia.Tipo_Resultado.Negocios;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;


namespace Presidencia.Tipo_Resultado.Datos
{
    public class Cls_Cat_Con_Tipo_Resultado_Datos
    {
        #region (Métodos Operación)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Tipo_Resultado
            /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
            ///               2. Da de Alta el Tipo de Resultado en la BD con los datos proporcionados 
            ///                  por elusuario
            /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Alta_Tipo_Resultado(Cls_Cat_Con_Tipo_Resultado_Negocio Datos)
            {
                String Mi_SQL;            //Variable de Consulta para la Alta del Tipo de Resultado
                Object Tipo_Resultado_ID; //Variable que contendrá el ID de la consulta

                try
                {
                    //Obtiene el ID del Tipo de Resultado a dar de alta
                    Mi_SQL = "SELECT NVL(MAX (" + Cat_Con_Tipo_Resultado.Campo_Tipo_Resultado_ID + "),'00000') ";
                    Mi_SQL = Mi_SQL + "FROM " + Cat_Con_Tipo_Resultado.Tabla_Cat_Con_Tipo_Resultado;
                    Tipo_Resultado_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    if (Convert.IsDBNull(Tipo_Resultado_ID))
                    {
                        Datos.P_Tipo_Resultado_ID = "00001";
                    }
                    else
                    {
                        Datos.P_Tipo_Resultado_ID = String.Format("{0:00000}", Convert.ToInt32(Tipo_Resultado_ID) + 1);
                    }
                    //Da de Alta los datos del Tipo de Resultado con los datos proporcionados por el usuario
                    Mi_SQL = "INSERT INTO " + Cat_Con_Tipo_Resultado.Tabla_Cat_Con_Tipo_Resultado + " (";
                    Mi_SQL = Mi_SQL + Cat_Con_Tipo_Resultado.Campo_Tipo_Resultado_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Tipo_Resultado.Campo_Descripcion + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Tipo_Resultado.Campo_Comentarios + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Tipo_Resultado.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Tipo_Resultado.Campo_Fecha_Creo + ") ";
                    Mi_SQL = Mi_SQL + "VALUES ('" + Datos.P_Tipo_Resultado_ID + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Descripcion + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Comentarios + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Nombre_Usuario + "', SYSDATE)";

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
            /// NOMBRE DE LA FUNCION: Modificar_Tipo_Resultado
            /// DESCRIPCION : Modifica los datos del Tipo de Resultado con lo que fueron introducidos 
            ///               por el usuario
            /// PARAMETROS  : Datos: Datos que son enviados de la capa de Negocios y que fueron 
            ///                       proporcionados por el usuario y van a sustituir a los datos que se
            ///                       encuentran en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Modificar_Tipo_Resultado(Cls_Cat_Con_Tipo_Resultado_Negocio Datos)
            {
                String Mi_SQL; //Variable de Consulta para la Modificación del Tipo de Resultado
                try
                {
                    //Consulta para la modificación del tipo de Resultado con los datos proporcionados por el usuario
                    Mi_SQL = "UPDATE " + Cat_Con_Tipo_Resultado.Tabla_Cat_Con_Tipo_Resultado + " SET ";
                    Mi_SQL = Mi_SQL + Cat_Con_Tipo_Resultado.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                    Mi_SQL = Mi_SQL + Cat_Con_Tipo_Resultado.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                    Mi_SQL = Mi_SQL + Cat_Con_Tipo_Resultado.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                    Mi_SQL = Mi_SQL + Cat_Con_Tipo_Resultado.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                    Mi_SQL = Mi_SQL + Cat_Con_Tipo_Resultado.Campo_Tipo_Resultado_ID + " = '" + Datos.P_Tipo_Resultado_ID + "'";
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
            /// NOMBRE DE LA FUNCION: Eliminar_Tipo_Resultado
            /// DESCRIPCION : Elimina el Tipo de Resultado que fue seleccionada por el usuario de la BD
            /// PARAMETROS  : Datos: Obtiene que Tipo de Resultado desea eliminar de la BD
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Eliminar_Tipo_Resultado(Cls_Cat_Con_Tipo_Resultado_Negocio Datos)
            {
                String Mi_SQL; //Variable de Consulta para la Eliminación del Tipo de Resultado
                try
                {
                    Mi_SQL = "DELETE FROM " + Cat_Con_Tipo_Resultado.Tabla_Cat_Con_Tipo_Resultado;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Resultado.Campo_Tipo_Resultado_ID + " = '" + Datos.P_Tipo_Resultado_ID + "'";
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
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Tipo_Resultado
            /// DESCRIPCION : Consulta los Tipos de Resultado que estan dadas de alta en la BD
            ///               con todos sus datos
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 08-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Datos_Tipo_Resultado(Cls_Cat_Con_Tipo_Resultado_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los Tipos de Resultados

            try
            {
                Mi_SQL = "SELECT * FROM " + Cat_Con_Tipo_Resultado.Tabla_Cat_Con_Tipo_Resultado;

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Resultado_ID))
                {
                    if (Mi_SQL.Contains("WHERE"))
                        Mi_SQL += " AND " + Cat_Con_Tipo_Resultado.Campo_Tipo_Resultado_ID + "='" + Datos.P_Tipo_Resultado_ID + "'";
                    else
                        Mi_SQL += " WHERE " + Cat_Con_Tipo_Resultado.Campo_Tipo_Resultado_ID + "='" + Datos.P_Tipo_Resultado_ID + "'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Descripcion))
                {
                    if (Mi_SQL.Contains("WHERE"))
                        Mi_SQL = Mi_SQL + " AND UPPER(" + Cat_Con_Tipo_Resultado.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                    else
                        Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Con_Tipo_Resultado.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Con_Tipo_Resultado.Campo_Descripcion;

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
            /// NOMBRE DE LA FUNCION: Consulta_Tipos_Poliza
            /// DESCRIPCION : Consulta los Tipos de Poliza de las cuentas contables que estan 
            ///              dados de alta en la BD 
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 21-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Tipos_Resultado(Cls_Cat_Con_Tipo_Resultado_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta de los Tipos de Resultado

                try
                {
                    //Consulta los Tipos de Resultado que estan dados de alta en la base de datos
                    Mi_SQL = "SELECT " + Cat_Con_Tipo_Resultado.Campo_Tipo_Resultado_ID + ", " + Cat_Con_Tipo_Resultado.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Tipo_Resultado.Tabla_Cat_Con_Tipo_Resultado;
                    if (Datos.P_Tipo_Resultado_ID != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Resultado.Campo_Tipo_Resultado_ID + " = '" + Datos.P_Tipo_Resultado_ID + "'";
                    }
                    if (Datos.P_Descripcion != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Resultado.Campo_Descripcion + " LIKE '%" + Datos.P_Descripcion + "%'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Con_Tipo_Resultado.Campo_Descripcion;
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

