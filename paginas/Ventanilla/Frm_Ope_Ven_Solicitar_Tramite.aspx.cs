using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.IO;
using Presidencia.Solicitud_Tramites.Negocios;
using Presidencia.Ventanilla_Usarios.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Ordenamiento_Territorial_Inspectores.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Ventanilla_Consultar_Tramites.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Operacion_Predial_Pagos_Instit_Externas.Negocio;
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;
using System.Web.Security;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using System.Drawing;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;

public partial class paginas_Ventanilla_Frm_Ope_Ven_Solicitar_Tramite : System.Web.UI.Page
{

    #region (Page Load)

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION :
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 24/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {

        //Refresca la session del usuario lagueado al sistema.
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        //Valida que exista algun usuario logueado al sistema.
        //if (Cls_Sessiones.Ciudadano_ID == null || Cls_Sessiones.Nombre_Ciudadano.Equals(String.Empty)) 
        //    Response.Redirect("../Ventanilla/Frm_Apl_Login_Ventanilla.aspx");


        if (!IsPostBack)
        {
            Inicializa_Controles();

            this.Form.Enctype = "multipart/form-data";

            if (Cmb_Tramite.SelectedItem.Text == "Avaluo" || Cmb_Tramite.SelectedItem.Text == "Solicitud de registro" || Cmb_Tramite.SelectedItem.Text == "Solicitud de refrendo")
            {
                LLenar_Combos_Catastro();
            }
            else
            {
                LLenar_Combos();
            }

            string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Ven_Busqueda_Avanzada_Peritos.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Perito.Attributes.Add("onclick", Ventana_Modal);
        }
    }

    #endregion (Page Load)

