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
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pat_Com_Cemovientes_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio {

    public class Cls_Ope_Pat_Com_Cemovientes_Negocio {

        #region Variables Internas

            private String Cemoviente_ID = null;
            private String Producto_ID = null;
            private String Dependencia_ID = null;
            private String Tipo_Alimentacion_ID = null;
            private String Tipo_Adiestramiento_ID = null;
            private String Raza_ID = null;
            private String Tipo_Ascendencia = null;
            private String Padre_ID = null;
            private String Madre_ID = null;
            private String Sexo = null;
            private String Tipo_Cemoviente_ID = null;
            private String Funcion_ID = null;
            private String Veterinario_ID = null;
            private String Color_ID = null;
            private String Nombre = null;
            private Int64 Numero_Inventario = -1;
            private Double Costo_Inicial = 0.0;
            private Double Costo_Actual = 0.0;
            private DateTime Fecha_Adquisicion;
            private DateTime Fecha_Nacimiento;
            private String Estatus = null;
            private String Motivo_Baja = null;
            private String Observaciones = null;
            private String RFC_Resguardante = null;
            private String Resguardante_ID = null;
            private String Tipo_Filtro_Busqueda = null;
            private DataTable Resguardantes = null;
            private String Usuario_ID = null;
            private String Usuario_Nombre = null;
            private String Tipo_DataTable = null;
            private Boolean Buscar_Numero_Inventario = false;
            private DataTable Historial_Resguardos = null;
            private DataTable Dt_Vacunas = null;
            private Int32 Cantidad = 0;
            private Boolean Producto_Almacen = true;

            private String Archivo = null;
            private DataTable Dt_Historial_Archivos = null;

            private String Procedencia = null;
            private String Proveniente = null;
            private String Donador_ID = null;
            private String No_Requisicion = null;

            private String No_Factura = null;
            private String Proveedor_ID = null;

            private String Dato_Creacion = null;
            private String Dato_Modificacion = null;
            private String No_Inventario_Anterior = null;

            private String Nombre_Resguardante = null;
            private String No_Empleado_Resguardante = null;

            private String Clasificacion_ID = null;
            private String Clase_Activo_ID = null;

        #endregion

        #region Variables Publicas

            public String P_Cemoviente_ID
            {
                get { return Cemoviente_ID; }
                set { Cemoviente_ID = value; }
            }
            public String P_Producto_ID
            {
                get { return Producto_ID; }
                set { Producto_ID = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Tipo_Alimentacion_ID
            {
                get { return Tipo_Alimentacion_ID; }
                set { Tipo_Alimentacion_ID = value; }
            }
            public String P_Tipo_Adiestramiento_ID
            {
                get { return Tipo_Adiestramiento_ID; }
                set { Tipo_Adiestramiento_ID = value; }
            }
            public String P_Raza_ID
            {
                get { return Raza_ID; }
                set { Raza_ID = value; }
            }
            public String P_Tipo_Ascendencia
            {
                get { return Tipo_Ascendencia; }
                set { Tipo_Ascendencia = value; }
            }
            public String P_Padre_ID
            {
                get { return Padre_ID; }
                set { Padre_ID = value; }
            }
            public String P_Madre_ID
            {
                get { return Madre_ID; }
                set { Madre_ID = value; }
            }
            public String P_Sexo
            {
                get { return Sexo; }
                set { Sexo = value; }
            }
            public String P_Tipo_Cemoviente_ID
            {
                get { return Tipo_Cemoviente_ID; }
                set { Tipo_Cemoviente_ID = value; }
            }
            public String P_Funcion_ID
            {
                get { return Funcion_ID; }
                set { Funcion_ID = value; }
            }
            public String P_Veterinario_ID
            {
                get { return Veterinario_ID; }
                set { Veterinario_ID = value; }
            }
            public String P_Color_ID
            {
                get { return Color_ID; }
                set { Color_ID = value; }
            }
            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }
            public Int64 P_Numero_Inventario
            {
                get { return Numero_Inventario; }
                set { Numero_Inventario = value; }
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
            public DateTime P_Fecha_Adquisicion
            {
                get { return Fecha_Adquisicion; }
                set { Fecha_Adquisicion = value; }
            }
            public DateTime P_Fecha_Nacimiento
            {
                get { return Fecha_Nacimiento; }
                set { Fecha_Nacimiento = value; }
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
            public DataTable P_Dt_Vacunas
            {
                get { return Dt_Vacunas; }
                set { Dt_Vacunas = value; }
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
            public String P_No_Factura
            {
                get { return No_Factura; }
                set { No_Factura = value; }
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

            public String P_No_Inventario_Anterior
            {
                get { return No_Inventario_Anterior; }
                set { No_Inventario_Anterior = value; }
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

            public Cls_Ope_Pat_Com_Cemovientes_Negocio Alta_Cemoviente() {
                return Cls_Ope_Pat_Com_Cemovientes_Datos.Alta_Cemoviente(this);
            }
            public void Modificar_Cemoviente() {
                Cls_Ope_Pat_Com_Cemovientes_Datos.Modificar_Cemoviente(this);
            }
            public DataTable Consultar_DataTable() {
                return Cls_Ope_Pat_Com_Cemovientes_Datos.Consultar_DataTable(this);
            }
            public Cls_Ope_Pat_Com_Cemovientes_Negocio Consultar_Detalles_Cemoviente() {
                return Cls_Ope_Pat_Com_Cemovientes_Datos.Consultar_Detalles_Cemoviente(this);
            }

            public Cls_Ope_Pat_Com_Cemovientes_Negocio Alta_Migrar_Cemoviente() {
                return Cls_Ope_Pat_Com_Cemovientes_Datos.Alta_Migrar_Cemoviente(this);
            }
            public DataTable Consultar_Empleados_Resguardos() {
                return Cls_Ope_Pat_Com_Cemovientes_Datos.Consultar_Empleados_Resguardos(this);
            }

        #endregion

    }

}