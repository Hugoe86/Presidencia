using System;
using System.Data;
using System.Web.UI;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;

public partial class paginas_Ordenamiento_Territorial_Frm_Cat_Ort_Parametros : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Mostrar_Informacion("", false);

        if (!IsPostBack)
        {
            Habilitar_Controles("Inicial");
            Llenar_Combos();
            Consultar_Parametros();

            string Ventana_Modal = "Abrir_Ventana_Modal('../Atencion_Ciudadana/Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Dependencia.Attributes.Add("onclick", Ventana_Modal);
            Btn_Buscar_Dependencia_Ambiental.Attributes.Add("onclick", Ventana_Modal);
            Btn_Buscar_Dependencia_Urbana.Attributes.Add("onclick", Ventana_Modal);
            Btn_Buscar_Dependencia_Fraccionamientos.Attributes.Add("onclick", Ventana_Modal);
            Btn_Buscar_Dependencia_Catastro.Attributes.Add("onclick", Ventana_Modal);
        }
    }

    #region METODOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Habilitar_Botones
    ///DESCRIPCIÓN: Establece las propiedades de los botones modificar y salir y del combo programa
    ///         dependiendo del contenido del parámetro recibido.
    ///PARÁMETROS:
    /// 		1. Estado: indica la operación que se pretende realizar y para la que se van a preparar los controles
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Habilitar_Controles(String Estado)
    {
        switch (Estado)
        {
            //Estado Incicial de los controles
            case "Inicial":
                Btn_Modificar.Visible = true;
                Btn_Salir.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Cmb_Dependencia.Enabled = false;
                Cmb_Dependencia_Ambiental.Enabled = false;
                Cmb_Dependencia_Fraccionamientos.Enabled = false;
                Cmb_Dependencia_Urbana.Enabled = false;
                Cmb_Dependencia_Catastro.Enabled = false;
                Cmb_Rol_Ordenamiento_Territorial.Enabled = false;
                Cmb_Rol_Ordenamiento_Ambiental.Enabled = false;
                Cmb_Rol_Ordenamiento_Fraccionamientos.Enabled = false;
                Cmb_Rol_Ordenamiento_Urbana.Enabled = false;
                Cmb_Rol_Inspector_Ordenamiento.Enabled = false;
                Btn_Buscar_Dependencia.Enabled = false;
                Btn_Buscar_Dependencia_Ambiental.Enabled = false;
                Btn_Buscar_Dependencia_Fraccionamientos.Enabled = false;
                Btn_Buscar_Dependencia_Urbana.Enabled = false;
                Txt_Costo_Perito.Enabled = false;
                Txt_Costo_Bitacora.Enabled = false;
                break;
            //Estado de Modificar
            case "Modificar":
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ToolTip = "Cancelar modificación";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Cmb_Dependencia.Enabled = true;
                Btn_Buscar_Dependencia.Enabled = true;
                Cmb_Dependencia_Ambiental.Enabled = true;
                Cmb_Dependencia_Fraccionamientos.Enabled = true;
                Cmb_Dependencia_Urbana.Enabled = true;
                Cmb_Dependencia_Catastro.Enabled = true;
                Cmb_Rol_Ordenamiento_Territorial.Enabled = true;
                Cmb_Rol_Ordenamiento_Ambiental.Enabled = true;
                Cmb_Rol_Ordenamiento_Fraccionamientos.Enabled = true;
                Cmb_Rol_Ordenamiento_Urbana.Enabled = true;
                Cmb_Rol_Inspector_Ordenamiento.Enabled = true;
                Btn_Buscar_Dependencia_Ambiental.Enabled = true;
                Btn_Buscar_Dependencia_Fraccionamientos.Enabled = true;
                Btn_Buscar_Dependencia_Urbana.Enabled = true;
                Txt_Costo_Perito.Enabled = true;
                Txt_Costo_Bitacora.Enabled = true;
                break;
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: LLenar_Combos
    ///DESCRIPCIÓN: Consulta los catálogos y carga los datos en el combo correspondiente
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Llenar_Combos()
    {
        
        try
        {
            //llenar UR
            Cls_Cat_Dependencias_Negocio Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
            DataTable Dt_UR = Dependencia_Negocio.Consulta_Dependencias();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia, Dt_UR, "CLAVE_NOMBRE", "DEPENDENCIA_ID");
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia_Ambiental, Dt_UR, "CLAVE_NOMBRE", "DEPENDENCIA_ID");
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia_Urbana, Dt_UR, "CLAVE_NOMBRE", "DEPENDENCIA_ID");
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia_Fraccionamientos, Dt_UR, "CLAVE_NOMBRE", "DEPENDENCIA_ID");
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia_Catastro, Dt_UR, "CLAVE_NOMBRE", "DEPENDENCIA_ID");

            //  llenar los perfiles
            Cls_Cat_Ort_Parametros_Negocio Negocio_Rol = new Cls_Cat_Ort_Parametros_Negocio();
            DataTable Dt_Rol = Negocio_Rol.Consultar_Rol();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Rol_Ordenamiento_Territorial, Dt_Rol, Apl_Cat_Roles.Campo_Nombre, Apl_Cat_Roles.Campo_Rol_ID);
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Rol_Ordenamiento_Ambiental, Dt_Rol, Apl_Cat_Roles.Campo_Nombre, Apl_Cat_Roles.Campo_Rol_ID);
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Rol_Ordenamiento_Fraccionamientos, Dt_Rol, Apl_Cat_Roles.Campo_Nombre, Apl_Cat_Roles.Campo_Rol_ID);
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Rol_Ordenamiento_Urbana, Dt_Rol, Apl_Cat_Roles.Campo_Nombre, Apl_Cat_Roles.Campo_Rol_ID);
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Rol_Inspector_Ordenamiento, Dt_Rol, Apl_Cat_Roles.Campo_Nombre, Apl_Cat_Roles.Campo_Rol_ID);
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al cargar combos: " + Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Parametros
    ///DESCRIPCIÓN: Consulta los parámetros y si existen en el combo correspondiente, lo selecciona
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Consultar_Parametros()
    {
        var Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();

        try
        {
            // consultar parámetros
            Obj_Parametros.Consultar_Parametros();

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
            {
                // si la dependencia existe en el combo, seleccionarlo
                string Dependencia_Id = Obj_Parametros.P_Dependencia_ID_Ordenamiento;
                if (Cmb_Dependencia.Items.FindByValue(Dependencia_Id) != null)
                {
                    Cmb_Dependencia.SelectedValue = Dependencia_Id;
                }
            }

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
            {
                // si la dependencia existe en el combo, seleccionarlo
                string Dependencia_Id = Obj_Parametros.P_Dependencia_ID_Ambiental;
                if (Cmb_Dependencia_Ambiental.Items.FindByValue(Dependencia_Id) != null)
                {
                    Cmb_Dependencia_Ambiental.SelectedValue = Dependencia_Id;
                }
            }

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
            {
                // si la dependencia existe en el combo, seleccionarlo
                string Dependencia_Id = Obj_Parametros.P_Dependencia_ID_Urbanistico;
                if (Cmb_Dependencia_Urbana.Items.FindByValue(Dependencia_Id) != null)
                {
                    Cmb_Dependencia_Urbana.SelectedValue = Dependencia_Id;
                }
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
            {
                // si la dependencia existe en el combo, seleccionarlo
                string Dependencia_Id = Obj_Parametros.P_Dependencia_ID_Inmobiliario;
                if (Cmb_Dependencia_Fraccionamientos.Items.FindByValue(Dependencia_Id) != null)
                {
                    Cmb_Dependencia_Fraccionamientos.SelectedValue = Dependencia_Id;
                }
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Catastro))
            {
                // si la dependencia existe en el combo, seleccionarlo
                string Dependencia_Id = Obj_Parametros.P_Dependencia_ID_Catastro;
                if (Cmb_Dependencia_Catastro.Items.FindByValue(Dependencia_Id) != null)
                {
                    Cmb_Dependencia_Catastro.SelectedValue = Dependencia_Id;
                }
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
            {
                // si la dependencia existe en el combo, seleccionarlo
                string Dependencia_Id = Obj_Parametros.P_Rol_Director_Ordenamiento;
                if (Cmb_Rol_Ordenamiento_Territorial.Items.FindByValue(Dependencia_Id) != null)
                {
                    Cmb_Rol_Ordenamiento_Territorial.SelectedValue = Dependencia_Id;
                }
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ambiental))
            {
                // si la dependencia existe en el combo, seleccionarlo
                string Dependencia_Id = Obj_Parametros.P_Rol_Director_Ambiental;
                if (Cmb_Rol_Ordenamiento_Ambiental.Items.FindByValue(Dependencia_Id) != null)
                {
                    Cmb_Rol_Ordenamiento_Ambiental.SelectedValue = Dependencia_Id;
                }
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Fraccionamientos))
            {
                // si la dependencia existe en el combo, seleccionarlo
                string Dependencia_Id = Obj_Parametros.P_Rol_Director_Fraccionamientos;
                if (Cmb_Rol_Ordenamiento_Fraccionamientos.Items.FindByValue(Dependencia_Id) != null)
                {
                    Cmb_Rol_Ordenamiento_Fraccionamientos.SelectedValue = Dependencia_Id;
                }
            }
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Urbana))
            {
                // si la dependencia existe en el combo, seleccionarlo
                string Dependencia_Id = Obj_Parametros.P_Rol_Director_Urbana;
                if (Cmb_Rol_Ordenamiento_Urbana.Items.FindByValue(Dependencia_Id) != null)
                {
                    Cmb_Rol_Ordenamiento_Urbana.SelectedValue = Dependencia_Id;
                }
            }

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Inspector_Ordenamiento))
            {
                // si la dependencia existe en el combo, seleccionarlo
                string Dependencia_Id = Obj_Parametros.P_Rol_Inspector_Ordenamiento;
                if (Cmb_Rol_Inspector_Ordenamiento.Items.FindByValue(Dependencia_Id) != null)
                {
                    Cmb_Rol_Inspector_Ordenamiento.SelectedValue = Dependencia_Id;
                }
            }

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Costo_Perito))
            {
                Txt_Costo_Perito.Text = Obj_Parametros.P_Costo_Perito;
            }

            if (!string.IsNullOrEmpty(Obj_Parametros.P_Costo_Bitacora))
            {
                Txt_Costo_Bitacora.Text = Obj_Parametros.P_Costo_Bitacora;
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al cargar parámetros: " + Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Informacion
    ///DESCRIPCIÓN: Muestra en la página el mensaje recibido como parámetro y establece la visibilidad 
    ///             de los controles  para mostrar mensajes con el segundo parámetro
    ///PARÁMETROS:
    /// 		1. Mensaje: Texto a mostrar en la página
    /// 		2. Mostrar: establece la visibilidad de los controles en los que se muestran los mensajes de la página
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Mostrar_Informacion(String Mensaje, bool Mostrar)
    {
        Lbl_Informacion.Text = Mensaje;
        Lbl_Informacion.Visible = Mostrar;
        Img_Informacion.Visible = Mostrar;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Modificar_Parametros
    ///DESCRIPCIÓN: Consulta los parámetros y si exiten en el combo correspondiente, lo selecciona
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Modificar_Parametros()
    {
        var Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();

        try
        {
            // agregar valores para actualizar
            Obj_Parametros.P_Dependencia_ID_Ordenamiento = Cmb_Dependencia.SelectedValue;
            Obj_Parametros.P_Dependencia_ID_Ambiental = Cmb_Dependencia_Ambiental.SelectedValue;
            Obj_Parametros.P_Dependencia_ID_Urbanistico = Cmb_Dependencia_Urbana.SelectedValue;
            Obj_Parametros.P_Dependencia_ID_Inmobiliario = Cmb_Dependencia_Fraccionamientos.SelectedValue;
            Obj_Parametros.P_Dependencia_ID_Catastro = Cmb_Dependencia_Catastro.SelectedValue;
            Obj_Parametros.P_Rol_Director_Ordenamiento = Cmb_Rol_Ordenamiento_Territorial.SelectedValue;
            Obj_Parametros.P_Rol_Director_Ambiental = Cmb_Rol_Ordenamiento_Ambiental.SelectedValue;
            Obj_Parametros.P_Rol_Director_Fraccionamientos = Cmb_Rol_Ordenamiento_Fraccionamientos.SelectedValue;
            Obj_Parametros.P_Rol_Director_Urbana = Cmb_Rol_Ordenamiento_Urbana.SelectedValue;
            Obj_Parametros.P_Rol_Inspector_Ordenamiento = Cmb_Rol_Inspector_Ordenamiento.SelectedValue;
            Obj_Parametros.P_Costo_Perito = Txt_Costo_Perito.Text;
            Obj_Parametros.P_Costo_Bitacora = Txt_Costo_Bitacora.Text;
            Obj_Parametros.P_Usuario = Cls_Sessiones.Nombre_Empleado;

            if (Obj_Parametros.Actualizar_Parametros() > 0)
            {
                Habilitar_Controles("Inicial");
                Consultar_Parametros();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('Actualización exitosa.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('No fue posible realizar la actualización.');", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al actualizar parámetros: " + Ex, true);
        }
    }


    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Validar_Informacion
    ///DESCRIPCIÓN: valida que la informacion se ingrese
    ///PARÁMETROS:
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 21-Nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public Boolean Validar_Informacion()
    {
        Mostrar_Informacion("", false);
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;
        String Texto_Mensaje = "";

        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Texto_Mensaje += "Es necesario Introducir: <br>";
        try
        {
            if (Cmb_Dependencia.SelectedIndex == 0)
            {
                Texto_Mensaje += "*Seleccione la dependencia de Ordenamiento territorial.<br>";
                Datos_Validos = false;
            }
            if (Cmb_Dependencia_Ambiental.SelectedIndex == 0)
            {
                Texto_Mensaje += "*Seleccione la dependencia de la area de ambiental.<br>";
                Datos_Validos = false;
            }

            if (Cmb_Dependencia_Fraccionamientos.SelectedIndex == 0)
            {
                Texto_Mensaje += "*Seleccione la dependencia de la area de Fraccionamientos.<br>";
                Datos_Validos = false;
            }
            if (Cmb_Dependencia_Urbana.SelectedIndex == 0)
            {
                Texto_Mensaje += "*Seleccione la dependencia de la area de Urbano.<br>";
                Datos_Validos = false;
            }
            if (Cmb_Dependencia_Catastro.SelectedIndex == 0)
            {
                Texto_Mensaje += "*Seleccione la dependencia de Catastro.<br>";
                Datos_Validos = false;
            }
            if (Cmb_Rol_Ordenamiento_Territorial.SelectedIndex == 0)
            {
                Texto_Mensaje += "*Seleccione el rol de ordenamiento territorial.<br>";
                Datos_Validos = false;
            }
            if (Cmb_Rol_Ordenamiento_Ambiental.SelectedIndex == 0)
            {
                Texto_Mensaje += "*Seleccione el rol de ambiental.<br>";
                Datos_Validos = false;
            }
            if (Cmb_Rol_Ordenamiento_Fraccionamientos.SelectedIndex == 0)
            {
                Texto_Mensaje += "*Seleccione el rol de Fraccionamientos.<br>";
                Datos_Validos = false;
            }
            if (Cmb_Rol_Ordenamiento_Urbana.SelectedIndex == 0)
            {
                Texto_Mensaje += "*Seleccione el rol de Urbano.<br>";
                Datos_Validos = false;
            } 
            if (Cmb_Rol_Inspector_Ordenamiento.SelectedIndex == 0)
            {
                Texto_Mensaje += "*Seleccione el rol de los inspectores de ordenaminento.<br>";
                Datos_Validos = false;
            }
            if (Txt_Costo_Perito.Text == "")
            {
                Texto_Mensaje += "*Ingrese el costo de los peritos.<br>";
                Datos_Validos = false;
            }
            if (Txt_Costo_Bitacora.Text == "")
            {
                Texto_Mensaje += "*Ingrese el costo del uso de la bitacora.<br>";
                Datos_Validos = false;
            }
          
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al actualizar parámetros: " + Ex, true);
        }
        return Datos_Validos;
    }
    #endregion METODOS


    #region EVENTOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Dependiendo del estado del botón (tooltipo: Modificar o Actualizar)
    ///         Configurar controles o actualiza el parámetro
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        // limpiar mensajes de error
        Mostrar_Informacion("", false);

        try
        {
            // validar estado del botón
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                Habilitar_Controles("Modificar");
            }
            else
            {
                // validar que haya un programa seleccionado
                if (Validar_Informacion())
                {
                    Modificar_Parametros();
                }
                else
                {
                    Mostrar_Informacion("Es necesario seleccionar la Unidad Responsable.", true);
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
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
        Mostrar_Informacion("", false);

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
                    if (Cmb_Dependencia.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencia.SelectedValue = Dependencia_ID;
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message, true);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Dependencia_Ambiental_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Dependencia_Ambiental_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Informacion("", false);

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
                    if (Cmb_Dependencia_Ambiental.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencia_Ambiental.SelectedValue = Dependencia_ID;
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message, true);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Dependencia_Urbana_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Dependencia_Urbana_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Informacion("", false);

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
                    if (Cmb_Dependencia_Urbana.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencia_Urbana.SelectedValue = Dependencia_ID;
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message, true);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Dependencia_Fraccionamientos_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Dependencia_Fraccionamientos_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Informacion("", false);

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
                    if (Cmb_Dependencia_Fraccionamientos.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencia_Fraccionamientos.SelectedValue = Dependencia_ID;
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message, true);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Dependencia_Catastro_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Dependencia_Catastro_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Informacion("", false);

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
                    if (Cmb_Dependencia_Catastro.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencia_Catastro.SelectedValue = Dependencia_ID;
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message, true);
                }

                // limpiar variables de sesión
                Session.Remove("DEPENDENCIA_ID");
                Session.Remove("NOMBRE_DEPENDENCIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_DEPENDENCIAS");
        }
    }
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Manejo del evento click en el botón de salir: dependiendo del tooltip del botón, regresa a 
    ///         la página principal o reinicia los controles de la página a su estado Inicial
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Inicio")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Habilitar_Controles("Inicial");
            Consultar_Parametros();
        }
    }

    #endregion EVENTOS

}
