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
using Presidencia.Catalogo_Casos_Especiales.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Reportes;

public partial class paginas_Predial_Frm_Rpt_Pre_Cuentas : System.Web.UI.Page
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
            Cargar_Combo_Beneficios();
            //String Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            //Btn_Mostrar_Busqueda_Avanzada_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal);
        }
        Txt_Nombre_Institucion.Style["display"] = "none";
        Txt_Nombre_Contribuyente.Style["display"] = "none";
        Fila_Beneficios.Style["display"] = "none";
        Fila_Otros_Filtros.Style["display"] = "none";
        if (Opt_A_Nombre_X_Institucion.Checked)
        {
            Txt_Nombre_Institucion.Style["display"] = "";
        }
        else
        {
            if (Opt_A_Nombre_X_Contribuyente.Checked)
            {
                Txt_Nombre_Contribuyente.Style["display"] = "";
            }
            else
            {
                if (Opt_Con_Beneficio.Checked)
                {
                    Fila_Beneficios.Style["display"] = "";
                    Fila_Otros_Filtros.Style["display"] = "";
                }
            }
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
        //Txt_Cuenta_Predial.Text = "";
        //Hdn_Cuenta_Predial_ID.Value = "";
        Opt_Con_Exencion.Checked = false;
        Opt_A_Nombre_X_Institucion.Checked = false;
        Opt_A_Nombre_X_Contribuyente.Checked = false;
        Opt_Foraneas.Checked = false;
        Opt_Con_Beneficio.Checked = false;
        Txt_Fecha_Inicio.Text = "";
        Txt_Fecha_Termino.Text = "";
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
        Boolean Con_Adeudo_Corriente_Rezago = false;

        if (Validar_Campos_Obligatorios())
        {
            {
                Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
                DataTable Dt_Datos_Cuentas;
                Ds_Rpt_Pre_Cuentas_Predial Reporte_Cuentas_Predial = new Ds_Rpt_Pre_Cuentas_Predial();
                Cuentas.P_Campos_Dinamicos = "DISTINCT ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", ";
                Cuentas.P_Unir_Tablas = "";
                Cuentas.P_Filtros_Dinamicos = "";
                Cuentas.P_Ordenar_Dinamico = "";
                if (Opt_Con_Exencion.Checked)
                {
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                    Cuentas.P_Campos_Dinamicos += "UPPER(" + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_CONTRIBUYENTE ";
                    Cuentas.P_Unir_Tablas = Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + ", " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AND ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + " <> 0 ";
                    Cuentas.P_Ordenar_Dinamico = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                    Nombre_Repote_Crystal = "Rpt_Pre_Cuentas_Con_Exencion.rpt";
                    Nombre_Reporte = "Reporte de Cuentas con Exencion";
                }
                if (Opt_A_Nombre_X_Institucion.Checked)
                {
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_CONTRIBUYENTE ";
                    Cuentas.P_Unir_Tablas = Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + ", " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AND ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND ";
                    Cuentas.P_Filtros_Dinamicos += "UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + ") LIKE '%" + Txt_Nombre_Institucion.Text.Trim().ToUpper() + "%' ";
                    Cuentas.P_Ordenar_Dinamico = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                    Nombre_Repote_Crystal = "Rpt_Pre_Cuentas_X_Institucion.rpt";
                    Nombre_Reporte = "Reporte de Cuentas con (X) Nombre de Institución";
                }
                if (Opt_A_Nombre_X_Contribuyente.Checked)
                {
                    Cuentas.P_Campos_Dinamicos += "UPPER(" + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_CONTRIBUYENTE, ";
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Anio + ", ";
                    Cuentas.P_Campos_Dinamicos += "(NVL(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ", 0)) ADEUDO, ";
                    Cuentas.P_Campos_Dinamicos += "(NVL(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ", 0)) PAGADO, ";
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Monto_Por_Pagar + " ";
                    Cuentas.P_Unir_Tablas = Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + ", " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + ", " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AND ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " AND ";
                    Cuentas.P_Filtros_Dinamicos += "(UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + ") LIKE '%" + Txt_Nombre_Contribuyente.Text.Trim().ToUpper() + "%' OR ";
                    Cuentas.P_Filtros_Dinamicos += "UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") LIKE '%" + Txt_Nombre_Contribuyente.Text.Trim().ToUpper() + "%' OR ";
                    Cuentas.P_Filtros_Dinamicos += "UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + ") LIKE '%" + Txt_Nombre_Contribuyente.Text.Trim().ToUpper() + "%') ";
                    Cuentas.P_Ordenar_Dinamico = Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + "." + Ope_Pre_Adeudos_Predial.Campo_Anio;
                    Nombre_Repote_Crystal = "Rpt_Pre_Cuentas_X_Persona_Con_Adeudo.rpt";
                    Nombre_Reporte = "Reporte de Cuentas con (X) Nombre de Persona con Adeudo";
                }
                if (Opt_Foraneas.Checked)
                {
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
                    Cuentas.P_Campos_Dinamicos += "UPPER(" + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_CONTRIBUYENTE, ";
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Tasa_Valor + " ";
                    Cuentas.P_Unir_Tablas = Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + ", " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + ", " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + " ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AND ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND (";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO' OR ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " = 'POSEEDOR') AND ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + " = 'SI' AND ";
                    Cuentas.P_Filtros_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + " ";
                    Cuentas.P_Ordenar_Dinamico = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                    Nombre_Repote_Crystal = "Rpt_Pre_Cuentas_Foraneas.rpt";
                    Nombre_Reporte = "Reporte de Cuentas Foraneas";
                    Con_Adeudo_Corriente_Rezago = true;
                }
                if (Opt_Con_Beneficio.Checked)
                {
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                    Cuentas.P_Campos_Dinamicos += "UPPER(" + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_CONTRIBUYENTE, ";
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Cuota_Minima + ", ";
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Exedente_Construccion + ", ";
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Excedente_Construccion_Total + ", ";
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Exedente_Valor + ", ";
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Excedente_Valor_Total + ", ";
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Plazo_Financiamiento + ", ";
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Tasa_Valor + ", ";
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Descripcion + " ";
                    Cuentas.P_Unir_Tablas = Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + ", " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + ", " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + ", " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + " ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AND ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND (";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO' OR ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " = 'POSEEDOR') AND ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + " = " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " AND ";
                    Cuentas.P_Filtros_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Caso_Especial_Id + " = " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " AND ";

                    if (Cmb_Beneficios.SelectedValue != "TODOS")
                    {
                        Cuentas.P_Filtros_Dinamicos += Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = '" + Cmb_Beneficios.SelectedValue + "' AND ";
                    }
                    Nombre_Repote_Crystal = "Rpt_Pre_Cuentas_Con_Beneficio.rpt";
                    Nombre_Reporte = "Reporte de Beneficios";

                    switch (Cmb_Otros_Filtros.SelectedValue)
                    {
                        case "EXCEDENTE_CONSTRUCCION":
                            Cuentas.P_Filtros_Dinamicos += "NVL(" + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Excedente_Construccion_Total + ", 0) <> 0 ";
                            Nombre_Repote_Crystal = "Rpt_Pre_Cuentas_Con_Beneficio_Y_Excedente_Construccion.rpt";
                            Nombre_Reporte = "Reporte de Cuentas con Beneficio y Excedente de Construccion";
                            break;
                        case "EXCEDENTE_VALOR":
                            Cuentas.P_Filtros_Dinamicos += "NVL(" + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Excedente_Valor_Total + ", 0) <> 0 ";
                            Nombre_Repote_Crystal = "Rpt_Pre_Cuentas_Con_Beneficio_Y_Excedente_Valor.rpt";
                            Nombre_Reporte = "Reporte de Cuentas con Beneficio y Excedente de Valor";
                            break;
                        case "ASOCIACIONES_CIVILES":
                            Cuentas.P_Filtros_Dinamicos += Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Descripcion + " LIKE '%CIVIL%' ";
                            Nombre_Repote_Crystal = "Rpt_Pre_Cuentas_Con_Beneficio_Asociacion_Civil.rpt";
                            Nombre_Reporte = "Reporte de Cuentas con Beneficio de Asociacion Civil";
                            break;
                    }
                    if (Cuentas.P_Filtros_Dinamicos.EndsWith(" AND "))
                    {
                        Cuentas.P_Filtros_Dinamicos = Cuentas.P_Filtros_Dinamicos.Substring(0, Cuentas.P_Filtros_Dinamicos.Length - 5);
                    }
                    //Cuentas.P_Ordenar_Dinamico = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                }
                if (Opt_Beneficios_Por_Año.Checked)
                {
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", ";
                    Cuentas.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + ", ";
                    Cuentas.P_Campos_Dinamicos += Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Descripcion + " ";
                    Cuentas.P_Unir_Tablas = Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ", " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + ", " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + " ";
                    Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AND ";
                    Cuentas.P_Filtros_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Cuota_Fija + " = " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " AND ";
                    Cuentas.P_Filtros_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Caso_Especial_Id + " = " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " ";
                    Cuentas.P_Ordenar_Dinamico = Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " DESC, " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                    Nombre_Repote_Crystal = "Rpt_Pre_Beneficios_Por_Año.rpt";
                    Nombre_Reporte = "Reporte de Beneficios por Año";
                }
                Dt_Datos_Cuentas = Cuentas.Consultar_Datos_Reporte();
                if (Con_Adeudo_Corriente_Rezago)
                {
                    Cls_Ope_Pre_Parametros_Negocio Consulta_Parametros = new Cls_Ope_Pre_Parametros_Negocio();
                    Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Generar_Adeudo_Predial = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
                    Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
                    //AGEGAR LOS CAMPOS REQUERIDOS DE PERIODO E IMPORTE REZAGO, PERIODO E IMPORTE CORRIENTE, RECARGOS, HONORARIOS Y CONVENIDA
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
                        //if (!Dt_Datos_Cuentas.Columns.Contains("CONVENIDA"))
                        //{
                        //    Dt_Datos_Cuentas.Columns.Add("CONVENIDA", typeof(String));
                        //}

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

                                //Resumen_Predio.P_Validar_Convenios_Cumplidos = true;
                                //Resumen_Predio.P_Cuenta_Predial = Dr_Datos_Cuentas[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
                                //DataTable Dt_Resumen_Predio_Convenios = Resumen_Predio.Consultar_Convenios();
                                //String Cuenta_Convenida = "";

                                //if (Dt_Resumen_Predio_Convenios != null)
                                //{
                                //    if (Dt_Resumen_Predio_Convenios.Rows.Count > 0)
                                //    {
                                //        if (Dt_Resumen_Predio_Convenios.Rows[0]["Estatus"] != null)
                                //        {
                                //            if (Dt_Resumen_Predio_Convenios.Rows[0]["Estatus"].ToString() == "ACTIVO")
                                //            {
                                //                foreach (DataRow Dr_Resumen_Predio_Convenios in Dt_Resumen_Predio_Convenios.Rows)
                                //                {
                                //                    //////if (Dr_Resumen_Predio_Convenios["No_Convenio"].ToString() != "")
                                //                    //////{
                                //                    //////    Cuenta_Convenida = Dr_Resumen_Predio_Convenios["No_Convenio"].ToString();
                                //                    //////}
                                //                    //if (Dr_Resumen_Predio_Convenios["No_Convenio"].ToString().StartsWith("CDER"))
                                //                    //{
                                //                    //    Cuenta_Convenida = Dr_Resumen_Predio_Convenios["No_Convenio"].ToString();
                                //                    //    //Hdn_Tipo_Convenio.Value = "CONVENIDA DS";
                                //                    //}
                                //                    //if (Dr_Resumen_Predio_Convenios["No_Convenio"].ToString().StartsWith("CFRA"))
                                //                    //{
                                //                    //    Cuenta_Convenida = Dr_Resumen_Predio_Convenios["No_Convenio"].ToString();
                                //                    //    //Hdn_Tipo_Convenio.Value = "CONVENIDA FRACCIONAMIENTOS";
                                //                    //}
                                //                    //if (Dr_Resumen_Predio_Convenios["No_Convenio"].ToString().StartsWith("CTRA"))
                                //                    //{
                                //                    //    Cuenta_Convenida = Dr_Resumen_Predio_Convenios["No_Convenio"].ToString();
                                //                    //    //Hdn_Tipo_Convenio.Value = "CONVENIDA TD";
                                //                    //}
                                //                    if (Dr_Resumen_Predio_Convenios["No_Convenio"].ToString().StartsWith("CPRE"))
                                //                    {
                                //                        Cuenta_Convenida = Dr_Resumen_Predio_Convenios["No_Convenio"].ToString();
                                //                        //Hdn_Tipo_Convenio.Value = "CONVENIDA PREDIAL";
                                //                    }
                                //                }
                                //            }
                                //            //else
                                //            //{
                                //            //    if (Dt_Resumen_Predio_Convenios.Rows[0]["Estatus"].ToString() == "INCUMPLIDO")
                                //            //    {
                                //            //        Cuenta_Convenida = "CONVENIO INCUMPLIDO";
                                //            //    }
                                //            //}
                                //        }
                                //    }
                                //}

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
                                //Dr_Datos_Cuentas["CONVENIDA"] = Cuenta_Convenida;
                            }
                        }
                    }
                }
                Dt_Datos_Cuentas.TableName = "Dt_Cuentas_Predial";
                Reporte_Cuentas_Predial.Clear();
                Reporte_Cuentas_Predial.Tables.Clear();
                Reporte_Cuentas_Predial.Tables.Add(Dt_Datos_Cuentas.Copy());
                Generar_Reportes(Reporte_Cuentas_Predial, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
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
        if (!Opt_Con_Exencion.Checked
            && !Opt_A_Nombre_X_Institucion.Checked
            && !Opt_A_Nombre_X_Contribuyente.Checked
            && !Opt_Foraneas.Checked
            && !Opt_Con_Beneficio.Checked
            && !Opt_Beneficios_Por_Año.Checked)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccione una Opción de Reporte para poder imprimir.";
            Validacion = false;
        }
        if (Opt_Con_Exencion.Checked)
        {
        }
        if (Opt_A_Nombre_X_Institucion.Checked)
        {
            if (Txt_Nombre_Institucion.Text.Trim() == "")
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Proporcione un Nombre de Institución por favor para poder imprimir.";
                Validacion = false;
            }
        }
        if (Opt_A_Nombre_X_Contribuyente.Checked)
        {
            if (Txt_Nombre_Contribuyente.Text.Trim() == "")
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Proporcione un Nombre de Contribuyente por favor para poder imprimir.";
                Validacion = false;
            }
        }
        if (Opt_Foraneas.Checked)
        {
        }
        if (Opt_Con_Beneficio.Checked)
        {
        }
        if (Opt_Beneficios_Por_Año.Checked)
        {
        }
        //if (Txt_Fecha_Inicio.Text.Trim() == "" && Txt_Fecha_Termino.Text.Trim() == "")
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Seleccione un Rango de Fechas para poder imprimir.";
        //    Validacion = false;
        //}
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

    private void Cargar_Combo_Beneficios()
    {
        Cls_Cat_Pre_Casos_Especiales_Negocio Casos_Especiales = new Cls_Cat_Pre_Casos_Especiales_Negocio();
        DataTable Dt_Casos_Especiales;
        DataRow Dr_Casos_Especiales;

        Cmb_Beneficios.Items.Clear();
        Dt_Casos_Especiales = Casos_Especiales.Consultar_Nombre_Beneficios();
        Dr_Casos_Especiales = Dt_Casos_Especiales.NewRow();
        Dr_Casos_Especiales[Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID] = "TODOS";
        Dr_Casos_Especiales[Cat_Pre_Casos_Especiales.Campo_Descripcion] = "<TODOS>";
        Dt_Casos_Especiales.Rows.InsertAt(Dr_Casos_Especiales, 0);
        Cmb_Beneficios.DataValueField = Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID;
        Cmb_Beneficios.DataTextField = Cat_Pre_Casos_Especiales.Campo_Descripcion;
        Cmb_Beneficios.DataSource = Dt_Casos_Especiales;
        Cmb_Beneficios.DataBind();
    }
}
