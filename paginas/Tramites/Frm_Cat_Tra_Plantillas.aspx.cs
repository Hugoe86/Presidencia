using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Plantillas.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;


public partial class paginas_Tramites_Frm_Cat_Tra_Plantillas_Subprocesos : System.Web.UI.Page
{

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Estado_Botones("inicial");
                Llenar_Grid();
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error(true, "" + Ex.ToString());
        }
       
    }


    #endregion

    #region Metodos


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones
    ///DESCRIPCIÓN: metodo que muestra los botones de acuerdo al estado en el que se encuentre
    ///PARAMETROS:   1.- String Estado: El estado de los botones solo puede tomar 
    ///                 + inicial
    ///                 + nuevo
    ///                 + modificar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estado_Botones(String Estado)
    {
        try
        {
            switch (Estado)
            {
                case "inicial":
                    //Boton Nuevo
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Nuevo.Enabled = true;
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    //Boton Modificar
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Modificar.Enabled = false;
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    //Boton Eliminar
                    Btn_Eliminar.Enabled = false;
                    Btn_Eliminar.Visible = true;
                    //Boton Salir
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Salir.Enabled = true;
                    Btn_Salir.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Subir_Archivo.Enabled = false;
                    //Cajas de texto
                    Txt_Plantilla_ID.Text = "";
                    Txt_Plantilla_ID.Enabled = false;
                    Txt_Nombre_Archivo.Text = "";
                    Txt_Nombre_Archivo.Enabled = false;
                    Txt_Nombre.Text = "";
                    Txt_Nombre.Enabled = false;
                    //Txt_Busqueda.Text = "";
                    break;
                case "nuevo":
                    //Boton Nuevo
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Nuevo.Enabled = true;
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    //Boton Modificar
                    Btn_Modificar.Visible = false;
                    //Boton Eliminar
                    Btn_Eliminar.Visible = false;
                    //Boton Salir
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.Enabled = true;
                    Btn_Salir.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Subir_Archivo.Enabled = true;
                    //Cajas de Texto
                    Txt_Plantilla_ID.Text = "";
                    Txt_Plantilla_ID.Enabled = false;
                    Txt_Nombre.Text = "";
                    Txt_Nombre.Enabled = true;
                    Txt_Nombre_Archivo.Enabled = false;
                    Txt_Nombre_Archivo.Text = "";
                    //Txt_Busqueda.Text = "";
                    break;
                case "modificar":
                    //Boton Nuevo
                    Btn_Nuevo.Visible = false;
                    //Boton Modificar
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Modificar.Enabled = true;
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    //Boton Eliminar
                    Btn_Eliminar.Visible = false;
                    //Boton Salir
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.Enabled = true;
                    Btn_Salir.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Subir_Archivo.Enabled = true;
                    //Cajas de Texto
                    Txt_Plantilla_ID.Enabled = false;
                    Txt_Nombre_Archivo.Enabled = false;
                    Txt_Nombre.Enabled = true;

                    //Txt_Busqueda.Text = "";
                    break;

            }//fin del switch
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Cajas
    ///DESCRIPCIÓN: metodo que muestra los botones de acuerdo al estado en el que se encuentre
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Validar_Cajas()
    {
        if (Txt_Nombre.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ El nombre de la plantilla es obligatorio <br />";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Subir_Archivo
    ///DESCRIPCIÓN: metodo que sube un archivo a la carpeta de Plantillas_Word
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Subir_Archivo()
    {
        try
        {
            //Se sube el archivo a la carpeta
            String Ruta = Server.MapPath("../../Plantillas_Word");
            if (Btn_Subir_Archivo.HasFile)
            {
                if (!Directory.Exists(Ruta))
                {
                    Directory.CreateDirectory(Ruta);
                }
                String Direccion_Archivo = Ruta + "/" + Txt_Nombre_Archivo.Text;
                if (File.Exists(Direccion_Archivo))
                {
                    File.Delete(Direccion_Archivo);
                }
                Btn_Subir_Archivo.SaveAs(Direccion_Archivo);

            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Negocio
    ///DESCRIPCIÓN: metodo que carga los datos de la capa de negocio
    ///PARAMETROS:  1.- bool Estado: en caso de ser true carga los datos 
    ///                              en caso de ser false les asigna valor null
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public Cls_Cat_Tra_Plantillas_Negocio Cargar_Datos_Negocio(bool Estado, Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio)
    {
        try
        {
            if (Estado == true)
            {
                Plantilla_Negocio.P_Plantilla_ID = Txt_Plantilla_ID.Text;
                Plantilla_Negocio.P_Nombre = Txt_Nombre.Text;
                Plantilla_Negocio.P_Nombre_Archivo = Txt_Nombre_Archivo.Text;
                Plantilla_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            }
            else
            {
                Plantilla_Negocio.P_Plantilla_ID = null;
                Plantilla_Negocio.P_Nombre = null;
                Plantilla_Negocio.P_Nombre_Archivo = null;

            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        return Plantilla_Negocio;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Extension
    ///DESCRIPCIÓN: Maneja la extencion del archivo
    ///PROPIEDADES: String Ruta, direccion que 
    ///contiene el nombre del archivo al cual se le sacara la extension
    ///CREO: Francisco Gallardo
    ///FECHA_CREO: 16/Marzo/2010
    ///MODIFICO: Silvia Morales
    ///FECHA_MODIFICO: 19/Octubre/2010
    ///CAUSA_MODIFICACIÓN: Se adecuo al estandar
    ///*******************************************************************************
    private string Obtener_Extension(String Ruta)
    {
        String Extension = "";
        try
        {

            int index = Ruta.LastIndexOf(".");
            if (index < Ruta.Length)
            {
                Extension = Ruta.Substring(index + 1);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        return Extension;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Eliminar_Archivo
    ///DESCRIPCIÓN: metodo que elimina el archivo de la carpeta de Plantillas_Word
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Eliminar_Archivo()
    {
        try
        {
            String Ruta = Server.MapPath("../../Plantillas_Word");
            String Direccion_Archivo = Ruta + "/" + Txt_Nombre_Archivo.Text;
            if (File.Exists(Direccion_Archivo))
            {
                File.Delete(Direccion_Archivo);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Archivo_Word
    ///DESCRIPCIÓN  : Muestra un Archivo del cual se le pasa la ruta como parametro.
    ///PARAMETROS   :
    ///             1.  Ruta.  Ruta del Archivo.
    ///CREO         : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO   : 12/Noviembre/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************  
    public void Mostrar_Archivo_Word(String Ruta)
    {
        try
        {
            if (System.IO.File.Exists(Server.MapPath(Ruta)))
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Ruta + "','Window_Archivo','left=0,top=0')", true);

            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('El archivo no existe o fue eliminado');", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "');", true);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Mensaje_Error
    ///DESCRIPCIÓN:          habilitara los mensajes de error
    ///PARAMETROS:           1.  Habilitar.  el tipo de visibilidad del objeto
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           12/Noviembre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Mostrar_Mensaje_Error(Boolean Habilitar, String Texto_Error)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = Habilitar;
            IBtn_Imagen_Error.Visible = Habilitar;
            Lbl_Mensaje_Error.Visible = Habilitar;
            Lbl_Mensaje_Error.Text = Texto_Error;
        }
        catch (Exception Ex)
        {
            IBtn_Imagen_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Subir_Archivo
    ///DESCRIPCIÓN: metodo que valida al momento de subir un archivo
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Validar_Subir_Archivo()
    {
        if ((!Btn_Subir_Archivo.HasFile) && (Txt_Nombre_Archivo.Text.Trim() == String.Empty))
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ El archivo es obligatorio de clic en examinar para seleccionar el archivo<br />";
        }

        String Raiz = @Server.MapPath("../../Plantillas_Word/" + Txt_Nombre_Archivo.Text);
        try
        {
            if (Btn_Subir_Archivo.HasFile)
            {
                if (System.IO.File.Exists(Raiz))
                {
                    System.IO.File.Delete(Raiz);
                }

                Txt_Nombre_Archivo.Text = Txt_Plantilla_ID.Text + "_" + Txt_Nombre.Text + "." +
                                        Obtener_Extension(Btn_Subir_Archivo.FileName);
                Subir_Archivo();
            }
        }
        catch (Exception Ex)
        {
        }
    }


    #endregion

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid
    ///DESCRIPCIÓN: Metodo que llena el GridView de Plantillas
    ///PARAMETROS: GridView que se llenara
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************            
    public void Llenar_Grid()
    {
        Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio = new Cls_Cat_Tra_Plantillas_Negocio();        
        DataSet Data_Set = new DataSet();
        try
        {
            if (Txt_Busqueda.Text != "") 
                Plantilla_Negocio.P_Campo_Buscar = Txt_Busqueda.Text.Trim().ToUpper();


            Data_Set = Plantilla_Negocio.Consulta_Plantillas();

            if (Data_Set.Tables[0].Rows.Count > 0)
            {
                //  se ordena por nombre
                DataTable Dt_Ordenar_Campos = Data_Set.Tables[0];
                DataView Dv_Ordenar = new DataView(Dt_Ordenar_Campos);
                Dv_Ordenar.Sort = Cat_Tra_Plantillas.Campo_Nombre + "," + Cat_Tra_Plantillas.Campo_Archivo;
                Dt_Ordenar_Campos = Dv_Ordenar.ToTable();

                Grid_Plantillas.Columns[1].Visible = true;
                Grid_Plantillas.DataSource = Dt_Ordenar_Campos;
                Grid_Plantillas.DataBind();
                Grid_Plantillas.Columns[1].Visible = false;

                Grid_Plantillas.SelectedIndex = -1;
            }
            else
            {
                Grid_Plantillas.Columns[1].Visible = true;
                Grid_Plantillas.DataSource = new DataTable();
                Grid_Plantillas.DataBind();
                Grid_Plantillas.Columns[1].Visible = false;
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Plantillas_PageIndexChanging
    ///DESCRIPCIÓN: Metodo para manejar la paginacion del Grid_Colonias
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Grid_Plantillas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Plantillas.PageIndex = e.NewPageIndex;
            Grid_Plantillas.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Plantillas_SelectedIndexChanged
    ///DESCRIPCIÓN: metodo que carga los datos seleccionados en el grid a las cajas de texto. 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Plantillas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio = new Cls_Cat_Tra_Plantillas_Negocio();
        String Plantilla_Seleccionada = "";
        String Plantilla_Nombre = "";
        String Plantilla_Archivo = "";
        GridViewRow Renglon;
        try
        {
            Mostrar_Mensaje_Error(false, "");
            Estado_Botones("inicial");
            Btn_Modificar.Enabled = true;
            Btn_Eliminar.Enabled = true;

            Renglon = Grid_Plantillas.Rows[Grid_Plantillas.SelectedIndex];
            Plantilla_Seleccionada = Convert.ToString(Renglon.Cells[1].Text);
            Plantilla_Nombre = Convert.ToString(Renglon.Cells[2].Text);
            Plantilla_Archivo = Convert.ToString(Renglon.Cells[3].Text);
            Txt_Plantilla_ID.Text = Plantilla_Seleccionada;
            Txt_Nombre.Text = Plantilla_Nombre;
            Txt_Nombre_Archivo.Text = Plantilla_Archivo;

            Plantilla_Negocio = Cargar_Datos_Negocio(false, Plantilla_Negocio);
            //Llenar_Grid();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Evento del Boton Nuevo
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio = new Cls_Cat_Tra_Plantillas_Negocio();

        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            switch (Btn_Nuevo.ToolTip)
            {
                case "Nuevo":
                    Estado_Botones("nuevo");
                    break;
                case "Dar de Alta":
                    Validar_Cajas();
                    Txt_Plantilla_ID.Text = Plantilla_Negocio.Generar_ID();
                    Validar_Subir_Archivo();
                    if (Div_Contenedor_Msj_Error.Visible == false)
                    {
                        Cargar_Datos_Negocio(true, Plantilla_Negocio);
                        Plantilla_Negocio.Alta_Plantilla();
                        Btn_Subir_Archivo.Enabled = false;
                        Plantilla_Negocio.P_Campo_Buscar = null;
                        Estado_Botones("inicial");
                        Txt_Busqueda.Text = "";
                        Llenar_Grid();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Btn_Nuevo_Click", "alert('Alta Exitosa');", true);
                    }
                    break;
            }//fin del switch
        }
        catch (Exception ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del Boton Modificar
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio = new Cls_Cat_Tra_Plantillas_Negocio();
        try
        {
            Mostrar_Mensaje_Error(false, "");

            if (Txt_Plantilla_ID.Text.Trim() == String.Empty)
            {
                Mostrar_Mensaje_Error(true, "* Debe seleccionar una plantilla <br />");
            }
            else
            {

                switch (Btn_Modificar.ToolTip)
                {
                    //Validacion para actualizar un registro y para habilitar los controles que se requieran

                    case "Modificar":
                        Estado_Botones("modificar");
                        Plantilla_Negocio.P_Campo_Buscar = null;
                        Llenar_Grid();
                        break;
                    case "Actualizar":
                        Validar_Cajas();
                        Validar_Subir_Archivo();
                        if (Div_Contenedor_Msj_Error.Visible == false)
                        {
                            Plantilla_Negocio = Cargar_Datos_Negocio(true, Plantilla_Negocio);
                            Plantilla_Negocio.Modificar_Plantilla();
                            Plantilla_Negocio.P_Campo_Buscar = null;
                           
                            Estado_Botones("inicial");
                            Txt_Busqueda.Text = "";
                            Llenar_Grid();

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Btn_Modificar_Click", "alert('Modificación Exitosa');", true);
                        }
                        break;
                }//fin del switch
            }
        }
        catch (Exception ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Documento_Click
    ///DESCRIPCIÓN  : Se maneja el evento del boton de crear documento de una plantilla.
    ///PARAMETROS   :     
    ///CREO         : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO   : 12/Noviembre/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Btn_Ver_Documento_Click(object sender, ImageClickEventArgs e)
    {
        String Ruta_Archivo = "";
        try
        {
            Mostrar_Mensaje_Error(false, "");

            if (Txt_Nombre_Archivo.Text != "")
            {
                Ruta_Archivo = "../../Plantillas_Word/" + Txt_Nombre_Archivo.Text.Trim();
                Mostrar_Archivo_Word(Ruta_Archivo);
            }
            else
            {
                Mostrar_Mensaje_Error(true, "Seleccione el regitro de la tabla que desea ver");
            }
            //if (sender != null)
            //{
            //    ImageButton Boton = (ImageButton)sender;
            //    String Documento = Boton.CommandArgument;
            //    String URL = null;
            //    for (Int32 Contador = 0; Contador < Grid_Documentos_Tramite.Rows.Count; Contador++)
            //    {
            //        if (Grid_Documentos_Tramite.Rows[Contador].Cells[0].Text.Equals(Documento))
            //        {
            //            //URL = Server.MapPath("../../Archivos/" + Txt_Clave_Solicitud.Text + "/" + Path.GetFileName(Grid_Documentos_Tramite.Rows[Contador].Cells[3].Text));
            //            URL = "../../Archivos/" + "TR-" + HDN_Solicitud_ID.Value + "/" + HttpUtility.HtmlDecode(Path.GetFileName(Grid_Documentos_Tramite.Rows[Contador].Cells[3].Text));
            //            break;
            //        }
            //    }
            //    if (URL != null)
            //    {
            //        Mostrar_Archivo_Word(URL);
            //    }
            //}

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Evento del Boton Eliminar
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio = new Cls_Cat_Tra_Plantillas_Negocio();
        try
        {

            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            //Validamos que este seleccionado una plantilla 
            if (Txt_Plantilla_ID.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "+ Debe seleccionar una plantilla para poder eliminar <br /> ";

            }
            else
            {
                Plantilla_Negocio = Cargar_Datos_Negocio(true, Plantilla_Negocio);
                Plantilla_Negocio.Eliminar_Plantilla();
                Eliminar_Archivo();
                //Llenamos el grid nuevamente
                Plantilla_Negocio.P_Campo_Buscar = null;

                Txt_Busqueda.Text = "";
                Llenar_Grid();
                //limpiamos las cajas 
                Estado_Botones("inicial");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Btn_Eliminar_Click", "alert('Baja Exitosa');", true);

            }
        }
        catch (Exception ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del Boton Salir
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio = new Cls_Cat_Tra_Plantillas_Negocio();

        switch (Btn_Salir.ToolTip)
        {
            case "Cancelar":
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
                Estado_Botones("inicial");
                Txt_Busqueda.Text = "";
                Plantilla_Negocio.P_Campo_Buscar = null;
                Llenar_Grid();
                break;

            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                break;
        }//fin del switch
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Evento del Boton Buscar
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Tra_Plantillas_Negocio Plantilla_Negocio = new Cls_Cat_Tra_Plantillas_Negocio();

        try
        {
            if (Txt_Busqueda.Text.Trim() == String.Empty)
            {
                Mostrar_Mensaje_Error(true, "* Debe insertar un nombre a buscar<br />");
                Estado_Botones("inicial");
                Llenar_Grid();
            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Llenar_Grid();
                Estado_Botones("inicial");

            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion


}
