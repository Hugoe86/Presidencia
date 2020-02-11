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
using System.Text;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Almacen_Reporte_Contrarecibos.Negocio;

namespace Presidencia.Almacen_Reporte_Contrarecibos.Datos
{
    public class Cls_Ope_Alm_Rpt_Contrarecibos_Datos
    {
        #region Metodos
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_ContraRecibos
        ///DESCRIPCIÓN:          Método utilizado para consultar los contrarecibos
        ///PARAMETROS:   
        ///CREO:                 Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:           27/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_ContraRecibos(Cls_Ope_Alm_Rpt_Contrarecibos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta SQL
            try
            {
                Mi_SQL.Append("SELECT * ");
                Mi_SQL.Append(" FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores);

                //para la fecha
                Mi_SQL.Append(" where " + Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion + ">='" + Datos.P_Fecha_Inicial + "' " + 
                              " and " + Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion + "<='" + Datos.P_Fecha_Final +"'");

                //para el numero de contra recibo
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los contra recibos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre_Proveedor
        ///DESCRIPCIÓN:          Método utilizado para consultar el nombre del proveedor
        ///PARAMETROS:   
        ///CREO:                 Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:           27/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Nombre_Proveedor(Cls_Ope_Alm_Rpt_Contrarecibos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta SQL
            try
            {
                Mi_SQL.Append("SELECT * ");
                Mi_SQL.Append(" FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores);
                Mi_SQL.Append(" where " + Cat_Com_Proveedores.Campo_Proveedor_ID + "='" + Datos.P_Proveedor_ID + "'");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los contra recibos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Numero_Contra_Recibos
        ///DESCRIPCIÓN:          Método utilizado para consultar El numero de contra recibos
        ///PARAMETROS:   
        ///CREO:                 Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:           14/Enero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Numero_Contra_Recibos(Cls_Ope_Alm_Rpt_Contrarecibos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta SQL
            try
            {
                Mi_SQL.Append("SELECT * ");
                Mi_SQL.Append(" FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores);

                if (!string.IsNullOrEmpty(Datos.P_No_Contra_Recibo))
                {
                    //para el numero de contra recibo
                    Mi_SQL.Append(" where " + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + "='" + Datos.P_No_Contra_Recibo + "'");
                }

                if ((!string.IsNullOrEmpty(Datos.P_No_Contra_Recibo_Inicial)) && (!string.IsNullOrEmpty(Datos.P_No_Contra_Recibo_Final)))
                {
                    //para el rango de numero de contra recibo
                    Mi_SQL.Append(" where " + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno +
                                    ">='" + Datos.P_No_Contra_Recibo_Inicial + "'" +
                                    " And " + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno +
                                    "<='" + Datos.P_No_Contra_Recibo_Final + "'");
                }
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los contra recibos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Documentos_Soporte
        ///DESCRIPCIÓN:          Método utilizado para consultar los documentos relacionados con El numero de contra recibos
        ///PARAMETROS:   
        ///CREO:                 Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:           14/Enero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Documentos_Soporte(Cls_Ope_Alm_Rpt_Contrarecibos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta SQL
            try
            {
                Mi_SQL.Append("SELECT " + Ope_Com_Det_Doc_Soporte.Campo_Documento_ID + "," +
                                Ope_Com_Det_Doc_Soporte.Campo_No_Factura_Interno +
                                ",(Select " + Cat_Com_Documentos.Campo_Nombre + " From " +
                                Cat_Com_Documentos.Tabla_Cat_Com_Documentos + " Where " + Cat_Com_Documentos.Campo_Documento_ID +
                                "=" + Ope_Com_Det_Doc_Soporte.Tabla_Ope_Com_Det_Doc_Soporte + "." +
                                Ope_Com_Det_Doc_Soporte.Campo_Documento_ID + ") Nombre_Documento");
                
                Mi_SQL.Append(" FROM " + Ope_Com_Det_Doc_Soporte.Tabla_Ope_Com_Det_Doc_Soporte);
                //para el numero de contra recibo
                Mi_SQL.Append(" where " + Ope_Com_Det_Doc_Soporte.Campo_No_Factura_Interno + "='" + Datos.P_Documento_ID + "'");
                Mi_SQL.Append(" order by " + Ope_Com_Det_Doc_Soporte.Campo_Documento_ID);
                //para el numero de contra recibo
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los contra recibos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Documentos
        ///DESCRIPCIÓN:          Método utilizado para consultar los documentos relacionados con El numero de contra recibos
        ///PARAMETROS:   
        ///CREO:                 Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:           14/Enero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Documentos(Cls_Ope_Alm_Rpt_Contrarecibos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta SQL
            try
            {

                Mi_SQL.Append("SELECT * ");
                Mi_SQL.Append(" FROM " + Cat_Com_Documentos.Tabla_Cat_Com_Documentos);


                if (!string.IsNullOrEmpty(Datos.P_Documento_ID))
                {
                    Mi_SQL.Append(" where " + Cat_Com_Documentos.Campo_Documento_ID + "='" + Datos.P_Documento_ID + "'");
                }

                Mi_SQL.Append(" order by " + Cat_Com_Documentos.Campo_Documento_ID);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los contra recibos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }
        #endregion
    }
}