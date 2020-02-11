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
using Presidencia.Almacen_Generar_Kardex_Productos.Datos;

/// <summary>
/// Summary description for Cls_Ope_Alm_Generar_Kardex_Productos_Negocio
/// </summary>
namespace Presidencia.Almacen_Generar_Kardex_Productos.Negocio { 
    
    public class Cls_Ope_Alm_Generar_Kardex_Productos_Negocio {

        #region Variables Internas

            private String Producto_ID = null;
            private String Clave = null;
            private String Descripcion = null;
            private String Modelo = null;
            private String Marca = null;
            private Int32 Inicial = -1;
            private String Unidad = null;
            private String Estatus = null;
            private DataTable Dt_Entradas = null;
            private DataTable Dt_Salidas = null;
            private DataTable Dt_Comprometidos = null;
            private DataTable Dt_Entradas_Ajuste = null;
            private DataTable Dt_Salidas_Ajuste = null;
            private Int32 Total_Entradas = -1;
            private Int32 Total_Salidas = -1;
            private Int32 Existencias = -1;
            private Int32 Total_Comprometido = -1;
            private Int32 Disponible = -1;
            private DateTime Fecha_Inicio;
            private Boolean Tomar_Fecha_Inicio = false;
            private DateTime Fecha_Fin;
            private Boolean Tomar_Fecha_Fin = false;
            private String Estatus_Salida = null;

            private String Fecha_I;
            private String Fecha_F;
            private String Partida_ID;
            private String Dependencia_ID;

        #endregion

        #region Variables Publicas
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Partida_ID
            {
                get { return Partida_ID; }
                set { Partida_ID = value; }
            }
            public String P_Fecha_F
            {
                get { return Fecha_F; }
                set { Fecha_F = value; }
            }

            public String P_Fecha_I
            {
                get { return Fecha_I; }
                set { Fecha_I = value; }
            }
            public String P_Producto_ID
            {
                get { return Producto_ID; }
                set { Producto_ID = value; }
            }
            public String P_Clave
            {
                get { return Clave; }
                set { Clave = value; }
            }
            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
            public String P_Modelo
            {
                get { return Modelo; }
                set { Modelo = value; }
            }
            public String P_Marca
            {
                get { return Marca; }
                set { Marca = value; }
            }
            public Int32 P_Inicial
            {
                get { return Inicial; }
                set { Inicial = value; }
            }
            public String P_Unidad
            {
                get { return Unidad; }
                set { Unidad = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Estatus_Salida
            {
                get { return Estatus_Salida; }
                set { Estatus_Salida = value; }
            }
            public DataTable P_Dt_Entradas {
                get { return Dt_Entradas; }
                set { Dt_Entradas = value; }
            }
            public DataTable P_Dt_Salidas {
                get { return Dt_Salidas; }
                set { Dt_Salidas = value; }
            }
            public DataTable P_Dt_Comprometidos {
                get { return Dt_Comprometidos; }
                set { Dt_Comprometidos = value; }
            }
            public DataTable P_Dt_Entradas_Ajuste
            {
                get { return Dt_Entradas_Ajuste; }
                set { Dt_Entradas_Ajuste = value; }
            }
            public DataTable P_Dt_Salidas_Ajuste
            {
                get { return Dt_Salidas_Ajuste; }
                set { Dt_Salidas_Ajuste = value; }
            }
            public Int32 P_Total_Entradas
            {
                get { return Total_Entradas; }
                set { Total_Entradas = value; }
            }
            public Int32 P_Total_Salidas
            {
                get { return Total_Salidas; }
                set { Total_Salidas = value; }
            }
            public Int32 P_Existencias
            {
                get { return Existencias; }
                set { Existencias = value; }
            }
            public Int32 P_Total_Comprometido
            {
                get { return Total_Comprometido; }
                set { Total_Comprometido = value; }
            }
            public Int32 P_Disponible
            {
                get { return Disponible; }
                set { Disponible = value; }
            }
            public DateTime P_Fecha_Inicio {
                get { return Fecha_Inicio; }
                set { Fecha_Inicio = value; }
            }
            public Boolean P_Tomar_Fecha_Inicio
            {
                get { return Tomar_Fecha_Inicio; }
                set { Tomar_Fecha_Inicio = value; }
            }
            public DateTime P_Fecha_Fin {
                get { return Fecha_Fin; }
                set { Fecha_Fin = value; }
            }
            public Boolean P_Tomar_Fecha_Fin
            {
                get { return Tomar_Fecha_Fin; }
                set { Tomar_Fecha_Fin = value; }
            }

        #endregion

        #region Metodos

            public Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Obtener_Detalles_Producto() {
                return Cls_Ope_Alm_Generar_Kardex_Productos_Datos.Obtener_Detalles_Producto(this);
            }

            public Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Obtener_Detalles_Kardex() {
                return Cls_Ope_Alm_Generar_Kardex_Productos_Datos.Obtener_Detalles_Kardex(this);
            }

            public DataTable Consultar_Kardex_Actualizado()
            {
                return Cls_Ope_Alm_Generar_Kardex_Productos_Datos.Consultar_Kardex_Actualizado(this);
            }


            public DataTable Consultar_Kardex()
            {
                return Cls_Ope_Alm_Generar_Kardex_Productos_Datos.Consultar_Kardex(this);
            }
            
            public DataTable Consultar_Entradas()
            {
                return Cls_Ope_Alm_Generar_Kardex_Productos_Datos.Consultar_Entradas(this);
            }

            public DataTable Consultar_Entradas_Ajuste()
            {
                return Cls_Ope_Alm_Generar_Kardex_Productos_Datos.Consultar_Entradas_Ajuste(this);
            }

            public DataTable Consultar_Salidas()
            {
                return Cls_Ope_Alm_Generar_Kardex_Productos_Datos.Consultar_Salidas(this);
            }
            public DataTable Consultar_Salidas_Ajuste()
            {
                return Cls_Ope_Alm_Generar_Kardex_Productos_Datos.Consultar_Salidas_Ajuste(this);
            }
            public DataTable Consultar_Compromisos()
            {
                return Cls_Ope_Alm_Generar_Kardex_Productos_Datos.Consultar_Compromisos(this);
            }

            public DataTable Consultar_Salidas_Unidad_Responsable()
            {
                return Cls_Ope_Alm_Generar_Kardex_Productos_Datos.Consultar_Salidas_Unidad_Responsable(this);
            }
        
        #endregion
    }

}
