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
using Presidencia.Bitacora_Eventos;
using Presidencia.Reportes_Contrarecibos.Negocio;



/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Datos
/// </summary>
/// 

namespace Presidencia.Reportes_Contrarecibos.Datos
{
    public class Cls_Ope_Com_Alm_Rpts_Contrarecibos_Datos
    {
        public Cls_Ope_Com_Alm_Rpts_Contrarecibos_Datos()
        {
        }

        #region Metodos
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Productos
        ///DESCRIPCIÓN:          Método utilizado para consultar los productos
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           06/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Contrarecibos(Cls_Ope_Com_Alm_Rpts_Contrarecibos_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataTable Dt_Consulta = new DataTable();
            Boolean where =true;

        try
        {
            Mi_SQL = " SELECT  F_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " as NO_CONTRARECIBO";
            Mi_SQL = Mi_SQL + ", F_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor + "";
            Mi_SQL = Mi_SQL + ", F_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion + "";
            Mi_SQL = Mi_SQL + ", F_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Total + " as TOTAL_FACTURA";
            Mi_SQL = Mi_SQL + ", O_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio + "";
            Mi_SQL = Mi_SQL + ", O_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Subtotal + " as SUB_TOTAL";
            Mi_SQL = Mi_SQL + ", O_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total_IEPS + "";
            Mi_SQL = Mi_SQL + ", O_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total_IVA + "";
            Mi_SQL = Mi_SQL + ", O_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total + " as TOTAL_OC";
            Mi_SQL = Mi_SQL + ", PROVEEDOR." + Cat_Com_Proveedores.Campo_Compañia+ " AS PROVEEDOR";
            Mi_SQL = Mi_SQL + ", EMPLEADOS." + Cat_Empleados.Campo_Nombre + " ||' ' || EMPLEADOS.";
            Mi_SQL = Mi_SQL  + Cat_Empleados.Campo_Apellido_Paterno + " ||' ' || EMPLEADOS.";
            Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Materno + " as USUARIO_GENERO";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " F_PROVEEDORES, ";
            Mi_SQL = Mi_SQL  + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDOR, ";
            Mi_SQL = Mi_SQL  + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " O_COMPRA, ";
            Mi_SQL = Mi_SQL  + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS ";
            Mi_SQL = Mi_SQL + " WHERE F_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID + " = ";
            Mi_SQL = Mi_SQL + " PROVEEDOR." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
            Mi_SQL = Mi_SQL + " AND F_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + " O_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno+ "";
            Mi_SQL = Mi_SQL + " AND F_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Usuario_Creo + " = ";
            Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + "";

            if (Datos.P_No_ContraRecibo != null)  // CONSULTA EN BASE AL NO. CONTRA RECIBO
            {
                Mi_SQL = Mi_SQL + " AND " + "F_PROVEEDORES.";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = '" + Datos.P_No_ContraRecibo + "'";
            }

            if (Datos.P_Proveedor_ID != null)  // CONSULTA EN BASE AL PROVEEDOR
            {
                Mi_SQL = Mi_SQL + " AND " + "F_PROVEEDORES.";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "'";
            }

            if (Datos.P_Usuario_Genero != null)  // CONSULTA EN BASE AL USUARIO QUE GENERO EL CONTRA RECIBO
            {
                Mi_SQL = Mi_SQL + " AND " + "F_PROVEEDORES.";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_Usuario_Creo + " = '" + Datos.P_Usuario_Genero+ "'";
            }

            if (Datos.P_Fecha_Inicial != null)  // CONSULTA EN BASE A LA FECHA
            {
                Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + "F_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
                " AND '" + Datos.P_Fecha_Final + "'";
            }

            Mi_SQL = Mi_SQL + " ORDER BY F_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;

            if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) 
            {
                 Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
             return Dt_Consulta;
        }
      
        catch (Exception Ex)
        {
            String Mensaje = "Error al intentar consultar los contra recibos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
      }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tablas
        ///DESCRIPCIÓN:          Método utilizado para consultar las Partidas Especificas,
        ///                      Modelos, Marcas y Proveedores
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           05/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Tablas(Cls_Ope_Com_Alm_Rpts_Contrarecibos_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataTable Dt_consulta = new DataTable();

            try
             {
                 if (Datos.P_Nombre_Tabla == "CONTRARECIBOS")
                 {
                     Mi_SQL = " SELECT DISTINCT " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "."+ Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + "";
                     Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores;
                     Mi_SQL = Mi_SQL + ", " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                     Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;
                     Mi_SQL = Mi_SQL + " = " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "."+ Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
                     Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores+"."+ Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;
                 }
                 else if (Datos.P_Nombre_Tabla == "USUARIOS")
                 {
                     Mi_SQL = "SELECT DISTINCT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre;
                     Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno;
                     Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "."+ Cat_Empleados.Campo_Apellido_Materno + " AS EMPLEADO FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                     Mi_SQL = Mi_SQL + ", " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores;
                     Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                     Mi_SQL = Mi_SQL + " = " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_Usuario_Creo;
                     Mi_SQL = Mi_SQL + " ORDER BY EMPLEADO";
                 }
                 else if (Datos.P_Nombre_Tabla == "PROVEEDORES")
                 {
                     Mi_SQL = " SELECT DISTINCT " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", ";
                     Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Compañia + " as PROVEEDOR";
                     Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                     Mi_SQL = Mi_SQL + ", " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores;
                     Mi_SQL = Mi_SQL + " WHERE " +Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                     Mi_SQL = Mi_SQL + " = " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID;
                     Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Compañia;
                 }
                 Dt_consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                 return Dt_consulta;
             }
             catch (Exception Ex)
             {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
             }
        }
        #endregion
    }

}
