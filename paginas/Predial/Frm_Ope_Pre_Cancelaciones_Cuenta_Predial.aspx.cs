using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Movimientos.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Reportes;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using CrystalDecisions.Shared;

public partial class paginas_Predial_Frm_Ope_Pre_Cancelaciones_Cuenta_Predial : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load.
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página.
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (!IsPostBack)
            {
                //Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Session["Dt_Cuentas_Canceladas"] = null;
                Configuracion_Acceso("Frm_Ope_Pre_Cancelaciones_Cuenta_Predial.aspx");
                Llenar_Combo();
                Configuracion_Formulario(true);
                String Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Lanzar_Mpe_Busqueda_Avanzada.Attributes.Add("onclick", Ventana_Modal);
                Session["ESTATUS_CUENTAS"] = "VIGENTE";

                Llenar_Cancelaciones(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. estatus.    Estatus en el que se cargara la configuración de los
    ///                            controles.
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean estatus)
    {
        if (!Btn_Nuevo.AlternateText.Equals("Actualizar"))
        {
            Cmb_Movimiento.SelectedIndex = (0);
        }
        Btn_Nuevo.Visible = true;
        Btn_Nuevo.AlternateText = "Modificar";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Imprimir.Visible = estatus;
        Btn_Imprimir.Enabled = false;
        Btn_Lanzar_Mpe_Busqueda_Avanzada.Enabled = !estatus;
        Txt_Observaciones_Cancelacion.Enabled = !estatus;
        Txt_Observaciones_Validacion.Enabled = false;
        Txt_Fecha_Final.Enabled = estatus;
        Txt_Fecha_Inicial.Enabled = estatus;
        Btn_Txt_Fecha_Final.Enabled = estatus;
        Btn_Txt_Fecha_Inicial.Enabled = estatus;
        Cmb_Movimiento.Enabled = !estatus;
        Grid_Cancelacion.Enabled = estatus;
        Grid_Cancelacion.SelectedIndex = (-1);
        Btn_Buscar_Cancelacion.Enabled = estatus;
        Txt_Busqueda_Cuenta.Enabled = estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Hdf_Cuenta_Predial_ID.Value = "";
        Txt_Adeudo_Id.Text = "";
        Txt_Adeudo_Total_Cancelado.Text = "";
        Txt_Observaciones_Cancelacion.Text = "";
        Txt_Detalle_Cuenta_Predial.Text = "";
        Txt_Detalle_Propietatio.Text = "";
        Txt_Detalle_Colonia.Text = "";
        Txt_Fecha_Inicial.Text = "";
        Txt_Fecha_Final.Text = "";
        Txt_Fecha_Inicial_Filtro.Text = "";
        Txt_Fecha_Final_Filtro.Text = "";
        Txt_Busqueda_Cuenta.Text = "";
        Cmb_Movimiento.SelectedIndex = 0;
    }

    protected void Txt_Fecha_Inicial_TextChanged(object sender, EventArgs e)
    {
        //Cls_Ope_Pre_Orden_Variacion_Negocio Cancelacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        //try
        //{
        //    Txt_Fecha_Inicial_Filtro.Text = Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString("dd/MM/yyyy");
        //}
        //catch (Exception exce)
        //{
        //    Txt_Fecha_Inicial.Text = "";
        //    Txt_Fecha_Inicial_Filtro.Text = "";
        //    //Agregar variables de fechas...
        //}
        //try
        //{
        //    Txt_Fecha_Final_Filtro.Text = Convert.ToDateTime(Txt_Fecha_Final.Text).AddDays(1).ToString("dd/MM/yyyy");
        //}
        //catch (Exception exce)
        //{
        //    Txt_Fecha_Final.Text = "";
        //    Txt_Fecha_Final_Filtro.Text = "";
        //    //Agregar variables de fechas...
        //}
        ////Realiar con las fechas...
        //if (!String.IsNullOrEmpty(Txt_Fecha_Inicial.Text))
        //{
        //    Session["Dt_Cuentas_Canceladas"] = null;
        //    Llenar_Cancelaciones(0);
        //}
    }

    protected void Txt_Fecha_Final_TextChanged(object sender, EventArgs e)
    {
        //Cls_Ope_Pre_Orden_Variacion_Negocio Cancelacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        //try
        //{
        //    Txt_Fecha_Inicial_Filtro.Text = Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString("dd/MM/yyyy");
        //}
        //catch (Exception exce)
        //{
        //    Txt_Fecha_Inicial.Text = "";
        //    Txt_Fecha_Inicial_Filtro.Text = "";
        //    //Agregar variables de fechas...
        //}
        //try
        //{
        //    Txt_Fecha_Final_Filtro.Text = Convert.ToDateTime(Txt_Fecha_Final.Text).AddDays(1).ToString("dd/MM/yyyy");
        //}
        //catch (Exception exce)
        //{
        //    Txt_Fecha_Final.Text = "";
        //    Txt_Fecha_Final_Filtro.Text = "";
        //    //Agregar variables de fechas...
        //}
        ////Realizar con las fechas...
        //if (!String.IsNullOrEmpty(Txt_Fecha_Final.Text))
        //{
        //    Session["Dt_Cuentas_Canceladas"] = null;
        //    Llenar_Cancelaciones(0);
        //}
    }

    #region Grids
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Convenios_Cuenta_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento del grid de convenios que muestra los detalles de la fila en una ventana emergente
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:Jesus Toledo
    ///FECHA_MODIFICO: 11/11/11
    ///CAUSA_MODIFICACIÓN: Cargar datos en cajas de texto
    ///*******************************************************************************
    protected void Grid_Cancelacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Cargar_Ventana_Emergente_Validacion_Orden_Variacion();
        if (Grid_Cancelacion.SelectedRow != null)
        {
            Hdf_Orden_Variacion.Value = Grid_Cancelacion.SelectedDataKey["NO_ORDEN"].ToString();
            Hdf_Anio_Orden.Value = Grid_Cancelacion.SelectedDataKey["ANIO_ORDEN"].ToString();
            if (!String.IsNullOrEmpty(Grid_Cancelacion.SelectedRow.Cells[2].Text))
                Cargar_Datos_Cuenta_Predial(Grid_Cancelacion.SelectedRow.Cells[2].Text);
            Btn_Imprimir.Enabled = true;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Validacion_Orden_Variacion
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Validacion_Orden_Variacion()
    {
        String Ventana_Modal_Movimientos = "Abrir_Ventana_Modal('Ventanas_Emergentes/Resumen_Predial/Frm_Validacion_Orden_Variacion.aspx";
        String Parametros = "?Año_Orden_Variacion=" + Grid_Cancelacion.SelectedDataKey["ANIO_ORDEN"].ToString();
        Parametros += "&No_Orden_Variacion=" + Convert.ToInt64(Grid_Cancelacion.SelectedDataKey["NO_ORDEN"]).ToString("0000000000");
        //Parametros += "&No_Contrarecibo=" + Grid_Historial_Movimientos.SelectedRow.Cells[3].Text;
        Parametros += "&Cuenta_Predial=" + Grid_Cancelacion.SelectedRow.Cells[2].Text;
        Parametros += "&Cuenta_Predial_ID=" + Hdf_Cuenta_Predial_ID.Value;
        Parametros += "'";
        String Propiedades = ", 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHeight:800px;dialogHide:true;help:no;scroll:yes');";
        Grid_Cancelacion.Attributes.Add("onclick", Ventana_Modal_Movimientos + Parametros + Propiedades);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Cancelaciones
    ///DESCRIPCIÓN: Llena la tabla de Cajas
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 24/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Cancelaciones(int Pagina)
    {
        try
        {
            DataTable Dt_Aux = null;
            Cls_Ope_Pre_Orden_Variacion_Negocio Cancelacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Cancelacion.P_Generar_Orden_Fecha_Inicial = Txt_Fecha_Inicial.Text.Replace("_", "").Replace("//", "");
            Cancelacion.P_Generar_Orden_Fecha_Final = Txt_Fecha_Final.Text.Replace("_", "").Replace("//", "");
            if (Txt_Fecha_Final.Text.Trim().Replace("_", "").Replace("//", "") != "")
            {
                Cancelacion.P_Generar_Orden_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text.Replace("_", "").Replace("//", "")).AddDays(1).ToString("dd/MMM/yyyy");
            }
            Cancelacion.P_Generar_Orden_Estatus = "ACEPTADA', 'RECHAZADA";
            Cancelacion.P_Estatus_Cuenta = "CANCELADA";
            Cancelacion.P_Cargar_Modulos = "CANCELACION_CUENTAS";
            Cancelacion.P_Cuenta_Predial = Txt_Busqueda_Cuenta.Text.ToUpper();
            if (Session["Dt_Cuentas_Canceladas"] == null)
            {
                Dt_Aux = Cancelacion.Consultar_Cuentas_Reactivadas();
                Session["Dt_Cuentas_Canceladas"] = Dt_Aux;
            }
            else
            {
                Dt_Aux = (DataTable)Session["Dt_Cuentas_Canceladas"];
            }
            Grid_Cancelacion.DataSource = Dt_Aux;
            Grid_Cancelacion.PageIndex = Pagina;
            Grid_Cancelacion.DataBind();
            if (Grid_Cancelacion.Rows.Count == 0)
            {
                Txt_Adeudo_Total_Cancelado.Text = "0.00";
            }
            else
            {
                double Adeuto_Total = 0.00;
                foreach (DataRow Renglon_Actual in Dt_Aux.Rows)
                {
                    if (Renglon_Actual["ADEUDO_CANCELADO"] != null)
                    {
                        if (Renglon_Actual["ADEUDO_CANCELADO"].ToString() != "")
                        {
                            Adeuto_Total += Convert.ToDouble(Renglon_Actual["ADEUDO_CANCELADO"]);
                        }
                        else
                        {
                            Renglon_Actual["ADEUDO_CANCELADO"] = "0";
                        }
                    }
                    else
                    {
                        Renglon_Actual["ADEUDO_CANCELADO"] = "0";
                    }
                }
                Txt_Adeudo_Total_Cancelado.Text = "" + Adeuto_Total.ToString("#,###,###,##0.00");
            }
            Grid_Cancelacion.PageIndex = Pagina;
            Grid_Cancelacion.DataBind();
        }
        catch (Exception Ex)
        {
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Detalle_Cuenta_Predial
    ///DESCRIPCIÓN          : Limpia los componentes del modal de detalles.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Limpiar_Detalle_Cuenta_Predial()
    {
        Hdf_Cuenta_Predial_ID.Value = "";
        Txt_Detalle_Colonia.Text = "";
        Txt_Detalle_Cuenta_Predial.Text = "";
        Txt_Detalle_Propietatio.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Detalle_Cuenta_Predial_Click
    ///DESCRIPCIÓN: Cierra el modal de detalles de cuenta
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 15/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cerrar_Detalle_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Detalle_Cuenta_Predial();
    }

    protected void Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Ubicaciones;
        String Cuenta_Predial_ID;
        String Cuenta_Predial;

        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdf_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Detalle_Cuenta_Predial.Text = Cuenta_Predial;
                Consultar_Detalle_Cuenta_Predial();
            }
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        Session.Remove("CUENTA_PREDIAL");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Detalles_Constancias_Click
    ///DESCRIPCIÓN          : Consulta el Detalle de la cuenta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Detalles_Constancias_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Detalle_Cuenta_Predial();
        Consultar_Detalle_Cuenta_Predial();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Detalle_Cuenta_Predial
    ///DESCRIPCIÓN          : Realiza la búsqueda de los datos de la cuenta predial introducida
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Consultar_Detalle_Cuenta_Predial()
    {
        DataTable Dt_Cuentas_Predial;
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        if (Txt_Detalle_Cuenta_Predial.Text.Trim() != "")
        {
            Cuentas_Predial.P_Cuenta_Predial = Txt_Detalle_Cuenta_Predial.Text.Trim();
            Cuentas_Predial.P_Incluir_Campos_Foraneos = true;
            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            if (Dt_Cuentas_Predial.Rows.Count > 0)
            {
                Txt_Detalle_Cuenta_Predial.Text = Dt_Cuentas_Predial.Rows[0]["CUENTA_PREDIAL"].ToString();
                Txt_Detalle_Propietatio.Text = Dt_Cuentas_Predial.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                Txt_Detalle_Colonia.Text = Dt_Cuentas_Predial.Rows[0]["NOMBRE_COLONIA"].ToString();
            }
        }
        else
        {
            //Mpe_Detalles_Cuenta_Predial.Hide();
            Lbl_Mensaje_Error.Text = "* Seleccione una cuenta predial antes de ver los detalles de la misma.";
            Lbl_Mensaje_Error.Visible = true;
        }
    }


    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo
    ///DESCRIPCIÓN: Llena el combo de movimientos
    ///PROPIEDADES:         
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo()
    {
        try
        {
            Cls_Cat_Pre_Movimientos_Negocio Movimientos = new Cls_Cat_Pre_Movimientos_Negocio();
            Movimientos.P_Cargar_Modulos = "LIKE 'CANCELACION_CUENTAS'";
            DataTable tabla = Movimientos.Consultar_Movimientos_Cancelacion();
            DataRow fila = tabla.NewRow();
            fila[Cat_Pre_Movimientos.Campo_Movimiento_ID] = "SELECCIONE";
            fila[Cat_Pre_Movimientos.Campo_Identificador] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            tabla.Rows.InsertAt(fila, 0);
            Cmb_Movimiento.DataSource = tabla;
            Cmb_Movimiento.DataValueField = Cat_Pre_Movimientos.Campo_Movimiento_ID;
            Cmb_Movimiento.DataTextField = Cat_Pre_Movimientos.Campo_Identificador;
            Cmb_Movimiento.DataBind();
        }
        catch (Exception Ex)
        {
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }

    #endregion

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación.
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 23/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Movimiento.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Movimientos.";
            Validacion = false;
        }
        if (Txt_Detalle_Cuenta_Predial.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Cuenta Predial.";
            Validacion = false;
        }
        if (Txt_Observaciones_Cancelacion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir los Comentarios.";
            Validacion = false;
        }
        if (Validacion == false)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Mensaje_Error;
            Img_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Cancelaciones_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de las Cancelaciones 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cancelaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Cancelacion.SelectedIndex = (-1);
        Llenar_Cancelaciones(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer una Cancelación
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataSet Dt_Imprimir_Reactivacion = new DataSet();
        DataTable Dt_Datos_Cuenta = new DataTable();
        DataSet Ds_Actual = new DataSet();
        String Orden_Reactivacion = null;
        String Dato2 = "";
        Cls_Ope_Pre_Parametros_Negocio Anio_Corriente = new Cls_Ope_Pre_Parametros_Negocio();
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta_Predial_Negocio = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Modificar"))
            {
                Btn_Nuevo.AlternateText = "Actualizar";
                Configuracion_Formulario(false);
                Limpiar_Catalogo();
                Btn_Nuevo.AlternateText = "Actualizar";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Ope_Pre_Orden_Variacion_Negocio Cancelacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                    Cancelacion.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Cancelacion.P_Generar_Orden_Obserbaciones = Txt_Observaciones_Cancelacion.Text.ToUpper();
                    Cancelacion.P_Generar_Orden_Movimiento_ID = Cmb_Movimiento.SelectedItem.Value;
                    Cancelacion.P_Generar_Orden_Estatus = "POR VALIDAR";
                    Cancelacion.P_Generar_Orden_Cuenta_ID = Hdf_Cuenta_Predial_ID.Value;
                    Cancelacion.P_Cuenta_Predial = Txt_Detalle_Cuenta_Predial.Text.Trim();
                    Cuenta_Predial_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Dt_Datos_Cuenta = Cuenta_Predial_Negocio.Consultar_Cuenta();

                    for (int i = 0; i < Dt_Datos_Cuenta.Columns.Count; i++)
                    {
                        Dato2 = Dt_Datos_Cuenta.Rows[0].ItemArray[i].ToString();
                        Cancelacion.Agregar_Variacion(Dt_Datos_Cuenta.Columns[i].ToString(), Dato2);
                    }

                    Cancelacion.Agregar_Variacion(Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta, "CANCELADA");
                    Orden_Reactivacion = Cancelacion.Generar_Orden_Variacion();

                    //DataTable para Impresion 
                    Cancelacion.P_Generar_Orden_No_Orden = Orden_Reactivacion;
                    Cancelacion.P_Generar_Orden_Anio = Anio_Corriente.Consultar_Anio_Corriente().ToString();
                    Dt_Imprimir_Reactivacion = Cancelacion.Consulta_Datos_Reporte();
                    Ruta_Reporte_Crystal = "../Rpt/Predial/Rpt_Pre_Orden_Variacion.rpt";

                    // Se crea el nombre del reporte
                    String Nombre_Reporte = "Orden_Variacion_Cancelacion" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));
                    Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
                    Generar_Reporte(ref Dt_Imprimir_Reactivacion, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, "PDF");
                    Mostrar_Reporte(Nombre_Reporte_Generar, "PDF");

                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Cancelaciones(Grid_Cancelacion.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Cancelación de pago predial", "alert('Actualización de Cancelación de cuenta predial Exitosa');", true);
                    Btn_Nuevo.AlternateText = "Modificar";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de una Cancelación ya hecha
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 13/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Cancelacion_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Cancelacion_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Cancelacion.SelectedIndex = (-1);
        Llenar_Cancelaciones(0);
        Limpiar_Catalogo();
        if (Grid_Cancelacion.Rows.Count == 0 && Txt_Busqueda_Cuenta.Text.Trim().Length > 0)
        {
            Lbl_Mensaje_Error.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda_Cuenta.Text + "\" no se encotrarón coincidencias";
            Lbl_Mensaje_Error.Text = "<br>(Se cargaron todos las Cancelaciones de cuenta predial almacenadas)";
            Lbl_Mensaje_Error.Visible = true;
            Txt_Busqueda_Cuenta.Text = "";
            Llenar_Cancelaciones(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Limpiar_Catalogo();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
    }

    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Dt_Imprimir_Reactivacion = new DataSet();
        Cls_Ope_Pre_Orden_Variacion_Negocio Cancelacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        try
        {
            if (!String.IsNullOrEmpty(Hdf_Orden_Variacion.Value))
            {
                Cancelacion.P_Generar_Orden_No_Orden = Hdf_Orden_Variacion.Value.ToString();
                Cancelacion.P_Generar_Orden_Anio = Hdf_Anio_Orden.Value.ToString();
                Dt_Imprimir_Reactivacion = Cancelacion.Consulta_Datos_Reporte();
                Ruta_Reporte_Crystal = "../Rpt/Predial/Rpt_Pre_Orden_Variacion.rpt";
                // Se crea el nombre del reporte
                String Nombre_Reporte = "Orden_Variacion_Cancelacion" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
                Generar_Reporte(ref Dt_Imprimir_Reactivacion, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, "PDF");
                Mostrar_Reporte(Nombre_Reporte_Generar, "PDF");
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Ocurrió un error al Generar Reporte. Favor de Reimprir Orden No. " + Convert.ToInt32(Hdf_Orden_Variacion.Value).ToString();
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Cuenta_Predial
    ///DESCRIPCIÓN: Carga los Campos con los Datos de la Cuenta predial Seleccionada.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos_Cuenta_Predial(String P_Cuenta_Predial)
    {
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        RP_Negocio.P_Cuenta_Predial = P_Cuenta_Predial;
        DataTable Dt_Datos = RP_Negocio.Consultar_Cuentas_Predial();
        if (Dt_Datos.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_ID.Value = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim() : "";
            Txt_Detalle_Cuenta_Predial.Text = Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim();
            Txt_Detalle_Propietatio.Text = Dt_Datos.Rows[0]["PROPIETARIO"].ToString().Trim();
            Txt_Detalle_Colonia.Text = Dt_Datos.Rows[0]["UBICACION"].ToString().Trim();
            if (Grid_Cancelacion.SelectedIndex != -1)
            {

                if (!String.IsNullOrEmpty(Grid_Cancelacion.SelectedRow.Cells[9].Text))
                {
                    Txt_Observaciones_Cancelacion.Text = Grid_Cancelacion.SelectedRow.Cells[9].Text;
                }
                if (!String.IsNullOrEmpty(Grid_Cancelacion.SelectedDataKey["OBSERVACIONES_VALIDACION"].ToString()))
                {
                    Txt_Observaciones_Validacion.Text = Grid_Cancelacion.SelectedDataKey["OBSERVACIONES_VALIDACION"].ToString();
                }
                if (!String.IsNullOrEmpty(Grid_Cancelacion.SelectedDataKey["MOVIMIENTO_ID"].ToString()))
                {
                    try
                    {
                        Cmb_Movimiento.SelectedValue = Grid_Cancelacion.SelectedDataKey["MOVIMIENTO_ID"].ToString();
                    }
                    catch
                    {
                        Cmb_Movimiento.SelectedIndex = -1;
                    }
                }
            }
            //RP_Negocio.P_Cuenta_Predial = Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim();
            //DataTable Dt_Adeudos = RP_Negocio.Consultar_Adeudos_Cuentas_Predial();
            //Llenar_Combo_Anios(Dt_Adeudos);
            //Cmb_Bimestre_Final.SelectedIndex = (Cmb_Bimestre_Final.Items.Count - 1);
            //Cmb_Anio_Final.SelectedIndex = (Cmb_Anio_Final.Items.Count - 1);
        }
        else
        {
            //Lbl_Ecabezado_Mensaje.Text = "No se han encontrado datos para la Cuenta Predial.";
            Lbl_Mensaje_Error.Text = "";
            //Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Hace la Busqueda mediante la cuenta predial Seleccionada.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Lanzar_Mpe_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            String Cuenta_Predial = null;
            if (Session["CUENTA_PREDIAL"] != null)
            {
                Cuenta_Predial = Session["CUENTA_PREDIAL"].ToString();
                Cargar_Datos_Cuenta_Predial(Cuenta_Predial);
            }
            Session.Remove("CUENTA_PREDIAL_ID");
            Session.Remove("CUENTA_PREDIAL");
        }
        catch (Exception Ex)
        {
            //Lbl_Ecabezado_Mensaje.Text = "Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            //Div_Contenedor_Msj_Error.Visible = true;
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
            Botones.Add(Btn_Imprimir);
            Botones.Add(Btn_Buscar_Cancelacion);
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

    #region Metodos Reportes

    /// *************************************************************************************
    /// NOMBRE:             Generar_Reporte
    /// DESCRIPCIÓN:        Método que invoca la generación del reporte.
    ///              
    /// PARÁMETROS:         Ds_Reporte_Crystal.- Es el DataSet con el que se muestra el reporte en cristal 
    ///                     Ruta_Reporte_Crystal.-  Ruta y Nombre del archivo del Crystal Report.
    ///                     Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    ///                     Formato.- Es el tipo de reporte "PDF", "Excel"
    /// USUARIO CREO:       Juan Alberto Hernández Negrete.
    /// FECHA CREO:         3/Mayo/2011 18:15 p.m.
    /// USUARIO MODIFICO:   Salvador Henrnandez Ramirez
    /// FECHA MODIFICO:     16/Mayo/2011
    /// CAUSA MODIFICACIÓN: Se cambio Nombre_Plantilla_Reporte por Ruta_Reporte_Crystal, ya que este contendrá tambien la ruta
    ///                     y se asigno la opción para que se tenga acceso al método que muestra el reporte en Excel.
    /// *************************************************************************************
    public void Generar_Reporte(ref DataSet Ds_Reporte_Crystal, String Ruta_Reporte_Crystal, String Nombre_Reporte_Generar, String Formato)
    {
        ReportDocument Reporte = new ReportDocument(); // Variable de tipo reporte.
        String Ruta = String.Empty;  // Variable que almacenará la ruta del archivo del crystal report. 

        try
        {
            Ruta = HttpContext.Current.Server.MapPath(Ruta_Reporte_Crystal);
            Reporte.Load(Ruta);

            if (Ds_Reporte_Crystal is DataSet)
            {
                if (Ds_Reporte_Crystal.Tables.Count > 0)
                {
                    //ParameterField Parametro = new ParameterField();
                    //ParameterDiscreteValue Valor = new ParameterDiscreteValue();
                    //ParameterFields Parametros = new ParameterFields();
                    //ParameterValues Valores_Parametros = new ParameterValues();

                    //Parametro.Name = "Titulo_Reporte";
                    //Valor.Value = "BAJA DEFINITIVA DE PREDIO";
                    //Parametro.CurrentValues.Add(Valor);
                    //Parametros.Add(Parametro);
                    //Valores_Parametros = Parametro.CurrentValues;

                    Reporte.SetDataSource(Ds_Reporte_Crystal);
                    Reporte.Subreports["DT_COPROPIETARIOS"].SetDataSource(Ds_Reporte_Crystal.Tables["DT_COPROPIETARIOS"]);
                    Reporte.SetParameterValue("Titulo_Reporte", "BAJA DEFINITIVA DE PREDIO");
                    if (Formato == "PDF")
                    {
                        Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE:             Exportar_Reporte_PDF
    /// DESCRIPCIÓN:        Método que guarda el reporte generado en formato PDF en la ruta
    ///                     especificada.
    /// PARÁMETROS:         Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///                     Nombre_Reporte.- Nombre que se le dio al reporte.
    /// USUARIO CREO:       Juan Alberto Hernández Negrete.
    /// FECHA CREO:         3/Mayo/2011 18:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    public void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte_Generar)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = HttpContext.Current.Server.MapPath("../../Reporte/" + Nombre_Reporte_Generar);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
        }
    }


    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN     : Btn_Consultar_Por_Fechas_Click
    ///DESCRIPCIÓN              : Evento Click de botón para Consultar las Cuentas para Reactivar en base al rango de Fechas dado
    ///PROPIEDADES:
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 02/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Consultar_Por_Fechas_Click(object sender, ImageClickEventArgs e)
    {
        if (!String.IsNullOrEmpty(Txt_Fecha_Final.Text))
        {
            Session["Dt_Cuentas_Canceladas"] = null;
            Llenar_Cancelaciones(0);
        }
    }
}
