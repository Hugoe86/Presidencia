using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Acciones_AC.Negocio;
using Presidencia.Constantes;

public partial class paginas_Atencion_Ciudadana_Frm_Cat_Ate_Acciones : System.Web.UI.Page
{
    //Modos de formulario
    private const String MODO_LISTADO = "listado";
    private const String MODO_INICIO = "inicio";
    private const String MODO_NUEVO = "nuevo";
    private const String MODO_MODIFICAR = "modificar";
    //Estatus
    private const String ESTATUS_ACTIVO = "ACTIVO";
    private const String ESTATUS_INACTIVO = "INACTIVO";
    //Tool Tips
    private const String TOOLTIP_NUEVO = "Nuevo";
    private const String TOOLTIP_GUARDAR = "Guardar";
    private const String TOOLTIP_ACTUALIZAR = "Actualizar";
    private const String TOOLTIP_MODIFICAR = "Modificar";
    private const String TOOLTIP_CANCELAR = "Cancelar";
    private const String TOOLTIP_INICIO = "Inicio";
    private const String TOOLTIP_SALIR = "Salir";
    private const String TOOLTIP_ELIMINAR = "Eliminar";
    //Sesiones
    private static String P_Dt_Datos = "Dt_Datos_Acciones";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Cargar_Datos_Iniciales();
            Llenar_Informacion_Grid();
            Manejo_Comandos(MODO_INICIO);
        }
        Informacion_Formulario("", false);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Datos_Iniciales
    ///DESCRIPCIÓN: Carga los elementos del combo estatus
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Datos_Iniciales()
    {
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("SELECCIONAR");
        Cmb_Estatus.Items.Add("ACTIVO");
        Cmb_Estatus.Items.Add("INACTIVO");
        Cmb_Estatus.Items[0].Selected = true;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Limpia la inforamación de la s cajas de texto y la selcción del combo
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Formulario()
    {
        Txt_Busqueda.Text = "";
        Txt_Clave.Text = "";
        Txt_Nombre.Text = "";
        Txt_Tiempo_Solucion.Text = "";
        Txt_Descripcion.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Manejo_Comandos
    ///DESCRIPCIÓN: Configura los controles de la página para la operación que se recibo como parámetro
    ///PARÁMETROS:
    /// 		1. Modo: cadena de texto con el modo a preparar de los controles
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Manejo_Comandos(String Modo)
    {
        switch (Modo)
        {
            case MODO_LISTADO:

                break;
            case MODO_INICIO:
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = true;
                Btn_Eliminar.Visible = true;
                Btn_Salir.Visible = true;
                Btn_Buscar.Enabled = true;
                Txt_Busqueda.Enabled = true;

                Btn_Nuevo.ToolTip = TOOLTIP_NUEVO;
                Btn_Modificar.ToolTip = TOOLTIP_MODIFICAR;
                Btn_Eliminar.ToolTip = TOOLTIP_ELIMINAR;
                Btn_Salir.ToolTip = TOOLTIP_INICIO;

                Txt_Clave.Enabled = false;
                Txt_Nombre.Enabled = false;
                Txt_Tiempo_Solucion.Enabled = false;
                Txt_Descripcion.Enabled = false;
                Cmb_Estatus.Enabled = false;
                Grid_Datos.Enabled = true;

                Llenar_Informacion_Grid();
                Limpiar_Formulario();
                HF_ID.Value = "";
                break;
            case MODO_NUEVO:
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = false;
                Btn_Eliminar.Visible = false;
                Btn_Salir.Visible = true;
                Btn_Buscar.Enabled = false;
                Txt_Busqueda.Enabled = false;

                Btn_Nuevo.ToolTip = TOOLTIP_GUARDAR;
                Btn_Salir.ToolTip = TOOLTIP_CANCELAR;

                Limpiar_Formulario();
                Txt_Clave.Enabled = true;
                Txt_Nombre.Enabled = true;
                Txt_Tiempo_Solucion.Enabled = true;
                Txt_Descripcion.Enabled = true;
                Cmb_Estatus.Enabled = true;
                Grid_Datos.Enabled = false;
                Cmb_Estatus.SelectedValue = ESTATUS_ACTIVO;
                HF_ID.Value = "";
                break;
            case MODO_MODIFICAR:
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                Btn_Nuevo.Visible = false;
                Btn_Modificar.Visible = true;
                Btn_Eliminar.Visible = false;
                Btn_Salir.Visible = true;
                Btn_Buscar.Enabled = false;
                Txt_Busqueda.Enabled = false;

                Btn_Modificar.ToolTip = TOOLTIP_ACTUALIZAR;
                Btn_Salir.ToolTip = TOOLTIP_CANCELAR;

                Txt_Clave.Enabled = true;
                Txt_Nombre.Enabled = true;
                Txt_Tiempo_Solucion.Enabled = true;
                Txt_Descripcion.Enabled = true;
                Cmb_Estatus.Enabled = true;
                Grid_Datos.Enabled = false;
                break;
        }

    }

    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Informacion_Grid();
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: elimina el registro de la acción seleccionada y muestra un mensaje indicando el 
    ///             resultado de la operación
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {

        if (Btn_Nuevo.ToolTip == TOOLTIP_NUEVO)
        {
            Manejo_Comandos(MODO_NUEVO);
        }
        else if (Btn_Nuevo.ToolTip == TOOLTIP_GUARDAR)
        {
            //proceso guaradr aqui
            if (Validar_Campos())
            {
                if (!Clave_Duplicada())
                {

                    int Registros_Nuevos = Nuevo_Registro();
                    if (Registros_Nuevos > 0)
                    {
                        Mensaje_PopUp("Se guardó el registro correctamente");
                        Manejo_Comandos(MODO_INICIO);
                    }
                    else
                    {
                        Mensaje_PopUp("No se pudo guardar la información. Verifique datos");
                    }
                }
                else
                {
                    Mensaje_PopUp("La clave ingresada ya es usuada por otro registro");
                }
            }
            else
            {
                Informacion_Formulario("Los campos marcados con * son obligatorios", true);
            }
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: prepara los controles para modificar el registro seleccionado o si ya se modificó y validó, 
    ///             se llama al método que actualiza el registro en base de datos
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Modificar.ToolTip == TOOLTIP_MODIFICAR)
        {
            if (Txt_Clave.Text.Trim().Length > 0)
            {
                Manejo_Comandos(MODO_MODIFICAR);
            }
            else
            {
                Mensaje_PopUp("Seleccione un registro para modificar");
            }
        }
        else if (Btn_Modificar.ToolTip == TOOLTIP_ACTUALIZAR)
        {
            if (Validar_Campos())
            {
                //cargar datos de negocio
                if (!Clave_Duplicada())
                {
                    int Registros_Actualizados = Actualizar_Registro();
                    if (Registros_Actualizados > 0)
                    {
                        Mensaje_PopUp("Se actualizó el registro correctamente");
                        Manejo_Comandos(MODO_INICIO);
                    }
                    else
                    {
                        Mensaje_PopUp("No se pudo actualizar la información. Verifique datos");
                    }
                }
                else
                {
                    Mensaje_PopUp("La clave ingresada ya es usuada por otro registro");
                }
            }
            else
            {
                Informacion_Formulario("Los campos marcados con * son obligatorios", true);
            }
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: elimina el registro de la acción seleccionada y muestra un mensaje indicando el 
    ///             resultado de la operación
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Clave.Text.Trim().Length > 0)
        {
            //cargar datos de negocio

            int Registros_Nuevos = Eliminar_Registro();
            if (Registros_Nuevos > 0)
            {
                Mensaje_PopUp("El registro fué eliminado");
                Limpiar_Formulario();
                Manejo_Comandos(MODO_INICIO);
            }
            else
            {
                Mensaje_PopUp("No se eliminaron los datos, es posible que sea usuado en otros registros");
            }
            Manejo_Comandos(MODO_INICIO);
        }
        else
        {
            Mensaje_PopUp("Seleccione un registro para eliminar");
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: dependiendo del tooltip del botón salir, redirecciona a la página principal 
    ///             o limpia los controles en la página
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == TOOLTIP_INICIO)
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else if (Btn_Salir.ToolTip == TOOLTIP_CANCELAR)
        {
            //proceso cancelar aqui
            Limpiar_Formulario();
            Manejo_Comandos(MODO_INICIO);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Seleccionar_Click
    ///DESCRIPCIÓN: Llama al método que carga los datos de la acción seleccionada
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Seleccionar_Click(object sender, ImageClickEventArgs e)
    {
        String ID = ((ImageButton)sender).CommandArgument;
        HF_ID.Value = ID;
        Cargar_Informacion_Seleccionada(ID);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Informacion_Seleccionada
    ///DESCRIPCIÓN: Carga en los controles de la página la información de la acción cuyo ID se recibe como parámetro
    ///PARÁMETROS:
    /// 		1. ID: id de la acción que se va a consultar y mostrar en la página
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Informacion_Seleccionada(String ID)
    {
        DataTable Dt_Tabla = Session[P_Dt_Datos] as DataTable;
        DataRow[] Datos_Seleccionados = Dt_Tabla.Select("ACCION_ID = '" + ID + "'");
        Limpiar_Formulario();
        if (Datos_Seleccionados != null && Datos_Seleccionados.Length > 0)
        {
            Txt_Clave.Text = Datos_Seleccionados[0]["CLAVE"].ToString().Trim();
            Txt_Nombre.Text = Datos_Seleccionados[0]["NOMBRE"].ToString().Trim();
            Txt_Tiempo_Solucion.Text = Datos_Seleccionados[0][Cat_Ate_Acciones.Campo_Tiempo_Estimado_Solucion].ToString().Trim();
            Txt_Descripcion.Text = Datos_Seleccionados[0]["DESCRIPCION"].ToString().Trim();
            Cmb_Estatus.SelectedValue = Datos_Seleccionados[0]["ESTATUS"].ToString().Trim();
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Llenar_Informacion_Grid
    ///DESCRIPCIÓN: Carga en el grid acciones el contenido de la sessión P_Dt_Datos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Llenar_Informacion_Grid()
    {
        DataTable Dt_Datos = Consultar_Registros();
        Session[P_Dt_Datos] = Dt_Datos;
        Grid_Datos.DataSource = Dt_Datos;
        Grid_Datos.DataBind();
    }

    //VALIDACION DE CAMPOS
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Validar_Campos
    ///DESCRIPCIÓN: Regresa verdadero si todos los campos obligatorios contienen los datos correctos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private bool Validar_Campos()
    {
        int Dias_Solucion;
        bool Valido = true;
        if (Txt_Clave.Text.Trim().Length == 0)
        {
            Valido = false;
        }
        if (Txt_Nombre.Text.Trim().Length == 0)
        {
            Valido = false;
        }
        if (!int.TryParse(Txt_Tiempo_Solucion.Text.Trim(), out Dias_Solucion))
        {
            Valido = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            Valido = false;
        }
        return Valido;
    }

    //MANEJO BASE DE DATOS
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Registros
    ///DESCRIPCIÓN: Regresa un datatable con los registros de acciones en la tabla Cat_Ate_Acciones
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Consultar_Registros()
    {
        DataTable Dt_Tabla_Consulta = null;
        //Cargar datos de negocio y consultar
        Cls_Cat_Ate_Acciones_Negocio Negocio = new Cls_Cat_Ate_Acciones_Negocio();
        Negocio.P_Clave = Txt_Busqueda.Text.Trim();
        Dt_Tabla_Consulta = Negocio.Consultar_Registros();
        return Dt_Tabla_Consulta;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Actualizar_Registro
    ///DESCRIPCIÓN: Actualiza la información de la acción con los datos en la página mediante el uso de la 
    ///         clase de negocio Cls_Cat_Ate_Acciones_Negocio y regresa un entero con el número de registros actualizados
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private int Actualizar_Registro()
    {
        int Registros_Actualizados = 0;
        int Tiempo_Solucion;
        int.TryParse(Txt_Tiempo_Solucion.Text, out Tiempo_Solucion);
        //Cargar datos de negocio y actualizar
        Cls_Cat_Ate_Acciones_Negocio Negocio = new Cls_Cat_Ate_Acciones_Negocio();
        Negocio.P_ID = HF_ID.Value;
        Negocio.P_Clave = Txt_Clave.Text.Trim();
        Negocio.P_Nombre = Txt_Nombre.Text.Trim();
        Negocio.P_Tiempo_Estimado_Solucion = Tiempo_Solucion;
        Negocio.P_Descripcion = Txt_Descripcion.Text.Trim();
        Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
        Registros_Actualizados = Negocio.Actualizar_Registro();
        return Registros_Actualizados;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Eliminar_Registro
    ///DESCRIPCIÓN: elimina el registro seleccionado mediante la llamada al método del mismo nombre de la clase de negocio
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private int Eliminar_Registro()
    {
        int Registros_Eliminados = 0;
        //Cargar datos de negocio y actualizar
        Cls_Cat_Ate_Acciones_Negocio Negocio = new Cls_Cat_Ate_Acciones_Negocio();
        Negocio.P_ID = HF_ID.Value;
        Registros_Eliminados = Negocio.Eliminar_Registro();
        return Registros_Eliminados;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Nuevo_Registro
    ///DESCRIPCIÓN: da de alta la acción con los datos en la página mediante el uso de la 
    ///         clase de negocio Cls_Cat_Ate_Acciones_Negocio y regresa un entero con el número de registros guardados
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private int Nuevo_Registro()
    {
        int Registros_Actualizados = 0;
        int Tiempo_Solucion;
        int.TryParse(Txt_Tiempo_Solucion.Text, out Tiempo_Solucion);
        //Cargar datos de negocio y registrar
        Cls_Cat_Ate_Acciones_Negocio Negocio = new Cls_Cat_Ate_Acciones_Negocio();
        Negocio.P_Clave = Txt_Clave.Text.Trim();
        Negocio.P_Nombre = Txt_Nombre.Text.Trim();
        Negocio.P_Tiempo_Estimado_Solucion = Tiempo_Solucion;
        Negocio.P_Descripcion = Txt_Descripcion.Text.Trim();
        Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
        Registros_Actualizados = Negocio.Guardar_Registro();
        return Registros_Actualizados;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Clave_Duplicada
    ///DESCRIPCIÓN: Regresa el valor de la consulta de la capa de negocio que busca claves existentes
    ///PARÁMETROS:
    /// 		1. Negocio: instancia de la clase de negocio con parámetros para la consulta
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private bool Clave_Duplicada()
    {
        bool Duplicada = false;
        Cls_Cat_Ate_Acciones_Negocio Negocio = new Cls_Cat_Ate_Acciones_Negocio();
        Negocio.P_Clave = Txt_Clave.Text.Trim();
        Negocio.P_ID = HF_ID.Value;
        Duplicada = Negocio.Clave_Duplicada();
        return Duplicada;
    }

    //MENSAJES
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mensaje_PopUp
    ///DESCRIPCIÓN: Mostrar una alerta de javascript con el texto recibido como parámetro
    ///PARÁMETROS:
    /// 		1. Texto: cadena de texto con el mensaje a mostrar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mensaje_PopUp(String Texto)
    {
        ScriptManager.RegisterStartupScript(
            this, this.GetType(), "Catalogo", "alert('" + Texto + "');", true);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Informacion_Formulario
    ///DESCRIPCIÓN: Muestra un mensaje en la página o se oculta dependiendo del parámetro recibido
    ///PARÁMETROS:
    /// 		1. Texto: mesanje a cargar en la etiqueta de mensajes
    /// 		2. Visible: variable que determina la visibilidad de la etiqueta y mensaje de información
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Informacion_Formulario(String Texto, bool Visible)
    {
        Lbl_Informacion.Text = Texto;
        Lbl_Informacion.Visible = Visible;
        Img_Informacion.Visible = Visible;
    }
}
