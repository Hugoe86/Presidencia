﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Compras_Unidades.Negocio;
using System.Data;

public partial class paginas_Compras_Frm_Cat_Com_Unidades : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 1;
    private const int Const_Estado_Modificar = 2;
    private const int Const_Estado_Buscar = 3;
    private static DataTable Dt_Unidades = new DataTable();
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
                Cargar_Grid(0);
            }
            Mensaje_Error();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Metodos
    
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grido con estos datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 12:14:35 p.m.
    ///MODIFICO:           Salvador Hernández Ramírez
    ///FECHA_MODIFICO:     20-Mayo-11
    ///CAUSA_MODIFICACIÓN: Se mostro la columna uno del grid antes de hacer el Grid_Unidades.DataBind();
    ///                    posteriromente se oculto la misma columna.
    ///*******************************************************************************
    private void Cargar_Grid(int Page_Index)
    {
        try
        {
            Cls_Cat_Com_Unidades_Negocio Unidades_Negocio = new Cls_Cat_Com_Unidades_Negocio();
            Unidades_Negocio.P_Nombre = M_Busqueda;
            Dt_Unidades = Unidades_Negocio.Consulta_Unidades();
            Grid_Unidades.PageIndex = Page_Index;
            Grid_Unidades.DataSource = Dt_Unidades;
            Grid_Unidades.Columns[1].Visible = true;
            Grid_Unidades.DataBind();
            Grid_Unidades.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }    
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: metodo que recibe el DataRow seleccionado de la grilla y carga los datos en los componetes de l formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 02:07:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos(DataRow Dr_Unidades)
    {
        try
        {
            Txt_Comentarios.Text = Dr_Unidades[Cat_Com_Unidades.Campo_Comentarios].ToString();
            Txt_Abreviatura.Text = Dr_Unidades[Cat_Com_Unidades.Campo_Abreviatura].ToString();
            Txt_ID.Text = Dr_Unidades[Cat_Com_Unidades.Campo_Unidad_ID].ToString();
            Txt_Nombre.Text = Dr_Unidades[Cat_Com_Unidades.Campo_Nombre].ToString();
            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
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
                //M_Busqueda = String.Empty;
                Txt_Comentarios.Text = String.Empty;
                Txt_Abreviatura.Text = String.Empty;
                Txt_ID.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;

                Txt_Comentarios.Enabled = false;
                Txt_Abreviatura.Enabled = false;
                Txt_ID.Enabled = false;
                Txt_Nombre.Enabled = false;                

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

                Grid_Unidades.Enabled = true;
                Grid_Unidades.SelectedIndex = (-1);

                Btn_Busqueda.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                Btn_Eliminar.Visible = true;
                Btn_Salir.Visible = true;
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = true;

                Configuracion_Acceso("Frm_Cat_Com_Unidades.aspx");
                break;

            case 1: //Nuevo
                Mensaje_Error();

                Txt_Comentarios.Text = String.Empty;
                Txt_Abreviatura.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;
                Txt_ID.Text = String.Empty;
                
                Txt_Comentarios.Enabled = true;
                Txt_Abreviatura.Enabled = true;
                Txt_Nombre.Enabled = true;

                Btn_Eliminar.Enabled = false;
                Btn_Modificar.Enabled = false;
                Btn_Nuevo.Enabled = true;
                Btn_Salir.Enabled = true;

                Grid_Unidades.SelectedIndex = (-1);
                Grid_Unidades.Enabled = false;

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

                Grid_Unidades.Enabled = false;

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
                Txt_ID.Text = String.Empty;
                Txt_Nombre.Text = String.Empty;                

                break;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Datos
    ///DESCRIPCIÓN: Guardar datos para dar de alta un nuevo registro de un servicio
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 10:45:17 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Resultado = true;
        try
        {
            if (Txt_Nombre.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar el nombre de la Unidad");
            }
            if (Txt_Abreviatura.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar la Abreviatura de la Unidad");
            }           
            
            if (!Txt_Comentarios.Text.Trim().Equals(""))
            {
                if (Txt_Comentarios.Text.Trim().Length >= 250)
                {
                    Txt_Comentarios.Text = Txt_Comentarios.Text.Trim().Substring(0, 250);
                }
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Resultado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Alta_Unidad
    ///DESCRIPCIÓN: Realiza el alta de un nuevo registro de una unidad
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/02/2011 19:36:45
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Alta_Unidad()
    {
        try
        {
            if (Validar_Datos())
            {
                Cls_Cat_Com_Unidades_Negocio Unidades_Negocio = new Cls_Cat_Com_Unidades_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    Unidades_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                }
                Unidades_Negocio.P_Abreviatura = Txt_Abreviatura.Text.Trim();
                
                Unidades_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
                Unidades_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Unidades_Negocio.Alta_Unidades();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Unidades", "alert('El Alta de la Unidad fue Exitosa');", true);
                Estado_Botones(Const_Estado_Inicial);
                Txt_Busqueda.Text = "";
                Cargar_Grid(0);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Baja
    ///DESCRIPCIÓN: dar de baja un registro de la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/04/2011 11:25:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Baja_Unidades()
    {
        try
        {
            Cls_Cat_Com_Unidades_Negocio Unidades_Negocio = new Cls_Cat_Com_Unidades_Negocio();
            Unidades_Negocio.P_Unidad_ID = HttpUtility.HtmlDecode(Grid_Unidades.SelectedRow.Cells[1].Text.Trim());
            Unidades_Negocio.Baja_Unidades();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Unidades", "alert('La Baja de la Unidad fue Exitosa');", true);
            Estado_Botones(Const_Estado_Inicial);
            Txt_Busqueda.Text = "";
            Cargar_Grid(0);
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Modificar
    ///DESCRIPCIÓN: Modifica un registro y lo guarda en la base de datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/04/2011 11:58:30 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Modificar_Unidad()
    {
        try
        {
            if (Validar_Datos())
            {
                Cls_Cat_Com_Unidades_Negocio Unidades_Negocio = new Cls_Cat_Com_Unidades_Negocio();
                if (!Txt_Comentarios.Text.Equals(""))
                {
                    Unidades_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
                }
               
                Unidades_Negocio.P_Unidad_ID = HttpUtility.HtmlDecode(Grid_Unidades.SelectedRow.Cells[1].Text.Trim());
                Unidades_Negocio.P_Abreviatura = Txt_Abreviatura.Text.Trim();                
                Unidades_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
                Unidades_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Unidades_Negocio.Cambio_Unidades();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Unidades", "alert('La modificación de la Unidad fue Exitosa');", true);
                Estado_Botones(Const_Estado_Inicial);
                Txt_Busqueda.Text = "";
                Cargar_Grid(0);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }
    }
    #endregion

    #region Eventos
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Estado_Botones(Const_Estado_Nuevo);
            }
            else
            {
                Alta_Unidad();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Unidades.SelectedIndex > (-1))
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Estado_Botones(Const_Estado_Modificar);
                }
                else
                {
                    Modificar_Unidad();
                }
            }
            else
            {
                Mensaje_Error("Favor de seleccionar la Unidad a modificar");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);

        }
    }
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Unidades.SelectedIndex > (-1))
            {
                Baja_Unidades();
            }
            else
            {
                Mensaje_Error("Favor de seleccionar la Unidad a eliminar");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Estado_Botones(Const_Estado_Inicial);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Btn_Busqueda_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Estado_Botones(Const_Estado_Buscar);
            Grid_Unidades.SelectedIndex = (-1);
            M_Busqueda = Txt_Busqueda.Text.Trim();
            Cargar_Grid(0);

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Eventos Grid        
    
    protected void Grid_Unidades_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Unidades.SelectedIndex > (-1))
            {
                Cargar_Datos(Dt_Unidades.Rows[Grid_Unidades.SelectedIndex + (Grid_Unidades.PageIndex * 5)]);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Unidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Unidades.SelectedIndex = (-1);
            Cargar_Grid(e.NewPageIndex);
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
