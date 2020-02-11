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
using Presidencia.Control_Patrimonial_Listado_Bienes.Datos;

/// <summary>
/// Summary description for Cls_Pat_Com_Listado_Bienes
/// </summary>
namespace Presidencia.Control_Patrimonial_Listado_Bienes.Negocio{

    public class Cls_Ope_Pat_Com_Listado_Bienes_Negocio {

        #region Variables Internas

            private String Inventario_Anterior = null;
            private String Numero_Inventario = null;
            private String Producto = null;
            private String Filtro = null;
            private String Tipo = null;
            private String Tipo_Cemoviente = null;
            private String Raza = null;
            private String Marca = null;
            private String Modelo = null;
            private String Estatus = null;
            private String Numero_Factura = null;
            private String Numero_Serie = null;
            private String No_Empleado = null;
            private String RFC_Resguardante = null;
            private String Resguardante = null;
            private String Unidad_Responsable = null;

            private String Tipo_DataTable = null;

        #endregion

        #region Variables Publicas

            public String P_Inventario_Anterior
            {
                get { return Inventario_Anterior; }
                set { Inventario_Anterior = value; }
            }
            public String P_Numero_Inventario
            {
                get { return Numero_Inventario; }
                set { Numero_Inventario = value; }
            }
            public String P_Producto
            {
                get { return Producto; }
                set { Producto = value; }
            }
            public String P_Filtro
            {
                get { return Filtro; }
                set { Filtro = value; }
            }
            public String P_Tipo
            {
                get { return Tipo; }
                set { Tipo = value; }
            }
            public String P_Tipo_Cemoviente
            {
                get { return Tipo_Cemoviente; }
                set { Tipo_Cemoviente = value; }
            }
            public String P_Raza
            {
                get { return Raza; }
                set { Raza = value; }
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
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Numero_Factura
            {
                get { return Numero_Factura; }
                set { Numero_Factura = value; }
            }
            public String P_Numero_Serie
            {
                get { return Numero_Serie; }
                set { Numero_Serie = value; }
            }
            public String P_No_Empleado
            {
                get { return No_Empleado; }
                set { No_Empleado = value; }
            }
            public String P_RFC_Resguardante
            {
                get { return RFC_Resguardante; }
                set { RFC_Resguardante = value; }
            }
            public String P_Resguardante
            {
                get { return Resguardante; }
                set { Resguardante = value; }
            }
            public String P_Unidad_Responsable
            {
                get { return Unidad_Responsable; }
                set { Unidad_Responsable = value; }
            }

            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }             

        #endregion

        #region Metodos

            public DataTable Consultar_DataTable() {
                return Cls_Ope_Pat_Com_Listado_Bienes_Datos.Consultar_DataTable(this);
            }    

            public DataTable Consultar_Listado_Bienes() {
                return Cls_Ope_Pat_Com_Listado_Bienes_Datos.Consultar_Listado_Bienes(this);
            }

        #endregion

    }

}