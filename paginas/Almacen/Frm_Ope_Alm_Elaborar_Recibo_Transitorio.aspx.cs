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
using Presidencia.Almacen_Elaborar_Recibo_Transitorio.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Reportes;
using Presidencia.Requisiciones_Stock.Negocio;

public partial class paginas_Almacen_Frm_Ope_Alm_Elaborar_Recibo_Transitorio : System.Web.UI.Page
{
    #region Variables
    Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Recibo_Transitorio = new Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio();
    #endregion

    # region  Load

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
       
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

        if (!IsPostBack)
        {
                Estatus_Inicial();
                Btn_Generar_Recibo_T.Visible = false;
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:          Evento que configura algunos de los componentes a su estatus inicial
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           25/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Salir")
        {
            Recibo_Transitorio = new Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio();
            Estatus_Inicial();
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Estatus_Inicial();
        }
    }

    protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
    {
        try
        {
            Estado_Inicial_Busqueda_Avanzada();
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Text = "Error: (Btn_Busqueda_Avanzada_Click)" + ex.ToString();
            Lbl_Informacion.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN:          Evento utilizado para realizar una consulta simple
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           11/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Consultar_Ordenes_Compra();
    }
    

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Seleccionar_Orden_Compra_Click
    ///DESCRIPCIÓN:          Evento del botón con la imagen que se encuentra en el grid "Grid_Ordenes_Compra"
    ///                      Este evento es utilizado para mostrar los detalles de la orden de compra
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           05/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Seleccionar_Orden_Compra_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton Btn_Seleccionar_Orden_C = null;
        DataTable Dt_Ordenes_Compra = new DataTable();
        String No_Orden_Compra =String.Empty;
        String Proveedor_Id = String.Empty;
        String Folio = String.Empty;
        String Total = String.Empty;
        String Fecha_Construccion = String.Empty;
        String Proveedor = String.Empty;
        String Estatus = String.Empty;
        String No_Contra_Recibo = String.Empty;
        DataRow[] Dr_Orden_Compra;

        try 
        {
            Btn_Seleccionar_Orden_C = (ImageButton)sender;
            No_Orden_Compra = Btn_Seleccionar_Orden_C.CommandArgument;
            Session["No_Orden_Compra_RT"] = No_Orden_Compra;

            if (Session["Dt_Ordenes_Compra_RT"] != null)
            {
                Dt_Ordenes_Compra = (DataTable)Session["Dt_Ordenes_Compra_RT"];
            }

            if (Dt_Ordenes_Compra is DataTable)
            {
                Div_Busqueda_Av.Visible = false;

                if (Dt_Ordenes_Compra.Rows.Count > 0)
                {
                    Dr_Orden_Compra = Dt_Ordenes_Compra.Select("NO_ORDEN_COMPRA='" + No_Orden_Compra + "'");

                    if (Dr_Orden_Compra.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["PROVEEDOR_ID"].ToString()))
                            Proveedor_Id = Dr_Orden_Compra[0]["PROVEEDOR_ID"].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["FOLIO"].ToString()))
                            Folio = Dr_Orden_Compra[0]["FOLIO"].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["FECHA"].ToString()))
                            Fecha_Construccion = Dr_Orden_Compra[0]["FECHA"].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["PROVEEDOR"].ToString()))
                            //Proveedor = Dr_Orden_Compra[0]["PROVEEDOR"].ToString().Trim();
                            Proveedor = HttpUtility.HtmlDecode(Dr_Orden_Compra[0]["PROVEEDOR"].ToString().Trim());
                        if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["ESTATUS"].ToString()))
                            Estatus = Dr_Orden_Compra[0]["ESTATUS"].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["ESTATUS"].ToString()))
                            No_Contra_Recibo = Dr_Orden_Compra[0]["NO_CONTRA_RECIBO"].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["ESTATUS"].ToString()))
                            Total = Dr_Orden_Compra[0]["TOTAL"].ToString().Trim();

                       Mostrar_Productos_Ordenes_Compra(Folio, Fecha_Construccion, Proveedor, Estatus, No_Contra_Recibo, Total);
                       Session["Dt_Serie_Productos"] = null;  // Cada que se da un clic sobre el grid, se inicializa la variable que contiene los productos serializados en null
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un producto de la tabla de productos. Error: [" + Ex.Message + "]");
        }
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Chk_Fecha_B_CheckedChanged
    ///DESCRIPCION:             Evento utilizado para habilitar o deshabilitar los botones
    ///                         utilizados para asignar la fecha inicio y facha final.
    ///PARAMETROS:              
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              11/Abril/2011 
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
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

    protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Numero_Empleado.Text.Trim() != "")
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Llenar_Combo_Empleado(Txt_Numero_Empleado.Text.Trim());
        }
        else
        {
            Lbl_Informacion.Text = "Asignar el Número de Empleado a Consultar";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Llenar_Combo_Empleado
    /// DESCRIPCION:            Método utilizado para seleccionar del combo el numero de empleado a consultar                 
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            23/Junio/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Llenar_Combo_Empleado(String No_Empleado)
    {
        Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio Consulta_Requisiciones = new Cls_Ope_Com_Alm_Requisiciones_Stock_Negocio();
        DataTable Dt_Empleados_UR = new DataTable();

        try
        {
            Cmb_Responsable.DataSource = Dt_Empleados_UR; // Limpia el Combo
            Cmb_Responsable.DataBind();

            Consulta_Requisiciones.P_No_Empleado = No_Empleado.Trim();
            Dt_Empleados_UR = Consulta_Requisiciones.Consultar_Empleado();
            if (Dt_Empleados_UR.Rows.Count > 0)
            {
                Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Responsable,Dt_Empleados_UR,1,0);
                if (Cmb_Responsable.Items.Count > 1)
                    Cmb_Responsable.SelectedIndex = 1;
                // Se le agrega un ToolTip a cada elemento del combo, ya que los valores estan muy grandes.
                if (Cmb_Responsable != null)
                    foreach (ListItem li in Cmb_Responsable.Items)
                        li.Attributes.Add("title", li.Text);

                Div_Contenedor_Msj_Error.Visible = false;
            }
            else
            {
                Lbl_Informacion.Text = "No se Encontró el Empleado con el Número Asignado";
                Div_Contenedor_Msj_Error.Visible = true;

            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

      #region Grid
        
       # endregion

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Inicial
    ///DESCRIPCIÓN:          Método utilizado para configurar inicialmente algunos de los componentes de la página
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           24/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Estatus_Inicial()
    {
        Consultar_Ordenes_Compra();
        Div_Busqueda_Av.Visible = true;
        Txt_Factura.Text = "";
        Txt_Fecha_Surtido.Text = "";
        Txt_Proveedor.Text = "";
        Txt_Orden_Compra.Text = "";
        Txt_Importe.Text = "";
        Txt_Unidad_Responsable.Text = "";
        Txt_Requisicion.Text = "";
        Estatus_Inicial_Botones(false);
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Inicial_Botones
    ///DESCRIPCIÓN:          Método utilizado asignarle el estatus correspondiente a los botones
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           24/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Estatus_Inicial_Botones(Boolean Estatus)
    {
        Btn_Generar_Recibo_T.Visible = Estatus;

        if (Estatus)
        {
            Btn_Salir.AlternateText = "Atras";
            Btn_Salir.ToolTip = "Atras";
            Mostrar_Busqueda(false);
        }
        else
        {
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ToolTip = "Salir";
            Mostrar_Busqueda(true);
        }

        if (Btn_Generar_Recibo_T.Visible) // Si  el boton esta visible, verifica si el usuario tiene acceso a el
        {
            Configuracion_Acceso("Frm_Ope_Alm_Elaborar_Recibo_Transitorio.aspx");
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Busqueda
    ///DESCRIPCIÓN:          Método utilizado para mostrar y ocultar los controles
    ///                      utilizados para realizar la búsqueda simple y abanzada
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           12/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Mostrar_Busqueda(Boolean Estatus)
    {
        Txt_Busqueda.Visible = Estatus;
        Btn_Buscar.Visible = Estatus;
    }

    public void Llenar_Combo_Empleados(String Unidad_Responsable_ID)
    {
        Recibo_Transitorio = new Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio();
        DataTable Dt_Empleados = new DataTable();

        Recibo_Transitorio.P_Unidad_Responsable_ID = Unidad_Responsable_ID;
        Dt_Empleados = Recibo_Transitorio.Consulta_Empleados_Almacen();

        if (Dt_Empleados.Rows.Count > 0)
        {
            // Se agrega la fila a la tabla
            DataRow Fila_Empleados = Dt_Empleados.NewRow();
            Fila_Empleados["EMPLEADO_ID"] = "SELECCIONE";
            Fila_Empleados["EMPLEADO"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Dt_Empleados.Rows.InsertAt(Fila_Empleados, 0);
            Cmb_Responsable.DataSource = Dt_Empleados; // Se agrega la información al combo
            Cmb_Responsable.DataValueField = "EMPLEADO_ID";
            Cmb_Responsable.DataTextField = "EMPLEADO";
            Cmb_Responsable.DataBind();
            Cmb_Responsable.SelectedIndex = 0; // Se selecciona el indice 0

            // Se le agrega un ToolTip a cada elemento del combo, ya que los valores estan muy grandes.
            if (Cmb_Responsable != null)
                foreach (ListItem li in Cmb_Responsable.Items)
                    li.Attributes.Add("title", li.Text); 
        }
   }
    

    

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Detalles_Productos
    ///DESCRIPCIÓN:          Método utilizado para llenar el grid con los datos de cada producto
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           24/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Llenar_Grid_Detalles_Productos(String Producto_Id, String Nombre_Producto, String Clave_Producto, Int16 Cantidad_Productos)
    {
        String No_Orden_Compra = "";
        DataTable Dt_Detalles_Productos = new DataTable();  // Se crea la tabla 

        try
          {
            if (Session["No_Orden_Compra_RT"]!= null)
            No_Orden_Compra = Session["No_Orden_Compra_RT"].ToString();

            Dt_Detalles_Productos.Columns.Add("PRODUCTO_ID");
            Dt_Detalles_Productos.Columns.Add("DESCRIPCION");
            Dt_Detalles_Productos.Columns.Add("CLAVE");
           

        }
         catch (Exception Ex)
         {
             throw new Exception("Error al llenar el grid con los detalles del producto. Error: [" + Ex.Message + "]");
         }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Ordenes_Compra
    ///DESCRIPCIÓN:          Método utilizado para consultar las Ordenes de Compra
    ///                      con estatus "SURTIDA" y que no tengan recibo transitorio
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           24/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Consultar_Ordenes_Compra()
    {
        DataTable Dt_Ordenes_compra = new DataTable();
      

        try
        {
            if (Txt_Busqueda.Text.Trim() != "")
                Recibo_Transitorio.P_No_Orden_Compra = Txt_Busqueda.Text.Trim();
            else
                Recibo_Transitorio.P_No_Orden_Compra = null;

            if (Txt_Req_Buscar.Text.Trim() != "")
                Recibo_Transitorio.P_No_Requisicion = Txt_Req_Buscar.Text.Trim();
            else
                Recibo_Transitorio.P_No_Requisicion = null;

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
                                Recibo_Transitorio.P_Fecha_Inicio_B = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                                Recibo_Transitorio.P_Fecha_Fin_B = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                                Div_Contenedor_Msj_Error.Visible = false;
                            }
                            else
                            {
                                String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                Recibo_Transitorio.P_Fecha_Inicio_B = Fecha;
                                Recibo_Transitorio.P_Fecha_Fin_B = Fecha;
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
            else
            {
               
            }


            Dt_Ordenes_compra = Recibo_Transitorio.Consulta_Ordenes_Compra();

            if (Dt_Ordenes_compra.Rows.Count > 0)
            {
                Grid_Ordenes_Compra.DataSource = Dt_Ordenes_compra;
                Session["Dt_Ordenes_Compra_RT"] = Dt_Ordenes_compra;
                Grid_Ordenes_Compra.Columns[7].Visible = true;
                Grid_Ordenes_Compra.Columns[8].Visible = true;
                Grid_Ordenes_Compra.Columns[9].Visible = true;
                Grid_Ordenes_Compra.DataBind();
                Grid_Ordenes_Compra.Columns[7].Visible=false;
                Grid_Ordenes_Compra.Columns[8].Visible = false;
                Grid_Ordenes_Compra.Columns[9].Visible = false;
                Div_Contenedor_Msj_Error.Visible = false;
                Div_Ordenes_Compra.Visible = true;
                Div_Detalles_Orden_Compra.Visible = false;
            }
            else
            {
                Lbl_Informacion.Text = "No se enconontraron órdenes de compra";
                Div_Contenedor_Msj_Error.Visible = true;
                Div_Ordenes_Compra.Visible = false;
                Btn_Generar_Recibo_T.Visible = false;
                Div_Detalles_Orden_Compra.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las orden de compra. Error: [" + Ex.Message + "]");
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Productos_Ordenes_Compra
    ///DESCRIPCIÓN:          En este método se consultan los productos que pertenecen a la orden de compra
    ///                      seleccionada por el usuario
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           23/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Mostrar_Productos_Ordenes_Compra(String Folio, String Fecha_Construccion, String Proveedor, String Estatus, String No_Contra_Recibo, String Total)
    {
        DataTable Dt_Productos_OC = new DataTable();
        DataTable Dt_Mostrar_Productos_OC = new DataTable();
        Boolean Mostrar_Productos = false; // Variable utilizada para indicar si la consulta arojo resultados y de esta manera se muestran los productos
        DataTable Dt_Borrar_Grid = new DataTable();
       
        Grid_Productos_RT_Unidad.DataSource = Dt_Borrar_Grid; // Se borra el grid, para que este no contenga elementos
        Grid_Productos_RT_Unidad.DataBind();

        Grid_Productos_RT_Totalidad.DataSource = Dt_Borrar_Grid; // Se borra la información del Grid
        Grid_Productos_RT_Totalidad.DataBind();

        Grid_Productos_RT_Totalidad.Visible = false; // Se ocultan el Grid
        Grid_Productos_RT_Unidad.Visible = false; // Se oculta el grid
        Lbl_Productos_Por_Totalidad.Visible = false; // Se ocultan el label
        Lbl_Productos_Por_Unidad.Visible = false;    // Se ocultan el label

        try
        {
            if (Session["No_Orden_Compra_RT"] != null)
            Recibo_Transitorio.P_No_Orden_Compra = Session["No_Orden_Compra_RT"].ToString().Trim();

            Recibo_Transitorio.P_No_Contra_Recibo = No_Contra_Recibo.Trim();
            Session["No_Contra_Recibo_RT"] = No_Contra_Recibo.Trim();

            Dt_Productos_OC = Recibo_Transitorio.Consulta_Productos_Orden_Compra();// Se consultan los productos para mostrarlos en el grid

            if (Dt_Productos_OC.Rows.Count > 0)
            {
                // Se llena el Grid
                Grid_Productos_RT_Unidad.DataSource = Dt_Productos_OC;
                Grid_Productos_RT_Unidad.Columns[0].Visible = true;
                Grid_Productos_RT_Unidad.Columns[8].Visible = true; // Se pone visible la columa No. Inventario
                Grid_Productos_RT_Unidad.DataBind();
                Grid_Productos_RT_Unidad.Columns[0].Visible = false;
                Grid_Productos_RT_Unidad.Columns[8].Visible = false; // Se oculta la columa que tre el No. Inventario
                Grid_Productos_RT_Unidad.Visible = true; // Se pone visible este grid
                Lbl_Productos_Por_Unidad.Visible = true;
                Mostrar_Productos = true;
                Session["Reporte_RT"] = "POR_UNIDAD"; // Se asigna la variable que indica que el reporte se debe imprimir por Unidad
            }
            else  // Si no se encontraron productos
            {
                // Se revisá si la orden de compra tiene productos que deben ser registrados por totalidad
                DataTable Dt_Productos_RT_Totalidad = new DataTable();
                Dt_Productos_RT_Totalidad = Recibo_Transitorio.Consulta_Productos_Requision();// Se consultan los productos de la requisición ya que no se registraron datos

                if (Dt_Productos_RT_Totalidad.Rows.Count > 0)
                {
                    // Se realiza la obtencion de los datos para determianr si existencias tiene 0, entonces se agregan
                    for (int i = 0; i < Dt_Productos_RT_Totalidad.Rows.Count; i++)
                    {
                        Int64 Cantidad_Entregada = 0;
                        Int64 Cantidad = 0;

                        if (Dt_Productos_RT_Totalidad.Rows[i]["EXISTENCIA"].ToString().Trim() != "") // Se Verifica que la cantidad entregada tenga valor
                            Cantidad_Entregada = Convert.ToInt64(Dt_Productos_RT_Totalidad.Rows[i]["EXISTENCIA"].ToString().Trim());
                        else
                            Cantidad_Entregada = 0;

                        if (Dt_Productos_RT_Totalidad.Rows[i]["CANTIDAD"].ToString().Trim() != "")
                             Cantidad = Convert.ToInt64(Dt_Productos_RT_Totalidad.Rows[i]["CANTIDAD"].ToString().Trim());

                        Int64 Existencia = (Cantidad - Cantidad_Entregada);
                        Dt_Productos_RT_Totalidad.Rows[i].SetField("EXISTENCIA", Existencia);
                    }
                    // Se llena el Grid
                    Grid_Productos_RT_Totalidad.DataSource = Dt_Productos_RT_Totalidad;
                    Grid_Productos_RT_Totalidad.Columns[0].Visible = true;
                    Grid_Productos_RT_Totalidad.DataBind();
                    Grid_Productos_RT_Totalidad.Columns[0].Visible = false;
                    Grid_Productos_RT_Totalidad.Visible = true; // Se muestra el Grid que contiene los productos por  totalidad
                    Lbl_Productos_Por_Totalidad.Visible = true;
                    Session["Reporte_RT"] = "POR_TOTALIDAD";  // Se asigna la variable que indica que el reprote se debe imprimir por Totalidad
                    Mostrar_Productos = true;
                }
            }

            if (Mostrar_Productos == true) // Si la requisición si contiene productos que deben ser mostrados, se llena el  encabezado
            {
                Estatus_Inicial_Botones(true);
                Div_Detalles_Orden_Compra.Visible = true;
                Div_Ordenes_Compra.Visible = false;
                Txt_Proveedor.Text = Proveedor;
                Txt_Orden_Compra.Text = Folio;
                Txt_Importe.Text = "" + Total;

                DateTime Fecha_Convertida = Convert.ToDateTime(Fecha_Construccion);  // Se conviente la fecha
                Txt_Fecha_Surtido.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Convertida);

                DataTable Dt_Datos_G = new DataTable();
                Dt_Datos_G = Recibo_Transitorio.Consulta_Datos_Generales();

                String Unidad_Responsable_ID = "";

                if (Dt_Datos_G.Rows.Count > 0)
                {
                    Txt_Requisicion.Text = "" + HttpUtility.HtmlDecode(Dt_Datos_G.Rows[0]["NO_REQUISICION"].ToString().Trim());
                    Txt_Unidad_Responsable.Text = "" + HttpUtility.HtmlDecode(Dt_Datos_G.Rows[0]["UNIDAD_RESPONSABLE"].ToString().Trim());
                    Unidad_Responsable_ID = "" + HttpUtility.HtmlDecode(Dt_Datos_G.Rows[0]["UNIDAD_RESPONSABLE_ID"].ToString().Trim());
                }

                Llenar_Combo_Empleados("00036");//Llenar_Combo_Empleados(Unidad_Responsable_ID);// En esta parte se realiza la busqueda de empleados
                Recibo_Transitorio.P_No_Contra_Recibo = Session["No_Contra_Recibo_RT"].ToString().Trim();
              
                DataTable Dt_Facturas = new DataTable();
                Dt_Facturas = Recibo_Transitorio.Consulta_Facturas();

                if (Dt_Facturas.Rows.Count > 0)
                {
                    Txt_Factura.Text = "" + HttpUtility.HtmlDecode(Dt_Facturas.Rows[0]["NO_FACTURA_PROVEEDOR"].ToString().Trim());
                }
            }
            else
            {
                Lbl_Informacion.Text = "No se encontrarón productos de la órdenes de compra";
                Div_Contenedor_Msj_Error.Visible = true;
                Div_Ordenes_Compra.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los productos de la orden de compra. Error: [" + Ex.Message + "]");
        }
    }

   

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.- Dt_Cabecera.- Contiene la informacion general de la orden de compra
    ///                      2.- Dt_Detalles.- Contiene los detalles que se vana mostrar en el reporte 
    ///                      3.- Ds_Recibo.- Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///                      4.- Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///                      5.- Nombre_Archivo, Es el nombre del documento que se va a generar en PDF
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           09/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataTable Dt_Cabecera, DataTable Dt_Detalles, DataSet Ds_Recibo, String Nombre_Reporte_Crystal, String Nombre_Reporte, String Formato)
    {
        DataRow Renglon;
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
       
        // Llenar la tabla "Cabecera" del Dataset
        Renglon = Dt_Cabecera.Rows[0];
        Ds_Recibo.Tables[0].ImportRow(Renglon);

        // Llenar los detalles del DataSet
        for (int Cont_Elementos = 0; Cont_Elementos < Dt_Detalles.Rows.Count; Cont_Elementos++)
        {
            Renglon = Dt_Detalles.Rows[Cont_Elementos]; //Instanciar renglon e importarlo
            Ds_Recibo.Tables[1].ImportRow(Renglon);
        }

        // Ruta donde se encuentra el reporte Crystal
        Ruta_Reporte_Crystal = "../Rpt/Almacen/" + Nombre_Reporte_Crystal;

        // Se crea el nombre del reporte
        String Nombre_Report = Nombre_Reporte + "_"+ Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

        // Se da el nombre del reporte que se va generar
        if (Formato == "PDF")
            Nombre_Reporte_Generar = Nombre_Report + ".pdf";  // Es el nombre del reporte PDF que se va a generar
        else if (Formato == "Excel")
            Nombre_Reporte_Generar = Nombre_Report + ".xls";  // Es el nombre del repote en Excel que se va a generar

        Cls_Reportes Reportes = new Cls_Reportes();
        Reportes.Generar_Reporte(ref Ds_Recibo, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
        Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
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


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Estado_Inicial_Busqueda_Avanzada
    /// DESCRIPCION:            Colocar la ventana de la busqueda avanzada en un estado inicial
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            19/Marzo/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Estado_Inicial_Busqueda_Avanzada()
    {
        try
        {
            Txt_Fecha_Inicio.Text = "";
            Txt_Fecha_Fin.Text = "";
            Chk_Fecha_B.Checked = false;
            Txt_Req_Buscar.Text = "";
            Txt_Busqueda.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN:         Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///                     en la busqueda del Modalpopupfile:///C:\Documents and Settings\gangeles\Escritorio\SIAS 08-JULIO-11\SIAS\App_Code\Ayudante\Cls_DateAndTime .cs
    ///PARAMETROS:   
    ///CREO:                Salvador Hernández Ramírez
    ///FECHA_CREO:          19/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Verificar_Fecha()
    {
        DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date2 = new DateTime();


        try
        {
            if (Chk_Fecha_B.Checked)
            {
                if ((Txt_Fecha_Inicio.Text.Length != 0))
                {
                    if ((Txt_Fecha_Inicio.Text.Length == 11) && (Txt_Fecha_Fin.Text.Length == 11))
                    {
                        //Convertimos el Texto de los TextBox fecha a dateTime
                        Date1 = DateTime.Parse(Txt_Fecha_Inicio.Text);
                        Date2 = DateTime.Parse(Txt_Fecha_Fin.Text);

                        if ((Date1 < Date2) | (Date1 == Date2)) //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                        {
                            if (Txt_Fecha_Fin.Text.Length != 0)
                            {
                                //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                Recibo_Transitorio.P_Fecha_Inicio_B = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                                Recibo_Transitorio.P_Fecha_Fin_B = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                            }
                            else
                            {
                                String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                Recibo_Transitorio.P_Fecha_Inicio_B = Fecha;
                                Recibo_Transitorio.P_Fecha_Fin_B = Fecha;
                            }
                        }
                        else
                        {
                            Lbl_Informacion.Text = " Fecha no valida ";
                        }
                    }
                    else
                    {
                        Lbl_Informacion.Text = " Fecha no valida ";
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:     Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:      1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                     en caso de que cumpla la condicion del if
    ///CREO:            Salvador Hernández Ramírez
    ///FECHA_CREO:      19/Marzo/2011 
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
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }

    #endregion

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Generar_Recibo_T);
            Botones.Add(Btn_Buscar);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]); // Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }


    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso_LinkButton(String URL_Pagina)
    {
        List<LinkButton> Botones = new List<LinkButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            ////Agregamos los botones a la lista de botones de la página.
            //Botones.Add(Btn_Busqueda_Avanzada);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numero(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion


    protected void Btn_Generar_Recibo_T_Click(object sender, ImageClickEventArgs e)
    {
        String Tipo=""; // Guardara el tipo de recibo transitorio que se genera " Por unidad o por Totalidad"

        if (Cmb_Responsable.SelectedIndex != 0) // Si se seleciono un empleado
        {
            Recibo_Transitorio = new Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio();
            Recibo_Transitorio.P_Usuario_Creo=Cls_Sessiones.Nombre_Empleado.Trim();
            Recibo_Transitorio.P_Responsable_ID = Cmb_Responsable.SelectedValue; // Se agrega el empleado del combo

            if (Session["No_Contra_Recibo_RT"] != null)
                Recibo_Transitorio.P_No_Contra_Recibo = Session["No_Contra_Recibo_RT"].ToString().Trim();

            if (Session["No_Orden_Compra_RT"] != null)
                Recibo_Transitorio.P_No_Orden_Compra = Session["No_Orden_Compra_RT"].ToString().Trim();

            if (Session["Reporte_RT"] != null){
                   Tipo = Session["Reporte_RT"].ToString();

                   if (Tipo == "POR_UNIDAD")
                        Recibo_Transitorio.P_Tipo_Recibo ="UNIDAD";
                   else if (Tipo == "POR_TOTALIDAD")
                       Recibo_Transitorio.P_Tipo_Recibo = "TOTALIDAD";
                }
            Recibo_Transitorio.P_No_Requisicion = Txt_Requisicion.Text.Trim();
            Recibo_Transitorio.P_No_Requisicion = Recibo_Transitorio.P_No_Requisicion.Replace("RQ-", "");
            Int64 No_Recibo_Transitorio = Recibo_Transitorio.Guardar_Recibo(); //Metodo utilizado para guardar el recibo transitorio    

            Consultar_Recibo_Transitorio(No_Recibo_Transitorio, Tipo); // Se consulta el recibo transitorio
            Estatus_Inicial(); // Se configura la página para que este en estatus inicial.
        }
        else
        {
            Lbl_Informacion.Text = " Seleccionar el Responsable";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Recibo_Transitorio
    ///DESCRIPCIÓN:          
    ///PARAMETROS:           No_Recibo: Es el No. Recibo que se genero
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           09/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Consultar_Recibo_Transitorio(Int64 No_Recibo, String Tipo2)
    {
        Recibo_Transitorio = new Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio();
        DataTable Dt_Productos = new DataTable();
        DataTable Dt_Datos_Generales = new DataTable();
        String Tipo = "";
        String Nombre_Reporte_Crystal = "";

        if (Session["No_Contra_Recibo_RT"] != null)

            Recibo_Transitorio.P_No_Contra_Recibo = Session["No_Contra_Recibo_RT"].ToString().Trim();

        if (Session["Reporte_RT"] != null)
            Tipo = Session["Reporte_RT"].ToString().Trim();

        Recibo_Transitorio.P_No_Recibo_Transitorio = "" + No_Recibo;

        if (Tipo == "POR_UNIDAD") // Si el recibo  transitorio es por unidad
        {
            Dt_Productos = Recibo_Transitorio.Consulta_Productos_Recibo_Transitorio();// Se consultan los productos del recibo transitorio
            Dt_Datos_Generales = Recibo_Transitorio.Consulta_Datos_Generales_Recibo_Transitorio(); // Se consultan los datos generales del recibo transitorio
            Nombre_Reporte_Crystal = "Rpt_Ope_Com_Recibo_Transitorio_Por_Unidad.rpt";
        }
        else if (Tipo == "POR_TOTALIDAD") // Si el recibo  transitorio es por Totalidad
        {
            Dt_Productos = Recibo_Transitorio.Consulta_Productos_Requision();// Se consultan los productos del recibo transitorio por totalidad de los producto
            Nombre_Reporte_Crystal = "Rpt_Ope_Com_Recibo_Transitorio_Por_Totalidad.rpt";
            Dt_Datos_Generales = Recibo_Transitorio.Consulta_Datos_Generales_Recibo_Transitorio_Totalidad(); // Se consultan los datos generales del recibo transitorio

            if (Dt_Productos.Rows.Count > 0)
            {
                // Se realiza la obtencion de los datos para determianr si existencias tiene 0, entonces se agregan
                for (int i = 0; i < Dt_Productos.Rows.Count; i++)
                {
                    Int64 Cantidad_Entregada =0;
                    Int64 Cantidad = 0;

                    if (Dt_Productos.Rows[i]["EXISTENCIA"].ToString().Trim() != "")  // Se Verifica que la cantidad entregada tenga valor
                        Cantidad_Entregada = Convert.ToInt64(Dt_Productos.Rows[i]["EXISTENCIA"].ToString().Trim());
                    else
                        Cantidad_Entregada = 0;

                    if (Dt_Productos.Rows[i]["CANTIDAD"].ToString().Trim() != "")
                        Cantidad = Convert.ToInt64(Dt_Productos.Rows[i]["CANTIDAD"].ToString().Trim());   // Cantidad Solicitada

                    String Marca = Dt_Productos.Rows[i]["MARCA"].ToString().Trim();
                    String Modelo = Dt_Productos.Rows[i]["MODELO"].ToString().Trim();

                    Int64 Existencia = (Cantidad - Cantidad_Entregada);       // Se optiene la existencia
                    Dt_Productos.Rows[i].SetField("EXISTENCIA", Existencia); // Se agregan las existencias
                       
                    if (Modelo.Trim() == "")
                        Dt_Productos.Rows[i].SetField("MODELO", "INDISTINTO");
                    if (Marca.Trim() == "")
                        Dt_Productos.Rows[i].SetField("MARCA", "INDISTINTA");
                }
            }
        }
      
        Ds_Ope_Com_Recibos Ds_Recibos = new Ds_Ope_Com_Recibos();
        String Nombre_Reporte = "Recibo_Transitorio";
        String Formato = "PDF";
        Generar_Reporte(Dt_Datos_Generales, Dt_Productos, Ds_Recibos, Nombre_Reporte_Crystal, Nombre_Reporte, Formato);
    }
    protected void Grid_Ordenes_Compra_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Estado_Inicial_Busqueda_Avanzada();
    }
}