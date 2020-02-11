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
using Presidencia.Operacion_Cat_Respuesta_Oficios.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Oracle.DataAccess;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Ope_Cat_Respuesta_Oficios_Datos
/// </summary>
/// 
namespace Presidencia.Operacion_Cat_Respuesta_Oficios.Datos
{
    public class Cls_Ope_Cat_Respuesta_Oficios_Datos
    {        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Oficio
        ///DESCRIPCIÓN: Modifica un Oficio
        ///PARAMENTROS:     
        ///             1. Oficio.         Instancia de la Clase de Negocio de recepcion de oficios
        ///                                 con los datos del que van a ser
        ///                                 modificado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Oficio(Cls_Ope_Cat_Respuesta_Oficios_Negocio Oficio)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            try
            {
                Mi_sql = "UPDATE " + Ope_Cat_Recepcion_Oficios.Tabla_Ope_Cat_Recepcion_Oficios;
                Mi_sql += " SET " + Ope_Cat_Recepcion_Oficios.Campo_Hora_Recepcion + " = '" + Oficio.P_Hora_Recepcion + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Fecha_Respuesta + " = '" + Oficio.P_Fecha_Respuesta.ToString("d-M-yyyy") + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Hora_Respuesta + " = '" + Oficio.P_Hora_Respuesta + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Respuesta + " = '" + Oficio.P_No_Oficio_Respuesta + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Recepcion_Oficios.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio + " = '" + Oficio.P_No_Oficio + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Oficio: [" + E.Message + "].");
            }
            Trans.Commit();
            return Alta;
        }            
    }
}