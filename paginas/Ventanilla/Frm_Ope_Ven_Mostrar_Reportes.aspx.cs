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

public partial class paginas_Ventanilla_Frm_Ope_Ven_Mostrar_Reportes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String Extension_Archivo = String.Empty;
        String Nombre_Archivo = "";
        String Ruta = "";

        if (!IsPostBack)
        {
            try
            {
                Nombre_Archivo = Request.QueryString["Reporte"];
                Ruta = @Server.MapPath("../../Reporte/") + Nombre_Archivo;
                Extension_Archivo = Path.GetExtension(Ruta);
            }
            catch { }

            Response.Clear();
            Response.ClearHeaders();

            switch (Extension_Archivo)
            {
                case ".docx":
                    try
                    {
                        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(Nombre_Archivo));
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        Response.WriteFile(Ruta);
                    }
                    catch { }
                    break;
                case ".xls":
                    try
                    {
                        Response.ContentType = "application/x-msexcel";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + Nombre_Archivo);
                        Response.TransmitFile(Ruta);
                    }
                    catch { }
                    break;
                case ".xlsx":
                    try
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + Nombre_Archivo);
                        Response.TransmitFile(Ruta);
                    }
                    catch { }
                    break;
                case ".pdf":
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(Ruta);
                    break;

                default:
                    break;
            }
            Response.Flush();
            Response.Close();

            File.Delete(Ruta);
        }
    }

}
