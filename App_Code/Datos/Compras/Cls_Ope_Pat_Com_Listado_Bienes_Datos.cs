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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Control_Patrimonial_Listado_Bienes.Negocio;

/// <summary>
/// Summary description for Cls_Pat_Com_Listado_Bienes_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Listado_Bienes.Datos {  
  
    public class Cls_Ope_Pat_Com_Listado_Bienes_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 
        ///                     1.  Parametros. Contiene los parametros que se van a utilizar para
        ///                                     hacer la consulta de la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 12/Septiembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Ope_Pat_Com_Listado_Bienes_Negocio Parametros) {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            try {
                if (Parametros.P_Tipo_DataTable.Equals("MARCAS")) {
                    Mi_SQL = "SELECT " + Cat_Com_Marcas.Campo_Marca_ID + " AS MARCA_ID, " + Cat_Com_Marcas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ORDER BY " + Cat_Com_Marcas.Campo_Nombre;
                } else if (Parametros.P_Tipo_DataTable.Equals("RAZAS")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Razas.Campo_Raza_ID + " AS RAZA_ID, " + Cat_Pat_Razas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + " WHERE  " + Cat_Pat_Razas.Campo_Estatus + " = 'VIGENTE' ORDER BY " + Cat_Pat_Razas.Campo_Nombre;
                } else if (Parametros.P_Tipo_DataTable.Equals("TIPOS_CEMOVIENTES")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID + " AS TIPO_CEMOVIENTE_ID, " + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + " WHERE  " + Cat_Pat_Tipos_Cemovientes.Campo_Estatus + " = 'VIGENTE' ORDER BY " + Cat_Pat_Tipos_Cemovientes.Campo_Nombre;
                } else if (Parametros.P_Tipo_DataTable.Equals("DEPENDENCIAS")) {
                    Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID, " + Cat_Dependencias.Campo_Clave + "||' - '|| " + Cat_Dependencias.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE  " + Cat_Dependencias.Campo_Estatus + " = 'ACTIVO' ORDER BY " + Cat_Dependencias.Campo_Nombre;
                }  else if (Parametros.P_Tipo_DataTable.Equals("EMPLEADOS")) {
                    Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID, " + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Campo_Nombre + " AS NOMBRE FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Unidad_Responsable + "'" + " ORDER BY NOMBRE";
                } 
                //SE OBTIENEN LOS RESULTADOS
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) { Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL); } 
                if (Ds_Datos != null) { Dt_Datos = Ds_Datos.Tables[0]; }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Listado_Bienes
        ///DESCRIPCIÓN: De los filtros pasados en el parametro, hace una consulta listando los Bienes
        ///             que entran dentro de esos filtros.
        ///PARAMENTROS:   
        ///             1.  Parametros. Objeto de la Clase de Negocio con los filtros cargados para poder
        ///                             realizar el listado de los Bienes;
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Listado_Bienes(Cls_Ope_Pat_Com_Listado_Bienes_Negocio Parametros) {
            DataTable Dt_Listado_Bienes = new DataTable();
            DataSet Ds_Listado_Bienes = null;
            String Mi_SQL = null;
            Boolean Ejecutar_Union = false;
            Boolean Entro_Where = false;
            try {
                if (Parametros.P_Tipo != null) {
                    if (Parametros.P_Tipo.Trim().Equals("BIEN_MUEBLE") || Parametros.P_Tipo.Trim().Equals("TODOS")) {
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + ", 'BIEN_MUEBLE' AS CLASIFICACION_BIEN";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " AS FECHA";
                        Mi_SQL = Mi_SQL + ", '1' AS CANTIDAD";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS PRODUCTO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS INVENTARIO_ANTERIOR";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + ") AS INVENTARIO_NUEVO";
                        Mi_SQL = Mi_SQL + ", (" + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre;
                        Mi_SQL = Mi_SQL + " ||', '|| " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo;
                        Mi_SQL = Mi_SQL + " ||', '|| " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion;
                        Mi_SQL = Mi_SQL + " ||', '|| NVL(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + ", 'S/S')";
                        Mi_SQL = Mi_SQL + " ||', '|| " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + ") AS CARACTERISTICAS";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS CONDICIONES";
                        Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS IMPORTE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " AS NO_FACTURA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Observadores + " AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus   + " AS ESTATUS";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ON " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " ON " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " ON " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID;
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ON " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                        Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + "";
                        if (Parametros.P_Inventario_Anterior != null && Parametros.P_Inventario_Anterior.Trim().Length > 0) {
                            if (Entro_Where) {
                                Mi_SQL = Mi_SQL + " AND ";
                            } else {
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Entro_Where = true;
                            }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Inventario_Anterior + "'";
                        }
                        if (Parametros.P_Numero_Inventario != null && Parametros.P_Numero_Inventario.Trim().Length > 0) { 
                            if (Entro_Where) {
                                Mi_SQL = Mi_SQL + " AND ";
                            } else {
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Entro_Where = true;
                            }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Inventario + "'";
                        }
                        if (Parametros.P_Producto != null && Parametros.P_Producto.Trim().Length > 0) { 
                            if (Entro_Where) {
                                Mi_SQL = Mi_SQL + " AND ";
                            } else {
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Entro_Where = true;
                            }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre+ "";
                            if (Parametros.P_Filtro.Equals("EXACTO")) {
                                Mi_SQL = Mi_SQL + " = '" + Parametros.P_Producto + "'";
                            } else { 
                                Mi_SQL = Mi_SQL + " LIKE '%" + Parametros.P_Producto + "%'";
                            }
                        }
                        if (Parametros.P_Marca != null && Parametros.P_Marca.Trim().Length > 0) {
                            if (Entro_Where) {
                                Mi_SQL = Mi_SQL + " AND ";
                            } else {
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Entro_Where = true;
                            }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Marca + "'";                            
                        }
                        if (Parametros.P_Modelo != null && Parametros.P_Modelo.Trim().Length > 0) { 
                            if (Entro_Where) {
                                Mi_SQL = Mi_SQL + " AND ";
                            } else {
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Entro_Where = true;
                            }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + "";
                            Mi_SQL = Mi_SQL + " LIKE '%" + Parametros.P_Modelo + "%'";
                        }
                        if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) { 
                            if (Entro_Where) {
                                Mi_SQL = Mi_SQL + " AND ";
                            } else {
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Entro_Where = true;
                            }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Estatus + "'";
                        }
                        if (Parametros.P_Numero_Factura != null && Parametros.P_Numero_Factura.Trim().Length > 0) { 
                            if (Entro_Where) {
                                Mi_SQL = Mi_SQL + " AND ";
                            } else {
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Entro_Where = true;
                            }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Factura + "'";
                        }
                        if (Parametros.P_Numero_Serie != null && Parametros.P_Numero_Serie.Trim().Length > 0) {
                            if (Entro_Where) {
                                Mi_SQL = Mi_SQL + " AND ";
                            } else {
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Entro_Where = true;
                            }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + "";
                            Mi_SQL = Mi_SQL + " LIKE '%" + Parametros.P_Numero_Serie + "%'";
                        }
                        if (Parametros.P_No_Empleado != null && Parametros.P_No_Empleado.Trim().Length > 0) {
                            if (Entro_Where) {
                                Mi_SQL = Mi_SQL + " AND ";
                            } else {
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Entro_Where = true;
                            }
                            Mi_SQL = Mi_SQL +  Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + Parametros.P_No_Empleado + "' )";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE' AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE')";
                        }
                        if (Parametros.P_RFC_Resguardante != null && Parametros.P_RFC_Resguardante.Trim().Length > 0) {
                            if (Entro_Where) {
                                Mi_SQL = Mi_SQL + " AND ";
                            } else {
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Entro_Where = true;
                            }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_RFC + " LIKE '%" + Parametros.P_RFC_Resguardante + "%' )";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE' AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE')";
                        }
                        if (Parametros.P_Unidad_Responsable != null && Parametros.P_Unidad_Responsable.Trim().Length > 0) {
                            if (Entro_Where) {
                                Mi_SQL = Mi_SQL + " AND ";
                            } else {
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Entro_Where = true;
                            }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID;
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Unidad_Responsable + "'";
                        }
                        if (Parametros.P_Resguardante != null && Parametros.P_Resguardante.Trim().Length > 0) {
                            if (Entro_Where) {
                                Mi_SQL = Mi_SQL + " AND ";
                            } else {
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Entro_Where = true;
                            }
                            Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Parametros.P_Resguardante + "'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE' AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE')";
                        }
                        Ejecutar_Union = true;
                    }
                    if (Parametros.P_Tipo.Trim().Equals("CEMOVIENTE") || Parametros.P_Tipo.Trim().Equals("TODOS")) {
                        if (Ejecutar_Union) {
                            Mi_SQL = Mi_SQL + " UNION ";
                        }
                        Mi_SQL = Mi_SQL + "SELECT " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre + " AS PRODUCTO";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Numero_Inventario + ") AS INVENTARIO_ANTERIOR";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Numero_Inventario + ") AS INVENTARIO_NUEVO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Nombre + " AS MARCA";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " AS MODELO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Estatus + " AS ESTATUS";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + ", " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + ", " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Raza_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Raza_ID + "";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID + "";
                        if (Parametros.P_Inventario_Anterior != null && Parametros.P_Inventario_Anterior.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Numero_Inventario + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Inventario_Anterior + "'";
                        }
                        if (Parametros.P_Numero_Inventario != null && Parametros.P_Numero_Inventario.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Numero_Inventario + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Inventario + "'";
                        }
                        if (Parametros.P_Producto != null && Parametros.P_Producto.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre + "";
                            if (Parametros.P_Filtro.Equals("EXACTO")) {
                                Mi_SQL = Mi_SQL + " = '" + Parametros.P_Producto + "'";
                            } else { 
                                Mi_SQL = Mi_SQL + " LIKE '%" + Parametros.P_Producto + "%'";
                            }
                        }
                        if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Estatus + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Estatus + "'";
                        }
                        if (Parametros.P_Tipo_Cemoviente != null && Parametros.P_Tipo_Cemoviente.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Tipo_Cemoviente + "'";                            
                        }
                        if (Parametros.P_Raza != null && Parametros.P_Raza.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Raza_ID + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Raza + "'";                            
                        }
                        if (Parametros.P_Numero_Factura != null && Parametros.P_Numero_Factura.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Factura + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Factura + "'";
                        }
                        if (Parametros.P_No_Empleado != null && Parametros.P_No_Empleado.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + Parametros.P_No_Empleado + "' )";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE' AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CEMOVIENTE')";
                        }
                        if (Parametros.P_RFC_Resguardante != null && Parametros.P_RFC_Resguardante.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_RFC + " LIKE '%" + Parametros.P_RFC_Resguardante + "%' )";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE' AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CEMOVIENTE')";
                        }
                        if (Parametros.P_Unidad_Responsable != null && Parametros.P_Unidad_Responsable.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Dependencia_ID;
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Unidad_Responsable + "'";
                        }
                        if (Parametros.P_Resguardante != null && Parametros.P_Resguardante.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Parametros.P_Resguardante + "'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE' AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CEMOVIENTE')";
                        }
                        Ejecutar_Union = true;
                    }
                    if (Parametros.P_Tipo.Trim().Equals("VEHICULO") || Parametros.P_Tipo.Trim().Equals("TODOS")) {
                        if (Ejecutar_Union) {
                            Mi_SQL = Mi_SQL + " UNION ";
                        }
                        Mi_SQL = Mi_SQL + "SELECT " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS BIEN_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre + " AS PRODUCTO";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + ") AS INVENTARIO_ANTERIOR";
                        Mi_SQL = Mi_SQL + ", TO_CHAR(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + ") AS INVENTARIO_NUEVO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Ope_Pat_Vehiculos.Campo_Nombre + " AS MARCA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo + " AS MODELO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + " AS ESTATUS";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID + "";
                        Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                        if (Parametros.P_Inventario_Anterior != null && Parametros.P_Inventario_Anterior.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Inventario_Anterior + "'";
                        }
                        if (Parametros.P_Numero_Inventario != null && Parametros.P_Numero_Inventario.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Inventario + "'";
                        }
                        if (Parametros.P_Producto != null && Parametros.P_Producto.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre + "";
                            if (Parametros.P_Filtro.Equals("EXACTO")) {
                                Mi_SQL = Mi_SQL + " = '" + Parametros.P_Producto + "'";
                            } else { 
                                Mi_SQL = Mi_SQL + " LIKE '%" + Parametros.P_Producto + "%'";
                            }
                        }
                        if (Parametros.P_Marca != null && Parametros.P_Marca.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Marca + "'";                            
                        }
                        if (Parametros.P_Modelo != null && Parametros.P_Modelo.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo + "";
                            Mi_SQL = Mi_SQL + " LIKE '%" + Parametros.P_Modelo + "%'";
                        }
                        if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Estatus + "'";
                        }
                        if (Parametros.P_Numero_Factura != null && Parametros.P_Numero_Factura.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_No_Factura + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Factura + "'";
                        }
                        if (Parametros.P_Numero_Serie != null && Parametros.P_Numero_Serie.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + "";
                            Mi_SQL = Mi_SQL + " LIKE '%" + Parametros.P_Numero_Serie + "%'";
                        }
                        if (Parametros.P_No_Empleado != null && Parametros.P_No_Empleado.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + Parametros.P_No_Empleado + "' )";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE' AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO')";
                        }
                        if (Parametros.P_RFC_Resguardante != null && Parametros.P_RFC_Resguardante.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_RFC + " LIKE '%" + Parametros.P_RFC_Resguardante + "%' )";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE' AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO')";
                        }
                        if (Parametros.P_Unidad_Responsable != null && Parametros.P_Unidad_Responsable.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID;
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Unidad_Responsable + "'";
                        }
                        if (Parametros.P_Resguardante != null && Parametros.P_Resguardante.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Parametros.P_Resguardante + "'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE' AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO')";
                        }
                    }
                }
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) { Ds_Listado_Bienes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL); }
                if (Ds_Listado_Bienes != null) { Dt_Listado_Bienes = Ds_Listado_Bienes.Tables[0]; }
            } catch (OracleException Ex) {
                String Mensaje = "Error al intentar consultar los registros de Bienes en la Base de Datos. Error: [" + Ex.Message + "]"; 
                throw new Exception(Mensaje);
            } catch (Exception Ex) {
                String Mensaje = "Error General al intentar consultar los registros de Bienes. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);                
            }
            return Dt_Listado_Bienes;
        }

    }
}