using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Cat_Identificadores_Predio.Negocio;
using Presidencia.Operacion_Predial_Convenios_Impuestos_Traslado_Dominio.Negocio;
using System.Data;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Constantes;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;

public partial class paginas_Catastro_Frm_Cat_Cat_Identificadores_Predio : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN:
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(true);
                Session.Remove("ESTATUS_CUENTAS");
                Session.Remove("TIPO_CONTRIBUYENTE");
                Session["ESTATUS_CUENTAS"] = "IN ('PENDIENTE','ACTIVA','VIGENTE','BLOQUEADA','SUSPENDIDA','CANCELADA')";
                String Ventana_Modal1 = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:700px;dialogHeight:420px;dialogHide:true;help:no;scroll:no');";
                Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal1);
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Enabled)
    {
        Txt_Region.Enabled = !Enabled;
        Txt_Manzana.Enabled = !Enabled;
        Txt_Lote.Enabled = !Enabled;
        Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = !Enabled;
        Txt_X_Horas.Enabled = !Enabled;
        Txt_X_Minutos.Enabled = !Enabled;
        Txt_X_Segundos.Enabled = !Enabled;
        Cmb_Latitud.Enabled = !Enabled;
        Txt_Y_Horas.Enabled = !Enabled;
        Txt_Y_Minutos.Enabled = !Enabled;
        Txt_Y_Segundos.Enabled = !Enabled;
        Cmb_Longitud.Enabled = !Enabled;
        Txt_Coordenadas_UTM.Enabled = !Enabled;      
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Formulario()
    {
        Txt_Region.Text = "";
        Txt_Manzana.Text = "";
        Txt_Lote.Text = "";
        Txt_Cuenta_Predial.Text = "";
        Txt_Propietario.Text = "";
        Txt_Calle.Text = "";
        Txt_Colonia.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_X_Horas.Text = "";
        Txt_X_Minutos.Text = "";
        Txt_X_Segundos.Text = "";
        Cmb_Latitud.SelectedIndex = 0;
        Txt_Y_Horas.Text = "";
        Txt_Y_Minutos.Text = "";
        Txt_Y_Segundos.Text = "";
        Cmb_Longitud.SelectedIndex = 0;
        Txt_Tipo_Predio.Text = "";
        Txt_Superficie_Construida.Text = "";
        Txt_Superficie_Predio.Text = "";
        Txt_Coordenadas_UTM.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del botón modificar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Limpiar_Formulario();
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Cat_Cat_Identificadores_Predio_Negocio Identificadores = new Cls_Cat_Cat_Identificadores_Predio_Negocio();
                        Identificadores.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
                        if (Txt_Tipo_Predio.Text.Trim() != "RUSTICO")
                        {
                            Identificadores.P_Region = Txt_Region.Text.ToUpper().Trim();
                            Identificadores.P_Manzana = Txt_Manzana.Text.ToUpper().Trim();
                            Identificadores.P_Lote = Txt_Lote.Text.ToUpper().Trim();
                            Identificadores.P_Horas_X = "";
                            Identificadores.P_Minutos_X = "";
                            Identificadores.P_Segundos_X = "";
                            Identificadores.P_Coordenadas_UTM = "";
                            Identificadores.P_Horas_Y = "";
                            Identificadores.P_Minutos_Y = "";
                            Identificadores.P_Segundos_Y = "";
                            Identificadores.P_Tipo = "";
                            Identificadores.P_Coordenadas_UTM = "";
                            Identificadores.P_Coordenadas_UTM_Y = "";
                        }
                        else
                        {
                            if (Cmb_Coordenadas.SelectedValue == "CART")
                            {
                                Identificadores.P_Tipo = Cmb_Coordenadas.SelectedValue;
                                Identificadores.P_Coordenadas_UTM = "";
                                Identificadores.P_Coordenadas_UTM_Y = "";
                                Identificadores.P_Horas_X = Txt_X_Horas.Text.ToUpper().Trim();
                                Identificadores.P_Minutos_X = Txt_X_Minutos.Text.ToUpper().Trim();
                                Identificadores.P_Segundos_X = Txt_X_Segundos.Text.ToUpper().Trim();
                                Identificadores.P_Orientacion_X = Cmb_Latitud.SelectedValue;
                                Identificadores.P_Horas_Y = Txt_Y_Horas.Text.ToUpper().Trim();
                                Identificadores.P_Minutos_Y = Txt_Y_Minutos.Text.ToUpper().Trim();
                                Identificadores.P_Segundos_Y = Txt_Y_Segundos.Text.ToUpper().Trim();
                                Identificadores.P_Orientacion_Y = Cmb_Longitud.SelectedValue;
                            }
                            else if (Cmb_Coordenadas.SelectedValue == "UTM")
                            {
                                Identificadores.P_Tipo = Cmb_Coordenadas.SelectedValue;
                                Identificadores.P_Coordenadas_UTM = Txt_Coordenadas_UTM.Text.ToUpper();
                                Identificadores.P_Coordenadas_UTM_Y = Txt_Coordenadas_UTM_Y.Text.ToUpper();
                                Identificadores.P_Horas_X = "";
                                Identificadores.P_Minutos_X = "";
                                Identificadores.P_Segundos_X = "";
                                Identificadores.P_Orientacion_X = "";
                                Identificadores.P_Horas_Y = "";
                                Identificadores.P_Minutos_Y = "";
                                Identificadores.P_Segundos_Y = "";
                                Identificadores.P_Orientacion_Y = "";
                            }
                        }                        
                        if ((Identificadores.Modificar_Identificadores_Predio()))
                        {
                            Configuracion_Formulario(true);
                            Btn_Modificar.AlternateText = "Modificar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Div_Identificadores_Predio.Visible = true;
                            Div_Coordenadas.Visible = true;
                            Lbl_Colonia.Text = "Colonia";
                            Div_Cartograficas.Visible = true;
                            Div_Coordenadas.Visible = false;
                            Limpiar_Formulario();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Identificadores de Predio", "alert('Actualización Exitosa.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Identificadores de Predio", "alert('Error al intentar Actualizar.');", true);
                        }
                    }
                }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del botón salir
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Limpiar_Formulario();
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(true);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Lbl_Colonia.Text = "Colonia";
            Div_Coordenadas.Visible = true;
            Div_Identificadores_Predio.Visible = true;

        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Ubicaciones;
        String Cuenta_Predial_ID;
        String Cuenta_Predial;
        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Limpiar_Formulario();
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdf_Cuenta_Predial_Id.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Cuenta_Pendiente = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                Cuenta_Pendiente.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_Id.Value;
                if (!Cuenta_Pendiente.Consultar_Cuenta_Pendiente())
                {
                    Cargar_Datos();
                }
                    
                else
                {
                    Txt_Cuenta_Predial_TextChanged();
                }
                Consultar_Identificadores_Predio();
            }
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Cuenta_Predial_TextChanged
    ///DESCRIPCIÓN          : 
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Cuenta_Predial_TextChanged()
    {
        DataTable Dt_Orden;
        if (Hdf_Cuenta_Predial_Id.Value.Length > 0)
        {
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuenta.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_Id.Value;
            Cuenta = Cuenta.Consultar_Datos_Propietario();
            Txt_Calle.Text = Cuenta.P_Nombre_Calle;
            Txt_Propietario.Text = Cuenta.P_Nombre_Propietario;
            Txt_Colonia.Text = Cuenta.P_Nombre_Colonia;
            Txt_No_Exterior.Text = Cuenta.P_No_Exterior;
            Txt_No_Interior.Text = Cuenta.P_No_Interior;          
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Orden.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_Id.Value;
            Dt_Orden = Orden.Consultar_Ordenes_Variacion();
            if (Dt_Orden.Rows.Count == 0)
            {
                return;
            }
            Orden.P_Año = Convert.ToInt32(Dt_Orden.Rows[0][Ope_Pre_Orden_Variacion.Campo_Anio].ToString());
            Orden.P_Orden_Variacion_ID = Dt_Orden.Rows[0][Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion].ToString();
            Dt_Orden = Orden.Consultar_Domicilio_Y_Propietario();
            if (Dt_Orden.Rows.Count > 0)
            {
                String Dom_Foraneo = "";
                String No_int_not = "";
                String No_ext_not = "";
                String No_Int = "";
                String No_Ext = "";
                String Dom_Not_Colonia = "";
                String Dom_Not_Calle = "";
                String Dom_Colonia = "";
                String Dom_Calle = "";
                foreach (DataRow Renglon_Actual in Dt_Orden.Rows)
                {
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo].ToString() != "")
                    {
                        Dom_Foraneo = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion].ToString() != "")
                    {
                        No_int_not = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion].ToString() != "")
                    {
                        No_ext_not = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString() != "")
                    {
                        No_Int = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString() != "")
                    {
                        No_Ext = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion].ToString() != "")
                    {
                        Dom_Not_Calle = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion].ToString() != "")
                    {
                        Dom_Not_Colonia = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion].ToString() != "")
                    {
                        Dom_Calle = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion].ToString();
                    }
                    if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion].ToString() != "")
                    {
                        Dom_Colonia = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion].ToString();
                    }
                }
                Txt_Propietario.Text = Dt_Orden.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString();
                if (Dom_Foraneo == "SI" && Dom_Foraneo != "")
                {
                    Txt_Calle.Text = Dom_Calle;
                    Txt_Colonia.Text = Dom_Colonia;
                    Txt_No_Exterior.Text = No_ext_not;
                    Txt_No_Interior.Text = No_int_not;
                }
                else if (Dom_Foraneo == "NO" && Dom_Foraneo != "")
                {
                    Cls_Cat_Pre_Calles_Negocio Calle = new Cls_Cat_Pre_Calles_Negocio();
                    Calle.P_Calle_ID = Dom_Not_Calle;
                    Calle.P_Mostrar_Nombre_Calle_Nombre_Colonia = true;
                    if (Calle.P_Calle_ID != "")
                    {
                        DataTable Dt_Calle_Colonia = Calle.Consultar_Nombre_Id_Calles();
                        String[] Calle_Col = Dt_Calle_Colonia.Rows[0]["NOMBRE"].ToString().Split('-');
                        Txt_Calle.Text = Calle_Col[0];
                        Txt_Colonia.Text = Calle_Col[1];
                    }
                    Txt_No_Exterior.Text = No_Ext;
                    Txt_No_Interior.Text = No_Int;
                }
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Datos
    ///DESCRIPCIÓN          : 
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Datos()
    {
        try
        {
            if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
            {
                //KONSULTA DATOS CUENTA HACER DS
                Busqueda_Cuentas();
                //LLENAR CAJAS
                if (Session["Ds_Cuenta_Datos"] != null)
                {
                    Cargar_Generales_Cuenta(((DataSet)Session["Ds_Cuenta_Datos"]).Tables["Dt_Generales"]);
                    Busqueda_Propietario();
                }

            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio;

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Busqueda_Cuentas
    ///DESCRIPCIÓN          : realiza en vase a un id la busqueda
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Busqueda_Cuentas()
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataSet Ds_Cuenta;
        try
        {
            Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Ds_Cuenta = Resumen_Predio.Consulta_Datos_Cuenta_Generales();
            if (Ds_Cuenta.Tables[0].Rows.Count - 1 >= 0)
            {
                if (Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_Id"].ToString().Trim() != string.Empty)
                {
                    Session["Cuenta_Predial_ID"] = Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_Id"].ToString().Trim();
                }
            }
            if (Ds_Cuenta.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Cuenta_Datos");
                M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                Session["Ds_Cuenta_Datos"] = Ds_Cuenta;
            }
            else
            {
                
            }
        }
        catch (Exception Ex)
        {   }
    }


    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Generales_Cuenta
    ///DESCRIPCIÓN: asignar datos generales de cuenta a los controles y objeto de negocio
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Cargar_Generales_Cuenta(DataTable dataTable)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        try
        {
            Div_Identificadores_Predio.Visible = true;
            Div_Coordenadas.Visible = true;
            Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Tipo_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                DataTable Dt_Tipo_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tipo_Predio();
                Txt_Tipo_Predio.Text = Dt_Tipo_Predio.Rows[0]["DESCRIPCION"].ToString();

            }
            if (Txt_Tipo_Predio.Text == "RUSTICO")
            {
                Div_Identificadores_Predio.Visible = false;
                Div_Coordenadas.Visible = true;
                Lbl_Colonia.Text = "Localidad";
            }
            else if (Txt_Tipo_Predio.Text == "URBANO")
            {
                Div_Coordenadas.Visible = false;
                Lbl_Colonia.Text = "Colonia";
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Uso_Suelo_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
                DataTable Dt_Uso_Suelo = Rs_Consulta_Ope_Resumen_Predio.Consultar_Uso_Predio();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
                DataTable Dt_Estado_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                DataTable Dt_Calles = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                Txt_Calle.Text = Dt_Calles.Rows[0]["Nombre"].ToString();
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = Dt_Calles.Rows[0]["Colonia_ID"].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Colonia.Text = Dt_Colonia.Rows[0]["Nombre"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString() != string.Empty)
            {
                Txt_No_Exterior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString() != string.Empty)
            {
                Txt_No_Interior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            }
            Txt_Superficie_Construida.Text = dataTable.Rows[0]["Superficie_Construida"].ToString();            
            Txt_Superficie_Predio.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Busqueda_Propietario
    ///DESCRIPCIÓN: asignar datos de propietario de la cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:44:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Busqueda_Propietario()
    {
        DataSet Ds_Prop;
        String Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
        try
        {
            M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
            Ds_Prop = M_Orden_Negocio.Consulta_Datos_Propietario();
            if (Ds_Prop.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Prop_Datos");
                Session["Ds_Prop_Datos"] = Ds_Prop;
                Cargar_Datos_Propietario(((DataSet)Session["Ds_Prop_Datos"]).Tables["Dt_Propietarios"]);
            }
        }
        catch (Exception Ex)
        {
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Propietario
    ///DESCRIPCIÓN: asignar datos de propietario de la cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:44:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos_Propietario(DataTable dataTable)
    {
        try
        {
            if (dataTable.Rows.Count > 0 && dataTable != null)
            {
                M_Orden_Negocio.P_Propietario_ID = dataTable.Rows[0]["PROPIETARIO"].ToString(); ;

                Txt_Propietario.Text = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
            }

        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Propietario: " + Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Valida que los datos esten ingresados
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Componentes()
    {
        String Mensaje_Error = "Error: ";
        Boolean valido = true;

        if (Txt_Cuenta_Predial.Text.Trim() == "" || Hdf_Cuenta_Predial_Id.Value.Trim()=="" )
        {
            if (Mensaje_Error.Length > 0)
            {
                Mensaje_Error += "<br/>";
            }
            Mensaje_Error += "+ Seleccione una Cuenta Predial.";
            valido = false;
        }
        if (Txt_Tipo_Predio.Text.Trim() != "RUSTICO")
        {
            if (Txt_Region.Text.Trim() == "")
            {
                if (Mensaje_Error.Length > 0)
                {
                    Mensaje_Error += "<br/>";
                }
                Mensaje_Error += "+ Ingrese la Región.";
                valido = false;
            }
            if (Txt_Manzana.Text.Trim() == "")
            {
                if (Mensaje_Error.Length > 0)
                {
                    Mensaje_Error += "<br/>";
                }
                Mensaje_Error += "+ Ingrese la Manzana.";
                valido = false;
            }
            if (Txt_Lote.Text.Trim() == "")
            {
                if (Mensaje_Error.Length > 0)
                {
                    Mensaje_Error += "<br/>";
                }
                Mensaje_Error += "+ Ingrese el Lote.";
                valido = false;
            }
        }
        else
        {
            if (Cmb_Coordenadas.SelectedValue == "CART")
            {
                if (Txt_X_Horas.Text.Trim() == "")
                {
                    if (Mensaje_Error.Length > 0)
                    {
                        Mensaje_Error += "<br/>";
                    }
                    Mensaje_Error += "+ Ingrese las horas en X.";
                    valido = false;
                }
                if (Txt_X_Minutos.Text.Trim() == "")
                {
                    if (Mensaje_Error.Length > 0)
                    {
                        Mensaje_Error += "<br/>";
                    }
                    Mensaje_Error += "+ Ingrese los minutos en X.";
                    valido = false;
                }
                if (Txt_X_Segundos.Text.Trim() == "")
                {
                    if (Mensaje_Error.Length > 0)
                    {
                        Mensaje_Error += "<br/>";
                    }
                    Mensaje_Error += "+ Ingrese los segundos en X.";
                    valido = false;
                }
                if (Cmb_Latitud.SelectedIndex == 0)
                {
                    if (Mensaje_Error.Length > 0)
                    {
                        Mensaje_Error += "<br/>";
                    }
                    Mensaje_Error += "+ Ingrese la Orientación en X.";
                    valido = false;
                }
                if (Txt_Y_Horas.Text.Trim() == "")
                {
                    if (Mensaje_Error.Length > 0)
                    {
                        Mensaje_Error += "<br/>";
                    }
                    Mensaje_Error += "+ Ingrese las horas en Y.";
                    valido = false;
                }
                if (Txt_Y_Minutos.Text.Trim() == "")
                {
                    if (Mensaje_Error.Length > 0)
                    {
                        Mensaje_Error += "<br/>";
                    }
                    Mensaje_Error += "+ Ingrese los minutos en Y.";
                    valido = false;
                }
                if (Txt_Y_Segundos.Text.Trim() == "")
                {
                    if (Mensaje_Error.Length > 0)
                    {
                        Mensaje_Error += "<br/>";
                    }
                    Mensaje_Error += "+ Ingrese los segundos en Y.";
                    valido = false;
                }
                if (Cmb_Longitud.SelectedIndex == 0)
                {
                    if (Mensaje_Error.Length > 0)
                    {
                        Mensaje_Error += "<br/>";
                    }
                    Mensaje_Error += "+ Seleccione la orientación en Y.";
                    valido = false;
                }
            }
            else if (Cmb_Coordenadas.SelectedValue == "UTM")
            {
                if (Txt_Coordenadas_UTM.Text.Trim() == "")
                {
                    if (Mensaje_Error.Length > 0)
                    {
                        Mensaje_Error += "<br/>";
                    }
                    Mensaje_Error += "+ Ingrese las cordenadas UTM en X.";
                    valido = false;
                }
                if (Txt_Coordenadas_UTM_Y.Text.Trim() == "")
                {
                    if (Mensaje_Error.Length > 0)
                    {
                        Mensaje_Error += "<br/>";
                    }
                    Mensaje_Error += "+ Ingrese las cordenadas UTM en Y.";
                    valido = false;
                }
            }
            else
            {
                if (Mensaje_Error.Length > 0)
                {
                    Mensaje_Error += "<br/>";
                }
                Mensaje_Error += "+ Seleccione Coordenadas Cartograficas o UTM.";
                valido = false;
            }
        }
        if (!valido)
        {
            Lbl_Ecabezado_Mensaje.Text = Mensaje_Error;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return valido;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Consultar_Identificadores_Predio
    ///DESCRIPCIÓN: Obtiene el lote, región y manzana.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Consultar_Identificadores_Predio()
    {
        try
        {
            DataTable Dt_Identificadores;
            Cls_Cat_Cat_Identificadores_Predio_Negocio Identificadores = new Cls_Cat_Cat_Identificadores_Predio_Negocio();
            Identificadores.P_Cuenta_Predial_Id = Hdf_Cuenta_Predial_Id.Value;
            Dt_Identificadores = Identificadores.Consultar_Identificadores_Predio();
            if (Dt_Identificadores.Rows.Count > 0)
            {
                Txt_Region.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Region].ToString().Trim();
                Txt_Manzana.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Manzana].ToString().Trim();
                Txt_Lote.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Lote].ToString().Trim();
                Txt_X_Horas.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Horas_X].ToString().Trim();
                Txt_X_Minutos.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Minutos_X].ToString().Trim();
                Txt_X_Segundos.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Segundos_X].ToString().Trim();                
                if (Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Orientacion_X].ToString().Trim() != "")
                {
                    Cmb_Latitud.SelectedValue = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Orientacion_X].ToString().Trim();
                }
                else
                {
                    Cmb_Latitud.SelectedValue = "SELECCIONE";
                }
                Txt_Y_Horas.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Horas_Y].ToString().Trim();
                Txt_Y_Minutos.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Minutos_Y].ToString().Trim();
                Txt_Y_Segundos.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Segundos_Y].ToString().Trim();
                if (Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Orientacion_Y].ToString().Trim() != "")
                {
                    Cmb_Longitud.SelectedValue = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Orientacion_Y].ToString().Trim();
                }
                else
                {
                    Cmb_Longitud.SelectedValue = "SELECCIONE";
                }
                Txt_Coordenadas_UTM.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Coordenadas_UTM].ToString().Trim();
                Txt_Coordenadas_UTM.Text = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Coordenadas_UTM_Y].ToString().Trim();
                if (Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo].ToString().Trim() == "")
                {
                    Cmb_Coordenadas.SelectedIndex = 0;
                }
                else
                {
                    Cmb_Coordenadas.SelectedValue = Dt_Identificadores.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo].ToString().Trim();
                }
                Cmb_Coordenadas_SelectedIndexChanged(null, null);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Consultar_Identificadores_Predio: " + Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Coordenadas_SelectedIndexChanged
    ///DESCRIPCIÓN: reacciona al evento del combo coordenadas
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Coordenadas_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Coordenadas.SelectedValue == "UTM")
        {
            Div_UTM.Visible = true;
            Div_Cartograficas.Visible = false;
        }
        else if (Cmb_Coordenadas.SelectedValue == "CART")
        {
            Div_Cartograficas.Visible = true;
            Div_UTM.Visible = false;
        }
        else
        {
            Div_Cartograficas.Visible = false;
            Div_UTM.Visible = false;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_X_Segundos_TextChanged
    ///DESCRIPCIÓN: reacciona al evento del Txt_X_Segundos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_X_Segundos_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_X_Segundos.Text.Trim() == "")
            {
                Txt_X_Segundos.Text = "";
            }
            else
            {
                Double Segundos = Convert.ToDouble(Txt_X_Segundos.Text);
                if (Segundos > 60)
                {
                    Segundos = 60;
                }
                Txt_X_Segundos.Text = (Segundos).ToString("#0.00");
            }
        }
        catch(Exception Exc)
        {
            Txt_X_Segundos.Text = "";
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_X_Minutos_TextChanged
    ///DESCRIPCIÓN: reacciona al evento del Txt_X_Minutos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_X_Minutos_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_X_Minutos.Text.Trim() == "")
            {
                Txt_X_Minutos.Text = "";
            }
            else
            {
                Double Minutos = Convert.ToDouble(Txt_X_Minutos.Text);
                if (Minutos > 60)
                {
                    Minutos = 60;
                }
                Txt_X_Minutos.Text = (Minutos).ToString("#0");
            }
        }
        catch (Exception Exc)
        {
            Txt_X_Minutos.Text = "";
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_X_Horas_TextChanged
    ///DESCRIPCIÓN: reacciona al evento del Txt_X_Horas
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_X_Horas_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_X_Horas.Text.Trim() == "")
            {
                Txt_X_Horas.Text = "";
            }
            else
            {
                Double Horas = Convert.ToDouble(Txt_X_Horas.Text);
                if (Horas > 160)
                {
                    Horas = 160;
                }
                Txt_X_Horas.Text = (Horas).ToString("##0");
            }
        }
        catch (Exception Exc)
        {
            Txt_X_Horas.Text = "";
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_y_Horas_TextChanged
    ///DESCRIPCIÓN: reacciona al evento del Txt_Y_Horas
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Y_Horas_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Y_Horas.Text.Trim() == "")
            {
                Txt_Y_Horas.Text = "";
            }
            else
            {
                Double Horas = Convert.ToDouble(Txt_Y_Horas.Text);
                if (Horas > 160)
                {
                    Horas = 160;
                }
                Txt_Y_Horas.Text = (Horas).ToString("##0");
            }
        }
        catch (Exception Exc)
        {
            Txt_Y_Horas.Text = "";
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_y_Horas_TextChanged
    ///DESCRIPCIÓN: reacciona al evento del Txt_Y_Horas
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Y_Minutos_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Y_Minutos.Text.Trim() == "")
            {
                Txt_Y_Minutos.Text = "";
            }
            else
            {
                Double Minutos = Convert.ToDouble(Txt_Y_Minutos.Text);
                if (Minutos > 60)
                {
                    Minutos = 60;
                }
                Txt_Y_Minutos.Text = (Minutos).ToString("#0");
            }
        }
        catch (Exception Exc)
        {
            Txt_Y_Minutos.Text = "";
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Y_Segundos_TextChanged
    ///DESCRIPCIÓN: reacciona al evento del Txt_Y_Segundos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/May/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Y_Segundos_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Y_Segundos.Text.Trim() == "")
            {
                Txt_Y_Segundos.Text = "";
            }
            else
            {
                Double Segundos = Convert.ToDouble(Txt_Y_Segundos.Text);
                if (Segundos > 60)
                {
                    Segundos = 60;
                }
                Txt_Y_Segundos.Text = (Segundos).ToString("#0.00");
            }
        }
        catch (Exception Exc)
        {
            Txt_Y_Segundos.Text = "";
        }
    }
}