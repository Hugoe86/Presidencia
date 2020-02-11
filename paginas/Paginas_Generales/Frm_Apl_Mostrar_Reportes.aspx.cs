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

public partial class paginas_Paginas_Generales_Frm_Apl_Mostrar_Reportes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            String Ruta = @Server.MapPath("../../Reporte/") + Request.QueryString["Reporte"];
            Response.Clear();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.WriteFile(Ruta);
            Response.Flush();
            Response.Close();
            System.IO.File.Delete(Ruta);
        }
    }
   
}
