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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Numalet;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using Presidencia.Catalogo_Cat_Motivos_Rechazo.Negocio;

public partial class paginas_Catastro_Frm_Ope_Cat_Seguimiento_Avaluo_Rustico_Inconformidades : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);
                Llenar_Tabla_Avaluos_Urbanos(0);
                Llenar_Combo_Motivos_Avaluo();
                Llenar_Combo_Motivos_Rechazo();
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
        Cmb_Motivo_Avaluo.Enabled = false;
        Txt_No_Avaluo.Enabled = false;
        Txt_Cuenta_Predial.Enabled = false;
        Txt_Propietario.Enabled = false;
        Txt_Solicitante.Enabled = false;
        Txt_Clave_Catastral.Enabled = false;
        Txt_Domicilio_Not.Enabled = false;
        Txt_Municipio_Notificar.Enabled = false;
        Txt_Ubicacion_Predio.Enabled = false;
        Txt_Localidad.Enabled = false;
        Txt_Nombre_Predio.Enabled = false;
        Txt_Observaciones.Enabled = false;
        Txt_X_Horas.Enabled = false;
        Txt_X_Minutos.Enabled = false;
        Txt_X_Segundos.Enabled = false;
        Cmb_Latitud.Enabled = false;
        Txt_Y_Horas.Enabled = false;
        Txt_Y_Minutos.Enabled = false;
        Txt_Y_Segundos.Enabled = false;
        Cmb_Longitud.Enabled = false;
        //Txt_Base_Gravable.Enabled = false;
        //Txt_Impuesto_Bimestral.Enabled = false;
        Txt_Valor_Total_Predio.Enabled = false;
        Txt_Sur.Enabled = false;
        Txt_Norte.Enabled = false;
        Txt_Poniente.Enabled = false;
        Txt_Oriente.Enabled = false;
        Txt_Terreno_Superficie_Total.Enabled = false;
        Txt_Terreno_Valor_Total.Enabled = false;
        Txt_Construccion_Superficie_Total.Enabled = false;
        Txt_Construccion_Valor_Total.Enabled = false;
        Txt_Precio_Avaluo.Enabled = false;
        Cmb_Estatus.Enabled = false;
        Txt_Busqueda.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
        Txt_Observaciones_Rechazo.Enabled = false;
        Grid_Avaluos_Urbanos.Enabled = Enabled;
        Grid_Calculos.Enabled = false;
        Grid_Descripcion_Terreno.Enabled = false;
        Grid_Valores_Construccion.Enabled = false;
        Cmb_Motivos_Rechazo.Enabled = !Enabled;
        Btn_Agregar_Observacion.Enabled = !Enabled;
        Btn_Eliminar_Observacion.Enabled = !Enabled;
        Txt_Observaciones_Rechazo.Enabled = !Enabled;
        Grid_Observaciones.Enabled = !Enabled;
        Txt_Terreno_Superficie_Total.Style["text-align"] = "Right";
        Txt_Terreno_Valor_Total.Style["text-align"] = "Right";
        Txt_Construccion_Superficie_Total.Style["text-align"] = "Right";
        Txt_Construccion_Valor_Total.Style["text-align"] = "Right";
        Txt_Precio_Avaluo.Style["text-align"] = "Right";
        //Txt_Impuesto_Bimestral.Style["text-align"] = "Right";
        Txt_Valor_Total_Predio.Style["text-align"] = "Right";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Motivos_Rechazo
    ///DESCRIPCIÓN: Llena la el combo con los datos de los motivos de rechazo
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 07/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Motivos_Rechazo()
    {
        try
        {
            DataTable Dt_Motivos_Rechazo;
            DataRow Dr_Renglon_Nuevo;
            Cls_Cat_Cat_Motivos_Rechazo_Negocio Motivos = new Cls_Cat_Cat_Motivos_Rechazo_Negocio();
            Dt_Motivos_Rechazo = Motivos.Consultar_Motivos_Rechazo();
            Dr_Renglon_Nuevo = Dt_Motivos_Rechazo.NewRow();
            Dr_Renglon_Nuevo[Cat_Cat_Motivos_Rechazo.Campo_Motivo_Id] = "SELECCIONE";
            Dr_Renglon_Nuevo[Cat_Cat_Motivos_Rechazo.Campo_Motivo_Descripcion] = "<SELECCIONE>";
            Dt_Motivos_Rechazo.Rows.InsertAt(Dr_Renglon_Nuevo, 0);
            Cmb_Motivos_Rechazo.DataSource = Dt_Motivos_Rechazo;
            Cmb_Motivos_Rechazo.DataTextField = Cat_Cat_Motivos_Rechazo.Campo_Motivo_Descripcion;
            Cmb_Motivos_Rechazo.DataValueField = Cat_Cat_Motivos_Rechazo.Campo_Motivo_Id;
            Cmb_Motivos_Rechazo.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
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
        //Txt_Base_Gravable.Text = "";
        //Txt_Impuesto_Bimestral.Text = "0.00";
        Txt_Valor_Total_Predio.Text = "0.00";
        Txt_Sur.Text = "";
        Txt_Norte.Text = "";
        Txt_Poniente.Text = "";
        Txt_Oriente.Text = "";
        Txt_Terreno_Superficie_Total.Text = "0.00";
        Txt_Terreno_Valor_Total.Text = "0.00";
        Txt_Construccion_Superficie_Total.Text = "0.00";
        Txt_Construccion_Valor_Total.Text = "0.00";
        Txt_Precio_Avaluo.Text = "0.00";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Busqueda.Text = "";
        Txt_Observaciones_Rechazo.Text = "";
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
            Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio Avaluo_Urb = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Avaluo_Urb.P_Folio = Txt_Busqueda.Text.Trim();
            }
            //Avaluo_Urb.P_Perito_Externo_Id = Cls_Sessiones.Empleado_ID;
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
                if (Grid_Avaluos_Urbanos.SelectedRow.Cells[8].Text == "POR VALIDAR" || Grid_Avaluos_Urbanos.SelectedRow.Cells[8].Text == "RECHAZADO")
                {
                    if (Btn_Modificar.AlternateText.Equals("Modificar"))
                    {
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Imprimir.Visible = false;
                    }
                    else
                    {
                        Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio Avaluo = new Cls_Ope_Cat_Avaluo_Rustico_Inconformidades_Negocio();
                        Avaluo.P_Dt_Observaciones = (DataTable)Session["Dt_Motivos_Rechazo"];
                        Avaluo.P_No_Avaluo = Hdf_No_Avaluo.Value;
                        Avaluo.P_Anio_Avaluo = Hdf_Anio_Avaluo.Value;
                        Cambiar_Estatus_Avaluo_Rechazado_Autorizada();
                        Avaluo.P_Estatus = Cmb_Estatus.SelectedValue;
                        Avaluo.P_Veces_Rechazo = (Convert.ToInt16(Grid_Avaluos_Urbanos.SelectedRow.Cells[4].Text) + 1).ToString();
                        Avaluo.P_Observaciones_Perito = Txt_Observaciones_Rechazo.Text;
                        if ((Avaluo.Modificar_Observaciones_Ara()))
                        {
                            if (Cmb_Estatus.SelectedValue == "RECHAZADO")
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico Inconformidades", "alert('Se ha rechazado el Avalúo Exitosamente');", true);

                            }
                            else if (Cmb_Estatus.SelectedValue == "AUTORIZADO")
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico Inconformidades", "alert('Se ha Autorizado el Avalúo Exitosamente');", true);
                                //Insertar_Pasivo("AR" + Txt_No_Avaluo.Text);
                                //Imprimir_Reporte(Crear_Ds_Avaluo_Urbano(), "Rpt_Ope_Cat_Folio_Pago_Avaluo_Rustico.rpt", "Rpt_Folio_Pago_Avaluo_Rustico", "Window_Frm", "Avaluo");
                            }
                            Btn_Salir_Click(null, null);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Urbano Inconformidades", "alert('Error');", true);
                        }
                    }
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
        Perito_Interno.P_Perito_Externo_Id = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Perito_Externo_Id].ToString();
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
        Dr_Renglon_Nuevo["CANTIDAD"] = Convert.ToDouble(Txt_Precio_Avaluo.Text);
        Dr_Renglon_Nuevo["UBICACION"] = Txt_Ubicacion_Predio.Text + ", " + Txt_Localidad.Text;
        Dr_Renglon_Nuevo["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
        Dr_Renglon_Nuevo["CANTIDAD_LETRAS"] = Cantidad.ToCustomCardinal(Txt_Precio_Avaluo.Text.Replace(",", "")).ToUpper();
        Dr_Renglon_Nuevo["VALOR_PREDIO"] = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
        Dr_Renglon_Nuevo["DIA"] = DateTime.Now.Day.ToString();
        Dr_Renglon_Nuevo["MES"] = DateTime.Now.ToString("MMMM").ToUpper();
        Dr_Renglon_Nuevo["ANIO"] = DateTime.Now.Year.ToString();
        Session["E_Mail"] = Dt_Avaluo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Usuario].ToString();
        Dt_Folio_Avaluo.Rows.Add(Dr_Renglon_Nuevo);
        return Folio_Avaluo;
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
            Enviar_Correo_Cuenta((String)Session["E_Mail"], Server.MapPath("../../Reporte/" + Archivo_PDF));
            Session.Remove("E_Mail");
            //Mostrar_Reporte(Archivo_PDF, Tipo, Formato);
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
        }
        catch (Exception Ex)
        {
            throw new Exception("No se pudo enviar el Correo.");
        }

    }

    private void Cambiar_Estatus_Avaluo_Rechazado_Autorizada()
    {
        DataTable Dt_Motivos_Rechazo = (DataTable)Session["Dt_Motivos_Rechazo"];
        Int16 Registros = 0;
        foreach (DataRow Dr_Renglon in Dt_Motivos_Rechazo.Rows)
        {
            if (Dr_Renglon["ESTATUS"].ToString() == "VIGENTE")
            {
                Registros++;
            }
        }
        if (Registros > 0)
        {
            Cmb_Estatus.SelectedIndex = 1;
        }
        else
        {
            Cmb_Estatus.SelectedIndex = 2;
        }
    }

    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {

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
                    if (Hdf_Cobro_Anterior.Value.Trim() == "")
                    {
                        Hdf_Cobro_Anterior.Value = "0.00";
                    }
                    if ((Convert.ToDouble(Txt_Precio_Avaluo.Text) - Convert.ToDouble(Hdf_Cobro_Anterior.Value)) > 0)
                    {
                        Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                        Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = (Convert.ToDouble(Txt_Precio_Avaluo.Text) - Convert.ToDouble(Hdf_Cobro_Anterior.Value)).ToString("0.00");
                    }
                    else
                    {
                        Calculo_Impuesto_Traslado.P_Estatus = "PAGADO";
                        Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = "0.00";
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
            if (Dt_Factores_Cobro.Rows.Count > 0)
            {
                Hdf_Base_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Base_Cobro].ToString();
                Hdf_Factor_Cobro1.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_2].ToString();
                Hdf_Factor_Cobro2.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Factor_Cobro_2].ToString();
                Hdf_Porcentaje_Cobro.Value = Dt_Factores_Cobro.Rows[0][Cat_Cat_Factores_Cobro_Avaluos.Campo_Porcentaje_PE].ToString();
            }
            else
            {
                Hdf_Base_Cobro.Value = "0.00";
                Hdf_Factor_Cobro1.Value = "0.00";
                Hdf_Factor_Cobro2.Value = "0.00";
                Hdf_Porcentaje_Cobro.Value = "0.00";
            }
            DataTable Dt_Avaluo;
            Hdf_Anio_Avaluo.Value = Grid_Avaluos_Urbanos.SelectedRow.Cells[2].Text;
            Hdf_No_Avaluo.Value = Grid_Avaluos_Urbanos.SelectedRow.Cells[1].Text;
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
            Grid_Valores_Construccion.DataSource = Aval_Urb.P_Dt_Calculo_Valor_Construccion;
            Grid_Valores_Construccion.PageIndex = 0;
            Grid_Valores_Construccion.DataBind();
            Session["Dt_Caracteristicas"] = Aval_Urb.P_Dt_Clasificacion_Zona.Copy();
            Grid_Descripcion_Terreno.DataSource = Aval_Urb.P_Dt_Clasificacion_Zona;
            Grid_Descripcion_Terreno.DataBind();
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
            Grid_Observaciones.Columns[1].Visible = true;
            Grid_Observaciones.Columns[2].Visible = true;
            Grid_Observaciones.Columns[4].Visible = true;
            Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
            Grid_Observaciones.PageIndex = 0;
            Grid_Observaciones.DataBind();
            Grid_Observaciones.Columns[1].Visible = false;
            Grid_Observaciones.Columns[2].Visible = false;
            Grid_Observaciones.Columns[4].Visible = false;
        }
    }

    private void Cargar_Datos_Avaluo(DataTable Dt_Avaluo)
    {
        if (Dt_Avaluo.Rows.Count > 0)
        {
            Hdf_No_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_No_Avaluo].ToString();
            Hdf_Anio_Avaluo.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Anio_Avaluo].ToString();
            Hdf_Cuenta_Predial_Id.Value = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Cuenta_Predial_Id].ToString();
            Cmb_Motivo_Avaluo.SelectedIndex = Cmb_Motivo_Avaluo.Items.IndexOf(Cmb_Motivo_Avaluo.Items.FindByValue(HttpUtility.HtmlDecode(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Motivo_Avaluo_Id].ToString())));
            Txt_No_Avaluo.Text = Dt_Avaluo.Rows[0]["AVALUO"].ToString();
            Txt_Cuenta_Predial.Text = Dt_Avaluo.Rows[0]["CUENTA_PREDIAL"].ToString();
            Txt_Clave_Catastral.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Clave_Catastral].ToString();
            Txt_Propietario.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Propietario].ToString();
            Cmb_Estatus.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Estatus].ToString();
            Txt_Domicilio_Not.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Domicilio_Notificacion].ToString();
            Txt_Municipio_Notificar.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Municipio_Notificacion].ToString();
            Txt_Ubicacion_Predio.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Ubicacion].ToString();
            Txt_Localidad.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Localidad_Municipio].ToString();
            Txt_Fecha.Text = Convert.ToDateTime(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Fecha_Creo].ToString()).ToString("dd/MMM/yyyy");
            Txt_Solicitante.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Solicitante].ToString();
            Txt_Nombre_Predio.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Nombre_Predio].ToString();
            Txt_X_Horas.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Coord_X_Grados].ToString();
            Txt_X_Minutos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Coord_X_Minutos].ToString();
            Txt_X_Segundos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Coord_X_Segundos].ToString();
            Cmb_Latitud.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Orientacion_X].ToString();
            Txt_Y_Horas.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Coord_Y_Grados].ToString();
            Txt_Y_Minutos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Coord_Y_Minutos].ToString();
            Txt_Y_Segundos.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Coord_Y_Segundos].ToString();
            Cmb_Longitud.SelectedValue = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Orientacion_Y].ToString();
            Txt_Observaciones.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Observaciones].ToString();
            //Txt_Base_Gravable.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Base_Gravable].ToString();
            //Txt_Impuesto_Bimestral.Text = Convert.ToDouble(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Impuesto_Bimestral].ToString()).ToString("#,###,###,###,###,##0.00");
            Txt_Valor_Total_Predio.Text = Convert.ToDouble(Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Valor_Total_Predio].ToString()).ToString("#,###,###,###,###,###,###,##0.00");
            Txt_Norte.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Coord_Norte].ToString();
            Txt_Sur.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Coord_Sur].ToString();
            Txt_Oriente.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Coord_Oriente].ToString();
            Txt_Poniente.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Coord_Poniente].ToString();
            Txt_Observaciones_Rechazo.Text = Dt_Avaluo.Rows[0][Ope_Cat_Avaluo_Rustico.Campo_Observaciones_Perito].ToString();
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

    protected void Grid_Descripcion_Terreno_DataBound(object sender, EventArgs e)
    {
        DataTable Dt_Caracteristicas = (DataTable)Session["Dt_Caracteristicas"];
        for (int i = 0; i < Dt_Caracteristicas.Rows.Count; i++)
        {
            CheckBox Chk_Indicador_Valor_A = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[5].FindControl("Chk_Indicador_Valor_A");
            CheckBox Chk_Indicador_Valor_B = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[7].FindControl("Chk_Indicador_Valor_B");
            CheckBox Chk_Indicador_Valor_C = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[9].FindControl("Chk_Indicador_Valor_C");
            CheckBox Chk_Indicador_Valor_D = (CheckBox)Grid_Descripcion_Terreno.Rows[i].Cells[11].FindControl("Chk_Indicador_Valor_D");
            if (Dt_Caracteristicas.Rows[i][Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A].ToString().Trim() != "")
            {
                if (Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_A].ToString().Trim() != "")
                {
                    Chk_Indicador_Valor_A.Checked = true;
                }
                else
                {
                    Chk_Indicador_Valor_A.Checked = true;
                }
            }
            else
            {
                Chk_Indicador_Valor_A.Visible = false;
                Chk_Indicador_Valor_A.Checked = false;
            }
            if (Dt_Caracteristicas.Rows[i][Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_B].ToString().Trim() != "")
            {
                if (Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_B].ToString().Trim() != "")
                {
                    Chk_Indicador_Valor_B.Checked = true;
                }
                else
                {
                    Chk_Indicador_Valor_B.Checked = true;
                }
            }
            else
            {
                Chk_Indicador_Valor_B.Visible = false;
                Chk_Indicador_Valor_B.Checked = false;
            }
            if (Dt_Caracteristicas.Rows[i][Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_C].ToString().Trim() != "")
            {
                if (Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_C].ToString().Trim() != "")
                {
                    Chk_Indicador_Valor_C.Checked = true;
                }
                else
                {
                    Chk_Indicador_Valor_C.Checked = true;
                }
            }
            else
            {
                Chk_Indicador_Valor_C.Visible = false;
                Chk_Indicador_Valor_C.Checked = false;
            }
            if (Dt_Caracteristicas.Rows[i][Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_D].ToString().Trim() != "")
            {
                if (Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_D].ToString().Trim() != "")
                {
                    Chk_Indicador_Valor_D.Checked = true;
                }
                else
                {
                    Chk_Indicador_Valor_D.Checked = true;
                }
            }
            else
            {
                Chk_Indicador_Valor_D.Visible = false;
                Chk_Indicador_Valor_D.Checked = false;
            }
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
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_A] = "X";
            }
            else
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_A] = "";
            }
            if (Chk_Indicador_Valor_B.Checked)
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_B] = "X";
            }
            else
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_B] = "";
            }
            if (Chk_Indicador_Valor_C.Checked)
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_C] = "X";
            }
            else
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_C] = "";
            }
            if (Chk_Indicador_Valor_D.Checked)
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_D] = "X";
            }
            else
            {
                Dt_Caracteristicas.Rows[i][Ope_Cat_Caracteristicas_Ara.Campo_Valor_Indicador_D] = "";
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




        if (Txt_X_Horas.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese los grados en X.";
            Valido = false;
        }
        if (Txt_X_Minutos.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese los Minutos en X.";
            Valido = false;
        }
        if (Txt_X_Segundos.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese los Segundos en X.";
            Valido = false;
        }
        if (Cmb_Latitud.SelectedValue == "SELECCIONE")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Seleccione la Orientación en X.";
            Valido = false;
        }

        if (Txt_Y_Horas.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese los grados en Y.";
            Valido = false;
        }
        if (Txt_Y_Minutos.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese los Minutos en Y.";
            Valido = false;
        }
        if (Txt_Y_Segundos.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese los Segundos en Y.";
            Valido = false;
        }
        if (Cmb_Longitud.SelectedValue == "SELECCIONE")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Seleccione la Orientación en Y.";
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
        if (Txt_Norte.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese las medidas y colindancias al Norte.";
            Valido = false;
        }
        if (Txt_Sur.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese las medidas y colindancias al Sur.";
            Valido = false;
        }
        if (Txt_Oriente.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese las medidas y colindancias al Oriente.";
            Valido = false;
        }

        if (Txt_Poniente.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese las medidas y colindancias al Poniente.";
            Valido = false;
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
        Double Valor_Total_Predio = 0;
        Double Precio_Avaluo = 0;
        Valor_Total_Predio = Convert.ToDouble(Txt_Valor_Total_Predio.Text);
        Precio_Avaluo = (((Valor_Total_Predio * Convert.ToDouble(Hdf_Factor_Cobro1.Value)) + Convert.ToDouble(Hdf_Base_Cobro.Value)) * Convert.ToDouble(Hdf_Factor_Cobro2.Value)) * (Convert.ToDouble(Hdf_Porcentaje_Cobro.Value) / 100);
        Txt_Precio_Avaluo.Text = Precio_Avaluo.ToString("###,###,###,###,##0.00");
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
            Txt_Croquis.Text = Dt_Valores_Construccion.Rows[i]["CROQUIS"].ToString();

            Dt_Valores_Construccion.Rows[i]["CROQUIS"] = Txt_Croquis.Text.ToUpper();
            Dt_Valores_Construccion.Rows[i]["SUPERFICIE_M2"] = Convert.ToDouble(Txt_Superficie_M2_Temporal.Text);
            Dt_Valores_Construccion.Rows[i]["FACTOR"] = Convert.ToDouble(Txt_Factor_Temporal.Text);
            Dt_Valores_Construccion.Rows[i]["VALOR_PARCIAL"] = Convert.ToDouble(Txt_Total_Temporal.Text);
            Dt_Valores_Construccion.Rows[i]["EDAD_CONSTRU"] = Convert.ToDouble(Txt_Edad.Text);
            Dt_Valores_Construccion.Rows[i]["USO_CONTRU"] = Txt_Uso.Text.ToUpper();
        }
    }

    protected void Btn_Agregar_Observacion_Click(object sender, ImageClickEventArgs e)
    {
        if (Cmb_Motivos_Rechazo.SelectedValue != "SELECCIONE")
        {
            DataTable Dt_Motivos_Rechazo = (DataTable)Session["Dt_Motivos_Rechazo"];
            Boolean Existe_Registro = false;
            if (Dt_Motivos_Rechazo.Rows.Count == 0)
            {
                DataRow Dr_Nuevo_Registro = Dt_Motivos_Rechazo.NewRow();
                Dr_Nuevo_Registro["NO_SEGUIMIENTO"] = " ";
                Dr_Nuevo_Registro["MOTIVO_ID"] = Cmb_Motivos_Rechazo.SelectedValue;
                Dr_Nuevo_Registro["ESTATUS"] = "VIGENTE";
                Dr_Nuevo_Registro["ACCION"] = "ALTA";
                Dr_Nuevo_Registro["MOTIVO_DESCRIPCION"] = Cmb_Motivos_Rechazo.SelectedItem.Text;
                Dt_Motivos_Rechazo.Rows.Add(Dr_Nuevo_Registro);
                Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
                Grid_Observaciones.Columns[1].Visible = true;
                Grid_Observaciones.Columns[2].Visible = true;
                Grid_Observaciones.Columns[4].Visible = true;
                Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
                Grid_Observaciones.PageIndex = Grid_Observaciones.PageIndex;
                Grid_Observaciones.DataBind();
                Grid_Observaciones.Columns[1].Visible = false;
                Grid_Observaciones.Columns[2].Visible = false;
                Grid_Observaciones.Columns[4].Visible = false;
                Grid_Observaciones.SelectedIndex = -1;
            }
            else
            {
                foreach (DataRow Dr_Renglon in Dt_Motivos_Rechazo.Rows)
                {
                    if (Dr_Renglon["MOTIVO_ID"].ToString() == Cmb_Motivos_Rechazo.SelectedValue && Dr_Renglon["ESTATUS"].ToString() != "BAJA")
                    {
                        Existe_Registro = true;
                    }
                }
                if (!Existe_Registro)
                {
                    DataRow Dr_Nuevo_Registro = Dt_Motivos_Rechazo.NewRow();
                    Dr_Nuevo_Registro["NO_SEGUIMIENTO"] = " ";
                    Dr_Nuevo_Registro["MOTIVO_ID"] = Cmb_Motivos_Rechazo.SelectedValue;
                    Dr_Nuevo_Registro["ESTATUS"] = "VIGENTE";
                    Dr_Nuevo_Registro["ACCION"] = "ALTA";
                    Dr_Nuevo_Registro["MOTIVO_DESCRIPCION"] = Cmb_Motivos_Rechazo.SelectedItem.Text;
                    Dt_Motivos_Rechazo.Rows.Add(Dr_Nuevo_Registro);
                    Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
                    Dt_Motivos_Rechazo.DefaultView.Sort = "ESTATUS DESC";
                    Grid_Observaciones.Columns[1].Visible = true;
                    Grid_Observaciones.Columns[2].Visible = true;
                    Grid_Observaciones.Columns[4].Visible = true;
                    Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
                    Grid_Observaciones.PageIndex = Grid_Observaciones.PageIndex;
                    Grid_Observaciones.DataBind();
                    Grid_Observaciones.Columns[1].Visible = false;
                    Grid_Observaciones.Columns[2].Visible = false;
                    Grid_Observaciones.Columns[4].Visible = false;
                    Grid_Observaciones.SelectedIndex = -1;
                }
            }
        }
    }

    protected void Grid_Observaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Observaciones.SelectedIndex > -1)
        {
            try
            {
                Hdf_Motivo_Id.Value = Grid_Observaciones.SelectedRow.Cells[2].Text;
                Cmb_Motivos_Rechazo.SelectedIndex = Cmb_Motivos_Rechazo.Items.IndexOf(Cmb_Motivos_Rechazo.Items.FindByValue(HttpUtility.HtmlDecode(Grid_Observaciones.SelectedRow.Cells[2].Text)));
            }
            catch (Exception ex)
            {
                Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
                Lbl_Ecabezado_Mensaje.Visible = true;
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    protected void Btn_Eliminar_Observacion_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Observaciones.SelectedIndex > -1)
        {
            DataTable Dt_Motivos_Rechazo = (DataTable)Session["Dt_Motivos_Rechazo"];
            int Indice = (Grid_Observaciones.PageIndex * 10) + Grid_Observaciones.SelectedIndex;
            Dt_Motivos_Rechazo.Rows[Indice]["ESTATUS"] = "BAJA";
            if (Dt_Motivos_Rechazo.Rows[Indice]["NO_SEGUIMIENTO"].ToString().Trim() == "")
            {
                Dt_Motivos_Rechazo.Rows[Indice]["ACCION"] = "ALTA";
            }
            else
            {
                Dt_Motivos_Rechazo.Rows[Indice]["ACCION"] = "ACTUALIZAR";
            }
            Session["Dt_Motivos_Rechazo"] = Dt_Motivos_Rechazo.Copy();
            Dt_Motivos_Rechazo.DefaultView.Sort = "ESTATUS DESC";
            Grid_Observaciones.Columns[1].Visible = true;
            Grid_Observaciones.Columns[2].Visible = true;
            Grid_Observaciones.DataSource = Dt_Motivos_Rechazo;
            Grid_Observaciones.PageIndex = Grid_Observaciones.PageIndex;
            Grid_Observaciones.DataBind();
            Grid_Observaciones.Columns[1].Visible = false;
            Grid_Observaciones.Columns[2].Visible = false;
            Grid_Observaciones.SelectedIndex = -1;
            if (Cmb_Estatus.SelectedValue == "POR VALIDAR")
            {
                Cambiar_Estatus_Avaluo_Rechazado_Autorizada();
            }
        }

    }
}
