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
using System.IO;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using openXML_Wp = DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Rpt_Constancias.Negocio;
using System.IO;

public partial class paginas_Nomina_Frm_Rpt_Nom_Plantillas_Constancias : System.Web.UI.Page
{
    #region Page Load
        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Page_Load
        ///DESCRIPCIÓN          : Inicio de la pagina
        ///PARAMETROS           : 
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 11/Abril/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
                if (!IsPostBack)
                {
                    Configuracion_Inicial();
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en el inicio del la pagina. Error [" + Ex.Message + "]");
            }
        }
    #endregion

    #region Metodos
        #region Metodos Generales
            ///*****************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Configuracion_Inicial
            ///DESCRIPCIÓN          : Inicio de la pagina
            ///PARAMETROS           : 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Abril/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*****************************************************************************************************************
            private void Configuracion_Inicial()
            {
                try
                {
                    Lbl_Mensaje_Error.Text = String.Empty;
                    Div_Contenedor_Msj_Error.Visible = false;
                    Limpiar_Controles();
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error en el inicio de la pagina. Error [" + Ex.Message + "]");
                }
            }

            ///*****************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Limpiar_Controles
            ///DESCRIPCIÓN          : Metodo para limpiar los controles del formulario
            ///PARAMETROS           : 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Abril/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*****************************************************************************************************************
            private void Limpiar_Controles()
            {
                try
                {
                    Lbl_Mensaje_Error.Text = String.Empty;
                    Div_Contenedor_Msj_Error.Visible = false;
                    Txt_No_Empleado.Text = String.Empty;
                    Txt_Nombre_Empleado.Text = String.Empty;
                    Txt_Titulo.Text = String.Empty;
                    Txt_Puesto.Text = String.Empty;
                    Txt_Presente.Text = String.Empty;
                    Txt_Datos_UR.Text = String.Empty;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al limpiar los controles Error [" + Ex.Message + "]");
                }
            }

            ///*****************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Validar_Datos
            ///DESCRIPCIÓN          : Metodo para validar los datos de la constancia
            ///PARAMETROS           : 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Abril/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*****************************************************************************************************************
            private Boolean Validar_Datos()
            {
                Lbl_Mensaje_Error.Text = String.Empty;
                Div_Contenedor_Msj_Error.Visible = false;
                Boolean Datos_Validos = true;//Variable que alamacenara el resultado de la validacion de los datos ingresados por el usuario.
                try
                {
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

                    if (String.IsNullOrEmpty(Txt_Director.Text.Trim()))
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El nombre del Director de Recursos Humanos. <br>";
                        Datos_Validos = false;
                    }
                    if (Rbl_Reporte.SelectedIndex < 0)
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccionar un tipo de contancia <br>";
                        Datos_Validos = false;
                    }
                    if (String.IsNullOrEmpty(Txt_No_Empleado.Text.Trim()) && String.IsNullOrEmpty(Txt_Nombre_Empleado.Text.Trim()))
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El nombre o número del empleado. <br>";
                        Datos_Validos = false;
                    }
                    if (String.IsNullOrEmpty(Txt_Presente.Text.Trim()))
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La persona a la que va dirigida la constancia. <br>";
                        Datos_Validos = false;
                    }
                    if (Rbl_Reporte.SelectedIndex == 0)
                    {
                        if (String.IsNullOrEmpty(Txt_Datos_UR.Text.Trim()))
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La ubicación o teléfono de la Unidad Responsable. <br>";
                            Datos_Validos = false;
                        }
                        if (String.IsNullOrEmpty(Txt_Titulo.Text.Trim()))
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Asunto de la constancia. <br>";
                            Datos_Validos = false;
                        }
                    }
                    if (Rbl_Reporte.SelectedIndex == 0)
                    {
                        if (String.IsNullOrEmpty(Txt_Datos_UR.Text.Trim()))
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La ubicación de la Unidad Responsable. <br>";
                            Datos_Validos = false;
                        }

                    }

                    return Datos_Validos;
    
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al limpiar los controles Error [" + Ex.Message + "]");
                }
            }
            
        #endregion

        #region Metodos Plantillas
            ///*****************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Construir_Constancias
            ///DESCRIPCIÓN          : Metodo para elaborar las plantillas de las constantes
            ///PARAMETROS           1: Tipo_Constancia: constancia que se elaborara 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Abril/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*****************************************************************************************************************
            private void Construir_Constancias(String Tipo_Constancia)
            {
                Lbl_Mensaje_Error.Text = String.Empty;
                Div_Contenedor_Msj_Error.Visible = false;
                String Ruta_Plantilla = String.Empty;
                String Documento_Salida = String.Empty;
                ReportDocument Reporte = new ReportDocument();
                StringBuilder newXml = new StringBuilder();
                MainDocumentPart main;
                CustomXmlPart CustomXml;
                String Nombre_Archivo = String.Empty;

                try
                {
                    Ruta_Plantilla = Server.MapPath("Plantillas/" + Tipo_Constancia + ".docx");
                    Nombre_Archivo = "Constancia_" + Session.SessionID + ".docx";
                    Documento_Salida = Server.MapPath("../../Reporte/" + Nombre_Archivo);

                    //eliminamos el documento si es que existe
                    if (System.IO.Directory.Exists(Server.MapPath("../../Reporte")))
                    {
                        if (System.IO.File.Exists(Documento_Salida))
                        {
                            System.IO.File.Delete(Documento_Salida);
                        }
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory("../../Reporte");
                    }
                    //copiamos la plantilla
                    File.Copy(Ruta_Plantilla, Documento_Salida);

                    using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
                    {
                        newXml.Append("<root>");
                        newXml.Append(Obtener_Informacion());
                        newXml.Append(Obtener_Informacion_Constancia());
                        newXml.Append("<Fecha>" + Obtener_Fecha(String.Format("{0:MM/dd/yyyy}", DateTime.Now), "Mayusculas") + "</Fecha>");
                        newXml.Append("</root>");

                        main = doc.MainDocumentPart;
                        main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);
                        CustomXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                        using (StreamWriter ts = new StreamWriter(CustomXml.GetStream()))
                        {
                            ts.Write(newXml);
                        }
                        // guardar los cambios en el documento
                        main.Document.Save();
                        doc.Close();
                    }


                    String Pagina = "Frm_Mostrar_Archivos.aspx?Documento=";
                    Pagina = Pagina + "../../Reporte/" + Nombre_Archivo;
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(), "Constancia", "window.open('" + Pagina +
                        "', '" + "msword" + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1,height=1');",
                        true
                        );
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al elaborar la constancia Error [" + Ex.Message + "]");
                }
            }

            ///*****************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Informacion_Constancia
            ///DESCRIPCIÓN          : Metodo para elaborar las plantillas de las constantes
            ///PARAMETROS           : 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Abril/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*****************************************************************************************************************
            private String Obtener_Informacion_Constancia()
            {
                Cls_Rpt_Nom_Constancias_Negocio Constancias_Negocio = new Cls_Rpt_Nom_Constancias_Negocio();
                StringBuilder Cadena = new StringBuilder();
                DataTable Dt_Datos = new DataTable();
                DataTable Dt_Datos_Baja = new DataTable();

                try 
	            {
            		if(!String.IsNullOrEmpty(Txt_No_Empleado.Text.Trim()))
                    {
                        Constancias_Negocio.P_No_Empleado = String.Format("{0:000000}", Convert.ToInt32(Txt_No_Empleado.Text.Trim()));
                    }
                    Constancias_Negocio.P_Nombre_Empleado = Txt_Nombre_Empleado.Text.Trim();
                    
                    Dt_Datos = Constancias_Negocio.Consultar_Datos_Empleado();

                    if (Dt_Datos != null)
                    {
                        if(Dt_Datos.Rows.Count > 0)
                        {
                            Cadena.Append("<Empleado>"+Dt_Datos.Rows[0]["Empleado"].ToString().Trim()+"</Empleado>");
                            Cadena.Append("<RFC>" + Dt_Datos.Rows[0]["RFC"].ToString().Trim() + "</RFC>");
                            Cadena.Append("<Puesto>" + Dt_Datos.Rows[0]["Puesto"].ToString().Trim() + "</Puesto>");
                            Cadena.Append("<IMSS>" + Dt_Datos.Rows[0]["IMSS"].ToString().Trim() + "</IMSS>");
                            Cadena.Append("<UR>" + Dt_Datos.Rows[0]["UR"].ToString().Trim() + "</UR>");
                            Cadena.Append("<IMSS>" + Dt_Datos.Rows[0]["IMSS"].ToString().Trim() + "</IMSS>");
                            Cadena.Append("<Horario>" + Dt_Datos.Rows[0]["Horario"].ToString().Trim() + "</Horario>");
                            Cadena.Append("<Salario>" + String.Format("{0:c}", Dt_Datos.Rows[0]["Salario"]) + "</Salario>");
                            Cadena.Append("<Fecha_Inicio>");
                            Cadena.Append(Obtener_Fecha(String.Format("{0:MM/dd/yyyy}", Dt_Datos.Rows[0]["Fecha_Inicio"]), "Minusculas"));
                            Cadena.Append("</Fecha_Inicio>");

                            if (Rbl_Reporte.Text.Trim().Equals("3"))
                            {
                                Constancias_Negocio.P_No_Empleado = Dt_Datos.Rows[0]["EMPLEADO_ID"].ToString();
                                Dt_Datos_Baja = Constancias_Negocio.Consultar_Datos_Empleado_Baja();
                                if (Dt_Datos_Baja != null)
                                {
                                    if (Dt_Datos_Baja.Rows.Count > 0)
                                    {
                                        Cadena.Append("<Causa>" + Dt_Datos_Baja.Rows[0]["Causa"].ToString().Trim() + "</Causa>");
                                    }
                                    else
                                    {
                                        Cadena.Append("<Causa>(Sin Motivo)</Causa>");
                                    }
                                }
                                else 
                                {
                                    Cadena.Append("<Causa>(Sin Motivo)</Causa>");
                                }
                                Cadena.Append("<Fecha_Fin>");
                                Cadena.Append(Obtener_Fecha(String.Format("{0:MM/dd/yyyy}", Dt_Datos.Rows[0]["Fecha_Fin"]), "Minusculas"));
                                Cadena.Append("</Fecha_Fin>");
                            }
                            else 
                            {
                                Cadena.Append("<Causa>(Sin Motivo)</Causa>");
                            }
                        }
                        else 
                        {
                            Lbl_Mensaje_Error.Text = "Favor de introducir el nombre del empleado o numero de empleado correctos";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                    else 
                    {
                        Lbl_Mensaje_Error.Text = "Favor de introducir el nombre del empleado o numero de empleado correctos";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
	            }
	            catch (Exception Ex)
	            {
		            throw new Exception("Error al consultar los datos de la constancia. Error[" + Ex.Message + "]");
	            }
                return  Cadena.ToString(); 
            }

            ///*****************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Informacion
            ///DESCRIPCIÓN          : Metodo para elaborar las plantillas de las constantes
            ///PARAMETROS           :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Abril/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*****************************************************************************************************************
            private String Obtener_Informacion()
            {
                StringBuilder Cadena = new StringBuilder();
                try
                {
                    if (!String.IsNullOrEmpty(Txt_Titulo.Text.Trim()))
                    {
                        Cadena.Append("<Titulo>" + Txt_Titulo.Text.Trim().ToUpper() + "</Titulo>");
                    }
                    else 
                    {
                        Cadena.Append("<Titulo> </Titulo>");
                    }

                    if (!String.IsNullOrEmpty(Txt_Presente.Text.Trim()))
                    {
                        Cadena.Append("<Presente>" + Txt_Presente.Text.Trim().ToUpper() + "</Presente>");
                    }
                    else
                    {
                        Cadena.Append("<Presente> </Presente>");
                    }

                    if (!String.IsNullOrEmpty(Txt_Datos_UR.Text.Trim()))
                    {
                        Cadena.Append("<Datos_UR>" + Txt_Datos_UR.Text.Trim().ToUpper() + "</Datos_UR>");
                    }
                    else
                    {
                        Cadena.Append("<Datos_UR> </Datos_UR>");
                    }

                    if (!String.IsNullOrEmpty(Txt_Director.Text.Trim()))
                    {
                        Cadena.Append("<Director>" + Txt_Director.Text.Trim().ToUpper() + "</Director>");
                    }
                    else
                    {
                        Cadena.Append("<Director> </Director>");
                    }

                    if (!String.IsNullOrEmpty(Txt_Puesto.Text.Trim()))
                    {
                        Cadena.Append("<Puesto_Empleado>" + Txt_Puesto.Text.Trim().ToUpper() + "</Puesto_Empleado>");
                    }
                    else
                    {
                        Cadena.Append("<Puesto_Empleado></Puesto_Empleado>");
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar los datos de la constancia. Error[" + Ex.Message + "]");
                }
                return Cadena.ToString();
            }

    ///*****************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Fecha
            ///DESCRIPCIÓN          : Metodo para obtener la fecha con formato
            ///PARAMETROS           :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Abril/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*****************************************************************************************************************
            private String Obtener_Fecha(String Fecha, String Tipo) 
            {
                String Fecha_Formateada = String.Empty;
                String Mes = String.Empty;
                String[] Fechas;

                try
                {
                    Fechas = Fecha.Split('/');
                    Mes = Fechas[0].ToString();
                    switch (Mes)
                    {
                        case "01":
                            if (Tipo.Trim().Equals("Mayusculas")){
                                Mes = "ENERO";
                            }
                            else {
                                Mes = "Enero";
                            }
                            break;
                        case "02":
                            if (Tipo.Trim().Equals("Mayusculas")){
                                Mes = "FEBRERO";
                            }
                            else{
                                Mes = "Febrero";
                            }
                            break;
                        case "03":
                            if (Tipo.Trim().Equals("Mayusculas")){
                                Mes = "MARZO";
                            }
                            else{
                                Mes = "Marzo";
                            }
                            break;
                        case "04":
                            if (Tipo.Trim().Equals("Mayusculas")){
                                Mes = "ABRIL";
                            }
                            else{
                                Mes = "Abril";
                            }
                            break;
                        case "05":
                            if (Tipo.Trim().Equals("Mayusculas")){
                                Mes = "MAYO";
                            }
                            else{
                                Mes = "Mayo";
                            }
                            break;
                        case "06":
                            if (Tipo.Trim().Equals("Mayusculas")){
                                Mes = "JUNIO";
                            }
                            else{
                                Mes = "Junio";
                            }
                            break;
                        case "07":
                            if (Tipo.Trim().Equals("Mayusculas")){
                                Mes = "JULIO";
                            }
                            else{
                                Mes = "Julio";
                            }
                            break;
                        case "08":
                            if (Tipo.Trim().Equals("Mayusculas")){
                                Mes = "AGOSTO";
                            }
                            else{
                                Mes = "Agosto";
                            }
                            break;
                        case "09":
                            if (Tipo.Trim().Equals("Mayusculas")){
                                Mes = "SEPTIEMBRE";
                            }
                            else{
                                Mes = "Septiembre";
                            }
                            break;
                        case "10":
                            if (Tipo.Trim().Equals("Mayusculas")){
                                Mes = "OCTUBRE";
                            }
                            else{
                                Mes = "Octubre";
                            }
                            break;
                        case "11":
                            if (Tipo.Trim().Equals("Mayusculas")){
                                Mes = "NOVIEMBRE";
                            }
                            else{
                                Mes = "Noviembre";
                            }
                            break;
                        default:
                            if (Tipo.Trim().Equals("Mayusculas")) {
                                Mes = "DICIEMBRE";
                            }
                            else {
                                Mes = "Diciembre";
                            }
                            break;
                    }

                    if (Tipo.Trim().Equals("Mayusculas"))
                    {
                        Fecha_Formateada = Fechas[1].ToString() + " DE " + Mes + " DE " + Fechas[2].ToString();
                    }
                    else
                    {
                        Fecha_Formateada = Fechas[1].ToString() + " de " + Mes + " de " + Fechas[2].ToString();
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar los datos de la constancia. Error[" + Ex.Message + "]");
                }
                return Fecha_Formateada;
            }
        #endregion
    #endregion

    #region Eventos
            ///*****************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Btn_Generar_Reporte_Click
            ///DESCRIPCIÓN          : Evento del boton de generar la plantilla de la constancia
            ///PARAMETROS           :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Abril/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*****************************************************************************************************************
            protected void Btn_Generar_Reporte_Click(object sender, EventArgs e)
            {
                Lbl_Mensaje_Error.Text = String.Empty;
                Div_Contenedor_Msj_Error.Visible = false;
                String Nombre_Plantilla = String.Empty;

                try
                {
                    if (Validar_Datos())
                    {
                        if (Rbl_Reporte.Text.Trim().Equals("3"))
                        {
                            Nombre_Plantilla = "Plantilla_Constancia_Baja";
                        }
                        else if (Rbl_Reporte.Text.Trim().Equals("1"))
                        {
                            Nombre_Plantilla = "Plantilla_Constancia_Laboral_Uno";
                        }
                        else if (Rbl_Reporte.Text.Trim().Equals("2"))
                        {
                            Nombre_Plantilla = "Plantilla_Constancia_Laboral";
                        }
                        else if (Rbl_Reporte.Text.Trim().Equals("0"))
                        {
                            Nombre_Plantilla = "Plantilla_Constancia_Registro_Patronal";
                        }
                        Construir_Constancias(Nombre_Plantilla);
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Div_Contenedor_Msj_Error.Visible = true;
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
}
