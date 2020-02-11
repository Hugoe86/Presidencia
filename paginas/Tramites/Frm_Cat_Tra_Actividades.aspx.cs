using System;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Sessiones;
using System.Drawing;
using System.Drawing.Drawing2D;
using Presidencia.Constantes;
using AjaxControlToolkit;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text.RegularExpressions;
using System.Collections.Generic; 
using Presidencia.Tramites_Actividades.Negocio;

public partial class paginas_Tramites_Frm_Cat_Tra_Actividades : System.Web.UI.Page
{
    #region Load
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : 
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializar_Controles();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion

    #region Metodos generales
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
    ///               realizar diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializar_Controles()
    {
        try
        {
            Limpiar_Controles();
            Habilitar_Controles("Inicial");
            Cargar_Inspectores();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Hdf_Elemento_ID.Value = "";
            Txt_Busqueda.Text = "";
            Txt_Nombre.Text = "";

        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE:         Habilitar_Controles
    /// DESCRIPCION :   Habilita y Deshabilita los controles de la forma para prepara la página
    ///                 para a siguiente operación
    /// PARAMETROS:     1.- Operacion: Indica la operación que se desea realizar 
    /// CREO:           Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO:     11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado = false; ///Indica si el control de la forma va hacer habilitado para utilización del usuario
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
            //  mensajes de error
            Mostrar_Mensaje_Error(false);

            Txt_Nombre.Enabled = Habilitado;
            Txt_Busqueda.Enabled = !Habilitado;
            Btn_Buscar.Enabled = !Habilitado;
            Grid_Inspector.Enabled = !Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Mensaje_Error
    ///DESCRIPCIÓN          : se habilitan los mensajes de error
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Mostrar_Mensaje_Error(Boolean Accion)
    {
        try
        {
            Img_Error.Visible = Accion;
            Lbl_Mensaje_Error.Visible = Accion;
        }
        catch (Exception ex)
        {
            throw new Exception("Mostrar_Mensaje_Error " + ex.Message.ToString());
        }
    }
    #endregion

    #region Validaciones
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 05/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "";
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";
        Mostrar_Mensaje_Error(true);

        if (Txt_Nombre.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El nombre del inspector.<br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Modificar
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Modificar()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "";
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";
        Mostrar_Mensaje_Error(true);

        if (Hdf_Elemento_ID.Value == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione algun registro.<br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Inspectores
    ///DESCRIPCIÓN          : se cargara las areas
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Inspectores()
    {
        Cls_Cat_Tra_Actividades_Negocio Negocio_Consulta = new Cls_Cat_Tra_Actividades_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Dt_Consulta = Negocio_Consulta.Consultar_Actividades();
            Cargar_Grid_Inspectores(Dt_Consulta);
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Area " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Inspectores
    ///DESCRIPCIÓN          : se cargara el grid
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Grid_Inspectores(DataTable Dt_Consulta)
    {
        try
        {
            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Grid_Inspector.Columns[1].Visible = true;
                Grid_Inspector.DataSource = Dt_Consulta;
                Grid_Inspector.DataBind();
                Grid_Inspector.Columns[1].Visible = false;
                Grid_Inspector.SelectedIndex = -1;
            }
            else
            {
                Grid_Inspector.Columns[1].Visible = true;
                Grid_Inspector.DataSource = new DataTable();
                Grid_Inspector.DataBind();
                Grid_Inspector.Columns[1].Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Gridr_Area " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Alta
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Alta()
    {
        Cls_Cat_Tra_Actividades_Negocio Negocio_Alta = new Cls_Cat_Tra_Actividades_Negocio();
        try
        {
            Negocio_Alta.P_Nombre = Txt_Nombre.Text;
            Negocio_Alta.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Negocio_Alta.Alta();
        }
        catch (Exception ex)
        {
            throw new Exception("Alta " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Modificar
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Modificar()
    {
        Cls_Cat_Tra_Actividades_Negocio Negocio_Modificar = new Cls_Cat_Tra_Actividades_Negocio();
        try
        {
            Negocio_Modificar.P_Actividad_ID = Hdf_Elemento_ID.Value;
            Negocio_Modificar.P_Nombre = Txt_Nombre.Text;
            Negocio_Modificar.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Negocio_Modificar.Modificar();
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Eliminar
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Eliminar()
    {
        Cls_Cat_Tra_Actividades_Negocio Negocio_Eliminar = new Cls_Cat_Tra_Actividades_Negocio();
        try
        {
            Negocio_Eliminar.P_Actividad_ID = Hdf_Elemento_ID.Value;
            Negocio_Eliminar.Eliminar();
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Buscar
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Buscar()
    {
        Cls_Cat_Tra_Actividades_Negocio Negocio_Buscar = new Cls_Cat_Tra_Actividades_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Negocio_Buscar.P_Nombre = Txt_Busqueda.Text;
            Dt_Consulta = Negocio_Buscar.Consultar_Actividades();
            Cargar_Grid_Inspectores(Dt_Consulta);
        }
        catch (Exception ex)
        {
            throw new Exception("Buscar " + ex.Message.ToString());
        }
    }
    #endregion

    #region Botones
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Nuevo_Click
    /// DESCRIPCION : realiza la alta del usuario
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);

            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Limpiar_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
            }
            else
            {
                if (Validar_Datos())
                {
                    Alta();
                    Inicializar_Controles();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Nuevo_Click", "alert('Alta Exitosa');", true);
                }
                else
                {
                    Mostrar_Mensaje_Error(true);
                }
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Modificar_Click
    /// DESCRIPCION : realiza la modificacion del usuario
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);

            if (Btn_Modificar.ToolTip == "Modificar" && Hdf_Elemento_ID.Value != "")
            {
                Habilitar_Controles("Modificar"); //Habilita los controles para la introducción de datos por parte del usuario
            }
            else
            {
                if (Validar_Datos_Modificar())
                {
                    Modificar();
                    Inicializar_Controles();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Modificar_Click", "alert('Modificacion Exitosa');", true);
                }
                else
                {
                    Mostrar_Mensaje_Error(true);
                }
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Click
    /// DESCRIPCION : realiza la baja
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);

            if (!string.IsNullOrEmpty(Hdf_Elemento_ID.Value))
            {
                Eliminar();
                Inicializar_Controles();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Eliminar_Click", "alert('Baja Exitosa');", true);
            }
            else
            {
                Mostrar_Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "Seleccione el registro que se eliminara <br>";
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Salir_Click
    /// DESCRIPCION : 
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Cancelar")
            {
                Inicializar_Controles();
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Buscar_Click
    /// DESCRIPCION : 
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);
            if (!String.IsNullOrEmpty(Txt_Busqueda.Text))
            {
                Buscar();
            }
            else
            {
                Mostrar_Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "Ingrese el nombre a buscar";
                Cargar_Inspectores();
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion

    #region Grid
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Areas_SelectedIndexChanged
    /// DESCRIPCION : se cargara la informacion del grid en las cajas de texto
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Inspector_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Limpiar_Controles();
            GridViewRow selectedRow = Grid_Inspector.Rows[Grid_Inspector.SelectedIndex];
            Hdf_Elemento_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
            Txt_Nombre.Text = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion
}
