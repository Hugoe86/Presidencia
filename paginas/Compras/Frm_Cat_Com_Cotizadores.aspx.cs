using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Cotizadores.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data;
using Presidencia.Catalogo_SAP_Conceptos.Negocio;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Cat_Com_Cotizadores : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;
    private const int Const_Grid_Cotizador = 2;
    private const int Const_Estado_Modificar = 3;

    private static DataTable Dt_giro_seleccionado = new DataTable();
    private static DataTable Dt_Cotizadores = new DataTable();
    private static DataTable Dt_Giros = new DataTable();

    private static string M_Busqueda = "";

    #endregion

    #region Page Load / Init

    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            
            if (!Page.IsPostBack)
            {
                Estado_Botones(Const_Estado_Inicial);
                
                Cargar_Grid_Cotizadores(0);
            }
            Mensaje_Error();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
            Estado_Botones(Const_Estado_Inicial);
        }
        
    }

    #endregion

    #region Metodos

    #region Metodos Generales
    
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          : Susana Trigueros Armenta
    ///FECHA_MODIFICO    : 7/OCT/2011
    ///CAUSA_MODIFICACION: Nuevos campos y estructura en el catalogo de Cotizadores
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Error.Text += P_Mensaje + "</br>";
        Div_Contenedor_error.Visible = true;
    }
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Error.Text = "";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones
    ///DESCRIPCIÓN: Metodo para establecer el estado de los botones y componentes del formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/02/2011 05:49:53 p.m.
    ///MODIFICO          : Susana Trigueros Armenta
    ///FECHA_MODIFICO    : 7/OCT/2011
    ///CAUSA_MODIFICACION: Nuevos campos y estructura en el catalogo de Cotizadores
    ///*******************************************************************************
    private void Estado_Botones(int P_Estado)
    {
         switch (P_Estado)
        {
            case 0: //Estado inicial                    

                Div_Listado_Cotizadores.Visible = true;
                Div_Datos_Cotizador.Visible = true;
                Grid_Cotizadores.Enabled = true;
                
                Txt_Busqueda.Text = String.Empty;
                
                Txt_Nombre_Cotizador.Text = "";
                Txt_No_Empleado.Text = "";
                Txt_Correo.Text = "";
                Txt_Password.Text = "";
                Txt_Password.Attributes.Add("value", Txt_Password.Text);
                Txt_Direccion_IP.Text = "";
                Grid_Cotizadores.Enabled = true;
                Grid_Cotizadores.SelectedIndex = (-1);


                Txt_Nombre_Cotizador.Enabled = false;
                Txt_No_Empleado.Enabled = false;
                Txt_Correo.Enabled = false;
                Txt_Password.Enabled =false;
                Txt_Direccion_IP.Enabled =false;

                Btn_Busqueda.AlternateText = "Buscar";
                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Inicio";

                Btn_Busqueda.ToolTip = "Consultar";
                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Inicio";

                Btn_Buscar_Empleado.Enabled = false;
                 

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                Btn_Busqueda.Enabled = true;
                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;

                Btn_Nuevo.Visible = true;
                Btn_Eliminar.Visible = false;
                Btn_Modificar.Visible = false;

                Configuracion_Acceso("Frm_Cat_Com_Cotizadores.aspx");
                break;

            case 1: //Nuevo                    

                Div_Listado_Cotizadores.Visible = true;
                Div_Datos_Cotizador.Visible = true;
                Grid_Cotizadores.Enabled = false;

                Txt_No_Empleado.Text = String.Empty;
                Txt_No_Empleado.Enabled = true;
                Txt_Nombre_Cotizador.Text = "";
                Txt_Correo.Text = "";
                Txt_Password.Text = "";
                Txt_Direccion_IP.Text="";

                Txt_Nombre_Cotizador.Enabled = false;
                Txt_No_Empleado.Enabled = true;
                Txt_Correo.Enabled = true;
                Txt_Password.Enabled = true;
                Txt_Direccion_IP.Enabled = true;

                Btn_Eliminar.Visible = false;
                Btn_Modificar.Visible = false;

                Grid_Cotizadores.SelectedIndex = (-1);
                Grid_Cotizadores.Enabled = false;

                Btn_Buscar_Empleado.Enabled = true;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Salir.ToolTip = "Cancelar";

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar_deshabilitado.png";

                break;

            case 2: //Grid Cotizador

                Div_Listado_Cotizadores.Visible =true;
                Div_Datos_Cotizador.Visible = true;
                Grid_Cotizadores.Enabled = true;

                Txt_No_Empleado.Visible = true;
                Txt_No_Empleado.Enabled = false;
                Txt_Nombre_Cotizador.Visible = true;
                Txt_Nombre_Cotizador.Enabled = false;
                Btn_Eliminar.Visible = true;
                Btn_Modificar.Visible = true;
                Btn_Eliminar.Enabled = true;
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Visible = false;

                Btn_Buscar_Empleado.Enabled = false;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Listado";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Listado";

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";


                break;

            case 3: //Modificar

                Div_Listado_Cotizadores.Visible = true;
                Grid_Cotizadores.Enabled = false;
                Div_Datos_Cotizador.Visible = true;

                Txt_Nombre_Cotizador.Visible = true;

                Btn_Eliminar.Visible = false;
                Btn_Modificar.Visible = true;
                Btn_Nuevo.Visible = false;

                Txt_Nombre_Cotizador.Enabled = false;
                Txt_No_Empleado.Enabled = false;
                Btn_Buscar_Empleado.Enabled = false;

                Txt_Correo.Enabled = true;
                Txt_Password.Enabled = true;
                Txt_Direccion_IP.Enabled = true;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Cancelar";

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_Nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                break;
        }
    }
    #endregion

    

    #endregion

    #region Metodos ABC
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar_Cotizador
    ///DESCRIPCIÓN: se actualizan los conceptos agregados al cotizador seleccionado
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 14/Marzo/2011 01:48:56 p.m.
    ///MODIFICO          : Susana Trigueros Armenta
    ///FECHA_MODIFICO    : 7/OCT/2011
    ///CAUSA_MODIFICACION: Nuevos campos y estructura en el catalogo de Cotizadores
    ///*******************************************************************************

    private void Modificar_Cotizador(Boolean Bln_Nuevo_Cotizador)
    {
        Div_Contenedor_error.Visible = false;
        Lbl_Error.Text = "";
        try
        {
            Cls_Cat_Com_Cotizadores_Negocio Cotizadores_Negocio = new Cls_Cat_Com_Cotizadores_Negocio();
            Validar_Cajas();
            Validar_Email();
            Validar_IP();
            Txt_Password.Attributes.Add("value", Txt_Password.Text);
            if (Div_Contenedor_error.Visible == false)
            {

                Cotizadores_Negocio.P_No_Empleado = Txt_No_Empleado.Text.Trim();
                Cotizadores_Negocio.P_Nombre_Completo = Txt_Nombre_Cotizador.Text.Trim();
                Cotizadores_Negocio.P_Correo = Txt_Correo.Text.Trim();
                Cotizadores_Negocio.P_Password = Txt_Password.Text.Trim();
                Cotizadores_Negocio.P_Direccion_IP = Txt_Direccion_IP.Text.Trim();


                Cotizadores_Negocio.Actualizar_Cotizadores();
                if (!Bln_Nuevo_Cotizador)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Cotizadores", "alert('La Modificacion del cotizador fue Exitosa');", true);
                    Estado_Botones(Const_Estado_Inicial);
                    Cargar_Grid_Cotizadores(0);
                }


            }//Fin del if Div_Contenedor_error.Visible == false
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message.ToString());
        }

    }//fin de Modificar
    
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Eliminar_Cotizador
    ///DESCRIPCIÓN: se da de baja un cotizador y sus detalles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 15/Abril/2011 11:46:26 a.m.
    ///MODIFICO          : Susana Trigueros Armenta
    ///FECHA_MODIFICO    : 7/OCT/2011
    ///CAUSA_MODIFICACION: Nuevos campos y estructura en el catalogo de Cotizadores
    ///*******************************************************************************        
    private void Eliminar_Cotizador()
    {
        try
        {
            Cls_Cat_Com_Cotizadores_Negocio Cotizadores_Negocio = new Cls_Cat_Com_Cotizadores_Negocio();//Variable de conexion con la capa de negocios.

            if (Txt_No_Empleado.Text.Trim() != String.Empty && Txt_No_Empleado.Text.Trim() != "")
            {
                Cotizadores_Negocio.P_No_Empleado = Txt_No_Empleado.Text.Trim();
                Cotizadores_Negocio.Eliminar_Cotizadores();
                Txt_No_Empleado.Text = "";
                Txt_Nombre_Cotizador.Text = "";
                Txt_Correo.Text = "";
                Txt_Password.Text = "";
                Txt_Password.Attributes.Add("value", Txt_Password.Text);
                Txt_Direccion_IP.Text = "";

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Catalogo de Cotizadores", "alert('La baja del Cotizador fue exitosa');", true);
                Estado_Botones(Const_Estado_Inicial);
                Cargar_Grid_Cotizadores(0);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region Metodos/Grids

    #region Metodos/Grid/Cotizadores
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grido con estos datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 12:14:35 p.m.
    ///MODIFICO          : Susana Trigueros Armenta
    ///FECHA_MODIFICO    : 7/OCT/2011
    ///CAUSA_MODIFICACION: Nuevos campos y estructura en el catalogo de Cotizadores
    ///*******************************************************************************
    private void Cargar_Grid_Cotizadores(int Page_Index)
    {
        try
        {
            Cls_Cat_Com_Cotizadores_Negocio Cotizadores_Negocio = new Cls_Cat_Com_Cotizadores_Negocio();
            Cotizadores_Negocio.P_Nombre_Completo = M_Busqueda;
            Dt_Cotizadores = Cotizadores_Negocio.Consulta_Datos();
            Grid_Cotizadores.PageIndex = Page_Index;
            Grid_Cotizadores.DataSource = Dt_Cotizadores;
            Grid_Cotizadores.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }


    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Cajas
    ///DESCRIPCIÓN: Metodo que valida que las cajas tengan contenido. 
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 06/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Validar_Cajas()
    {

        if (Txt_Correo.Text.Trim() == String.Empty)
        {
            Lbl_Error.Text += "El Usuario de Correo es obligatorio <br />";
            Div_Contenedor_error.Visible = true;
        }
        if (Txt_Password.Text.Trim() == String.Empty)
        {
            Lbl_Error.Text += "El Password de Correo es obligatorio <br />";
            Div_Contenedor_error.Visible = true;
        }


    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Email
    ///DESCRIPCIÓN: 
    ///PARAMETROS: Metodo que permite validar el correo ingresado por el usuario
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Validar_Email()
    {

        Regex Exp_Regular = new Regex("^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+$");
        Match Comparar = Exp_Regular.Match(Txt_Correo.Text);

        if (!Comparar.Success)
        {
            Lbl_Error.Text += "+ El contenido del Correo es incorrecto <br />";
            Div_Contenedor_error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_IP
    ///DESCRIPCIÓN: 
    ///PARAMETROS: Metodo que permite validar el correo ingresado por el usuario
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Validar_IP()
    {
        Regex Mascara_IP = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){2}(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]))$");

        Match Comparar = Mascara_IP.Match(Txt_Direccion_IP.Text);

        if (!Comparar.Success)
        {
            Lbl_Error.Text += "+ El contenido de la Direccion IP es incorrecto <br />";
            Div_Contenedor_error.Visible = true;
        }
    }

    #endregion

    #region Metodos/Operacion
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Conceptos
    ///DESCRIPCIÓN: crear el datatable de conceptos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 15/Abril/2011 12:49:48 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************                
    private void Crear_Tabla_Conceptos()//ICollection crearTabla()
    {
        try
        {
            //se crean las columnas de las tablas
            Dt_Giros.Columns.Add(new DataColumn("CONCEPTO_ID", typeof(string)));
            Dt_Giros.Columns.Add(new DataColumn("CLAVE", typeof(string)));
            Dt_Giros.Columns.Add(new DataColumn("DESCRIPCION", typeof(string)));
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }



    
    #endregion



    #region Eventos

    #region Eventos ABC

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Evento del Boton Modificar
    ///PARAMETROS:   
    ///CREO: Jacqueline Ramìrez Sierra
    ///FECHA_CREO: 14 feb 2011 
    ///MODIFICO          : Susana Trigueros Armenta
    ///FECHA_MODIFICO    : 7/OCT/2011
    ///CAUSA_MODIFICACION: Nuevos campos y estructura en el catalogo de Cotizadores
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Cls_Cat_Com_Cotizadores_Negocio Cotizadores_Negocio = new Cls_Cat_Com_Cotizadores_Negocio();

            if (Btn_Nuevo.AlternateText == "Nuevo")
            {
                Estado_Botones(Const_Estado_Nuevo);
            }
            else if (Btn_Nuevo.AlternateText == "Dar de Alta")
            {
                Validar_Cajas();
                Validar_Email();
                Validar_IP();
                Txt_Password.Attributes.Add("value", Txt_Password.Text);
                //Si pasa todas las Validaciones damos de alta el cotizador
                if (Div_Contenedor_error.Visible == false)
                {
                    Cotizadores_Negocio.P_No_Empleado = String.Format("{0:000000}", Convert.ToInt32(Txt_No_Empleado.Text));
                    Cotizadores_Negocio.P_Nombre_Completo = Txt_Nombre_Cotizador.Text.Trim();
                    Cotizadores_Negocio.P_Correo = Txt_Correo.Text.Trim();
                    Cotizadores_Negocio.P_Password = Txt_Password.Text.Trim();
                    
                    Cotizadores_Negocio.P_Direccion_IP = Txt_Direccion_IP.Text.Trim();
                    try
                    {
                        Cotizadores_Negocio.Alta_Cotizadores();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Cotizadores", "alert('El alta del cotizador fue Exitosa');", true);
                        Estado_Botones(Const_Estado_Inicial);
                        Cargar_Grid_Cotizadores(0);
                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Cotizadores", "alert('El alta del cotizador no fue Exitosa');", true);
                        Estado_Botones(Const_Estado_Inicial);
                        Cargar_Grid_Cotizadores(0);
                    }

                }
             
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del Boton Modificar
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO          : Susana Trigueros Armenta
    ///FECHA_MODIFICO    : 7/OCT/2011
    ///CAUSA_MODIFICACION: Nuevos campos y estructura en el catalogo de Cotizadores
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            if (Btn_Modificar.AlternateText == "Modificar")
            {
                Estado_Botones(Const_Estado_Modificar);
            }
            else if (Btn_Modificar.AlternateText == "Actualizar")
            {
                Modificar_Cotizador(false);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message.ToString());
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Btn_Eliminar_Click
    ///DESCRIPCION:             Boton para eliminar
    ///PARAMETROS:              
    ///CREO:                   Jacqueline Ramirez Sierra
    ///FECHA_CREO:              15/Febrero/2011
    ///MODIFICO          : Susana Trigueros Armenta
    ///FECHA_MODIFICO    : 7/OCT/2011
    ///CAUSA_MODIFICACION: Nuevos campos y estructura en el catalogo de Cotizadores
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Eliminar_Cotizador();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Btn_Salir_Click
    ///DESCRIPCION:             Boton para SALIR
    ///PARAMETROS:              
    ///CREO:                   Susana Trigueros Armenta
    ///FECHA_CREO:             7/OCT/2011
    ///MODIFICO          : 
    ///FECHA_MODIFICO    : 
    ///CAUSA_MODIFICACION: 
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Inicio")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");

        }
        else
        {
            Estado_Botones(Const_Estado_Inicial);
            Txt_No_Empleado.Text = "";
            Txt_Nombre_Cotizador.Text = "";
            Txt_Correo.Text = "";
            Txt_Password.Text = "";
            Txt_Password.Attributes.Add("value", Txt_Password.Text);
            Txt_Direccion_IP.Text = "";
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Btn_Busqueda_Click
    ///DESCRIPCION:             Boton para buscar
    ///PARAMETROS:              
    ///CREO:                   Susana Trigueros Armenta
    ///FECHA_CREO:             7/OCT/2011
    ///MODIFICO          : 
    ///FECHA_MODIFICO    : 
    ///CAUSA_MODIFICACION: 
    ///*******************************************************************************
    protected void Btn_Busqueda_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            M_Busqueda = Txt_Busqueda.Text.Trim();
            Cargar_Grid_Cotizadores(0);
            Estado_Botones(Const_Estado_Inicial);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion

    #region Eventos Grid Cotizadores

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Cotizadores_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo para cargar los datos del elemento seleccionado
    ///PARAMETROS:   
    ///CREO: Jacqueline Ramìrez Sierra
    ///FECHA_CREO: 16/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Cotizadores_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            Estado_Botones(Const_Grid_Cotizador);
            //se limpia la lista de conceptos
            Dt_Giros.Clear();
            //Obtenemos el Id del cotizador seleccionado
            GridViewRow selectedRow = Grid_Cotizadores.Rows[Grid_Cotizadores.SelectedIndex];
            String Id = Convert.ToString(selectedRow.Cells[1].Text);
            //Realizamos la consulta para modificar el cotizador
            Cls_Cat_Com_Cotizadores_Negocio Clase_Negocio = new Cls_Cat_Com_Cotizadores_Negocio();
            Clase_Negocio.P_Empleado_ID = Id;
            DataTable Dt_Datos_Cotizador = Clase_Negocio.Consulta_Cotizadores();

            if (Dt_Datos_Cotizador.Rows.Count != 0)
            {
                Txt_No_Empleado.Text = Dt_Datos_Cotizador.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString().Trim();
                Txt_Nombre_Cotizador.Text = Dt_Datos_Cotizador.Rows[0]["COTIZADOR"].ToString().Trim();
                Txt_Correo.Text = Dt_Datos_Cotizador.Rows[0][Cat_Com_Cotizadores.Campo_Correo].ToString().Trim();
                
                Txt_Password.Text = Dt_Datos_Cotizador.Rows[0][Cat_Com_Cotizadores.Campo_Password_Correo].ToString().Trim();
                Txt_Password.Attributes.Add("value", Txt_Password.Text);
                Txt_Direccion_IP.Text = Dt_Datos_Cotizador.Rows[0][Cat_Com_Cotizadores.Campo_IP_Correo_Saliente].ToString().Trim();
            }
            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Cotizadores_PageIndexChanging
    ///DESCRIPCIÓN: Metodo para manejar la paginacion del Grid_Cotizadores
    ///PARAMETROS:   
    ///CREO: Jacqueline Ramìrez Sierra
    ///FECHA_CREO: 16/Febrero/2011  
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Cotizadores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Cotizadores.SelectedIndex = (-1);
            Cargar_Grid_Cotizadores(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

  

    #endregion
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Btn_Buscar_Empleado_Click
    ///DESCRIPCION:             Boton para buscar el empleado
    ///PARAMETROS:              
    ///CREO:                   Susana Trigueros Armenta
    ///FECHA_CREO:             7/OCT/2011
    ///MODIFICO          : 
    ///FECHA_MODIFICO    : 
    ///CAUSA_MODIFICACION: 
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_error.Visible = false;
        Lbl_Error.Text = "";
        if (Txt_No_Empleado.Text.Trim() == String.Empty)
        {
            Div_Contenedor_error.Visible = true;
            Lbl_Error.Text = "Es necesario indicar el No. Empleado</br>";
        
        }
        //Consultamos si este empleado ya fue dado d alta como cotizador
         Cls_Cat_Com_Cotizadores_Negocio Clase_Negocio = new Cls_Cat_Com_Cotizadores_Negocio();
         Clase_Negocio.P_No_Empleado = String.Format("{0:000000}", Convert.ToInt32(Txt_No_Empleado.Text));
         DataTable Dt_Datos_Cotizador = Clase_Negocio.Consulta_Cotizadores();
         if (Dt_Datos_Cotizador.Rows.Count != 0)
         {
             Lbl_Error.Text = "Este Cotizador ya fue dado de Alta</br>";
             Div_Contenedor_error.Visible = true;
         }

        if (Div_Contenedor_error.Visible == false)
        {
            //Consultamos el Nombre del Cotizador de acuerdo al No. de Empleado

            Clase_Negocio.P_No_Empleado = String.Format("{0:000000}", Convert.ToInt32(Txt_No_Empleado.Text));
            DataTable Dt_Empleado = Clase_Negocio.Consultar_Nombre_Empleado();
            if (Dt_Empleado.Rows.Count != 0)
            {
                Txt_Nombre_Cotizador.Text = Dt_Empleado.Rows[0]["NOMBRE"].ToString().Trim();
                Txt_Correo.Text = "";
                Txt_Direccion_IP.Text = "";
                Txt_Password.Text = "";
            }
            else
            {
                Mensaje_Error("No se encontro el Numero de Empleado");
            }
        }
    }

    #endregion

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
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Eliminar);
            Botones.Add(Btn_Busqueda);

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
