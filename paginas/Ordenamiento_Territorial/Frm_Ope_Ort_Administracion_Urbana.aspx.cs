using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Negocio;
using Presidencia.Orden_Territorial_Administracion_Urbana.Negocio;
using Presidencia.Orden_Territorial_Formato_Ficha_Inspeccion.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;
using System.Net.Mail;
using Presidencia.Catalogo_Tramites_Parametros.Negocio;
using Presidencia.Registro_Peticion.Datos;
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;
using Presidencia.Catalogo_Tramites.Negocio;
using Presidencia.Solicitud_Tramites.Negocios;
using Presidencia.Orden_Territorial_Bitacora_Documentos.Negocio;

public partial class paginas_Ordenamiento_Territorial_Frm_Ope_Ort_Administracion_Urbana : System.Web.UI.Page
{
    #region Page load
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Page_Load
        /// DESCRIPCION : 
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 01/Junio/2012
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
                    Inicializar_Controles();

                    if (Request.QueryString["Solicitud"] != null || (Session["Cedula_Solicitud_Id"] != null && Session["Cedula_Solicitud_Id"].ToString() != ""))
                    {
                        if (Request.QueryString["Solicitud"] != null)
                        {
                            //  proviene de la ficha de revision
                            Hdf_Solicitud_ID.Value = Request.QueryString["Solicitud"];
                            Session.Remove("Solicitud");
                        }
                        else
                        {
                            //  proviene de la bandeja de tramites
                            Hdf_Solicitud_ID.Value = Session["Cedula_Solicitud_Id"].ToString();
                            Cls_Sessiones.Ciudadano_ID = Hdf_Solicitud_ID.Value;
                            Session.Remove("Cedula_Solicitud_Id");
                        }
                        
                        Hdf_Redireccionar.Value = "SI";
                        Mostrar_Datos_Ficha();
                    }
                    else if (Session["Solicitud_Id"] != null && Session["Solicitud_Id"].ToString() != "")
                    {
                        Llenar_Solicitud();
                        Hdf_Redireccionar.Value = "SI";
                    }
                }

                // registro de scripts del lado del servidor para mostrar ventanas emergentes para búsqueda avanzada
                string Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Calle.Attributes.Add("onclick", Ventana_Modal);
                Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Colonia.Attributes.Add("onclick", Ventana_Modal);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

    #endregion

    #region Metodos generales
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Inicializa_Controles
        /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
        ///               realizar diferentes operaciones
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 01/Junio/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Inicializar_Controles()
        {
            try
            {
                Limpiar_Controles();
                Habilitar_Controles("Inicial");

                Consultar_Llenado_Formatos(Consultar_Formato_ID());
                Llenar_Grid_Listado(0);
                Cargar_Combo_Zona();
                Cargar_Combo_Tipo_Supervision();
                Cargar_Combo_Condiciones_Inmueble();
                Cargar_Combo_Condiciones_Avance();
                //Cargar_Combo_Areas_Donacion();
                Cargar_Combo_Areas_Uso_Actual();
                Cargar_Combo_Funcionamiento();
                Cargar_Combo_Materiales();
                Cargar_Combo_Inspectores();
                Cargar_Combo_Tipo_Residuos();
                Cargar_Combo_Colonias();
                Cargar_Combo_Calles(new DataTable());
                
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
        /// FECHA_CREO  : 01/Junio/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Limpiar_Controles()
        {
            try
            {
                //  para los campos ocultos
                Hdf_Administracion_Urbana_ID.Value = "";
                Hdf_Tramite_ID.Value = "";
                Hdf_Solicitud_ID.Value = "";
                Hdf_Subproceso_ID.Value = "";

                //  radio butons
                RBtn_Acta_Inspeccion.SelectedIndex = -1;
                RBtn_Agua_Potable.SelectedIndex = -1;
                RBtn_Agua_Potable_Drenaje.SelectedIndex = -1;
                RBtn_Agua_Potable_Fosa_Septica.SelectedIndex = -1;
                RBtn_Agua_Potable_Japami.SelectedIndex = -1;
                RBtn_Agua_Potable_Particular.SelectedIndex = -1;
                RBtn_Area_Inspeccion.SelectedIndex = -1;
                RBtn_Avance_Obra_Acorde_Solicitado.SelectedIndex = -1;
                RBtn_Clausurado.SelectedIndex = -1;
                RBtn_Cuenta_Estacionamiento.SelectedIndex = -1;
                RBtn_Cuenta_Estacionamiento_Area_Carga.SelectedIndex = -1;
                RBtn_Cuenta_Estacionamiento_Dentro_Inmueble.SelectedIndex = -1;
                RBtn_Cuenta_Estacionamiento_Rentado.SelectedIndex = -1;
                RBtn_Lavabo.SelectedIndex = -1;
                RBtn_Letrina.SelectedIndex = -1;
                RBtn_Medidas_Seguridad_Equipo.SelectedIndex = -1;
                RBtn_Medidas_Seguridad_Material_Flamable.SelectedIndex = -1;
                RBtn_Medidas_Seguridad_Senalizacion.SelectedIndex = -1;
                RBtn_Mixto.SelectedIndex = -1;
                RBtn_Multado.SelectedIndex = -1;
                RBtn_Notificado.SelectedIndex = -1;
                RBtn_Servicios_Sanitarios.SelectedIndex = -1;
                RBtn_Uso_Diferente_Adicional.SelectedIndex = -1;
                RBtn_Wc.SelectedIndex = -1; 

                //  para los textbox
                Txt_Acta_Inspeccion_Folio.Text = "";
                Txt_Afluencia.Text = "";
                Txt_Arbol_Altura.Text = "";
                Txt_Arbol_Diametro.Text = "";
                Txt_Arbol_Diametro_Fronda.Text = "";
                Txt_Arbol_Estado.Text = "";
                Txt_Area_Acticidad.Text = "";
                Txt_Avance_Obra_Aproximado.Text = "";
                Txt_Avance_Obra_Niveles_Actuales.Text = "";
                Txt_Avance_Obra_Niveles_Construir.Text = "";
                Txt_Bano_Hombres.Text = "";
                Txt_Bano_Mujeres.Text = "";
                //Txt_Calle.Text = "";
                Txt_Cantidad_Personas_Laboran.Text = "";
                Txt_Multa_Folio.Text = "";
                Txt_Clausurado_Folio.Text = "";
                //Txt_Colonia.Text = "";
                Txt_Consecutivo_ID.Text = "";
                Txt_Destinado.Text = "";
                Cmb_Evaluacion.SelectedIndex = -1;
                Txt_Dimenciones_Anuncion_1.Text = "";
                Txt_Dimenciones_Anuncion_2.Text = "";
                Txt_Dimenciones_Anuncion_3.Text = "";
                Txt_Dimenciones_Anuncion_4.Text = "";
                Txt_Usos_Cercanos_Riesgo.Text = "";
                Txt_Domicilio_Estacionamiento.Text = "";
                Txt_Especificacion_Restriccion.Text = "";
                Txt_Especificar_Tipo_Uso.Text = "";
                Txt_Generales_Observaciones_Del_Inspector.Text = "";
                Txt_Generales_Observaciones_Para_Inspector.Text = "";
                Txt_Generales_Recepcion_Campo_Fecha.Text = "";
                Txt_Generales_Recepcion_Coordinador_Fecha.Text = "";
                Txt_Generales_Recepcion_Inspector_Fecha.Text = "";
                Txt_Lote.Text = "";
                Txt_Maquinaria_Utilizar.Text = "";
                Txt_Medidas_Seguridad_Especificar.Text = "";
                Txt_Notificacion_Folio.Text = "";
                Txt_Numero_Cajones.Text = "";
                Txt_Numero_Fisico.Text = "";
                Txt_Superficie_Metros2.Text = "";
                Txt_Tipo_Anuncio_1.Text = "";
                Txt_Tipo_Anuncio_2.Text = "";
                Txt_Tipo_Anuncio_3.Text = "";
                Txt_Tipo_Anuncio_4.Text = "";
                Txt_Total_Banos.Text = "";
                Txt_Usos_Colindantes.Text = "";
                Txt_Usos_Frente_Inmueble.Text = "";
                Txt_Manzana.Text = "";

                Txt_Licencia_Equipo_Emisor.Text = "";
                Txt_Licencia_Gastos_Combustible.Text = "";
                Txt_Licencia_Hora_Funcionamiento.Text = "";
                Txt_Licencia_Tipo_Conbustible.Text = "";
                Txt_Licencia_Tipo_Emision.Text = "";
                Txt_Manifiesto_Afectacion.Text = "";
                Txt_Manifiesto_Colindancia.Text = "";
                Txt_Manifiesto_Superficie_Total.Text = "";
                Txt_Manifiesto_Tipo_Proyecto.Text = "";
                //Txt_Materiales_Accesibilidad.Text = "";
                Txt_Materiales_Flora.Text = "";
                Txt_Materiales_Inclinacion.Text = "";
                Txt_Materiales_Petreo.Text = "";
                Txt_Materiales_Superficie_Total.Text = "";

                Chk_Dias_Labor_Jueves.Checked = false;
                Chk_Dias_Labor_Lunes.Checked = false;
                Chk_Dias_Labor_Martes.Checked = false;
                Chk_Dias_Labor_Miercoles.Checked = false;
                Chk_Dias_Labor_Viernes.Checked = false;
                Chk_Construccion_Marquesina.Checked = false;
                Chk_Invasion_Areas_donacion.Checked = false;
                Chk_Invasion_Material.Checked = false;
                Chk_Sobresale_Paramento.Checked = false;

                //  para los RadioButtonList
                RBtn_Aprovechamiento_Existe_Separacion.SelectedIndex = -1;
                RBtn_Aprovechamiento_Revuelven_Liquidos.SelectedIndex = -1;
                RBtn_Aprovechamiento_Uso_Suelo.SelectedIndex = -1;
                RBtn_Material_Permiso_Ecologia.SelectedIndex = -1;
                RBtn_Material_Permiso_Suelo.SelectedIndex = -1;
                RBtn_Aprovechamiento_Almacen_Residuos.SelectedIndex = -1;
                RBtn_Material_Accesibilidad_Vehiculo.SelectedIndex = -1;

                Session.Remove("Dt_Tipo_Residuo");


                Hdf_Redireccionar.Value = "";

                Grid_Tipos_Residuos.DataSource = null;
                Grid_Tipos_Residuos.DataBind();

            }
            catch (Exception ex)
            {
                throw new Exception("Limpia_Controles " + ex.Message);
            }
        }
        
        ///*******************************************************************************
        /// NOMBRE:         Habilitar_Controles
        /// DESCRIPCION :   Habilita y Deshabilita los controles de la forma para prepara la página
        ///                 para a siguiente operación
        /// PARAMETROS:     1.- Operacion: Indica la operación que se desea realizar 
        /// CREO:           Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO:     01/Junio/2012
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
                        Btn_Nuevo.ToolTip = "Nuevo";
                        Btn_Salir.ToolTip = "Salir";
                        Btn_Nuevo.Visible = false;
                        Btn_Nuevo.CausesValidation = false;
                        Btn_Modificar.Visible = false;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Modificar.AlternateText = "Modificar";
                        Div_Lista_Formatos.Style.Value = "display:block";
                        Div_Principal_Llenado_Formato.Style.Value = "display:none";
                        break;
                    case "Nuevo":
                        Habilitado = true;
                        Btn_Nuevo.ToolTip = "Dar de Alta";
                        Btn_Salir.ToolTip = "Cancelar";
                        Btn_Nuevo.Visible = true;
                        Btn_Nuevo.CausesValidation = true;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                        break;
                    case "Modificar":
                        Habilitado = true;
                        Grid_Tipos_Residuos.Columns[3].Visible = true;
                        Btn_Agregar_Tipo_Residuo.Visible = true;
                        Btn_Salir.ToolTip = "Cancelar";
                        break;
                }
                //  mensajes de error
               Mostrar_Mensaje_Error(false);

                //  radio butons
                RBtn_Acta_Inspeccion.Enabled = Habilitado;
                RBtn_Agua_Potable.Enabled = Habilitado;
                RBtn_Agua_Potable_Drenaje.Enabled = Habilitado;
                RBtn_Agua_Potable_Fosa_Septica.Enabled = Habilitado;
                RBtn_Agua_Potable_Japami.Enabled = Habilitado;
                RBtn_Agua_Potable_Particular.Enabled = Habilitado;
                RBtn_Area_Inspeccion.Enabled = Habilitado;
                RBtn_Avance_Obra_Acorde_Solicitado.Enabled = Habilitado;
                RBtn_Clausurado.Enabled = Habilitado;
                RBtn_Cuenta_Estacionamiento.Enabled = Habilitado;
                RBtn_Cuenta_Estacionamiento_Area_Carga.Enabled = Habilitado;
                RBtn_Cuenta_Estacionamiento_Dentro_Inmueble.Enabled = Habilitado;
                RBtn_Cuenta_Estacionamiento_Rentado.Enabled = Habilitado;
                RBtn_Lavabo.Enabled = Habilitado;
                RBtn_Letrina.Enabled = Habilitado;
                RBtn_Medidas_Seguridad_Equipo.Enabled = Habilitado;
                RBtn_Medidas_Seguridad_Material_Flamable.Enabled = Habilitado;
                RBtn_Medidas_Seguridad_Senalizacion.Enabled = Habilitado;
                RBtn_Mixto.Enabled = Habilitado;
                RBtn_Multado.Enabled = Habilitado;
                RBtn_Notificado.Enabled = Habilitado;
                RBtn_Servicios_Sanitarios.Enabled = Habilitado;
                RBtn_Uso_Diferente_Adicional.Enabled = Habilitado;
                RBtn_Wc.Enabled = Habilitado;
                RBtn_Material_Permiso_Ecologia.Enabled = Habilitado;
                RBtn_Material_Permiso_Suelo.Enabled = Habilitado;
                RBtn_Material_Accesibilidad_Vehiculo.Enabled = Habilitado;
                RBtn_Aprovechamiento_Uso_Suelo.Enabled = Habilitado;
                RBtn_Aprovechamiento_Almacen_Residuos.Enabled = Habilitado;
                RBtn_Aprovechamiento_Existe_Separacion.Enabled = Habilitado;
                RBtn_Aprovechamiento_Revuelven_Liquidos.Enabled = Habilitado;
                RBtn_Arboles_Tipo_Poda.Enabled = Habilitado;
                RBtn_Arboles_Tipo_Tala.Enabled = Habilitado;
                RBtn_Arboles_Tipo_Trasplante.Enabled = Habilitado;

                //  para los textbox
                Txt_Acta_Inspeccion_Folio.Enabled = Habilitado;
                Txt_Afluencia.Enabled = Habilitado;
                Txt_Arbol_Altura.Enabled = Habilitado;
                Txt_Arbol_Diametro.Enabled = Habilitado;
                Txt_Arbol_Diametro_Fronda.Enabled = Habilitado;
                Txt_Arbol_Estado.Enabled = Habilitado;
                Txt_Area_Acticidad.Enabled = Habilitado;
                Txt_Avance_Obra_Aproximado.Enabled = Habilitado;
                Txt_Avance_Obra_Niveles_Actuales.Enabled = Habilitado;
                Txt_Avance_Obra_Niveles_Construir.Enabled = Habilitado;
                Txt_Bano_Hombres.Enabled = Habilitado;
                Txt_Bano_Mujeres.Enabled = Habilitado;
                Cmb_Calle.Enabled = Habilitado;
                Txt_Cantidad_Personas_Laboran.Enabled = Habilitado;
                Txt_Multa_Folio.Enabled = Habilitado;
                Txt_Clausurado_Folio.Enabled = Habilitado;
                Cmb_Colonias.Enabled = Habilitado;
                Txt_Consecutivo_ID.Enabled = false;
                Txt_Destinado.Enabled = Habilitado;
                Cmb_Evaluacion.Enabled = Habilitado;
                Txt_Dimenciones_Anuncion_1.Enabled = Habilitado;
                Txt_Dimenciones_Anuncion_2.Enabled = Habilitado;
                Txt_Dimenciones_Anuncion_3.Enabled = Habilitado;
                Txt_Dimenciones_Anuncion_4.Enabled = Habilitado;
                Txt_Usos_Cercanos_Riesgo.Enabled = Habilitado;
                Txt_Domicilio_Estacionamiento.Enabled = Habilitado;
                Txt_Especificacion_Restriccion.Enabled = Habilitado;
                Txt_Especificar_Tipo_Uso.Enabled = Habilitado;
                Txt_Generales_Observaciones_Del_Inspector.Enabled = Habilitado;
                Txt_Generales_Observaciones_Para_Inspector.Enabled = Habilitado;
                Txt_Generales_Recepcion_Campo_Fecha.Enabled = false;
                Txt_Generales_Recepcion_Coordinador_Fecha.Enabled = false;
                Txt_Generales_Recepcion_Inspector_Fecha.Enabled = false;
                Txt_Lote.Enabled = Habilitado;
                Txt_Maquinaria_Utilizar.Enabled = Habilitado;
                Txt_Medidas_Seguridad_Especificar.Enabled = Habilitado;
                Txt_Notificacion_Folio.Enabled = Habilitado;
                Txt_Numero_Cajones.Enabled = Habilitado;
                Txt_Numero_Fisico.Enabled = Habilitado;
                Txt_Superficie_Metros2.Enabled = Habilitado;
                Txt_Tipo_Anuncio_1.Enabled = Habilitado;
                Txt_Tipo_Anuncio_2.Enabled = Habilitado;
                Txt_Tipo_Anuncio_3.Enabled = Habilitado;
                Txt_Tipo_Anuncio_4.Enabled = Habilitado;
                Txt_Total_Banos.Enabled = false;
                Txt_Usos_Colindantes.Enabled = Habilitado;
                Txt_Usos_Frente_Inmueble.Enabled = Habilitado;
                Txt_Manzana.Enabled = Habilitado;
                Txt_Cuenta_Predial.Enabled = false;
                Txt_Materiales_Superficie_Total.Enabled = Habilitado;
                Txt_Materiales_Profundidad.Enabled = Habilitado;
                Txt_Materiales_Inclinacion.Enabled = Habilitado;
                Txt_Materiales_Flora.Enabled = Habilitado;
                Txt_Materiales_Petreo.Enabled = Habilitado;
                Txt_Manifiesto_Afectacion.Enabled = Habilitado;
                Txt_Manifiesto_Colindancia.Enabled = Habilitado;
                Txt_Manifiesto_Superficie_Total.Enabled = Habilitado;
                Txt_Manifiesto_Tipo_Proyecto.Enabled = Habilitado;
                Txt_Licencia_Equipo_Emisor.Enabled = Habilitado;
                Txt_Licencia_Gastos_Combustible.Enabled = Habilitado;
                Txt_Licencia_Hora_Funcionamiento.Enabled = Habilitado;
                Txt_Licencia_Tipo_Conbustible.Enabled = Habilitado;
                Txt_Licencia_Tipo_Emision.Enabled = Habilitado;
                Txt_Aprovechamiento_Emisiones_Atmosfera.Enabled = Habilitado;
                Txt_Aprovechamiento_Horario_Final.Enabled = Habilitado;
                Txt_Aprovechamiento_Horario_Inicial.Enabled = Habilitado;
                Txt_Aprovechamiento_Metodo_Sepearacion.Enabled = Habilitado;
                Txt_Aprovechamiento_Nivel_Ruido.Enabled = Habilitado;
                Txt_Aprovechamiento_Servicio_Recoleccion.Enabled = Habilitado;
                Txt_Aprovechamiento_Tipo_Contenedor.Enabled = Habilitado;
                Txt_Aprovechamiento_Tipo_Ruido.Enabled = Habilitado;
                Txt_Arboles_Especie.Enabled = Habilitado;
                Txt_Arboles_Cantidad_Poda.Enabled = Habilitado;
                Txt_Arboles_Cantidad_Tala.Enabled = Habilitado;
                Txt_Arboles_Cantidad_Trasplante.Enabled = Habilitado;
                Txt_Licencia_Almacenaje_Combustible.Enabled = Habilitado;
                Txt_Licencia_Cantidad_Combustible.Enabled = Habilitado;
                
                //  para los combos
                Cmb_Avance_Obra.Enabled = Habilitado;
                Cmb_Condiciones_Inmueble.Enabled = Habilitado;
                Cmb_Funcionamiento.Enabled = Habilitado;
                Cmb_Material_Muros.Enabled = Habilitado;
                Cmb_Material_Techo.Enabled = Habilitado;
                Cmb_Tipo_Supervision.Enabled = Habilitado;
                Cmb_Uso_Actual.Enabled = Habilitado;
                //Cmb_Via_Publica_Area_Donacion.Enabled = Habilitado;
                Cmb_Zona.Enabled = false;
                Cmb_Calle.Enabled = Habilitado;
                Cmb_Colonias.Enabled = Habilitado;
                Cmb_Inspector.Enabled = false;
                Cmb_Tipo_Residuo.Enabled = Habilitado;
                Chk_Dias_Labor_Jueves.Enabled = Habilitado;
                Chk_Dias_Labor_Lunes.Enabled = Habilitado;
                Chk_Dias_Labor_Martes.Enabled = Habilitado;
                Chk_Dias_Labor_Miercoles.Enabled = Habilitado;
                Chk_Dias_Labor_Viernes.Enabled = Habilitado;
                Chk_Dias_Labor_Sabado.Enabled = Habilitado;
                Chk_Dias_Labor_Domingo.Enabled = Habilitado;

                //  para los botones
                Btn_Generales_Recepcion_Campo_Fecha.Enabled = Habilitado;
                Btn_Generales_Recepcion_Coordinador_Fecha.Enabled = Habilitado;
                Btn_Generales_Recepcion_Inspector_Fecha.Enabled = Habilitado;
            }
            catch (Exception ex)
            {
                throw new Exception("Habilitar_Controles " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Mostrar_Mensaje_Error
        ///DESCRIPCIÓN          : se habilitan los mensajes de error
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Mostrar_Mensaje_Error(Boolean Accion)
        {
            try
            {
                Img_Error.Visible = Accion;
                Lbl_Mensaje_Error.Visible = Accion;
            }
            catch (Exception ex)
            {
                throw new Exception("Mostrar_Mensaje_Error " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Colonias
        ///DESCRIPCIÓN: cargara la informacion de las calles y colonias
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cargar_Combo_Colonias()
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            DataTable Dt_Colonias = new DataTable();
            try
            {
                //  para las colonias
                Dt_Colonias = Negocio_Consulta.Consultar_Colonia();

                Cmb_Colonias.DataSource = Dt_Colonias;
                Cmb_Colonias.DataValueField = Cat_Ate_Colonias.Campo_Colonia_ID;
                Cmb_Colonias.DataTextField = Cat_Ate_Colonias.Campo_Nombre;
                Cmb_Colonias.DataBind();
                Cmb_Colonias.Items.Insert(0, "< SELECCIONE >");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Calles
        ///DESCRIPCIÓN: cargara la informacion de las calles pertenecientes a una colonia
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  31/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cargar_Combo_Calles(DataTable Dt_Calles)
        {
            try
            {
                Cmb_Calle.DataSource = Dt_Calles;
                Cmb_Calle.DataValueField = Cat_Pre_Calles.Campo_Calle_ID;
                Cmb_Calle.DataTextField = Cat_Pre_Calles.Campo_Nombre;
                Cmb_Calle.DataBind();
                Cmb_Calle.Items.Insert(0, "< SELECCIONE >");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Llenar_Combo_Con_DataTable
        ///DESCRIPCIÓN: Asigna los valores de la tabla en el combo recibidos como parámetros
        ///PARÁMETROS:
        /// 		1. Obj_Combo: control al que se van a asignar los datos en la tabla
        /// 		2. Dt_Temporal: tabla con los datos a mostrar en el control Obj_Combo
        /// 		3. Indice_Campo_Valor: entero con el número de columna de la tabla con el valor para el combo
        /// 		4. Indice_Campo_Texto: entero con el número de columna de la tabla con el texto para el combo
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17-may-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public void Llenar_Combo_Con_DataTable(DropDownList Obj_Combo, DataTable Dt_Temporal, int Indice_Campo_Valor, int Indice_Campo_Texto)
        {
            Obj_Combo.Items.Clear();
            Obj_Combo.SelectedValue = null;
            Obj_Combo.DataSource = Dt_Temporal;
            Obj_Combo.DataTextField = Dt_Temporal.Columns[Indice_Campo_Texto].ToString();
            Obj_Combo.DataValueField = Dt_Temporal.Columns[Indice_Campo_Valor].ToString();
            Obj_Combo.DataBind();
            Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
            Obj_Combo.SelectedIndex = 0;
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Residuos
        ///DESCRIPCIÓN          : se llena el grid con el tipo de residuo que se selecciona
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 06/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Grid_Residuos(DataTable Dt_Resiudo)
        {
            try
            {
                Grid_Tipos_Residuos.Columns[0].Visible = true;
                Grid_Tipos_Residuos.Columns[1].Visible = true;
                Grid_Tipos_Residuos.DataSource = Dt_Resiudo;
                Grid_Tipos_Residuos.DataBind();
                Grid_Tipos_Residuos.Columns[1].Visible = false;
                Grid_Tipos_Residuos.Columns[0].Visible = false;
                Session["Dt_Tipo_Residuo"] = Dt_Resiudo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Zona
        ///DESCRIPCIÓN          : se cargara el combo con las zonas
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 04/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Cargar_Combo_Zona()
        {
            Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Consulta_Zona = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Dt_Consulta = Negocio_Consulta_Zona.Consultar_Zonas();

                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    Cmb_Zona.DataSource = Dt_Consulta;
                    Cmb_Zona.DataValueField = Cat_Ort_Zona.Campo_Zona_ID;
                    Cmb_Zona.DataTextField = Cat_Ort_Zona.Campo_Nombre;
                    Cmb_Zona.DataBind();
                    Cmb_Zona.Items.Insert(0, "< SELECCIONE >");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cargar_Combo_Zona " + ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Tipo_Supervision
        ///DESCRIPCIÓN          : se cargara el combo los tipos de supervision
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 04/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Cargar_Combo_Tipo_Supervision()
        {
            Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Consulta_Tipo_Supervision = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Dt_Consulta = Negocio_Consulta_Tipo_Supervision.Consultar_Tipo_Supervision();

                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    Cmb_Tipo_Supervision.DataSource = Dt_Consulta;
                    Cmb_Tipo_Supervision.DataValueField = Cat_Ort_Tipo_Supervision.Campo_Tipo_Supervision_ID;
                    Cmb_Tipo_Supervision.DataTextField = Cat_Ort_Tipo_Supervision.Campo_Nombre;
                    Cmb_Tipo_Supervision.DataBind();
                    Cmb_Tipo_Supervision.Items.Insert(0, "< SELECCIONE >");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cargar_Combo_Tipo_Supervision " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Condiciones_Inmueble
        ///DESCRIPCIÓN          : se cargara el combo con los distintos tipos de condiciones del inmueble
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 04/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Cargar_Combo_Condiciones_Inmueble()
        {
            Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Consulta_Condicion_Inmuble = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Dt_Consulta = Negocio_Consulta_Condicion_Inmuble.Consultar_Condiciones_Inmueble();

                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    Cmb_Condiciones_Inmueble.DataSource = Dt_Consulta;
                    Cmb_Condiciones_Inmueble.DataValueField = Cat_Ort_Condi_Inmueble.Campo_Condicion_Inmueble_ID;
                    Cmb_Condiciones_Inmueble.DataTextField = Cat_Ort_Condi_Inmueble.Campo_Nombre;
                    Cmb_Condiciones_Inmueble.DataBind();
                    Cmb_Condiciones_Inmueble.Items.Insert(0, "< SELECCIONE >");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cargar_Combo_Condiciones_Inmueble " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Condiciones_Avance
        ///DESCRIPCIÓN          : se cargara el combo con los distintos tipos de avances
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 04/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Cargar_Combo_Condiciones_Avance()
        {
            Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Consulta_Avance = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Dt_Consulta = Negocio_Consulta_Avance.Consultar_Avance_Obra();

                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    Cmb_Avance_Obra.DataSource = Dt_Consulta;
                    Cmb_Avance_Obra.DataValueField = Cat_Ort_Avance_Obra.Campo_Avance_Obra_ID;
                    Cmb_Avance_Obra.DataTextField = Cat_Ort_Avance_Obra.Campo_Nombre;
                    Cmb_Avance_Obra.DataBind();
                    Cmb_Avance_Obra.Items.Insert(0, "< SELECCIONE >");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cargar_Combo_Condiciones_Avance " + ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Areas_Donacion
        ///DESCRIPCIÓN          : se cargara el combo con los distintos areas de donacion y via publica
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 04/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        //private void Cargar_Combo_Areas_Donacion()
        //{
        //    Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Consulta_Area_Publicas_Donacion = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
        //    DataTable Dt_Consulta = new DataTable();
        //    try
        //    {
        //        Dt_Consulta = Negocio_Consulta_Area_Publicas_Donacion.Consultar_Condiciones_Via_Publica_Donacion();

        //        if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
        //        {
        //            Cmb_Via_Publica_Area_Donacion.DataSource = Dt_Consulta;
        //            Cmb_Via_Publica_Area_Donacion.DataValueField = Cat_Ort_Area_Public_Donac.Campo_Area_ID;
        //            Cmb_Via_Publica_Area_Donacion.DataTextField = Cat_Ort_Area_Public_Donac.Campo_Descripcion;
        //            Cmb_Via_Publica_Area_Donacion.DataBind();
        //            Cmb_Via_Publica_Area_Donacion.Items.Insert(0, "< SELECCIONE >");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Cargar_Combo_Areas_Donacion " + ex.Message.ToString());
        //    }
        //}
        
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
        private void Enviar_Correo_Notificacion(Cls_Ope_Bandeja_Tramites_Negocio Solicitud,Cls_Ope_Bandeja_Tramites_Negocio Informacion_Solicitud, String Estatus_Anterior, String Actividad)
        {
            MailMessage Correo = new MailMessage();
            String Para = "";
            String De = "";
            String Puerto = "";
            String Servidor = "";
            String Contraseña = "";
            String Mensaje = "";
            Boolean Mensaje_Repetido = false;

            var Obj_Parametros = new Cls_Cat_Tra_Parametros_Negocio();
            try
            {
                // consultar parámetros
                Obj_Parametros.Consultar_Parametros();

                if (Solicitud.P_Correo_Electronico != null && Solicitud.P_Correo_Electronico.Trim().Length > 0)
                {
                    Para = Solicitud.P_Correo_Electronico;
                    De = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Correo_Saliente].ToString();
                    Contraseña = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Password_Correo].ToString();
                    Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Servidor_Correo].ToString();
                    Puerto = "25";

                    Correo.To.Add(Para);
                    Correo.From = new MailAddress(De, Obj_Parametros.P_Correo_Encabezado);//    va el asunto que se obtiene de los parametros
                    Correo.Subject = "Tramite - " + Informacion_Solicitud.P_Tramite.ToUpper().Trim();
                    Correo.SubjectEncoding = System.Text.Encoding.UTF8;

                    /**********************estructura del documento:*******************************
                     * secdcion del titulo del correo (ejemplo: seguimiento de solictud)
                     * fecha
                     * cuerpo del correo
                     * informacion de la solicutd y estatus
                     * despedida
                     * persona quien firma
                     * 
                     *******************************************************************************/

                    //  para el texto del mensaje
                    Mensaje = "<html>";
                    Mensaje += "<body> ";

                    //  seccion del titulo del correo
                    Mensaje += "<table width=\"100%\" > ";
                    Mensaje += "<tr>";
                    Mensaje += "<td style=\"width:100%;font-size:18px;\" align=\"center\" >";
                    Mensaje += Obj_Parametros.P_Correo_Encabezado.ToUpper().Trim();
                    Mensaje += "</td>";
                    Mensaje += "</hr>";
                    Mensaje += "</table>";

                    Mensaje += "<br>";

                    //  seccion de la fecha del documento
                    Mensaje += "<table width=\"100%\" > ";
                    Mensaje += "<tr>";
                    Mensaje += "<td style=\"width:100%;font-size:14px;\" align=\"right\" >";
                    Mensaje += "Irapuato, Guanajuato a " + String.Format("{0:D}", DateTime.Now) + ".";
                    Mensaje += "</td>";
                    Mensaje += "</hr>";
                    Mensaje += "</table>";


                    //  seccion del cuerpo del correo
                    Mensaje += "<table width=\"100%\" > ";
                    Mensaje += "<tr>";
                    Mensaje += "<td style=\"width:100%;font-size:14px;\" align=\"left\" >";
                    Mensaje += Obj_Parametros.P_Correo_Cuerpo;
                    Mensaje += "</td>";
                    Mensaje += "</hr>";
                    Mensaje += "</table>";

                    //  linea
                    Mensaje += "<hr width=\"98%\">";

                    //  informacion general de la solicitud
                    Mensaje += "<br> <p align=justify style=\"font-size:14px\"> ";
                    Mensaje += "Información general de la solicitud:";
                    Mensaje += "<br> Nombre del solicitante <b>" + Informacion_Solicitud.P_Solicito + "</b> ";
                    Mensaje += "<br> Solicitud de Tramite <b>" + Informacion_Solicitud.P_Tramite + "</b> ";
                    Mensaje += "<br> Con Folio <b>" + Informacion_Solicitud.P_Clave_Solicitud + "<b>. <br>";

                    //  estatus terminado
                    if (Solicitud.P_Estatus.Equals("TERMINADO"))
                    {
                        Mensaje += "<br> Ha  <b>FINALIZADO</b> de manera exitosa.";
                        //if (!String.IsNullOrEmpty(Solicitud.P_Comentarios))
                        //    Mensaje += "<br><br> <b>NOTA:</b> " + Solicitud.P_Comentarios + ".";
                    }

                    //  estatus detenido
                    else if (Solicitud.P_Estatus.Equals("DETENIDO"))
                    {
                        Mensaje += "<br>  Ha sido <b>DETENIDA</b>.";
                        Mensaje += "<br><b>POR LA CAUSA:</b> " + Solicitud.P_Comentarios;
                        Mensaje += "."; //  punto final de la causa
                        Mensaje_Repetido = true;
                    }

                    //  estatus cancelado
                    else if (Solicitud.P_Estatus.Equals("CANCELADO"))
                    {
                        Mensaje += "<br>  Ha sido <b>CANCELADA</b>.";
                        Mensaje += "<br><b>POR LA CAUSA:</b> " + Solicitud.P_Comentarios;
                        Mensaje += "."; //  punto final de la causa
                        Mensaje_Repetido = true;
                    }

                    //  VALIDACION PARA PODER IMPRIMIR EL REPORTE DE LA SOLICITUD
                    if (Estatus_Anterior == "PENDIENTE" )
                    {
                        Mensaje += "<br><b>USTED YA PUEDE IMPRIMIR EL FORMATO</b> ";
                        Mensaje += "."; //  punto final de la causa
                        Mensaje_Repetido = true;
                    }

                    //  validacion para cuando se cumple con una actividad
                    else if (Estatus_Anterior == "PROCESO")
                    {
                        if (Mensaje_Repetido != true)
                        {
                            Mensaje += "<br>  Ha pasado <b> la actividad  </b>.";
                            Mensaje += "<br><b>" + Actividad + "</b> ";
                            Mensaje += "."; //  punto final de la causa
                        }
                    }

                    Mensaje += "</p>";
                    Mensaje += "<hr width=\"98%\">";

                    //  Seccion de la despedida
                    Mensaje += "<br>" + "<p align =left> <b> " + Obj_Parametros.P_Correo_Despedida + " </b>.</p> ";

                    //  Seccion de quien firma el correo
                    Mensaje += "<br> <p align =center> Atentemente </b> </p> ";
                    Mensaje += "<br> <p align =center>" + Obj_Parametros.P_Correo_Firma + "</b> </p> ";

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
                throw new Exception("Enviar_Correo_Notificacion" + Ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Tipo_Residuos
        ///DESCRIPCIÓN          : se cargara el combo con las zonas
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 04/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Cargar_Combo_Tipo_Residuos()
        {
            Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio Negocio_Consulta_Resudios = new Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio();
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Dt_Consulta = Negocio_Consulta_Resudios.Consultar_Tipos_Residuos();

                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    Cmb_Tipo_Residuo.DataSource = Dt_Consulta;
                    Cmb_Tipo_Residuo.DataValueField = Cat_Ort_Tipo_Residuos.Campo_Residuo_ID;
                    Cmb_Tipo_Residuo.DataTextField = Cat_Ort_Tipo_Residuos.Campo_Nombre;
                    Cmb_Tipo_Residuo.DataBind();
                    Cmb_Tipo_Residuo.Items.Insert(0, "< SELECCIONE >");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cargar_Combo_Tipo_Residuos " + ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Formato_ID
        ///DESCRIPCIÓN          : se cargara el combo con los distintos areas de donacion y via publica
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 04/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private String Consultar_Formato_ID()
        {
            Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Consulta_Formato_ID = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
            DataTable Dt_Consulta = new DataTable();
            String Formato_ID = "";
            try
            {
                Negocio_Consulta_Formato_ID.P_Nombre_Plantilla = "Cedula de Visita";
                Dt_Consulta = Negocio_Consulta_Formato_ID.Consultar_Formato_ID();

                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    Formato_ID = Dt_Consulta.Rows[0][Cat_Tra_Formato_Predefinido.Campo_Formato_ID].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Inicializar_Controles " + ex.Message.ToString());
            }
            return Formato_ID;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Llenado_Formatos
        ///DESCRIPCIÓN          : se cargara el grid con los formatos a llenar
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private String Consultar_Llenado_Formatos(String Formato_ID)
        {
            Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Consulta_Formato_ID = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Negocio_Consulta_Formato_ID.P_Plantilla_ID = Formato_ID;
                Negocio_Consulta_Formato_ID.P_Empleado_Id = Cls_Sessiones.Empleado_ID;

                Dt_Consulta = Negocio_Consulta_Formato_ID.Consultar_Llenado_Solicitud_Formato();

               
                    Grid_Formatos.Columns[1].Visible = true;
                    Grid_Formatos.Columns[2].Visible = true;
                    Grid_Formatos.Columns[3].Visible = true;
                    if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                    {
                        Grid_Formatos.DataSource = Dt_Consulta;
                        Pnl_Captura_Cedula.Style.Value = "display: block";
                    }
                    else
                    {
                        Grid_Formatos.DataSource = new DataTable();
                        Pnl_Captura_Cedula.Style.Value = "display: none";
                    }

                    Grid_Formatos.DataBind();
                    Grid_Formatos.Columns[1].Visible = false;
                    Grid_Formatos.Columns[2].Visible = false;
                    Grid_Formatos.Columns[3].Visible = false;

                //Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
                //Obj_Parametros.Consultar_Parametros();
                
                //if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                //{
                //    if (Cls_Sessiones.Rol_ID == Obj_Parametros.P_Rol_Director_Ordenamiento)
                //    {
                //        Llenar_Grid_Listado(0);
                //        Div_Grid_Formatos.Style.Value = "display:none";
                //        Grid_Listado.Style.Value = "display:block";
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("Inicializar_Controles " + ex.Message.ToString());
            }
            return Formato_ID;
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Areas_Uso_Actual
        ///DESCRIPCIÓN          : se cargara el combo con los distintos tipos de areas de uso actual
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 04/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Cargar_Combo_Areas_Uso_Actual()
        {
            Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Consulta_Uso_Actual = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Dt_Consulta = Negocio_Consulta_Uso_Actual.Consultar_Uso_Actual_Terreno();

                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    Cmb_Uso_Actual.DataSource = Dt_Consulta;
                    Cmb_Uso_Actual.DataValueField = Cat_Ort_Uso_Actual.Campo_Uso_Actual_ID;
                    Cmb_Uso_Actual.DataTextField = Cat_Ort_Uso_Actual.Campo_Descripcion;
                    Cmb_Uso_Actual.DataBind();
                    Cmb_Uso_Actual.Items.Insert(0, "< SELECCIONE >");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Inicializar_Controles " + ex.Message.ToString());
            }
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
            String Ventana_Modal = "Abrir_Resumen('../Ventanilla/Ventanas_Emergentes/Frm_Resumen_Predio_Ventanilla.aspx";
            String Propiedades = ", 'center:yes,resizable=no,status=no,width=750,scrollbars=yes,');";
            //String Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:800px;dialogHeight:600px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Funcionamiento
        ///DESCRIPCIÓN          : se cargara el combo con los distintos tipos problemas con respecto al
        ///                         al funcionamiento
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 04/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Cargar_Combo_Funcionamiento()
        {
            Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Consulta_Funcionamiento = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Dt_Consulta = Negocio_Consulta_Funcionamiento.Consultar_Problemas_Funcionamiento();

                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    Cmb_Funcionamiento.DataSource = Dt_Consulta;
                    Cmb_Funcionamiento.DataValueField = Cat_Ort_Funcionamiento.Campo_Funcionamiento_ID;
                    Cmb_Funcionamiento.DataTextField = Cat_Ort_Funcionamiento.Campo_Nombre;
                    Cmb_Funcionamiento.DataBind();
                    Cmb_Funcionamiento.Items.Insert(0, "< SELECCIONE >");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Inicializar_Controles " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Materiales
        ///DESCRIPCIÓN          : se cargara el combo con los distintos tipos de materiales
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 04/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Cargar_Combo_Materiales()
        {
            Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Consulta_Funcionamiento = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Dt_Consulta = Negocio_Consulta_Funcionamiento.Consultar_Tipos_Materiales();

                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    Cmb_Material_Muros.DataSource = Dt_Consulta;
                    Cmb_Material_Muros.DataValueField = Cat_Ort_Tipo_Material.Campo_Material_ID;
                    Cmb_Material_Muros.DataTextField = Cat_Ort_Tipo_Material.Campo_Nombre;
                    Cmb_Material_Muros.DataBind();
                    Cmb_Material_Muros.Items.Insert(0, "< SELECCIONE >");

                    Cmb_Material_Techo.DataSource = Dt_Consulta;
                    Cmb_Material_Techo.DataValueField = Cat_Ort_Tipo_Material.Campo_Material_ID;
                    Cmb_Material_Techo.DataTextField = Cat_Ort_Tipo_Material.Campo_Nombre;
                    Cmb_Material_Techo.DataBind();
                    Cmb_Material_Techo.Items.Insert(0, "< SELECCIONE >");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Inicializar_Controles " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Inspectores
        ///DESCRIPCIÓN          : se cargara el combo con los distintos inspectores
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Cargar_Combo_Inspectores()
        {
            Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Consultar_Solicitud = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
            DataTable Dt_Consulta = new DataTable();
            try
            {
                //Dt_Consulta = Negocio_Consulta_Funcionamiento.Consultar_Inspectores();

                //if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                //{
                //    Cmb_Inspector.DataSource = Dt_Consulta;
                //    Cmb_Inspector.DataValueField = Cat_Ort_Inspectores.Campo_Inspector_ID;
                //    Cmb_Inspector.DataTextField = Cat_Ort_Inspectores.Campo_Nombre;
                //    Cmb_Inspector.DataBind();
                //    Cmb_Inspector.Items.Insert(0, "< SELECCIONE >");
                //}

                Dt_Consulta = Negocio_Consultar_Solicitud.Consultar_Personal();
                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    Cmb_Inspector.DataSource = Dt_Consulta;
                    Cmb_Inspector.DataValueField = Cat_Empleados.Campo_Empleado_ID;
                    Cmb_Inspector.DataTextField = "Nombre_Usuario";
                    Cmb_Inspector.DataBind();
                    Cmb_Inspector.Items.Insert(0, "< SELECCIONE >");
                }
                else
                {
                    Cmb_Inspector.DataSource = new DataTable();
                    Cmb_Inspector.DataBind();

                    Lbl_Mensaje_Error.Text = "No se encuentran personal";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Inicializar_Controles " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Fomato
        ///DESCRIPCIÓN          : se cargara la clase de negocios con la informacion
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Alta_Formato()
        {
            Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Alta_Formato = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
            Cls_Ope_Bandeja_Tramites_Negocio Neg_Enviar_Correo = new Cls_Ope_Bandeja_Tramites_Negocio();
            Cls_Ope_Bandeja_Tramites_Negocio Neg_Informacion_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
            String Actividad = "";
            try
            {

                Neg_Informacion_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                //  se carga los datos de la consulta en la capa de negocios
                Neg_Informacion_Solicitud = Neg_Informacion_Solicitud.Consultar_Datos_Solicitud(); // Se obtienen los Datos a Detalle de la Solicitud Seleccionada
                Actividad = Neg_Informacion_Solicitud.P_Subproceso_Nombre;

                if (Grid_Datos_Dictamen_Modificar.Style.Value == "display:none")
                {
                    DataTable Dt_Datos = (DataTable)(Session["Grid_Datos"]);
                    String[,] Datos = new String[Dt_Datos.Rows.Count, 2];

                    for (int Contador_For = 0; Contador_For < Dt_Datos.Rows.Count; Contador_For++)
                    {

                        Datos[Contador_For, 0] = Dt_Datos.Rows[Contador_For].ItemArray[0].ToString();

                        String Temporal = Grid_Datos_Dictamen.Rows[Contador_For].Cells[0].Text;

                        String Valor_Dato = ((TextBox)Grid_Datos_Dictamen.Rows[Contador_For].FindControl("Txt_Descripcion_Datos")).Text;

                        if (Valor_Dato != "" || (Dt_Datos.Rows[Contador_For][Cat_Tra_Datos_Tramite.Campo_Dato_Requerido].ToString()) == "N")
                        {
                            Datos[Contador_For, 1] = Valor_Dato;
                        }
                    }

                    Neg_Informacion_Solicitud.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Neg_Informacion_Solicitud.P_Datos = Datos;
                    Neg_Informacion_Solicitud.P_Tramite_id = Hdf_Tramite_ID.Value;
                    Neg_Informacion_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                    Neg_Informacion_Solicitud.Alta_Datos_Dictamen();
                }
                else
                {
                    DataTable Dt_Datos_Mod = new DataTable();
                    Dt_Datos_Mod.Columns.Add("OPE_DATO_ID", typeof(String));
                    Dt_Datos_Mod.Columns.Add("VALOR", typeof(String));

                    foreach (GridViewRow Gr_Row in Grid_Datos_Dictamen_Modificar.Rows)
                    {
                        DataRow Fila = Dt_Datos_Mod.NewRow();
                        Fila["OPE_DATO_ID"] = Gr_Row.Cells[0].Text.ToString();
                        Fila["VALOR"] = ((TextBox)Gr_Row.Cells[3].FindControl("Txt_Valor_Dato")).Text;
                        Dt_Datos_Mod.Rows.Add(Fila);
                    }
                    Neg_Informacion_Solicitud.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Neg_Informacion_Solicitud.P_Dt_Datos = Dt_Datos_Mod;
                    Neg_Informacion_Solicitud.Modificar_Datos_Dictamen();
                }

                //  para el dato de la cuenta predila
                if (Txt_Cuenta_Predial.Text != "")
                    Negocio_Alta_Formato.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;

                else
                {
                    Negocio_Alta_Formato.P_Area_Calle = Cmb_Calle.SelectedValue;
                    Negocio_Alta_Formato.P_Area_Colonia = Cmb_Colonias.SelectedValue;
                    Negocio_Alta_Formato.P_Area_Numero_Fisico = Txt_Numero_Fisico.Text;
                    Negocio_Alta_Formato.P_Area_Manzana = Txt_Lote.Text;
                    Negocio_Alta_Formato.P_Area_Lote = Txt_Manzana.Text;
                }

                //  para los datos de los id generales
                Negocio_Alta_Formato.P_Tramite_ID = Hdf_Tramite_ID.Value;
                Negocio_Alta_Formato.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                Negocio_Alta_Formato.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
                Negocio_Alta_Formato.P_Inspector_ID = Cmb_Inspector.SelectedValue;

                //  para los datos de la area
                Negocio_Alta_Formato.P_Area_Inspeccion = RBtn_Area_Inspeccion.SelectedValue;
                Negocio_Alta_Formato.P_Area_Zona = Cmb_Zona.SelectedValue;
                Negocio_Alta_Formato.P_Area_Uso_Solicitado = Txt_Destinado.Text;

                //  para los datos del tipo de supervision
                Negocio_Alta_Formato.P_Tipo_Supervision_ID = Cmb_Tipo_Supervision.SelectedValue;

                //  para los datos de las condiciones del inmueble
                Negocio_Alta_Formato.P_Condiciones_Inmueble_ID = Cmb_Condiciones_Inmueble.SelectedIndex == 0 ? "" : Cmb_Condiciones_Inmueble.SelectedValue;

                //  para el avance de la obra
                Negocio_Alta_Formato.P_Avance_Obra_ID = Cmb_Avance_Obra.SelectedIndex == 0 ? "" : Cmb_Avance_Obra.SelectedValue;
                if (Txt_Avance_Obra_Aproximado.Text != "")
                    Negocio_Alta_Formato.P_Avance_Bardeo_Aproximado = Txt_Avance_Obra_Aproximado.Text;

                if (Txt_Avance_Obra_Niveles_Actuales.Text != "")
                    Negocio_Alta_Formato.P_Avance_Niveles_Actuales = Txt_Avance_Obra_Niveles_Actuales.Text;

                if (Txt_Avance_Obra_Niveles_Construir.Text != "")
                    Negocio_Alta_Formato.P_Avance_Niveles_Construccion = Txt_Avance_Obra_Niveles_Construir.Text;

                Negocio_Alta_Formato.P_Avance_Proyecto_Acorde = RBtn_Avance_Obra_Acorde_Solicitado.SelectedValue;

                //  para las vias publicas y donaciones
                Negocio_Alta_Formato.P_VIA_PUBLICA_INVASION_DONACION = Chk_Invasion_Areas_donacion.Checked ? "SI" : "NO";
                Negocio_Alta_Formato.P_VIA_PUBLICA_INVASION_MATERIAL = Chk_Invasion_Material.Checked ? "SI" : "NO";
                Negocio_Alta_Formato.P_VIA_PUBLICA_PARAMENTO = Chk_Sobresale_Paramento.Checked ? "SI" : "NO";
                Negocio_Alta_Formato.P_VIA_PUBLICA_SOBRE_MARQUESINA = Chk_Construccion_Marquesina.Checked ? "SI" : "NO";
                //Negocio_Alta_Formato.P_Area_Via_ID = Cmb_Via_Publica_Area_Donacion.SelectedValue;
                Negocio_Alta_Formato.P_Area_Via_Especificar_Restricciones = Txt_Especificacion_Restriccion.Text;

                //  para las datos referentes a las inspecciones 
                Negocio_Alta_Formato.P_Inspeccion_Notificacion = RBtn_Notificado.SelectedValue;
                if (RBtn_Notificado.SelectedValue == "NO" || RBtn_Notificado.SelectedIndex == -1)
                    Negocio_Alta_Formato.P_Inspeccion_Notificacion_Folio = "";
                else
                    Negocio_Alta_Formato.P_Inspeccion_Notificacion_Folio = Txt_Notificacion_Folio.Text;

                Negocio_Alta_Formato.P_Inspeccion_Acta = RBtn_Acta_Inspeccion.SelectedValue;

                if (RBtn_Acta_Inspeccion.SelectedValue == "NO" || RBtn_Acta_Inspeccion.SelectedIndex == -1)
                    Negocio_Alta_Formato.P_Inspeccion_Acta_Folio = "";
                else
                    Negocio_Alta_Formato.P_Inspeccion_Acta_Folio = Txt_Acta_Inspeccion_Folio.Text;

                Negocio_Alta_Formato.P_Inspeccion_Clausurado = RBtn_Clausurado.SelectedValue;
                if (RBtn_Clausurado.SelectedValue == "NO" || RBtn_Clausurado.SelectedIndex == -1)
                    Negocio_Alta_Formato.P_Inspeccion_Clausurado_Folio = "";
                else
                    Negocio_Alta_Formato.P_Inspeccion_Clausurado_Folio = Txt_Clausurado_Folio.Text;

                Negocio_Alta_Formato.P_Inspeccion_Multado = RBtn_Multado.SelectedValue;

                if (RBtn_Multado.SelectedValue == "NO" || RBtn_Multado.SelectedIndex == -1)
                    Negocio_Alta_Formato.P_Inspeccion_Multado_Folio = "";
                else
                    Negocio_Alta_Formato.P_Inspeccion_Multado_Folio = Txt_Multa_Folio.Text;

                //  para el uso actual
                Negocio_Alta_Formato.P_Uso_Actual_ID = Cmb_Uso_Actual.SelectedIndex == 0 ? "" : Cmb_Uso_Actual.SelectedValue;
                Negocio_Alta_Formato.P_Uso_Actual_Acorde_Solicitado = RBtn_Uso_Diferente_Adicional.SelectedValue;
                Negocio_Alta_Formato.P_Uso_Actual_Especificar_Tipo_Uso = Txt_Especificar_Tipo_Uso.Text;

                //  para el uso predominante de la zona  
                Negocio_Alta_Formato.P_Uso_Predominante_Colindantes = Txt_Usos_Colindantes.Text;
                Negocio_Alta_Formato.P_Uso_Predominante_Frente_Inmueble = Txt_Usos_Frente_Inmueble.Text;
                Negocio_Alta_Formato.P_Uso_Predominante_Impacto = Txt_Usos_Cercanos_Riesgo.Text;

                //  para el uso del funcionamiento
                Negocio_Alta_Formato.P_Funcionamiento_Actividad = Txt_Area_Acticidad.Text;
                if (Txt_Superficie_Metros2.Text != "")
                    Negocio_Alta_Formato.P_Funcionamiento_Metros_Cuadrados = Txt_Superficie_Metros2.Text;

                Negocio_Alta_Formato.P_Funcionamiento_Maquinaria = Txt_Maquinaria_Utilizar.Text;
                Negocio_Alta_Formato.P_Funcionamiento_ID = Cmb_Funcionamiento.SelectedIndex == 0 ? "" : Cmb_Funcionamiento.SelectedValue;
                if (Txt_Cantidad_Personas_Laboran.Text != "")
                    Negocio_Alta_Formato.P_Funcionamiento_No_Personas = Txt_Cantidad_Personas_Laboran.Text;

                if (Txt_Afluencia.Text != "")
                    Negocio_Alta_Formato.P_Funcionamiento_No_Clientes = Txt_Afluencia.Text;

                //  para los campos de anuncios 
                Negocio_Alta_Formato.P_Anuncio_1 = Txt_Tipo_Anuncio_1.Text;
                if (Txt_Dimenciones_Anuncion_1.Text != "")
                    Negocio_Alta_Formato.P_Anuncio_1_Dimensiones = Txt_Dimenciones_Anuncion_1.Text;

                Negocio_Alta_Formato.P_Anuncio_2 = Txt_Tipo_Anuncio_2.Text;
                if (Txt_Dimenciones_Anuncion_2.Text != "")
                    Negocio_Alta_Formato.P_Anuncio_2_Dimensiones = Txt_Dimenciones_Anuncion_2.Text;

                Negocio_Alta_Formato.P_Anuncio_3 = Txt_Tipo_Anuncio_3.Text;
                if (Txt_Dimenciones_Anuncion_3.Text != "")
                    Negocio_Alta_Formato.P_Anuncio_3_Dimensiones = Txt_Dimenciones_Anuncion_3.Text;

                Negocio_Alta_Formato.P_Anuncio_4 = Txt_Tipo_Anuncio_4.Text;
                if (Txt_Dimenciones_Anuncion_4.Text != "")
                    Negocio_Alta_Formato.P_Anuncio_4_Dimensiones = Txt_Dimenciones_Anuncion_4.Text;

                //  para los servicios
                Negocio_Alta_Formato.P_Servicios_Cuenta_Sanitarios = RBtn_Servicios_Sanitarios.SelectedValue;
                Negocio_Alta_Formato.P_Servicios_WC = RBtn_Wc.SelectedValue;
                Negocio_Alta_Formato.P_Servicios_Lavabo = RBtn_Lavabo.SelectedValue;
                Negocio_Alta_Formato.P_Servicios_Letrina = RBtn_Letrina.SelectedValue;
                Negocio_Alta_Formato.P_Servicios_Mixto = RBtn_Mixto.SelectedValue;
                if (Txt_Bano_Hombres.Text != "")
                    Negocio_Alta_Formato.P_Servicios_Numero_Sanitarios_Hombres = Txt_Bano_Hombres.Text;
                else
                    Negocio_Alta_Formato.P_Servicios_Numero_Sanitarios_Hombres = "0";

                if (Txt_Bano_Mujeres.Text != "")
                    Negocio_Alta_Formato.P_Servicios_Numero_Sanitarios_Mujeres = Txt_Bano_Mujeres.Text;
                else
                    Negocio_Alta_Formato.P_Servicios_Numero_Sanitarios_Mujeres = "0";

                Negocio_Alta_Formato.P_Servicios_Agua_Potable = RBtn_Agua_Potable.SelectedValue;
                Negocio_Alta_Formato.P_Servicios_Agua_Abastecimiento_Particular = RBtn_Agua_Potable_Particular.SelectedValue;
                Negocio_Alta_Formato.P_Servicios_Agua_Abastecimiento_Japami = RBtn_Agua_Potable_Japami.SelectedValue;
                Negocio_Alta_Formato.P_Servicios_Drenaje = RBtn_Agua_Potable_Drenaje.SelectedValue;
                Negocio_Alta_Formato.P_Servicios_Fosa_Septica = RBtn_Agua_Potable_Fosa_Septica.SelectedValue;
                Negocio_Alta_Formato.P_Servicios_Estacionamiento = RBtn_Cuenta_Estacionamiento.SelectedValue;
                Negocio_Alta_Formato.P_Servicios_Estacionamiento_Propio = RBtn_Cuenta_Estacionamiento_Dentro_Inmueble.SelectedValue;
                Negocio_Alta_Formato.P_Servicios_Estacionamiento_Rentado = RBtn_Cuenta_Estacionamiento_Rentado.SelectedValue;
                if (Txt_Numero_Cajones.Text != "")
                    Negocio_Alta_Formato.P_Servicios_Estacionamiento_Numero_Cajones = Txt_Numero_Cajones.Text;
                //else
                //    Negocio_Alta_Formato.P_Servicios_Estacionamiento_Numero_Cajones = "0";

                Negocio_Alta_Formato.P_Servicios_Estacionamiento_Area_Descarga = RBtn_Cuenta_Estacionamiento_Area_Carga.SelectedValue;
                Negocio_Alta_Formato.P_Servicios_Estacionamiento_Domicilio = Txt_Domicilio_Estacionamiento.Text;

                //  para los materiales empleados
                Negocio_Alta_Formato.P_Materiales_Empleado_Muros = Cmb_Material_Muros.SelectedIndex == 0 ? "" : Cmb_Material_Muros.SelectedValue;
                Negocio_Alta_Formato.P_Materiales_Empleado_Techos = Cmb_Material_Techo.SelectedIndex == 0 ? "" : Cmb_Material_Techo.SelectedValue;

                //  para las medidas de seguridad
                Negocio_Alta_Formato.P_Seguridad_Medidas = RBtn_Medidas_Seguridad_Senalizacion.SelectedValue;
                Negocio_Alta_Formato.P_Seguridad_Equipo = RBtn_Medidas_Seguridad_Equipo.SelectedValue;
                Negocio_Alta_Formato.P_Seguridad_Material_Flamable = RBtn_Medidas_Seguridad_Material_Flamable.SelectedValue;
                Negocio_Alta_Formato.P_Seguridad_Especificar = Txt_Medidas_Seguridad_Especificar.Text;

                //  para la poda de arboles
                if (Txt_Arbol_Altura.Text != "")
                    Negocio_Alta_Formato.P_Poda_Altura = Txt_Arbol_Altura.Text.Trim();

                if (Txt_Arbol_Diametro.Text != "")
                    Negocio_Alta_Formato.P_Poda_Diametro_Tronco = Txt_Arbol_Diametro.Text;

                if (Txt_Arbol_Diametro_Fronda.Text != "")
                    Negocio_Alta_Formato.P_Poda_Fronda = Txt_Arbol_Diametro_Fronda.Text;

                Negocio_Alta_Formato.P_Poda_Estado = Txt_Arbol_Estado.Text;

                //  para los campos generales                
                Negocio_Alta_Formato.P_Generales_Recepcion_Inspector = Convert.ToDateTime(Txt_Generales_Recepcion_Inspector_Fecha.Text);
                Negocio_Alta_Formato.P_Generales_Fecha_Revision_Campo = Convert.ToDateTime(Txt_Generales_Recepcion_Campo_Fecha.Text);
                Negocio_Alta_Formato.P_Generales_Recepcion_Coordinacion = Convert.ToDateTime(Txt_Generales_Recepcion_Coordinador_Fecha.Text);
                Negocio_Alta_Formato.P_Generales_Observaciones_Para_Inspector = Txt_Generales_Observaciones_Para_Inspector.Text;
                Negocio_Alta_Formato.P_Generales_Observaciones_Inspector = Txt_Generales_Observaciones_Del_Inspector.Text;

                //  para los campos de auditoria
                Negocio_Alta_Formato.P_Usuario = Cls_Sessiones.Nombre_Empleado;

                //  para los datos del manifiesto de impacto ambiental
                Negocio_Alta_Formato.P_Impacto_Afectables = Txt_Manifiesto_Afectacion.Text;
                Negocio_Alta_Formato.P_Impacto_Colindancias = Txt_Manifiesto_Colindancia.Text;
                Negocio_Alta_Formato.P_Impacto_Superficie = Txt_Manifiesto_Superficie_Total.Text;
                Negocio_Alta_Formato.P_Impacto_Tipo_Proyecto = Txt_Manifiesto_Tipo_Proyecto.Text;

                // para los datos de licencia
                Negocio_Alta_Formato.P_Licencia_Tipo_Equipo = Txt_Licencia_Equipo_Emisor.Text;
                Negocio_Alta_Formato.P_Licencia_Tipo_Emision = Txt_Licencia_Tipo_Emision.Text;
                Negocio_Alta_Formato.P_Licencia_Horario_Funcionamiento = Txt_Licencia_Hora_Funcionamiento.Text;
                Negocio_Alta_Formato.P_Licencia_Tipo_Combustible = Txt_Licencia_Tipo_Conbustible.Text;
                Negocio_Alta_Formato.P_Licencia_Tipo_Gastos_Combustible = Txt_Licencia_Gastos_Combustible.Text;
                Negocio_Alta_Formato.P_Licencia_Almacenaje = Txt_Licencia_Almacenaje_Combustible.Text;
                Negocio_Alta_Formato.P_Licencia_Cantidad_Combustible = Txt_Licencia_Cantidad_Combustible.Text;


                //  para los datos del banco de materiales 
                Negocio_Alta_Formato.P_Material_Permiso_Ecologico = RBtn_Material_Permiso_Ecologia.SelectedValue;
                Negocio_Alta_Formato.P_Material_Permiso_Suelo = RBtn_Material_Permiso_Suelo.SelectedValue;
                Negocio_Alta_Formato.P_Material_Superficie_Total = Txt_Materiales_Superficie_Total.Text;
                Negocio_Alta_Formato.P_Material_Profundidad = Txt_Materiales_Profundidad.Text;
                Negocio_Alta_Formato.P_Material_Inclinacion = Txt_Materiales_Inclinacion.Text;
                Negocio_Alta_Formato.P_Material_Flora = Txt_Materiales_Flora.Text;
                Negocio_Alta_Formato.P_Material_Acceso_Vehiculos = RBtn_Material_Accesibilidad_Vehiculo.SelectedValue;
                Negocio_Alta_Formato.P_Material_Petreo = Txt_Materiales_Petreo.Text;
                Negocio_Alta_Formato.P_Material_Especie_Arbol = Txt_Arboles_Especie.Text;

                if (RBtn_Arboles_Tipo_Poda.Checked == true)
                {
                    Negocio_Alta_Formato.P_Material_Tipo_Poda = "SI";
                    Negocio_Alta_Formato.P_Material_Cantidad_Poda = Txt_Arboles_Cantidad_Poda.Text;
                }
                else
                    Negocio_Alta_Formato.P_Material_Tipo_Poda = "NO";

                if (RBtn_Arboles_Tipo_Tala.Checked == true)
                {
                    Negocio_Alta_Formato.P_Material_Tipo_Tala = "SI";
                    Negocio_Alta_Formato.P_Material_Cantidad_Tala = Txt_Arboles_Cantidad_Tala.Text;
                }
                else
                    Negocio_Alta_Formato.P_Material_Tipo_Tala = "NO";

                if (RBtn_Arboles_Tipo_Tala.Checked == true)
                {
                    Negocio_Alta_Formato.P_Material_Tipo_Trasplante = "SI";
                    Negocio_Alta_Formato.P_Material_Cantidad_Trasplante = Txt_Arboles_Cantidad_Trasplante.Text;
                }
                else
                    Negocio_Alta_Formato.P_Material_Tipo_Trasplante = "NO";

                //  para los datos de la autorizacion de aprovechamiento ambiental
                Negocio_Alta_Formato.P_Autoriza_Suelos = RBtn_Aprovechamiento_Uso_Suelo.SelectedValue;
                Negocio_Alta_Formato.P_Autoriza_Area_Residuos = RBtn_Aprovechamiento_Almacen_Residuos.SelectedValue;
                Negocio_Alta_Formato.P_Autoriza_Separacion = RBtn_Aprovechamiento_Existe_Separacion.SelectedValue;
                Negocio_Alta_Formato.P_Autoriza_Metodo_Separacion = Txt_Aprovechamiento_Metodo_Sepearacion.Text;
                Negocio_Alta_Formato.P_Autoriza_Servicio_Recoleccion = Txt_Aprovechamiento_Servicio_Recoleccion.Text;
                Negocio_Alta_Formato.P_Autoriza_Revuelven_Solidos_Liquidos = RBtn_Aprovechamiento_Revuelven_Liquidos.SelectedValue;
                Negocio_Alta_Formato.P_Autoriza_Tipo_Contenedor = Txt_Aprovechamiento_Tipo_Contenedor.Text;
                Negocio_Alta_Formato.P_Autoriza_Tipo_Ruido = Txt_Aprovechamiento_Tipo_Ruido.Text;

                if (Txt_Aprovechamiento_Nivel_Ruido.Text == "")
                    Txt_Aprovechamiento_Nivel_Ruido.Text = "0";

                Negocio_Alta_Formato.P_Autoriza_Nivel_Ruido = Txt_Aprovechamiento_Nivel_Ruido.Text;

                if (!String.IsNullOrEmpty(Txt_Aprovechamiento_Horario_Inicial.Text) && !String.IsNullOrEmpty(Txt_Aprovechamiento_Horario_Final.Text))
                    Negocio_Alta_Formato.P_Autoriza_Horario_Labores = Txt_Aprovechamiento_Horario_Inicial.Text + " a " + Txt_Aprovechamiento_Horario_Final.Text;

                if (Chk_Dias_Labor_Lunes.Checked == true)
                    Negocio_Alta_Formato.P_Autoriza_Lunes = "SI";
                else
                    Negocio_Alta_Formato.P_Autoriza_Lunes = "NO";

                if (Chk_Dias_Labor_Martes.Checked == true)
                    Negocio_Alta_Formato.P_Autoriza_Martes = "SI";
                else
                    Negocio_Alta_Formato.P_Autoriza_Martes = "NO";

                if (Chk_Dias_Labor_Miercoles.Checked == true)
                    Negocio_Alta_Formato.P_Autoriza_Miercoles = "SI";
                else
                    Negocio_Alta_Formato.P_Autoriza_Miercoles = "NO";

                if (Chk_Dias_Labor_Jueves.Checked == true)
                    Negocio_Alta_Formato.P_Autoriza_Jueves = "SI";
                else
                    Negocio_Alta_Formato.P_Autoriza_Jueves = "NO";

                if (Chk_Dias_Labor_Viernes.Checked == true)
                    Negocio_Alta_Formato.P_Autoriza_Viernes = "SI";
                else
                    Negocio_Alta_Formato.P_Autoriza_Viernes = "NO";

                if (Chk_Dias_Labor_Sabado.Checked == true)
                    Negocio_Alta_Formato.P_Autoriza_Sabado = "SI";
                else
                    Negocio_Alta_Formato.P_Autoriza_Sabado = "NO";

                if (Chk_Dias_Labor_Domingo.Checked == true)
                    Negocio_Alta_Formato.P_Autoriza_Domingo = "SI";
                else
                    Negocio_Alta_Formato.P_Autoriza_Domingo = "NO";

                if (Txt_Aprovechamiento_Emisiones_Atmosfera.Text == "")
                    Txt_Aprovechamiento_Emisiones_Atmosfera.Text = "0";

                Negocio_Alta_Formato.P_Autoriza_Emisiones = Txt_Aprovechamiento_Emisiones_Atmosfera.Text;

                //  para los elementos de residuos peligrosos
                Negocio_Alta_Formato.P_Dt_Residuos = (DataTable)Session["Dt_Tipo_Residuo"];

                if (Negocio_Alta_Formato.P_Dt_Residuos != null && Negocio_Alta_Formato.P_Dt_Residuos.Rows.Count > 0)
                {
                    //  se ordenara la tabla por ID
                    DataView Dv_Ordenar = new DataView(Negocio_Alta_Formato.P_Dt_Residuos);
                    Dv_Ordenar.Sort = "TIPO_RESIDUO_ID asc";//SOLICITO asc, 
                    Negocio_Alta_Formato.P_Dt_Residuos = Dv_Ordenar.ToTable();
                }

                //  se ejecuta el metodo de alta
                Negocio_Alta_Formato.Guardar_Formato();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tramites", "alert('Alta de formato exitosa');", true);
                Inicializar_Controles();
                if (Grid_Formatos.Rows.Count > 0)
                {
                    Grid_Formatos.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
                Mostrar_Mensaje_Error(true);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Actualizar_Ficha_Inspeccion
        ///DESCRIPCIÓN          : Actualiza una ficha de inspeccion
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Actualizar_Ficha_Inspeccion()
        {
            Cls_Cat_Ort_Administracion_Urbana_Negocio Ficha_Inspeccion_Negocio = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
            Ficha_Inspeccion_Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
            DataTable Dt_Cedula = Ficha_Inspeccion_Negocio.Consultar_Administracion_Urbana(Ficha_Inspeccion_Negocio);

            Ficha_Inspeccion_Negocio.P_Administracion_Urbana_ID = Dt_Cedula.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Administracion_Urbana_ID].ToString();

            //  para el dato de la cuenta predila
            if (Txt_Cuenta_Predial.Text != "")
                Ficha_Inspeccion_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;

            else
            {
                Ficha_Inspeccion_Negocio.P_Area_Calle = Cmb_Calle.SelectedValue;
                Ficha_Inspeccion_Negocio.P_Area_Colonia = Cmb_Colonias.SelectedValue;
                Ficha_Inspeccion_Negocio.P_Area_Numero_Fisico = Txt_Numero_Fisico.Text;
                Ficha_Inspeccion_Negocio.P_Area_Manzana = Txt_Lote.Text;
                Ficha_Inspeccion_Negocio.P_Area_Lote = Txt_Manzana.Text;
            }

            //  para los datos de los id generales
            Ficha_Inspeccion_Negocio.P_Tramite_ID = Hdf_Tramite_ID.Value;
            Ficha_Inspeccion_Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
            Ficha_Inspeccion_Negocio.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
            Ficha_Inspeccion_Negocio.P_Inspector_ID = Cmb_Inspector.SelectedValue;

            //  para los datos de la area
            Ficha_Inspeccion_Negocio.P_Area_Inspeccion = RBtn_Area_Inspeccion.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Area_Zona = Cmb_Zona.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Area_Uso_Solicitado = Txt_Destinado.Text;

            //  para los datos del tipo de supervision
            Ficha_Inspeccion_Negocio.P_Tipo_Supervision_ID = Cmb_Tipo_Supervision.SelectedValue;

            //  para los datos de las condiciones del inmueble
            Ficha_Inspeccion_Negocio.P_Condiciones_Inmueble_ID = Cmb_Condiciones_Inmueble.SelectedIndex == 0 ? "" : Cmb_Condiciones_Inmueble.SelectedValue;

            //  para el avance de la obra
            Ficha_Inspeccion_Negocio.P_Avance_Obra_ID = Cmb_Avance_Obra.SelectedIndex == 0 ? "" : Cmb_Avance_Obra.SelectedValue;
            if (Txt_Avance_Obra_Aproximado.Text != "")
                Ficha_Inspeccion_Negocio.P_Avance_Bardeo_Aproximado = Txt_Avance_Obra_Aproximado.Text;

            if (Txt_Avance_Obra_Niveles_Actuales.Text != "")
                Ficha_Inspeccion_Negocio.P_Avance_Niveles_Actuales = Txt_Avance_Obra_Niveles_Actuales.Text;

            if (Txt_Avance_Obra_Niveles_Construir.Text != "")
                Ficha_Inspeccion_Negocio.P_Avance_Niveles_Construccion = Txt_Avance_Obra_Niveles_Construir.Text;

            Ficha_Inspeccion_Negocio.P_Avance_Proyecto_Acorde = RBtn_Avance_Obra_Acorde_Solicitado.SelectedValue;

            //  para las vias publicas y donaciones
            Ficha_Inspeccion_Negocio.P_VIA_PUBLICA_INVASION_DONACION = Chk_Invasion_Areas_donacion.Checked ? "SI" : "NO";
            Ficha_Inspeccion_Negocio.P_VIA_PUBLICA_INVASION_MATERIAL = Chk_Invasion_Material.Checked ? "SI" : "NO";
            Ficha_Inspeccion_Negocio.P_VIA_PUBLICA_PARAMENTO = Chk_Sobresale_Paramento.Checked ? "SI" : "NO";
            Ficha_Inspeccion_Negocio.P_VIA_PUBLICA_SOBRE_MARQUESINA = Chk_Construccion_Marquesina.Checked ? "SI" : "NO";
            //Ficha_Inspeccion_Negocio.P_Area_Via_ID = Cmb_Via_Publica_Area_Donacion.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Area_Via_Especificar_Restricciones = Txt_Especificacion_Restriccion.Text;

            //  para las datos referentes a las inspecciones 
            Ficha_Inspeccion_Negocio.P_Inspeccion_Notificacion = RBtn_Notificado.SelectedValue;
            if (RBtn_Notificado.SelectedValue == "NO" || RBtn_Notificado.SelectedIndex == -1)
                Ficha_Inspeccion_Negocio.P_Inspeccion_Notificacion_Folio = "null";
            else
                Ficha_Inspeccion_Negocio.P_Inspeccion_Notificacion_Folio = Txt_Notificacion_Folio.Text;

            Ficha_Inspeccion_Negocio.P_Inspeccion_Acta = RBtn_Acta_Inspeccion.SelectedValue;

            if (RBtn_Acta_Inspeccion.SelectedValue == "NO" || RBtn_Acta_Inspeccion.SelectedIndex == -1)
                Ficha_Inspeccion_Negocio.P_Inspeccion_Acta_Folio = "null";
            else
                Ficha_Inspeccion_Negocio.P_Inspeccion_Acta_Folio = Txt_Acta_Inspeccion_Folio.Text;

            Ficha_Inspeccion_Negocio.P_Inspeccion_Clausurado = RBtn_Clausurado.SelectedValue;
            if (RBtn_Clausurado.SelectedValue == "NO" || RBtn_Clausurado.SelectedIndex == -1)
                Ficha_Inspeccion_Negocio.P_Inspeccion_Clausurado_Folio = "null";
            else
                Ficha_Inspeccion_Negocio.P_Inspeccion_Clausurado_Folio = Txt_Clausurado_Folio.Text;

            Ficha_Inspeccion_Negocio.P_Inspeccion_Multado = RBtn_Multado.SelectedValue;

            if (RBtn_Multado.SelectedValue == "NO" || RBtn_Multado.SelectedIndex == -1)
                Ficha_Inspeccion_Negocio.P_Inspeccion_Multado_Folio = "";
            else
                Ficha_Inspeccion_Negocio.P_Inspeccion_Multado_Folio = Txt_Multa_Folio.Text;

            //  para el uso actual
            Ficha_Inspeccion_Negocio.P_Uso_Actual_ID = Cmb_Uso_Actual.SelectedIndex == 0 ? "" : Cmb_Uso_Actual.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Uso_Actual_Acorde_Solicitado = RBtn_Uso_Diferente_Adicional.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Uso_Actual_Especificar_Tipo_Uso = Txt_Especificar_Tipo_Uso.Text;

            //  para el uso predominante de la zona  
            Ficha_Inspeccion_Negocio.P_Uso_Predominante_Colindantes = Txt_Usos_Colindantes.Text;
            Ficha_Inspeccion_Negocio.P_Uso_Predominante_Frente_Inmueble = Txt_Usos_Frente_Inmueble.Text;
            Ficha_Inspeccion_Negocio.P_Uso_Predominante_Impacto = Txt_Usos_Cercanos_Riesgo.Text;

            //  para el uso del funcionamiento
            Ficha_Inspeccion_Negocio.P_Funcionamiento_Actividad = Txt_Area_Acticidad.Text;
            if (Txt_Superficie_Metros2.Text != "")
                Ficha_Inspeccion_Negocio.P_Funcionamiento_Metros_Cuadrados = Txt_Superficie_Metros2.Text;

            Ficha_Inspeccion_Negocio.P_Funcionamiento_Maquinaria = Txt_Maquinaria_Utilizar.Text;
            Ficha_Inspeccion_Negocio.P_Funcionamiento_ID = Cmb_Funcionamiento.SelectedIndex == 0 ? "" : Cmb_Funcionamiento.SelectedValue;
            if (Txt_Cantidad_Personas_Laboran.Text != "")
                Ficha_Inspeccion_Negocio.P_Funcionamiento_No_Personas = Txt_Cantidad_Personas_Laboran.Text;

            if (Txt_Afluencia.Text != "")
                Ficha_Inspeccion_Negocio.P_Funcionamiento_No_Clientes = Txt_Afluencia.Text;

            //  para los campos de anuncios 
            Ficha_Inspeccion_Negocio.P_Anuncio_1 = Txt_Tipo_Anuncio_1.Text;
            if (Txt_Dimenciones_Anuncion_1.Text != "")
                Ficha_Inspeccion_Negocio.P_Anuncio_1_Dimensiones = Txt_Dimenciones_Anuncion_1.Text;

            Ficha_Inspeccion_Negocio.P_Anuncio_2 = Txt_Tipo_Anuncio_2.Text;
            if (Txt_Dimenciones_Anuncion_2.Text != "")
                Ficha_Inspeccion_Negocio.P_Anuncio_2_Dimensiones = Txt_Dimenciones_Anuncion_2.Text;

            Ficha_Inspeccion_Negocio.P_Anuncio_3 = Txt_Tipo_Anuncio_3.Text;
            if (Txt_Dimenciones_Anuncion_3.Text != "")
                Ficha_Inspeccion_Negocio.P_Anuncio_3_Dimensiones = Txt_Dimenciones_Anuncion_3.Text;

            Ficha_Inspeccion_Negocio.P_Anuncio_4 = Txt_Tipo_Anuncio_4.Text;
            if (Txt_Dimenciones_Anuncion_4.Text != "")
                Ficha_Inspeccion_Negocio.P_Anuncio_4_Dimensiones = Txt_Dimenciones_Anuncion_4.Text;

            //  para los servicios
            Ficha_Inspeccion_Negocio.P_Servicios_Cuenta_Sanitarios = RBtn_Servicios_Sanitarios.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Servicios_WC = RBtn_Wc.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Servicios_Lavabo = RBtn_Lavabo.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Servicios_Letrina = RBtn_Letrina.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Servicios_Mixto = RBtn_Mixto.SelectedValue;
            if (Txt_Bano_Hombres.Text != "")
                Ficha_Inspeccion_Negocio.P_Servicios_Numero_Sanitarios_Hombres = Txt_Bano_Hombres.Text;
            else
                Ficha_Inspeccion_Negocio.P_Servicios_Numero_Sanitarios_Hombres = "0";

            if (Txt_Bano_Mujeres.Text != "")
                Ficha_Inspeccion_Negocio.P_Servicios_Numero_Sanitarios_Mujeres = Txt_Bano_Mujeres.Text;
            else
                Ficha_Inspeccion_Negocio.P_Servicios_Numero_Sanitarios_Mujeres = "0";

            Ficha_Inspeccion_Negocio.P_Servicios_Agua_Potable = RBtn_Agua_Potable.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Servicios_Agua_Abastecimiento_Particular = RBtn_Agua_Potable_Particular.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Servicios_Agua_Abastecimiento_Japami = RBtn_Agua_Potable_Japami.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Servicios_Drenaje = RBtn_Agua_Potable_Drenaje.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Servicios_Fosa_Septica = RBtn_Agua_Potable_Fosa_Septica.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Servicios_Estacionamiento = RBtn_Cuenta_Estacionamiento.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Servicios_Estacionamiento_Propio = RBtn_Cuenta_Estacionamiento_Dentro_Inmueble.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Servicios_Estacionamiento_Rentado = RBtn_Cuenta_Estacionamiento_Rentado.SelectedValue;
            if (Txt_Numero_Cajones.Text != "")
                Ficha_Inspeccion_Negocio.P_Servicios_Estacionamiento_Numero_Cajones = Txt_Numero_Cajones.Text;
            //else
            //    Ficha_Inspeccion_Negocio.P_Servicios_Estacionamiento_Numero_Cajones = "0";

            Ficha_Inspeccion_Negocio.P_Servicios_Estacionamiento_Area_Descarga = RBtn_Cuenta_Estacionamiento_Area_Carga.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Servicios_Estacionamiento_Domicilio = Txt_Domicilio_Estacionamiento.Text;

            //  para los materiales empleados
            Ficha_Inspeccion_Negocio.P_Materiales_Empleado_Muros = Cmb_Material_Muros.SelectedIndex == 0 ? "" : Cmb_Material_Muros.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Materiales_Empleado_Techos = Cmb_Material_Techo.SelectedIndex == 0 ? "" : Cmb_Material_Techo.SelectedValue;

            //  para las medidas de seguridad
            Ficha_Inspeccion_Negocio.P_Seguridad_Medidas = RBtn_Medidas_Seguridad_Senalizacion.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Seguridad_Equipo = RBtn_Medidas_Seguridad_Equipo.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Seguridad_Material_Flamable = RBtn_Medidas_Seguridad_Material_Flamable.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Seguridad_Especificar = Txt_Medidas_Seguridad_Especificar.Text;

            //  para la poda de arboles
            if (Txt_Arbol_Altura.Text != "")
                Ficha_Inspeccion_Negocio.P_Poda_Altura = Txt_Arbol_Altura.Text.Trim();

            if (Txt_Arbol_Diametro.Text != "")
                Ficha_Inspeccion_Negocio.P_Poda_Diametro_Tronco = Txt_Arbol_Diametro.Text;

            if (Txt_Arbol_Diametro_Fronda.Text != "")
                Ficha_Inspeccion_Negocio.P_Poda_Fronda = Txt_Arbol_Diametro_Fronda.Text;

            Ficha_Inspeccion_Negocio.P_Poda_Estado = Txt_Arbol_Estado.Text;

            //  para los campos generales                
            Ficha_Inspeccion_Negocio.P_Generales_Recepcion_Inspector = Convert.ToDateTime(Txt_Generales_Recepcion_Inspector_Fecha.Text);
            Ficha_Inspeccion_Negocio.P_Generales_Fecha_Revision_Campo = Convert.ToDateTime(Txt_Generales_Recepcion_Campo_Fecha.Text);
            Ficha_Inspeccion_Negocio.P_Generales_Recepcion_Coordinacion = Convert.ToDateTime(Txt_Generales_Recepcion_Coordinador_Fecha.Text);
            Ficha_Inspeccion_Negocio.P_Generales_Observaciones_Para_Inspector = Txt_Generales_Observaciones_Para_Inspector.Text;
            Ficha_Inspeccion_Negocio.P_Generales_Observaciones_Inspector = Txt_Generales_Observaciones_Del_Inspector.Text;

            //  para los campos de auditoria
            Ficha_Inspeccion_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;

            //  para los datos del manifiesto de impacto ambiental
            Ficha_Inspeccion_Negocio.P_Impacto_Afectables = Txt_Manifiesto_Afectacion.Text;
            Ficha_Inspeccion_Negocio.P_Impacto_Colindancias = Txt_Manifiesto_Colindancia.Text;
            Ficha_Inspeccion_Negocio.P_Impacto_Superficie = Txt_Manifiesto_Superficie_Total.Text;
            Ficha_Inspeccion_Negocio.P_Impacto_Tipo_Proyecto = Txt_Manifiesto_Tipo_Proyecto.Text;

            // para los datos de licencia
            Ficha_Inspeccion_Negocio.P_Licencia_Tipo_Equipo = Txt_Licencia_Equipo_Emisor.Text;
            Ficha_Inspeccion_Negocio.P_Licencia_Tipo_Emision = Txt_Licencia_Tipo_Emision.Text;
            Ficha_Inspeccion_Negocio.P_Licencia_Horario_Funcionamiento = Txt_Licencia_Hora_Funcionamiento.Text;
            Ficha_Inspeccion_Negocio.P_Licencia_Tipo_Combustible = Txt_Licencia_Tipo_Conbustible.Text;
            Ficha_Inspeccion_Negocio.P_Licencia_Tipo_Gastos_Combustible = Txt_Licencia_Gastos_Combustible.Text;
            Ficha_Inspeccion_Negocio.P_Licencia_Almacenaje = Txt_Licencia_Almacenaje_Combustible.Text;
            Ficha_Inspeccion_Negocio.P_Licencia_Cantidad_Combustible = Txt_Licencia_Cantidad_Combustible.Text;


            //  para los datos del banco de materiales 
            Ficha_Inspeccion_Negocio.P_Material_Permiso_Ecologico = RBtn_Material_Permiso_Ecologia.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Material_Permiso_Suelo = RBtn_Material_Permiso_Suelo.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Material_Superficie_Total = Txt_Materiales_Superficie_Total.Text;
            Ficha_Inspeccion_Negocio.P_Material_Profundidad = Txt_Materiales_Profundidad.Text;
            Ficha_Inspeccion_Negocio.P_Material_Inclinacion = Txt_Materiales_Inclinacion.Text;
            Ficha_Inspeccion_Negocio.P_Material_Flora = Txt_Materiales_Flora.Text;
            Ficha_Inspeccion_Negocio.P_Material_Acceso_Vehiculos = RBtn_Material_Accesibilidad_Vehiculo.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Material_Petreo = Txt_Materiales_Petreo.Text;
            Ficha_Inspeccion_Negocio.P_Material_Especie_Arbol = Txt_Arboles_Especie.Text;

            if (RBtn_Arboles_Tipo_Poda.Checked == true)
            {
                Ficha_Inspeccion_Negocio.P_Material_Tipo_Poda = "SI";
                Ficha_Inspeccion_Negocio.P_Material_Cantidad_Poda = Txt_Arboles_Cantidad_Poda.Text;
            }
            else
                Ficha_Inspeccion_Negocio.P_Material_Tipo_Poda = "NO";

            if (RBtn_Arboles_Tipo_Tala.Checked == true)
            {
                Ficha_Inspeccion_Negocio.P_Material_Tipo_Tala = "SI";
                Ficha_Inspeccion_Negocio.P_Material_Cantidad_Tala = Txt_Arboles_Cantidad_Tala.Text;
            }
            else
                Ficha_Inspeccion_Negocio.P_Material_Tipo_Tala = "NO";

            if (RBtn_Arboles_Tipo_Tala.Checked == true)
            {
                Ficha_Inspeccion_Negocio.P_Material_Tipo_Trasplante = "SI";
                Ficha_Inspeccion_Negocio.P_Material_Cantidad_Trasplante = Txt_Arboles_Cantidad_Trasplante.Text;
            }
            else
                Ficha_Inspeccion_Negocio.P_Material_Tipo_Trasplante = "NO";

            //  para los datos de la autorizacion de aprovechamiento ambiental
            Ficha_Inspeccion_Negocio.P_Autoriza_Suelos = RBtn_Aprovechamiento_Uso_Suelo.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Autoriza_Area_Residuos = RBtn_Aprovechamiento_Almacen_Residuos.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Autoriza_Separacion = RBtn_Aprovechamiento_Existe_Separacion.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Autoriza_Metodo_Separacion = Txt_Aprovechamiento_Metodo_Sepearacion.Text;
            Ficha_Inspeccion_Negocio.P_Autoriza_Servicio_Recoleccion = Txt_Aprovechamiento_Servicio_Recoleccion.Text;
            Ficha_Inspeccion_Negocio.P_Autoriza_Revuelven_Solidos_Liquidos = RBtn_Aprovechamiento_Revuelven_Liquidos.SelectedValue;
            Ficha_Inspeccion_Negocio.P_Autoriza_Tipo_Contenedor = Txt_Aprovechamiento_Tipo_Contenedor.Text;
            Ficha_Inspeccion_Negocio.P_Autoriza_Tipo_Ruido = Txt_Aprovechamiento_Tipo_Ruido.Text;

            if (Txt_Aprovechamiento_Nivel_Ruido.Text == "")
                Txt_Aprovechamiento_Nivel_Ruido.Text = "0";

            Ficha_Inspeccion_Negocio.P_Autoriza_Nivel_Ruido = Txt_Aprovechamiento_Nivel_Ruido.Text;

            if (!String.IsNullOrEmpty(Txt_Aprovechamiento_Horario_Inicial.Text) && !String.IsNullOrEmpty(Txt_Aprovechamiento_Horario_Final.Text))
                Ficha_Inspeccion_Negocio.P_Autoriza_Horario_Labores = Txt_Aprovechamiento_Horario_Inicial.Text + " a " + Txt_Aprovechamiento_Horario_Final.Text;

            if (Chk_Dias_Labor_Lunes.Checked == true)
                Ficha_Inspeccion_Negocio.P_Autoriza_Lunes = "SI";
            else
                Ficha_Inspeccion_Negocio.P_Autoriza_Lunes = "NO";

            if (Chk_Dias_Labor_Martes.Checked == true)
                Ficha_Inspeccion_Negocio.P_Autoriza_Martes = "SI";
            else
                Ficha_Inspeccion_Negocio.P_Autoriza_Martes = "NO";

            if (Chk_Dias_Labor_Miercoles.Checked == true)
                Ficha_Inspeccion_Negocio.P_Autoriza_Miercoles = "SI";
            else
                Ficha_Inspeccion_Negocio.P_Autoriza_Miercoles = "NO";

            if (Chk_Dias_Labor_Jueves.Checked == true)
                Ficha_Inspeccion_Negocio.P_Autoriza_Jueves = "SI";
            else
                Ficha_Inspeccion_Negocio.P_Autoriza_Jueves = "NO";

            if (Chk_Dias_Labor_Viernes.Checked == true)
                Ficha_Inspeccion_Negocio.P_Autoriza_Viernes = "SI";
            else
                Ficha_Inspeccion_Negocio.P_Autoriza_Viernes = "NO";

            if (Chk_Dias_Labor_Sabado.Checked == true)
                Ficha_Inspeccion_Negocio.P_Autoriza_Sabado = "SI";
            else
                Ficha_Inspeccion_Negocio.P_Autoriza_Sabado = "NO";

            if (Chk_Dias_Labor_Domingo.Checked == true)
                Ficha_Inspeccion_Negocio.P_Autoriza_Domingo = "SI";
            else
                Ficha_Inspeccion_Negocio.P_Autoriza_Domingo = "NO";

            if (Txt_Aprovechamiento_Emisiones_Atmosfera.Text == "")
                Txt_Aprovechamiento_Emisiones_Atmosfera.Text = "0";

            Ficha_Inspeccion_Negocio.P_Autoriza_Emisiones = Txt_Aprovechamiento_Emisiones_Atmosfera.Text;

            //  para los elementos de residuos peligrosos
            Ficha_Inspeccion_Negocio.P_Dt_Residuos = (DataTable)Session["Dt_Tipo_Residuo"];

            if (Ficha_Inspeccion_Negocio.P_Dt_Residuos != null)
            {
                //  se ordenara la tabla por ID
                DataView Dv_Ordenar = new DataView(Ficha_Inspeccion_Negocio.P_Dt_Residuos);
                Dv_Ordenar.Sort = "TIPO_RESIDUO_ID asc";//SOLICITO asc, 
                Ficha_Inspeccion_Negocio.P_Dt_Residuos = Dv_Ordenar.ToTable();
            }

            Ficha_Inspeccion_Negocio.Modificar_Formato();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tramites", "alert('Alta de formato exitosa');", true);
            Inicializar_Controles();
            if (Grid_Formatos.Rows.Count > 0)
            {
                Grid_Formatos.SelectedIndex = -1;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Listado
        ///DESCRIPCIÓN         : Llena el Listado con una consulta que puede o no
        ///                      tener Filtros.
        ///PARÁMETROS          : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        private void Llenar_Grid_Listado(int Pagina)
        {
            try
            {
                Grid_Listado.SelectedIndex = (-1);
                Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
                Grid_Listado.Columns[1].Visible = true;
                Grid_Listado.Columns[2].Visible = true;
                Grid_Listado.Columns[3].Visible = true;
                Grid_Listado.Columns[4].Visible = true;

                Negocio.P_Empleado_Id = Cls_Sessiones.Empleado_ID;
                DataTable Dt_Zonas = Negocio.Consultar_Zonas();
                // si el usuario autenticado es el responsable de la zona
                Negocio.P_Area_Zona = Dt_Zonas.Rows.Count > 0 ? Dt_Zonas.Rows[0][Cat_Ort_Zona.Campo_Zona_ID].ToString() : "-";

                Grid_Listado.DataSource = Negocio.Consultar_Tabla_Administracion_Urbana(Negocio);
                Grid_Listado.PageIndex = Pagina;
                Grid_Listado.DataBind();
                Grid_Listado.Columns[1].Visible = false;
                Grid_Listado.Columns[2].Visible = false;
                Grid_Listado.Columns[3].Visible = false;
                Grid_Listado.Columns[4].Visible = false;
                Pnl_Modificar_Cedula.Style.Value = "display: block";
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Solicitud_Cedula
        ///DESCRIPCIÓN          : se cargara el combo con los distintos areas de donacion y via publica
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 20/Septiembre/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Solicitud()
        {
            Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Negocio_Datos_Solicitud.P_Solicitud_ID = Session["Solicitud_Id"].ToString();
                Negocio_Datos_Solicitud = Negocio_Datos_Solicitud.Consultar_Datos_Solicitud();

                Hdf_Solicitud_ID.Value = Negocio_Datos_Solicitud.P_Solicitud_ID;
                Hdf_Tramite_ID.Value = Negocio_Datos_Solicitud.P_Tramite_id;
                Hdf_Subproceso_ID.Value = Negocio_Datos_Solicitud.P_Subproceso_ID;

                Txt_Nombre.Text = (Negocio_Datos_Solicitud.P_Solicito != "") ? Negocio_Datos_Solicitud.P_Solicito : "";
                Txt_Estatus.Text = (Negocio_Datos_Solicitud.P_Estatus != "") ? Negocio_Datos_Solicitud.P_Estatus : "";
                Txt_Proceso.Text = (Negocio_Datos_Solicitud.P_Subproceso_Nombre != "") ? Negocio_Datos_Solicitud.P_Subproceso_Nombre : "";
                Txt_Avance.Text = Convert.ToString(Negocio_Datos_Solicitud.P_Porcentaje_Avance) + " %";

                //  se llenan las cajas de texto de consecutivo y cuenta predial
                Txt_Consecutivo_ID.Text = Negocio_Datos_Solicitud.P_Consecutivo;

                if (Negocio_Datos_Solicitud.P_Cuenta_Predial.ToString() != "")
                    Txt_Cuenta_Predial.Text = Negocio_Datos_Solicitud.P_Cuenta_Predial;
                else
                {
                    Lbl_Cuenta_Predial.Visible = false;
                    Txt_Cuenta_Predial.Visible = false;
                    Btn_Buscar_Cuenta_Predial.Visible = false;
                    Tabla_Datos_Calle.Style.Value = "display:block";
                    Tabla_Calle.Style.Value = "display:block";
                }

                //  para cargar la zona
                Cmb_Zona.SelectedIndex = Cmb_Zona.Items.IndexOf(Cmb_Zona.Items.FindByValue(Negocio_Datos_Solicitud.P_Zona_ID));

                //  para cargar el tipo de supervision
                Cmb_Tipo_Supervision.SelectedIndex = Cmb_Tipo_Supervision.Items.IndexOf(Cmb_Tipo_Supervision.Items.FindByText(Negocio_Datos_Solicitud.P_Tramite));

                //  para cargar el inspector
                Cmb_Inspector.SelectedIndex = Cmb_Inspector.Items.IndexOf(Cmb_Inspector.Items.FindByValue(Negocio_Datos_Solicitud.P_Persona_Inspecciona));

                //  para el evento de resumen de predio
                Cargar_Ventana_Emergente_Resumen_Predio();

                Div_Lista_Formatos.Style.Value = "display:none";
                Div_Principal_Llenado_Formato.Style.Value = "display:block";
                Btn_Nuevo.Visible = true;
                Habilitar_Controles("Nuevo");



                //  CODIGO PARA CONSULTAR DATOS FINALES
                DataTable Dt_Consulta_Dato_Final = Negocio_Datos_Solicitud.Consultar_Datos_Finales();
                //  para cuando tenga registros para el dictamen
                if (Dt_Consulta_Dato_Final != null && Dt_Consulta_Dato_Final.Rows.Count > 0)
                {
                    DataTable Dt_Consulta_Dato_Final_Llenado = Negocio_Datos_Solicitud.Consultar_Datos_Finales_Operacion();
                    //Grid_Datos_Dictamen_Modificar.DataSource = Dt_Consulta_Dato_Final_Llenado;
                    //Grid_Datos_Dictamen_Modificar.DataBind();
                    Cargar_Grid_Datos_Dictamen_Modificar(Dt_Consulta_Dato_Final_Llenado);
                    Grid_Datos_Dictamen.Style.Value = "display:none";
                }
                else
                {
                    Cls_Ope_Solicitud_Tramites_Negocio Negocio_Consultar_Datos_Dictamen = new Cls_Ope_Solicitud_Tramites_Negocio();
                    Negocio_Consultar_Datos_Dictamen.P_Tramite_ID = Negocio_Datos_Solicitud.P_Tramite_id;
                    Negocio_Consultar_Datos_Dictamen.P_Tipo_Dato = "FINAL";
                    DataSet Ds_Datos = new DataSet();
                    Ds_Datos = Negocio_Consultar_Datos_Dictamen.Consultar_Datos_Tramite();

                    //  se ordenara la tabla por fecha
                    DataView Dv_Ordenar = new DataView(Ds_Datos.Tables[0]);
                    Dv_Ordenar.Sort = Cat_Tra_Datos_Tramite.Campo_Dato_ID;//SOLICITO asc, 
                    DataTable Dt_Datos_Ordenados = Dv_Ordenar.ToTable();
                    Grid_Datos_Dictamen.DataSource = Dt_Datos_Ordenados;
                    Grid_Datos_Dictamen.DataBind();
                    Grid_Datos_Dictamen_Modificar.Style.Value = "display:none";
                    Session["Grid_Datos"] = Dt_Datos_Ordenados;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cargar_Combo_Areas_Donacion " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Datos_Ficha
        ///DESCRIPCIÓN         : Muestra los valores del elemento seleccionado.
        ///PARÁMETROS          : 
        ///CREO                : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO          : 10/Septiembre/2012 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        private void Mostrar_Datos_Ficha()
        {
            Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
            DataTable Dt_Admon_Urbana = new DataTable();

            try
            {
                Negocio.P_Administracion_Urbana_ID = Hdf_Administracion_Urbana_ID.Value;
                Negocio.P_Tramite_ID = Hdf_Tramite_ID.Value;
                Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                Negocio.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
                Dt_Admon_Urbana = Negocio.Consultar_Administracion_Urbana(Negocio);
                Dt_Admon_Urbana.TableName = "Dt_Admon_Urbana";

                if (Dt_Admon_Urbana != null && Dt_Admon_Urbana.Rows.Count > 0)
                {
                    Div_Principal_Llenado_Formato.Style.Value = "display:block";
                    Div_Lista_Formatos.Style.Value = "display:none";
                    Cargar_Combo_Zona();
                    Cargar_Combo_Tipo_Supervision();
                    Cargar_Combo_Condiciones_Inmueble();
                    Cargar_Combo_Condiciones_Avance();
                    //Cargar_Combo_Areas_Donacion();
                    Cargar_Combo_Areas_Uso_Actual();
                    Cargar_Combo_Funcionamiento();
                    Cargar_Combo_Materiales();
                    Cargar_Combo_Inspectores();
                    Cargar_Combo_Tipo_Residuos();
                    Cargar_Combo_Colonias();
                    Cargar_Combo_Calles(new DataTable());

                    //Txt_Consecutivo_ID.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID].ToString();
                    Txt_Destinado.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Area_Uso_Solicitado].ToString();
                    Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                    Negocio_Datos_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                    Negocio_Datos_Solicitud = Negocio_Datos_Solicitud.Consultar_Datos_Solicitud();
                    Txt_Consecutivo_ID.Text = Negocio_Datos_Solicitud.P_Consecutivo;
                    Txt_Nombre.Text = (Negocio_Datos_Solicitud.P_Solicito != "") ? Negocio_Datos_Solicitud.P_Solicito : "";
                    Txt_Estatus.Text = (Negocio_Datos_Solicitud.P_Estatus != "") ? Negocio_Datos_Solicitud.P_Estatus : "";
                    Txt_Proceso.Text = (Negocio_Datos_Solicitud.P_Subproceso_Nombre != "") ? Negocio_Datos_Solicitud.P_Subproceso_Nombre : "";
                    Txt_Avance.Text = Convert.ToString(Negocio_Datos_Solicitud.P_Porcentaje_Avance) + " %";


                    RBtn_Area_Inspeccion.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Area_Inspeccion].ToString() == "URBANISTICO" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Area_Inspeccion].ToString() == "INMOBILIARIO" ? 1 : -1;
                    Txt_Cuenta_Predial.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Cuenta_Predial].ToString();
                    Cmb_Tipo_Supervision.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Tipo_Supervision_ID].ToString();
                    Cmb_Inspector.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspector_ID].ToString();
                    Cmb_Zona.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Area_Zona].ToString();

                    Cmb_Condiciones_Inmueble.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Condiciones_Inmueble].ToString();
                    //Cmb_Via_Publica_Area_Donacion.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Area_Via_ID].ToString();
                    Chk_Construccion_Marquesina.Checked = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Sobre_Marquesina].ToString() == "SI" ? true : false;
                    Chk_Invasion_Areas_donacion.Checked = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Invasion_Donacion].ToString() == "SI" ? true : false;
                    Chk_Invasion_Material.Checked = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Invasion_Material].ToString() == "SI" ? true : false;
                    Chk_Sobresale_Paramento.Checked = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Paramento].ToString() == "SI" ? true : false;

                    Txt_Especificacion_Restriccion.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Area_Via_Especificar_Restriccion].ToString();
                    Txt_Usos_Colindantes.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Predominante_Colindantes].ToString();
                    Txt_Usos_Frente_Inmueble.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Predominante_Frente_Inmueble].ToString();
                    Txt_Usos_Cercanos_Riesgo.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Predominante_Impacto_Considarar].ToString();
                    Cmb_Avance_Obra.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Obra_ID].ToString();
                    Txt_Avance_Obra_Aproximado.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Bardeo_Aproximado].ToString();
                    Txt_Avance_Obra_Niveles_Actuales.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Niveles_Actuales].ToString();
                    Txt_Avance_Obra_Niveles_Construir.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Avances_Niveles_Construccion].ToString();
                    RBtn_Avance_Obra_Acorde_Solicitado.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Proyecto_Acorde_Solicitado].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Proyecto_Acorde_Solicitado].ToString() == "NO" ? 1 : -1;
                    RBtn_Notificado.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Notificacion].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Notificacion].ToString() == "NO" ? 1 : -1;
                    Txt_Notificacion_Folio.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Notificacion_Folio].ToString();
                    RBtn_Acta_Inspeccion.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Acta].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Acta].ToString() == "NO" ? 1 : -1;
                    Txt_Acta_Inspeccion_Folio.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Acta_Folio].ToString();
                    RBtn_Clausurado.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Clausurado].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Clausurado].ToString() == "NO" ? 1 : -1;
                    Txt_Clausurado_Folio.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Clausurado_Folio].ToString();
                    RBtn_Multado.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Multado].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Multado].ToString() == "NO" ? 1 : -1;
                    Txt_Multa_Folio.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Multado_Folio].ToString();
                    Cmb_Uso_Actual.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Actual_ID].ToString();
                    RBtn_Uso_Diferente_Adicional.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Acorde_Solicitado].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Acorde_Solicitado].ToString() == "NO" ? 1 : -1;
                    Txt_Especificar_Tipo_Uso.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Especificar_Tipo_Uso].ToString();
                    Txt_Area_Acticidad.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Actividad].ToString();
                    Txt_Superficie_Metros2.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Metros].ToString();
                    Txt_Maquinaria_Utilizar.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Maquinaria].ToString();
                    Cmb_Funcionamiento.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_ID].ToString();
                    Txt_Cantidad_Personas_Laboran.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Numero_Personal].ToString();
                    Txt_Afluencia.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Numero_Clientes].ToString();
                    Txt_Tipo_Anuncio_1.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_1].ToString();
                    Txt_Dimenciones_Anuncion_1.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_1].ToString();
                    Txt_Tipo_Anuncio_2.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_2].ToString();
                    Txt_Dimenciones_Anuncion_2.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_2].ToString();
                    Txt_Tipo_Anuncio_3.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_3].ToString();
                    Txt_Dimenciones_Anuncion_3.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_3].ToString();
                    Txt_Tipo_Anuncio_4.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_4].ToString();
                    Txt_Dimenciones_Anuncion_4.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_4].ToString();
                    RBtn_Servicios_Sanitarios.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Cuenta].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Cuenta].ToString() == "NO" ? 1 : -1;
                    RBtn_Wc.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_WC].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_WC].ToString() == "NO" ? 1 : -1;
                    RBtn_Lavabo.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Lavabo].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Lavabo].ToString() == "NO" ? 1 : -1;
                    RBtn_Letrina.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Letrina].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Letrina].ToString() == "NO" ? 1 : -1;
                    RBtn_Mixto.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Mixto].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Mixto].ToString() == "NO" ? 1 : -1;
                    Txt_Bano_Hombres.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Numero_Sanitarios_Hombres].ToString();
                    Txt_Bano_Mujeres.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Numero_Sanitarios_Mujeres].ToString();
                    Txt_Total_Banos.Text = "" + (
                        Convert.ToInt32(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Numero_Sanitarios_Hombres].ToString()) +
                        Convert.ToInt32(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Numero_Sanitarios_Mujeres].ToString())).ToString();
                    RBtn_Agua_Potable.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Potable].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Potable].ToString() == "NO" ? 1 : -1;
                    RBtn_Agua_Potable_Particular.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Abast_Particular].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Abast_Particular].ToString() == "NO" ? 1 : -1;
                    RBtn_Agua_Potable_Japami.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Abast_Japami].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Abast_Japami].ToString() == "NO" ? 1 : -1;
                    RBtn_Agua_Potable_Drenaje.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Drenaje].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Drenaje].ToString() == "NO" ? 1 : -1;
                    RBtn_Agua_Potable_Fosa_Septica.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Fosa_Septica].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Fosa_Septica].ToString() == "NO" ? 1 : -1;
                    RBtn_Cuenta_Estacionamiento.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento].ToString() == "NO" ? 1 : -1;
                    RBtn_Cuenta_Estacionamiento_Dentro_Inmueble.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Propio].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Propio].ToString() == "NO" ? 1 : -1;
                    RBtn_Cuenta_Estacionamiento_Rentado.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Rentado].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Rentado].ToString() == "NO" ? 1 : -1;
                    Txt_Numero_Cajones.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Numero_Cajones].ToString();
                    RBtn_Cuenta_Estacionamiento_Area_Carga.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Area_Descarga].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Area_Descarga].ToString() == "NO" ? 1 : -1;
                    Txt_Domicilio_Estacionamiento.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Domicilio].ToString();
                    Cmb_Material_Muros.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Material_Empleado_Muros].ToString();
                    Cmb_Material_Techo.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Material_Empleado_Techo].ToString();
                    RBtn_Medidas_Seguridad_Senalizacion.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Medidas].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Medidas].ToString() == "NO" ? 1 : -1;
                    RBtn_Medidas_Seguridad_Equipo.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Equipo].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Equipo].ToString() == "NO" ? 1 : -1;
                    RBtn_Medidas_Seguridad_Material_Flamable.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Material_Flamable].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Material_Flamable].ToString() == "NO" ? 1 : -1;
                    Txt_Medidas_Seguridad_Especificar.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Especificar].ToString();
                    Txt_Arbol_Altura.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Altura].ToString();
                    Txt_Arbol_Diametro.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Diametro_Tronco].ToString();
                    Txt_Arbol_Diametro_Fronda.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Diametro_Fronda].ToString();
                    Txt_Arbol_Estado.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Estado].ToString();
                    RBtn_Material_Permiso_Ecologia.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Permiso_Ecologia].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Permiso_Ecologia].ToString() == "NO" ? 1 : -1;
                    RBtn_Material_Permiso_Suelo.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Permiso_Suelo].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Permiso_Suelo].ToString() == "NO" ? 1 : -1;


                    if (Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Tipo_Poda].ToString() == "SI")
                    {
                        RBtn_Arboles_Tipo_Poda.Checked =true;
                        Txt_Arboles_Cantidad_Poda.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Cantidad_Poda].ToString();
                    }
                    if (Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Tipo_Tala].ToString() == "SI")
                    {
                        RBtn_Arboles_Tipo_Tala.Checked = true;
                        Txt_Arboles_Cantidad_Tala.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Cantidad_Tala].ToString();
                    }
                    if (Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Tipo_Trasplante].ToString() == "SI")
                    {
                        RBtn_Arboles_Tipo_Trasplante.Checked = true;
                        Txt_Arboles_Cantidad_Trasplante.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Cantidad_Trasplante].ToString();
                    }

                    Txt_Arboles_Especie.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Arboles_Especie].ToString();
                    Txt_Materiales_Superficie_Total.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Superficie_Total].ToString();
                    Txt_Materiales_Profundidad.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Profundidad].ToString();
                    Txt_Materiales_Inclinacion.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Inclinacion].ToString();
                    Txt_Materiales_Flora.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Flora].ToString();
                    Txt_Materiales_Petreo.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Petreo].ToString();
                    RBtn_Material_Accesibilidad_Vehiculo.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Acceso_Vehiculoas].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Acceso_Vehiculoas].ToString() == "NO" ? 1 : -1;
                    Txt_Manifiesto_Afectacion.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Afectaciones].ToString();
                    Txt_Manifiesto_Colindancia.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Colindancias].ToString();
                    Txt_Manifiesto_Superficie_Total.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Superficie].ToString();
                    Txt_Manifiesto_Tipo_Proyecto.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Tipo_Proyecto].ToString();
                    Txt_Licencia_Equipo_Emisor.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Equipo_Emisor].ToString();
                    Txt_Licencia_Tipo_Emision.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Emision].ToString();
                    if (!String.IsNullOrEmpty(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Horario_Funcionamiento].ToString()))
                        Txt_Licencia_Hora_Funcionamiento.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Horario_Funcionamiento].ToString().Substring(0, 13);
                    Txt_Licencia_Tipo_Conbustible.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Conbustible].ToString();
                    Txt_Licencia_Gastos_Combustible.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Gasto_Combustible].ToString();

                    Txt_Licencia_Almacenaje_Combustible.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Almacenaje].ToString();
                    Txt_Licencia_Cantidad_Combustible.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Cantidad_Combustible].ToString();


                    RBtn_Aprovechamiento_Uso_Suelo.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Suelos].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Suelos].ToString() == "NO" ? 1 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Suelos].ToString() == "En tramite" ? 2 : -1;
                    RBtn_Aprovechamiento_Almacen_Residuos.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Area_Residuos].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Area_Residuos].ToString() == "NO" ? 1 : -1;
                    RBtn_Aprovechamiento_Existe_Separacion.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Separacion].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Separacion].ToString() == "NO" ? 1 : -1;

                    Negocio.P_Administracion_Urbana_ID = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Administracion_Urbana_ID].ToString();
                    DataTable Dt_Residuos = Negocio.Consultar_Residuos();
                    Grid_Tipos_Residuos.Columns[1].Visible = true;
                    Grid_Tipos_Residuos.DataSource = Dt_Residuos;
                    Grid_Tipos_Residuos.DataBind();
                    Grid_Tipos_Residuos.Columns[1].Visible = false;
                    Grid_Tipos_Residuos.Columns[3].Visible = false;
                    Session["Dt_Tipo_Residuo"] = Dt_Residuos;
                    Btn_Agregar_Tipo_Residuo.Visible = false;

                    Txt_Aprovechamiento_Metodo_Sepearacion.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Metodo_Separacion].ToString();
                    Txt_Aprovechamiento_Servicio_Recoleccion.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Servicio_Recoloccion].ToString();
                    RBtn_Aprovechamiento_Revuelven_Liquidos.SelectedIndex =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Revuelven_Solidos_Liquidos].ToString() == "SI" ? 0 :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Revuelven_Solidos_Liquidos].ToString() == "NO" ? 1 : -1;
                    Txt_Aprovechamiento_Tipo_Contenedor.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Tipo_Contenedor].ToString();
                    Txt_Aprovechamiento_Tipo_Ruido.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Tipo_Ruido].ToString();
                    Txt_Aprovechamiento_Nivel_Ruido.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Nivel_Ruido].ToString();
                    
                    
                    if (!String.IsNullOrEmpty(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Horario_Labores].ToString()))
                    {
                        Txt_Aprovechamiento_Horario_Inicial.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Horario_Labores].ToString().Substring(0, 13);
                        Txt_Aprovechamiento_Horario_Final.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Horario_Labores].ToString().Substring(16, 13);
                    }
                    Chk_Dias_Labor_Lunes.Checked =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Lunes].ToString() == "SI" ? true :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Lunes].ToString() == "NO" ? false : false;
                    Chk_Dias_Labor_Martes.Checked =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Martes].ToString() == "SI" ? true :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Martes].ToString() == "NO" ? false : false;
                    Chk_Dias_Labor_Miercoles.Checked =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Miercoles].ToString() == "SI" ? true :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Miercoles].ToString() == "NO" ? false : false;
                    Chk_Dias_Labor_Jueves.Checked =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Jueves].ToString() == "SI" ? true :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Jueves].ToString() == "NO" ? false : false;
                    Chk_Dias_Labor_Viernes.Checked =
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Viernes].ToString() == "SI" ? true :
                        Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Viernes].ToString() == "NO" ? false : false;

                    Chk_Dias_Labor_Sabado.Checked =
                      Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Sabado].ToString() == "SI" ? true :
                      Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Sabado].ToString() == "NO" ? false : false;
                    Chk_Dias_Labor_Domingo.Checked =
                      Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Domingo].ToString() == "SI" ? true :
                      Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Domingo].ToString() == "NO" ? false : false;

                    Txt_Aprovechamiento_Emisiones_Atmosfera.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Emisiones].ToString();
                    Txt_Generales_Recepcion_Inspector_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Recepcion_Inspector].ToString()));
                    Txt_Generales_Recepcion_Campo_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Fecha_Realizada_Campo].ToString()));
                    Txt_Generales_Recepcion_Coordinador_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Recepcion_Coordinacion].ToString()));
                    Txt_Generales_Observaciones_Para_Inspector.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Observaciones_Para_Insepctor].ToString();
                    Txt_Generales_Observaciones_Del_Inspector.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Observaciones_Del_Insepctor].ToString();

                }

                if (Session["Cedula_Solicitud_Id"] != null)
                    Btn_Modificar.Visible = false;

                Grid_Listado.Style.Value = "display:none";
                System.Threading.Thread.Sleep(500);

            }
            catch (Exception ex)
            {
                throw new Exception("Mostrar_Datos_Ficha: " + ex.Message.ToString(), ex);
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Datos
        ///DESCRIPCIÓN         : Muestra los valores del elemento seleccionado.
        ///PARÁMETROS          : 
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        private void Mostrar_Datos()
        {
            if (Grid_Listado.SelectedIndex > (-1) && Hdf_Administracion_Urbana_ID.Value.ToString().Length != 0)
            {
                Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
                DataTable Dt_Admon_Urbana = new DataTable();
                
                try
                {
                    Negocio.P_Administracion_Urbana_ID = Hdf_Administracion_Urbana_ID.Value;
                    Negocio.P_Tramite_ID = Hdf_Tramite_ID.Value;
                    Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                    Negocio.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
                    Dt_Admon_Urbana = Negocio.Consultar_Administracion_Urbana(Negocio);
                    Dt_Admon_Urbana.TableName = "Dt_Admon_Urbana";

                    if (Dt_Admon_Urbana.Rows.Count >= 0)
                    {
                        Div_Principal_Llenado_Formato.Style.Value = "display:block";
                        Cargar_Combo_Zona();
                        Cargar_Combo_Tipo_Supervision();
                        Cargar_Combo_Condiciones_Inmueble();
                        Cargar_Combo_Condiciones_Avance();
                        //Cargar_Combo_Areas_Donacion();
                        Cargar_Combo_Areas_Uso_Actual();
                        Cargar_Combo_Funcionamiento();
                        Cargar_Combo_Materiales();
                        Cargar_Combo_Inspectores();
                        Cargar_Combo_Tipo_Residuos();
                        Cargar_Combo_Colonias();
                        Cargar_Combo_Calles(new DataTable());

                        Txt_Consecutivo_ID.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID].ToString();
                        Txt_Destinado.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Area_Uso_Solicitado].ToString();
                        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                        Negocio_Datos_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        Negocio_Datos_Solicitud = Negocio_Datos_Solicitud.Consultar_Datos_Solicitud();
                        Txt_Nombre.Text = (Negocio_Datos_Solicitud.P_Solicito != "") ? Negocio_Datos_Solicitud.P_Solicito : "";
                        Txt_Estatus.Text = (Negocio_Datos_Solicitud.P_Estatus != "") ? Negocio_Datos_Solicitud.P_Estatus : "";
                        Txt_Proceso.Text = (Negocio_Datos_Solicitud.P_Subproceso_Nombre != "") ? Negocio_Datos_Solicitud.P_Subproceso_Nombre : "";
                        Txt_Avance.Text = Convert.ToString(Negocio_Datos_Solicitud.P_Porcentaje_Avance) + " %";


                        RBtn_Area_Inspeccion.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Area_Inspeccion].ToString() == "URBANISTICO" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Area_Inspeccion].ToString() == "INMOBILIARIO" ? 1 : -1;
                        Txt_Cuenta_Predial.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Cuenta_Predial].ToString();
                        Cmb_Tipo_Supervision.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Tipo_Supervision_ID].ToString();
                        Cmb_Inspector.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspector_ID].ToString();
                        Cmb_Zona.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Area_Zona].ToString();

                        Cmb_Condiciones_Inmueble.SelectedIndex = Cmb_Condiciones_Inmueble.Items.IndexOf(Cmb_Condiciones_Inmueble.Items.FindByValue(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Condiciones_Inmueble].ToString()));
                        //Cmb_Via_Publica_Area_Donacion.SelectedValue = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Area_Via_ID].ToString();
                        Chk_Construccion_Marquesina.Checked = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Sobre_Marquesina].ToString() == "SI" ? true : false;
                        Chk_Invasion_Areas_donacion.Checked = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Invasion_Donacion].ToString() == "SI" ? true : false;
                        Chk_Invasion_Material.Checked = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Invasion_Material].ToString() == "SI" ? true : false;
                        Chk_Sobresale_Paramento.Checked = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Paramento].ToString() == "SI" ? true : false;

                        Txt_Especificacion_Restriccion.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Area_Via_Especificar_Restriccion].ToString();
                        Txt_Usos_Colindantes.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Predominante_Colindantes].ToString();
                        Txt_Usos_Frente_Inmueble.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Predominante_Frente_Inmueble].ToString();
                        Txt_Usos_Cercanos_Riesgo.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Predominante_Impacto_Considarar].ToString();
                        Cmb_Avance_Obra.SelectedIndex = Cmb_Avance_Obra.Items.IndexOf(Cmb_Avance_Obra.Items.FindByValue(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Obra_ID].ToString()));
                        Txt_Avance_Obra_Aproximado.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Bardeo_Aproximado].ToString();
                        Txt_Avance_Obra_Niveles_Actuales.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Niveles_Actuales].ToString();
                        Txt_Avance_Obra_Niveles_Construir.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Avances_Niveles_Construccion].ToString();
                        RBtn_Avance_Obra_Acorde_Solicitado.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Proyecto_Acorde_Solicitado].ToString() == "SI" ? 0 : 
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Proyecto_Acorde_Solicitado].ToString() == "NO" ? 1 : -1;
                        RBtn_Notificado.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Notificacion].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Notificacion].ToString() == "NO" ? 1 : -1;
                        Txt_Notificacion_Folio.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Notificacion_Folio].ToString();
                        RBtn_Acta_Inspeccion.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Acta].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Acta].ToString() == "NO" ? 1 : -1;
                        Txt_Acta_Inspeccion_Folio.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Acta_Folio].ToString();
                        RBtn_Clausurado.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Clausurado].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Clausurado].ToString() == "NO" ? 1 : -1;
                        Txt_Clausurado_Folio.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Clausurado_Folio].ToString();
                        RBtn_Multado.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Multado].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Multado].ToString() == "NO" ? 1 : -1;
                        Txt_Multa_Folio.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Multado_Folio].ToString();
                        Cmb_Uso_Actual.SelectedIndex = Cmb_Uso_Actual.Items.IndexOf(Cmb_Uso_Actual.Items.FindByValue(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Actual_ID].ToString()));
                        RBtn_Uso_Diferente_Adicional.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Acorde_Solicitado].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Acorde_Solicitado].ToString() == "NO" ? 1 : -1;
                        Txt_Especificar_Tipo_Uso.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Especificar_Tipo_Uso].ToString();
                        Txt_Area_Acticidad.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Actividad].ToString();
                        Txt_Superficie_Metros2.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Metros].ToString();
                        Txt_Maquinaria_Utilizar.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Maquinaria].ToString();
                        Cmb_Funcionamiento.SelectedIndex = Cmb_Funcionamiento.Items.IndexOf(Cmb_Funcionamiento.Items.FindByValue(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_ID].ToString()));
                        Txt_Cantidad_Personas_Laboran.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Numero_Personal].ToString();
                        Txt_Afluencia.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Numero_Clientes].ToString();
                        Txt_Tipo_Anuncio_1.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_1].ToString();
                        Txt_Dimenciones_Anuncion_1.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_1].ToString();
                        Txt_Tipo_Anuncio_2.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_2].ToString();
                        Txt_Dimenciones_Anuncion_2.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_2].ToString();
                        Txt_Tipo_Anuncio_3.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_3].ToString();
                        Txt_Dimenciones_Anuncion_3.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_3].ToString();
                        Txt_Tipo_Anuncio_4.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_4].ToString();
                        Txt_Dimenciones_Anuncion_4.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_4].ToString();
                        RBtn_Servicios_Sanitarios.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Cuenta].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Cuenta].ToString() == "NO" ? 1 : -1;
                        RBtn_Wc.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_WC].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_WC].ToString() == "NO" ? 1 : -1;
                        RBtn_Lavabo.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Lavabo].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Lavabo].ToString() == "NO" ? 1 : -1;
                        RBtn_Letrina.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Letrina].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Letrina].ToString() == "NO" ? 1 : -1;
                        RBtn_Mixto.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Mixto].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Mixto].ToString() == "NO" ? 1 : -1;
                        Txt_Bano_Hombres.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Numero_Sanitarios_Hombres].ToString();
                        Txt_Bano_Mujeres.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Numero_Sanitarios_Mujeres].ToString();
                        Txt_Total_Banos.Text = (
                            Convert.ToInt32(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Numero_Sanitarios_Hombres].ToString()) +
                            Convert.ToInt32(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Numero_Sanitarios_Mujeres].ToString())).ToString();
                        RBtn_Agua_Potable.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Potable].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Potable].ToString() == "NO" ? 1 : -1;
                        RBtn_Agua_Potable_Particular.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Abast_Particular].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Abast_Particular].ToString() == "NO" ? 1 : -1;
                        RBtn_Agua_Potable_Japami.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Abast_Japami].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Abast_Japami].ToString() == "NO" ? 1 : -1;
                        RBtn_Agua_Potable_Drenaje.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Drenaje].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Drenaje].ToString() == "NO" ? 1 : -1;
                        RBtn_Agua_Potable_Fosa_Septica.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Fosa_Septica].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Fosa_Septica].ToString() == "NO" ? 1 : -1;
                        RBtn_Cuenta_Estacionamiento.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento].ToString() == "NO" ? 1 : -1;
                        RBtn_Cuenta_Estacionamiento_Dentro_Inmueble.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Propio].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Propio].ToString() == "NO" ? 1 : -1;
                        RBtn_Cuenta_Estacionamiento_Rentado.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Rentado].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Rentado].ToString() == "NO" ? 1 : -1;
                        Txt_Numero_Cajones.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Numero_Cajones].ToString();
                        RBtn_Cuenta_Estacionamiento_Area_Carga.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Area_Descarga].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Area_Descarga].ToString() == "NO" ? 1 : -1;
                        Txt_Domicilio_Estacionamiento.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Domicilio].ToString();
                        Cmb_Material_Muros.SelectedIndex = Cmb_Material_Muros.Items.IndexOf(Cmb_Material_Muros.Items.FindByValue(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Material_Empleado_Muros].ToString()));
                        Cmb_Material_Techo.SelectedIndex = Cmb_Material_Techo.Items.IndexOf(Cmb_Material_Techo.Items.FindByValue(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Material_Empleado_Techo].ToString()));
                        RBtn_Medidas_Seguridad_Senalizacion.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Medidas].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Medidas].ToString() == "NO" ? 1 : -1;
                        RBtn_Medidas_Seguridad_Equipo.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Equipo].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Equipo].ToString() == "NO" ? 1 : -1;
                        RBtn_Medidas_Seguridad_Material_Flamable.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Material_Flamable].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Material_Flamable].ToString() == "NO" ? 1 : -1;
                        Txt_Medidas_Seguridad_Especificar.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Especificar].ToString();
                        Txt_Arbol_Altura.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Altura].ToString();
                        Txt_Arbol_Diametro.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Diametro_Tronco].ToString();
                        Txt_Arbol_Diametro_Fronda.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Diametro_Fronda].ToString();
                        Txt_Arbol_Estado.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Estado].ToString();
                        RBtn_Material_Permiso_Ecologia.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Permiso_Ecologia].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Permiso_Ecologia].ToString() == "NO" ? 1 : -1;
                        RBtn_Material_Permiso_Suelo.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Permiso_Suelo].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Permiso_Suelo].ToString() == "NO" ? 1 : -1;
                        Txt_Materiales_Superficie_Total.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Superficie_Total].ToString();
                        Txt_Materiales_Profundidad.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Profundidad].ToString();
                        Txt_Materiales_Inclinacion.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Inclinacion].ToString();
                        Txt_Materiales_Flora.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Flora].ToString();
                        Txt_Materiales_Petreo.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Petreo].ToString();
                        RBtn_Material_Accesibilidad_Vehiculo.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Acceso_Vehiculoas].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Acceso_Vehiculoas].ToString() == "NO" ? 1 : -1;
                        Txt_Manifiesto_Afectacion.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Afectaciones].ToString();
                        Txt_Manifiesto_Colindancia.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Colindancias].ToString();
                        Txt_Manifiesto_Superficie_Total.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Superficie].ToString();
                        Txt_Manifiesto_Tipo_Proyecto.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Tipo_Proyecto].ToString();
                        Txt_Licencia_Equipo_Emisor.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Equipo_Emisor].ToString();
                        Txt_Licencia_Tipo_Emision.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Emision].ToString();
                        if (!String.IsNullOrEmpty(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Horario_Funcionamiento].ToString()))
                            Txt_Licencia_Hora_Funcionamiento.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Horario_Funcionamiento].ToString().Substring(0, 13);
                        Txt_Licencia_Tipo_Conbustible.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Conbustible].ToString();
                        Txt_Licencia_Gastos_Combustible.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Gasto_Combustible].ToString();
                        RBtn_Aprovechamiento_Uso_Suelo.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Suelos].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Suelos].ToString() == "NO" ? 1 : -1;
                        RBtn_Aprovechamiento_Almacen_Residuos.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Area_Residuos].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Area_Residuos].ToString() == "NO" ? 1 : -1;
                        RBtn_Aprovechamiento_Existe_Separacion.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Separacion].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Separacion].ToString() == "NO" ? 1 : -1;

                        DataTable Dt_Residuos = Negocio.Consultar_Residuos();
                        Grid_Tipos_Residuos.Columns[1].Visible = true;
                        Grid_Tipos_Residuos.DataSource = Dt_Residuos;
                        Grid_Tipos_Residuos.DataBind();
                        Grid_Tipos_Residuos.Columns[1].Visible = false;
                        Grid_Tipos_Residuos.Columns[3].Visible = false;
                        Session["Dt_Tipo_Residuo"] = Dt_Residuos;
                        Btn_Agregar_Tipo_Residuo.Visible = false;

                        Txt_Aprovechamiento_Metodo_Sepearacion.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Metodo_Separacion].ToString();
                        Txt_Aprovechamiento_Servicio_Recoleccion.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Servicio_Recoloccion].ToString();
                        RBtn_Aprovechamiento_Revuelven_Liquidos.SelectedIndex =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Revuelven_Solidos_Liquidos].ToString() == "SI" ? 0 :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Revuelven_Solidos_Liquidos].ToString() == "NO" ? 1 : -1;
                        Txt_Aprovechamiento_Tipo_Contenedor.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Tipo_Contenedor].ToString();
                        Txt_Aprovechamiento_Tipo_Ruido.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Tipo_Ruido].ToString();
                        Txt_Aprovechamiento_Nivel_Ruido.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Nivel_Ruido].ToString();
                        if (!String.IsNullOrEmpty(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Horario_Labores].ToString()))
                        {
                            Txt_Aprovechamiento_Horario_Inicial.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Horario_Labores].ToString().Substring(0, 13);
                            Txt_Aprovechamiento_Horario_Final.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Horario_Labores].ToString().Substring(16, 13);
                        }
                        Chk_Dias_Labor_Lunes.Checked =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Lunes].ToString() == "SI" ? true :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Lunes].ToString() == "NO" ? false : false;
                        Chk_Dias_Labor_Martes.Checked =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Martes].ToString() == "SI" ? true :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Martes].ToString() == "NO" ? false : false;
                        Chk_Dias_Labor_Miercoles.Checked =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Miercoles].ToString() == "SI" ? true :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Miercoles].ToString() == "NO" ? false : false;
                        Chk_Dias_Labor_Jueves.Checked =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Jueves].ToString() == "SI" ? true :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Jueves].ToString() == "NO" ? false : false;
                        Chk_Dias_Labor_Viernes.Checked =
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Viernes].ToString() == "SI" ? true :
                            Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Viernes].ToString() == "NO" ? false : false;
                        Txt_Aprovechamiento_Emisiones_Atmosfera.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Emisiones].ToString();
                        Txt_Generales_Recepcion_Inspector_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Recepcion_Inspector].ToString()));
                        Txt_Generales_Recepcion_Campo_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Fecha_Realizada_Campo].ToString()));
                        Txt_Generales_Recepcion_Coordinador_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Recepcion_Coordinacion].ToString()));
                        Txt_Generales_Observaciones_Para_Inspector.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Observaciones_Para_Insepctor].ToString();
                        Txt_Generales_Observaciones_Del_Inspector.Text = Dt_Admon_Urbana.Rows[0][Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Observaciones_Del_Insepctor].ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Generar_Reporte_Ficha_Revision: " + ex.Message.ToString(), ex);
                }

            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Datos_Dictamen_Modificar
        ///DESCRIPCIÓN: Carga el grid de datos 
        ///PARAMETROS:      String Tramite_ID:contiene el id del tramite
        ///CREO:            HUGO ENRIQUE RAMÍREZ AGUILERA
        ///FECHA_CREO:      20/Agosto/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cargar_Grid_Datos_Dictamen_Modificar(DataTable Dt_Consulta)
        {
            try
            {
                Grid_Datos_Dictamen_Modificar.Columns[0].Visible = true;
                Grid_Datos_Dictamen_Modificar.Columns[1].Visible = true;

                //  se ordenara la tabla por fecha
                DataView Dv_Ordenar = new DataView(Dt_Consulta);
                Dv_Ordenar.Sort = Cat_Tra_Datos_Tramite.Campo_Dato_ID;//SOLICITO asc, 
                DataTable Dt_Datos_Ordenados = Dv_Ordenar.ToTable();

                if (Dt_Datos_Ordenados.Rows.Count > 0)
                {
                    Grid_Datos_Dictamen_Modificar.DataSource = Dt_Datos_Ordenados;
                    Session["Grid_Datos_Modificar"] = Dt_Datos_Ordenados;
                    //Grid_Datos_Dictamen.Visible = true;
                }
                else
                {
                    Grid_Datos_Dictamen_Modificar.DataSource = new DataTable();
                    //Grid_Datos_Dictamen.Visible = false;
                }

                Grid_Datos_Dictamen_Modificar.DataBind();
                Grid_Datos_Dictamen_Modificar.Columns[0].Visible = false;
                Grid_Datos_Dictamen_Modificar.Columns[1].Visible = false;

                //Se cargará el valor del Dato
                if (Grid_Datos_Dictamen_Modificar.Rows.Count == Dt_Datos_Ordenados.Rows.Count)
                {
                    for (Int32 Contador = 0; Contador < Grid_Datos_Dictamen_Modificar.Rows.Count; Contador++)
                    {
                        TextBox Txt_Valor_Temporal = (TextBox)Grid_Datos_Dictamen_Modificar.Rows[Contador].Cells[3].Controls[1];
                        Txt_Valor_Temporal.Text = Dt_Datos_Ordenados.Rows[Contador][3].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

    #endregion

    #region Validaciones
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Datos
        /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
        /// PARAMETROS: 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 05/Junio/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Validar_Datos()
        {
            String Espacios_Blanco = "";
            Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
            Lbl_Mensaje_Error.Text = "";
            Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";

            //if (Cmb_Zona.SelectedIndex == 0)
            //{
            //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione la zona.<br>";
            //    Datos_Validos = false;
            //}
            if (Cmb_Tipo_Supervision.SelectedIndex == 0)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el tipo de supervision.<br>";
                Datos_Validos = false;
            }
            //if (Cmb_Condiciones_Inmueble.SelectedIndex == 0)
            //{
            //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el tipo de condicion del inmueble.<br>";
            //    Datos_Validos = false;
            //}
            //if (Cmb_Avance_Obra.SelectedIndex == 0)
            //{
            //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el porcentaje del avance.<br>";
            //    Datos_Validos = false;
            //}
            //if (Cmb_Via_Publica_Area_Donacion.SelectedIndex == 0)
            //{
            //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el tipo de via publica y area de donacion.<br>";
            //    Datos_Validos = false;
            //}
            //if (Cmb_Uso_Actual.SelectedIndex == 0)
            //{
            //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el tipo de uso actual.<br>";
            //    Datos_Validos = false;
            //}
            //if (Cmb_Funcionamiento.SelectedIndex == 0)
            //{
            //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el tipo de problema dentro del funcionamiento.<br>";
            //    Datos_Validos = false;
            //}
            //if (Cmb_Material_Muros.SelectedIndex == 0)
            //{
            //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el tipo de material de los muros.<br>";
            //    Datos_Validos = false;
            //}
            //if (Cmb_Material_Techo.SelectedIndex == 0)
            //{
            //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el tipo de material del techo.<br>";
            //    Datos_Validos = false;
            //}
            //if (Cmb_Colonias.SelectedIndex == 0)
            //{
            //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione la colonia.<br>";
            //    Datos_Validos = false;
            //}
            //if (Cmb_Calle.SelectedIndex == 0)
            //{
            //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione la calle.<br>";
            //    Datos_Validos = false;
            //}
            //if (Txt_Numero_Fisico.Text == "")
            //{
            //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el numero fisico.<br>";
            //    Datos_Validos = false;
            //}
            DateTime Fecha = (DateTime.Now);

            if (!String.IsNullOrEmpty(Txt_Generales_Recepcion_Inspector_Fecha.Text))
            {
                if (Convert.ToDateTime(Txt_Generales_Recepcion_Inspector_Fecha.Text) > Fecha)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Fecha de recepción por el Inspector Superior a la actual.<br>";
                    Datos_Validos = false;
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Fecha de recepción por el Inspector.<br>";
                Datos_Validos = false;
            }

            if (!String.IsNullOrEmpty(Txt_Generales_Recepcion_Campo_Fecha.Text))
            {
                if (Convert.ToDateTime(Txt_Generales_Recepcion_Campo_Fecha.Text) > Fecha)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Fecha en que se realizo en campo Superior a la actual.<br>";
                    Datos_Validos = false;
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Fecha en que se realizo en campo.<br>";
                Datos_Validos = false;
            }

            if (!String.IsNullOrEmpty(Txt_Generales_Recepcion_Coordinador_Fecha.Text))
            {
                if (Convert.ToDateTime(Txt_Generales_Recepcion_Coordinador_Fecha.Text) > Fecha)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Fecha de recepción por el coordinador Superior a la actual.<br>";
                    Datos_Validos = false;
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Fecha de recepción por el coordinador.<br>";
                Datos_Validos = false;
            }

            Mostrar_Mensaje_Error(!Datos_Validos);

            return Datos_Validos;
        }

    #endregion

    #region Botones
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Btn_Nuevo_Click
        /// DESCRIPCION : realiza la alta del usuario
        /// PARAMETROS: 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 01/Junio/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Mostrar_Mensaje_Error(false);

                if (Btn_Nuevo.ToolTip == "Nuevo")
                {
                    Limpiar_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                    Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                }
                else
                {
                    if (Validar_Datos())
                    {
                        Mostrar_Mensaje_Error(false);

                        Cls_Cat_Ort_Administracion_Urbana_Negocio Cedula_Negocio = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
                        Cedula_Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        DataTable Dt_Cedula = Cedula_Negocio.Consultar_Administracion_Urbana(Cedula_Negocio);

                        if (Dt_Cedula.Rows.Count > 0)
                            Actualizar_Ficha_Inspeccion();
                        else
                            Alta_Formato();
                    }
                    else
                    {
                        Mostrar_Mensaje_Error(true);
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
        /// NOMBRE DE LA FUNCION: Btn_Salir_Click
        /// DESCRIPCION : 
        /// PARAMETROS: 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 31/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //if (Div_Principal_Llenado_Formato.Style.Value == "display:block")
                //{
                //    Inicializar_Controles(); //Habilita los controles para la siguiente operación del usuario en el catálogo
                   
                //}
                //else
                //{ 

                if (Hdf_Redireccionar.Value == "")
                {
                    Inicializar_Controles();
                }
                else
                {
                    Session["Solicitud"] = Hdf_Solicitud_ID.Value;
                    Response.Redirect("../Tramites/Frm_Bandeja_Tramites.aspx");
                    //}
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Autorizar_Formato_Click
        ///DESCRIPCIÓN: Autorizar el llenado del formato
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  09/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Autorizar_Formato_Click(object sender, EventArgs e)
        {
            DataTable Dt_Informacion_Grid = new DataTable();
            Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
            DataTable Dt_Consulta = new DataTable();
            try
            {
                ImageButton Imagen_Boton = (ImageButton)sender;
                TableCell Celda = (TableCell)Imagen_Boton.Parent;
                GridViewRow Renglon = (GridViewRow)Celda.Parent;
                Grid_Formatos.SelectedIndex = Renglon.RowIndex;
                int Fila = Renglon.RowIndex;


                Limpiar_Controles();
                GridViewRow selectedRow = Grid_Formatos.Rows[Fila];
                Hdf_Solicitud_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
                Hdf_Tramite_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
                Hdf_Subproceso_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[3].Text).ToString();

                //  se carga la informacion de la solicitud
                Negocio_Datos_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                Negocio_Datos_Solicitud = Negocio_Datos_Solicitud.Consultar_Datos_Solicitud();
                Cls_Sessiones.Ciudadano_ID = Hdf_Solicitud_ID.Value;
                Txt_Nombre.Text = (Negocio_Datos_Solicitud.P_Solicito != "") ? Negocio_Datos_Solicitud.P_Solicito : "";
                Txt_Estatus.Text = (Negocio_Datos_Solicitud.P_Estatus != "") ? Negocio_Datos_Solicitud.P_Estatus : "";
                Txt_Proceso.Text = (Negocio_Datos_Solicitud.P_Subproceso_Nombre !="") ? Negocio_Datos_Solicitud.P_Subproceso_Nombre : "";
                Txt_Avance.Text = Convert.ToString(Negocio_Datos_Solicitud.P_Porcentaje_Avance) + " %";
                
                //  se llenan las cajas de texto de consecutivo y cuenta predial
                Txt_Consecutivo_ID.Text = Negocio_Datos_Solicitud.P_Consecutivo;

                if (Negocio_Datos_Solicitud.P_Cuenta_Predial.ToString() != "")
                {
                    Txt_Cuenta_Predial.Text = Negocio_Datos_Solicitud.P_Cuenta_Predial;
                    Tabla_Datos_Calle.Style.Value = "display:none";
                    Tabla_Colonia.Style.Value = "display:none";
                    Tabla_Calle.Style.Value = "display:none";
                }
                else
                {
                    Lbl_Cuenta_Predial.Visible = false;
                    Txt_Cuenta_Predial.Visible = false;
                    Btn_Buscar_Cuenta_Predial.Visible = false;
                    Tabla_Datos_Calle.Style.Value = "display:block";
                    Tabla_Colonia.Style.Value = "display:block";
                    Tabla_Calle.Style.Value = "display:block";
                    Cargar_Combo_Colonias();
                }

                //  para cargar la zona
                Cmb_Zona.SelectedIndex = Cmb_Zona.Items.IndexOf(Cmb_Zona.Items.FindByValue(Negocio_Datos_Solicitud.P_Zona_ID));

                //  para cargar el tipo de supervision
                Cmb_Tipo_Supervision.SelectedIndex = Cmb_Tipo_Supervision.Items.IndexOf(Cmb_Tipo_Supervision.Items.FindByText(Negocio_Datos_Solicitud.P_Tramite));

                //  para cargar el inspector
                Cmb_Inspector.SelectedIndex = Cmb_Inspector.Items.IndexOf(Cmb_Inspector.Items.FindByValue(Negocio_Datos_Solicitud.P_Inspector_ID));

                //  para el evento de resumen de predio
                Cargar_Ventana_Emergente_Resumen_Predio();

                Div_Lista_Formatos.Style.Value = "display:none";
                Div_Principal_Llenado_Formato.Style.Value = "display:block";
                Btn_Nuevo.Visible = true;
                Habilitar_Controles("Nuevo");

                

                //  CODIGO PARA CONSULTAR DATOS FINALES
                DataTable Dt_Consulta_Dato_Final = Negocio_Datos_Solicitud.Consultar_Datos_Finales();
                //  para cuando tenga registros para el dictamen
                if (Dt_Consulta_Dato_Final != null && Dt_Consulta_Dato_Final.Rows.Count > 0)
                {
                    DataTable Dt_Consulta_Dato_Final_Llenado = Negocio_Datos_Solicitud.Consultar_Datos_Finales_Operacion();
                    //Grid_Datos_Dictamen_Modificar.DataSource = Dt_Consulta_Dato_Final_Llenado;
                    //Grid_Datos_Dictamen_Modificar.DataBind();
                    Cargar_Grid_Datos_Dictamen_Modificar(Dt_Consulta_Dato_Final_Llenado);
                    Grid_Datos_Dictamen.Style.Value = "display:none";
                }
                else
                {
                    Cls_Ope_Solicitud_Tramites_Negocio Negocio_Consultar_Datos_Dictamen = new Cls_Ope_Solicitud_Tramites_Negocio();
                    Negocio_Consultar_Datos_Dictamen.P_Tramite_ID = Negocio_Datos_Solicitud.P_Tramite_id;
                    Negocio_Consultar_Datos_Dictamen.P_Tipo_Dato = "FINAL";
                    DataSet Ds_Datos = new DataSet();
                    Ds_Datos = Negocio_Consultar_Datos_Dictamen.Consultar_Datos_Tramite();

                    //  se ordenara la tabla por fecha
                    DataView Dv_Ordenar = new DataView(Ds_Datos.Tables[0]);
                    Dv_Ordenar.Sort = Cat_Tra_Datos_Tramite.Campo_Dato_ID;//SOLICITO asc, 
                    DataTable Dt_Datos_Ordenados = Dv_Ordenar.ToTable();
                    Grid_Datos_Dictamen.DataSource = Dt_Datos_Ordenados;
                    Grid_Datos_Dictamen.DataBind();
                    Grid_Datos_Dictamen_Modificar.Style.Value = "display:none";
                    Session["Grid_Datos"] = Dt_Datos_Ordenados;
                }

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Btn_Agregar_Tipo_Residuo_Click
        /// DESCRIPCION :   agregara un tipo de residuo al grid
        /// PARAMETROS: 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 06/Junio/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Btn_Agregar_Tipo_Residuo_Click(object sender, ImageClickEventArgs e)
        {
            DataTable Dt_Residuo;
            DataRow Fila;
            Boolean Estado = false;
            try
            {
                if (Cmb_Tipo_Residuo.SelectedIndex > 0)
                {
                    Mostrar_Mensaje_Error(false);
                    if (Session["Dt_Tipo_Residuo"] == null)
                    {
                        Dt_Residuo = new DataTable("Dt_Tipo_Residuo");
                        Dt_Residuo.Columns.Add("TIPO_RESIDUO_ID", Type.GetType("System.String"));
                        Dt_Residuo.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    }
                    else
                    {
                        Dt_Residuo = (DataTable)Session["Dt_Tipo_Residuo"];
                    }

                    //  buscar que no se encuentre dentro del grid

                    if (Dt_Residuo != null && Dt_Residuo.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Dt_Residuo.Rows)
                        {
                            if ((Cmb_Tipo_Residuo.SelectedValue == Registro["TIPO_RESIDUO_ID"].ToString()))
                            {
                                Estado = true;
                            }
                        }
                    }

                    if (Estado == false)
                    {
                        Fila = Dt_Residuo.NewRow();

                        Fila["TIPO_RESIDUO_ID"] = Cmb_Tipo_Residuo.SelectedValue;
                        Fila["NOMBRE"] = Cmb_Tipo_Residuo.SelectedItem.ToString();
                        Dt_Residuo.Rows.Add(Fila);

                        Llenar_Grid_Residuos(Dt_Residuo);
                        Cmb_Tipo_Residuo.SelectedIndex = 0;
                    }
                    else
                    {
                        Mostrar_Mensaje_Error(true);
                        Lbl_Mensaje_Error.Text = "El tipo de residuo [" + Cmb_Tipo_Residuo.SelectedItem.ToString() + "] ya se encuentra seleccionado";
                    }
                }
                else
                {
                    Mostrar_Mensaje_Error(true);
                    Lbl_Mensaje_Error.Text = "Seleccione el tipo de residuo";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Btn_Quitar_Tipo_Residuo_Click
        /// DESCRIPCION :   quitara el tipo de residuo del grid
        /// PARAMETROS: 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 06/Junio/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Btn_Quitar_Tipo_Residuo_Click(object sender, ImageClickEventArgs e)
        {
            TableCell Tabla;
            GridViewRow Row;
            int Fila;
            DataTable Dt_Elimiar_Registro = new DataTable();
            try
            {
                ImageButton Btn_Eliminar = (ImageButton)sender;
                Tabla = (TableCell)Btn_Eliminar.Parent;
                Row = (GridViewRow)Tabla.Parent;
                Grid_Tipos_Residuos.SelectedIndex = Row.RowIndex;
                Fila = Row.RowIndex;

                Dt_Elimiar_Registro = (DataTable)Session["Dt_Tipo_Residuo"];
                Dt_Elimiar_Registro.Rows.RemoveAt(Fila);
                Session["Dt_Tipo_Residuo"] = Dt_Elimiar_Registro;
                Grid_Tipos_Residuos.SelectedIndex = (-1);
                Llenar_Grid_Residuos(Dt_Elimiar_Registro);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN         : Deja los componentes listos para hacer la modificacion.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    if (Grid_Listado.Rows.Count > 0 && Grid_Listado.SelectedIndex > (-1)
                            || (Request.QueryString["Solicitud"] != null || Session["Cedula_Solicitud_Id"].ToString() != "" || Session["Solicitud_Id"] != null))
                    {
                        Habilitar_Controles("Modificar");
                        Cmb_Evaluacion.Enabled = true;
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Debe seleccionar el Registro que se desea Modificar.";
                    }
                }
                else
                {
                    if (Validar_Datos())
                    {
                        #region Aprobar

                        Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
                        Cls_Ope_Bandeja_Tramites_Negocio Neg_Informacion_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                        String Actividad = "";
                        Negocio.P_Administracion_Urbana_ID = Hdf_Administracion_Urbana_ID.Value;
                        Neg_Informacion_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        //  se carga los datos de la consulta en la capa de negocios
                        Neg_Informacion_Solicitud = Neg_Informacion_Solicitud.Consultar_Datos_Solicitud(); // Se obtienen los Datos a Detalle de la Solicitud Seleccionada
                        Actividad = Neg_Informacion_Solicitud.P_Subproceso_Nombre;


                        //  para el dato de la cuenta predila
                        if (Txt_Cuenta_Predial.Text != "")
                            Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;

                        else
                        {
                            Negocio.P_Area_Calle = Cmb_Calle.SelectedValue;
                            Negocio.P_Area_Colonia = Cmb_Colonias.SelectedValue;
                            Negocio.P_Area_Numero_Fisico = Txt_Numero_Fisico.Text;
                            Negocio.P_Area_Manzana = Txt_Lote.Text;
                            Negocio.P_Area_Lote = Txt_Manzana.Text;
                        }

                        //  para los datos de los id generales
                        Negocio.P_Tramite_ID = Hdf_Tramite_ID.Value;
                        Negocio.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        Negocio.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
                        Negocio.P_Inspector_ID = Cmb_Inspector.SelectedValue;

                        //  para los datos de la area
                        Negocio.P_Area_Inspeccion = RBtn_Area_Inspeccion.SelectedValue;
                        Negocio.P_Area_Zona = Cmb_Zona.SelectedValue;
                        Negocio.P_Area_Uso_Solicitado = Txt_Destinado.Text;

                        //  para los datos del tipo de supervision
                        Negocio.P_Tipo_Supervision_ID = Cmb_Tipo_Supervision.SelectedValue;

                        //  para los datos de las condiciones del inmueble
                        Negocio.P_Condiciones_Inmueble_ID = Cmb_Condiciones_Inmueble.SelectedIndex == 0 ? "" : Cmb_Condiciones_Inmueble.SelectedValue;

                        //  para el avance de la obra
                        Negocio.P_Avance_Obra_ID = Cmb_Avance_Obra.SelectedIndex == 0 ? "" : Cmb_Avance_Obra.SelectedValue;
                        if (Txt_Avance_Obra_Aproximado.Text != "")
                            Negocio.P_Avance_Bardeo_Aproximado = Txt_Avance_Obra_Aproximado.Text;

                        if (Txt_Avance_Obra_Niveles_Actuales.Text != "")
                            Negocio.P_Avance_Niveles_Actuales = Txt_Avance_Obra_Niveles_Actuales.Text;

                        if (Txt_Avance_Obra_Niveles_Construir.Text != "")
                            Negocio.P_Avance_Niveles_Construccion = Txt_Avance_Obra_Niveles_Construir.Text;

                        Negocio.P_Avance_Proyecto_Acorde = RBtn_Avance_Obra_Acorde_Solicitado.SelectedValue;

                        //  para las vias publicas y donaciones
                        //Negocio.P_Area_Via_ID = Cmb_Via_Publica_Area_Donacion.SelectedValue;
                        Negocio.P_VIA_PUBLICA_INVASION_DONACION = Chk_Invasion_Areas_donacion.Checked ? "SI" : "NO";
                        Negocio.P_VIA_PUBLICA_INVASION_MATERIAL = Chk_Invasion_Material.Checked ? "SI" : "NO";
                        Negocio.P_VIA_PUBLICA_PARAMENTO = Chk_Sobresale_Paramento.Checked ? "SI" : "NO";
                        Negocio.P_VIA_PUBLICA_SOBRE_MARQUESINA = Chk_Construccion_Marquesina.Checked ? "SI" : "NO";
                        Negocio.P_Area_Via_Especificar_Restricciones = Txt_Especificacion_Restriccion.Text;

                        //  para las datos referentes a las inspecciones 
                        Negocio.P_Inspeccion_Notificacion = RBtn_Notificado.SelectedValue;
                        if (RBtn_Notificado.SelectedValue == "NO" || RBtn_Notificado.SelectedIndex == -1)
                            Negocio.P_Inspeccion_Notificacion_Folio = "null";
                        else
                            Negocio.P_Inspeccion_Notificacion_Folio = Txt_Notificacion_Folio.Text;

                        Negocio.P_Inspeccion_Acta = RBtn_Acta_Inspeccion.SelectedValue;

                        if (RBtn_Acta_Inspeccion.SelectedValue == "NO" || RBtn_Acta_Inspeccion.SelectedIndex == -1)
                            Negocio.P_Inspeccion_Acta_Folio = "null";
                        else
                            Negocio.P_Inspeccion_Acta_Folio = Txt_Acta_Inspeccion_Folio.Text;

                        Negocio.P_Inspeccion_Clausurado = RBtn_Clausurado.SelectedValue;
                        if (RBtn_Clausurado.SelectedValue == "NO" || RBtn_Clausurado.SelectedIndex == -1)
                            Negocio.P_Inspeccion_Clausurado_Folio = "null";
                        else
                            Negocio.P_Inspeccion_Clausurado_Folio = Txt_Clausurado_Folio.Text;

                        Negocio.P_Inspeccion_Multado = RBtn_Multado.SelectedValue;

                        if (RBtn_Multado.SelectedValue == "NO" || RBtn_Multado.SelectedIndex == -1)
                            Negocio.P_Inspeccion_Multado_Folio = "";
                        else
                            Negocio.P_Inspeccion_Multado_Folio = Txt_Multa_Folio.Text;

                        //  para el uso actual
                        Negocio.P_Uso_Actual_ID = Cmb_Uso_Actual.SelectedIndex == 0 ? "" : Cmb_Uso_Actual.SelectedValue;
                        Negocio.P_Uso_Actual_Acorde_Solicitado = RBtn_Uso_Diferente_Adicional.SelectedValue;
                        Negocio.P_Uso_Actual_Especificar_Tipo_Uso = Txt_Especificar_Tipo_Uso.Text;

                        //  para el uso predominante de la zona  
                        Negocio.P_Uso_Predominante_Colindantes = Txt_Usos_Colindantes.Text;
                        Negocio.P_Uso_Predominante_Frente_Inmueble = Txt_Usos_Frente_Inmueble.Text;
                        Negocio.P_Uso_Predominante_Impacto = Txt_Usos_Cercanos_Riesgo.Text;

                        //  para el uso del funcionamiento
                        Negocio.P_Funcionamiento_Actividad = Txt_Area_Acticidad.Text;
                        if (Txt_Superficie_Metros2.Text != "")
                            Negocio.P_Funcionamiento_Metros_Cuadrados = Txt_Superficie_Metros2.Text;

                        Negocio.P_Funcionamiento_Maquinaria = Txt_Maquinaria_Utilizar.Text;
                        Negocio.P_Funcionamiento_ID = Cmb_Funcionamiento.SelectedIndex == 0 ? "" : Cmb_Funcionamiento.SelectedValue;
                        if (Txt_Cantidad_Personas_Laboran.Text != "")
                            Negocio.P_Funcionamiento_No_Personas = Txt_Cantidad_Personas_Laboran.Text;

                        if (Txt_Afluencia.Text != "")
                            Negocio.P_Funcionamiento_No_Clientes = Txt_Afluencia.Text;

                        //  para los campos de anuncios 
                        Negocio.P_Anuncio_1 = Txt_Tipo_Anuncio_1.Text;
                        if (Txt_Dimenciones_Anuncion_1.Text != "")
                            Negocio.P_Anuncio_1_Dimensiones = Txt_Dimenciones_Anuncion_1.Text;

                        Negocio.P_Anuncio_2 = Txt_Tipo_Anuncio_2.Text;
                        if (Txt_Dimenciones_Anuncion_2.Text != "")
                            Negocio.P_Anuncio_2_Dimensiones = Txt_Dimenciones_Anuncion_2.Text;

                        Negocio.P_Anuncio_3 = Txt_Tipo_Anuncio_3.Text;
                        if (Txt_Dimenciones_Anuncion_3.Text != "")
                            Negocio.P_Anuncio_3_Dimensiones = Txt_Dimenciones_Anuncion_3.Text;

                        Negocio.P_Anuncio_4 = Txt_Tipo_Anuncio_4.Text;
                        if (Txt_Dimenciones_Anuncion_4.Text != "")
                            Negocio.P_Anuncio_4_Dimensiones = Txt_Dimenciones_Anuncion_4.Text;

                        //  para los servicios
                        Negocio.P_Servicios_Cuenta_Sanitarios = RBtn_Servicios_Sanitarios.SelectedValue;
                        Negocio.P_Servicios_WC = RBtn_Wc.SelectedValue;
                        Negocio.P_Servicios_Lavabo = RBtn_Lavabo.SelectedValue;
                        Negocio.P_Servicios_Letrina = RBtn_Letrina.SelectedValue;
                        Negocio.P_Servicios_Mixto = RBtn_Mixto.SelectedValue;
                        if (Txt_Bano_Hombres.Text != "")
                            Negocio.P_Servicios_Numero_Sanitarios_Hombres = Txt_Bano_Hombres.Text;
                        else
                            Negocio.P_Servicios_Numero_Sanitarios_Hombres = "0";

                        if (Txt_Bano_Mujeres.Text != "")
                            Negocio.P_Servicios_Numero_Sanitarios_Mujeres = Txt_Bano_Mujeres.Text;
                        else
                            Negocio.P_Servicios_Numero_Sanitarios_Mujeres = "0";

                        Negocio.P_Servicios_Agua_Potable = RBtn_Agua_Potable.SelectedValue;
                        Negocio.P_Servicios_Agua_Abastecimiento_Particular = RBtn_Agua_Potable_Particular.SelectedValue;
                        Negocio.P_Servicios_Agua_Abastecimiento_Japami = RBtn_Agua_Potable_Japami.SelectedValue;
                        Negocio.P_Servicios_Drenaje = RBtn_Agua_Potable_Drenaje.SelectedValue;
                        Negocio.P_Servicios_Fosa_Septica = RBtn_Agua_Potable_Fosa_Septica.SelectedValue;
                        Negocio.P_Servicios_Estacionamiento = RBtn_Cuenta_Estacionamiento.SelectedValue;
                        Negocio.P_Servicios_Estacionamiento_Propio = RBtn_Cuenta_Estacionamiento_Dentro_Inmueble.SelectedValue;
                        Negocio.P_Servicios_Estacionamiento_Rentado = RBtn_Cuenta_Estacionamiento_Rentado.SelectedValue;
                        if (Txt_Numero_Cajones.Text != "")
                            Negocio.P_Servicios_Estacionamiento_Numero_Cajones = Txt_Numero_Cajones.Text;
                        //else
                        //    Negocio_Alta_Formato.P_Servicios_Estacionamiento_Numero_Cajones = "0";

                        Negocio.P_Servicios_Estacionamiento_Area_Descarga = RBtn_Cuenta_Estacionamiento_Area_Carga.SelectedValue;
                        Negocio.P_Servicios_Estacionamiento_Domicilio = Txt_Domicilio_Estacionamiento.Text;

                        //  para los materiales empleados
                        Negocio.P_Materiales_Empleado_Muros = Cmb_Material_Muros.SelectedIndex == 0 ? "" : Cmb_Material_Muros.SelectedValue;
                        Negocio.P_Materiales_Empleado_Techos = Cmb_Material_Techo.SelectedIndex == 0 ? "" : Cmb_Material_Techo.SelectedValue;

                        //  para las medidas de seguridad
                        Negocio.P_Seguridad_Medidas = RBtn_Medidas_Seguridad_Senalizacion.SelectedValue;
                        Negocio.P_Seguridad_Equipo = RBtn_Medidas_Seguridad_Equipo.SelectedValue;
                        Negocio.P_Seguridad_Material_Flamable = RBtn_Medidas_Seguridad_Material_Flamable.SelectedValue;
                        Negocio.P_Seguridad_Especificar = Txt_Medidas_Seguridad_Especificar.Text;

                        //  para la poda de arboles
                        if (Txt_Arbol_Altura.Text != "")
                            Negocio.P_Poda_Altura = Txt_Arbol_Altura.Text.Trim();

                        if (Txt_Arbol_Diametro.Text != "")
                            Negocio.P_Poda_Diametro_Tronco = Txt_Arbol_Diametro.Text;

                        if (Txt_Arbol_Diametro_Fronda.Text != "")
                            Negocio.P_Poda_Fronda = Txt_Arbol_Diametro_Fronda.Text;

                        Negocio.P_Poda_Estado = Txt_Arbol_Estado.Text;

                        //  para los campos generales                
                        Negocio.P_Generales_Recepcion_Inspector = Convert.ToDateTime(Txt_Generales_Recepcion_Inspector_Fecha.Text);
                        Negocio.P_Generales_Fecha_Revision_Campo = Convert.ToDateTime(Txt_Generales_Recepcion_Campo_Fecha.Text);
                        Negocio.P_Generales_Recepcion_Coordinacion = Convert.ToDateTime(Txt_Generales_Recepcion_Coordinador_Fecha.Text);
                        Negocio.P_Generales_Observaciones_Para_Inspector = Txt_Generales_Observaciones_Para_Inspector.Text;
                        Negocio.P_Generales_Observaciones_Inspector = Txt_Generales_Observaciones_Del_Inspector.Text;

                        //  para los campos de auditoria
                        Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;

                        //  para los datos del manifiesto de impacto ambiental
                        Negocio.P_Impacto_Afectables = Txt_Manifiesto_Afectacion.Text;
                        Negocio.P_Impacto_Colindancias = Txt_Manifiesto_Colindancia.Text;
                        Negocio.P_Impacto_Superficie = Txt_Manifiesto_Superficie_Total.Text;
                        Negocio.P_Impacto_Tipo_Proyecto = Txt_Manifiesto_Tipo_Proyecto.Text;

                        // para los datos de licencia
                        Negocio.P_Licencia_Tipo_Equipo = Txt_Licencia_Equipo_Emisor.Text;
                        Negocio.P_Licencia_Tipo_Emision = Txt_Licencia_Tipo_Emision.Text;
                        Negocio.P_Licencia_Horario_Funcionamiento = Txt_Licencia_Hora_Funcionamiento.Text;
                        Negocio.P_Licencia_Tipo_Combustible = Txt_Licencia_Tipo_Conbustible.Text;
                        Negocio.P_Licencia_Tipo_Gastos_Combustible = Txt_Licencia_Gastos_Combustible.Text;
                        Negocio.P_Licencia_Almacenaje = Txt_Licencia_Almacenaje_Combustible.Text;
                        Negocio.P_Licencia_Cantidad_Combustible = Txt_Licencia_Cantidad_Combustible.Text;


                        //  para los datos del banco de materiales 
                        Negocio.P_Material_Permiso_Ecologico = RBtn_Material_Permiso_Ecologia.SelectedValue;
                        Negocio.P_Material_Permiso_Suelo = RBtn_Material_Permiso_Suelo.SelectedValue;
                        Negocio.P_Material_Superficie_Total = Txt_Materiales_Superficie_Total.Text;
                        Negocio.P_Material_Profundidad = Txt_Materiales_Profundidad.Text;
                        Negocio.P_Material_Inclinacion = Txt_Materiales_Inclinacion.Text;
                        Negocio.P_Material_Flora = Txt_Materiales_Flora.Text;
                        Negocio.P_Material_Acceso_Vehiculos = RBtn_Material_Accesibilidad_Vehiculo.SelectedValue;
                        Negocio.P_Material_Petreo = Txt_Materiales_Petreo.Text;
                        Negocio.P_Material_Especie_Arbol = Txt_Arboles_Especie.Text;

                        if (RBtn_Arboles_Tipo_Poda.Checked == true)
                        {
                            Negocio.P_Material_Tipo_Poda = "SI";
                            Negocio.P_Material_Cantidad_Poda = Txt_Arboles_Cantidad_Poda.Text;
                        }
                        else
                            Negocio.P_Material_Tipo_Poda = "NO";

                        if (RBtn_Arboles_Tipo_Tala.Checked == true)
                        {
                            Negocio.P_Material_Tipo_Tala = "SI";
                            Negocio.P_Material_Cantidad_Tala = Txt_Arboles_Cantidad_Tala.Text;
                        }
                        else
                            Negocio.P_Material_Tipo_Tala = "NO";

                        if (RBtn_Arboles_Tipo_Tala.Checked == true)
                        {
                            Negocio.P_Material_Tipo_Trasplante = "SI";
                            Negocio.P_Material_Cantidad_Trasplante = Txt_Arboles_Cantidad_Trasplante.Text;
                        }
                        else
                            Negocio.P_Material_Tipo_Trasplante = "NO";

                        //  para los datos de la autorizacion de aprovechamiento ambiental
                        Negocio.P_Autoriza_Suelos = RBtn_Aprovechamiento_Uso_Suelo.SelectedValue;
                        Negocio.P_Autoriza_Area_Residuos = RBtn_Aprovechamiento_Almacen_Residuos.SelectedValue;
                        Negocio.P_Autoriza_Separacion = RBtn_Aprovechamiento_Existe_Separacion.SelectedValue;
                        Negocio.P_Autoriza_Metodo_Separacion = Txt_Aprovechamiento_Metodo_Sepearacion.Text;
                        Negocio.P_Autoriza_Servicio_Recoleccion = Txt_Aprovechamiento_Servicio_Recoleccion.Text;
                        Negocio.P_Autoriza_Revuelven_Solidos_Liquidos = RBtn_Aprovechamiento_Revuelven_Liquidos.SelectedValue;
                        Negocio.P_Autoriza_Tipo_Contenedor = Txt_Aprovechamiento_Tipo_Contenedor.Text;
                        Negocio.P_Autoriza_Tipo_Ruido = Txt_Aprovechamiento_Tipo_Ruido.Text;

                        if (Txt_Aprovechamiento_Nivel_Ruido.Text == "")
                            Txt_Aprovechamiento_Nivel_Ruido.Text = "0";

                        Negocio.P_Autoriza_Nivel_Ruido = Txt_Aprovechamiento_Nivel_Ruido.Text;

                        if (!String.IsNullOrEmpty(Txt_Aprovechamiento_Horario_Inicial.Text) && !String.IsNullOrEmpty(Txt_Aprovechamiento_Horario_Final.Text))
                            Negocio.P_Autoriza_Horario_Labores = Txt_Aprovechamiento_Horario_Inicial.Text + " a " + Txt_Aprovechamiento_Horario_Final.Text;

                        if (Chk_Dias_Labor_Lunes.Checked == true)
                            Negocio.P_Autoriza_Lunes = "SI";
                        else
                            Negocio.P_Autoriza_Lunes = "NO";

                        if (Chk_Dias_Labor_Martes.Checked == true)
                            Negocio.P_Autoriza_Martes = "SI";
                        else
                            Negocio.P_Autoriza_Martes = "NO";

                        if (Chk_Dias_Labor_Miercoles.Checked == true)
                            Negocio.P_Autoriza_Miercoles = "SI";
                        else
                            Negocio.P_Autoriza_Miercoles = "NO";

                        if (Chk_Dias_Labor_Jueves.Checked == true)
                            Negocio.P_Autoriza_Jueves = "SI";
                        else
                            Negocio.P_Autoriza_Jueves = "NO";

                        if (Chk_Dias_Labor_Viernes.Checked == true)
                            Negocio.P_Autoriza_Viernes = "SI";
                        else
                            Negocio.P_Autoriza_Viernes = "NO";

                        if (Chk_Dias_Labor_Sabado.Checked == true)
                            Negocio.P_Autoriza_Sabado = "SI";
                        else
                            Negocio.P_Autoriza_Sabado = "NO";

                        if (Chk_Dias_Labor_Domingo.Checked == true)
                            Negocio.P_Autoriza_Domingo = "SI";
                        else
                            Negocio.P_Autoriza_Domingo = "NO";

                        if (Txt_Aprovechamiento_Emisiones_Atmosfera.Text == "")
                            Txt_Aprovechamiento_Emisiones_Atmosfera.Text = "0";

                        Negocio.P_Autoriza_Emisiones = Txt_Aprovechamiento_Emisiones_Atmosfera.Text;

                        //  para los elementos de residuos peligrosos
                        Negocio.P_Dt_Residuos = (DataTable)Session["Dt_Tipo_Residuo"];


                        if (Negocio.P_Dt_Residuos != null && Negocio.P_Dt_Residuos.Rows.Count > 0)
                        {
                            DataView Dv_Ordenar = new DataView(Negocio.P_Dt_Residuos);
                            Dv_Ordenar.Sort = "TIPO_RESIDUO_ID asc";
                            Negocio.P_Dt_Residuos = Dv_Ordenar.ToTable();
                        }

                        Negocio.P_Estatus = Cmb_Evaluacion.SelectedItem.Value;

                        /*********************** INICIO  modificar formato ***********************/
                        Negocio.Modificar_Formato();
                        /*********************** FIN  modificar formato ***********************/

                        Llenar_Grid_Listado(Grid_Listado.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo", "alert('Actualización Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                        //if (!Cmb_Evaluacion.SelectedValue.Equals("APROBAR"))
                        //{
                        //    Cls_Ope_Bandeja_Tramites_Negocio Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                        //    Cls_Ope_Bandeja_Tramites_Negocio Negocio_Datos_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                        //    Cls_Ope_Bandeja_Tramites_Negocio Negocio_Tipo_Actividad = new Cls_Ope_Bandeja_Tramites_Negocio();
                        //    String Tipo_Actividad = "";
                        //    String Orden_Actividad = "";
                        //    Boolean Realizar_Consulta = false;
                        //    Boolean Tipo_Actividad_Cobro = false;

                        //    Solicitud.P_Tipo_DataTable = "ACTUALIZACION_SOLICITUD";
                        //    Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        //    DataTable Subprocesos = Solicitud.Consultar_DataTable();

                        //    Negocio_Datos_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        //    Negocio_Datos_Solicitud.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
                        //    Negocio_Datos_Solicitud = Negocio_Datos_Solicitud.Consultar_Datos_Solicitud();

                        //    //  si la variable no contiene la actividad anterior se busca cual es la anterior
                        //    if (!String.IsNullOrEmpty(Negocio_Datos_Solicitud.P_SubProceso_Anterior))
                        //    {
                        //        //  se comenzara de la ultima actividad a la primera
                        //        for (Int32 Contador = Subprocesos.Rows.Count - 1; Contador >= 0; Contador--)
                        //        {
                        //            if (Negocio_Datos_Solicitud.P_SubProceso_Anterior.Equals(Subprocesos.Rows[Contador][0].ToString()))
                        //            {
                        //                //  sse obtiene el id de la actividad anterior
                        //                Tipo_Actividad = Subprocesos.Rows[Contador][3].ToString();
                        //                Orden_Actividad = Subprocesos.Rows[Contador][2].ToString();
                        //                break;
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        for (Int32 Contador = Subprocesos.Rows.Count - 1; Contador >= 0; Contador--)
                        //        {
                        //            if (Negocio_Datos_Solicitud.P_Subproceso_ID.Equals(Subprocesos.Rows[Contador][0].ToString()))
                        //            {
                        //                //  sse obtiene el id de la actividad anterior
                        //                Tipo_Actividad = Subprocesos.Rows[Contador - 1][3].ToString();
                        //                Orden_Actividad = Subprocesos.Rows[Contador][2].ToString();
                        //                break;
                        //            }
                        //        }
                        //    }

                        //    if (!String.IsNullOrEmpty(Orden_Actividad))
                        //    {
                        //        Negocio_Datos_Solicitud.P_Orden = Orden_Actividad;
                        //        Negocio_Datos_Solicitud.P_Tramite_id = Hdf_Tramite_ID.Value;
                        //        DataTable Dt_Actividad_Condicion = Negocio_Datos_Solicitud.Consultar_Tipo_Actividad();
                        //        DataTable Dt_Tipo_Actividad = new DataTable();

                        //        if (Dt_Actividad_Condicion != null && Dt_Actividad_Condicion.Rows.Count > 0)
                        //        {
                        //            foreach (DataRow Registro in Dt_Actividad_Condicion.Rows)
                        //            {
                        //                for (int Contador_For = 0; Contador_For < 3; Contador_For++)
                        //                {
                        //                    Negocio_Tipo_Actividad.P_Tramite_id = Hdf_Tramite_ID.Value;

                        //                    // validacino para el numero de la orden de la actividad
                        //                    if (Contador_For == 0)
                        //                    {
                        //                        if (!String.IsNullOrEmpty(Registro[Cat_Tra_Subprocesos.Campo_Orden].ToString()))
                        //                        {
                        //                            Negocio_Tipo_Actividad.P_Orden = Registro[Cat_Tra_Subprocesos.Campo_Orden].ToString();
                        //                            Realizar_Consulta = true;
                        //                        }

                        //                        else
                        //                            Realizar_Consulta = false;
                        //                    }

                        //                    // validacino para la condicion si
                        //                    else if (Contador_For == 1)
                        //                    {
                        //                        if (!String.IsNullOrEmpty(Registro[Cat_Tra_Subprocesos.Campo_Condicion_Si].ToString()))
                        //                        {
                        //                            Negocio_Tipo_Actividad.P_Orden = Registro[Cat_Tra_Subprocesos.Campo_Condicion_Si].ToString();
                        //                            Realizar_Consulta = true;
                        //                        }

                        //                        else
                        //                            Realizar_Consulta = false;
                        //                    }

                        //                    // validacino para la condicion si
                        //                    else if (Contador_For == 2)
                        //                    {
                        //                        if (!String.IsNullOrEmpty(Registro[Cat_Tra_Subprocesos.Campo_Condicion_No].ToString()))
                        //                        {
                        //                            Negocio_Tipo_Actividad.P_Orden = Registro[Cat_Tra_Subprocesos.Campo_Condicion_No].ToString();
                        //                            Realizar_Consulta = true;
                        //                        }

                        //                        else
                        //                            Realizar_Consulta = false;
                        //                    }

                        //                    //  validacion para realizar la consulta
                        //                    if (Realizar_Consulta == true)
                        //                    {
                        //                        Dt_Tipo_Actividad = Negocio_Tipo_Actividad.Consultar_Valor_Subproceso_ID();

                        //                        if (Dt_Tipo_Actividad != null && Dt_Tipo_Actividad.Rows.Count > 0)
                        //                        {
                        //                            if (Dt_Tipo_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Tipo_Actividad].ToString() == "COBRO")
                        //                            {
                        //                                Tipo_Actividad = "COBRO";
                        //                                Tipo_Actividad_Cobro = true;
                        //                                break;
                        //                            }
                        //                        }
                        //                    }
                        //                }

                        //                if (Tipo_Actividad_Cobro == true)
                        //                    break;
                        //            }
                        //        }
                        //    }

                        //    if (Tipo_Actividad == "COBRO")
                        //    {
                        //        Lbl_Mensaje_Error.Visible = true;
                        //        Img_Error.Visible = true;
                        //        Lbl_Mensaje_Error.Text = "+ No se puede regresar a una actividad anterior ya que el ciudadano ya realizo su pago .<br/>";
                        //    }
                        //    else
                        //    {
                        //        Cls_Ope_Bandeja_Tramites_Negocio Regresar_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                        //        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Consultar_Dias = new Cls_Ope_Bandeja_Tramites_Negocio();
                        //        Regresar_Solicitud.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        //        Negocio_Consultar_Dias.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
                        //        Negocio_Consultar_Dias = Negocio_Consultar_Dias.Consultar_Datos_Solicitud();

                        //        Regresar_Solicitud.P_SubProceso_Anterior = Negocio_Consultar_Dias.P_SubProceso_Anterior;
                        //        Regresar_Solicitud.P_Estatus = Cmb_Evaluacion.SelectedItem.Value;
                        //        Regresar_Solicitud.P_Tramite_id = Negocio_Consultar_Dias.P_Tramite_id;

                        //        Regresar_Solicitud.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
                        //        Regresar_Solicitud.P_Usuario = Cls_Sessiones.Nombre_Empleado;

                        //        //  campo comentarios
                        //        Regresar_Solicitud.P_Comentarios = "Regreso a la actividad anterior por el director general de ordenamiento";
                        //        Regresar_Solicitud.P_Comentarios_Internos = "Re regreso a la actividad anterior por el director general de ordenamiento";
                        //        Regresar_Solicitud = Regresar_Solicitud.Evaluar_Solicitud();


                        //        Llenar_Grid_Listado(Grid_Listado.PageIndex);
                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo", "alert('Actualización Exitosa');", true);
                        //        Btn_Modificar.AlternateText = "Modificar";
                        //        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        //        Btn_Salir.AlternateText = "Salir";
                        //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        //        Inicializar_Controles();
                        //    }
                        //}
                        //Inicializar_Controles();
                        #endregion Aprobar
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

    #endregion

    #region RadioButtonList
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: RBtn_Notificado_OnSelectedIndexChanged
        /// DESCRIPCION : se habilitara el numero de foliio
        /// PARAMETROS: 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 04/Junio/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void RBtn_Notificado_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (RBtn_Notificado.SelectedValue == "NO")
                {
                    Txt_Notificacion_Folio.Enabled = false;
                    Txt_Notificacion_Folio.Text = "";
                }
                else
                {
                    Txt_Notificacion_Folio.Enabled = true;
                    Txt_Notificacion_Folio.Text = "";
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
        /// NOMBRE DE LA FUNCION: RBtn_Acta_Inspeccion_OnSelectedIndexChanged
        /// DESCRIPCION : se habilitara el numero de foliio
        /// PARAMETROS: 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 04/Junio/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void RBtn_Acta_Inspeccion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (RBtn_Acta_Inspeccion.SelectedValue == "NO")
                {
                    Txt_Acta_Inspeccion_Folio.Enabled = false;
                    Txt_Acta_Inspeccion_Folio.Text = "";
                }
                else
                {
                    Txt_Acta_Inspeccion_Folio.Enabled = true;
                    Txt_Acta_Inspeccion_Folio.Text = "";
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
        /// NOMBRE DE LA FUNCION: RBtn_Clausurado_OnSelectedIndexChanged
        /// DESCRIPCION : se habilitara el numero de foliio
        /// PARAMETROS: 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 04/Junio/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void RBtn_Clausurado_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (RBtn_Clausurado.SelectedValue == "NO")
                {
                    Txt_Clausurado_Folio.Enabled = false;
                    Txt_Clausurado_Folio.Text = "";
                }
                else
                {
                    Txt_Clausurado_Folio.Enabled = true;
                    Txt_Clausurado_Folio.Text = "";
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
        /// NOMBRE DE LA FUNCION: RBtn_Multado_OnSelectedIndexChanged
        /// DESCRIPCION : se habilitara el numero de foliio
        /// PARAMETROS: 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 04/Junio/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void RBtn_Multado_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (RBtn_Multado.SelectedValue == "NO")
                {
                    Txt_Multa_Folio.Enabled = false;
                    Txt_Multa_Folio.Text = "";
                    
                }
                else
                {
                    Txt_Multa_Folio.Enabled = true;
                    Txt_Multa_Folio.Text = "";
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
        /// NOMBRE DE LA FUNCION: RBtn_Uso_Diferente_Adicional_OnSelectedIndexChanged
        /// DESCRIPCION : se habilitara el numero de folio
        /// PARAMETROS: 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 04/Junio/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void RBtn_Uso_Diferente_Adicional_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (RBtn_Uso_Diferente_Adicional.SelectedValue == "NO")
                {
                    Txt_Especificar_Tipo_Uso.Enabled = false;
                    Txt_Especificar_Tipo_Uso.Text = "";

                }
                else
                {
                    Txt_Especificar_Tipo_Uso.Enabled = true;
                    Txt_Especificar_Tipo_Uso.Text = "";
                }

            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }

    #endregion

    #region Combo
        
        ///*******************************************************************************
        ///NOMBRE:      Cmb_Colonias_OnSelectedIndexChanged
        ///DESCRIPCIÓN: se cargara la colonia 
        ///PARAMETROS:
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Cmb_Colonias_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            DataTable Dt_Calles = new DataTable();
            DataTable Dt_Colonias = new DataTable();
            try
            {
                Negocio_Consulta.P_Colonia_ID = Cmb_Colonias.SelectedValue;
                Dt_Calles = Negocio_Consulta.Consultar_Calles();

                Cargar_Combo_Calles(Dt_Calles);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

    #endregion

    #region Grid
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Formatos_OnSelectedIndexChanged
        ///DESCRIPCIÓN          : permitira el llenado del formato
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Grid_Formatos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Limpiar_Controles();            
                GridViewRow selectedRow = Grid_Formatos.Rows[Grid_Formatos.SelectedIndex];
                Hdf_Solicitud_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
                Hdf_Tramite_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
                Hdf_Subproceso_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[3].Text).ToString();

                Div_Lista_Formatos.Style.Value = "display:none";
                Div_Principal_Llenado_Formato.Style.Value = "display:block";
                Btn_Nuevo.Visible = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Inicializar_Controles " + ex.Message.ToString());
            }
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Formatos_Formatos_RowDataBound
        ///DESCRIPCIÓN: Evento de RowDataBound del Grid de formatos.
        ///PARAMETROS:     
        ///CREO: Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO: 09/Junio/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        protected void Grid_Formatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio Negocio_Formato_Ficha_Inspeccion = new Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio();
            DataTable Dt_Consulta_Formato = new DataTable();
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    System.Web.UI.WebControls.ImageButton Btn_Solicitar = (System.Web.UI.WebControls.ImageButton)e.Row.Cells[0].FindControl("Btn_Autorizar_Formato");

                    Negocio_Formato_Ficha_Inspeccion.P_Tabla = Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana;
                    Negocio_Formato_Ficha_Inspeccion.P_Solicitud_ID = e.Row.Cells[1].Text;
                    Negocio_Formato_Ficha_Inspeccion.P_Subproceso_ID = e.Row.Cells[3].Text;
                    Dt_Consulta_Formato = Negocio_Formato_Ficha_Inspeccion.Consultar_Formato_Existente();

                    if (Dt_Consulta_Formato != null && Dt_Consulta_Formato.Rows.Count > 0)
                    {
                        Btn_Solicitar.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Grid_Formatos_OnSelectedIndexChanged " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_SelectedIndexChanged
        ///DESCRIPCIÓN         : Obtiene el elemento seleccionado.
        ///PARÁMETROS          :     
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Grid_Listado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Listado.SelectedIndex > (-1))
                {
                    Hdf_Administracion_Urbana_ID.Value = HttpUtility.HtmlDecode(Grid_Listado.SelectedRow.Cells[1].Text.Trim());
                    Hdf_Tramite_ID.Value = HttpUtility.HtmlDecode(Grid_Listado.SelectedRow.Cells[2].Text.Trim());
                    Hdf_Solicitud_ID.Value = HttpUtility.HtmlDecode(Grid_Listado.SelectedRow.Cells[3].Text.Trim());
                    Hdf_Subproceso_ID.Value = HttpUtility.HtmlDecode(Grid_Listado.SelectedRow.Cells[4].Text.Trim());
                    Mostrar_Datos();
                    Btn_Modificar.Visible = true;
                    Pnl_Modificar_Cedula.Style.Value = "display:none";
                    Pnl_Captura_Cedula.Style.Value = "display:none";
                    System.Threading.Thread.Sleep(500);
                }
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            }
        }

    #endregion

    #region Combos
        
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Colonia_Click
        ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la colonia seleccionada en la 
        ///             búsqueda avanzada
        ///PARAMETROS: 
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17/may/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        protected void Btn_Buscar_Colonia_Click(object sender, ImageClickEventArgs e)
        {
            // validar que la variable de sesión existe
            if (Session["BUSQUEDA_COLONIAS"] != null)
            {
                // si el valor de la sesión es igual a true
                if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS"]) == true)
                {
                    try
                    {
                        string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                        // si el combo colonias contiene la colonia con el ID, seleccionar
                        if (Cmb_Colonias.Items.FindByValue(Colonia_ID) != null)
                        {
                            Cmb_Colonias.SelectedValue = Colonia_ID;
                            Cmb_Colonias_OnSelectedIndexChanged(null, null);
                        }
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception(Ex.Message.ToString());
                        //Mostrar_Informacion(Ex.Message, true);
                    }

                    // limpiar variables de sesión
                    Session.Remove("COLONIA_ID");
                    Session.Remove("NOMBRE_COLONIA");
                }
                // limpiar variable de sesión
                Session.Remove("BUSQUEDA_COLONIAS");
            }
        }
        
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Calles_Click
        ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la calle seleccionada en la 
        ///             búsqueda avanzada
        ///PARAMETROS: 
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 17/may/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        protected void Btn_Buscar_Calles_Click(object sender, ImageClickEventArgs e)
        {
            // validar que la variable de sesión existe
            if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
            {
                // si el valor de la sesión es igual a true
                if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
                {
                    var Obj_Calles = new Cls_Cat_Pre_Calles_Negocio();

                    try
                    {
                        string Calle_ID = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                        string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                        // consultar las calles de la colonia a la que pertenece la calle seleccionada
                        Cmb_Calle.Items.Clear();
                        Obj_Calles.P_Colonia_ID = Colonia_ID;
                        Llenar_Combo_Con_DataTable(Cmb_Calle, Obj_Calles.Consultar_Calles(), 0, 5);
                        // si el combo colonias contiene la colonia con el ID, seleccionar
                        if (Cmb_Colonias.Items.FindByValue(Colonia_ID) != null)
                        {
                            Cmb_Colonias.SelectedValue = Colonia_ID;
                        }
                        // si el combo calles contiene un elemento con el ID, seleccionar
                        if (Cmb_Calle.Items.FindByValue(Calle_ID) != null)
                        {
                            Cmb_Calle.SelectedValue = Calle_ID;
                        }
                    }
                    catch (Exception Ex)
                    {
                        throw new Exception(Ex.Message.ToString());
                    }

                    // limpiar variables de sesión
                    Session.Remove("COLONIA_ID");
                    Session.Remove("CLAVE_COLONIA");
                    Session.Remove("CALLE_ID");
                    Session.Remove("CLAVE_CALLE");
                }
                // limpiar variable de sesión
                Session.Remove("BUSQUEDA_COLONIAS_CALLES");
            }
        }

    #endregion

}
