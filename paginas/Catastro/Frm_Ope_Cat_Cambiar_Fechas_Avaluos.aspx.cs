using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Operacion_Cat_Avaluo_Rustico_Autorizacion_Valor.Negocio;
using Presidencia.Operacion_Cat_Asignacion_Cuentas.Negocio;
using Presidencia.Sessiones;

public partial class paginas_Catastro_Frm_Ope_Cat_Cambiar_Fechas_Avaluos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
       // Limpia_Mensaje_Error();
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
        Txt_Folio_Inicial.Enabled = !Enabled;
        Txt_Folio_Final.Enabled = !Enabled;
        Txt_Anio.Enabled = !Enabled;
        Txt_Fecha_Avaluo.Enabled = !Enabled;
        Twe_Fecha_Avaluo.Enabled = !Enabled;
        Btn_Fecha_Avaluo.Enabled = !Enabled;
        Txt_Folio_Inicial.Style["text-align"] = "Right";
        Txt_Folio_Final.Style["text-align"] = "Right";
        Txt_Anio.Style["text-align"] = "Right";
        Txt_Fecha_Avaluo.Style["text-align"] = "Center";
        
       
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
            Limpiar_Formulario();
           
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
          
            Configuracion_Formulario(true);
            
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
           
        }
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


        if (Btn_Modificar.AlternateText.Equals("Modificar"))
        {

            Configuracion_Formulario(false);
            Btn_Modificar.AlternateText = "Actualizar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
            Btn_Salir.AlternateText = "Cancelar";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
          
           
        }
        else if (Validar_Componentes())
        {
            Cls_Ope_Cat_Asignacion_Cuentas_Negocio Cuentas = new Cls_Ope_Cat_Asignacion_Cuentas_Negocio();
            Cuentas.P_Folio_Inicial = Txt_Folio_Inicial.Text;
            Cuentas.P_Folio_Final = Txt_Folio_Final.Text;
            Cuentas.P_Anio_Avaluo = Txt_Anio.Text;
            Cuentas.P_Fecha_Avaluo =Convert.ToDateTime(Txt_Fecha_Avaluo.Text);
            if (Cuentas.Modificar_Avaluos_Fecha())
            {
               

                Btn_Modificar.Visible = true;
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                Configuracion_Formulario(true);

                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Limpiar_Formulario();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Avalúo Rústico", "alert('Se modificaron correctamente los Avaluos.');", true);
            }
        }

       
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
    private void Limpiar_Formulario()
    {
        Txt_Folio_Inicial.Text = "";
        Txt_Folio_Final.Text = "";
        Txt_Anio.Text = "";
        Txt_Fecha_Avaluo.Text = "";
      
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Valida los datos ingresados
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
        Boolean Valido = true;
        String Msj_Error = "Error: ";
       
        if (Txt_Folio_Inicial.Text=="")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Favor de ingresar el Folio Inicial.";
            Valido = false;
        }

        if (Txt_Folio_Final.Text == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Favor de ingresar el Folio Final.";
            Valido = false;
        }
        if (Txt_Anio.Text == "")
        {
            Msj_Error += "<br/>";
            Msj_Error += "+ Favor de ingresar el Año.";
            Valido = false;
        }
        if (Txt_Folio_Final.Text != "" && Txt_Folio_Inicial.Text != "")
        {
            if (Convert.ToInt64(Txt_Folio_Inicial.Text) > Convert.ToInt64(Txt_Folio_Final.Text))
            {
                Msj_Error += "<br/>";
                Msj_Error += "+ Verifique los Folios *No puede ser Menor el Folio Final";
                Valido = false;
            }
        }
        if (!Valido)
        {
            Mostrar_Mensaje_Error(Msj_Error);
        }
        return Valido;
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mostrar_Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mostrar_Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Encabezado_Error.Text = "";
        Lbl_Encabezado_Error.Text = P_Mensaje + "</br>";
    }

    private void Limpia_Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Encabezado_Error.Text = "";
    }

}

