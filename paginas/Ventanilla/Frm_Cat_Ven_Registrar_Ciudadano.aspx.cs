using System;
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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using AjaxControlToolkit;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Globalization;
using System.Text.RegularExpressions;
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Datos;
using System.Text;
using Presidencia.Ayudante_Curp_Rfc;


public partial class paginas_Ventanilla_Frm_Cat_Ven_Registrar_Ciudadano : System.Web.UI.Page
{
    #region (Page Load)

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Page_Load
        ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
        ///PARAMETROS           : sender y e
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 31-Mayo-2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Inicializar_Controles();
                }
                else
                {
                    Txt_Registrar_Password.Attributes.Add("Value", Txt_Registrar_Password.Text);
                }
                string Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Calle.Attributes.Add("onclick", Ventana_Modal);
                Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Colonia.Attributes.Add("onclick", Ventana_Modal);
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }

    #endregion

    #region Metodos generales

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Inicializar_Controles
        ///DESCRIPCIÓN          : Se incializaran los controles
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 31-Mayo-2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Inicializar_Controles()
        {
            try
            {
                Limpiar_Controles();
                Habilitar_Controles("Nuevo");

                Cmb_Anio.Items.Clear();
                Cmb_Mes.SelectedIndex = 0;
                for (int Cont_Anios = 1900; Cont_Anios < 2014; Cont_Anios++)
                    Cmb_Anio.Items.Add(new ListItem(Cont_Anios.ToString(), Cont_Anios.ToString()));

                Validar_Fecha();
            }
            catch (Exception ex)
            {
                throw new Exception("Inicializar_Controles " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Mostrar_Error
        ///DESCRIPCIÓN          : se habilitan los mensajes de error
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 31-Mayo-2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Mostrar_Error(Boolean Accion)
        {
            try
            {
                Img_Error.Visible = Accion;
                Lbl_Mensaje_Error.Visible = Accion;
            }
            catch (Exception ex)
            {
                throw new Exception("Inicializar_Controles " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Limpiar_Controles
        ///DESCRIPCIÓN          : Se limpiaran los controles
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 31-Mayo-2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Limpiar_Controles()
        {
            try
            {
                Mostrar_Error(false);

                Txt_Apellido_Materno.Text = "";
                Txt_Apellido_Paterno.Text = "";
                //Txt_Ciudad.Text = "IRAPUATO";
                Txt_Confirmar_Email.Text = "";
                Txt_CP.Text = "";
                Txt_Curp.Text = "";
                Txt_Edad.Text = "";
                Txt_Email.Text = "";
                //Txt_Estado.Text = "GUANAJUATO";
                //Txt_Fecha_Nacimiento.Text = "";
                Txt_Nombre.Text = "";
                Txt_Numero_Calle.Text = "";
                Txt_Registrar_Password.Text = "";
                Txt_Respuesta_Secreta.Text = "";
                Txt_Rfc.Text = "";
                Txt_Telefono_Casa.Text = "";
                Txt_Telefono_Celular.Text = "";

                Cargar_Combo_Calles_Colonias();
                Cargar_Combo_Calles(new DataTable());
                Cargar_Combo_Estado();
                Cargar_Combo_Ciudades(new DataTable());

                Cmb_Pregunta_Secreta.SelectedIndex = 0;
                Cmb_Sexo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Limpia_Controles " + ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Habilitar_Controles
        ///DESCRIPCIÓN          : Se habilitaran los controles
        ///PARAMETROS           : 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 31-Mayo-2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Habilitar_Controles(String Operacion)
        {
            Boolean Habilitado; ///Indica si el control de la forma va hacer habilitado para utilización del usuario
            try
            {
                Habilitado = false;
                switch (Operacion)
                {
                    case "Inicial":
                        Habilitado = false;
                        Btn_Nuevo.ToolTip = "Nuevo";
                        Btn_Salir.ToolTip = "Salir";
                        Btn_Nuevo.Visible = true;
                        Btn_Nuevo.CausesValidation = false;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
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
                }
                Txt_Apellido_Materno.Enabled = Habilitado;
                Txt_Apellido_Paterno.Enabled = Habilitado;
                Cmb_Ciudad.Enabled = Habilitado;
                Txt_Confirmar_Email.Enabled = Habilitado;
                Txt_CP.Enabled = Habilitado;
                Txt_Curp.Enabled = Habilitado;
                Txt_Edad.Enabled = Habilitado;
                Txt_Email.Enabled = Habilitado;
                Cmb_Estado.Enabled = Habilitado;
                Txt_Nombre.Enabled = Habilitado;
                Txt_Numero_Calle.Enabled = Habilitado;
                Txt_Registrar_Password.Enabled = Habilitado;
                Txt_Respuesta_Secreta.Enabled = Habilitado;
                Txt_Rfc.Enabled = Habilitado;
                Txt_Telefono_Casa.Enabled = Habilitado;
                Txt_Telefono_Celular.Enabled = Habilitado;
                //Txt_Fecha_Nacimiento.Enabled = Habilitado;

                Cmb_Calle.Enabled = Habilitado;
                Cmb_Colonias.Enabled = Habilitado;
                Cmb_Pregunta_Secreta.Enabled = Habilitado;
                Cmb_Sexo.Enabled = Habilitado;
                //Btn_Fecha_Nacimiento.Enabled = Habilitado;

                Btn_Buscar_Calle.Enabled = Habilitado;
                Btn_Buscar_Colonia.Enabled = Habilitado;

            }
            catch (Exception ex)
            {
                throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Calles_Colonias
        ///DESCRIPCIÓN: cargara la informacion de las calles y colonias
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cargar_Combo_Calles_Colonias()
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            DataTable Dt_Calles = new DataTable();
            DataTable Dt_Colonias = new DataTable();
            try
            {
                //  para las calles
                Dt_Calles = Negocio_Consulta.Consultar_Calles();
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
        ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Estado
        ///DESCRIPCIÓN: cargara la informacion de los estados de la republica
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cargar_Combo_Estado()
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            DataTable Dt_Estado = new DataTable();
            try
            {
                //  para las calles
                Dt_Estado = Negocio_Consulta.Consultar_Estados();

                Cmb_Estado.DataSource = Dt_Estado;
                Cmb_Estado.DataValueField = Cat_Pre_Estados.Campo_Estado_ID;
                Cmb_Estado.DataTextField = Cat_Pre_Estados.Campo_Nombre;
                Cmb_Estado.DataBind();
                Cmb_Estado.Items.Insert(0, "< SELECCIONE >");
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
                Cmb_Calle.DataValueField = Cat_Pre_Calles.Campo_Nombre;
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
        ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Calles
        ///DESCRIPCIÓN: cargara la informacion de las ciudades pertenecientes al estado
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  31/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Cargar_Combo_Ciudades(DataTable Dt_Ciudades)
        {
            try
            {
                Cmb_Ciudad.DataSource = Dt_Ciudades;
                Cmb_Ciudad.DataValueField = Cat_Pre_Ciudades.Campo_Nombre;
                Cmb_Ciudad.DataTextField = Cat_Pre_Ciudades.Campo_Nombre;
                Cmb_Ciudad.DataBind();
                Cmb_Ciudad.Items.Insert(0, "< SELECCIONE >");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
       
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Datos
        /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
        /// PARAMETROS: 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 31/Mayo/2012
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
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            DateTime Fecha_Nacimiento;


            if (Txt_Nombre.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el nombre.<br>";
                Datos_Validos = false;
            }
            if (Txt_Apellido_Paterno.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el apellido paterno de quién solicita.<br>";
                Datos_Validos = false;
            }

            if (Txt_Apellido_Materno.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el apellido materno de quién solicita.<br>";
                Datos_Validos = false;
            }

            if (Txt_Rfc.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el rfc.<br>";
                Datos_Validos = false;
            }

            if (Txt_Curp.Text != "" && Txt_Rfc.Text.Length > 9)
            {
                String RFC = Txt_Rfc.Text.Substring(0, 10);
                if (!Txt_Curp.Text.Contains(RFC))
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El CURP y RFC no coinciden.<br>";
                    Datos_Validos = false;
                }
            }

            //if (!DateTime.TryParse(Txt_Fecha_Nacimiento.Text.Trim(), out Fecha_Nacimiento) || Fecha_Nacimiento == DateTime.MinValue)
            //{
            //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la fecha de nacimiento.<br>";
            //    Datos_Validos = false;
            //}

            if (Txt_Edad.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la edad.<br>";
                Datos_Validos = false;
            }

            if (Txt_Email.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el email.<br>";
                Datos_Validos = false;
            }
            else
            {
                if (Txt_Confirmar_Email.Text == "")
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la confirmación del email.<br>";
                    Datos_Validos = false;
                }
                else if (Txt_Confirmar_Email.Text != Txt_Email.Text)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La confirmación y el email no son iguales.<br>";
                    Datos_Validos = false;            
                }
            }

            if (Txt_Registrar_Password.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la contraseña.<br>";
                Datos_Validos = false;
            }
            else
            {
                if (Txt_Registrar_Password.Text.Length < 8)
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*la contraseña debe de tener por lo menos 8 caracteres.<br>";
                    Datos_Validos = false;
                }
            }

            if (Cmb_Pregunta_Secreta.SelectedIndex == 0)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione la pregunta secreta.<br>";
                Datos_Validos = false;
            }
            else
            {
                if (Txt_Respuesta_Secreta.Text == "")
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la respuesta secreta.<br>";
                    Datos_Validos = false;
                }
            }

            
            if (Cmb_Calle.SelectedIndex == 0)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la calle.<br>";
                Datos_Validos = false;
            }
            if (Cmb_Colonias.SelectedIndex == 0)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la colonia..<br>";
                Datos_Validos = false;
            }
            if (Txt_Numero_Calle.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el número..<br>";
                Datos_Validos = false;
            }
            if (Txt_CP.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el código postal.<br>";
                Datos_Validos = false;
            }

            if (Cmb_Estado.SelectedIndex == 0)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el estado.<br>";
                Datos_Validos = false;
            }
            if (Cmb_Ciudad.SelectedIndex == 0)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione la ciudad.<br>";
                Datos_Validos = false;
            }
            if (Txt_Telefono_Casa.Text == "" && Txt_Telefono_Celular.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el número telefonico (celular o de casa).<br>";
                Datos_Validos = false;
            }

            return Datos_Validos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Ciudadano
        ///DESCRIPCIÓN: pasara la informacion a la capa de negocio
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public Boolean Alta_Ciudadano()
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consultar_Existencia = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            DataTable Dt_Usuarios = new DataTable();
            String Nombre = "";
            String Email = "";
            String Password = "";
            Boolean Accion_Exitosa = false;
            DateTime Fecha_Nacimiento;

            try
            {
                //  para el nombre del ciudadano
                Negocio.P_Nombre = Txt_Nombre.Text.Trim().ToUpper();
                Negocio.P_Nombre_Completo = Txt_Nombre.Text.Trim().ToUpper() + " ";
                Negocio.P_Apellido_Paterno = Txt_Apellido_Paterno.Text.Trim().ToUpper();
                Negocio.P_Nombre_Completo += Txt_Apellido_Paterno.Text.Trim().ToUpper() + " ";
                Negocio.P_Apellido_Materno = Txt_Apellido_Materno.Text.Trim().ToUpper();
                Negocio.P_Nombre_Completo += Txt_Apellido_Materno.Text.Trim().ToUpper();

                //  rfc y curp
                Negocio.P_Curp = Txt_Curp.Text.Trim().ToUpper();
                Negocio.P_Rfc = Txt_Rfc.Text.Trim().ToUpper();

                //  para la fecha de nacimiento y edad
                DateTime.TryParse(Cmb_Dia.SelectedValue.ToString() + "/" + Cmb_Mes.SelectedIndex.ToString() + "/" + Cmb_Anio.SelectedValue.ToString(), out Fecha_Nacimiento);
                Negocio.P_Fecha_Nacimiento = Fecha_Nacimiento.ToString("dd/MM/yyyy");
                Negocio.P_Edad = Txt_Edad.Text.Trim().ToUpper();

                //  para el sexo
                Negocio.P_Sexo = Cmb_Sexo.SelectedValue;

                //  para el email
                Negocio.P_Email = Txt_Email.Text.Trim();

                //  para la contraseña
                Negocio.P_Password = Txt_Registrar_Password.Text;

                //  para los datos de la pregunta secreta
                Negocio.P_Pregunta_Secreta = Cmb_Pregunta_Secreta.SelectedValue;
                Negocio.P_Respuesta_Secreta = Txt_Respuesta_Secreta.Text.Trim();

                //  para la calle, colonia y codigo postal
                Negocio.P_Calle = Cmb_Calle.SelectedItem.ToString() + " " + Txt_Numero_Calle.Text;
                Negocio.P_Colonia = Cmb_Colonias.SelectedItem.ToString();
                Negocio.P_Colonia_ID = Cmb_Colonias.SelectedValue;
                Negocio.P_Calle_ID = Cmb_Calle.SelectedValue;
                Negocio.P_Codigo_Postal = Txt_CP.Text.Trim();

                //  para la ciudad y estado
                Negocio.P_Ciudad = Cmb_Ciudad.SelectedValue;
                Negocio.P_Estado = Cmb_Estado.SelectedItem.ToString();

                //  para los telefonos
                Negocio.P_Telefono_Casa = Txt_Telefono_Casa.Text.Trim().ToUpper();
                Negocio.P_Celular = Txt_Telefono_Celular.Text.Trim().ToUpper();

                //  para el estatus y comentarios
                Negocio.P_Estatus = "ACTIVO";
                Negocio.P_Comentarios = String.Empty;

                //  se consulta para ver si ya esta dado de alta
                Negocio_Consultar_Existencia.P_Email = Txt_Email.Text;
                Dt_Usuarios = Negocio_Consultar_Existencia.Consultar_Usuarios();
                if (Dt_Usuarios != null && Dt_Usuarios.Rows.Count > 0)
                {

                    Nombre = Dt_Usuarios.Rows[0][Cat_Ven_Usuarios.Campo_Nombre_Completo].ToString().Trim();
                    Email = Dt_Usuarios.Rows[0][Cat_Ven_Usuarios.Campo_Email].ToString().Trim();
                    Password = Dt_Usuarios.Rows[0][Cat_Ven_Usuarios.Campo_Password].ToString().Trim();
                    Negocio.Enviar_Correo(Email, Password, Nombre);
                }
                //  se da de alta
                else
                {
                    Negocio.Guardar_Usuario();
                    Accion_Exitosa = true;
                }
                return Accion_Exitosa;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Fecha
        ///DESCRIPCIÓN         : Valida que la fecha seleccionada en los combos sea correcta
        ///PARAMETROS          : 
        ///CREO                : Salvador Vazquez Camacho
        ///FECHA_CREO          : 17/Octubre/2012
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  : 
        ///*******************************************************************************
        public void Validar_Fecha()
        {
            Int16 Anio = Convert.ToInt16(Cmb_Anio.SelectedValue.ToString());
            Int16 Mes = Convert.ToInt16(Cmb_Mes.SelectedValue.ToString());
            int Dias = 0;
            Boolean bisiesto = false;

            // Verificar si el año seleccionado es bisiesto
            if (Anio % 4 == 0 && (Anio % 100 != 0 || Anio % 400 == 0))
                bisiesto = true;

            switch (Mes)
            {
                case 1: Dias = 32;
                    break;
                case 2: Dias = bisiesto ? 30 : 29;
                    break;
                case 3: Dias = 32;
                    break;
                case 4: Dias = 31;
                    break;
                case 5: Dias = 32;
                    break;
                case 6: Dias = 31;
                    break;
                case 7: Dias = 32;
                    break;
                case 8: Dias = 32;
                    break;
                case 9: Dias = 31;
                    break;
                case 10: Dias = 32;
                    break;
                case 11: Dias = 31;
                    break;
                case 12: Dias = 32;
                    break;
            }
            Cmb_Dia.Items.Clear();
            for (int Cont_Dias = 1; Cont_Dias < Dias; Cont_Dias++)
            {
                Cmb_Dia.Items.Add(new ListItem(Cont_Dias.ToString(), String.Format("{0:00}", Cont_Dias)));
            }

            if (!String.IsNullOrEmpty(Txt_Nombre.Text) && !String.IsNullOrEmpty(Txt_Apellido_Paterno.Text) && !String.IsNullOrEmpty(Txt_Apellido_Materno.Text))
                Sugerir_CURP_RFC();

            DateTime FechaNacimiento = new DateTime(Convert.ToInt16(Cmb_Anio.SelectedValue), Convert.ToInt16(Cmb_Mes.SelectedValue), Convert.ToInt16(Cmb_Dia.SelectedValue));
            int edad = DateTime.Now.Year - FechaNacimiento.Year;

            if (DateTime.Now.Month < FechaNacimiento.Month || (DateTime.Now.Month == FechaNacimiento.Month && DateTime.Now.Day < FechaNacimiento.Day))
                edad--;

            Txt_Edad.Text = edad.ToString();
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN    : Sugerir_CURP_RFC
        ///DESCRIPCIÓN       : Metodo que carga los valores al
        ///PARÁMETROS        :
        ///CREO              : Salvador Vazquez Camacho
        ///FECHA_CREO        : 20/Octubre/2012
        ///MODIFICÓ          : 
        ///FECHA_MODIFICÓ    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        private void Sugerir_CURP_RFC()
        {
            Cls_Ayudante_Curp_Rfc Rs_Curp_Rfc = new Cls_Ayudante_Curp_Rfc();

            try
            {
                Rs_Curp_Rfc.P_Nombre = Txt_Nombre.Text;
                Rs_Curp_Rfc.P_Apellido_Paterno = Txt_Apellido_Paterno.Text;
                Rs_Curp_Rfc.P_Apellido_Materno = Txt_Apellido_Materno.Text;
                Rs_Curp_Rfc.P_Entidad_Federativa = Cmb_Estado_Nacimiento.SelectedItem.Text;
                Rs_Curp_Rfc.P_Sexo = Cmb_Sexo.SelectedItem.Text;
                Rs_Curp_Rfc.P_Fecha_Nacimiento = Convert.ToDateTime(Cmb_Anio.SelectedValue + "/" + Cmb_Mes.SelectedValue + "/" + Cmb_Dia.SelectedValue);
                Rs_Curp_Rfc.Calcular();
                Txt_Curp.Text = Rs_Curp_Rfc.P_CURP;
                Txt_Rfc.Text = Rs_Curp_Rfc.P_RFC;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }
        }

    #endregion

    #region Botones

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Btn_Nuevo_Click
        /// DESCRIPCION : realiza la alta del usuario
        /// PARAMETROS: 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 31/Mayo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            Boolean Accion_Exitosa = false;
            try
            {
                Mostrar_Error(false);

                if (Btn_Nuevo.ToolTip == "Nuevo")
                {
                    Limpiar_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                    Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                }
                else
                {
                    if (Validar_Datos())
                    {
                        Mostrar_Error(false);
                        Accion_Exitosa = Alta_Ciudadano();

                        if (Accion_Exitosa == true)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Nuevo_Click", "alert('Usuario Registrado, Su Usuario y Contraseña fueron enviados a la dirección de correo proporcionada');", true);
                        }
                        else 
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Nuevo_Click", "alert('Su Usuario y Contraseña para accesar al sistema fueron enviados de nuevo a su correo, porque usted ya se encuentra registrado');", true);
     
                        }
                        Inicializar_Controles();
                    }
                    else
                    {
                        Mostrar_Error(true);
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
                            Cmb_Colonias_SelectedIndexChanged(null, null);
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
                if (Btn_Salir.ToolTip == "Salir")
                {
                    Response.Redirect("../Ventanilla/Frm_Apl_Login_Ventanilla.aspx");
                }
                else
                {
                    Inicializar_Controles(); //Habilita los controles para la siguiente operación del usuario en el catálogo
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

    #region Combos

        /////*******************************************************************************
        /////NOMBRE:      Cmb_Colonias_OnSelectedIndexChanged
        /////DESCRIPCIÓN: se cargara la colonia 
        /////PARAMETROS:
        /////CREO:        Hugo Enrique Ramírez Aguilera
        /////FECHA_CREO:  23/Mayo/2012 
        /////MODIFICO:
        /////FECHA_MODIFICO:
        /////CAUSA_MODIFICACIÓN:
        /////*******************************************************************************
        //protected void Cmb_Colonias_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
        //    DataTable Dt_Calles = new DataTable();
        //    DataTable Dt_Colonias = new DataTable();
        //    try
        //    {
        //        Negocio_Consulta.P_Colonia_ID = Cmb_Colonias.SelectedValue;
        //        Dt_Calles = Negocio_Consulta.Consultar_Calles();

        //        Cargar_Combo_Calles(Dt_Calles);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message.ToString());
        //    }
        //}

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Cmb_Colonia_SelectedIndexChanged
        ///DESCRIPCIÓN: Si se selecciona una colonia se actualiza el combo de calles de la colonia seleccionada
        ///PARÁMETROS: NO APLICA
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 16-may-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        protected void Cmb_Colonias_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Obj_Calles = new Cls_Cat_Pre_Calles_Negocio();

            try
            {
                Cmb_Calle.Items.Clear();
                // cargar combo calles si hay una colonia seleccionada
                if (Cmb_Colonias.SelectedIndex > 0)
                {
                    Obj_Calles.P_Colonia_ID = Cmb_Colonias.SelectedValue;
                    Llenar_Combo_Con_DataTable(Cmb_Calle, Obj_Calles.Consultar_Calles(), 0, 5);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE:      Cmb_Estado_OnSelectedIndexChanged
        ///DESCRIPCIÓN: se cargara la colonia 
        ///PARAMETROS:
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  23/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Cmb_Estado_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            DataTable Dt_Ciudades = new DataTable();
            try
            {
                Negocio_Consulta.P_Estado_ID = Cmb_Estado.SelectedValue;
                Dt_Ciudades = Negocio_Consulta.Consultar_Ciudades();

                Cargar_Combo_Ciudades(Dt_Ciudades);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN    : Cmb_Anio_SelectedIndexChanged
        ///DESCRIPCIÓN       : Evento que se desencadena al cambiar la seleccion del año y hace el calculo del
        ///                    RFC y CURP con la nueva fecha.
        ///PARÁMETROS        :
        ///CREO              : Salvador Vazquez Camacho
        ///FECHA_CREO        : 20/Octubre/2012
        ///MODIFICÓ          : 
        ///FECHA_MODIFICÓ    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        protected void Cmb_Anio_SelectedIndexChanged(object sender, EventArgs e)
        {
            Validar_Fecha();
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN    : Cmb_Mes_SelectedIndexChanged
        ///DESCRIPCIÓN       : Evento que se desencadena al cambiar la seleccion del mes y hace el calculo del
        ///                    RFC y CURP con la nueva fecha.
        ///PARÁMETROS        :
        ///CREO              : Salvador Vazquez Camacho
        ///FECHA_CREO        : 20/Octubre/2012
        ///MODIFICÓ          : 
        ///FECHA_MODIFICÓ    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        protected void Cmb_Mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Validar_Fecha();
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN    : Cmb_Dia_SelectedIndexChanged
        ///DESCRIPCIÓN       : Evento que se desencadena al cambiar la seleccion del día y hace el calculo del
        ///                    RFC y CURP con la nueva fecha.
        ///PARÁMETROS        :
        ///CREO              : Salvador Vazquez Camacho
        ///FECHA_CREO        : 20/Octubre/2012
        ///MODIFICÓ          : 
        ///FECHA_MODIFICÓ    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        protected void Cmb_Dia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Txt_Apellido_Paterno.Text) && !String.IsNullOrEmpty(Txt_Apellido_Materno.Text) && !String.IsNullOrEmpty(Txt_Nombre.Text))
                Sugerir_CURP_RFC();

            DateTime FechaNacimiento = new DateTime(Convert.ToInt16(Cmb_Anio.SelectedValue), Convert.ToInt16(Cmb_Mes.SelectedValue), Convert.ToInt16(Cmb_Dia.SelectedValue));
            int edad = DateTime.Now.Year - FechaNacimiento.Year;

            if (DateTime.Now.Month < FechaNacimiento.Month || (DateTime.Now.Month == FechaNacimiento.Month && DateTime.Now.Day < FechaNacimiento.Day))
                edad--;

            Txt_Edad.Text = edad.ToString();
        }

    #endregion

}

public class CURP_RFC
{
    public String General;
    public String RFC;
    public String CURP;
    public String Nombre;
    public String Apellido_Paterno;
    public String Apellido_Materno;
    public String Entidad_Federativa;
    public String Sexo;
    public String Fecha_Nacimiento;

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN    : Calcular
    ///DESCRIPCIÓN       : Calcula el RFC y CURP
    ///PARÁMETROS        :
    ///CREO              : Salvador Vazquez Camacho
    ///FECHA_CREO        : 20/Octubre/2012
    ///MODIFICÓ          : 
    ///FECHA_MODIFICÓ    : 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Calcular()
    {
        //Almacenara cada uno de los nombres
        String[] Nombres;

        //Quitamos los acentos
        Nombre = QuitarAcentos(Nombre);
        Apellido_Paterno = QuitarAcentos(Apellido_Paterno);
        Apellido_Materno = QuitarAcentos(Apellido_Materno);

        //Cambiamos todo a mayúsculas
        Nombre = Nombre.ToUpper();
        Apellido_Paterno = Apellido_Paterno.ToUpper();
        Apellido_Materno = Apellido_Materno.ToUpper();
        Sexo = Sexo.ToUpper();
        Entidad_Federativa = Entidad_Federativa.ToUpper();

        //RFC que se regresará
        General = String.Empty;

        //Quitamos los espacios al principio y final del nombre y apellidos
        Nombre.Trim();
        Apellido_Paterno = Apellido_Paterno.Trim();
        Apellido_Materno = Apellido_Materno.Trim();

        //Quitamos los artículos del nombre y apellidos
        Nombre = QuitarArticulos(Nombre);
        Apellido_Paterno = QuitarArticulos(Apellido_Paterno);
        Apellido_Paterno = QuitarArticulos(Apellido_Paterno);

        //Obtenemos cada nombre en una posicion del arreglo
        Nombres = Nombre.Split(' ');

        //Verificamos si son dos o mas nombres
        if (Nombres.Length >= 2)
        {
            // si el nombre es jose, maria o guadalupe, tomar segundo nombre
            if (Nombres[0] == "JOSE" || Nombres[0] == "JOSÉ" ||
                Nombres[0] == "MARIA" || Nombres[0] == "MARÍA" || Nombres[0] == "MA" ||
                Nombres[0] == "GUADALUPE")
            {
                Nombre = Nombres[1];
            }
        }

        //Agregamos el primer caracter del apellido paterno
        General = Apellido_Paterno.Substring(0, 1);

        //Buscamos y agregamos al rfc la primera vocal del primer apellido
        foreach (char c in Apellido_Paterno)
        {
            if (EsVocal(c))
            {
                General += c;
                break;
            }
        }

        //Agregamos el primer caracter del apellido materno
        General += Apellido_Materno.Substring(0, 1);

        //Agregamos el primer caracter del primer nombre
        General += Nombre.Substring(0, 1);

        //agregamos la fecha yymmdd (por ejemplo: 680825, 25 de agosto de 1968 )
        General += Fecha_Nacimiento.Substring(2, 2) + Fecha_Nacimiento.Substring(5, 2) + Fecha_Nacimiento.Substring(8, 2);

        CURP = RFC = General;

        //Le agregamos la homoclave al rfc 
        CalcularHomoclave(Apellido_Paterno + " " + Apellido_Materno + " " + Nombre, Fecha_Nacimiento);

        //Calculamos y agregamos los digitos restantes al CURP
        CalcularCURP();
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN    : CalcularCURP
    ///DESCRIPCIÓN       : Calcula los digitos restantes del CURP
    ///PARÁMETROS        :
    ///CREO              : Salvador Vazquez Camacho
    ///FECHA_CREO        : 20/Octubre/2012
    ///MODIFICÓ          : 
    ///FECHA_MODIFICÓ    : 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void CalcularCURP()
    {
        //Cadena base de caracteres
        String Alfanumerico = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
        //Arreglo que almacena un valor para cada caracter del CURP
        int[] CURP_En_Numero;
        //Variable que almacena la sumatoria
        int Sumatoria = 0;
        //Digito verificador del CURP
        int Digito = 0;
        //Factor para calcular el digito verificador
        int Factor = 19;

        //Se agrega la inicial de acuerdo al Sexo
        if (Sexo == "HOMBRE" || Sexo == "MASCULINO" || Sexo == "H")
            CURP += "H";
        if (Sexo == "MUJER" || Sexo == "FEMENINO" || Sexo == "F")
            CURP += "M";

        //Se evalua el estado y se agrega la clave correspondiente
        switch (Entidad_Federativa)
        {
            case "AGUASCALIENTES":
                CURP += "AS";
                break;
            case "BAJA CALIFORNIA":
                CURP += "BC";
                break;
            case "BAJA CALIFORNIA SUR":
                CURP += "BS";
                break;
            case "CAMPECHE":
                CURP += "CC";
                break;
            case "COAHUILA":
                CURP += "CL";
                break;
            case "COLIMA":
                CURP += "CM";
                break;
            case "CHIAPAS":
                CURP += "CS";
                break;
            case "CHIHUAHUA":
                CURP += "CH";
                break;
            case "DISTRITO FEDERAL":
                CURP += "DF";
                break;
            case "DURANGO":
                CURP += "DG";
                break;
            case "GUANAJUATO":
                CURP += "GT";
                break;
            case "GUERRERO":
                CURP += "GR";
                break;
            case "HIDALGO":
                CURP += "HG";
                break;
            case "JALISCO":
                CURP += "JC";
                break;
            case "MEXICO":
                CURP += "MC";
                break;
            case "MICHOACAN":
                CURP += "MN";
                break;
            case "MORELOS":
                CURP += "MS";
                break;
            case "NAYARIT":
                CURP += "NT";
                break;
            case "NUEVO LEON":
                CURP += "NL";
                break;
            case "OAXACA":
                CURP += "OC";
                break;
            case "PUEBLA":
                CURP += "PL";
                break;
            case "QUERETARO":
                CURP += "QT";
                break;
            case "QUINTANA ROO":
                CURP += "QR";
                break;
            case "SAN LUIS POTOSI":
                CURP += "SP";
                break;
            case "SINALOA":
                CURP += "SL";
                break;
            case "SONORA":
                CURP += "SR";
                break;
            case "TABASCO":
                CURP += "TC";
                break;
            case "TAMAULIPAS":
                CURP += "TS";
                break;
            case "TLAXCALA":
                CURP += "TL";
                break;
            case "VERACRUZ":
                CURP += "VZ";
                break;
            case "YUCATAN":
                CURP += "YN";
                break;
            case "ZACATECAS":
                CURP += "ZS";
                break;
            default:
                CURP += "--";
                break;
        }

        //Encontrar la Primer consonante del apellido paterno
        foreach (Char Caracter in Apellido_Paterno.Substring(1, Apellido_Paterno.Length - 1))
        {
            if (!EsVocal(Caracter))
            {
                CURP += Caracter.ToString();
                break;
            }
        }

        //Encontrar la Primer consonante del apellido materno
        foreach (Char Caracter in Apellido_Materno.Substring(1, Apellido_Materno.Length - 1))
        {
            if (!EsVocal(Caracter))
            {
                CURP += Caracter.ToString();
                break;
            }
        }

        //Encontrar la Primer consonante del Nombre
        foreach (Char Caracter in Nombre.Substring(1, Nombre.Length - 1))
        {
            if (!EsVocal(Caracter))
            {
                CURP += Caracter.ToString();
                break;
            }
        }

        //Asignamos al arreglo el tamaño de la CURP
        CURP_En_Numero = new int[CURP.Length];    

        //Recorremos cada caracter de la CURP
        for (int C_Curp = 0; C_Curp < CURP.Length; C_Curp++)
        {
            //Recorremos cada caracter de la cadena Alfanumerico
            for (int C_Alfanumerico = 0; C_Alfanumerico < Alfanumerico.Length; C_Alfanumerico++)
            {
                //Comparamos cada caracter de la CURP con el del arreglo Alfanumericos
                if (CURP[C_Curp] == Alfanumerico[C_Alfanumerico])
                {
                    //Si son iguales se almacena el valor de la posicion
                    CURP_En_Numero[C_Curp] = C_Alfanumerico;
                    break;
                }
            }
        }

        //Se realiza la sumatoria en base a la formula indicada
        foreach (int C_Sum in CURP_En_Numero)
        {
            Factor--;
            Sumatoria += (Factor * C_Sum);
        }

        //Se calcula el digito verificador
        Digito = 10 - (Sumatoria % 10);

        //Se agrega el digito a la CURP
        CURP += Digito == 10 ? "00" : String.Format("{0:00}", Digito);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN    : CalcularHomoclave
    ///DESCRIPCIÓN       : Calcula la Homoclave del RFC
    ///PARÁMETROS        :
    ///CREO              : Salvador Vazquez Camacho
    ///FECHA_CREO        : 20/Octubre/2012
    ///MODIFICÓ          : 
    ///FECHA_MODIFICÓ    : 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void CalcularHomoclave(string nombreCompleto, string fecha)
    {
        //Guardara el nombre en su correspondiente numérico
        StringBuilder nombreEnNumero = new StringBuilder(); ;
        //La suma de la secuencia de números de nombreEnNumero
        long valorSuma = 0;

        #region Tablas para calcular la homoclave
        //Estas tablas realmente no se porque son como son
        //solo las copie de lo que encontré en internet

        #region TablaRFC 1
        Hashtable tablaRFC1 = new Hashtable();
        tablaRFC1.Add("&", 10);
        tablaRFC1.Add("Ñ", 10);
        tablaRFC1.Add("A", 11);
        tablaRFC1.Add("B", 12);
        tablaRFC1.Add("C", 13);
        tablaRFC1.Add("D", 14);
        tablaRFC1.Add("E", 15);
        tablaRFC1.Add("F", 16);
        tablaRFC1.Add("G", 17);
        tablaRFC1.Add("H", 18);
        tablaRFC1.Add("I", 19);
        tablaRFC1.Add("J", 21);
        tablaRFC1.Add("K", 22);
        tablaRFC1.Add("L", 23);
        tablaRFC1.Add("M", 24);
        tablaRFC1.Add("N", 25);
        tablaRFC1.Add("O", 26);
        tablaRFC1.Add("P", 27);
        tablaRFC1.Add("Q", 28);
        tablaRFC1.Add("R", 29);
        tablaRFC1.Add("S", 32);
        tablaRFC1.Add("T", 33);
        tablaRFC1.Add("U", 34);
        tablaRFC1.Add("V", 35);
        tablaRFC1.Add("W", 36);
        tablaRFC1.Add("X", 37);
        tablaRFC1.Add("Y", 38);
        tablaRFC1.Add("Z", 39);
        tablaRFC1.Add("0", 0);
        tablaRFC1.Add("1", 1);
        tablaRFC1.Add("2", 2);
        tablaRFC1.Add("3", 3);
        tablaRFC1.Add("4", 4);
        tablaRFC1.Add("5", 5);
        tablaRFC1.Add("6", 6);
        tablaRFC1.Add("7", 7);
        tablaRFC1.Add("8", 8);
        tablaRFC1.Add("9", 9);
        #endregion

        #region TablaRFC 2
        Hashtable tablaRFC2 = new Hashtable();
        tablaRFC2.Add(0, "1");
        tablaRFC2.Add(1, "2");
        tablaRFC2.Add(2, "3");
        tablaRFC2.Add(3, "4");
        tablaRFC2.Add(4, "5");
        tablaRFC2.Add(5, "6");
        tablaRFC2.Add(6, "7");
        tablaRFC2.Add(7, "8");
        tablaRFC2.Add(8, "9");
        tablaRFC2.Add(9, "A");
        tablaRFC2.Add(10, "B");
        tablaRFC2.Add(11, "C");
        tablaRFC2.Add(12, "D");
        tablaRFC2.Add(13, "E");
        tablaRFC2.Add(14, "F");
        tablaRFC2.Add(15, "G");
        tablaRFC2.Add(16, "H");
        tablaRFC2.Add(17, "I");
        tablaRFC2.Add(18, "J");
        tablaRFC2.Add(19, "K");
        tablaRFC2.Add(20, "L");
        tablaRFC2.Add(21, "M");
        tablaRFC2.Add(22, "N");
        tablaRFC2.Add(23, "P");
        tablaRFC2.Add(24, "Q");
        tablaRFC2.Add(25, "R");
        tablaRFC2.Add(26, "S");
        tablaRFC2.Add(27, "T");
        tablaRFC2.Add(28, "U");
        tablaRFC2.Add(29, "V");
        tablaRFC2.Add(30, "W");
        tablaRFC2.Add(31, "X");
        tablaRFC2.Add(32, "Y");
        #endregion

        #region TablaRFC 3
        Hashtable tablaRFC3 = new Hashtable();
        tablaRFC3.Add("A", 10);
        tablaRFC3.Add("B", 11);
        tablaRFC3.Add("C", 12);
        tablaRFC3.Add("D", 13);
        tablaRFC3.Add("E", 14);
        tablaRFC3.Add("F", 15);
        tablaRFC3.Add("G", 16);
        tablaRFC3.Add("H", 17);
        tablaRFC3.Add("I", 18);
        tablaRFC3.Add("J", 19);
        tablaRFC3.Add("K", 20);
        tablaRFC3.Add("L", 21);
        tablaRFC3.Add("M", 22);
        tablaRFC3.Add("N", 23);
        tablaRFC3.Add("O", 25);
        tablaRFC3.Add("P", 26);
        tablaRFC3.Add("Q", 27);
        tablaRFC3.Add("R", 28);
        tablaRFC3.Add("S", 29);
        tablaRFC3.Add("T", 30);
        tablaRFC3.Add("U", 31);
        tablaRFC3.Add("V", 32);
        tablaRFC3.Add("W", 33);
        tablaRFC3.Add("X", 34);
        tablaRFC3.Add("Y", 35);
        tablaRFC3.Add("Z", 36);
        tablaRFC3.Add("0", 0);
        tablaRFC3.Add("1", 1);
        tablaRFC3.Add("2", 2);
        tablaRFC3.Add("3", 3);
        tablaRFC3.Add("4", 4);
        tablaRFC3.Add("5", 5);
        tablaRFC3.Add("6", 6);
        tablaRFC3.Add("7", 7);
        tablaRFC3.Add("8", 8);
        tablaRFC3.Add("9", 9);
        tablaRFC3.Add("", 24);
        tablaRFC3.Add(" ", 37);
        #endregion

        #endregion

        //agregamos un cero al inicio de la representación númerica del nombre
        nombreEnNumero.Append("0");

        //Recorremos el nombre y vamos convirtiendo las letras en 
        //su valor numérico
        foreach (char c in nombreCompleto)
        {
            if (tablaRFC1.ContainsKey(c.ToString()))
                nombreEnNumero.Append(tablaRFC1[c.ToString()].ToString());
            else
                nombreEnNumero.Append("00");
        }

        //Calculamos la suma de la secuencia de números 
        //calculados anteriormente
        //la formula es:
        //( (el caracter actual multiplicado por diez)
        //mas el valor del caracter siguiente )
        //(y lo anterior multiplicado por el valor del caracter siguiente)
        for (int i = 0; i < nombreEnNumero.Length - 1; i++)
        {
            valorSuma += ((Convert.ToInt32(nombreEnNumero[i].ToString()) * 10) + Convert.ToInt32(nombreEnNumero[i + 1].ToString())) * Convert.ToInt32(nombreEnNumero[i + 1].ToString());
        }

        //Lo siguiente no se porque se calcula así, es parte del algoritmo.
        //Los magic numbers que aparecen por ahí deben tener algún origen matemático
        //relacionado con el algoritmo al igual que el proceso mismo de calcular el 
        //digito verificador.
        //Por esto no puedo añadir comentarios a lo que sigue, lo hice por acto de fe.

        int div = 0, mod = 0;
        div = Convert.ToInt32(valorSuma) % 1000;
        mod = div % 34;
        div = (div - mod) / 34;

        int indice = 0;
        string hc = String.Empty;  //los dos primeros caracteres de la homoclave
        while (indice <= 1)
        {
            if (tablaRFC2.ContainsKey((indice == 0) ? div : mod))
                hc += tablaRFC2[(indice == 0) ? div : mod];
            else
                hc += "Z";
            indice++;
        }

        //Agregamos al RFC los dos primeros caracteres de la homoclave
        RFC += hc;

        //Aqui empieza el calculo del digito verificador basado en lo que tenemos del RFC
        //En esta parte tampoco conozco el origen matemático del algoritmo como para dar 
        //una explicación del proceso, así que ¡tengamos fe hermanos!.
        int rfcAnumeroSuma = 0, sumaParcial = 0;
        for (int i = 0; i < RFC.Length; i++)
        {
            if (tablaRFC3.ContainsKey(RFC[i].ToString()))
            {
                rfcAnumeroSuma = Convert.ToInt32(tablaRFC3[RFC[i].ToString()]);
                sumaParcial += (rfcAnumeroSuma * (14 - (i + 1)));
            }
        }

        int moduloVerificador = sumaParcial % 11;
        if (moduloVerificador == 0)
            RFC += "0";
        else
        {
            sumaParcial = 11 - moduloVerificador;
            if (sumaParcial == 10)
                RFC += "A";
            else
                RFC += sumaParcial.ToString();
        }

        //en este punto la variable rfc ya debe tener la homoclave.
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN    : EsVocal
    ///DESCRIPCIÓN       : Calcula la Homoclave del RFC
    ///PARÁMETROS        :
    ///CREO              : Salvador Vazquez Camacho
    ///FECHA_CREO        : 20/Octubre/2012
    ///MODIFICÓ          : 
    ///FECHA_MODIFICÓ    : 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    static private bool EsVocal(char letra)
    {
        return letra == 'a' || letra == 'e' || letra == 'i' || letra == 'o' || letra == 'u' ||
               letra == 'á' || letra == 'é' || letra == 'í' || letra == 'ó' || letra == 'ú' ||
               letra == 'A' || letra == 'E' || letra == 'I' || letra == 'O' || letra == 'U' ||
               letra == 'Á' || letra == 'É' || letra == 'Í' || letra == 'Ó' || letra == 'Ú';
    }

    /// ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN    : QuitarArticulos
    ///DESCRIPCIÓN       : Remplaza los artículos comúnes en los apellidos con caracter vacío
    ///PARÁMETROS        : Palabra: Palabra que se le quitaran los artículos
    ///CREO              : Salvador Vazquez Camacho
    ///FECHA_CREO        : 20/Octubre/2012
    ///MODIFICÓ          : 
    ///FECHA_MODIFICÓ    : 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    static private string QuitarArticulos(string Palabra)
    {
        return Palabra.Replace("DE LOS ", String.Empty).Replace("DEL ", String.Empty).Replace("LAS ", String.Empty).Replace("DE ", String.Empty).Replace("LA ", String.Empty).Replace("Y ", String.Empty);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN    : QuitarAcentos
    ///DESCRIPCIÓN       : Remplaza las vocales acentuadas por vocales sin acento
    ///PARÁMETROS        :
    ///CREO              : Salvador Vazquez Camacho
    ///FECHA_CREO        : 20/Octubre/2012
    ///MODIFICÓ          : 
    ///FECHA_MODIFICÓ    : 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    static private string QuitarAcentos(String Palabra)
    {
        return Palabra.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u').Replace('Á', 'A').Replace('É', 'E').Replace('Í', 'I').Replace('Ó', 'O').Replace('Ú', 'U');
    }
}

