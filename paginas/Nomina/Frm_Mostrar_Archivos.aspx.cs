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
using System.IO;

public partial class paginas_Nomina_Frm_Mostrar_Archivos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String Ruta_Documento_Servidor = String.Empty;

        try
        {
            if (Request.QueryString["Documento"] != null)
            {
                Ruta_Documento_Servidor = Request.QueryString["Documento"];

                if (!Page.IsPostBack)
                {
                    Abrir_Documento_Anexado(Ruta_Documento_Servidor);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al abrir los archivos de dispersión a bancos. Error: [" + Ex.Message + "]");
        }
    }

    protected void Abrir_Documento_Anexado(String Ruta_Documento_Servidor)
    {
        String Nombre_Archivo = String.Empty;
        String Extensión_Archivo = String.Empty;
        String Tipo_Archivo = String.Empty;

        try
        {
            Nombre_Archivo = Path.GetFileName(Ruta_Documento_Servidor);
            Extensión_Archivo = Path.GetExtension(Ruta_Documento_Servidor);

            if (!String.IsNullOrEmpty(Extensión_Archivo))
            {
                Extensión_Archivo = Extensión_Archivo.Trim().ToUpper();
            }

            switch (Extensión_Archivo)
            {
                case ".html":
                    Tipo_Archivo = "text/HTML";
                    break;
                case "htm":
                    Tipo_Archivo = "text/HTML";
                    break;
                case ".txt":
                    Tipo_Archivo = "text/plain";
                    break;
                case ".doc":
                    Tipo_Archivo = "Application/msword";
                    break;
                case ".DOCX":
                    Tipo_Archivo = "vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case "xlsx":
                   Tipo_Archivo = "application/vnd.ms-excel.12";
                    break;
                case ".xls":
                    Tipo_Archivo = "Application/msword";
                    break;
                case ".pdf":
                    Tipo_Archivo = "Application/pdf";
                    break;
                default:
                    Tipo_Archivo = "text/plain";
                    break;
            }

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = Tipo_Archivo;
            Response.AppendHeader("content-disposition", "attachment; filename=" + Nombre_Archivo);
            Response.WriteFile(Ruta_Documento_Servidor);
            Response.Flush();
            Response.Close();
            System.IO.File.Delete(Ruta_Documento_Servidor);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al abrir los archivos de dispersión a bancos. Error: [" + Ex.Message + "]");
        }
    }
}
