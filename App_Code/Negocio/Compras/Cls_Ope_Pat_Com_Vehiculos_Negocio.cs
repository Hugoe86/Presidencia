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
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Datos;
using System.Collections.Generic;


/// <summary>
/// Summary description for Cls_Ope_Pat_Com_Vehiculos_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio {

    public class Cls_Ope_Pat_Com_Vehiculos_Negocio {

        #region Variables Internas

            private String Vehiculo_ID = null;
            private String Producto_ID = null;
            private String Nombre_Producto = null;
            private String Dependencia_ID = null;
            private String Color_ID = null;
            private String Marca_ID = null;
            private String Modelo_ID = null;
            private String Zona_ID = null;
            private Int64 Numero_Inventario = -1;
            private Int32 Numero_Economico = -1;
            private String Numero_Economico_ = null;
            private String Placas = null;
            private Double Costo_Inicial = 0.0;
            private Double Costo_Actual = 0.0;
            private Int32 Clave_Programatica_Revision = 0;
            private String Capacidad_Carga = null;
            private String Tipo_Vehiculo_ID = null;
            private String Tipo_Combustible_ID = null;
            private Int32 Anio_Fabricacion = -1;
            private String Serie_Carroceria = null;
            private String Serie_Motor = null;
            private Int32 Numero_Cilindros = -1;
            private Double Kilometraje = 0.0;
            private Int32 Numero_Ejes = -1;
            private String Odometro = null;
            private DateTime Fecha_Adquisicion;
            private String Estatus = null;
            private String Motivo_Baja = null;
            private String Observaciones = null;

            private Int32 Vehiculo_Aseguradora_ID = -1;
            private String Aseguradora_ID = null;
            private String Descripcion_Seguro = null;
            private String Cobertura_Seguro = null;
            private String No_Poliza_Seguro = null;

            private String RFC_Resguardante = null;
            private String No_Empleado = null;
            private String Resguardante_ID = null;
            private String Tipo_Filtro_Busqueda = null;
            private DataTable Resguardantes = null;
            private String Usuario_ID = null;
            private String Usuario_Nombre = null;
            private String Tipo_DataTable = null;
            private Boolean Buscar_Numero_Inventario = false;
            private DataTable Historial_Resguardos = null;
            private Int32 Cantidad = 1;

            private String Archivo = null;
            private DataTable Dt_Historial_Archivos = null;

            private Boolean Producto_Almacen = true; 
            private String Procedencia = null;
            private String Proveniente = null;
            private String Donador_ID = null;
            private String No_Requisicion = null;

            private DataTable Dt_Detalles = null;
            private Int64 No_Factura = -1;
            private String No_Factura_ = null;
            private String Proveedor_ID = null;

            private String Dato_Creacion = null;
            private String Dato_Modificacion = null;

            private String Nombre_Resguardante = null;
            private String No_Empleado_Resguardante = null;

            private String Empleado_Operador = null;
            private String Empleado_Funcionario_Recibe = null;
            private String Empleado_Autorizo = null;
            private DataTable Dt_Partes_Vehiculo = null;

            private String Clasificacion_ID = null;
            private String Clase_Activo_ID = null;

        #endregion

            #region Variables Publicas

            public String P_Vehiculo_ID
            {
                get { return Vehiculo_ID; }
                set { Vehiculo_ID = value; }
            }
            public String P_Producto_ID
            {
                get { return Producto_ID; }
                set { Producto_ID = value; }
            }
            public String P_Nombre_Producto
            {
                get { return Nombre_Producto; }
                set { Nombre_Producto = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
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
            public String P_Zona_ID
            {
                get { return Zona_ID; }
                set { Zona_ID = value; }
            }
            public Int64 P_Numero_Inventario
            {
                get { return Numero_Inventario; }
                set { Numero_Inventario = value; }
            }
            public Int32 P_Numero_Economico
            {
                get { return Numero_Economico; }
                set { Numero_Economico = value; }
            }
            public String P_Numero_Economico_
            {
                get { return Numero_Economico_; }
                set { Numero_Economico_ = value; }
            }
            public String P_Placas
            {
                get { return Placas; }
                set { Placas = value; }
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
            public Int32 P_Clave_Programatica_Revision
            {
                get { return Clave_Programatica_Revision; }
                set { Clave_Programatica_Revision = value; }
            }
            public String P_Capacidad_Carga
            {
                get { return Capacidad_Carga; }
                set { Capacidad_Carga = value; }
            }
            public String P_Tipo_Vehiculo_ID
            {
                get { return Tipo_Vehiculo_ID; }
                set { Tipo_Vehiculo_ID = value; }
            }
            public String P_Tipo_Combustible_ID
            {
                get { return Tipo_Combustible_ID; }
                set { Tipo_Combustible_ID = value; }
            }
            public Int32 P_Anio_Fabricacion
            {
                get { return Anio_Fabricacion; }
                set { Anio_Fabricacion = value; }
            }
            public String P_Serie_Carroceria
            {
                get { return Serie_Carroceria; }
                set { Serie_Carroceria = value; }
            }
            public String P_Serie_Motor
            {
                get { return Serie_Motor; }
                set { Serie_Motor = value; }
            }
            public Int32 P_Numero_Cilindros
            {
                get { return Numero_Cilindros; }
                set { Numero_Cilindros = value; }
            }
            public Double P_Kilometraje
            {
                get { return Kilometraje; }
                set { Kilometraje = value; }
            }
            public Int32 P_Numero_Ejes
            {
                get { return Numero_Ejes; }
                set { Numero_Ejes = value; }
            }
            public String P_Odometro
            {
                get { return Odometro; }
                set { Odometro = value; }
            }
            public DateTime P_Fecha_Adquisicion
            {
                get { return Fecha_Adquisicion; }
                set { Fecha_Adquisicion = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Motivo_Baja {
                get { return Motivo_Baja; }
                set { Motivo_Baja = value; }
            }
            public String P_Observaciones
            {
                get { return Observaciones; }
                set { Observaciones = value; }
            }
            public Int32 P_Vehiculo_Aseguradora_ID
            {
                get { return Vehiculo_Aseguradora_ID; }
                set { Vehiculo_Aseguradora_ID = value; }
            }
            public String P_Aseguradora_ID
            {
                get { return Aseguradora_ID; }
                set { Aseguradora_ID = value; }
            }
            public String P_Descripcion_Seguro
            {
                get { return Descripcion_Seguro; }
                set { Descripcion_Seguro = value; }
            }
            public String P_Cobertura_Seguro
            {
                get { return Cobertura_Seguro; }
                set { Cobertura_Seguro = value; }
            }
            public String P_No_Poliza_Seguro
            {
                get { return No_Poliza_Seguro; }
                set { No_Poliza_Seguro = value; }
            }
            public String P_No_Empleado {
                get { return No_Empleado; }
                set { No_Empleado = value; }
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
            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
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
            public Int32 P_Cantidad
            {
                get { return Cantidad; }
                set { Cantidad = value; }
            }
            public Boolean P_Producto_Almacen
            {
                get { return Producto_Almacen; }
                set { Producto_Almacen = value; }
            }
            public String P_Procedencia
            {
                get { return Procedencia; }
                set { Procedencia = value; }
            }
            public String P_Proveniente
            {
                get { return Proveniente; }
                set { Proveniente = value; }
            }
            public String P_Donador_ID
            {
                get { return Donador_ID; }
                set { Donador_ID = value; }
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
            public String P_No_Requisicion
            {
                get { return No_Requisicion; }
                set { No_Requisicion = value; }
            }
            public DataTable P_Dt_Detalles
            {
                get { return Dt_Detalles; }
                set { Dt_Detalles = value; }
            }
            public Int64 P_No_Factura
            {
                get { return No_Factura; }
                set { No_Factura = value; }
            }
            public String P_No_Factura_
            {
                get { return No_Factura_; }
                set { No_Factura_ = value; }
            }
            public String P_Proveedor_ID
            {
                get { return Proveedor_ID; }
                set { Proveedor_ID = value; }
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
            public String P_Empleado_Operador
            {
                get { return Empleado_Operador; }
                set { Empleado_Operador = value; }
            }
            public String P_Empleado_Funcionario_Recibe
            {
                get { return Empleado_Funcionario_Recibe; }
                set { Empleado_Funcionario_Recibe = value; }
            }
            public String P_Empleado_Autorizo
            {
                get { return Empleado_Autorizo; }
                set { Empleado_Autorizo = value; }
            }
            public DataTable P_Dt_Partes_Vehiculo
            {
                get { return Dt_Partes_Vehiculo; }
                set { Dt_Partes_Vehiculo = value; }
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

        #endregion

        #region Metodos

            public Cls_Ope_Pat_Com_Vehiculos_Negocio Alta_Vehiculo() {
               return Cls_Ope_Pat_Com_Vehiculos_Datos.Alta_Vehiculo(this);
            }

            public void Modificar_Vehiculo() {
                Cls_Ope_Pat_Com_Vehiculos_Datos.Modificar_Vehiculo(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Ope_Pat_Com_Vehiculos_Datos.Consultar_DataTable(this);
            }

            public Cls_Ope_Pat_Com_Vehiculos_Negocio Consultar_Datos_Vehiculo() {
                return Cls_Ope_Pat_Com_Vehiculos_Datos.Consultar_Datos_Vehiculo(this);
            }

            public Cls_Ope_Pat_Com_Vehiculos_Negocio Consultar_Detalles_Vehiculo() {
                return Cls_Ope_Pat_Com_Vehiculos_Datos.Consultar_Detalles_Vehiculo(this);
            }

            public Cls_Ope_Pat_Com_Vehiculos_Negocio Alta_Migrar_Vehiculo()
            {
                return Cls_Ope_Pat_Com_Vehiculos_Datos.Alta_Migrar_Vehiculo(this);
            }
            public DataTable Consultar_Empleados_Resguardos()
            {
                return Cls_Ope_Pat_Com_Vehiculos_Datos.Consultar_Empleados_Resguardos(this);
            }

        #endregion

    }

}