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
using Presidencia.Asignar_Password.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
/// <summary>
/// Summary description for Cls_Ope_Psw_Datos
/// </summary>
/// 
namespace Presidencia.Asignar_Password.Datos
{
    public class Cls_Ope_Psw_Datos
    {
        public Cls_Ope_Psw_Datos()
        {
        }
        #region MÉTODOS
        public static DataTable Consultar_Roles()
        {
            String Mi_Sql = "SELECT " + Apl_Cat_Roles.Campo_Rol_ID + "," + Apl_Cat_Roles.Campo_Nombre + 
                " FROM " + 
                Apl_Cat_Roles.Tabla_Apl_Cat_Roles;
            DataTable Dt_Roles = null;
            DataSet Ds_Roles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            if (Ds_Roles.Tables.Count > 0)
                Dt_Roles = Ds_Roles.Tables[0];
            return Dt_Roles;
        }


        public static DataTable Consultar_Dependencias()
        {
            String Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + "," + Cat_Dependencias.Campo_Clave;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Dependencias.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Campo_Estatus;
            Mi_SQL = Mi_SQL + "='ACTIVO'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        }


        public static DataTable Consultar_Empleado(Cls_Ope_Psw_Negocio Negocio)
        {
            String Mi_Sql = "SELECT " +
                Cat_Empleados.Campo_No_Empleado + "," +
                Cat_Empleados.Campo_Rol_ID + "," +
                Cat_Empleados.Campo_Password + "," +
                Cat_Empleados.Campo_Nombre + "||' '||" +
                Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE_EMPLEADO," +
                Cat_Empleados.Campo_Dependencia_ID + "" + 
                " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                " WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + Negocio.P_No_Empleado +"'";
            DataTable Dt_Empleado = null;
            DataSet Ds_Empleado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            if (Ds_Empleado.Tables.Count > 0)
                Dt_Empleado = Ds_Empleado.Tables[0];
            return Dt_Empleado;
        }
        public static int Actualizar_Empleado(Cls_Ope_Psw_Negocio Negocio)
        {
            int Registros_Afectados = 0;
            try
            {
            String Mi_Sql = "UPDATE " + Cat_Empleados.Tabla_Cat_Empleados +
                " SET " + Cat_Empleados.Campo_Rol_ID + " = '" + Negocio.P_Rol_ID + "'," +
                Cat_Empleados.Campo_Password + " = '" + Negocio.P_Password+ "'" +
                " WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + Negocio.P_No_Empleado + "'";
                Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            }
            catch(Exception Ex)
            {
                Ex.ToString();
                Registros_Afectados = 0;
            }
            return Registros_Afectados;
        }

        public static DataTable Consultar_Detalle_UR_Empleado(Cls_Ope_Psw_Negocio Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Det_Empleado_UR.Tabla_Cat_Det_Empleado_UR+"." + Cat_Det_Empleado_UR.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Dependencias.Campo_Clave;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Dependencias.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + "=" + Cat_Det_Empleado_UR.Tabla_Cat_Det_Empleado_UR + "." + Cat_Det_Empleado_UR.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + ") AS DEPENDENCIA FROM " + Cat_Det_Empleado_UR.Tabla_Cat_Det_Empleado_UR;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Det_Empleado_UR.Tabla_Cat_Det_Empleado_UR + "." + Cat_Det_Empleado_UR.Campo_Empleado_ID;
            Mi_SQL = Mi_SQL + "=(SELECT " + Cat_Empleados.Campo_Empleado_ID;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado;
            Mi_SQL = Mi_SQL + "='" + Negocio.P_No_Empleado + "')";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


        public static bool Guardar_Detalle_UR(Cls_Ope_Psw_Negocio Negocio)
        {
            bool Operacion_Realizada = false;
            String Mi_SQL = "";
            //Primero Eliminamos los detalles pertenecientes a este empleado
            Mi_SQL = "DELETE " + Cat_Det_Empleado_UR.Tabla_Cat_Det_Empleado_UR;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Det_Empleado_UR.Campo_Empleado_ID;
            Mi_SQL = Mi_SQL + "=(SELECT " + Cat_Empleados.Campo_Empleado_ID;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado;
            Mi_SQL = Mi_SQL + "='" + Negocio.P_No_Empleado + "')";

            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            //Damos de alta los detalles 
            if(Negocio.P_Dt_Dependencias.Rows.Count>0)
            {

                try
                {
                    for (int i = 0; i < Negocio.P_Dt_Dependencias.Rows.Count; i++)
                    {
                        Mi_SQL = "";
                        Mi_SQL = "INSERT INTO " + Cat_Det_Empleado_UR.Tabla_Cat_Det_Empleado_UR;
                        Mi_SQL = Mi_SQL + " VALUES('" + Negocio.P_Dt_Dependencias.Rows[i]["DEPENDENCIA_ID"].ToString().Trim();
                        Mi_SQL = Mi_SQL + "',(SELECT " + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado;
                        Mi_SQL = Mi_SQL + "='" + Negocio.P_No_Empleado + "'))";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    Operacion_Realizada = true;
                }
                catch(Exception Ex)
                {
                    Operacion_Realizada = false;
                }
            }//Fin del if



            return Operacion_Realizada;
        }
        #endregion

    }
}