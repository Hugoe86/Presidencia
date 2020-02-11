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
using System.Text;
using Presidencia.Empleados_Folios.Negocio;

namespace Presidencia.Empleados_Folios.Datos
{
    public class Cls_Ope_Caj_Empleados_Folio_Datos
    {
        public Cls_Ope_Caj_Empleados_Folio_Datos()
        {
        }
        #region(Alta-Modificacion-Eliminar)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Folio_Empleado
            /// DESCRIPCION : 1.Consulta el último NO dado de alta para poder ingresar el siguiente
            ///               2. Da de Alta el Folio asignado al empleado en la BD con los 
            ///                  datos proporcionados por el usuario
            /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Alta_Folio_Empleado(Cls_Ope_Caj_Empleados_Folio_Negocios Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la cadena de inserción hacía la base de datos
                Object No_Folio;                        //Obtiene el No con la cual se guardo los datos en la base de datos

                try
                {
                    Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Empleados_Folios.Campo_No_Folio + "),'0000000000')");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Empleados_Folios.Tabla_Ope_Caj_Empleados_Folios);
                    No_Folio = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                    if (Convert.IsDBNull(No_Folio))
                    {
                        Datos.P_No_Folio = "0000000001";
                    }
                    else
                    {
                        Datos.P_No_Folio = String.Format("{0:0000000000}", Convert.ToInt32(No_Folio) + 1);
                    }
                    Mi_SQL.Length = 0;
                    //Consulta para la inserción del folio con los datos proporcionados por el usuario
                    Mi_SQL.Append("INSERT INTO " + Ope_Caj_Empleados_Folios.Tabla_Ope_Caj_Empleados_Folios + " (");
                    Mi_SQL.Append(Ope_Caj_Empleados_Folios.Campo_No_Folio + ", ");
                    Mi_SQL.Append(Ope_Caj_Empleados_Folios.Campo_Empleado_ID + ", ");
                    Mi_SQL.Append(Ope_Caj_Empleados_Folios.Campo_Folio_Inicial + ", ");
                    Mi_SQL.Append(Ope_Caj_Empleados_Folios.Campo_Folio_Final + ", ");
                    Mi_SQL.Append(Ope_Caj_Empleados_Folios.Campo_Usuario_Creo + ", ");
                    Mi_SQL.Append(Ope_Caj_Empleados_Folios.Campo_Fecha_Creo + ")");
                    Mi_SQL.Append(" VALUES ('" + Datos.P_No_Folio + "', '" + Datos.P_Empleado_ID + "',");
                    Mi_SQL.Append(" '" + Datos.P_Folio_Inicial + "', '" + Datos.P_Folio_Final + "',");
                    Mi_SQL.Append(" '" + Datos.P_Nombre_Empleado + "', SYSDATE)");
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
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
            /// NOMBRE DE LA FUNCION: Modificar_Folio_Empleado
            /// DESCRIPCION : Modifica los datos del Folio con lo que fueron introducidos por el usuario
            /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Modificar_Folio_Empleado(Cls_Ope_Caj_Empleados_Folio_Negocios Datos)
            {
                StringBuilder Mi_SQL =new StringBuilder(); //Obtiene la cadena de modificación hacía la base de datos

                try
                {
                    //Consulta para la modificación del folio con los datos proporcionados por el usuario
                    Mi_SQL.Append("UPDATE " + Ope_Caj_Empleados_Folios.Tabla_Ope_Caj_Empleados_Folios);
                    Mi_SQL.Append(" SET " + Ope_Caj_Empleados_Folios.Campo_Folio_Inicial + " = '" + Datos.P_Folio_Inicial + "', ");
                    Mi_SQL.Append(Ope_Caj_Empleados_Folios.Campo_Folio_Final + " = '" + Datos.P_Folio_Final + "', ");
                    Mi_SQL.Append(Ope_Caj_Empleados_Folios.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Empleado + "', ");
                    Mi_SQL.Append(Ope_Caj_Empleados_Folios.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Empleados_Folios.Campo_No_Folio + " = '" + Datos.P_No_Folio + "'");
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
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
            /// NOMBRE DE LA FUNCION: Actualiza_Ultimo_Folio
            /// DESCRIPCION : Actualiza el último fólio ocupado por el empleado
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Actualiza_Ultimo_Folio(Cls_Ope_Caj_Empleados_Folio_Negocios Datos)
            {
                StringBuilder Mi_SQL =new StringBuilder(); //Variable para la consulta de los folios

                try
                {
                    //Consulta para la modificación del último folio con los datos proporcionados por el usuario
                    Mi_SQL.Append("UPDATE " + Ope_Caj_Empleados_Folios.Tabla_Ope_Caj_Empleados_Folios);
                    Mi_SQL.Append(" SET " + Ope_Caj_Empleados_Folios.Campo_Ultimo_Folio_Utilizado + " = '" + Datos.P_Ultimo_Folio_Utilizado + "'");
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Empleados_Folios.Campo_Folio_Inicial + " >= '" + Datos.P_Ultimo_Folio_Utilizado + "'");
                    Mi_SQL.Append(" AND " + Ope_Caj_Empleados_Folios.Campo_Folio_Final + " <= '" + Datos.P_Folio_Final + "'");
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
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
            /// NOMBRE DE LA FUNCION: Eliminar_Folio_Empleado
            /// DESCRIPCION : Elimina el registro del Folio que fue seleccionado por el usuario de la BD
            /// PARAMETROS  : Datos: Obtiene que puesto desea eliminar de la BD
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Eliminar_Folio_Empleado(Cls_Ope_Caj_Empleados_Folio_Negocios Datos)
            {
                StringBuilder Mi_SQL =new StringBuilder(); //Variable de Consulta para la eliminación del folio
                try
                {
                    Mi_SQL.Append("DELETE FROM " + Ope_Caj_Empleados_Folios.Tabla_Ope_Caj_Empleados_Folios);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Empleados_Folios.Campo_No_Folio + " = '" + Datos.P_No_Folio + "'");
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
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
        #region(Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Folios_Empleados
            /// DESCRIPCION : Consulta todos los datos de los Folios que estan dadas de alta en
            ///               la BD con todos sus datos
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Datos_Folios_Empleados(Cls_Ope_Caj_Empleados_Folio_Negocios Datos)
            {
                StringBuilder Mi_SQL =new StringBuilder(); //Variable para la consulta de los Folios

                try
                {
                    //Consulta todos los datos del folio que se fue seleccionado por el usuario
                    Mi_SQL.Append("SELECT * FROM " + Ope_Caj_Empleados_Folios.Tabla_Ope_Caj_Empleados_Folios);
                    if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                    {
                        Mi_SQL.Append(" WHERE " + Ope_Caj_Empleados_Folios.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                    }
                    if (Datos.P_No_Folio != null)
                    {
                        Mi_SQL.Append(" WHERE " + Ope_Caj_Empleados_Folios.Campo_No_Folio + " = '" + Datos.P_No_Folio + "'");
                    }
                    Mi_SQL.Append(" ORDER BY " + Ope_Caj_Empleados_Folios.Campo_No_Folio + " DESC");
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
            /// NOMBRE DE LA FUNCION: Consulta_Rango_Folio_Empleado
            /// DESCRIPCION : Consulta si el folio que se pretende dar de alta no se haya dado
            ///               ya con anterioridad
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Rango_Folio_Empleado(Cls_Ope_Caj_Empleados_Folio_Negocios Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta si ya existe el folio o no

                try
                {
                    //Consulta todos los datos del folio que se fue seleccionado por el usuario
                    Mi_SQL.Append("SELECT " + Ope_Caj_Empleados_Folios.Campo_Folio_Inicial + ", " + Ope_Caj_Empleados_Folios.Campo_Folio_Final);
                    Mi_SQL.Append(" FROM " + Ope_Caj_Empleados_Folios.Tabla_Ope_Caj_Empleados_Folios);
                    Mi_SQL.Append(" WHERE (" + Ope_Caj_Empleados_Folios.Campo_Folio_Inicial + " >= '" + Datos.P_Folio_Inicial + "'");
                    Mi_SQL.Append(" OR  " + Ope_Caj_Empleados_Folios.Campo_Folio_Final + " >= '" + Datos.P_Folio_Inicial + "')");
                    if (!String.IsNullOrEmpty(Datos.P_No_Folio))
                    {
                        Mi_SQL.Append(" AND " + Ope_Caj_Empleados_Folios.Campo_No_Folio + " <> '" + Datos.P_No_Folio + "'");
                    }
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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