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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Operacion_Cierre_Definitivo_Dia.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Cierre_Definitivo_Dia : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                //Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Llenar_Grid_Cierres_Dia();
                Llenar_Combo_Modulos();
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    protected void Cmb_Caja_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio Cajero = new Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio();
        DataTable Dt_Cajero = new DataTable();
        try
        {
            Cajero.P_Caja_ID = Cmb_Caja.SelectedValue;
            Dt_Cajero = Cajero.Consultar_Cajero();
            if (Dt_Cajero.Rows[0]["Usuario_Creo"].ToString().Trim() != "")
            {
                Txt_Cajero.Text = Dt_Cajero.Rows[0]["Usuario_Creo"].ToString().Trim();
            }
        }
        catch (Exception)
        {
        }
    }

    private void Llenar_Combo_Caja()
    {
        Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio Combo_Caja = new Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio();
        DataTable Dt_Caja = new DataTable();
        try
        {
            Combo_Caja.P_Modulo_ID = Cmb_Modulo.SelectedValue;
            Dt_Caja = Combo_Caja.Llenar_Combo_Caja();
            DataRow fila = Dt_Caja.NewRow();
            //fila[Cat_Pre_Cajas.Campo_Caja_ID] = "SELECCIONE";
            //fila[Cat_Pre_Cajas.Campo_Numero_De_Caja] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");

            Dt_Caja.Rows.InsertAt(fila, 0);
            Cmb_Caja.DataTextField = Cat_Pre_Cajas.Campo_Numero_De_Caja;
            Cmb_Caja.DataValueField = Cat_Pre_Cajas.Campo_Caja_ID;
            Cmb_Caja.DataSource = Dt_Caja;
            Cmb_Caja.DataBind();
        }
        catch (Exception)
        {
        }
    }

    private void Llenar_Combo_Modulos()
    {
        Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio Combo_Modulo = new Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio();
        DataTable Dt_Modulos = new DataTable();
        try
        {
            Dt_Modulos = Combo_Modulo.Llenar_Combo_Modulos();
            DataRow fila = Dt_Modulos.NewRow();
            fila[Cat_Pre_Modulos.Campo_Descripcion] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Modulos.Campo_Modulo_Id] = "SELECCIONE";
            Dt_Modulos.Rows.InsertAt(fila, 0);
            Cmb_Modulo.DataTextField = Cat_Pre_Modulos.Campo_Descripcion;
            Cmb_Modulo.DataValueField = Cat_Pre_Modulos.Campo_Modulo_Id;
            Cmb_Modulo.DataSource = Dt_Modulos;
            Cmb_Modulo.DataBind();
        }
        catch (Exception)
        {
        }
    }
    // llenar grid con columnas en blanco
    private void Llenar_Grid_Cierres_Dia()
    {
        Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio Cierre_Dia = new Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio();
        DataTable Dt_Cierre_Dia = new DataTable();
        try
        {
            Dt_Cierre_Dia = Cierre_Dia.Consultar_Cierres_Dia();
            Grid_Cierres_Dia.DataSource = Dt_Cierre_Dia;
            Grid_Cierres_Dia.DataBind();
            Session["Dt_Cierre_Dia"] = Dt_Cierre_Dia;
            //cOMPROBAR QUE SE HAYAN  CERRADO TODOS LOS MOVIMIENTOS PARA PODER REALIZAR LA IMPRESION
            for (int x = 0; x < Dt_Cierre_Dia.Rows.Count; x++)
            {
                if (Dt_Cierre_Dia.Rows[x]["Estatus"].ToString().Trim() == "ABIERTO")
                {
                    Btn_Imprimir.Enabled = false;
                }
            }

        }
        catch (Exception)
        {
        }
    }
    protected void Cmb_Modulo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Combo_Caja();
    }
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio Ope_Cierre_Definitivo = new Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio();
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                //Configuracion_Formulario(false);
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Txt_Fecha_Cierre.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                //Txt_Usuario.Text = Cls_Sessiones.Nombre_Empleado;
                //Cmb_Beneficio.Enabled = true;
                //Txt_Justificacion.Enabled = true;
                //Grid_Quitar_Beneficios.Enabled = true;
                //Btn_Nuevo.AlternateText = "Guardar";
                //Btn_Nuevo.Attributes.Clear();
            }
            else if (Btn_Nuevo.AlternateText.Equals("Dar de Alta"))
            {
                //DataTable Dt_Cierre_Definitivo = (DataTable)Session["Dt_Cierre_Dia"];
                //Ope_Cierre_Definitivo.P_Dt_Cierre = Dt_Cierre_Definitivo;
                ////Validar_Beneficio();
                //Quitar_Cuota.P_Dt_Quitar_Beneficio = Dt_Quitar_Beneficio;
                //Quitar_Cuota.P_Beneficio = Cmb_Beneficio.SelectedValue;
                //Alta_Orden_Beneficios();
                ////ScriptManager.RegisterStartupScript(this, this.GetType(), "Quitar Beneficio", "alert('El Beneficio fue quitado Exitosamente');", true);
                //Grid_Quitar_Beneficios.DataSource = null;
                //Grid_Quitar_Beneficios.DataBind();
                //Configuracion_Formulario(true);
                ////Btn_Nuevo.Attributes.Add("OnClick" = 'return confirm('¿Está seguro que desea Quitar los Beneficios?');'");
                ////Btn_Busqueda_Cuota_Minima.Attributes.Add("onclick", Ventana_Cuotas);
                //Btn_Nuevo.AlternateText = "Nuevo";
                //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                //Btn_Salir.AlternateText = "Salir";
                //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //Btn_Nuevo.AlternateText = "Nuevo";
                //Configuracion_Formulario(true);
            }
        }

        catch (Exception Ex)
        {
            //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            //Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio Imprimir_Cierre = new Cls_Ope_Pre_Cierre_Definitivo_Dia_Negocio();
        DataTable Dt_Imprimir_Cierre = (DataTable)Session["Dt_Cierre_Dia"];
        DataTable Dt_Imprimir_Detalles = new DataTable();
        try
        {
            Imprimir_Cierre.P_Dt_Cierre = Dt_Imprimir_Cierre;
            Dt_Imprimir_Detalles = Imprimir_Cierre.Consultar_Impresion();
        }
        catch (Exception Ex)
        {
        }
    }
}
