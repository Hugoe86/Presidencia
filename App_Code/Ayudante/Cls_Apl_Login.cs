using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;
using Presidencia.Constantes;
/// <summary>
/// Summary description for Cls_Apl_Login
/// </summary>
/// 
namespace Presidencia.Login
{
    public class Cls_Apl_Login
    {
        public Cls_Apl_Login()
        {
        }
        public static bool Validar_Acceso(String Usuario, String Psw)
        {
            bool Acceso = false;                     
            String Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " +
            Cat_Empleados.Tabla_Cat_Empleados + " WHERE " +
            Cat_Empleados.Campo_No_Empleado + " = '" + Usuario + "' AND " +
            Cat_Empleados.Campo_Password + " = '" + Psw + "'";
            try
            {
                DataTable Dt_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                if (Dt_Tabla != null && Dt_Tabla.Rows.Count > 0)
                {
                    Acceso = true;
                }
            }
            catch(Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Acceso;
        }

        public static DataTable Consultar_Datos_Empleado(String No_Empleado)
        {
            DataTable Dt_Tabla = null;
            String Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + ".*, " +
            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " ||' '|| " +
            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| " +
            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " AS EMPLEADO FROM " +
            Cat_Empleados.Tabla_Cat_Empleados + " WHERE " +
            Cat_Empleados.Campo_No_Empleado + " = '" + No_Empleado + "'";
            try
            {
                Dt_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];           
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Dt_Tabla;
        }
    }

}