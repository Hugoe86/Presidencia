using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Generacion_Polizas.Negocio;
using Presidencia.Orden_Compra.Negocio;

public partial class paginas_Paginas_Generales_Frm_Ope_SAP_Generacion_Polizas : System.Web.UI.Page
{

///*********************************************************************************************************
///                                                   METODOS
///*********************************************************************************************************
#region METODOS

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Inicializa_Controles
    /// 	DESCRIPCIÓN: Prepara los controles en la forma para que el usuario pueda realizar 
    /// 	            diferentes operaciones
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            //Limpiar_Controles(); //Limpia los controles de la forma
            //Cargar_Combo_Capitulos();
            //Cargar_Combo_Giros();
            Consulta_Ordenes_Compra(); //Consultar las ordenes de compra con estatus SURTIDA y llenar el grid
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Habilitar_Controles
    /// 	DESCRIPCIÓN: Habilita o Deshabilita los controles de la forma para según se requiera para la 
    /// 	             siguiente operación
    /// 	PARÁMETROS:
    /// 	            1. Operacion: Indica si se preparan los controles para un alta, una modificación o
    /// 	                    se limpian como estado inicial
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        try
        {
            switch (Operacion)
            {
                case "Inicial":
                    Txt_Fecha_Inicio.Text ="01/" + DateTime.Today.ToString("MMM/yyyy");
                    Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                    break;
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Consulta_Ordenes_Compra
    /// 	DESCRIPCIÓN: COnsulta
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Consulta_Ordenes_Compra()
    {
        Cls_Ope_Com_Orden_Compra_Negocio RS_Consulta_Ordenes_Compras = new Cls_Ope_Com_Orden_Compra_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Ordenes_Compra; //Variable que obtendrá los datos de la consulta 

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            if (Txt_Busqueda.Text.Length > 0)
            {
                RS_Consulta_Ordenes_Compras.P_Folio = Txt_Busqueda.Text.Trim();
            }
            else
            {
                RS_Consulta_Ordenes_Compras.P_Fecha_Inicial = Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim()).ToString("dd/MM/yyyy");
                RS_Consulta_Ordenes_Compras.P_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text.Trim()).ToString("dd/MM/yyyy");
            }
            RS_Consulta_Ordenes_Compras.P_Estatus = "SURTIDA";
            Dt_Ordenes_Compra = RS_Consulta_Ordenes_Compras.Consultar_Ordenes_Compra(); //Consulta las Ordenes de compra
            Session["Consulta_Ordenes_Compra"] = Dt_Ordenes_Compra;
            Llena_Grid_Ordenes_Compra();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Ordenes_Compra " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Llena_Grid_Ordenes_Compra
    /// 	DESCRIPCIÓN: Llena el grid con las Ordenes de compra de la base de datos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Llena_Grid_Ordenes_Compra()
    {
        DataTable Dt_Ordenes_Compra; //Variable que obtendrá los datos de la consulta 
        try
        {
            Grid_Ordenes_Compra.Columns[1].Visible = true;
            Dt_Ordenes_Compra = (DataTable)Session["Consulta_Ordenes_Compra"];
            Grid_Ordenes_Compra.DataSource = Dt_Ordenes_Compra;
            Grid_Ordenes_Compra.DataBind();
            Grid_Ordenes_Compra.Columns[1].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Ordenes_Compra " + ex.Message.ToString(), ex);
        }
    }

#endregion METODOS

///*********************************************************************************************************
///                                                   EVENTOS
///*********************************************************************************************************
#region EVENTOS


    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.GetCurrent(Page).RegisterPostBackControl(Btn_Generar_Archivo_Polizas);
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "F_Inicial", @"function F_Inicial(e){__doPostBack('Txt_Fecha_Inicio','') ;}", true);
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "F_Final", @"function F_Final(e){__doPostBack('Txt_Fecha_Final','') ;}", true);
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Generar_Archivo_Polizas_Click
    /// 	DESCRIPCIÓN: Manejar evento click sobre boton generar Archivo Polizas. Generar archivo de 
    /// 	            Polizas y ofrecer para descarga
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 14-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Generar_Archivo_Polizas_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_SAP_Generacion_Polizas_Negocio Generador_Polizas = new Cls_Ope_SAP_Generacion_Polizas_Negocio();
        StringWriter Texto_Poliza = new StringWriter();
        DataTable Dt_Poliza;
        int Dias_Credito = 0;
        bool Bnd_Seleccionados = false;

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        if (Grid_Ordenes_Compra.Rows.Count <= 0)    // si no hay datos en el grid, mostrar mensaje y abandonar generacion de poliza
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Debe seleccionar por lo menos una orden de compra para generar la póliza.";
            return;
        }

        foreach (GridViewRow Fila in Grid_Ordenes_Compra.Rows)
        {
            CheckBox Chk_Orden = (CheckBox)Fila.FindControl("Chk_Orden_Compra_Seleccionada");
            if (Chk_Orden != null && Chk_Orden.Checked) //Si por hay por lo menos un checkbox seleccionado salir
            {
                Bnd_Seleccionados = true;
                break;
            }
        }
        if (Bnd_Seleccionados == false)         // si ningun checkbox esta seleccionado, mostrar mensaje y salir
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Debe seleccionar las ordenes de compra de las que desea generar póliza.";
            return;
        }

        try
        {
            foreach (GridViewRow Fila in Grid_Ordenes_Compra.Rows)
            {

                CheckBox Chk_Orden = (CheckBox)Fila.FindControl("Chk_Orden_Compra_Seleccionada");
                if (Chk_Orden != null && Chk_Orden.Checked)
                {
                    if (Grid_Ordenes_Compra.Rows[Fila.RowIndex].Cells[8].Text != "SURTIDA")
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "La orden de compra " + Grid_Ordenes_Compra.Rows[Fila.RowIndex].Cells[2].Text + " aún no ha sido surtida.";
                        return;
                    }
                    Dt_Poliza = Generador_Polizas.Consultar_Datos_Poliza(Grid_Ordenes_Compra.Rows[Fila.RowIndex].Cells[1].Text.Trim());//Consultar datos de la orden de compra seleccionada
                    if (Dt_Poliza.Rows.Count > 0)   //Si se recibieron registros de la consulta
                    {
                        foreach (DataRow Poliza in Dt_Poliza.Rows)
                        {
                            Generador_Polizas.P_Fecha = Convert.ToDateTime(Poliza[Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura]).ToString("ddMMyyyy");
                            Generador_Polizas.P_Periodo = Convert.ToDateTime(Poliza[Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura]).ToString("MM");
                            Generador_Polizas.P_Clave = Poliza["CLAVE_OPERACION_SAP"].ToString().Trim();
                            Generador_Polizas.P_Cuenta = Poliza["CUENTA_SAP"].ToString().Trim();
                            Generador_Polizas.P_Unidad_Responsable = Poliza["CLAVE_DEPENDENCIA"].ToString().Trim();
                            if (Poliza[Cat_Com_Partidas.Campo_Afecta_Area_Funcional].ToString() == "SI")
                                Generador_Polizas.P_Area_Funcional = Poliza["CLAVE_AREA_FUNCIONAL"].ToString().Trim().Replace(".","");
                            if (Poliza[Cat_Com_Partidas.Campo_Afecta_Elemento_PEP].ToString() == "SI")
                                Generador_Polizas.P_Elemento_PEP = Poliza[Cat_Com_Proyectos_Programas.Campo_Elemento_PEP].ToString().Trim();
                            Int32.TryParse(Poliza[Cat_Com_Proveedores.Campo_Dias_Credito].ToString(), out Dias_Credito);
                            Generador_Polizas.P_No_De_Reserva = Generador_Polizas.P_Fecha = Poliza[Ope_Com_Ordenes_Compra.Campo_No_Reserva].ToString(); ;
                            Generador_Polizas.P_Fecha_Base = Generador_Polizas.P_Fecha = Convert.ToDateTime(Poliza[Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura]).AddDays(Dias_Credito).ToString("ddMMyyyy");
                            Generador_Polizas.P_Via_Pago = Poliza[Cat_Com_Proveedores.Campo_Forma_Pago].ToString().Substring(0, 1).ToUpper();
                            Generador_Polizas.P_Asignacion = Poliza[Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor].ToString().Trim();
                            Generador_Polizas.P_Texto_Posicion = Poliza[Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor].ToString().Trim() + Poliza["FOLIO_ORDEN_COMPRA"].ToString().Trim();
                            Generador_Polizas.P_Sociedad = Poliza[Ope_SAP_Parametros.Campo_Sociedad].ToString();
                            if (Poliza[Cat_Com_Partidas.Campo_Afecta_Fondo].ToString() == "SI")
                                Generador_Polizas.P_Fondo = Poliza[Ope_SAP_Parametros.Campo_Fondo].ToString();
                            Generador_Polizas.P_Division = Poliza[Ope_SAP_Parametros.Campo_Division].ToString();
                            Generador_Polizas.P_Importe = Poliza[Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString();
                            
                            //Escribir en el StringWriter el resultado de la llamada al metodo Generar_Texto_Poliza de la capa de negocio del catalogo
                            Texto_Poliza.WriteLine(Generador_Polizas.Generar_Texto_Poliza());
                        }

                    }
                }
            }
            Response.ContentType = "Text/plain";
            Response.AddHeader("content-disposition", "attachment;filename=polizas_" + String.Format("{0:ddMMyyyy}", DateTime.Now) + ".txt");
            Response.Clear();
            using (StreamWriter writer = new StreamWriter(Response.OutputStream, Encoding.Unicode))
            {   
                writer.Write(Texto_Poliza.ToString());
            }
        }   
        catch (Exception ex)
        {
            throw new Exception("Generar_Archivo_Polizas " + ex.Message.ToString(), ex);
        }
        Response.End();
        Context.Response.Flush();
        Context.ApplicationInstance.CompleteRequest();
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Salir_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click para el control Btn_Salir. Ir a la página principal o 
    /// 	        inicializar controles 
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                //Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Txt_Fecha_TextChanged
    /// 	DESCRIPCIÓN: Manejo del evento Cambio en el texto de fecha final e inicial, volver a llenar el 
    /// 	            grid con la nueva fecha, revisando que el formato de las fechas es correcto
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Fecha_TextChanged(object sender, EventArgs e)
    {
        Regex Rge_Fecha = new Regex(@"[0-9]{2}\/[a-zA-Z]{3,}\/[0-9]{4}");
        string[] Partes_Fecha_Inicial = Txt_Fecha_Inicio.Text.Split('/');
        string[] Partes_Fecha_Final = Txt_Fecha_Final.Text.Split('/');

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        
        if (Rge_Fecha.IsMatch(Txt_Fecha_Inicio.Text) && Rge_Fecha.IsMatch(Txt_Fecha_Final.Text)) //Revisar formato de fecha
        {
            if (Partes_Fecha_Final.Length == 3 && Partes_Fecha_Inicial.Length == 3) //que la fecha se compone de 3 partes separadas por diagonal
            {
                Txt_Busqueda.Text = ""; //Borrar campo busqueda para evitar que Consulta ordenes de compra, busque por folio
                Consulta_Ordenes_Compra();
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Revise que las fechas inicial y final tengan el formato correcto(" + DateTime.Now.ToString("dd/MMM/yyyy") + ").";
            }
        }
        else
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Revise que las fechas inicial y final tengan el formato correcto(" + DateTime.Now.ToString("dd/MMM/yyyy") + ").";
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Chk_Seleccionar_Todas_Ordenes_CheckedChanged
    /// 	DESCRIPCIÓN: Manejo del evento cambio de seleccion en el combo encabezado del grid ordenes de 
    /// 	            compra. Seleccionar o desseleccionar todos los combos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Chk_Seleccionar_Todas_Ordenes_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox Cb_Selecciona_Todos = (CheckBox)Grid_Ordenes_Compra.HeaderRow.FindControl("Chk_Seleccionar_Todas_Ordenes");
        foreach (GridViewRow Fila in Grid_Ordenes_Compra.Rows)
        {
            CheckBox cb = (CheckBox)Fila.FindControl("Chk_Orden_Compra_Seleccionada");
            cb.Checked = Cb_Selecciona_Todos.Checked;
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Buscar_Click
    /// 	DESCRIPCIÓN: Manejo del evento clic en el boton de busqueda. Buscar una orden de compra por folio.
    /// 	            Llamando al metodo Consulta_Ordenes_Compra
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 18-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        if (Txt_Busqueda.Text.Length > 0) //revisar que se escribio un texto a buscar
        {
            Consulta_Ordenes_Compra();
        }
        else
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Debe proporcionar el folio a buscar";
        }
    }

#endregion EVENTOS

}
