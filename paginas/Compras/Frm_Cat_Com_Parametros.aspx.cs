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
using Presidencia.Constantes;
using Presidencia.Catalogo_Compras_Parametros.Negocio;
using Presidencia.Sessiones;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Cat_Com_Parametros : System.Web.UI.Page
{
    #region (Variables)
    private Cls_Cat_Com_Parametros_Negocio Parametros_Negocio;
    #endregion

    #region (Page Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Estado_Inicial();
                Llenar_Combo_Generico();
                //Cmb_Partida_Especifica_Listados.Enabled = false;
                //Cmb_Partida_Generica_Listados.Enabled = true;
                //Llenar_Combo_Especificas();
            }
            else
            {
                Lbl_Informacion.Visible = false;
                Img_Warning.Visible = false;
                Lbl_Informacion.Text = "";
            }
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Text = "Error: (Page_Load)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }
    #endregion

    #region (Metodos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Mostrar_Informacion
    ///DESCRIPCION:             Habilita o deshabilita la muestra en pantalle del mensaje 
    ///                         de Mostrar_Informacion para el usuario
    ///PARAMETROS:              1.- Condicion, entero para saber si es 1 habilita para que se muestre mensaje si es cero
    ///                         deshabilita para que no se muestre el mensaje
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              07/Enero/2011 11:39
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private void Mostrar_Informacion(int Condicion)
    {
        try
        {
            if (Condicion == 1)
            {
                Lbl_Informacion.Enabled = true;
                Img_Warning.Visible = true;
            }
            else
            {
                Lbl_Informacion.Text = "";
                Lbl_Informacion.Enabled = false;
                Img_Warning.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Enabled = true;
            Img_Warning.Visible = true;
            Lbl_Informacion.Text = "Error: " + ex.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Estado_Inicial
    ///DESCRIPCION:             Colocar la pagina en un estatus inicial
    ///PARAMETROS:              
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              07/Enero/2011 11:40
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private void Estado_Inicial()
    {
        try
        {
            //Eliminar sesion
            Session.Remove("Dt_Parametros");
            Limpiar_Controles();
            Habilita_Controles("Inicial");
            Llena_Datos_Controles();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Llena_Datos_Controles
    ///DESCRIPCION:             Coloca los datos en los controles
    ///PARAMETROS:              
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              07/Enero/2011 11:40
    ///MODIFICO:                Jacqueline Ramírez Sierra
    ///FECHA_MODIFICO:          08/Marzo/2011
    ///CAUSA_MODIFICACION:      Mostrar campo Txt_Plazo_Surtir_Orden_Compra (campo nuevo).
    ///MODIFICO:                Salvador Hernandez Ramirez
    ///FECHA_MODIFICO:          29/Mayo/2011
    ///CAUSA_MODIFICACION:      Se agregó  "TryParse" con el fin de validar cuando el valor obtenido de la consulta tiene decimales
    ///                         esto se requiere por la mascara que se le asigno al campo "Txt_Salario_Minimo_Resguardado"
    ///*******************************************************************************
    private void Llena_Datos_Controles()
    {
        //Declaracion de Variables
        Cls_Cat_Com_Parametros_Negocio Parametros_Negocio = new Cls_Cat_Com_Parametros_Negocio(); //Variable para la capa de negocios
        DataTable Dt_Parametros = new DataTable(); //Tabla para los datos

        try
        {
            //Realizar la consulta
            Dt_Parametros = Parametros_Negocio.Consulta_Parametros();

            //Valida que se hayan encontrado registros
            if (Dt_Parametros.Rows.Count > 0)
            {
                //Valida que no este nulo el dato de saliario minimo resguardado
                if (Dt_Parametros.Rows[0][Cat_Com_Parametros.Campo_Cantidad_Sal_Min_Resguardo].ToString() != null)
                {
                    String Salario_Minimo = Dt_Parametros.Rows[0][Cat_Com_Parametros.Campo_Cantidad_Sal_Min_Resguardo].ToString();

                    int y;
                    if (int.TryParse(Salario_Minimo, out y)) // Se revisa si El saliario minimo contiene decimales si no, se le agregan decimales para que se muestre el valor entero con el .00
                    {
                        String Valor = Salario_Minimo + ".00";
                        Txt_Salario_Minimo_Resguardado.Text = Valor;
                    }
                    else
                    {
                        Txt_Salario_Minimo_Resguardado.Text = Salario_Minimo;
                    }

                    //Txt_Salario_Minimo_Resguardado.Text = Dt_Parametros.Rows[0][Cat_Com_Parametros.Campo_Cantidad_Sal_Min_Resguardo].ToString();
                }
                if (Dt_Parametros.Rows[0][Cat_Com_Parametros.Campo_Plazo_Surtir_Orden_Compra].ToString() != null)
                {
                    Txt_Plazo_Surtir_Orden_Compra.Text = Dt_Parametros.Rows[0][Cat_Com_Parametros.Campo_Plazo_Surtir_Orden_Compra].ToString();
                }
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Limpiar_Controles
    ///DESCRIPCION:             Limpiar los controles del formulario
    ///PARAMETROS:              
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              07/Enero/2011 11:40
    ///MODIFICO:                Jacqueline Ramírez Sierra
    ///FECHA_MODIFICO:          08-Marzo-2011
    ///CAUSA_MODIFICACION:      Limpiar nuevo campo Txt_Plazo_Surtir_Orden_Compra
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            //Limpiar los controles
            //Txt_Parametro_ID.Text = "";
            Txt_Salario_Minimo_Resguardado.Text = "";
            Txt_Plazo_Surtir_Orden_Compra.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Valida_Datos
    ///DESCRIPCION:             Validar que esten llenos los datos del formulario
    ///PARAMETROS:              
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              07/Enero/2011 11:42
    ///MODIFICO:                Jacqueline Ramírez Sierra
    ///FECHA_MODIFICO:          08/Marzo/2011
    ///CAUSA_MODIFICACION:      Valida datos de Txt_Plazo_Surtir_Orden_Compra(campo nuevo)
    ///*******************************************************************************
    private String Valida_Datos()
    {
        //Declaracion de variables
        String Resultado = String.Empty; //Variable para el resultado

        try
        {
            //Verificar si esta asignado el nombre
            if (Txt_Salario_Minimo_Resguardado.Text.Trim() == "" || Txt_Salario_Minimo_Resguardado.Text.Trim() == String.Empty)
                Resultado = "Favor de proporcionar el salario minimo resguardado";

            if (Txt_Plazo_Surtir_Orden_Compra.Text.Trim() == "" || Txt_Plazo_Surtir_Orden_Compra.Text.Trim() == String.Empty)
                Resultado = "Favor de proporcionar el plazo minimo a Surtir la orden de compra";

            if (Txt_Plazo_Surtir_Orden_Compra.Text.Length > 1000)
            {
                Lbl_Informacion.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + La clave debe ser de máximo 1000 caracteresbr />";
            }

            //Entregar resultado
            return Resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Alta_Parametros
    ///DESCRIPCION:             Dar de alta un nuevo Parametro
    ///PARAMETROS:              
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              07/Enero/2010 11:44
    ///MODIFICO:                Jacqueline Ramírez Sierra
    ///FECHA_MODIFICO:          08/Marzo/2011
    ///CAUSA_MODIFICACION:      Declaracion de campo Txt_Plazo_Surtir_Orden_Compra (campo nuevo) para dar de alta el parametro
    ///*******************************************************************************
    private void Alta_Parametros()
    {
        //Declaracion de variables
        Cls_Cat_Com_Parametros_Negocio Parametros_Negocio = new Cls_Cat_Com_Parametros_Negocio(); //Variable para la capa de negocios

        try
        {
            //Asignar propiedades
            Parametros_Negocio.P_Salario_Minimo_Resguardado = Txt_Salario_Minimo_Resguardado.Text.Trim();
            Parametros_Negocio.P_Plazo_Surtir_Orden_Compra = Txt_Plazo_Surtir_Orden_Compra.Text.Trim();
            Parametros_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;


            //Dar de alta el Parametro
            Parametros_Negocio.Alta_Parametros();
            Estado_Inicial();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Cambio_Parametros
    ///DESCRIPCION:             Modificar un Parametro existente
    ///PARAMETROS:              
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              07/Enero/2011 11:46
    ///MODIFICO:                Jacqueline Ramírez Sierra
    ///FECHA_MODIFICO:          08/Marzo/2011
    ///CAUSA_MODIFICACION:      Nuevo campo (Txt_Plazo_Surtir_Orden_Compra)
    ///*******************************************************************************
    private void Cambio_Parametros()
    {
        //Declaracion de variables
        Cls_Cat_Com_Parametros_Negocio Parametros_Negocio = new Cls_Cat_Com_Parametros_Negocio(); //Variable para la capa de negocios

        try
        {
            //Asignar propiedades
            //Parametros_Negocio.P_Parametro_ID = Txt_Parametro_ID.Text.Trim();
            Parametros_Negocio.P_Salario_Minimo_Resguardado = Txt_Salario_Minimo_Resguardado.Text.Trim();
            Parametros_Negocio.P_Plazo_Surtir_Orden_Compra = Txt_Plazo_Surtir_Orden_Compra.Text.Trim();
            Parametros_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;

            //Cambiar el Parametro
            Parametros_Negocio.Cambio_Parametros();
            Estado_Inicial();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Habilita_Controles
    ///DESCRIPCION:             Habilitar los controles del formulario de acuerdo al modo
    ///PARAMETROS:              
    ///CREO:                    Noe Mosqueda Valadez
    ///FECHA_CREO:              22/Octubre/2010 13:11
    ///MODIFICO:                Jacqueline Ramírez Sierra
    ///FECHA_MODIFICO:          08/Marzo/2011
    ///CAUSA_MODIFICACION:      Agregar el campo 
    ///*******************************************************************************
    private void Habilita_Controles(string Modo)
    {
        Boolean Habilitar = false;
        try
        {
            switch (Modo)
            {
                case "Inicial":
                    Habilitar = false;
                    Btn_Modificar.Enabled = true;
                    Btn_Modificar.ImageUrl = "../imagenes/paginas/icono_modificar.png";
                    Btn_Salir.ImageUrl = "../imagenes/paginas/icono_salir.png";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";

                    Configuracion_Acceso("Frm_Cat_Com_Parametros.aspx");
                    break;

                case "Modificar":
                    Habilitar = true;

                    //Verificar el modo
                    Btn_Modificar.Enabled = true;
                    Btn_Modificar.ImageUrl = "../imagenes/paginas/icono_guardar.png";
                    Btn_Modificar.ToolTip = "Guardar";

                    Btn_Salir.ImageUrl = "../imagenes/paginas/icono_cancelar.png";
                    Btn_Salir.ToolTip = "Cancelar";
                    break;

                default: break;
            }

            Txt_Salario_Minimo_Resguardado.Enabled = Habilitar;
            Txt_Plazo_Surtir_Orden_Compra.Enabled = Habilitar;
            Cmb_Partida_Especifica_Listados.Enabled = Habilitar;
            Cmb_Partida_Generica_Listados.Enabled = Habilitar;


        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Generico
    ///DESCRIPCIÓN          : Llena el Combo de Partidas Genericas con los existentes en la Base de Datos.
    ///PROPIEDADES          : 
    ///CREO: Jacqueline Ramìrez Sierra
    ///FECHA_CREO: 12/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    public void Llenar_Combo_Generico()
    {
        Cls_Cat_Com_Parametros_Negocio Parametros_Negocio = new Cls_Cat_Com_Parametros_Negocio();
        Cmb_Partida_Generica_Listados.Items.Clear();
        DataTable Data_Table = Parametros_Negocio.Consultar_Generico();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Partida_Generica_Listados, Data_Table);

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Especificas
    ///DESCRIPCIÓN          : Llena el Combo de Partidas Especificas con los existentes en la Base de Datos.
    ///PROPIEDADES          : 
    ///CREO: Jacqueline Ramìrez Sierra
    ///FECHA_CREO: 12/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    public void Llenar_Combo_Especificas()
    {
        Cls_Cat_Com_Parametros_Negocio Parametros_Negocio = new Cls_Cat_Com_Parametros_Negocio();
        Cmb_Partida_Especifica_Listados.Items.Clear();
        Parametros_Negocio.P_Partida_Generica_ID = Cmb_Partida_Generica_Listados.SelectedValue.ToString().Trim();
        DataTable Data_Table = Parametros_Negocio.Consultar_Especificas();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Partida_Especifica_Listados, Data_Table);

    }
    #endregion

    #region (Eventos)

    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        //Declaracion de variables
        String Validacion = String.Empty; //Variable que contiene el resultado de la validacion

        try
        {
        
            //Verificar el tooltip del boton
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                Habilita_Controles("Modificar");
                Cmb_Partida_Especifica_Listados.Enabled = false;
            }
            else
            {
                //Verificar si la validacion es correcta
                Validacion = Valida_Datos();
                if (Validacion == "" || Validacion == String.Empty)
                {
                    //Valida si el registro de los parametros esta creado, si no esta ejecuta
                    //el proceso de Alta pero si ya esta ejecuta el proceso de Cambio
                    if (Txt_Salario_Minimo_Resguardado.Text.Trim() != "")
                        if (Txt_Plazo_Surtir_Orden_Compra.Text.Trim() != "")
                            //    Alta_Parametros();
                            //else
                            Cambio_Parametros();
                }
                else
                {
                    Lbl_Informacion.Visible = true;
                    Img_Warning.Visible = true;
                    Lbl_Informacion.Text = Validacion;
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Text = "Error: (Btn_Modificar_Click)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //Verificar el mensaje de tooltip del boton
            if (Btn_Salir.ToolTip == "Cancelar")
                Estado_Inicial();
            else
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        catch (Exception ex)
        {
            Lbl_Informacion.Text = "Error: (Btn_Salir_Click)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }
    #endregion

    //protected void Cmb_Partida_Generica_Listados_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //Cls_Cat_Com_Parametros_Negocio Parametros_Negocio = new Cls_Cat_Com_Parametros_Negocio(); //Variable para la capa de negocios
    //    //int Partida_Generica_ID = Cmb_Partida_Generica_Listados.SelectedIndex;
    //    //Parametros_Negocio.P_Partida_Generica_ID = Partida_Generica_ID.ToString();
    //    //Parametros_Negocio.Consultar_Especificas();

    //    Llenar_Combo_Especificas();
    //    Cmb_Partida_Especifica_Listados.Enabled = true;

    //}
    protected void Cmb_Partida_Generica_Listados_SelectedIndexChanged(object sender, EventArgs e)
    {

        Llenar_Combo_Especificas();
        Cmb_Partida_Especifica_Listados.Enabled = true;
    }

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
            Botones.Add(Btn_Modificar);

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
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion
}