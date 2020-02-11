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
/// Summary description for cat_ate_colonias
/// </summary>
public class Cls_Ate_Colonias_Negocio
{
    #region(Variables/Constantes)
    
    private String nombre;
    private String descripcion;
    private String colonia_id;
    private String usuario;
    private String Tipo_Colonia_ID;
    private String Costo_Construccion;
    private String Campos_Dinamicos;
    private String Filtros_Dinamicos;
    private String Agrupar_Dinamico;
    private String Ordenar_Dinamico;


    public String Usuario
    {
        get { return usuario; }
        set { usuario = value; }
    }


    public String Colonia_id
    {
        get { return colonia_id; }
        set { colonia_id = value; }
    }

    public String Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }


    public String Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }


    public String P_Tipo_Colonia_ID
    {
        get { return Tipo_Colonia_ID; }
        set { Tipo_Colonia_ID = value; }
    }

    public String P_Costo_Construccion
    {
        get { return Costo_Construccion; }
        set { Costo_Construccion = value; }
    }
    private Cls_Ate_Colonias_Datos colonia_dato;

    public String P_Campos_Dinamicos
    {
        get { return Campos_Dinamicos; }
        set { Campos_Dinamicos = value.Trim(); }
    }

    public String P_Filtros_Dinamicos
    {
        get { return Filtros_Dinamicos; }
        set { Filtros_Dinamicos = value.Trim(); }
    }

    public String P_Agrupar_Dinamico
    {
        get { return Agrupar_Dinamico; }
        set { Agrupar_Dinamico = value.Trim(); }
    }

    public String P_Ordenar_Dinamico
    {
        get { return Ordenar_Dinamico; }
        set { Ordenar_Dinamico = value.Trim(); }
    }

    #endregion

    #region(Metodos)

    public Cls_Ate_Colonias_Negocio()
	{
        colonia_dato = new Cls_Ate_Colonias_Datos();
	}
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Operaciones_Basicas
    ///DESCRIPCIÓN: Metodo que indica el tipo de operacion a realizar
    ///PROPIEDADES:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 23/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Operaciones_Basicas(String operacion)
    {
        colonia_dato.Ejecutar_Query(this,operacion);

    }//fin de dar alta 

     ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: BindGrid
    ///DESCRIPCIÓN: Metodo que llena el GridView
    ///PARAMETROS: GridView que se llenara
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 24/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************            
    public void BindGrid(GridView Grid_colonias)
    {

        DataSet data_set = colonia_dato.Llenar_Tabla();
        if (data_set != null)
        {
            Grid_colonias.DataSource = data_set;
            Grid_colonias.DataBind();
        }
    }//fin del BindGrid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: BindGrid
    ///DESCRIPCIÓN: Metodo que llena el GridView
    ///PARAMETROS: GridView que se llenara y el nombre de la busqueda
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 24/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************            

    public void BindGrid(GridView Grid_colonias, string nombre_busqueda)
    {

        DataSet data_set = colonia_dato.Realizar_Busqueda(nombre_busqueda);
        if (data_set != null)
        {
            Grid_colonias.DataSource = data_set;
            Grid_colonias.DataBind();
        }//fin del if 

    }//

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Colonias
    ///DESCRIPCIÓN          : Método para ejecutar la consulta de Colonias de la capa de datos
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guarado
    ///FECHA_CREO           : 16/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************            
    public DataTable Consultar_Colonias()
    {
        return Cls_Ate_Colonias_Datos.Consultar_Colonias(this);
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_ID
    ///DESCRIPCIÓN: Metodo que regresa un ID
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 24/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    public String Generar_ID()
    {
        //colonia_dato = new cat_ate_colonias_datos();
        String Id_generado = colonia_dato.consecutivo();
        return Id_generado;
    }// fin del Generar_ID


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Buscar_id
    ///DESCRIPCIÓN: Metodo que llama el metodo buscar_id de las colonias
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 06/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataTable Realizar_Busqueda_ID()
    {
        return Cls_Ate_Colonias_Datos.buscar_id(this);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: BindGrid
    ///DESCRIPCIÓN: Metodo para registrar la accion
    ///PARAMETROS: GridView que se llenara y el nombre de la busqueda
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 18/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************            

    public void Registrar_Bitacora(int Accion)
    {
        switch (Accion)
        { 
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;

        }//fin de switch
       

    }//Fin de Registrar_Bitacora

    #endregion
}
