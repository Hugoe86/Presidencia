using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Seguimiento_Ordenes.Datos;
using System.Data;




/// <summary>
/// Summary description for Cls_Ope_Com_Seguimiento_Ordenes_Compra_Negocio
/// </summary>
/// 
namespace Presidencia.Seguimiento_Ordenes.Negocio
{
    public class Cls_Ope_Com_Seguimiento_Ordenes_Compra_Negocio
    {
        #region VARIABLES INTERNAS
        private long No_Orden_Compra;
        private long No_Requisicion;
        //private long No_Consolidacion;
        private long No_Cotizacion;
        private long No_ComiteCompra;
        private long No_Licitacion;
        private String Estatus;
        private String Folio;
        private String Tipo_Articulo;
        private String Comentarios;
        private String Fecha_Entrega;

        private double Subtotal;
        private double Total_IEPS;
        private double Total_IVA;
        private double Total;
        private String Fecha_Ejercio;
        private String Usuario;

        private DataTable Dt_Proveedores;
        private DataTable Dt_Detalles;

        private String Lista_Requisiciones;
        private long No_Factura_Interno;
        private String Tipo_Compra;

        private String Fecha_Inicial;
        private String Fecha_Final;

        private String No_Reserva;
        private String Cotizador_ID;

        private String Especiales;

        private String Mensaje;
        private String Condicion1;
        private String Condicion2;
        private String Condicion3;
        private String Condicion4;
        private String Condicion5;
        private String Condicion6;

        #endregion

        #region VARIABLES PUBLICAS
        public String P_Condicion1
        {
            get { return Condicion1; }
            set { Condicion1 = value; }
        }
        public String P_Condicion2
        {
            get { return Condicion2; }
            set { Condicion2 = value; }
        }
        public String P_Condicion3
        {
            get { return Condicion3; }
            set { Condicion3 = value; }
        }
        public String P_Condicion4
        {
            get { return Condicion4; }
            set { Condicion4 = value; }
        }
        public String P_Condicion5
        {
            get { return Condicion5; }
            set { Condicion5 = value; }
        }
        public String P_Condicion6
        {
            get { return Condicion6; }
            set { Condicion6 = value; }
        }
        public String P_Mensaje
        {
            get { return Mensaje; }
            set { Mensaje = value; }
        }
        public String P_Especiales
        {
            get { return Especiales; }
            set { Especiales = value; }
        }
        public String P_Cotizador_ID
        {
            get { return Cotizador_ID; }
            set { Cotizador_ID = value; }
        }
        public String P_No_Reserva
        {
            get { return No_Reserva; }
            set { No_Reserva = value; }
        }

        public String P_Fecha_Entrega
        {
            get { return Fecha_Entrega; }
            set { Fecha_Entrega = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }
        public String P_Tipo_Articulo
        {
            get { return Tipo_Articulo; }
            set { Tipo_Articulo = value; }
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
        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public long P_No_Factura_Interno
        {
            get { return No_Factura_Interno; }
            set { No_Factura_Interno = value; }
        }
        public String P_Lista_Requisiciones
        {
            get { return Lista_Requisiciones; }
            set { Lista_Requisiciones = value; }
        }

        public long P_No_Cotizacion
        {
            get { return No_Cotizacion; }
            set { No_Cotizacion = value; }
        }
        public long P_No_ComiteCompra
        {
            get { return No_ComiteCompra; }
            set { No_ComiteCompra = value; }
        }
        public long P_No_Licitacion
        {
            get { return No_Licitacion; }
            set { No_Licitacion = value; }
        }

        public long P_No_Orden_Compra
        {
            get { return No_Orden_Compra; }
            set { No_Orden_Compra = value; }
        }

        public long P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }
        //public long P_No_Consolidacion
        //{
        //    get { return No_Consolidacion; }
        //    set { No_Consolidacion = value; }
        //}
        public String P_Tipo_Compra
        {
            get { return Tipo_Compra; }
            set { Tipo_Compra = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public double P_Subtotal
        {
            get { return Subtotal; }
            set { Subtotal = value; }
        }
        public double P_Total_IEPS
        {
            get { return Total_IEPS; }
            set { Total_IEPS = value; }
        }
        public double P_Total_IVA
        {
            get { return Total_IVA; }
            set { Total_IVA = value; }
        }
        public double P_Total
        {
            get { return Total; }
            set { Total = value; }
        }
        public DataTable P_Dt_Detalles_Orden_Compra
        {
            get { return Dt_Detalles; }
            set { Dt_Detalles = value; }
        }
        public DataTable P_Dt_Proveedores
        {
            get { return Dt_Proveedores; }
            set { Dt_Proveedores = value; }
        }
        public String P_Fecha_Ejercio
        {
            get { return Fecha_Ejercio; }
            set { Fecha_Ejercio = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        #endregion


        public DataTable Consultar_Ordenes_Compra()
        {
            return Cls_Ope_Com_Seguimiento_Ordenes_Compra_Datos.Consultar_Ordenes_Compra(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Cabecera_Orden_Compra
        /// DESCRIPCION:            Instancia al método "Consulta_Cabecera_Orden_Compra" el cual retorna
        ///                         una tabla con la información de la orden de compra
        /// PARAMETROS :            
        /// CREO       :            Susana Trigueros Armenta
        /// FECHA_CREO :            24/SEP/12
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Cabecera_Orden_Compra()
        {
            return Cls_Ope_Com_Seguimiento_Ordenes_Compra_Datos.Consulta_Cabecera_Orden_Compra(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Detalles_Orden_Compra
        /// DESCRIPCION:            Instancia al método "Consulta_Cabecera_Orden_Compra" el cual retorna
        ///                         una tabla con los productos de la orden de compra
        /// PARAMETROS :            
        /// CREO       :            Susana Trigueros Armenta
        /// FECHA_CREO :            24/Sep/12
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Detalles_Orden_Compra()
        {
            return Cls_Ope_Com_Seguimiento_Ordenes_Compra_Datos.Consulta_Detalles_Orden_Compra(this);
        }

    }
}