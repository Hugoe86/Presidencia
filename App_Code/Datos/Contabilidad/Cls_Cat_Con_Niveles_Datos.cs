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
using Presidencia.Niveles.Negocio;

namespace Presidencia.Niveles.Datos
{
    public class Cls_Cat_Con_Niveles_Datos
    {
        #region (Métodos Operación)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Nivel
            /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
            ///               2. Da de Alta el Nivel en la BD con los datos proporcionados 
            ///                  por elusuario
            /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Alta_Nivel(Cls_Cat_Con_Niveles_Negocio Datos)
            {
                String Mi_SQL;   //Variable de Consulta para la Alta del Nivel de Poliza
                Object Nivel_ID; //Variable que contendrá el ID de la consulta

                try
                {
                    //Obtiene el ID del Nivel a dar de alta
                    Mi_SQL = "SELECT NVL(MAX (" + Cat_Con_Niveles.Campo_Nivel_ID + "),'00000') ";
                    Mi_SQL = Mi_SQL + "FROM " + Cat_Con_Niveles.Tabla_Cat_Con_Niveles;
                    Nivel_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    if (Convert.IsDBNull(Nivel_ID))
                    {
                        Datos.P_Nivel_ID = "00001";
                    }
                    else
                    {
                        Datos.P_Nivel_ID = String.Format("{0:00000}", Convert.ToInt32(Nivel_ID) + 1);
                    }
                    //Da de Alta los datos del Nivel de la Póliza con los datos proporcionados por el usuario
                    Mi_SQL = "INSERT INTO " + Cat_Con_Niveles.Tabla_Cat_Con_Niveles + " (";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Nivel_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Descripcion + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Inicio_Nivel + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Final_Nivel + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Comentarios + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Fecha_Creo + ") ";
                    Mi_SQL = Mi_SQL + "VALUES ('" + Datos.P_Nivel_ID + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Descripcion + "', ";
                    Mi_SQL = Mi_SQL + Datos.P_Inicio_Nivel + ", ";
                    Mi_SQL = Mi_SQL + Datos.P_Final_Nivel + ", '";
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
            /// NOMBRE DE LA FUNCION: Modificar_Nivel
            /// DESCRIPCION : Modifica los datos del Nivel con lo que fueron introducidos 
            ///              por el usuario
            /// PARAMETROS  :  Datos: Datos que son enviados de la capa de Negocios y que fueron 
            ///                       proporcionados por el usuario y van a sustituir a los datos que se
            ///                       encuentran en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Modificar_Nivel(Cls_Cat_Con_Niveles_Negocio Datos)
            {
                String Mi_SQL; //Variable de Consulta para la Modificación del Nivel
                try
                {
                    //Consulta para la modificación del Nivel con los datos proporcionados por el usuario
                    Mi_SQL = "UPDATE " + Cat_Con_Niveles.Tabla_Cat_Con_Niveles + " SET ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Inicio_Nivel + " = " + Datos.P_Inicio_Nivel + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Final_Nivel + " = " + Datos.P_Final_Nivel + ", ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                    Mi_SQL = Mi_SQL + Cat_Con_Niveles.Campo_Nivel_ID + " = '" + Datos.P_Nivel_ID + "'";
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
            /// NOMBRE DE LA FUNCION: Eliminar_Nivel
            /// DESCRIPCION : Elimina el Nivel que fue seleccionada por el usuario de la BD
            /// PARAMETROS  : Datos: Obtiene que Nivel desea eliminar de la BD
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Eliminar_Nivel(Cls_Cat_Con_Niveles_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la Eliminación del Nivel
            try
            {
                Mi_SQL = "DELETE FROM " + Cat_Con_Niveles.Tabla_Cat_Con_Niveles;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Niveles.Campo_Nivel_ID + " = '" + Datos.P_Nivel_ID + "'";
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
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Nivel
            /// DESCRIPCION : Consulta los Niveles que estan dadas de alta en la BD
            ///               con todos sus datos
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Datos_Nivel(Cls_Cat_Con_Niveles_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta de los Niveles

                try
                {
                    Mi_SQL = "SELECT * FROM " + Cat_Con_Niveles.Tabla_Cat_Con_Niveles;

                    if (!string.IsNullOrEmpty(Datos.P_Nivel_ID))
                    {
                        if (Mi_SQL.Contains("WHERE"))
                            Mi_SQL += " AND " + Cat_Con_Niveles.Campo_Nivel_ID + "='" + Datos.P_Nivel_ID + "'";
                        else
                            Mi_SQL += " WHERE " + Cat_Con_Niveles.Campo_Nivel_ID + "='" + Datos.P_Nivel_ID + "'";
                    }

                    if (!string.IsNullOrEmpty(Datos.P_Descripcion))
                    {
                        if (Mi_SQL.Contains("WHERE"))
                            Mi_SQL = Mi_SQL + " AND UPPER(" + Cat_Con_Niveles.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                        else
                            Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Con_Niveles.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Con_Niveles.Campo_Inicio_Nivel;

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
            /// NOMBRE DE LA FUNCION: Consulta_Niveles
            /// DESCRIPCION : Consulta los Niveles de las cuentas contables que estan dados de
            ///               alta en la BD 
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 21-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Niveles(Cls_Cat_Con_Niveles_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta de los Niveles

                try
                {
                    //Consulta los Niveles que estan dados de alta en la base de datos
                    Mi_SQL = "SELECT " + Cat_Con_Niveles.Campo_Nivel_ID + ", " + Cat_Con_Niveles.Campo_Descripcion;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Niveles.Tabla_Cat_Con_Niveles;
                    if (Datos.P_Nivel_ID != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Niveles.Campo_Nivel_ID + " = '" + Datos.P_Nivel_ID + "'";
                    }
                    if (Datos.P_Descripcion != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Niveles.Campo_Descripcion + " LIKE '%" + Datos.P_Descripcion + "%'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Con_Niveles.Campo_Descripcion;
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