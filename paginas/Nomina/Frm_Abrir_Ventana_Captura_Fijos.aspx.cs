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
public partial class paginas_Nomina_Frm_Abrir_Ventana_Captura_Fijos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterClientScriptBlock(this.GetType(), "Captura Fijos",
            "window.open('Frm_Ope_Nom_Captura_Fijos.aspx','_blank', 'width=800, height=600, top=0, left=0, resizable=1');", true);
    }

}
