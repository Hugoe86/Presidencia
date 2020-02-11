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
using Presidencia.Catalogo_Cat_Reg_Condominio.Negocio;
using Presidencia.Operacion_Cat_Memorias_Descriptivas.Negocio;
using Presidencia.Catalogo_Cat_Factores_Cobro_Memorias_Descriptivas.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Web.UI.WebControls;
using System.IO;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;


public partial class paginas_Catastro_Frm_Ope_Cat_Memorias_Descriptivas_Fraccionador : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Tool_ScriptManager.RegisterPostBackControl(Btn_Agregar_Documento);
            Btn_Agregar_Documento.Attributes["onclick"] = "$get('" + Uprg_Reporte.ClientID + "').style.display = 'block'; return true;";
            if (!Page.IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                //Hdf_Perito_Externo.Value = Cls_Sessiones.Empleado_ID;
                Llenar_Tabla_Memorias(0);
                Llenar_Combo_Doc_Regimen_Condominio();
                Configuracion_Formulario(true);
                
                //Session.Remove("ESTATUS_CUENTAS");
                //Session.Remove("TIPO_CONTRIBUYENTE");
                //Session["ESTATUS_CUENTAS"] = "IN ('PENDIENTE','ACTIVA','VIGENTE','BLOQUEADA','SUSPENDIDA','CANCELADA')";
                //String Ventana_Modal1 = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:700px;dialogHeight:420px;dialogHide:true;help:no;scroll:no');";
                //Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal1);
            }
        }
        catch (Exception ex)
        {
            //Mostrar_Mensaje_Error(ex.Message.ToString());
        }
        Lbl_Encabezado_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipos_Construccion
    ///DESCRIPCIÓN: Llena el combo de Tipos de Construccion
    ///PROPIEDADES:         
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 24/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Doc_Regimen_Condominio()
    {
        try
        {
            Cls_Cat_Cat_Reg_Condominio_Negocio Tipos_Construccion = new Cls_Cat_Cat_Reg_Condominio_Negocio();
            Tipos_Construccion.P_Estatus = "VIGENTE";
            Tipos_Construccion.P_Tipo = Cmb_Tipo.SelectedValue;
            DataTable tabla = Tipos_Construccion.Consultar_Regimen_Condominio();
            DataRow fila = tabla.NewRow();
            fila[Cat_Cat_Reg_Condominio.Campo_Regimen_Condominio_ID] = "SELECCIONE";
            fila[Cat_Cat_Reg_Condominio.Campo_Nombre_Documento] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            tabla.Rows.InsertAt(fila, 0);
            Cmb_Documento.DataSource = tabla;
            Cmb_Documento.DataValueField = Cat_Cat_Reg_Condominio.Campo_Regimen_Condominio_ID;
            Cmb_Documento.DataTextField = Cat_Cat_Reg_Condominio.Campo_Nombre_Documento;
            Cmb_Documento.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = Ex.Message;
        }
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
    private void Llenar_Tabla_Memorias(int Pagina)
    {
        try
        {
            Cls_Ope_Cat_Memorias_Descriptivas_Negocio Memorias = new Cls_Ope_Cat_Memorias_Descriptivas_Negocio();
            DataTable Dt_Documentos;
            Memorias.P_Perito_Externo_Id = Cls_Sessiones.Empleado_ID;
            Dt_Documentos = Memorias.Consultar_Memorias_Descriptivas();
            Grid_Memorias_Descriptivas.Columns[1].Visible = true;
            Grid_Memorias_Descriptivas.Columns[2].Visible = true;
            Grid_Memorias_Descriptivas.Columns[6].Visible = true;
            Grid_Memorias_Descriptivas.Columns[7].Visible = true;
            Grid_Memorias_Descriptivas.Columns[8].Visible = true;
            Grid_Memorias_Descriptivas.Columns[9].Visible = true;
            Grid_Memorias_Descriptivas.Columns[10].Visible = true;
            Grid_Memorias_Descriptivas.Columns[11].Visible = true;
            Grid_Memorias_Descriptivas.DataSource = Dt_Documentos;
            Grid_Memorias_Descriptivas.PageIndex = Pagina;
            Grid_Memorias_Descriptivas.DataBind();
            Grid_Memorias_Descriptivas.Columns[1].Visible = false;
            Grid_Memorias_Descriptivas.Columns[2].Visible = false;
            Grid_Memorias_Descriptivas.Columns[6].Visible = false;
            Grid_Memorias_Descriptivas.Columns[7].Visible = false;
            Grid_Memorias_Descriptivas.Columns[8].Visible = false;
            Grid_Memorias_Descriptivas.Columns[9].Visible = false;
            Grid_Memorias_Descriptivas.Columns[10].Visible = false;
            Grid_Memorias_Descriptivas.Columns[11].Visible = false;
        }
        catch (Exception E)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = E.Message;
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
        Txt_No_Memorias_Descriptivas.Enabled = !Enabled;
        Cmb_Documento.Enabled = !Enabled;
        //Cmb_Estatus.Enabled = !Enabled;
        Cmb_Tipo.Enabled = !Enabled;
        Fup_Documento.Enabled = !Enabled;
        Grid_Memorias_Descriptivas.Enabled = Enabled;
        Grid_Documentos.Enabled = !Enabled;
        Btn_Agregar_Documento.Enabled = !Enabled;
        Txt_Fraccionamiento.Enabled = !Enabled;
        Txt_Solicitante.Enabled = !Enabled;
        //Txt_Calculo_Valores_Memorias.Enabled = !Enabled;
        Txt_Calculo_Valores_Memorias.Style["text-align"] = "Right";
        Txt_Ubicacion.Enabled = !Enabled;
        Txt_Cuent_Predial.Enabled = !Enabled;
        Cmb_Horientacion.Enabled = !Enabled;

    }

    private void Limpiar_Componentes()
    {
        //Txt_Cuenta_Predial.Text = "";
        Txt_No_Memorias_Descriptivas.Text = "";
        Cmb_Tipo.SelectedIndex = 0;
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Documento.SelectedValue = "SELECCIONE";
        Txt_Solicitante.Text = "";
        Txt_Fraccionamiento.Text = "";
        Txt_Observacion.Text = "";
        Txt_Calculo_Valores_Memorias.Text = "";
        Txt_Ubicacion.Text = "";
        Txt_Cuent_Predial.Text = "";
        Cmb_Horientacion.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Evento del botón nuevo
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
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
                if (Grid_Documentos.Rows.Count == 0)
                {
                    Configuracion_Formulario(false);
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    DataTable Dt_Documentos = new DataTable();
                    Dt_Documentos.Columns.Add("NO_DOCUMENTO", typeof(String));
                    Dt_Documentos.Columns.Add("ANIO_DOCUMENTO", typeof(int));
                    Dt_Documentos.Columns.Add("REGIMEN_CONDOMINIO_ID", typeof(String));
                    Dt_Documentos.Columns.Add("DOCUMENTO", typeof(String));
                    Dt_Documentos.Columns.Add("RUTA_DOCUMENTO", typeof(String));
                    Dt_Documentos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
                    Dt_Documentos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
                    Dt_Documentos.Columns.Add("ACCION", typeof(String));
                    Session["Dt_Documentos"] = Dt_Documentos;
                    Cmb_Estatus.Enabled = false;
                    Cmb_Estatus.SelectedValue = "VIGENTE";
                    Hdf_Anio.Value = DateTime.Now.Year.ToString();
                }
                else
                {
                    Lbl_Encabezado_Error.Visible = true;
                    Lbl_Encabezado_Error.Text = "Imposible dar de alta";
                }
            }
            else if (Validar_Componentes())
            {
                Cls_Ope_Cat_Memorias_Descriptivas_Negocio Recepcion = new Cls_Ope_Cat_Memorias_Descriptivas_Negocio();
                Recepcion.P_Cantidad_Mem_Descript = Txt_No_Memorias_Descriptivas.Text.Trim();
                Recepcion.P_Anio = Hdf_Anio.Value;
                Recepcion.P_Anio_Documento = Hdf_Anio.Value;
                Recepcion.P_Dt_Archivos = (DataTable)Session["Dt_Documentos"];
                Recepcion.P_Observaciones = Txt_Observacion.Text.ToUpper();
                Recepcion.P_Fraccionamiento = Txt_Fraccionamiento.Text.ToUpper();
                Recepcion.P_Solicitante = Txt_Solicitante.Text.ToUpper();
                Recepcion.P_Ubicacion = Txt_Ubicacion.Text.ToUpper();
                Recepcion.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                Recepcion.P_Estatus = Cmb_Estatus.SelectedValue;
                Recepcion.P_Tipo = Cmb_Tipo.SelectedValue;
                Recepcion.P_Horientacion = Cmb_Horientacion.SelectedValue;
                Recepcion.P_Perito_Externo_Id = Cls_Sessiones.Empleado_ID;
                //Recepcion.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                if ((Recepcion.Alta_Memoria_Descriptiva()))
                {
                    //String Url = Server.MapPath("../Catastro/Archivos_Memorias");
                    //System.IO.Directory.CreateDirectory(Url + "/" + Recepcion.P_No_Mem_Descript + "/");
                    Hdf_No_Mem_Descript.Value = Recepcion.P_No_Mem_Descript;
                    Guardar_Imagenes(Recepcion.P_Dt_Archivos);
                    //Eliminar_Imagenes(Recepcion.P_Dt_Archivos);
                    Configuracion_Formulario(true);
                    Btn_Salir_Click(null, null);
                    //Llenar_Tabla_Documentos(0);
                    Grid_Documentos.SelectedIndex = -1;
                    Limpiar_Componentes();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Memorias Descriptivas", "alert('Alta exitosa.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Registro", "alert('Error, vuelva a intentar.');", true);
                }
            }
        }
        catch (Exception E)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = E.Message;
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
            if (Cmb_Estatus.Text == "VIGENTE" || Cmb_Estatus.Text == "RECHAZADA")
            {
                if (Grid_Memorias_Descriptivas.SelectedIndex > -1)
                {

                    if (Btn_Modificar.AlternateText.Equals("Modificar"))
                    {

                        if (Grid_Documentos.Rows.Count > 0)
                        {
                            Configuracion_Formulario(false);
                            Btn_Modificar.AlternateText = "Actualizar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                            Btn_Salir.AlternateText = "Cancelar";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Nuevo.Visible = false;

                            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
                            DataTable Dt_Cuenta;
                            Cuenta_Predial.P_Cuenta_Predial = Txt_Cuent_Predial.Text.ToUpper();
                            Dt_Cuenta = Cuenta_Predial.Consultar_Cuenta();

                            Hdf_Cuenta_Predial_Id.Value = Dt_Cuenta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();

                            //Txt_Fraccionamiento.Text = "";
                            //Txt_No_Memorias_Descriptivas.Text = "";
                            //Txt_Observacion.Text = "";
                            //Txt_Solicitante.Text = "";

                            //Grid_Memorias_Descriptivas.SelectedIndex = -1;

                            //Cmb_Tipo.SelectedIndex = 0;
                            //Cmb_Documento.SelectedIndex = 0;


                        }
                        else
                        {
                            Lbl_Encabezado_Error.Visible = true;
                            Lbl_Encabezado_Error.Text = "Imposible modificar";
                        }
                    }

                    else if (Validar_Componentes())
                    {
                        Cls_Ope_Cat_Memorias_Descriptivas_Negocio Recepcion = new Cls_Ope_Cat_Memorias_Descriptivas_Negocio();
                        Recepcion.P_Cantidad_Mem_Descript = Txt_No_Memorias_Descriptivas.Text.Trim();
                        Recepcion.P_Perito_Externo_Id = Cls_Sessiones.Empleado_ID;
                        Recepcion.P_Anio = Hdf_Anio.Value;
                        Recepcion.P_Anio_Documento = Hdf_Anio.Value;
                        Recepcion.P_Dt_Archivos = (DataTable)Session["Dt_Documentos"];
                        Recepcion.P_Observaciones = Txt_Observacion.Text.ToUpper().Trim();
                        Recepcion.P_Estatus = "VIGENTE";
                        Recepcion.P_Tipo = Cmb_Tipo.SelectedValue;
                        Recepcion.P_Fraccionamiento = Txt_Fraccionamiento.Text.ToUpper().Trim();
                        Recepcion.P_Solicitante = Txt_Solicitante.Text.ToUpper().Trim();
                        Recepcion.P_Ubicacion = Txt_Ubicacion.Text.ToUpper();
                        Recepcion.P_No_Mem_Descript = Hdf_No_Mem_Descript.Value;
                        Recepcion.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                        Recepcion.P_Horientacion = Cmb_Horientacion.SelectedValue;
                        if ((Recepcion.Modificar_Memoria_Descriptiva()))
                        {
                            String Url = Server.MapPath("../Catastro/Archivos_Memorias");
                            System.IO.Directory.CreateDirectory(Url + "/" + Recepcion.P_No_Mem_Descript + "/");
                            Hdf_No_Mem_Descript.Value = Recepcion.P_No_Mem_Descript;
                            Guardar_Imagenes(Recepcion.P_Dt_Archivos);
                            Eliminar_Imagenes(Recepcion.P_Dt_Archivos);
                            Configuracion_Formulario(true);
                            Btn_Salir_Click(null, null);
                            //Llenar_Tabla_Documentos(0);
                            Grid_Memorias_Descriptivas.SelectedIndex = -1;
                            Limpiar_Componentes();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Memorias Descriptivas", "alert('Modificacion exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Registro", "alert('Error, vuelva a intentar.');", true);
                        }
                    }
                }
                else
                {

                    Lbl_Encabezado_Error.Visible = true;
                    Lbl_Encabezado_Error.Text = "Seleccione un registro de la tabla de Memorias Descriptivas";

                }
            }
            else
            {

                Lbl_Encabezado_Error.Visible = true;
                Lbl_Encabezado_Error.Text = "No se puede modificar los registro autorizados";
            }
        }
        catch (Exception E)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = E.Message;
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
            Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        }
        else
        {

            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(true);
            Limpiar_Componentes();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Llenar_Tabla_Memorias(0);
            Session["Dt_Documentos"] = null;
            Grid_Documentos.DataSource = null;
            Grid_Documentos.DataBind();
            Llenar_Combo_Doc_Regimen_Condominio();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Evento del botón Agregar Documento
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Documento_Click(object sender, ImageClickEventArgs e)
    {
        if (Cmb_Documento.SelectedValue != "SELECCIONE" && Fup_Documento.FileName.Trim() != "")
        {
            DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
            Boolean Entro = false;
            foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
            {
                if (Dr_Renglon[Ope_Cat_Doc_Mem_Descript.Campo_Regimen_Condominio_Id].ToString() == Cmb_Documento.SelectedValue && Dr_Renglon["ACCION"].ToString() != "BAJA")
                {
                    Entro = true;
                    break;
                }
            }
            if (!Entro)
            {
                DataRow Dr_Nuevo = Dt_Documentos.NewRow();
                Dr_Nuevo["NO_DOCUMENTO"] = " ";
                Dr_Nuevo["ANIO_DOCUMENTO"] = Convert.ToInt16(Hdf_Anio.Value);
                Dr_Nuevo["ACCION"] = "ALTA";
                Dr_Nuevo["EXTENSION_ARCHIVO"] = Path.GetExtension(Fup_Documento.FileName).ToLower();
                Dr_Nuevo["DOCUMENTO"] = Cmb_Documento.SelectedItem.Text;
                Dr_Nuevo["BITS_ARCHIVO"] = Fup_Documento.FileBytes;
                Dr_Nuevo["REGIMEN_CONDOMINIO_ID"] = Cmb_Documento.SelectedValue;
                Dr_Nuevo["RUTA_DOCUMENTO"] = Cmb_Documento.SelectedItem.Text.Replace(' ', '_') + Path.GetExtension(Fup_Documento.FileName).ToLower();
                Dt_Documentos.Rows.Add(Dr_Nuevo);
                Dt_Documentos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
                Grid_Documentos.Columns[0].Visible = true;
                Grid_Documentos.Columns[1].Visible = true;
                Grid_Documentos.DataSource = Dt_Documentos;
                Grid_Documentos.DataBind();
                Grid_Documentos.Columns[0].Visible = false;
                Grid_Documentos.Columns[1].Visible = false;
            }
            Cmb_Documento.SelectedValue = "SELECCIONE";
        }
    }






    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Evento del botón Agregar Documento
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
                    //Label Lbl_Url_Temporal = (Label)Grid_Documentos.Rows[i].Cells[3].FindControl("Lbl_Url");
                    if (File.Exists(Server.MapPath(Dr_Renglon["RUTA_DOCUMENTO"].ToString())))
                    {
                        HyperLink Hlk_Enlace = new HyperLink();
                        Hlk_Enlace.Text = Path.GetFileName(Dr_Renglon["RUTA_DOCUMENTO"].ToString());
                        Hlk_Enlace.NavigateUrl = Dr_Renglon["RUTA_DOCUMENTO"].ToString();
                        Hlk_Enlace.CssClass = "enlace_fotografia";
                        Hlk_Enlace.Target = "blank";
                        //e.Row.Cells[3].Controls.Add(Hlk_Enlace);
                        Grid_Documentos.Rows[i].Cells[4].Controls.Add(Hlk_Enlace);
                        i++;
                    }
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Evento del botón Agregar Documento
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
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
            if (Dr_Documento["DOCUMENTO"].ToString() == Grid_Documentos.SelectedRow.Cells[3].Text && Dr_Documento["ACCION"].ToString() != "BAJA")
            {
                Dr_Documento["ACCION"] = "BAJA";
                break;
            }
        }
        //Session["Dt_Documentos"] = Dt_Documentos.Copy();
        Dt_Documentos.DefaultView.RowFilter = "ACCION <> 'BAJA'";
        Grid_Documentos.Columns[0].Visible = true;
        Grid_Documentos.Columns[1].Visible = true;
        Grid_Documentos.DataSource = Dt_Documentos;
        Grid_Documentos.PageIndex = 0;
        Grid_Documentos.DataBind();
        Grid_Documentos.Columns[0].Visible = false;
        Grid_Documentos.Columns[1].Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Evento del botón Agregar Documento
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Tipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Combo_Doc_Regimen_Condominio();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Evento del botón Agregar Documento
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Memorias_Descriptivas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Memorias(e.NewPageIndex);
        Grid_Memorias_Descriptivas.SelectedIndex = -1;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Evento del botón Agregar Documento
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Memorias_Descriptivas_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Memorias_Descriptivas.SelectedIndex > -1)
        {

            Limpiar_Componentes();
            Hdf_No_Mem_Descript.Value = Grid_Memorias_Descriptivas.SelectedRow.Cells[1].Text;
            Hdf_Anio.Value = Grid_Memorias_Descriptivas.SelectedRow.Cells[2].Text;
            Txt_No_Memorias_Descriptivas.Text = Grid_Memorias_Descriptivas.SelectedRow.Cells[3].Text;
            Cmb_Tipo.SelectedValue = Grid_Memorias_Descriptivas.SelectedRow.Cells[4].Text;
            Cmb_Estatus.SelectedValue = Grid_Memorias_Descriptivas.SelectedRow.Cells[5].Text;
          
             Txt_Ubicacion.Text = Grid_Memorias_Descriptivas.SelectedRow.Cells[9].Text;

             Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
             DataTable Dt_Cuenta;
             Cuenta_Predial.P_Cuenta_Predial_ID = Grid_Memorias_Descriptivas.SelectedRow.Cells[10].Text;
             Dt_Cuenta = Cuenta_Predial.Consultar_Cuenta();
             Txt_Cuent_Predial.Text = Dt_Cuenta.Rows[0]["CUENTA_PREDIAL"].ToString();
             Cmb_Horientacion.SelectedValue = Grid_Memorias_Descriptivas.SelectedRow.Cells[11].Text;

            
            if (Grid_Memorias_Descriptivas.SelectedRow.Cells[6].Text != "&nbsp;")
            {
                Txt_Observacion.Text = Grid_Memorias_Descriptivas.SelectedRow.Cells[6].Text;
            }
            Txt_Fraccionamiento.Text = Grid_Memorias_Descriptivas.SelectedRow.Cells[7].Text;
            Txt_Solicitante.Text = Grid_Memorias_Descriptivas.SelectedRow.Cells[8].Text;
            Llenar_Combo_Doc_Regimen_Condominio();
            Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio Factores = new Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio();
            Factores.P_Anio = Hdf_Anio.Value;
            DataTable Dt_Factores = Factores.Consulta_Factores_Cobro_Memorias_Descriptivas();
            Hdf_Cantidad_Cobro1.Value = Dt_Factores.Rows[0]["CANTIDAD_COBRO_1"].ToString();
            Hdf_Cantidad_Cobro2.Value = Dt_Factores.Rows[0]["CANTIDAD_COBRO_2"].ToString();
            Txt_Calculo_Valores_Memorias.Text = ((Convert.ToDouble(Hdf_Cantidad_Cobro1.Value) * Convert.ToDouble(Txt_No_Memorias_Descriptivas.Text.Trim())) + Convert.ToDouble(Hdf_Cantidad_Cobro2.Value)).ToString("###,###,###,##0.00");
            Cls_Ope_Cat_Memorias_Descriptivas_Negocio Memorias = new Cls_Ope_Cat_Memorias_Descriptivas_Negocio();
            Memorias.P_No_Mem_Descript = Hdf_No_Mem_Descript.Value;
            DataTable Dt_Documentos = Memorias.Consultar_Documentos_Memorias_Descriptivas();
            Dt_Documentos.Columns.Add("BITS_ARCHIVO", Type.GetType("System.Byte[]"));
            Dt_Documentos.Columns.Add("EXTENSION_ARCHIVO", typeof(String));
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
        if (!Directory.Exists(Server.MapPath("../Catastro/Archivos_Memorias/") + Hdf_Anio.Value + "_" + Hdf_No_Mem_Descript.Value))
        {
            Directory.CreateDirectory(Server.MapPath("../Catastro/Archivos_Memorias/") + Hdf_Anio.Value + "_" + Hdf_No_Mem_Descript.Value);
        }
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "ALTA")
            {
                //crear filestream y binarywriter para guardar archivo
                FileStream Escribir_Archivo = new FileStream(Server.MapPath("../Catastro/Archivos_Memorias/") + Hdf_Anio.Value + "_" + Hdf_No_Mem_Descript.Value + "/" + Dr_Renglon["RUTA_DOCUMENTO"].ToString(), FileMode.Create, FileAccess.Write);
                BinaryWriter Datos_Archivo = new BinaryWriter(Escribir_Archivo);
                Datos_Archivo.Write((Byte[])Dr_Renglon["BITS_ARCHIVO"]);
            }
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
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        if (Txt_Fraccionamiento.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingresa  el Fraccionamiento.";
            Valido = false;
        }

        if (Txt_Solicitante.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingresa  el nombre del solicitante.";
            Valido = false;
        }
        if (Txt_Ubicacion.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingresa  la Ubicacion.";
            Valido = false;
        }
        if (Txt_No_Memorias_Descriptivas.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingresa las Memorias Descriptivas solicitadas.";
            Valido = false;
        }
        if (Txt_Observacion.Text.Trim() == "&nbsp;")
        {
            Txt_Observacion.Text = "";
        }
        if (Cmb_Tipo.SelectedValue == "SELECCIONE")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Seleccione el tipo de Trámite solicitado.";
            Valido = false;
        }
        if (Dt_Documentos.Rows.Count == 0)
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Favor de ingresar Documentos.";
            Valido = false;
        }
        if (Txt_Cuent_Predial.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingrese la Cuenta predial.";
            Valido = false;
        }
        if (Dt_Documentos.Rows.Count > 0 && ((Cmb_Documento.Items.Count - 1) > Grid_Documentos.Rows.Count))
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Debe de Ingresar todos los Documentos Requeridos.";
            Valido = false;
        }

        if (!Valido)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = Msj_Error;
        }
        return Valido;
    }

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

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    /////DESCRIPCIÓN          : Muestra los datos de la consulta
    /////PARAMETROS:     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 29/Julio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
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
    //            Hdf_Cuenta_Predial_Id.Value = Cuenta_Predial_ID;
    //            Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
    //            Txt_Cuenta_Predial.Text = Cuenta_Predial;
    //        }
    //    }
    //    Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
    //    Session.Remove("CUENTA_PREDIAL_ID");
    //    //Session.Remove("CUENTA_PREDIAL");
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_No_Memorias_Descriptivas_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_No_Memorias_Descriptivas
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 09/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_No_Memorias_Descriptivas_TextChanged(object sender, EventArgs e)
    {
        Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio Factores = new Cls_Cat_Cat_Factores_Cobro_Memorias_Descriptivas_Negocio();
        Factores.P_Anio = Hdf_Anio.Value;
        DataTable Dt_Factores = Factores.Consulta_Factores_Cobro_Memorias_Descriptivas();
        Hdf_Cantidad_Cobro1.Value = Dt_Factores.Rows[0]["CANTIDAD_COBRO_1"].ToString();
        Hdf_Cantidad_Cobro2.Value = Dt_Factores.Rows[0]["CANTIDAD_COBRO_2"].ToString();
        Txt_Calculo_Valores_Memorias.Text = ((Convert.ToDouble(Hdf_Cantidad_Cobro1.Value) * Convert.ToDouble(Txt_No_Memorias_Descriptivas.Text.Trim())) + Convert.ToDouble(Hdf_Cantidad_Cobro2.Value)).ToString("###,###,###,##0.00");
        

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Calculo_Valores_Memorias_TextChanged
    ///DESCRIPCIÓN: Evento del componente Txt_Cantidad_1
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 09/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Cuenta_Predial_TextChanged(object sender, EventArgs e)
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        DataTable Dt_Cuenta;
        Cuenta_Predial.P_Cuenta_Predial = Txt_Cuent_Predial.Text.ToUpper();
        Dt_Cuenta = Cuenta_Predial.Consultar_Cuenta();
        if (Dt_Cuenta.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_Id.Value = Dt_Cuenta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();

            Btn_Modificar.Enabled = true;
            Btn_Nuevo.Enabled = true;
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Memorias Descriptivas", "alert('La Cuenta Predial ingresada es correcta');", true);
        }
        else
        {
            Hdf_Cuenta_Predial_Id.Value = "";
            Btn_Modificar.Enabled = false;
            Btn_Nuevo.Enabled = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Memorias Descriptivas", "alert('La Cuenta Predial ingresada no existe actualmente');", true);
        }

    }
}

