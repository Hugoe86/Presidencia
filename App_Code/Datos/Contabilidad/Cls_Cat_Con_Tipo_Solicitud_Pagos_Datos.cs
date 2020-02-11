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
using System.Data.OracleClient;
using System.Text;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Tipo_Solicitud_Pagos.Negocios;

namespace Presidencia.Tipo_Solicitud_Pagos.Datos
{
    public class Cls_Cat_Con_Tipo_Solicitud_Pagos_Datos
    {
        public Cls_Cat_Con_Tipo_Solicitud_Pagos_Datos()
        {
        }
        #region (Métodos Operación)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Tipo_Solicitud_Pago
            /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
            ///               2. Da de Alta El Tipo de Solictud de Pago en la BD con los datos 
            ///                  proporcionados por elusuario
            /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Alta_Tipo_Solicitud_Pago(Cls_Cat_Con_Tipo_Solicitud_Pagos_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la cadena de inserción a realizar hacia la base de datos
                Object Tipo_Solicitud_Pago_ID;              //Obtiene el ID que le corresponde al nuevo registro en la base de datos
                try
                {
                    //Consulta el último ID que fue dato de alta en la base de datos
                    Mi_SQL.Append("SELECT NVL(MAX(" + Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + "),'00000')");
                    Mi_SQL.Append(" FROM " + Cat_Con_Tipo_Solicitud_Pagos.Tabla_Cat_Con_Tipo_Solicitud_Pago);
                    Tipo_Solicitud_Pago_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                    if (Convert.IsDBNull(Tipo_Solicitud_Pago_ID))
                    {
                        Datos.P_Tipo_Solicitud_Pago_ID = "00001";
                    }
                    else
                    {
                        Datos.P_Tipo_Solicitud_Pago_ID = String.Format("{0:00000}", Convert.ToInt32(Tipo_Solicitud_Pago_ID) + 1);
                    }

                    Mi_SQL.Length = 0;
                    //Inserta un nuevo registro en la base de datos con los datos obtenidos por el usuario
                    Mi_SQL.Append("INSERT INTO " + Cat_Con_Tipo_Solicitud_Pagos.Tabla_Cat_Con_Tipo_Solicitud_Pago);
                    Mi_SQL.Append(" (" + Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + ", " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Descripcion + ", ");
                    Mi_SQL.Append(Cat_Con_Tipo_Solicitud_Pagos.Campo_Estatus + ", " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Comentarios + ", ");
                    Mi_SQL.Append(Cat_Con_Tipo_Solicitud_Pagos.Campo_Usuario_Creo + ", " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Fecha_Creo + ")");
                    Mi_SQL.Append(" VALUES ('" + Datos.P_Tipo_Solicitud_Pago_ID + "', '" + Datos.P_Descripcion + "', '" + Datos.P_Estatus + "', ");
                    if (!String.IsNullOrEmpty(Datos.P_Comentarios))
                    {
                        Mi_SQL.Append("'" + Datos.P_Comentarios + "', ");
                    }
                    else
                    {
                        Mi_SQL.Append("NULL, ");
                    }
                    Mi_SQL.Append("'" + Datos.P_Nombre_Usuario + "', SYSDATE)");
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
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
            /// NOMBRE DE LA FUNCION: Modificar_Tipo_Solicitud_Pago
            /// DESCRIPCION : Modifica los datos del Tipo de Solicitud de Pago con lo que fueron 
            ///               introducidos por el usuario
            /// PARAMETROS  :  Datos: Datos que son enviados de la capa de Negocios y que fueron 
            ///                       proporcionados por el usuario y van a sustituir a los datos que se
            ///                       encuentran en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Modificar_Tipo_Solicitud_Pago(Cls_Cat_Con_Tipo_Solicitud_Pagos_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Variable a contener la modificación de los datos a la base de datos
                try
                {
                    //Modifica el registro del tipo de pago seleccionado por el usuario con los nuevos valores que fueron proporcionados por el usuario
                    Mi_SQL.Append("UPDATE " + Cat_Con_Tipo_Solicitud_Pagos.Tabla_Cat_Con_Tipo_Solicitud_Pago);
                    Mi_SQL.Append(" SET " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ");
                    Mi_SQL.Append(Cat_Con_Tipo_Solicitud_Pagos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ");
                    if (!String.IsNullOrEmpty(Datos.P_Comentarios))
                    {
                        Mi_SQL.Append(Cat_Con_Tipo_Solicitud_Pagos.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ");
                    }
                    else
                    {
                        Mi_SQL.Append(Cat_Con_Tipo_Solicitud_Pagos.Campo_Comentarios + " = NULL, ");
                    }
                    Mi_SQL.Append(Cat_Con_Tipo_Solicitud_Pagos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Cat_Con_Tipo_Solicitud_Pagos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + " = '" + Datos.P_Tipo_Solicitud_Pago_ID + "'");
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
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
            /// NOMBRE DE LA FUNCION: Eliminar_Tipo_Solicitud_Pago
            /// DESCRIPCION : Elimina el Tipo de Solicitud que fue seleccionada por el usuario de la BD
            /// PARAMETROS  : Datos: Obtiene que Cuenta Contable desea eliminar de la BD
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Eliminar_Tipo_Solicitud_Pago(Cls_Cat_Con_Tipo_Solicitud_Pagos_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Variable a contener la eliminación del registro hacia la base de datos
                try
                {
                    //Elimina el registro al cual pertenece el tipo de solicitud de pago que fue seleccionado por el usuario
                    Mi_SQL.Append("DELETE FROM " + Cat_Con_Tipo_Solicitud_Pagos.Tabla_Cat_Con_Tipo_Solicitud_Pago);
                    Mi_SQL.Append(" WHERE " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + " = '" + Datos.P_Tipo_Solicitud_Pago_ID + "'");
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
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
            /// NOMBRE DE LA FUNCION: Consulta_Tipo_Solicitud_Pagos
            /// DESCRIPCION : Consulta todos los datos que se tienen registrados en la base
            ///               de datos ya sea de acuerdo a los parametros proporcionado pos el
            ///               usuario o todos
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Tipo_Solicitud_Pagos(Cls_Cat_Con_Tipo_Solicitud_Pagos_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos

                try
                {
                    //Consulta todos los datos de tipo de solicitud que estan registrados en la base de datos, si el usuario proporciono algun tipo
                    //de filtro por consultar entonces solo consultara los registros que coincidan con los valores proporcionados
                    Mi_SQL.Append("SELECT * FROM " + Cat_Con_Tipo_Solicitud_Pagos.Tabla_Cat_Con_Tipo_Solicitud_Pago);
                    if(!String.IsNullOrEmpty(Datos.P_Tipo_Solicitud_Pago_ID))
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + " = '" + Datos.P_Tipo_Solicitud_Pago_ID + "'");
                    }
                    if(!String.IsNullOrEmpty(Datos.P_Descripcion))
                    {
                        Mi_SQL.Append(" WHERE UPPER(" + Cat_Con_Tipo_Solicitud_Pagos.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')");
                    }
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
            /// NOMBRE DE LA FUNCION: Consulta_Tipo_Solicitud_Pagos_Combo
            /// DESCRIPCION : Consulta unicamente los datos que son necesario para el llenado
            ///               de algun combo dentro del sistema
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 16-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Tipo_Solicitud_Pagos_Combo(Cls_Cat_Con_Tipo_Solicitud_Pagos_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos

                try
                {
                    Mi_SQL.Append("SELECT " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + ", " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Descripcion);
                    Mi_SQL.Append(" FROM " + Cat_Con_Tipo_Solicitud_Pagos.Tabla_Cat_Con_Tipo_Solicitud_Pago);
                    Mi_SQL.Append(" WHERE " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Estatus + " = 'ACTIVO'");
                    if (!String.IsNullOrEmpty(Datos.P_Descripcion))
                    {
                        Mi_SQL.Append(" AND " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Descripcion + "LIKE UPPER ('%" + Datos.P_Descripcion + "%')");
                    }
                    if (!String.IsNullOrEmpty(Datos.P_Tipo_Solicitud_Pago_ID))
                    {
                        Mi_SQL.Append(" AND " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + " = '" + Datos.P_Tipo_Solicitud_Pago_ID + "'");
                    }
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
    }
}