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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.IO;
using Presidencia.Catalogo_Cat_Claves_Catastrales.Negocio;
using Presidencia.Operacion_Cat_Solicitud_Claves_Catastrales.Negocio;

public partial class paginas_Catastro_Frm_Ope_Cat_Solicitud_Claves_Catastrales : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) 
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Tool_ScriptManager.RegisterPostBackControl(Btn_Agregar_Documento);
            Btn_Agregar_Documento.Attributes["onclick"] = "$get('" + Uprg_Reporte.ClientID + "').style.display = 'block'; return true;";
            if (!Page.IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Llenar_Tabla_Memorias(0);
                Llenar_Combo_Doc_Regimen_Condominio();
                Configuracion_Formulario(true);
                Session.Remove("ESTATUS_CUENTA");
                Session.Remove("TIPO_CONTRIBUYENTE");
                Session["ESTATUS_CUENTAS"] = "IN ('PENDIENTE','ACTIVA','VIGENTE','BLOQUEADA','SUSPENDIDA','CANCELADA')";
                String Ventana_Modal1 = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:700px;dialogHeight:420px;dialogHide:true;help:no;scroll:no');";
                Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal1);
            }
        }
        catch (Exception Ex)
        {

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
    ///
    private void Llenar_Combo_Doc_Regimen_Condominio()
    {
        try
        {
            Cls_Cat_Cat_Claves_Catastrales_Negocio Clave = new Cls_Cat_Cat_Claves_Catastrales_Negocio();
            Clave.P_Estatus = "VIGENTE";
            Clave.P_Tipo = Cmb_Tipo.SelectedValue;            
            DataTable tabla = Clave.Consultar_Claves_Catastrales();
            DataRow fila = tabla.NewRow();
            fila[Cat_Cat_Claves_Catastrales.Campo_Claves_Catastrales_ID] = "SELECCIONE";
            fila[Cat_Cat_Claves_Catastrales.Campo_Identificador] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            tabla.Rows.InsertAt(fila, 0);
            Cmb_Documento.DataSource = tabla;
            Cmb_Documento.DataValueField = Cat_Cat_Claves_Catastrales.Campo_Claves_Catastrales_ID;
            Cmb_Documento.DataTextField = Cat_Cat_Claves_Catastrales.Campo_Identificador;
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
            Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio Solicitud = new Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio();
            DataTable Dt_Documentos;
            Solicitud.P_No_Claves_Catastrales = Hdf_Clave_Externo_Id.Value;
            Dt_Documentos = Solicitud.Consultar_Solicitud_Claves_Catastrales();
            Session["Dt_Documentos"] = Dt_Documentos.Copy();            
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
        Txt_Cuenta_Predial.Enabled = false;
        Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = !Enabled;
        Txt_Solicitante.Enabled = !Enabled;
        Txt_Correo.Enabled = !Enabled;
        Cmb_Documento.Enabled = !Enabled;
        Cmb_Estatus.Enabled = !Enabled;
        Cmb_Tipo.Enabled = !Enabled;
        Fup_Documento.Enabled = !Enabled;       
        Grid_Documentos.Enabled = !Enabled;
        Btn_Agregar_Documento.Enabled = !Enabled;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Componentes
    ///DESCRIPCIÓN: Establece la limpieza de la informacion del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Limpiar_Componentes()
    {        
        Txt_Solicitante.Text = "";
        Txt_Correo.Text = "";
        Cmb_Tipo.SelectedIndex = 0;
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Documento.SelectedValue = "SELECCIONE";
        Txt_Cuenta_Predial.Text = "";
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
                    DataTable Dt_Documentos = new DataTable();
                    Dt_Documentos.Columns.Add("NO_DOCUMENTO", typeof(String));
                    Dt_Documentos.Columns.Add("ANIO_DOCUMENTO", typeof(int));
                    Dt_Documentos.Columns.Add("CLAVES_CATASTRALES_ID", typeof(String));
                    Dt_Documentos.Columns.Add("CANTIDAD_CLAVES_CATASTRALES", typeof(String));
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
                Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio Recepcion = new Cls_Ope_Cat_Solicitud_Claves_Catastrales_Negocio();
                Recepcion.P_Solicitante = Txt_Solicitante.Text.Trim().ToUpper();
                Recepcion.P_Correo = Txt_Correo.Text.Trim().ToLower();
                Recepcion.P_Anio = Hdf_Anio.Value;
                Recepcion.P_Anio_Documento = Hdf_Anio.Value;
                Recepcion.P_Dt_Archivos = (DataTable)Session["Dt_Documentos"];
                Recepcion.P_Observaciones = "";
                Recepcion.P_Estatus = Cmb_Estatus.SelectedValue;
                Recepcion.P_Tipo = Cmb_Tipo.SelectedValue;
                Recepcion.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                if ((Recepcion.Alta_Solicitud_Claves_Catastrales()))
                {
                    String Url = Server.MapPath("../Catastro/Archivos_CC");
                    System.IO.Directory.CreateDirectory(Url + "/" + Recepcion.P_No_Claves_Catastrales + "/");
                    Hdf_No_Clav_Catastral.Value = Recepcion.P_No_Claves_Catastrales;
                    Guardar_Imagenes(Recepcion.P_Dt_Archivos);                    
                    Configuracion_Formulario(true);
                    Btn_Salir_Click(null, null);                    
                    Grid_Documentos.SelectedIndex = -1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Claves Catastrales", "alert('Alta exitosa.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Claves Catastrales", "alert('Error, vuelva a intentar.');", true);
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
        if (Cmb_Documento.SelectedValue!= "SELECCIONE" && Fup_Documento.FileName.Trim() != "")
        {
            DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
            Boolean Entro = false;
            foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
            {
                if (Dr_Renglon[Ope_Cat_Doc_Clave_Catastral.Campo_Claves_Catastrales_id].ToString() == Cmb_Documento.SelectedValue && Dr_Renglon["ACCION"].ToString() != "BAJA")
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
                Dr_Nuevo["CLAVES_CATASTRALES_ID"] = Cmb_Documento.SelectedValue;
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_DataBound
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
    ///
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
    ///
    protected void Grid_Clave_Catastral_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Llenar_Tabla_Memorias(e.NewPageIndex);
        
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
    ///
    private void Guardar_Imagenes(DataTable Dt_Documentos)
    {
        if (!Directory.Exists(Server.MapPath("../Catastro/Archivos_CC/") + Hdf_Anio.Value + "_" + Hdf_No_Clav_Catastral.Value))
        {
            Directory.CreateDirectory(Server.MapPath("../Catastro/Archivos_CC/") + Hdf_Anio.Value + "_" + Hdf_No_Clav_Catastral.Value);
        }
        foreach (DataRow Dr_Renglon in Dt_Documentos.Rows)
        {
            if (Dr_Renglon["ACCION"].ToString() == "ALTA")
            {
                //crear filestream y binarywriter para guardar archivo
                FileStream Escribir_Archivo = new FileStream(Server.MapPath("../Catastro/Archivos_CC/" 
                    + Hdf_Anio.Value + "_" + Hdf_No_Clav_Catastral.Value + "/" + Dr_Renglon["RUTA_DOCUMENTO"].ToString()), FileMode.Create, FileAccess.Write);
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
    ///
    private Boolean Validar_Componentes()
    {
        Boolean Valido = true;
        String Msj_Error = "Error: ";
        DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
        if (Txt_Cuenta_Predial.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingresa la Cuenta Predial.";
            Valido = false;
        }
        if (Txt_Correo.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingresa las Correo.";
            Valido = false;
        }
        if (Txt_Solicitante.Text.Trim() == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Ingresa Solicitante.";
            Valido = false;
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
        if (Dt_Documentos.Rows.Count > 0 && ((Cmb_Documento.Items.Count -1)>Grid_Documentos.Rows.Count))
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Debe de Ingresar la Cantidad de Documentos Requeridos.";
            Valido = false;
        }
        if (!Valido)
        {
            Lbl_Encabezado_Error.Visible = true;
            Lbl_Encabezado_Error.Text = Msj_Error;
        }
        return Valido;
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
    ///
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
            }
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");        
    }
}
