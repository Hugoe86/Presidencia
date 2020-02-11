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
using Presidencia.Registro_Gastos.Datos;


namespace Presidencia.Registro_Gastos.Negocio
{
    public class Cls_Ope_Com_Registro_Gastos_Negocio
    {
        public Cls_Ope_Com_Registro_Gastos_Negocio()
        {
        }
            #region Variables Privadas

            private String Dependencia_ID;
            private String Proyecto_Programa_ID;
            private String Partida_ID;
            private String Gasto_ID;
            private DataSet Ds_Productos;
            private DataSet Ds_Servicios;
            private DataSet Ds_Productos_Tmp;
            private DataSet Ds_Servicios_Tmp;
            private String Folio;
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


            public String P_Folio
            {
                get { return Folio; }
                set { Folio = value; }
            }

            public String P_Gasto_ID
            {
                get { return Gasto_ID; }
                set { Gasto_ID = value; }
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


            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }

            #endregion


            #region Metodos


            public void Proceso_Actualizar_Gasto()
            {
                Cls_Ope_Com_Registro_Gastos_Datos.Proceso_Actualizar_Gasto(this);
            }

            public DataTable Consultar_Proyectos_Programas()
            {
                return Cls_Ope_Com_Registro_Gastos_Datos.Consultar_Proyectos_Programas(this);
            }
            //Se usa
            public DataTable Consultar_Partidas_De_Un_Programa()
            {
                return Cls_Ope_Com_Registro_Gastos_Datos.Consultar_Partidas_De_Un_Programa(this);
            }
            //public DataTable Consultar_Productos()
            //{
            //    return Cls_Ope_Com_Registro_Gastos_Datos.Consultar_Productos(this);
            //}

            //public DataTable Consultar_Servicios()
            //{
            //    return Cls_Ope_Com_Registro_Gastos_Datos.Consultar_Servicios(this);
            //}
            public DataSet Consultar_Impuesto()
            {
                return Cls_Ope_Com_Registro_Gastos_Datos.Consultar_Impuesto(this);
            }
            public DataTable Consultar_Gastos()
            {
                return Cls_Ope_Com_Registro_Gastos_Datos.Consultar_Gastos(this);
            }
            public DataTable Consultar_Productos_Servicios()
            {
                return Cls_Ope_Com_Registro_Gastos_Datos.Consultar_Productos_Servicios(this);
            }
            public DataTable Consultar_Fuentes_Financiamiento()
            {
                return Cls_Ope_Com_Registro_Gastos_Datos.Consultar_Fuentes_Financiamiento(this);
            }
            public DataTable Consultar_Presupuesto_Partidas()
            {
                return Cls_Ope_Com_Registro_Gastos_Datos.Consultar_Presupuesto_Partidas(this);
            }
            //public DataTable Consultar_Poducto_Por_ID()
            //{
            //    return Cls_Ope_Com_Registro_Gastos_Datos.Consultar_Poducto_Por_ID(this);
            //}
            //public DataTable Consultar_Servicio_Por_ID()
            //{
            //    return Cls_Ope_Com_Registro_Gastos_Datos.Consultar_Servicio_Por_ID(this);
            //}
            public void Proceso_Registrar_Gasto()
            {
                Cls_Ope_Com_Registro_Gastos_Datos.Proceso_Registrar_Gasto(this);
            }
            #endregion
        

    }
}