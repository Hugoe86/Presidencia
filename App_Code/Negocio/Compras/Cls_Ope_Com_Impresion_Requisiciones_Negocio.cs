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
using Presidencia.Compras.Impresion_Requisiciones.Datos;

namespace Presidencia.Compras.Impresion_Requisiciones.Negocio
{

    public class Cls_Ope_Com_Impresion_Requisiciones_Negocio
    {
        #region Variables Privadas

        private String No_Requisicion;
        private String Dependencia_ID;
        private String Area_ID;
        private String Proyecto_Programa_ID;
        private String Partida_ID;
        private String Requisicion_ID;
        private DataSet Ds_Productos;
        private DataSet Ds_Servicios;
        private DataSet Ds_Productos_Tmp;
        private DataSet Ds_Servicios_Tmp;
        private String Folio;
        private String Codigo_Programatico;
        private String Estatus;
        private String Comentarios;
        private String Subtotal;
        private String IVA;
        private String IEPS;
        private String Total;
        private DataSet Ds_Documentos_Requisicion;
        private String Tipo;
        private String Fase;
        private String Producto_ID;
        private String Impuesto_ID;
        private String Familia_ID;
        private String Modelo_ID;
        private String Subfamilia_ID;
        private String Nombre_Producto_Servicio;
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String Justificacion_Compra;
        private String Especificacion_Productos;
        private String Verificacion_Entrega;

        private DataTable Dt_Productos_Servicios;
        private String Tipo_Articulo;
        private String Fuente_Financiamiento;

        private DataTable Dt_Productos_Almacen;
        private DataTable Dt_Partidas;
        //DEL PRODUCTO
        private String Giro_ID;
        //Del presupuesto
        private int Anio_Presupuesto;
        #endregion

        #region Variables Publicas
        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }
        public int P_Anio_Presupuesto
        {
            get { return Anio_Presupuesto; }
            set { Anio_Presupuesto = value; }
        }

        public DataTable P_Dt_Productos_Almacen
        {
            get { return Dt_Productos_Almacen; }
            set { Dt_Productos_Almacen = value; }
        }
        public DataTable P_Dt_Partidas
        {
            get { return Dt_Partidas; }
            set { Dt_Partidas = value; }
        }

        public String P_Fuente_Financiamiento
        {
            get { return Fuente_Financiamiento; }
            set { Fuente_Financiamiento = value; }
        }
        public String P_Giro_ID
        {
            get { return Giro_ID; }
            set { Giro_ID = value; }
        }

        public String P_Tipo_Articulo
        {
            get { return Tipo_Articulo; }
            set { Tipo_Articulo = value; }
        }
        public DataTable P_Dt_Productos_Servicios
        {
            get { return Dt_Productos_Servicios; }
            set { Dt_Productos_Servicios = value; }
        }
        public String P_Justificacion_Compra
        {
            get { return Justificacion_Compra; }
            set { Justificacion_Compra = value; }
        }
        public String P_Especificacion_Productos
        {
            get { return Especificacion_Productos; }
            set { Especificacion_Productos = value; }
        }
        public String P_Verificacion_Entrega
        {
            get { return Verificacion_Entrega; }
            set { Verificacion_Entrega = value; }
        }
        public String P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }
        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }
        public String P_Nombre_Producto_Servicio
        {
            get { return Nombre_Producto_Servicio; }
            set { Nombre_Producto_Servicio = value; }
        }

        public String P_Subfamilia_ID
        {
            get { return Subfamilia_ID; }
            set { Subfamilia_ID = value; }
        }

        public String P_Modelo_ID
        {
            get { return Modelo_ID; }
            set { Modelo_ID = value; }
        }

        public String P_Familia_ID
        {
            get { return Familia_ID; }
            set { Familia_ID = value; }
        }


        public DataSet P_Ds_Servicios
        {
            get { return Ds_Servicios; }
            set { Ds_Servicios = value; }
        }

        public DataSet P_Ds_Productos
        {
            get { return Ds_Productos; }
            set { Ds_Productos = value; }
        }

        public DataSet P_Ds_Servicios_Tmp
        {
            get { return Ds_Servicios_Tmp; }
            set { Ds_Servicios_Tmp = value; }
        }

        public DataSet P_Ds_Productos_Tmp
        {
            get { return Ds_Productos_Tmp; }
            set { Ds_Productos_Tmp = value; }
        }

        public String P_Impuesto_ID
        {
            get { return Impuesto_ID; }
            set { Impuesto_ID = value; }
        }

        public String P_Producto_ID
        {
            get { return Producto_ID; }
            set { Producto_ID = value; }
        }

        public String P_Fase
        {
            get { return Fase; }
            set { Fase = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public DataSet P_Ds_Documentos_Requisicion
        {
            get { return Ds_Documentos_Requisicion; }
            set { Ds_Documentos_Requisicion = value; }
        }

        public String P_Total
        {
            get { return Total; }
            set { Total = value; }
        }

        public String P_IEPS
        {
            get { return IEPS; }
            set { IEPS = value; }
        }

        public String P_IVA
        {
            get { return IVA; }
            set { IVA = value; }
        }

        public String P_Subtotal
        {
            get { return Subtotal; }
            set { Subtotal = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Codigo_Programatico
        {
            get { return Codigo_Programatico; }
            set { Codigo_Programatico = value; }
        }

        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }

        public String P_Requisicion_ID
        {
            get { return Requisicion_ID; }
            set { Requisicion_ID = value; }
        }

        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }

        public String P_Proyecto_Programa_ID
        {
            get { return Proyecto_Programa_ID; }
            set { Proyecto_Programa_ID = value; }
        }

        public String P_Area_ID
        {
            get { return Area_ID; }
            set { Area_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        #endregion

        #region (Metodos)
        public Cls_Ope_Com_Impresion_Requisiciones_Negocio()
        {
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisiciones
        ///DESCRIPCIÓN: 
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 04/21/2011 02:37:19 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Requisiciones()
        {
            return Cls_Ope_Com_Impresion_Requisiciones_Datos.Consultar_Requisiciones(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisiciones_Detalles
        ///DESCRIPCIÓN: 
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 04/21/2011 02:37:19 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Requisiciones_Detalles()
        {
            return Cls_Ope_Com_Impresion_Requisiciones_Datos.Consultar_Requisiciones_Detalles(this);
        }

        public DataTable Consultar_Detalle_Requisicion()
        {
            return Cls_Ope_Com_Impresion_Requisiciones_Datos.Consultar_Detalle_Requisicion(this);
        }
        public DataTable Consultar_Productos_Servicios()
        {
            return Cls_Ope_Com_Impresion_Requisiciones_Datos.Consultar_Productos_Servicios(this);
        }
        public DataTable Consultar_Req_Origen()
        {
            return Cls_Ope_Com_Impresion_Requisiciones_Datos.Consultar_Req_Origen(this);
        }
        #endregion
    }
}