using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
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
using Presidencia.Sessiones;
using Presidencia.Ventanilla_Usarios.Negocio;

namespace Presidencia.Ventanilla_Usarios.Datos
{
    public class Cls_Ven_Usuarios_Datos
    {
        #region Consultas
        /// *******************************************************************************
        /// NOMBRE:         Validar_Usuario
        /// COMENTARIOS:    validara la informacion del usuario
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     01/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Validar_Usuario(Cls_Ven_Usuarios_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); ; //Variable para las consultas
            try
            {
                Mi_SQL.Append("Select ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + ".*");
                Mi_SQL.Append(" From ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes);
                
                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Email + "='" + Datos.P_Email + "' ");

                if (!String.IsNullOrEmpty(Datos.P_Password))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Password + "='" + Datos.P_Password + "' ");
                }

                if (!String.IsNullOrEmpty(Datos.P_Usuario_ID))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "='" + Datos.P_Usuario_ID + "' ");
                }
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Estatus + "='VIGENTE'");
               
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Contribuyente
        ///DESCRIPCIÓN: Genera y ejecuta consulta de los datos de contrubyentes registrados en la tabla Cat_Pre_Contribuyentes
        ///         Devuelve un datatable con los resultados y se puede filtrar por contribuyente_id
        ///PARÁMETROS:
        /// 		1. Negocio: instancia de la clase de negocio con los datos para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 21-may-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static DataTable Consultar_Contribuyente(Cls_Ven_Usuarios_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Nombre_Completo + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Sexo + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_RFC + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Edad + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Email + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Calle_ID + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Colonia_ID + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Telefono_Casa + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Estatus + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_CURP + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Codigo_Postal + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Contribuyente_ID);
                Mi_Sql.Append(" FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes);

                // agregar filtros
                if (!String.IsNullOrEmpty(Negocio.P_Usuario_ID))
                {
                    if (Mi_Sql.ToString().Trim().Contains("WHERE"))
                    {
                        Mi_Sql.Append(" AND " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = '" + Negocio.P_Usuario_ID.Trim() + "'");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = '" + Negocio.P_Usuario_ID.Trim() + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Negocio.P_Estatus))
                {
                    if (Mi_Sql.ToString().Trim().Contains("WHERE"))
                    {
                        Mi_Sql.Append(" AND " + Cat_Pre_Contribuyentes.Campo_Estatus + " = '" + Negocio.P_Estatus.Trim() + "'");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE " + Cat_Pre_Contribuyentes.Campo_Estatus + " = '" + Negocio.P_Estatus.Trim() + "'");
                    }
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consultar_Parametros
        ///DESCRIPCIÓN: Genera y ejecuta consulta de los parametros de ventanilla única
        ///PARÁMETROS:
        /// 		1. Negocio: instancia de la clase de negocio con los datos para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 21-may-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static DataTable Consultar_Parametros(Cls_Ven_Usuarios_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT " + Cat_Ven_Parametros.Campo_Programa_ID_Web);
                Mi_Sql.Append("," + Cat_Ven_Parametros.Campo_Programa_ID_Ventanilla);
                Mi_Sql.Append(",(SELECT " + Cat_Ate_Programas.Campo_Nombre + " FROM "); // subconsulta de nombre de programa Campo_Programa_ID_Web
                Mi_Sql.Append(Cat_Ate_Programas.Tabla_Cat_Ate_Programas + " WHERE ");
                Mi_Sql.Append(Cat_Ate_Programas.Campo_Programa_ID + " = ");
                Mi_Sql.Append(Cat_Ven_Parametros.Tabla_Cat_Ven_Parametros + "." + Cat_Ven_Parametros.Campo_Programa_ID_Web + ") " + Cat_Ate_Programas.Campo_Nombre);
                Mi_Sql.Append(" FROM " + Cat_Ven_Parametros.Tabla_Cat_Ven_Parametros);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        #endregion

    }
}