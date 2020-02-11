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
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Sessiones;
using Presidencia.Operacion_Predial_Parametros.Negocio;

public partial class paginas_predial_Frm_Cat_Cuotas_Minimas : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************       
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
            Llenar_Tabla_Cuotas_Minimas(0);
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. estatus.    Estatus en el que se cargara la configuración de los
    ///                             controles.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean estatus)
    {
        Btn_Nuevo.Visible = true;
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Btn_Eliminar.Visible = estatus;
        Txt_Anio.Enabled = !estatus;
        Txt_Cuota.Enabled = !estatus;
        Grid_Cuotas_Minimas.Enabled = estatus;
        Grid_Cuotas_Minimas.SelectedIndex = (-1);
        Btn_Buscar_Cuotas_Minimas.Enabled = estatus;
        Txt_Busqueda_Cuotas_Minimas.Enabled = estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_ID_Cuota_Minima.Text = "";
        Hdf_Cuota_Minima_ID.Value = "";
        Txt_Anio.Text = "";
        Txt_Cuota.Text = "";
        Txt_Bimestre.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Cuotas_Minimas
    ///DESCRIPCIÓN: Llena la tabla de Registros de Coutas Minimas con una consulta que puede o no
    ///             tener Filtros.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Cuotas_Minimas(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuota_Minima = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
            Cuota_Minima.P_Anio = Txt_Busqueda_Cuotas_Minimas.Text.Trim();
            Grid_Cuotas_Minimas.DataSource = Cuota_Minima.Consultar_Cuotas_Minimas();
            Grid_Cuotas_Minimas.PageIndex = Pagina;
            Grid_Cuotas_Minimas.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Anio.Text.Trim().Length == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir el Año.";
            Validacion = false;
        }
        if (Txt_Cuota.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Cuota.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Cuotas_Minimas_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de la Tabla de Cuotas Minimas 
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cuotas_Minimas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Cuotas_Minimas.SelectedIndex = (-1);
            Llenar_Tabla_Cuotas_Minimas(e.NewPageIndex);
            Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Cuotas_Minimas_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de una Cuota Minima Seleccionada para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cuotas_Minimas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Cuotas_Minimas.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Cuotas_Minimas.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuota_Minima = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
                Cuota_Minima.P_Cuota_Minima_ID = ID_Seleccionado;
                Cuota_Minima = Cuota_Minima.Consultar_Datos_Cuota_Minima();
                Hdf_Cuota_Minima_ID.Value = ID_Seleccionado;
                Txt_ID_Cuota_Minima.Text = ID_Seleccionado;
                Txt_Anio.Text = Cuota_Minima.P_Anio;
                Txt_Cuota.Text = Cuota_Minima.P_Cuota.ToString("#,###,###,###,###,###.00");
                Txt_Bimestre.Text = (Cuota_Minima.P_Cuota / 6).ToString("#,###,###,###,###,###.00");
                System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nuevo registro de 
    ///             Cuota Minima.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Limpiar_Catalogo();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuota_Minima = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
                    Cuota_Minima.P_Anio = Txt_Anio.Text.Trim();
                    Cuota_Minima.P_Cuota = Convert.ToDouble(Txt_Cuota.Text.Trim());
                    Cuota_Minima.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    if (!Cuota_Minima.Validar_Existe())
                    {
                        Cuota_Minima.Alta_Cuota_Minima();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Cuotas_Minimas(Grid_Cuotas_Minimas.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Cuotas Mínimas", "alert('Alta de Cuota Mínima Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Cuotas Mínimas", "alert('La Cuota Mínima que intenta dar de Alta ya Existe');", true);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Registro de 
    ///             Cuota Minima.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Cuotas_Minimas.Rows.Count > 0 && Grid_Cuotas_Minimas.SelectedIndex > (-1))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuota_Minima = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
                    Cuota_Minima.P_Cuota_Minima_ID = Hdf_Cuota_Minima_ID.Value;
                    Cuota_Minima.P_Anio = Txt_Anio.Text.Trim();
                    Cuota_Minima.P_Cuota = Convert.ToDouble(Txt_Cuota.Text.Trim());
                    Cuota_Minima.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    if (!Cuota_Minima.Validar_Existe())
                    {
                        Cuota_Minima.Modificar_Cuota_Minima();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Cuotas_Minimas(Grid_Cuotas_Minimas.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Cuotas Mínimas", "alert('Actualización de Cuota Mínima Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Cuotas Mínimas", "alert('La Cuota Mínima que intenta Modificar ya Existe');", true);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Cuotas_Minimas_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Cuotas_Minimas_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Grid_Cuotas_Minimas.SelectedIndex = (-1);
            Grid_Cuotas_Minimas.SelectedIndex = (-1);
            Llenar_Tabla_Cuotas_Minimas(0);
            if (Grid_Cuotas_Minimas.Rows.Count == 0 && Txt_Busqueda_Cuotas_Minimas.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Año \"" + Txt_Busqueda_Cuotas_Minimas.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todas las Cuotas Minimas almacenadas)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda_Cuotas_Minimas.Text = "";
                Llenar_Tabla_Cuotas_Minimas(0);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Elimina un registro de Cuota Minima de la Base de Datos
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Cuotas_Minimas.Rows.Count > 0 && Grid_Cuotas_Minimas.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuota_Minima = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
                Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
                if (Convert.ToInt32(Txt_Anio.Text) > Parametros.Consultar_Anio_Corriente())
                {
                    Cuota_Minima.P_Cuota_Minima_ID = Grid_Cuotas_Minimas.SelectedRow.Cells[1].Text;
                    Cuota_Minima.Eliminar_Cuota_Minima();
                    Grid_Cuotas_Minimas.SelectedIndex = (-1);
                    Llenar_Tabla_Cuotas_Minimas(Grid_Cuotas_Minimas.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Cuotas Minimas", "alert('La Cuota Mínima fue eliminada exitosamente');", true);
                    Limpiar_Catalogo();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Cuotas Minimas", "alert('No se puede eliminar la cuota mínima seleccionada');", true);
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Limpiar_Catalogo();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
    }

    #endregion

    protected void Txt_Cuota_TextChanged(object sender, EventArgs e)
    {
        if (Txt_Cuota.Text.Trim() == "")
        {
            Txt_Cuota.Text = "0.00";
            Txt_Bimestre.Text = "0.00";
        }
        else
        {
            try
            {
                Txt_Cuota.Text = Convert.ToDouble(Txt_Cuota.Text).ToString("#,###,###,###,###,###,###,###,##0.00");
                Txt_Bimestre.Text = (Convert.ToDouble(Txt_Cuota.Text) / 6).ToString("#,###,###,###,###,###,###,###,##0.00");
            }
            catch
            {
                Txt_Cuota.Text = "0.00";
                Txt_Bimestre.Text = "0.00";
            }
        }
    }
}