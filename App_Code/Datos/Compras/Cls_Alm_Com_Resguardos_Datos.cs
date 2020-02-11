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
using Presidencia.Almacen_Resguardos.Negocio;
using Presidencia.Almacen_Resguardos.Datos;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;


/// <summary>
/// Summary description for Cls_Alm_Com_Resguardos_Datos
/// </summary>
namespace Presidencia.Almacen_Resguardos.Datos
{
    public class Cls_Alm_Com_Resguardos_Datos
    {
        public Cls_Alm_Com_Resguardos_Datos()
        {
        }

        #region Metodos
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Resguardos_Bienes
        ///DESCRIPCIÓN:             Realiza una consulta a la base de datos para buscar informacion 
        ///                         sobre el resguardo del bien asiganado a diversos empleados.
        ///PARAMETROS:              1.-Negocio, objeto de la clase de Negocio que contiene los datos para realizar la consulta
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              17/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Resguardos_Bienes(Cls_Alm_Com_Resguardos_Negocio Negocio, Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Id) {
           
            try {
                String Mi_SQL = "SELECT " +
                "  BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS CLAVE_SISTEMa" +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Modelo +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Garantia +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + " as FECHA_INVENTARIO" +
                ", DEPENDENCIA." + Cat_Dependencias.Campo_Nombre + " as DEPENDENCIA" +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Area_ID +
                ", AREAS." + Cat_Areas.Campo_Nombre + " as AREA" +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Producto_ID +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " as PRODUCTO" +
                ", PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + " as PROVEEDOR " +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " AS OPERACION " +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " AS MODELO " +
                ", ZONAS." + Cat_Pat_Zonas.Campo_Descripcion + " AS ZONA " +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS CLASIFICACION ";      
                          if (Negocio.P_Operacion == "RESGUARDO")
                          {
                              Mi_SQL = Mi_SQL + ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie +
                              ", COLORES." + Cat_Pat_Colores.Campo_Descripcion + " as COLOR" +
                              ", MATERIALES." + Cat_Pat_Materiales.Campo_Descripcion + " as MATERIAL" +
                              ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Estado +
                              ", DECODE(BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Estatus + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS" +
                              ", NVL(BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ",1) AS CANTIDAD" +
                              ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " as COSTO_UNITARIO" +
                              ", NVL(BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + ") as FECHA_MODIFICO" +
                              ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Inventario +
                              ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Factura +
                              ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Observadores +
                              ", NVL(BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Modifico + ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") AS USUARIO_CREO" +
                              ", EMPLEADOS." + Cat_Empleados.Campo_Nombre + " as NOMBRE_E" +
                              ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " as APELLIDO_PATERNO_E" +
                              ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " as APELLIDO_MATERNO_E" +
                              ", EMPLEADOS." + Cat_Empleados.Campo_RFC + " as RFC_E" +
                              ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID +
                              ", PROCEDENCIAS." + Cat_Pat_Procedencias.Campo_Nombre + " as PROCEDENCIA" +
                              ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion +
                              ",( select " + Cat_Com_Marcas.Campo_Nombre + "  from " +
                               Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " Where " + Cat_Com_Marcas.Campo_Marca_ID +
                               " = BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " ) as MARCA " +
                              " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " BIENES_M" +
                              " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " BIENES_R " +
                              " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "= BIENES_R." +
                              Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " AND " + " BIENES_R." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + "='BIEN_MUEBLE'" +
                             " AND " + " BIENES_R." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + "='VIGENTE'" +
                               " LEFT OUTER JOIN " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + " PROCEDENCIAS" +
                              " ON PROCEDENCIAS." + Cat_Pat_Procedencias.Campo_Procedencia_ID + "= BIENES_M." +
                              Ope_Pat_Bienes_Muebles.Campo_Procedencia +
                              " LEFT OUTER JOIN " + Cat_Areas.Tabla_Cat_Areas + " AREAS" +
                              " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Area_ID + "= AREAS." +
                              Cat_Areas.Campo_Area_ID +
                              " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES" +
                              " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + "= COLORES." +
                              Cat_Pat_Colores.Campo_Color_ID +
                              " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " MATERIALES" +
                              " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + "= MATERIALES." +
                              Cat_Pat_Materiales.Campo_Material_ID +
                              " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES" +
                              " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + "= PROVEEDORES." +
                              Cat_Com_Proveedores.Campo_Proveedor_ID + 
                              " LEFT OUTER JOIN " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + " ZONAS" +
                              " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + "= ZONAS." +
                              Cat_Pat_Zonas.Campo_Zona_ID + 
                              " FULL OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS" +
                             " ON BIENES_R." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "= EMPLEADOS." +
                             Cat_Empleados.Campo_Empleado_ID +
                              " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA " +
                              " ON EMPLEADOS." + Cat_Empleados.Campo_Dependencia_ID + "= DEPENDENCIA." +
                              Cat_Dependencias.Campo_Dependencia_ID +
                             " WHERE " + " BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "='" + Bien_Id.P_Bien_Mueble_ID + "'";
                }
                else if (Negocio.P_Operacion == "RECIBO")
                {
                        Mi_SQL = Mi_SQL + ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie +
                        ", COLORES." + Cat_Pat_Colores.Campo_Descripcion + " as COLOR" +
                        ", MATERIALES." + Cat_Pat_Materiales.Campo_Descripcion + " as MATERIAL" +
                        ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Estado +
                        ", DECODE(BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Estatus + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS" +
                        ", NVL(BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ",1) AS CANTIDAD"+
                        ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " as COSTO_UNITARIO" +
                        ", NVL(BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo +") as FECHA_MODIFICO" +
                        ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Inventario +
                        ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Factura +
                        ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Observadores +
                        ", NVL(BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Modifico + ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ") AS USUARIO_CREO" +
                        ", EMPLEADOS." + Cat_Empleados.Campo_Nombre + " as NOMBRE_E" +
                        ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " as APELLIDO_PATERNO_E" +
                        ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " as APELLIDO_MATERNO_E" +
                        ", EMPLEADOS." + Cat_Empleados.Campo_RFC + " as RFC_E" +
                        ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID +
                        ", PROCEDENCIAS." + Cat_Pat_Procedencias.Campo_Nombre + " as PROCEDENCIA" +
                        ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion +
                        ",( select " + Cat_Com_Marcas.Campo_Nombre + "  from " +
                        Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " Where " + Cat_Com_Marcas.Campo_Marca_ID +
                        " = BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " ) as MARCA " +
                         " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " BIENES_M " +
                        " LEFT OUTER JOIN " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "  BIENES_R " +
                        " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "= BIENES_R." +
                        Ope_Pat_Bienes_Recibos.Campo_Bien_ID + 
                        " AND " + " BIENES_R." + Ope_Pat_Bienes_Recibos.Campo_Tipo + "='BIEN_MUEBLE'" +
                        " AND " + " BIENES_R." + Ope_Pat_Bienes_Recibos.Campo_Estatus + "='VIGENTE'" +                      
                        // " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA " +
                        //" ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + "= DEPENDENCIA." +
                        //Cat_Dependencias.Campo_Dependencia_ID +
                         " LEFT OUTER JOIN " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + " PROCEDENCIAS" +
                        " ON PROCEDENCIAS." + Cat_Pat_Procedencias.Campo_Procedencia_ID + "= BIENES_M." +
                        Ope_Pat_Bienes_Muebles.Campo_Procedencia +
                        " LEFT JOIN " + Cat_Areas.Tabla_Cat_Areas + " AREAS" +
                        " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Area_ID + "= AREAS." +
                        Cat_Areas.Campo_Area_ID +
                        " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES" +
                        " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + "= COLORES." +
                        Cat_Pat_Colores.Campo_Color_ID +
                        " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " MATERIALES" +
                        " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + "= MATERIALES." +
                        Cat_Pat_Materiales.Campo_Material_ID +
                          " LEFT JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES" +
                          " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + "= PROVEEDORES." +
                          Cat_Com_Proveedores.Campo_Proveedor_ID +
                        " LEFT JOIN " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + " ZONAS" +
                        " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + "= ZONAS." +
                        Cat_Pat_Zonas.Campo_Zona_ID +
                        " FULL OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS" +
                       " ON BIENES_R." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + "= EMPLEADOS." +
                       Cat_Empleados.Campo_Empleado_ID +
                      " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA " +
                      " ON EMPLEADOS." + Cat_Empleados.Campo_Dependencia_ID + "= DEPENDENCIA." +
                      Cat_Dependencias.Campo_Dependencia_ID +
                       " WHERE " + " BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " ='" + Bien_Id.P_Bien_Mueble_ID + "'" ;
                }
                          DataTable Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                          return Data_Set;

            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Resguardos_Bienes
        ///DESCRIPCIÓN:             Realiza una consulta a la base de datos para buscar informacion 
        ///                         sobre el resguardo del bien asiganado a diversos empleados.
        ///PARAMETROS:              1.-Negocio, objeto de la clase de Negocio que contiene los datos para realizar la consulta
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              17/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consulta_Resguardos_Bienes2(Cls_Alm_Com_Resguardos_Negocio Negocio, Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Id)
        {

            try
            {
                String Mi_SQL = "SELECT " +
                "  BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Clave_Sistema +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + " as FECHA_INVENTARIO" +
                ", DEPENDENCIA." + Cat_Dependencias.Campo_Nombre + " as DEPENDENCIA" +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Area_ID +
                ", AREAS." + Cat_Areas.Campo_Nombre + " as AREA" +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Producto_ID +
                ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " as PRODUCTO" +
                ", PROVEEDORES." + Cat_Com_Proveedores.Campo_Compañia + " as PROVEEDOR " +
                ", PROVEEDORES." + Cat_Com_Proveedores.Campo_Compañia + " AS OPERACION " +
                ",( select " + Cat_Com_Productos.Campo_Descripcion + "  from " +
                 Cat_Com_Productos.Tabla_Cat_Com_Productos + " Where " + Cat_Com_Productos.Campo_Producto_ID +
                 " = BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Producto_ID + " ) as CLASIFICACION ";   // La descripción del producto se asigna al campo CLASIFICACION

                if (Negocio.P_Operacion == "RESGUARDO")
                {
                    Mi_SQL = Mi_SQL + ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie +
                    ", COLORES." + Cat_Pat_Colores.Campo_Descripcion + " as COLOR" +
                    ", MATERIALES." + Cat_Pat_Materiales.Campo_Descripcion + " as MATERIAL" +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Estado +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Estatus +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Cantidad +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " as COSTO_UNITARIO" +
                    ", BIENES_R." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " as FECHA_MODIFICO" +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Inventario +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Factura +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Observadores +
                    ", BIENES_R." + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo +
                    ", EMPLEADOS." + Cat_Empleados.Campo_Nombre + " as NOMBRE_E" +
                    ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " as APELLIDO_PATERNO_E" +
                    ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " as APELLIDO_MATERNO_E" +
                    ", EMPLEADOS." + Cat_Empleados.Campo_RFC + " as RFC_E" +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID +
                    ", PROCEDENCIAS." + Cat_Pat_Procedencias.Campo_Nombre + " as PROCEDENCIA" +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion +
                    ",( select " + Cat_Com_Marcas.Campo_Nombre + "  from " +
                     Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " Where " + Cat_Com_Marcas.Campo_Marca_ID +
                     " = BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " ) as MARCA " +
                    ",( select " + Cat_Com_Modelos.Campo_Nombre + "  from " +
                     Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " Where " + Cat_Com_Modelos.Campo_Modelo_ID +
                     " = BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Modelo_ID + " ) as MODELO " +
                    " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " BIENES_R" +
                    " JOIN " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " BIENES_M " +
                    " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "= BIENES_R." +
                    Ope_Pat_Bienes_Resguardos.Campo_Bien_ID +
                    " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA " +
                    " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + "= DEPENDENCIA." +
                    Cat_Dependencias.Campo_Dependencia_ID +
                     " JOIN " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + " PROCEDENCIAS" +
                    " ON PROCEDENCIAS." + Cat_Pat_Procedencias.Campo_Procedencia_ID + "= BIENES_M." +
                    Ope_Pat_Bienes_Muebles.Campo_Procedencia +
                    " JOIN " + Cat_Areas.Tabla_Cat_Areas + " AREAS" +
                    " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Area_ID + "= AREAS." +
                    Cat_Areas.Campo_Area_ID +
                    " JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES" +
                    " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + "= COLORES." +
                    Cat_Pat_Colores.Campo_Color_ID +
                    " JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " MATERIALES" +
                    " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + "= MATERIALES." +
                    Cat_Pat_Materiales.Campo_Material_ID +
                    " JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES" +
                    " ON BIENES_M." + Cat_Com_Modelos_Productos.Campo_Proveedor_ID + "= PROVEEDORES." +
                    Cat_Com_Proveedores.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + " JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS" +
                   " ON BIENES_R." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "= EMPLEADOS." +
                   Cat_Empleados.Campo_Empleado_ID +
                   " WHERE " + " BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "='" + Bien_Id.P_Bien_Mueble_ID + "'" + "AND " + " BIENES_R." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + "='" + "BIEN_MUEBLE" + "'" +
                   " AND " + " BIENES_R." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + "='VIGENTE'";
                }
                else if (Negocio.P_Operacion == "RECIBO")
                {
                    Mi_SQL = Mi_SQL + ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie +
                    ", COLORES." + Cat_Pat_Colores.Campo_Descripcion + " as COLOR" +
                    ", MATERIALES." + Cat_Pat_Materiales.Campo_Descripcion + " as MATERIAL" +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Estado +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Estatus +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Cantidad +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " as COSTO_UNITARIO" +
                    ", BIENES_R." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial + " as FECHA_MODIFICO" +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Inventario +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Factura +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Observadores +
                    ", BIENES_R." + Ope_Pat_Bienes_Recibos.Campo_Usuario_Creo +
                    ", EMPLEADOS." + Cat_Empleados.Campo_Nombre + " as NOMBRE_E" +
                    ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " as APELLIDO_PATERNO_E" +
                    ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " as APELLIDO_MATERNO_E" +
                    ", EMPLEADOS." + Cat_Empleados.Campo_RFC + " as RFC_E" +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID +
                    ", PROCEDENCIAS." + Cat_Pat_Procedencias.Campo_Nombre + " as PROCEDENCIA" +
                    ", BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion +
                    ",( select " + Cat_Com_Marcas.Campo_Nombre + "  from " +
                    Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " Where " + Cat_Com_Marcas.Campo_Marca_ID +
                    " = BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " ) as MARCA " +
                    ",( select " + Cat_Com_Modelos.Campo_Nombre + "  from " +
                    Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " Where " + Cat_Com_Modelos.Campo_Modelo_ID +
                    " = BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Modelo_ID + " ) as MODELO " +
                    " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "  BIENES_R " +
                    " JOIN " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " BIENES_M " +
                    " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "= BIENES_R." +
                    Ope_Pat_Bienes_Recibos.Campo_Bien_ID +
                     " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA " +
                    " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + "= DEPENDENCIA." +
                    Cat_Dependencias.Campo_Dependencia_ID +
                     " JOIN " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + " PROCEDENCIAS" +
                    " ON PROCEDENCIAS." + Cat_Pat_Procedencias.Campo_Procedencia_ID + "= BIENES_M." +
                    Ope_Pat_Bienes_Muebles.Campo_Procedencia +
                    " JOIN " + Cat_Areas.Tabla_Cat_Areas + " AREAS" +
                    " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Area_ID + "= AREAS." +
                    Cat_Areas.Campo_Area_ID +
                    " JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES" +
                    " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + "= COLORES." +
                    Cat_Pat_Colores.Campo_Color_ID +
                    " JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " MATERIALES" +
                    " ON BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + "= MATERIALES." +
                    Cat_Pat_Materiales.Campo_Material_ID +
                    " JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES" +
                    " ON BIENES_M." + Cat_Com_Modelos_Productos.Campo_Proveedor_ID + " = PROVEEDORES." +
                    Cat_Com_Proveedores.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + " JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS" +
                   " ON BIENES_R." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + "= EMPLEADOS." +
                   Cat_Empleados.Campo_Empleado_ID +
                   " WHERE " + " BIENES_M." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " ='" + Bien_Id.P_Bien_Mueble_ID + "'" + "AND " + " BIENES_R." + Ope_Pat_Bienes_Recibos.Campo_Tipo + "='" + "BIEN_MUEBLE" + "'" +
                   " AND " + " BIENES_R." + Ope_Pat_Bienes_Recibos.Campo_Estatus + "='VIGENTE'";
                }
                DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                return Data_Set;
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:        Consulta_Resguardos_Vehiculos
        ///DESCRIPCIÓN:                 Realiza una consulta a la base de datos para buscar informacion 
        ///                             sobre el resguardo del vehiculo asiganado a diversos empleados.
        ///PARAMETROS:                  1.-Negocio, objeto de la clase de Negocio que contiene los datos para realizar la consulta
        ///CREO:                        Salvador Hernández Ramírez
        ///FECHA_CREO:                  23/Diciembre/2010 
        ///MODIFICO:                    Salvador Hernández Ramírez
        ///FECHA_MODIFICO:              05/Febrero/2011
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consulta_Resguardos_Vehiculos(Cls_Alm_Com_Resguardos_Negocio Negocio, Cls_Ope_Pat_Com_Vehiculos_Negocio Id_Vehiculo)
        {
            try
            {
                     String Mi_SQL = "SELECT " +
                    "  VEHICULOS." + Ope_Pat_Vehiculos.Campo_Numero_Economico +
                    ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID +
                    ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Numero_Inventario +
                    ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Dependencia_ID +
                    ", DEPENDENCIA." + Cat_Dependencias.Campo_Nombre + " as NOMBRE_DEPENDENCIA" +
                    ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Producto_ID +
                    ", MARCAS." + Cat_Com_Marcas.Campo_Nombre + " as NOMBRE_MARCA" +
                    ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Modelo + " as NOMBRE_MODELO" +
                    ", PROCEDENCIAS." + Cat_Pat_Procedencias.Campo_Nombre + " as PROCEDENCIA" +
                    ", PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + " as NOMBRE_PROVEEDOR";
                    


                if (Id_Vehiculo.P_Producto_Almacen) {
                    Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Modelos_Productos.Campo_Nombre + " as NOMBRE_PRODUCTO " +
                                      ", PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + " as NOMBRE_PROVEEDOR";
                } else {
                    Mi_SQL = Mi_SQL + ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Nombre + " as NOMBRE_PRODUCTO " +
                                      ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Proveniente + " as NOMBRE_PROVEEDOR";
                }
                    Mi_SQL = Mi_SQL + ", COLORES." + Cat_Pat_Colores.Campo_Descripcion + " as NOMBRE_COLOR" +
                    ", NVL(VEHICULOS." + Ope_Pat_Vehiculos.Campo_Cantidad + ",1) AS CANTIDAD" +
                    ", NVL(VEHICULOS." + Ope_Pat_Vehiculos.Campo_Costo_Actual + ",0) as COSTO_UNITARIO" +
                    ", T_VEHICULO." + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " as TIPO_VEHICULO" +
                    ", T_COMBUSTIBLE." + Cat_Pat_Tipos_Combustible.Campo_Descripcion + " as TIPO_COMBUSTIBLE" +
                    ", NVL(VEHICULOS." + Ope_Pat_Vehiculos.Campo_Kilometraje + ",0) as KILOMETRAJE" +
                    ", ZONAS." + Cat_Pat_Zonas.Campo_Descripcion + " as NOMBRE_ZONA" +
                    ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Placas +
                    ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Serie_Carroceria +
                    ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Serie_Motor +
                    ", NVL(VEHICULOS." + Ope_Pat_Vehiculos.Campo_Numero_Ejes + ",1) AS NUMERO_EJES" +
                    ", DECODE(VEHICULOS." + Ope_Pat_Vehiculos.Campo_Estatus + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS" +
                    ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Fecha_Adquisicion +
                    ", NVL(VEHICULOS." + Ope_Pat_Vehiculos.Campo_Fecha_Modifico + ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Fecha_Creo + ") as FECHA_MODIFICO" +
                    ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Observaciones +
                    ", NVL(VEHICULOS." + Ope_Pat_Vehiculos.Campo_Usuario_Modifico + ", VEHICULOS." + Ope_Pat_Vehiculos.Campo_Usuario_Creo + ") as ELABORO_FIRMA" +
                    ", NVL(VEHICULOS." + Ope_Pat_Vehiculos.Campo_Numero_Cilindros + ",1) AS NUMERO_CILINDROS" +
                    ", EMPLEADOS." + Cat_Empleados.Campo_Nombre + " as NOMBRE_E" +
                    ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " as APELLIDO_PATERNO_E" +
                    ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " as APELLIDO_MATERNO_E" +
                    ", EMPLEADOS." + Cat_Empleados.Campo_RFC + " as RFC_E" +

                    " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + " VEHICULOS" +
                    " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS" +
                    " ON VEHICULOS." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + "= RESGUARDOS." +
                    Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + 
                    " AND " + " RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + "='VEHICULO'" +
                    " AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + "='VIGENTE'" +
                    
                    " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA " +
                    " ON VEHICULOS." + Cat_Dependencias.Campo_Dependencia_ID + "= DEPENDENCIA." +
                    Cat_Dependencias.Campo_Dependencia_ID;
                
                        if (Id_Vehiculo.P_Producto_Almacen) {
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS " +
                            " ON VEHICULOS." + Ope_Pat_Vehiculos.Campo_Producto_ID + "= PRODUCTOS." +
                            Cat_Com_Productos.Campo_Producto_ID +
                            " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS " +
                            " ON PRODUCTOS." + Cat_Com_Productos.Campo_Marca_ID + "= MARCAS." +
                            Cat_Com_Marcas.Campo_Marca_ID +
                            " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES " +
                            " ON PRODUCTOS." + Cat_Com_Productos.Campo_Proveedor_ID + "= PROVEEDORES." +
                            Cat_Com_Proveedores.Campo_Proveedor_ID;
                        }  else {
                            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS " +
                            " ON VEHICULOS." + Ope_Pat_Vehiculos.Campo_Marca_ID + "= MARCAS." +
                            Cat_Com_Marcas.Campo_Marca_ID;
                        }

                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES " +
                        " ON VEHICULOS." + Ope_Pat_Vehiculos.Campo_Color_ID + "= COLORES." +
                        Cat_Pat_Colores.Campo_Color_ID +
                        " LEFT OUTER JOIN " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " T_VEHICULO " +
                        " ON VEHICULOS." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + "= T_VEHICULO." +
                        Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID +
                        " LEFT OUTER JOIN " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible + " T_COMBUSTIBLE " +
                        " ON VEHICULOS." + Ope_Pat_Vehiculos.Campo_Tipo_Combustible_ID + "= T_COMBUSTIBLE." +
                        Cat_Pat_Tipos_Combustible.Campo_Tipo_Combustible_ID +
                        " LEFT OUTER JOIN " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + " ZONAS " +
                        " ON VEHICULOS." + Ope_Pat_Vehiculos.Campo_Zona_ID + "= ZONAS." +
                        Cat_Pat_Zonas.Campo_Zona_ID +
                        " LEFT OUTER JOIN " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + " PROCEDENCIAS" +
                        " ON PROCEDENCIAS." + Cat_Pat_Procedencias.Campo_Procedencia_ID + "= VEHICULOS." +
                          Ope_Pat_Vehiculos.Campo_Procedencia +
                        " FULL OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS " +
                        " ON RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "= EMPLEADOS." +
                        Cat_Empleados.Campo_Empleado_ID +

                        " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES " +
                        " ON VEHICULOS." + Ope_Pat_Vehiculos.Campo_Proveedor_ID + "= PROVEEDORES." +
                        Cat_Com_Proveedores.Campo_Proveedor_ID +

                        " WHERE " + " VEHICULOS." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + "='" + Id_Vehiculo.P_Vehiculo_ID + "'";

                DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                return Data_Set;
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Vehiculos_Asegurados
        ///DESCRIPCIÓN:             Realiza una consulta a la base de datos para buscar informacion 
        ///                         sobre el resguardo del vehiculo asiganado a diversos empleados.
        ///PARAMETROS:              1.-Negocio, objeto de la clase de Negocio que contiene los datos para realizar la consulta
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              23/Diciembre/2010 
        ///MODIFICO:                Salvador Hernandez Ramirez
        ///FECHA_MODIFICO:          23/Mayo/2011
        ///CAUSA_MODIFICACIÓN:      Se modificó la consulta del seguro del vehiculo, 
        ///                         ya que hubo cambios en los campos de las tablas.
        ///*******************************************************************************
        public static DataSet Consulta_Vehiculos_Asegurados(Cls_Alm_Com_Resguardos_Negocio Negocio, Cls_Ope_Pat_Com_Vehiculos_Negocio Id_Vehiculo)
        {
            try {
                 String Mi_SQL = "SELECT " +
                 "  ASEGURADORAS." + Cat_Pat_Aseguradora.Campo_Nombre_Fiscal + " as NOMBRE_ASEGURADORA" +
                 ", V_ASEGURADORAS." + Ope_Pat_Vehiculo_Aseguradora.Campo_No_Poliza +
                 ", V_ASEGURADORAS." + Ope_Pat_Vehiculo_Aseguradora.Campo_Descripcion_Seguro +
                 ", V_ASEGURADORAS." + Ope_Pat_Vehiculo_Aseguradora.Campo_Cobertura +
                 " FROM " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora + " V_ASEGURADORAS" +
                 " JOIN " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora + " ASEGURADORAS " +
                 " ON V_ASEGURADORAS." + Ope_Pat_Vehiculo_Aseguradora.Campo_Aseguradora_ID + "= ASEGURADORAS." +
                 Cat_Pat_Aseguradora.Campo_Aseguradora_ID +
                 " JOIN " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + " VEHICULOS " +
                 " ON VEHICULOS." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = V_ASEGURADORAS." +
                 Ope_Pat_Vehiculo_Aseguradora.Campo_Tipo_Vehiculo_ID +
                 " WHERE " + " VEHICULOS." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + "='" + Id_Vehiculo.P_Vehiculo_ID + "'" + 
                 " AND V_ASEGURADORAS." + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus +"= 'VIGENTE'";

                DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                return Data_Set;
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Datos_Producto
        ///DESCRIPCIÓN:             Realiza una consulta a la base de datos para buscar informacion 
        ///                         sobre el producto que corresponde al bien mueble que se va a resguardar.
        ///PARAMETROS:              1.-Id_Producto, contiene el identificador del producto a consultar
        ///                         2.-No_Requisicion, contiene el numero de requisicion que solicito dicho producto
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              29/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consulta_Datos_Producto(String Id_Producto, String No_Requisicion)
        {
            try{
                 String Mi_SQL = "SELECT " +
                 "  PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID +
                 ", PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " as NOMBRE_PRODUCTO" +
                 ", MARCAS." + Cat_Com_Marcas.Campo_Nombre + " as NOMBRE_MARCA" +
                 ", MODELOS." + Cat_Com_Modelos.Campo_Nombre + " as NOMBRE_MODELO" +
                 ", PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + " as NOMBRE_PROVEEDOR" +
                 ", PRODUCTOS." + Cat_Com_Productos.Campo_Clave +
                 ", PRODUCTOS." + Cat_Com_Productos.Campo_Costo +
                 ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as NOMBRE_DEPENDENCIA" +
                 ", DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " as DEPENDENCIA_ID" +
                 " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS" +
                 " JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS " +
                 " ON PRODUCTOS." + Cat_Com_Productos.Campo_Marca_ID + "= MARCAS." +
                 Cat_Com_Marcas.Campo_Marca_ID +
                 " JOIN " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " MODELOS " +
                 " ON PRODUCTOS." + Cat_Com_Productos.Campo_Modelo_ID + "= MODELOS." +
                 Cat_Com_Modelos.Campo_Modelo_ID +
                 " JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES " +
                 " ON PRODUCTOS." + Cat_Com_Productos.Campo_Proveedor_ID + "= PROVEEDORES." +
                 Cat_Com_Proveedores.Campo_Proveedor_ID +
                 " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES " +
                 " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "=" +
                 No_Requisicion +
                 " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS " +
                 " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID + "= DEPENDENCIAS." +
                 Cat_Dependencias.Campo_Dependencia_ID +
                 " WHERE " + " PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + "='" + Id_Producto + "'" + "AND " + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + No_Requisicion + "'";

            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;

            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Resguardos_Cemovientes
        ///DESCRIPCIÓN:          Realiza una consulta a la base de datos para buscar informacion 
        ///                      sobre el resguardo del cemoviente asiganado a diversos empleados.
        ///PARAMETROS:           1.-Negocio, objeto de la clase de Negocio que contiene los datos para realizar la consulta
        ///                      2.- Id_Cemoviente, id del cemoviente que se va a consultar.
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           29/Diciembre/2010 
        ///MODIFICO:      
        ///FECHA_MODIFICO:       05/Febrero/2010 
        ///CAUSA_MODIFICACIÓN:   Se toma en cuenta en la consulta cuando el Anímal es donado
        ///*******************************************************************************
        public static DataSet Consulta_Resguardos_Cemovientes(Cls_Alm_Com_Resguardos_Negocio Negocio, Cls_Ope_Pat_Com_Cemovientes_Negocio Id_Cemoviente)
        {
            try{

                String Mi_SQL = "SELECT " +
                "  CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS CEMOVIENTE_ID" +
                ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Numero_Inventario +
                ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior + " AS INVENTARIO_ANTERIOR "+
                ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Dependencia_ID +
                ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as NOMBRE_DEPENDENCIA" +
                ", PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " as NOMBRE_PRODUCTO" +
                ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Producto_ID +
                ", PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + " as PROVEEDOR" +
                ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Nombre + " as NOMBRE_CEMOVIENTE" +
               ", T_CEMOVIENTE." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " as TIPO_CEMOVIENTE" +
               ", T_ADIESTRAMIENTO." + Cat_Pat_Tipos_Adiestramiento.Campo_Nombre + " as TIPO_ADIESTRAMIENTO" +
               ", RAZAS." + Cat_Pat_Razas.Campo_Nombre + " as NOMBRE_RAZA" +
               ", COLORES." + Cat_Pat_Colores.Campo_Descripcion + " as NOMBRE_COLOR" +
               ", T_ALIMENTACION." + Cat_Pat_Tipos_Alimentacion.Campo_Nombre + " as TIPO_ALIMENTACION" +
               ", FUNCIONES." + Cat_Pat_Funciones.Campo_Nombre + " as NOMBRE_FUNCION" +
               ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Sexo +
               ", DECODE(CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Estatus + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS" +
               ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Padre_ID + " as PADRE" +
               ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Madre_ID + " as MADRE" +
               ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia +
               ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Fecha_Nacimiento +
               ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion +
               ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Costo_Actual +
               ", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Observaciones +
               ", NVL(CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Usuario_Modifico +", CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Usuario_Creo +") AS USUARIO_CREO" +
               ", NVL(CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Cantidad + ",1) AS CANTIDAD"+
               ", PROCEDENCIAS." + Cat_Pat_Procedencias.Campo_Nombre + " AS PROCEDENCIA" +
               ", EMPLEADOS." + Cat_Empleados.Campo_Nombre + " as NOMBRE_E" +
               ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " as APELLIDO_PATERNO_E" +
               ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " as APELLIDO_MATERNO_E" +
               ", EMPLEADOS." + Cat_Empleados.Campo_RFC + " as RFC_E" +
               " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + " CEMOVIENTES" +
               " LEFT OUTER JOIN " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " BIENES_R " +
               " ON CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + "= BIENES_R." +
                Ope_Pat_Bienes_Resguardos.Campo_Bien_ID +
                " AND " + " BIENES_R." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + "='" + "CEMOVIENTE" + "'" +
                " AND " + " BIENES_R." + Ope_Pat_Bienes_Resguardos.Campo_Estatus + "='VIGENTE'"+
                " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS " +
                " ON CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Dependencia_ID + "= DEPENDENCIAS." +
                 Cat_Dependencias.Campo_Dependencia_ID +
                " LEFT OUTER JOIN " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + " T_CEMOVIENTE " +
                " ON CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + "= T_CEMOVIENTE." +
                 Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID +
                " LEFT OUTER JOIN " + Cat_Pat_Tipos_Adiestramiento.Tabla_Cat_Pat_Tipos_Adiestramiento + " T_ADIESTRAMIENTO " +
                " ON CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Tipo_Adiestramiento_ID + "= T_ADIESTRAMIENTO." +
                 Cat_Pat_Tipos_Adiestramiento.Campo_Tipo_Adiestramiento_ID +
                " LEFT OUTER JOIN " + Cat_Pat_Tipos_Alimentacion.Tabla_Cat_Pat_Tipos_Alimentacion + " T_ALIMENTACION " +
                " ON CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Tipo_Alimentacion_ID + "= T_ALIMENTACION." +
                 Cat_Pat_Tipos_Alimentacion.Campo_Tipo_Alimentacion_ID +
                " LEFT OUTER JOIN " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + " RAZAS " +
                " ON CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Raza_ID + "= RAZAS." +
                 Cat_Pat_Razas.Campo_Raza_ID +
                " LEFT OUTER JOIN " + Cat_Pat_Funciones.Tabla_Cat_Pat_Funciones + " FUNCIONES " +
                " ON CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Funcion_ID + "= FUNCIONES." +
                 Cat_Pat_Funciones.Campo_Funcion_ID +
                " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLORES " +
                " ON CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Color_ID + "= COLORES." +
                 Cat_Pat_Colores.Campo_Color_ID +
                " LEFT OUTER JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS " +
                " ON CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Producto_ID + "= PRODUCTOS." +
                 Cat_Com_Productos.Campo_Producto_ID +
                " FULL OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS " +
                " ON BIENES_R." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "= EMPLEADOS." +
                 Cat_Empleados.Campo_Empleado_ID +

                " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES " +
                " ON CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Proveedor_ID + "= PROVEEDORES." +
                Cat_Com_Proveedores.Campo_Proveedor_ID +

                " LEFT OUTER JOIN " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + " PROCEDENCIAS " +
                " ON CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Procedencia + "= PROCEDENCIAS." +
                Cat_Pat_Procedencias.Campo_Procedencia_ID +

                " WHERE " + " CEMOVIENTES." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + "='" + Id_Cemoviente.P_Cemoviente_ID + "'";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }

        #endregion
    }
}