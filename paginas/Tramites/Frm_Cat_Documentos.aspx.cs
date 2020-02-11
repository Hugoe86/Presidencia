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
using Presidencia.Sessiones;
public partial class Frm_Cat_Documentos : System.Web.UI.Page
{
    

    #region Page Load / Init
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN:
    ///PARAMETROS:   
    ///CREO:       
    ///FECHA_CREO: 
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
         
        Mostrar_Informacion("", false);

        if (!IsPostBack)
        {           
            
            Refrescar_Tabla_Datos();
            Habilitar_Controles("Inicial");
            Limpiar_Formulario();
        }
    }
    # endregion

    #region Eventos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN:
    ///PARAMETROS:   
    ///CREO:       
    ///FECHA_CREO: 
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Documentos_Negocio Negocio = new Cls_Cat_Documentos_Negocio();
        try
        {
            if (Btn_Nuevo.ToolTip.Contains("Nuevo"))
            {
                Habilitar_Controles("Nuevo");
                Limpiar_Formulario();
            }
            else
            {

                if (Validaciones())
                {
                    Negocio = Asignar_Valor_Attributos();
                    Habilitar_Controles("Inicial");
                    Negocio.Alta();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('El registro fue dado de alta');", true);
                    Refrescar_Tabla_Datos();
                    Limpiar_Formulario();
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN:
    ///PARAMETROS:   
    ///CREO:       
    ///FECHA_CREO: 
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Documentos_Negocio Negocio = new Cls_Cat_Documentos_Negocio();
        try
        {
            if (String.IsNullOrEmpty(Hdf_Id_Documento.Value))
            {
                //verifica que este seleccionado un registro 
                Mostrar_Informacion("+ Debe seleccionar un registro de la tabla!!", true);
            }
            else
            {
                if (Btn_Modificar.ToolTip.Contains("Modificar"))
                {
                    Habilitar_Controles("Modificar");
                }
                else
                {
                    if (Validaciones())
                    {
                        Negocio = Asignar_Valor_Attributos();
                        Habilitar_Controles("Inicial");
                        Negocio.Modificar();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('El registro fue modificado');", true);
                        Refrescar_Tabla_Datos();
                        Limpiar_Formulario();
                    }
                }

            }
        }
        catch (Exception ex)
        {
             Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN:
    ///PARAMETROS:   
    ///CREO:       
    ///FECHA_CREO: 
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Documentos_Negocio Negocio = new Cls_Cat_Documentos_Negocio();
        try
        {
            if (String.IsNullOrEmpty(Hdf_Id_Documento.Value))
            {
                Mostrar_Informacion("+ Debe seleccionar un registro de la tabla!!", true);
            }
            else
            {
                Negocio = Asignar_Valor_Attributos();
                Negocio.Eliminar();
                Refrescar_Tabla_Datos();

                if (Grid_Datos.Rows.Count > 0)
                    Grid_Datos.SelectedIndex = -1;

                Limpiar_Formulario();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('El registro ha sido eliminado');", true);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:
    ///PARAMETROS:   
    ///CREO:       
    ///FECHA_CREO: 
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        if (Btn_Salir.ToolTip.Contains("Cancelar"))
        {
            Habilitar_Controles("Inicial");
            Limpiar_Formulario();
        }
        else
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN:
    ///PARAMETROS:   
    ///CREO:       
    ///FECHA_CREO: 
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Documentos_Negocio Negocio = new Cls_Cat_Documentos_Negocio();
        try
        {
            Negocio = Asignar_Valor_Attributos();
            DataSet data = Negocio.Busqueda_Por_Nombre();
            if (data != null)
            {
                Grid_Datos.Columns[1].Visible = true;
                Grid_Datos.DataSource = data;
                Grid_Datos.DataBind();
                Grid_Datos.Columns[1].Visible = false;
            }
            else
            {
                Grid_Datos.Columns[1].Visible = true;
                Grid_Datos.DataSource = new DataTable();
                Grid_Datos.DataBind();
                Grid_Datos.Columns[1].Visible = false;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }
    }
    # endregion

    #region Grid
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Datos_PageIndexChanging
    ///DESCRIPCIÓN:
    ///PARAMETROS:   
    ///CREO:       
    ///FECHA_CREO: 
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Datos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Datos.PageIndex = e.NewPageIndex;
            Grid_Datos.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Datos_SelectedIndexChanged
    ///DESCRIPCIÓN:
    ///PARAMETROS:   
    ///CREO:       
    ///FECHA_CREO: 
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Datos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Btn_Modificar.Enabled = true;
            Btn_Eliminar.Enabled = true;

            //  se obtiene la informacion del grid
            Grid_Datos.Columns[1].Visible = true;
            GridViewRow row = Grid_Datos.SelectedRow;
            Grid_Datos.Columns[1].Visible = false;

            Hdf_Id_Documento.Value = HttpUtility.HtmlDecode(row.Cells[1].Text.ToString());
            Txt_Nombre.Text = HttpUtility.HtmlDecode(row.Cells[2].Text.ToString());
            Txt_Comentarios.Text = HttpUtility.HtmlDecode(row.Cells[3].Text.ToString());
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    /*******************************************************************************
    * NOMBRE DE LA FUNCIÓN: Refrescar_Tabla_Datos
    * DESCRIPCIÓN: Refresca el grid con los registros
    * que existen en la base de datos
    * RETORNA: 
    * CREO: Gustavo Angeles Cruz
    * FECHA_CREO: 23/Agosto/2010 
    * MODIFICO:
    * FECHA_MODIFICO:
    * CAUSA_MODIFICACIÓN:
    ********************************************************************************/
    private void Refrescar_Tabla_Datos()
    {
        Cls_Cat_Documentos_Negocio Negocio = new Cls_Cat_Documentos_Negocio();
        try
        {
            DataSet Data_Set = Negocio.Consultar_Todo();
            if (Data_Set != null)
            {
                //  se ordena la informacion por nombre de documento
                DataTable Dt_Ordenar_Campos = Data_Set.Tables[0];
                DataView Dv_Ordenar = new DataView(Dt_Ordenar_Campos);
                Dv_Ordenar.Sort = Cat_Tra_Documentos.Campo_Nombre;
                Dt_Ordenar_Campos = Dv_Ordenar.ToTable();

                Grid_Datos.Columns[1].Visible = true;
                Grid_Datos.DataSource = Dt_Ordenar_Campos;
                Grid_Datos.DataBind();
                Grid_Datos.Columns[1].Visible = false;
            }
            else
            {
                Grid_Datos.Columns[1].Visible = true;
                Grid_Datos.DataSource = new DataTable();
                Grid_Datos.DataBind();
                Grid_Datos.Columns[1].Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion

    #region Métodos
    /*******************************************************************************
     * NOMBRE DE LA FUNCIÓN: Habilitar_Controles
     * DESCRIPCIÓN: Habilita la configuracion de acuerdo a la operacion     
     * RETORNA: 
     * CREO: Gustavo Angeles Cruz
     * FECHA_CREO: 30/Agosto/2010 
     * MODIFICO:
     * FECHA_MODIFICO
     * CAUSA_MODIFICACIÓN   
     *******************************************************************************/
    private void Habilitar_Controles(String Modo)
    {
        try
        {
            Boolean Habilitado = false;
            switch (Modo)
            {
                case "Inicial":
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Salir.Visible = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Eliminar.ToolTip = "Eliminar";
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    //Txt_Busqueda_Registro_Peticion.Text = String.Empty;
                    //Txt_Busqueda_Registro_Peticion.Enabled = true;
                    Btn_Buscar.Enabled = true;
                    break;
                //Estado de Nuevo
                case "Nuevo":
                    Txt_Nombre.Focus();
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    //Txt_Busqueda_Registro_Peticion.Text = String.Empty;
                    //Txt_Busqueda_Registro_Peticion.Enabled = false;
                    Btn_Buscar.Enabled = false;
                    Habilitado = true;
                    break;
                //Estado de Modificar
                case "Modificar":
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    //Txt_Busqueda_Registro_Peticion.Text = String.Empty;
                    //Txt_Busqueda_Registro_Peticion.Enabled = false;
                    Btn_Buscar.Enabled = false;
                    Habilitado = true;
                    break;
                default: break;

            }
            Txt_Nombre.Enabled = Habilitado;
            Txt_Comentarios.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            Mostrar_Informacion(ex.ToString(), true);
        }
    }
    /*******************************************************************************
    * NOMBRE DE LA FUNCIÓN: Mostrar_Información
    * DESCRIPCIÓN: Llena las areas de texto con el registro seleccionado del grid
    * RETORNA: 
    * CREO: Gustavo Angeles Cruz
    * FECHA_CREO: 24/Agosto/2010 
    * MODIFICO:
    * FECHA_MODIFICO:
    * CAUSA_MODIFICACIÓN:
    ********************************************************************************/
    private void Mostrar_Informacion(String txt, Boolean mostrar)
    {
        Lbl_Informacion.Visible = mostrar;
        Img_Warning.Visible = mostrar;
        Lbl_Informacion.Text = txt;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Mensaje_Error
    ///DESCRIPCIÓN: Metodo que llena el grid view con el metodo de Consulta_tramites
    ///PARAMETROS:   
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  05/Noviembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Mostrar_Mensaje_Error(Boolean Estatus, String Mensaje)
    {
        try
        {
            Lbl_Informacion.Visible = Estatus;
            Img_Warning.Visible = Estatus;
            Lbl_Informacion.Text = Mensaje;
        }
        catch (Exception Ex)
        {
           
            throw new Exception("Error al mostrar mensaje de error. Error: [" + Ex.Message + "]");
        }
    }
    /*******************************************************************************
    * NOMBRE DE LA FUNCIÓN: Asignar_Valor_Attributos
    * DESCRIPCIÓN: Asigna valor a los atributos de la clase para pasarlos como parametro 
    * en los metodos que lo requieran en la capa de Negocio
    * RETORNA: 
    * CREO: Gustavo Angeles Cruz
    * FECHA_CREO: 24/Agosto/2010 
    * MODIFICO:
    * FECHA_MODIFICO:
    * CAUSA_MODIFICACIÓN:
    ********************************************************************************/
    private Cls_Cat_Documentos_Negocio Asignar_Valor_Attributos()
    {
        Cls_Cat_Documentos_Negocio Negocio = new Cls_Cat_Documentos_Negocio();
        try
        {
            Negocio.P_ID = Hdf_Id_Documento.Value;
            Negocio.P_Nombre = HttpUtility.HtmlDecode(Txt_Nombre.Text);
            Negocio.P_Comentarios = HttpUtility.HtmlDecode(Txt_Comentarios.Text);
            Negocio.P_Buscar = Txt_Busqueda.Text;
            Negocio.P_Usuario_Creo_Modifico = Cls_Sessiones.Nombre_Empleado.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        return Negocio;
    }
    /*******************************************************************************
    * NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    * DESCRIPCIÓN: Limpia las areas de texto y deja los combos en su valor inical
    * RETORNA: 
    * CREO: Gustavo Angeles Cruz
    * FECHA_CREO: 24/Agosto/2010 
    * MODIFICO:
    * FECHA_MODIFICO:
    * CAUSA_MODIFICACIÓN:
    ********************************************************************************/
    private void Limpiar_Formulario()
    {
        try
        {
            Hdf_Id_Documento.Value = "";
            Txt_Nombre.Text = "";
            Txt_Comentarios.Text = "";
            Txt_Busqueda.Text = "";
            Btn_Modificar.Enabled = false;
            Btn_Eliminar.Enabled = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    /*******************************************************************************
    * NOMBRE DE LA FUNCIÓN: Validaciones
     * DESCRIPCIÓN: Genera el String con la informacion que falta y ejecuta la 
     * operacion solicitada si las validaciones son positivas
     * RETORNA: 
     * CREO: Gustavo Angeles Cruz
     * FECHA_CREO: 24/Agosto/2010 
     * MODIFICO:
     * FECHA_MODIFICO:
     * CAUSA_MODIFICACIÓN:
    ********************************************************************************/
    private bool Validaciones()
    {
        String Info = "";
        Mostrar_Informacion("", false);
        String Tabulador = "";
        try
        {
            Tabulador = "&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;";

            if (Txt_Nombre.Text.Length < 1 || Txt_Nombre.Text.Length > 100)
                Info = Info + "<br/>" + Tabulador + "* Revisar la longitud del Nombre del Documento";

            if (Txt_Comentarios.Text.Length < 1 || Txt_Comentarios.Text.Length > 4000)
                Info = Info + "<br/>" + Tabulador + "* Revisar la longitud de la Descripción";

            if (Info.Length > 0)
            {
                Mostrar_Informacion("Es necesario introducir: " + Info, true);
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:
    ///PARAMETROS:   
    ///CREO:       
    ///FECHA_CREO: 
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }
}
