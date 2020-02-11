using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Perfiles.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Programas_AC.Negocio;
using Presidencia.Catalogo_Atencion_Ciudadana_Organigrama.Negocio;
using Presidencia.Catalogo_Atencion_Ciudadana_Programas_Empleado.Negocio;

public partial class paginas_Atencion_Ciudadana_Frm_Cat_Ate_Directores_Unidades : System.Web.UI.Page
{
    #region Page Load
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: 
    ///PARAMETROS: 
    ///CREO:        Roberto González Oseguera
    ///FECHA_CREO:  30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializar_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Consulta_Empleados();
            }
            string Ventana_Modal = "Abrir_Ventana_Modal('../Tramites/Ventanas_Emergente/Frm_Busqueda_Avanzada_Empleado.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Empleado.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Dependencia.Attributes.Add("onclick", Ventana_Modal);

        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region Metodos Generales
    ///*******************************************************************************
    ///NOMBRE:          Inicializa_Controles
    ///DESCRIPCIÓN:     Inicializa los controles
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Inicializar_Controles()
    {
        try
        {
            Limpia_Controles();
            Habilitar_Controles("Inicial");
            Llenar_Combo_Unidad_Responsable();
            Consulta_Empleados();
            Consulta_Organigrama();
        }
        catch (Exception ex)
        {
            throw new Exception("Inicializa_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE:          Limpia_Controles
    ///DESCRIPCIÓN:     Limpa los controles
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Busqueda.Text = "";
            Txt_Filtro_Nombre_Empleado.Text = "";
            Txt_Filtro_Numero_Empleado.Text = "";
            Txt_Nombre_Empleado.Text = "";

            Cmb_Filtro_Unidad_Responsable.SelectedIndex = -1;
            Cmb_Empleado.SelectedIndex = -1;
            Cmb_Unidad_Responsable.SelectedIndex = -1;

            Grid_Organigrama.DataSource = new DataTable();
            Grid_Organigrama.DataBind();
            Grid_Organigrama.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Consulta_Organigrama
    ///DESCRIPCIÓN:     Realizar una búsqueda de programas empleados y cargar el resultado en el grid
    ///                 si el campo de búsqueda contiene texto, se agrega como filtro
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Consulta_Organigrama()
    {
        DataTable Dt_Datos_Empleado = new DataTable();
        var Neg_Consulta_Organigrama = new Cls_Cat_Ate_Organigrama_Negocios();
        try
        {
            if (Txt_Busqueda.Text.Trim().Length > 0)
            {
                Neg_Consulta_Organigrama.P_Nombre_Empleado = Txt_Busqueda.Text.Trim();
            }
            // siempre consultar los del módulo ATENCION CIUDADANA
            Neg_Consulta_Organigrama.P_Modulo = "ATENCION CIUDADANA";

            Dt_Datos_Empleado = Neg_Consulta_Organigrama.Consultar_Empleado_Unidad();
            Llenar_Grid_Organigrama(Dt_Datos_Empleado);
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Organigrama " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Consulta_Empleados
    ///DESCRIPCIÓN:     Realizar una búsqueda de empleados llama al método Llenar_Combo_Empleados
    ///                 para mostrar en el grid el resultado de la búsqueda
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Consulta_Empleados()
    {
        var Negocio_Consultar_Empleado = new Cls_Cat_Ate_Programas_Empleado_Negocio();
        try
        {
            DataTable Dt_Empleados = Negocio_Consultar_Empleado.Consultar_Empleado();
            Llenar_Combo_Empleados(Dt_Empleados);
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Empleados " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Llenar_Combo_Empleados
    ///DESCRIPCIÓN:     Llenara el combo de los perfiles
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Combo_Empleados(DataTable Dt_Consulta)
    {
        try
        {
            Cmb_Empleado.DataSource = Dt_Consulta;
            Cmb_Empleado.DataValueField = Cat_Empleados.Campo_Empleado_ID;
            Cmb_Empleado.DataTextField = "Nombre_Empleado";
            Cmb_Empleado.DataBind();
            Cmb_Empleado.Items.Insert(0, "< SELECCIONE >");

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Combo_Empleados " + ex.Message.ToString());
        }
    }


    ///*******************************************************************************
    ///NOMBRE:          Llenar_Combo_Unidad_Responsable
    ///DESCRIPCIÓN:     Llenara el combo de los perfiles
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Combo_Unidad_Responsable()
    {
        Cls_Cat_Dependencias_Negocio Negocio_Responsable = new Cls_Cat_Dependencias_Negocio();
        DataTable Dt_Unidad_Responsable = new DataTable();
        try
        {
            //  1 para la unidad resposable
            Dt_Unidad_Responsable = Negocio_Responsable.Consulta_Dependencias();
            //   2 SE ORDENA LA TABLA POR 
            DataView Dv_Ordenar = new DataView(Dt_Unidad_Responsable);
            Dv_Ordenar.Sort = Cat_Dependencias.Campo_Nombre;

            Dt_Unidad_Responsable = Dv_Ordenar.ToTable();
            Cmb_Filtro_Unidad_Responsable.DataSource = Dt_Unidad_Responsable;
            Cmb_Filtro_Unidad_Responsable.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Filtro_Unidad_Responsable.DataTextField = Cat_Dependencias.Campo_Nombre;
            Cmb_Filtro_Unidad_Responsable.DataBind();
            Cmb_Filtro_Unidad_Responsable.Items.Insert(0, "< SELECCIONE >");
            // llenar combo Unidad responsable
            Cmb_Unidad_Responsable.DataSource = Dt_Unidad_Responsable;
            Cmb_Unidad_Responsable.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Unidad_Responsable.DataTextField = Cat_Dependencias.Campo_Nombre;
            Cmb_Unidad_Responsable.DataBind();
            Cmb_Unidad_Responsable.Items.Insert(0, "< SELECCIONE >");

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Combo_Unidad_Responsable " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Alta_Empleado_Puesto
    ///DESCRIPCIÓN:     llamar al método de la capa de negocio para dar de alta el registro
    ///PARAMETROS:      
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private int Alta_Empleado_Puesto()
    {
        var Neg_Alta = new Cls_Cat_Ate_Organigrama_Negocios();
        try
        {
            // validar que haya un elemento seleccionado en Cmb_Empleado antes de asignar el valor
            if (Cmb_Empleado.SelectedIndex > 0)
            {
                Neg_Alta.P_Empleado_ID = Cmb_Empleado.SelectedValue;
            }
            Neg_Alta.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;
            Neg_Alta.P_Nombre_Empleado = Txt_Nombre_Empleado.Text.Trim();
            // siempre utilizar módulo ATENCION CIUDADANA
            Neg_Alta.P_Modulo = "ATENCION CIUDADANA";
            Neg_Alta.P_Tipo = "DIRECTOR";
            Neg_Alta.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            return Neg_Alta.Alta_Empleado_Unidad();
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Empleado_Puesto " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Modificar_Empleado_Unidad
    ///DESCRIPCIÓN:     Utilizando la capa de negocio actualiza el elemento seleccionado
    ///PARAMETROS:      
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private int Modificar_Empleado_Unidad()
    {
        var Negocio_Modificar = new Cls_Cat_Ate_Organigrama_Negocios();
        try
        {
            Negocio_Modificar.P_Parametro_ID = Grid_Organigrama.SelectedRow.Cells[1].Text;
            // si hay un empleado seleccionado, asignar ID
            if (Cmb_Empleado.SelectedIndex > 0)
            {
                Negocio_Modificar.P_Empleado_ID = Cmb_Empleado.SelectedValue;
            }
            else
            {
                Negocio_Modificar.P_Empleado_ID = "";
            }
            Negocio_Modificar.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;
            Negocio_Modificar.P_Nombre_Empleado = Txt_Nombre_Empleado.Text.Trim();
            Negocio_Modificar.P_Modulo = "ATENCION CIUDADANA";
            Negocio_Modificar.P_Tipo = "DIRECTOR";
            Negocio_Modificar.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            return Negocio_Modificar.Modificar_Empleado_Unidad();
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Empleado_Unidad " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Eliminar_Programa_Empleado
    ///DESCRIPCIÓN:     mediante la clase de negocio elimina el registro seleccionado
    ///PARAMETROS:      
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private int Eliminar_Programa_Empleado()
    {
        var Neg_Eliminar = new Cls_Cat_Ate_Organigrama_Negocios();
        try
        {
            Neg_Eliminar.P_Parametro_ID = Grid_Organigrama.SelectedRow.Cells[1].Text;
            Neg_Eliminar.P_Modulo = "ATENCION CIUDADANA";
            return Neg_Eliminar.Eliminar_Empleado_Unidad();
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Programa_Empleado " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Validar_Datos
    ///DESCRIPCIÓN:     Validar que se haya seleccionado un programa y un empleado
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private string Validar_Datos()
    {
        string Mensaje_Error = "";

        try
        {
            if (Cmb_Empleado.SelectedIndex <= 0 && Txt_Nombre_Empleado.Text.Trim().Length <= 0)
            {
                Mensaje_Error += " - Seleccionar o ingresar el nombre de un Empleado.<br />";
            }
            if (Cmb_Unidad_Responsable.SelectedIndex <= 0)
            {
                Mensaje_Error += " - Seleccionar una Unidad Responsable<br />";
            }
            if (Cmb_Empleado.SelectedIndex > 0 && Txt_Nombre_Empleado.Text.Trim().Length > 0)
            {
                Mensaje_Error += " - Borrar el nombre o quitar la selección de empleado, no se permiten ambos.<br />";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Validar_Datos " + ex.Message.ToString());
        }

        return Mensaje_Error;
    }

    //////*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Mensaje
    ///DESCRIPCIÓN: Muestra el texto que recibe como parámetro o si es nulo o mensaje vacío, se ocultan los controles de mensaje
    ///PARÁMETROS:
    /// 		1. Mensaje: texto a mostrar como mensaje en la página
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 30-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Mensaje(string Mensaje)
    {
        if (string.IsNullOrEmpty(Mensaje))
        {
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        else
        {
            Lbl_Mensaje_Error.Text = Mensaje;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE:          Habilitar_Controles
    ///DESCRIPCIÓN:     Habilitara los controles para poder ealizar las operaciones
    ///PARAMETROS:      String Operaciones:el tipo de operacion
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
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
            Txt_Busqueda.Enabled = !Habilitado;
            Txt_Filtro_Nombre_Empleado.Enabled = Habilitado;
            Txt_Filtro_Numero_Empleado.Enabled = Habilitado;
            Cmb_Filtro_Unidad_Responsable.Enabled = Habilitado;
            Cmb_Empleado.Enabled = Habilitado;
            Txt_Nombre_Empleado.Enabled = Habilitado;
            Cmb_Unidad_Responsable.Enabled = Habilitado;
            Btn_Buscar_Empleado.Enabled = Habilitado;
            Btn_Buscar_Dependencia.Enabled = Habilitado;
            Btn_Buscar.Enabled = !Habilitado;
            Grid_Organigrama.Enabled = !Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    #endregion Metodos Generales

    #region Botones
    ///*******************************************************************************
    ///NOMBRE:          Btn_Nuevo_Click
    ///DESCRIPCIÓN:     Realizara los metodos requeridos para dar de alta el perfil
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje("");

            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Inicializar_Controles();          //Limpia los controles de la forma para poder introducir nuevos datos
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
            }
            else
            {
                string Mensaje_Validacion = Validar_Datos();
                if (Mensaje_Validacion.Length <= 0)
                {
                    // validar que no haya un registro con los datos seleccionados
                    DataTable Dt_Datos_Empleado = new DataTable();
                    var Neg_Consulta_Organigrama = new Cls_Cat_Ate_Organigrama_Negocios();
                    Neg_Consulta_Organigrama.P_Modulo = "ATENCION CIUDADANA";
                    Neg_Consulta_Organigrama.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;
                    Dt_Datos_Empleado = Neg_Consulta_Organigrama.Consultar_Empleado_Unidad();
                    // si la consulta regresó valores, mostrar mensaje indicando que ya hay un registro y abandonar método
                    if (Dt_Datos_Empleado != null && Dt_Datos_Empleado.Rows.Count > 0)
                    {
                        Mostrar_Mensaje("Ya hay un registro con los datos seleccionados (no es posible dar de alta dos Directores para la misma Unidad Responsable).");
                        return;
                    }

                    // llamar método alta de programa
                    int Resultado_Insercion = Alta_Empleado_Puesto();
                    if (Resultado_Insercion > 0)
                    {
                        Inicializar_Controles();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Programas Empleados", "alert('Alta de registro Exitosa');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Programas Empleados", "alert('No fue posible dar de alta el registro');", true);
                    }
                }
                else
                {
                    Mostrar_Mensaje("Se requiere:<br />" + Mensaje_Validacion);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Nuevo_Click " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Btn_Modificar_Click
    ///DESCRIPCIÓN:     Llamar al método en la clase de negocio para modificar el registro seleccionado
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Mensaje("");

        try
        {
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Cmb_Empleado.SelectedIndex > 0 || Txt_Nombre_Empleado.Text.Length > 0)
                {
                    Habilitar_Controles("Modificar");
                }
                else
                {
                    Mostrar_Mensaje("Seleccione un elemento.");
                }
            }
            else
            {
                string Mensaje_Validacion = Validar_Datos();
                if (Mensaje_Validacion.Length <= 0)
                {
                    if (Modificar_Empleado_Unidad() > 0)
                    {
                        Inicializar_Controles();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Modificar_Click", "alert('Modificacion Exitosa');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Modificar_Click", "alert('Ocurrió un error y no fue posible actualizar el registro.');", true);
                    }
                }
                else
                {
                    Mostrar_Mensaje("Se requiere:<br />" + Mensaje_Validacion);
                }
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Modificar_Click " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Btn_Eliminar_Click
    ///DESCRIPCIÓN:     Eliminar el registro seleccionado de la base de datos
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Mensaje("");

        try
        {
            if (Grid_Organigrama.SelectedIndex >= 0)
            {
                if (Eliminar_Programa_Empleado() > 0)
                {
                    Inicializar_Controles();
                    Grid_Organigrama.SelectedIndex = -1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Programas empleado", "alert('El registro fue eliminado de forma exitosa');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Programas empleado", "alert('Ocurrió un error y el registro no pudo ser eliminado');", true);
                }
            }
            else
            {
                Mostrar_Mensaje("Seleccione un elemento.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Eliminar_Click " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Btn_Salir_Click
    ///DESCRIPCIÓN:     Realizara los metodos requeridos para salir
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Mensaje("");

        if (Btn_Salir.ToolTip == "Salir")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Inicializar_Controles();
        }

    }

    ///*******************************************************************************
    ///NOMBRE:          Btn_Buscar_Click
    ///DESCRIPCIÓN:     llama al método de consulta que carga los registros en el grid
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Mensaje("");

        try
        {
            Consulta_Organigrama();
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje("Btn_Buscar_Click " + ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Llenar_Grid_Organigrama
    ///DESCRIPCIÓN:     Realizara los metodos requeridos buscar un perfil
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_Grid_Organigrama(DataTable Dt_Organigrama)
    {
        Mostrar_Mensaje("");

        try
        {
            Grid_Organigrama.Columns[1].Visible = true;
            Grid_Organigrama.Columns[2].Visible = true;
            Grid_Organigrama.Columns[3].Visible = true;
            Grid_Organigrama.Columns[4].Visible = true;
            Grid_Organigrama.DataSource = Dt_Organigrama;
            Grid_Organigrama.DataBind();
            Grid_Organigrama.Columns[1].Visible = false;
            Grid_Organigrama.Columns[2].Visible = false;
            Grid_Organigrama.Columns[3].Visible = false;
            Grid_Organigrama.Columns[4].Visible = false;

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid_Organigrama " + ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Btn_Buscar_Empleado_Click
    ///DESCRIPCIÓN:     Realizara los metodos requeridos buscar al empleado
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Click(object sender, EventArgs e)
    {
        DataTable Dt_Empleado = new DataTable();
        String Empleado_ID = "";

        Mostrar_Mensaje("");

        try
        {
            if (Session["BUSQUEDA_EMPLEADO"] != null)
            {
                Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_EMPLEADO"].ToString());

                if (Estado != false)
                {
                    Empleado_ID = Session["EMPLEADO_ID"].ToString();
                    Dt_Empleado = (DataTable)(Session["Dt_Empleados"]);

                    Llenar_Combo_Empleados(Dt_Empleado);
                    Cmb_Empleado.SelectedIndex = Cmb_Empleado.Items.IndexOf(Cmb_Empleado.Items.FindByValue(Empleado_ID));

                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje("Btn_Buscar_Empleado_Click " + ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Dependencia_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Dependencia_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Mensaje("");

        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_DEPENDENCIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_DEPENDENCIAS"]) == true)
            {
                try
                {
                    string Dependencia_ID = Session["DEPENDENCIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Unidad_Responsable.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Unidad_Responsable.SelectedValue = Dependencia_ID;
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Mensaje(Ex.Message);
                }

                // limpiar variables de sesión
                Session.Remove("DEPENDENCIA_ID");
                Session.Remove("NOMBRE_DEPENDENCIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_DEPENDENCIAS");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Busqueda_Empleado_Click
    ///DESCRIPCIÓN: oculta el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Roberto González Oseguera
    ///FECHA_CREO:  30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cerrar_Busqueda_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Mensaje("");

        try
        {
            Div_Filtro_Empleados.Style.Value = "display:none";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Btn_Buscar_Empleado_Filtro_Click
    ///DESCRIPCIÓN:    Búsqueda de empleados 
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Filtro_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Empleados_Negocios Negocio_Empleado = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Datos_Empleado = new DataTable();
        var Neg_Consulta_Programa_Empleado = new Cls_Cat_Ate_Programas_Empleado_Negocio();

        Mostrar_Mensaje("");

        try
        {
            //  para el nombre
            if (!String.IsNullOrEmpty(Txt_Filtro_Nombre_Empleado.Text))
            {
                Neg_Consulta_Programa_Empleado.P_Nombre_Empleado = Txt_Filtro_Nombre_Empleado.Text.ToUpper().Trim();
            }
            //  para el numero de empleado
            if (!String.IsNullOrEmpty(Txt_Filtro_Numero_Empleado.Text))
            {
                //String.Format("{0:00000}", Convert.ToInt32(Reloj_Checador_ID) + 1);
                Neg_Consulta_Programa_Empleado.P_Numero_Empleado = String.Format("{0:000000}", Convert.ToInt32(Txt_Filtro_Numero_Empleado.Text.ToUpper().Trim()));
            }

            //  para la unidad responsable
            if (Cmb_Filtro_Unidad_Responsable.SelectedIndex > 0)
            {
                Neg_Consulta_Programa_Empleado.P_Unidad_Responsable_ID = Cmb_Filtro_Unidad_Responsable.SelectedValue;
            }
            //Consulta_Empleados_Dependencia
            Dt_Datos_Empleado = Neg_Consulta_Programa_Empleado.Consultar_Empleado();

            Llenar_Combo_Empleados(Dt_Datos_Empleado);
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Buscar_Empleado_Click " + ex.Message.ToString(), ex);
        }
    }

    #endregion Botones

    #region Grid
    ///*******************************************************************************
    ///NOMBRE:          Grid_Organigrama_OnSelectedIndexChanged
    ///DESCRIPCIÓN:     carga los datos del elemento seleccionado en el grid para mostrarlos en la página
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Organigrama_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        String Empleado_ID = "";
        String Unidad_Responsable_Id = "";
        String Nombre_Empleado = "";

        Mostrar_Mensaje("");

        // limpiar valores
        Cmb_Empleado.SelectedIndex = -1;
        Cmb_Unidad_Responsable.SelectedIndex = -1;
        Txt_Nombre_Empleado.Text = "";

        try
        {
            if (Grid_Organigrama.SelectedIndex > (-1))
            {
                Empleado_ID = HttpUtility.HtmlDecode(Grid_Organigrama.SelectedRow.Cells[2].Text).ToString();
                Unidad_Responsable_Id = HttpUtility.HtmlDecode(Grid_Organigrama.SelectedRow.Cells[3].Text).ToString();
                Nombre_Empleado = HttpUtility.HtmlDecode(Grid_Organigrama.SelectedRow.Cells[4].Text).ToString();

                // validar que no sea nulo o vacío
                if (!string.IsNullOrEmpty(Empleado_ID))
                {
                    // validar que el elemento a seleccionar exista en el combo
                    if (Cmb_Empleado.Items.FindByValue(Empleado_ID) != null)
                    {
                        Cmb_Empleado.SelectedValue = Empleado_ID;
                    }
                }
                // si el campo nombre empleado contiene valores, agregar el texto a Txt_Nombre_Empleado
                if (!string.IsNullOrEmpty(Nombre_Empleado))
                {
                    Txt_Nombre_Empleado.Text = Nombre_Empleado;
                }
                // validar que Unidad_Responsable_Id existe en el combo antes de seleccionar
                if (Cmb_Unidad_Responsable.Items.FindByValue(Unidad_Responsable_Id) != null)
                {
                    Cmb_Unidad_Responsable.SelectedValue = Unidad_Responsable_Id;
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje("Error al cargar datos: " + ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Txt_Nombre_Empleado_TextChanged
    ///DESCRIPCIÓN: manejo del evento cambio de texto en Txt_Nombre_Empleado
    ///         quitar selección del combo empleado si se ingresó texto
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-oct-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Nombre_Empleado_TextChanged(object sender, EventArgs e)
    {
        Mostrar_Mensaje("");

        try
        {
            // si hay texto en Txt_Nombre_Empleado y un elemento seleccionado en Cmb_Empleado
            if (Txt_Nombre_Empleado.Text.Trim().Length > 0 && Cmb_Empleado.SelectedIndex >= 0)
            {
                //  quitar selección del combo
                Cmb_Empleado.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje("Error al cargar datos: " + ex.Message);
        }
    }
    #endregion Grid

}
