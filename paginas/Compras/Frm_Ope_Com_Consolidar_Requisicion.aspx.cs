using System;
using System.Collections;
using System.Collections.Generic;
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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Consolidar_Requisicion.Negocio;

public partial class paginas_Compras_Frm_Ope_Com_Consolidar_Requisicion : System.Web.UI.Page
{
    #region ATRIBUTOS
    private Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio_Consolidar;
    private static DataTable P_Dt_Detalles_Consolidacion;
    private static String P_Requisas_Seleccionadas;
    private static String Parametro;
    private static DataTable P_Dt_Requisiciones;
    private const String INICIAL = "inicial";
    private const String NUEVO = "nuevo";
    private const String MODIFICAR = "modificar";
    #endregion

    #region LOAD/INIT
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Txt_Estatus.Text = "FILTRADA";
            Txt_Tipo.Text = "TRANSITORIA";
            Llenar_Combos_Generales();
            Llenar_Grid_Requisiciones_Filtradas();            
            Manejo_Controles(INICIAL);
            if (Request.QueryString != null)
            {
                Parametro = Request.QueryString.ToString();
                if (Parametro.Trim().Length > 0) 
                {
                    Modo_Modificar();
                }
            }
        }
        Mostrar_Informacion("",false);
    }
    #endregion

    #region EVENTOS
    protected void Btn_Ver_Consolidaciones_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Compras/Frm_Ope_Com_Listado_Consolidaciones.aspx");
    }
    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Guardar.ToolTip == "Guardar Consolidación")
        {
            Guardar_Consolidacion();
        }
        else if (Btn_Guardar.ToolTip == "Actualizar Consolidación")
        {
            //System.Windows.Forms.MessageBox.Show("Actua");
            Actualizar_Consolidacion();
        }
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Inicio")
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        else 
        {
            Manejo_Controles(INICIAL);
            Llenar_Grid_Requisiciones_Filtradas();
            Grid_Requisas_Consolidadas.DataSource = null;
            Grid_Requisas_Consolidadas.DataBind();
        }
    }
    private int Evento_Boton_Consolidar()
    {
        String Requisas_Seleccionadas = Verifica_Requisas_Seleccionadas();
        P_Requisas_Seleccionadas = Requisas_Seleccionadas;
        if (Requisas_Seleccionadas == "")
        {
            Mostrar_Informacion("Debe seleccionar al menos una requisición para consolidar", true);
            return 0;
        }
        Negocio_Consolidar = new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
        Negocio_Consolidar.P_Requisas_Seleccionadas = Requisas_Seleccionadas;
        Negocio_Consolidar.P_Estatus = Txt_Estatus.Text;
        DataTable Dt_Articulos = null;
        if (Cmb_Tipo_Articulo.SelectedValue == "PRODUCTO")
        {
            Dt_Articulos = Negocio_Consolidar.Consolidar_Requisiciones_Productos();
        }
        else if (Cmb_Tipo_Articulo.SelectedValue == "SERVICIO") 
        {
            Dt_Articulos = Negocio_Consolidar.Consolidar_Requisiciones_Servicios();
        }
        P_Dt_Detalles_Consolidacion = Dt_Articulos;
        Grid_Requisas_Consolidadas.DataSource = Dt_Articulos;
        Grid_Requisas_Consolidadas.DataBind();
        Txt_Total.Text = Obtener_Monto_Consolidacion() + "";
        
        return 1;
    }
    protected void Btn_Consolidar_Click(object sender, ImageClickEventArgs e)
    {
        Manejo_Controles(NUEVO);
        Evento_Boton_Consolidar();
        if (P_Requisas_Seleccionadas != "")
        {
            Btn_Guardar.Visible = true;
        }        
    }
    protected void Cmb_Tipo_Articulo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Grid_Requisiciones_Filtradas();
    }
    #endregion

    #region EVENTOS GRID
    protected void Grid_Requisas_Consolidadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Requisas_Consolidadas.DataSource = P_Dt_Detalles_Consolidacion;
        Grid_Requisas_Consolidadas.PageIndex = e.NewPageIndex;
        Grid_Requisas_Consolidadas.DataBind();
    }
    #endregion

    #region METODOS
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Mostrar_Información
    //DESCRIPCIÓN: Llena las areas de texto con el registro seleccionado del grid
    //PARAMETROS: 
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 24/Agosto/2010 
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private void Mostrar_Informacion(String txt, Boolean mostrar)
    {
        Lbl_Informacion.Visible = mostrar;
        Img_Warning.Visible = mostrar;
        Lbl_Informacion.Text = txt;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Verifica_Requisas_Seleccionadas
    //  DESCRIPCIÓN: Este método obtiene un String con las requisas que se 
    //  seleccionaron, con formato: '000000','00000000'... para poder poner este dato
    //  en la busqueda
    //  que tienen las requisas con estatus de filtradas
    /// PARAMETROS  : 
    /// CREO        : Gustavo Angeles Cruz
    /// FECHA_CREO  : 27-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public String Verifica_Requisas_Seleccionadas()
    {
        String Requisas_Seleccionadas = "";
        int Contador = 0;
        foreach (GridViewRow Renglon_Grid in Grid_Requisiciones.Rows)
        {
            bool Seleccionado = ((System.Web.UI.WebControls.CheckBox)Renglon_Grid.FindControl("Chk_Requisa")).Checked;
            if (Seleccionado)
            {
                Grid_Requisiciones.SelectedIndex = Contador;
                Requisas_Seleccionadas += ("'" + Grid_Requisiciones.SelectedDataKey["NO_REQUISICION"].ToString() + "',");
            }
            Contador++;
        }
        if (Requisas_Seleccionadas.Length > 0)
        {
            Requisas_Seleccionadas = Requisas_Seleccionadas.Substring(0, Requisas_Seleccionadas.Length - 1);
        }
        return Requisas_Seleccionadas;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Monto_Consolidacion
    //  DESCRIPCIÓN: Este método obtiene un String con las requisas que se 
    //  seleccionaron, con formato: '000000','00000000'... para poder poner este dato
    //  en la busqueda
    //  que tienen las requisas con estatus de filtradas
    /// PARAMETROS  : 
    /// CREO        : Gustavo Angeles Cruz
    /// FECHA_CREO  : 27-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private double Obtener_Monto_Consolidacion()
    {
        //String Requisas_Seleccionadas = "";
        int Contador = 0;
        double Suma = 0;
        foreach (GridViewRow Renglon_Grid in Grid_Requisiciones.Rows)
        {
            bool Seleccionado = ((System.Web.UI.WebControls.CheckBox)Renglon_Grid.FindControl("Chk_Requisa")).Checked;
            if (Seleccionado)
            {
                Grid_Requisiciones.SelectedIndex = Contador;
                GridViewRow Row = Grid_Requisiciones.SelectedRow;
                String ID = Row.Cells[6].Text;
                Suma += double.Parse(Grid_Requisiciones.SelectedDataKey["TOTAL"].ToString());
            }
            Contador++;
        }
        return Suma;
    }

    //temporal
    //public DataTable Llenar_Grid_Requisiciones_Filtradas1()
    //{
    //    Negocio_Consolidar = new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
    //    Negocio_Consolidar.P_Requisas_Seleccionadas = Verifica_Requisas_Seleccionadas();
    //    Negocio_Consolidar.P_Estatus = Txt_Estatus.Text;
    //    DataTable _DataTable = Negocio_Consolidar.Consultar_Requisicion();
    //    return _DataTable;
    //}

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Productos_Requisas_Filtradas
    //  DESCRIPCIÓN: Este método obtiene un arreglo se String con todos los productos
    //  que tienen las requisas con estatus de filtradas
    /// PARAMETROS  : 
    /// CREO        : Gustavo Angeles Cruz
    /// FECHA_CREO  : 28-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public void Llenar_Grid_Requisiciones_Filtradas()
    {
        Negocio_Consolidar = new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
        Negocio_Consolidar.P_Estatus = Txt_Estatus.Text;
        //Negocio_Consolidar.P_Estatus = "GENERADA','EN CONSTRUCCION','AUTORIZADA','FILTRADA','CONFIRMADA','SURTIDA";
        //Negocio_Consolidar.P_Tipo = "'TRANSITORIO'";
        //Negocio_Consolidar.P_Tipo = "'STOCK','TRANSITORIA'";
        Negocio_Consolidar.P_Tipo = Txt_Tipo.Text;
        Negocio_Consolidar.P_Tipo_Articulo = Cmb_Tipo_Articulo.SelectedValue;
        Llenar_Combos_Generales();
        DataTable _DataTable = Negocio_Consolidar.Consultar_Requisicion();
        if (_DataTable != null)
        {
            DataColumn Columna = new DataColumn("GRUPO", System.Type.GetType("System.String"));
            _DataTable.Columns.Add(Columna);
            String[] Requisas_Agrupadas = Comparar_Productos_De_Requisas();
            _DataTable = Marcar_Requisas_Similares(Requisas_Agrupadas, _DataTable);
        }
        if (_DataTable != null)
        {
            P_Dt_Requisiciones = _DataTable;
            DataView Dv = _DataTable.DefaultView;
            Dv.Sort = "GRUPO";
            Grid_Requisiciones.DataSource = Dv;//_DataTable;
            Grid_Requisiciones.DataBind();
        }
        else
        {
            Grid_Requisiciones.DataSource = null;
            Grid_Requisiciones.DataBind();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Productos_Requisas_Filtradas
    //  DESCRIPCIÓN: Este método obtiene un arreglo se String con todos los productos
    //  que tienen las requisas con estatus de filtradas
    /// PARAMETROS  : 
    /// CREO        : Gustavo Angeles Cruz
    /// FECHA_CREO  : 28-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String[] Obtener_Productos_Requisas_Filtradas()
    {
        Negocio_Consolidar = new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
        Negocio_Consolidar.P_Estatus = Txt_Estatus.Text;
        //Negocio_Consolidar.P_Estatus = "GENERADA','EN CONSTRUCCION','AUTORIZADA','FILTRADA','CONFIRMADA','SURTIDA";
        DataTable _DataTable = Negocio_Consolidar.Consultar_Productos_Requisas_Filtradas();
        String[] Arreglo = null;
        if ( _DataTable != null )
        {
            int Longitud = _DataTable.Rows.Count;
            Arreglo = new String[Longitud];
            int Indice = 0;            
            foreach ( DataRow Row in _DataTable.Rows )
            {
                Arreglo[Indice] = Row["PROD_SERV_ID"].ToString();
                Indice++;
            }
        }
        return Arreglo;
    }

    /////*******************************************************************************
    ///// NOMBRE DE LA FUNCION: Obtener_Servicios_Requisas_Filtradas
    ////  DESCRIPCIÓN: Este método obtiene un arreglo se String con todos los productos
    ////  que tienen las requisas con estatus de filtradas
    ///// PARAMETROS  : 
    ///// CREO        : Gustavo Angeles Cruz
    ///// FECHA_CREO  : 28-Diciembre-2010
    ///// MODIFICO          :
    ///// FECHA_MODIFICO    :
    ///// CAUSA_MODIFICACION:
    /////*******************************************************************************
    /////####################

    //private String[] Obtener_Servicios_Requisas_Filtradas()
    //{
    //    Negocio_Consolidar = new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
    //    Negocio_Consolidar.P_Requisas_Seleccionadas = Verifica_Requisas_Seleccionadas();
    //    Negocio_Consolidar.P_Estatus = Txt_Estatus.Text;
    //    DataTable _DataTable = Negocio_Consolidar.Consultar_Servicios_Requisas_Filtradas();
    //    String[] Arreglo = null;
    //    if (_DataTable != null)
    //    {
    //        int Longitud = _DataTable.Rows.Count;
    //        Arreglo = new String[Longitud];
    //        int Indice = 0;
    //        foreach (DataRow Row in _DataTable.Rows)
    //        {
    //            Arreglo[Indice] = Row["SERVICIO_ID"].ToString();
    //            Indice++;
    //        }
    //    }
    //    return Arreglo;
    //}


    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Requisas_Con_Productos
    //  DESCRIPCIÓN: Este método obtiene un DataTable con dos columnas, el ID de las
    //  las requisas y el el id de los productos    
    /// PARAMETROS  : 
    /// CREO        : Gustavo Angeles Cruz
    /// FECHA_CREO  : 28-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Obtener_Requisas_Con_Productos()
    {
        Negocio_Consolidar = new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
        //Negocio_Consolidar.P_Requisas_Seleccionadas = Verifica_Requisas_Seleccionadas();
        Negocio_Consolidar.P_Estatus = Txt_Estatus.Text;
        //Negocio_Consolidar.P_Estatus = "GENERADA','EN CONSTRUCCION','AUTORIZADA','FILTRADA','CONFIRMADA','SURTIDA";
        DataTable _DataTable = Negocio_Consolidar.Obtener_Requisas_Con_Productos();
        return _DataTable;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Requisas_Con_Servicios
    //  DESCRIPCIÓN: Este método obtiene un DataTable con dos columnas, el ID de las
    //  las requisas y el el id de los productos    
    /// PARAMETROS  : 
    /// CREO        : Gustavo Angeles Cruz
    /// FECHA_CREO  : 28-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Obtener_Requisas_Con_Servicios()
    {
        Negocio_Consolidar = new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
        Negocio_Consolidar.P_Requisas_Seleccionadas = Verifica_Requisas_Seleccionadas();
        Negocio_Consolidar.P_Estatus = Txt_Estatus.Text;
        DataTable _DataTable = Negocio_Consolidar.Obtener_Requisas_Con_Servicios();
        return _DataTable;
    }
    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Llenar_Combos_Generales()
    // DESCRIPCIÓN: Llena los combos principales de la interfaz de usuario
    // PARAMETROS: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: Diciembre/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Combos_Generales()
    {
        if (Cmb_Tipo_Articulo.Items.Count == 0)
        {
            Cmb_Tipo_Articulo.Items.Add("PRODUCTO");
            Cmb_Tipo_Articulo.Items.Add("SERVICIO");
            Cmb_Tipo_Articulo.Items[0].Selected = true;
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Obtener_Requisas_Que_Coinciden_Con_Cada_Producto_Servicio
    /// DESCRIPCIÓN: Obtiene un String separado por comas con las Requisas que 
    /// coinciden con el producto o servicio que se pasa por parametro
    /// PARAMETROS: String Tipo, String ID, DataTable Dt_Productos_Servicios donde se 
    /// obtendra el listado de Requisas
    /// CREO: Gustavo Angeles Cruz
    /// FECHA_CREO: Enero/2010 
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///********************************************************************************/
    private String Obtener_Requisas_Que_Coinciden_Con_Cada_Producto_Servicio(String ID, DataTable Dt_Productos_Servicios ) 
    {
        String Requisas = "";
        DataRow[] Registros = null;
        Registros = Dt_Productos_Servicios.Select("PROD_SERV_ID='" + ID + "'");
        foreach( DataRow Row in Registros )
        {
            Requisas = Requisas + Row["NO_REQUISICION"].ToString() + ",";
        }
        if (Requisas.Length > 0)
        {
            Requisas = Requisas.Substring(0, Requisas.Length - 1);
        }
        return Requisas;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: 
    /// DESCRIPCIÓN: De un String separado por comas lo convierte en arreglo y quita 
    /// campos repetidos
    /// PARAMETROS: String separado por comas con las requisas
    /// CREO: Gustavo Angeles Cruz
    /// FECHA_CREO: Enero/2010 
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///********************************************************************************/
    private String Eliminar_Requisas_Repetidas(String Requisas)
    {
        //List<String> Lista = new List<String>();
        char[] ch = { ',' };
        String[] Split_Requisas = Requisas.Split(ch);
        String Respuesta = "";
        for (int i = 0; i < Split_Requisas.Length; i++)
        {
            if (!Respuesta.Contains(Split_Requisas[i]))
            {
                Respuesta += Split_Requisas[i] + ",";
            }
        }
        if (Respuesta.Length > 0)
        {
            Respuesta = Respuesta.Substring(0, Respuesta.Length - 1);
        }
        return Respuesta;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Comparar_Arreglo
    /// DESCRIPCIÓN: De un Arreglo de String's separados por coma y que contienen 
    /// numeros de Requisas, busca las similitudes entre cada valor de los registros
    /// del arreglo y los une si es que  coinciden
    /// PARAMETROS: Arreglo de String con Requisas
    /// CREO: Gustavo Angeles Cruz
    /// FECHA_CREO: Enero/2010 
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///********************************************************************************/
    private String[] Comparar_Arreglo(String[]Requisas) 
    {        
        char[] ch = { ',' };
        for (int i = 0; i < Requisas.Length; i++)
        {
            String[] Split_Requisas = Requisas[i].Split(ch);
            for (int j = 0; j < Split_Requisas.Length; j++ )
            {
                for (int k = 0; k < Requisas.Length; k++)
                {
                    if ( k != i )
                    {
                        if( Requisas[k].Contains(Split_Requisas[j]) )
                        {
                            Requisas[i] = Requisas[i] + "," + Requisas[k];
                            Requisas[k] = "";
                        }
                    }
                }
            }
        }

        //Quitar repetidas de cada registro
        int contador = 0;
        for ( int i = 0; i < Requisas.Length; i++ )
        {
            if (Requisas[i].Length > 0)
            {
                contador++;
                Requisas[i] = Eliminar_Requisas_Repetidas(Requisas[i]);
            }
        }
        String [] Arreglo = new String[contador];
        contador = 0;
        //Crear Lista con las requisas unifcadas
        for (int i = 0; i < Requisas.Length; i++)
        {
            if (Requisas[i].Length > 0)
            {
                Arreglo[contador] = Requisas[i];
                contador++;
            }
        }        
        return Arreglo;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Marcar_Requisas_Similares
    /// DESCRIPCIÓN: Agrega una marca, es decir un número a los grupos que se generaron
    /// de las requisas despues de haberse comparado
    /// PARAMETROS: Arreglo de Requisas y el datatable que mostrara el grid
    /// CREO: Gustavo Angeles Cruz
    /// FECHA_CREO: Enero/2010 
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///********************************************************************************/
    private DataTable Marcar_Requisas_Similares(String[] Requisas_Agrupadas, DataTable Dt_Requisas) 
    {
        char[] ch = {','};        
        for (int i = 0; i < Requisas_Agrupadas.Length; i++)
        {
            String[] Split_Requisas = Requisas_Agrupadas[i].Split(ch);
            for (int j = 0; j < Split_Requisas.Length; j++)
            {
                foreach( DataRow Row in Dt_Requisas.Rows )
                {
                    if (Row["NO_REQUISICION"].ToString() == Split_Requisas[j])
                    {
                        Row["GRUPO"] = (i + 1 + "");
                        break;
                    }
                }
            }
        }
        return Dt_Requisas;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Comparar_Productos_De_Requisas
    /// DESCRIPCIÓN: Este método obtiene los productos y servicios de las requisas
    /// seleccionadas y de este modo ayudandose de metos qu ebuscan coincidencias
    /// las compara y las une segun sean parecidas en los productos que contienen
    /// y los devuelve en un Aregglo de Strings
    /// PARAMETROS: 
    /// CREO: Gustavo Angeles Cruz
    /// FECHA_CREO: Enero/2010 
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///********************************************************************************/
   private String [] Comparar_Productos_De_Requisas()
   {
        //Obtener todos los productos que tienen las requisas filtradas
        String[] Productos = Obtener_Productos_Requisas_Filtradas();
        //String[] Servicios = Obtener_Servicios_Requisas_Filtradas();
        int longitud = 0;
        if (Productos != null)
            longitud += Productos.Length;
        //if (Servicios != null)
        //    longitud += Servicios.Length;
        //String[] Requisas = new String[Productos.Length + Servicios.Length];
        String[] Requisas = new String[longitud];
        //Obtener cada productos con su respectiva requisa
        DataTable Dt_Articulos = Obtener_Requisas_Con_Productos();
        int indice_requisas = 0;
        if (Productos != null)
        {
            for (int i = 0; i < Productos.Length; i++)
            {
                Requisas[indice_requisas] = Obtener_Requisas_Que_Coinciden_Con_Cada_Producto_Servicio(Productos[i], Dt_Articulos);
                indice_requisas++;
            }
        }
        String[] Requisas_Agrupadas = Comparar_Arreglo(Requisas);
        return Requisas_Agrupadas;
   }

   ///*******************************************************************************
   /// NOMBRE DE LA FUNCIÓN: Unir_Data_Tables
   /// DESCRIPCIÓN: Sirve para unir Dos DataTable con informacion ya sea que tengan 
   /// los mismos campos o no
   /// PARAMETROS: Dos DataTable qu eseran unidos
   /// CREO: Gustavo Angeles Cruz
   /// FECHA_CREO: Enero/2010 
   /// MODIFICO:
   /// FECHA_MODIFICO:
   /// CAUSA_MODIFICACIÓN:
   ///********************************************************************************/
   public DataTable Unir_Data_Tables(DataTable Dt_Uno, DataTable Dt_Dos)
   {
       DataTable Dt_Union = new DataTable();
       //Construyo columnas
       DataColumn[] Columnas = new DataColumn[Dt_Uno.Columns.Count];
       for (int i = 0; i < Dt_Uno.Columns.Count; i++)
       {
           Columnas[i] = new DataColumn(Dt_Uno.Columns[i].ColumnName, Dt_Uno.Columns[i].DataType);
       }
       //Agrego las columnas a las tabla
       Dt_Union.Columns.AddRange(Columnas);
       Dt_Union.BeginLoadData();
       //Cargo los datos de la primera tabla
       foreach (DataRow row in Dt_Uno.Rows)
       {
           Dt_Union.LoadDataRow(row.ItemArray, true);
       }
       //Cargo los datos de la segunda tabla
       foreach (DataRow row in Dt_Dos.Rows)
       {
           Dt_Union.LoadDataRow(row.ItemArray, true);
       }
       Dt_Union.EndLoadData();
       return Dt_Union;
   }

   ///*******************************************************************************
   ///NOMBRE DE LA FUNCIÓN: Guardar_Consolidacion
   ///DESCRIPCIÓN: Hace la insercion de una consolidacion con sus detalles y 
   ///la actualizacion a las requisiciones marcandolas como consolidadas a traves
   ///de la clse de Negocio y Datos
   ///PARAMETROS: 
   ///CREO: Gustavo Angeles Cruz
   ///FECHA_CREO: 10/Enero/2011
   ///MODIFICO:
   ///FECHA_MODIFICO
   ///CAUSA_MODIFICACIÓN
   ///*******************************************************************************
   public void Guardar_Consolidacion() 
   {
       int indice = Presidencia.Consolidar_Requisicion.Datos.Cls_Ope_Com_Consolidar_Requisicion_Datos.Obtener_Consecutivo("NO_CONSOLIDACION", "OPE_COM_CONSOLIDACIONES");
       Negocio_Consolidar = new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
       Negocio_Consolidar.P_Monto = double.Parse(Txt_Total.Text);
       //Negocio_Consolidar.P_Dt_Detalles_Consolidacion = P_Dt_Detalles_Consolidacion;
       Negocio_Consolidar.P_Tipo_Articulo = Cmb_Tipo_Articulo.SelectedValue;
       Negocio_Consolidar.P_Usuario = Cls_Sessiones.Nombre_Empleado;
       Negocio_Consolidar.P_Requisas_Seleccionadas = P_Requisas_Seleccionadas;// Verifica_Requisas_Seleccionadas();
       try
       {
           Negocio_Consolidar.Guardar_Consolidacion();
           String Mensaje = "La Consolidación fue registrada";
           ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
           Llenar_Grid_Requisiciones_Filtradas();
           Grid_Requisas_Consolidadas.DataSource = null;
           Grid_Requisas_Consolidadas.DataBind();
           Txt_Num_Consolidacion.Text = "";
           Txt_Total.Text = "";
           Btn_Guardar.Visible = false;
       }
       catch (Exception ex) 
       {
           Mostrar_Informacion(ex.ToString(),true);
       }
   }
   ///*******************************************************************************
   ///NOMBRE DE LA FUNCIÓN: Actualizar_Consolidacion
   ///DESCRIPCIÓN: Hace la insercion de una consolidacion con sus detalles y 
   ///la actualizacion a las requisiciones marcandolas como consolidadas a traves
   ///de la clse de Negocio y Datos
   ///PARAMETROS: 
   ///CREO: Gustavo Angeles Cruz
   ///FECHA_CREO: 10/Enero/2011
   ///MODIFICO:
   ///FECHA_MODIFICO
   ///CAUSA_MODIFICACIÓN
   ///*******************************************************************************
   public void Actualizar_Consolidacion()
   {
       //int indice = Presidencia.Consolidar_Requisicion.Datos.Cls_Ope_Com_Consolidar_Requisicion_Datos.Obtener_Consecutivo("NO_CONSOLIDACION", "OPE_COM_CONSOLIDACIONES");
       Negocio_Consolidar = new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
       Negocio_Consolidar.P_Monto = double.Parse(Txt_Total.Text);
       //Negocio_Consolidar.P_Dt_Detalles_Consolidacion = Dt_Detalles_Consolidacion;
       Negocio_Consolidar.P_Usuario = Cls_Sessiones.Nombre_Empleado;
       Negocio_Consolidar.P_Requisas_Seleccionadas = P_Requisas_Seleccionadas;// Verifica_Requisas_Seleccionadas();
       Negocio_Consolidar.P_No_Consolidacion = Parametro;
       try
       {
           Negocio_Consolidar.Actualizar_Consolidacion();
           String Mensaje = "La Consolidación fue modificada";
           ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
           Llenar_Grid_Requisiciones_Filtradas();
           Grid_Requisas_Consolidadas.DataSource = null;
           Grid_Requisas_Consolidadas.DataBind();
           Txt_Num_Consolidacion.Text = "";
           Txt_Total.Text = "";
           Btn_Guardar.ToolTip = "Guardar Consolidación";
           Btn_Guardar.Visible = false;
           Manejo_Controles(INICIAL);
       }
       catch (Exception ex)
       {
           Mostrar_Informacion(ex.ToString(), true);
       }
   }

   private void Modo_Modificar()
   {
       Manejo_Controles(MODIFICAR);
       Negocio_Consolidar = new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
       Negocio_Consolidar.P_Folio = "CN-" + Parametro;
       DataTable Dt_Consolidacion = Negocio_Consolidar.Consultar_Consolidaciones();
       if (Dt_Consolidacion != null && Dt_Consolidacion.Rows.Count > 0)
       {
           Txt_Num_Consolidacion.Text = Dt_Consolidacion.Rows[0]["FOLIO"].ToString();
           Txt_Total.Text = "$ " + Dt_Consolidacion.Rows[0]["MONTO"].ToString();
           Negocio_Consolidar.P_Requisas_Seleccionadas = Dt_Consolidacion.Rows[0]["LISTA_REQUISICIONES"].ToString();

           DataTable Dt_Tmp = Negocio_Consolidar.Consultar_Requisiciones_Consolidacion();
           //DataTable Dt_Union = null;
           if (P_Dt_Requisiciones != null && P_Dt_Requisiciones.Rows.Count > 0 && Dt_Tmp != null && Dt_Tmp.Rows.Count > 0)
           {
               P_Dt_Requisiciones = Unir_Data_Tables(Dt_Tmp, P_Dt_Requisiciones);
           }
           else
           {
               if (Dt_Tmp != null && Dt_Tmp.Rows.Count > 0)
               {
                   P_Dt_Requisiciones = Dt_Tmp;
               }
           }
           Grid_Requisiciones.DataSource = P_Dt_Requisiciones;//_
           Grid_Requisiciones.DataBind();

           int Contador = 0;
           foreach (DataRow Renglon in P_Dt_Requisiciones.Rows)
           {
               if (Renglon["GRUPO"].ToString() == "CN")
               {
                   GridViewRow Renglon_Grid = Grid_Requisiciones.Rows[Contador];
                   ((System.Web.UI.WebControls.CheckBox)Renglon_Grid.FindControl("Chk_Requisa")).Checked = true;
               }
               Contador++;
           }
           Evento_Boton_Consolidar();
       }
   }

   private void Manejo_Controles(String modo) 
   {
       switch(modo)
       {
           case INICIAL:
               Btn_Guardar.ToolTip = "Guardar Consolidación";
               Btn_Ver_Consolidaciones.Visible = true;
               Btn_Guardar.Visible = false;
               Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
               Btn_Salir.ToolTip = "Inicio";
               int No_Consolidacion =
                   Presidencia.Consolidar_Requisicion.Datos.
                   Cls_Ope_Com_Consolidar_Requisicion_Datos.
                   Obtener_Consecutivo(Ope_Com_Consolidaciones.
                   Campo_No_Consolidacion, Ope_Com_Consolidaciones.Tabla_Ope_Com_Consolidaciones);
               Txt_Num_Consolidacion.Text = "CN-" + No_Consolidacion;
               Txt_Total.Text = "0.0";
               Cmb_Tipo_Articulo.Enabled = true;
               break;
           case NUEVO:
               Btn_Ver_Consolidaciones.Visible = false;
               Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
               Btn_Salir.ToolTip = "Cancelar";
               Cmb_Tipo_Articulo.Enabled = true;
               break;
           case MODIFICAR:
               Btn_Guardar.ToolTip = "Actualizar Consolidación";
               Btn_Guardar.Visible = false;
               Cmb_Tipo_Articulo.Enabled = false;
               Btn_Ver_Consolidaciones.Visible = false;
               Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
               Btn_Salir.ToolTip = "Cancelar";
               break;
       }
   }
    #endregion

}
