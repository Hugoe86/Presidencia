using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Cls_Ope_Pre_Pae_Depositario_Negocios
/// </summary>
public class Cls_Ope_Pre_Pae_Depositario_Negocios
{
	public Cls_Ope_Pre_Pae_Depositario_Negocios()
	{}
    #region Variable Internas
    //Para la tabla de Depositario
    private String No_Depositario;
    private String No_Detalle_Etapa;
    private String Figura;
    private String Nombre_Depositario;
    private String Domicilio_Depositario;
    private String Fecha_Remocion;
    #endregion

    #region Variables Publicas
    public String P_No_Depositario
    {
        get { return No_Depositario; }
        set { No_Depositario = value; }
    }
    public String P_No_Detalle_Etapa
    {
        get { return No_Detalle_Etapa; }
        set { No_Detalle_Etapa = value; }
    }
    public String P_Figura
    {
        get { return Figura; }
        set { Figura = value; }
    }
    public String P_Nombre_Depositario
    {
        get { return Nombre_Depositario; }
        set { Nombre_Depositario = value; }
    }
    public String P_Domicilio_Depositario
    {
        get { return Domicilio_Depositario; }
        set { Domicilio_Depositario = value; }
    }
    public String P_Fecha_Remocion
    {
        get { return Fecha_Remocion; }
        set { Fecha_Remocion = value; }
    }
    #endregion

    #region Metodos
    public void Alta_Depositario()
    {
        Cls_Ope_Pre_Pae_Depositario_Datos.Alta_Pae_Det_Etapas_Depositario(this);
    }
    #endregion
}
