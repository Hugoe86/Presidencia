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
using Presidencia.Almacen_Elaborar_Contrarecibo.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Reportes;
using Presidencia.Stock;

public partial class paginas_Almacen_Frm_Ope_Alm_Elaborar_Contrarecibo : System.Web.UI.Page
{

    # region Variables

    Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Contra_Recibo; // Objeto de la clase Negocio utilizado para acceder a los métodos y variables

    # endregion

    # region  Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Activa"] = true;
            Estatus_Inicial();
        }
    }

    #endregion

    # region  Eventos

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Salir_Click
    /// DESCRIPCION:            Evento utilizado para salir de la aplicación
    ///                         o para configurar la página a sus estatus inicial
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            18/Marzo/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Salir")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Estatus_Inicial();
            Estado_Inicial_Busqueda_Avanzada();
        }
    }

    /// *************************************************************************************
    /// NOMBRE:              Txt_Fecha_Pago_TextChanged
    /// DESCRIPCIÓN:         En este método se valida  que no se ingrese la fecha de pago anterior a la fecha actual
    /// PARÁMETROS:          No se realiza la validacipon por que el TextBox esta inhabilitado           
    /// USUARIO CREO:        Salvador Hernández Ramírez
    /// FECHA CREO:          14-Julio-11
    /// USUARIO MODIFICO:   
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN: 
    /// *************************************************************************************
    protected void Txt_Fecha_Pago_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime FechaActual = DateTime.Now;
            DateTime FechaEntrega = Convert.ToDateTime(Txt_Fecha_Pago.Text.Trim());
            if (FechaEntrega >= FechaActual)
            {
                Lbl_Msg_Fecha_Pago.Visible = false;
            }
            else
            {
                Lbl_Msg_Fecha_Pago.Text = "La fecha de pago debe ser mayor al día de hoy";
                Lbl_Msg_Fecha_Pago.Visible = true;
            }
        }
        catch (Exception Ex)
        {
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Cmb_Almacen_SelectedIndexChanged
    /// DESCRIPCION:            Evento utilizado cuando se selecciona el elemento del combo Cmb_Almacen                
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            02/Agosto/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Cmb_Almacen_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Cmb_RegistroX_SelectedIndexChanged
    /// DESCRIPCION:            Evento utilizado cuando se selecciona el elemento del combo Cmb_Registro                
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            02/Agosto/2011  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Cmb_RegistroX_SelectedIndexChanged(object sender, EventArgs e)
    {
    }


    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Estado_Inicial_Busqueda_Avanzada();
    }

    #region Eventos Documentos Soporte

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Agregar_Doc_Soporte_Click
    /// DESCRIPCION:            Evento utilizado para agregar los documentos seleccionados por
    ///                         el usuario.
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            18/Marzo/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Agregar_Doc_Soporte_Click(object sender, ImageClickEventArgs e)
    {
        if (Cmb_Doc_Soporte.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Agregar_Documento();
        }

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Quitar_Doc_Soporte_Click
    /// DESCRIPCION:            Evento utilizado para quitar los documentos seleccionados por
    ///                         el usuario.
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            18/Marzo/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Quitar_Doc_Soporte_Click(object sender, ImageClickEventArgs e)
    {
        Quitar_Documentos();
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Seleccionar_Documento_Click
    /// DESCRIPCION:            Evento clic del botón que se encuentra en el grid "Grid_Documentos"
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            06/Abril/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Seleccionar_Documento_Click(object sender, ImageClickEventArgs e)
    {
        Btn_Quitar_Doc_Soporte.Enabled = true;
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Cmb_Doc_Soporte_SelectedIndexChanged
    /// DESCRIPCION:            Evento utilizado para habilitar o desabilitar el boton 
    ///                         "Btn_Agregar_Doc_Soporte".
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            06/Abril/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Cmb_Doc_Soporte_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Doc_Soporte.SelectedIndex != 0)
        {
            Btn_Agregar_Doc_Soporte.Enabled = true; // Se habilitan los botones
        }
        else
        {
            Btn_Agregar_Doc_Soporte.Enabled = false; // Se deshabilitan lso botones
        }
        Btn_Quitar_Doc_Soporte.Enabled = false;
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Eventos Grid

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Grid_Doc_Soporte_PageIndexChanging
    /// DESCRIPCION:            Se maneja la paginación del Grid
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            06/Abril/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Grid_Doc_Soporte_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Doc_Soporte.PageIndex = e.NewPageIndex;
        Grid_Doc_Soporte.DataSource = (DataTable)Session["Dt_Documentos"];
        Grid_Doc_Soporte.Columns[1].Visible = true;
        Grid_Doc_Soporte.DataBind();
        Grid_Doc_Soporte.Columns[1].Visible = false;
    }

   
    

        #endregion

    #region Búsqueda Abanzada 

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Buscar_Click
    /// DESCRIPCION:            Evento utilizado para realizar una busqueda simple                    
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            18/Marzo/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Detalles.Visible = false;
        Div_Ordenes_Compra.Visible = false;
        Session.Remove("Dt_Ordenes_Compra");  // Se elimina la session 
        Consultar_Ordenes_Compra();
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Busqueda_Avanzada_Click
    /// DESCRIPCION:            Evento utilizado para instanciar el método que se utiliza para 
    ///                         establecer el estatus inicial de la busqueda abanzada
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            19/Marzo/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
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
    ///NOMBRE DE LA FUNCION:    Chk_Proveedor_CheckedChanged
    ///DESCRIPCION:             Evento utilizado para habilitar o deshabilitar el combo proveedores.
    ///PARAMETROS:              
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              16/Marzo/2011 
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    protected void Chk_Proveedor_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Proveedor.Checked == true)
        {
            Cmb_Proveedores.Enabled = true;
        }
        else
        {
            Cmb_Proveedores.Enabled = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Chk_Fecha_B_CheckedChanged
    ///DESCRIPCION:             Evento utilizado para habilitar o deshabilitar los botones
    ///                         utilizados para asignar la fecha inicio y facha final.
    ///PARAMETROS:              
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              16/Marzo/2011 
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

    # endregion


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Seleccionar_Orden_Compra_Click
    /// DESCRIPCION:            Evento utilizado para agregar consultar los detalle
    ///                         de la orden de compra seleccionada por el usuario
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            19/Marzo/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Seleccionar_Orden_Compra_Click(object sender, ImageClickEventArgs e)
    {
        // Declaración de Objetos y Variables
        Session["Dt_Documentos"] = null;
        Grid_Doc_Soporte.DataSource = null;
        Grid_Doc_Soporte.DataBind();
        ImageButton Btn_Selec_Orden_Compra = null;
        Contra_Recibo = new Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio(); // Se crea el objeto para el manejo de los m{etodos
        String No_Orden_Compra = String.Empty;
        DataTable Dt_Ordenes_Compra = new DataTable();
        DataTable Dt_Produtos_OC = new DataTable();
        DataTable Dt_Montos_OC = new DataTable();
        DataRow[] Dr_Orden;
        String Tipo_Articulo = "";
        String Listado_Almacen = "";

        try
        {
            Btn_Selec_Orden_Compra = (ImageButton)sender;
            No_Orden_Compra = Btn_Selec_Orden_Compra.CommandArgument; // Se selecciona el No de Orden de Compra

            Session["NO_ORDEN_COMPRA_CR"] = No_Orden_Compra.Trim(); // Se asigna a la variable de session

            if (Session["Dt_Ordenes_Compra_CR"] != null) // Se carga la tabla con las Ordenes de Compra
                Dt_Ordenes_Compra = (DataTable)Session["Dt_Ordenes_Compra_CR"];

            if (Dt_Ordenes_Compra.Rows.Count > 0) // Si la tabla contiene productos
            {
                Dr_Orden = Dt_Ordenes_Compra.Select("NO_ORDEN_COMPRA='" + No_Orden_Compra.Trim() + "'");

                if (Dr_Orden.Length > 0) // Si el renglon tiene información
                {
                    if (!string.IsNullOrEmpty(Dr_Orden[0]["PROVEEDOR"].ToString()))
                        Txt_Proveedor.Text = HttpUtility.HtmlDecode(Dr_Orden[0]["PROVEEDOR"].ToString().Trim());
                    if (!string.IsNullOrEmpty(Dr_Orden[0]["FECHA"].ToString()))
                    {
                        String Fecha = HttpUtility.HtmlDecode(Dr_Orden[0]["FECHA"].ToString().Trim());  // Se optiene la fecha
                        DateTime Fecha_Convertida = Convert.ToDateTime(Fecha);  // Se conviente la fecha
                        Txt_Fecha_Generacion.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Convertida);
                    }
                    if (!string.IsNullOrEmpty(Dr_Orden[0]["FOLIO"].ToString()))
                        Txt_Orden_Compra.Text = HttpUtility.HtmlDecode(Dr_Orden[0]["FOLIO"].ToString().Trim());

                    if (!string.IsNullOrEmpty(Dr_Orden[0]["PROVEEDOR_ID"].ToString()))
                        Txt_Proveedor_ID.Value = HttpUtility.HtmlDecode(Dr_Orden[0]["PROVEEDOR_ID"].ToString().Trim());

                    if (!string.IsNullOrEmpty(Dr_Orden[0]["LISTADO_ALMACEN"].ToString()))
                        Session["LISTADO_ALMACEN"] = HttpUtility.HtmlDecode(Dr_Orden[0]["LISTADO_ALMACEN"].ToString().Trim());
                    else
                        Session["LISTADO_ALMACEN"] = "";

                    if (!string.IsNullOrEmpty(Dr_Orden[0]["NO_REQUISICION"].ToString()))
                        Txt_Requisicion.Text = HttpUtility.HtmlDecode(Dr_Orden[0]["NO_REQUISICION"].ToString().Trim());

                    if (!string.IsNullOrEmpty(Dr_Orden[0]["TIPO_ARTICULO"].ToString()))
                    {
                        Tipo_Articulo = HttpUtility.HtmlDecode(Dr_Orden[0]["TIPO_ARTICULO"].ToString().Trim());
                        Session["Tipo_Articulo_CR"] = Tipo_Articulo;
                    }
                }
            }

            Contra_Recibo.P_No_Orden_Compra = No_Orden_Compra.Trim(); // Se asigna el numero de orden de compra
            Contra_Recibo.P_Tipo_Articulo = Tipo_Articulo;
            Dt_Produtos_OC = Contra_Recibo.Consulta_Productos_Orden_Compra();

            if (Dt_Produtos_OC.Rows.Count > 0) // Si la consulta trae productos se llena el Grid
            {
                Div_Busqueda_Av.Visible = false;
                Session["Dt_Actualizar_Monto_Productos"] = Dt_Produtos_OC;
                Grid_Productos_Orden_Compra.Columns[0].Visible = true;
                Grid_Productos_Orden_Compra.DataSource = Dt_Produtos_OC; // Se llena el Grid
                Grid_Productos_Orden_Compra.DataBind();
                Grid_Productos_Orden_Compra.Columns[0].Visible = false;

                // Se llenan los combos que se encuentran en el grid
                for (int j = 0; j < Grid_Productos_Orden_Compra.Rows.Count; j++)
                {
                    DropDownList Cmb_Temporal_Almacen = (DropDownList)Grid_Productos_Orden_Compra.Rows[j].FindControl("Cmb_Almacen");
                    DropDownList Cmb_Temporal_Registro = (DropDownList)Grid_Productos_Orden_Compra.Rows[j].FindControl("Cmb_RegistroX");

                    if (Cmb_Temporal_Almacen.Items.Count == 0)
                    {
                        Cmb_Temporal_Almacen.Items.Add("<Seleccionar>");
                        Cmb_Temporal_Almacen.Items.Add("Ninguno");
                        Cmb_Temporal_Almacen.Items.Add("Resguardo");
                        Cmb_Temporal_Almacen.Items.Add("Recibo");
                        Cmb_Temporal_Almacen.Items[0].Value = "0";
                        Cmb_Temporal_Almacen.Items[0].Selected = true;
                    }

                    if (Cmb_Temporal_Registro.Items.Count == 0)
                    {
                        Cmb_Temporal_Registro.Items.Add("<Seleccionar>");
                        Cmb_Temporal_Registro.Items.Add("Ninguno");
                        Cmb_Temporal_Registro.Items.Add("Unidad");
                        Cmb_Temporal_Registro.Items.Add("Totalidad");
                        Cmb_Temporal_Registro.Items[0].Value = "0";
                        Cmb_Temporal_Registro.Items[0].Selected = true;
                    }
                }

                    if(Session["LISTADO_ALMACEN"].ToString().Trim()=="SI"){
                        Txt_Tipo_Requisicion.Text = " LISTADO DE ALMACÉN";
                         Listado_Almacen = Session["LISTADO_ALMACEN"].ToString().Trim(); // Se asigna la variable que indica si la requisicion es de un listado de almacén
                    }
                    else if(Tipo_Articulo=="SERVICIO")
                        Txt_Tipo_Requisicion.Text = "SERVICIOS";
                    else if (Tipo_Articulo == "PRODUCTO")
                        Txt_Tipo_Requisicion.Text = "PRODUCTOS";

                    if ((Listado_Almacen == "SI")|(Tipo_Articulo=="SERVICIO")) // Si al orden de compra, contiene la requisicion de Listado de almacén o es una orden de compra de servicios
                    {
                          // Se inabilitan los combos que se encuentran en el grid
                        
                        for (int j = 0; j < Grid_Productos_Orden_Compra.Rows.Count; j++) 
                        {
                            DropDownList Cmb_Temporal_Almacen = (DropDownList)Grid_Productos_Orden_Compra.Rows[j].FindControl("Cmb_Almacen");
                            DropDownList Cmb_Temporal_Registro = (DropDownList)Grid_Productos_Orden_Compra.Rows[j].FindControl("Cmb_RegistroX");
                            CheckBox Chk_Temporal_Recibo_T = (CheckBox)Grid_Productos_Orden_Compra.Rows[j].FindControl("Chk_R_Transitorio");

                            Cmb_Temporal_Almacen.Enabled = false;
                            Cmb_Temporal_Registro.Enabled = false;
                            Chk_Temporal_Recibo_T.Enabled = false;
                        }
                    }
                
                // Se asignan los montos de la orden de compra
                Dt_Montos_OC = Contra_Recibo.Consulta_Montos_Orden_Compra();

                if (Dt_Montos_OC.Rows.Count > 0)
                {
                    Lbl_SubTotal.Text = "" + Dt_Montos_OC.Rows[0]["SUBTOTAL"].ToString().Trim();
                    Lbl_IVA.Text = "" + Dt_Montos_OC.Rows[0]["TOTAL_IVA"].ToString().Trim();
                    Lbl_Total.Text = "" + Dt_Montos_OC.Rows[0]["TOTAL"].ToString().Trim();
                }
                Cargar_Factura_Inicial(); // Se carga un registro para escribir el No. Factura
                Div_Detalles.Visible = true;  // Se pone visible el Div_Detalles
                Div_Ordenes_Compra.Visible = false;
                Div_Contenedor_Msj_Error.Visible = false;
                Estatus_Inicial_Botones(true);  // Se configuran los botones y la búsqueda
                Btn_Guardar_Contra_Recibo.Visible = true;
            }
            else
            {
                DataTable Dt_Tem = new DataTable(); // Tabla temporal
                Grid_Productos_Orden_Compra.DataSource = Dt_Tem; // Se limpia el Grid
                Grid_Productos_Orden_Compra.DataBind();
                Div_Detalles.Visible = false;  // Se oculta  el Div_Detalles
                Lbl_Informacion.Text = "La Orden de Compra no contiene productos";
                Div_Contenedor_Msj_Error.Visible = true;
                Div_Ordenes_Compra.Visible = true;
                Btn_Guardar_Contra_Recibo.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Agregar_Facturas_Click
    /// DESCRIPCION:            Evento utilizado para agregar registros al Grid
    ///                         donde se va asignar los datos de las facturas
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            19/Marzo/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Agregar_Facturas_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Facturas = new DataTable(); // Se crea la tabla
        DataRow Dr_Dt_Facturas;
        Dt_Facturas.Columns.Add("NO_REGISTRO"); // Se inserta una columna a la tabla
        Lbl_Msg_Importe_Fact.Visible = false;
        Int16 No_Facturas = 0;

        try
        {
            if (Txt_No_Facturas.Text.Trim() != "")
            {
                No_Facturas = Convert.ToInt16(Txt_No_Facturas.Text.Trim());

                // Se valida si es un numero valido
                if ((No_Facturas > 0) & (No_Facturas < 11))
                {
                    for (int i = 0; i < No_Facturas; i++)
                    {
                        Dr_Dt_Facturas = Dt_Facturas.NewRow(); // Se crea el nuevo registro
                        Dr_Dt_Facturas["NO_REGISTRO"] = Convert.ToString(i); // Se convierte el valor
                        Dt_Facturas.Rows.InsertAt(Dr_Dt_Facturas, 0); // Se Inserta el Registro
                    }

                    if (Dt_Facturas.Rows.Count > 0) // Si la tabla contiene registros Se asignan al grid
                    {
                        Grid_Facturas.Columns[0].Visible = true;
                        Grid_Facturas.DataSource = Dt_Facturas;
                        Grid_Facturas.DataBind();
                        Grid_Facturas.Columns[0].Visible = false;
                    }
                    Div_Contenedor_Msj_Error.Visible = false;
                }
                else
                {
                    Lbl_Informacion.Text = "El número de facturas debe ser de 1 a 10";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Informacion.Text = "Ingresar el número de facturas";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Guardar_Contra_Recibo_Click
    /// DESCRIPCION:            Evento utilizado para acceder al método donde se da de alta el contra recibo                    
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            19/Junio/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Guardar_Contra_Recibo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
           if (Validacion_Controles())
            {
                Alta_Contra_Recibo(); // Instancia a los métodos
                Estado_Inicial_Busqueda_Avanzada();
                Estatus_Inicial();
            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Txt_Importe_Factura_TextChanged
    /// DESCRIPCION:            Evento utilizado para validar las cantidades que asigna el usuario                    
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            19/Junio/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Txt_Importe_Factura_TextChanged(object sender, EventArgs e)
    {
        Validar_Montos_Facturas(); // Se instancia al método utilizado para validar el los montos de la factura
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Btn_Fecha_Pago_Click
    /// DESCRIPCION:            Evento utilizado instanciar el método donde se valida la  fecha de pago que asigno el usuario                 
    /// PARAMETROS :            
    /// CREO       :            Salvador Hernández Ramírez
    /// FECHA_CREO :            19/Junio/2010  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    protected void Btn_Fecha_Pago_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Msg_Importe_Fact.Visible = false;
        Validar_Montos_Facturas();
    }

    #endregion

    # region  Métodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:    Estatus_Inicial
    ///DESCRIPCIÓN:             Método utilizado para asignar un estatus inicial a los componentes de la página
    ///PARAMETROS:  
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              14-Marzo-2011
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACIÓN:      
    ///*******************************************************************************
    public void Estatus_Inicial()
    {
        Lbl_Informacion.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        Div_Busqueda_Av.Visible = true;
        Div_Ordenes_Compra.Visible = false;
        Div_Detalles.Visible = false;
        Elimina_Sesiones();
        Consultar_Ordenes_Compra();
        Cargar_Documentos_Sopote();
        Estatus_Inicial_Botones(false);
        Btn_Guardar_Contra_Recibo.Visible = false;
        Llena_Combo_Proveedores();
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Consultar_Ordenes_Compra
    /// DESCRIPCION:            Método utilizado para consultar las ordenes de compra con estatus "SURTIDA"
    /// PARAMETROS :            
    /// CREO       :            Salvador Hérnandez Ramírez
    /// FECHA_CREO :            14/Marzo/2011
    /// MODIFICO          :     
    /// FECHA_MODIFICO    :    
    /// CAUSA_MODIFICACION:                            
    ///*******************************************************************************/
    public void Consultar_Ordenes_Compra()
    {
        Contra_Recibo = new Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio(); // Se crea objeto de la clase de negocio
        DataTable Dt_Ordenes_Compra = new DataTable();

        try
        {
                if (Txt_Busqueda.Text.Trim() != "")
                    Contra_Recibo.P_No_Orden_Compra = Txt_Busqueda.Text.Trim();

                if (Txt_Req_Buscar.Text.Trim() != "")
                    Contra_Recibo.P_No_Requisicion = Txt_Req_Buscar.Text.Trim();

                if (Chk_Proveedor.Checked == true)
                {
                    if (Cmb_Proveedores.SelectedIndex != 0)
                    {
                        Contra_Recibo.P_Proveedor_ID = Cmb_Proveedores.SelectedValue.Trim();
                    }
                    else
                        Contra_Recibo.P_Proveedor_ID = "";

                }

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
                                    Contra_Recibo.P_Fecha_Inicio_B = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                                    Contra_Recibo.P_Fecha_Fin_B = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                                    Div_Contenedor_Msj_Error.Visible = false;
                                }
                                else
                                {
                                    String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                    Contra_Recibo.P_Fecha_Inicio_B = Fecha;
                                    Contra_Recibo.P_Fecha_Fin_B = Fecha;
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

                Dt_Ordenes_Compra = Contra_Recibo.Consulta_Ordenes_Compra();

            if (Dt_Ordenes_Compra.Rows.Count > 0) // Si la consulta arrojo resultados
            {
                Grid_Ordenes_Compra.DataSource = Dt_Ordenes_Compra; // Se llena el Grid
                Grid_Ordenes_Compra.Columns[7].Visible = true;
                Grid_Ordenes_Compra.Columns[8].Visible = true;
                Grid_Ordenes_Compra.Columns[9].Visible = true; // Se pone visible la columna "LISTADO_ALMACEN"
                Grid_Ordenes_Compra.DataBind();
                Div_Ordenes_Compra.Visible = true;
                Div_Contenedor_Msj_Error.Visible = false;
                Grid_Ordenes_Compra.Columns[7].Visible = false;
                Grid_Ordenes_Compra.Columns[8].Visible = false;
                Grid_Ordenes_Compra.Columns[9].Visible = false; // Se pone visible la columna "LISTADO_ALMACEN"

                Session["Dt_Ordenes_Compra_CR"] = Dt_Ordenes_Compra; // Se guarda la tabla en una variable de session
            }
            else
            {
                Lbl_Informacion.Text = "No se encontraron órdenes de compra ";
                Div_Contenedor_Msj_Error.Visible = true;
                Div_Ordenes_Compra.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al consultar las órdenes de compra. Error: [" + ex.Message + "]");
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Elimina_Sesiones
    /// DESCRIPCION:            Método utilizado para eliminar las sessiones
    /// PARAMETROS :            
    /// CREO       :            Salvador Hérnandez Ramírez
    /// FECHA_CREO :            06/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public void Elimina_Sesiones()
    {
        Session.Remove("Dt_Ordenes_Compra");
        Session["NO_ORDEN_COMPRA_CR"] = null;
        Session["Dt_Documentos"] = null;
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
    }


    #region Metodos Búsqueda 

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Llena_Combo_Proveedores
    /// DESCRIPCION:            Llenar el combo de los proveedores
    /// PARAMETROS :            
    /// CREO       :            Salvador Hérnandez Ramírez
    /// FECHA_CREO :            19/Marzo/2011
    /// MODIFICO          :      
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private void Llena_Combo_Proveedores()
    {
        Contra_Recibo = new Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio();  
        try
        {
            Cmb_Proveedores.DataSource = Contra_Recibo.Consulta_Proveedores();
            Cmb_Proveedores.DataTextField = Cat_Com_Proveedores.Campo_Compañia;
            Cmb_Proveedores.DataValueField = Cat_Com_Proveedores.Campo_Proveedor_ID;
            Cmb_Proveedores.DataBind();
            Cmb_Proveedores.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
            Cmb_Proveedores.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
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
            if (Cmb_Proveedores.Items.Count != 0)
                Cmb_Proveedores.SelectedIndex = 0;
          
            Cmb_Proveedores.Enabled = false;
            Txt_Fecha_Inicio.Text = "";
            Txt_Fecha_Fin.Text = "";
            Txt_Busqueda.Text = "";
            Txt_Req_Buscar.Text = "";
            Chk_Proveedor.Checked = false;
            Chk_Fecha_B.Checked = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
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
       

        //if (Btn_Busqueda_Avanzada.Visible)
        //{
        //    //Configuracion_Acceso_LinkButton("Frm_Ope_Alm_Elaborar_Contrarecibo.aspx");
        //    if (Btn_Busqueda_Avanzada.Visible)
        //    {
        //        Txt_Busqueda.Visible = true;
        //        Btn_Buscar.Visible = true;
        //    }
        //    else
        //    {
        //        Txt_Busqueda.Visible = false;
        //        Btn_Buscar.Visible = false;
        //    }
        //}
        //else
        //{
        //    Txt_Busqueda.Visible = false;
        //    Btn_Buscar.Visible = false;
        //}
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Estatus_Busqueda
    ///DESCRIPCIÓN:          Metodo que valida que seleccione un proveedor dentro del modalpopup
    ///PARAMETROS:   
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Validar_Estatus_Busqueda()
    {
        Contra_Recibo = new Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio();
        
        Boolean Chk_Activado = false;
        if (Chk_Proveedor.Checked == true)
        {
            if (Cmb_Proveedores.SelectedIndex != 0)
            {
                Contra_Recibo.P_Proveedor_ID = Cmb_Proveedores.SelectedValue;
               
            }
            else
            {
          
            }
            Chk_Activado = true;
        }

        if (Chk_Fecha_B.Checked == true)
        {
            Chk_Activado = true;
        }

       
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN:          Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///                      en la busqueda del Modalpopup
    ///PARAMETROS:   
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Verificar_Fecha()
    {
        DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date2 = new DateTime();

        if (Chk_Fecha_B.Checked)
        {
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
                            //Elaborar_Contrarecibo.P_Fecha_Inicio_B = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                            //Elaborar_Contrarecibo.P_Fecha_Fin_B = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                        }
                        else
                        {
                            String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                            //Elaborar_Contrarecibo.P_Fecha_Inicio_B = Fecha;
                            //Elaborar_Contrarecibo.P_Fecha_Fin_B = Fecha;
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Marzo/2011 
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
    

    #region Metodos Documentos Soporte


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Cargar_Documentos_Sopote
    /// DESCRIPCION:            Método utilizado llenar el DropDownList con los documetos de soporte de almacén
    /// PARAMETROS :            
    /// CREO       :            Salvador Hérnandez Ramírez
    /// FECHA_CREO :            14-Marzo-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public void Cargar_Documentos_Sopote()
    {
        Contra_Recibo = new Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio();  

         try
            {
                 Cmb_Doc_Soporte.DataSource = Contra_Recibo.Consulta_Documentos_Soporte();
                 Cmb_Doc_Soporte.DataTextField= Cat_Com_Documentos.Campo_Nombre;
                 Cmb_Doc_Soporte.DataValueField=Cat_Com_Documentos.Campo_Documento_ID;
                 Cmb_Doc_Soporte.DataBind();
                 Cmb_Doc_Soporte.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
                 Cmb_Doc_Soporte.SelectedIndex=0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar al combo los documentos soporte. Error: [" + ex.Message + "]");
            }
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Agregar_Documento
    /// DESCRIPCION:            Método utilizado agregar el documento seleccionado por el usuario al Grid 
    /// PARAMETROS :            
    /// CREO       :            Salvador Hérnandez Ramírez
    /// FECHA_CREO :            14-Marzo-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public void  Agregar_Documento()
    {
        Contra_Recibo = new Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio();
        DataTable Dt_Detalles_Doc_Soporte = new DataTable();
        Contra_Recibo.P_Documento_ID = Cmb_Doc_Soporte.SelectedItem.Value;
        
         try {

             Dt_Detalles_Doc_Soporte = Contra_Recibo.Consulta_Documentos_Soporte();
            DataTable Dt_Doc_Soporte = (DataTable)Grid_Doc_Soporte.DataSource;

            if (Session["Dt_Documentos"] == null)
            {
                Dt_Doc_Soporte = new DataTable();
                Dt_Doc_Soporte.Columns.Add("DOCUMENTO_ID", Type.GetType("System.String"));
                Dt_Doc_Soporte.Columns.Add("NOMBRE", Type.GetType("System.String"));
                Dt_Doc_Soporte.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
            }
            else
            {
                Dt_Doc_Soporte = (DataTable)Session["Dt_Documentos"];
            }

            if (!Buscar_Clave_DataTable(Cmb_Doc_Soporte.SelectedItem.Value, Dt_Doc_Soporte, 0))
            {
                DataRow Fila = Dt_Doc_Soporte.NewRow();
                Fila["DOCUMENTO_ID"] = HttpUtility.HtmlDecode(Dt_Detalles_Doc_Soporte.Rows[0]["DOCUMENTO_ID"].ToString()); // Se debe realizar una consulta para obtenerlo
                Fila["NOMBRE"] = HttpUtility.HtmlDecode(Dt_Detalles_Doc_Soporte.Rows[0]["NOMBRE"].ToString());
                Fila["DESCRIPCION"] = HttpUtility.HtmlDecode(Dt_Detalles_Doc_Soporte.Rows[0]["DESCRIPCION"].ToString());

                Dt_Doc_Soporte.Rows.Add(Fila);
                Grid_Doc_Soporte.DataSource = Dt_Doc_Soporte;
                Grid_Doc_Soporte.Columns[1].Visible=true;
                Grid_Doc_Soporte.DataBind();
                Grid_Doc_Soporte.Columns[1].Visible = false;
                Grid_Doc_Soporte.Visible = true;
                Session["Dt_Documentos"] = Dt_Doc_Soporte;
                //Div_Error_Datos_OC.Visible = false;
            }
            else
            {
                //Lbl_Información_OC.Text = "El documento ya esta agregado.";
                //Div_Error_Datos_OC.Visible = true;
            }
         }
         catch (Exception ex)
         {
             throw new Exception("Error al agregar al grid los documentos soporte. Error: [" + ex.Message + "]");
            
         }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Buscar_Clave_DataTable
    /// DESCRIPCION:            Método utilizado quitar del grid el documento seleccionado por el usuario
    /// PARAMETROS :            
    /// CREO       :            Salvador Hérnandez Ramírez
    /// FECHA_CREO :            14-Marzo-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public void Quitar_Documentos()
    {

        try
        {
            if (Grid_Doc_Soporte.Rows.Count > 0 && Grid_Doc_Soporte.SelectedIndex > (-1))
            {
                Int32 Registro = ((Grid_Doc_Soporte.PageIndex) * Grid_Doc_Soporte.PageSize) + (Grid_Doc_Soporte.SelectedIndex);

                if (Session["Dt_Documentos"] != null)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Documentos"];
                    Tabla.Rows.RemoveAt(Registro);
                    Session["Dt_Documentos"] = Tabla;
                    Grid_Doc_Soporte.SelectedIndex = (-1);
                    Llenar_Grid_Documentos(Grid_Doc_Soporte.PageIndex, Tabla);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al quitar los documentos. Error: [" + ex.Message + "]");
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Documentos
    ///DESCRIPCIÓN:          Llena la tabla de Resguardantes
    ///PROPIEDADES:     
    ///                      1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///                      2.  Tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO:                 Salvador Hernández  Rámirez.
    ///FECHA_CREO:           14-Marzo-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Documentos(Int32 Pagina, DataTable Tabla)
    {
        Grid_Doc_Soporte.Columns[1].Visible = true;
        Grid_Doc_Soporte.DataSource = Tabla;
        Grid_Doc_Soporte.PageIndex = Pagina;
        Grid_Doc_Soporte.DataBind();
        Grid_Doc_Soporte.Columns[1].Visible = false;
        Session["Dt_Documentos"] = Tabla;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Buscar_Clave_DataTable
    /// DESCRIPCION:            Método utilizado para validad que no se agreguen 2 documentos iguales 
    /// PARAMETROS :            
    /// CREO       :            Salvador Hérnandez Ramírez
    /// FECHA_CREO :            15-Marzo-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    private Boolean Buscar_Clave_DataTable(String Clave, DataTable Tabla, Int32 Columna)
    {
        Boolean Resultado_Busqueda = false;

        try
        {
            if (Tabla != null && Tabla.Rows.Count > 0 && Tabla.Columns.Count > 0)
            {
                if (Tabla.Columns.Count > Columna)
                {
                    for (Int32 Contador = 0; Contador < Tabla.Rows.Count; Contador++)
                    {
                        if (Tabla.Rows[Contador][Columna].ToString().Trim().Equals(Clave.Trim()))
                        {
                            Resultado_Busqueda = true;
                            break;
                        }
                    }
                }
            }

            return Resultado_Busqueda;

        }
        catch (Exception ex)
        {
            throw new Exception("Error validar los documentos. Error: [" + ex.Message + "]");
        }
    }

    #endregion


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
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.- Dt_Datos_Generales.- Contiene los datos generales del contra recibo
    ///                      2.- Dt_Facturas_Proveedor - Contiene las facturas del contra recibo
    ///                      2.- Dt_Documentos_Soporte.- Contiene los documentos del contra recibo
    ///                      3.- DataSet_Reporte.-  Instancia del dataset creado para el llenado del repote 
    ///                      4.- Formato, contiene el nombre del documento que se va a generar
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           16/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Reporte(DataTable Dt_Datos_Generales, DataTable Dt_Facturas_Proveedor, DataTable Dt_Documentos_Soporte, DataSet DataSet_Reporte, String Formato)
    {
        DataRow Renglon;
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";

        try
        {
            // Llenar la tabla "Cabecera" del Dataset
            Renglon = Dt_Datos_Generales.Rows[0];
            DataSet_Reporte.Tables[0].ImportRow(Renglon);

            if (Dt_Facturas_Proveedor.Rows.Count > 0)
            {
                // Llenar los detalles del DataSet
                for (int Cont_Elementos = 0; Cont_Elementos < Dt_Facturas_Proveedor.Rows.Count; Cont_Elementos++)
                {
                    // Instanciar renglon e importarlo
                    Renglon = Dt_Facturas_Proveedor.Rows[Cont_Elementos];
                    DataSet_Reporte.Tables[1].ImportRow(Renglon);
                }
            }

            if (Dt_Documentos_Soporte.Rows.Count > 0)
            {
                // Llenar los documentos del DataSet
                for (int Elementos = 0; Elementos < Dt_Documentos_Soporte.Rows.Count; Elementos++)
                {
                    // Instanciar renglon e importarlo
                    Renglon = Dt_Documentos_Soporte.Rows[Elementos];
                    DataSet_Reporte.Tables[2].ImportRow(Renglon);
                }
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Contrarecibo.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Contra_Recibo_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref DataSet_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);

        }
        catch (Exception ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + ex.Message + "]");
        }
    }

    /// *************************************************************************************
    /// NOMBRE:              Cargar_Factura_Inicial
    /// DESCRIPCIÓN:         Por predeterminado se agrega un registro al Grid para que el usuario asigne los datos 
    ///                      de la factura.
    /// PARÁMETROS:                      
    /// USUARIO CREO:        Salvador Hernández Ramírez
    /// FECHA CREO:          08-Julio-11
    /// USUARIO MODIFICO:   
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN: 
    /// *************************************************************************************
    private void Cargar_Factura_Inicial()
    {
        DataRow Dr_Dt_Facturas;
        DataTable Dt_Facturas = new DataTable();

        try
        { 
                Dt_Facturas.Columns.Add("NO_REGISTRO"); // Se inserta una columna a la tabla

                Dr_Dt_Facturas = Dt_Facturas.NewRow(); // Se crea el nuevo registro
                Dr_Dt_Facturas["NO_REGISTRO"] = Convert.ToString(1); // Se convierte el valor
                Dt_Facturas.Rows.InsertAt(Dr_Dt_Facturas, 0); // Se Inserta el Registro

                if (Dt_Facturas.Rows.Count > 0) // Si la tabla contiene registros Se asignan al grid
                {
                    Grid_Facturas.Columns[0].Visible = true;
                    Grid_Facturas.DataSource = Dt_Facturas;
                    Grid_Facturas.DataBind();
                    Grid_Facturas.Columns[0].Visible = false;
                }
                String Fecha = DateTime.Now.ToString("dd/MMM/yyyy");
                String Importe = Lbl_Total.Text.Trim();

                // Se ingresan los valores a los TextBox del Grid  Grid_Facturas
                for (int i = 0; i < Grid_Facturas.Rows.Count; i++)
                {
                    TextBox Txt_Importe_Factura = (TextBox)Grid_Facturas.Rows[i].FindControl("Txt_Importe_Factura");
                    TextBox Txt_Fecha_Factura = (TextBox)Grid_Facturas.Rows[i].FindControl("Txt_Fecha_Factura");

                    Txt_Importe_Factura.Text = Importe;
                    Txt_Fecha_Factura.Text = Fecha;
                }
          }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }

    /// *************************************************************************************
    /// NOMBRE:              Validacion_Controles
    /// DESCRIPCIÓN:         Se valida que el usuario seleccione correctamente los datos  
    ///                      para generar el contra recibo.
    /// PARÁMETROS:                      
    /// USUARIO CREO:        Salvador Hernández Ramírez
    /// FECHA CREO:          08-Julio-11
    /// USUARIO MODIFICO:   
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN: 
    /// *************************************************************************************
    private Boolean Validacion_Controles()
    {
        Boolean Validacion = true;
        String Mensaje = "";
        Double Importe_Orden_Compra = 0;
        String Listado_Almacen = "";
        String Tipo_Articulo = "";


        try
        {  
                if (Session["LISTADO_ALMACEN"] != null)
                    Listado_Almacen = Session["LISTADO_ALMACEN"].ToString().Trim();

                if (Session["Tipo_Articulo_CR"] != null)
                    Tipo_Articulo = Session["Tipo_Articulo_CR"].ToString().Trim();
                String Mensaje_II = "";
                if ((Listado_Almacen == "")&(Tipo_Articulo == "PRODUCTO")) // Si no es listado de almacén o no es de servicio
                {
                        // Se valida que el los productos del grid se  seleccionen completamente
                        for (int i = 0; i < Grid_Productos_Orden_Compra.Rows.Count; i++)
                        {
                            DropDownList Cmb_Temporal_Almacen = (DropDownList)Grid_Productos_Orden_Compra.Rows[i].FindControl("Cmb_Almacen");
                            DropDownList Cmb_Temporal_Registro = (DropDownList)Grid_Productos_Orden_Compra.Rows[i].FindControl("Cmb_RegistroX");

                            if ((Cmb_Temporal_Almacen.SelectedIndex == 0) | (Cmb_Temporal_Registro.SelectedIndex == 0))
                            {
                                Validacion = false;
                                Mensaje = " Seleccionar la opción de Almacén y Registro para todos los productos. <br/> ";
                                i = Grid_Productos_Orden_Compra.Rows.Count;
                            }

                            if ((Cmb_Temporal_Almacen.SelectedItem.Text == "Resguardo") && (Cmb_Temporal_Registro.SelectedItem.Text == "Ninguno"))
                            {
                                Validacion = false;
                                Mensaje_II = Mensaje_II + "Un Resguardo debe llevar un registro de datos, indique el Registro en el producto " + (i + 1) + " de la lista. <br/>";
                            }

                            if ((Cmb_Temporal_Almacen.SelectedItem.Text == "Recibo") && (Cmb_Temporal_Registro.SelectedItem.Text == "Ninguno"))
                            {
                                Validacion = false;
                                Mensaje_II = Mensaje_II + "Un Recibo debe llevar un registro de datos, indique el Registro en el producto " + (i + 1) + " de la lista. <br/>";
                            }
                        }

                        Mensaje = Mensaje + Mensaje_II;
                }


                if (Txt_Fecha_Pago.Text.Equals("")) // Validar que se haya seleccionado la fecha de pago
                {
                    if (!Validacion) { Mensaje = Mensaje + "<br/>"; }

                    Mensaje = Mensaje + " Seleccionar la Fecha de Pago";
                    Validacion = false;
                }

                // Se valida que se haga asignado completamente los datos de las facturas
                for (int h = 0; h < Grid_Facturas.Rows.Count; h++)
                {
                    TextBox Txt_No_Factura = (TextBox)Grid_Facturas.Rows[h].FindControl("Txt_No_Factura");
                    TextBox Txt_Importe_Factura = (TextBox)Grid_Facturas.Rows[h].FindControl("Txt_Importe_Factura");
                    TextBox Txt_Fecha_Factura = (TextBox)Grid_Facturas.Rows[h].FindControl("Txt_Fecha_Factura");

                    if ((Txt_No_Factura.Text.Trim() == "") | (Txt_Importe_Factura.Text.Trim() == "") | (Txt_Fecha_Factura.Text.Trim() == ""))
                    {
                        if (!Validacion) { Mensaje = Mensaje + "<br/>"; }

                        Mensaje = Mensaje + " Para cada factura, agregar el No. Factura, importe y seleccionar la fecha";
                        h = Grid_Facturas.Rows.Count;
                        Validacion = false;
                    }

                    if (Txt_Importe_Factura.Text != "")
                    {
                        Double Importe = Convert.ToDouble(Txt_Importe_Factura.Text.Trim());
                        Importe_Orden_Compra = Importe_Orden_Compra + Importe;
                    }
                }

                if (Grid_Facturas.Rows.Count <= 0)
                {
                    if (!Validacion) { Mensaje = Mensaje + "<br/>"; }

                    Mensaje = Mensaje + " Agregar como minimo una factura";
                    Validacion = false;
                }

                if (Validacion == false)
                {
                    Lbl_Informacion.Text = HttpUtility.HtmlDecode(Mensaje);
                    Lbl_Informacion.Visible = true;
                }
                return Validacion;

          }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }

    /// *************************************************************************************
    /// NOMBRE:              Alta_Contra_Recibo
    /// DESCRIPCIÓN:         Método Utilizado para dar de alta el contra recibo
    /// PARÁMETROS:                      
    /// USUARIO CREO:        Salvador Hernández Ramírez
    /// FECHA CREO:          08-Julio-11
    /// USUARIO MODIFICO:   
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN: 
    /// *************************************************************************************
    private void Alta_Contra_Recibo()
    {
        Contra_Recibo = new Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio(); // Se crea la instancia del objeto

        DataTable Dt_Productos_OC = new DataTable(); // Tabla para Guardar los productos
        DataTable Dt_Facturas_OC = new DataTable();  // Tabla para Guardar las Facturas
        DataTable Dt_Docuemntos_Soporte_OC = new DataTable(); // Tabla para Guardar los Documentos Soporte
        DataRow Dr_Productos;
        DataRow Dr_Factura;
        DataRow Dr_Documento;
        Int64 No_Contra_Recibo = 0;
        String Listado_Almacen = "";
        String Tipo_Articulo = "";

        try
        {  
            if(Session["LISTADO_ALMACEN"] != null)
                Listado_Almacen = Session["LISTADO_ALMACEN"].ToString().Trim();

            if (Session["Tipo_Articulo_CR"] != null)
                Tipo_Articulo = Session["Tipo_Articulo_CR"].ToString();

            // Se agregan las columnas a la tabla Dt_Productos_OC
            Dt_Productos_OC.Columns.Add("PRODUCTO_ID");
            Dt_Productos_OC.Columns.Add("RESGUARDO");
            Dt_Productos_OC.Columns.Add("RECIBO");
            Dt_Productos_OC.Columns.Add("UNIDAD");
            Dt_Productos_OC.Columns.Add("TOTALIDAD");
            Dt_Productos_OC.Columns.Add("RECIBO_TRANSITORIO");

            // Se hace el recorrido del grid para llenar la tabla con los productos
            for (int i = 0; i < Grid_Productos_Orden_Compra.Rows.Count; i++)
            {
                DropDownList Cmb_Temporal_Almacen = (DropDownList)Grid_Productos_Orden_Compra.Rows[i].FindControl("Cmb_Almacen");
                DropDownList Cmb_Temporal_Registro = (DropDownList)Grid_Productos_Orden_Compra.Rows[i].FindControl("Cmb_RegistroX");
                CheckBox Chk_Temporal_Recibo_T = (CheckBox)Grid_Productos_Orden_Compra.Rows[i].FindControl("Chk_R_Transitorio");

                String Resguardo = "";
                String Recibo = "";
                String Unidad = "";
                String Totalidad = "";
                String Recibo_Transitorio = "";

                if (Cmb_Temporal_Almacen.SelectedIndex == 2) // Para indicar si es Resguardo o el Recibo
                    Resguardo = "SI";
                else if (Cmb_Temporal_Almacen.SelectedIndex == 3)
                    Recibo = "SI";

                if (Cmb_Temporal_Registro.SelectedIndex == 2) // Para indicar si es Unidad o Totalidad
                    Unidad = "SI";
                else if (Cmb_Temporal_Registro.SelectedIndex == 3)
                    Totalidad = "SI";

                if (Chk_Temporal_Recibo_T.Checked) // Para indicar si lleva Resibo Transitorio
                    Recibo_Transitorio = "SI";
                else
                    Recibo_Transitorio = "NO";

                Dr_Productos = Dt_Productos_OC.NewRow(); // Se crea el nuevo registro

                Dr_Productos["PRODUCTO_ID"] = HttpUtility.HtmlDecode(Grid_Productos_Orden_Compra.Rows[i].Cells[0].Text.ToString().Trim());
                Dr_Productos["RESGUARDO"] = Resguardo.Trim();
                Dr_Productos["RECIBO"] = Recibo.Trim();
                Dr_Productos["UNIDAD"] = Unidad.Trim();
                Dr_Productos["TOTALIDAD"] = Totalidad.Trim();
                Dr_Productos["RECIBO_TRANSITORIO"] = Recibo_Transitorio.Trim();

                Dt_Productos_OC.Rows.InsertAt(Dr_Productos, i);// Se inserta el registro a la tabla "Dt_Productos_OC"
            }

            // Se agegan las columnas a la tabla Dt_Facturas
            Dt_Facturas_OC.Columns.Add("NO_FACTURA_PROVEEDOR");
            Dt_Facturas_OC.Columns.Add("IMPORTE_FACTURA");
            Dt_Facturas_OC.Columns.Add("FECHA_FACTURA");

            if (Grid_Facturas.Rows.Count > 0)
            {
                // Se realiza el recorrido por el Grid_Facturas para el llenado de la tabla Dt_Facturas_OC
                for (int j = 0; j < Grid_Facturas.Rows.Count; j++)
                {
                    TextBox Txt_No_Factura = (TextBox)Grid_Facturas.Rows[j].FindControl("Txt_No_Factura");
                    TextBox Txt_Importe_Factura = (TextBox)Grid_Facturas.Rows[j].FindControl("Txt_Importe_Factura");
                    TextBox Txt_Fecha_Factura = (TextBox)Grid_Facturas.Rows[j].FindControl("Txt_Fecha_Factura");

                    Dr_Factura = Dt_Facturas_OC.NewRow(); // Se crea el nuevo registro

                    Dr_Factura["NO_FACTURA_PROVEEDOR"] = Txt_No_Factura.Text.Trim(); // Se agregan los datos
                    Dr_Factura["IMPORTE_FACTURA"] = Txt_Importe_Factura.Text.Trim();
                    Dr_Factura["FECHA_FACTURA"] = Txt_Fecha_Factura.Text.Trim();

                    Dt_Facturas_OC.Rows.InsertAt(Dr_Factura, j);// Se inserta el registro a la tabla "Dt_Facturas_OC"
                }
            }

            // Se agrega la columna  a la tabla "Dt_Docuemntos_Soporte_OC"
            Dt_Docuemntos_Soporte_OC.Columns.Add("DOCUMENTO_ID");

            // Se realiza el recorrido por el Grid_Doc_Soporte para el llenado de la tabla Dt_Docuemntos_Soporte_OC
            if (Grid_Doc_Soporte.Rows.Count > 0)
            {
                for (int h = 0; h < Grid_Doc_Soporte.Rows.Count; h++)
                {
                    String Documento_ID = HttpUtility.HtmlDecode(Grid_Doc_Soporte.Rows[h].Cells[1].Text.ToString().Trim()); // Se agrega el Documento_ID
                    Dr_Documento = Dt_Docuemntos_Soporte_OC.NewRow(); // Se crea el nuevo registro
                    Dr_Documento["DOCUMENTO_ID"] = Documento_ID;
                    Dt_Docuemntos_Soporte_OC.Rows.InsertAt(Dr_Documento, h);// Se inserta el registro a la tabla "Dt_Docuemntos_Soporte_OC"
                }
            }

            if (Session["NO_ORDEN_COMPRA_CR"] != null) // Se verifica que la session no sea null
                Contra_Recibo.P_No_Orden_Compra = Session["NO_ORDEN_COMPRA_CR"].ToString().Trim();

            //Contra_Recibo.P_Fecha_Pago = Txt_Fecha_Pago.Text.Trim(); // Se asigan para dar de alta el contra recibo
            Contra_Recibo.P_Fecha_Pago = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Txt_Fecha_Pago.Text.Trim()));            
            
            Contra_Recibo.P_Dt_Productos_OC = Dt_Productos_OC;

            if (Session["Dt_Actualizar_Monto_Productos"] != null)
            {
                DataTable Dt_Productos = new DataTable();
                Dt_Productos = (DataTable)Session["Dt_Actualizar_Monto_Productos"]; // Se asigna la tabla a Dt_Productos
          
                if (Listado_Almacen =="SI")
                {
                    Contra_Recibo.P_Tipo_Orden_Compra = "LISTADO_ALMACEN";
                    Contra_Recibo.P_Dt_Actualizar_Productos = Dt_Productos; // Se asigna la tabla a la variable de session
                }
                else if (Listado_Almacen == "")
                {
                    Contra_Recibo.P_Tipo_Orden_Compra = "TRANSITORIA";

                        if (Dt_Productos.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString().Trim() != "")
                            Contra_Recibo.P_Proyecto_Programa_ID = Dt_Productos.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString().Trim();

                        if (Dt_Productos.Rows[0]["DEPENDENCIA_ID"].ToString().Trim() != "")
                            Contra_Recibo.P_Dependencia_ID = Dt_Productos.Rows[0]["DEPENDENCIA_ID"].ToString().Trim();

                        if (Dt_Productos.Rows[0]["PARTIDA_ID"].ToString().Trim() != "")
                            Contra_Recibo.P_Partida_ID = Dt_Productos.Rows[0]["PARTIDA_ID"].ToString().Trim();
                }
            }

            Contra_Recibo.P_Dt_Facturas_Proveedor = Dt_Facturas_OC;
            Contra_Recibo.P_Dt_Documentos_Soporte = Dt_Docuemntos_Soporte_OC;
            Contra_Recibo.P_SubTotal = Convert.ToDouble(Lbl_SubTotal.Text.Trim());
            Contra_Recibo.P_IVA = Convert.ToDouble(Lbl_IVA.Text.Trim());
            Contra_Recibo.P_Total = Convert.ToDouble(Lbl_Total.Text.Trim());
            Contra_Recibo.P_Proveedor_ID = Txt_Proveedor_ID.Value.ToString().Trim();
            Contra_Recibo.P_Tipo_Articulo = Tipo_Articulo;

            TextBox Txt_No_Factura_P = (TextBox)Grid_Facturas.Rows[0].FindControl("Txt_No_Factura");
            TextBox Txt_Fecha_Factura_P = (TextBox)Grid_Facturas.Rows[0].FindControl("Txt_Fecha_Factura");

            if (Txt_No_Factura_P.Text.Trim() != "")
                Contra_Recibo.P_No_Factura_Proveedor = Txt_No_Factura_P.Text.Trim();

            if (Txt_Fecha_Factura_P.Text.Trim() != "")
                Contra_Recibo.P_Fecha_Factura = Txt_Fecha_Factura_P.Text.Trim();

            if (Txt_Observaciones.Text.Trim() != "") // Si hay observaciones
            {
                if (Txt_Observaciones.Text.Trim().Length < 249)
                    Contra_Recibo.P_Observaciones = Txt_Observaciones.Text.Trim();
                else if (Txt_Observaciones.Text.Trim().Length < 249)
                    Contra_Recibo.P_Observaciones = Txt_Observaciones.Text.Substring(0, 248);
            }
            else
                Contra_Recibo.P_Observaciones = "";

            // Datos del empleado
            
            Contra_Recibo.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado.Trim();
            Contra_Recibo.P_Empleado_Almacen_ID = Cls_Sessiones.Empleado_ID.Trim();
            No_Contra_Recibo = Contra_Recibo.Guardar_Contra_Recibo(); // Se genera el contra recibo

            Consultar_Datos_Contra_Recibo(No_Contra_Recibo);  // Se consultan los datos del contra recibo, para generar el reporte
            //GENERAR PÓLIZA
            //Cls_Ope_Alm_Stock.Crear_Poliza_Compra_Transitoria(Txt_Requisicion.Text.Replace("RQ-",""));
            //bool Operacion_Realizada = Contra_Recibo.Generar_Poliza();
            
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// *************************************************************************************
    /// NOMBRE:              Cargar_Factura_Inicial
    /// DESCRIPCIÓN:         Método Utilizado para dar consultar el contra recibo generado                 
    /// PARÁMETROS:                      
    /// USUARIO CREO:        Salvador Hernández Ramírez
    /// FECHA CREO:          10-Julio-11
    /// USUARIO MODIFICO:   
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN: 
    /// *************************************************************************************
    private void Consultar_Datos_Contra_Recibo(Int64 No_Contra_Recibo)
    {
        DataTable Dt_Datos_Generales = new DataTable();  // Se crean las tablas
        DataTable Dt_Facturas_Proveedor = new DataTable();
        DataTable Dt_Documentos_Soporte = new DataTable();

        try
        {   
            Contra_Recibo = new Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio();
            Contra_Recibo.P_No_Contra_Recibo = No_Contra_Recibo; // Se asigna el No. Contra Recibo
            Dt_Datos_Generales = Contra_Recibo.Consultar_Datos_Generales_ContraRecibo(); // Se consulta la información para mostrar el reporte
            Dt_Facturas_Proveedor = Contra_Recibo.Consultar_Facturas_ContraRecibo();
            Dt_Documentos_Soporte = Contra_Recibo.Consultar_Documentos_Contrarecibo();

            Ds_Alm_Com_Contrarecibo Ds_Contra_Recibo = new Ds_Alm_Com_Contrarecibo();
            String Formato = "PDF";
            Generar_Reporte(Dt_Datos_Generales, Dt_Facturas_Proveedor, Dt_Documentos_Soporte, Ds_Contra_Recibo, Formato);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    /// *************************************************************************************
    /// NOMBRE:              Validar_Montos_Facturas
    /// DESCRIPCIÓN:         En este método se valida lo montos de las facturas
    /// PARÁMETROS:                      
    /// USUARIO CREO:        Salvador Hernández Ramírez
    /// FECHA CREO:          08-Julio-11
    /// USUARIO MODIFICO:   
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN: 
    /// *************************************************************************************
    private void Validar_Montos_Facturas()
    {
        Double Total_OC = 0; // Variable para asigar el valor Total de la OC
        Double Importe_Facturas_OC = 0; // Variable utilizada para guardar el monto total de las Facturas
        Double Diferencia = 0;
        try
        {  
            if (Lbl_Total.Text.Trim() != "0.00")
                Total_OC = Convert.ToDouble(Lbl_Total.Text.Trim()); // Se asigna el Total de la OC
            // Se recorre el Grid para optener el monto total de las facturas
            for (int h = 0; h < Grid_Facturas.Rows.Count; h++)
            {
                TextBox Txt_Importe_Factura = (TextBox)Grid_Facturas.Rows[h].FindControl("Txt_Importe_Factura");

                if (Txt_Importe_Factura.Text != "")
                {
                    Double Importe = Convert.ToDouble(Txt_Importe_Factura.Text.Trim());
                    Importe_Facturas_OC = Importe_Facturas_OC + Importe;
                }
            }

            if (Importe_Facturas_OC != Total_OC) // Si el Importe de las Facturas es Distinto al Importe de la Orden de Compra se muestra un mensaje
            {
                Diferencia = Math.Round((Importe_Facturas_OC - Total_OC),3); // Se redondea a 3 decimales
                //Lbl_Error_Busqueda.Text = "Los Importes de las Facturas, no suman el Monto Total de la Orden de Compra";
                Div_Contenedor_Msj_Error.Visible = true;
                //Lbl_Error_Busqueda.Visible = true;
                Lbl_Msg_Importe_Fact.Text = "Diferencia, Monto Orden de Compra " + "  " + Diferencia;
                Lbl_Msg_Importe_Fact.Visible = true;
            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Msg_Importe_Fact.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
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
            Botones.Add(Btn_Guardar_Contra_Recibo);
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

   

    
}
