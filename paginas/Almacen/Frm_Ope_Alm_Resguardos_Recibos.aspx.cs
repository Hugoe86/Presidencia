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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Xml.Linq;
using Presidencia.Resguardos_Recibos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Reportes;
using System.Collections.Generic;
using Presidencia.Almacen_Resguardos.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;

public partial class paginas_Frm_Ope_Alm_Resguardos_Recibos : System.Web.UI.Page
{
    # region Variables Globales
    Cls_Ope_Alm_Resguardos_Recibos_Negocio Resguardo_Recibo;
    #endregion


    # region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Estatus_Inicial();
        }
    }
    #endregion


    # region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:          Botón utilizado para salir de la aplicación o para configurar la pagina en su estado inicial
    ///PARAMETROS: 
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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
    //protected void Btn_Resguardar_Click(object sender, ImageClickEventArgs e)
    //{

    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN:          Botón utilizado para buscar las ordenes de compra
    ///PARAMETROS: 
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Cargar_Ordenes_Compra();
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Seleccionar_Orden_Compra_Click
    ///DESCRIPCIÓN:     En este botón se  asignan los datos del resguardante al resguardo
    ///PARAMETROS: 
    ///CREO:            Salvador Hernández Ramírez
    ///FECHA_CREO:      19/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Seleccionar_Orden_Compra_Click(object sender, ImageClickEventArgs e)
    {
        // Declaración de Objetos y Variables
        ImageButton Btn_Selec_Requisicion = null;
        Resguardo_Recibo = new Cls_Ope_Alm_Resguardos_Recibos_Negocio(); // Se crea el objeto para el manejo de los m{etodos
        String No_Orden_Compra = String.Empty;
        String No_Contra_Recibo = String.Empty;
        DataTable Dt_Produtos_OC = new DataTable();
        DataTable Dt_Ordenes_Compra = new DataTable();
        DataRow[] Dr_Orden;

        try
        { 
            Btn_Selec_Requisicion = (ImageButton)sender;
            No_Orden_Compra = Btn_Selec_Requisicion.CommandArgument; // Se selecciona el No de Orden de Compra
            Session["NO_ORDEN_COMPRA"] = No_Orden_Compra.Trim(); // Se asigna a la variable de session

            if (Session["Dt_Ordenes_Compra"] != null) // Se carga la tabla con las Ordenes de Compra
                Dt_Ordenes_Compra = (DataTable)Session["Dt_Ordenes_Compra"];

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
                        Txt_Fecha_Surtido.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Convertida);
                    }
                    if (!string.IsNullOrEmpty(Dr_Orden[0]["NO_CONTRA_RECIBO"].ToString())){
                         No_Contra_Recibo = HttpUtility.HtmlDecode(Dr_Orden[0]["NO_CONTRA_RECIBO"].ToString().Trim());
                         Session["NO_CONTRA_RECIBO_RESG"]= No_Contra_Recibo.Trim();
                    }
                    if (!string.IsNullOrEmpty(Dr_Orden[0]["FOLIO_OC"].ToString()))
                        Txt_Orden_Compra.Text = HttpUtility.HtmlDecode(Dr_Orden[0]["FOLIO_OC"].ToString().Trim());

                    if (!string.IsNullOrEmpty(Dr_Orden[0]["FOLIO_REQ"].ToString()))
                        Txt_Requisicion.Text = HttpUtility.HtmlDecode(Dr_Orden[0]["FOLIO_REQ"].ToString().Trim());

                    if (!string.IsNullOrEmpty(Dr_Orden[0]["TOTAL"].ToString()))
                        Txt_Importe.Text = HttpUtility.HtmlDecode(Dr_Orden[0]["TOTAL"].ToString().Trim());

                    Consultar_Productos_Requisicion( No_Orden_Compra.Trim());
                    Div_Busqueda_Av.Visible = false;
                    Estatus_Inicial_Botones(true); 
                }
                Div_Detalles_Orden_Compra.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

   
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN:     En este botón se  asignan los datos del resguardante al resguardo
    ///PARAMETROS: 
    ///CREO:            Salvador Hernández Ramírez
    ///FECHA_CREO:      19/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        // Declaración de Objetos y Variables
        ImageButton Btn_Selec_Producto = null;
        Cls_Ope_Alm_Resguardos_Recibos_Negocio Resguardo_Recibo = new Cls_Ope_Alm_Resguardos_Recibos_Negocio(); // Se crea el objeto para el manejo de los métodos   
      
        String No_Inventario = String.Empty;
        DataTable Dt_Productos = new DataTable();
        DataTable Dt_Dts_Gnrls_OC = new DataTable();

        String Producto_ID = "";
        String Operacion = "";
        String Descripcion = "";
        String Producto = "";
        String No_Serie = "";
        String Material = "";

        String Color = "";
        String No_Registro = "";
        String Material_ID = "";
        String Color_ID = "";
        String Costo = "0";
        String No_Resguardo_Recibo= "";
        
        String Dependencia_ID = "";
        String Area_ID = "";
        String Responsable_ID = "";

        String Proveedor_ID = "";
        String No_Factura_Proveedor = "";
        String Fecha_Factura_Proveedor = "";

        String Marca_ID = ""; // Nuevos campos agregados
        String Modelo = "";
        String Garantia = "";

        DataRow[] Dr_Orden_Compra;

        try
        { 
            Dt_Productos = (DataTable)Session["Dt_Productos_RR"];

            Btn_Selec_Producto = (ImageButton)sender;
            No_Inventario = Btn_Selec_Producto.CommandArgument; // Se selecciona el No de Orden de Compra

            Dr_Orden_Compra = Dt_Productos.Select("NO_INVENTARIO='" + No_Inventario.Trim() + "'");

            if (Session["NO_CONTRA_RECIBO_RESG"] != null) // Se consultan los datos generales de la orden de compra
            {
                Resguardo_Recibo.P_No_Contra_Recibo = Session["NO_CONTRA_RECIBO_RESG"].ToString().Trim();
                Dt_Dts_Gnrls_OC = Resguardo_Recibo.Consultar_Datos_G_Ordenes_Compra();
            }

            if ((!string.IsNullOrEmpty(Dr_Orden_Compra[0]["DEPENDENCIA_ID"].ToString())) && (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["EMPLEADO_ID"].ToString())))
            {
                if (Dr_Orden_Compra.Length > 0)
                {
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["PRODUCTO_ID"].ToString()))
                        Producto_ID = Dr_Orden_Compra[0]["PRODUCTO_ID"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["OPERACION"].ToString()))
                        Operacion = Dr_Orden_Compra[0]["OPERACION"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["PRODUCTO"].ToString()))
                        Producto = Dr_Orden_Compra[0]["PRODUCTO"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["DESCRIPCION"].ToString()))
                        Descripcion = Dr_Orden_Compra[0]["DESCRIPCION"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["NO_SERIE"].ToString()))
                        No_Serie = Dr_Orden_Compra[0]["NO_SERIE"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["COLOR"].ToString()))
                        Color = Dr_Orden_Compra[0]["COLOR"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["MATERIAL"].ToString()))
                        Material = Dr_Orden_Compra[0]["MATERIAL"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["NO_REGISTRO"].ToString()))
                        No_Registro = Dr_Orden_Compra[0]["NO_REGISTRO"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["MATERIAL_ID"].ToString()))
                        Material_ID = Dr_Orden_Compra[0]["MATERIAL_ID"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["COLOR_ID"].ToString()))
                        Color_ID = Dr_Orden_Compra[0]["COLOR_ID"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["DEPENDENCIA_ID"].ToString()))
                        Dependencia_ID = Dr_Orden_Compra[0]["DEPENDENCIA_ID"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["AREA_ID"].ToString()))
                        Area_ID = Dr_Orden_Compra[0]["AREA_ID"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["EMPLEADO_ID"].ToString()))
                        Responsable_ID = Dr_Orden_Compra[0]["EMPLEADO_ID"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["COSTO"].ToString()))
                        Costo = Dr_Orden_Compra[0]["COSTO"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["GARANTIA"].ToString()))
                        Garantia = Dr_Orden_Compra[0]["GARANTIA"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["MODELO"].ToString()))
                        Modelo = Dr_Orden_Compra[0]["MODELO"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["MARCA_ID"].ToString()))
                        Marca_ID = Dr_Orden_Compra[0]["MARCA_ID"].ToString().Trim();

                    if (Dt_Dts_Gnrls_OC.Rows.Count > 0) // Se guardan los datos generales de la factura
                    {
                        if (!string.IsNullOrEmpty(Dt_Dts_Gnrls_OC.Rows[0]["PROVEEDOR_ID"].ToString()))
                            Proveedor_ID = Dt_Dts_Gnrls_OC.Rows[0]["PROVEEDOR_ID"].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Dts_Gnrls_OC.Rows[0]["NO_FACTURA_PROVEEDOR"].ToString()))
                            No_Factura_Proveedor = Dt_Dts_Gnrls_OC.Rows[0]["NO_FACTURA_PROVEEDOR"].ToString().Trim();
                        if (!string.IsNullOrEmpty(Dt_Dts_Gnrls_OC.Rows[0]["FECHA_FACTURA"].ToString()))
                        {
                            String Fecha = Dt_Dts_Gnrls_OC.Rows[0]["FECHA_FACTURA"].ToString().Trim();
                            DateTime Fecha_Convertida = Convert.ToDateTime(Fecha);
                            Fecha_Factura_Proveedor = String.Format("{0:dd/MMM/yyyy}", Fecha_Convertida);
                        }
                    }

                    // Se deshabilita el boton para poner el check
                    Grid_Productos.Rows[(Convert.ToInt32(No_Registro.ToString().Trim()))].Cells[0].Enabled = false;

                    // Se oculta el Boton para resguardar y se muestra el boton para imprimir
                    Grid_Productos.Rows[(Convert.ToInt32(No_Registro.ToString().Trim()))].FindControl("Btn_Imprimir").Visible = false; // Se pone no visible el boton para el resguardo
                    Grid_Productos.Rows[(Convert.ToInt32(No_Registro.ToString().Trim()))].FindControl("Btn_Imprimir_Resguardo").Visible = true; // Se pone no visible el boton para el resguardo

                    Resguardo_Recibo.P_Producto_ID = Producto_ID.Trim();

                    if (Txt_Requisicion.Text.Trim() != "")
                        Resguardo_Recibo.P_No_Requisicion = Txt_Requisicion.Text.Trim().Replace("RQ-","");
                    else
                        Resguardo_Recibo.P_No_Requisicion = "";

                    if (Txt_Orden_Compra.Text.Trim() != "")
                        Resguardo_Recibo.P_No_Orden_Compra = Txt_Orden_Compra.Text.Trim().Replace("OC-","");
                    else
                        Resguardo_Recibo.P_No_Orden_Compra = "";

                    Resguardo_Recibo.P_No_Inventario = No_Inventario.Trim();
                    Resguardo_Recibo.P_Operacion = Operacion.Trim();
                    Resguardo_Recibo.P_Producto = Producto.Trim();
                    Resguardo_Recibo.P_Descripcion = Descripcion.Trim();
                    Resguardo_Recibo.P_No_Serie = No_Serie.Trim();
                    Resguardo_Recibo.P_Color_ID = Color_ID.Trim();
                    Resguardo_Recibo.P_Material_ID = Material_ID.Trim();
                    Resguardo_Recibo.P_Area_ID = Area_ID.Trim();
                    Resguardo_Recibo.P_Unidad_Responsable_ID = Dependencia_ID.Trim();
                    Resguardo_Recibo.P_Responsable_ID = Responsable_ID.Trim();
                    Resguardo_Recibo.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado.Trim();
                    Resguardo_Recibo.P_Empleado_Almacen_ID = Cls_Sessiones.Empleado_ID.Trim();
                    Resguardo_Recibo.P_Proveedor_ID = Proveedor_ID.Trim();
                    Resguardo_Recibo.P_No_Factura = No_Factura_Proveedor.Trim();
                    Resguardo_Recibo.P_Fecha_Adquisicion = Fecha_Factura_Proveedor.Trim();
                    Resguardo_Recibo.P_Fecha_Inventario = ""+ DateTime.Now.ToString("dd/MMM/yyyy");
                    Resguardo_Recibo.P_Costo = Costo.Trim();
                    Resguardo_Recibo.P_Marca_ID = Marca_ID.Trim(); // Nuevos Campos que se agregaron
                    Resguardo_Recibo.P_Garantia = Garantia.Trim();
                    Resguardo_Recibo.P_Modelo = Modelo.Trim();

                    if (Txt_Observaciones.Text.Trim() != "") // Se verifica si hay observaciones
                    {
                        if (Txt_Observaciones.Text.Trim().Length < 250)
                        {
                            Resguardo_Recibo.P_Observaciones = Txt_Observaciones.Text.Trim();
                        }
                        else
                        {
                            Resguardo_Recibo.P_Observaciones = Txt_Observaciones.Text.Substring(0, 249);
                        }
                    }
                    else
                    {
                        Resguardo_Recibo.P_Observaciones = "";
                    }

                    if (Operacion == "RESGUARDO")
                        Resguardo_Recibo.P_Operacion = "RESGUARDO";
                    else if (Operacion == "RECIBO")
                        Resguardo_Recibo.P_Operacion = "RECIBO";

                    Session["OPERACION_BM"] = Resguardo_Recibo.P_Operacion;
                    String Bien_Mueble_ID_Rpt = Resguardo_Recibo.Alta_Resguardo_Recibo();

                        //No_Resguardo_Recibo = Resguardo_Recibo.Alta_Resguardo_Recibo();  // Se sa de alta el recibo o resguardo
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Bien_Mueble.P_Bien_Mueble_ID = Bien_Mueble_ID_Rpt.Trim();
                    Bien_Mueble = Bien_Mueble.Consultar_Detalles_Bien_Mueble();
                    Llenar_DataSet_Resguardos_Bienes(Bien_Mueble);


                        //Consultar_Datos_Resguardo_Recibo(No_Inventario.Trim()); // Se imprime el Resguardo o Recibo

                        Boolean Actualizar_OC = true; // Variable declarada para indicar si la orden de compra se actualiza a Resguardada ="SI"
                       
                        for (int g = 0; g < Grid_Productos.Rows.Count; g++)
                        {
                            if ((Grid_Productos.Rows[g].FindControl("Btn_Imprimir").Visible != false) & (Grid_Productos.Rows[g].FindControl("Btn_Imprimir_Resguardo").Visible != true))
                            {
                                Actualizar_OC = false;
                                g = Grid_Productos.Rows.Count;
                            }
                        }

                         // Si ya no hay productos por resguardar, se actualiza la orden de compra a Resguardada = "SI"
                        if (Actualizar_OC == true)
                        {
                            if (Session["NO_CONTRA_RECIBO_RESG"] != null)
                            Resguardo_Recibo.P_No_Contra_Recibo = Session["NO_CONTRA_RECIBO_RESG"].ToString().Trim();
                            Resguardo_Recibo.Actualizar_Orden_Compra();
                        }
                }
                    Div_Contenedor_Msj_Error.Visible=false;
            }
            else
            {
                Lbl_Informacion.Text = "Seleccionar Unidad y Responsable";
                Div_Contenedor_Msj_Error.Visible=true;
            }

          }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    //protected void Grid_Productos_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Unidad_Responsable_SelectedIndexChanged
    ///DESCRIPCIÓN:          Evento utilizado para llenar el combo de empleados al seleccionar la unidad responsable
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez.
    ///FECHA_CREO:           31/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Unidad_Responsable_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            String Unidad = Cmb_Unidad_Responsable.SelectedValue;
            Llenar_Combo_Empleados(Unidad);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Aceptar_Click
    ///DESCRIPCIÓN:          Evento de boton utilizado para agregar los resguardantes
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez.
    ///FECHA_CREO:           31/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Aceptar_Click(object sender, EventArgs e)
    {
        DataTable Dt_Productos = new DataTable();
        DataTable Dt_Productos_Resguardar = new DataTable();

        Dt_Productos = (DataTable)Session["Dt_Productos_RR"];

        try
        {
            if ((Cmb_Responsable.SelectedIndex != 0) & (Cmb_Unidad_Responsable.SelectedIndex != 0))
            {
                Boolean V_CkeckBox = false;
                for (int i = 0; i < Grid_Productos.Rows.Count; i++)
                {
                    CheckBox Chk_Producto = (CheckBox)Grid_Productos.Rows[i].FindControl("Chk_Producto");

                    if (Chk_Producto.Checked)
                    {
                        V_CkeckBox = true;
                        String Dependencia_ID = Cmb_Unidad_Responsable.SelectedItem.Value.Trim();
                        //String Area_ID= Cmb_Area.SelectedItem.Value.Trim();
                        String Empleado_ID = Cmb_Responsable.SelectedItem.Value.Trim();

                        if (Dependencia_ID != "")
                            Dt_Productos.Rows[i].SetField("DEPENDENCIA_ID", Dependencia_ID);

                        Dt_Productos.Rows[i].SetField("AREA_ID", "");
                        Dt_Productos.Rows[i].SetField("EMPLEADO_ID", Empleado_ID);

                        Chk_Producto.BackColor = System.Drawing.Color.SlateGray; // Se le agrega el color para saber que el check box ya ha sifo seleccionado
                        Chk_Producto.Checked = false; // Se le quita la palomita al check
                    }
                }

                Session["Dt_Productos_RR"] = Dt_Productos; // Nuevamente se asigna la tabla a la variable de session

                if (V_CkeckBox == false)
                {
                    Lbl_Informacion.Text = " Seleccionar el producto a resguardar";
                    Lbl_Informacion.Visible = true;
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Informacion.Text = " Seleccionar Unidad R. Área y Responsable";
                Lbl_Informacion.Visible = true;
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Producto_CheckedChanged
    ///DESCRIPCIÓN:          Evento de boton utilizado para agregar los resguardantes
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez.
    ///FECHA_CREO:           31/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Chk_Producto_CheckedChanged(object sender, EventArgs e)
    {
        Boolean Boton_Habilitado = false;

        for (int i = 0; i < Grid_Productos.Rows.Count; i++)
        {
            CheckBox Chk_Producto = (CheckBox)Grid_Productos.Rows[i].FindControl("Chk_Producto");

            if (Chk_Producto.Checked)
            {
                Boton_Habilitado = true;
            }
        }

        if (Boton_Habilitado == true)
            Btn_Aceptar.Enabled = true;
        else
            Btn_Aceptar.Enabled = false;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Producto_CheckedChanged
    ///DESCRIPCIÓN:          Se muestra en PDF el resguardo para reimprimirse
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez.
    ///FECHA_CREO:           31/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Resguardo_Click(object sender, ImageClickEventArgs e)
    {
        // Declaración de Objetos y Variables
        ImageButton Btn_Selec_Producto = null;
        String No_Inventario = ""; // Variable que contendra el No_Inventario
        String Operacion = "";
        try
        {
            Btn_Selec_Producto = (ImageButton)sender;
            No_Inventario = Btn_Selec_Producto.CommandArgument; // Se selecciona el No de Orden de Compra

            for (int i = 0; i < Grid_Productos.Rows.Count; i++)
            {
                if (Grid_Productos.Rows[i].Cells[2].Text.Contains(No_Inventario))
                {
                    Operacion = Grid_Productos.Rows[i].Cells[8].Text.Trim();
                    break;
                }                
            }
            //ScriptManager.RegisterStartupScript(
            //    this, this.GetType(), "Requisiciones", "alert('Ok: " + No_Inventario  + ", " + Operacion + "');", true);

            Cls_Ope_Alm_Resguardos_Recibos_Negocio Negocio = new Cls_Ope_Alm_Resguardos_Recibos_Negocio();
            Negocio.P_Operacion = Operacion;
            Negocio.P_No_Inventario = No_Inventario;
            String Bien_Mueble_ID_Rpt = Negocio.Consulta_Datos_Reimpresion_Resguardo_Recibo();
            Session["OPERACION_BM"] = Operacion;
            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
            Bien_Mueble.P_Bien_Mueble_ID = Bien_Mueble_ID_Rpt.Trim();
            Bien_Mueble = Bien_Mueble.Consultar_Detalles_Bien_Mueble();
            Llenar_DataSet_Resguardos_Bienes(Bien_Mueble);               
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Fecha_B_CheckedChanged
    ///DESCRIPCIÓN:          Evento utilizado para establecer la fecha de búsqueda 
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez.
    ///FECHA_CREO:           30-Julio-11 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Click
    ///DESCRIPCIÓN:          Botón utilizado para configurar el estatus inicial de la búsqueda avanzada
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez.
    ///FECHA_CREO:           30-Julio-11 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Estado_Inicial_Busqueda_Avanzada();
    }


    #endregion


    # region  Métodos

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
            Txt_Busqueda.Text = "";
            Txt_Req_Buscar.Text = "";
            Chk_Fecha_B.Checked = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///                      2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///                      3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataTable Data_Set_Consulta_DB, DataSet Ds_Reporte, String Formato, String Tmp)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataRow Renglon;

        try
        {
            Renglon = Data_Set_Consulta_DB.Rows[0];
            Ds_Reporte.Tables[1].ImportRow(Renglon);
            
            for (int Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_DB.Rows.Count; Cont_Elementos++)
            {
                Renglon = Data_Set_Consulta_DB.Rows[Cont_Elementos];
                Ds_Reporte.Tables[0].ImportRow(Renglon);
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Resguardos_Bienes_Almacen.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Resguardo_B_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al llenar el DataSet. Error: [" + Ex.Message + "]");
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


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Inicial
    ///DESCRIPCIÓN:          Se establece el estatus inicial de la página
    ///PARAMETROS: 
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Estatus_Inicial()
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Div_Ordenes_Compra.Visible = false;
        Div_Detalles_Orden_Compra.Visible = false;

        try
        {      
            Estatus_Inicial_Botones(false);
            Cargar_Ordenes_Compra();
            Llenar_Combo_Dependencias();
            //Llenar_Combo_Areas();
            Div_Busqueda_Av.Visible = true; // Se muestra el panel que contiene la busqueda abanzada
           
            if (Cmb_Responsable.SelectedIndex != 0)
                Cmb_Responsable.SelectedIndex = 0;

              }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Ordenes_Compra
    ///DESCRIPCIÓN:     Se consultan las ordenes de compra
    ///PARAMETROS: 
   
    ///CREO:            Salvador Hernández Ramírez
    ///FECHA_CREO:      19/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ordenes_Compra()
    {
        Resguardo_Recibo = new Cls_Ope_Alm_Resguardos_Recibos_Negocio();
        DataTable Dt_Ordenes_Compra = new DataTable();

        try
        { 
                if (Txt_Busqueda.Text.Trim() != "")
                    Resguardo_Recibo.P_No_Orden_Compra = Txt_Busqueda.Text.Trim();
                else
                    Resguardo_Recibo.P_No_Orden_Compra = null;

                if (Txt_Req_Buscar.Text.Trim() != "")
                    Resguardo_Recibo.P_No_Requisicion = Txt_Req_Buscar.Text.Trim();
                else
                    Resguardo_Recibo.P_No_Requisicion = null;

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
                                    Resguardo_Recibo.P_Fecha_Inicio_B = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                                    Resguardo_Recibo.P_Fecha_Fin_B = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                                    Div_Contenedor_Msj_Error.Visible = false;
                                }
                                else
                                {
                                    String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                    Resguardo_Recibo.P_Fecha_Inicio_B = Fecha;
                                    Resguardo_Recibo.P_Fecha_Fin_B = Fecha;
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
                Dt_Ordenes_Compra = Resguardo_Recibo.Consultar_Ordenes_Compra();

                if (Dt_Ordenes_Compra.Rows.Count > 0) // Si la tabla contiene datos
                {
                    Session["Dt_Ordenes_Compra"] = Dt_Ordenes_Compra;

                    Grid_Ordenes_Compra.Columns[7].Visible = true; 
                    Grid_Ordenes_Compra.Columns[8].Visible = true; // No_Contra Recibo
                    Grid_Ordenes_Compra.DataSource = Dt_Ordenes_Compra;
                    Grid_Ordenes_Compra.DataBind();
                    Grid_Ordenes_Compra.Columns[7].Visible = false;
                    Grid_Ordenes_Compra.Columns[8].Visible = false; // No Contra Recibo
                    Div_Ordenes_Compra.Visible = true;
                    Div_Detalles_Orden_Compra.Visible = false;
                    Div_Contenedor_Msj_Error.Visible = false;
                }
                else
                {
                    Div_Detalles_Orden_Compra.Visible = false;
                    Div_Ordenes_Compra.Visible = false;
                    Lbl_Informacion.Text = " No se encontraron productos a resguardar";
                    Div_Contenedor_Msj_Error.Visible = true;
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


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Productos_Requisicion
    ///DESCRIPCIÓN:          Método utilizado para consultar los productos de la requisición
    ///PROPIEDADES:          No_Orden_Compra: El numero de la orden de compra
    ///CREO:                 Salvador Hernández Ramírez.
    ///FECHA_CREO:           29-Julio-11 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Productos_Requisicion( String No_Orden_Compra)
    {
        Resguardo_Recibo = new Cls_Ope_Alm_Resguardos_Recibos_Negocio();
        DataTable Dt_Productos = new DataTable();

        try
        {      
            Resguardo_Recibo.P_No_Orden_Compra = No_Orden_Compra.Trim();
            Dt_Productos = Resguardo_Recibo.Consultar_Productos_Requisicion();

            for (int i = 0; i < Dt_Productos.Rows.Count; i++)
            {
                String Resguardo = Dt_Productos.Rows[i]["RESGUARDO"].ToString().Trim();
                String Resibo = Dt_Productos.Rows[i]["RECIBO"].ToString().Trim();

                if (Resguardo == "SI")
                    Dt_Productos.Rows[i].SetField("OPERACION", "RESGUARDO");
                else if (Resibo == "SI")
                    Dt_Productos.Rows[i].SetField("OPERACION", "RECIBO");
            }

            if (Dt_Productos.Rows.Count > 0) // Si la tabla contiene datos
            {
                Grid_Productos.Columns[1].Visible = true;
                Grid_Productos.Columns[10].Visible = true;
                Grid_Productos.Columns[11].Visible = true;
                Grid_Productos.Columns[13].Visible = true; // Se pone visible el costo
                Grid_Productos.DataSource = Dt_Productos;
                Grid_Productos.DataBind();
                Grid_Productos.Columns[1].Visible = false;
                Grid_Productos.Columns[10].Visible = false;
                Grid_Productos.Columns[11].Visible = false;
                Grid_Productos.Columns[13].Visible = false; // Se oculta  el costo

                for (int i = 0; i < Grid_Productos.Rows.Count; i++) // Se agrega el numero de registro
                {
                    Dt_Productos.Rows[i].SetField("NO_REGISTRO",i); // Se agrega un numero de registro
                    String Resguardado = "";

                    if (Dt_Productos.Rows[i]["RESGUARDADO"].ToString().Trim() != "") // Se optiene el valor de la tabla para determinar si ya esta resgaurdado el bien
                        Resguardado = Dt_Productos.Rows[i]["RESGUARDADO"].ToString().Trim();

                    if (Resguardado == "SI")
                    {
                        Grid_Productos.Rows[i].Cells[0].Enabled = false; // Se deshabilita el registro donde esta el chech
                        Grid_Productos.Rows[i].FindControl("Btn_Imprimir").Visible = false;
                    }
                    else if ((Resguardado == "") | (Resguardado == "NO"))
                    {
                        Grid_Productos.Rows[i].Cells[0].Enabled = true; // Se deshabilita el registro donde esta el chech
                        Grid_Productos.Rows[i].FindControl("Btn_Imprimir_Resguardo").Visible = false;
                    }
                }

                Session["Dt_Productos_RR"] = Dt_Productos;  // Se agrega la tabla a la variable de session

                Div_Contenedor_Msj_Error.Visible = false;
                Grid_Productos.Visible = true;
                Div_Detalles_Orden_Compra.Visible = true;
                Div_Ordenes_Compra.Visible = false;
            }
            else
            {
                Lbl_Informacion.Text = " No se encontraron productos a resguardar";
                Div_Contenedor_Msj_Error.Visible = true;
                Div_Detalles_Orden_Compra.Visible = false;
                Div_Ordenes_Compra.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Dependencias
    ///DESCRIPCIÓN: Llena el combo de Dependencias.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 15/Marzo/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Dependencias()
    {
        //SE LLENA EL COMBO DE DEPENDENCIAS
        Resguardo_Recibo = new Cls_Ope_Alm_Resguardos_Recibos_Negocio();

        try
        {  
                Resguardo_Recibo.P_Tipo_Combo = "DEPENDENCIAS";
                DataTable Dependencias = Resguardo_Recibo.Llenar_Combo();
               
                if (Dependencias.Rows.Count > 0)
                {
                    DataRow Fila_Dependencia = Dependencias.NewRow();
                    Fila_Dependencia["DEPENDENCIA_ID"] = "SELECCIONE";
                    Fila_Dependencia["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                    Dependencias.Rows.InsertAt(Fila_Dependencia, 0);
                    Cmb_Unidad_Responsable.DataSource = Dependencias;
                    Cmb_Unidad_Responsable.DataValueField = "DEPENDENCIA_ID";
                    Cmb_Unidad_Responsable.DataTextField = "NOMBRE";
                    Cmb_Unidad_Responsable.DataBind();
                }
          }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
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
           
        }
        else
        {
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ToolTip = "Salir";
        }
    }

    /////DESCRIPCIÓN: Llena el combo de Dependencias.
    /////PROPIEDADES:     
    /////CREO: Francisco Antonio Gallardo Castañeda.
    /////FECHA_CREO: 15/Marzo/2010 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private void Llenar_Combo_Areas()
    //{
    //    //SE LLENA EL COMBO DE AREAS
    //    Resguardo_Recibo = new Cls_Ope_Alm_Resguardos_Recibos_Negocio();

    //    Resguardo_Recibo.P_Tipo_Combo = "AREAS";
    //    DataTable Areas = Resguardo_Recibo.Llenar_Combo();
    //    DataRow Fila_Area = Areas.NewRow();
    //    Fila_Area["AREA_ID"] = "SELECCIONE";
    //    Fila_Area["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
    //    Areas.Rows.InsertAt(Fila_Area, 0);
    //    //Cmb_Area.DataSource = Areas;
    //    //Cmb_Area.DataValueField = "AREA_ID";
    //    //Cmb_Area.DataTextField = "NOMBRE";
    //    //Cmb_Area.DataBind();
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados
    ///DESCRIPCIÓN: Llena el combo de Empleados.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Empleados( String Unidad_Responzable)
    {
        // SE LLENA EL COMBO DE DEPENDENCIAS
        Resguardo_Recibo = new Cls_Ope_Alm_Resguardos_Recibos_Negocio();

        try
        {
            Resguardo_Recibo.P_Unidad_Responsable_ID = Unidad_Responzable.Trim();
            Resguardo_Recibo.P_Tipo_Combo = "EMPLEADOS";
            DataTable Tabla = Resguardo_Recibo.Llenar_Combo();

            if (Tabla.Rows.Count > 0)
            {
                DataRow Fila_Empleado = Tabla.NewRow();
                Fila_Empleado["EMPLEADO_ID"] = "SELECCIONE";
                Fila_Empleado["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Tabla.Rows.InsertAt(Fila_Empleado, 0);
                Cmb_Responsable.DataSource = Tabla;
                Cmb_Responsable.DataValueField = "EMPLEADO_ID";
                Cmb_Responsable.DataTextField = "NOMBRE";
                Cmb_Responsable.DataBind();
            }
        }
        catch (Exception Ex)
        {
            //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            //Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    ///*******************************************************************************
    ///##############################################################################
    ///##############################################################################
    ///
    ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Resguardo_Recibo
    ///DESCRIPCIÓN:          Metodo utilizado para consultaro los datos generales del resguardo o recibo
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez.
    ///FECHA_CREO:           30-Julio-11 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Datos_Resguardo_Recibo(String No_Inventario)
    {
        Cls_Ope_Alm_Resguardos_Recibos_Negocio Resguardo_Recibo = new Cls_Ope_Alm_Resguardos_Recibos_Negocio(); // Se crea el objeto para el manejo de los métodos   
        String Operacion = "";
        String Producto_ID = "";

        String Resguardantes = "";
        String RFC = "";

        DataTable Dt_Resguardos_Recibos = new DataTable();
        DataTable Dt_Productos = new DataTable();
        DataRow[] Dr_Orden_Compra;

        Dt_Productos = (DataTable)Session["Dt_Productos_RR"]; // Se agrega la tabla

        try
        {
            Dr_Orden_Compra = Dt_Productos.Select("NO_INVENTARIO='" + No_Inventario.Trim() + "'");

            if (Dr_Orden_Compra.Length > 0)
            {
                if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["PRODUCTO_ID"].ToString()))
                    Producto_ID = Dr_Orden_Compra[0]["PRODUCTO_ID"].ToString().Trim();
                if (!string.IsNullOrEmpty(Dr_Orden_Compra[0]["OPERACION"].ToString()))
                    Operacion = Dr_Orden_Compra[0]["OPERACION"].ToString().Trim();
            }

            if (Operacion == "RESGUARDO")
                Resguardo_Recibo.P_Operacion = "RESGUARDO";
            else if (Operacion == "RECIBO")
                Resguardo_Recibo.P_Operacion = "RECIBO";

            Resguardo_Recibo.P_No_Inventario = No_Inventario.Trim();
            Dt_Resguardos_Recibos = Resguardo_Recibo.Consulta_Recibos_Resguardos();  // Se consulta el recibo o resguardo

            if (Operacion == "RESGUARDO")
                Dt_Resguardos_Recibos.Rows[0].SetField("OPERACION", "CONTROL DE RESGUARDOS DE BIENES MUEBLES");
            else if (Operacion == "RECIBO")
                Dt_Resguardos_Recibos.Rows[0].SetField("OPERACION", "CONTROL DE RECIBOS DE BIENES MUEBLES");

            if ((string.IsNullOrEmpty(Dt_Resguardos_Recibos.Rows[0]["MARCA"].ToString())) | (Dt_Resguardos_Recibos.Rows[0]["MARCA"].ToString().Trim() == ""))
                Dt_Resguardos_Recibos.Rows[0].SetField("MARCA", "INDISTINTA");

            if (string.IsNullOrEmpty(Dt_Resguardos_Recibos.Rows[0]["MODELO"].ToString()))
                Dt_Resguardos_Recibos.Rows[0].SetField("MODELO", "INDISTINTO");

            if (!string.IsNullOrEmpty(Dt_Resguardos_Recibos.Rows[0]["RESGUARDANTES"].ToString()))
                Resguardantes = Dt_Resguardos_Recibos.Rows[0]["RESGUARDANTES"].ToString().Trim();

            if (!string.IsNullOrEmpty(Dt_Resguardos_Recibos.Rows[0]["RFC"].ToString()))
                RFC = Dt_Resguardos_Recibos.Rows[0]["RFC"].ToString().Trim();

            Resguardantes = Resguardantes + " (" + RFC + ")";
            Dt_Resguardos_Recibos.Rows[0].SetField("RESGUARDANTES", Resguardantes);

            String Formato = "PDF";
            Ds_Alm_Com_Resguardos_Bienes Ds_Consulta_Resguardos_Bienes = new Ds_Alm_Com_Resguardos_Bienes();
            Generar_Reporte(Dt_Resguardos_Recibos, Ds_Consulta_Resguardos_Bienes, Formato);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    

    #endregion







    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Resguardos_Bienes()
    ///DESCRIPCIÓN: Llena el dataSet "Data_Set_Resguardos_Bienes" con las personas a las que se les asigno el
    ///bien mueble y sus detalles, para que con estos datos se genere el reporte.
    ///PARAMETROS:  
    ///CREO: Salvador Hernández Ramírez
    ///FECHA_CREO: 17/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_DataSet_Resguardos_Bienes(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Id)
    {
        String Formato = "PDF";
        Cls_Alm_Com_Resguardos_Negocio Consulta_Resguardos_Negocio = new Cls_Alm_Com_Resguardos_Negocio();
        Bien_Id.P_Producto_Almacen = false;
        DataTable Data_Set_Resguardos_Bienes;
        Consulta_Resguardos_Negocio.P_Operacion = Bien_Id.P_Operacion.Trim();

        if (Session["OPERACION_BM"] != null)
            Consulta_Resguardos_Negocio.P_Operacion = Session["OPERACION_BM"].ToString().Trim();

        Data_Set_Resguardos_Bienes = Consulta_Resguardos_Negocio.Consulta_Resguardos_Bienes(Bien_Id);

        Ds_Alm_Com_Resguardos_Bienes Ds_Consulta_Resguardos_Bienes = new Ds_Alm_Com_Resguardos_Bienes();
        Generar_Reporte(Data_Set_Resguardos_Bienes, Ds_Consulta_Resguardos_Bienes, Formato);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///                      2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///                      3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataTable Data_Set_Consulta_DB, DataSet Ds_Reporte, String Formato)
    {

        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataRow Renglon;

        try
        {
            Renglon = Data_Set_Consulta_DB.Rows[0];
            String Cantidad = Data_Set_Consulta_DB.Rows[0]["CANTIDAD"].ToString();
            String Costo = Data_Set_Consulta_DB.Rows[0]["COSTO_UNITARIO"].ToString();
            Double Resultado = (Convert.ToDouble(Cantidad)) * (Convert.ToDouble(Costo));
            Ds_Reporte.Tables[1].ImportRow(Renglon);
            Ds_Reporte.Tables[1].Rows[0].SetField("COSTO_TOTAL", Resultado);

            if (Session["OPERACION_BM"] != null)
            {
                if (Session["OPERACION_BM"].ToString().Trim() == "RESGUARDO")
                    Ds_Reporte.Tables[1].Rows[0].SetField("OPERACION", "CONTROL DE RESGUARDOS DE BIENES MUEBLES");

                else if (Session["OPERACION_BM"].ToString().Trim() == "RECIBO")
                    Ds_Reporte.Tables[1].Rows[0].SetField("OPERACION", "CONTROL DE RECIBOS DE BIENES MUEBLES");

                Session.Remove("OPERACION_BM");
            }
            if ((string.IsNullOrEmpty(Ds_Reporte.Tables[1].Rows[0]["MARCA"].ToString())) | (Ds_Reporte.Tables[1].Rows[0]["MARCA"].ToString().Trim() == ""))
                Ds_Reporte.Tables[1].Rows[0].SetField("MARCA", "INDISTINTA");

            if (string.IsNullOrEmpty(Ds_Reporte.Tables[1].Rows[0]["MODELO"].ToString()))
                Ds_Reporte.Tables[1].Rows[0].SetField("MODELO", "INDISTINTO");



            for (int Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_DB.Rows.Count; Cont_Elementos++)
            {
                Renglon = Data_Set_Consulta_DB.Rows[Cont_Elementos];
                Ds_Reporte.Tables[0].ImportRow(Renglon);
                String Nombre_E = Data_Set_Consulta_DB.Rows[Cont_Elementos]["NOMBRE_E"].ToString();
                String Apellido_Paterno_E = Data_Set_Consulta_DB.Rows[Cont_Elementos]["APELLIDO_PATERNO_E"].ToString();
                String Apellido_Materno_E = Data_Set_Consulta_DB.Rows[Cont_Elementos]["APELLIDO_MATERNO_E"].ToString();
                String RFC_E = Data_Set_Consulta_DB.Rows[Cont_Elementos]["RFC_E"].ToString();
                String Resguardante = Nombre_E + " " + Apellido_Paterno_E + " " + Apellido_Materno_E + " " + "(" + RFC_E + ")";
                Ds_Reporte.Tables[0].Rows[Cont_Elementos].SetField("RESGUARDANTES", Resguardante);
            }


            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Resguardos_Bienes_Almacen.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte;

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = "Rpt_Alm_Com_Resguardos_Bienes_Almacen.pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = "Rpt_Alm_Com_Resguardos_Bienes_Almacen.xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al llenar el DataSet. Error: [" + Ex.Message + "]");
        }
    }

    protected void Btn_Resguardar_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void Grid_Productos_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
