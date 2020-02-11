using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Reportes;
using Presidencia.Sessiones;

public partial class paginas_Predial_Frm_Rpt_Pre_Padron : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Btn_Exportar_pdf_Click(object sender, ImageClickEventArgs e)
    {
        Imprimir_Reporte("PDF");
    }
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {
        Imprimir_Reporte("Excel");
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Imprimir
    ///DESCRIPCIÓN          : Manda a imprimir el reporte con el formato indicado
    ///PARAMETROS:     
    ///CREO                 : Jesus Toledo
    ///FECHA_CREO           : 
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
            Nombre_Repote_Crystal = "Rpt_Pre_Padron.rpt";
            Nombre_Reporte = "Reporte de Padron";
                Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
                DataTable Dt_Datos_Cuentas;
                Ds_Pre_Padron Reporte_Cuentas_Predial = new Ds_Pre_Padron();
                Cuentas.P_Campos_Dinamicos = "";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Nombre + " AS CALLE, ";
                Cuentas.P_Campos_Dinamicos += Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA, ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", ";

                Cuentas.P_Campos_Dinamicos += "UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_CONTRIBUYENTE, ";

                Cuentas.P_Campos_Dinamicos += Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Cuota_Minima + " AS CUOTA_MINIMA, ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " AS TASA, ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS TIPO_PREDIO, ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + "." + Cat_Pre_Estados_Predio.Campo_Descripcion + " AS ESTADO_PREDIO, ";
                Cuentas.P_Campos_Dinamicos += Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Descripcion + " AS TIPO_BENEFICIO, ";
                Cuentas.P_Campos_Dinamicos += "(SELECT NVL(TO_CHAR(MAX(" + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " ), 'DD/Mon/YYYY HH:MI:SS PM'),'-') FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AND " + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + " IN(SELECT " + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Descripcion + " LIKE '%DE VALOR%')) AS FECHA_ACT_VALOR" ;

                Cuentas.P_Join = "";
                Cuentas.P_Join = " LEFT JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " ON " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Calle_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                Cuentas.P_Join += " LEFT JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " ON " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Colonia_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID;
                Cuentas.P_Join += " LEFT JOIN " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + " ON " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija;
                Cuentas.P_Join += " LEFT JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Cuentas.P_Join += " LEFT JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " ON " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                Cuentas.P_Join += " LEFT JOIN " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " ON " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID;
                Cuentas.P_Join += " LEFT JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " ON " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID;
                Cuentas.P_Join += " LEFT JOIN " + Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + " ON " + Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + "." + Cat_Pre_Estados_Predio.Campo_Estado_Predio_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID;
                Cuentas.P_Join += " LEFT JOIN " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + " ON " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + "." + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + "." + Ope_Pre_Cuotas_Fijas.Campo_Caso_Especial_Id;                

                Cuentas.P_Filtros_Dinamicos = "";
            if(Opt_Cuenta.Checked && !String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
                Cuentas.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Txt_Cuenta_Predial.Text.Trim().ToUpper() + "%'";
            else if (Opt_Nombre_Contribuyente.Checked && !String.IsNullOrEmpty(Txt_Nombre_Contribuyente.Text.Trim()))
                Cuentas.P_Filtros_Dinamicos += "UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") || ' ' || UPPER(" + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + ") LIKE '%" + Txt_Nombre_Contribuyente.Text.Trim().ToUpper() + "%'";
            Cuentas.P_Filtros_Dinamicos += " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " NOT IN( 'CANCELADA' ) ";
                    if (Cuentas.P_Filtros_Dinamicos.EndsWith(" AND "))
                    {
                        Cuentas.P_Filtros_Dinamicos = Cuentas.P_Filtros_Dinamicos.Substring(0, Cuentas.P_Filtros_Dinamicos.Length - 5);
                    }
                    Cuentas.P_Ordenar_Dinamico = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;

                    if (!string.IsNullOrEmpty(Cuentas.P_Filtros_Dinamicos))
                    {
                        Dt_Datos_Cuentas = Cuentas.Consultar_Datos_Reporte();
                        Dt_Datos_Cuentas.TableName = "Dt_Cuentas_Predial";
                        Reporte_Cuentas_Predial.Clear();
                        Reporte_Cuentas_Predial.Tables.Clear();
                        Reporte_Cuentas_Predial.Tables.Add(Dt_Datos_Cuentas.Copy());
                        Generar_Reportes(Reporte_Cuentas_Predial, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
                    }
        }
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
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (!Opt_Cuenta.Checked            
            && !Opt_Nombre_Contribuyente.Checked)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccione una Opción de Reporte para poder imprimir.";
            Lbl_Mensaje_Error.Text = "Es necesario.";
            Validacion = false;
        }
        return Validacion;
    }
}