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
using Presidencia.Catalogo_Atencion_Ciudadana_Programas_Empleado.Negocio;

public partial class paginas_Atencion_Ciudadana_Frm_Cat_Ate_Programas_Empleado : System.Web.UI.Page
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
            Llenar_Combo_Programas();
            Llenar_Combo_Unidad_Responsable();
            Consulta_Programas_Empleados();
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

            Cmb_Filtro_Unidad_Responsable.SelectedIndex = -1;
            Cmb_Empleado.SelectedIndex = -1;
            Cmb_Programa.SelectedIndex = 0;
            Cmb_Estatus.SelectedIndex = 0;

            Grid_Programa_Empleado.DataSource = new DataTable();
            Grid_Programa_Empleado.DataBind();

        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Consulta_Programas_Empleados
    ///DESCRIPCIÓN:     Realizar una búsqueda de programas empleados y cargar el resultado en el grid
    ///                 si el campo de búsqueda contiene texto, se agrega como filtro
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Consulta_Programas_Empleados()
    {
        DataTable Dt_Datos_Empleado = new DataTable();
        var Neg_Consulta_Programa_Empleado = new Cls_Cat_Ate_Programas_Empleado_Negocio();
        try
        {
            if (Txt_Busqueda.Text.Trim().Length > 0)
            {
                Neg_Consulta_Programa_Empleado.P_Nombre_Empleado = Txt_Busqueda.Text;
            }
            Dt_Datos_Empleado = Neg_Consulta_Programa_Empleado.Consultar_Programas_Empleados();
            Llenar_Grid_Programa_Empleado(Dt_Datos_Empleado);
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Programas_Empleados " + ex.Message.ToString(), ex);
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
        Cls_Cat_Ate_Programas_Empleado_Negocio Negocio_Consultar_Empleado = new Cls_Cat_Ate_Programas_Empleado_Negocio();
        try
        {
            DataTable Dt_Empleados = Negocio_Consultar_Empleado.Consultar_Empleado();
            Llenar_Combo_Empleados(Dt_Empleados);
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Programas_Empleados " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Llenar_Combo_Programas
    ///DESCRIPCIÓN:     Llenara el combo de los perfiles
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Combo_Programas()
    {
        var Neg_Consulta_Programas = new Cls_Cat_Ate_Programas_Negocio();
        try
        {
            Neg_Consulta_Programas.P_Estatus = "ACTIVO";
            Cmb_Programa.DataSource = Neg_Consulta_Programas.Consultar_Programas();
            Cmb_Programa.DataValueField = Cat_Ate_Programas.Campo_Programa_ID;
            Cmb_Programa.DataTextField = Cat_Ate_Programas.Campo_Nombre;
            Cmb_Programa.DataBind();
            Cmb_Programa.Items.Insert(0, "< SELECCIONE >");

            Cmb_Programa.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Combo_Programas " + ex.Message.ToString());
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

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Combo_Unidad_Responsable " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Alta_Programa_Empleado
    ///DESCRIPCIÓN:     llamar al método de la capa de negocio para dar de alta el registro
    ///PARAMETROS:      
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private int Alta_Programa_Empleado()
    {
        var Neg_Alta = new Cls_Cat_Ate_Programas_Empleado_Negocio();
        try
        {
            Neg_Alta.P_Empleado_ID = Cmb_Empleado.SelectedValue;
            Neg_Alta.P_Programa_ID = Convert.ToInt32(Cmb_Programa.SelectedValue);
            Neg_Alta.P_Estatus = Cmb_Estatus.SelectedValue;
            Neg_Alta.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            return Neg_Alta.Alta_Programa_Empleado();
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Programa_Empleado " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Modificar_Programa_Empleado
    ///DESCRIPCIÓN:     Utilizando la capa de negocio actualiza el elemento seleccionado
    ///PARAMETROS:      
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private int Modificar_Programa_Empleado()
    {
        Cls_Cat_Ate_Programas_Empleado_Negocio Negocio_Modificar = new Cls_Cat_Ate_Programas_Empleado_Negocio();
        try
        {
            Negocio_Modificar.P_Programa_Empleado_ID = Grid_Programa_Empleado.SelectedRow.Cells[3].Text;
            Negocio_Modificar.P_Empleado_ID = Cmb_Empleado.SelectedValue;
            Negocio_Modificar.P_Programa_ID = Convert.ToInt32(Cmb_Programa.SelectedValue);
            Negocio_Modificar.P_Estatus = Cmb_Estatus.SelectedValue;
            Negocio_Modificar.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            return Negocio_Modificar.Modificar_Programa_Empleado();
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Programa_Empleado " + ex.Message.ToString());
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
        Cls_Cat_Ate_Programas_Empleado_Negocio Negocio_Modificar = new Cls_Cat_Ate_Programas_Empleado_Negocio();
        try
        {
            Negocio_Modificar.P_Programa_Empleado_ID = Grid_Programa_Empleado.SelectedRow.Cells[3].Text;
            return Negocio_Modificar.Eliminar_Programa_Empleado();
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
            if (Cmb_Empleado.SelectedIndex <= 0)
            {
                Mensaje_Error += " - Seleccionar un Empleado<br />";
            }
            if (Cmb_Programa.SelectedIndex <= 0)
            {
                Mensaje_Error += " - Seleccionar un Programa<br />";
            }
            if (Cmb_Estatus.SelectedIndex <= 0)
            {
                Mensaje_Error += " - Seleccionar un Estatus<br />";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Validar_Datos " + ex.Message.ToString());
        }

        return Mensaje_Error;
    }

    ///*******************************************************************************
    ///NOMBRE:          Validar_Existe_Programa_Empleado
    ///DESCRIPCIÓN:     Validar existencia de registro con datos seleccionados
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private string Validar_Existe_Programa_Empleado()
    {
        DataTable Dt_Resultado = new DataTable();
        var Neg_Consulta = new Cls_Cat_Ate_Programas_Empleado_Negocio();
        string Mensaje_Error = "";

        try
        {
            // buscar registros con datos seleccionados
            Neg_Consulta.P_Empleado_ID = Cmb_Empleado.SelectedValue;
            Neg_Consulta.P_Programa_ID = Convert.ToInt32(Cmb_Programa.SelectedValue);
            Dt_Resultado = Neg_Consulta.Consultar_Programas_Empleados();

            // si la consulta arrojó resultados, asignar mensaje de error
            if (Dt_Resultado != null && Dt_Resultado.Rows.Count > 0)
            {
                Mensaje_Error = "Ya existe un registro con los datos seleccionados<br />";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Validar_Existe_Programa_Empleado " + ex.Message.ToString());
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
                    Cmb_Estatus.SelectedValue = "ACTIVO";
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
            Cmb_Empleado.Enabled = false;
            Cmb_Programa.Enabled = Habilitado;
            Cmb_Estatus.Enabled = Habilitado;
            Btn_Buscar_Empleado.Enabled = Habilitado;
            Btn_Buscar.Enabled = !Habilitado;
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
                    Mensaje_Validacion = Validar_Existe_Programa_Empleado();
                    // si no se encontraron registros con los datos seleccionados, llamar método alta de programa
                    if (string.IsNullOrEmpty(Mensaje_Validacion))
                    {
                        int Resultado_Insercion = Alta_Programa_Empleado();
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
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Programas Empleados", "alert('Ya existe un registro con los datos seleccionados');", true);
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
                if (Cmb_Empleado.SelectedIndex > 0)
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
                    Modificar_Programa_Empleado();
                    Inicializar_Controles();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Modificar_Click", "alert('Modificacion Exitosa');", true);
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
            if (Grid_Programa_Empleado.SelectedIndex >= 0)
            {
                if (Eliminar_Programa_Empleado() > 0)
                {
                    Inicializar_Controles();
                    Grid_Programa_Empleado.SelectedIndex = -1;
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
            Div_Buscar_Programa_Empleado.Style.Value = "display:block;";
            Consulta_Programas_Empleados();
        }
        catch (Exception ex)
        {
            throw new Exception("Btn_Buscar_Click " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE:          Llenar_Grid_Programa_Empleado
    ///DESCRIPCIÓN:     Realizara los metodos requeridos buscar un perfil
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_Grid_Programa_Empleado(DataTable Dt_Perfil)
    {
        Mostrar_Mensaje("");

        try
        {
            Grid_Programa_Empleado.Columns[1].Visible = true;
            Grid_Programa_Empleado.Columns[2].Visible = true;
            Grid_Programa_Empleado.Columns[3].Visible = true;
            Grid_Programa_Empleado.DataSource = Dt_Perfil;
            Grid_Programa_Empleado.DataBind();
            Grid_Programa_Empleado.Columns[1].Visible = false;
            Grid_Programa_Empleado.Columns[2].Visible = false;
            Grid_Programa_Empleado.Columns[3].Visible = false;

        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid_Programa_Empleado " + ex.Message.ToString(), ex);
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
            throw new Exception("Btn_Buscar_Empleado_Click " + ex.Message.ToString(), ex);
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
    ///NOMBRE:          Grid_Programa_Empleado_OnSelectedIndexChanged
    ///DESCRIPCIÓN:     carga los datos del elemento seleccionado en el grid para mostrarlos en la página
    ///PARAMETROS: 
    ///CREO:            Roberto González Oseguera
    ///FECHA_CREO:      30-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Programa_Empleado_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        String Empleado_ID = "";
        String Programa_ID = "";
        String Estatus = "";

        Mostrar_Mensaje("");
        try
        {
            if (Grid_Programa_Empleado.SelectedIndex > (-1))
            {
                Empleado_ID = HttpUtility.HtmlDecode(Grid_Programa_Empleado.SelectedRow.Cells[1].Text).ToString();
                Programa_ID = HttpUtility.HtmlDecode(Grid_Programa_Empleado.SelectedRow.Cells[2].Text).ToString();
                Estatus = HttpUtility.HtmlDecode(Grid_Programa_Empleado.SelectedRow.Cells[6].Text).ToString();

                // validar que el elemento a seleccionar exista en el combo
                if (Cmb_Empleado.Items.FindByValue(Empleado_ID) != null)
                {
                    Cmb_Empleado.SelectedValue = Empleado_ID;
                }
                if (Cmb_Programa.Items.FindByValue(Programa_ID) != null)
                {
                    Cmb_Programa.SelectedValue = Programa_ID;
                }
                if (Cmb_Estatus.Items.FindByValue(Estatus) != null)
                {
                    Cmb_Estatus.SelectedValue = Estatus;
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje("Error al cargar datos: " + ex.Message);
        }
    }
    #endregion Grid

}
