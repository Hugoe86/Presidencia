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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Calendario_Nominas.Negocios;
using System.Globalization;
using AjaxControlToolkit;
using System.Text.RegularExpressions;
using Presidencia.DateDiff;
using System.Collections.Generic;

public partial class paginas_Nomina_Frm_Cat_Nom_Calendario_Nominas : System.Web.UI.Page
{

    #region (Load)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Carga inicial de la página
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Configuracion_Inicial();
                ViewState["SortDirection"] = "ASC";
            }
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            TPnl_Catorcenas.Visible = true;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion

    #region (Grid)

    #region (Grid Calendario Nomina)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calendario_Nominas_PageIndexChanging
    ///DESCRIPCIÓN:Cambiar de Pagina de la tabla de Calendario de Nominas
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Calendario_Nominas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Calendario_Nominas.PageIndex = e.NewPageIndex;
            Consultar_Calendario_Nominas();
            Grid_Calendario_Nominas.SelectedIndex = -1;
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Inicializar Eventos JQuery", "Inicializar_Eventos_Calendario_Nomina()", true); 
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cambiar de pagina " + Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calendario_Nominas_SelectedIndexChanged
    ///DESCRIPCIÓN: seleccionar algun elemento de la tabla de Calendario de Nominas
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Calendario_Nominas_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Grid_Calendario_Nominas.SelectedIndex;
        try
        {
            Txt_Nomina_ID.Text = HttpUtility.HtmlDecode(Grid_Calendario_Nominas.Rows[index].Cells[1].Text);
            Txt_Fecha_Inicio.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(HttpUtility.HtmlDecode(Grid_Calendario_Nominas.Rows[index].Cells[2].Text)));
            Txt_Fecha_Fin.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(HttpUtility.HtmlDecode(Grid_Calendario_Nominas.Rows[index].Cells[3].Text)));
            Txt_Anio_Calendario_Nomina.Text = HttpUtility.HtmlDecode(Grid_Calendario_Nominas.Rows[index].Cells[4].Text); ;

            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Inicializar Eventos JQuery", "Inicializar_Eventos_Calendario_Nomina()", true); 
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un elemento de la tabla " + Ex.Message);
        }
    }
    #endregion

    #region (Grid Catorcenas)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Catorcenas_RowDataBound
    ///DESCRIPCIÓN: Antes de Renderizar el Grid de Catorcenas
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 22/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Catorcenas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        String onmouseoverStyle = "this.style.backgroundColor='#DFE8F6';this.style.cursor='hand';this.style.color='DarkBlue';" +
            "this.style.borderStyle='none';this.style.borderColor='Silver';";

        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                e.Row.Cells[4].Font.Bold = true;
                e.Row.Cells[4].Attributes.Add("onmouseover", onmouseoverStyle);
                if (e.Row.RowIndex % 2 == 0)
                {
                    e.Row.Cells[4].Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF';this.style.color='Black';this.style.borderStyle='none';");
                }
                else
                {
                    e.Row.Cells[4].Attributes.Add("onmouseout", "this.style.backgroundColor='#E6E6E6';this.style.color='Black';this.style.borderStyle='none';");
                }

                e.Row.Cells[4].Attributes.Add("onclick", @"alert('No Nomina: " + e.Row.Cells[1].Text + @"\nFecha de Pago: " + e.Row.Cells[4].Text + @"\nDias Periodo Nominal: " + e.Row.Cells[3].Text + "');");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error en el Evento de Grid_Catorcenas_RowDataBound. Error: [" + Ex.Message + "]");
        }//End Catch
    }
    #endregion

    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Configuracion_Inicial
    /// DESCRIPCION : Habilita la configuracion inicial de la página.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Consultar_Calendario_Nominas();
        Limpiar_Controles();
        Habilitar_Controles("Inicial");
        LLenar_Cmb_Dias_Periodo_Nominal();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        Txt_Nomina_ID.Text = "";
        Txt_Fecha_Inicio.Text = "";
        Txt_Fecha_Inicio_CalendarExtender.SelectedDate = null;
        Txt_Fecha_Fin.Text = "";
        Grid_Calendario_Nominas.SelectedIndex = -1;
        Txt_Anio_Calendario_Nomina.Text = "";
        //if (Session["Calendario_Nominas"] != null) Session.Remove("Calendario_Nominas");
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado = false;

        try
        {
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Txt_Busqueda_Calendario_Nomina.Enabled = true;
                    Btn_Buscar_Calendario_Nomina.Enabled = true;
                    Btn_Fecha_Inicio.Enabled = false;
                    Btn_Fecha_Fin.Enabled = false;
                    Txt_Fecha_Inicio.Enabled = false;
                    Txt_Fecha_Fin.Enabled = false;
                    Limpiar_GridView(Grid_Catorcenas);
                    Contenedor.ActiveTabIndex = 0;
                    Btn_Ver_Catorcenas.Enabled = true;
                    Btn_Ver_Catorcenas.ToolTip = "Catorcena";
                    Btn_Ver_Catorcenas.Text = "Ver Fechas de Catorcenas";
                    TPnl_Catorcenas.Visible = false;
                    Tab_Nominas.Visible = true;

                    Configuracion_Acceso("Frm_Cat_Nom_Calendario_Nominas.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Txt_Busqueda_Calendario_Nomina.Enabled = false;
                    Btn_Buscar_Calendario_Nomina.Enabled = false;
                    Btn_Fecha_Inicio.Enabled = false;
                    Btn_Fecha_Fin.Enabled = true;
                    Txt_Fecha_Inicio.Enabled = false;
                    Txt_Fecha_Fin.Enabled = true;
                    Limpiar_GridView(Grid_Catorcenas);
                    Contenedor.ActiveTabIndex = 1;
                    Btn_Ver_Catorcenas.Enabled = false;
                    Btn_Ver_Catorcenas.ToolTip = "Catorcena";
                    Btn_Ver_Catorcenas.Text = "Ver Fechas de Catorcenas";
                    TPnl_Catorcenas.Visible = true;
                    Tab_Nominas.Visible = false;
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Txt_Busqueda_Calendario_Nomina.Enabled = false;
                    Btn_Buscar_Calendario_Nomina.Enabled = false;
                    Btn_Fecha_Inicio.Enabled = false;
                    Btn_Fecha_Fin.Enabled = false;
                    Txt_Fecha_Inicio.Enabled = false;
                    Txt_Fecha_Fin.Enabled = false;
                    Contenedor.ActiveTabIndex = 1;
                    Btn_Ver_Catorcenas.Enabled = false;
                    Btn_Ver_Catorcenas.ToolTip = "Catorcena";
                    Btn_Ver_Catorcenas.Text = "Ver Fechas de Catorcenas";
                    TPnl_Catorcenas.Visible = true;
                    Tab_Nominas.Visible = false;
                    Consultar_Catorcenas();
                    //Consulta_Detalles_Nomina(Txt_Nomina_ID.Text);
                    Consulta_Periodos_Nominales(Txt_Nomina_ID.Text);
                    break;
            }
            Txt_Nomina_ID.Enabled = false;
            Grid_Calendario_Nominas.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Grid_Catorcenas.Enabled = Habilitado;

            Cmb_Dias_Periodo_Nominal.Enabled = Habilitado;
            Txt_Numero_Periodos_Generar.Enabled = Habilitado;
            Btn_Agregar_Periodos.Enabled = Habilitado;
            Btn_Limpiar_Periodos.Enabled = Habilitado;
            Txt_Anio_Calendario_Nomina.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    /// ********************************************************************************
    /// Nombre: Limpiar_GridView
    /// Descripción: Limpia el grid que se pasa como parametro
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private void Limpiar_GridView(GridView Tabla)
    {
        Tabla.DataSource = new DataTable();
        Tabla.DataBind();
    }
    #endregion

    #region (Metodos Validacion)
    /// ********************************************************************************
    /// Nombre: Busqueda_Calendario_Nomina
    /// Descripción: Valida que la Fecha Inicial no sea mayor que la Final, y que el año
    ///              de la nomina a generar sea mayor al año actual.
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private Boolean Validar_Fecha_Inicio_Fin()
    {
        //Obtenemos las fecha de inicio y fin que vamos a evaluar.
        DateTime Fecha_Inicio = Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim());
        DateTime Fecha_Fin = Convert.ToDateTime(Txt_Fecha_Fin.Text.Trim());
        Boolean Fecha_Valida = false;
        //Valida que la fecha final no sea menor que la inicial.
        if (Fecha_Inicio < Fecha_Fin)
        {
            //Valida que la nomina a generar no se a del año actual.
            //if (Fecha_Inicio.AddDays(14).Year > DateTime.Now.Year)
            //{
                Fecha_Valida = true;
            //}
        }
        return Fecha_Valida;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Formato_Fecha
    /// DESCRIPCION : Valida el formato de las fechas.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Formato_Fecha(String Fecha)
    {
        String Cadena_Fecha = @"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$";
        if (Fecha != null) return Regex.IsMatch(Fecha, Cadena_Fecha);
        else return false;
    }
    /// ********************************************************************************
    /// Nombre: Validar_Anios
    /// Descripción: valida que no se den de alta calendarios de nomina repetidos.
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private Boolean Validar_Nomina_Anio_Repetido(GridView Tabla_Calendario_Nominas)
    {
        Boolean Anio_Repetido = false;//Almacenará el valor si es que la nomina a generar ya se encuentra en el sistema.

        for (int indice = 0; indice < Tabla_Calendario_Nominas.Rows.Count; indice++)
        {
            //Obtenemos la fecha del calendario de nomina.
            DateTime Fecha_Registrada = Convert.ToDateTime(Tabla_Calendario_Nominas.Rows[indice].Cells[2].Text.Trim());
            //Validamos que el calendario de nomina no se encuentre ya registrado en el sistema.
            if (Fecha_Registrada.Year == Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim()).AddDays(14).Year)
            {
                Anio_Repetido = true;
            }
        }
        return Anio_Repetido;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 22/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Generar_Calendario_Nomina()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_Fecha_Inicio.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Inicial <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de Fecha Inicial Incorrecto <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Fin.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Final <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Fin.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de Fecha Final Incorrecto <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Anio_Calendario_Nomina.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Año de la Nómina <br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }
    #endregion

    #region (Metodos Generar Catorcenas)
    /// ********************************************************************************
    /// Nombre: Genera_Periodos_Nominales_Pago_Operacion
    /// Descripción: Algoritmo para el calculo de las fechas de pago de la nomina
    /// por catorcena.
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private DataTable Genera_Periodos_Nominales_Pago_Operacion()
    {
        Int32 No_Catorcena = 1;
        DateTime Fecha_Pago_Catorcenal;
        DataTable DT_Catorcenas = new DataTable();
        try
        {
            if (Validar_Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()) && Validar_Formato_Fecha(Txt_Fecha_Fin.Text.Trim()))
            {
                DateTime Fecha_Inicio = Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim());
                DateTime Fecha_Fin = Convert.ToDateTime(Txt_Fecha_Fin.Text.Trim());

                DT_Catorcenas.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_No_Nomina, Type.GetType("System.Int32"));
                DT_Catorcenas.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio, Type.GetType("System.DateTime"));
                DT_Catorcenas.Columns.Add("Separador", Type.GetType("System.String"));
                DT_Catorcenas.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin, Type.GetType("System.DateTime"));

                Fecha_Pago_Catorcenal = Fecha_Inicio;
                while (Fecha_Pago_Catorcenal < Fecha_Fin)
                {
                    DataRow Renglon = DT_Catorcenas.NewRow();
                    Renglon[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio] = Fecha_Pago_Catorcenal.AddDays(1);

                    Fecha_Pago_Catorcenal = Fecha_Pago_Catorcenal.AddDays(14);
                    if (Fecha_Pago_Catorcenal > Fecha_Fin)
                    {
                        if (No_Catorcena <= 26)
                        {
                            DT_Catorcenas = null;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                                "alert('El numero de catorcenas generado es incorrecto verifique las fechas seleccionadas');", true);
                            return DT_Catorcenas;
                        }
                        else
                        {
                            return DT_Catorcenas;
                        }
                    }
                    Renglon[Cat_Nom_Nominas_Detalles.Campo_No_Nomina] = No_Catorcena;
                    Renglon["Separador"] = "Al";
                    Renglon[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin] = Fecha_Pago_Catorcenal;

                    DT_Catorcenas.Rows.Add(Renglon);
                    No_Catorcena += 1;
                    if (No_Catorcena > 26)
                    {
                        break;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar las catorcenas de pago. Error: [" + Ex.Message + "]");
        }        
        return DT_Catorcenas;
    }
    /// ********************************************************************************
    /// Nombre: Genera_Periodos_Nominales_Pago_Consulta
    /// Descripción: Algoritmo para el calculo de las fechas de pago de la nomina
    /// por catorcena.
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private DataTable Genera_Periodos_Nominales_Pago_Consulta()
    {
        Int32 No_Catorcena = 1;
        DateTime Fecha_Pago_Catorcenal;
        DataTable DT_Catorcenas = new DataTable();

        if (Validar_Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()) && Validar_Formato_Fecha(Txt_Fecha_Fin.Text.Trim()))
        {
            DateTime Fecha_Inicio = Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim()).AddDays(-1);
            DateTime Fecha_Fin = Convert.ToDateTime(Txt_Fecha_Fin.Text.Trim());

            DT_Catorcenas.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_No_Nomina, Type.GetType("System.Int32"));
            DT_Catorcenas.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio, Type.GetType("System.DateTime"));
            DT_Catorcenas.Columns.Add("Separador", Type.GetType("System.String"));
            DT_Catorcenas.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin, Type.GetType("System.DateTime"));

            Fecha_Pago_Catorcenal = Fecha_Inicio;
            while (Fecha_Pago_Catorcenal < Fecha_Fin)
            {
                DataRow row = DT_Catorcenas.NewRow();
                row[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio] = Fecha_Pago_Catorcenal.AddDays(1);

                Fecha_Pago_Catorcenal = Fecha_Pago_Catorcenal.AddDays(14);
                if (Fecha_Pago_Catorcenal > Fecha_Fin)
                {
                    return DT_Catorcenas;
                }
                row[Cat_Nom_Nominas_Detalles.Campo_No_Nomina] = No_Catorcena;
                row["Separador"] = "Al";
                row[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin] = Fecha_Pago_Catorcenal;

                DT_Catorcenas.Rows.Add(row);
                No_Catorcena += 1;
            }
        }
        return DT_Catorcenas;
    }
    /// ********************************************************************************
    /// Nombre: Estatus_Catorcenas
    /// Descripción: Consulta el DataTable actual de catorcenas y Cambia el estatus segun
    /// el estado en el que se encuentra el Checkbox.
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 22/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private DataTable Estatus_Catorcenas()
    {
        DataTable DT_Catorcenas = new DataTable();
        DataRow Renglon = null; //Renglon para el llenado de la tabla
        Int32 Cont_Elementos = 0; //Variable para el contador

        try
        {
            DT_Catorcenas.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_No_Nomina, Type.GetType("System.Int32"));
            DT_Catorcenas.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio, Type.GetType("System.DateTime"));
            DT_Catorcenas.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin, Type.GetType("System.DateTime"));
            DT_Catorcenas.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Estatus, Type.GetType("System.String"));


            //Ciclo para el llenado de la tabla
            for (Cont_Elementos = 0; Cont_Elementos < Grid_Catorcenas.Rows.Count; Cont_Elementos++)
            {
                //Variable para el checkbox
                CheckBox _Chk_Estatus_Catorcena = (CheckBox)Grid_Catorcenas.Rows[Cont_Elementos].FindControl("Chk_Estatus_Catorcena");

                //Verificar si esta chechado el check
                if (_Chk_Estatus_Catorcena.Checked == true)
                {
                    //Instanciar y llenar renglon
                    Renglon = DT_Catorcenas.NewRow();
                    Renglon[Cat_Nom_Nominas_Detalles.Campo_No_Nomina] = Grid_Catorcenas.Rows[Cont_Elementos].Cells[1].Text; ;
                    Renglon[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio] = Grid_Catorcenas.Rows[Cont_Elementos].Cells[2].Text;
                    Renglon[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin] = Grid_Catorcenas.Rows[Cont_Elementos].Cells[4].Text;
                    Renglon[Cat_Nom_Nominas_Detalles.Campo_Estatus] = "ACTIVO";
                    //Colocar renglon en la tabla
                    DT_Catorcenas.Rows.Add(Renglon);
                }
                else
                {
                    Renglon = DT_Catorcenas.NewRow();
                    Renglon[Cat_Nom_Nominas_Detalles.Campo_No_Nomina] = Grid_Catorcenas.Rows[Cont_Elementos].Cells[1].Text; ;
                    Renglon[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio] = Grid_Catorcenas.Rows[Cont_Elementos].Cells[2].Text;
                    Renglon[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin] = Grid_Catorcenas.Rows[Cont_Elementos].Cells[4].Text;
                    Renglon[Cat_Nom_Nominas_Detalles.Campo_Estatus] = "INACTIVO";
                    DT_Catorcenas.Rows.Add(Renglon);
                }
            }//End For
            //Entregar resultados
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }//End Catch   
        return DT_Catorcenas;
    }
    #endregion

    #region (Metodos Consulta Calendario Nominas Y Catorcenas)
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Consulta_Detalles_Nomina
    /// DESCRIPCIÓN: Se hace un barrido de la consulta de los Detalles de la Nomina seleccionada,
    /// y se le asigna el valor al checkbox de la tabla, dependiendo del valor del Estatus
    /// de la consulta.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 22/Octubre/2010
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    private void Consulta_Detalles_Nomina(String Nomina_ID)
    {
        int Count_Fila = 0;
        CheckBox _Chk_Estatus_Catorcena;
        DataTable Dt_Catorcenas;
        Cls_Cat_Nom_Calendario_Nominas_Negocio Cat_Nom_Calendario_Nominas_Negocio;

        try
        {
            Cat_Nom_Calendario_Nominas_Negocio = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
            Cat_Nom_Calendario_Nominas_Negocio.P_Nomina_ID = Nomina_ID.Trim();
            Dt_Catorcenas = Cat_Nom_Calendario_Nominas_Negocio.Consulta_Detalles_Nomina();

            if (Dt_Catorcenas.Rows.Count > 0)
            {
                foreach (DataRow Renglon in Dt_Catorcenas.Rows)
                {
                    _Chk_Estatus_Catorcena = (CheckBox)Grid_Catorcenas.Rows[Count_Fila].FindControl("Chk_Estatus_Catorcena");
                    if (Renglon[Cat_Nom_Nominas_Detalles.Campo_Estatus].ToString().Trim().Equals("INACTIVO"))
                    {
                        _Chk_Estatus_Catorcena.Checked = false;
                    }
                    else
                    {
                        _Chk_Estatus_Catorcena.Checked = true;
                    }
                    Count_Fila = Count_Fila + 1;
                }
            }//End If
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.Trim());
        }//End Catch
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Calendario_Nominas
    ///DESCRIPCIÓN: Consulta los Calendarios de la Nomina
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Catorcenas()
    {
        DataTable Dt_Calendario_Nominas = null;
        try
        {
            Grid_Catorcenas.Columns[0].Visible = true;
            Grid_Catorcenas.Columns[2].Visible = true;
            Grid_Catorcenas.Columns[3].Visible = true;

            if (Btn_Nuevo.ToolTip.Equals("Dar de Alta"))
            {
                Dt_Calendario_Nominas = Genera_Periodos_Nominales_Pago_Operacion();
            }
            else if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Dt_Calendario_Nominas = Genera_Periodos_Nominales_Pago_Consulta();
            }
            Grid_Catorcenas.DataSource = Dt_Calendario_Nominas;
            Grid_Catorcenas.DataBind();
            Grid_Catorcenas.Columns[0].Visible = true;
            Grid_Catorcenas.Columns[2].Visible = true;
            Grid_Catorcenas.Columns[3].Visible = true;
        }
        catch (Exception e)
        {
            throw new Exception("Error al Consultar Catorcenas" + e.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Calendario_Nominas
    ///DESCRIPCIÓN: Consulta los Calendarios de la Nomina
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Calendario_Nominas()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Cat_Calendarios_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
        DataTable Dt_Calendario_Nominas;
        try
        {
            Dt_Calendario_Nominas = Cat_Calendarios_Nomina.Consultar_Calendario_Nominas();
            Session["Calendario_Nominas"] = Dt_Calendario_Nominas;
            Grid_Calendario_Nominas.Columns[4].Visible = true;
            Grid_Calendario_Nominas.DataSource = (DataTable)Session["Calendario_Nominas"];
            Grid_Calendario_Nominas.DataBind();
            Grid_Calendario_Nominas.Columns[4].Visible = false;

            if (Grid_Calendario_Nominas.Rows.Count > 0) {
                Grid_Calendario_Nominas.UseAccessibleHeader = true;
                Grid_Calendario_Nominas.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        catch (Exception e)
        {
            throw new Exception("Error al Consultar Calendario de Nominas" + e.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Ultima_Nomina_Generada
    ///DESCRIPCIÓN: Obtiene el último registro de calendario de nomina 
    ///             en el sistema.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 24/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Consultar_Ultima_Nomina_Generada()
    {
        Int64 Nomina_ID = 0;//Almacena el numero de nomina consultada.
        DateTime? Fecha_Final_Ultima_Nomina_Generada = null;//Fecha final de la ultima nomina generada. 
        Int64 Ultima_Nomina = 0;//Va almacenando la ultima nomina encontrada.
        try
        {
            foreach (GridViewRow Renglon in Grid_Calendario_Nominas.Rows)
            {
                Nomina_ID = Convert.ToInt64(Renglon.Cells[1].Text);
                if (Nomina_ID > Ultima_Nomina)
                {
                    Ultima_Nomina = Nomina_ID;
                    Fecha_Final_Ultima_Nomina_Generada = Convert.ToDateTime(Renglon.Cells[3].Text);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar la ultima nomina generada. Error: [" + Ex.Message + "]");
        }
        return string.Format("{0:dd/MMM/yyyy}", (Fecha_Final_Ultima_Nomina_Generada != null)?Fecha_Final_Ultima_Nomina_Generada: null );
    }
    #endregion

    #region ( Metodos Operación [Alta - Modificar - Eliminar - Busqueda] )
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta Fechas Nomina
    /// DESCRIPCION : Ejecuta el Alta Nomina
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************    
    private void Alta_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Cat_Calendarios_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
        try
        {
            Cat_Calendarios_Nomina.P_Anio = Convert.ToInt32(Txt_Anio_Calendario_Nomina.Text.Trim());
            Cat_Calendarios_Nomina.P_Fecha_Inicio = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(HttpUtility.HtmlDecode(Txt_Fecha_Inicio.Text.Trim())).AddDays(1));
            Cat_Calendarios_Nomina.P_Fecha_Fin = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(HttpUtility.HtmlDecode(Txt_Fecha_Fin.Text.Trim())));
            Cat_Calendarios_Nomina.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            Cat_Calendarios_Nomina.P_Dt_Periodos_Pago = Estatus_Catorcenas();
            if (Session["Calendario_Nominas"] != null) Session.Remove("Calendario_Nominas");
            if (Session["Periodos_Nominales"] != null) Session.Remove("Periodos_Nominales");
            //Validar que existan catorcenas de pago
            if (Cat_Calendarios_Nomina.P_Dt_Periodos_Pago.Rows.Count > 0)
            {
                if (Validar_Datos_Generar_Calendario_Nomina())
                {
                    if (!Validar_Nomina_Anio_Repetido(Grid_Calendario_Nominas))
                    {
                        if (Validar_Fecha_Inicio_Fin())
                        {
                            if (Cat_Calendarios_Nomina.Alta_Calendarios_Nomina())
                            {
                                Configuracion_Inicial();
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Alta Calendario Nomina]');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", @"alert('La Fecha Final no puede ser menor o igual que la Inicial o\nno puede existir un alta calendario de nomina de un año menor al actual');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('El Calendario de Nomina para el año seleccionada ya existe');", true);
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Error al Generar las catorcenas de pago');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta " + Ex.Message.ToString(), Ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Actualizar_Calendario_Nomina
    /// DESCRIPCION : Ejecuta la Actualizacion del Calendario de Nomina
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 20/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************    
    private void Actualizar_Calendario_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Cat_Calendarios_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
        try
        {
            Cat_Calendarios_Nomina.P_Nomina_ID = HttpUtility.HtmlDecode(Txt_Nomina_ID.Text);
            Cat_Calendarios_Nomina.P_Anio = Convert.ToInt32(Txt_Anio_Calendario_Nomina.Text.Trim());
            Cat_Calendarios_Nomina.P_Fecha_Inicio = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(HttpUtility.HtmlDecode(Txt_Fecha_Inicio.Text)));
            Cat_Calendarios_Nomina.P_Fecha_Fin = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(HttpUtility.HtmlDecode(Txt_Fecha_Fin.Text)));
            Cat_Calendarios_Nomina.P_Usuario_Modico = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            Cat_Calendarios_Nomina.P_Dt_Periodos_Pago = Estatus_Catorcenas();
            if (Session["Calendario_Nominas"] != null) Session.Remove("Calendario_Nominas");
            if (Session["Periodos_Nominales"] != null) Session.Remove("Periodos_Nominales");
            if (Validar_Datos_Generar_Calendario_Nomina())
            {
                if (Validar_Fecha_Inicio_Fin())
                {
                    if (Cat_Calendarios_Nomina.Actualizar_Calendario_Nomina())
                    {
                        Configuracion_Inicial();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Actualizar Calendario Nomina]');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", @"alert('La Fecha Final no puede ser menor o igual que la Inicial o\nno puede existir un alta calendario de nomina de un año menor al actual');", true);
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Actualizar " + Ex.Message.ToString(), Ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Calendario_Nomina
    /// DESCRIPCION : Ejecuta la baja registro seleccionado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 20/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************    
    private void Eliminar_Calendario_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Cat_Calendarios_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexion con la capa de negocios.
        try
        {
            Cat_Calendarios_Nomina.P_Nomina_ID = HttpUtility.HtmlDecode(Txt_Nomina_ID.Text);//Obtenemos el ID del calendario de la nomina.

            if (Cat_Calendarios_Nomina.Eliminar_Calendario_Nomina())
            {
                if (Session["Calendario_Nominas"] != null) Session.Remove("Calendario_Nominas");
                if (Session["Periodos_Nominales"] != null) Session.Remove("Periodos_Nominales");
                Configuracion_Inicial();//Volvemos a la configuracion inicial.
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Eliminar Calendario Nomina]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Elimnar " + Ex.Message.ToString(), Ex);
        }
    }
    /// ********************************************************************************
    /// Nombre: Busqueda_Calendario_Nomina
    /// Descripción: Ejecuta la busqueda del Calendario por su No de Nomina
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private void Busqueda_Calendario_Nomina(String Nomina_ID, GridView Tabla_Calendario_Nominas)
    {
        if (!Nomina_ID.Equals(""))
        {
            DataTable Dt_Calendario_Nominas = (DataTable)Session["Calendario_Nominas"];
            DataView DV_Vista_Calendario_Nomina = new DataView(Dt_Calendario_Nominas);
            String Expresion_De_Busqueda = null;

            Expresion_De_Busqueda = string.Format("{0} '{1}'", Tabla_Calendario_Nominas.SortExpression, Nomina_ID);

            DV_Vista_Calendario_Nomina.RowFilter = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + " = " + Expresion_De_Busqueda;
            Tabla_Calendario_Nominas.DataSource = DV_Vista_Calendario_Nomina;
            Tabla_Calendario_Nominas.DataBind();
        }
        else
        {
            Consultar_Calendario_Nominas();//Consulta de los calendarios de nominas que se encuentran en el sistema.
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
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Eliminar);
            Botones.Add(Btn_Buscar_Calendario_Nomina);

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


    #endregion

    #region (Eventos)

    #region (Eventos Operación [Alta - Modificar - Eliminar - Consultar])
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Ejecuta el alta de un calendario de nomina seleccionado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpiar_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                Txt_Fecha_Inicio.Text = Consultar_Ultima_Nomina_Generada();
                if (!Validar_Formato_Fecha(Txt_Fecha_Inicio.Text))
                {
                    Txt_Fecha_Inicio.Enabled = true;
                    Btn_Fecha_Inicio.Enabled = true;
                    Txt_Fecha_Inicio_CalendarExtender.SelectedDate = null;
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (Validar_Datos_Generar_Calendario_Nomina())
                {
                    //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                    if (Validar_Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()) && Validar_Formato_Fecha(Txt_Fecha_Fin.Text.Trim()))
                    {
                        if (Validar_Suma_Total_Dias_No_Exeda_Anyo_Nominal())
                        {
                            Alta_Calendarios_Nomina(); //Da de alta los datos proporcionados por el usuario
                        }
                    }
                    //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

                        if (!Validar_Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()))
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Inicio <br>";
                        }
                        if (!Validar_Formato_Fecha(Txt_Fecha_Fin.Text.Trim()))
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Fin <br>";
                        }
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Inicializar Eventos JQuery", "Inicializar_Eventos_Calendario_Nomina()", true); 
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }  
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Modifica el calendario de nomina seleccionado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_Nomina_ID.Text != "")
                {
                    if (Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim()).Year > DateTime.Now.Year)
                    {
                        Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "No es posible modificar una nomina anterior o actual.";
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea modificar sus datos <br>";
                }
            }
            else
            {
                if (Validar_Datos_Generar_Calendario_Nomina())
                {
                    //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos valores en la base de datos
                    if (Validar_Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()) && Validar_Formato_Fecha(Txt_Fecha_Fin.Text.Trim()))
                    {
                        if (Validar_Suma_Total_Dias_No_Exeda_Anyo_Nominal())
                        {
                            Actualizar_Calendario_Nomina(); //Da de alta los datos proporcionados por el usuario
                        }
                    }
                    //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                        if (!Validar_Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()))
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Inicio <br>";
                        }
                        if (!Validar_Formato_Fecha(Txt_Fecha_Fin.Text.Trim()))
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Fin <br>";
                        }
                    }
                }
                else {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Inicializar Eventos JQuery", "Inicializar_Eventos_Calendario_Nomina()", true); 
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Elimina el calendario de nomikna seleccionado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Txt_Nomina_ID.Text != "")
            {
                Eliminar_Calendario_Nomina(); 
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el registro que desea eliminar <br>";
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Inicializar Eventos JQuery", "Inicializar_Eventos_Calendario_Nomina()", true); 
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir del formulario o cancelar la operación actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Session.Remove("Calendario_Nominas");
                Session.Remove("Periodos_Nominales"); 
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
                Session.Remove("Periodos_Nominales"); 
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Inicializar Eventos JQuery", "Inicializar_Eventos_Calendario_Nomina()", true); 
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Calendario_Nomina_Click
    ///DESCRIPCIÓN: Busca los calendarios de nomina que se encuentran dados de alta 
    ///             en el sistema.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Calendario_Nomina_Click(object sender, ImageClickEventArgs e)
    {
        Busqueda_Calendario_Nomina(Txt_Busqueda_Calendario_Nomina.Text.Trim(), Grid_Calendario_Nominas);
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Inicializar Eventos JQuery", "Inicializar_Eventos_Calendario_Nomina()", true); 
    }
    #endregion

    #region (Eventos Generacion Catorcenas)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Carga inicial de la página
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Ver_Catorcenas_Click(object sender, EventArgs e)
    {
        if (Grid_Calendario_Nominas.SelectedIndex != -1)
        {
            if (Btn_Ver_Catorcenas.ToolTip == "Catorcena")
            {
                //Consultar_Catorcenas();
                //Consulta_Detalles_Nomina(Txt_Nomina_ID.Text);
                Consulta_Periodos_Nominales(Txt_Nomina_ID.Text.Trim());

                Contenedor.ActiveTabIndex = 1;
                Btn_Ver_Catorcenas.ToolTip = "Nominas";
                Btn_Ver_Catorcenas.Text = "Calendario de Nominas";

                TPnl_Catorcenas.Visible = true;
                Tab_Nominas.Visible = false;

                Btn_Buscar_Calendario_Nomina.Enabled = false;
                Txt_Busqueda_Calendario_Nomina.Enabled = false;
            }
            else if (Btn_Ver_Catorcenas.ToolTip == "Nominas")
            {
                Contenedor.ActiveTabIndex = 0;
                Btn_Ver_Catorcenas.ToolTip = "Catorcena";
                Btn_Ver_Catorcenas.Text = "Ver Fechas de Catorcenas";

                TPnl_Catorcenas.Visible = false;
                Tab_Nominas.Visible = true;

                Btn_Buscar_Calendario_Nomina.Enabled = true;
                Txt_Busqueda_Calendario_Nomina.Enabled = true;
                if (Session["Calendario_Nominas"] != null) Session.Remove("Calendario_Nominas");

                if (Session["Periodos_Nominales"] != null) Session.Remove("Periodos_Nominales");
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Inicializar Eventos JQuery", "Inicializar_Eventos_Calendario_Nomina()", true); 
        }
        else {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Seleccione el registro de la tabla');", true);   
        }
    }
    #endregion

    #endregion



    /// **************************************************************************************************************************************
    /// NOMBRE: Btn_Limpiar_Periodos_Click
    /// 
    /// DESCRIPCIÓN: Limpia la tabla de periodos nominales.
    ///              
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 11:06 am.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Btn_Limpiar_Periodos_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Remove("Periodos_Nominales");
            Grid_Catorcenas.DataSource = new DataTable();
            Grid_Catorcenas.DataBind();
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Inicializar Eventos JQuery", "Inicializar_Eventos_Calendario_Nomina()", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Btn_Agregar_Periodos_Click
    /// 
    /// DESCRIPCIÓN: Agrega los periodos nominales a la tabla de periodos en base a los parámetros de cantidad de dias para el periodo
    ///              y el número de periodos que se generaran con esa estructura.
    ///              
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 11:06 am.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Btn_Agregar_Periodos_Click(object sender, EventArgs e)
    {
        String Mensaje = "";

        try
        {
            if (Cmb_Dias_Periodo_Nominal.SelectedIndex > 0 && !string.IsNullOrEmpty(Txt_Numero_Periodos_Generar.Text.Trim()))
            {
                Crear_Periodos_Nominales();         //Generamos los periodos nominales.
                LLenar_Grid_Periodos_Nominales();   //Llenamos la tabla de periodos nominales.
                Habilitar_Opcion_Estatus_Grid();    //Habilita ó Deshabilita la opción de estatus de la tabla de periodos nominales.

                Cmb_Dias_Periodo_Nominal.SelectedIndex = -1;
                Txt_Numero_Periodos_Generar.Text = "";
            }
            else {
                Mensaje += "Es necesario ingresar: <br />";

                if (Cmb_Dias_Periodo_Nominal.SelectedIndex <= 0)
                    Mensaje += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ El número de días del periodo nominal a generar es requerido. <br />";
                if(string.IsNullOrEmpty(Txt_Numero_Periodos_Generar.Text.Trim()))
                    Mensaje += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ El número de periodos nominales a generar es requerido. <br />";

                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Mensaje;
            }

            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Inicializar Eventos JQuery", "Inicializar_Eventos_Calendario_Nomina()", true); 
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Crear_Periodos_Nominales
    /// 
    /// DESCRIPCIÓN: Crea la tabla de periodos nominales que se usará para cargar el GridView de periodos nominales.
    ///              [No_Nomina, Fecha_Inicia_Periodo, Separador, Fecha_Fin_Periodo].
    ///              
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 11:08 am.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    private void Crear_Periodos_Nominales()
    {
        Int32 Numero_Periodos_Nominales_Generar = 0;    //Numero de periodos a generar con la descripción ingresada.
        Int32 Dias_Periodo_Nominal = 0;                 //Dias que se consideran para la generación de los periodos nominales.
        DataTable Dt_Periodos_Nominales = null;         //Variable que almacenara los periodos nominales.
        DataRow Dr_Periodo_Nominal = null;              //Registro de periodo nominal a agregar al calendario de nomina.
        DateTime? Fecha_Inicio_Anyo_Nominal = null;     //Fecha de inicio del periodo nominal        

        try
        {
            Fecha_Inicio_Anyo_Nominal = Convert.ToDateTime(HttpUtility.HtmlDecode(Txt_Fecha_Inicio.Text.Trim()));//Obtenemos la fecha de inicio del calendario de nómina.
            Numero_Periodos_Nominales_Generar = Convert.ToInt32(Txt_Numero_Periodos_Generar.Text.Trim());//Obtenemos el número de periodos a generar con los parámetros indicados.
            Dias_Periodo_Nominal = Convert.ToInt32(Cmb_Dias_Periodo_Nominal.SelectedValue.Trim());//Obtenemos los dias a considerar como intervalo del periodo.

            //Validamos si el la tabla de periodos nominales esta vacia o ya contiene registros.
            if (Session["Periodos_Nominales"] != null)
            {
                Dt_Periodos_Nominales = (DataTable)Session["Periodos_Nominales"];
                if (Dt_Periodos_Nominales.Rows.Count > 0) {
                    Fecha_Inicio_Anyo_Nominal = Convert.ToDateTime(Dt_Periodos_Nominales.Rows[Dt_Periodos_Nominales.Rows.Count - 1][3].ToString().Trim());
                }
            }
            else {
                //Si la tabla de periodos nominales es NULL, se procede a crear la estructura de la misma.
                Dt_Periodos_Nominales = new DataTable("PERIODOS_NOMINALES");
                Dt_Periodos_Nominales.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_No_Nomina, Type.GetType("System.Int32"));//Indica el número de periodo generado.
                Dt_Periodos_Nominales.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio, Type.GetType("System.DateTime"));//Indica la fecha de inicio del periodo nominal.
                Dt_Periodos_Nominales.Columns.Add("Separador", Type.GetType("System.String"));//Separador utilizado para separar la fechas en el grid.
                Dt_Periodos_Nominales.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin, Type.GetType("System.DateTime"));//Indica la fecha de final del periodo nominal.                
            }

            //Obtenemos el siguiente periodo que deberá ser generado.
            Numero_Periodos_Nominales_Generar = Numero_Periodos_Nominales_Generar + Dt_Periodos_Nominales.Rows.Count;

            //Creamos los periodos nominales con los parámetros indicados [Número de días y Número de Periodos Nominales]. 
            for (Int32 Contador_Periodos_Generados = (Dt_Periodos_Nominales.Rows.Count == 0) ? 1 : Dt_Periodos_Nominales.Rows.Count + 1; Contador_Periodos_Generados <= Numero_Periodos_Nominales_Generar; Contador_Periodos_Generados++)
            {
                //Validamos que las fechas de los periodos nominales no excedan la fecha indicada como final en el calendario de nómina.
                if (((DateTime)Fecha_Inicio_Anyo_Nominal).AddDays(Dias_Periodo_Nominal) > Convert.ToDateTime(HttpUtility.HtmlDecode(Txt_Fecha_Fin.Text.Trim())))
                {
                    throw new Exception("--> Las fechas de los periodos nominales no pueden exeder la fecha de fin del calendario de nomina.");
                }
                //Si corresponde a un periodo válido dentro del rango. Se genera el registro a agregar al grid.
                Dr_Periodo_Nominal = Dt_Periodos_Nominales.NewRow();
                Dr_Periodo_Nominal[Cat_Nom_Nominas_Detalles.Campo_No_Nomina] = Contador_Periodos_Generados;
                Dr_Periodo_Nominal[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio] = ((DateTime)Fecha_Inicio_Anyo_Nominal).AddDays(1);
                Dr_Periodo_Nominal["Separador"] = "[ " + Dias_Periodo_Nominal + " ]";
                //Se realiza el incremento de los dias a considerar para dicho periodo nominal.
                Fecha_Inicio_Anyo_Nominal = ((DateTime)Fecha_Inicio_Anyo_Nominal).AddDays(Dias_Periodo_Nominal);
                Dr_Periodo_Nominal[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin] = ((DateTime)Fecha_Inicio_Anyo_Nominal);
                Dt_Periodos_Nominales.Rows.Add(Dr_Periodo_Nominal);//Agregamos el resgitro del periodo nominal a la tabla de periodos nominales.
            }
            //Almacenamos la variable temporal en una session para evitar que la información de los periodos generados se pierda.
            Session["Periodos_Nominales"] = Dt_Periodos_Nominales;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar los periodos nominales indicados. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: LLenar_Grid_Periodos_Nominales
    /// 
    /// DESCRIPCIÓN: Carga el GridView de periodos nominales.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 11:08 am.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    private void LLenar_Grid_Periodos_Nominales()
    {
        DataTable Dt_Tabla_Periodos_Nominales = null;   //Variable que almacenará los periodos nominales generados.

        try
        {
            //Validamos que existan periodos nominales que  mostrar en la tabla de periodos nominales.
            if (Session["Periodos_Nominales"] != null)
            {
                //Obtenemos la tabla de periodos nominales de la session que la almacena.
                Dt_Tabla_Periodos_Nominales = (DataTable)Session["Periodos_Nominales"];
                //Validamos que la tabla de periodos nominales extraida de la session sea una instancia de un objeto DataTable.
                if (Dt_Tabla_Periodos_Nominales is DataTable)
                {
                    //Validamos que la tabla de periodos nominales contenga registros que mostrar en la tabla de periodos nominales.
                    if (Dt_Tabla_Periodos_Nominales.Rows.Count > 0)
                    {
                        Grid_Catorcenas.DataSource = Dt_Tabla_Periodos_Nominales;
                        Grid_Catorcenas.DataBind();
                        Grid_Catorcenas.SelectedIndex = -1;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al llenar el grid de periodos nominales. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Habilitar_Opcion_Estatus_Grid
    /// 
    /// DESCRIPCIÓN: Habilita o Deshabilita la opción del estatus del periodo nominal dentro del GridView.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 11:08 am.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    private void Habilitar_Opcion_Estatus_Grid()
    {
        DateTime? Fecha_Ultima_Periodo_Nominal_Generado = null;
        DateTime? Fecha_Fin_Periodo_Nominal = null;

        try
        {
            //Recorremos el Grid ó tabla de periodos nominales para válidar que si la misma tiene menos de 26 periodos generados
            //no permita modificar los estatus, esta operación se hailitara hasta que el número de periodos nominales sea el correcto
            //para el calendario de la nomina.
            if (!string.IsNullOrEmpty(Grid_Catorcenas.Rows[Grid_Catorcenas.Rows.Count - 1].Cells[2].Text.Trim()))
            {
                Fecha_Ultima_Periodo_Nominal_Generado = Convert.ToDateTime(Grid_Catorcenas.Rows[Grid_Catorcenas.Rows.Count - 1].Cells[4].Text.Trim());
                if (!string.IsNullOrEmpty(Txt_Fecha_Fin.Text.Trim()))
                {
                    Fecha_Fin_Periodo_Nominal = Convert.ToDateTime(Txt_Fecha_Fin.Text.Trim());
                    if (Fecha_Ultima_Periodo_Nominal_Generado == Fecha_Fin_Periodo_Nominal)
                    {
                        foreach (GridViewRow Fila in Grid_Catorcenas.Rows)
                        {
                            Fila.Cells[0].Enabled = true;//Si el número de periodos generados es menor a 26. La opción de Estatus estará deshabilitada.   
                        }
                    }
                    else
                    {
                        foreach (GridViewRow Fila in Grid_Catorcenas.Rows)
                        {
                            Fila.Cells[0].Enabled = false;//Si el número de periodos generados es menor a 26. La opción de Estatus estará deshabilitada.   
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar o deshabilitar la opción de estatus en el grid de periodos nóminales. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Validar_Periodos_Nominales_Generados
    /// 
    /// DESCRIPCIÓN: Valida que los periodos nominales generados hallan sido generados correctamente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 13:06 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    private void Validar_Periodos_Nominales_Generados()
    {
        Int32 No_Periodos_7_Dias = 0;
        Int32 No_Periodos_14_Dias = 0;
        Int32 No_Periodos_21_Dias = 0;
        String Fecha_Fin_Ultimo_Periodo_Generado = "";
        String Cadena_Leida = "";
        String Mensaje = "";
        Boolean Datos_Validos = false;


        try
        {
            foreach (GridViewRow Fila_Grid in Grid_Catorcenas.Rows) {
                Cadena_Leida = Fila_Grid.Cells[3].Text.Trim();
                Cadena_Leida = Cadena_Leida.Replace("[", "");
                Cadena_Leida = Cadena_Leida.Replace("]", "");

                switch (Cadena_Leida.Trim())
                {
                    case "7":
                        ++No_Periodos_7_Dias;
                        break;
                    case "14":
                        ++No_Periodos_14_Dias;
                        break;
                    case "21":
                        ++No_Periodos_21_Dias;
                        break;
                    default:
                        break;
                }                               
            }

            if (No_Periodos_7_Dias != 1)
            {
                Mensaje += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ No puede haber mas de un periodo de 7 días. <br />";
            }
            else if (No_Periodos_14_Dias != 24)
            {
                Mensaje += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ No puede haber mas o menos de 24 periodos de 14 días. <br />";
            }
            else if (No_Periodos_21_Dias != 1)
            {
                Mensaje += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ No puede haber mas de un periodo de 21 días. <br />";
            }

            if(!Grid_Catorcenas.Rows[0].Cells[3].Text.Trim().Contains("7"))
                Mensaje += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ El Primer periodo deberá ser de 7 días. <br />";

            if (!Grid_Catorcenas.Rows[Grid_Catorcenas.Rows.Count -1].Cells[3].Text.Trim().Contains("21"))
                Mensaje += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ El último periodo deberá ser de 21 días. <br />";

            if (!String.IsNullOrEmpty(Grid_Catorcenas.Rows[Grid_Catorcenas.Rows.Count - 1].Cells[4].Text.Trim()))
            {
                DateTime Fecha_Ultimo_Periodo = Convert.ToDateTime(Grid_Catorcenas.Rows[Grid_Catorcenas.Rows.Count - 1].Cells[4].Text.Trim());
                DateTime Fecha_Final_Calendario_Nomina = Convert.ToDateTime(Txt_Fecha_Fin.Text.Trim());

                if (new DateTime(Fecha_Ultimo_Periodo.Year, Fecha_Ultimo_Periodo.Month, Fecha_Ultimo_Periodo.Day, 0, 0 ,0) !=
                    new DateTime(Fecha_Final_Calendario_Nomina.Year, Fecha_Final_Calendario_Nomina.Month, Fecha_Final_Calendario_Nomina.Day, 0, 0, 0))
                {
                    Mensaje += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ El último periodo de nómina generado de coincidir con la fecha final del calendario de nómina. <br />";
                }
            }

            if (!String.IsNullOrEmpty(Mensaje)) {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Mensaje = "<br />" + Mensaje;
                Lbl_Mensaje_Error.Text = Mensaje;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar los periodos nominales generados. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Consulta_Periodos_Nominales
    /// 
    /// DESCRIPCIÓN: Consulta los periodos nominales que existen actualmente en el sistema como detalles de la nomina.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 13:06 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    private void Consulta_Periodos_Nominales(String Nomina_ID)
    {
        int Count_Fila = 0;
        CheckBox _Chk_Estatus_Catorcena;
        DataTable Dt_Catorcenas;
        Cls_Cat_Nom_Calendario_Nominas_Negocio Cat_Nom_Calendario_Nominas_Negocio;
        DataTable Dt_Periodos_Nominales = new DataTable("PERIODOS_NOMINALES");
        DataRow Dr_Periodo_Nominal_Insertar = null;

        try
        {
            Dt_Periodos_Nominales.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_No_Nomina, Type.GetType("System.Int32"));//Indica el número de periodo generado.
            Dt_Periodos_Nominales.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio, Type.GetType("System.DateTime"));//Indica la fecha de inicio del periodo nominal.
            Dt_Periodos_Nominales.Columns.Add("Separador", Type.GetType("System.String"));//Separador utilizado para separar la fechas en el grid.
            Dt_Periodos_Nominales.Columns.Add(Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin, Type.GetType("System.DateTime"));//Indica la fecha de final del periodo nominal.                

            Cat_Nom_Calendario_Nominas_Negocio = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
            Cat_Nom_Calendario_Nominas_Negocio.P_Nomina_ID = Nomina_ID.Trim();
            Dt_Catorcenas = Cat_Nom_Calendario_Nominas_Negocio.Consulta_Detalles_Nomina();

            foreach (DataRow Periodo_Nominal in Dt_Catorcenas.Rows)
            {
                if (Periodo_Nominal is DataRow) {
                    Dr_Periodo_Nominal_Insertar = Dt_Periodos_Nominales.NewRow();

                    if (!string.IsNullOrEmpty(Periodo_Nominal[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString()))
                        Dr_Periodo_Nominal_Insertar[Cat_Nom_Nominas_Detalles.Campo_No_Nomina] = Convert.ToInt32(Periodo_Nominal[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString());

                    if (!string.IsNullOrEmpty(Periodo_Nominal[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString()))
                        Dr_Periodo_Nominal_Insertar[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio] = Convert.ToDateTime(Periodo_Nominal[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());

                    Dr_Periodo_Nominal_Insertar["Separador"] = "[ " + (Cls_DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(Periodo_Nominal[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString()),
                        (Convert.ToDateTime(Periodo_Nominal[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString()))) + 1) + " ]";

                    if (!string.IsNullOrEmpty(Periodo_Nominal[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString()))
                        Dr_Periodo_Nominal_Insertar[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin] = Convert.ToDateTime(Periodo_Nominal[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                    Dt_Periodos_Nominales.Rows.Add(Dr_Periodo_Nominal_Insertar);
                }
            }

            Grid_Catorcenas.DataSource = Dt_Periodos_Nominales;
            Grid_Catorcenas.DataBind();

            if (Dt_Catorcenas.Rows.Count > 0)
            {
                foreach (DataRow Renglon in Dt_Catorcenas.Rows)
                {
                    _Chk_Estatus_Catorcena = (CheckBox)Grid_Catorcenas.Rows[Count_Fila].FindControl("Chk_Estatus_Catorcena");
                    if (Renglon[Cat_Nom_Nominas_Detalles.Campo_Estatus].ToString().Trim().Equals("INACTIVO"))
                    {
                        _Chk_Estatus_Catorcena.Checked = false;
                    }
                    else
                    {
                        _Chk_Estatus_Catorcena.Checked = true;
                    }
                    Count_Fila = Count_Fila + 1;
                }
            }//End If
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.Trim());
        }//End Catch
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Calendario_Nominas_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Calendario_Nominas_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consultar_Calendario_Nominas();
        DataTable Dt_Calendario_Nominas = (Grid_Calendario_Nominas.DataSource as DataTable);

        if (Dt_Calendario_Nominas != null)
        {
            DataView Dv_Calendario_Nominas = new DataView(Dt_Calendario_Nominas);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Calendario_Nominas.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Calendario_Nominas.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Calendario_Nominas.DataSource = Dv_Calendario_Nominas;
            Grid_Calendario_Nominas.DataBind();
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: LLenar_Cmb_Dias_Periodo_Nominal
    /// 
    /// DESCRIPCIÓN: LLena el combo que almacena la cantidad de dias de la cual puede estar compuesta la catorcena.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 21/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    private void LLenar_Cmb_Dias_Periodo_Nominal()
    {
        try
        {
            Cmb_Dias_Periodo_Nominal.DataBind();

            for (Int32 Contador = 0; Contador <= 366; Contador++) {
                if(Contador == 0)
                    Cmb_Dias_Periodo_Nominal.Items.Insert(Contador, new ListItem("<-- Selecciones -->", Contador.ToString()));
                else
                    Cmb_Dias_Periodo_Nominal.Items.Insert(Contador, new ListItem(HttpUtility.HtmlDecode((Contador == 1) ? (Contador + "&nbsp;&nbsp;Día") : (Contador + "&nbsp;&nbsp;Dias")), Contador.ToString()));
            }

            Cmb_Dias_Periodo_Nominal.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el combo de los dias a seleccionar para el periodo nominal. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Validar_Suma_Total_Dias_No_Exeda_Anyo_Nominal
    /// 
    /// DESCRIPCIÓN: Valida que los periodos que los dias de los periodos nominales no exedean los dias del año nominal
    ///              del que se esta generando la nómina.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 21/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    private Boolean Validar_Suma_Total_Dias_No_Exeda_Anyo_Nominal()
    {
        GridView Ctrl_Tabla_Periodos_Nominales = null;//Variable que hará referencia a la tabla de periodos nominales.
        String Dias_Periodo_Nominal = "";
        Int32 Contador_Dias = 0;
        Int32 Dias_Anio_Nominal = 0;
        Int32 Anio = 0;
        Boolean Estatus_Validacion = false;

        try
        {
            //Obtenemos el año del periodo nominal.
            Anio = Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim()).AddMonths(1).Year;

            //Validamos si se trata de un año bisiesto.
            if (DateTime.DaysInMonth(Anio, 2) == 28) {
                Dias_Anio_Nominal = 365;
            }
            else if (DateTime.DaysInMonth(Anio, 2) == 29) {
                Dias_Anio_Nominal = 366;
            }

            Ctrl_Tabla_Periodos_Nominales = Grid_Catorcenas;

            foreach (GridViewRow Renglon_Tabla_Periodo_Nominal in Ctrl_Tabla_Periodos_Nominales.Rows) {
                if (Renglon_Tabla_Periodo_Nominal is GridViewRow) {
                    if (!string.IsNullOrEmpty(Renglon_Tabla_Periodo_Nominal.Cells[3].Text.Trim()))
                    {
                        Dias_Periodo_Nominal = Renglon_Tabla_Periodo_Nominal.Cells[3].Text.Trim();
                        Dias_Periodo_Nominal = Dias_Periodo_Nominal.Replace("[", "");
                        Dias_Periodo_Nominal = Dias_Periodo_Nominal.Replace("]", "");
                        Dias_Periodo_Nominal = Dias_Periodo_Nominal.Trim();

                        Contador_Dias += Convert.ToInt32(Dias_Periodo_Nominal);
                    }
                }
            }

            if (Contador_Dias > Dias_Anio_Nominal)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Los dias de la tabla de periodos nominales no pueden exeder, los dias del año nóminal. Dias del Año Nominal: [" + Dias_Anio_Nominal + "] Dias.";
            }
            else { Estatus_Validacion = true; }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar los dias que tiene el año nominal. Error: " + Ex.Message + "]");
        }
        return Estatus_Validacion;
    }
}
