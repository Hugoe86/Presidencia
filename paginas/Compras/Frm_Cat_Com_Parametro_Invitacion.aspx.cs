using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Parametro_Invitacion.Negocio;
using System.Data;
using Presidencia.Constantes;

public partial class paginas_Compras_Frm_Cat_Com_Parametro_Invitacion : System.Web.UI.Page
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
           Txt_Texto_Invitacion.Enabled = false;
           //Consultamos el texto de la invitacion
           Consultar_Parametros();
           
        }
    }
    #endregion
    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    #region Metodos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Configurar_Formulario
    ///DESCRIPCIÓN: Metodo que ayuda a configuarar el formulario 
    ///PARAMETROS:Estatus, Puede Ser Inicio, Nuevo, Modificar 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 1/Noviembre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Consultar_Parametros()
    {
        Cls_Cat_Com_Parametro_Invitacion_Negocio clase_negocio = new Cls_Cat_Com_Parametro_Invitacion_Negocio();
        DataTable Dt_Invitacion = clase_negocio.Consular_Paremetros();
        if (Dt_Invitacion.Rows[0][Cat_Com_Parametros.Campo_Invitacion_Proveedores].ToString().Trim() != String.Empty)
        {
            Txt_Texto_Invitacion.Text = Dt_Invitacion.Rows[0][Cat_Com_Parametros.Campo_Invitacion_Proveedores].ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Configurar_Formulario
    ///DESCRIPCIÓN: Metodo que ayuda a configuarar el formulario 
    ///PARAMETROS:Estatus, Puede Ser Inicio, Nuevo, Modificar 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 1/Noviembre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Configurar_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicio":
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //Habilitamos la caja de texto
                Txt_Texto_Invitacion.Enabled = false;
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
                //Habilitamos la caja de texto
                Txt_Texto_Invitacion.Enabled = true;
                break;
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: 
    ///PARAMETROS: Evento del boton Modificar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 2/Noviembre/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        switch (Btn_Modificar.ToolTip.Trim())
        {
            case "Modificar":
                Configurar_Formulario("Modificar");
                break;
            case "Actualizar":

                if (Txt_Texto_Invitacion.Text.Trim() != String.Empty)
                {
                    Cls_Cat_Com_Parametro_Invitacion_Negocio Clase_Negocio = new Cls_Cat_Com_Parametro_Invitacion_Negocio();
                    Clase_Negocio.P_Invitacion_Proveedores = Txt_Texto_Invitacion.Text;
                    int Registros_Afectados = Clase_Negocio.Actualizar_Parametros();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Proveedores", "alert('Se Actualizaron los datos');", true);
                    Configurar_Formulario("Inicio");
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "Es necesario indicar el mensaje";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                break;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: 
    ///PARAMETROS: Evento del boton Salir
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 2/Noviembre/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        switch (Btn_Salir.ToolTip.Trim())
        {
            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                break;
            case "Cancelar":
                Configurar_Formulario("Inicio");
                Consultar_Parametros();
                break;
        }
    }
    #endregion




    
}
