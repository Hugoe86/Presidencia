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
using System.Data.OracleClient;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Nomina_Tipos_Pago.Negocio;

public partial class paginas_Nomina_Frm_Cat_Nom_Tipos_Pago : System.Web.UI.Page
{
    #region Variables Globales
    public static Int32 Contador_Consultas = 0;//   sirve para llevar el control de las consultas
    #endregion
    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Refresca la sesion del usuario logeado en el sistema
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //Valida que existe un usuario logueado en el sistema
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ViewState["SortDirection"] = "ASC";
                Session["Session_Tipos_Pago"] = null;

            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }

    }
#endregion

#region Metodos
    #region Metodos Generales
    ///**********************************************************************************************
    /// NOMBRE:         Inicializa_Controles
    /// DESCRIPCION :   Prepara los controles en la forma para que el usuario pueda
    ///                 realizar diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        :   Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  :   06/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///**********************************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpiar_Controles(); //Limpia los controles del forma
            Habilitar_Controles("Inicial");//Inicializa todos los controles
            Cargar_Grid_Tipo_Pago();
            Contador_Consultas = 0; // inicializo el contador para indicar que no se a realizo ninguna consulta
        }
        catch (Exception ex)
        {
            throw new Exception("Inicializa_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE:         Limpiar_Controles
    /// DESCRIPCION :   Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        :   Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  :   06/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            //para busqueta
            Txt_Busqueda.Text = "";

            //para datos generales
            Txt_Tipos_Pago_ID.Text = "";
            Txt_Nombre.Text = "";

            //  para el grid
            Grid_Tipos_Pago.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : 1.- Operacion: Indica la operación que se desea realizar por parte del usuario
    ///               si es una alta, modificacion
    ///                           
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 07/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va hacer habilitado para utilización del usuario
        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Cat_Nom_Tipos_Pago.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";


                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            //para datos generales
            Txt_Tipos_Pago_ID.Enabled = Habilitado;
            Txt_Nombre.Enabled = Habilitado;
            
            //mensajes de error
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }

        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString());
        }
    }
    #endregion
    #region(Control Acceso Pagina)
    /// ******************************************************************************
    /// NOMBRE:         Configuracion_Acceso
    /// DESCRIPCIÓN:    Habilita las operaciones que podrá realizar el usuario en la página.
    /// PARÁMETROS  :
    /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
    /// FECHA CREÓ  :   06/Enero/2012
    /// USUARIO MODIFICO  :
    /// FECHA MODIFICO    :
    /// CAUSA MODIFICACIÓN:
    /// ******************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Eliminar);
            Botones.Add(Btn_Buscar);

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
    /// NOMBRE:         Es_Numero
    /// DESCRIPCION :   Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS  :   Cadena.- El dato a evaluar si es numerico.
    /// CREO        :   Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  :   06/Enero/2012
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

    #region(Validacion)
    ///*******************************************************************************
    /// NOMBRE:         Validar_Datos
    /// DESCRIPCION :   Validar que se hallan proporcionado todos los datos
    /// CREO        :   Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  :   06/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        String Espacios_Blanco;
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";

        
        //para la seccion de datos generales
        if (Txt_Nombre.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el monto deseado.<br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }
    #endregion

    #region(metodos De consulta)
    ///*******************************************************************************
    /// NOMBRE:         Cargar_Grid_Tipo_Pago
    /// DESCRIPCION :   Consulta los tipos de pago que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        :   Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO  :   06/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Grid_Tipo_Pago()
    {
        Cls_Cat_Nom_Tipos_Pago_Negocio Rs_Consulta = new Cls_Cat_Nom_Tipos_Pago_Negocio();
        DataTable Dt_Consulta = new DataTable();
       try
        {
            Grid_Tipos_Pago.Columns[1].Visible = true;
           Dt_Consulta= Rs_Consulta.Consultar_Tipo_Pago();            
           Session["Session_Tipos_Pago"] = Dt_Consulta;
           Grid_Tipos_Pago.DataSource = (DataTable)Session["Session_Tipos_Pago"];
           Grid_Tipos_Pago.DataBind();
           Grid_Tipos_Pago.Columns[1].Visible = false;
           Grid_Tipos_Pago.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid_Tipo_Pago" + ex.Message.ToString(), ex);
        }
    }
    #endregion
    #region(Operaciones)
    ///*******************************************************************************
    ///NOMBRE:      Alta_Tipo_Pago
    ///DESCRIPCIÓN: toma la informacion para poder realizar la alta
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Alta_Tipo_Pago()
    {
        Cls_Cat_Nom_Tipos_Pago_Negocio Rs_Alta = new Cls_Cat_Nom_Tipos_Pago_Negocio();
        try
        {
            Rs_Alta.P_Nombre = Txt_Nombre.Text.ToUpper();
            Rs_Alta.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
            Rs_Alta.Alta_Tipo_Pago();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alta_Tipos_Pago", "alert('Alta Exitosa');", true);
            Inicializa_Controles();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE:      Modificacion_Tipo_Pago
    ///DESCRIPCIÓN: toma la informacion para poder realizar una modificacion
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Modificacion_Tipo_Pago()
    {
        Cls_Cat_Nom_Tipos_Pago_Negocio Rs_Modificacion = new Cls_Cat_Nom_Tipos_Pago_Negocio();
        try
        {
            Rs_Modificacion.P_Tipo_Pago_ID = Txt_Tipos_Pago_ID.Text;
            Rs_Modificacion.P_Nombre = Txt_Nombre.Text.ToUpper();
            Rs_Modificacion.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
            Rs_Modificacion.Modificar_Tipo_Pago();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modificacion_Tipos_Pago", "alert('Modificación Exitosa');", true);
            Inicializa_Controles();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE:      Eliminar_Tipo_Pago
    ///DESCRIPCIÓN: toma la informacion para poder eliminar un registro
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Eliminar_Tipo_Pago()
    {
        Cls_Cat_Nom_Tipos_Pago_Negocio Rs_Baja = new Cls_Cat_Nom_Tipos_Pago_Negocio();
        try
        {
            Rs_Baja.P_Tipo_Pago_ID = Txt_Tipos_Pago_ID.Text;
            Rs_Baja.Eliminar_Tipo_Pago();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Eliminar_Tipos_Pago", "alert('Baja Exitosa');", true);
            Inicializa_Controles();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion
#endregion

#region Eventos
    #region Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Habilita las cajas de texto necesarias para crear un Nuevo tipo de pago
    ///             y lo guardara en la base de datos
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Limpiar_Controles();
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
            }
            else
            {
                if (Validar_Datos())
                {
                    Alta_Tipo_Pago();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Habilita las cajas de texto necesarias para poder Modificar la informacion,
    ///             y actualiza el registro en la base de datos
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_Tipos_Pago_ID.Text == "")
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encuentra ningun registro seleccionado.<br> *Pruebe seleccionando algun registro de la tabla.";
                }
                else
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
            }
            else
            {
                if (Validar_Datos())
                {
                    Modificacion_Tipo_Pago();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }//else de validar_Datos
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Eliminara el registro de manera permanente de la base de datos
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            if (!string.IsNullOrEmpty(Txt_Tipos_Pago_ID.Text.Trim()))
            {
                Eliminar_Tipo_Pago();
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encuentra ningun registro seleccionado.<br> *Pruebe seleccionando algun registro de la tabla.";
            }//else de validar_Datos
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE:      Btn_Buscar_Click
    ///DESCRIPCIÓN: Busca el registro y lo carga en el grid
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Nom_Tipos_Pago_Negocio Rs_Consulta = new Cls_Cat_Nom_Tipos_Pago_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";


            if (!string.IsNullOrEmpty(Txt_Busqueda.Text))
            {
                Rs_Consulta.P_Nombre = Txt_Busqueda.Text.ToUpper();
                Dt_Consulta = Rs_Consulta.Consultar_Tipo_Pago();
                Grid_Tipos_Pago.Columns[1].Visible = true;
                Grid_Tipos_Pago.DataSource = Dt_Consulta;
                Grid_Tipos_Pago.DataBind();
                Grid_Tipos_Pago.Columns[1].Visible = false;
                Contador_Consultas++;// indica las consultas que se han realizado
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Ingrese el valor que desea buscar";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///***********************************************************************************
    ///NOMBRE:      Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///***********************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if ((Btn_Salir.ToolTip == "Salir") && (Contador_Consultas == 0)) // si no se realizan la consultas regresa a la pagina principal
                                                                                
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else if ((Btn_Salir.ToolTip == "Cancelar") || (Contador_Consultas > 0))
            {
                Inicializa_Controles(); //  Habilita los controles para la siguiente operación del usuario en el catálogo
            }
            else
            {
                Inicializa_Controles();
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion
#endregion

#region Grid
    ///***************************************************************************************
    /// NOMBRE:         Grid_Tipos_Pago_SelectedIndexChanged
    /// DESCRIPCION :   Consulta los datos de los tipos de pago seleccionada por el usuario
    /// CREO        :   Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO  :   06/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///***************************************************************************************
    protected void Grid_Tipos_Pago_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 Fila_Seleccionada = Grid_Tipos_Pago.SelectedIndex;//Obtenemos la fila seleccionada de la tabla de roles.

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Limpiar_Controles(); //Limpia los controles del la forma para poder agregar los valores del registro seleccionado

            Txt_Tipos_Pago_ID.Text = HttpUtility.HtmlDecode(Grid_Tipos_Pago.Rows[Fila_Seleccionada].Cells[1].Text);
            Txt_Nombre.Text = HttpUtility.HtmlDecode(Grid_Tipos_Pago.Rows[Fila_Seleccionada].Cells[2].Text);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    /// ********************************************************************************
    /// NOMBRE:         Grid_Movimiento_Sorting
    /// DESCRIPCIÓN:    Ordena las columnas en orden ascendente o descendente.
    /// CREÓ:           Hugo Enrique Ramirez Aguilera
    /// FECHA CREÓ:     06/Enero/2012
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// ********************************************************************************
    protected void Grid_Tipos_Pago_Sorting(object sender, GridViewSortEventArgs e)
    {
        Cargar_Grid_Tipo_Pago();
        DataTable Dt_Tipos_Pago = (Grid_Tipos_Pago.DataSource as DataTable);

        if (Dt_Tipos_Pago != null)
        {
            DataView Dv_Tipos_Pago = new DataView(Dt_Tipos_Pago);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Tipos_Pago.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Tipos_Pago.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Tipos_Pago.DataSource = Dv_Tipos_Pago;
            Grid_Tipos_Pago.DataBind();
        }
    }
#endregion
}
