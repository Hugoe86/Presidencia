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
using Presidencia.Localidades.Negocios;

public partial class paginas_Atencion_Ciudadana_Frm_Cat_Localidades : System.Web.UI.Page
{
    #region Variables Internas
    //objeto de la clase de negocio de localidades para acceder a la clase de datos y realizar copnexion
    private Cls_Cat_Ate_Localidades_Negocio Localidades_Negocio;
    #endregion

    #region Page_Load / Init
    protected void Page_Load(object sender, EventArgs e)
    {
        Localidades_Negocio = new Cls_Cat_Ate_Localidades_Negocio();
        Refrescar_Grid();
    }
    #endregion 

    #region Métodos


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Forma
    ///DESCRIPCIÓN: es un metodo generico para habilitar todos los campos de la 
    ///forma que pueden ser editados
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 20/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Forma()
    {
        Txt_ID.Text = "";
        Txt_Nombre.Text = "";
        Txt_Descripcion.Text = "";
        Txt_Descripcion.Enabled = true;
        Txt_Nombre.Enabled = true;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Deshabilitar_Forma
    ///DESCRIPCIÓN: es un metodo generico para deshabilitar todos los campos de la 
    ///forma que pueden ser editados
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 20/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Deshabilitar_Forma()
    {
        Txt_ID.Text = "";
        Txt_Nombre.Text = "";
        Txt_Descripcion.Text = "";
        Txt_Descripcion.Enabled = false;
        Txt_Nombre.Enabled = false;
        Localidades_Negocio.P_Localidad_ID = null;
        Localidades_Negocio.P_Nombre = null;
        Localidades_Negocio.P_Descripcion = null;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Refrescar_Grid
    ///DESCRIPCIÓN: refresca el gris con los registros de asuntos mas actuales 
    ///que existen en la base de datos
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 23/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Refrescar_Grid()
    {
        DataSet Data_Set = Localidades_Negocio.Consultar_Localidad();
        if (Data_Set != null)
        {
            Grid_Localidades.DataSource = Data_Set;
            Grid_Localidades.DataBind();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Informacion
    ///DESCRIPCIÓN: Habilita o deshabilita la muestra en pantalle del mensaje 
    ///de Mostrar_Informacion para el usuario
    ///PARAMETROS: 1.- Condicion, entero para saber si es 1 habilita para que se muestre mensaje si es cero
    ///deshabiñina para que no se muestre el mensaje
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 23/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Mostrar_Informacion(int Condicion)
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Manejar_Botones
    ///DESCRIPCIÓN: es un metodo generico para habilitar y deshabilitar todos 
    ///los botones de la forma de acuerdo a sus eventos
    ///PARAMETROS: 1.- Modo, indica la forma en que se ´pondran los botones en 
    ///tanto a vidibilidad y tooltip
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 20/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    private void Manejar_Botones(int Modo)
    {
        switch (Modo)
        {
            //Click en Nuevo
            case 1:
                Txt_ID.Text = "";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Nuevo.ToolTip = "Dar Alta";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.Visible = false;
                Bnt_Eliminar.Visible = false;
                break;


            //Click en Modificar
            case 2:
                Btn_Salir.ToolTip = "Cancelar";
                Bnt_Eliminar.Visible = false;
                Btn_Nuevo.Visible = false;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                break;

            //Estado Inicial
            case 3:
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = true;
                Bnt_Eliminar.Visible = true;
                Btn_Salir.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Modificar.ToolTip = "Modificar";
                Bnt_Eliminar.ToolTip = "Eliminar";
                Btn_Salir.ToolTip = "Salir";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Bnt_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                break;
            default: break;
        }
    }
    #endregion

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Localidades_SelectedIndexChanged
    ///DESCRIPCIÓN: Cuando se da clic en un reglon del grid llenan los campos de 
    ///la forma con los datos de la localidad seleccionado
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 23/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Localidades_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow Row = Grid_Localidades.SelectedRow;
        Txt_ID.Text = Row.Cells[1].Text;
        Txt_Nombre.Text = Row.Cells[2].Text;
        Txt_Descripcion.Text = Row.Cells[3].Text;
        Mostrar_Informacion(0);
        Txt_Nombre.Enabled = false;
        Txt_Descripcion.Enabled = false;
        if (Btn_Nuevo.ToolTip == "Dar Alta")
        {
            Btn_Nuevo.ToolTip = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Localidades_PageIndexChangning
    ///DESCRIPCIÓN: Da un correcto funcionamiento al paginado del grid
    ///refrescando cuando se cambia de pagina
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 23/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Localidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Localidades.PageIndex = e.NewPageIndex;
        Grid_Localidades.DataBind();
    }
    #endregion

    #region Eventos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Habilita las cajas de texto necesarias para crear una nueva localidad
    ///y coloca un id de forma automatica en la caja de texto de id, se convierte en dar alta
    ///cuando oprimimos Nuevo y dar alta  Crea un registro de un asunto en la base de datos 
    ///PARAMTROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 23/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        
            if (Btn_Nuevo.ToolTip == "Dar Alta")
            {
                if (Txt_Nombre.Text.Length > 1 && Txt_Descripcion.Text.Length > 1)
                {
                    if (Txt_Descripcion.Text.Length > 120)
                    {
                        String Cadena_Informacion = "El campo descripción exede el número de caracteres permitidos, sea tan amable de verificar.";
                        Lbl_Informacion.Text = Cadena_Informacion;
                        Mostrar_Informacion(1);
                    }
                    else
                    {
                        //Txt_ID.Text = Cls_Util.consecutivo(Localidades.Campo_LocalidadID, Localidades.Tabla_Cat_Ate_Localidades);
                        Localidades_Negocio.P_Localidad_ID = Txt_ID.Text;
                        Localidades_Negocio.P_Nombre = Txt_Nombre.Text;
                        Localidades_Negocio.P_Descripcion = Txt_Descripcion.Text;
                        Localidades_Negocio.Insertar_Localidad(Cls_Sessiones.Nombre_Empleado);
                        Mostrar_Informacion(0);
                        Manejar_Botones(3);
                        Deshabilitar_Forma();
                        Refrescar_Grid();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Localidades", "alert('La localidad ha sido registrada');", true);
                    }
                }//fin if lenght
                else
                {
                    String Cadena_Informacion = "Los siguientes datos son requeridos por el sistema para realizar la operación, sea tan amable de verificar.";
                    Cadena_Informacion = Cadena_Informacion + "<br/>&nbsp;&nbsp;&nbsp;+ Nombre de la licalidad <br/>&nbsp;&nbsp;&nbsp;+ Descripción de la localidad";
                    Lbl_Informacion.Text = Cadena_Informacion;
                    Mostrar_Informacion(1);

                }//fin else lenght
            }//fin if Nuevo
            else
            {
                if (Btn_Modificar.ToolTip == "Actualizar")
                {
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Mostrar_Informacion(0);
                }//fin if Modificar

                Lbl_Informacion.Text = "";
                Lbl_Informacion.Enabled = false;
                Img_Warning.Visible = false;
                Habilitar_Forma();
                Txt_ID.Text = "";
                Manejar_Botones(1);
            }//fin else Nuevo
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Habilita las cajas de texto necesarias para Modificar una localidad existente
    ///se convierte en actualizar cuando oprimimos Modificar y actualiza un registro de un
    ///asunto en la bd
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 23/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Modificar.ToolTip == "Actualizar")
        {
            if (Txt_Nombre.Text.Length > 1 && Txt_Descripcion.Text.Length > 1)
            {
                if (Txt_Descripcion.Text.Length > 120)
                {
                    String Cadena_Informacion = "El campo descripción exede el número de caracteres permitidos, sea tan amable de verificar.";
                    Lbl_Informacion.Text = Cadena_Informacion;
                    Mostrar_Informacion(1);
                }
                else
                {
                    Localidades_Negocio.P_Localidad_ID = Txt_ID.Text;
                    Localidades_Negocio.P_Nombre = Txt_Nombre.Text;
                    Localidades_Negocio.P_Descripcion = Txt_Descripcion.Text;
                    Localidades_Negocio.Modificar_Localidad(Cls_Sessiones.Nombre_Empleado);
                    Mostrar_Informacion(0);
                    Deshabilitar_Forma();
                    Refrescar_Grid();
                    Manejar_Botones(3);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Localidades", "alert('La localidad ha sido actualizada');", true);

                }
            }
            else
            {
                String Cadena_Informacion = "Los siguientes datos son requeridos por el sistema para realizar la operación, sea tan amable de verificar.";
                Cadena_Informacion = Cadena_Informacion + "<br/>+ Nombre de la localidad <br/>+ Descripcion de la localidad";
                Lbl_Informacion.Text = Cadena_Informacion;
                Mostrar_Informacion(1);
            }
        }
        else
        {
            if (Txt_ID.Text.Length < 1)
            {
                String Cadena_Informacion = "Para realizar la operacion es necesario escoger una localidad de la tabla, sea tan amable de verificar.";
                Lbl_Informacion.Text = Cadena_Informacion;
                Mostrar_Informacion(1);
                Manejar_Botones(3);
            }
            else
            {

                Txt_Nombre.Enabled = true;
                Txt_Descripcion.Enabled = true;
                Manejar_Botones(2);
            }
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Bnt_Eliminar_Click
    ///DESCRIPCIÓN: Elimina un registro de una localidad de la base de datos
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 23/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Bnt_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_ID.Text.Length < 1)
        {
            String Cadena_Informacion = "Para realizar la operación es necesario escoger una localidad de la tabla, sea tan amable de verificar.";
            Lbl_Informacion.Text = Cadena_Informacion;
            Mostrar_Informacion(1);
        }
        else
        {
            Localidades_Negocio.P_Localidad_ID = Txt_ID.Text;
            Localidades_Negocio.Eliminar_Localidad();
            Deshabilitar_Forma();
            Refrescar_Grid();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Localidades", "alert('La localidad ha sido eliminada');", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando o sale del formulario
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 23/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Salir")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else{
            Manejar_Botones(3);
            Mostrar_Informacion(0);
        }

    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Busca una localidad por medio del nombre en la base de datos 
    ///y pone el resultado de las coincidencias de la busqueda en el grid
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 23/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Localidades_Negocio.P_Nombre = Txt_Busqueda.Text;
        DataSet Data_Set = Localidades_Negocio.Consultar_Localidad();
        if (Data_Set != null)
        {
            Grid_Localidades.DataSource = Data_Set;
            Grid_Localidades.DataBind();
        }

    }
    
    #endregion 

   }
