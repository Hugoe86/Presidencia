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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Reloj_Checador.Negocios;

namespace Presidencia.Reloj_Checador.Datos
{
    public class Cls_Cat_Nom_Reloj_Checador_Datos
    {
        #region(Alta-Modificar-Eliminar)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Reloj_Checador
            /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
            ///               2. Da de Alta el Reloj Checador en la BD con los datos 
            ///               proporcionados por el usuario
            /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 15-Julio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Alta_Reloj_Checador(Cls_Cat_Nom_Reloj_Checador_Negocio Datos)
            {
                String Mi_SQL;            //Obtiene la cadena de inserción hacía la base de datos
                Object Reloj_Checador_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

                try
                {   
                    //Obtiene el ID del último registro que fue dado de alta en la base de datos
                    Mi_SQL = "SELECT NVL(MAX(" + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID + "),'00000') ";
                    Mi_SQL = Mi_SQL + "FROM " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador;
                    Reloj_Checador_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    if (Convert.IsDBNull(Reloj_Checador_ID))
                    {
                        Datos.P_Reloj_Checador_ID = "00001";
                    }
                    else
                    {
                        Datos.P_Reloj_Checador_ID = String.Format("{0:00000}", Convert.ToInt32(Reloj_Checador_ID) + 1);
                    }
                    //Agrega el registro a la tabla con los datos proporcionados por el usuario
                    Mi_SQL = "INSERT INTO " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "(";
                    Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Clave + ", ";
                    Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Ubicacion + ", ";
                    Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Comentarios + ", ";
                    Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Fecha_Creo + ") VALUES ('";
                    Mi_SQL = Mi_SQL + Datos.P_Reloj_Checador_ID + "', '" + Datos.P_Clave + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Ubicacion + "', ";
                    if (!String.IsNullOrEmpty(Datos.P_Comentarios))
                    {
                        Mi_SQL = Mi_SQL + "'" + Datos.P_Comentarios + "', '";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + Datos.P_Comentarios + "NULL, '";
                    }
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
                finally
                {
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Reloj_Checador
            /// DESCRIPCION : Modifica los datos del Reloj con lo que fueron introducidos por el usuario
            /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 15-Julio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Modificar_Reloj_Checador(Cls_Cat_Nom_Reloj_Checador_Negocio Datos)
            {
                String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos
                try
                {
                    //Modifica los datos del registro con los datos proporcionados por el usuario
                    Mi_SQL = "UPDATE " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + " SET " +
                            Cat_Nom_Reloj_Checador.Campo_Clave + " = '" + Datos.P_Clave + "', " +
                            Cat_Nom_Reloj_Checador.Campo_Ubicacion + " = '" + Datos.P_Ubicacion + "', ";
                    if (!String.IsNullOrEmpty(Datos.P_Comentarios))
                    {
                        Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Comentarios + " = NULL, ";
                    }
                    Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                    Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Fecha_Modifico + " =  SYSDATE WHERE ";
                    Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID + " = '" + Datos.P_Reloj_Checador_ID + "'";
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
            /// NOMBRE DE LA FUNCION: Elimina_Reloj_Checador
            /// DESCRIPCION : Elimina el Reloj que fue seleccionada por el usuario de la BD
            /// PARAMETROS  : Datos: Obtiene que reloj checador desea eliminar de la BD
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 15-Julio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Elimina_Reloj_Checador(Cls_Cat_Nom_Reloj_Checador_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del reloj checador
            try
            {
                Mi_SQL = "DELETE FROM " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID + " = '" + Datos.P_Reloj_Checador_ID + "'";
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

        #region(Consultas)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Reloj_Checador
            /// DESCRIPCION : Consulta todos los datos de los reloj que estan dadas de alta en la BD
            ///               con todos sus datos
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 15-Julio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Datos_Reloj_Checador(Cls_Cat_Nom_Reloj_Checador_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta de las areas

                try
                {
                    //Consulta todos los datos del reloj que se fue seleccionada por el usuario
                    Mi_SQL = "SELECT * FROM " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador;
                    if (Datos.P_Reloj_Checador_ID != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID + " = '" + Datos.P_Reloj_Checador_ID + "'";
                    }
                    if (Datos.P_Clave != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Nom_Reloj_Checador.Campo_Clave + ") LIKE UPPER('%" + Datos.P_Clave + "%')";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Reloj_Checador.Campo_Clave;
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
            /// NOMBRE DE LA FUNCION: Consulta_Reloj_Checador
            /// DESCRIPCION : Consulta los reloj que estan dadas de alta en la BD
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 15-Julio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    : 
            /// CAUSA_MODIFICACION: 
            ///*******************************************************************************
            public static DataTable Consulta_Reloj_Checador(Cls_Cat_Nom_Reloj_Checador_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta de los reloj

                try
                {
                    Mi_SQL = "SELECT " + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID + ", " + Cat_Nom_Reloj_Checador.Campo_Clave + ", (";
                    Mi_SQL = Mi_SQL + Cat_Nom_Reloj_Checador.Campo_Clave + " || ' ' || " + Cat_Nom_Reloj_Checador.Campo_Ubicacion + ") AS RELOJ";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador;
                    if (Datos.P_Reloj_Checador_ID != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID + " = '" + Datos.P_Reloj_Checador_ID + "'";
                    }
                    if (Datos.P_Clave != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Nom_Reloj_Checador.Campo_Clave + ") LIKE UPPER('%" + Datos.P_Clave + "%')";
                    }
                    if (Datos.P_Ubicacion != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Reloj_Checador.Campo_Ubicacion + " = '" + Datos.P_Ubicacion + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Reloj_Checador.Campo_Clave;
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
        #endregion
    }
}