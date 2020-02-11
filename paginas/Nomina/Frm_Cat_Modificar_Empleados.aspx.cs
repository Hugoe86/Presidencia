using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.Adapters;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Roles.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Areas.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Requisitos_Empleados.Negocios;
using AjaxControlToolkit;
using System.IO;
using System.Globalization;
using Presidencia.Programas.Negocios;
using Presidencia.Tipos_Contratos.Negocios;
using Presidencia.Puestos.Negocios;
using Presidencia.Escolaridad.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Turnos.Negocios;
using Presidencia.Zona_Economica.Negocios;
using Presidencia.Tipo_Trabajador.Negocios;
using System.Text.RegularExpressions;
using Presidencia.Vacaciones_Empleado.Negocio;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Utilidades_Nomina;
using Presidencia.Bancos_Nomina.Negocio;
using Presidencia.Catalogo_SAP_Fuente_Financiamiento.Negocio;
using Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio;
using Presidencia.Catalogo_Compras_Partidas.Negocio;
using Presidencia.Indemnizacion.Negocio;
using Presidencia.Nomina_Tipos_Pago.Negocio;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;

public partial class paginas_Nomina_Frm_Cat_Empleados : System.Web.UI.Page
{
    #region (Page Load)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
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
                Session["Activa"] = true;//Variable para mantener la session activa.
                Inicializa_Controles();
                ViewState["SortDirection"] = "ASC";
            }

            Txt_Password_Empleado.Attributes.Add("value", Txt_Password_Empleado.Text);
            Txt_Confirma_Password_Empleado.Attributes.Add("value", Txt_Confirma_Password_Empleado.Text);

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            Btn_Nuevo.Visible = false;
            Btn_Modificar.Visible = true;
            Btn_Eliminar_Mostrar_Popup.Visible = false;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpia_Controles();
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            //Consulta_Empleados();
            Consulta_Requisitos_Empleado();
            Consultar_Roles();
            Consultar_Tipos_Contratos();
            //Consultar_Puestos();
            Consultar_Escolaridad();
            Consultar_Sindicatos();
            Consultar_Turnos();
            Consultar_Zonas();
            Consultar_Tipo_Trabajador();
            Consultar_Tipos_Nomina();
            Consultar_Retenciones_Terceros();
            Consultar_Indemnizaciones();

            Consultar_Dependencias_Busqueda();
            Consultar_Roles_Busqueda();
            Consultar_Escolaridad_Busqueda();
            Consultar_Puestos_Busqueda();
            Consultar_Sindicatos_Busqueda();
            Consultar_Tipo_Trabajador_Busqueda();
            Consultar_Tipos_Contratos_Busqueda();
            Consultar_Tipos_Nomina_Busqueda();
            Consultar_Turnos_Busqueda();
            Consultar_Zonas_Busqueda();
            Consultar_Bancos();

            Consultar_SAP_Unidades_Responsables();

            Consultar_Formas_Pago();
            Consultar_Cuentas_Contables();
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
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            ///Datos Generales del Empleado
            Txt_Empleado_ID.Text = "";
            Txt_No_Empleado.Text = "";
            Cmb_Estatus_Empleado.SelectedIndex = 1;
            Txt_Nombre_Empleado.Text = "";
            Txt_Apellido_Paterno_Empleado.Text = "";
            Txt_Apellido_Materno_Empleado.Text = "";
            Txt_Password_Empleado.Text = "";
            Txt_Confirma_Password_Empleado.Text = "";
            Txt_Password_Empleado.Attributes.Add("value", "");
            Txt_Confirma_Password_Empleado.Attributes.Add("value", "");            
            Txt_Comentarios_Empleado.Text = "";
            Txt_Fecha_Nacimiento_Empleado.Text = "";
            Cmb_Sexo_Empleado.SelectedIndex = 0;
            Txt_RFC_Empleado.Text = "";
            Txt_CURP_Empleado.Text = "";
            Txt_Domicilio_Empleado.Text = "";
            Txt_Colonia_Empleado.Text = "";
            Txt_Codigo_Postal_Empleado.Text = "";
            Txt_Ciudad_Empleado.Text = "";
            Txt_Estado_Empleado.Text = "";
            Txt_Telefono_Casa_Empleado.Text = "";
            Txt_Celular_Empleado.Text = "";
            Txt_Nextel_Empleado.Text = "";
            Txt_Telefono_Oficina_Empleado.Text = "";
            Txt_Extension_Empleado.Text = "";
            Txt_Fax_Empleado.Text = "";
            Txt_Correo_Electronico_Empleado.Text = "";
            if (Cmb_Roles_Empleado.Items.Count > 0) Cmb_Roles_Empleado.SelectedIndex = 0;
            if (Cmb_SAP_Unidad_Responsable.Items.Count > 0) Cmb_SAP_Unidad_Responsable.SelectedIndex = 0;
            Cmb_Areas_Empleado.DataBind();            
            ///Datos Presidencia
            Cmb_Areas_Empleado.SelectedIndex = -1;
            Cmb_Tipo_Contrato.SelectedIndex = -1;
            Cmb_Puestos.SelectedIndex = -1;
            Cmb_Escolaridad.SelectedIndex = -1;
            Cmb_Sindicato.SelectedIndex = -1;
            Cmb_Turno.SelectedIndex = -1;
            Cmb_Zona.SelectedIndex = -1;
            Cmb_Tipo_Trabajador.SelectedIndex = -1;
            Cmb_Terceros.SelectedIndex = -1;
            ///Recursos Humanos
            Cmb_Tipo_Nomina.SelectedIndex = -1;
            Txt_No_IMSS.Text = "";
            Cmb_Forma_Pago.SelectedIndex = -1;
            Txt_Cuenta_Bancaria.Text = "";
            Txt_Fecha_Inicio.Text = "";
            Txt_Fecha_Termino.Text = "";
            Cmb_Tipo_Finiquito.SelectedIndex = -1;
            Txt_Fecha_Baja.Text = "";
            Txt_Salario_Diario.Text = "";
            Txt_Salario_Diario_Integrado.Text = "";
            Txt_No_Licencia.Text = "";
            Txt_Fecha_Vencimiento_Licencia.Text = "";
            Grid_Tipo_Nomina_Percepciones.DataSource = new DataTable();
            Grid_Tipo_Nomina_Percepciones.DataBind();
            Grid_Tipo_Nomina_Deducciones.DataSource = new DataTable();
            Grid_Tipo_Nomina_Deducciones.DataBind();
            Cmb_Bancos.SelectedIndex = -1;
            Cmb_Reloj_Checador.SelectedIndex = -1;
            Txt_No_Tarjeta.Text = String.Empty;
            Txt_No_Seguro_Poliza_Empleado.Text = "";
            Txt_Beneficiario_Empleado.Text = "";
            ///Dias Trabaja
            Chk_Lunes.Checked = false;
            Chk_Martes.Checked = false;
            Chk_Miercoles.Checked = false;
            Chk_Jueves.Checked = false;
            Chk_Viernes.Checked = false;
            Chk_Sabado.Checked = false;
            Chk_Domingo.Checked = false;

            //SAP Código Programático.
            Cmb_SAP_Fuente_Financiamiento.SelectedIndex = -1;
            Cmb_SAP_Area_Funcional.SelectedIndex = -1;
            Cmb_SAP_Programas.SelectedIndex = -1;
            Cmb_SAP_Unidad_Responsable.SelectedIndex = -1;
            Cmb_SAP_Partida.SelectedIndex = -1;

            Txt_SAP_Fuente_Financiamiento.Text = String.Empty;
            Txt_SAP_Area_Responsable.Text = String.Empty;
            Txt_SAP_Programa.Text = String.Empty;
            Txt_SAP_Unidad_Responsable.Text = String.Empty;
            Txt_SAP_Partida.Text = String.Empty;

            Chk_Aplica_Esquema_ISSEG.Checked = false;

            Cmb_Cuentas_Contables.SelectedIndex = -1;
            Cmb_Busqueda_Estatus.SelectedIndex = 1;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
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
                    Cmb_Estatus_Empleado.Enabled = Habilitado;
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Eliminar_Mostrar_Popup.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Btn_Subir_Foto.Enabled = false;
                    Img_Foto_Empleado.ImageUrl = "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG";
                    Img_Foto_Empleado.DataBind();
                    Txt_Ruta_Foto.Value = "";
                    Btn_Actualizar_Salario.Enabled = false;
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Eliminar_Mostrar_Popup.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    //Cmb_Estatus_Empleado.Enabled = false;
                    Btn_Subir_Foto.Enabled = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                    Img_Foto_Empleado.ImageUrl = "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG";
                    Img_Foto_Empleado.DataBind();
                    Txt_Ruta_Foto.Value = "";
                    Cmb_Estatus_Empleado.SelectedIndex = 1;
                    Btn_Actualizar_Salario.Enabled = false;
                    break;
                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Cmb_Estatus_Empleado.Enabled = true;
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Eliminar_Mostrar_Popup.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Subir_Foto.Enabled = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Actualizar_Salario.Enabled = true;
                    break;
            }
            ///Datos Personales
            Txt_No_Empleado.Enabled = false;
            Txt_Nombre_Empleado.Enabled = Habilitado;
            Txt_Apellido_Paterno_Empleado.Enabled = Habilitado;
            Txt_Apellido_Materno_Empleado.Enabled = Habilitado;
            Txt_Password_Empleado.Enabled = Habilitado;
            Txt_Confirma_Password_Empleado.Enabled = Habilitado;
            Txt_Comentarios_Empleado.Enabled = Habilitado;
            Txt_Fecha_Nacimiento_Empleado.Enabled = Habilitado;
            Cmb_Sexo_Empleado.Enabled = Habilitado;
            Txt_RFC_Empleado.Enabled = Habilitado;
            Txt_CURP_Empleado.Enabled = Habilitado;
            Txt_Domicilio_Empleado.Enabled = Habilitado;
            Txt_Colonia_Empleado.Enabled = Habilitado;
            Txt_Codigo_Postal_Empleado.Enabled = Habilitado;
            Txt_Ciudad_Empleado.Enabled = Habilitado;
            Txt_Estado_Empleado.Enabled = Habilitado;
            Txt_Telefono_Casa_Empleado.Enabled = Habilitado;
            Txt_Celular_Empleado.Enabled = Habilitado;
            Txt_Nextel_Empleado.Enabled = Habilitado;
            Txt_Telefono_Oficina_Empleado.Enabled = Habilitado;
            Txt_Extension_Empleado.Enabled = Habilitado;
            Txt_Fax_Empleado.Enabled = Habilitado;
            Txt_Correo_Electronico_Empleado.Enabled = Habilitado;
            Cmb_Roles_Empleado.Enabled = Habilitado;
            Btn_Busqueda_Empleados.Enabled = !Habilitado;
            Grid_Empleados.Enabled = !Habilitado;
            Grid_Documentos_Empleado.Enabled = Habilitado;
            Async_Foto_Empleado.Enabled = Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Cmb_Estatus_Empleado.Enabled = Habilitado;
            ///Datos Presidencia
            Cmb_Areas_Empleado.Enabled = false;
            Cmb_Tipo_Contrato.Enabled = Habilitado;
            Cmb_Puestos.Enabled = Habilitado;
            Cmb_Escolaridad.Enabled = Habilitado;
            Cmb_Sindicato.Enabled = Habilitado;
            Cmb_Turno.Enabled = Habilitado;
            Cmb_Zona.Enabled = Habilitado;
            Cmb_Tipo_Trabajador.Enabled = Habilitado;
            Cmb_Terceros.Enabled = Habilitado;
            ///Recursos Humanos
            Cmb_Tipo_Nomina.Enabled = Habilitado;
            Txt_No_IMSS.Enabled = Habilitado;
            Cmb_Forma_Pago.Enabled = Habilitado;
            Txt_Cuenta_Bancaria.Enabled = Habilitado;
            Txt_Fecha_Inicio.Enabled = Habilitado;
            Txt_Fecha_Termino.Enabled = Habilitado;
            Cmb_Tipo_Finiquito.Enabled = Habilitado;
            Txt_Fecha_Baja.Enabled = Habilitado;
            Txt_Salario_Diario.Enabled = Habilitado;
            Txt_Salario_Diario_Integrado.Enabled = Habilitado;
            Txt_No_Licencia.Enabled = Habilitado;
            Txt_Fecha_Vencimiento_Licencia.Enabled = Habilitado;
            Cmb_Bancos.Enabled = Habilitado;
            Cmb_Reloj_Checador.Enabled = Habilitado;
            Txt_No_Tarjeta.Enabled = Habilitado;
            Txt_No_Seguro_Poliza_Empleado.Enabled = Habilitado;
            Txt_Beneficiario_Empleado.Enabled=Habilitado;
            ///Dias Laborales
            Chk_Lunes.Enabled = Habilitado;
            Chk_Martes.Enabled = Habilitado;
            Chk_Miercoles.Enabled = Habilitado;
            Chk_Jueves.Enabled = Habilitado;
            Chk_Viernes.Enabled = Habilitado;
            Chk_Sabado.Enabled = Habilitado;
            Chk_Domingo.Enabled = Habilitado;

            Grid_Tipo_Nomina_Percepciones.Enabled = Habilitado;
            Grid_Tipo_Nomina_Deducciones.Enabled = Habilitado;

            Cmb_Percepciones_All.Enabled = Habilitado;
            Btn_Agregar_Percepcion.Enabled = Habilitado;

            Cmb_Deducciones_All.Enabled = Habilitado;
            Btn_Agregar_Deduccion.Enabled = Habilitado;

            //Solo es posible consultar
            Btn_Nuevo.Visible = false;
            Btn_Modificar.Visible = true;
            Btn_Eliminar_Mostrar_Popup.Visible = false;

            Tab_Percepciones_Tipo_Nomina.Visible = false;
            Tab_Deducciones_Tipo_Nomina.Visible = false;

            ////SAP Código Programático.
            Cmb_SAP_Fuente_Financiamiento.Enabled = false;
            Cmb_SAP_Area_Funcional.Enabled = false;
            Cmb_SAP_Programas.Enabled = false;
            Cmb_SAP_Unidad_Responsable.Enabled = Habilitado;
            Cmb_SAP_Partida.Enabled = false;

            Txt_SAP_Fuente_Financiamiento.Enabled = false;
            Txt_SAP_Area_Responsable.Enabled = false;
            Txt_SAP_Programa.Enabled = false;
            Txt_SAP_Unidad_Responsable.Enabled = false;
            Txt_SAP_Partida.Enabled = false;

            Chk_Aplica_Esquema_ISSEG.Enabled = Habilitado;

            Cmb_Cuentas_Contables.Enabled = Habilitado;
            //  para desabilitar el combo de la busqueda del estatus
            //  y de esta manera cargarlo con ACTIVO
            //Cmb_Busqueda_Estatus.Enabled = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Tooltip_Combos
    /// DESCRIPCION : Agregar tooltip al combo que es pasado como parametro.
    /// PARAMETROS  : _DropDownList es el combo al cual se le agregara el tooltip
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Tooltip_Combos(DropDownList Cmb_Combo)
    {
        for (int i = 0; i <= Cmb_Combo.Items.Count - 1; i++)
        {
            Cmb_Combo.Items[i].Attributes.Add("Title", Cmb_Combo.Items[i].Text);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Guardar_Documentos
    /// DESCRIPCION : Guarda los Documentos que se han anexado como requisitos
    /// que se han pedido al empleado.        
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private String Guardar_Documentos()
    {
        DataTable Dt_Documentos_Anexos = new DataTable(); //Tabla para el llenado del grid
        DataRow Renglon = null; //Renglon para el llenado de la tabla
        Int32 Contador_Documentos;
        AsyncFileUpload Asy_FileUpload;
        String Ruta_Servidor_Empleados = "";
        String Nombre_Dir_Empleado = "";
        Cls_Cat_Empleados_Negocios Cat_Empleados = new Cls_Cat_Empleados_Negocios();

        try
        {
            //Se obtiene la direccion en donde se va a guardar el archivo. Ej. C:/Dir_Servidor/..
            Ruta_Servidor_Empleados = Server.MapPath("Empleados");

            //Crear el Directorio Proveedores. Ej. Proveedores
            if (!Directory.Exists(Ruta_Servidor_Empleados))
            {
                System.IO.Directory.CreateDirectory(Ruta_Servidor_Empleados);
            }

            //Se establece el nombre del directorio Ej. Empleado_00001
            Nombre_Dir_Empleado = "Empleado_" + (Txt_Empleado_ID.Text.Trim().Equals("") ? Cat_Empleados.Consulta_Id_Empleado() : Txt_Empleado_ID.Text.Trim());

            if (Directory.Exists(Ruta_Servidor_Empleados))
            {
                //Se recorren las filas del grid. 
                for (Contador_Documentos = 0; Contador_Documentos < Grid_Documentos_Empleado.Rows.Count; Contador_Documentos++)
                {

                    //Obtenemos el Ctlr AsyncFileUpload del GridView.
                    Asy_FileUpload = (AsyncFileUpload)Grid_Documentos_Empleado.Rows[Contador_Documentos].Cells[2].FindControl("Async_Requisito_Empleado");

                    //Validamos que el nombre del archivo no se encuentre vacio.
                    if (!Asy_FileUpload.FileName.Equals("") && ((CheckBox)Grid_Documentos_Empleado.Rows[Contador_Documentos].Cells[3].FindControl("Chk_Requisito_Empleado")).Checked)
                    {
                        //Valida que no exista el directorio, si no existe lo crea [172.16.0.103/Web/Project/Empleado/Empleado_00001]
                        DirectoryInfo Ruta_Completa_Dir_Empleado;
                        if (!Directory.Exists(Ruta_Servidor_Empleados + Nombre_Dir_Empleado))
                        {
                            Ruta_Completa_Dir_Empleado = Directory.CreateDirectory(Ruta_Servidor_Empleados + @"\" + Nombre_Dir_Empleado);
                        }

                        //Se asigna el directorio en donde se va a guardar los documentos. Ej. [Empleado/]
                        String Ruta_Dir_Empleado = Nombre_Dir_Empleado + @"\";

                        //Se establece la ruta completa del archivo . Ej. [172.16.0.103/Web/Project/Empleado/Empleado_00001/File1.txt]
                        String Ruta_Completa_Archivo_A_Cargar = Ruta_Servidor_Empleados + @"\" + Ruta_Dir_Empleado +
                            Grid_Documentos_Empleado.Rows[Contador_Documentos].Cells[1].Text + "_" + Nombre_Dir_Empleado +
                            "." + Asy_FileUpload.FileName.Split(new Char[] { '.' })[1];

                        //Se valida que el Ctlr AsyncFileUpload. Contenga el archivo a guardar.
                        if (Asy_FileUpload.HasFile)
                        {
                            //Se guarda el archivo. En la ruta indicada. Ej.  [172.16.0.103/Web/Project/Empleado/Empleado_00001/File1.txt]
                            Asy_FileUpload.SaveAs(Ruta_Completa_Archivo_A_Cargar);
                        }

                        //Agregar datos de la fila a una tabla asignada a una variable de Ssesion
                        //Verificar si la variable de sesion tiene datos
                        if (Session["Dt_Requisitos_Empleado"] != null)
                        {
                            Dt_Documentos_Anexos = (DataTable)Session["Dt_Requisitos_Empleado"];
                        }
                        else
                        {
                            //Crear la tabla de los documentos
                            Dt_Documentos_Anexos.Columns.Add(Cat_Nom_Requisitos_Empleados.Campo_Requisito_ID, Type.GetType("System.String"));
                            Dt_Documentos_Anexos.Columns.Add(Cat_Nom_Requisitos_Empleados.Campo_Nombre, Type.GetType("System.String"));
                            Dt_Documentos_Anexos.Columns.Add(Ope_Nom_Requisitos_Empleado.Campo_Ruta_Documento, Type.GetType("System.String"));
                            Dt_Documentos_Anexos.Columns.Add(Ope_Nom_Requisitos_Empleado.Campo_Entregado, Type.GetType("System.String"));
                        }
                        //Crear renglon y agregarlo a la tabla y colocarlo en la variable de sesion
                        Renglon = Dt_Documentos_Anexos.NewRow();
                        Renglon[Cat_Nom_Requisitos_Empleados.Campo_Requisito_ID] = Grid_Documentos_Empleado.Rows[Contador_Documentos].Cells[0].Text;
                        Renglon[Cat_Nom_Requisitos_Empleados.Campo_Nombre] = Grid_Documentos_Empleado.Rows[Contador_Documentos].Cells[1].ToolTip;
                        Renglon[Ope_Nom_Requisitos_Empleado.Campo_Ruta_Documento] = Ruta_Completa_Archivo_A_Cargar;
                        Renglon[Ope_Nom_Requisitos_Empleado.Campo_Entregado] = ((CheckBox)Grid_Documentos_Empleado.Rows[Contador_Documentos].Cells[3].FindControl("Chk_Requisito_Empleado")).Checked ? "S" : "N";
                        Dt_Documentos_Anexos.Rows.Add(Renglon);
                        Session["Dt_Requisitos_Empleado"] = Dt_Documentos_Anexos;

                    }
                    else if (Grid_Documentos_Empleado.Rows[Contador_Documentos].Cells[4].Text.Equals("N") && ((CheckBox)Grid_Documentos_Empleado.Rows[Contador_Documentos].Cells[3].FindControl("Chk_Requisito_Empleado")).Checked)
                    {
                        String Empleado_ID = "";
                        String Requisito_ID = "";

                        if (!Txt_Empleado_ID.Text.Trim().Equals(""))
                        {
                            Empleado_ID = Txt_Empleado_ID.Text.Trim();
                            Requisito_ID = Grid_Documentos_Empleado.Rows[Contador_Documentos].Cells[0].Text;
                        }

                        if (Cat_Empleados.Consultar_Requisitos_Empleados_Entregados_Por_ID(Empleado_ID, Requisito_ID).Rows.Count == 0)
                        {
                            //Agregar datos de la fila a una tabla asignada a una variable de Ssesion
                            //Verificar si la variable de sesion tiene datos
                            if (Session["Dt_Requisitos_Empleado"] != null)
                            {
                                Dt_Documentos_Anexos = (DataTable)Session["Dt_Requisitos_Empleado"];
                            }
                            else
                            {
                                //Crear la tabla de los documentos
                                Dt_Documentos_Anexos.Columns.Add(Cat_Nom_Requisitos_Empleados.Campo_Requisito_ID, Type.GetType("System.String"));
                                Dt_Documentos_Anexos.Columns.Add(Cat_Nom_Requisitos_Empleados.Campo_Nombre, Type.GetType("System.String"));
                                Dt_Documentos_Anexos.Columns.Add(Ope_Nom_Requisitos_Empleado.Campo_Ruta_Documento, Type.GetType("System.String"));
                                Dt_Documentos_Anexos.Columns.Add(Ope_Nom_Requisitos_Empleado.Campo_Entregado, Type.GetType("System.String"));
                            }
                            //Crear renglon y agregarlo a la tabla y colocarlo en la variable de sesion
                            Renglon = Dt_Documentos_Anexos.NewRow();
                            Renglon[Cat_Nom_Requisitos_Empleados.Campo_Requisito_ID] = Grid_Documentos_Empleado.Rows[Contador_Documentos].Cells[0].Text;
                            Renglon[Cat_Nom_Requisitos_Empleados.Campo_Nombre] = Grid_Documentos_Empleado.Rows[Contador_Documentos].Cells[1].ToolTip;
                            Renglon[Ope_Nom_Requisitos_Empleado.Campo_Ruta_Documento] = "";
                            Renglon[Ope_Nom_Requisitos_Empleado.Campo_Entregado] = ((CheckBox)Grid_Documentos_Empleado.Rows[Contador_Documentos].Cells[3].FindControl("Chk_Requisito_Empleado")).Checked ? "S" : "N";
                            Dt_Documentos_Anexos.Rows.Add(Renglon);
                            Session["Dt_Requisitos_Empleado"] = Dt_Documentos_Anexos;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('" + ex.Message + "');", true);
        }
        return (Ruta_Servidor_Empleados + @"\" + Nombre_Dir_Empleado);
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Remover_Sesiones_Control_Carga_Archivos
    /// DESCRIPCION : Remueve la sesion del Ctlr AsyncFileUpload que mantiene al archivo
    /// en memoria.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Remover_Sesiones_Control_Carga_Archivos(String Client_ID)
    {
        HttpContext currentContext;
        if (HttpContext.Current != null && HttpContext.Current.Session != null)
        {
            currentContext = HttpContext.Current;
        }
        else
        {
            currentContext = null;
        }

        if (currentContext != null)
        {
            foreach (String key in currentContext.Session.Keys)
            {
                if (key.Contains(Client_ID))
                {
                    currentContext.Session.Remove(key);
                    break;
                }
            }
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Recorre_Grid_Envia_Limpiar_Control_Subir_Archivos
    /// DESCRIPCION : Recorre el GridView que es pasado como parametro y obtiene el ID
    /// del control AsyncFileUpload. Para posteriormente remover la sesion que mantiene al
    /// archivo en memoria.
    /// PARAMETROS: _GridView
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Recorre_Grid_Envia_Limpiar_Control_Subir_Archivos(GridView Grid_Generico)
    {
        for (int cont = 0; cont < Grid_Generico.Rows.Count; cont++)
        {
            String Client_ID = ((AsyncFileUpload)Grid_Generico.Rows[cont].Cells[2].FindControl("Async_Requisito_Empleado")).ClientID;
            Remover_Sesiones_Control_Carga_Archivos(Client_ID);
        }
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Subir_Foto_Click
    /// DESCRIPCIÓN: Carga la Foto del empleado a dar de alta
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 30/Octubre/2010
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    protected void Subir_Foto_Click(object sender, EventArgs e)
    {
        String Ruta_Servidor_Empleados = "";
        String Nombre_Dir_Empleado = "";
        AsyncFileUpload Asy_FileUpload;
        Cls_Cat_Empleados_Negocios Cat_Empleados = new Cls_Cat_Empleados_Negocios();

        try
        {
            if (Async_Foto_Empleado.HasFile)
            {
                //Se obtiene la direccion en donde se va a guardar el archivo. Ej. C:/Dir_Servidor/..
                Ruta_Servidor_Empleados = Server.MapPath("Foto_Empleados");

                //Crear el Directorio Proveedores. Ej. Proveedores
                if (!Directory.Exists(Ruta_Servidor_Empleados))
                {
                    System.IO.Directory.CreateDirectory(Ruta_Servidor_Empleados);
                }

                //Se establece el nombre del directorio Ej. Empleado_00001
                Nombre_Dir_Empleado = "Empleado_" + (Txt_Empleado_ID.Text.Trim().Equals("") ? Cat_Empleados.Consulta_Id_Empleado() : Txt_Empleado_ID.Text.Trim());

                if (Directory.Exists(Ruta_Servidor_Empleados))
                {
                    //Obtenemos el Ctlr AsyncFileUpload del GridView.
                    Asy_FileUpload = Async_Foto_Empleado;

                    //Validamos que el nombre del archivo no se encuentre vacio.
                    if (!Asy_FileUpload.FileName.Equals(""))
                    {
                        //Valida que no exista el directorio, si no existe lo crea [172.16.0.103/Web/Project/Empleado/Empleado_00001]
                        DirectoryInfo Ruta_Completa_Dir_Empleado;
                        if (!Directory.Exists(Ruta_Servidor_Empleados + Nombre_Dir_Empleado))
                        {
                            Ruta_Completa_Dir_Empleado = Directory.CreateDirectory(Ruta_Servidor_Empleados + @"\" + Nombre_Dir_Empleado);
                        }

                        //Se asigna el directorio en donde se va a guardar los documentos. Ej. [Empleado/]
                        String Ruta_Dir_Empleado = Nombre_Dir_Empleado + @"\";

                        //Se establece la ruta completa del archivo . Ej. [172.16.0.103/Web/Project/Empleado/Empleado_00001/File1.txt]
                        String Ruta_Completa_Archivo_A_Cargar = Ruta_Servidor_Empleados + @"\" + Ruta_Dir_Empleado +
                            Nombre_Dir_Empleado + "." + Asy_FileUpload.FileName.Split(new Char[] { '.' })[1];

                        //Se valida que el Ctlr AsyncFileUpload. Contenga el archivo a guardar.
                        if (Asy_FileUpload.HasFile)
                        {
                            DirectoryInfo directory = new DirectoryInfo((Ruta_Servidor_Empleados + @"\" + Ruta_Dir_Empleado));
                            foreach (FileInfo fi in directory.GetFiles())
                            {
                                File.Delete((Ruta_Servidor_Empleados + @"\" + Ruta_Dir_Empleado) + @"\" + fi.Name);
                            }

                            //Se guarda el archivo. En la ruta indicada. Ej.  [172.16.0.103/Web/Project/Empleado/Empleado_00001/File1.txt]
                            Asy_FileUpload.SaveAs(Ruta_Completa_Archivo_A_Cargar);
                            //Guardamos en el campo hidden la ruta de la foto del empelado.
                            Txt_Ruta_Foto.Value = @HttpUtility.HtmlDecode("Foto_Empleados" + @"\" + Ruta_Dir_Empleado + Nombre_Dir_Empleado + "." + Asy_FileUpload.FileName.Split(new Char[] { '.' })[1]);
                            Img_Foto_Empleado.ImageUrl = @HttpUtility.HtmlDecode("Foto_Empleados" + @"\" + Ruta_Dir_Empleado + Nombre_Dir_Empleado + "." + Asy_FileUpload.FileName.Split(new Char[] { '.' })[1]);
                            Img_Foto_Empleado.DataBind();
                            Remover_Sesiones_Control_Carga_Archivos(Async_Foto_Empleado.ClientID);
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('No se ha seleccionado  ninguna foto a guardar');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al cargar la foto del empleado. ERROR: [" + Ex.Message + "]");
        }
    }
    /// ********************************************************************************
    /// Nombre: Crear_Tabla_Mostrar_Errores_Pagina
    /// Descripción: Crea la tabla que almacenara que datos son requeridos 
    /// por el sistema
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private String Crear_Tabla_Mostrar_Errores_Pagina( String Errores ) {
        String Tabla_Inicio = "<table style='width:100%px;font-size:10px;color:red;text-align:left;'>";
        String Tabla_Cierra = "</table>";
        String Fila_Inicia = "<tr>";
        String Fila_Cierra = "</tr>";
        String Celda_Inicia = "<td style='width:25%;text-align:left;vertical-align:top;font-size:10px;' " +
                                "onmouseover=this.style.background='#DFE8F6';this.style.color='#000000'"+
                                " onmouseout=this.style.background='#ffffff';this.style.color='red'>";
        String Celda_Cierra = "</td>";
        char[] Separador = {'+'};
        String[] _Errores_Temp = Errores.Replace("<br>", "").Split(Separador);
        String[] _Errores = new String[(_Errores_Temp.Length - 1)];
        String Tabla;
        String Filas = "";
        String Celdas = "";
        int Contador_Celdas = 1;
        for (int i = 0; i < _Errores.Length; i++) _Errores[i] = _Errores_Temp[i+1];

        Tabla = Tabla_Inicio;
        for (int i = 0; i < _Errores.Length; i++)
        {
            if (Contador_Celdas == 5 )
            {
                Filas += Fila_Inicia;
                Filas += Celdas;
                Filas += Fila_Cierra;
                Celdas = "";
                Contador_Celdas = 0;
                i = i - 1;
            }
            else
            {
                Celdas += Celda_Inicia;
                Celdas += "<b style='font-size:12px;'>+</b>" + _Errores[i];
                Celdas += Celda_Cierra;                
            }
            Contador_Celdas = Contador_Celdas + 1;            
        }
        if (_Errores.Length < 5 || Contador_Celdas > 0) {
            Filas += Fila_Inicia;
            Filas += Celdas;
            Filas += Fila_Cierra;
        }
        Tabla += Filas;
        Tabla += Tabla_Cierra;
        return Tabla;
    }
    /// ******************************************************************************************************
    /// NOMBRE: Cargar_Codigo_Programatico
    /// 
    /// DESCRIPCIÓN: Recibe como parámetro una cadena que almacena el código programático.
    /// 
    /// PARÁMETROS: Codigo_Programatico.- Almacena el código programatico del empleado, donde, las claves
    ///             se encuentran separadas por guiones.
    /// 
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 2/Mayo/2011 10:31 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ******************************************************************************************************
    protected void Cargar_Codigo_Programatico(String Codigo_Programatico)
    {
        String[] Arreglo_Codigo_Programatico;//Variable de tipo estrctura que almacenara la información del código programático.

        try
        {
            if (!String.IsNullOrEmpty(Codigo_Programatico))
            {
                Arreglo_Codigo_Programatico = Codigo_Programatico.Split(new char[] { '-' });

                if (Arreglo_Codigo_Programatico.Length > 0)
                {
                    if (!String.IsNullOrEmpty(Arreglo_Codigo_Programatico[0]))
                        Txt_SAP_Fuente_Financiamiento.Text = Arreglo_Codigo_Programatico[0].Trim();

                    if (!String.IsNullOrEmpty(Arreglo_Codigo_Programatico[1]))
                        Txt_SAP_Area_Responsable.Text = Arreglo_Codigo_Programatico[1].Trim();

                    if (!String.IsNullOrEmpty(Arreglo_Codigo_Programatico[2]))
                        Txt_SAP_Programa.Text = Arreglo_Codigo_Programatico[2].Trim();

                    if (!String.IsNullOrEmpty(Arreglo_Codigo_Programatico[3]))
                        Txt_SAP_Unidad_Responsable.Text = Arreglo_Codigo_Programatico[3].Trim();

                    if (!String.IsNullOrEmpty(Arreglo_Codigo_Programatico[4]))
                        Txt_SAP_Partida.Text = Arreglo_Codigo_Programatico[4].Trim();
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el código programático en los controles de la página. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
    
    #region (Operacion [Alta - Modificar - Eliminar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Empleado
    /// DESCRIPCION : Da de Alta el Empleado con los datos proporcionados por el usuario
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :Juan Alberto Hernandez Negrete
    /// FECHA_MODIFICO    :3/Noviembre/2010
    /// CAUSA_MODIFICACION: Completar el Catalogo
    ///*******************************************************************************
    private void Alta_Empleado()
    {
        Cls_Cat_Empleados_Negocios Rs_Alta_Cat_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            ///Datos Generales del Empleado
            Rs_Alta_Cat_Empleados.P_Ruta_Foto = Txt_Ruta_Foto.Value;
            Rs_Alta_Cat_Empleados.P_No_Empleado = Convert.ToString(Txt_No_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Estatus = Cmb_Estatus_Empleado.SelectedValue;
            Rs_Alta_Cat_Empleados.P_Nombre = Convert.ToString(Txt_Nombre_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Apellido_Paterno = Convert.ToString(Txt_Apellido_Paterno_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Apelldo_Materno = Convert.ToString(Txt_Apellido_Materno_Empleado.Text);
            if (Cmb_Roles_Empleado.SelectedIndex > 0) Rs_Alta_Cat_Empleados.P_Rol_ID = Cmb_Roles_Empleado.SelectedValue;
            if (Txt_Password_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_Password = Convert.ToString(Txt_Password_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Comentarios = Convert.ToString(Txt_Comentarios_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Confronto = Txt_Empleado_Confronto.Text.Trim();

            ///Datos Personales del Empleado
            Rs_Alta_Cat_Empleados.P_Fecha_Nacimiento = Convert.ToDateTime(Txt_Fecha_Nacimiento_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Sexo = Cmb_Sexo_Empleado.SelectedValue;
            if (Txt_RFC_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_RFC = Convert.ToString(Txt_RFC_Empleado.Text);
            if (Txt_CURP_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_CURP = Convert.ToString(Txt_CURP_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Calle = Convert.ToString(Txt_Domicilio_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Colonia = Convert.ToString(Txt_Colonia_Empleado.Text);
            if (Txt_Codigo_Postal_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_Codigo_Postal = Convert.ToInt32(Txt_Codigo_Postal_Empleado.Text.ToString());
            Rs_Alta_Cat_Empleados.P_Ciudad = Convert.ToString(Txt_Ciudad_Empleado.Text);
            Rs_Alta_Cat_Empleados.P_Estado = Convert.ToString(Txt_Estado_Empleado.Text);
            if (Txt_Telefono_Casa_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_Telefono_Casa = Convert.ToString(Txt_Telefono_Casa_Empleado.Text);
            if (Txt_Celular_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_Celular = Convert.ToString(Txt_Celular_Empleado.Text);
            if (Txt_Nextel_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_Nextel = Convert.ToString(Txt_Nextel_Empleado.Text);
            if (Txt_Telefono_Oficina_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_Telefono_Oficina = Convert.ToString(Txt_Telefono_Oficina_Empleado.Text);
            if (Txt_Extension_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_Extension = Convert.ToString(Txt_Extension_Empleado.Text);
            if (Txt_Fax_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_Fax = Convert.ToString(Txt_Fax_Empleado.Text);
            if (Txt_Correo_Electronico_Empleado.Text != "") Rs_Alta_Cat_Empleados.P_Correo_Electronico = Convert.ToString(Txt_Correo_Electronico_Empleado.Text);
           
            ///Datos Recursos Humanos
            Rs_Alta_Cat_Empleados.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedIndex > 0 ? Cmb_Tipo_Nomina.SelectedValue.Trim():"";
            Rs_Alta_Cat_Empleados.P_No_IMSS = Txt_No_IMSS.Text;
            Rs_Alta_Cat_Empleados.P_Forma_Pago = Cmb_Forma_Pago.SelectedIndex > 0 ? Cmb_Forma_Pago.SelectedValue.Trim() : "";
            Rs_Alta_Cat_Empleados.P_No_Cuenta_Bancaria = Txt_Cuenta_Bancaria.Text;
            Rs_Alta_Cat_Empleados.P_Fecha_Inicio = Convert.ToDateTime( Txt_Fecha_Inicio.Text);
            if (!string.IsNullOrEmpty(Txt_Fecha_Termino.Text)) Rs_Alta_Cat_Empleados.P_Fecha_Termino_Contrato = Convert.ToDateTime(Txt_Fecha_Termino.Text);
            Rs_Alta_Cat_Empleados.P_Tipo_Finiquito = Cmb_Tipo_Finiquito.SelectedIndex > 0 ? Cmb_Tipo_Finiquito.SelectedValue.Trim() : "";
            if (!string.IsNullOrEmpty(Txt_Fecha_Baja.Text)) Rs_Alta_Cat_Empleados.P_Fecha_Baja = Convert.ToDateTime(Txt_Fecha_Baja.Text);
            if (!string.IsNullOrEmpty(Txt_Salario_Diario.Text)) Rs_Alta_Cat_Empleados.P_Salario_Diario = Convert.ToDouble((Txt_Salario_Diario.Text.Equals("$  _,___,___.__") || Txt_Salario_Diario.Text.Equals("")) ? "0" : Txt_Salario_Diario.Text);
            if (!string.IsNullOrEmpty(Txt_Salario_Diario_Integrado.Text)) Rs_Alta_Cat_Empleados.P_Salario_Diario_Integrado = Convert.ToDouble((Txt_Salario_Diario_Integrado.Text.Equals("$  _,___,___.__") || Txt_Salario_Diario_Integrado.Text.Equals("")) ? "0" : Txt_Salario_Diario_Integrado.Text);
            Rs_Alta_Cat_Empleados.P_No_Licencia = Txt_No_Licencia.Text.Trim();
            Rs_Alta_Cat_Empleados.P_Fecha_Vigencia_Licencia = Convert.ToDateTime(Txt_Fecha_Vencimiento_Licencia.Text.Trim());
            Rs_Alta_Cat_Empleados.P_Banco_ID = Cmb_Bancos.SelectedValue.Trim();
            Rs_Alta_Cat_Empleados.P_Reloj_Checador = Cmb_Reloj_Checador.SelectedItem.Text.Trim();
            Rs_Alta_Cat_Empleados.P_No_Tarjeta = Txt_No_Tarjeta.Text.Trim();
            Rs_Alta_Cat_Empleados.P_No_Seguro_Poliza = Txt_No_Seguro_Poliza_Empleado.Text.Trim();
            Rs_Alta_Cat_Empleados.P_Beneficiario_Seguro = Txt_Beneficiario_Empleado.Text.Trim();

            ///Datos Dias Trabajados
            Rs_Alta_Cat_Empleados.P_Lunes = Chk_Lunes.Checked ? "SI" :"NO";
            Rs_Alta_Cat_Empleados.P_Martes = Chk_Martes.Checked ? "SI" : "NO";
            Rs_Alta_Cat_Empleados.P_Miercoles = Chk_Miercoles.Checked ? "SI" : "NO";
            Rs_Alta_Cat_Empleados.P_Jueves = Chk_Jueves.Checked ? "SI" : "NO";
            Rs_Alta_Cat_Empleados.P_Viernes = Chk_Viernes.Checked ? "SI" : "NO";
            Rs_Alta_Cat_Empleados.P_Sabado = Chk_Sabado.Checked ? "SI" : "NO";
            Rs_Alta_Cat_Empleados.P_Domingo = Chk_Domingo.Checked ? "SI" : "NO";

            ///Datos Presidencia
            //Rs_Alta_Cat_Empleados.P_Area_ID = Cmb_Areas_Empleado.SelectedValue;
            Rs_Alta_Cat_Empleados.P_Area_ID = "";
            //Rs_Alta_Cat_Empleados.P_Tipo_Contrato_ID = Cmb_Tipo_Contrato.SelectedIndex > 0 ? Cmb_Tipo_Contrato.SelectedValue :"";
            Rs_Alta_Cat_Empleados.P_Tipo_Contrato_ID = "";
            Rs_Alta_Cat_Empleados.P_Puesto_ID = Cmb_Puestos.SelectedIndex > 0 ? Cmb_Puestos.SelectedValue : "";
            Rs_Alta_Cat_Empleados.P_Escolaridad_ID = Cmb_Escolaridad.SelectedIndex > 0 ? Cmb_Escolaridad.SelectedValue : "";
            Rs_Alta_Cat_Empleados.P_Sindicado_ID = Cmb_Sindicato.SelectedIndex > 0 ? Cmb_Sindicato.SelectedValue : "";
            Rs_Alta_Cat_Empleados.P_Turno_ID = Cmb_Turno.SelectedIndex > 0 ? Cmb_Turno.SelectedValue : "";
            Rs_Alta_Cat_Empleados.P_Zona_ID = Cmb_Zona.SelectedIndex > 0 ? Cmb_Zona.SelectedValue : "";
            //Rs_Alta_Cat_Empleados.P_Tipo_Trabajador_ID = Cmb_Tipo_Trabajador.SelectedIndex > 0 ? Cmb_Tipo_Trabajador.SelectedValue : "";
            Rs_Alta_Cat_Empleados.P_Tipo_Trabajador_ID = "";
            Rs_Alta_Cat_Empleados.P_Terceros_ID = Cmb_Terceros.SelectedIndex > 0 ? Cmb_Terceros.SelectedValue : "";
            //if (Cmb_Cuentas_Contables.SelectedIndex > 0) { Rs_Alta_Cat_Empleados.P_Cuenta_Contable_ID = Cmb_Cuentas_Contables.SelectedValue.Trim(); }
            Rs_Alta_Cat_Empleados.P_Cuenta_Contable_ID = "";

            ///Usuario creo
            Rs_Alta_Cat_Empleados.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            ///Crea el DataTable de los Documentos
            Guardar_Documentos();
            if (Session["Dt_Requisitos_Empleado"] != null)
            {
                Rs_Alta_Cat_Empleados.P_Documentos_Anexos_Empleado = (DataTable)Session["Dt_Requisitos_Empleado"];
                Session.Remove("Dt_Requisitos_Empleado");
            }
            Recorre_Grid_Envia_Limpiar_Control_Subir_Archivos(Grid_Documentos_Empleado);

            ///Bitacora de movimientos
            Rs_Alta_Cat_Empleados.P_Tipo_Movimiento = "ALTA";
            Rs_Alta_Cat_Empleados.P_Sueldo_Actual = (Calculo_Salario_Diario() * (new Cls_Utlidades_Nomina(DateTime.Now).P_Dias_Promedio_Mes_Anio));
            ///Percepciones Deducciones Tipo Nomina
            Rs_Alta_Cat_Empleados.P_Dt_Tipo_Nomina_Lista_Percepciones = Crear_DataTable_Percepciones_Deducciones(Grid_Tipo_Nomina_Percepciones, "Txt_Cantidad_Percepcion");
            Rs_Alta_Cat_Empleados.P_Dt_Tipo_Nomina_Lista_Deducciones = Crear_DataTable_Percepciones_Deducciones(Grid_Tipo_Nomina_Deducciones, "Txt_Cantidad_Deduccion");
            ///Percepciones Deducciones Sindicato
            Rs_Alta_Cat_Empleados.P_Dt_Sindicato_Lista_Percepciones = Obtener_Percepciones_Deducciones_Sindicato(Cmb_Sindicato.SelectedValue.Trim(), "PERCEPCION");
            Rs_Alta_Cat_Empleados.P_Dt_Sindicato_Lista_Deducciones = Obtener_Percepciones_Deducciones_Sindicato(Cmb_Sindicato.SelectedValue.Trim(), "DEDUCCION");

            //------------------------------------  SAP Código Programático  ----------------------------------------------
            Rs_Alta_Cat_Empleados.P_Dependencia_ID = Cmb_SAP_Unidad_Responsable.SelectedValue.Trim();
            Rs_Alta_Cat_Empleados.P_SAP_Fuente_Financiamiento = Cmb_SAP_Fuente_Financiamiento.SelectedValue.Trim();
            Rs_Alta_Cat_Empleados.P_SAP_Area_Responsable_ID = Cmb_SAP_Area_Funcional.SelectedValue.Trim();
            Rs_Alta_Cat_Empleados.P_SAP_Programa_ID = Cmb_SAP_Programas.SelectedValue.Trim();
            Rs_Alta_Cat_Empleados.P_SAP_Partida_ID = Cmb_SAP_Partida.SelectedValue.Trim();
            Rs_Alta_Cat_Empleados.P_SAP_Codigo_Programatico = Txt_SAP_Fuente_Financiamiento.Text.Trim() + "-" +
                Txt_SAP_Area_Responsable.Text.Trim() + "-" + Txt_SAP_Programa.Text.Trim() + "-" +
                Txt_SAP_Unidad_Responsable.Text.Trim() + "-" + Txt_SAP_Partida.Text.Trim();
            //---------------------------------------------------------------------------------------------------------------

            if (Cmb_Puestos.SelectedItem.Text.Trim().Split(new Char[] { '*' }).Length > 1)
                Rs_Alta_Cat_Empleados.P_Clave = Cmb_Puestos.SelectedItem.Text.Trim().Split(new Char[] { '*' })[0];

            Rs_Alta_Cat_Empleados.P_Aplica_ISSEG = (Chk_Aplica_Esquema_ISSEG.Checked) ? "SI" : "NO";

            Rs_Alta_Cat_Empleados.Alta_Empleado(); //Da de alta los datos del Empleado proporcionados por el usuario en la BD
            Txt_Empleado_ID.Text = Rs_Alta_Cat_Empleados.P_Empleado_ID;
            //Consulta_Empleados();//Comentado para no mostrar el grid de empleados.
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Empleados", "alert('El Alta del Empleado fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Empleado " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Empleado
    /// DESCRIPCION : Modifica los datos del Empleado con los proporcionados por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :Juan Alberto Hernandez Negrete
    /// FECHA_MODIFICO    :3/Noviembre/2010
    /// CAUSA_MODIFICACION: Completar el Catalogo
    ///*******************************************************************************
    private void Modificar_Empleado()
    {
        Cls_Cat_Empleados_Negocios Rs_Modificar_Cat_Empleados = new Cls_Cat_Empleados_Negocios();
        try
        {
            ///Datos Generales del Empleado
            Rs_Modificar_Cat_Empleados.P_Ruta_Foto = Txt_Ruta_Foto.Value;
            Rs_Modificar_Cat_Empleados.P_Empleado_ID = Txt_Empleado_ID.Text;
            Rs_Modificar_Cat_Empleados.P_No_Empleado = Convert.ToString(Txt_No_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Estatus = Cmb_Estatus_Empleado.SelectedValue;
            Rs_Modificar_Cat_Empleados.P_Nombre = Convert.ToString(Txt_Nombre_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Apellido_Paterno = Convert.ToString(Txt_Apellido_Paterno_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Apelldo_Materno = Convert.ToString(Txt_Apellido_Materno_Empleado.Text);
            if (Cmb_Roles_Empleado.SelectedIndex > 0)
            {
                if (Txt_Password_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_Password = Convert.ToString(Txt_Password_Empleado.Text);
                Rs_Modificar_Cat_Empleados.P_Rol_ID = Cmb_Roles_Empleado.SelectedValue;
            }
            Rs_Modificar_Cat_Empleados.P_Comentarios = Convert.ToString(Txt_Comentarios_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Confronto = Txt_Empleado_Confronto.Text.Trim();

            ///Datos Personales del Empleado
            Rs_Modificar_Cat_Empleados.P_Fecha_Nacimiento = Convert.ToDateTime(Txt_Fecha_Nacimiento_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Sexo = Cmb_Sexo_Empleado.SelectedValue;
            if (Txt_RFC_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_RFC = Convert.ToString(Txt_RFC_Empleado.Text);
            if (Txt_CURP_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_CURP = Convert.ToString(Txt_CURP_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Calle = Convert.ToString(Txt_Domicilio_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Colonia = Convert.ToString(Txt_Colonia_Empleado.Text);
            if (Txt_Codigo_Postal_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_Codigo_Postal = Convert.ToInt32(Txt_Codigo_Postal_Empleado.Text.ToString());
            Rs_Modificar_Cat_Empleados.P_Ciudad = Convert.ToString(Txt_Ciudad_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Estado = Convert.ToString(Txt_Estado_Empleado.Text);
            if (Txt_Telefono_Casa_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_Telefono_Casa = Convert.ToString(Txt_Telefono_Casa_Empleado.Text);
            if (Txt_Celular_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_Celular = Convert.ToString(Txt_Celular_Empleado.Text);
            if (Txt_Nextel_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_Nextel = Convert.ToString(Txt_Nextel_Empleado.Text);
            if (Txt_Telefono_Oficina_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_Telefono_Oficina = Convert.ToString(Txt_Telefono_Oficina_Empleado.Text);
            if (Txt_Extension_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_Extension = Convert.ToString(Txt_Extension_Empleado.Text);
            if (Txt_Fax_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_Fax = Convert.ToString(Txt_Fax_Empleado.Text);
            if (Txt_Correo_Electronico_Empleado.Text != "") Rs_Modificar_Cat_Empleados.P_Correo_Electronico = Convert.ToString(Txt_Correo_Electronico_Empleado.Text);
            Rs_Modificar_Cat_Empleados.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

            ///Datos Recursos Humanos
            Rs_Modificar_Cat_Empleados.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedIndex > 0 ? Cmb_Tipo_Nomina.SelectedValue.Trim() : "";
            Rs_Modificar_Cat_Empleados.P_No_IMSS = Txt_No_IMSS.Text;
            Rs_Modificar_Cat_Empleados.P_Forma_Pago = Cmb_Forma_Pago.SelectedIndex > 0 ? Cmb_Forma_Pago.SelectedValue.Trim() : "";
            Rs_Modificar_Cat_Empleados.P_No_Cuenta_Bancaria = Txt_Cuenta_Bancaria.Text;
            Rs_Modificar_Cat_Empleados.P_Fecha_Inicio = Convert.ToDateTime(Txt_Fecha_Inicio.Text);
            if (!string.IsNullOrEmpty(Txt_Fecha_Termino.Text)) Rs_Modificar_Cat_Empleados.P_Fecha_Termino_Contrato = Convert.ToDateTime(Txt_Fecha_Termino.Text);
            Rs_Modificar_Cat_Empleados.P_Tipo_Finiquito = Cmb_Tipo_Finiquito.SelectedIndex > 0 ? Cmb_Tipo_Finiquito.SelectedValue.Trim() : "";
            if (!string.IsNullOrEmpty(Txt_Fecha_Baja.Text)) Rs_Modificar_Cat_Empleados.P_Fecha_Baja = Convert.ToDateTime(Txt_Fecha_Baja.Text);
            if (!string.IsNullOrEmpty(Txt_Salario_Diario.Text)) Rs_Modificar_Cat_Empleados.P_Salario_Diario = Convert.ToDouble((Txt_Salario_Diario.Text.Equals("$  _,___,___.__") || Txt_Salario_Diario.Text.Equals("")) ? "0" : Txt_Salario_Diario.Text);
            if (!string.IsNullOrEmpty(Txt_Salario_Diario_Integrado.Text)) Rs_Modificar_Cat_Empleados.P_Salario_Diario_Integrado = Convert.ToDouble((Txt_Salario_Diario_Integrado.Text.Equals("$  _,___,___.__") || Txt_Salario_Diario_Integrado.Text.Equals("")) ? "0" : Txt_Salario_Diario_Integrado.Text);
            Rs_Modificar_Cat_Empleados.P_No_Licencia = Txt_No_Licencia.Text.Trim();
            if (!string.IsNullOrEmpty(Txt_Fecha_Vencimiento_Licencia.Text)) Rs_Modificar_Cat_Empleados.P_Fecha_Vigencia_Licencia = Convert.ToDateTime(Txt_Fecha_Vencimiento_Licencia.Text.Trim());
            Rs_Modificar_Cat_Empleados.P_Banco_ID = Cmb_Bancos.SelectedValue.Trim();
            Rs_Modificar_Cat_Empleados.P_Reloj_Checador = Cmb_Reloj_Checador.SelectedItem.Text.Trim();
            Rs_Modificar_Cat_Empleados.P_No_Tarjeta = Txt_No_Tarjeta.Text.Trim();
            Rs_Modificar_Cat_Empleados.P_No_Seguro_Poliza = Txt_No_Seguro_Poliza_Empleado.Text.Trim();
            Rs_Modificar_Cat_Empleados.P_Beneficiario_Seguro = Txt_Beneficiario_Empleado.Text.Trim();

            ///Datos Dias Trabajados
            Rs_Modificar_Cat_Empleados.P_Lunes = Chk_Lunes.Checked ? "SI" : "NO";
            Rs_Modificar_Cat_Empleados.P_Martes = Chk_Martes.Checked ? "SI" : "NO";
            Rs_Modificar_Cat_Empleados.P_Miercoles = Chk_Miercoles.Checked ? "SI" : "NO";
            Rs_Modificar_Cat_Empleados.P_Jueves = Chk_Jueves.Checked ? "SI" : "NO";
            Rs_Modificar_Cat_Empleados.P_Viernes = Chk_Viernes.Checked ? "SI" : "NO";
            Rs_Modificar_Cat_Empleados.P_Sabado = Chk_Sabado.Checked ? "SI" : "NO";
            Rs_Modificar_Cat_Empleados.P_Domingo = Chk_Domingo.Checked ? "SI" : "NO";

            ///Datos Presidencia
            //Rs_Modificar_Cat_Empleados.P_Area_ID = Cmb_Areas_Empleado.SelectedValue;
            Rs_Modificar_Cat_Empleados.P_Area_ID = "";
            //Rs_Modificar_Cat_Empleados.P_Tipo_Contrato_ID = Cmb_Tipo_Contrato.SelectedIndex > 0 ? Cmb_Tipo_Contrato.SelectedValue : "";
            Rs_Modificar_Cat_Empleados.P_Tipo_Contrato_ID = "";
            Rs_Modificar_Cat_Empleados.P_Puesto_ID = Cmb_Puestos.SelectedIndex > 0 ? Cmb_Puestos.SelectedValue : "";
            Rs_Modificar_Cat_Empleados.P_Escolaridad_ID = Cmb_Escolaridad.SelectedIndex > 0 ? Cmb_Escolaridad.SelectedValue : "";
            Rs_Modificar_Cat_Empleados.P_Sindicado_ID = Cmb_Sindicato.SelectedIndex > 0 ? Cmb_Sindicato.SelectedValue : "";
            Rs_Modificar_Cat_Empleados.P_Turno_ID = Cmb_Turno.SelectedIndex > 0 ? Cmb_Turno.SelectedValue : "";
            Rs_Modificar_Cat_Empleados.P_Zona_ID = Cmb_Zona.SelectedIndex > 0 ? Cmb_Zona.SelectedValue : "";
            //Rs_Modificar_Cat_Empleados.P_Tipo_Trabajador_ID = Cmb_Tipo_Trabajador.SelectedIndex > 0 ? Cmb_Tipo_Trabajador.SelectedValue : "";
            Rs_Modificar_Cat_Empleados.P_Tipo_Trabajador_ID = "";
            Rs_Modificar_Cat_Empleados.P_Terceros_ID = Cmb_Terceros.SelectedIndex > 0 ? Cmb_Terceros.SelectedValue : "";
            //if (Cmb_Cuentas_Contables.SelectedIndex > 0) { Rs_Modificar_Cat_Empleados.P_Cuenta_Contable_ID = Cmb_Cuentas_Contables.SelectedValue.Trim(); }
            Rs_Modificar_Cat_Empleados.P_Cuenta_Contable_ID = "";

            ///Crea el DataTable de los Documentos
            Guardar_Documentos();
            if (Session["Dt_Requisitos_Empleado"] != null)
            {
                Rs_Modificar_Cat_Empleados.P_Documentos_Anexos_Empleado = (DataTable)Session["Dt_Requisitos_Empleado"];
                Session.Remove("Dt_Requisitos_Empleado");
            }
            Recorre_Grid_Envia_Limpiar_Control_Subir_Archivos(Grid_Documentos_Empleado);

            ///Bitacora de movimientos
            Rs_Modificar_Cat_Empleados.P_Tipo_Movimiento = "ACTUALIZACION";
            Rs_Modificar_Cat_Empleados.P_Sueldo_Actual = (Calculo_Salario_Diario() * (new Cls_Utlidades_Nomina(DateTime.Now).P_Dias_Promedio_Mes_Anio));
            ///Percepciones Deducciones Tipo Nomina
            Rs_Modificar_Cat_Empleados.P_Dt_Tipo_Nomina_Lista_Percepciones = Crear_DataTable_Percepciones_Deducciones(Grid_Tipo_Nomina_Percepciones, "Txt_Cantidad_Percepcion");
            Rs_Modificar_Cat_Empleados.P_Dt_Tipo_Nomina_Lista_Deducciones = Crear_DataTable_Percepciones_Deducciones(Grid_Tipo_Nomina_Deducciones, "Txt_Cantidad_Deduccion");
            ///Percepciones Deducciones Sindicato
            Rs_Modificar_Cat_Empleados.P_Dt_Sindicato_Lista_Percepciones = Obtener_Percepciones_Deducciones_Sindicato(Cmb_Sindicato.SelectedValue.Trim(), "PERCEPCION");
            Rs_Modificar_Cat_Empleados.P_Dt_Sindicato_Lista_Deducciones = Obtener_Percepciones_Deducciones_Sindicato(Cmb_Sindicato.SelectedValue.Trim(), "DEDUCCION");

            //------------------------------------  SAP Código Programático  ----------------------------------------------
            Rs_Modificar_Cat_Empleados.P_Dependencia_ID = Cmb_SAP_Unidad_Responsable.SelectedValue.Trim();
            Rs_Modificar_Cat_Empleados.P_SAP_Fuente_Financiamiento = Cmb_SAP_Fuente_Financiamiento.SelectedValue.Trim();
            Rs_Modificar_Cat_Empleados.P_SAP_Area_Responsable_ID = Cmb_SAP_Area_Funcional.SelectedValue.Trim();
            Rs_Modificar_Cat_Empleados.P_SAP_Programa_ID = Cmb_SAP_Programas.SelectedValue.Trim();
            Rs_Modificar_Cat_Empleados.P_SAP_Partida_ID = Cmb_SAP_Partida.SelectedValue.Trim();
            Rs_Modificar_Cat_Empleados.P_SAP_Codigo_Programatico = Txt_SAP_Fuente_Financiamiento.Text.Trim() + "-" +
                Txt_SAP_Area_Responsable.Text.Trim() + "-" + Txt_SAP_Programa.Text.Trim() + "-" +
                Txt_SAP_Unidad_Responsable.Text.Trim() + "-" + Txt_SAP_Partida.Text.Trim();
            //---------------------------------------------------------------------------------------------------------------

            if (Cmb_Puestos.SelectedItem.Text.Trim().Split(new Char[] { '*' }).Length > 1)
                Rs_Modificar_Cat_Empleados.P_Clave = Cmb_Puestos.SelectedItem.Text.Trim().Split(new Char[] { '*' })[0];

            Rs_Modificar_Cat_Empleados.P_Aplica_ISSEG = (Chk_Aplica_Esquema_ISSEG.Checked) ? "SI" : "NO";

            Rs_Modificar_Cat_Empleados.Modificar_Empleado(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            //Consulta_Empleados();//Comentado para no mostrar el grid de empleados.
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Empleados", "alert('La Modificación del Empleado fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Empleado " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Empleado
    /// DESCRIPCION : Elimina los datos del Empleado que fue seleccionado por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Empleado()
    {
        Cls_Cat_Empleados_Negocios Rs_Eliminar_Cat_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Cat_Empleados.P_Empleado_ID = Txt_Empleado_ID.Text;
            Rs_Eliminar_Cat_Empleados.P_Dependencia_ID = Cmb_SAP_Unidad_Responsable.SelectedValue.Trim();
            Rs_Eliminar_Cat_Empleados.P_Puesto_ID = Cmb_Puestos.SelectedValue.Trim();
            Rs_Eliminar_Cat_Empleados.P_Tipo_Nomina_ID= Cmb_Tipo_Nomina.SelectedValue.Trim();
            Rs_Eliminar_Cat_Empleados.P_Tipo_Movimiento = "BAJA";
            Rs_Eliminar_Cat_Empleados.P_Motivo_Movimiento = Txt_Motivo_Baja.Text.Trim();
            Rs_Eliminar_Cat_Empleados.P_Sueldo_Actual = (Calculo_Salario_Diario() * (new Cls_Utlidades_Nomina(DateTime.Now).P_Dias_Promedio_Mes_Anio));
            Rs_Eliminar_Cat_Empleados.P_Estatus = "INACTIVO";

            if (Cmb_Puestos.SelectedItem.Text.Trim().Split(new Char[] { '*' }).Length > 1)
                Rs_Eliminar_Cat_Empleados.P_Clave = Cmb_Puestos.SelectedItem.Text.Trim().Split(new Char[] { '*' })[0];

            Rs_Eliminar_Cat_Empleados.Eliminar_Empleado(); //Elimina el Empleado que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            Grid_Empleados.DataBind();            
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Empleado " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Combos Pagina)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Indemnizaciones
    /// 
    /// DESCRIPCION : Consulta los registros de indemnización.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 20/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Consultar_Indemnizaciones()
    {
        Cls_Cat_Nom_Indemnizacion_Negocio Obj_Indemniacion = new Cls_Cat_Nom_Indemnizacion_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Indemnizaciones = null;//Variable que listas las indemnizaciones registradas en sistema.

        try
        {
            Dt_Indemnizaciones = Obj_Indemniacion.Consultar_Indemnizaciones();
            Cmb_Tipo_Finiquito.DataSource = Dt_Indemnizaciones;

            if (Dt_Indemnizaciones is DataTable)
            {
                if (Dt_Indemnizaciones.Rows.Count > 0)
                {
                    foreach (DataRow INDEMNIZACION in Dt_Indemnizaciones.Rows)
                    {
                        if (INDEMNIZACION is DataRow)
                        {
                            INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Nombre] = "[" + INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Dias].ToString().Trim() + "] -- " +
                                INDEMNIZACION[Cat_Nom_Indemnizacion.Campo_Nombre].ToString().Trim();
                        }
                    }
                }
            }

            Cmb_Tipo_Finiquito.DataTextField = Cat_Nom_Indemnizacion.Campo_Nombre;
            Cmb_Tipo_Finiquito.DataValueField = Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID;
            Cmb_Tipo_Finiquito.DataBind();

            Cmb_Tipo_Finiquito.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_Tipo_Finiquito.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las indemnizaciones en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Roles
    /// DESCRIPCION : Consulta los Roles que estan dadas de alta en la DB
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Roles()
    {
        DataTable Dt_Roles;
        Cls_Apl_Cat_Roles_Business Rs_Consulta_Apl_Cat_Roles = new Cls_Apl_Cat_Roles_Business();

        try
        {
            Dt_Roles = Rs_Consulta_Apl_Cat_Roles.Llenar_Tbl_Roles();
            Cmb_Roles_Empleado.DataSource = Dt_Roles;
            Cmb_Roles_Empleado.DataValueField = "Rol_ID";
            Cmb_Roles_Empleado.DataTextField = "Nombre";
            Cmb_Roles_Empleado.DataBind();
            Cmb_Roles_Empleado.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Roles_Empleado.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Areas_Dependencia
    /// DESCRIPCION : Consulta las áreas que tiene asignada la Dependencia
    /// PARAMETROS  : Dependencia_ID: Guarda el ID de la Dependencia a consultar sus áreas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Areas_Dependencia(String Dependencia_ID)
    {
        DataTable Dt_Areas;
        Cls_Cat_Areas_Negocio Rs_Consulta_Cat_Areas = new Cls_Cat_Areas_Negocio();

        try
        {
            Rs_Consulta_Cat_Areas.P_Dependencia_ID = Dependencia_ID;
            Dt_Areas = Rs_Consulta_Cat_Areas.Consulta_Areas();
            Cmb_Areas_Empleado.DataSource = Dt_Areas;
            Cmb_Areas_Empleado.DataValueField = "Area_ID";
            Cmb_Areas_Empleado.DataTextField = "Nombre";
            Cmb_Areas_Empleado.DataBind();
            Cmb_Areas_Empleado.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Areas_Empleado.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Areas_Dependencia " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Tipos_Contratos
    /// DESCRIPCION : Consulta los tipos de trabajador que existen.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Tipos_Contratos() {
        DataTable Dt_Tipo_Contratos;
        Cls_Cat_Nom_Tipos_Contratos_Negocio Cat_Nom_Tipo_Contratos = new Cls_Cat_Nom_Tipos_Contratos_Negocio();

        try
        {
            Dt_Tipo_Contratos = Cat_Nom_Tipo_Contratos.Consulta_Tipos_Contratos();
            Cmb_Tipo_Contrato.DataSource = Dt_Tipo_Contratos;
            Cmb_Tipo_Contrato.DataValueField = Presidencia.Constantes.Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID;
            Cmb_Tipo_Contrato.DataTextField = Presidencia.Constantes.Cat_Nom_Tipos_Contratos.Campo_Descripcion;
            Cmb_Tipo_Contrato.DataBind();
            Cmb_Tipo_Contrato.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Tipo_Contrato.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Tipos_Contratos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Puestos
    /// DESCRIPCION : Consulta los puestos
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Puestos(String Dependencia_ID, String Estatus, String Puesto_ID, String Empleado_ID)
    {
        DataTable Dt_Puestos;
        Cls_Cat_Puestos_Negocio Cat_Nom_Puestos = new Cls_Cat_Puestos_Negocio();
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        String DataTextField = String.Empty;
        String DataValueField = String.Empty;

        try
        {
            if (!String.IsNullOrEmpty(Estatus)) Cat_Nom_Puestos.P_Estatus = Estatus;
            Cat_Nom_Puestos.P_Dependencia_ID = Dependencia_ID;

            Dt_Puestos = Cat_Nom_Puestos.Consultar_Puestos_Disponibles_Dependencia();

            Cmb_Puestos.DataSource = Dt_Puestos;
            Cmb_Puestos.DataValueField = Presidencia.Constantes.Cat_Puestos.Campo_Puesto_ID;
            Cmb_Puestos.DataTextField = "PUESTO";
            Cmb_Puestos.DataBind();
            Cmb_Puestos.Items.Insert(0, new ListItem("<- Seleccione ->", ""));

            if (!String.IsNullOrEmpty(Txt_Empleado_ID.Text.Trim()))
            {
                Cat_Nom_Puestos.P_Empleado_ID = Txt_Empleado_ID.Text.Trim();
                Cat_Nom_Puestos.P_Puesto_ID = Obj_Empleados.Obtener_Puesto_ID(Txt_Empleado_ID.Text.Trim());
            }


            if (!String.IsNullOrEmpty(Cat_Nom_Puestos.P_Puesto_ID) && !String.IsNullOrEmpty(Cat_Nom_Puestos.P_Empleado_ID))
            {
                Dt_Puestos = Cat_Nom_Puestos.Consultar_Puestos();
                if (Dt_Puestos is DataTable)
                {
                    if (Dt_Puestos.Rows.Count > 0)
                    {
                        foreach (DataRow PUESTO in Dt_Puestos.Rows)
                        {
                            if (PUESTO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Nombre].ToString().Trim()))
                                {
                                    DataTextField = PUESTO[Cat_Puestos.Campo_Nombre].ToString().Trim();
                                }

                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Puesto_ID].ToString().Trim()))
                                {
                                    DataValueField = PUESTO[Cat_Puestos.Campo_Puesto_ID].ToString().Trim();
                                }

                                Cmb_Puestos.Items.Insert(1, new ListItem(DataTextField, DataValueField));
                            }
                        }
                    }
                }

            }

            Cmb_Puestos.SelectedIndex = -1;

            Presidencia.Codigo_Programatico.Cls_Ayudante_Codigo_Programatico.Load_Plazas(ref Cmb_Puestos, Txt_Empleado_ID.Text, Dependencia_ID);
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Escolaridad
    /// DESCRIPCION : Consulta Escolaridad
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Escolaridad() {
        DataTable Dt_Escolaridad;
        Cls_Cat_Nom_Escolaridad_Negocio Cat_Nom_Escolaridad = new Cls_Cat_Nom_Escolaridad_Negocio();

        try
        {
            Dt_Escolaridad = Cat_Nom_Escolaridad.Consulta_Escolaridad();
            Cmb_Escolaridad.DataSource = Dt_Escolaridad;
            Cmb_Escolaridad.DataValueField = Presidencia.Constantes.Cat_Nom_Escolaridad.Campo_Escolaridad_ID;
            Cmb_Escolaridad.DataTextField = Presidencia.Constantes.Cat_Nom_Escolaridad.Campo_Escolaridad;
            Cmb_Escolaridad.DataBind();
            Cmb_Escolaridad.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Escolaridad.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Escolaridad " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:Consultar_Sindicatos
    /// DESCRIPCION : Consulta Programas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Sindicatos() {
        DataTable Dt_Sindicatos;
        Cls_Cat_Nom_Sindicatos_Negocio Cat_Nom_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();

        try
        {
            Dt_Sindicatos = Cat_Nom_Sindicatos.Consulta_Sindicato();
            Cmb_Sindicato.DataSource = Dt_Sindicatos;
            Cmb_Sindicato.DataValueField = Presidencia.Constantes.Cat_Nom_Sindicatos.Campo_Sindicato_ID;
            Cmb_Sindicato.DataTextField = Presidencia.Constantes.Cat_Nom_Sindicatos.Campo_Nombre;
            Cmb_Sindicato.DataBind();
            Cmb_Sindicato.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Sindicato.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Sindicatos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:Consultar_Turnos
    /// DESCRIPCION : Consulta los turnos
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Turnos() {
        DataTable Dt_Turnos;
        Cls_Cat_Turnos_Negocio Cat_Turnos = new Cls_Cat_Turnos_Negocio();

        try
        {
            Dt_Turnos = Cat_Turnos.Consulta_Turnos();
            Cmb_Turno.DataSource = Dt_Turnos;
            Cmb_Turno.DataValueField = Presidencia.Constantes.Cat_Turnos.Campo_Turno_ID;
            Cmb_Turno.DataTextField = Presidencia.Constantes.Cat_Turnos.Campo_Descripcion;
            Cmb_Turno.DataBind();
            Cmb_Turno.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Turno.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Turnos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:Consultar_Zonas
    /// DESCRIPCION : Consulta las zonas de salario
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Zonas() {
        DataTable Dt_Zonas;
        Cls_Cat_Nom_Zona_Economica_Negocio Cat_Nom_Zonas = new Cls_Cat_Nom_Zona_Economica_Negocio();

        try
        {
            Dt_Zonas = Cat_Nom_Zonas.Consulta_Zona_Economica();
            Cmb_Zona.DataSource = Dt_Zonas;
            Cmb_Zona.DataValueField = Presidencia.Constantes.Cat_Nom_Zona_Economica.Campo_Zona_ID;
            Cmb_Zona.DataTextField = Presidencia.Constantes.Cat_Nom_Zona_Economica.Campo_Zona_Economica;
            Cmb_Zona.DataBind();
            Cmb_Zona.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Zona.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Zonas " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:Consultar_Tipo_Trabajador
    /// DESCRIPCION : Consulta los tipos de trabajador
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Tipo_Trabajador() {
        DataTable Dt_Tipos_Trabajador;
        Cls_Cat_Nom_Tipo_Trabajador_Negocio Cat_Nom_Tipos_Trabajador = new Cls_Cat_Nom_Tipo_Trabajador_Negocio();

        try
        {
            Dt_Tipos_Trabajador = Cat_Nom_Tipos_Trabajador.Consulta_Tipo_Trabajador();
            Cmb_Tipo_Trabajador.DataSource = Dt_Tipos_Trabajador;
            Cmb_Tipo_Trabajador.DataValueField = Presidencia.Constantes.Cat_Nom_Tipo_Trabajador.Campo_Tipo_Trabajador_ID;
            Cmb_Tipo_Trabajador.DataTextField = Presidencia.Constantes.Cat_Nom_Tipo_Trabajador.Campo_Descripcion;
            Cmb_Tipo_Trabajador.DataBind();
            Cmb_Tipo_Trabajador.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Tipo_Trabajador.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Tipos_Nomina
    /// DESCRIPCION : Consulta los tipos de Nomina que existen
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 5/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Tipos_Nomina()
    {
        DataTable Dt_Tipos_Nomina;
        Cls_Cat_Empleados_Negocios Cat_Empleados = new Cls_Cat_Empleados_Negocios();

        try
        {
            Dt_Tipos_Nomina = Cat_Empleados.Consultar_Tipos_Nomina();
            Cmb_Tipo_Nomina.DataSource = Dt_Tipos_Nomina;
            Cmb_Tipo_Nomina.DataValueField = Presidencia.Constantes.Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID;
            Cmb_Tipo_Nomina.DataTextField = Presidencia.Constantes.Cat_Nom_Tipos_Nominas.Campo_Nomina;
            Cmb_Tipo_Nomina.DataBind();
            Cmb_Tipo_Nomina.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Tipo_Nomina.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Tipos_Nomina
    /// DESCRIPCION : Consulta los Tipos de Retenciones a Terceros
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 5/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Retenciones_Terceros()
    {
        DataTable Dt_Terceros;
        Cls_Cat_Empleados_Negocios Cat_Empleados = new Cls_Cat_Empleados_Negocios();

        try
        {
            Dt_Terceros = Cat_Empleados.Consultar_Terceros();
            Cmb_Terceros.DataSource = Dt_Terceros;
            Cmb_Terceros.DataValueField = Presidencia.Constantes.Cat_Nom_Terceros.Campo_Tercero_ID;
            Cmb_Terceros.DataTextField = Presidencia.Constantes.Cat_Nom_Terceros.Campo_Nombre;
            Cmb_Terceros.DataBind();
            Cmb_Terceros.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Terceros.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Formas_Pago
    /// DESCRIPCION : Consulta los Tipos de pago que actualmente existen en el sistema.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Formas_Pago()
    {
        Cls_Cat_Nom_Tipos_Pago_Negocio Obj_Tipos_Pago = new Cls_Cat_Nom_Tipos_Pago_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tipos_Pago = null;//Variable que lista los tipos de pago.

        try
        {
            Dt_Tipos_Pago = Obj_Tipos_Pago.Consultar_Tipo_Pago();
            Cmb_Forma_Pago.DataSource = Dt_Tipos_Pago;
            Cmb_Forma_Pago.DataTextField = Cat_Nom_Tipos_Pagos.Campo_Nombre;
            Cmb_Forma_Pago.DataValueField = Cat_Nom_Tipos_Pagos.Campo_Tipo_Pago_ID;
            Cmb_Forma_Pago.DataBind();

            Cmb_Forma_Pago.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Forma_Pago.SelectedIndex = (-1);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las formas de pago que actualmente existen. Error: [" + Ex.Message + "]");
        }
    }
    /// ***********************************************************************************************
    /// Nombre: Consultar_Cuentas_Contables
    /// 
    /// Descripción: Consultar las cuentas contables.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 09/Noviembre/2011.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************************
    private void Consultar_Cuentas_Contables()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Cuentas_Contables = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexión con la capa de negocios.
        DataTable Dt_Cuentas_Contables = null;//Variable que almacenara un listado de cuentas contables.

        try
        {
            Dt_Cuentas_Contables = Obj_Cuentas_Contables.Consultar_Cuentas_Contables();
            Cmb_Cuentas_Contables.DataSource = Dt_Cuentas_Contables;
            Cmb_Cuentas_Contables.DataTextField = "CUENTA_CONTABLE";
            Cmb_Cuentas_Contables.DataValueField = Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID;
            Cmb_Cuentas_Contables.DataBind();

            Cmb_Cuentas_Contables.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Cuentas_Contables.SelectedIndex = (-1);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las cuentas contables. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Combos Busqueda)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Dependencias_Busqueda
    /// DESCRIPCION : Consulta las Dependencias uy Roles que estan dadas de alta en la DB
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Dependencias_Busqueda()
    {
        DataTable Dt_Dependencias;
        Cls_Cat_Dependencias_Negocio Rs_Consulta_Cat_Dependencias = new Cls_Cat_Dependencias_Negocio();

        try
        {
            Dt_Dependencias = Rs_Consulta_Cat_Dependencias.Consulta_Dependencias();
            Cmb_Busqueda_Dependencia.DataSource = Dt_Dependencias;
            Cmb_Busqueda_Dependencia.DataValueField = "Dependencia_ID";
            Cmb_Busqueda_Dependencia.DataTextField = "Nombre";
            Cmb_Busqueda_Dependencia.DataBind();
            Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Dependencia.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Dependencias " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Roles
    /// DESCRIPCION : Consulta los Roles que estan dadas de alta en la DB
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Roles_Busqueda()
    {
        DataTable Dt_Roles;
        Cls_Apl_Cat_Roles_Business Rs_Consulta_Apl_Cat_Roles = new Cls_Apl_Cat_Roles_Business();

        try
        {
            Dt_Roles = Rs_Consulta_Apl_Cat_Roles.Llenar_Tbl_Roles();
            Cmb_Busqueda_Rol.DataSource = Dt_Roles;
            Cmb_Busqueda_Rol.DataValueField = "Rol_ID";
            Cmb_Busqueda_Rol.DataTextField = "Nombre";
            Cmb_Busqueda_Rol.DataBind();
            Cmb_Busqueda_Rol.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Rol.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Areas_Dependencia
    /// DESCRIPCION : Consulta las áreas que tiene asignada la Dependencia
    /// PARAMETROS  : Dependencia_ID: Guarda el ID de la Dependencia a consultar sus áreas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Areas_Dependencia_Busqueda(String Dependencia_ID)
    {
        DataTable Dt_Areas;
        Cls_Cat_Areas_Negocio Rs_Consulta_Cat_Areas = new Cls_Cat_Areas_Negocio();

        try
        {
            Rs_Consulta_Cat_Areas.P_Dependencia_ID = Dependencia_ID;
            Dt_Areas = Rs_Consulta_Cat_Areas.Consulta_Areas();
            Cmb_Busqueda_Areas.DataSource = Dt_Areas;
            Cmb_Busqueda_Areas.DataValueField = "Area_ID";
            Cmb_Busqueda_Areas.DataTextField = "Nombre";
            Cmb_Busqueda_Areas.DataBind();
            Cmb_Busqueda_Areas.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Areas.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Areas_Dependencia " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Tipos_Contratos
    /// DESCRIPCION : Consulta los tipos de trabajador que existen.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Tipos_Contratos_Busqueda()
    {
        DataTable Dt_Tipo_Contratos;
        Cls_Cat_Nom_Tipos_Contratos_Negocio Cat_Nom_Tipo_Contratos = new Cls_Cat_Nom_Tipos_Contratos_Negocio();

        try
        {
            Dt_Tipo_Contratos = Cat_Nom_Tipo_Contratos.Consulta_Tipos_Contratos();
            Cmb_Busqueda_Tipo_Contrato.DataSource = Dt_Tipo_Contratos;
            Cmb_Busqueda_Tipo_Contrato.DataValueField = Presidencia.Constantes.Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID;
            Cmb_Busqueda_Tipo_Contrato.DataTextField = Presidencia.Constantes.Cat_Nom_Tipos_Contratos.Campo_Descripcion;
            Cmb_Busqueda_Tipo_Contrato.DataBind();
            Cmb_Busqueda_Tipo_Contrato.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Tipo_Contrato.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Tipos_Contratos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Puestos
    /// DESCRIPCION : Consulta los puestos
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Puestos_Busqueda()
    {
        DataTable Dt_Puestos;
        Cls_Cat_Puestos_Negocio Cat_Nom_Puestos = new Cls_Cat_Puestos_Negocio();

        try
        {
            Cat_Nom_Puestos.P_Tipo_DataTable = "PUESTOS";
            Dt_Puestos = Cat_Nom_Puestos.Consulta_DataTable();
            Cmb_Busqueda_Puesto.DataSource = Dt_Puestos;
            Cmb_Busqueda_Puesto.DataValueField = Presidencia.Constantes.Cat_Puestos.Campo_Puesto_ID;
            Cmb_Busqueda_Puesto.DataTextField = Presidencia.Constantes.Cat_Puestos.Campo_Nombre;
            Cmb_Busqueda_Puesto.DataBind();
            Cmb_Busqueda_Puesto.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Puesto.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Escolaridad
    /// DESCRIPCION : Consulta Escolaridad
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Escolaridad_Busqueda()
    {
        DataTable Dt_Escolaridad;
        Cls_Cat_Nom_Escolaridad_Negocio Cat_Nom_Escolaridad = new Cls_Cat_Nom_Escolaridad_Negocio();

        try
        {
            Dt_Escolaridad = Cat_Nom_Escolaridad.Consulta_Escolaridad();
            Cmb_Busqueda_Escolaridad.DataSource = Dt_Escolaridad;
            Cmb_Busqueda_Escolaridad.DataValueField = Presidencia.Constantes.Cat_Nom_Escolaridad.Campo_Escolaridad_ID;
            Cmb_Busqueda_Escolaridad.DataTextField = Presidencia.Constantes.Cat_Nom_Escolaridad.Campo_Escolaridad;
            Cmb_Busqueda_Escolaridad.DataBind();
            Cmb_Busqueda_Escolaridad.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Escolaridad.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Escolaridad " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:Consultar_Sindicatos
    /// DESCRIPCION : Consulta Programas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Sindicatos_Busqueda()
    {
        DataTable Dt_Sindicatos;
        Cls_Cat_Nom_Sindicatos_Negocio Cat_Nom_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();

        try
        {
            Dt_Sindicatos = Cat_Nom_Sindicatos.Consulta_Sindicato();
            Cmb_Busqueda_Sindicato.DataSource = Dt_Sindicatos;
            Cmb_Busqueda_Sindicato.DataValueField = Presidencia.Constantes.Cat_Nom_Sindicatos.Campo_Sindicato_ID;
            Cmb_Busqueda_Sindicato.DataTextField = Presidencia.Constantes.Cat_Nom_Sindicatos.Campo_Nombre;
            Cmb_Busqueda_Sindicato.DataBind();
            Cmb_Busqueda_Sindicato.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Sindicato.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Sindicatos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:Consultar_Turnos
    /// DESCRIPCION : Consulta los turnos
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Turnos_Busqueda()
    {
        DataTable Dt_Turnos;
        Cls_Cat_Turnos_Negocio Cat_Turnos = new Cls_Cat_Turnos_Negocio();

        try
        {
            Dt_Turnos = Cat_Turnos.Consulta_Turnos();
            Cmb_Busqueda_Turno.DataSource = Dt_Turnos;
            Cmb_Busqueda_Turno.DataValueField = Presidencia.Constantes.Cat_Turnos.Campo_Turno_ID;
            Cmb_Busqueda_Turno.DataTextField = Presidencia.Constantes.Cat_Turnos.Campo_Descripcion;
            Cmb_Busqueda_Turno.DataBind();
            Cmb_Busqueda_Turno.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Turno.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Turnos " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:Consultar_Zonas
    /// DESCRIPCION : Consulta las zonas de salario
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Zonas_Busqueda()
    {
        DataTable Dt_Zonas;
        Cls_Cat_Nom_Zona_Economica_Negocio Cat_Nom_Zonas = new Cls_Cat_Nom_Zona_Economica_Negocio();

        try
        {
            Dt_Zonas = Cat_Nom_Zonas.Consulta_Zona_Economica();
            Cmb_Busqueda_Zona.DataSource = Dt_Zonas;
            Cmb_Busqueda_Zona.DataValueField = Presidencia.Constantes.Cat_Nom_Zona_Economica.Campo_Zona_ID;
            Cmb_Busqueda_Zona.DataTextField = Presidencia.Constantes.Cat_Nom_Zona_Economica.Campo_Zona_Economica;
            Cmb_Busqueda_Zona.DataBind();
            Cmb_Busqueda_Zona.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Zona.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Zonas " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION:Consultar_Tipo_Trabajador
    /// DESCRIPCION : Consulta los tipos de trabajador
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Tipo_Trabajador_Busqueda()
    {
        DataTable Dt_Tipos_Trabajador;
        Cls_Cat_Nom_Tipo_Trabajador_Negocio Cat_Nom_Tipos_Trabajador = new Cls_Cat_Nom_Tipo_Trabajador_Negocio();

        try
        {
            Dt_Tipos_Trabajador = Cat_Nom_Tipos_Trabajador.Consulta_Tipo_Trabajador();
            Cmb_Busqueda_Tipo_Trabajador.DataSource = Dt_Tipos_Trabajador;
            Cmb_Busqueda_Tipo_Trabajador.DataValueField = Presidencia.Constantes.Cat_Nom_Tipo_Trabajador.Campo_Tipo_Trabajador_ID;
            Cmb_Busqueda_Tipo_Trabajador.DataTextField = Presidencia.Constantes.Cat_Nom_Tipo_Trabajador.Campo_Descripcion;
            Cmb_Busqueda_Tipo_Trabajador.DataBind();
            Cmb_Busqueda_Tipo_Trabajador.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Tipo_Trabajador.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Tipos_Nomina
    /// DESCRIPCION : Consulta los tipos de Nomina que existen
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 5/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Tipos_Nomina_Busqueda()
    {
        DataTable Dt_Tipos_Nomina;
        Cls_Cat_Empleados_Negocios Cat_Empleados = new Cls_Cat_Empleados_Negocios();

        try
        {
            Dt_Tipos_Nomina = Cat_Empleados.Consultar_Tipos_Nomina();
            Cmb_Busqueda_Tipo_Nomina.DataSource = Dt_Tipos_Nomina;
            Cmb_Busqueda_Tipo_Nomina.DataValueField = Presidencia.Constantes.Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID;
            Cmb_Busqueda_Tipo_Nomina.DataTextField = Presidencia.Constantes.Cat_Nom_Tipos_Nominas.Campo_Nomina;
            Cmb_Busqueda_Tipo_Nomina.DataBind();
            Cmb_Busqueda_Tipo_Nomina.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Tipo_Nomina.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Bancos
    /// 
    /// DESCRIPCION : Consulta los bancos que existen actualmente registrados en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 20/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Bancos()
    {
        Cls_Cat_Nom_Bancos_Negocio Obj_Bancos = new Cls_Cat_Nom_Bancos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Bancos = null;//Variable que almacena la lista de bancos registrados actualmente en el sistema.

        try
        {
            Dt_Bancos = Obj_Bancos.Consulta_Bancos();
            Cmb_Bancos.DataSource = Dt_Bancos;
            Cmb_Bancos.DataTextField = Cat_Nom_Bancos.Campo_Nombre;
            Cmb_Bancos.DataValueField = Cat_Nom_Bancos.Campo_Banco_ID;
            Cmb_Bancos.DataBind();
            Cmb_Bancos.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_Bancos.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los bancos que existen actualmente en sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Calculos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Calculo_Salario_Diario
    /// DESCRIPCION : Ejecuta el calculo del salario diario.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 09/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Double Calculo_Salario_Diario() {
        Cls_Cat_Empleados_Negocios Cls_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        Cls_Utlidades_Nomina Obj_Utilidades_Nomina = new Cls_Utlidades_Nomina(DateTime.Now);//Variable de conexion con las utilidades de nómina.
        DataTable Dt_Datos_Puesto = null;//Variable que alamacenara los datos del puesto consultado.
        Double Salario_Diario = 0.0;//Alamcenara el salario diario del empleado.
        String Puesto_ID = "";//Variable que almacenara el id del puesto seleccionado.

        try
        {
            if (Cmb_Puestos.SelectedIndex > 0)
            {
                Puesto_ID = Cmb_Puestos.SelectedValue.Trim();
                Cls_Empleados.P_Puesto_ID = Puesto_ID;
                Dt_Datos_Puesto = Cls_Empleados.Consulta_Puestos_Empleados();//Consultamos la informacion del puesto

                if (Dt_Datos_Puesto != null)
                {
                    if (Dt_Datos_Puesto.Rows.Count > 0)
                    {
                        String Salario_Puesto = HttpUtility.HtmlDecode(Dt_Datos_Puesto.Rows[0][Cat_Puestos.Campo_Salario_Mensual].ToString());
                        if (!string.IsNullOrEmpty(Salario_Puesto))
                        {
                            Salario_Diario = (Convert.ToDouble(Salario_Puesto) / Cls_Utlidades_Nomina.Dias_Mes_Fijo);
                        }
                    }
                    else
                    {
                        Salario_Diario = 0;
                    }
                }
                else
                {
                    Salario_Diario = 0;
                }
            }
            else {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Calcular Salario Diario",
                    "alert('No es posible calcular el salario diario \nsi no se a seleccionado el puesto para el empleado');", true);
            }           
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al calcular el Salario diario del empleado. Error: [" + Ex.Message + "]");
        }
        return Salario_Diario;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Calculo_Salario_Diario_Integrado
    /// DESCRIPCION : Ejecuta el calculo del salario diario integrado.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 09/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Double Calculo_Salario_Diario_Integrado(Double Salario_Diario, String No_Empleado)
    {
        Double Salario_Diario_Integrado = 0.0;//Variable que el almacenara el salario diario itegrado.

        try
        {
            Salario_Diario_Integrado = (Salario_Diario * 1.126);
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Salario_Diario_Integrado;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Calcular_Salarios
    /// DESCRIPCION : Calcuar salaio diario y salario diario integrado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Calcular_Salarios() {
        Double Salario_Diario = 0.0;
        Double Salario_Diario_Integrado = 0.0;
        try
        {
            Salario_Diario = Calculo_Salario_Diario();
            Salario_Diario_Integrado = Calculo_Salario_Diario_Integrado(Salario_Diario, Txt_No_Empleado.Text.Trim());
            Txt_Salario_Diario.Text = string.Format("{0:#,###,##0.00}", Salario_Diario);
            Txt_Salario_Diario_Integrado.Text = string.Format("{0:#,###,##0.00}", Salario_Diario_Integrado);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Calcular_Salarios
    /// DESCRIPCION : Calcuar salaio diario y salario diario integrado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Validar_Salario_Diario()
    {
        if (!string.IsNullOrEmpty(Txt_Salario_Diario.Text.Trim()))
        {
            String Salario_Diario_Guardado = string.Format("{0:#,###,###.00}", Convert.ToDouble(Txt_Salario_Diario.Text.Trim()));
            String Salario_Diario_Nuevo = string.Format("{0:#,###,###.00}", Calculo_Salario_Diario());
            if (!Salario_Diario_Guardado.Equals(Salario_Diario_Nuevo))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('El salario diario no esta actualizado');", true);
            }
        }
    }
    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Empleados_Avanzada
    /// DESCRIPCION : Ejecuta la busqueda de empleados
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Empleados_Avanzada()
    {
        Cls_Cat_Empleados_Negocios Rs_Consulta_Ca_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 

        try
        {
            if (!string.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text.Trim())) Rs_Consulta_Ca_Empleados.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim();
            if (!string.IsNullOrEmpty(Txt_Busqueda_RFC.Text.Trim())) Rs_Consulta_Ca_Empleados.P_RFC = Txt_Busqueda_RFC.Text.Trim();
            if (Cmb_Busqueda_Estatus.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();
            if (!string.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text.Trim()) && (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 2)) Rs_Consulta_Ca_Empleados.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim();
            if (Cmb_Busqueda_Rol.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Rol_ID = Cmb_Busqueda_Rol.SelectedValue.Trim();
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedValue.Trim();            
            if (Cmb_Busqueda_Tipo_Contrato.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Tipo_Contrato_ID = Cmb_Busqueda_Tipo_Contrato.SelectedValue.Trim();
            if (Cmb_Busqueda_Areas.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Area_ID = Cmb_Busqueda_Areas.SelectedValue.Trim();
            if (Cmb_Busqueda_Puesto.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Puesto_ID = Cmb_Busqueda_Puesto.SelectedValue.Trim();
            if (Cmb_Busqueda_Escolaridad.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Escolaridad_ID = Cmb_Busqueda_Escolaridad.SelectedValue.Trim();
            if (Cmb_Busqueda_Sindicato.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Sindicado_ID = Cmb_Busqueda_Sindicato.SelectedValue.Trim();
            if (Cmb_Busqueda_Turno.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Turno_ID = Cmb_Busqueda_Turno.SelectedValue.Trim();
            if (Cmb_Busqueda_Zona.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Zona_ID = Cmb_Busqueda_Zona.SelectedValue.Trim();
            if (Cmb_Busqueda_Tipo_Trabajador.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Tipo_Trabajador_ID = Cmb_Busqueda_Tipo_Trabajador.SelectedValue.Trim();
            if (Cmb_Busqueda_Tipo_Nomina.SelectedIndex > 0) Rs_Consulta_Ca_Empleados.P_Tipo_Nomina_ID = Cmb_Busqueda_Tipo_Nomina.SelectedValue.Trim();
            if (!String.IsNullOrEmpty(Txt_Busqueda_Codigo_Programatico.Text)) Rs_Consulta_Ca_Empleados.P_SAP_Codigo_Programatico = Txt_Busqueda_Codigo_Programatico.Text.Trim().Replace(" ", ""); ;

            if (!string.IsNullOrEmpty(Txt_Busqueda_Fecha_Inicio.Text.Trim()) && !string.IsNullOrEmpty(Txt_Busqueda_Fecha_Fin.Text.Trim()))
            {
                if (Validar_Formato_Fecha(Txt_Busqueda_Fecha_Inicio.Text.Trim()) && Validar_Formato_Fecha(Txt_Busqueda_Fecha_Fin.Text.Trim()))
                {
                    Rs_Consulta_Ca_Empleados.P_Fecha_Inicio_Busqueda = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Busqueda_Fecha_Inicio.Text.Trim()));
                    Rs_Consulta_Ca_Empleados.P_Fecha_Fin_Busqueda = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Busqueda_Fecha_Fin.Text.Trim()));
                }
            }

            Dt_Empleados = Rs_Consulta_Ca_Empleados.Consulta_Empleados_General(); //Consulta todos los Empleados que coindican con lo proporcionado por el usuario
            Session["Consulta_Empleados"] = Dt_Empleados;
            Llena_Grid_Empleados();
            Mpe_Busqueda_Empleados.Hide();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Empleados " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Empleados
    /// DESCRIPCION : Consulta los Empleados que estan dadas de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Empleados()
    {
        Cls_Cat_Empleados_Negocios Rs_Consulta_Ca_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 

        try
        {
            if (!string.IsNullOrEmpty(Txt_Empleado_ID.Text))
            {
                Rs_Consulta_Ca_Empleados.P_Empleado_ID = Txt_Empleado_ID.Text;
            }
            Dt_Empleados = Rs_Consulta_Ca_Empleados.Consulta_Empleados(); //Consulta todos los Empleados que coindican con lo proporcionado por el usuario
            Session["Consulta_Empleados"] = Dt_Empleados;
            Llena_Grid_Empleados();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Empleados " + ex.Message.ToString(), ex);
        }
    }
    ///******************************************************************************
    /// NOMBRE DE LA FUNCIÓN: Consulta_Detalles_Requisitos
    /// DESCRIPCIÓN: Se hace un barrido de la consulta de los Requisitos del Empleado seleccionado,
    /// y se le asigna el valor al checkbox de la tabla, dependiendo del valor del Estatus
    /// del documento.
    /// CREO: Juan Alberto Hernandez Negrete
    /// FECHA_CREO: 22/Octubre/2010
    /// MODIFICO:
    /// FECHA_MODIFICO
    /// CAUSA_MODIFICACIÓN   
    ///******************************************************************************
    private void Consulta_Detalles_Requisitos()
    {
        CheckBox _Chk_Estatus_Requisito;
        DataTable Dt_Requisitos;
        Cls_Cat_Empleados_Negocios Cat_Empleados;

        try
        {
            Cat_Empleados = new Cls_Cat_Empleados_Negocios();
            Cat_Empleados.P_Empleado_ID = Txt_Empleado_ID.Text.Trim();
            Dt_Requisitos = Cat_Empleados.Consultar_Requisitos_Empleados_Entregados();

            if (Dt_Requisitos.Rows.Count > 0)
            {
                foreach (DataRow Renglon in Dt_Requisitos.Rows)
                {
                    for (int Count_Fila = 0; Count_Fila < Grid_Documentos_Empleado.Rows.Count; Count_Fila++)
                    {
                        _Chk_Estatus_Requisito = (CheckBox)Grid_Documentos_Empleado.Rows[Count_Fila].Cells[3].FindControl("Chk_Requisito_Empleado");
                        if (Renglon[Ope_Nom_Requisitos_Empleado.Campo_Entregado].ToString().Trim().Equals("N"))
                        {
                            _Chk_Estatus_Requisito.Checked = false;
                        }
                        else if (Grid_Documentos_Empleado.Rows[Count_Fila].Cells[0].Text.Equals(Renglon[Ope_Nom_Requisitos_Empleado.Campo_Requisitos_ID].ToString()))
                        {
                            _Chk_Estatus_Requisito.Checked = true;
                        }
                    }
                }
            }//End If
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.Trim());
        }//End Catch
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Requisitos_Empleado
    /// DESCRIPCION : Consulta los Requisitos del Empleado. Y Carga el Grid de los requisitos
    /// del empleado.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 26/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Requisitos_Empleado()
    {
        Cls_Cat_Empleados_Negocios Cat_Empleado = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Requisitos_Empleado;
        try
        {
            Dt_Requisitos_Empleado = Cat_Empleado.Consultar_Requisitos_Empleados();
            Grid_Documentos_Empleado.DataSource = Dt_Requisitos_Empleado;
            Grid_Documentos_Empleado.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro generado al consultar los requisitos del empleado" + ex.Message);
        }
    }
    #endregion

    #region (Metodos Validacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Email
    /// DESCRIPCION : Valida el E-mail Ingresado
    /// CREO        : Susana Trigueros Armenta
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Email()
    {
        string Patron_Email = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" +
                                   @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." +
                                   @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" +
                                   @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        if (Txt_Correo_Electronico_Empleado.Text != null) return Regex.IsMatch(Txt_Correo_Electronico_Empleado.Text, Patron_Email);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_RFC
    /// DESCRIPCION : Valida el RFC Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_RFC()
    {
        string Patron_RFC = @"^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$";

        if (Txt_RFC_Empleado.Text != null) return Regex.IsMatch(Txt_RFC_Empleado.Text, Patron_RFC);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Codigo_Postal
    /// DESCRIPCION : Valida el Codigo Postal Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Codigo_Postal()
    {
        string Patron_CP = @"^([1-9]{2}|[0-9][1-9]|[1-9][0-9])[0-9]{3}$";

        if (Txt_Codigo_Postal_Empleado.Text != null) return Regex.IsMatch(Txt_Codigo_Postal_Empleado.Text, Patron_CP);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Celular_Empleado
    /// DESCRIPCION : Valida el Telefono Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Celular_Empleado()
    {
        string Patron_Celular = @"^[0-9]{2,3}-? ?[0-9]{7,10}$";

        if (Txt_Celular_Empleado.Text != null) return Regex.IsMatch(Txt_Celular_Empleado.Text, Patron_Celular);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Telefono_Casa_Empleado
    /// DESCRIPCION : Valida el Telefono Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Telefono_Casa_Empleado()
    {
        string Patron_Tel_Casa = @"^[0-9]{2,3}-? ?[0-9]{5,8}$";

        if (Txt_Telefono_Casa_Empleado.Text != null) return Regex.IsMatch(Txt_Telefono_Casa_Empleado.Text, Patron_Tel_Casa);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Telefono_Oficina_Empleado
    /// DESCRIPCION : Valida el Telefono Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Telefono_Oficina_Empleado()
    {
        string Patron_Telefono_Oficina = @"^[0-9]{2,3}-? ?[0-9]{6,8}$";

        if (Txt_Telefono_Oficina_Empleado.Text != null) return Regex.IsMatch(Txt_Telefono_Oficina_Empleado.Text, Patron_Telefono_Oficina);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Fax
    /// DESCRIPCION : Valida el Fax Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Fax()
    {
        string Patron_Fax = @"^[0-9]{2,3}-? ?[0-9]{6,8}$";

        if (Txt_Fax_Empleado.Text != null) return Regex.IsMatch(Txt_Fax_Empleado.Text, Patron_Fax);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_CURP
    /// DESCRIPCION : Valida el Fax Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_CURP()
    {
        string Patron_Curp = @"^[a-zA-Z]{4}(\d{6})([a-zA-Z]{6})(\d{2})?$";

        if (Txt_CURP_Empleado.Text != null) return Regex.IsMatch(Txt_CURP_Empleado.Text, Patron_Curp);
        else return false;
    }
    /// ********************************************************************************
    /// Nombre: Validar_Datos
    /// Descripción: Validar Campos
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "";

        ///-------------------------------  Datos Generales  --------------------------------------------
        if (Txt_No_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ El No. de Empleado <br>";
            Datos_Validos = false;
        }
        if (Cmb_Estatus_Empleado.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ El Estatus del Empleado <br>";
            Datos_Validos = false;
        }
        else if(Cmb_Estatus_Empleado.SelectedItem.Text.Trim().Equals("INACTIVO"))
        {
            Lbl_Mensaje_Error.Text += "+ No es posible modificar ningun dato del empleado si su estatus es inactivo. <br>";
            Datos_Validos = false;
        }


        if (Txt_Nombre_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ El Nombre del Empleado <br>";
            Datos_Validos = false;
        }
        //if (Cmb_Roles_Empleado.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "+ El Rol del Empleado <br>";
        //    Datos_Validos = false;
        //}
        //if (Txt_Apellido_Paterno_Empleado.Text == "")
        //{
        //    Lbl_Mensaje_Error.Text += "+ El Apellido Paterno del Empleado <br>";
        //    Datos_Validos = false;
        //}
        //if (string.IsNullOrEmpty(Txt_Apellido_Materno_Empleado.Text))
        //{
        //    Lbl_Mensaje_Error.Text += "+ El Apellido Materno del Empleado <br>";
        //    Datos_Validos = false;
        //}
        //if (string.IsNullOrEmpty(Txt_Password_Empleado.Text))
        //{
        //    Lbl_Mensaje_Error.Text += "+ Password del Empleado <br>";
        //    Datos_Validos = false;
        //}
        //if (string.IsNullOrEmpty(Txt_Confirma_Password_Empleado.Text))
        //{
        //    Lbl_Mensaje_Error.Text += "+ Confirmacion del Password del Empleado <br>";
        //    Datos_Validos = false;
        //}
        if (string.IsNullOrEmpty(Txt_Comentarios_Empleado.Text))
        {
            Lbl_Mensaje_Error.Text += "+ Comentarios <br>";
            Datos_Validos = false;
        }
        if (Txt_Comentarios_Empleado.Text.Length > 250)
        {
            Lbl_Mensaje_Error.Text += "+ Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
            Datos_Validos = false;
        }
        ///---------------------------------------  Datos Personales  -----------------------------------------------
        if (string.IsNullOrEmpty(Txt_Fecha_Nacimiento_Empleado.Text) || (Txt_Fecha_Nacimiento_Empleado.Text.Trim().Equals("__/___/____")))
        {
            Lbl_Mensaje_Error.Text += "+ La Fecha de Nacimiento del Empleado <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Nacimiento_Empleado.Text.Trim()))
        {
            Txt_Fecha_Nacimiento_Empleado.Text = "";
            Lbl_Mensaje_Error.Text += "+ Formato de Fecha de Nacimiento Incorrecto <br>";
            Datos_Validos = false;
        }

        if (Cmb_Sexo_Empleado.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += "+ El Sexo del Empleado <br>";
            Datos_Validos = false;
        }
        if (!string.IsNullOrEmpty(Txt_RFC_Empleado.Text))
        {
            if (!Validar_RFC())
            {
                Lbl_Mensaje_Error.Text += "+ Formato del RFC Incorrecto <br>";
                Datos_Validos = false;
            }
        }
        if (!string.IsNullOrEmpty(Txt_CURP_Empleado.Text))
        {
            if (!Validar_CURP())
            {
                Lbl_Mensaje_Error.Text += "+ Formato del CURP Incorrecto <br>";
                Datos_Validos = false;
            }
        }
        if (Txt_Domicilio_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ El Domicilio del Empleado <br>";
            Datos_Validos = false;
        }
        if (Txt_Colonia_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ La Colonia del Empleado <br>";
            Datos_Validos = false;
        }
        if (!string.IsNullOrEmpty(Txt_Codigo_Postal_Empleado.Text))
        {
            if (!Validar_Codigo_Postal())
            {
                Lbl_Mensaje_Error.Text += "+ Formato del Codigo Postal Incorrecto <br>";
                Datos_Validos = false;
            }
        }
        if (Txt_Ciudad_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ La Ciudad del Empleado <br>";
            Datos_Validos = false;
        }
        if (Txt_Estado_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ El Estado del Empleado <br>";
            Datos_Validos = false;
        }
        if (!string.IsNullOrEmpty(Txt_Telefono_Casa_Empleado.Text))
        {
            if (!Validar_Telefono_Casa_Empleado())
            {
                Lbl_Mensaje_Error.Text += "+ Formato del Telefono de Casa Incorrecto <br>";
                Datos_Validos = false;
            }
        }
        if (!string.IsNullOrEmpty(Txt_Celular_Empleado.Text))
        {
            if (!Validar_Celular_Empleado())
            {
                Lbl_Mensaje_Error.Text += "+ Formato del Celular Incorrecto <br>";
                Datos_Validos = false;
            }
        }
        if (!string.IsNullOrEmpty(Txt_Telefono_Oficina_Empleado.Text))
        {
            if (!Validar_Telefono_Oficina_Empleado())
            {
                Lbl_Mensaje_Error.Text += "+ Formato del Telefono de Oficina Incorrecto <br>";
                Datos_Validos = false;
            }
        }
        if (!string.IsNullOrEmpty(Txt_Fax_Empleado.Text))
        {
            if (!Validar_Fax())
            {
                Lbl_Mensaje_Error.Text += "+ Formato del Fax Incorrecto <br>";
                Datos_Validos = false;
            }
        }
        if (!string.IsNullOrEmpty(Txt_Correo_Electronico_Empleado.Text))
        {
            if (!Validar_Email())
            {
                Lbl_Mensaje_Error.Text += "+ Formato del Correo Electronico Incorrecto <br>";
                Datos_Validos = false;
            }
        }
        ///----------------------------------------  Datos Presidencia  ----------------------------------
        //if (Cmb_Areas_Empleado.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "+ El Área a la cual pertenece el Empleado <br>";
        //    Datos_Validos = false;
        //}
        //if (Cmb_Tipo_Contrato.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "+ Tipo de Contrato al cual pertenece el Empleado <br>";
        //    Datos_Validos = false;
        //}
        if (Cmb_Puestos.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ Puestos al cual pertenece el Empleado <br>";
            Datos_Validos = false;
        }
        if (Cmb_Escolaridad.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ Escolaridad que tiene el Empleado <br>";
            Datos_Validos = false;
        }

        if (Cmb_Turno.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ Turno al cual pertenece el Empleado <br>";
            Datos_Validos = false;
        }
        if (Cmb_Zona.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ Zona al cual pertenece el Empleado <br>";
            Datos_Validos = false;
        }
        //if (Cmb_Tipo_Trabajador.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "+ Tipo de Trabajador al cual pertenece el Empleado <br>";
        //    Datos_Validos = false;
        //}
        ///------------------------------------------  Recursos Humanos  ----------------------------------------
        if (Cmb_Tipo_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ Tipo de Nomina al cual pertenece el Empleado <br>";
            Datos_Validos = false;
        }
        if (Txt_No_IMSS.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "+ Número de IMSS <br>";
            Datos_Validos = false;
        }
        //if (Cmb_Forma_Pago.SelectedIndex <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "+ Forma de Pago <br>";
        //    Datos_Validos = false;
        //}
        //if (Txt_Cuenta_Bancaria.Text.Equals(""))
        //{
        //    Lbl_Mensaje_Error.Text += "+ Número de Cuenta Bancaria <br>";
        //    Datos_Validos = false;
        //}
        if (string.IsNullOrEmpty(Txt_Fecha_Inicio.Text) || (Txt_Fecha_Inicio.Text.Trim().Equals("__/___/____")))
        {
            Lbl_Mensaje_Error.Text += "+ Fecha Inicio Contrato <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Inicio.Text.Trim()))
        {
            Txt_Fecha_Inicio.Text = "";
            Lbl_Mensaje_Error.Text += "+ Formato de Fecha Inicio Contrato Incorrecto <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Termino.Text) || (Txt_Fecha_Termino.Text.Trim().Equals("__/___/____")))
        {
            Txt_Fecha_Termino.Text = "";
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Termino.Text.Trim()))
        {
            Txt_Fecha_Termino.Text = "";
            Lbl_Mensaje_Error.Text += "+ Formato de Fecha Termino Contrato Incorrecto <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Baja.Text) || (Txt_Fecha_Baja.Text.Trim().Equals("__/___/____")))
        {
            Txt_Fecha_Baja.Text = "";
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Baja.Text.Trim()))
        {
            Txt_Fecha_Baja.Text = "";
            Lbl_Mensaje_Error.Text += "+ Formato de Fecha Baja Contrato Incorrecto <br>";
            Datos_Validos = false;
        }

        if (Txt_Salario_Diario.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "+ Salario Diario <br>";
            Datos_Validos = false;
        }
        if (Txt_Salario_Diario_Integrado.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "+ Salario Diario Integrado <br>";
            Datos_Validos = false;
        }

        //if (string.IsNullOrEmpty(Txt_No_Licencia.Text.Trim()))
        //{
        //    Lbl_Mensaje_Error.Text += "+ No de Licencia <br>";
        //    Datos_Validos = false;
        //}

        if (Cmb_Reloj_Checador.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ Aplica Reloj Checador. <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Vencimiento_Licencia.Text.Trim()) || (Txt_Fecha_Vencimiento_Licencia.Text.Trim().Equals("__/___/____")))
        {
            Txt_Fecha_Vencimiento_Licencia.Text = "";
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Vencimiento_Licencia.Text.Trim()))
        {
            Txt_Fecha_Vencimiento_Licencia.Text = "";
            Lbl_Mensaje_Error.Text += "+ Formato de Fecha Vencimiento Licencia <br>";
            Datos_Validos = false;
        }

        ///----------------------------------------  Validar Rango de Fechas  -------------------------------------------------
        if (!string.IsNullOrEmpty(Txt_Fecha_Inicio.Text) && !string.IsNullOrEmpty(Txt_Fecha_Termino.Text))
        {
            if ((Validar_Formato_Fecha(Txt_Fecha_Inicio.Text.Trim())) && (Validar_Formato_Fecha(Txt_Fecha_Termino.Text.Trim())))
            {
                if (!Validar_Fechas(Txt_Fecha_Inicio.Text, Txt_Fecha_Termino.Text))
                {
                    Lbl_Mensaje_Error.Text += "+ Fecha Termino Contrato no puede ser menor que la Fecha Inicio Contrato <br>";
                    Datos_Validos = false;

                }
            }
        }

        //------------------------------ SAP Código Programático  ----------------------------------------------
        if (Cmb_SAP_Fuente_Financiamiento.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ Seleccione Fuente de Financiamiento. <br>";
            Datos_Validos = false;
        }

        if (Cmb_SAP_Area_Funcional.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ Seleccione Fuente de Área Funcional. <br>";
            Datos_Validos = false;
        }

        if (Cmb_SAP_Programas.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ Seleccione Programa. <br>";
            Datos_Validos = false;
        }

        if (Cmb_SAP_Unidad_Responsable.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ Seleccione Unidad Responsable. <br>";
            Datos_Validos = false;
        }

        if (Cmb_SAP_Partida.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ Seleccione Partida. <br>";
            Datos_Validos = false;
        }
        //-------------------------------------------------------------------------------------------------------

        Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("Es necesario Introducir: <br>" + Crear_Tabla_Mostrar_Errores_Pagina(Lbl_Mensaje_Error.Text));
        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Formato_Fecha
    /// DESCRIPCION : Valida el formato de las fechas.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Formato_Fecha(String Fecha)
    {
        String Cadena_Fecha = @"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$";
        if (Fecha != null)
        {
            return Regex.IsMatch(Fecha, Cadena_Fecha);
        }
        else
        {
            return false;
        }
    }
    /// ********************************************************************************
    /// Nombre: Validar_Fechas
    /// Descripción: Valida que la Fecha Inicial no sea mayor que la Final
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private Boolean Validar_Fechas(String _Fecha_Inicio, String _Fecha_Fin)
    {
        DateTime Fecha_Inicio = Convert.ToDateTime(_Fecha_Inicio);
        DateTime Fecha_Fin = Convert.ToDateTime(_Fecha_Fin);
        Boolean Fecha_Valida = false;
        if (Fecha_Inicio < Fecha_Fin) Fecha_Valida = true;
        return Fecha_Valida;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 18/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean IsNumeric(String Cadena)
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

    #region (Modulo Cat_Nom_Empl_Perc_Dedu_Det [Control Tipos Nomina (Percepciones y/o Deducciones)])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Grid_Tipo_Nomina_Percepciones_Deducciones
    /// DESCRIPCION : Cargar el grid con los datos pasados como parámetros.
    /// 
    /// PARAMETROS  : Dt_Datos.- Tabla con la informacion a cargar en el GridView
    ///               GridView_General .- GridView donde serán mostrados los datos.
    ///               
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 05/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Grid_Tipo_Nomina_Percepciones_Deducciones(DataTable Dt_Datos, GridView GridView_General, String TextBox_ID)
    {
        GridView_General.Columns[4].Visible = true;
        GridView_General.DataSource = Dt_Datos;
        GridView_General.DataBind();
        GridView_General.SelectedIndex = -1;
        GridView_General.Columns[4].Visible = false;

        Checked_UnChecked_Grid_Percepciones_Deducciones(GridView_General, Dt_Datos, TextBox_ID);

        for (int Contador_Fila = 0; Contador_Fila < GridView_General.Rows.Count; Contador_Fila++)
        {
            if (!((CheckBox)GridView_General.Rows[Contador_Fila].Cells[0].FindControl("Chk_Aplica")).Checked)
            {
                GridView_General.Rows[Contador_Fila].Visible = false;
            }
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Crear_DataTable_Percepciones_Deducciones
    /// DESCRIPCION : Crea una tabla con las percepciones y deducciones que aplican para 
    ///               el empleado.
    /// 
    /// PARAMETROS  : Grid_Tipo_Nomina_Percepciones_Deducciones.- Grid de donde se tomará
    ///               la información para generar la tabvle de percepciones deducciones
    ///               que le aplicaran al empleado.
    ///               
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 05/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Crear_DataTable_Percepciones_Deducciones(GridView Grid_Tipo_Nomina_Percepciones_Deducciones, String TextBox_ID)
    {
        DataTable Dt_Percepciones_Deducciones_Aplican = null;//Variable que almacenara una lista de percepciones deducciones que aplican para el empleado.
        Boolean Aplica = false;//Variable que almacenara si la percepcion o deduccion aplica al empleado o no.
        Double Cantidad = 0.0;//Variable que almacenar la cantidad de la percepcio y/o deducción.
        String Cantidad_Text = "";
        try
        {
            Dt_Percepciones_Deducciones_Aplican = new DataTable();
            Dt_Percepciones_Deducciones_Aplican.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID, typeof(String));
            Dt_Percepciones_Deducciones_Aplican.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Nombre, typeof(String));
            Dt_Percepciones_Deducciones_Aplican.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad, typeof(String));

            for (int Contador_Fila = 0; Contador_Fila < Grid_Tipo_Nomina_Percepciones_Deducciones.Rows.Count; Contador_Fila++)
            {
                Aplica = ((CheckBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[0].FindControl("Chk_Aplica")).Checked;
                if (Aplica)
                {
                    DataRow Renglon = Dt_Percepciones_Deducciones_Aplican.NewRow();
                    Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID] = Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[1].Text;
                    Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] = Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[2].Text;
                    Cantidad_Text = ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[5].FindControl(TextBox_ID)).Text;
                    Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad] = ((!String.IsNullOrEmpty(Cantidad_Text)) && (!Cantidad_Text.Equals("$  _,___,___.__"))) ? Convert.ToDouble(Cantidad_Text) : 0;
                    Dt_Percepciones_Deducciones_Aplican.Rows.Add(Renglon);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar al el el DataTable Percepciones Deducciones. Error: [" + Ex.Message + "]");
        }
        return Dt_Percepciones_Deducciones_Aplican;
    }
    ///****************************************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Grid_Percepciones_Deducciones
    /// DESCRIPCION : Checked o UnChecked la percepción o deduccion del grid de Tipo Nómina
    ///               Percepciones o Deducciones
    /// 
    /// PARAMETROS  : Dt_Datos.- Tabla con la informacion a cargar en el GridView
    ///               GridView_General .- GridView donde serán mostrados los datos.
    ///               
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 05/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///****************************************************************************************************
    private void Checked_UnChecked_Grid_Percepciones_Deducciones(GridView Grid_Tipo_Nomina_Percepciones_Deducciones, DataTable Dt_Datos, String TextBox_ID)
    {
        Boolean Aplica = false;//Variable que almacenara si la percepcion o deduccion aplica al empleado o no.
        try
        {
            for (int Contador_Fila = 0; Contador_Fila < Grid_Tipo_Nomina_Percepciones_Deducciones.Rows.Count; Contador_Fila++)
            {
                ((CheckBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[0].FindControl("Chk_Aplica")).Checked = false;

                foreach (DataRow Renglon in Dt_Datos.Rows)
                {
                    ((CheckBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[0].FindControl("Chk_Aplica")).Checked = false;

                    if (Renglon[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Equals(Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[1].Text))
                    {
                        ((CheckBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[0].FindControl("Chk_Aplica")).Checked = true;
                        ((TextBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[5].FindControl(TextBox_ID)).Text = string.Format("{0:#,###,###.00}", Convert.ToDouble(Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()));
                        break;
                    }
                }
            }


            for (int Contador_Fila = 0; Contador_Fila < Grid_Tipo_Nomina_Percepciones_Deducciones.Rows.Count; Contador_Fila++)
            {
                if (!((CheckBox)Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Cells[0].FindControl("Chk_Aplica")).Checked) {
                    Grid_Tipo_Nomina_Percepciones_Deducciones.Rows[Contador_Fila].Visible = false;
                }               
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar al el el DataTable Percepciones Deducciones. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Percepciones_Deducciones_Sindicato
    /// DESCRIPCION : Obtiene las Percepciones y deducciones correspondientes al sindicato
    ///               seleccionado.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 05/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Obtener_Percepciones_Deducciones_Sindicato(String Sindicato_ID, String Tipo)
    {
        Cls_Cat_Nom_Sindicatos_Negocio Cls_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Percepciones_Deducciones_Sindicato = null;//Lista de percepciones o deducciones del sindicato.
        try
        {
            Cls_Sindicatos.P_Tipo_Percepcion = Tipo;
            Cls_Sindicatos.P_Sindicato_ID = Sindicato_ID;
            Dt_Percepciones_Deducciones_Sindicato = Cls_Sindicatos.Consultar_Percepciones_Deducciones();//consultamos
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        return Dt_Percepciones_Deducciones_Sindicato;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Percepciones_Deducciones_Tipo_Nomina
    /// DESCRIPCION : Obtiene las Percepciones y deducciones correspondientes al Tipo Nomina
    ///               seleccionado.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 05/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Obtener_Percepciones_Deducciones_Tipo_Nomina(String Tipo_Nomina_ID, String Tipo)
    {
        Cls_Cat_Tipos_Nominas_Negocio Consulta_Percepciones_Deducciones = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Tipo_Nomina_Percepciones_Deducciones = null;//variable que almacenara una lista de percepciones.
        try
        {
            Consulta_Percepciones_Deducciones.P_Tipo = Tipo;
            Consulta_Percepciones_Deducciones.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
            Dt_Tipo_Nomina_Percepciones_Deducciones = Consulta_Percepciones_Deducciones.Consulta_Percepciones_Deducciones_Nomina();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        return Dt_Tipo_Nomina_Percepciones_Deducciones;
    }
    #endregion

    #region (Codigo Programatico)
    private void Consultar_SAP_Unidades_Responsables()
    {
        Cls_Cat_Dependencias_Negocio Obj_Unidades_Responsables = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Unidades_Responsables = null;//Variable que lista las unidades responsables registrdas en sistema.

        try
        {
            Dt_Unidades_Responsables = Obj_Unidades_Responsables.Consulta_Dependencias();
            Cmb_SAP_Unidad_Responsable.DataSource = Dt_Unidades_Responsables;
            Cmb_SAP_Unidad_Responsable.DataTextField = "CLAVE_NOMBRE";
            Cmb_SAP_Unidad_Responsable.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_SAP_Unidad_Responsable.DataBind();
            Cmb_SAP_Unidad_Responsable.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_SAP_Unidad_Responsable.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
        }
    }

    private void Consultar_SAP_Fuentes_Financiamiento(String Dependencia_ID)
    {
        Cls_Cat_Dependencias_Negocio Obj_Fte_Financiamiento = new Cls_Cat_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Fte_Financiamiento = null;//Variable que almacenara los resultados de la busqueda realizada.

        try
        {
            Obj_Fte_Financiamiento.P_Dependencia_ID = Dependencia_ID;
            Dt_Fte_Financiamiento = Obj_Fte_Financiamiento.Consultar_Sap_Det_Fte_Dependencia();
            Cmb_SAP_Fuente_Financiamiento.DataSource = Dt_Fte_Financiamiento;
            Cmb_SAP_Fuente_Financiamiento.DataTextField = "FTE_FINANCIAMIENTO";
            Cmb_SAP_Fuente_Financiamiento.DataValueField = Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
            Cmb_SAP_Fuente_Financiamiento.DataBind();

            Cmb_SAP_Fuente_Financiamiento.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_SAP_Fuente_Financiamiento.SelectedIndex = -1;

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las fuentes de financiamento registradas actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
    }

    private void Consulta_SAP_Programas(String Dependencia_ID)
    {
        Cls_Cat_Dependencias_Negocio Obj_Programas = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios
        DataTable Dt_Programas = null;//Variable que alamacenara los resultados obtenidos de la busqueda realizada.

        try
        {
            Obj_Programas.P_Dependencia_ID = Dependencia_ID;
            Dt_Programas = Obj_Programas.Consultar_Sap_Det_Prog_Dependencia();
            Cmb_SAP_Programas.DataSource = Dt_Programas;
            Cmb_SAP_Programas.DataTextField = "PROYECTO_PROGRAMA";
            Cmb_SAP_Programas.DataValueField = Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
            Cmb_SAP_Programas.DataBind();

            Cmb_SAP_Programas.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_SAP_Programas.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los programas registrados actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
    }

    private void Consultar_SAP_Partidas(String Programa_ID)
    {
        Cls_Cat_Empleados_Negocios Obj_Partidas = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Partidas = null;

        try
        {
            Dt_Partidas = Obj_Partidas.Consultar_Partidas(Programa_ID);
            Cmb_SAP_Partida.DataSource = Dt_Partidas;
            Cmb_SAP_Partida.DataValueField = Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
            Cmb_SAP_Partida.DataTextField = "PARTIDA";
            Cmb_SAP_Partida.DataBind();
            Cmb_SAP_Partida.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_SAP_Partida.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las partidas registrdas en el sistema. Error: [" + Ex.Message + "]");
        }
    }

    private void Consultar_SAP_Areas_Funcionales(String Area_Funcional_ID)
    {
        Cls_Cat_Dependencias_Negocio Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Areas_Funcionales = null;                                             //Variable que almacena un listado de areas funcionales registradas actualmente en el sistema.

        try
        {
            Dt_Areas_Funcionales = Obj_Dependencias.Consulta_Area_Funcional();
            Cmb_SAP_Area_Funcional.DataSource = Dt_Areas_Funcionales;
            Cmb_SAP_Area_Funcional.DataTextField = "Clave_Nombre";
            Cmb_SAP_Area_Funcional.DataValueField = Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID;
            Cmb_SAP_Area_Funcional.DataBind();
            // Area_Funcional_ID
            Cmb_SAP_Area_Funcional.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
            Cmb_SAP_Area_Funcional.SelectedIndex = Cmb_SAP_Area_Funcional.Items.IndexOf(Cmb_SAP_Area_Funcional.Items.FindByValue(Area_Funcional_ID));

            if (Dt_Areas_Funcionales is DataTable)
            {
                if (Dt_Areas_Funcionales.Rows.Count > 0)
                {
                    foreach (DataRow AREAS_FUNCIONALES in Dt_Areas_Funcionales.Rows)
                    {
                        if (AREAS_FUNCIONALES is DataRow)
                        {
                            if (!String.IsNullOrEmpty(AREAS_FUNCIONALES[Cat_SAP_Area_Funcional.Campo_Clave].ToString()))
                                Txt_SAP_Area_Responsable.Text = AREAS_FUNCIONALES[Cat_SAP_Area_Funcional.Campo_Clave].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las áreas funcionales registradas actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Grid)

    #region (Grid Empleados)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Empleados_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del Empleado que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Empleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios Rs_Consulta_Cat_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión a la capa de Negocios para la consulta de los datos del empleado
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Empleados.P_Empleado_ID = Grid_Empleados.SelectedRow.Cells[1].Text;
            Dt_Empleados = Rs_Consulta_Cat_Empleados.Consulta_Datos_Empleado(); //Consulta los datos del empleado que fue seleccionado por el usuario
            if (Dt_Empleados.Rows.Count > 0)
            {
                //Agrega los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Dt_Empleados.Rows)
                {
                    Img_Foto_Empleado.ImageUrl = (@Registro[Cat_Empleados.Campo_Ruta_Foto].ToString().Equals("")) ? "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" : @Registro[Cat_Empleados.Campo_Ruta_Foto].ToString();
                    Img_Foto_Empleado.DataBind();
                    Txt_Ruta_Foto.Value = (@Registro[Cat_Empleados.Campo_Ruta_Foto].ToString().Equals("")) ? "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" : @Registro[Cat_Empleados.Campo_Ruta_Foto].ToString();

                    Txt_Empleado_ID.Text = Registro[Cat_Empleados.Campo_Empleado_ID].ToString();
                    if (Registro[Cat_Empleados.Campo_No_Empleado].ToString() != null) Txt_No_Empleado.Text = Registro[Cat_Empleados.Campo_No_Empleado].ToString();
                    if (Registro[Cat_Empleados.Campo_Estatus].ToString() != null) Cmb_Estatus_Empleado.SelectedValue = Registro[Cat_Empleados.Campo_Estatus].ToString();
                    if (Registro[Cat_Empleados.Campo_Nombre].ToString() != null) Txt_Nombre_Empleado.Text = Registro[Cat_Empleados.Campo_Nombre].ToString();
                    if (Registro[Cat_Empleados.Campo_Apellido_Paterno].ToString() != null) Txt_Apellido_Paterno_Empleado.Text = Registro[Cat_Empleados.Campo_Apellido_Paterno].ToString();
                    if (Registro[Cat_Empleados.Campo_Apellido_Materno].ToString() != null) Txt_Apellido_Materno_Empleado.Text = Registro[Cat_Empleados.Campo_Apellido_Materno].ToString();

                    if (!String.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Confronto].ToString())) Txt_Empleado_Confronto.Text = Registro[Cat_Empleados.Campo_Confronto].ToString();

                    if (Registro[Cat_Empleados.Campo_Rol_ID].ToString() != "") Cmb_Roles_Empleado.SelectedValue = Registro[Cat_Empleados.Campo_Rol_ID].ToString();
                    if (Registro[Cat_Empleados.Campo_Fecha_Nacimiento].ToString() != null) Txt_Fecha_Nacimiento_Empleado.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Empleados.Campo_Fecha_Nacimiento].ToString()));
                    if (Registro[Cat_Empleados.Campo_Sexo].ToString() != null)
                    {
                        Cmb_Sexo_Empleado.SelectedValue = Registro[Cat_Empleados.Campo_Sexo].ToString();
                    }
                    else
                    {
                        Cmb_Sexo_Empleado.SelectedIndex = 0;
                    }
                    if (Registro[Cat_Empleados.Campo_RFC].ToString() != null) Txt_RFC_Empleado.Text = Registro[Cat_Empleados.Campo_RFC].ToString();
                    if (Registro[Cat_Empleados.Campo_CURP].ToString() != null) Txt_CURP_Empleado.Text = Registro[Cat_Empleados.Campo_CURP].ToString();
                    if (Registro[Cat_Empleados.Campo_Calle].ToString() != null) Txt_Domicilio_Empleado.Text = Registro[Cat_Empleados.Campo_Calle].ToString();
                    if (Registro[Cat_Empleados.Campo_Colonia].ToString() != null) Txt_Colonia_Empleado.Text = Registro[Cat_Empleados.Campo_Colonia].ToString();
                    if (Registro[Cat_Empleados.Campo_Codigo_Postal].ToString() != null) Txt_Codigo_Postal_Empleado.Text = Registro[Cat_Empleados.Campo_Codigo_Postal].ToString();
                    if (Registro[Cat_Empleados.Campo_Ciudad].ToString() != null) Txt_Ciudad_Empleado.Text = Registro[Cat_Empleados.Campo_Ciudad].ToString();
                    if (Registro[Cat_Empleados.Campo_Estado].ToString() != null) Txt_Estado_Empleado.Text = Registro[Cat_Empleados.Campo_Estado].ToString();
                    if (Registro[Cat_Empleados.Campo_Telefono_Casa].ToString() != null) Txt_Telefono_Casa_Empleado.Text = Registro[Cat_Empleados.Campo_Telefono_Casa].ToString();
                    if (Registro[Cat_Empleados.Campo_Celular].ToString() != null) Txt_Celular_Empleado.Text = Registro[Cat_Empleados.Campo_Celular].ToString();
                    if (Registro[Cat_Empleados.Campo_Nextel].ToString() != null) Txt_Nextel_Empleado.Text = Registro[Cat_Empleados.Campo_Nextel].ToString();
                    if (Registro[Cat_Empleados.Campo_Telefono_Oficina].ToString() != null) Txt_Telefono_Oficina_Empleado.Text = Registro[Cat_Empleados.Campo_Telefono_Oficina].ToString();
                    if (Registro[Cat_Empleados.Campo_Extension].ToString() != null) Txt_Extension_Empleado.Text = Registro[Cat_Empleados.Campo_Extension].ToString();
                    if (Registro[Cat_Empleados.Campo_Fax].ToString() != null) Txt_Fax_Empleado.Text = Registro[Cat_Empleados.Campo_Fax].ToString();
                    if (Registro[Cat_Empleados.Campo_Correo_Electronico].ToString() != null) Txt_Correo_Electronico_Empleado.Text = Registro[Cat_Empleados.Campo_Correo_Electronico].ToString();
                    if (Registro[Cat_Empleados.Campo_Comentarios].ToString() != null) Txt_Comentarios_Empleado.Text = Registro[Cat_Empleados.Campo_Comentarios].ToString();

                    ///Datos Presidencia
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Tipo_Contrato_ID].ToString())) Cmb_Tipo_Contrato.SelectedIndex = Cmb_Tipo_Contrato.Items.IndexOf(Cmb_Tipo_Contrato.Items.FindByValue(Registro[Cat_Empleados.Campo_Tipo_Contrato_ID].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Escolaridad_ID].ToString())) Cmb_Escolaridad.SelectedIndex = Cmb_Escolaridad.Items.IndexOf(Cmb_Escolaridad.Items.FindByValue(Registro[Cat_Empleados.Campo_Escolaridad_ID].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Sindicato_ID].ToString())) Cmb_Sindicato.SelectedIndex = Cmb_Sindicato.Items.IndexOf(Cmb_Sindicato.Items.FindByValue(Registro[Cat_Empleados.Campo_Sindicato_ID].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Turno_ID].ToString())) Cmb_Turno.SelectedIndex = Cmb_Turno.Items.IndexOf(Cmb_Turno.Items.FindByValue(Registro[Cat_Empleados.Campo_Turno_ID].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Zona_ID].ToString())) Cmb_Zona.SelectedIndex = Cmb_Zona.Items.IndexOf(Cmb_Zona.Items.FindByValue(Registro[Cat_Empleados.Campo_Zona_ID].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Tipo_Trabajador_ID].ToString())) Cmb_Tipo_Trabajador.SelectedIndex = Cmb_Tipo_Trabajador.Items.IndexOf(Cmb_Tipo_Trabajador.Items.FindByValue(Registro[Cat_Empleados.Campo_Tipo_Trabajador_ID].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Terceros_ID].ToString())) Cmb_Terceros.SelectedIndex = Cmb_Terceros.Items.IndexOf(Cmb_Terceros.Items.FindByValue(Registro[Cat_Empleados.Campo_Terceros_ID].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_No_Tarjeta].ToString())) Txt_No_Tarjeta.Text = Registro[Cat_Empleados.Campo_No_Tarjeta].ToString();
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_No_Seguro].ToString())) Txt_No_Seguro_Poliza_Empleado.Text = Registro[Cat_Empleados.Campo_No_Seguro].ToString();
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Beneficiario].ToString())) Txt_Beneficiario_Empleado.Text = Registro[Cat_Empleados.Campo_Beneficiario].ToString();

                    ///Recursos Humanos
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString()))
                    {
                        Cmb_Tipo_Nomina.SelectedIndex = Cmb_Tipo_Nomina.Items.IndexOf(Cmb_Tipo_Nomina.Items.FindByValue(Registro[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString()));
                        DataTable Dt_Tipo_Nomina_Percepciones = Obtener_Percepciones_Deducciones_Tipo_Nomina(Cmb_Tipo_Nomina.SelectedValue.Trim(), "PERCEPCION");
                        Cargar_Grid_Tipo_Nomina_Percepciones_Deducciones(Dt_Tipo_Nomina_Percepciones, Grid_Tipo_Nomina_Percepciones, "Txt_Cantidad_Percepcion");
                        //..
                        Cargar_Percepciones_Deducciones(Dt_Tipo_Nomina_Percepciones, Cmb_Percepciones_All);

                        DataTable Dt_Tipo_Nomina_Deducciones = Obtener_Percepciones_Deducciones_Tipo_Nomina(Cmb_Tipo_Nomina.SelectedValue.Trim(), "DEDUCCION");
                        Cargar_Grid_Tipo_Nomina_Percepciones_Deducciones(Dt_Tipo_Nomina_Deducciones, Grid_Tipo_Nomina_Deducciones, "Txt_Cantidad_Deduccion");
                        //..
                        Cargar_Percepciones_Deducciones(Dt_Tipo_Nomina_Deducciones, Cmb_Deducciones_All);

                        Rs_Consulta_Cat_Empleados.P_Empleado_ID = Grid_Empleados.SelectedRow.Cells[1].Text;
                        DataTable Dt_Percepciones_Deducciones_Tipo_Nomina = Rs_Consulta_Cat_Empleados.Consultar_Percepciones_Deducciones_Tipo_Nomina();

                        Checked_UnChecked_Grid_Percepciones_Deducciones(Grid_Tipo_Nomina_Percepciones, Dt_Percepciones_Deducciones_Tipo_Nomina, "Txt_Cantidad_Percepcion");
                        Checked_UnChecked_Grid_Percepciones_Deducciones(Grid_Tipo_Nomina_Deducciones, Dt_Percepciones_Deducciones_Tipo_Nomina, "Txt_Cantidad_Deduccion");
                    }
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_No_IMSS].ToString())) Txt_No_IMSS.Text = Registro[Cat_Empleados.Campo_No_IMSS].ToString();
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Forma_Pago].ToString())) Cmb_Forma_Pago.SelectedIndex = Cmb_Forma_Pago.Items.IndexOf(Cmb_Forma_Pago.Items.FindByValue(Registro[Cat_Empleados.Campo_Forma_Pago].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString())) Txt_Cuenta_Bancaria.Text = Registro[Cat_Empleados.Campo_No_Cuenta_Bancaria].ToString();
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Fecha_Inicio].ToString())) Txt_Fecha_Inicio.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Empleados.Campo_Fecha_Inicio].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Fecha_Termino_Contrato].ToString())) Txt_Fecha_Termino.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Empleados.Campo_Fecha_Termino_Contrato].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Tipo_Finiquito].ToString())) Cmb_Tipo_Finiquito.SelectedIndex = Cmb_Tipo_Finiquito.Items.IndexOf(Cmb_Tipo_Finiquito.Items.FindByValue(Registro[Cat_Empleados.Campo_Tipo_Finiquito].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Fecha_Baja].ToString())) Txt_Fecha_Baja.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Empleados.Campo_Fecha_Baja].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Salario_Diario].ToString())) Txt_Salario_Diario.Text = String.Format("{0:#,###,###.00}", Convert.ToDouble(Registro[Cat_Empleados.Campo_Salario_Diario].ToString().Replace("$", "")));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Salario_Diario_Integrado].ToString())) Txt_Salario_Diario_Integrado.Text = String.Format("{0:#,###,###.00}", Convert.ToDouble(Registro[Cat_Empleados.Campo_Salario_Diario_Integrado].ToString().Replace("$", "")));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_No_Licencia_Manejo].ToString())) Txt_No_Licencia.Text = Registro[Cat_Empleados.Campo_No_Licencia_Manejo].ToString();
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Fecha_Vencimiento_Licencia].ToString())) Txt_Fecha_Vencimiento_Licencia.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Empleados.Campo_Fecha_Vencimiento_Licencia].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Banco_ID].ToString()))
                        Cmb_Bancos.SelectedIndex = Cmb_Bancos.Items.IndexOf(Cmb_Bancos.Items.FindByValue(Registro[Cat_Empleados.Campo_Banco_ID].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Reloj_Checador].ToString()))
                        Cmb_Reloj_Checador.SelectedIndex = Cmb_Reloj_Checador.Items.IndexOf(Cmb_Reloj_Checador.Items.FindByText(Registro[Cat_Empleados.Campo_Reloj_Checador].ToString()));
                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Cuenta_Contable_ID].ToString())) 
                        Cmb_Cuentas_Contables.SelectedIndex = Cmb_Cuentas_Contables.Items.IndexOf(Cmb_Cuentas_Contables.Items.FindByValue(Registro[Cat_Empleados.Campo_Cuenta_Contable_ID].ToString()));

                    ///Dias Laborales del Empleado
                    Chk_Lunes.Checked = Registro[Cat_Empleados.Campo_Lunes].ToString().Equals("SI") ? true : false;
                    Chk_Martes.Checked = Registro[Cat_Empleados.Campo_Martes].ToString().Equals("SI") ? true : false;
                    Chk_Miercoles.Checked = Registro[Cat_Empleados.Campo_Miercoles].ToString().Equals("SI") ? true : false;
                    Chk_Jueves.Checked = Registro[Cat_Empleados.Campo_Jueves].ToString().Equals("SI") ? true : false;
                    Chk_Viernes.Checked = Registro[Cat_Empleados.Campo_Viernes].ToString().Equals("SI") ? true : false;
                    Chk_Sabado.Checked = Registro[Cat_Empleados.Campo_Sabado].ToString().Equals("SI") ? true : false;
                    Chk_Domingo.Checked = Registro[Cat_Empleados.Campo_Domingo].ToString().Equals("SI") ? true : false;

                    Consulta_Detalles_Requisitos();

                    if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Password].ToString()))
                    {
                        Txt_Password_Empleado.Text = Registro[Cat_Empleados.Campo_Password].ToString();
                        Txt_Password_Empleado.Attributes.Add("value", Txt_Password_Empleado.Text);

                        Txt_Confirma_Password_Empleado.Text = Registro[Cat_Empleados.Campo_Password].ToString();
                        Txt_Confirma_Password_Empleado.Attributes.Add("value", Txt_Confirma_Password_Empleado.Text);
                    }

                    //SAP Código Programático.------------------------------------------------------------------------------------
                    if (!String.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Dependencia_ID].ToString()))
                    {
                        Cmb_SAP_Unidad_Responsable.SelectedIndex = Cmb_SAP_Unidad_Responsable.Items.IndexOf(
                            Cmb_SAP_Unidad_Responsable.Items.FindByValue(Registro[Cat_Empleados.Campo_Dependencia_ID].ToString()));

                        if (!string.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Puesto_ID].ToString()))
                        {
                            Consultar_Puestos(Registro[Cat_Empleados.Campo_Dependencia_ID].ToString(),
                                "DISPONIBLE", Registro[Cat_Empleados.Campo_Puesto_ID].ToString(),
                                Registro[Cat_Empleados.Campo_Empleado_ID].ToString());

                            //Cmb_Puestos.SelectedIndex = Cmb_Puestos.Items.IndexOf(Cmb_Puestos.Items.FindByValue(Registro[Cat_Empleados.Campo_Puesto_ID].ToString()));
                        }

                        Consulta_Areas_Dependencia(Registro[Cat_Empleados.Campo_Dependencia_ID].ToString());
                        if (Registro[Cat_Empleados.Campo_Area_ID].ToString() != null)
                            Cmb_Areas_Empleado.SelectedIndex = Cmb_Areas_Empleado.Items.IndexOf(
                                Cmb_Areas_Empleado.Items.FindByValue(Registro[Cat_Empleados.Campo_Area_ID].ToString()));

                        Consultar_SAP_Fuentes_Financiamiento(Cmb_SAP_Unidad_Responsable.SelectedValue.Trim());
                        if (!String.IsNullOrEmpty(Registro[Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID].ToString()))
                            Cmb_SAP_Fuente_Financiamiento.SelectedIndex = Cmb_SAP_Fuente_Financiamiento.Items.IndexOf(
                                Cmb_SAP_Fuente_Financiamiento.Items.FindByValue(Registro[Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID].ToString()));

                        Consulta_SAP_Programas(Cmb_SAP_Unidad_Responsable.SelectedValue.Trim());
                        if (!String.IsNullOrEmpty(Registro[Cat_Empleados.Campo_SAP_Programa_ID].ToString()))
                        {
                            Cmb_SAP_Programas.SelectedIndex = Cmb_SAP_Programas.Items.IndexOf(
                                Cmb_SAP_Programas.Items.FindByValue(Registro[Cat_Empleados.Campo_SAP_Programa_ID].ToString()));

                            Consultar_SAP_Partidas(Cmb_SAP_Programas.SelectedValue.Trim());
                            if (!String.IsNullOrEmpty(Registro[Cat_Empleados.Campo_SAP_Partida_ID].ToString()))
                            {
                                Cmb_SAP_Partida.SelectedIndex = Cmb_SAP_Partida.Items.IndexOf(
                                    Cmb_SAP_Partida.Items.FindByValue(Registro[Cat_Empleados.Campo_SAP_Partida_ID].ToString()));
                            }
                        }

                        if (!String.IsNullOrEmpty(Registro[Cat_Empleados.Campo_SAP_Area_Responsable_ID].ToString()))
                            Consultar_SAP_Areas_Funcionales(Registro[Cat_Empleados.Campo_SAP_Area_Responsable_ID].ToString());

                        if (!String.IsNullOrEmpty(Registro[Cat_Empleados.Campo_SAP_Codigo_Programatico].ToString()))
                            Cargar_Codigo_Programatico(Registro[Cat_Empleados.Campo_SAP_Codigo_Programatico].ToString());
                        //----------------------------------------------------------------------------------------------------------
                    }

                    if (!String.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Aplica_ISSEG].ToString().Trim()))
                        Chk_Aplica_Esquema_ISSEG.Checked = (Registro[Cat_Empleados.Campo_Aplica_ISSEG].ToString().Trim().Equals("SI")) ?
                            true : false;
                    //Validar_Salario_Diario();
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Empleados_PageIndexChanging
    /// DESCRIPCION : Cambia la pagina de la tabla de empleados
    ///               
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                        //Limpia todos los controles de la forma
            Grid_Empleados.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Empleados();                    //Carga los Empleados que estan asignados a la página seleccionada

            Grid_Empleados.SelectedIndex = -1;
            Img_Foto_Empleado.ImageUrl = "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG";
            Img_Foto_Empleado.DataBind();
            Txt_Ruta_Foto.Value = "";
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Empleados
    /// DESCRIPCION : Llena el grid con los Empleados que fueron obtenidos de la consulta
    ///               Consulta_Empleados
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Empleados()
    {
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Empleados.Columns[1].Visible = true;
            Grid_Empleados.DataBind();
            Dt_Empleados = (DataTable)Session["Consulta_Empleados"];
            Grid_Empleados.DataSource = Dt_Empleados;
            Grid_Empleados.DataBind();
            Grid_Empleados.Columns[1].Visible = false;
            Grid_Empleados.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Empleados " + ex.Message.ToString(), ex);
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Empleados_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Empleados_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Empleados_Avanzada();
        DataTable Dt_Calendario_Nominas = (Grid_Empleados.DataSource as DataTable);

        if (Dt_Calendario_Nominas != null)
        {
            DataView Dv_Calendario_Nominas = new DataView(Dt_Calendario_Nominas);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Calendario_Nominas.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Calendario_Nominas.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Empleados.DataSource = Dv_Calendario_Nominas;
            Grid_Empleados.DataBind();
        }
    }
    #endregion

    #region (Grid Documentos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Documentos_Empleado_RowDataBound
    /// DESCRIPCION : Personalizar las filas, antes de ser renderizadas al usuario.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Documentos_Empleado_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        int _Length_Cadena = 0;
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Verificar si son menus de Parent_ID = 0
                if (e.Row.Cells[4].Text.Equals("N"))
                {
                    e.Row.Cells[2].Enabled = false;
                    e.Row.Cells[3].Enabled = false;
                }
                else if (e.Row.Cells[4].Text.Equals("S"))
                {
                    e.Row.Cells[2].Enabled = true;
                    e.Row.Cells[3].Enabled = true;
                }
                e.Row.Cells[1].Style.Add("color", "Black");
                e.Row.Cells[1].ToolTip = e.Row.Cells[1].Text;
               _Length_Cadena = e.Row.Cells[1].Text.Length;
               if (_Length_Cadena >= 30) {
                   e.Row.Cells[1].Text = e.Row.Cells[1].Text.Substring(0, 29) + "...";
               }

            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error en el Evento Grid_Documentos_Empleado_RowDataBound del grid de Documentos del Empelado. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Grid Percepciones)
    ///********************************************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Tipo_Nomina_Percepciones_RowDataBound
    /// DESCRIPCION : Agregamos algunas validaciones al GridView antes de que sea renderizado
    ///               al usuario.  
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Enero/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///********************************************************************************************************
    protected void Grid_Tipo_Nomina_Percepciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                e.Row.Cells[5].Enabled = false;

                if (e.Row.Cells[3].Text.ToUpper().Equals("FIJA") && e.Row.Cells[4].Text.Trim().Equals("$0.00"))
                {
                    e.Row.Cells[5].Enabled = true;
                }
                else if (!e.Row.Cells[4].Text.Trim().Equals("$0.00") && e.Row.Cells[3].Text.ToUpper().Equals("FIJA"))
                {
                    ((TextBox)e.Row.Cells[5].FindControl("Txt_Cantidad_Percepcion")).Text = string.Format("{0:#,###,###.00}", Convert.ToDouble(e.Row.Cells[4].Text.Replace("$", "").Trim()));
                }
                else if (!e.Row.Cells[3].Text.ToUpper().Equals("FIJA"))
                {
                    e.Row.Cells[4].Text = "";
                }

                if (!e.Row.Cells[3].Text.ToUpper().Equals("FIJA"))
                {
                    ((TextBox)e.Row.Cells[5].FindControl("Txt_Cantidad_Percepcion")).Style.Add("display", "none");
                }

                ((CheckBox)e.Row.Cells[0].FindControl("Chk_Aplica")).ToolTip = "" + e.Row.RowIndex;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Grid Deducciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Tipo_Nomina_Deduccion_RowDataBound
    /// DESCRIPCION : Agregamos algunas validaciones al GridView antes de que sea renderizado
    ///               al usuario. 
    ///               
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Enero/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Tipo_Nomina_Deduccion_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                e.Row.Cells[5].Enabled = false;

                if (e.Row.Cells[3].Text.ToUpper().Equals("FIJA") && e.Row.Cells[4].Text.Trim().Equals("$0.00"))
                {
                    e.Row.Cells[5].Enabled = true;
                }
                else if (!e.Row.Cells[4].Text.Trim().Equals("$0.00") && e.Row.Cells[3].Text.ToUpper().Equals("FIJA"))
                {
                    ((TextBox)e.Row.Cells[5].FindControl("Txt_Cantidad_Deduccion")).Text = string.Format("{0:#,###,###.00}", Convert.ToDouble(e.Row.Cells[4].Text.Replace("$", "").Trim()));
                }
                else if (!e.Row.Cells[3].Text.ToUpper().Equals("FIJA"))
                {
                    e.Row.Cells[4].Text = "";
                }

                if (!e.Row.Cells[3].Text.ToUpper().Equals("FIJA"))
                {
                    ((TextBox)e.Row.Cells[5].FindControl("Txt_Cantidad_Deduccion")).Style.Add("display", "none");
                }

                ((CheckBox)e.Row.Cells[0].FindControl("Chk_Aplica")).ToolTip = "" + e.Row.RowIndex;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Botones)

    #region (Operacion [Alta - Modificar - Eliminar - Consultar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Nuevo_Click
    /// DESCRIPCION : Alta de Empleado
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                if (Validar_Datos())
                {
                    if (Txt_Confirma_Password_Empleado.Text != "" || Txt_Password_Empleado.Text != "")
                    {
                        if (Txt_Password_Empleado.Text == "")
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "+ El password del Empleado";
                            return;
                        }
                        if (Txt_Confirma_Password_Empleado.Text == "")
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "+ La confirmación del password del Empleado";
                            return;
                        }
                        if (Txt_Password_Empleado.Text != Txt_Confirma_Password_Empleado.Text)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "+ El password y su confirmación deben ser iguales, favor de verificar";
                            return;
                        }
                    }
                    if (Validar_Datos())
                    {
                        Alta_Empleado(); //Da de alta los datos proporcionados por el usuario
                        Limpia_Controles();    //Limpia los controles de la forma
                        Grid_Empleados.DataSource = new DataTable();
                        Grid_Empleados.DataBind();
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Modificar_Click
    /// DESCRIPCION : Modificar al Empleado Seleccionado
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_Empleado_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                    Consulta_Detalles_Requisitos();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Empleado que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                if (Validar_Datos())
                {
                    if (Txt_Confirma_Password_Empleado.Text != "" || Txt_Password_Empleado.Text != "")
                    {
                        if (Txt_Password_Empleado.Text == "")
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "+ El password del Empleado";
                            return;
                        }
                        if (Txt_Confirma_Password_Empleado.Text == "")
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                            Lbl_Mensaje_Error.Text += "+ La confirmación del password del Empleado";
                            return;
                        }
                        if (Txt_Password_Empleado.Text != Txt_Confirma_Password_Empleado.Text)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "El password y su confirmación deben ser iguales, favor de verificar";
                            return;
                        }
                    }
                    if (Validar_Datos())
                    {
                        Modificar_Empleado(); //Modifica los datos del Empleado con los datos proporcionados por el usuario   
                        Grid_Empleados.DataSource = new DataTable();
                        Grid_Empleados.DataBind();
                    }
                    else {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Click
    /// DESCRIPCION : Eliminar al Empleado Seleccionado
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Si el usuario selecciono un Empleado entonces lo elimina de la base de datos
            if (Txt_Empleado_ID.Text != "")
            {
                Eliminar_Empleado(); //Elimina el Empleado que fue seleccionado por el usuario
                Grid_Empleados.DataSource = new DataTable();
                Grid_Empleados.DataBind();
            }
            //Si el usuario no selecciono algún Empleado manda un mensaje indicando que es necesario que seleccione algun para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Empleado que desea eliminar <br>";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Salir_Click
    /// DESCRIPCION : Salir o Cancelar la Operacion Actual
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Salir")
            {
                Session.Remove("Consulta_Empleados");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Busqueda_Empleados_Click
    /// DESCRIPCION : Ejecuta la busqueda de empleados
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        try
        {
            Consulta_Empleados_Avanzada();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region (Actualizar Salario)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Actualizar_Salario_Click
    /// DESCRIPCION : Obtiene el salario diario y el salario diario integrado.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Actualizar_Salario_Click(Object sender, EventArgs e)
    {
        try
        {
            Calcular_Salarios();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region (ModalPopup)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Mostrar_Popup_Click
    /// DESCRIPCION : Carga los controles con la informacion del empleado a eliminar.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Mostrar_Popup_Click(Object sender, EventArgs e)
    {
        try
        {
            if (Grid_Empleados.SelectedIndex != -1)
            {
                Txt_Baja_No_Empleado.Text = Txt_No_Empleado.Text;
                Txt_Baja_Nombre_Empleado.Text = Txt_Nombre_Empleado.Text;
                Img_Empleado_Eliminar.ImageUrl = Img_Foto_Empleado.ImageUrl;
                Img_Empleado_Eliminar.DataBind();
                Mpe_Baja_Empleado.Show();
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el empleado que desea dar de baja";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }

    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Cerrar_Ventana_Click
    /// DESCRIPCION : Cierra la ventana de busqueda de empleados.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Busqueda_Empleados.Hide();
    }
    #endregion

    #endregion

    #region (Combos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cmb_Puestos_SelectedIndexChanged
    /// DESCRIPCION : Obtiene el salario diario y el salario diario integrado.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Cmb_Puestos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Calcular_Salarios();

            Presidencia.Codigo_Programatico.Cls_Ayudante_Codigo_Programatico.Configurar_Codigo_Promatico(
                ref Cmb_SAP_Fuente_Financiamiento,
                ref Cmb_SAP_Area_Funcional,
                ref Cmb_SAP_Programas,
                ref Cmb_SAP_Unidad_Responsable,
                ref Cmb_SAP_Partida,
                (((Cmb_Puestos.SelectedItem.Text.Trim().Split('*')[0]) != null) ? Cmb_Puestos.SelectedItem.Text.Trim().Split('*')[0] : String.Empty),
                ref  Txt_SAP_Fuente_Financiamiento,
                ref  Txt_SAP_Area_Responsable,
                ref  Txt_SAP_Programa,
                ref  Txt_SAP_Unidad_Responsable,
                ref  Txt_SAP_Partida
                );
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cmb_Busqueda_Dependencia_SelectedIndexChanged
    /// DESCRIPCION : Cargar las area correspodientes a la dependencia seleccionada.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Cmb_Busqueda_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Consulta_Areas_Dependencia_Busqueda(Cmb_Busqueda_Dependencia.SelectedValue);
            Cmb_Busqueda_Areas.Enabled = true;
            Mpe_Busqueda_Empleados.Show();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cmb_Tipo_Nomina_SelectedIndexChanged
    /// DESCRIPCION : Carga las Percepciones y deducciones correspondientes al tipo de nomina
    ///               seleccionado.
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernández Negrete
    /// FECHA_CREO  : 04/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Cmb_Tipo_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Tipo_Nomina_Percepciones = null;//variable que almacenara una lista de percepciones.
        DataTable Dt_Tipo_Nomina_Deducciones = null;//Variable que almacenará una lista de deducciones.

        try
        {
            Dt_Tipo_Nomina_Percepciones = Obtener_Percepciones_Deducciones_Tipo_Nomina(Cmb_Tipo_Nomina.SelectedValue.Trim(), "PERCEPCION");
            Cargar_Grid_Tipo_Nomina_Percepciones_Deducciones(Dt_Tipo_Nomina_Percepciones, Grid_Tipo_Nomina_Percepciones, "Txt_Cantidad_Percepcion");
            //..
            Cargar_Percepciones_Deducciones(Dt_Tipo_Nomina_Percepciones, Cmb_Percepciones_All);

            Dt_Tipo_Nomina_Deducciones = Obtener_Percepciones_Deducciones_Tipo_Nomina(Cmb_Tipo_Nomina.SelectedValue.Trim(), "DEDUCCION");
            Cargar_Grid_Tipo_Nomina_Percepciones_Deducciones(Dt_Tipo_Nomina_Deducciones, Grid_Tipo_Nomina_Deducciones, "Txt_Cantidad_Deduccion");
            //..
            Cargar_Percepciones_Deducciones(Dt_Tipo_Nomina_Deducciones, Cmb_Deducciones_All);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    protected void Cmb_SAP_Unidad_Responsable_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Dependencias_Negocio Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();
        DataTable Dt_Depedencias = null;
        String Clave_Unidad_Responsable = String.Empty;
        String Area_Funcional = String.Empty;

        try
        {
            if (Cmb_SAP_Unidad_Responsable.SelectedIndex > 0)
            {
                Consultar_SAP_Fuentes_Financiamiento(Cmb_SAP_Unidad_Responsable.SelectedValue.Trim());
                Consulta_SAP_Programas(Cmb_SAP_Unidad_Responsable.SelectedValue.Trim());
                Consulta_Areas_Dependencia(Cmb_SAP_Unidad_Responsable.SelectedValue);
                Cmb_Areas_Empleado.Enabled = true;
                Consultar_Puestos(Cmb_SAP_Unidad_Responsable.SelectedValue.Trim(), "DISPONIBLE", null, null);//Linea Agregada

                Obj_Dependencias.P_Dependencia_ID = Cmb_SAP_Unidad_Responsable.SelectedValue.Trim();
                Dt_Depedencias = Obj_Dependencias.Consulta_Dependencias();

                if (Dt_Depedencias is DataTable)
                {
                    if (Dt_Depedencias.Rows.Count > 0)
                    {
                        foreach (DataRow DEPENDENCIA in Dt_Depedencias.Rows)
                        {
                            if (DEPENDENCIA is DataRow)
                            {
                                if (!String.IsNullOrEmpty(DEPENDENCIA[Cat_Dependencias.Campo_Clave].ToString()))
                                    Clave_Unidad_Responsable = DEPENDENCIA[Cat_Dependencias.Campo_Clave].ToString();

                                if (!String.IsNullOrEmpty(DEPENDENCIA[Cat_Dependencias.Campo_Area_Funcional_ID].ToString()))
                                {
                                    Area_Funcional = DEPENDENCIA[Cat_Dependencias.Campo_Area_Funcional_ID].ToString();
                                    Consultar_SAP_Areas_Funcionales(Area_Funcional);
                                }

                                Txt_SAP_Unidad_Responsable.Text = Clave_Unidad_Responsable;
                            }
                        }
                    }
                }
            }

            Cmb_SAP_Fuente_Financiamiento.Enabled = true;
            Cmb_SAP_Programas.Enabled = true;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un elemento de la lista de Unidades Responsables. Error: [" + Ex.Message + "]");
        }
    }

    protected void Cmb_SAP_Fuente_Financiamiento_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_SAP_Fuente_Financiamiento_Negocio Obj_Fuentes_Financiamiento = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio();
        DataTable Dt_Fuentes_Financiamiento = null;
        String Clave_Fuente_Financiamiento = String.Empty;

        try
        {
            if (Cmb_SAP_Fuente_Financiamiento.SelectedIndex > 0) { 
                Obj_Fuentes_Financiamiento.P_Fuente_Financiamiento_ID = Cmb_SAP_Fuente_Financiamiento.SelectedValue.Trim();
                Dt_Fuentes_Financiamiento = Obj_Fuentes_Financiamiento.Consulta_Datos_Fuente_Financiamiento();

                if (Dt_Fuentes_Financiamiento is DataTable) {
                    if (Dt_Fuentes_Financiamiento.Rows.Count > 0) {
                        foreach (DataRow FUENTE_FINANCIAMIENTO in Dt_Fuentes_Financiamiento.Rows) {
                            if (FUENTE_FINANCIAMIENTO is DataRow) {
                                if (!String.IsNullOrEmpty(FUENTE_FINANCIAMIENTO[Cat_SAP_Fuente_Financiamiento.Campo_Clave].ToString()))
                                    Clave_Fuente_Financiamiento = FUENTE_FINANCIAMIENTO[Cat_SAP_Fuente_Financiamiento.Campo_Clave].ToString();

                                Txt_SAP_Fuente_Financiamiento.Text = Clave_Fuente_Financiamiento;
                            }   
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un elemento de la lista de Fuentes de Financiamiento. Error: [" + Ex.Message + "]");
        }
    }

    protected void Cmb_SAP_Programas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Com_Proyectos_Programas_Negocio Obj_Programas = new Cls_Cat_Com_Proyectos_Programas_Negocio();
        DataTable Dt_Programas = null;
        String Clave_Proyecto_Programa =String.Empty;

        try
        {
            if(Cmb_SAP_Programas.SelectedIndex > 0){
                Consultar_SAP_Partidas(Cmb_SAP_Programas.SelectedValue.Trim());
                Cmb_SAP_Partida.Enabled = true;

                Obj_Programas.P_Proyecto_Programa_ID = Cmb_SAP_Programas.SelectedValue.Trim();
                Dt_Programas = Obj_Programas.Consulta_Programas_Proyectos();

                if (Dt_Programas is DataTable) {
                    if (Dt_Programas.Rows.Count > 0) {
                        foreach (DataRow PROGRAMAS in Dt_Programas.Rows) {
                            if (PROGRAMAS is DataRow) {
                                if (!String.IsNullOrEmpty(PROGRAMAS[Cat_Com_Proyectos_Programas.Campo_Clave].ToString()))
                                    Clave_Proyecto_Programa = PROGRAMAS[Cat_Com_Proyectos_Programas.Campo_Clave].ToString();

                                Txt_SAP_Programa.Text = Clave_Proyecto_Programa;
                            }
                        }
                    }                
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un elemento de la lista de Programas. Error: [" + Ex.Message + "]");
        }
    }

    protected void Cmb_SAP_Partida_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios Obj_Partidas = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Partidas = null;
        String Clave_Partida = String.Empty;

        try
        {
            if (Cmb_SAP_Partida.SelectedIndex > 0)
            {
               Dt_Partidas= Obj_Partidas.Consultar_Partida(Cmb_SAP_Partida.SelectedValue.Trim())  ;

                if(Dt_Partidas is DataTable){
                    if(Dt_Partidas.Rows.Count > 0){
                        foreach(DataRow PARTIDA in Dt_Partidas.Rows){
                            if(PARTIDA is DataRow){
                                if(!String.IsNullOrEmpty(PARTIDA[Cat_Sap_Partidas_Especificas.Campo_Clave].ToString()))
                                    Clave_Partida = PARTIDA[Cat_Sap_Partidas_Especificas.Campo_Clave].ToString();

                                Txt_SAP_Partida.Text = Clave_Partida;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un elemento de la lista de Partidas. Error: [" + Ex.Message + "]");
        }
    }

    #endregion

    #region (Eventos CheckBox  Tipo Nomina [Percepciones - Deducciones])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Chk_Aplica_Percepciones_CheckedChanged
    /// DESCRIPCION : Valida que si aplica la percepcion al empleado habilite el campo
    ///               para agregar la cantidad.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Chk_Aplica_Percepciones_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox Chk_Aplica = (CheckBox)sender;

        if (Grid_Tipo_Nomina_Percepciones.Rows[Convert.ToInt32(Chk_Aplica.ToolTip)].Cells[5].Text.ToUpper().Equals("FIJA") &&
            Grid_Tipo_Nomina_Percepciones.Rows[Convert.ToInt32(Chk_Aplica.ToolTip)].Cells[5].Text.Trim().Equals("$0.00"))
        {
            Grid_Tipo_Nomina_Percepciones.Rows[Convert.ToInt32(Chk_Aplica.ToolTip)].Cells[5].Enabled = Chk_Aplica.Checked;
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Chk_Aplica_Deducciones_CheckedChanged
    /// DESCRIPCION : Valida que si aplica la Deducción al empleado habilite el campo
    ///               para agregar la cantidad.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Chk_Aplica_Deducciones_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox Chk_Aplica = (CheckBox)sender;

        if (Grid_Tipo_Nomina_Deducciones.Rows[Convert.ToInt32(Chk_Aplica.ToolTip)].Cells[5].Text.ToUpper().Equals("FIJA") &&
            Grid_Tipo_Nomina_Deducciones.Rows[Convert.ToInt32(Chk_Aplica.ToolTip)].Cells[5].Text.Trim().Equals("$0.00"))
        {
            Grid_Tipo_Nomina_Deducciones.Rows[Convert.ToInt32(Chk_Aplica.ToolTip)].Cells[5].Enabled = Chk_Aplica.Checked;
        }
    }
    #endregion

    #endregion

    protected void Cargar_Percepciones_Deducciones(DataTable Dt_Perc_Deduc, DropDownList Combo)
    {
        try
        {
            Combo.DataSource = Dt_Perc_Deduc;
            Combo.DataTextField = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
            Combo.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Combo.DataBind();
            Combo.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Combo.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar los combos de percepciones y/o deducciones. Error: [" + Ex.Message + "]");
        }
    }

    protected void Btn_Agregar_Percepcion_Click(object sender, EventArgs e) {
        String Percepcion_Deduccion_ID_Agregar = "";
        String Percepcion_Deduccion_ID_A_Mostrar = "";

        try
        {
            if (Cmb_Percepciones_All.SelectedIndex > 0)
            {
                Percepcion_Deduccion_ID_Agregar = Cmb_Percepciones_All.SelectedValue.Trim();

                foreach (GridViewRow Fila in Grid_Tipo_Nomina_Percepciones.Rows)
                {
                    if (Fila is GridViewRow)
                    {
                        Percepcion_Deduccion_ID_A_Mostrar = Fila.Cells[1].Text.Trim();
                        if (Percepcion_Deduccion_ID_Agregar.Equals(Percepcion_Deduccion_ID_A_Mostrar))
                        {
                            Fila.Visible = true;
                            ((CheckBox)Fila.Cells[0].FindControl("Chk_Aplica")).Checked = true;
                            Cmb_Percepciones_All.SelectedIndex = -1;
                            break;
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Agregar Percepciones", "alert('Seleccione la percepcion que desea agregar al empleado');", true);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    protected void Btn_Quitar_Percepcion_Click(object sender, EventArgs e)
    {
        String Percepcion_Deduccion_ID_Quitar = "";
        String Percepcion_Deduccion_ID_A_Ocultar = "";

        try
        {
            Percepcion_Deduccion_ID_Quitar = ((ImageButton)sender).CommandArgument.ToString();

            foreach (GridViewRow Fila in Grid_Tipo_Nomina_Percepciones.Rows)
            {
                if (Fila is GridViewRow)
                {
                    Percepcion_Deduccion_ID_A_Ocultar = Fila.Cells[1].Text.Trim();
                    if (Percepcion_Deduccion_ID_Quitar.Equals(Percepcion_Deduccion_ID_A_Ocultar))
                    {
                        Fila.Visible = false;
                        ((CheckBox)Fila.Cells[0].FindControl("Chk_Aplica")).Checked = false;
                        Cmb_Percepciones_All.SelectedIndex = -1;
                        break;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    protected void Btn_Agregar_Deduccion_Click(object sender, EventArgs e)
    {
        String Percepcion_Deduccion_ID_Agregar = "";
        String Percepcion_Deduccion_ID_A_Mostrar = "";

        try
        {
            if (Cmb_Deducciones_All.SelectedIndex > 0)
            {
                Percepcion_Deduccion_ID_Agregar = Cmb_Deducciones_All.SelectedValue.Trim();

                foreach (GridViewRow Fila in Grid_Tipo_Nomina_Deducciones.Rows)
                {
                    if (Fila is GridViewRow)
                    {
                        Percepcion_Deduccion_ID_A_Mostrar = Fila.Cells[1].Text.Trim();
                        if (Percepcion_Deduccion_ID_Agregar.Equals(Percepcion_Deduccion_ID_A_Mostrar))
                        {
                            Fila.Visible = true;
                            ((CheckBox)Fila.Cells[0].FindControl("Chk_Aplica")).Checked = true;
                            Cmb_Deducciones_All.SelectedIndex = -1;
                            break;
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Agregar Percepciones", "alert('Seleccione la deduccion que desea agregar al empleado');", true);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    protected void Btn_Quitar_Deduccion_Click(object sender, EventArgs e)
    {
        String Percepcion_Deduccion_ID_Quitar = "";
        String Percepcion_Deduccion_ID_A_Ocultar = "";

        try
        {
            Percepcion_Deduccion_ID_Quitar = ((ImageButton)sender).CommandArgument.ToString();

            foreach (GridViewRow Fila in Grid_Tipo_Nomina_Deducciones.Rows)
            {
                if (Fila is GridViewRow)
                {
                    Percepcion_Deduccion_ID_A_Ocultar = Fila.Cells[1].Text.Trim();
                    if (Percepcion_Deduccion_ID_Quitar.Equals(Percepcion_Deduccion_ID_A_Ocultar))
                    {
                        Fila.Visible = false;
                        ((CheckBox)Fila.Cells[0].FindControl("Chk_Aplica")).Checked = false;
                        Cmb_Deducciones_All.SelectedIndex = -1;
                        break;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
}
