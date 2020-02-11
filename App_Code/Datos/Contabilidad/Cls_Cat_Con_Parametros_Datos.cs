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
using Presidencia.Parametros_Contabilidad.Negocio;

namespace Presidencia.Parametros_Contabilidad.Datos
{
    public class Cls_Cat_Con_Parametros_Datos
    {
        #region (Metodos Operacion)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Parametros
        /// DESCRIPCION : 1.Consulta el último ID dado de alta en la tabla.
        ///               2. Da de alta el nuevo registro con los datos proporcionados por
        ///                  el usuario.
        /// PARAMETROS  : Datos: Almacena los datos a insertarse en la BD.
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 15/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Parametros(Cls_Cat_Con_Parametros_Negocio Datos)
        {
            String Mi_SQL;   //Variable de Consulta para la Alta del de una Nueva Mascara
            Object Parametros_ID; //Variable que contendrá el ID de la consulta

            try
            {
                //Busca el maximo ID de la tabla Parametros.
                Mi_SQL = "SELECT NVL(MAX (" + Cat_Con_Parametros.Campo_Parametro_Contabilidad_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Con_Parametros.Tabla_Cat_Con_Parametros;
                Parametros_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Parametros_ID)) //Si no existen valores en la tabla, asigna el primer valor manualmente.
                {
                    Datos.P_Parametro_Contabilidad_ID = "00001";
                }
                else // Si ya existen registros, toma el valor maximo y le suma 1 para el nuevo registro.
                {
                    Datos.P_Parametro_Contabilidad_ID = String.Format("{0:00000}", Convert.ToInt32(Parametros_ID) + 1);
                }
                //Da de Alta los datos del Nuevo Parametro con los datos proporcionados por el usuario.
                Mi_SQL = "INSERT INTO " + Cat_Con_Parametros.Tabla_Cat_Con_Parametros + " (";
                Mi_SQL = Mi_SQL + Cat_Con_Parametros.Campo_Parametro_Contabilidad_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Con_Parametros.Campo_Mes_Contable + ", ";
                Mi_SQL = Mi_SQL + Cat_Con_Parametros.Campo_Mascara_Cuenta_Contable + ", ";
                Mi_SQL = Mi_SQL + Cat_Con_Parametros.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Con_Parametros.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES ('" + Datos.P_Parametro_Contabilidad_ID + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Mes_Contable + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Mascara_Cuenta_Contable + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Usuario_Creo + "', SYSDATE)";

                //Manda Mi_SQL para ser procesada por ORACLE.
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
        /// NOMBRE DE LA FUNCION: Consulta_Parametros
        /// DESCRIPCION : 1.Consulta el último ID dado de alta en la tabla.
        ///               2.Regresa la mascara correspondiente a ese ID.
        /// PARAMETROS  : 
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 19/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Parametros()
        {
            String Mi_SQL; //Variable para almacenar la instruccion de consulta a la BD.
            try
            {
                Mi_SQL = "SELECT " + Cat_Con_Parametros.Campo_Mascara_Cuenta_Contable;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Parametros.Tabla_Cat_Con_Parametros;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Parametros.Campo_Parametro_Contabilidad_ID + "=";
                Mi_SQL = Mi_SQL + "(SELECT MAX(" + Cat_Con_Parametros.Campo_Parametro_Contabilidad_ID + ") FROM ";
                Mi_SQL = Mi_SQL + Cat_Con_Parametros.Tabla_Cat_Con_Parametros + ")";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Parametros
        /// DESCRIPCION : 1. Modifica el Unico registro existente
        /// PARAMETROS  : Datos: Almacena los datos que van a modificarse en la BD.
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 21/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Parametros(Cls_Cat_Con_Parametros_Negocio Datos)
        {
            String Mi_SQL;   //Variable de Consulta para la Alta del de una Nueva Mascara
            Object Parametros_ID; //Variable que contendrá el ID de la consulta

            try
            {
                //Modifica los datos del Nuevo Parametro con los datos proporcionados por el usuario.
                Mi_SQL = "UPDATE " + Cat_Con_Parametros.Tabla_Cat_Con_Parametros;
                Mi_SQL += " SET " + Cat_Con_Parametros.Campo_Mascara_Cuenta_Contable + " = '" + Datos.P_Mascara_Cuenta_Contable + "', ";
                Mi_SQL += Cat_Con_Parametros.Campo_Mes_Contable + " = '" + Datos.P_Mes_Contable + "', ";
                Mi_SQL += Cat_Con_Parametros.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Modifico + "', ";
                Mi_SQL += Cat_Con_Parametros.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL += " WHERE " + Cat_Con_Parametros.Campo_Parametro_Contabilidad_ID + " = '00001'";

                //Manda Mi_SQL para ser procesada por ORACLE.
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
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Parametros
        /// DESCRIPCION : Consulta los datos almacenados
        /// PARAMETROS  : 
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 21/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Parametros()
        {
            String Mi_SQL; //Variable para almacenar la instruccion de consulta a la BD.
            try
            {
                Mi_SQL = "SELECT * FROM " + Cat_Con_Parametros.Tabla_Cat_Con_Parametros;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Oracle Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        #endregion
    }
}
