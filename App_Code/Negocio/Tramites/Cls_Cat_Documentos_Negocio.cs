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
/// Summary description for Cls_Cat_Documentos
/// </summary>
public class Cls_Cat_Documentos_Negocio
{
    private String ID;
    private String Nombre;
    private String Comentarios;
    private String Busqueda;
    private String Usuario_Creo_Modifico;

	public Cls_Cat_Documentos_Negocio()
	{
        
	}
    public String P_Usuario_Creo_Modifico
    {
        get { return Usuario_Creo_Modifico; }
        set { Usuario_Creo_Modifico = value; }
    }
    public String P_Buscar
    {
        get { return Busqueda; }
        set { Busqueda = value; }
    }
    public String P_ID
    {
        get { return ID; }
        set { ID = value; }
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
    #region "metodos"
    public void Alta() {
        Cls_Cat_Documentos_Datos.Alta(this);
    }
    public void Modificar()
    {
        Cls_Cat_Documentos_Datos.Modificar(this);
    }
    public void Eliminar()
    {
        Cls_Cat_Documentos_Datos.Eliminar(this);
    }
    public DataSet Consultar_Todo() 
    {
        return Cls_Cat_Documentos_Datos.Consultar_Documento();
    }
    public DataSet Busqueda_Por_Nombre()
    {
        return Cls_Cat_Documentos_Datos.Busqueda_Por_Nombre(this);
    }
    #endregion
}
