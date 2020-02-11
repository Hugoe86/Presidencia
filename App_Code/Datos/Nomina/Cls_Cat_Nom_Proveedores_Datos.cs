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
using Presidencia.Proveedores.Negocios;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Globalization;

namespace Presidencia.Proveedores.Datos
{
    public class Cls_Cat_Nom_Proveedores_Datos
    {
        #region (Metodos)

        #region (Metodos Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Proveedores
        /// DESCRIPCION : Consulta la tabla de Proveedores
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 23/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Proveedores(Cls_Cat_Nom_Proveedores_Negocio Parametros)
        {
            String Mi_SQL = "";//Variable que alamcenara la consulta.
            DataTable Dt_Proveedores = null;//Variable que alamcenara una lista de proveedores.
            try
            {
                Mi_SQL = " SELECT " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + ".* " +
                         " FROM " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores;
                if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Proveedores.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Proveedores.Campo_Nombre;

                Dt_Proveedores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Proveedores;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Deducciones_Proveedor
        /// DESCRIPCION : Consulta la tabla de Proveedores
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 23/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Deducciones_Proveedor(Cls_Cat_Nom_Proveedores_Negocio Datos) {
            String Mi_SQL = "";//Variable que alamcenara la consulta.
            DataTable Dt_Deducciones_Proveedor = null;//Variable que almacenara una lista de deducciones.

            try
            {
                Mi_SQL = " SELECT " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + ".* " +
                         " FROM " + Cat_Nom_Proveedores_Detalles.Tabla_Cat_Nom_Proveedores_Detalles +
                         " INNER JOIN " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion +
                         " ON " + Cat_Nom_Proveedores_Detalles.Tabla_Cat_Nom_Proveedores_Detalles + "." + Cat_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID +
                         "=" + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                         " WHERE " + Cat_Nom_Proveedores_Detalles.Tabla_Cat_Nom_Proveedores_Detalles + "." + Cat_Nom_Proveedores_Detalles.Campo_Proveedor_ID +
                         "='" + Datos.P_Proveedor_ID + "'";

                Dt_Deducciones_Proveedor = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                         
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Deducciones_Proveedor;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Percepciones_Deducciones
        /// DESCRIPCION : Consulta todas las Percepciones o Deducciones que se tienen activas
        ///               para la generación de la nómina
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 10-Noviembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Percepciones_Deducciones(Cls_Cat_Nom_Proveedores_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las Percepciones y Deducciones

            try
            {
                //Consulta todos las Percepciones o Deducciones que se encuentren activas en la base de datos
                Mi_SQL = "SELECT " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + "(" + Cat_Nom_Percepcion_Deduccion.Campo_Clave + " || ' ' || ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ") AS NOMBRE_CONCEPTO";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Estatus + " = 'ACTIVO'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + " = 'DEDUCCION'";
                //Mi_SQL = Mi_SQL + " and " + Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + "='VARIABLE'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Nom_Percepcion_Deduccion.Campo_Concepto + "='TIPO_NOMINA'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
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

        #region (Metodos Operacion)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Proveedores
        /// DESCRIPCION : Ejecuta el Alta de un Proveedor
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 23/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Alta_Proveedores( Cls_Cat_Nom_Proveedores_Negocio Datos ) {
            String Mi_Oracle;  //Obtiene la cadena de inserción hacía la base de datos
            Object Proveedor_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Boolean Operacion_Completa = false;//Variable para almacenar el estatus de la operacion.
            DataTable Dt_Deducciones = null;//Variable que almacenara una lista de deducciones.

            try
            {
                Mi_Oracle = "SELECT NVL(MAX(" + Cat_Nom_Proveedores.Campo_Proveedor_ID + "),'00000') ";
                Mi_Oracle = Mi_Oracle + "FROM " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores;
                Proveedor_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                if (Convert.IsDBNull(Proveedor_ID))
                {
                    Datos.P_Proveedor_ID = "00001";
                }
                else
                {
                    Datos.P_Proveedor_ID = String.Format("{0:00000}", Convert.ToInt32(Proveedor_ID) + 1);
                }

                //Consulta para la inserción del área con los datos proporcionados por el usuario
                Mi_Oracle = "INSERT INTO " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + " (" +
                Cat_Nom_Proveedores.Campo_Proveedor_ID + ", " +
                Cat_Nom_Proveedores.Campo_Nombre + ", " +
                Cat_Nom_Proveedores.Campo_RFC + ", " +
                Cat_Nom_Proveedores.Campo_Estatus + ", " +
                Cat_Nom_Proveedores.Campo_Calle + ", " +
                Cat_Nom_Proveedores.Campo_Numero + ", " +
                Cat_Nom_Proveedores.Campo_Colonia + ", " +
                Cat_Nom_Proveedores.Campo_Codigo_Postal + ", " +
                Cat_Nom_Proveedores.Campo_Ciudad + ", " +
                Cat_Nom_Proveedores.Campo_Estado + ", " +
                Cat_Nom_Proveedores.Campo_Telefono + ", " +
                Cat_Nom_Proveedores.Campo_Fax + ", " +
                Cat_Nom_Proveedores.Campo_Email + ", " +
                Cat_Nom_Proveedores.Campo_Contacto + ", " +
                Cat_Nom_Proveedores.Campo_Comentarios + ", " +
                Cat_Nom_Proveedores.Campo_Usuario_Creo + ", " +
                Cat_Nom_Proveedores.Campo_Fecha_Creo + ", " +
                Cat_Nom_Proveedores.Campo_Aval + ") VALUES ('" +
                Datos.P_Proveedor_ID + "', '" +
                Datos.P_Nombre + "', '" +
                Datos.P_RFC + "', '" +
                Datos.P_Estatus + "', '" +
                Datos.P_Calle + "', " +
                Datos.P_Numero + ", '" +
                Datos.P_Colonia + "', " +
                Datos.P_Codigo_Postal + ", '" +
                Datos.P_Ciudad + "', '" +
                Datos.P_Estado + "', '" +
                Datos.P_Telefono + "', '" +
                Datos.P_Fax + "', '" +
                Datos.P_Email + "', '" +
                Datos.P_Contacto + "', '" +
                Datos.P_Comentarios + "', '" +
                Datos.P_Usuario_Creo + "', " +
                " SYSDATE " + ", '" + 
                Datos.P_Aval + "')";
                
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);//Alta de Proveedor

                //Alta detalles del proveedor
                Dt_Deducciones = Datos.P_Dt_Deducciones;

                if (Dt_Deducciones != null)
                {
                    if (Dt_Deducciones.Rows.Count > 0)
                    {
                        foreach (DataRow Renglon in Dt_Deducciones.Rows)
                        {
                            Mi_Oracle = " INSERT INTO " + Cat_Nom_Proveedores_Detalles.Tabla_Cat_Nom_Proveedores_Detalles +
                                        " ( " + Cat_Nom_Proveedores_Detalles.Campo_Proveedor_ID + ", " +
                                                Cat_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID + ", " +
                                                Cat_Nom_Proveedores_Detalles.Campo_Usuario_Creo + ", " +
                                                Cat_Nom_Proveedores_Detalles.Campo_Fecha_Creo +
                                        " ) VALUES( " +
                                                "'" + Datos.P_Proveedor_ID + "', " +
                                                "'" + Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim() + "', " +
                                                "'" + Datos.P_Usuario_Creo + "', SYSDATE )";
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                        }
                    }
                }
                Operacion_Completa = true;
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

            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Proveedores
        /// DESCRIPCION : Ejecuta el Modificacion de un Proveedor
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 23/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Modificar_Proveedores(Cls_Cat_Nom_Proveedores_Negocio Datos) {
            Boolean Operacion_Completa = false;//Variable que alamcenara el estatus de la operacion.
            String Mi_Oracle;  //Obtiene la cadena de inserción hacía la base de datos
            DataTable Dt_Deducciones = null;//Variable que almacenara una lista de deducciones.

            try
            {
                //Actualizar los datos del Proveedor Seleccionado
                Mi_Oracle = "UPDATE " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + " SET " +
                Cat_Nom_Proveedores.Campo_Nombre + "='" + Datos.P_Nombre + "', " +
                Cat_Nom_Proveedores.Campo_RFC + "= '" + Datos.P_RFC + "', " +
                Cat_Nom_Proveedores.Campo_Estatus + "='" + Datos.P_Estatus + "', " +
                Cat_Nom_Proveedores.Campo_Calle + "='" + Datos.P_Calle + "', " +
                Cat_Nom_Proveedores.Campo_Numero + "=" + Datos.P_Numero + ", " +
                Cat_Nom_Proveedores.Campo_Colonia + "='" + Datos.P_Colonia + "', " +
                Cat_Nom_Proveedores.Campo_Codigo_Postal + "=" + Datos.P_Codigo_Postal + ", " +
                Cat_Nom_Proveedores.Campo_Ciudad + "='" + Datos.P_Ciudad + "', " +
                Cat_Nom_Proveedores.Campo_Estado + "='" + Datos.P_Estado + "', " +
                Cat_Nom_Proveedores.Campo_Telefono + "='" + Datos.P_Telefono + "', " +
                Cat_Nom_Proveedores.Campo_Fax + "='" + Datos.P_Fax + "', " +
                Cat_Nom_Proveedores.Campo_Email + "='" + Datos.P_Email + "', " +
                Cat_Nom_Proveedores.Campo_Contacto + "='" + Datos.P_Contacto + "', " +
                Cat_Nom_Proveedores.Campo_Comentarios + "='" + Datos.P_Comentarios + "', " +
                Cat_Nom_Proveedores.Campo_Usuario_Creo + "='" + Datos.P_Usuario_Modifico + "', " +
                Cat_Nom_Proveedores.Campo_Fecha_Creo + "=SYSDATE" + ", " +
                Cat_Nom_Proveedores.Campo_Aval + "='" + Datos.P_Aval + "'" +
                " WHERE " +
                 Cat_Nom_Proveedores.Campo_Proveedor_ID + "='" + Datos.P_Proveedor_ID +"'";                 

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                //Eliminamos las deducciones.
                Mi_Oracle = " DELETE FROM " + Cat_Nom_Proveedores_Detalles.Tabla_Cat_Nom_Proveedores_Detalles +
                            " WHERE " + Cat_Nom_Proveedores_Detalles.Campo_Proveedor_ID + "='" + Datos.P_Proveedor_ID  + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                //Alta detalles del proveedor
                Dt_Deducciones = Datos.P_Dt_Deducciones;

                if (Dt_Deducciones != null)
                {
                    if (Dt_Deducciones.Rows.Count > 0)
                    {
                        foreach (DataRow Renglon in Dt_Deducciones.Rows)
                        {
                            Mi_Oracle = " INSERT INTO " + Cat_Nom_Proveedores_Detalles.Tabla_Cat_Nom_Proveedores_Detalles +
                                        " ( " + Cat_Nom_Proveedores_Detalles.Campo_Proveedor_ID + ", " +
                                                Cat_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID + ", " +
                                                Cat_Nom_Proveedores_Detalles.Campo_Usuario_Creo + ", " +
                                                Cat_Nom_Proveedores_Detalles.Campo_Fecha_Creo +
                                        " ) VALUES( " +
                                                "'" + Datos.P_Proveedor_ID + "', " +
                                                "'" + Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim() + "', " +
                                                "'" + Datos.P_Usuario_Creo + "', SYSDATE )";
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                        }
                    }
                }

                Operacion_Completa = true;
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

            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Proveedores
        /// DESCRIPCION : Ejecuta la Baja de un Proveedor
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 23/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Eliminar_Proveedores(Cls_Cat_Nom_Proveedores_Negocio Datos) {
            String Mi_Oracle;  //Obtiene la cadena de inserción hacía la base de datos
            Boolean Operacion_Completa = false;//Variable que almacenara el estatus de la operacion.

            try
            {
                //Eliminamos las deducciones.
                Mi_Oracle = " DELETE FROM " + Cat_Nom_Proveedores_Detalles.Tabla_Cat_Nom_Proveedores_Detalles +
                            " WHERE " + Cat_Nom_Proveedores_Detalles.Campo_Proveedor_ID + "='" + Datos.P_Proveedor_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                //Eliminar el Proveedor Seleccionado
                Mi_Oracle = "DELETE FROM " + Cat_Nom_Proveedores.Tabla_Cat_Nom_Proveedores + 
                " WHERE " + Cat_Nom_Proveedores.Campo_Proveedor_ID + "='" + Datos.P_Proveedor_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
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
            return Operacion_Completa;
        }
        #endregion

        #endregion
    }
}