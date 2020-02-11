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
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Reportes;
using CrystalDecisions.Shared;
using Presidencia.Operacion_Predial_Parametros.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Reactivaciones_Cuenta_Predial : System.Web.UI.Page
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
            //Respons6e.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (!IsPostBack)
            {
                Session["Dt_Cuentas_Canceladas"] = null;
                Configuracion_Acceso("Frm_Ope_Pre_Reactivaciones_Cuenta_Predial.aspx");
                Llenar_Combo();
                Configuracion_Formulario(true);
                Llenar_Reactivaciones(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Adeudo_Diferencias
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente de los Adeudos con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Adeudo_Diferencias()
    {
        String Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Resumen_Predial/Frm_Adeudo_Diferencias.aspx";
        String Propiedades = ", 'resizable=no,status=no,width=750,scrollbars=yes');";
        Btn_Vista_Previa.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "&Anio_Orden=" + Grid_Cancelacion.SelectedDataKey["ANIO_ORDEN"].ToString() + "&Consulta_Adeudos_Cancelados=true'" + Propiedades);
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
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        if (Btn_Modificar.AlternateText.Equals("Actualizar"))
        {
        }
        else
        {
            Cmb_Movimiento.SelectedIndex = (0);
        }
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Btn_Imprimir.Visible = estatus;
        Btn_Imprimir.Enabled = !estatus;
        Txt_Observaciones.Enabled = !estatus;
        Txt_Fecha_Final.Enabled = estatus;
        Txt_Fecha_Inicial.Enabled = estatus;
        Btn_Txt_Fecha_Final.Enabled = estatus;
        Btn_Txt_Fecha_Inicial.Enabled = estatus;
        Cmb_Movimiento.Enabled = !estatus;
        Btn_Buscar_Reactivacion.Enabled = estatus;
        //Btn_Mostrar_Busqueda_Cuentas.Enabled = !estatus;
        Grid_Cancelacion.Enabled = estatus;
        Grid_Cancelacion.SelectedIndex = -1;
        Btn_Vista_Previa.Enabled = true;
        Txt_Busqueda_Reactivacion.Enabled = estatus;
        Orden.Consulta_Id_Movimiento("RP");
        //Cmb_Movimiento.SelectedValue = Orden.P_Generar_Orden_Movimiento_ID;
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
        Txt_Adeudo_Id.Text = "";
        Txt_Observaciones.Text = "";
        Txt_Cuenta_Predial.Text = "";
        //Txt_Fecha_Inicial.Text = "";
        //Txt_Fecha_Final.Text = "";
        //Txt_Fecha_Inicial_Filtro.Text = "";
        //Txt_Fecha_Final_Filtro.Text = "";
        Txt_Busqueda_Reactivacion.Text = "";
        Cmb_Movimiento.SelectedIndex = 0;
        Grid_Cancelacion.SelectedIndex = -1;
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
        //Session["Dt_Cuentas_Canceladas"] = null;
        ////Realiar con las fechas...
        //Llenar_Reactivaciones(0);
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
        //if (Txt_Fecha_Final.Text.Trim() != "")
        //{
        //    Session["Dt_Cuentas_Canceladas"] = null;
        //    Llenar_Reactivaciones(0);
        //}
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Reactivaciones
    ///DESCRIPCIÓN: Llena la tabla de Reactivaciones de Cuentas Prediales
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 13/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Reactivaciones(int Pagina)
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
            Cancelacion.P_Generar_Orden_Estatus = "ACEPTADA', 'VIGENTE', 'RECHAZADA";
            Cancelacion.P_Estatus_Cuenta = "CANCELADA";
            Cancelacion.P_Cargar_Modulos = "REACTIVACION_CUENTAS";
            Cancelacion.P_Cuenta_Predial = Txt_Busqueda_Reactivacion.Text.ToUpper();
            if (Session["Dt_Cuentas_Canceladas"] == null)
            {
                Dt_Aux = Cancelacion.Consultar_Cuentas_Reactivadas();
                Session["Dt_Cuentas_Canceladas"] = Dt_Aux;
            }
            else
            {
                Dt_Aux = (DataTable)Session["Dt_Cuentas_Canceladas"];
            }
            Grid_Cancelacion.Columns[1].Visible = true;
            Grid_Cancelacion.DataSource = Dt_Aux;
            foreach (DataRow Renglon_Actual in Dt_Aux.Rows)
            {
                if (Renglon_Actual["ADEUDO_CANCELADO"] != null)
                {
                    if (Renglon_Actual["ADEUDO_CANCELADO"].ToString() == "")
                    {
                        Renglon_Actual["ADEUDO_CANCELADO"] = "0";
                    }
                }
            }
            Grid_Cancelacion.PageIndex = Pagina;
            Grid_Cancelacion.DataBind();
            Grid_Cancelacion.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }

    //protected void Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click(object sender, ImageClickEventArgs e)
    //{
    //    Boolean Busqueda_Ubicaciones;
    //    String Cuenta_Predial_ID;
    //    String Cuenta_Predial;

    //    Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
    //    if (Busqueda_Ubicaciones)
    //    {
    //        if (Session["CUENTA_PREDIAL_ID"] != null)
    //        {
    //            Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
    //            Txt_Adeudo_Id.Text = Cuenta_Predial_ID;
    //            Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
    //            Txt_Cuenta_Predial.Text = Cuenta_Predial;
    //        }
    //    }
    //    Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
    //    Session.Remove("CUENTA_PREDIAL_ID");
    //    Session.Remove("CUENTA_PREDIAL");
    //}

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
            Movimientos.P_Cargar_Modulos = "LIKE 'REACTIVACION_CUENTAS'";
            DataTable tabla = Movimientos.Consultar_Nombre_Id_Movimientos();
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Fechas_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de una Fecha de Aplicación seleccionada para mostrarla a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Reactivaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Cancelacion.SelectedIndex > (-1))
        {
            Lbl_Mensaje_Error.Text = "";
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Visible = false;

            Grid_Cancelacion.Columns[1].Visible = true;
            String Id = Grid_Cancelacion.SelectedRow.Cells[1].Text;
            String Cuenta = Txt_Adeudo_Id.Text = Grid_Cancelacion.SelectedRow.Cells[2].Text;
            ////Limpiar_Catalogo();
            Txt_Cuenta_Predial.Text = Cuenta;
            Txt_Adeudo_Id.Text = Id;
            Hdf_Cuenta_Predial_ID.Value = Id;
            Hdf_No_Orden.Value = Grid_Cancelacion.DataKeys[Grid_Cancelacion.SelectedIndex].Values[0].ToString();
            Hdf_Anio_Orden.Value = Grid_Cancelacion.SelectedDataKey["ANIO_ORDEN"].ToString();
            Grid_Cancelacion.Columns[1].Visible = false;
            Consultar_Detalle_Cuenta_Predial();
            Btn_Imprimir.Enabled = true;

            Session["Cuenta_Predial_ID_Adeudos"] = Hdf_Cuenta_Predial_ID.Value;
            Session["Orden_Variacion_ID_Adeudos"] = Hdf_No_Orden.Value;
            Cargar_Ventana_Emergente_Adeudo_Diferencias();
        }
        else
        {
            Btn_Imprimir.Enabled = false;
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
        if (Txt_Cuenta_Predial.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Cuenta Predial.";
            Validacion = false;
        }
        if (Txt_Observaciones.Text.Trim().Length == 0)
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
        //Grid_Cancelacion.SelectedIndex = (-1);
        Llenar_Reactivaciones(e.NewPageIndex);
        //Limpiar_Catalogo();
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Cancelación
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Dt_Imprimir_Reactivacion = new DataSet();
        String Orden_Reactivacion = null;
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        String Dato2 = "";
        DataTable Dt_Datos_Cuenta = new DataTable();
        DataSet Ds_Actual = new DataSet();
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta_Predial_Negocio = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Ope_Pre_Parametros_Negocio Anio_Corriente = new Cls_Ope_Pre_Parametros_Negocio();
        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Txt_Cuenta_Predial.Text.Trim() != "")
                {
                    Btn_Modificar.AlternateText = "Actualizar";
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "* Debe seleccionar una Cuenta Cancelada para Reactivar.";
                    Lbl_Mensaje_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Ope_Pre_Orden_Variacion_Negocio Cancelacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                    Cancelacion.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;
                    Cancelacion.P_Generar_Orden_Obserbaciones = Txt_Observaciones.Text.ToUpper();
                    Cancelacion.P_Generar_Orden_Movimiento_ID = Cmb_Movimiento.SelectedItem.Value;
                    Cancelacion.P_Generar_Orden_Estatus = "POR VALIDAR";
                    Cancelacion.P_Generar_Orden_Cuenta_ID = Hdf_Cuenta_Predial_ID.Value;
                    Cuenta_Predial_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Dt_Datos_Cuenta = Cuenta_Predial_Negocio.Consultar_Cuenta();

                    for (int i = 0; i < Dt_Datos_Cuenta.Columns.Count; i++)
                    {
                        Dato2 = Dt_Datos_Cuenta.Rows[0].ItemArray[i].ToString();
                        Cancelacion.Agregar_Variacion(Dt_Datos_Cuenta.Columns[i].ToString(), Dato2);
                    }

                    Cancelacion.Agregar_Variacion(Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta, "VIGENTE");

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
                    Llenar_Reactivaciones(Grid_Cancelacion.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Reactivación de Cuenta predial", "alert('Actualización de Reactivación de cuenta predial Exitosa');", true);
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
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
        Llenar_Reactivaciones(0);
        Limpiar_Catalogo();
        if (Grid_Cancelacion.Rows.Count == 0 && Txt_Busqueda_Reactivacion.Text.Trim().Length > 0)
        {
            Lbl_Mensaje_Error.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda_Reactivacion.Text + "\" no se encotrarón coincidencias";
            Lbl_Mensaje_Error.Text = "<br>(Se cargaron todos las Reactivaciones de cuenta predial almacenadas)";
            Lbl_Mensaje_Error.Visible = true;
            Txt_Busqueda_Reactivacion.Text = "";
            Llenar_Reactivaciones(0);
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
        Txt_Detalle_Calle.Text = "";
        Txt_Detalle_Colonia.Text = "";
        Txt_Detalle_Cuenta_Predial.Text = "";
        Txt_Detalle_Estatus.Text = "";
        Txt_Detalle_Propietatio.Text = "";
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
        if (Txt_Cuenta_Predial.Text.Trim() != "")
        {
            Cuentas_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Cuentas_Predial.P_Incluir_Campos_Foraneos = true;
            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            if (Dt_Cuentas_Predial != null)
            {
                if (Dt_Cuentas_Predial.Rows.Count > 0)
                {
                    Txt_Detalle_Cuenta_Predial.Text = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
                    Txt_Detalle_Estatus.Text = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
                    Txt_Detalle_Propietatio.Text = Dt_Cuentas_Predial.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                    Txt_Detalle_Calle.Text = Dt_Cuentas_Predial.Rows[0]["NOMBRE_CALLE"].ToString();
                    Txt_Detalle_Colonia.Text = Dt_Cuentas_Predial.Rows[0]["NOMBRE_COLONIA"].ToString();
                }
            }
        }
        else
        {
            Lbl_Mensaje_Error.Text = "* Seleccione una cuenta predial antes de ver los detalles de la misma.";
            Lbl_Mensaje_Error.Visible = true;
        }
    }

    protected void Txt_Detalle_Cuenta_TextChanged(object sender, EventArgs e)
    {

        //Limpiar_Detalle_Cuenta_Predial();
        //Consultar_Detalle_Cuenta_Predial();
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Ventana_Historial_Click
    ///DESCRIPCIÓN: Cierra el modal de historial
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 15/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cerrar_Ventana_Historial_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Historial_Cuenta.Hide();
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
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Imprimir);
            Botones.Add(Btn_Buscar_Reactivacion);
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
                    Reporte.SetDataSource(Ds_Reporte_Crystal);
                    Reporte.Subreports["DT_COPROPIETARIOS"].SetDataSource(Ds_Reporte_Crystal.Tables["DT_COPROPIETARIOS"]);
                    Reporte.SetParameterValue("Titulo_Reporte", "REACTIVACIÓN DE CUENTA");
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
    ///NOMBRE DE LA FUNCIÓN     : Btn_Imprimir_Click
    ///DESCRIPCIÓN              : Manda Imprimir la Orden de Variación
    ///PROPIEDADES:
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 08/Noviembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        if (Hdf_No_Orden.Value != "")
        {
            DataSet Dt_Imprimir_Reactivacion = new DataSet();
            Cls_Ope_Pre_Orden_Variacion_Negocio Cancelacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            String Ruta_Reporte_Crystal = "";
            String Nombre_Reporte_Generar = "";
            Cancelacion.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;
            Cancelacion.P_Generar_Orden_Obserbaciones = Txt_Observaciones.Text.ToUpper();
            Cancelacion.P_Generar_Orden_Movimiento_ID = Cmb_Movimiento.SelectedItem.Value;
            Cancelacion.P_Generar_Orden_Estatus = "POR VALIDAR";
            Cancelacion.P_Generar_Orden_No_Orden = Hdf_No_Orden.Value;
            Cancelacion.P_Generar_Orden_Anio = Hdf_Anio_Orden.Value;
            Dt_Imprimir_Reactivacion = Cancelacion.Consulta_Datos_Reporte();
            Ruta_Reporte_Crystal = "../Rpt/Predial/Rpt_Pre_Orden_Variacion.rpt";
            // Se crea el nombre del reporte
            String Nombre_Reporte = "Orden_Variacion_Cancelacion" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));
            Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            Generar_Reporte(ref Dt_Imprimir_Reactivacion, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, "PDF");
            Mostrar_Reporte(Nombre_Reporte_Generar, "PDF");
        }
    }

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
        if (Txt_Fecha_Final.Text.Trim() != "")
        {
            Session["Dt_Cuentas_Canceladas"] = null;
            Llenar_Reactivaciones(0);
        }
    }
}
