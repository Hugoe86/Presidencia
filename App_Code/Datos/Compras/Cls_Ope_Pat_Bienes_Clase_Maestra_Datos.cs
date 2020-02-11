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
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Bienes_Clase_Maestra.Negocio;
using Presidencia.Constantes;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Ope_Pat_Bienes_Clase_Maestra_Datos
/// </summary>
namespace Presidencia.Control_Patrimonial_Bienes_Clase_Maestra.Datos {

    public class Cls_Ope_Pat_Bienes_Clase_Maestra_Datos {

        #region "Metodos"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Animales_General
            ///DESCRIPCIÓN          : Hace una consulta de los animales que hay registrados,
            ///                       y los regresa en una tabla. Con o sin filtros.
            ///PARAMETROS           : Parametros. Contiene los Filtros para la consulta.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 11/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Animales_General(Cls_Ope_Pat_Com_Cemovientes_Negocio Parametros) {
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                try {
                
                } catch (Exception Ex) {
                    throw new Exception("Excepcion al Consultar los Animales:'" + Ex.Message + "'");
                }
                return Dt_Datos; 
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Bienes_Muebles_General
            ///DESCRIPCIÓN          : Hace una consulta de los Bienes Muebles que hay registrados,
            ///                       y los regresa en una tabla. Con o sin filtros.
            ///PARAMETROS           : Bien_Mueble. Contiene los Filtros para la consulta.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 11/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Bienes_Muebles_General(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble)  {
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                try {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_MUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " AS NO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS NO_INVENTARIO_ANTERIOR";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS NOMBRE_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " AS COLOR";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS ESTADO";
                    Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " AS MODELO";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID;
                    Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
                    if (Bien_Mueble.P_Numero_Inventario_Anterior != null && Bien_Mueble.P_Numero_Inventario_Anterior.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior;
                        Mi_SQL = Mi_SQL + " LIKE '%" + Bien_Mueble.P_Numero_Inventario_Anterior + "%'";
                    }
                    if (Bien_Mueble.P_Nombre_Producto != null && Bien_Mueble.P_Nombre_Producto.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Nombre_Producto + "'";
                    }
                    if (Bien_Mueble.P_Dependencia_ID != null && Bien_Mueble.P_Dependencia_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Dependencia_ID + "'";
                    }
                    if (Bien_Mueble.P_Modelo != null && Bien_Mueble.P_Modelo.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo;
                        Mi_SQL = Mi_SQL + " LIKE '%" + Bien_Mueble.P_Modelo + "%'";
                    }
                    if (Bien_Mueble.P_Marca_ID != null && Bien_Mueble.P_Marca_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Marca_ID + "'";
                    }
                    if (Bien_Mueble.P_Estatus != null && Bien_Mueble.P_Estatus.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Estatus + "'";
                    }
                    if (Bien_Mueble.P_Estado != null && Bien_Mueble.P_Estado.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Estado + "'";
                    }
                    if (Bien_Mueble.P_Factura != null && Bien_Mueble.P_Factura.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Factura + "'";
                    }
                    if (Bien_Mueble.P_Numero_Serie != null && Bien_Mueble.P_Numero_Serie.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie;
                        Mi_SQL = Mi_SQL + " LIKE '%" + Bien_Mueble.P_Numero_Serie + "%'";
                    }
                    if (Bien_Mueble.P_RFC_Resguardante != null && Bien_Mueble.P_RFC_Resguardante.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Campo_RFC + " LIKE '%" + Bien_Mueble.P_RFC_Resguardante + "%' )";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'"+ " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE')";
                    }
                    if (Bien_Mueble.P_RFC_Resguardante != null && Bien_Mueble.P_RFC_Resguardante.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Campo_No_Empleado + " = '" + Bien_Mueble.P_No_Empleado_Resguardante + "' )";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'"+ " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE')";
                    }
                    if (Bien_Mueble.P_Resguardante_ID != null && Bien_Mueble.P_Resguardante_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Bien_Mueble.P_Resguardante_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'" + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE')";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre;
                } catch (Exception Ex) {
                    throw new Exception("Excepcion al Consultar los Animales:'" + Ex.Message + "'");
                }
                return Dt_Datos; 
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Vehiculos_General
            ///DESCRIPCIÓN          : Hace una consulta de los Vehículos que hay registrados,
            ///                       y los regresa en una tabla. Con o sin filtros.
            ///PARAMETROS           : Vehiculo. Contiene los Filtros para la consulta.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 11/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Vehiculos_General(Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo) {
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                try {
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
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Campo_RFC + " LIKE '%" + Vehiculo.P_RFC_Resguardante + "%' )";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'" + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO')";
                    }
                    if (Vehiculo.P_RFC_Resguardante != null && Vehiculo.P_RFC_Resguardante.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Campo_No_Empleado + " = '" + Vehiculo.P_No_Empleado_Resguardante + "' )";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'" + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO')";
                    }
                    if (Vehiculo.P_Resguardante_ID != null && Vehiculo.P_Resguardante_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Vehiculo.P_Resguardante_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'" + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO')";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre;
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                } catch (Exception Ex) {
                    throw new Exception("Excepcion al Consultar los Animales:'" + Ex.Message + "'");
                }
                return Dt_Datos; 
            }

        #endregion
        

    }

}
