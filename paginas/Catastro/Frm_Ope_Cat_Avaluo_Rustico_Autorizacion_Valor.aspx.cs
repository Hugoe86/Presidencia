﻿using System;
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
using Presidencia.Sessiones;
using System.Data.OracleClient;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Numalet;
using System.IO;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Registro_Peticion.Datos;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Cat_Parametros.Negocio;
using Presidencia.Catalogo_Cat_Tabla_Factores.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Cat_Identificadores_Predio.Negocio;
using Presidencia.Catalogo_Cat_Motivos_Avaluo.Negocio;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using Presidencia.Catalogo_Cat_Peritos_Internos.Negocio;
using Presidencia.Operacion_Cat_Avaluo_Rustico_Autorizacion_Valor.Negocio;
using Presidencia.Operacion_Cat_Avaluo_Rustico_Autorizacion_Valor.Datos;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;

/*Cls_Ope_Predial_res_predio*/


using Presidencia.Operacion_Cat_Avaluo_Rustico_Autorizacion_Valor.Negocio;
using Presidencia.Operacion_Cat_Asignacion_Cuentas.Negocio;
using Presidencia.Catalogo_Cat_Tasas.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Catalogo_Cat_Tipos_Construccion.Negocio;

public partial class paginas_Catastro_Frm_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Tsm_Generar_Nomina.RegisterPostBackControl(Btn_Agregar_Documento);
            Btn_Agregar_Documento.Attributes["onclick"] = "$get('" + Uprg_Reporte.ClientID + "').style.display = 'block'; return true;";
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);
                Llenar_Cuentas_Asignadas(0);
                Llenar_Tabla_Avaluos_Urbanos(0);
                Llenar_Combo_Motivos_Avaluo();
                Llenar_Combo_Firmante();
                Llenar_Combo_Tipos_Construccion();
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                String Ventana_Modal1 = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:700px;dialogHeight:420px;dialogHide:true;help:no;scroll:no');";
                Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal1);
                Limpiar_Formulario();
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Enabled)
    {
        Txt_Observaciones_Rechazo.Enabled = false;
        //Txt_No_Avaluo.Enabled = false;
        Txt_Valor_Total_Predio.Enabled = false;
        Txt_Terreno_Superficie_Total.Enabled = false;
        Txt_Terreno_Valor_Total.Enabled = false;
        Txt_Construccion_Superficie_Total.Enabled = false;
        Txt_Construccion_Valor_Total.Enabled = false;
        Cmb_Estatus.Enabled = false;
        Txt_Uso_Constru.Enabled = !Enabled;
        Cmb_Motivo_Avaluo.Enabled = !Enabled;
        Txt_Medida_Colindancia.Enabled = !Enabled;
        Btn_Agregar_Med_Col.Enabled = !Enabled;
        Cmb_Revision.Enabled = !Enabled;
        //Txt_Cuenta_Predial.Enabled = !Enabled;
        //Txt_Propietario.Enabled = !Enabled;
        Txt_Solicitante.Enabled = !Enabled;
        //Txt_Solicitante.Enabled = Enabled;
        //Txt_Domicilio_Not.Enabled = !Enabled;
        //Txt_Municipio_Notificar.Enabled = !Enabled;
        //Txt_Ubicacion_Predio.Enabled = !Enabled;
        //Txt_Localidad.Enabled = !Enabled;
        //Txt_Nombre_Predio.Enabled = !Enabled;
        Cmb_Coordenadas.Enabled = !Enabled;
        Txt_Observaciones.Enabled = !Enabled;
        Txt_X_Horas.Enabled = !Enabled;
        Txt_X_Minutos.Enabled = !Enabled;
        Txt_X_Segundos.Enabled = !Enabled;
        Cmb_Latitud.Enabled = !Enabled;
        Txt_Y_Horas.Enabled = !Enabled;
        Txt_Y_Minutos.Enabled = !Enabled;
        Txt_Y_Segundos.Enabled = !Enabled;
        Txt_Coordenadas_UTM.Enabled = !Enabled;
        Txt_Coordenadas_UTM_Y.Enabled = !Enabled;
        Cmb_Longitud.Enabled = !Enabled;
        //Txt_Sur.Enabled = !Enabled;
        //Txt_Norte.Enabled = !Enabled;
        //Txt_Poniente.Enabled = !Enabled;
        //Txt_Oriente.Enabled = !Enabled;
        Txt_Busqueda.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
        Txt_Busqueda_Asignadas.Enabled = Enabled;
        Btn_Busqueda_Asignadas.Enabled = Enabled;
        Grid_Avaluos_Urbanos.Enabled = Enabled;
        Grid_Elementos_Construccion.Enabled = !Enabled;
        Grid_Calculos.Enabled = !Enabled;
        Grid_Colindancias.Enabled = !Enabled;
        Grid_Descripcion_Terreno.Enabled = !Enabled;
        Grid_Observaciones.Enabled = !Enabled;
        Grid_Valores_Construccion.Enabled = !Enabled;
        Txt_Terreno_Superficie_Total.Style["text-align"] = "Right";
        Txt_Terreno_Valor_Total.Style["text-align"] = "Right";
        Txt_Construccion_Superficie_Total.Style["text-align"] = "Right";
        Txt_Construccion_Valor_Total.Style["text-align"] = "Right";
        Txt_Valor_Total_Predio.Style["text-align"] = "Right";
        Txt_Diferencia_Anual.Style["text-align"] = "Right";
        Txt_Valor_Total_Predio_Anterior.Style["text-align"] = "Right";
        Txt_Cuota_Anual_Actual.Style["text-align"] = "Right";
        Txt_Cuota_Anual_Anterior.Style["text-align"] = "Right";
        Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Visible = false;
        Cmb_Tipo_Construccion.Enabled = !Enabled;
        if (Enabled == false && Cmb_Tipo_Construccion.SelectedValue == "OTRO")
        {
            Txt_Uso_Constru.Enabled = !Enabled;
        }
        else if (Enabled == false && Cmb_Tipo_Construccion.SelectedValue != "OTRO")
        {
            Txt_Uso_Constru.Enabled = Enabled;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Formulario()
    {
        Cmb_Motivo_Avaluo.SelectedIndex = 0;
        Txt_Clave_Catastral.Text = "";
        Txt_No_Avaluo.Text = "";
        Txt_Cuenta_Predial.Text = "";
        Txt_Busqueda_Asignadas.Text = "";
        //Hdf_Cuenta_Predial_Id.Value = "";
        Txt_Propietario.Text = "";
        Txt_Solicitante.Text = "";
        Txt_Clave_Catastral.Text = "";
        Txt_Domicilio_Not.Text = "";
        Txt_Municipio_Notificar.Text = "";
        Txt_Ubicacion_Predio.Text = "";
        Txt_Localidad.Text = "";
        //Txt_Nombre_Predio.Text = "";
        Txt_Observaciones.Text = "";
        Txt_X_Horas.Text = "";
        Txt_X_Minutos.Text = "";
        Txt_X_Segundos.Text = "";
        Cmb_Latitud.SelectedIndex = 0;
        Txt_Y_Horas.Text = "";
        Txt_Y_Minutos.Text = "";
        Txt_Y_Segundos.Text = "";
        Cmb_Longitud.SelectedIndex = 0;
        Cmb_Revision.SelectedValue = "NO";
        Txt_Valor_Total_Predio.Text = "0.00";
        Txt_Valor_Total_Predio_Anterior.Text = "0.00";
        Txt_Cuota_Anual_Actual.Text = "0.00";
        Txt_Cuota_Anual_Anterior.Text = "0.00";
        //Txt_Sur.Text = "";
        //Txt_Norte.Text = "";
        //Txt_Poniente.Text = "";
        //Txt_Oriente.Text = "";
        Txt_Terreno_Superficie_Total.Text = "0.00";
        Txt_Terreno_Valor_Total.Text = "0.00";
        Txt_Construccion_Superficie_Total.Text = "0.00";
        Txt_Construccion_Valor_Total.Text = "0.00";
        ///Txt_Precio_Avaluo.Text = "0.00";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Busqueda.Text = "";
        Txt_Observaciones_Rechazo.Text = "";
        Txt_Fecha.Text = "";
        Txt_Coordenadas_UTM.Text = "";
        Txt_Coordenadas_UTM_Y.Text = "";
        Div_Cartograficas.Visible = false;
        Div_UTM.Visible = false;
        Txt_Observaciones_Rechazo.Text = "";
        Txt_Uso_Constru.Text = "";
        Txt_Medida_Colindancia.Text = "";
        Cmb_Coordenadas.SelectedValue = "SELECCIONE";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Firmante
    ///DESCRIPCIÓN: Llena la el combo con los datos de los firmantes
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Firmante()
    {

        try
        {
            DataTable Dt_Firmantes_Combo = new DataTable();
            Dt_Firmantes_Combo.Columns.Add("FIRMANTE", typeof(String));
            DataTable Dt_Firmante = new DataTable();
            DataRow Dr_Renglon_Nuevo;
            Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio Firmante = new Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio();
            Dt_Firmante = Firmante.Consulta_Firmante();
            Dr_Renglon_Nuevo = Dt_Firmantes_Combo.NewRow();
            Dr_Renglon_Nuevo["FIRMANTE"] = Dt_Firmante.Rows[0]["FIRMANTE_1"].ToString();
            Dt_Firmantes_Combo.Rows.InsertAt(Dr_Renglon_Nuevo, 0);
            Dr_Renglon_Nuevo = Dt_Firmantes_Combo.NewRow();
            Dr_Renglon_Nuevo["FIRMANTE"] = Dt_Firmante.Rows[0]["FIRMANTE_2"].ToString();
            Dt_Firmantes_Combo.Rows.InsertAt(Dr_Renglon_Nuevo, 1);
            Leyendas = Dt_Firmante.Rows[0]["LEYENDA"].ToString();

            Cmb_Firmante.DataTextField = "FIRMANTE";
            Cmb_Firmante.DataValueField = "FIRMANTE";
            Cmb_Firmante.DataSource = Dt_Firmantes_Combo;
            Cmb_Firmante.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Motivos_Avaluo
    ///DESCRIPCIÓN: Llena la el combo con los datos de los motivos de avalúo
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Motivos_Avaluo()
    {
        try
        {
            DataTable Dt_Motivos_Avaluo;
            DataRow Dr_Renglon_Nuevo;
            Cls_Cat_Cat_Motivos_Avaluo_Negocio Motivos = new Cls_Cat_Cat_Motivos_Avaluo_Negocio();
            Dt_Motivos_Avaluo = Motivos.Consultar_Motivos_Avaluo();
            Dr_Renglon_Nuevo = Dt_Motivos_Avaluo.NewRow();
            Dr_Renglon_Nuevo[Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id] = "SELECCIONE";
            Dr_Renglon_Nuevo[Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion] = "<SELECCIONE>";
            Dt_Motivos_Avaluo.Rows.InsertAt(Dr_Renglon_Nuevo, 0);
            Cmb_Motivo_Avaluo.DataSource = Dt_Motivos_Avaluo;
            Cmb_Motivo_Avaluo.DataTextField = Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion;
            Cmb_Motivo_Avaluo.DataValueField = Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id;
            Cmb_Motivo_Avaluo.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Calidad
    ///DESCRIPCIÓN: Llena la tabla de los datos de calidad
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Avaluos_Urbanos(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio Avaluo_Urb = new Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Avaluo_Urb.P_Folio = Txt_Busqueda.Text.Trim();
            }


            Cls_Cat_Cat_Peritos_Internos_Negocio Perito_Interno = new Cls_Cat_Cat_Peritos_Internos_Negocio();
            Perito_Interno.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
            DataTable Dt_Perito_Interno;
            Dt_Perito_Interno = Perito_Interno.Consultar_Peritos_Internos();



            Avaluo_Urb.P_Perito_Interno_Id = Dt_Perito_Interno.Rows[0]["PERITO_INTERNO_ID"].ToString();

            Grid_Avaluos_Urbanos.Columns[1].Visible = true;
            Grid_Avaluos_Urbanos.Columns[2].Visible = true;
            Grid_Avaluos_Urbanos.DataSource = Avaluo_Urb.Consultar_Avaluo_Rustico();
            Grid_Avaluos_Urbanos.PageIndex = Pagina;
            Grid_Avaluos_Urbanos.DataBind();
            Grid_Avaluos_Urbanos.Columns[1].Visible = false;
            Grid_Avaluos_Urbanos.Columns[2].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Motivos_Avaluo
    ///DESCRIPCIÓN: Llena la el combo con los datos de los motivos de avalúo
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Tipos_Construccion()
    {
        try
        {
            DataTable Dt_Tipos_Construccion;
            DataRow Dr_Renglon_Nuevo;
            Cls_Cat_Cat_Tipos_Construccion_Negocio Tipo_Construccion = new Cls_Cat_Cat_Tipos_Construccion_Negocio();
            Tipo_Construccion.P_Identificador = "ANTIGUO";
            Tipo_Construccion.P_Estatus = " = 'VIGENTE' ";
            Dt_Tipos_Construccion = Tipo_Construccion.Consultar_Tipos_Construccion_Uso();
            Dr_Renglon_Nuevo = Dt_Tipos_Construccion.NewRow();
            Dr_Renglon_Nuevo[Cat_Cat_Tipos_Construccion.Campo_Identificador] = "OTRO";
            Dr_Renglon_Nuevo[Cat_Cat_Tipos_Construccion.Campo_Identificador] = "OTRO";
            Dt_Tipos_Construccion.Rows.Add(Dr_Renglon_Nuevo);
            Cmb_Tipo_Construccion.DataSource = Dt_Tipos_Construccion;
            Cmb_Tipo_Construccion.DataTextField = Cat_Cat_Tipos_Construccion.Campo_Identificador;
            Cmb_Tipo_Construccion.DataValueField = Cat_Cat_Tipos_Construccion.Campo_Identificador;
            Cmb_Tipo_Construccion.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    private void Crear_Tabla_Construccion_Dominante(DataTable Dt_Construccion_Dominante)
    {
        DataTable Dt_Construccion_Dominante_Avaluo = new DataTable();
        DataRow Dr_Renglon_Nuevo;
        Int16 i = 0;
        int Contador_Renglones = 0;
        Dt_Construccion_Dominante_Avaluo.Columns.Add("DESC_CONSTRU_RUSTICO_ID", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("IDENTIFICADOR", typeof(String));


        Dt_Construccion_Dominante_Avaluo.Columns.Add("DESCRIPCION_RUSTICO_ID1", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("VALOR_INDICE1", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("INDICADOR_A", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("VALOR_INDICADOR_A", typeof(String));


        Dt_Construccion_Dominante_Avaluo.Columns.Add("DESCRIPCION_RUSTICO_ID2", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("VALOR_INDICE2", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("INDICADOR_B", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("VALOR_INDICADOR_B", typeof(String));

        Dt_Construccion_Dominante_Avaluo.Columns.Add("DESCRIPCION_RUSTICO_ID3", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("VALOR_INDICE3", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("INDICADOR_C", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("VALOR_INDICADOR_C", typeof(String));

        Dt_Construccion_Dominante_Avaluo.Columns.Add("DESCRIPCION_RUSTICO_ID4", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("VALOR_INDICE4", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("INDICADOR_D", typeof(String));
        Dt_Construccion_Dominante_Avaluo.Columns.Add("VALOR_INDICADOR_D", typeof(String));
        Dr_Renglon_Nuevo = Dt_Construccion_Dominante_Avaluo.NewRow();
        String Identificador = "";
        while (Contador_Renglones < Dt_Construccion_Dominante.Rows.Count)
        {
            if (i == 0)
            {
                if (Identificador != Dt_Construccion_Dominante.Rows[Contador_Renglones]["IDENTIFICADOR"].ToString())
                {
                    Identificador = Dt_Construccion_Dominante.Rows[Contador_Renglones]["IDENTIFICADOR"].ToString();
                    Dr_Renglon_Nuevo["IDENTIFICADOR"] = Dt_Construccion_Dominante.Rows[Contador_Renglones]["IDENTIFICADOR"].ToString();
                }
                else
                {
                    Dr_Renglon_Nuevo["IDENTIFICADOR"] = " ";
                }
                Dr_Renglon_Nuevo["DESC_CONSTRU_RUSTICO_ID"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Ope_Cat_Caracteristicas_Ara.Campo_Desc_Constru_Rustico_Id].ToString();
            }
            if (i == 0)
            {
                Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID1"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Ope_Cat_Caracteristicas_Ara.Campo_Descripcion_Rustico_Id].ToString();
                Dr_Renglon_Nuevo["VALOR_INDICE1"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice].ToString();
                Dr_Renglon_Nuevo["VALOR_INDICADOR_A"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_A].ToString();
                Dr_Renglon_Nuevo["INDICADOR_A"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A].ToString();
                if (Contador_Renglones == Dt_Construccion_Dominante.Rows.Count)
                {
                    Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID2"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICE2"] = "";
                    Dr_Renglon_Nuevo["INDICADOR_B"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICADOR_B"] = "";
                    Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID3"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICE3"] = "";
                    Dr_Renglon_Nuevo["INDICADOR_C"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICADOR_C"] = "";
                    Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID4"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICE4"] = "";
                    Dr_Renglon_Nuevo["INDICADOR_D"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICADOR_D"] = "";
                    Dt_Construccion_Dominante_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    break;
                }
                i++;
            }
            else if (i == 1)
            {
                if (Identificador == Dt_Construccion_Dominante.Rows[Contador_Renglones]["IDENTIFICADOR"].ToString())
                {
                    Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID2"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Ope_Cat_Caracteristicas_Ara.Campo_Descripcion_Rustico_Id].ToString();
                    Dr_Renglon_Nuevo["VALOR_INDICE2"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice].ToString();
                    Dr_Renglon_Nuevo["VALOR_INDICADOR_B"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_A].ToString();
                    Dr_Renglon_Nuevo["INDICADOR_B"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A].ToString();
                    if (Contador_Renglones == Dt_Construccion_Dominante.Rows.Count)
                    {
                        Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID3"] = "";
                        Dr_Renglon_Nuevo["VALOR_INDICE3"] = "";
                        Dr_Renglon_Nuevo["INDICADOR_C"] = "";
                        Dr_Renglon_Nuevo["VALOR_INDICADOR_C"] = "";
                        Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID4"] = "";
                        Dr_Renglon_Nuevo["VALOR_INDICE4"] = "";
                        Dr_Renglon_Nuevo["INDICADOR_D"] = "";
                        Dr_Renglon_Nuevo["VALOR_INDICADOR_D"] = "";
                        Dt_Construccion_Dominante_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                        break;
                    }
                    i++;
                }
                else
                {
                    Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID2"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICE2"] = "";
                    Dr_Renglon_Nuevo["INDICADOR_B"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICADOR_B"] = "";
                    Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID3"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICE3"] = "";
                    Dr_Renglon_Nuevo["INDICADOR_C"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICADOR_C"] = "";
                    Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID4"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICE4"] = "";
                    Dr_Renglon_Nuevo["INDICADOR_D"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICADOR_D"] = "";
                    Dt_Construccion_Dominante_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    Dr_Renglon_Nuevo = Dt_Construccion_Dominante_Avaluo.NewRow();
                    Contador_Renglones--;
                    i = 0;
                }
            }
            else if (i == 2)
            {
                if (Identificador == Dt_Construccion_Dominante.Rows[Contador_Renglones]["IDENTIFICADOR"].ToString())
                {
                    Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID3"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Ope_Cat_Caracteristicas_Ara.Campo_Descripcion_Rustico_Id].ToString();
                    Dr_Renglon_Nuevo["VALOR_INDICE3"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice].ToString();
                    Dr_Renglon_Nuevo["VALOR_INDICADOR_C"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_A].ToString();
                    Dr_Renglon_Nuevo["INDICADOR_C"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A].ToString();
                    if (Contador_Renglones == Dt_Construccion_Dominante.Rows.Count)
                    {
                        Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID4"] = "";
                        Dr_Renglon_Nuevo["VALOR_INDICE4"] = "";
                        Dr_Renglon_Nuevo["INDICADOR_D"] = "";
                        Dr_Renglon_Nuevo["VALOR_INDICADOR_D"] = "";
                        Dt_Construccion_Dominante_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                        break;
                    }
                    i++;
                }
                else
                {
                    Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID3"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICE3"] = "";
                    Dr_Renglon_Nuevo["INDICADOR_C"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICADOR_C"] = "";
                    Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID4"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICE4"] = "";
                    Dr_Renglon_Nuevo["INDICADOR_D"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICADOR_D"] = "";
                    Dt_Construccion_Dominante_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    Dr_Renglon_Nuevo = Dt_Construccion_Dominante_Avaluo.NewRow();
                    Contador_Renglones--;
                    i = 0;
                }
            }
            else if (i == 3)
            {
                if (Identificador == Dt_Construccion_Dominante.Rows[Contador_Renglones]["IDENTIFICADOR"].ToString())
                {
                    Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID4"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Ope_Cat_Caracteristicas_Ara.Campo_Descripcion_Rustico_Id].ToString();
                    Dr_Renglon_Nuevo["VALOR_INDICE4"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice].ToString();
                    Dr_Renglon_Nuevo["VALOR_INDICADOR_D"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_A].ToString();
                    Dr_Renglon_Nuevo["INDICADOR_D"] = Dt_Construccion_Dominante.Rows[Contador_Renglones][Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A].ToString();
                    Dt_Construccion_Dominante_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    Dr_Renglon_Nuevo = Dt_Construccion_Dominante_Avaluo.NewRow();
                    i = 0;
                }
                else
                {
                    Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID4"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICE4"] = "";
                    Dr_Renglon_Nuevo["INDICADOR_D"] = "";
                    Dr_Renglon_Nuevo["VALOR_INDICADOR_D"] = "";
                    Dt_Construccion_Dominante_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                    Dr_Renglon_Nuevo = Dt_Construccion_Dominante_Avaluo.NewRow();
                    Contador_Renglones--;
                    i = 0;
                }
            }
            Contador_Renglones++;
        }
        try
        {
            if (i < 4)
            {
                switch (i)
                {
                    case 1:
                        Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID3"] = "";
                        Dr_Renglon_Nuevo["VALOR_INDICE3"] = "";
                        Dr_Renglon_Nuevo["INDICADOR_C"] = "";
                        Dr_Renglon_Nuevo["VALOR_INDICADOR_C"] = "";
                        Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID4"] = "";
                        Dr_Renglon_Nuevo["VALOR_INDICE4"] = "";
                        Dr_Renglon_Nuevo["INDICADOR_D"] = "";
                        Dr_Renglon_Nuevo["VALOR_INDICADOR_D"] = "";
                        break;
                    case 2:
                        Dr_Renglon_Nuevo["DESCRIPCION_RUSTICO_ID4"] = "";
                        Dr_Renglon_Nuevo["VALOR_INDICE4"] = "";
                        Dr_Renglon_Nuevo["INDICADOR_D"] = "";
                        Dr_Renglon_Nuevo["VALOR_INDICADOR_D"] = "";
                        break;
                    case 3:
                        Dt_Construccion_Dominante_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
                        break;

                }

                Dt_Construccion_Dominante_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
            }
        }
        catch
        { }
        Session["Dt_Caracteristicas"] = Dt_Construccion_Dominante_Avaluo.Copy();
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Evento del botón nuevo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Cuentas_Asignadas.SelectedIndex > -1)
            {
                if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                {
                    String Cuenta_Predial = Txt_Cuenta_Predial.Text;
                    String Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                    Cls_Cat_Cat_Parametros_Negocio Par = new Cls_Cat_Cat_Parametros_Negocio();
                    Int16 columnas = Convert.ToInt16(Par.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Columnas_Calc_Construccion].ToString());
                    Configuracion_Formulario(false);
                    Limpiar_Formulario();
                    Txt_Cuenta_Predial.Text = Cuenta_Predial;
                    Hdf_Cuenta_Predial_Id.Value = Cuenta_Predial_Id;
                    Cargar_Datos();
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Btn_Imprimir.Visible = false;
                    Div_Datos_Avaluo.Visible = true;
                    Div_Grid_Avaluo.Visible = false;
                    Crear_Dt_Valores_Construccion();
                    Crear_Dt_Calculos();
                    Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio();
                    Avaluo.P_Anio_Avaluo = DateTime.Now.Year.ToString();
                    DataTable Dt_Elementos_Construccion = Avaluo.Consultar_Tabla_Elementos_Construccion();
                    Session["Dt_Grid_Elementos_Construccion"] = Dt_Elementos_Construccion.Copy();
                    Grid_Elementos_Construccion.Columns[0].Visible = true;

                    for (Int16 i = 1; i < (columnas + 1); i++)
                    {
                        Grid_Elementos_Construccion.Columns[i + 1].Visible = true;
                    }
                    Grid_Elementos_Construccion.DataSource = Dt_Elementos_Construccion;
                    Grid_Elementos_Construccion.DataBind();
                    Grid_Elementos_Construccion.Columns[0].Visible = false;
                    for (int i = (columnas + 1); i < 16; i++)
                    {
                        Grid_Elementos_Construccion.Columns[i + 1].Visible = false;
                    }
                    Session["Dt_Tabla_Valores_Construccion"] = Avaluo.Consultar_Tabla_Valores_Construccion();

                    DataTable Dt_Caracteristicas = Avaluo.Consultar_Tabla_Caracteristicas_Terreno();
                    Crear_Tabla_Construccion_Dominante(Dt_Caracteristicas);
                    Dt_Caracteristicas = (DataTable)Session["Dt_Caracteristicas"];

                    Grid_Descripcion_Terreno.Columns[0].Visible = true;
                    Grid_Descripcion_Terreno.Columns[2].Visible = true;
                    Grid_Descripcion_Terreno.Columns[6].Visible = true;
                    Grid_Descripcion_Terreno.Columns[10].Visible = true;
                    Grid_Descripcion_Terreno.Columns[14].Visible = true;
                    Grid_Descripcion_Terreno.DataSource = Dt_Caracteristicas;
                    Grid_Descripcion_Terreno.DataBind();
                    Grid_Descripcion_Terreno.Columns[0].Visible = false;
                    Grid_Descripcion_Terreno.Columns[2].Visible = false;
                    Grid_Descripcion_Terreno.Columns[6].Visible = false;
                    Grid_Descripcion_Terreno.Columns[10].Visible = false;
                    Grid_Descripcion_Terreno.Columns[14].Visible = false;
                    Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Factores = new Cls_Cat_Cat_Tabla_Factores_Negocio();
                    Tabla_Factores.P_Anio = DateTime.Now.Year.ToString();
                    DataTable Dt_Factores_Cobro = Tabla_Factores.Consultar_Tabla_Factores_Cobro_Avaluos();
                    Cmb_Estatus.SelectedValue = "POR VALIDAR";
                    DataTable Dt_Medidas = new DataTable();
                    Dt_Medidas.Columns.Add(Ope_Cat_Colindancias_Arv.Campo_No_Colindancia, typeof(String));
                    Dt_Medidas.Columns.Add(Ope_Cat_Colindancias_Arv.Campo_Medida_Colindancia, typeof(String));
                    Dt_Medidas.Columns.Add("ACCION", typeof(String));
                    Session["Dt_Medidas"] = Dt_Medidas;
                    Grid_Colindancias.Columns[0].Visible = true;
                    Grid_Colindancias.DataSource = Dt_Medidas;
                    Grid_Colindancias.DataBind();
                    Grid_Colindancias.Columns[0].Visible = false;
                    Txt_Solicitante.Text = "TESORERÍA MUNICIPAL";
                    DataTable Dt_Documentos = new DataTable();
                    Dt_Documentos.Columns.Add("NO_DOCUMENTO", typeof(String));
                    Dt_Documentos.Columns.Add("ANIO_DOCUMENTO", typeof(int));
                    Dt_Documentos.Columns.Add("DOCUMENTO", typeof(String));
                    Dt_Documentos.Columns.Add("RUTA_DOCUMENTO", typeof(String));
                    Dt_Documentos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
                    Dt_Documentos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
                    Dt_Documentos.Columns.Add("ACCION", typeof(String));
                    Session["Dt_Documentos"] = Dt_Documentos;
                    Grid_Documentos.Columns[0].Visible = true;
                    Grid_Documentos.Columns[1].Visible = true;
                    Grid_Documentos.DataSource = Dt_Documentos;
                    Grid_Documentos.DataBind();
                    Grid_Documentos.Columns[0].Visible = false;
                    Grid_Documentos.Columns[1].Visible = false;
                    Btn_Imprimir_Cuentas.Visible = false;
                    try
                    {
                        Cmb_Motivo_Avaluo.SelectedIndex = Cmb_Motivo_Avaluo.Items.IndexOf(Cmb_Motivo_Avaluo.Items.FindByText(HttpUtility.HtmlDecode("ACTUALIZACION DE VALOR")));
                    }
                    catch { }
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio();
                        Avaluo.P_Anio_Avaluo = DateTime.Now.Year.ToString();
                        Avaluo.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                        Avaluo.P_Motivo_Avaluo_Id = Cmb_Motivo_Avaluo.SelectedValue;
                        Avaluo.P_Propietario = Txt_Propietario.Text.ToUpper();
                        Avaluo.P_Solicitante = Txt_Solicitante.Text.ToUpper();
                        Avaluo.P_Domicilio_Notificar = Txt_Domicilio_Not.Text.ToUpper();
                        Avaluo.P_Municipio_Notificar = Txt_Municipio_Notificar.Text.ToUpper();
                        Avaluo.P_Ubicacion = Txt_Ubicacion_Predio.Text.ToUpper();
                        Avaluo.P_Localidad_Municipio = Txt_Localidad.Text.ToUpper();
                        Avaluo.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                        Avaluo.P_Clave_Catastral = Txt_Clave_Catastral.Text;
                        if (Cmb_Tipo_Construccion.SelectedValue == "OTRO")
                        {
                            Avaluo.P_Uso = Txt_Uso_Constru.Text.ToUpper();
                        }
                        else
                        {
                            Avaluo.P_Uso = Cmb_Tipo_Construccion.SelectedValue;
                        }

                        if (Cmb_Coordenadas.SelectedValue == "CART")
                        {
                            Avaluo.P_Tipo = Cmb_Coordenadas.SelectedValue;
                            Avaluo.P_Coordenadas_UTM = "";
                            Avaluo.P_Coordenadas_UTM_Y = "";
                            Avaluo.P_Grados_X = Txt_X_Horas.Text.ToUpper().Trim();
                            Avaluo.P_Minutos_X = Txt_X_Minutos.Text.ToUpper().Trim();
                            Avaluo.P_Segundos_X = Txt_X_Segundos.Text.ToUpper().Trim();
                            Avaluo.P_Orientacion_X = Cmb_Latitud.SelectedValue;
                            Avaluo.P_Grados_Y = Txt_Y_Horas.Text.ToUpper().Trim();
                            Avaluo.P_Minutos_Y = Txt_Y_Minutos.Text.ToUpper().Trim();
                            Avaluo.P_Segundos_Y = Txt_Y_Segundos.Text.ToUpper().Trim();
                            Avaluo.P_Orientacion_Y = Cmb_Longitud.SelectedValue;
                        }
                        else if (Cmb_Coordenadas.SelectedValue == "UTM")
                        {
                            Avaluo.P_Tipo = Cmb_Coordenadas.SelectedValue;
                            Avaluo.P_Coordenadas_UTM = Txt_Coordenadas_UTM.Text.ToUpper();
                            Avaluo.P_Coordenadas_UTM_Y = Txt_Coordenadas_UTM_Y.Text.ToUpper();
                            Avaluo.P_Grados_X = "";
                            Avaluo.P_Minutos_X = "";
                            Avaluo.P_Segundos_X = "";
                            Avaluo.P_Orientacion_X = "";
                            Avaluo.P_Grados_Y = "";
                            Avaluo.P_Minutos_Y = "";
                            Avaluo.P_Segundos_Y = "";
                            Avaluo.P_Orientacion_Y = "";
                        }

                        Avaluo.P_Valor_Total_Predio = Txt_Valor_Total_Predio.Text.Replace(",", "");
                        Avaluo.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                        //Avaluo.P_Coord_Norte = Txt_Norte.Text.ToUpper();
                        //Avaluo.P_Coord_Oriente = Txt_Oriente.Text.ToUpper();
                        //Avaluo.P_Coord_Poniente = Txt_Poniente.Text.ToUpper();
                        //Avaluo.P_Coord_Sur = Txt_Sur.Text.ToUpper();

                        Cls_Cat_Cat_Peritos_Internos_Negocio Perito_Int = new Cls_Cat_Cat_Peritos_Internos_Negocio();
                        Perito_Int.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
                        DataTable Dt_Peritos_Internos = Perito_Int.Consultar_Peritos_Internos();
                        Avaluo.P_Perito_Interno_Id = Dt_Peritos_Internos.Rows[0]["PERITO_INTERNO_ID"].ToString();
                        Avaluo.P_Nombre_Predio = "";
                        Avaluo.P_Estatus = Cmb_Estatus.SelectedValue;
                        //Guardar Dt's
                        Guardar_Grid_Calculos();
                        Guardar_Grid_Descripcion_Terreno();
                        Guardar_Grid_Valores_Construccion();
                        Guardar_Dt_Elementos_Construccion();
                        Avaluo.P_Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
                        Avaluo.P_Dt_Calculo_Valor_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
                        Avaluo.P_Dt_Calculo_Valor_Terreno = (DataTable)Session["Dt_Grid_Calculos"];
                        Avaluo.P_Dt_Clasificacion_Zona = (DataTable)Session["Dt_Caracteristicas"];
                        Avaluo.P_Dt_Medidas = (DataTable)Session["Dt_Medidas"];
                        Avaluo.P_Dt_Documentos = (DataTable)Session["Dt_Documentos"];
                        Avaluo.P_No_Asignacion = Hdf_No_Asignacion.Value;
                        Avaluo.P_Permitir_Revision = Cmb_Revision.SelectedValue;
                        Avaluo.P_Comentarios_Revisor = "";
                        if ((Avaluo.Alta_Avaluo_Rustico()))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico", "alert('Alta Exitosa');", true);
                            Hdf_No_Avaluo.Value = Avaluo.P_No_Avaluo;
                            Hdf_Anio_Avaluo.Value = Avaluo.P_Anio_Avaluo;
                            Txt_No_Avaluo.Text = Avaluo.P_Anio_Avaluo + "/" + Convert.ToDouble(Avaluo.P_No_Avaluo);
                            Guardar_Imagenes(Avaluo.P_Dt_Documentos);
                            Imprimir_Reporte(Crear_Ds_Avaluo_Urbano_Reporte(), "Rpt_Ope_Cat_Avaluo_Rustico_Av.rpt", "Avaluo_Rustico", "Window_Frm", "Avaluo_Rustico");
                            Btn_Salir_Click(null, null);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico", "alert('Alta Errónea');", true);
                        }
                    }
                }
            }
            else
            {

            }
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = "Seleccione una Cuenta Predial asignada.";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del botón modificar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Avaluos_Urbanos.SelectedIndex > -1)
            {
                if ((Grid_Avaluos_Urbanos.SelectedRow.Cells[8].Text == "POR VALIDAR" 
                    || Grid_Avaluos_Urbanos.SelectedRow.Cells[8].Text == "RECHAZADO")
                    )
                {
                        if (Btn_Modificar.AlternateText.Equals("Modificar"))
                        {
                            if (Cmb_Revision.SelectedValue == "NO")
                            {
                                if (Grid_Avaluos_Urbanos.SelectedRow.Cells[8].Text == "PAGADO")
                                {
                                    Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio();
                                    Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
                                    Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
                                    DateTime DaTi = Convert.ToDateTime(Avaluo.Consultar_Avaluo_Rustico().Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Fecha_Autorizo].ToString());
                                    DateTime DaTi_New = DaTi.AddDays(30);
                                    if (DateTime.Compare(DaTi, DaTi_New) == 1)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('No es posible actualizar el Avalúo');", true);
                                        return;
                                    }
                                }
                                Configuracion_Formulario(false);
                                Btn_Modificar.AlternateText = "Actualizar";
                                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                                Btn_Salir.AlternateText = "Cancelar";
                                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                                Btn_Nuevo.Visible = false;
                                Btn_Imprimir.Visible = false;
                                Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Factores = new Cls_Cat_Cat_Tabla_Factores_Negocio();
                                Tabla_Factores.P_Anio = DateTime.Now.Year.ToString();
                                DataTable Dt_Factores_Cobro = Tabla_Factores.Consultar_Tabla_Factores_Cobro_Avaluos();
                                Grid_Documentos.Columns[0].Visible = true;
                                Grid_Documentos.Columns[1].Visible = true;
                                Grid_Documentos.DataSource = (DataTable)Session["Dt_Documentos"];
                                Grid_Documentos.DataBind();
                                Grid_Documentos.Columns[0].Visible = false;
                                Grid_Documentos.Columns[1].Visible = false;
                                Btn_Imprimir_Cuentas.Visible = false;
                            }
                        }
                    else
                    {
                        if (Validar_Componentes())
                        {
                            Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio();
                            Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
                            Avaluo.P_Anio_Avaluo = DateTime.Now.Year.ToString();
                            Avaluo.P_Motivo_Avaluo_Id = Cmb_Motivo_Avaluo.SelectedValue;
                            Avaluo.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                            Avaluo.P_Propietario = Txt_Propietario.Text.ToUpper();
                            Avaluo.P_Solicitante = Txt_Solicitante.Text.ToUpper();
                            Avaluo.P_Clave_Catastral = Txt_Clave_Catastral.Text.ToUpper();
                            Avaluo.P_Domicilio_Notificar = Txt_Domicilio_Not.Text.ToUpper();
                            Avaluo.P_Municipio_Notificar = Txt_Municipio_Notificar.Text.ToUpper();
                            Avaluo.P_Ubicacion = Txt_Ubicacion_Predio.Text.ToUpper();
                            Avaluo.P_Localidad_Municipio = Txt_Localidad.Text.ToUpper();
                            if (Cmb_Coordenadas.SelectedValue == "CART")
                            {
                                Avaluo.P_Tipo = Cmb_Coordenadas.SelectedValue;
                                Avaluo.P_Coordenadas_UTM = "";
                                Avaluo.P_Coordenadas_UTM_Y = "";
                                Avaluo.P_Grados_X = Txt_X_Horas.Text.ToUpper().Trim();
                                Avaluo.P_Minutos_X = Txt_X_Minutos.Text.ToUpper().Trim();
                                Avaluo.P_Segundos_X = Txt_X_Segundos.Text.ToUpper().Trim();
                                Avaluo.P_Orientacion_X = Cmb_Latitud.SelectedValue;
                                Avaluo.P_Grados_Y = Txt_Y_Horas.Text.ToUpper().Trim();
                                Avaluo.P_Minutos_Y = Txt_Y_Minutos.Text.ToUpper().Trim();
                                Avaluo.P_Segundos_Y = Txt_Y_Segundos.Text.ToUpper().Trim();
                                Avaluo.P_Orientacion_Y = Cmb_Longitud.SelectedValue;
                            }
                            else if (Cmb_Coordenadas.SelectedValue == "UTM")
                            {
                                Avaluo.P_Tipo = Cmb_Coordenadas.SelectedValue;
                                Avaluo.P_Coordenadas_UTM = Txt_Coordenadas_UTM.Text.ToUpper();
                                Avaluo.P_Coordenadas_UTM_Y = Txt_Coordenadas_UTM_Y.Text.ToUpper();
                                Avaluo.P_Grados_X = "";
                                Avaluo.P_Minutos_X = "";
                                Avaluo.P_Segundos_X = "";
                                Avaluo.P_Orientacion_X = "";
                                Avaluo.P_Grados_Y = "";
                                Avaluo.P_Minutos_Y = "";
                                Avaluo.P_Segundos_Y = "";
                                Avaluo.P_Orientacion_Y = "";
                            }
                            Avaluo.P_Valor_Total_Predio = Txt_Valor_Total_Predio.Text.Replace(",", "");
                            Avaluo.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                            //Avaluo.P_Coord_Norte = Txt_Norte.Text.ToUpper();
                            //Avaluo.P_Coord_Oriente = Txt_Oriente.Text.ToUpper();
                            //Avaluo.P_Coord_Poniente = Txt_Poniente.Text.ToUpper();
                            //Avaluo.P_Coord_Sur = Txt_Sur.Text.ToUpper();
                            if (Cmb_Tipo_Construccion.SelectedValue == "OTRO")
                            {
                                Avaluo.P_Uso = Txt_Uso_Constru.Text.ToUpper();
                            }
                            else
                            {
                                Avaluo.P_Uso = Cmb_Tipo_Construccion.SelectedValue;
                            }
                            Avaluo.P_Perito_Externo_Id = Cls_Sessiones.Empleado_ID;
                            Avaluo.P_Nombre_Predio = "";
                            Avaluo.P_Estatus = Cmb_Estatus.SelectedValue;
                            Avaluo.P_Veces_Rechazo = Grid_Avaluos_Urbanos.SelectedRow.Cells[4].Text;
                            Avaluo.P_Permitir_Revision = Cmb_Revision.SelectedValue;
                            //Guardar Dt's
                            Guardar_Dt_Elementos_Construccion();
                            Guardar_Grid_Calculos();
                            Guardar_Grid_Descripcion_Terreno();
                            Guardar_Grid_Valores_Construccion();
                            Avaluo.P_Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
                            Avaluo.P_Dt_Calculo_Valor_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
                            Avaluo.P_Dt_Calculo_Valor_Terreno = (DataTable)Session["Dt_Grid_Calculos"];
                            Avaluo.P_Dt_Clasificacion_Zona = (DataTable)Session["Dt_Caracteristicas"];
                            Avaluo.P_Dt_Observaciones = (DataTable)Session["Dt_Motivos_Rechazo"];
                            Avaluo.P_Dt_Medidas = (DataTable)Session["Dt_Medidas"];
                            Avaluo.P_Dt_Documentos = (DataTable)Session["Dt_Documentos"];
                            if (Grid_Avaluos_Urbanos.SelectedRow.Cells[8].Text != "PAGADO")
                            {
                                if (Avaluo.P_Dt_Observaciones != null && Avaluo.P_Dt_Observaciones.Rows.Count > 0)
                                {
                                    Avaluo.P_Estatus = "POR VALIDAR";
                                }
                            }
                            else if (Grid_Avaluos_Urbanos.SelectedRow.Cells[8].Text == "PAGADO")
                            {
                                Avaluo.P_Estatus = "AUTORIZADO";
                            }
                            if ((Avaluo.Modificar_Avaluo_Rustico()))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico", "alert('Actualizacion Exitosa');", true);
                                Hdf_No_Avaluo.Value = Avaluo.P_No_Avaluo;
                                Hdf_Anio_Avaluo.Value = Avaluo.P_Anio_Avaluo;
                                Eliminar_Imagenes(Avaluo.P_Dt_Documentos);
                                Guardar_Imagenes(Avaluo.P_Dt_Documentos);
                                Btn_Salir_Click(null, null);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico", "alert('Actualización Errónea');", true);
                            }
                        }
                    }
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el Avalúo a modificar.";
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Guardar_Dt_Elementos_Construccion
    ///DESCRIPCIÓN: Guarda los cambios en el grid de elementos de la construccion
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Guardar_Dt_Elementos_Construccion()
    {
        DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
        for (int i = 0; i < Dt_Elementos_Construccion.Rows.Count; i++)
        {
            TextBox Txt_A_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[2].FindControl("Txt_A");
            TextBox Txt_B_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[3].FindControl("Txt_B");
            TextBox Txt_C_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[4].FindControl("Txt_C");
            TextBox Txt_D_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[5].FindControl("Txt_D");
            TextBox Txt_E_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[6].FindControl("Txt_E");
            TextBox Txt_F_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[7].FindControl("Txt_F");
            TextBox Txt_G_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[8].FindControl("Txt_G");
            TextBox Txt_H_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[9].FindControl("Txt_H");
            TextBox Txt_I_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[10].FindControl("Txt_I");
            TextBox Txt_J_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[11].FindControl("Txt_J");
            TextBox Txt_K_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[12].FindControl("Txt_K");
            TextBox Txt_L_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[13].FindControl("Txt_L");
            TextBox Txt_M_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[14].FindControl("Txt_M");
            TextBox Txt_N_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[15].FindControl("Txt_N");
            TextBox Txt_O_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[16].FindControl("Txt_O");
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_A"] = Txt_A_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_B"] = Txt_B_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_C"] = Txt_C_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_D"] = Txt_D_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_E"] = Txt_E_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_F"] = Txt_F_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_G"] = Txt_G_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_H"] = Txt_H_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_I"] = Txt_I_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_J"] = Txt_J_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_K"] = Txt_K_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_L"] = Txt_L_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_M"] = Txt_M_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_N"] = Txt_N_Temporal.Text.ToUpper();
            Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_O"] = Txt_O_Temporal.Text.ToUpper();
        }
    }

    protected void Btn_Agregar_Med_Col_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Medida_Colindancia.Text.Trim() != "")
        {
            DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
            Boolean Entro = false;
            foreach (DataRow Dr_Renglon in Dt_Medidas.Rows)
            {
                if (Dr_Renglon[Ope_Cat_Colindancias_Arv.Campo_Medida_Colindancia].ToString() == Txt_Medida_Colindancia.Text.ToUpper() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Entro = true;
                    break;
                }
            }
            if (!Entro)
            {
                DataRow Dr_Nuevo = Dt_Medidas.NewRow();
                Dr_Nuevo[Ope_Cat_Colindancias_Arv.Campo_No_Colindancia] = " ";
                Dr_Nuevo["ACCION"] = "ALTA";
                Dr_Nuevo[Ope_Cat_Colindancias_Arv.Campo_Medida_Colindancia] = Txt_Medida_Colindancia.Text.ToUpper();
                Dt_Medidas.Rows.Add(Dr_Nuevo);
                Dt_Medidas.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Grid_Colindancias.Columns[0].Visible = true;
                Grid_Colindancias.DataSource = Dt_Medidas;
                Grid_Colindancias.DataBind();
                Grid_Colindancias.Columns[0].Visible = false;
            }
            Txt_Medida_Colindancia.Text = "";
        }
    }
    protected void Grid_Colindancias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
        if (Dt_Medidas != null)
        {
            Dt_Medidas.DefaultView.RowFilter = "ACCION <> 'BAJA'";
            Grid_Colindancias.Columns[0].Visible = true;
            Grid_Colindancias.DataSource = Dt_Medidas;
            Grid_Colindancias.DataBind();
            Grid_Colindancias.Columns[0].Visible = false;

        }
    }
    protected void Grid_Colindancias_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Colindancias.SelectedIndex > -1)
        {
            DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
            if (Dt_Medidas != null)
            {
                foreach (DataRow Dr_Renglon in Dt_Medidas.Rows)
                {
                    if (Dr_Renglon[Ope_Cat_Colindancias_Arv.Campo_Medida_Colindancia].ToString() == Grid_Colindancias.SelectedRow.Cells[1].Text.ToUpper() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                    {
                        Dr_Renglon["ACCION"] = "BAJA";
                    }
                }
                Dt_Medidas.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Grid_Colindancias.Columns[0].Visible = true;
                Grid_Colindancias.DataSource = Dt_Medidas;
                Grid_Colindancias.DataBind();
                Grid_Colindancias.Columns[0].Visible = false;
                Grid_Colindancias.SelectedIndex = -1;
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Avaluo_Urbano
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte del avalúo urbano
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Avaluo_Urbano()
    {
        Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio();
        Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Interno = new Cls_Cat_Cat_Peritos_Externos_Negocio();
        Ds_Ope_Cat_Folio_Pago_Avaluo_Rustico Folio_Avaluo = new Ds_Ope_Cat_Folio_Pago_Avaluo_Rustico();
        Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
        Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
        DataTable Dt_Avaluo = Avaluo.Consultar_Avaluo_Rustico();
        Perito_Interno.P_Perito_Externo_Id = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Perito_Externo_Id].ToString();
        
        Numalet Cantidad = new Numalet();
        Cantidad.MascaraSalidaDecimal = "00/100 M.N.";
        Cantidad.SeparadorDecimalSalida = "Pesos";
        Cantidad.ApocoparUnoParteEntera = true;
        Cantidad.LetraCapital = true;
        Dt_Avaluo = Perito_Interno.Consultar_Peritos_Externos();
        DataTable Dt_Folio_Avaluo = Folio_Avaluo.Tables["Dt_Folio"];
        DataRow Dr_Renglon_Nuevo = Dt_Folio_Avaluo.NewRow();
        Dr_Renglon_Nuevo["NOMBRE"] = Dt_Avaluo.Rows[0]["PERITO_EXTERNO"].ToString();
        Dr_Renglon_Nuevo["RFC"] = Dt_Avaluo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Observaciones].ToString();
        Dr_Renglon_Nuevo["PERITO_EXTERNO_ID"] = Convert.ToInt16(Dt_Avaluo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id].ToString()).ToString();
        Dr_Renglon_Nuevo["FOLIO"] = "AR" + Txt_No_Avaluo.Text;
        Dr_Renglon_Nuevo["UBICACION"] = Txt_Ubicacion_Predio.Text + ", " + Txt_Localidad.Text;
        Dr_Renglon_Nuevo["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
        //Dr_Renglon_Nuevo["CANTIDAD_LETRAS"] = Cantidad.ToCustomCardinal(Cantidad_Cobro).ToUpper();
        Dr_Renglon_Nuevo["VALOR_PREDIO"] = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
        Dr_Renglon_Nuevo["DIA"] = DateTime.Now.Day.ToString();
        Dr_Renglon_Nuevo["MES"] = DateTime.Now.ToString("MMMM").ToUpper();
        Dr_Renglon_Nuevo["ANIO"] = DateTime.Now.Year.ToString();
        Session["E_Mail"] = Dt_Avaluo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Usuario].ToString();
        Dt_Folio_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
        return Folio_Avaluo;
    }
    public static String Leyendas = " ";
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Avaluo_Urbano
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte del avalúo urbano
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Avaluo_Urbano_Reporte()
    {
        String Valuador = "";
        Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio();
        Cls_Cat_Cat_Peritos_Internos_Negocio Perito_Interno = new Cls_Cat_Cat_Peritos_Internos_Negocio();

        DataTable Dt_Perito_Interno;

        Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
        Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;

        DataTable Dt_Avaluo = Avaluo.Consultar_Avaluo_Rustico();
        Ds_Ope_Cat_Avaluo_Rustico Ds_Avaluo_Urbano = new Ds_Ope_Cat_Avaluo_Rustico();

        Perito_Interno.P_Perito_Interno_Id = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Perito_Interno_Id].ToString();
        Dt_Perito_Interno = Perito_Interno.Consultar_Peritos_Internos();
        if (Dt_Perito_Interno.Rows.Count > 0)
        {
            Valuador=Dt_Perito_Interno.Rows[0]["EMPLEADO"].ToString();
            }
            //if (Dt_Perito_Interno.Rows[0][Cat_Cat_Peritos_Internos.Campo_Telefono].ToString().Trim() != "")
            //    Valuador += "TEL. " + Dt_Perito_Interno.Rows[0][Cat_Cat_Peritos_Externos.Campo_Telefono].ToString();
        
        
        DataTable Dt_Datos_Generales = Ds_Avaluo_Urbano.Tables["DT_DATOS_GENERALES"];
        DataRow Dr_Avaluo;
        Dr_Avaluo = Dt_Datos_Generales.NewRow();
        Dr_Avaluo["MOTIVO_AVALUO"] = Cmb_Motivo_Avaluo.SelectedItem.Text;
        Dr_Avaluo["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
        Dr_Avaluo["PROPIETARIO"] = Txt_Propietario.Text;
        Dr_Avaluo["SOLICITANTE"] = Txt_Solicitante.Text;
        Dr_Avaluo["CLAVE_CATASTRAL"] = Txt_Clave_Catastral.Text;
        Dr_Avaluo["DOMICILIO_NOTIFICAR"] = Txt_Domicilio_Not.Text;
        Dr_Avaluo["MUNICIPIO_NOTIFICAR"] = Txt_Municipio_Notificar.Text;
        Dr_Avaluo["UBICACION"] = Txt_Ubicacion_Predio.Text;
        Dr_Avaluo["LOCALIDAD_MUNICIPIO"] = Txt_Localidad.Text;
        Dr_Avaluo["FECHA"] = Convert.ToDateTime(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Fecha_Autorizo].ToString());
        Dr_Avaluo["NOMBRE_PREDIO"] = "";
        if (Cmb_Coordenadas.SelectedValue == "CART")
        {
            Dr_Avaluo["COORD_X"] = Txt_X_Horas.Text + "°" + Txt_X_Minutos.Text + "'" + Txt_X_Segundos.Text + "'' " + Cmb_Latitud.SelectedValue;
            Dr_Avaluo["COORD_Y"] = Txt_Y_Horas.Text + "°" + Txt_Y_Minutos.Text + "'" + Txt_Y_Segundos.Text + "'' " + Cmb_Longitud.SelectedValue;
        }
        else if (Cmb_Coordenadas.SelectedValue == "UTM") 
        {
            Dr_Avaluo["COORD_X"] = Txt_Coordenadas_UTM.Text;
            Dr_Avaluo["COORD_Y"] = Txt_Coordenadas_UTM_Y.Text;
        }
        Dr_Avaluo["VALOR_TOTAL_PREDIO"] = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
        //Dr_Avaluo["NORTE"] = Txt_Norte.Text;
        //Dr_Avaluo["SUR"] = Txt_Sur.Text;
        //Dr_Avaluo["ORIENTE"] = Txt_Oriente.Text;
        //Dr_Avaluo["PONIENTE"] = Txt_Poniente.Text;
        Dr_Avaluo["VALUADOR"] = Valuador;
        Dr_Avaluo["NO_VALUADOR"] = Convert.ToInt16(Dt_Perito_Interno.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Perito_Interno_Id].ToString());
        Dr_Avaluo["OBSERVACIONES"] = Txt_Observaciones.Text;
        Dr_Avaluo["LEYENDAS"] = Leyendas.ToString();
        Dr_Avaluo["FIRMANTE"] = Cmb_Firmante.SelectedItem.Text.Split('-')[0].ToString().Trim();
        Dr_Avaluo["PUESTO"] = Cmb_Firmante.SelectedItem.Text.Split('-')[1].ToString().Trim();
        //Dr_Avaluo["CLAVE_CATASTRAL"] = Txt_Clave_Catastral.Text;
        //Dr_Avaluo["MANZANA"] = Txt_Manzana.Text;
        //Dr_Avaluo["LOTE"] = Txt_Lote.Text;
        Dr_Avaluo["FOLIO"] = "AR" + Txt_No_Avaluo.Text;
        Dt_Datos_Generales.Rows.Add(Dr_Avaluo);
        DataTable Dt_Terreno = Ds_Avaluo_Urbano.Tables["DT_TERRENO"];
        DataTable Dt_Calculos = (DataTable)Session["Dt_Grid_Calculos"];
        foreach (DataRow Dr_Renglon in Dt_Calculos.Rows)
        {
            Dr_Avaluo = Dt_Terreno.NewRow();
            Dr_Avaluo["CLASIFICACION"] = Dr_Renglon[Cat_Cat_Tipos_Constru_Rustico.Campo_Identificador].ToString();
            Dr_Avaluo["SUPERFICIE_HA"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Terreno_Arv.Campo_Superficie].ToString());
            Dr_Avaluo["VALOR_HA"] = Convert.ToDouble(Dr_Renglon[Cat_Cat_Tab_Val_Const_Rustico.Campo_Valor_M2].ToString());
            Dr_Avaluo["FACTOR"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Terreno_Arv.Campo_Factor].ToString());
            Dr_Avaluo["VALOR_PARCIAL"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Terreno_Arv.Campo_Valor_Parcial].ToString());
            Dr_Avaluo["GRUPO"] = "A";
            Dt_Terreno.Rows.Add(Dr_Avaluo);
        }

        DataTable Dt_Construccion = Ds_Avaluo_Urbano.Tables["DT_CONSTRUCCION"];
        DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
        foreach (DataRow Dr_Renglon in Dt_Valores_Construccion.Rows)
        {
            Dr_Avaluo = Dt_Construccion.NewRow();
            Dr_Avaluo["CROQUIS"] = Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arv.Campo_Croquis].ToString();
            Dr_Avaluo["TIPO"] = Convert.ToInt16(Dr_Renglon["TIPO"].ToString());
            Dr_Avaluo["EDO"] = Convert.ToInt16(Dr_Renglon["CON_SERV"].ToString());
            Dr_Avaluo["SUPERFICIE_M2"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arv.Campo_Superficie_M2].ToString());
            Dr_Avaluo["VALOR_PARCIAL"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arv.Campo_Valor_Parcial].ToString());

            if ((Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arv.Campo_Edad_Constru].ToString()) == "")
            {
                Dr_Avaluo["EDAD"] = 0;
             
            }
            else
            {
                Dr_Avaluo["EDAD"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arv.Campo_Edad_Constru].ToString());
            }
            Dr_Avaluo["FACTOR"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arv.Campo_Factor].ToString());
            Dr_Avaluo["USO"] = Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arv.Campo_Uso_Contru].ToString();
            Dr_Avaluo["VALOR_M2"] = Convert.ToDouble(Dr_Renglon["VALOR_M2"].ToString());
            Dr_Avaluo["GRUPO"] = "A";
            Dt_Construccion.Rows.Add(Dr_Avaluo);
        }

        DataTable Dt_Caracteristicas_Terreno = Ds_Avaluo_Urbano.Tables["DT_CARACTERISTICAS"];
        DataTable Dt_Caracteristicas = (DataTable)Session["Dt_Caracteristicas"];
        foreach (DataRow Dr_Renglon in Dt_Caracteristicas.Rows)
        {
            Dr_Avaluo = Dt_Caracteristicas_Terreno.NewRow();
            Dr_Avaluo["CLASIFICACION"] = Dr_Renglon[Cat_Cat_Descrip_Const_Rustico.Campo_Identificador].ToString();
            //Dr_Avaluo["INDICE"] = Convert.ToDouble(Dr_Renglon[Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice].ToString());
            Dr_Avaluo["INDICADOR_A"] = Dr_Renglon[Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A].ToString();
            Dr_Avaluo["VALOR_INDICADOR_A"] = Dr_Renglon[Ope_Cat_Caracteristicas_Arv.Campo_Valor_Indicador_A].ToString();

            Dr_Avaluo["INDICADOR_B"] = Dr_Renglon[Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_B].ToString();
            Dr_Avaluo["VALOR_INDICADOR_B"] = Dr_Renglon[Ope_Cat_Caracteristicas_Arv.Campo_Valor_Indicador_B].ToString();
            Dr_Avaluo["INDICADOR_C"] = Dr_Renglon[Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_C].ToString();
            Dr_Avaluo["VALOR_INDICADOR_C"] = Dr_Renglon[Ope_Cat_Caracteristicas_Arv.Campo_Valor_Indicador_C].ToString();
            Dr_Avaluo["INDICADOR_D"] = Dr_Renglon[Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_D].ToString();
            Dr_Avaluo["VALOR_INDICADOR_D"] = Dr_Renglon[Ope_Cat_Caracteristicas_Arv.Campo_Valor_Indicador_D].ToString();
            Dr_Avaluo["GRUPO"] = "A";
            Dt_Caracteristicas_Terreno.Rows.Add(Dr_Avaluo);
        }

        DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
        DataTable Dt_MedColindancias = Ds_Avaluo_Urbano.Tables["DT_COLINDANCIAS"];
        foreach (DataRow Dr_Renglon in Dt_Medidas.Rows)
        {
            Dr_Avaluo = Dt_MedColindancias.NewRow();
            Dr_Avaluo["COLINDANCIA"] = Dr_Renglon[Ope_Cat_Colindancias_Ara.Campo_Medida_Colindancia].ToString();
            Dr_Avaluo["GRUPO"] = "A";
            Dt_MedColindancias.Rows.Add(Dr_Avaluo);
        }
        return Ds_Avaluo_Urbano;
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
        String File_Path = Server.MapPath("../Rpt/Catastro/" + Nombre_Reporte);
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
        catch (Exception Ex)
        {

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
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Frm_Formato, "window.open('" + Pagina + "', '" + Formato + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo_Cuenta
    ///DESCRIPCIÓN: Envia un correo al correo del perito externo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Enviar_Correo_Cuenta(String E_Mail, String Url_Adjunto)
    {
        String Contenido = "";
        Contenido = "Su avalúo Rústico ha sido autorizado. Favor de pasar a pagar en las cajas de Presidencia de Irapuato, su folio de pago se encuentra adjunto a este correo. Favor de imprimirlo dos veces";
        try
        {
            if (E_Mail.Trim().Length > 0)
            {
                Cls_Mail mail = new Cls_Mail();
                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = E_Mail.Trim();
                mail.P_Subject = "Avalúo Rústico Autorizado";
                mail.P_Texto = Contenido;
                mail.P_Adjunto = Url_Adjunto;//Hacer_Pdf();
                mail.Enviar_Correo();
            }
            if (File.Exists(Server.MapPath("../../Reporte/Rpt_Folio_Pago_Avaluo_Rustico.pdf")))
            {
                File.Delete(Server.MapPath("../../Reporte/Rpt_Folio_Pago_Avaluo_Rustico.pdf"));
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("No se pudo enviar el Correo.");
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN: Evento del botón Imprimir
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        //Mandar imprimir el reporte
        if (Grid_Avaluos_Urbanos.SelectedIndex > -1)
        {
            //if (Grid_Avaluos_Urbanos.SelectedRow.Cells[8].Text == "AUTORIZADO")
            //{
                Imprimir_Reporte(Crear_Ds_Avaluo_Urbano_Reporte(), "Rpt_Ope_Cat_Avaluo_Rustico_Av.rpt", "Avaluo_Rustico", "Window_Frm", "Avaluo_Rustico");
            //}
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
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
            String Clave_Ingreso_Id = "";
            //String Costo_Clave_Ingreso = "";
            String Dependencia_Id = "";
            String Consulta = "SELECT " + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " LIKE '%AUTORIZACION AVALUOS PERITOS FISCALES%'";
            Clave_Ingreso_Id = Obtener_Dato_Consulta(Consulta);
            if (Clave_Ingreso_Id.Trim() != "")
            {
                Consulta = "SELECT " + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID 
                + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso 
                + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Descripcion 
                + " LIKE '%AUTORIZACION AVALUOS PERITOS FISCALES%'";
                Dependencia_Id = Obtener_Dato_Consulta(Consulta);
                if (Dependencia_Id.Trim() != "")
                {
                    Cls_Cat_Cat_Parametros_Negocio Dias = new Cls_Cat_Cat_Parametros_Negocio();
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "AUTORIZACION DE AVALUO RUSTICO";
                    if (Hdf_Cobro_Anterior.Value.Trim() == "")
                    {
                        Hdf_Cobro_Anterior.Value = "0.00";
                    }
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Clave_Ingreso_Id;
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dependencia_Id;
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_Id.Value;
                    Calculo_Impuesto_Traslado.P_Contribuyente = Txt_Propietario.Text;
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.AddDays(Convert.ToInt16(Dias.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Dias_Vigencia].ToString())).ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                }
                else
                {
                    
                }
            }
            else
            {
            }
        }
        catch (Exception Ex)
        {
            //Mostrar_Mensaje_Error("No se puede insertar el pasivo.");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Obtener_Dato_Consulta(String Consulta)
    {
        String Dato_Consulta = "";

        try
        {
            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Consulta);

            if (Dr_Dato.Read())
            {
                if (Dr_Dato[0] != null)
                {
                    Dato_Consulta = Dr_Dato[0].ToString();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato.Close();
            }
            else
            {
                Dato_Consulta = "";
            }
            Dr_Dato = null;
        }
        catch
        {
        }
        finally
        {
        }

        return Dato_Consulta;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Evento del botón buscar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Busqueda.Text.Trim() != "")
        {
            Llenar_Tabla_Avaluos_Urbanos(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Avaluos_Urbanos_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Avaluos_Urbanos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Avaluos_Urbanos(e.NewPageIndex);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Avaluos_Urbanos_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Avaluos_Urbanos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Avaluos_Urbanos.SelectedIndex > -1)
        {

            Cls_Cat_Cat_Parametros_Negocio Par = new Cls_Cat_Cat_Parametros_Negocio();
            Int16 columnas = Convert.ToInt16(Par.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Columnas_Calc_Construccion].ToString());
            Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Factores = new Cls_Cat_Cat_Tabla_Factores_Negocio();
            Tabla_Factores.P_Anio = DateTime.Now.Year.ToString();
            DataTable Dt_Factores_Cobro = Tabla_Factores.Consultar_Tabla_Factores_Cobro_Avaluos();
            DataTable Dt_Avaluo;
            Hdf_Anio_Avaluo.Value = Grid_Avaluos_Urbanos.SelectedRow.Cells[2].Text;
            Hdf_No_Avaluo.Value = Grid_Avaluos_Urbanos.SelectedRow.Cells[1].Text;
            Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio Aval_Urb = new Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio();
            Aval_Urb.P_No_Avaluo = Hdf_No_Avaluo.Value;
            Aval_Urb.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
            Session["Dt_Tabla_Valores_Construccion"] = Aval_Urb.Consultar_Tabla_Valores_Construccion();
            Dt_Avaluo = Aval_Urb.Consultar_Avaluo_Rustico();
            Cargar_Datos_Avaluo(Dt_Avaluo);
            Session["Dt_Grid_Calculos"] = Aval_Urb.P_Dt_Calculo_Valor_Terreno.Copy();
            Grid_Calculos.Columns[3].Visible = true;
            Grid_Calculos.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Terreno;
            Grid_Calculos.PageIndex = 0;
            Grid_Calculos.DataBind();
            Grid_Calculos.Columns[3].Visible = false;
            Session["Dt_Grid_Valores_Construccion"] = Aval_Urb.P_Dt_Calculo_Valor_Construccion.Copy();
            Grid_Valores_Construccion.Columns[5].Visible = true;
            Grid_Valores_Construccion.Columns[7].Visible = true;
            Grid_Valores_Construccion.Columns[9].Visible = true;
            Grid_Valores_Construccion.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Construccion;
            Grid_Valores_Construccion.PageIndex = 0;
            Grid_Valores_Construccion.DataBind();
            Grid_Valores_Construccion.Columns[5].Visible = false;
            Grid_Valores_Construccion.Columns[7].Visible = false;
            Grid_Valores_Construccion.Columns[9].Visible = false;

            DataTable Dt_Caracteristicas;
            Crear_Tabla_Construccion_Dominante(Aval_Urb.P_Dt_Clasificacion_Zona.Copy());
            Dt_Caracteristicas = (DataTable)Session["Dt_Caracteristicas"];
            Grid_Descripcion_Terreno.Columns[0].Visible = true;
            Grid_Descripcion_Terreno.Columns[2].Visible = true;
            Grid_Descripcion_Terreno.Columns[6].Visible = true;
            Grid_Descripcion_Terreno.Columns[10].Visible = true;
            Grid_Descripcion_Terreno.Columns[14].Visible = true;
            Grid_Descripcion_Terreno.DataSource = Dt_Caracteristicas;
            Grid_Descripcion_Terreno.DataBind();
            Grid_Descripcion_Terreno.Columns[0].Visible = false;
            Grid_Descripcion_Terreno.Columns[2].Visible = false;
            Grid_Descripcion_Terreno.Columns[6].Visible = false;
            Grid_Descripcion_Terreno.Columns[10].Visible = false;
            Grid_Descripcion_Terreno.Columns[14].Visible = false;

            Session["Dt_Grid_Elementos_Construccion"] = Aval_Urb.P_Dt_Elementos_Construccion.Copy();
            Grid_Elementos_Construccion.Columns[0].Visible = true;
            for (Int16 i = 1; i < (columnas + 1); i++)
            {
                Grid_Elementos_Construccion.Columns[i + 1].Visible = true;
            }
            Grid_Elementos_Construccion.DataSource = Aval_Urb.P_Dt_Elementos_Construccion;
            Grid_Elementos_Construccion.PageIndex = 0;
            Grid_Elementos_Construccion.DataBind();
            Grid_Elementos_Construccion.Columns[0].Visible = false;
            for (int i = (columnas + 1); i < 16; i++)
            {
                Grid_Elementos_Construccion.Columns[i + 1].Visible = false;
            }
            //for (int i = 1; i < (columnas + 11); i++)
            //{
            //    Grid_Elementos_Construccion.Columns[i + 1].Visible = true;
            //}


            DataTable Dt_Medidas = Aval_Urb.P_Dt_Medidas.Copy();
            Grid_Colindancias.Columns[0].Visible = true;
            Grid_Colindancias.DataSource = Dt_Medidas;
            Grid_Colindancias.DataBind();
            Grid_Colindancias.Columns[0].Visible = false;
            Session["Dt_Medidas"] = Dt_Medidas.Copy();
            //Cargar los demás grids con las tablas que trae el objeto Aval_Urb.
            //Fin de cargar datos del avalúo
            Div_Grid_Avaluo.Visible = false;
            Div_Datos_Avaluo.Visible = true;
            Session["Anio"] = Hdf_Anio_Avaluo.Value;
            Calcular_Totales_Construccion();
            Calcular_Totales_Terreno();
            Calcular_Valor_Total_Predio();
            Btn_Salir.AlternateText = "Atras";
            Div_Observaciones.Visible = true;
            DataTable Dt_Motivos_Rechazo;
            Aval_Urb.P_Estatus = "= 'VIGENTE'";
            Dt_Motivos_Rechazo = Aval_Urb.Consultar_Motivos_Rechazo_Avaluo();
            Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
            Grid_Observaciones.Columns[0].Visible = true;
            Grid_Observaciones.Columns[1].Visible = true;
            Grid_Observaciones.Columns[3].Visible = true;
            Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
            Grid_Observaciones.PageIndex = 0;
            Grid_Observaciones.DataBind();
            Grid_Observaciones.Columns[0].Visible = false;
            Grid_Observaciones.Columns[1].Visible = false;
            Grid_Observaciones.Columns[3].Visible = false;

            DataTable Dt_Archivos = Aval_Urb.P_Dt_Documentos.Copy();
            Dt_Archivos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
            Dt_Archivos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
            Session["Dt_Documentos"] = Dt_Archivos;
            Grid_Documentos.Columns[0].Visible = true;
            Grid_Documentos.Columns[1].Visible = true;
            Grid_Documentos.DataSource = Dt_Archivos;
            Grid_Documentos.DataBind();
            Grid_Documentos.Columns[0].Visible = false;
            Grid_Documentos.Columns[1].Visible = false;
        }
    }

    private void Consultar_Identificadores_Predio()
    {
        try
        {
            DataTable Dt_Identificadores;
            Cls_Cat_Cat_Identificadores_Predio_Negocio Identificadores = new Cls_Cat_Cat_Identificadores_Predio_Negocio();
            Identificadores.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
            Dt_Identificadores = Identificadores.Consultar_Identificadores_Predio();
            if (Dt_Identificadores.Rows.Count > 0)
            {
                //Txt_Region.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Region].ToString().Trim();
                //Txt_Manzana.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Manzana].ToString().Trim();
                //Txt_Lote.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Lote].ToString().Trim();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Consultar_Identificadores_Predio: " + Ex.Message);
        }
    }

    private void Cargar_Datos_Avaluo(DataTable Dt_Avaluo)

    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_consulta_Ope_Resumen = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        try
        {
            if (Dt_Avaluo.Rows.Count > 0)
            {
                Hdf_Cuenta_Predial_Id.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Cuenta_Predial_Id].ToString();
                Txt_Observaciones.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Observaciones].ToString();
                Txt_Observaciones_Rechazo.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Observaciones_Perito].ToString();
                Txt_Solicitante.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Solicitante].ToString();
                Txt_No_Avaluo.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo].ToString()+"/" + Convert.ToInt16(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo]).ToString();
                Txt_Cuenta_Predial.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Cuenta_Predial].ToString();
                Hdf_Anio_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo].ToString();
                Cmb_Motivo_Avaluo.SelectedIndex = Cmb_Motivo_Avaluo.Items.IndexOf(Cmb_Motivo_Avaluo.Items.FindByValue(HttpUtility.HtmlDecode(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Motivo_Avaluo_Id].ToString())));
                Txt_Propietario.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Propietario].ToString();
                Cmb_Estatus.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Estatus].ToString();
                Txt_Domicilio_Not.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Domicilio_Notificacion].ToString();
                Txt_Municipio_Notificar.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Municipio_Notificacion].ToString();
                Txt_Ubicacion_Predio.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Ubicacion].ToString();
                Txt_Localidad.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Localidad_Municipio].ToString();
                Txt_Fecha.Text = Convert.ToDateTime(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Fecha_Creo].ToString()).ToString("dd/MMM/yyyy");
                Cmb_Revision.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Permitir_Revision].ToString();
                //Txt_Nombre_Predio.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Nombre_Predio].ToString();
                Txt_X_Horas.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_X_Grados].ToString();
                Txt_X_Minutos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_X_Minutos].ToString();
                Txt_X_Segundos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_X_Segundos].ToString();
                try
                {
                    Cmb_Tipo_Construccion.SelectedIndex = -1;
                    Cmb_Tipo_Construccion.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Uso].ToString();
                    if (Cmb_Tipo_Construccion.SelectedValue != Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Uso].ToString())
                    {
                        Cmb_Tipo_Construccion.SelectedValue = "OTRO";
                        Txt_Uso_Constru.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Uso].ToString();
                        Txt_Uso_Constru.Enabled = false;
                    }
                    Txt_Uso_Constru.Enabled = false;
                }
                catch
                {
                    Cmb_Tipo_Construccion.SelectedValue = "OTRO";
                    Txt_Uso_Constru.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Uso].ToString();
                    Txt_Uso_Constru.Enabled = false;
                }
                if (Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Orientacion_X].ToString().Trim() != "")
                {
                    Cmb_Latitud.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Orientacion_X].ToString().Trim();
                }
                else
                {
                    Cmb_Latitud.SelectedValue = "SELECCIONE";
                }
                Txt_Y_Horas.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_Y_Grados].ToString();
                Txt_Y_Minutos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_Y_Minutos].ToString();
                Txt_Y_Segundos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_Y_Segundos].ToString();
                if (Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Orientacion_Y].ToString().Trim() != "")
                {
                    Cmb_Longitud.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Orientacion_Y].ToString().Trim();
                }
                else
                {
                    Cmb_Longitud.SelectedValue = "SELECCIONE";
                }

                Txt_Coordenadas_UTM.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Coordenadas_UTM].ToString().Trim();
                Txt_Coordenadas_UTM_Y.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Coordenadas_UTM_Y].ToString().Trim();
                if (Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Tipo].ToString().Trim() == "")
                {
                    Cmb_Coordenadas.SelectedIndex = 0;
                }
                else
                {
                    Cmb_Coordenadas.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Tipo].ToString().Trim();
                }
                Cmb_Coordenadas_SelectedIndexChanged(null, null);
                //Txt_Observaciones.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Observaciones].ToString();
                //Txt_Valor_Total_Predio.Text = Convert.ToDouble(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Valor_Total_Predio].ToString()).ToString("#,###,###,###,###,###,###,##0.00");
                //Txt_Norte.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_Norte].ToString();
                //Txt_Sur.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_Sur].ToString();
                //Txt_Oriente.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_Oriente].ToString();
                //Txt_Poniente.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_Poniente].ToString();
                //Txt_Observaciones_Rechazo.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Obervaciones_Perito].ToString();
                //Txt_Lote.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Lote].ToString();
                //Txt_Manzana.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Lote].ToString();
                //Txt_Region.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Region].ToString();

                Hdf_Cuenta_Predial_Id.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Cuenta_Predial_Id].ToString();
                ////Txt_Observaciones.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Observaciones].ToString();
                //// Txt_No_Avaluo.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo].ToString();
                //Txt_Cuenta_Predial.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Cuenta_Predial].ToString();

                //Rs_consulta_Ope_Resumen.P_Cuenta_Predial_ID = Dt_Avaluo.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();

                //if (Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString() != string.Empty)
                //{
                //    Rs_consulta_Ope_Resumen.P_Estado_Predio = Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
                //    DataTable Dt_Estado_Predio = Rs_consulta_Ope_Resumen.Consultar_Estado_Predio();
                //    Txt_Municipio_Notificar.Text = Dt_Estado_Predio.Rows[0]["Descripcion"].ToString();
                //}
                //if (Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString() != string.Empty)
                //{
                //    DateTime Fecha_Avaluo;
                //    DateTime.TryParse(Dt_Avaluo.Rows[0]["Fecha_Avaluo"].ToString(), out Fecha_Avaluo);

                //    //Txt_Fecha.Text = "";
                //    //M_Orden_Negocio.P_Fecha_Avaluo = Fecha_Avaluo;

                //}


                //if (Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString() != string.Empty)
                //{
                //    Rs_consulta_Ope_Resumen.P_Calle_ID = Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                //    DataTable Dt_Calles = Rs_consulta_Ope_Resumen.Consultar_Calle_Generales();
                //    Txt_Ubicacion_Predio.Text = Dt_Calles.Rows[0]["Nombre"].ToString();
                //    Rs_consulta_Ope_Resumen.P_Colonia_ID = Dt_Calles.Rows[0]["Colonia_ID"].ToString();
                //    DataTable Dt_Colonia = Rs_consulta_Ope_Resumen.Consultar_Colonia_Generales();
                //    // Txt_Colonia.Text = Dt_Colonia.Rows[0]["Nombre"].ToString();
                //}
                //if (Dt_Avaluo.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString() != "")
                //{
                //    Rs_consulta_Ope_Resumen.P_Colonia_ID = Dt_Avaluo.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                //    DataTable Dt_Colonia = Rs_consulta_Ope_Resumen.Consultar_Colonia_Generales();
                //    //Txt_Colonia.Text = Dt_Colonia.Rows[0][Cat_Ate_Colonias.Campo_Nombre].ToString();
                //    M_Orden_Negocio.P_Ubicacion_Cuenta = Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                //}
                //if (Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString().Trim() != "")
                //{
                //    Txt_Ubicacion_Predio.Text = Txt_Ubicacion_Predio.Text + " NO. EXT. " + Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
                //}
                //M_Orden_Negocio.P_Exterior_Cuenta = Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
                //if (Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim() != "")
                //{
                //    Txt_Ubicacion_Predio.Text = Txt_Ubicacion_Predio.Text + " NO. INT. " + Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
                //}
                //M_Orden_Negocio.P_Interior_Cuenta = Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
                Txt_Clave_Catastral.Text = Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
                //M_Orden_Negocio.P_Clave_Catastral = Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
                //if (String.IsNullOrEmpty(Dt_Avaluo.Rows[0]["Estado_ID_Notificacion"].ToString()))
                //{
                //    Rs_consulta_Ope_Resumen.P_Estado_Predio = (Dt_Avaluo.Rows[0]["Estado_ID_Notificacion"].ToString());
                //    DataTable Dt_Estado_Propietario = Rs_consulta_Ope_Resumen.Consultar_Estado_Predio_Propietario();
                //    if (Dt_Estado_Propietario.Rows.Count > 0)
                //    {
                //        Txt_Localidad.Text = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                //        Txt_Ubicacion_Predio.Text = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                //        M_Orden_Negocio.P_Estado_Propietario = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                //    }
                //}
                //else if (!String.IsNullOrEmpty(Dt_Avaluo.Rows[0]["Estado_Notificacion"].ToString()))
                //{
                //    Txt_Localidad.Text = Dt_Avaluo.Rows[0]["Estado_Notificacion"].ToString();
                //}
                //if (!String.IsNullOrEmpty(Dt_Avaluo.Rows[0]["Ciudad_ID_Notificacion"].ToString()))
                //{
                //    Rs_consulta_Ope_Resumen.P_Ciudad_ID = Dt_Avaluo.Rows[0]["Ciudad_ID_Notificacion"].ToString();
                //    DataTable Dt_Ciudad_Propietario = Rs_consulta_Ope_Resumen.Consultar_Ciudad();
                //    Txt_Municipio_Notificar.Text = Dt_Ciudad_Propietario.Rows[0]["Nombre"].ToString();
                //    M_Orden_Negocio.P_Ciudad_Propietario = Dt_Avaluo.Rows[0]["Ciudad_ID_Notificacion"].ToString();
                //}
                //else if (!String.IsNullOrEmpty(Dt_Avaluo.Rows[0]["Ciudad_Notificacion"].ToString()))
                //{
                //    Txt_Municipio_Notificar.Text = Dt_Avaluo.Rows[0]["Ciudad_Notificacion"].ToString();
                //}

                Txt_Municipio_Notificar.Text = "IRAPUATO";
                Txt_Localidad.Text = "IRAPUATO,GTO.";
                //if (Dt_Avaluo.Rows[0]["Domicilio_Foraneo"].ToString().Trim() == "SI")
                //{
                //    if (!String.IsNullOrEmpty(Dt_Avaluo.Rows[0]["Calle_Notificacion"].ToString()))
                //    {
                //        Txt_Domicilio_Not.Text = Dt_Avaluo.Rows[0]["Calle_Notificacion"].ToString();
                //        if (Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString().Trim() != "")
                //        {
                //            Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + " NO. EXT. " + Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
                //        }
                //        if (Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim() != "")
                //        {
                //            Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + " NO. INT. " + Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
                //        }
                //    }
                //}
                //else
                //{
                //    if (!String.IsNullOrEmpty(Dt_Avaluo.Rows[0]["Colonia_ID_Notificacion"].ToString()))
                //    {
                //        Rs_consulta_Ope_Resumen.P_Colonia_ID = Dt_Avaluo.Rows[0]["Colonia_ID_Notificacion"].ToString();
                //        DataTable DT_Colonia_Propietario = Rs_consulta_Ope_Resumen.Consultar_Colonia_Generales();
                //        //Txt_Colonia_Not.Text = DT_Colonia_Propietario.Rows[0]["Nombre"].ToString();
                //    }
                //    if (!String.IsNullOrEmpty(Dt_Avaluo.Rows[0]["Calle_ID_Notificacion"].ToString()))
                //    {
                //        Rs_consulta_Ope_Resumen.P_Calle_ID = Dt_Avaluo.Rows[0]["Calle_ID_Notificacion"].ToString();//*
                //        DataTable Dt_Calle_Propietario = Rs_consulta_Ope_Resumen.Consultar_Calle_Generales();
                //        Txt_Domicilio_Not.Text = Dt_Calle_Propietario.Rows[0]["Nombre"].ToString();
                //        if (Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString().Trim() != "")
                //        {
                //            Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + ", NO. EXT. " + Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString();
                //        }
                //        if (Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim() != "")
                //        {
                //            Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + ", NO. INT. " + Dt_Avaluo.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString();
                //        }
            }



            Consultar_Identificadores_Predio();
        }

        catch (Exception e)
        {
            Lbl_Ecabezado_Mensaje.Text = e.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    protected void Txt_Cuenta_Predial_TextChanged(object sender, EventArgs e)
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        DataTable Dt_Cuenta;
        Cuenta_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.ToUpper();
        Dt_Cuenta = Cuenta_Predial.Consultar_Cuenta();
        if (Dt_Cuenta.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_Id.Value = Dt_Cuenta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
        }
        else
        {
            Hdf_Cuenta_Predial_Id.Value = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico", "alert('La Cuenta Predial ingresada no existe actualmente');", true);
        }
    }

    private void Crear_Mascara(Int16 Cantidad_Decimales)
    {
        Mascara_Caracteres = "";
        if (Cantidad_Decimales > 0)
        {
            for (int i = 0; i < Cantidad_Decimales; i++)
            {
                Mascara_Caracteres += "0";
            }
        }
    }

    String Mascara_Caracteres;

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Calculos_DataBound
    ///DESCRIPCIÓN: carga los datos en los componentes del grid
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Calculos_DataBound(object sender, EventArgs e)
    {
        Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
        DataTable Dt_Parametros = Parametros.Consultar_Parametros();
        Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
        DataTable Dt_Calculos = (DataTable)Session["Dt_Grid_Calculos"];
        for (int i = 0; i < Dt_Calculos.Rows.Count; i++)
        {
            TextBox Txt_Superficie_M2_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[2].FindControl("Txt_Superficie_M2");
            TextBox Txt_Valor_M2_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[4].FindControl("Txt_Valor_M2");
            TextBox Txt_Factor_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[5].FindControl("Txt_Factor");
            TextBox Txt_Total_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[6].FindControl("Txt_Total");
            Txt_Superficie_M2_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["SUPERFICIE"].ToString()).ToString("###,###,###,##0." + Mascara_Caracteres);
            if (Dt_Calculos.Rows[i]["VALOR_M2"].ToString().Trim() != "")
            {
                Txt_Valor_M2_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["VALOR_M2"].ToString()).ToString("###,###,###,##0.00");
            }
            else
            {
                Txt_Valor_M2_Temporal.Text = "0.00";
            }
            Txt_Factor_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["FACTOR"].ToString()).ToString("###,###,###,##0." + Mascara_Caracteres);
            Txt_Total_Temporal.Text = Convert.ToDouble(Dt_Calculos.Rows[i]["VALOR_PARCIAL"].ToString()).ToString("###,###,###,###,###,##0.00");
        }
    }

    protected void Grid_Calculos_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void Grid_Valores_Construccion_DataBound(object sender, EventArgs e)
    {
        DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
        for (int i = 0; i < Dt_Valores_Construccion.Rows.Count; i++)
        {
            TextBox Txt_Croquis = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[0].FindControl("Txt_Croquis");
            TextBox Txt_Tipo_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[1].FindControl("Txt_Tipo");
            TextBox Txt_Con_Serv_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[2].FindControl("Txt_Con_Serv");
            TextBox Txt_Superficie_M2_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[3].FindControl("Txt_Superficie_M2");
            TextBox Txt_Valor_M2_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[4].FindControl("Txt_Valor_X_M2");
            TextBox Txt_Valor_Construccion_Id_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[5].FindControl("Txt_Valor_Construccion_Id");
            TextBox Txt_Factor_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[8].FindControl("Txt_Factor");
            TextBox Txt_Edad = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[7].FindControl("Txt_Edad");
            TextBox Txt_Total_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[6].FindControl("Txt_Total");
            TextBox Txt_Uso = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[9].FindControl("Txt_Uso");
            Txt_Croquis.Text = Dt_Valores_Construccion.Rows[i]["CROQUIS"].ToString();
            Txt_Tipo_Temporal.Text = Dt_Valores_Construccion.Rows[i]["TIPO"].ToString();
            Txt_Con_Serv_Temporal.Text = Dt_Valores_Construccion.Rows[i]["CON_SERV"].ToString();
            Txt_Superficie_M2_Temporal.Text = Convert.ToDouble(Dt_Valores_Construccion.Rows[i]["SUPERFICIE_M2"].ToString()).ToString("###,###,###,##0.00");
            Txt_Valor_M2_Temporal.Text = Convert.ToDouble(Dt_Valores_Construccion.Rows[i]["VALOR_M2"].ToString()).ToString("###,###,###,##0.00");
            Txt_Valor_Construccion_Id_Temporal.Text = Dt_Valores_Construccion.Rows[i]["VALOR_CONSTRUCCION_ID"].ToString();
            Txt_Factor_Temporal.Text = Convert.ToDouble(Dt_Valores_Construccion.Rows[i]["FACTOR"].ToString()).ToString("###,###,###,##0.00");
            Txt_Total_Temporal.Text = Convert.ToDouble(Dt_Valores_Construccion.Rows[i]["VALOR_PARCIAL"].ToString()).ToString("###,###,###,###,###,##0.00");
            Txt_Edad.Text = Dt_Valores_Construccion.Rows[i]["EDAD_CONSTRU"].ToString();
            Txt_Uso.Text = Dt_Valores_Construccion.Rows[i]["USO_CONTRU"].ToString();
        }
    }

    protected void Guardar_Grid_Descripcion_Terreno()
    {
        DataTable Dt_Caracteristicas = (DataTable)Session["Dt_Caracteristicas"];
        for (int i = 0; i < Dt_Caracteristicas.Rows.Count; i++)
        {
            CheckBox Chk_Indicador_Valor_A = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[5].FindControl("Chk_Indicador_Valor_A");
            CheckBox Chk_Indicador_Valor_B = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[7].FindControl("Chk_Indicador_Valor_B");
            CheckBox Chk_Indicador_Valor_C = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[9].FindControl("Chk_Indicador_Valor_C");
            CheckBox Chk_Indicador_Valor_D = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[11].FindControl("Chk_Indicador_Valor_D");
            if (Chk_Indicador_Valor_A.Checked)
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Arv.Campo_Valor_Indicador_A] = "X";
            }
            else
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Arv.Campo_Valor_Indicador_A] = "";
            }
            if (Chk_Indicador_Valor_B.Checked)
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Arv.Campo_Valor_Indicador_B] = "X";
            }
            else
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Arv.Campo_Valor_Indicador_B] = "";
            }
            if (Chk_Indicador_Valor_C.Checked)
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Arv.Campo_Valor_Indicador_C] = "X";
            }
            else
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Arv.Campo_Valor_Indicador_C] = "";
            }
            if (Chk_Indicador_Valor_D.Checked)
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Arv.Campo_Valor_Indicador_D] = "X";
            }
            else
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Arv.Campo_Valor_Indicador_D] = "";
            }
        }

    }

    protected void Grid_Descripcion_Terreno_DataBound(object sender, EventArgs e)
    {
        DataTable Dt_Caracteristicas = (DataTable)Session["Dt_Caracteristicas"];
        for (int i = 0; i < Dt_Caracteristicas.Rows.Count; i++)
        {
            CheckBox Chk_Indicador_Valor_A = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[5].FindControl("Chk_Indicador_Valor_A");
            CheckBox Chk_Indicador_Valor_B = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[9].FindControl("Chk_Indicador_Valor_B");
            CheckBox Chk_Indicador_Valor_C = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[13].FindControl("Chk_Indicador_Valor_C");
            CheckBox Chk_Indicador_Valor_D = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[17].FindControl("Chk_Indicador_Valor_D");
            if (Dt_Caracteristicas.Rows[i]["DESCRIPCION_RUSTICO_ID1"].ToString().Trim() != "")
            {
                if (Dt_Caracteristicas.Rows[i]["VALOR_INDICADOR_A"].ToString().Trim() != "")
                {
                    Chk_Indicador_Valor_A.Checked = true;
                }
                else
                {
                    Chk_Indicador_Valor_A.Checked = false;
                }
            }
            else
            {
                Chk_Indicador_Valor_A.Visible = false;
                Chk_Indicador_Valor_A.Checked = false;
            }
            if (Dt_Caracteristicas.Rows[i]["DESCRIPCION_RUSTICO_ID2"].ToString().Trim() != "")
            {
                if (Dt_Caracteristicas.Rows[i]["VALOR_INDICADOR_B"].ToString().Trim() != "")
                {
                    Chk_Indicador_Valor_B.Checked = true;
                }
                else
                {
                    Chk_Indicador_Valor_B.Checked = false;
                }
            }
            else
            {
                Chk_Indicador_Valor_B.Visible = false;
                Chk_Indicador_Valor_B.Checked = false;
            }
            if (Dt_Caracteristicas.Rows[i]["DESCRIPCION_RUSTICO_ID3"].ToString().Trim() != "")
            {
                if (Dt_Caracteristicas.Rows[i]["VALOR_INDICADOR_C"].ToString().Trim() != "")
                {
                    Chk_Indicador_Valor_C.Checked = true;
                }
                else
                {
                    Chk_Indicador_Valor_C.Checked = false;
                }
            }
            else
            {
                Chk_Indicador_Valor_C.Visible = false;
                Chk_Indicador_Valor_C.Checked = false;
            }
            if (Dt_Caracteristicas.Rows[i]["DESCRIPCION_RUSTICO_ID4"].ToString().Trim() != "")
            {
                if (Dt_Caracteristicas.Rows[i]["VALOR_INDICADOR_D"].ToString().Trim() != "")
                {
                    Chk_Indicador_Valor_D.Checked = true;
                }
                else
                {
                    Chk_Indicador_Valor_D.Checked = false;
                }
            }
            else
            {
                Chk_Indicador_Valor_D.Visible = false;
                Chk_Indicador_Valor_D.Checked = false;
            }
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Observaciones_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Observaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Observaciones.SelectedIndex = -1;
            Grid_Observaciones.Columns[1].Visible = true;
            Grid_Observaciones.Columns[2].Visible = true;
            Grid_Observaciones.DataSource = (DataTable)Session["Dt_Motivos_Rechazo"];
            Grid_Observaciones.PageIndex = e.NewPageIndex;
            Grid_Observaciones.DataBind();
            Grid_Observaciones.Columns[1].Visible = false;
            Grid_Observaciones.Columns[2].Visible = false;
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Superficie_M2_Cal_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Superficie_M2 en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Superficie_M2_Cal_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            TextBox Txt_Superficie_M2_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Superficie_M2_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Superficie_M2 = gvr.FindControl("Txt_Superficie_M2") as TextBox;
            try
            {
                if (Text_Txt_Superficie_M2.Text.Trim() != "")
                {
                    Text_Txt_Superficie_M2.Text = Convert.ToDouble(Text_Txt_Superficie_M2.Text).ToString("###,###,###,###,##0." + Mascara_Caracteres);
                }
                else
                {
                    Text_Txt_Superficie_M2.Text = "0." + Mascara_Caracteres;
                }
            }
            catch (Exception Exc)
            {
                Text_Txt_Superficie_M2.Text = "0." + Mascara_Caracteres;
            }
            Calcular_Valor_Parcial_Terreno(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Factor_Cal_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Factor en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Factor_Cal_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            DataTable Dt_Parametros = Parametros.Consultar_Parametros();
            Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
            TextBox Txt_Factor_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Factor_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Factor = gvr.FindControl("Txt_Factor") as TextBox;
            try
            {
                if (Text_Txt_Factor.Text.Trim() != "")
                {
                    Text_Txt_Factor.Text = Convert.ToDouble(Text_Txt_Factor.Text).ToString("###,###,###,###,##0." + Mascara_Caracteres);
                }
                else
                {
                    Text_Txt_Factor.Text = "0." + Mascara_Caracteres;
                }
            }
            catch (Exception Exc)
            {
                Text_Txt_Factor.Text = "0." + Mascara_Caracteres;
            }
            Calcular_Valor_Parcial_Terreno(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Superficie_M2_Constru_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Factor_Ef en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Superficie_M2_Constru_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            TextBox Txt_Superficie_M2_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Superficie_M2_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Superficie_M2 = gvr.FindControl("Txt_Superficie_M2") as TextBox;
            try
            {
                if (Text_Txt_Superficie_M2.Text.Trim() != "")
                {
                    Text_Txt_Superficie_M2.Text = Convert.ToDouble(Text_Txt_Superficie_M2.Text).ToString("###,###,###,###,##0.00");
                }
                else
                {
                    Text_Txt_Superficie_M2.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
                Text_Txt_Superficie_M2.Text = "0.00";
            }
            Calcular_Valor_Parcial_Construccion(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    protected void Txt_Edad_TextChanged(object sender, EventArgs e)
    {

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Factor_Constru_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Factor_Ef en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Factor_Constru_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            TextBox Txt_Factor_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Factor_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Factor = gvr.FindControl("Txt_Factor") as TextBox;
            try
            {
                if (Text_Txt_Factor.Text.Trim() != "")
                {
                    Text_Txt_Factor.Text = Convert.ToDouble(Text_Txt_Factor.Text).ToString("###,###,###,###,##0.00");
                }
                else
                {
                    Text_Txt_Factor.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
                Text_Txt_Factor.Text = "0.00";
            }
            Calcular_Valor_Parcial_Construccion(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    private void Calcular_Valor_Parcial_Construccion(int Index)
    {
        Double Valor_Parcial = 0;
        Valor_Parcial = Convert.ToDouble(((TextBox)Grid_Valores_Construccion.Rows[Index].Cells[3].FindControl("Txt_Superficie_M2")).Text) 
            * Convert.ToDouble(((TextBox)Grid_Valores_Construccion.Rows[Index].Cells[4].FindControl("Txt_Valor_X_M2")).Text) 
            * Convert.ToDouble(((TextBox)Grid_Valores_Construccion.Rows[Index].Cells[8].FindControl("Txt_Factor")).Text);
        TextBox Text_Txt_Valor_Parcial = (TextBox)Grid_Valores_Construccion.Rows[Index].Cells[6].FindControl("Txt_Total");
        Text_Txt_Valor_Parcial.Text = Valor_Parcial.ToString("###,###,###,###,###,##0.00");
        Calcular_Totales_Construccion();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Totales_Construccion
    ///DESCRIPCIÓN: recorre el grid de valores de construcción, suma la superficie_m2 y el valor parcial y los visualiza en las cajas de texto correspondientes.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Totales_Construccion()
    {
        Double Superficie_Total = 0;
        Double Valor_Total = 0;
        foreach (GridViewRow Renglon_Grid in Grid_Valores_Construccion.Rows)
        {
            Superficie_Total += Convert.ToDouble(((TextBox)Renglon_Grid.Cells[3].FindControl("Txt_Superficie_M2")).Text);
            Valor_Total += Convert.ToDouble(((TextBox)Renglon_Grid.Cells[7].FindControl("Txt_Total")).Text);
        }
        Txt_Construccion_Superficie_Total.Text = Superficie_Total.ToString("###,###,###,###,###,##0.00");
        Txt_Construccion_Valor_Total.Text = Valor_Total.ToString("###,###,###,###,###,##0.00");
        Calcular_Valor_Total_Predio();
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
                Hdf_Cuenta_Predial_Id.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                Cargar_Datos();
            }
        }
        //Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        //Session.Remove("CUENTA_PREDIAL_ID");
    }


    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: asignar datos de cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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
            Lbl_Ecabezado_Mensaje.Text = Ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
            //throw new Exception(Ex.Message);

        }
        Div_Contenedor_Msj_Error.Visible = false;
    }
    Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio;

    private void Cargar_Generales_Cuenta(DataTable dataTable)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        try
        {
            Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy");
             Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            Txt_Cuota_Anual_Anterior.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()).ToString("###,###,###,###,###,###,##0.00");
            Txt_Valor_Total_Predio_Anterior.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()).ToString("###,###,###,###,###,###,##0.00");
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
                DataTable Dt_Estado_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                DataTable Dt_Calles = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                Txt_Ubicacion_Predio.Text = "CALLE " + Dt_Calles.Rows[0]["Nombre"].ToString();
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = Dt_Calles.Rows[0]["Colonia_ID"].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                //Txt_Colonia.Text = Dt_Colonia.Rows[0]["Nombre"].ToString();
            }

            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString().Trim() != "")
            {
                Txt_Ubicacion_Predio.Text = Txt_Ubicacion_Predio.Text + ", NO. EXT. " + dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            }
            M_Orden_Negocio.P_Exterior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim() != "")
            {
                Txt_Ubicacion_Predio.Text = Txt_Ubicacion_Predio.Text + ", NO. INT. " + dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            }
            M_Orden_Negocio.P_Interior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            if (dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString() != "")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Ubicacion_Predio.Text =Txt_Ubicacion_Predio.Text + ", COLONIA " + Dt_Colonia.Rows[0][Cat_Ate_Colonias.Campo_Nombre].ToString();
                M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            }
            Txt_Clave_Catastral.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            M_Orden_Negocio.P_Clave_Catastral = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            //Txt_Valor_Total_Predio_Anterior.Text = Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()).ToString("###,###,###,###,###,###,##0.00"); ;
            //Txt_Cuota_Anual_Anterior.Text = Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()).ToString("###,###,###,###,###,###,##0.00");
            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = (dataTable.Rows[0]["Estado_ID_Notificacion"].ToString());
                DataTable Dt_Estado_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio_Propietario();
                if (Dt_Estado_Propietario.Rows.Count > 0)
                {
                    //Txt_Localidad_Not.Text = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                    Txt_Localidad.Text = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                    M_Orden_Negocio.P_Estado_Propietario = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                }
            }
            //else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_Notificacion"].ToString()))
            //{
            //    Txt_Localidad_Not.Text = dataTable.Rows[0]["Estado_Notificacion"].ToString();
            //}
            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Ciudad_ID = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
                DataTable Dt_Ciudad_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ciudad();
                //Txt_Municipio_Not.Text = Dt_Ciudad_Propietario.Rows[0]["Nombre"].ToString();
                M_Orden_Negocio.P_Ciudad_Propietario = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
            }
            //else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_Notificacion"].ToString()))
            //{
            //    Txt_Municipio_Not.Text = dataTable.Rows[0]["Ciudad_Notificacion"].ToString();
            //}
            Txt_Municipio_Notificar.Text = "IRAPUATO";
            Txt_Localidad.Text = "IRAPUATO, GTO.";
            if (dataTable.Rows[0]["Domicilio_Foraneo"].ToString().Trim() == "SI")
            {
                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_Notificacion"].ToString()))
                {
                    Txt_Domicilio_Not.Text = "CALLE " + dataTable.Rows[0]["Calle_Notificacion"].ToString();
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString().Trim() != "")
                    {
                        Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + ", NO. EXT. " + dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
                    }
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim() != "")
                    {
                        Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + ", NO. INT. " + dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
                    }
                    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_Notificacion"].ToString()))
                    {
                        Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + ", COLONIA " + dataTable.Rows[0]["Colonia_Notificacion"].ToString();
                    }
                }
            }
            else
            {

                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_ID_Notificacion"].ToString()))
                {
                    Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0]["Calle_ID_Notificacion"].ToString();//*
                    DataTable Dt_Calle_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                    Txt_Domicilio_Not.Text = "CALLE " + Dt_Calle_Propietario.Rows[0]["Nombre"].ToString();
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString().Trim() != "")
                    {
                        Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + ", NO. EXT. " + dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString();
                    }
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim() != "")
                    {
                        Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + ", NO. INT. " + dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString();
                    }
                    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString()))
                    {
                        Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString();
                        DataTable DT_Colonia_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                        Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text + ", COLONIA " + DT_Colonia_Propietario.Rows[0]["Nombre"].ToString();
                    }
                }
            }
            String t="  ";
            Txt_Domicilio_Not.Text = Txt_Domicilio_Not.Text.Replace("   ", "");
            Txt_Ubicacion_Predio.Text = Txt_Ubicacion_Predio.Text.Replace("   ", "");
            Consultar_Identificadores_Predio();
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
                Cargar_Datos_Propietario(((DataSet)Session["Ds_Prop_Datos"]).Tables["Dt_Propietarios"]);
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }

    }

    private void Cargar_Datos_Propietario(DataTable dataTable)
    {
        try
        {
            if (dataTable.Rows.Count > 0 && dataTable != null)
            {
                M_Orden_Negocio.P_Propietario_ID = dataTable.Rows[0]["PROPIETARIO"].ToString(); ;
                Txt_Propietario.Text = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                M_Orden_Negocio.P_Nombre_Propietario = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                M_Orden_Negocio.P_RFC_Propietario = dataTable.Rows[0]["RFC"].ToString();
            }

        }
        catch (Exception Ex)
        {
        }
    }


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
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Valida los datos ingresados
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes()
    {
        Boolean Valido = true;
        String Msj_Error = "Error: ";
        if (Cmb_Motivo_Avaluo.SelectedValue == "SELECCIONE")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Seleccione un motivo para el avalúo.";
            Valido = false;
        }
        if (Hdf_Cuenta_Predial_Id.Value.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese una Cuenta Predial existente.";
            Valido = false;
        }

        if (Txt_Solicitante.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese el Solicitante.";
            Valido = false;
        }
        
        //if (Txt_Nombre_Predio.Text.Trim() == "")
        //{
        //    Msj_Error += "<br/>";
        //    Msj_Error += "+ Ingrese el Nombre del Predio.";
        //    Valido = false;
        //}

        if (Cmb_Coordenadas.SelectedValue == "CART")
        {
            if (Txt_X_Horas.Text.Trim() == "")
            {
                if (Msj_Error.Length > 0)
                {
                    Msj_Error += "<br/>";
                }
                Msj_Error += "+ Ingrese las horas en X.";
                Valido = false;
            }


            if (Txt_X_Minutos.Text.Trim() == "")
            {
                if (Msj_Error.Length > 0)
                {
                    Msj_Error += "<br/>";
                }
                Msj_Error += "+ Ingrese los minutos en X.";
                Valido = false;
            }

            if (Txt_X_Segundos.Text.Trim() == "")
            {
                if (Msj_Error.Length > 0)
                {
                    Msj_Error += "<br/>";
                }
                Msj_Error += "+ Ingrese los segundos en X.";
                Valido = false;
            }

            if (Cmb_Latitud.SelectedIndex == 0)
            {
                if (Msj_Error.Length > 0)
                {
                    Msj_Error += "<br/>";
                }
                Msj_Error += "+ Ingrese la Orientación en X.";
                Valido = false;
            }

            if (Txt_Y_Horas.Text.Trim() == "")
            {
                if (Msj_Error.Length > 0)
                {
                    Msj_Error += "<br/>";
                }
                Msj_Error += "+ Ingrese las horas en Y.";
                Valido = false;
            }


            if (Txt_Y_Minutos.Text.Trim() == "")
            {
                if (Msj_Error.Length > 0)
                {
                    Msj_Error += "<br/>";
                }
                Msj_Error += "+ Ingrese los minutos en Y.";
                Valido = false;
            }


            if (Txt_Y_Segundos.Text.Trim() == "")
            {
                if (Msj_Error.Length > 0)
                {
                    Msj_Error += "<br/>";
                }
                Msj_Error += "+ Ingrese los segundos en Y.";
                Valido = false;
            }

            if (Cmb_Longitud.SelectedIndex == 0)
            {
                if (Msj_Error.Length > 0)
                {
                    Msj_Error += "<br/>";
                }
                Msj_Error += "+ Seleccione la orientación en Y.";
                Valido = false;
            }
        }
        else if (Cmb_Coordenadas.SelectedValue == "UTM")
        {
            if (Txt_Coordenadas_UTM.Text.Trim() == "")
            {
                if (Msj_Error.Length > 0)
                {
                    Msj_Error += "<br/>";
                }
                Msj_Error += "+ Ingrese las cordenadas UTM en X.";
                Valido = false;
            }
            if (Txt_Coordenadas_UTM_Y.Text.Trim() == "")
            {
                if (Msj_Error.Length > 0)
                {
                    Msj_Error += "<br/>";
                }
                Msj_Error += "+ Ingrese las cordenadas UTM en Y.";
                Valido = false;
            }
        }
        else
        {
            if (Msj_Error.Length > 0)
            {
                Msj_Error += "<br/>";
            }
            Msj_Error += "+ Seleccione Coordenadas Cartograficas o UTM.";
            Valido = false;
        }
        if (Txt_Observaciones.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese las Observaciones.";
            Valido = false;
        }

        //if (Txt_Norte.Text.Trim() == "")
        //{
        //    Msj_Error += "<br/>";
        //    Msj_Error += "+ Ingrese las medidas y colindancias al Norte.";
        //    Valido = false;
        //}
        //if (Txt_Sur.Text.Trim() == "")
        //{
        //    Msj_Error += "<br/>";
        //    Msj_Error += "+ Ingrese las medidas y colindancias al Sur.";
        //    Valido = false;
        //}
        //if (Txt_Oriente.Text.Trim() == "")
        //{
        //    Msj_Error += "<br/>";
        //    Msj_Error += "+ Ingrese las medidas y colindancias al Oriente.";
        //    Valido = false;
        //}

        //if (Txt_Poniente.Text.Trim() == "")
        //{
        //    Msj_Error += "<br/>";
        //    Msj_Error += "+ Ingrese las medidas y colindancias al Poniente.";
        //    Valido = false;
        //}
        int Filas_R = Grid_Elementos_Construccion.Rows.Count;
        int Filas_LLenas = 0;

        for (int c = 2; c < Grid_Elementos_Construccion.Columns.Count; c++)
        {
            for (int f = 0; f < Grid_Elementos_Construccion.Rows.Count; f++)
            {
                TextBox Txt_A_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_A");
                TextBox Txt_B_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_B");
                TextBox Txt_C_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_C");
                TextBox Txt_D_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_D");
                TextBox Txt_E_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_E");
                TextBox Txt_F_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_F");
                TextBox Txt_G_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_G");
                TextBox Txt_H_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_H");
                TextBox Txt_I_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_I");
                TextBox Txt_J_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_J");
                TextBox Txt_K_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_K");
                TextBox Txt_L_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_L");
                TextBox Txt_M_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_M");
                TextBox Txt_N_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_N");
                TextBox Txt_O_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[f].Cells[c].FindControl("Txt_O");
                if (Txt_A_Temporal.Text != "" && c == 2)
                {

                    Filas_LLenas++;
                }

                if (Txt_B_Temporal.Text != "" && c == 3)
                {

                    Filas_LLenas++;
                }
                if (Txt_C_Temporal.Text != "" && c == 4)
                {

                    Filas_LLenas++;
                }
                if (Txt_D_Temporal.Text != "" && c == 5)
                {

                    Filas_LLenas++;
                }
                if (Txt_E_Temporal.Text != "" && c == 6)
                {

                    Filas_LLenas++;
                }
                if (Txt_F_Temporal.Text != "" && c == 7)
                {

                    Filas_LLenas++;
                }
                if (Txt_G_Temporal.Text != "" && c == 8)
                {

                    Filas_LLenas++;
                }
                if (Txt_H_Temporal.Text != "" && c == 9)
                {

                    Filas_LLenas++;
                }
                if (Txt_I_Temporal.Text != "" && c == 10)
                {

                    Filas_LLenas++;
                }
                if (Txt_J_Temporal.Text != "" && c == 11)
                {

                    Filas_LLenas++;
                }
                if (Txt_K_Temporal.Text != "" && c == 12)
                {

                    Filas_LLenas++;
                }
                if (Txt_L_Temporal.Text != "" && c == 13)
                {

                    Filas_LLenas++;
                }
                if (Txt_M_Temporal.Text != "" && c == 14)
                {

                    Filas_LLenas++;

                }
                if (Txt_N_Temporal.Text != "" && c == 15)
                {

                    Filas_LLenas++;
                }
                if (Txt_O_Temporal.Text != "" && c == 16)
                {

                    Filas_LLenas++;
                }

            }


            if (Filas_R != Filas_LLenas && Filas_LLenas != 0)
            {
                switch (c)
                {

                    case 2:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna A.";
                        Msj_Error += "<br/>";
                        break;

                    case 3:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna B.";
                        Msj_Error += "<br/>";
                        break;
                    case 4:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna C.";
                        Msj_Error += "<br/>";
                        break;
                    case 5:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna D.";
                        Msj_Error += "<br/>";
                        break;

                    case 6:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna E.";
                        Msj_Error += "<br/>";
                        break;
                    case 7:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna F.";
                        Msj_Error += "<br/>";
                        break;
                    case 8:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna G.";
                        Msj_Error += "<br/>";
                        break;

                    case 9:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna H.";
                        Msj_Error += "<br/>";
                        break;
                    case 10:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna I.";
                        Msj_Error += "<br/>";
                        break;
                    case 11:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna J.";
                        Msj_Error += "<br/>";
                        break;
                    case 12:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna K.";
                        Msj_Error += "<br/>";
                        break;
                    case 13:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna L.";
                        Msj_Error += "<br/>";
                        break;
                    case 14:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna M.";
                        Msj_Error += "<br/>";
                        break;
                    case 15:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna N.";
                        Msj_Error += "<br/>";
                        break;

                    case 16:
                        Msj_Error += "<br/>";
                        Msj_Error += "+ Ingresa todos los campos de la columna O.";
                        Msj_Error += "<br/>";
                        break;
                    default:
                        break;
                }
                Valido = false;


            }
            Filas_LLenas = 0;
        }

        if (!Valido)
        {
            Lbl_Ecabezado_Mensaje.Text = Msj_Error;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Valido;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Dt_Valores_Construccion
    ///DESCRIPCIÓN: Crea la tabla inicial de calculos para el grid Grid_Valores_Construccion
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Crear_Dt_Valores_Construccion()
    {
        Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
        Int16 renglones = Convert.ToInt16(Parametros.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Renglones_Calc_Construccion].ToString());
        DataTable Dt_Valores_Construccion = new DataTable();
        Dt_Valores_Construccion.Columns.Add("CROQUIS", typeof(String));
        Dt_Valores_Construccion.Columns.Add("TIPO", typeof(Int16));
        Dt_Valores_Construccion.Columns.Add("CON_SERV", typeof(Int16));
        Dt_Valores_Construccion.Columns.Add("SUPERFICIE_M2", typeof(Double));
        Dt_Valores_Construccion.Columns.Add("VALOR_M2", typeof(Double));
        Dt_Valores_Construccion.Columns.Add("VALOR_CONSTRUCCION_ID", typeof(String));
        Dt_Valores_Construccion.Columns.Add("FACTOR", typeof(Double));
        Dt_Valores_Construccion.Columns.Add("EDAD_CONSTRU", typeof(Double));
        Dt_Valores_Construccion.Columns.Add("VALOR_PARCIAL", typeof(Double));
        Dt_Valores_Construccion.Columns.Add("USO_CONTRU", typeof(String));
        DataRow Dr_renglon;
        for (int i = 0; i < renglones; i++)
        {
            Dr_renglon = Dt_Valores_Construccion.NewRow();
            switch (i)
            {
                case 0:
                    Dr_renglon["CROQUIS"] = "A";
                    break;
                case 1:
                    Dr_renglon["CROQUIS"] = "B";
                    break;
                case 2:
                    Dr_renglon["CROQUIS"] = "C";
                    break;
                case 3:
                    Dr_renglon["CROQUIS"] = "D";
                    break;
                case 4:
                    Dr_renglon["CROQUIS"] = "E";
                    break;
                case 5:
                    Dr_renglon["CROQUIS"] = "F";
                    break;
                case 6:
                    Dr_renglon["CROQUIS"] = "G";
                    break;
                case 7:
                    Dr_renglon["CROQUIS"] = "H";
                    break;
                case 8:
                    Dr_renglon["CROQUIS"] = "I";
                    break;
                case 9:
                    Dr_renglon["CROQUIS"] = "J";
                    break;
                case 10:
                    Dr_renglon["CROQUIS"] = "K";
                    break;
            }
            Dr_renglon["TIPO"] = 0;
            Dr_renglon["CON_SERV"] = 0;
            Dr_renglon["SUPERFICIE_M2"] = 0;
            Dr_renglon["VALOR_M2"] = 0;
            Dr_renglon["VALOR_CONSTRUCCION_ID"] = " ";
            Dr_renglon["FACTOR"] = 1;
            Dr_renglon["EDAD_CONSTRU"] = 0;
            Dr_renglon["VALOR_PARCIAL"] = 0;
            Dr_renglon["USO_CONTRU"] = "";
            Dt_Valores_Construccion.Rows.Add(Dr_renglon);
        }
        Session["Dt_Grid_Valores_Construccion"] = Dt_Valores_Construccion.Copy();
        Grid_Valores_Construccion.Columns[5].Visible = true;
        Grid_Valores_Construccion.Columns[7].Visible = true;
        Grid_Valores_Construccion.Columns[9].Visible = true;
        Grid_Valores_Construccion.DataSource = Dt_Valores_Construccion;
        Grid_Valores_Construccion.PageIndex = 0;
        Grid_Valores_Construccion.DataBind();
        Grid_Valores_Construccion.Columns[5].Visible = false;
        Grid_Valores_Construccion.Columns[7].Visible = false;
        Grid_Valores_Construccion.Columns[9].Visible = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Con_Serv_Constru_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Factor_Ef en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Con_Serv_Constru_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
            TextBox Txt_Con_Serv_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Con_Serv_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Con_Serv = gvr.FindControl("Txt_Con_Serv") as TextBox;
            TextBox Text_Txt_Tipo = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[1].FindControl("Txt_Tipo");
            try
            {
                if (Text_Txt_Con_Serv.Text.Trim() != "")
                {
                    Text_Txt_Con_Serv.Text = Convert.ToInt16(Text_Txt_Con_Serv.Text).ToString();

                    if (Text_Txt_Tipo.Text.Trim() != "")
                    {
                        DataTable Dt_Tabla_Valores = (DataTable)Session["Dt_Tabla_Valores_Construccion"];
                        Boolean Coinciden_Tipo_Con_Serv = false;
                        String Valor_Construccion_Id = " ";
                        Double Valor_M2 = 0;
                        foreach (DataRow Dr_Renglon in Dt_Tabla_Valores.Rows)
                        {
                            if (Text_Txt_Con_Serv.Text.Trim() == Dr_Renglon["CON_SERV"].ToString() && Text_Txt_Tipo.Text.Trim() == Dr_Renglon["TIPO"].ToString())
                            {
                                Coinciden_Tipo_Con_Serv = true;
                                Valor_Construccion_Id = Dr_Renglon["VALOR_CONSTRUCCION_ID"].ToString();
                                Valor_M2 = Convert.ToDouble(Dr_Renglon["VALOR_M2"].ToString());
                                break;
                            }
                        }
                        if (Coinciden_Tipo_Con_Serv)
                        {
                            TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                            TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                            Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = Valor_Construccion_Id;
                            Txt_Temporal_Val_Const_Id.Text = Valor_Construccion_Id;
                            Txt_Temporal_Valor_M2.Text = Valor_M2.ToString("###,###,###,###,##0.00");
                        }
                        else
                        {
                            TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                            TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                            Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = "";
                            Txt_Temporal_Val_Const_Id.Text = "";
                            Txt_Temporal_Valor_M2.Text = Valor_M2.ToString("###,###,###,###,##0.00");
                        }
                    }
                }
                else
                {
                    Text_Txt_Con_Serv.Text = "0";
                    TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                    TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                    Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = "";
                    Txt_Temporal_Val_Const_Id.Text = "";
                    Txt_Temporal_Valor_M2.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
                Text_Txt_Con_Serv.Text = "0";
                TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = "";
                Txt_Temporal_Val_Const_Id.Text = "";
                Txt_Temporal_Valor_M2.Text = "0.00";
            }
            Calcular_Valor_Parcial_Construccion(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Factor_Ef_Cal_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Factor_Ef en el Grid de Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Tipo_Constru_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
            TextBox Txt_Tipo_Temporal = sender as TextBox;
            GridViewRow gvr = Txt_Tipo_Temporal.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Txt_Tipo = gvr.FindControl("Txt_Tipo") as TextBox;
            TextBox Text_Txt_Con_Serv = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[2].FindControl("Txt_Con_Serv");
            try
            {
                if (Text_Txt_Tipo.Text.Trim() != "")
                {
                    Text_Txt_Tipo.Text = Text_Txt_Tipo.Text.Trim();
                    if (Text_Txt_Con_Serv.Text.Trim() != "")
                    {
                        DataTable Dt_Tabla_Valores = (DataTable)Session["Dt_Tabla_Valores_Construccion"];
                        Boolean Coinciden_Tipo_Con_Serv = false;
                        String Valor_Construccion_Id = " ";
                        Double Valor_M2 = 0;
                        foreach (DataRow Dr_Renglon in Dt_Tabla_Valores.Rows)
                        {
                            if (Text_Txt_Con_Serv.Text.Trim() == Dr_Renglon["CON_SERV"].ToString() && Text_Txt_Tipo.Text.Trim() == Dr_Renglon["TIPO"].ToString())
                            {
                                Coinciden_Tipo_Con_Serv = true;
                                Valor_Construccion_Id = Dr_Renglon["VALOR_CONSTRUCCION_ID"].ToString();
                                Valor_M2 = Convert.ToDouble(Dr_Renglon["VALOR_M2"].ToString());
                                break;
                            }
                        }
                        if (Coinciden_Tipo_Con_Serv)
                        {
                            TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                            TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                            Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = Valor_Construccion_Id;
                            Txt_Temporal_Val_Const_Id.Text = Valor_Construccion_Id;
                            Txt_Temporal_Valor_M2.Text = Valor_M2.ToString("###,###,###,###,##0.00");
                        }
                        else
                        {
                            TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                            TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                            Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = "";
                            Txt_Temporal_Val_Const_Id.Text = "";
                            Txt_Temporal_Valor_M2.Text = "0.00";
                        }
                    }
                }
                else
                {
                    Text_Txt_Tipo.Text = "0";
                    TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                    TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                    Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = "";
                    Txt_Temporal_Val_Const_Id.Text = "";
                    Txt_Temporal_Valor_M2.Text = "0.00";
                }
            }
            catch (Exception Exc)
            {
                Lbl_Ecabezado_Mensaje.Text = Exc.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            Calcular_Valor_Parcial_Construccion(index);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Dt_Calculos
    ///DESCRIPCIÓN: Crea la tabla inicial de calculos para el grid Grid_Calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Crear_Dt_Calculos()
    {
        Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
        Int16 renglones = Convert.ToInt16(Parametros.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Renglones_Calc_Construccion].ToString());
        Double Inc_Esq = Convert.ToDouble(Parametros.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Factor_Ef].ToString());
        DataTable Dt_Calculos = new DataTable();
        Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio();
        avaluo.P_Anio_Avaluo = DateTime.Now.Year.ToString();
        Dt_Calculos = avaluo.Consultar_Tabla_Terreno();
        Session["Dt_Grid_Calculos"] = Dt_Calculos.Copy();
        Grid_Calculos.Columns[0].Visible = true;
        Grid_Calculos.Columns[3].Visible = true;
        Grid_Calculos.DataSource = Dt_Calculos;
        Grid_Calculos.PageIndex = 0;
        Grid_Calculos.DataBind();
        Grid_Calculos.Columns[0].Visible = false;
        Grid_Calculos.Columns[3].Visible = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Valor_Parcial_Terreno
    ///DESCRIPCIÓN: Cálcula el valor parcial del grid_calculos y lo inserta en la caja de texto del grid
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Valor_Parcial_Terreno(int Index)
    {
        TextBox Txt_Superficie_M2 = (TextBox)Grid_Calculos.Rows[Index].Cells[2].FindControl("Txt_Superficie_M2");
        TextBox Txt_Valor_M2 = (TextBox)Grid_Calculos.Rows[Index].Cells[4].FindControl("Txt_Valor_M2");
        TextBox Txt_Factor = (TextBox)Grid_Calculos.Rows[Index].Cells[5].FindControl("Txt_Factor");
        TextBox Text_Txt_Valor_Parcial = (TextBox)Grid_Calculos.Rows[Index].Cells[6].FindControl("Txt_Total");
        Text_Txt_Valor_Parcial.Text = (Convert.ToDouble(Txt_Superficie_M2.Text)
            * Convert.ToDouble(Txt_Valor_M2.Text) * Convert.ToDouble(Txt_Factor.Text)).ToString("###,###,###,###,###,##0.00");
        Calcular_Totales_Terreno();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Totales_Terreno
    ///DESCRIPCIÓN: Cálcula el total de la superficie de m2 y total del grid_calculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Totales_Terreno()
    {
        Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
        DataTable Dt_Parametros = Parametros.Consultar_Parametros();
        Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
        Double Superficie_Total = 0;
        Double Valor_Total = 0;
        foreach (GridViewRow Renglon_Grid in Grid_Calculos.Rows)
        {
            Superficie_Total += Convert.ToDouble(((TextBox)Renglon_Grid.Cells[2].FindControl("Txt_Superficie_M2")).Text);
            Valor_Total += Convert.ToDouble(((TextBox)Renglon_Grid.Cells[6].FindControl("Txt_Total")).Text);
        }
        Txt_Terreno_Superficie_Total.Text = Superficie_Total.ToString("###,###,###,###,###,##0." + Mascara_Caracteres);
        Txt_Terreno_Valor_Total.Text = Valor_Total.ToString("###,###,###,###,###,##0.00");

        Calcular_Valor_Total_Predio();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Valor_Total_Predio
    ///DESCRIPCIÓN: Cálcula el valor total del predio
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Valor_Total_Predio()
    {
        Double Couta_Anual_Minima = 0; 
        Cls_Cat_Pre_Cuotas_Minimas_Negocio CuentasMin = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        DataTable Cuotas = new DataTable();
        CuentasMin.P_Anio = DateTime.Now.Year.ToString();
        Cuotas = CuentasMin.Consultar_Cuotas_Minimas();
        Couta_Anual_Minima = Convert.ToDouble(Cuotas.Rows[0]["CUOTA"].ToString().Trim());
        Cls_Cat_Cat_Tasas_Negocio Tasas = new Cls_Cat_Cat_Tasas_Negocio();
        Tasas.P_Anio = DateTime.Now.Year.ToString();
        Double Valor_Construccion = 0;
        Double Valor_Terreno = 0;
        Double Valor_Total_Predio = 0;
        Double Cuota_Anual_Actual = 0;
        Double Cuota_Anual_Anterior = 0;
        Double Diferencia_Anual = 0;
        Valor_Construccion = Convert.ToDouble(Txt_Construccion_Valor_Total.Text);
        Valor_Terreno = Convert.ToDouble(Txt_Terreno_Valor_Total.Text);
        Valor_Total_Predio = Valor_Construccion + Valor_Terreno;
        Txt_Valor_Total_Predio.Text = Valor_Total_Predio.ToString("###,###,###,###,##0.00");
        try
        {
            Txt_Cuota_Anual_Actual.Text = (Valor_Total_Predio * (Convert.ToDouble(Tasas.Consultar_Tasa().Rows[0][Cat_Cat_Tasas.Campo_Valor_Rustico].ToString()) / 1000)).ToString("###,###,###,###,##0.00");
            Cuota_Anual_Actual = Convert.ToDouble(Txt_Cuota_Anual_Actual.Text);
            if (Cuota_Anual_Actual < Couta_Anual_Minima)
            {
                Txt_Cuota_Anual_Actual.Text = Couta_Anual_Minima.ToString("###,###,###,###,##0.00");
                Cuota_Anual_Actual = Couta_Anual_Minima;
            }
            else
            {
                Txt_Cuota_Anual_Actual.Text = Cuota_Anual_Actual.ToString("###,###,###,###,##0.00");
            }    
            Cuota_Anual_Anterior = Convert.ToDouble(Txt_Cuota_Anual_Anterior.Text);
            Diferencia_Anual = Cuota_Anual_Actual - Cuota_Anual_Anterior;
            if (Diferencia_Anual < 0)
            {
                this.Txt_Diferencia_Anual.BackColor = System.Drawing.Color.Red;
            }
            if (Diferencia_Anual > 0)
            {
                this.Txt_Diferencia_Anual.BackColor = System.Drawing.Color.White;
            }
            Txt_Diferencia_Anual.Text = Diferencia_Anual.ToString("###,###,###,###,##0.00");
        }
        catch
        {
            throw new Exception("Error al consultar las tasas, falta la tasa para el valor rústico.");
        }
        Calcular_Precio_Avaluo();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Precio_Avaluo
    ///DESCRIPCIÓN: Cálcula el importe del avalúo
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Precio_Avaluo()
    {
        Double Hectareas = 0;
        Double Valor_Menor_Ha = 0;
        Double Valor_Mayor_Ha = 0;
        Double Porcentaje = 0;
        Double Factor = 0;
        Double Importe_Terreno = 0;
        Double Construccion = 0;
        Double Precio_Avaluo = 0;/*
        Hectareas = Convert.ToDouble(Txt_Terreno_Superficie_Total.Text);
        Valor_Menor_Ha = Convert.ToDouble(Hdf_Menos_Ha.Value);
        Valor_Mayor_Ha = Convert.ToDouble(Hdf_No_Avaluo.Value);
        Porcentaje = Convert.ToDouble(Hdf_Porcentaje_Cobro.Value);
        Construccion = Convert.ToDouble(Hdf_Porcentaje_Cobro.Value);
        Factor = Convert.ToDouble(Hdf_Factor_Cobro.Value);
        if (Hectareas > 0)
        {
            Importe_Terreno = Valor_Menor_Ha;
            Hectareas = Hectareas - 1;
            if (Hectareas > 0)
            {
                Importe_Terreno += (Hectareas * Valor_Mayor_Ha);
            }
        }
        Precio_Avaluo = (Importe_Terreno + (Construccion * Factor)) * (Porcentaje / 100);
        //Txt_Precio_Avaluo.Text = Precio_Avaluo.ToString("###,###,###,###,##0.00");*/
    }

    protected void Cmb_Coordenadas_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Coordenadas.SelectedValue == "UTM")
        {
            Div_UTM.Visible = true;
            Div_Cartograficas.Visible = false;
        }
        else if (Cmb_Coordenadas.SelectedValue == "CART")
        {
            Div_Cartograficas.Visible = true;
            Div_UTM.Visible = false;
        }
        else
        {
            Div_Cartograficas.Visible = false;
            Div_UTM.Visible = false;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Elementos_Construccion_DataBound
    ///DESCRIPCIÓN: Carga los datos en los componentes del grid
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Elementos_Construccion_DataBound(object sender, EventArgs e)
    {
        DataTable Dt_Elementos_Construccion = (DataTable)Session["Dt_Grid_Elementos_Construccion"];
        for (int i = 0; i < Dt_Elementos_Construccion.Rows.Count; i++)
        {
            TextBox Txt_A_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[2].FindControl("Txt_A");
            TextBox Txt_B_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[3].FindControl("Txt_B");
            TextBox Txt_C_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[4].FindControl("Txt_C");
            TextBox Txt_D_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[5].FindControl("Txt_D");
            TextBox Txt_E_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[6].FindControl("Txt_E");
            TextBox Txt_F_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[7].FindControl("Txt_F");
            TextBox Txt_G_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[8].FindControl("Txt_G");
            TextBox Txt_H_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[9].FindControl("Txt_H");
            TextBox Txt_I_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[10].FindControl("Txt_I");
            TextBox Txt_J_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[11].FindControl("Txt_J");
            TextBox Txt_K_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[12].FindControl("Txt_K");
            TextBox Txt_L_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[13].FindControl("Txt_L");
            TextBox Txt_M_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[14].FindControl("Txt_M");
            TextBox Txt_N_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[15].FindControl("Txt_N");
            TextBox Txt_O_Temporal = (TextBox)Grid_Elementos_Construccion.Rows[i].Cells[16].FindControl("Txt_O");
            Txt_A_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_A"].ToString();
            Txt_B_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_B"].ToString();
            Txt_C_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_C"].ToString();
            Txt_D_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_D"].ToString();
            Txt_E_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_E"].ToString();
            Txt_F_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_F"].ToString();
            Txt_G_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_G"].ToString();
            Txt_H_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_H"].ToString();
            Txt_I_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_I"].ToString();
            Txt_J_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_J"].ToString();
            Txt_K_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_K"].ToString();
            Txt_L_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_L"].ToString();
            Txt_M_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_M"].ToString();
            Txt_N_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_N"].ToString();
            Txt_O_Temporal.Text = Dt_Elementos_Construccion.Rows[i]["ELEMENTO_CONSTRUCCION_O"].ToString();
        }
    }

    protected void Chk_Indicador_Valor_CheckedChanged(object sender, EventArgs e)
    {
        Double Factor = 1;
        foreach (GridViewRow Grid_Row_Actual in Grid_Descripcion_Terreno.Rows)
        {
            CheckBox Chk_Indicador_Valor_A = (CheckBox)Grid_Row_Actual.Cells[5].FindControl("Chk_Indicador_Valor_A");
            CheckBox Chk_Indicador_Valor_B = (CheckBox)Grid_Row_Actual.Cells[9].FindControl("Chk_Indicador_Valor_B");
            CheckBox Chk_Indicador_Valor_C = (CheckBox)Grid_Row_Actual.Cells[13].FindControl("Chk_Indicador_Valor_C");
            CheckBox Chk_Indicador_Valor_D = (CheckBox)Grid_Row_Actual.Cells[17].FindControl("Chk_Indicador_Valor_D");
            if (Chk_Indicador_Valor_A.Checked)
            {
                Factor = Factor * Convert.ToDouble(Grid_Row_Actual.Cells[3].Text);
            }
            if (Chk_Indicador_Valor_B.Checked)
            {
                Factor = Factor * Convert.ToDouble(Grid_Row_Actual.Cells[7].Text);
            }
            if (Chk_Indicador_Valor_C.Checked)
            {
                Factor = Factor * Convert.ToDouble(Grid_Row_Actual.Cells[11].Text);
            }
            if (Chk_Indicador_Valor_D.Checked)
            {
                Factor = Factor * Convert.ToDouble(Grid_Row_Actual.Cells[15].Text);
            }
        }
        Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
        DataTable Dt_Parametros = Parametros.Consultar_Parametros();
        Crear_Mascara(Convert.ToInt16(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString()));
        foreach (GridViewRow Grid_Row_Actual in Grid_Calculos.Rows)
        {
            TextBox Txt_Superficie_M2_Temporal = (TextBox)Grid_Row_Actual.Cells[2].FindControl("Txt_Superficie_M2");
            TextBox Txt_Valor_M2_Temporal = (TextBox)Grid_Row_Actual.Cells[4].FindControl("Txt_Valor_M2");
            TextBox Txt_Factor_Temporal = (TextBox)Grid_Row_Actual.Cells[5].FindControl("Txt_Factor");
            TextBox Txt_Total_Temporal = (TextBox)Grid_Row_Actual.Cells[6].FindControl("Txt_Total");
            Txt_Factor_Temporal.Text = Factor.ToString("#,###,###,###,###,##0." + Mascara_Caracteres);
            Txt_Total_Temporal.Text = (Convert.ToDouble(Txt_Superficie_M2_Temporal.Text) * Convert.ToDouble(Txt_Valor_M2_Temporal.Text) * Convert.ToDouble(Txt_Factor_Temporal.Text)).ToString("#,###,###,###,###,###,###,##0.00");
        }
        Calcular_Totales_Terreno();

        foreach (GridViewRow Grid_Row_Actual in Grid_Valores_Construccion.Rows)
        {
            TextBox Txt_Superficie_M2_Temporal = (TextBox)Grid_Row_Actual.Cells[3].FindControl("Txt_Superficie_M2");
            TextBox Txt_Valor_M2_Temporal = (TextBox)Grid_Row_Actual.Cells[4].FindControl("Txt_Valor_X_M2");
            TextBox Txt_Factor_Temporal = (TextBox)Grid_Row_Actual.Cells[8].FindControl("Txt_Factor");
            TextBox Txt_Total_Temporal = (TextBox)Grid_Row_Actual.Cells[6].FindControl("Txt_Total");
            Txt_Factor_Temporal.Text = (1).ToString("#,###,###,###,###,##0.00");
            Txt_Total_Temporal.Text = (Convert.ToDouble(Txt_Superficie_M2_Temporal.Text) * Convert.ToDouble(Txt_Valor_M2_Temporal.Text) * Convert.ToDouble(Txt_Factor_Temporal.Text)).ToString("#,###,###,###,###,###,###,##0.00");
        }
        Calcular_Totales_Construccion();
    }


    protected void Txt_X_Segundos_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_X_Segundos.Text.Trim() == "")
            {
                Txt_X_Segundos.Text = "";
            }
            else
            {
                Txt_X_Segundos.Text = Convert.ToDouble(Txt_X_Segundos.Text).ToString("##0.00");
            }
        }
        catch (Exception Exc)
        {
            Txt_X_Segundos.Text = "";
        }
    }

    protected void Txt_Y_Segundos_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Y_Segundos.Text.Trim() == "")
            {
                Txt_Y_Segundos.Text = "";
            }
            else
            {
                Txt_Y_Segundos.Text = Convert.ToDouble(Txt_Y_Segundos.Text).ToString("##0.00");
            }
        }
        catch (Exception Exc)
        {
            Txt_Y_Segundos.Text = "";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del botón salir
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
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
            Limpiar_Formulario();
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Imprimir.Visible = true;
            Configuracion_Formulario(true);
            Llenar_Tabla_Avaluos_Urbanos(Grid_Avaluos_Urbanos.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Avaluos_Urbanos.SelectedIndex = -1;
            Div_Datos_Avaluo.Visible = false;
            Div_Grid_Avaluo.Visible = true;
            Div_Observaciones.Visible = false;
            Session["Dt_Grid_Valores_Construccion"] = null;
            Session["Dt_Grid_Calculos"] = null;
            Session["Dt_Grid_Elementos_Construccion"] = null;
            Session["Dt_Grid_Valores_Construccion"] = null;
            Session["Anio"] = null;
            Session["Dt_Tabla_Valores_Construccion"] = null;
            Session["Dt_Motivos_Rechazo"] = null;
            Session["Dt_Documentos"] = null;
            Llenar_Cuentas_Asignadas(0);
            Btn_Imprimir_Cuentas.Visible = true;
            Grid_Cuentas_Asignadas.SelectedIndex = -1;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Guardar_Grid_Calculos
    ///DESCRIPCIÓN: Llena la sesión con los datos a guardar en la BD's
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Guardar_Grid_Calculos()
    {
        Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
        DataTable Dt_Calculos = (DataTable)Session["Dt_Grid_Calculos"];
        for (int i = 0; i < Dt_Calculos.Rows.Count; i++)
        {
            TextBox Txt_Superficie_M2_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[2].FindControl("Txt_Superficie_M2");
            TextBox Txt_Factor_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[5].FindControl("Txt_Factor");
            TextBox Txt_Total_Temporal = (TextBox)Grid_Calculos.Rows[i].Cells[6].FindControl("Txt_Total");
            Dt_Calculos.Rows[i]["SUPERFICIE"] = Convert.ToDouble(Txt_Superficie_M2_Temporal.Text);
            Dt_Calculos.Rows[i]["FACTOR"] = Convert.ToDouble(Txt_Factor_Temporal.Text);
            Dt_Calculos.Rows[i]["VALOR_PARCIAL"] = Convert.ToDouble(Txt_Total_Temporal.Text);
        }
    }

    protected void Guardar_Grid_Valores_Construccion()
    {
        DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
        for (int i = 0; i < Dt_Valores_Construccion.Rows.Count; i++)
        {
            TextBox Txt_Croquis = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[0].FindControl("Txt_Croquis");
            TextBox Txt_Superficie_M2_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[3].FindControl("Txt_Superficie_M2");
            TextBox Txt_Factor_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[8].FindControl("Txt_Factor");
            TextBox Txt_Edad = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[7].FindControl("Txt_Edad");
            TextBox Txt_Total_Temporal = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[6].FindControl("Txt_Total");
            TextBox Txt_Uso = (TextBox)Grid_Valores_Construccion.Rows[i].Cells[9].FindControl("Txt_Uso");
            Dt_Valores_Construccion.Rows[i]["CROQUIS"] = Txt_Croquis.Text.ToUpper();
            Dt_Valores_Construccion.Rows[i]["SUPERFICIE_M2"] = Convert.ToDouble(Txt_Superficie_M2_Temporal.Text);
            Dt_Valores_Construccion.Rows[i]["FACTOR"] = Convert.ToDouble(Txt_Factor_Temporal.Text);
            Dt_Valores_Construccion.Rows[i]["VALOR_PARCIAL"] = Convert.ToDouble(Txt_Total_Temporal.Text);
            Dt_Valores_Construccion.Rows[i]["EDAD_CONSTRU"] = "0";
            Dt_Valores_Construccion.Rows[i]["USO_CONTRU"] = "";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calles_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cuentas_Asignadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Cuentas_Asignadas(e.NewPageIndex);
    }

    protected void Grid_Cuentas_Asignadas_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Cuentas_Asignadas.SelectedIndex > -1)
        {
            Hdf_Cuenta_Predial_Id.Value = Grid_Cuentas_Asignadas.SelectedRow.Cells[4].Text;
            Txt_Cuenta_Predial.Text = Grid_Cuentas_Asignadas.SelectedRow.Cells[5].Text;
            //Hdf_Perito_Interno_Id.Value = Grid_Cuentas_Asignadas.SelectedRow.Cells[2].Text;
            //Txt_Perito_Interno.Text = Grid_Cuentas_Asignadas.SelectedRow.Cells[3].Text;
            Hdf_No_Asignacion.Value = Grid_Cuentas_Asignadas.SelectedRow.Cells[1].Text;
            Btn_Salir.AlternateText = "Atras";
            Div_Grid_Avaluo.Visible = false;
            Div_Datos_Avaluo.Visible = true;
            Div_Observaciones.Visible = false;
            Cargar_Datos();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Cuentas_Asignadas(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
            DataTable Dt_Cuentas_Asignadas;
            Cls_Cat_Cat_Peritos_Internos_Negocio Perito = new Cls_Cat_Cat_Peritos_Internos_Negocio();
            Perito.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
            Dt_Cuentas_Asignadas = Perito.Consultar_Peritos_Internos();
            if (Dt_Cuentas_Asignadas.Rows.Count > 0)
            {
                Cuentas.P_Perito_Interno_Id = Dt_Cuentas_Asignadas.Rows[0][Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id].ToString();
                Cuentas.P_Tipo_Predio = "RUSTICO";
                if (Cmb_Busqueda.SelectedValue == "COLONIA")
                {
                    Cuentas.P_Colonia = Txt_Busqueda_Asignadas.Text.ToUpper();
                }
                else if (Cmb_Busqueda.SelectedValue == "CALLE")
                {
                    Cuentas.P_Calle = Txt_Busqueda_Asignadas.Text.ToUpper();
                }
                else if (Cmb_Busqueda.SelectedValue == "PROPIETARIO")
                {
                    Cuentas.P_Propietario = Txt_Busqueda_Asignadas.Text.ToUpper();
                }
                else if (Cmb_Busqueda.SelectedValue == "CUENTA_PREDIAL")
                {
                    Cuentas.P_Cuenta_Predial = Txt_Busqueda_Asignadas.Text.ToUpper();
                }
                Cuentas.P_Avaluo = true;
                Dt_Cuentas_Asignadas = Cuentas.Consultar_Cuentas_Asignadas();
                Grid_Cuentas_Asignadas.Columns[1].Visible = true;
                Grid_Cuentas_Asignadas.Columns[2].Visible = true;
                Grid_Cuentas_Asignadas.Columns[4].Visible = true;
                Grid_Cuentas_Asignadas.DataSource = Dt_Cuentas_Asignadas;
                Grid_Cuentas_Asignadas.PageIndex = Pagina;
                Grid_Cuentas_Asignadas.DataBind();
                Grid_Cuentas_Asignadas.Columns[1].Visible = false;
                Grid_Cuentas_Asignadas.Columns[2].Visible = false;
                Grid_Cuentas_Asignadas.Columns[4].Visible = false;
            }
            else
            {
                Grid_Cuentas_Asignadas.Columns[1].Visible = true;
                Grid_Cuentas_Asignadas.Columns[2].Visible = true;
                Grid_Cuentas_Asignadas.Columns[4].Visible = true;
                Grid_Cuentas_Asignadas.DataSource = null;
                Grid_Cuentas_Asignadas.PageIndex = 0;
                Grid_Cuentas_Asignadas.DataBind();
                Grid_Cuentas_Asignadas.Columns[1].Visible = false;
                Grid_Cuentas_Asignadas.Columns[2].Visible = false;
                Grid_Cuentas_Asignadas.Columns[4].Visible = false;
            }
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Asignadas_Click
    ///DESCRIPCIÓN: Evento del botón buscar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Asignadas_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Busqueda_Asignadas.Text.Trim() != "")
        {
            Llenar_Cuentas_Asignadas(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Agrega el documento al grid.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 04/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Documento_Click(object sender, ImageClickEventArgs e)
    {
        if (Fup_Documento.FileName.Trim() != "")
        {
            DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
            Boolean Entro = false;
            foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
            {
                if (Dr_Renglon["DOCUMENTO"].ToString() == Cmb_Documento.SelectedValue && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Entro = true;
                    break;
                }
            }
            if (!Entro)
            {
                DataRow Dr_Nuevo = Dt_Documentos.NewRow();
                Dr_Nuevo["NO_DOCUMENTO"] = " ";
                Dr_Nuevo["ACCION"] = "ALTA";
                Dr_Nuevo["EXTENSION_ARCHIVO"] = Path.GetExtension(Fup_Documento.FileName).ToLower();
                Dr_Nuevo["DOCUMENTO"] = Cmb_Documento.SelectedValue;
                Dr_Nuevo["BITS_ARCHIVO"] = Fup_Documento.FileBytes;
                Dr_Nuevo["ANIO_DOCUMENTO"] = DateTime.Now.Year;
                Dr_Nuevo["RUTA_DOCUMENTO"] = Cmb_Documento.SelectedValue.Replace(' ', '_') + Path.GetExtension(Fup_Documento.FileName).ToLower();
                Dt_Documentos.Rows.Add(Dr_Nuevo);
                Dt_Documentos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Grid_Documentos.Columns[0].Visible = true;
                Grid_Documentos.DataSource = Dt_Documentos;
                Grid_Documentos.DataBind();
                Grid_Documentos.Columns[0].Visible = false;
                Grid_Colindancias.SelectedIndex = -1;
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_DataBound
    ///DESCRIPCIÓN: Carga los componentes del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Documentos_DataBound(object sender, EventArgs e)
    {
        Int16 i = 0;
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "NADA")
            {
                //Label Lbl_Url_Temporal = (Label)Grid_Documentos.Rows[i].Cells[3].FindControl("Lbl_Url");
                if (File.Exists(Server.MapPath(Dr_Renglon["RUTA_DOCUMENTO"].ToString())))
                {
                    HyperLink Hlk_Enlace = new HyperLink();
                    Hlk_Enlace.Text = Path.GetFileName(Dr_Renglon["RUTA_DOCUMENTO"].ToString());
                    Hlk_Enlace.NavigateUrl = Dr_Renglon["RUTA_DOCUMENTO"].ToString();
                    Hlk_Enlace.CssClass = "enlace_fotografia";
                    Hlk_Enlace.Target = "blank";
                    //e.Row.Cells[3].Controls.Add(Hlk_Enlace);
                    Grid_Documentos.Rows[i].Cells[3].Controls.Add(Hlk_Enlace);
                    i++;
                }
            }
            else if (Dr_Renglon["ACCION"].ToString() == "ALTA")
            {
                Label Lbl_Guardar = new Label();
                Lbl_Guardar.Text = "Guardar para poder visualizar este archivo";
                Grid_Documentos.Rows[i].Cells[3].Controls.Add(Lbl_Guardar);
                i++;
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_SelectedIndexChanged
    ///DESCRIPCIÓN: Cambia la acción a BAJA para eliminarlo del sistema.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 04/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Documentos_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Eliminar registro y archivo en caso de tenerlo
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        foreach (DataRow Dr_Documento in Dt_Documentos.Rows)
        {
            if (Dr_Documento["DOCUMENTO"].ToString() == Grid_Documentos.SelectedRow.Cells[2].Text && Dr_Documento["ACCION"].ToString() != "BAJA")
            {
                Dr_Documento["ACCION"] = "BAJA";
                break;
            }
        }
        Dt_Documentos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
        Grid_Documentos.Columns[0].Visible = true;
        Grid_Documentos.Columns[1].Visible = true;
        Grid_Documentos.DataSource = Dt_Documentos;
        Grid_Documentos.PageIndex = 0;
        Grid_Documentos.DataBind();
        Grid_Documentos.Columns[0].Visible = false;
        Grid_Documentos.Columns[1].Visible = false;
        Grid_Documentos.SelectedIndex = -1;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Guardar_Imagenes
    ///DESCRIPCIÓN: Crea las imagenes en la carpeta del perito para poder tener sus documentos dentro del sistema
    ///PROPIEDADES:     Dt_Documentos:      Tabla que contiene todos los datos para ser creados como imagenes.
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Guardar_Imagenes(DataTable Dt_Documentos)
    {
        if (!Directory.Exists(Server.MapPath("../Catastro/Archivos_Arv/" + Hdf_Anio_Avaluo.Value + "_" + Hdf_No_Avaluo.Value + "/")))
        {
            Directory.CreateDirectory(Server.MapPath("../Catastro/Archivos_Arv/" + Hdf_Anio_Avaluo.Value + "_" + Hdf_No_Avaluo.Value + "/"));
        }
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "ALTA")
            {
                //crear filestream y binarywriter para guardar archivo
                FileStream Escribir_Archivo = new FileStream(Server.MapPath("../Catastro/Archivos_Arv/" + Hdf_Anio_Avaluo.Value + "_" + Hdf_No_Avaluo.Value + "/" + Dr_Renglon["RUTA_DOCUMENTO"].ToString()), FileMode.Create, FileAccess.Write);
                BinaryWriter Datos_Archivo = new BinaryWriter(Escribir_Archivo);
                Datos_Archivo.Write((Byte[])Dr_Renglon["BITS_ARCHIVO"]);
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Eliminar_Imagenes
    ///DESCRIPCIÓN: Elimina las imagenes en la carpeta del perito
    ///PROPIEDADES:     Dt_Documentos:      Tabla que contiene todos los datos para ser Eliminados de la carpeta del perito.
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 06/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Eliminar_Imagenes(DataTable Dt_Documentos)
    {
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["NO_DOCUMENTO"].ToString().Trim().Replace("&nbsp;", "") != "")
            {
                //Elimina el archivo con la ruta asignadaen la columna RUTA_DOCUMENTO
                File.Delete(Server.MapPath(Dr_Renglon["RUTA_DOCUMENTO"].ToString()));
            }
        }
    }


    protected void Btn_Imprimir_Cuentas_Click(object sender, ImageClickEventArgs e)
    {
        Imprimir_Reporte(Crear_Ds_Cuentas_Asignadas(), "Rpt_Ope_Cat_Asignacion_Cuentas.rpt", "Cuentas_Asignadas", "Window_Frm", "Cuentas_Asignadas");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Avaluo_Urbano
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte del avalúo urbano
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Cuentas_Asignadas()
    {
        Ds_Ope_Cat_Cuentas_Asignadas Ds_Cuentas_Asignadas = new Ds_Ope_Cat_Cuentas_Asignadas();
        DataTable Dt_Cuentas;
        Dt_Cuentas = Ds_Cuentas_Asignadas.Tables["DT_CUENTAS_ASIGNADAS"];


        Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
            DataTable Dt_Cuentas_Asignadas;
            Cls_Cat_Cat_Peritos_Internos_Negocio Perito = new Cls_Cat_Cat_Peritos_Internos_Negocio();
            Perito.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
            Dt_Cuentas_Asignadas = Perito.Consultar_Peritos_Internos();
            if (Dt_Cuentas_Asignadas.Rows.Count > 0)
            {
                Cuentas.P_Perito_Interno_Id = Dt_Cuentas_Asignadas.Rows[0][Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id].ToString();
                Cuentas.P_Tipo_Predio = "RUSTICO";
                Cuentas.P_Avaluo = true;
                Dt_Cuentas_Asignadas = Cuentas.Consultar_Cuentas_Asignadas();
                DataRow Dr_Renglon_Nuevo;
                foreach (DataRow Dr_Renglon_Actual in Dt_Cuentas_Asignadas.Rows)
                {
                    Dr_Renglon_Nuevo = Dt_Cuentas.NewRow();
                    Dr_Renglon_Nuevo["CUENTA_PREDIAL"] = Dr_Renglon_Actual["CUENTA_PREDIAL"];
                    Dr_Renglon_Nuevo["NOMBRE_COLONIA"] = Dr_Renglon_Actual["NOMBRE_COLONIA"];
                    Dr_Renglon_Nuevo["NOMBRE_CALLE"] = Dr_Renglon_Actual["NOMBRE_CALLE"];
                    Dr_Renglon_Nuevo["NO_EXTERIOR"] = Dr_Renglon_Actual["NO_EXTERIOR"];
                    Dr_Renglon_Nuevo["NO_INTERIOR"] = Dr_Renglon_Actual["NO_INTERIOR"];
                    Dr_Renglon_Nuevo["EFECTOS"] = Dr_Renglon_Actual["EFECTOS"];
                    Dr_Renglon_Nuevo["SUPERFICIE_CONSTRUIDA"] = Dr_Renglon_Actual["SUPERFICIE_CONSTRUIDA"];
                    Dr_Renglon_Nuevo["SUPERFICIE_TOTAL"] = Dr_Renglon_Actual["SUPERFICIE_TOTAL"];
                    Dr_Renglon_Nuevo["AGRUPACION"] = "A";
                    Dt_Cuentas.Rows.Add(Dr_Renglon_Nuevo);
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Usted no es un perito interno vigente.";
                Lbl_Ecabezado_Mensaje.Visible = true;
                Div_Contenedor_Msj_Error.Visible = true;
            }
        return Ds_Cuentas_Asignadas;
    }
}
