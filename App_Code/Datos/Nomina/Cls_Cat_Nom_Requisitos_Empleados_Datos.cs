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
using System.Data.OracleClient;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Requisitos_Empleados.Negocios;

namespace Presidencia.Requisitos_Empleados.Datos
{
    public class Cls_Cat_Nom_Requisitos_Empleados_Datos
    {
        public Cls_Cat_Nom_Requisitos_Empleados_Datos()
        {
        }

        #region (Metodos)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Requisitos_Empleados
        /// DESCRIPCION : Consulta de la base de Datos todos los requisitos del empleados
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 19/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
            public static DataTable Consultar_Requisitos_Empleados(){
                String Mi_SQL = "SELECT " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados + ".* " +
                    "FROM " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados;

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                DataTable tabla;
                if (dataset == null)
                {
                    tabla = new DataTable();
                }
                else
                {
                    tabla = dataset.Tables[0];
                }
                return tabla;
            }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta de un requisito para el empleado
        /// DESCRIPCION : Ejecuta el alta de un requisito para el empleado
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 19/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
            public static Boolean Alta_Requisito_Empleado(Cls_Cat_Nom_Requisitos_Empleado_Negocio Datos)
            {
                String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
                Object Requisito_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos
                Boolean Operacion_Completa = false;

                try
                {
                    Mi_SQL = "SELECT NVL(MAX(" + Cat_Nom_Requisitos_Empleados.Campo_Requisito_ID + "),'00000') ";
                    Mi_SQL = Mi_SQL + "FROM " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados;
                    Requisito_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    if (Convert.IsDBNull(Requisito_ID))
                    {
                        Datos.P_Requisito_ID = "00001";
                    }
                    else
                    {
                        Datos.P_Requisito_ID = String.Format("{0:00000}", Convert.ToInt32(Requisito_ID) + 1);
                    }
                    //Consulta para la inserción del área con los datos proporcionados por el usuario
                    Mi_SQL = "INSERT INTO " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados + " (";
                    Mi_SQL = Mi_SQL + Cat_Nom_Requisitos_Empleados.Campo_Requisito_ID + ", " + Cat_Nom_Requisitos_Empleados.Campo_Nombre + ", ";
                    Mi_SQL = Mi_SQL + Cat_Nom_Requisitos_Empleados.Campo_Estatus + ", " + Cat_Nom_Requisitos_Empleados.Campo_Archivo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Nom_Requisitos_Empleados.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Nom_Requisitos_Empleados.Campo_Fecha_Creo + ") VALUES ('";
                    Mi_SQL = Mi_SQL + Datos.P_Requisito_ID + "', '" + Datos.P_Nombre + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Estatus + "', '" + Datos.P_Archivo + "', '" + Datos.P_Usuario_Creo + "', ";
                    Mi_SQL = Mi_SQL + "SYSDATE)";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
            /// NOMBRE DE LA FUNCION: Actualizar_Requisito_Empleado
            /// DESCRIPCION : Ejecuta la Actualizacion de un requisito para el empleado
            /// CREO        : Juan Alberto Hernández Negrete
            /// FECHA_CREO  : 19/Octubre/2010
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static Boolean Actualizar_Requisito_Empleado(Cls_Cat_Nom_Requisitos_Empleado_Negocio Datos)
            {
                String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
                Boolean Operacion_Completa = false;

                try
                {
                    //Consulta para la inserción del área con los datos proporcionados por el usuario
                    Mi_SQL = "UPDATE " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados + " SET " +
                        Cat_Nom_Requisitos_Empleados.Campo_Nombre + "='" + Datos.P_Nombre + "', " +
                        Cat_Nom_Requisitos_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "', " +
                        Cat_Nom_Requisitos_Empleados.Campo_Archivo + "='" + Datos.P_Archivo + "', " +
                        Cat_Nom_Requisitos_Empleados.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', " +
                        Cat_Nom_Requisitos_Empleados.Campo_Fecha_Modifico + "= SYSDATE " +
                        " WHERE " + Cat_Nom_Requisitos_Empleados.Campo_Requisito_ID + "='" + Datos.P_Requisito_ID + "'";

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
            /// NOMBRE DE LA FUNCION: Eliminar_Requisito_Empleado
            /// DESCRIPCION : Ejecuta la baja del requisito
            /// CREO        : Juan Alberto Hernández Negrete
            /// FECHA_CREO  : 19/Octubre/2010
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static Boolean Eliminar_Requisito_Empleado(Cls_Cat_Nom_Requisitos_Empleado_Negocio Datos)
            {
                String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
                Boolean Operacion_Completa = false;

                try
                {
                    //Consulta para la inserción del área con los datos proporcionados por el usuario
                    Mi_SQL = "DELETE FROM " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados +
                        " WHERE " + Cat_Nom_Requisitos_Empleados.Campo_Requisito_ID + "='" + Datos.P_Requisito_ID + "'";

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
    }
}