    #region Metodos Generales

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
    ///               realizar diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 02/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpia_Controles();
            Habilitar_Controles("Nuevo");
            Cargar_Combo_Tramites();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Resumen_Predio
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 09/Julio/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Resumen_Predio()
    {
        String Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Frm_Resumen_Predio_Ventanilla.aspx";
        String Propiedades = ", 'center:yes,resizable=no,status=no,width=750,scrollbars=yes,');";
        //String Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:800px;dialogHeight:600px;dialogHide:true;help:no;scroll:no');";
        Btn_Buscar_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim().ToUpper() + "'" + Propiedades);
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita los controles de la forma realizar diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        try
        {
            switch (Operacion)
            {
                case "Inicial":
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Lbl_Datos_Requeridos.Visible = false;
                    Lbl_Documentos_Requeridos.Visible = false;
                    Lbl_Documento_Opcional.Visible = false;
                    Pnl_Cuenta_Predial.Style.Value = "display: none";
                    Div_Grid_Datos_Tramite.Style.Value = "display: none";
                    Div_Grid_Documentos.Style.Value = "display: none";
                    break;

                case "Nuevo":
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                    break;

                case "Modificar":
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    break;

                default:
                    return;
            }

            Txt_Email.Enabled = false;
            Txt_Nombre.Enabled = false;
            Txt_Apellido_Paterno.Enabled = false;
            Txt_Apellido_Materno.Enabled = false;
            Cmb_Tramite.Enabled = false;
            Btn_Generar_Reporte.Visible = false;

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Folio.Text = "";
            Txt_Apellido_Materno.Text = "";
            Txt_Apellido_Paterno.Text = "";
            Txt_Nombre_Completo.Text = "";
            Txt_Nombre.Text = "";
            //Txt_Costo.Text = "";
            Hdf_Costo.Value = "";
            Txt_Tiempo_Estimado.Text = "";
            Txt_Avance.Text = "";
            Txt_Email.Text = "";
            Txt_Cuenta_Predial.Text = "";
            Txt_Direccion_Predio.Text = "";
            Txt_Propietario_Cuenta_Predial.Text = "";
            Txt_Calle_Predio.Text = "";
            Txt_Numero_Predio.Text = "";
            Txt_Lote_Predio.Text = "";
            Txt_Manzana_Predio.Text = "";
            Txt_Otros_Predio.Text = "";
            Hdf_Cuenta_Predial.Value = "";
            Hdf_Dependencia_ID.Value = "";
            Cmb_Perito.SelectedIndex = -1;
            Btn_Buscar_Cuenta_Predial.Visible = false;
            Txt_Cantidad_Solicitud.Text = "1";


            if (Cmb_Tramite.Items.Count > 0)
                Cmb_Tramite.Items.Clear();

            Grid_Datos.DataSource = new DataTable();
            Grid_Datos.DataBind();

            Grid_Documentos.DataSource = new DataTable();
            Grid_Documentos.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Tramites()
    {
        Cls_Ope_Solicitud_Tramites_Negocio Solicitud_Negocio = new Cls_Ope_Solicitud_Tramites_Negocio();
        Cls_Ven_Usuarios_Negocio Rs_Datos_Usuario = new Cls_Ven_Usuarios_Negocio();
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        DataSet Ds_Tramites = new DataSet();
        DataTable Dt_Datos_Usuario = new DataTable();
        string Dependencia_ID_Ordenamiento = "";
        string Dependencia_ID_Ambiental = "";
        string Dependencia_ID_Urbanistico = "";
        string Dependencia_ID_Inmobiliario = "";
        string Dependencia_ID_Catastro = "";
        string Rol_Director_Ordenamiento = "";
        try
        {
            //  para los parametros de ordenamiento
            Obj_Parametros.Consultar_Parametros();

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
            {
                Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
            {
                Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
            {
                Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
            {
                Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Catastro))
            {
                Dependencia_ID_Catastro = Obj_Parametros.P_Dependencia_ID_Catastro;
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
            {
                Rol_Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;
            }

            //  para el tramite
            Solicitud_Negocio.P_Tramite_ID = Cls_Sessiones.No_Empleado;
            Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Tramite, Ds_Tramites.Tables[0], 3, 0);
            Cmb_Tramite.SelectedIndex = Cmb_Tramite.Items.IndexOf(Cmb_Tramite.Items.FindByValue(Cls_Sessiones.No_Empleado));
            Hdf_Dependencia_ID.Value = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString();


            //  para los tramites pertenecientes a ordenamiento territorial
            if (Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Ordenamiento
                || Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Ambiental
                || Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Inmobiliario
                || Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Urbanistico
                || Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Catastro)
            {
                Pnl_Cuenta_Predial.Visible = true;
                Hdf_Cuenta_Predial.Value = "Si";
                if (Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Catastro && (Cmb_Tramite.SelectedItem.Text == "Avaluo" || Cmb_Tramite.SelectedItem.Text == "Solicitud de registro" || Cmb_Tramite.SelectedItem.Text == "Solicitud de refrendo"))
                {
                    Btn_Buscar_Cuenta_Predial.Visible = false;
                    Btn_Link_Catastro.Visible = true;
                    if (Cmb_Tramite.SelectedItem.Text == "Avaluo")
                    {
                        Btn_Link_Catastro2.Visible = true;
                        Cmb_Perito.Visible = true;
                        Btn_Buscar_Perito.Visible = false;
                    }
                    else
                    {
                        Btn_Link_Catastro2.Visible = false;
                        Cmb_Perito.Visible = false;
                        Btn_Buscar_Perito.Visible = false;
                    }
                    Btn_Nuevo.Visible = false;
                    LLenar_Combos_Catastro();
                }
            }
            else
            {
                Pnl_Cuenta_Predial.Visible = false;
                Hdf_Cuenta_Predial.Value = "No";
            }
            //  para el usuario
            //  para el usuario
            if (Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Catastro)
            {
                Lbl_Costo.Text = "Costo por unidad";
            }
            else
            {
                Lbl_Costo.Text = "Costo";
            }


            //Rs_Datos_Usuario.P_Email = Cls_Sessiones.Nombre_Empleado;
            //Rs_Datos_Usuario.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
            //Dt_Datos_Usuario = Rs_Datos_Usuario.Validar_Usuario();

            //cargar las cajas de texto
            Txt_Avance.Text = "0";
            //Cls_Sessiones.Datos_Ciudadano

            Txt_Nombre.Text = Cls_Sessiones.Datos_Ciudadano.Rows[0][Cat_Ven_Usuarios.Campo_Nombre].ToString();
            Txt_Apellido_Paterno.Text = Cls_Sessiones.Datos_Ciudadano.Rows[0][Cat_Ven_Usuarios.Campo_Apellido_Paterno].ToString();
            Txt_Apellido_Materno.Text = Cls_Sessiones.Datos_Ciudadano.Rows[0][Cat_Ven_Usuarios.Campo_Apellido_Materno].ToString();
            Txt_Nombre_Completo.Text = Txt_Nombre.Text + " " + Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text;
            Txt_Email.Text = Cls_Sessiones.Datos_Ciudadano.Rows[0][Cat_Ven_Usuarios.Campo_Email].ToString();

            Cargar_Grid_Datos(Solicitud_Negocio);
            Cargar_Grid_Documentos(Solicitud_Negocio);
            Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
            Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
            //Txt_Costo.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Costo].ToString();
            Hdf_Costo.Value = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Costo].ToString();
            Txt_Tiempo_Estimado.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Tiempo_Estimado].ToString();

            //  se asigna el nombre al link
            Btn_Link_Catastro.Text = "Llenar " + Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString();
            if (Cmb_Tramite.SelectedItem.Text == "Avaluo")
            {
                Btn_Link_Catastro.Text += " Urbano";
                Btn_Link_Catastro2.Text = "Llenar Avaluo Rústico";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: FileUp_Subir_Archivo_UploadedComplete
    /// DESCRIPCION :subira el documento a su carpeta
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 30/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void FileUp_Subir_Archivo_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
    {
        int Fila = 0;
        TableCell Celda = new TableCell();
        GridViewRow Renglon;
        AsyncFileUpload Boton = new AsyncFileUpload();
        try
        {
            Boton = (AsyncFileUpload)sender;
            Celda = (TableCell)Boton.Parent;
            Renglon = (GridViewRow)Celda.Parent;
            Grid_Documentos.SelectedIndex = Renglon.RowIndex;
            Fila = Renglon.RowIndex;

            AsyncFileUpload Afu_Subir_Archivo = (AsyncFileUpload)Grid_Documentos.Rows[Fila].Cells[2].FindControl("FileUp");
            TextBox Txt_Url = (TextBox)Grid_Documentos.Rows[Fila].FindControl("Txt_Url");

            if (Afu_Subir_Archivo.HasFile)
            {
                Txt_Url.Text = Afu_Subir_Archivo.FileName;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Archivo
    ///DESCRIPCIÓN:          Muestra un Archivo del cual se le pasa la ruta como parametro.
    ///PARAMETROS:           1.  Ruta.  Ruta del Archivo.
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           24/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    public void Mostrar_Archivo(String Ruta)
    {
        //String Archivo = "";
        try
        {
            if (System.IO.File.Exists(Ruta))
            {
                //System.Diagnostics.Process proceso = new System.Diagnostics.Process();
                //proceso.StartInfo.FileName = Ruta;
                //proceso.Start();

                String Archivo = "../../Portafolio/" + Cls_Sessiones.Ciudadano_ID + "/" + Path.GetFileName(Ruta);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Archivo + "','Window_Archivo','left=0,top=0')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('El archivo no existe');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "');", true);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Cargar_Grid_Documentos
    /// DESCRIPCION : cargara la informacion del grid
    /// PARAMETROS  : clase de negocios Cls_Ope_Solicitud_Tramites_Negocio
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public void Cargar_Grid_Documentos(Cls_Ope_Solicitud_Tramites_Negocio Solicitud_Negocio)
    {
        DataSet Ds_Documentos = new DataSet();
        try
        {
            Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
            Ds_Documentos = Solicitud_Negocio.Consultar_Documentos_Tramite();

            //  se ordenara la tabla por nombre
            DataView Dv_Ordenar = new DataView(Ds_Documentos.Tables[0]);
            Dv_Ordenar.Sort = "DOCUMENTO_REQUERIDO desc, DOCUMENTO asc";
            DataTable Dt_Datos_Ordenados = Dv_Ordenar.ToTable();
            Session["Grid_Documentos"] = Dt_Datos_Ordenados;

            if (Ds_Documentos.Tables[0].Rows.Count > 0)
            {
                Grid_Documentos.Columns[2].Visible = true;
                Grid_Documentos.Columns[3].Visible = true;
                Grid_Documentos.Visible = true;
                Grid_Documentos.DataSource = Dt_Datos_Ordenados;
                Grid_Documentos.DataBind();
                Grid_Documentos.Columns[2].Visible = false;
                Grid_Documentos.Columns[3].Visible = true;

                Lbl_Documentos_Requeridos.Visible = true;
                Lbl_Documento_Opcional.Visible = true;
                Div_Grid_Documentos.Style.Value = "overflow:auto;height:150px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:block";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Cargar_Grid_Datos
    /// DESCRIPCION : cargara la informacion del grid
    /// PARAMETROS  : clase de negocios Cls_Ope_Solicitud_Tramites_Negocio
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public void Cargar_Grid_Datos(Cls_Ope_Solicitud_Tramites_Negocio Solicitud_Negocio)
    {
        DataSet Ds_Datos = new DataSet();
        try
        {
            Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
            Solicitud_Negocio.P_Tipo_Dato = "INICIAL";
            Ds_Datos = Solicitud_Negocio.Consultar_Datos_Tramite();

            //  se ordenara la tabla por fecha
            DataView Dv_Ordenar = new DataView(Ds_Datos.Tables[0]);
            Dv_Ordenar.Sort = "ORDEN";
            DataTable Dt_Datos_Ordenados = Dv_Ordenar.ToTable();
            Session["Grid_Datos"] = Dt_Datos_Ordenados;

            if (Dt_Datos_Ordenados.Rows.Count > 0)
            {
                Grid_Datos.Columns[2].Visible = true;
                Grid_Datos.DataSource = Dt_Datos_Ordenados;
                Grid_Datos.DataBind();
                Grid_Datos.Columns[2].Visible = false;
                Grid_Datos.Visible = true;
                Lbl_Datos_Requeridos.Visible = true;
                Div_Grid_Datos_Tramite.Style.Value = "overflow:auto;height:150px;width:99%;vertical-align:top;border-style:outset;border-color:Silver;display:block";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Alta_Tramite
    /// DESCRIPCION : Realizara el alta del tramite que se solicita
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Alta_Tramite()
    {
        Cls_Ope_Solicitud_Tramites_Negocio Rs_Alta = new Cls_Ope_Solicitud_Tramites_Negocio();
        DataTable Dt_Documentos = (DataTable)(Session["Grid_Documentos"]);
        DataTable Dt_Datos = (DataTable)(Session["Grid_Datos"]);
        DataSet Ds_Tramite;
        String Valor_Dato = "";
        String Extension = "";
        String Direccion_Archivo = "";
        String Directorio_Expediente = "";
        String Raiz = "";
        String URL = "";
        Boolean Operacion_Completa = false;
        String Directorio_Portafolio = "";
        String Nombre_Archivo = "";
        try
        {
            //  cambiarlo por las sesiones
            String[,] Datos = new String[Dt_Datos.Rows.Count, 2];
            String[,] Documentos = new String[Dt_Documentos.Rows.Count, 2];

            if (Validar_Datos_Grid_Datos())
            {
                if (Validar_Datos_Grid_Documentos())
                {
                    //  se pasa la informacion a la capa de negocio
                    DateTime Fecha_Solucion;
                    String Nombre = "";
                    String Email = "";
                    String Telefono = "";
                    String Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim().ToUpper();
                    string Perito_ID;
                    string Cuenta_Predial_Visible = "";
                    string Nombre_Tramite = "";
                    String Dependencia_ID = "";

                    //  para obtener la informacion de los datos
                    for (int Contador_For = 0; Contador_For < Dt_Datos.Rows.Count; Contador_For++)
                    {
                        Datos[Contador_For, 0] = Dt_Datos.Rows[Contador_For].ItemArray[0].ToString();

                        String Temporal = Grid_Datos.Rows[Contador_For].Cells[0].Text;

                        Valor_Dato = ((TextBox)Grid_Datos.Rows[Contador_For].FindControl("Txt_Descripcion_Datos")).Text;

                        if (Valor_Dato != "" || (Dt_Datos.Rows[Contador_For][Cat_Tra_Datos_Tramite.Campo_Dato_Requerido].ToString()) == "N")
                            Datos[Contador_For, 1] = Valor_Dato;
                    }

                    if (Dt_Documentos.Rows.Count > 0 && Dt_Documentos != null)
                    {
                        for (int Cnt_Documentos = 0; Cnt_Documentos < Dt_Documentos.Rows.Count; Cnt_Documentos++)
                        {

                            for (int Cnt_Documentos_Celdas = 0; Cnt_Documentos_Celdas < 2; Cnt_Documentos_Celdas++)
                            {
                                AsyncFileUpload AsFileUp = (AsyncFileUpload)Grid_Documentos.Rows[Cnt_Documentos].Cells[2].FindControl("FileUp");
                                String Nombre_Documento = Dt_Documentos.Rows[Cnt_Documentos][1].ToString();
                                TextBox Txt_Url = (TextBox)Grid_Documentos.Rows[Cnt_Documentos].Cells[2].FindControl("Txt_Url");

                                if (AsFileUp.FileName != "")
                                    Extension = Obtener_Extension(AsFileUp.FileName);
                                else
                                    Extension = Obtener_Extension(Txt_Url.Text);

                                if (Dt_Documentos.Rows[Cnt_Documentos][Tra_Detalle_Documentos.Campo_Documento_Requerido].ToString() == "S" ||
                                    AsFileUp.FileName != "" || Txt_Url.Text != "")
                                {
                                    if (Cnt_Documentos_Celdas == 0)
                                        Documentos[Cnt_Documentos, Cnt_Documentos_Celdas] = Dt_Documentos.Rows[Cnt_Documentos].ItemArray[0].ToString();

                                    if (Cnt_Documentos_Celdas == 1)
                                    {
                                        Directorio_Expediente = "TR-";
                                        Raiz = @Server.MapPath("../../Archivos");

                                        Direccion_Archivo = Raiz + "/" + Directorio_Expediente +
                                                   "/" + Server.HtmlEncode(Dt_Documentos.Rows[Cnt_Documentos].ItemArray[3].ToString() +
                                                   "_" + Dt_Documentos.Rows[Cnt_Documentos].ItemArray[2].ToString() +
                                                   "." + Extension);

                                        Documentos[Cnt_Documentos, Cnt_Documentos_Celdas] = Direccion_Archivo;
                                    }
                                }
                            }
                        }
                    }

                    //  para el filtro del telefono
                    if ((Cls_Sessiones.Datos_Ciudadano != null) && (Cls_Sessiones.Datos_Ciudadano.Rows.Count > 0))
                    {
                        foreach (DataRow Registro in Cls_Sessiones.Datos_Ciudadano.Rows)
                        {
                            Telefono = Registro[Cat_Ven_Usuarios.Campo_Telefono_Casa].ToString();
                        }
                    }

                    var Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                    Fecha_Solucion = Dias_Inhabilies.Calcular_Fecha("" + DateTime.Today, Txt_Tiempo_Estimado.Text);

                    Rs_Alta.P_Fecha_Entrega = Fecha_Solucion;
                    Rs_Alta.P_Porcentaje = Txt_Avance.Text;
                    Rs_Alta.P_Tramite_ID = Cmb_Tramite.SelectedValue;
                    Ds_Tramite = Rs_Alta.Consultar_Tramites();

                    if (Ds_Tramite.Tables.Count > 0)
                        if (Ds_Tramite.Tables[0].Rows.Count > 0)
                            Rs_Alta.P_Folio = Ds_Tramite.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Clave_Tramite].ToString();


                    Rs_Alta.P_E_Mail = Txt_Email.Text;
                    Rs_Alta.P_Estatus = Cmb_Estatus.SelectedValue;
                    Rs_Alta.P_Nombre_Solicitante = Txt_Nombre.Text;
                    String Clave_Unica = Cls_Util.Generar_Folio_Tramite();

                    Rs_Alta.P_Clave_Solicitud = Clave_Unica;
                    //  se consulta el subproceso para pasarse a la capa de negocio
                    DataSet Ds_Subproceso = Rs_Alta.Consultar_Subproceso();
                    Rs_Alta.P_Subproceso_ID = Ds_Subproceso.Tables[0].Rows[0].ItemArray[0].ToString();
                    Rs_Alta.P_Datos = Datos;
                    Rs_Alta.P_Documentos = Documentos;

                    //  para el apellido paterno
                    if (!String.IsNullOrEmpty(Txt_Apellido_Materno.Text))
                        Rs_Alta.P_Apellido_Materno = Txt_Apellido_Materno.Text;

                    else
                        Rs_Alta.P_Apellido_Materno = "X";

                    Rs_Alta.P_Apellido_Paterno = Txt_Apellido_Paterno.Text;
                    Rs_Alta.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim().ToUpper();
                    Rs_Alta.P_Direccion_Predio = Txt_Direccion_Predio.Text;
                    Rs_Alta.P_Propietario_Predio = Txt_Propietario_Cuenta_Predial.Text;
                    Rs_Alta.P_Calle_Predio = Txt_Calle_Predio.Text;
                    Rs_Alta.P_Nuemro_Predio = Txt_Numero_Predio.Text;
                    Rs_Alta.P_Manzana_Predio = Txt_Manzana_Predio.Text;
                    Rs_Alta.P_Lote_Predio = Txt_Lote_Predio.Text;
                    Rs_Alta.P_Otros_Predio = Txt_Otros_Predio.Text;

                    // si hay un perito seleccionado, pasar a la clase de negocio
                    if (Cmb_Perito.SelectedIndex > 0)
                        Rs_Alta.P_Perito_ID = Cmb_Perito.SelectedValue;

                    if (Txt_Propietario_Cuenta_Predial.Text != "")
                        Nombre = Txt_Propietario_Cuenta_Predial.Text;

                    else
                        Nombre = Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text + " " + Txt_Nombre.Text;

                    Email = Txt_Email.Text;
                    Perito_ID = Cmb_Perito.SelectedValue;
                    Dependencia_ID = Hdf_Dependencia_ID.Value;
                    Cuenta_Predial_Visible = Hdf_Cuenta_Predial.Value;
                    string Nombre_Propietario = "";

                    if (Txt_Propietario_Cuenta_Predial.Text != "")
                        Nombre_Propietario = Txt_Propietario_Cuenta_Predial.Text;
                    else
                        Nombre_Propietario = "";

                    // si hay un trámite seleccionado, guardar dato para reporte
                    if (Cmb_Tramite.SelectedIndex > 0)
                        Nombre_Tramite = Cmb_Tramite.SelectedItem.Text;

                    //  se agregan los costos a la solicitud
                    Rs_Alta.P_Costo_Base = Hdf_Costo.Value;

                    if (Txt_Cantidad_Solicitud.Text == "" || Convert.ToInt16(Txt_Tiempo_Estimado.Text) == 0)
                        Txt_Cantidad_Solicitud.Text = "1";

                    Rs_Alta.P_Cantidad = Convert.ToInt16(Txt_Cantidad_Solicitud.Text).ToString();
                    Rs_Alta.P_Contribuyente_ID = Cls_Sessiones.Ciudadano_ID;
                    double Cantidad_Total_Final = Convert.ToDouble(Hdf_Costo.Value) * Convert.ToDouble(Txt_Cantidad_Solicitud.Text);
                    Rs_Alta.P_Costo_Total = "" + Cantidad_Total_Final;

                    //   *********************** inicio alta la solicitud *******************************************************************
                    String Consecutivo = Rs_Alta.Alta_Solicitud(Cls_Sessiones.Datos_Ciudadano.Rows[0][Cat_Ven_Usuarios.Campo_Email].ToString());
                    //   *********************** fin alta la solicitud **********************************************************************

                    //  para obtener la informacion de los documentos
                    for (int Contador_For = 0; Contador_For < Dt_Documentos.Rows.Count; Contador_For++)
                    {
                        Documentos[Contador_For, 0] = Dt_Documentos.Rows[Contador_For].ItemArray[0].ToString();
                        String Nombre_Documento = Dt_Documentos.Rows[Contador_For][2].ToString();
                        AsyncFileUpload AsFileUp = (AsyncFileUpload)Grid_Documentos.Rows[Contador_For].Cells[1].FindControl("FileUp");
                        TextBox Txt_Url = (TextBox)Grid_Documentos.Rows[Contador_For].Cells[1].FindControl("Txt_Url");
                        Extension = Obtener_Extension(AsFileUp.FileName);

                        if (Extension == "pdf" || Extension == "jpg" || Extension == "jpeg" || Extension == "rar" || Extension == "zip" || Extension == "dwg" || Txt_Url.Text != "")
                        {
                            //  para formar la dirección del archivo
                            Directorio_Expediente = "TR-" + Consecutivo;
                            Raiz = @Server.MapPath("../../Archivos");
                            Direccion_Archivo = "";

                            if (!Directory.Exists(Raiz))
                                Directory.CreateDirectory(Raiz);

                            if (AsFileUp.FileName != "")
                                URL = AsFileUp.FileName;

                            else
                                URL = Txt_Url.Text;

                            Extension = Obtener_Extension(URL);

                            if (URL != "")
                            {
                                if (!Directory.Exists(Raiz + Directorio_Expediente))
                                    Directory.CreateDirectory(Raiz + "/" + Directorio_Expediente);

                                //se crea el Nombre_Commando del archivo que se va a guardar
                                Direccion_Archivo = Raiz + "/" + Directorio_Expediente +
                                    "/" + Server.HtmlEncode(Dt_Documentos.Rows[Contador_For].ItemArray[3].ToString() +
                                    "_" + Dt_Documentos.Rows[Contador_For].ItemArray[2].ToString() +
                                    "." + Extension);

                                //se valida que contega un archivo 

                                if (AsFileUp.HasFile)
                                {
                                    //se guarda el archivo
                                    AsFileUp.SaveAs(Direccion_Archivo);

                                    // se subira el archivo al portafolio*************************
                                    Directorio_Portafolio = Cls_Sessiones.Ciudadano_ID;
                                    Raiz = @Server.MapPath("../../Portafolio");
                                    URL = AsFileUp.FileName;

                                    String[] Archivos = Directory.GetFiles(MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));

                                    //verifica si existe un directorio 
                                    if (!Directory.Exists(Raiz + "/" + Directorio_Portafolio))
                                        Directory.CreateDirectory(Raiz + "/" + Directorio_Portafolio);

                                    //  se busca el archivo
                                    for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                                    {
                                        Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                                        if (Nombre_Archivo.Contains(Nombre_Documento))
                                        {
                                            System.IO.File.Delete(Archivos[Contador].Trim());
                                            break;
                                        }

                                    }// fin del for

                                    // ejemplo: Portafolio/0000000002/0000000003_
                                    //          Ife.jpg
                                    Direccion_Archivo = Raiz + "/" + Directorio_Portafolio + "/" + Server.HtmlEncode(Dt_Documentos.Rows[Contador_For].ItemArray[3].ToString() +
                                        "_" + Dt_Documentos.Rows[Contador_For].ItemArray[2].ToString() + "." + Extension);

                                    if (AsFileUp.HasFile)
                                    {
                                        //se guarda el archivo
                                        AsFileUp.SaveAs(Direccion_Archivo);
                                    }// fin del if (AFU_Subir_Archivo.HasFile)************************
                                }
                                else
                                    System.IO.File.Copy(Txt_Url.Text, Direccion_Archivo);

                                Documentos[Contador_For, 1] = Direccion_Archivo;

                            }// fin del if url

                        }// fin de el if Extension

                    }// fin del for


                    String Solicitud_ID = Consecutivo;

                    ////  se consulta los datos de la solicitud generada
                    //Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                    //Negocio_Datos_Solicitud.P_Solicitud_ID = Solicitud_ID;
                    //Negocio_Datos_Solicitud = Negocio_Datos_Solicitud.Consultar_Datos_Solicitud();

                    ////  se genere el reporte sin las fechas
                    //Generar_Reporte_Folio_Solicitud(Clave_Unica, Fecha_Solucion, Nombre, Email, Nombre_Tramite, Cuenta_Predial,
                    //    Nombre_Propietario, Negocio_Datos_Solicitud.P_Consecutivo, Negocio_Datos_Solicitud.P_Dependencia_ID, Negocio_Datos_Solicitud.P_Area_Dependencia, "");

                    Habilitar_Controles("Inicial");
                    Btn_Nuevo.Visible = false;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Programas Empleados", "alert('Alta de tramite');", true);

                }// fin de la validacion

            }// fin de la validacion            
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Tramite " + ex.Message.ToString(), ex);
        }
        return Operacion_Completa;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Alta_Tramite_Catastro
    /// DESCRIPCION : Realizara el alta del tramite que se solicita
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public String Alta_Tramite_Catastro()
    {
        Cls_Ope_Solicitud_Tramites_Negocio Rs_Alta = new Cls_Ope_Solicitud_Tramites_Negocio();
        DataTable Dt_Documentos = (DataTable)(Session["Grid_Documentos"]);
        DataTable Dt_Datos = (DataTable)(Session["Grid_Datos"]);
        DataSet Ds_Tramite;
        String Valor_Dato = "";
        String Extension = "";
        String Direccion_Archivo = "";
        String Directorio_Expediente = "";
        String Raiz = "";
        String URL = "";
        Boolean Operacion_Completa = false;
        String Directorio_Portafolio = "";
        String Nombre_Archivo = "";
        String Solicitud_ID = "";
        String Consecutivo_ID = "";
        try
        {

            //  cambiarlo por las sesiones
            String[,] Datos = new String[Dt_Datos.Rows.Count, 2];
            String[,] Documentos = new String[Dt_Documentos.Rows.Count, 2];


            if (Validar_Datos_Grid_Datos())
            {
                if (Validar_Datos_Grid_Documentos())
                {
                    //  para obtener la informacion de los datos
                    for (int Contador_For = 0; Contador_For < Dt_Datos.Rows.Count; Contador_For++)
                    {
                        Datos[Contador_For, 0] = Dt_Datos.Rows[Contador_For].ItemArray[0].ToString();

                        String Temporal = Grid_Datos.Rows[Contador_For].Cells[0].Text;

                        Valor_Dato = ((TextBox)Grid_Datos.Rows[Contador_For].FindControl("Txt_Descripcion_Datos")).Text;

                        if (Valor_Dato != "" || (Dt_Datos.Rows[Contador_For][Cat_Tra_Datos_Tramite.Campo_Dato_Requerido].ToString()) == "N")
                        {
                            Datos[Contador_For, 1] = Valor_Dato;
                        }
                    }

                    if (Dt_Documentos.Rows.Count > 0 && Dt_Documentos != null)
                    {
                        for (int Cnt_Documentos = 0; Cnt_Documentos < Dt_Documentos.Rows.Count; Cnt_Documentos++)
                        {

                            for (int Cnt_Documentos_Celdas = 0; Cnt_Documentos_Celdas < 2; Cnt_Documentos_Celdas++)
                            {
                                AsyncFileUpload AsFileUp = (AsyncFileUpload)Grid_Documentos.Rows[Cnt_Documentos].Cells[2].FindControl("FileUp");
                                String Nombre_Documento = Dt_Documentos.Rows[Cnt_Documentos][1].ToString();
                                TextBox Txt_Url = (TextBox)Grid_Documentos.Rows[Cnt_Documentos].Cells[2].FindControl("Txt_Url");

                                if (AsFileUp.FileName != "")
                                    Extension = Obtener_Extension(AsFileUp.FileName);
                                else
                                    Extension = Obtener_Extension(Txt_Url.Text);

                                if (Dt_Documentos.Rows[Cnt_Documentos][Tra_Detalle_Documentos.Campo_Documento_Requerido].ToString() == "S" ||
                                    AsFileUp.FileName != "" || Txt_Url.Text != "")
                                {
                                    if (Cnt_Documentos_Celdas == 0)
                                        Documentos[Cnt_Documentos, Cnt_Documentos_Celdas] = Dt_Documentos.Rows[Cnt_Documentos].ItemArray[0].ToString();

                                    if (Cnt_Documentos_Celdas == 1)
                                    {
                                        Directorio_Expediente = "TR-";
                                        Raiz = @Server.MapPath("../../Archivos");

                                        Direccion_Archivo = Raiz + "/" + Directorio_Expediente +
                                                   "/" + Server.HtmlEncode(Dt_Documentos.Rows[Cnt_Documentos].ItemArray[3].ToString() +
                                                   "_" + Dt_Documentos.Rows[Cnt_Documentos].ItemArray[2].ToString() +
                                                   "." + Extension);

                                        Documentos[Cnt_Documentos, Cnt_Documentos_Celdas] = Direccion_Archivo;
                                    }
                                }
                            }
                        }
                    }

                    //  se pasa la informacion a la capa de negocio
                    DateTime Fecha_Solucion;
                    String Nombre = "";
                    String Email = "";
                    String Telefono = "";
                    String Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim().ToUpper();
                    string Perito_ID;
                    string Cuenta_Predial_Visible = "";
                    string Nombre_Tramite = "";
                    String Dependencia_ID = "";

                    //  para el filtro del telefono
                    if ((Cls_Sessiones.Datos_Ciudadano != null) && (Cls_Sessiones.Datos_Ciudadano.Rows.Count > 0))
                    {
                        foreach (DataRow Registro in Cls_Sessiones.Datos_Ciudadano.Rows)
                        {
                            Telefono = Registro[Cat_Ven_Usuarios.Campo_Telefono_Casa].ToString();
                        }
                    }

                    var Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                    Fecha_Solucion = Dias_Inhabilies.Calcular_Fecha("" + DateTime.Today, Txt_Tiempo_Estimado.Text);

                    Rs_Alta.P_Fecha_Entrega = Fecha_Solucion;
                    Rs_Alta.P_Porcentaje = Txt_Avance.Text;
                    Rs_Alta.P_Tramite_ID = Cmb_Tramite.SelectedValue;
                    Ds_Tramite = Rs_Alta.Consultar_Tramites();

                    if (Ds_Tramite.Tables.Count > 0)
                        if (Ds_Tramite.Tables[0].Rows.Count > 0)
                            Rs_Alta.P_Folio = Ds_Tramite.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Clave_Tramite].ToString();


                    Rs_Alta.P_E_Mail = Txt_Email.Text;
                    Rs_Alta.P_Estatus = Cmb_Estatus.SelectedValue;
                    Rs_Alta.P_Nombre_Solicitante = Txt_Nombre.Text;
                    String Clave_Unica = Cls_Util.Generar_Folio_Tramite();

                    if (Clave_Unica.Length > 13)
                    {
                        Clave_Unica = Clave_Unica.Substring(0, 8);
                    }
                    Rs_Alta.P_Clave_Solicitud = Clave_Unica;
                    //  se consulta el subproceso para pasarse a la capa de negocio
                    DataSet Ds_Subproceso = Rs_Alta.Consultar_Subproceso();
                    Rs_Alta.P_Subproceso_ID = Ds_Subproceso.Tables[0].Rows[0].ItemArray[0].ToString();
                    Rs_Alta.P_Datos = Datos;
                    Rs_Alta.P_Documentos = Documentos;

                    //  para el apellido paterno
                    if (!String.IsNullOrEmpty(Txt_Apellido_Materno.Text))
                    {
                        Rs_Alta.P_Apellido_Materno = Txt_Apellido_Materno.Text;
                    }
                    else
                    {
                        Rs_Alta.P_Apellido_Materno = "null";
                    }
                    Rs_Alta.P_Apellido_Paterno = Txt_Apellido_Paterno.Text;
                    Rs_Alta.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim().ToUpper();
                    // si hay un perito seleccionado, pasar a la clase de negocio
                    if (Cmb_Perito.SelectedIndex > 0)
                    {
                        Rs_Alta.P_Perito_ID = Cmb_Perito.SelectedValue;
                    }

                    if (Txt_Propietario_Cuenta_Predial.Text != "")
                        Nombre = Txt_Propietario_Cuenta_Predial.Text;

                    else
                        Nombre = Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text + " " + Txt_Nombre.Text;

                    Email = Txt_Email.Text;
                    Perito_ID = Cmb_Perito.SelectedValue;
                    Dependencia_ID = Hdf_Dependencia_ID.Value;
                    Cuenta_Predial_Visible = Hdf_Cuenta_Predial.Value;
                    string Nombre_Propietario = "";
                    if (Txt_Propietario_Cuenta_Predial.Text != "")
                        Nombre_Propietario = Txt_Propietario_Cuenta_Predial.Text;
                    else
                        Nombre_Propietario = Txt_Apellido_Paterno.Text + " " + Txt_Apellido_Materno.Text + " " + Txt_Nombre.Text;

                    // si hay un trámite seleccionado, guardar dato para reporte
                    if (Cmb_Tramite.SelectedIndex > 0)
                    {
                        Nombre_Tramite = Cmb_Tramite.SelectedItem.Text;
                    }

                    //  se agregan los costos a la solicitud
                    Rs_Alta.P_Costo_Base = Hdf_Costo.Value;
                    Rs_Alta.P_Cantidad = "1";
                    Rs_Alta.P_Costo_Total = Hdf_Costo.Value;


                    /********************************   para dar de alta la solicitud ***************************************/
                    Consecutivo_ID = Rs_Alta.Alta_Solicitud(Cls_Sessiones.Datos_Ciudadano.Rows[0][Cat_Ven_Usuarios.Campo_Email].ToString());
                    Operacion_Completa = true;
                    /********************************************************************************************************/

                    //  para obtener la informacion de los documentos
                    for (int Contador_For = 0; Contador_For < Dt_Documentos.Rows.Count; Contador_For++)
                    {
                        Documentos[Contador_For, 0] = Dt_Documentos.Rows[Contador_For].ItemArray[0].ToString();
                        String Nombre_Documento = Dt_Documentos.Rows[Contador_For][2].ToString();
                        AsyncFileUpload AsFileUp = (AsyncFileUpload)Grid_Documentos.Rows[Contador_For].Cells[1].FindControl("FileUp");
                        TextBox Txt_Url = (TextBox)Grid_Documentos.Rows[Contador_For].Cells[1].FindControl("Txt_Url");
                        Extension = Obtener_Extension(AsFileUp.FileName);

                        if (Extension == "pdf" || Extension == "jpg" || Extension == "jpeg" || Extension == "rar" || Extension == "zip" || Txt_Url.Text != "")
                        {
                            //  para formar la dirección del archivo
                            Directorio_Expediente = "TR-" + Consecutivo_ID;
                            Raiz = @Server.MapPath("../../Archivos");
                            Direccion_Archivo = "";

                            if (!Directory.Exists(Raiz))
                                Directory.CreateDirectory(Raiz);

                            if (AsFileUp.FileName != "")
                                URL = AsFileUp.FileName;

                            else
                                URL = Txt_Url.Text;

                            Extension = Obtener_Extension(URL);

                            if (URL != "")
                            {
                                if (!Directory.Exists(Raiz + Directorio_Expediente))
                                    Directory.CreateDirectory(Raiz + "/" + Directorio_Expediente);

                                //se crea el Nombre_Commando del archivo que se va a guardar
                                Direccion_Archivo = Raiz + "/" + Directorio_Expediente +
                                    "/" + Server.HtmlEncode(Dt_Documentos.Rows[Contador_For].ItemArray[3].ToString() +
                                    "_" + Dt_Documentos.Rows[Contador_For].ItemArray[2].ToString() +
                                    "." + Extension);

                                //se valida que contega un archivo 

                                if (AsFileUp.HasFile)
                                {
                                    //se guarda el archivo
                                    AsFileUp.SaveAs(Direccion_Archivo);

                                    // se subira el archivo al portafolio*************************
                                    Directorio_Portafolio = Cls_Sessiones.Ciudadano_ID;
                                    Raiz = @Server.MapPath("../../Portafolio");
                                    URL = AsFileUp.FileName;

                                    String[] Archivos = Directory.GetFiles(MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));

                                    //verifica si existe un directorio 
                                    if (!Directory.Exists(Raiz + "/" + Directorio_Portafolio))
                                        Directory.CreateDirectory(Raiz + "/" + Directorio_Portafolio);

                                    //  se busca el archivo
                                    for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                                    {
                                        Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                                        if (Nombre_Archivo.Contains(Nombre_Documento))
                                        {
                                            System.IO.File.Delete(Archivos[Contador].Trim());
                                            break;
                                        }

                                    }// fin del for

                                    // ejemplo: Portafolio/0000000002/0000000003_
                                    //          Ife.jpg
                                    Direccion_Archivo = Raiz + "/" + Directorio_Portafolio + "/" + Server.HtmlEncode(Dt_Documentos.Rows[Contador_For].ItemArray[3].ToString() +
                                        "_" + Dt_Documentos.Rows[Contador_For].ItemArray[2].ToString() + "." + Extension);

                                    if (AsFileUp.HasFile)
                                    {
                                        //se guarda el archivo
                                        AsFileUp.SaveAs(Direccion_Archivo);
                                    }// fin del if (AFU_Subir_Archivo.HasFile)************************
                                }
                                else
                                    System.IO.File.Copy(Txt_Url.Text, Direccion_Archivo);

                                Documentos[Contador_For, 1] = Direccion_Archivo;

                            }// fin del if url

                        }// fin de el if Extension

                    }// fin del for

                    //  se obtendra el id de la solicitud    
                    Solicitud_ID = Consecutivo_ID;

                }// fin de la validacion

            }// fin de la validacion            
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Tramite " + ex.Message.ToString(), ex);
        }
        return Solicitud_ID;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Extension
    ///DESCRIPCIÓN:     Maneja la Extension del archivo
    ///PROPIEDADES:     String Ruta, direccion que 
    ///                 contiene el nombre del archivo al cual se le sacara la extension
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      03/Mayo/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    private string Obtener_Extension(String Ruta)
    {
        String Extension = "";
        int index = Ruta.LastIndexOf(".");
        if (index < Ruta.Length)
        {
            Extension = Ruta.Substring(index + 1);
        }
        return Extension;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Extension
    ///DESCRIPCIÓN:     Maneja la Extension del archivo
    ///PROPIEDADES:     String Ruta, direccion que 
    ///                 contiene el nombre del archivo al cual se le sacara la extension
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      03/Mayo/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    private void Generar_Reporte_Solicitud(String Clave_Unica, DateTime Fecha_Solucion, String Nombre_Completos, String Email)
    {
        Cls_Rpt_Ven_Consultar_Tramites_Negocio Negocio_Actividades_Realizadas = new Cls_Rpt_Ven_Consultar_Tramites_Negocio();
        DataTable Dt_Consulta_Solicitud = new DataTable();
        DataTable Dt_Actividades_Realizadas = new DataTable();
        DataTable Dt_Reporte_Datos = new DataTable();
        DataTable Dt_Datos = (DataTable)(Session["Grid_Datos"]);
        String Valor_Dato = "";

        Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite Ds_Reporte = new Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite();
        DataSet Ds_Consulta = new DataSet();
        try
        {
            //  previsualizacion de la solicitud
            Dt_Consulta_Solicitud = Ds_Reporte.Dt_Datos_Solicitud.Clone();
            Dt_Actividades_Realizadas = Ds_Reporte.Dt_Seguimiento_Solicitud.Clone();
            Dt_Reporte_Datos = Ds_Reporte.Dt_Datos.Clone();

            DataRow Fila = Dt_Consulta_Solicitud.NewRow();
            Fila["CLAVE_SOLICITUD"] = Clave_Unica;
            Fila["PORCENTAJE_AVANCE"] = "0";
            Fila["ESTATUS"] = "PENDIENTE";
            Fila["FECHA_TRAMITE"] = DateTime.Today;
            Fila["FECHA_ENTREGA"] = Fecha_Solucion;
            Fila["NOMBRE_COMPLETO"] = Nombre_Completos;
            Fila["CORREO_ELECTRONICO"] = Email;
            Fila["NOMBRE"] = "";
            Dt_Consulta_Solicitud.Rows.Add(Fila);

            String[,] Datos = new String[Dt_Datos.Rows.Count, 2];
            //  para obtener la informacion de los datos
            for (int Contador_For = 0; Contador_For < Dt_Datos.Rows.Count; Contador_For++)
            {
                Datos[Contador_For, 0] = Dt_Datos.Rows[Contador_For].ItemArray[0].ToString();
                String Nombre_Dato = Grid_Datos.Rows[Contador_For].Cells[0].Text;
                Valor_Dato = ((TextBox)Grid_Datos.Rows[Contador_For].FindControl("Txt_Descripcion_Datos")).Text;
                Fila = Dt_Reporte_Datos.NewRow();
                Fila["NOMBRE_DATO"] = Nombre_Dato;
                Fila["VALOR"] = Valor_Dato;
                Dt_Reporte_Datos.Rows.Add(Fila);
            }

            Dt_Consulta_Solicitud.TableName = "Dt_Datos_Solicitud";
            Dt_Actividades_Realizadas.TableName = "Dt_Seguimiento_Solicitud";

            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Consulta_Solicitud.Copy());
            Ds_Reporte.Tables.Add(Dt_Actividades_Realizadas.Copy());
            Ds_Reporte.Tables.Add(Dt_Reporte_Datos.Copy());
            Generar_Reporte(Ds_Reporte, "PDF", "Folio_Solicitud", "../Rpt/Ordenamiento_Territorial/Rpt_Ort_Seguimiento_Solicitud_Tramite.rpt");

        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Tramite " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Generar_Reporte_Folio_Solicitud
    ///DESCRIPCIÓN: Breve descripción de lo que hace la función.
    ///PARÁMETROS:
    /// 		1. Clave_Unica: folio de la solicitud
    /// 		2. Fecha_Solucion: fecha de solución probable para la solicitud
    /// 		3. Nombre_Completo: nombre del solicitante
    /// 		4. Email: correo electrónico del solicitante
    /// 		5. Nombre_Tramite: nombre del trámite que se pasa como parámetro
    /// 		6. Cuenta_Predial: cadena de texto con el número de cuenta predial de la solicitud
    /// 		7. Propietario: nombre del propietario de la cuenta predial en la solicitud
    /// 		7. Solicitud_ID: Id de la solicitud
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 11-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Generar_Reporte_Folio_Solicitud(string Clave_Unica, DateTime Fecha_Solucion, string Nombre_Completo, string Email,
            string Nombre_Tramite, string Cuenta_Predial, string Propietario, String Solicitud_ID, String Dependencia, String Area, String Folio)
    {
        Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite Ds_Reporte = new Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite();
        Cls_Cat_Dependencias_Negocio Negocio_Dependencia = new Cls_Cat_Dependencias_Negocio();
        DataTable Dt_Dependencia = new DataTable();
        try
        {
            Negocio_Dependencia.P_Dependencia_ID = Dependencia;
            Dt_Dependencia = Negocio_Dependencia.Consulta_Dependencias();

            DataRow Fila = Ds_Reporte.Tables["Dt_Datos_Solicitud"].NewRow();
            Fila["CLAVE_SOLICITUD"] = Clave_Unica;
            Fila["PORCENTAJE_AVANCE"] = "0";
            Fila["ESTATUS"] = "PENDIENTE";
            //Fila["FECHA_TRAMITE"] = DateTime.Now;
            //Fila["FECHA_ENTREGA"] = "";
            Fila["NOMBRE_COMPLETO"] = Nombre_Completo;
            Fila["CORREO_ELECTRONICO"] = Email;
            Fila["NOMBRE"] = "";
            Fila["NOMBRE_TRAMITE"] = Nombre_Tramite;
            Fila["CUENTA_PREDIAL"] = Cuenta_Predial;
            Fila["PROPIETARIO_CUENTA"] = Propietario;
            Fila["CONSECUTIVO"] = Solicitud_ID;


            //  validacion para evitar error
            if (Dt_Dependencia != null && Dt_Dependencia.Rows.Count > 0)
            {
                Fila["DEPENDENCIA"] = Dt_Dependencia.Rows[0][Cat_Dependencias.Campo_Nombre].ToString();
            }
            else
            {
                Fila["DEPENDENCIA"] = "Prueba";
            }

            Fila["AREA"] = Area;
            Fila["FOLIO"] = Folio;
            Ds_Reporte.Tables["Dt_Datos_Solicitud"].Rows.Add(Fila);

            Generar_Reporte(Ds_Reporte, "PDF", "Folio_Solicitud", "../Rpt/Ventanilla/Rpt_Ven_Folio_Solicitud_Tramite.rpt");

        }
        catch (Exception ex)
        {
            throw new Exception("Generar_Reporte_Folio_Solicitud: " + ex.Message.ToString(), ex);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Formato
    ///DESCRIPCIÓN:     Maneja la Extension del archivo
    ///PROPIEDADES:     String Ruta, direccion que 
    ///                 contiene el nombre del archivo al cual se le sacara la extension
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      03/Mayo/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    private void Generar_Reporte_Formato(String Clave_Unica, DateTime Fecha_Solucion, String Nombre_Completo, String Telefono, String Cuenta_Predial, String Perito_ID, String Solicitud_ID)
    {

        Cls_Ope_Solicitud_Tramites_Negocio Negocio_Actividades_Realizadas = new Cls_Ope_Solicitud_Tramites_Negocio();
        DataTable Dt_Datos_Inmueble = new DataTable();
        DataTable Dt_Datos_Propietario = new DataTable();
        DataTable Dt_Datos_Solictud = new DataTable();
        DataTable Dt_Perito = new DataTable();
        DataTable Dt_Ubicacion_Obra = new DataTable();
        DataRow Fila;
        DataTable Dt_Datos = (DataTable)(Session["Grid_Datos"]);
        String Valor_Dato = "";

        Ds_Rpt_Remodelacion_Ampliacion Ds_Reporte = new Ds_Rpt_Remodelacion_Ampliacion();
        DataSet Ds_Consulta = new DataSet();
        try
        {
            //  previsualizacion de la solicitud
            Dt_Datos_Inmueble = Ds_Reporte.Dt_Datos_Inmueble.Clone();
            Dt_Datos_Propietario = Ds_Reporte.Dt_Datos_Propietario.Clone();
            Dt_Datos_Solictud = Ds_Reporte.Dt_Datos_Solictud.Clone();
            Dt_Perito = Ds_Reporte.Dt_Perito.Clone();
            Dt_Ubicacion_Obra = Ds_Reporte.Dt_Ubicacion_Obra.Clone();


            //  para la ubicacion de la obra
            Negocio_Actividades_Realizadas.P_Cuenta_Predial = Cuenta_Predial;
            Dt_Ubicacion_Obra = Negocio_Actividades_Realizadas.Consultar_Datos_Obra();

            //  para los datos del perito
            Negocio_Actividades_Realizadas.P_Perito_ID = Perito_ID;
            Dt_Perito = Negocio_Actividades_Realizadas.Consultar_Inspectores();

            //  para los datos de la solicutd
            Fila = Dt_Datos_Solictud.NewRow();
            Fila["CLAVE_SOLICITUD"] = Clave_Unica;
            Fila["FECHA_TRAMITE"] = DateTime.Today;
            Fila["FECHA_ENTREGA"] = Convert.ToDateTime(Fecha_Solucion);
            Fila["SOLICITUD_ID"] = Solicitud_ID;
            Dt_Datos_Solictud.Rows.Add(Fila);


            if (Dt_Ubicacion_Obra != null && Dt_Ubicacion_Obra.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Ubicacion_Obra.Rows)
                {
                    //  para los datos del propietario
                    Fila = Dt_Datos_Propietario.NewRow();
                    Fila["NOMBRE"] = Nombre_Completo;
                    Fila["DOMICILIO"] = Registro["Calle"].ToString();
                    Fila["COLONIA"] = Registro["Colonia"].ToString();
                    Fila["TELEFONO"] = Telefono;
                    Dt_Datos_Propietario.Rows.Add(Fila);
                    break;
                }
            }
            //para los datos de la solicitud
            String[,] Datos = new String[Dt_Datos.Rows.Count, 2];
            //  para obtener la informacion de los datos
            for (int Contador_For = 0; Contador_For < Dt_Datos.Rows.Count; Contador_For++)
            {
                Datos[Contador_For, 0] = Dt_Datos.Rows[Contador_For].ItemArray[0].ToString();
                String Nombre_Dato = Grid_Datos.Rows[Contador_For].Cells[0].Text;
                Valor_Dato = ((TextBox)Grid_Datos.Rows[Contador_For].FindControl("Txt_Descripcion_Datos")).Text;
                //  se agregan los campos a la tabla de datos del inmueble
                Fila = Dt_Datos_Inmueble.NewRow();
                Fila["NOMBRE_DATO"] = Nombre_Dato;
                Fila["VALOR"] = Valor_Dato;
                Dt_Datos_Inmueble.Rows.Add(Fila);
            }

            Dt_Ubicacion_Obra.TableName = "Dt_Ubicacion_Obra";
            Dt_Perito.TableName = "Dt_Perito";
            Dt_Datos_Solictud.TableName = "Dt_Datos_Solictud";
            Dt_Datos_Inmueble.TableName = "Dt_Datos_Inmueble";
            Dt_Datos_Propietario.TableName = "Dt_Datos_Propietario";

            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Ubicacion_Obra.Copy());
            Ds_Reporte.Tables.Add(Dt_Perito.Copy());
            Ds_Reporte.Tables.Add(Dt_Datos_Solictud.Copy());
            Ds_Reporte.Tables.Add(Dt_Datos_Inmueble.Copy());
            Ds_Reporte.Tables.Add(Dt_Datos_Propietario.Copy());
            Generar_Reporte_Solicitud_Formato(Ds_Reporte, "PDF", "Formato_Solicitud");

        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Tramite " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Solicitud_Formato
    ///DESCRIPCIÓN: genera el reporte de pdf
    ///PARÁMETROS : 	
    ///         1. Ds_Reporte: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// 		4. Tipo: Parámetro para ventana emergente
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 02-Julio-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    public void Generar_Reporte_Solicitud_Formato(DataSet Ds_Reporte, String Extension_Archivo, String Tipo)
    {
        String Nombre_Archivo = "Reporte_Formato_" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
        String Ruta_Archivo = @Server.MapPath("../Rpt/Ordenamiento_Territorial/");//Obtiene la ruta en la cual será guardada el archivo
        ReportDocument Reporte = new ReportDocument();

        try
        {
            Reporte.Load(Ruta_Archivo + "Rpt_Ort_Remodelacion_Ampliacion.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();
            //  para el tipo de archivo
            if (Extension_Archivo == "PDF")
                Nombre_Archivo += ".pdf";
            else if (Extension_Archivo == "EXCEL")
                Nombre_Archivo += ".xls";

            Ruta_Archivo = @Server.MapPath("../../Reporte/");
            m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

            ExportOptions Opciones_Exportacion = new ExportOptions();
            Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
            Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;

            //  para el tipo de archivo
            if (Extension_Archivo == "PDF")
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
            else if (Extension_Archivo == "EXCEL")
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;

            Reporte.Export(Opciones_Exportacion);

            if (Extension_Archivo == "PDF")
                Abrir_Ventana(Nombre_Archivo, Tipo);
            else if (Extension_Archivo == "EXCEL")
            {
                String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', '" + Tipo + "','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: genera el reporte de pdf
    ///PARÁMETROS : 	
    ///         1. Ds_Reporte: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 02-Julio-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    public void Generar_Reporte(DataSet Ds_Reporte, String Extension_Archivo, String Tipo, string Ruta_Archivo_Rpt)
    {
        String Nombre_Archivo = "Reporte_Seguimiento_Solicitud_" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
        String Ruta_Archivo = @Server.MapPath(Ruta_Archivo_Rpt);//Obtiene la ruta en la cual será guardada el archivo
        ReportDocument Reporte = new ReportDocument();

        try
        {
            Reporte.Load(Ruta_Archivo);
            Reporte.SetDataSource(Ds_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();
            //  para el tipo de archivo
            if (Extension_Archivo == "PDF")
                Nombre_Archivo += ".pdf";
            else if (Extension_Archivo == "EXCEL")
                Nombre_Archivo += ".xls";

            Ruta_Archivo = @Server.MapPath("../../Reporte/");
            m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

            ExportOptions Opciones_Exportacion = new ExportOptions();
            Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
            Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;

            //  para el tipo de archivo
            if (Extension_Archivo == "PDF")
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
            else if (Extension_Archivo == "EXCEL")
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;

            Reporte.Export(Opciones_Exportacion);

            if (Extension_Archivo == "PDF")
                Abrir_Ventana(Nombre_Archivo, Tipo);
            else if (Extension_Archivo == "EXCEL")
            {
                String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', '" + Tipo + "','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
    ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
    ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
    ///                             para mostrar los datos al usuario
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 02-Julio-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Abrir_Ventana(String Nombre_Archivo, String Tipo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
        try
        {
            Pagina = Pagina + Nombre_Archivo;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Tipo,
            "window.open('" + Pagina + "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Cuenta_Predial_ID
    /// DESCRIPCIÓN: Regresar el id de la cuenta predial, se busca mediante la cuenta predial 
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial: cuenta predial a localizar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 10-jun-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Consultar_Cuenta_Predial_ID(String Cuenta_Predial)
    {
        var Consulta_Cuenta = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        String Cuenta_Predial_ID = "";
        DataTable Dt_Resultado_Consulta;

        try
        {
            if (!String.IsNullOrEmpty(Cuenta_Predial))
            {
                // consultar cuenta predial
                Consulta_Cuenta.P_Cuenta_Predial = Cuenta_Predial;
                Dt_Resultado_Consulta = Consulta_Cuenta.Consultar_Cuenta_Predial_ID();
                if (Dt_Resultado_Consulta != null && Dt_Resultado_Consulta.Rows.Count > 0)
                {
                    Cuenta_Predial_ID = Dt_Resultado_Consulta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Cuenta_Predial_ID: " + ex.Message.ToString(), ex);
        }
        return Cuenta_Predial_ID;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Propietario
    /// DESCRIPCIÓN: Regresar el nombre del propietario de la cuenta predial con el id proporcionado
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: id de la cuenta predial a consultar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 10-jun-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Consultar_Propietario(String Cuenta_Predial_ID)
    {
        String Propietario = "";
        DataTable Dt_Resultado_Consulta;
        var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();

        try
        {
            if (!String.IsNullOrEmpty(Cuenta_Predial_ID))
            {
                // consultar cuenta predial
                Consulta_Propietario_Negocio.P_Cuenta_Predial_Id = Cuenta_Predial_ID;
                Dt_Resultado_Consulta = Consulta_Propietario_Negocio.Consultar_Propietario();
                if (Dt_Resultado_Consulta != null && Dt_Resultado_Consulta.Rows.Count > 0)
                {
                    Propietario = Dt_Resultado_Consulta.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message;
        }
        return Propietario;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: LLenar_Combos
    ///DESCRIPCIÓN: Consulta los datos para los combos y los asigna al combo correspondiente
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 10-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void LLenar_Combos()
    {
        var Obj_Peritos = new Cls_Cat_Ort_Inspectores_Negocio();
        DataTable Dt_Peritos;

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            // consultar peritos
            Dt_Peritos = Obj_Peritos.Consultar_Inspectores();
            // cargar datos en el combo
            Cmb_Perito.Items.Clear();
            Cmb_Perito.DataSource = Dt_Peritos;
            Cmb_Perito.DataTextField = Cat_Ort_Inspectores.Campo_Nombre;
            Cmb_Perito.DataValueField = Cat_Ort_Inspectores.Campo_Inspector_ID;
            Cmb_Perito.DataBind();
            Cmb_Perito.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), ""));
            Cmb_Perito.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: LLenar_Combos_Catastro
    ///DESCRIPCIÓN: Consulta los datos para los combos y los asigna al combo correspondiente
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 10-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void LLenar_Combos_Catastro()
    {
        var Obj_Peritos = new Cls_Cat_Cat_Peritos_Externos_Negocio();
        DataTable Dt_Peritos;

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            // consultar peritos
            Obj_Peritos.P_Estatus = "='VIGENTE";
            Dt_Peritos = Obj_Peritos.Consultar_Peritos_Externos();
            // cargar datos en el combo
            Cmb_Perito.Items.Clear();
            Cmb_Perito.DataSource = Dt_Peritos;
            Cmb_Perito.DataTextField = "PERITO_EXTERNO";
            Cmb_Perito.DataValueField = Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id;
            Cmb_Perito.DataBind();
            Cmb_Perito.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), ""));
            Cmb_Perito.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }

    #endregion Metodos Generales

    #region(Validacion)

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.

        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir la cuenta Predial o : <br />";
        Lbl_Mensaje_Error.Visible = true;
        Img_Error.Visible = true;


        //Cmb_Tramite.SelectedIndex != 0 && Txt_Nombre.Text.Length > 1 &&
        //            Txt_Apellido_Paterno.Text.Length > 1)

        if (Pnl_Cuenta_Predial.Visible == true)
        {
            if (String.IsNullOrEmpty(Txt_Cuenta_Predial.Text))
            {
                if (String.IsNullOrEmpty(Txt_Propietario_Cuenta_Predial.Text))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el Propietario del predio.<br />";
                    Datos_Validos = false;
                }
                if (String.IsNullOrEmpty(Txt_Direccion_Predio.Text))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la Colonia del predio.<br />";
                    Datos_Validos = false;
                }
                if (String.IsNullOrEmpty(Txt_Calle_Predio.Text))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la Calle del predio.<br />";
                    Datos_Validos = false;
                }
                if (String.IsNullOrEmpty(Txt_Numero_Predio.Text))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el Nuemero del predio.<br />";
                    Datos_Validos = false;
                }
                if (String.IsNullOrEmpty(Txt_Manzana_Predio.Text))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la Manzana del predio.<br />";
                    Datos_Validos = false;
                }
                if (String.IsNullOrEmpty(Txt_Lote_Predio.Text))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el Lote del predio.<br />";
                    Datos_Validos = false;
                }

            }
        }
        if ((Txt_Otros_Predio.Text != "") && (Txt_Otros_Predio.Text.Length > 1000))
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Longitud superiror en la casilla de OTROS.<br />";
            Datos_Validos = false;
        }
        if (Cmb_Tramite.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el tramite que desea realizar.<br />";
            Datos_Validos = false;
        }

        if (Txt_Nombre.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el nombre de quién solicita..<br />";
            Datos_Validos = false;
        }

        if (Txt_Apellido_Paterno.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el apellido paterno de quién solicita..<br />";
            Datos_Validos = false;
        }

        //if (Txt_Apellido_Materno.Text == "")
        //{
        //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el apellido materno de quién solicita..<br />";
        //    Datos_Validos = false;
        //}
        if (Txt_Email.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el email de quién solicita..<br />";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Grid_Datos
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Grid_Datos()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        DataTable Dt_Documentos = (DataTable)(Session["Grid_Documentos"]);
        DataTable Dt_Datos = (DataTable)(Session["Grid_Datos"]);
        try
        {

            Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br />";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;


            //  saca las dimenciones del arreglo
            String[,] Datos = new String[Dt_Datos.Rows.Count, 2];
            String[,] Documentos = new String[Dt_Documentos.Rows.Count, 2];

            //  para saber si cuenta con informacion 
            for (int Contador_For = 0; Contador_For < Dt_Datos.Rows.Count; Contador_For++)
            {
                String Valor_Dato = ((TextBox)Grid_Datos.Rows[Contador_For].FindControl("Txt_Descripcion_Datos")).Text;

                if (Valor_Dato != "" || (Dt_Datos.Rows[Contador_For][Cat_Tra_Datos_Tramite.Campo_Dato_Requerido].ToString()) == "N")
                {
                }

                else
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el dato de " +
                            Dt_Datos.Rows[Contador_For][Cat_Tra_Datos_Tramite.Campo_Nombre] + ".<br />";
                    Datos_Validos = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }

        return Datos_Validos;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Grid_Documentos
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Grid_Documentos()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        DataTable Dt_Documentos = (DataTable)(Session["Grid_Documentos"]);
        DataTable Dt_Datos = (DataTable)(Session["Grid_Datos"]);
        String[,] Datos = new String[Dt_Datos.Rows.Count, 2];
        String[,] Documentos = new String[Dt_Documentos.Rows.Count, 2];
        try
        {

            Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br />";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;

            ////  para saber si cuenta con informacion 
            for (int Contador_For = 0; Contador_For < Dt_Documentos.Rows.Count; Contador_For++)
            {
                AsyncFileUpload AsFileUp = (AsyncFileUpload)Grid_Documentos.Rows[Contador_For].Cells[1].FindControl("FileUp");
                TextBox Txt_Url = (TextBox)Grid_Documentos.Rows[Contador_For].Cells[1].FindControl("Txt_Url");
                FileUpload File_Acutalizar = (FileUpload)Grid_Documentos.Rows[Contador_For].Cells[1].FindControl("FileUp_Acutalizacion");

                String Extension = Obtener_Extension(AsFileUp.FileName);


                if (Dt_Documentos.Rows[Contador_For][Tra_Detalle_Documentos.Campo_Documento_Requerido].ToString() == "S")
                {
                    if (Extension == "pdf" || Extension == "jpg" || Extension == "jpeg" || Extension == "rar" || Extension == "zip" || Extension == "dwg")
                    {

                    }
                    else
                    {
                        // para los documentos que se encuentra dentro del portafolio
                        if (Txt_Url.Text != "")
                        {

                        }
                        else
                        {
                            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el dato de " + Dt_Documentos.Rows[Contador_For]["DOCUMENTO"] + " ya que es un documento requerido.<br />";
                            Datos_Validos = false;
                        }
                    }
                }
                else
                {
                    if (Txt_Url.Text != "")
                    {

                    }
                    else if (Extension == "pdf" || Extension == "jpg" || Extension == "jpeg" || Extension == "rar" || Extension == "zip" || Extension == "dwg")
                    {

                    }
                    else if (Txt_Url.Text == "" && Extension == "")
                    {

                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El formato del documento" + Dt_Documentos.Rows[Contador_For]["DOCUMENTO"] + " no es valido.<br />";
                        Datos_Validos = false;
                    }
                }
            }


        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }

        return Datos_Validos;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Botones_Grid
    ///DESCRIPCIÓN: mostrara los botones del grid
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  30/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Mostrar_Botones_Grid(Boolean Estado)
    {
        try
        {
        }
        catch (Exception ex)
        {
            throw new Exception("Mostrar_Botones_Grid " + ex.Message.ToString(), ex);
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


    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  02/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Operacion = false;
        try
        {
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Validar_Datos())
            {
                Operacion = Alta_Tramite();

                if (Operacion == true)
                {
                    Limpia_Controles();
                    Div_Grid_Datos_Tramite.Style.Value = "display:none";
                    Div_Grid_Documentos.Style.Value = "display:none";
                    Btn_Generar_Reporte.Visible = false;
                }

            }
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Nuevo_Click " + ex.Message, ex);
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Click
    ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Julio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DateTime Fecha_Solucion;
            var Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
            Fecha_Solucion = Dias_Inhabilies.Calcular_Fecha("" + DateTime.Today, Txt_Tiempo_Estimado.Text);

            String Nombre = Txt_Nombre.Text;
            String Email = Txt_Email.Text;

            Generar_Reporte_Solicitud("", Fecha_Solucion, Nombre, Email);
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Generar_Reporte_Click " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  02/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Salir")
            Response.Redirect("../Ventanilla/Frm_Ope_Ven_Lista_Tramites.aspx");

        else
            Response.Redirect("../Ventanilla/Frm_Ope_Ven_Lista_Tramites.aspx");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Perito_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID del Perito seleccionado en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 10-jun-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Perito_Click(object sender, ImageClickEventArgs e)
    {
        // limpiar mensaje de error
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_PERITOS"] != null)
        {
            // si el valor de la sesión es igual a true y el valor de la sesiones PERITO_ID no es nulo ni vacío
            if (Convert.ToBoolean(Session["BUSQUEDA_PERITOS"]) == true && Session["PERITO_ID"] != null && Session["PERITO_ID"].ToString().Length > 0)
            {
                try
                {
                    string Perito_ID = Session["PERITO_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Perito.Items.FindByValue(Perito_ID) != null)
                    {
                        Cmb_Perito.SelectedValue = Perito_ID;
                    }
                    else if (Session["NOMBRE_PERITO"] != null && Session["NOMBRE_PERITO"].ToString().Length > 0)
                    {
                        Cmb_Perito.Items.Add(new ListItem(HttpUtility.HtmlDecode(Session["NOMBRE_PERITO"].ToString()), Perito_ID));
                        Cmb_Perito.SelectedValue = Perito_ID;
                    }
                }
                catch (Exception Ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Ex.Message;
                }

                // limpiar variables de sesión
                Session.Remove("PERITO_ID");
                Session.Remove("NOMBRE_PERITO");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_PERITOS");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Link_Catastro_Click
    ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  13/Agosto/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Link_Catastro_Click(object sender, EventArgs e)
    {
        String Solicitud_ID = "";
        try
        {
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Validar_Datos())
            {
                Solicitud_ID = Alta_Tramite_Catastro();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }


        if (Solicitud_ID != "")
        {
            Session["Tramite_Id"] = Solicitud_ID;
            Session["Postback_grid"] = null;
            //Cls_Sessiones.No_Empleado = Solicitud_ID;
            FormsAuthentication.Initialize();
            String Consulta = "SELECT " + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Txt_Cuenta_Predial.Text.Trim() + "'";
            if (Cmb_Tramite.SelectedItem.Text == "Solicitud de registro")
            {
                Response.Redirect("../Catastro/Frm_Ope_Cat_Recepcion_Documentos_Perito_externo.aspx");
            }
            else if (Cmb_Tramite.SelectedItem.Text == "Solicitud de refrendo")
            {
                Response.Redirect("../Catastro/Frm_Ope_Cat_Solicitud_Refrendo.aspx");
            }
            else if (Cmb_Tramite.SelectedItem.Text.ToUpper().Trim() == "AVALUO")
            {
                Response.Redirect("../Catastro/Frm_Ope_Cat_Avaluo_Urbano_Perito.aspx");
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Link_Catastro2_Click
    ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Miguel Angel Bedolla Moreno
    ///FECHA_CREO:  13/Agosto/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Link_Catastro2_Click(object sender, EventArgs e)
    {
        String Solicitud_ID = "";
        try
        {
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Validar_Datos())
            {
                Solicitud_ID = Alta_Tramite_Catastro();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }


        if (Solicitud_ID != "")
        {
            Session["Tramite_Id"] = Solicitud_ID;
            Session["Postback_grid"] = null;
            //Cls_Sessiones.No_Empleado = Solicitud_ID;
            FormsAuthentication.Initialize();
            String Consulta = "SELECT " + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Txt_Cuenta_Predial.Text.Trim() + "'";
            Response.Redirect("../Catastro/Frm_Ope_Cat_Avaluo_Rustico_Perito.aspx");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Regrasar_OnClick
    ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  02/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Regrasar_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("../Ventanilla/Frm_Apl_Ventanilla.aspx");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Actualizar_Documento_Click
    ///DESCRIPCIÓN: permitira actualizar el archivo
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  24/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Actualizar_Documento_Click(object sender, ImageClickEventArgs e)
    {
        int Fila = 0;
        TableCell Celda = new TableCell();
        GridViewRow Renglon;
        ImageButton Boton = new ImageButton();
        try
        {
            Boton = (ImageButton)sender;
            Celda = (TableCell)Boton.Parent;
            Renglon = (GridViewRow)Celda.Parent;
            Grid_Documentos.SelectedIndex = Renglon.RowIndex;
            Fila = Renglon.RowIndex;

            ImageButton Btn_Acutalizar_Documento = (ImageButton)Grid_Documentos.Rows[Fila].Cells[1].FindControl("Btn_Acutalizar_Documento");
            ImageButton Btn_Ver_Documento = (ImageButton)Grid_Documentos.Rows[Fila].Cells[1].FindControl("Btn_Ver_Documento");
            AsyncFileUpload Afu_Subir_Archivo = (AsyncFileUpload)Grid_Documentos.Rows[Fila].Cells[1].FindControl("FileUp");
            TextBox Txt_Url = (TextBox)Grid_Documentos.Rows[Fila].Cells[1].FindControl("Txt_Url");

            //  se limpia la ruta del archivo
            Txt_Url.Text = "";
            //Txt_Url.Visible = true;
            //  se ocultan los botones de ver y actualizar
            Btn_Ver_Documento.Visible = false;
            Btn_Acutalizar_Documento.Visible = false;
            //  se muestra el boton de subir archivo
            Afu_Subir_Archivo.Visible = true;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Cuenta_Predial_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            //if (Txt_Cuenta_Predial.Text != "")
            //{
            //    Cargar_Ventana_Emergente_Resumen_Predio();
            //}
            //else
            //{ 

            //}
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Buscar_Cuenta_Predial_Click " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Documento_Click
    ///DESCRIPCIÓN: mostrara el documento
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  24/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Ver_Documento_Click(object sender, ImageClickEventArgs e)
    {
        String URL = String.Empty;
        int Fila = 0;
        TableCell Celda = new TableCell();
        GridViewRow Renglon;
        ImageButton Boton = new ImageButton();
        String Nombre_Archivo = "";
        String Nombre_Documento = "";
        String Directorio_Portafolio = "";
        String Raiz = "";
        try
        {
            //  para obtener el id del documento 
            Boton = (ImageButton)sender;
            Celda = (TableCell)Boton.Parent;
            Renglon = (GridViewRow)Celda.Parent;
            Grid_Documentos.SelectedIndex = Renglon.RowIndex;
            Fila = Renglon.RowIndex;

            //  se obtiene el nombre del documento y el id del ciudadano
            Nombre_Documento = Grid_Documentos.Rows[Fila].Cells[0].Text.Trim();
            Directorio_Portafolio = Cls_Sessiones.Ciudadano_ID;
            Raiz = @Server.MapPath("../../Portafolio");

            //   se revisa que el directorio exista
            if (!Directory.Exists(Raiz))
            {
                Directory.CreateDirectory(Raiz);
            }

            if (!Directory.Exists(Raiz + "/" + Directorio_Portafolio))
            {
                Directory.CreateDirectory(Raiz + "/" + Directorio_Portafolio);
            }

            //  se obtiene el nombre de los archivos existentes en la carpeta
            String[] Archivos = Directory.GetFiles(MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));

            //  se busca el archivo
            for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
            {
                Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                if (Nombre_Archivo.Contains(Nombre_Documento))
                {
                    URL = Archivos[Contador].Trim();
                    break;
                }

            }// fin del for

            if (URL != null)
            {
                Mostrar_Archivo(URL);
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }

    #endregion

    #region TextBox

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Cuenta_Predial_OnTextChanged
    ///DESCRIPCIÓN: habilitara el boton de busqueda de cuenta predial
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  09/Julio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Cuenta_Predial_OnTextChanged(object sender, EventArgs e)
    {
        // limpiar mensaje de error
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            // limpiar y ocultar el campo propietario y el botón resumen predio
            Btn_Buscar_Cuenta_Predial.Visible = false;
            Txt_Propietario_Cuenta_Predial.Text = "";
            Txt_Direccion_Predio.Text = "";
            Txt_Calle_Predio.Text = "";
            Txt_Numero_Predio.Text = "";
            Txt_Lote_Predio.Text = "";
            Txt_Manzana_Predio.Text = "";
            Txt_Otros_Predio.Text = "";

            // si el campo cuenta predial contiene texto, buscar el id de la cuenta
            if (Txt_Cuenta_Predial.Text.Length > 0)
            {
                // validar que la se haya obtenido una valor para la cuenta
                if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text))
                {
                    Cls_Ope_Solicitud_Tramites_Negocio Negocio = new Cls_Ope_Solicitud_Tramites_Negocio();
                    Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.ToUpper();
                    DataTable Dt_Predio = Negocio.Consultar_Cuenta_Predial();

                    if (Dt_Predio.Rows.Count > 0)
                    {
                        Txt_Propietario_Cuenta_Predial.Text = String.IsNullOrEmpty(Dt_Predio.Rows[0]["PROPIETARIO"].ToString()) ? ""
                            : Dt_Predio.Rows[0]["PROPIETARIO"].ToString();
                        Txt_Direccion_Predio.Text = String.IsNullOrEmpty(Dt_Predio.Rows[0]["COLONIA"].ToString()) ? ""
                            : Dt_Predio.Rows[0]["COLONIA"].ToString();
                        Txt_Calle_Predio.Text = String.IsNullOrEmpty(Dt_Predio.Rows[0]["CALLE"].ToString()) ? ""
                            : Dt_Predio.Rows[0]["CALLE"].ToString();

                        Txt_Numero_Predio.Text = String.IsNullOrEmpty(Dt_Predio.Rows[0]["NO_EXTERIOR"].ToString()) ? ""
                            : Dt_Predio.Rows[0]["NO_EXTERIOR"].ToString();
                        Txt_Manzana_Predio.Text = String.IsNullOrEmpty(Dt_Predio.Rows[0]["MANZANA"].ToString()) ? ""
                            : Dt_Predio.Rows[0]["MANZANA"].ToString();
                        Txt_Lote_Predio.Text = String.IsNullOrEmpty(Dt_Predio.Rows[0]["LOTE"].ToString()) ? ""
                            : Dt_Predio.Rows[0]["LOTE"].ToString();
                    }
                }
                else
                {
                    // mostrar mensaje indicando que no se encontró la cuenta
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "La Cuenta Predial proporcionada no se encuentra en el sistema.<br /><br />";
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message;
        }
    }

    #endregion

    #region Grid



    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Datos_RowDataBound
    ///DESCRIPCIÓN          : Cargara el toltip de las cajas de texto
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO          : 31/Agosto/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Datos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox Txt_Descripcion_Datos = (TextBox)e.Row.Cells[1].FindControl("Txt_Descripcion_Datos");
                Txt_Descripcion_Datos.ToolTip = HttpUtility.HtmlDecode(e.Row.Cells[2].Text.ToString());
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Documentos_RowDataBound
    ///DESCRIPCIÓN          :cargara los botones dentro del grid
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO          : 24/Mayo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Documentos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        String Nombre_Archivo = "";
        String Nombre_Documento = "";
        String Directorio_Portafolio = "";
        Boolean Encontrado = false;
        String Raiz = "";
        Color Color_Requerido = Color.LightBlue;

        try
        {
            Directorio_Portafolio = Cls_Sessiones.Ciudadano_ID;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AsyncFileUpload Afu_Subir_Archivo = (AsyncFileUpload)e.Row.Cells[1].FindControl("FileUp");
                TextBox Txt_Url = (TextBox)e.Row.Cells[1].FindControl("Txt_Url");
                ImageButton Btn_Acutalizar_Documento = (ImageButton)e.Row.Cells[1].FindControl("Btn_Acutalizar_Documento");
                ImageButton Btn_Ver_Documento = (ImageButton)e.Row.Cells[1].FindControl("Btn_Ver_Documento");

                e.Row.Cells[3].Visible = true;
                String Requerido = e.Row.Cells[3].Text;
                e.Row.Cells[3].Visible = false;

                Directorio_Portafolio = Cls_Sessiones.Ciudadano_ID;
                Raiz = @Server.MapPath("../../Portafolio");

                //  se agrega al ToolTip
                String Documento = e.Row.Cells[0].Text;
                Afu_Subir_Archivo.ToolTip = HttpUtility.HtmlDecode(e.Row.Cells[2].Text.ToString());
                Btn_Acutalizar_Documento.ToolTip = HttpUtility.HtmlDecode(e.Row.Cells[2].Text.ToString());

                if (!Directory.Exists(Raiz))
                {
                    Directory.CreateDirectory(Raiz);
                }

                if (!Directory.Exists(Raiz + "/" + Directorio_Portafolio))
                {
                    Directory.CreateDirectory(Raiz + "/" + Directorio_Portafolio);
                }

                String[] Archivos = Directory.GetFiles(MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));
                Nombre_Documento = e.Row.Cells[0].Text;

                for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                {
                    Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                    if (Nombre_Archivo.Contains(Nombre_Documento))
                    {
                        //  se carga la ruta del archivo
                        Txt_Url.Text = Archivos[Contador].Trim();

                        Btn_Acutalizar_Documento.Visible = true;
                        Btn_Ver_Documento.Visible = true;
                        Afu_Subir_Archivo.Visible = false;
                        Txt_Url.Visible = false;
                        Encontrado = true;
                        break;
                    }

                }// fin del for

                if (Encontrado == false)
                {
                    Afu_Subir_Archivo.Visible = true;
                    Txt_Url.Visible = false;
                    Btn_Acutalizar_Documento.Visible = false;
                    Btn_Ver_Documento.Visible = false;
                    Txt_Url.Visible = false;
                }
                else
                {
                    Mostrar_Botones_Grid(true);

                }

                if (Requerido == "N")
                {
                    e.Row.Cells[1].BackColor = Color_Requerido;
                }

            }

        }// fin del try
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    #endregion
}
