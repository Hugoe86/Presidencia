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
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pat_Com_Bienes_Muebles_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio {

    public class Cls_Ope_Pat_Com_Bienes_Muebles_Negocio {

        #region Variables Internas

            private String Bien_Mueble_ID = null;
            private String Producto_ID = null;
            private String Clave_Producto = null;
            private String Dependencia_ID = null;
            private String Area_ID = null;
            private String Material_ID = null;
            private String Color_ID = null;
            private String Marca_ID = null;
            private String Modelo_ID = null;
            private String Proveedor_ID = null;
            private String Clasificacion_ID = null;
            private String Clase_Activo_ID = null;
            private String Numero_Inventario = null;
            private String Numero_Inventario_Anterior = null;
            private Int32 Clave_Sistema = 0;
            private Int32 Clave_Inventario = 0;
            private String Factura = null;
            private String Numero_Serie = null;
            private Double Costo_Inicial = 0.0;
            private Double Costo_Actual = 0.0;
            private String Tipo = null;
            private String Estatus = null;
            private String Motivo_Baja = null;
            private String Estado = null;
            private String Observaciones = null;
            private String RFC_Resguardante = null;
            private String Resguardante_ID = null;
            private String Tipo_Filtro_Busqueda = null;
            private DataTable Resguardantes = null;
            private String Usuario_ID = null;
            private String Usuario_Nombre = null;
            private Int32 Cantidad = 0;
            private String Tipo_DataTable = null;
            private String Fecha_Adquisicion = null;
            private Boolean Buscar_Numero_Inventario = false;
            private DataTable Historial_Resguardos = null;
            private Boolean Producto_Almacen = true;

            private String Procedencia = null;
            private String Proveniente = null;
            private String Donador_ID = null;

            private String Nombre_Producto = null;

            private String Archivo = null;
            private DataTable Dt_Historial_Archivos = null;

            private String Ascencendia = null;
            private DataTable Dt_Bienes_Dependientes = null;

            private String No_Requisicion = null;
            private String Operacion = null;
            private String Modelo = null;
            private String Garantia = null;

            private String Dato_Creacion = null;
            private String Dato_Modificacion = null;
            private String Zona = null;

            private DateTime Fecha_Inventario_;
            private DateTime Fecha_Adquisicion_;
            private DateTime Fecha_Creo;

            private String Nombre_Resguardante = null;
            private String No_Empleado_Resguardante = null;

            private String Estatus_Empleado = "ACTIVO";

        #endregion

        #region Variables Publicas
            public String P_Bien_Mueble_ID
            {
                get { return Bien_Mueble_ID; }
                set { Bien_Mueble_ID = value; }
            }
            public String P_Producto_ID
            {
                get { return Producto_ID; }
                set { Producto_ID = value; }
            }
            public String P_No_Requisicion
            {
                get { return No_Requisicion; }
                set { No_Requisicion = value; }
            }
            public String P_Clave_Producto
            {
                get { return Clave_Producto; }
                set { Clave_Producto = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Area_ID
            {
                get { return Area_ID; }
                set { Area_ID = value; }
            }
            public String P_Material_ID
            {
                get { return Material_ID; }
                set { Material_ID = value; }
            }
            public String P_Color_ID
            {
                get { return Color_ID; }
                set { Color_ID = value; }
            }
            public String P_Marca_ID
            {
                get { return Marca_ID; }
                set { Marca_ID = value; }
            }
            public String P_Modelo_ID
            {
                get { return Modelo_ID; }
                set { Modelo_ID = value; }
            }
            public String P_Proveedor_ID
            {
                get { return Proveedor_ID; }
                set { Proveedor_ID = value; }
            }
            public String P_Clasificacion_ID
            {
                get { return Clasificacion_ID; }
                set { Clasificacion_ID = value; }
            }
            public String P_Clase_Activo_ID
            {
                get { return Clase_Activo_ID; }
                set { Clase_Activo_ID = value; }
            }
            public String P_Numero_Inventario
            {
                get { return Numero_Inventario; }
                set { Numero_Inventario = value; }
            }
            public String P_Numero_Inventario_Anterior
            {
                get { return Numero_Inventario_Anterior; }
                set { Numero_Inventario_Anterior = value; }
            }
            public Int32 P_Clave_Sistema
            {
                get { return Clave_Sistema; }
                set { Clave_Sistema = value; }
            }
            public Int32 P_Clave_Inventario
            {
                get { return Clave_Inventario; }
                set { Clave_Inventario = value; }
            }
            public String P_Factura
            {
                get { return Factura; }
                set { Factura = value; }
            }
            public String P_Numero_Serie
            {
                get { return Numero_Serie; }
                set { Numero_Serie = value; }
            }
            public Double P_Costo_Inicial
            {
                get { return Costo_Inicial; }
                set { Costo_Inicial = value; }
            }
            public Double P_Costo_Actual
            {
                get { return Costo_Actual; }
                set { Costo_Actual = value; }
            }
            public String P_Tipo
            {
                get { return Tipo; }
                set { Tipo = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Motivo_Baja
            {
                get { return Motivo_Baja; }
                set { Motivo_Baja = value; }
            }
            public String P_Estado
            {
                get { return Estado; }
                set { Estado = value; }
            }
            public String P_Observaciones
            {
                get { return Observaciones; }
                set { Observaciones = value; }
            }
            public String P_RFC_Resguardante
            {
                get { return RFC_Resguardante; }
                set { RFC_Resguardante = value; }
            }
            public String P_Resguardante_ID
            {
                get { return Resguardante_ID; }
                set { Resguardante_ID = value; }
            }
            public String P_Tipo_Filtro_Busqueda
            {
                get { return Tipo_Filtro_Busqueda; }
                set { Tipo_Filtro_Busqueda = value; }
            }
            public DataTable P_Resguardantes
            {
                get { return Resguardantes; }
                set { Resguardantes = value; }
            }
            public String P_Usuario_Nombre
            {
                get { return Usuario_Nombre; }
                set { Usuario_Nombre = value; }
            }
            public String P_Usuario_ID
            {
                get { return Usuario_ID; }
                set { Usuario_ID = value; }
            }
            public Int32 P_Cantidad
            {
                get { return Cantidad; }
                set { Cantidad = value; }
            }
            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }
            public String P_Fecha_Adquisicion
            {
                get { return Fecha_Adquisicion; }
                set { Fecha_Adquisicion = value; }
            }
            public Boolean P_Buscar_Numero_Inventario
            {
                get { return Buscar_Numero_Inventario; }
                set { Buscar_Numero_Inventario = value; }
            }
            public DataTable P_Historial_Resguardos
            {
                get { return Historial_Resguardos; }
                set { Historial_Resguardos = value; }
            }
            public String P_Donador_ID
            {
                get { return Donador_ID; }
                set { Donador_ID = value; }
            }
            public String P_Proveniente
            {
                get { return Proveniente; }
                set { Proveniente = value; }
            }
            public String P_Procedencia
            {
                get { return Procedencia; }
                set { Procedencia = value; }
            }
            public Boolean P_Producto_Almacen
            {
                get { return Producto_Almacen; }
                set { Producto_Almacen = value; }
            }
            public String P_Nombre_Producto
            {
                get { return Nombre_Producto; }
                set { Nombre_Producto = value; }
            }
            public String P_Archivo
            {
                get { return Archivo; }
                set { Archivo = value; }
            }
            public DataTable P_Dt_Historial_Archivos
            {
                get { return Dt_Historial_Archivos; }
                set { Dt_Historial_Archivos = value; }
            }
            public String P_Ascencendia
            {
                get { return Ascencendia; }
                set { Ascencendia = value; }
            }
            public DataTable P_Dt_Bienes_Dependientes
            {
                get { return Dt_Bienes_Dependientes; }
                set { Dt_Bienes_Dependientes = value; }
            }
            public String P_Operacion
            {
                get { return Operacion; }
                set { Operacion = value; }
            }
            public String P_Modelo
            {
                get { return Modelo; }
                set { Modelo = value; }
            }
            public String P_Garantia
            {
                get { return Garantia; }
                set { Garantia = value; }
            }
            public String P_Dato_Creacion
            {
                get { return Dato_Creacion; }
                set { Dato_Creacion = value; }
            }
            public String P_Dato_Modificacion
            {
                get { return Dato_Modificacion; }
                set { Dato_Modificacion = value; }
            }

            public DateTime P_Fecha_Inventario_
            {
                get { return Fecha_Inventario_; }
                set { Fecha_Inventario_ = value; }
            }
            public DateTime P_Fecha_Adquisicion_
            {
                get { return Fecha_Adquisicion_; }
                set { Fecha_Adquisicion_ = value; }
            }
            public DateTime P_Fecha_Creo
            {
                get { return Fecha_Creo; }
                set { Fecha_Creo = value; }
            }
            public String P_Zona
            {
                get { return Zona; }
                set { Zona = value; }
            }
            public String P_Nombre_Resguardante
            {
                get { return Nombre_Resguardante; }
                set { Nombre_Resguardante = value; }
            }
            public String P_No_Empleado_Resguardante
            {
                get { return No_Empleado_Resguardante; }
                set { No_Empleado_Resguardante = value; }
            }
            public String P_Estatus_Empleado
            {
                get { return Estatus_Empleado; }
                set { Estatus_Empleado = value; }
            }

        #endregion

        #region Metodos

            public Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Alta_Bien_Mueble()
            {
                return Cls_Ope_Pat_Com_Bienes_Muebles_Datos.Alta_Bien_Mueble(this);
            }

            public void Modificar_Bien_Mueble() {
                Cls_Ope_Pat_Com_Bienes_Muebles_Datos.Modificar_Bien_Mueble(this);
            }

            public void Modificar_Bien_Mueble_Secundarios() {
                Cls_Ope_Pat_Com_Bienes_Muebles_Datos.Modificar_Bien_Mueble_Secundarios(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Ope_Pat_Com_Bienes_Muebles_Datos.Consultar_DataTable(this);
            }

            public Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Consultar_Detalles_Bien_Mueble() {
                return Cls_Ope_Pat_Com_Bienes_Muebles_Datos.Consultar_Detalles_Bien_Mueble(this);
            }

            public Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Alta_Migrar_Bien_Mueble()
            {
                return Cls_Ope_Pat_Com_Bienes_Muebles_Datos.Alta_Migrar_Bien_Mueble(this);
            }


            public Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Alta_Migrar_Resguardos_Bien_Mueble() {
                return Cls_Ope_Pat_Com_Bienes_Muebles_Datos.Alta_Migrar_Resguardos_Bien_Mueble(this);
            }
            public void Actualizar_Estatus_Bienes(){
                Cls_Ope_Pat_Com_Bienes_Muebles_Datos.Actualizar_Estatus_Bienes(this);
            }
            public DataTable Consultar_Empleados_Resguardos() {
                return Cls_Ope_Pat_Com_Bienes_Muebles_Datos.Consultar_Empleados_Resguardos(this);
            }

            public void Actualizar_Bienes_Migracion() {
                Cls_Ope_Pat_Com_Bienes_Muebles_Datos.Actualizar_Bienes_Migracion(this);
            }

        #endregion

    }

}