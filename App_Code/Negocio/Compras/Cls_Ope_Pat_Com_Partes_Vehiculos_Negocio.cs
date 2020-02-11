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
using Presidencia.Control_Patrimonial_Operacion_Partes_Vehiculos.Datos;
using System.Collections.Generic;

/// <summary>
/// Summary description for Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Operacion_Partes_Vehiculos.Negocio {

    public class Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio {

        #region Variables Internas

            private Int32 Parte_ID;
            private String Producto_ID = null;
            private String Vehiculo_ID = null;
            private String Nombre = null;
            private String Marca = null;
            private String Modelo = null;
            private Double Costo = 0.0;
            private String Material = null;
            private String Color = null;
            private Int32 Cantidad = -1;
            private String Estado = null;
            private String Numero_Inventario = null;
            private String Estatus = null;
            private String Comentarios = null;
            private String Motivo_Baja = null;
            private DateTime Fecha_Adquisicion;
            private DataTable Resguardantes = null;
            private String Usuario_ID = null;
            private String Usuario_Nombre = null;
            private String Tipo_DataTable = null;
            private DataTable Historial_Resguardos = null;
            private String Clave_Interna = null;
            private String RFC_Resguardante = null;
            private String Resguardante_ID = null;
            private String Tipo_Filtro_Busqueda = null;
            private String Dependencia_ID = null;
            private String Numero_Inventario_Vehiculo = null;
            private Boolean Buscar_Fecha_Adquisicion = false;

        #endregion

        #region Variables Publicas

            public Int32 P_Parte_ID
            {
                get { return Parte_ID; }
                set { Parte_ID = value; }
            }
            public String P_Producto_ID
            {
                get { return Producto_ID; }
                set { Producto_ID = value; }
            }
            public String P_Vehiculo_ID
            {
                get { return Vehiculo_ID; }
                set { Vehiculo_ID = value; }
            }
            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }
            public String P_Marca
            {
                get { return Marca; }
                set { Marca = value; }
            }
            public String P_Modelo
            {
                get { return Modelo; }
                set { Modelo = value; }
            }
            public Double P_Costo
            {
                get { return Costo; }
                set { Costo = value; }
            }
            public String P_Material
            {
                get { return Material; }
                set { Material = value; }
            }
            public String P_Color
            {
                get { return Color; }
                set { Color = value; }
            }
            public Int32 P_Cantidad
            {
                get { return Cantidad; }
                set { Cantidad = value; }
            }
            public String P_Estado
            {
                get { return Estado; }
                set { Estado = value; }
            }
            public String P_Numero_Inventario
            {
                get { return Numero_Inventario; }
                set { Numero_Inventario = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }
            public String P_Motivo_Baja
            {
                get { return Motivo_Baja; }
                set { Motivo_Baja = value; }
            }
            public DateTime P_Fecha_Adquisicion
            {
                get { return Fecha_Adquisicion; }
                set { Fecha_Adquisicion = value; }
            }
            public DataTable P_Resguardantes
            {
                get { return Resguardantes; }
                set { Resguardantes = value; }
            }
            public String P_Usuario_ID
            {
                get { return Usuario_ID; }
                set { Usuario_ID = value; }
            }
            public String P_Usuario_Nombre
            {
                get { return Usuario_Nombre; }
                set { Usuario_Nombre = value; }
            }
            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }
            public DataTable P_Historial_Resguardos
            {
                get { return Historial_Resguardos; }
                set { Historial_Resguardos = value; }
            }
            public String P_Clave_Interna
            {
                get { return Clave_Interna; }
                set { Clave_Interna = value; }
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
            public String P_RFC_Resguardante
            {
                get { return RFC_Resguardante; }
                set { RFC_Resguardante = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Numero_Inventario_Vehiculo
            {
                get { return Numero_Inventario_Vehiculo; }
                set { Numero_Inventario_Vehiculo = value; }
            }
            public Boolean P_Buscar_Fecha_Adquisicion
            {
                get { return Buscar_Fecha_Adquisicion; }
                set { Buscar_Fecha_Adquisicion = value; }
            }
        #endregion

        #region Metodos

            public Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Alta_Parte() {
                return Cls_Ope_Pat_Com_Partes_Vehiculos_Datos.Alta_Parte(this);
            }

            public void Modificar_Parte() {
                Cls_Ope_Pat_Com_Partes_Vehiculos_Datos.Modificar_Parte(this);
            }

            public void Eliminar_Parte() {
                Cls_Ope_Pat_Com_Partes_Vehiculos_Datos.Eliminar_Parte(this);
            }

            public Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Consultar_Datos_Parte_Vehiculo() {
                return Cls_Ope_Pat_Com_Partes_Vehiculos_Datos.Consultar_Datos_Parte_Vehiculo(this);
            }

            public List<Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio> Consultar_Listado_Partes_Vehiculo() {
                return Cls_Ope_Pat_Com_Partes_Vehiculos_Datos.Consultar_Listado_Partes_Vehiculo(this);
            }

            public DataTable Listar_Productos_Partes(){
                return Cls_Ope_Pat_Com_Partes_Vehiculos_Datos.Listar_Productos_Partes(this);
            }

            public DataTable Listado_Partes_Vehiculos() {
                return Cls_Ope_Pat_Com_Partes_Vehiculos_Datos.Listado_Partes_Vehiculos(this);
            }

        #endregion

    }

}