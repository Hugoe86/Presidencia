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
using Presidencia.Control_Patrimonial_Reporte_Completo_Vehiculos.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Rpt_Pat_Completo_Vehiculos_Datos
/// </summary>


namespace Presidencia.Control_Patrimonial_Reporte_Completo_Vehiculos.Datos {
    
    public class Cls_Rpt_Pat_Completo_Vehiculos_Datos{

        #region "Metodos"

                    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Datos_Generales
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                     1.  Vehiculo.   Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 11/Julio/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Obtener_Datos_Generales(Cls_Rpt_Pat_Completo_Vehiculos_Negocio Vehiculo) {
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                try {
                    if ((Vehiculo.P_Vehiculo_ID != null) && (Vehiculo.P_Vehiculo_ID.Trim().Length>0)) {
                        Mi_SQL = "SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UNIDAD_RESPONSABLE";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre + " AS CLASIFICACION";
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " AS TIPO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo + " AS MODELO";
                        Mi_SQL = Mi_SQL + ", '' AS NO_RFV";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Serie_Motor + " AS NO_SERIE_MOTOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + " AS NO_SERIE";
                        Mi_SQL = Mi_SQL + ", '' AS NO_PUERTAS";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Vehiculos. Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Cilindros + ",0) AS NO_CILINDROS";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " AS COLOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Placas + " AS NO_PLACAS";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " AS NO_INVENTARIO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Economico + " AS NO_ECONOMICO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible + "." + Cat_Pat_Tipos_Combustible.Campo_Descripcion + " AS COMBUSTIBLE";
                        Mi_SQL = Mi_SQL + ", '' AS OTROS_DATOS";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + "." + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible + "." + Cat_Pat_Tipos_Combustible.Campo_Tipo_Combustible_ID;
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Combustible_ID;
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                    }
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                }
                catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }
        
            ///*****************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Datos_Adquisicion
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                     1.  Vehiculo.   Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 12/Julio/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Obtener_Datos_Adquisicion(Cls_Rpt_Pat_Completo_Vehiculos_Negocio Vehiculo) {
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                try {
                    if ((Vehiculo.P_Vehiculo_ID != null) && (Vehiculo.P_Vehiculo_ID.Trim().Length>0)) {
                        Mi_SQL = "SELECT " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_No_Factura + " AS NO_FACTURA";
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " AS FECHA_ADQUISICION";
                        Mi_SQL = Mi_SQL + ", NVL(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Costo_Actual + ",0) AS COSTO_ADQUISICION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Creo + " AS FECHA_ALTA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Modifico + " AS FECHA_MODIFICACION";
                        Mi_SQL = Mi_SQL + ", '' AS FECHA_BAJA";
                        Mi_SQL = Mi_SQL + ", '' AS FECHA_AVALUO";
                        Mi_SQL = Mi_SQL + ", '' AS COSTO_APROXIMADO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Observaciones + " AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                        Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ON " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                    }
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                }
                catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }
        
            ///*****************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Datos_Estado_Detalles_Vehiculo
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                     1.  Vehiculo.   Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 12/Julio/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Obtener_Datos_Estado_Detalles_Vehiculo(Cls_Rpt_Pat_Completo_Vehiculos_Negocio Vehiculo) {
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                try {
                    if ((Vehiculo.P_Vehiculo_ID != null) && (Vehiculo.P_Vehiculo_ID.Trim().Length>0)) {
                        Mi_SQL = "SELECT " + Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles + "." + Cat_Pat_Vehiculo_Detalles.Campo_Detalle_Vehiculo_ID + " AS DETALLE_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Det_Vehiculos.Tabla_Cat_Pat_Det_Vehiculos + "." + Cat_Pat_Det_Vehiculos.Campo_Nombre + " AS DESCRIPCION";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles + "." + Cat_Pat_Vehiculo_Detalles.Campo_Estado + " AS ESTADO";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Det_Vehiculos.Tabla_Cat_Pat_Det_Vehiculos + ", " + Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Det_Vehiculos.Tabla_Cat_Pat_Det_Vehiculos + "." + Cat_Pat_Det_Vehiculos.Campo_Detalle_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles + "." + Cat_Pat_Vehiculo_Detalles.Campo_Detalle_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles + "." + Cat_Pat_Vehiculo_Detalles.Campo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                    }
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Datos != null) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                }
                catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Datos;
            }

        #endregion

    }

}