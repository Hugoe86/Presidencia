using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Caja_Cierre_Turno.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Predial_Frm_Ope_Pre_Cierre_Turno : System.Web.UI.Page
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
                Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocios = new Cls_Ope_Pre_Cierre_Turno_Negocio();
                Llenar_Grid_Cierre_Turno(Clase_Negocios);
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
                    Div_Grid_Cierre_Turno.Visible = true;
                    Div_Contenedor_Msj_Error.Visible = false;
                    Div_Cierre_Caja.Visible = false;
                    Btn_Salir.ToolTip = "Inicio";
                    
                    
                break;
            case "General":
                    Div_Grid_Cierre_Turno.Visible = false;
                    Div_Contenedor_Msj_Error.Visible = false;
                    Div_Cierre_Caja.Visible = true;
                    Btn_Salir.ToolTip = "Listado";

                break;

            case "Modificar":
                break;
        }
    }

    public void Limpiar_Formas()
    {
      
        Txt_Cajero.Text = "";
        Txt_No_Caja.Text = "";
        Txt_Clave_Caja.Text = "";
        Txt_Hora_Apertura.Text = "";
        Txt_Aplicacion_Pagos.Text = "";
        Txt_Recibo_Inicial.Text = "";
        Txt_Fondo_Inicial.Text = "";
        Txt_Hora_Cierre.Text = "";
        Txt_Fecha_Apertura.Text = "";
        Txt_Fecha_Cierre.Text = "";

        Txt_Total_Detalles.Text = "0.0";
        Txt_Total_Efectivo.Text = "0.0";
        Txt_Total_Bancos.Text = "0.0";
        Txt_Total_Cheques.Text = "0.0";
        Txt_Total_Transferencia.Text = "0.0";
        Txt_Monto_Total.Text = "0.0";
    
        //Limpiamos clases de sessiones
        Session["ESTATUS"] = null;
        Session["Total_Detalles"] = null;
        Session["No_Turno"] = null;
        Session["Caja_ID"] = null;
        Session["Dt_Caja_Turnos"] = null;
    }


    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    public void Llenar_Grid_Cierre_Turno(Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocios)
    {
        DataTable Dt_Caja_Turnos = Clase_Negocios.Consultar_Caj_Turno();

        if (Dt_Caja_Turnos.Rows.Count != 0)
        {
            Grid_Cierre_Turno.DataSource = Dt_Caja_Turnos;
            Grid_Cierre_Turno.DataBind();
            Session["Dt_Caja_Turnos"] = Dt_Caja_Turnos;

        }
        else
        {
            Grid_Cierre_Turno.DataSource = new DataTable();
            Grid_Cierre_Turno.DataBind();
        }
        


    }
    

    protected void Grid_Cierre_Turno_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Consultamos los detalles del turno
        Configurar_Formulario("General");
        Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocio = new Cls_Ope_Pre_Cierre_Turno_Negocio();
        GridViewRow selectedRow = Grid_Cierre_Turno.Rows[Grid_Cierre_Turno.SelectedIndex];
        int num_fila = Grid_Cierre_Turno.SelectedIndex;
        //indicamos la variable de session 
        DataTable Dt_Caja_Turnos = (DataTable)Session["Dt_Caja_Turnos"];
        Clase_Negocio.P_No_Turno = Grid_Cierre_Turno.SelectedDataKey["No_Turno"].ToString();
        Session["No_Turno"] = Clase_Negocio.P_No_Turno;
        Clase_Negocio.P_Caja_ID = Dt_Caja_Turnos.Rows[num_fila]["Caja_ID"].ToString().Trim();
        Session["Caja_ID"] = Clase_Negocio.P_Caja_ID;
        //Consultamos los detalles
        
        DataTable Dt_Dato_Caj_Turno = Clase_Negocio.Consultar_Caj_Turno();
        Txt_Cajero.Text = Dt_Caja_Turnos.Rows[num_fila]["CAJERO"].ToString().Trim();
        Txt_No_Caja.Text = Dt_Caja_Turnos.Rows[num_fila]["NUM_CAJA"].ToString().Trim();
        Txt_Clave_Caja.Text = Dt_Caja_Turnos.Rows[num_fila]["CLAVE_CAJA"].ToString().Trim();
        if (Dt_Caja_Turnos.Rows[num_fila]["Hora_Apertura"].ToString().Trim() != String.Empty)
        {
            Txt_Hora_Apertura.Text = String.Format("{0:HH:mm:ss}", Convert.ToDateTime(Dt_Caja_Turnos.Rows[num_fila]["Hora_Apertura"].ToString().Trim()));
        }
        Txt_Aplicacion_Pagos.Text = Dt_Dato_Caj_Turno.Rows[0][Ope_Caj_Turnos.Campo_Aplicacion_Pago].ToString().Trim();
        Txt_Recibo_Inicial.Text = Dt_Dato_Caj_Turno.Rows[0][Ope_Caj_Turnos.Campo_Recibo_Inicial].ToString().Trim();
        Txt_Fondo_Inicial.Text = Dt_Dato_Caj_Turno.Rows[0][Ope_Caj_Turnos.Campo_Fondo_Inicial].ToString().Trim();
        if (Dt_Dato_Caj_Turno.Rows[0][Ope_Caj_Turnos.Campo_Hora_Cierre].ToString().Trim() != String.Empty)
        {
            Txt_Hora_Cierre.Text = String.Format("{0:HH:mm:ss}", Convert.ToDateTime(Dt_Dato_Caj_Turno.Rows[0][Ope_Caj_Turnos.Campo_Hora_Cierre].ToString().Trim()));
        } 
        Session["ESTATUS"] = Dt_Dato_Caj_Turno.Rows[0]["ESTATUS"].ToString().Trim();
        Txt_Estatus.Text = Dt_Dato_Caj_Turno.Rows[0]["ESTATUS"].ToString().Trim();
        //if (Session["ESTATUS"].ToString().Trim() == "ABIERTO")
        //{
        //    Lbl_Fecha.Visible = true;
        //    Txt_Hora_Cierre.Visible = false;
        //}
        //else
        //{
        //    Lbl_Fecha.Visible = false;
        //    Txt_Hora_Cierre.Visible = true;
        //}
        //Consultamos los totales de acuerdo al estatus
        Clase_Negocio.P_Estatus = Dt_Dato_Caj_Turno.Rows[0]["ESTATUS"].ToString().Trim();
        DataTable Dt_Totales = Clase_Negocio.Consultar_Totales_Caj_Turno();
        if (Dt_Totales.Rows.Count != 0)
        {
            if (Dt_Dato_Caj_Turno.Rows[0]["ESTATUS"].ToString().Trim() == "ABIERTO")
            {

                Txt_Total_Bancos.Text = String.Format("{0:c}", Obtener_Total(Dt_Totales, "BANCO")).Replace("$","");
                Txt_Total_Cheques.Text = String.Format("{0:c}", Obtener_Total(Dt_Totales, "CHEQUE")).Replace("$", "");
                Txt_Total_Efectivo.Text = String.Format("{0:c}", Obtener_Total(Dt_Totales, "EFECTIVO")).Replace("$", "");
                Txt_Total_Transferencia.Text = String.Format("{0:c}", Obtener_Total(Dt_Totales, "TRANSFERENCIA")).Replace("$", "");

            }
            if (Dt_Dato_Caj_Turno.Rows[0]["ESTATUS"].ToString().Trim() == "CERRADO")
            {

                if (Dt_Totales.Rows[0][Ope_Caj_Turnos.Campo_Total_Bancos].ToString().Trim() != String.Empty)
                    Txt_Total_Bancos.Text = String.Format("{0:#,###,###,#0.00}", Convert.ToDouble((String.IsNullOrEmpty(Dt_Totales.Rows[0][Ope_Caj_Turnos.Campo_Total_Bancos].ToString())) ? "0" : Dt_Totales.Rows[0][Ope_Caj_Turnos.Campo_Total_Bancos].ToString().Trim()));
                if (Dt_Totales.Rows[0][Ope_Caj_Turnos.Campo_Total_Cheques].ToString().Trim() != String.Empty)
                    Txt_Total_Cheques.Text = String.Format("{0:#,###,###,#0.00}", Convert.ToDouble((String.IsNullOrEmpty(Dt_Totales.Rows[0][Ope_Caj_Turnos.Campo_Total_Cheques].ToString())) ? "0" : Dt_Totales.Rows[0][Ope_Caj_Turnos.Campo_Total_Cheques].ToString().Trim()));
                if (Dt_Totales.Rows[0][Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema].ToString().Trim() != String.Empty)
                    Txt_Total_Efectivo.Text = String.Format("{0:#,###,###,#0.00}", Convert.ToDouble((String.IsNullOrEmpty(Dt_Totales.Rows[0][Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema].ToString())) ? "0" : Dt_Totales.Rows[0][Ope_Caj_Turnos.Campo_Total_Efectivo_Sistema].ToString().Trim()));
                if (Dt_Totales.Rows[0][Ope_Caj_Turnos.Campo_Total_Transferencias].ToString().Trim() != String.Empty)
                    Txt_Total_Transferencia.Text = String.Format("{0:#,###,###,#0.00}", Convert.ToDouble((String.IsNullOrEmpty(Dt_Totales.Rows[0][Ope_Caj_Turnos.Campo_Total_Transferencias].ToString())) ? "0" : Dt_Totales.Rows[0][Ope_Caj_Turnos.Campo_Total_Transferencias].ToString().Trim()));
            }
            //reALIZAR LA SUMA DE LOS MONTOS
            double Total_Bancos = double.Parse(Txt_Total_Bancos.Text);
            double Total_Cheques = double.Parse(Txt_Total_Cheques.Text);
            double Total_Efectivo = double.Parse(Txt_Total_Efectivo.Text);
            double Total_Transferencia = double.Parse(Txt_Total_Transferencia.Text);
            double Monto_Total = Total_Bancos + Total_Cheques + Total_Efectivo + Total_Transferencia;
            Txt_Monto_Total.Text = String.Format("{0:#,###,###,#0.00}", Monto_Total);
            //Consultamos los detalles del pago
            Session["Total_Detalles"] = 0;
            Llenar_Grid_Detalles_Pagos(Clase_Negocio);

            if (Session["Total_Detalles"] != null)
                Txt_Total_Detalles.Text = String.Format("{0:#,###,###,#0.00}", Convert.ToDouble(Session["Total_Detalles"].ToString()));
        }

    }

    public void Llenar_Grid_Detalles_Pagos(Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocio)
    {
        DataTable Dt_Dependencias = Clase_Negocio.Consultar_Dependencias();
        Grid_Detalle_Pago.DataSource = Dt_Dependencias;
        Grid_Detalle_Pago.DataBind();
       // DataTable Dt_Pagos = Clase_Negocio.Consultar_Detalle_Pagos();
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Menus_RowDataBound
    ///DESCRIPCIÓN: 
    ///
    ///PARAMETROS:  
    ///CREO:Juan Alberto Hernández Negrete.
    ///FECHA_CREO: 27/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Detalle_Pago_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocio = new Cls_Ope_Pre_Cierre_Turno_Negocio();//Variable de conexión a la capa de negocios.
        DataTable Dt_Sub_Detalle_Pago = null;//Variable que almacena la lista de submenús que tiene el menú.
        double Suma_Montos = double.Parse(Session["Total_Detalles"].ToString().Trim());
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                //Obtenemos el grid que almacenara el grid que contiene los submenús del menú.
                GridView Grid_Sub_Detalle_Pagos = (GridView)e.Row.Cells[4].FindControl("Grid_Sub_Detalle_Pago");
                String Dependencia_ID = e.Row.Cells[1].Text.Trim();
                String No_Turno = Session["No_Turno"].ToString().Trim();
                String Caja_ID = Session["Caja_ID"].ToString().Trim();
                Clase_Negocio.P_No_Turno = No_Turno;
                Clase_Negocio.P_Caja_ID = Caja_ID;
                Clase_Negocio.P_Dependencia_ID = Dependencia_ID;
                Dt_Sub_Detalle_Pago = Clase_Negocio.Consultar_Detalle_Pagos();      //Consultamos los submenús que tiene el parent_id del menú. 

                Grid_Sub_Detalle_Pagos.DataSource = Dt_Sub_Detalle_Pago;
                Grid_Sub_Detalle_Pagos.DataBind();
                //Sumamos para obtener el monto total
                for (int i = 0; i < Dt_Sub_Detalle_Pago.Rows.Count; i++)
                {
                    Suma_Montos = Suma_Montos + double.Parse(Dt_Sub_Detalle_Pago.Rows[i]["Monto"].ToString().Trim());
                    Session["Total_Detalles"] = Suma_Montos;
                }



            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    public double Obtener_Total(DataTable Dt_Tabla_Montos, String Forma_Pago)
    {
        double Monto_Total= 0.0;

        for (int i = 0; i < Dt_Tabla_Montos.Rows.Count; i++)
        {
            if (Dt_Tabla_Montos.Rows[i]["FORMA_PAGO"].ToString().Trim() == Forma_Pago)
                Monto_Total = double.Parse(Dt_Tabla_Montos.Rows[i][0].ToString().Trim());
        }

            return Monto_Total;
    }


    /// *********************************************************************************************************************
    /// Nombre: Generar_Reporte
    /// 
    /// Descripción: Metodo que genera el reporte de cierre de caja.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 25/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *********************************************************************************************************************
    protected void Generar_Reporte()
    {
        Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Datos = new Cls_Ope_Pre_Cierre_Turno_Negocio();//Variable de conexión con la capa de negocios.
        DataSet Ds_Reporte = null;//Variable que almacena las tabla de ingresos.
        Clase_Datos.P_Caja_ID = Session["Caja_ID"].ToString();
        Clase_Datos.P_No_Turno = Session["No_Turno"].ToString();

        try
        {
            Ds_Reporte = Clase_Datos.Rpt_Caj_Ingresos();//Consulta de las tablas de ingresos.
            Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Ing_Corte_Caja.rpt", "Rpt_Ingresos_" + Session.SessionID + ".pdf");//Generamos el reporte de ingresos.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocio = new Cls_Ope_Pre_Cierre_Turno_Negocio();
        switch (Btn_Salir.ToolTip)
        {
            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");

                
                break;
            case "Cancelar":
                
                Limpiar_Formas();
                Configurar_Formulario("Inicio");
                Llenar_Grid_Cierre_Turno(Clase_Negocio);

                break;
            case "Listado":
                Limpiar_Formas();
                Configurar_Formulario("Inicio");
                Llenar_Grid_Cierre_Turno(Clase_Negocio);
                break;
        }


    }


    protected void Btn_Cerrar_Caja_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        if (Session["ESTATUS"].ToString().Trim() == "CERRADO")
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ No se puede cerrar esta caja ya que ya fue cerrada";

        }

        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            //cargamos todos los registros para actualizar 
            Cls_Ope_Pre_Cierre_Turno_Negocio Clase_Negocio = new Cls_Ope_Pre_Cierre_Turno_Negocio();
            Clase_Negocio.P_Caja_ID = Session["Caja_ID"].ToString().Trim();
            Clase_Negocio.P_No_Turno = Session["No_Turno"].ToString().Trim();
            Clase_Negocio.P_Total_Bancos = Txt_Total_Bancos.Text.Trim().Replace(",","");
            //Clase_Negocio.P_Total_Cheques = Txt_Total_Cheques.Text.Trim();
            Clase_Negocio.P_Total_Efectivo_Sistema = Txt_Total_Efectivo.Text.Trim().Replace(",", "");
            Clase_Negocio.P_Total_Transferencias = Txt_Total_Transferencia.Text.Trim().Replace(",", "");

            //SE ESTABLECEN LOS VALORES PARA DAR EL ALTA DE LAS DENONI
            Clase_Negocio.P_Cant_Diez_Cent = Txt_Denom_10_Cent.Text.Trim();
            Clase_Negocio.P_Cant_Veinte_Cent = Txt_Denom_20_Cent.Text.Trim();
            Clase_Negocio.P_Cant_Cinc_Cent = Txt_Denom_50_Cent.Text.Trim();
            Clase_Negocio.P_Cant_Un_P = Txt_Denom_1_Peso.Text.Trim();
            Clase_Negocio.P_Cant_Dos_P = Txt_Denom_2_Pesos.Text.Trim();
            Clase_Negocio.P_Cant_Cinco_P = Txt_Denom_5_Pesos.Text.Trim();
            Clase_Negocio.P_Cant_Diez_P = Txt_Denom_10_Pesos.Text.Trim();
            Clase_Negocio.P_Cant_Veinte_P = Txt_Denom_20_Pesos.Text.Trim();
            Clase_Negocio.P_Cant_Cincuenta_P = Txt_Denom_50_Pesos.Text.Trim();
            Clase_Negocio.P_Cant_Cien_P = Txt_Denom_100_Pesos.Text.Trim();
            Clase_Negocio.P_Cant_Doscientos_P = Txt_Denom_200_Pesos.Text.Trim();
            Clase_Negocio.P_Cant_Quinientos_P= Txt_Denom_500_Pesos.Text.Trim();
            Clase_Negocio.P_Cant_Mil_P = Txt_Denom_1000_Pesos.Text.Trim();
            Clase_Negocio.P_Cant_Monto_Total = Convert.ToDouble((String.IsNullOrEmpty(Txt_Total.Text.Trim())) ? "0" : Txt_Total.Text.Trim()).ToString();


            //Cerramos la caja
            try
            {
                bool Operacion_Realizada = Clase_Negocio.Cerrar_Caja();
                if (Operacion_Realizada== true)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Cierre de Caja", "alert('Se Cerro el turno');", true);
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Exito... Se cerro la caja </br>";
                        

                    //creamos la poliza
                    //Operacion_Realizada = Clase_Negocio.Generar_Poliza();
                    if (Operacion_Realizada == true)
                    {
                        //Div_Contenedor_Msj_Error.Visible = true;
                        //Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+Se creo la poliza </br>";
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Cierre de Caja", "alert('Se creo la poliza correspondiente al pago');", true);
                    }
                    //generamos el reporte 
                    Generar_Reporte();


                }
            }
            catch(Exception Ex)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se cerro la caja";

            }
            //Mandamos llamar el metodo de Actualizar
        }
        

    }

    #endregion

    #region (Reportes)
    /// *************************************************************************************
    /// NOMBRE: Generar_Reporte
    /// 
    /// DESCRIPCIÓN: Método que invoca la generación del reporte.
    ///              
    /// PARÁMETROS: Nombre_Plantilla_Reporte.- Nombre del archivo del Crystal Report.
    ///             Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:15 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Generar_Reporte(ref DataSet Ds_Datos, String Nombre_Plantilla_Reporte, String Nombre_Reporte_Generar)
    {
        ReportDocument Reporte = new ReportDocument();//Variable de tipo reporte.
        String Ruta = String.Empty;//Variable que almacenara la ruta del archivo del crystal report. 

        try
        {
            Ruta = @Server.MapPath("../Rpt/Ingresos/" + Nombre_Plantilla_Reporte);
            Reporte.Load(Ruta);

            if (Ds_Datos is DataSet)
            {
                if (Ds_Datos.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Datos);
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    Mostrar_Reporte(Nombre_Reporte_Generar);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Exportar_Reporte_PDF
    /// 
    /// DESCRIPCIÓN: Método que guarda el reporte generado en formato PDF en la ruta
    ///              especificada.
    ///              
    /// PARÁMETROS: Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///             Nombre_Reporte.- Nombre que se le dará al reporte.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = @Server.MapPath("../../Reporte/" + Nombre_Reporte);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Reporte
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en pantalla.
    ///              
    /// PARÁMETROS: Nombre_Reporte.- Nombre que tiene el reporte que se mostrara en pantalla.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Empleados",
                "window.open('" + Pagina + "', 'Busqueda_Empleados','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion



}//Fin del Class
