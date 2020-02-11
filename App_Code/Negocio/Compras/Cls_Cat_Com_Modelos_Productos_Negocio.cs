using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for Cls_Cat_Com_Modelos_Productos
/// </summary>
public class Cls_Cat_Com_Modelos_Productos
{
	public Cls_Cat_Com_Modelos_Productos()
	{
	}

    private String Modelo_ID;
    private String Nombre;
    private String Comentarios;
    private String Usuario_Creo;

    public String P_Modelo_ID
    {
        get { return Modelo_ID; }
        set { Modelo_ID = value; }
    }
    
    public String P_Nombre
    {
        get { return Nombre; }
        set { Nombre = value; }
    }
    
    public String P_Comentarios
    {
        get { return Comentarios; }
        set { Comentarios = value; }
    }
    
    public String P_Usuario_Creo
    {
        get { return Usuario_Creo; }
        set { Usuario_Creo = value; }
    }
}
