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
using Presidencia.Cotizacion_Manual.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Cotizacion_Manua_Negocio
/// </summary>
/// 
namespace Presidencia.Cotizacion_Manual.Negocio
{
public class Cls_Ope_Com_Cotizacion_Manual_Negocio
{
    ///*******************************************************************************
    /// VARIABLES INTERNAS 
    ///******************************************************************************
    #region Variables_Internas
    private String No_Requisicion;
    private DataTable Dt_Productos;
    private String Tipo_Articulo;
    private String Concepto_ID;
    private String Producto_ID;
    private String IVA_Cotizado;
    private String IEPS_Cotizado;
    private String Total_Cotizado;
    private String Subtotal_Cotizado;
    private String Estatus;

    

    #endregion


    ///*******************************************************************************
    /// VARIABLES PUBLICAS
    ///*******************************************************************************
    #region Variables_Publicas

    public String P_No_Requisicion
    {
        get { return No_Requisicion; }
        set { No_Requisicion = value; }
    }
    public String P_IEPS_Cotizado
    {
        get { return IEPS_Cotizado; }
        set { IEPS_Cotizado = value; }
    }
    public String P_IVA_Cotizado
    {
        get { return IVA_Cotizado; }
        set { IVA_Cotizado = value; }
    }

    public String P_Estatus
    {
        get { return Estatus; }
        set { Estatus = value; }
    }

    public String P_Subtotal_Cotizado
    {
        get { return Subtotal_Cotizado; }
        set { Subtotal_Cotizado = value; }
    }


    public DataTable P_Dt_Productos
    {
        get { return Dt_Productos; }
        set { Dt_Productos = value; }
    }

    public String P_Tipo_Articulo
    {
        get { return Tipo_Articulo; }
        set { Tipo_Articulo = value; }
    }

    public String P_Total_Cotizado
    {
        get { return Total_Cotizado; }
        set { Total_Cotizado = value; }
    }
    public String P_Concepto_ID
    {
        get { return Concepto_ID; }
        set { Concepto_ID = value; }
    }

    public String P_Producto_ID
    {
        get { return Producto_ID; }
        set { Producto_ID = value; }
    }

    #endregion

    ///*******************************************************************************
    /// METODOS
    ///*******************************************************************************
    #region Metodos

    public DataTable Consultar_Productos_Servicios()
    {
        return Cls_Ope_Com_Cotizacion_Manual_Datos.Consultar_Productos_Servicios(this);
    }

    public DataTable Consultar_Requisiciones()
    {
        return Cls_Ope_Com_Cotizacion_Manual_Datos.Consultar_Requisiciones();
    }

    public DataTable Consultar_Detalle_Requisicion()
    {
        return Cls_Ope_Com_Cotizacion_Manual_Datos.Consultar_Detalle_Requisicion(this);
    }

    public DataTable Consultar_Proveedores()
    {
        return Cls_Ope_Com_Cotizacion_Manual_Datos.Consultar_Proveedores(this);
    }

    public DataTable Consultar_Impuesto_Producto()
    {
        return Cls_Ope_Com_Cotizacion_Manual_Datos.Consultar_Impuesto_Producto(this);
    }

    public bool Agregar_Cotizaciones()
    {
        return Cls_Ope_Com_Cotizacion_Manual_Datos.Agregar_Cotizaciones(this);
    }

    public bool Modificar_Requisicion()
    {

        return Cls_Ope_Com_Cotizacion_Manual_Datos.Modificar_Requisicion(this);
    }

    #endregion

}
}//fin ddel namespace
