using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Numalet;
using Presidencia.Sessiones;
using Presidencia.Operacion_Cat_Solicitud_Claves_Catastrales.Negocio;
using Presidencia.Catalogo_Cat_Claves_Catastrales.Negocio;
using Presidencia.Operacion_Cat_Solicitud_Claves_Catastrales.Datos;
using Presidencia.Registro_Peticion.Datos;
using System.Data.OracleClient;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Catalogo_Cat_Tabla_Valores_Catastrales.Negocio;
using CrystalDecisions.Shared;

public partial class paginas_Catastro_Frm_Ope_Cat_Seguimiento_Claves_Catastrales : System.Web.UI.Page
{
   
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN:
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 17/Julio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
                Tool_ScriptManager.RegisterPostBackControl(Btn_Aceptar);
                Tool_ScriptManager.RegisterPostBackControl(Txt_No_Claves_Catastrales);
                Btn_Aceptar.Attributes["onclick"] = "$get('" + Uprg_Reporte.ClientID + "').style.display='block'; return true;";
            if (!Page.IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Llenar_Tabla_Claves_Catastrales(0);
                Limpiar_Campos();
                Configurar_Formulario(true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error(Ex.Message);   
        }
        Lbl_Encabezado_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Memorias
    ///DESCRIPCIÓN: Llena el Grid de las memorias descriptivas
    ///PROPIEDADES:
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    ///
    private void Llenar_Tabla_Claves_Catastrales(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio Peritos_Externos = new Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio();
            DataTable Dt_Peritos_Externos;
            Dt_Peritos_Externos = Peritos_Externos.Consultar_Solicitud_Claves_Catastrales();
            Grid_Claves_Catastrales.Columns[1].Visible = true;
            Grid_Claves_Catastrales.Columns[2].Visible = true;
            Grid_Claves_Catastrales.Columns[8].Visible = true;
            Grid_Claves_Catastrales.Columns[9].Visible = true;
            Grid_Claves_Catastrales.DataSource = Dt_Peritos_Externos;
            Grid_Claves_Catastrales.PageIndex = Pagina;
            Grid_Claves_Catastrales.DataBind();
            Grid_Claves_Catastrales.Columns[1].Visible = true;
            Grid_Claves_Catastrales.Columns[2].Visible = false;
            Grid_Claves_Catastrales.Columns[8].Visible = false;
            Grid_Claves_Catastrales.Columns[9].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = E.Message;
            Mostrar_Mensaje_Error(E.Message);
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

    private void Configurar_Formulario(Boolean Enable)
    {
        Txt_Observaciones.Enabled = Enable;
        Txt_No_Claves_Catastrales.Enabled = Enable;
        Txt_No_Claves_Catastrales.Style["text-align"] = "Right";
        Txt_Cuenta_Predial.Enabled = !Enable;
        Txt_Correo.Enabled = !Enable;
        Txt_Solicitante.Enabled = !Enable;
        Cmb_Estatus.Enabled = !Enable;
        Cmb_Documento.Enabled = !Enable;
        Cmb_Tipo.Enabled = !Enable;
        Fup_Documento.Enabled = !Enable;        
        Txt_Calculo_Valores_Claves_Catastrales.Enabled = !Enable;
        Txt_Calculo_Valores_Claves_Catastrales.Style["text-align"] = "Right";
        Txt_Calculo_Valores_Claves_Catastrales.Text = "0.00";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Campos
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    
    private void Limpiar_Campos()
    {
        Txt_Observaciones.Text = "";
        Txt_No_Claves_Catastrales.Text = "";
        Txt_Correo.Text = "";
        Txt_Solicitante.Text = "";
        Cmb_Estatus.SelectedIndex = -1;
        Cmb_Documento.SelectedIndex = -1;
        Cmb_Tipo.SelectedIndex = -1;
        Txt_Cuenta_Predial.Text = "";
        Hdf_Cuenta_Predial_Id.Value = "";
        Txt_Calculo_Valores_Claves_Catastrales.Text = "0.00";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Peritos_Externos_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la página del grid
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Clave_Catastral_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try{
            Llenar_Tabla_Claves_Catastrales(e.NewPageIndex);
            Grid_Claves_Catastrales.SelectedIndex = -1;//
        }
        catch (Exception E)
        {
            Mostrar_Mensaje_Error(E.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Peritos_Externos_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un registro del grid y toma los datos de los mismos campos del componente
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Clave_Catastral_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Anio;
        if (Grid_Claves_Catastrales.SelectedIndex > -1)
        {   
            Txt_Solicitante.Text = HttpUtility.HtmlDecode(Grid_Claves_Catastrales.SelectedRow.Cells[1].Text);
            Txt_Correo.Text = HttpUtility.HtmlDecode(Grid_Claves_Catastrales.SelectedRow.Cells[2].Text);
            Anio = HttpUtility.HtmlDecode(Grid_Claves_Catastrales.SelectedRow.Cells[3].Text);
            Txt_No_Claves_Catastrales.Text = HttpUtility.HtmlDecode(Grid_Claves_Catastrales.SelectedRow.Cells[4].Text);
            Cmb_Tipo.SelectedValue = HttpUtility.HtmlDecode(Grid_Claves_Catastrales.SelectedRow.Cells[5].Text);
            Txt_Observaciones.Text = Grid_Claves_Catastrales.SelectedRow.Cells[7].Text;
            if (Txt_Observaciones.Text == "&nbsp;")
            {
                Txt_Observaciones.Text="";
                
            }
            
            Hdf_Cuenta_Predial_Id.Value = HttpUtility.HtmlDecode(Grid_Claves_Catastrales.SelectedRow.Cells[8].Text);
            Hdf_Clave_Catastral_Id.Value = HttpUtility.HtmlDecode(Grid_Claves_Catastrales.SelectedRow.Cells[9].Text);
            Txt_Cuenta_Predial.Text = HttpUtility.HtmlDecode(Grid_Claves_Catastrales.SelectedRow.Cells[10].Text);
            Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio Claves_Catastrales = new Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio();
            Claves_Catastrales.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
            Hdf_Estatus_Clave_Catastral.Value = Grid_Claves_Catastrales.SelectedRow.Cells[6].Text;
            Claves_Catastrales.P_No_Claves_Catastrales = Grid_Claves_Catastrales.SelectedRow.Cells[9].Text;
            Claves_Catastrales.P_Anio = Grid_Claves_Catastrales.SelectedRow.Cells[3].Text;
            Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio Factore = new Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio();
            Factore.P_Anio = Hdf_Anio.Value;
            DataTable Dt_Documentos = Factore.Consultar_Tabla_Valores_Catastrales();
            Hdf_Cantidad_Cobro1.Value =Dt_Documentos.Rows[0]["CANTIDAD_1"].ToString();
            Hdf_Cantidad_Cobro2.Value = Dt_Documentos.Rows[0]["CANTIDAD_2"].ToString();
            if (Txt_No_Claves_Catastrales.Text.Trim() == "")
            {
                Txt_No_Claves_Catastrales.Text = "0";
            }
            
                Txt_Calculo_Valores_Claves_Catastrales.Text = ((Convert.ToDouble(Hdf_Cantidad_Cobro1.Value)
                    * Convert.ToDouble(Txt_No_Claves_Catastrales.Text.Trim()))
                    + Convert.ToDouble(Hdf_Cantidad_Cobro2.Value)).ToString("#,###,##0.00");
            
            Dt_Documentos = Claves_Catastrales.Consultar_Documentos_Claves_Catastrales();
            Session["Dt_Documentos"] = Dt_Documentos.Copy();
            Grid_Documentos.Columns[0].Visible = true;
            Grid_Documentos.Columns[1].Visible = true;
            Grid_Documentos.Columns[2].Visible = true;
            Grid_Documentos.DataSource = Dt_Documentos;
            Grid_Documentos.DataBind();
            Grid_Documentos.Columns[0].Visible = false;
            Grid_Documentos.Columns[1].Visible = false;
            Grid_Documentos.Columns[2].Visible = false;
            
            Btn_Salir.AlternateText = "Atras"; 
            Div_Grid_Datos_Clave.Visible = true;
            Div_Detalles.Visible = true;
            
        }
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Grid_Documentos_SelectedIndexChanged
    ///DESCRIPCION : 
    ///PARAMETROS  : object sender, EventArgs e
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************    
    protected void Grid_Documentos_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Eliminar registro y archivo en caso de tenerlo
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        foreach (DataRow Dr_Documento in Dt_Documentos.Rows)
        {
            if (Dr_Documento["DOCUMENTO"].ToString() == Grid_Documentos.SelectedRow.Cells[3].Text && Dr_Documento["ACCION"].ToString() != "BAJA")
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
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mostrar_Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mostrar_Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Encabezado_Error.Text = "";
        Lbl_Encabezado_Error.Text = P_Mensaje + "</br>";
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Limpia_Mensaje_Error
    ///DESCRIPCION : Limpia el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Limpia_Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Encabezado_Error.Text = "";
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
        if (Dt_Documentos != null)
        {
            foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
            {
                if (Dr_Renglon["ACCION"].ToString() == "NADA")
                {
                    if (File.Exists(Server.MapPath(Dr_Renglon["RUTA_DOCUMENTO"].ToString())))
                    {
                        HyperLink Hlk_Enlace = new HyperLink();
                        Hlk_Enlace.Text = Path.GetFileName(Dr_Renglon["RUTA_DOCUMENTO"].ToString());
                        Hlk_Enlace.NavigateUrl = Dr_Renglon["RUTA_DOCUMENTO"].ToString();
                        Hlk_Enlace.CssClass = "enlace_fotografia";
                        Hlk_Enlace.Target = "blank";
                        Grid_Documentos.Rows[i].Cells[4].Controls.Add(Hlk_Enlace);
                        i++;
                    }
                }
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Documentos(int Pagina)
    ///DESCRIPCIÓN: Evento del botón Agregar Documento
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    ///
    private void Llenar_Tabla_Documentos(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio Documentos = new Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio();
            DataTable Dt_Documentos;
            Documentos.P_Ruta_Documento = "/" + DateTime.Now.Year + "/";
            Dt_Documentos = Documentos.Consultar_Documentos_Claves_Catastrales();
            Session["Dt_Documentos"] = Dt_Documentos.Copy();
            Grid_Documentos.Columns[0].Visible = true;
            
            Grid_Documentos.DataSource = Dt_Documentos;
            Grid_Documentos.PageIndex = Pagina;
            Grid_Documentos.DataBind();
            Grid_Documentos.Columns[0].Visible = false;
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error(Ex.Message);
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
            Configurar_Formulario(true);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Llenar_Tabla_Claves_Catastrales(Grid_Claves_Catastrales.PageIndex);
            Limpiar_Campos();
            Session["Dt_Documentos"] = null;
            Grid_Documentos.DataSource = null;
            Grid_Documentos.DataBind();
            Txt_Calculo_Valores_Claves_Catastrales.Text = "";
            
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
    ///
    private void Enviar_Correo_Cuenta()
    {
        String Contenido = "";
        Contenido = "La Solicitud de Claves Catastrales ha Sido Revisada, y a Resultado AUTORIZADA Para Atenderse Quedando Como Folio de pago: PE"
            + Hdf_Clave_Catastral_Id.Value;
        Contenido += "<br/>";
        Contenido += "<br/>";
        Contenido += Txt_Observaciones.Text.ToUpper();
        try
        {
            if (Txt_Correo.Text.Trim().Length > 0)
            {
                Cls_Mail mail = new Cls_Mail();
                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = Txt_Correo.Text.Trim();
                mail.P_Subject = "Solicitud Aceptada";
                mail.P_Texto = Contenido;
                mail.P_Adjunto = null;//Hacer_Pdf();
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error("No se pudo enviar el Correo." + Ex.Message );
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo_Cuenta_Rechazado
    ///DESCRIPCIÓN: Envia un correo al correo del perito externo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Enviar_Correo_Cuenta_Rechazado()
    {
        String Contenido = "";
        Contenido = "Su solicitud ha sido Rechazada";
        Contenido += "<br/>";
        Contenido += "<br/>";
        Contenido += Txt_Correo.Text.ToUpper();
        try
        {
            if (Txt_Correo.Text.Trim().Length > 0)
            {
                Cls_Mail mail = new Cls_Mail();
                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = Txt_Correo.Text.Trim();
                mail.P_Subject = "Solicitud Rechazada";
                mail.P_Texto = Contenido;
                mail.P_Adjunto = null;//Hacer_Pdf();
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error("No se pudo enviar el Correo." + Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_No_Claves_Catastrales_TextChanged(object sender, EventArgs e)
    {
       Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio Factores = new Cls_Cat_Cat_Tabla_Valores_Catastrales_Negocio();
       DataTable Dt_Documentos = Factores.Consultar_Tabla_Valores_Catastrales();
       Hdf_Cantidad_Cobro1.Value =Dt_Documentos.Rows[0]["CANTIDAD_1"].ToString();
       Hdf_Cantidad_Cobro2.Value = Dt_Documentos.Rows[0]["CANTIDAD_2"].ToString();
       if (Txt_No_Claves_Catastrales.Text.Trim() == "")
       {
        Txt_No_Claves_Catastrales.Text = "0";
       }
       Txt_Calculo_Valores_Claves_Catastrales.Text = ((Convert.ToDouble(Hdf_Cantidad_Cobro1.Value)
       * Convert.ToDouble(Txt_No_Claves_Catastrales.Text.Trim())) + Convert.ToDouble(Hdf_Cantidad_Cobro2.Value)).ToString();

       DataTable Dt_Documentos2 = (DataTable)Session["Dt_Documentos"];
       Grid_Documentos.DataSource = Dt_Documentos2;
       Grid_Documentos.DataBind();
    }
         
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    ///
    private String Obtener_Dato_Consulta(String Consulta)
    {
        String Dato_Consulta = "";
        try{
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
    ///NOMBRE DE LA FUNCIÓN : Insertar_Pasivo
    ///DESCRIPCIÓN          : Consulta el Costo del Documento y lo Inserta en Pasivo
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    ///
   private void Insertar_Pasivo (String Referencia)
    {
     try
     {
         Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo_Impuestro_Trasladado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
         String Clave_Ingreso_Id="";
         String Dependencia_Id="";
         String Consulta= "SELECT " + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso
             + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " LIKE '%ASIGNACION CLAVE CATASTRAL%'";
         Clave_Ingreso_Id = Obtener_Dato_Consulta(Consulta);
         if(Clave_Ingreso_Id.Trim() != ""){
             Consulta = "SELECT " + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso
                 + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " LIKE '%ASIGNACION CLAVE CATASTRAL%'";
             Calculo_Impuestro_Trasladado.P_Referencia = Referencia;
             Calculo_Impuestro_Trasladado.P_Descripcion = "CLAVES CATASTRALES";
             Calculo_Impuestro_Trasladado.P_Estatus = "POR PAGAR";
             Calculo_Impuestro_Trasladado.P_Clave_Ingreso_ID = Clave_Ingreso_Id;
             Calculo_Impuestro_Trasladado.P_Dependencia_ID = Dependencia_Id;
             Calculo_Impuestro_Trasladado.P_Monto_Total_Pagar = Convert.ToDouble(Txt_Calculo_Valores_Claves_Catastrales.Text).ToString("0.00");
             Calculo_Impuestro_Trasladado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
             Calculo_Impuestro_Trasladado.P_Cuenta_Predial_ID = "";
             Calculo_Impuestro_Trasladado.P_Contribuyente = Txt_Solicitante.Text + " " + Txt_Correo.Text;
             Calculo_Impuestro_Trasladado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.AddDays(15).ToString("dd/MMM/yyyy");
             Calculo_Impuestro_Trasladado.Alta_Pasivo();
         }
         else
         {

         }
     }
     catch (Exception Ex)
     {
         Mostrar_Mensaje_Error("No se Puede Insertar el Pasivo" + Ex.Message);
     }

    }
   ///******************************************************************************* 
   ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Ope_Cat_Seguimiento_Claves_Catastrales
   ///DESCRIPCIÓN          : Crea un DataSet para el reporte de memorias descriptivas
   ///PARAMETROS: 
   ///CREO                 : Miguel Angel Bedolla Moreno
   ///FECHA_CREO           : 22/Junio/2012
   ///MODIFICO: 
   ///FECHA_MODIFICO:
   ///CAUSA_MODIFICACIÓN:
   ///*******************************************************************************
   private DataSet Crear_Ds_Ope_Cat_Seguimiento_Claves_Catastrales()
   {
       Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio Solicitud = new Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio();
       Cls_Cat_Cat_Claves_Catastrales_Negocio Claves_Catastrales = new Cls_Cat_Cat_Claves_Catastrales_Negocio();
       Ds_Ope_Cat_Seguimiento_Claves_Catastrales Folio_Claves= new Ds_Ope_Cat_Seguimiento_Claves_Catastrales();
       Solicitud.P_No_Claves_Catastrales = Hdf_Clave_Catastral_Id.Value;
       Solicitud.P_Anio = HttpUtility.HtmlDecode(Grid_Claves_Catastrales.SelectedRow.Cells[3].Text);
       DataTable Dt_Solicitudes_Claves_Catastrales = Solicitud.Consultar_Solicitud_Claves_Catastrales();
       Claves_Catastrales.P_Claves_Catastrales_ID = Hdf_Clave_Catastral_Id.Value;
       Numalet Cantidad = new Numalet();
       Cantidad.MascaraSalidaDecimal = "00/100 M.N.";
       Cantidad.SeparadorDecimalSalida = "Pesos";
       Cantidad.ApocoparUnoParteEntera = true;
       Cantidad.LetraCapital = true;
       DataTable Dt_Folio_Claves_Catastrales_Ds = Folio_Claves.Tables["DT_SEGUIMIENTO_SOLICITUD_CLAVES_CATASTRALES"];
       DataRow Dr_Renglon_Nuevo = Dt_Folio_Claves_Catastrales_Ds.NewRow();
       Dr_Renglon_Nuevo["SOLICITANTE"] = Txt_Solicitante.Text;
       Dr_Renglon_Nuevo["NUMERO_CLAVES_CATASTRALES"] = Txt_No_Claves_Catastrales.Text;
       Dr_Renglon_Nuevo["OBSERVACIONES"] = Txt_Observaciones.Text;
       Dr_Renglon_Nuevo["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
       Dr_Renglon_Nuevo["IMPORTE"] = "$" + Convert.ToDouble(Txt_Calculo_Valores_Claves_Catastrales.Text).ToString("#,###,##0.00");
       //Dr_Renglon_Nuevo["TIPO"] = Cmb_Tipo.SelectedValue;
       if (Cmb_Tipo.SelectedValue == "AC")
       {
           Dr_Renglon_Nuevo["TIPO"] = "ASIGNACIÓN DE CLAVES CATASTRALES";
       }
       if (Cmb_Tipo.SelectedValue == "MC")
       {
           Dr_Renglon_Nuevo["TIPO"] = "MODIFICACIÓN DE CLAVES CATASTRALES";
       }
       if (Cmb_Tipo.SelectedValue == "ACC")
       {
           Dr_Renglon_Nuevo["TIPO"] = "ASIGNACION DE CÉDULA CATASTRAL";
       }

       if (Cmb_Tipo.SelectedValue == "MCC")
       {
           Dr_Renglon_Nuevo["TIPO"] = "MODIFICACIÓN DE CÉDULA CATASTRAL";
       }

       Dr_Renglon_Nuevo["FOLIO"] = ("CC" + Convert.ToInt32(Hdf_Clave_Catastral_Id.Value)).ToString();
       Session["E_Mail"] = Dt_Solicitudes_Claves_Catastrales.Rows[0][Ope_Cat_Claves_Catastrales.Campo_Correo].ToString();
       Dt_Folio_Claves_Catastrales_Ds.Rows.Add(Dr_Renglon_Nuevo);
       return Folio_Claves;
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
           Lbl_Encabezado_Error.Visible = true;
           Lbl_Encabezado_Error.Text = "No se pudo cargar el reporte para su impresion";
       }
       String Archivo_PDF = Nombre_Archivo + ".pdf"; // Nombre del PDF que ve a generar
       try
       {
           ExportOptions Export_Options = new ExportOptions();
           DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
           Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/"+ Archivo_PDF);
           Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
           Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
           Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
           Reporte.Export(Export_Options);
       }
       catch (Exception Ex)
       {
           Lbl_Encabezado_Error.Visible = true;
           Lbl_Encabezado_Error.Text = "NO se pudo cargar el reporte" + Ex.Message;
       }
       try
       {
           Enviar_Correo_Cuenta_Reporte((String)Session["E_Mail"], Server.MapPath("../../Reporte/" + Archivo_PDF));
           Session.Remove("E_Mail");
       }catch(Exception Ex)
       {
           Lbl_Encabezado_Error.Visible = true;
           Lbl_Encabezado_Error.Text = Ex.Message;
        }
   }
   ///******************************************************************************* 
   ///NOMBRE DE LA FUNCIÓN : Enviar_Correo_Cuenta_Reporte
   ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
   ///PARAMETROS: 
   ///CREO                 : Antonio Salvador Benavides Guardado
   ///FECHA_CREO           : 29/Julio/2011
   ///MODIFICO: 
   ///FECHA_MODIFICO:
   ///CAUSA_MODIFICACIÓN:
   ///*******************************************************************************
   private void Enviar_Correo_Cuenta_Reporte(String E_Mail, String Url_Adjunto)
   {
       String Contenido = "";
       Contenido = "Su solicitud de claves catastrales ha sido autorizada. Favor de pasar a pagar en las cajas de Presidencia, su folio de pago se encuentra adjunto a este correo. Favor imprimirlo dos veces";
       try
       {
           if (E_Mail.Trim().Length > 0)
           {
               Cls_Mail mail = new Cls_Mail();
               mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Servidor_Correo].ToString();
               mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Correo_Saliente].ToString();
               mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Password_Correo].ToString();
               mail.P_Recibe = E_Mail.Trim();
               mail.P_Subject = "Claves Catastrales autorizadas";
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Click
    ///DESCRIPCIÓN: Evento del botón Cancelar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cancelar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Claves_Catastrales.SelectedIndex > -1)
            {
                if ((Txt_Observaciones.Text.Trim() != "") && (Txt_No_Claves_Catastrales.Text.Trim() != ""))
                {
                    Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio Recepcion = new Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio();
                    Recepcion.P_Estatus = Grid_Claves_Catastrales.SelectedRow.Cells[6].Text;
                    
                        if (Recepcion.P_Estatus == "VIGENTE")
                        {
                            Recepcion.P_No_Claves_Catastrales = Hdf_Clave_Catastral_Id.Value;
                            Recepcion.P_Anio = HttpUtility.HtmlDecode(Grid_Claves_Catastrales.SelectedRow.Cells[3].Text).Trim();
                            Recepcion.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                            Recepcion.P_Solicitante = Txt_Solicitante.Text.ToUpper();
                            Recepcion.P_Correo = Txt_Correo.Text.ToLower();
                            Recepcion.P_Estatus = "RECHAZADA";
                            if ((Recepcion.Modificar_Solicitud_Claves_Catastrales()))
                            {
                                if (Hdf_Estatus_Clave_Catastral.Value == "VIGENTE")
                                {
                                    Enviar_Correo_Cuenta_Rechazado();
                                    Configurar_Formulario(true);
                                    Btn_Salir_Click(null, null);
                                    Grid_Documentos.SelectedIndex = -1;
                                    Grid_Claves_Catastrales.SelectedIndex = -1;
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud de Registro", "alert('Solicitud Rechazada.');", true);
                                    Llenar_Tabla_Claves_Catastrales(Grid_Claves_Catastrales.PageIndex);
                                }
                                else
                                {
                                    Btn_Cancelar_Click(null, null);
                                    Grid_Claves_Catastrales.SelectedIndex = -1;
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud de Registro Cancelada", "alert('La solicitud ya ha sido Registro.');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud de Registro", "alert('Error  al Intentar Rechazar.');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud No Autorizada", "alert('Solicitud No Vigente.');", true);
                        }
                }
                else
                {
                    Btn_Cancelar_Click(null, null);
                    Grid_Claves_Catastrales.SelectedIndex = -1;
                    Configurar_Formulario(true);
                }

            }
            else
            {
                Mostrar_Mensaje_Error("Seleccione una Solicitud de Clave Catastral");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Seleccione una Solicitud de Clave Catastral", "alert('Seleccione una Solicitud de Clave Catastral');", true);
            }
    }
    catch (Exception E)
    {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = E.Message;
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
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Claves_Catastrales.SelectedIndex > -1)
            {

                if (Txt_No_Claves_Catastrales.Text.Trim() != "" && Txt_Observaciones.Text.Trim() !="" && Txt_Calculo_Valores_Claves_Catastrales.Text.Trim() !="" )
                {
                    Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio Recepcion = new Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio();
                    Recepcion.P_Estatus = Grid_Claves_Catastrales.SelectedRow.Cells[6].Text;

                    if (Recepcion.P_Estatus == "VIGENTE")
                    {
                        Insertar_Pasivo("CC" + Convert.ToInt32(Hdf_Clave_Catastral_Id.Value));
                        Imprimir_Reporte(Crear_Ds_Ope_Cat_Seguimiento_Claves_Catastrales(),"Rpt_Ope_Cat_Seguimiento_Claves_Catastrales.rpt","Rpt_Ope_Cat_Seguimiento_Claves_Catastrales","Window_Frm","Memorias");
                        Recepcion.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                        Recepcion.P_Cantidad_Claves_Catastrales = Txt_No_Claves_Catastrales.Text.Trim();
                        Recepcion.P_No_Claves_Catastrales = Hdf_Clave_Catastral_Id.Value;
                        Recepcion.P_Correo = Txt_Correo.Text.ToLower();
                        Recepcion.P_Solicitante = Txt_Solicitante.Text.ToUpper();
                        Recepcion.P_Anio = HttpUtility.HtmlDecode(Grid_Claves_Catastrales.SelectedRow.Cells[3].Text).Trim();
                        Recepcion.P_Estatus = "AUTORIZADA";
                        Llenar_Tabla_Claves_Catastrales(Grid_Claves_Catastrales.PageIndex);
                        Txt_Calculo_Valores_Claves_Catastrales.Text = "";
                        if ((Recepcion.Modificar_Solicitud_Claves_Catastrales()))
                        {
                            Configurar_Formulario(true);
                            Grid_Documentos.SelectedIndex = -1;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud de Registro", "alert('Solicitud Autorizada.');", true);
                            Llenar_Tabla_Claves_Catastrales(Grid_Claves_Catastrales.PageIndex);
                        }
                        else
                        {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud de Registro", "alert('Solicitud al Registro.');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud No Autorizada", "alert('solicitud No Vigente.');", true);
                    }
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud de Registro", "alert('Ingrese Observaciones, Numero de Claves Catastrales e Importe del Tramite');", true);
                }
            }
            else
            {
                
                Mostrar_Mensaje_Error("Seleccione una Solicitud de Clave Catastral");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Seleccione una Solicitud de Clave Catastral", "alert('Seleccione una Solicitud de Clave Catastral');", true);
            }
        }
        catch (Exception E)
        {
            
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = E.Message;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Valida que los componentes tengan los datos necesarios para ingresarlos a la BD.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes()
    {
        String Mensaje_Error = "Error: ";
        Boolean valido = true;

        if (Txt_No_Claves_Catastrales.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese Numero por favor.";
            valido = false;
        }

        if (Txt_Observaciones.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese Observaciones.";
            valido = false;
        }
        if (!valido)
        {
            Lbl_Encabezado_Error.Text = Mensaje_Error;
            Lbl_Encabezado_Error.Text = "";
        }
        return valido;
    }

   
    
}

