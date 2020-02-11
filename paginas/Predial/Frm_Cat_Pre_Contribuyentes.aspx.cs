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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Contribuyentes.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Contribuyentes : System.Web.UI.Page {

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 11/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************        
        protected void Page_Load(object sender, EventArgs e){
            try
            {
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

                if (!IsPostBack)
                {
                    Configuracion_Acceso("Frm_Cat_Pre_Contribuyentes.aspx");
                    Configuracion_Formulario(true); ;
                    Llenar_Tabla_Contribuyentes(0);
                }
            }
            catch (Exception ex)
            {
                Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
                Lbl_Ecabezado_Mensaje.Visible = true;
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion

    #region Metodos
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. estatus.    Estatus en el que se cargara la configuración de los
        ///                             controles.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 11/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario( Boolean estatus ) {
            Txt_Busqueda_Contribuyente.Enabled = estatus;
            Btn_Buscar_Contribuyente.Enabled = estatus;
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Eliminar.Visible = estatus;
            Grid_Contribuyentes.Enabled = estatus;
            Grid_Contribuyentes.SelectedIndex = (-1);
            Cmb_Tipo_Persona.Enabled = !estatus;
            Cmb_Estatus.Enabled = !estatus;
            Txt_Apellido_Paterno.Enabled = !estatus;
            Txt_Apellido_Materno.Enabled = !estatus;
            Txt_Nombre.Enabled = !estatus;
            Cmb_Sexo.Enabled = !estatus;
            Cmb_Estado_Civil.Enabled = !estatus;
            Txt_Fecha_Nacimiento.Enabled = false;
            Btn_Fecha_Nacimiento.Enabled = !estatus;
            Txt_RFC.Enabled = !estatus;
            Txt_CURP.Enabled = !estatus;
            Txt_IFE.Enabled = !estatus;
            Txt_Representante_Legal.Enabled = !estatus;
            Cmb_Tipo_Propietario.Enabled = !estatus;
            Txt_Domicilio.Enabled = !estatus;
            Txt_Interior.Enabled = !estatus;
            Txt_Exterior.Enabled = !estatus;
            Txt_Colonia.Enabled = !estatus;
            Txt_Ciudad.Enabled = !estatus;
            Txt_Estado.Enabled = !estatus;
            Txt_Codigo_Postal.Enabled = !estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 11/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Hdf_Contribuyente.Value = "";
            Txt_ID_Contribuyente.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Cmb_Tipo_Persona.SelectedIndex = 0;
            Txt_Apellido_Paterno.Text = "";
            Txt_Apellido_Materno.Text = "";
            Txt_Nombre.Text = "";
            Cmb_Sexo.SelectedIndex = 0;
            Cmb_Estado_Civil.SelectedIndex = 0;
            Txt_Fecha_Nacimiento.Text = "";
            Txt_RFC.Text = "";
            Txt_CURP.Text = "";
            Txt_IFE.Text = "";
            Txt_Representante_Legal.Text = "";

            Cmb_Tipo_Propietario.SelectedIndex = 0;
            Txt_Domicilio.Text = "";
            Txt_Interior.Text = "";
            Txt_Exterior.Text = "";
            Txt_Colonia.Text = "";
            Txt_Ciudad.Text = "";
            Txt_Codigo_Postal.Text = "";
            Txt_Estado.Text = "";

            TWE_Txt_Apellido_Paterno.WatermarkText = " ";
            TWE_Txt_Apellido_Materno.WatermarkText = " ";
            TWE_Txt_Nombre.WatermarkText = " ";
            TWE_Txt_Fecha_Nacimiento.WatermarkText = " ";
            TWE_Txt_RFC.WatermarkText = " ";
            TWE_Txt_CURP.WatermarkText = " ";
            TWE_Txt_IFE.WatermarkText = " ";
            TWE_Txt_Representante_Legal.WatermarkText = " ";

            TWE_Txt_Domicilio.WatermarkText = " ";
            TWE_Txt_Interior.WatermarkText = " ";
            TWE_Txt_Exterior.WatermarkText = " ";
            TWE_Txt_Colonia.WatermarkText = " ";
            TWE_Txt_Ciudad.WatermarkText = " ";
            TWE_Txt_Codigo_Postal.WatermarkText = " ";
            TWE_Txt_Estado.WatermarkText = " ";
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Controla la habilitacion de Controles dependiendo del Tipo de 
        ///             Contribuyente.
        ///PROPIEDADES:     
        ///             1.  Configuracion.      Tipo de Persona para cagar su configuracion
        ///             2.  Mostrar_Marcas.     Muestra o no un mensaje en las Marcas de 
        ///                                     Agua de los Campos
        ///             3.  Habilitar_Campo.    Habilitar o no los Campos
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario(String Configuracion, Boolean Mostrar_Marcas, Boolean Habilitar_Campo) {
            if (Configuracion.Equals("FISICA")) {
                if (Habilitar_Campo) {
                    Txt_RFC.Enabled = true;
                    Txt_Nombre.Enabled = true;
                    Txt_Apellido_Paterno.Enabled = true;
                    Txt_Apellido_Materno.Enabled = true;
                    Cmb_Estatus.Enabled = true;
                    Cmb_Sexo.Enabled = true;
                    Cmb_Estado_Civil.Enabled = true;
                    Txt_Fecha_Nacimiento.Enabled = true;
                    Btn_Fecha_Nacimiento.Enabled = true;
                    Txt_CURP.Enabled = true;
                    Txt_IFE.Enabled = true;
                    Txt_Representante_Legal.Enabled = false;

                    Cmb_Tipo_Propietario.Enabled = true;
                    Txt_Domicilio.Enabled = true;
                    Txt_Interior.Enabled = true;
                    Txt_Exterior.Enabled = true;
                    Txt_Colonia.Enabled = true;
                    Txt_Ciudad.Enabled = true;
                    Txt_Codigo_Postal.Enabled = true;
                    Txt_Estado.Enabled = true;
                }
                if (Mostrar_Marcas) {
                    TWE_Txt_RFC.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Nombre.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Apellido_Paterno.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Apellido_Materno.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Fecha_Nacimiento.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_CURP.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_IFE.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Representante_Legal.WatermarkText = HttpUtility.HtmlDecode("&lt; NO APLICA &gt;");

                    TWE_Txt_Domicilio.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Interior.WatermarkText = HttpUtility.HtmlDecode("&lt; OPCIONAL &gt;");
                    TWE_Txt_Exterior.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Colonia.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Ciudad.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Codigo_Postal.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Estado.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");   

                }
            } else if (Configuracion.Equals("MORAL")) {
                if (Habilitar_Campo) {
                    Txt_RFC.Enabled = true;
                    Txt_Nombre.Enabled = true;
                    Txt_Apellido_Paterno.Enabled = false;
                    Txt_Apellido_Materno.Enabled = false;
                    Cmb_Sexo.Enabled = false;
                    Cmb_Estatus.Enabled = true;
                    Cmb_Estado_Civil.Enabled = false;
                    Txt_Fecha_Nacimiento.Enabled = true;
                    Btn_Fecha_Nacimiento.Enabled = true;
                    Txt_CURP.Enabled = false;
                    Txt_IFE.Enabled = false;
                    Txt_Representante_Legal.Enabled = true;

                    Cmb_Tipo_Propietario.Enabled = true;
                    Txt_Domicilio.Enabled = true;
                    Txt_Interior.Enabled = true;
                    Txt_Exterior.Enabled = true;
                    Txt_Colonia.Enabled = true;
                    Txt_Ciudad.Enabled = true;
                    Txt_Codigo_Postal.Enabled = true;
                    Txt_Estado.Enabled = true;            
                }
                if (Mostrar_Marcas) {
                    TWE_Txt_RFC.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Nombre.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Apellido_Paterno.WatermarkText = HttpUtility.HtmlDecode("&lt; NO APLICA &gt;");
                    TWE_Txt_Apellido_Materno.WatermarkText = HttpUtility.HtmlDecode("&lt; NO APLICA &gt;");
                    TWE_Txt_Fecha_Nacimiento.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_CURP.WatermarkText = HttpUtility.HtmlDecode("&lt; NO APLICA &gt;");
                    TWE_Txt_IFE.WatermarkText = HttpUtility.HtmlDecode("&lt; NO APLICA &gt;");
                    TWE_Txt_Representante_Legal.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");

                    TWE_Txt_Domicilio.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Interior.WatermarkText = HttpUtility.HtmlDecode("&lt; OPCIONAL &gt;");
                    TWE_Txt_Exterior.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Colonia.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Ciudad.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Codigo_Postal.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");
                    TWE_Txt_Estado.WatermarkText = HttpUtility.HtmlDecode("&lt; APLICA &gt;");          
                }
            } else {
                if (Habilitar_Campo) {
                    Txt_RFC.Enabled = false;
                    Txt_Nombre.Enabled = false;
                    Txt_Apellido_Paterno.Enabled = false;
                    Txt_Apellido_Materno.Enabled = false;
                    Cmb_Estatus.Enabled = false;
                    Cmb_Sexo.Enabled = false;
                    Cmb_Estado_Civil.Enabled = false;
                    Txt_Fecha_Nacimiento.Enabled = false;
                    Btn_Fecha_Nacimiento.Enabled = false;
                    Txt_CURP.Enabled = false;
                    Txt_IFE.Enabled = false;
                    Txt_Representante_Legal.Enabled = false;

                    Cmb_Tipo_Propietario.Enabled = false;
                    Txt_Domicilio.Enabled = false;
                    Txt_Interior.Enabled = false;
                    Txt_Exterior.Enabled = false;
                    Txt_Colonia.Enabled = false;
                    Txt_Ciudad.Enabled = false;
                    Txt_Codigo_Postal.Enabled = false;
                    Txt_Estado.Enabled = false;         
                }
                if (Mostrar_Marcas) {
                    TWE_Txt_RFC.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_Nombre.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_Apellido_Paterno.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_Apellido_Materno.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_Fecha_Nacimiento.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_CURP.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_IFE.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_Representante_Legal.WatermarkText = HttpUtility.HtmlDecode(" ");

                    TWE_Txt_Domicilio.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_Interior.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_Exterior.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_Colonia.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_Ciudad.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_Codigo_Postal.WatermarkText = HttpUtility.HtmlDecode(" ");
                    TWE_Txt_Estado.WatermarkText = HttpUtility.HtmlDecode(" ");          
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Contribuyentes
        ///DESCRIPCIÓN: Llena la tabla de Contribuyentes con una consulta que puede o no
        ///             tener Filtros.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Contribuyentes(int Pagina) {
            try{
                Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente = new Cls_Cat_Pre_Contribuyentes_Negocio();
                DataTable Tabla = Contribuyente.Consultar_Contribuyentes();
                DataView Vista = new DataView(Tabla);
                String Expresion_De_Busqueda = string.Format("{0} '%{1}%'", Grid_Contribuyentes.SortExpression, Txt_Busqueda_Contribuyente.Text.Trim().ToUpper());
                Vista.RowFilter = "NOMBRE_COMPLETO LIKE " + Expresion_De_Busqueda;
                Grid_Contribuyentes.DataSource = Vista;
                Grid_Contribuyentes.PageIndex = Pagina;
                Grid_Contribuyentes.DataBind();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Sugerir_RFC
        ///DESCRIPCIÓN: Si el campo no contiene 10 caracteres o mas y ya se llenaron los 
        ///         apellidos, el nombre y la fecha de nacimiento, se sugiere un RFC
        ///PROPIEDADES:     
        ///CREO: Roberto Gonzalez Oseguera
        ///FECHA_CREO: 15-07-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected String Sugerir_RFC()
        {
            String Iniciales = "";
            String Fecha = "";
            DateTime Fecha_Nacimiento = new DateTime();
            String RFC = "";

            // validar primero si el campo RFC esta vacio o tiene ceros al final, sugerir RFC
                if (Txt_RFC.Text.Length < 4 || Txt_RFC.Text.Substring(Txt_RFC.Text.Length - 3, 3) == "000")
            {

                if (Cmb_Tipo_Persona.SelectedItem.Value.Equals("FISICA"))
                {
                    // si los campos de texto ya contienen valores, y hay fecha, sugerir RFC con fecha
                    if (Txt_Apellido_Paterno.Text.Length > 2 && Txt_Apellido_Materno.Text.Length > 2
                        && Txt_Nombre.Text.Length > 2)
                    {
                        char[] Letras_APaterno = Txt_Apellido_Paterno.Text.ToCharArray();
                        char[] Letras_AMaterno = Txt_Apellido_Materno.Text.ToCharArray();
                        char[] Letras_Nom;
                        String[] Nombres = Txt_Nombre.Text.Split(' ');
                        int i = 1;

                        // si hay una fecha de nacimiento valida
                        if (DateTime.TryParse(Txt_Fecha_Nacimiento.Text, out Fecha_Nacimiento))
                        {
                            Fecha = Fecha_Nacimiento.ToString("yyMMdd");
                        }

                        // si son dos o mas nombre
                        if (Nombres.Length >= 2)
                        {
                            // si el nombre es jose, maria o guadalupe, tomar inicial del segundo nombre
                            if (Nombres[0].ToLower() == "jose" || Nombres[0].ToLower() == "josé" ||
                                Nombres[0].ToLower() == "maria" || Nombres[0].ToLower() == "maría" || Nombres[0].ToLower() == "ma" ||
                                Nombres[0].ToLower() == "guadalupe")
                            {
                                Letras_Nom = Nombres[1].ToCharArray();
                            }
                            else // tomar nombre completo
                            {
                                Letras_Nom = Txt_Nombre.Text.ToCharArray();
                            }
                        }
                        else
                        {
                            Letras_Nom = Txt_Nombre.Text.ToCharArray();
                        }
                        Iniciales = char.ToUpper(Letras_APaterno[0]).ToString();
                        do
                        {
                            // si es vocal agregar a iniciales
                            if (Es_Vocal(Char.ToLower(Letras_APaterno[i])))
                            {
                                Iniciales += char.ToUpper(Letras_APaterno[i]).ToString();
                            }
                            else        // si no, incrementar contador
                            {
                                i++;
                            }
                        } while (i < Letras_APaterno.Length && Iniciales.Length < 2);
                        Iniciales += char.ToUpper(Letras_AMaterno[0]).ToString();
                        Iniciales += char.ToUpper(Letras_Nom[0]).ToString();

                        if (Fecha == "" && Iniciales.Length > 0)
                        {
                            Fecha = "000000";
                        }
                    }

                }
                // personas morales
                else
                {
                    // si los campos de texto ya contienen valores, y hay fecha, sugerir RFC con fecha
                    if (Txt_Nombre.Text.Length > 2)
                    {
                        char[] Letras_Nom = Txt_Nombre.Text.ToCharArray();
                        Iniciales += char.ToUpper(Letras_Nom[0]).ToString();
                        Iniciales += char.ToUpper(Letras_Nom[1]).ToString();
                        Iniciales += char.ToUpper(Letras_Nom[2]).ToString();

                        // si hay una fecha de nacimiento valida
                        if (DateTime.TryParse(Txt_Fecha_Nacimiento.Text, out Fecha_Nacimiento))
                        {
                            Fecha = Fecha_Nacimiento.ToString("yyMMdd");
                        }

                        if (Fecha == "")
                        {
                            Fecha = "000000";
                        }
                    }
                }
                RFC = Iniciales + Fecha;
                return RFC;
            }
            return String.Empty;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Es_Vocal
        ///DESCRIPCIÓN: Si el char que se recibe es vocal, regresa verdadero
        ///PROPIEDADES:     
        ///CREO: Roberto Gonzalez Oseguera
        ///FECHA_CREO: 15-07-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private bool Es_Vocal(char letter)
        {
            return letter == 'a' ||
            letter == 'e' ||
            letter == 'i' ||
            letter == 'o' ||
            letter == 'u' ||
            letter == 'á' ||
            letter == 'é' ||
            letter == 'í' ||
            letter == 'ó' ||
            letter == 'ú';
        }


        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 13/Septiembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Cmb_Estatus.SelectedIndex == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
                    Validacion = false;
                }
                if (Cmb_Tipo_Persona.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Tipo de Persona.";
                    Validacion = false;
                }
                if (Txt_Nombre.Text.Trim().Equals("")) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre.";
                    Validacion = false;
                }
                if (Txt_RFC.Text.Trim().Equals("") || Txt_RFC.Text.Length<4 || Txt_RFC.Text.Length>10) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir el RFC.";
                    Validacion = false;
                }
                else
                {
                    if (Txt_RFC.Text.Length >= 4)
                    {
                        while(Txt_RFC.Text.Length<10)
                        {
                            Txt_RFC.Text = Txt_RFC.Text + "0";
                        }
                    }
                }
                if (Cmb_Tipo_Persona.SelectedItem.Value.Equals("FISICA")) {
                    if (Txt_Apellido_Paterno.Text.Trim().Equals("")) {
                        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                        Mensaje_Error = Mensaje_Error + "+ Introducir el Apellido Paterno.";
                        Validacion = false;
                    }
                    if (Txt_Apellido_Materno.Text.Trim().Equals("")) {
                        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                        Mensaje_Error = Mensaje_Error + "+ Introducir el Apellido Materno.";
                        Validacion = false;
                    }
                    //if (Cmb_Sexo.SelectedIndex == 0) {
                    //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    //    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Sexo.";
                    //    Validacion = false;
                    //}
                    //if (Cmb_Estado_Civil.SelectedIndex == 0) {
                    //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    //    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estado Civil.";
                    //    Validacion = false;
                    //}
                    //if (Txt_Fecha_Nacimiento.Text.Trim().Equals("")) {
                    //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    //    Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha de Nacimiento.";
                    //    Validacion = false;
                    //}
                    //if (Txt_CURP.Text.Trim().Equals(""))
                    //{
                    //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    //    Mensaje_Error = Mensaje_Error + "+ Introducir el CURP.";
                    //    Validacion = false;
                    //}
                    //if (Txt_IFE.Text.Trim().Equals("") || Txt_IFE.Text.Length!=13) {
                    //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    //    Mensaje_Error = Mensaje_Error + "+ Introducir el IFE.";
                    //    Validacion = false;
                    //}
                } else {
                    if (Cmb_Tipo_Persona.SelectedItem.Value.Equals("MORAL")) {
                        if (Txt_Representante_Legal.Text.Trim().Equals("")) {
                            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                            Mensaje_Error = Mensaje_Error + "+ Introducir el Representante Legal.";
                            Validacion = false;
                        }
                    }
                }
                //if (Cmb_Tipo_Propietario.SelectedIndex == 0) {
                //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                //    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Tipo de Propietario.";
                //    Validacion = false;
                //}
                //if (Txt_Domicilio.Text.Trim().Equals("")) {
                //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                //    Mensaje_Error = Mensaje_Error + "+ Introducir el Domicilio.";
                //    Validacion = false;
                //}
                //if (Txt_Exterior.Text.Trim().Equals(""))
                //{
                //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                //    Mensaje_Error = Mensaje_Error + "+ Introducir el Número Exterior.";
                //    Validacion = false;
                //}
                //if (Txt_Colonia.Text.Trim().Equals(""))
                //{
                //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                //    Mensaje_Error = Mensaje_Error + "+ Introducir la Colonia.";
                //    Validacion = false;
                //}
                //if (Txt_Ciudad.Text.Trim().Equals(""))
                //{
                //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                //    Mensaje_Error = Mensaje_Error + "+ Introducir la Ciudad.";
                //    Validacion = false;
                //}
                //if (Txt_Codigo_Postal.Text.Trim().Equals(""))
                //{
                //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                //    Mensaje_Error = Mensaje_Error + "+ Introducir la Codigo Postal.";
                //    Validacion = false;
                //}
                //if (Txt_Estado.Text.Trim().Equals(""))
                //{
                //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                //    Mensaje_Error = Mensaje_Error + "+ Introducir la Estado.";
                //    Validacion = false;
                //}

                if (!Validacion)
                {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

        #endregion

    #endregion

    #region Grids
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Contribuyentes_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Contribuyentes
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Contribuyentes_PageIndexChanging(object sender, GridViewPageEventArgs e){
            Grid_Contribuyentes.SelectedIndex = (-1);
            Llenar_Tabla_Contribuyentes(e.NewPageIndex);
            Limpiar_Catalogo();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Contribuyentes_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de un Contribuyente Seleccionado para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Contribuyentes_SelectedIndexChanged(object sender, EventArgs e){
            if (Grid_Contribuyentes.SelectedIndex > (-1)) {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Contribuyentes.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente = new Cls_Cat_Pre_Contribuyentes_Negocio();
                Contribuyente.P_Contribuyente_ID = ID_Seleccionado;
                Contribuyente = Contribuyente.Consultar_Datos_Contribuyente();
                Hdf_Contribuyente.Value = Contribuyente.P_Contribuyente_ID;
                Txt_ID_Contribuyente.Text = Contribuyente.P_Contribuyente_ID;
                Txt_Nombre.Text = Contribuyente.P_Nombre;
                Txt_RFC.Text = Contribuyente.P_RFC;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Contribuyente.P_Estatus));
                Cmb_Tipo_Persona.SelectedIndex = Cmb_Tipo_Persona.Items.IndexOf(Cmb_Tipo_Persona.Items.FindByValue(Contribuyente.P_Tipo_Persona));
                if (Contribuyente.P_Tipo_Persona.Equals("FISICA")) {
                    Txt_Apellido_Paterno.Text = Contribuyente.P_Apellido_Paterno;
                    Txt_Apellido_Materno.Text = Contribuyente.P_Apellido_Materno;
                    Txt_Fecha_Nacimiento.Text = String.Format("{0:dd/MMM/yyyy}", Contribuyente.P_Fecha_Nacimiento);
                    Txt_CURP.Text = Contribuyente.P_CURP;
                    Txt_IFE.Text = Contribuyente.P_IFE;
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Contribuyente.P_Estatus));
                    Cmb_Tipo_Persona.SelectedIndex = Cmb_Tipo_Persona.Items.IndexOf(Cmb_Tipo_Persona.Items.FindByValue(Contribuyente.P_Tipo_Persona));
                    Cmb_Sexo.SelectedIndex = Cmb_Sexo.Items.IndexOf(Cmb_Sexo.Items.FindByValue(Contribuyente.P_Sexo));
                    Cmb_Estado_Civil.SelectedIndex = Cmb_Estado_Civil.Items.IndexOf(Cmb_Estado_Civil.Items.FindByValue(Contribuyente.P_Estado_Civil));
                } else {
                    Txt_Representante_Legal.Text = Contribuyente.P_Representante_Legal;
                }
                Cmb_Tipo_Propietario.SelectedIndex = Cmb_Tipo_Propietario.Items.IndexOf(Cmb_Tipo_Propietario.Items.FindByValue(Contribuyente.P_Tipo_Propietario));
                Txt_Domicilio.Text = Contribuyente.P_Domicilio;
                Txt_Interior.Text = Contribuyente.P_Interior;
                Txt_Exterior.Text = Contribuyente.P_Exterior;
                Txt_Colonia.Text = Contribuyente.P_Colonia;
                Txt_Ciudad.Text = Contribuyente.P_Ciudad;
                Txt_Codigo_Postal.Text = Contribuyente.P_Codigo_Postal;
                Txt_Estado.Text = Contribuyente.P_Estado;
                Configuracion_Formulario(Contribuyente.P_Tipo_Persona, true, false);
                System.Threading.Thread.Sleep(1000);          
            }
        }
    
    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Contribuyente
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e){
            try{
                if (Btn_Nuevo.AlternateText.Equals("Nuevo")){
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Configuracion_Formulario("SELECCIONE", true, true);
                } else {
                    if (Validar_Componentes()) {
                        Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente = new Cls_Cat_Pre_Contribuyentes_Negocio();
                        Contribuyente.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Contribuyente.P_Tipo_Persona = Cmb_Tipo_Persona.SelectedItem.Value;
                        Contribuyente.P_Nombre = Txt_Nombre.Text.Trim().ToUpper();
                        Contribuyente.P_RFC = Txt_RFC.Text.Trim().ToUpper();
                        if (Contribuyente.P_Tipo_Persona.Equals("FISICA")){
                            Contribuyente.P_Apellido_Paterno = Txt_Apellido_Paterno.Text.Trim().ToUpper();
                            Contribuyente.P_Apellido_Materno = Txt_Apellido_Materno.Text.Trim().ToUpper();
                            Contribuyente.P_Sexo = Cmb_Sexo.SelectedItem.Value;
                            Contribuyente.P_Estado_Civil = Cmb_Estado_Civil.SelectedItem.Value;
                            Contribuyente.P_Fecha_Nacimiento = Convert.ToDateTime(Txt_Fecha_Nacimiento.Text.Trim());
                            Contribuyente.P_CURP = Txt_CURP.Text.Trim().ToUpper();
                            Contribuyente.P_IFE = Txt_IFE.Text.Trim().ToUpper();
                        }else {
                            Contribuyente.P_Representante_Legal = Txt_Representante_Legal.Text.Trim().ToUpper();
                        }
                        Contribuyente.P_Tipo_Propietario = Cmb_Tipo_Propietario.SelectedItem.Value;
                        Contribuyente.P_Domicilio = Txt_Domicilio.Text.Trim().ToUpper();
                        Contribuyente.P_Interior = Txt_Interior.Text.Trim();
                        Contribuyente.P_Exterior = Txt_Exterior.Text.Trim();
                        Contribuyente.P_Colonia = Txt_Colonia.Text.Trim().ToUpper();
                        Contribuyente.P_Ciudad = Txt_Ciudad.Text.Trim().ToUpper();
                        Contribuyente.P_Codigo_Postal = Txt_Codigo_Postal.Text.Trim();
                        Contribuyente.P_Estado = Txt_Estado.Text.Trim().ToUpper();
                        Contribuyente.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Contribuyente.Alta_Contribuyente();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Contribuyentes(Grid_Contribuyentes.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Contribuyentes", "alert('Alta de Contribuyente Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }    
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un
        ///             Contribuyente.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Grid_Contribuyentes.Rows.Count > 0 && Grid_Contribuyentes.SelectedIndex > (-1)){
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                    }else{
                        Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } else {
                    if (Validar_Componentes()){
                        Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente = new Cls_Cat_Pre_Contribuyentes_Negocio();
                        Contribuyente.P_Contribuyente_ID = Hdf_Contribuyente.Value;
                        Contribuyente.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Contribuyente.P_Tipo_Persona = Cmb_Tipo_Persona.SelectedItem.Value;
                        Contribuyente.P_Nombre = Txt_Nombre.Text.Trim().ToUpper();
                        Contribuyente.P_RFC = Txt_RFC.Text.Trim().ToUpper();
                        if (Contribuyente.P_Tipo_Persona.Equals("FISICA")){
                            Contribuyente.P_Apellido_Paterno = Txt_Apellido_Paterno.Text.Trim().ToUpper();
                            Contribuyente.P_Apellido_Materno = Txt_Apellido_Materno.Text.Trim().ToUpper();
                            Contribuyente.P_Sexo = Cmb_Sexo.SelectedItem.Value;
                            Contribuyente.P_Estado_Civil = Cmb_Estado_Civil.SelectedItem.Value;
                            Contribuyente.P_Fecha_Nacimiento = Convert.ToDateTime(Txt_Fecha_Nacimiento.Text.Trim());
                            Contribuyente.P_CURP = Txt_CURP.Text.Trim().ToUpper();
                            Contribuyente.P_IFE = Txt_IFE.Text.Trim().ToUpper();
                        }else{
                            Contribuyente.P_Representante_Legal = Txt_Representante_Legal.Text.Trim().ToUpper();
                        }
                        Contribuyente.P_Tipo_Propietario = Cmb_Tipo_Propietario.SelectedItem.Value;
                        Contribuyente.P_Domicilio = Txt_Domicilio.Text.Trim().ToUpper();
                        Contribuyente.P_Interior = Txt_Interior.Text.Trim();
                        Contribuyente.P_Exterior = Txt_Exterior.Text.Trim();
                        Contribuyente.P_Colonia = Txt_Colonia.Text.Trim().ToUpper();
                        Contribuyente.P_Ciudad = Txt_Ciudad.Text.Trim().ToUpper();
                        Contribuyente.P_Codigo_Postal = Txt_Codigo_Postal.Text.Trim();
                        Contribuyente.P_Estado = Txt_Estado.Text.Trim().ToUpper();
                        Contribuyente.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Contribuyente.Modificar_Contribuyente();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Contribuyentes(Grid_Contribuyentes.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Contribuyentes", "alert('Actualización Contribuyente Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Contribuyente_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Contribuyente_Click(object sender, ImageClickEventArgs e){
            try{
                Limpiar_Catalogo();
                Grid_Contribuyentes.SelectedIndex = (-1);
                Llenar_Tabla_Contribuyentes(0);
                if (Grid_Contribuyentes.Rows.Count == 0 && Txt_Busqueda_Contribuyente.Text.Trim().Length > 0) {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el nombre \"" + Txt_Busqueda_Contribuyente.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todos los contribuyentes almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Contribuyente.Text = "";
                    Llenar_Tabla_Contribuyentes(0);
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un Contribuyente de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e){
            try{
                if (Grid_Contribuyentes.Rows.Count > 0 && Grid_Contribuyentes.SelectedIndex > (-1)){
                    Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente = new Cls_Cat_Pre_Contribuyentes_Negocio();
                    Contribuyente.P_Contribuyente_ID = Grid_Contribuyentes.SelectedRow.Cells[1].Text;
                    Contribuyente.Eliminar_Contribuyente();
                    Grid_Contribuyentes.SelectedIndex = (-1);
                    Llenar_Tabla_Contribuyentes(Grid_Contribuyentes.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Contribuyentes", "alert('El Contribuyente fue eliminado exitosamente');", true);
                    Limpiar_Catalogo();
                }else{
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale
        ///             del Formulario.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e){
            if (Btn_Salir.AlternateText.Equals("Salir")){
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }else {
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Habilita los Controles dependiendo del Tipo de Contribuyente.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Cmb_Tipo_Persona_SelectedIndexChanged(object sender, EventArgs e) {
            Configuracion_Formulario(Cmb_Tipo_Persona.SelectedItem.Value, true, true);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Apellido_Paterno_TextChanged
        ///DESCRIPCIÓN: Maneja el evento cambio de texto en la caja de texto Apellido paterno
        ///             llama al metodo para sugerir el RFC
        ///PROPIEDADES:     
        ///CREO: Roberto Gonzalez
        ///FECHA_CREO: 15-07-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Apellido_Paterno_TextChanged(object sender, EventArgs e)
        {
            String RFC_Sugerido = Sugerir_RFC();

            if (RFC_Sugerido.Length > 0)
            {
                Txt_RFC.Text = RFC_Sugerido;
            }
            Txt_Apellido_Materno.Focus();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Apellido_Materno_TextChanged
        ///DESCRIPCIÓN: Maneja el evento cambio de texto en la caja de texto Apellido materno
        ///             llama al metodo para sugerir el RFC
        ///PROPIEDADES:     
        ///CREO: Roberto Gonzalez Oseguera
        ///FECHA_CREO: 15-07-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Apellido_Materno_TextChanged(object sender, EventArgs e)
        {
            String RFC_Sugerido = Sugerir_RFC();

            if (RFC_Sugerido.Length > 0)
            {
                Txt_RFC.Text = RFC_Sugerido;
            }
            Txt_Nombre.Focus();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Nombre_TextChanged
        ///DESCRIPCIÓN: Maneja el evento cambio de texto en la caja de texto Nombre
        ///             llama al metodo para sugerir el RFC (solo personas fisicas)
        ///PROPIEDADES:     
        ///CREO: Roberto Gonzalez
        ///FECHA_CREO: 15-07-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Nombre_TextChanged(object sender, EventArgs e)
        {
            String RFC_Sugerido = Sugerir_RFC();

            if (RFC_Sugerido.Length > 0)
            {
                Txt_RFC.Text = RFC_Sugerido;
            }
            Cmb_Sexo.Focus();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Fecha_Nacimiento_TextChanged
        ///DESCRIPCIÓN: Maneja el evento cambio de texto en la caja de texto Fecha nacimiento
        ///             llama al metodo para sugerir el RFC
        ///PROPIEDADES:     
        ///CREO: Roberto Gonzalez
        ///FECHA_CREO: 15-07-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Txt_Fecha_Nacimiento_TextChanged(object sender, EventArgs e)
        {
            String RFC_Sugerido = Sugerir_RFC();

            if (RFC_Sugerido.Length > 0)
            {
                Txt_RFC.Text = RFC_Sugerido;
            }
            Txt_CURP.Focus();
        }

    #endregion

        #region (Control Acceso Pagina)
        /// *****************************************************************************************************************************
        /// NOMBRE: Configuracion_Acceso
        /// 
        /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
        /// 
        /// PARÁMETROS: No Áplica.
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// *****************************************************************************************************************************
        protected void Configuracion_Acceso(String URL_Pagina)
        {
            List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
            DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

            try
            {
                //Agregamos los botones a la lista de botones de la página.
                Botones.Add(Btn_Nuevo);
                Botones.Add(Btn_Modificar);
                Botones.Add(Btn_Eliminar);
                Botones.Add(Btn_Buscar_Contribuyente);
                if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
                {
                    if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                    {
                        //Consultamos el menu de la página.
                        Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);
                        if (Dr_Menus.Length > 0)
                        {
                            //Validamos que el menu consultado corresponda a la página a validar.
                            if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                            {
                                Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                            }
                            else
                            {
                                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                            }
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: IsNumeric
        /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
        /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 29/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************

        private Boolean Es_Numero(String Cadena)
        {
            Boolean Resultado = true;
            Char[] Array = Cadena.ToCharArray();
            try
            {
                for (int index = 0; index < Array.Length; index++)
                {
                    if (!Char.IsDigit(Array[index])) return false;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
            }
            return Resultado;
        }

        #endregion

}