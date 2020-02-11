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
using Presidencia.Mis_Datos_Proveedor.Negocio;
using Presidencia.Constantes;
using Presidencia.Registro_Peticion.Datos;
using Presidencia.Sessiones;


public partial class paginas_Compras_Frm_Cat_Com_Mis_Datos_Proveedor : System.Web.UI.Page
{

    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        //
        Limpiar_Componentes();
        //obtenemos Los datos del proveedor 
        Llenar_Datos_Proveedor();
        Llenar_Grid_Conceptos();

    }
    #endregion
    ///*******************************************************************************
    ///METODOS
    ///******************************************************************************

    #region Metodos

    public void Limpiar_Componentes()
    {
        Txt_Proveedor_ID.Text = "";
        Txt_Compania.Text = "";
        Txt_Nombre.Text = "";
        Txt_Contacto.Text = "";
        Txt_RFC.Text = "";
        Txt_Direccion.Text = "";
        Txt_Colonia.Text = "";
        Txt_Ciudad.Text = "";
        Txt_Estado.Text = "";
        Txt_CP.Text = "";
        Txt_Telefono_1.Text = "";
        Txt_Telefono_2.Text = "";
        Txt_Nextel.Text = "";
        Txt_Fax.Text = "";
        Txt_Correo_Electronico.Text = "";
        Txt_Dias_Credito.Text = "";
        Txt_Forma_Pago.Text = "";
        Txt_Comentarios.Text = "";
        Grid_Conceptos_Proveedor.DataSource = new DataTable();
        Grid_Conceptos_Proveedor.DataBind();
    }

    public void Llenar_Datos_Proveedor()
    {
        //Consultamos la variable de session de Proveedores 
        DataTable Dt_Proveedor_Session = (DataTable)Cls_Sessiones.Datos_Proveedor;

        Cls_Cat_Com_Mis_Datos_Proveedor_Negocio Datos_Negocio = new Cls_Cat_Com_Mis_Datos_Proveedor_Negocio();
        Datos_Negocio.P_Proveedor_ID = Dt_Proveedor_Session.Rows[0][Cat_Com_Proveedores.Campo_Proveedor_ID].ToString().Trim(); 
        //Consultamos lod datos del proveedor
        DataTable Dt_Datos_Proveedor = Datos_Negocio.Consultar_Datos_Proveedor();
        Txt_Proveedor_ID.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Proveedor_ID].ToString().Trim();
        Txt_Compania.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Compañia].ToString().Trim();
        Txt_Nombre.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Nombre].ToString().Trim();
        Txt_Contacto.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Contacto].ToString().Trim();
        Txt_RFC.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_RFC].ToString().Trim();
        Txt_Direccion.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Direccion].ToString().Trim();
        Txt_Colonia.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Colonia].ToString().Trim();
        Txt_Ciudad.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Ciudad].ToString().Trim();
        Txt_Estado.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Estado].ToString().Trim();
        Txt_CP.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_CP].ToString().Trim();
        Txt_Telefono_1.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Telefono_1].ToString().Trim();
        Txt_Telefono_2.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Telefono_2].ToString().Trim();
        Txt_Nextel.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Nextel].ToString().Trim();
        Txt_Fax.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Fax].ToString().Trim();
        Txt_Correo_Electronico.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Correo_Electronico].ToString().Trim();
        Txt_Dias_Credito.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Dias_Credito].ToString().Trim();
        Txt_Forma_Pago.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Forma_Pago].ToString().Trim();
        Txt_Comentarios.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Comentarios].ToString().Trim();
        Txt_Cuenta.Text = Dt_Datos_Proveedor.Rows[0]["Cuenta_Contable"].ToString().Trim();


    }

    public void Llenar_Grid_Conceptos()
    {
        Cls_Cat_Com_Mis_Datos_Proveedor_Negocio Datos_Negocio = new Cls_Cat_Com_Mis_Datos_Proveedor_Negocio();
        DataTable Dt_Proveedor_Session=(DataTable) Cls_Sessiones.Datos_Proveedor;
        Datos_Negocio.P_Proveedor_ID = Dt_Proveedor_Session.Rows[0]["Proveedor_ID"].ToString().Trim();
        DataTable Dt_Conceptos_Proveedor = Datos_Negocio.Consultar_Conceptos_Proveedor();
        if (Dt_Conceptos_Proveedor.Rows.Count > 0)
        {
            Grid_Conceptos_Proveedor.DataSource = Dt_Conceptos_Proveedor;
            Grid_Conceptos_Proveedor.DataBind();
        }


    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo
    ///DESCRIPCIÓN: Envia un correo a el encargado de Almacen para notificar que una solicitud ha sido creada 
    ///             o que hay alguna que no ha sido revisada.
    ///PROPIEDADES: 
    ///             1.  cabecera.       Número de Solicitud de la cual se quieren obtener sus detalles.
    ///             2.  no_apartado.    Número de Apartado de la cual se le quiere notificar al encargado
    ///                                 del almacen.
    ///             3.  solicito.       Nombre de quien hizo la solicitud del apartado.
    ///             4.  fecha.          Fecha del Proyecto del cual se hace la solicitud de Apartado.
    ///             5.  proyecto.       Proyecto del cual se hace la solicitud de apartado.
    ///             6.  descripcion.    Descripción del Proyecto del cual se hizó la solicitud de apartado.
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:    Francisco Antonio Gallardo Castañeda.
    ///FECHA_MODIFICO:  Junio 2010.
    ///CAUSA_MODIFICACIÓN:  Se adapto para que el funcionamiento del Catalogo de Solicitud de Apartado. 
    ///*******************************************************************************
    public void Enviar_Correo(String Email)
    {

        try
        {
            if (Email != "" && Email != null)
            {
                Cls_Mail mail = new Cls_Mail();

                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = Email;
                mail.P_Subject = "Solicitud del Proveedor " + Txt_Nombre.Text.Trim();
                mail.P_Texto = Txt_Solicitud.Text.Trim();
                mail.P_Adjunto = null;//Hacer_Pdf();
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
            throw new Exception(Ex.ToString());
        }
    }
    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grids

    #endregion 

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region

    protected void Btn_Enviar_Solicitud_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Solicitud.Text.Trim() != String.Empty)
        {
            try
            {
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Enviar_Correo("gustavo.angeles@contel.com.mx");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud", "alert('Se ha enviado un correo con su solicitud');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud", "alert('No se pudo enviar el correo con su solicitud');", true);
            }
        }//fin del if
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario ingresar la solicitud que se enviará";
        }
    }
    #endregion


    
}
