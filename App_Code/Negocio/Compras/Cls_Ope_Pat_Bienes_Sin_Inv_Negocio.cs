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
using Presidencia.Control_Patrimonial_Bienes_Sin_Inventario.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pat_Bienes_Sin_Inv_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Bienes_Sin_Inventario.Negocio { 
    public class Cls_Ope_Pat_Bienes_Sin_Inv_Negocio {

        #region Variables Internas

            //Detalles del Bien
            private Int32 Bien_ID = (-1);
            private String Bien_Parent_ID = null;
            private String Tipo_Parent = null;
            private String Nombre = null;
            private String Marca = null;
            private Double Costo_Inicial = (-1.0);
            private String Material = null;
            private String Color = null;
            private String Estado = null;
            private String Estatus = null;
            private String Comentarios = null;
            private String Motivo_Baja = null;
            private DateTime Fecha_Adquisicion;
            private String Producto_ID = null;
            private String Modelo = null;
            private String Numero_Serie = null;

            //Datos para el manejo del Bien
            private DataTable Dt_Resguardantes = null;
            private String Usuario_ID = null;
            private String Usuario_Nombre = null;
            private String Tipo_DataTable = null;
            private String No_Inventario_Parent = null;
            private DataTable Dt_Historial_Resguardos = null;
            private String RFC_Resguardante = null;
            private String No_Empleado_Resguardante = null;
            private String Resguardante_ID = null;
            private String Tipo_Filtro_Busqueda = null;
            private String Dependencia_ID = null;
            private String Dato_Creo = null;
            private String Dato_Modifico = null;
            private String Identificador_Generico = null;


        #endregion

        #region Variables Publicas

            //Detalles del Bien
            public Int32 P_Bien_ID
            {
                get { return Bien_ID; }
                set { Bien_ID = value; }
            }
            public String P_Bien_Parent_ID
            {
                get { return Bien_Parent_ID; }
                set { Bien_Parent_ID = value; }
            }
            public String P_Tipo_Parent
            {
                get { return Tipo_Parent; }
                set { Tipo_Parent = value; }
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
            public Double P_Costo_Inicial
            {
                get { return Costo_Inicial; }
                set { Costo_Inicial = value; }
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
            public String P_Estado
            {
                get { return Estado; }
                set { Estado = value; }
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
            public String P_Producto_ID
            {
                get { return Producto_ID; }
                set { Producto_ID = value; }
            }
            public String P_Modelo
            {
                get { return Modelo; }
                set { Modelo = value; }
            }
            public String P_Numero_Serie
            {
                get { return Numero_Serie; }
                set { Numero_Serie = value; }
            }

            //Datos para el manejo del Bien
            public DataTable P_Dt_Resguardantes
            {
                get { return Dt_Resguardantes; }
                set { Dt_Resguardantes = value; }
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
            public DataTable P_Dt_Historial_Resguardos
            {
                get { return Dt_Historial_Resguardos; }
                set { Dt_Historial_Resguardos = value; }
            }
            public String P_RFC_Resguardante
            {
                get { return RFC_Resguardante; }
                set { RFC_Resguardante = value; }
            }
            public String P_Resguardante_ID {
                get { return Resguardante_ID; }
                set { Resguardante_ID = value; }
            }
            public String P_Tipo_Filtro_Busqueda
            {
                get { return Tipo_Filtro_Busqueda; }
                set { Tipo_Filtro_Busqueda = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Dato_Creo
            {
                get { return Dato_Creo; }
                set { Dato_Creo = value; }
            }
            public String P_Dato_Modifico
            {
                get { return Dato_Modifico; }
                set { Dato_Modifico = value; }
            }
            public String P_Identificador_Generico
            {
                get { return Identificador_Generico; }
                set { Identificador_Generico = value; }
            }
            public String P_No_Inventario_Parent
            {
                get { return No_Inventario_Parent; }
                set { No_Inventario_Parent = value; }
            }
            public String P_No_Empleado_Resguardante
            {
                get { return No_Empleado_Resguardante; }
                set { No_Empleado_Resguardante = value; }
            }
        #endregion

        #region Metodos

            public Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Alta_Bien_Sin_Inventario() {
                return Cls_Ope_Pat_Bienes_Sin_Inv_Datos.Alta_Bien_Sin_Inventario(this);
            }

            public void Modifica_Bien_Sin_Inventario() {
                Cls_Ope_Pat_Bienes_Sin_Inv_Datos.Modifica_Bien_Sin_Inventario(this);
            }

            public DataTable Consultar_Bienes_Sin_Inventario() {
                return Cls_Ope_Pat_Bienes_Sin_Inv_Datos.Consultar_Bienes_Sin_Inventario(this);
            }

            public Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Consultar_Detalles_Bien_Sin_Inventario() {
                return Cls_Ope_Pat_Bienes_Sin_Inv_Datos.Consultar_Detalles_Bien_Sin_Inventario(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Ope_Pat_Bienes_Sin_Inv_Datos.Consultar_DataTable(this);
            }

        #endregion

    }
}

