using System;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
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
using Presidencia.Solicitud_Tramites.Negocios;
using Presidencia.Operacion_Predial_Pagos_Instit_Externas.Negocio;
using System.Drawing;
using System.Drawing.Drawing2D;
using Presidencia.Constantes;
using AjaxControlToolkit;
using Presidencia.Ordenamiento_Territorial_Inspectores.Negocio;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Reportes;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Presidencia.Ventanilla_Consultar_Tramites.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Catalogo_Tramites.Negocio;
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Catalogo_Cat_Peritos_Externos.Negocio;

public partial class paginas_Ventanilla_Frm_Rpt_Ven_Consultar_Tramites : System.Web.UI.Page
{
    #region Page load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones  
                
                this.Form.Enctype = "multipart/form-data";

            }
            String Ventana_Modal = "Abrir_Ventana_Modal('../Atencion_Ciudadana/Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Dependencia.Attributes.Add("onclick", Ventana_Modal);
        }

    #endregion

    #region Metodos generales

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Inicializa_Controles
        /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
        ///               realizar diferentes operaciones
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 03/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Inicializa_Controles()
        {
            try
            {
                Limpiar_Controles();
                Habilitar_Controles("Inicial");
                Cargar_Grid(true);
                Cargar_Combo_Unidad_Responsable();
                LLenar_Combos();
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
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 07/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Limpiar_Controles()
        {
            try
            {
                //  para los datos generales de la solicitud
                Txt_Detalle_Clave.Text = "";
                Txt_Detalle_Nombre.Text = "";
                Txt_Detalle_Estatus.Text = "";
                Txt_Detalle_Porcentaje.Text = "";
                //Txt_Detalle_Costo.Text = "";
                Hdf_Costo.Value = "";
                Txt_Detalle_Email.Text = "";
                Txt_Detalle_Nombre_Solicitante.Text = "";
                Txt_Detalle_Actividad.Text = "";
                Hdf_Solicitud_ID.Value = "";
                Txt_Fecha_Entrega.Text = "";
                Txt_Nombre_Tramite.Text = "";
                Hdf_Propietario_Cuenta_Predial.Value = "";
                Hdf_Reporte_Orden_Pago.Value = "";
                Txt_Direccion_Predio.Text = "";
                Txt_Propietario_Cuenta_Predial.Text = "";
                Txt_Calle_Predio.Text = "";
                Txt_Numero_Predio.Text = "";
                Txt_Manzana_Predio.Text = "";
                Txt_Lote_Predio.Text = "";
                Txt_Otros_Predio.Text = "";
                Txt_Cuenta_Predial.Text = "";

                //  para las fechas
                Txt_Detalle_Fecha_Inicio.Text = "";
                //Txt_Detalle_Fecha_Termino.Text = "";
                Txt_Detalle_Tiempo_Estimado.Text = "";


                Cmb_Estatus.SelectedIndex = 0;
                Txt_Fecha_Inicio.Text = "";
                Txt_Fecha_Fin.Text = ""; //String.Format("{0:dd/MMM/yyyy}", DateTime.Today);

                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Limpia_Controles " + ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        /// NOMBRE:         Habilitar_Controles
        /// DESCRIPCION :   Habilita y Deshabilita los controles de la forma para prepara la página
        ///                 para a siguiente operación
        /// PARAMETROS:     1.- Operacion: Indica la operación que se desea realizar 
        /// CREO:           Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO:     07/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Habilitar_Controles(String Operacion)
        {
            Boolean Habilitado = false; ///Indica si el control de la forma va hacer habilitado para utilización del usuario
            try
            {
                Habilitado = false;
                switch (Operacion)
                {
                    case "Inicial":
                        Habilitado = false;
                        Btn_Salir.ToolTip = "Salir";
                        Btn_Modificar.ToolTip = "Modificar"; 
                        Btn_Modificar.Visible = false;
                        Btn_Modificar.CausesValidation = false;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Div_Detalles_Solicitud.Style.Value = "display:none";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        break;

                    case "Modificar":
                        Habilitado = true;
                        Btn_Modificar.ToolTip = "Actualizar";
                        Btn_Salir.ToolTip = "Cancelar";
                        Btn_Modificar.Visible = true;
                        Btn_Modificar.CausesValidation = true;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        break;
                }

                //  mensajes de error
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;

                //  para los datos generales de la solicitud
                Txt_Detalle_Clave.Enabled = false;
                Txt_Detalle_Estatus.Enabled = false;
                Txt_Detalle_Porcentaje.Enabled = false;
                Txt_Detalle_Costo.Enabled = false;
                Txt_Detalle_Email.Enabled = false;
                Txt_Detalle_Nombre_Solicitante.Enabled = false;
                Txt_Detalle_Actividad.Enabled = false;
                Txt_Detalle_Nombre.Enabled = false;

                //  para las fechas
                Txt_Detalle_Fecha_Inicio.Enabled = false;
                Txt_Detalle_Tiempo_Estimado.Enabled = false;
                Txt_Detalle_Cantidad.Enabled = Habilitado;
              

                Txt_Fecha_Inicio.Enabled = false;
                Txt_Fecha_Fin.Enabled = false;
                Txt_Fecha_Entrega.Enabled = false;

                Txt_Cuenta_Predial.Enabled = Habilitado;
                Txt_Direccion_Predio.Enabled = Habilitado;
                Txt_Propietario_Cuenta_Predial.Enabled = Habilitado;
                Txt_Calle_Predio.Enabled = Habilitado;
                Txt_Numero_Predio.Enabled = Habilitado;
                Txt_Manzana_Predio.Enabled = Habilitado;
                Txt_Lote_Predio.Enabled = Habilitado;
                Txt_Otros_Predio.Enabled = Habilitado;
                Cmb_Perito.Enabled = Habilitado;
                Grid_Datos.Enabled = Habilitado;
                Btn_Copiar.Visible = false;
            }

            catch (Exception ex)
            {
                throw new Exception("Limpia_Controles " + ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Cargar_Grid
        /// DESCRIPCION : Limpia los controles que se encuentran en la forma
        /// PARAMETROS  : Dt_Consulta la tabla con la infromacion a cargar en el grid 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 03/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Cargar_Grid(Boolean Carga_Inicial)
        {
            DataTable Dt_Consulta = new DataTable();
            DataTable Dt_Consulta_Cancelaciones = new DataTable();
            Cls_Rpt_Ven_Consultar_Tramites_Negocio Rs_Consulta = new Cls_Rpt_Ven_Consultar_Tramites_Negocio();
            Boolean Estado_Fechas = true;
            Boolean Mensaje_Error = false;
            try
            {
                //  para buscar los tramites que tenga el usuario
                if (!String.IsNullOrEmpty(Cls_Sessiones.Ciudadano_ID))
                {
                    Lbl_Clave_Solicitud.Visible = false;
                    Txt_Clave_Solicitud.Visible = false;
                    Btn_Buscar.Visible = true;

                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    Div_Filtros.Style.Value = "display:block";

                    Rs_Consulta.P_Email = Cls_Sessiones.Datos_Ciudadano.Rows[0][Cat_Ven_Usuarios.Campo_Email].ToString();
                    
                    //  Filtro para la unidad responsable
                    if (Cmb_Unidad_Responsable_Filtro.SelectedIndex > 0)
                    {
                        Rs_Consulta.P_Dependencia_ID = Cmb_Unidad_Responsable_Filtro.SelectedValue;
                        Rs_Consulta.P_Filtro = "SI";
                    }
                    
                    //  para el filtro del nombre del tramite
                    if (Txt_Nombre_Tramite.Text != "")
                    {
                        Rs_Consulta.P_Nombre_Tramite = Txt_Nombre_Tramite.Text;
                        Rs_Consulta.P_Filtro = "SI";
                    }
                    //  Filtro para la fecha inicio
                    if (Txt_Fecha_Inicio.Text != "" && Txt_Fecha_Fin.Text != "")
                    {
                        Estado_Fechas = Validar_Fechas(Txt_Fecha_Inicio.Text, Txt_Fecha_Fin.Text);
                        if (Estado_Fechas == true)
                        {
                            if (Txt_Fecha_Inicio.Text != "")
                            {
                                Rs_Consulta.P_Dtime_Fecha_Inicio = Convert.ToDateTime(Txt_Fecha_Inicio.Text);
                                Rs_Consulta.P_Filtro = "SI";
                            }

                            //  Filtro para la fecha fin
                            if (Txt_Fecha_Fin.Text != "")
                            {
                                Rs_Consulta.P_Dtime_Fecha_Fin = Convert.ToDateTime(Txt_Fecha_Fin.Text);
                                Rs_Consulta.P_Filtro = "SI";
                            }
                        }
                        else
                        {
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "La fecha de inicio debe ser menor a la final";
                        }
                    }
                    else if (Txt_Fecha_Inicio.Text == "" && Txt_Fecha_Fin.Text == "")
                    {
                        
                    }
                    else
                    {
                        Img_Error.Visible = true; 
                        Lbl_Mensaje_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "revisar la fecha inicio y la fecha fin";
                    }
                    //  Filtro para el estatus
                    if (Cmb_Estatus.SelectedIndex > 0)
                    {
                        Rs_Consulta.P_Estatus = Cmb_Estatus.SelectedValue;
                        Rs_Consulta.P_Filtro = "SI";
                    }

                    //  filtro para la carga inicial del formulario
                    //  pendientes y proceso
                    if (Carga_Inicial == true)
                    {
                        Rs_Consulta.P_Solicitudes_Pendiente_Proceso = true;
                    }

                    if (Estado_Fechas == true)
                    {
                        Dt_Consulta = Rs_Consulta.Consultar_Tramites();
                        //Dt_Consulta_Cancelaciones = Rs_Consulta.Consultar_Tramites();

                        //  para llenar el grid de los tramites en proceso
                        if (Dt_Consulta is DataTable && Dt_Consulta != null)
                        {
                            if (Dt_Consulta.Rows.Count > 0)
                                Cargar_Grid_DataTable(Dt_Consulta);

                            else
                                Cargar_Grid_DataTable(new DataTable());
                        }
                    }
                    else
                    {
                    }
                }//FIN DEL IF Cls_Sessiones.Ciudadano_ID
                else
                {
                    Div_Grid.Style.Value = "display: none";
                    Div_Busqueda_Sin_Usuario_Registrado.Style.Value = "display: block";
                    Lbl_Clave_Solicitud.Visible = true;
                    Txt_Clave_Solicitud.Visible = true;
                    Div_Filtros.Style.Value = "display:none";
                    Btn_Buscar.Visible = true;

                    if (!String.IsNullOrEmpty(Txt_Clave_Solicitud.Text))
                    {
                        Rs_Consulta.P_Clave_Solicitud = Txt_Clave_Solicitud.Text.Trim();
                        Dt_Consulta = Rs_Consulta.Consultar_Tramites();

                        if (Dt_Consulta is DataTable)
                        {
                            if (Dt_Consulta.Rows.Count > 0)
                                Cargar_Grid_DataTable(Dt_Consulta);

                            else
                            {
                                Cargar_Grid_DataTable(Dt_Consulta);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Mensaje", "alert('No se encuentra solicitud con esa clave');", true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cargar_Grid " + ex.Message.ToString());
            }

        }
        
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: LLenar_Combos
        ///DESCRIPCIÓN: Consulta los datos para los combos y los asigna al combo correspondiente
        ///PARÁMETROS:
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 10-jun-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public void LLenar_Combos()
        {
            var Obj_Peritos = new Cls_Cat_Ort_Inspectores_Negocio();
            DataTable Dt_Peritos;

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            try
            {
                // consultar peritos
                Dt_Peritos = Obj_Peritos.Consultar_Inspectores();
                // cargar datos en el combo
                Cmb_Perito.Items.Clear();
                Cmb_Perito.DataSource = Dt_Peritos;
                Cmb_Perito.DataTextField = Cat_Ort_Inspectores.Campo_Nombre;
                Cmb_Perito.DataValueField = Cat_Ort_Inspectores.Campo_Inspector_ID;
                Cmb_Perito.DataBind();
                Cmb_Perito.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), ""));
                Cmb_Perito.SelectedIndex = 0;
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message;
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: LLenar_Combos_Catastro
        ///DESCRIPCIÓN: Consulta los datos para los combos y los asigna al combo correspondiente
        ///PARÁMETROS:
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 10-jun-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public void LLenar_Combos_Catastro()
        {
            var Obj_Peritos = new Cls_Cat_Cat_Peritos_Externos_Negocio();
            DataTable Dt_Peritos;

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            try
            {
                // consultar peritos
                Obj_Peritos.P_Estatus = "='VIGENTE";
                Dt_Peritos = Obj_Peritos.Consultar_Peritos_Externos();
                // cargar datos en el combo
                Cmb_Perito.Items.Clear();
                Cmb_Perito.DataSource = Dt_Peritos;
                Cmb_Perito.DataTextField = "PERITO_EXTERNO";
                Cmb_Perito.DataValueField = Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id;
                Cmb_Perito.DataBind();
                Cmb_Perito.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), ""));
                Cmb_Perito.SelectedIndex = 0;
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message;
            }
        }
        
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
                Session["Grid_Documentos"] = Tabla;

                Grid_Documentos_Tramite.SelectedIndex = (-1);
                Grid_Documentos_Tramite.Columns[0].Visible = true;
                Grid_Documentos_Tramite.Columns[2].Visible = true;
                if (Tabla != null && Tabla.Rows.Count > 0)
                {
                    Grid_Documentos_Tramite.DataSource = Tabla;
                    Div_Documentos_Anexados.Style.Value = "border-style: solid; border-color: Silver; display: block";
                }
                else
                {
                    Grid_Documentos_Tramite.DataSource = new DataTable();
                    Div_Documentos_Anexados.Style.Value = "border-style: solid; border-color: Silver; display: block";
                }
                Grid_Documentos_Tramite.DataBind();
                Grid_Documentos_Tramite.Columns[0].Visible = false;
                Grid_Documentos_Tramite.Columns[2].Visible = false;

                //if (Txt_Detalle_Estatus.Text != "PENDIENTE")
                //{
                //    Grid_Documentos_Tramite.Columns[2].Visible = false;
                //}
            
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Historial
        ///DESCRIPCIÓN: Llena el Grid del historial de la solicitud
        ///PARAMETROS:
        ///             1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************      
        private void Llenar_Grid_Historial(DataTable Dt_Historial_Actividades)
        {
            try
            {
                if (Dt_Historial_Actividades != null && Dt_Historial_Actividades.Rows.Count > 0)
                {
                    Grid_Historial_Actividades.DataSource = Dt_Historial_Actividades;
                    Div_Grid_Historial.Style.Value = "overflow: auto; height: 150px;width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block";
                }
                else
                {
                    Grid_Historial_Actividades.DataSource = new DataTable();
                    Div_Grid_Historial.Style.Value = "overflow: auto;width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block";
                }

                Grid_Historial_Actividades.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// ********************************************************************************
        /// Nombre: Validar_Fechas
        /// Descripción: Valida que la Fecha Inicial no sea mayor que la Final
        /// Creo: Hugo Enrique Ramírez Aguilera
        /// Fecha Creo: 13/Julio/2012
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
        ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private String Obtener_Dato_Consulta(String Consulta)
        {
            String Dato_Consulta = "";

            try
            {
                OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Consulta);

                if (Dr_Dato.Read())
                {
                    if (Dr_Dato[0] != null)
                    {
                        Dato_Consulta = Dr_Dato[0].ToString();
                    }
                    else
                    {
                        Dato_Consulta = "";
                    }
                    Dr_Dato.Close();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato = null;
            }
            catch
            {
            }
            finally
            {
            }

            return Dato_Consulta;
        }

        ///*******************************************************************************
        /// NOMBRE:       Cargar_Combo_Unidad_Responsable
        /// DESCRIPCION : Cargara las unidades responsables 
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 30/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Cargar_Combo_Unidad_Responsable()
        {
            Cls_Cat_Dependencias_Negocio Rs_Responsable = new Cls_Cat_Dependencias_Negocio();
            DataTable Dt_Unidad_Responsable = new DataTable();
            try
            {
                //  1 para la unidad resposable
                Dt_Unidad_Responsable = Rs_Responsable.Consulta_Dependencias();
                //   2 SE ORDENA LA TABLA POR 
                DataView Dv_Ordenar = new DataView(Dt_Unidad_Responsable);
                Dv_Ordenar.Sort = Cat_Dependencias.Campo_Nombre;
                Dt_Unidad_Responsable = Dv_Ordenar.ToTable();
                Cmb_Unidad_Responsable_Filtro.DataSource = Dt_Unidad_Responsable;
                Cmb_Unidad_Responsable_Filtro.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
                Cmb_Unidad_Responsable_Filtro.DataTextField = Cat_Dependencias.Campo_Nombre;
                Cmb_Unidad_Responsable_Filtro.DataBind();
                Cmb_Unidad_Responsable_Filtro.Items.Insert(0, "< SELECCIONE >");

            }
            catch (Exception ex)
            {
                throw new Exception("Cargar_Grid " + ex.Message.ToString());
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Calcular_Costo
        ///DESCRIPCIÓN: habilita los comentarios
        ///PARAMETROS:     
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  10/Sempiembre/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        private Double Calcular_Costo(String Actividad_ID, Cls_Ope_Bandeja_Tramites_Negocio Neg_Solicitud)
        {
            DataTable Dt_Subproceso;
            DataTable Dt_Solicitud_Hijas = new DataTable();
            String Porcentaje_Penalizacion = "";
            Cls_Cat_Tramites_Negocio Neg_Consulta_Subprocesos = new Cls_Cat_Tramites_Negocio();
            Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
            Double Resultado = 0.0;
            Double Resultado_Solicitud_Hija = 0.0;
            Double Suma_Costos = 0.0;
            Double Costo_Bitacora = 0.0;
            Double Costo_Perito = 0.0;

            try
            {
                //  se consultan los parametros
                Obj_Parametros.Consultar_Parametros();

                if (!String.IsNullOrEmpty(Obj_Parametros.P_Costo_Bitacora))
                    Costo_Bitacora = Convert.ToDouble(Obj_Parametros.P_Costo_Bitacora);

                if (!String.IsNullOrEmpty(Obj_Parametros.P_Costo_Perito))
                    Costo_Perito = Convert.ToDouble(Obj_Parametros.P_Costo_Perito);

                //  si contiene solicitudes hijas se obtendra la suma de estas
                Dt_Solicitud_Hijas = Neg_Solicitud.Consultar_Solicitudes_Hija();
                Porcentaje_Penalizacion = Neg_Solicitud.Consultar_Penalizaciones();

                if (Dt_Solicitud_Hijas is DataTable)
                {
                    if (Dt_Solicitud_Hijas != null && Dt_Solicitud_Hijas.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Dt_Solicitud_Hijas.Rows)
                        {
                            if (Registro is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Registro[Ope_Tra_Solicitud.Campo_Costo_Total].ToString()))
                                {
                                    Resultado_Solicitud_Hija += Convert.ToDouble(Registro[Ope_Tra_Solicitud.Campo_Costo_Total].ToString());
                                }
                            }
                        }
                    }
                }

                //  proceso normal
                Neg_Consulta_Subprocesos.P_Sub_Proceso_ID = Actividad_ID;
                Dt_Subproceso = Neg_Consulta_Subprocesos.Consultar_Subprocesos_Tramite();

                // validar que la consulta haya regresado resultados
                if (Dt_Subproceso != null && Dt_Subproceso.Rows.Count > 0)
                {
                    // dar de alta el pasivo para el pago si el subproceso siguiente es de tipo COBRO
                    foreach (DataRow Fila_Subproceso in Dt_Subproceso.Rows)
                    {
                        // datos del tramite
                        Cls_Cat_Tramites_Negocio Negocio_Datos_Tramite = new Cls_Cat_Tramites_Negocio();
                        Negocio_Datos_Tramite.P_Tramite_ID = Neg_Solicitud.P_Tramite_id;
                        Negocio_Datos_Tramite = Negocio_Datos_Tramite.Consultar_Datos_Tramite();

                        //  datos del ditamen
                        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Dictamen = new Cls_Ope_Bandeja_Tramites_Negocio();
                        Negocio_Datos_Dictamen.P_Solicitud_ID = Neg_Solicitud.P_Solicitud_ID;
                        Negocio_Datos_Dictamen = Negocio_Datos_Dictamen.Consultar_Datos_Solicitud();
                        DataTable Dt_Consulta_Dato_Final_Llenado = Negocio_Datos_Dictamen.Consultar_Datos_Finales_Operacion();
                        DataTable Dt_Matriz_Costo = Negocio_Datos_Dictamen.Consultar_Costo_Matriz();

                        Double Parametro1 = 0.0;
                        Double Parametro2 = 0.0;
                        Double Costo_Matriz = 0.0;

                        int Cnt_Tipo_Matriz = 0;
                        String Tipo_Dato_Matriz = "";

                        if (!String.IsNullOrEmpty(Negocio_Datos_Tramite.P_Parametro1))
                        {
                            if (Dt_Consulta_Dato_Final_Llenado != null && Dt_Consulta_Dato_Final_Llenado.Rows.Count > 0)
                            {
                                foreach (DataRow Registro in Dt_Consulta_Dato_Final_Llenado.Rows)
                                {
                                    if (Negocio_Datos_Tramite.P_Parametro1 == Registro["NOMBRE_DATO"].ToString())
                                    {
                                        if (!String.IsNullOrEmpty(Registro["VALOR"].ToString()) && Registro["VALOR"].ToString() != "0")
                                        {
                                            Parametro1 = Convert.ToDouble(Registro["VALOR"].ToString());
                                            Cnt_Tipo_Matriz++;
                                        }
                                    }

                                    if (Registro["NOMBRE_DATO"].ToString().ToUpper().Trim() == "Tipo Matriz".ToUpper().Trim())
                                    {
                                        if (!String.IsNullOrEmpty(Registro["VALOR"].ToString()) && Registro["VALOR"].ToString() != "0")
                                        {
                                            Tipo_Dato_Matriz = Registro["VALOR"].ToString();
                                            Cnt_Tipo_Matriz++;
                                        }
                                    }

                                    if (Cnt_Tipo_Matriz == 2)
                                    {
                                        break;
                                    }

                                }
                            }
                        }// fin del if

                        if (Tipo_Dato_Matriz != "")
                        {
                            if (Dt_Matriz_Costo != null && Dt_Matriz_Costo.Rows.Count > 0)
                            {
                                foreach (DataRow Registro in Dt_Matriz_Costo.Rows)
                                {
                                    if (Registro[Ope_Tra_Matriz_Costo.Campo_Tipo].ToString().ToUpper().Trim() == Tipo_Dato_Matriz.Trim().ToUpper())
                                    {
                                        Costo_Matriz = Convert.ToDouble(Registro[Ope_Tra_Matriz_Costo.Campo_Costo_Base].ToString());
                                        break;
                                    }
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(Negocio_Datos_Tramite.P_Parametro2))
                        {
                            if (Dt_Consulta_Dato_Final_Llenado != null && Dt_Consulta_Dato_Final_Llenado.Rows.Count > 0)
                            {
                                foreach (DataRow Registro in Dt_Consulta_Dato_Final_Llenado.Rows)
                                {
                                    if (Negocio_Datos_Tramite.P_Parametro2 == Registro["NOMBRE_DATO"].ToString())
                                    {
                                        if (!String.IsNullOrEmpty(Registro["VALOR"].ToString()) && Registro["VALOR"].ToString() != "0")
                                        {
                                            Parametro2 = Convert.ToDouble(Registro["VALOR"].ToString());
                                            break;
                                        }
                                    }

                                }
                            }
                        }// fin del if

                        if (Parametro1 != 0 && Parametro2 != 0)
                        {
                            if (Negocio_Datos_Tramite.P_Operador2 == "+")
                                Resultado = Parametro1 + Parametro2;

                            else if (Negocio_Datos_Tramite.P_Operador2 == "-")
                                Resultado = Parametro1 - Parametro2;

                            else if (Negocio_Datos_Tramite.P_Operador2 == "/")
                                Resultado = Parametro1 / Parametro2;

                            else if (Negocio_Datos_Tramite.P_Operador2 == "*")
                                Resultado = Parametro1 * Parametro2;
                        }

                        else if (Parametro1 != 0)
                            Resultado = Parametro1;

                        else if (Parametro2 != 0)
                            Resultado = Parametro2;


                        //  se asigna el costo de la matriz o del detalle de la solicitud
                        if (Resultado != 0.0)
                        {
                            if (Costo_Matriz == 0.0)
                                Costo_Matriz = Negocio_Datos_Tramite.P_Costo;

                            //  operador sera el primero que compara costo del [(tramite) {operador * - / +} (parametros)]
                            if (Negocio_Datos_Tramite.P_Operador1 == "+")
                                Resultado = Resultado + Costo_Matriz;

                            else if (Negocio_Datos_Tramite.P_Operador1 == "-")
                                Resultado = Resultado - Costo_Matriz;

                            else if (Negocio_Datos_Tramite.P_Operador1 == "/")
                                Resultado = Resultado / Costo_Matriz;

                            else if (Negocio_Datos_Tramite.P_Operador1 == "*")
                                Resultado = Resultado * Costo_Matriz;

                        }
                        else
                            Resultado = Convert.ToDouble(Neg_Solicitud.P_Costo_Total);
                    }

                    if (Porcentaje_Penalizacion != "")
                    {
                        Suma_Costos = (Resultado_Solicitud_Hija + Resultado) * (1 + ((Convert.ToDouble(Porcentaje_Penalizacion)) / 100));
                    }
                    else
                    {
                        Suma_Costos = (Resultado_Solicitud_Hija + Resultado);
                    }

                    if (!String.IsNullOrEmpty(Neg_Solicitud.P_Inspector_ID))
                    {
                        Suma_Costos += Costo_Bitacora + Costo_Perito;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Calcular_Costo " + ex.Message.ToString());
            }
            
            return Suma_Costos;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Resumen_Predio
        ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
        ///PARAMETROS: 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 09/Julio/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Cargar_Ventana_Emergente_Resumen_Predio()
        {
            String Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Frm_Resumen_Predio_Ventanilla.aspx";
            String Propiedades = ", 'center:yes,resizable=no,status=no,width=750,scrollbars=yes,');";
            //String Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:800px;dialogHeight:600px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Extension
        ///DESCRIPCIÓN:     Maneja la Extension del archivo
        ///PROPIEDADES:     String Ruta, direccion que 
        ///                 contiene el nombre del archivo al cual se le sacara la extension
        ///CREO:            Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:      03/Mayo/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO: 
        ///CAUSA_MODIFICACIÓN: 
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

                //  se ordenara la tabla por fecha
                DataView Dv_Ordenar = new DataView(Tabla);
                Dv_Ordenar.Sort = Cat_Tra_Datos_Tramite.Campo_Dato_ID;//SOLICITO asc, 
                DataTable Dt_Datos_Ordenados = Dv_Ordenar.ToTable();
                Session["Grid_Datos"] = Dt_Datos_Ordenados;

                Grid_Datos.SelectedIndex = (-1);
                Grid_Datos.Columns[0].Visible = true;
                Grid_Datos.Columns[1].Visible = true;
                Grid_Datos.Columns[4].Visible = true;
                if (Tabla != null && Tabla.Rows.Count > 0)
                {
                    Grid_Datos.DataSource = Dt_Datos_Ordenados;
                    Grid_Datos.Enabled = false;
                    Div_Grid_Datos_Tramite.Style.Value = "overflow: auto; height: 150px;width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block";
                }
                else
                {
                    Grid_Datos.DataSource = new DataTable();
                    Grid_Datos.Enabled = true;
                    Div_Grid_Datos_Tramite.Style.Value = "overflow: auto;width: 99%; vertical-align: top; border-style: solid; border-color: Silver; display: block";
                }
                Grid_Datos.DataBind();
                Grid_Datos.Columns[0].Visible = false;
                Grid_Datos.Columns[1].Visible = false;
                Grid_Datos.Columns[4].Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Formato
        ///DESCRIPCIÓN:     Maneja la Extension del archivo
        ///PROPIEDADES:     String Ruta, direccion que 
        ///                 contiene el nombre del archivo al cual se le sacara la extension
        ///CREO:            Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:      03/Mayo/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        private void Generar_Reporte_Formato(String Clave_Unica, DateTime Fecha_Solucion, DateTime Fecha_Tramite, String Nombre_Completo, String Telefono, String Cuenta_Predial, String Perito_ID, String Solicitud_ID, DataTable Dt_Datos)
        {

            Cls_Ope_Solicitud_Tramites_Negocio Negocio_Actividades_Realizadas = new Cls_Ope_Solicitud_Tramites_Negocio();
            Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
            Cls_Cat_Dependencias_Negocio Negocio_Dependencia = new Cls_Cat_Dependencias_Negocio();
            Cls_Cat_Tramites_Negocio Negocio_Tramite = new Cls_Cat_Tramites_Negocio();
            DataTable Dt_Datos_Inmueble = new DataTable();
            DataTable Dt_Datos_Propietario = new DataTable();
            DataTable Dt_Datos_Solictud = new DataTable();
            DataTable Dt_Perito = new DataTable();
            DataTable Dt_Ubicacion_Obra = new DataTable();
            DataTable Dt_Dependencia = new DataTable();
            DataTable Dt_Datos_Documentos = new DataTable();
            DataRow Fila;
            String Valor_Dato = "";

            Ds_Rpt_Remodelacion_Ampliacion Ds_Reporte = new Ds_Rpt_Remodelacion_Ampliacion();
            DataSet Ds_Consulta = new DataSet();
            try
            {
                //  se carga la informacion de la solitidud
                Negocio_Datos_Solicitud.P_Solicitud_ID = Solicitud_ID;
                Negocio_Datos_Solicitud = Negocio_Datos_Solicitud.Consultar_Datos_Solicitud();

                //  se carga la informacion del tramite
                Negocio_Tramite.P_Tramite_ID = Negocio_Datos_Solicitud.P_Tramite_id;
                Negocio_Tramite = Negocio_Tramite.Consultar_Datos_Tramite();

                //   se consulta la dependencia
                Negocio_Dependencia.P_Dependencia_ID = Negocio_Datos_Solicitud.P_Dependencia_ID;
                Dt_Dependencia = Negocio_Dependencia.Consulta_Dependencias();

                //  previsualizacion de la solicitud
                Dt_Datos_Inmueble = Ds_Reporte.Dt_Datos_Inmueble.Clone();
                Dt_Datos_Propietario = Ds_Reporte.Dt_Datos_Propietario.Clone();
                Dt_Datos_Solictud = Ds_Reporte.Dt_Datos_Solictud.Clone();
                Dt_Perito = Ds_Reporte.Dt_Perito.Clone();
                Dt_Ubicacion_Obra = Ds_Reporte.Dt_Ubicacion_Obra.Clone();
                Dt_Datos_Documentos = Ds_Reporte.Dt_Datos_Documentos.Clone();

                //  para la ubicacion de la obra
                Negocio_Actividades_Realizadas.P_Cuenta_Predial = Cuenta_Predial;
                Dt_Ubicacion_Obra = Negocio_Actividades_Realizadas.Consultar_Datos_Obra();

                //  para los datos del perito
                Negocio_Actividades_Realizadas.P_Perito_ID = Perito_ID;
                Dt_Perito = Negocio_Actividades_Realizadas.Consultar_Inspectores();
                Dt_Perito.Columns.Add("NO_PERITO", typeof(String));
                Dt_Perito.AcceptChanges();
                if (Dt_Perito != null && Dt_Perito.Rows.Count > 0)
                    Dt_Perito.Rows[0]["NO_PERITO"] = Perito_ID;

                //  para los datos de la solicutd
                Fila = Dt_Datos_Solictud.NewRow();
                Fila["CLAVE_SOLICITUD"] = Clave_Unica;
                Fila["FECHA_TRAMITE"] = Convert.ToDateTime(Fecha_Tramite);
                Fila["FECHA_ENTREGA"] = Convert.ToDateTime(Fecha_Solucion);
                Fila["SOLICITUD_ID"] = Negocio_Datos_Solicitud.P_Consecutivo;
                Fila["DEPENDENCIA"] = Dt_Dependencia.Rows[0][Cat_Dependencias.Campo_Nombre].ToString();
                Fila["AREA"] = Negocio_Datos_Solicitud.P_Area_Dependencia;
                Fila["CLAVE_TRAMITE"] = Negocio_Tramite.P_Clave_Tramite + " " + Negocio_Tramite.P_Nombre;
                Dt_Datos_Solictud.Rows.Add(Fila);


                Dt_Ubicacion_Obra.Columns.Add("CUENTA_PREDIAL", typeof(String));
                if (Cuenta_Predial != "")
                {
                    Dt_Ubicacion_Obra.Rows[0]["CUENTA_PREDIAL"] = Cuenta_Predial;
                }
                //  para los datos del propietario
                Fila = Dt_Datos_Propietario.NewRow();
                Fila["NOMBRE"] = Negocio_Datos_Solicitud.P_Propietario_Predio;
                Fila["DOMICILIO"] = Negocio_Datos_Solicitud.P_Calle_Predio;
                Fila["COLONIA"] = Negocio_Datos_Solicitud.P_Direccion_Predio;
                Fila["TELEFONO"] = "";
                Dt_Datos_Propietario.Rows.Add(Fila);



                //  para los datos del tramite
                if (Negocio_Tramite.P_Documentacion_Tramite != null && Negocio_Tramite.P_Documentacion_Tramite.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Negocio_Tramite.P_Documentacion_Tramite.Rows)
                    {
                        Fila = Dt_Datos_Documentos.NewRow();
                        Fila["NOMBRE"] = Registro["NOMBRE"].ToString();
                        Fila["DESCRIPCION"] = Registro["DESCRIPCION"].ToString();
                        Dt_Datos_Documentos.Rows.Add(Fila);
                    }
                    //Dt_Datos_Documentos = Negocio_Tramite.P_Documentacion_Tramite.Copy();
                }
                //  para los datos de la solicitud
                //  para obtener la informacion de los datos
                foreach (DataRow Registro in Dt_Datos.Rows)
                {
                    String Nombre_Dato = Registro["NOMBRE_DATO"].ToString();
                    Valor_Dato = Registro["VALOR"].ToString();
                    //  se agregan los campos a la tabla de datos del inmueble
                    Fila = Dt_Datos_Inmueble.NewRow();
                    Fila["NOMBRE_DATO"] = Nombre_Dato;
                    Fila["VALOR"] = Valor_Dato;
                    Dt_Datos_Inmueble.Rows.Add(Fila);
                }

                Dt_Ubicacion_Obra.TableName = "Dt_Ubicacion_Obra";
                Dt_Perito.TableName = "Dt_Perito";
                Dt_Datos_Solictud.TableName = "Dt_Datos_Solictud";
                Dt_Datos_Inmueble.TableName = "Dt_Datos_Inmueble";
                Dt_Datos_Propietario.TableName = "Dt_Datos_Propietario";
                Dt_Datos_Documentos.TableName = "Dt_Datos_Documentos";

                Ds_Reporte.Clear();
                Ds_Reporte.Tables.Clear();
                Ds_Reporte.Tables.Add(Dt_Ubicacion_Obra.Copy());
                Ds_Reporte.Tables.Add(Dt_Perito.Copy());
                Ds_Reporte.Tables.Add(Dt_Datos_Solictud.Copy());
                Ds_Reporte.Tables.Add(Dt_Datos_Inmueble.Copy());
                Ds_Reporte.Tables.Add(Dt_Datos_Propietario.Copy());
                Ds_Reporte.Tables.Add(Dt_Datos_Documentos.Copy());
                Generar_Reporte_Solicitud_Formato(Ds_Reporte, "PDF", "Formato_Solicitud");

            }
            catch (Exception ex)
            {
                throw new Exception("Alta_Tramite " + ex.Message.ToString(), ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Formato
        ///DESCRIPCIÓN:     Maneja la Extension del archivo
        ///PROPIEDADES:     String Ruta, direccion que 
        ///                 contiene el nombre del archivo al cual se le sacara la extension
        ///CREO:            Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:      03/Mayo/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        private void Generar_Reporte_Formato_Catastro(String Clave_Unica, DateTime Fecha_Solucion, DateTime Fecha_Tramite, String Nombre_Completo, String Telefono, String Cuenta_Predial, String Perito_ID, String Solicitud_ID, DataTable Dt_Datos)
        {

            Cls_Ope_Solicitud_Tramites_Negocio Negocio_Actividades_Realizadas = new Cls_Ope_Solicitud_Tramites_Negocio();
            Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
            Cls_Cat_Dependencias_Negocio Negocio_Dependencia = new Cls_Cat_Dependencias_Negocio();
            Cls_Cat_Tramites_Negocio Negocio_Tramite = new Cls_Cat_Tramites_Negocio();
            DataTable Dt_Datos_Inmueble = new DataTable();
            DataTable Dt_Datos_Propietario = new DataTable();
            DataTable Dt_Datos_Solictud = new DataTable();
            DataTable Dt_Perito = new DataTable();
            DataTable Dt_Ubicacion_Obra = new DataTable();
            DataTable Dt_Dependencia = new DataTable();
            DataTable Dt_Datos_Documentos = new DataTable();
            DataRow Fila;
            String Valor_Dato = "";

            Ds_Rpt_Remodelacion_Ampliacion Ds_Reporte = new Ds_Rpt_Remodelacion_Ampliacion();
            DataSet Ds_Consulta = new DataSet();
            try
            {
                //  se carga la informacion de la solitidud
                Negocio_Datos_Solicitud.P_Solicitud_ID = Solicitud_ID;
                Negocio_Datos_Solicitud = Negocio_Datos_Solicitud.Consultar_Datos_Solicitud();

                //  se carga la informacion del tramite
                Negocio_Tramite.P_Tramite_ID = Negocio_Datos_Solicitud.P_Tramite_id;
                Negocio_Tramite = Negocio_Tramite.Consultar_Datos_Tramite();

                //   se consulta la dependencia
                Negocio_Dependencia.P_Dependencia_ID = Negocio_Datos_Solicitud.P_Dependencia_ID;
                Dt_Dependencia = Negocio_Dependencia.Consulta_Dependencias();

                //  previsualizacion de la solicitud
                Dt_Datos_Inmueble = Ds_Reporte.Dt_Datos_Inmueble.Clone();
                Dt_Datos_Propietario = Ds_Reporte.Dt_Datos_Propietario.Clone();
                Dt_Datos_Solictud = Ds_Reporte.Dt_Datos_Solictud.Clone();
                Dt_Perito = Ds_Reporte.Dt_Perito.Clone();
                Dt_Ubicacion_Obra = Ds_Reporte.Dt_Ubicacion_Obra.Clone();
                Dt_Datos_Documentos = Ds_Reporte.Dt_Datos_Documentos.Clone();

                //  para la ubicacion de la obra
                Negocio_Actividades_Realizadas.P_Cuenta_Predial = Cuenta_Predial;
                Dt_Ubicacion_Obra = Negocio_Actividades_Realizadas.Consultar_Datos_Obra();

                //  para los datos del perito
                Negocio_Actividades_Realizadas.P_Perito_ID = Perito_ID;
                Dt_Perito = Negocio_Actividades_Realizadas.Consultar_Inspectores();
                Dt_Perito.Columns.Add("NO_PERITO", typeof(String));
                Dt_Perito.Rows[0]["NO_PERITO"] = Perito_ID;
                //  para los datos de la solicutd
                Fila = Dt_Datos_Solictud.NewRow();
                Fila["CLAVE_SOLICITUD"] = Clave_Unica;
                Fila["FECHA_TRAMITE"] = Convert.ToDateTime(Fecha_Tramite);
                Fila["FECHA_ENTREGA"] = Convert.ToDateTime(Fecha_Solucion);
                Fila["SOLICITUD_ID"] = Solicitud_ID;
                Fila["DEPENDENCIA"] = Dt_Dependencia.Rows[0][Cat_Dependencias.Campo_Nombre].ToString();
                Fila["AREA"] = Negocio_Datos_Solicitud.P_Area_Dependencia;
                Fila["CLAVE_TRAMITE"] = Negocio_Tramite.P_Clave_Tramite;
                Dt_Datos_Solictud.Rows.Add(Fila);


                if (Dt_Ubicacion_Obra != null && Dt_Ubicacion_Obra.Rows.Count > 0)
                {
                    Dt_Ubicacion_Obra.Columns.Add("CUENTA_PREDIAL", typeof(String));
                    Dt_Ubicacion_Obra.Rows[0]["CUENTA_PREDIAL"] = Cuenta_Predial;
                    foreach (DataRow Registro in Dt_Ubicacion_Obra.Rows)
                    {
                        //  para los datos del propietario
                        Fila = Dt_Datos_Propietario.NewRow();
                        Fila["NOMBRE"] = Nombre_Completo;
                        Fila["DOMICILIO"] = Registro["Calle"].ToString();
                        Fila["COLONIA"] = Registro["Colonia"].ToString();
                        Fila["TELEFONO"] = Telefono;
                        Dt_Datos_Propietario.Rows.Add(Fila);
                        break;
                    }
                }

                //  para los datos del tramite
                if (Negocio_Tramite.P_Documentacion_Tramite != null && Negocio_Tramite.P_Documentacion_Tramite.Rows.Count > 0)
                {
                    Dt_Datos_Documentos = Negocio_Tramite.P_Documentacion_Tramite.Copy();
                }
                //  para los datos de la solicitud
                //  para obtener la informacion de los datos
                if (Negocio_Datos_Solicitud.P_Datos_Solicitud.Rows.Count > 0 && Negocio_Datos_Solicitud.P_Datos_Solicitud != null)
                {
                    foreach (DataRow Registro in Negocio_Datos_Solicitud.P_Datos_Solicitud.Rows)
                    {
                        String Nombre_Dato = Registro["NOMBRE_DATO"].ToString();
                        Valor_Dato = Registro["VALOR"].ToString();
                        //  se agregan los campos a la tabla de datos del inmueble
                        Fila = Dt_Datos_Inmueble.NewRow();
                        Fila["NOMBRE_DATO"] = Nombre_Dato;
                        Fila["VALOR"] = Valor_Dato;
                        Dt_Datos_Inmueble.Rows.Add(Fila);
                    }
                }

                Dt_Ubicacion_Obra.TableName = "Dt_Ubicacion_Obra";
                Dt_Perito.TableName = "Dt_Perito";
                Dt_Datos_Solictud.TableName = "Dt_Datos_Solictud";
                Dt_Datos_Inmueble.TableName = "Dt_Datos_Inmueble";
                Dt_Datos_Propietario.TableName = "Dt_Datos_Propietario";
                Dt_Datos_Documentos.TableName = "Dt_Datos_Documentos";

                Ds_Reporte.Clear();
                Ds_Reporte.Tables.Clear();
                Ds_Reporte.Tables.Add(Dt_Ubicacion_Obra.Copy());
                Ds_Reporte.Tables.Add(Dt_Perito.Copy());
                Ds_Reporte.Tables.Add(Dt_Datos_Solictud.Copy());
                Ds_Reporte.Tables.Add(Dt_Datos_Inmueble.Copy());
                Ds_Reporte.Tables.Add(Dt_Datos_Propietario.Copy());
                Ds_Reporte.Tables.Add(Dt_Datos_Documentos.Copy());
                Generar_Reporte_Solicitud_Formato_Catastro(Ds_Reporte, "PDF", "Formato_Solicitud");

            }
            catch (Exception ex)
            {
                throw new Exception("Alta_Tramite " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Solicitud_Formato
        ///DESCRIPCIÓN: genera el reporte de pdf
        ///PARÁMETROS : 	
        ///         1. Ds_Reporte: Dataset con datos a imprimir
        /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
        /// 		3. Nombre_Archivo: Nombre del archivo a generar
        /// 		4. Tipo: Parámetro para ventana emergente
        ///CREO       : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO  : 02-Julio-2012
        ///MODIFICO          :
        ///FECHA_MODIFICO    :
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public void Generar_Reporte_Solicitud_Formato_Catastro(DataSet Ds_Reporte, String Extension_Archivo, String Tipo)
        {
            String Nombre_Archivo = "Reporte_Formato_" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
            String Ruta_Archivo = @Server.MapPath("../Rpt/Ordenamiento_Territorial/");//Obtiene la ruta en la cual será guardada el archivo
            ReportDocument Reporte = new ReportDocument();

            try
            {
                Reporte.Load(Ruta_Archivo + "Rpt_Ort_Solicitud_Tramite.rpt");
                Reporte.SetDataSource(Ds_Reporte);

                DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //  para el tipo de archivo
                if (Extension_Archivo == "PDF")
                    Nombre_Archivo += ".pdf";
                else if (Extension_Archivo == "EXCEL")
                    Nombre_Archivo += ".xls";

                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;

                //  para el tipo de archivo
                if (Extension_Archivo == "PDF")
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                else if (Extension_Archivo == "EXCEL")
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;

                Reporte.Export(Opciones_Exportacion);

                if (Extension_Archivo == "PDF")
                    Abrir_Ventana(Nombre_Archivo, Tipo);
                else if (Extension_Archivo == "EXCEL")
                {
                    String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', '" + Tipo + "','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Generar_Reporte_Folio_Solicitud
        ///DESCRIPCIÓN: Breve descripción de lo que hace la función.
        ///PARÁMETROS:
        /// 		1. Clave_Unica: folio de la solicitud
        /// 		2. Fecha_Solucion: fecha de solución probable para la solicitud
        /// 		3. Nombre_Completo: nombre del solicitante
        /// 		4. Email: correo electrónico del solicitante
        /// 		5. Nombre_Tramite: nombre del trámite que se pasa como parámetro
        /// 		6. Cuenta_Predial: cadena de texto con el número de cuenta predial de la solicitud
        /// 		7. Propietario: nombre del propietario de la cuenta predial en la solicitud
        /// 		7. Solicitud_ID: Id de la solicitud
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 11-jul-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private void Generar_Reporte_Folio_Solicitud(string Clave_Unica, DateTime Fecha_Solucion, string Nombre_Completo, string Email, string Nombre_Tramite, string Cuenta_Predial, string Propietario,
                    String Solicitud_ID, String Dependencia, String Area, string Folio)
        {
            Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite Ds_Reporte = new Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite();
            Cls_Cat_Dependencias_Negocio Negocio_Dependencia = new Cls_Cat_Dependencias_Negocio();
            DataTable Dt_Dependencia = new DataTable();
            try
            {
                Negocio_Dependencia.P_Dependencia_ID = Dependencia;
                Dt_Dependencia = Negocio_Dependencia.Consulta_Dependencias();

                DataRow Fila = Ds_Reporte.Tables["Dt_Datos_Solicitud"].NewRow();
                Fila["CLAVE_SOLICITUD"] = Clave_Unica;
                Fila["PORCENTAJE_AVANCE"] = Txt_Detalle_Porcentaje.Text;
                Fila["ESTATUS"] = Txt_Detalle_Estatus.Text;
                Fila["FECHA_TRAMITE"] = Txt_Detalle_Fecha_Inicio.Text;
                Fila["FECHA_ENTREGA"] = Fecha_Solucion;
                Fila["NOMBRE_COMPLETO"] = Nombre_Completo;
                Fila["CORREO_ELECTRONICO"] = Email;
                Fila["NOMBRE"] = "";
                Fila["NOMBRE_TRAMITE"] = Nombre_Tramite;
                Fila["CUENTA_PREDIAL"] = Cuenta_Predial;
                Fila["PROPIETARIO_CUENTA"] = Propietario;
                Fila["CONSECUTIVO"] = Solicitud_ID;
                Fila["DEPENDENCIA"] = Dt_Dependencia.Rows[0][Cat_Dependencias.Campo_Nombre].ToString();
                Fila["AREA"] = Area;
                Fila["FOLIO"] = Folio;
                Ds_Reporte.Tables["Dt_Datos_Solicitud"].Rows.Add(Fila);

                Generar_Reporte(Ds_Reporte, "PDF", "Folio_Solicitud", "../Rpt/Ventanilla/Rpt_Ven_Folio_Solicitud_Tramite.rpt");

            }
            catch (Exception ex)
            {
                throw new Exception("Generar_Reporte_Folio_Solicitud: " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Solicitud_Formato
        ///DESCRIPCIÓN: genera el reporte de pdf
        ///PARÁMETROS : 	
        ///         1. Ds_Reporte: Dataset con datos a imprimir
        /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
        /// 		3. Nombre_Archivo: Nombre del archivo a generar
        /// 		4. Tipo: Parámetro para ventana emergente
        ///CREO       : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO  : 02-Julio-2012
        ///MODIFICO          :
        ///FECHA_MODIFICO    :
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public void Generar_Reporte_Solicitud_Formato(DataSet Ds_Reporte, String Extension_Archivo, String Tipo)
        {
            String Nombre_Archivo = "Reporte_Formato_" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
            String Ruta_Archivo = @Server.MapPath("../Rpt/Ordenamiento_Territorial/");//Obtiene la ruta en la cual será guardada el archivo
            ReportDocument Reporte = new ReportDocument();

            try
            {
                Reporte.Load(Ruta_Archivo + "Rpt_Ort_Remodelacion_Ampliacion.rpt");
                Reporte.SetDataSource(Ds_Reporte);

                DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //  para el tipo de archivo
                if (Extension_Archivo == "PDF")
                    Nombre_Archivo += ".pdf";
                else if (Extension_Archivo == "EXCEL")
                    Nombre_Archivo += ".xls";

                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;

                //  para el tipo de archivo
                if (Extension_Archivo == "PDF")
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                else if (Extension_Archivo == "EXCEL")
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;

                Reporte.Export(Opciones_Exportacion);

                if (Extension_Archivo == "PDF")
                    Abrir_Ventana(Nombre_Archivo, Tipo);
                else if (Extension_Archivo == "EXCEL")
                {
                    String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', '" + Tipo + "','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
        ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
        ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
        ///                             para mostrar los datos al usuario
        ///CREO       : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO  : 02-Julio-2012
        ///MODIFICO          :
        ///FECHA_MODIFICO    :
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        private void Abrir_Ventana(String Nombre_Archivo, String Tipo)
        {
            String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
            try
            {
                Pagina = Pagina + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), Tipo,
                "window.open('" + Pagina + "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            catch (Exception ex)
            {
                throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Imprimir
        ///DESCRIPCIÓN          : Manda a imprimir el reporte con el formato indicado
        ///PARAMETROS:     
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 17/Septiembre/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Imprimir_Reporte(String Formato)
        {
            String Nombre_Repote_Crystal = "Rpt_Ing_Orden_Pago.rpt";
            String Nombre_Reporte = "Reporte de Orden de Pago_";

            Cls_Ope_Bandeja_Tramites_Negocio Negocio_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
            Cls_Cat_Tramites_Negocio Negocio_Tramite = new Cls_Cat_Tramites_Negocio();
            Cls_Rpt_Ven_Consultar_Tramites_Negocio Negocio_Consulta_Subconcepto = new Cls_Rpt_Ven_Consultar_Tramites_Negocio();

            Ds_Ope_Ing_Orden_Pago Ds_Orden_Pago = new Ds_Ope_Ing_Orden_Pago();
            DataTable Dt_Orden_Pago = Ds_Orden_Pago.Tables["Dt_Orden_Pago"].Copy();
            DataTable Dt_Conceptos_Orden_Pago = Ds_Orden_Pago.Tables["Dt_Conceptos_Orden_Pago"].Copy();
            DataTable Dt_Contribuyente = Ds_Orden_Pago.Tables["Dt_Contribuyente"].Copy();
            DataTable Dt_Descuentos_Orden_Pago = Ds_Orden_Pago.Tables["Dt_Descuentos_Orden_Pago"].Copy();
            DataTable Dt_Datos_Ciudadano = Cls_Sessiones.Datos_Ciudadano;
            DataTable Dt_Consulta = new DataTable();
            DataTable Dt_Consulta_Fecha_Pasivo = new DataTable();

            // Se obtienen los Datos a Detalle de la Solicitud Seleccionada
            Negocio_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
            Negocio_Solicitud = Negocio_Solicitud.Consultar_Datos_Solicitud();
            // Se obtienen los Datos del tramite
            Negocio_Tramite.P_Tramite_ID = Negocio_Solicitud.P_Tramite_id;
            Negocio_Tramite = Negocio_Tramite.Consultar_Datos_Tramite();
            //  se obtiene el nombre de la clave y fundamento
            Negocio_Consulta_Subconcepto.P_Clave = Negocio_Tramite.P_Cuenta_ID;
            Dt_Consulta = Negocio_Consulta_Subconcepto.Consultar_Clave_Fundamento();
            //  se consulta la fecha del pasivo
            Negocio_Consulta_Subconcepto.P_Clave_Solicitud = Negocio_Solicitud.P_Clave_Solicitud;
            Dt_Consulta_Fecha_Pasivo = Negocio_Consulta_Subconcepto.Consultar_Fecha_Pasivo();

            //  para la orden de pago
            DataRow Dr_Orden_Pago;
            Dr_Orden_Pago = Dt_Orden_Pago.NewRow();
            Dr_Orden_Pago["NO_ORDEN_PAGO"] = "00000";
            Dr_Orden_Pago["AÑO"] = DateTime.Today.Year;
            Dr_Orden_Pago["NOMBRE_CONTRIBUYENTE"] = Cls_Sessiones.Nombre_Ciudadano;
            Dr_Orden_Pago["CONTRIBUYENTE_ID"] = Cls_Sessiones.Ciudadano_ID;
            Dr_Orden_Pago["FOLIO"] = Txt_Detalle_Clave.Text.Trim().ToUpper(); 
            Dr_Orden_Pago["ESTATUS"] = "";
            Dr_Orden_Pago["PROTECCION"] = "";
            if (Dt_Consulta_Fecha_Pasivo is DataTable)
            {
                if (Dt_Consulta_Fecha_Pasivo != null && Dt_Consulta_Fecha_Pasivo.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Consulta_Fecha_Pasivo.Rows)
                    {
                        if (Registro is DataRow)
                        {
                            Dr_Orden_Pago["FECHA_CREO"] = Registro[Ope_Ing_Pasivo.Campo_Fecha_Creo].ToString();
                            Dr_Orden_Pago["USUARIO_CREO"] = Registro[Ope_Ing_Pasivo.Campo_Usuario_Creo].ToString();
                            break;
                        }
                    }
                }
            }
            Dt_Orden_Pago.Rows.Add(Dr_Orden_Pago);
            Dt_Orden_Pago.TableName = "Dt_Orden_Pago";

            //  para el concepto de orden de pago
            DataRow Dr_Concepto_Orden_Pago;
            Dr_Concepto_Orden_Pago = Dt_Conceptos_Orden_Pago.NewRow();
            Dr_Concepto_Orden_Pago["NO_ORDEN_PAGO"] = "00000";
            Dr_Concepto_Orden_Pago["AÑO"] = DateTime.Today.Year;
            if (Dt_Consulta is DataTable)
            {
                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Consulta.Rows)
                    {
                        if (Registro is DataRow)
                        {
                            Dr_Concepto_Orden_Pago["CLAVE_INGRESO"] = Registro[Cat_Psp_SubConcepto_Ing.Campo_Descripcion].ToString();
                            Dr_Concepto_Orden_Pago["FUNDAMENTO"] = Registro[Cat_Psp_SubConcepto_Ing.Campo_Fundamento].ToString();
                        }
                    }
                }
            }
            if (Dt_Consulta_Fecha_Pasivo is DataTable)
            {
                if (Dt_Consulta_Fecha_Pasivo != null && Dt_Consulta_Fecha_Pasivo.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Consulta_Fecha_Pasivo.Rows)
                    {
                        if (Registro is DataRow)
                        {
                            Dr_Concepto_Orden_Pago["OBSERVACIONES"] = Registro[Ope_Ing_Pasivo.Campo_Origen].ToString();
                        }
                    }
                }
            }
            Dr_Concepto_Orden_Pago["HONORARIOS"] = 0;
            Dr_Concepto_Orden_Pago["MULTAS"] = 0;
            Dr_Concepto_Orden_Pago["DESCUENTO_IMPORTE"] = 0;
            Dr_Concepto_Orden_Pago["AJUSTE_TARIFARIO"] = 0;
            Dr_Concepto_Orden_Pago["MONTO_IMPORTE"] = 0;
            Dr_Concepto_Orden_Pago["MORATORIOS"] = 0;
            Dr_Concepto_Orden_Pago["RECARGOS"] = 0;
            Dr_Concepto_Orden_Pago["IMPORTE"] = Convert.ToDouble(Hdf_Costo.Value.ToString().Trim().ToUpper());
            Dr_Concepto_Orden_Pago["REFERENCIA"] = Txt_Detalle_Clave.Text.Trim().ToUpper();
            Dr_Concepto_Orden_Pago["UNIDADES"] = Convert.ToDouble(Txt_Detalle_Cantidad.Text.Trim().ToUpper());
            Dr_Concepto_Orden_Pago["TOTAL"] = Convert.ToDouble(Hdf_Costo.Value.ToString().Trim().ToUpper()) * Convert.ToDouble(Txt_Detalle_Cantidad.Text.Trim().ToUpper());
            Dt_Conceptos_Orden_Pago.Rows.Add(Dr_Concepto_Orden_Pago);
            Dt_Conceptos_Orden_Pago.TableName = "Dt_Conceptos_Orden_Pago";

            //  para la informacion del contribuyente
            DataRow Dr_Contribuyente;
            if (Dt_Datos_Ciudadano is DataTable)
            {
                if (Dt_Datos_Ciudadano != null && Dt_Datos_Ciudadano.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Datos_Ciudadano.Rows)
                    {
                        if (Registro is DataRow)
                        {
                            Dr_Contribuyente = Dt_Contribuyente.NewRow();
                            Dr_Contribuyente["CONTRIBUYENTE_ID"] = Cls_Sessiones.Ciudadano_ID;
                            Dr_Contribuyente["RFC"] = Registro[Cat_Pre_Contribuyentes.Campo_RFC].ToString();
                            Dr_Contribuyente["CURP"] = Registro[Cat_Pre_Contribuyentes.Campo_CURP].ToString();
                            Dr_Contribuyente["IFE"] = Registro[Cat_Pre_Contribuyentes.Campo_CURP].ToString();
                            Dr_Contribuyente["SEXO"] = Registro[Cat_Pre_Contribuyentes.Campo_Sexo].ToString();
                            Dr_Contribuyente["ESTATUS"] = Registro[Cat_Pre_Contribuyentes.Campo_Estatus].ToString();
                            Dr_Contribuyente["CALLE_FISCAL"] = Registro[Cat_Pre_Contribuyentes.Campo_Calle_Ubicacion].ToString();
                            Dr_Contribuyente["COLONIA_FISCAL"] = Registro[Cat_Pre_Contribuyentes.Campo_Colonia_Ubicacion].ToString();
                            Dr_Contribuyente["NO_EXTERIOR_FISCAL"] = "";
                            Dr_Contribuyente["NO_INTERIOR_FISCAL"] = "";
                            Dr_Contribuyente["ESTADO_FISCAL"] = Registro[Cat_Pre_Contribuyentes.Campo_Estado_Ubicacion].ToString();
                            Dr_Contribuyente["CIUDAD_FISCAL"] = Registro[Cat_Pre_Contribuyentes.Campo_Ciudad_Ubicacion].ToString();
                            Dt_Contribuyente.Rows.Add(Dr_Contribuyente);
                        }
                    }
                }
            }
            Dt_Contribuyente.TableName = "Dt_Contribuyente";

            DataRow Dr_Descuentos_Orden_Pago;

            Dr_Descuentos_Orden_Pago = Dt_Descuentos_Orden_Pago.NewRow();
            Dr_Descuentos_Orden_Pago["NOMBRE_CONTRIBUYENTE"] = Cls_Sessiones.Nombre_Ciudadano;
            Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Referencia] = Dt_Orden_Pago.Rows[0][Ope_Ing_Ordenes_Pago.Campo_Folio].ToString();// Txt_Referencia.Text.Trim();
            Dr_Descuentos_Orden_Pago["HONORARIOS"] = 0;//Convert.ToDecimal(Txt_Honorarios.Text);
            Dr_Descuentos_Orden_Pago["MULTAS"] = 0;//Convert.ToDecimal(Txt_Multas.Text);
            Dr_Descuentos_Orden_Pago["MORATORIOS"] = 0;//Convert.ToDecimal(Txt_Moratorios.Text);
            Dr_Descuentos_Orden_Pago["RECARGOS"] = 0;//Convert.ToDecimal(Txt_Recargos.Text);
            Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Monto_Honorarios] = 0;// Convert.ToDecimal(Txt_Monto_Honorarios.Text);
            Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Monto_Multas] = 0;
            Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Monto_Moratorios] = 0;
            Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Monto_Recargos] = 0;
            Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Descuento_Honorarios] = 0;// Convert.ToDecimal(Txt_Descuento_Honorarios.Text);
            Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Descuento_Multas] = 0;
            Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Descuento_Moratorios] = 0;
            Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Descuento_Recargos] = 0;
            Dr_Descuentos_Orden_Pago[Ope_Ing_Descuentos.Campo_Descuento_Recargos] = 0;
            Dr_Descuentos_Orden_Pago["TOTAL"] = Hdf_Costo.Value;
            Dr_Descuentos_Orden_Pago["REALIZO"] = Cls_Sessiones.Nombre_Empleado;
            Dt_Descuentos_Orden_Pago.Rows.Add(Dr_Descuentos_Orden_Pago);
            Dt_Descuentos_Orden_Pago.TableName = "Dt_Descuentos_Orden_Pago";

            Ds_Orden_Pago.Clear();
            Ds_Orden_Pago.Tables.Clear();
            Ds_Orden_Pago.Tables.Add(Dt_Orden_Pago.Copy());
            Ds_Orden_Pago.Tables.Add(Dt_Conceptos_Orden_Pago.Copy());
            Ds_Orden_Pago.Tables.Add(Dt_Contribuyente.Copy());
            Ds_Orden_Pago.Tables.Add(Dt_Descuentos_Orden_Pago.Copy());
            Generar_Reportes(Ds_Orden_Pago, Nombre_Repote_Crystal, Nombre_Reporte, Formato);

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Generar_Reportes
        ///DESCRIPCIÓN          : Prepara la información necesaria para generar el reporte
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 12/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Generar_Reportes(DataSet Ds_Rpt_Orden_Pago, String Nombre_Reporte_Crystal, String Nombre_Reporte, String Formato)
        {
            String Ruta_Reporte_Crystal = "";
            String Nombre_Reporte_Generar = "";

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Ingresos/" + Nombre_Reporte_Crystal;

            // Se crea el nombre del reporte
            String Nombre_Report = Nombre_Reporte + "_" + Session.SessionID;

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Report + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Report + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Rpt_Orden_Pago, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            //Mostrar_Reporte(Nombre_Reporte_Generar, Formato);

            Abrir_Ventana(Nombre_Reporte_Generar, "Orden_Pago");
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
        ///DESCRIPCIÓN: genera el reporte de pdf
        ///PARÁMETROS : 	
        ///         1. Ds_Reporte: Dataset con datos a imprimir
        /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
        /// 		3. Nombre_Archivo: Nombre del archivo a generar
        ///CREO       : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO  : 02-Julio-2012
        ///MODIFICO          :
        ///FECHA_MODIFICO    :
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public void Generar_Reporte(DataSet Ds_Reporte, String Extension_Archivo, String Tipo, string Ruta_Archivo_Rpt)
        {
            String Nombre_Archivo = "Reporte_Seguimiento_Solicitud_" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
            String Ruta_Archivo = @Server.MapPath(Ruta_Archivo_Rpt);//Obtiene la ruta en la cual será guardada el archivo
            ReportDocument Reporte = new ReportDocument();

            try
            {
                Reporte.Load(Ruta_Archivo);
                Reporte.SetDataSource(Ds_Reporte);

                DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //  para el tipo de archivo
                if (Extension_Archivo == "PDF")
                    Nombre_Archivo += ".pdf";
                else if (Extension_Archivo == "EXCEL")
                    Nombre_Archivo += ".xls";

                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;

                //  para el tipo de archivo
                if (Extension_Archivo == "PDF")
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                else if (Extension_Archivo == "EXCEL")
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;

                Reporte.Export(Opciones_Exportacion);

                if (Extension_Archivo == "PDF")
                    Abrir_Ventana(Nombre_Archivo, Tipo);
                else if (Extension_Archivo == "EXCEL")
                {
                    String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', '" + Tipo + "','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
        ///DESCRIPCIÓN          : Manda a pantalla el reporte cargado
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 12/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
        {
            String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

            try
            {
                if (Formato == "PDF")
                {
                    Pagina = Pagina + Nombre_Reporte_Generar;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                    "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }
                else if (Formato == "Excel")
                {
                    String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
            }
        }
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Cargar_Grid
        /// DESCRIPCION : Limpia los controles que se encuentran en la forma
        /// PARAMETROS  : Dt_Consulta la tabla con la infromacion a cargar en el grid 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 03/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Cargar_Grid_DataTable(DataTable Dt_Consulta)
    {
        try
        {
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Visible = false;

            //  para llenar el grid 
            if (Dt_Consulta is DataTable)
            {
                if (Dt_Consulta.Rows.Count > 0)
                {
                    Grid_Consulta_Tramites.Columns[0].Visible = true;
                    Grid_Consulta_Tramites.Columns[7].Visible = true;
                    Grid_Consulta_Tramites.DataSource = Dt_Consulta;
                    Grid_Consulta_Tramites.DataBind();
                    Grid_Consulta_Tramites.Columns[0].Visible = false;
                    Grid_Consulta_Tramites.Columns[7].Visible = false;

                }
                else
                {
                    Grid_Consulta_Tramites.Columns[0].Visible = true;
                    Grid_Consulta_Tramites.DataSource = Dt_Consulta;
                    Grid_Consulta_Tramites.DataBind();
                    Grid_Consulta_Tramites.Columns[0].Visible = false;
                }
            }         
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }
    
    #endregion

    #region Eventos
   
        #region Botones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
            ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  02/Mayo/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
            {
                if (!String.IsNullOrEmpty(Cls_Sessiones.Ciudadano_ID) && Div_Grid.Style.Value != "display:none")
                    Response.Redirect("../Ventanilla/Frm_Apl_Ventanilla.aspx");

                else if (Btn_Modificar.ToolTip == "Actualizar")
                {
                    Habilitar_Controles("Inicial");
                    Div_Detalles_Solicitud.Style.Value = "display:block";
                    Btn_Modificar.Visible = true;
                    Grid_Documentos_Tramite.Columns[2].Visible = false;
                }

                else if (Div_Grid.Style.Value == "display:none")
                {
                    Div_Grid.Style.Value = "display:block";
                    Habilitar_Controles("Inicial");
                    Div_Detalles_Solicitud.Style.Value = "display:none";
                    Div_Filtros.Style.Value = "display:block";
                    Btn_Generar_Reporte.Visible = false;
                    Cargar_Grid(true);
                }

                else
                {
                    Response.Redirect("../Ventanilla/Frm_Apl_Login_Ventanilla.aspx");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
            ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion
            ///PARAMETROS:     
            ///CREO: Hugo Enrique Ramírez Aguilera.
            ///FECHA_CREO: 16/Julio/2012 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Btn_Modificar_Click(object sender, EventArgs e)
            {
                String Extension = "";
                String Directorio_Portafolio = "";
                String Direccion_Archivo = "";
                String Directorio_Expediente = "";
                String Raiz = "";
                String URL = "";
                String Nombre_Archivo = "";
                String Valor_Dato = "";
                try
                {
                    if (Btn_Modificar.ToolTip == "Modificar")
                    {
                        Habilitar_Controles("Modificar"); //Habilita los controles para la introducción de datos por parte del usuario
                        Grid_Documentos_Tramite.Columns[2].Visible = true;
                    }
                    else
                    {
                        if (Cls_Sessiones.Ciudadano_ID != "")
                        {
                            DataTable Dt_Documentos = (DataTable)(Session["Grid_Documentos"]);
                            DataTable Dt_Datos = (DataTable)(Session["Grid_Datos"]);
                            String[,] Documentos = new String[Dt_Documentos.Rows.Count, 2];
                            String[,] Datos = new String[Dt_Datos.Rows.Count, 2];


                            //  para los datos de la solicitud
                            for (int Contador_For = 0; Contador_For < Dt_Datos.Rows.Count; Contador_For++)
                            {
                                Datos[Contador_For, 0] = Dt_Datos.Rows[Contador_For].ItemArray[0].ToString();

                                String Temporal = Grid_Datos.Rows[Contador_For].Cells[0].Text;

                                Valor_Dato = ((System.Web.UI.WebControls.TextBox)Grid_Datos.Rows[Contador_For].FindControl("Txt_Descripcion_Datos")).Text;

                                if (Valor_Dato != "" || (Dt_Datos.Rows[Contador_For][Cat_Tra_Datos_Tramite.Campo_Dato_Requerido].ToString()) == "N")
                                {
                                    Datos[Contador_For, 1] = Valor_Dato;
                                }
                            }

                            //  para actualizar los documentos anexados a la solicitud y al protafolio
                            for (int Contador_For = 0; Contador_For < Dt_Documentos.Rows.Count; Contador_For++)
                            {
                                Documentos[Contador_For, 0] = Dt_Documentos.Rows[Contador_For].ItemArray[0].ToString();
                                String Nombre_Documento = Dt_Documentos.Rows[Contador_For][1].ToString();
                                AsyncFileUpload AsFileUp = (AsyncFileUpload)Grid_Documentos_Tramite.Rows[Contador_For].Cells[2].FindControl("FileUp");
                                Extension = Obtener_Extension(AsFileUp.FileName);

                                if (Extension == "pdf" || Extension == "jpg" || Extension == "jpeg" || Extension == "dwg")
                                {
                                    //  para formar la dirección del archivo
                                    Directorio_Expediente = "TR-" + Hdf_Solicitud_ID.Value;
                                    Raiz = @Server.MapPath("../../Archivos");
                                    Direccion_Archivo = "";

                                    if (!Directory.Exists(Raiz))
                                        Directory.CreateDirectory(Raiz);

                                    if (AsFileUp.FileName != "")
                                        URL = AsFileUp.FileName;

                                    if (URL != "")
                                    {
                                        if (!Directory.Exists(Raiz + Directorio_Expediente))
                                            Directory.CreateDirectory(Raiz + "/" + Directorio_Expediente);

                                        //se crea el Nombre_Commando del archivo que se va a guardar
                                        Direccion_Archivo = Raiz + "/" + Directorio_Expediente +
                                            "/" + Server.HtmlEncode(Dt_Documentos.Rows[Contador_For].ItemArray[2].ToString() +
                                            "_" + Dt_Documentos.Rows[Contador_For].ItemArray[1].ToString() +
                                            "." + Extension);

                                        if (AsFileUp.HasFile)
                                        {
                                            //se guarda el archivo
                                            AsFileUp.SaveAs(Direccion_Archivo);
                                        }

                                        // se subira el archivo al portafolio*************************
                                        Directorio_Portafolio = Cls_Sessiones.Ciudadano_ID;
                                        Raiz = @Server.MapPath("../../Portafolio");
                                        URL = AsFileUp.FileName;

                                        String[] Archivos = Directory.GetFiles(MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));

                                        //verifica si existe un directorio 
                                        if (!Directory.Exists(Raiz + "/" + Directorio_Portafolio))
                                        {
                                            Directory.CreateDirectory(Raiz + "/" + Directorio_Portafolio);
                                        }

                                        //  se busca el archivo
                                        for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                                        {
                                            Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                                            if (Nombre_Archivo.Contains(Nombre_Documento))
                                            {
                                                System.IO.File.Delete(Archivos[Contador].Trim());
                                                break;
                                            }
                                        }

                                        // se subira el archivo al portafolio*************************
                                        Directorio_Portafolio = Cls_Sessiones.Ciudadano_ID;
                                        Raiz = @Server.MapPath("../../Portafolio");
                                        URL = AsFileUp.FileName;

                                        Archivos = Directory.GetFiles(MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));

                                        //verifica si existe un directorio 
                                        if (!Directory.Exists(Raiz + "/" + Directorio_Portafolio))
                                        {
                                            Directory.CreateDirectory(Raiz + "/" + Directorio_Portafolio);
                                        }

                                        //  se busca el archivo
                                        for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                                        {
                                            Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                                            if (Nombre_Archivo.Contains(Nombre_Documento))
                                            {
                                                System.IO.File.Delete(Archivos[Contador].Trim());
                                                break;
                                            }

                                        }// fin del for

                                        // ejemplo: Portafolio/0000000002/0000000003_
                                        //          Ife.jpg
                                        Direccion_Archivo = Raiz + "/" + Directorio_Portafolio + "/" + Server.HtmlEncode(Dt_Documentos.Rows[Contador_For].ItemArray[2].ToString() +
                                            "_" + Dt_Documentos.Rows[Contador_For].ItemArray[1].ToString() + "." + Extension);

                                        if (AsFileUp.HasFile)
                                        {
                                            //se guarda el archivo
                                            AsFileUp.SaveAs(Direccion_Archivo);
                                        }// fin del if (AFU_Subir_Archivo.HasFile)************************

                                    }

                                }// fin del if principal
                            }
                            Cls_Ope_Solicitud_Tramites_Negocio Negocio_Modificacion = new Cls_Ope_Solicitud_Tramites_Negocio();
                            //  para los datos de la solicitud
                            Negocio_Modificacion.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;
                            Negocio_Modificacion.P_Direccion_Predio = Txt_Direccion_Predio.Text;
                            Negocio_Modificacion.P_Propietario_Predio = Txt_Propietario_Cuenta_Predial.Text;
                            Negocio_Modificacion.P_Calle_Predio = Txt_Calle_Predio.Text;
                            Negocio_Modificacion.P_Nuemro_Predio = Txt_Numero_Predio.Text;
                            Negocio_Modificacion.P_Manzana_Predio = Txt_Manzana_Predio.Text;
                            Negocio_Modificacion.P_Lote_Predio = Txt_Lote_Predio.Text;
                            Negocio_Modificacion.P_Otros_Predio = Txt_Otros_Predio.Text;
                            Negocio_Modificacion.P_Inspector_ID = Cmb_Perito.SelectedValue;
                            Negocio_Modificacion.P_Solicitud_ID = Hdf_Solicitud_ID.Value;

                            //  para los datos 
                            Negocio_Modificacion.P_Datos = Datos;
                            Negocio_Modificacion.P_Dt_Datos = Dt_Datos;
                            Negocio_Modificacion.P_Usuario = Cls_Sessiones.Datos_Ciudadano.Rows[0][Cat_Ven_Usuarios.Campo_Email].ToString();

                            //  Para los costos del tramite
                            Negocio_Modificacion.P_Costo_Base = Hdf_Costo.Value;
                            if (String.IsNullOrEmpty(Txt_Detalle_Cantidad.Text))
                                Txt_Detalle_Cantidad.Text = "1";
                            Negocio_Modificacion.P_Cantidad = Txt_Detalle_Cantidad.Text;
                            double Cantidad_Total_Final = Convert.ToDouble(Hdf_Costo.Value) * Convert.ToDouble(Txt_Detalle_Cantidad.Text);
                            Negocio_Modificacion.P_Costo_Total = "" + Cantidad_Total_Final;

                            /**************************  metodo de modificar **************************/
                            Negocio_Modificacion.Modificar_Solicitud_Estatus_Pendiente();
                            /**************************************************************************/

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Modificar_Click", "alert('Modificacion Exitosa');", true);
                            Habilitar_Controles("Inicial");
                            Div_Detalles_Solicitud.Style.Value = "display:block";
                            Btn_Modificar.Visible = true;
                            Grid_Documentos_Tramite.Columns[2].Visible = false;
                        }
                        else
                        {
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Para poder modificar la informacion debe de acceder al porta con su cuenta, Gracias.";
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
            ///DESCRIPCIÓN: Metodo que permite acceder a la pagina de atencion ciudadana
            ///PARAMETROS:  
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  04/Mayo/2012 
            ///MODIFICO:t
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
            {
                try
                {
                    Cargar_Grid(false);

                    if (String.IsNullOrEmpty(Cls_Sessiones.Ciudadano_ID))
                    {
                        if (Grid_Consulta_Tramites.Rows.Count > 0)
                        {
                            Grid_Consulta_Tramites.SelectedIndex = 0;
                            Btn_Consulta_Grid_Selected_Click(sender, null);
                            Mostrar_Mensaje_Error(false, "");
                        }
                       
                    }
                
                }
                catch (Exception ex)
                {
                    throw new Exception("Cargar_Grid " + ex.Message.ToString());
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Link_Catastro_Click
            ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  13/Agosto/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Link_Catastro_Click(object sender, EventArgs e)
            {
                String Solicitud_ID = "";
                try
                {
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";


                }
                catch (Exception ex)
                {
                    throw new Exception("Cargar_Grid " + ex.Message.ToString());
                }


                if (Hdf_Solicitud_ID.Value != "")
                {
                    Session["Tramite_Id"] = Hdf_Solicitud_ID.Value;
                    Session["Postback_grid"] = null;
                    FormsAuthentication.Initialize();
                    String Consulta = "SELECT " + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Txt_Cuenta_Predial.Text + "'";
                    if (Txt_Detalle_Nombre.Text == "Solicitud de registro")
                    {
                        Response.Redirect("../Catastro/Frm_Ope_Cat_Recepcion_Documentos_Perito_externo.aspx");
                    }
                    else if (Txt_Detalle_Nombre.Text == "Solicitud de refrendo")
                    {
                        Response.Redirect("../Catastro/Frm_Ope_Cat_Solicitud_Refrendo.aspx");
                    }
                    else if (Txt_Detalle_Nombre.Text == "Avaluo")
                    {
                        Response.Redirect("../Catastro/Frm_Ope_Cat_Avaluo_Urbano_Perito.aspx");
                    }

                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Link_Catastro2_Click
            ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  13/Agosto/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Link_Catastro2_Click(object sender, EventArgs e)
            {
                String Solicitud_ID = "";
                try
                {
                    Img_Error.Visible = false;
                    Lbl_Mensaje_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";


                }
                catch (Exception ex)
                {
                    throw new Exception("Cargar_Grid " + ex.Message.ToString());
                }


                if (Hdf_Solicitud_ID.Value != "")
                {
                    Session["Tramite_Id"] = Hdf_Solicitud_ID.Value;
                    Session["Postback_grid"] = null;
                    FormsAuthentication.Initialize();
                    Response.Redirect("../Catastro/Frm_Ope_Cat_Avaluo_Rustico_Perito.aspx");
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Actualizar_Documento_Click
            ///DESCRIPCIÓN: permitira actualizar el archivo
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  24/Mayo/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Actualizar_Documento_Click(object sender, ImageClickEventArgs e)
            {
                int Fila = 0;
                TableCell Celda = new TableCell();
                GridViewRow Renglon;
                ImageButton Boton = new ImageButton();
                try
                {
                    Boton = (ImageButton)sender;
                    Celda = (TableCell)Boton.Parent;
                    Renglon = (GridViewRow)Celda.Parent;
                    Grid_Documentos_Tramite.SelectedIndex = Renglon.RowIndex;
                    Fila = Renglon.RowIndex;

                    ImageButton Btn_Acutalizar_Documento = (ImageButton)Grid_Documentos_Tramite.Rows[Fila].Cells[1].FindControl("Btn_Acutalizar_Documento");
                    ImageButton Btn_Ver_Documento = (ImageButton)Grid_Documentos_Tramite.Rows[Fila].Cells[1].FindControl("Btn_Ver_Documento");
                    AsyncFileUpload Afu_Subir_Archivo = (AsyncFileUpload)Grid_Documentos_Tramite.Rows[Fila].Cells[1].FindControl("FileUp");
                    System.Web.UI.WebControls.TextBox Txt_Url = (System.Web.UI.WebControls.TextBox)Grid_Documentos_Tramite.Rows[Fila].Cells[1].FindControl("Txt_Url");

                    //  se limpia la ruta del archivo
                    Txt_Url.Text = "";
                    //Txt_Url.Visible = true;
                    //  se ocultan los botones de ver y actualizar
                    Btn_Ver_Documento.Visible = false;
                    Btn_Acutalizar_Documento.Visible = false;
                    //  se muestra el boton de subir archivo
                    Afu_Subir_Archivo.Visible = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Cargar_Grid " + ex.Message.ToString());
                }
            }
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Click
            ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  13/Julio/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e)
            {
                String Telefono = "";
                Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
                DataTable Dt_Datos_Solicitud = new DataTable();
                String Cuenta_Predial_Id = "";
                String Propietario = ""; 
                string Dependencia_ID_Ordenamiento = "";
                string Dependencia_ID_Ambiental = "";
                string Dependencia_ID_Urbanistico = "";
                string Dependencia_ID_Inmobiliario = "";
                string Dependencia_ID_Catastro = "";
                String Rol_Director_Ordenamiento = "";
                try
                {
                    Obj_Parametros.Consultar_Parametros();

                    // validar que la consulta haya regresado valor
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                        Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;
                 
                    // validar que la consulta haya regresado valor
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                        Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;
                    
                    // validar que la consulta haya regresado valor
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                        Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;
                   
                    // validar que la consulta haya regresado valor
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                        Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;
                   
                    // validar que la consulta haya regresado valor
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Catastro))
                        Dependencia_ID_Catastro = Obj_Parametros.P_Dependencia_ID_Catastro;
                    
                    // validar que la consulta haya regresado valor
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                        Rol_Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;
                    

                    //  para el filtro del telefono
                    if ((Cls_Sessiones.Datos_Ciudadano != null) && (Cls_Sessiones.Datos_Ciudadano.Rows.Count > 0))
                    {
                        foreach (DataRow Registro in Cls_Sessiones.Datos_Ciudadano.Rows)
                        {
                            Telefono = Registro[Cat_Ven_Usuarios.Campo_Telefono_Casa].ToString();
                        }
                    }

                    if ((Hdf_Dependencia_ID.Value == Dependencia_ID_Catastro))
                    {
                        Generar_Reporte_Formato_Catastro(Txt_Detalle_Clave.Text, Convert.ToDateTime(Txt_Fecha_Entrega.Text), Convert.ToDateTime(Txt_Detalle_Fecha_Inicio.Text), Hdf_Propietario_Cuenta_Predial.Value, Telefono, Txt_Cuenta_Predial.Text, Cmb_Perito.SelectedValue, Hdf_Solicitud_ID.Value, Negocio_Datos_Solicitud.P_Datos_Solicitud);
                    }

                    else if (Hdf_Dependencia_ID.Value == Dependencia_ID_Ordenamiento
                          || Hdf_Dependencia_ID.Value == Dependencia_ID_Ambiental
                          || Hdf_Dependencia_ID.Value == Dependencia_ID_Inmobiliario
                          || Hdf_Dependencia_ID.Value == Dependencia_ID_Urbanistico)
                    {
                        Negocio_Datos_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        Negocio_Datos_Solicitud = Negocio_Datos_Solicitud.Consultar_Datos_Solicitud();
                        Generar_Reporte_Formato(Txt_Detalle_Clave.Text, Convert.ToDateTime(Txt_Fecha_Entrega.Text), Convert.ToDateTime(Txt_Detalle_Fecha_Inicio.Text), Hdf_Propietario_Cuenta_Predial.Value, Telefono, Txt_Cuenta_Predial.Text, Cmb_Perito.SelectedValue, Hdf_Solicitud_ID.Value, Negocio_Datos_Solicitud.P_Datos_Solicitud);

                        Cuenta_Predial_Id = Consultar_Cuenta_Predial_ID(Txt_Cuenta_Predial.Text.Trim().ToUpper());
                        // validar que la se haya obtenido una valor para la cuenta
                        if (!string.IsNullOrEmpty(Cuenta_Predial_Id))
                        {
                            Propietario = Consultar_Propietario(Cuenta_Predial_Id);
                        }
                    }
                    else
                    {
                        Propietario = Txt_Detalle_Nombre_Solicitante.Text;
                    }

                    Cls_Ope_Bandeja_Tramites_Negocio Negocio = new Cls_Ope_Bandeja_Tramites_Negocio();
                    Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                    Negocio = Negocio.Consultar_Datos_Solicitud();
                    
                    Generar_Reporte_Folio_Solicitud(Txt_Detalle_Clave.Text, Convert.ToDateTime(Txt_Fecha_Entrega.Text), Propietario, 
                            Txt_Detalle_Email.Text, Txt_Detalle_Nombre.Text, Txt_Cuenta_Predial.Text, "",
                            Negocio.P_Consecutivo, Negocio.P_Dependencia_ID,
                            Negocio.P_Area_Dependencia, Negocio.P_Folio);

                    if (Hdf_Reporte_Orden_Pago.Value.Trim().ToUpper() == "POR PAGAR")
                        Imprimir_Reporte("PDF");
                }
                catch (Exception ex)
                {
                    throw new Exception("Btn_Generar_Reporte_Click " + ex.Message.ToString(), ex);
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
                            if (Cmb_Unidad_Responsable_Filtro.Items.FindByValue(Dependencia_ID) != null)
                            {
                                Cmb_Unidad_Responsable_Filtro.SelectedValue = Dependencia_ID;
                                Btn_Buscar_Click(sender, null);
                            }
                        }
                        catch (Exception Ex)
                        {
                            throw new Exception(Ex.Message.ToString());
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
                Int32 Contador = 0;
                String Nombre_Archivo = "";
                String Nombre_Documento = "";
                String URL = String.Empty;
                int Fila = 0;
                TableCell Celda = new TableCell();
                GridViewRow Renglon;
                ImageButton Boton = new ImageButton();
                try
                {
                    if (sender != null)
                    {
                        Boton = (ImageButton)sender;
                        Celda = (TableCell)Boton.Parent;
                        Renglon = (GridViewRow)Celda.Parent;
                        Grid_Documentos_Tramite.SelectedIndex = Renglon.RowIndex;
                        Fila = Renglon.RowIndex;

                        //   se revisa que el directorio exista
                        if (!Directory.Exists((MapPath("../../Archivos/" + "TR-" + Hdf_Solicitud_ID.Value + "/"))))
                        {
                            Directory.CreateDirectory((MapPath("../../Archivos/" + "TR-" + Hdf_Solicitud_ID.Value + "/")));
                        }

                        String[] Archivos = Directory.GetFiles(MapPath("../../Archivos/" + "TR-" + Hdf_Solicitud_ID.Value + "/"));

                        Nombre_Documento = Path.GetFileName(Grid_Documentos_Tramite.Rows[Fila].Cells[1].Text.Trim());

                        //  se busca el archivo
                        for (Contador = 0; Contador < Archivos.Length; Contador++)
                        {
                            Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                            if (Nombre_Archivo.Contains(Nombre_Documento))
                            {
                                URL = Archivos[Contador].Trim();
                                Mostrar_Archivo_Anexado(URL);
                                break;
                            }

                        }// fin del for

                        if (URL != null)
                        {
                            Mostrar_Archivo_Anexado(URL);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Consulta_Grid_Selected_Click
            ///DESCRIPCIÓN: Mostrara los documentos que se requieren para el tramite
            ///PARAMETROS: 
            ///CREO:        Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO:  08/Mayo/2012
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Btn_Consulta_Grid_Selected_Click(object sender, EventArgs e)
            {
                String Clave_Solicitud = "";
                Cls_Rpt_Ven_Consultar_Tramites_Negocio Rs_Consulta = new Cls_Rpt_Ven_Consultar_Tramites_Negocio();
                Cls_Ope_Bandeja_Tramites_Negocio Negocio_Bandeja_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
                DataTable Dt_Consulta = new DataTable(); 
                DataTable Dt_Historial_Actividades = new DataTable();
                DataTable Dt_Siguiente_Actividad = new DataTable();
                DateTime Fecha = new DateTime();
                Double Costo = 0.0;
                String Cuenta_Predial_Id = "";
                String Dependencia_ID_Ordenamiento = "";
                String Dependencia_ID_Ambiental = "";
                String Dependencia_ID_Urbanistico = "";
                String Dependencia_ID_Inmobiliario = "";
                String Dependencia_ID_Catastro = ""; 
                String Rol_Director_Ordenamiento = "";
                String Estatus = "";
                int Fila = 0;
                try
                {
                    Div_Busqueda_Sin_Usuario_Registrado.Style.Value = "display: none";

                    if (!String.IsNullOrEmpty(Cls_Sessiones.Ciudadano_ID))
                    {
                        ImageButton ImageButton = (ImageButton)sender;
                        TableCell TableCell = (TableCell)ImageButton.Parent;
                        GridViewRow Row = (GridViewRow)TableCell.Parent;
                        Grid_Consulta_Tramites.SelectedIndex = Row.RowIndex;
                        Fila = Row.RowIndex;
                    }
                    else
                        Fila = Grid_Consulta_Tramites.SelectedIndex;
                   

                    Limpiar_Controles();

                    //  para los parametros de ordenamiento
                    Obj_Parametros.Consultar_Parametros();

                    // validar que la consulta haya regresado valor
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                        Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;
                    
                    // validar que la consulta haya regresado valor
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                        Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;
                 
                    // validar que la consulta haya regresado valor
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                        Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;
                  
                    // validar que la consulta haya regresado valor
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                        Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;
                   
                     // validar que la consulta haya regresado valor
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Catastro))
                        Dependencia_ID_Catastro = Obj_Parametros.P_Dependencia_ID_Catastro;
                    
                    // validar que la consulta haya regresado valor
                    if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                        Rol_Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;
                   

                    //  se ocultan los filtros
                    Div_Filtros.Style.Value = "display:none";
                    
                    GridViewRow Indice = Grid_Consulta_Tramites.Rows[Fila];
                    DataTable Dt_Pasivo = new DataTable();
                    Clave_Solicitud = HttpUtility.HtmlDecode(Indice.Cells[0].Text).ToString();

                    Clave_Solicitud = HttpUtility.HtmlDecode(Indice.Cells[0].Text).ToString();

                    Rs_Consulta.P_Solicitud_id = Clave_Solicitud.Trim();
                    Dt_Consulta = Rs_Consulta.Consultar_Tramites();

                    if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Clave_Solicitud].ToString()))
                    {
                        Rs_Consulta.P_Clave_Solicitud = Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Clave_Solicitud].ToString();
                        Dt_Pasivo = Rs_Consulta.Consultar_Datos_Pasivo();
                    }

                    Dt_Historial_Actividades = Rs_Consulta.Consultar_Historial_Actividades();

                    //  se ordenara la tabla por fecha desc
                    DataView Dv_Ordenar = new DataView(Dt_Historial_Actividades);
                    Dv_Ordenar.Sort = Ope_Tra_Det_Solicitud.Campo_Fecha + " asc " ;
                    Dt_Historial_Actividades = Dv_Ordenar.ToTable();

                    if (Dt_Consulta is DataTable)
                    {
                        if (Dt_Consulta.Rows.Count > 0)
                        {

                            Div_Detalles_Solicitud.Style.Value = "display:block";
                            Div_Grid.Style.Value = "display:none";
                            Limpiar_Controles();

                            Hdf_Dependencia_ID.Value = Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString();


                            if (Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Ordenamiento
                                || Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Ambiental
                                || Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Inmobiliario
                                || Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Urbanistico
                                || Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Catastro)
                            {
                                Div_Centa_Predial.Style.Value = "display:block";
                                //Hdf_Cuenta_Predial.Value = "Si";

                                if (Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Catastro && (Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString() == "Regularizacion" || Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString() == "Avaluo" || Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString() == "Solicitud de registro" || Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString() == "Solicitud de refrendo"))
                                {
                                    Btn_Link_Catastro.Visible = true;
                                    Btn_Link_Catastro.Enabled = true;
                                    Btn_Link_Catastro2.Visible = true;
                                    Btn_Link_Catastro2.Enabled = true;

                                    //  se asigna el nombre al link
                                    Btn_Link_Catastro.Text = "Llenar " + Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString();
                                    if (Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString() == "Avaluo")
                                    {
                                        Btn_Link_Catastro.Text += " Urbano";
                                        Btn_Link_Catastro2.Text = "Llenar Avaluo Rústico";
                                    }
                                }
                                else
                                {
                                    Btn_Link_Catastro.Visible = false;
                                    Btn_Link_Catastro2.Visible = false;
                                }

                                if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Cuenta_Predial].ToString()))
                                {
                                    Txt_Cuenta_Predial.Text = Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Cuenta_Predial].ToString();

                                    if (Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Catastro)
                                    {
                                        Btn_Buscar_Cuenta_Predial.Visible = false;
                                        Btn_Buscar_Cuenta_Predial.Enabled = false;
                                    }
                                    else
                                    {
                                        Btn_Buscar_Cuenta_Predial.Visible = true;
                                        Btn_Buscar_Cuenta_Predial.Enabled = true;
                                    }
                                    //  se saca el id de la cuenta predial
                                    Cuenta_Predial_Id = Consultar_Cuenta_Predial_ID(Txt_Cuenta_Predial.Text.Trim().ToUpper());
                                    //   se cargar la informacion del predio
                                    Cargar_Ventana_Emergente_Resumen_Predio();
                                    //  se saca el nombre del propietario de la cuentqa predial
                                    Hdf_Propietario_Cuenta_Predial.Value = Consultar_Propietario(Cuenta_Predial_Id);

                                    Txt_Cuenta_Predial_OnTextChanged(sender, null);

                                }
                                Txt_Direccion_Predio.Text = String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Direccion_Predio].ToString()) ? ""
                                    : Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Direccion_Predio].ToString();
                                Txt_Propietario_Cuenta_Predial.Text = String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Propietario_Predio].ToString()) ? ""
                                    : Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Propietario_Predio].ToString();
                                Txt_Calle_Predio.Text = String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Calle_Predio].ToString()) ? ""
                                    : Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Calle_Predio].ToString();
                                Txt_Numero_Predio.Text = String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Numero_Predio].ToString()) ? ""
                                    : Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Numero_Predio].ToString();
                                Txt_Manzana_Predio.Text = String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Manzana_Predio].ToString()) ? ""
                                    : Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Manzana_Predio].ToString();
                                Txt_Lote_Predio.Text = String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Lote_Predio].ToString()) ? ""
                                    : Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Lote_Predio].ToString();
                                Txt_Otros_Predio.Text = String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Otros_Predio].ToString()) ? ""
                                    : Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Otros_Predio].ToString();

                                if (Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString() == "Avaluo")
                                {
                                    LLenar_Combos_Catastro();
                                }
                                if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Inspector_ID].ToString()))
                                    Cmb_Perito.SelectedIndex = Cmb_Perito.Items.IndexOf(Cmb_Perito.Items.FindByValue(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Inspector_ID].ToString()));
                            }
                            //  si no pertenece a la dependencia de ordenamiento oculta los campos
                            else
                                Div_Centa_Predial.Style.Value = "display:none";

                            if (Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Dependencia_ID].ToString() == Dependencia_ID_Catastro)
                                Lbl_Detalle_Costo.Text = "Costo por Unidad";

                            else
                                Lbl_Detalle_Costo.Text = "Costo";

                            //  para la solicitud id
                            if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString()))
                            {
                                Negocio_Bandeja_Solicitud.P_Solicitud_ID = Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString();
                                Negocio_Bandeja_Solicitud = Negocio_Bandeja_Solicitud.Consultar_Datos_Solicitud();
                                Hdf_Solicitud_ID.Value = Negocio_Bandeja_Solicitud.P_Solicitud_ID;
                            }

                            //  para la clave de la solicitud
                            if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Clave_Solicitud].ToString()))
                                Txt_Detalle_Clave.Text = Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Clave_Solicitud].ToString();

                            //  para el nombre del tramite que se realiza
                            if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString()))
                                Txt_Detalle_Nombre.Text = Dt_Consulta.Rows[0][Cat_Tra_Tramites.Campo_Nombre].ToString();

                            //  para el estatus de la solicitud
                            if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Estatus].ToString()))
                            {
                                Txt_Detalle_Estatus.Text = Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Estatus].ToString();
                                Estatus = Txt_Detalle_Estatus.Text;

                                if (Txt_Detalle_Estatus.Text != "PENDIENTE")
                                    Btn_Modificar.Visible = false;

                                else
                                    Btn_Modificar.Visible = true;

                                if (Txt_Detalle_Estatus.Text == "CANCELADO" || Txt_Detalle_Estatus.Text == "DETENIDO")
                                    Btn_Copiar.Visible = true;

                                if (Txt_Detalle_Estatus.Text == "PROCESO")
                                    Btn_Generar_Reporte.Visible = true;
                            }

                            //  para el porcentaje de avance de la solicitud
                            if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Porcentaje_Avance].ToString()))
                                Txt_Detalle_Porcentaje.Text = Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Porcentaje_Avance].ToString();

                            //  para el nombre de la actividad
                            if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0]["Nombre_Actividad"].ToString()))
                            {
                                if (Estatus == "TERMINADO")
                                    Txt_Detalle_Actividad.Text = "TRAMITE TERMINADO";

                                else
                                    Txt_Detalle_Actividad.Text = (Dt_Consulta.Rows[0]["Nombre_Actividad"].ToString());
                            }

                            //  se consulta la siguiente actividad
                            Dt_Siguiente_Actividad = Negocio_Bandeja_Solicitud.Consultar_Siguiente_Actividad();
                            if (Dt_Siguiente_Actividad != null && Dt_Siguiente_Actividad.Rows.Count > 0)
                            {
                                Txt_Siguiente_Subproceso.Text = Dt_Siguiente_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Nombre].ToString();
                                Txt_Siguiente_Subproceso.Visible = true;
                                Lbl_Siguiente_Subproceso.Visible = true;
                            }
                            else
                            {
                                if (Txt_Detalle_Estatus.Text.Trim().ToUpper() != "TERMINADO")
                                {
                                    Txt_Siguiente_Subproceso.Text = "FIN DE LA SOLICITUD";
                                    Txt_Siguiente_Subproceso.Visible = true;
                                    Lbl_Siguiente_Subproceso.Visible = true;
                                }
                                else
                                {
                                    Txt_Siguiente_Subproceso.Text = "TRAMITE TERMINADO";
                                    Txt_Siguiente_Subproceso.Visible = true;
                                    Lbl_Siguiente_Subproceso.Visible = true;
                                }
                            }

                            //  para el costo de la solicitud
                            if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Costo_Total].ToString()))
                                Hdf_Costo.Value = Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Costo_Total].ToString();

                            //  para el email del solicitande
                            if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Correo_Electronico].ToString()))
                                Txt_Detalle_Email.Text = Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Correo_Electronico].ToString();

                            //  para la catidad
                            if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Cantidad].ToString()))
                                Txt_Detalle_Cantidad.Text = Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Cantidad].ToString();

                            //  para el reporte de orden de pago
                            if (Dt_Pasivo != null && Dt_Pasivo.Rows.Count > 0)
                            {
                                Costo = Calcular_Costo("", Negocio_Bandeja_Solicitud);
                                if (!String.IsNullOrEmpty(Dt_Pasivo.Rows[0][Ope_Ing_Pasivo.Campo_Estatus].ToString()))
                                    Hdf_Reporte_Orden_Pago.Value = Dt_Pasivo.Rows[0][Ope_Ing_Pasivo.Campo_Estatus].ToString();

                                if (!String.IsNullOrEmpty(Dt_Pasivo.Rows[0]["TOTAL"].ToString()))
                                    Hdf_Costo.Value = Costo.ToString();
                            }
                            //  para el nombre del solicitande
                            if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0]["Nombre_Completo"].ToString()))
                                Txt_Detalle_Nombre_Solicitante.Text = Dt_Consulta.Rows[0]["Nombre_Completo"].ToString();

                            //  para el tiempo estimado del tramite
                            if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0]["TIEMPO_ESTIMADO"].ToString()))
                                Txt_Detalle_Tiempo_Estimado.Text = Dt_Consulta.Rows[0]["TIEMPO_ESTIMADO"].ToString();

                            //  PARA la fecha de entrega

                            if (Estatus != "PENDIENTE")
                            {
                                //  para la fecha de inicio
                                if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Fecha_Entrega].ToString()))
                                    Txt_Fecha_Entrega.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Consulta.Rows[0][Ope_Tra_Solicitud.Campo_Fecha_Entrega].ToString()));

                                //  para la fecha de termino
                                if (!String.IsNullOrEmpty(Dt_Consulta.Rows[0]["Fecha_Tramite"].ToString()))
                                    Txt_Detalle_Fecha_Inicio.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Consulta.Rows[0]["Fecha_Tramite"].ToString()));
                            }
                            else
                            {
                                Txt_Fecha_Entrega.Text = "";
                                Txt_Detalle_Fecha_Inicio.Text = "";
                            }

                            //  cargar el grid de los datos
                            Cls_Ope_Solicitud_Tramites_Negocio Negocio_Solicitud = new Cls_Ope_Solicitud_Tramites_Negocio();
                            Negocio_Solicitud.P_Tramite_ID = Negocio_Bandeja_Solicitud.P_Tramite_id;
                            DataSet Ds_Documetos_Anexados = Negocio_Solicitud.Consultar_Documentos_Tramite();
                            DataTable Dt_Documetos_Anexados = Ds_Documetos_Anexados.Tables[0].Copy();

                            //  se llenan los datos ingresados por el solicitante
                            Llenar_Grid_Datos_Solicitud(Negocio_Bandeja_Solicitud.P_Datos_Solicitud);
                            //  se cargan los documentos que se ingresaron por parte del solicitante
                            Llenar_Grid_Documentacion_Solicitud(Dt_Documetos_Anexados);
                            //  se cargan los documentos creados a lo largo del tramite
                            Cargar_Documentos_Seguimiento(Negocio_Bandeja_Solicitud);
                            //  se cargara el historial de los detalles de las actividades
                            Llenar_Grid_Historial(Dt_Historial_Actividades);
                        }
                        else
                            Btn_Generar_Reporte.Visible = false;
                    }

                    Grid_Consulta_Tramites.SelectedIndex = -1;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al selecionar una dependencia. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_Copiar_Click
            ///DESCRIPCIÓN         : Se crea una nueva solicitud apartir de una existente
            ///PARAMETROS          :     
            ///CREO                : Salvador Vazquez Camacho
            ///FECHA_CREO          : 11/Julio/2012 
            ///MODIFICO            :
            ///FECHA_MODIFICO      :
            ///CAUSA_MODIFICACIÓN  :
            ///******************************************************************************* 
            protected void Btn_Copiar_Click(object sender, ImageClickEventArgs e)
            {
                Cls_Ope_Bandeja_Tramites_Negocio Negocio = new Cls_Ope_Bandeja_Tramites_Negocio();
                Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                Negocio = Negocio.Consultar_Datos_Solicitud();

                Cls_Ope_Solicitud_Tramites_Negocio Solicitud_Negocio = new Cls_Ope_Solicitud_Tramites_Negocio();
                Solicitud_Negocio.P_Tramite_ID = Negocio.P_Tramite_id;
                Solicitud_Negocio.P_Estatus = "PENDIENTE";
                Solicitud_Negocio.P_Porcentaje = "0";

                Solicitud_Negocio.P_Solicitud_ID = Negocio.P_Solicitud_ID;
                DataSet Ds_Datos = Solicitud_Negocio.Consultar_Datos_Solicitud();
                if (Ds_Datos.Tables[0].Rows.Count > 0)
                {
                    String[,] Datos = new String[Ds_Datos.Tables[0].Rows.Count, 2];
                    for (int i = 0; i < Ds_Datos.Tables[0].Rows.Count; i++)
                    {
                        Datos[i, 0] = Ds_Datos.Tables[0].Rows[i][Ope_Tra_Datos.Campo_Dato_ID].ToString();
                        Datos[i, 1] = Ds_Datos.Tables[0].Rows[i][Ope_Tra_Datos.Campo_Valor].ToString();
                    }
                    Solicitud_Negocio.P_Datos = Datos;
                }
                else
                    Solicitud_Negocio.P_Documentos = new String[0, 0];

                DataSet Ds_Documentos = Solicitud_Negocio.Consultar_Documentos_Solicitud();
                if (Ds_Documentos.Tables[0].Rows.Count > 0)
                {
                    String[,] Documentos = new String[Ds_Documentos.Tables[0].Rows.Count, 2];
                    for (int i = 0; i < Ds_Documentos.Tables[0].Rows.Count; i++)
                    {
                        Documentos[i, 0] = Ds_Documentos.Tables[0].Rows[i][Ope_Tra_Documentos.Campo_Detalle_Documento_ID].ToString();
                        Documentos[i, 1] = Ds_Documentos.Tables[0].Rows[i][Ope_Tra_Documentos.Campo_URL].ToString();
                    }
                    Solicitud_Negocio.P_Documentos = Documentos;
                }
                else
                    Solicitud_Negocio.P_Documentos = new String[0, 0];


                DataSet Ds_Solicitudes = Solicitud_Negocio.Consultar_Tramites();
                DateTime Fecha_Solucion;
                var Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                Fecha_Solucion = Dias_Inhabilies.Calcular_Fecha("" + DateTime.Today, Ds_Solicitudes.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Tiempo_Estimado].ToString());
                Solicitud_Negocio.P_Fecha_Entrega = Fecha_Solucion;

                DataSet Ds_Tramite = Solicitud_Negocio.Consultar_Tramites();

                if (Ds_Tramite.Tables.Count > 0)
                    if (Ds_Tramite.Tables[0].Rows.Count > 0)
                        Solicitud_Negocio.P_Folio = Ds_Tramite.Tables[0].Rows[0][Cat_Tra_Tramites.Campo_Clave_Tramite].ToString();



                Solicitud_Negocio.P_Clave_Solicitud = Negocio.P_Clave_Solicitud;
                Ds_Solicitudes = Solicitud_Negocio.Consultar_Solicitud();
                Solicitud_Negocio.P_Clave_Solicitud = Cls_Util.Generar_Folio_Tramite();
                Solicitud_Negocio.P_Nombre_Solicitante = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Nombre_Solicitante].ToString();
                Solicitud_Negocio.P_Apellido_Paterno = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Apellido_Paterno].ToString();
                Solicitud_Negocio.P_Apellido_Materno = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Apellido_Materno].ToString();
                Solicitud_Negocio.P_E_Mail = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Correo_Electronico].ToString();
                Solicitud_Negocio.P_Cuenta_Predial = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Cuenta_Predial].ToString();
                Solicitud_Negocio.P_Zona_ID = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Zona_ID].ToString();
                Solicitud_Negocio.P_Empleado_ID = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Empleado_ID].ToString();
                Solicitud_Negocio.P_Costo_Base = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Costo_Base].ToString();
                Solicitud_Negocio.P_Cantidad = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Cantidad].ToString();
                Solicitud_Negocio.P_Costo_Total = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Costo_Total].ToString();
                Solicitud_Negocio.P_Direccion_Predio = Ds_Solicitudes.Tables[0].Rows[0][Ope_Tra_Solicitud.Campo_Direccion_Predio].ToString();

                DataSet Ds_Subproceso = Solicitud_Negocio.Consultar_Subproceso();
                Solicitud_Negocio.P_Subproceso_ID = Ds_Subproceso.Tables[0].Rows[0][Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString();
                Negocio.P_Comentarios = "Registro generado apartir de la solicitud con clave: " + Negocio.P_Clave_Solicitud;
                Solicitud_Negocio.P_Cuenta_Predial = Negocio.P_Cuenta_Predial;
                Solicitud_Negocio.P_Perito_ID = Negocio.P_Inspector_ID;

                /**************************  metodo de alta de solicitud **************************/
                Solicitud_Negocio.Alta_Solicitud(Cls_Sessiones.Datos_Ciudadano.Rows[0][Cat_Ven_Usuarios.Campo_Email].ToString());
                /**********************************************************************************/

                Negocio.P_Solicitud_ID = Solicitud_Negocio.P_Solicitud_ID;
                Negocio.P_Subproceso_ID = Solicitud_Negocio.P_Subproceso_ID;
                Negocio.P_Estatus = Solicitud_Negocio.P_Estatus;
                Negocio.Alta_Detalles_Solicitud(Negocio);


                Cls_Ope_Solicitud_Tramites_Negocio Rs_Alta = new Cls_Ope_Solicitud_Tramites_Negocio();
                String Raiz = @Server.MapPath("../../Archivos");
                String Directorio_Expediente = "TR-" + Solicitud_Negocio.P_Solicitud_ID;

                String Nombre_Archivo = "";

                if (!Directory.Exists(Raiz + Directorio_Expediente))
                    Directory.CreateDirectory(Raiz + "/" + Directorio_Expediente);

                String[] Archivos = Directory.GetFiles(MapPath("../../Archivos/TR-" + Hdf_Solicitud_ID.Value + "/"));

                for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                {
                    Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());
                    File.Copy(Raiz + "/TR-" + Hdf_Solicitud_ID.Value + "/" + Nombre_Archivo, Raiz + "/" + Directorio_Expediente + "/" + Nombre_Archivo);
                }// fin del for

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Btn_Copiar_Click", "alert('Solicitud generada con nueva clave: " + Solicitud_Negocio.P_Clave_Solicitud + "');", true);
            }

        #endregion

    #endregion

    #region Grid
    
        ///*******************************************************************************
        /// NOMBRE:         Grid_Consulta_Tramites_OnSelectedIndexChanged
        /// DESCRIPCION :   
        /// CREO        :   Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO  :   05/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Consulta_Tramites_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Consulta_Tramites_OnRowDataBound
        ///DESCRIPCIÓN          : Evento del grid del registro que seleccionaremos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO          : 08/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        protected void Grid_Consulta_Tramites_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Color Color_Grid = Color.LightBlue;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    System.Web.UI.WebControls.Image Imagen_Estatus = (System.Web.UI.WebControls.Image)e.Row.FindControl("Btn_Consulta_Grid_Select");

                    if (e.Row.Cells[4].Text == "DETENIDO" || e.Row.Cells[4].Text == "CANCELADO")
                        Imagen_Estatus.ImageUrl = "~/paginas/imagenes/gridview/circle_red.png";

                    else if (e.Row.Cells[4].Text == "PENDIENTE")
                        Imagen_Estatus.ImageUrl = "~/paginas/imagenes/gridview/circle_grey.png";

                    else if (e.Row.Cells[4].Text == "PROCESO")
                        Imagen_Estatus.ImageUrl = "~/paginas/imagenes/gridview/circle_yellow.png";

                    else if (e.Row.Cells[4].Text == "TERMINADO" || e.Row.Cells[4].Text == "PAGADO")
                        Imagen_Estatus.ImageUrl = "~/paginas/imagenes/gridview/circle_green.png";

                    if (e.Row.Cells[7].Text == "")
                    {
                        for (int Cnt_For = 0; Cnt_For < 8; Cnt_For++)
                        {
                            e.Row.Cells[Cnt_For].BackColor = Color_Grid;
                        }
                        Imagen_Estatus.Visible = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
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
                    ImageButton Boton = (ImageButton)e.Row.Cells[3].FindControl("Btn_Ver_Documento");
                    Boton.CommandArgument = e.Row.Cells[0].Text;

                    //AsyncFileUpload Afu_Actualizar_Archivo = (AsyncFileUpload)e.Row.Cells[4].FindControl("FileUp");

                    //if (Txt_Detalle_Estatus.Text != "PENDIENTE")
                    //{
                    //    Afu_Actualizar_Archivo.Enabled = false;
                    //    Afu_Actualizar_Archivo.Visible = false;
                    //}
                    //else
                    //{
                    //    Afu_Actualizar_Archivo.Visible = true;
                    //}
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
        ///CREO:        Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO:  08/Mayo/2012 
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Datos_RowDataBound
        ///DESCRIPCIÓN: Evento de RowDataBound del Grid de Documentos creados durante el 
        ///             seguimiento.
        ///PARAMETROS:     
        ///CREO:        Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO:  08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        protected void Grid_Datos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    System.Web.UI.WebControls.TextBox Txt_Descripcion = (System.Web.UI.WebControls.TextBox)e.Row.Cells[3].FindControl("Txt_Descripcion_Datos");
                    Txt_Descripcion.Text = e.Row.Cells[4].Text;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Documentos_RowDataBound
        ///DESCRIPCIÓN          :cargara los botones dentro del grid
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO          : 24/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        protected void Grid_Documentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            String Nombre_Archivo = "";
            String Nombre_Documento = "";
            String Directorio_Portafolio = "";
            Boolean Encontrado = false;
            String Raiz = "";

            try
            {
                Directorio_Portafolio = Cls_Sessiones.Ciudadano_ID;

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AsyncFileUpload Afu_Subir_Archivo = (AsyncFileUpload)e.Row.Cells[1].FindControl("FileUp");
                    System.Web.UI.WebControls.TextBox Txt_Url = (System.Web.UI.WebControls.TextBox)e.Row.Cells[1].FindControl("Txt_Url");
                    ImageButton Btn_Acutalizar_Documento = (ImageButton)e.Row.Cells[1].FindControl("Btn_Acutalizar_Documento");
                    ImageButton Btn_Ver_Documento = (ImageButton)e.Row.Cells[1].FindControl("Btn_Ver_Documento");

                    Directorio_Portafolio = Cls_Sessiones.Ciudadano_ID;
                    Raiz = @Server.MapPath("../../Portafolio");


                    if (!Directory.Exists(Raiz))
                    {
                        Directory.CreateDirectory(Raiz);
                    }

                    if (!Directory.Exists(Raiz + "/" + Directorio_Portafolio))
                    {
                        Directory.CreateDirectory(Raiz + "/" + Directorio_Portafolio);
                    }

                    String[] Archivos = Directory.GetFiles(MapPath("../../Portafolio/" + Directorio_Portafolio + "/"));
                    Nombre_Documento = e.Row.Cells[0].Text;

                    for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                    {
                        Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                        if (Nombre_Archivo.Contains(Nombre_Documento))
                        {
                            //  se carga la ruta del archivo
                            Txt_Url.Text = Archivos[Contador].Trim();

                            Btn_Acutalizar_Documento.Visible = true;
                            Btn_Ver_Documento.Visible = true;
                            Afu_Subir_Archivo.Visible = false;
                            Txt_Url.Visible = false;
                            Encontrado = true;
                            break;
                        }

                    }// fin del for

                    if (Encontrado == false)
                    {
                        Afu_Subir_Archivo.Visible = true;
                        Txt_Url.Visible = false;
                        Btn_Acutalizar_Documento.Visible = false;
                        Btn_Ver_Documento.Visible = false;
                        Txt_Url.Visible = false;
                    }
                    else
                    {
                    }
                }

            }// fin del try
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Documento_Seguimiento_Click
        ///DESCRIPCIÓN: Se maneja el evento para ver el documento creado dentro del seguimiento.
        ///PARAMETROS:     
        ///CREO:        Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO:  08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        protected void Btn_Ver_Documento_Seguimiento_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //if (sender != null)
                //{
                //    ImageButton Boton = (ImageButton)sender;
                //    String Documento = Boton.CommandArgument;
                //    String URL = null;
                //    for (Int32 Contador = 0; Contador < Grid_Documentos_Seguimiento.Rows.Count; Contador++)
                //    {
                //        if (Grid_Documentos_Seguimiento.Rows[Contador].Cells[0].Text.Equals(Documento))
                //        {
                //            URL = Server.MapPath("../../Archivos/" + Txt_Detalle_Clave.Text.Trim() + "/" + Path.GetFileName(Grid_Documentos_Seguimiento.Rows[Contador].Cells[1].Text));
                //            break;
                //        }
                //    } if (URL != null)
                //    {
                //        Mostrar_Archivo(URL);
                //    }
                //}
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
        ///CREO:        Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO:  08/Mayo/2012 
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

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Ruta + "','Window_Archivo','left=0,top=0')", true);
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Archivo
        ///DESCRIPCIÓN: Muestra un Archivo del cual se le pasa la ruta como parametro.
        ///PARAMETROS:
        ///             1.  Ruta.  Ruta del Archivo.
        ///CREO:        Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO:  08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        public void Mostrar_Archivo_Anexado(String Ruta)
        {
            try
            {
                if (System.IO.File.Exists(Ruta))
                {
                    String Archivo = "";
                    Archivo = "../../Archivos/Tr-" + Hdf_Solicitud_ID.Value + "/" + Path.GetFileName(Ruta);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Archivo + "','Window_Archivo','left=0,top=0')", true);
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
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Mensaje_Error
        ///DESCRIPCIÓN: Muestra el mensaje de error
        ///PARAMETROS:  1.El tipo de Visible de los elementos (true o false)
        ///CREO:        Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO:  08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        public void Mostrar_Mensaje_Error(Boolean Habilitar, String Texto_Error)
        {
            try
            {
                Img_Error.Visible = Habilitar;
                Lbl_Mensaje_Error.Visible = Habilitar;
                Lbl_Mensaje_Error.Text = Texto_Error;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "');", true);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Documentos_Seguimiento
        ///DESCRIPCIÓN: Llena el Grid de Documentos de Seguimiento del  Subproceso.
        ///PARAMETROS:
        ///             1.  Solicitud.  Objeto del cual se obtienen los datos para cargar
        ///                             el Grid.
        ///CREO:        Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO:  08/Mayo/2012 
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
                }
                else
                {
                    Llenar_Grid_Documentos_Seguimiento(new DataTable());
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "')", true);
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Cuenta_Predial_ID
        /// DESCRIPCIÓN: Regresar el id de la cuenta predial, se busca mediante la cuenta predial 
        /// PARÁMETROS:
        /// 		1. Cuenta_Predial: cuenta predial a localizar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 10-jun-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Consultar_Cuenta_Predial_ID(String Cuenta_Predial)
        {
            var Consulta_Cuenta = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
            String Cuenta_Predial_ID = "";
            DataTable Dt_Resultado_Consulta;

            try
            {
                if (!String.IsNullOrEmpty(Cuenta_Predial))
                {
                    // consultar cuenta predial
                    Consulta_Cuenta.P_Cuenta_Predial = Cuenta_Predial;
                    Dt_Resultado_Consulta = Consulta_Cuenta.Consultar_Cuenta_Predial_ID();
                    if (Dt_Resultado_Consulta != null && Dt_Resultado_Consulta.Rows.Count > 0)
                    {
                        Cuenta_Predial_ID = Dt_Resultado_Consulta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Consultar_Cuenta_Predial_ID: " + ex.Message.ToString(), ex);
            }
            return Cuenta_Predial_ID;
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Documentos_Seguimiento
        ///DESCRIPCIÓN: Llena el Grid de Platillas del  Subproceso.
        ///PARAMETROS:
        ///             1.  Tabla.  DataTable con los datos con los que se va a llenar el Grid.
        ///CREO:        Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO:  08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************      
        private void Llenar_Grid_Documentos_Seguimiento(DataTable Tabla)
        {
            try
            {
                //Grid_Documentos_Seguimiento.Columns[1].Visible = true;
                //Grid_Documentos_Seguimiento.SelectedIndex = (-1);
                //if (Tabla != null)
                //{
                //    Grid_Documentos_Seguimiento.DataSource = Tabla;
                //}
                //else
                //{
                //    Grid_Documentos_Seguimiento.DataSource = new DataTable();
                //}
                //Grid_Documentos_Seguimiento.DataBind();
                //Grid_Documentos_Seguimiento.Columns[1].Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

    #endregion

    #region TextBox
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Clave_Solicitud_TextChanged
        ///DESCRIPCIÓN: 
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  19/Septiembre/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Txt_Clave_Solicitud_TextChanged(object sender, EventArgs e)
        {
            Mostrar_Mensaje_Error(false, "");

            try
            {
                if (Txt_Clave_Solicitud.Text.Length == 12)
                    Btn_Buscar_Click(sender, null);
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message;
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Cuenta_Predial_OnTextChanged
        ///DESCRIPCIÓN: habilitara el boton de busqueda de cuenta predial
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  09/Julio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Txt_Cuenta_Predial_OnTextChanged(object sender, EventArgs e)
        {
            string Cuenta_Predial_Id = "";

            // limpiar mensaje de error
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            try
            {
                // si el campo cuenta predial contiene texto, buscar el id de la cuenta
                if (Txt_Cuenta_Predial.Text.Length > 0)
                {
                    // llamar al método que consulta la cuenta predial id con la cuenta ingresada
                    Cuenta_Predial_Id = Consultar_Cuenta_Predial_ID(Txt_Cuenta_Predial.Text.Trim().ToUpper());
                    // validar que la se haya obtenido una valor para la cuenta
                    if (!string.IsNullOrEmpty(Cuenta_Predial_Id))
                    {
                        Txt_Propietario_Cuenta_Predial.Text = Consultar_Propietario(Cuenta_Predial_Id);
                    }
                    else
                    {
                        // mostrar mensaje indicando que no se encontró la cuenta
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "La cuenta proporcionada no se encuentra en el sistema.<br /><br />";
                    }
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message;
            }
        }
       
        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Propietario
        /// DESCRIPCIÓN: Regresar el nombre del propietario de la cuenta predial con el id proporcionado
        /// PARÁMETROS:
        /// 		1. Cuenta_Predial_ID: id de la cuenta predial a consultar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 10-jun-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private String Consultar_Propietario(String Cuenta_Predial_ID)
        {
            String Propietario = "";
            DataTable Dt_Resultado_Consulta;
            var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();

            try
            {
                if (!String.IsNullOrEmpty(Cuenta_Predial_ID))
                {
                    // consultar cuenta predial
                    Consulta_Propietario_Negocio.P_Cuenta_Predial_Id = Cuenta_Predial_ID;
                    Dt_Resultado_Consulta = Consulta_Propietario_Negocio.Consultar_Propietario();
                    if (Dt_Resultado_Consulta != null && Dt_Resultado_Consulta.Rows.Count > 0)
                    {
                        Propietario = Dt_Resultado_Consulta.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message;
            }
            return Propietario;
        }

    #endregion

    #region Combo
     ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Condicion_SelectedIndexChanged
    ///DESCRIPCIÓN: Manejo del evento cambio de índice en el combo condicion, consultar el nombre 
    ///             de la actividad a la que procede
    ///PARÁMETROS:
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 27-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
        protected void Cmb_Unidad_Responsable_Filtro_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Cmb_Unidad_Responsable_Filtro.SelectedIndex > 0)
                {
                    Btn_Buscar_Click(sender, null);
                }
                else
                {
                    Btn_Buscar_Click(sender, null);
                }
            }

            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }

        }
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Cmb_Estatus_SelectedIndexChanged
        ///DESCRIPCIÓN: Manejo del evento cambio de índice en el combo condicion, consultar el nombre 
        ///             de la actividad a la que procede
        ///PARÁMETROS:
        ///CREO: Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO: 27-jul-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        protected void Cmb_Estatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Cmb_Estatus.SelectedIndex > 0)
                {
                    Btn_Buscar_Click(sender, null);
                }
                else
                {
                    Cargar_Grid(true);
                }
            }

            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }

        }
    #endregion
}
