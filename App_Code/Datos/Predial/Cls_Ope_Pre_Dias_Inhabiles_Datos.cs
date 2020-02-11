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
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;

namespace Presidencia.Operacion_Predial_Dias_Inhabiles.Datos
{
    public class Cls_Ope_Pre_Dias_Inhabiles_Datos
    {
        public Cls_Ope_Pre_Dias_Inhabiles_Datos()
        {
        }
        #region Metodos
        #region(Alta-Modificacion-Eliminacion)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Dia_Inhabil
        /// DESCRIPCION : 1.Consulta el último No dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Día Inhabil con todos los datos proporcionados
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 08-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Dia_Inhabil(Cls_Ope_Pre_Dias_Inhabiles_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta de inserción a realizar en la base de datos
            Object No_Dia_Inhabil;                      //Obtiene el No con el cual se va a guardar el registro
            try
            {
                //Consulta el No. Consecutivo que le corresponderá al nuevo registro
                Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Pre_Dias_Inhabiles.Campo_Dia_Inhabil_ID + "),'0000000000') ");
                Mi_SQL.Append(" FROM " + Ope_Pre_Dias_Inhabiles.Tabla_Ope_Pre_Dias_Inhabiles);
                No_Dia_Inhabil = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                if (Convert.IsDBNull(No_Dia_Inhabil))
                {
                    No_Dia_Inhabil = "0000000001";
                }
                else
                {
                    No_Dia_Inhabil = String.Format("{0:0000000000}", Convert.ToInt32(No_Dia_Inhabil) + 1);
                }

                Datos.P_No_Dia_Inhabil = Convert.ToString(No_Dia_Inhabil);

                Mi_SQL.Length = 0;
                //Inserción del registro a la base de datos con los datos proporcionados por el usuario
                Mi_SQL.Append("INSERT INTO " + Ope_Pre_Dias_Inhabiles.Tabla_Ope_Pre_Dias_Inhabiles);
                Mi_SQL.Append(" (" + Ope_Pre_Dias_Inhabiles.Campo_Dia_Inhabil_ID + ", ");
                Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Dia_ID + ", ");
                Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Estatus + ", ");
                Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Fecha + ", ");
                Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Descripcion + ", ");
                Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Motivo + ", ");
                Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Fecha_Creo + ")");
                Mi_SQL.Append(" VALUES ('" + Datos.P_No_Dia_Inhabil + "', ");
                if (!String.IsNullOrEmpty(Datos.P_Dia_ID))
                {
                    Mi_SQL.Append("'" + Datos.P_Dia_ID + "', ");
                }
                else
                {
                    Mi_SQL.Append("NULL, ");
                }
                Mi_SQL.Append("'" + Datos.P_Estatus + "', ");
                Mi_SQL.Append("TO_TIMESTAMP ('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Datos.P_Fecha) + "', 'DD/MM/YYYY HH24:MI:SS'), ");
                Mi_SQL.Append("'" + Datos.P_Descripcion + "', ");
                if (!String.IsNullOrEmpty(Datos.P_Motivo))
                {
                    Mi_SQL.Append("'" + Datos.P_Motivo + "', ");
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
        /// NOMBRE DE LA FUNCION: Modiicar_Dia_Inhabil
        /// DESCRIPCION : Modifica los datos del Día Inhabil con los datos que fueron
        ///               introducidos por el usuario
        /// PARAMETROS  : Datos: Datos que son enviados de la capa de Negocios y que fueron 
        ///                      proporcionados por el usuario y van a sustituir a los datos que se
        ///                      encuentran en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 08-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modiicar_Dia_Inhabil(Cls_Ope_Pre_Dias_Inhabiles_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta de modificación de los datos a la base de datos
            try
            {
                //Se obtiene la modificación a la base de datos con todos los datos proporcionados por el usuario
                Mi_SQL.Append("UPDATE " + Ope_Pre_Dias_Inhabiles.Tabla_Ope_Pre_Dias_Inhabiles);
                Mi_SQL.Append(" SET " + Ope_Pre_Dias_Inhabiles.Campo_Fecha + " = TO_TIMESTAMP ('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Datos.P_Fecha) + "', 'DD/MM/YYYY HH24: MI: SS'), ");
                Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Estatus + " = '" + Datos.P_Estatus + "', ");
                Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ");
                if (!String.IsNullOrEmpty(Datos.P_Dia_ID))
                {
                    Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Dia_ID + " = '" + Datos.P_Dia_ID + "', ");
                }
                else
                {
                    Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Dia_ID + " = NULL, ");
                }
                if (!String.IsNullOrEmpty(Datos.P_Motivo))
                {
                    Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Motivo + " = '" + Datos.P_Motivo + "', ");
                }
                else
                {
                    Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Motivo + " = NULL, ");
                }
                Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                Mi_SQL.Append(Ope_Pre_Dias_Inhabiles.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Pre_Dias_Inhabiles.Campo_Dia_Inhabil_ID + " = '" + Datos.P_No_Dia_Inhabil + "'");
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
        /// NOMBRE DE LA FUNCION: Eliminar_Dia_Inhabil
        /// DESCRIPCION : Elimina el Día Inhabil que fue seleccionada por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que dependencia desea eliminar de la BD
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 08-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_Dia_Inhabil(Cls_Ope_Pre_Dias_Inhabiles_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta de eliminación a realizar a la base de datos
            try
            {
                //Obtiene la consulta a realizar a la base de datos para poder eliminar el registro seleccionado por usuario
                Mi_SQL.Append("DELETE FROM " + Ope_Pre_Dias_Inhabiles.Tabla_Ope_Pre_Dias_Inhabiles);
                Mi_SQL.Append(" WHERE " + Ope_Pre_Dias_Inhabiles.Campo_Dia_Inhabil_ID + " = '" + Datos.P_No_Dia_Inhabil + "'");
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
        /// NOMBRE DE LA FUNCION: Consulta_Dias_Inhabiles
        /// DESCRIPCION : Consulta los datos de los días inhabiles de acuerdo a los parámetros
        ///               proporcionados por el usuario
        /// PARAMETROS  : Datos: Obtiene los parámetros de consulta a realizar
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 08-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Dias_Inhabiles(Cls_Ope_Pre_Dias_Inhabiles_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar para la obtención de los datos
            DateTime Fecha_Inicial = DateTime.MinValue;
            DateTime Fecha_Final = DateTime.MaxValue;

            try
            {
                //Consulta todos los datos de los días inhabiles de acuerdo a los parámetros proporcionados por el usuario
                Mi_SQL.Append("SELECT * FROM " + Ope_Pre_Dias_Inhabiles.Tabla_Ope_Pre_Dias_Inhabiles);
                if (!String.IsNullOrEmpty(Datos.P_No_Dia_Inhabil))
                {
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Dias_Inhabiles.Campo_Dia_Inhabil_ID + " = '" + Datos.P_No_Dia_Inhabil + "'");
                }
                if (!String.IsNullOrEmpty(Datos.P_Anio))
                {
                    DateTime.TryParseExact("01/01/" + Datos.P_Anio, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out Fecha_Inicial);
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Dias_Inhabiles.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Fecha_Inicial) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                    DateTime.TryParseExact("12/31/" + Datos.P_Anio, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out Fecha_Final);
                    Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Fecha_Final) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                }
                if (!String.IsNullOrEmpty(Datos.P_Fecha_Consulta))
                {
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Dias_Inhabiles.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Consulta)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                    Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Consulta)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                }
                Mi_SQL.Append(" ORDER BY " + Ope_Pre_Dias_Inhabiles.Campo_Fecha + " DESC ");
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dias
        ///DESCRIPCIÓN: Consulta los dias inhabiles de la direccion de impuestos inmobiliarios
        ///PARAMETROS: 
        ///CREO: jesus toledo
        ///FECHA_CREO: 24/Junio/2011 10:51:24 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Dias(Cls_Ope_Pre_Dias_Inhabiles_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a la base de datos
            try
            {
                Mi_SQL.Append("SELECT " + Ope_Pre_Dias_Inhabiles.Tabla_Ope_Pre_Dias_Inhabiles + ".*, NULL AS DIA, NULL AS MES, NULL AS ANIO ");
                Mi_SQL.Append(" FROM " + Ope_Pre_Dias_Inhabiles.Tabla_Ope_Pre_Dias_Inhabiles + " ");

                if (Datos.P_Anio != null && Datos.P_Anio != "")
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Dias_Inhabiles.Tabla_Ope_Pre_Dias_Inhabiles + "." + Ope_Pre_Dias_Inhabiles.Campo_Fecha + " BETWEEN '" + Datos.P_Fecha_Inicial_Busqueda + "' AND '" + Datos.P_Fecha_Final_Busqueda + "'");

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de los Dias Inhabiles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        internal static DateTime Calcular_Fecha(String P_Fecha, String Dias_Habiles)
        {
            String Dia = "";
            DateTime Fecha_Inicial;
            DateTime Fecha;
            Int32 Cantidad_Dias_Habiles;
            DataTable Dt_Dia_Festivo;
            Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Negocio = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
            try
            {
                Fecha_Inicial = Convert.ToDateTime(P_Fecha);
                Fecha = Fecha_Inicial;

                Cantidad_Dias_Habiles = Convert.ToInt32(Dias_Habiles);
                for (int i = 1; i <= Cantidad_Dias_Habiles; i++)
                {
                    Fecha = Fecha.AddDays(1);
                    Dia = Fecha.ToString("dddd");
                    Dias_Negocio.P_Anio = "3000";
                    Dias_Negocio.P_Fecha_Inicial_Busqueda = Fecha.ToString("dd/MM/yyyy");
                    Dias_Negocio.P_Fecha_Final_Busqueda = Fecha.ToString("dd/MM/yyyy");

                    Dt_Dia_Festivo = Dias_Negocio.Consultar_Dias();

                    if (Dia == "sábado" || Dia == "sabado" || Dia == "domingo" || Dia == "saturday" || Dia == "sunday" || Dt_Dia_Festivo.Rows.Count > 0)
                    {
                        i--;
                    }
                }
                return Fecha;
            }
            catch (Exception Ex)
            {
                throw new Exception("Ocurrio un Error al calcular la Fecha Error: " + Ex.Message);
            }
        }
        #endregion
        #endregion
    }
}