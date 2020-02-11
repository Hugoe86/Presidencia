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
using Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Rpt_Pat_Listado_Bienes_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Datos {

    public class Cls_Rpt_Pat_Listado_Bienes_Datos{

        #region Metodos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Registros_Bienes_Muebles_Cuenta_Publica
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable
            ///                       de los registros de Bienes Muebles.
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Mayo/2011 
            ///MODIFICO             : Francisco Antonio Gallardo Castañeda
            ///FECHA_MODIFICO       : 14/Diciembre/2011 
            ///CAUSA_MODIFICACIÓN   : Se actualizo la consulta con los filtros de reales
            ///*******************************************************************************
            public static DataTable Obtener_Registros_Bienes_Muebles_Cuenta_Publica(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Bienes_Muebles = null;
                DataTable Dt_Bienes_Muebles = new DataTable();
                Boolean Entro_Where = false;
                try {
                    if (Parametros.P_Tipo.Trim().Equals("ALTAS") || Parametros.P_Tipo.Trim().Equals("TODAS")) {
                        Mi_SQL = "SELECT 'ALTA' AS MOVIMIENTO";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + ", 'DD/MM/YYYY') AS FECHA";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ") AS CANTIDAD";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS TIPO_BIEN";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS NUMERO_INVENTARIO";
                        Mi_SQL = Mi_SQL + " , ( TRIM(NVL(" + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + ", '-'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(" + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + ", '-'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + ", 'S/S'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(" + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + ", '-'))) AS CARACTERISTICAS";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS CONDICIONES";
                        Mi_SQL = Mi_SQL + " , NVL(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " , NVL(" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + " , '' AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + ", 0) AS IMPORTE";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + ", 'S/F') AS NO_FACTURA";
                        Mi_SQL = Mi_SQL + ", TRIM(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Observadores + ") AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " AS OPERACION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                        if (Parametros.P_Busqueda_Nombre_Empleado != null && Parametros.P_Busqueda_Nombre_Empleado.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + " TRIM(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") = '" + Parametros.P_Busqueda_Nombre_Empleado.Trim() + "'";
                        }
                    } if (Parametros.P_Tipo.Trim().Equals("BAJAS") || Parametros.P_Tipo.Trim().Equals("TODAS")) {
                        Entro_Where = false;
                        if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                            Mi_SQL += " UNION ";
                            Mi_SQL += "SELECT 'BAJA' AS MOVIMIENTO";
                        } else {
                            Mi_SQL = "SELECT 'BAJA' AS MOVIMIENTO";
                        }
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + ", 'DD/MM/YYYY') AS FECHA";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ") AS CANTIDAD";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS TIPO_BIEN";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS NUMERO_INVENTARIO";
                        Mi_SQL = Mi_SQL + " , ( TRIM(NVL(" + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + ", '-'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(" + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + ", '-'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + ", 'S/S'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(" + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + ", '-'))) AS CARACTERISTICAS";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS CONDICIONES";
                        Mi_SQL = Mi_SQL + " , NVL(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " , NVL(" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + " , '' AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS IMPORTE";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + ", 'S/F') AS NO_FACTURA";
                        Mi_SQL = Mi_SQL + ", '' AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " AS OPERACION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                        if (Parametros.P_Busqueda_Nombre_Empleado != null && Parametros.P_Busqueda_Nombre_Empleado.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + " TRIM(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") = '" + Parametros.P_Busqueda_Nombre_Empleado.Trim() + "'";
                        }
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = 'DEFINITIVA'";
                    }
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Bienes_Muebles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Bienes_Muebles != null) {
                        Dt_Bienes_Muebles = Ds_Bienes_Muebles.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Bienes_Muebles;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Registros_Vehiculos_Cuenta_Publica
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable
            ///                       de los registros de Vehiculos.
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Mayo/2011 
            ///MODIFICO             : Francisco Antonio Gallardo Castañeda
            ///FECHA_MODIFICO       : 14/Diciembre/2011 
            ///CAUSA_MODIFICACIÓN   : Se actualizo la consulta con los filtros de reales
            ///*******************************************************************************
            public static DataTable Obtener_Registros_Vehiculos_Cuenta_Publica(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Vehiculos = null;
                DataTable Dt_Vehiculos = new DataTable();
                Boolean Entro_Where = false;
                try {
                    if (Parametros.P_Tipo.Trim().Equals("ALTAS") || Parametros.P_Tipo.Trim().Equals("TODAS")) {
                        Mi_SQL = "SELECT 'ALTA' AS MOVIMIENTO";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + ", 'DD/MM/YYYY') AS FECHA";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Cantidad + ") AS CANTIDAD";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre + " AS TIPO_BIEN";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " AS NUMERO_INVENTARIO";
                        Mi_SQL = Mi_SQL + " , ( NVL(" + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + ", 'S/S')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + ", '-')) AS CARACTERISTICAS";
                        Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + "";
                        Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'DADO DE BAJA', 'TEMPORAL', 'DADO DE BAJA', 'VIGENTE', 'BUENO') AS CONDICIONES";
                        Mi_SQL = Mi_SQL + " , NVL(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " , NVL(" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + " , '' AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Costo_Actual + ", 0) AS IMPORTE";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_No_Factura + ", 'S/F') AS NO_FACTURA";
                        Mi_SQL = Mi_SQL + ", TRIM(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Observaciones + ") AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                    } if (Parametros.P_Tipo.Trim().Equals("BAJAS") || Parametros.P_Tipo.Trim().Equals("TODAS")) {
                        Entro_Where = false;
                        if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                            Mi_SQL += " UNION ";
                            Mi_SQL += "SELECT 'BAJA' AS MOVIMIENTO";
                        } else {
                            Mi_SQL = "SELECT 'BAJA' AS MOVIMIENTO";
                        }
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Modifico + ", 'DD/MM/YYYY') AS FECHA";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Cantidad + ") AS CANTIDAD";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre + " AS TIPO_BIEN";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " AS NUMERO_INVENTARIO";
                        Mi_SQL = Mi_SQL + " , ( NVL(" + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + ", 'S/S')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + ", '-')) AS CARACTERISTICAS";
                        Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + "";
                        Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'DADO DE BAJA', 'TEMPORAL', 'DADO DE BAJA', 'VIGENTE', 'BUENO') AS CONDICIONES";
                        Mi_SQL = Mi_SQL + " , NVL(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " , NVL(" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + " , '' AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Costo_Actual + ", 0) AS IMPORTE";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_No_Factura + ", 'S/F') AS NO_FACTURA";
                        Mi_SQL = Mi_SQL + ", '' AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Modifico + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + " = 'DEFINITIVA'";
                    }
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) { 
                        Ds_Vehiculos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Vehiculos != null) {
                        Dt_Vehiculos = Ds_Vehiculos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Vehiculos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Registros_Animales_Cuenta_Publica
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable
            ///                       de los registros de Cemovientes.
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Mayo/2011
            ///MODIFICO             : Francisco Antonio Gallardo Castañeda
            ///FECHA_MODIFICO       : 15/Diciembre/2011 
            ///CAUSA_MODIFICACIÓN   : Se actualizo la consulta con los filtros de reales
            ///*******************************************************************************
            public static DataTable Obtener_Registros_Animales_Cuenta_Publica(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Cemovientes = null;
                DataTable Dt_Cemovientes = new DataTable();
                Boolean Entro_Where = false;
                try {
                   if (Parametros.P_Tipo.Trim().Equals("ALTAS") || Parametros.P_Tipo.Trim().Equals("TODAS")) {
                        Mi_SQL = "SELECT 'ALTA' AS MOVIMIENTO";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + ", 'DD/MM/YYYY') AS FECHA";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cantidad + ") AS CANTIDAD";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " AS TIPO_BIEN";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " AS NUMERO_INVENTARIO";
                        Mi_SQL = Mi_SQL + " , ( NVL(" + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Nombre + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Sexo + ", 'S/S')";
                        Mi_SQL = Mi_SQL + " ||', ASCENDENCIA '|| NVL(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia + ", '-')) AS CARACTERISTICAS";
                        Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Estatus + "";
                        Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'DADO DE BAJA', 'TEMPORAL', 'DADO DE BAJA', 'VIGENTE', 'BUENO') AS CONDICIONES";
                        Mi_SQL = Mi_SQL + " , NVL(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " , NVL(" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + " , '' AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Costo_Actual + ", 0) AS IMPORTE";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Factura + ", 'S/F') AS NO_FACTURA";
                        Mi_SQL = Mi_SQL + ", TRIM(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Observaciones + ") AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Raza_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Raza_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                    } if (Parametros.P_Tipo.Trim().Equals("BAJAS") || Parametros.P_Tipo.Trim().Equals("TODAS")) {
                        Entro_Where = false;
                        if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                            Mi_SQL += " UNION ";
                            Mi_SQL += "SELECT 'BAJA' AS MOVIMIENTO";
                        } else {
                            Mi_SQL = "SELECT 'BAJA' AS MOVIMIENTO";
                        }
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Modifico + ", 'DD/MM/YYYY') AS FECHA";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cantidad + ") AS CANTIDAD";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " AS TIPO_BIEN";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " AS NUMERO_INVENTARIO";
                        Mi_SQL = Mi_SQL + " , ( NVL(" + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Nombre + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Sexo + ", 'S/S')";
                        Mi_SQL = Mi_SQL + " ||', ASCENDENCIA '|| NVL(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia + ", '-')) AS CARACTERISTICAS";
                        Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Estatus + "";
                        Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'DADO DE BAJA', 'TEMPORAL', 'DADO DE BAJA', 'VIGENTE', 'BUENO') AS CONDICIONES";
                        Mi_SQL = Mi_SQL + " , NVL(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " , NVL(" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + " , '' AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Costo_Actual + ", 0) AS IMPORTE";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Factura + ", 'S/F') AS NO_FACTURA";
                        Mi_SQL = Mi_SQL + ", '' AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Raza_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Raza_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Modifico + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Estatus + " = 'DEFINITIVA'";
                    }
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) { 
                        Ds_Cemovientes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Cemovientes != null) {
                        Dt_Cemovientes = Ds_Cemovientes.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Cemovientes;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Bienes_Muebles
            ///DESCRIPCIÓN          : Consulta los Bienes Muebles [Datos Generales].
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 02/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Bienes_Muebles(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Bienes_Muebles = null;
                DataTable Dt_Bienes_Muebles = new DataTable();
                try {
                    if (Parametros.P_Operacion.Trim().Equals("RESGUARDO") || Parametros.P_Operacion.Trim().Equals("TODOS")) { 
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS CLAVE_BIEN";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS INVENTARIO_ANTERIOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " AS INVENTARIO_SIAS";
                        //Mi_SQL = Mi_SQL + ", (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                        //Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS UNIDAD_RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", '' AS UNIDAD_RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " AS MODELO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS DESCRIPCION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " AS SERIE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Descripcion + " AS ZONA";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + " AS MATERIAL";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " AS COLOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " AS FECHA_ADQUISICION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " AS FACTURA";
                        Mi_SQL = Mi_SQL + "," + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS VALOR_INCIAL";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS VALOR_ACTUAL";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Observadores + " AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + "";
                        Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS ESTADO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " AS OPERACION";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Modifico + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") AS USUARIO_MODIFICO";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + ") AS FECHA_MODIFICO";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Zona_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RESGUARDO'";
                        if (Parametros.P_No_Inventario_Anterior != null && Parametros.P_No_Inventario_Anterior.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " LIKE '%" + Parametros.P_No_Inventario_Anterior + "%'"; ;
                        }
                        if (Parametros.P_No_Inventario_SIAS != null && Parametros.P_No_Inventario_SIAS.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " = '" + Parametros.P_No_Inventario_SIAS + "'"; ;
                        }
                        if (Parametros.P_Nombre_Producto != null && Parametros.P_Nombre_Producto.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " LIKE '%" + Parametros.P_Nombre_Producto + "%'";
                        }
                        if (Parametros.P_Clasificacion_ID != null && Parametros.P_Clasificacion_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID + " = '" + Parametros.P_Clasificacion_ID + "'";
                        }
                        if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                            if (Parametros.P_Estatus_Resguardante != null && Parametros.P_Estatus_Resguardante.Trim().Length > 0) {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = '" + Parametros.P_Estatus_Resguardante + "'";
                            }
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                            Mi_SQL = Mi_SQL +" ) )";
                        }
                        if (Parametros.P_Modelo != null && Parametros.P_Modelo.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " LIKE '%" + Parametros.P_Modelo + "%'";
                        }
                        if (Parametros.P_Marca_ID != null && Parametros.P_Marca_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = '" + Parametros.P_Marca_ID + "'";
                        }
                        if (Parametros.P_Material_ID != null && Parametros.P_Material_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = '" + Parametros.P_Material_ID + "'";
                        }
                        if (Parametros.P_Color_ID != null && Parametros.P_Color_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = '" + Parametros.P_Color_ID + "'";
                        }
                        if (Parametros.P_Zona_ID != null && Parametros.P_Zona_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + " = '" + Parametros.P_Zona_ID + "'";
                        }
                        if (Parametros.P_Factura != null && Parametros.P_Factura.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " = '" + Parametros.P_Factura + "'";
                        }
                        if (Parametros.P_Serie != null && Parametros.P_Serie.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " LIKE '%" + Parametros.P_Serie + "%'";
                        }
                        if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                        }
                        if (Parametros.P_Estado != null && Parametros.P_Estado.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " = '" + Parametros.P_Estado + "'";
                        }
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Proveedor != null && Parametros.P_Proveedor.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = '" + Parametros.P_Proveedor + "'";
                        }
                        if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                            if (Parametros.P_Estatus_Resguardante != null && Parametros.P_Estatus_Resguardante.Trim().Length > 0) {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = '" + Parametros.P_Estatus_Resguardante + "'";
                            }
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Parametros.P_Resguardante_ID + "')";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Adquisicion_Final).AddDays(1).Date) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                    }
                    if (Parametros.P_Operacion.Trim().Equals("TODOS")) { Mi_SQL = Mi_SQL + " UNION "; }
                    if (Parametros.P_Operacion.Trim().Equals("RECIBO") || Parametros.P_Operacion.Trim().Equals("TODOS")) { 
                        Mi_SQL = Mi_SQL + " SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS CLAVE_BIEN";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS INVENTARIO_ANTERIOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " AS INVENTARIO_SIAS";
                        Mi_SQL = Mi_SQL + ", (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                        Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS UNIDAD_RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " AS MODELO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS DESCRIPCION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " AS SERIE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Descripcion + " AS ZONA";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + " AS MATERIAL";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " AS COLOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " AS FECHA_ADQUISICION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " AS FACTURA";
                        Mi_SQL = Mi_SQL + "," + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS VALOR_INCIAL";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS VALOR_ACTUAL";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Observadores + " AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + "";
                        Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS ESTADO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " AS OPERACION";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Modifico + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") AS USUARIO_MODIFICO";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + ") AS FECHA_MODIFICO";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Zona_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RECIBO'";
                        if (Parametros.P_No_Inventario_Anterior != null && Parametros.P_No_Inventario_Anterior.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " LIKE '%" + Parametros.P_No_Inventario_Anterior + "%'"; ;
                        }
                        if (Parametros.P_No_Inventario_SIAS != null && Parametros.P_No_Inventario_SIAS.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " = '" + Parametros.P_No_Inventario_SIAS+ "'"; ;
                        }
                        if (Parametros.P_Nombre_Producto != null && Parametros.P_Nombre_Producto.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " LIKE '%" + Parametros.P_Nombre_Producto + "%'";
                        }
                        if (Parametros.P_Clasificacion_ID != null && Parametros.P_Clasificacion_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID + " = '" + Parametros.P_Clasificacion_ID + "'";
                        }
                        if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                            if (Parametros.P_Estatus_Resguardante != null && Parametros.P_Estatus_Resguardante.Trim().Length > 0) {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = '" + Parametros.P_Estatus_Resguardante + "'";
                            }
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                            Mi_SQL = Mi_SQL + " ) )";
                        }
                        if (Parametros.P_Modelo != null && Parametros.P_Modelo.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " LIKE '%" + Parametros.P_Modelo + "%'";
                        }
                        if (Parametros.P_Marca_ID != null && Parametros.P_Marca_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = '" + Parametros.P_Marca_ID + "'";
                        }
                        if (Parametros.P_Material_ID != null && Parametros.P_Material_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = '" + Parametros.P_Material_ID + "'";
                        }
                        if (Parametros.P_Color_ID != null && Parametros.P_Color_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = '" + Parametros.P_Color_ID + "'";
                        }
                        if (Parametros.P_Zona_ID != null && Parametros.P_Zona_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + " = '" + Parametros.P_Zona_ID + "'";
                        }
                        if (Parametros.P_Factura != null && Parametros.P_Factura.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " = '" + Parametros.P_Factura + "'";
                        }
                        if (Parametros.P_Serie != null && Parametros.P_Serie.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " LIKE '%" + Parametros.P_Serie + "%'";
                        }
                        if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                        }
                        if (Parametros.P_Estado != null && Parametros.P_Estado.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " = '" + Parametros.P_Estado + "'";
                        }
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Proveedor != null && Parametros.P_Proveedor.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = '" + Parametros.P_Proveedor + "'";
                        }
                        if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                            if (Parametros.P_Estatus_Resguardante != null && Parametros.P_Estatus_Resguardante.Trim().Length > 0) {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = '" + Parametros.P_Estatus_Resguardante + "'";
                            }
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + " = '" + Parametros.P_Resguardante_ID + "')";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Adquisicion_Final).AddDays(1).Date) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }

                    }
                    Ds_Bienes_Muebles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Bienes_Muebles != null) {
                        Dt_Bienes_Muebles = Ds_Bienes_Muebles.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Bienes_Muebles;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Bienes_Muebles
            ///DESCRIPCIÓN          : Consulta los Bienes Muebles [Datos Generales].
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 02/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Bienes_Muebles_Completo(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Bienes_Muebles = null;
                DataTable Dt_Bienes_Muebles = new DataTable();
                try {
                    if (Parametros.P_Operacion.Trim().Equals("RESGUARDO") || Parametros.P_Operacion.Trim().Equals("TODOS")) { 
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS CLAVE_BIEN";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS INVENTARIO_ANTERIOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " AS INVENTARIO_SIAS";
                        Mi_SQL = Mi_SQL + ", DECODE(TRIM(" + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ")";
                        Mi_SQL = Mi_SQL + ", 'VIGENTE', (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ")";
                        Mi_SQL = Mi_SQL + ", 'BAJA', '') UNIDAD_RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " AS MODELO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS DESCRIPCION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " AS SERIE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Descripcion + " AS ZONA";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + " AS MATERIAL";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " AS COLOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " AS FECHA_ADQUISICION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " AS FACTURA";
                        Mi_SQL = Mi_SQL + "," + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS VALOR_INCIAL";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS VALOR_ACTUAL";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Observadores + " AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + "";
                        Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS ESTADO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " AS OPERACION";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Modifico + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") AS USUARIO_MODIFICO";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + ") AS FECHA_MODIFICO";
                        Mi_SQL = Mi_SQL + ", (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "";
                        Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " AS ESTATUS_RESGUARDO";

                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Zona_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + "";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                        
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RESGUARDO'";
                        
                        if (Parametros.P_No_Inventario_Anterior != null && Parametros.P_No_Inventario_Anterior.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " LIKE '%" + Parametros.P_No_Inventario_Anterior + "%'"; ;
                        }
                        if (Parametros.P_No_Inventario_SIAS != null && Parametros.P_No_Inventario_SIAS.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " = '" + Parametros.P_No_Inventario_SIAS + "'"; ;
                        }
                        if (Parametros.P_Nombre_Producto != null && Parametros.P_Nombre_Producto.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " LIKE '%" + Parametros.P_Nombre_Producto + "%'";
                        }
                        if (Parametros.P_Clasificacion_ID != null && Parametros.P_Clasificacion_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID + " = '" + Parametros.P_Clasificacion_ID + "'";
                        }
                        if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " IN ('" + Parametros.P_Dependencia_ID + "')";
                        }
                        if (Parametros.P_Modelo != null && Parametros.P_Modelo.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " LIKE '%" + Parametros.P_Modelo + "%'";
                        }
                        if (Parametros.P_Marca_ID != null && Parametros.P_Marca_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = '" + Parametros.P_Marca_ID + "'";
                        }
                        if (Parametros.P_Material_ID != null && Parametros.P_Material_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = '" + Parametros.P_Material_ID + "'";
                        }
                        if (Parametros.P_Color_ID != null && Parametros.P_Color_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = '" + Parametros.P_Color_ID + "'";
                        }
                        if (Parametros.P_Zona_ID != null && Parametros.P_Zona_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + " = '" + Parametros.P_Zona_ID + "'";
                        }
                        if (Parametros.P_Factura != null && Parametros.P_Factura.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " = '" + Parametros.P_Factura + "'";
                        }
                        if (Parametros.P_Serie != null && Parametros.P_Serie.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " LIKE '%" + Parametros.P_Serie + "%'";
                        }
                        if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                        }
                        if (Parametros.P_Estatus_Resguardante != null && Parametros.P_Estatus_Resguardante.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = '" + Parametros.P_Estatus_Resguardante + "'";
                        }
                        if (Parametros.P_Estado != null && Parametros.P_Estado.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " = '" + Parametros.P_Estado + "'";
                        }
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Proveedor != null && Parametros.P_Proveedor.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = '" + Parametros.P_Proveedor + "'";
                        }
                        if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " IN ('" + Parametros.P_Resguardante_ID + "')";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Adquisicion_Final).AddDays(1).Date) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                    }
                    if (Parametros.P_Operacion.Trim().Equals("TODOS")) { Mi_SQL = Mi_SQL + " UNION "; }
                    if (Parametros.P_Operacion.Trim().Equals("RECIBO") || Parametros.P_Operacion.Trim().Equals("TODOS")) { 
                        Mi_SQL = Mi_SQL + " SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS CLAVE_BIEN";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS INVENTARIO_ANTERIOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " AS INVENTARIO_SIAS";
                        Mi_SQL = Mi_SQL + ", DECODE(TRIM(" + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Estatus + ")";
                        Mi_SQL = Mi_SQL + ", 'VIGENTE', (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ")";
                        Mi_SQL = Mi_SQL + ", 'BAJA', '') UNIDAD_RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " AS MODELO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS DESCRIPCION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " AS SERIE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Descripcion + " AS ZONA";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + " AS MATERIAL";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " AS COLOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " AS FECHA_ADQUISICION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " AS FACTURA";
                        Mi_SQL = Mi_SQL + "," + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS VALOR_INCIAL";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS VALOR_ACTUAL";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Observadores + " AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + "";
                        Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS ESTADO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " AS OPERACION";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Modifico + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") AS USUARIO_MODIFICO";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + ") AS FECHA_MODIFICO";
                        Mi_SQL = Mi_SQL + ", (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "";
                        Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Estatus + " AS ESTATUS_RESGUARDO";

                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Zona_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + "";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";

                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RECIBO'";
                        
                        if (Parametros.P_No_Inventario_Anterior != null && Parametros.P_No_Inventario_Anterior.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " LIKE '%" + Parametros.P_No_Inventario_Anterior + "%'"; ;
                        }
                        if (Parametros.P_No_Inventario_SIAS != null && Parametros.P_No_Inventario_SIAS.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " = '" + Parametros.P_No_Inventario_SIAS+ "'"; ;
                        }
                        if (Parametros.P_Nombre_Producto != null && Parametros.P_Nombre_Producto.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " LIKE '%" + Parametros.P_Nombre_Producto + "%'";
                        }
                        if (Parametros.P_Clasificacion_ID != null && Parametros.P_Clasificacion_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID + " = '" + Parametros.P_Clasificacion_ID + "'";
                        }
                        if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " IN ('" + Parametros.P_Dependencia_ID + "')";
                        }
                        if (Parametros.P_Modelo != null && Parametros.P_Modelo.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " LIKE '%" + Parametros.P_Modelo + "%'";
                        }
                        if (Parametros.P_Marca_ID != null && Parametros.P_Marca_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = '" + Parametros.P_Marca_ID + "'";
                        }
                        if (Parametros.P_Material_ID != null && Parametros.P_Material_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = '" + Parametros.P_Material_ID + "'";
                        }
                        if (Parametros.P_Color_ID != null && Parametros.P_Color_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = '" + Parametros.P_Color_ID + "'";
                        }
                        if (Parametros.P_Zona_ID != null && Parametros.P_Zona_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + " = '" + Parametros.P_Zona_ID + "'";
                        }
                        if (Parametros.P_Factura != null && Parametros.P_Factura.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " = '" + Parametros.P_Factura + "'";
                        }
                        if (Parametros.P_Serie != null && Parametros.P_Serie.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " LIKE '%" + Parametros.P_Serie + "%'";
                        }
                        if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                        }
                        if (Parametros.P_Estatus_Resguardante != null && Parametros.P_Estatus_Resguardante.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = '" + Parametros.P_Estatus_Resguardante + "'";
                        }
                        if (Parametros.P_Estado != null && Parametros.P_Estado.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " = '" + Parametros.P_Estado + "'";
                        }
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Proveedor != null && Parametros.P_Proveedor.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = '" + Parametros.P_Proveedor + "'";
                        }
                        if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " IN ('" + Parametros.P_Resguardante_ID + "')";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Adquisicion_Final).AddDays(1).Date) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                    }
                    Ds_Bienes_Muebles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Bienes_Muebles != null) {
                        Dt_Bienes_Muebles = Ds_Bienes_Muebles.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Bienes_Muebles;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Bienes_Muebles
            ///DESCRIPCIÓN          : Consulta los Bienes Muebles [Datos Generales].
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 02/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static String Consultar_Query_Bienes_Muebles(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                try {
                    if (Parametros.P_Operacion.Trim().Equals("RESGUARDO") || Parametros.P_Operacion.Trim().Equals("TODOS")) { 
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Zona_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RESGUARDO'";
                        if (Parametros.P_No_Inventario_Anterior != null && Parametros.P_No_Inventario_Anterior.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " LIKE '%" + Parametros.P_No_Inventario_Anterior + "%'"; ;
                        }
                        if (Parametros.P_No_Inventario_SIAS != null && Parametros.P_No_Inventario_SIAS.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " = '" + Parametros.P_No_Inventario_SIAS + "'"; ;
                        }
                        if (Parametros.P_Nombre_Producto != null && Parametros.P_Nombre_Producto.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " LIKE '%" + Parametros.P_Nombre_Producto + "%'";
                        }
                        if (Parametros.P_Clasificacion_ID != null && Parametros.P_Clasificacion_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID + " = '" + Parametros.P_Clasificacion_ID + "'";
                        }
                        if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                            if (Parametros.P_Estatus_Resguardante != null && Parametros.P_Estatus_Resguardante.Trim().Length > 0) {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = '" + Parametros.P_Estatus_Resguardante + "'";
                            }
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                            Mi_SQL = Mi_SQL +" ) )";
                        }
                        if (Parametros.P_Modelo != null && Parametros.P_Modelo.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " LIKE '%" + Parametros.P_Modelo + "%'";
                        }
                        if (Parametros.P_Marca_ID != null && Parametros.P_Marca_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = '" + Parametros.P_Marca_ID + "'";
                        }
                        if (Parametros.P_Material_ID != null && Parametros.P_Material_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = '" + Parametros.P_Material_ID + "'";
                        }
                        if (Parametros.P_Color_ID != null && Parametros.P_Color_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = '" + Parametros.P_Color_ID + "'";
                        }
                        if (Parametros.P_Zona_ID != null && Parametros.P_Zona_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + " = '" + Parametros.P_Zona_ID + "'";
                        }
                        if (Parametros.P_Factura != null && Parametros.P_Factura.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " = '" + Parametros.P_Factura + "'";
                        }
                        if (Parametros.P_Serie != null && Parametros.P_Serie.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " LIKE '%" + Parametros.P_Serie + "%'";
                        }
                        if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                        }
                        if (Parametros.P_Estado != null && Parametros.P_Estado.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " = '" + Parametros.P_Estado + "'";
                        }
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Proveedor != null && Parametros.P_Proveedor.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = '" + Parametros.P_Proveedor + "'";
                        }
                        if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                            if (Parametros.P_Estatus_Resguardante != null && Parametros.P_Estatus_Resguardante.Trim().Length > 0) {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = '" + Parametros.P_Estatus_Resguardante + "'";
                            }
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Parametros.P_Resguardante_ID + "')";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Adquisicion_Final).AddDays(1).Date) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                    }
                    if (Parametros.P_Operacion.Trim().Equals("TODOS")) { Mi_SQL = Mi_SQL + " UNION "; }
                    if (Parametros.P_Operacion.Trim().Equals("RECIBO") || Parametros.P_Operacion.Trim().Equals("TODOS")) { 
                        Mi_SQL = Mi_SQL + " SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Zona_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RECIBO'";
                        if (Parametros.P_No_Inventario_Anterior != null && Parametros.P_No_Inventario_Anterior.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " LIKE '%" + Parametros.P_No_Inventario_Anterior + "%'"; ;
                        }
                        if (Parametros.P_No_Inventario_SIAS != null && Parametros.P_No_Inventario_SIAS.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " = '" + Parametros.P_No_Inventario_SIAS+ "'"; ;
                        }
                        if (Parametros.P_Nombre_Producto != null && Parametros.P_Nombre_Producto.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " LIKE '%" + Parametros.P_Nombre_Producto + "%'";
                        }
                        if (Parametros.P_Clasificacion_ID != null && Parametros.P_Clasificacion_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID + " = '" + Parametros.P_Clasificacion_ID + "'";
                        }
                        if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                            if (Parametros.P_Estatus_Resguardante != null && Parametros.P_Estatus_Resguardante.Trim().Length > 0) {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = '" + Parametros.P_Estatus_Resguardante + "'";
                            }
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                            Mi_SQL = Mi_SQL + " ) )";
                        }
                        if (Parametros.P_Modelo != null && Parametros.P_Modelo.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " LIKE '%" + Parametros.P_Modelo + "%'";
                        }
                        if (Parametros.P_Marca_ID != null && Parametros.P_Marca_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = '" + Parametros.P_Marca_ID + "'";
                        }
                        if (Parametros.P_Material_ID != null && Parametros.P_Material_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = '" + Parametros.P_Material_ID + "'";
                        }
                        if (Parametros.P_Color_ID != null && Parametros.P_Color_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = '" + Parametros.P_Color_ID + "'";
                        }
                        if (Parametros.P_Zona_ID != null && Parametros.P_Zona_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + " = '" + Parametros.P_Zona_ID + "'";
                        }
                        if (Parametros.P_Factura != null && Parametros.P_Factura.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " = '" + Parametros.P_Factura + "'";
                        }
                        if (Parametros.P_Serie != null && Parametros.P_Serie.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " LIKE '%" + Parametros.P_Serie + "%'";
                        }
                        if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                        }
                        if (Parametros.P_Estado != null && Parametros.P_Estado.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " = '" + Parametros.P_Estado + "'";
                        }
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Proveedor != null && Parametros.P_Proveedor.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = '" + Parametros.P_Proveedor + "'";
                        }
                        if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                            if (Parametros.P_Estatus_Resguardante != null && Parametros.P_Estatus_Resguardante.Trim().Length > 0) {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = '" + Parametros.P_Estatus_Resguardante + "'";
                            }
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + " = '" + Parametros.P_Resguardante_ID + "')";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Adquisicion_Final).AddDays(1).Date) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Mi_SQL;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Bienes_Animales
            ///DESCRIPCIÓN          : Consulta los Animales [Datos Generales].
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 05/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Animales(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Animales = null;
                DataTable Dt_Animales = new DataTable();
                Boolean Entro_Where = false;
                try {
                    Mi_SQL = "SELECT " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS CLAVE_BIEN";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior + " AS INVENTARIO_ANTERIOR";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Numero_Inventario + " AS INVENTARIO_SIAS";
                    Mi_SQL = Mi_SQL + ", (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                    Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS UNIDAD_RESPONSABLE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre + " AS NOMBRE_ANIMAL";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " AS TIPO_ANIMAL";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Nombre + " AS RAZA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Adiestramiento.Tabla_Cat_Pat_Tipos_Adiestramiento + "." + Cat_Pat_Tipos_Adiestramiento.Campo_Nombre + " AS TIPO_ADIESTRAMIENTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Funciones.Tabla_Cat_Pat_Funciones + "." + Cat_Pat_Funciones.Campo_Nombre + " AS FUNCION";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Alimentacion.Tabla_Cat_Pat_Tipos_Alimentacion + "." + Cat_Pat_Tipos_Alimentacion.Campo_Nombre + " AS TIPO_ALIMENTACION";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " AS COLOR";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Nacimiento + " AS FECHA_NACIMIENTO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " AS FECHA_ADQUISICION";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Sexo + " AS SEXO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia + " AS TIPO_ASCENDENCIA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Costo_Actual + " AS VALOR_INCIAL";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Costo_Actual + " AS VALOR_ACTUAL";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Factura + " AS FACTURA";
                    Mi_SQL = Mi_SQL + "," + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Observaciones + " AS OBSERVACIONES";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Dependencia_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Raza_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Raza_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Adiestramiento.Tabla_Cat_Pat_Tipos_Adiestramiento;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Adiestramiento_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Adiestramiento.Tabla_Cat_Pat_Tipos_Adiestramiento + "." + Cat_Pat_Tipos_Adiestramiento.Campo_Tipo_Adiestramiento_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Funciones.Tabla_Cat_Pat_Funciones;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Funcion_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Funciones.Tabla_Cat_Pat_Funciones + "." + Cat_Pat_Funciones.Campo_Funcion_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Alimentacion.Tabla_Cat_Pat_Tipos_Alimentacion;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Alimentacion_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Alimentacion.Tabla_Cat_Pat_Tipos_Alimentacion + "." + Cat_Pat_Tipos_Alimentacion.Campo_Tipo_Alimentacion_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Color_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Proveedor_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                    if (Parametros.P_No_Inventario_Anterior != null && Parametros.P_No_Inventario_Anterior.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior + " LIKE '%" + Parametros.P_No_Inventario_Anterior + "%'"; ;
                    }
                    if (Parametros.P_No_Inventario_SIAS != null && Parametros.P_No_Inventario_SIAS.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Numero_Inventario + " = '" + Parametros.P_No_Inventario_SIAS + "'"; ;
                    }
                    if (Parametros.P_Nombre_Producto != null && Parametros.P_Nombre_Producto.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre + " LIKE '%" + Parametros.P_Nombre_Producto + "%'";
                    }
                    if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                    }
                    if (Parametros.P_Tipo != null && Parametros.P_Tipo.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + " = '" + Parametros.P_Tipo + "'";
                    }
                    if (Parametros.P_Raza_ID != null && Parametros.P_Raza_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Raza_ID + " = '" + Parametros.P_Raza_ID + "'";
                    }
                    if (Parametros.P_Color_ID != null && Parametros.P_Color_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Color_ID + " = '" + Parametros.P_Color_ID + "'";
                    }
                    if (Parametros.P_Tipo_Alimentacion_ID != null && Parametros.P_Tipo_Alimentacion_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Alimentacion_ID + " = '" + Parametros.P_Tipo_Alimentacion_ID + "'";
                    }
                    if (Parametros.P_Tipo_Adiestramiento_ID != null && Parametros.P_Tipo_Adiestramiento_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Adiestramiento_ID + " = '" + Parametros.P_Tipo_Adiestramiento_ID + "'";
                    }
                    if (Parametros.P_Funcion_ID != null && Parametros.P_Funcion_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Funcion_ID + " = '" + Parametros.P_Funcion_ID + "'";
                    }
                    if (Parametros.P_Sexo != null && Parametros.P_Sexo.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Sexo + " = '" + Parametros.P_Sexo + "'";
                    }
                    if (Parametros.P_Factura != null && Parametros.P_Factura.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Factura + " = '" + Parametros.P_Factura + "'";
                    }
                    if (Parametros.P_Tipo_Ascendencia != null && Parametros.P_Tipo_Ascendencia.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia + " = '" + Parametros.P_Tipo_Ascendencia + "'";
                    }
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                    }
                    if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CEMOVIENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Parametros.P_Resguardante_ID + "')";
                    }
                    if (Parametros.P_Tomar_Fecha_Inicial) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Final) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Adquisicion_Final).AddDays(1).Date) + "'";
                    }
                    Ds_Animales = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Animales != null) {
                        Dt_Animales = Ds_Animales.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Animales;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Bienes_Animales
            ///DESCRIPCIÓN          : Consulta los Animales [Datos Generales].
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 05/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Animales_Completo(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Animales = null;
                DataTable Dt_Animales = new DataTable();
                Boolean Entro_Where = false;
                try {
                    Mi_SQL = "SELECT " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS CLAVE_BIEN";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior + " AS INVENTARIO_ANTERIOR";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Numero_Inventario + " AS INVENTARIO_SIAS";
                    Mi_SQL = Mi_SQL + ", DECODE(TRIM(" + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ")";
                    Mi_SQL = Mi_SQL + ", 'VIGENTE', (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ")";
                    Mi_SQL = Mi_SQL + ", 'BAJA', '') UNIDAD_RESPONSABLE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre + " AS NOMBRE_ANIMAL";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " AS TIPO_ANIMAL";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Nombre + " AS RAZA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Adiestramiento.Tabla_Cat_Pat_Tipos_Adiestramiento + "." + Cat_Pat_Tipos_Adiestramiento.Campo_Nombre + " AS TIPO_ADIESTRAMIENTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Funciones.Tabla_Cat_Pat_Funciones + "." + Cat_Pat_Funciones.Campo_Nombre + " AS FUNCION";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Alimentacion.Tabla_Cat_Pat_Tipos_Alimentacion + "." + Cat_Pat_Tipos_Alimentacion.Campo_Nombre + " AS TIPO_ALIMENTACION";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " AS COLOR";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Nacimiento + " AS FECHA_NACIMIENTO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " AS FECHA_ADQUISICION";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Sexo + " AS SEXO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia + " AS TIPO_ASCENDENCIA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Costo_Actual + " AS VALOR_INCIAL";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Costo_Actual + " AS VALOR_ACTUAL";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Factura + " AS FACTURA";
                    Mi_SQL = Mi_SQL + "," + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Observaciones + " AS OBSERVACIONES";
                    Mi_SQL = Mi_SQL + ", (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "";
                    Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS RESPONSABLE";

                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Raza_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Raza_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Adiestramiento.Tabla_Cat_Pat_Tipos_Adiestramiento;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Adiestramiento_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Adiestramiento.Tabla_Cat_Pat_Tipos_Adiestramiento + "." + Cat_Pat_Tipos_Adiestramiento.Campo_Tipo_Adiestramiento_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Funciones.Tabla_Cat_Pat_Funciones;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Funcion_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Funciones.Tabla_Cat_Pat_Funciones + "." + Cat_Pat_Funciones.Campo_Funcion_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Alimentacion.Tabla_Cat_Pat_Tipos_Alimentacion;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Alimentacion_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Alimentacion.Tabla_Cat_Pat_Tipos_Alimentacion + "." + Cat_Pat_Tipos_Alimentacion.Campo_Tipo_Alimentacion_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Color_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Proveedor_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + ""; 
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + "";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CEMOVIENTE'";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                    Mi_SQL = Mi_SQL + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                    if (Parametros.P_No_Inventario_Anterior != null && Parametros.P_No_Inventario_Anterior.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior + " LIKE '%" + Parametros.P_No_Inventario_Anterior + "%'"; ;
                    }
                    if (Parametros.P_No_Inventario_SIAS != null && Parametros.P_No_Inventario_SIAS.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Numero_Inventario + " = '" + Parametros.P_No_Inventario_SIAS + "'"; ;
                    }
                    if (Parametros.P_Nombre_Producto != null && Parametros.P_Nombre_Producto.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre + " LIKE '%" + Parametros.P_Nombre_Producto + "%'";
                    }
                    if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " IN ('" + Parametros.P_Dependencia_ID + "')";
                    }
                    if (Parametros.P_Tipo != null && Parametros.P_Tipo.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + " = '" + Parametros.P_Tipo + "'";
                    }
                    if (Parametros.P_Raza_ID != null && Parametros.P_Raza_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Raza_ID + " = '" + Parametros.P_Raza_ID + "'";
                    }
                    if (Parametros.P_Color_ID != null && Parametros.P_Color_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Color_ID + " = '" + Parametros.P_Color_ID + "'";
                    }
                    if (Parametros.P_Tipo_Alimentacion_ID != null && Parametros.P_Tipo_Alimentacion_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Alimentacion_ID + " = '" + Parametros.P_Tipo_Alimentacion_ID + "'";
                    }
                    if (Parametros.P_Tipo_Adiestramiento_ID != null && Parametros.P_Tipo_Adiestramiento_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Adiestramiento_ID + " = '" + Parametros.P_Tipo_Adiestramiento_ID + "'";
                    }
                    if (Parametros.P_Funcion_ID != null && Parametros.P_Funcion_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Funcion_ID + " = '" + Parametros.P_Funcion_ID + "'";
                    }
                    if (Parametros.P_Sexo != null && Parametros.P_Sexo.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Sexo + " = '" + Parametros.P_Sexo + "'";
                    }
                    if (Parametros.P_Factura != null && Parametros.P_Factura.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Factura + " = '" + Parametros.P_Factura + "'";
                    }
                    if (Parametros.P_Tipo_Ascendencia != null && Parametros.P_Tipo_Ascendencia.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia + " = '" + Parametros.P_Tipo_Ascendencia + "'";
                    }
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                    }
                    if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " IN ('" + Parametros.P_Resguardante_ID + "')";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                    }
                    if (Parametros.P_Tomar_Fecha_Inicial) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Final) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Adquisicion_Final).AddDays(1).Date) + "'";
                    }
                    Ds_Animales = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Animales != null) {
                        Dt_Animales = Ds_Animales.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Animales;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Vehiculos
            ///DESCRIPCIÓN          : Consulta los Vehículos [Datos Generales].
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 05/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Vehiculos(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Vehiculos = null;
                DataTable Dt_Vehiculos = new DataTable();
                Boolean Entro_Where = false;
                try {
                    Mi_SQL = "SELECT " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS CLAVE_BIEN";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre + " AS NOMBRE_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS DEPENDENCIA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " AS TIPO_VEHICULO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo + " AS MODELO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " AS COLOR";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible + "." + Cat_Pat_Tipos_Combustible.Campo_Descripcion + " AS COMBUSTIBLE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Placas + " AS PLACAS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " AS NUMERO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Economico + " AS NUMERO_ECONOMICO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Descripcion + " AS ZONA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_No_Factura+ " AS FACTURA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Costo_Actual + " AS COSTO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Capacidad_Carga + " AS CAPACIDAD";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Anio_Fabricacion + " AS ANIO_FABRICACION";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + " AS SERIE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Cilindros + " AS NO_CILINDROS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " AS FECHA_ADQUISICION";
                    Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Observaciones + " AS OBSERVACIONES";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora + "." + Cat_Pat_Aseguradora.Campo_Nombre + " AS ASEGURADORA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + "." + Cat_Pat_Procedencias.Campo_Nombre + " AS PROCEDENCIA";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Color_ID + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Combustible_ID + " = " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible + "." + Cat_Pat_Tipos_Combustible.Campo_Tipo_Combustible_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Proveedor_ID + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Zona_ID + " = " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Zona_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora + " ON " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Aseguradora_ID + " = " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora + "." + Cat_Pat_Aseguradora.Campo_Aseguradora_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Procedencia + " = " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + "." + Cat_Pat_Procedencias.Campo_Procedencia_ID + "";
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + " = '" + Parametros.P_Estatus.Trim() + "'";
                    }
                    if (Parametros.P_Factura != null && Parametros.P_Factura.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_No_Factura + " = '" + Parametros.P_Factura.Trim() + "'";
                    }
                    if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID.Trim() + "'";
                    }
                    if (Parametros.P_Tipo != null && Parametros.P_Tipo.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = '" + Parametros.P_Tipo.Trim() + "'";
                    }
                    if (Parametros.P_Aseguradora_ID != null && Parametros.P_Aseguradora_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " IN ( SELECT " + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + " FROM " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " WHERE " + Cat_Pat_Tipos_Vehiculo.Campo_Aseguradora_ID + " = '" + Parametros.P_Aseguradora_ID.Trim() + "')";
                    }
                    if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " IN ( SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Parametros.P_Resguardante_ID.Trim() + "' AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO' AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE' )";
                    }
                    if (Parametros.P_Proveedor != null && Parametros.P_Proveedor.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Proveedor_ID + " = '" + Parametros.P_Proveedor.Trim() + "'";
                    }
                    if (Parametros.P_Zona_ID != null && Parametros.P_Zona_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Zona_ID+ " = '" + Parametros.P_Zona_ID.Trim() + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Inicial) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Final) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Adquisicion_Final).AddDays(1).Date) + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                    Ds_Vehiculos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Vehiculos != null) {
                        Dt_Vehiculos = Ds_Vehiculos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Vehiculos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Vehiculos_Completo
            ///DESCRIPCIÓN          : Consulta los Vehículos [Datos Generales].
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 05/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Vehiculos_Completo(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Vehiculos = null;
                DataTable Dt_Vehiculos = new DataTable();
                Boolean Entro_Where = false;
                try {
                    Mi_SQL = "SELECT " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS CLAVE_BIEN";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre + " AS NOMBRE_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", DECODE(TRIM(" + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ")";
                    Mi_SQL = Mi_SQL + ", 'VIGENTE', (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ")";
                    Mi_SQL = Mi_SQL + ", 'BAJA', '') AS DEPENDENCIA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " AS TIPO_VEHICULO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo + " AS MODELO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " AS COLOR";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible + "." + Cat_Pat_Tipos_Combustible.Campo_Descripcion + " AS COMBUSTIBLE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Placas + " AS PLACAS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " AS NUMERO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Economico + " AS NUMERO_ECONOMICO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Descripcion + " AS ZONA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_No_Factura+ " AS FACTURA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Costo_Actual + " AS COSTO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Capacidad_Carga + " AS CAPACIDAD";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Anio_Fabricacion + " AS ANIO_FABRICACION";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + " AS SERIE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Cilindros + " AS NO_CILINDROS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " AS FECHA_ADQUISICION";
                    Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Observaciones + " AS OBSERVACIONES";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora + "." + Cat_Pat_Aseguradora.Campo_Nombre + " AS ASEGURADORA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + "." + Cat_Pat_Procedencias.Campo_Nombre + " AS PROCEDENCIA";
                    Mi_SQL = Mi_SQL + ", (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "";
                    Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS RESPONSABLE";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Color_ID + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Combustible_ID + " = " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible + "." + Cat_Pat_Tipos_Combustible.Campo_Tipo_Combustible_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Proveedor_ID + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Zona_ID + " = " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + "." + Cat_Pat_Zonas.Campo_Zona_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora + " ON " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Aseguradora_ID + " = " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora + "." + Cat_Pat_Aseguradora.Campo_Aseguradora_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Procedencia + " = " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + "." + Cat_Pat_Procedencias.Campo_Procedencia_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " = " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO'";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " ON " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + " = '" + Parametros.P_Estatus.Trim() + "'";
                    }
                    if (Parametros.P_Factura != null && Parametros.P_Factura.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_No_Factura + " = '" + Parametros.P_Factura.Trim() + "'";
                    }
                    if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " IN ('" + Parametros.P_Dependencia_ID + "')";
                    }
                    if (Parametros.P_Tipo != null && Parametros.P_Tipo.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = '" + Parametros.P_Tipo.Trim() + "'";
                    }
                    if (Parametros.P_Aseguradora_ID != null && Parametros.P_Aseguradora_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " IN ( SELECT " + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + " FROM " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " WHERE " + Cat_Pat_Tipos_Vehiculo.Campo_Aseguradora_ID + " = '" + Parametros.P_Aseguradora_ID.Trim() + "')";
                    }
                    if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " IN ('" + Parametros.P_Resguardante_ID + "')";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                    }
                    if (Parametros.P_Proveedor != null && Parametros.P_Proveedor.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Proveedor_ID + " = '" + Parametros.P_Proveedor.Trim() + "'";
                    }
                    if (Parametros.P_Zona_ID != null && Parametros.P_Zona_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Zona_ID+ " = '" + Parametros.P_Zona_ID.Trim() + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Inicial) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Final) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Adquisicion_Final).AddDays(1).Date) + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                    Ds_Vehiculos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Vehiculos != null) {
                        Dt_Vehiculos = Ds_Vehiculos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Vehiculos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Resguardantes
            ///DESCRIPCIÓN          : consulta los resguardantes de los Bienes Muebles.
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 03/Diciembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Resguardantes(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Bienes_Muebles = null;
                DataTable Dt_Bienes_Muebles = new DataTable();
                Boolean Entro_Where = false;
                try {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AS CLAVE_BIEN";
                    Mi_SQL = Mi_SQL + ", (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                    Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS DEPENDENCIA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " AS ESTATUS_RESGUARDO";
                    Mi_SQL = Mi_SQL + ", (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "";
                    Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS RESPONSABLE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " AS DEPENDENCIA_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA_NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                    Mi_SQL = Mi_SQL + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                    if (Parametros.P_Bien_ID != null && Parametros.P_Bien_ID.Trim().Length > 0)
                    {
                        if (Entro_Where) { Mi_SQL += " AND "; }
                        else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " IN (" + Parametros.P_Bien_ID + ")"; ;
                    }
                    if (Parametros.P_Tipo_Bien != null && Parametros.P_Tipo_Bien.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; }
                        else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = '" + Parametros.P_Tipo_Bien + "'"; ;
                    }
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; }
                        else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                    }
                    if (Parametros.P_Tipo_Bien.Trim().Equals("BIEN_MUEBLE")) {
                        Entro_Where = false;
                        Mi_SQL += " UNION ";
                        Mi_SQL += "SELECT " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " AS CLAVE_BIEN";
                        Mi_SQL = Mi_SQL + ", (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                        Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Estatus + " AS ESTATUS_RESGUARDO";
                        Mi_SQL = Mi_SQL + ", (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "";
                        Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " AS DEPENDENCIA_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA_NOMBRE";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                        if (Parametros.P_Bien_ID != null && Parametros.P_Bien_ID.Trim().Length > 0)
                        {
                            if (Entro_Where) { Mi_SQL += " AND "; }
                            else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " IN (" + Parametros.P_Bien_ID + ")"; ;
                        }
                        if (Parametros.P_Tipo_Bien != null && Parametros.P_Tipo_Bien.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL += " AND "; }
                            else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = '" + Parametros.P_Tipo_Bien + "'"; ;
                        }
                        if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL += " AND "; }
                            else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                        }
                    }
                    if (Parametros.P_Estatus == null || Parametros.P_Estatus.Length == 0) {
                        Mi_SQL = Mi_SQL + " ORDER BY ESTATUS_RESGUARDO DESC";
                    }
                    Ds_Bienes_Muebles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Bienes_Muebles != null) {
                        Dt_Bienes_Muebles = Ds_Bienes_Muebles.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Bienes_Muebles;
            }
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Resguardantes_Cuenta_Publica
            ///DESCRIPCIÓN          : Consulta los resguardantes de los Bienes Muebles para la
            ///                       cuenta publica.
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 14/Enero/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Resguardantes_Cuenta_Publica(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Temporal = null;
                DataTable Dt_Resguardantes = new DataTable();
                Boolean Entro_Where = false;
                try {
                    if (Parametros.P_Operacion.Trim().Equals("RESGUARDO")) { 
                        if (Parametros.P_Estatus.Equals("ALTA")) {
                            Mi_SQL = "SELECT DISTINCT(" + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ") AS FECHA_MOVIMIENTO " +
                                     "FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos +
                                     " WHERE TIPO = '" + Parametros.P_Tipo_Bien + "' AND BIEN_ID = '" + Parametros.P_Bien_ID + "'" +
                                     " ORDER BY " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " ASC";
                        } else if (Parametros.P_Estatus.Equals("BAJA")) {
                            Mi_SQL = "SELECT DISTINCT(" + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + ") AS FECHA_MOVIMIENTO " +
                                     "FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos +
                                     " WHERE TIPO = '" + Parametros.P_Tipo_Bien + "' AND BIEN_ID = '" + Parametros.P_Bien_ID + "'" +
                                     " ORDER BY " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " DESC";
                        }
                    } else if(Parametros.P_Operacion.Trim().Equals("RECIBO")) {
                        if (Parametros.P_Estatus.Equals("ALTA")) {
                            Mi_SQL = "SELECT DISTINCT(" + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial + ") AS FECHA_MOVIMIENTO " +
                                     "FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos +
                                     " WHERE TIPO = '" + Parametros.P_Tipo_Bien + "' AND BIEN_ID = '" + Parametros.P_Bien_ID + "'" +
                                     " ORDER BY " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial + " ASC";
                        } else if (Parametros.P_Estatus.Equals("BAJA")) {
                            Mi_SQL = "SELECT DISTINCT(" + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final + ") AS FECHA_MOVIMIENTO " +
                                     "FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos +
                                     " WHERE TIPO = '" + Parametros.P_Tipo_Bien + "' AND BIEN_ID = '" + Parametros.P_Bien_ID + "'" +
                                     " ORDER BY " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final + " DESC";
                        }
                    }
                
                    if (!String.IsNullOrEmpty(Mi_SQL.ToString())) { 
                        DataSet Aux_DsFecha = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        Mi_SQL = "";
                        if (Aux_DsFecha != null && Aux_DsFecha.Tables.Count > 0) {
                            DataTable Aux_DtFecha = Aux_DsFecha.Tables[0];
                            if (Aux_DtFecha != null && Aux_DtFecha.Rows.Count > 0 ) {
                                if (!String.IsNullOrEmpty(Aux_DtFecha.Rows[0]["FECHA_MOVIMIENTO"].ToString())) { 
                                    DateTime Fecha_Movimiento = Convert.ToDateTime(Aux_DtFecha.Rows[0]["FECHA_MOVIMIENTO"]);
                                    if (Parametros.P_Operacion.Trim().Equals("RESGUARDO")) { 
                                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AS CLAVE_BIEN";
                                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " AS ESTATUS_RESGUARDO";
                                        Mi_SQL = Mi_SQL + ", (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "";
                                        Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS RESPONSABLE";
                                        Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
                                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados;
                                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "";
                                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "";
                                        Mi_SQL = Mi_SQL + " FULL OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                                        Mi_SQL = Mi_SQL + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "";
                                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                                        if (Parametros.P_Bien_ID != null && Parametros.P_Bien_ID.Trim().Length > 0) {
                                            if (Entro_Where) { Mi_SQL += " AND "; }
                                            else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " IN (" + Parametros.P_Bien_ID + ")"; ;
                                        }
                                        if (Parametros.P_Tipo_Bien != null && Parametros.P_Tipo_Bien.Trim().Length > 0) {
                                            if (Entro_Where) { Mi_SQL += " AND "; }
                                            else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = '" + Parametros.P_Tipo_Bien + "'"; ;
                                        }
                                        if (Parametros.P_Estatus.Equals("ALTA")) {
                                            if (Entro_Where) { Mi_SQL += " AND ("; }
                                            else { Mi_SQL += " WHERE ("; Entro_Where = true; }
                                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " >= '" + String.Format("{0:dd/MM/yyyy}", Fecha_Movimiento) + "'";
                                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " < '" + String.Format("{0:dd/MM/yyyy}", Fecha_Movimiento.AddDays(1)) + "')";
                                        } else if (Parametros.P_Estatus.Equals("BAJA")) {
                                            if (Entro_Where) { Mi_SQL += " AND ("; }
                                            else { Mi_SQL += " WHERE ("; Entro_Where = true; }
                                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " >= '" + String.Format("{0:dd/MM/yyyy}", Fecha_Movimiento) + "'";
                                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " < '" + String.Format("{0:dd/MM/yyyy}", Fecha_Movimiento.AddDays(1)) + "')";
                                        }
                                        Mi_SQL = Mi_SQL + " ORDER BY BIEN_RESGUARDO_ID DESC";
                                    } else if(Parametros.P_Operacion.Trim().Equals("RECIBO")) {
                                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " AS CLAVE_BIEN";
                                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Estatus + " AS ESTATUS_RESGUARDO";
                                        Mi_SQL = Mi_SQL + ", (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "";
                                        Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS RESPONSABLE";
                                        Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
                                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + " AS BIEN_RECIBO_ID";
                                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "";
                                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados;
                                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + "";
                                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "";
                                        Mi_SQL = Mi_SQL + " FULL OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                                        Mi_SQL = Mi_SQL + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "";
                                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                                        if (Parametros.P_Bien_ID != null && Parametros.P_Bien_ID.Trim().Length > 0) {
                                            if (Entro_Where) { Mi_SQL += " AND "; }
                                            else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " IN (" + Parametros.P_Bien_ID + ")"; ;
                                        }
                                        if (Parametros.P_Tipo_Bien != null && Parametros.P_Tipo_Bien.Trim().Length > 0) {
                                            if (Entro_Where) { Mi_SQL += " AND "; }
                                            else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = '" + Parametros.P_Tipo_Bien + "'"; ;
                                        }
                                        if (Parametros.P_Estatus.Equals("ALTA")) {
                                            if (Entro_Where) { Mi_SQL += " AND ("; }
                                            else { Mi_SQL += " WHERE ("; Entro_Where = true; }
                                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial + " >= '" + String.Format("{0:dd/MM/yyyy}", Fecha_Movimiento) + "'";
                                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial + " < '" + String.Format("{0:dd/MM/yyyy}", Fecha_Movimiento.AddDays(1)) + "')";
                                        } else if (Parametros.P_Estatus.Equals("BAJA")) {
                                            if (Entro_Where) { Mi_SQL += " AND ("; }
                                            else { Mi_SQL += " WHERE ("; Entro_Where = true; }
                                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final + " >= '" + String.Format("{0:dd/MM/yyyy}", Fecha_Movimiento) + "'";
                                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final + " < '" + String.Format("{0:dd/MM/yyyy}", Fecha_Movimiento.AddDays(1)) + "')";
                                        }
                                        Mi_SQL = Mi_SQL + " ORDER BY BIEN_RECIBO_ID DESC";
                                    }
                                }
                                if (!String.IsNullOrEmpty(Mi_SQL.ToString())) { 
                                    Ds_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                                    if (Ds_Temporal != null) {
                                        Dt_Resguardantes = Ds_Temporal.Tables[0];
                                    }
                                }
                            }  
                        }                  
                    }

                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Resguardantes;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Empleados
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un 
            ///                       DataTable sobre la consulta de Empleados.
            ///PARAMETROS           : 1. Paramentros_Negocio.    Contiene los parametros que se 
            ///                         van a utilizar para hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           :14/Julio/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Empleados(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros)
            {
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try
                {
                    Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", TRIM(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                    Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS DEPENDENCIA";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                    Mi_SQL = Mi_SQL + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + " = 'ACTIVO'";
                    if (Parametros.P_Busqueda_No_Empleado != null && Parametros.P_Busqueda_No_Empleado.Trim().Length > 0)
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Parametros.P_Busqueda_No_Empleado), 6) + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY NOMBRE";
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null)
                    {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
            ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
            ///PARÁMETROS:     
            ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
            ///             2. Longitud_ID. Longitud que tendra el ID. 
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 10/Marzo/2010 
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID) {
                String Retornar = "";
                String Dato = "" + Dato_ID;
                for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                    Retornar = Retornar + "0";
                }
                Retornar = Retornar + Dato;
                return Retornar;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_Listado_Activos_Fijos_Bienes_Muebles
            ///DESCRIPCIÓN: Obtiene el Listado de los Bienes Muebles
            ///PARÁMETROS:     
            ///             1. Parametros. Parametros a Pasar.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 03/Febrero/2012
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static DataTable Obtener_Listado_Activos_Fijos_Bienes_Muebles(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try {
                    Mi_SQL = " SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_MUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + "." + Cat_Pat_Clases_Activo.Campo_Clave + " AS CLASE_ACTIVO";
                    Mi_SQL = Mi_SQL + ", '' AS SOCIEDAD";
                    Mi_SQL = Mi_SQL + ", SUBSTR((" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " ||' '|| " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " ||' '|| " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + " ||' '|| " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + "), 0, 50) AS NOMBRE_PRODUCTO_1";
                    Mi_SQL = Mi_SQL + ", SUBSTR((" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " ||' '|| " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " ||' '|| " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + " ||' '|| " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + "), 50, 50) AS NOMBRE_PRODUCTO_2";
                    Mi_SQL = Mi_SQL + ", SUBSTR((" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " ||' '|| " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " ||' '|| " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + " ||' '|| " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + "), 0, 50) AS NO_PRINCIPAL_ACTIVO_FIJO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " AS NO_SERIE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS NO_INVENTARIO_ANTERIOR";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + "), 'ddmmyyyy') AS FECHA_ULTIMO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", '' AS NOTA_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", '' AS CAPITALIZADO_EL";
                    Mi_SQL = Mi_SQL + ", '' AS DIVISION";
                    Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " AS CENTRO_COSTE";
                    Mi_SQL = Mi_SQL + ", '' AS FONDO";
                    Mi_SQL = Mi_SQL + ", '' AS AREA_FUNCIONAL";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + "." + Cat_Pat_Clasificaciones.Campo_Clave + " AS TIPO_ACTIVO";
                    Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + ", 'BUENO', 'BUEN', 'REGULAR', 'REGU', 'MALO', 'MALO') AS CARACTERISTICAS";
                    Mi_SQL = Mi_SQL + ", '' AS MUNICIPIO";
                    Mi_SQL = Mi_SQL + ", '' AS DESTINO_INVERSION";
                    Mi_SQL = Mi_SQL + ", '' AS ACCION_LEGAL";
                    Mi_SQL = Mi_SQL + ", '' AS CRTI_CLASIF_5";
                    Mi_SQL = Mi_SQL + ", '' AS CLAVE_PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", SUBSTR(" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + ", 0 , 30) AS PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", SUBSTR(" + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + ", 0 , 30) AS FABRICANTE";
                    Mi_SQL = Mi_SQL + ", '' AS PAIS_ORIGEN";
                    Mi_SQL = Mi_SQL + ", '' AS DENOMINACION_TIPO";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Inventario + ", 'ddmmyyyy') AS FECHA_ALTA_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + ", 0) AS VALOR_ORIGINAL";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID + " = " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + "." + Cat_Pat_Clases_Activo.Campo_Clase_Activo_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID + " = " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + "." + Cat_Pat_Clasificaciones.Campo_Clasificacion_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RESGUARDO'"; Consulta_Entro_Where = true;
                    if (Parametros.P_Valor_Minimo > (-1)) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " >= '" + Parametros.P_Valor_Minimo.ToString() + "'";
                    }
                    if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        Mi_SQL = Mi_SQL + " ) )";
                    }
                    if (Parametros.P_Tomar_Fecha_Inicial) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Final) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Final.AddDays(1)) + "'";
                    }
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                    }
                    if (Parametros.P_Clasificacion_ID != null && Parametros.P_Clasificacion_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID + " = '" + Parametros.P_Clasificacion_ID + "'";
                    }
                    if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                    }
                    Mi_SQL = Mi_SQL + " UNION ";
                    Mi_SQL = Mi_SQL + " SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_MUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + "." + Cat_Pat_Clases_Activo.Campo_Clave + " AS CLASE_ACTIVO";
                    Mi_SQL = Mi_SQL + ", '' AS SOCIEDAD";
                    Mi_SQL = Mi_SQL + ", SUBSTR((" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " ||' '|| " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " ||' '|| " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + " ||' '|| " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + "), 0, 50) AS NOMBRE_PRODUCTO_1";
                    Mi_SQL = Mi_SQL + ", SUBSTR((" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " ||' '|| " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " ||' '|| " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + " ||' '|| " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + "), 50, 50) AS NOMBRE_PRODUCTO_2";
                    Mi_SQL = Mi_SQL + ", SUBSTR((" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " ||' '|| " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " ||' '|| " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + " ||' '|| " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + "), 0, 50) AS NO_PRINCIPAL_ACTIVO_FIJO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " AS NO_SERIE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS NO_INVENTARIO_ANTERIOR";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + "), 'ddmmyyyy') AS FECHA_ULTIMO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", '' AS NOTA_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", '' AS CAPITALIZADO_EL";
                    Mi_SQL = Mi_SQL + ", '' AS DIVISION";
                    Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " AS CENTRO_COSTE";
                    Mi_SQL = Mi_SQL + ", '' AS FONDO";
                    Mi_SQL = Mi_SQL + ", '' AS AREA_FUNCIONAL";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + "." + Cat_Pat_Clasificaciones.Campo_Clave + " AS TIPO_ACTIVO";
                    Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + ", 'BUENO', 'BUEN', 'REGULAR', 'REGU', 'MALO', 'MALO') AS CARACTERISTICAS";
                    Mi_SQL = Mi_SQL + ", '' AS MUNICIPIO";
                    Mi_SQL = Mi_SQL + ", '' AS DESTINO_INVERSION";
                    Mi_SQL = Mi_SQL + ", '' AS ACCION_LEGAL";
                    Mi_SQL = Mi_SQL + ", '' AS CRTI_CLASIF_5";
                    Mi_SQL = Mi_SQL + ", '' AS CLAVE_PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", SUBSTR(" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + ", 0 , 30) AS PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", SUBSTR(" + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + ", 0 , 30) AS FABRICANTE";
                    Mi_SQL = Mi_SQL + ", '' AS PAIS_ORIGEN";
                    Mi_SQL = Mi_SQL + ", '' AS DENOMINACION_TIPO";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Inventario + ", 'ddmmyyyy') AS FECHA_ALTA_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + ", 0) AS VALOR_ORIGINAL";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID + " = " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + "." + Cat_Pat_Clases_Activo.Campo_Clase_Activo_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID + " = " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + "." + Cat_Pat_Clasificaciones.Campo_Clasificacion_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RECIBO'"; Consulta_Entro_Where = true;
                    if (Parametros.P_Valor_Minimo > (-1))
                    {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " >= '" + Parametros.P_Valor_Minimo.ToString() + "'";
                    }
                    if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0)
                    {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        Mi_SQL = Mi_SQL + " ) )";
                    }
                    if (Parametros.P_Tomar_Fecha_Inicial)
                    {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Final)
                    {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Final.AddDays(1)) + "'";
                    }
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0)
                    {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                    }
                    if (Parametros.P_Clasificacion_ID != null && Parametros.P_Clasificacion_ID.Trim().Length > 0)
                    {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID + " = '" + Parametros.P_Clasificacion_ID + "'";
                    }
                    if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0)
                    {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY BIEN_MUEBLE_ID";

                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_Listado_Activos_Fijos_Animales
            ///DESCRIPCIÓN: Obtiene el Listado de los Animales.
            ///PARÁMETROS:     
            ///             1. Parametros. Parametros a Pasar.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 07/Febrero/2012
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static DataTable Obtener_Listado_Activos_Fijos_Animales(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try {
                    Mi_SQL = " SELECT " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS ANIMAL_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + "." + Cat_Pat_Clases_Activo.Campo_Clave + " AS CLASE_ACTIVO";
                    Mi_SQL = Mi_SQL + ", '' AS SOCIEDAD";
                    Mi_SQL = Mi_SQL + ", SUBSTR((" + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " ||' '|| " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre + " ||' '|| " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " ||' '|| " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Nombre + "), 0, 50) AS NOMBRE_PRODUCTO_1";
                    Mi_SQL = Mi_SQL + ", SUBSTR((" + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " ||' '|| " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre + " ||' '|| " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Nombre + "), 50, 50) AS NOMBRE_PRODUCTO_2";
                    Mi_SQL = Mi_SQL + ", SUBSTR((" + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " ||' '|| " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre + " ||' '|| " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Nombre + "), 0, 50) AS NO_PRINCIPAL_ACTIVO_FIJO";
                    Mi_SQL = Mi_SQL + ", '' AS NO_SERIE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior + " AS NO_INVENTARIO_ANTERIOR";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(NVL(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Modifico + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Creo + "), 'ddmmyyyy') AS FECHA_ULTIMO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", '' AS NOTA_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", '' AS CAPITALIZADO_EL";
                    Mi_SQL = Mi_SQL + ", '' AS DIVISION";
                    Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " AS CENTRO_COSTE";
                    Mi_SQL = Mi_SQL + ", '' AS FONDO";
                    Mi_SQL = Mi_SQL + ", '' AS AREA_FUNCIONAL";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + "." + Cat_Pat_Clasificaciones.Campo_Clave + " AS TIPO_ACTIVO";
                    Mi_SQL = Mi_SQL + ", '' AS CARACTERISTICAS";
                    Mi_SQL = Mi_SQL + ", '' AS MUNICIPIO";
                    Mi_SQL = Mi_SQL + ", '' AS DESTINO_INVERSION";
                    Mi_SQL = Mi_SQL + ", '' AS ACCION_LEGAL";
                    Mi_SQL = Mi_SQL + ", '' AS CRTI_CLASIF_5";
                    Mi_SQL = Mi_SQL + ", '' AS CLAVE_PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", SUBSTR(" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + ", 0 , 30) AS PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia + ", 'GOBIERNO', 'Nacido en Municipio', 'DESCONOCIDO', 'Ascendencia Desconocida') AS FABRICANTE";
                    Mi_SQL = Mi_SQL + ", '' AS PAIS_ORIGEN";
                    Mi_SQL = Mi_SQL + ", '' AS DENOMINACION_TIPO";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + ", 'ddmmyyyy') AS FECHA_ALTA_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Costo_Actual + ", 0) AS VALOR_ORIGINAL";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Clase_Activo_ID + " = " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + "." + Cat_Pat_Clases_Activo.Campo_Clase_Activo_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + " = " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Color_ID + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Dependencia_ID + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Clasificacion_ID + " = " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + "." + Cat_Pat_Clasificaciones.Campo_Clasificacion_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Proveedor_ID + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + " ON " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Raza_ID + " = " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Raza_ID + "";
                    if (Parametros.P_Valor_Minimo > (-1)) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Costo_Actual + " >= '" + Parametros.P_Valor_Minimo.ToString() + "'";
                    }
                    if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CEMOVIENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        Mi_SQL = Mi_SQL + " ) )";
                    }
                    if (Parametros.P_Tomar_Fecha_Inicial) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Final) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Final.AddDays(1)) + "'";
                    }
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                    }
                    if (Parametros.P_Clasificacion_ID != null && Parametros.P_Clasificacion_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Clasificacion_ID + " = '" + Parametros.P_Clasificacion_ID + "'";
                    }
                    if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + "";
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_Listado_Activos_Fijos_Vehiculos
            ///DESCRIPCIÓN: Obtiene el Listado de los Vehiculos
            ///PARÁMETROS:     
            ///             1. Parametros. Parametros a Pasar.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 03/Febrero/2012
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static DataTable Obtener_Listado_Activos_Fijos_Vehiculos(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try {
                    Mi_SQL = " SELECT " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS BIEN_MUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + "." + Cat_Pat_Clases_Activo.Campo_Clave + " AS CLASE_ACTIVO";
                    Mi_SQL = Mi_SQL + ", '' AS SOCIEDAD";
                    Mi_SQL = Mi_SQL + ", SUBSTR((" + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " ||' '|| " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " ||' '|| " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo + " ||' '|| " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + "), 0, 50) AS NOMBRE_PRODUCTO_1";
                    Mi_SQL = Mi_SQL + ", SUBSTR((" + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " ||' '|| " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " ||' '|| " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo + " ||' '|| " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + "), 50, 50) AS NOMBRE_PRODUCTO_2";
                    Mi_SQL = Mi_SQL + ", SUBSTR((" + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " ||' '|| " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " ||' '|| " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo + " ||' '|| " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + "), 0, 50) AS NO_PRINCIPAL_ACTIVO_FIJO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + " AS NO_SERIE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " AS NO_INVENTARIO_ANTERIOR";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(NVL(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Modifico + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Creo + "), 'ddmmyyyy') AS FECHA_ULTIMO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", '' AS NOTA_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", '' AS CAPITALIZADO_EL";
                    Mi_SQL = Mi_SQL + ", '' AS DIVISION";
                    Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " AS CENTRO_COSTE";
                    Mi_SQL = Mi_SQL + ", '' AS FONDO";
                    Mi_SQL = Mi_SQL + ", '' AS AREA_FUNCIONAL";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + "." + Cat_Pat_Clasificaciones.Campo_Clave + " AS TIPO_ACTIVO";
                    Mi_SQL = Mi_SQL + ", '' AS CARACTERISTICAS";
                    Mi_SQL = Mi_SQL + ", '' AS MUNICIPIO";
                    Mi_SQL = Mi_SQL + ", '' AS DESTINO_INVERSION";
                    Mi_SQL = Mi_SQL + ", '' AS ACCION_LEGAL";
                    Mi_SQL = Mi_SQL + ", '' AS CRTI_CLASIF_5";
                    Mi_SQL = Mi_SQL + ", '' AS CLAVE_PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", SUBSTR(" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + ", 0 , 30) AS PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", SUBSTR(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre + ", 0 , 30) AS FABRICANTE";
                    Mi_SQL = Mi_SQL + ", '' AS PAIS_ORIGEN";
                    Mi_SQL = Mi_SQL + ", '' AS DENOMINACION_TIPO";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + ", 'ddmmyyyy') AS FECHA_ALTA_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Costo_Actual + ", 0) AS VALOR_ORIGINAL";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Clase_Activo_ID + " = " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + "." + Cat_Pat_Clases_Activo.Campo_Clase_Activo_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Color_ID + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Clasificacion_ID + " = " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + "." + Cat_Pat_Clasificaciones.Campo_Clasificacion_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Proveedor_ID + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    if (Parametros.P_Valor_Minimo > (-1)) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Costo_Actual + " >= '" + Parametros.P_Valor_Minimo.ToString() + "'";
                    }
                    if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        Mi_SQL = Mi_SQL + " ) )";
                    }
                    if (Parametros.P_Tomar_Fecha_Inicial) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Final) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Final.AddDays(1)) + "'";
                    }
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                    }
                    if (Parametros.P_Clasificacion_ID != null && Parametros.P_Clasificacion_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Clasificacion_ID + " = '" + Parametros.P_Clasificacion_ID + "'";
                    }
                    if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + "";
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_Listado_Padron_Vehiculos
            ///DESCRIPCIÓN: Obtiene el Listado de los Vehiculos como el Padrón
            ///PARÁMETROS:     
            ///             1. Parametros. Parametros a Pasar.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 03/Febrero/2012
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static DataTable Obtener_Listado_Padron_Vehiculos(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Entro_Where = false;
                try {
                    Mi_SQL = "SELECT " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS VEHICULO_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " AS NUMERO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", DECODE(TRIM(" + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ")";
                    Mi_SQL = Mi_SQL + ", 'VIGENTE', (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + ")";
                    Mi_SQL = Mi_SQL + ", 'BAJA', '') AS DEPENDENCIA_ID";
                    Mi_SQL = Mi_SQL + ", DECODE(TRIM(" + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ")";
                    Mi_SQL = Mi_SQL + ", 'VIGENTE', (" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ")";
                    Mi_SQL = Mi_SQL + ", 'BAJA', '') AS DEPENDENCIA_NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Economico + " AS NUMERO_ECONOMICO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " AS CLASE";
                    Mi_SQL = Mi_SQL + ", (" + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " ||' '|| " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo + ") AS MARCA_TIPO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Anio_Fabricacion+ " AS MODELO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + " AS SERIE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Placas + " AS PLACAS";
                    Mi_SQL = Mi_SQL + ", '' AS RESGUARDANTE";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " ON " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " = " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO' AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " ON " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0)
                    {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + " = '" + Parametros.P_Estatus.Trim() + "'";
                    }
                    if (Parametros.P_Factura != null && Parametros.P_Factura.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_No_Factura + " = '" + Parametros.P_Factura.Trim() + "'";
                    }
                    if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " IN ('" + Parametros.P_Dependencia_ID + "')";
                    }
                    if (Parametros.P_Tipo != null && Parametros.P_Tipo.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = '" + Parametros.P_Tipo.Trim() + "'";
                    }
                    if (Parametros.P_Aseguradora_ID != null && Parametros.P_Aseguradora_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " IN ( SELECT " + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + " FROM " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " WHERE " + Cat_Pat_Tipos_Vehiculo.Campo_Aseguradora_ID + " = '" + Parametros.P_Aseguradora_ID.Trim() + "')";
                    }
                    if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " IN ('" + Parametros.P_Resguardante_ID + "')";
                    }
                    if (Parametros.P_Proveedor != null && Parametros.P_Proveedor.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Proveedor_ID + " = '" + Parametros.P_Proveedor.Trim() + "'";
                    }
                    if (Parametros.P_Zona_ID != null && Parametros.P_Zona_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Zona_ID+ " = '" + Parametros.P_Zona_ID.Trim() + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Inicial) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion_Inicial) + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Final) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Adquisicion_Final).AddDays(1).Date) + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID + "";
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Bienes_Inmuebles
            ///DESCRIPCIÓN          : Consulta los Bienes Inmuebles [Datos Generales].
            ///PARAMETROS           : 
            ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
            ///                                 hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : Marzo/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Bienes_Inmuebles(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Bienes_Inmuebles = null;
                DataTable Dt_Bienes_Inmuebles = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try {
                    Mi_SQL = "SELECT BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " AS BIEN_INMUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS CALLE";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Exterior + " AS NUMERO_EXTERIOR";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Interior + " AS NUMERO_INTERIOR";
                    Mi_SQL = Mi_SQL + ", COLONIAS." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA";
                    Mi_SQL = Mi_SQL + ", CUENTAS_PREDIAL." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Manzana + " AS MANZANA";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Lote + " AS LOTE";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Superficie + " AS SUPERFICIE";
                    Mi_SQL = Mi_SQL + ", CUENTAS_PREDIAL." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + " AS VALOR_FISCAL";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Valor_Comercial + " AS VALOR_COMERCIAL";
                    Mi_SQL = Mi_SQL + ", USOS_INMUEBLES." + Cat_Pat_Usos_Inmuebles.Campo_Descripcion + " AS USO_INMUEBLE";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Area_ID + " AS AREA_DONACION_ID";
                    Mi_SQL = Mi_SQL + ", AREAS_DONACION." + Cat_Pat_Areas_Donacion.Campo_Descripcion + " AS AREA_DONACION";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Estado + " AS ESTADO";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Estatus + " AS ESTATUS";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Inmuebles.Tabla_Ope_Pat_Bienes_Inmuebles + " BIENES_INMUEBLES";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Calle_ID + " = CALLES." + Cat_Pre_Calles.Campo_Calle_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID + " = COLONIAS." + Cat_Ate_Colonias.Campo_Colonia_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTAS_PREDIAL";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Cuenta_Predial_ID + " = CUENTAS_PREDIAL." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Usos_Inmuebles.Tabla_Cat_Pat_Usos_Inmuebles + " USOS_INMUEBLES";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Uso_ID + " = USOS_INMUEBLES." + Cat_Pat_Usos_Inmuebles.Campo_Uso_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Areas_Donacion.Tabla_Cat_Pat_Areas_Donacion + " AREAS_DONACION";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Area_ID + " = AREAS_DONACION." + Cat_Pat_Areas_Donacion.Campo_Area_ID;

                                        
                    if (Parametros.P_Sin_Uso) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Uso_ID + " IS NULL";
                    }
                    if (Parametros.P_Sin_Destino) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Destino_ID + " IS NULL";
                    }
                    if (Parametros.P_Sin_Origen) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Origen_ID + " IS NULL";
                    }
                    if (Parametros.P_Sin_Estatus) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Estatus + " IS NULL";
                    }
                    if (Parametros.P_Sin_Areas_Donacion) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Area_ID + " IS NULL";
                    }
                    if (Parametros.P_Sin_Tipo_Bien) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Bien + " IS NULL";
                    }
                    if (Parametros.P_Sin_Sector) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Sector_ID + " IS NULL";
                    }
                    if (Parametros.P_Sin_Clasificacion_Zona) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Clasificacion_Zona_ID + " IS NULL";
                    }
                    if (Parametros.P_Sin_Clase_Activo) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Clase_Activo_ID + " IS NULL";
                    }
                    if (Parametros.P_Sin_Tipo_Predio) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Predio_ID + " IS NULL";
                    }

                    if (Parametros.P_Escritura != null && Parametros.P_Escritura.Trim().Length > 0) {
                        if (Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " IN (SELECT T_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID + " FROM " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + " T_JURIDICO WHERE T_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA' AND T_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Escritura + " LIKE '%" + Parametros.P_Escritura + "%')";
                    }                    
                    if (Parametros.P_Calle_ID != null && Parametros.P_Calle_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Calle_ID + " = '" + Parametros.P_Calle_ID + "'";
                    }
                    if (Parametros.P_Colonia_ID != null && Parametros.P_Colonia_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID + " = '" + Parametros.P_Colonia_ID + "'";
                    }
                    if (Parametros.P_Uso_ID != null && Parametros.P_Uso_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Uso_ID + " = '" + Parametros.P_Uso_ID + "'";
                    }
                    if (Parametros.P_Destino_ID != null && Parametros.P_Destino_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Destino_ID + " = '" + Parametros.P_Destino_ID + "'";
                    }
                    if (Parametros.P_Origen_ID != null && Parametros.P_Origen_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Origen_ID + " = '" + Parametros.P_Origen_ID + "'";
                    }
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                    }
                    if (Parametros.P_Area_Donacion != null && Parametros.P_Area_Donacion.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Area_ID + " = '" + Parametros.P_Area_Donacion + "'";
                    }
                    if (Parametros.P_Tipo_Bien != null && Parametros.P_Tipo_Bien.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Bien + " = '" + Parametros.P_Tipo_Bien + "'";
                    }
                    if (Parametros.P_Sector != null && Parametros.P_Sector.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Sector_ID + " = '" + Parametros.P_Sector + "'";
                    }
                    if (Parametros.P_Clasificacion_ID != null && Parametros.P_Clasificacion_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Clasificacion_Zona_ID + " = '" + Parametros.P_Clasificacion_ID + "'";
                    }
                    if (Parametros.P_Bien_ID != null && Parametros.P_Bien_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " IN ('" + Parametros.P_Bien_ID + "')";
                    }
                    if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                    }
                    if (Parametros.P_Estado != null && Parametros.P_Estado.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Estado + " = '" + Parametros.P_Estado + "'";
                    }
                    if (Parametros.P_Libre_Gravamen != null && Parametros.P_Libre_Gravamen.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " IN ( ";
                        Mi_SQL = Mi_SQL + "SELECT " + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID + " FROM " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_B_Inm_Juridico.Campo_Libertad_Gravament + " = '" + Parametros.P_Libre_Gravamen + "'";
                        Mi_SQL = Mi_SQL + " )";
                    }
                    if (Parametros.P_Cuenta_Predial_ID != null && Parametros.P_Cuenta_Predial_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Cuenta_Predial_ID + " = '" + Parametros.P_Cuenta_Predial_ID + "'";
                    }
                    if (Parametros.P_Tipo_Predio != null && Parametros.P_Tipo_Predio.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Predio_ID + " = '" + Parametros.P_Tipo_Predio + "'";
                    }
                    if (Parametros.P_Superficie_Inicial > (-1.0)) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Superficie + " >= '" + Parametros.P_Superficie_Inicial + "'";
                    }
                    if (Parametros.P_Superficie_Final > (-1.0)) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Superficie + " <= '" + Parametros.P_Superficie_Final + "'";
                    }
                    if (Parametros.P_Valor_Comercial_Inicial > (-1.0)) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Valor_Comercial + " >= '" + Parametros.P_Valor_Comercial_Inicial + "'";
                    }
                    if (Parametros.P_Valor_Comercial_Final > (-1.0)) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Valor_Comercial + " <= '" + Parametros.P_Valor_Comercial_Final + "'";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Registral_Inicial).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Registral_Inicial) + "'";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Registral_Final).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Registral_Final.AddDays(1)) + "'";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Escritura_Inicial).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " IN ( ";
                        Mi_SQL = Mi_SQL + "SELECT " + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID + " FROM " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Escritura + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Escritura_Inicial) + "'";
                        Mi_SQL = Mi_SQL + " )";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Escritura_Final).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " IN ( ";
                        Mi_SQL = Mi_SQL + "SELECT " + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID + " FROM " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Escritura + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Escritura_Final.AddDays(1)) + "'";
                        Mi_SQL = Mi_SQL + " )";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Baja_Inicial).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Baja + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Baja_Inicial) + "'";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Baja_Final).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Baja + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Baja_Final.AddDays(1)) + "'";
                    }
                    Ds_Bienes_Inmuebles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Bienes_Inmuebles != null) {
                        Dt_Bienes_Inmuebles = Ds_Bienes_Inmuebles.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Bienes_Inmuebles;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_Listado_Activos_Fijos_Bienes_Inmuebles
            ///DESCRIPCIÓN: Obtiene el Listado de los Bienes Inmuebles.
            ///PARÁMETROS:     
            ///             1. Parametros. Parametros a Pasar.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: Marzo/2012
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static DataTable Obtener_Listado_Activos_Fijos_Bienes_Inmuebles(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try {
                    Mi_SQL = "SELECT BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " AS BIEN_INMUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", CLASES_ACTIVOS." + Cat_Pat_Clases_Activo.Campo_Clave + " AS CLASE_ACTIVO";
                    Mi_SQL = Mi_SQL + ", 'MX' AS SOCIEDAD";
                    Mi_SQL = Mi_SQL + ", SUBSTR((COLONIAS." + Cat_Ate_Colonias.Campo_Nombre + " ||', '|| CALLES." + Cat_Pre_Calles.Campo_Nombre + " ||', '|| DESTINOS." + Cat_Pat_Destinos_Inmuebles.Campo_Descripcion + " ||' Mzn '|| BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Manzana + " ||' Lte. '|| BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Lote + " ||' No.'|| BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Exterior + "), 0, 50) AS NOMBRE_PRODUCTO_1";
                    Mi_SQL = Mi_SQL + ", SUBSTR((COLONIAS." + Cat_Ate_Colonias.Campo_Nombre + " ||', '|| CALLES." + Cat_Pre_Calles.Campo_Nombre + " ||', '|| DESTINOS." + Cat_Pat_Destinos_Inmuebles.Campo_Descripcion + " ||' Mzn '|| BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Manzana + " ||' Lte. '|| BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Lote + " ||' No.'|| BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Exterior + "), 50, 50) AS NOMBRE_PRODUCTO_2";
                    Mi_SQL = Mi_SQL + ", SUBSTR((COLONIAS." + Cat_Ate_Colonias.Campo_Nombre + " ||' - '|| DESTINOS." + Cat_Pat_Destinos_Inmuebles.Campo_Descripcion + "), 0, 50) AS NO_PRINCIPAL_ACTIVO_FIJO";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " AS NO_INVENTARIO_ANTERIOR";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(NVL(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Modifico + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Creo + "), 'ddMMyyyy') AS FECHA_ULTIMO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Estado + " AS NOTA_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", 'M150' AS DIVISION";
                    Mi_SQL = Mi_SQL + ", SUBSTR(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Proveedor + ", 0, 30) AS PROVEEDOR";
                    Mi_SQL = Mi_SQL + ", 'MX' AS PAIS_ORIGEN";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro + ", 'ddMMyyyy') AS FECHA_ALTA_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", CUENTAS_PREDIAL." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + " AS VALOR_ORIGINAL";
                    Mi_SQL = Mi_SQL + ", 'IRAPUATO' AS MUNICIPIO";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro + ", 'yyyy') AS ANIO_ADQUISICION_ORIGINAL";
                    Mi_SQL = Mi_SQL + ", SUBSTR(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Registro_Propiedad + ", 0, 8) AS REG_PROPIEDAD_D";
                    Mi_SQL = Mi_SQL + ", TO_CHAR(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro + ", 'ddMMyyyy') AS INSCRIPCION";
                    Mi_SQL = Mi_SQL + ", SUBSTR(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Tomo + ", 0, 5) AS TOMO_REGISTRO_PROPIEDAD";
                    Mi_SQL = Mi_SQL + ", SUBSTR(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Hoja + ", 0, 5) AS HOJA";
                    Mi_SQL = Mi_SQL + ", SUBSTR(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Acta + ", 0, 4) AS NO_ACT";
                    Mi_SQL = Mi_SQL + ", SUBSTR(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Cartilla_Parcelaria + ", 0, 4) AS CARTILLA_PARCELARIA";
                    Mi_SQL = Mi_SQL + ", NVL(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Superficie_Contable + ", 0) AS SUPERFICIE";
                    Mi_SQL = Mi_SQL + ", SUBSTR(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Unidad_Superficie + ", 0, 3) AS UNI_SUPERFICIE";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Inmuebles.Tabla_Ope_Pat_Bienes_Inmuebles + " BIENES_INMUEBLES";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Clases_Activo.Tabla_Cat_Pat_Clases_Activo + " CLASES_ACTIVOS";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Clase_Activo_ID + " = CLASES_ACTIVOS." + Cat_Pat_Clases_Activo.Campo_Clase_Activo_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Calle_ID + " = CALLES." + Cat_Pre_Calles.Campo_Calle_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Usos_Inmuebles.Tabla_Cat_Pat_Usos_Inmuebles + " USOS_INMUEBLES";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Uso_ID + " = USOS_INMUEBLES." + Cat_Pat_Usos_Inmuebles.Campo_Uso_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Destinos_Inmuebles.Tabla_Cat_Pat_Destinos_Inmuebles + " DESTINOS";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Destino_ID + " = DESTINOS." + Cat_Pat_Destinos_Inmuebles.Campo_Destino_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID + " = COLONIAS." + Cat_Ate_Colonias.Campo_Colonia_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTAS_PREDIAL";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Cuenta_Predial_ID + " = CUENTAS_PREDIAL." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + " TABLA_JURIDICO";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " = TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID + " AND TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA'";
                    if (Parametros.P_Tipo_Bien != null && Parametros.P_Tipo_Bien.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Bien + " = '" + Parametros.P_Tipo_Bien.Trim() + "'";
                    }
                    if (Parametros.P_Origen_ID != null && Parametros.P_Origen_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Origen_ID + " = '" + Parametros.P_Origen_ID.Trim() + "'";
                    }
                    if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID.Trim() + "'";
                    }
                    if (Parametros.P_Estado != null && Parametros.P_Estado.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Estado + " = '" + Parametros.P_Estado.Trim() + "'";
                    }
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Estatus + " = '" + Parametros.P_Estatus.Trim() + "'";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Registral_Inicial).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Registral_Inicial) + "'";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Registral_Final).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Registral_Final.AddDays(1)) + "'";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Baja_Inicial).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Baja + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Baja_Inicial) + "'";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Baja_Final).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Baja + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Baja_Final.AddDays(1)) + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID;
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Generales_BI_Ficha_Tecnica
            ///DESCRIPCIÓN: Obtiene los datos generales de los Bienes Inmuebles para el Reporte
            ///             de Ficha Técnica.
            ///PARÁMETROS: 1. Parametros. Parametros para ejecutar la Consulta.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: Marzo/2012
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static DataTable Consultar_Datos_Generales_BI_Ficha_Tecnica(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try {
                    Mi_SQL = "SELECT BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " AS BIEN_INMUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", ORIENTACIONES." + Cat_Pat_Orientaciones_Inm.Campo_Descripcion + " AS SECTOR";
                    Mi_SQL = Mi_SQL + ", COLONIAS." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA";
                    Mi_SQL = Mi_SQL + ", TRIM(CONTRIBUYENTES." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| CONTRIBUYENTES." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| CONTRIBUYENTES." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS PROPIETARIO";
                    Mi_SQL = Mi_SQL + ", CUENTAS_PREDIAL." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL";
                    Mi_SQL = Mi_SQL + ", DESTINOS." + Cat_Pat_Destinos_Inmuebles.Campo_Descripcion + " AS DESTINO";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Superficie + " AS SUPERFICIE";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Manzana + " AS MANZANA";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Lote + " AS LOTE";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Densidad_Construccion + " AS DENSIDAD_CONSTRUCCION";
                    Mi_SQL = Mi_SQL + ", TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS TIPO_PREDIO";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Efectos_Fiscales + " AS EFECTOS_FISCALES";
                    Mi_SQL = Mi_SQL + ", USOS." + Cat_Pat_Usos_Inmuebles.Campo_Descripcion + " AS USO";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Construccion + " AS CONSTRUCCION";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Porcentaje_Ocupacion + " AS PORCENTAJE_OCUPACION";
                    Mi_SQL = Mi_SQL + ", CLASIFICACION_ZONAS." + Cat_Pat_Clasif_Zona_Inm.Campo_Descripcion + " AS CLASIFICACION_ZONA";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Vias_Acceso + " AS VIAS_ACCESO";
                    Mi_SQL = Mi_SQL + ", CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS CALLE";
                    Mi_SQL = Mi_SQL + ", UPPER('Escritura Publica No. '|| NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Escritura + ",'-') ||', Notario Publico No.'|| NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_No_Notario + ",'-') ||' '|| NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Nombre_Notario + ",'-') ) AS ACREDITACION_PROPIEDAD";
                    Mi_SQL = Mi_SQL + ", UPPER(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Constancia_Registral + ") AS CONSTANCIA_REGISTRAL";
                    Mi_SQL = Mi_SQL + ", TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Folio_Real + " AS FOLIO_REAL";
                    Mi_SQL = Mi_SQL + ", TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Libertad_Gravament + " AS CERTIFICADO_GRAVAMEN";
                    Mi_SQL = Mi_SQL + ", UPPER(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Proveedor + ") AS PROVEEDOR";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Inmuebles.Tabla_Ope_Pat_Bienes_Inmuebles + " BIENES_INMUEBLES";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID + " = COLONIAS." + Cat_Ate_Colonias.Campo_Colonia_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Orientaciones_Inm.Tabla_Cat_Pat_Orientaciones_Inm + " ORIENTACIONES ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Sector_ID + " = ORIENTACIONES." + Cat_Pat_Orientaciones_Inm.Campo_Orientacion_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTAS_PREDIAL ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Cuenta_Predial_ID + " = CUENTAS_PREDIAL." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROPIETARIOS ON CUENTAS_PREDIAL." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = PROPIETARIOS." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AND PROPIETARIOS." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO'";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CONTRIBUYENTES ON PROPIETARIOS." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = CONTRIBUYENTES." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Destinos_Inmuebles.Tabla_Cat_Pat_Destinos_Inmuebles + " DESTINOS ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Destino_ID + " = DESTINOS." + Cat_Pat_Destinos_Inmuebles.Campo_Destino_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " TIPOS_PREDIO ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Predio_ID + " = TIPOS_PREDIO." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Usos_Inmuebles.Tabla_Cat_Pat_Usos_Inmuebles + " USOS ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Uso_ID + " = USOS." + Cat_Pat_Usos_Inmuebles.Campo_Uso_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Clasif_Zona_Inm.Tabla_Cat_Pat_Clasif_Zona_Inm + " CLASIFICACION_ZONAS ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Clasificacion_Zona_ID + " = CLASIFICACION_ZONAS." + Cat_Pat_Clasif_Zona_Inm.Campo_Clasificacion_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Calle_ID + " = CALLES." + Cat_Pre_Calles.Campo_Calle_ID + "";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + " TABLA_JURIDICO ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " = TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID + " AND TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA'";
                    if (Parametros.P_Escritura != null && Parametros.P_Escritura.Trim().Length > 0) {
                        if (Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; }
                        Mi_SQL = Mi_SQL + " TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Escritura + " LIKE '%" + Parametros.P_Escritura + "%'";
                    }    
                    if (Parametros.P_Bien_ID != null && Parametros.P_Bien_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " IN ('" + Parametros.P_Bien_ID.Trim() + "')";
                    }
                    if (Parametros.P_Calle_ID != null && Parametros.P_Calle_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Calle_ID + " = '" + Parametros.P_Calle_ID + "'";
                    }
                    if (Parametros.P_Colonia_ID != null && Parametros.P_Colonia_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID + " = '" + Parametros.P_Colonia_ID + "'";
                    }
                    if (Parametros.P_Uso_ID != null && Parametros.P_Uso_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Uso_ID + " = '" + Parametros.P_Uso_ID + "'";
                    }
                    if (Parametros.P_Destino_ID != null && Parametros.P_Destino_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Destino_ID + " = '" + Parametros.P_Destino_ID + "'";
                    }
                    if (Parametros.P_Origen_ID != null && Parametros.P_Origen_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Origen_ID + " = '" + Parametros.P_Origen_ID + "'";
                    }
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                    }
                    if (Parametros.P_Area_Donacion != null && Parametros.P_Area_Donacion.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Area_ID + " = '" + Parametros.P_Area_Donacion + "'";
                    }
                    if (Parametros.P_Tipo_Bien != null && Parametros.P_Tipo_Bien.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Bien + " = '" + Parametros.P_Tipo_Bien + "'";
                    }
                    if (Parametros.P_Sector != null && Parametros.P_Sector.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Sector_ID + " = '" + Parametros.P_Sector + "'";
                    }
                    if (Parametros.P_Clasificacion_ID != null && Parametros.P_Clasificacion_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Clasificacion_Zona_ID + " = '" + Parametros.P_Clasificacion_ID + "'";
                    }
                    if (Parametros.P_Clase_Activo_ID != null && Parametros.P_Clase_Activo_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                    }
                    if (Parametros.P_Estado != null && Parametros.P_Estado.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Estado + " = '" + Parametros.P_Estado + "'";
                    }
                    if (Parametros.P_Libre_Gravamen != null && Parametros.P_Libre_Gravamen.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " IN ( ";
                        Mi_SQL = Mi_SQL + "SELECT " + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID + " FROM " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_B_Inm_Juridico.Campo_Libertad_Gravament + " = '" + Parametros.P_Libre_Gravamen + "'";
                        Mi_SQL = Mi_SQL + " )";
                    }
                    if (Parametros.P_Cuenta_Predial_ID != null && Parametros.P_Cuenta_Predial_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Cuenta_Predial_ID + " = '" + Parametros.P_Cuenta_Predial_ID + "'";
                    }
                    if (Parametros.P_Tipo_Predio != null && Parametros.P_Tipo_Predio.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Predio_ID + " = '" + Parametros.P_Tipo_Predio + "'";
                    }
                    if (Parametros.P_Superficie_Inicial > (-1.0)) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Superficie + " >= '" + Parametros.P_Superficie_Inicial + "'";
                    }
                    if (Parametros.P_Superficie_Final > (-1.0)) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Superficie + " <= '" + Parametros.P_Superficie_Final + "'";
                    }
                    if (Parametros.P_Valor_Comercial_Inicial > (-1.0)) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Valor_Comercial + " >= '" + Parametros.P_Valor_Comercial_Inicial + "'";
                    }
                    if (Parametros.P_Valor_Comercial_Final > (-1.0)) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Valor_Comercial + " <= '" + Parametros.P_Valor_Comercial_Final + "'";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Registral_Inicial).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Registral_Inicial) + "'";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Registral_Final).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Registral_Final.AddDays(1)) + "'";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Escritura_Inicial).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " IN ( ";
                        Mi_SQL = Mi_SQL + "SELECT " + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID + " FROM " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Escritura + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Escritura_Inicial) + "'";
                        Mi_SQL = Mi_SQL + " )";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Escritura_Final).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " IN ( ";
                        Mi_SQL = Mi_SQL + "SELECT " + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID + " FROM " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Escritura + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Escritura_Final.AddDays(1)) + "'";
                        Mi_SQL = Mi_SQL + " )";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Baja_Inicial).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Baja + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Baja_Inicial) + "'";
                    }
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Baja_Final).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Baja + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Baja_Final.AddDays(1)) + "'";
                    }
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Medidas_Colindancias_BI_Ficha_Tecnica
            ///DESCRIPCIÓN: Obtiene los datos de las Medidas y Colindancias de los Bienes 
            ///             Inmuebles para el Reporte de Ficha Técnica.
            ///PARÁMETROS: 1. Parametros. Parametros para ejecutar la Consulta.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: Marzo/2012
            ///MODIFICO : 
            ///FECHA_MODIFICO : 
            ///CAUSA_MODIFICACIÓN : 
            ///*******************************************************************************
            public static DataTable Consultar_Datos_Medidas_Colindancias_BI_Ficha_Tecnica(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try {
                    Mi_SQL = "SELECT MEDIDAS_COLINDANCIAS." + Ope_Pat_B_Inm_Medidas.Campo_No_Registro + " AS NO_REGISTRO";
                    Mi_SQL = Mi_SQL + ", MEDIDAS_COLINDANCIAS." + Ope_Pat_B_Inm_Medidas.Campo_Bien_Inmueble_ID + " AS BIEN_INMUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", ORIENTACIONES." + Cat_Pat_Orientaciones_Inm.Campo_Descripcion + " AS ORIENTACION";
                    Mi_SQL = Mi_SQL + ", MEDIDAS_COLINDANCIAS." + Ope_Pat_B_Inm_Medidas.Campo_Medida + " AS MEDIDA";
                    Mi_SQL = Mi_SQL + ", MEDIDAS_COLINDANCIAS." + Ope_Pat_B_Inm_Medidas.Campo_Colindancia + " AS COLINDANCIA";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas+ " MEDIDAS_COLINDANCIAS";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Orientaciones_Inm.Tabla_Cat_Pat_Orientaciones_Inm + " ORIENTACIONES ON MEDIDAS_COLINDANCIAS." + Ope_Pat_B_Inm_Medidas.Campo_Orientacion_ID + " = ORIENTACIONES." + Cat_Pat_Orientaciones_Inm.Campo_Orientacion_ID + "";
                    if (Parametros.P_Bien_ID != null && Parametros.P_Bien_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " MEDIDAS_COLINDANCIAS." + Ope_Pat_B_Inm_Medidas.Campo_Bien_Inmueble_ID + " IN ('" + Parametros.P_Bien_ID.Trim() + "')";
                    }
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Archivos_BI_Ficha_Tecnica
            ///DESCRIPCIÓN: Obtiene los datos de los Archivos de los Bienes 
            ///             Inmuebles para el Reporte de Ficha Técnica.
            ///PARÁMETROS: 1. Parametros. Parametros para ejecutar la Consulta.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: Marzo/2012
            ///MODIFICO : 
            ///FECHA_MODIFICO : 
            ///CAUSA_MODIFICACIÓN : 
            ///*******************************************************************************
            public static DataTable Consultar_Datos_Archivos_BI_Ficha_Tecnica(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try {
                    Mi_SQL = "SELECT TABLA_ARCHIVOS." + Ope_Pat_B_Inm_Archivos.Campo_No_Registro + " AS NO_REGISTRO";
                    Mi_SQL = Mi_SQL + ", TABLA_ARCHIVOS." + Ope_Pat_B_Inm_Archivos.Campo_Bien_Inmueble_ID + " AS BIEN_INMUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", TABLA_ARCHIVOS." + Ope_Pat_B_Inm_Archivos.Campo_Descripcion_Archivo + " AS DESCRIPCION_ARCHIVO";
                    Mi_SQL = Mi_SQL + ", TABLA_ARCHIVOS." + Ope_Pat_B_Inm_Archivos.Campo_Ruta_Archivo + " AS RUTA_ARCHIVO";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_B_Inm_Archivos.Tabla_Ope_Pat_B_Inm_Archivos + " TABLA_ARCHIVOS";
                    if (Parametros.P_Tipo != null && Parametros.P_Tipo.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " TABLA_ARCHIVOS." + Ope_Pat_B_Inm_Archivos.Campo_Tipo_Archivo + " IN ('" + Parametros.P_Tipo.Trim() + "')";
                    }
                    if (Parametros.P_Bien_ID != null && Parametros.P_Bien_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " TABLA_ARCHIVOS." + Ope_Pat_B_Inm_Archivos.Campo_Bien_Inmueble_ID + " IN ('" + Parametros.P_Bien_ID.Trim() + "')";
                    }
                    if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                    Mi_SQL = Mi_SQL + " TABLA_ARCHIVOS." + Ope_Pat_B_Inm_Archivos.Campo_Estatus + " = 'ACTIVO'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_B_Inm_Archivos.Campo_No_Registro + " DESC";
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Observaciones_BI_Ficha_Tecnica
            ///DESCRIPCIÓN: Obtiene los datos de los Archivos de los Bienes 
            ///             Inmuebles para el Reporte de Ficha Técnica.
            ///PARÁMETROS: 1. Parametros. Parametros para ejecutar la Consulta.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: Marzo/2012
            ///MODIFICO : 
            ///FECHA_MODIFICO : 
            ///CAUSA_MODIFICACIÓN : 
            ///*******************************************************************************
            public static DataTable Consultar_Datos_Observaciones_BI_Ficha_Tecnica(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try {
                    Mi_SQL = "SELECT TABLA_OBSERVACIONES." + Ope_Pat_B_Inm_Observaciones.Campo_No_Observacion + " AS NO_OBSERVACION";
                    Mi_SQL = Mi_SQL + ", TABLA_OBSERVACIONES." + Ope_Pat_B_Inm_Observaciones.Campo_Bien_Inmueble_ID + " AS BIEN_INMUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", TABLA_OBSERVACIONES." + Ope_Pat_B_Inm_Observaciones.Campo_Observacion + " AS OBSERVACION";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_B_Inm_Observaciones.Tabla_Ope_Pat_B_Inm_Observaciones + " TABLA_OBSERVACIONES";
                    if (Parametros.P_Bien_ID != null && Parametros.P_Bien_ID.Trim().Length > 0) {
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " TABLA_OBSERVACIONES." + Ope_Pat_B_Inm_Observaciones.Campo_Bien_Inmueble_ID + " IN ('" + Parametros.P_Bien_ID.Trim() + "')";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY TABLA_OBSERVACIONES." + Ope_Pat_B_Inm_Observaciones.Campo_No_Observacion + " DESC";
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_Datos_Reporte_Cuenta_Publica_Bienes_Inmuebles
            ///DESCRIPCIÓN: Obtiene los datos de la Cuenta Pública para Bienes Inmuebles
            ///             Inmuebles para el Reporte de Ficha Técnica.
            ///PARÁMETROS: 1. Parametros. Parametros para ejecutar la Consulta.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: Marzo/2012
            ///MODIFICO : 
            ///FECHA_MODIFICO : 
            ///CAUSA_MODIFICACIÓN : 
            ///*******************************************************************************
            public static DataTable Obtener_Datos_Reporte_Cuenta_Publica_Bienes_Inmuebles(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                String Mi_SQL = "";
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Consulta_Entro_Where = false;
                try {
                    if (Parametros.P_Movimiento.Trim().Equals("ALTA") || Parametros.P_Movimiento.Trim().Equals("TODOS")) { 
                        Mi_SQL = Mi_SQL + "SELECT BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " AS BIEN_INMUEBLE_ID";
                        Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Alta_Cta_Pub + " AS FECHA_MOVIMIENTO";
                        Mi_SQL = Mi_SQL + ", 'ALTA' AS MOVIMIENTO";
                        Mi_SQL = Mi_SQL + ", COLONIAS." + Cat_Ate_Colonias.Campo_Nombre + " AS UBICACION";
                        Mi_SQL = Mi_SQL + ", TRIM('Escritura Pública No. '|| NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Escritura + ", ' - ')"; 
                        Mi_SQL = Mi_SQL + " ||', Notario Público No. '|| NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_No_Notario + ", ' - ')";
                        Mi_SQL = Mi_SQL + " ||' '|| NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Nombre_Notario + ", ' - ')) AS DATOS_JURIDICO";
                        Mi_SQL = Mi_SQL + ", NVL(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Superficie + ", 0) || ' M2' AS SUPERFICIE";
                        Mi_SQL = Mi_SQL + ", '' AS DESCRIPCION";
                        Mi_SQL = Mi_SQL + ", '' AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Proveedor + ", ' - ') AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + ", NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Folio_Real + ", ' - ') AS FOLIO_REAL";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Inmuebles.Tabla_Ope_Pat_Bienes_Inmuebles + " BIENES_INMUEBLES";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS";
                        Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID + " = COLONIAS." + Cat_Ate_Colonias.Campo_Colonia_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + " TABLA_JURIDICO";
                        Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " = TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID;
                        Mi_SQL = Mi_SQL + " AND TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA'";
                        if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Modificacion_Inicial).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                            if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                            Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Alta_Cta_Pub + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Modificacion_Final).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                            if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                            Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Alta_Cta_Pub + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Final.AddDays(1)) + "'";
                        }
                    }
                    if (Parametros.P_Movimiento.Equals("TODOS")) { Mi_SQL = Mi_SQL + " UNION "; }
                    if (Parametros.P_Movimiento.Trim().Equals("BAJA") || Parametros.P_Movimiento.Trim().Equals("TODOS")) {
                        Consulta_Entro_Where = false;
                        Mi_SQL = Mi_SQL + "SELECT BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " AS BIEN_INMUEBLE_ID";
                        Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Baja + " AS FECHA_MOVIMIENTO";
                        Mi_SQL = Mi_SQL + ", 'BAJA' AS MOVIMIENTO";
                        Mi_SQL = Mi_SQL + ", COLONIAS." + Cat_Ate_Colonias.Campo_Nombre + " AS UBICACION";
                        Mi_SQL = Mi_SQL + ", TRIM('Escritura Pública No. '|| NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Escritura + ", ' - ')"; 
                        Mi_SQL = Mi_SQL + " ||', Notario Público No. '|| NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_No_Notario + ", ' - ')";
                        Mi_SQL = Mi_SQL + " ||' '|| NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Nombre_Notario + ", ' - ')) AS DATOS_JURIDICO";
                        Mi_SQL = Mi_SQL + ", NVL(BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Superficie + ", 0) || ' M2' AS SUPERFICIE";
                        Mi_SQL = Mi_SQL + ", '' AS DESCRIPCION";
                        Mi_SQL = Mi_SQL + ", '' AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Proveedor + ", ' - ') AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + ", NVL(TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Folio_Real + ", ' - ') AS FOLIO_REAL";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Inmuebles.Tabla_Ope_Pat_Bienes_Inmuebles + " BIENES_INMUEBLES";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS";
                        Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID + " = COLONIAS." + Cat_Ate_Colonias.Campo_Colonia_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + " TABLA_JURIDICO";
                        Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " = TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID;
                        Mi_SQL = Mi_SQL + " AND TABLA_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA'";
                        if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Modificacion_Inicial).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                            if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                            Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Baja + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Modificacion_Final).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                            if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                            Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Baja + " < '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Final.AddDays(1)) + "'";
                        }
                        if (!Consulta_Entro_Where) { Mi_SQL = Mi_SQL + " WHERE "; Consulta_Entro_Where = true; } else { Mi_SQL = Mi_SQL + " AND "; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Estado + " = 'BAJA'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY MOVIMIENTO ASC";
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

        #endregion

        #region "Cuenta Publica"

                ///*******************************************************************************
                ///NOMBRE DE LA FUNCIÓN : Obtener_Registros_Bienes_Muebles_Cuenta_Publica
                ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable
                ///                       de los registros de Bienes Muebles.
                ///PARAMETROS           : 
                ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
                ///                                 hacer la consulta de la Base de Datos.
                ///CREO                 : Francisco Antonio Gallardo Castañeda
                ///FECHA_CREO           : 28/Mayo/2011 
                ///MODIFICO             : Francisco Antonio Gallardo Castañeda
                ///FECHA_MODIFICO       : 14/Diciembre/2011 
                ///CAUSA_MODIFICACIÓN   : Se actualizo la consulta con los filtros de reales
                ///*******************************************************************************
                public static DataTable Obtener_Registros_Bienes_Muebles_Cuenta_Publica_Completo(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                    String Mi_SQL = null;
                    DataSet Ds_Bienes_Muebles = null;
                    DataTable Dt_Bienes_Muebles = new DataTable();
                    Boolean Entro_Where = false;
                    try {
                        if (Parametros.P_Tipo.Trim().Equals("ALTAS") || Parametros.P_Tipo.Trim().Equals("TODAS")) {
                            Mi_SQL = "SELECT 'ALTA' AS MOVIMIENTO";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + ", 'DD/MM/YYYY') AS FECHA";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ") AS CANTIDAD";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS TIPO_BIEN";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + ") AS NUMERO_INVENTARIO";
                            Mi_SQL = Mi_SQL + " , ( TRIM(NVL(MARCAS." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Modelo + ", '-'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(COLORES." + Cat_Pat_Colores.Campo_Descripcion + ", '-'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + ", 'S/S'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(MATERIALES." + Cat_Pat_Materiales.Campo_Descripcion + ", '-'))) AS CARACTERISTICAS";
                            Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Estado + " AS CONDICIONES";
                            Mi_SQL = Mi_SQL + " , NVL(DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                            Mi_SQL = Mi_SQL + " , (TRIM(EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + ")||'-'||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) AS RESPONSABLE";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Costo_Alta + ", 0) AS IMPORTE";
                            Mi_SQL = Mi_SQL + " , NVL(PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Factura + ", 'S/F') AS NO_FACTURA";
                            Mi_SQL = Mi_SQL + ", TRIM(RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Observaciones + ") AS OBSERVACIONES";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                            Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " PRINCIPAL";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = MARCAS." + Cat_Com_Marcas.Campo_Marca_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = COLORES." + Cat_Pat_Colores.Campo_Color_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " MATERIALES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = MATERIALES." + Cat_Pat_Materiales.Campo_Material_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE' AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta + " = 'SI'";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                            Mi_SQL = Mi_SQL + " WHERE PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RESGUARDO'"; Entro_Where = true;
                            if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                            }
                            if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                            }
                            if (Parametros.P_Busqueda_Nombre_Empleado != null && Parametros.P_Busqueda_Nombre_Empleado.Trim().Length > 0) { 
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + " TRIM(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") = '" + Parametros.P_Busqueda_Nombre_Empleado.Trim() + "'";
                            }
                            Entro_Where = false;
                            Mi_SQL = Mi_SQL + " UNION ";
                            Mi_SQL = Mi_SQL + "SELECT 'ALTA' AS MOVIMIENTO";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + ", 'DD/MM/YYYY') AS FECHA";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ") AS CANTIDAD";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS TIPO_BIEN";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + ") AS NUMERO_INVENTARIO";
                            Mi_SQL = Mi_SQL + " , ( TRIM(NVL(MARCAS." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Modelo + ", '-'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(COLORES." + Cat_Pat_Colores.Campo_Descripcion + ", '-'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + ", 'S/S'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(MATERIALES." + Cat_Pat_Materiales.Campo_Descripcion + ", '-'))) AS CARACTERISTICAS";
                            Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Estado + " AS CONDICIONES";
                            Mi_SQL = Mi_SQL + " , NVL(DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                            Mi_SQL = Mi_SQL + " , (TRIM(EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + ")||'-'||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) AS RESPONSABLE";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Costo_Alta + ", 0) AS IMPORTE";
                            Mi_SQL = Mi_SQL + " , NVL(PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Factura + ", 'S/F') AS NO_FACTURA";
                            Mi_SQL = Mi_SQL + ", TRIM(RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Observaciones + ") AS OBSERVACIONES";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                            Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " PRINCIPAL";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = MARCAS." + Cat_Com_Marcas.Campo_Marca_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = COLORES." + Cat_Pat_Colores.Campo_Color_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " MATERIALES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = MATERIALES." + Cat_Pat_Materiales.Campo_Material_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + " RESGUARDOS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " AND RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE' AND RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Movimiento_Alta + " = 'SI'";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                            Mi_SQL = Mi_SQL + " WHERE PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RECIBO'"; Entro_Where = true;
                            if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                            }
                            if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                            }
                            if (Parametros.P_Busqueda_Nombre_Empleado != null && Parametros.P_Busqueda_Nombre_Empleado.Trim().Length > 0) { 
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + " TRIM(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") = '" + Parametros.P_Busqueda_Nombre_Empleado.Trim() + "'";
                            }
                        } if (Parametros.P_Tipo.Trim().Equals("BAJAS") || Parametros.P_Tipo.Trim().Equals("TODAS")) {
                            Entro_Where = false;
                            if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                                Mi_SQL += " UNION ";
                                Mi_SQL += "SELECT 'BAJA' AS MOVIMIENTO";
                            } else {
                                Mi_SQL = "SELECT 'BAJA' AS MOVIMIENTO";
                            }
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + ", 'DD/MM/YYYY') AS FECHA";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ") AS CANTIDAD";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS TIPO_BIEN";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + ") AS NUMERO_INVENTARIO";
                            Mi_SQL = Mi_SQL + " , ( TRIM(NVL(MARCAS." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Modelo + ", '-'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(COLORES." + Cat_Pat_Colores.Campo_Descripcion + ", '-'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + ", 'S/S'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(MATERIALES." + Cat_Pat_Materiales.Campo_Descripcion + ", '-'))) AS CARACTERISTICAS";
                            Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Estado + " AS CONDICIONES";
                            Mi_SQL = Mi_SQL + " , NVL(DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                            Mi_SQL = Mi_SQL + " , (TRIM(EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + ")||'-'||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) AS RESPONSABLE";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + ", 0) AS IMPORTE";
                            Mi_SQL = Mi_SQL + " , NVL(PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Factura + ", 'S/F') AS NO_FACTURA";
                            Mi_SQL = Mi_SQL + ", TRIM(RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Observaciones + ") AS OBSERVACIONES";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                            Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " PRINCIPAL";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = MARCAS." + Cat_Com_Marcas.Campo_Marca_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = COLORES." + Cat_Pat_Colores.Campo_Color_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " MATERIALES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = MATERIALES." + Cat_Pat_Materiales.Campo_Material_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE' AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja + " = 'SI'";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                            Mi_SQL = Mi_SQL + " WHERE PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RESGUARDO'"; Entro_Where = true;
                            Mi_SQL = Mi_SQL + " AND PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = 'DEFINITIVA'";
                            if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                            }
                            if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                            }
                            if (Parametros.P_Busqueda_Nombre_Empleado != null && Parametros.P_Busqueda_Nombre_Empleado.Trim().Length > 0) { 
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + " TRIM(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") = '" + Parametros.P_Busqueda_Nombre_Empleado.Trim() + "'";
                            }
                            Entro_Where = false;
                            Mi_SQL = Mi_SQL + " UNION ";
                            Mi_SQL = Mi_SQL + "SELECT 'BAJA' AS MOVIMIENTO";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + ", 'DD/MM/YYYY') AS FECHA";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ") AS CANTIDAD";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS TIPO_BIEN";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + ") AS NUMERO_INVENTARIO";
                            Mi_SQL = Mi_SQL + " , ( TRIM(NVL(MARCAS." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Modelo + ", '-'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(COLORES." + Cat_Pat_Colores.Campo_Descripcion + ", '-'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + ", 'S/S'))";
                            Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(MATERIALES." + Cat_Pat_Materiales.Campo_Descripcion + ", '-'))) AS CARACTERISTICAS";
                            Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Estado + " AS CONDICIONES";
                            Mi_SQL = Mi_SQL + " , NVL(DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                            Mi_SQL = Mi_SQL + " , (TRIM(EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + ")||'-'||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) AS RESPONSABLE";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Costo_Alta + ", 0) AS IMPORTE";
                            Mi_SQL = Mi_SQL + " , NVL(PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Factura + ", 'S/F') AS NO_FACTURA";
                            Mi_SQL = Mi_SQL + ", TRIM(RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Observaciones + ") AS OBSERVACIONES";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                            Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " PRINCIPAL";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = MARCAS." + Cat_Com_Marcas.Campo_Marca_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = COLORES." + Cat_Pat_Colores.Campo_Color_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " MATERIALES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = MATERIALES." + Cat_Pat_Materiales.Campo_Material_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + " RESGUARDOS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " AND RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE' AND RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Movimiento_Baja + " = 'SI'";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                            Mi_SQL = Mi_SQL + " WHERE PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RECIBO'"; Entro_Where = true;
                            Mi_SQL = Mi_SQL + " AND PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = 'DEFINITIVA'";
                            if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                            }
                            if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                            }
                            if (Parametros.P_Busqueda_Nombre_Empleado != null && Parametros.P_Busqueda_Nombre_Empleado.Trim().Length > 0) { 
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + " TRIM(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") = '" + Parametros.P_Busqueda_Nombre_Empleado.Trim() + "'";
                            }
                        }
                        if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                            Ds_Bienes_Muebles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }
                        if (Ds_Bienes_Muebles != null) {
                            Dt_Bienes_Muebles = Ds_Bienes_Muebles.Tables[0];
                        }
                    } catch (Exception Ex) {
                        String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                        throw new Exception(Mensaje);
                    }
                    return Dt_Bienes_Muebles;
                }

                ///*******************************************************************************
                ///NOMBRE DE LA FUNCIÓN : Obtener_Registros_Vehiculos_Cuenta_Publica
                ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable
                ///                       de los registros de Vehiculos.
                ///PARAMETROS           : 
                ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
                ///                                 hacer la consulta de la Base de Datos.
                ///CREO                 : Francisco Antonio Gallardo Castañeda
                ///FECHA_CREO           : 28/Mayo/2011 
                ///MODIFICO             : Francisco Antonio Gallardo Castañeda
                ///FECHA_MODIFICO       : 14/Diciembre/2011 
                ///CAUSA_MODIFICACIÓN   : Se actualizo la consulta con los filtros de reales
                ///*******************************************************************************
                public static DataTable Obtener_Registros_Vehiculos_Cuenta_Publica_Completo(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                    String Mi_SQL = null;
                    DataSet Ds_Vehiculos = null;
                    DataTable Dt_Vehiculos = new DataTable();
                    Boolean Entro_Where = false;
                    try {
                        if (Parametros.P_Tipo.Trim().Equals("ALTAS") || Parametros.P_Tipo.Trim().Equals("TODAS")) {
                            Mi_SQL = "SELECT 'ALTA' AS MOVIMIENTO";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + ", 'DD/MM/YYYY') AS FECHA";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Cantidad + ") AS CANTIDAD";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Nombre + " AS TIPO_BIEN";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + ") AS NUMERO_INVENTARIO";
                            Mi_SQL = Mi_SQL + " , ( NVL(MARCAS." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Modelo + ", '-')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(COLORES." + Cat_Pat_Colores.Campo_Descripcion + ", '-')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + ", 'S/S')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(TIPOS_VEHICULO." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + ", '-')) AS CARACTERISTICAS";
                            Mi_SQL = Mi_SQL + ", 'BUENO' AS CONDICIONES";
                            Mi_SQL = Mi_SQL + " , NVL(DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                            Mi_SQL = Mi_SQL + " , (TRIM(EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + ")||'-'||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) AS RESPONSABLE";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Costo_Alta + ", 0) AS IMPORTE";
                            Mi_SQL = Mi_SQL + " , NVL(PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_No_Factura + ", 'S/F') AS NO_FACTURA";
                            Mi_SQL = Mi_SQL + ", TRIM(RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Observaciones + ") AS OBSERVACIONES";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS BIEN_ID";
                            Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + " PRINCIPAL";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Marca_ID + " = MARCAS." + Cat_Com_Marcas.Campo_Marca_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Color_ID + " = COLORES." + Cat_Pat_Colores.Campo_Color_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " TIPOS_VEHICULO";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = TIPOS_VEHICULO." + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Proveedor_ID  + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO' AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta + " = 'SI'";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                            if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                            }
                            if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                            }
                        } if (Parametros.P_Tipo.Trim().Equals("BAJAS") || Parametros.P_Tipo.Trim().Equals("TODAS")) {
                            Entro_Where = false;
                            if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                                Mi_SQL += " UNION ";
                                Mi_SQL += "SELECT 'BAJA' AS MOVIMIENTO";
                            } else {
                                Mi_SQL = "SELECT 'BAJA' AS MOVIMIENTO";
                            }
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Fecha_Modifico + ", 'DD/MM/YYYY') AS FECHA";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Cantidad + ") AS CANTIDAD";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Nombre + " AS TIPO_BIEN";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + ") AS NUMERO_INVENTARIO";
                            Mi_SQL = Mi_SQL + " , ( NVL(MARCAS." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Modelo + ", '-')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(COLORES." + Cat_Pat_Colores.Campo_Descripcion + ", '-')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + ", 'S/S')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(TIPOS_VEHICULO." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + ", '-')) AS CARACTERISTICAS";
                            Mi_SQL = Mi_SQL + ", 'MALO' AS CONDICIONES";
                            Mi_SQL = Mi_SQL + " , NVL(DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                            Mi_SQL = Mi_SQL + " , (TRIM(EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + ")||'-'||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) AS RESPONSABLE";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Costo_Actual + ", 0) AS IMPORTE";
                            Mi_SQL = Mi_SQL + " , NVL(PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_No_Factura + ", 'S/F') AS NO_FACTURA";
                            Mi_SQL = Mi_SQL + ", TRIM(RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Observaciones + ") AS OBSERVACIONES";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS BIEN_ID";
                            Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + " PRINCIPAL";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Marca_ID + " = MARCAS." + Cat_Com_Marcas.Campo_Marca_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Color_ID + " = COLORES." + Cat_Pat_Colores.Campo_Color_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " TIPOS_VEHICULO";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = TIPOS_VEHICULO." + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Proveedor_ID  + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO' AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja + " = 'SI'";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                            if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                            }
                            if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Fecha_Modifico+ " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                            }
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Estatus + " = 'DEFINITIVA'";
                        }
                        if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) { 
                            Ds_Vehiculos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }
                        if (Ds_Vehiculos != null) {
                            Dt_Vehiculos = Ds_Vehiculos.Tables[0];
                        }
                    } catch (Exception Ex) {
                        String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                        throw new Exception(Mensaje);
                    }
                    return Dt_Vehiculos;
                }

                ///*******************************************************************************
                ///NOMBRE DE LA FUNCIÓN : Obtener_Registros_Animales_Cuenta_Publica
                ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable
                ///                       de los registros de Cemovientes.
                ///PARAMETROS           : 
                ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
                ///                                 hacer la consulta de la Base de Datos.
                ///CREO                 : Francisco Antonio Gallardo Castañeda
                ///FECHA_CREO           : 28/Mayo/2011
                ///MODIFICO             : Francisco Antonio Gallardo Castañeda
                ///FECHA_MODIFICO       : 15/Diciembre/2011 
                ///CAUSA_MODIFICACIÓN   : Se actualizo la consulta con los filtros de reales
                ///*******************************************************************************
                public static DataTable Obtener_Registros_Animales_Cuenta_Publica_Completo(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                    String Mi_SQL = null;
                    DataSet Ds_Cemovientes = null;
                    DataTable Dt_Cemovientes = new DataTable();
                    Boolean Entro_Where = false;
                    try {
                       if (Parametros.P_Tipo.Trim().Equals("ALTAS") || Parametros.P_Tipo.Trim().Equals("TODAS")) {
                            Mi_SQL = "SELECT 'ALTA' AS MOVIMIENTO";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + ", 'DD/MM/YYYY') AS FECHA";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Cantidad + ") AS CANTIDAD";
                            Mi_SQL = Mi_SQL + ", TIPOS_CEMOVIENTES." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " AS TIPO_BIEN";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior + ") AS NUMERO_INVENTARIO";
                            Mi_SQL = Mi_SQL + " , ( NVL(RAZAS." + Cat_Pat_Razas.Campo_Nombre + ", '-')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Nombre + ", '-')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(COLORES." + Cat_Pat_Colores.Campo_Descripcion + ", '-')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Sexo + ", 'S/S')";
                            Mi_SQL = Mi_SQL + " ||', ASCENDENCIA '|| NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia + ", '-')) AS CARACTERISTICAS";
                            Mi_SQL = Mi_SQL + ", 'BUENO' AS CONDICIONES";
                            Mi_SQL = Mi_SQL + " , NVL(DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                            Mi_SQL = Mi_SQL + " , NVL(PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                            Mi_SQL = Mi_SQL + " , (TRIM(EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + ")||'-'||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) AS RESPONSABLE";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Costo_Alta + ", 0.0) AS IMPORTE";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_No_Factura + ", 'S/F') AS NO_FACTURA";
                            Mi_SQL = Mi_SQL + ", TRIM(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Observaciones + ") AS OBSERVACIONES";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS BIEN_ID";
                            Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + " PRINCIPAL";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + " TIPOS_CEMOVIENTES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + " = TIPOS_CEMOVIENTES." + Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + " RAZAS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Raza_ID + " = RAZAS." + Cat_Pat_Razas.Campo_Raza_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Color_ID + " = COLORES." + Cat_Pat_Colores.Campo_Color_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Proveedor_ID + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CEMOVIENTE' AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta + " = 'SI'";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                            if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                            }
                            if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                            }
                        } if (Parametros.P_Tipo.Trim().Equals("BAJAS") || Parametros.P_Tipo.Trim().Equals("TODAS")) {
                            Entro_Where = false;
                            if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                                Mi_SQL += " UNION ";
                                Mi_SQL += "SELECT 'BAJA' AS MOVIMIENTO";
                            } else {
                                Mi_SQL = "SELECT 'BAJA' AS MOVIMIENTO";
                            }
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Fecha_Modifico + ", 'DD/MM/YYYY') AS FECHA";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Cantidad + ") AS CANTIDAD";
                            Mi_SQL = Mi_SQL + ", TIPOS_CEMOVIENTES." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " AS TIPO_BIEN";
                            Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior + ") AS NUMERO_INVENTARIO";
                            Mi_SQL = Mi_SQL + " , ( NVL(RAZAS." + Cat_Pat_Razas.Campo_Nombre + ", '-')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Nombre + ", '-')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(COLORES." + Cat_Pat_Colores.Campo_Descripcion + ", '-')";
                            Mi_SQL = Mi_SQL + " ||', '|| NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Sexo + ", 'S/S')";
                            Mi_SQL = Mi_SQL + " ||', ASCENDENCIA '|| NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia + ", '-')) AS CARACTERISTICAS";
                            Mi_SQL = Mi_SQL + ", 'MALO' AS CONDICIONES";
                            Mi_SQL = Mi_SQL + " , NVL(DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                            Mi_SQL = Mi_SQL + " , NVL(PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                            Mi_SQL = Mi_SQL + " , (TRIM(EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + ")||'-'||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) AS RESPONSABLE";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Costo_Actual + ", 0.0) AS IMPORTE";
                            Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_No_Factura + ", 'S/F') AS NO_FACTURA";
                            Mi_SQL = Mi_SQL + ", TRIM(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Observaciones + ") AS OBSERVACIONES";
                            Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS BIEN_ID";
                            Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + " PRINCIPAL";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + " TIPOS_CEMOVIENTES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + " = TIPOS_CEMOVIENTES." + Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + " RAZAS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Raza_ID + " = RAZAS." + Cat_Pat_Razas.Campo_Raza_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Color_ID + " = COLORES." + Cat_Pat_Colores.Campo_Color_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Proveedor_ID + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS";
                            Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CEMOVIENTE' AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja + " = 'SI'";
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                            Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                            if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0)
                            {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                            }
                            if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0)
                            {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Inicial_Modificacion)
                            {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Fecha_Modifico + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                            }
                            if (Parametros.P_Tomar_Fecha_Final_Modificacion)
                            {
                                if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                                Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Fecha_Modifico + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                            }
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Estatus + " = 'DEFINITIVA'";
                        }
                        if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) { 
                            Ds_Cemovientes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }
                        if (Ds_Cemovientes != null) {
                            Dt_Cemovientes = Ds_Cemovientes.Tables[0];
                        }
                    } catch (Exception Ex) {
                        String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                        throw new Exception(Mensaje);
                    }
                    return Dt_Cemovientes;
                }

        #endregion "Cuenta Publica"

        #region "Cuenta Publica Vigente"

                ///*******************************************************************************
                ///NOMBRE DE LA FUNCIÓN : Obtener_Registros_Bienes_Muebles_Cuenta_Publica
                ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable
                ///                       de los registros de Bienes Muebles.
                ///PARAMETROS           : 
                ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
                ///                                 hacer la consulta de la Base de Datos.
                ///CREO                 : Francisco Antonio Gallardo Castañeda
                ///FECHA_CREO           : 28/Mayo/2011 
                ///MODIFICO             : Francisco Antonio Gallardo Castañeda
                ///FECHA_MODIFICO       : 14/Diciembre/2011 
                ///CAUSA_MODIFICACIÓN   : Se actualizo la consulta con los filtros de reales
                ///*******************************************************************************
                public static DataTable Obtener_Registros_Bienes_Muebles_Cuenta_Publica_Vigente(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                    String Mi_SQL = null;
                    DataSet Ds_Bienes_Muebles = null;
                    DataTable Dt_Bienes_Muebles = new DataTable();
                    Boolean Entro_Where = false;
                    try {
                        Mi_SQL = "SELECT 'ALTA' AS MOVIMIENTO";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + ", 'DD/MM/YYYY') AS FECHA";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ") AS CANTIDAD";
                        Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS TIPO_BIEN";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + ") AS NUMERO_INVENTARIO";
                        Mi_SQL = Mi_SQL + " , ( TRIM(NVL(MARCAS." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Modelo + ", '-'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(COLORES." + Cat_Pat_Colores.Campo_Descripcion + ", '-'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + ", 'S/S'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(MATERIALES." + Cat_Pat_Materiales.Campo_Descripcion + ", '-'))) AS CARACTERISTICAS";
                        Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS CONDICIONES";
                        Mi_SQL = Mi_SQL + " , NVL(DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " , (TRIM(EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + ")||'-'||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Costo_Alta + ", 0) AS IMPORTE";
                        Mi_SQL = Mi_SQL + " , NVL(PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Factura + ", 'S/F') AS NO_FACTURA";
                        Mi_SQL = Mi_SQL + ", TRIM(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Observadores + ") AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " PRINCIPAL";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = MARCAS." + Cat_Com_Marcas.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = COLORES." + Cat_Pat_Colores.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " MATERIALES";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = MATERIALES." + Cat_Pat_Materiales.Campo_Material_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE' AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                        Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                        Mi_SQL = Mi_SQL + " ON EMPLEADOS." + Cat_Empleados.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " WHERE PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RESGUARDO'"; Entro_Where = true;
                        Mi_SQL = Mi_SQL + " AND PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = 'VIGENTE'";
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                        if (Parametros.P_Busqueda_Nombre_Empleado != null && Parametros.P_Busqueda_Nombre_Empleado.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + " TRIM(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") = '" + Parametros.P_Busqueda_Nombre_Empleado.Trim() + "'";
                        }
                        Entro_Where = false;
                        Mi_SQL = Mi_SQL + " UNION ";
                        Mi_SQL = Mi_SQL + "SELECT 'ALTA' AS MOVIMIENTO";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + ", 'DD/MM/YYYY') AS FECHA";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ") AS CANTIDAD";
                        Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS TIPO_BIEN";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + ") AS NUMERO_INVENTARIO";
                        Mi_SQL = Mi_SQL + " , ( TRIM(NVL(MARCAS." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Modelo + ", '-'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(COLORES." + Cat_Pat_Colores.Campo_Descripcion + ", '-'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + ", 'S/S'))";
                        Mi_SQL = Mi_SQL + " ||', '|| TRIM(NVL(MATERIALES." + Cat_Pat_Materiales.Campo_Descripcion + ", '-'))) AS CARACTERISTICAS";
                        Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS CONDICIONES";
                        Mi_SQL = Mi_SQL + " , NVL(DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " , (TRIM(EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + ")||'-'||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Costo_Alta + ", 0) AS IMPORTE";
                        Mi_SQL = Mi_SQL + " , NVL(PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Factura + ", 'S/F') AS NO_FACTURA";
                        Mi_SQL = Mi_SQL + ", TRIM(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Observadores + ") AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " PRINCIPAL";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = MARCAS." + Cat_Com_Marcas.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = COLORES." + Cat_Pat_Colores.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " MATERIALES";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = MATERIALES." + Cat_Pat_Materiales.Campo_Material_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + " RESGUARDOS";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " AND RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE' AND RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                        Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                        Mi_SQL = Mi_SQL + " ON EMPLEADOS." + Cat_Empleados.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " WHERE PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = 'RECIBO'"; Entro_Where = true;
                        Mi_SQL = Mi_SQL + " AND PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = 'VIGENTE'";
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "RESGUARDOS." + Ope_Pat_Bienes_Recibos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                        if (Parametros.P_Busqueda_Nombre_Empleado != null && Parametros.P_Busqueda_Nombre_Empleado.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + " TRIM(PRINCIPAL." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") = '" + Parametros.P_Busqueda_Nombre_Empleado.Trim() + "'";
                        }
                        if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                            Ds_Bienes_Muebles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }
                        if (Ds_Bienes_Muebles != null) {
                            Dt_Bienes_Muebles = Ds_Bienes_Muebles.Tables[0];
                        }
                    } catch (Exception Ex) {
                        String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                        throw new Exception(Mensaje);
                    }
                    return Dt_Bienes_Muebles;
                }

                ///*******************************************************************************
                ///NOMBRE DE LA FUNCIÓN : Obtener_Registros_Vehiculos_Cuenta_Publica
                ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable
                ///                       de los registros de Vehiculos.
                ///PARAMETROS           : 
                ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
                ///                                 hacer la consulta de la Base de Datos.
                ///CREO                 : Francisco Antonio Gallardo Castañeda
                ///FECHA_CREO           : 28/Mayo/2011 
                ///MODIFICO             : Francisco Antonio Gallardo Castañeda
                ///FECHA_MODIFICO       : 14/Diciembre/2011 
                ///CAUSA_MODIFICACIÓN   : Se actualizo la consulta con los filtros de reales
                ///*******************************************************************************
                public static DataTable Obtener_Registros_Vehiculos_Cuenta_Publica_Vigente(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                    String Mi_SQL = null;
                    DataSet Ds_Vehiculos = null;
                    DataTable Dt_Vehiculos = new DataTable();
                    Boolean Entro_Where = false;
                    try {
                        Mi_SQL = "SELECT 'ALTA' AS MOVIMIENTO";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + ", 'DD/MM/YYYY') AS FECHA";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Cantidad + ") AS CANTIDAD";
                        Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Nombre + " AS TIPO_BIEN";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + ") AS NUMERO_INVENTARIO";
                        Mi_SQL = Mi_SQL + " , ( NVL(MARCAS." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Modelo + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(COLORES." + Cat_Pat_Colores.Campo_Descripcion + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + ", 'S/S')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(TIPOS_VEHICULO." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + ", '-')) AS CARACTERISTICAS";
                        Mi_SQL = Mi_SQL + ", 'BUENO' AS CONDICIONES";
                        Mi_SQL = Mi_SQL + " , NVL(DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " , (TRIM(EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + ")||'-'||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Costo_Alta + ", 0) AS IMPORTE";
                        Mi_SQL = Mi_SQL + " , NVL(PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_No_Factura + ", 'S/F') AS NO_FACTURA";
                        Mi_SQL = Mi_SQL + ", TRIM(PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Observaciones + ") AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + " PRINCIPAL";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Marca_ID + " = MARCAS." + Cat_Com_Marcas.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Color_ID + " = COLORES." + Cat_Pat_Colores.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " TIPOS_VEHICULO";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = TIPOS_VEHICULO." + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Proveedor_ID  + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO' AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                        Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                        Mi_SQL = Mi_SQL + " ON EMPLEADOS." + Cat_Empleados.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " WHERE PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Estatus + " = 'VIGENTE'"; Entro_Where = true;
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                        if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) { 
                            Ds_Vehiculos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }
                        if (Ds_Vehiculos != null) {
                            Dt_Vehiculos = Ds_Vehiculos.Tables[0];
                        }
                    } catch (Exception Ex) {
                        String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                        throw new Exception(Mensaje);
                    }
                    return Dt_Vehiculos;
                }

                ///*******************************************************************************
                ///NOMBRE DE LA FUNCIÓN : Obtener_Registros_Animales_Cuenta_Publica
                ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable
                ///                       de los registros de Cemovientes.
                ///PARAMETROS           : 
                ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
                ///                                 hacer la consulta de la Base de Datos.
                ///CREO                 : Francisco Antonio Gallardo Castañeda
                ///FECHA_CREO           : 28/Mayo/2011
                ///MODIFICO             : Francisco Antonio Gallardo Castañeda
                ///FECHA_MODIFICO       : 15/Diciembre/2011 
                ///CAUSA_MODIFICACIÓN   : Se actualizo la consulta con los filtros de reales
                ///*******************************************************************************
                public static DataTable Obtener_Registros_Animales_Cuenta_Publica_Vigente(Cls_Rpt_Pat_Listado_Bienes_Negocio Parametros) { 
                    String Mi_SQL = null;
                    DataSet Ds_Cemovientes = null;
                    DataTable Dt_Cemovientes = new DataTable();
                    Boolean Entro_Where = false;
                    try {
                        Mi_SQL = "SELECT 'ALTA' AS MOVIMIENTO";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + ", 'DD/MM/YYYY') AS FECHA";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Cantidad + ", 1)) AS CANTIDAD";
                        Mi_SQL = Mi_SQL + ", TIPOS_CEMOVIENTES." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " AS TIPO_BIEN";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior + ") AS NUMERO_INVENTARIO";
                        Mi_SQL = Mi_SQL + " , ( NVL(RAZAS." + Cat_Pat_Razas.Campo_Nombre + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Nombre + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(COLORES." + Cat_Pat_Colores.Campo_Descripcion + ", '-')";
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Sexo + ", 'S/S')";
                        Mi_SQL = Mi_SQL + " ||', ASCENDENCIA '|| NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia + ", '-')) AS CARACTERISTICAS";
                        Mi_SQL = Mi_SQL + ", 'BUENO' AS CONDICIONES";
                        Mi_SQL = Mi_SQL + " , NVL(DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + ", '-') AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + " , NVL(PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + ", '-') AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + " , (TRIM(EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + ")||'-'||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + ")||' '||TRIM(EMPLEADOS." + Cat_Empleados.Campo_Nombre + ")) AS RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Costo_Alta + ", 0.0) AS IMPORTE";
                        Mi_SQL = Mi_SQL + ", NVL(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_No_Factura + ", 'S/F') AS NO_FACTURA";
                        Mi_SQL = Mi_SQL + ", TRIM(PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Observaciones + ") AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + " PRINCIPAL";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + " TIPOS_CEMOVIENTES";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + " = TIPOS_CEMOVIENTES." + Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + " RAZAS";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Raza_ID + " = RAZAS." + Cat_Pat_Razas.Campo_Raza_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Color_ID + " = COLORES." + Cat_Pat_Colores.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Proveedor_ID + " = PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS";
                        Mi_SQL = Mi_SQL + " ON PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " = RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CEMOVIENTE' AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
                        Mi_SQL = Mi_SQL + " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                        Mi_SQL = Mi_SQL + " ON EMPLEADOS." + Cat_Empleados.Campo_Dependencia_ID + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " WHERE PRINCIPAL." + Ope_Pat_Vehiculos.Campo_Estatus + " = 'VIGENTE'"; Entro_Where = true;
                        if (Parametros.P_Procedencia != null && Parametros.P_Procedencia.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Procedencia + " = '" + Parametros.P_Procedencia + "'";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) { 
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Inicial_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Modificacion_Inicial) + "'";
                        }
                        if (Parametros.P_Tomar_Fecha_Final_Modificacion) {
                            if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                            Mi_SQL = Mi_SQL + "PRINCIPAL." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parametros.P_Fecha_Modificacion_Final).AddDays(1).Date) + "'";
                        }
                        if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) { 
                            Ds_Cemovientes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }
                        if (Ds_Cemovientes != null) {
                            Dt_Cemovientes = Ds_Cemovientes.Tables[0];
                        }
                    } catch (Exception Ex) {
                        String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                        throw new Exception(Mensaje);
                    }
                    return Dt_Cemovientes;
                }

        #endregion "Cuenta Publica Vigente"

    }
}