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
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Claves_Grupos_Movimiento.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Reportes;

public partial class paginas_Predial_Frm_Rpt_Pre_Ordenes_Variacion_Aplicadas : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load.
    ///DESCRIPCIÓN          : Metodo que se ejecuta en PostBack de la Página.
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides guardado
    ///FECHA_CREO           : 15/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Llenar_Combo_Grupos_Movimientos();
            Llenar_Combo_Tipos_Predio();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Evento de botón para salir de la página cargada o cancelar los datos mostrados por alguna acción
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 15/Enero/2012
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Campos
    ///DESCRIPCIÓN          : Quita los textos u opciones seleccionadas de los campos de la página
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 15/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Limpiar_Campos()
    {
        //Txt_Cuenta_Predial.Text = "";
        //Hdn_Cuenta_Predial_ID.Value = "";
        Txt_Fecha_Inicio.Text = "";
        Txt_Fecha_Termino.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Imprimir
    ///DESCRIPCIÓN          : Manda a imprimir el reporte con el formato indicado
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 15/Enero/2012
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
                Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                DataTable Dt_Datos_Ordenes;
                Ds_Rpt_Pre_Ordenes_Variacion Reporte_Ordenes_Variacion = new Ds_Rpt_Pre_Ordenes_Variacion();
                Ordenes.P_Campos_Dinamicos = "DISTINCT ";
                Ordenes.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial + ", ";
                Ordenes.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", ";
                Ordenes.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + ", ";
                Ordenes.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ", ";
                Ordenes.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Clave + /*" || ' - ' || " + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Nombre +*/ " FROM " + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + " WHERE " + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + ") AS CLAVE_NOMBRE_GRUPO_MOVIMIENTO, ";
                Ordenes.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " WHERE " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + ") AS DESCRIPCION_TIPO_PREDIO, ";
                Ordenes.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + ", ";
                Ordenes.P_Campos_Dinamicos += "CASE WHEN " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " IS NULL THEN 'PREDIAL' ELSE 'TRASLADO' END AS AREA, ";
                Ordenes.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS IDENTIFICADOR_MOVIMIENTO, ";
                Ordenes.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ", ";
                Ordenes.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Usuario_Creo + ", ";
                Ordenes.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Valido + ", ";
                Ordenes.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Usuario_Valido + " ";
                //Ordenes.P_Unir_Tablas = "";
                Ordenes.P_Filtros_Dinamicos = "";
                Ordenes.P_Filtros_Dinamicos += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'ACEPTADA' ";
                if (Txt_Fecha_Inicio.Text.Trim() != "" && Txt_Fecha_Termino.Text.Trim() != "")
                {
                    Ordenes.P_Filtros_Dinamicos += " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Valido + " >= '" + DateTime.ParseExact(Txt_Fecha_Inicio.Text, "dd/MMM/yyyy", null).ToString("dd-MM-yyyy") + "'";
                    Ordenes.P_Filtros_Dinamicos += " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Valido + " < '" + DateTime.ParseExact(Txt_Fecha_Termino.Text, "dd/MMM/yyyy", null).AddDays(1).ToString("dd-MM-yyyy") + "'";
                }
                if (Cmb_Grupos.SelectedIndex > 0)
                {
                    Ordenes.P_Filtros_Dinamicos += " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + " = '" + Cmb_Grupos.SelectedItem.Value + "'";
                }
                if (Cmb_Tipos_Predio.SelectedIndex > 0)
                {
                    Ordenes.P_Filtros_Dinamicos += " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + " = '" + Cmb_Tipos_Predio.SelectedItem.Value + "'";
                }
                Ordenes.P_Ordenar_Dinamico = "";
                Ordenes.P_Ordenar_Dinamico += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Valido + " DESC, ";
                Ordenes.P_Ordenar_Dinamico += "CLAVE_NOMBRE_GRUPO_MOVIMIENTO, ";
                Ordenes.P_Ordenar_Dinamico += "DESCRIPCION_TIPO_PREDIO, ";
                Ordenes.P_Ordenar_Dinamico += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + " DESC, ";
                Ordenes.P_Ordenar_Dinamico += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC ";

                Nombre_Repote_Crystal = "Rpt_Pre_Ordenes_Aplicadas.rpt";
                Nombre_Reporte = "Reporte de Órdenes Aplicadas";
                Dt_Datos_Ordenes = Ordenes.Consultar_Datos_Reporte_Movimientos();
                Dt_Datos_Ordenes.TableName = "Dt_Ordenes_Aplicadas";
                Reporte_Ordenes_Variacion.Clear();
                Reporte_Ordenes_Variacion.Tables.Clear();
                Reporte_Ordenes_Variacion.Tables.Add(Dt_Datos_Ordenes.Copy());
                Generar_Reportes(Reporte_Ordenes_Variacion, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
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
    ///FECHA_CREO           : 15/Enero/2012
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
    ///FECHA_CREO           : 15/Enero/2012
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
    ///FECHA_CREO           : 15/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Campos_Obligatorios()
    {
        Lbl_Mensaje_Error.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN     : Llenar_Combo_Grupos_Movimientos
    ///DESCRIPCIÓN              : Consulta los Grupos de Movimientos en el Catálogo y los carga en el Combo.
    ///PROPIEDADES:     
    ///CREO                     : Antonio Salvador Benavides Guardado.
    ///FECHA_CREO               : 07/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Grupos_Movimientos()
    {
        try
        {
            Img_Error.Visible = false;
            Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Claves_Grupos_Movimiento = new Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio();
            Claves_Grupos_Movimiento.P_Campos_Dinamicos = Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + ", (" + Cat_Pre_Grupos_Movimiento.Campo_Clave + " || ' - ' || " + Cat_Pre_Grupos_Movimiento.Campo_Nombre + ") AS CLAVE_NOMBRE_GRUPO";
            Claves_Grupos_Movimiento.P_Ordenar_Dinamico = "CLAVE_NOMBRE_GRUPO";
            DataTable Dt_Grupos_Movimientos = Claves_Grupos_Movimiento.Consultar_Grupos_Movimientos();
            DataRow Dr_Grupos_Movimientos = Dt_Grupos_Movimientos.NewRow();

            Dr_Grupos_Movimientos["CLAVE_NOMBRE_GRUPO"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Dr_Grupos_Movimientos[Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID] = "SELECCIONE";
            Dt_Grupos_Movimientos.Rows.InsertAt(Dr_Grupos_Movimientos, 0);

            Cmb_Grupos.DataTextField = "CLAVE_NOMBRE_GRUPO";
            Cmb_Grupos.DataValueField = Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID;
            Cmb_Grupos.DataSource = Dt_Grupos_Movimientos;
            Cmb_Grupos.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Encabezado_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN     : Llenar_Combo_Tipos_Predio
    ///DESCRIPCIÓN              : Consulta los Tipos de Predio en el Catálogo y los carga en el Combo.
    ///PROPIEDADES:     
    ///CREO                     : Antonio Salvador Benavides Guardado.
    ///FECHA_CREO               : 07/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Tipos_Predio()
    {
        try
        {
            Img_Error.Visible = false;
            Cls_Cat_Pre_Tipos_Predio_Negocio Tipos_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();
            Tipos_Predio.P_Ordenar_Dinamico = Cat_Pre_Tipos_Predio.Campo_Descripcion;
            //if (Cmb_Grupos.SelectedIndex > 1)
            //{
            //    Tipos_Predio.P_Filtros_Dinamicos = 0;
            //}
            DataTable Dt_Tipos_Predios = Tipos_Predio.Consultar_Tipo_Predio();
            DataRow Dr_Fila_Cabecera = Dt_Tipos_Predios.NewRow();

            Dr_Fila_Cabecera[Cat_Pre_Tipos_Predio.Campo_Descripcion] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Dr_Fila_Cabecera[Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID] = "SELECCIONE";
            Dt_Tipos_Predios.Rows.InsertAt(Dr_Fila_Cabecera, 0);

            Cmb_Tipos_Predio.DataTextField = Cat_Pre_Tipos_Predio.Campo_Descripcion;
            Cmb_Tipos_Predio.DataValueField = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID;
            Cmb_Tipos_Predio.DataSource = Dt_Tipos_Predios;
            Cmb_Tipos_Predio.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Encabezado_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Img_Error.Visible = true;
        }
    }
}
