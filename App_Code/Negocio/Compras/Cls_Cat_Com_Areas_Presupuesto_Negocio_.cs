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
/// Summary description for Cls_Cat_Com_Areas_Presupuesto_Negocio_
/// </summary>
public class Cls_Cat_Com_Areas_Presupuesto_Negocio_
{
	public Cls_Cat_Com_Areas_Presupuesto_Negocio_()
	{
	}

    //Propiedades
    private String Area_ID;
    private int Año;
    private Double Autorizado;
    private Double Transito;
    private Double Comprometido;
    private Double Ejercido;
    private Double Disponible;
    private String Comentarios;
    private String Nombre_Usuario;

    public String P_Area_ID
    {
        get { return Area_ID; }
        set { Area_ID = value; }
    }
    
    public int P_Año
    {
        get { return Año; }
        set { Año = value; }
    }
    
    public Double P_Autorizado
    {
        get { return Autorizado; }
        set { Autorizado = value; }
    }
    
    public Double P_Transito
    {
        get { return Transito; }
        set { Transito = value; }
    }
    
    public Double P_Comprometido
    {
        get { return Comprometido; }
        set { Comprometido = value; }
    }
    
    public Double P_Ejercido
    {
        get { return Ejercido; }
        set { Ejercido = value; }
    }
    
    public Double P_Disponible
    {
        get { return Disponible; }
        set { Disponible = value; }
    }
    
    public String P_Comentarios
    {
        get { return Comentarios; }
        set { Comentarios = value; }
    }
    
    public String P_Nombre_Usuario
    {
        get { return Nombre_Usuario; }
        set { Nombre_Usuario = value; }
    }         
}
