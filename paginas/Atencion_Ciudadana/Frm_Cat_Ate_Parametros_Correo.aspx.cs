using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Atencion_Ciudadana_Parametros_Correo.Negocio;
using System.IO;
using System.Collections.Generic;

public partial class paginas_Atencion_Ciudadana_Frm_Cat_Ate_Parametros_Correo : System.Web.UI.Page
{
    private const string Directorio_Imagenes_Correo = "~/Archivos/Atencion_Ciudadana/Imagenes/";

    protected void Page_Load(object sender, EventArgs e)
    {
        Mostrar_Informacion("", false);
        Tsm_Acciones.RegisterPostBackControl(Btn_Subir_Archivo);

        if (!IsPostBack)
        {
            // mostrar el UpdateProgress
            Btn_Subir_Archivo.Attributes["onclick"] = "$get('" + Uprg_Servicios.ClientID + "').style.display = 'block'; return true;";
            // inicializar controles
            Habilitar_Controles("Inicial");
            Consultar_Parametros();
            Llenar_Grid_Archivos();
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
        bool Habilitado = false;
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
                Habilitado = false;
                break;
            //Estado de Modificar
            case "Modificar":
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Habilitado = true;
                break;
        }

        // habilitar controles
        Cmb_Tipo_Correo.Enabled = !Habilitado;
        Txt_Servidor.Enabled = Habilitado;
        Txt_Puerto.Enabled = Habilitado;
        Txt_Usuario.Enabled = Habilitado;
        Txt_Password.Enabled = Habilitado;
        Txt_Saludo_Correo.Enabled = Habilitado;
        Txt_Cuerpo_Correo.Enabled = Habilitado;
        Txt_Firma_Correo.Enabled = Habilitado;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Parametros
    ///DESCRIPCIÓN: Consulta los parámetros y carga los datos en los controles correspondientes
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 04-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Consultar_Parametros()
    {
        var Obj_Parametros = new Cls_Cat_Ate_Parametros_Correo_Negocio();
        DataTable Dt_Parametros;
        try
        {
            Obj_Parametros.P_Tipo_Correo = Cmb_Tipo_Correo.SelectedValue;
            // obtener los parámetros
            Dt_Parametros = Obj_Parametros.Consultar_Parametros_Correo();
            // cargar datos en los controles
            Txt_Servidor.Text = Obj_Parametros.P_Correo_Servidor;
            Txt_Puerto.Text = Obj_Parametros.P_Correo_Puerto;
            Txt_Usuario.Text = Obj_Parametros.P_Correo_Remitente;
            Txt_Password.Text = Obj_Parametros.P_Password_Usuario_Correo;
            Txt_Saludo_Correo.Text = Obj_Parametros.P_Correo_Saludo;
            Txt_Cuerpo_Correo.Text = Obj_Parametros.P_Correo_Cuerpo;
            Txt_Firma_Correo.Text = Obj_Parametros.P_Correo_Firma;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar parámetros: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Llenar_Grid_Archivos
    ///DESCRIPCIÓN: obtiene un listado de archivos de un directorio y los muestra en el grid de archivos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 21-ene-2013
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Llenar_Grid_Archivos()
    {
        DataTable Dt_Archivos;
        DataRow Dr_Archivo;
        List<string> Lista_Archivos;
        string Ruta_Directorio = Server.MapPath(Directorio_Imagenes_Correo);

        try
        {
            // crear datatable 
            Dt_Archivos = new DataTable();
            Dt_Archivos.Columns.Add("NOMBRE_ARCHIVO", typeof(string));
            Dt_Archivos.Columns.Add("CADENA_ARCHIVO", typeof(string));
            // validar que el dirtorio existe
            if (Directory.Exists(Ruta_Directorio))
            {
                // obtener listado de archivos
                Lista_Archivos = Directory.GetFiles(Ruta_Directorio, "*.*", SearchOption.TopDirectoryOnly).ToList<string>();
                foreach (string Archivo in Lista_Archivos)
                {
                    Dr_Archivo = Dt_Archivos.NewRow();
                    Dr_Archivo[0] = Path.GetFileName(Archivo);
                    Dr_Archivo[1] = "cid:" + Path.GetFileNameWithoutExtension(Archivo);
                    Dt_Archivos.Rows.Add(Dr_Archivo);
                }
                // cargar en el grid
                Grid_Archivos.DataSource = Dt_Archivos;
                Grid_Archivos.DataBind();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al leer archivos: " + Ex.Message);
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
        var Obj_Parametros = new Cls_Cat_Ate_Parametros_Correo_Negocio();

        try
        {
            // agregar valores para actualizar
            Obj_Parametros.P_Tipo_Correo = Cmb_Tipo_Correo.SelectedValue;
            Obj_Parametros.P_Correo_Servidor = Txt_Servidor.Text;
            Obj_Parametros.P_Correo_Puerto = Txt_Puerto.Text;
            Obj_Parametros.P_Correo_Remitente = Txt_Usuario.Text;
            // sustituir caracter comilla en el campo password (no está filtrado del lado del cliente)
            Obj_Parametros.P_Password_Usuario_Correo = Txt_Password.Text.Replace("'", "''");
            Obj_Parametros.P_Correo_Saludo = Txt_Saludo_Correo.Text;
            Obj_Parametros.P_Correo_Cuerpo = Txt_Cuerpo_Correo.Text.Replace("'", "''");
            Obj_Parametros.P_Correo_Firma = Txt_Firma_Correo.Text;
            Obj_Parametros.P_Usuario_Creo_Modifico = Cls_Sessiones.Nombre_Empleado;

            if (Obj_Parametros.Actualizar_Parametros_Correo() > 0)
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
            throw new Exception("Error al actualizar parámetros: " + Ex.Message);
        }
    }

    #endregion METODOS


    #region EVENTOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Dependiendo del estado del botón (tooltipo: Modificar o Actualizar)
    ///         Configurar controles o actualiza el parametro
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
                // llamar método para actualizar parámetros
                Modificar_Parametros();
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
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

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Manejo del evento click en el botón de eliminar del grid: eliminar el archivo que llega 
    ///             como CommandArgument
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 21-ene-2013
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        // limpiar mensajes de error
        Mostrar_Informacion("", false);

        try
        {
            // validar que el objeto que envia la señal no sea nulo
            if (sender != null)
            {
                ImageButton Boton = (ImageButton)sender;
                // validar que el commandargument contenga texto
                if (!string.IsNullOrEmpty(Boton.CommandArgument))
                {
                    // validar que el archivo existe
                    if (File.Exists(Server.MapPath(Directorio_Imagenes_Correo + Boton.CommandArgument)))
                    {
                        // borrar archivo
                        File.Delete(Server.MapPath(Directorio_Imagenes_Correo + Boton.CommandArgument));
                        Llenar_Grid_Archivos();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Información archivo eliminado", "alert('El archivo fue eliminado exitosamente.');", true);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Subir_Archivo_Click
    ///DESCRIPCIÓN: Manejo del evento click en el botón de subir archivo: guardar archivo subido por usuario
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 21-ene-2013
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Subir_Archivo_Click(object sender, ImageClickEventArgs e)
    {
        // limpiar mensajes de error
        Mostrar_Informacion("", false);
        List<string> Extensiones_Aceptadas = new List<string>() { ".png", ".jpg", ".gif" };

        try
        {
            // comprobar que se recibe un archivo
            if (Fup_Archivo.HasFile)
            {
                // comprobar la extensión del archivo
                if (Extensiones_Aceptadas.Contains(Path.GetExtension(Fup_Archivo.FileName)))
                {
                    string Ruta_Archivo = Server.MapPath(Directorio_Imagenes_Correo + Fup_Archivo.FileName.ToLower().Replace(" ", "_"));
                    // si el directorio no existe, crearlo
                    if (!Directory.Exists(Server.MapPath(Directorio_Imagenes_Correo)))
                    {
                        Directory.Exists(Server.MapPath(Directorio_Imagenes_Correo));
                    }
                    // borrar archivo antes de guardar nuevo
                    if (File.Exists(Ruta_Archivo))
                    {
                        File.Delete(Ruta_Archivo);
                    }
                    // guardar el archivo
                    Fup_Archivo.SaveAs(Ruta_Archivo);
                    // mostrar mensaje notificando que el archivo se guardó
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('El archivo se subió con éxito.');", true);
                    Llenar_Grid_Archivos();
                }
                else
                {
                    Llenar_Grid_Archivos();
                    Mostrar_Informacion("La extensión del archivo no es válida.", true);
                }
            }
            else
            {
                Llenar_Grid_Archivos();
                Mostrar_Informacion("Se requiere especificar un archivo.", true);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Tipo_Correo_SelectedIndexChanged
    ///DESCRIPCIÓN: Manejo del evento cambio de selección en el combo tipo de correo: consultar los 
    ///             parámetros del tipo de correo seleccionado
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 22-oct-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    protected void Cmb_Tipo_Correo_SelectedIndexChanged(object sender, EventArgs e)
    {
        // limpiar mensajes de error
        Mostrar_Informacion("", false);

        try
        {
            Consultar_Parametros();
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message, true);
        }
    }

    #endregion EVENTOS
}
