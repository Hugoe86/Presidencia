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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Catalogo_Casos_Especiales.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Reportes;

public partial class paginas_Predial_Frm_Rpt_Pre_Cuentas_Rezago : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load.
    ///DESCRIPCIÓN          : Metodo que se ejecuta en PostBack de la Página.
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides guardado
    ///FECHA_CREO           : 10/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String Ventana_Modal;
            //Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            //Btn_Mostrar_Busqueda_Avanzada_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal);

            //Scrip para mostrar Ventana Modal para la Busqueda Avanzada de colonias y Calles
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Ubicacion.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Notificacion.Attributes.Add("onclick", Ventana_Modal);
            Llenar_Combo_Años();
            Llenar_Combo_Bimestres();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Evento de botón para salir de la página cargada o cancelar los datos mostrados por alguna acción
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Enero/2012
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
            Limpiar_Campos();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
    }

    protected void Txt_Cuenta_Predial_TextChanged(object sender, EventArgs e)
    {

    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    /////DESCRIPCIÓN          : Muestra la Ventana Emergente para consultar Cuentas Predial
    /////PARAMETROS:     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 10/Enero/2012
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Mostrar_Busqueda_Avanzada_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    //{
    //    Boolean Busqueda_Ubicaciones;
    //    String Cuenta_Predial_ID;
    //    String Cuenta_Predial;

    //    Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
    //    if (Busqueda_Ubicaciones)
    //    {
    //        if (Session["CUENTA_PREDIAL_ID"] != null)
    //        {
    //            Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
    //            Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
    //            Hdn_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
    //            Txt_Cuenta_Predial.Text = Cuenta_Predial;
    //        }
    //    }
    //    Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
    //    Session.Remove("CUENTA_PREDIAL_ID");
    //    Session.Remove("CUENTA_PREDIAL");
    //}

    ///NOMBRE DE LA FUNCIÓN : Limpiar_Campos
    ///DESCRIPCIÓN          : Quita los textos u opciones seleccionadas de los campos de la página
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Limpiar_Campos()
    {
        Txt_Calle_Ubicacion.Text = "";
        Hdn_Calle_ID_Ubicacion.Value = "";
        Txt_Colonia_Ubicacion.Text = "";
        Hdn_Colonia_ID_Ubicacion.Value = "";
        Txt_Interior_Ubicacion.Text = "";
        Txt_Exterior_Ubicacion.Text = "";
        Txt_Calle_Notificacion.Text = "";
        Hdn_Calle_ID_Notificacion.Value = "";
        Txt_Colonia_Notificacion.Text = "";
        Hdn_Colonia_ID_Notificacion.Value = "";
        Txt_Interior_Notificacion.Text = "";
        Txt_Exterior_Notificacion.Text = "";
        Btn_Por_Rezago.Checked = false;
        Btn_Por_Corriente.Checked = false;
        Txt_Monto_Rezago_Inicial.Text = "";
        Txt_Monto_Rezago_Final.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Imprimir
    ///DESCRIPCIÓN          : Manda a imprimir el reporte con el formato indicado
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Imprimir_Reporte(String Formato)
    {
        String Nombre_Repote_Crystal = "";
        String Nombre_Reporte = "";

        if (Validar_Campos_Obligatorios())
        {
            {
                Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
                Ds_Rpt_Pre_Cuentas_Con_Rezago Reporte_Cuentas_Con_Rezago = new Ds_Rpt_Pre_Cuentas_Con_Rezago();
                Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Generar_Adeudo_Predial = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
                Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
                Cls_Ope_Pre_Parametros_Negocio Consulta_Parametros = new Cls_Ope_Pre_Parametros_Negocio();

                Cuentas.P_Campos_Dinamicos = "DISTINCT ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Cuentas.P_Campos_Dinamicos += "UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_CONTRIBUYENTE, ";
                Cuentas.P_Campos_Dinamicos += "CALLES_CUENTA." + Cat_Pre_Calles.Campo_Nombre + " CALLE_CUENTA, ";
                Cuentas.P_Campos_Dinamicos += "COLONIAS_CUENTA." + Cat_Ate_Colonias.Campo_Nombre + " COLONIA_CUENTA, ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", ";
                Cuentas.P_Campos_Dinamicos += "CASE WHEN NOT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " IS NULL THEN CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Nombre + " ELSE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + " END CALLE_NOTIFICACION, ";
                Cuentas.P_Campos_Dinamicos += "CASE WHEN NOT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " IS NULL THEN COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Nombre + " ELSE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + " END COLONIA_NOTIFICACION, ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
                Cuentas.P_Campos_Dinamicos += "CASE WHEN NOT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " IS NULL THEN " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + "." + Cat_Pre_Ciudades.Campo_Nombre + " ELSE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + " END CIUDAD_NOTIFICACION, ";
                Cuentas.P_Campos_Dinamicos += "CASE WHEN NOT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " IS NULL THEN " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados + "." + Cat_Pre_Estados.Campo_Nombre + " ELSE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + " END ESTADO_NOTIFICACION ";
                Cuentas.P_Unir_Tablas = "";
                Cuentas.P_Unir_Tablas += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + ", " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + ", ";
                Cuentas.P_Unir_Tablas += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_CUENTA, " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_NOTIFICACION, ";
                Cuentas.P_Unir_Tablas += Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_CUENTA, " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_NOTIFICACION, ";
                Cuentas.P_Unir_Tablas += Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + ", ";
                Cuentas.P_Unir_Tablas += Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " ";
                Cuentas.P_Filtros_Dinamicos = "";
                Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AND ";
                Cuentas.P_Filtros_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND ";
                Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = CALLES_CUENTA." + Cat_Pre_Calles.Campo_Calle_ID + "(+) AND ";
                Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = COLONIAS_CUENTA." + Cat_Ate_Colonias.Campo_Colonia_ID + "(+) AND ";
                Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Calle_ID + "(+) AND ";
                Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Colonia_ID + "(+) AND ";
                Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " = " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + "." + Cat_Pre_Ciudades.Campo_Ciudad_ID + "(+) AND ";
                Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados + "." + Cat_Pre_Estados.Campo_Estado_ID + "(+) ";

                if (Hdn_Calle_ID_Ubicacion.Value != "")
                {
                    Cuentas.P_Filtros_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = '" + Hdn_Calle_ID_Ubicacion.Value + "' ";
                }
                if (Hdn_Colonia_ID_Ubicacion.Value != "")
                {
                    Cuentas.P_Filtros_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = '" + Hdn_Colonia_ID_Ubicacion.Value + "' ";
                }

                if (Txt_Interior_Ubicacion.Text.Trim() != "")
                {
                    Cuentas.P_Filtros_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " LIKE '%" + Txt_Interior_Ubicacion.Text.Trim() + "%' ";
                }
                if (Txt_Exterior_Ubicacion.Text.Trim() != "")
                {
                    Cuentas.P_Filtros_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " LIKE '%" + Txt_Exterior_Ubicacion.Text.Trim() + "%' ";
                }

                if (Hdn_Calle_ID_Notificacion.Value != "")
                {
                    Cuentas.P_Filtros_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = '" + Hdn_Calle_ID_Notificacion.Value + "' ";
                }
                else
                {
                    if (Txt_Calle_Notificacion.Text.Trim() != "")
                    {
                        Cuentas.P_Filtros_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + " LIKE '%" + Txt_Calle_Notificacion.Text.Trim() + "%' ";
                    }
                }
                if (Hdn_Colonia_ID_Notificacion.Value != "")
                {
                    Cuentas.P_Filtros_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = '" + Hdn_Colonia_ID_Notificacion.Value + "' ";
                }
                else
                {
                    if (Txt_Colonia_Notificacion.Text.Trim() != "")
                    {
                        Cuentas.P_Filtros_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + " LIKE '%" + Txt_Colonia_Notificacion.Text.Trim() + "%' ";
                    }
                }

                if (Txt_Interior_Notificacion.Text.Trim() != "")
                {
                    Cuentas.P_Filtros_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + " LIKE '%" + Txt_Interior_Notificacion.Text.Trim() + "%' ";
                }
                if (Txt_Exterior_Notificacion.Text.Trim() != "")
                {
                    Cuentas.P_Filtros_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + " LIKE '%" + Txt_Exterior_Notificacion.Text.Trim() + "%' ";
                }

                Cuentas.P_Ordenar_Dinamico = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                DataTable Dt_Datos_Cuentas = Cuentas.Consultar_Datos_Reporte();
                //AGEGAR LOS CAMPOS FALTANTES DE PERIODO E IMPORTE REZAGO, PERIODO E IMPORTE CORRIENTE, RECARGOS, HONORARIOS Y CONVENIDA
                if (Dt_Datos_Cuentas.Rows.Count > 0)
                {
                    if (!Dt_Datos_Cuentas.Columns.Contains("PERIODO_REZAGO"))
                    {
                        Dt_Datos_Cuentas.Columns.Add("PERIODO_REZAGO", typeof(String));
                    }
                    if (!Dt_Datos_Cuentas.Columns.Contains("BIMESTRE_FINAL_REZAGO"))
                    {
                        Dt_Datos_Cuentas.Columns.Add("BIMESTRE_FINAL_REZAGO", typeof(int));
                    }
                    if (!Dt_Datos_Cuentas.Columns.Contains("AÑO_FINAL_REZAGO"))
                    {
                        Dt_Datos_Cuentas.Columns.Add("AÑO_FINAL_REZAGO", typeof(int));
                    }
                    if (!Dt_Datos_Cuentas.Columns.Contains("IMPORTE_REZAGO"))
                    {
                        Dt_Datos_Cuentas.Columns.Add("IMPORTE_REZAGO", typeof(Decimal));
                    }
                    if (!Dt_Datos_Cuentas.Columns.Contains("PERIODO_CORRIENTE"))
                    {
                        Dt_Datos_Cuentas.Columns.Add("PERIODO_CORRIENTE", typeof(String));
                    }
                    if (!Dt_Datos_Cuentas.Columns.Contains("BIMESTRE_FINAL_CORRIENTE"))
                    {
                        Dt_Datos_Cuentas.Columns.Add("BIMESTRE_FINAL_CORRIENTE", typeof(int));
                    }
                    if (!Dt_Datos_Cuentas.Columns.Contains("AÑO_FINAL_CORRIENTE"))
                    {
                        Dt_Datos_Cuentas.Columns.Add("AÑO_FINAL_CORRIENTE", typeof(int));
                    }
                    if (!Dt_Datos_Cuentas.Columns.Contains("IMPORTE_CORRIENTE"))
                    {
                        Dt_Datos_Cuentas.Columns.Add("IMPORTE_CORRIENTE", typeof(Decimal));
                    }
                    if (!Dt_Datos_Cuentas.Columns.Contains("RECARGOS"))
                    {
                        Dt_Datos_Cuentas.Columns.Add("RECARGOS", typeof(Decimal));
                    }
                    if (!Dt_Datos_Cuentas.Columns.Contains("HONORARIOS"))
                    {
                        Dt_Datos_Cuentas.Columns.Add("HONORARIOS", typeof(Decimal));
                    }
                    if (!Dt_Datos_Cuentas.Columns.Contains("CONVENIDA"))
                    {
                        Dt_Datos_Cuentas.Columns.Add("CONVENIDA", typeof(String));
                    }

                    // obtener año corriente
                    String Periodo_Corriente_Inicial = "";
                    String Periodo_Corriente_Final = "";
                    String Periodo_Rezago_Inicial = "";
                    String Periodo_Rezago_Final = "";
                    int Anio_Corriente = Consulta_Parametros.Consultar_Anio_Corriente();
                    // verificar que se obtuvo valor mayor que cero, si no, tomar año actual
                    if (Anio_Corriente <= 0)
                    {
                        Anio_Corriente = DateTime.Now.Year;
                    }

                    foreach (DataRow Dr_Datos_Cuentas in Dt_Datos_Cuentas.Rows)
                    {
                        if (Dr_Datos_Cuentas[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID] != null)
                        {
                            Generar_Adeudo_Predial.Calcular_Recargos_Predial(Dr_Datos_Cuentas[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString());
                            DataTable Dt_Estado_Cuenta = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Dr_Datos_Cuentas[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString(), null, 0, 0);
                            foreach (DataRow Dr_Estado_Cuenta in Dt_Estado_Cuenta.Rows)
                            {
                                Decimal Monto_Adeudo;
                                Int16 Anio_Adeudo;
                                for (Int16 Contador_Bimestre = 1; Contador_Bimestre <= 6; Contador_Bimestre++)
                                {
                                    Decimal.TryParse(Dr_Estado_Cuenta["Adeudo_Bimestre_" + Contador_Bimestre].ToString().Trim(), out Monto_Adeudo);
                                    Int16.TryParse(Dr_Estado_Cuenta["Anio"].ToString(), out Anio_Adeudo);
                                    if (Monto_Adeudo > 0)
                                    {
                                        if (Anio_Adeudo < Anio_Corriente)
                                        {
                                            if (Periodo_Rezago_Inicial.Length <= 0)
                                            {
                                                Periodo_Rezago_Inicial = Contador_Bimestre + "/" + Anio_Adeudo;
                                            }
                                            Periodo_Rezago_Final = Contador_Bimestre + "/" + Anio_Adeudo;
                                        }
                                        if (Anio_Adeudo == Anio_Corriente)
                                        {
                                            if (Periodo_Corriente_Inicial.Length <= 0)
                                            {
                                                Periodo_Corriente_Inicial = Contador_Bimestre + "/" + Anio_Adeudo;
                                            }
                                            Periodo_Corriente_Final = Contador_Bimestre + "/" + Anio_Adeudo;
                                        }
                                    }
                                }
                            }

                            Resumen_Predio.P_Validar_Convenios_Cumplidos = true;
                            Resumen_Predio.P_Cuenta_Predial = Dr_Datos_Cuentas[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
                            DataTable Dt_Resumen_Predio_Convenios = Resumen_Predio.Consultar_Convenios();
                            String Cuenta_Convenida = "";

                            if (Dt_Resumen_Predio_Convenios != null)
                            {
                                if (Dt_Resumen_Predio_Convenios.Rows.Count > 0)
                                {
                                    if (Dt_Resumen_Predio_Convenios.Rows[0]["Estatus"] != null)
                                    {
                                        if (Dt_Resumen_Predio_Convenios.Rows[0]["Estatus"].ToString() == "ACTIVO")
                                        {
                                            foreach (DataRow Dr_Resumen_Predio_Convenios in Dt_Resumen_Predio_Convenios.Rows)
                                            {
                                                //////if (Dr_Resumen_Predio_Convenios["No_Convenio"].ToString() != "")
                                                //////{
                                                //////    Cuenta_Convenida = Dr_Resumen_Predio_Convenios["No_Convenio"].ToString();
                                                //////}
                                                //if (Dr_Resumen_Predio_Convenios["No_Convenio"].ToString().StartsWith("CDER"))
                                                //{
                                                //    Cuenta_Convenida = Dr_Resumen_Predio_Convenios["No_Convenio"].ToString();
                                                //    //Hdn_Tipo_Convenio.Value = "CONVENIDA DS";
                                                //}
                                                //if (Dr_Resumen_Predio_Convenios["No_Convenio"].ToString().StartsWith("CFRA"))
                                                //{
                                                //    Cuenta_Convenida = Dr_Resumen_Predio_Convenios["No_Convenio"].ToString();
                                                //    //Hdn_Tipo_Convenio.Value = "CONVENIDA FRACCIONAMIENTOS";
                                                //}
                                                //if (Dr_Resumen_Predio_Convenios["No_Convenio"].ToString().StartsWith("CTRA"))
                                                //{
                                                //    Cuenta_Convenida = Dr_Resumen_Predio_Convenios["No_Convenio"].ToString();
                                                //    //Hdn_Tipo_Convenio.Value = "CONVENIDA TD";
                                                //}
                                                if (Dr_Resumen_Predio_Convenios["No_Convenio"].ToString().StartsWith("CPRE"))
                                                {
                                                    Cuenta_Convenida = Dr_Resumen_Predio_Convenios["No_Convenio"].ToString();
                                                    //Hdn_Tipo_Convenio.Value = "CONVENIDA PREDIAL";
                                                }
                                            }
                                        }
                                        //else
                                        //{
                                        //    if (Dt_Resumen_Predio_Convenios.Rows[0]["Estatus"].ToString() == "INCUMPLIDO")
                                        //    {
                                        //        Cuenta_Convenida = "CONVENIO INCUMPLIDO";
                                        //    }
                                        //}
                                    }
                                }
                            }

                            Dr_Datos_Cuentas["PERIODO_REZAGO"] = Periodo_Rezago_Inicial + " - " + Periodo_Rezago_Final;
                            if (Periodo_Rezago_Final.Trim() != "")
                            {
                                Dr_Datos_Cuentas["BIMESTRE_FINAL_REZAGO"] = Periodo_Rezago_Final.Trim().Split('/').GetValue(0);
                                Dr_Datos_Cuentas["AÑO_FINAL_REZAGO"] = Periodo_Rezago_Final.Trim().Split('/').GetValue(1);
                            }
                            Dr_Datos_Cuentas["IMPORTE_REZAGO"] = Generar_Adeudo_Predial.p_Total_Rezago;
                            Dr_Datos_Cuentas["PERIODO_CORRIENTE"] = Periodo_Corriente_Inicial + " - " + Periodo_Corriente_Final;
                            if (Periodo_Corriente_Final.Trim() != "")
                            {
                                Dr_Datos_Cuentas["BIMESTRE_FINAL_CORRIENTE"] = Periodo_Corriente_Final.Trim().Split('/').GetValue(0);
                                Dr_Datos_Cuentas["AÑO_FINAL_CORRIENTE"] = Periodo_Corriente_Final.Trim().Split('/').GetValue(1);
                            }
                            Dr_Datos_Cuentas["IMPORTE_CORRIENTE"] = Generar_Adeudo_Predial.p_Total_Corriente;
                            Dr_Datos_Cuentas["RECARGOS"] = Generar_Adeudo_Predial.p_Total_Recargos_Generados;
                            Dr_Datos_Cuentas["HONORARIOS"] = 0;
                            Dr_Datos_Cuentas["CONVENIDA"] = Cuenta_Convenida;
                        }
                    }
                }

                //FILTRA POR TIPO DE ADEUDO
                if (Btn_Por_Rezago.Checked)
                {
                    Dt_Datos_Cuentas = Dt_Datos_Cuentas.Select("IMPORTE_REZAGO <> 0").CopyToDataTable();
                }
                else
                {
                    if (Btn_Por_Corriente.Checked)
                    {
                        Dt_Datos_Cuentas = Dt_Datos_Cuentas.Select("IMPORTE_CORRIENTE <> 0").CopyToDataTable();
                    }
                }
                //FILTRA POR MONTO DE ADEUDO
                if (Txt_Monto_Rezago_Inicial.Text.Trim() != "")
                {
                    Dt_Datos_Cuentas = Dt_Datos_Cuentas.Select("IMPORTE_REZAGO >= " + Txt_Monto_Rezago_Inicial.Text.Trim()).CopyToDataTable();
                }
                if (Txt_Monto_Rezago_Final.Text.Trim() != "")
                {
                    Dt_Datos_Cuentas = Dt_Datos_Cuentas.Select("IMPORTE_REZAGO <= " + Txt_Monto_Rezago_Final.Text.Trim()).CopyToDataTable();
                }
                //FILTRA POR PERIODO FINAL
                Dt_Datos_Cuentas = Dt_Datos_Cuentas.Select("(BIMESTRE_FINAL_REZAGO <= " + Cmb_Bimestres.SelectedItem.Value + " AND AÑO_FINAL_REZAGO <= " + Cmb_Años.SelectedItem.Value + ") OR (BIMESTRE_FINAL_CORRIENTE <= " + Cmb_Bimestres.SelectedItem.Value + " AND AÑO_FINAL_CORRIENTE <= " + Cmb_Años.SelectedItem.Value + ")").CopyToDataTable();
                Dt_Datos_Cuentas.TableName = "Dt_Cuentas_Con_Rezago";

                Reporte_Cuentas_Con_Rezago.Clear();
                Reporte_Cuentas_Con_Rezago.Tables.Clear();
                Reporte_Cuentas_Con_Rezago.Tables.Add(Dt_Datos_Cuentas.Copy());
                Nombre_Repote_Crystal = "Rpt_Pre_Cuentas_Con_Rezago.rpt";
                Nombre_Reporte = "Reporte de Cuentas con Rezago";
                Generar_Reportes(Reporte_Cuentas_Con_Rezago, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Exportar_pdf_Click
    ///DESCRIPCIÓN          : Prepara la información necesaria para mandar imprimir en PDF
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 15/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Exportar_pdf_Click(object sender, ImageClickEventArgs e)
    {
        Imprimir_Reporte("PDF");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Exportar_Excel_Click
    ///DESCRIPCIÓN          : Prepara la información necesaria para mandar imprimir en EXCEL
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 15/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {
        Imprimir_Reporte("Excel");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Generar_Reportes
    ///DESCRIPCIÓN          : Prepara la información necesaria para generar el reporte
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Generar_Reportes(DataSet Ds_Datos, String Nombre_Reporte_Crystal, String Nombre_Reporte, String Formato)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";

        // Ruta donde se encuentra el reporte Crystal
        Ruta_Reporte_Crystal = "../Rpt/Predial/" + Nombre_Reporte_Crystal;

        // Se crea el nombre del reporte
        String Nombre_Report = Nombre_Reporte + "_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("HH'-'mm'-'ss"));

        // Se da el nombre del reporte que se va generar
        if (Formato == "PDF")
            Nombre_Reporte_Generar = Nombre_Report + ".pdf";  // Es el nombre del reporte PDF que se va a generar
        else if (Formato == "Excel")
            Nombre_Reporte_Generar = Nombre_Report + ".xls";  // Es el nombre del repote en Excel que se va a generar

        Cls_Reportes Reportes = new Cls_Reportes();
        Reportes.Generar_Reporte(ref Ds_Datos, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
        Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
    ///DESCRIPCIÓN          : Manda a pantalla el reporte cargado
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Enero/2012
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
    ///NOMBRE DE LA FUNCIÓN : Validar_Campos_Obligatorios
    ///DESCRIPCIÓN          : Determina que los campos obligatorios se hallan seleccionado
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Campos_Obligatorios()
    {
        Lbl_Mensaje_Error.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Validacion == false)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Mensaje_Error;
            Img_Error.Visible = true;
        }
        else
        {
            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            Img_Error.Visible = false;
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Ubicacion_Click
    ///DESCRIPCIÓN          : Evento de Botón para asignar los valores de la Calle y Colonia de Ubicación
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 09/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Ubicacion_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
        {
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
            {
                Hdn_Colonia_ID_Ubicacion.Value = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                Txt_Colonia_Ubicacion.Text = Session["NOMBRE_COLONIA"].ToString().Replace("&nbsp;", "");
                //Txt_Colonia_Ubicacion.Text = Session["CLAVE_COLONIA"].ToString().Replace("&nbsp;", "") + " " + Session["NOMBRE_COLONIA"].ToString().Replace("&nbsp;", "");
                Hdn_Calle_ID_Ubicacion.Value = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                Txt_Calle_Ubicacion.Text = Session["NOMBRE_CALLE"].ToString().Replace("&nbsp;", "");
                //Txt_Calle_Ubicacion.Text = Session["CLAVE_CALLE"].ToString().Replace("&nbsp;", "") + " " + Session["NOMBRE_CALLE"].ToString().Replace("&nbsp;", "");
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Notificacion_Click
    ///DESCRIPCIÓN          : Evento de Botón para asignar los valores de la Calle y Colonia de Notificación
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 09/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Notificacion_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
        {
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
            {
                Hdn_Colonia_ID_Notificacion.Value = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                Txt_Colonia_Notificacion.Text = Session["NOMBRE_COLONIA"].ToString().Replace("&nbsp;", "");
                //Txt_Colonia_Notificacion.Text = Session["CLAVE_COLONIA"].ToString().Replace("&nbsp;", "") + " " + Session["NOMBRE_COLONIA"].ToString().Replace("&nbsp;", "");
                Hdn_Calle_ID_Notificacion.Value = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                Txt_Calle_Notificacion.Text = Session["NOMBRE_CALLE"].ToString().Replace("&nbsp;", "");
                //Txt_Calle_Notificacion.Text = Session["CLAVE_CALLE"].ToString().Replace("&nbsp;", "") + " " + Session["NOMBRE_CALLE"].ToString().Replace("&nbsp;", "");
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Filtros_Click
    ///DESCRIPCIÓN          : Evento de Botón para Limpiar los datos cargados en los Filtros
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Limpiar_Filtros_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Campos();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Años
    ///DESCRIPCIÓN          : Metodo que llena el Combo con Años de 1992 al año corriente.
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Años()
    {
        Cls_Ope_Pre_Parametros_Negocio Consulta_Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        DataTable Dt_Años;
        int Año_Inicial = 1992;
        int Año_Final = DateTime.Now.Year;
        int Año_Corriente = 0;
        int Cont_Años = 0;
        try
        {
            Año_Corriente = Consulta_Parametros.Consultar_Anio_Corriente();
            if (Año_Corriente > 0)
            {
                Año_Final = Año_Corriente;
            }

            Dt_Años = new DataTable();
            Dt_Años.Columns.Add("AÑO", typeof(Int16));

            DataRow Dr_Años;
            for (Cont_Años = Año_Inicial; Cont_Años <= Año_Final; Cont_Años++)
            {
                Dr_Años = null;
                Dr_Años = Dt_Años.NewRow();
                Dr_Años["AÑO"] = Cont_Años;
                Dt_Años.Rows.Add(Dr_Años);
            }

            Dt_Años.DefaultView.Sort = "AÑO DESC";
            Dt_Años = Dt_Años.DefaultView.ToTable();
            Cmb_Años.DataSource = Dt_Años;
            Cmb_Años.DataTextField = "AÑO";
            Cmb_Años.DataValueField = "AÑO";
            Cmb_Años.DataBind();
            Cmb_Años.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            //throw new Exception("Resumen Predio " + ex.Message.ToString(), ex);
            //Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Bimestres
    ///DESCRIPCIÓN          : Metodo que llena el Combo con los Bimestres del Año.
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Bimestres()
    {
        DataTable Dt_Bimestres;
        int Bimestre_Inicial = 1;
        int Bimestre_Final = 6;
        int Cont_Bimestres = 0;
        try
        {

            Dt_Bimestres = new DataTable();
            Dt_Bimestres.Columns.Add("BIMESTRE", typeof(Int16));

            DataRow Dr_Bimestres;
            for (Cont_Bimestres = Bimestre_Inicial; Cont_Bimestres <= Bimestre_Final; Cont_Bimestres++)
            {
                Dr_Bimestres = null;
                Dr_Bimestres = Dt_Bimestres.NewRow();
                Dr_Bimestres["BIMESTRE"] = Cont_Bimestres;
                Dt_Bimestres.Rows.Add(Dr_Bimestres);
            }

            Cmb_Bimestres.DataSource = Dt_Bimestres;
            Cmb_Bimestres.DataTextField = "BIMESTRE";
            Cmb_Bimestres.DataValueField = "BIMESTRE";
            Cmb_Bimestres.DataBind();
            Cmb_Bimestres.SelectedValue = Obtener_Bimetres_Actual().ToString();
        }
        catch (Exception Ex)
        {
            //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            //throw new Exception("Resumen Predio " + ex.Message.ToString(), ex);
            //Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Bimetres_Actual
    ///DESCRIPCIÓN          : Calcula el Bimestre Actual en base al Mes en curso
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private int Obtener_Bimetres_Actual()
    {
        int Bimestre_Actual = 0;
        if (DateTime.Now.Month % (Decimal)2 == 1)
        {
            Bimestre_Actual = (int)((DateTime.Now.Month / (Decimal)2) + (Decimal)0.5);
        }
        else
        {
            Bimestre_Actual = (int)(DateTime.Now.Month / 2);
        }
        return Bimestre_Actual;
    }
}
