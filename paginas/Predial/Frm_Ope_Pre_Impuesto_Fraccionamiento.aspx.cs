using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Impuestos_Fraccionamientos.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Catalogo_Fraccionamientos.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Catalogo_Multas_Fraccionamientos.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Operacion_Predial_Convenios_Impuestos_Traslado_Dominio.Negocio;
using System.Data.OracleClient;

public partial class paginas_Predial_Frm_Ope_Pre_Impuesto_Fraccionamiento : System.Web.UI.Page
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
        string Ventana_Modal;
        if (!IsPostBack)
        {
            Session["Activa"] = true;//Variable para mantener la session activa.
            Hdf_Estatus.Value = "POR PAGAR";
            Configuracion_Formulario(true);
            Cargar_Grid_Impuestos_Fraccionamientos(0, Hdf_Estatus.Value);
            
            Session.Remove("ESTATUS_CUENTAS");
            Session.Remove("TIPO_CONTRIBUYENTE");
            Session["ESTATUS_CUENTAS"] = "IN ('PENDIENTE','ACTIVA','VIGENTE','BLOQUEADA','SUSPENDIDA')";
            //Scrip para mostrar Ventana Modal de las cuentas
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal);
            //Scrip para mostrar Ventana Modal de los costos
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Fraccionamientos/Frm_Menu_Pre_Fraccionamientos.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Mostrar_Busqueda_Avanzada_Costos.Attributes.Add("onclick", Ventana_Modal);
            //Scrip para mostrar Ventana Modal de las multas
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Fraccionamientos/Frm_Menu_Pre_Multas.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Multas.Attributes.Add("onclick", Ventana_Modal);
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

    #region Propiedades
    public String No_Impuesto_Fraccionamiento
    {
        get
        {
            return Hdf_No_Impuesto_Fraccionamiento.Value;
        }
    }

    public String Cuenta_Predial_ID
    {
        get
        {
            return Hdf_Cuenta_Predial_ID.Value;
        }
    }

    public String Cuenta_Predial
    {
        get
        {
            return Txt_Cuenta_Predial.Text;
        }
    }

    private enum Orden_Datos
    {
        Ascendente,
        Descendente
    }
    #endregion

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
        //Btn_Nuevo.Visible = true;
        //Btn_Nuevo.AlternateText = "Nuevo";
        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        //Btn_Modificar.Visible = true;
        //Btn_Modificar.AlternateText = "Modificar";
        //Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Grid_Impuestos_Fraccionamientos.Enabled = Estatus;
        Grid_Detalle_Impuesto_Fraccionamiento.Enabled = !Estatus;
        Grid_Detalle_Impuesto_Fraccionamiento.SelectedIndex = (-1);
        if (Btn_Modificar.AlternateText == "Actualizar")
        {
            Cmb_Estatus.Enabled = true;
        }
        else
        {
            Cmb_Estatus.Enabled = false;
        }
        Txt_Cuenta_Predial.Enabled = false;
        Txt_Superficie_Predio.Enabled = false;
        Txt_Superficie_Fraccionar.Enabled = !Estatus;
        Txt_Tipo_Predio.Enabled = false;
        Txt_Descripcion_Costo_M2.Enabled = false;
        Txt_Costo_M2.Enabled = false;
        Txt_Importe.Enabled = false;
        Txt_Total.Enabled = false;
        Txt_Calle.Enabled = false;
        Txt_Colonia.Enabled = false;
        Txt_No_Exterior.Enabled = false;
        Txt_No_Interior.Enabled = false;
        Txt_Propietario.Enabled = false;
        Txt_Fecha_Oficio.Enabled = !Estatus;
        Btn_Fecha_Oficio.Enabled = !Estatus;
        Txt_Fecha_Vencimiento.Enabled = false;
        Txt_Recargos.Enabled = false;
        Txt_Observaciones.Enabled = !Estatus;
        Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = !Estatus;
        Btn_Detalles_Cuenta_Predial.Enabled = !Estatus;
        Btn_Mostrar_Busqueda_Avanzada_Costos.Enabled = !Estatus;
        Btn_Agregar_Impuesto.Enabled = !Estatus;
        Txt_Total_Impuestos_Grid.Enabled = false;

        Txt_Superficie_Fraccionar.Style["text-align"] = "right";
        Txt_Costo_M2.Style["text-align"] = "right";
        Txt_Importe.Style["text-align"] = "right";
        Txt_Recargos.Style["text-align"] = "right";
        Txt_Total.Style["text-align"] = "right";
        Txt_Total_Impuestos_Grid.Style["text-align"] = "right";

        Panel_Datos.Visible = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Catálogo
    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    ///PARAMETROS           :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        Hdf_Cuenta_Predial_ID.Value = "";
        Hdf_Multa.Value = "";
        Hdf_No_Impuesto_Fraccionamiento.Value = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Cuenta_Predial.Text = "";
        Txt_Superficie_Predio.Text = "";
        Txt_Superficie_Fraccionar.Text = "";
        Txt_Tipo_Predio.Text = "";
        Txt_Descripcion_Costo_M2.Text = "";
        Txt_Costo_M2.Text = "";
        Txt_Importe.Text = "";
        Txt_Calle.Text = "";
        Txt_Colonia.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Propietario.Text = "";
        Txt_Total.Text = "";
        Dtp_Fecha_Oficio.SelectedDate = null;
        Txt_Fecha_Oficio.Text = "";
        Dtp_Fecha_Vencimiento.SelectedDate = null;
        Txt_Fecha_Vencimiento.Text = "";
        Txt_Recargos.Text = "";
        Txt_Observaciones.Text = "";
        Grid_Detalle_Impuesto_Fraccionamiento.DataSource = null;
        Grid_Detalle_Impuesto_Fraccionamiento.DataBind();
        Txt_Total_Impuestos_Grid.Text = "";
        Txt_Multa.Text = "";
        Hdf_Fecha_Ya_Asignada.Value = "";

        Session["Cuenta_Predial"] = null;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Impuestos_Fraccionamientos
    ///DESCRIPCIÓN          : Llena la tabla de Impuestos de Fraccionamientos con los registros encontrados.
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Impuestos_Fraccionamientos(Int32 Pagina, String Por_Estatus)
    {
        try
        {
            Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuestos_Fraccionamiento = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
            Impuestos_Fraccionamiento.P_Campos_Dinamicos = "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + ") AS Cuenta_Predial, ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Oficio + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus;
            Impuestos_Fraccionamiento.P_Filtros_Dinamicos = "";
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos = "(";
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " = '" + Txt_Busqueda.Text.Trim() + "'  OR ";
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos += "" + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Txt_Busqueda.Text.Trim().ToUpper() + "')";
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos += ")";
            }
            if (Impuestos_Fraccionamiento.P_Filtros_Dinamicos != "")
            {
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos += " AND ";
            }
            if (Impuestos_Fraccionamiento.P_Filtros_Dinamicos != null && Impuestos_Fraccionamiento.P_Filtros_Dinamicos != "")
            {
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos += "(" + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR' OR " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'CANCELADA' OR " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'SIN PAGAR' OR " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'IMPRESA' OR ";
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'PAGADO') ";
            }
            else
            {
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos = "(" + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = '" + Por_Estatus + "' OR " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'CANCELADA' OR " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'SIN PAGAR' OR " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'IMPRESA' OR ";
                Impuestos_Fraccionamiento.P_Filtros_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'PAGADO') ";
            }
            Impuestos_Fraccionamiento.P_Ordenar_Dinamico = Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " DESC";
            DataTable Tabla = Impuestos_Fraccionamiento.Consultar_Impuestos_Fraccionamiento();
            if (Tabla != null)
            {
                Grid_Impuestos_Fraccionamientos.Columns[7].Visible = true;
                Grid_Impuestos_Fraccionamientos.DataSource = Tabla;
                Grid_Impuestos_Fraccionamientos.PageIndex = Pagina;
                Grid_Impuestos_Fraccionamientos.DataBind();
                Grid_Impuestos_Fraccionamientos.Columns[7].Visible = false;
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
    ///NOMBRE DE LA FUNCIÓN : Llenar_Tabla_Impuestos_Fraccionamiento
    ///DESCRIPCIÓN          : Llena la tabla de Impuestos de Fraccionamientos con una consulta que puede o no tener Filtros.
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Detalles_Impuestos_Fraccionamiento(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuestos_Fraccionamiento = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
            Impuestos_Fraccionamiento.P_Campos_Dinamicos = "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + ") AS Cuenta_Predial, ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + ") AS Superficie_Construida, ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + ") AS Superficie_Total, ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Tipos_Predio.Campo_Descripcion + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " WHERE " + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + "." + Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + ")) AS Tipo_Predio, ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Oficio + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + ", ";
            Impuestos_Fraccionamiento.P_Campos_Dinamicos += Ope_Pre_Impuestos_Fraccionamientos.Campo_Observaciones;
            //Impuestos_Fraccionamiento.P_Filtros_Dinamicos = Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " LIKE '%" + Txt_Busqueda.Text.Trim() + "%'";
            Impuestos_Fraccionamiento.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Fraccionamiento.Value;// Convert.ToInt32(Txt_Busqueda.Text.Trim()).ToString("0000000000");
            //Impuestos_Fraccionamiento.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            DataTable Tabla = Impuestos_Fraccionamiento.Consultar_Impuestos_Fraccionamiento();
            if (Tabla != null)
            {
                Hdf_No_Impuesto_Fraccionamiento.Value = Tabla.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento].ToString();
                Hdf_Cuenta_Predial_ID.Value = Tabla.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID].ToString();
                Txt_Cuenta_Predial.Text = Tabla.Rows[0]["Cuenta_Predial"].ToString();
                if (Tabla.Rows[0]["Superficie_Construida"].ToString() != "")
                {
                    Txt_Superficie_Predio.Text = Tabla.Rows[0]["Superficie_Construida"].ToString();
                }
                else
                {
                    Txt_Superficie_Predio.Text = "0.00";
                }
                Txt_Tipo_Predio.Text = Tabla.Rows[0]["Tipo_Predio"].ToString();
                Cmb_Estatus.SelectedValue = Tabla.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus].ToString();
                //Txt_Fecha_Vencimiento.Text = Tabla.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento].ToString();
                Dtp_Fecha_Vencimiento.SelectedDate = Convert.ToDateTime(Tabla.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento].ToString());
                if (Tabla.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Oficio].ToString().Length != 0)
                {
                    Dtp_Fecha_Oficio.SelectedDate = Convert.ToDateTime(Tabla.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Oficio].ToString());
                }
                else
                {
                    Txt_Fecha_Oficio.Text = "----------";
                }
                DataTable Dt_Ayudante;
                Txt_Observaciones.Text = Tabla.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Observaciones].ToString();
                Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = true;
                Dt_Ayudante = Impuestos_Fraccionamiento.P_Dt_Detalles_Impuestos_Fraccionamiento;
                if (!(Dt_Ayudante.Rows.Count == 0))
                {
                    foreach (DataRow Renglon_Actual in Dt_Ayudante.Rows)
                    {
                        if (Renglon_Actual["MULTAS"].ToString() == "")
                        {
                            Renglon_Actual["MULTAS"] = "0.00";
                        }
                    }
                }
                Grid_Detalle_Impuesto_Fraccionamiento.DataSource = Dt_Ayudante;
                Grid_Detalle_Impuesto_Fraccionamiento.PageIndex = Pagina;
                Grid_Detalle_Impuesto_Fraccionamiento.DataBind();
                Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = false;

                //Se calcula el Total del Grid
                Calcular_Total_Impuestos();
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
    ///NOMBRE DE LA FUNCIÓN : Calcular_Total_Impuestos
    ///DESCRIPCIÓN          : Suma los Totales del grid
    ///PARAMETROS:
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Double Calcular_Total_Impuestos()
    {
        //Se calcula el Total del Grid
        Double Sum_Total = 0;
        foreach (GridViewRow Row in Grid_Detalle_Impuesto_Fraccionamiento.Rows)
        {
            if (Row.Cells[6].Text != "" && Row.Cells[7].Text != "&nbsp;")
            {
                Sum_Total += Convert.ToDouble(Row.Cells[7].Text.Replace("$", ""));
            }
        }
        Txt_Total_Impuestos_Grid.Text = Sum_Total.ToString("0.00");
        return Sum_Total;
    }

    #region Validaciones

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
        if (Txt_Fecha_Vencimiento.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccione una Fecha de Vencimiento.";
            Validacion = false;
        }
        //if (Txt_Fecha_Vencimiento.Text.Trim() != "")
        //{
        //    if (!(Convert.ToDateTime(Txt_Fecha_Vencimiento.Text) > Convert.ToDateTime(DateTime.Now.ToShortDateString())))
        //    {
        //        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('La Fecha de Vencimiento debe ser menor a la Fecha Actual');", true);
        //        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //        Mensaje_Error = Mensaje_Error + "+ La Fecha de Vencimiento debe ser Mayor o Igual a la Fecha Actual.";
        //        Validacion = false;
        //    }
        //}
        if (Cmb_Estatus.SelectedIndex <= 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Indique un Estatus.";
            Validacion = false;
        }
        if (Grid_Detalle_Impuesto_Fraccionamiento.Rows.Count == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introduzca por lo menos un Impuesto.";
            Validacion = false;
        }
        if (Btn_Nuevo.AlternateText != "Dar de Alta" && Btn_Modificar.AlternateText == "Actualizar")
        {
            if (Txt_Observaciones.Text.Equals(""))
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introduzca las Observaciones.";
                Lbl_Mensaje_Error.Text = "";
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Total_Superficie_Fraccionar
    ///DESCRIPCIÓN          : Devuelve True si el total de la Superficie a Fraccionar no excedió el total de la superficie del Predio, en caso contrario devolverá False.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Total_Superficie_Fraccionar()
    {
        //Se calcula el Total de la Superficie a Fraccionar del Grid
        Double Sum_Superficie_Fraccionar_Total = 0;
        Double Superficie_Predio = 0;
        Double Superficie_Fraccionar = 0;
        foreach (GridViewRow Row in Grid_Detalle_Impuesto_Fraccionamiento.Rows)
        {
            if (Row.Cells[0].Text != "" && Row.Cells[0].Text != "&nbsp;")
            {
                Sum_Superficie_Fraccionar_Total += Convert.ToDouble(Row.Cells[0].Text);
            }
        }
        if (Txt_Superficie_Predio.Text.Trim() != "")
        {
            Superficie_Predio = Convert.ToDouble(Txt_Superficie_Predio.Text);
        }
        if (Txt_Superficie_Fraccionar.Text.Trim() != "")
        {
            Superficie_Fraccionar = Convert.ToDouble(Txt_Superficie_Fraccionar.Text);
        }
        Sum_Superficie_Fraccionar_Total += Superficie_Fraccionar;
        return Sum_Superficie_Fraccionar_Total <= Superficie_Predio;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Adeudo_Impuesto_Predial
    ///DESCRIPCIÓN          : Buscará en en Adeudo Predial si no hay Recargos.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 13/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Adeudo_Impuesto_Predial()
    {
        Boolean Presenta_Adeudo;
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Adeudo_Predial = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();

        Adeudo_Predial.Calcular_Recargos_Predial(Hdf_Cuenta_Predial_ID.Value);
        if (Adeudo_Predial.p_Total_Adeudos_Generados != 0)
        {
            Presenta_Adeudo = true;
        }
        else
        {
            Presenta_Adeudo = false;
        }
        return Presenta_Adeudo;
    }
    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Impuestos_Fraccionamientos_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Impuestos_Fraccionamientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            String Estatus = Hdf_Estatus.Value;
            Grid_Impuestos_Fraccionamientos.SelectedIndex = (-1);
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Estatus = "";
            }
            Cargar_Grid_Impuestos_Fraccionamientos(e.NewPageIndex, Estatus);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Impuestos_Fraccionamientos_SelectedIndexChanged
    ///DESCRIPCIÓN          : Maneja la selección de las filas del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Impuestos_Fraccionamientos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Impuestos_Fraccionamientos.Rows.Count > 0)
        {
            Hdf_No_Impuesto_Fraccionamiento.Value = Grid_Impuestos_Fraccionamientos.SelectedRow.Cells[1].Text;
            Hdf_Cuenta_Predial_ID.Value = Grid_Impuestos_Fraccionamientos.DataKeys[Grid_Impuestos_Fraccionamientos.SelectedIndex].Value.ToString();
            Llenar_Tabla_Detalles_Impuestos_Fraccionamiento(0);
            Txt_Cuenta_Predial_TextChanged();
            Grid_Impuestos_Fraccionamientos.Visible = false;
            Btn_Salir.AlternateText = "Atrás";
            Panel_Datos.Visible = true;
            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Cuenta_Pendiente = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
            Cuenta_Pendiente.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            if (!Cuenta_Pendiente.Consultar_Cuenta_Pendiente())
            {
                Cargar_Datos();
            }
        }
        Cargar_Ventana_Emergente_Resumen_Predio();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Detalles_Impuesto_Fraccionamiento_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Detalles_Impuesto_Fraccionamiento_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Detalle_Impuesto_Fraccionamiento.SelectedIndex = (-1);
            Llenar_Tabla_Detalles_Impuestos_Fraccionamiento(e.NewPageIndex);
            Limpiar_Controles();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Detalles_Impuesto_Fraccionamiento_RowCommand
    ///DESCRIPCIÓN          : Evento RowCommand para procesas los diferentes botones de comando en el gridview
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Detalles_Impuesto_Fraccionamiento_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Eliminar":
                int PageIndex = Grid_Detalle_Impuesto_Fraccionamiento.PageIndex;

                //Imprimir_Reporte(Crear_Ds_Constancias(Convert.ToInt32(e.CommandArgument)), "Rpt_Pre_Constancias.rpt", "Impuestos de Derechos de Supervisión");
                DataTable Dt_Impuestos = new DataTable();
                Dt_Impuestos.Columns.Add(new DataColumn("SUPERFICIE_FRACCIONAR", typeof(Double)));
                Dt_Impuestos.Columns.Add(new DataColumn("IMPUESTO_FRACCIONAMIENTO_ID", typeof(String)));
                Dt_Impuestos.Columns.Add(new DataColumn("DESCRIPCION_MONTO", typeof(String)));
                Dt_Impuestos.Columns.Add(new DataColumn("MONTO_FRACCIONAMIENTO", typeof(Double)));
                Dt_Impuestos.Columns.Add(new DataColumn("MULTAS", typeof(Double)));
                Dt_Impuestos.Columns.Add(new DataColumn("IMPORTE", typeof(Double)));
                Dt_Impuestos.Columns.Add(new DataColumn("RECARGOS", typeof(Double)));
                Dt_Impuestos.Columns.Add(new DataColumn("TOTAL", typeof(Double)));
                Dt_Impuestos.Columns.Add(new DataColumn("FRA_MULTA_CUOTA_ID", typeof(String)));

                DataRow Dr_Impuestos;
                //Se barre el Grid para cargar el DataTable con los valores del grid
                Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = true;
                foreach (GridViewRow Row in Grid_Detalle_Impuesto_Fraccionamiento.Rows)
                {
                    if (Convert.ToInt32(e.CommandArgument) != Row.DataItemIndex)
                    {
                        Dr_Impuestos = Dt_Impuestos.NewRow();
                        Dr_Impuestos["SUPERFICIE_FRACCIONAR"] = Convert.ToDouble(Row.Cells[0].Text);
                        Dr_Impuestos["IMPUESTO_FRACCIONAMIENTO_ID"] = Grid_Detalle_Impuesto_Fraccionamiento.DataKeys[Row.RowIndex].Value.ToString();// Row.Cells[1].Text;
                        Dr_Impuestos["DESCRIPCION_MONTO"] = HttpUtility.HtmlDecode(Row.Cells[2].Text).Trim();
                        Dr_Impuestos["MONTO_FRACCIONAMIENTO"] = Convert.ToDouble(Row.Cells[3].Text.Replace("$", ""));
                        Dr_Impuestos["MULTAS"] = Convert.ToDouble(Row.Cells[4].Text.Replace("$", ""));
                        Dr_Impuestos["IMPORTE"] = Convert.ToDouble(Row.Cells[5].Text.Replace("$", ""));
                        Dr_Impuestos["RECARGOS"] = Convert.ToDouble(Row.Cells[6].Text.Replace("$", ""));
                        Dr_Impuestos["TOTAL"] = Convert.ToDouble(Row.Cells[7].Text.Replace("$", ""));
                        Dr_Impuestos["FRA_MULTA_CUOTA_ID"] = HttpUtility.HtmlDecode(Row.Cells[8].Text);
                        Dt_Impuestos.Rows.Add(Dr_Impuestos);
                    }
                }

                Grid_Detalle_Impuesto_Fraccionamiento.DataSource = Dt_Impuestos;
                if (PageIndex >= Grid_Detalle_Impuesto_Fraccionamiento.PageCount)
                {
                    Grid_Detalle_Impuesto_Fraccionamiento.PageIndex = PageIndex - 1;
                }
                else
                {
                    Grid_Detalle_Impuesto_Fraccionamiento.PageIndex = PageIndex;
                }
                Grid_Detalle_Impuesto_Fraccionamiento.DataBind();
                Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = false;

                Calcular_Total_Impuestos();
                break;
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Impuestos_Fraccionamiento
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Limpiar_Controles();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Btn_Imprimir.Visible = false;
                Btn_Convenio.Visible = false;
                Grid_Impuestos_Fraccionamientos.Visible = false;
                Cmb_Estatus.SelectedIndex = 1;
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuestos_Fraccionamiento = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
                    Impuestos_Fraccionamiento.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Impuestos_Fraccionamiento.P_Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text.Trim());
                    Impuestos_Fraccionamiento.P_Fecha_Oficio = Convert.ToDateTime(Txt_Fecha_Oficio.Text.Trim());
                    Cmb_Estatus.SelectedIndex = 2;
                    Impuestos_Fraccionamiento.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    Impuestos_Fraccionamiento.P_Observaciones = Txt_Observaciones.Text.Trim().ToUpper();
                    Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = true;
                    Impuestos_Fraccionamiento.P_Dt_Detalles_Impuestos_Fraccionamiento = Crear_Tabla_Detalles_Impuestos_Fraccionamientos();
                    Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = false;
                    Impuestos_Fraccionamiento.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    if (Impuestos_Fraccionamiento.Alta_Impuesto_Fraccionamiento())
                    {
                        Insertar_Pasivo("IMP" + String.Format("{0:yy}", DateTime.Now) + (Convert.ToInt32(Impuestos_Fraccionamiento.P_No_Impuesto_Fraccionamiento)));
                        Grid_Impuestos_Fraccionamientos.Visible = true;
                        Cargar_Grid_Impuestos_Fraccionamientos(0, Hdf_Estatus.Value);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('Alta de Impuestos de Fraccionamiento Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.Visible = true;
                        Btn_Imprimir.Visible = true;
                        Btn_Convenio.Visible = true;
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Hdf_No_Impuesto_Fraccionamiento.Value = Impuestos_Fraccionamiento.P_No_Impuesto_Fraccionamiento;
                        Hdf_Cuenta_Predial_ID.Value = Impuestos_Fraccionamiento.P_Cuenta_Predial_ID;
                        if (Hdf_No_Impuesto_Fraccionamiento.Value != "")
                        {
                            Imprimir_Reporte(Crear_Ds_Impuestos_Fraccionamientos(), "Rpt_Pre_Impuestos_Fraccionamientos.rpt", "Impuestos Fraccionamientos");
                        }
                        Configuracion_Formulario(true);
                        Limpiar_Controles();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('Alta de Impuestos de Fraccionamiento No fue Exitosa');", true);
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Insertar_Pasivo
    ///DESCRIPCIÓN          : Consulta el Costo del Documento y lo Inserta en Pasivo
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Insertar_Pasivo(String Referencia)
    {
        try
        {
            //OracleConnection Cn = new OracleConnection();
            //OracleCommand Cmd = new OracleCommand();
            //OracleTransaction Trans = null;
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
            DataTable Dt_Clave;
            Double Recargos_Ord = 0;
            Double Multas = 0;
            Double Impuestos = 0;

            ////// crear transaccion para modificar tabla de calculos y de adeudos folio
            ////Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            ////Cn.Open();
            ////Trans = Cn.BeginTransaction();
            ////Cmd.Connection = Cn;
            ////Cmd.Transaction = Trans;
            ////Calculo_Impuesto_Traslado.P_Cmd_Calculo = Cmd;


            foreach (GridViewRow Dt_Detalles in Grid_Detalle_Impuesto_Fraccionamiento.Rows)
            {
                Multas += Convert.ToDouble(Dt_Detalles.Cells[4].Text.Replace("$", ""));//Multas
                Recargos_Ord += Convert.ToDouble(Dt_Detalles.Cells[6].Text.Replace("$", ""));//recargos
                Impuestos += Convert.ToDouble(Dt_Detalles.Cells[5].Text.Replace("$", ""));//Impuestos
            }

            Calculo_Impuesto_Traslado.P_Referencia = Referencia + "' AND " + Ope_Ing_Pasivo.Campo_Descripcion + " NOT LIKE '%DESCUENTO%";
            Calculo_Impuesto_Traslado.Eliminar_Referencias_Pasivo();

            if (Impuestos > 0)
            {
                Claves_Ingreso.P_Tipo = "FRACCIONAMIENTOS";
                Claves_Ingreso.P_Tipo_Predial_Traslado = "IMPUESTO";
                Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                if (Dt_Clave.Rows.Count > 0)
                {
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "IMPUESTO FRACCIONAMIENTOS";
                    if (Cmb_Estatus.SelectedValue != "CANCELADA")
                    {
                        Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                    }
                    else
                    {
                        Calculo_Impuesto_Traslado.P_Estatus = "CANCELADO";
                    }
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = Impuestos.ToString("0.00");
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                }
            }
            if (Multas > 0)
            {
                Claves_Ingreso.P_Tipo = "FRACCIONAMIENTOS";
                Claves_Ingreso.P_Tipo_Predial_Traslado = "MULTAS";
                Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                if (Dt_Clave.Rows.Count > 0)
                {
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "MULTAS";
                    if (Cmb_Estatus.SelectedValue != "CANCELADA")
                    {
                        Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                    }
                    else
                    {
                        Calculo_Impuesto_Traslado.P_Estatus = "CANCELADO";
                    }
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = Multas.ToString("0.00");
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                }
            }
            if (Recargos_Ord > 0)
            {
                Claves_Ingreso.P_Tipo = "FRACCIONAMIENTOS";
                Claves_Ingreso.P_Tipo_Predial_Traslado = "RECARGOS ORDINARIOS";
                Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                if (Dt_Clave.Rows.Count > 0)
                {
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "RECARGOS ORDINARIOS";
                    if (Cmb_Estatus.SelectedValue != "CANCELADA")
                    {
                        Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                    }
                    else
                    {
                        Calculo_Impuesto_Traslado.P_Estatus = "CANCELADO";
                    }
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = Recargos_Ord.ToString("0.00");
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "El Pasivo no pudo ser insertado: " + Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Impuestos_Fraccionamiento.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Impuestos_Fraccionamientos.SelectedIndex > -1)
        {
            Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuesto_En_Convenio = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
            Impuesto_En_Convenio.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Fraccionamiento.Value;
            DataTable Tabla;
            Tabla = Impuesto_En_Convenio.Consultar_Impuestos_Con_Convenio();
            if (Tabla == null || Tabla.Rows.Count == 0)
            {
                try
                {
                    if (!Grid_Impuestos_Fraccionamientos.SelectedRow.Cells[6].Text.Equals("PAGADO"))
                    {
                        if (Btn_Modificar.AlternateText.Equals("Modificar"))
                        {
                            if (Hdf_No_Impuesto_Fraccionamiento.Value != "")
                            {
                                Btn_Modificar.AlternateText = "Actualizar";
                                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                                Btn_Salir.AlternateText = "Cancelar";
                                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                                Btn_Nuevo.Visible = false;
                                Btn_Imprimir.Visible = false;
                                Btn_Convenio.Visible = false;
                                Configuracion_Formulario(false);
                                Grid_Impuestos_Fraccionamientos.Visible = false;
                                Cmb_Estatus.Items.RemoveAt(1);
                                Cmb_Estatus.Items.RemoveAt(2);
                            }
                        }
                        else
                        {
                            if (Validar_Componentes())
                            {
                                Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuestos_Fraccionamiento = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
                                Impuestos_Fraccionamiento.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Fraccionamiento.Value;
                                Impuestos_Fraccionamiento.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                                //Impuestos_Fraccionamiento.P_Fecha = Convert.ToDateTime(Txt_Fecha.Text.Trim());
                                Impuestos_Fraccionamiento.P_Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text.Trim());
                                Impuestos_Fraccionamiento.P_Fecha_Oficio = Convert.ToDateTime(Txt_Fecha_Oficio.Text.Trim());
                                Impuestos_Fraccionamiento.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                                Impuestos_Fraccionamiento.P_Observaciones = Txt_Observaciones.Text.Trim().ToUpper();
                                Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = true;
                                Impuestos_Fraccionamiento.P_Dt_Detalles_Impuestos_Fraccionamiento = Crear_Tabla_Detalles_Impuestos_Fraccionamientos();
                                Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = false;
                                Impuestos_Fraccionamiento.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                                if (Impuestos_Fraccionamiento.Modificar_Impuesto_Fraccionamiento())
                                {
                                    if (Cmb_Estatus.SelectedValue != "IMPRESA")
                                    {
                                        Insertar_Pasivo("IMP" + Grid_Impuestos_Fraccionamientos.SelectedRow.Cells[7].Text.Substring(9) + Convert.ToInt32(Impuestos_Fraccionamiento.P_No_Impuesto_Fraccionamiento));
                                    }

                                    if (Hdf_No_Impuesto_Fraccionamiento.Value != "")
                                    {
                                        Imprimir_Reporte(Crear_Ds_Impuestos_Fraccionamientos(), "Rpt_Pre_Impuestos_Fraccionamientos.rpt", "Impuestos Fraccionamientos");
                                    }

                                    Limpiar_Controles();
                                    Cargar_Grid_Impuestos_Fraccionamientos(0, Hdf_Estatus.Value);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('Actualización de Impuestos de Fraccionamiento Exitosa');", true);
                                    Btn_Modificar.AlternateText = "Modificar";
                                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                                    Btn_Nuevo.Visible = true;
                                    Btn_Imprimir.Visible = true;
                                    Btn_Convenio.Visible = true;
                                    Btn_Salir.AlternateText = "Salir";
                                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                                    Configuracion_Formulario(true);
                                    Grid_Impuestos_Fraccionamientos.Visible = true;
                                    Cmb_Estatus.Items.Insert(1, new ListItem("SIN PAGAR", "SIN PAGAR"));
                                    Cmb_Estatus.Items.Insert(3, new ListItem("PAGADO", "PAGADO"));
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('Actualización de Impuestos de Fraccionamiento No fue Exitosa');", true);
                                }
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('El impuesto se encuentra pagado.');", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('El impuesto se encuentra convenido.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('Seleccione un impuesto por favor.');", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Constancia_Propiedad_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Controles();
            Grid_Impuestos_Fraccionamientos.SelectedIndex = (-1);
            Grid_Detalle_Impuesto_Fraccionamiento.SelectedIndex = (-1);
            Cargar_Grid_Impuestos_Fraccionamientos(0, Hdf_Estatus.Value);
            //Llenar_Tabla_Detalles_Impuestos_Fraccionamiento(0);
            if (Grid_Impuestos_Fraccionamientos.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda de \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "";
                //Lbl_Mensaje_Error.Text = "(Se cargarón todos los Impuestos de Fraccionamientos encontrados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                //Llenar_Tabla_Impuestos_Fraccionamiento(0);
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
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
        else if (Btn_Salir.AlternateText == "Atrás")
        {
            Panel_Datos.Visible = false;
            Grid_Impuestos_Fraccionamientos.Visible = true;
            Grid_Impuestos_Fraccionamientos.SelectedIndex = -1;
            Btn_Salir.AlternateText = "Salir";
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
            Btn_Convenio.Visible = true;
            Configuracion_Formulario(true);
            Limpiar_Controles();
            Cargar_Grid_Impuestos_Fraccionamientos(0, Hdf_Estatus.Value);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Panel_Datos.Visible = false;
            Grid_Impuestos_Fraccionamientos.Visible = true;
            Grid_Impuestos_Fraccionamientos.SelectedIndex = -1;
            Btn_Salir.AlternateText = "Salir";
            if (Btn_Modificar.AlternateText == "Modificar")
            {
                Cmb_Estatus.Items.Insert(1, new ListItem("SIN PAGAR", "SIN PAGAR"));
                Cmb_Estatus.Items.Insert(3, new ListItem("PAGADO", "PAGADO"));
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click
    ///DESCRIPCIÓN          : Muestra los datos de la busqueda
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
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
                Txt_Cuenta_Predial_TextChanged();
                Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Cuenta_Pendiente = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                Cuenta_Pendiente.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                if (!Cuenta_Pendiente.Consultar_Cuenta_Pendiente())
                {
                    Cargar_Datos();
                }
            }
            Consultar_Datos_Cuenta_Predial();
            Cargar_Ventana_Emergente_Resumen_Predio();
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        //Session.Remove("CUENTA_PREDIAL");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Constanica
    ///DESCRIPCIÓN          : Realiza la búsqueda de los datos de la cuenta predial introducida
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Consultar_Datos_Cuenta_Predial()
    {
        DataTable Dt_Cuentas_Predial;
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        if (Txt_Cuenta_Predial.Text.Trim() != "")
        {
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            //Consulta la Cuenta Predial
            Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS Tipo_Predio ";
            Cuentas_Predial.P_Join = "LEFT OUTER JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " ";
            Cuentas_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            if (Dt_Cuentas_Predial.Rows.Count > 0)
            {
                Hdf_Cuenta_Predial_ID.Value = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                Txt_Superficie_Predio.Text = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString();
                if (Txt_Superficie_Predio.Text == "")
                {
                    Txt_Superficie_Predio.Text = "0.00";
                }
                Txt_Tipo_Predio.Text = Dt_Cuentas_Predial.Rows[0]["Tipo_Predio"].ToString();
            }
            if (Validar_Adeudo_Impuesto_Predial())
            {
                Lbl_Mensaje_Adeudo_Impuesto_Predial.Visible = true;
                Lbl_Mensaje_Adeudo_Impuesto_Predial.Text = "La Cuenta presenta Adeudo de Impuesto Predial";
            }
            else
            {
                Lbl_Mensaje_Adeudo_Impuesto_Predial.Visible = false;
                Lbl_Mensaje_Adeudo_Impuesto_Predial.Text = "";
            }
        }
    }

    #endregion

    #region Cálculos

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Calcular_Importe
    ///DESCRIPCIÓN          : Realiza el cálculo del Importe
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private double Calcular_Importe()
    {
        Double Superficie_Fraccionar = 0;
        Double Costo_M2 = 0;
        Double Importe = 0;

        if (Txt_Superficie_Fraccionar.Text.Trim() != "")
        {
            Superficie_Fraccionar = Convert.ToDouble(Txt_Superficie_Fraccionar.Text);
        }
        else
        {
            Txt_Superficie_Fraccionar.Text = "0.00";
        }
        if (Txt_Costo_M2.Text.Trim() != "")
        {
            Costo_M2 = Convert.ToDouble(Txt_Costo_M2.Text);
        }
        else
        {
            Txt_Costo_M2.Text = "0.00";
        }
        if (Txt_Multa.Text.Trim() == "")
        {
            Txt_Multa.Text = "0.00";
        }

        Importe = Superficie_Fraccionar * Costo_M2;
        Txt_Importe.Text = Importe.ToString("0.00");

        return Importe;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Calcular_Recargos
    ///DESCRIPCIÓN          : Realiza el cálculo de los Recargos
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private double Calcular_Recargos()
    {
        Int32 Dif_Meses = 0;
        Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        DataTable Dt_Parametros;
        Double Importe = 0;
        Double Parametro = 0;
        Double Recargos = 0;
        Dt_Parametros = Parametros.Consultar_Parametros();

        if (Txt_Fecha_Vencimiento.Text.Trim() != "")
        {
            //Dif_Meses = DateTime.Now.Month - Convert.ToDateTime(Txt_Fecha_Vencimiento.Text).Month;
            Dif_Meses = Calcular_Meses_Entre_Fechas(Convert.ToDateTime(Txt_Fecha_Oficio.Text), DateTime.Now);
        }

        if (Txt_Importe.Text.Trim() != "")
        {
            Importe = Convert.ToDouble(Txt_Importe.Text);
        }
        else
        {
            Txt_Importe.Text = "0.00";
        }
        if (Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Recargas_Traslado].ToString() != "")
        {
            Parametro = Convert.ToDouble(Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Recargas_Traslado]);
        }
        Recargos = Importe * ((Dif_Meses * Parametro) / 100);
        Txt_Recargos.Text = Recargos.ToString("0.00");
        return Recargos;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Meses_Entre_Fechas
    /// DESCRIPCIÓN: Regresa un enteron con el numero de meses entre dos fecha
    /// PARÁMETROS:
    /// 		1. Desde_Fecha: Fecha inicial a comparar
    /// 		2. Hasta_Fecha: Fecha final a comparar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 05-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Int32 Calcular_Meses_Entre_Fechas(DateTime Desde_Fecha, DateTime Hasta_Fecha)
    {
        DateTime Fecha_Inicial = Convert.ToDateTime(Desde_Fecha.ToShortDateString());
        DateTime Fecha_Final = Convert.ToDateTime(Hasta_Fecha.ToShortDateString());
        int Meses = 0;

        // aumentar el numero de meses mientras la fecha inicial mas los meses no supere la fecha final
        while (Fecha_Final > Fecha_Inicial.AddMonths(Meses + 1))
        {
            Meses++;
        }

        return Meses;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Calcular_Total
    ///DESCRIPCIÓN          : Realiza el cálculo del Importe
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private double Calcular_Total()
    {
        Double Importe = 0;
        Double Recargos = 0;
        Double Total = 0;
        if (Txt_Importe.Text.Trim() != "")
        {
            Importe = Convert.ToDouble(Txt_Importe.Text);
        }
        else
        {
            Txt_Importe.Text = "0.00";
        }
        if (Txt_Recargos.Text.Trim() != "")
        {
            Recargos = Convert.ToDouble(Txt_Recargos.Text);
        }
        else
        {
            Txt_Recargos.Text = "0.00";
        }
        Total = Importe + Recargos;
        Txt_Total.Text = Total.ToString("0.00");

        return Total;

    }

    protected void Txt_Superficie_Fraccionar_TextChanged(object sender, EventArgs e)
    {
        if (Txt_Fecha_Oficio.Text != "")
        {
            Double Superficie_Predio = 0;
            Double Superficie_Fraccinar = 0;

            if (Txt_Superficie_Predio.Text.Trim() != "")
            {
                Superficie_Predio = Convert.ToDouble(Txt_Superficie_Predio.Text);
            }
            else
            {
                Txt_Superficie_Predio.Text = "0.0";
            }
            if (Txt_Superficie_Fraccionar.Text.Trim() != "")
            {
                Superficie_Fraccinar = Convert.ToDouble(Txt_Superficie_Fraccionar.Text);
                Txt_Superficie_Fraccionar.Text = Superficie_Fraccinar.ToString("###,###,###,##0.00");
            }
            else
            {
                Txt_Superficie_Fraccionar.Text = "0.0";
            }

            //if (Superficie_Fraccinar <= Superficie_Predio)
            //{
            Calcular_Importe();
            Calcular_Recargos();
            Calcular_Total();
            //}
            //else
            //{
            //    Lbl_Ecabezado_Mensaje.Text = HttpUtility.HtmlDecode("+ La Superficie a Fraccionar debe ser Menor o Igual a la Superficie del Predio.");
            //    Lbl_Mensaje_Error.Text = "";
            //    Div_Contenedor_Msj_Error.Visible = true;
            //}
        }
        else
        {
            Txt_Superficie_Fraccionar.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamientos", "alert('Seleccione una fecha de oficio por favor');", true);
        }
    }

    protected void Txt_Costo_M2_TextChanged(object sender, EventArgs e)
    {
        Calcular_Importe();
        Calcular_Recargos();
        Calcular_Total();
    }

    //protected void Txt_Recargos_TextChanged(object sender, EventArgs e)
    //{
    //    Calcular_Total();
    //}

    #endregion

    #region Impresion Folios

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataTable Dt_Impuestos_Fraccionamientos, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Reporte.SetDataSource(Dt_Impuestos_Fraccionamientos);

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar        
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;

        Reporte.Export(Export_Options);
        Mostrar_Reporte(Archivo_PDF, "PDF");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Impuestos_Fraccionamientos, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);

        //Grid_Dt_Impuestos_Fraccionamientos.DataSource = Ds_Impuestos_Fraccionamientos.Tables["Dt_Impuestos_Fraccionamientos"];
        //Grid_Dt_Impuestos_Fraccionamientos.DataBind();
        //Grid_Dt_Detalles_Impuestos_Fraccionamientos.DataSource = Ds_Impuestos_Fraccionamientos.Tables["Dt_Detalles_Impuestos_Fraccionamientos"];
        //Grid_Dt_Detalles_Impuestos_Fraccionamientos.DataBind();
        //Grid_Dt_Cuentas_Predial.DataSource = Ds_Impuestos_Fraccionamientos.Tables["Dt_Cuentas_Predial"];
        //Grid_Dt_Cuentas_Predial.DataBind();
        //Grid_Dt_Tipos_Predio.DataSource = Ds_Impuestos_Fraccionamientos.Tables["Dt_Tipos_Predio"];
        //Grid_Dt_Tipos_Predio.DataBind();
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Impuestos_Fraccionamientos);
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
            Mostrar_Reporte(Archivo_PDF, "PDF");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Dt_Constancias_Propiedad
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos de la Impuestos de Fraccionamiento Seleccionada en el GridView
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Crear_Dt_Constancias_Propiedad(int Indice_Fila)
    {
        Ds_Pre_Constancias Ds_Impuestos_Fraccionamientos = new Ds_Pre_Constancias();
        DataRow Dr_Constancias_Propiedad;

        //Inserta los datos de la Impuestos de Fraccionamiento en la Tabla
        Dr_Constancias_Propiedad = Ds_Impuestos_Fraccionamientos.Tables["Dt_Constancias_Propiedad"].NewRow();
        Dr_Constancias_Propiedad["Cuenta_Predial"] = Grid_Detalle_Impuesto_Fraccionamiento.Rows[Indice_Fila].Cells[1].Text;
        Dr_Constancias_Propiedad["Propietario"] = Grid_Detalle_Impuesto_Fraccionamiento.Rows[Indice_Fila].Cells[2].Text;
        Dr_Constancias_Propiedad["Folio"] = Grid_Detalle_Impuesto_Fraccionamiento.Rows[Indice_Fila].Cells[3].Text;
        Dr_Constancias_Propiedad["Fecha"] = Grid_Detalle_Impuesto_Fraccionamiento.Rows[Indice_Fila].Cells[4].Text;

        Ds_Impuestos_Fraccionamientos.Tables["Dt_Constancias_Propiedad"].Rows.Add(Dr_Constancias_Propiedad);

        return Ds_Impuestos_Fraccionamientos.Tables["Dt_Constancias_Propiedad"];
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Constancias_Propiedad
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos de la Impuestos de Fraccionamiento Seleccionada en el GridView
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Impuestos_Fraccionamientos()
    {
        Ds_Pre_Impuestos_Fraccionamientos Ds_Impuestos_Fraccionamientos = new Ds_Pre_Impuestos_Fraccionamientos();

        Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuestos_Fraccionamientos = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Cat_Pre_Tipos_Predio_Negocio Tipos_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();
        Cls_Cat_Pre_Fraccionamientos_Negocio Fraccionamientos = new Cls_Cat_Pre_Fraccionamientos_Negocio();

        //DataTable Dt_Impuestos_Fraccionamientos;
        DataTable Dt_Cuenta_Predial = null;
        //DataTable Dt_Tipo_Predio;
        DataTable Dt_Temp;
        DataTable Dt_Temp_Detalles = null;
        DataRow Dr_Impuestos_Fraccionameinto;

        String Impuestos_Fraccionamientos_ID = "";

        //Cuentas_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        //Dt_Cuenta_Predial = Cuentas_Predial.Consultar_Cuenta();

        //Tipos_Predio.P_Filtros_Dinamicos = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = '" + Dt_Cuenta_Predial.Rows[0][Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID] + "'";
        //Dt_Tipo_Predio = Tipos_Predio.Consultar_Tipo_Predio();

        foreach (DataTable Dt_Impuestos_Fraccionamientos in Ds_Impuestos_Fraccionamientos.Tables)
        {
            if (Dt_Impuestos_Fraccionamientos.TableName == "Dt_Impuestos_Fraccionamientos")
            {
                Impuestos_Fraccionamientos.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Fraccionamiento.Value;
                Dt_Temp = Impuestos_Fraccionamientos.Consultar_Impuestos_Fraccionamiento();
                Dt_Temp_Detalles = Impuestos_Fraccionamientos.P_Dt_Detalles_Impuestos_Fraccionamiento;

                foreach (DataRow Dr_Temp in Dt_Temp.Rows)
                {
                    int Numero_Impuesto;
                    int.TryParse(Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento].ToString(), out Numero_Impuesto);
                    //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
                    Dr_Impuestos_Fraccionameinto = Dt_Impuestos_Fraccionamientos.NewRow();
                    Dr_Impuestos_Fraccionameinto["NO_IMPUESTO_FRACCIONAMIENTO"] = Numero_Impuesto;
                    Dr_Impuestos_Fraccionameinto["CUENTA_PREDIAL_ID"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Cuenta_Predial_ID];
                    Dr_Impuestos_Fraccionameinto["FECHA_VENCIMIENTO"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento].ToString();
                    Dr_Impuestos_Fraccionameinto["ESTATUS"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus];
                    Dr_Impuestos_Fraccionameinto["OBSERVACIONES"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Observaciones];
                    Dr_Impuestos_Fraccionameinto["FECHA_ELABORACION"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo].ToString();
                    Dr_Impuestos_Fraccionameinto["FECHA_OFICIO"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Oficio].ToString().Substring(0, 10);
                    Dr_Impuestos_Fraccionameinto["ELABORO"] = Dr_Temp[Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Creo];
                    Dr_Impuestos_Fraccionameinto["UBICACION"] = Txt_Colonia.Text + ", " + Txt_Calle.Text + ", NO. EXT " + Txt_No_Exterior.Text + ", NO. INT " + Txt_No_Interior.Text;
                    Dr_Impuestos_Fraccionameinto["PROPIETARIO"] = Txt_Propietario.Text;
                    Dt_Impuestos_Fraccionamientos.Rows.Add(Dr_Impuestos_Fraccionameinto);
                }
            }
            if (Dt_Impuestos_Fraccionamientos.TableName == "Dt_Detalles_Impuestos_Fraccionamientos")
            {
                //Impuestos_Fraccionamientos.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Fraccionamiento.Value;
                //Impuestos_Fraccionamientos.Consultar_Impuestos_Fraccionamiento();
                //Dt_Temp = Impuestos_Fraccionamientos.P_Dt_Detalles_Impuestos_Fraccionamiento;

                foreach (DataRow Dr_Temp in Dt_Temp_Detalles.Rows)
                {
                    int Numero_Impuesto;
                    int.TryParse(Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento].ToString(), out Numero_Impuesto);
                    //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
                    Dr_Impuestos_Fraccionameinto = Dt_Impuestos_Fraccionamientos.NewRow();
                    Dr_Impuestos_Fraccionameinto["NO_IMPUESTO_FRACCIONAMIENTO"] = Numero_Impuesto;
                    Dr_Impuestos_Fraccionameinto["SUPERFICIE_FRACCIONAR"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Superficie_Fraccionar];
                    Dr_Impuestos_Fraccionameinto["DESCRIPCION_MONTO"] = Dr_Temp["DESCRIPCION_MONTO"];
                    if (Dr_Temp["MULTAS"].ToString().Equals(""))
                    {
                        Dr_Impuestos_Fraccionameinto["MULTAS"] = 0.00;
                    }
                    else
                    {
                        Dr_Impuestos_Fraccionameinto["MULTAS"] = Dr_Temp["MULTAS"];
                    }
                    Dr_Impuestos_Fraccionameinto["IMPUESTO_FRACCIONAMIENTO_ID"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID];
                    Dr_Impuestos_Fraccionameinto["IMPORTE"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe];
                    Dr_Impuestos_Fraccionameinto["RECARGOS"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos];
                    Dr_Impuestos_Fraccionameinto["TOTAL"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total];
                    Dt_Impuestos_Fraccionamientos.Rows.Add(Dr_Impuestos_Fraccionameinto);
                    Impuestos_Fraccionamientos_ID += "'" + Dr_Temp[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Impuesto_Fraccionamiento_ID] + "', ";
                }
                if (Impuestos_Fraccionamientos_ID.EndsWith("', "))
                {
                    Impuestos_Fraccionamientos_ID = Impuestos_Fraccionamientos_ID.Substring(0, Impuestos_Fraccionamientos_ID.Length - 2);
                }
            }
            if (Dt_Impuestos_Fraccionamientos.TableName == "Dt_Cuentas_Predial")
            {
                Cuentas_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                Dt_Temp = Cuentas_Predial.Consultar_Cuenta();
                Dt_Cuenta_Predial = Dt_Temp;

                foreach (DataRow Dr_Temp in Dt_Temp.Rows)
                {
                    //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
                    Dr_Impuestos_Fraccionameinto = Dt_Impuestos_Fraccionamientos.NewRow();
                    Dr_Impuestos_Fraccionameinto["CUENTA_PREDIAL_ID"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID];
                    Dr_Impuestos_Fraccionameinto["CUENTA_PREDIAL"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial];
                    Dr_Impuestos_Fraccionameinto["TIPO_PREDIO_ID"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID];
                    Dr_Impuestos_Fraccionameinto["SUPERFICIE_CONSTRUIDA"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida];
                    Dt_Impuestos_Fraccionamientos.Rows.Add(Dr_Impuestos_Fraccionameinto);
                }
            }
            if (Dt_Impuestos_Fraccionamientos.TableName == "Dt_Tipos_Predio")
            {
                Tipos_Predio.P_Filtros_Dinamicos = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = '" + Dt_Cuenta_Predial.Rows[0][Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID] + "'";
                Dt_Temp = Tipos_Predio.Consultar_Tipo_Predio();

                foreach (DataRow Dr_Temp in Dt_Temp.Rows)
                {
                    //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
                    Dr_Impuestos_Fraccionameinto = Dt_Impuestos_Fraccionamientos.NewRow();
                    Dr_Impuestos_Fraccionameinto["TIPO_PREDIO_ID"] = Dr_Temp[Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID];
                    Dr_Impuestos_Fraccionameinto["DESCRIPCION"] = Dr_Temp[Cat_Pre_Tipos_Predio.Campo_Descripcion];
                    Dt_Impuestos_Fraccionamientos.Rows.Add(Dr_Impuestos_Fraccionameinto);
                }
            }
            if (Dt_Impuestos_Fraccionamientos.TableName == "Dt_Fraccionamientos")
            {
                Fraccionamientos.P_Fraccionamiento_ID = "IN (SELECT " + Cat_Pre_Fracc_Impuestos.Campo_Fraccionamiento_ID + " FROM " + Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + " WHERE " + Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID + " IN (" + Impuestos_Fraccionamientos_ID + "))";
                Dt_Temp = Fraccionamientos.Consultar_Fraccionamientos();

                foreach (DataRow Dr_Temp in Dt_Temp.Rows)
                {
                    //Inserta los datos de los Impuestos de Derechos de Supervision en la Tabla
                    Dr_Impuestos_Fraccionameinto = Dt_Impuestos_Fraccionamientos.NewRow();
                    Dr_Impuestos_Fraccionameinto["FRACCIONAMIENTO_ID"] = Dr_Temp[Cat_Pre_Fraccionamientos.Campo_Fraccionamiento_ID];
                    Dr_Impuestos_Fraccionameinto["IDENTIFICADOR"] = Dr_Temp[Cat_Pre_Fraccionamientos.Campo_Identificador];
                    Dr_Impuestos_Fraccionameinto["DESCRIPCION"] = Dr_Temp[Cat_Pre_Fraccionamientos.Campo_Descripcion];
                    Dt_Impuestos_Fraccionamientos.Rows.Add(Dr_Impuestos_Fraccionameinto);
                }
            }
            if (Dt_Impuestos_Fraccionamientos.TableName == "Dt_Fraccionamientos_Impuestos")
            {
                Fraccionamientos.P_Fraccionamiento_Impuesto_ID = "IN (" + Impuestos_Fraccionamientos_ID + ")";
                Fraccionamientos = Fraccionamientos.Consultar_Datos_Fraccionamiento();
                Dt_Temp = Fraccionamientos.P_Fraccionamientos_Impuestos;

                foreach (DataRow Dr_Temp in Dt_Temp.Rows)
                {
                    //Inserta los datos de los Impuestos de Derechos de Supervision en la Tabla
                    Dr_Impuestos_Fraccionameinto = Dt_Impuestos_Fraccionamientos.NewRow();
                    Dr_Impuestos_Fraccionameinto["IMPUESTO_FRACCIONAMIENTO_ID"] = Dr_Temp[Cat_Pre_Fracc_Impuestos.Campo_Impuesto_Fraccionamiento_ID];
                    Dr_Impuestos_Fraccionameinto["FRACCIONAMIENTO_ID"] = Dr_Temp[Cat_Pre_Fracc_Impuestos.Campo_Fraccionamiento_ID];
                    Dr_Impuestos_Fraccionameinto["ANIO"] = Dr_Temp[Cat_Pre_Fracc_Impuestos.Campo_Año];
                    Dr_Impuestos_Fraccionameinto["MONTO"] = Dr_Temp[Cat_Pre_Fracc_Impuestos.Campo_Monto];
                    Dt_Impuestos_Fraccionamientos.Rows.Add(Dr_Impuestos_Fraccionameinto);
                }
            }
        }

        return Ds_Impuestos_Fraccionamientos;
    }

    #endregion

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Agregar_Impuesto_Click
    ///DESCRIPCIÓN          : Agrega las partidas de los Impuestos al Grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Agregar_Impuesto_Click(object sender, ImageClickEventArgs e)
    {
        //Double Superficie_Fraccionar = 0;
        //Double Superficie_Predio = 0;

        //if (Txt_Superficie_Fraccionar.Text.Trim() != "")
        //{
        //    Superficie_Fraccionar = Convert.ToDouble(Txt_Superficie_Fraccionar.Text);
        //}
        //if (Txt_Superficie_Predio.Text.Trim() != "")
        //{
        //    Superficie_Predio = Convert.ToDouble(Txt_Superficie_Predio.Text);
        //}

        if (Txt_Superficie_Fraccionar.Text.Trim() != "")
        {
            if (Convert.ToDouble(Txt_Superficie_Fraccionar.Text) != 0)
            {
                //if (Validar_Total_Superficie_Fraccionar())
                //if (Superficie_Fraccionar <= Superficie_Predio)
                {
                    DataTable Dt_Impuestos = new DataTable();
                    Dt_Impuestos.Columns.Add(new DataColumn("SUPERFICIE_FRACCIONAR", typeof(Double)));
                    Dt_Impuestos.Columns.Add(new DataColumn("IMPUESTO_FRACCIONAMIENTO_ID", typeof(String)));
                    Dt_Impuestos.Columns.Add(new DataColumn("DESCRIPCION_MONTO", typeof(String)));
                    Dt_Impuestos.Columns.Add(new DataColumn("MONTO_FRACCIONAMIENTO", typeof(Double)));
                    Dt_Impuestos.Columns.Add(new DataColumn("MULTAS", typeof(Double)));
                    Dt_Impuestos.Columns.Add(new DataColumn("IMPORTE", typeof(Double)));
                    Dt_Impuestos.Columns.Add(new DataColumn("RECARGOS", typeof(Double)));
                    Dt_Impuestos.Columns.Add(new DataColumn("TOTAL", typeof(Double)));
                    Dt_Impuestos.Columns.Add(new DataColumn("FRA_MULTA_CUOTA_ID", typeof(String)));

                    DataRow Dr_Impuestos;
                    //Se barre el Grid para cargar el DataTable con los valores del grid
                    Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = true;
                    foreach (GridViewRow Row in Grid_Detalle_Impuesto_Fraccionamiento.Rows)
                    {
                        Dr_Impuestos = Dt_Impuestos.NewRow();
                        Dr_Impuestos["SUPERFICIE_FRACCIONAR"] = Convert.ToDouble(Row.Cells[0].Text);
                        Dr_Impuestos["IMPUESTO_FRACCIONAMIENTO_ID"] = Grid_Detalle_Impuesto_Fraccionamiento.DataKeys[Row.RowIndex].Value.ToString();// Row.Cells[1].Text;
                        Dr_Impuestos["DESCRIPCION_MONTO"] = HttpUtility.HtmlDecode(Row.Cells[2].Text).Trim();
                        Dr_Impuestos["MONTO_FRACCIONAMIENTO"] = Convert.ToDouble(Row.Cells[3].Text.Replace("$", ""));
                        Dr_Impuestos["MULTAS"] = Convert.ToDouble(Row.Cells[4].Text.Replace("$", ""));
                        Dr_Impuestos["IMPORTE"] = Convert.ToDouble(Row.Cells[5].Text.Replace("$", ""));
                        Dr_Impuestos["RECARGOS"] = Convert.ToDouble(Row.Cells[6].Text.Replace("$", ""));
                        Dr_Impuestos["TOTAL"] = Convert.ToDouble(Row.Cells[7].Text.Replace("$", ""));
                        Dr_Impuestos["FRA_MULTA_CUOTA_ID"] = HttpUtility.HtmlDecode(Row.Cells[8].Text);
                        Dt_Impuestos.Rows.Add(Dr_Impuestos);
                    }
                    Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = false;

                    //Se inserta el nuevo registro de Impuesto
                    Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = true;
                    Dr_Impuestos = Dt_Impuestos.NewRow();
                    Boolean Algun_Valor = false;
                    if (Txt_Superficie_Fraccionar.Text.Trim() != "")
                    {
                        Dr_Impuestos["SUPERFICIE_FRACCIONAR"] = Convert.ToDouble(Txt_Superficie_Fraccionar.Text);
                        Algun_Valor = true;
                    }
                    if (Txt_Costo_M2.Text.Trim() != "")
                    {
                        Dr_Impuestos["IMPUESTO_FRACCIONAMIENTO_ID"] = Hdf_Impuesto_Fraccionamiento_ID.Value;
                        Dr_Impuestos["DESCRIPCION_MONTO"] = HttpUtility.HtmlDecode(Txt_Descripcion_Costo_M2.Text).Trim();
                        Dr_Impuestos["MONTO_FRACCIONAMIENTO"] = Convert.ToDouble(Txt_Costo_M2.Text.Replace("$", ""));
                        Algun_Valor = true;
                    }
                    else
                    {
                        Dr_Impuestos["IMPUESTO_FRACCIONAMIENTO_ID"] = "";
                        Dr_Impuestos["DESCRIPCION_MONTO"] = "";
                        Dr_Impuestos["MONTO_FRACCIONAMIENTO"] = 0;
                    }
                    if (Txt_Multa.Text.Trim() != "")
                    {
                        Dr_Impuestos["MULTAS"] = Convert.ToDouble(Txt_Multa.Text);
                        Dr_Impuestos["FRA_MULTA_CUOTA_ID"] = Hdf_Multa.Value;
                        Algun_Valor = true;
                    }
                    if (Txt_Importe.Text.Trim() != "")
                    {
                        Dr_Impuestos["IMPORTE"] = Convert.ToDouble(Txt_Importe.Text);
                        Algun_Valor = true;
                    }
                    if (Txt_Recargos.Text.Trim() != "")
                    {
                        Dr_Impuestos["RECARGOS"] = Convert.ToDouble(Txt_Recargos.Text);
                        Algun_Valor = true;
                    }
                    if (Txt_Total.Text.Trim() != "")
                    {
                        Dr_Impuestos["TOTAL"] = Convert.ToDouble(Txt_Total.Text);
                        Algun_Valor = true;
                    }

                    if (Algun_Valor)
                    {
                        Dt_Impuestos.Rows.Add(Dr_Impuestos);

                        Grid_Detalle_Impuesto_Fraccionamiento.DataSource = Dt_Impuestos;
                        Grid_Detalle_Impuesto_Fraccionamiento.PageIndex = 0;
                        Grid_Detalle_Impuesto_Fraccionamiento.DataBind();
                        Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = false;

                        //Se limpian los textos de los Impuestos
                        Txt_Descripcion_Costo_M2.Text = "";
                        Txt_Costo_M2.Text = "";
                        Txt_Superficie_Fraccionar.Text = "";
                        Txt_Importe.Text = "";
                        Txt_Recargos.Text = "";
                        Txt_Total.Text = "";
                        Txt_Multa.Text = "";
                        Hdf_Multa.Value = "";

                        Calcular_Total_Impuestos();
                    }
                }
                //else
                //{
                //    //if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                //    //Mensaje_Error = Mensaje_Error + "+ Indique la Cuenta Predial.";
                //    Lbl_Ecabezado_Mensaje.Text = HttpUtility.HtmlDecode("+ La Superficie a Fraccionar debe ser Menor o Igual a la Superficie del Predio.");
                //    Lbl_Mensaje_Error.Text = "";
                //    Div_Contenedor_Msj_Error.Visible = true;
                //}
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Detalles_Impuestos_Fraccionamientos
    ///DESCRIPCIÓN          : Lee el grid de los Detalles de los Impuestos y devuelve una instancia en un DataTable
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 03/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Detalles_Impuestos_Fraccionamientos()
    {
        DataTable Dt_Impuestos = new DataTable();
        Dt_Impuestos.Columns.Add(new DataColumn("SUPERFICIE_FRACCIONAR", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("IMPUESTO_FRACCIONAMIENTO_ID", typeof(String)));
        Dt_Impuestos.Columns.Add(new DataColumn("DESCRIPCION_MONTO", typeof(String)));
        Dt_Impuestos.Columns.Add(new DataColumn("MONTO_FRACCIONAMIENTO", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("MULTAS", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("IMPORTE", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("RECARGOS", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("TOTAL", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("FRA_MULTA_CUOTA_ID", typeof(String)));

        DataRow Dr_Impuestos;
        //Se barre el Grid para cargar el DataTable con los valores del grid
        Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = true;
        foreach (GridViewRow Row in Grid_Detalle_Impuesto_Fraccionamiento.Rows)
        {
            Dr_Impuestos = Dt_Impuestos.NewRow();
            Dr_Impuestos["SUPERFICIE_FRACCIONAR"] = Convert.ToDouble(Row.Cells[0].Text);
            Dr_Impuestos["IMPUESTO_FRACCIONAMIENTO_ID"] = Grid_Detalle_Impuesto_Fraccionamiento.DataKeys[Row.RowIndex].Value.ToString();// Row.Cells[1].Text;
            Dr_Impuestos["DESCRIPCION_MONTO"] = Row.Cells[2].Text;
            Dr_Impuestos["MONTO_FRACCIONAMIENTO"] = Convert.ToDouble(Row.Cells[3].Text.Replace("$", ""));
            Dr_Impuestos["MULTAS"] = Convert.ToDouble(Row.Cells[4].Text.Replace("$", ""));
            Dr_Impuestos["IMPORTE"] = Convert.ToDouble(Row.Cells[5].Text.Replace("$", ""));
            Dr_Impuestos["RECARGOS"] = Convert.ToDouble(Row.Cells[6].Text.Replace("$", ""));
            Dr_Impuestos["TOTAL"] = Convert.ToDouble(Row.Cells[7].Text.Replace("$", ""));
            Dr_Impuestos["FRA_MULTA_CUOTA_ID"] = Row.Cells[8].Text.Replace("&amp;nbsp;", "0").Replace("&nbsp;", "0").Replace("&#160;", "0");
            Dt_Impuestos.Rows.Add(Dr_Impuestos);
        }
        Grid_Detalle_Impuesto_Fraccionamiento.Columns[8].Visible = false;
        return Dt_Impuestos;
    }

    protected void Txt_Cuenta_Predial_TextChanged()
    {
        DataTable Dt_Orden;
        if (Hdf_Cuenta_Predial_ID.Value.Length > 0)
        {
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuenta.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Cuenta = Cuenta.Consultar_Datos_Propietario();

            Txt_Calle.Text = Cuenta.P_Nombre_Calle;
            Txt_Propietario.Text = Cuenta.P_Nombre_Propietario;
            Txt_Colonia.Text = Cuenta.P_Nombre_Colonia;
            Txt_No_Exterior.Text = Cuenta.P_No_Exterior;
            Txt_No_Interior.Text = Cuenta.P_No_Interior;

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
                if (Dt_Orden.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString().Trim() != "")
                {
                    Txt_Propietario.Text = Dt_Orden.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString();
                }
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
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Imprimir_Click
    ///DESCRIPCIÓN          : Lee el grid de los Detalles de los Impuestos y devuelve una instancia en un DataTable
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 03/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        if (Hdf_No_Impuesto_Fraccionamiento.Value != "")
        {
            if (Grid_Impuestos_Fraccionamientos.Rows.Count > 0)
                Imprimir_Reporte(Crear_Ds_Impuestos_Fraccionamientos(), "Rpt_Pre_Impuestos_Fraccionamientos.rpt", "Impuestos Fraccionamientos");
        }
    }

    protected void Txt_Fecha_Oficio_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_Oficio = DateTime.Now;
        int dias_inhabiles = 0;
        try
        {
            if (Txt_Fecha_Oficio.Text.Trim() != "")
            {
                Fecha_Oficio = Convert.ToDateTime(Txt_Fecha_Oficio.Text);
                DateTime Fecha_hoy = DateTime.Now;
                if (DateTime.Compare(Fecha_Oficio, Fecha_hoy) == 1)
                {
                    Txt_Fecha_Oficio.Text = "";
                    Dtp_Fecha_Oficio.SelectedDate = null;
                    Txt_Fecha_Vencimiento.Text = "";
                    Dtp_Fecha_Vencimiento.SelectedDate = null;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Fraccionamiento", "alert('La fecha de oficio no puede ser mayor a la fecha actual. Seleccione otra fecha por favor.');", true);
                    return;
                }
                Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                Dtp_Fecha_Oficio.SelectedDate = Fecha_Oficio;
                //Fecha_Oficio = Convert.ToDateTime(Dtp_Fecha_Oficio.SelectedDate);
            }
            try
            {
                //if (Hdf_Fecha_Ya_Asignada.Value == "")
                //{
                Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
                Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                dias_inhabiles = Parametros.Consultar_Dias_Vencimiento();
                Fecha_Oficio = Fecha_Oficio.AddDays(dias_inhabiles);
                Txt_Fecha_Vencimiento.Text = Dias_Inhabiles.Calcular_Fecha("" + Fecha_Oficio.AddDays(-1), "1").ToString("dd/MMM/yyyy");
                Dtp_Fecha_Vencimiento.SelectedDate = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text);
                //Hdf_Fecha_Ya_Asignada.Value = "Falso";
                //}
                //else
                //{
                //    Hdf_Fecha_Ya_Asignada.Value = "";
                //}
            }
            catch (Exception exc)
            {
                Lbl_Ecabezado_Mensaje.Text = exc.Message.ToString() + "<br>Se fijo la fecha actual como fecha de vencimiento.";
                Txt_Fecha_Vencimiento.Text = Txt_Fecha_Oficio.Text;
            }
        }
        catch
        {
            Dtp_Fecha_Oficio.SelectedDate = null;
            Txt_Fecha_Oficio.Text = "";
            Dtp_Fecha_Vencimiento.SelectedDate = null;
            Txt_Fecha_Vencimiento.Text = "";
        }
    }

    private void Cargar_Datos()
    {
        try
        {
            if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
            {
                //KONSULTA DATOS CUENTA HACER DS
                Busqueda_Cuentas();
                //LLENAR CAJAS
                if (Session["Ds_Cuenta_Datos"] != null)
                {
                    Cargar_Generales_Cuenta(((DataSet)Session["Ds_Cuenta_Datos"]).Tables["Dt_Generales"]);
                    Busqueda_Propietario();
                }

            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio;

    private void Busqueda_Cuentas()
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataSet Ds_Cuenta;
        try
        {
            Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Ds_Cuenta = Resumen_Predio.Consulta_Datos_Cuenta_Generales();
            if (Ds_Cuenta.Tables[0].Rows.Count - 1 >= 0)
            {
                if (Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_Id"].ToString().Trim() != string.Empty)
                {
                    Session["Cuenta_Predial_ID"] = Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_Id"].ToString().Trim();
                }
            }
            if (Ds_Cuenta.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Cuenta_Datos");
                M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                Session["Ds_Cuenta_Datos"] = Ds_Cuenta;
            }
            else
            {
                //Mensaje_Error("No se encontraron los datos necesarios para la consulta de la cuenta");
                //Lbl_Mensaje_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }

    }


    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Generales_Cuenta
    ///DESCRIPCIÓN: asignar datos generales de cuenta a los controles y objeto de negocio
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Cargar_Generales_Cuenta(DataTable dataTable)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        try
        {
            Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            DataTable Dt_Ultimo_Movimiento = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ultimo_Movimiento();
            if (Dt_Ultimo_Movimiento.Rows.Count > 0)
            {
                if (Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString() != string.Empty)
                {
                    if (Dt_Ultimo_Movimiento.Rows[0]["descripcion"].ToString() != "APERTURA")
                    {
                        //Txt_Ultimo_Movimiento_General.Text = Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString().Trim();
                    }
                }
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "BLOQUEADA")
            {
                //Lbl_Estatus.Text = " BLOQUEADA" + " " + Lbl_Estatus.Text;
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "PENDIENTE")
            {
                //Lbl_Estatus.Text = " Cuenta No Generada";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Resumen de Predio", "alert('Cuenta No Generada')", true);

                //Bloquear_Controles();
            }
            //Txt_Cuenta_Origen.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            //M_Orden_Negocio.P_Cuenta_Origen = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Tipo_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                DataTable Dt_Tipo_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tipo_Predio();
                //Txt_Tipo_Periodo_Impuestos.Text = Dt_Tipo_Predio.Rows[0]["Descripcion"].ToString().Trim();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Uso_Suelo_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
                DataTable Dt_Uso_Suelo = Rs_Consulta_Ope_Resumen_Predio.Consultar_Uso_Predio();
                //Txt_Uso_Predio_General.Text = Dt_Uso_Suelo.Rows[0]["Descripcion"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
                DataTable Dt_Estado_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio();
                //Txt_Estado_Predio_General.Text = Dt_Estado_Predio.Rows[0]["Descripcion"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                DataTable Dt_Calles = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                Txt_Calle.Text = Dt_Calles.Rows[0]["Nombre"].ToString();
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = Dt_Calles.Rows[0]["Colonia_ID"].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Colonia.Text = Dt_Colonia.Rows[0]["Nombre"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString() != string.Empty)
            {
                Txt_No_Exterior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString() != string.Empty)
            {
                Txt_No_Interior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
    }


    private void Busqueda_Propietario()
    {
        DataSet Ds_Prop;
        String Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
        try
        {
            M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
            Ds_Prop = M_Orden_Negocio.Consulta_Datos_Propietario();
            if (Ds_Prop.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Prop_Datos");
                Session["Ds_Prop_Datos"] = Ds_Prop;
                //Cargar_Datos_Propietario(((DataSet)Session["Ds_Prop_Datos"]).Tables["Dt_Propietarios"]);
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }

    }

    protected void Btn_Mostrar_Busqueda_Avanzada_Costos_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["IMPUESTO_FRACCIONAMIENTO_COSTO_ID"] != null)
        {
            //Asigna los datos del impuesto seleccionado
            Hdf_Impuesto_Fraccionamiento_ID.Value = Session["IMPUESTO_FRACCIONAMIENTO_COSTO_ID"].ToString();
            Txt_Descripcion_Costo_M2.Text = Session["IMPUESTO_FRACCIONAMIENTO_COSTO_DESCRIPCION"].ToString();
            Txt_Costo_M2.Text = Session["IMPUESTO_FRACCIONAMIENTO_COSTO"].ToString();
            //Realiza los calculos
            Calcular_Importe();
            Calcular_Recargos();
            Calcular_Total();
            //Elimina las sessiones
            Session.Remove("IMPUESTO_FRACCIONAMIENTO_COSTO_ID");
            Session.Remove("IMPUESTO_FRACCIONAMIENTO_COSTO_DESCRIPCION");
            Session.Remove("IMPUESTO_FRACCIONAMIENTO_COSTO");
        }
    }

    protected void Btn_Multas_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["IMPUESTO_FRACCIONAMIENTO_MULTA_ID"] != null)
        {
            //Asigna los datos de la multa seleccionada
            Hdf_Multa.Value = Session["IMPUESTO_FRACCIONAMIENTO_MULTA_ID"].ToString();
            Txt_Multa.Text = Session["IMPUESTO_FRACCIONAMIENTO_MULTA"].ToString();
            //Realiza los calculos
            Calcular_Importe();
            Calcular_Recargos();
            Calcular_Total();
            //Elimina las sessiones
            Session.Remove("IMPUESTO_FRACCIONAMIENTO_MULTA_ID");
            Session.Remove("IMPUESTO_FRACCIONAMIENTO_MULTA");
        }
    }
}
