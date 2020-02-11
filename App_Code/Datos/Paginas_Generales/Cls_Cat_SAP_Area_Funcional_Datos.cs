using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Area_Funcional.Negocio;
using Presidencia.Area_Funcional;
using System.Text;
using Presidencia.Bitacora_Eventos;
namespace Presidencia.Area_Funcional.Datos
{
    public class Cls_Cat_SAP_Area_Funcional_Datos
    {
       #region METODOS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Area_Funcional
        ///DESCRIPCIÓN: Busca un elemento dentro del grid view de acuerdo al nombre del area
        ///PARAMETROS: 1.- Cls_Cat_SAP_Area_Funcional_Negocios
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consulta_Area_Funcional(Cls_Cat_SAP_Area_Funcional_Negocio Area_Funcional)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataSet Ds_Area_Funcional = null;//Listado de Area_Funcional.

            try
            {
                Mi_SQL.Append("SELECT " + Cat_SAP_Area_Funcional.Campo_Clave);
                Mi_SQL.Append(", " + Cat_SAP_Area_Funcional.Campo_Descripcion);
                Mi_SQL.Append(", " + Cat_SAP_Area_Funcional.Campo_Estatus);
                Mi_SQL.Append(", " +Cat_SAP_Area_Funcional.Campo_Anio);
                Mi_SQL.Append(" FROM " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);

                if (Area_Funcional.P_Clave != null)
                {
                    Mi_SQL.Append(" WHERE " + Cat_SAP_Area_Funcional.Campo_Clave + " LIKE '%" + Area_Funcional.P_Clave + "%'");
                }
                Mi_SQL.Append(" ORDER BY " + Cat_SAP_Area_Funcional.Campo_Clave);
                Ds_Area_Funcional = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al invocar Consulta_Area_Funcional. Error: [" + Ex.Message + "]");
            }
            return (Ds_Area_Funcional is DataSet) ? Ds_Area_Funcional : new DataSet();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Area_Funcional
        ///DESCRIPCIÓN:Elimina un giro proveedor en la base de datos
        ///PARAMETROS:  1.- Cls_Cat_SAP_Area_Funcional_Negocios
        ///CREO: Leslie Gonzalez Vazquez
        ///FECHA_CREO: 26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Eliminar_Area_Funcional(Cls_Cat_SAP_Area_Funcional_Negocio Area_Funcional)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.

