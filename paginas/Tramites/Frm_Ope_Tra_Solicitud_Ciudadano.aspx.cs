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
using Presidencia.Solicitud_Tramites.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using AjaxControlToolkit;
using System.IO;
using System.Text.RegularExpressions;

public partial class paginas_Tramites_Frm_Ope_Tra_Solicitud : System.Web.UI.Page
{
    #region Variables
    //objeto de la clase de negocio de solicitudes_tramites para acceder a la clase de datos y realizar copnexion
    private Cls_Ope_Solicitud_Tramites_Negocio Solicitud_Negocio;
    //variable para guardar los datos de la consulta de datos de un tramite
    private static DataSet Ds_Datos;
    //ariable para guardar los datos de la consulta de documentos de un tramite
    private static DataSet Ds_Documentos;
    //variable para guardar los datos de la consulta de un tramite
    private static DataSet Ds_Tramites;
    #endregion 

    #region Métodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Extension
    ///DESCRIPCIÓN: Maneja la extencion del archivo
    ///PROPIEDADES: String Ruta, direccion que 
    ///contiene el nombre del archivo al cual se le sacara la extension
    ///CREO: Francisco Gallardo
    ///FECHA_CREO: 16/Marzo/2010
    ///MODIFICO: Silvia Morales
    ///FECHA_MODIFICO: 19/Octubre/2010
    ///CAUSA_MODIFICACIÓN: Se adecuo al estandar
    ///*******************************************************************************
    private string Obtener_Extension(String Ruta)
    {
        String Extension = "";
        int index = Ruta.LastIndexOf(".");
        if (index < Ruta.Length)
        {
            Extension = Ruta.Substring(index + 1);
        }
        return Extension;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: CRefrescar_Grid_Documentos
    ///DESCRIPCIÓN: Carga el grid de documentos 
    ///con los documentos aplicables al tramite q se escogio y hace vissible el grid
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Refrescar_Grid_Documentos()
    {
        Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
        Ds_Documentos = new DataSet();
        Ds_Documentos = Solicitud_Negocio.Consultar_Documentos_Tramite();
        if (Ds_Documentos.Tables[0].Rows.Count > 0)
        {
            Grid_Documentos.Visible = true;
            Grid_Documentos.DataSource = Ds_Documentos;
            Grid_Documentos.DataBind();
            
            Lbl_Documentos_Requeridos.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Refrescar_Grid_Documentos_Modificar
    ///DESCRIPCIÓN: Carga el grid de documentos
    ///con los documentos aplicables al tramite q se escogio y hace vissible el grid
    ///este ocurre cuando se realiza una busqueda
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Refrescar_Grid_Documentos_Modificar()
    {
        Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
        Ds_Documentos = new DataSet();
        Ds_Documentos = Solicitud_Negocio.Consultar_Documentos_Tramite();
        if (Ds_Documentos.Tables[0].Rows.Count > 0)
        {
            Grid_Documentos_Modificar.Visible = true;
            Grid_Documentos_Modificar.DataSource = Ds_Documentos;
            Grid_Documentos_Modificar.DataBind();

            Lbl_Documentos_Requeridos.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: CRefrescar_Grid_Datos
    ///DESCRIPCIÓN: Carga el grid de datos 
    ///con los datos aplicables al tramite q se escogio y hace vissible el grid
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Refrescar_Grid_Datos()
    {
        Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
        Ds_Datos = new DataSet();
        Ds_Datos = Solicitud_Negocio.Consultar_Datos_Tramite(); ;
        if (Ds_Datos.Tables[0].Rows.Count > 0)
        {
            Grid_Datos.DataSource = Ds_Datos;
            Grid_Datos.DataBind();
            Grid_Datos.Visible = true;
            Lbl_Datos_Requeridos.Visible = true;

        }
    }

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
        Txt_Folio.Text = "";
        Txt_Apellido_Materno.Text = "";
        Txt_Apellido_Paterno.Text = "";
        Txt_Nombre.Text = "";
        Txt_Costo.Text = "";
        Txt_Tiempo_Estimado.Text = "";
        Txt_Email.Enabled = true;
        Txt_Nombre.Enabled = true;
        Txt_Apellido_Paterno.Enabled = true;
        Txt_Apellido_Materno.Enabled = true;
        Cmb_Tramite.SelectedIndex = 0;
        Cmb_Tramite.Enabled = true;
        
        
        
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
        Txt_Folio.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Tramite.SelectedIndex = 0;
        Cmb_Tramite.Enabled = false;
        Cmb_Estatus.Enabled = false;
        Txt_Email.Enabled = false;
        Txt_Email.Text = "";
        Txt_Apellido_Materno.Text = "";
        Txt_Apellido_Paterno.Text = "";
        Txt_Nombre.Text = "";
        Txt_Costo.Text = "";
        Txt_Tiempo_Estimado.Text = "";
        Txt_Avance.Text = "";
        Txt_Nombre.Enabled = false;
        Txt_Apellido_Paterno.Enabled = false;
        Txt_Apellido_Materno.Enabled = true;
        Solicitud_Negocio.P_Clave_Solicitud = null;
        Solicitud_Negocio.P_Datos = null;
        Solicitud_Negocio.P_Documentos = null;
        Solicitud_Negocio.P_Estatus = null;
        Solicitud_Negocio.P_Porcentaje = null;
        Solicitud_Negocio.P_Solicitud_ID = null;
        Solicitud_Negocio.P_Tramite_ID = null;
        Solicitud_Negocio.P_Apellido_Materno = null;
        Solicitud_Negocio.P_Apellido_Paterno = null;
        Solicitud_Negocio.P_Nombre_Solicitante = null;
        Grid_Datos.Visible = false;
        Grid_Documentos.Visible = false;
        Grid_Documentos_Modificar.Visible = false;
        Lbl_Datos_Requeridos.Visible = false;
        Lbl_Documentos_Requeridos.Visible = false;
        
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
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Nuevo.ToolTip = "Dar Alta";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.Visible = false;
                Btn_Eliminar.Visible = false;
                break;


            //Click en Modificar
            case 2:
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Eliminar.Visible = false;
                Btn_Nuevo.Visible = false;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                break;

            //Estado Inicial
            case 3:
                Btn_Nuevo.Visible = true;
                Btn_Eliminar.Visible = false;
                Btn_Salir.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Eliminar.ToolTip = "Eliminar";
                Btn_Salir.ToolTip = "Salir";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                break;
            default: break;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Expresion
    ///DESCRIPCIÓN: es un metodo generico para habilitar y deshabilitar todos 
    ///los botones de la forma de acuerdo a sus eventos
    ///PARAMETROS: 1.- Cadena, es la cadena de caracteres que se va a validar
    ///            2.- Tipo_Validacion.- que se requiere que la cadena contenga
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 19/Octubre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public Boolean Validar_Expresion(String Str_Cadena, String Str_Tipo_Validacion)
    {
        String Str_Expresion;
        Str_Expresion = String.Empty;
        //Se seleccion el tipo de valor a validar
        switch (Str_Tipo_Validacion)
        {
            case "String":
                Str_Expresion = "[^a-zA-Z.ÑÁÉÍÓñáéíóúü\\s]";
                break;
            case "Integer":
                Str_Expresion = "[^0-9]";
                break;
            case "Varchar":
                Str_Expresion = "[^a-zA-ZÑÁÉÍÓñáéíóúü\\/\\*,-.()0-9\\s]";
                break;
            case "Date":
                Str_Expresion = "\\d{2}/\\d{2}/\\d{2}";
                break;
            case "Email":
                Str_Expresion = "^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+$";
                //@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){2}(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5]))$";                        
                break;
            case "Password":
                Str_Expresion = "[^a-zA-ZÑÁÉÍÓñáéíóúü.0-9\\s]";
                break;
            case "CURP":
                Str_Expresion = "^[a-zA-Z]{4}(\\d{6})([a-zA-Z]{6})(\\d{2})?$";
                break;
        }

        //Se revisa la expresion
        Regex Exp_Regular = new Regex(Str_Expresion);
        //Regresa un valor true o false segun se cumplan las condiciones
        if (Str_Tipo_Validacion == "Date" || Str_Tipo_Validacion == "Email" || Str_Tipo_Validacion == "CURP")
            return !(Exp_Regular.IsMatch(Str_Cadena));
        else
            return Exp_Regular.IsMatch(Str_Cadena);

    }
    #endregion

    #region Page Load Init
   
    protected void Page_Load(object sender, EventArgs e)
    {
      
       Solicitud_Negocio = new Cls_Ope_Solicitud_Tramites_Negocio();
        
        if (Cmb_Estatus.Items.Count == 0)
        {
            Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
            Cmb_Estatus.Items.Add("PENDIENTE");
            Cmb_Estatus.Items.Add("PROCESO");
            Cmb_Estatus.Items.Add("TERMINADO");
            Cmb_Estatus.Items.Add("DETENIDO");
            Cmb_Estatus.Items.Add("CANCELADO");

            Cmb_Estatus.Items[0].Value = "0";
            Cmb_Estatus.Items[0].Selected = true;
        }
        if (Cmb_Tramite.Items.Count == 0) {
            Ds_Tramites = new DataSet();
            Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Tramite, Ds_Tramites.Tables[0], 3, 0);
            Cmb_Tramite.SelectedIndex = 0;
        }
        if (!IsPostBack)
        {
            Lbl_Informacion.Text = "";
            Lbl_Informacion.Enabled = false;
            Img_Warning.Visible = false;
            Grid_Datos.Visible = false;
            Grid_Documentos.Visible = false;
            Grid_Documentos_Modificar.Visible = false;
            Lbl_Datos_Requeridos.Visible = false;
            Lbl_Documentos_Requeridos.Visible = false;

            Habilitar_Forma();
            Cmb_Estatus.SelectedIndex = 1;
            Txt_Avance.Text = "0";

            Manejar_Botones(1);

            Cmb_Tramite.SelectedIndex = 1;
            Mostrar_Informacion(0);
            if (Cmb_Tramite.SelectedIndex != 0)
            {
                Refrescar_Grid_Datos();
                Refrescar_Grid_Documentos();
                Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
                Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
                Txt_Costo.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Costo].ToString();
                Txt_Tiempo_Estimado.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Tiempo_Estimado].ToString();
            }
        
        
        }
    }
    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Habilita las cajas de texto necesarias para crear un Nueva solicitud de tramite
    ///y coloca un Asunto_Id de forma automatica en la caja de texto de Asunto_Id, se convierte en dar alta
    ///cuando oprimimos Nuevo y dar alta  Crea un registro de una solicitud en la base de datos 
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 16/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Informacion(0);
        String Cadena_Informacion = "";
        Boolean Error = false;
        int Contador = 0;
        if (Btn_Nuevo.ToolTip == "Dar Alta")
        {
           
                if (Validar_Expresion(Txt_Email.Text, "Email"))
                {
                    Lbl_Informacion.Text = "+ Formato de e-mail incorrecto.";
                    Mostrar_Informacion(1);
                }
            
                else
            {
                    if (Cmb_Tramite.SelectedIndex != 0 && Txt_Nombre.Text.Length > 1 &&
                        Txt_Apellido_Paterno.Text.Length > 1)
                    {
                            Solicitud_Negocio.P_Porcentaje = "0";
                            Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
                            Solicitud_Negocio.P_E_Mail = Txt_Email.Text;
                            DataSet Ds_Subproceso = Solicitud_Negocio.Consultar_Subproceso();
                            Solicitud_Negocio.P_Subproceso_ID = Ds_Subproceso.Tables[0].Rows[0].ItemArray[0].ToString();
                            Solicitud_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                            String[,] Datos = new String[Ds_Datos.Tables[0].Rows.Count, 2];
                            String[,] Documentos = new String[Ds_Documentos.Tables[0].Rows.Count, 2];
                            Cadena_Informacion = "Los siguientes datos son requeridos por el sistema para realizar la operación, sea tan amable de verificar.";
                            //llenar matriz de datos
                            for (int i = 0; i < Ds_Datos.Tables[0].Rows.Count; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (j == 0)
                                    {
                                        Datos[i, j] = Ds_Datos.Tables[0].Rows[i].ItemArray[0].ToString();
                                    }//fin if
                                    else
                                    {
                                        String Valor_Dato = ((TextBox)Grid_Datos.Rows[i].FindControl("Txt_Descripcion_Datos")).Text;
                                        if (Valor_Dato != "" ||
                                            (Ds_Datos.Tables[0].Rows[i][Cat_Tra_Datos_Tramite.Campo_Dato_Requerido].ToString()) == "N")
                                        {
                                            Datos[i, j] = Valor_Dato;
                                        }
                                        else
                                        {
                                            Error = true;
                                            Cadena_Informacion = Cadena_Informacion +
                                                "<br/>&nbsp;&nbsp;&nbsp;+ " +
                                                Ds_Datos.Tables[0].Rows[i][Cat_Tra_Datos_Tramite.Campo_Nombre].ToString();
                                            Lbl_Informacion.Text = Cadena_Informacion;
                                            Mostrar_Informacion(1);
                                        }

                                    }//fin else
                                }//fin for j
                            }//fin for i 

                            //lenar matriz documentos
                            for (int i = 0; i < Ds_Documentos.Tables[0].Rows.Count; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (j == 0)
                                    {

                                        Documentos[i, j] = Ds_Documentos.Tables[0].Rows[i].ItemArray[0].ToString();

                                    }//fin if j= 0
                                    if (j == 1)
                                    {
                                        AsyncFileUpload AsFileUp = (AsyncFileUpload)Grid_Documentos.Rows[i].Cells[2].FindControl("FileUp");
                                        String Extension = Obtener_Extension(AsFileUp.FileName);
                                        if (Extension == "pdf" || Extension == "jpg" || Extension == "jpeg")
                                        {                                            
                                            //HttpPostedFile HttpFile = AsFileUp.PostedFile; 
                                            String Directorio_Expediente = "TR-" + 
                                                Solicitud_Negocio.Obtener_Consecutivo(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud, Ope_Tra_Solicitud.Campo_Solicitud_ID);
                                            String Raiz = @Server.MapPath("../../Archivos");
                                            String Direccion_Archivo = "";
                                            //verifica si existe el directorio donde se guardan los archivos
                                            // si no existe lo crea
                                            if (!Directory.Exists(Raiz))
                                            {
                                                Directory.CreateDirectory(Raiz);
                                            }//FIN IF EXISTE DIRECTORIO raiz
                                            String URL = AsFileUp.FileName;
                                            //verifica que ya exista una url osea un archivo seleccionado para ser subido
                                            if (URL != "")
                                            {
                                                //verifica si existe un directorio llamado con ese Nombre_Commando de expediente
                                                if (!Directory.Exists(Raiz + Directorio_Expediente))
                                                {
                                                    Directory.CreateDirectory(Raiz + "/" + Directorio_Expediente);
                                                }//fin if si existe directorio expediente

                                                //se crea el Nombre_Commando del archivo que se va a guardar
                                                Direccion_Archivo = Raiz + "/" + Directorio_Expediente +
                                                    "/" + Server.HtmlEncode(Ds_Documentos.Tables[0].Rows[i].ItemArray[2].ToString() +
                                                    "_" + Ds_Documentos.Tables[0].Rows[i].ItemArray[1].ToString() +
                                                    "." + Obtener_Extension(AsFileUp.FileName));

                                                //se valida que contega un archivo 
                                                if (AsFileUp.HasFile)
                                                {
                                                    //se guarda el archivo
                                                    AsFileUp.SaveAs(Direccion_Archivo);
                                                }//fin if hasFile
                                                //String URL = ((AsyncFileUpload)Grid_Documentos.FindControl("Fup_URL")).FileName.ToString();

                                                Documentos[i, j] = Direccion_Archivo;
                                            }//fin if url
                                            else
                                            {
                                                Error = true;
                                                Cadena_Informacion = Cadena_Informacion +
                                                    "<br/>&nbsp;&nbsp;&nbsp;+ " +
                                                    Ds_Documentos.Tables[0].Rows[i]["DOCUMENTO"].ToString();
                                                Lbl_Informacion.Text = Cadena_Informacion;
                                                Mostrar_Informacion(1);
                                            }
                                        }//fin if extension
                                        else 
                                        {
                                            Error = true;
                                            if (Contador == 0)
                                            {
                                                Cadena_Informacion = Cadena_Informacion + "<br/>&nbsp;&nbsp;&nbsp;+Faltan archivos por subir ó los archivos proporcionados tienen formato distinto a PDF, JPG ó JPGE.";
                                            }
                                            Contador++;
                                            Lbl_Informacion.Text = Cadena_Informacion;
                                            Mostrar_Informacion(1);
                                        }
                                    }//fin if j == 1
                                }//fin for j
                            }//fin for i 

                            if (!Error)
                            {
                                Solicitud_Negocio.P_Datos = Datos;
                                Solicitud_Negocio.P_Documentos = Documentos;
                                if (Txt_Apellido_Materno.Text.Length > 0)
                                {
                                    Solicitud_Negocio.P_Apellido_Materno = Txt_Apellido_Materno.Text;
                                }
                                else
                                {
                                    Solicitud_Negocio.P_Apellido_Materno = "null";
                                }
                                Solicitud_Negocio.P_Apellido_Paterno = Txt_Apellido_Paterno.Text;
                                Solicitud_Negocio.P_Nombre_Solicitante = Txt_Nombre.Text;
                                String Clave_Unica = Cls_Util.Generar_Folio_Tramite();
                                Solicitud_Negocio.P_Clave_Solicitud = Clave_Unica;
                                Solicitud_Negocio.Alta_Solicitud(Cls_Sessiones.Nombre_Empleado);
                                Mostrar_Informacion(0);
                                Manejar_Botones(3);
                                Deshabilitar_Forma();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Trámites", "alert('Solicitud registrada con el Folio: " + Clave_Unica + "');", true);
                            }
                        
                    }//fin if lenght
                    else
                    {
                        Cadena_Informacion = "Los siguientes datos son requeridos por el sistema para realizar la operación, sea tan amable de verificar.";
                        Cadena_Informacion = Cadena_Informacion +
                            "<br/>&nbsp;&nbsp;&nbsp;+ Tramite que desea realizar." +
                            "<br/>&nbsp;&nbsp;&nbsp;+ Nombre de quién solicita." +
                            "<br/>&nbsp;&nbsp;&nbsp;+ Apellido paterno de quién solicita"+
                            "<br/>&nbsp;&nbsp;&nbsp;+ E-mail"+
                            "<br/>&nbsp;&nbsp;&nbsp;+ Archivos en formato PDF, JPG ó JPEG";
                        Lbl_Informacion.Text = Cadena_Informacion;
                        Mostrar_Informacion(1);

                    }//fin else lenght
            }//fin else email
           
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
            Grid_Datos.Visible = false;
            Grid_Documentos.Visible = false;
            Grid_Documentos_Modificar.Visible = false;
            Lbl_Datos_Requeridos.Visible = false;
            Lbl_Documentos_Requeridos.Visible = false;

            Habilitar_Forma();
            Cmb_Estatus.SelectedIndex = 1;
            Txt_Avance.Text = "0";
            
           Manejar_Botones(1);
        }//fin else Nuevo
    }
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        //Mostrar_Informacion(0);
        //String Cadena_Informacion = "";
        //Boolean Error = false;
        //if (Modificar)
        //{

        //    if (Cmb_Tramite.SelectedIndex != 0 && Txt_Nombre.Text.Length > 1 &&
        //        Txt_Apellido_Paterno.Text.Length > 1)
        //    {
        //        Solicitud_Negocio.P_Clave_Solicitud = Txt_Folio.Text;
        //        DataSet Ds_Solicitud = Solicitud_Negocio.Consultar_Solicitud();
        //        Solicitud_Negocio.P_Solicitud_ID = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString();
        //        Solicitud_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
        //        String[,] Datos = new String[Ds_Datos.Tables[0].Rows.Count, 2];
        //        String[,] Documentos = new String[Ds_Documentos.Tables[0].Rows.Count, 3];
        //        Cadena_Informacion = "Los siguientes datos son requeridos por el sistema para realizar la operación, sea tan amable de verificar.";
        //        //llenar matriz de datos
        //        for (int i = 0; i < Ds_Datos.Tables[0].Rows.Count; i++)
        //        {
        //            for (int j = 0; j < 2; j++)
        //            {
        //                if (j == 0)
        //                {

        //                    Datos[i, j] = Ds_Datos.Tables[0].Rows[i].ItemArray[0].ToString();

        //                }//fin if
        //                else
        //                {
        //                    String Valor_Dato = ((TextBox)Grid_Datos.Rows[i].FindControl("Txt_Descripcion_Datos")).Text;
        //                    if (Valor_Dato != "" ||
        //                        (Ds_Datos.Tables[0].Rows[i][Cat_Tra_Datos_Tramite.Campo_Dato_Requerido].ToString()) != "N")
        //                    {
        //                        Datos[i, j] = Valor_Dato;
        //                    }
        //                    else
        //                    {
        //                        Error = true;
        //                        Cadena_Informacion = Cadena_Informacion +
        //                            "<br/>&nbsp;&nbsp;&nbsp;+ " +
        //                            Ds_Datos.Tables[0].Rows[i][Cat_Tra_Datos_Tramite.Campo_Nombre].ToString();
        //                        Lbl_Informacion.Text = Cadena_Informacion;
        //                        Mostrar_Informacion(1);
        //                    }

        //                }//fin else
        //            }//fin for j
        //        }//fin for i 

                
        //        if (!Error)
        //        {
        //            Solicitud_Negocio.P_Datos = Datos;
        //            if (Txt_Apellido_Materno.Text.Length > 0)
        //            {
        //                Solicitud_Negocio.P_Apellido_Materno = Txt_Apellido_Materno.Text;
        //            }
        //            else
        //            {
        //                Solicitud_Negocio.P_Apellido_Materno = "null";
        //            }

        //            Solicitud_Negocio.P_Apellido_Paterno = Txt_Apellido_Paterno.Text;
        //            Solicitud_Negocio.P_Nombre_Solicitante = Txt_Nombre.Text;


        //            Solicitud_Negocio.Modificar_solicitud(Cls_Sessiones.Nombre_Empleado);


        //            Mostrar_Informacion(0);
        //            Manejar_Botones(3);
        //            Deshabilitar_Forma();
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Solicitud de Trámites", "alert('La solicitud ha sido actualizada');", true);
        //        }

        //    }//fin if lenght
        //    else
        //    {
        //        Cadena_Informacion = "Los siguientes datos son requeridos por el sistema para realizar la operación, sea tan amable de verificar.";
        //        Cadena_Informacion = Cadena_Informacion +
        //            "<br/>&nbsp;&nbsp;&nbsp;+ Tramite que desea realizar." +
        //            "<br/>&nbsp;&nbsp;&nbsp;+ Nombre de quién solicita." +
        //            "<br/>&nbsp;&nbsp;&nbsp;+ Apellido paterno de quién solicita";
        //        Lbl_Informacion.Text = Cadena_Informacion;
        //        Mostrar_Informacion(1);

        //    }//fin else lenght
        //}//fin if modificar
        //else
        //{
        //    if (Modificar)
        //    {
        //        Modificar = false;
        //        Btn_Modificar.ToolTip = "Modificar";
        //        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        //        Mostrar_Informacion(0);
        //    }//fin if
        //    else
        //    {
        //        if (Txt_Folio.Text.Length < 1)
        //        {
        //            Cadena_Informacion = "Para realizar la operacion es necesario buscar una solicitud, sea tan amable de verificar.";
        //            Lbl_Informacion.Text = Cadena_Informacion;
        //            Mostrar_Informacion(1);
        //            Manejar_Botones(3);
        //        }//fin if
        //        else
        //        {

        //            Lbl_Informacion.Text = "";
        //            Lbl_Informacion.Enabled = false;
        //            Img_Warning.Visible = false;
        //            Txt_Apellido_Materno.Enabled = true;
        //            Txt_Apellido_Paterno.Enabled = true;
        //            Txt_Nombre.Enabled = true;
        //            Cmb_Tramite.Enabled = false;
        //            Cmb_Estatus.Enabled = true;
        //            Cmb_Estatus.Enabled = true;
        //            Nuevo = true;
        //            Cancelar = true;
        //            Manejar_Botones(2);
        //        }//fin else folio lenght
        //    }
        //}//fin else modificar
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operacion actual qye se este realizando
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 16/OCtubre/2010 
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
        else
        {
            Manejar_Botones(3);
            Mostrar_Informacion(0);
            Deshabilitar_Forma();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Tramite_SelectedIndexChanged
    ///DESCRIPCIÓN: Se interactua con los gris para cargagar en ellos los
    ///datos y documentops requeridos para el tramite seleccionado en el combo
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 4/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Tramite_SelectedIndexChanged(object sender, EventArgs e)
    {
        Mostrar_Informacion(0);
        if (Cmb_Tramite.SelectedIndex != 0)
        {
            Refrescar_Grid_Datos();
            Refrescar_Grid_Documentos();
            Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
            Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
            Txt_Costo.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Costo].ToString();
            Txt_Tiempo_Estimado.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Tiempo_Estimado].ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Busca un Asunto por medio del Nombre en la base de datos 
    ///y pone el resultado de las coincidencias de la busqueda en el grid
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 23/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Mostrar_Informacion(0);
        Solicitud_Negocio.P_Clave_Solicitud = Txt_Busqueda.Text;
        DataSet Ds_Solicitud = Solicitud_Negocio.Consultar_Solicitud();
        if (Ds_Solicitud.Tables[0].Rows.Count > 0)
        {
            //colocar datos de la solicitud
            Txt_Folio.Text = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Clave_Solicitud].ToString();
            Txt_Nombre.Text = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Nombre_Solicitante].ToString();
            Txt_Apellido_Materno.Text = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Apellido_Materno].ToString();
            Txt_Apellido_Paterno.Text = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Apellido_Paterno].ToString();
            Cmb_Tramite.SelectedValue = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Tramite_ID].ToString();
            Cmb_Estatus.SelectedValue = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Estatus].ToString();
            Txt_Avance.Text = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Porcentaje_Avance].ToString();
            Txt_Email.Text = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Correo_Electronico].ToString();
            Solicitud_Negocio.P_Tramite_ID = Cmb_Tramite.SelectedValue;
            //colocar datos del tramite
            Ds_Tramites = Solicitud_Negocio.Consultar_Tramites();
            Txt_Costo.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Costo].ToString();
            Txt_Tiempo_Estimado.Text = Ds_Tramites.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Tiempo_Estimado].ToString();
            Solicitud_Negocio.P_Solicitud_ID = Ds_Solicitud.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString();
            Refrescar_Grid_Datos();
            Refrescar_Grid_Documentos_Modificar();
            DataSet Ds_Datos_Solicitud = Solicitud_Negocio.Consultar_Datos_Solicitud();
            DataSet Ds_Documentos_Solicitud = Solicitud_Negocio.Consultar_Documentos_Solicitud();
//            String Valor_Dato = "";
            for (int i = 0; i < Ds_Datos.Tables[0].Rows.Count; i++) 
            {
                ((TextBox)Grid_Datos.Rows[i].FindControl("Txt_Descripcion_Datos")).Text = Ds_Datos_Solicitud.Tables[0].Rows[i][Ope_Tra_Datos.Campo_Valor].ToString();
            }
            
        }
        else {
            Lbl_Informacion.Text = "No se encontro registro de una solicitud con el número de folio proporcionado, <br/>"+
                "sea tan amable de verificar";
            Mostrar_Informacion(1);
        }
    }


    #endregion



}
