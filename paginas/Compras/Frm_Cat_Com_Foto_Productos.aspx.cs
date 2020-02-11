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
using AjaxControlToolkit;
using System.IO;
using System.Globalization;
using Presidencia.Catalogo_Compras_Productos.Negocio;

public partial class paginas_Compras_Frm_Cat_Com_Foto_Productos : System.Web.UI.Page
{

    #region Variables Globales


    #endregion


    #region LOAD

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Estatus_Inicial();
        }
    }

    #endregion


    #region Metodos

    private void Estatus_Inicial()
    {
        String Producto_ID = HttpUtility.HtmlDecode(Request.QueryString["Producto_ID"]).Trim();
        String Modificar = HttpUtility.HtmlDecode(Request.QueryString["Modificar"]).Trim();
        String Pagina_Productos = HttpUtility.HtmlDecode(Request.QueryString["Pagina_P"]).Trim();
        Session["Pagina_Productos"] = Pagina_Productos;
        Session["Producto_ID"] = Producto_ID;

        if (Modificar=="true")
        {
            Cargar_Foto_Producto( Producto_ID );
        }
    }

    private void Actualizar_Foto_Producto()
    {
        if (@Txt_Ruta_Foto.ToString().Trim() != "")
        {
            Cls_Cat_Com_Productos_Negocio Rs_Alta_Producto = new Cls_Cat_Com_Productos_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
            Rs_Alta_Producto.P_Ruta_Foto = @Txt_Ruta_Foto.Value.ToString().Trim();
            Rs_Alta_Producto.P_Producto_ID = Session["Producto_ID"].ToString();
            Rs_Alta_Producto.Modificar_Foto_Producto();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Productos ", "alert('Se Agregó La Foto Correctamente');", true);

            if (Session["Pagina_Productos"].ToString() != null)
            {
                String ruta = "../Compras/Frm_Cat_Com_Productos.aspx?PAGINA=" + Session["Pagina_Productos"].ToString().Trim();
                Response.Redirect(ruta);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('No se ha seleccionado ninguna foto a guardar');", true);
        }
    }

    private void Cargar_Foto_Producto( String Producto_ID)
    {
        Cls_Cat_Com_Productos_Negocio Rs_Modificar_Producto = new Cls_Cat_Com_Productos_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        Rs_Modificar_Producto.P_Producto_ID = Producto_ID.Trim();

        String Ruta_Foto = @HttpUtility.HtmlDecode(Rs_Modificar_Producto.Consulta_Foto_Producto());

        if (Ruta_Foto.Trim() != "")
        {
            //Se obtiene la direccion en donde se va a guardar el archivo. Ej. C:/Dir_Servidor/..
            String Ruta_Servidor_Productos = Server.MapPath("Fotos_Productos");
            String Ruta_Completa = Ruta_Servidor_Productos + "\\" + Ruta_Foto.Trim();
            Img_Foto_Producto.ImageUrl = Ruta_Completa;
            Img_Foto_Producto.DataBind();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert(' El Producto no Tiene Foto');", true);
        }
    }


    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Remover_Sesiones_Control_Carga_Archivos
    /// DESCRIPCION : Remueve la sesion del Ctlr AsyncFileUpload que mantiene al archivo
    /// en memoria.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Remover_Sesiones_Control_Carga_Archivos(String Client_ID)
    {
        HttpContext currentContext;
        if (HttpContext.Current != null && HttpContext.Current.Session != null)
        {
            currentContext = HttpContext.Current;
        }
        else
        {
            currentContext = null;
        }

        if (currentContext != null)
        {
            foreach (String key in currentContext.Session.Keys)
            {
                if (key.Contains(Client_ID))
                {
                    currentContext.Session.Remove(key);
                    break;
                }
            }
        }
    }

    #endregion


    #region Eventos


    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        Actualizar_Foto_Producto();
    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["Pagina_Productos"].ToString() != null)
        {
            String ruta = "../Compras/Frm_Cat_Com_Productos.aspx?PAGINA=" + Session["Pagina_Productos"].ToString().Trim();
            Response.Redirect(ruta);
        }
    }

    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Subir_Foto_Click
    /// DESCRIPCIÓN:          Carga la Foto del empleado a dar de alta
    /// CREO:                 Juan Alberto Hernandez Negrete
    /// FECHA_CREO:           30/Octubre/2010
    /// MODIFICO:  
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    protected void Subir_Foto_Click(object sender, ImageClickEventArgs e)
    {
        String Ruta_Servidor_Productos = "";
        String Nombre_Dir_Productos = "";
        AsyncFileUpload Asy_FileUpload;
       
        try
        {
            if (Async_Foto_Producto.HasFile)
            {
                //Se obtiene la direccion en donde se va a guardar el archivo. Ej. C:/Dir_Servidor/..
                Ruta_Servidor_Productos = Server.MapPath("Fotos_Productos");

                //Crear el Directorio Proveedores. Ej. Proveedores
                if (!Directory.Exists(Ruta_Servidor_Productos))
                {
                    System.IO.Directory.CreateDirectory(Ruta_Servidor_Productos);
                }

                //Se establece el nombre del directorio Ej. Producto_00005
                Nombre_Dir_Productos = "Producto_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));


                if (Directory.Exists(Ruta_Servidor_Productos))
                {
                    //Obtenemos el Ctlr AsyncFileUpload del GridView.
                    Asy_FileUpload = Async_Foto_Producto;

                    //Validamos que el nombre del archivo no se encuentre vacio.
                    if (!Asy_FileUpload.FileName.Equals(""))
                    {
                        //Valida que no exista el directorio, si no existe lo crea [172.16.0.103/Web/Project/Empleado/Empleado_00001]
                        DirectoryInfo Ruta_Completa_Dir_Productos;
                        if (!Directory.Exists(Ruta_Servidor_Productos + Nombre_Dir_Productos))
                        {
                            Ruta_Completa_Dir_Productos = Directory.CreateDirectory(Ruta_Servidor_Productos + @"\" + Nombre_Dir_Productos);
                        }

                        //Se asigna el directorio en donde se va a guardar los documentos. Ej. [Empleado/]
                        String Ruta_Dir_Product = Nombre_Dir_Productos + @"\";

                        //Se establece la ruta completa del archivo . Ej. [172.16.0.103/Web/Project/Empleado/Empleado_00001/File1.txt]
                        String Ruta_Completa_Archivo_A_Cargar = Ruta_Servidor_Productos + @"\" + Ruta_Dir_Product +
                            Nombre_Dir_Productos + "." + Asy_FileUpload.FileName.Split(new Char[] { '.' })[1];

                        //Se valida que el Ctlr AsyncFileUpload. Contenga el archivo a guardar.
                        if (Asy_FileUpload.HasFile)
                        {
                            DirectoryInfo directory = new DirectoryInfo((Ruta_Servidor_Productos + @"\" + Ruta_Dir_Product));

                            foreach (FileInfo fi in directory.GetFiles())
                            {
                                File.Delete((Ruta_Servidor_Productos + @"\" + Ruta_Dir_Product) + @"\" + fi.Name);
                            }

                            //Se guarda el archivo. En la ruta indicada. Ej.  [172.16.0.103/Web/Project/Empleado/Empleado_00001/File1.txt]
                            Asy_FileUpload.SaveAs(Ruta_Completa_Archivo_A_Cargar);
                            //Guardamos en el campo hidden la ruta de la foto del producto
                            Txt_Ruta_Foto.Value = @HttpUtility.HtmlDecode("Foto_Producto" + @"\" + Ruta_Dir_Product + Nombre_Dir_Productos + "." + Asy_FileUpload.FileName.Split(new Char[] { '.' })[1]);
                            Img_Foto_Producto.ImageUrl = @HttpUtility.HtmlDecode("Foto_Producto" + @"\" + Ruta_Dir_Product + Nombre_Dir_Productos + "." + Asy_FileUpload.FileName.Split(new Char[] { '.' })[1]);
                            Img_Foto_Producto.DataBind();
                            Remover_Sesiones_Control_Carga_Archivos(Async_Foto_Producto.ClientID);
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('No se ha seleccionado ninguna foto a guardar');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al cargar la foto del empleado. ERROR: [" + Ex.Message + "]");
        }

    }

    #endregion


    
}
