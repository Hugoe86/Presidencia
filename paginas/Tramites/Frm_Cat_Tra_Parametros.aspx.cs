using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Tramites_Parametros.Negocio;

public partial class paginas_Tramites_Frm_Cat_Tra_Parametros : System.Web.UI.Page
{
    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        Mostrar_Informacion("", false);

        if (!IsPostBack)
        {
            Habilitar_Controles("Inicial");
            Consultar_Parametros();
        }
    }
    #endregion Load

    #region Metodos

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Habilitar_Botones
    ///DESCRIPCIÓN: Establece las propiedades de los botones modificar y salir y del combo programa
    ///         dependiendo del contenido del parámetro recibido.
    ///PARÁMETROS:
    /// 		1. Estado: indica la operación que se pretende realizar y para la que se van a preparar los controles
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 02-Agosto-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Habilitar_Controles(String Estado)
    {
        switch (Estado)
        {
            //Estado Incicial de los controles
            case "Inicial":
                Btn_Modificar.Visible = true;
                Btn_Salir.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Txt_Correo_Cuerpo.Enabled = false;
                Txt_Correo_Despedida.Enabled = false;
                Txt_Correo_Encabezado.Enabled = false;
                Txt_Correo_Firma.Enabled = false;
                break;
            //Estado de Modificar
            case "Modificar":
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ToolTip = "Cancelar modificación";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Txt_Correo_Cuerpo.Enabled = true;
                Txt_Correo_Despedida.Enabled = true;
                Txt_Correo_Encabezado.Enabled = true;
                Txt_Correo_Firma.Enabled = true;
                break;
        }
    }
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Parametros
    ///DESCRIPCIÓN: Consulta los parámetros y si existen en el combo correspondiente, lo selecciona
    ///PARÁMETROS:
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 02-Agosto-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Consultar_Parametros()
    {
        var Obj_Parametros = new Cls_Cat_Tra_Parametros_Negocio();
        String Parametro = "";
        try
        {
            // consultar parámetros
            Obj_Parametros.Consultar_Parametros();

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Correo_Encabezado))
            {
                Parametro = Obj_Parametros.P_Correo_Encabezado;
                Txt_Correo_Encabezado.Text = Parametro;
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Correo_Cuerpo))
            {
                // si la dependencia existe en el combo, seleccionarlo
                Parametro = Obj_Parametros.P_Correo_Cuerpo;
                Txt_Correo_Cuerpo.Text = Parametro;
            }

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Correo_Despedida))
            {
                // si la dependencia existe en el combo, seleccionarlo
                Parametro = Obj_Parametros.P_Correo_Despedida;
                Txt_Correo_Despedida.Text = Parametro;
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Correo_Firma))
            {
                // si la dependencia existe en el combo, seleccionarlo
                Parametro = Obj_Parametros.P_Correo_Firma;
                Txt_Correo_Firma.Text = Parametro;
            }

           
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al cargar parámetros: " + Ex.Message, true);
        }
    }
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Informacion
    ///DESCRIPCIÓN: Muestra en la página el mensaje recibido como parámetro y establece la visibilidad 
    ///             de los controles  para mostrar mensajes con el segundo parámetro
    ///PARÁMETROS:
    /// 		1. Mensaje: Texto a mostrar en la página
    /// 		2. Mostrar: establece la visibilidad de los controles en los que se muestran los mensajes de la página
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 02-Agosto-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Mostrar_Informacion(String Mensaje, bool Mostrar)
    {
        Lbl_Informacion.Text = Mensaje;
        Lbl_Informacion.Visible = Mostrar;
        Img_Informacion.Visible = Mostrar;
    }
    
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Modificar_Parametros
    ///DESCRIPCIÓN: Consulta los parámetros y si exiten en el combo correspondiente, lo selecciona
    ///PARÁMETROS:
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 02-Agosto-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Modificar_Parametros()
    {
        var Obj_Parametros = new Cls_Cat_Tra_Parametros_Negocio();

        try
        {
            // agregar valores para actualizar
            Obj_Parametros.P_Correo_Encabezado = Txt_Correo_Encabezado.Text;
            Obj_Parametros.P_Correo_Cuerpo = Txt_Correo_Cuerpo.Text;
            Obj_Parametros.P_Correo_Despedida = Txt_Correo_Despedida.Text;
            Obj_Parametros.P_Correo_Firma = Txt_Correo_Firma.Text;
            Obj_Parametros.P_Usuario = Cls_Sessiones.Nombre_Empleado;

            if (Obj_Parametros.Actualizar_Parametros() > 0)
            {
                Habilitar_Controles("Inicial");
                Consultar_Parametros();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('Actualización exitosa.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('No fue posible realizar la actualización.');", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al actualizar parámetros: " + Ex, true);
        }
    }
    #endregion Metodos


    #region EVENTOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Dependiendo del estado del botón (tooltipo: Modificar o Actualizar)
    ///         Configurar controles o actualiza el parámetro
    ///PARÁMETROS: NO APLICA
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 02-Agosto-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        // limpiar mensajes de error
        Mostrar_Informacion("", false);

        try
        {
            // validar estado del botón
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                Habilitar_Controles("Modificar");
            }
            else
            {
                // validar que haya un programa seleccionado
                if (Txt_Correo_Encabezado.Text !=""
                    && Txt_Correo_Cuerpo.Text != ""
                    && Txt_Correo_Despedida.Text != ""
                    && Txt_Correo_Firma.Text != "")
                {
                    Modificar_Parametros();
                }
                else
                {
                    Mostrar_Informacion("Es necesario seleccionar la estructura del correo.", true);
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Manejo del evento click en el botón de salir: dependiendo del tooltip del botón, regresa a 
    ///         la página principal o reinicia los controles de la página a su estado Inicial
    ///PARÁMETROS:
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 02-Agosto-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Inicio")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Habilitar_Controles("Inicial");
            Consultar_Parametros();
        }
    }

    #endregion Eventos
}
