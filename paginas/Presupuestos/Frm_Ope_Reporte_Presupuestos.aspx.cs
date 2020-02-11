using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Constantes;
using Presidencia.Reporte_Presupuestos.Negocios;

public partial class paginas_presupuestos_Frm_Ope_Reporte_Presupuestos : System.Web.UI.Page
{

    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //El unico que llenamos es el de Dependencias, Capitulos, Partida Generica
            Llenar_Combo_Unidad_Responsable();
            Llenar_Combo_Programas();
            Llenar_Combo_Fte_Financiamiento();
            Llenar_Combo_Capitulos();
            Llenar_Combo_Conceptos();
            Llenar_Combo_Partidas_Genericas();
            Llenar_Combo_Partidas_Especificas();
            Txt_Anio.Text = "";
           
        }
    }

    #endregion


    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    #region Metodos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Unidad_Responsable
    ///DESCRIPCIÓN: Metodo que llena el combo de Unidad responsable
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 19/OCT/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Unidad_Responsable()
    {
        Cmb_U_Responsable.Items.Clear();
        Cls_Ope_Reporte_Presupuestos_Negocio Negocios = new Cls_Ope_Reporte_Presupuestos_Negocio();
        DataTable Data_Table = Negocios.Consultar_Dependencias();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_U_Responsable, Data_Table);
        Cmb_U_Responsable.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Unidad_Responsable
    ///DESCRIPCIÓN: Metodo que llena el combo de Unidad responsable
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 19/OCT/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Programas()
    {
        Cmb_Programas.Items.Clear();
        Cls_Ope_Reporte_Presupuestos_Negocio Negocios = new Cls_Ope_Reporte_Presupuestos_Negocio();
        DataTable Data_Table = Negocios.Consultar_Programas();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Programas, Data_Table);
        Cmb_Programas.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Fte_Financiamiento
    ///DESCRIPCIÓN: Metodo que llena el combo de Fte_Financiamiento
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 19/OCT/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Fte_Financiamiento()
    {
        Cmb_Fte_Financiamiento.Items.Clear();
        Cls_Ope_Reporte_Presupuestos_Negocio Negocios = new Cls_Ope_Reporte_Presupuestos_Negocio();
        DataTable Data_Table = Negocios.Consultar_Fuentes_Financiamiento();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Fte_Financiamiento, Data_Table);
        Cmb_Fte_Financiamiento.SelectedIndex = 0;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Capitulos
    ///DESCRIPCIÓN: Metodo que llena el combo de Capitulos
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 19/OCT/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Capitulos()
    {
        Cmb_Capitulo.Items.Clear();
        Cls_Ope_Reporte_Presupuestos_Negocio Negocios = new Cls_Ope_Reporte_Presupuestos_Negocio();
        DataTable Data_Table = Negocios.Consultar_Capitulos();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Capitulo, Data_Table);
        Cmb_Capitulo.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Conceptos
    ///DESCRIPCIÓN: Metodo que llena el combo de Conceptos
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 19/OCT/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Conceptos()
    {
        Cmb_Concepto.Items.Clear();
        Cls_Ope_Reporte_Presupuestos_Negocio Negocios = new Cls_Ope_Reporte_Presupuestos_Negocio();
        DataTable Data_Table = Negocios.Consultar_Conceptos();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Concepto, Data_Table);
        Cmb_Concepto.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Partidas_Genericas
    ///DESCRIPCIÓN: Metodo que llena el combo de Partida_Generica
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 19/OCT/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Partidas_Genericas()
    {
        Cmb_Partida_Generica.Items.Clear();
        Cls_Ope_Reporte_Presupuestos_Negocio Negocios = new Cls_Ope_Reporte_Presupuestos_Negocio();
        DataTable Data_Table = Negocios.Consultar_Partidas_Genericas();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Partida_Generica, Data_Table);
        Cmb_Partida_Generica.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Partidas_Especificas
    ///DESCRIPCIÓN: Metodo que llena el combo de Partidas_Especificas
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 19/OCT/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Partidas_Especificas()
    {
        Cmb_Partida_Especifica.Items.Clear();
        Cls_Ope_Reporte_Presupuestos_Negocio Negocios = new Cls_Ope_Reporte_Presupuestos_Negocio();
        DataTable Data_Table = Negocios.Consultar_Partida_Especifica();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Partida_Especifica, Data_Table);
        Cmb_Partida_Especifica.SelectedIndex = 0;
    }

    
    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    #endregion


    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Click
    ///DESCRIPCIÓN: Metodo que ejecuta el evento del boton limpiar
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 19/OCT/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        //El unico que llenamos es el de Dependencias, Capitulos, Partida Generica
        Llenar_Combo_Unidad_Responsable();
        Llenar_Combo_Fte_Financiamiento();
        Llenar_Combo_Capitulos();
        Llenar_Combo_Conceptos();
        Llenar_Combo_Partidas_Genericas();
        Llenar_Combo_Partidas_Especificas();
        Txt_Anio.Text = "";
        Session["Filtros_Seleccionados"] = null;
        Session["Dt_Presupuestos"] = null;
        Grid_Presupuestos.DataSource = new DataTable();
        Grid_Presupuestos.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Btn_Buscar_Click
    ///DESCRIPCIÓN: Metodo que ejecuta el evento del boton Buscar
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 19/OCT/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Session["Filtros_Seleccionados"] = null;
        Session["Dt_Presupuestos"] = null;
        Grid_Presupuestos.DataSource = new DataTable();
        Grid_Presupuestos.DataBind();

        Cls_Ope_Reporte_Presupuestos_Negocio Clase_Negocio = new Cls_Ope_Reporte_Presupuestos_Negocio();
        String Filtros_Seleccionados="";
        if (Cmb_U_Responsable.SelectedIndex != 0)
        {
            Clase_Negocio.P_Dependencia_ID = Cmb_U_Responsable.SelectedValue;
            Filtros_Seleccionados = Filtros_Seleccionados + "<<U. Responsable>> " + Cmb_U_Responsable.SelectedItem.Text + ".";
        }
        if (Cmb_Programas.SelectedIndex != 0)
        {

        }
        if (Cmb_Fte_Financiamiento.SelectedIndex != 0)
        {
            Clase_Negocio.P_Fuente_Financiamiento_ID = Cmb_Fte_Financiamiento.SelectedValue;
            Filtros_Seleccionados = Filtros_Seleccionados +"<<Fte. Financiamiento>> " + Cmb_Fte_Financiamiento.SelectedItem.Text + ".";
        }
        if (Cmb_Concepto.SelectedIndex != 0)
        {
            Clase_Negocio.P_Concepto_ID = Cmb_Concepto.SelectedValue;
            Filtros_Seleccionados = Filtros_Seleccionados + "<<Concepto>> " + Cmb_Concepto.SelectedItem.Text + ".";
        }
        if (Cmb_Capitulo.SelectedIndex != 0)
        {
            Clase_Negocio.P_Capitulo_ID = Cmb_Capitulo.SelectedValue;
            Filtros_Seleccionados = Filtros_Seleccionados + "<<Capitulo>> " + Cmb_Capitulo.SelectedItem.Text + ".";
        }
        if (Cmb_Partida_Generica.SelectedIndex != 0)
        {
            Clase_Negocio.P_Partida_Generica_ID = Cmb_Partida_Generica.SelectedValue;
            Filtros_Seleccionados = Filtros_Seleccionados + "<<Partida Generica>> " + Cmb_Partida_Generica.SelectedItem.Text + ".";
        }
        if (Cmb_Partida_Especifica.SelectedIndex != 0)
        {
            Clase_Negocio.P_Partida_Especifica_ID = Cmb_Partida_Especifica.SelectedValue;
            Filtros_Seleccionados = Filtros_Seleccionados + "<<Partida Especifica>> " + Cmb_Partida_Especifica.SelectedItem.Text + ".";
        }
        if (Txt_Anio.Text.Trim() != String.Empty)
        {
            //Verificamos que el año sea con el formato valido
            if(Txt_Anio.Text.Length != 4)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Año no Valido";

            }
            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                Clase_Negocio.P_Anio = Txt_Anio.Text.Trim();
                Filtros_Seleccionados = Filtros_Seleccionados + "<<Año>> " + Txt_Anio.Text + ".";
            }
        }

        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            DataTable Dt_Presupuestos = Clase_Negocio.Consultar_Presupuestos();
            Session["Filtros_Seleccionados"] = Filtros_Seleccionados;
            if (Dt_Presupuestos.Rows.Count != 0)
            {
                //Grid_Presupuestos.DataSource = Dt_Presupuestos;
                //Grid_Presupuestos.DataBind();
                Session["Dt_Presupuestos"] = Dt_Presupuestos;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "W",
                "window.open('" + "../Presupuestos/Frm_Vista_Previa_Presupuestos.aspx" + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);


            }
            else
            {
                Grid_Presupuestos.EmptyDataText = "No se encontraron registros";
                Grid_Presupuestos.DataSource = new DataTable();
                Grid_Presupuestos.DataBind();
            }
        }

    }



    #endregion


   
}
