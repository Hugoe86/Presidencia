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
using Presidencia.Pass_Proveedor.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Ope_Cambiar_Pass_Prov : System.Web.UI.Page
{
    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            DataTable Dt_Proveedor_Session = (DataTable)Cls_Sessiones.Datos_Proveedor;            
            

            Btn_Cancelar.Visible = false;
            Btn_Buscar_Proveedor.Visible = false;
            Txt_Num_Padron.Text = Dt_Proveedor_Session.Rows[0][Cat_Com_Proveedores.Campo_Proveedor_ID].ToString().Trim(); 
            Txt_Num_Padron.Enabled = false;
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            if (Txt_Num_Padron.Text.Trim() != String.Empty)
            {
                //realizamos la consulta de los datos del proveedor
                Cls_Ope_Cambiar_Pass_Prov_Negocio Clase_Negocio = new Cls_Ope_Cambiar_Pass_Prov_Negocio();
                Clase_Negocio.P_Num_Proveedor = String.Format("{0:0000000000}", Convert.ToInt32(Txt_Num_Padron.Text));
                DataTable Dt_Proveedor = Clase_Negocio.Consultar_Proveedor();
                //Asignamos los valores del proveedor
                Txt_Razon_Social.Text = Dt_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Nombre].ToString().Trim();
                Txt_Nombre_Comercial.Text = Dt_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Compañia].ToString().Trim();
                //Guardamos en una variable de session el Password anterior y el id del proveedor
                Session["Proveedor_ID"] = Clase_Negocio.P_Num_Proveedor.Trim();
                Session["Password_Anterior"] = Dt_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Password].ToString().Trim();
            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Es necesario indicar el Numero de Padron de Proveedor";
            }
        }
    }
    #endregion

    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    #region Metodos


    public void Limpiar_Componentes()
    {
        Txt_Num_Padron.Text = "";
        Txt_Razon_Social.Text = "";
        Txt_Nombre_Comercial.Text = "";
        Txt_Password_Actual.Text = "";
        Txt_Password_Nuevo.Text = "";
        Txt_Confirmar_Password.Text = "";
        Session["Proveedor_ID"] = null;
        Session["Password_Anterior"] = null;
        }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Cajas
    ///DESCRIPCIÓN: Metodo que valida que las cajas tengan contenido. 
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 06/Octubre/2010 
    ///MODIFICO: Gustavo AC
    ///FECHA_MODIFICO: 24 Marzo 2012
    ///CAUSA_MODIFICACIÓN: No funciona correctamente la validacion, 
    ///*******************************************************************************
    public void Validar_Cajas()
    {

        if (Txt_Password_Actual.Text.Trim() == String.Empty)
        {
            Lbl_Mensaje_Error.Text += "La contraseña actual es obligatoria <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        if (Txt_Password_Nuevo.Text.Trim() == String.Empty)
        {
            Lbl_Mensaje_Error.Text += "La nueva contraseña es obligatoria<br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }


        if (Txt_Password_Actual.Text.Trim() != Session["Password_Anterior"].ToString().Trim())
        {
            Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "La contraseña actual no es correcta<br/>";
        }


        if (Txt_Password_Nuevo.Text.Trim() != Txt_Confirmar_Password.Text.Trim())
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "La nueva contraseña no coincide con la confirmación.<br/>";
        }


    }
   

   
    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos
    
    protected void Btn_Cancelar_Click(object sender, EventArgs e)
    {
        Limpiar_Componentes();
    }

    protected void Btn_Buscar_Proveedor_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Num_Padron.Text.Trim() != String.Empty)
        {
            //realizamos la consulta de los datos del proveedor
            Cls_Ope_Cambiar_Pass_Prov_Negocio Clase_Negocio = new Cls_Ope_Cambiar_Pass_Prov_Negocio();
            Clase_Negocio.P_Num_Proveedor = String.Format("{0:0000000000}", Convert.ToInt32(Txt_Num_Padron.Text));
            DataTable Dt_Proveedor = Clase_Negocio.Consultar_Proveedor();
            //Asignamos los valores del proveedor
            Txt_Razon_Social.Text = Dt_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Nombre].ToString().Trim();
            Txt_Nombre_Comercial.Text = Dt_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Compañia].ToString().Trim();
            //Guardamos en una variable de session el Password anterior y el id del proveedor
            Session["Proveedor_ID"] = Clase_Negocio.P_Num_Proveedor.Trim();
            Session["Password_Anterior"] = Dt_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Password].ToString().Trim();



        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario indicar el Numero de Padron de Proveedor";
        }
    }

    protected void Btn_Guardar_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Razon_Social.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario indicar el Proveedor a cambiar la contraseña";
        }
        else
        {

            Validar_Cajas();
            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                Cls_Ope_Cambiar_Pass_Prov_Negocio Clase_Negocio = new Cls_Ope_Cambiar_Pass_Prov_Negocio();
                Clase_Negocio.P_Num_Proveedor = Session["Proveedor_ID"].ToString().Trim();
                Clase_Negocio.P_Password_Nuevo = Txt_Password_Nuevo.Text.Trim();

                String Mensaje_error = Clase_Negocio.Modificar_Password_Proveedor();

                if (Mensaje_error.Trim() != String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Mensaje_error;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cambio Password Proveedor", "alert('Se cambio exitosamente el password');", true);
                    //Response.Redirect("../Paginas_Generales/Frm_Apl_Principal_Proveedores.aspx");
                    //Limpiar_Componentes();
                }

            }//fin del if
        }//fin del else
    }

    #endregion


    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("/Inicio");
    }
}
