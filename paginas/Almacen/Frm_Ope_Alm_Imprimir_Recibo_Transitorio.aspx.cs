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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Collections.Generic;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Reportes;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Almacen_Impresion_Recibos.Negocio;
using Presidencia.Almacen_Elaborar_Recibo_Transitorio.Negocio;

public partial class paginas_Almacen_Frm_Ope_Alm_Imprimir_Recibo_Transitorio : System.Web.UI.Page
{

    #region Variables
    Cls_Ope_Com_Alm_Impresion_Recibos_Negocio Recibo = new Cls_Ope_Com_Alm_Impresion_Recibos_Negocio();
    #endregion

    # region  Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Cmb_Tipo.Items.Clear();
            Cmb_Tipo.Items.Add("RECIBO/RESGUARDO");
            Cmb_Tipo.Items.Add("EQUIPO");
            Cmb_Tipo.SelectedIndex = 0;
            Estatus_Inicial();
            //Llenar_Combo_Recibos();
        }
    }
    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN:          Evento utiliado para mostrar en pantalla el recibo transitorio en PDF
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           25/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "PDF";
        Llenar_Tablas_Recibos(Formato);
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Excel_Click
    ///DESCRIPCIÓN:          Evento utiliado para mostrar en pantalla el recibo transitorio en Excel
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           18/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "Excel";
        Llenar_Tablas_Recibos(Formato);
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tablas_Recibos 
    ///DESCRIPCIÓN:          Método utilizado para llenar el DataSet con las tablas
    ///                      Dt_Cabecera y Dt_Detalles, e instanciar el método Generar_Reporte
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           18/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Llenar_Tablas_Recibos( String Formato)
    {
        Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Recibo_Transitorio = new Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio();

        DataTable Dt_Datos_Generales = new DataTable();
        DataTable Dt_Productos = new DataTable();
        String Nombre_Reporte_Crystal = "";      // Se guardara el nombre del reporte

        Recibo_Transitorio.P_No_Orden_Compra = Session["No_Orden_Compra_I_RT"].ToString(); // Se asigna el No. de Orden de Compra
        Recibo_Transitorio.P_No_Contra_Recibo = Session["No_Contra_Recibo_I"].ToString(); // se asigna el No. Contra Recibo

        Dt_Productos = (DataTable)Session["Mostrar_Productos_RT"];  // Se asigna a la variable de session la tabla que contiene los productos del recibo transitorio


        if (Session["TIPO_RT"].ToString().Trim() == "UNIDAD")
        {
            Nombre_Reporte_Crystal = "Rpt_Ope_Com_Recibo_Transitorio_Por_Unidad.rpt";
            Dt_Datos_Generales = Recibo_Transitorio.Consulta_Datos_Generales_Recibo_Transitorio(); // Se consultan los datos generales del recibo transitorio
        }
        else if (Session["TIPO_RT"].ToString().Trim() == "TOTALIDAD")
        {
            for (int j = 0; j < Dt_Productos.Rows.Count; j++)
            {
                double Precio = 0;
                double Monto_Total = 0;
                Int32 Existencia = 0;

                if (Dt_Productos.Rows[j]["PRECIO"].ToString().Trim() != "")
                    Precio = Convert.ToDouble(Dt_Productos.Rows[j]["PRECIO"]);

                if (Dt_Productos.Rows[j]["EXISTENCIA"].ToString().Trim() != "")
                    Existencia = Convert.ToInt32(Dt_Productos.Rows[j]["EXISTENCIA"]);

                Monto_Total = Existencia * Precio;

                Dt_Productos.Rows[j].SetField("MONTO_TOTAL", Monto_Total);
            }

            Nombre_Reporte_Crystal = "Rpt_Ope_Com_Recibo_Transitorio_Por_Totalidad.rpt";
            Dt_Datos_Generales = Recibo_Transitorio.Consulta_Datos_Generales_Recibo_Transitorio_Totalidad(); // Se consultan los datos generales del recibo transitorio
        }

        Ds_Ope_Com_Recibos Ds_Recibos = new Ds_Ope_Com_Recibos();
        String Nombre_Reporte = "Recibo_Transitorio";
      
        Generar_Reporte(Dt_Datos_Generales, Dt_Productos, Ds_Recibos, Nombre_Reporte_Crystal, Nombre_Reporte, Formato);
    }


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
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        else
        {
            Estatus_Inicial();
            Configuracion_Botones(true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN:          Evento utilizado para realizar un abusqueda simple por numero de orden de compra
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           10/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {   
        Consultar_Recibos_Transitorios();
    }
      #region Grid
       
        protected void Grid_Recibos_Transitorios_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow SelectedRow = Grid_Recibos_Transitorios.Rows[Grid_Recibos_Transitorios.SelectedIndex];//GridViewRow representa una fila individual de un control GridView

            String No_Recibo_T = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[1].Text.Trim()));    // Se obtiene el No. Recibo Transitorio
            String Fecha_Elaboro = Convert.ToString(SelectedRow.Cells[2].Text); // Se obtiene la fecha de elaboración
            String Folio_Orden_Compra = Convert.ToString(SelectedRow.Cells[3].Text.Trim()); // Se obtiene el  Folio de la Orden de Compra
            String Requisicion = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[4].Text.Trim())); // Se obtiene la Fecha de Construcción
            String Proveedor = HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[5].Text.Trim()));    // Se obtiene el nombre del proveedor
            String No_Orden_Compra = Convert.ToString(SelectedRow.Cells[6].Text.Trim()); // Se obtiene el No_Orden_Compra
            Session["No_Orden_Compra_I_RT"] = No_Orden_Compra;
            String No_Contra_Recibo = Convert.ToString(SelectedRow.Cells[7].Text.Trim()); // Se obtiene el numero de recibo
            Session["No_Contra_Recibo_I"] = No_Contra_Recibo;
            String Usuario_Creo= HttpUtility.HtmlDecode(Convert.ToString(SelectedRow.Cells[8].Text.Trim())); // Se obtiene la Fecha de Construcción
            String Total = Convert.ToString(SelectedRow.Cells[9].Text.Trim()); // Se obtiene la Fecha de Construcción
            String Tipo = Convert.ToString(SelectedRow.Cells[10].Text.Trim()); // Se obtiene el Tipo de Recibo Transitorio, si se elaboro por "UNIDAD" O "TOTALIDAD"
            Session["TIPO_RT"] = Tipo;
            Mostrar_Productos_Recibo_Transitorio(No_Recibo_T, Fecha_Elaboro, Folio_Orden_Compra, Requisicion, Proveedor, No_Orden_Compra, No_Contra_Recibo, Usuario_Creo, Total);
        }
        
    # endregion

    #endregion

    #region Metodos



        private void Llenar_Combo_Recibos()
        {
            DataTable Dt_Recibos_Transitorios = new DataTable();
            DataTable DT_Orden_Compra = new DataTable();

            Recibo.P_Tipo_Tabla = "RECIBOS_TRANSITORIOS";
            Dt_Recibos_Transitorios = Recibo.Consulta_Tablas();

            if (Dt_Recibos_Transitorios.Rows.Count > 0)
            {
                // Se agrega la fila a la tabla
                //DataRow Fila = Dt_Recibos_Transitorios.NewRow();
                //Fila["NO_CONTRA_RECIBO"] = "SELECCIONE";
                //Fila["NO_RECIBO"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                //Dt_Recibos_Transitorios.Rows.InsertAt(Fila, 0);
                //Cmb_Recibos_Transitorios.DataSource = Dt_Recibos_Transitorios;
                //Cmb_Recibos_Transitorios.DataValueField = "NO_CONTRA_RECIBO";
                //Cmb_Recibos_Transitorios.DataTextField = "NO_RECIBO";
                //Cmb_Recibos_Transitorios.DataBind();
                //Cmb_Recibos_Transitorios.SelectedIndex = 0; // Se selecciona el indice 0
                
                //Cmb_Responsable.DataValueField = "EMPLEADO_ID";
                //Cmb_Responsable.DataTextField = "EMPLEADO";
            }
        }

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

        Consultar_Recibos_Transitorios();
        Div_Detalles_Recibo_Transitorio.Visible = false;
        Estatus_Busqueda_Abanzada();
        Llenar_Combo_Proveedores();

    }




    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Busqueda_Abanzada
    ///DESCRIPCIÓN:          Método utilizado para configurar el estatus inicial de la busqueda abanzada
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Estatus_Busqueda_Abanzada()
    {
        if (Cmb_Proveedores.Items.Count != 0)
            Cmb_Proveedores.SelectedIndex = 0;

        Chk_Proveedor.Checked = false;
        Cmb_Proveedores.Enabled = false;

        Chk_Fecha_B.Checked = false;
        Img_Btn_Fecha_Inicio.Enabled = false;
        Img_Btn_Fecha_Fin.Enabled = false;
        Txt_Fecha_Fin.Text = "";
        Txt_Fecha_Inicio.Text = "";
        Txt_OrdenC_Buscar.Text = "";
        Txt_Req_Buscar.Text = "";
        Txt_ReciboT_Buscar.Text = "";
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Proveedores
    ///DESCRIPCIÓN:          Método utilizado para llenar el combo con los proveedores 
    ///                      que se encuentren en la Base de Datos
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    public void Llenar_Combo_Proveedores()
    {
        try
        {
            Recibo.P_Tipo_Tabla = "PROVEEDORES_RECIBO_TRANSITORIO";
            Cmb_Proveedores.DataSource = Recibo.Consulta_Tablas();
            Cmb_Proveedores.DataTextField = Cat_Com_Proveedores.Campo_Nombre;
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
    public void Consultar_Recibos_Transitorios()
    {
        DataTable Dt_Recibos_Transitorios_Unidad = new DataTable();
        DataTable Dt_Recibos_Transitorios_Totalidad = new DataTable();
        DataTable Dt_Totalidad_Recibos = new DataTable();

        if (Txt_OrdenC_Buscar.Text.Trim() != "")
            Recibo.P_No_Orden_Compra = Txt_OrdenC_Buscar.Text.Trim();
        else
            Recibo.P_No_Orden_Compra = null;

        if (Txt_Req_Buscar.Text.Trim() != "")
            Recibo.P_No_Requisicion = Txt_Req_Buscar.Text.Trim();
        else
            Recibo.P_No_Requisicion = null;

        if (Txt_ReciboT_Buscar.Text.Trim() != "")
            Recibo.P_No_Recibo = Txt_ReciboT_Buscar.Text.Trim();
        else
            Recibo.P_No_Recibo = null;

        if (Chk_Proveedor.Checked == true)
        {
            if (Cmb_Proveedores.SelectedIndex != 0)
                Recibo.P_Proveedor = Cmb_Proveedores.SelectedValue.Trim();
            else
                Recibo.P_Proveedor = null;
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
                            Recibo.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim());
                            Recibo.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Fin.Text.Trim());
                            Div_Contenedor_Msj_Error.Visible = false;
                        }
                        else
                        {
                            String Fecha = Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()); //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                            Recibo.P_Fecha_Inicial = Fecha;
                            Recibo.P_Fecha_Final = Fecha;
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




        Dt_Recibos_Transitorios_Unidad = Recibo.Consulta_Recibos_Transitorios(); // Se consultan los registros por unidad
        Dt_Recibos_Transitorios_Totalidad = Recibo.Consulta_Recibos_Transitorios_Totalidad(); // Se consultan los registros por totalidad

        Dt_Totalidad_Recibos = Dt_Recibos_Transitorios_Totalidad.Clone(); // So clona la tabla, ya que esta contendra la información de las 2 tablas de recibos

        if (Cmb_Tipo.SelectedValue.Trim() == "RECIBO/RESGUARDO")
        {
            if (Dt_Recibos_Transitorios_Unidad.Rows.Count > 0)
            {
                for (int i = 0; i < Dt_Recibos_Transitorios_Unidad.Rows.Count; i++)
                { // Se llena la primera tabla con los registros

                    DataRow Dr_Recibo = Dt_Totalidad_Recibos.NewRow();

                    if (Dt_Recibos_Transitorios_Unidad.Rows[i]["NO_RECIBO"].ToString().Trim() != "")
                        Dr_Recibo["NO_RECIBO"] = Dt_Recibos_Transitorios_Unidad.Rows[i]["NO_RECIBO"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Unidad.Rows[i]["TIPO"].ToString().Trim() != "")
                        Dr_Recibo["TIPO"] = Dt_Recibos_Transitorios_Unidad.Rows[i]["TIPO"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Unidad.Rows[i]["FECHA_CREO"].ToString().Trim() != "")
                        Dr_Recibo["FECHA_CREO"] = Dt_Recibos_Transitorios_Unidad.Rows[i]["FECHA_CREO"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Unidad.Rows[i]["USUARIO_CREO"].ToString().Trim() != "")
                        Dr_Recibo["USUARIO_CREO"] = Dt_Recibos_Transitorios_Unidad.Rows[i]["USUARIO_CREO"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Unidad.Rows[i]["NO_ORDEN_COMPRA"].ToString().Trim() != "")
                        Dr_Recibo["NO_ORDEN_COMPRA"] = Dt_Recibos_Transitorios_Unidad.Rows[i]["NO_ORDEN_COMPRA"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Unidad.Rows[i]["FOLIO"].ToString().Trim() != "")
                        Dr_Recibo["FOLIO"] = Dt_Recibos_Transitorios_Unidad.Rows[i]["FOLIO"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Unidad.Rows[i]["TOTAL"].ToString().Trim() != "")
                        Dr_Recibo["TOTAL"] = Dt_Recibos_Transitorios_Unidad.Rows[i]["TOTAL"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Unidad.Rows[i]["NO_CONTRA_RECIBO"].ToString().Trim() != "")
                        Dr_Recibo["NO_CONTRA_RECIBO"] = Dt_Recibos_Transitorios_Unidad.Rows[i]["NO_CONTRA_RECIBO"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Unidad.Rows[i]["NO_REQUISICION"].ToString().Trim() != "")
                        Dr_Recibo["NO_REQUISICION"] = Dt_Recibos_Transitorios_Unidad.Rows[i]["NO_REQUISICION"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Unidad.Rows[i]["PROVEEDOR"].ToString().Trim() != "")
                        Dr_Recibo["PROVEEDOR"] = Dt_Recibos_Transitorios_Unidad.Rows[i]["PROVEEDOR"].ToString().Trim();

                    Int16 Longitud = Convert.ToInt16(Dt_Totalidad_Recibos.Rows.Count);
                    if (Longitud == 0)
                        Dt_Totalidad_Recibos.Rows.InsertAt(Dr_Recibo, Longitud);
                    else
                        Dt_Totalidad_Recibos.Rows.InsertAt(Dr_Recibo, (Longitud + 1));
                }
            }
        }

        if (Cmb_Tipo.SelectedValue == "EQUIPO")
        {
            if (Dt_Recibos_Transitorios_Totalidad.Rows.Count > 0) // Se agregan a la tabla los recibos transitorios por totalidad
            {
                for (int j = 0; j < Dt_Recibos_Transitorios_Totalidad.Rows.Count; j++)
                { // Se llena la primera tabla con los registros

                    DataRow Dr_Recibo = Dt_Totalidad_Recibos.NewRow();

                    if (Dt_Recibos_Transitorios_Totalidad.Rows[j]["NO_RECIBO"].ToString().Trim() != "")
                        Dr_Recibo["NO_RECIBO"] = Dt_Recibos_Transitorios_Totalidad.Rows[j]["NO_RECIBO"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Totalidad.Rows[j]["TIPO"].ToString().Trim() != "")
                        Dr_Recibo["TIPO"] = Dt_Recibos_Transitorios_Totalidad.Rows[j]["TIPO"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Totalidad.Rows[j]["FECHA_CREO"].ToString().Trim() != "")
                        Dr_Recibo["FECHA_CREO"] = Dt_Recibos_Transitorios_Totalidad.Rows[j]["FECHA_CREO"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Totalidad.Rows[j]["USUARIO_CREO"].ToString().Trim() != "")
                        Dr_Recibo["USUARIO_CREO"] = Dt_Recibos_Transitorios_Totalidad.Rows[j]["USUARIO_CREO"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Totalidad.Rows[j]["NO_ORDEN_COMPRA"].ToString().Trim() != "")
                        Dr_Recibo["NO_ORDEN_COMPRA"] = Dt_Recibos_Transitorios_Totalidad.Rows[j]["NO_ORDEN_COMPRA"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Totalidad.Rows[j]["FOLIO"].ToString().Trim() != "")
                        Dr_Recibo["FOLIO"] = Dt_Recibos_Transitorios_Totalidad.Rows[j]["FOLIO"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Totalidad.Rows[j]["TOTAL"].ToString().Trim() != "")
                        Dr_Recibo["TOTAL"] = Dt_Recibos_Transitorios_Totalidad.Rows[j]["TOTAL"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Totalidad.Rows[j]["NO_CONTRA_RECIBO"].ToString().Trim() != "")
                        Dr_Recibo["NO_CONTRA_RECIBO"] = Dt_Recibos_Transitorios_Totalidad.Rows[j]["NO_CONTRA_RECIBO"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Totalidad.Rows[j]["NO_REQUISICION"].ToString().Trim() != "")
                        Dr_Recibo["NO_REQUISICION"] = Dt_Recibos_Transitorios_Totalidad.Rows[j]["NO_REQUISICION"].ToString().Trim();

                    if (Dt_Recibos_Transitorios_Totalidad.Rows[j]["PROVEEDOR"].ToString().Trim() != "")
                        Dr_Recibo["PROVEEDOR"] = Dt_Recibos_Transitorios_Totalidad.Rows[j]["PROVEEDOR"].ToString().Trim();

                    Int16 Longitud = Convert.ToInt16(Dt_Totalidad_Recibos.Rows.Count);
                    if (Longitud == 0)
                        Dt_Totalidad_Recibos.Rows.InsertAt(Dr_Recibo, Longitud);
                    else
                        Dt_Totalidad_Recibos.Rows.InsertAt(Dr_Recibo, (Longitud + 1));
                }
            }
        }


        if (Dt_Totalidad_Recibos.Rows.Count > 0) // si la tabla contiene registros
        {
            Grid_Recibos_Transitorios.DataSource = Dt_Totalidad_Recibos;
            Session["Dt_Recibos_Transitorios"] = Dt_Totalidad_Recibos;
           
            Grid_Recibos_Transitorios.Columns[6].Visible = true;
            Grid_Recibos_Transitorios.Columns[7].Visible = true;
            Grid_Recibos_Transitorios.Columns[8].Visible = true;
            Grid_Recibos_Transitorios.Columns[9].Visible = true;
            Grid_Recibos_Transitorios.DataBind();
            Grid_Recibos_Transitorios.Columns[6].Visible = false;
            Grid_Recibos_Transitorios.Columns[7].Visible = false;
            Grid_Recibos_Transitorios.Columns[8].Visible = false;
            Grid_Recibos_Transitorios.Columns[9].Visible = false;

            Div_Contenedor_Msj_Error.Visible = false;
            Div_Recibos_Transitorios.Visible = true;
        }
        else
        {
            Lbl_Informacion.Text = "No se encontraron recibos transitorios";
            Div_Contenedor_Msj_Error.Visible = true;
            Div_Recibos_Transitorios.Visible = false;
        }
        Div_Detalles_Recibo_Transitorio.Visible = false;
        Btn_Imprimir.Visible = false;
        Btn_Imprimir_Excel.Visible = false;
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
    public void Mostrar_Productos_Recibo_Transitorio(String No_Recibo_T, String Fecha_Elaboro, String Folio_Orden_Compra, String Requisicion, String Proveedor, String No_Orden_Compra, String No_Contra_Recibo, String Usuario_Creo, String Total)
    {
        DataTable Dt_Detalles_ReciboT= new DataTable();
        DataTable Dt_Cabecera_ReciboT = new DataTable();
        Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Recibo_Transitorio = new Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio();

            Txt_No_Recibo.Text = No_Recibo_T;
            Txt_Usuario_Elaboro.Text = Usuario_Creo;
            Txt_Fecha_Elaboro.Text = Fecha_Elaboro;
            Txt_Folio_OrdenC.Text = Folio_Orden_Compra;

            if (Requisicion.Trim() != "")
            Txt_No_Requisicion.Text = "RQ-" + Requisicion ;

            Txt_Proveedor.Text = Proveedor;

            Recibo_Transitorio.P_No_Orden_Compra = No_Orden_Compra;
            Session["No_Orden_Compra_I_RT"] = No_Orden_Compra;
            Recibo_Transitorio.P_No_Contra_Recibo = No_Contra_Recibo;
            Session["No_Contra_Recibo_I"] = No_Contra_Recibo;

            Grid_Productos_RT_Unidad.Visible = false;
            Grid_Productos_RT_Totalidad.Visible = false;
        

            if (Session["TIPO_RT"].ToString() == "UNIDAD") // Si el Recibo Transitorio es por unidad
            {
                Dt_Detalles_ReciboT = Recibo_Transitorio.Consulta_Productos_Recibo_Transitorio();// Se consultan los productos de la tabla PROD_CONTRARECIBO

                if (Dt_Detalles_ReciboT.Rows.Count > 0)
                {
                    Session["Mostrar_Productos_RT"] = Dt_Detalles_ReciboT; // Se guarda en la variable de session la tabla
                    Grid_Productos_RT_Unidad.DataSource = Dt_Detalles_ReciboT;
                    Grid_Productos_RT_Unidad.Columns[0].Visible = true; // Se muestra el No_Inventario
                    Grid_Productos_RT_Unidad.Columns[8].Visible = true;
                    Grid_Productos_RT_Unidad.DataBind();
                    Grid_Productos_RT_Unidad.Columns[0].Visible = false;
                    Grid_Productos_RT_Unidad.Columns[8].Visible = false; // Se oculta el No_Inventario
                    Grid_Productos_RT_Unidad.Visible = true; // Se pone visible el Grid

                    Btn_Imprimir.Visible = true;
                    Btn_Imprimir_Excel.Visible = true;
                    Div_Recibos_Transitorios.Visible = false;
                    Div_Detalles_Recibo_Transitorio.Visible = true;

                    Configuracion_Botones(false);
                    Mostrar_Busqueda(false);
                }
            }
            else if (Session["TIPO_RT"].ToString() == "TOTALIDAD")
            {
                Dt_Detalles_ReciboT = Recibo_Transitorio.Consulta_Productos_Requision(); // Se consultan los productos de la requisición

                if (Dt_Detalles_ReciboT.Rows.Count > 0)
                {
                    // Se realiza la obtencion de los datos para determianr si existencias tiene 0, entonces se agregan
                    for (int i = 0; i < Dt_Detalles_ReciboT.Rows.Count; i++)
                    {
                        Int64 Cantidad_Entregada = 0;
                        Int64 Cantidad = 0;

                        if (Dt_Detalles_ReciboT.Rows[i]["EXISTENCIA"].ToString().Trim() != "")
                         Cantidad_Entregada = Convert.ToInt64(Dt_Detalles_ReciboT.Rows[i]["EXISTENCIA"].ToString().Trim()); // Cantidad Entregada
                        else
                            Cantidad_Entregada = 0;

                        if(Dt_Detalles_ReciboT.Rows[i]["CANTIDAD"].ToString().Trim() !="")
                             Cantidad = Convert.ToInt64(Dt_Detalles_ReciboT.Rows[i]["CANTIDAD"].ToString().Trim());// Cantidad Solicitada

                        String Marca = Dt_Detalles_ReciboT.Rows[i]["MARCA"].ToString().Trim();
                        String Modelo = Dt_Detalles_ReciboT.Rows[i]["MODELO"].ToString().Trim();

                        Int64 Existencia = (Cantidad - Cantidad_Entregada);    // Se optiene la existencia
                            Dt_Detalles_ReciboT.Rows[i].SetField("EXISTENCIA", Existencia); // Se agregan las existencias

                        if (Modelo.Trim() == "")
                            Dt_Detalles_ReciboT.Rows[i].SetField("MODELO", "INDISTINTO");
                        if (Marca.Trim() == "")
                            Dt_Detalles_ReciboT.Rows[i].SetField("MARCA", "INDISTINTA");
                    }

                    Session["Mostrar_Productos_RT"] = Dt_Detalles_ReciboT; // Se guarda en la variable de session la tabla
                    
                    Grid_Productos_RT_Totalidad.DataSource = Dt_Detalles_ReciboT;
                    Grid_Productos_RT_Totalidad.Columns[0].Visible = true;
                    Grid_Productos_RT_Totalidad.DataBind();
                    Grid_Productos_RT_Totalidad.Columns[0].Visible = false;
                    Grid_Productos_RT_Totalidad.Visible = true; // Se pone visible el Grid

                    Btn_Imprimir.Visible = true;
                    Btn_Imprimir_Excel.Visible = true;
                    Div_Recibos_Transitorios.Visible = false;
                    Div_Detalles_Recibo_Transitorio.Visible = true;

                    Configuracion_Botones(false);
                    Mostrar_Busqueda(false);
                    Div_Contenedor_Msj_Error.Visible = false;
                }
            }
            else
            {
                Configuracion_Botones(true);
                Lbl_Informacion.Text = " No se encontraron productos del recibo transitorio";
                Div_Contenedor_Msj_Error.Visible = true;
            }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Botones
    ///DESCRIPCIÓN:          Método utilizado para configuran los botones "Btn_Salir y Btn_Imprimir"
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           23/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Configuracion_Botones(bool Estatus)
    {
        if (Estatus == false)
        {
            Btn_Salir.AlternateText = "Atras";
            Btn_Salir.ToolTip = "Atras";

            Mostrar_Busqueda(false);
            Btn_Imprimir.Visible = true;
            Btn_Imprimir_Excel.Visible = true;
        }
        else if (Estatus == true)
        {
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ToolTip = "Salir";

            Mostrar_Busqueda(true);
            Btn_Imprimir.Visible = false;
            Btn_Imprimir_Excel.Visible = false;
        }

        if (Btn_Imprimir.Visible)
        {
            Configuracion_Acceso("Frm_Ope_Alm_Imprimir_Recibo_Transitorio.aspx");
            Configurar_Boton_Imprimir();
        }
    }



    public void Configurar_Boton_Imprimir()
    {

        if (Btn_Imprimir.Visible)
            Btn_Imprimir_Excel.Visible = true;
        else
            Btn_Imprimir_Excel.Visible = false;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Busqueda
    ///DESCRIPCIÓN:          Método utilizado para mostrar y ocultar los controles
    ///                      utilizados para realizar la búsqueda simble y abanzada
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           12/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Mostrar_Busqueda(Boolean Estatus)
    {
        Div_Busqueda_Av.Visible = Estatus;  
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.- Dt_Cabecera.- Contiene la informacion general de la orden de compra
    ///             2.- Dt_Detalles.- Contiene la información 
    ///             2.- Ds_Recibo.- Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.- Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///             4.- Nombre_Archivo, Es el nombre del documento que se va a generar en PDF
    ///CREO:        Salvador Hernández Ramírez
    ///FECHA_CREO:  09/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataTable Dt_Cabecera, DataTable Dt_Detalles, DataSet Ds_Recibo, String Nombre_Reporte_Crystal, String Nombre_Reporte, String Formato)
    {
        DataRow Renglon;
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";

        // Llenar la tabla "Cabecera" del DataSet
        Renglon = Dt_Cabecera.Rows[0];
        Ds_Recibo.Tables[0].ImportRow(Renglon);

        // Llenar la tabla "Detalles" del DataSet
        for (int Cont_Elementos = 0; Cont_Elementos < Dt_Detalles.Rows.Count; Cont_Elementos++)
        {
            Renglon = Dt_Detalles.Rows[Cont_Elementos]; //Instanciar renglon e importarlo
            Ds_Recibo.Tables[1].ImportRow(Renglon);
        }

        // Ruta donde se encuentra el reporte Crystal
        Ruta_Reporte_Crystal = "../Rpt/Almacen/" + Nombre_Reporte_Crystal;

        // Se crea el nombre del reporte
        String Nombre_Rep = Nombre_Reporte + "_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

        // Se da el nombre del reporte que se va generar
        if (Formato == "PDF")
            Nombre_Reporte_Generar = Nombre_Rep + ".pdf";  // Es el nombre del reporte PDF que se va a generar
        else if (Formato == "Excel")
            Nombre_Reporte_Generar = Nombre_Rep + ".xls";  // Es el nombre del repote en Excel que se va a generar

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
    ///NOMBRE DE LA FUNCIÓN:    Formato_Fecha
    ///DESCRIPCIÓN:             Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:              1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                         en caso de que cumpla la condicion del if
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              10/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {
        String Fecha_Valida = Fecha;
        String[] aux = Fecha.Split('/'); //Se le aplica un split a la fecha 
        switch (aux[1]) //Se modifica a mayusculas para que oracle acepte el formato.
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        // Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
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
            Botones.Add(Btn_Imprimir);
            //Botones.Add(Btn_Buscar);

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

    protected void Chk_Proveedor_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Proveedor.Checked)
        {
            Cmb_Proveedores.Enabled = true;
            Cmb_Proveedores.SelectedIndex = 0;
        }
        else
        {
            Cmb_Proveedores.Enabled = false;
            Cmb_Proveedores.SelectedIndex = 0;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:    Chk_Fecha_B_CheckedChanged
    ///DESCRIPCIÓN:             Maneja el evento click del CheckList "Chk_Fecha_B"
    ///PROPIEDADES:     
    ///CREO:                    Salvador Hernández Ramírez
    ///FECHA_CREO:              04/Abril/2011 
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
    protected void Grid_Productos_RT_Totalidad_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Estatus_Busqueda_Abanzada();
    }
    protected void Cmb_Tipo_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}