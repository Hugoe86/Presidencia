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
using Presidencia.Kardex_Productos.Negocio;
using Presidencia.Reportes;
using Presidencia.Sessiones;

public partial class paginas_Almacen_Frm_Ope_Alm_Kardex_Productos : System.Web.UI.Page
{

    #region Variables Globales
    Cls_Ope_Alm_Kardex_Productos_Negocio Kardex_Productos;
    #endregion

    #region Evento Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if(! IsPostBack)
        Estatus_Inicial_Busqueda();
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Fecha_B_CheckedChanged
    ///DESCRIPCIÓN:          Evento que se ejecuta cuando se da clic en el CheckBox
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           16/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Fecha_B_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Fecha_B.Checked == true)
        {
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");

            Img_Btn_Fecha_Inicio.Enabled = true;
            Img_Btn_Fecha_Fin.Enabled = true;
        }
        else
        {
            Txt_Fecha_Inicio.Text = "";
            Txt_Fecha_Fin.Text = "";
            Img_Btn_Fecha_Inicio.Enabled = false;
            Img_Btn_Fecha_Fin.Enabled = false;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Fecha_B_CheckedChanged
    ///DESCRIPCIÓN:          Evento utilizado para buscar el producto seleccionado por el usuario
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           16/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Kardex_Productos = new Cls_Ope_Alm_Kardex_Productos_Negocio();
        DataTable DT_Entradas_Productos = new DataTable();
        DataTable DT_Detalles_Productos = new DataTable();
        DataTable DT_Salidas_Productos = new DataTable();

        try
        { 
            if (Txt_Clave.Text.Trim() != "")
                Kardex_Productos.P_Clave_B = Txt_Clave.Text.Trim();
            else
                Kardex_Productos.P_Clave_B = null;

            if (Chk_Fecha_B.Checked) // Si esta activado el Check
            {
                DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
                DateTime Date2 = new DateTime();

                if ((Txt_Fecha_Inicio.Text.Length != 0))
                {
                    if ((Txt_Fecha_Inicio.Text.Length == 11) && (Txt_Fecha_Fin.Text.Length == 11))
                    {
                        //Convertimos el Texto de los TextBox fecha a dateTime
                        Date1 = DateTime.Parse(Txt_Fecha_Inicio.Text);
                        Date2 = DateTime.Parse(Txt_Fecha_Fin.Text);

                        //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                        if ((Date1 < Date2) | (Date1 == Date2))
                        {
                            if (Txt_Fecha_Fin.Text.Length != 0)
                            {
                                //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                Kardex_Productos.P_Fecha_Inicio_B = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                                Kardex_Productos.P_Fecha_Fin_B = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                                Div_Contenedor_Msj_Error.Visible = false;
                            }
                            else
                            {
                                String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                Kardex_Productos.P_Fecha_Inicio_B = Fecha;
                                Kardex_Productos.P_Fecha_Fin_B = Fecha;
                                Div_Contenedor_Msj_Error.Visible = false;
                            }
                        }
                        else
                        {
                            Lbl_Informacion.Text = " Fecha no valida ";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                    else
                    {
                        Lbl_Informacion.Text = " Fecha no valida ";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }

            DT_Entradas_Productos = Kardex_Productos.Consultar_Entradas_Productos();  // Se consultan las entradas de el producto
            DT_Salidas_Productos = Kardex_Productos.Consultar_Salidas_Productos();    // Se consultan las salidas de el producto

            // Se agrega nuevamente la clave
            if (Txt_Clave.Text.Trim() != "")
                Kardex_Productos.P_Clave_B = Txt_Clave.Text.Trim();
            else
                Kardex_Productos.P_Clave_B = null;

            DT_Detalles_Productos = Kardex_Productos.Consultar_Datos_Producto(); // Se consultan los datos generales del producto

            if (DT_Detalles_Productos.Rows.Count > 0)
            {
                Session["DT_DETALLES_PRODUCTOS"] = DT_Detalles_Productos;
                
                Txt_Producto.Text = "" + DT_Detalles_Productos.Rows[0]["PRODUCTO"].ToString().Trim();
                Txt_Descripcion.Text = "" + DT_Detalles_Productos.Rows[0]["DESCRIPCION"].ToString().Trim();
                Txt_Clave_Producto.Text = "" + DT_Detalles_Productos.Rows[0]["CLAVE"].ToString().Trim();
                Txt_Estatus.Text = "" + DT_Detalles_Productos.Rows[0]["ESTATUS"].ToString().Trim();
                Txt_Comprometido.Text = "" + DT_Detalles_Productos.Rows[0]["COMPROMETIDO"].ToString().Trim();
                Txt_Unidad.Text = "" + DT_Detalles_Productos.Rows[0]["UNIDAD"].ToString().Trim();
                Txt_Modelo.Text = "" + DT_Detalles_Productos.Rows[0]["MODELO"].ToString().Trim();
                Txt_Marca.Text = "" + DT_Detalles_Productos.Rows[0]["MARCA"].ToString().Trim();

                Btn_Imprimir_Excel.Visible = true;
                Btn_Imprimir_PDF.Visible = true;
                Div_Detalles.Visible = true;

                if (DT_Entradas_Productos.Rows.Count > 0) // Si hay entradas del producto seelccionado
                {
                    Session["DT_ENTRADAS_PRODUCTOS"] = DT_Entradas_Productos;

                    Grid_Entradas.DataSource = DT_Entradas_Productos;
                    Grid_Entradas.DataBind();
                    Grid_Entradas.Visible = true;
                    Lbl_Entradas.Visible = true;
                    Div_Contenedor_Msj_Error.Visible = false;
                }
                else
                {
                    Grid_Entradas.Visible = false;
                    Lbl_Entradas.Visible = false;
                }


              if (DT_Salidas_Productos.Rows.Count > 0) // Si hay SALIDAS del producto seleccionado
                {
                    Session["DT_SALIDAS_PRODUCTOS"] = DT_Salidas_Productos;

                    Grid_Salidas.DataSource = DT_Salidas_Productos;
                    Grid_Salidas.DataBind();
                    Grid_Salidas.Visible = true;
                    Lbl_Salidas.Visible = true;
                    Div_Contenedor_Msj_Error.Visible = false;
                }
                else
                {
                    Grid_Salidas.Visible = false;
                    Lbl_Salidas.Visible = false;
                }
            }
            else
            {
                Lbl_Informacion.Text = " No se encontraron detalles del producto";
                Div_Contenedor_Msj_Error.Visible = true;
                Div_Detalles.Visible = false;
                Btn_Imprimir_Excel.Visible = false;
                Btn_Imprimir_PDF.Visible = false;
            }

        }
        catch (Exception ex)
        {
		    throw new Exception("Error al buscar el producto. Error: [" + ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Click
    ///DESCRIPCIÓN:          Evento utilizado establecer la configuración inicial de la página
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           16/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Estatus_Inicial_Busqueda();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_PDF_Click
    ///DESCRIPCIÓN:          Evento utilizado para llenar el dataSet para generar el reporte
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_PDF_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_DataSet_Reporte("PDF");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Excel_Click
    ///DESCRIPCIÓN:          Evento utilizado para llenar el dataSet para generar el reporte
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_DataSet_Reporte("Excel");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:          Evento utilizado para  salir de la página.
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }

    #endregion

    #region Métodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Inicial_Busqueda
    ///DESCRIPCIÓN:          Método donde se configura el estatus inicial de la búsqueda.
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Estatus_Inicial_Busqueda()
    {
        Txt_Fecha_Fin.Text = "";
        Txt_Fecha_Inicio.Text = "";
        Txt_Clave.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {
        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica a mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        // Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:          Método utilizado para  llenar las tablas que formaran para genera el reporte
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_DataSet_Reporte(String Formato)
    {
        DataTable Dt_Cabecera = new DataTable();
        DataTable Dt_Detalles_Entradas = new DataTable();
        DataTable Dt_Detalles_Salidas = new DataTable();

        try
        { 
            if (Session["DT_SALIDAS_PRODUCTOS"] != null)
                Dt_Detalles_Salidas = (DataTable)Session["DT_SALIDAS_PRODUCTOS"];

            if (Session["DT_ENTRADAS_PRODUCTOS"] != null)
                Dt_Detalles_Entradas = (DataTable)Session["DT_ENTRADAS_PRODUCTOS"];

            if (Session["DT_DETALLES_PRODUCTOS"] != null)
                Dt_Cabecera = (DataTable)Session["DT_DETALLES_PRODUCTOS"];

            Ds_Alm_Com_Kardex_Productos Ds_Kardes = new Ds_Alm_Com_Kardex_Productos();
            Generar_Reporte(Dt_Detalles_Entradas, Dt_Detalles_Salidas, Dt_Cabecera, Ds_Kardes, Formato); // Se genera el reporte
        }
        catch (Exception ex)
        {
            throw new Exception("Error al llenar el DataSet. Error: [" + ex.Message + "]");
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.- Dt_Detalles_Entradas.- Contiene los datos generales de las entradas del producto
    ///                      2.- Dt_Detalles_Salidas - Contiene los datos generales de las salidas del producto
    ///                      2.- Dt_Cabecera.- Contiene los documentos del contra recibo
    ///                      3.- Ds_Kardes.- Instancia del dataset creado para el llenado del repote 
    ///                      4.- Formato.- Contiene el nombre del documento que se va a generar
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Reporte(DataTable Dt_Detalles_Entradas, DataTable Dt_Detalles_Salidas, DataTable Dt_Cabecera, DataSet Ds_Kardes, String Formato)
    {
        DataRow Renglon;
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";

        try
        {
            // Llenar la tabla "Cabecera" del Dataset
            Renglon = Dt_Cabecera.Rows[0];
            Ds_Kardes.Tables[0].ImportRow(Renglon);
            Ds_Kardes.Tables[0].Rows[0].SetField("USUARIO_ELABORO", Cls_Sessiones.Datos_Empleado.ToString().Trim());
            Ds_Kardes.Tables[0].Rows[0].SetField("REGISTRO_ID", "0");

            if (Dt_Detalles_Entradas.Rows.Count > 0)
            {
                // Llenar las entradas en el DataSet
                for (int Cont_Elementos = 0; Cont_Elementos < Dt_Detalles_Entradas.Rows.Count; Cont_Elementos++)
                {
                    // Instanciar renglon e importarlo
                    Renglon = Dt_Detalles_Entradas.Rows[Cont_Elementos];
                    Ds_Kardes.Tables[1].ImportRow(Renglon);
                    Ds_Kardes.Tables[1].Rows[Cont_Elementos].SetField("REGISTRO_ID", "0");
                }
            }

            if (Dt_Detalles_Salidas.Rows.Count > 0)
            {
                // Agregar las salidas al DataSet
                for (int Elementos = 0; Elementos < Dt_Detalles_Salidas.Rows.Count; Elementos++)
                {
                    // Instanciar renglon e importarlo
                    Renglon = Dt_Detalles_Salidas.Rows[Elementos];
                    Ds_Kardes.Tables[2].ImportRow(Renglon);
                    Ds_Kardes.Tables[2].Rows[Elementos].SetField("REGISTRO_ID", "0");
                }
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Kardex_Productos.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Kardex_Productos" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se crea el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Report = new Cls_Reportes();
            Report.Generar_Reporte(ref Ds_Kardes, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + ex.Message + "]");
        }
    }


    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

      try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion





  
   


    


    
}
