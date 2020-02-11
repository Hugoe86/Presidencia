using System;
using System.IO;

public partial class paginas_Atencion_Ciudadana_Frm_Ate_Mostrar_Archivos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Parametro_Archivo = Request.QueryString["Reporte"];
            String Ruta = @Server.MapPath(Parametro_Archivo);
            Response.Clear();
            Response.ClearHeaders();
            if (Parametro_Archivo.ToLower().Contains(".pdf"))
            {
                Response.ContentType = "application/pdf";
            }
            else if (Parametro_Archivo.ToLower().Contains(".jpg") || Parametro_Archivo.ToLower().Contains(".jpeg"))
            {
                Response.ContentType = "image/jpeg";
            }
            else if (Parametro_Archivo.ToLower().Contains(".png"))
            {
                Response.ContentType = "image/png";
            }
            else if (Parametro_Archivo.ToLower().Contains(".ppt"))
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(Parametro_Archivo));
                Response.ContentType = "application/vnd.ms-powerpoint";
            }
            else if (Parametro_Archivo.ToLower().Contains(".docx"))
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(Parametro_Archivo));
                Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }
            else if (Parametro_Archivo.ToLower().Contains(".doc"))
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(Parametro_Archivo));
                Response.ContentType = "application/msword";
            }
            Response.WriteFile(Ruta);
            Response.Flush();
            Response.Close();
        }
    }

}
