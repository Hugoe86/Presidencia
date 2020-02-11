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
using Presidencia.Catalogo_Compras_Familias.Negocio;
using Presidencia.Sessiones;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Cat_Com_Familias : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;
    private const int Const_Estado_Modificar = 2;
    private const int Const_Estado_Buscar = 3;
    private static DataTable Dt_Familias = new DataTable();
    private static string M_Busqueda = "";
    #endregion

    #region (Page Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Estado_Botones(Const_Estado_Inicial);
                Estado_Inicial();
                Cargar_Combos();
            }
            else
            {
                Lbl_Error.Visible = false;
                Img_Error.Visible = false;
                Lbl_Error.Text = "";
            }
        }
        catch (Exception ex)
        {
            Lbl_Error.Text = "Error: (Page_Load)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }
    #endregion

    #region (Metodos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Mostrar_Informacion
    ///DESCRIPCION:             Habilita o deshabilita la muestra en pantalla del mensaje 
    ///                         de Mostrar_Informacion para el usuario
    ///PARAMETROS:              1.- Condicion, entero para saber si es 1 habilita para que se muestre mensaje si es cero
    ///                         deshabilita para que no se muestre el mensaje
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              05/Enero/2011 11:26
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
                Lbl_Error.Enabled = true;
                Img_Error.Visible = true;
            }
            else
            {
                Lbl_Error.Text = "";
                Lbl_Error.Enabled = false;
                Img_Error.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Lbl_Error.Enabled = true;
            Img_Error.Visible = true;
            Lbl_Error.Text = "Error: " + ex.ToString();
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combos
    ///DESCRIPCIÓN: metodo usado para caragar la informacion de todos los combos del formulario con la respectiva consulta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 08:46:12 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combos()
    {        
        Cmb_Estatus.Items.Add(new ListItem("<SELECCIONE>", "0"));
        Cmb_Estatus.Items.Add(new ListItem("ACTIVO", "ACTIVO"));
        Cmb_Estatus.Items.Add(new ListItem("INACTIVO", "INACTIVO"));
    }

    #region Estado_Botones
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones
    ///DESCRIPCIÓN: Metodo para establecer el estado de los botones y componentes del formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/02/2011 05:49:53 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Estado_Botones(int P_Estado)
    {
        switch (P_Estado)
        {
            case 0: //Estado inicial

                Mensaje_Error();
                Txt_Busqueda.Text = String.Empty;                
                Txt_Comentarios.Text = String.Empty;
                Txt_Abreviatura.Text = String.Empty;
                Txt_Familia_ID.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;

                Txt_Comentarios.Enabled = false;
                Txt_Abreviatura.Enabled = false;
                Txt_Familia_ID.Enabled = false;
                Txt_Nombre.Enabled = false;

                Cmb_Estatus.SelectedIndex = 0;                

                Cmb_Estatus.Enabled = false;                

                Grid_Familias.Enabled = true;
                Grid_Familias.SelectedIndex = (-1);

                Btn_Busqueda.Enabled = true;
                Btn_Eliminar.Enabled = true;
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;
                Btn_Busqueda.AlternateText = "Buscar";
                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Salir";

                Btn_Busqueda.ToolTip = "Consultar";
                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Salir";

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                
                Btn_Eliminar.Visible = true;
                Btn_Salir.Visible = true;
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = true;

                Configuracion_Acceso("Frm_Cat_Com_Familias.aspx");
                break;

            case 1: //Nuevo
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;
                Txt_Abreviatura.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;
                Txt_Familia_ID.Text = String.Empty;

                Cmb_Estatus.SelectedIndex = 0;                

                Txt_Comentarios.Enabled = true;
                Txt_Abreviatura.Enabled = true;
                Txt_Nombre.Enabled = true;

                Cmb_Estatus.Enabled = true;                

                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;

                Grid_Familias.SelectedIndex = (-1);
                Grid_Familias.Enabled = false;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Guardar";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Salir.ToolTip = "Cancelar";

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar_deshabilitado.png";

                Btn_Eliminar.Visible = false;
                Btn_Salir.Visible = true;
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = false;

                break;

            case 2: //Modificar
                Mensaje_Error();

                Txt_Comentarios.Enabled = true;
                Txt_Abreviatura.Enabled = true;
                Txt_Nombre.Enabled = true;

                Cmb_Estatus.Enabled = true;                

                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = true;
                Btn_Nuevo.Enabled = false;
                Btn_Salir.Enabled = true;

                Btn_Eliminar.AlternateText = "Eliminar";
                Btn_Modificar.AlternateText = "Guardar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Cancelar";

                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Cancelar";

                Grid_Familias.Enabled = false;

                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar_deshabilitado.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo_deshabilitado.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                Btn_Eliminar.Visible = false;
                Btn_Salir.Visible = true;
                Btn_Nuevo.Visible = false;
                Btn_Modificar.Visible = true;

                break;

            case 3: //Buscar
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;
                Txt_Abreviatura.Text = String.Empty;
                Txt_Familia_ID.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;

                Cmb_Estatus.SelectedIndex = 0;
                

                break;
        }
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Error.Text += P_Mensaje + "</br>";
    }
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Error.Text = "";
    }
    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Estado_Inicial
    ///DESCRIPCION:             Colocar la pagina en un estatus inicial
    ///PARAMETROS:              
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              05/Enero/2011 11:28
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private void Estado_Inicial()
    {
        try
        {
            //Eliminar sesion
            Session.Remove("Dt_Familias");            
            Estado_Botones(Const_Estado_Inicial);
            Cargar_Grid(0);
            Grid_Familias.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grido con estos datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 12:14:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Grid(int Page_Index)
    {
        try
        {
            Cls_Cat_Com_Familias_Negocio Familias_Negocio = new Cls_Cat_Com_Familias_Negocio();
            Familias_Negocio.P_Nombre = M_Busqueda;
            Dt_Familias = Familias_Negocio.Consulta_Familias();
            Grid_Familias.PageIndex = Page_Index;
            Grid_Familias.DataSource = Dt_Familias;
            Grid_Familias.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Llena_Grid_Familias
    ///DESCRIPCION:             Llenar el grid de los Familias de acuerdo a un 
    ///                         criterio de busqueda
    ///PARAMETROS:              1. Busqueda: Cadena de texto que tiene el elemento a buscar
    ///                         2. Pagina: Entero que indica la pagina del grid a visualizar
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              05/Enero/2011 11:28
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private void Llena_Grid_(String Busqueda, int Pagina)
    {
        //Declaracion de Variables
        Cls_Cat_Com_Familias_Negocio Familias_Negocio = new Cls_Cat_Com_Familias_Negocio(); //Variable para la capa de negocios

        try
        {
            //Verificar si hay una busqueda
            if (Busqueda != "")
            {
                //Verificar el texto de la busqueda
                if (Busqueda == "Todos")
                    Busqueda = "";

                //Realizar la consulta
                Familias_Negocio.P_Nombre = Busqueda;
                Dt_Familias = Familias_Negocio.Consulta_Familias();
            }
            else
            {
                //Verificar si hay variable de sesion
                if (Session["Dt_Familias"] != null)
                    Dt_Familias = (DataTable)Session["Dt_Familias"];
                else
                {
                    //Realizar consulta
                    Familias_Negocio.P_Nombre = "";
                    Dt_Familias = Familias_Negocio.Consulta_Familias();
                }
            }

            //Llenar el grid
            Grid_Familias.DataSource = Dt_Familias;

            //Verificar si hay una pagina
            if (Pagina > -1)
                Grid_Familias.PageIndex = Pagina;

            Grid_Familias.DataBind();

            //Colocar tabla en variable de sesion
            Session["Dt_Familias"] = Dt_Familias;
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
    ///FECHA_CREO:              05/Enero/2011 11:33
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private String Valida_Datos()
    {
        //Declaracion de variables
        String Resultado = String.Empty; //Variable para el resultado

        try
        {
            //Verificar si esta asignado el nombre
            if (Txt_Nombre.Text.Trim() == "" || Txt_Nombre.Text.Trim() == String.Empty)
                Resultado = "Favor de proporcionar el nombre de la Familia";
            
            //Verifica si se a tecleado la abreviatura
            if (Txt_Abreviatura.Text.Trim() == "" || Txt_Abreviatura.Text.Trim() == String.Empty)
                Resultado = "Favor de proporcionar la abreviatura de la Familia";

            //Verifica si se a seleccionado un estatus
            if (Cmb_Estatus.SelectedIndex < 1)
                Resultado = "Favor de seleccione el estatus de la Familia";


            //Entregar resultado
            return Resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Alta_Familias
    ///DESCRIPCION:             Dar de alta un nuevo Familia
    ///PARAMETROS:              
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              05/Enero/2011 11:34
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private void Alta_Familias()
    {
        //Declaracion de variables
        Cls_Cat_Com_Familias_Negocio Familias_Negocio = new Cls_Cat_Com_Familias_Negocio(); //Variable para la capa de negocios

        try
        {
            //Asignar propiedades
            Familias_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
            Familias_Negocio.P_Abreviatura = Txt_Abreviatura.Text.Trim();
            Familias_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;

            //Verificar si los comentarios son 250 caracteres maximo
            if (Txt_Comentarios.Text.Trim().Length > 250)
                Familias_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim().Substring(0, 250);
            else
                Familias_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();

            Familias_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;

            //Dar de alta el Familia
            Familias_Negocio.Alta_Familias();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Familias", "alert('El Alta de la Familia fue Exitosa');", true);
            Estado_Inicial();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Baja_Familias
    ///DESCRIPCION:             Eliminar un Familia existente
    ///PARAMETROS:              
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              05/Enero/2011 11:35
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private void Baja_Familias()
    {
        //Declaracion de variables
        Cls_Cat_Com_Familias_Negocio Familias_Negocio = new Cls_Cat_Com_Familias_Negocio(); //Variable para la capa de negocios

        try
        {
            //Asignar propiedades
            Familias_Negocio.P_Familia_ID = Txt_Familia_ID.Text.Trim();

            //Eliminar el Familia
            Familias_Negocio.Baja_Familias();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Familias", "alert('La Baja de la Familia fue Exitosa');", true);
            Estado_Inicial();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Cambio_Familias
    ///DESCRIPCION:             Modificar un Familia existente
    ///PARAMETROS:              
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              05/Enero/2011 11:36
    ///MODIFICO:                
    ///FECHA_MODIFICO:          
    ///CAUSA_MODIFICACION:      
    ///*******************************************************************************
    private void Cambio_Familias()
    {
        //Declaracion de variables
        Cls_Cat_Com_Familias_Negocio Familias_Negocio = new Cls_Cat_Com_Familias_Negocio(); //Variable para la capa de negocios

        try
        {
            //Asignar propiedades
            Familias_Negocio.P_Familia_ID = Txt_Familia_ID.Text.Trim();
            Familias_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
            Familias_Negocio.P_Abreviatura = Txt_Abreviatura.Text.Trim();
            Familias_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;

            //Verificar si los comentarios son 250 caracteres maximo
            if (Txt_Comentarios.Text.Trim().Length > 250)
                Familias_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim().Substring(0, 250);
            else
                Familias_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();

            Familias_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;

            //Cambiar el Familia
            Familias_Negocio.Cambio_Familias();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Subfamilias", "alert('La modificación de la Subfamilia fue Exitosa');", true);
            Estado_Inicial();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCION:    Llena_Datos_Controles
    ///DESCRIPCION:             Llenar los controles con los datos del Familia seleccionado
    ///PARAMETROS:              DataRow Dr_Familias: Renglon del grid seleccionado
    ///CREO:                    José Antonio López Hernández
    ///FECHA_CREO:              05/Enero/2011 11:37
    ///MODIFICO:                Jesus Toledo Rodriguez
    ///FECHA_MODIFICO:          25/Marzo/2011
    ///CAUSA_MODIFICACION:      Se cambio la fuente de los datos a casusa de modificacion en el grid
    ///*******************************************************************************
    private void Llena_Datos_Controles(DataRow Dr_Familias)
    {
        try
        {
            Txt_Familia_ID.Text = Dr_Familias[Cat_Com_Familias.Campo_Familia_ID].ToString();
            Txt_Nombre.Text = Dr_Familias[Cat_Com_Familias.Campo_Nombre].ToString();
            Txt_Abreviatura.Text = Dr_Familias[Cat_Com_Familias.Campo_Abreviatura].ToString();
            Cmb_Estatus.SelectedValue = Dr_Familias[Cat_Com_Familias.Campo_Estatus].ToString();
            Txt_Comentarios.Text = Dr_Familias[Cat_Com_Familias.Campo_Comentarios].ToString();
            
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }    
    #endregion

    #region (Grid)
    protected void Grid_Familias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //Llenar el grid con la pagina nueva
            Grid_Familias.SelectedIndex = (-1);
            Cargar_Grid(e.NewPageIndex);
        }
        catch (Exception ex)
        {
            Lbl_Error.Text = "Error: (Grid_Familias_PageIndexChanging)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }
    protected void Grid_Familias_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Familias.SelectedIndex > (-1))
            {
                Llena_Datos_Controles(Dt_Familias.Rows[Grid_Familias.SelectedIndex + (Grid_Familias.PageIndex * 5)]);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region (Eventos)
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        //Declaracion de variables
        String Validacion = String.Empty; //Variable que contiene el resultado de la validacion

        try
        {
            //Verificar el tooltip del boton
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                //Habilitar y limpiar
                Estado_Botones(Const_Estado_Nuevo);                
            }
            else
            {
                //Verificar si la validacion es correcta
                Validacion = Valida_Datos();
                if (Validacion == "" || Validacion == String.Empty)
                    Alta_Familias();
                else
                {
                    Lbl_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Error.Text = Validacion;
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Error.Text = "Error: (Btn_Nuevo_Click)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }

    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        //Declaracion de variables
        String Validacion = String.Empty; //Variable que contiene el resultado de la validacion

        try
        {
            //Verificar el tooltip del boton
            if (Btn_Modificar.ToolTip == "Modificar")
                //Verificar si se ha seleccionado un elemento
                if (Grid_Familias.SelectedIndex > -1)
                    Estado_Botones(Const_Estado_Modificar);
                else
                {
                    Lbl_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Error.Text = "Favor de seleccionar el Familia a modificar";
                }
            else
            {
                //Verificar si se ha seleccionado un elementos
                if (Grid_Familias.SelectedIndex > -1)
                {
                    //Verificar si la validacion es correcta
                    Validacion = Valida_Datos();
                    if (Validacion == "" || Validacion == String.Empty)
                        Cambio_Familias();
                    else
                    {
                        Lbl_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Error.Text = Validacion;
                    }
                }
                else
                {
                    Lbl_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Error.Text = "Favor de seleccionar el Familia a modificar";
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Error.Text = "Error: (Btn_Modificar_Click)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }

    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //Verificar si se ha seleccionado un elemento
            if (Grid_Familias.SelectedIndex > -1)
                Baja_Familias();
            else
            {
                Lbl_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Error.Text = "Favor de seleccionar el Familia a eliminar";
            }
        }
        catch (Exception ex)
        {
            Lbl_Error.Text = "Error: (Btn_Eliminar_Click)" + ex.ToString();
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
            Lbl_Error.Text = "Error: (Btn_Salir_Click)" + ex.ToString();
            Mostrar_Informacion(1);
        }
    }
    protected void Btn_Busqueda_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Estado_Botones(Const_Estado_Buscar);
            Grid_Familias.SelectedIndex = (-1);
            M_Busqueda = Txt_Busqueda.Text.Trim();            
            Cargar_Grid(0);

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
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