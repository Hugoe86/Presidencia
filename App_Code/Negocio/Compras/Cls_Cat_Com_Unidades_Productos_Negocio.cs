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
/// Summary description for Cls_Cat_Com_Unidades_Productos_Negocio
/// </summary>
public class Cls_Cat_Com_Unidades_Productos_Negocio
{
	public Cls_Cat_Com_Unidades_Productos_Negocio()
	{
	}
        private String Unidad_ID;

public String P_Unidad_ID
{
  get { return Unidad_ID; }
  set { Unidad_ID = value; }
}
        private String Nombre;

public String P_Nombre
{
  get { return Nombre; }
  set { Nombre = value; }
}
private String Abreviatura;

public String P_Abreviatura
{
    get { return Abreviatura; }
    set { Abreviatura = value; }
}

    private String Comentarios;

public String P_Comentarios
{
    get { return Comentarios; }
    set { Comentarios = value; }
}
private String Usuario_Creo;      

public String P_Usuario_Creo
{
    get { return Usuario_Creo; }
    set { Usuario_Creo = value; }
}
}
