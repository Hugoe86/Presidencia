using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio
/// </summary>
public class Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Negocio
{
    #region variables privadas

    private String Anio;
    #endregion

    #region variables publicas
    public String P_Anio
    {
        get { return Anio; }
        set { Anio = value; }
    }


    #endregion

    #region Metodos
    public DataTable Consultar_Avaluos_Asignados_Primera_Entrega()
    {
        return Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Datos.Consultar_Avaluos_Asignados_Primera_Entrega(this);
    }
    public DataTable Consultar_Avaluos_Asignados_Segunda_Entrega()
    {
        return Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Datos.Consultar_Avaluos_Asignados_Segunda_Entrega(this);
    }
    public DataTable Consultar_Avaluos_Asignados_Tercera_Entrega()
    {
        return Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Datos.Consultar_Avaluos_Asignados_Tercera_Entrega(this);
    }
    public DataTable Consultar_Avaluos_Asignados_Cuarta_Entrega()
    {
        return Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Datos.Consultar_Avaluos_Asignados_Cuarta_Entrega(this);
    }
    public DataTable Consultar_Avaluos_Asignados_Quinta_Entrega()
    {
        return Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Datos.Consultar_Avaluos_Asignados_Quinta_Entrega(this);
    }
    public DataTable Consultar_Avaluos_Asignados_Sexta_Entrega()
    {
        return Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Datos.Consultar_Avaluos_Asignados_Sexta_Entrega(this);
    }
    public DataTable Consultar_Avaluos_Asignados_Septima_Entrega()
    {
        return Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Datos.Consultar_Avaluos_Asignados_Septima_Entrega(this);
    }
    public DataTable Consultar_Avaluos_Fiscales()
    {
        return Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Datos.Consultar_Avaluos_Fiscales(this);
    }
    public DataTable Consultar_Metas_Autorizacion()
    {
        return Cls_Rpt_Cat_Imprimir_Numero_Avaluos_Datos.Consultar_Metas_Autorizacion(this);
    }
    #endregion
}

