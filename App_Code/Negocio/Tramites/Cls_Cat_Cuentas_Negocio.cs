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
/// Summary description for Cls_Cat_Cuentas_Negocio
/// </summary>
public class Cls_Cat_Cuentas_Negocio
{
    private String Cuenta_ID;
    private String Dependencia_ID;
    private String Numero_Cuenta;
    private String Banco;
    private String Comentarios;
    private String Busqueda;
    private String Usuario_Creo_Modifico;

    public Cls_Cat_Cuentas_Negocio()
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
    public String P_Cuenta_ID
    {
        get { return Cuenta_ID; }
        set { Cuenta_ID = value; }
    }
    public String P_Dependencia_ID
    {
        get { return Dependencia_ID; }
        set { Dependencia_ID = value; }
    }
    public String P_Numero_Cuenta
    {
        get { return Numero_Cuenta; }
        set { Numero_Cuenta = value; }
    }

    public String P_Banco
    {
        get { return Banco; }
        set { Banco = value; }
    }
    public String P_Comentarios
    {
        get { return Comentarios; }
        set { Comentarios = value; }
    }
    public void Alta_Cuenta() {
        Cls_Cat_Cuentas_Datos.Alta_Cuenta(this);
    }
    public void Modificar_Cuenta()
    {
        Cls_Cat_Cuentas_Datos.Modificar_Cuenta(this);
    }
    public void Eliminar_Cuenta()
    {
        Cls_Cat_Cuentas_Datos.Eliminar_Cuenta(this);
    }
    public DataSet Consultar_Todas_Cuentas() 
    {
        return Cls_Cat_Cuentas_Datos.Consultar_Todas_Cuentas(this);
    }
    public DataSet Busqueda_Por_Banco()
    {
        return Cls_Cat_Cuentas_Datos.Busqueda_Por_Banco(this);
    }
    public DataSet Busqueda_Por_Num_Cuenta()
    {
        return Cls_Cat_Cuentas_Datos.Busqueda_Por_Num_Cuenta(this);
    }

}
