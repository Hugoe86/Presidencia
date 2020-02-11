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
using Presidencia.Grupos_Dependencias.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;


/// <summary>
/// Summary description for Cls_Cat_Grupos_Dependencias_Datos
/// </summary>
/// 
namespace Presidencia.Grupos_Dependencias.Datos
{
    public class Cls_Cat_Grupos_Dependencias_Datos
    {
        ///*******************************************************************************
        ///METODOS
        ///*******************************************************************************
        #region Metodos

        public static DataTable Consultar_Grupos_Dependencias(Cls_Cat_Grupos_Dependencias_Negocio Datos_Grupos_Dependencias)
        {
            String Mi_SQL = "SELECT " + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID;
            Mi_SQL = Mi_SQL + "," + Cat_Grupos_Dependencias.Campo_Clave;
            Mi_SQL = Mi_SQL + "," + Cat_Grupos_Dependencias.Campo_Nombre;
            Mi_SQL = Mi_SQL + "," + Cat_Grupos_Dependencias.Campo_Estatus;
            Mi_SQL = Mi_SQL + "," + Cat_Grupos_Dependencias.Campo_Clave + " || ' ' || " + Cat_Grupos_Dependencias.Campo_Nombre + " as Clave_Nombre";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias;

           

            if (Datos_Grupos_Dependencias.P_Clave != null)
            {
                Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Grupos_Dependencias.Campo_Clave + ")";
                Mi_SQL = Mi_SQL + " LIKE UPPER('%" + Datos_Grupos_Dependencias.P_Clave + "%')"; 
            }

            if (Datos_Grupos_Dependencias.P_Grupo_Dependencia_ID != null)
            {
                Mi_SQL = " SELECT * FROM " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID;
                Mi_SQL = Mi_SQL + "='" + Datos_Grupos_Dependencias.P_Grupo_Dependencia_ID + "'";
            }
            Mi_SQL = Mi_SQL + " Order by " + Cat_Grupos_Dependencias.Campo_Clave;

            DataTable Dt_Grupos_Dependencias = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Grupos_Dependencias;
        }

        public static String Alta_Grupo_Dependencia(Cls_Cat_Grupos_Dependencias_Negocio Datos_Grupos_Dependencias)
        {
            String Mensaje ="";
            String Mi_SQL = "";
            String Grupo_Dependencia_ID = Consecutivo(Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID, Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias);
            try
            {
                Mi_SQL = "INSERT INTO " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias;
                Mi_SQL = Mi_SQL + "(" + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID;
                Mi_SQL = Mi_SQL + "," + Cat_Grupos_Dependencias.Campo_Clave;
                Mi_SQL = Mi_SQL + "," + Cat_Grupos_Dependencias.Campo_Nombre;
                Mi_SQL = Mi_SQL + "," + Cat_Grupos_Dependencias.Campo_Comentarios;
                Mi_SQL = Mi_SQL + "," + Cat_Grupos_Dependencias.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + "," + Cat_Grupos_Dependencias.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + "," + Cat_Grupos_Dependencias.Campo_Estatus;
                Mi_SQL = Mi_SQL + ") VALUES ('" + Grupo_Dependencia_ID + "','";
                Mi_SQL = Mi_SQL + Datos_Grupos_Dependencias.P_Clave + "','";
                Mi_SQL = Mi_SQL + Datos_Grupos_Dependencias.P_Nombre + "','";
                Mi_SQL = Mi_SQL + Datos_Grupos_Dependencias.P_Comentarios + "','";
                Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "',SYSDATE, '";
                Mi_SQL = Mi_SQL + Datos_Grupos_Dependencias.P_Estatus + "')";
                //damos de alta 
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Mensaje = "Se creo Satisfactoriamente el Grupo Dependencia con la clave " + Datos_Grupos_Dependencias.P_Clave;
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo dar de Alta el Grupo Dependencia, genero el Error:" + ex.Message;

            }


            return Mensaje;
        }

        public static String Modificar_Grupo_Dependencia(Cls_Cat_Grupos_Dependencias_Negocio Datos_Grupos_Dependencias)
        {
            String Mensaje = "";
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "UPDATE " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias;
                Mi_SQL = Mi_SQL + " SET " + Cat_Grupos_Dependencias.Campo_Clave + "='" + Datos_Grupos_Dependencias.P_Clave + "',";
                Mi_SQL = Mi_SQL + Cat_Grupos_Dependencias.Campo_Nombre + "='" +Datos_Grupos_Dependencias.P_Nombre +"',";
                Mi_SQL = Mi_SQL + Cat_Grupos_Dependencias.Campo_Comentarios + "='" + Datos_Grupos_Dependencias.P_Comentarios + "',";
                Mi_SQL = Mi_SQL + Cat_Grupos_Dependencias.Campo_Usuario_Modifico + "='" + Cls_Sessiones.Nombre_Empleado + "',";
                Mi_SQL = Mi_SQL + Cat_Grupos_Dependencias.Campo_Fecha_Modifico + "=SYSDATE,";
                Mi_SQL = Mi_SQL + Cat_Grupos_Dependencias.Campo_Estatus + "='" + Datos_Grupos_Dependencias.P_Estatus + "' ";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID + "='" + Datos_Grupos_Dependencias.P_Grupo_Dependencia_ID+"'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Mensaje = "Se modifico Satisfactoriamente el Grupo Dependencia con la clave " + Datos_Grupos_Dependencias.P_Clave;
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo Modificar El Grupo Dependencia, genero el Error:" + ex.Message;

            }

            return Mensaje;
        }

        public static String Eliminar_Grupo_Dependencia(Cls_Cat_Grupos_Dependencias_Negocio Datos_Grupos_Dependencias)
        {
            String Mensaje = "";
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "DELETE FROM " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID + "='" + Datos_Grupos_Dependencias.P_Grupo_Dependencia_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Mensaje = "Se Elimino Satisfactoriamente el Grupo Dependencia con la clave " + Datos_Grupos_Dependencias.P_Clave;
            }
            catch (Exception ex)
            {
                Mensaje = "No se pudo Eliminar el Grupo Dependencia";

            }

            return Mensaje;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consecutivo
        ///DESCRIPCIÓN: Metodo que verfifica el consecutivo en la tabla y ayuda a generar el nuevo Id. 
        ///PARAMETROS: 
        ///CREO:
        ///FECHA_CREO: 24/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static String Consecutivo(String ID, String Tabla)
        {
            String Consecutivo = "";
            String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
            Object Asunto_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            Mi_SQL = "SELECT NVL(MAX (" + ID + "),'00000') ";
            Mi_SQL = Mi_SQL + "FROM " + Tabla;
            Asunto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Asunto_ID))
            {
                Consecutivo = "00001";
            }
            else
            {
                Consecutivo = string.Format("{0:00000}", Convert.ToInt32(Asunto_ID) + 1);
            }
            return Consecutivo;
        }//fin de consecutivo


        #endregion


    }
}