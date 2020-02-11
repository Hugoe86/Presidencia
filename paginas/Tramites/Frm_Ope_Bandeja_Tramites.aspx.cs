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
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;
using Presidencia.Sessiones;
using Presidencia.Plantillas_Word;
using Presidencia.Registro_Peticion.Datos;
using System.IO;
using System.Net.Mail;
using Presidencia.Constantes;

public partial class paginas_Tramites_Ope_Bandeja_Tramites : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Error_MPE_Crear_Plantilla.Visible = false;
        //Lbl_Mensaje_Plantillas.Visible = false;
        //Lbl_Mensaje_Documentos_Seguimiento.Visible = false;
        //Lbl_Mensaje_Documentos_Anexos.Visible = false;
        //Lbl_Mensaje_Datos_Solicitud.Visible = false;
        try
        {
            if (!IsPostBack)
            {
                Grid_Bandeja_Entrada.Columns[1].Visible = false;
                Grid_Datos_Tramite.Columns[0].Visible = false;
                Grid_Datos_Tramite.Columns[1].Visible = false;
                Grid_Documentos_Tramite.Columns[0].Visible = false;
                Grid_Documentos_Tramite.Columns[1].Visible = false;
                Grid_Documentos_Tramite.Columns[3].Visible = false;
                Grid_Plantillas.Columns[1].Visible = false;
                Grid_Plantillas.Columns[3].Visible = false;
                Grid_Marcadores_Platilla.Columns[0].Visible = false;
                Grid_Documentos_Seguimiento.Columns[1].Visible = false;
                Consultar_Solicitudes_Tramites();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Metodos

    #region Grid Solicitudes Tramites

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Solicitudes_Tramites
    ///DESCRIPCIÓN: Hace una consulta a la Base de Datos para obtener los tramites
    ///             para el Empleado.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Consultar_Solicitudes_Tramites()
    {
        try
        {
            Limpiar_Formulario(); // Se limpia el Formulario
            Configuracion_Catalogo(false); //Se inhabilitan los componentes
            Session.Remove("GRID_BANDEJA_TRAMITES"); // Se elimina la variable de Session que contiene los datos del Grid
            Cls_Ope_Bandeja_Tramites_Negocio Tramites = new Cls_Ope_Bandeja_Tramites_Negocio();
            Tramites.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Tramites.P_Tipo_DataTable = "BANDEJA_TRAMITES";
            Tramites.P_Estatus = Cmb_Buscar_Solicitudes_Estatus.SelectedItem.Value;
            DataTable Tabla = Tramites.Consultar_DataTable(); // Se obtienen las Solicitudes de Tramite en proceso y pendientes.
            if (Tabla.Rows.Count > 0)
            {
                Session["GRID_BANDEJA_TRAMITES"] = Tabla;
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "No hay Solicitudes de Tramite en la Bandeja para el Estatus '" + Cmb_Buscar_Solicitudes_Estatus.SelectedItem.Text + "' ";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            Llenar_Grid_Solicitudes_Tramites(0);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Solicitudes_Tramites
    ///DESCRIPCIÓN: Llena el Grid de Solicitudes de Tramites.
    ///PARAMETROS:
    ///             1. Pagina.  Pagina de registros en que se mostrará el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Solicitudes_Tramites(Int32 Pagina)
    {
        try
        {
            Grid_Bandeja_Entrada.SelectedIndex = (-1);
            Grid_Bandeja_Entrada.Columns[1].Visible = true;
            if (Session["GRID_BANDEJA_TRAMITES"] != null)
            {
                Grid_Bandeja_Entrada.DataSource = (DataTable)Session["GRID_BANDEJA_TRAMITES"];
                Grid_Bandeja_Entrada.PageIndex = Pagina;
            }
            else
            {
                Grid_Bandeja_Entrada.DataSource = new DataTable();
            }
            Grid_Bandeja_Entrada.DataBind();
            Grid_Bandeja_Entrada.Columns[1].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Grid Datos Solicitud


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Datos_Solicitud
    ///DESCRIPCIÓN: Llena el Grid de Datos de la Solicitud.
    ///PARAMETROS:
    ///             1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Datos_Solicitud(DataTable Tabla)
    {
        try
        {
            Grid_Datos_Tramite.SelectedIndex = (-1);
            Grid_Datos_Tramite.Columns[0].Visible = true;
            Grid_Datos_Tramite.Columns[1].Visible = true;
            if (Tabla != null)
            {
                Grid_Datos_Tramite.DataSource = Tabla;
            }
            else
            {
                Grid_Datos_Tramite.DataSource = new DataTable();
            }
            Grid_Datos_Tramite.DataBind();
            Grid_Datos_Tramite.Columns[0].Visible = false;
            Grid_Datos_Tramite.Columns[1].Visible = false;

            //Se cargará el valor del Dato
            if (Grid_Datos_Tramite.Rows.Count == Tabla.Rows.Count)
            {
                for (Int32 Contador = 0; Contador < Grid_Datos_Tramite.Rows.Count; Contador++)
                {
                    TextBox Txt_Valor_Temporal = (TextBox)Grid_Datos_Tramite.Rows[Contador].Cells[3].Controls[1];
                    Txt_Valor_Temporal.Text = Tabla.Rows[Contador][3].ToString();
                }
            }
            if (Grid_Datos_Tramite.Rows.Count == 0)
            {
                //Lbl_Mensaje_Datos_Solicitud.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Grid Documentacion Solicitud

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Documentacion_Solicitud
    ///DESCRIPCIÓN: Llena el Grid de Documentacion de Solicitud.
    ///PARAMETROS:
    ///             1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Documentacion_Solicitud(DataTable Tabla)
    {
        try
        {
            Grid_Documentos_Tramite.SelectedIndex = (-1);
            Grid_Documentos_Tramite.Columns[0].Visible = true;
            Grid_Documentos_Tramite.Columns[1].Visible = true;
            Grid_Documentos_Tramite.Columns[3].Visible = true;
            if (Tabla != null)
            {
                Grid_Documentos_Tramite.DataSource = Tabla;
            }
            else
            {
                Grid_Documentos_Tramite.DataSource = new DataTable();
            }
            Grid_Documentos_Tramite.DataBind();
            Grid_Documentos_Tramite.Columns[0].Visible = false;
            Grid_Documentos_Tramite.Columns[1].Visible = false;
            Grid_Documentos_Tramite.Columns[3].Visible = false;
            if (Grid_Documentos_Tramite.Rows.Count == 0)
            {
                //Lbl_Mensaje_Documentos_Anexos.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Archivo
    ///DESCRIPCIÓN: Muestra un Archivo del cual se le pasa la ruta como parametro.
    ///PARAMETROS:
    ///             1.  Ruta.  Ruta del Archivo.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    public void Mostrar_Archivo(String Ruta)
    {
        try
        {
            if (System.IO.File.Exists(Ruta))
            {
                System.Diagnostics.Process proceso = new System.Diagnostics.Process();
                proceso.StartInfo.FileName = Ruta;
                proceso.Start();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('El archivo no existe');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion

    #region Grid Documentos Seguimiento

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Documentos_Seguimiento
    ///DESCRIPCIÓN: Llena el Grid de Documentos de Seguimiento del  Subproceso.
    ///PARAMETROS:
    ///             1.  Solicitud.  Objeto del cual se obtienen los datos para cargar
    ///                             el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Documentos_Seguimiento(Cls_Ope_Bandeja_Tramites_Negocio Solicitud)
    {
        try
        {
            if (Directory.Exists(MapPath("../../Archivos/" + Solicitud.P_Clave_Solicitud.Trim() + "/")))
            {
                DataTable Documentos_Seguimiento = null;
                String[] Archivos = Directory.GetFiles(MapPath("../../Archivos/" + Solicitud.P_Clave_Solicitud.Trim() + "/"));
                for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                {
                    Boolean Encontrado = false;
                    String Documento = Path.GetFileName(Archivos[Contador].Trim());
                    for (Int32 Contador2 = 0; Contador2 < Solicitud.P_Documentos_Solicitud.Rows.Count; Contador2++)
                    {
                        String Documento_Tramite = Path.GetFileName(Solicitud.P_Documentos_Solicitud.Rows[Contador2][3].ToString().Trim());
                        if (Documento.Equals(Documento_Tramite))
                        {
                            Encontrado = true;
                            break;
                        }
                    }
                    if (!Encontrado)
                    {
                        if (Documentos_Seguimiento == null)
                        {
                            Documentos_Seguimiento = new DataTable();
                            Documentos_Seguimiento.Columns.Add("NOMBRE_DOCUMENTO", Type.GetType("System.String"));
                            Documentos_Seguimiento.Columns.Add("URL", Type.GetType("System.String"));
                        }
                        DataRow Fila = Documentos_Seguimiento.NewRow();
                        Fila["NOMBRE_DOCUMENTO"] = Documento;
                        Fila["URL"] = Archivos[Contador].Trim();
                        Documentos_Seguimiento.Rows.Add(Fila);
                    }
                }
                if (Documentos_Seguimiento == null)
                {
                    Llenar_Grid_Documentos_Seguimiento(new DataTable());
                }
                else
                {
                    Llenar_Grid_Documentos_Seguimiento(Documentos_Seguimiento);
                }
            
            }// fin del if principal
            else
            {
                Llenar_Grid_Documentos_Seguimiento(new DataTable());
            }
        
        }// fin del try
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "')", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Documentos_Seguimiento
    ///DESCRIPCIÓN: Llena el Grid de Platillas del  Subproceso.
    ///PARAMETROS:
    ///             1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Documentos_Seguimiento(DataTable Tabla)
    {
        try
        {
            Grid_Documentos_Seguimiento.Columns[1].Visible = true;
            Grid_Documentos_Seguimiento.SelectedIndex = (-1);
            if (Tabla != null)
            {
                Grid_Documentos_Seguimiento.DataSource = Tabla;
            }
            else
            {
                Grid_Documentos_Seguimiento.DataSource = new DataTable();
            }
            Grid_Documentos_Seguimiento.DataBind();
            Grid_Documentos_Seguimiento.Columns[1].Visible = false;
            if (Grid_Documentos_Seguimiento.Rows.Count == 0)
            {
                //Lbl_Mensaje_Documentos_Seguimiento.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Grid Platillas


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Platillas_Subproceso
    ///DESCRIPCIÓN: Llena el Grid de Platillas del  Subproceso.
    ///PARAMETROS:
    ///             1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Platillas_Subproceso(DataTable Tabla)
    {
        try
        {
            Grid_Plantillas.Columns[1].Visible = true;
            Grid_Plantillas.Columns[3].Visible = true;
            Grid_Plantillas.SelectedIndex = (-1);
            if (Tabla != null)
            {
                Grid_Plantillas.DataSource = Tabla;
            }
            else
            {
                Grid_Plantillas.DataSource = new DataTable();
            }
            Grid_Plantillas.DataBind();
            Grid_Plantillas.Columns[1].Visible = false;
            Grid_Plantillas.Columns[3].Visible = false;
            if (Grid_Plantillas.Rows.Count == 0)
            {
                //Lbl_Mensaje_Plantillas.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }


    #endregion

    #region Grid Plantilla Marcadores

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Marcadores_Platillas
    ///DESCRIPCIÓN: Llena el Grid de Marcadores Platillas.
    ///PARAMETROS:
    ///             1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Grid_Marcadores_Platillas(DataTable Tabla)
    {
        try
        {
            Grid_Marcadores_Platilla.Columns[0].Visible = true;
            Grid_Marcadores_Platilla.SelectedIndex = (-1);
            if (Tabla != null)
            {
                Grid_Marcadores_Platilla.DataSource = Tabla;
            }
            else
            {
                Grid_Marcadores_Platilla.DataSource = new DataTable();
            }
            Grid_Marcadores_Platilla.DataBind();
            Grid_Marcadores_Platilla.Columns[0].Visible = false;
            Buscar_Marcadores_Fuente_Datos();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Marcadores_Planilla
    ///DESCRIPCIÓN: Carga los Marcadores de la plantilla en la Tabla.
    ///PARAMETROS:
    ///             1.  Nombre_Plantilla.   Nombre de la Platilla que se va a leer para
    ///                                     llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Marcadores_Planilla(String Nombre_Plantilla)
    {
        Cls_Interaccion_Word Plantilla = new Cls_Interaccion_Word();
        DataTable Tabla_Marcadores;
        try
        {
            Plantilla.P_Documento_Origen = MapPath("../../Plantillas_Word/" + Nombre_Plantilla);
            Plantilla.Iniciar_Aplicacion();
            Tabla_Marcadores = Plantilla.Obtener_Marcadores();
            Plantilla.Cerrar_Aplicacion();
            Llenar_Grid_Marcadores_Platillas(Tabla_Marcadores);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Buscar_Marcadores_Fuente_Datos
    ///DESCRIPCIÓN: Busca coincidencias entre los marcadores de la Tabla y los de la fuente
    ///             de datos.
    ///PARAMETROS:
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Buscar_Marcadores_Fuente_Datos()
    {
        try
        {
            if (Grid_Marcadores_Platilla.Rows.Count > 0 && Session["Dt_Fuente_Datos"] != null)
            {
                DataTable Fuente_Datos = (DataTable)Session["Dt_Fuente_Datos"];
                for (Int32 Contador = 0; Contador < Grid_Marcadores_Platilla.Rows.Count; Contador++)
                {
                    String Marcador = Grid_Marcadores_Platilla.Rows[Contador].Cells[0].Text.ToUpper();
                    for (Int32 Contador_DT = 0; Contador_DT < Fuente_Datos.Columns.Count; Contador_DT++)
                    {
                        String Elemento_Fuente = Fuente_Datos.Columns[Contador_DT].ColumnName.ToUpper();
                        if (Elemento_Fuente.Equals(Marcador))
                        {
                            TextBox Text_Temporal = (TextBox)Grid_Marcadores_Platilla.Rows[Contador].FindControl("Txt_Valor_Marcador");
                            if (Fuente_Datos.Columns[Contador_DT].DataType == Type.GetType("System.DateTime"))
                            {
                                Text_Temporal.Text = ((DateTime)Fuente_Datos.Rows[0][Contador_DT]).ToString("dd/MMM/yyyy");
                            }
                            else
                            {
                                Text_Temporal.Text = Fuente_Datos.Rows[0][Contador_DT].ToString();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Generales

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Se limpian los controles del Formulario.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Limpiar_Formulario()
    {
        try
        {
            Hdf_Solicitud_ID.Value = "";
            Hdf_Subproceso_ID.Value = "";
            Hdf_Plantilla_Seleccionada.Value = "";
            Txt_Clave_Solicitud.Text = "";
            Txt_Porcentaje_Avance.Text = "";
            Txt_Porcentaje_Actual_Proceso.Text = "";
            Txt_Nombre_Tramite.Text = "";
            Txt_Solicito.Text = "";
            Txt_Subproceso.Text = "";
            Txt_Estatus.Text = "";
            Txt_Fecha_Solicitud.Text = "";
            Txt_Comentarios_Evaluacion.Text = "";
            Cmb_Evaluacion.SelectedIndex = 0;
            /*  para los grid */
            //  datos
            Grid_Datos_Tramite.DataSource = new DataTable();
            Grid_Datos_Tramite.DataBind();
            //  documentos
            Grid_Documentos_Tramite.DataSource = new DataTable();
            Grid_Documentos_Tramite.DataBind();
            //  plantillas
            Grid_Plantillas.DataSource = new DataTable();
            Grid_Plantillas.DataBind();
            //  marcadores de plantillas
            Grid_Marcadores_Platilla.DataSource = new DataTable();
            Grid_Marcadores_Platilla.DataBind();
            MPE_Crear_Plantilla.TargetControlID = Btn_Comodin_FGC.ID;
            Session.Remove("Dt_Fuente_Datos");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Catalogo
    ///DESCRIPCIÓN: Maneja la habilitacion e inhabilitacion de los componentes.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Configuracion_Catalogo(Boolean Estatus)
    {
        try
        {
            Grid_Datos_Tramite.Enabled = Estatus;
            Grid_Documentos_Tramite.Enabled = Estatus;
            Grid_Plantillas.Enabled = Estatus;
            Cmb_Evaluacion.Enabled = Estatus;
            Btn_Guardar_Avance_Solucion.Enabled = Estatus;
            Btn_Dar_Solucion.Enabled = Estatus;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo
    ///DESCRIPCIÓN: Envia un correo al Usuario cuando el estatus de la solicitud cambio
    ///             a 'DETENIDO', 'CANDELADO' ó 'TERMINADO'.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Enviar_Correo(Cls_Ope_Bandeja_Tramites_Negocio Solicitud)
    {
        try
        {
            if (Solicitud.P_Correo_Electronico != null && Solicitud.P_Correo_Electronico.Trim().Length > 0)
            {
                Cls_Mail mail = new Cls_Mail();
                mail.P_Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Servidor_Correo].ToString();
                mail.P_Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Correo_Saliente].ToString();
                mail.P_Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Password_Correo].ToString();
                mail.P_Recibe = Solicitud.P_Correo_Electronico;
                mail.P_Subject = "SEGUIMIENTO A SOLICITUD DE TRAMITE: " + Txt_Nombre_Tramite.Text;
                String Mensaje = "Al Cuidadano " + Txt_Solicito.Text + "<br/><br/>Se le notifica que la Solicitud para el Tramite \"" + Txt_Nombre_Tramite.Text + "\"";
                if (Solicitud.P_Estatus.Equals("TERMINADO"))
                {
                    Mensaje = Mensaje + " ha <b>FINALIZADO</b> de manera exitosa." + "<br/><br/><b>NOTA:</b><br/>" + Solicitud.P_Comentarios + ".";
                }
                else if (Solicitud.P_Estatus.Equals("DETENIDO"))
                {
                    Mensaje = Mensaje + " ha sido <b>DETENIDA</b>." + "<br/><br/><b>CAUSA:</b><br/>" + Solicitud.P_Comentarios + ".";
                }
                else if (Solicitud.P_Estatus.Equals("CANCELADO"))
                {
                    Mensaje = Mensaje + " ha sido <b>CANCELADA</b>." + "<br/><br/><b>CAUSA:</b><br/>" + Solicitud.P_Comentarios + ".";
                }
                Mensaje = Mensaje + "<br/><br/>Por su Atenci&oacute;n </b>Gracias<br/>";
                Mensaje = Mensaje + "<hr width=\"98%\"><br/>PRESIDENCIA MUNICIPAL DE IRAPUATO, GTO";
                mail.P_Texto = HttpUtility.HtmlDecode(Mensaje);
                mail.Enviar_Correo();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo_Notificacion
    ///DESCRIPCIÓN: Envia un correo al Usuario cuando el estatus de la solicitud cambio
    ///             a 'DETENIDO', 'CANDELADO' ó 'TERMINADO'.
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  14/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private void Enviar_Correo_Notificacion(Cls_Ope_Bandeja_Tramites_Negocio Solicitud)
    {
        MailMessage Correo = new MailMessage();
        String Para = "";
        String De = "";
        String Puerto = "";
        String Servidor = "";
        String Contraseña = "";
        String Mensaje = "";

        try
        {
            if (Solicitud.P_Correo_Electronico != null && Solicitud.P_Correo_Electronico.Trim().Length > 0)
            {
                Para = Solicitud.P_Correo_Electronico;
                
                De = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Correo_Saliente].ToString();
                Contraseña = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Password_Correo].ToString();
                Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Servidor_Correo].ToString();
                Puerto = "25";
                
                Correo.To.Add(Para);
                Correo.From = new MailAddress(De, "SEGUIMIENTO A SOLICITUD DE TRAMITE");
                Correo.Subject = Txt_Nombre_Tramite.Text.ToUpper().Trim();
                Correo.SubjectEncoding = System.Text.Encoding.UTF8;

                //  para el texto del mensaje
                Mensaje = "<html>";
                Mensaje += "<body> ";

                Mensaje += "<table width=\"100%\" > ";
                Mensaje += "<tr>";
                Mensaje += "<td style=\"width:100%;font-size:18px;\" align=\"center\" >";
                Mensaje += "SEGUIMIENTO A SOLICITUD DE TRAMITE ";
                Mensaje += "</td>";
                Mensaje += "</hr>";
                Mensaje += "</table>";

                Mensaje += "<br>";

                Mensaje += "<table width=\"100%\" > ";
                Mensaje += "<tr>";
                Mensaje += "<td style=\"width:100%;font-size:14px;\" align=\"right\" >";
                Mensaje += "Irapuato, Guanajuato a " + String.Format("{0:dd/MMM/yyyy}", DateTime.Today) + ".";
                Mensaje += "</td>";
                Mensaje += "</hr>";
                Mensaje += "</table>";

                Mensaje += "<br> <p align=justify style=\"font-size:12px\"> ";
                Mensaje += "Por medio de la presente se hace de su cocimiento que al Cuidadano <b>" + Txt_Solicito.Text + "</b> ";
                Mensaje += "se le notifica que la Solicitud de Tramite <b>" + Txt_Nombre_Tramite.Text + "</b> ";
                Mensaje += "con Folio <b>" + Txt_Clave_Solicitud.Text.Trim() + "<b>.";

                Mensaje += "<hr width=\"98%\">";
                //  estatus terminado
                if (Solicitud.P_Estatus.Equals("TERMINADO"))
                {
                    Mensaje += "<br> Ha  <b>FINALIZADO</b> de manera exitosa.";

                    if (!String.IsNullOrEmpty(Solicitud.P_Comentarios))
                        Mensaje += "<br><br> <b>NOTA:</b> " + Solicitud.P_Comentarios + ".";
                }

                //  estatus detenido
                else if (Solicitud.P_Estatus.Equals("DETENIDO"))
                {
                    Mensaje += "<br>  Ha sido <b>DETENIDA</b>.";
                    Mensaje += "<br><b>POR LA CAUSA:</b> " + Solicitud.P_Comentarios;
                    Mensaje += "."; //  punto final de la causa
                }

                //  estatus cancelado
                else if (Solicitud.P_Estatus.Equals("CANCELADO"))
                {
                    Mensaje += "<br>  Ha sido <b>CANCELADA</b>.";
                    Mensaje += "<br><b>POR LA CAUSA:</b> " + Solicitud.P_Comentarios;
                    Mensaje += "."; //  punto final de la causa
                }

                Mensaje += "</p>";
                Mensaje += "<hr width=\"98%\">";

                Mensaje += "<br>" +
                            "<p align =left> <b> Por su Atenci&oacute;n  Gracias </b>.</p> ";
                Mensaje += "<br> <p align =center> Atentamente: </b> </p> ";
                Mensaje += "<p align =center> <b> PRESIDENCIA MUNICIPAL DE IRAPUATO, GTO </b> </p>";
                Mensaje += "</body>";
                Mensaje += "</html>";

                Mensaje = HttpUtility.HtmlDecode(Mensaje);

                if ((!Correo.From.Equals("") || Correo.From != null) && (!Correo.To.Equals("") || Correo.To != null))
                {
                    Correo.Body = Mensaje;

                    Correo.BodyEncoding = System.Text.Encoding.UTF8;
                    Correo.IsBodyHtml = true;

                    SmtpClient cliente_correo = new SmtpClient();
                    cliente_correo.Port = int.Parse(Puerto);
                    cliente_correo.UseDefaultCredentials = true;
                    //cliente_correo.Credentials = new System.Net.NetworkCredential(De, Contraseña);
                    cliente_correo.Credentials = new System.Net.NetworkCredential(De, Contraseña);
                    cliente_correo.Host = Servidor;
                    cliente_correo.Send(Correo);
                    Correo = null;
                }
                else
                {
                    throw new Exception("No se tiene configurada una cuenta de correo, favor de notificar");
                }
            }// fin del if principal

        }// fin del try
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    #endregion

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Evaluacion
    ///DESCRIPCIÓN: Valida que antes de Evaluar la Solicitud, se contengan todos los
    ///             datos necesarios.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private Boolean Validar_Evaluacion()
    {
        Boolean Completo = true;
        String Mensaje_Error = "";
        Lbl_Ecabezado_Mensaje.Text = "Es necesario...";
        if (Cmb_Evaluacion.SelectedItem.Value.Equals("APROBAR"))
        {
            if (!Validar_Plantillas_Llenas())
            {
                Mensaje_Error = Mensaje_Error + "+ Llenar Plantillas";
                Completo = false;
            }
        }
        if (Txt_Comentarios_Evaluacion.Text.Trim().Length == 0)
        {
            if (!Completo) { Mensaje_Error = Mensaje_Error + "<br/>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir Comentarios.";
            Completo = false;
        }
        if (!Completo)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Completo;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Plantillas_Llenas
    ///DESCRIPCIÓN: Valida que las plantillas hayan sido terminadas.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private Boolean Validar_Plantillas_Llenas()
    {
        Boolean Plantillas_Completas = true;
        try
        {
            for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
            {
                CheckBox Check_Temporal = (CheckBox)Grid_Plantillas.Rows[Contador].FindControl("Chk_Realizado");
                if (!Check_Temporal.Checked)
                {
                    Plantillas_Completas = false;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        return Plantillas_Completas;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Plantilla
    ///DESCRIPCIÓN: Valida que todos los campos de la platilla fueron llenados.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    private Boolean Validar_Plantilla()
    {
        Boolean Plantilla_Completa = true;
        try
        {
            for (Int32 Contador = 0; Contador < Grid_Marcadores_Platilla.Rows.Count; Contador++)
            {
                TextBox Text_Temporal = (TextBox)Grid_Marcadores_Platilla.Rows[Contador].FindControl("Txt_Valor_Marcador");
                if (Text_Temporal.Text.Trim().Length == 0)
                {
                    Plantilla_Completa = false;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        return Plantilla_Completa;
    }
    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Bandeja_Entrada_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del Grid de Bandeja de Entrada.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Bandeja_Entrada_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Llenar_Grid_Solicitudes_Tramites(e.NewPageIndex);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Bandeja_Entrada_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el Evento del Cambio de Selección.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 15/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Bandeja_Entrada_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Elemento_ID = ""; // Se obtiene el ID de la Solicitud Seleccionada.
        Cls_Ope_Bandeja_Tramites_Negocio Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        DataTable Dt_Valor_Subproceso = new DataTable();
        DataTable Temporal = new DataTable();
        Double Porcentaje = 0.0;
        try
        {
            Limpiar_Formulario(); // Se limpia el Catalogo
            if (Grid_Bandeja_Entrada.SelectedIndex > (-1))
            {
                //  se cargan los id
                Elemento_ID = Grid_Bandeja_Entrada.SelectedRow.Cells[1].Text; // Se obtiene el ID de la Solicitud Seleccionada.
                Solicitud.P_Solicitud_ID = Elemento_ID;
                Solicitud = Solicitud.Consultar_Datos_Solicitud(); // Se obtienen los Datos a Detalle de la Solicitud Seleccionada
                Hdf_Solicitud_ID.Value = Solicitud.P_Solicitud_ID;

                Txt_Clave_Solicitud.Text = Solicitud.P_Clave_Solicitud;
                Txt_Porcentaje_Avance.Text = Solicitud.P_Porcentaje_Avance.ToString("#,###,###.00");
                Txt_Nombre_Tramite.Text = Solicitud.P_Tramite;
                Txt_Solicito.Text = Solicitud.P_Solicito;
                Hdf_Subproceso_ID.Value = Solicitud.P_Subproceso_ID;

                //  se consulta el valor % del proceso que se esta realizando
                Dt_Valor_Subproceso = Solicitud.Consultar_Valor_Subproceso_ID();
                if (Dt_Valor_Subproceso is DataTable)
                {
                    if (Dt_Valor_Subproceso.Rows.Count > 0)
                    {
                        Porcentaje = Convert.ToDouble(Dt_Valor_Subproceso.Rows[0][Cat_Tra_Subprocesos.Campo_Valor].ToString());
                        Txt_Porcentaje_Actual_Proceso.Text = Porcentaje.ToString("#,###,###.00");
                    }
                }

                Txt_Subproceso.Text = Solicitud.P_Subproceso_Nombre;
                Txt_Estatus.Text = Solicitud.P_Estatus;
                Txt_Fecha_Solicitud.Text = Solicitud.P_Fecha_Solicitud.ToString("dd/MMM/yyyy");

                //  se llenan los grids
                Llenar_Grid_Datos_Solicitud(Solicitud.P_Datos_Solicitud);
                Llenar_Grid_Documentacion_Solicitud(Solicitud.P_Documentos_Solicitud);
                Llenar_Grid_Platillas_Subproceso(Solicitud.P_Plantillas_Subproceso);

                Cargar_Documentos_Seguimiento(Solicitud);

                //  se consulta la informacion de las pantillas
                Solicitud.P_Tipo_DataTable = "FUENTE_DATOS_PLANTILLAS";
                Temporal = Solicitud.Consultar_DataTable();
                if (Temporal != null && Temporal.Rows.Count > 0)
                {
                    Session["Dt_Fuente_Datos"] = Temporal;
                }
                Configuracion_Catalogo(true);

            }
            else
            {
                Configuracion_Catalogo(false);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Plantillas_RowDataBound
    ///DESCRIPCIÓN: Evento de RowDataBound del Grid de Plantillas.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Plantillas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton Boton = (ImageButton)e.Row.Cells[4].FindControl("Btn_Generar_Documento");
                Boton.CommandArgument = e.Row.Cells[1].Text;
                AsyncPostBackTrigger Disparador = new AsyncPostBackTrigger();
                Disparador.ControlID = Boton.ID;
                Disparador.EventName = "Click";
                UpPnl_Plantilla.Triggers.Add(Disparador);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_Tramite_RowDataBound
    ///DESCRIPCIÓN: Evento de RowDataBound del Grid de Documentos Anexados.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Documentos_Tramite_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton Boton = (ImageButton)e.Row.Cells[4].FindControl("Btn_Ver_Documento");
                Boton.CommandArgument = e.Row.Cells[0].Text;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_Seguimiento_RowDataBound
    ///DESCRIPCIÓN: Evento de RowDataBound del Grid de Documentos creados durante el 
    ///             seguimiento.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Documentos_Seguimiento_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton Boton = (ImageButton)e.Row.Cells[2].FindControl("Btn_Ver_Documento_Seguimiento");
                Boton.CommandArgument = e.Row.Cells[0].Text;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }


    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Solicitudes_Estatus
    ///DESCRIPCIÓN: Maneja la Busqueda de las Solicitudes por Estatus.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 16/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Buscar_Solicitudes_Estatus_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Consultar_Solicitudes_Tramites();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Documento_Click
    ///DESCRIPCIÓN: Maneja el Evento Click del Boton dentro de la tabla usado para ver 
    ///             la plantilla.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Generar_Documento_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Cmb_Evaluacion.SelectedItem.Value.Equals("APROBAR"))
            {
                if (sender != null)
                {
                    ImageButton Boton = (ImageButton)sender;
                    String Plantilla_Seleccionada = Boton.CommandArgument;
                    Hdf_Plantilla_Seleccionada.Value = Plantilla_Seleccionada;
                    String Archivo = "";
                    for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                    {
                        if (Plantilla_Seleccionada.Equals(Grid_Plantillas.Rows[Contador].Cells[1].Text.Trim()))
                        {
                            Archivo = Grid_Plantillas.Rows[Contador].Cells[3].Text.Trim();
                            break;
                        }
                    }
                    Cargar_Marcadores_Planilla(Archivo);
                    Pnl_Crear_Plantilla.Style.Remove("display");
                    Pnl_Crear_Plantilla.Style.Add("display", "inline");
                    UpPnl_Plantilla.Update();
                    MPE_Crear_Plantilla.Show();
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Para el estatus de '" + Cmb_Evaluacion.SelectedItem.Value + "':";
                Lbl_Mensaje_Error.Text = " + No es necesario llenar las Plantillas";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Evaluar_Click
    ///DESCRIPCIÓN: Se maneja el evento del boton de evaluacion y envia los datos para 
    ///              ser actualizados.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Evaluar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Validar_Evaluacion())
            {
                Cls_Ope_Bandeja_Tramites_Negocio Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                Solicitud.P_Estatus = Cmb_Evaluacion.SelectedItem.Value;
                if (!Solicitud.P_Estatus.Equals("APROBAR"))
                {
                    Solicitud.P_Comentarios = Txt_Comentarios_Evaluacion.Text.Trim();
                }
                Solicitud.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
                Solicitud.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Solicitud = Solicitud.Evaluar_Solicitud();
                if (Solicitud.P_Enviar_Correo_Electronico != null && Solicitud.P_Enviar_Correo_Electronico)
                {
                    //Enviar_Correo(Solicitud);
                    Enviar_Correo_Notificacion(Solicitud);
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Bandeja", "alert('Evaluacion Realizada Exitosamente');", true);
                Consultar_Solicitudes_Tramites();
                Btn_Cancela.Visible = false;
                Btn_Guardar_Avance_Solucion.Visible = false;
                Div_Dar_Solucion.Style.Value = "display:none";
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Crear_Documento_Click
    ///DESCRIPCIÓN: Se maneja el evento del boton de crear documento de una plantilla.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Crear_Documento_Click(object sender, EventArgs e)
    {
        try
        {
            if (Validar_Plantilla())
            {
                String Plantilla_Seleccionada = Hdf_Plantilla_Seleccionada.Value;
                String Archivo = "";
                for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                {
                    if (Plantilla_Seleccionada.Equals(Grid_Plantillas.Rows[Contador].Cells[1].Text.Trim()))
                    {
                        Archivo = Grid_Plantillas.Rows[Contador].Cells[3].Text.Trim();
                        break;
                    }
                }
                Cls_Interaccion_Word Word = new Cls_Interaccion_Word();
                Word.P_Documento_Origen = MapPath("../../Plantillas_Word/" + Archivo);
                Word.Iniciar_Aplicacion();
                for (Int32 Contador = 0; Contador < Grid_Marcadores_Platilla.Rows.Count; Contador++)
                {
                    TextBox Text_Temporal = (TextBox)Grid_Marcadores_Platilla.Rows[Contador].FindControl("Txt_Valor_Marcador");
                    String Marcador_ID = Grid_Marcadores_Platilla.Rows[Contador].Cells[0].Text;
                    Word.Escribir_Sobre_Marcador(Marcador_ID, Text_Temporal.Text.Trim());
                }
                Word.P_Documento_Destino = Server.MapPath("../../Archivos/" + Txt_Clave_Solicitud.Text.Trim() + "/SUB_" + Hdf_Subproceso_ID.Value.Trim() + " - " + Archivo);
                Word.Guardar_Nuevo_Documento();
                Word.Cerrar_Aplicacion();
                for (Int32 Contador = 0; Contador < Grid_Plantillas.Rows.Count; Contador++)
                {
                    if (Grid_Plantillas.Rows[Contador].Cells[1].Text.Equals(Hdf_Plantilla_Seleccionada.Value))
                    {
                        CheckBox Check_Temporal = (CheckBox)Grid_Plantillas.Rows[Contador].FindControl("Chk_Realizado");
                        Check_Temporal.Checked = true;
                        break;
                    }
                }
                Grid_Marcadores_Platilla.DataSource = new DataTable();
                Grid_Marcadores_Platilla.DataBind();
                UpPnl_Plantilla.Triggers.Clear();
                Hdf_Plantilla_Seleccionada.Value = "";
                MPE_Crear_Plantilla.Hide();
            }
            else
            {
                Lbl_Error_MPE_Crear_Plantilla.Text = "Faltan Datos de Llenar para este documento!!";
                Lbl_Error_MPE_Crear_Plantilla.Visible = true;
                MPE_Crear_Plantilla.Show();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Documento_Click
    ///DESCRIPCIÓN: Se maneja el evento del boton de crear documento de una plantilla.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Ver_Documento_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (sender != null)
            {
                ImageButton Boton = (ImageButton)sender;
                String Documento = Boton.CommandArgument;
                String URL = null;
                for (Int32 Contador = 0; Contador < Grid_Documentos_Tramite.Rows.Count; Contador++)
                {
                    if (Grid_Documentos_Tramite.Rows[Contador].Cells[0].Text.Equals(Documento))
                    {
                        //URL = Server.MapPath("../../Archivos/" + Txt_Clave_Solicitud.Text + "/" + Path.GetFileName(Grid_Documentos_Tramite.Rows[Contador].Cells[3].Text));
                        URL = Server.MapPath("../../Archivos/" + "TR-" + Hdf_Solicitud_ID.Value + "/" + Path.GetFileName(Grid_Documentos_Tramite.Rows[Contador].Cells[3].Text));
                        break;
                    }
                }
                if (URL != null)
                {
                    Mostrar_Archivo(URL);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Documento_Seguimiento_Click
    ///DESCRIPCIÓN: Se maneja el evento para ver el documento creado dentro del seguimiento.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Ver_Documento_Seguimiento_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (sender != null)
            {
                ImageButton Boton = (ImageButton)sender;
                String Documento = Boton.CommandArgument;
                String URL = null;
                for (Int32 Contador = 0; Contador < Grid_Documentos_Seguimiento.Rows.Count; Contador++)
                {
                    if (Grid_Documentos_Seguimiento.Rows[Contador].Cells[0].Text.Equals(Documento))
                    {
                        URL = Server.MapPath("../../Archivos/" + Txt_Clave_Solicitud.Text.Trim() + "/" + Path.GetFileName(Grid_Documentos_Seguimiento.Rows[Contador].Cells[1].Text));
                        break;
                    }
                } if (URL != null)
                {
                    Mostrar_Archivo(URL);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Dar_Solucion_Click
    ///DESCRIPCIÓN: Se maneja el evento del boton para dar solución
    ///PARAMETROS:     
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 5/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Dar_Solucion_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Dar_Solucion.Style.Value = "display:block";
            Btn_Guardar_Avance_Solucion.Visible = true;
            Btn_Cancela.Visible = true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancela_Click
    ///DESCRIPCIÓN: Se maneja el evento del boton para cancelar
    ///PARAMETROS:     
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 5/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Cancela_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Dar_Solucion.Style.Value = "display:none";
            Btn_Guardar_Avance_Solucion.Visible = false;
            Btn_Cancela.Visible = false;
            Limpiar_Formulario();
            if (Grid_Bandeja_Entrada.Rows.Count > 0)
                Grid_Bandeja_Entrada.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Se maneja el evento del boton para salir
    ///PARAMETROS:     
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 5/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");

    }

    #endregion

}