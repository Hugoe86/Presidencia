using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Operacion_Cat_Avaluo_Rustico_Inconformidades.Negocio;
using System.Data;
using Presidencia.Catalogo_Cat_Motivos_Avaluo.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Cat_Parametros.Negocio;
using Presidencia.Catalogo_Cat_Tabla_Factores.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Registro_Peticion.Datos;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using Presidencia.Numalet;
using System.IO;
using Presidencia.Catalogo_Cat_Peritos_Internos.Negocio;
using Presidencia.Operacion_Cat_Avaluo_Urbano_Av.Negocio;
using Presidencia.Operacion_Cat_Avaluo_Rustico_Autorizacion_Valor.Negocio;
using Presidencia.Catalogo_Cat_Identificadores_Predio.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Operacion_Cat_Recepcion_Oficios.Negocio;
using Presidencia.Catalogo_Cat_Tipos_Construccion.Negocio;

public partial class paginas_Catastro_Frm_Ope_Cat_Avaluo_Rustico_Inconformidades : System.Web.UI.Page
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
                Llenar_Tabla_Avaluos_Urbanos(0);
                Llenar_Combo_Motivos_Avaluo();
                Llenar_Combo_Tipos_Construccion();
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
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
        Cmb_Motivo_Avaluo.Enabled = !Enabled;
        Txt_No_Avaluo.Enabled = false;
        Txt_Solicitante.Enabled = !Enabled;
        Txt_Nombre_Predio.Enabled = !Enabled;
        Txt_Observaciones.Enabled = !Enabled;
        Txt_X_Horas.Enabled = !Enabled;
        Txt_X_Minutos.Enabled = !Enabled;
        Txt_X_Segundos.Enabled = !Enabled;
        Cmb_Latitud.Enabled = !Enabled;
        Txt_Y_Horas.Enabled = !Enabled;
        Txt_Y_Minutos.Enabled = !Enabled;
        Txt_Y_Segundos.Enabled = !Enabled;
        Cmb_Longitud.Enabled = !Enabled;
        Txt_Valor_Total_Predio.Enabled = false;
        Txt_Terreno_Superficie_Total.Enabled = false;
        Txt_Terreno_Valor_Total.Enabled = false;
        Txt_Construccion_Superficie_Total.Enabled = false;
        Txt_Construccion_Valor_Total.Enabled = false;
        Cmb_Estatus.Enabled = false;
        Txt_Busqueda.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
        Grid_Avaluos_Urbanos_Inconformidades.Enabled = Enabled;
        Grid_Calculos.Enabled = !Enabled;
        Grid_Descripcion_Terreno.Enabled = !Enabled;
        Grid_Elementos_Construccion.Enabled = !Enabled;
        Grid_Valores_Construccion.Enabled = !Enabled;
        Txt_Terreno_Superficie_Total.Style["text-align"] = "Right";
        Txt_Terreno_Valor_Total.Style["text-align"] = "Right";
        Txt_Construccion_Superficie_Total.Style["text-align"] = "Right";
        Txt_Construccion_Valor_Total.Style["text-align"] = "Right";
        Txt_Valor_Total_Predio.Style["text-align"] = "Right";
        Txt_Uso_Constru.Enabled = !Enabled;
        Txt_Coordenadas_UTM.Enabled = !Enabled;
        Txt_Coordenadas_UTM_Y.Enabled = !Enabled;
        Cmb_Coordenadas.Enabled = !Enabled;
        Btn_Busqueda_Avaluos_Av.Enabled = false;
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
        Txt_No_Avaluo.Text = "";
        Txt_Cuenta_Predial.Text = "";
        Hdf_Cuenta_Predial_Id.Value = "";
        Txt_Propietario.Text = "";
        Txt_Solicitante.Text = "";
        Txt_Clave_Catastral.Text = "";
        Txt_Domicilio_Not.Text = "";
        Txt_Municipio_Notificar.Text = "";
        Txt_Ubicacion_Predio.Text = "";
        Txt_Localidad.Text = "";
        Txt_Nombre_Predio.Text = "";
        Txt_Observaciones.Text = "";
        Txt_X_Horas.Text = "";
        Txt_X_Minutos.Text = "";
        Txt_X_Segundos.Text = "";
        Cmb_Latitud.SelectedIndex = 0;
        Txt_Y_Horas.Text = "";
        Txt_Y_Minutos.Text = "";
        Txt_Y_Segundos.Text = "";
        Cmb_Longitud.SelectedIndex = 0;
        Txt_Valor_Total_Predio.Text = "0.00";
        Txt_Terreno_Superficie_Total.Text = "0.00";
        Txt_Terreno_Valor_Total.Text = "0.00";
        Txt_Construccion_Superficie_Total.Text = "0.00";
        Txt_Construccion_Valor_Total.Text = "0.00";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Busqueda.Text = "";
        Cmb_Tipo_Construccion.SelectedIndex = 0;
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
            Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio Avaluo_Urb = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Avaluo_Urb.P_Folio = Txt_Busqueda.Text.Trim();
            }
            //Cls_Cat_Cat_Peritos_Internos_Negocio Perito_Interno = new Cls_Cat_Cat_Peritos_Internos_Negocio();
            //Perito_Interno.P_Empleado_Id= Cls_Sessiones.Empleado_ID;
            //DataTable Dt_Perito_Interno = Perito_Interno.Consultar_Peritos_Internos();
            //Avaluo_Urb.P_Perito_Externo_Id = Dt_Perito_Interno.Rows[0]["PERITO_INTERNO_ID"].ToString();
            Grid_Avaluos_Urbanos_Inconformidades.Columns[1].Visible = true;
            Grid_Avaluos_Urbanos_Inconformidades.Columns[2].Visible = true;
            Grid_Avaluos_Urbanos_Inconformidades.DataSource = Avaluo_Urb.Consultar_Avaluo_Rustico();
            Grid_Avaluos_Urbanos_Inconformidades.PageIndex = Pagina;
            Grid_Avaluos_Urbanos_Inconformidades.DataBind();
            Grid_Avaluos_Urbanos_Inconformidades.Columns[1].Visible = false;
            Grid_Avaluos_Urbanos_Inconformidades.Columns[2].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
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
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Cls_Cat_Cat_Parametros_Negocio Par = new Cls_Cat_Cat_Parametros_Negocio();
                Int16 columnas = Convert.ToInt16(Par.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Columnas_Calc_Construccion].ToString());
                //DataTable Dt_Valores;
                Configuracion_Formulario(false);
                Limpiar_Formulario();
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
                Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();
                Avaluo.P_Anio_Avaluo = DateTime.Now.Year.ToString();
                DataTable Dt_Elementos_Construccion = Avaluo.Consultar_Tabla_Elementos_Construccion_Inconformidades();
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
                if (Dt_Factores_Cobro.Rows.Count > 0)
                {
                    Hdf_Factor_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_2].ToString();
                    Hdf_Mayor_Ha.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Mayor_1_Ha].ToString();
                    Hdf_Menos_Ha.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Menor_1_Ha].ToString();
                    Hdf_Porcentaje_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Porcentaje_PE].ToString();
                }
                else
                {
                    Hdf_Factor_Cobro.Value = "0.00";
                    Hdf_Mayor_Ha.Value = "0.00";
                    Hdf_Menos_Ha.Value = "0.00";
                    Hdf_Porcentaje_Cobro.Value = "0.00";
                }
               
                Cmb_Estatus.SelectedValue = "POR VALIDAR";
                DataTable Dt_Medidas = new DataTable();
                Dt_Medidas.Columns.Add(Ope_Cat_Colindancias_Ari.Campo_No_Colindancia, typeof(String));
                Dt_Medidas.Columns.Add(Ope_Cat_Colindancias_Ari.Campo_Medida_Colindancia, typeof(String));
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
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();
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
                    if (Cmb_Tipo_Construccion.SelectedValue == "OTRO")
                    {
                        Avaluo.P_Uso_Constru = Txt_Uso_Constru.Text.ToUpper();
                    }
                    else
                    {
                        Avaluo.P_Uso_Constru = Cmb_Tipo_Construccion.SelectedValue;
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
                    Cls_Cat_Cat_Peritos_Internos_Negocio Perito_Interno = new Cls_Cat_Cat_Peritos_Internos_Negocio();
                    Perito_Interno.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
                    Perito_Interno.P_Estatus = "= 'VIGENTE'";
                    DataTable Dt_Perito_Interno = Perito_Interno.Consultar_Peritos_Internos();
                    Avaluo.P_Perito_Interno_Id = Dt_Perito_Interno.Rows[0]["PERITO_INTERNO_ID"].ToString();
                    Avaluo.P_Nombre_Predio = Txt_Nombre_Predio.Text.ToUpper();
                    Avaluo.P_Solicitud_Id = Hdf_Solicitud_Id.Value;
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
                    if ((Avaluo.Alta_Avaluo_Rustico()))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico", "alert('Alta Exitosa');", true);
                        Hdf_No_Avaluo.Value = Avaluo.P_No_Avaluo;
                        Hdf_Anio_Avaluo.Value = Avaluo.P_Anio_Avaluo;
                        Guardar_Imagenes(Avaluo.P_Dt_Documentos);
                        Btn_Salir_Click(null, null);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico", "alert('Alta Errónea');", true);
                    }
                }
            }
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message;
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
            if (Grid_Avaluos_Urbanos_Inconformidades.SelectedIndex > -1)
            {
                if (Grid_Avaluos_Urbanos_Inconformidades.SelectedRow.Cells[8].Text == "POR VALIDAR" || Grid_Avaluos_Urbanos_Inconformidades.SelectedRow.Cells[8].Text == "RECHAZADO")
                {
                    if (Btn_Modificar.AlternateText.Equals("Modificar"))
                    {
                        if (Grid_Avaluos_Urbanos_Inconformidades.SelectedRow.Cells[8].Text == "PAGADO")
                        {
                            Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();
                            Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
                            Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
                            DateTime DaTi = Convert.ToDateTime(Avaluo.Consultar_Avaluo_Rustico().Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Fecha_Autorizo].ToString());
                            DateTime DaTi_New = DaTi.AddDays(30);
                            if (DateTime.Compare(DaTi, DaTi_New) == 1)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano", "alert('No es posible actualizar el Avalúo');", true);
                                return;
                            }
                            //Hdf_Cobro_Anterior.Value = Txt_Precio_Avaluo.Text;
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
                    }
                    else
                    {
                        if (Validar_Componentes())
                        {
                            Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();
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
                            if (Cmb_Tipo_Construccion.SelectedValue == "OTRO")
                            {
                                Avaluo.P_Uso_Constru = Txt_Uso_Constru.Text.ToUpper();
                            }
                            else
                            {
                                Avaluo.P_Uso_Constru = Cmb_Tipo_Construccion.SelectedValue;
                            }
                            Avaluo.P_Valor_Total_Predio = Txt_Valor_Total_Predio.Text.Replace(",", "");
                            Avaluo.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                            Avaluo.P_Solicitud_Id = Hdf_Solicitud_Id.Value;
                            Avaluo.P_Perito_Interno_Id = Cls_Sessiones.Empleado_ID;
                            Avaluo.P_Nombre_Predio = Txt_Nombre_Predio.Text.ToUpper();
                            Avaluo.P_Estatus = Cmb_Estatus.SelectedValue;
                            Avaluo.P_Veces_Rechazo = Grid_Avaluos_Urbanos_Inconformidades.SelectedRow.Cells[4].Text;
                            //Guardar Dt's
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
                            if ((Avaluo.Modificar_Avaluo_Rustico()))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico Inconformidades", "alert('Actualizacion Exitosa');", true);
                                Hdf_No_Avaluo.Value = Avaluo.P_No_Avaluo;
                                Hdf_Anio_Avaluo.Value = Avaluo.P_Anio_Avaluo;
                                Eliminar_Imagenes(Avaluo.P_Dt_Documentos);
                                Guardar_Imagenes(Avaluo.P_Dt_Documentos);
                                Btn_Salir_Click(null, null);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano Inconformidades", "alert('Actualización Errónea');", true);
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
        Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();
        Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Interno = new Cls_Cat_Cat_Peritos_Externos_Negocio();
        Ds_Ope_Cat_Folio_Pago_Avaluo_Rustico Folio_Avaluo = new Ds_Ope_Cat_Folio_Pago_Avaluo_Rustico();
        Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
        Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
        DataTable Dt_Avaluo = Avaluo.Consultar_Avaluo_Rustico();
        Perito_Interno.P_Perito_Externo_Id = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Perito_Externo_Id].ToString();
        //String Cantidad_Cobro = "";
        //if ((Convert.ToDouble(Txt_Precio_Avaluo.Text) - Convert.ToDouble(Hdf_Cobro_Anterior.Value) > 0))
        //{
        //    Cantidad_Cobro = (Convert.ToDouble(Txt_Precio_Avaluo.Text) - Convert.ToDouble(Hdf_Cobro_Anterior.Value)).ToString("0.00");
        //}
        //else
        //{
        //    Cantidad_Cobro = "0.00";
        //}
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
        //Dr_Renglon_Nuevo["CANTIDAD"] = Convert.ToDouble(Cantidad_Cobro);
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
        String Ruta_Archivo = "";
        Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();
        Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Externo = new Cls_Cat_Cat_Peritos_Externos_Negocio();

        DataTable Dt_Perito_Externo;

        Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
        Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;

        DataTable Dt_Avaluo = Avaluo.Consultar_Avaluo_Rustico();
        Ds_Ope_Cat_Avaluo_Rustico Ds_Avaluo_Urbano = new Ds_Ope_Cat_Avaluo_Rustico();

        Perito_Externo.P_Perito_Externo_Id = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Perito_Externo_Id].ToString();
        Dt_Perito_Externo = Perito_Externo.Consultar_Peritos_Externos();
        if (Dt_Perito_Externo.Rows.Count > 0)
        {
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Nombre].ToString().Trim() != "")
                Valuador += Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Nombre].ToString() + " ";
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno].ToString().Trim() != "")
                Valuador += Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno].ToString() + " ";
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Materno].ToString().Trim() != "")
                Valuador += Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Materno].ToString() + " ";
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Materno].ToString().Trim() != "" || Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Materno].ToString().Trim() != "")
            {
                Valuador += "(";
                if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Calle].ToString().Trim() != "")
                    Valuador += Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Calle].ToString() + " ";
                Valuador += ") ";
                if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Colonia].ToString().Trim() != "")
                    Valuador += Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Colonia].ToString() + " ";
            }
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Telefono].ToString().Trim() != "")
                Valuador += "TEL. " + Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Telefono].ToString();
        }

        DataTable Dt_Datos_Generales = Ds_Avaluo_Urbano.Tables["DT_DATOS_GENERALES"];
        DataRow Dr_Avaluo;
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
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
        if (!String.IsNullOrEmpty(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Fecha_Autorizo].ToString()))
            Dr_Avaluo["FECHA"] = Convert.ToDateTime(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Fecha_Autorizo].ToString());
        Dr_Avaluo["NOMBRE_PREDIO"] = Txt_Nombre_Predio.Text;
        if (Cmb_Coordenadas.SelectedIndex == 1)
        {
            Dr_Avaluo["COORD_X"] = Txt_X_Horas.Text + "°" + Txt_X_Minutos.Text + "'" + Txt_X_Segundos.Text + "'' " + Cmb_Latitud.SelectedValue;
            Dr_Avaluo["COORD_Y"] = Txt_Y_Horas.Text + "°" + Txt_Y_Minutos.Text + "'" + Txt_Y_Segundos.Text + "'' " + Cmb_Longitud.SelectedValue;
        }
        else
        {
            Dr_Avaluo["COORD_X"] = Txt_Coordenadas_UTM.Text.ToUpper();
            Dr_Avaluo["COORD_Y"] = Txt_Coordenadas_UTM.Text.ToUpper();
        }
        Dr_Avaluo["BASE_GRAVABLE"] = "";
        Dr_Avaluo["IMPUESTO_BIMESTRAL"] = 0.00;
        Dr_Avaluo["VALOR_TOTAL_PREDIO"] = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
        Dr_Avaluo["NORTE"] = "";
        Dr_Avaluo["SUR"] = "";
        Dr_Avaluo["ORIENTE"] = "";
        Dr_Avaluo["PONIENTE"] = "";
        Dr_Avaluo["VALUADOR"] = Valuador;
        Dr_Avaluo["NO_VALUADOR"] = Convert.ToInt16(Dt_Perito_Externo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Perito_Externo_Id].ToString());
        Dr_Avaluo["OBSERVACIONES"] = Txt_Observaciones.Text;
        Dr_Avaluo["FOLIO"] = "AR" + Txt_No_Avaluo.Text;
        foreach (DataRow Dr_Renglon_Actual in Dt_Documentos.Rows)
        {
            if (Dr_Renglon_Actual[Ope_Cat_Documentos_Ari.Campo_Documento].ToString().Trim() == "ANEXO_1")
            {
                Ruta_Archivo = Server.MapPath(Dr_Renglon_Actual[Ope_Cat_Documentos_Ari.Campo_Ruta_Documento].ToString());
                if (File.Exists(Ruta_Archivo))
                    Dr_Avaluo["ANEXO_1"] = Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image.FromFile(Server.MapPath(Dr_Renglon_Actual[Ope_Cat_Documentos_Ari.Campo_Ruta_Documento].ToString())));
            }
            else if (Dr_Renglon_Actual[Ope_Cat_Documentos_Arr.Campo_Documento].ToString().Trim() == "ANEXO_2")
            {
                Ruta_Archivo = Server.MapPath(Dr_Renglon_Actual[Ope_Cat_Documentos_Ari.Campo_Ruta_Documento].ToString());
                if(File.Exists(Ruta_Archivo))
                    Dr_Avaluo["ANEXO_2"] = Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image.FromFile(Server.MapPath(Dr_Renglon_Actual[Ope_Cat_Documentos_Ari.Campo_Ruta_Documento].ToString()))); ;
            }
        }
        Dt_Datos_Generales.Rows.Add(Dr_Avaluo);
        DataTable Dt_Terreno = Ds_Avaluo_Urbano.Tables["DT_TERRENO"];
        DataTable Dt_Calculos = (DataTable)Session["Dt_Grid_Calculos"];
        foreach (DataRow Dr_Renglon in Dt_Calculos.Rows)
        {
            Dr_Avaluo = Dt_Terreno.NewRow();
            Dr_Avaluo["CLASIFICACION"] = Dr_Renglon[Cat_Cat_Tipos_Constru_Rustico.Campo_Identificador].ToString();
            Dr_Avaluo["SUPERFICIE_HA"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Superficie].ToString());
            Dr_Avaluo["VALOR_HA"] = Convert.ToDouble(Dr_Renglon[Cat_Cat_Tab_Val_Const_Rustico.Campo_Valor_M2].ToString());
            Dr_Avaluo["FACTOR"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Factor].ToString());
            Dr_Avaluo["VALOR_PARCIAL"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Parcial].ToString());
            Dr_Avaluo["GRUPO"] = "A";
            Dt_Terreno.Rows.Add(Dr_Avaluo);
        }

        DataTable Dt_Construccion = Ds_Avaluo_Urbano.Tables["DT_CONSTRUCCION"];
        DataTable Dt_Valores_Construccion = (DataTable)Session["Dt_Grid_Valores_Construccion"];
        foreach (DataRow Dr_Renglon in Dt_Valores_Construccion.Rows)
        {
            Dr_Avaluo = Dt_Construccion.NewRow();
            Dr_Avaluo["CROQUIS"] = Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arr.Campo_Croquis].ToString();
            Dr_Avaluo["TIPO"] = Convert.ToInt16(Dr_Renglon["TIPO"].ToString());
            Dr_Avaluo["EDO"] = Convert.ToInt16(Dr_Renglon["CON_SERV"].ToString());
            Dr_Avaluo["SUPERFICIE_M2"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arr.Campo_Superficie_M2].ToString());
            Dr_Avaluo["VALOR_PARCIAL"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arr.Campo_Valor_Parcial].ToString());
            Dr_Avaluo["EDAD"] = Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arr.Campo_Edad_Constru].ToString();
            Dr_Avaluo["FACTOR"] = Convert.ToDouble(Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arr.Campo_Factor].ToString());
            Dr_Avaluo["USO"] = Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arr.Campo_Uso_Constru].ToString();
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
            if (Dr_Renglon["VALOR_INDICADOR_A"].ToString().Trim() != "")
            {
                Dr_Avaluo["INDICE"] = Convert.ToDouble(Dr_Renglon["VALOR_INDICE1"].ToString());
            }
            Dr_Avaluo["INDICADOR_A"] = Dr_Renglon[Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A].ToString();
            Dr_Avaluo["VALOR_INDICADOR_A"] = Dr_Renglon["VALOR_INDICADOR_A"].ToString();
            if (Dr_Renglon["VALOR_INDICADOR_B"].ToString().Trim() != "")
            {
                Dr_Avaluo["INDICE"] = Convert.ToDouble(Dr_Renglon["VALOR_INDICE2"].ToString());
            }
            Dr_Avaluo["INDICADOR_B"] = Dr_Renglon[Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_B].ToString();
            Dr_Avaluo["VALOR_INDICADOR_B"] = Dr_Renglon["VALOR_INDICADOR_B"].ToString();
            if (Dr_Renglon["VALOR_INDICADOR_C"].ToString().Trim() != "")
            {
                Dr_Avaluo["INDICE"] = Convert.ToDouble(Dr_Renglon["VALOR_INDICE3"].ToString());
            }
            Dr_Avaluo["INDICADOR_C"] = Dr_Renglon[Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_C].ToString();
            Dr_Avaluo["VALOR_INDICADOR_C"] = Dr_Renglon["VALOR_INDICADOR_C"].ToString();
            if (Dr_Renglon["VALOR_INDICADOR_D"].ToString().Trim() != "")
            {
                Dr_Avaluo["INDICE"] = Convert.ToDouble(Dr_Renglon["VALOR_INDICE4"].ToString());
            }
            Dr_Avaluo["INDICADOR_D"] = Dr_Renglon[Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_D].ToString();
            Dr_Avaluo["VALOR_INDICADOR_D"] = Dr_Renglon["VALOR_INDICADOR_D"].ToString();
            Dr_Avaluo["GRUPO"] = "A";
            Dt_Caracteristicas_Terreno.Rows.Add(Dr_Avaluo);
        }

        DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
        DataTable Dt_MedColindancias = Ds_Avaluo_Urbano.Tables["DT_COLINDANCIAS"];
        foreach (DataRow Dr_Renglon in Dt_Medidas.Rows)
        {
            Dr_Avaluo = Dt_MedColindancias.NewRow();
            Dr_Avaluo["COLINDANCIA"] = Dr_Renglon[Ope_Cat_Colindancias_Arr.Campo_Medida_Colindancia].ToString();
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
        if (Grid_Avaluos_Urbanos_Inconformidades.SelectedIndex > -1)
        {            
                Imprimir_Reporte(Crear_Ds_Avaluo_Urbano_Reporte(), "Rpt_Ope_Cat_Avaluo_Rustico_Re.rpt", "Avaluo_Rustico", "Window_Frm", "Avaluo_Rustico");            
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
                //Consulta = "SELECT " + Cat_Pre_Claves_Ing_Costos.Campo_Costo + " FROM " + Cat_Pre_Claves_Ing_Costos.Tabla_Cat_Pre_Claves_Ing_Costos + " WHERE " + Cat_Pre_Claves_Ing_Costos.Campo_Clave_Ingreso_ID + " = '" + Clave_Ingreso_Id + "' AND " + Cat_Pre_Claves_Ing_Costos.Campo_Anio + "=" + DateTime.Now.Year;
                //Costo_Clave_Ingreso = Obtener_Dato_Consulta(Consulta);
                Consulta = "SELECT " + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " LIKE '%AUTORIZACION AVALUOS PERITOS FISCALES%'";
                Dependencia_Id = Obtener_Dato_Consulta(Consulta);
                if (Dependencia_Id.Trim() != "")
                {
                    Cls_Cat_Cat_Parametros_Negocio Dias = new Cls_Cat_Cat_Parametros_Negocio();
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "AUTORIZACION DE AVALUO RUSTICO";
                //    if (Hdf_Cobro_Anterior.Value.Trim() == "")
                //    {
                //        Hdf_Cobro_Anterior.Value = "0.00";
                //    }
                //    if ((Convert.ToDouble(Txt_Precio_Avaluo.Text) - Convert.ToDouble(Hdf_Cobro_Anterior.Value)) > 0)
                //    {
                //        Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                //        Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = (Convert.ToDouble(Txt_Precio_Avaluo.Text) - Convert.ToDouble(Hdf_Cobro_Anterior.Value)).ToString("0.00");
                //    }
                //    else
                //    {
                //        Calculo_Impuesto_Traslado.P_Estatus = "PAGADO";
                //        Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = "0.00";
                //    }
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
                    //Mostrar_Mensaje_Error("No se puede insertar el pasivo, falta el costo de la clave de ingreso del año " + DateTime.Now.Year + ".");
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
    protected void Grid_Avaluos_Urbanos_Inconformidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
    protected void Grid_Avaluos_Urbanos_Inconformidades_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Avaluos_Urbanos_Inconformidades.SelectedIndex > -1)
        {
            Cls_Cat_Cat_Parametros_Negocio Par = new Cls_Cat_Cat_Parametros_Negocio();
            Int16 columnas = Convert.ToInt16(Par.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Columnas_Calc_Construccion].ToString());
            Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Factores = new Cls_Cat_Cat_Tabla_Factores_Negocio();
            Tabla_Factores.P_Anio = DateTime.Now.Year.ToString();
            DataTable Dt_Factores_Cobro = Tabla_Factores.Consultar_Tabla_Factores_Cobro_Avaluos();
            if (Dt_Factores_Cobro.Rows.Count > 0)
            {
                Hdf_Factor_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_2].ToString();
                Hdf_Mayor_Ha.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Mayor_1_Ha].ToString();
                Hdf_Menos_Ha.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Menor_1_Ha].ToString();
                //Hdf_Porcentaje_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Porcentaje_PE].ToString();
            }
            else
            {
                Hdf_Factor_Cobro.Value = "0.00";
                Hdf_Mayor_Ha.Value = "0.00";
                Hdf_Menos_Ha.Value = "0.00";
                //Hdf_Porcentaje_Cobro.Value = "0.00";
            }
            DataTable Dt_Avaluo;
            Hdf_Anio_Avaluo.Value = Grid_Avaluos_Urbanos_Inconformidades.SelectedRow.Cells[2].Text;
            Hdf_No_Avaluo.Value = Grid_Avaluos_Urbanos_Inconformidades.SelectedRow.Cells[1].Text;
            Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio Aval_Urb = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();
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
            //Div_Observaciones.Visible = true;
            DataTable Dt_Motivos_Rechazo;
            Aval_Urb.P_Estatus = "= 'VIGENTE'";
            Dt_Motivos_Rechazo = Aval_Urb.Consultar_Motivos_Rechazo_Avaluo();
            Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();

            DataTable Dt_Tramites = new DataTable();



            Dt_Tramites = Aval_Urb.Consultar_Solicitud_Tramite_Avaluos();
            foreach (DataRow Dr_Renglon_Actual in Dt_Tramites.Rows)
            {
                if (Dr_Renglon_Actual["CLAVE_SOLICITUD"].ToString().ToUpper().Trim() == Dt_Avaluo.Rows[0]["CLAVE_SOLICITUD"].ToString().ToUpper().Trim())
                {
                    Txt_Motivo_Avaluo_Tramite.Text = Dr_Renglon_Actual["MOTIVO_AVALUO"].ToString();
                    Txt_Folio_Avaluo_Tramite.Text = Dr_Renglon_Actual["AVALUO_INCONFORMIDAD"].ToString();
                }
            }

            if (Cmb_Estatus.Text == "AUTORIZADO")
            {
                Btn_Autorizar_Avaluo.Visible = true;

            }
            else
            {
                Btn_Autorizar_Avaluo.Visible = true;
            }
        }
    }

    private void Cargar_Datos_Avaluo(DataTable Dt_Avaluo)
    {
        if (Dt_Avaluo.Rows.Count > 0)
        {
            Hdf_No_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_No_Avaluo].ToString();
            Hdf_Anio_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Anio_Avaluo].ToString();
            Hdf_Cuenta_Predial_Id.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Cuenta_Predial_Id].ToString();
            Cmb_Motivo_Avaluo.SelectedIndex = Cmb_Motivo_Avaluo.Items.IndexOf(Cmb_Motivo_Avaluo.Items.FindByValue(HttpUtility.HtmlDecode(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Motivo_Avaluo_Id].ToString())));
            Txt_No_Avaluo.Text = Dt_Avaluo.Rows[0]["AVALUO"].ToString();
            Txt_Cuenta_Predial.Text = Dt_Avaluo.Rows[0]["CUENTA_PREDIAL"].ToString();
            Txt_Clave_Catastral.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Clave_Catastral].ToString();
            Txt_Propietario.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Propietario].ToString();
            Cmb_Estatus.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Estatus].ToString();
            Txt_Domicilio_Not.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Domicilio_Notificacion].ToString();
            Txt_Municipio_Notificar.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Municipio_Notificacion].ToString();
            Txt_Ubicacion_Predio.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Ubicacion].ToString();
            Txt_Localidad.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Localidad_Municipio].ToString();
            Txt_Fecha.Text = Convert.ToDateTime(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Fecha_Creo].ToString()).ToString("dd/MMM/yyyy");
            Txt_Solicitante.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Solicitante].ToString();
            Txt_Nombre_Predio.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Nombre_Predio].ToString();
            Txt_X_Horas.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Coord_X_Grados].ToString();
            Txt_X_Minutos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Coord_X_Minutos].ToString();
            Txt_X_Segundos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Coord_X_Segundos].ToString();
            //Txt_Uso_Constru.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Uso_Constru].ToString().Trim();
            try
            {
                Cmb_Tipo_Construccion.SelectedIndex = -1;
                Cmb_Tipo_Construccion.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Uso_Constru].ToString();
                if (Cmb_Tipo_Construccion.SelectedValue != Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Uso_Constru].ToString())
                {
                    Cmb_Tipo_Construccion.SelectedValue = "OTRO";
                    Txt_Uso_Constru.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Uso_Constru].ToString();
                    Txt_Uso_Constru.Enabled = false;
                }
                Txt_Uso_Constru.Enabled = false;
            }
            catch
            {
                Cmb_Tipo_Construccion.SelectedValue = "OTRO";
                Txt_Uso_Constru.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Uso_Constru].ToString();
                Txt_Uso_Constru.Enabled = false;
            }
            Txt_Avaluo_Av.Text = Dt_Avaluo.Rows[0]["AVALUO"].ToString();
            Txt_Oficio.Text = Dt_Avaluo.Rows[0]["NO_OFICIO"].ToString();
            if (Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_R.Campo_Orientacion_X].ToString().Trim() != "")
            {
                Cmb_Latitud.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Orientacion_X].ToString().Trim();
            }
            else
            {
                Cmb_Latitud.SelectedValue = "SELECCIONE";
            }
            Txt_Y_Horas.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Coord_Y_Grados].ToString();
            Txt_Y_Minutos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Coord_Y_Minutos].ToString();
            Txt_Y_Segundos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Coord_Y_Segundos].ToString();
            if (Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Orientacion_Y].ToString().Trim() != "")
            {
                Cmb_Longitud.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Orientacion_Y].ToString().Trim();
            }
            else
            {
                Cmb_Longitud.SelectedValue = "SELECCIONE";
            }
            Txt_Coordenadas_UTM.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Coordenadas_UTM].ToString().Trim();
            Txt_Coordenadas_UTM_Y.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Coordenadas_UTM_Y].ToString().Trim();
            if (Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Tipo].ToString().Trim() == "")
            {
                Cmb_Coordenadas.SelectedIndex = 0;
            }
            else
            {
                Cmb_Coordenadas.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Tipo].ToString().Trim();
            }
            Cmb_Coordenadas_SelectedIndexChanged(null, null);
            Txt_Observaciones.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Observaciones].ToString();
            Txt_Valor_Total_Predio.Text = Convert.ToDouble(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Valor_Total_Predio].ToString()).ToString("#,###,###,###,###,###,###,##0.00");
            Hdf_Solicitud_Id.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_I.Campo_Solicitud_Id].ToString();
            Txt_Clave_Tramite.Text = Dt_Avaluo.Rows[0]["CLAVE_SOLICITUD"].ToString();
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico Inconformidades", "alert('La Cuenta Predial ingresada no existe actualmente');", true);
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

    //protected void Txt_Impuesto_Bimestral_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Txt_Impuesto_Bimestral.Text.Trim() != "")
    //        {
    //            Txt_Impuesto_Bimestral.Text = Convert.ToDouble(Txt_Impuesto_Bimestral.Text).ToString("###,###,###,###,##0.00");
    //        }
    //        else
    //        {
    //            Txt_Impuesto_Bimestral.Text = "0.00";
    //        }
    //    }
    //    catch (Exception Exc)
    //    {
    //        Txt_Impuesto_Bimestral.Text = "0.00";
    //    }
    //}

    protected void Guardar_Grid_Descripcion_Terreno()
    {
        DataTable Dt_Caracteristicas = (DataTable)Session["Dt_Caracteristicas"];
        for (int i = 0; i < Dt_Caracteristicas.Rows.Count; i++)
        {
            CheckBox Chk_Indicador_Valor_A = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[5].FindControl("Chk_Indicador_Valor_A");
            CheckBox Chk_Indicador_Valor_B = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[9].FindControl("Chk_Indicador_Valor_B");
            CheckBox Chk_Indicador_Valor_C = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[13].FindControl("Chk_Indicador_Valor_C");
            CheckBox Chk_Indicador_Valor_D = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[17].FindControl("Chk_Indicador_Valor_D");
            if (Chk_Indicador_Valor_A.Checked)
            {
                Dt_Caracteristicas.Rows[i]["VALOR_INDICADOR_A"] = "X";
            }
            else
            {
                Dt_Caracteristicas.Rows[i]["VALOR_INDICADOR_A"] = "";
            }

            if (Chk_Indicador_Valor_B.Checked)
            {
                Dt_Caracteristicas.Rows[i]["VALOR_INDICADOR_B"] = "X";
            }
            else
            {
                Dt_Caracteristicas.Rows[i]["VALOR_INDICADOR_B"] = "";
            }

            if (Chk_Indicador_Valor_C.Checked)
            {
                Dt_Caracteristicas.Rows[i]["VALOR_INDICADOR_C"] = "X";
            }
            else
            {
                Dt_Caracteristicas.Rows[i]["VALOR_INDICADOR_C"] = "";
            }

            if (Chk_Indicador_Valor_D.Checked)
            {
                Dt_Caracteristicas.Rows[i]["VALOR_INDICADOR_D"] = "X";
            }
            else
            {
                Dt_Caracteristicas.Rows[i]["VALOR_INDICADOR_D"] = "";
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

    /////******************************************************************************* 
    /////NOMBRE DE LA FUNCIÓN: Grid_Observaciones_PageIndexChanging
    /////DESCRIPCIÓN: Cambia la página del grid
    /////PARAMETROS: 
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 23/May/2012
    /////MODIFICO: 
    /////FECHA_MODIFICO:
    /////CAUSA_MODIFICACIÓN:
    /////******************************************************************************* 
    //protected void Grid_Observaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    try
    //    {
    //        Grid_Observaciones.SelectedIndex = -1;
    //        Grid_Observaciones.Columns[1].Visible = true;
    //        Grid_Observaciones.Columns[2].Visible = true;
    //        Grid_Observaciones.DataSource = (DataTable)Session["Dt_Motivos_Rechazo"];
    //        Grid_Observaciones.PageIndex = e.NewPageIndex;
    //        Grid_Observaciones.DataBind();
    //        Grid_Observaciones.Columns[1].Visible = false;
    //        Grid_Observaciones.Columns[2].Visible = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
    //        Lbl_Ecabezado_Mensaje.Visible = true;
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

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
        Valor_Parcial = Convert.ToDouble(((TextBox)Grid_Valores_Construccion.Rows[Index].Cells[3].FindControl("Txt_Superficie_M2")).Text) * Convert.ToDouble(((TextBox)Grid_Valores_Construccion.Rows[Index].Cells[4].FindControl("Txt_Valor_X_M2")).Text) * Convert.ToDouble(((TextBox)Grid_Valores_Construccion.Rows[Index].Cells[8].FindControl("Txt_Factor")).Text);
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

        if (Txt_Propietario.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese el Propietario.";
            Valido = false;
        }
        if (Txt_Solicitante.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese el Solicitante.";
            Valido = false;
        }
        if (Txt_Clave_Catastral.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese la Clave Catastral.";
            Valido = false;
        }
        if (Txt_Domicilio_Not.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese el Domicilio de Notificación.";
            Valido = false;
        }
        if (Txt_Municipio_Notificar.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese el Municipio de Notificación.";
            Valido = false;
        }

        if (Txt_Ubicacion_Predio.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese la ubicación del predio.";
            Valido = false;
        }
        if (Txt_Localidad.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese la Localidad y Municipio del Predio.";
            Valido = false;
        }
        if (Txt_Nombre_Predio.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese el Nombre del Predio.";
            Valido = false;
        }




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

        //if (Txt_Base_Gravable.Text.Trim() == "")
        //{
        //    Msj_Error += "<br/>";
        //    Msj_Error += "+ Ingrese la Base Gravable.";
        //    Valido = false;
        //}
        //if (Txt_Impuesto_Bimestral.Text.Trim() == "")
        //{
        //    Msj_Error += "<br/>";
        //    Msj_Error += "+ Ingrese el Impuesto Bimestral.";
        //    Valido = false;
        //}
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

            Dr_renglon["CROQUIS"] = "";
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
        Grid_Valores_Construccion.DataSource = Dt_Valores_Construccion;
        Grid_Valores_Construccion.PageIndex = 0;
        Grid_Valores_Construccion.DataBind();
        Grid_Valores_Construccion.Columns[5].Visible = false;
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
            //Calcular_Valor_Parcial_Construccion(index);
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
                            Txt_Temporal_Valor_M2.Text = Valor_M2.ToString("###,###,###,###,##0.00");
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
                Text_Txt_Tipo.Text = "0";
                TextBox Txt_Temporal_Val_Const_Id = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[5].FindControl("Txt_Valor_Construccion_Id");
                TextBox Txt_Temporal_Valor_M2 = (TextBox)Grid_Valores_Construccion.Rows[index].Cells[4].FindControl("Txt_Valor_X_M2");
                Dt_Valores_Construccion.Rows[index]["VALOR_CONSTRUCCION_ID"] = "";
                Txt_Temporal_Val_Const_Id.Text = "";
                Txt_Temporal_Valor_M2.Text = "0.00";
            }
            //Calcular_Valor_Parcial_Construccion(index);
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
        Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();
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
        Text_Txt_Valor_Parcial.Text = (Convert.ToDouble(Txt_Superficie_M2.Text) * Convert.ToDouble(Txt_Valor_M2.Text) * Convert.ToDouble(Txt_Factor.Text)).ToString("###,###,###,###,###,##0.00");
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
        Double Valor_Construccion = 0;
        Double Valor_Terreno = 0;
        Double Valor_Total_Predio = 0;
        Valor_Construccion = Convert.ToDouble(Txt_Construccion_Valor_Total.Text);
        Valor_Terreno = Convert.ToDouble(Txt_Terreno_Valor_Total.Text);
        Valor_Total_Predio = Valor_Construccion + Valor_Terreno;
        Txt_Valor_Total_Predio.Text = Valor_Total_Predio.ToString("###,###,###,###,##0.00");
        //Calcular_Precio_Avaluo();
    }

    /////******************************************************************************* 
    /////NOMBRE DE LA FUNCIÓN: Calcular_Precio_Avaluo
    /////DESCRIPCIÓN: Cálcula el importe del avalúo
    /////PARAMETROS: 
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 23/May/2012
    /////MODIFICO: 
    /////FECHA_MODIFICO:
    /////CAUSA_MODIFICACIÓN:
    /////******************************************************************************* 
    //private void Calcular_Precio_Avaluo()
    //{
    //    Double Valor_Total_Predio = 0;
    //    Double Precio_Avaluo = 0;
    //    Valor_Total_Predio = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
    //    Precio_Avaluo = (((Valor_Total_Predio * Convert.ToDouble(Hdf_Factor_Cobro1.Value)) + Convert.ToDouble(Hdf_Base_Cobro.Value)) * Convert.ToDouble(Hdf_Factor_Cobro2.Value)) * (Convert.ToDouble(Hdf_Porcentaje_Cobro.Value) / 100);
    //    Txt_Precio_Avaluo.Text = Precio_Avaluo.ToString("###,###,###,###,##0.00");
    //}

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
            Llenar_Tabla_Avaluos_Urbanos(Grid_Avaluos_Urbanos_Inconformidades.PageIndex);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Avaluos_Urbanos_Inconformidades.SelectedIndex = -1;
            Div_Datos_Avaluo.Visible = false;
            Div_Grid_Avaluo.Visible = true;
            //Div_Observaciones.Visible = false;
            Session["Dt_Grid_Valores_Construccion"] = null;
            Session["Dt_Grid_Calculos"] = null;
            Session["Dt_Grid_Elementos_Construccion"] = null;
            Session["Dt_Grid_Valores_Construccion"] = null;
            Session["Anio"] = null;
            Session["Dt_Tabla_Valores_Construccion"] = null;
            Session["Dt_Motivos_Rechazo"] = null;
            Session["Dt_Documentos"] = null;
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
            Dt_Valores_Construccion.Rows[i]["USO_CONTRU"] = Txt_Uso.Text.ToUpper();
        }

    }


    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Coordenadas_SelectedIndexChanged
    ///DESCRIPCIÓN: evento del combo de tipos de coordenadas
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 24/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
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
                Double Segundos = Convert.ToDouble(Txt_X_Segundos.Text);
                if (Segundos > 60)
                {
                    Segundos = 60;
                }
                Txt_X_Segundos.Text = (Segundos).ToString("#0.00");
            }
        }
        catch (Exception Exc)
        {
            Txt_X_Segundos.Text = "";
        }
    }
    protected void Txt_X_Minutos_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_X_Minutos.Text.Trim() == "")
            {
                Txt_X_Minutos.Text = "";
            }
            else
            {
                Double Minutos = Convert.ToDouble(Txt_X_Minutos.Text);
                if (Minutos > 60)
                {
                    Minutos = 60;
                }
                Txt_X_Minutos.Text = (Minutos).ToString("#0");
            }
        }
        catch (Exception Exc)
        {
            Txt_X_Minutos.Text = "";
        }
    }
    protected void Txt_X_Horas_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_X_Horas.Text.Trim() == "")
            {
                Txt_X_Horas.Text = "";
            }
            else
            {
                Double Horas = Convert.ToDouble(Txt_X_Horas.Text);
                if (Horas > 160)
                {
                    Horas = 160;
                }
                Txt_X_Horas.Text = (Horas).ToString("##0");
            }
        }
        catch (Exception Exc)
        {
            Txt_X_Horas.Text = "";
        }
    }
    protected void Txt_Y_Horas_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Y_Horas.Text.Trim() == "")
            {
                Txt_Y_Horas.Text = "";
            }
            else
            {
                Double Horas = Convert.ToDouble(Txt_Y_Horas.Text);
                if (Horas > 160)
                {
                    Horas = 160;
                }
                Txt_Y_Horas.Text = (Horas).ToString("##0");
            }
        }
        catch (Exception Exc)
        {
            Txt_Y_Horas.Text = "";
        }
    }
    protected void Txt_Y_Minutos_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Y_Minutos.Text.Trim() == "")
            {
                Txt_Y_Minutos.Text = "";
            }
            else
            {
                Double Minutos = Convert.ToDouble(Txt_Y_Minutos.Text);
                if (Minutos > 60)
                {
                    Minutos = 60;
                }
                Txt_Y_Minutos.Text = (Minutos).ToString("#0");
            }
        }
        catch (Exception Exc)
        {
            Txt_Y_Minutos.Text = "";
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
                Double Segundos = Convert.ToDouble(Txt_Y_Segundos.Text);
                if (Segundos > 60)
                {
                    Segundos = 60;
                }
                Txt_Y_Segundos.Text = (Segundos).ToString("#0.00");
            }
        }
        catch (Exception Exc)
        {
            Txt_Y_Segundos.Text = "";
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
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Chk_Indicador_Valor_CheckedChanged
    ///DESCRIPCIÓN: Realiza los calculos de los valores
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Med_Col_Click
    ///DESCRIPCIÓN: Carga al grid las medidas de colindancia
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 

    protected void Btn_Agregar_Med_Col_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Medida_Colindancia.Text.Trim() != "")
        {
            DataTable Dt_Medidas = (DataTable)Session["Dt_Medidas"];
            Boolean Entro = false;
            foreach (DataRow Dr_Renglon in Dt_Medidas.Rows)
            {
                if (Dr_Renglon[Ope_Cat_Colindancias_Aui.Campo_Medida_Colindancia].ToString() == Txt_Medida_Colindancia.Text.ToUpper() && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Entro = true;
                    break;
                }
            }
            if (!Entro)
            {
                DataRow Dr_Nuevo = Dt_Medidas.NewRow();
                Dr_Nuevo[Ope_Cat_Colindancias_Aui.Campo_No_Colindancia] = " ";
                Dr_Nuevo["ACCION"] = "ALTA";
                Dr_Nuevo[Ope_Cat_Colindancias_Aui.Campo_Medida_Colindancia] = Txt_Medida_Colindancia.Text.ToUpper();
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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Colindancias_PageIndexChanging
    ///DESCRIPCIÓN: 
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
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
                    if (Dr_Renglon[Ope_Cat_Colindancias_Aui.Campo_Medida_Colindancia].ToString() == Grid_Colindancias.SelectedRow.Cells[1].Text.ToUpper() && Dr_Renglon["ACCION"].ToString() != "BAJA")
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Ventana_Avaluo_Click
    ///DESCRIPCIÓN: Cierra la ventana de la busqueda modal
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Btn_Cerrar_Ventana_Avaluo_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Avaluos.Hide();
        Txt_Busqueda_Avaluo.Text = "";
        Grid_Avaluos_Av.DataSource = null;
        Grid_Avaluos_Av.DataBind();
    }



    protected void Btn_Busqueda_Avaluos_Click(object sender, EventArgs e)
    {
        Llenar_Tabla_Avaluos_Urbanos_Av(0);
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Avaluos_Urbanos_Av
    ///DESCRIPCIÓN: Llena la tabla de los datos de calidad
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Avaluos_Urbanos_Av(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio Avaluo_Urb = new Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio();
            Avaluo_Urb.P_Folio = Txt_Busqueda_Avaluo.Text.Trim();
            Avaluo_Urb.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
            Avaluo_Urb.P_Estatus = " ='AUTORIZADO'";
            Grid_Avaluos_Av.Columns[1].Visible = true;
            Grid_Avaluos_Av.Columns[2].Visible = true;
            Grid_Avaluos_Av.DataSource = Avaluo_Urb.Consultar_Avaluo_Rustico();
            Grid_Avaluos_Av.PageIndex = Pagina;
            Grid_Avaluos_Av.DataBind();
            Grid_Avaluos_Av.Columns[1].Visible = false;
            Grid_Avaluos_Av.Columns[2].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Avaluos_Av_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Avaluos_Av_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Avaluos_Av.SelectedIndex > -1)
        {
            Cls_Cat_Cat_Parametros_Negocio Par = new Cls_Cat_Cat_Parametros_Negocio();
            Int16 columnas = Convert.ToInt16(Par.Consultar_Parametros().Rows[0][Cat_Cat_Parametros.Campo_Columnas_Calc_Construccion].ToString());
            Cls_Cat_Cat_Tabla_Factores_Negocio Tabla_Factores = new Cls_Cat_Cat_Tabla_Factores_Negocio();
            Tabla_Factores.P_Anio = DateTime.Now.Year.ToString();
            DataTable Dt_Factores_Cobro = Tabla_Factores.Consultar_Tabla_Factores_Cobro_Avaluos();
            DataTable Dt_Avaluo;
            Hdf_Anio_Avaluo_AV.Value = Grid_Avaluos_Av.SelectedRow.Cells[2].Text;
            Hdf_No_Avaluo_AV.Value = Grid_Avaluos_Av.SelectedRow.Cells[1].Text;
            Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio Aval_Urb = new Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio();
            Aval_Urb.P_No_Avaluo = Hdf_No_Avaluo_AV.Value;
            Aval_Urb.P_Anio_Avaluo = Hdf_Anio_Avaluo_AV.Value;
            Session["Dt_Tabla_Valores_Construccion"] = Aval_Urb.Consultar_Tabla_Valores_Construccion();
            Dt_Avaluo = Aval_Urb.Consultar_Avaluo_Rustico();
            Cargar_Datos_Avaluo_Av(Dt_Avaluo);
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
           

            foreach (DataRow Dr_Renglon in Dt_Medidas.Rows)
            {
                Dr_Renglon["ACCION"] = "ALTA";
            }
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
           // Div_Observaciones.Visible = true;
            DataTable Dt_Motivos_Rechazo;
            Aval_Urb.P_Estatus = "= 'VIGENTE'";
            Dt_Motivos_Rechazo = Aval_Urb.Consultar_Motivos_Rechazo_Avaluo();
            Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
            //Grid_Observaciones.Columns[0].Visible = true;
            //Grid_Observaciones.Columns[1].Visible = true;
            //Grid_Observaciones.Columns[3].Visible = true;
            //Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
            //Grid_Observaciones.PageIndex = 0;
            //Grid_Observaciones.DataBind();
            //Grid_Observaciones.Columns[0].Visible = false;
            //Grid_Observaciones.Columns[1].Visible = false;
            //Grid_Observaciones.Columns[3].Visible = false;

            //DataTable Dt_Archivos = Aval_Urb.P_Dt_Documentos.Copy();
            //Dt_Archivos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
            //Dt_Archivos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
            //Session["Dt_Documentos"] = Dt_Archivos;
            //Grid_Documentos.Columns[0].Visible = true;
            //Grid_Documentos.Columns[1].Visible = true;
            //Grid_Documentos.DataSource = Dt_Archivos;
            //Grid_Documentos.DataBind();
            //Grid_Documentos.Columns[0].Visible = false;
            //Grid_Documentos.Columns[1].Visible = false;
            ////DataTable Dt_Avaluo;
            //Hdf_Anio_Avaluo_AV.Value = Grid_Avaluos_Av.SelectedRow.Cells[2].Text;
            //Hdf_No_Avaluo_AV.Value = Grid_Avaluos_Av.SelectedRow.Cells[1].Text;
            //Txt_Avaluo_Av.Text = Hdf_Anio_Avaluo_AV.Value + "/" + Convert.ToInt16(Hdf_No_Avaluo_AV.Value);

            //Grid_Avaluos_Av.SelectedIndex = -1;
            //Grid_Avaluos_Av.DataSource = null;
            //Grid_Avaluos_Av.PageIndex = 0;
            //Grid_Avaluos_Av.DataBind();
            Mpe_Avaluos.Hide();
        }



    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Avaluos_Av_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Avaluos_Av_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Avaluos_Urbanos_Av(e.NewPageIndex);
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Construccion_Dominante
    ///DESCRIPCIÓN: 
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 23/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
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
        if (!Directory.Exists(Server.MapPath("../Catastro/Archivos_Ari/" + Hdf_Anio_Avaluo.Value + "_" + Hdf_No_Avaluo.Value + "/")))
        {
            Directory.CreateDirectory(Server.MapPath("../Catastro/Archivos_Ari/" + Hdf_Anio_Avaluo.Value + "_" + Hdf_No_Avaluo.Value + "/"));
        }
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "ALTA")
            {
                //crear filestream y binarywriter para guardar archivo
                FileStream Escribir_Archivo = new FileStream(Server.MapPath("../Catastro/Archivos_Ari/" + Hdf_Anio_Avaluo.Value + "_" + Hdf_No_Avaluo.Value + "/" + Dr_Renglon["RUTA_DOCUMENTO"].ToString()), FileMode.Create, FileAccess.Write);
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
    ///NOMBRE DE LA FUNCIÓN: Convertir_Imagen_A_Cadena_Bytes
    ///DESCRIPCIÓN: Convierte la Imagen a una Cadena de Bytes.
    ///PROPIEDADES:   1.  P_Imagen.  Imagen a Convertir.    
    ///CREO: Francisco Antonio Gallardo Castañeda
    ///FECHA_CREO: Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Byte[] Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image P_Imagen)
    {
        Byte[] Img_Bytes = null;
        try
        {
            if (P_Imagen != null)
            {
                MemoryStream MS_Tmp = new MemoryStream();
                P_Imagen.Save(MS_Tmp, P_Imagen.RawFormat);
                Img_Bytes = MS_Tmp.GetBuffer();
                MS_Tmp.Close();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Verificar.";
            Lbl_Mensaje_Error.Text = "Verificar.";
            Div_Contenedor_Msj_Error.Visible = false;
        }
        return Img_Bytes;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Ventana_Click
    ///DESCRIPCIÓN: Cierra la ventana de la busqueda modal
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 04/Jun/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************

    protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Solicitud.Hide();
        Txt_Busqueda.Text = "";
        Grid_Solicitudes.DataSource = null;
        Grid_Solicitudes.DataBind();

        Mpe_Busqueda_Oficios.Hide();
        Txt_Busqueda.Text = "";
        Grid_Oficios.DataSource = null;
        Grid_Oficios.DataBind();
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
    protected void Grid_Solicitudes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Tabla_Solicitudes(e.NewPageIndex);
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Calles_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Solicitudes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Solicitudes.SelectedIndex > -1)
        {
            Hdf_Solicitud_Id.Value = Grid_Solicitudes.SelectedRow.Cells[1].Text;

            if (Grid_Solicitudes.SelectedRow.Cells[2].Text != "&nbsp;" && Grid_Solicitudes.SelectedRow.Cells[2].Text != "")
            {
                Txt_Clave_Tramite.Text = Grid_Solicitudes.SelectedRow.Cells[2].Text;
                Txt_Cuenta_Predial.Text = Grid_Solicitudes.SelectedRow.Cells[5].Text;
                Txt_Motivo_Avaluo_Tramite.Text = Grid_Solicitudes.SelectedRow.Cells[6].Text;
                Txt_Folio_Avaluo_Tramite.Text = Grid_Solicitudes.SelectedRow.Cells[7].Text;
                String Consulta = "SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID 
                    + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " 
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "= '" + Txt_Cuenta_Predial.Text.Trim() + "'";
                Hdf_Cuenta_Predial_Id.Value = Obtener_Dato_Consulta(Consulta);
                Btn_Busqueda_Avaluos_Av.Enabled = true;
            }
            else
            {
                Txt_Clave_Tramite.Text = "";

            }
            Grid_Solicitudes.SelectedIndex = -1;
            Mpe_Busqueda_Solicitud.Hide();
        }
    }


    protected void Btn_Busqueda_Solicitudes_Click(object sender, EventArgs e)
    {
        Llenar_Tabla_Solicitudes(0);
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
    private void Llenar_Tabla_Solicitudes(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio Solicitud_Tramite = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();
            DataTable Dt_Tramites = new DataTable(); ;
            //Dt_Tramites.Columns.Add("NOMBRE_COMPLETO", typeof(String));
            //DataRow Dr_renglon;
            //for (int i = 0; i <  i++)
            //{
            //    Dr_renglon = Dt_Tramites.NewRow();

            //    Dr_renglon["NOMBRE_COMPLETO"] = "";

            //    Dt_Tramites.Rows.Add(Dr_renglon);
            //}

            Solicitud_Tramite.P_Cuenta_Predial = Txt_Cuenta_Predial_Bqd.Text.ToUpper();
            Solicitud_Tramite.P_Solicitante = Txt_Solicitante_Bqd.Text.ToUpper();
            Dt_Tramites = Solicitud_Tramite.Consultar_Solicitud_Tramite();


            Grid_Solicitudes.Columns[1].Visible = true;

            Grid_Solicitudes.DataSource = Dt_Tramites;
            Grid_Solicitudes.PageIndex = Pagina;
            Grid_Solicitudes.DataBind();
            Grid_Solicitudes.Columns[1].Visible = false;
            Txt_Cuenta_Predial_Bqd.Text = "";
            Txt_Solicitante_Bqd.Text = "";

        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
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

    private void Cargar_Datos_Avaluo_Av(DataTable Dt_Avaluo)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_consulta_Ope_Resumen = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        try
        {
            if (Dt_Avaluo.Rows.Count > 0)
            {
                Txt_Uso_Constru.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Uso].ToString();
                //Hdf_Cuenta_Predial_Id.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Cuenta_Predial_Id].ToString();
                Txt_Observaciones.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Observaciones].ToString();
               // Txt_Observaciones_Rechazo.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Observaciones_Perito].ToString();
                Txt_Solicitante.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Solicitante].ToString();
                Txt_Avaluo_Av.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo].ToString() + "/" + Convert.ToInt16(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo]).ToString();
                //Txt_No_Avaluo.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo].ToString() + "/" + Convert.ToInt16(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_No_Avaluo]).ToString();
                //Txt_Cuenta_Predial.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Cuenta_Predial].ToString();
                Hdf_Anio_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Anio_Avaluo].ToString();
                Cmb_Motivo_Avaluo.SelectedIndex = Cmb_Motivo_Avaluo.Items.IndexOf(Cmb_Motivo_Avaluo.Items.FindByValue(HttpUtility.HtmlDecode(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Motivo_Avaluo_Id].ToString())));
                Txt_Propietario.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Propietario].ToString();
                //Cmb_Estatus.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Estatus].ToString();
                Txt_Domicilio_Not.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Domicilio_Notificacion].ToString();
                Txt_Municipio_Notificar.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Municipio_Notificacion].ToString();
                Txt_Ubicacion_Predio.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Ubicacion].ToString();
                Txt_Localidad.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Localidad_Municipio].ToString();
                Txt_Fecha.Text = Convert.ToDateTime(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Fecha_Creo].ToString()).ToString("dd/MMM/yyyy");
                //Cmb_Revision.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Permitir_Revision].ToString();
                //Txt_Nombre_Predio.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Nombre_Predio].ToString();
                Txt_X_Horas.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_X_Grados].ToString();
                Txt_X_Minutos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_X_Minutos].ToString();
                Txt_X_Segundos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico_V.Campo_Coord_X_Segundos].ToString();
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Siguiente_Fase_Click
    ///DESCRIPCIÓN: Evento del botón Siguiente_fase
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Bt_Autorizar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Avaluos_Urbanos_Inconformidades.SelectedIndex > -1)
            {
                Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();



                Avaluo.P_Solicitud_Id = Hdf_Solicitud_Id.Value;
                Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
                Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
                Avaluo.P_Estatus = "AUTORIZADO";
                if ((Avaluo.Modificar_Estatus_Avaluo_Rustico_In()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico", "alert('El avalúo a sido autorizado.');", true);
                    Btn_Salir_Click(null, null);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico", "alert('Error');", true);
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Seleccione el Avalúo.";
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Calles_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Oficios_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Oficios.SelectedIndex > -1)
        {
            Hdf_No_Oficio.Value = Grid_Oficios.SelectedRow.Cells[1].Text;

            if (Grid_Oficios.SelectedRow.Cells[4].Text != "&nbsp;" && Grid_Oficios.SelectedRow.Cells[4].Text != "")
            {
                Txt_Oficio.Text = Grid_Oficios.SelectedRow.Cells[4].Text;
            }
            else
            {
                Txt_Oficio.Text = "";

            }
            Grid_Oficios.SelectedIndex = -1;
            Mpe_Busqueda_Oficios.Hide();
        }
    }


    protected void Btn_Busqueda_Oficios_Click(object sender, EventArgs e)
    {
        Llenar_Tabla_Oficios(0);
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
    private void Llenar_Tabla_Oficios(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Recepcion_Oficios_Negocio Oficios = new Cls_Ope_Cat_Recepcion_Oficios_Negocio();
            DataTable Dt_Oficios;
            Oficios.P_No_Oficio_Recepcion = Txt_No_Oficio.Text.ToUpper();
            Oficios.P_Dependencia = Txt_Dependencia.Text.ToUpper();

            Dt_Oficios = Oficios.Consultar_Oficios_Avaluos();
            Grid_Oficios.Columns[1].Visible = true;

            Grid_Oficios.DataSource = Dt_Oficios;
            Grid_Oficios.PageIndex = Pagina;
            Grid_Oficios.DataBind();
            Grid_Oficios.Columns[1].Visible = false;
            Txt_No_Oficio.Text = "";
            Txt_Dependencia.Text = "";

        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
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
    protected void Grid_Oficios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Tabla_Oficios(e.NewPageIndex);
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = E.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    

}
