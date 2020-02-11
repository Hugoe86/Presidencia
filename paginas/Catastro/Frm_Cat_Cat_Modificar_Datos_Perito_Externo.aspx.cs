using System;
using System.Collections.Generic;
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
using Presidencia.Operacion_Cat_Recepcion_Documentos_Perito_Externo.Negocio;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;
using Presidencia.Constantes;

public partial class paginas_Catastro_Frm_Cat_Cat_Modificar_Datos_Perito_Externo : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN:
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);
                             
                Hdf_Mod_Perito_Externo.Value = Cls_Sessiones.Empleado_ID;
                Cargar_Datos_Perito_Externo();
                Txt_Informacion_Adicional.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Informacion_Adicional.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Txt_Informacion_Adicional.Attributes.Add("OnKeyPress", " ValidarCaracteres(this,250)");
                Txt_Informacion_Adicional.Attributes.Add("OnKeyUp", " ValidarCaracteres(this,250)");
                Cmb_Estatus.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Img_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Enabled)
    {
        Txt_Nombre.Enabled = !Enabled;
        Txt_Apellido_Materno.Enabled = !Enabled;
        Txt_Apellido_Paterno.Enabled = !Enabled;
        Txt_Calle.Enabled = !Enabled;
        Txt_Celular.Enabled = !Enabled;
        Txt_Ciudad.Enabled = !Enabled;
        Txt_Colonia.Enabled = !Enabled;
        Txt_Estado.Enabled = !Enabled;
        Txt_Informacion_Adicional.Enabled = !Enabled;
        Txt_Password.Enabled = !Enabled;
        Txt_Password_Confirma.Enabled = !Enabled;
        Txt_Telefono.Enabled = !Enabled;
        Txt_Usuario.Enabled = !Enabled;       
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Perito_Externo
    ///DESCRIPCIÓN: Selecciona un registro de la tabla y los asigna
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Datos_Perito_Externo()
    {
        if (Hdf_Mod_Perito_Externo.Value.Trim() != "")
        {
            Hdf_Mod_Perito_Externo.Value = Hdf_Mod_Perito_Externo.Value;
            Cls_Cat_Cat_Peritos_Externos_Negocio Perito_Externo = new Cls_Cat_Cat_Peritos_Externos_Negocio();
            DataTable Dt_Perito_Externo;
            Perito_Externo.P_Perito_Externo_Id = Hdf_Mod_Perito_Externo.Value;
            Dt_Perito_Externo = Perito_Externo.Consultar_Peritos_Externos();
            Txt_Usuario.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Usuario].ToString();
            Txt_Password.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Password].ToString();
            Txt_Password.Attributes.Add("value", Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Password].ToString());           
            Txt_Password_Confirma.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Password].ToString();
            Txt_Password_Confirma.Attributes.Add("value", Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Password].ToString());              
            Txt_Apellido_Materno.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Materno].ToString();
            Txt_Apellido_Paterno.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno].ToString();
            Txt_Calle.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Calle].ToString();
            Txt_Celular.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Celular].ToString();
            Txt_Ciudad.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Ciudad].ToString();
            Txt_Colonia.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Colonia].ToString();
            Txt_Estado.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Estado].ToString();            
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Estatus].ToString()));
            if (Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Fecha].ToString().Trim() != "")
            {
                Txt_Fecha.Text = Convert.ToDateTime(Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Fecha].ToString()).ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha.Text = "";
            }
            Txt_Nombre.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Nombre].ToString();
            Txt_Informacion_Adicional.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Observaciones].ToString();
            Txt_Telefono.Text = Dt_Perito_Externo.Rows[0][Cat_Cat_Peritos_Externos.Campo_Telefono].ToString();           
        }
    }

   

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Campos
    ///DESCRIPCIÓN: Limpia todos los campos del formulario
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Campos()
    {
        Txt_Usuario.Text = "";
        Txt_Password.Text = "";
        Txt_Password.Attributes.Add("value", "");
        Txt_Password_Confirma.Text = "";
        Txt_Password_Confirma.Attributes.Add("value", "");
        Txt_Nombre.Text = "";
        Txt_Apellido_Materno.Text = "";
        Txt_Apellido_Paterno.Text = "";
        Txt_Calle.Text = "";
        Txt_Celular.Text = "";
        Txt_Ciudad.Text = "";
        Txt_Colonia.Text = "";
        Txt_Estado.Text = "";
        Txt_Nombre.Text = "";
        Txt_Telefono.Text = "";
        Txt_Informacion_Adicional.Text = "";
        Cmb_Estatus.SelectedValue = "SELECCIONE";
        Hdf_Mod_Perito_Externo.Value = "";
    }   

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del botón modificar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {                
                Configuracion_Formulario(false);
                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Div_Grid_Datos_Peritos.Visible = true;               
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Cat_Peritos_Externos_Negocio Perito = new Cls_Cat_Cat_Peritos_Externos_Negocio();
                    Perito.P_Apellido_Materno = Txt_Apellido_Materno.Text.ToUpper();
                    Perito.P_Apellido_Paterno = Txt_Apellido_Paterno.Text.ToUpper();
                    Perito.P_Calle = Txt_Calle.Text.ToUpper();
                    Perito.P_Celular = Txt_Celular.Text;
                    Perito.P_Ciudad = Txt_Ciudad.Text.ToUpper();
                    Perito.P_Colonia = Txt_Colonia.Text.ToUpper();
                    Perito.P_Estado = Txt_Estado.Text.ToUpper();
                    Perito.P_Estatus = Cmb_Estatus.SelectedValue;
                    Perito.P_Fecha = Txt_Fecha.Text;
                    Perito.P_Nombre = Txt_Nombre.Text.ToUpper();
                    Perito.P_Observaciones = Txt_Informacion_Adicional.Text.ToUpper();
                    Perito.P_Password = Txt_Password.Text;
                    Perito.P_Telefono = Txt_Telefono.Text;
                    Perito.P_Usuario = Txt_Usuario.Text;
                    Perito.P_Perito_Externo_Id = Hdf_Mod_Perito_Externo.Value;
                    if ((Perito.Modificar_Perito_Externo()))
                    {
                        Configuracion_Formulario(true);                     
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";                        
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Configuracion_Formulario(true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Peritos Externos", "alert('Actualización de Perito Externo Exitosa.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Peritos Externos", "alert('Error al Actualizar el Perito Externo.');", true);
                    }
                }             
             }
        }
        catch (Exception ex)
        {
           Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Img_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del botón salir
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {            
            Configuracion_Formulario(true);            
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";            
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";            
            Div_Grid_Datos_Peritos.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: verifica todos los campos del formulario para que sean correctos
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes()
    {
        String Mensaje_Error = "Error: ";
        Boolean valido = true;

        if (Txt_Nombre.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Nombre del Perito.";
            valido = false;
        }
        if (Txt_Apellido_Paterno.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Apellido Paterno del Perito.";
            valido = false;
        }
        if (Txt_Apellido_Materno.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Apellido Materno del perito.";
            valido = false;
        }
        if (Txt_Usuario.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Usuario.";
            valido = false;
        }
        if (Txt_Password.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la Contraseña.";
            valido = false;
        }
        if (Txt_Password_Confirma.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la Contraseña nuevamente para confirmar.";
            valido = false;
        }
        if (Txt_Password_Confirma.Text.Trim() != "" && Txt_Password.Text.Trim() != "" && Txt_Password.Text != Txt_Password_Confirma.Text)
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Las contraseñas no coinciden, favor de verificarlas.";
            Txt_Password.Attributes.Add("value","");
            Txt_Password_Confirma.Attributes.Add("value", "");
            valido = false;
        }
        if (Txt_Calle.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la Calle.";
            valido = false;
        }
        if (Txt_Colonia.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la Colonia.";
            valido = false;
        }
        if (Txt_Estado.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Estado.";
            valido = false;
        }
        if (Txt_Ciudad.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese la Ciudad.";
            valido = false;
        }
        if (Txt_Telefono.Text.Trim() == "")
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Ingrese el Teléfono.";
            valido = false;
        }
        else if (Txt_Telefono.Text.Trim().Length != 10)
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ El Teléfono es incorrecto, se requieren 10 dígitos.";
            valido = false;
        }
        else if (Txt_Celular.Text.Trim() != "" && Txt_Celular.Text.Trim().Length != 10)
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ El Celular es incorrecto, se requieren 10 dígitos.";
            valido = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Seleccione un estatus.";
            valido = false;
        }
        if (!valido)
        {
            Lbl_Ecabezado_Mensaje.Text = Mensaje_Error;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;

        }
        else { 
            Div_Contenedor_Msj_Error.Visible = false;}       
        return valido;
    }
}