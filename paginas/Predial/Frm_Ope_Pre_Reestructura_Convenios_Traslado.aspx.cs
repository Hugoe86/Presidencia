using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Convenios_Impuestos_Traslado_Dominio.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Impuestos_Fraccionamientos.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using DocumentFormat.OpenXml.Packaging;
using openXML_Wp = DocumentFormat.OpenXml.Wordprocessing;
using Presidencia.Numalet;
using System.IO;

public partial class paginas_Predial_Frm_Ope_Pre_Reestructura_Convenios_Traslado : System.Web.UI.Page
{

    #region Pago_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Page_Load(object sender, EventArgs e)
    {
        Recuperar_Variables_Sesion();
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
            //Cargar_Grid_Contrarecibos_Pendientes(0);
            Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(0);

            //Scrip para mostrar Ventana Modal de las Tasas de Traslado
            //string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            //Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal);
            Grid_Convenios_Impuestos_Traslado.Visible = true;
            Panel_Datos.Visible = false;

            Session.Remove("ESTATUS_CUENTAS");
            Session.Remove("TIPO_CONTRIBUYENTE");
            Session["ESTATUS_CUENTAS"] = "IN ('LISTO', 'BLOQUEADA','PENDIENTE')";

        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Resumen_Predio
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Resumen_Predio()
    {
        String Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Frm_Resumen_Predio.aspx";
        String Propiedades = ",'height=600,width=800,scrollbars=1');";
        Btn_Detalles_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
    }

    #endregion

    private String Boton_Pulsado = "";
    private Boolean No_Limpiar_Campos_Clave = false;

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Configuracion_Formulario
    ///DESCRIPCIÓN          : Carga una configuracion de los controles del Formulario
    ///PARAMETROS           : 1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        //Btn_Nuevo.Visible = true;
        //Btn_Nuevo.AlternateText = "Nuevo";
        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        //Btn_Modificar.Visible = true;
        //Btn_Modificar.AlternateText = "Modificar";
        //Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Txt_Calle.Enabled = false;
        Txt_Colonia.Enabled = false;
        Txt_No_Exterior.Enabled = false;
        Txt_No_Interior.Enabled = false;
        Txt_Propietario.Enabled = false;
        Txt_Cuenta_Predial.Enabled = false;
        Txt_Monto_Impuesto.Enabled = false;
        Txt_Estatus_Calculo.Enabled = false;
        Txt_Monto_Recargos.Enabled = false;
        Txt_Monto_Multas.Enabled = false;
        Txt_Recargos_Moratorios.Enabled = false;
        Txt_Realizo_Calculo.Enabled = false;
        Txt_Fecha_Calculo.Enabled = false;
        Txt_Costo_Constancia.Enabled = false;
        Txt_Imp_Div.Enabled = false;
        //Convenio
        Txt_Numero_Reestructura.Enabled = false;
        Txt_Numero_Convenio.Enabled = false;
        Txt_Realizo.Enabled = false;
        Txt_Fecha.Enabled = false;
        //Descuentos
        Txt_Total_Adeudo.Enabled = false;
        Txt_Total_Descuento.Enabled = false;
        Txt_Sub_Total.Enabled = false;
        Txt_Total_Convenio.Enabled = false;
        if (Cmb_Tipo_Solicitante.SelectedValue == "DEUDOR SOLIDARIO")
        {
            Txt_Solicitante.Enabled = true;
            Txt_RFC.Enabled = true;
        }
        else
        {
            Txt_Solicitante.Enabled = false;
            Txt_RFC.Enabled = false;
        }
        if (Boton_Pulsado == "Btn_Nuevo" || Boton_Pulsado == "")
        {
            Grid_Parcialidades.Enabled = true;
            Grid_Parcialidades.SelectedIndex = (-1);
            //Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = !Estatus;
            Btn_Detalles_Cuenta_Predial.Enabled = !Estatus;
            //Convenio
            Cmb_Estatus.Enabled = false;
            Cmb_Tipo_Solicitante.Enabled = !Estatus;
            Cmb_Periodicidad_Pago.Enabled = !Estatus;
            Txt_Numero_Parcialidades.Enabled = !Estatus;
            Txt_Observaciones.Enabled = !Estatus;
            //Descuentos
            Txt_Descuento_Recargos_Ordinarios.Enabled = false;
            Txt_Descuento_Recargos_Moratorios.Enabled = false;
            Txt_Descuento_Multas.Enabled = false;
            //Parcialidades
            Txt_Porcentaje_Anticipo.Enabled = !Estatus;
            Txt_Total_Anticipo.Enabled = !Estatus;
        }
        else
        {
            if (Boton_Pulsado == "Btn_Modificar")
            {
                Grid_Parcialidades.Enabled = true;
                Grid_Parcialidades.SelectedIndex = (-1);
                //Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = !Estatus;
                Btn_Detalles_Cuenta_Predial.Enabled = !Estatus;
                //Convenio
                Cmb_Estatus.Enabled = !Estatus;
                Cmb_Tipo_Solicitante.Enabled = !Estatus;
                Cmb_Periodicidad_Pago.Enabled = !Estatus;
                Txt_Numero_Parcialidades.Enabled = !Estatus;
                Txt_Observaciones.Enabled = !Estatus;
                //Descuentos
                Txt_Descuento_Recargos_Ordinarios.Enabled = false;
                Txt_Descuento_Recargos_Moratorios.Enabled = false;
                Txt_Descuento_Multas.Enabled = false;
                //Parcialidades
                Txt_Porcentaje_Anticipo.Enabled = !Estatus;
                Txt_Total_Anticipo.Enabled = !Estatus;
            }
        }

        Txt_Monto_Impuesto.Style["text-align"] = "right";
        Txt_Monto_Recargos.Style["text-align"] = "right";
        Txt_Monto_Multas.Style["text-align"] = "right";
        Txt_Descuento_Recargos_Ordinarios.Style["text-align"] = "right";
        Txt_Descuento_Recargos_Moratorios.Style["text-align"] = "right";
        Txt_Descuento_Multas.Style["text-align"] = "right";
        Txt_Total_Adeudo.Style["text-align"] = "right";
        Txt_Total_Descuento.Style["text-align"] = "right";
        Txt_Sub_Total.Style["text-align"] = "right";
        Txt_Porcentaje_Anticipo.Style["text-align"] = "right";
        Txt_Total_Anticipo.Style["text-align"] = "right";
        Txt_Total_Convenio.Style["text-align"] = "right";
        Txt_Recargos_Moratorios.Style["text-align"] = "right";
        Txt_Costo_Constancia.Style["text-align"] = "right";
        Txt_Imp_Div.Style["text-align"] = "right";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Catálogo
    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    ///PARAMETROS           :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        if (!No_Limpiar_Campos_Clave)
        {
            Hdf_Cuenta_Predial_ID.Value = "";
            Hdf_Propietario_ID.Value = "";
            Session["Cuenta_Predial"] = null;
        }
        //Datos Cuenta
        Txt_Cuenta_Predial.Text = "";
        Txt_Costo_Constancia.Text = "";
        Txt_Monto_Impuesto.Text = "";
        Txt_Estatus_Calculo.Text = "";
        Txt_Calle.Text = "";
        Txt_Colonia.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Propietario.Text = "";
        Txt_Monto_Recargos.Text = "";
        Txt_Monto_Multas.Text = "";
        Txt_Recargos_Moratorios.Text = "";
        Txt_Realizo_Calculo.Text = "";
        Txt_Fecha_Calculo.Text = "";
        Txt_Imp_Div.Text = "";
        //Convenio
        Txt_Numero_Convenio.Text = "";
        Txt_Numero_Reestructura.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Solicitante.Text = "";
        Txt_RFC.Text = "";
        Cmb_Tipo_Solicitante.SelectedIndex = 0;
        Txt_Numero_Parcialidades.Text = "";
        Cmb_Periodicidad_Pago.SelectedIndex = 0;
        Txt_Realizo.Text = "";
        Txt_Fecha.Text = "";
        Txt_Observaciones.Text = "";
        //Descuentos
        Txt_Descuento_Recargos_Ordinarios.Text = "";
        Txt_Descuento_Recargos_Moratorios.Text = "";
        Txt_Descuento_Multas.Text = "";
        Txt_Total_Adeudo.Text = "";
        Txt_Total_Descuento.Text = "";
        Txt_Sub_Total.Text = "";
        //Parcialidades
        Txt_Porcentaje_Anticipo.Text = "";
        Txt_Total_Anticipo.Text = "";
        Txt_Total_Convenio.Text = "";
        if (!No_Limpiar_Campos_Clave)
        {
            Grid_Convenios_Impuestos_Traslado.DataSource = null;
            Grid_Convenios_Impuestos_Traslado.DataBind();
        }
        Grid_Parcialidades.DataSource = null;
        Grid_Parcialidades.DataBind();
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Cargar_Grid_Contrarecibos_Pendientes
    /////DESCRIPCIÓN          : Llena el grid de Convenios con los registros encontrados
    /////PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 15/Agosto/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private void Cargar_Grid_Contrarecibos_Pendientes(Int32 Pagina)
    //{
    //    try
    //    {
    //        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculos_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
    //        DataTable Dt_Calculos_Impuesto_Traslado;

    //        Calculos_Impuesto_Traslado.P_Estatus = "LISTO";
    //        Dt_Calculos_Impuesto_Traslado = Calculos_Impuesto_Traslado.Consulta_Calculos_Contrarecibo();

    //        if (Dt_Calculos_Impuesto_Traslado != null)
    //        {
    //            Grid_Contrarecibos_Pendientes.DataSource = Dt_Calculos_Impuesto_Traslado;
    //            Grid_Contrarecibos_Pendientes.PageIndex = Pagina;
    //            Grid_Contrarecibos_Pendientes.DataBind();
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Convenios_Impuestos_Traslado_Dominio
    ///DESCRIPCIÓN          : Llena el grid de Convenios con los registros encontrados
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(Int32 Pagina)
    {
        try
        {
            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
            DataTable Dt_Convenios_Fraccionamientos;

            Convenios_Traslado_Dominio.P_Campos_Foraneos = true;
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Convenios_Traslado_Dominio.P_Filtros_Dinamicos = "(";
                Convenios_Traslado_Dominio.P_Filtros_Dinamicos += Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " LIKE '%" + Txt_Busqueda.Text.Trim() + "%'";
                Convenios_Traslado_Dominio.P_Filtros_Dinamicos += " OR " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Txt_Busqueda.Text.Trim() + "%')";
                Convenios_Traslado_Dominio.P_Filtros_Dinamicos += ")";
            }
            Dt_Convenios_Fraccionamientos = Convenios_Traslado_Dominio.Consultar_Convenio_Traslado_Dominio();

            if (Dt_Convenios_Fraccionamientos != null)
            {
                Grid_Convenios_Impuestos_Traslado.Columns[8].Visible = true;
                Grid_Convenios_Impuestos_Traslado.Columns[9].Visible = true;
                Grid_Convenios_Impuestos_Traslado.Columns[10].Visible = true;
                Grid_Convenios_Impuestos_Traslado.Columns[11].Visible = true;
                Grid_Convenios_Impuestos_Traslado.DataSource = Dt_Convenios_Fraccionamientos;
                Grid_Convenios_Impuestos_Traslado.PageIndex = Pagina;
                Grid_Convenios_Impuestos_Traslado.DataBind();
                Grid_Convenios_Impuestos_Traslado.Columns[8].Visible = false;
                Grid_Convenios_Impuestos_Traslado.Columns[9].Visible = false;
                Grid_Convenios_Impuestos_Traslado.Columns[10].Visible = false;
                Grid_Convenios_Impuestos_Traslado.Columns[11].Visible = false;
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
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Resolucion
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente de la Resolución con la ruta y parámetros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Resolucion()
    {
        String Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Resumen_Predial/Frm_Resolucion.aspx";
        String Parametros = "?No_Calculo=" + Hdf_No_Calculo.Value;
        Parametros += "&Cuenta_Predial_ID=" + Hdf_Cuenta_Predial_ID.Value;
        Parametros += "&Cuenta_Predial=" + Txt_Cuenta_Predial.Text;
        if (Txt_Fecha_Calculo.Text.Trim() != "")
        {
            Parametros += "&Año=" + Convert.ToDateTime(Txt_Fecha_Calculo.Text.Trim()).Year.ToString();
        }
        Parametros += "'";
        String Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:800px;dialogHide:true;help:no;scroll:on');";
        Btn_Detalles_Calculo.Attributes.Add("onclick", Ventana_Modal + Parametros + Propiedades);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Recuperar_Variables_Sesion
    ///DESCRIPCIÓN          : Lee las variable de Sesion creadas para reasignarlas a las variables del sistema.
    ///PARAMETROS           :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Recuperar_Variables_Sesion()
    {
        if (Session["Boton_Pulsado"] != null)
        {
            Boton_Pulsado = (String)Session["Boton_Pulsado"];
            Session["Boton_Pulsado"] = null;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Parcialidades
    ///DESCRIPCIÓN          : Lee el grid de las parcialidades y devuelve una instancia en un DataTable
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 08/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Parcialidades()
    {
        DataTable Dt_Parcialidades = new DataTable();
        Dt_Parcialidades.Columns.Add(new DataColumn("NO_PAGO", typeof(String)));
        Dt_Parcialidades.Columns.Add(new DataColumn("HONORARIOS", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("CONSTANCIA", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_MULTAS", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO_DIVISION", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("FECHA_VENCIMIENTO", typeof(DateTime)));
        Dt_Parcialidades.Columns.Add(new DataColumn("ESTATUS", typeof(String)));

        DataRow Dr_Parcialidades;
        //Se barre el Grid para cargar el DataTable con los valores del grid
        foreach (GridViewRow Row in Grid_Parcialidades.Rows)
        {
            Dr_Parcialidades = Dt_Parcialidades.NewRow();
            Dr_Parcialidades["NO_PAGO"] = Row.Cells[0].Text;
            Dr_Parcialidades["HONORARIOS"] = Convert.ToDouble(Row.Cells[1].Text.Replace("$", ""));
            Dr_Parcialidades["CONSTANCIA"] = Convert.ToDouble(Row.Cells[2].Text.Replace("$", ""));
            Dr_Parcialidades["MONTO_MULTAS"] = Convert.ToDouble(Row.Cells[3].Text.Replace("$", ""));
            Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Convert.ToDouble(Row.Cells[4].Text.Replace("$", ""));
            Dr_Parcialidades["RECARGOS_MORATORIOS"] = Convert.ToDouble(Row.Cells[5].Text.Replace("$", ""));
            Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDouble(Row.Cells[6].Text.Replace("$", ""));
            Dr_Parcialidades["MONTO_IMPUESTO_DIVISION"] = Convert.ToDouble(Row.Cells[7].Text.Replace("$", ""));
            Dr_Parcialidades["FECHA_VENCIMIENTO"] = Convert.ToDateTime(Row.Cells[9].Text);
            Dr_Parcialidades["ESTATUS"] = Row.Cells[10].Text;
            Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
        }
        return Dt_Parcialidades;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Convenio
    ///DESCRIPCIÓN          : Llena la tabla de Convenios por Derechos de Supervisión con una consulta que puede o no tener Filtros.
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Convenio()
    {
        try
        {
            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
            DataTable Dt_Convenios_Traslado_Dominio;

            Convenios_Traslado_Dominio.P_Campos_Foraneos = true;
            //if (Boton_Pulsado == "Btn_Buscar")
            //{
            //    Convenios_Traslado_Dominio.P_No_Convenio = Hdf_No_Convenio.Value;
            //}
            //else
            //{
            //    Convenios_Traslado_Dominio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            //}
            if (Hdf_No_Convenio.Value != "")
            {
                Convenios_Traslado_Dominio.P_No_Convenio = Hdf_No_Convenio.Value;
            }
            else
            {
                if (Hdf_Cuenta_Predial_ID.Value != "")
                {
                    Convenios_Traslado_Dominio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                }
            }
            if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[8].Text.Replace("&nbsp;", "").Equals(""))
            {
                Convenios_Traslado_Dominio.P_Mostrar_Convenio_Con_Reestructura = false;
            }
            else
            {
                Convenios_Traslado_Dominio.P_Mostrar_Convenio_Con_Reestructura = true;
            }
            if (Hdf_No_Convenio.Value != "" || Hdf_Cuenta_Predial_ID.Value != "")
            {
                //Convenios_Traslado_Dominio.P_Estatus = "ACTIVO";
                Convenios_Traslado_Dominio.P_Mostrar_Ultimo_Convenio = true;
                Dt_Convenios_Traslado_Dominio = Convenios_Traslado_Dominio.Consultar_Convenio_Traslado_Dominio();

                if (Dt_Convenios_Traslado_Dominio != null)
                {
                    if (Dt_Convenios_Traslado_Dominio.Rows.Count > 0)
                    {
                        foreach (DataRow Row in Dt_Convenios_Traslado_Dominio.Rows)
                        {
                            Hdf_Cuenta_Predial_ID.Value = Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID].ToString();
                            Hdf_Propietario_ID.Value = Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Propietario_ID].ToString();
                            Txt_Cuenta_Predial.Text = Row["Cuenta_Predial"].ToString();
                            //Txt_Clasificacion.Text = Row["Tipo_Predio"].ToString();
                            Consultar_Datos_Cuenta_Predial();
                            Txt_Numero_Convenio.Text = Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio].ToString();
                            Txt_Numero_Reestructura.Text = Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Reestructura].ToString();
                            Cmb_Estatus.SelectedValue = Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus].ToString();
                            if (Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Propietario_ID] != null
                                && Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Propietario_ID].ToString() != "")
                            {
                                Txt_Solicitante.Text = Row["Nombre_Propietario"].ToString();
                                Cmb_Tipo_Solicitante.SelectedIndex = 0;
                            }
                            else
                            {
                                if (Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Solicitante].ToString() != "")
                                {
                                    Txt_Solicitante.Text = Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Solicitante].ToString();
                                    Txt_RFC.Text = Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_RFC].ToString();
                                    Cmb_Tipo_Solicitante.SelectedIndex = 1;
                                }
                            }
                            Txt_Numero_Parcialidades.Text = Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Numero_Parcialidades].ToString();
                            Cmb_Periodicidad_Pago.SelectedValue = Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Periodicidad_Pago].ToString();
                            Txt_Realizo.Text = Row["Nombre_Realizo"].ToString();
                            Txt_Fecha.Text = Convert.ToDateTime(Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Fecha].ToString()).ToString("dd/MMM/yyyy");
                            Txt_Observaciones.Text = Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Observaciones].ToString();
                            Txt_Descuento_Recargos_Ordinarios.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Recargos_Ordinarios]).ToString();
                            Hdf_Desc_Recargos.Value = Convert.ToDouble(Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Recargos_Ordinarios]).ToString();
                            Txt_Descuento_Recargos_Moratorios.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Recargos_Moratorios]).ToString();
                            Txt_Descuento_Multas.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Multas]).ToString();
                            Hdf_Desc_Multas.Value = Convert.ToDouble(Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Multas]).ToString();
                            Txt_Total_Adeudo.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Adeudo]).ToString();
                            Txt_Total_Descuento.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Descuento]).ToString();
                            Txt_Sub_Total.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Sub_Total]).ToString("0.00");
                            Txt_Porcentaje_Anticipo.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Porcentaje_Anticipo]).ToString("0.00");
                            Txt_Total_Anticipo.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Anticipo]).ToString("0.00");
                            Txt_Total_Convenio.Text = Row[Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Convenio].ToString();

                            Grid_Parcialidades.DataSource = Convenios_Traslado_Dominio.P_Dt_Parcialidades;
                            Grid_Parcialidades.PageIndex = 0;
                            Grid_Parcialidades.DataBind();

                            Grid_Pagos.DataSource = Convenios_Traslado_Dominio.P_Dt_Parcialidades_Pagadas;
                            Grid_Pagos.PageIndex = 0;
                            Grid_Pagos.DataBind();
                            if (Cmb_Estatus.SelectedValue == "ACTIVO")
                            {
                                Cambiar_Convenio_Incumplido();
                            }
                            try
                            {
                                Sumar_Totales_Parcialidades();
                                Sumar_Totales_Parcialidades_Pagadas();
                            }
                            catch
                            { }
                        }
                    }
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

    private void Sumar_Totales_Parcialidades()
    {
        Double Total_Honorarios = 0;
        Double Total_Multas = 0;
        Double Total_Constancia = 0;
        Double Total_Recargos_Ordinarios = 0;
        Double Total_Recargos_Moratorios = 0;
        Double Total_Impuesto = 0;
        Double Total_Impuesto_Division = 0;
        Double Total_Importe = 0;

        foreach (GridViewRow Fila_Grid in Grid_Parcialidades.Rows)
        {
            if (Fila_Grid.Cells[1].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Honorarios += Convert.ToDouble(Fila_Grid.Cells[1].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[2].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Constancia += Convert.ToDouble(Fila_Grid.Cells[2].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[3].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Multas += Convert.ToDouble(Fila_Grid.Cells[3].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[4].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Recargos_Ordinarios += Convert.ToDouble(Fila_Grid.Cells[4].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[5].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Recargos_Moratorios += Convert.ToDouble(Fila_Grid.Cells[5].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[6].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Impuesto += Convert.ToDouble(Fila_Grid.Cells[6].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[7].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Impuesto_Division += Convert.ToDouble(Fila_Grid.Cells[7].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[8].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Importe += Convert.ToDouble(Fila_Grid.Cells[8].Text.Replace("$", ""));
            }
        }

        Grid_Parcialidades.FooterRow.Cells[1].Text = Total_Honorarios.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[2].Text = Total_Constancia.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[3].Text = Total_Multas.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[4].Text = Total_Recargos_Ordinarios.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[5].Text = Total_Recargos_Moratorios.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[6].Text = Total_Impuesto.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[7].Text = Total_Impuesto_Division.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[8].Text = Total_Importe.ToString("$###,###,##0.00");
    }

    private void Sumar_Totales_Parcialidades_Pagadas()
    {
        Double Total_Honorarios = 0;
        Double Total_Multas = 0;
        Double Total_Constancia = 0;
        Double Total_Recargos_Ordinarios = 0;
        Double Total_Recargos_Moratorios = 0;
        Double Total_Impuesto = 0;
        Double Total_Impuesto_Division = 0;
        Double Total_Importe = 0;

        foreach (GridViewRow Fila_Grid in Grid_Pagos.Rows)
        {
            if (Fila_Grid.Cells[1].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Honorarios += Convert.ToDouble(Fila_Grid.Cells[1].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[2].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Constancia += Convert.ToDouble(Fila_Grid.Cells[2].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[3].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Multas += Convert.ToDouble(Fila_Grid.Cells[3].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[4].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Recargos_Ordinarios += Convert.ToDouble(Fila_Grid.Cells[4].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[5].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Recargos_Moratorios += Convert.ToDouble(Fila_Grid.Cells[5].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[6].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Impuesto += Convert.ToDouble(Fila_Grid.Cells[6].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[7].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Impuesto_Division += Convert.ToDouble(Fila_Grid.Cells[7].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[8].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Importe += Convert.ToDouble(Fila_Grid.Cells[8].Text.Replace("$", ""));
            }
        }

        Grid_Pagos.FooterRow.Cells[1].Text = Total_Honorarios.ToString("$###,###,##0.00");
        Grid_Pagos.FooterRow.Cells[2].Text = Total_Constancia.ToString("$###,###,##0.00");
        Grid_Pagos.FooterRow.Cells[3].Text = Total_Multas.ToString("$###,###,##0.00");
        Grid_Pagos.FooterRow.Cells[4].Text = Total_Recargos_Ordinarios.ToString("$###,###,##0.00");
        Grid_Pagos.FooterRow.Cells[5].Text = Total_Recargos_Moratorios.ToString("$###,###,##0.00");
        Grid_Pagos.FooterRow.Cells[6].Text = Total_Impuesto.ToString("$###,###,##0.00");
        Grid_Pagos.FooterRow.Cells[7].Text = Total_Impuesto_Division.ToString("$###,###,##0.00");
        Grid_Pagos.FooterRow.Cells[8].Text = Total_Importe.ToString("$###,###,##0.00");
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Consultar_Datos_Adeudo
    /////DESCRIPCIÓN          : Realiza la búsqueda de los adeudos
    /////PARAMETROS:     
    /////CREO                 : Miguel Angel Bedolla Moreno
    /////FECHA_CREO           : 13/Septiembre/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Consultar_Datos_Adeudo()
    //{
    //    Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenios = new Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio();
    //    DataTable Dt_Adeudos;
    //    Convenios.P_No_Convenio = Hdf_No_Convenio.Value;
    //    Double Sum_Impuestos = 0;
    //    Double Sum_Recargos_Ordinarios = 0;
    //    Double Sum_Recargos_Moratorios = 0;
    //    Double Sum_Totales = 0;
    //    Double Sum_Multas = 0;
    //    Double Sum_Honorarios = 0;
    //    if (Grid_Convenios_Impuestos_Fraccionamientos.SelectedRow.Cells[9].Text.Equals("&nbsp;"))
    //    {
    //        Convenios.P_Mostrar_Detalles_Con_Reestructura = false;
    //    }
    //    else
    //    {
    //        Convenios.P_Mostrar_Detalles_Con_Reestructura = true;
    //    }
    //    Dt_Adeudos = Convenios.Consultar_Adeudos_Derecho_Supervisions();
    //    Dt_Adeudos = Convenios.P_Dt_Parcialidades;
    //    foreach (DataRow Row in Dt_Adeudos.Rows)
    //    {
    //        //if (Row[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Importe] != null
    //        //    && Row[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Importe].ToString() != "")
    //        //{
    //        //    Sum_Importes += Convert.ToDouble(Row[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Importe].ToString());
    //        //}
    //        //if (Row[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Recargos] != null
    //        //    && Row[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Recargos].ToString() != "")
    //        //{
    //        //    Sum_Recargos += Convert.ToDouble(Row[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Recargos].ToString());
    //        //}
    //        //if (Row[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Total] != null
    //        //    && Row[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Total].ToString() != "")
    //        //{
    //        //    Sum_Totales += Convert.ToDouble(Row[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Total].ToString());
    //        //}
    //        if (Row[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto] != null
    //                && Row[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto].ToString() != "")
    //        {
    //            Sum_Impuestos += Convert.ToDouble(Row[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto].ToString());
    //        }
    //        else
    //        {
    //            Sum_Impuestos += 0.00;
    //        }
    //        if (Row[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios] != null
    //            && Row[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios].ToString() != "")
    //        {
    //            Sum_Recargos_Ordinarios += Convert.ToDouble(Row[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios].ToString());
    //        }
    //        else
    //        {
    //            Sum_Recargos_Ordinarios += 0.00;
    //        }
    //        if (Row[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios] != null
    //            && Row[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios].ToString() != "")
    //        {
    //            Sum_Recargos_Moratorios += Convert.ToDouble(Row[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios].ToString());
    //        }
    //        else
    //        {
    //            Sum_Recargos_Moratorios += 0.00;
    //        }
    //        if (Row["HONORARIOS"] != null
    //            && Row["HONORARIOS"].ToString() != "")
    //        {
    //            Sum_Honorarios += Convert.ToDouble(Row["HONORARIOS"].ToString());
    //        }
    //        else
    //        {
    //            Sum_Honorarios += 0.00;
    //        }
    //        if (Row[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas] != null
    //            && Row[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas].ToString() != "")
    //        {
    //            Sum_Multas += Convert.ToDouble(Row[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas].ToString());
    //        }
    //        else
    //        {
    //            Sum_Multas += 0.00;
    //        }
    //    }

    //    Txt_Monto_Impuesto.Text = Sum_Impuestos.ToString("0.00");
    //    Txt_Monto_Recargos.Text = Sum_Recargos_Ordinarios.ToString("0.00");
    //    Txt_Monto_Multas.Text = Sum_Multas.ToString("0.00");
    //    //Txt_Honorarios.Text = Sum_Honorarios.ToString("0.00");
    //    Txt_Recargos_Moratorios.Text = Sum_Recargos_Moratorios.ToString("0.00");

    //    Calcular_Recargos_Moratorios();
    //    Calcular_Total_Adeudos();
    //    //Calcular_Total_Adeudos();
    //    Calcular_Total_Descuento();
    //    Calcular_Total_Convenio();
    //    Calcular_Sub_Total();
    //    //Calcular_Parcialidades();
    //    //Calcular_Total_Anticipo();

    //    //DataTable Dt_Propietarios;
    //    //Cls_Cat_Pre_Propietarios_Negocio Propietarios = new Cls_Cat_Pre_Propietarios_Negocio();
    //    ////Consulta los Propietarios de la Cuenta Predial
    //    //Propietarios.P_Campos_Dinamicos = Cat_Pre_Propietarios.Campo_Contribuyente_ID;
    //    //Propietarios.P_Cuenta_Predial_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
    //    //Propietarios.P_Propietario_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Propietario_ID].ToString();
    //    ////Propietarios.P_Tipo = "PROPIETARIO";
    //    //Dt_Propietarios = Propietarios.Consultar_Propietario();
    //    //if (Dt_Propietarios.Rows.Count > 0)
    //    //{
    //    //    DataTable Dt_Contribuyentes;
    //    //    Cls_Cat_Pre_Contribuyentes_Negocio Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();
    //    //    //Consulta los datos del Contribuyente
    //    //    Contribuyentes.P_Contribuyente_ID = Dt_Propietarios.Rows[0][Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString();
    //    //    Dt_Contribuyentes = Contribuyentes.Consultar_Contribuyentes();
    //    //    if (Dt_Contribuyentes.Rows.Count > 0)
    //    //    {
    //    //        if (Propietarios.P_Propietario_ID != "")
    //    //        {
    //    //            Hdf_Propietario_ID.Value = Propietarios.P_Propietario_ID;
    //    //        }
    //    //        else
    //    //        {
    //    //            Hdf_Propietario_ID.Value = Contribuyentes.P_Contribuyente_ID;
    //    //        }
    //    //        Hdf_Cuenta_Predial_ID.Value = Propietarios.P_Cuenta_Predial_ID;
    //    //        Txt_Propietario.Text = Dt_Contribuyentes.Rows[0]["NOMBRE_COMPLETO"].ToString();
    //    //        Txt_Domicilio.Text = Dt_Contribuyentes.Rows[0]["DOMICILIO"].ToString();
    //    //    }
    //    //}
    //    //Txt_Solicitante.Enabled = false;
    //    //Txt_RFC.Enabled = false;

    //    //if (Boton_Pulsado != "Btn_Buscar")
    //    //{
    //    //    Cargar_Convenio();
    //    //}
    //}


    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Hdf_Cuenta_Predial_ID.Value.Trim() == "" && Txt_Cuenta_Predial.Text != "")
        {
            Consultar_Datos_Cuenta_Predial();
        }
        if (Txt_Cuenta_Predial.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Indique la Cuenta Predial.";
            Validacion = false;
        }
        if (Txt_Numero_Parcialidades.Text.Trim() == "")
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introduzca el Número de Parcialidades.";
            Validacion = false;
        }
        if (Cmb_Periodicidad_Pago.SelectedIndex <= 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccione un Periodo de Pago.";
            Validacion = false;
        }
        if (Cmb_Estatus.SelectedIndex <= 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Indique un Estatus.";
            Validacion = false;
        }
        //if (Btn_Nuevo.AlternateText != "Dar de Alta" && Btn_Modificar.AlternateText == "Actualizar")
        //{
        //    if (Txt_Observaciones.Text.Equals(""))
        //    {
        //        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //        Mensaje_Error = Mensaje_Error + "+ Introduzca las Observaciones.";
        //        Validacion = false;
        //    }
        //}
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #endregion

    #region Grids

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Grid_Contrarecibos_Pendientes_PageIndexChanging
    /////DESCRIPCIÓN          : Maneja la paginación del GridView
    /////PARAMETROS:     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 15/Agosto/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Grid_Contrarecibos_Pendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    try
    //    {
    //        Grid_Contrarecibos_Pendientes.SelectedIndex = (-1);
    //        Cargar_Grid_Contrarecibos_Pendientes(e.NewPageIndex);
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Grid_Contrarecibos_Pendientes_SelectedIndexChanged
    /////DESCRIPCIÓN          : Maneja la selección de fila en el GridView
    /////PARAMETROS:     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 15/Agosto/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Grid_Contrarecibos_Pendientes_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (Grid_Contrarecibos_Pendientes.Rows.Count > 0)
    //    {
    //        Hdf_Cuenta_Predial_ID.Value = Grid_Contrarecibos_Pendientes.DataKeys[Grid_Contrarecibos_Pendientes.SelectedIndex].Value.ToString();
    //        No_Limpiar_Campos_Clave = true;
    //        Limpiar_Controles();
    //        No_Limpiar_Campos_Clave = false;
    //        Cargar_Convenio();
    //    }
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Fraccionamientos_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Convenios_Impuestos_Traslados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Convenios_Impuestos_Traslado.SelectedIndex = (-1);
            Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Fraccionamientos_SelectedIndexChanged
    ///DESCRIPCIÓN          : Maneja la selección de fila en el GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Convenios_Impuestos_Traslados_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Convenios_Impuestos_Traslado.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_ID.Value = Grid_Convenios_Impuestos_Traslado.DataKeys[Grid_Convenios_Impuestos_Traslado.SelectedIndex].Value.ToString();
            Txt_Imp_Div.Text = "0";
            Hdf_No_Convenio.Value = Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[1].Text;
            Txt_Numero_Convenio.Text = Hdf_No_Convenio.Value;
            Txt_Cuenta_Predial.Text = Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[3].Text;
            Hdf_No_Calculo.Value = Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[10].Text;
            Hdf_Anio.Value = Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[11].Text;
            Cargar_Convenio();
            Txt_Imp_Div.Text = "0";
            Consultar_Adeudos_Convenio(false);
            Txt_Cuenta_Predial_TextChanged();
            Grid_Convenios_Impuestos_Traslado.Visible = false;
            Panel_Datos.Visible = true;
            Btn_Salir.AlternateText = "Atras";
            DataTable Dt_Calculo = Calcular_Impuestos();
            Cargar_Ventana_Emergente_Resolucion();
        }
        Cargar_Ventana_Emergente_Resumen_Predio();
    }

    private DataTable Calcular_Impuestos()
    {
        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Convenio = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
        if (Grid_Convenios_Impuestos_Traslado.SelectedIndex > -1)
        {
            Convenio.P_No_Calculo = HttpUtility.HtmlDecode(Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[10].Text).Trim();
        }
        else
        {
            Convenio.P_No_Calculo = HttpUtility.HtmlDecode(Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[10].Text).Trim();
        }
        DataTable Dt_Impuestos = Convenio.Consulta_Calculos_Convenio();

        if (Dt_Impuestos.Rows.Count > 0)
        {
            Double Monto_Total_Adeudo = Convert.ToDouble(Txt_Monto_Impuesto.Text);
            Double Monto_Impuesto = 0;
            Double Imp_Div = 0;
            Double Total_Impuestos = 0;
            Double Adeudo;

            Monto_Impuesto = Convert.ToDouble(Dt_Impuestos.Rows[0]["MONTO_TRASLADO"].ToString());
            Imp_Div = Convert.ToDouble(Dt_Impuestos.Rows[0]["MONTO_DIVISION"].ToString());
            Total_Impuestos = Monto_Impuesto + Imp_Div;
            Adeudo = Total_Impuestos - Monto_Total_Adeudo;

            if (Adeudo == 0)
            {
                Txt_Monto_Impuesto.Text = Monto_Impuesto.ToString("###,###,###,##0.00");
                Txt_Imp_Div.Text = Imp_Div.ToString("###,###,###,##0.00");
            }
            else
            {
                if (Adeudo <= Monto_Impuesto)
                {
                    Txt_Monto_Impuesto.Text = (Monto_Impuesto - Adeudo).ToString("###,###,###,##0.00");
                    Adeudo = 0;
                }
                else
                {
                    Txt_Monto_Impuesto.Text = "0.00";
                    Adeudo = Adeudo - Monto_Impuesto;
                }

                if (Adeudo <= Imp_Div)
                {
                    Txt_Imp_Div.Text = (Imp_Div - Adeudo).ToString("###,###,###,##0.00");
                    Adeudo = 0;
                }
                else
                {
                    Txt_Imp_Div.Text = "0.00";
                    Adeudo = Adeudo - Imp_Div;
                }
            }
            Txt_Realizo_Calculo.Text = Dt_Impuestos.Rows[0]["REALIZO_CALCULO"].ToString();
            Txt_Estatus_Calculo.Text = Dt_Impuestos.Rows[0]["ESTATUS"].ToString();
            Txt_Fecha_Calculo.Text = Convert.ToDateTime(Dt_Impuestos.Rows[0]["FECHA_CALCULO"]).ToString("dd/MMM/yyyy");
        }
        return Dt_Impuestos;
    }

    protected void Cmb_Estatus_Selected_Index_Changed(object sender, EventArgs e)
    {
        if (Btn_Modificar.AlternateText == "Actualizar" && Cmb_Estatus.SelectedValue == "ACTIVO")
        {
            Cargar_Descuentos();
        }
        else
        {
            Txt_Descuento_Multas.Text = Hdf_Desc_Multas.Value;
            Txt_Descuento_Recargos_Ordinarios.Text = Hdf_Desc_Recargos.Value;
            Calcular_Total_Descuento();
            Calcular_Sub_Total();
            Calcular_Total_Anticipo();
            Calcular_Total_Convenio();
            Calcular_Parcialidades();
        }
    }

    private void Cargar_Descuentos()
    {
        Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenio_Descuentos = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
        Convenio_Descuentos.P_No_Calculo = Hdf_No_Calculo.Value;
        Convenio_Descuentos.P_Anio = Hdf_Anio.Value;
        DataTable Dt_Descuentos = Convenio_Descuentos.Consultar_Descuentos();
        if (Dt_Descuentos.Rows.Count > 0)
        {
            Txt_Descuento_Multas.Text = Convert.ToDouble(Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Der_Sup.Campo_Desc_Multa].ToString()).ToString("0.00");
            Txt_Descuento_Recargos_Ordinarios.Text = Convert.ToDouble(Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Der_Sup.Campo_Desc_Recargo].ToString()).ToString("0.00");
            Hdf_No_Descuento.Value = Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Der_Sup.Campo_No_Descuento].ToString();
        }
        else
        {
            Txt_Descuento_Multas.Text = "0.00";
            Txt_Descuento_Recargos_Ordinarios.Text = "0.00";
        }
        Txt_Descuento_Recargos_Moratorios.Text = "0.00";
        Calcular_Total_Descuento();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Adeudos_Convenio
    ///DESCRIPCIÓN          : Consulta los adeudos del convenio
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 29/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Consultar_Adeudos_Convenio(Boolean Total_O_A_Pagar)
    {
        try
        {
            DataTable Dt_Adeudos = new DataTable();
            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
            Convenio.P_No_Convenio = Hdf_No_Convenio.Value;
            if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[8].Text.Replace("&nbsp;", "").Equals(""))
            {
                Convenio.P_Reestructura = false;
            }
            else
            {
                Convenio.P_Reestructura = true;
                Convenio.P_Reestructura = true;
            }
            Dt_Adeudos = Convenio.Consultar_Adeudos_Convenio(Total_O_A_Pagar);
            if (Dt_Adeudos.Rows.Count > 0)
            {
                Txt_Monto_Impuesto.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_IMPUESTO"].ToString()).ToString("0.00");
                Txt_Imp_Div.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_IMPUESTO_DIVISION"].ToString()).ToString("0.00");
                Txt_Monto_Multas.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_MULTAS"].ToString()).ToString("0.00");
                Txt_Monto_Recargos.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_ORDINARIOS"].ToString()).ToString("0.00");
                Txt_Recargos_Moratorios.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_MORATORIOS"].ToString()).ToString("0.00");
                if (!Dt_Adeudos.Rows[0]["TOTAL_CONSTANCIA"].ToString().Equals(""))
                    Txt_Costo_Constancia.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_CONSTANCIA"].ToString()).ToString("0.00");
                Calcular_Recargos_Moratorios();
                Calcular_Total_Adeudos();
                Calcular_Total_Descuento();
                Calcular_Sub_Total();
                Calcular_Total_Convenio();
                //Calcular_Parcialidades();
                Calcular_Total_Anticipo();
            }
            else
            {
                Txt_Monto_Impuesto.Text = "0.00";
                Txt_Imp_Div.Text = "0.00";
                Txt_Monto_Multas.Text = "0.00";
                Txt_Monto_Recargos.Text = "0.00";
                Txt_Costo_Constancia.Text = "0.00";
                Calcular_Recargos_Moratorios();
                Calcular_Total_Adeudos();
                Calcular_Total_Descuento();
                Calcular_Sub_Total();
                Calcular_Total_Convenio();
                //Calcular_Parcialidades();
                Calcular_Total_Anticipo();
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Convenios_Traslado_Dominio
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Convenios_Impuestos_Traslado.SelectedIndex > -1)
        {
            try
            {
                if (!Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("PAGADO") && !Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("CANCELADO") && Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("INCUMPLIDO"))
                {
                    Boton_Pulsado = ((ImageButton)sender).ID;
                    Session["Boton_Pulsado"] = Boton_Pulsado;
                    if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                    {
                        Configuracion_Formulario(false);
                        if (Grid_Convenios_Impuestos_Traslado.SelectedIndex == -1)
                        {
                            Limpiar_Controles();
                        }
                        Btn_Nuevo.AlternateText = "Dar de Alta";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Modificar.Visible = false;
                        Btn_Imprimir.Visible = false;
                        Consultar_Adeudos_Convenio(true);
                        Calcular_Impuestos();
                        Txt_Realizo.Text = Cls_Sessiones.Nombre_Empleado;
                        Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                        Cmb_Estatus.SelectedIndex = 1;
                        Grid_Convenios_Impuestos_Traslado.Visible = false;
                        Panel_Datos.Visible = true;
                        Txt_Descuento_Multas.Text = "0";
                        Txt_Descuento_Recargos_Moratorios.Text = "0";
                        Txt_Descuento_Recargos_Ordinarios.Text = "0";
                        Calcular_Total_Adeudos();
                        Calcular_Total_Descuento();
                        Calcular_Sub_Total();
                        Calcular_Total_Anticipo();
                        Calcular_Total_Convenio();
                        Calcular_Parcialidades();
                    }
                    else
                    {
                        if (Validar_Componentes())
                        {
                            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                            Convenios_Traslado_Dominio.P_No_Convenio = Txt_Numero_Convenio.Text;
                            Convenios_Traslado_Dominio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                            if (Hdf_Propietario_ID.Value != "")
                            {
                                Convenios_Traslado_Dominio.P_Propietario_ID = Hdf_Propietario_ID.Value;
                            }
                            else
                            {
                                if (Cmb_Tipo_Solicitante.SelectedIndex == 1)
                                {
                                    Convenios_Traslado_Dominio.P_Solicitante = Txt_Solicitante.Text.Trim();
                                    Convenios_Traslado_Dominio.P_RFC = Txt_RFC.Text.Trim();
                                }
                            }
                            Convenios_Traslado_Dominio.P_Realizo = Cls_Sessiones.Empleado_ID;
                            Convenios_Traslado_Dominio.P_Estatus = Cmb_Estatus.SelectedValue;
                            Convenios_Traslado_Dominio.P_Numero_Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);
                            Convenios_Traslado_Dominio.P_Periodicidad_Pago = Cmb_Periodicidad_Pago.SelectedValue;
                            Convenios_Traslado_Dominio.P_Fecha = Convert.ToDateTime(Txt_Fecha.Text);
                            Convenios_Traslado_Dominio.P_Observaciones = Txt_Observaciones.Text;
                            Convenios_Traslado_Dominio.P_Descuento_Recargos_Ordinarios = Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text);
                            Convenios_Traslado_Dominio.P_Descuento_Recargos_Moratorios = Convert.ToDouble(Txt_Descuento_Recargos_Moratorios.Text);
                            Convenios_Traslado_Dominio.P_Descuento_Multas = Convert.ToDouble(Txt_Descuento_Multas.Text);
                            Convenios_Traslado_Dominio.P_Total_Adeudo = Convert.ToDouble(Txt_Total_Adeudo.Text);
                            Convenios_Traslado_Dominio.P_Total_Descuento = Convert.ToDouble(Txt_Total_Descuento.Text);
                            Convenios_Traslado_Dominio.P_Sub_Total = Convert.ToDouble(Txt_Sub_Total.Text);
                            Convenios_Traslado_Dominio.P_Porcentaje_Anticipo = Convert.ToDouble(Txt_Porcentaje_Anticipo.Text);
                            Convenios_Traslado_Dominio.P_Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text);
                            Convenios_Traslado_Dominio.P_Total_Convenio = Convert.ToDouble(Txt_Total_Convenio.Text);
                            Convenios_Traslado_Dominio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                            Convenios_Traslado_Dominio.P_Dt_Parcialidades = Crear_Tabla_Parcialidades();
                            if (Convenios_Traslado_Dominio.Alta_Reestructura_Convenio_Traslado_Dominio())
                            {
                                //Insertar_Pasivo("RTRA" + Convenios_Traslado_Dominio.P_No_Convenio + Convenios_Traslado_Dominio.P_No_Reestructura);
                                //Imprimir_Reporte(Crear_Ds_Convenios_Traslado(), "Rpt_Convenio_TD.rpt", "Rpt_Convenio_Traslado_Dominio", "Window_Frm", "Convenio");
                                Imprimir_Convenio();
                                Imprimir_Reporte(Crear_Ds_Convenios_Folio("CTRA" + Convert.ToInt32(Convenios_Traslado_Dominio.P_No_Convenio)), "Rpt_Pre_Folio_Convenios.rpt", "Rpt_Reestructura_Convenio_Traslado", "Window_Rpt", "Folio");
                                Configuracion_Formulario(true);
                                Limpiar_Controles();
                                Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(0);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('Alta de Reestructura de Convenio por Traslado de Dominio Exitosa');", true);
                                Btn_Nuevo.AlternateText = "Nuevo";
                                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                                Btn_Modificar.Visible = true;
                                Btn_Imprimir.Visible = true;
                                Btn_Salir.AlternateText = "Salir";
                                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                                Grid_Convenios_Impuestos_Traslado.Visible = true;
                                Panel_Datos.Visible = false;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('Alta de Reestructura de Convenios por Traslado de Dominio No fue Exitosa');", true);
                            }
                        }
                    }
                }
                else if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("CANCELADO"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('El convenio ó reestructura se encuentra cancelado(a).');", true);
                }
                else if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("PAGADO"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('El convenio ó reestructura se encuentra pagado(a).');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('El convenio ó reestructura aún se encuentra vigente.');", true);
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('Seleccione un elemento de la tabla.');", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Convenios_Traslado_Dominio.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Convenios_Impuestos_Traslado.SelectedIndex > -1)
        {
            if (!Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("PAGADO") && !Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[8].Text.Replace("&nbsp;", "").Equals("") && !Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("CANCELADO") && !Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("INCUMPLIDO"))
            {
                try
                {
                    Boton_Pulsado = ((ImageButton)sender).ID;
                    Session["Boton_Pulsado"] = Boton_Pulsado;
                    if (Btn_Modificar.AlternateText.Equals("Modificar"))
                    {
                        if (Txt_Numero_Convenio.Text.Trim() != "")
                        {
                            Btn_Modificar.AlternateText = "Actualizar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                            Btn_Salir.AlternateText = "Cancelar";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Nuevo.Visible = false;
                            Btn_Imprimir.Visible = false;
                            Configuracion_Formulario(false);
                            Grid_Convenios_Impuestos_Traslado.Visible = false;
                            Panel_Datos.Visible = true;
                        }
                        else
                        {
                            Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                    else
                    {
                        if (Validar_Componentes())
                        {
                            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                            Convenios_Traslado_Dominio.P_No_Convenio = Txt_Numero_Convenio.Text;
                            Convenios_Traslado_Dominio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                            if (Hdf_Propietario_ID.Value != "")
                            {
                                Convenios_Traslado_Dominio.P_Propietario_ID = Hdf_Propietario_ID.Value;
                            }
                            else
                            {
                                if (Cmb_Tipo_Solicitante.SelectedIndex == 1)
                                {
                                    Convenios_Traslado_Dominio.P_Solicitante = Txt_Solicitante.Text.Trim();
                                    Convenios_Traslado_Dominio.P_RFC = Txt_RFC.Text.Trim();
                                }
                            }
                            Convenios_Traslado_Dominio.P_Realizo = Cls_Sessiones.Empleado_ID;
                            Convenios_Traslado_Dominio.P_Estatus = Cmb_Estatus.SelectedValue;
                            Convenios_Traslado_Dominio.P_Numero_Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);
                            Convenios_Traslado_Dominio.P_Periodicidad_Pago = Cmb_Periodicidad_Pago.SelectedValue;
                            Convenios_Traslado_Dominio.P_Fecha = Convert.ToDateTime(Txt_Fecha.Text);
                            Convenios_Traslado_Dominio.P_Observaciones = Txt_Observaciones.Text;
                            Convenios_Traslado_Dominio.P_Descuento_Recargos_Ordinarios = Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text);
                            Convenios_Traslado_Dominio.P_Descuento_Recargos_Moratorios = Convert.ToDouble(Txt_Descuento_Recargos_Moratorios.Text);
                            Convenios_Traslado_Dominio.P_Descuento_Multas = Convert.ToDouble(Txt_Descuento_Multas.Text);
                            Convenios_Traslado_Dominio.P_Total_Adeudo = Convert.ToDouble(Txt_Total_Adeudo.Text);
                            Convenios_Traslado_Dominio.P_Total_Descuento = Convert.ToDouble(Txt_Total_Descuento.Text);
                            Convenios_Traslado_Dominio.P_Sub_Total = Convert.ToDouble(Txt_Sub_Total.Text);
                            Convenios_Traslado_Dominio.P_Porcentaje_Anticipo = Convert.ToDouble(Txt_Porcentaje_Anticipo.Text);
                            Convenios_Traslado_Dominio.P_Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text);
                            Convenios_Traslado_Dominio.P_Total_Convenio = Convert.ToDouble(Txt_Total_Convenio.Text);
                            Convenios_Traslado_Dominio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                            Convenios_Traslado_Dominio.P_No_Reestructura = Txt_Numero_Reestructura.Text.Trim();
                            Convenios_Traslado_Dominio.P_Dt_Parcialidades = Crear_Tabla_Parcialidades();
                            Convenios_Traslado_Dominio.P_No_Descuento = Hdf_No_Descuento.Value;
                            if (Convenios_Traslado_Dominio.Modificar_Reestructura_Convenio_Traslado_Dominio())
                            {
                                Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Mod_Pasivo = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
                                Mod_Pasivo.Cancelar_Pasivo("RTRA" + Convert.ToInt32(Convenios_Traslado_Dominio.P_No_Convenio), Cmb_Estatus.SelectedValue, Grid_Parcialidades.Rows[0].Cells[7].Text.Replace("$", ""));
                                //Convenios_Traslado_Dominio.Cancelar_Pasivo("RTRA" + Convenios_Traslado_Dominio.P_No_Convenio + Txt_Numero_Reestructura.Text, Txt_Total_Anticipo.Text);
                                if (Cmb_Estatus.SelectedValue == "ACTIVO")
                                {
                                    Imprimir_Reporte(Crear_Ds_Convenios_Folio("CTRA" + Convert.ToInt32(Convenios_Traslado_Dominio.P_No_Convenio)), "Rpt_Pre_Folio_Convenios.rpt", "Rpt_Reestructura_Convenio_Traslado", "Window_Rpt", "Folio");
                                    Imprimir_Convenio();
                                }
                                Limpiar_Controles();
                                Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(0);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('Actualización de Reestructura de Convenios por Traslado de Dominio Exitosa');", true);
                                Btn_Modificar.AlternateText = "Modificar";
                                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                                Btn_Nuevo.Visible = true;
                                Btn_Modificar.Visible = true;
                                Btn_Imprimir.Visible = true;
                                Btn_Salir.AlternateText = "Salir";
                                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                                Configuracion_Formulario(true);
                                Grid_Convenios_Impuestos_Traslado.Visible = true;
                                Panel_Datos.Visible = false;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('Actualización de Reestructura de Convenios por Traslado de Dominio No fue Exitosa');", true);
                            }
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
            else if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[8].Text.Replace("&nbsp;", "").Equals(""))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('No puede modificar este convenio');", true);
            }
            else if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("PAGADO"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('El convenio ó reestructura se encuentra pagado(a).');", true);
            }
            else if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("CANCELADO"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('El convenio ó reestructura se encuentra cancelado(a).');", true);
            }
            else if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("INCUMPLIDO"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('El convenio ó reestructura se encuentra incumplido, es necesario generar una nueva reestructura para continuar.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('El convenio ó reestructura aún se encuentra vigente.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Traslado de Dominio", "alert('Seleccione un elemento de la tabla.');", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Constancia_Propiedad_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Boton_Pulsado = ((ImageButton)sender).ID;
            Session["Boton_Pulsado"] = Boton_Pulsado;
            Limpiar_Controles();
            Grid_Convenios_Impuestos_Traslado.SelectedIndex = (-1);
            Grid_Parcialidades.SelectedIndex = (-1);
            Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(0);
            if (Grid_Convenios_Impuestos_Traslado.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda de \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                //Lbl_Mensaje_Error.Text = "(Se cargaron todos los Convenios por Fraccionamientos encontrados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                //Cargar_Convenio();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
    /////DESCRIPCIÓN          : Elimina un Convenios_Traslado_Dominio de la Base de Datos
    /////PARAMETROS          :     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 29/Julio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Eliminar_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Grid_Parcialidades.Rows.Count > 0 && Grid_Parcialidades.SelectedIndex > (-1))
    //        {
    //            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
    //            Convenios_Traslado_Dominio.P_Folio = Grid_Parcialidades.SelectedRow.Cells[3].Text;
    //            if (Convenios_Traslado_Dominio.Eliminar_Constancia_Propiedad())
    //            {
    //                Grid_Parcialidades.SelectedIndex = (-1);
    //                Llenar_Tabla_Constancias_Propiedad(Grid_Parcialidades.PageIndex);
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Fraccionamientos", "alert('Convenio por Fraccionamiento fue Eliminada Exitosamente');", true);
    //                Limpiar_Controles();
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Fraccionamientos", "alert('La Convenio por Fraccionamiento No fue Eliminada');", true);
    //            }
    //        }
    //        else
    //        {
    //            Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
    //            Lbl_Mensaje_Error.Text = "";
    //            Div_Contenedor_Msj_Error.Visible = true;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }

    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
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
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Imprimir.Visible = true;
            Configuracion_Formulario(true);
            Limpiar_Controles();
            Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(0);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Convenios_Impuestos_Traslado.Visible = true;
            Panel_Datos.Visible = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
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
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
            }
            Consultar_Datos_Cuenta_Predial();
            Cargar_Ventana_Emergente_Resumen_Predio();
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        //Session.Remove("CUENTA_PREDIAL");
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Constanica
    /////DESCRIPCIÓN          : Realiza la búsqueda de los datos de la cuenta predial introducida
    /////PARAMETROS:     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 29/Julio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Consultar_Datos_Cuenta_Predial()
    //{
    //    Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
    //    DataTable Dt_Cuentas_Predial;
    //    if (Txt_Cuenta_Predial.Text.Trim() != "")
    //    {
    //        //Consulta la Cuenta Predial
    //        Cuentas_Predial.P_Incluir_Campos_Foraneos = true;
    //        Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
    //        Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Propietario_ID;
    //        Cuentas_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
    //        Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
    //        if (Dt_Cuentas_Predial.Rows.Count > 0)
    //        {
    //            //Pone los datos de la cuenta y los Impuestos
    //            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Impuestos_Traslado_Tominio = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
    //            DataTable Dt_Impuestos_Derechos_Supervision;

    //            Impuestos_Traslado_Tominio.P_Cuenta_Predial = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
    //            Impuestos_Traslado_Tominio.P_Estatus = "LISTO";
    //            Dt_Impuestos_Derechos_Supervision = Impuestos_Traslado_Tominio.Consulta_Calculos_Contrarecibo();

    //            if (Dt_Impuestos_Derechos_Supervision.Rows.Count > 0)
    //            {
    //                Txt_Estatus_Calculo.Text = Dt_Impuestos_Derechos_Supervision.Rows[0]["ESTATUS_CALCULO"].ToString();
    //                if (Dt_Impuestos_Derechos_Supervision != null)
    //                {
    //                    if (Dt_Impuestos_Derechos_Supervision.Rows.Count > 0)
    //                    {
    //                        //Txt_Realizo_Calculo.Text = Dt_Impuestos_Derechos_Supervision.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Usuario_Creo].ToString();
    //                        Txt_Fecha_Calculo.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Impuestos_Derechos_Supervision.Rows[0]["FECHA_CALCULO"].ToString()));
    //                    }
    //                }

    //                Txt_Monto_Impuesto.Text = Dt_Impuestos_Derechos_Supervision.Rows[0]["MONTO_TOTAL_CALCULO"].ToString();
    //                Txt_Monto_Recargos.Text = Dt_Impuestos_Derechos_Supervision.Rows[0]["MONTO_RECARGOS"].ToString();
    //                Txt_Monto_Multas.Text = Dt_Impuestos_Derechos_Supervision.Rows[0]["MONTO_MULTA"].ToString();
    //                Txt_Realizo_Calculo.Text = Dt_Impuestos_Derechos_Supervision.Rows[0]["REALIZO_CALCULO"].ToString();


    //                if (Boton_Pulsado != "Btn_Buscar")
    //                {
    //                    Calcular_Total_Adeudos();
    //                    Calcular_Total_Descuento();
    //                    Calcular_Sub_Total();
    //                    Calcular_Total_Convenio();
    //                    Calcular_Parcialidades();
    //                    Calcular_Total_Anticipo();
    //                }

    //                //DataTable Dt_Propietarios;
    //                //Cls_Cat_Pre_Propietarios_Negocio Propietarios = new Cls_Cat_Pre_Propietarios_Negocio();
    //                ////Consulta los Propietarios de la Cuenta Predial
    //                //Propietarios.P_Campos_Dinamicos = Cat_Pre_Propietarios.Campo_Contribuyente_ID;
    //                //Propietarios.P_Cuenta_Predial_ID = Dt_Impuestos_Derechos_Supervision.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
    //                //Propietarios.P_Propietario_ID = Dt_Impuestos_Derechos_Supervision.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Propietario_ID].ToString();
    //                ////Propietarios.P_Tipo = "PROPIETARIO";
    //                //Dt_Propietarios = Propietarios.Consultar_Propietario();
    //                //if (Dt_Propietarios.Rows.Count > 0)
    //                //{
    //                //    DataTable Dt_Contribuyentes;
    //                //    Cls_Cat_Pre_Contribuyentes_Negocio Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();
    //                //    //Consulta los datos del Contribuyente
    //                //    Contribuyentes.P_Contribuyente_ID = Dt_Propietarios.Rows[0][Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString();
    //                //    Dt_Contribuyentes = Contribuyentes.Consultar_Contribuyentes();
    //                //    if (Dt_Contribuyentes.Rows.Count > 0)
    //                //    {
    //                //        if (Propietarios.P_Propietario_ID != "")
    //                //        {
    //                //            Hdf_Propietario_ID.Value = Propietarios.P_Propietario_ID;
    //                //        }
    //                //        else
    //                //        {
    //                //            Hdf_Propietario_ID.Value = Contribuyentes.P_Contribuyente_ID;
    //                //        }
    //                //        Hdf_Cuenta_Predial_ID.Value = Propietarios.P_Cuenta_Predial_ID;
    //                //        Txt_Propietario.Text = Dt_Contribuyentes.Rows[0]["NOMBRE_COMPLETO"].ToString();
    //                //        Txt_Domicilio.Text = Dt_Contribuyentes.Rows[0]["DOMICILIO"].ToString();
    //                //    }
    //                //}
    //            }
    //            //else
    //            //{
    //            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de Dominio", "alert('El número de Convenio está Activo');", true);
    //            //    if (Btn_Salir.AlternateText == "Cancelar")
    //            //    {
    //            //        Btn_Salir_Click(null, null);
    //            //    }
    //            //}
    //        }
    //    }
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Constanica
    ///DESCRIPCIÓN          : Realiza la búsqueda de los datos de la cuenta predial introducida
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Consultar_Datos_Cuenta_Predial()
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        DataTable Dt_Cuentas_Predial;
        if (Txt_Cuenta_Predial.Text.Trim() != "")
        {
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            //Consulta la Cuenta Predial
            Cuentas_Predial.P_Incluir_Campos_Foraneos = true;
            Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Propietario_ID;
            Cuentas_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            if (Dt_Cuentas_Predial.Rows.Count > 0)
            {

                //Pone los datos de la cuenta y los Impuestos
                Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Impuestos_Traslado_Tominio = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
                DataTable Dt_Impuestos_Derechos_Supervision;

                Impuestos_Traslado_Tominio.P_Cuenta_Predial_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                Impuestos_Traslado_Tominio.P_Estatus = "LISTO";
                Dt_Impuestos_Derechos_Supervision = Impuestos_Traslado_Tominio.Consulta_Calculos_Contrarecibo();

                if (Dt_Impuestos_Derechos_Supervision.Rows.Count > 0)
                {
                    Txt_Estatus_Calculo.Text = Dt_Impuestos_Derechos_Supervision.Rows[0]["ESTATUS_CALCULO"].ToString();
                    if (Dt_Impuestos_Derechos_Supervision != null)
                    {
                        if (Dt_Impuestos_Derechos_Supervision.Rows.Count > 0)
                        {
                            //Txt_Realizo_Calculo.Text = Dt_Impuestos_Derechos_Supervision.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Creo].ToString();
                            Txt_Fecha_Calculo.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Impuestos_Derechos_Supervision.Rows[0]["FECHA_CALCULO"].ToString()));
                        }
                    }

                    Txt_Monto_Impuesto.Text = Convert.ToDouble(Dt_Impuestos_Derechos_Supervision.Rows[0]["MONTO_TOTAL_CALCULO"].ToString()).ToString("0.00");
                    Txt_Monto_Recargos.Text = Convert.ToDouble(Dt_Impuestos_Derechos_Supervision.Rows[0]["MONTO_RECARGOS"].ToString()).ToString("0.00");
                    Txt_Monto_Multas.Text = Convert.ToDouble(Dt_Impuestos_Derechos_Supervision.Rows[0]["MONTO_MULTA"].ToString()).ToString("0.00");
                    Txt_Realizo_Calculo.Text = Dt_Impuestos_Derechos_Supervision.Rows[0]["REALIZO_CALCULO"].ToString();

                    if (Boton_Pulsado != "Btn_Buscar")
                    {
                        Calcular_Total_Adeudos();
                        Calcular_Total_Descuento();
                        Calcular_Sub_Total();
                        Calcular_Total_Convenio();
                        Calcular_Parcialidades();
                        Calcular_Total_Anticipo();
                    }

                    Calcular_Recargos_Moratorios();

                    //DataTable Dt_Propietarios;
                    //Cls_Cat_Pre_Propietarios_Negocio Propietarios = new Cls_Cat_Pre_Propietarios_Negocio();
                    ////Consulta los Propietarios de la Cuenta Predial
                    //Propietarios.P_Campos_Dinamicos = Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                    //Propietarios.P_Cuenta_Predial_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    //Propietarios.P_Propietario_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Propietario_ID].ToString();
                    ////Propietarios.P_Tipo = "PROPIETARIO";
                    //Dt_Propietarios = Propietarios.Consultar_Propietario();
                    //if (Dt_Propietarios.Rows.Count > 0)
                    //{
                    //    DataTable Dt_Contribuyentes;
                    //    Cls_Cat_Pre_Contribuyentes_Negocio Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();
                    //    //Consulta los datos del Contribuyente
                    //    Contribuyentes.P_Contribuyente_ID = Dt_Propietarios.Rows[0][Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString();
                    //    Dt_Contribuyentes = Contribuyentes.Consultar_Contribuyentes();
                    //    if (Dt_Contribuyentes.Rows.Count > 0)
                    //    {
                    //        if (Propietarios.P_Propietario_ID != "")
                    //        {
                    //            Hdf_Propietario_ID.Value = Propietarios.P_Propietario_ID;
                    //        }
                    //        else
                    //        {
                    //            Hdf_Propietario_ID.Value = Contribuyentes.P_Contribuyente_ID;
                    //        }
                    //        Hdf_Cuenta_Predial_ID.Value = Propietarios.P_Cuenta_Predial_ID;
                    //        Txt_Propietario.Text = Dt_Contribuyentes.Rows[0]["NOMBRE_COMPLETO"].ToString();
                    //        Txt_Domicilio.Text = Dt_Contribuyentes.Rows[0]["DOMICILIO"].ToString();
                    //    }
                    //}
                }
            }
        }
    }

    private double Calcular_Total_Adeudos()
    {
        Double Monto_Impuesto = 0;
        Double Monto_Recargos = 0;
        Double Monto_Moratorios = 0;
        Double Monto_Constancia = 0;
        Double Monto_Multas = 0;
        Double Total_Adeudo = 0;

        if (Txt_Monto_Impuesto.Text.Trim() != "")
        {
            Monto_Impuesto = Convert.ToDouble(Txt_Monto_Impuesto.Text) + Convert.ToDouble(Txt_Imp_Div.Text);
        }
        else
        {
            Txt_Monto_Impuesto.Text = "0.00";
        }
        if (Txt_Monto_Recargos.Text.Trim() != "")
        {
            Monto_Recargos = Convert.ToDouble(Txt_Monto_Recargos.Text);
        }
        else
        {
            Txt_Monto_Recargos.Text = "0.00";
        }
        if (Txt_Monto_Multas.Text.Trim() != "")
        {
            Monto_Multas = Convert.ToDouble(Txt_Monto_Multas.Text);
        }
        else
        {
            Txt_Monto_Multas.Text = "0.00";
        }
        if (Txt_Recargos_Moratorios.Text.Trim() != "")
        {
            Monto_Moratorios = Convert.ToDouble(Txt_Recargos_Moratorios.Text);
        }
        else
        {
            Txt_Recargos_Moratorios.Text = "0.00";
        }
        if (Txt_Costo_Constancia.Text.Trim() != "")
        {
            Monto_Constancia = Convert.ToDouble(Txt_Costo_Constancia.Text);
        }
        else
        {
            Txt_Costo_Constancia.Text = "0.00";
        }
        Total_Adeudo = Monto_Impuesto + Monto_Recargos + Monto_Multas + Monto_Moratorios + Monto_Constancia;
        Txt_Total_Adeudo.Text = Total_Adeudo.ToString("###,###,##0.00");

        return Total_Adeudo;
    }

    private Double Calcular_Sub_Total()
    {
        Double Total_Adeudo = 0.0;
        Double Total_Descuento = 0.0;
        Double Sub_Total = 0.0;

        if (Txt_Total_Adeudo.Text.Trim() != "")
        {
            Total_Adeudo = Convert.ToDouble(Txt_Total_Adeudo.Text);
        }
        else
        {
            Txt_Total_Adeudo.Text = "0.00";
        }
        if (Txt_Total_Descuento.Text.Trim() != "")
        {
            Total_Descuento = Convert.ToDouble(Txt_Total_Descuento.Text);
        }
        else
        {
            Txt_Total_Descuento.Text = "0.00";
        }
        Sub_Total = Total_Adeudo - Total_Descuento;
        Txt_Sub_Total.Text = Sub_Total.ToString("0.00");

        return Sub_Total;
    }

    private Double Calcular_Total_Convenio()
    {
        Double Sub_Total = 0.0;
        Double Total_Anticipo = 0.0;
        Double Total_Convenio = 0.0;

        if (Txt_Sub_Total.Text.Trim() != "")
        {
            Sub_Total = Convert.ToDouble(Txt_Sub_Total.Text);
        }
        else
        {
            Txt_Sub_Total.Text = "0.00";
        }
        if (Txt_Total_Anticipo.Text.Trim() != "")
        {
            Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text);
        }
        else
        {
            Txt_Total_Anticipo.Text = "0.00";
        }
        Txt_Total_Anticipo.Text = Total_Anticipo.ToString("###,###,##0.00");
        Total_Convenio = Sub_Total - Total_Anticipo;
        Txt_Total_Convenio.Text = Total_Convenio.ToString("0.00");

        return Total_Convenio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Numero_Parcialidades_TextChanged
    ///DESCRIPCIÓN          : Genera las parcialidades en el grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Numero_Parcialidades_TextChanged(object sender, EventArgs e)
    {
        Calcular_Parcialidades();
        Calcular_Total_Anticipo();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Periodicidad_Pago_SelectedIndexChanged
    ///DESCRIPCIÓN          : Genera las parcialidades en el grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Periodicidad_Pago_SelectedIndexChanged(object sender, EventArgs e)
    {
        Calcular_Parcialidades();
        Calcular_Total_Anticipo();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Periodicidad_Pago_SelectedIndexChanged
    ///DESCRIPCIÓN          : Genera las parcialidades en el grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Descuento_Recargos_Ordinarios_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Descuento();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Periodicidad_Pago_SelectedIndexChanged
    ///DESCRIPCIÓN          : Genera las parcialidades en el grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Descuento_Recargos_Moratorios_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Descuento();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Periodicidad_Pago_SelectedIndexChanged
    ///DESCRIPCIÓN          : Genera las parcialidades en el grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Descuento_Multas_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Descuento();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    private void Calcular_Total_Descuento()
    {
        Double Monto_Recargos = 0.0;
        Double Monto_Multas = 0.0;
        Double Recargos_Moratorios = 0.0;
        Double Descuento_Recargos_Ordinarios = 0.0;
        Double Descuento_Recargos_Moratorios = 0.0;
        Double Descuento_Recargos_Multa = 0.0;
        Double Total_Descuento = 0.0;

        if (Txt_Monto_Recargos.Text.Trim() != "")
        {
            Monto_Recargos = Convert.ToDouble(Txt_Monto_Recargos.Text.Trim());
        }
        else
        {
            Txt_Monto_Recargos.Text = "0.00";
        }
        if (Txt_Monto_Multas.Text.Trim() != "")
        {
            Monto_Multas = Convert.ToDouble(Txt_Monto_Multas.Text.Trim());
        }
        else
        {
            Txt_Monto_Multas.Text = "0.00";
        }
        if (Txt_Recargos_Moratorios.Text.Trim() != "")
        {
            Recargos_Moratorios = Convert.ToDouble(Txt_Recargos_Moratorios.Text.Trim());
        }
        else
        {
            Txt_Recargos_Moratorios.Text = "0.00";
        }
        if (Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("_", "").Replace(",", "") != ""
            && Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("_", "").Replace(",", "") != ".")
        {
            Descuento_Recargos_Ordinarios = Monto_Recargos * (Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("_", "").Replace(",", "")) / 100);
        }
        else
        {
            Txt_Descuento_Recargos_Ordinarios.Text = "0.00";
        }
        if (Txt_Descuento_Recargos_Moratorios.Text.Trim().Replace("_", "").Replace(",", "") != ""
            && Txt_Descuento_Recargos_Moratorios.Text.Trim().Replace("_", "").Replace(",", "") != ".")
        {
            Descuento_Recargos_Moratorios = Recargos_Moratorios * (Convert.ToDouble(Txt_Descuento_Recargos_Moratorios.Text.Trim().Replace("_", "").Replace(",", "")) / 100);
        }
        else
        {
            Txt_Descuento_Recargos_Moratorios.Text = "0.00";
        }
        if (Txt_Descuento_Multas.Text.Trim().Replace("_", "").Replace(",", "") != ""
            && Txt_Descuento_Multas.Text.Trim().Replace("_", "").Replace(",", "") != ".")
        {
            Descuento_Recargos_Multa = Monto_Multas * (Convert.ToDouble(Txt_Descuento_Multas.Text.Trim().Replace("_", "").Replace(",", "")) / 100);
        }
        else
        {
            Txt_Descuento_Multas.Text = "0.00";
        }

        Total_Descuento = Descuento_Recargos_Ordinarios + Descuento_Recargos_Moratorios + Descuento_Recargos_Multa;
        Txt_Total_Descuento.Text = Total_Descuento.ToString("###,###,##0.00");
    }

    //[System.Web.Services.WebMethod]
    //public static void WM_Calcular_Parcialidades()
    //{
    //    paginas_Predial_Frm_Ope_Pre_Convenios_Derechos_Supervision Convenios_Derechos_Supervision = new paginas_Predial_Frm_Ope_Pre_Convenios_Derechos_Supervision();
    //    Convenios_Derechos_Supervision.Calcular_Parcialidades();
    //}

    private void Calcular_Parcialidades()
    {
        if (Txt_Numero_Parcialidades.Text != "" && Convert.ToInt32(Txt_Numero_Parcialidades.Text) > 0)
        {
            //Parcialidades
            Int32 Parcialidades;
            Parcialidades = 0;
            Int32 Cont_Parcialidades;
            Cont_Parcialidades = 0;
            //DataTable
            DataTable Dt_Parcialidades = new DataTable();
            DataRow Dr_Parcialidades;
            //Cantidades
            Double Monto_Impuesto = 0;
            Double Monto_Impuesto_Division = 0;
            Double Monto_Recargos = 0;
            Double Monto_Moratorios = 0;
            Double Monto_Multas = 0;
            Double Monto_Honorarios = 0;
            Double Total_Convenio = 0;
            Double Monto_Importe = 0;
            Double Monto_Constancia = 0;
            //Monto de cada parcialidad de la 2 en adelante
            Double Monto_Parcialidades = 0;
            //Periodicidad
            String Dias_Periodo = "";
            Double Total_Anticipo = 0;
            Double Total_Importe_Parcialidad = 0;
            Double Monto_Total = 0;
            Double Total_Importe = 0;
            Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();

            if (Txt_Monto_Multas.Text != "")
            {
                Monto_Multas = Convert.ToDouble(Txt_Monto_Multas.Text) - ((Convert.ToDouble(Txt_Descuento_Multas.Text) / 100) * Convert.ToDouble(Txt_Monto_Multas.Text));
            }
            if (Txt_Monto_Recargos.Text != "")
            {
                Monto_Recargos = Convert.ToDouble(Txt_Monto_Recargos.Text) - ((Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text) / 100) * Convert.ToDouble(Txt_Monto_Recargos.Text));
            }
            if (Txt_Monto_Impuesto.Text != "")
            {
                Monto_Impuesto = Convert.ToDouble(Txt_Monto_Impuesto.Text);
            }
            if (Txt_Imp_Div.Text.Trim() != "")
            {
                Monto_Impuesto_Division = Convert.ToDouble(Txt_Imp_Div.Text);
            }
            if (Txt_Recargos_Moratorios.Text != "")
            {
                Monto_Moratorios = Convert.ToDouble(Txt_Recargos_Moratorios.Text);
            }
            if (Txt_Costo_Constancia.Text != "")
            {
                Monto_Constancia = Convert.ToDouble(Txt_Costo_Constancia.Text);
            }
            if (Txt_Numero_Parcialidades.Text != "")
            {
                Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);
            }
            if (Txt_Total_Anticipo.Text != "")
            {
                Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text);
            }
            if (Txt_Total_Convenio.Text != "")
            {
                Total_Convenio = Convert.ToDouble(Txt_Total_Convenio.Text);
            }
            if (Cmb_Periodicidad_Pago.SelectedIndex > 0)
            {
                Dias_Periodo = Cmb_Periodicidad_Pago.SelectedValue;
            }
            Dt_Parcialidades.Columns.Add(new DataColumn("NO_PAGO", typeof(Int32)));
            Dt_Parcialidades.Columns.Add(new DataColumn("HONORARIOS", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("CONSTANCIA", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_MULTAS", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO_DIVISION", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPORTE", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("FECHA_VENCIMIENTO", typeof(DateTime)));
            Dt_Parcialidades.Columns.Add(new DataColumn("ESTATUS", typeof(String)));

            Dr_Parcialidades = Dt_Parcialidades.NewRow();
            Dr_Parcialidades["NO_PAGO"] = 1;
            if (Monto_Honorarios <= Total_Anticipo)
            {
                Dr_Parcialidades["HONORARIOS"] = Monto_Honorarios;
                Total_Anticipo = Total_Anticipo - Monto_Honorarios;
                Monto_Importe += Monto_Honorarios;
                Monto_Honorarios = 0;
            }
            else
            {
                Dr_Parcialidades["HONORARIOS"] = Total_Anticipo;
                Monto_Honorarios = Monto_Honorarios - Total_Anticipo;
                Monto_Importe += Total_Anticipo;
                Total_Anticipo = 0;
            }
            if (Monto_Constancia <= Total_Anticipo)
            {
                Dr_Parcialidades["CONSTANCIA"] = Monto_Constancia;
                Total_Anticipo = Total_Anticipo - Monto_Constancia;
                Monto_Importe += Monto_Constancia;
                Monto_Constancia = 0;
            }
            else
            {
                Dr_Parcialidades["CONSTANCIA"] = Total_Anticipo;
                Monto_Constancia = Monto_Constancia - Total_Anticipo;
                Monto_Importe += Total_Anticipo;
                Total_Anticipo = 0;
            }
            if (Monto_Multas <= Total_Anticipo)
            {
                Dr_Parcialidades["MONTO_MULTAS"] = Monto_Multas;
                Total_Anticipo = Total_Anticipo - Monto_Multas;
                Monto_Importe += Monto_Multas;
                Monto_Multas = 0;
            }
            else
            {
                Dr_Parcialidades["MONTO_MULTAS"] = Total_Anticipo;
                Monto_Multas = Monto_Multas - Total_Anticipo;
                Monto_Importe += Total_Anticipo;
                Total_Anticipo = 0;
            }
            if (Monto_Recargos <= Total_Anticipo)
            {
                Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
                Total_Anticipo = Total_Anticipo - Monto_Recargos;
                Monto_Importe += Monto_Recargos;
                Monto_Recargos = 0;
            }
            else
            {
                Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Total_Anticipo;
                Monto_Recargos = Monto_Recargos - Total_Anticipo;
                Monto_Importe += Total_Anticipo;
                Total_Anticipo = 0;
            }
            if (Monto_Moratorios <= Total_Anticipo)
            {
                Dr_Parcialidades["RECARGOS_MORATORIOS"] = Monto_Moratorios;
                Total_Anticipo = Total_Anticipo - Monto_Moratorios;
                Monto_Importe += Monto_Moratorios;
                Monto_Moratorios = 0;
            }
            else
            {
                Dr_Parcialidades["RECARGOS_MORATORIOS"] = Total_Anticipo;
                Monto_Moratorios = Monto_Moratorios - Total_Anticipo;
                Monto_Importe += Total_Anticipo;
                Total_Anticipo = 0;
            }


            if (Monto_Impuesto <= Total_Anticipo)
            {
                Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Impuesto;
                Total_Anticipo = Total_Anticipo - Monto_Impuesto;
                Monto_Importe += Monto_Impuesto;
                Monto_Impuesto = 0;
            }
            else
            {
                Dr_Parcialidades["MONTO_IMPUESTO"] = Total_Anticipo;
                Monto_Impuesto = Monto_Impuesto - Total_Anticipo;
                Monto_Importe += Total_Anticipo;
                Total_Anticipo = 0;
            }
            if (Monto_Impuesto_Division <= Total_Anticipo)
            {
                Dr_Parcialidades["MONTO_IMPUESTO_DIVISION"] = Monto_Impuesto_Division;
                Total_Anticipo = Total_Anticipo - Monto_Impuesto_Division;
                Monto_Importe += Monto_Impuesto_Division;
                Monto_Impuesto_Division = 0;
            }
            else
            {
                Dr_Parcialidades["MONTO_IMPUESTO_DIVISION"] = Total_Anticipo;
                Monto_Impuesto_Division = Monto_Impuesto_Division - Total_Anticipo;
                Monto_Importe += Total_Anticipo;
                Total_Anticipo = 0;
            }

            Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe;
            Dr_Parcialidades["FECHA_VENCIMIENTO"] = Dias_Inhabilies.Calcular_Fecha(Obtener_Fecha_Periodo(DateTime.Now, Dias_Periodo, Cont_Parcialidades).ToShortDateString(), "1");
            Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
            Dt_Parcialidades.Rows.Add(Dr_Parcialidades);

            Total_Importe_Parcialidad = Convert.ToDouble((Total_Convenio / (Parcialidades - 1)).ToString("0.00"));

            for (Cont_Parcialidades = 1; Cont_Parcialidades < Parcialidades; Cont_Parcialidades++)
            {
                Monto_Parcialidades = Total_Importe_Parcialidad;
                Monto_Importe = 0;
                Dr_Parcialidades = Dt_Parcialidades.NewRow();
                Dr_Parcialidades["NO_PAGO"] = Cont_Parcialidades + 1;
                if (Cont_Parcialidades == (Parcialidades - 1))
                {
                    Monto_Total = Total_Convenio - Total_Importe;
                    if (Monto_Honorarios <= Monto_Total)
                    {
                        Dr_Parcialidades["HONORARIOS"] = Monto_Honorarios;
                        Monto_Total = Monto_Total - Monto_Honorarios;
                        Monto_Importe += Monto_Honorarios;
                        Monto_Honorarios = 0;
                    }
                    else
                    {
                        Dr_Parcialidades["HONORARIOS"] = Monto_Total;
                        Monto_Honorarios = Monto_Honorarios - Monto_Total;
                        Monto_Importe += Monto_Total;
                        Monto_Total = 0;
                    }
                    if (Monto_Constancia <= Monto_Total)
                    {
                        Dr_Parcialidades["CONSTANCIA"] = Monto_Constancia;
                        Monto_Total = Monto_Total - Monto_Constancia;
                        Monto_Importe += Monto_Constancia;
                        Monto_Constancia = 0;
                    }
                    else
                    {
                        Dr_Parcialidades["CONSTANCIA"] = Monto_Total;
                        Monto_Constancia = Monto_Constancia - Monto_Total;
                        Monto_Importe += Monto_Total;
                        Monto_Total = 0;
                    }
                    if (Monto_Multas <= Monto_Total)
                    {
                        Dr_Parcialidades["MONTO_MULTAS"] = Monto_Multas;
                        Monto_Total = Monto_Total - Monto_Multas;
                        Monto_Importe += Monto_Multas;
                        Monto_Multas = 0;
                    }
                    else
                    {
                        Dr_Parcialidades["MONTO_MULTAS"] = Monto_Total;
                        Monto_Multas = Monto_Multas - Monto_Total;
                        Monto_Importe += Monto_Total;
                        Monto_Total = 0;
                    }
                    if (Monto_Recargos <= Monto_Total)
                    {
                        Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
                        Monto_Total = Monto_Total - Monto_Recargos;
                        Monto_Importe += Monto_Recargos;
                        Monto_Recargos = 0;
                    }
                    else
                    {
                        Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Total;
                        Monto_Recargos = Monto_Recargos - Monto_Total;
                        Monto_Importe += Monto_Total;
                        Monto_Total = 0;
                    }
                    if (Monto_Moratorios <= Monto_Total)
                    {
                        Dr_Parcialidades["RECARGOS_MORATORIOS"] = Monto_Moratorios;
                        Monto_Total = Monto_Total - Monto_Moratorios;
                        Monto_Importe += Monto_Moratorios;
                        Monto_Moratorios = 0;
                    }
                    else
                    {
                        Dr_Parcialidades["RECARGOS_MORATORIOS"] = Monto_Total;
                        Monto_Moratorios = Monto_Moratorios - Monto_Total;
                        Monto_Importe += Monto_Total;
                        Monto_Total = 0;
                    }
                    if (Monto_Impuesto <= Monto_Total)
                    {
                        Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Impuesto;
                        Monto_Total = Monto_Total - Monto_Impuesto;
                        Monto_Importe += Monto_Impuesto;
                        Monto_Impuesto = 0;
                    }
                    else
                    {
                        Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Total;
                        Monto_Impuesto = Monto_Impuesto - Monto_Total;
                        Monto_Importe += Monto_Total;
                        Monto_Total = 0;
                    }
                    Total_Importe += Monto_Importe;
                    //if (Total_Importe > Total_Convenio)
                    //{
                    //    Monto_Total = Monto_Total - (Total_Importe - Total_Convenio);
                    //}
                    //else if (Total_Convenio > Total_Importe)
                    //{
                    //    Monto_Total = Monto_Total - (Total_Convenio - Total_Importe);
                    //}
                    Monto_Importe += Monto_Total;
                    Dr_Parcialidades["MONTO_IMPUESTO_DIVISION"] = Monto_Total;
                    Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe;
                    Dr_Parcialidades["FECHA_VENCIMIENTO"] = Dias_Inhabilies.Calcular_Fecha(Obtener_Fecha_Periodo(DateTime.Now, Dias_Periodo, Cont_Parcialidades).ToShortDateString(), "1");
                    Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
                    Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
                    Grid_Parcialidades.DataSource = Dt_Parcialidades;
                    Grid_Parcialidades.PageIndex = 0;
                    Grid_Parcialidades.DataBind();

                    Sumar_Totales_Parcialidades();
                    return;
                }

                if (Monto_Honorarios <= Monto_Parcialidades)
                {
                    Dr_Parcialidades["HONORARIOS"] = Monto_Honorarios;
                    Monto_Parcialidades = Monto_Parcialidades - Monto_Honorarios;
                    Monto_Importe += Monto_Honorarios;
                    Monto_Honorarios = 0;
                    Monto_Total += Monto_Honorarios;
                }
                else
                {
                    Dr_Parcialidades["HONORARIOS"] = Monto_Parcialidades;
                    Monto_Honorarios = Monto_Honorarios - Monto_Parcialidades;
                    Monto_Importe += Monto_Parcialidades;
                    Monto_Parcialidades = 0;
                    Monto_Total += Monto_Parcialidades;
                }
                if (Monto_Constancia <= Monto_Parcialidades)
                {
                    Dr_Parcialidades["CONSTANCIA"] = Monto_Constancia;
                    Monto_Parcialidades = Monto_Parcialidades - Monto_Constancia;
                    Monto_Importe += Monto_Constancia;
                    Monto_Constancia = 0;
                    Monto_Total += Monto_Constancia;
                }
                else
                {
                    Dr_Parcialidades["CONSTANCIA"] = Monto_Parcialidades;
                    Monto_Constancia = Monto_Constancia - Monto_Parcialidades;
                    Monto_Importe += Monto_Parcialidades;
                    Monto_Parcialidades = 0;
                    Monto_Total += Monto_Parcialidades;
                }
                if (Monto_Multas <= Monto_Parcialidades)
                {
                    Dr_Parcialidades["MONTO_MULTAS"] = Monto_Multas;
                    Monto_Parcialidades = Monto_Parcialidades - Monto_Multas;
                    Monto_Importe += Monto_Multas;
                    Monto_Multas = 0;
                    Monto_Total += Monto_Multas;
                }
                else
                {
                    Dr_Parcialidades["MONTO_MULTAS"] = Monto_Parcialidades;
                    Monto_Multas = Monto_Multas - Monto_Parcialidades;
                    Monto_Importe += Monto_Parcialidades;
                    Monto_Parcialidades = 0;
                    Monto_Total += Monto_Parcialidades;
                }
                if (Monto_Recargos <= Monto_Parcialidades)
                {
                    Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
                    Monto_Parcialidades = Monto_Parcialidades - Monto_Recargos;
                    Monto_Importe += Monto_Recargos;
                    Monto_Recargos = 0;
                    Monto_Total += Monto_Recargos;
                }
                else
                {
                    Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Parcialidades;
                    Monto_Recargos = Monto_Recargos - Monto_Parcialidades;
                    Monto_Importe += Monto_Parcialidades;
                    Monto_Parcialidades = 0;
                    Monto_Total += Monto_Parcialidades;
                }
                Dr_Parcialidades["RECARGOS_MORATORIOS"] = 0;
                if (Monto_Impuesto <= Monto_Parcialidades)
                {
                    Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Impuesto;
                    Monto_Parcialidades = Monto_Parcialidades - Monto_Impuesto;
                    Monto_Importe += Monto_Impuesto;
                    Monto_Impuesto = 0;
                    Monto_Total += Monto_Impuesto;
                }
                else
                {
                    Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Parcialidades;
                    Monto_Impuesto = Monto_Impuesto - Monto_Parcialidades;
                    Monto_Importe += Monto_Parcialidades;
                    Monto_Parcialidades = 0;
                    Monto_Total += Monto_Parcialidades;
                }
                if (Monto_Impuesto_Division <= Monto_Parcialidades)
                {
                    Dr_Parcialidades["MONTO_IMPUESTO_DIVISION"] = Monto_Impuesto_Division;
                    Monto_Parcialidades = Monto_Parcialidades - Monto_Impuesto_Division;
                    Monto_Importe += Monto_Impuesto_Division;
                    Monto_Impuesto_Division = 0;
                    Monto_Total += Monto_Impuesto_Division;
                }
                else
                {
                    Dr_Parcialidades["MONTO_IMPUESTO_DIVISION"] = Monto_Parcialidades;
                    Monto_Impuesto_Division = Monto_Impuesto_Division - Monto_Parcialidades;
                    Monto_Importe += Monto_Parcialidades;
                    Monto_Parcialidades = 0;
                    Monto_Total += Monto_Parcialidades;
                }

                Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe;
                Total_Importe += Monto_Importe;
                Dr_Parcialidades["FECHA_VENCIMIENTO"] = Dias_Inhabilies.Calcular_Fecha(Obtener_Fecha_Periodo(DateTime.Now, Dias_Periodo, Cont_Parcialidades).ToShortDateString(), "1");
                Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
                Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
            }
            Grid_Parcialidades.DataSource = Dt_Parcialidades;
            Grid_Parcialidades.PageIndex = 0;
            Grid_Parcialidades.DataBind();

            Sumar_Totales_Parcialidades();
        }
        else
        {
            Grid_Parcialidades.DataSource = null;
            Grid_Parcialidades.DataBind();
        }
    }

    private DateTime Obtener_Fecha_Periodo(DateTime Primer_Fecha_Periodo, String Periodo, Int32 Parcialidades)
    {
        int Dias_Periodo;
        DateTime Fecha_Periodo = Primer_Fecha_Periodo;
        if (int.TryParse(Periodo, out Dias_Periodo))
        {
            Fecha_Periodo = Primer_Fecha_Periodo.AddDays(Dias_Periodo * Parcialidades);
        }
        else
        {
            switch (Periodo)
            {
                case "Mensual":
                    Fecha_Periodo = Primer_Fecha_Periodo.AddMonths(Parcialidades);
                    break;
                case "Anual":
                    Fecha_Periodo = Primer_Fecha_Periodo.AddYears(Parcialidades);
                    break;
            }
        }
        Fecha_Periodo = Fecha_Periodo.AddDays(-1);
        return Fecha_Periodo;
    }

    protected void Txt_Porcentaje_Anticipo_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    protected void Txt_Total_Anticipo_TextChanged(object sender, EventArgs e)
    {
        Calcular_Porcentaje_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    private Double Calcular_Porcentaje_Anticipo()
    {
        Double Sub_Total = 0.0;
        Double Total_Anticipo = 0.0;
        Double Porcentaje_Anticipo = 0.0;

        if (Txt_Sub_Total.Text.Trim() != "")
        {
            Sub_Total = Convert.ToDouble(Txt_Sub_Total.Text);
        }
        else
        {
            Txt_Sub_Total.Text = "0.00";
        }
        if (Txt_Total_Anticipo.Text.Trim().Replace("_", "").Replace(",", "") != ""
            && Txt_Total_Anticipo.Text.Trim().Replace("_", "").Replace(",", "") != ".")
        {
            Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text.Trim().Replace("_", "").Replace(",", ""));
        }
        else
        {
            Txt_Total_Anticipo.Text = "0.00";
        }
        Porcentaje_Anticipo = (Total_Anticipo * 100) / Sub_Total;
        Txt_Porcentaje_Anticipo.Text = Porcentaje_Anticipo.ToString("###,###,##0.00");
        return Porcentaje_Anticipo;
    }

    private Double Calcular_Recargos_Moratorios()
    {
        Double Monto_Impuesto = 0;
        Double Monto_Recargos = 0;
        Double Recargos_Moratorios = 0;
        if (Grid_Parcialidades.Rows.Count > 0)
        {
            if (Convert.ToDateTime(Grid_Parcialidades.Rows[0].Cells[8].Text).Month < DateTime.Now.Month || Convert.ToDateTime(Grid_Parcialidades.Rows[0].Cells[8].Text).Year < DateTime.Now.Year)
            {

                if (Txt_Monto_Impuesto.Text.Trim() != "")
                {
                    Monto_Impuesto = Convert.ToDouble(Txt_Monto_Impuesto.Text);
                }
                else
                {
                    Txt_Monto_Impuesto.Text = "0.00";
                }
                if (Txt_Monto_Recargos.Text.Trim() != "")
                {
                    Monto_Recargos = Convert.ToDouble(Txt_Monto_Recargos.Text);
                }
                else
                {
                    Txt_Monto_Recargos.Text = "0.00";
                }
                Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
                DataTable Dt_Ayudante;
                Dt_Ayudante = Parametros.Consultar_Parametros();
                Double Porcetaje;
                Porcetaje = (Convert.ToDouble(Dt_Ayudante.Rows[0]["RECARGAS_TRASLADO"].ToString()) / 100);
                Recargos_Moratorios = Monto_Impuesto * Porcetaje;
                Txt_Recargos_Moratorios.Text = Recargos_Moratorios.ToString("0.00");
            }
        }
        else
        {
            if (Txt_Monto_Impuesto.Text.Trim() != "")
            {
                Monto_Impuesto = Convert.ToDouble(Txt_Monto_Impuesto.Text);
            }
            else
            {
                Txt_Monto_Impuesto.Text = "0.00";
            }
            if (Txt_Monto_Recargos.Text.Trim() != "")
            {
                Monto_Recargos = Convert.ToDouble(Txt_Monto_Recargos.Text);
            }
            else
            {
                Txt_Monto_Recargos.Text = "0.00";
            }
            Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
            DataTable Dt_Ayudante;
            Dt_Ayudante = Parametros.Consultar_Parametros();
            Double Porcetaje;
            Porcetaje = (Convert.ToDouble(Dt_Ayudante.Rows[0]["RECARGAS_TRASLADO"].ToString()) / 100);
            Recargos_Moratorios = Monto_Impuesto * Porcetaje;
            Txt_Recargos_Moratorios.Text = Recargos_Moratorios.ToString("0.00");
        }
        return Recargos_Moratorios;
    }

    //private Double Calcular_Total_Anticipo()
    //{
    //    Double Sub_Total = 0.0;
    //    Double Porcentaje_Anticipo = 0.0;
    //    Double Total_Anticipo = 0.0;

    //    if (Txt_Sub_Total.Text.Trim() != "")
    //    {
    //        Sub_Total = Convert.ToDouble(Txt_Sub_Total.Text);
    //    }
    //    else
    //    {
    //        Txt_Sub_Total.Text = "0.00";
    //    }
    //    if (Txt_Porcentaje_Anticipo.Text.Trim().Replace("_", "").Replace(",", "") != ""
    //        && Txt_Porcentaje_Anticipo.Text.Trim().Replace("_", "").Replace(",", "") != ".")
    //    {
    //        Porcentaje_Anticipo = Convert.ToDouble(Txt_Porcentaje_Anticipo.Text.Trim().Replace("_", "").Replace(",", ""));
    //    }
    //    else
    //    {
    //        Txt_Porcentaje_Anticipo.Text = "0.00";
    //    }
    //    Total_Anticipo = Sub_Total * Porcentaje_Anticipo / 100;
    //    Txt_Total_Anticipo.Text = Total_Anticipo.ToString("0.00");
    //    return Total_Anticipo;
    //}
    private Double Calcular_Total_Anticipo()
    {
        Double Sub_Total = 0.0;
        Double Porcentaje_Anticipo = 0.0;
        Double Total_Anticipo = 0.0;

        if (Txt_Sub_Total.Text.Trim() != "")
        {
            Sub_Total = Convert.ToDouble(Txt_Sub_Total.Text);
        }
        else
        {
            Txt_Sub_Total.Text = "0.00";
        }
        if (Txt_Porcentaje_Anticipo.Text.Trim().Replace("_", "").Replace(",", "") != ""
            && Txt_Porcentaje_Anticipo.Text.Trim().Replace("_", "").Replace(",", "") != ".")
        {
            Porcentaje_Anticipo = Convert.ToDouble(Txt_Porcentaje_Anticipo.Text.Trim().Replace("_", "").Replace(",", ""));
        }
        else
        {
            Txt_Porcentaje_Anticipo.Text = "0.00";
        }
        Total_Anticipo = Sub_Total * Porcentaje_Anticipo / 100;
        Txt_Total_Anticipo.Text = Total_Anticipo.ToString("###,###,##0.00");
        return Total_Anticipo;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Insertar_Pasivo
    ///DESCRIPCIÓN          : Consulta el Costo del Documento y lo Inserta en Pasivo
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 07/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Insertar_Pasivo(String Referencia)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
            DataTable Dt_Clave;

            Claves_Ingreso.P_Documento_ID = "00005";
            Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
            if (Dt_Clave.Rows.Count > 0)
            {
                Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                Calculo_Impuesto_Traslado.P_Descripcion = "COBRO POR REESTRUCTURA DE CONVENIO DE TRASLADO DE DOMINIO DE LA CUENTA PREDIAL " + Txt_Cuenta_Predial.Text;
                Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = "" + Convert.ToDouble(Txt_Total_Anticipo.Text);
                Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.ToString("dd/MMM/yyyy");
                Calculo_Impuesto_Traslado.Alta_Pasivo();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "El Pasivo no pudo ser insertado: " + Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    //protected void Txt_Cuenta_Predial_TextChanged()
    //{
    //    DataTable Dt_Orden;
    //    if (Hdf_Cuenta_Predial_ID.Value.Length > 0)
    //    {
    //        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
    //        Cuenta.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
    //        Cuenta = Cuenta.Consultar_Datos_Propietario();
    //        if (Cuenta.P_RFC_Propietario != null || Cuenta.P_Nombre_Calle != null || Cuenta.P_Nombre_Propietario != null || Cuenta.P_Nombre_Colonia != null || Cuenta.P_No_Exterior != null || Cuenta.P_No_Interior != null || Cuenta.P_Pro_Propietario_ID != null)
    //        {
    //            Txt_Calle.Text = Cuenta.P_Nombre_Calle;
    //            Txt_Propietario.Text = Cuenta.P_Nombre_Propietario;
    //            Txt_Colonia.Text = Cuenta.P_Nombre_Colonia;
    //            Txt_No_Exterior.Text = Cuenta.P_No_Exterior;
    //            Txt_No_Interior.Text = Cuenta.P_No_Interior;
    //            Hdf_Propietario_ID.Value = Cuenta.P_Pro_Propietario_ID;
    //            Hdf_RFC.Value = Cuenta.P_RFC_Propietario;
    //        }
    //        else
    //        {

    //            Cls_Ope_Pre_Orden_Variacion_Negocio Orden = new Cls_Ope_Pre_Orden_Variacion_Negocio();
    //            Orden.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
    //            Dt_Orden = Orden.Consultar_Ordenes_Variacion();

    //            Orden.P_Año = Convert.ToInt32(Dt_Orden.Rows[0][Ope_Pre_Orden_Variacion.Campo_Anio].ToString());
    //            Orden.P_Orden_Variacion_ID = Dt_Orden.Rows[0][Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion].ToString();
    //            Dt_Orden = Orden.Consultar_Propietarios_Variacion().Tables[0];
    //            if (Dt_Orden.Rows.Count > 0)
    //            {
    //                Txt_Colonia.Text = Dt_Orden.Rows[0]["NOMBRE_CALLE"].ToString();
    //                Txt_Calle.Text = Dt_Orden.Rows[0]["NOMBRE_COLONIA"].ToString();
    //                Txt_Propietario.Text = Dt_Orden.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString();
    //                Txt_No_Exterior.Text = Dt_Orden.Rows[0]["NO_EXTERIOR_NOTIFICACION"].ToString();
    //                Txt_No_Interior.Text = Dt_Orden.Rows[0]["NO_INTERIOR_NOTIFICACION"].ToString();
    //                Txt_No_Interior.Text = Dt_Orden.Rows[0]["NO_INTERIOR_NOTIFICACION"].ToString();
    //                Hdf_Propietario_ID.Value = Dt_Orden.Rows[0]["CONTRIBUYENTE_ID"].ToString();
    //                Hdf_RFC.Value = Dt_Orden.Rows[0]["RFC"].ToString();
    //                Txt_RFC.Text = Hdf_RFC.Value;
    //            }
    //        }
    //    }
    //}

    protected void Txt_Cuenta_Predial_TextChanged()
    {
        DataTable Dt_Orden;
        if (Hdf_Cuenta_Predial_ID.Value.Length > 0)
        {
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuenta.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Cuenta = Cuenta.Consultar_Datos_Propietario();
            //if (Cuenta.P_RFC_Propietario != null || Cuenta.P_Nombre_Calle != null || Cuenta.P_Nombre_Propietario != null || Cuenta.P_Nombre_Colonia != null || Cuenta.P_No_Exterior != null || Cuenta.P_No_Interior != null || Cuenta.P_Pro_Propietario_ID != null)
            //{
            Txt_Calle.Text = Cuenta.P_Nombre_Calle;
            Txt_Propietario.Text = Cuenta.P_Nombre_Propietario;
            Txt_Colonia.Text = Cuenta.P_Nombre_Colonia;
            Txt_No_Exterior.Text = Cuenta.P_No_Exterior;
            Txt_No_Interior.Text = Cuenta.P_No_Interior;
            if (Cuenta.P_Propietario_ID != null)
            {
                Hdf_Propietario_ID.Value = Cuenta.P_Propietario_ID;
            }
            if (Cmb_Tipo_Solicitante.SelectedValue == "PROPIETARIO")
            {
                Hdf_RFC.Value = Cuenta.P_RFC_Propietario;
                Txt_Solicitante.Text = Txt_Propietario.Text;
            }
            Txt_RFC.Text = Hdf_RFC.Value;
            //}
            //else
            //{
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Orden.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Dt_Orden = Orden.Consultar_Ordenes_Variacion();
            if (Dt_Orden.Rows.Count == 0)
            {
                return;
            }
            Orden.P_Año = Convert.ToInt32(Dt_Orden.Rows[0][Ope_Pre_Orden_Variacion.Campo_Anio].ToString());
            Orden.P_Orden_Variacion_ID = Dt_Orden.Rows[0][Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion].ToString();
            Dt_Orden = Orden.Consultar_Domicilio_Y_Propietario();
            if (Dt_Orden.Rows.Count > 0)
            {
                Hdf_Propietario_ID.Value = Dt_Orden.Rows[0]["CONTRIBUYENTE_ID"].ToString();
                String Dom_Foraneo = "";
                String No_int_not = "";
                String No_ext_not = "";
                String No_Int = "";
                String No_Ext = "";
                String Dom_Not_Colonia = "";
                String Dom_Not_Calle = "";
                String Dom_Colonia = "";
                String Dom_Calle = "";
                foreach (DataRow Renglon_Actual in Dt_Orden.Rows)
                {
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo].ToString() != "")
                    {
                        Dom_Foraneo = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion].ToString() != "")
                    {
                        No_int_not = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion].ToString() != "")
                    {
                        No_ext_not = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString() != "")
                    {
                        No_Int = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString() != "")
                    {
                        No_Ext = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion].ToString() != "")
                    {
                        Dom_Not_Calle = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion].ToString() != "")
                    {
                        Dom_Not_Colonia = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion].ToString() != "")
                    {
                        Dom_Calle = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion].ToString() != "")
                    {
                        Dom_Colonia = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion].ToString();
                    }
                }
                Txt_Propietario.Text = Dt_Orden.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString();
                if (Cmb_Tipo_Solicitante.SelectedValue == "PROPIETARIO")
                {
                    Txt_RFC.Text = Dt_Orden.Rows[0]["RFC"].ToString();
                    Txt_Solicitante.Text = Dt_Orden.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString();
                }
                Hdf_RFC.Value = Dt_Orden.Rows[0]["RFC"].ToString();
                if (Dom_Foraneo == "SI" && Dom_Foraneo != "")
                {
                    Txt_Calle.Text = Dom_Calle;
                    Txt_Colonia.Text = Dom_Colonia;
                    Txt_No_Exterior.Text = No_ext_not;
                    Txt_No_Interior.Text = No_int_not;
                }
                else if (Dom_Foraneo == "NO" && Dom_Foraneo != "")
                {
                    Cls_Cat_Pre_Calles_Negocio Calle = new Cls_Cat_Pre_Calles_Negocio();
                    Calle.P_Calle_ID = Dom_Not_Calle;
                    Calle.P_Mostrar_Nombre_Calle_Nombre_Colonia = true;
                    if (Calle.P_Calle_ID != "")
                    {
                        DataTable Dt_Calle_Colonia = Calle.Consultar_Nombre_Id_Calles();
                        String[] Calle_Col = Dt_Calle_Colonia.Rows[0]["NOMBRE"].ToString().Split('-');
                        Txt_Calle.Text = Calle_Col[0];
                        Txt_Colonia.Text = Calle_Col[1];
                    }
                    Txt_No_Exterior.Text = No_Ext;
                    Txt_No_Interior.Text = No_Int;
                }
            }
            //}
        }
    }


    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Convenio
    /// DESCRIPCIÓN: Generar convenio (con OpenXML SDK a partir de documento con controles de contenido)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Convenio()
    {
        string Ruta_Plantilla = Server.MapPath("PlantillasWord/" + "Formato_Reestructura_Convenio_Traslado.docx");
        string Documento_Salida = Server.MapPath("../../Reporte/" + "Reestructura_Convenio_Traslado.docx");
        //Documento_Salida = Server.MapPath("~/Reporte/" + "Convenio_Der_Sup.docx");

        //create copy of template so that we don't overwrite it
        if (System.IO.File.Exists(Documento_Salida))
        {
            System.IO.File.Delete(Documento_Salida);
        }
        File.Copy(Ruta_Plantilla, Documento_Salida);

        ReportDocument Reporte = new ReportDocument();
        String Nombre_Archivo = "Reestructura_Convenio_Traslado.docx";
        String Tipo_Solicitante;
        String Calle_Numero;
        DateTime Fecha_Convenio;
        String PDF_Convenio = Nombre_Archivo + ".pdf";
        String Importe_Letra;
        string Periodicidad = "";

        // si no existe el directorio, crearlo
        if (!System.IO.Directory.Exists(Server.MapPath("../../Reporte")))
            System.IO.Directory.CreateDirectory("../../Reporte");

        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        // tipo de contribuyente
        if (Cmb_Tipo_Solicitante.SelectedValue == "PROPIETARIO")
            Tipo_Solicitante = "CONTRIBUYENTE";
        else
            Tipo_Solicitante = Cmb_Tipo_Solicitante.SelectedValue;
        // formar ubicacion (calle, numero y si existe, numero interior)
        Calle_Numero = Txt_Calle.Text + " " + Txt_No_Exterior.Text + " ";
        if (Txt_No_Interior.Text != "")
            Calle_Numero += "INT " + Txt_No_Interior.Text + " ";
        if (!DateTime.TryParse(Txt_Fecha.Text, out Fecha_Convenio))
            Fecha_Convenio = DateTime.Now;

        // convertir el importe del convenio a letra
        Numalet Cantidad = new Numalet();
        Cantidad.MascaraSalidaDecimal = "00/100 M.N.";
        Cantidad.SeparadorDecimalSalida = "Pesos";
        Cantidad.ApocoparUnoParteEntera = true;
        Cantidad.LetraCapital = true;
        Importe_Letra = Cantidad.ToCustomCardinal(Txt_Sub_Total.Text.Replace(",", ""));
        // periodicidad
        switch (Cmb_Periodicidad_Pago.SelectedValue)
        {
            case "7":
                Periodicidad = "PAGOS SEMANALES";
                break;
            case "14":
                Periodicidad = "PAGOS CATORCENALES";
                break;
            case "15":
                Periodicidad = "PAGOS QUINCENALES";
                break;
            case "Mensual":
                Periodicidad = "MENSUALIDADES";
                break;
            case "Anual":
                Periodicidad = "ANUALIDADES";
                break;
        }

        try
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
            {
                Int32 Parcialidades = 0;
                //create XML string matching custom XML part
                string newXml = "<root>"
                    + "<Folio>" + "CTRA" + Convert.ToInt32(Hdf_No_Convenio.Value) + "</Folio>"
                    + "<Nombre_Solicitante>" + Txt_Solicitante.Text.Trim().ToUpper() + "</Nombre_Solicitante>"
                    + "<Tipo_Solicitante>" + Tipo_Solicitante + "</Tipo_Solicitante>"
                    + "<Tipo_Solicitante2>" + Tipo_Solicitante + "</Tipo_Solicitante2>"
                    + "<Tipo_Solicitante3>" + Tipo_Solicitante + "</Tipo_Solicitante3>"
                    + "<rfc>" + Txt_RFC.Text + "</rfc>"
                    + "<Nombre_Titular>" + Txt_Propietario.Text.Trim().ToUpper() + "</Nombre_Titular>"
                    + "<Calle_Numero>" + Calle_Numero + "</Calle_Numero>"
                    + "<Nombre_Colonia>" + Txt_Colonia.Text + "</Nombre_Colonia>"
                    + "<Cuenta_Predial>" + Txt_Cuenta_Predial.Text + "</Cuenta_Predial>"
                    + "<Cantidad_Letra>" + Importe_Letra + "</Cantidad_Letra>"
                    + "<Cantidad_Numero>" + Txt_Sub_Total.Text + "</Cantidad_Numero>"
                    + "<Numero_Parcialidades>" + Txt_Numero_Parcialidades.Text + "</Numero_Parcialidades>"
                    + "<Periodicidad>" + Periodicidad + "</Periodicidad>"
                    + "<Dia_Del_Mes>" + Fecha_Convenio.Day.ToString() + "</Dia_Del_Mes>"
                    + "<Mes>" + Fecha_Convenio.ToString("MMMM").ToUpper() + "</Mes>"
                    + "<Anio>" + Fecha_Convenio.Year.ToString() + "</Anio>"
                    + "</root>";

                MainDocumentPart main = doc.MainDocumentPart;
                main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);

                //add and write new XML part
                CustomXmlPart customXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                //*** openXML_Wp = DocumentFormat.OpenXml.Wordprocessing
                // localizar la etiqueta del control de contenido en la que esta la tabla
                openXML_Wp.SdtBlock ccWithTable = main.Document.Body.Descendants<openXML_Wp.SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<openXML_Wp.Tag>().Val == "TABLA_PARCIALIDADES").Single();
                // Localizar la tabla
                openXML_Wp.Table Tabla_Parcialidades = ccWithTable.Descendants<openXML_Wp.Table>().Single();
                // localizar la ultima fila de la tabla
                openXML_Wp.TableRow Fila_Vacia = Tabla_Parcialidades.Elements<openXML_Wp.TableRow>().Last();

                // estilo para los campos (crear un estilo y asignarle el id del estilo en el documento original de word)
                openXML_Wp.ParagraphProperties Propiedades_Parrafo = new openXML_Wp.ParagraphProperties();
                Propiedades_Parrafo.ParagraphStyleId = new openXML_Wp.ParagraphStyleId { Val = "EstiloTablaParcialidades" };
                openXML_Wp.ParagraphProperties Prop_Montos_Parcialidades = new openXML_Wp.ParagraphProperties();
                Prop_Montos_Parcialidades.ParagraphStyleId = new openXML_Wp.ParagraphStyleId { Val = "MontoTablaParcialidades" };
                openXML_Wp.ParagraphProperties Propiedades_Parrafo0;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo1;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo2;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo3;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo4;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo5;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo6;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo7;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo8;

                foreach (GridViewRow fila in Grid_Parcialidades.Rows)
                {
                    //if (fila.Cells[8].Text == "POR PAGAR")
                    //{
                    DateTime Fecha_Parcialidad;

                    // convertir la fecha
                    DateTime.TryParse(fila.Cells[9].Text, out Fecha_Parcialidad);

                    // copiar una fila nueva en la tabla
                    openXML_Wp.TableRow Nueva_Fila = (openXML_Wp.TableRow)Fila_Vacia.CloneNode(true);

                    // generar estilos para cada elemento (una vez enlazado no se puede agregar a otro, por eso se duplican con OuterXML)
                    Propiedades_Parrafo0 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                    Propiedades_Parrafo1 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                    Propiedades_Parrafo2 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo3 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo4 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo5 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo6 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo7 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo8 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);

                    // agregar datos a la nueva fila (el texto debe ir dentro de un elemento Run, para poderlo agregar en un elemento párrafo)
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(0).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(0).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo0,
                        new openXML_Wp.Run(new openXML_Wp.Text((++Parcialidades).ToString()))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(1).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(1).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo1,
                        new openXML_Wp.Run(new openXML_Wp.Text(Fecha_Parcialidad.ToString("dd-MMM-yyyy").ToUpper()))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(2).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(2).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo2,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[2].Text.Replace("$", "")))));

                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(3).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(3).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo3,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[3].Text.Replace("$", "")))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(4).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(4).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo4,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[4].Text.Replace("$", "")))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(5).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(5).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo5,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[5].Text.Replace("$", "")))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(6).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(6).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo6,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[6].Text.Replace("$", "")))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(7).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(7).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo7,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[7].Text.Replace("$", "")))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(8).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(8).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo8,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[8].Text.Replace("$", "")))
                        ));

                    // agregar la nueva fila a la tabla
                    Tabla_Parcialidades.AppendChild(Nueva_Fila);

                    //} // si estatus == POR PAGAR
                }

                // copiar una fila nueva en la tabla para los totales
                openXML_Wp.TableRow Fila_Totales = (openXML_Wp.TableRow)Fila_Vacia.CloneNode(true);

                openXML_Wp.Paragraph Parrafo_Nuevo = new openXML_Wp.Paragraph();

                // cambiar estilo para totales de la tabla
                Propiedades_Parrafo.ParagraphStyleId = new openXML_Wp.ParagraphStyleId { Val = "EstiloEncabezadoParcialidades" };
                Prop_Montos_Parcialidades.ParagraphStyleId = new openXML_Wp.ParagraphStyleId { Val = "MontoEncabezadoParcialidades" };
                Propiedades_Parrafo0 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                Propiedades_Parrafo1 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                Propiedades_Parrafo2 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo3 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo4 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo5 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo6 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo7 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo8 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);

                // agregar datos a la fila de totales
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(0).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(0).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo0,
                    new openXML_Wp.Run(new openXML_Wp.Text(""))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(1).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(1).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo1,
                    new openXML_Wp.Run(new openXML_Wp.Text("TOTALES"))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(2).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(2).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo2,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[2].Text.Replace("$", "")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(3).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(3).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo3,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[3].Text.Replace("$", "")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(4).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(4).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo4,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[4].Text.Replace("$", "")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(5).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(5).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo5,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[5].Text.Replace("$", "")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(6).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(6).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo6,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[6].Text.Replace("$", "")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(7).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(7).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo7,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[7].Text.Replace("$", "")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(8).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(8).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo8,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[8].Text.Replace("$", "")))
                    ));

                // agregar la nueva fila a la tabla
                Tabla_Parcialidades.AppendChild(Fila_Totales);

                // eliminar la fila vacia que se tomo como base
                Tabla_Parcialidades.RemoveChild(Fila_Vacia);
                using (StreamWriter ts = new StreamWriter(customXml.GetStream()))
                {
                    ts.Write(newXml);
                }
                // guardar los cambios en el documento
                main.Document.Save();

                //closing WordprocessingDocument automatically saves the document
            }

            //string Ruta = @HttpContext.Current.Server.MapPath("~/Reporte/" + Nombre_Archivo);
            //// ofrecer para descarga
            //Response.Clear();
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.ContentType = "application/x-msword";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta);
            ////           'Visualiza el archivo
            //Response.WriteFile(Ruta);
            //Response.Flush();
            //Response.Close();

            String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
            Pagina = Pagina + "Reestructura_Convenio_Traslado.docx";
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Formato_Convenio",
                "window.open('" + Pagina +
                "', '" + "msword" + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch (Exception Ex)
        {
            throw new Exception("Imprimir convenio: " + Ex.Message);
        }
    }

    #endregion

    #region Impresion Folios

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataTable Dt_Constancias, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Reporte.Subreports["Rpt_Constancias_Propiedad"].SetDataSource(Dt_Constancias);
        Reporte.Subreports["Rpt_Constancias_No_Propiedad"].SetDataSource(Dt_Constancias);
        Reporte.Subreports["Rpt_Constancias_No_Adeudo"].SetDataSource(Dt_Constancias);
        Reporte.Subreports["Rpt_Certificaciones"].SetDataSource(Dt_Constancias);

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar        
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;

        Reporte.Export(Export_Options);
        Reporte.PrintToPrinter(1, true, 0, 0);
    }

    /////******************************************************************************* 
    /////NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    /////DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    /////PARAMETROS: 
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 29/Julio/2011
    /////MODIFICO: 
    /////FECHA_MODIFICO:
    /////CAUSA_MODIFICACIÓN:
    /////*******************************************************************************
    //private void Imprimir_Reporte(DataSet Ds_Constancias, String Nombre_Reporte, String Nombre_Archivo)
    //{
    //    ReportDocument Reporte = new ReportDocument();
    //    String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
    //    try
    //    {
    //        Reporte.Load(File_Path);
    //        Reporte.Subreports["Rpt_Constancias_Propiedad"].SetDataSource(Ds_Constancias.Tables["Dt_Constancias_Propiedad"]);
    //        Reporte.Subreports["Rpt_Constancias_No_Propiedad"].SetDataSource(Ds_Constancias.Tables["Dt_Constancias_No_Propiedad"]);
    //        Reporte.Subreports["Rpt_Constancias_No_Adeudo"].SetDataSource(Ds_Constancias.Tables["Dt_Constancias_No_Adeudo"]);
    //        Reporte.Subreports["Rpt_Certificaciones"].SetDataSource(Ds_Constancias.Tables["Dt_Certificaciones"]);
    //    }
    //    catch
    //    {
    //        Lbl_Mensaje_Error.Visible = true;
    //        Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
    //    }

    //    String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
    //    try
    //    {
    //        ExportOptions Export_Options = new ExportOptions();
    //        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
    //        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
    //        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
    //        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
    //        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
    //        Reporte.Export(Export_Options);
    //    }
    //    catch
    //    {
    //        //Lbl_Mensaje_Error.Visible = true;
    //        //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
    //    }

    //    try
    //    {
    //        Reporte.PrintToPrinter(1, true, 0, 0);
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Mensaje_Error.Visible = true;
    //        Lbl_Mensaje_Error.Text = Ex.Message.ToString();
    //    }
    //}

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Convenios_Traslado
    ///DESCRIPCIÓN          : Crea un DataTable con el folio de pago
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 13/Octubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Convenios_Folio(String Folio)
    {
        Ds_Pre_Folio_Convenios Ds_Convenio = new Ds_Pre_Folio_Convenios();
        DataTable Dt_Convenio = Ds_Convenio.Tables["Dt_Convenio"];
        DataRow Dr_Convenio;

        Dr_Convenio = Dt_Convenio.NewRow();
        Dr_Convenio["FOLIO"] = Folio;
        Dr_Convenio["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text.ToUpper();
        Dr_Convenio["PROPIETARIO"] = Txt_Propietario.Text.ToUpper();
        Dr_Convenio["TIPO_CONVENIO"] = "Reestructura de convenio de traslado de dominio";
        String Domicilio = "";
        Domicilio = Txt_Calle.Text;
        if (Txt_No_Exterior.Text != "")
        {
            if (Domicilio != "")
            {
                Domicilio += ", #";
            }
            Domicilio += Txt_No_Exterior.Text;
        }
        if (Txt_No_Interior.Text != "")
        {
            if (Domicilio != "")
            {
                Domicilio += ", #";
            }
            Domicilio += Txt_No_Interior.Text;
        }
        if (Txt_Colonia.Text != "")
        {
            if (Domicilio != "")
            {
                Domicilio += ", ";
            }
            Domicilio += Txt_Colonia.Text;
        }

        Dr_Convenio["UBICACION"] = Domicilio;
        Dr_Convenio["MONTO_IMPUESTO"] = Grid_Parcialidades.Rows[0].Cells[6].Text;
        Dr_Convenio["MONTO_MULTAS"] = Grid_Parcialidades.Rows[0].Cells[3].Text;
        Dr_Convenio["MONTO_RECARGOS_ORD"] = Grid_Parcialidades.Rows[0].Cells[4].Text;
        Dr_Convenio["MONTO_RECARGOS_MOR"] = Grid_Parcialidades.Rows[0].Cells[5].Text;
        Dr_Convenio["CONSTANCIA"] = Grid_Parcialidades.Rows[0].Cells[2].Text;
        Dr_Convenio["TOTAL_A_PAGAR"] = Grid_Parcialidades.Rows[0].Cells[7].Text;
        Dt_Convenio.Rows.Add(Dr_Convenio);

        return Ds_Convenio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
    ///DESCRIPCIÓN          : Visualiza en pantalla el reporte indicado
    ///PARAMETROS           : Nombre_Reporte: cadena con el nombre del archivo.
    ///                     : Formato: Exensión del archivo a visualizar.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Formato, String Frm_Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            //if (Formato == "PDF")
            //{
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Frm_Formato, "window.open('" + Pagina + "', '" + Formato + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            //}
            //else if (Formato == "Excel")
            //{
            //    String Ruta = "../../Reporte/" + Nombre_Reporte;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            //}
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }


    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenios, String Nombre_Reporte, String Nombre_Archivo, String Formato, String Tipo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Convenios);
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
        catch //(Exception Ex)
        {
            //Lbl_Mensaje_Error.Visible = true;
            //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
        }

        try
        {
            Mostrar_Reporte(Archivo_PDF, Tipo, Formato);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Convenios_Fraccionamientos
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos del convenios de Fraccionamiento Seleccionado en el GridView
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 26/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Convenios_Traslado()
    {
        Ds_Convenio_Impuesto_Traslado_Dominio Ds_Convenios_TD = new Ds_Convenio_Impuesto_Traslado_Dominio();

        Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Td = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();

        //DataTable Dt_Impuestos_Fraccionamientos;
        DataTable Dt_Cuenta_Predial = null;
        //DataTable Dt_Tipo_Predio;
        DataTable Dt_Temp;
        DataTable Dt_Temp_Detalles = null;
        DataRow Dr_Convenios_Td;

        DataTable Dt_Traslado = Ds_Convenios_TD.Tables["Dt_Pre_Convenios_TD"];
        Convenios_Td.P_No_Convenio = Hdf_No_Convenio.Value;
        Convenios_Td.P_Mostrar_Convenio_Con_Reestructura = true;
        Dt_Temp = Convenios_Td.Consultar_Convenio_Traslado_Dominio();
        Dt_Temp_Detalles = Convenios_Td.P_Dt_Parcialidades;

        foreach (DataRow Dr_Temp in Dt_Temp.Rows)
        {
            //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
            Dr_Convenios_Td = Dt_Traslado.NewRow();
            Dr_Convenios_Td["NO_CONVENIO"] = Hdf_No_Convenio.Value;
            Dr_Convenios_Td["REALIZO"] = Dr_Temp[Ope_Pre_Convenios_Traslados_Dominio.Campo_Realizo];
            Dr_Convenios_Td["SOLICITANTE"] = Dr_Temp[Ope_Pre_Convenios_Traslados_Dominio.Campo_Solicitante];
            Dt_Traslado.Rows.Add(Dr_Convenios_Td);
        }

        Dt_Traslado = Ds_Convenios_TD.Tables["Dt_Pre_Convenios_Det_TD"];
        foreach (DataRow Dr_Temp in Dt_Temp_Detalles.Rows)
        {
            //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
            Dr_Convenios_Td = Dt_Traslado.NewRow();
            Dr_Convenios_Td["NO_PAGO"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago].ToString();
            Dr_Convenios_Td["NO_CONVENIO"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio].ToString();
            Dr_Convenios_Td["FECHA_PAGO"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Fecha_Vencimiento];
            if (Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Impuesto].ToString().Equals(""))
            {
                Dr_Convenios_Td["MONTO_IMPUESTO"] = 0.00;
            }
            else
            {
                Dr_Convenios_Td["MONTO_IMPUESTO"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Impuesto];
            }
            if (Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Ordinarios].ToString().Equals(""))
            {
                Dr_Convenios_Td["RECARGOS_ORDINARIOS"] = 0.00;
            }
            else
            {
                Dr_Convenios_Td["RECARGOS_ORDINARIOS"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Ordinarios];
            }
            if (Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Multas].ToString().Equals(""))
            {
                Dr_Convenios_Td["MONTO_MULTAS"] = 0.00;
            }
            else
            {
                Dr_Convenios_Td["MONTO_MULTAS"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Multas];
            }

            if (Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Moratorios].ToString().Equals(""))
            {
                Dr_Convenios_Td["RECARGOS_MORATORIOS"] = 0.00;
            }
            else
            {
                Dr_Convenios_Td["RECARGOS_MORATORIOS"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Moratorios];
            }
            Dr_Convenios_Td["HONORARIOS"] = 0.00;
            if (Dr_Temp["MONTO_IMPORTE"].ToString().Equals(""))
            {
                Dr_Convenios_Td["MONTO_IMPORTE"] = 0.00;
            }
            else
            {
                Dr_Convenios_Td["MONTO_IMPORTE"] = Dr_Temp["MONTO_IMPORTE"];
            }
            if (Dr_Temp["CONSTANCIA"].ToString().Equals(""))
            {
                Dr_Convenios_Td["CONSTANCIA"] = 0.00;
            }
            else
            {
                Dr_Convenios_Td["CONSTANCIA"] = Dr_Temp["CONSTANCIA"];
            }
            Dt_Traslado.Rows.Add(Dr_Convenios_Td);
        }

        Dt_Traslado = Ds_Convenios_TD.Tables["Dt_Pre_Cuentas_Predial"];

        //Inserta los datos de los Impuestos de Traslado en la Tabla
        Dr_Convenios_Td = Dt_Traslado.NewRow();
        Dr_Convenios_Td["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
        Dr_Convenios_Td["DOMICILIO"] = "COL. " + Txt_Colonia.Text + ", CALLE " + Txt_Calle.Text + ", NO EXT " + Txt_No_Exterior.Text + ", NO INT. " + Txt_No_Interior.Text;
        Dt_Traslado.Rows.Add(Dr_Convenios_Td);
        return Ds_Convenios_TD;
    }

    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        if (Hdf_No_Convenio.Value != "")
        {
            //if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[8].Text.Equals("PAGADO"))
            //{
            Imprimir_Reporte(Crear_Ds_Convenios_Traslado(), "Rpt_Convenio_TD.rpt", "Rpt_Convenio_Traslado_Dominio", "Impresion", "Convenio");
            //}
        }
    }

    #endregion

    private void Cambiar_Convenio_Incumplido()
    {
        DateTime Fecha_Vencimiento;
        DateTime Fecha_Actual;
        Fecha_Actual = DateTime.Now;
        foreach (GridViewRow Renglon_Actual in Grid_Parcialidades.Rows)
        {
            if (Renglon_Actual.Cells[9].Text == "POR PAGAR")
            {
                Fecha_Vencimiento = Convert.ToDateTime(Renglon_Actual.Cells[8].Text).AddDays(1);
                if (DateTime.Compare(Fecha_Actual, Fecha_Vencimiento) == 1)
                {
                    if (Renglon_Actual.Cells[0].Text != "1")
                    {
                        Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenio_Tras = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                        Convenio_Tras.P_No_Convenio = Hdf_No_Convenio.Value;
                        Convenio_Tras.P_No_Reestructura = Txt_Numero_Reestructura.Text;
                        Convenio_Tras.Convenio_Incumplido();
                        Cargar_Convenio();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Fraccionamiento", "alert('Reestructura de Convenio Incumplida');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Reestructura de Convenios por Fraccionamiento", "alert('La reestructura de convenio se encuentra vencida, sin embargo aún puede modificarla.');", true);
                    }
                }
                return;
            }
        }
    }
}
