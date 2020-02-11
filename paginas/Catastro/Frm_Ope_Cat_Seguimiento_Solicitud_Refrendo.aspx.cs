using System;
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
using Presidencia.Operacion_Cat_Recepcion_Documentos_Perito_Externo.Negocio;
using Presidencia.Constantes;
using Presidencia.Registro_Peticion.Datos;
using SharpContent.ApplicationBlocks.Data;

using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Sessiones;
using System.IO;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Numalet;
using System.Data.OracleClient;




public partial class paginas_Frm_Ope_Cat_Seguimiento_Solicitud_Refrendo : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN:
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);
                Llenar_Tabla_Peritos_Externos(0);
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Observaciones.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message);
        }
        Limpia_Mensaje_Error();
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
    private void Llenar_Tabla_Peritos_Externos(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Peritos_Externos = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
            DataTable Dt_Peritos_Externos;
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Peritos_Externos.P_Nombre = Txt_Busqueda.Text.ToUpper();
            }
            Peritos_Externos.P_Estatus = "IN ('VALIDAR','REFRENDO')";
            Dt_Peritos_Externos = Peritos_Externos.Consultar_Peritos_Externos();
            Grid_Peritos_Externos.Columns[1].Visible = true;
            Grid_Peritos_Externos.DataSource = Dt_Peritos_Externos;
            Grid_Peritos_Externos.PageIndex = Pagina;
            Grid_Peritos_Externos.DataBind();
            Grid_Peritos_Externos.Columns[1].Visible = false;
        }
        catch (Exception E)
        {
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
    private void Configuracion_Formulario(Boolean Enabled)
    {
        Cmb_Estatus.Enabled = !Enabled;
        Txt_Observaciones.Enabled = true;
        Txt_Busqueda.Enabled = Enabled;
        Btn_Buscar.Enabled = Enabled;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Campos
    ///DESCRIPCIÓN: Limpia todos los campos del formulario
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Campos()
    {
        Txt_Apellido_Materno.Text = "";
        Txt_Apellido_Paterno.Text = "";
        Txt_Calle.Text = "";
        Txt_Celular.Text = "";
        Txt_Ciudad.Text = "";
        Txt_Colonia.Text = "";
        Txt_Estado.Text = "";
        Txt_Nombre.Text = "";
        Txt_Observaciones.Text = "";
        Txt_Telefono.Text = "";
        Txt_Busqueda.Text = "";
        Hdf_Perito_Externo_Id.Value = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Evento del botón buscar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        if (Div_Grid_Peritos.Visible == true)
            Llenar_Tabla_Peritos_Externos(0);
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
    protected void Grid_Peritos_Externos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Tabla_Peritos_Externos(e.NewPageIndex);
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
    protected void Grid_Peritos_Externos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Peritos_Externos.SelectedIndex > -1)
        {
            Hdf_Perito_Externo_Id.Value = Grid_Peritos_Externos.SelectedRow.Cells[1].Text;
            Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Perito_Externo = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
            DataTable Dt_Perito_Externo;
            Perito_Externo.P_Perito_Externo_Id = Hdf_Perito_Externo_Id.Value;
            Dt_Perito_Externo = Perito_Externo.Consultar_Peritos_Externos();
            Txt_Apellido_Materno.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Materno].ToString();
            Txt_Apellido_Paterno.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno].ToString();
            Txt_Calle.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Calle].ToString();
            Txt_Celular.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Celular].ToString();
            Txt_Ciudad.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Ciudad].ToString();
            Txt_Colonia.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Colonia].ToString();
            Txt_Estado.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Estado].ToString();
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Estatus].ToString()));
            Hdf_Estatus_Perito_Externo.Value = Cmb_Estatus.SelectedValue;
            Txt_Nombre.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Nombre].ToString();
            Txt_Informacion_Adicional.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Observaciones].ToString();
            Txt_Telefono.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Telefono].ToString();
            Txt_E_Mail.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Usuario].ToString();
            Btn_Salir.AlternateText = "Atras";
            Div_Grid_Peritos.Visible = false;
            Div_Grid_Datos_Peritos.Visible = true;
            Div_Detalles.Visible = true;
            Llenar_Tabla_Documentos(0);
        }
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
    private void Llenar_Tabla_Documentos(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Documentos = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
            DataTable Dt_Documentos;
            Documentos.P_Perito_Externo_Id = Hdf_Perito_Externo_Id.Value;
            Documentos.P_Ruta_Documento = "/" + DateTime.Now.Year + "/";
            Dt_Documentos = Documentos.Consultar_Documentos_Perito_Externo();
            Session["Dt_Documentos"] = Dt_Documentos.Copy();
            Grid_Documentos.Columns[0].Visible = true;
            Grid_Documentos.Columns[1].Visible = true;
            Grid_Documentos.DataSource = Dt_Documentos;
            Grid_Documentos.PageIndex = Pagina;
            Grid_Documentos.DataBind();
            Grid_Documentos.Columns[0].Visible = false;
            Grid_Documentos.Columns[1].Visible = false;
        }
        catch (Exception E)
        {
            Mostrar_Mensaje_Error(E.Message);
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
            Limpiar_Campos();
            //Btn_Modificar.Visible = true;
            //Btn_Modificar.AlternateText = "Modificar";
            //Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(true);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Llenar_Tabla_Documentos(0);
            Llenar_Tabla_Peritos_Externos(Grid_Peritos_Externos.PageIndex);
            Session["Dt_Documentos"] = null;
            Div_Detalles.Visible = false;
            Div_Grid_Datos_Peritos.Visible = false;
            Div_Grid_Peritos.Visible = true;
        }
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    /////DESCRIPCIÓN: Evento del botón modificar
    /////PROPIEDADES:     
    /////            
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 05/May_2012
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        if (Grid_Peritos_Externos.SelectedIndex > -1)
    //        {
    //            if (Btn_Modificar.AlternateText.Equals("Modificar"))
    //            {
    //                if (Grid_Documentos.Rows.Count > 0)
    //                {
    //                    Configuracion_Formulario(false);
    //                    Btn_Modificar.AlternateText = "Actualizar";
    //                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
    //                    Btn_Salir.AlternateText = "Cancelar";
    //                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
    //                    Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Documentos = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
    //                    DataTable Dt_Documentos;
    //                    Documentos.P_Temp_Perito_Externo_Id = Hdf_Perito_Externo_Id.Value;
    //                    Dt_Documentos = Documentos.Consultar_Documentos();
    //                    Session["Dt_Documentos"] = Dt_Documentos.Copy();
    //                    Grid_Documentos.Columns[0].Visible = true;
    //                    Grid_Documentos.Columns[1].Visible = true;
    //                    Grid_Documentos.DataSource = Dt_Documentos;
    //                    Grid_Documentos.PageIndex = 0;
    //                    Grid_Documentos.DataBind();
    //                    Grid_Documentos.Columns[0].Visible = false;
    //                    Grid_Documentos.Columns[1].Visible = false;
    //                }
    //                else
    //                {
    //                    Mostrar_Mensaje_Error("Imposible modificar.");
    //                }
    //            }
    //            else
    //            {
    //                Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Recepcion = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
    //                Recepcion.P_Temp_Perito_Externo_Id = Hdf_Perito_Externo_Id.Value;
    //                Recepcion.P_Estatus = Cmb_Estatus.SelectedValue;
    //                if ((Recepcion.Modificar_Estatus_Perito_Externo_Temporal()))
    //                {
    //                    if (Hdf_Estatus_Perito_Externo.Value == "VALIDAR" && Cmb_Estatus.SelectedValue=="POR PAGAR")
    //                    {
    //                        Enviar_Correo_Cuenta();
    //                        //Insertar Pasivo
    //                        Insertar_Pasivo("PE" + Convert.ToInt16(Hdf_Perito_Externo_Id.Value));
    //                    }
    //                    Configuracion_Formulario(true);
    //                    Grid_Documentos.SelectedIndex = -1;
    //                    Btn_Salir_Click(null, null);
    //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud de Registro", "alert('Actualización Exitosa.');", true);
    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud de Registro", "alert('Error al Actualizar.');", true);
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        Mostrar_Mensaje_Error(Ex.Message);
    //    }
    //}

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
    private void Enviar_Correo_Cuenta()
    {
        String Contenido = "";
        Contenido = "La Solicitud ya fue revisada y ha sido AUTORIZADA para atenderse quedando como Folio de pago: PE" + Convert.ToInt16(Hdf_Perito_Externo_Id.Value);
        Contenido += "<br/>";
        Contenido += "<br/>";
        Contenido += Txt_Observaciones.Text.ToUpper();

        try
        {
            if (Txt_E_Mail.Text.Trim().Length > 0)
            {
                Cls_Mail mail = new Cls_Mail();
                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = Txt_E_Mail.Text.Trim();
                mail.P_Subject = "Solicitud Aceptada";
                mail.P_Texto = Contenido;
                mail.P_Adjunto = null;//Hacer_Pdf();
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error("No se pudo enviar el Correo.");
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
    private void Enviar_Correo_Cuenta_Rechazado()
    {
        String Contenido = "";
        Contenido = "Su solicitud ha sido Rechazada";
        Contenido += "<br/>";
        Contenido += "<br/>";

        try
        {
            if (Txt_E_Mail.Text.Trim().Length > 0)
            {
                Cls_Mail mail = new Cls_Mail();
                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = Txt_E_Mail.Text.Trim();
                mail.P_Subject = "Solicitud Rechazada";
                mail.P_Texto = Contenido;
                mail.P_Adjunto = null;//Hacer_Pdf();
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error("No se pudo enviar el Correo.");
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
            String Costo_Clave_Ingreso = "";
            String Dependencia_Id = "";
            String Consulta = "SELECT " + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " LIKE '%INSCRIPCION O REFRENDOS%'";
            Clave_Ingreso_Id = Obtener_Dato_Consulta(Consulta);
            if (Clave_Ingreso_Id.Trim() != "")
            {
                Consulta = "SELECT " + Cat_Pre_Claves_Ing_Costos.Campo_Costo + " FROM " + Cat_Pre_Claves_Ing_Costos.Tabla_Cat_Pre_Claves_Ing_Costos + " WHERE " + Cat_Pre_Claves_Ing_Costos.Campo_Clave_Ingreso_ID + " = '" + Clave_Ingreso_Id + "' AND " + Cat_Pre_Claves_Ing_Costos.Campo_Anio + "=" + DateTime.Now.Year;
                Costo_Clave_Ingreso = Obtener_Dato_Consulta(Consulta);
                Consulta = "SELECT " + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " LIKE '%INSCRIPCION O REFRENDOS%'";
                Dependencia_Id = Obtener_Dato_Consulta(Consulta);
                if (Costo_Clave_Ingreso.Trim() != "")
                {
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "SOLICITUD DE REGISTRO DE PERITO EXTERNO";
                    Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Clave_Ingreso_Id;
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dependencia_Id;
                    Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = Convert.ToDouble(Costo_Clave_Ingreso).ToString("0.00");
                    Hdf_Importe_Total_Pago.Value = Calculo_Impuesto_Traslado.P_Monto_Total_Pagar;
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = "";
                    Calculo_Impuesto_Traslado.P_Contribuyente = Txt_Nombre.Text + " " + Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text;
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.AddDays(15).ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                }
                else
                {
                    Mostrar_Mensaje_Error("No se puede insertar el pasivo, falta el costo de la clave de ingreso del año " + DateTime.Now.Year + ".");
                }
            }
            else
            {
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error("No se puede insertar el pasivo.");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Autoriza la solicitud de registro y envia un correo
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 14/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Peritos_Externos.SelectedIndex > -1)
        {
            Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Recepcion = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
            Recepcion.P_Perito_Externo_Id = Hdf_Perito_Externo_Id.Value;
            Recepcion.P_Estatus = "POR PAGAR";
            if ((Recepcion.Modificar_Perito_Externo_Est()))
            {
                if (Hdf_Estatus_Perito_Externo.Value == "VALIDAR")
                {
                   // Enviar_Correo_Cuenta();
                    //Insertar Pasivo
                    Insertar_Pasivo("PE" + Convert.ToInt16(Hdf_Perito_Externo_Id.Value));
                    Imprimir_Reporte(Crear_Ds_Ope_Cat_Solicitud_Refrendo(), "Rpt_Ope_Cat_Solicitud_Registro.rpt", "Rpt_Folio_Solicitud_Registro", "Window_Frm", "Refrendo");
                }
                Configuracion_Formulario(true);
                Grid_Documentos.SelectedIndex = -1;
                Btn_Salir_Click(null, null);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud de Refrendo", "alert('Solicitud Autorizada.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud de Refrendo", "alert('Error al Intentar Autorizar.');", true);
            }
        }
        else
        {
            Mostrar_Mensaje_Error("Seleccione un Perito externo a Autorizar");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Cancelar_Click
    ///DESCRIPCIÓN          : Rechaza la solicitud de registro y envia un correo
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 14/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cancelar_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Peritos_Externos.SelectedIndex > -1)
        {
            Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Recepcion = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
            Recepcion.P_Temp_Perito_Externo_Id = Hdf_Perito_Externo_Id.Value;
            Recepcion.P_Estatus = "BAJA";
            if ((Recepcion.Modificar_Estatus_Perito_Externo_Temporal()))
            {
                if (Hdf_Estatus_Perito_Externo.Value == "VALIDAR")
                {
                    Enviar_Correo_Cuenta_Rechazado();
                    //Insertar Pasivo
                    //Insertar_Pasivo("PE" + Convert.ToInt16(Hdf_Perito_Externo_Id.Value));
                }
                Configuracion_Formulario(true);
                Grid_Documentos.SelectedIndex = -1;
                Btn_Salir_Click(null, null);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud de Registro", "alert('Solicitud Rechazada.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento de Solicitud de Registro", "alert('Error  al Intentar Rechazar.');", true);
            }
        }
        else
        {
            Mostrar_Mensaje_Error("Seleccione un perito externo a rechazar");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Ope_Cat_Solicitud_Refrendo
    ///DESCRIPCIÓN          : Crea un DataSet para el reporte de refrendo perito Externo
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Ope_Cat_Solicitud_Refrendo()
    {
        Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio Memoria = new Cls_Ope_Cat_Recepcion_Documentos_Perito_Externo_Negocio();
        Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Externo = new Cls_Cat_Cat_Peritos_Externos_Negocio();
        Ds_Ope_Cat_Solicitud_Registro Folio_Refrendo = new Ds_Ope_Cat_Solicitud_Registro();
        Memoria.P_Perito_Externo_Id = Hdf_Perito_Externo_Id.Value;
        DataTable Dt_Refrendos = Memoria.Consultar_Peritos_Externos();
//        DataTable Dt_Refrendos = Memoria.Consultar_Peritos_Externos();
        //Perito_Externo.P_Perito_Externo_Id = Hdf_Perito_Externo_Id.Value; //Dt_Memorias.Rows[0][Ope_Cat_Avaluo_Urbano.Campo_Perito_Externo_Id].ToString();
        Numalet Cantidad = new Numalet();
        Cantidad.MascaraSalidaDecimal = "00/100 M.N.";
        Cantidad.SeparadorDecimalSalida = "Pesos";
        Cantidad.ApocoparUnoParteEntera = true;
        Cantidad.LetraCapital = true;

        DataTable Dt_Folio_Refrendo = Folio_Refrendo.Tables["Dt_Solicitud_Registro"];
        DataRow Dr_Renglon_Nuevo = Dt_Folio_Refrendo.NewRow();
        Dr_Renglon_Nuevo["Solicitante"] = Txt_Nombre.Text + " " + Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text;
        Dr_Renglon_Nuevo["Importe"] = Convert.ToDouble(Hdf_Importe_Total_Pago.Value);
        Dr_Renglon_Nuevo["Fecha_Autorizacion"] = DateTime.Now.ToString("dd/MMM/yyyy");
        Dr_Renglon_Nuevo["Folio"] = "PE" + Convert.ToInt16(Hdf_Perito_Externo_Id.Value).ToString();
        Dr_Renglon_Nuevo["Observaciones"] = Txt_Observaciones.Text;
        Dr_Renglon_Nuevo["Tipo_Solicitud"] = "S     O     L     I     C     I     T     U     D        D E        R     E     F     R     E     N     D     O";
        
        //Dr_Renglon_Nuevo["IMPORTE_AVALUO_LETRAS"] = Cantidad.ToCustomCardinal(Txt_Precio_Avaluo.Text.Replace(",", "")); ;
        //Dr_Renglon_Nuevo["IMPORTE_AVALUO"] = Convert.ToDouble(Txt_Precio_Avaluo.Text);
        Session["E_Mail"] = Dt_Refrendos.Rows[0][Cat_Cat_Peritos_Externos.Campo_Usuario].ToString();
        Dt_Folio_Refrendo.Rows.Add(Dr_Renglon_Nuevo);
        return Folio_Refrendo;
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
            Lbl_Encabezado_Error.Text = "No se pudo cargar el reporte para su impresión";

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
            Enviar_Correo_Cuenta_Reporte((String)Session["E_Mail"], Server.MapPath("../../Reporte/" + Archivo_PDF));
            Session.Remove("E_Mail");
            //Mostrar_Reporte(Archivo_PDF, Tipo, Formato);
        }
        catch (Exception Ex)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = Ex.Message;
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
    private void Enviar_Correo_Cuenta_Reporte(String E_Mail, String Url_Adjunto)
    {
        String Contenido = "";
        Contenido = "Su solicitud de Refrendo ha sido autorizado. Favor de pasar a pagar en las cajas de Presidencia de Irapuato, su folio de pago se encuentra adjunto a este correo. Favor de imprimirlo dos veces";
        try
        {
            if (E_Mail.Trim().Length > 0)
            {
                Cls_Mail mail = new Cls_Mail();
                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = E_Mail.Trim();
                mail.P_Subject = "Solicitud de Refrendo autorizada";
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
}
