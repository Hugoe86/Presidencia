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
using Presidencia.Generar_Requisicion.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Requisiciones_Negocio
/// </summary>
/// 

namespace Presidencia.Generar_Requisicion.Negocio
{
    public class Cls_Ope_Com_Requisiciones_Negocio
    {
        #region Variables Privadas

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
        private String Elemento_PEP;
        private String No_Reserva;
        private String Cotizador_ID;
        private String Listado_Almacen;

        private DataTable Dt_Productos_Servicios;
        private DataTable Dt_Productos_Servicios_Aux;
        private String Tipo_Articulo;
        private String Fuente_Financiamiento;

        private DataTable Dt_Productos_Almacen;
        private DataTable Dt_Partidas;
        //DEL PRODUCTO
        private String Giro_ID;
        //Del presupuesto
        private int Anio_Presupuesto;

        private String Especial_Ramo33;
        
        #endregion 

        #region Variables Publicas
        public String P_Especial_Ramo33
        {
            get { return Especial_Ramo33; }
            set { Especial_Ramo33 = value; }
        }
        public int P_Anio_Presupuesto
        {
            get { return Anio_Presupuesto; }
            set { Anio_Presupuesto = value; }
        }
        public String P_Cotizador_ID
        {
            get { return Cotizador_ID; }
            set { Cotizador_ID = value; }
        }
        public String P_Listado_Almacen
        {
            get { return Listado_Almacen; }
            set { Listado_Almacen = value; }
        }
        public String P_No_Reserva
        {
            get { return No_Reserva; }
            set { No_Reserva = value; }
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

        public String P_Elemento_PEP
        {
            get { return Elemento_PEP; }
            set { Elemento_PEP = value; }
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
        public DataTable P_Dt_Productos_Servicios_Aux
        {
            get { return Dt_Productos_Servicios_Aux; }
            set { Dt_Productos_Servicios_Aux = value; }
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

       
        #region Metodos

        public Cls_Ope_Com_Requisiciones_Negocio()
        {
        }

        public String Proceso_Actualizar_Requisicion() 
        {
            return Cls_Ope_Com_Requisiciones_Datos.Proceso_Actualizar_Requisicion(this);
        }

        public DataTable Consultar_Proyectos_Programas() 
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Proyectos_Programas(this);
        }
        //Se usa
        public DataTable Consultar_Partidas_De_Un_Programa()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Partidas_De_Un_Programa(this);
        }
        public DataTable Consultar_Productos() 
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Productos(this);
        }
        
        public DataTable Consultar_Servicios()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Servicios(this);
        }
        public DataSet Consultar_Impuesto()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Impuesto(this);
        }
        public DataTable Consultar_Requisiciones()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Requisiciones(this);
        }
        public DataTable Consultar_Requisiciones_Generales()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Requisiciones_Generales(this);
        }
        public DataTable Consultar_Requisiciones_En_Web()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Requisiciones_En_Web(this);
        }        
        public DataTable Consultar_Productos_Servicios_Requisiciones()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Productos_Servicios_Requisiciones(this);
        }
        public DataTable Consultar_Fuentes_Financiamiento()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Fuentes_Financiamiento(this);
        }
        //xxxxx
        public DataTable Consultar_Presupuesto_Partidas()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Presupuesto_Partidas(this);
        }
        public DataTable Consultar_Poducto_Por_ID()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Poducto_Por_ID(this);
        }
        public DataTable Consultar_Servicio_Por_ID()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Servicio_Por_ID(this);
        }
        public String Proceso_Insertar_Requisicion() 
        {
            return Cls_Ope_Com_Requisiciones_Datos.Proceso_Insertar_Requisicion(this);
        }
        //xxxxxxx
        //public void Comprometer_Presupuesto_Partidas_Usadas_En_Requisicion()
        //{
        //    Cls_Ope_Com_Requisiciones_Datos.Comprometer_Presupuesto_Partidas_Usadas_En_Requisicion(this);
        //}
        public bool Proceso_Filtrar()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Proceso_Filtrar(this);
        }
        public bool Registrar_Historial(String Estado, String No_Requisicion)
        {
            return Cls_Ope_Com_Requisiciones_Datos.Registrar_Historial(Estado, No_Requisicion);
        }

        public DataTable Consultar_Historial_Requisicion()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Historial_Requisicion(this);
        }
        //public bool Verificar_Rango_Caja_Chica() 
        //{
        //    return Cls_Ope_Com_Requisiciones_Datos.Verificar_Rango_Caja_Chica(this);
        //}
        public int Asignar_Reserva()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Asignar_Reserva(this);
        }
        public int Rechaza_Contabilidad()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Rechaza_Contabilidad(this);
        }

        public DataTable Consultar_Partida_Presupuestal_Por_Tipo_Requisicion()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Partida_Presupuestal_Por_Tipo_Requisicion(this);
        }
        public String Elaborar_Requisicion_A_Partir_De_Otra(int No_Requisicion)
        {
            return Cls_Ope_Com_Requisiciones_Datos.Elaborar_Requisicion_A_Partir_De_Otra(No_Requisicion);
        }
        public int Actualizar_Requisicion_Estatus()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Actualizar_Requisicion_Estatus(this);
        }
        public DataTable Consultar_Requisiciones_Reporte_Gerencial()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Requisiciones_Reporte_Gerencial(this);
        }

        public DataTable Consultar_Requisiciones_Entrega_Bienes_Transitorios()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Requisiciones_Entrega_Bienes_Transitorios(this);
        }

        public DataTable Consultar_Requisiciones_Entrega_Stock()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Requisiciones_Entrega_Stock(this);
        }

        public DataTable Consultar_Grupo_Dependencia()
        {
            return Cls_Ope_Com_Requisiciones_Datos.Consultar_Grupo_Dependencia(this);
        }

        #endregion 
    }
}
