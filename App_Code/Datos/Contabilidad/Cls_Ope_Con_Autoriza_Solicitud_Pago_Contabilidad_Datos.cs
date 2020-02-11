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
using Presidencia.Autoriza_Solicitud_Pago_Contabilidad.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Con_Autoriza_Solicitud_Pago_Datos
/// </summary>

namespace Presidencia.Autoriza_Solicitud_Pago_Contabilidad.Datos
{
    public class Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Datos
    {
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Solicitudes_SinAutotizar
        /// DESCRIPCION : Consulta las solicitudes de pago que estan pendientes de autorizar o rechazar 
        /// PARAMETROS  : Datos: Recibe los datos proporcionados por el usuario.
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 15/Noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Solicitudes_SinAutotizar(Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Negocio   Datos)
        {
            String Mi_SQL;  //Almacenara la Query de Consulta.
            try
            {
                //Consulta los movimientos de las cuentas contables.
                Mi_SQL = "SELECT Solicitud." + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + ", Solicitud." + Ope_Con_Solicitud_Pagos.Campo_No_Reserva ;
                Mi_SQL += ", Solicitud."+ Ope_Con_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID +", Solicitud."+ Ope_Con_Solicitud_Pagos.Campo_Concepto ;
                Mi_SQL += ", Solicitud." + Ope_Con_Solicitud_Pagos.Campo_Monto + ", Tipo." + Cat_Con_Tipo_Solicitud_Pagos.Campo_Descripcion + " as Tipo_Pago FROM ";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos+" Solicitud, "+ Cat_Con_Tipo_Solicitud_Pagos.Tabla_Cat_Con_Tipo_Solicitud_Pago +" Tipo ";
                Mi_SQL += " WHERE solicitud." + Ope_Con_Solicitud_Pagos.Campo_Estatus  + " = 'PREAUTORIZADO' AND Solicitud."+ Ope_Con_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID +" = Tipo."+ Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID ;
                Mi_SQL += " ORDER BY " + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago  + " ASC";

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
        /// NOMBRE DE LA FUNCION: Cambiar_Estatus_Solicitud_Pago
            /// DESCRIPCION : Autoriza o rechaza la solicitud de pago 
            /// PARAMETROS  : 
            /// CREO        : Sergio Manuel Gallardo Andrade
            /// FECHA_CREO  : 15/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
        public static void Cambiar_Estatus_Solicitud_Pago(Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Negocio Datos)
            {
                String Mi_SQL;
                try
                {
                    //Da de Alta los datos del Cierre Mensual con los datos proporcionados por el usuario
                    Mi_SQL = "UPDATE " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + " SET ";
                    Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Comentarios_Contabilidad   + "='" + Datos.P_Comentario  + "',";
                    //Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Empleado_ID_Contabilidad  + "='" + Datos.P_Empleado_ID_Contabilidad + "',";
                    Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Estatus + "='" + Datos.P_Estatus + "',";
                    Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "',";
                    Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Fecha_Autorizo_Rechazo_Contabilidad + "= SYSDATE " + " WHERE ";
                    Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + " ='" + Datos.P_No_Solicitud_Pago + "'";
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        /// NOMBRE DE LA FUNCION: Consultar_Solicitud_Pago
        /// DESCRIPCION : Consulta las solicitudes de pago que estan pendientes de autorizar o rechazar 
        /// PARAMETROS  : Datos: Recibe los datos proporcionados por el usuario.
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 15/Noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Solicitud_Pago(Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Negocio Datos)
        {
            String Mi_SQL;  //Almacenara la Query de Consulta.
            try
            {
                //Consulta los movimientos de las cuentas contables.
                Mi_SQL = "SELECT " + Ope_Con_Solicitud_Pagos.Campo_No_Reserva + "," + Ope_Con_Solicitud_Pagos.Campo_No_Poliza + "," + Ope_Con_Solicitud_Pagos.Campo_Tipo_Poliza_ID+", ";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Monto +", "+ Ope_Con_Solicitud_Pagos.Campo_Mes_Ano + " FROM ";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos ;
                Mi_SQL += " WHERE " + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + " = '" + Datos.P_No_Solicitud_Pago + "'";
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
        
    }
}