            try
            {
                Mi_SQL.Append("UPDATE " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);
                Mi_SQL.Append(" SET " + Cat_SAP_Area_Funcional.Campo_Estatus + " = 'INACTIVO'");
                Mi_SQL.Append(" WHERE " + Cat_SAP_Area_Funcional.Campo_Clave + " = '" + Area_Funcional.P_Clave + "'");

                //Mi_SQL.Append("DELETE FROM " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);
                //Mi_SQL.Append(" WHERE " + Cat_SAP_Area_Funcional.Campo_Clave + " = '" + Area_Funcional.P_Clave + "'" );

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar Eliminar_Area_Funcional. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Area_Funcional
        ///DESCRIPCIÓN:Da de alta un Area_Funcional en la base de datos
        ///PARAMETROS:  1.- Cls_Cat_SAP_Area_Funcional_Negocios
        ///CREO: Leslie Gonzalez Vazquez
        ///FECHA_CREO: 26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Area_Funcional(Cls_Cat_SAP_Area_Funcional_Negocio Area_Funcional)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que almacenara la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.

            try
            {
                Mi_SQL.Append("INSERT INTO " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);
                Mi_SQL.Append("(" + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + ", ");
                Mi_SQL.Append(Cat_SAP_Area_Funcional.Campo_Clave + "," + Cat_SAP_Area_Funcional.Campo_Descripcion + ", ");
                Mi_SQL.Append(Cat_SAP_Area_Funcional.Campo_Estatus + ", " + Cat_SAP_Area_Funcional.Campo_Usuario_Creo + ",");
                Mi_SQL.Append(Cat_SAP_Area_Funcional.Campo_Fecha_Creo +", " +Cat_SAP_Area_Funcional.Campo_Anio);
                Mi_SQL.Append(") VALUES ('" + Area_Funcional.P_Area_Funcional_ID + "', '");
                Mi_SQL.Append(Area_Funcional.P_Clave + "', '" + Area_Funcional.P_Descripcion + "', '" + Area_Funcional.P_Estatus + "', '");
                Mi_SQL.Append(Area_Funcional.P_Usuario_Creo + "', SYSDATE, ");
                Mi_SQL.Append(Area_Funcional.P_Anio +")");

                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
                Mi_SQL = Mi_SQL.Replace("'","?");
                Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Cat_SAP_Area_Funcional.aspx", "A.Funcional", Mi_SQL.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Alta_Area_Funcional. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Area_Funcional
        ///DESCRIPCIÓN: Modificar el Area_Funcional en la base de datos
        ///PARAMETROS: 1.- Cls_Cat_SAP_Area_Funcional_Negocios
        ///            2.- Clave Para guardar la referencia de la clave antes de que el usuario hiciera los cambios
        ///CREO: Leslie Gonzalez Vazquez
        ///FECHA_CREO: 26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************/
        public static Boolean Modificar_Area_Funcional(Cls_Cat_SAP_Area_Funcional_Negocio Area_Funcional, String Clave)
        {
            StringBuilder MI_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            String Clave_Anterior = Clave;

            try
            {
                MI_SQL.Append("UPDATE " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);
                MI_SQL.Append(" SET " + Cat_SAP_Area_Funcional.Campo_Clave + " = '" + Area_Funcional.P_Clave + "', ");
                MI_SQL.Append(Cat_SAP_Area_Funcional.Campo_Descripcion + " = '" + Area_Funcional.P_Descripcion + "', ");
                MI_SQL.Append(Cat_SAP_Area_Funcional.Campo_Estatus + " = '" + Area_Funcional.P_Estatus + "', ");
                MI_SQL.Append(Cat_SAP_Area_Funcional.Campo_Usuario_Modifico + " = '" + Area_Funcional.P_Usuario_Modifico + "', ");
                MI_SQL.Append(Cat_SAP_Area_Funcional.Campo_Fecha_Modifico + " = SYSDATE, ");
                MI_SQL.Append(Cat_SAP_Area_Funcional.Campo_Anio +"= " + Area_Funcional.P_Anio);
                MI_SQL.Append(" WHERE " + Cat_SAP_Area_Funcional.Campo_Clave + " = '" + Clave_Anterior + "'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, MI_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar Modificar_Area_Funcional. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consecutivo
        ///DESCRIPCIÓN: Metodo que verfifica el consecutivo en la tabla y ayuda a generar el nuevo Id. 
        ///PARAMETROS: 
        ///CREO:    Leslie González Vázquez
        ///FECHA_CREO: 28/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static String Consecutivo_ID()
        {
            String Consecutivo = "";
            StringBuilder Mi_SQL = new StringBuilder();
            object Area_Funcional_id; //Obtiene el ID con la cual se guardo los datos en la base de datos

            Mi_SQL.Append("SELECT NVL(MAX (" + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + "), '00000')");
            Mi_SQL.Append(" FROM " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);

            Area_Funcional_id = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (Convert.IsDBNull(Area_Funcional_id))
            {
                Consecutivo = "00001";
            }
            else
            {
                Consecutivo = string.Format("{0:00000}", Convert.ToInt32(Area_Funcional_id) + 1);
            }
            return Consecutivo;
        }//fin de consecutivo         

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Area_Funcional_Especial
        ///DESCRIPCIÓN: Consulta las Areas Funcionales asociadas a la Fte de Financiamiento
        ///PARAMETROS:  1.- Cls_Cat_SAP_Area_Funcional_Negocios
        ///CREO: Salvador L. Rea Ayala
        ///FECHA_CREO: 4/Octubre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Area_Funcional_Especial(Cls_Cat_SAP_Area_Funcional_Negocio Datos)
        {
            string Mi_SQL;  //Variable que contendra la Query de consutla.

            try
            {
                Mi_SQL = "SELECT " + Cat_SAP_Area_Funcional.Campo_Clave + " ||' - '|| " + Cat_SAP_Area_Funcional.Campo_Descripcion + " AS CLAVE_NOMBRE, ";
                Mi_SQL += Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID;
                Mi_SQL += " FROM " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional;
                Mi_SQL += " WHERE " + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + " = '" + Datos.P_Area_Funcional_ID + "'";
                Mi_SQL += " ORDER BY " + Cat_SAP_Area_Funcional.Campo_Clave + " ASC";

                //Sentencia que ejecuta el query
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Alta_Area_Funcional. Error: [" + Ex.Message + "]");
            }
        }
       #endregion 
    }
}