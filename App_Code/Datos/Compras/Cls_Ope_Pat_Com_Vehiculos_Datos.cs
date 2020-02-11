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
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using System.Collections.Generic;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using System.IO;
using Presidencia.Control_Patrimonial_Operacion_Partes_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Pat_Com_Vehiculos_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Operacion_Vehiculos.Datos {

    public class Cls_Ope_Pat_Com_Vehiculos_Datos {

        ///*******************************************************************************Alta_Migrar_Vehiculo
        ///NOMBRE DE LA FUNCIÓN : Alta_Vehiculo
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo Bien-Vehiculo.
        ///PARAMETROS           : 
        ///                     1.  Vehiculo.   Contiene los parametros que se van a dar de
        ///                                     Alta en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 03/Diciembre/2010 
        ///MODIFICO             : Salvador Hernández Ramírez
        ///FECHA_MODIFICO       : 03/Febrero/2011 
        ///CAUSA_MODIFICACIÓN   : Se le asigno codigo para que cuando el producto venga de una requisición en "PROCEDENCIA" se ponga "REQUISICION"
        ///MODIFICO             : Salvador Hernández  Ramírez
        ///FECHA_MODIFICO       : 09/Febrero/2011 
        ///CAUSA_MODIFICACIÓN   : Se implementó el método "Alta_Bitacora" para capturar los Insert y Update en la tabla "APL_BITACORA" de la Base de Datos
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Vehiculos_Negocio Alta_Vehiculo(Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo) {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_SQL = "";

            try {

                // Se actualiza el Producto
                if (Vehiculo.P_No_Requisicion != null)  {
                    Mi_SQL = " UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Req_Producto.Campo_Resguardado + " = 'SI'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = '" + Vehiculo.P_Producto_ID+ "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = '" + Vehiculo.P_No_Requisicion + "'";

                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }

                Int64 No_Inventario = Consulta_Consecutivo_Inventario();
                Vehiculo.P_Numero_Inventario = No_Inventario;

                String Vehiculo_ID = Obtener_ID_Consecutivo(Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos, Ope_Pat_Vehiculos.Campo_Vehiculo_ID, 10);
               
                Mi_SQL = "INSERT INTO " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Producto_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Procedencia;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Marca_ID; 
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Modelo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Donador_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID; 
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Tipo_Combustible_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Color_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Zona_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Numero_Inventario;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Numero_Economico;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Capacidad_Carga;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Placas;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Anio_Fabricacion;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Serie_Carroceria;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Numero_Cilindros;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Kilometraje;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Odometro;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Costo_Actual;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Costo_Alta;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Observaciones;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_No_Factura;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Clase_Activo_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Clasificacion_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Cantidad;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Vehiculo_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Producto_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Procedencia + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Nombre_Producto + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Marca_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Modelo_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Donador_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Tipo_Vehiculo_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Tipo_Combustible_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Color_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Zona_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Dependencia_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Vehiculo.P_Numero_Inventario + "";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Numero_Economico_ + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Capacidad_Carga + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Placas + "'";
                Mi_SQL = Mi_SQL + ", " + Vehiculo.P_Anio_Fabricacion + "";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Serie_Carroceria + "'";
                Mi_SQL = Mi_SQL + ", " + Vehiculo.P_Numero_Cilindros + "";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Kilometraje + "'";
                Mi_SQL = Mi_SQL + ",'" + Vehiculo.P_Odometro + "'";
                Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Vehiculo.P_Fecha_Adquisicion) + "'";
                Mi_SQL = Mi_SQL + ",'" + Vehiculo.P_Costo_Actual + "'";
                Mi_SQL = Mi_SQL + ",'" + Vehiculo.P_Costo_Actual + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Observaciones + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_No_Factura_ + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Proveedor_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Clase_Activo_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Clasificacion_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Cantidad + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Usuario_Nombre + "'"; 
                Mi_SQL = Mi_SQL + ", SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Alta_Vehiculos.aspx", Vehiculo_ID, Mi_SQL);  // Se da de alta el insert en la tabla "APL_BITACORA" de la BD

                if (Vehiculo.P_Resguardantes != null && Vehiculo.P_Resguardantes.Rows.Count > 0){
                    String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 50);
                    for (Int32 Cnt = 0; Cnt < Vehiculo.P_Resguardantes.Rows.Count; Cnt++) {
                        Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estado + ", " + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta + ", " + Ope_Pat_Bienes_Resguardos.Campo_Observaciones;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Vehiculo_ID + "', 'VEHICULO','" + Vehiculo.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                        Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Usuario_ID + "', 'VIGENTE', '" + Vehiculo.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                        Mi_SQL = Mi_SQL + ",'','" + Vehiculo.P_Dependencia_ID + "'";
                        Mi_SQL = Mi_SQL + ",'SI','" + Vehiculo.P_Observaciones + "'";
                        Mi_SQL = Mi_SQL + ",'" + Vehiculo.P_Usuario_Nombre + "', SYSDATE)";
                        ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Alta_Vehiculos.aspx", ID_Consecutivo, Mi_SQL);  // Se da de alta el insert en la tabla "APL_BITACORA" de la BD
                    }
                }

                // Asignar No. Inventario
                Int64 Inventario_ID = Convert.ToInt64(Obtener_ID_Consecutivo(Ope_Alm_Pat_Inv_Vehiculos.Tabla_Ope_Alm_Pat_Inv_Vehiculos, Ope_Alm_Pat_Inv_Vehiculos.Campo_No_Inventario, 25));
                Mi_SQL = "INSERT INTO " + Ope_Alm_Pat_Inv_Vehiculos.Tabla_Ope_Alm_Pat_Inv_Vehiculos + " (";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_No_Inventario + ", "; // Es el No de  contra recibo
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_Inventario + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_Producto_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES(" + Inventario_ID + ", ";
                Mi_SQL = Mi_SQL + No_Inventario + ", ";
                Mi_SQL = Mi_SQL + "'" + Vehiculo.P_Producto_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Vehiculo.P_Usuario_ID + "', ";
                Mi_SQL = Mi_SQL + "SYSDATE)";

                //Ejecutar consulta
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery(); // Se ejecuta la operación

                String ID_Consecutivo_Archivo = "";
                if (Vehiculo.P_Archivo != null && Vehiculo.P_Archivo.Trim().Length > 0) {
                    ID_Consecutivo_Archivo = Obtener_ID_Consecutivo(Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes, Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID, 50);
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes + " ( " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha_Creo + " ) VALUES ( " + Convert.ToInt32(ID_Consecutivo_Archivo) + ", '" + Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + " , 'VEHICULO', SYSDATE, '" + Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Vehiculo.P_Archivo) + "', 'NORMAL', '" + Vehiculo.P_Usuario_Nombre + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                Vehiculo.P_Vehiculo_ID = Vehiculo_ID;
                Trans.Commit();
                if (Vehiculo.P_Archivo != null && Vehiculo.P_Archivo.Trim().Length > 0) {
                    Vehiculo.P_Archivo = Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Vehiculo.P_Archivo);
                }
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]"; 
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]"; 
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else {
                    Mensaje = "Error al intentar dar de Alta Vehiculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }finally {
                 Cn.Close();
            }
            return Vehiculo;
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 
        ///                     1.  Vehiculo.   Contiene los parametros que se van a utilizar para
        ///                                     hacer la consulta de la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 03/Diciembre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo)
        {
            String Mi_SQL = null;
            DataSet Ds_Vehiculo = null;
            DataTable Dt_Vehiculo = new DataTable();
            try {
                if (Vehiculo.P_Tipo_DataTable.Equals("PRODUCTO")) {
                    Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " AS PRODUCTO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + " AS CLAVE_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " AS NOMBRE_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " AS MODELO_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Costo + " AS COSTO_PRODUCTO";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Tipo + " ='VEHICULO'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Modelo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " = '" + Vehiculo.P_Producto_ID + "'";
                } else if (Vehiculo.P_Tipo_DataTable.Equals("DEPENDENCIAS")) {
                    Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID, " + Cat_Dependencias.Campo_Clave + "||'-'|| " + Cat_Dependencias.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " ORDER BY " + Cat_Dependencias.Campo_Nombre;
                } else if (Vehiculo.P_Tipo_DataTable.Equals("AREAS")) {
                    Mi_SQL = "SELECT " + Cat_Areas.Campo_Area_ID + " AS AREA_ID, " + Cat_Areas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Areas.Tabla_Cat_Areas + " ORDER BY " + Cat_Areas.Campo_Nombre;
                } else if (Vehiculo.P_Tipo_DataTable.Equals("TIPOS_VEHICULOS")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + " AS TIPO_VEHICULO_ID, " + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " ORDER BY " + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion;
                } else if (Vehiculo.P_Tipo_DataTable.Equals("TIPOS_COMBUSTIBLE")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Tipos_Combustible.Campo_Tipo_Combustible_ID + " AS TIPO_COMBUSTIBLE_ID, " + Cat_Pat_Tipos_Combustible.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible + " ORDER BY " + Cat_Pat_Tipos_Combustible.Campo_Descripcion;
                } else if (Vehiculo.P_Tipo_DataTable.Equals("ZONAS")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Zonas.Campo_Zona_ID + " AS ZONA_ID, " + Cat_Pat_Zonas.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + " ORDER BY " + Cat_Pat_Zonas.Campo_Descripcion;
                } else if (Vehiculo.P_Tipo_DataTable.Equals("MODELOS")) {
                    Mi_SQL = "SELECT " + Cat_Com_Modelos.Campo_Modelo_ID + " AS MODELO_ID, " + Cat_Com_Modelos.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " ORDER BY " + Cat_Com_Modelos.Campo_Nombre;
                } else if (Vehiculo.P_Tipo_DataTable.Equals("MATERIALES")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Materiales.Campo_Material_ID + " AS MATERIAL_ID, " + Cat_Pat_Materiales.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " ORDER BY " + Cat_Pat_Materiales.Campo_Descripcion;
                } else if (Vehiculo.P_Tipo_DataTable.Equals("MARCAS")) {
                    Mi_SQL = "SELECT " + Cat_Com_Marcas.Campo_Marca_ID + " AS MARCA_ID, " + Cat_Com_Marcas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ORDER BY " + Cat_Com_Marcas.Campo_Nombre;
                } else if (Vehiculo.P_Tipo_DataTable.Equals("COLORES")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Colores.Campo_Color_ID + " AS COLOR_ID, " + Cat_Pat_Colores.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " ORDER BY " + Cat_Pat_Colores.Campo_Descripcion;
                } else if (Vehiculo.P_Tipo_DataTable.Equals("PROVEEDORES")) {
                    Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID + " AS PROVEEDOR_ID, " + Cat_Com_Proveedores.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ORDER BY " + Cat_Com_Proveedores.Campo_Nombre;
                } else if (Vehiculo.P_Tipo_DataTable.Equals("EMPLEADOS")) {
                    Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID, " + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Campo_Nombre + " AS NOMBRE FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Vehiculo.P_Dependencia_ID + "'" + " ORDER BY NOMBRE";
                } else if (Vehiculo.P_Tipo_DataTable.Equals("EMPLEADOS_VEHICULO")) {
                    Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID, " + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Campo_Nombre + " AS NOMBRE FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + " IN ( SELECT " + Ope_Pat_Vehiculos.Campo_Dependencia_ID + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " = '" + Vehiculo.P_Vehiculo_ID + "')" + " ORDER BY NOMBRE";
                } else if (Vehiculo.P_Tipo_DataTable.Equals("ASEGURADORAS")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Aseguradora.Campo_Aseguradora_ID + " AS ASEGURADORA_ID, " + Cat_Pat_Aseguradora.Campo_Nombre_Fiscal + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora + " ORDER BY " + Cat_Pat_Aseguradora.Campo_Nombre;
                } else if (Vehiculo.P_Tipo_DataTable.Equals("VEHICULOS")) {
                    Mi_SQL = "SELECT " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS VEHICULO_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " AS NUMERO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Economico + " AS NUMERO_ECONOMICO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre + " AS VEHICULO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo + " AS MODELO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Anio_Fabricacion + " AS ANIO";
                    Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " = " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID;
                    if (Vehiculo.P_Numero_Inventario > (-1)) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario;
                        Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Numero_Inventario + "'";
                    }
                    if (Vehiculo.P_Numero_Economico_ != null && Vehiculo.P_Numero_Economico_.Trim().Length>0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Economico;
                        Mi_SQL = Mi_SQL + " LIKE '%" + Vehiculo.P_Numero_Economico_.Trim() + "%'";
                    }
                    if (Vehiculo.P_Modelo_ID != null && Vehiculo.P_Modelo_ID.Trim().Length>0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo;
                        Mi_SQL = Mi_SQL + " LIKE '%" + Vehiculo.P_Modelo_ID.Trim() + "%'";
                    }
                    if (Vehiculo.P_Marca_ID != null && Vehiculo.P_Marca_ID.Trim().Length>0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Marca_ID.Trim() + "'";
                    }
                    if (Vehiculo.P_Tipo_Vehiculo_ID != null && Vehiculo.P_Tipo_Vehiculo_ID.Trim().Length>0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Tipo_Vehiculo_ID.Trim() + "'";
                    }
                    if (Vehiculo.P_Tipo_Combustible_ID != null && Vehiculo.P_Tipo_Combustible_ID.Trim().Length>0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Combustible_ID;
                        Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Tipo_Combustible_ID.Trim() + "'";
                    }
                    if (Vehiculo.P_Anio_Fabricacion > (-1)) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Anio_Fabricacion;
                        Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Anio_Fabricacion + "'";
                    }
                    if (Vehiculo.P_Color_ID != null && Vehiculo.P_Color_ID.Trim().Length>0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Color_ID.Trim() + "'";
                    }
                    if (Vehiculo.P_Zona_ID != null && Vehiculo.P_Zona_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Zona_ID;
                        Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Zona_ID.Trim() + "'";
                    }
                    if (Vehiculo.P_Estatus != null && Vehiculo.P_Estatus.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Estatus.Trim() + "'";
                    }
                    if (Vehiculo.P_Dependencia_ID != null && Vehiculo.P_Dependencia_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Dependencia_ID.Trim() + "'";
                    }
                    if (Vehiculo.P_RFC_Resguardante != null && Vehiculo.P_RFC_Resguardante.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_RFC + " LIKE '%" + Vehiculo.P_RFC_Resguardante + "%' )";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'" + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO')";
                    }
                    if (Vehiculo.P_No_Empleado != null && Vehiculo.P_No_Empleado.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Vehiculo.P_No_Empleado), 6) + "' )";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'" + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO')";
                    }
                    if (Vehiculo.P_Resguardante_ID != null && Vehiculo.P_Resguardante_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Vehiculo.P_Resguardante_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'" + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO')";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre;
                }

                //SE OBTIENEN LOS RESULTADOS
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) { Ds_Vehiculo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL); } 
                if (Ds_Vehiculo != null) { Dt_Vehiculo = Ds_Vehiculo.Tables[0]; }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Vehiculo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Vehiculo
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Vehiculo.
        ///PARAMETROS:     
        ///             1. Vehiculo. Contiene los parametros para actualizar el registro
        ///                         en la Base de Datos.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 03/Diciembre/2010 
        ///MODIFICO             : Salvador Hernández  Ramírez
        ///FECHA_MODIFICO       : 09/Febrero/2011 
        ///CAUSA_MODIFICACIÓN   : Se implementó el método "Alta_Bitacora" para capturar los Insert y Update en la tabla "APL_BITACORA" de la Base de Datos
        ///*******************************************************************************
        public static void Modificar_Vehiculo(Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                Cls_Ope_Pat_Com_Vehiculos_Negocio Temporal_1 = Consultar_Detalles_Vehiculo(Vehiculo);
                String Mi_SQL = "UPDATE " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Vehiculos.Campo_Numero_Economico + " = '" + Vehiculo.P_Numero_Economico_ + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Dependencia_ID+ " = '" + Vehiculo.P_Dependencia_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Placas + " = '" + Vehiculo.P_Placas + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Color_ID + " = '" + Vehiculo.P_Color_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Estatus + " = '" + Vehiculo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_No_Factura + " = '" + Vehiculo.P_No_Factura_ + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Capacidad_Carga + " = '" + Vehiculo.P_Capacidad_Carga + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = '" + Vehiculo.P_Tipo_Vehiculo_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Tipo_Combustible_ID + " = '" + Vehiculo.P_Tipo_Combustible_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Anio_Fabricacion + " = '" + Vehiculo.P_Anio_Fabricacion + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + " = '" + Vehiculo.P_Serie_Carroceria + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Numero_Cilindros + " = '" + Vehiculo.P_Numero_Cilindros + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Kilometraje + " = '" + Vehiculo.P_Kilometraje + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " = '" + String.Format("{0:dd/MM/yyyy}", Vehiculo.P_Fecha_Adquisicion) + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Odometro + " = '" + Vehiculo.P_Odometro + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Zona_ID + " = '" + Vehiculo.P_Zona_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Observaciones + " = '" + Vehiculo.P_Observaciones + "'";
                if (!Vehiculo.P_Estatus.Equals("VIGENTE")) {
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Motivo_Baja + " = '" + Vehiculo.P_Motivo_Baja + "'";
                }
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Proveedor_ID + " = '" + Vehiculo.P_Proveedor_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Clase_Activo_ID + " = '" + Vehiculo.P_Clase_Activo_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Clasificacion_ID + " = '" + Vehiculo.P_Clasificacion_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Empleado_Operador_ID + " = '" + Vehiculo.P_Empleado_Operador + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Empleado_Recibe_ID + " = '" + Vehiculo.P_Empleado_Funcionario_Recibe + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Empleado_Autorizo_ID + " = '" + Vehiculo.P_Empleado_Autorizo + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Usuario_Modifico + " = '" + Vehiculo.P_Usuario_Nombre + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Vehiculos.aspx", Vehiculo.P_Vehiculo_ID, Mi_SQL);  // Se da de alta el update en la tabla "APL_BITACORA" de la BD

                if (Vehiculo.P_Estatus.Trim().Equals("VIGENTE")) {

                    Cls_Ope_Pat_Com_Vehiculos_Negocio Temporal = Obtener_Diferencia_Resguardos(Temporal_1, Vehiculo);

                    //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                    for (Int32 Contador = 0; Contador < Temporal.P_Resguardantes.Rows.Count; Contador++) {
                        Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Vehiculo.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal.P_Resguardantes.Rows[Contador][0].ToString() + "'";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        String Bien_Resguardo_ID = ""+ Temporal.P_Resguardantes.Rows[Contador][0].ToString();
                        //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Vehiculos.aspx", Bien_Resguardo_ID, Mi_SQL);  // Se da de alta el Update en la tabla "APL_BITACORA" de la BD
                    }

                    Cls_Ope_Pat_Com_Vehiculos_Negocio Temporal_2 = Obtener_Diferencia_Resguardos(Vehiculo, Temporal_1);

                    //SE DAN DE ALTA LOS NUEVOS RESGUARDANTES
                    if (Temporal_2.P_Resguardantes != null && Temporal_2.P_Resguardantes.Rows.Count > 0) {
                        String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 50);
                        for (Int32 Cnt = 0; Cnt < Temporal_2.P_Resguardantes.Rows.Count; Cnt++) {
                            Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estado + ", " + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Modificacion + ", " + Ope_Pat_Bienes_Resguardos.Campo_Observaciones;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Vehiculo.P_Vehiculo_ID + "', 'VEHICULO','" + Temporal_2.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Usuario_ID + "', 'VIGENTE', '" + Temporal_2.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                            Mi_SQL = Mi_SQL + ",'','" + Vehiculo.P_Dependencia_ID + "'";
                            Mi_SQL = Mi_SQL + ",'SI','" + Vehiculo.P_Observaciones + "'";
                            Mi_SQL = Mi_SQL + ",'" + Vehiculo.P_Usuario_Nombre + "', SYSDATE)";
                            ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Actualizacion_Vehiculos.aspx", ID_Consecutivo, Mi_SQL);  // Se da de alta el Insert en la tabla "APL_BITACORA" de la BD
                        }
                    }
                } else {
                    //Boolean Aseguradora_Nueva = Nueva_Aseguradora(Vehiculo);
                    //if (Aseguradora_Nueva) {
                    //    Mi_SQL = "UPDATE " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                    //    Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + " = 'BAJA'";
                    //    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Usuario_Modifico + " = '" + Vehiculo.P_Usuario_Nombre + "'";
                    //    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Fecha_Modifico + " = SYSDATE";
                    //    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID + "=" + Vehiculo.P_Vehiculo_Aseguradora_ID + "";
                    //    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + "= 'VIGENTE'";
                    //    Cmd.CommandText = Mi_SQL;
                    //    Cmd.ExecuteNonQuery();
                    //    //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Vehiculos.aspx", Convert.ToString(Vehiculo.P_Vehiculo_Aseguradora_ID), Mi_SQL);  // Se da de alta el Update en la tabla "APL_BITACORA" de la BD

                    //    String Vehiculo_Aseguradora_ID = Obtener_ID_Consecutivo(Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora, Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID, 50);
                    //    Mi_SQL = "INSERT INTO " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                    //    Mi_SQL = Mi_SQL + " (" + Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Aseguradora_ID + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Tipo_Vehiculo_ID;
                    //    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Descripcion_Seguro + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Cobertura + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_No_Poliza;
                    //    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Usuario_Creo + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Fecha_Creo;
                    //    Mi_SQL = Mi_SQL + ") VALUES (" + Convert.ToInt32(Vehiculo_Aseguradora_ID) + ", '" + Vehiculo.P_Aseguradora_ID + "', '" + Vehiculo.P_Vehiculo_ID + "'";
                    //    Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Descripcion_Seguro + "', '" + Vehiculo.P_Cobertura_Seguro + "', '" + Vehiculo.P_No_Poliza_Seguro + "'";
                    //    Mi_SQL = Mi_SQL + ", 'VIGENTE', '" + Vehiculo.P_Usuario_Nombre + "', SYSDATE)";
                    //    Cmd.CommandText = Mi_SQL;
                    //    Cmd.ExecuteNonQuery();
                    //    //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Actualizacion_Vehiculos.aspx", Vehiculo_Aseguradora_ID, Mi_SQL);  // Se da de alta el Update en la tabla "APL_BITACORA" de la BD
                    //} else {
                    //    Mi_SQL = "UPDATE " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                    //    Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Vehiculo_Aseguradora.Campo_No_Poliza + " = '" + Vehiculo.P_No_Poliza_Seguro + "'";
                    //    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Descripcion_Seguro + " = '" + Vehiculo.P_Descripcion_Seguro + "'";
                    //    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Cobertura + " = '" + Vehiculo.P_Cobertura_Seguro + "'";
                    //    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Usuario_Modifico + " = '" + Vehiculo.P_Usuario_Nombre + "'";
                    //    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Fecha_Modifico + " = SYSDATE";
                    //    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID + "=" + Vehiculo.P_Vehiculo_Aseguradora_ID + "";
                    //    Cmd.CommandText = Mi_SQL;
                    //    Cmd.ExecuteNonQuery();
                    //    //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Vehiculos.aspx", Convert.ToString(Vehiculo.P_Vehiculo_Aseguradora_ID), Mi_SQL);  // Se da de alta el Update en la tabla "APL_BITACORA" de la BD
                    //}
                    for (Int32 Contador = 0; Contador < Temporal_1.P_Resguardantes.Rows.Count; Contador++) {
                        Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja + " = 'SI'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Vehiculo.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString() + "'";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        String Bien_Resguardo_ID = "" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString();
                        //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Vehiculos.aspx", Bien_Resguardo_ID, Mi_SQL);  // Se da de alta el Update en la tabla "APL_BITACORA" de la BD
                    }
                }

                String ID_Consecutivo_Archivo = "";
                if (Vehiculo.P_Archivo != null && Vehiculo.P_Archivo.Trim().Length > 0) {
                    ID_Consecutivo_Archivo = Obtener_ID_Consecutivo(Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes, Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID, 50);
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes + " ( " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha_Creo + " ) VALUES ( " + Convert.ToInt32(ID_Consecutivo_Archivo) + ", '" + Vehiculo.P_Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + " , 'VEHICULO', SYSDATE, '" + Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Vehiculo.P_Archivo) + "', 'NORMAL', '" + Vehiculo.P_Usuario_Nombre + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }

                //SE CARGA LA ACTUALIZACIÓN DE LOS DETALLES DEL VEHICULO
                Mi_SQL = "DELETE FROM " + Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles + " WHERE " + Cat_Pat_Vehiculo_Detalles.Campo_Vehiculo_ID + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Vehiculo.P_Dt_Detalles != null && Vehiculo.P_Dt_Detalles.Rows.Count >0) {
                    Int32 Registro_ID = Convert.ToInt32(Obtener_ID_Consecutivo(Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles, Cat_Pat_Vehiculo_Detalles.Campo_Registro_ID, 100));
                    for (Int32 Contador = 0; Contador < (Vehiculo.P_Dt_Detalles.Rows.Count); Contador++) {
                        Mi_SQL = "INSERT INTO " + Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles +
                                 " ( " + Cat_Pat_Vehiculo_Detalles.Campo_Registro_ID + ", " + Cat_Pat_Vehiculo_Detalles.Campo_Vehiculo_ID + ", " + Cat_Pat_Vehiculo_Detalles.Campo_Detalle_Vehiculo_ID +
                                 ", " + Cat_Pat_Vehiculo_Detalles.Campo_Estado + ", " + Cat_Pat_Vehiculo_Detalles.Campo_Usuario_Creo + ", " + Cat_Pat_Vehiculo_Detalles.Campo_Fecha_Creo + ") " +
                                 "VALUES ( '" + Registro_ID + "','" + Vehiculo.P_Vehiculo_ID + "', '" + Vehiculo.P_Dt_Detalles.Rows[Contador]["DETALLE_ID"].ToString() +
                                 "', '" + Vehiculo.P_Dt_Detalles.Rows[Contador]["ESTADO"].ToString() + "', '" + Vehiculo.P_Usuario_Nombre + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Registro_ID = Registro_ID + 1;
                    }
                }

                Trans.Commit();
                if (Vehiculo.P_Archivo != null && Vehiculo.P_Archivo.Trim().Length > 0) {
                    Vehiculo.P_Archivo = Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Vehiculo.P_Archivo);
                }
                Actualizar_Partes_Vehiculos(Vehiculo);
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 2627)  {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar Modificar el Vehiculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Vehiculo
        ///DESCRIPCIÓN: Obtiene los Datos a Detalle de un Vehiculo en Especifico.
        ///PARAMETROS:   
        ///             1. Parametros.   Vehiculo que se va a ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Vehiculos_Negocio Consultar_Datos_Vehiculo(Cls_Ope_Pat_Com_Vehiculos_Negocio Parametros)
        {
            String Mi_SQL = "SELECT " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " AS VEHICULO_ID";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " AS NUMERO_INVENTARIO";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Economico + " AS NUMERO_ECONOMICO";
            Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
            Mi_SQL = Mi_SQL + " || ' - ' || " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " AS TIPO_VEHICULO";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Combustible_ID + " AS TIPO_COMBUSTIBLE";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Color_ID + " AS COLOR";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Zona_ID + " AS ZONA";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Placas + " AS PLACAS";
            //Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Clave_Programatica_Revision + " AS CLAVE_PROGRAMATICA_REVISION";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Producto_ID + " AS PRODUCTO_ID";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Costo_Actual + " AS COSTO_ACTUAL";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Capacidad_Carga + " AS CAPACIDAD_CARGA";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Anio_Fabricacion + " AS ANIO_FABRICACION";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria + " AS SERIE_CARROCERIA";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Serie_Motor + " AS SERIE_MOTOR";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Cilindros + " AS NUMERO_CILINDROS";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Ejes + " AS NUMERO_EJES";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion + " AS FECHA_ADQUISICION";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Kilometraje + " AS KILOMETRAJE";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Estatus + " AS ESTATUS";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Odometro + " AS ODOMETRO";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Motivo_Baja + " AS MOTIVO_BAJA";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Observaciones + " AS OBSERVACIONES";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Cantidad + " AS CANTIDAD";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + ", " + Cat_Dependencias.Tabla_Cat_Dependencias;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
            Mi_SQL = Mi_SQL + " = " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID + "";
            if (!Parametros.P_Buscar_Numero_Inventario) {
                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + "";
                Mi_SQL = Mi_SQL + " = '" + Parametros.P_Vehiculo_ID + "'";
            } else {
                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + "";
                Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Inventario + "'";
            }
            Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
            OracleDataReader Data_Reader;
            try {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read()) {
                    Vehiculo.P_Vehiculo_ID = (Data_Reader["VEHICULO_ID"] != null) ? Data_Reader["VEHICULO_ID"].ToString() : "";
                    Vehiculo.P_Numero_Inventario = (Data_Reader["NUMERO_INVENTARIO"] != null) ? Convert.ToInt32(Data_Reader["NUMERO_INVENTARIO"]) : 0;
                    Vehiculo.P_Numero_Economico = (Data_Reader["NUMERO_ECONOMICO"] != null) ? Convert.ToInt32(Data_Reader["NUMERO_ECONOMICO"]) : 0;
                    Vehiculo.P_Producto_ID = (Data_Reader["PRODUCTO_ID"] != null) ? Data_Reader["PRODUCTO_ID"].ToString() : "";
                    Vehiculo.P_Dependencia_ID = (Data_Reader["DEPENDENCIA"] != null) ? Data_Reader["DEPENDENCIA"].ToString() : " - ";
                    Vehiculo.P_Tipo_Vehiculo_ID = (Data_Reader["TIPO_VEHICULO"] != null) ? Data_Reader["TIPO_VEHICULO"].ToString() : "";
                    Vehiculo.P_Tipo_Combustible_ID = (Data_Reader["TIPO_COMBUSTIBLE"] != null) ? Data_Reader["TIPO_COMBUSTIBLE"].ToString() : "";
                    Vehiculo.P_Color_ID = (Data_Reader["COLOR"] != null) ? Data_Reader["COLOR"].ToString() : "";
                    Vehiculo.P_Zona_ID = (Data_Reader["ZONA"] != null) ? Data_Reader["ZONA"].ToString() : "";
                    Vehiculo.P_Placas = (Data_Reader["PLACAS"] != null) ? Data_Reader["PLACAS"].ToString() : "";
                    //Vehiculo.P_Clave_Programatica_Revision = (Data_Reader["CLAVE_PROGRAMATICA_REVISION"] != null) ? Convert.ToInt32(Data_Reader["CLAVE_PROGRAMATICA_REVISION"]) : 0;
                    Vehiculo.P_Costo_Actual = (Data_Reader["COSTO_ACTUAL"] != null) ? Convert.ToDouble(Data_Reader["COSTO_ACTUAL"]) : 0;
                    Vehiculo.P_Capacidad_Carga = (Data_Reader["CAPACIDAD_CARGA"] != null) ? (Data_Reader["CAPACIDAD_CARGA"]).ToString() : "";
                    Vehiculo.P_Anio_Fabricacion = (Data_Reader["ANIO_FABRICACION"] != null) ? Convert.ToInt32(Data_Reader["ANIO_FABRICACION"]) : 0;
                    Vehiculo.P_Serie_Carroceria = (Data_Reader["SERIE_CARROCERIA"] != null) ? Data_Reader["SERIE_CARROCERIA"].ToString() : "";
                    Vehiculo.P_Serie_Motor = (Data_Reader["SERIE_MOTOR"] != null) ? Data_Reader["SERIE_MOTOR"].ToString() : "";
                    Vehiculo.P_Numero_Cilindros = (Data_Reader["NUMERO_CILINDROS"] != null) ? Convert.ToInt32(Data_Reader["NUMERO_CILINDROS"]) : 0;
                    Vehiculo.P_Numero_Ejes = (Data_Reader["NUMERO_EJES"] != null) ? Convert.ToInt32(Data_Reader["NUMERO_EJES"]) : 0;
                    Vehiculo.P_Fecha_Adquisicion = (Data_Reader["FECHA_ADQUISICION"] != null) ? Convert.ToDateTime(Data_Reader["FECHA_ADQUISICION"]) : new DateTime();
                    Vehiculo.P_Kilometraje = (Data_Reader["KILOMETRAJE"] != null) ? Convert.ToDouble(Data_Reader["KILOMETRAJE"]) : 0;
                    Vehiculo.P_Estatus = (Data_Reader["ESTATUS"] != null) ? Data_Reader["ESTATUS"].ToString() : "";
                    Vehiculo.P_Odometro = (Data_Reader["ODOMETRO"] != null) ? Data_Reader["ODOMETRO"].ToString() : "";
                    Vehiculo.P_Motivo_Baja = (Data_Reader["MOTIVO_BAJA"] != null) ? Data_Reader["MOTIVO_BAJA"].ToString() : "";
                    Vehiculo.P_Observaciones = (Data_Reader["OBSERVACIONES"] != null) ? Data_Reader["OBSERVACIONES"].ToString() : "";
                    Vehiculo.P_Cantidad = (Data_Reader["CANTIDAD"] != null) ? Convert.ToInt32(Data_Reader["CANTIDAD"]) : 1;
                }
                Data_Reader.Close();
                //OBTIENE DATOS MAS GENERALES DEL VEHICULO
                if (Vehiculo.P_Producto_ID != null && Vehiculo.P_Producto_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " || ' - ' || " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + "";
                    Mi_SQL = Mi_SQL + " || ' - ' || " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " AS MODELO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Costo + " AS COSTO_INICIAL";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + "";
                    Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Producto_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Modelo_ID + "";
                } else {
                    Mi_SQL = "SELECT " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " || ' - ' || " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + "";
                    Mi_SQL = Mi_SQL + " || ' - ' || " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " AS MODELO";
                    Mi_SQL = Mi_SQL + ", 0.0 AS COSTO_INICIAL";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + "";
                    Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo_ID + "";                  
                }
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read()) {
                    Vehiculo.P_Marca_ID = (Data_Reader["MARCA"] != null) ? Data_Reader["MARCA"].ToString() : "";
                    Vehiculo.P_Modelo_ID = (Data_Reader["MODELO"] != null) ? Data_Reader["MODELO"].ToString() : "";
                    Vehiculo.P_Costo_Inicial = (Data_Reader["COSTO_INICIAL"] != null) ? Convert.ToDouble(Data_Reader["COSTO_INICIAL"]) : 0;
                } 
                Data_Reader.Close();

                //OBTIENE LOS DETALLES DE LA ASEGURADORA DEL VEHICULO
                if (Vehiculo.P_Tipo_Vehiculo_ID != null && Vehiculo.P_Tipo_Vehiculo_ID.Trim().Length > 0) {
                    if (Vehiculo.P_Tipo_Vehiculo_ID != null && Vehiculo.P_Tipo_Vehiculo_ID.Trim().Length > 0) {
                        Mi_SQL = "SELECT " + Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID + " AS VEHICULO_ASEGURADORA_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Aseguradora_ID + " AS ASEGURADORA_ID";
                        //Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Tipo_Vehiculo_ID + " AS VEHICULO_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Descripcion_Seguro + " AS DESCRIPCION_SEGURO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Cobertura + " AS COBERTURA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_No_Poliza + " AS NO_POLIZA"; ;
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculo_Aseguradora.Campo_Tipo_Vehiculo_ID + " = '" + Vehiculo.P_Tipo_Vehiculo_ID + "'";
                        Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        while (Data_Reader.Read()) {
                            Vehiculo.P_Vehiculo_Aseguradora_ID = (Data_Reader["VEHICULO_ASEGURADORA_ID"] != null) ? Convert.ToInt32(Data_Reader["VEHICULO_ASEGURADORA_ID"]) : 0;
                            Vehiculo.P_Aseguradora_ID = (Data_Reader["ASEGURADORA_ID"] != null) ? Data_Reader["ASEGURADORA_ID"].ToString() : "";
                            //Vehiculo.P_Vehiculo_ID = (Data_Reader["VEHICULO_ID"] != null) ? Data_Reader["VEHICULO_ID"].ToString() : "";
                            Vehiculo.P_Descripcion_Seguro = (Data_Reader["DESCRIPCION_SEGURO"] != null) ? Data_Reader["DESCRIPCION_SEGURO"].ToString() : "";
                            Vehiculo.P_Cobertura_Seguro = (Data_Reader["COBERTURA"] != null) ? Data_Reader["COBERTURA"].ToString() : "";
                            Vehiculo.P_No_Poliza_Seguro = (Data_Reader["NO_POLIZA"] != null) ? Data_Reader["NO_POLIZA"].ToString() : "";
                        }
                        Data_Reader.Close();                        
                    }
                }

                //OBTIENE LOS RESGUARDANTES DEL VEHICULO
                DataSet Ds_Vehiculo = null;
                if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                    Mi_SQL = Mi_SQL + " = 'VIGENTE'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                    Mi_SQL = Mi_SQL + " = 'VEHICULO'";
                    Ds_Vehiculo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Vehiculo == null) {
                    Vehiculo.P_Resguardantes = new DataTable();
                } else {
                    Vehiculo.P_Resguardantes = Ds_Vehiculo.Tables[0];
                }

                //OBTIENE LOS RESGUARDANTES ANTERIORES DEL VEHICULO
                Ds_Vehiculo = null;
                if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " AS FECHA_INICIAL";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " AS FECHA_FINAL";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                    Mi_SQL = Mi_SQL + " = 'BAJA'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                    Mi_SQL = Mi_SQL + " = 'VEHICULO'";
                    Ds_Vehiculo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Vehiculo == null) {
                    Vehiculo.P_Historial_Resguardos = new DataTable();
                } else {
                    Vehiculo.P_Historial_Resguardos = Ds_Vehiculo.Tables[0];
                }

                Ds_Vehiculo = null;
                if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0)
                {
                    Mi_SQL = "SELECT " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID + " AS ARCHIVO_BIEN_ID, " + Ope_Pat_Archivos_Bienes.Campo_Fecha + " AS FECHA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + " AS ARCHIVO, '' AS DESCRIPCION FROM " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Archivos_Bienes.Campo_Tipo + " = 'VEHICULO' AND " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + "='" + Vehiculo.P_Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Archivos_Bienes.Campo_Fecha + " DESC";
                    Ds_Vehiculo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Vehiculo == null) {
                    Vehiculo.P_Dt_Historial_Archivos = new DataTable();
                } else {
                    Vehiculo.P_Dt_Historial_Archivos = Ds_Vehiculo.Tables[0];
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los datos del Vehiculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Vehiculo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Detalles_Vehiculo
        ///DESCRIPCIÓN: Obtiene los Datos a Detalle de un Vehiculo en Especifico.
        ///PARAMETROS:   
        ///             1. Parametros.   Vehiculo que se va a ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Julio/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Vehiculos_Negocio Consultar_Detalles_Vehiculo(Cls_Ope_Pat_Com_Vehiculos_Negocio Parametros) {
            String Mi_SQL = "SELECT * FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
            if (!Parametros.P_Buscar_Numero_Inventario) {
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + "";
                Mi_SQL = Mi_SQL + " = '" + Parametros.P_Vehiculo_ID + "'";
            } else {
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + "";
                Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Inventario + "'";
            }
            Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
            OracleDataReader Data_Reader;
            try {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read()) {
                    Vehiculo.P_Vehiculo_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Vehiculo_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Vehiculo_ID].ToString() : "";
                    Vehiculo.P_Nombre_Producto = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Nombre].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Nombre].ToString() : "";
                    Vehiculo.P_Numero_Inventario = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Numero_Inventario].ToString())) ? Convert.ToInt32(Data_Reader[Ope_Pat_Vehiculos.Campo_Numero_Inventario]) : 0;
                    Vehiculo.P_Numero_Economico_ = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Numero_Economico].ToString())) ? (Data_Reader[Ope_Pat_Vehiculos.Campo_Numero_Economico]).ToString() : "";
                    Vehiculo.P_Producto_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Producto_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Producto_ID].ToString() : "";
                    Vehiculo.P_Dependencia_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Dependencia_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Dependencia_ID].ToString() : "";
                    Vehiculo.P_Tipo_Vehiculo_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID].ToString() : "";
                    Vehiculo.P_Tipo_Combustible_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Tipo_Combustible_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Tipo_Combustible_ID].ToString() : "";
                    Vehiculo.P_Color_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Color_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Color_ID].ToString() : "";
                    Vehiculo.P_Zona_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Zona_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Zona_ID].ToString() : "";
                    Vehiculo.P_Placas = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Placas].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Placas].ToString() : "";
                    Vehiculo.P_Costo_Inicial = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Costo_Alta].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Vehiculos.Campo_Costo_Alta]) : 0;
                    Vehiculo.P_Costo_Actual = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Costo_Actual].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Vehiculos.Campo_Costo_Actual]) : 0;
                    Vehiculo.P_Capacidad_Carga = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Capacidad_Carga].ToString())) ? (Data_Reader[Ope_Pat_Vehiculos.Campo_Capacidad_Carga]).ToString() : "";
                    Vehiculo.P_Anio_Fabricacion = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Anio_Fabricacion].ToString())) ? Convert.ToInt32(Data_Reader[Ope_Pat_Vehiculos.Campo_Anio_Fabricacion]) : 0;
                    Vehiculo.P_Serie_Carroceria = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Serie_Carroceria].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Serie_Carroceria].ToString() : "";
                    Vehiculo.P_Numero_Cilindros = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Numero_Cilindros].ToString())) ? Convert.ToInt32(Data_Reader[Ope_Pat_Vehiculos.Campo_Numero_Cilindros]) : 0;
                    Vehiculo.P_Fecha_Adquisicion = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion].ToString())) ? Convert.ToDateTime(Data_Reader[Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion]) : new DateTime();
                    Vehiculo.P_Kilometraje = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Kilometraje].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Vehiculos.Campo_Kilometraje]) : 0;
                    Vehiculo.P_Estatus = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Estatus].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Estatus].ToString() : "";
                    Vehiculo.P_Odometro = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Odometro].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Odometro].ToString() : "";
                    Vehiculo.P_Motivo_Baja = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Motivo_Baja].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Motivo_Baja].ToString() : "";
                    Vehiculo.P_Observaciones = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Observaciones].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Observaciones].ToString() : "";
                    Vehiculo.P_Cantidad = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Cantidad].ToString())) ? Convert.ToInt32(Data_Reader[Ope_Pat_Vehiculos.Campo_Cantidad]) : 1;
                    Vehiculo.P_Marca_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Marca_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Marca_ID].ToString() : "";
                    Vehiculo.P_Modelo_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Modelo].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Modelo].ToString() : "";
                    Vehiculo.P_No_Factura_ = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_No_Factura].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_No_Factura].ToString() : "";
                    Vehiculo.P_Proveedor_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Proveedor_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Proveedor_ID].ToString() : "";
                    Vehiculo.P_Empleado_Operador = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Empleado_Operador_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Empleado_Operador_ID].ToString() : "";
                    Vehiculo.P_Empleado_Funcionario_Recibe = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Empleado_Recibe_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Empleado_Recibe_ID].ToString() : "";
                    Vehiculo.P_Empleado_Autorizo = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Empleado_Autorizo_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Empleado_Autorizo_ID].ToString() : "";
                    Vehiculo.P_Clase_Activo_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Clase_Activo_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Clase_Activo_ID].ToString() : "";
                    Vehiculo.P_Clasificacion_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Vehiculos.Campo_Clasificacion_ID].ToString())) ? Data_Reader[Ope_Pat_Vehiculos.Campo_Clasificacion_ID].ToString() : "";
                }
                Data_Reader.Close();
                if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT (" + Ope_Pat_Vehiculos.Campo_Usuario_Creo + " ||' ['|| TO_CHAR(" + Ope_Pat_Vehiculos.Campo_Fecha_Creo + ", 'DD/MM/YYYY') ||']') AS CREO";
                    Mi_SQL = Mi_SQL + ", (" + Ope_Pat_Vehiculos.Campo_Usuario_Modifico + " ||' ['|| TO_CHAR(" + Ope_Pat_Vehiculos.Campo_Fecha_Modifico + ", 'DD/MM/YYYY') ||']') AS MODIFICO";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                     Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                     while (Data_Reader.Read()) {
                         Vehiculo.P_Dato_Creacion = (!String.IsNullOrEmpty(Data_Reader["CREO"].ToString())) ? Data_Reader["CREO"].ToString() : "";
                         Vehiculo.P_Dato_Modificacion = (!String.IsNullOrEmpty(Data_Reader["MODIFICO"].ToString())) ? Data_Reader["MODIFICO"].ToString() : "";
                     }
                     Data_Reader.Close();
                }
                //OBTIENE LOS DETALLES DE LA ASEGURADORA DEL VEHICULO
                if (Vehiculo.P_Tipo_Vehiculo_ID != null && Vehiculo.P_Tipo_Vehiculo_ID.Trim().Length > 0) {
                    if (Vehiculo.P_Tipo_Vehiculo_ID != null && Vehiculo.P_Tipo_Vehiculo_ID.Trim().Length > 0) {
                        Mi_SQL = "SELECT " + Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID + " AS VEHICULO_ASEGURADORA_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Aseguradora_ID + " AS ASEGURADORA_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Descripcion_Seguro + " AS DESCRIPCION_SEGURO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Cobertura + " AS COBERTURA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_No_Poliza + " AS NO_POLIZA"; ;
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculo_Aseguradora.Campo_Tipo_Vehiculo_ID + " = '" + Vehiculo.P_Tipo_Vehiculo_ID + "'";
                        Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        while (Data_Reader.Read()) {
                            Vehiculo.P_Vehiculo_Aseguradora_ID = (Data_Reader["VEHICULO_ASEGURADORA_ID"] != null) ? Convert.ToInt32(Data_Reader["VEHICULO_ASEGURADORA_ID"]) : 0;
                            Vehiculo.P_Aseguradora_ID = (Data_Reader["ASEGURADORA_ID"] != null) ? Data_Reader["ASEGURADORA_ID"].ToString() : "";
                            Vehiculo.P_Descripcion_Seguro = (Data_Reader["DESCRIPCION_SEGURO"] != null) ? Data_Reader["DESCRIPCION_SEGURO"].ToString() : "";
                            Vehiculo.P_Cobertura_Seguro = (Data_Reader["COBERTURA"] != null) ? Data_Reader["COBERTURA"].ToString() : "";
                            Vehiculo.P_No_Poliza_Seguro = (Data_Reader["NO_POLIZA"] != null) ? Data_Reader["NO_POLIZA"].ToString() : "";
                        }
                        Data_Reader.Close();                        
                    }
                }

                //OBTIENE LOS RESGUARDANTES DEL VEHICULO
                DataSet Ds_Vehiculo = null;
                if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                    Mi_SQL = Mi_SQL + "," + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                    Mi_SQL = Mi_SQL + " = 'VIGENTE'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                    Mi_SQL = Mi_SQL + " = 'VEHICULO'";
                    Ds_Vehiculo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Vehiculo == null) {
                    Vehiculo.P_Resguardantes = new DataTable();
                } else {
                    Vehiculo.P_Resguardantes = Ds_Vehiculo.Tables[0];
                }

                //OBTIENE LOS RESGUARDANTES ANTERIORES DEL VEHICULO
                Ds_Vehiculo = null;
                if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " AS FECHA_INICIAL";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " AS FECHA_FINAL";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                    Mi_SQL = Mi_SQL + " = 'BAJA'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                    Mi_SQL = Mi_SQL + " = 'VEHICULO'";
                    Ds_Vehiculo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Vehiculo == null) {
                    Vehiculo.P_Historial_Resguardos = new DataTable();
                } else {
                    Vehiculo.P_Historial_Resguardos = Ds_Vehiculo.Tables[0];
                }
                //OBTIENE LOS ARCHIVOS QUE HAN SIDO CARGADOS AL VEHICULO
                Ds_Vehiculo = null;
                if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0)
                {
                    Mi_SQL = "SELECT " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID + " AS ARCHIVO_BIEN_ID, " + Ope_Pat_Archivos_Bienes.Campo_Fecha + " AS FECHA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + " AS ARCHIVO, '' AS DESCRIPCION FROM " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Archivos_Bienes.Campo_Tipo + " = 'VEHICULO' AND " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + "='" + Vehiculo.P_Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Archivos_Bienes.Campo_Fecha + " DESC";
                    Ds_Vehiculo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Vehiculo == null) {
                    Vehiculo.P_Dt_Historial_Archivos = new DataTable();
                } else {
                    Vehiculo.P_Dt_Historial_Archivos = Ds_Vehiculo.Tables[0];
                }
                //OBTIENE LOS DETALLES DEL VEHICULO
                Ds_Vehiculo = null;
                if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles + "." + Cat_Pat_Vehiculo_Detalles.Campo_Detalle_Vehiculo_ID + " AS DETALLE_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Det_Vehiculos.Tabla_Cat_Pat_Det_Vehiculos + "." + Cat_Pat_Det_Vehiculos.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles + "." + Cat_Pat_Vehiculo_Detalles.Campo_Estado + " AS ESTADO";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Det_Vehiculos.Tabla_Cat_Pat_Det_Vehiculos + ", " + Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Det_Vehiculos.Tabla_Cat_Pat_Det_Vehiculos + "." + Cat_Pat_Det_Vehiculos.Campo_Detalle_Vehiculo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles + "." + Cat_Pat_Vehiculo_Detalles.Campo_Detalle_Vehiculo_ID;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pat_Vehiculo_Detalles.Tabla_Cat_Pat_Vehiculo_Detalles + "." + Cat_Pat_Vehiculo_Detalles.Campo_Vehiculo_ID;
                    Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                    Ds_Vehiculo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Vehiculo == null) {
                    Vehiculo.P_Dt_Detalles = new DataTable();
                } else {
                    Vehiculo.P_Dt_Detalles = Ds_Vehiculo.Tables[0];
                }
                //OBTIENE LAS PARTES DE LOS VEHICULOS [BIENES MUEBLES QUE SON PARTE DEL VEHICULO]
                Ds_Vehiculo = null;
                if (Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_MUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS NO_INVENTARIO_ANTERIOR";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " AS NO_INVENTARIO_SIAS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " AS MODELO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS ESTADO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " AS ESTATUS";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + " ON " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Parent_ID;
                    Mi_SQL = Mi_SQL + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveniente;
                    Mi_SQL = Mi_SQL + " = 'VEHICULO'";
                    Ds_Vehiculo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Vehiculo == null) {
                    Vehiculo.P_Dt_Partes_Vehiculo = new DataTable();
                } else {
                    Vehiculo.P_Dt_Partes_Vehiculo = Ds_Vehiculo.Tables[0];
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los datos del Vehiculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Vehiculo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Diferencia_Resguardos
        ///DESCRIPCIÓN: Saca la diferencia de unos resguardantes a otros.
        ///PARAMETROS:     
        ///             1. Actuales.        Bien Mueble como esta actualmente en la Base de Datos.
        ///             2. Actualizados.    Bien Mueble como quiere que quede al Actualizarlo.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Cls_Ope_Pat_Com_Vehiculos_Negocio Obtener_Diferencia_Resguardos(Cls_Ope_Pat_Com_Vehiculos_Negocio Comparar, Cls_Ope_Pat_Com_Vehiculos_Negocio Base_Comparacion)
        {
            Cls_Ope_Pat_Com_Vehiculos_Negocio Resguardos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();

            DataTable Dt_Tabla = new DataTable();
            Dt_Tabla.Columns.Add("BIEN_RESGUARDO_ID", Type.GetType("System.Int32"));
            Dt_Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
            Dt_Tabla.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
            Dt_Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));
            for (int Contador_1 = 0; Contador_1 < Comparar.P_Resguardantes.Rows.Count; Contador_1++)
            {
                Boolean Eliminar = true;
                for (int Contador_2 = 0; Contador_2 < Base_Comparacion.P_Resguardantes.Rows.Count; Contador_2++)
                {
                    if (Comparar.P_Resguardantes.Rows[Contador_1][1].ToString().Equals(Base_Comparacion.P_Resguardantes.Rows[Contador_2][1].ToString()))
                    {
                        Eliminar = false;
                        break;
                    }
                }
                if (Eliminar)
                {
                    DataRow Fila = Dt_Tabla.NewRow();
                    Fila["BIEN_RESGUARDO_ID"] = Convert.ToInt32(Comparar.P_Resguardantes.Rows[Contador_1][0]);
                    Fila["EMPLEADO_ID"] = Comparar.P_Resguardantes.Rows[Contador_1][1].ToString();
                    Fila["NOMBRE_EMPLEADO"] = Comparar.P_Resguardantes.Rows[Contador_1][2].ToString();
                    Fila["COMENTARIOS"] = Comparar.P_Resguardantes.Rows[Contador_1][3].ToString();
                    Dt_Tabla.Rows.Add(Fila);
                }
            }
            Resguardos.P_Resguardantes = Dt_Tabla;
            return Resguardos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Nueva_Aseguradora
        ///DESCRIPCIÓN: Verifica si la Aseguradora es nueva para el Vehiculo o es la misma.
        ///PARAMETROS:     
        ///             1. Parametros.  Datos del Vehiculo.
        ///FECHA_CREO: 09/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Boolean Nueva_Aseguradora(Cls_Ope_Pat_Com_Vehiculos_Negocio Parametros)
        {
            Boolean Nueva = true;
            try
            {
                String Mi_SQL = "SELECT " + Ope_Pat_Vehiculo_Aseguradora.Campo_Aseguradora_ID + " FROM " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculo_Aseguradora.Campo_Tipo_Vehiculo_ID + " = '" + Parametros.P_Vehiculo_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + " = 'VIGENTE'";
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if ((Obj_Temp != null) && !(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    if (Parametros.P_Aseguradora_ID.Equals(Obj_Temp.ToString().Trim()))
                    {
                        Nueva = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error: [" + Ex.Message + "]";
                throw new Exception(Mensaje);
            }
            return Nueva;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
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
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
        {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
            {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Partes_Vehiculos
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARÁMETROS:     
        ///             1. Vehiculo. Objeto del cual se actualizarán sus registros.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        private static void Actualizar_Partes_Vehiculos(Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo) {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            String Mi_SQL = null;
            DataTable Dt_Datos = null;
            DataSet Ds_Datos = null;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {
                Mi_SQL = "SELECT * FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Campo_Proveniente + " = 'VEHICULO'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = 'VIGENTE'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Campo_Bien_Parent_ID + " = '" + Vehiculo.P_Vehiculo_ID + "'";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count > 0) {
                    Dt_Datos = Ds_Datos.Tables[0];
                    for (Int32 Contador = 0; Contador < Dt_Datos.Rows.Count; Contador++) {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio BM_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                        BM_Negocio.P_Bien_Mueble_ID = Dt_Datos.Rows[Contador][Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID].ToString();
                        BM_Negocio = BM_Negocio.Consultar_Detalles_Bien_Mueble();
                        BM_Negocio.P_Resguardantes = Vehiculo.P_Resguardantes;
                        BM_Negocio.P_Dependencia_ID = Vehiculo.P_Dependencia_ID;
                        BM_Negocio.P_Usuario_Nombre = Vehiculo.P_Usuario_Nombre;
                        BM_Negocio.Modificar_Bien_Mueble();
                    }
                }
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar Modificar. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }            
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Consecutivo_Inventario
        /// DESCRIPCION:            Método utilizado para consultar el No de Invetario correspondiente
        /// PARAMETROS :            
        ///                         
        /// CREO       :            Salvador Hernandez Ramirez
        /// FECHA_CREO :            08/Julio/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static Int64 Consulta_Consecutivo_Inventario() {
            String Mi_SQL = String.Empty; //Variable para las consultas
            Object Consecutivo;
            Int64 No_Consecutivo;

            try {
                // Consulta 
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Pat_Inv_Vehiculos.Campo_Inventario + "), 0) ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Pat_Inv_Vehiculos.Tabla_Ope_Alm_Pat_Inv_Vehiculos;
                
                //Ejecutar consulta
                Consecutivo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Verificar si no es nulo
                if (Consecutivo != null && Convert.IsDBNull(Consecutivo) == false) { No_Consecutivo = Convert.ToInt64(Consecutivo) + 1; }
                else { No_Consecutivo = 1; }

                return No_Consecutivo;
            } catch (OracleException ex) {
                throw new Exception("Error: " + ex.Message);
            } catch (DBConcurrencyException ex) {
                throw new Exception("Error: " + ex.Message);
            } catch (Exception ex) {
                throw new Exception("Error: " + ex.Message);
            } finally {
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE: Alta_Migrar_Vehiculo
        /// DESCRIPCION:Da de Alta los Vehiculos al Migrar la Información del Proyecto.
        /// PARAMETROS :  Vehiculo. Información del Vehiculo a Migrar.       
        /// CREO       :Francisco Antonio Gallardo Castañeda.
        /// FECHA_CREO :29/Agosto/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static Cls_Ope_Pat_Com_Vehiculos_Negocio Alta_Migrar_Vehiculo(Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo) {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_SQL = "";

            try {
                
                //Alta de Vehiculos.
                String Vehiculo_ID = Obtener_ID_Consecutivo(Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos, Ope_Pat_Vehiculos.Campo_Vehiculo_ID, 10);
                Mi_SQL = "INSERT INTO " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Producto_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Tipo_Combustible_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Color_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Zona_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Numero_Inventario;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Numero_Economico;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Placas;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Anio_Fabricacion;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Serie_Carroceria;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Serie_Motor;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Odometro;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Marca_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Modelo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Procedencia;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Observaciones;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Cantidad ;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculos.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Vehiculo_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Producto_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Dependencia_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Tipo_Vehiculo_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Tipo_Combustible_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Color_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Zona_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Numero_Inventario+ "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Numero_Economico_ + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Placas + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Anio_Fabricacion + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Serie_Carroceria + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Serie_Motor + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Odometro + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Nombre_Producto + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Marca_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Modelo_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Procedencia + "'";
                Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Observaciones + "'";
                Mi_SQL = Mi_SQL + ", '1'";
                Mi_SQL = Mi_SQL + ", 'FERNANDO AYALA CASTAÑEDA'";
                Mi_SQL = Mi_SQL + ", SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Alta de los Resguardos
                if (Vehiculo.P_Resguardantes != null && Vehiculo.P_Resguardantes.Rows.Count > 0){
                    Int32 ID_Consecutivo = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 20));
                    for (Int32 Cnt = 0; Cnt < Vehiculo.P_Resguardantes.Rows.Count; Cnt++) {
                        Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES ('" + ID_Consecutivo + "'";
                        Mi_SQL = Mi_SQL + ", '" + Vehiculo_ID + "'";
                        Mi_SQL = Mi_SQL + ", 'VEHICULO'";
                        Mi_SQL = Mi_SQL + ", '" + Vehiculo.P_Resguardantes.Rows[Cnt]["EMPLEADO_ID"].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE";
                        Mi_SQL = Mi_SQL + ", 'LA FECHA INICIAL DEL RESGUARDO ES LA FECHA EN QUE SE DIO DE ALTA EN EL SISTEMA SIAS YA QUE NO SE TENIA LA FECHA INICIAL REAL.'";
                        Mi_SQL = Mi_SQL + ", 'VIGENTE'";
                        Mi_SQL = Mi_SQL + ", 'FERNANDO AYALA CASTAÑEDA'";
                        Mi_SQL = Mi_SQL + ", SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        ID_Consecutivo = ID_Consecutivo + 1;
                    }
                }

                // Asignar al Inventario
                Int64 Inventario_ID = Convert.ToInt64(Obtener_ID_Consecutivo(Ope_Alm_Pat_Inv_Vehiculos.Tabla_Ope_Alm_Pat_Inv_Vehiculos, Ope_Alm_Pat_Inv_Vehiculos.Campo_No_Inventario, 25));
                Mi_SQL = "INSERT INTO " + Ope_Alm_Pat_Inv_Vehiculos.Tabla_Ope_Alm_Pat_Inv_Vehiculos + " (";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_No_Inventario + ", "; // Es el No de  contra recibo
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_Inventario + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_Producto_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_Marca_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_Modelo + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_Color_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_No_Serie + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Vehiculos.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES(" + Inventario_ID + ", ";
                Mi_SQL = Mi_SQL + Vehiculo.P_Numero_Inventario + ", ";
                Mi_SQL = Mi_SQL + "'" + Vehiculo.P_Nombre_Producto + "', ";
                Mi_SQL = Mi_SQL + "'" + Vehiculo.P_Producto_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Vehiculo.P_Marca_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Vehiculo.P_Modelo_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Vehiculo.P_Color_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Vehiculo.P_Serie_Carroceria + "', ";
                Mi_SQL = Mi_SQL + "'" + Vehiculo.P_Usuario_Nombre + "', ";
                Mi_SQL = Mi_SQL + "SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
               
                Trans.Commit();
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]"; 
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]"; 
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else {
                    Mensaje = "Error al intentar dar de Alta Vehiculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }finally {
                 Cn.Close();
            }
            return Vehiculo;
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Empleados_Resguardos
        ///DESCRIPCIÓN          : Obtiene empleados de la Base de Datos y los regresa en un 
        ///                       DataTable de acuerdo a los filtros pasados.
        ///PARAMETROS           : Parametros.  Contiene los parametros que se van a utilizar para
        ///                                         hacer la consulta de la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 24/Octubre/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static DataTable Consultar_Empleados_Resguardos(Cls_Ope_Pat_Com_Vehiculos_Negocio Parametros) {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            try {
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
                if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID.Trim() + "'";
                }
                if (Parametros.P_RFC_Resguardante != null && Parametros.P_RFC_Resguardante.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + " = '" + Parametros.P_RFC_Resguardante.Trim() + "'";
                }
                if (Parametros.P_No_Empleado_Resguardante != null && Parametros.P_No_Empleado_Resguardante.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Parametros.P_No_Empleado_Resguardante.Trim()), 6) + "'";
                }
                if (Parametros.P_Nombre_Resguardante != null && Parametros.P_Nombre_Resguardante.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND (TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") LIKE '%" + Parametros.P_Nombre_Resguardante.Trim() + "%'";
                    Mi_SQL = Mi_SQL + " OR TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") LIKE '%" + Parametros.P_Nombre_Resguardante.Trim() + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY NOMBRE";
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

    }

}