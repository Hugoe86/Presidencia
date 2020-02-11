using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Operacion_Caj_Cierre_Dia.Negocio;
using System.Text;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Catalogo_Cajas.Negocio;
using Presidencia.Catalogo_Modulos.Negocio;
using Presidencia.Operacion_Pre_Caj_Detalles.Negocio;
using Presidencia.Sessiones;
using Presidencia.Folios_Inutilizados_General_Negocio;
using Presidencia.Empleados.Negocios;

public partial class paginas_Predial_Frm_Ope_Pre_Rpt_Cajas_General : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
        }
    }
    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Configuracion_Formulario
    ///DESCRIPCIÓN          : Carga una configuracion de los controles del Formulario
    ///PARAMETROS           : 1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        Llenar_Combo_Modulos();
        Cmb_Modulos.SelectedIndex = 0;
        Llenar_Combo_Cajas(Cmb_Modulos.SelectedItem.Value);
        Cmb_Cajas.SelectedIndex = 0;
        Rdb_Tipo_Reporte.SelectedValue = "1";
        Div_Contenedor_Empleado.Style.Add("visibility", "hidden");
        Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
        Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Campos
    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    ///PARAMETROS           :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Campos()
    {
        Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
        //Rdb_Tipo_Reporte.SelectedValue = "1";
        Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
        String Mensaje_Error = "";
        Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
        Div_Contenedor_Msj_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Modulos
    ///DESCRIPCIÓN: Llena el combo de modulos
    ///PROPIEDADES:         
    ///CREO: Ismael Prieto Sánchez
    ///FECHA_CREO: 19/Noviembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Modulos()
    {
        try
        {
            Cls_Cat_Pre_Modulos_Negocio Modulos = new Cls_Cat_Pre_Modulos_Negocio();
            DataTable tabla = Modulos.Consultar_Nombre_Modulos();
            Cmb_Modulos.DataSource = tabla;
            Cmb_Modulos.DataValueField = Cat_Pre_Modulos.Campo_Modulo_Id;
            Cmb_Modulos.DataTextField = Cat_Pre_Modulos.Campo_Ubicacion;
            Cmb_Modulos.DataBind();
            Cmb_Modulos.Items.Insert(0, new ListItem("GLOBAL", ""));
        }
        catch (Exception Ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Cajas
    ///DESCRIPCIÓN: Llena el combo de Cajas
    ///PROPIEDADES: Modulo_ID, pasa el id del modulo a consultar        
    ///CREO: Sergio Manuel Gallardo Andrade
    ///FECHA_CREO: 16/Octubre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Cajas(String Modulo_ID)
    {
        try
        {
            Cls_Cat_Pre_Cajas_Negocio Cajas = new Cls_Cat_Pre_Cajas_Negocio();
            Cajas.P_Modulo = Modulo_ID;
            DataTable tabla = Cajas.Consultar_Cajas_Modulo();
            Cmb_Cajas.DataSource = tabla;
            Cmb_Cajas.DataValueField = Cat_Pre_Cajas.Campo_Caja_Id;
            Cmb_Cajas.DataTextField = Cat_Pre_Cajas.Campo_Numero_De_Caja;
            Cmb_Cajas.DataBind();
            Cmb_Cajas.Items.Insert(0, new ListItem("GLOBAL", ""));
        }
        catch (Exception Ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
        }
    }

    private void Reporte_General_Cajas()
    {
        DataSet Ds_Reporte = null;
        DataTable Dt_Pagos = null;
        DataView Dt_Filtro = null;
        int Total = 0;
        Cls_Ope_Pre_Caj_Detalles_Negocio Caj_General = new Cls_Ope_Pre_Caj_Detalles_Negocio();

        Ds_Reporte = new DataSet();
        Caj_General.P_Modulo_Id = Cmb_Modulos.SelectedItem.Value;
        Caj_General.P_Caja_Id = Cmb_Cajas.SelectedValue;
        Caj_General.P_Fecha = Txt_Fecha_Inicial.Text;
        Caj_General.P_Fecha_Final = Txt_Fecha_Final.Text;
        Dt_Pagos = Caj_General.Consulta_Pagos_General();
        if (Dt_Pagos.Rows.Count > 0)
        {
            Dt_Pagos.TableName = "DT_Datos_Generales";
            // en esta parte agregamos un dataview para obtener el total de los recibos generados en el cajero
            Dt_Filtro = Dt_Pagos.DefaultView;
            Total = Dt_Filtro.ToTable(true, "NO_OPERACION").Rows.Count;
            // reasignamos los valores a las columnas del datatable dt_pagos para poner la iinformacion correcta en el rpt
            foreach (DataRow DR in Dt_Pagos.Rows)
            {
                DR["TOTAL_RECIBOS"] = Total.ToString();
                DR["FECHA"] = Convert.ToDateTime(Txt_Fecha_Inicial.Text);// String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicial.Text));
                DR["FECHA_FINAL"] = Convert.ToDateTime(Txt_Fecha_Final.Text);// String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Final.Text));
            }
            Ds_Reporte.Tables.Add(Dt_Pagos.Copy());
            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Pagos_General_Enc.rpt", "Reporte_Caja_General_ENC_" + Session.SessionID + ".pdf", ".pdf");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('La Caja(s) no tiene movimientos los dia del " + Txt_Fecha_Inicial.Text + "al " + Txt_Fecha_Final.Text + "');", true);
        }
    }

    private void Reporte_Detallado_Cajas()
    {
        DataSet Ds_Reporte = null;
        DataTable Dt_Pagos = null;
        Cls_Ope_Pre_Caj_Detalles_Negocio Caj_Detalles = new Cls_Ope_Pre_Caj_Detalles_Negocio();

        Ds_Reporte = new DataSet();
        Caj_Detalles.P_Caja_Id = Cmb_Cajas.SelectedValue;
        Caj_Detalles.P_Fecha = Txt_Fecha_Inicial.Text;
        Dt_Pagos = Caj_Detalles.Consulta_Pagos();
        if (Dt_Pagos.Rows.Count > 0)
        {
            Dt_Pagos.TableName = "Dt_CAJA_PAGOS";
            Ds_Reporte.Tables.Add(Dt_Pagos.Copy());
            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Pagos_Detalle_Enc.rpt", "Reporte_Caja_DetalleENC_" + Session.SessionID + ".pdf", ".pdf");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('La Caja(s) no  tiene movimientos el dia " + Txt_Fecha_Inicial.Text + "');", true);
        }
    }

    private void Reporte_Cancelaciones_Cajas()
    {
        DataSet Ds_Reporte = null;
        DataTable Dt_Pagos = null;
        Cls_Ope_Pre_Caj_Detalles_Negocio Caj_Detalles = new Cls_Ope_Pre_Caj_Detalles_Negocio();

        Ds_Reporte = new DataSet();
        Caj_Detalles.P_Modulo_Id = Cmb_Modulos.SelectedItem.Value;
        Caj_Detalles.P_Caja_Id = Cmb_Cajas.SelectedValue;
        Caj_Detalles.P_Fecha = Txt_Fecha_Inicial.Text;
        Caj_Detalles.P_Estatus = "CANCELADOS";
        Dt_Pagos = Caj_Detalles.Consulta_Pagos();
        if (Dt_Pagos.Rows.Count > 0)
        {
            Dt_Pagos.TableName = "Dt_CAJA_PAGOS";
            Ds_Reporte.Tables.Add(Dt_Pagos.Copy());
            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Pagos_Cancelados_Detalle_Enc.rpt", "Reporte_Caja_Cancelados" + Session.SessionID + ".pdf", ".pdf");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('La Caja(s) no  tiene movimientos el dia " + Txt_Fecha_Inicial.Text + "');", true);
        }
    }

    private void Reporte_Recibos_Cancelados_Empleado()
    {
        DataSet Ds_Reporte = null;
        DataTable Dt_Consulta = null;
        Cls_Ope_Pre_Caj_Detalles_Negocio Rs_Consulta = new Cls_Ope_Pre_Caj_Detalles_Negocio();

        Ds_Reporte = new DataSet();
        Rs_Consulta.P_Fecha = Txt_Fecha_Inicial.Text;
        Rs_Consulta.P_Empleado_ID = HF_Empleado_ID.Value;
        Dt_Consulta = Rs_Consulta.Reporte_Recibos_Cancelados_Empleado();
        if (Dt_Consulta.Rows.Count > 0)
        {
            Dt_Consulta.TableName = "Dt_Cancelados";
            Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Cancelados_Cajeros.rpt", "Reporte_Caja_Cancelados" + Session.SessionID + ".pdf", ".pdf");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('La Caja(s) no  tiene movimientos el dia " + Txt_Fecha_Inicial.Text + "');", true);
        }
    }

    private void Reporte_Desglosada_Tarjeta_Bancaria()
    {
        Cls_Ope_Pre_Caj_Detalles_Negocio Caj_Tarjeta_Bancaria = new Cls_Ope_Pre_Caj_Detalles_Negocio();
        DataSet Ds_Reporte = new DataSet();
        DataTable Dt_Consulta;

        Caj_Tarjeta_Bancaria.P_Modulo_Id = Cmb_Modulos.SelectedItem.Value;
        Caj_Tarjeta_Bancaria.P_Caja_Id = Cmb_Cajas.SelectedItem.Value;
        Caj_Tarjeta_Bancaria.P_Fecha = Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString();
        Caj_Tarjeta_Bancaria.P_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text).ToString();
        Dt_Consulta = Caj_Tarjeta_Bancaria.Reporte_Desglosado_Tarjeta_Bancaria();
        if (Dt_Consulta.Rows.Count > 0)
        {
            Dt_Consulta.TableName = "Pagos_Tarjeta_Bancaria";
            Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Caj_Pagos_Tarjeta_Desglosado.rpt", "Reporte_Desglosado_Tarjeta_Bancaria_" + Session.SessionID + ".pdf", ".pdf");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('No existen registros con los filtros de búsqueda asignados');", true);
        }
    }

    private void Reporte_Concentracion_Monetaria()
    {
        Cls_Ope_Pre_Caj_Detalles_Negocio Caj_Det = new Cls_Ope_Pre_Caj_Detalles_Negocio();
        DataTable Dt_Consulta = null;
        DataSet Ds_Reporte = new DataSet();

        Caj_Det.P_Modulo_Id = Cmb_Modulos.SelectedItem.Value;
        Caj_Det.P_Caja_Id = Cmb_Cajas.SelectedItem.Value;
        Caj_Det.P_Fecha = Txt_Fecha_Inicial.Text;
        Caj_Det.P_Fecha_Final = Txt_Fecha_Final.Text;
        Dt_Consulta = Caj_Det.Reporte_Concentracion_Monetarea();
        if (Dt_Consulta.Rows.Count > 0)
        {
            Dt_Consulta.TableName = "Dt_Pagos_Detalles";
            Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Concentracion_Monetarea.rpt", "Reporte_Concentracion_Monetaria_" + Session.SessionID + ".pdf", ".pdf");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('No existen registros con los filtros de búsqueda asignados');", true);
        }
    }

    private void Reporte_Concentrado_Tarjeta_Bancaria()
    {
        Cls_Ope_Pre_Caj_Detalles_Negocio Caj_Det = new Cls_Ope_Pre_Caj_Detalles_Negocio();
        DataTable Dt_Consulta = null;
        DataSet Ds_Reporte = new DataSet();

        Caj_Det.P_Modulo_Id = Cmb_Modulos.SelectedItem.Value;
        Caj_Det.P_Caja_Id = Cmb_Cajas.SelectedItem.Value;
        Caj_Det.P_Fecha = Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString();
        Caj_Det.P_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text).ToString();
        Dt_Consulta = Caj_Det.Reporte_Pagos_tarjetas();
        if (Dt_Consulta.Rows.Count > 0)
        {
            Dt_Consulta.TableName = "Dt_Detalles";
            Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_relacion_Pagos_Tarjeta_Bancaria.rpt", "Reporte_Concentrado_Tarjeta_Bancaria_" + Session.SessionID + ".pdf", ".pdf");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('No existen registros con los filtros de búsqueda asignados');", true);
        }
    }

    private void Reporte_Detallado_Pagos_Con_Tarjeta()
    {
        Cls_Ope_Pre_Caj_Detalles_Negocio Reporte_Detalle_Pagos_Tarjeta = new Cls_Ope_Pre_Caj_Detalles_Negocio();
        DataTable Dt_Consulta = null;
        DataSet Ds_Reporte = new DataSet();

        Reporte_Detalle_Pagos_Tarjeta.P_Modulo_Id = Cmb_Modulos.SelectedItem.Value;
        Reporte_Detalle_Pagos_Tarjeta.P_Caja_Id = Cmb_Cajas.SelectedItem.Value;
        Reporte_Detalle_Pagos_Tarjeta.P_Fecha = Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString();
        Reporte_Detalle_Pagos_Tarjeta.P_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text).ToString();
        Dt_Consulta = Reporte_Detalle_Pagos_Tarjeta.Reporte_Detallado_Pagos_Tarjeta();
        if (Dt_Consulta.Rows.Count > 0)
        {
            Dt_Consulta.TableName = "Dt_Detalles";
            Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Detalle_Pagos_Tarjeta.rpt", "Reporte_Det_Tarjeta_Bancaria_" + Session.SessionID + ".pdf", ".pdf");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('No existen registros con los filtros de búsqueda asignados');", true);
        }
    }

    private void Reporte_Detallado_Pagos_Con_Cheque()
    {
        Cls_Ope_Pre_Caj_Detalles_Negocio Reporte_Detalle_Pagos_Cheque = new Cls_Ope_Pre_Caj_Detalles_Negocio();
        DataTable Dt_Consulta = null;
        DataSet Ds_Reporte = new DataSet();

        Reporte_Detalle_Pagos_Cheque.P_Modulo_Id = Cmb_Modulos.SelectedItem.Value;
        Reporte_Detalle_Pagos_Cheque.P_Caja_Id = Cmb_Cajas.SelectedItem.Value;
        Reporte_Detalle_Pagos_Cheque.P_Fecha = Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString();
        Reporte_Detalle_Pagos_Cheque.P_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text).ToString();
        Dt_Consulta = Reporte_Detalle_Pagos_Cheque.Reporte_Detallado_Pagos_Cheque();
        if (Dt_Consulta.Rows.Count > 0)
        {
            Dt_Consulta.TableName = "Dt_Detalles";
            Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Detalle_Pagos_Cheque.rpt", "Reporte_Det_Cheque_" + Session.SessionID + ".pdf", ".pdf");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('No existen registros con los filtros de búsqueda asignados');", true);
        }
    }

    private void Reporte_Detallado_Pagos_Con_Transferencia()
    {
        Cls_Ope_Pre_Caj_Detalles_Negocio Reporte_Detalle_Pagos_Transferencia = new Cls_Ope_Pre_Caj_Detalles_Negocio();
        DataTable Dt_Consulta = null;
        DataSet Ds_Reporte = new DataSet();

        Reporte_Detalle_Pagos_Transferencia.P_Modulo_Id = Cmb_Modulos.SelectedItem.Value;
        Reporte_Detalle_Pagos_Transferencia.P_Caja_Id = Cmb_Cajas.SelectedItem.Value;
        Reporte_Detalle_Pagos_Transferencia.P_Fecha = Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString();
        Reporte_Detalle_Pagos_Transferencia.P_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text).ToString();
        Dt_Consulta = Reporte_Detalle_Pagos_Transferencia.Reporte_Detallado_Pagos_Transferencia();
        if (Dt_Consulta.Rows.Count > 0)
        {
            Dt_Consulta.TableName = "Dt_Detalles";
            Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Detalle_Pagos_Transferencia.rpt", "Reporte_Det_Transferencia_" + Session.SessionID + ".pdf", ".pdf");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('No existen registros con los filtros de búsqueda asignados');", true);
        }
    }

    private void Reporte_Folios_Inutilizados()
    {
        Cls_Ope_Caj_Folios_Inutilizados_General_Negocio Folios_Negocio = new Cls_Ope_Caj_Folios_Inutilizados_General_Negocio();
        Ds_Ope_Caj_Folios_Inutilizados_General Ds_Folios = new Ds_Ope_Caj_Folios_Inutilizados_General();
        DataTable Dt_Folios = new DataTable();
        try
        {
            //obtenemos los valores de las cajas de texto
            if (Cmb_Modulos.SelectedIndex > 0)
            {
                Folios_Negocio.P_Modulo_ID = Cmb_Modulos.SelectedValue;
            }
            if (Cmb_Cajas.SelectedIndex > 0)
            {
                Folios_Negocio.P_Caja_ID = Cmb_Cajas.SelectedValue;
            }
            if (!string.IsNullOrEmpty(Txt_Empleado.Text))
            {
                Folios_Negocio.P_Empleado_ID = HF_Empleado_ID.Value;
            }
            if (!string.IsNullOrEmpty(Txt_Fecha_Inicial.Text))
            {
                Folios_Negocio.P_Fecha_Inicio = string.Format("{0:dd/MM/yyyy}", Txt_Fecha_Inicial.Text);
            }
            if (!string.IsNullOrEmpty(Txt_Fecha_Final.Text))
            {
                Folios_Negocio.P_Fecha_Fin = string.Format("{0:dd/MM/yyyy}", Txt_Fecha_Final.Text);
            }
            Dt_Folios = Folios_Negocio.Consultar_Folio();

            if (Dt_Folios.Rows.Count > 0)
            {
                Dt_Folios.TableName = "Dt_Folios_Inutilizados";
                Ds_Folios.Clear();
                Ds_Folios.Tables.Clear();
                Ds_Folios.Tables.Add(Dt_Folios.Copy());
                Imprimir_Reporte(Ds_Folios, "Rpt_Ope_Caj_Folios_Inutilizados_General.rpt", "Reporte de Folios Inutilizados");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('No existen registros con los filtros de búsqueda asignados');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error en el evento del boton de imprimir Error:[" + Ex.Message + "]");
        }
    }

    private void Reporte_Concentracion_Ingreso()
    {
        Cls_Ope_Pre_Caj_Detalles_Negocio Caj_Det = new Cls_Ope_Pre_Caj_Detalles_Negocio();
        DataTable Dt_Consulta = null;
        DataSet Ds_Reporte = new DataSet();

        Caj_Det.P_Modulo_Id = Cmb_Modulos.SelectedItem.Value;
        Caj_Det.P_Caja_Id = Cmb_Cajas.SelectedItem.Value;
        Caj_Det.P_Fecha = String.Format("{0:dd/MM/yyyy}", Txt_Fecha_Inicial.Text);
        Caj_Det.P_Fecha_Final = String.Format("{0:dd/MM/yyyy}", Txt_Fecha_Final.Text);
        Dt_Consulta = Caj_Det.Reporte_Concentracion_Ingreso();
        if (Dt_Consulta.Rows.Count > 0)
        {
            Dt_Consulta.TableName = "Dt_Detalles";
            Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_relacion_Pagos_Tarjeta_Bancaria.rpt", "Reporte_Concentrado_Tarjeta_Bancaria_" + Session.SessionID + ".pdf", ".pdf");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('No existen registros con los filtros de búsqueda asignados');", true);
        }
    }

    private void Consultar_Resumen_Diario_Ingresos()
    {
        Cls_Ope_Pre_Caj_Detalles_Negocio Caj_Det = new Cls_Ope_Pre_Caj_Detalles_Negocio();
        DataTable Dt_Consulta = null;
        DataSet Ds_Reporte = new DataSet();
        Int32 Filas = 0;
        Double Recaudacion_Dias = 0;
        Double Recaudacion_Mes = 0;
        Double Recaudacion_Anio = 0;
        Double Presupuesto_Anual = 0;
        Double Porcentaje = 0;
        Double Presupuesto_Ejercer = 0;
        Dt_Consulta = Caj_Det.Consultar_Resumen_Diario_Ingresos();
        if (Dt_Consulta.Rows.Count > 0)
        {
            foreach (DataRow Dr_Renglon in Dt_Consulta.Rows)
            {
                if (Dr_Renglon["RECAUDACION_DIA"].ToString() == "")
                {
                    Dr_Renglon["RECAUDACION_DIA"] = "0.00";
                }
                else
                {
                    Dr_Renglon["RECAUDACION_DIA"] = Convert.ToDouble(Dr_Renglon["RECAUDACION_DIA"].ToString()).ToString("#,###,###,###,###,###,##0.00");
                }
                if (Dr_Renglon["RECAUDACION_MES"].ToString() == "")
                {
                    Dr_Renglon["RECAUDACION_MES"] = "0.00";
                }
                else
                {
                    Dr_Renglon["RECAUDACION_MES"] = Convert.ToDouble(Dr_Renglon["RECAUDACION_MES"].ToString()).ToString("#,###,###,###,###,###,##0.00");
                }
                if (Dr_Renglon["RECAUDACION_ANIO"].ToString() == "")
                {
                    Dr_Renglon["RECAUDACION_ANIO"] = "0.00";
                }
                else
                {
                    Dr_Renglon["RECAUDACION_ANIO"] = Convert.ToDouble(Dr_Renglon["RECAUDACION_ANIO"].ToString()).ToString("#,###,###,###,###,###,##0.00");
                }
                if (Dr_Renglon["PRESUPUESTO_ANIO"].ToString() == "")
                {
                    Dr_Renglon["PRESUPUESTO_ANIO"] = "0.00";
                }
                else
                {
                    Dr_Renglon["PRESUPUESTO_ANIO"] = Convert.ToDouble(Dr_Renglon["PRESUPUESTO_ANIO"].ToString()).ToString("#,###,###,###,###,###,##0.00");
                }
                if (Dr_Renglon["PRESUPUESTO_ANIO"].ToString() == "0.00")
                {
                    if (Dr_Renglon["RECAUDACION_ANIO"].ToString() == "0.00")
                    {
                        Dr_Renglon["PORCENTAJE"] = "0.00";
                    }
                    else
                    {
                        Dr_Renglon["PORCENTAJE"] = "-100.00";
                    }
                }
                else
                {
                    Dr_Renglon["PORCENTAJE"] = (((Convert.ToDouble(Dr_Renglon["PRESUPUESTO_ANIO"].ToString()) - Convert.ToDouble(Dr_Renglon["RECAUDACION_ANIO"].ToString())) / Convert.ToDouble(Dr_Renglon["PRESUPUESTO_ANIO"].ToString())) * 100).ToString("#,###,###,###,###,###,##0.00");
                }
                Dr_Renglon["PRESUPUESTO_EJERCER"] = (Convert.ToDouble(Dr_Renglon["PRESUPUESTO_ANIO"].ToString()) - Convert.ToDouble(Dr_Renglon["RECAUDACION_ANIO"].ToString())).ToString("#,###,###,###,###,###,##0.00");
                //if (Dr_Renglon["DESCRIPCION"].ToString() == "")
                //{
                //    Dr_Renglon.Delete();
                //}
                //else
                //{
                Recaudacion_Dias += Convert.ToDouble(Dr_Renglon["RECAUDACION_DIA"].ToString());
                Recaudacion_Mes += Convert.ToDouble(Dr_Renglon["RECAUDACION_MES"].ToString());
                Recaudacion_Anio += Convert.ToDouble(Dr_Renglon["RECAUDACION_ANIO"].ToString());
                Presupuesto_Anual += Convert.ToDouble(Dr_Renglon["PRESUPUESTO_ANIO"].ToString());
                Porcentaje += Convert.ToDouble(Dr_Renglon["PORCENTAJE"].ToString());
                Presupuesto_Ejercer += Convert.ToDouble(Dr_Renglon["PRESUPUESTO_EJERCER"].ToString());
                Filas++;
                //}
            }
            DataTable Dt_Totales = new DataTable();
            Dt_Totales.Columns.Add("RECAUDACION_DIA", typeof(String));
            Dt_Totales.Columns.Add("RECAUDACION_MES", typeof(String));
            Dt_Totales.Columns.Add("RECAUDACION_ANIO", typeof(String));
            Dt_Totales.Columns.Add("PRESUPUESTO_ANIO", typeof(String));
            Dt_Totales.Columns.Add("PORCENTAJE", typeof(String));
            Dt_Totales.Columns.Add("PRESUPUESTO_EJERCER", typeof(String));
            Dt_Totales.Columns.Add("TIPO", typeof(String));
            DataRow Dr_Totales = Dt_Totales.NewRow();
            Dr_Totales["RECAUDACION_DIA"] = Recaudacion_Dias.ToString("#,###,###,###,###,###,##0.00");
            Dr_Totales["RECAUDACION_MES"] = Recaudacion_Mes.ToString("#,###,###,###,###,###,##0.00");
            Dr_Totales["RECAUDACION_ANIO"] = Recaudacion_Anio.ToString("#,###,###,###,###,###,##0.00");
            Dr_Totales["PRESUPUESTO_ANIO"] = Presupuesto_Anual.ToString("#,###,###,###,###,###,##0.00");
            Dr_Totales["PORCENTAJE"] = (Porcentaje / Filas).ToString("#,###,###,###,###,###,##0.00");
            Dr_Totales["PRESUPUESTO_EJERCER"] = Presupuesto_Ejercer.ToString("#,###,###,###,###,###,##0.00");
            Dr_Totales["TIPO"] = "RESUMEN_DIARIO_INGRESOS";
            Dt_Totales.Rows.Add(Dr_Totales);
            Dt_Consulta.TableName = "Dt_Ope_Caj_Resumen_Diario_Ingresos";
            Dt_Totales.TableName = "Dt_Ope_Caj_Resumen_Diario_Ingresos_Totales";
            Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
            Ds_Reporte.Tables.Add(Dt_Totales.Copy());
            ////Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Resumen_Diario_Ingresos.rpt", "Resumen_Diario_Ingresos_" + Session.SessionID + ".pdf", ".pdf");

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('No existen registros de ingresos');", true);
        }
    }

    private void Consultar_Analisis_Entrega_Dia()
    {
        Cls_Ope_Pre_Caj_Detalles_Negocio Caj_Det = new Cls_Ope_Pre_Caj_Detalles_Negocio();
        DataTable Dt_Consulta = null;
        DataSet Ds_Reporte = new DataSet();
        Caj_Det.P_Caja_Id = Cmb_Cajas.SelectedValue;
        Caj_Det.P_Fecha = Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString("dd/MM/yyyy");
        Caj_Det.P_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text).ToString("dd/MM/yyyy");
        DataRow Dr_Cabecera;
        DataTable Dt_Cabecera = new DataTable();
        Dt_Cabecera.Columns.Add("FECHAS", typeof(String));
        Dt_Consulta = Caj_Det.Consultar_Analisis_Entrega_Dia();
        if (Dt_Consulta.Rows.Count > 0)
        {
            Dr_Cabecera = Dt_Cabecera.NewRow();
            Dr_Cabecera["FECHAS"] = Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString("dd") +
                " DE " + Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString("MMMM").ToUpper() +
                " " + Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString("yyyy") +
                " AL " + Convert.ToDateTime(Txt_Fecha_Final.Text).ToString("dd") +
                " DE " + Convert.ToDateTime(Txt_Fecha_Final.Text).ToString("MMMM").ToUpper() +
                " " + Convert.ToDateTime(Txt_Fecha_Final.Text).ToString("yyyy");
            Dt_Cabecera.Rows.Add(Dr_Cabecera);
            Dt_Consulta.TableName = "Dt_Ope_Caj_Analisis_Entrega_Dia";
            Dt_Cabecera.TableName = "Dt_Cabecera";
            Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
            Ds_Reporte.Tables.Add(Dt_Cabecera.Copy());
            ////Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Analisis_Entrega_Dia.rpt", "Analisis_Entrega_Por_Dia_" + Session.SessionID + ".pdf", ".pdf");

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('No existen registros de ingresos');", true);
        }
    }

    private void Consultar_Corte_Caja()
    {
        Cls_Ope_Pre_Caj_Detalles_Negocio Caj_Det = new Cls_Ope_Pre_Caj_Detalles_Negocio();
        DataTable Dt_Consulta = null;
        DataSet Ds_Reporte = new DataSet();
        Caj_Det.P_Fecha = Convert.ToDateTime(Txt_Fecha_Inicial.Text).ToString("dd/MM/yyyy");
        Caj_Det.P_Caja_Id = Cmb_Cajas.SelectedValue;
        Dt_Consulta = Caj_Det.Consultar_Corte_Caja();
        if (Dt_Consulta.Rows.Count > 0)
        {
            foreach (DataRow Dr_Renglon in Dt_Consulta.Rows)
            {
                Dr_Renglon["NO_OPERACION"] = Dr_Renglon["NO_OPERACION"].ToString().Replace(".00", "");
                Dr_Renglon["NO_RECIBO"] = Dr_Renglon["NO_RECIBO"].ToString().Replace(".00", "");
                Dr_Renglon["NO_CAJA"] = Dr_Renglon["NO_CAJA"].ToString().Replace(".00", "");
                Dr_Renglon["IMPORTE"] = Convert.ToDouble(Dr_Renglon["IMPORTE"].ToString()).ToString("#,###,###,###,###,###,##0.00");
                Dr_Renglon["REZAGO"] = Convert.ToDouble(Dr_Renglon["REZAGO"].ToString()).ToString("#,###,###,###,###,###,##0.00");
                Dr_Renglon["RECARGOS"] = Convert.ToDouble(Dr_Renglon["RECARGOS"].ToString()).ToString("#,###,###,###,###,###,##0.00");
                Dr_Renglon["HONORARIOS"] = Convert.ToDouble(Dr_Renglon["HONORARIOS"].ToString()).ToString("#,###,###,###,###,###,##0.00");
                Dr_Renglon["DESCUENTOS"] = Convert.ToDouble(Dr_Renglon["DESCUENTOS"].ToString()).ToString("#,###,###,###,###,###,##0.00");
                Dr_Renglon["TOTAL"] = Convert.ToDouble(Dr_Renglon["TOTAL"].ToString()).ToString("#,###,###,###,###,###,##0.00");
            }
            Dt_Consulta.TableName = "Dt_Ope_Caj_Corte_Caja";
            Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
            ////Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Caj_Corte_Caja.rpt", "Corte_Caja_" + Session.SessionID + ".pdf", ".pdf");

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reportes", "alert('No existen registros de ingresos');", true);
        }
    }
    #endregion

    #region "Metodos Empleados"
    //*******************************************************************************
    //NOMBRE DE LA FUNCIÓN : Llena_Grid_Empleados
    //DESCRIPCIÓN          : Metodo para llenar el grid con los empleados encontrados en la busqueda
    //PARAMETROS           :   
    //CREO                 : Leslie González Vázquez
    //FECHA_CREO           : 26/octubre/2011 
    //MODIFICO             :
    //FECHA_MODIFICO       :
    //CAUSA_MODIFICACIÓN   :
    //*******************************************************************************
    private void Llena_Grid_Empleados()
    {
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Empleados.Columns[1].Visible = true;
            Grid_Empleados.DataBind();
            Dt_Empleados = (DataTable)Session["Consulta_Empleados"];
            Grid_Empleados.DataSource = Dt_Empleados;
            Grid_Empleados.DataBind();
            Grid_Empleados.Columns[1].Visible = false;
            Grid_Empleados.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Empleados " + ex.Message.ToString(), ex);
        }
    }

    //*******************************************************************************
    //NOMBRE DE LA FUNCIÓN : Validar_Busqueda
    //DESCRIPCIÓN          : Metodo para validar los datos de la busqueda del empleado
    //PARAMETROS           :   
    //CREO                 : Leslie González Vázquez
    //FECHA_CREO           : 26/octubre/2011 
    //MODIFICO             :
    //FECHA_MODIFICO       :
    //CAUSA_MODIFICACIÓN   :
    //*******************************************************************************
    private Boolean Validar_Busqueda()
    {
        Boolean Datos_Validos;
        Datos_Validos = true;
        try
        {
            if (string.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text) && string.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text))
            {
                Lbl_Error_Busqueda.Text = "Por favor introduce un No. Empleado o el nombre del Empleado para hacer la busqueda";
                Lbl_Error_Busqueda.Style.Add("display", "block");
                Img_Error_Busqueda.Style.Add("display", "block");
                Datos_Validos = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar los datos de la busqueda Error:[" + Ex.Message + "]");
        }
        return Datos_Validos;
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Cajas/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds);
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
            Mostrar_Reporte(Archivo_PDF);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    //*******************************************************************************
    //NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
    //DESCRIPCIÓN          : Metodo para mostrar el reporte
    //PARAMETROS           1 Nombre_Reporte: Nombre que tiene el reporte que se mostrara en pantalla.   
    //CREO                 : Leslie González Vázquez
    //FECHA_CREO           : 25/octubre/2011 
    //MODIFICO             :
    //FECHA_MODIFICO       :
    //CAUSA_MODIFICACIÓN   :
    //*******************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Folios_Inutilizados",
                "window.open('" + Pagina + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    //*******************************************************************************
    //NOMBRE DE LA FUNCIÓN : Btn_Busqueda_Empleados_Click
    //DESCRIPCIÓN          : Evento del boton de busquedas de empleados
    //PARAMETROS           :   
    //CREO                 : Leslie González Vázquez
    //FECHA_CREO           : 26/octubre/2011 
    //MODIFICO             :
    //FECHA_MODIFICO       :
    //CAUSA_MODIFICACIÓN   :
    //*******************************************************************************
    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios Rs_Consulta_Ca_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
        Int32 No_Letras = 0;
        String No_Empleado = String.Empty;

        try
        {
            if (Validar_Busqueda())
            {
                if (!string.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text.Trim()))
                {
                    No_Letras = Txt_Busqueda_No_Empleado.Text.Trim().Length; //obtenemos el no de letras que tiene el numero de empleado 

                    if (No_Letras < 6)
                    {
                        for (Int32 i = 0; i < 6 - No_Letras; i++)
                        {
                            No_Empleado += "0";
                        }
                    }
                    Rs_Consulta_Ca_Empleados.P_No_Empleado = No_Empleado + Txt_Busqueda_No_Empleado.Text.Trim();
                }
                if (!string.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text.Trim())) Rs_Consulta_Ca_Empleados.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim();
                Rs_Consulta_Ca_Empleados.P_Estatus = "ACTIVO";
                Dt_Empleados = Rs_Consulta_Ca_Empleados.Consulta_Empleados_General(); //Consulta todos los Empleados que coindican con lo proporcionado por el usuario
                Session["Consulta_Empleados"] = Dt_Empleados;
                Llena_Grid_Empleados();
                Mpe_Busqueda_Empleados.Show();
                Lbl_Error_Busqueda.Style.Add("display", "none");
                Img_Error_Busqueda.Style.Add("display", "none");

                if (Dt_Empleados is DataTable)
                    Lbl_Numero_Registros.Text = "Registros Encontrados: [" + Dt_Empleados.Rows.Count + "]";
                else
                    Lbl_Numero_Registros.Text = "Registros Encontrados: [0]";
            }
            else
            {
                Mpe_Busqueda_Empleados.Show();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Empleados " + ex.Message.ToString(), ex);
        }
    }

    //*******************************************************************************
    //NOMBRE DE LA FUNCIÓN : Grid_Empleados_SelectedIndexChanged
    //DESCRIPCIÓN          : Evento de seleccion de un registro del del grid
    //PARAMETROS           :   
    //CREO                 : Leslie González Vázquez
    //FECHA_CREO           : 26/octubre/2011 
    //MODIFICO             :
    //FECHA_MODIFICO       :
    //CAUSA_MODIFICACIÓN   :
    //*******************************************************************************
    protected void Grid_Empleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error_Busqueda.Visible = false;
            HF_Empleado_ID.Value = Grid_Empleados.SelectedRow.Cells[1].Text;
            Txt_Empleado.Text = Grid_Empleados.SelectedRow.Cells[3].Text;
            Mpe_Busqueda_Empleados.Hide();
            Lbl_Error_Busqueda.Text = "";
            Lbl_Error_Busqueda.Style.Add("display", "none");
            Img_Error_Busqueda.Style.Add("display", "none");
            Txt_Busqueda_Nombre_Empleado.Text = "";
            Txt_Busqueda_No_Empleado.Text = "";
            Grid_Empleados.DataBind();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error_Busqueda.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region Validaciones
    /// ********************************************************************************
    /// Nombre: Validar_Fechas
    /// Descripción: Valida que la Fecha Inicial no sea mayor que la Final
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 3/Mayo/2011 12:20p.m.
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private Boolean Validar_Fechas(String _Fecha_Inicio, String _Fecha_Fin)
    {
        DateTime Fecha_Inicio = Convert.ToDateTime(_Fecha_Inicio);
        DateTime Fecha_Fin = Convert.ToDateTime(_Fecha_Fin);
        Boolean Fecha_Valida = false;
        if (Fecha_Inicio <= Fecha_Fin) Fecha_Valida = true;
        return Fecha_Valida;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        String fecha = DateTime.Now.ToString();
        if (Convert.ToInt32(Rdb_Tipo_Reporte.SelectedValue) == 1)
        {
            if (Txt_Fecha_Final.Text.Trim() == "__/___/____")
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introduzca la Fecha Termino.";
                Validacion = false;
            }
            else
            {
                if (!Validar_Fechas(Txt_Fecha_Inicial.Text, Txt_Fecha_Final.Text))
                {
                    Mensaje_Error = Mensaje_Error + "+ Fecha inicial no puede ser mayor que la Fecha Termino.";
                    Validacion = false;
                }
            }
        }
        if (Txt_Fecha_Inicial.Text.Trim() == "__/___/____")
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introduzca la fecha .";
            Validacion = false;
        }
        //if (Cmb_Cajas.SelectedIndex <= 0)
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Selecciona la caja a consultar .";
        //    Validacion = false;
        //}
        if (Txt_Fecha_Inicial.Text.Trim() != "__/___/____")
        {
            if (!Validar_Fechas(Txt_Fecha_Inicial.Text, fecha))
            {
                Mensaje_Error = Mensaje_Error + "+ Fecha no puede ser mayor que la Fecha Actual. <br>";
                Validacion = false;
            }
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }
    #endregion
    #region "Evento"
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta 
    ///PROPIEDADES:     
    ///CREO: Sergio Manuel Gallardo
    ///FECHA_CREO: 11/octubre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Validar_Componentes())
            {
                switch (Convert.ToInt32(Rdb_Tipo_Reporte.SelectedValue))
                {
                    case 1:
                        Reporte_General_Cajas();
                        break;

                    case 2:
                        Reporte_Detallado_Cajas();
                        break;

                    case 3:
                        Reporte_Cancelaciones_Cajas();
                        break;

                    case 4:
                        Reporte_Folios_Inutilizados();
                        break;

                    case 5:
                        Reporte_Concentracion_Monetaria();
                        break;

                    case 6:
                        Reporte_Concentrado_Tarjeta_Bancaria();
                        break;

                    case 7:
                        Reporte_Desglosada_Tarjeta_Bancaria();
                        break;

                    case 8:
                        Reporte_Concentracion_Ingreso();
                        break;

                    case 9:
                        Consultar_Corte_Caja();
                        break;

                    case 10:
                        Consultar_Resumen_Diario_Ingresos();
                        break;

                    case 11:
                        Consultar_Analisis_Entrega_Dia();
                        break;

                    case 12:
                        Reporte_Detallado_Pagos_Con_Tarjeta();
                        break;

                    case 13:
                        Reporte_Detallado_Pagos_Con_Cheque();
                        break;

                    case 14:
                        Reporte_Detallado_Pagos_Con_Transferencia();
                        break;

                    case 15:
                        Reporte_Recibos_Cancelados_Empleado();
                        break;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
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
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Rdb_Tipo_Reporte_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Rdb_Tipo_Reporte_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Empleado.Style.Add("visibility", "hidden");
        switch (Convert.ToInt32(Rdb_Tipo_Reporte.SelectedValue))
        {
            case 4:
                Div_Contenedor_Empleado.Style.Add("visibility", "visible");
                break;
        }
    }

    protected void Cmb_Modulos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Combo_Cajas(Cmb_Modulos.SelectedItem.Value);
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
        ParameterFieldDefinitions crParameterFieldDefinitions;
        ParameterValues crParameterValues;
        ParameterDiscreteValue crParameterDiscreteValue;
        String Rango = "";
        switch (Convert.ToInt32(Rdb_Tipo_Reporte.SelectedValue))
        {
            case 15:
                Rango = "AÑO " + Convert.ToDateTime(Txt_Fecha_Inicial.Text).Year.ToString();
                break;

            default:
                Rango = "DEL " + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicial.Text)) + " AL " + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Txt_Fecha_Final.Text));
                break;
        }

        try
        {
            Ruta = @Server.MapPath("../Rpt/Cajas/" + Ruta_Reporte_Crystal);
            Reporte.Load(Ruta);

            if (Ds_Reporte_Crystal is DataSet)
            {
                if (Ds_Reporte_Crystal.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Reporte_Crystal);
                    //Declara y asigna valores a los parametros
                    crParameterFieldDefinitions = Reporte.DataDefinition.ParameterFields;
                    foreach (ParameterFieldDefinition crParameterFieldDefinition in crParameterFieldDefinitions)
                    {
                        //Crea un nuevo valor de parametro
                        crParameterDiscreteValue = new ParameterDiscreteValue();
                        //Asigna el parametro
                        crParameterValues = crParameterFieldDefinition.CurrentValues;

                        //Aplica los valores cargados
                        switch (crParameterFieldDefinition.Name)
                        {
                            case "Modulo":
                                crParameterDiscreteValue.Value = Cmb_Modulos.SelectedItem.Text;
                                crParameterValues.Add(crParameterDiscreteValue);
                                break;

                            case "Rango_Fechas":
                                crParameterDiscreteValue.Value = Rango;
                                crParameterValues.Add(crParameterDiscreteValue);
                                break;
                        }
                        crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    }
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    Mostrar_Reporte(Nombre_Reporte_Generar, ".pdf");
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
                Direccion_Guardar_Disco.DiskFileName = @Server.MapPath("../../Reporte/" + Nombre_Reporte_Generar);
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
            Pagina = Pagina + Nombre_Reporte_Generar;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Cajas_Generales",
                "window.open('" + Pagina + "', 'Cajas','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    #endregion

}