using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Registro_Peticion.Negocios;
using Presidencia.Registro_Peticion.Datos;
using Presidencia.Sessiones;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Asuntos_AC.Negocio;
using Presidencia.Consulta_Peticiones.Negocios;
using CrystalDecisions.Shared;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using openXML_Wp = DocumentFormat.OpenXml.Wordprocessing;

public partial class paginas_Atencion_Ciudadana_Frm_Ope_Atencion_Respuesta_Solicitudes : System.Web.UI.Page
{
    #region Variables

    private static Cls_Cat_Ate_Peticiones_Negocio Peticion_Actual = new Cls_Cat_Ate_Peticiones_Negocio();
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Solucion = 1;
    private const int Const_Rol_Administrador = 2;
    private const int Const_Rol_Jefe_Dependencia = 3;
    private const int Const_Rol_Jefe_Area = 4;
    private const int Const_Estado_Mostrar_Detalles = 5;
    private const int Const_Estado_Modificar_Peticion = 6;

    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.Form.Enctype = "multipart/form-data";
            ScriptManager1.RegisterPostBackControl(Btn_Subir_Archivo);

            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 6000) + 5));
            if (string.IsNullOrEmpty(Cls_Sessiones.Empleado_ID))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                Mensaje_Error();
                LLenar_Combos();
                Estado_Botones(Const_Estado_Inicial);

                // llenar combo de Dependencia con estatus ACTIVO
                var Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();
                Obj_Dependencias.P_Estatus = "ACTIVO";
                Llenar_Combo_Con_DataTable(Cmb_Dependencia_Peticiones, Obj_Dependencias.Consulta_Dependencias());
                // seleccionar dependencia del usuario actual y deshabilitar
                if (Cmb_Dependencia_Peticiones.Items.FindByValue(Cls_Sessiones.Dependencia_ID_Empleado) != null)
                {
                    Cmb_Dependencia_Peticiones.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
                }
                Cmb_Dependencia_Peticiones.Enabled = false;
                Btn_Buscar_Dependencia_Filtro_Inicial.Enabled = false;
                Btn_Buscar_Dependencia_Filtro_Inicial.Visible = false;

                //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
                DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
                if (Dt_Grupo_Rol != null)
                {
                    String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
                    if (Grupo_Rol == "00001" || Grupo_Rol == "00002")
                    {
                        Cmb_Dependencia_Peticiones.Enabled = true;
                        Btn_Buscar_Dependencia_Filtro_Inicial.Enabled = true;
                        Btn_Buscar_Dependencia_Filtro_Inicial.Visible = true;
                    }
                    else
                    {
                        DataTable Dt_URs = Cls_Util.Consultar_URs_De_Empleado(Cls_Sessiones.Empleado_ID);
                        if (Dt_URs.Rows.Count > 1)
                        {
                            Cmb_Dependencia_Peticiones.Enabled = true;
                            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia_Peticiones, Dt_URs, 1, 0);
                            Cmb_Dependencia_Peticiones.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
                        }
                    }
                }

                // consultar peticiones de la dependencia seleccionada
                Consultar_Peticiones_Pendientes();
                // registro de scripts del lado del servidor para mostrar ventanas emergentes para búsqueda avanzada
                string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Asuntos.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Asunto.Attributes.Add("onclick", Ventana_Modal);
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Dependencia.Attributes.Add("onclick", Ventana_Modal);
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Dependencia_Filtro_Inicial.Attributes.Add("onclick", Ventana_Modal);
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message);
        }
    }

    #endregion

    #region METODOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: LLenar_Combos
    ///DESCRIPCIÓN: Consulta los datos para los combos y llama al método que carga los datos de la consulta en
    ///             los combos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void LLenar_Combos()
    {
        var Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();
        var Obj_Asunto = new Cls_Cat_Ate_Asuntos_Negocio();

        try
        {
            // Combo de Dependencia
            Obj_Dependencias.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Dependencia, Obj_Dependencias.Consulta_Dependencias());
            // Combo de Asunto
            Obj_Asunto.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Asunto, Obj_Asunto.Consultar_Registros());
        }
        catch (Exception Ex)
        {
            throw new Exception("No se pudo mostrar información: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Llenar_Combo_Con_DataTable
    ///DESCRIPCIÓN: Asigna los valores de la tabla en el combo recibidos como parámetros
    ///PARÁMETROS:
    /// 		1. Obj_Combo: control al que se van a asignar los datos en la tabla
    /// 		2. Dt_Temporal: tabla con los datos a mostrar en el control Obj_Combo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Llenar_Combo_Con_DataTable(DropDownList Obj_Combo, DataTable Dt_Temporal)
    {
        // ordenar elementos de la tabla
        Dt_Temporal.DefaultView.Sort = Dt_Temporal.Columns[1].ColumnName + " ASC";

        Obj_Combo.Items.Clear();
        Obj_Combo.SelectedValue = null;
        Obj_Combo.DataSource = Dt_Temporal;
        Obj_Combo.DataValueField = Dt_Temporal.Columns[0].ToString();
        Obj_Combo.DataTextField = Dt_Temporal.Columns[1].ToString();
        Obj_Combo.DataBind();
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
        Obj_Combo.SelectedIndex = 0;
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Convertir_Fecha
    ///DESCRIPCION : formato de fecha
    ///PARAMETROS  : Str_Fecha
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 27-Mayo-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    public String Convertir_Fecha(String Str_Fecha)
    {
        String[] Str_Temporal = Str_Fecha.Split('/');
        return Str_Temporal[1] + "/" + Str_Temporal[0] + "/" + Str_Temporal[2];
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Cargar_Rol
    ///DESCRIPCION : habilita los controles segun el rol
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 27-Mayo-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Cargar_Rol()
    {
        String Grupo_Rol = Cls_Cat_Ate_Peticiones_Datos.Consultar_Grupo_Rol(Cls_Sessiones.Rol_ID);

        switch (Grupo_Rol)
        {
            case "00001":
                Estado_Botones(Const_Rol_Administrador);
                break;
            case "00002":
                Estado_Botones(Const_Rol_Administrador);
                break;
            case "00003":
                Estado_Botones(Const_Rol_Jefe_Dependencia);
                break;
            case "00004":
                Estado_Botones(Const_Rol_Jefe_Area);
                break;
            default:
                Mensaje_Error("No Tiene suficientes privilegios para modificar esta información");
                break;
        }
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Guardar_Solucion
    ///DESCRIPCION : Asigna la solucion y la fecha de solucion, o en caso de no dar solucion
    ///              asigna fecha de solucion problable    
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 27-Mayo-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Guardar_Solucion()
    {
        Cls_Cat_Ate_Peticiones_Negocio Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
        DataTable Dt_Archivos = null;
        string Folio = Txt_Folio.Text;

        try
        {
            // parámetros para identificar la petición
            if (Grid_Peticiones.SelectedIndex > -1)
            {
                int Anio_Peticion;
                int.TryParse(Grid_Peticiones.SelectedRow.Cells[2].Text, out Anio_Peticion);
                Obj_Peticiones.P_No_Peticion = Grid_Peticiones.SelectedRow.Cells[1].Text;
                Obj_Peticiones.P_Programa_ID = Grid_Peticiones.SelectedRow.Cells[3].Text;
                Obj_Peticiones.P_Anio_Peticion = Anio_Peticion;
            }
            Obj_Peticiones.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
            Obj_Peticiones.P_Asunto_ID = Cmb_Asunto.SelectedValue;
            // descripción de la solución
            Obj_Peticiones.P_Descripcion_Solucion = Txt_Solucion.Text.Trim().Replace("'", "");
            Obj_Peticiones.P_Descripcion_Cambio = Txt_Solucion.Text.Trim().Replace("'", "");

            // sólo agregar tipo de solución si es POSITIVA o NEGATIVA
            if (Cmb_Solucion.SelectedValue != "EN PROCESO")
            {
                Obj_Peticiones.P_Tipo_Solucion = Cmb_Solucion.SelectedValue;
            }
            

            Obj_Peticiones.P_Estatus = "EN PROCESO";
            Obj_Peticiones.P_Por_Validar = "SI";
            Obj_Peticiones.P_Usuario_Creo_Modifico = Cls_Sessiones.Nombre_Empleado;

            // si la sesión ya existe, recuperar tabla con archivos, si no, llamar método que crea la tabla
            if (Session["Dt_Archivos"] != null)
            {
                Dt_Archivos = (DataTable)Session["Dt_Archivos"];
                // validar que la tabla contenga registros
                if (Dt_Archivos != null && Dt_Archivos.Rows.Count > 0)
                {
                    Obj_Peticiones.P_Dt_Archivos = Dt_Archivos;
                    Asignar_Ruta_Relativa_Archivos(Dt_Archivos, Folio);
                    Guardar_Archivos(Dt_Archivos);
                }
            }
            // si la consulta inserta registros, mostrar mensaje y reiniciar controles, si no, mostrar mensaje de error
            if (Obj_Peticiones.Modificar_Peticion_Solucion() > 0)
            {
                Consultar_Peticiones_Pendientes();
                Estado_Botones(Const_Estado_Inicial);
                Limpiar_Controles();
                //Enviar_Correo();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('La Petición fue actualizada.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('No fue posible actualizar la Petición.');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al guardar solución: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Asignar_Ruta_Relativa_Archivos
    /// DESCRIPCIÓN: Lee los nombres de archivos contenidos en la tabla que llega como parámetro y les 
    ///             agrega el nombre de directorio relativo: 
    ///             ..\..\Archivos\Atencion_Ciudadana\Peticiones\Folio\archivo
    /// PARÁMETROS:
    ///         1. Dt_Archivos: tabla que contiene los nombres de los archivos a los que se va a asignar una ruta relativa
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 20-jun-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Asignar_Ruta_Relativa_Archivos(DataTable Dt_Archivos, string Folio)
    {
        String Nombre_Directorio;

        try
        {
            if (Dt_Archivos != null)     // si la tabla trámites contiene datos
            {
                foreach (DataRow Dr_Archivo in Dt_Archivos.Rows)   // recorrer la tabla
                {
                    // nombre directorio: ..\..\Archivos\Atencion_Ciudadana\Peticiones\Folio\archivo
                    Nombre_Directorio = @"..\..\Archivos\Atencion_Ciudadana\Peticiones\" + Folio;
                    Dr_Archivo.BeginEdit();
                    // Si contiene un nombre de archivo y no contiene diagonales, agregar ruta relativa
                    if (Dr_Archivo["RUTA_ARCHIVO"].ToString() != "" && !Dr_Archivo["RUTA_ARCHIVO"].ToString().Contains("/") && !Dr_Archivo["RUTA_ARCHIVO"].ToString().Contains("\\"))
                    {
                        // actualizar nombres de archivos (ruta completa)
                        Dr_Archivo["RUTA_ARCHIVO"] = Nombre_Directorio + @"\" + Dr_Archivo["RUTA_ARCHIVO"].ToString().Trim();
                    }
                }
                Dt_Archivos.AcceptChanges();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Asignar_Ruta_Relativa_Archivos: " + Ex.Message, Ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Guardar_Archivos
    /// DESCRIPCIÓN: Guardar en el servidor los archivos que se hayan recibido
    /// PARÁMETROS:
    ///         1. Dt_Archivos: tabla que contiene los nombres de los archivos a guardar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 20-jun-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Guardar_Archivos(DataTable Dt_Archivos)
    {
        String Nombre_Directorio;
        String Ruta_Archivo;
        int Contador_Archivo_Existente;

        try
        {
            if (Dt_Archivos != null)     //si la tabla tramites contiene datos
            {
                foreach (DataRow Dr_Archivo in Dt_Archivos.Rows)   // recorrer la tabla
                {
                    Contador_Archivo_Existente = 0;

                    Nombre_Directorio = Server.MapPath(Path.GetDirectoryName(Dr_Archivo["RUTA_ARCHIVO"].ToString()));
                    Ruta_Archivo = Server.MapPath(HttpUtility.HtmlDecode(Dr_Archivo["RUTA_ARCHIVO"].ToString()));
                    if (!Directory.Exists(Nombre_Directorio))                       //si el directorio no existe, crearlo
                        Directory.CreateDirectory(Nombre_Directorio);
                    // si el archivo ya existe, agregar contador numérico
                    string Nombre_Original = Dr_Archivo["RUTA_ARCHIVO"].ToString();
                    while (File.Exists(Ruta_Archivo))
                    {
                        // agregar un contador al archivo
                        string Extension = Path.GetExtension(Dr_Archivo["RUTA_ARCHIVO"].ToString());
                        string Nombre_Archivo_Sin_Extension = Path.GetFileNameWithoutExtension(Dr_Archivo["RUTA_ARCHIVO"].ToString());
                        string Ultimo_Caracter_Nombre_Archivo = Nombre_Archivo_Sin_Extension.Substring(Nombre_Archivo_Sin_Extension.Length - 1, 1);
                        // si el nombre del archivo termina con un número, aumentar el número
                        if (int.TryParse(Ultimo_Caracter_Nombre_Archivo, out Contador_Archivo_Existente))
                        {
                            Ruta_Archivo = Path.GetDirectoryName(Nombre_Original) + @"\" + Path.GetFileName(Ruta_Archivo.Replace(Nombre_Archivo_Sin_Extension, Nombre_Archivo_Sin_Extension.Substring(0, Nombre_Archivo_Sin_Extension.Length - 1) + (++Contador_Archivo_Existente).ToString()));
                        }
                        else
                        {
                            // si el nombre del archivo no termina en número, agregar contador
                            Ruta_Archivo = Nombre_Original.Replace(Extension, (++Contador_Archivo_Existente).ToString() + Extension);
                        }
                        // actualizar valor en datatable
                        Dr_Archivo.BeginEdit();
                        Dr_Archivo["RUTA_ARCHIVO"] = Ruta_Archivo;
                        Dr_Archivo.AcceptChanges();
                        Ruta_Archivo = Server.MapPath(Ruta_Archivo);
                    }
                    //crear filestream y binarywriter para guardar archivo
                    using (FileStream Escribir_Archivo = new FileStream(Ruta_Archivo, FileMode.Create, FileAccess.Write))
                    {
                        using (BinaryWriter Datos_Archivo = new BinaryWriter(Escribir_Archivo))
                        {
                            // Guardar archivo (escribir datos en el filestream)
                            Datos_Archivo.Write((byte[])Dr_Archivo["ARCHIVO"]);
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al guardar archivos: " + Ex.Message, Ex);
        }
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Validar_Guardar
    ///DESCRIPCION : Valida que haya datos en los controles para guardar los datos en la peticion        
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 27-Mayo-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private bool Validar_Guardar()
    {
        Boolean Resultado = true;
        try
        {
            if (Txt_Solucion.Text.Trim() == "")
            {
                Resultado = false;
                Mensaje_Error("Favor de ingresar la descripcion de la solucion");
            }
            else
            {
                if (Txt_Solucion.Text.Trim().Length > 3000)
                {
                    Resultado = false;
                    Mensaje_Error("La descripción de la solución supera el límite de caracteres permitido.");
                }
            }
            if (Cmb_Solucion.SelectedIndex <= 0)
            {
                Resultado = false;
                Cmb_Solucion.Focus();
                Mensaje_Error("Favor de seleccionar el tipo de solución");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Resultado;
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Estado_Botones
    ///DESCRIPCION : determina el estado de los controles del formulario
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 31-Mayo-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Estado_Botones(int P_Estado)
    {
        switch (P_Estado)
        {
            case Const_Estado_Inicial:
                Txt_Folio.Enabled = false;
                Txt_Fecha.Enabled = false;
                Txt_Nombre.Enabled = false;
                Txt_Domicilio.Enabled = false;
                Txt_Referencia.Enabled = false;
                Cmb_Asunto.Enabled = false;
                Cmb_Dependencia.Enabled = false;
                Cmb_Estatus.Enabled = false;
                Txt_Descripcion.Enabled = false;
                Txt_Solucion.Enabled = false;
                Cmb_Solucion.Enabled = false;

                Lbl_Fecha_Probable.Visible = false;
                Txt_Fecha_Probable.Visible = false;

                Lbl_Motivo.Visible = false;
                Txt_Motivo.Visible = false;

                Btn_Solucion.Enabled = false;
                Btn_Salir.Enabled = true;
                Btn_Solucion.Visible = false;
                Btn_Imprimir.Visible = false;
                Btn_Imprimir.Enabled = false;
                Div_Cargar_Archivo.Style.Value = "text-align: right; color: #5D7B9D; display: none;";

                Btn_Solucion.AlternateText = "Solución";
                Btn_Salir.AlternateText = "Salir";

                Lbl_Motivo.Text = "Motivo de cambio";
                Lbl_Cmb_Asunto.Text = "Asunto";
                Lbl_Txt_Solucion.Text = "Solución";

                Btn_Solucion.ImageUrl = "~/paginas/imagenes/paginas/icono_respuesta_peticion.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                Contenedor_Grid_Peticiones.Visible = true;
                Contenedor_Controles_Seguimiento.Visible = false;
                break;

            case Const_Estado_Mostrar_Detalles:
                Txt_Folio.Enabled = false;
                Txt_Fecha.Enabled = false;
                Txt_Nombre.Enabled = false;
                Txt_Domicilio.Enabled = false;
                Txt_Referencia.Enabled = false;
                Cmb_Asunto.Enabled = false;
                Cmb_Dependencia.Enabled = false;
                Cmb_Estatus.Enabled = false;
                Txt_Descripcion.Enabled = false;
                Txt_Solucion.Enabled = false;
                Cmb_Solucion.Enabled = false;

                Lbl_Fecha_Probable.Visible = false;
                Txt_Fecha_Probable.Visible = false;

                Lbl_Motivo.Visible = false;
                Txt_Motivo.Visible = false;

                Btn_Solucion.Enabled = true;
                Btn_Salir.Enabled = true;
                Btn_Solucion.Visible = true;
                Btn_Buscar_Asunto.Enabled = false;
                Btn_Buscar_Dependencia.Enabled = false;
                Btn_Imprimir.Visible = true;
                Btn_Imprimir.Enabled = true;
                Div_Cargar_Archivo.Style.Value = "text-align: right; color: #5D7B9D; display: none;";

                Btn_Solucion.AlternateText = "Solución";
                Btn_Salir.AlternateText = "Regresar";

                Lbl_Motivo.Text = "Motivo de cambio";
                Lbl_Cmb_Asunto.Text = "Asunto";
                Lbl_Txt_Solucion.Text = "Solución";

                Btn_Solucion.ImageUrl = "~/paginas/imagenes/paginas/icono_respuesta_peticion.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                Contenedor_Grid_Peticiones.Visible = false;
                Contenedor_Controles_Seguimiento.Visible = true;
                break;

            case Const_Estado_Solucion:
                Txt_Folio.Enabled = false;
                Txt_Fecha.Enabled = false;
                Txt_Nombre.Enabled = false;
                Txt_Domicilio.Enabled = false;
                Txt_Referencia.Enabled = false;
                Cmb_Asunto.Enabled = false;
                Cmb_Dependencia.Enabled = false;
                Cmb_Estatus.Enabled = false;
                Txt_Descripcion.Enabled = false;
                Txt_Solucion.Enabled = true;
                Cmb_Solucion.Enabled = true;

                Lbl_Fecha_Probable.Visible = false;
                Txt_Fecha_Probable.Visible = false;

                Lbl_Motivo.Visible = false;
                Txt_Motivo.Visible = false;

                Btn_Solucion.Enabled = true;
                Btn_Salir.Enabled = true;
                Btn_Solucion.Visible = true;
                Btn_Buscar_Asunto.Enabled = false;
                Btn_Buscar_Dependencia.Enabled = false;
                Btn_Imprimir.Visible = false;
                Btn_Imprimir.Enabled = false;
                Div_Cargar_Archivo.Style.Value = "text-align: right; color: #5D7B9D; display: block;";

                Btn_Solucion.AlternateText = "Guardar";
                Btn_Salir.AlternateText = "Regresar";

                Lbl_Motivo.Text = "Motivo de cambio";
                Lbl_Cmb_Asunto.Text = "Asunto";
                Lbl_Txt_Solucion.Text = "*Solución";

                Btn_Solucion.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                Contenedor_Grid_Peticiones.Visible = false;
                Contenedor_Controles_Seguimiento.Visible = true;
                break;

            case Const_Estado_Modificar_Peticion:
                Txt_Folio.Enabled = false;
                Txt_Fecha.Enabled = false;
                Txt_Nombre.Enabled = false;
                Txt_Domicilio.Enabled = false;
                Txt_Referencia.Enabled = false;
                Cmb_Asunto.Enabled = true;
                Cmb_Dependencia.Enabled = true;
                Cmb_Estatus.Enabled = false;
                Txt_Descripcion.Enabled = false;
                Txt_Solucion.Enabled = false;
                Cmb_Solucion.Enabled = false;

                Lbl_Fecha_Probable.Visible = false;
                Txt_Fecha_Probable.Visible = false;

                Lbl_Motivo.Visible = true;
                Txt_Motivo.Visible = true;

                Btn_Solucion.Enabled = false;
                Btn_Salir.Enabled = true;
                Btn_Solucion.Visible = false;
                Btn_Buscar_Asunto.Enabled = true;
                Btn_Buscar_Dependencia.Enabled = true;
                Btn_Imprimir.Visible = false;
                Btn_Imprimir.Enabled = false;
                Div_Cargar_Archivo.Style.Value = "text-align: right; color: #5D7B9D; display: block;";

                Btn_Solucion.AlternateText = "Solución";
                Btn_Salir.AlternateText = "Cancelar";

                Lbl_Motivo.Text = "*Observaciones (Motivo de cambio)";
                Lbl_Cmb_Asunto.Text = "*Asunto";
                Lbl_Txt_Solucion.Text = "Solución";

                Btn_Solucion.ImageUrl = "~/paginas/imagenes/paginas/icono_respuesta_peticion.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                Contenedor_Grid_Peticiones.Visible = false;
                Contenedor_Controles_Seguimiento.Visible = true;
                break;
            case Const_Rol_Administrador:
                Cmb_Asunto.Enabled = true;
                Cmb_Dependencia.Enabled = true;
                Txt_Descripcion.Enabled = true;
                Cmb_Estatus.Enabled = false;
                Txt_Fecha.Enabled = false;
                Txt_Folio.Enabled = false;
                Txt_Nombre.Enabled = false;
                Txt_Domicilio.Enabled = false;
                Txt_Referencia.Enabled = false;
                Txt_Solucion.Enabled = false;
                Cmb_Solucion.Enabled = false;

                Lbl_Motivo.Visible = true;
                Txt_Motivo.Visible = true;

                Btn_Solucion.Enabled = false;
                Btn_Salir.Enabled = true;
                Btn_Imprimir.Visible = false;
                Btn_Imprimir.Enabled = false;

                Btn_Solucion.AlternateText = "Solución";
                Btn_Salir.AlternateText = "Cancelar";

                Lbl_Motivo.Text = "*Motivo de cambio";
                Lbl_Cmb_Asunto.Text = "*Asunto";
                Lbl_Txt_Solucion.Text = "Solución";

                Btn_Solucion.ImageUrl = "~/paginas/imagenes/paginas/icono_respuesta_peticion.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                Contenedor_Grid_Peticiones.Visible = false;
                Contenedor_Controles_Seguimiento.Visible = true;

                break;
            case Const_Rol_Jefe_Dependencia:
                Cmb_Asunto.Enabled = true;
                Cmb_Dependencia.Enabled = true;
                Txt_Descripcion.Enabled = true;
                Cmb_Estatus.Enabled = false;
                Txt_Fecha.Enabled = false;
                Txt_Folio.Enabled = false;
                Txt_Nombre.Enabled = false;
                Txt_Domicilio.Enabled = false;
                Txt_Referencia.Enabled = false;
                Txt_Solucion.Enabled = true;
                Cmb_Solucion.Enabled = true;

                Lbl_Motivo.Visible = true;
                Txt_Motivo.Visible = true;

                Btn_Solucion.Enabled = false;
                Btn_Salir.Enabled = true;
                Btn_Imprimir.Visible = false;
                Btn_Imprimir.Enabled = false;

                Btn_Solucion.AlternateText = "Solución";
                Btn_Salir.AlternateText = "Cancelar";

                Lbl_Motivo.Text = "*Motivo de cambio";
                Lbl_Cmb_Asunto.Text = "*Asunto";
                Lbl_Txt_Solucion.Text = "Solución";

                Btn_Solucion.ImageUrl = "~/paginas/imagenes/paginas/icono_respuesta_peticion.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                Txt_Motivo.Focus();

                break;
            case Const_Rol_Jefe_Area:
                Cmb_Asunto.Enabled = true;
                Cmb_Dependencia.Enabled = false;
                Txt_Descripcion.Enabled = true;
                Cmb_Estatus.Enabled = false;
                Txt_Fecha.Enabled = false;
                Txt_Folio.Enabled = false;
                Txt_Nombre.Enabled = false;
                Txt_Domicilio.Enabled = false;
                Txt_Referencia.Enabled = false;
                Txt_Solucion.Enabled = true;
                Cmb_Solucion.Enabled = true;

                Lbl_Motivo.Visible = true;
                Txt_Motivo.Visible = true;

                Btn_Solucion.Enabled = false;
                Btn_Salir.Enabled = true;
                Btn_Imprimir.Visible = false;
                Btn_Imprimir.Enabled = false;

                Btn_Solucion.AlternateText = "Solución";
                Btn_Salir.AlternateText = "Cancelar";

                Lbl_Motivo.Text = "*Motivo de cambio";
                Lbl_Cmb_Asunto.Text = "*Asunto";
                Lbl_Txt_Solucion.Text = "Solución";

                Btn_Solucion.ImageUrl = "~/paginas/imagenes/paginas/icono_respuesta_peticion.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                Txt_Motivo.Focus();

                break;
        }

    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Limpiar_Controles
    ///DESCRIPCIÓN: borra el contenido de las cajas de texto y selecciona el índice -1 de los combos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 24-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Controles()
    {
        Txt_Folio.Text = "";
        Txt_Fecha.Text = "";
        Txt_Nombre.Text = "";
        Hdn_Email.Value = "";
        Txt_Domicilio.Text = "";
        Txt_Referencia.Text = "";
        Cmb_Asunto.SelectedIndex = -1;
        Cmb_Dependencia.SelectedIndex = -1;
        Cmb_Estatus.SelectedIndex = -1;
        Txt_Fecha_Probable.Text = "";
        Txt_Motivo.Text = "";
        Txt_Descripcion.Text = "";
        Txt_Solucion.Text = "";
        Cmb_Solucion.SelectedIndex = -1;

        Session.Remove("Dt_Seguimiento");
        Session.Remove("Dt_Peticiones");
        Session.Remove("Dt_Archivos");

        Grid_Archivos.DataSource = null;
        Grid_Archivos.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA METODO: LLenar_Combo_Id
    ///        DESCRIPCIÓN: llena todos los combos
    ///         PARAMETROS: 1.- Obj_DropDownList: Combo a llenar
    ///                     2.- Dt_Temporal: DataTable genarada por una consulta a la base de datos
    ///                     3.- Texto: nombre de la columna del dataTable que mostrara el texto en el combo
    ///                     3.- Valor: nombre de la columna del dataTable que mostrara el valor en el combo
    ///                     3.- Seleccion: Id del combo el cual aparecera como seleccionado por default
    ///               CREO: Jesus S. Toledo Rdz.
    ///         FECHA_CREO: 06/9/2010
    ///           MODIFICO:
    ///     FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal, String Texto, String Valor, String Seleccion)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<Seleccione>", "0"));
            foreach (DataRow row in Dt_Temporal.Rows)
            {
                Obj_DropDownList.Items.Add(new ListItem(row[Texto].ToString(), row[Valor].ToString()));
            }
            Obj_DropDownList.SelectedValue = Seleccion;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<Seleccione>", "0"));
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo
    ///DESCRIPCIÓN: Envia un correo a el encargado de Almacen para notificar que una solicitud ha sido creada 
    ///             o que hay alguna que no ha sido revisada.
    ///PROPIEDADES: 
    ///             1.  cabecera.       Número de Solicitud de la cual se quieren obtener sus detalles.
    ///             2.  no_apartado.    Número de Apartado de la cual se le quiere notificar al encargado
    ///                                 del almacen.
    ///             3.  solicito.       Nombre de quien hizo la solicitud del apartado.
    ///             4.  fecha.          Fecha del Proyecto del cual se hace la solicitud de Apartado.
    ///             5.  proyecto.       Proyecto del cual se hace la solicitud de apartado.
    ///             6.  descripcion.    Descripción del Proyecto del cual se hizó la solicitud de apartado.
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:    Francisco Antonio Gallardo Castañeda.
    ///FECHA_MODIFICO:  Junio 2010.
    ///CAUSA_MODIFICACIÓN:  Se adapto para que el funcionamiento del Catalogo de Solicitud de Apartado. 
    ///*******************************************************************************
    public void Enviar_Correo()
    {
        try
        {
            if (Peticion_Actual.P_Email != "" && Peticion_Actual.P_Email != null)
            {
                Cls_Mail mail = new Cls_Mail();

                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = Peticion_Actual.P_Email;
                mail.P_Subject = "Municipio de Irapuato ( Solución de Petición )";
                mail.P_Texto = "Con un cordial saludo me permito enviarle la solución dada a su petición";
                mail.P_Adjunto = Hacer_Pdf();
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        Img_Warning.Visible = true;
        Lbl_Warning.Text += P_Mensaje + "</br>";
        SetFocus(Lbl_Warning);
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Sobrecarga del método que oculta la imagen y limpia el mensaje de error (no recibe parámetros)
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error()
    {
        Img_Warning.Visible = false;
        Lbl_Warning.Text = "";
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Hacer_Pdf
    ///DESCRIPCION : Genera el reporte de CrystalReports y lo convierte a formato PDF
    ///PARAMETROS  : Peticion: Clase de negocio de peticion
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 03-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private String Hacer_Pdf()
    {
        try
        {
            ReportDocument Reporte = new ReportDocument();
            string filePath = Server.MapPath("../Rpt/Atencion_Ciudadana/Rpt_Correo_Solucion.rpt");
            Reporte.Load(filePath);
            DataTable Temporal_Correo = Peticion_Actual.Consulta_Peticion_Correo_Solucion();
            Temporal_Correo.Rows[0][Presidencia.Constantes.Ope_Ate_Peticiones.Campo_Nombre_Solicitante] = Temporal_Correo.Rows[0][Presidencia.Constantes.Ope_Ate_Peticiones.Campo_Nombre_Solicitante] + " " + Temporal_Correo.Rows[0][Presidencia.Constantes.Ope_Ate_Peticiones.Campo_Apellido_Paterno] + " " + Temporal_Correo.Rows[0][Presidencia.Constantes.Ope_Ate_Peticiones.Campo_Apellido_Materno];
            Reporte.SetDataSource(Temporal_Correo);
            Reporte.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Server.MapPath("~\\Reporte").ToString() + "\\" + Peticion_Actual.P_Folio + ".pdf");
            Reporte.Dispose();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Server.MapPath("~").ToString() + "\\Reporte\\" + Peticion_Actual.P_Folio + ".pdf";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo_Aviso
    ///DESCRIPCIÓN: Envia un correo a el encargado de Almacen para notificar que una solicitud ha sido creada 
    ///             o que hay alguna que no ha sido revisada.
    ///PROPIEDADES: 
    ///             1.  cabecera.       Número de Solicitud de la cual se quieren obtener sus detalles.
    ///             2.  no_apartado.    Número de Apartado de la cual se le quiere notificar al encargado
    ///                                 del almacen.
    ///             3.  solicito.       Nombre de quien hizo la solicitud del apartado.
    ///             4.  fecha.          Fecha del Proyecto del cual se hace la solicitud de Apartado.
    ///             5.  proyecto.       Proyecto del cual se hace la solicitud de apartado.
    ///             6.  descripcion.    Descripción del Proyecto del cual se hizó la solicitud de apartado.
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:    Jesus Toledo Rodriguez.
    ///FECHA_MODIFICO:  Septiembre 2010.
    ///CAUSA_MODIFICACIÓN:  Se adapto para que el funcionamiento del Catalogo de Registro de peticiones.
    ///*******************************************************************************        
    public void Enviar_Correo_Aviso(String P_Dependencia, String P_Area)
    {
        String P_Email;
        try
        {
            P_Email = Cls_Cat_Ate_Peticiones_Datos.Correo_Jefe(P_Dependencia, P_Area);
            if (P_Email != "" && P_Email != null)
            {
                Cls_Mail mail = new Cls_Mail();
                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = P_Email;
                mail.P_Subject = "Municipio de Irapuato ( Petición )";
                string Str_Mensaje = "Le ha sido asignada una peticion ciudadana con numero de folio ";
                Str_Mensaje += Txt_Folio.Text.Trim() + " para que usted le de seguimiento y solucion </br> con fecha ";
                Str_Mensaje += String.Format("{0:dd/MMM/yyyy}", DateTime.Today).ToString() + " a las " + DateTime.Today.ToString().Substring(11).ToString();
                mail.P_Texto = Str_Mensaje;
                mail.P_Adjunto = null;
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            //Se muestra el error 
            Mensaje_Error("Error al enviar correo [" + Ex.Message + "]");
        }
    }
    ///****************************************************************************************
    ///NOMBRE_FUNCIÓN:Ver_Seguimiento
    ///DESCRIPCIÓN : Consulta la tabla seguimiento y observaciones, muestra los resultados 
    ///             en los grids correspondientes y guarda en variable de sesión el datatable 
    ///             obtenido de la consulta
    ///PARAMETROS  : 
    /// 		1. No_Peticion: número de petición a consultar
    /// 		2. Anio_Peticion: año de la petición a consultar
    /// 		3. Programa_ID: id del programa de la petición a consultar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 23-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Ver_Seguimiento(string No_Peticion, int Anio_Peticion, string Programa_ID)
    {
        var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
        DataTable Dt_Seguimiento;
        DataTable Dt_Observaciones;
        DataTable Dt_Archivos;

        Obj_Peticiones.P_No_Peticion = No_Peticion;
        Obj_Peticiones.P_Anio_Peticion = Anio_Peticion;
        Obj_Peticiones.P_Programa_ID = Programa_ID;
        Dt_Seguimiento = Obj_Peticiones.Consulta_Peticion_Seguimiento();

        Grid_Seguimiento.DataSource = Dt_Seguimiento;
        Grid_Seguimiento.DataBind();

        Dt_Observaciones = Obj_Peticiones.Consulta_Observaciones_Peticion();
        Grid_Observaciones.DataSource = Dt_Observaciones;
        Grid_Observaciones.DataBind();

        // si el grid observaciones no contiene filas, ocultarlo
        if (Grid_Observaciones.Rows.Count <= 0)
        {
            Contenedor_Grid_Historial.Visible = false;
        }
        else
        {
            Contenedor_Grid_Historial.Visible = true;
        }

        // consultar archivos de la petición y cargar en el grid
        Dt_Archivos = Obj_Peticiones.Consulta_Archivos_Peticion();
        Grid_Archivos_Peticiones.Columns[1].Visible = true;
        Grid_Archivos_Peticiones.DataSource = Dt_Archivos;
        Grid_Archivos_Peticiones.DataBind();
        Grid_Archivos_Peticiones.Columns[1].Visible = false;

        // si el grid archivos no contiene filas, ocultarlo
        if (Grid_Archivos_Peticiones.Rows.Count <= 0)
        {
            Contenedor_Grid_Archivos.Visible = false;
        }
        else
        {
            Contenedor_Grid_Archivos.Visible = true;
        }

        Session["Dt_Seguimiento"] = Dt_Seguimiento;
        Session["Dt_Observaciones"] = Dt_Observaciones;
        ViewState["SortDirection"] = "DESC";
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Peticiones_Pendientes
    ///DESCRIPCIÓN: Ejecuta consulta de peticiones pendientes y las carga en el grid peticiones
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 23-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Consultar_Peticiones_Pendientes()
    {
        var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();
        DataTable Dt_Peticiones = null;

        // si hay una dependencia seleccionada, consultar peticiones de esa dependencia
        if (Cmb_Dependencia_Peticiones.SelectedIndex > 0)
        {
            Obj_Peticiones.P_Estatus = "EN PROCESO";
            Obj_Peticiones.P_Por_Validar = "NO";
            Obj_Peticiones.P_Dependencia_ID = Cmb_Dependencia_Peticiones.SelectedValue;
            Dt_Peticiones = Obj_Peticiones.Consulta_Peticion_Bandeja();
            Cargar_Datos_Grid_Peticiones(Dt_Peticiones, null);
        }

        Session["Dt_Peticiones"] = Dt_Peticiones;
        ViewState["SortDirection"] = "DESC";
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Datos_Grid_Peticiones
    ///DESCRIPCIÓN: Carga en el grid Peticiones los datos recibidos como parámetro
    ///PARÁMETROS:
    /// 		1. Dt_Table: tabla con los datos a cargar en el grid
    /// 		2. Orden: orden para los datos (se utiliza en el DefaultView de la tabla)
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 23-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Datos_Grid_Peticiones(DataTable Dt_Table, string Orden)
    {
        if (Dt_Table != null && !string.IsNullOrEmpty(Orden))
        {
            Dt_Table.DefaultView.Sort = Orden;
        }

        // mostrar resultado en grid peticiones
        Grid_Peticiones.Columns[1].Visible = true;
        Grid_Peticiones.Columns[2].Visible = true;
        Grid_Peticiones.Columns[3].Visible = true;
        Grid_Peticiones.Columns[4].Visible = true;
        Grid_Peticiones.DataSource = Dt_Table;
        Grid_Peticiones.DataBind();
        Grid_Peticiones.Columns[1].Visible = false;
        Grid_Peticiones.Columns[2].Visible = false;
        Grid_Peticiones.Columns[3].Visible = false;
        Grid_Peticiones.Columns[4].Visible = false;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cargar_Datos_Peticion
    ///DESCRIPCIÓN: Consulta los datos de la petición expecificada y los carga en los constroles correspondientes
    ///PARÁMETROS:
    /// 		1. No_Peticion: número de petición a consultar
    /// 		2. Anio_Peticion: año de la petición a consultar
    /// 		3. Programa_ID: id del programa de la petición a consultar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 23-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Cargar_Datos_Peticion(string No_Peticion, int Anio_Peticion, string Programa_ID)
    {
        var Obj_Peticiones = new Cls_Cat_Ate_Peticiones_Negocio();

        Obj_Peticiones.P_No_Peticion = No_Peticion;
        Obj_Peticiones.P_Anio_Peticion = Anio_Peticion;
        Obj_Peticiones.P_Programa_ID = Programa_ID;
        DataTable Dt_Temporal = Obj_Peticiones.Consulta_Peticion();

        // si la consulta arroja resultados, cargar datos
        if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
        {
            DataRow Renglon = Dt_Temporal.Rows[0];

            Txt_Folio.Text = Renglon[Ope_Ate_Peticiones.Campo_Folio].ToString();
            Txt_Nombre.Text = Renglon["NOMBRE_COMPLETO_SOLICITANTE"].ToString();
            Hdn_Email.Value = Renglon[Ope_Ate_Peticiones.Campo_Email].ToString();
            Txt_Domicilio.Text = Renglon["CALLE"].ToString() + " " + Renglon[Ope_Ate_Peticiones.Campo_Numero_Exterior].ToString() + " " + Renglon["COLONIA"].ToString();
            Txt_Referencia.Text = Renglon[Ope_Ate_Peticiones.Campo_Referencia].ToString();
            Txt_Descripcion.Text = Renglon[Ope_Ate_Peticiones.Campo_Descripcion_Peticion].ToString();
            DateTime Fecha_Peticion;
            DateTime.TryParse(Renglon[Ope_Ate_Peticiones.Campo_Fecha_Peticion].ToString(), out Fecha_Peticion);
            if (Fecha_Peticion != DateTime.MinValue)
            {
                Txt_Fecha.Text = Fecha_Peticion.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha.Text = "";
            }

            // cargar los valores de los combos revisando primero que el valor a seleccionar exista entre los elementos de cada combo
            if (Cmb_Dependencia.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Dependencia_ID].ToString()) != null)
            {
                Cmb_Dependencia.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Dependencia_ID].ToString();
            }
            if (Cmb_Asunto.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Asunto_ID].ToString()) != null)
            {
                Cmb_Asunto.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Asunto_ID].ToString();
            }
            if (Cmb_Estatus.Items.FindByValue(Renglon[Ope_Ate_Peticiones.Campo_Estatus].ToString()) != null)
            {
                Cmb_Estatus.SelectedValue = Renglon[Ope_Ate_Peticiones.Campo_Estatus].ToString();
            }
            else // si no encuentra estatus, seleccionar EN PROCESO
            {
                Cmb_Estatus.SelectedValue = "EN PROCESO";
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.- Data_Set.- contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Ds_Reporte, contiene la Ruta del Reporte a mostrar en pantalla
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 30/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Reporte(DataSet Data_Set, DataSet Ds_Reporte, string Nombre_Reporte)
    {

        ReportDocument Reporte = new ReportDocument();
        string File_Path = Server.MapPath("../Rpt/Atencion_Ciudadana/" + Nombre_Reporte);

        Reporte.Load(File_Path);
        DataRow Renglon;

        for (int i = 0; i < Data_Set.Tables[0].Rows.Count; i++)
        {
            Renglon = Data_Set.Tables[0].Rows[i];
            Ds_Reporte.Tables[0].ImportRow(Renglon);
        }
        Reporte.SetDataSource(Ds_Reporte);

        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Consulta_Peticiones.pdf");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        // mostrar reporte
        String Ruta = "../../Reporte/Rpt_Consulta_Peticiones.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Archivos_Adjuntos
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para los archivos adjuntos
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 20-jun-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Archivos_Adjuntos()
    {
        DataTable Dt_Bienes = new DataTable();
        Dt_Bienes.Columns.Add(new DataColumn("ARCHIVO", typeof(byte[])));
        Dt_Bienes.Columns.Add(new DataColumn("RUTA_ARCHIVO", typeof(String)));
        return Dt_Bienes;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Archivo
    ///DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    ///PARÁMETROS:
    /// 		1. Nombre_Archivo: Nombre del reporte a generar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 21-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Archivo(String Nombre_Archivo)
    {
        String Pagina = "Frm_Ate_Mostrar_Archivos.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Archivo;
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Formato",
                "window.open('" + Pagina +
                "', 'Mostrar_Archivo','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el archivo. Error: [" + Ex.Message + "]");
        }
    }

    #endregion METODOS

    #region  Grid

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Seguimiento_Sorting
    ///DESCRIPCIÓN: Maneja el evento cambio del orden del grid, toma los datos de variables de sesión
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Seguimiento_Sorting(object sender, GridViewSortEventArgs e)
    {
        Mensaje_Error();

        try
        {
            if (Session["Dt_Seguimiento"] != null)
            {
                DataTable Dt_Seguimiento = (DataTable)Session["Dt_Seguimiento"];
                String Orden = ViewState["SortDirection"].ToString();
                if (Orden.Equals("ASC"))
                {
                    Dt_Seguimiento.DefaultView.Sort = e.SortExpression + " DESC";
                    Grid_Seguimiento.DataSource = Dt_Seguimiento;
                    Grid_Seguimiento.DataBind();
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dt_Seguimiento.DefaultView.Sort = e.SortExpression + " ASC";
                    Grid_Seguimiento.DataSource = Dt_Seguimiento;
                    Grid_Seguimiento.DataBind();
                    ViewState["SortDirection"] = "ASC";
                }
            }
        }
        catch (Exception Ex)
        {
            //Se muestra el error 
            Mensaje_Error("Error [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Observaciones_Sorting
    ///DESCRIPCIÓN: Maneja el evento cambio del orden del grid, toma los datos de variables de sesión
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 23-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Observaciones_Sorting(object sender, GridViewSortEventArgs e)
    {
        Mensaje_Error();

        try
        {
            if (Session["Dt_Observaciones"] != null)
            {
                DataTable Dt_Observaciones = (DataTable)Session["Dt_Observaciones"];
                String Orden = ViewState["SortDirection"].ToString();
                if (Orden.Equals("ASC"))
                {
                    Dt_Observaciones.DefaultView.Sort = e.SortExpression + " DESC";
                    Grid_Observaciones.DataSource = Dt_Observaciones;
                    Grid_Observaciones.DataBind();
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dt_Observaciones.DefaultView.Sort = e.SortExpression + " ASC";
                    Grid_Observaciones.DataSource = Dt_Observaciones;
                    Grid_Observaciones.DataBind();
                    ViewState["SortDirection"] = "ASC";
                }
            }
        }
        catch (Exception Ex)
        {
            //Se muestra el error 
            Mensaje_Error("Error [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Peticiones_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento cambio índice del grid, llama al método que carga los detalles 
    ///         de una petición en la página
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 23-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Peticiones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Mensaje_Error();
        int Anio_Peticion;

        try
        {
            //si hay una fila seleccionada, llamar al método que carga los datos en la página
            if (Grid_Peticiones.SelectedIndex > -1)
            {
                Estado_Botones(Const_Estado_Mostrar_Detalles);
                //Limpiar_Campos();
                int.TryParse(Grid_Peticiones.SelectedRow.Cells[2].Text, out Anio_Peticion);
                Cargar_Datos_Peticion(Grid_Peticiones.SelectedRow.Cells[1].Text, Anio_Peticion, Grid_Peticiones.SelectedRow.Cells[3].Text);

                Lbl_Seguimiento.Visible = true;
                Grid_Seguimiento.Visible = true;
                Ver_Seguimiento(Grid_Peticiones.SelectedRow.Cells[1].Text, Anio_Peticion, Grid_Peticiones.SelectedRow.Cells[3].Text);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Peticiones_OnRowDataBound
    ///DESCRIPCIÓN: Cambia la imagen en el grid dependiendo de la cercanía de la fecha de solución probable de la petición
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 26-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Peticiones_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DateTime Fecha_Solución_Probable;
        ImageButton Btn_Seleccion_Grid;

        Mensaje_Error();
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // validar que el estatus sea EN PROCESO
                if (e.Row.Cells[9].Text == "EN PROCESO")
                {
                    // validar que contenga una fecha de solución probable
                    if (DateTime.TryParse(e.Row.Cells[4].Text, out Fecha_Solución_Probable) && e.Row.Cells[0].Controls.Count > 0)
                    {
                        Btn_Seleccion_Grid = (ImageButton)e.Row.Cells[0].Controls[0];
                        // si la diferencia entre la fecha de solución y la actual es menor a 5, poner botón verde
                        if ((Fecha_Solución_Probable - DateTime.Now).TotalDays >= 5)
                        {
                            Btn_Seleccion_Grid.ImageUrl = "~/paginas/imagenes/gridview/circle_green.png";
                        }
                        // si la diferencia entre la fecha de solución y la actual es menor a 5, poner botón amarillo
                        else if ((Fecha_Solución_Probable - DateTime.Now).TotalDays < 5 && (Fecha_Solución_Probable - DateTime.Now).TotalDays >= 2)
                        {
                            Btn_Seleccion_Grid.ImageUrl = "~/paginas/imagenes/gridview/circle_yellow.png";
                        }
                        // si la diferencia entre la fecha de solución y la actual es menor a 2, poner botón rojo
                        else if ((Fecha_Solución_Probable - DateTime.Now).TotalDays < 2)
                        {
                            Btn_Seleccion_Grid.ImageUrl = "~/paginas/imagenes/gridview/circle_red.png";
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Peticiones_Sorting
    ///DESCRIPCIÓN: Maneja el evento cambio del orden del grid, toma los datos de variables de sesión
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Peticiones_Sorting(object sender, GridViewSortEventArgs e)
    {
        Mensaje_Error();

        try
        {
            if (Session["Dt_Peticiones"] != null)
            {
                DataTable Dt_Peticiones = (DataTable)Session["Dt_Peticiones"];
                String Orden = ViewState["SortDirection"].ToString();
                if (Orden.Equals("ASC"))
                {
                    Cargar_Datos_Grid_Peticiones(Dt_Peticiones, e.SortExpression + " DESC");
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Cargar_Datos_Grid_Peticiones(Dt_Peticiones, e.SortExpression + " ASC");
                    ViewState["SortDirection"] = "ASC";
                }
            }
        }
        catch (Exception Ex)
        {
            //Se muestra el error 
            Mensaje_Error("Error [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Archivos_SelectedIndexChanged
    /// DESCRIPCIÓN: Manejo del evento cambio de índice seleccionado en el grid Archivos
    ///             Eliminar la fila seleccionada de la tabla Dt_Archivos en la variable de sesión
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 20-jun-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Archivos_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Archivos = (DataTable)Session["Dt_Archivos"];
        // eliminar de la tabla el registro seleccionado
        Dt_Archivos.Rows.RemoveAt(Grid_Archivos.SelectedIndex);
        Grid_Archivos.DataSource = Dt_Archivos;
        Grid_Archivos.DataBind();
        Session["Dt_Archivos"] = Dt_Archivos;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Archivos_Peticiones_OnSelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento cambio índice del grid, llama al método muestra el archivo seleccionado
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 21-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Archivos_Peticiones_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Mensaje_Error();

        try
        {
            //si hay una fila seleccionada, llamar al método que carga los datos en la página
            if (Grid_Archivos_Peticiones.SelectedIndex > -1)
            {
                Mostrar_Archivo(Grid_Archivos_Peticiones.SelectedRow.Cells[1].Text.Replace("\\", "/"));
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Archivos_Peticiones_OnRowDataBound
    /// DESCRIPCIÓN: Cambia el contenido de la columna archivo para que sólo contenga el nombre del archivo
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-jun-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Archivos_Peticiones_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        Mensaje_Error();

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // asignar a la celda 3 sólo el nombre del archivo
                e.Row.Cells[3].Text = Path.GetFileName(e.Row.Cells[3].Text);
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error("Grid_Bienes_RowDataBound: " + ex.Message);
        }
    }

    #endregion Grid

    #region EVENTOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Asunto_SelectedIndexChanged
    ///DESCRIPCIÓN: Si se selecciona un asunto se consulta la dependencia a la que pertenece y se selecciona la dependencia seleccionada
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Asunto_SelectedIndexChanged(object sender, EventArgs e)
    {
        var Obj_Asuntos = new Cls_Cat_Ate_Asuntos_Negocio();
        DataTable Dt_Asuntos;

        Mensaje_Error();

        try
        {
            // si hay una asunto seleccionado, consultar la dependencia de ese Asunto
            if (Cmb_Asunto.SelectedIndex > 0)
            {
                Obj_Asuntos.P_ID = Cmb_Asunto.SelectedValue;
                Dt_Asuntos = Obj_Asuntos.Consultar_Registros();
                // validar que la tabla contiene registros
                if (Dt_Asuntos != null && Dt_Asuntos.Rows.Count > 0)
                {
                    string Dependencia_ID = Dt_Asuntos.Rows[0][Cat_Ate_Asuntos.Campo_DependenciaID].ToString();
                    // si el combo dependencias contiene un elemento con el id de la dependencia en la consulta, seleccionar el elemento
                    if (Cmb_Dependencia.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencia.SelectedValue = Dependencia_ID;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("No se pudo mostrar información: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Dependencia_SelectedIndexChanged
    ///DESCRIPCIÓN: Si se selecciona un dependencia, se consultan los asuntos de la dependencia seleccionada
    ///         y se cargan en el combo Asunto
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        var Obj_Asuntos = new Cls_Cat_Ate_Asuntos_Negocio();

        Mensaje_Error();
        try
        {
            // si hay una dependencia seleccionada, agregar filtro al Obj_Asuntos
            if (Cmb_Dependencia.SelectedIndex > 0)
            {
                Obj_Asuntos.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
            }

            // cargar el combo asuntos con los resultados de la consulta
            Llenar_Combo_Con_DataTable(Cmb_Asunto, Obj_Asuntos.Consultar_Registros());
        }
        catch (Exception Ex)
        {
            Mensaje_Error("No se pudo mostrar información: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Dependencia_Peticiones_SelectedIndexChanged
    ///DESCRIPCIÓN: Si se selecciona un dependencia, se consultan los asuntos de la dependencia seleccionada
    ///         y se cargan en el combo Asunto
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Dependencia_Peticiones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Mensaje_Error();
        try
        {
            Consultar_Peticiones_Pendientes();
        }
        catch (Exception Ex)
        {
            Mensaje_Error("No se pudo mostrar información: " + Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Asunto_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID del asunto seleccionado en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Asunto_Click(object sender, ImageClickEventArgs e)
    {
        Mensaje_Error();

        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_ASUNTOS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_ASUNTOS"]) == true)
            {
                try
                {
                    // volver a cargar combo asuntos
                    var Obj_Asunto = new Cls_Cat_Ate_Asuntos_Negocio();
                    Obj_Asunto.P_Estatus = "ACTIVO";
                    Llenar_Combo_Con_DataTable(Cmb_Asunto, Obj_Asunto.Consultar_Registros());

                    string Asunto_ID = Session["ASUNTO_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo asunto contiene el ID, seleccionar
                    if (Cmb_Asunto.Items.FindByValue(Asunto_ID) != null)
                    {
                        Cmb_Asunto.SelectedValue = Asunto_ID;
                        Cmb_Asunto_SelectedIndexChanged(null, null);
                    }
                }
                catch (Exception Ex)
                {
                    Mensaje_Error(Ex.Message);
                }

                // limpiar variables de sesión
                Session.Remove("ASUNTO_ID");
                Session.Remove("NOMBRE_ASUNTO");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_ASUNTOS");
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
        Mensaje_Error();

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
                        Cmb_Dependencia_SelectedIndexChanged(null, null);
                    }
                }
                catch (Exception Ex)
                {
                    Mensaje_Error(Ex.Message);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Dependencia_Filtro_Inicial_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada y seleccionarlo en el combo Cmb_Dependencia_Peticiones para filtrar peticiones
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 13-jun-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Dependencia_Filtro_Inicial_Click(object sender, ImageClickEventArgs e)
    {
        Mensaje_Error();

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
                    if (Cmb_Dependencia_Peticiones.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencia_Peticiones.SelectedValue = Dependencia_ID;
                        Cmb_Dependencia_Peticiones_SelectedIndexChanged(null, null);
                    }
                }
                catch (Exception Ex)
                {
                    Mensaje_Error(Ex.Message);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Solucion_Click
    ///DESCRIPCIÓN: llama al método que configura los botones para ingresar una solución
    ///         o si ya se ingresó la solución, llama al método que guarda los datos
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Btn_Solucion_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mensaje_Error();
            if (Btn_Solucion.AlternateText == "Solución")
            {
                Estado_Botones(Const_Estado_Solucion);
            }
            else
            {
                if (Validar_Guardar())
                {
                    Guardar_Solucion();
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Subir_Archivo_Click
    /// DESCRIPCIÓN: guardar archivo en variable de sesion Dt_Archivos
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 02-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Subir_Archivo_Click(object sender, ImageClickEventArgs e)
    {
        // arreglo con las extensiones de archivo permitidas
        String[] Extensiones_Permitidas = { ".jpg", ".jpeg", ".png", ".gif", ".doc", ".docx", ".ppt", ".pptx", ".pdf" };
        DataTable Dt_Archivos;
        DataRow Dr_Archivo;

        // si la sesión ya existe, recuperar tabla, si no, llamar método que crea la tabla
        if (Session["Dt_Archivos"] != null)
        {
            Dt_Archivos = (DataTable)Session["Dt_Archivos"];
        }
        else
        {
            Dt_Archivos = Crear_Tabla_Archivos_Adjuntos();
        }

        // validar que el control trae un archivo
        if (Fup_Subir_Archivo.HasFile)
        {
            String Extension_Archivo = Path.GetExtension(Fup_Subir_Archivo.FileName).ToLower();

            // si la extensión del archivo recibido no es válida, mostrar mensaje y regresar
            if (Array.IndexOf(Extensiones_Permitidas, Extension_Archivo) < 0)
            {
                Mensaje_Error(" No se permite subir archivos con extensión: " + Extension_Archivo);
                return;
            }
            // si la longitud del archivo recibido es mayor que 2MB, mostrar mensaje
            if (Fup_Subir_Archivo.FileBytes.Length > 2048000)
            {
                Mensaje_Error(" El tamaño del archivo es mayor al permitido.");
                return;
            }

            // agregar datos al tabla en una nueva fila
            Dr_Archivo = Dt_Archivos.NewRow();
            Dr_Archivo["ARCHIVO"] = Fup_Subir_Archivo.FileBytes;
            Dr_Archivo["RUTA_ARCHIVO"] = HttpUtility.HtmlEncode(Fup_Subir_Archivo.FileName.Replace(' ', '_').Replace("'", "").Replace("$", ""));
            Dt_Archivos.Rows.Add(Dr_Archivo);    //Se asigna la nueva fila a la tabla
            Grid_Archivos.DataSource = Dt_Archivos;
            Grid_Archivos.DataBind();
            Session["Dt_Archivos"] = Dt_Archivos;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: si el botón tiene el texto "Salir", se redirecciona a la página principal,
    ///             si no, se llama al método que inicializa los controles
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mensaje_Error();

            Session.Remove("Dt_Archivos");

            if (Btn_Salir.AlternateText == "Salir")
            {
                Session.Remove("Dt_Seguimiento");
                Session.Remove("Dt_Peticiones");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Grid_Peticiones.SelectedIndex = -1;
                Estado_Botones(Const_Estado_Inicial);
                Limpiar_Controles();
                Consultar_Peticiones_Pendientes();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Peticion_Click
    ///DESCRIPCIÓN: consulta la petición seleccionada y llama al método que muestra el reporte
    ///PARAMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 13-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Peticion_Click(object sender, ImageClickEventArgs e)
    {
        var Neg_Consulta_Peticiones = new Cls_Ope_Consulta_Peticiones_Negocio();

        Mensaje_Error();

        try
        {
            // validar que haya un folio
            if (Txt_Folio.Text.Length > 0)
            {
                Neg_Consulta_Peticiones.P_Folio = Txt_Folio.Text;
                DataSet Data_Set = Neg_Consulta_Peticiones.Consulta_Peticion_Detallada();
                if (Data_Set != null && Data_Set.Tables.Count > 0 && Data_Set.Tables[0].Rows.Count > 0)
                {
                    string Domicilio = Data_Set.Tables[0].Rows[0]["DIRECCION"].ToString();
                    string Referencia = Data_Set.Tables[0].Rows[0][Ope_Ate_Peticiones.Campo_Referencia].ToString().Trim();
                    // si hay una referencia, agregarla a la dirección
                    if (Domicilio.Length > 0 && Referencia.Length > 0)
                    {
                        Domicilio += "\\r\\n" + Referencia;
                    }
                    else if (Domicilio.Length <= 0 && Referencia.Length > 0)
                    {
                        Domicilio = Referencia;
                    }
                    Data_Set.Tables[0].Rows[0].BeginEdit();
                    Data_Set.Tables[0].Rows[0]["DIRECCION"] = Domicilio;
                    Data_Set.Tables[0].AcceptChanges();

                    Ds_Ope_Consulta_Peticiones_Especifico ds_consulta_peticiones = new Ds_Ope_Consulta_Peticiones_Especifico();
                    Generar_Reporte(Data_Set, ds_consulta_peticiones, "Rpt_Ope_Consulta_Peticiones_Especifico.rpt");
                }
            }
            else
            {
                Mensaje_Error("No hay folio para imprimir.");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Error al imprimir petición: " + Ex.Message);
        }
    }

    #endregion EVENTOS
}