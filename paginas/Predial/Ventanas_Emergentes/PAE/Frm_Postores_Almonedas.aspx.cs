using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Predial_Pae_Postores.Negocio;


public partial class paginas_Predial_Ventanas_Emergentes_PAE_Frm_Postores_Almonedas : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Hdn_No_Det_Etapa.Value = Request.QueryString["NO_DETALLE_ETAPA"].ToString();
            Hdn_Avaluo.Value = Request.QueryString["POSTURA_LEGAL"].ToString();
            Session["AGREGA_POSTOR"] = false;
            Txt_Porcentaje.Text = "10";
            Carga_Porcentaje("0.1");
            Carga_Postores();
        }
        Frm_Postores_Alomedas.Page.Title = "Postores";
        Lbl_Title.Text = "Postores";
        Mensaje_Error();
    }
    #endregion

    #region Metodos
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Armando Zavala Moreno.
    ///FECHA_CREO  : 17-Abril-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        IBtn_Imagen_Error.Visible = true;
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Ecabezado_Mensaje.Text += P_Mensaje;// +"</br>";
        Div_Contenedor_Msj_Error.Visible = true;

    }
    private void Mensaje_Error()
    {
        IBtn_Imagen_Error.Visible = false;
        Lbl_Ecabezado_Mensaje.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Validar
    ///DESCRIPCIÓN          : Valida los campos para insertar un nuevo postor
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 17/04/2012 04:40:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar()
    {
        Boolean Validacion = true;

        if (Txt_Nombre.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce el nombre del postor <br>";
            Validacion = false;
        }
        if (Txt_Deposito.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce el deposito <br>";
            Validacion = false;
        }
        if (Txt_Porcentaje.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce el porcentaje <br>";
            Validacion = false;
        }
        if (Txt_Domicilio.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce el domicilio del postor <br>";
            Validacion = false;
        }
        if (Txt_Telefono.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce el telefono del postor <br>";
            Validacion = false;
        }
        if (Txt_RFC.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce el RFC del postor <br>";
            Validacion = false;
        }
        if (Cmb_Sexo.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce el sexo <br>";
            Validacion = false;
        }
        if (Cmb_Estado_Civil.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce el estado civil <br>";
            Validacion = false;
        }
        return Validacion;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Carga_Porcentaje
    ///DESCRIPCIÓN          : 
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 12/05/2012 04:40:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Carga_Porcentaje(String Porcentaje)
    {
        Double Avaluo = Convert.ToDouble(Hdn_Avaluo.Value);
        Txt_Deposito.Text = String.Format("{0:C2}", (Avaluo * Convert.ToDouble(Porcentaje.ToString())));
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Carga_Postores
    ///DESCRIPCIÓN          : 
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 12/05/2012 04:40:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Carga_Postores()
    {
        try
        {
            Cls_Ope_Pre_Pae_Postores_Negocio Postores = new Cls_Ope_Pre_Pae_Postores_Negocio();
            DataTable Dt_Postores;
            Postores.P_No_Detalle_Etapa = Hdn_No_Det_Etapa.Value;
            Dt_Postores = Postores.Busqueda_Pae_Postores();
            Grid_Postores.DataSource = Dt_Postores;
            Grid_Postores.DataBind();
            Session["Postores"] = Dt_Postores;
        }
        catch(Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #region Tablas
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Postores
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para los postores
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 03/02/2012 05:10:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Postores()
    {
        DataTable Dt_Generadas = new DataTable();
        Dt_Generadas.Columns.Add(new DataColumn("NOMBRE_POSTOR", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("DEPOSITO", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("PORCENTAJE", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("DOMICILIO", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("TELEFONO", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("RFC", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("SEXO", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("ESTADO_CIVIL", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("NO_DETALLE_ETAPA", typeof(String)));
        return Dt_Generadas;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_A_Etapa
    ///DESCRIPCIÓN          : Agrega una nueva fila a las cuentas a generar y Caulcula el Perido, Rezago, etc
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/02/2012 04:38:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Postor(DataTable Dt_Generadas)
    {
        DataRow Dr_Generadas;
        Dr_Generadas = Dt_Generadas.NewRow();
        Dr_Generadas["NOMBRE_POSTOR"] = Txt_Nombre.Text;
        Dr_Generadas["DEPOSITO"] = Txt_Deposito.Text;
        Dr_Generadas["PORCENTAJE"] = Txt_Porcentaje.Text;
        Dr_Generadas["DOMICILIO"] = Txt_Domicilio.Text;
        Dr_Generadas["TELEFONO"] = Txt_Telefono.Text;
        Dr_Generadas["RFC"] = Txt_RFC.Text;
        Dr_Generadas["SEXO"] = Cmb_Sexo.SelectedItem.Text;
        Dr_Generadas["ESTADO_CIVIL"] = Cmb_Estado_Civil.SelectedItem.Text;
        Dr_Generadas["ESTATUS"] = "VIGENTE";
        Dr_Generadas["NO_DETALLE_ETAPA"] = Hdn_No_Det_Etapa.Value;
        Dt_Generadas.Rows.Add(Dr_Generadas);//Se asigna la nueva fila a la tabla
    }
    #endregion
    #endregion

    #region Eventos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Porcentaje_TextChanged
    ///DESCRIPCIÓN          : Valida el porcentaje y calcula el deposito
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 12/Mayo/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Porcentaje_TextChanged(object sender, EventArgs e)
    {
        if (Txt_Porcentaje.Text != "")
        {
            if (Convert.ToInt16(Txt_Porcentaje.Text) < 10) 
            { 
                Carga_Porcentaje("0.1");
                Txt_Porcentaje.Text = "10";
            }
            if (Convert.ToInt16(Txt_Porcentaje.Text) > 99) 
            { 
                Carga_Porcentaje("1");
                Txt_Porcentaje.Text = "100";
            }
            if (Convert.ToInt16(Txt_Porcentaje.Text) > 9 && Convert.ToInt16(Txt_Porcentaje.Text) < 100)
            {
                Carga_Porcentaje("0." + Txt_Porcentaje.Text);
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Acepta el motivo de omision
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 13/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Postores = Crear_Tabla_Postores();
        Mensaje_Error();
        if (Validar())
        {
            Llenar_DataRow_Postor(Dt_Postores);
            if (Dt_Postores.Rows.Count > 0)
            {
                Session["Postor_Nuevo"] = Dt_Postores;
                Session["AGREGA_GASTOS"] = true;
                //Cierra la ventana
                string Pagina = "<script language='JavaScript'>";
                Pagina += "window.close();";
                Pagina += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
            }
            else
            {
                Mensaje_Error("No se introdujo ningun dato para guardar");
            }
        }
        else
        {
            IBtn_Imagen_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 14/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["AGREGA_POSTORES"] = false;
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }
    #endregion

    #region Grids
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Publicacion_SelectedIndexChanged
    ///DESCRIPCIÓN          : Borra un Registro del Grid Publicacion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/03/2012 05:53:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Postores_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Postores = (DataTable)Session["Postores"];
        Dt_Postores.Rows.RemoveAt(Grid_Postores.SelectedIndex);
        Grid_Postores.DataSource = Dt_Postores;
        Grid_Postores.DataBind();
        Session["Postores"] = Dt_Postores;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Publicacion_PageIndexChanging
    ///DESCRIPCIÓN          : Cambia de pagina en el Grid Publicacion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/03/2012 05:53:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Postores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Postores.PageIndex = e.NewPageIndex;
            Grid_Postores.DataSource = Session["Postores"];
            Grid_Postores.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
     
    #endregion
}
