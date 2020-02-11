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
using Presidencia.Plantillas.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Tra_Plantillas_Datos
/// </summary>

namespace Presidencia.Plantillas.Datos
{
    public class Cls_Cat_Tra_Plantillas_Datos
    {
        #region Metodos 
                
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Plantillas
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia para dar de alta plantillas
        ///PARAMETROS:  1.- Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio Objeto de la clase de Negocios.
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 22/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Alta_Plantilla(Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio)
        {
            try
            {
                String Mi_SQL = "INSERT INTO " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + " (" +
                    Cat_Tra_Plantillas.Campo_Plantilla_ID +
                    ", " + Cat_Tra_Plantillas.Campo_Nombre +
                    "," + Cat_Tra_Plantillas.Campo_Archivo +
                    ", " + Cat_Tra_Plantillas.Campo_Usuario_Creo +
                    ", " + Cat_Tra_Plantillas.Campo_Fecha_Creo +
                    ") values ('" +
                    Plantilla_Negocio.P_Plantilla_ID +
                    "','" + Plantilla_Negocio.P_Nombre +
                    "','" + Plantilla_Negocio.P_Nombre_Archivo +
                    "','" + Plantilla_Negocio.P_Usuario +
                    "',SYSDATE)";
                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar insertar los registros. Error: [" + Ex.Message + "]");
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Plantillas
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia para modificar plantillas
        ///PARAMETROS:  1.- Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio Objeto de la clase de Negocios.
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 22/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Plantilla(Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio)
        {
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "UPDATE " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + " SET " +
                            Cat_Tra_Plantillas.Campo_Nombre + " = '" + Plantilla_Negocio.P_Nombre + "', " +
                            Cat_Tra_Plantillas.Campo_Archivo + " = '" + Plantilla_Negocio.P_Nombre_Archivo + "', " +
                            Cat_Tra_Plantillas.Campo_Usuario_Modifico + " = '" + Plantilla_Negocio.P_Usuario + "'," +
                            Cat_Tra_Plantillas.Campo_Fecha_Modifico + " = SYSDATE " +
                            " WHERE " + Cat_Tra_Plantillas.Campo_Plantilla_ID + " = '" + Plantilla_Negocio.P_Plantilla_ID + "'";
                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar insertar los registros. Error: [" + Ex.Message + "]");
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Plantillas
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia para eliminar plantillas
        ///PARAMETROS:  1.- Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio Objeto de la clase de Negocios.
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 22/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public void Eliminar_Plantilla(Cls_Cat_Tra_Plantillas_Negocio Plantillas_Negocio)
        {
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "DELETE FROM " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + " WHERE " +
                          Cat_Tra_Plantillas.Campo_Plantilla_ID + " = '" + Plantillas_Negocio.P_Plantilla_ID + "' ";
                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar insertar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Plantillas
        ///DESCRIPCIÓN: Metodo que consulta los registros de la tabla Cat_Tra_Plantillas
        ///PARAMETROS:  1.- Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio Objeto de la clase de Negocios.
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 22/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public DataSet Consulta_Plantillas(Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio)
        {
            DataSet Data_Set;
            try
            {
                String Mi_SQL = "SELECT " + Cat_Tra_Plantillas.Campo_Plantilla_ID + ", " +
                        Cat_Tra_Plantillas.Campo_Nombre + ", " +
                        Cat_Tra_Plantillas.Campo_Archivo +
                        " FROM " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas;

                if (Plantilla_Negocio.P_Campo_Buscar != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE  trim(upper(" + Cat_Tra_Plantillas.Campo_Nombre +
                        ")) LIKE  trim(upper('%" + Plantilla_Negocio.P_Campo_Buscar + "%'))";
                }
                Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar insertar los registros. Error: [" + Ex.Message + "]");
            }
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consecutivo
        ///DESCRIPCIÓN: Metodo que verfifica el Max_ID en la tabla y ayuda a generar el nuevo Id. 
        ///PARAMETROS: 
        ///CREO:
        ///FECHA_CREO: 24/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public string Consecutivo()
        {
            String Consecutivo = "";
            String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
            Object Asunto_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            try
            {
                Mi_SQL = "SELECT NVL(MAX (" + Cat_Tra_Plantillas.Campo_Plantilla_ID + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas;
                Asunto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Asunto_ID))
                {
                    Consecutivo = "00001";
                }
                else
                {
                    Consecutivo = string.Format("{0:00000}", Convert.ToInt32(Asunto_ID) + 1);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar insertar los registros. Error: [" + Ex.Message + "]");
            }
            return Consecutivo;
        }//fin de Max_ID


        #endregion
        
    }
}