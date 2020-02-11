using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using AjaxControlToolkit;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Globalization;
using System.Text.RegularExpressions;

public partial class paginas_Ventanilla_Frm_Cat_Ven_Portafolio : System.Web.UI.Page
{
    #region Page load
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
        if (!IsPostBack)
        {
            if (String.IsNullOrEmpty(Cls_Sessiones.Ciudadano_ID))
            {
                Response.Redirect("Frm_Apl_Login_Ventanilla.aspx");
            }
            else
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Inicializar_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones 
            }
        }
    }

    #endregion


    #region Metodos generales

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializar_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
    ///               realizar diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 24/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializar_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial");
            Limpiar_Controles();
            Cargar_Grid();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 24/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Habilitar_Mensaje_Error(false);
            Hdf_Tramite_ID.Value = "";
            Hdf_Nombre_Tramite.Value = "";
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE:         Habilitar_Controles
    /// DESCRIPCION :   Habilita y Deshabilita los controles de la forma para prepara la página
    ///                 para a siguiente operación
    /// PARAMETROS:     1.- Operacion: Indica la operación que se desea realizar 
    /// CREO:           Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO:     24/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado = false; ///Indica si el control de la forma va hacer habilitado para utilización del usuario
        try
        {
             Habilitado = false;
             switch (Operacion)
             {
                 case "Inicial":
                     Div_Cargar_Archivo.Style.Value = "color: #5D7B9D; display:none";
                     Grid_Documentos_Ciudadano.Enabled = true;
                     break;
             }
        }

        catch (Exception ex)
        {
            throw new Exception("v " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Grid
    /// DESCRIPCION :cargara los documentos
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 24/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Grid()
    {
        Cls_Cat_Documentos_Negocio Negocio_Cargar_Documentos = new Cls_Cat_Documentos_Negocio();
        DataTable Dt_Documentos = new DataTable();
        DataSet Ds_Consulta = new DataSet();  
        try
        { 
            //  se realiza la consulta
            Ds_Consulta = Negocio_Cargar_Documentos.Consultar_Todo();
            //  se ordenan los campos por nombre
            Dt_Documentos = Ds_Consulta.Tables[0];
            DataView Dv_Ordenar = new DataView(Dt_Documentos);
            Dv_Ordenar.Sort = Cat_Tra_Documentos.Campo_Nombre;
            Dt_Documentos = Dv_Ordenar.ToTable();

            //  se carga el grid
            Grid_Documentos_Ciudadano.Columns[0].Visible = true;
            Grid_Documentos_Ciudadano.DataSource = Dt_Documentos;
            Grid_Documentos_Ciudadano.DataBind();
            Grid_Documentos_Ciudadano.Columns[0].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }
    
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: AFU_Subir_Archivo_UploadedComplete
    /// DESCRIPCION :subira el documento a su carpeta
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 24/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void AFU_Subir_Archivo_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
    {
        String Raiz = "";
        String Directorio_Portafolio = "";
        String Direccion_Archivo = "";
        String Extension = "";
        String URL = "";
        Boolean Estatus = false;
        String Nombre_Archivo = "";
        try
        {
            if (AFU_Subir_Archivo.HasFile)
            {

                Extension = Obtener_Extension(AFU_Subir_Archivo.FileName);
                String Ext = System.IO.Path.GetExtension(AFU_Subir_Archivo.FileName);
                int Tamaño_Archivo = AFU_Subir_Archivo.PostedFile.ContentLength;

                //  tamaño aproximado de 2MB
                if (Tamaño_Archivo < 2100000)
                {

                }

                if (Extension == "pdf" || Extension == "jpg" || Extension == "jpeg")
                {
                    Directorio_Portafolio = Cls_Sessiones.Ciudadano_ID;
                    Raiz = @Server.MapPath("../../Portafolio");
                    URL = AFU_Subir_Archivo.FileName;

                    //   se revisa que el directorio exista
                    if (!Directory.Exists(Raiz))
                    {
                        Directory.CreateDirectory(Raiz);
                    }

                    if (!Directory.Exists(Raiz + "/" + Directorio_Portafolio))
                    {
                        Directory.CreateDirectory(Raiz + "/" + Directorio_Portafolio);
                    }

                    //  se borraran el archivo anterior
                    String[] Archivos = Directory.GetFiles(MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));
                    //  se busca el archivo
                    for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                    {
                        Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                        if (Nombre_Archivo.Contains(Hdf_Nombre_Tramite.Value))
                        {
                            System.IO.File.Delete(Archivos[Contador].Trim());
                        }

                    }// fin del for


                    if (URL != "")
                    {
                        //verifica si existe un directorio
                        if (!Directory.Exists(Raiz + "/" + Directorio_Portafolio))
                        {
                            Directory.CreateDirectory(Raiz + "/" + Directorio_Portafolio);
                        }
                        // ejemplo de Direccion_Archivo: Portafolio/(id_Ciudadano)0000000002/(id_Documento)0000000003_
                        //                               Ife.jpg
                        Direccion_Archivo = Raiz + "/" + Directorio_Portafolio + "/" + Hdf_Tramite_ID.Value + "_";
                        Direccion_Archivo += Hdf_Nombre_Tramite.Value + "." + Obtener_Extension(AFU_Subir_Archivo.FileName);


                        if (AFU_Subir_Archivo.HasFile)
                        {
                            //se guarda el archivo
                            AFU_Subir_Archivo.SaveAs(Direccion_Archivo);
                            Estatus = true;

                        }// fin del if (AFU_Subir_Archivo.HasFile)

                    }// fin del  if (URL != "")

                }// fin del if pdf

                else
                {
                    //  formato incorrecto
                    Estatus = false;
                }

            }// fin AFU_Subir_Archivo.HasFile


            if (Estatus == true)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Ruta del archivo", "alert('Operacion exitosa" + "');", true);
                //Inicializar_Controles();

            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Ruta del archivo", "alert(El formato del archivo debe de ser: pdf, jpg, jpeg" + "');", true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
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
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Mensaje_Error
    ///DESCRIPCIÓN:          habilitara los mensajes de error
    ///PARAMETROS:           1.  Habilitar.  el tipo de visibilidad del objeto
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           24/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Habilitar_Mensaje_Error(Boolean Habilitar)
    {
        try
        {
            Img_Error.Visible = Habilitar;
            Lbl_Mensaje_Error.Visible = Habilitar;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
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
        try
        {
            if (System.IO.File.Exists(Ruta))
            {
                //System.Diagnostics.Process proceso = new System.Diagnostics.Process();
                //proceso.StartInfo.FileName = Ruta;
                //proceso.Start();
                //proceso.Close();
                String Archivo = "";
                Archivo = "../../Portafolio/" + Cls_Sessiones.Ciudadano_ID + "/" + Path.GetFileName(Ruta);
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

    #endregion

    #region Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  24/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Ventanilla/Frm_Apl_Ventanilla.aspx");
    }


    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Operacion_Click
    ///DESCRIPCIÓN: cargara el archivo
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  24/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Operacion_Click(object sender, EventArgs e)
    {
        String Operacion = "";
        try
        {
            if (AFU_Subir_Archivo.HasFile)
            {
                Inicializar_Controles();
                if (Btn_Operacion.Text == "SUBIR")
                {
                    Operacion = "Fue Subido al sistema";
                }
                else
                {
                    Operacion = "Fue Actualizado";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Operacion_Click", "alert('Archivo [" + Operacion + "]');", true);
                
            }
            else
            {
                Habilitar_Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "Seleccione el archivo a [" + Btn_Operacion.Text.ToString() + "]";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Operacion_Click " + ex.Message.ToString());
        } 
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Div_Click
    ///DESCRIPCIÓN: cargara el archivo
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  24/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cerrar_Div_Click(object sender, EventArgs e)
    {
        String Operacion = "";
        try
        {
            Inicializar_Controles();
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }

     ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Realizar_Operacion_Click
    ///DESCRIPCIÓN: cargara el archivo
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  24/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    //protected void Btn_Realizar_Operacion_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Hdf_Accion_Realizada.Value != "")
    //        {
    //            Inicializar_Controles();
    //        }
    //        else
    //        {
    //            Img_Error.Visible = true;
    //            Lbl_Mensaje_Error.Visible = true;
    //            Lbl_Mensaje_Error.Text = "Seleccione el archivo";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Cargar_Grid " + ex.Message.ToString());
    //    }
    //}
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Subir_Documento_Click
    ///DESCRIPCIÓN: cargara el archivo
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  24/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Subir_Documento_Click(object sender, ImageClickEventArgs e)
    {
        int Fila = 0;
        TableCell Celda = new TableCell();
        GridViewRow Renglon;
        ImageButton Boton = new ImageButton();
        try
        {
            Div_Cargar_Archivo.Style.Value = "color: #5D7B9D; display:block";
            Grid_Documentos_Ciudadano.Enabled = true;

            //  para obtener el id del documento 
            Boton = (ImageButton)sender;
            Celda = (TableCell)Boton.Parent;
            Renglon = (GridViewRow)Celda.Parent;
            Grid_Documentos_Ciudadano.SelectedIndex = Renglon.RowIndex;
            Fila = Renglon.RowIndex;

            //  sa pasa el id al campo oculto como el tipo de operacion 
            Limpiar_Controles();
            Grid_Documentos_Ciudadano.Columns[0].Visible = true;
            Hdf_Tramite_ID.Value = Grid_Documentos_Ciudadano.Rows[Fila].Cells[0].Text.Trim();
            Hdf_Nombre_Tramite.Value = Grid_Documentos_Ciudadano.Rows[Fila].Cells[1].Text.Trim();
            Grid_Documentos_Ciudadano.Columns[0].Visible = false;

            //  se desabilita el grid
            Grid_Documentos_Ciudadano.Enabled = false;
            Btn_Operacion.Text = "SUBIR";
           
            
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
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
            Div_Cargar_Archivo.Style.Value = "color: #5D7B9D; display:block";
            Grid_Documentos_Ciudadano.Enabled = true;

            //  para obtener el id del documento 
            Boton = (ImageButton)sender;
            Celda = (TableCell)Boton.Parent;
            Renglon = (GridViewRow)Celda.Parent;
            Grid_Documentos_Ciudadano.SelectedIndex = Renglon.RowIndex;
            Fila = Renglon.RowIndex;

            //  sa pasa el id al campo oculto como el tipo de operacion
            Limpiar_Controles();
            Grid_Documentos_Ciudadano.Columns[0].Visible = true;
            Hdf_Tramite_ID.Value = Grid_Documentos_Ciudadano.Rows[Fila].Cells[0].Text.Trim();
            Hdf_Nombre_Tramite.Value = Grid_Documentos_Ciudadano.Rows[Fila].Cells[1].Text.Trim();
            Grid_Documentos_Ciudadano.Columns[0].Visible = false;

            //  se desabilita el grid
            Grid_Documentos_Ciudadano.Enabled = false;
            Btn_Operacion.Text = "ACTUALIZAR";
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
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
        try
        {
            //  para obtener el id del documento 
            Boton = (ImageButton)sender;
            Celda = (TableCell)Boton.Parent;
            Renglon = (GridViewRow)Celda.Parent;
            Grid_Documentos_Ciudadano.SelectedIndex = Renglon.RowIndex;
            Fila = Renglon.RowIndex;

            //  se obtiene el nombre del documento y el id del ciudadano
            Nombre_Documento = Grid_Documentos_Ciudadano.Rows[Fila].Cells[1].Text.Trim();
            Directorio_Portafolio = Cls_Sessiones.Ciudadano_ID;

            //  se obtiene el nombre de los archivos existentes en la carpeta
            String[] Archivos = Directory.GetFiles(MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));

            //  se busca el archivo
            for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
            {
                Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                if (Nombre_Archivo.Contains(Nombre_Documento))
                {
                    URL = Archivos[Contador].Trim();
                    Mostrar_Archivo(URL);
                    break;
                }

            }// fin del for

            //if (URL != null)
            //{
            //    Mostrar_Archivo(URL);
            //}

        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }
    #endregion

    #region Grid
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Documentos_Ciudadano_RowDataBound
    ///DESCRIPCIÓN          :cargara los botones dentro del grid
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO          : 24/Mayo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Documentos_Ciudadano_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        String Raiz = "";
        String Directorio_Portafolio = "";
        String Direccion_Archivo = "";
        String Extension = "";
        String Nombre_Archivo = "";
        String Nombre_Documento = "";
        Boolean Estatus = false;
        Boolean Encontrado = false;
        try
        {
           
            //Direccion_Archivo = Raiz + "/" + Directorio_Portafolio + "/" + Hdf_Tramite_ID.Value + "_";
            //Direccion_Archivo += Hdf_Nombre_Tramite.Value + "." + Obtener_Extension(AFU_Subir_Archivo.FileName);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Directorio_Portafolio = Cls_Sessiones.Ciudadano_ID;
                Raiz = @Server.MapPath("../../Portafolio");
                Nombre_Documento = e.Row.Cells[1].Text;
                Direccion_Archivo = Raiz + "/" + Directorio_Portafolio + "/" + Hdf_Tramite_ID.Value + "_" + Nombre_Archivo;

                ImageButton Imagen_Subir = (ImageButton)e.Row.Cells[3].FindControl("Btn_Subir_Documento");
                ImageButton Imagen_Actualizar = (ImageButton)e.Row.Cells[4].FindControl("Btn_Acutalizar_Documento");
                ImageButton Imagen_Mostrar = (ImageButton)e.Row.Cells[5].FindControl("Btn_Ver_Documento");

                //  si existe el directorio se busca que archivos de los tramites ya subio
                if (!Directory.Exists(Raiz + "/" + Directorio_Portafolio))
                {
                    Imagen_Actualizar.Enabled = false;
                    Imagen_Mostrar.Enabled = false;
                    Imagen_Actualizar.Visible = false;
                    Imagen_Mostrar.Visible = false;
                }
                else
                {
                    String[] Archivos = Directory.GetFiles(MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));
                    for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                    {
                        Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                        if (Nombre_Archivo.Contains(Nombre_Documento))
                        {
                            Imagen_Subir.Enabled = false;
                            Imagen_Subir.Visible = false;
                            Encontrado = true;
                            break;
                        }

                    }// fin del for

                    if (Encontrado == false)
                    {
                        Imagen_Actualizar.Enabled = false;
                        Imagen_Mostrar.Enabled = false;
                        Imagen_Actualizar.Visible = false;
                        Imagen_Mostrar.Visible = false;
                    }
                
                }// fin del else
            
            }// fin del if (e.Row.RowType)

        }// fin del try
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion
}
