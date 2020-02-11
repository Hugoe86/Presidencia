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
using Presidencia.Reportes_Nomina.Formato_Pago_Proveedores.Negocio;
using Presidencia.Sessiones;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using Presidencia.Constantes;
using Presidencia.Empleados.Negocios;
using System.IO;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using openXML_Wp = DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using Presidencia.Numalet;
using System.Globalization;


public partial class paginas_Nomina_Frm_Rpt_Nom_Formato_Pago_Proveedores : System.Web.UI.Page {

    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Consulta los calendarios de nomina vigentes actualmente.
        ///CREO: Franscisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 12/Abril/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack) {
                Llenar_Combo_Proveedor();
                Llenar_Combo_Busqueda_Dependencia();
                Llenar_Grid_Busqueda_Empleados();
                Consultar_Calendario_Nominas();
            }
        }

    #endregion

    #region "Metodos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Calendario_Nominas
        ///DESCRIPCIÓN: Consulta los calendarios de nomina vigentes actualmente.
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 19/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Consultar_Calendario_Nominas()
        {
            Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nominas = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Calendario_Nominas = null;//Variable que almacenara una lista de los calendarios de nomina vigentes.
            try
            {
                Dt_Calendario_Nominas = Consulta_Calendario_Nominas.Consultar_Calendario_Nominas();
                if (Dt_Calendario_Nominas != null)
                {
                    if (Dt_Calendario_Nominas.Rows.Count > 0)
                    {
                        Dt_Calendario_Nominas = Formato_Fecha_Calendario_Nomina(Dt_Calendario_Nominas);
                        Cmb_Anio.DataSource = Dt_Calendario_Nominas;
                        Cmb_Anio.DataTextField = "Nomina";
                        Cmb_Anio.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                        Cmb_Anio.DataBind();
                        Cmb_Anio.SelectedIndex = -1;


                        Cmb_Anio.SelectedIndex = Cmb_Anio.Items.IndexOf(Cmb_Anio.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Anyo));

                        Consultar_Periodos_Catorcenales_Nomina(Cmb_Anio.SelectedValue.Trim());

                        Cmb_Periodo.SelectedIndex = Cmb_Periodo.Items.IndexOf(Cmb_Periodo.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));

                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "No se encontraron nominas vigentes";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las nominas. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
        ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
        ///calendario de nomina seleccionado.
        ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
        ///                        los periodos catorcenales.
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 19/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID)
        {
            Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
            DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

            try
            {
                Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
                Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();

                if (Dt_Periodos_Catorcenales != null)
                {
                    if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                    {
                        Cmb_Periodo.DataSource = Dt_Periodos_Catorcenales;
                        Cmb_Periodo.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                        Cmb_Periodo.DataValueField = Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID;
                        Cmb_Periodo.DataBind();
                        Cmb_Periodo.SelectedIndex = -1;
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
        /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
        /// sistema.
        /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
        ///             en el sistema.
        /// 
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 19/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
        {
            DataTable Dt_Nominas = new DataTable();//Variable que almacenara los calendarios de nóminas.
            DataRow Renglon_Dt_Clon = null;//Variable que almacenará un renglón del calendario de la nómina.

            //Creamos las columnas.
            Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
            Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

            foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
            {
                Renglon_Dt_Clon = Dt_Nominas.NewRow();
                Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
                Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
                Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
            }
            return Dt_Nominas;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Proveedor
        ///DESCRIPCIÓN: Llena el Combo de los Proveedores
        ///CREO: Franscisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 12/Abril/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Proveedor()
        {
            Cmb_Proveedor.DataSource = Cls_Rpt_Nom_Formato_Pago_Proveedores_Negocio.Consultar_Proveedores_Activos();
            Cmb_Proveedor.DataTextField = "NOMBRE_PROVEEDOR";
            Cmb_Proveedor.DataValueField = "PROVEEDOR_ID";
            Cmb_Proveedor.DataBind();
            Cmb_Proveedor.Items.Insert(0, new System.Web.UI.WebControls.ListItem("< - SELECCIONE - >", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Busqueda_Dependencia
        ///DESCRIPCIÓN: Llena el Combo de las Dependencias
        ///CREO: Franscisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 12/Abril/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Busqueda_Dependencia()
        {
            Cmb_Busqueda_Dependencia.DataSource = Cls_Rpt_Nom_Formato_Pago_Proveedores_Negocio.Consultar_Dependencias_Activas();
            Cmb_Busqueda_Dependencia.DataTextField = "NOMBRE";
            Cmb_Busqueda_Dependencia.DataValueField = "DEPENDENCIA_ID";
            Cmb_Busqueda_Dependencia.DataBind();
            Cmb_Busqueda_Dependencia.Items.Insert(0, new System.Web.UI.WebControls.ListItem("< - SELECCIONE - >", ""));
        }

        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validacion
        ///DESCRIPCIÓN          : Hace la Validacion del Reporte
        ///PARAMETROS           : 
        ///CREO                 :Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 12/Abril/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        private Boolean Validacion()
        {
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
            Boolean Datos_Validos = true;//Variable que alamacenara el resultado de la validacion de los datos ingresados por el usuario.
            try
            {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario Introducir: <br>";

                if (String.IsNullOrEmpty(Txt_Tesorero_Municipal.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "+ Seleccionar al Tesorero. <br>";
                    Datos_Validos = false;
                }
                if (String.IsNullOrEmpty(Txt_Director_Contabilidad_Presupuesto.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "+ Seleccionar al Director de Contabilidad y Presupuestos. <br>";
                    Datos_Validos = false;
                }
                if (String.IsNullOrEmpty(Txt_Director_Recursos_Humanos.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Text += "+ Seleccionar al Director de Recursos Humanos. <br>";
                    Datos_Validos = false;
                }
                return Datos_Validos;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al limpiar los controles Error [" + Ex.Message + "]");
            }
        }

    protected String Convertir_Cantidad_Letras(Double Cantidad_Numero)
    {
        Numalet Obj_Numale = new Numalet();
        String Cantidad_Letra = String.Empty;

        try
        {
            Obj_Numale.MascaraSalidaDecimal = "centavos";
            Obj_Numale.SeparadorDecimalSalida = "pesos con";
            Obj_Numale.LetraCapital = true;
            Obj_Numale.ConvertirDecimales = true;
            Obj_Numale.Decimales = 2;
            Obj_Numale.CultureInfo = new CultureInfo("es-MX");
            Obj_Numale.ApocoparUnoParteEntera = true;
            Cantidad_Letra = Obj_Numale.ToCustomCardinal(Cantidad_Numero).Trim().ToUpper();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al convertir la cantidad a letras. Error:[" + Ex.Message + "]");
        }
        return Cantidad_Letra;
    }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Monto
        ///DESCRIPCIÓN: Consulta el Monto a pagar a los proveedores
        ///PROPIEDADES:     
        ///CREO:                 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Consultar_Monto()
        {
            Cls_Rpt_Nom_Formato_Pago_Proveedores_Negocio Negocio = new Cls_Rpt_Nom_Formato_Pago_Proveedores_Negocio();
            if (Cmb_Proveedor.SelectedIndex > 0) { Negocio.P_Proveedor_ID = Cmb_Proveedor.SelectedItem.Value; }
            if (Cmb_Anio.SelectedIndex > 0) { Negocio.P_Nomina_ID = Cmb_Anio.SelectedItem.Value; }
            if (Cmb_Periodo.SelectedIndex > 0) { Negocio.P_No_Nomina = Cmb_Periodo.SelectedItem.Value; }
            DataTable Dt_Tmp = Negocio.Consultar_Monto_Pagar_Proveedor();
            if (Dt_Tmp != null && Dt_Tmp.Rows.Count > 0)
            {
                Txt_Monto_Pago_Proveedor.Text = String.Format("{0:c}", Convert.ToDouble(Dt_Tmp.Rows[0]["MONTO"].ToString()));
                Txt_Monto_Letra.Text = Convertir_Cantidad_Letras(Convert.ToDouble(Dt_Tmp.Rows[0]["MONTO"].ToString()));
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Busqueda_Empleados_Resguardo
        ///DESCRIPCIÓN: Llena el Grid con los empleados que cumplan el filtro
        ///PROPIEDADES:     
        ///CREO:                 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Busqueda_Empleados()
        {
            Grid_Busqueda_Empleados.SelectedIndex = (-1);
            Grid_Busqueda_Empleados.Columns[1].Visible = true;
            Cls_Rpt_Nom_Formato_Pago_Proveedores_Negocio Negocio = new Cls_Rpt_Nom_Formato_Pago_Proveedores_Negocio();
            if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado = String.Format("{0:000000}", Convert.ToInt32(Txt_Busqueda_No_Empleado.Text.Trim())); }
            if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_RFC_Empleado = Txt_Busqueda_RFC.Text.Trim(); }
            if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre_Empleado = Txt_Busqueda_Nombre_Empleado.Text.Trim().ToUpper(); }
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }
            Grid_Busqueda_Empleados.DataSource = Negocio.Consultar_Empleados();
            Grid_Busqueda_Empleados.DataBind();
            Grid_Busqueda_Empleados.Columns[1].Visible = false;
        }

    #endregion

    #region "Grids"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Busqueda_Empleados_Resguardo_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el evento de cambio de Página del GridView de Busqueda
        ///             de empleados.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Busqueda_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Grid_Busqueda_Empleados.PageIndex = e.NewPageIndex;
                Llenar_Grid_Busqueda_Empleados();
                MPE_Resguardante.Show();
            }
            catch (Exception Ex)
            {
                //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                //Lbl_Mensaje_Error.Text = "";
                //Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Busqueda_Empleados_Resguardo_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del GridView de Busqueda
        ///             de empleados.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Busqueda_Empleados_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Busqueda_Empleados.SelectedIndex > (-1))
                {
                    String Empleado_Seleccionado_ID = Grid_Busqueda_Empleados.SelectedRow.Cells[1].Text.Trim();
                    Cls_Cat_Empleados_Negocios Empleado_Negocio = new Cls_Cat_Empleados_Negocios();
                    Empleado_Negocio.P_Empleado_ID = Empleado_Seleccionado_ID.Trim();
                    DataTable Dt_Datos_Empleado = Empleado_Negocio.Consulta_Empleados_General();
                    String Empleado = ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString().Trim() : null);
                    Empleado = Empleado + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString().Trim() : null);
                    Empleado = Empleado + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString().Trim() : null);

                    switch (Hdf_Seleccionado.Value)
                    {
                        case "TESORERO":
                            Txt_Tesorero_Municipal.Text = Empleado;
                            break;
                        case "CONTABILIDAD":
                            Txt_Director_Contabilidad_Presupuesto.Text = Empleado;
                            break;
                        case "RH":
                            Txt_Director_Recursos_Humanos.Text = Empleado;
                            break;
                        default:
                            break;
                    }
                    MPE_Resguardante.Hide();
                    Grid_Busqueda_Empleados.SelectedIndex = (-1);
                }
            }
            catch (Exception Ex)
            {
                //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                //Lbl_Mensaje_Error.Text = "";
                //Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

    #region "Eventos"
    
        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Generar_Documento_Click
        ///DESCRIPCIÓN          :Genera el Documento con los datos seleccionados
        ///PARAMETROS           : 
        ///CREO                 :Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 12/Abril/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        protected void Btn_Generar_Documento_Click(object sender, ImageClickEventArgs e) {
                Lbl_Mensaje_Error.Text = String.Empty;
                Div_Contenedor_Msj_Error.Visible = false;
                String Ruta_Plantilla = String.Empty;
                String Documento_Salida = String.Empty;
                ReportDocument Reporte = new ReportDocument();
                StringBuilder newXml = new StringBuilder();
                MainDocumentPart main;
                CustomXmlPart CustomXml;
                String Nombre_Archivo = String.Empty;

                try {


                    if (Validacion()) {
                        String Datos = "";
                        Datos += "<No_Oficio>" + Txt_No_Oficio.Text.Trim() + "</No_Oficio>";
                        Datos += "<Fecha>" + String.Format("{0:'a' dd ' de ' MMMMMMMMMMMMMMMMMMMMMM 'del' yyyy}", DateTime.Today).ToUpper() + "</Fecha>";
                        Datos += "<Tesorero>" + Txt_Tesorero_Municipal.Text.Trim() + "</Tesorero>";
                        Datos += "<Director_Cont>" + Txt_Director_Contabilidad_Presupuesto.Text.Trim() + "</Director_Cont>";
                        Datos += "<Cantidad>" + Txt_Monto_Pago_Proveedor.Text.Trim() + " (" + Txt_Monto_Letra.Text + ")" + "</Cantidad>";
                        Datos += "<Mes>" + String.Format("{0:MMMMMMMMMMMMMMMMMMMMMM}", DateTime.Today) + "</Mes>";
                        Datos += "<Anio>" + Cmb_Anio.SelectedItem.Text + "</Anio>";
                        Datos += "<Proveedor>" + Cmb_Proveedor.SelectedItem.Text.Trim() + "</Proveedor>";
                        Datos += "<Director_RH>" + Txt_Director_Recursos_Humanos.Text.Trim() + "</Director_RH>";
                        Ruta_Plantilla = Server.MapPath("Plantillas/Plantilla_Pago_Proveedores.docx");
                        Nombre_Archivo = "Plantilla_Pago_Proveedores_" + Session.SessionID + ".doc";
                        Documento_Salida = Server.MapPath("../../Reporte/" + Nombre_Archivo);

                        //eliminamos el documento si es que existe
                        if (System.IO.Directory.Exists(Server.MapPath("../../Reporte"))) {
                            if (System.IO.File.Exists(Documento_Salida)) {
                                System.IO.File.Delete(Documento_Salida);
                            }
                        } else {
                            System.IO.Directory.CreateDirectory("../../Reporte");
                        }
                        //copiamos la plantilla
                        File.Copy(Ruta_Plantilla, Documento_Salida);

                        using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true)) {
                            newXml.Append("<root>");
                            newXml.Append(Datos);
                            newXml.Append("</root>");

                            main = doc.MainDocumentPart;
                            main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);
                            CustomXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                            using (StreamWriter ts = new StreamWriter(CustomXml.GetStream())) {
                                ts.Write(newXml);
                            }
                            // guardar los cambios en el documento
                            main.Document.Save();
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
                }  catch (Exception Ex) {
                    throw new Exception("Error al elaborar la constancia Error [" + Ex.Message + "]");
                }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cmb_Proveedor_SelectedIndexChanged
        ///DESCRIPCIÓN: Consulta el monto del proveedor que se selecciona
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 19/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Cmb_Proveedor_SelectedIndexChanged(Object sender, EventArgs e) {
            Consultar_Monto();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cmb_Anio_SelectedIndexChanged
        ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 19/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Cmb_Anio_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Nomina_Seleccionada = 0;//Variable que almacena la nómina seleccionada del combo.

            try
            {
                ////Obtenemos elemento seleccionado del combo.
                if (Cmb_Anio.SelectedIndex > 0)
                {
                    Nomina_Seleccionada = Convert.ToInt32(Cmb_Anio.SelectedItem.Text.Trim());
                    Consultar_Periodos_Catorcenales_Nomina(Cmb_Anio.SelectedValue.Trim());
                }
                else
                {
                    Cmb_Periodo.DataSource = new DataTable();
                    Cmb_Periodo.DataBind();
                }
            }

            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message.ToString();
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cmb_Periodo_SelectedIndexChanged
        ///DESCRIPCIÓN: Consulta el Cambio del Periodo el monto.
        ///CREO: Juan alberto Hernández Negrete
        ///FECHA_CREO: 19/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Cmb_Periodo_SelectedIndexChanged(object sender, EventArgs e) {
            Consultar_Monto();
        }

        ///*********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Empleado_Click
        ///DESCRIPCIÓN          : Evento del boton de busqueda de empleados
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 24/Marzo/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        protected void Btn_Buscar_Tesorero_Click(object sender, ImageClickEventArgs e)
        {
            Hdf_Seleccionado.Value = "TESORERO";
            MPE_Resguardante.Show();
        }

        ///*********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Empleado_Click
        ///DESCRIPCIÓN          : Evento del boton de busqueda de empleados
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 24/Marzo/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        protected void Btn_Buscar_Director_Contabilidad_Click(object sender, ImageClickEventArgs e)
        {
            Hdf_Seleccionado.Value = "CONTABILIDAD";
            MPE_Resguardante.Show();
        }

        ///*********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Empleado_Click
        ///DESCRIPCIÓN          : Evento del boton de busqueda de empleados
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 24/Marzo/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        protected void Btn_Buscar_Director_RH_Click(object sender, ImageClickEventArgs e)
        {
            Hdf_Seleccionado.Value = "RH";
            MPE_Resguardante.Show();
        }
       
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Empleados_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda Avanzada para el Resguardante.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
        {
            try
            {
                Grid_Busqueda_Empleados.PageIndex = 0;
                Llenar_Grid_Busqueda_Empleados();
                MPE_Resguardante.Show();
            }
            catch (Exception Ex)
            {
                //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                //Lbl_Mensaje_Error.Text = "";
                //Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

}