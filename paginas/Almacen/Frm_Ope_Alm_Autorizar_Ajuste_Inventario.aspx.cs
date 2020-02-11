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
using Presidencia.Autorizar_Ajuste.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Ajustar_Stock.Negocio;
public partial class paginas_Almacen_Frm_Ope_Alm_Autorizar_Ajuste_Inventario : System.Web.UI.Page
{
    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configurar_Formulario("Inicio");
            Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio Clase_Negocio = new Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio();
            Llenar_Grid_Ajustes(Clase_Negocio);
        }
    }
    #endregion

    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    #region Metodos

    public void Configurar_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicio":
                Div_Busqueda_Avanzada.Visible = true;
                Div_Grid_Ajustes_Inv.Visible = true;
                Div_Datos_Ajuste.Visible = false;
                //Boton Modificar
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");

                break;
            case "General":
                Div_Busqueda_Avanzada.Visible = false;
                Div_Grid_Ajustes_Inv.Visible = false;
                Div_Datos_Ajuste.Visible = true;
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                break;
            case "Modificar":
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                //Habilitamos Componentes
                Cmb_Estatus.Enabled = true;
                Txt_Motivo_Ajuste_Dir.Enabled = true;
                Txt_Motivo_Ajuste_Dir.ReadOnly = false;

                break;

        }
    }


    public void Limpiar_Formulario()
    {
        Txt_No_Ajuste.Text = "";
        Txt_Fecha_Hora.Text ="";
        Txt_Empleado_Genero.Text = "";
        Txt_Motivo_Ajuste_Coordinador.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Fecha_Autorizacion.Text ="";
        Txt_Fecha_Rechazo.Text = "";
        Txt_Empleado_Autorizo.Text = "";
        Txt_Empleado_Rechazo.Text = "";
        Txt_Motivo_Ajuste_Dir.Text = "";
        Lbl_Entradas_Producto.Text ="";
        Lbl_Entradas_Unidad.Text = "";
        Lbl_Importe_Entradas.Text = "";
        Lbl_Salidas_Producto.Text = "";
        Lbl_Salidas_Unidad.Text = "";
        Lbl_Importe_Salidas.Text = "";
        Lbl_Producto_Ajustado.Text = "";
        Lbl_Producto_Ajustado.Text = "";
        Lbl_Unidades_Ajustadas.Text = "";
        Lbl_Importe_Saldo.Text = "";
        Session["Dt_Productos_Ajuste"] = null;
        Session["Dt_Ajuste_Detalle"] = null;
    }

    public void Calcular_Movimientos_Ajuste()
    {
        //Obtenemos el DAtatable que contiene los detalles del ajuste 
        if(Session["Dt_Productos_Ajuste"] != null)
        {
        DataTable Dt_Productos_Ajuste = (DataTable)Session["Dt_Productos_Ajuste"];
        int Entradas_Producto = 0;
        int Salidas_Producto = 0;
        int Total_Producto = 0;
        int Entradas_Unidad = 0;
        int Salidas_Unidad = 0;
        int Total_Unidad = 0;
        double Entradas_Importe = 0;
        double Salidas_Importe = 0;
        double Total_Importe = 0;

        for (int i = 0; i < Dt_Productos_Ajuste.Rows.Count; i++ )
        {

            if (Dt_Productos_Ajuste.Rows[i][Ope_Alm_Ajustes_Detalles.Campo_Tipo_Movimiento].ToString().Trim()== "ENTRADA")
            {
                Entradas_Producto = Entradas_Producto + 1;
                Entradas_Unidad = Entradas_Unidad + int.Parse(Dt_Productos_Ajuste.Rows[i][Ope_Alm_Ajustes_Detalles.Campo_Diferencia].ToString().Trim());
                Entradas_Importe = Entradas_Importe + double.Parse(Dt_Productos_Ajuste.Rows[i][Ope_Alm_Ajustes_Detalles.Campo_Importe_Diferencia].ToString().Trim());


            }
            if (Dt_Productos_Ajuste.Rows[i][Ope_Alm_Ajustes_Detalles.Campo_Tipo_Movimiento].ToString().Trim() == "SALIDA")
            {
                Salidas_Producto = Salidas_Producto + 1;
                Salidas_Unidad = Salidas_Unidad + int.Parse(Dt_Productos_Ajuste.Rows[i][Ope_Alm_Ajustes_Detalles.Campo_Diferencia].ToString().Trim());
                Salidas_Importe = Salidas_Importe + double.Parse(Dt_Productos_Ajuste.Rows[i][Ope_Alm_Ajustes_Detalles.Campo_Importe_Diferencia].ToString().Trim());
            }
        }//Fin del for
        //Asignamos los valores de las entradas y salidas a las cajas de texto
        Lbl_Entradas_Producto.Text = Entradas_Producto.ToString();
        Lbl_Entradas_Unidad.Text = Entradas_Unidad.ToString();
        Lbl_Importe_Entradas.Text = Entradas_Importe.ToString();
        Lbl_Salidas_Producto.Text = Salidas_Producto.ToString();
        Lbl_Salidas_Unidad.Text = Salidas_Unidad.ToString();
        Lbl_Importe_Salidas.Text = Salidas_Importe.ToString();
        Lbl_Producto_Ajustado.Text = Total_Producto.ToString();
        //Calculamos el Ajuste del Producto
        Total_Producto = Entradas_Producto - Salidas_Producto;
        //Obtenemos el Total Por UNIDAD
        Total_Unidad = Entradas_Unidad - Salidas_Unidad;
        //Onbtenemos el Total por Importe
        Total_Importe = Entradas_Importe - Salidas_Importe;
        //Asignamos los totales a los Label
        Lbl_Producto_Ajustado.Text = Total_Producto.ToString();
        Lbl_Unidades_Ajustadas.Text = Total_Unidad.ToString();
        Lbl_Importe_Saldo.Text = Total_Importe.ToString();


        }//Fin del if
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Nombre_PDF)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Almacen/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set_Consulta_DB;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/" + Nombre_PDF;
        Mostrar_Reporte(Nombre_PDF, "PDF");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
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
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///PARAMETROS: 1.-TextBox Fecha_Inicial 
    ///            2.-TextBox Fecha_Final
    ///            3.-Label Mensaje_Error
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Verificar_Fecha(Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio Clase_Negocio)
    {

        //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        bool Fecha_Valida = true;

        if ((Txt_Fecha_Inicial.Text.Length == 11) && (Txt_Fecha_Hora.Text.Length == 11))
        {
            //Convertimos el Texto de los TextBox fecha a dateTime
            Date1 = DateTime.Parse(Txt_Fecha_Inicial.Text);
            Date2 = DateTime.Parse(Txt_Fecha_Final.Text);
            //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
            if ((Date1 < Date2) | (Date1 == Date2))
            {
                //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                Clase_Negocio.P_Fecha_Inicio = Formato_Fecha(Txt_Fecha_Inicial.Text);
                Clase_Negocio.P_Fecha_Fin = Formato_Fecha(Txt_Fecha_Final.Text);

            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
               Lbl_Mensaje_Error.Text += "+ Fecha no valida <br />";
            }
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Fecha no valida <br />";
        }

        return Fecha_Valida;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN: Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:  1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                     en caso de que cumpla la condicion del if
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 2/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {

        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica el es a solo mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }// fin de Formato_Fecha

    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    public void Llenar_Grid_Ajustes(Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio Clase_Negocio)
    {
        DataTable Dt_Ajustes_Inv = Clase_Negocio.Consultar_Ajustes_Inventario();
        if (Dt_Ajustes_Inv.Rows.Count != 0)
        {
            Grid_Ajustes_Inv.DataSource = Dt_Ajustes_Inv;
            Grid_Ajustes_Inv.DataBind();
            Session["Dt_Ajustes_Inv"] = Dt_Ajustes_Inv;
        }
        else
        {
            Grid_Ajustes_Inv.EmptyDataText = "No se encontrado registros.";
            //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            Grid_Ajustes_Inv.DataSource = new DataTable();
            Grid_Ajustes_Inv.DataBind();
        }

    }

    public void Llenar_Grid_Productos(Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio Clase_Negocio)
    {
        DataTable Dt_Productos_Ajuste = Clase_Negocio.Consultar_Detalle_Ajustes();
        if (Dt_Productos_Ajuste.Rows.Count != 0)
        {
            Grid_Productos.DataSource = Dt_Productos_Ajuste;
            Grid_Productos.DataBind();
            Session["Dt_Productos_Ajuste"] = Dt_Productos_Ajuste;
        }
        else
        {
            Grid_Productos.EmptyDataText = "No se encontrado registros.";
            //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            Grid_Productos.DataSource = new DataTable();
            Grid_Productos.DataBind();
        }
        
    }

    protected void Grid_Ajustes_Inv_SelectedIndexChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        //creamos la instancia de la clase de negocios
        Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio Clase_Negocio = new Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio();
        Clase_Negocio.P_No_Ajuste = Grid_Ajustes_Inv.SelectedDataKey["No_Ajuste"].ToString();
        DataTable Dt_Ajuste_Detalle = Clase_Negocio.Consultar_Ajustes_Inventario();
        Session["Dt_Ajuste_Detalle"] = Dt_Ajuste_Detalle;
        Txt_No_Ajuste.Text = Dt_Ajuste_Detalle.Rows[0][Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste].ToString().Trim();
        Txt_Fecha_Hora.Text = Dt_Ajuste_Detalle.Rows[0][Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Hora].ToString().Trim();
        Txt_Empleado_Genero.Text = Dt_Ajuste_Detalle.Rows[0][Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Elaboro_ID].ToString().Trim();
        Txt_Motivo_Ajuste_Coordinador.Text = Dt_Ajuste_Detalle.Rows[0][Ope_Alm_Ajustes_Inv_Stock.Campo_Motivo_Ajuste_Coor].ToString().Trim();
        Cmb_Estatus.SelectedValue = Dt_Ajuste_Detalle.Rows[0][Ope_Alm_Ajustes_Inv_Stock.Campo_Estatus].ToString().Trim();
        Txt_Fecha_Autorizacion.Text = Dt_Ajuste_Detalle.Rows[0][Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Autorizo].ToString().Trim();
        Txt_Fecha_Rechazo.Text = Dt_Ajuste_Detalle.Rows[0][Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Rechazo].ToString().Trim();
        Txt_Empleado_Autorizo.Text = Dt_Ajuste_Detalle.Rows[0][Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Autorizo_ID].ToString().Trim();
        Txt_Empleado_Rechazo.Text = Dt_Ajuste_Detalle.Rows[0][Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Rechazo_ID].ToString().Trim(); 
        Txt_Motivo_Ajuste_Dir.Text = Dt_Ajuste_Detalle.Rows[0][Ope_Alm_Ajustes_Inv_Stock.Campo_Motivo_Ajuste_Dir].ToString().Trim();
        Configurar_Formulario("General");
        Llenar_Grid_Productos(Clase_Negocio);
        //Calculamos los Totales Por Producto, Por Unidad y Por Importe
        Calcular_Movimientos_Ajuste();


    }

    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Modificar.ToolTip)
        {
            case "Modificar":
                if (Txt_No_Ajuste.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario seleccionar un Ajuste";
                }

                if (Cmb_Estatus.SelectedValue != "GENERADO")
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se puede Modificar el Ajuste";
                }

                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    Configurar_Formulario("Modificar");


                }
                break;
            case "Actualizar":

                if ((Cmb_Estatus.SelectedValue != "GENERADO") && (Txt_Motivo_Ajuste_Dir.Text.Trim() == String.Empty))
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario indicar el Comentario de quien " + Cmb_Estatus.SelectedValue;

                }

                if ((Cmb_Estatus.SelectedValue == "GENERADO") && (Txt_Motivo_Ajuste_Dir.Text.Trim() != String.Empty))
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se guardara El comentario pues el estatus no es AUTORIZADO ó RECHAZADO " + Cmb_Estatus.SelectedValue;
                }

                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio Clase_Negocio= new Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio();
                    Clase_Negocio.P_No_Ajuste = Txt_No_Ajuste.Text.Trim();
                    Clase_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;

                    if (Cmb_Estatus.SelectedValue != "GENERADO")
                    {
                        if (Txt_Empleado_Autorizo.Text != String.Empty)
                        {
                            Clase_Negocio.P_Empleado_Autorizo_ID = Cls_Sessiones.Empleado_ID;

                        }
                        if (Txt_Empleado_Rechazo.Text != String.Empty)
                        {
                            Clase_Negocio.P_Empleado_Rechazo_ID = Cls_Sessiones.Empleado_ID;
                        }

                        Clase_Negocio.P_Motivo_Ajuste_Dir = Txt_Motivo_Ajuste_Dir.Text;
                        //Aqui se modifica el estatus del ajuste de Inventario
                        bool Operacion_Realizada = Clase_Negocio.Modificar_Ajuste_Inventario();
                        //Aqui hace los cambios a las EXISTENCIAS y DISPONIBLE en Cat_Com_productos
                        if (Clase_Negocio.P_Estatus == "AUTORIZADO")
                        {
                            Cls_Ope_Alm_Ajustar_Stock_Negocio Negocio = new Cls_Ope_Alm_Ajustar_Stock_Negocio();
                            Negocio.P_No_Ajuste = Txt_No_Ajuste.Text.Trim();
                            Negocio.Aplicar_Ajuste_Inventario();
                        }
                        if (Operacion_Realizada)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Autorizar Ajuste Inventario", "alert('Se modifico el Ajuste de Inventario Exitosamente');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Autorizar Ajuste Inventario", "alert('No se realizo la Operacion');", true);
                        }
                        //Deshabilitamos el combo y la caja de texto de Comentarios
                        Configurar_Formulario("General");
                        Cmb_Estatus.Enabled = false;
                        Txt_Motivo_Ajuste_Dir.Enabled = false;

                    }

                }
                break;

        }//Fin del switch
    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio Clase_Negocio = new Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio();
        switch (Btn_Salir.ToolTip)
        {
            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                Limpiar_Formulario();
                break;
            case "Listado":
                Configurar_Formulario("Inicio");
                Limpiar_Formulario();
                Llenar_Grid_Ajustes(Clase_Negocio);
               

                break;
            case "Cancelar":
                Configurar_Formulario("Inicio");
                Limpiar_Formulario();
                Llenar_Grid_Ajustes(Clase_Negocio);
                
                break;
        }
    }

    protected void Cmb_Estatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Estatus.SelectedValue == "GENERADO")
        {
            Txt_Empleado_Autorizo.Text = "";
            Txt_Fecha_Autorizacion.Text = "";
            Txt_Empleado_Rechazo.Text = "";
            Txt_Fecha_Rechazo.Text = "";
        }

        if(Cmb_Estatus.SelectedValue == "AUTORIZADO")
        {
            Txt_Empleado_Autorizo.Text = Cls_Sessiones.Nombre_Empleado;
            Txt_Fecha_Autorizacion.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Empleado_Rechazo.Text = "";
            Txt_Fecha_Rechazo.Text = "";

        }

        if (Cmb_Estatus.SelectedValue == "CANCELADO")
        {
            Txt_Empleado_Rechazo.Text = Cls_Sessiones.Nombre_Empleado;
            Txt_Fecha_Rechazo.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Empleado_Autorizo.Text = "";
            Txt_Fecha_Autorizacion.Text = "";
        }
    }


    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if(Txt_No_Ajuste.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario Seleccionar un Ajuste de Inventario";
        }

        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            //reALIZAMOS LA CONSULTA PARA TRAERNOS LOS DATOS QUE SE GUARDARON
            Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio Clase_Negocio = new Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio();
            Clase_Negocio.P_No_Ajuste = Txt_No_Ajuste.Text.Trim();
            DataTable Dt_Ajuste_Detalle = Clase_Negocio.Consultar_Ajustes_Inventario();

            //Preparamos el Dataset con los dos datatable para imprimir
            DataSet Ds_Imprimir_Ajuste = new DataSet();
           
            Dt_Ajuste_Detalle.Columns.Add("ENTRADA_PRODUCTO", typeof(System.String));
            Dt_Ajuste_Detalle.Columns.Add("SALIDA_PRODUCTO", typeof(System.String));
            Dt_Ajuste_Detalle.Columns.Add("PRODUCTO_AJUSTADO", typeof(System.String));
            Dt_Ajuste_Detalle.Columns.Add("ENTRADA_UNIDAD", typeof(System.String));
            Dt_Ajuste_Detalle.Columns.Add("SALIDA_UNIDAD", typeof(System.String));
            Dt_Ajuste_Detalle.Columns.Add("UNIDADES_AJUSTADAS", typeof(System.String));
            Dt_Ajuste_Detalle.Columns.Add("IMPORTE_ENTRADA", typeof(System.Double));
            Dt_Ajuste_Detalle.Columns.Add("IMPORTE_SALIDA", typeof(System.Double));
            Dt_Ajuste_Detalle.Columns.Add("IMPORTE_SALDO", typeof(System.Double));
            Dt_Ajuste_Detalle.Rows[0]["ENTRADA_PRODUCTO"] = Lbl_Entradas_Producto.Text.Trim();
            Dt_Ajuste_Detalle.Rows[0]["SALIDA_PRODUCTO"] = Lbl_Salidas_Producto.Text.Trim();
            Dt_Ajuste_Detalle.Rows[0]["PRODUCTO_AJUSTADO"] = Lbl_Producto_Ajustado.Text.Trim();

            Dt_Ajuste_Detalle.Rows[0]["ENTRADA_UNIDAD"] = Lbl_Entradas_Unidad.Text.Trim();
            Dt_Ajuste_Detalle.Rows[0]["SALIDA_UNIDAD"] = Lbl_Salidas_Unidad.Text.Trim();
            Dt_Ajuste_Detalle.Rows[0]["UNIDADES_AJUSTADAS"] = Lbl_Unidades_Ajustadas.Text.Trim();

            Dt_Ajuste_Detalle.Rows[0]["IMPORTE_ENTRADA"] = double.Parse(Lbl_Importe_Entradas.Text.Trim());
            Dt_Ajuste_Detalle.Rows[0]["IMPORTE_SALIDA"] = double.Parse(Lbl_Importe_Salidas.Text.Trim());
            Dt_Ajuste_Detalle.Rows[0]["IMPORTE_SALDO"] = double.Parse(Lbl_Importe_Saldo.Text.Trim());


            DataTable Dt_Productos_Ajuste = (DataTable)Session["Dt_Productos_Ajuste"];

            Ds_Imprimir_Ajuste.Tables.Add(Dt_Ajuste_Detalle.Copy());
            Ds_Imprimir_Ajuste.Tables[0].TableName = "Dt_Ajuste_Detalle";
            Ds_Imprimir_Ajuste.AcceptChanges();
            Ds_Imprimir_Ajuste.Tables.Add(Dt_Productos_Ajuste.Copy());
            Ds_Imprimir_Ajuste.Tables[1].TableName = "Dt_Productos_Ajuste";
            Ds_Imprimir_Ajuste.AcceptChanges();

            Ds_Ope_Alm_Autorizar_Ajuste_Inventario Obj_Ajuste_Inv = new Ds_Ope_Alm_Autorizar_Ajuste_Inventario();
            Generar_Reporte(Ds_Imprimir_Ajuste, Obj_Ajuste_Inv, "Rpt_Ope_Alm_Autorizar_Ajuste_Inventario.rpt", "Rpt_Ope_Alm_Autorizar_Ajuste_Inventario.pdf");
        }
    }



    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
    }
    #endregion

}
