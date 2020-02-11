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
using Presidencia.Bitacora_Eventos;
using Presidencia.Montos_Proceso_Compra.Negocio;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Cat_Com_Monto_Proceso_Compra : System.Web.UI.Page
{
    #region VARIABLES
        private Double Compra_Dir_Ini;
        private Double Cotiza_Ini;
        private Double Comit_Ini;
        private Double Licita_Rest_Ini;
        private Double Licita_Pub_Ini;
    #endregion
    #region PAGE LOAD / INIT
    protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Habilitar_Forma(false);
                Deshabilitar_montos_Inicio(false);
                Estado_Botones("inicial");
                Limpiar_Formulario();
                Llenar_Combo_Tipo();
            }
        }
    #endregion
    #region EVENTOS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Evento del Boton salir 
        ///PARAMETROS:   
        ///CREO: Leslie Gonzalez Vazquez
        ///FECHA_CREO: 14/Febrero/2011  
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e)
        {
            switch (Btn_Salir.ToolTip)
            {
                case "Cancelar":
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Estado_Botones("inicial");
                    Limpiar_Formulario();
                    Habilitar_Forma(false);
                    Deshabilitar_montos_Inicio(false);
                    break;
                case "Inicio":
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    break;
            }//fin del switch
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Evento del boton de modificar
        ///PARAMETROS:    
        ///CREO: Leslie Gonzalez Vazquez
        ///FECHA_CREO: 09/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
        {
            if (Cmb_Tipo.SelectedIndex > 0)
            {
                Cls_Cat_Com_Montos_Proceso_Compra_Negocio Monto_Proceso_Compra_Negocio = new Cls_Cat_Com_Montos_Proceso_Compra_Negocio();
                switch (Btn_Modificar.ToolTip)
                {
                    //Validacion para actualizar un registro y para habilitar los controles que se requieran
                    case "Modificar":
                        Estado_Botones("modificar");
                        Habilitar_Forma(true);
                        Deshabilitar_montos_Inicio(false);
                        //Cmb_Tipo.Enabled = true;
                        //consultar base de datos para cargar datos en la cajas de texto
                        break;
                    case "Actualizar":
                        Lbl_Mensaje_Error.Text = "";
                        Lbl_Mensaje_Error.Visible = false;
                        Img_Error.Visible = false;
                        if (Validar_Montos_Proceso_Compra())
                        {
                            Monto_Proceso_Compra_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                            Monto_Proceso_Compra_Negocio.P_Tipo = Cmb_Tipo.SelectedValue.Trim();
                            Monto_Proceso_Compra_Negocio.P_Compra_Directa_Inicio = Txt_Com_Directa_Inicio.Text;
                            Monto_Proceso_Compra_Negocio.P_Compra_Directa_Fin = Txt_Com_Directa_Fin.Text;
                            Monto_Proceso_Compra_Negocio.P_Cotizacion_Inicio = Txt_Cotizacion_Inicio.Text;
                            Monto_Proceso_Compra_Negocio.P_Cotizacion_Fin = Txt_Cotizacion_Fin.Text;
                            Monto_Proceso_Compra_Negocio.P_Comite_Inicio = Txt_Comite_Inicio.Text;
                            Monto_Proceso_Compra_Negocio.P_Comite_Fin = Txt_Comite_Fin.Text;
                            Monto_Proceso_Compra_Negocio.P_Licitacion_Restringida_Inicio = Txt_Lic_Restringida_Inicio.Text;
                            Monto_Proceso_Compra_Negocio.P_Licitacion_Restringida_Fin = Txt_Lic_Restringida_Fin.Text;
                            Monto_Proceso_Compra_Negocio.P_Licitacion_Publica_Inicio = Txt_Lic_Publica_Inicio.Text;
                            Monto_Proceso_Compra_Negocio.P_Licitacion_Publica_Fin = Txt_Lic_Publica_Fin.Text;
                            Monto_Proceso_Compra_Negocio.P_Fondo_Fijo_Fin = Txt_Fondo_Fijo_Fin.Text;
                            Monto_Proceso_Compra_Negocio.P_Fondo_Fijo_Inicio = Txt_Fondo_Fijo_Inicio.Text;
                            if (Monto_Proceso_Compra_Negocio.Modificar_Monto_Proceso_Compra())
                            {
                                Habilitar_Forma(false);
                                Estado_Botones("inicial");
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Modificar Monto Proceso Compra", "alert('Operacion Completa');", true);
                            }
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                        }
                        break;
                }//fin del switch
            }
            else
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione un tipo del combo, por favor! <br>";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }//fin de Modificar
      #endregion     
    #region METODOS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Habilitar_forma
        ///DESCRIPCIÓN: es un metodo generico para habilitar todos los campos de la 
        ///forma que pueden ser editados
        ///PARAMETROS: 
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 14/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Habilitar_Forma(Boolean Estatus)
        {
            Txt_Com_Directa_Fin.Enabled = Estatus;
            Txt_Comite_Fin.Enabled = Estatus;
            Txt_Cotizacion_Fin.Enabled = Estatus;
            Txt_Fondo_Fijo_Fin.Enabled = Estatus;
            Txt_Fondo_Fijo_Inicio.Enabled = Estatus;
            Txt_Lic_Publica_Fin.Enabled = Estatus;
            Txt_Lic_Restringida_Fin.Enabled = Estatus;
            Cmb_Tipo.Enabled = !Estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Deshabilitar_montos_inicio
        ///DESCRIPCIÓN: es un metodo generico para deshabilitar todos los campos de los 
        /// montos de inicio para que no se puedan modificar
        ///PARAMETROS: 
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 24/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Deshabilitar_montos_Inicio(Boolean Estatus)
        {
            Txt_Com_Directa_Inicio.Enabled = Estatus;
            Txt_Comite_Inicio.Enabled = Estatus;
            Txt_Cotizacion_Inicio.Enabled = Estatus;
            Txt_Lic_Publica_Inicio.Enabled = Estatus;
            Txt_Lic_Restringida_Inicio.Enabled = Estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Estado_Botones
        ///DESCRIPCIÓN: metodo que muestra los botones de acuerdo al estado en el que se encuentre
        ///PARAMETROS:   1.- String Estado: El estado de los botones solo puede tomar 
        ///                 + inicial
        ///                 + nuevo
        ///                 + modificar
        ///CREO: Leslie Gonzalez Vazquez
        ///FECHA_CREO: 14/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Estado_Botones(String Estado)
        {
            switch (Estado)
            {
                case "inicial":
                    //Boton Modificar
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Modificar.Enabled = true;
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    //Boton Salir
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Salir.Enabled = true;
                    Btn_Salir.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                    Configuracion_Acceso("Frm_Cat_Com_Monto_Proceso_Compra.aspx");
                    break;
                case "modificar":
                    //Boton Modificar
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Modificar.Enabled = true;
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    //Boton Salir
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.Enabled = true;
                    Btn_Salir.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    break;
            }//fin del switch
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
        ///DESCRIPCIÓN: Limpia los componentes del formulario
        ///PARAMETROS: 
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 14/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Limpiar_Formulario()
        {
            Cmb_Tipo.SelectedIndex = 0;
            Txt_Com_Directa_Fin.Text = "";
            Txt_Com_Directa_Inicio.Text = "";
            Txt_Comite_Fin.Text = "";
            Txt_Comite_Inicio.Text = "";
            Txt_Cotizacion_Fin.Text = "";
            Txt_Cotizacion_Inicio.Text = "";
            Txt_Fondo_Fijo_Fin.Text = "";
            Txt_Fondo_Fijo_Inicio.Text = "";
            Txt_Lic_Publica_Inicio.Text = "";
            Txt_Lic_Publica_Fin.Text = "";
            Txt_Lic_Restringida_Fin.Text = "";
            Txt_Lic_Restringida_Inicio.Text = "";
        }//fin de limpiar formulario

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Tipo
        ///DESCRIPCIÓN          : Llena el Combo de tipos.
        ///PROPIEDADES          
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 14/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************      
        private void Llenar_Combo_Tipo()
        {
            Cmb_Tipo.DataBind();
            Cmb_Tipo.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
            Cmb_Tipo.Items.Insert(1, new ListItem("PRODUCTO", "PRODUCTO"));
            Cmb_Tipo.Items.Insert(2, new ListItem("SERVICIO", "SERVICIO"));
            Cmb_Tipo.SelectedIndex = -1;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Montos_Proceso_Compra
        /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
        /// CREO                 : Leslie González Vázquez
        /// FECHA_CREO           : 15/Febrero/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Validar_Montos_Proceso_Compra()
        {
            Boolean Datos_Validos = true;
            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

            //Validar que este seleccionado un combo
                //if(Cmb_Tipo.SelectedIndex < 0)
                //{
                //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Tipo <br>";
                //    Datos_Validos = false;
                //}
            // Validar que el campo de compra directa inicio no este vacio y se introduzca un numero
            if (Txt_Com_Directa_Inicio.Text.Equals(""))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Compra Directa Inicio <br>";
                Datos_Validos = false;
            }
            // Validar que el campo de compra directa fin no este vacio y se introduzca un numero
                if (Txt_Com_Directa_Fin.Text.Equals(""))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Compra Directa Fin <br>";
                    Datos_Validos = false;
                }
            // Validar que el campo de comité inicio no este vacio y el valor introducido sea un número
                if (Txt_Comite_Inicio.Text.Equals(""))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Comit&eacute; inicio <br>";
                    Datos_Validos = false;
                }
            // Validar que el campo de comité  Fin no este vacio y el valor introducido  sea un número
                if (Txt_Comite_Fin.Text.Equals(""))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Comit&eacute; Fin <br>";
                    Datos_Validos = false;
                }
            // Validar que el campo de Cotización inicio no este vacio y el valor introducido  sea un número
                if (Txt_Cotizacion_Inicio.Text.Equals(""))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Cotizaci&oacute;n Inicio <br>";
                    Datos_Validos = false;
                }
            // Validar que el campo de Cotización Fin no este vacio y el valor introducido  sea un número
                if (Txt_Cotizacion_Fin.Text.Equals(""))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Cotizaci&oacute;n Fin <br>";
                    Datos_Validos = false;
                }
             // Validar que el campo de Licitación Publica Inicio no este vacio y el valor introducido  sea un número
                if (Txt_Lic_Publica_Inicio.Text.Equals(""))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Licita&oacute;n Publica Inicio <br>";
                    Datos_Validos = false;
                }
             // Validar que el campo de Licitación Publica Fin no este vacio y el valor introducido  sea un número
                if (Txt_Lic_Publica_Fin.Text.Equals(""))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Licita&oacute;n Publica Fin <br>";
                    Datos_Validos = false;
                }
            // Validar que el campo de Licitación Restringida Inicio no este vacio y el valor introducido  sea un número
                if (Txt_Lic_Restringida_Inicio.Text.Equals(""))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Licita&oacute;n Restringida Inicio <br>";
                    Datos_Validos = false;
                }
            // Validar que el campo de Licitación Restringida Fin no este vacio y el valor introducido  sea un número
                if (Txt_Lic_Restringida_Fin.Text.Equals(""))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Licita&oacute;n Restringida Fin <br>";
                    Datos_Validos = false;
                }
            // Validar que el campo de Fondo Fijo inicio no este vacio y el valor introducido  sea un número
                if (Txt_Fondo_Fijo_Inicio.Text.Equals(""))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fondo Fijo Inicio <br>";
                    Datos_Validos = false;
                }
             ////Validar que el valor introducido  sea un número
                if (Txt_Fondo_Fijo_Fin.Text.Equals(""))
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fondo Fijo Fin <br>";
                    Datos_Validos = false;
                }
             // Validar las cantidades intrducidas en las cajas de texto entre las de más cantidades
                if (!Validar_Cantidades())
                {
                    Datos_Validos = false;
                }
            return Datos_Validos;
        }

        //*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Cantidades
        /// DESCRIPCION : Valida el que el monto ingresado cumpla con los lineamientos
        ///CREO               : Leslie González Vázquez
        ///FECHA_CREO         : 17/Febrero/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private bool Validar_Cantidades()
        {
            bool Respuesta = false;
            //Convertir el valor string de la caja de texto en un número double, y para que no nos marque error se le pasa un cero cuando este vacia o la cantidad 
            double Fondo_Fijo_Ini = Convert.ToDouble(string.IsNullOrEmpty(Txt_Fondo_Fijo_Inicio.Text.Trim()) ? "0" : Txt_Fondo_Fijo_Inicio.Text.Trim());
            double Fondo_Fijo_Fin = Convert.ToDouble(string.IsNullOrEmpty(Txt_Fondo_Fijo_Fin.Text.Trim()) ? "0" : Txt_Fondo_Fijo_Fin.Text.Trim());
            double Compra_Directa_Ini = Convert.ToDouble(string.IsNullOrEmpty(Txt_Com_Directa_Inicio.Text.Trim()) ? "0" : Txt_Com_Directa_Inicio.Text.Trim());
            double Compra_Directa_Fin = Convert.ToDouble(string.IsNullOrEmpty(Txt_Com_Directa_Fin.Text.Trim()) ? "0" : Txt_Com_Directa_Fin.Text.Trim());
            double Cotizacion_Ini = Convert.ToDouble(string.IsNullOrEmpty(Txt_Cotizacion_Inicio.Text.Trim()) ? "0" : Txt_Cotizacion_Inicio.Text.Trim());
            double Cotizacion_Fin = Convert.ToDouble(string.IsNullOrEmpty(Txt_Cotizacion_Fin.Text.Trim()) ? "0" : Txt_Cotizacion_Fin.Text.Trim());
            double Comite_Ini = Convert.ToDouble(string.IsNullOrEmpty(Txt_Comite_Inicio.Text.Trim()) ? "0" : Txt_Comite_Inicio.Text.Trim());
            double Comite_Fin = Convert.ToDouble(string.IsNullOrEmpty(Txt_Comite_Fin.Text.Trim()) ? "0" : Txt_Comite_Fin.Text.Trim());
            double Licitacion_Rest_Ini = Convert.ToDouble(string.IsNullOrEmpty(Txt_Lic_Restringida_Inicio.Text.Trim()) ? "0" : Txt_Lic_Restringida_Inicio.Text.Trim());
            double Licitacion_Rest_Fin = Convert.ToDouble(string.IsNullOrEmpty(Txt_Lic_Restringida_Fin.Text.Trim()) ? "0" : Txt_Lic_Restringida_Fin.Text.Trim());
            double Licitacion_Pub_Ini = Convert.ToDouble(string.IsNullOrEmpty(Txt_Lic_Publica_Inicio.Text.Trim()) ? "0" : Txt_Lic_Publica_Inicio.Text.Trim());
            double Licitacion_Pub_Fin = Convert.ToDouble(string.IsNullOrEmpty(Txt_Lic_Publica_Fin.Text.Trim()) ? "0" : Txt_Lic_Publica_Fin.Text.Trim());

             //Se hacen las validaciones entre las cantidades introducidas para que cumplan con los limites
            if (Fondo_Fijo_Ini < Fondo_Fijo_Fin)
                if(Compra_Directa_Ini < Compra_Directa_Fin)
                    if(Cotizacion_Ini < Cotizacion_Fin)
                        if(Comite_Ini < Comite_Fin)
                            if(Licitacion_Rest_Ini < Licitacion_Rest_Fin)
                                if (Licitacion_Pub_Ini < Licitacion_Pub_Fin)
                                { Respuesta = true; }
                                else
                                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Licitación Pública Fin debe ser mayor que Licitación Pública Inicio<br>";
                            else
                                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Licitación Restringida Fin debe ser mayor que Licitación Restringida Inicio<br>";
                        else
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Comité Fin debe ser mayor que Comité Inicio<br>";
                    else
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Cotización Fin debe ser mayor que Cotización Inicio<br>";
                else
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Compra Directa Fin debe ser mayor que Compra Directa Inicio<br>";
            else
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fondo Fijo Inicio debe ser menor que Fondo Fijo Fin<br>";

            return Respuesta;
        }
    #endregion
    # region EVENTO COMBO
        //*******************************************************************************
        /// NOMBRE DE LA FUNCION: Cmb_Tipo_SelectedIndexChanged
        /// DESCRIPCION : Cargar datos de los montos de acuedo al tipo de seleccionado
        ///CREO               : Leslie González Vázquez
        ///FECHA_CREO         : 17/Febrero/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Cmb_Tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Cls_Cat_Com_Montos_Proceso_Compra_Negocio Montos_Proceso_Compra_Negocio = new Cls_Cat_Com_Montos_Proceso_Compra_Negocio();//Variable de conexion conla capa de negocios.
            DataTable Dt_Montos_Proceso_Compra_Negocio = null;//Variable que almacena un listado de Montos_Proceso_Compra_Negocio.
            try
            {
                if (Cmb_Tipo.SelectedIndex > 0)
                {
                    Montos_Proceso_Compra_Negocio.P_Tipo = Cmb_Tipo.SelectedValue.Trim();
                    Dt_Montos_Proceso_Compra_Negocio = Montos_Proceso_Compra_Negocio.Consultar_Monto_Proceso_Compra();
                   
                    if (Dt_Montos_Proceso_Compra_Negocio is DataTable)
                    {
                        if (Dt_Montos_Proceso_Compra_Negocio.Rows.Count > 0)
                        {
                            foreach (DataRow Renglon in Dt_Montos_Proceso_Compra_Negocio.Rows)
                            {
                                //validar que la consulta no venga vacia la compra directa inicio
                                if (!string.IsNullOrEmpty(Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Compra_Directa_Ini].ToString()))
                                    Txt_Com_Directa_Inicio.Text = Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Compra_Directa_Ini].ToString();//asignar el valor de la consulta a la caja de texto
                                //validar que la consulta no venga vacia la compra directa fin
                                if(!string.IsNullOrEmpty(Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Compra_Directa_Fin].ToString()))
                                    Txt_Com_Directa_Fin.Text = Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Compra_Directa_Fin].ToString();
                                //validar que la consulta no venga vacia la cotizacion inicio
                                if (!string.IsNullOrEmpty(Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Cotizacion_Ini].ToString()))
                                    Txt_Cotizacion_Inicio.Text = Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Cotizacion_Ini].ToString();
                                //validar que la consulta no venga vacia la cotizacion fin
                                if (!string.IsNullOrEmpty(Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Cotizacion_Fin].ToString()))
                                    Txt_Cotizacion_Fin.Text = Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Cotizacion_Fin].ToString();
                                //validar que la consulta no venga vacia la comite inicio
                                if (!string.IsNullOrEmpty(Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Comite_Ini].ToString()))
                                    Txt_Comite_Inicio.Text = Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Comite_Ini].ToString();
                                //validar que la consulta no venga vacia la comite fin
                                if (!string.IsNullOrEmpty(Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Comite_Fin].ToString()))
                                    Txt_Comite_Fin.Text = Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Comite_Fin].ToString();
                                //validar que la consulta no venga vacia la Licitacion restringida inicio
                                if (!string.IsNullOrEmpty(Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_R_Ini].ToString()))
                                    Txt_Lic_Restringida_Inicio.Text = Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_R_Ini].ToString();
                                //validar que la consulta no venga vacia la Licitacion restringida Fin
                                if (!string.IsNullOrEmpty(Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_R_Fin].ToString()))
                                    Txt_Lic_Restringida_Fin.Text = Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_R_Fin].ToString();
                                //validar que la consulta no venga vacia la Licitacion publica inicio
                                if (!string.IsNullOrEmpty(Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_P_Ini].ToString()))
                                    Txt_Lic_Publica_Inicio.Text = Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_P_Ini].ToString();
                                //validar que la consulta no venga vacia la Licitacion publica fin
                                if (!string.IsNullOrEmpty(Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_P_Fin].ToString()))
                                    Txt_Lic_Publica_Fin.Text = Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Monto_Licitacion_P_Fin].ToString();
                                //validar que la consulta no venga vacia el fondo fijo inicio
                                if (!string.IsNullOrEmpty(Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Fondo_Fijo_Ini].ToString()))
                                    Txt_Fondo_Fijo_Inicio.Text = Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Fondo_Fijo_Ini].ToString();
                                //validar que la consulta no venga vacia el fondo fijo fin
                                if (!string.IsNullOrEmpty(Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Fondo_Fijo_Fin].ToString()))
                                    Txt_Fondo_Fijo_Fin.Text = Renglon[Cat_Com_Monto_Proceso_Compra.Campo_Fondo_Fijo_Fin].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cargar los datos en los controles de la página. Error: [" + Ex.Message + "]");
            }
        }
    #endregion
    #region EVENTOS CAJAS DE TEXTO
        //*******************************************************************************
        /// NOMBRE DE LA FUNCION: Txt_Fondo_Fijo_Fin_TextChanged
        /// DESCRIPCION : Cargar datos de los montos de acuedo al tipo de seleccionado
        ///CREO               : Leslie González Vázquez
        ///FECHA_CREO         : 24/Febrero/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Txt_Fondo_Fijo_Fin_TextChanged(object sender, EventArgs e)
        {
            //Convertir el valor string de la caja de texto en un número double, y para que no nos marque error se le pasa un cero cuando este vacia o la cantidad 
            double Fondo_Fijo_Fin = Convert.ToDouble(string.IsNullOrEmpty(Txt_Fondo_Fijo_Fin.Text.Trim()) ? "0" : Txt_Fondo_Fijo_Fin.Text.Trim());
            Compra_Dir_Ini = (Fondo_Fijo_Fin + 0.01); //se le suma el valor de la caja de texto mas el 0.01
            Txt_Com_Directa_Inicio.Text = Compra_Dir_Ini.ToString(); //muestra el valor de la suma en la caja de texto correspondiente
        }

        //*******************************************************************************
        /// NOMBRE DE LA FUNCION: Txt_Com_Directa_Fin_TextChanged
        /// DESCRIPCION : Cargar datos de los montos de acuedo al tipo de seleccionado
        ///CREO               : Leslie González Vázquez
        ///FECHA_CREO         : 24/Febrero/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Txt_Com_Directa_Fin_TextChanged(object sender, EventArgs e)
        {
            //Convertir el valor string de la caja de texto en un número double, y para que no nos marque error se le pasa un cero cuando este vacia o la cantidad 
            double Compra_Directa_Fin = Convert.ToDouble(string.IsNullOrEmpty(Txt_Com_Directa_Fin.Text.Trim()) ? "0" : Txt_Com_Directa_Fin.Text.Trim());
            Cotiza_Ini = (Compra_Directa_Fin + 0.01); //se le suma el valor de la caja de texto mas el 0.01
            Txt_Cotizacion_Inicio.Text = Cotiza_Ini.ToString(); //muestra el valor de la suma en la caja de texto correspondiente
        }

        //*******************************************************************************
        /// NOMBRE DE LA FUNCION: Txt_Cotizacion_Fin_TextChanged
        /// DESCRIPCION : Cargar datos de los montos de acuedo al tipo de seleccionado
        ///CREO               : Leslie González Vázquez
        ///FECHA_CREO         : 25/Febrero/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Txt_Cotizacion_Fin_TextChanged(object sender, EventArgs e)
        {
            //Convertir el valor string de la caja de texto en un número double, y para que no nos marque error se le pasa un cero cuando este vacia o la cantidad 
            double Cotizacion_Fin = Convert.ToDouble(string.IsNullOrEmpty(Txt_Cotizacion_Fin.Text.Trim()) ? "0" : Txt_Cotizacion_Fin.Text.Trim());
            Comit_Ini = (Cotizacion_Fin + 0.01); //se le suma el valor de la caja de texto mas el 0.01
            Txt_Comite_Inicio.Text = Comit_Ini.ToString(); //muestra el valor de la suma en la caja de texto correspondiente
        }

        //*******************************************************************************
        /// NOMBRE DE LA FUNCION: Txt_Comite_Fin_TextChanged
        /// DESCRIPCION : Cargar datos de los montos de acuedo al tipo de seleccionado
        ///CREO               : Leslie González Vázquez
        ///FECHA_CREO         : 25/Febrero/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Txt_Comite_Fin_TextChanged(object sender, EventArgs e)
        {
            //Convertir el valor string de la caja de texto en un número double, y para que no nos marque error se le pasa un cero cuando este vacia o la cantidad 
            double Comite_Fin = Convert.ToDouble(string.IsNullOrEmpty(Txt_Comite_Fin.Text.Trim()) ? "0" : Txt_Comite_Fin.Text.Trim());
            Licita_Rest_Ini = (Comite_Fin + 0.01); //se le suma el valor de la caja de texto mas el 0.01
            Txt_Lic_Restringida_Inicio.Text = Licita_Rest_Ini.ToString(); //muestra el valor de la suma en la caja de texto correspondiente
        }

        //*******************************************************************************
        /// NOMBRE DE LA FUNCION: Txt_Lic_Restringida_Fin_TextChanged
        /// DESCRIPCION : Cargar datos de los montos de acuedo al tipo de seleccionado
        ///CREO               : Leslie González Vázquez
        ///FECHA_CREO         : 25/Febrero/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Txt_Lic_Restringida_Fin_TextChanged(object sender, EventArgs e)
        {
            //Convertir el valor string de la caja de texto en un número double, y para que no nos marque error se le pasa un cero cuando este vacia o la cantidad 
            double Licitacion_Rest_Fin = Convert.ToDouble(string.IsNullOrEmpty(Txt_Lic_Restringida_Fin.Text.Trim()) ? "0" : Txt_Lic_Restringida_Fin.Text.Trim());
            Licita_Pub_Ini = (Licitacion_Rest_Fin + 0.01); //se le suma el valor de la caja de texto mas el 0.01
            Txt_Lic_Publica_Inicio.Text = Licita_Pub_Ini.ToString(); //muestra el valor de la suma en la caja de texto correspondiente
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
                Botones.Add(Btn_Modificar);

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
