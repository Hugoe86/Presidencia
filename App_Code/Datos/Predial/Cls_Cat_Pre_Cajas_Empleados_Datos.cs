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
using System.Text;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Cajas_Empleados.Negocios;

/// <summary>
/// Summary description for Cls_Cat_Pre_Cajas_Empleados_Datos
/// </summary>
namespace Presidencia.Cajas_Empleados.Datos
{
    public class Cls_Cat_Pre_Cajas_Empleados_Datos
    {
        public Cls_Cat_Pre_Cajas_Empleados_Datos()
        {
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Caja_Empleado
        /// DESCRIPCION : Da de Alta la Caja la cual va a pertenecer al empleado para pròximas
        ///               aperturas de turno
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 10-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Caja_Empleado(Cls_Cat_Pre_Cajas_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la cadena de inserción hacía la base de datos
            try
            {
                //Registra la caja que va a tener asignado el empleado para futuras aperturas
                Mi_SQL.Append("INSERT INTO " + Cat_Pre_Cajas_Empleados.Tabla_Cat_Pre_Cajas_Empleados);
                Mi_SQL.Append(" (" + Cat_Pre_Cajas_Empleados.Campo_Caja_ID + ", " + Cat_Pre_Cajas_Empleados.Campo_Empleado_ID + ")");
                Mi_SQL.Append(" VALUES ('" + Datos.P_Caja_ID + "', '" + Datos.P_Empleado_ID + "')");
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
        /// NOMBRE DE LA FUNCION: Modificar_Caja_Empleado
        /// DESCRIPCION : Modifica la caja que tiene asignada el cajero
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 10-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Caja_Empleado(Cls_Cat_Pre_Cajas_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la cadena de modificación hacía la base de datos
            try
            {
                //Modifica la caja que tiene asignada el empleado para futuras aperturas de cajas
                Mi_SQL.Append("UPDATE " + Cat_Pre_Cajas_Empleados.Tabla_Cat_Pre_Cajas_Empleados);
                Mi_SQL.Append(" SET " + Cat_Pre_Cajas_Empleados.Campo_Caja_ID + " = '" + Datos.P_Caja_ID + "'");
                Mi_SQL.Append(" WHERE " + Cat_Pre_Cajas_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
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
        /// NOMBRE DE LA FUNCION: Consulta_Caja_Empleado
        /// DESCRIPCION : Consulta el módulo y la caja que tiene asignado el empleado que
        ///               pretende abrir el turno
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 10-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Caja_Empleado(Cls_Cat_Pre_Cajas_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta de la caja asignada al empleado
            try
            {
                //Consulta la caja y el módulo al cual pertenece la caja que tiene asigando el empleado que pretende abrir el turno
                Mi_SQL.Append("SELECT " + Cat_Pre_Cajas_Empleados.Tabla_Cat_Pre_Cajas_Empleados + "." + Cat_Pre_Cajas_Empleados.Campo_Caja_ID + ", ");
                Mi_SQL.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Modulo_Id + ", ");
                Mi_SQL.Append(Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Estatus + " AS Estatus_Modulo, ");
                Mi_SQL.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Estatus + " AS Estatus_Caja,");
                Mi_SQL.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS Caja,");
                Mi_SQL.Append(" (" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave);
                Mi_SQL.Append(" || ' ' || " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Ubicacion + ") AS Modulo");
                Mi_SQL.Append(" FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + ", " + Cat_Pre_Cajas_Empleados.Tabla_Cat_Pre_Cajas_Empleados + ", ");
                Mi_SQL.Append(Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo);
                Mi_SQL.Append(" WHERE " + Cat_Pre_Cajas_Empleados.Tabla_Cat_Pre_Cajas_Empleados + "." + Cat_Pre_Cajas_Empleados.Campo_Caja_ID);
                Mi_SQL.Append(" = " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                Mi_SQL.Append(" AND " + Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Modulo_Id);
                Mi_SQL.Append(" = " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id);
                Mi_SQL.Append(" AND " + Cat_Pre_Cajas_Empleados.Tabla_Cat_Pre_Cajas_Empleados + "." + Cat_Pre_Cajas_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
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
        /// NOMBRE DE LA FUNCION: Consulta_Cajas_Modulo
        /// DESCRIPCION : Consulta todas las cajas que pertenescan el modulo seleccionado
        ///               por el usuario y que estas se encuentren vigentes
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 10-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Cajas_Modulo(Cls_Cat_Pre_Cajas_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Consulta todas las cajas que pertenecen al modulo seleccionado por el usuario
            try
            {
                //Consulta todas las cajas que pertenecen al modulo seleccionado por el usuario
                Mi_SQL.Append("SELECT " + Cat_Pre_Cajas.Campo_Caja_ID + ", ");
                Mi_SQL.Append(Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS Caja");
                Mi_SQL.Append(" FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja);
                Mi_SQL.Append(" WHERE " + Cat_Pre_Cajas.Campo_Modulo_Id + " = '" + Datos.P_Modulo_ID + "'");
                Mi_SQL.Append(" AND " + Cat_Pre_Cajas.Campo_Estatus + " = 'VIGENTE'");
                Mi_SQL.Append(" ORDER BY " + Cat_Pre_Cajas.Campo_Numero_De_Caja);
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
        /// NOMBRE DE LA FUNCION: Consulta_Modulos_Cajas
        /// DESCRIPCION : Consulta los modulos de cajas que se encuentran actualmente
        ///               vigentes para la apertura del turno
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 10-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Modulos_Cajas()
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Consulta todas las cajas que pertenecen al modulo seleccionado por el usuario
            try
            {
                //Consulta los módulos de cajas que se encuentran vigente para poder realizar la apertura de turno
                Mi_SQL.Append("SELECT " + Cat_Pre_Modulos.Campo_Modulo_Id + ", (" + Cat_Pre_Modulos.Campo_Clave);
                Mi_SQL.Append(" || ' ' || " + Cat_Pre_Modulos.Campo_Ubicacion + ") AS Modulo");
                Mi_SQL.Append(" FROM " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo);
                Mi_SQL.Append(" WHERE " + Cat_Pre_Modulos.Campo_Estatus + " = 'VIGENTE'");
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
    }
}