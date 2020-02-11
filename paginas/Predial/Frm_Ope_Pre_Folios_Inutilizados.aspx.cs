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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Operacion_Folios_Inutilizados.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Predial_Frm_Ope_Pre_Folios_Inutilizados : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 25/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Configuracion_Acceso("Frm_Ope_Pre_Folios_Inutilizados.aspx");
                Configuracion_Formulario(true);
                Consulta_Caja_Empleado();
                Txt_Cajero.Text = Cls_Sessiones.Nombre_Empleado;
                Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Llenar_Tabla_Folios_Inutilizados(0);
                Llenar_Combo_Motivos();
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///****************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 25/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        Btn_Nuevo.Visible = true;
        //Btn_Nuevo.ToolTip = "Nuevo";
        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Txt_Recibo.Enabled = !Estatus;
        Txt_Cajero.Enabled = !Estatus;
        Txt_Fecha.Enabled = false;
        Cmb_Motivo.Enabled = !Estatus;
        Txt_Observaciones.Enabled = !Estatus;
        Grid_Folios_Inutilizados.SelectedIndex = (-1);
        Btn_Buscar.Enabled = Estatus;
        Txt_Busqueda.Enabled = Estatus;
        Txt_Folio_Fin.Enabled = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 25/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        //Txt_Caja_ID.Text = "";
        Txt_Recibo.Text = "";
        //Txt_Cajero.Text = "";
        //Txt_Fecha.Text = "";
        Txt_Observaciones.Text = "";
        Txt_Pago_ID.Text = "";
        Cmb_Motivo.SelectedIndex = 0;
        //Grid_Folios_Inutilizados.DataSource = new DataTable();
        //Grid_Folios_Inutilizados.DataBind();
        Txt_Folio_Fin.Text = "";
        Txt_Busqueda.Text = "";
        //Txt_Modulo.Text = "";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Caja_Empleado
    /// DESCRIPCION : Consulta la caja que tiene abierto el empleado
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 22-Agosto-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Caja_Empleado()
    {
        DataTable Dt_Caja; //Variable que obtendra los datos de la consulta 
        Cls_Ope_Pre_Folios_Inutilizados_Negocio Rs_Consulta_Cat_Pre_Cajas = new Cls_Ope_Pre_Folios_Inutilizados_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Rs_Consulta_Cat_Pre_Cajas.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Dt_Caja = Rs_Consulta_Cat_Pre_Cajas.Consulta_Caja_Empleado();
            if (Dt_Caja.Rows.Count > 0)
            {
                //Muestra todos los datos que tiene el folio que proporciono el usuario
                foreach (DataRow Registro in Dt_Caja.Rows)
                {
                    Txt_Caja_ID.Text = Registro[Ope_Caj_Turnos.Campo_Caja_Id].ToString();
                    Hfd_No_Turno.Value = Registro[Ope_Caj_Turnos.Campo_No_Turno].ToString();
                    Txt_Caja.Text = Registro["Caja"].ToString();
                    Txt_Modulo.Text = Registro["Modulo"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Caja_Empleado " + ex.Message.ToString(), ex);
        }
    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación que se vea afectada en la basae de datos.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 25/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (string.IsNullOrEmpty(Txt_Recibo.Text.Trim()))
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir el Número de Folio.";
            Validacion = false;
        }
        if (!string.IsNullOrEmpty(Txt_Folio_Fin.Text.Trim()))
        {
            if (!string.IsNullOrEmpty(Txt_Recibo.Text.Trim()))
            {
                Validacion = Validar_Folios();
                if(!Validacion){
                    Mensaje_Error = Mensaje_Error + "+ El número de folio esta siendo utilizado, favor de introducir un rango correcto .";
                }
            }
        }
        if (Txt_Fecha.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha";
            Validacion = false;
        }
        if (Txt_Observaciones.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir las Observaciones";
            Validacion = false;
        }
        if (Cmb_Motivo.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar un elemnto del combo Motivo.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    public Boolean Validar_Folios() {
        Cls_Ope_Pre_Folios_Inutilizados_Negocio Folios_Inutilizados_Negocio = new Cls_Ope_Pre_Folios_Inutilizados_Negocio(); 
        Boolean Datos_Validos = true;
        DataTable Dt_Folios = new DataTable();
        Int32 Inicio = Convert.ToInt32(Txt_Recibo.Text.Trim());
        Int32 Fin = Convert.ToInt32(Txt_Folio_Fin.Text.Trim());
        DataTable Dt_Folios_Utilizados = new DataTable();
        DataRow Fila_Folios;

        Folios_Inutilizados_Negocio.P_No_Recibo = Txt_Recibo.Text.Trim();
        Folios_Inutilizados_Negocio.P_No_Folio_Fin = Txt_Folio_Fin.Text.Trim();
        Dt_Folios_Utilizados = Folios_Inutilizados_Negocio.Consultar_Folios_Utilizados();
        Dt_Folios.Columns.Add("No_Folio");

        for (Int32 i = Inicio; i <= Fin; i++ )
        {
            foreach (DataRow Dr in Dt_Folios_Utilizados.Rows) {
                if (Dr["NO_RECIBO"].ToString().Equals(Convert.ToString(i)))
                {
                    Datos_Validos = false;
                    Session["Dt_Folios"] = null;
                    break;
                }
            }
            Fila_Folios = Dt_Folios.NewRow();
            Fila_Folios["No_Folio"] = Convert.ToString(i);
            Dt_Folios.Rows.Add(Fila_Folios);
            Session["Dt_Folios"] = Dt_Folios;
        }
        return Datos_Validos;
    }

    #endregion

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Folios_Inutilizados
    ///DESCRIPCIÓN: Llena la tabla de Folios Inutilizados
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_View
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 25/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Folios_Inutilizados(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Folios_Inutilizados_Negocio Folios = new Cls_Ope_Pre_Folios_Inutilizados_Negocio();
            Folios.P_Caja_ID = Txt_Caja_ID.Text;
            Folios.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Folios.P_No_Turno = Hfd_No_Turno.Value;
            Folios.P_Fecha = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Txt_Fecha.Text.ToString()));
            Grid_Folios_Inutilizados.DataSource = Folios.Consultar_Recibos();
            Grid_Folios_Inutilizados.PageIndex = Pagina;
            Grid_Folios_Inutilizados.Columns[1].Visible = true;
            Grid_Folios_Inutilizados.Columns[3].Visible = true;
            Grid_Folios_Inutilizados.Columns[6].Visible = true;
            Grid_Folios_Inutilizados.Columns[8].Visible = true;
            Grid_Folios_Inutilizados.DataBind();
            Grid_Folios_Inutilizados.Columns[1].Visible = false;
            Grid_Folios_Inutilizados.Columns[3].Visible = false;
            Grid_Folios_Inutilizados.Columns[6].Visible = false;
            Grid_Folios_Inutilizados.Columns[8].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Folios_Inutilizados_Busqueda
    ///DESCRIPCIÓN: Llena la tabla de Folios Inutilizados de auerdo a la busqueda introducida.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 25/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Folios_Inutilizados_Busqueda(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Folios_Inutilizados_Negocio Folios = new Cls_Ope_Pre_Folios_Inutilizados_Negocio();
            Folios.P_No_Recibo = Txt_Busqueda.Text.ToUpper().Trim();
            Grid_Folios_Inutilizados.DataSource = Folios.Consultar_Recibos_Busqueda();
            Grid_Folios_Inutilizados.PageIndex = Pagina;
            Grid_Folios_Inutilizados.Columns[1].Visible = true;
            Grid_Folios_Inutilizados.Columns[3].Visible = true;
            Grid_Folios_Inutilizados.Columns[6].Visible = true;
            Grid_Folios_Inutilizados.Columns[8].Visible = true;
            Grid_Folios_Inutilizados.DataBind();
            Grid_Folios_Inutilizados.Columns[1].Visible = false;
            Grid_Folios_Inutilizados.Columns[3].Visible = false;
            Grid_Folios_Inutilizados.Columns[6].Visible = false;
            Grid_Folios_Inutilizados.Columns[8].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Motivos
    ///DESCRIPCIÓN: Metodo que llena el Combo de Motivos con los motivos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 25/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Motivos()
    {
        try
        {
            Cls_Ope_Pre_Folios_Inutilizados_Negocio Folio = new Cls_Ope_Pre_Folios_Inutilizados_Negocio();
            DataTable Folios = Folio.Consultar_Motivos();
            DataRow fila = Folios.NewRow();
            fila[Cat_Pre_Motivos.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Motivos.Campo_Motivo_ID] = "SELECCIONE";
            Folios.Rows.InsertAt(fila, 0);
            Cmb_Motivo.DataTextField = Cat_Pre_Motivos.Campo_Nombre;
            Cmb_Motivo.DataValueField = Cat_Pre_Motivos.Campo_Motivo_ID;
            Cmb_Motivo.DataSource = Folios;
            Cmb_Motivo.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Folios_Inutilizados_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Folios Inutilizados
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 25/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Folios_Inutilizados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Grid_Folios_Inutilizados.SelectedIndex = (-1);
            Llenar_Tabla_Folios_Inutilizados(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Folios_Inutilizados_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos del Folio para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 25/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Folios_Inutilizados_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Folios_Inutilizados.SelectedIndex > (-1))
            {
                Txt_Pago_ID.Text = Grid_Folios_Inutilizados.SelectedRow.Cells[1].Text;
                Txt_Recibo.Text = Grid_Folios_Inutilizados.SelectedRow.Cells[2].Text;
                Txt_Caja_ID.Text = Grid_Folios_Inutilizados.SelectedRow.Cells[3].Text;
                Txt_Cajero.Text =  Grid_Folios_Inutilizados.SelectedRow.Cells[4].Text;
                Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}",Grid_Folios_Inutilizados.SelectedRow.Cells[5].Text);
                Txt_Observaciones.Text = Grid_Folios_Inutilizados.SelectedRow.Cells[8].Text;
                Cmb_Motivo.SelectedIndex = Cmb_Motivo.Items.IndexOf(Cmb_Motivo.Items.FindByValue(Grid_Folios_Inutilizados.SelectedRow.Cells[6].Text));
                System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un Folio Intilizado
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 25/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Limpiar_Catalogo();
                Btn_Nuevo.ToolTip  = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Imprimir.Visible = false;
                Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Cajero.Enabled = false;
                Txt_Cajero.Text = Cls_Sessiones.Nombre_Empleado;
                Cls_Ope_Pre_Folios_Inutilizados_Negocio Folios = new Cls_Ope_Pre_Folios_Inutilizados_Negocio();
                Folios.P_Empleado_ID = Cls_Sessiones.Empleado_ID;  
                Txt_Caja_ID.Text = Folios.Consultar_Caja();
                Session["Dt_Folios"] = null;
                //Txt_Fecha.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Now.ToShortDateString());
                //Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.ParseExact(DateTime.Now.ToShortDateString(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            }
            else
            {
                if (Validar_Componentes_Generales())
                {
                    Cls_Ope_Pre_Folios_Inutilizados_Negocio Folios = new Cls_Ope_Pre_Folios_Inutilizados_Negocio();
                    Folios.P_No_Pago_ID = Txt_Pago_ID.Text.ToUpper().Trim();
                    Folios.P_No_Recibo = Txt_Recibo.Text.ToUpper().Trim();
                    Folios.P_Caja_ID = Txt_Caja_ID.Text.ToUpper().Trim();
                    Folios.P_Fecha=  string.Format("{0:dd/MMM/yyyy}", Txt_Fecha.Text.Trim());
                    Folios.P_Motivo = Cmb_Motivo.SelectedItem.Value;
                    Folios.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    Folios.P_Observaciones = Txt_Observaciones.Text.ToUpper().Trim();
                    Folios.P_Cajero = Txt_Cajero.Text.Trim().ToUpper();
                    Folios.P_No_Folio_Fin = Txt_Folio_Fin.Text.Trim();
                    Folios.P_No_Turno = Hfd_No_Turno.Value;
                    Folios.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                    if ((DataTable )Session["Dt_Folios"] != null ){
                        Folios.P_Dt_Folios = (DataTable)Session["Dt_Folios"];
                    }
                    Folios.Alta_Folio_Inutilizado();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Tabla_Folios_Inutilizados(Grid_Folios_Inutilizados.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Proceso de Folios Inutilizados", "alert('Folio Inutilizado Exitoso');", true);
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Imprimir.Visible = true;
                    Grid_Folios_Inutilizados.Enabled = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Llena la Tabla de Folios Inutilizados con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 25/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Llenar_Tabla_Folios_Inutilizados_Busqueda(0);
            Limpiar_Catalogo();
            if (Grid_Folios_Inutilizados.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron  todos los Folios almacenados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                Llenar_Tabla_Folios_Inutilizados(0);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 25/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip.Equals("Inicio"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Llenar_Tabla_Folios_Inutilizados(0);
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Folios_Inutilizados.Enabled = true;
                Btn_Imprimir.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


     protected void Btn_Imprimir_Click(object sender, EventArgs e)
     {
        Cls_Ope_Pre_Folios_Inutilizados_Negocio Recibos_Inutilizados = new Cls_Ope_Pre_Folios_Inutilizados_Negocio();
        DataTable Dt_Recibos = new DataTable();
        Ds_Ope_Pre_Folios_Inutilizados Ds_Recibos_Inutilizados = new Ds_Ope_Pre_Folios_Inutilizados();
        DataTable Dt_Recibo = new DataTable();
        DataRow Fila_Recibo;
        try
        {
            Recibos_Inutilizados.P_Fecha = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime( DateTime.Now.ToShortDateString()));
            
            Dt_Recibos = Recibos_Inutilizados.Consultar_Recibos();

            if (Dt_Recibos.Columns.Count > 0)
            {
                if (Dt_Recibos.Rows.Count > 0)
                {
                    Dt_Recibo.Columns.Add("MODULO");
                    Dt_Recibo.Columns.Add("NO_CAJA");
                    Dt_Recibo.Columns.Add("FECHA");
                    Dt_Recibo.Columns.Add("NUM_RECIBO");
                    Dt_Recibo.Columns.Add("MOTIVO");
                    Dt_Recibo.Columns.Add("OBSERVACIONES");
                    Dt_Recibo.Columns.Add("CAJERO");

                    foreach (DataRow Dr in Dt_Recibos.Rows)
                    {
                        Fila_Recibo = Dt_Recibo.NewRow();
                        Fila_Recibo["MODULO"] = Convert.ToString(Dr["MODULO"]);
                        Fila_Recibo["NO_CAJA"] = Convert.ToString(Convert.ToInt32(Dr["NO_CAJA"]));
                        Fila_Recibo["FECHA"] = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime( Dr["FECHA"].ToString()));
                        Fila_Recibo["NUM_RECIBO"] = Dr["NUM_RECIBO"].ToString();
                        Fila_Recibo["MOTIVO"] = Dr["MOTIVO"].ToString();
                        Fila_Recibo["OBSERVACIONES"] = Dr["OBSERVACIONES"].ToString();
                        Fila_Recibo["CAJERO"] = Dr["CAJERO"].ToString();
                        Dt_Recibo.Rows.Add(Fila_Recibo );
                    }
                    Dt_Recibo.TableName = "Dt_Recibos_Inutilizados";
                    Ds_Recibos_Inutilizados.Clear();
                    Ds_Recibos_Inutilizados.Tables.Clear();
                    Ds_Recibos_Inutilizados.Tables.Add(Dt_Recibo.Copy());
                    Imprimir_Reporte(Ds_Recibos_Inutilizados, "Rpt_Ope_Pre_Recibos_Inutilizados.rpt", "Recibos Inutilizados");
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "No hay recibos inutilizados el día de hoy";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else {
                Lbl_Ecabezado_Mensaje.Text = "No hay recibos inutilizados el día de hoy";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #region (Control Acceso Pagina)

    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Salir);
            Botones.Add(Btn_Buscar);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numero(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }

    #endregion

    #region Reportes

    private void Imprimir_Reporte(DataSet Ds_Recibos, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Cajas/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Recibos);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error " + Ex.Message);
        }

        try
        {
            Mostrar_Reporte(Archivo_PDF, "PDF");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    private void Mostrar_Reporte(String Nombre_Reporte, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt", "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
}
