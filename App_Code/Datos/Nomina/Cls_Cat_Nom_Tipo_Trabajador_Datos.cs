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
using Presidencia.Tipo_Trabajador.Negocios;

/// <summary>
/// Summary description for Cls_Cat_Nom_Tipo_Trabajador_Datos
/// </summary>
namespace Presidencia.Tipo_Trabajador.Datos
{
    public class Cls_Cat_Nom_Tipo_Trabajador_Datos
    {
        public Cls_Cat_Nom_Tipo_Trabajador_Datos()
        {
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION:  Alta_Tipo_Trabajador
        /// DESCRIPCION : 1. Obtiene el proximo ID para un nuevo registro de catalogo
        ///               2. Da de Alta el Tipo de Personal al catalogo
        /// PARAMETROS  : Datos: Variable que contiene los datos a ingresar
        /// CREO        : Noe Mosqueda Valadez
        /// FECHA_CREO  : 04/Septiembre/2010 12:00
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Tipo_Trabajador(Cls_Cat_Nom_Tipo_Trabajador_Negocio Datos)
        {
            String Mi_ORACLE; //Variable para las consultas
            Object Tipo_Trabajador_ID; //Variable para el ID del tipo de trabajador

            try
            {
                //Asignar consulta para el ID
                Mi_ORACLE = "SELECT NVL(MAX(" + Cat_Nom_Tipo_Trabajador.Campo_Tipo_Trabajador_ID + "), '00000') ";
                Mi_ORACLE = Mi_ORACLE + "FROM " + Cat_Nom_Tipo_Trabajador.Tabla_Cat_Nom_Tipo_Trabajador + " ";

                //Ejecutar consulta
                Tipo_Trabajador_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_ORACLE);

                //Verificar si no es nulo
                if (Convert.IsDBNull(Tipo_Trabajador_ID))
                    Datos.P_Tipo_Trabajador_ID = "00001";
                else
                    Datos.P_Tipo_Trabajador_ID = String.Format("{0:00000}", Convert.ToInt32(Tipo_Trabajador_ID) + 1);

                //Asignar consulta para la insercion
                Mi_ORACLE = "INSERT INTO " + Cat_Nom_Tipo_Trabajador.Tabla_Cat_Nom_Tipo_Trabajador;
                Mi_ORACLE = Mi_ORACLE + " (" + Cat_Nom_Tipo_Trabajador.Campo_Tipo_Trabajador_ID + ", " + Cat_Nom_Tipo_Trabajador.Campo_Descripcion + ", ";
                Mi_ORACLE = Mi_ORACLE + Cat_Nom_Tipo_Trabajador.Campo_Estatus + ", " + Cat_Nom_Tipo_Trabajador.Campo_Comentarios + ", ";
                Mi_ORACLE = Mi_ORACLE + Cat_Nom_Tipo_Trabajador.Campo_Usuario_Creo + ", " + Cat_Nom_Tipo_Trabajador.Campo_Fecha_Creo + ") ";
                Mi_ORACLE = Mi_ORACLE + "VALUES('" + Datos.P_Tipo_Trabajador_ID + "', '" + Datos.P_Descripcion + "', '" + Datos.P_Estatus + "', ";
                Mi_ORACLE = Mi_ORACLE + "'" + Datos.P_Comentarios + "', '" + Datos.P_Nombre_Usuario + "', SYSDATE)";

                //ejecutar consulta
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_ORACLE);
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
        /// NOMBRE DE LA FUNCION: Elimina_Tipo_Trabajador    
        /// DESCRIPCION : Elimina un Tipo de Trabajador de la base de datos
        /// PARAMETROS  : Datos: Objeto que contiene el ID del Tipo de Trabajador a eliminar
        /// CREO        : Noe Mosqueda Valadez
        /// FECHA_CREO  : 04/Septiembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Elimina_Tipo_Trabajador(Cls_Cat_Nom_Tipo_Trabajador_Negocio Datos)
        {
            String Mi_ORACLE; //Variable para las consultas

            try
            {
                //Asignar consulta para eliminar
                Mi_ORACLE = "DELETE FROM " + Cat_Nom_Tipo_Trabajador.Tabla_Cat_Nom_Tipo_Trabajador + " ";
                Mi_ORACLE = Mi_ORACLE + "WHERE " + Cat_Nom_Tipo_Trabajador.Campo_Tipo_Trabajador_ID + " = '" + Datos.P_Tipo_Trabajador_ID + "'";

                //ejecutar consulta
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_ORACLE);
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
        /// NOMBRE DE LA FUNCION: Modifica_Tipo_Trabajador
        /// DESCRIPCION : Modifica los datos de un Tipo de Trabajador en la base de datos
        /// PARAMETROS  : Datos: Objeto que contiene los datos para modificar el Tipo de Trabajador
        /// CREO        : Noe Mosqueda Valadez
        /// FECHA_CREO  : 04/Septiembre/2010 12:56
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modifica_Tipo_Trabajador(Cls_Cat_Nom_Tipo_Trabajador_Negocio Datos)
        {
            String Mi_ORACLE; //Variable para las consultas

            try
            {
                //Asignar consulta para la modificacion
                Mi_ORACLE = "UPDATE " + Cat_Nom_Tipo_Trabajador.Tabla_Cat_Nom_Tipo_Trabajador + " ";
                Mi_ORACLE = Mi_ORACLE + "SET " + Cat_Nom_Tipo_Trabajador.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                Mi_ORACLE = Mi_ORACLE + Cat_Nom_Tipo_Trabajador.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                Mi_ORACLE = Mi_ORACLE + Cat_Nom_Tipo_Trabajador.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_ORACLE = Mi_ORACLE + Cat_Nom_Tipo_Trabajador.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_ORACLE = Mi_ORACLE + Cat_Nom_Tipo_Trabajador.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_ORACLE = Mi_ORACLE + "WHERE " + Cat_Nom_Tipo_Trabajador.Campo_Tipo_Trabajador_ID + " = '" + Datos.P_Tipo_Trabajador_ID + "'";

                //Ejecutar consulta
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_ORACLE);
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
        /// NOMBRE DE LA FUNCION: Consulta_Tipos_Trabajadores
        /// DESCRIPCION : Consulta los Tipos de Trabajaor que puede tener la presidencia
        /// PARAMETROS  : Datos: Objeto que contiene los datos para modificar el Tipo de Trabajador
        /// CREO        : Noe Mosqueda Valadez
        /// FECHA_CREO  : 04/Septiembre/2010 12:56
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Tipos_Trabajadores(Cls_Cat_Nom_Tipo_Trabajador_Negocio Datos)
        {
            String Mi_ORACLE; //Variable para la consulta

            try
            {
                //Asignar consulta
                Mi_ORACLE = "SELECT " + Cat_Nom_Tipo_Trabajador.Campo_Tipo_Trabajador_ID + ", " + Cat_Nom_Tipo_Trabajador.Campo_Descripcion + ", ";
                Mi_ORACLE = Mi_ORACLE + Cat_Nom_Tipo_Trabajador.Campo_Estatus + ", " + Cat_Nom_Tipo_Trabajador.Campo_Comentarios + " ";
                Mi_ORACLE = Mi_ORACLE + "FROM " + Cat_Nom_Tipo_Trabajador.Tabla_Cat_Nom_Tipo_Trabajador + " ";

                //Verificar si hay criterios de busqueda
                if (Datos.P_Tipo_Trabajador_ID != null)
                    Mi_ORACLE = Mi_ORACLE + "WHERE " + Cat_Nom_Tipo_Trabajador.Campo_Tipo_Trabajador_ID + " = '" + Datos.P_Tipo_Trabajador_ID + "' ";

                if (Datos.P_Estatus != null)
                    Mi_ORACLE = Mi_ORACLE + "WHERE " + Cat_Nom_Tipo_Trabajador.Campo_Estatus + " = '" + Datos.P_Estatus + "' ";

                if (Datos.P_Descripcion != null)
                    Mi_ORACLE = Mi_ORACLE + "WHERE UPPER(" + Cat_Nom_Tipo_Trabajador.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%') ";

                Mi_ORACLE = Mi_ORACLE + "ORDER BY " + Cat_Nom_Tipo_Trabajador.Campo_Descripcion;

                //Entregar resultado
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_ORACLE).Tables[0];
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