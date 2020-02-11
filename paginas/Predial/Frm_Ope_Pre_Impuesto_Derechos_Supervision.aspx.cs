using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Impuestos_Derechos_Supervision.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Catalogo_Derechos_Supervision.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Catalogo_Multas_Derechos_Supervision.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Operacion_Predial_Convenios_Impuestos_Traslado_Dominio.Negocio;
using Presidencia.Operacion_Predial_Impuestos_Fraccionamientos.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Impuesto_Derechos_Supervision : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            Session["Activa"] = true;//Variable para mantener la session activa.
            Hdf_Estatus.Value = "POR PAGAR";
            Configuracion_Formulario(true);
            Cargar_Grid_Impuestos_Derechos_Supervision(0, Hdf_Estatus.Value);
            //Llenar_Tabla_Impuestos_Derechos_Supervision(0);
            Txt_Anio.Text = "" + DateTime.Now.Year;
            Cargar_Multas(0);
            Cargar_Combo_Años(Cmb_Busqueda_Año, Orden_Datos.Descendente, DateTime.Now.Year);
            Btn_Busqueda_Avanzada_Tasas();

            Session.Remove("ESTATUS_CUENTAS");
            Session.Remove("TIPO_CONTRIBUYENTE");

            //Scrip para mostrar Ventana Modal de las Tasas de Traslado
            Session["ESTATUS_CUENTAS"] = "IN ('PENDIENTE','ACTIVA','VIGENTE','BLOQUEADA','SUSPENDIDA')";
            String Ventana_Modal;
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal);
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
    public String No_Impuesto_Derecho_Supervision
    {
        get
        {
            return Hdf_No_Impuesto_Derecho_Supervision.Value;
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
    ///FECHA_CREO           : 23/Julio/2011
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
        Grid_Impuestos_Derechos_Supervision.Enabled = Estatus;
        Grid_Detalle_Impuesto_Derechos_Supervision.Enabled = !Estatus;
        Grid_Detalle_Impuesto_Derechos_Supervision.SelectedIndex = (-1);
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
        Txt_Tipo_Predio.Enabled = false;
        Txt_Calle.Enabled = false;
        Txt_Colonia.Enabled = false;
        Txt_No_Exterior.Enabled = false;
        Txt_No_Interior.Enabled = false;
        Txt_Propietario.Enabled = false;
        Txt_Tasa_Descripcion.Enabled = false;
        Txt_Tasa.Enabled = false;
        Txt_Importe.Enabled = false;
        Txt_Total.Enabled = false;
        Txt_Valor_Estimado_Obra.Enabled = !Estatus;
        Txt_Fecha_Oficio.Enabled = !Estatus;
        Btn_Fecha_Oficio.Enabled = !Estatus;
        Txt_Fecha_Vencimiento.Enabled = false;
        Txt_Recargos.Enabled = false;
        Txt_Observaciones.Enabled = !Estatus;
        Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = !Estatus;
        Btn_Detalles_Cuenta_Predial.Enabled = !Estatus;
        Btn_Mostrar_Busqueda_Avanzada_Tasas.Enabled = !Estatus;
        Btn_Agregar_Impuesto.Enabled = !Estatus;
        Txt_Total_Impuestos_Grid.Enabled = false;

        Txt_Valor_Estimado_Obra.Style["text-align"] = "right";
        Txt_Tasa.Style["text-align"] = "right";
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
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        Hdf_Cuenta_Predial_ID.Value = "";
        Hdf_No_Impuesto_Derecho_Supervision.Value = "";
        Hdf_Multa.Value = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Cuenta_Predial.Text = "";
        Txt_Superficie_Predio.Text = "";
        Txt_Tipo_Predio.Text = "";
        Txt_Importe.Text = "";
        Txt_Calle.Text = "";
        Txt_Colonia.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Propietario.Text = "";
        Txt_Total.Text = "";
        Txt_Multa.Text = "";
        Txt_Valor_Estimado_Obra.Text = "";
        Txt_Tasa_Descripcion.Text = "";
        Txt_Tasa.Text = "";
        Dtp_Fecha_Oficio.SelectedDate = null;
        Txt_Fecha_Oficio.Text = "";
        Dtp_Fecha_Vencimiento.SelectedDate = null;
        Txt_Fecha_Vencimiento.Text = "";
        Txt_Recargos.Text = "";
        Txt_Observaciones.Text = "";
        Grid_Detalle_Impuesto_Derechos_Supervision.DataSource = null;
        Grid_Detalle_Impuesto_Derechos_Supervision.DataBind();
        Txt_Total_Impuestos_Grid.Text = "";
        Hdf_Fecha_Ya_Asignada.Value = "";

        Session["Cuenta_Predial"] = null;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Impuestos_Derechos_Supervision
    ///DESCRIPCIÓN          : Llena la tabla de Impuestos de Fraccionamientos con los registros encontrados.
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Impuestos_Derechos_Supervision(Int32 Pagina, String Por_Estatus)
    {
        try
        {
            Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio Impuestos_Derecho_Supervision = new Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio();
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos = "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID + ") AS Cuenta_Predial, ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + ", ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID + ", ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Oficio + ", ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Vencimiento + ", ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Creo + ", ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus;
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Impuestos_Derecho_Supervision.P_Filtros_Dinamicos = "(";
                Impuestos_Derecho_Supervision.P_Filtros_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + " = '" + Txt_Busqueda.Text.Trim() + "'";
                Impuestos_Derecho_Supervision.P_Filtros_Dinamicos += " OR " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Txt_Busqueda.Text.Trim() + "')";
                Impuestos_Derecho_Supervision.P_Filtros_Dinamicos += ")";
            }
            if (Impuestos_Derecho_Supervision.P_Filtros_Dinamicos != null && Impuestos_Derecho_Supervision.P_Filtros_Dinamicos != "")
            {
                Impuestos_Derecho_Supervision.P_Filtros_Dinamicos += " AND ";
            }
            if (Impuestos_Derecho_Supervision.P_Filtros_Dinamicos != null && Impuestos_Derecho_Supervision.P_Filtros_Dinamicos != "")
            {
                Impuestos_Derecho_Supervision.P_Filtros_Dinamicos += "(" + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR' OR " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'CANCELADA' OR " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'SIN PAGAR' OR " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'IMPRESA' OR ";
                Impuestos_Derecho_Supervision.P_Filtros_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'PAGADO') ";
            }
            else
            {
                Impuestos_Derecho_Supervision.P_Filtros_Dinamicos = "(" + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = '" + Por_Estatus + "' OR " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'CANCELADA' OR " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'SIN PAGAR' OR " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'IMPRESA' OR ";
                Impuestos_Derecho_Supervision.P_Filtros_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'PAGADO') ";
            }
            DataTable Tabla = Impuestos_Derecho_Supervision.Consultar_Impuestos_Derecho_Supervisions();
            if (Tabla != null)
            {
                Grid_Impuestos_Derechos_Supervision.Columns[7].Visible = true;
                Grid_Impuestos_Derechos_Supervision.DataSource = Tabla;
                Grid_Impuestos_Derechos_Supervision.PageIndex = Pagina;
                Grid_Impuestos_Derechos_Supervision.DataBind();
                Grid_Impuestos_Derechos_Supervision.Columns[7].Visible = false;
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
    private void Llenar_Tabla_Detalles_Impuestos_Derechos_Supervision(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio Impuestos_Derecho_Supervision = new Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio();
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos = "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID + ") AS Cuenta_Predial, ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID + ") AS Superficie_Construida, ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID + ") AS Superficie_Total, ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Tipos_Predio.Campo_Descripcion + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " WHERE " + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + "." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID + ")) AS Tipo_Predio, ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + ", ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID + ", ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Oficio + ", ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Vencimiento + ", ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + ", ";
            Impuestos_Derecho_Supervision.P_Campos_Dinamicos += Ope_Pre_Impuestos_Derechos_Supervision.Campo_Observaciones;
            //Impuestos_Derecho_Supervision.P_Filtros_Dinamicos = Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + " LIKE '%" + Txt_Busqueda.Text.Trim() + "%'";
            Impuestos_Derecho_Supervision.P_No_Impuesto_Derecho_Supervision = Hdf_No_Impuesto_Derecho_Supervision.Value;// Convert.ToInt32(Txt_Busqueda.Text.Trim()).ToString("0000000000");
            //Impuestos_Derecho_Supervision.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            DataTable Tabla = Impuestos_Derecho_Supervision.Consultar_Impuestos_Derecho_Supervisions();
            if (Tabla != null)
            {
                Hdf_No_Impuesto_Derecho_Supervision.Value = Tabla.Rows[0][Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision].ToString();
                Hdf_Cuenta_Predial_ID.Value = Tabla.Rows[0][Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID].ToString();
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
                Cmb_Estatus.SelectedValue = Tabla.Rows[0][Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus].ToString();
                //Txt_Fecha_Vencimiento.Text = Tabla.Rows[0][Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Vencimiento].ToString();
                Dtp_Fecha_Vencimiento.SelectedDate = Convert.ToDateTime(Tabla.Rows[0][Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Vencimiento].ToString());
                if (Tabla.Rows[0][Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Oficio].ToString().Length != 0)
                {
                    Dtp_Fecha_Oficio.SelectedDate = Convert.ToDateTime(Tabla.Rows[0][Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Oficio].ToString());
                }
                else
                {
                    Txt_Fecha_Oficio.Text = "----------";
                }
                Txt_Observaciones.Text = Tabla.Rows[0][Ope_Pre_Impuestos_Derechos_Supervision.Campo_Observaciones].ToString();
                Grid_Detalle_Impuesto_Derechos_Supervision.Columns[8].Visible = true;
                DataTable Dt_Ayudante;
                Dt_Ayudante = Impuestos_Derecho_Supervision.P_Dt_Detalles_Impuestos_Derechos_Supervision;
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
                Grid_Detalle_Impuesto_Derechos_Supervision.DataSource = Dt_Ayudante;
                Grid_Detalle_Impuesto_Derechos_Supervision.PageIndex = Pagina;
                Grid_Detalle_Impuesto_Derechos_Supervision.DataBind();
                Grid_Detalle_Impuesto_Derechos_Supervision.Columns[8].Visible = false;

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
        foreach (GridViewRow Row in Grid_Detalle_Impuesto_Derechos_Supervision.Rows)
        {
            if (Row.Cells[7].Text != "" && Row.Cells[7].Text != "&nbsp;")
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
    ///FECHA_CREO           : 23/Julio/2011
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
        if (Grid_Detalle_Impuesto_Derechos_Supervision.Rows.Count == 0)
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
    ///NOMBRE DE LA FUNCIÓN : Grid_Impuestos_Derechos_Supervision_PageIndexChanging1
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Impuestos_Derechos_Supervision_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {
        try
        {
            String Estatus = Hdf_Estatus.Value;
            Grid_Impuestos_Derechos_Supervision.SelectedIndex = (-1);
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Estatus = "";
            }
            Cargar_Grid_Impuestos_Derechos_Supervision(e.NewPageIndex, Estatus);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Impuestos_Derechos_Supervision_SelectedIndexChanged
    ///DESCRIPCIÓN          : Maneja la selección de las filas del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Impuestos_Derechos_Supervision_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Impuestos_Derechos_Supervision.Rows.Count > 0)
        {
            Hdf_No_Impuesto_Derecho_Supervision.Value = Grid_Impuestos_Derechos_Supervision.SelectedRow.Cells[1].Text;
            Hdf_Cuenta_Predial_ID.Value = Grid_Impuestos_Derechos_Supervision.DataKeys[Grid_Impuestos_Derechos_Supervision.SelectedIndex].Value.ToString();
            Llenar_Tabla_Detalles_Impuestos_Derechos_Supervision(0);
            Txt_Cuenta_Predial_TextChanged();
            Grid_Impuestos_Derechos_Supervision.Visible = false;
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
    ///NOMBRE DE LA FUNCIÓN : Grid_Detalles_Impuesto_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de los Tipos_Constancias
    ///PARAMETROS:
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Detalles_Impuesto_Derechos_Supervision_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Detalle_Impuesto_Derechos_Supervision.SelectedIndex = (-1);
            Llenar_Tabla_Detalles_Impuestos_Derechos_Supervision(e.NewPageIndex);
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
    ///NOMBRE DE LA FUNCIÓN : Grid_Detalles_Impuesto_Derechos_Supervision_RowCommand
    ///DESCRIPCIÓN          : Evento RowCommand para procesas los diferentes botones de comando en el gridview
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Detalles_Impuesto_Derechos_Supervision_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //protected void Grid_Detalles_Impuesto_Derechos_Supervision_RowCommand(object sender, EventArgs e)
        //{
        switch (e.CommandName)
        {
            case "Eliminar":

                int PageIndex = Grid_Detalle_Impuesto_Derechos_Supervision.PageIndex;

                //Imprimir_Reporte(Crear_Ds_Constancias(Convert.ToInt32(e.CommandArgument)), "Rpt_Pre_Constancias.rpt", "Impuestos de Derechos de Supervision");
                DataTable Dt_Impuestos = new DataTable();
                Dt_Impuestos.Columns.Add(new DataColumn("VALOR_ESTIMADO_OBRA", typeof(Double)));
                Dt_Impuestos.Columns.Add(new DataColumn("DERECHO_SUPERVISION_TASA_ID", typeof(String)));
                Dt_Impuestos.Columns.Add(new DataColumn("DESCRIPCION_TASA", typeof(String)));
                Dt_Impuestos.Columns.Add(new DataColumn("MULTAS", typeof(Double)));
                Dt_Impuestos.Columns.Add(new DataColumn("TASA_DERECHO_SUPERVISION", typeof(Double)));
                Dt_Impuestos.Columns.Add(new DataColumn("IMPORTE", typeof(Double)));
                Dt_Impuestos.Columns.Add(new DataColumn("RECARGOS", typeof(Double)));
                Dt_Impuestos.Columns.Add(new DataColumn("TOTAL", typeof(Double)));
                Dt_Impuestos.Columns.Add(new DataColumn("DER_MULTA_ID", typeof(String)));
                
                DataRow Dr_Impuestos;
                //Se barre el Grid para cargar el DataTable con los valores del grid
                Grid_Detalle_Impuesto_Derechos_Supervision.Columns[8].Visible = true;
                foreach (GridViewRow Row in Grid_Detalle_Impuesto_Derechos_Supervision.Rows)
                {
                    if (Convert.ToInt32(e.CommandArgument) != Row.DataItemIndex)
                    {
                        Dr_Impuestos = Dt_Impuestos.NewRow();
                        Dr_Impuestos["VALOR_ESTIMADO_OBRA"] = Convert.ToDouble(Row.Cells[0].Text.Replace("$", ""));
                        Dr_Impuestos["DERECHO_SUPERVISION_TASA_ID"] = Grid_Detalle_Impuesto_Derechos_Supervision.DataKeys[Row.RowIndex].Value.ToString();// Row.Cells[1].Text;
                        Dr_Impuestos["DESCRIPCION_TASA"] = Row.Cells[2].Text;
                        Dr_Impuestos["TASA_DERECHO_SUPERVISION"] = Convert.ToDouble(Row.Cells[3].Text.Replace("%", ""))/100;
                        Dr_Impuestos["MULTAS"] = Convert.ToDouble(Row.Cells[4].Text.Replace("$", ""));
                        Dr_Impuestos["IMPORTE"] = Convert.ToDouble(Row.Cells[5].Text.Replace("$", ""));
                        Dr_Impuestos["RECARGOS"] = Convert.ToDouble(Row.Cells[6].Text.Replace("$", ""));
                        Dr_Impuestos["TOTAL"] = Convert.ToDouble(Row.Cells[7].Text.Replace("$", ""));
                        Dr_Impuestos["DER_MULTA_ID"] = HttpUtility.HtmlDecode(Row.Cells[8].Text);
                        Dt_Impuestos.Rows.Add(Dr_Impuestos);
                    }
                }

                Grid_Detalle_Impuesto_Derechos_Supervision.DataSource = Dt_Impuestos;
                if (PageIndex >= Grid_Detalle_Impuesto_Derechos_Supervision.PageCount)
                {
                    Grid_Detalle_Impuesto_Derechos_Supervision.PageIndex = PageIndex - 1;
                }
                else
                {
                    Grid_Detalle_Impuesto_Derechos_Supervision.PageIndex = PageIndex;
                }
                Grid_Detalle_Impuesto_Derechos_Supervision.DataBind();
                Grid_Detalle_Impuesto_Derechos_Supervision.Columns[8].Visible = false;

                Calcular_Total_Impuestos();
                break;
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Impuestos_Derecho_Supervision
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Julio/2011
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
                Grid_Impuestos_Derechos_Supervision.Visible = false;
                Cmb_Estatus.SelectedIndex = 1;
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio Impuestos_Derecho_Supervision = new Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio();
                    Impuestos_Derecho_Supervision.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Impuestos_Derecho_Supervision.P_Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text.Trim());
                    Impuestos_Derecho_Supervision.P_Fecha_Oficio = Convert.ToDateTime(Txt_Fecha_Oficio.Text.Trim());
                    Cmb_Estatus.SelectedIndex = 2;
                    Impuestos_Derecho_Supervision.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Impuestos_Derecho_Supervision.P_Observaciones = Txt_Observaciones.Text.Trim().ToUpper();
                    Impuestos_Derecho_Supervision.P_Dt_Detalles_Impuestos_Derechos_Supervision = Crear_Tabla_Detalles_Impuestos_Derechos_Cupervision();
                    Impuestos_Derecho_Supervision.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    if (Impuestos_Derecho_Supervision.Alta_Impuestos_Derecho_Supervision())
                    {
                        Insertar_Pasivo("DER" + String.Format("{0:yy}", DateTime.Now) + Convert.ToInt32(Impuestos_Derecho_Supervision.P_No_Impuesto_Derecho_Supervision));
                        Grid_Impuestos_Derechos_Supervision.Visible = true;
                        Cargar_Grid_Impuestos_Derechos_Supervision(0, Hdf_Estatus.Value);
                        //Mpe_Busqueda_Multas.Dispose();
                        //Mpe_Busqueda_Tasas.Dispose();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Derechos de Supervisión", "alert('Alta de Derecho de Supervisión Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.Visible = true;
                        Btn_Imprimir.Visible = true;
                        Btn_Convenio.Visible = true;
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Hdf_No_Impuesto_Derecho_Supervision.Value = Impuestos_Derecho_Supervision.P_No_Impuesto_Derecho_Supervision;
                        Hdf_Cuenta_Predial_ID.Value = Impuestos_Derecho_Supervision.P_Cuenta_Predial_ID;
                        if (Hdf_No_Impuesto_Derecho_Supervision.Value != "")
                        {
                            Imprimir_Reporte(Crear_Ds_Impuestos_Derechos_Supervision(), "Rpt_Pre_Impuestos_Derechos_Supervision.rpt", "Derechos_Supervision");
                            //Impuestos Derechos Supervision
                        }
                        Limpiar_Controles();
                        Configuracion_Formulario(true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Derechos de Supervisión", "alert('Alta de Derecho de Supervisión No fue Exitosa');", true);
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


            foreach (GridViewRow Dt_Detalles in Grid_Detalle_Impuesto_Derechos_Supervision.Rows)
            {
                Multas += Convert.ToDouble(Dt_Detalles.Cells[4].Text.Replace("$", ""));//Multas
                Recargos_Ord += Convert.ToDouble(Dt_Detalles.Cells[6].Text.Replace("$", ""));//recargos
                Impuestos += Convert.ToDouble(Dt_Detalles.Cells[5].Text.Replace("$", ""));//Impuestos

            }

            Calculo_Impuesto_Traslado.P_Referencia = Referencia + "' AND " + Ope_Ing_Pasivo.Campo_Descripcion + " NOT LIKE '%DESCUENTO%";
            Calculo_Impuesto_Traslado.Eliminar_Referencias_Pasivo();

            if (Impuestos > 0)
            {
                Claves_Ingreso.P_Tipo = "DERECHOS DE SUPERVISION";
                Claves_Ingreso.P_Tipo_Predial_Traslado = "IMPUESTO";
                Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                if (Dt_Clave.Rows.Count > 0)
                {
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "DERECHOS DE SUPERVISION";
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
                Claves_Ingreso.P_Tipo = "DERECHOS DE SUPERVISION";
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
                Claves_Ingreso.P_Tipo = "DERECHOS DE SUPERVISION";
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
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Impuestos_Derecho_Supervision.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Impuestos_Derechos_Supervision.SelectedIndex > -1)
        {
            Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio Impuesto_En_Convenio = new Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio();
            Impuesto_En_Convenio.P_No_Impuesto_Derecho_Supervision = Hdf_No_Impuesto_Derecho_Supervision.Value;
            DataTable Tabla;
            Tabla = Impuesto_En_Convenio.Consultar_Impuestos_Con_Convenio();
            if (Tabla == null || Tabla.Rows.Count == 0)
            {
                try
                {
                    if (!Grid_Impuestos_Derechos_Supervision.SelectedRow.Cells[6].Text.Equals("PAGADO"))
                    {
                        if (Btn_Modificar.AlternateText.Equals("Modificar"))
                        {
                            if (Hdf_No_Impuesto_Derecho_Supervision.Value != "")
                            {
                                Btn_Modificar.AlternateText = "Actualizar";
                                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                                Btn_Salir.AlternateText = "Cancelar";
                                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                                Btn_Nuevo.Visible = false;
                                Btn_Imprimir.Visible = false;
                                Btn_Convenio.Visible = false;
                                Configuracion_Formulario(false);
                                Grid_Impuestos_Derechos_Supervision.Visible = false;
                                Cmb_Estatus.Items.RemoveAt(1);
                                Cmb_Estatus.Items.RemoveAt(2);
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
                                Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio Impuestos_Derecho_Supervision = new Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio();
                                Impuestos_Derecho_Supervision.P_No_Impuesto_Derecho_Supervision = Hdf_No_Impuesto_Derecho_Supervision.Value;
                                Impuestos_Derecho_Supervision.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                                //Impuestos_Derecho_Supervision.P_Fecha = Convert.ToDateTime(Txt_Fecha.Text.Trim());
                                Impuestos_Derecho_Supervision.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                                Impuestos_Derecho_Supervision.P_Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text.Trim());
                                Impuestos_Derecho_Supervision.P_Fecha_Oficio = Convert.ToDateTime(Txt_Fecha_Oficio.Text.Trim());
                                Impuestos_Derecho_Supervision.P_Observaciones = Txt_Observaciones.Text.Trim().ToUpper();
                                Impuestos_Derecho_Supervision.P_Dt_Detalles_Impuestos_Derechos_Supervision = Crear_Tabla_Detalles_Impuestos_Derechos_Cupervision();
                                Impuestos_Derecho_Supervision.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                                if (Impuestos_Derecho_Supervision.Modificar_Impuestos_Derecho_Supervision())
                                {
                                    if (Cmb_Estatus.SelectedValue != "IMPRESA")
                                    {
                                        int Numero_Impuesto;
                                        int.TryParse(Impuestos_Derecho_Supervision.P_No_Impuesto_Derecho_Supervision, out Numero_Impuesto);
                                        //Impuestos_Derecho_Supervision.Cancelar_Pasivo("DER" + Grid_Impuestos_Derechos_Supervision.SelectedRow.Cells[7].Text.Substring(9) + Numero_Impuesto, Cmb_Estatus.SelectedValue, Txt_Total_Impuestos_Grid.Text.Replace("$", ""));
                                        Insertar_Pasivo("DER" + Grid_Impuestos_Derechos_Supervision.SelectedRow.Cells[7].Text.Substring(9) + Numero_Impuesto);
                                    }

                                    if (Hdf_No_Impuesto_Derecho_Supervision.Value != "")
                                    {
                                        Imprimir_Reporte(Crear_Ds_Impuestos_Derechos_Supervision(), "Rpt_Pre_Impuestos_Derechos_Supervision.rpt", "Derechos_Supervision");
                                    }

                                    Limpiar_Controles();
                                    Cargar_Grid_Impuestos_Derechos_Supervision(0, Hdf_Estatus.Value);
                                    //Mpe_Busqueda_Multas.Dispose();
                                    //Mpe_Busqueda_Tasas.Dispose();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Derechos de Supervisión", "alert('Actualización de Derecho de Supervisión Exitosa');", true);
                                    Btn_Modificar.AlternateText = "Modificar";
                                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                                    Btn_Nuevo.Visible = true;
                                    Btn_Imprimir.Visible = true;
                                    Btn_Convenio.Visible = true;
                                    Btn_Salir.AlternateText = "Salir";
                                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                                    Configuracion_Formulario(true);
                                    Grid_Impuestos_Derechos_Supervision.Visible = true;
                                    Cmb_Estatus.Items.Insert(1, new ListItem("SIN PAGAR", "SIN PAGAR"));
                                    Cmb_Estatus.Items.Insert(3, new ListItem("PAGADO", "PAGADO"));
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Derechos de Supervisión", "alert('Actualización de Derecho de Supervisión No fue Exitosa');", true);
                                }
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Derechos de Supervisión", "alert('El Derecho de Supervisión se encuentra pagado.');", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Derechos de Supervisión", "alert('El Derecho de Supervisión se encuentra convenido.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Derechos de Supervisión", "alert('Seleccione un Derecho de Supervisión por favor');", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Constancia_Propiedad_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Controles();
            Grid_Impuestos_Derechos_Supervision.SelectedIndex = (-1);
            Grid_Detalle_Impuesto_Derechos_Supervision.SelectedIndex = (-1);
            Cargar_Grid_Impuestos_Derechos_Supervision(0, Hdf_Estatus.Value);
            //Llenar_Tabla_Detalles_Impuestos_Derechos_Supervision(0);
            if (Grid_Impuestos_Derechos_Supervision.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda de \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "";
                //Lbl_Mensaje_Error.Text = "(Se cargarón todos los Impuestos de Derechos de Supervisión encontrados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                //Llenar_Tabla_Detalles_Impuestos_Derechos_Supervision(0);
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
    /////DESCRIPCIÓN          : Elimina un Impuestos_Derecho_Supervision de la Base de Datos
    /////PARAMETROS          :     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 23/Julio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Eliminar_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Grid_Impuestos_Derechos_Supervision.Rows.Count > 0 && Grid_Impuestos_Derechos_Supervision.SelectedIndex > (-1))
    //        {
    //            Cls_Ope_Pre_Constancias_Negocio Impuestos_Derecho_Supervision = new Cls_Ope_Pre_Constancias_Negocio();
    //            Impuestos_Derecho_Supervision.P_Folio = Grid_Impuestos_Derechos_Supervision.SelectedRow.Cells[3].Text;
    //            if (Impuestos_Derecho_Supervision.Eliminar_Constancia_Propiedad())
    //            {
    //                Grid_Impuestos_Derechos_Supervision.SelectedIndex = (-1);
    //                Llenar_Tabla_Constancias_Propiedad(Grid_Impuestos_Derechos_Supervision.PageIndex);
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Derechos de Supervisión", "alert('Impuestos de Derecho de Supervisión fue Eliminada Exitosamente');", true);
    //                Limpiar_Catalogo();
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Derechos de Supervisión", "alert('La Impuestos de Derecho de Supervisión No fue Eliminada');", true);
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
    ///FECHA_CREO           : 23/Julio/2011
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
            Grid_Impuestos_Derechos_Supervision.Visible = true;
            Grid_Impuestos_Derechos_Supervision.SelectedIndex = -1;
            Btn_Salir.AlternateText = "Salir";
        }
        else
        {
            //Mpe_Busqueda_Multas.Dispose();
            //Mpe_Busqueda_Tasas.Dispose();
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
            Cargar_Grid_Impuestos_Derechos_Supervision(0, Hdf_Estatus.Value);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Impuestos_Derechos_Supervision.Visible = true;
            Grid_Impuestos_Derechos_Supervision.SelectedIndex = -1;
            Panel_Datos.Visible = false;
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
    ///FECHA_CREO           : 23/Julio/2011
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
    ///FECHA_CREO           : 23/Julio/2011
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
    ///FECHA_CREO           : 03/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private double Calcular_Importe()
    {
        Double Tasa = 0;
        Double Valor_Estimado = 0;
        Double Importe = 0;

        if (Txt_Valor_Estimado_Obra.Text.Trim() != "")
        {
            Valor_Estimado = Convert.ToDouble(Txt_Valor_Estimado_Obra.Text);
        }
        else
        {
            Txt_Valor_Estimado_Obra.Text = "0.00";
        }
        if (Txt_Tasa.Text.Trim() != "")
        {
            Tasa = Convert.ToDouble(Txt_Tasa.Text) / 100;
        }
        else
        {
            Txt_Tasa.Text = "0.00";
        }
        Importe = Tasa * Valor_Estimado;
        Txt_Importe.Text = Importe.ToString("0.00");

        return Importe;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Calcular_Recargos
    ///DESCRIPCIÓN          : Realiza el cálculo de los Recargos
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 03/Agosto/2011
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


    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Recalcular_Recargos
    ///DESCRIPCIÓN          : Realiza el re-cálculo de los Recargos
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 13/Septiembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private double Recalcular_Recargos()
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
    ///FECHA_CREO           : 03/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private double Calcular_Total()
    {
        Double Importe = 0;
        Double Recargos = 0;
        Double Multas = 0;
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
        if (Txt_Multa.Text.Trim() != "")
        {
            Multas = Convert.ToDouble(Txt_Multa.Text);
        }
        else
        {
            Txt_Multa.Text = "0.00";
        }
        Total = Importe + Recargos + Multas;
        Txt_Total.Text = Total.ToString("0.00");

        return Total;
    }

    protected void Txt_Valor_Estimado_Obra_TextChanged(object sender, EventArgs e)
    {
        if (Txt_Fecha_Oficio.Text != "")
        {
            Calcular_Importe();
            Calcular_Recargos();
            Calcular_Total();
            Txt_Valor_Estimado_Obra.Text = Convert.ToDouble(Txt_Valor_Estimado_Obra.Text).ToString("###,###,###,##0.00");
        }
        else
        {
            Txt_Valor_Estimado_Obra.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Derechos de Supervisión", "alert('Seleccione una fecha de oficio por favor');", true);
        }
    }

    protected void Txt_Tasa_TextChanged(object sender, EventArgs e)
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
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataTable Dt_Impuestos_Derechos_Supervision, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Reporte.SetDataSource(Dt_Impuestos_Derechos_Supervision);

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
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Impuestos_Derechos_Supervision, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Impuestos_Derechos_Supervision);
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
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos de la Impuestos de Derecho de Supervisión Seleccionada en el GridView
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Crear_Dt_Constancias_Propiedad(int Indice_Fila)
    {
        Ds_Pre_Constancias Ds_Impuestos_Derechos_Supervision = new Ds_Pre_Constancias();
        DataRow Dr_Constancias_Propiedad;

        //Inserta los datos de la Impuestos de Derecho de Supervisión en la Tabla
        Dr_Constancias_Propiedad = Ds_Impuestos_Derechos_Supervision.Tables["Dt_Constancias_Propiedad"].NewRow();
        Dr_Constancias_Propiedad["Cuenta_Predial"] = Grid_Detalle_Impuesto_Derechos_Supervision.Rows[Indice_Fila].Cells[1].Text;
        Dr_Constancias_Propiedad["Propietario"] = Grid_Detalle_Impuesto_Derechos_Supervision.Rows[Indice_Fila].Cells[3].Text;
        Dr_Constancias_Propiedad["Folio"] = Grid_Detalle_Impuesto_Derechos_Supervision.Rows[Indice_Fila].Cells[4].Text;
        Dr_Constancias_Propiedad["Fecha"] = Grid_Detalle_Impuesto_Derechos_Supervision.Rows[Indice_Fila].Cells[5].Text;

        Ds_Impuestos_Derechos_Supervision.Tables["Dt_Constancias_Propiedad"].Rows.Add(Dr_Constancias_Propiedad);

        return Ds_Impuestos_Derechos_Supervision.Tables["Dt_Constancias_Propiedad"];
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Constancias_Propiedad
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos de la Impuestos de Derecho de Supervisión Seleccionada en el GridView
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 03/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Impuestos_Derechos_Supervision()
    {
        Ds_Pre_Impuestos_Derechos_Supervision Ds_Impuestos_Derechos_Supervision = new Ds_Pre_Impuestos_Derechos_Supervision();

        Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio Impuestos_Derechos_Supervision = new Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio();
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Cat_Pre_Tipos_Predio_Negocio Tipos_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();
        Cls_Cat_Pre_Derechos_Supervision_Negocio Derechos_Supervision = new Cls_Cat_Pre_Derechos_Supervision_Negocio();

        //DataTable Dt_Impuestos_Derechos_Supervision;
        DataTable Dt_Cuenta_Predial = null;
        //DataTable Dt_Tipo_Predio;
        DataTable Dt_Temp;
        DataTable Dt_Temp_Detalles = null;
        DataRow Dr_Impuestos_Derechos_Supervision;

        String Derechos_Supervision_Tasas_ID = "";

        //Cuentas_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        //Dt_Cuenta_Predial = Cuentas_Predial.Consultar_Cuenta();

        //Tipos_Predio.P_Filtros_Dinamicos = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = '" + Dt_Cuenta_Predial.Rows[0][Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID] + "'";
        //Dt_Tipo_Predio = Tipos_Predio.Consultar_Tipo_Predio();

        foreach (DataTable Dt_Impuestos_Derechos_Supervision in Ds_Impuestos_Derechos_Supervision.Tables)
        {
            if (Dt_Impuestos_Derechos_Supervision.TableName == "Dt_Impuestos_Derechos_Supervision")
            {
                Impuestos_Derechos_Supervision.P_No_Impuesto_Derecho_Supervision = Hdf_No_Impuesto_Derecho_Supervision.Value;
                Dt_Temp = Impuestos_Derechos_Supervision.Consultar_Impuestos_Derecho_Supervisions();
                Dt_Temp_Detalles = Impuestos_Derechos_Supervision.P_Dt_Detalles_Impuestos_Derechos_Supervision;

                foreach (DataRow Dr_Temp in Dt_Temp.Rows)
                {
                    int Numero_Impuesto;
                    int.TryParse(Dr_Temp[Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision].ToString(), out Numero_Impuesto);
                    //Inserta los datos de los Impuestos de Derechos de Supervision en la Tabla
                    Dr_Impuestos_Derechos_Supervision = Dt_Impuestos_Derechos_Supervision.NewRow();
                    Dr_Impuestos_Derechos_Supervision["NO_IMPUESTO_DERECHO_SUPERVISIO"] = Numero_Impuesto;
                    Dr_Impuestos_Derechos_Supervision["CUENTA_PREDIAL_ID"] = Dr_Temp[Ope_Pre_Impuestos_Derechos_Supervision.Campo_Cuenta_Predial_ID];
                    Dr_Impuestos_Derechos_Supervision["FECHA_VENCIMIENTO"] = Dr_Temp[Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Vencimiento].ToString().Substring(0, 10);
                    Dr_Impuestos_Derechos_Supervision["ESTATUS"] = Dr_Temp[Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus];
                    Dr_Impuestos_Derechos_Supervision["FECHA_ELABORACION"] = Dr_Temp[Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Creo].ToString().Substring(0, 10);
                    Dr_Impuestos_Derechos_Supervision["FECHA_OFICIO"] = Dr_Temp[Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Oficio].ToString().Substring(0, 10);
                    Dr_Impuestos_Derechos_Supervision["ELABORO"] = Dr_Temp[Ope_Pre_Impuestos_Derechos_Supervision.Campo_Usuario_Creo];
                    Dr_Impuestos_Derechos_Supervision["OBSERVACIONES"] = Dr_Temp[Ope_Pre_Impuestos_Derechos_Supervision.Campo_Observaciones];
                    Dr_Impuestos_Derechos_Supervision["UBICACION"] = Txt_Colonia.Text + ", " + Txt_Calle.Text + ", NO. EXT. " + Txt_No_Exterior.Text + ", NO. INT. " + Txt_No_Interior.Text;
                    Dr_Impuestos_Derechos_Supervision["PROPIETARIO"] = Txt_Propietario.Text;
                    Dt_Impuestos_Derechos_Supervision.Rows.Add(Dr_Impuestos_Derechos_Supervision);
                }
            }
            if (Dt_Impuestos_Derechos_Supervision.TableName == "Dt_Detalles_Impuestos_Derechos_Supervision")
            {
                //Impuestos_Derechos_Supervision.P_No_Impuesto_Derecho_Supervision = Hdf_No_Impuesto_Derecho_Supervision.Value;
                //Impuestos_Derechos_Supervision.Consultar_Impuestos_Derecho_Supervisions();
                //Dt_Temp = Impuestos_Derechos_Supervision.P_Dt_Detalles_Impuestos_Derechos_Supervision;

                foreach (DataRow Dr_Temp in Dt_Temp_Detalles.Rows)
                {
                    int Numero_Impuesto;
                    int.TryParse(Dr_Temp[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision].ToString(), out Numero_Impuesto);
                    //Inserta los datos de los Impuestos de Derechos de Supervision en la Tabla
                    Dr_Impuestos_Derechos_Supervision = Dt_Impuestos_Derechos_Supervision.NewRow();
                    Dr_Impuestos_Derechos_Supervision["NO_IMPUESTO_DERECHO_SUPERVISIO"] = Numero_Impuesto;
                    Dr_Impuestos_Derechos_Supervision["VALOR_ESTIMADO_OBRA"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Valor_Estimado_Obra];
                    Dr_Impuestos_Derechos_Supervision["DESCRIPCION_TASA"] = Dr_Temp["DESCRIPCION_TASA"];
                    Dr_Impuestos_Derechos_Supervision["DERECHO_SUPERVISION_TASA_ID"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Derecho_Supervision_Tasa_ID];
                    Dr_Impuestos_Derechos_Supervision["IMPORTE"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Importe];
                    Dr_Impuestos_Derechos_Supervision["RECARGOS"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Recargos];
                    Dr_Impuestos_Derechos_Supervision["TOTAL"] = Dr_Temp[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Total];
                    if (Dr_Temp["MULTAS"].ToString().Equals(""))
                    {
                        Dr_Impuestos_Derechos_Supervision["MULTAS"] = 0.00;
                    }
                    else
                    {
                        Dr_Impuestos_Derechos_Supervision["MULTAS"] = Dr_Temp["MULTAS"];
                    }
                    Dt_Impuestos_Derechos_Supervision.Rows.Add(Dr_Impuestos_Derechos_Supervision);
                    Derechos_Supervision_Tasas_ID += "'" + Dr_Temp[Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Derecho_Supervision_Tasa_ID] + "', ";
                }
                if (Derechos_Supervision_Tasas_ID.EndsWith("', "))
                {
                    Derechos_Supervision_Tasas_ID = Derechos_Supervision_Tasas_ID.Substring(0, Derechos_Supervision_Tasas_ID.Length - 2);
                }
            }
            if (Dt_Impuestos_Derechos_Supervision.TableName == "Dt_Cuentas_Predial")
            {
                Cuentas_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                Dt_Temp = Cuentas_Predial.Consultar_Cuenta();
                Dt_Cuenta_Predial = Dt_Temp;

                foreach (DataRow Dr_Temp in Dt_Temp.Rows)
                {
                    //Inserta los datos de los Impuestos de Derechos de Supervision en la Tabla
                    Dr_Impuestos_Derechos_Supervision = Dt_Impuestos_Derechos_Supervision.NewRow();
                    Dr_Impuestos_Derechos_Supervision["CUENTA_PREDIAL_ID"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID];
                    Dr_Impuestos_Derechos_Supervision["CUENTA_PREDIAL"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial];
                    Dr_Impuestos_Derechos_Supervision["TIPO_PREDIO_ID"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID];
                    Dr_Impuestos_Derechos_Supervision["SUPERFICIE_CONSTRUIDA"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida];
                    Dt_Impuestos_Derechos_Supervision.Rows.Add(Dr_Impuestos_Derechos_Supervision);
                }
            }
            if (Dt_Impuestos_Derechos_Supervision.TableName == "Dt_Tipos_Predio")
            {
                Tipos_Predio.P_Filtros_Dinamicos = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = '" + Dt_Cuenta_Predial.Rows[0][Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID] + "'";
                Dt_Temp = Tipos_Predio.Consultar_Tipo_Predio();

                foreach (DataRow Dr_Temp in Dt_Temp.Rows)
                {
                    //Inserta los datos de los Impuestos de Derechos de Supervision en la Tabla
                    Dr_Impuestos_Derechos_Supervision = Dt_Impuestos_Derechos_Supervision.NewRow();
                    Dr_Impuestos_Derechos_Supervision["TIPO_PREDIO_ID"] = Dr_Temp[Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID];
                    Dr_Impuestos_Derechos_Supervision["DESCRIPCION"] = Dr_Temp[Cat_Pre_Tipos_Predio.Campo_Descripcion];
                    Dt_Impuestos_Derechos_Supervision.Rows.Add(Dr_Impuestos_Derechos_Supervision);
                }
            }
            if (Dt_Impuestos_Derechos_Supervision.TableName == "Dt_Derechos_Supervision")
            {
                Derechos_Supervision.P_Derecho_Supervision_ID = "IN (SELECT " + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_ID + " FROM " + Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + " WHERE " + Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID + " IN (" + Derechos_Supervision_Tasas_ID + "))";
                Dt_Temp = Derechos_Supervision.Consultar_Derechos_Supervision();

                foreach (DataRow Dr_Temp in Dt_Temp.Rows)
                {
                    //Inserta los datos de los Impuestos de Derechos de Supervision en la Tabla
                    Dr_Impuestos_Derechos_Supervision = Dt_Impuestos_Derechos_Supervision.NewRow();
                    Dr_Impuestos_Derechos_Supervision["DERECHO_SUPERVISION_ID"] = Dr_Temp[Cat_Pre_Derechos_Supervision.Campo_Derecho_Supervision_ID];
                    Dr_Impuestos_Derechos_Supervision["IDENTIFICADOR"] = Dr_Temp[Cat_Pre_Derechos_Supervision.Campo_Identificador];
                    Dr_Impuestos_Derechos_Supervision["DESCRIPCION"] = Dr_Temp[Cat_Pre_Derechos_Supervision.Campo_Descripcion];
                    Dt_Impuestos_Derechos_Supervision.Rows.Add(Dr_Impuestos_Derechos_Supervision);
                }
            }
            if (Dt_Impuestos_Derechos_Supervision.TableName == "Dt_Derechos_Supervision_Tasas")
            {
                Derechos_Supervision.P_Derecho_Supervision_Tasa_ID = "IN (" + Derechos_Supervision_Tasas_ID + ")";
                Derechos_Supervision = Derechos_Supervision.Consultar_Datos_Derecho_Supervision();
                Dt_Temp = Derechos_Supervision.P_Derechos_Tasas;

                foreach (DataRow Dr_Temp in Dt_Temp.Rows)
                {
                    //Inserta los datos de los Impuestos de Derechos de Supervision en la Tabla
                    Dr_Impuestos_Derechos_Supervision = Dt_Impuestos_Derechos_Supervision.NewRow();
                    Dr_Impuestos_Derechos_Supervision["DERECHO_SUPERVISION_TASA_ID"] = Dr_Temp[Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_Tasa_ID];
                    Dr_Impuestos_Derechos_Supervision["DERECHO_SUPERVISION_ID"] = Dr_Temp[Cat_Pre_Der_Super_Tasas.Campo_Derecho_Supervision_ID];
                    Dr_Impuestos_Derechos_Supervision["ANIO"] = Dr_Temp[Cat_Pre_Der_Super_Tasas.Campo_Año];
                    Dr_Impuestos_Derechos_Supervision["TASA"] = Dr_Temp[Cat_Pre_Der_Super_Tasas.Campo_Tasa];
                    Dt_Impuestos_Derechos_Supervision.Rows.Add(Dr_Impuestos_Derechos_Supervision);
                }
            }
        }

        return Ds_Impuestos_Derechos_Supervision;
    }

    #endregion

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

        //if (Superficie_Fraccionar <= Superficie_Predio)
        //{
        DataTable Dt_Impuestos = new DataTable();
        Dt_Impuestos.Columns.Add(new DataColumn("VALOR_ESTIMADO_OBRA", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("DERECHO_SUPERVISION_TASA_ID", typeof(String)));
        Dt_Impuestos.Columns.Add(new DataColumn("DESCRIPCION_TASA", typeof(String)));
        Dt_Impuestos.Columns.Add(new DataColumn("MULTAS", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("TASA_DERECHO_SUPERVISION", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("IMPORTE", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("RECARGOS", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("TOTAL", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("DER_MULTA_ID", typeof(String)));

        DataRow Dr_Impuestos;
        //Se barre el Grid para cargar el DataTable con los valores del grid
        Grid_Detalle_Impuesto_Derechos_Supervision.Columns[8].Visible = true;
        foreach (GridViewRow Row in Grid_Detalle_Impuesto_Derechos_Supervision.Rows)
        {
            Dr_Impuestos = Dt_Impuestos.NewRow();
            Dr_Impuestos["VALOR_ESTIMADO_OBRA"] = Convert.ToDouble(Row.Cells[0].Text.Replace("$", ""));
            Dr_Impuestos["DERECHO_SUPERVISION_TASA_ID"] = Grid_Detalle_Impuesto_Derechos_Supervision.DataKeys[Row.RowIndex].Value.ToString();// Row.Cells[1].Text;
            Dr_Impuestos["DESCRIPCION_TASA"] = HttpUtility.HtmlDecode(Row.Cells[2].Text);
            Dr_Impuestos["TASA_DERECHO_SUPERVISION"] = Convert.ToDouble(Row.Cells[3].Text.Replace("%", "")) / 100;
            Dr_Impuestos["MULTAS"] = Convert.ToDouble(Row.Cells[4].Text.Replace("$", ""));
            Dr_Impuestos["IMPORTE"] = Convert.ToDouble(Row.Cells[5].Text.Replace("$", ""));
            Dr_Impuestos["RECARGOS"] = Convert.ToDouble(Row.Cells[6].Text.Replace("$", ""));
            Dr_Impuestos["TOTAL"] = Convert.ToDouble(Row.Cells[7].Text.Replace("$", ""));
            Dr_Impuestos["DER_MULTA_ID"] = HttpUtility.HtmlDecode(Row.Cells[8].Text);
            Dt_Impuestos.Rows.Add(Dr_Impuestos);
        }

        //Se inserta el nuevo registro de Impuesto
        Dr_Impuestos = Dt_Impuestos.NewRow();
        Boolean Algun_Valor = false;
        if (Txt_Valor_Estimado_Obra.Text.Trim() != "")
        {
            Dr_Impuestos["VALOR_ESTIMADO_OBRA"] = Convert.ToDouble(Txt_Valor_Estimado_Obra.Text);
            Algun_Valor = true;
        }
        if (Txt_Tasa.Text.Trim() != "")
        {
            Dr_Impuestos["DERECHO_SUPERVISION_TASA_ID"] = Hdf_Derecho_Supervision_Tasa_ID.Value;
            Dr_Impuestos["DESCRIPCION_TASA"] = Txt_Tasa_Descripcion.Text;
            Dr_Impuestos["TASA_DERECHO_SUPERVISION"] = Convert.ToDouble(Txt_Tasa.Text.Replace("$", "")) / 100;
            Algun_Valor = true;
        }
        else
        {
            Dr_Impuestos["DERECHO_SUPERVISION_TASA_ID"] = "";
            Dr_Impuestos["DESCRIPCION_TASA"] = "";
            Dr_Impuestos["TASA_DERECHO_SUPERVISION"] = 0;
        }
        if (Txt_Multa.Text.Trim() != "")
        {
            Dr_Impuestos["MULTAS"] = Convert.ToDouble(Txt_Multa.Text);
            Dr_Impuestos["DER_MULTA_ID"] = Hdf_Multa.Value;
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

            Grid_Detalle_Impuesto_Derechos_Supervision.DataSource = Dt_Impuestos;
            Grid_Detalle_Impuesto_Derechos_Supervision.PageIndex = 0;
            Grid_Detalle_Impuesto_Derechos_Supervision.DataBind();
            Grid_Detalle_Impuesto_Derechos_Supervision.Columns[8].Visible = false;

            //Se limpian los textos de los Impuestos
            Txt_Tasa_Descripcion.Text = "";
            Txt_Tasa.Text = "";
            Txt_Valor_Estimado_Obra.Text = "";
            Txt_Importe.Text = "";
            Txt_Multa.Text = "";
            Txt_Recargos.Text = "";
            Txt_Total.Text = "";
            Txt_Multa.Text = "";
            Hdf_Multa.Value = "";

            Calcular_Total_Impuestos();
        }
        //}
        //else
        //{
        //    //if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    //Mensaje_Error = Mensaje_Error + "+ Indique la Cuenta Predial.";
        //    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("+ La Superficie a Fraccionar debe ser Menor o Igual a la Superficie del Predio.");
        //    Div_Contenedor_Msj_Error.Visible = true;
        //}
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Detalles_Impuestos_Derechos_Cupervision
    ///DESCRIPCIÓN          : Lee el grid de los Detalles de los Impuestos y devuelve una instancia en un DataTable
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 03/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Detalles_Impuestos_Derechos_Cupervision()
    {
        DataTable Dt_Impuestos = new DataTable();
        Dt_Impuestos.Columns.Add(new DataColumn("VALOR_ESTIMADO_OBRA", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("DERECHO_SUPERVISION_TASA_ID", typeof(String)));
        Dt_Impuestos.Columns.Add(new DataColumn("DESCRIPCION_TASA", typeof(String)));
        Dt_Impuestos.Columns.Add(new DataColumn("MULTAS", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("TASA_DERECHO_SUPERVISION", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("IMPORTE", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("RECARGOS", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("TOTAL", typeof(Double)));
        Dt_Impuestos.Columns.Add(new DataColumn("DER_MULTA_ID", typeof(String)));

        DataRow Dr_Impuestos;
        //Se barre el Grid para cargar el DataTable con los valores del grid
        Grid_Detalle_Impuesto_Derechos_Supervision.Columns[8].Visible = true;
        foreach (GridViewRow Row in Grid_Detalle_Impuesto_Derechos_Supervision.Rows)
        {
            Dr_Impuestos = Dt_Impuestos.NewRow();
            Dr_Impuestos["VALOR_ESTIMADO_OBRA"] = Convert.ToDouble(Row.Cells[0].Text.Replace("$", ""));
            Dr_Impuestos["DERECHO_SUPERVISION_TASA_ID"] = Grid_Detalle_Impuesto_Derechos_Supervision.DataKeys[Row.RowIndex].Value.ToString();// Row.Cells[1].Text;
            Dr_Impuestos["DESCRIPCION_TASA"] = Row.Cells[2].Text;
            Dr_Impuestos["TASA_DERECHO_SUPERVISION"] = Convert.ToDouble(Row.Cells[3].Text.Replace("%", ""));
            Dr_Impuestos["MULTAS"] = Convert.ToDouble(Row.Cells[4].Text.Replace("$", ""));
            Dr_Impuestos["IMPORTE"] = Convert.ToDouble(Row.Cells[5].Text.Replace("$", ""));
            Dr_Impuestos["RECARGOS"] = Convert.ToDouble(Row.Cells[6].Text.Replace("$", ""));
            Dr_Impuestos["TOTAL"] = Convert.ToDouble(Row.Cells[7].Text.Replace("$", ""));
            Dr_Impuestos["DER_MULTA_ID"] = Row.Cells[8].Text.Replace("&amp;nbsp;", "0").Replace("&nbsp;", "0").Replace("&amp;amp;nbsp;", "0").Replace("&#160;", "0");
            Dt_Impuestos.Rows.Add(Dr_Impuestos);
        }
        Grid_Detalle_Impuesto_Derechos_Supervision.Columns[8].Visible = false;
        return Dt_Impuestos;
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Btn_Cerrar_Busqueda_Tasas_Click
    /// 	DESCRIPCIÓN         : Ocultar el modal popup Busqueda de 
    /// 	PARÁMETROS:
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 03/Agosto/2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Cerrar_Busqueda_Tasas_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Tasas.Hide();
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN      : Btn_Limpiar_Busqueda_Tasas_Click
    /// 	DESCRIPCIÓN         : Limpia los controles de la búsqeuda avanzada
    /// 	PARÁMETROS:
    /// 	CREO                : Antonio Salvador Benavides Guardado
    /// 	FECHA_CREO          : 03/Agosto/2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Limpiar_Busqueda_Tasas_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Busqueda_Identificador.Text = "";
        Txt_Busqueda_Descripcion.Text = "";
        Cmb_Busqueda_Año.SelectedIndex = 0;
        Mpe_Busqueda_Tasas.Show();
    }

    private void Llenar_Tabla_Tasas(int Pagina)
    {
        try
        {
            DataTable Dt_Tasas;
            Cls_Cat_Pre_Derechos_Supervision_Negocio Tasas = new Cls_Cat_Pre_Derechos_Supervision_Negocio();
            if (Txt_Busqueda_Identificador.Text.Trim() != ""
            || Txt_Busqueda_Descripcion.Text.Trim() != ""
            || Cmb_Busqueda_Año.SelectedIndex > 0)
            {
                Tasas.P_Filtros_Dinamicos = "";
                if (Txt_Busqueda_Identificador.Text.Trim() != "")
                {
                    if (Tasas.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Tasas.P_Filtros_Dinamicos += " AND ";
                    }
                    Tasas.P_Filtros_Dinamicos += Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Identificador + " LIKE '%" + Txt_Busqueda_Identificador.Text.Trim().ToUpper() + "%'";
                }
                if (Txt_Busqueda_Descripcion.Text.Trim() != "")
                {
                    if (Tasas.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Tasas.P_Filtros_Dinamicos += " AND ";
                    }
                    Tasas.P_Filtros_Dinamicos += Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Descripcion + " LIKE '%" + Txt_Busqueda_Descripcion.Text.Trim().ToUpper() + "%'";
                }
                if (Cmb_Busqueda_Año.SelectedIndex > 0)
                {
                    if (Tasas.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Tasas.P_Filtros_Dinamicos += " AND ";
                    }
                    Tasas.P_Filtros_Dinamicos += Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + "." + Cat_Pre_Der_Super_Tasas.Campo_Año + " = " + Cmb_Busqueda_Año.SelectedValue;
                }
                Tasas.P_Ordenar_Dinamico = Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + "." + Cat_Pre_Der_Super_Tasas.Campo_Año + ", " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Identificador;
                Dt_Tasas = Tasas.Consultar_Derechos_Supervision_Tasas();
                Grid_Tasas.DataSource = Dt_Tasas;
                Grid_Tasas.PageIndex = Pagina;
                Grid_Tasas.DataBind();
            }
            else
            {
                Grid_Tasas.DataSource = null;
                Grid_Tasas.PageIndex = 0;
                Grid_Tasas.DataBind();
            }
            Mpe_Busqueda_Tasas.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Busqueda_Tasas_Click
    /// DESCRIPCION             : Ejecuta la búsqueda de mediante el modal popup 
    /// CREO                    : Antonio Salvador Benavides Guardado
    /// FECHA_CREO              : 03/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Tasas_Click(object sender, EventArgs e)
    {
        Llenar_Tabla_Tasas(0);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Años
    ///DESCRIPCIÓN          : Carga el combo indicado por el parámetro de los años
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 03/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combo_Años(DropDownList Combo, Orden_Datos Orden, Int32 Año_Seleccionado)
    {
        Int32 Cont_Años;

        if (Orden == Orden_Datos.Ascendente)
        {
            for (Cont_Años = 1980; Cont_Años <= DateTime.Now.Year; Cont_Años++)
            {
                Combo.Items.Add(Cont_Años.ToString());
            }
        }
        else
        {
            if (Orden == Orden_Datos.Descendente)
            {
                for (Cont_Años = DateTime.Now.Year; Cont_Años >= 1980; Cont_Años--)
                {
                    Combo.Items.Add(Cont_Años.ToString());
                }
            }
        }
        Combo.SelectedValue = Año_Seleccionado.ToString();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Tasas_PageIndexChanging
    ///DESCRIPCIÓN          : Lee el grid de los Detalles de los Impuestos y devuelve una instancia en un DataTable
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 03/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Tasas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Tasas.SelectedIndex = (-1);
            Llenar_Tabla_Tasas(e.NewPageIndex);
            Mpe_Busqueda_Tasas.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Tasas_SelectedIndexChanged
    ///DESCRIPCIÓN          : Lee el grid de los Detalles de los Impuestos y devuelve una instancia en un DataTable
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 03/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Tasas_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Txt_Fecha_Oficio.Text != "")
        {
            try
            {
                //Txt_Busqueda_Identificador.Text = Grid_Tasas.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
                //Txt_Busqueda_Descripcion.Text = Grid_Tasas.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
                //if (Grid_Tasas.SelectedRow.Cells[5].Text.Trim().Replace("&nbsp;", "") != "")
                //{
                //    Cmb_Busqueda_Año.SelectedValue = Grid_Tasas.SelectedRow.Cells[5].Text;
                //}
                //else
                //{
                //    Cmb_Busqueda_Año.SelectedIndex = 0;
                //}
                Hdf_Derecho_Supervision_Tasa_ID.Value = Grid_Tasas.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
                Txt_Tasa_Descripcion.Text = HttpUtility.HtmlDecode(Grid_Tasas.SelectedRow.Cells[3].Text);
                if (Grid_Tasas.SelectedRow.Cells[6].Text.Replace("&nbsp;", "") != "")
                {
                    Txt_Tasa.Text = Convert.ToDouble(Grid_Tasas.SelectedRow.Cells[6].Text.Replace("&nbsp;", "")).ToString("0.00");
                }
                Btn_Limpiar_Busqueda_Tasas_Click(sender, null);
                Cmb_Busqueda_Año.SelectedValue = DateTime.Now.Year.ToString();
                Calcular_Importe();
                Calcular_Recargos();
                Calcular_Total();
                Mpe_Busqueda_Tasas.Hide();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OcultarModalTasas", "top.$get(\""
                    + Pnl_Busqueda_Contenedor_Tasas.ClientID + "\").style.display = 'none';", true);
                Txt_Multa.Text = "0.00";
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Impuestos de Derechos de Supervisión", "alert('Seleccione una fecha de oficio por favor');", true);
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
        //Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio Impuestos_Derechos_Supervision = new Cls_Ope_Pre_Impuestos_Derechos_Supervision_Negocio();
        //Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        //Cls_Cat_Pre_Tipos_Predio_Negocio Tipos_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();

        //Ds_Pre_Impuestos_Fraccionamientos Ds_Impuestos_Fraccionamientos = new Ds_Pre_Impuestos_Fraccionamientos();
        //DataTable Impuesto_Fraccionamientos;
        //DataTable Cuenta_Predial;
        //DataTable Tipo_Predio;

        //Impuestos_Derechos_Supervision.P_No_Impuesto_Derecho_Supervision = Hdf_No_Impuesto_Derecho_Supervision.Value;
        //Impuesto_Fraccionamientos = Impuestos_Derechos_Supervision.Consultar_Impuestos_Derecho_Supervisions();

        //Cuentas_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        //Cuenta_Predial = Cuentas_Predial.Consultar_Cuenta();

        //Tipos_Predio.P_Filtros_Dinamicos = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = '" + Cuenta_Predial.Rows[0][Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID] + "'";
        //Tipo_Predio = Tipos_Predio.Consultar_Tipo_Predio();

        if (Hdf_No_Impuesto_Derecho_Supervision.Value != "")
        {
            //if (Grid_Impuestos_Derechos_Supervision.SelectedRow.Cells[6].Text.Equals("PAGADO"))
            //{
            Imprimir_Reporte(Crear_Ds_Impuestos_Derechos_Supervision(), "Rpt_Pre_Impuestos_Derechos_Supervision.rpt", "Impuestos Derechos Supervision");
            //}
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Derechos de Supervisión", "alert('La fecha de oficio no puede ser mayor a la fecha actual. Seleccione otra fecha por favor.');", true);
                    return;
                }
                Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                Dtp_Fecha_Oficio.SelectedDate = Fecha_Oficio;
                //Fecha_Oficio = Convert.ToDateTime(Dtp_Fecha_Oficio.SelectedDate);
            }
            try
            {
                //if (Hdf_Fecha_Ya_Asignada.Value="")
                //{
                Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
                Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                dias_inhabiles = Parametros.Consultar_Dias_Vencimiento();
                Fecha_Oficio = Fecha_Oficio.AddDays(dias_inhabiles);
                Txt_Fecha_Vencimiento.Text = Dias_Inhabiles.Calcular_Fecha("" + Fecha_Oficio.AddDays(-1), "1").ToString("dd/MMM/yyyy");
                Dtp_Fecha_Vencimiento.SelectedDate = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text);
                //Hdf_Fecha_Ya_Asignada.Value = "falso";
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

    protected void Txt_Fecha_Vencimiento_TextChanged(object sender, EventArgs e)
    {
        Txt_Fecha_Vencimiento.Text = "" + Dtp_Fecha_Vencimiento.SelectedDate;
    }

    protected void Btn_Busqueda_Avanzada_Tasas()
    {
        try
        {
            DataTable Dt_Tasas;
            Cls_Cat_Pre_Derechos_Supervision_Negocio Tasas = new Cls_Cat_Pre_Derechos_Supervision_Negocio();
            if (Txt_Busqueda_Identificador.Text.Trim() != ""
            || Txt_Busqueda_Descripcion.Text.Trim() != ""
            || Cmb_Busqueda_Año.SelectedIndex > 0)
            {
                Tasas.P_Filtros_Dinamicos = "";
                if (Txt_Busqueda_Identificador.Text.Trim() != "")
                {
                    if (Tasas.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Tasas.P_Filtros_Dinamicos += " AND ";
                    }
                    Tasas.P_Filtros_Dinamicos += Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Identificador + " LIKE '%" + Txt_Busqueda_Identificador.Text.Trim().ToUpper() + "%'";
                }
                if (Txt_Busqueda_Descripcion.Text.Trim() != "")
                {
                    if (Tasas.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Tasas.P_Filtros_Dinamicos += " AND ";
                    }
                    Tasas.P_Filtros_Dinamicos += Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Descripcion + " LIKE '%" + Txt_Busqueda_Descripcion.Text.Trim().ToUpper() + "%'";
                }
                if (Cmb_Busqueda_Año.SelectedIndex > 0)
                {
                    if (Tasas.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Tasas.P_Filtros_Dinamicos += " AND ";
                    }
                    Tasas.P_Filtros_Dinamicos += Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + "." + Cat_Pre_Der_Super_Tasas.Campo_Año + " = " + Cmb_Busqueda_Año.SelectedValue;
                }
                Tasas.P_Ordenar_Dinamico = Cat_Pre_Der_Super_Tasas.Tabla_Cat_Pre_Der_Super_Tasas + "." + Cat_Pre_Der_Super_Tasas.Campo_Año + ", " + Cat_Pre_Derechos_Supervision.Tabla_Cat_Pre_Derechos_Supervision + "." + Cat_Pre_Derechos_Supervision.Campo_Identificador;
                Dt_Tasas = Tasas.Consultar_Derechos_Supervision_Tasas();
                Grid_Tasas.DataSource = Dt_Tasas;
                Grid_Tasas.PageIndex = 0;
                Grid_Tasas.DataBind();
            }
            else
            {
                Grid_Tasas.DataSource = null;
                Grid_Tasas.PageIndex = 0;
                Grid_Tasas.DataBind();
            }
            //Mpe_Busqueda_Tasas.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Busqueda_Multas_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Multas_Click(object sender, EventArgs e)
    {
        try
        {
            Cargar_Multas(0);
            Mpe_Busqueda_Multas.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    private void Cargar_Multas(int Pagina)
    {
        Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio Multas = new Cls_Cat_Pre_Multas_Derechos_Supervision_Negocio();
        Multas.P_Incluir_Campos_Foraneos = true;
        Multas.P_Identificador = Txt_Busqueda_Identificador_Multa.Text;
        if (Txt_Anio.Text.Length != 0)
        {
            Multas.P_Filtros_Dinamicos = " " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Año + " LIKE '%" + Txt_Anio.Text + "%'";
        }
        DataTable Dt_Aux;
        Grid_Multas.Columns[1].Visible = true;
        Dt_Aux = Multas.Consultar_Cuotas_Multas();
        Grid_Multas.DataSource = Dt_Aux;

        if (!(Grid_Multas.Rows.Count == 0))
        {
            foreach (DataRow Renglon_Actual in Dt_Aux.Rows)
            {
                if (Renglon_Actual["IDENTIFICADOR"].ToString() == "")
                {
                    Renglon_Actual.Delete();
                }
            }
        }
        Grid_Multas.PageIndex = Pagina;
        Grid_Multas.DataBind();
        Grid_Multas.Columns[1].Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Multas_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de las multas.
    ///PARAMETROS:
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 11/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Multas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Multas.SelectedIndex = (-1);
            Cargar_Multas(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Multas_SelectedIndexChanged
    ///DESCRIPCIÓN          : Maneja la selección de las filas del GridView
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 11/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Multas_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Txt_Fecha_Oficio.Text != "")
        {
            if (Grid_Multas.Rows.Count > 0)
            {
                Hdf_Multa.Value = Grid_Multas.SelectedRow.Cells[1].Text;
                Txt_Multa.Text = Grid_Multas.SelectedRow.Cells[4].Text;
                Cargar_Multas(0);
                Mpe_Busqueda_Multas.Hide();
            }

            try
            {
                Txt_Total.Text = "" + (Convert.ToDouble(Txt_Total.Text) + Convert.ToDouble(Txt_Multa.Text));
                Txt_Total.AutoPostBack = true;
            }
            catch
            {
                Txt_Multa.Text = "0.00";
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Derechos de Supervisión", "alert('Seleccione una fecha de oficio por favor');", true);
        }
    }

    protected void Btn_Cerrar_Ventana_Click(object sender, EventArgs e)
    {
        Mpe_Busqueda_Multas.Hide();
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
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() != string.Empty)
            //{
            //    //Txt_Estatus_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            //    //M_Orden_Negocio.P_Estatus_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            //}

            ////Txt_Supe_Construida_General.Text = dataTable.Rows[0]["Superficie_Construida"].ToString();
            //M_Orden_Negocio.P_Superficie_Construida = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString();
            ////Txt_Super_Total_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            //M_Orden_Negocio.P_Superficie_Total = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            //if (dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString() != "")
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
            //    DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
            //    //Txt_Calle.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            //    M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            //}
            ////Txt_Numero_Exterior_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            //M_Orden_Negocio.P_Exterior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            ////Txt_Numero_Interior_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            //M_Orden_Negocio.P_Interior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            ////Txt_Clave_Catastral_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            //M_Orden_Negocio.P_Clave_Catastral = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != string.Empty)
            //{
            //    //Txt_Efectos_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
            //}
            ////Txt_Valor_Fiscal_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()));
            //M_Orden_Negocio.P_Valor_Fiscal = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString();
            ////Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            ////Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()))
            //{
            //    //Txt_Cuota_Anual_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()));
            //    //double Cuota_Bimestral = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()) / 6;
            //    //Txt_Cuota_Bimestral_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Cuota_Bimestral);
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual] != null)
            //{
            //    M_Orden_Negocio.P_Cuota_Anual = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString());
            //    M_Orden_Negocio.P_Cuota_Bimestral = ((Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString())) / 6);
            //}
            ////Txt_Porciento_Exencion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString();
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion] != null)
            //{
            //    M_Orden_Negocio.P_Exencion = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString());
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo] != null)
            //{
            //    if (dataTable.Rows[0]["Fecha_Avaluo"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
            //    {
            //        //Txt_Fecha_Avaluo_Impuestos.Text = "";
            //        M_Orden_Negocio.P_Fecha_Avaluo = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString());
            //    }
            //    else
            //    {
            //        //Txt_Fecha_Avaluo_Impuestos.Text = String.Format("{0:dd/MMM/yyyy}", dataTable.Rows[0]["Fecha_Avaluo"].ToString().Trim());
            //        M_Orden_Negocio.P_Fecha_Avaluo = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString());
            //    }
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion] != null)
            //{
            //    if (dataTable.Rows[0]["Termino_Exencion"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
            //    {
            //        //Txt_Fecha_Termino_Extencion.Text = "";
            //        M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
            //    }
            //    else
            //    {
            //        //Txt_Fecha_Termino_Extencion.Text = String.Format("{0:dd/MMM/yyyy}", dataTable.Rows[0]["Termino_Exencion"].ToString().Trim());
            //        M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
            //    }

            //}
            ////Txt_Dif_Construccion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();
            //M_Orden_Negocio.P_Diferencia_Construccion = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();

            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] != null)
            //{
            //    M_Orden_Negocio.P_Cuota_Minima = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString());
            //    //Cmb_Cuota_Minima.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString();
            //}
            ////Z1 HAY KU07A FIJ4!!! Seccion de carga de datos de la cuota fija
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] != null)
            //{
            //    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "NO" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "no" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "No")
            //    {
            //        //Chk_Cuota_Fija.Checked = false;
            //        M_Orden_Negocio.P_Cuota_Fija = "NO";
            //    }
            //    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "SI" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "si" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "Si")
            //    {
            //        //Chk_Cuota_Fija.Checked = true;
            //        M_Orden_Negocio.P_Cuota_Fija = "SI";

            //        //----K4RG4R D47OZ D3 14 CU0T4 F1J4!!!!!
            //        if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString() != "")
            //        {
            //            M_Orden_Negocio.P_No_Cuota_Fija = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString();
            //            //Cargar_Datos_Cuota_Fija(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString());
            //        }
            //    }
            //}
            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString()))
            //{
            //    M_Orden_Negocio.P_Tasa = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString());
            //    //Hdn_Tasa_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString();
            //}

            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString()))
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Tasa_Predial_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString();
            //    DataTable Dt_Tasa = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tasa();
            //    if (Dt_Tasa.Rows.Count > 0)
            //    {
            //        //Txt_Tasa_Impuestos.Text = Dt_Tasa.Rows[0]["Descripcion"].ToString();
            //    }
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() != String.Empty)
            //{
            //    //Cmb_Domicilio_Foraneo.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
            //    M_Orden_Negocio.P_Domicilio_Foraneo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
            //}
            //if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_ID_Notificacion"].ToString()))
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = (dataTable.Rows[0]["Estado_ID_Notificacion"].ToString());
            //    DataTable Dt_Estado_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio_Propietario();
            //    if (Dt_Estado_Propietario.Rows.Count > 0)
            //    {
            //        //Txt_Estado_Propietario.Text = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
            //        M_Orden_Negocio.P_Estado_Propietario = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
            //    }
            //}
            //else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_Notificacion"].ToString()))
            //{
            //    //Txt_Estado_Propietario.Text = dataTable.Rows[0]["Estado_Notificacion"].ToString();
            //}
            //if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString()))
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Ciudad_ID = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
            //    DataTable Dt_Ciudad_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ciudad();
            //    //Txt_Ciudad_Propietario.Text = Dt_Ciudad_Propietario.Rows[0]["Nombre"].ToString();
            //    M_Orden_Negocio.P_Ciudad_Propietario = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
            //}
            //else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_Notificacion"].ToString()))
            //{
            //    //Txt_Ciudad_Propietario.Text = dataTable.Rows[0]["Ciudad_Notificacion"].ToString();
            //}
            //if (dataTable.Rows[0]["Domicilio_Foraneo"].ToString().Trim() == "SI")
            //{
            //    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_Notificacion"].ToString()))
            //    {
            //        //Txt_Colonia_Propietario.Text = dataTable.Rows[0]["Colonia_Notificacion"].ToString();
            //    }
            //    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_Notificacion"].ToString()))
            //    {
            //        //Txt_Calle_Propietario.Text = dataTable.Rows[0]["Calle_Notificacion"].ToString();
            //    }
            //}
            //else
            //{

            //    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString()))
            //    {
            //        Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString();
            //        DataTable DT_Colonia_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
            //        //Txt_Colonia_Propietario.Text = DT_Colonia_Propietario.Rows[0]["Nombre"].ToString();
            //    }

            //    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_ID_Notificacion"].ToString()))
            //    {
            //        Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0]["Calle_ID_Notificacion"].ToString();//*
            //        DataTable Dt_Calle_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
            //        //Txt_Calle_Propietario.Text = Dt_Calle_Propietario.Rows[0]["Nombre"].ToString();
            //    }
            //}

            ////Txt_Numero_Exterior_Propietario.Text = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            //M_Orden_Negocio.P_Exterior_Propietario = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            ////Txt_Numero_Interior_Propietario.Text = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            //M_Orden_Negocio.P_Interior_Propietario = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            ////Txt_Cod_Postal_Propietario.Text = dataTable.Rows[0]["Codigo_Postal"].ToString();
            //M_Orden_Negocio.P_CP_Propietario = dataTable.Rows[0]["Codigo_Postal"].ToString();


            //M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            //M_Orden_Negocio.P_Dt_Copropietarios = M_Orden_Negocio.Consulta_Co_Propietarios();
            ////Dt_Agregar_Co_Propietarios = M_Orden_Negocio.P_Dt_Copropietarios;
            ////if (Dt_Agregar_Co_Propietarios.Rows.Count - 1 >= 0)
            ////{
            ////    for (int x = 0; x <= Dt_Agregar_Co_Propietarios.Rows.Count - 1; x++)
            ////    {
            ////        if (Dt_Agregar_Co_Propietarios.Rows[0]["Tipo"].ToString().Trim() == "COPROPIETARIO")
            ////        {
            ////            Txt_Copropietarios_Propietario.Text += Dt_Agregar_Co_Propietarios.Rows[x]["Nombre_Contribuyente"].ToString().Trim() + " \t" + Dt_Agregar_Co_Propietarios.Rows[x]["Rfc"].ToString().Trim() + "\n";

            ////        }
            ////    }
            ////}
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


}
