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
using Presidencia.Cat_Terceros.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Cat_Terceros.Datos
{
    public class Cls_Cat_Nom_Terceros_Datos
    {
        #region (Metodos)

        #region (Metodos Operacion [Alta - Modificar - Eliminar])
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Tercero
        /// DESCRIPCION : Consulta el MAX ID de la tabla de Terceros y obtiene el ID consecutivo 
        /// de la tabla. YPor ultimo da de alta el registro.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 5/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Alta_Tercero(Cls_Cat_Nom_Terceros_Negocio Datos) {
            Boolean Operacion_Completa=false;
            String Mi_Oracle;
            object Tercero_ID;

            try
            {
                Mi_Oracle = "SELECT NVL(MAX(" + Cat_Nom_Terceros.Campo_Tercero_ID + "),'00000') ";
                Mi_Oracle = Mi_Oracle + "FROM " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros;
                Tercero_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                if (Convert.IsDBNull(Tercero_ID))
                {
                    Datos.P_Tercero_ID= "00001";
                }
                else
                {
                    Datos.P_Tercero_ID = String.Format("{0:00000}", Convert.ToInt32(Tercero_ID) + 1);
                }

                ///Aqui se realiza la insercion del registro
                Mi_Oracle = "INSERT INTO " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + " (" +
                    Cat_Nom_Terceros.Campo_Tercero_ID + ", " +
                    Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID + ", " +
                    Cat_Nom_Terceros.Campo_Nombre + ", " +
                    Cat_Nom_Terceros.Campo_Contacto + ", " +
                    Cat_Nom_Terceros.Campo_Telefono + ", " +
                    Cat_Nom_Terceros.Campo_Porcentaje_Retencion + ", " +
                    Cat_Nom_Terceros.Campo_Comentarios + ", " +
                    Cat_Nom_Terceros.Campo_Usuario_Creo + ", " +
                    Cat_Nom_Terceros.Campo_Fecha_Creo + ") VALUES(" +                
                    "'" + Datos.P_Tercero_ID + "', " +
                    "'" + Datos.P_Percepcion_Deduccion_ID + "', " + 
                    "'" + Datos.P_Nombre + "', " +
                    "'" + Datos.P_Contacto + "', " +
                    "'" + Datos.P_Telefono + "', " +
                    "" + Datos.P_Porcentaje_Retencion + ", " +
                    "'" + Datos.P_Comentarios + "', " +
                    "'" + Datos.P_Usuario_Creo + "', SYSDATE)";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al dar de alta al Tercero. Error:["+Ex.Message+"]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Tercero
        /// DESCRIPCION : Actualiza la informacion del registro seleccionado
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 5/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Modificar_Tercero(Cls_Cat_Nom_Terceros_Negocio Datos)
        {
            Boolean Operacion_Completa = false;
            String Mi_Oracle;
            try
            {
                Mi_Oracle = "UPDATE " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + " SET " +
                    Cat_Nom_Terceros.Campo_Percepcion_Deduccion_ID + "='" + Datos.P_Percepcion_Deduccion_ID + "', " +
                    Cat_Nom_Terceros.Campo_Nombre + "='" + Datos.P_Nombre + "', " +
                    Cat_Nom_Terceros.Campo_Contacto + "='" + Datos.P_Contacto + "', " +
                    Cat_Nom_Terceros.Campo_Telefono + "='" + Datos.P_Telefono + "', " +
                    Cat_Nom_Terceros.Campo_Porcentaje_Retencion + "=" + Datos.P_Porcentaje_Retencion + ", " +
                    Cat_Nom_Terceros.Campo_Comentarios + "='" + Datos.P_Comentarios + "', " +
                    Cat_Nom_Terceros.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', " +
                    Cat_Nom_Terceros.Campo_Fecha_Modifico + "= SYSDATE" + " WHERE " +
                    Cat_Nom_Terceros.Campo_Tercero_ID + "='" + Datos.P_Tercero_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al dar de alta al Tercero. Error:[" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Tercero
        /// DESCRIPCION : Elimina el registro seleccionado que coincide con el ID  proporcionado.
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 5/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Eliminar_Tercero(Cls_Cat_Nom_Terceros_Negocio Datos) {
            Boolean Operacion_Completa = false;
            String Mi_Oracle;
            try
            {
                Mi_Oracle = "DELETE FROM " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + " WHERE " +
                    Cat_Nom_Terceros.Campo_Tercero_ID + " = '" + Datos.P_Tercero_ID +"'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Eliminar el registro seleccionado. Este registro esta referenciado por el modulo de Empleados. Error:[" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        #endregion

        #region (Metodos Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Terceros
        /// DESCRIPCION : Consulta y se trae todos los registros de la tabla de terceros
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 5/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Terceros()
        {
            DataTable Dt_Terceros=null;
            String Mi_ORACLE="";
            try
            {
                Mi_ORACLE = "SELECT " + Constantes.Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + ".* FROM " + Constantes.Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros;
                Dt_Terceros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_ORACLE).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al dar de alta al Tercero. Error:[" + Ex.Message + "]");
            }
            return Dt_Terceros;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Terceros_Por_Nombre
        /// DESCRIPCION : Consulta y se trae todos los registros de la tabla de terceros
        /// que coincidan con el ID proporcionado
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 5/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Terceros_Por_Nombre(Cls_Cat_Nom_Terceros_Negocio Datos)
        {
            DataTable Dt_Terceros = null;
            String Mi_ORACLE = "";
            try
            {
                Mi_ORACLE = "SELECT " + Constantes.Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + ".* FROM " + Constantes.Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros;
                  
                if (!string.IsNullOrEmpty(Datos.P_Tercero_ID))
                {
                    if (Mi_ORACLE.Contains("WHERE"))
                    {
                        Mi_ORACLE += " AND " + Cat_Nom_Terceros.Campo_Tercero_ID + "='" + Datos.P_Tercero_ID + "'";
                    }
                    else
                    {
                        Mi_ORACLE += " WHERE " + Cat_Nom_Terceros.Campo_Tercero_ID + "='" + Datos.P_Tercero_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_ORACLE.Contains("WHERE"))
                    {
                        Mi_ORACLE += " AND UPPER(" + Cat_Nom_Terceros.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                    else
                    {
                        Mi_ORACLE += " WHERE UPPER(" + Cat_Nom_Terceros.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                }


                Dt_Terceros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_ORACLE).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al dar de alta al Tercero. Error:[" + Ex.Message + "]");
            }
            return Dt_Terceros;
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
        public static DataTable Consulta_Percepciones_Deducciones(Cls_Cat_Nom_Terceros_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las Percepciones y Deducciones

            try
            {
                //Consulta todos las Percepciones o Deducciones que se encuentren activas en la base de datos
                Mi_SQL = "SELECT " + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Nom_Percepcion_Deduccion.Campo_Clave + ", " + Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Nom_Percepcion_Deduccion.Campo_Estatus + " = 'ACTIVO'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Nom_Percepcion_Deduccion.Campo_Tipo + " = 'DEDUCCION'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion + "='OPERACION'";
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

        #endregion

    }
}