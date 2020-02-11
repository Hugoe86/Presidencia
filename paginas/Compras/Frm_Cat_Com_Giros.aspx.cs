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
using Presidencia.Constantes;
using Presidencia.Catalogo_Compras_Giros.Negocio;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Cat_Com_Giros : System.Web.UI.Page
{
    #region (Variables)
    #endregion

    #region (Page Load)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

                if (!IsPostBack)
                {
                    Estado_Inicial();
                }
                else
                {
                    Lbl_Informacion.Visible = false;
                    Img_Warning.Visible = false;
                    Lbl_Informacion.Text = "";
                }
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Page_Load)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }
    #endregion

    #region (Metodos)
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Mostrar_Informacion
        ///DESCRIPCION:             Habilita o deshabilita la muestra en pantalle del mensaje 
        ///                         de Mostrar_Informacion para el usuario
        ///PARAMETROS:              1.- Condicion, entero para saber si es 1 habilita para que se muestre mensaje si es cero
        ///                         deshabilita para que no se muestre el mensaje
        ///CREO:                    Silvia Morales Portuhondo
        ///FECHA_CREO:              23/Septiembre/2010 
        ///MODIFICO:                Noe Mosqueda Valadez
        ///FECHA_MODIFICO:          22/Octubre/2010 11:38
        ///CAUSA_MODIFICACION:      Agregar try-catch para el manejo de errores  
        ///*******************************************************************************
        private void Mostrar_Informacion(int Condicion)
        {
            try
            {
                if (Condicion == 1)
                {
                    Lbl_Informacion.Enabled = true;
                    Img_Warning.Visible = true;
                }
                else
                {
                    Lbl_Informacion.Text = "";
                    Lbl_Informacion.Enabled = false;
                    Img_Warning.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Enabled = true;
                Img_Warning.Visible = true;
                Lbl_Informacion.Text = "Error: " + ex.ToString();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Estado_Inicial
        ///DESCRIPCION:             Colocar la pagina en un estatus inicial
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              04/Noviembre/2010 17:55
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Estado_Inicial()
        {
            try
            {
                //Eliminar sesion
                Session.Remove("Dt_Giros");
                Limpiar_Controles();
                Habilita_Controles("Inicial");
                Llena_Grid_Giros("", -1);
                Grid_Giros.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Llena_Grid_Giros
        ///DESCRIPCION:             Llenar el grid de los giros de acuerdo a un 
        ///                         criterio de busqueda
        ///PARAMETROS:              1. Busqueda: Cadena de texto que tiene el elemento a buscar
        ///                         2. Pagina: Entero que indica la pagina del grid a visualizar
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              04/Noviembre/2010 16:56
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Llena_Grid_Giros(String Busqueda, int Pagina)
        {
            //Declaracion de Variables
            Cls_Cat_Com_Giros_Negocio Giros_Negocio = new Cls_Cat_Com_Giros_Negocio(); //Variable para la capa de negocios
            DataTable Dt_Giros = new DataTable(); //Tabla para los datos

            try
            {
                //Verificar si hay una busqueda
                if (Busqueda != "")
                {
                    //Verificar el texto de la busqueda
                    if (Busqueda == "Todos")
                        Busqueda = "";

                    //Realizar la consulta
                    Giros_Negocio.P_Nombre = Busqueda;
                    Dt_Giros = Giros_Negocio.Consulta_Giros();
                }
                else
                {
                    //Verificar si hay variable de sesion
                    if (Session["Dt_Giros"] != null)
                        Dt_Giros = (DataTable)Session["Dt_Giros"];
                    else
                    {
                        //Realizar consulta
                        Giros_Negocio.P_Nombre = "";
                        Dt_Giros = Giros_Negocio.Consulta_Giros();
                    }
                }

                //Llenar el grid
                Grid_Giros.DataSource = Dt_Giros;

                //Verificar si hay una pagina
                if (Pagina > -1)
                    Grid_Giros.PageIndex = Pagina;

                Grid_Giros.DataBind();

                //Colocar tabla en variable de sesion
                Session["Dt_Giros"] = Dt_Giros;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Limpiar_Controles
        ///DESCRIPCION:             Limpiar los controles del formulario
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              04/Noviembre/2010 17:11
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Limpiar_Controles()
        {
            try
            {
                //Limpiar los controles
                Txt_Busqueda.Text = "";
                Txt_Comentarios.Text = "";
                Txt_Giro_ID.Text = "";
                Txt_Nombre.Text = "";
                Cmb_Estatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Valida_Datos
        ///DESCRIPCION:             Validar que esten llenos los datos del formulario
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              04/Noviembre/2010 17:29
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private String Valida_Datos()
        {
            //Declaracion de variables
            String Resultado = String.Empty; //Variable para el resultado

            try
            {
                //Verificar si esta asignado el nombre
                if (Txt_Nombre.Text.Trim() == "" || Txt_Nombre.Text.Trim() == String.Empty)
                    Resultado = "Favor de proporcionar el nombre del giro";

                if (Cmb_Estatus.SelectedIndex <= 0)
                    Resultado = "Favor de proporcionar el estatus del giro";

                //Entregar resultado
                return Resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Alta_Giros
        ///DESCRIPCION:             Dar de alta un nuevo giro
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              04/Noviembre/2010 17:34
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Alta_Giros()
        {
            //Declaracion de variables
            Cls_Cat_Com_Giros_Negocio Giros_Negocio = new Cls_Cat_Com_Giros_Negocio(); //Variable para la capa de negocios

            try
            {
                //Asignar propiedades
                Giros_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
                Giros_Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                
                //Verificar si los comentarios son 250 caracteres maximo
                if (Txt_Comentarios.Text.Trim().Length > 250)
                    Giros_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim().Substring(0, 250);
                else
                    Giros_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();

                Giros_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;

                //Dar de alta el giro
                Giros_Negocio.Alta_Giros();
                Estado_Inicial();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Baja_Giros
        ///DESCRIPCION:             Eliminar un Giro existente
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              22/Octubre/2010 16:41
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Baja_Giros()
        {
            //Declaracion de variables
            Cls_Cat_Com_Giros_Negocio Giros_Negocio = new Cls_Cat_Com_Giros_Negocio(); //Variable para la capa de negocios

            try
            {
                //Asignar propiedades
                Giros_Negocio.P_Giro_ID = Txt_Giro_ID.Text.Trim();

                //Eliminar el giro
                Giros_Negocio.Baja_Giros();
                Estado_Inicial();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Cambio_Giros
        ///DESCRIPCION:             Modificar un giro existente
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              04/Noviembre/2010 16:50
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Cambio_Giros()
        {
            //Declaracion de variables
            Cls_Cat_Com_Giros_Negocio Giros_Negocio = new Cls_Cat_Com_Giros_Negocio(); //Variable para la capa de negocios

            try
            {
                //Asignar propiedades
                Giros_Negocio.P_Giro_ID = Txt_Giro_ID.Text.Trim();
                Giros_Negocio.P_Nombre = Txt_Nombre.Text.Trim();
                Giros_Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value;

                //Verificar si los comentarios son 250 caracteres maximo
                if (Txt_Comentarios.Text.Trim().Length > 250)
                    Giros_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim().Substring(0, 250);
                else
                    Giros_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();

                Giros_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;

                //Cambiar el giro
                Giros_Negocio.Cambio_Giros();
                Estado_Inicial();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Llena_Datos_Controles
        ///DESCRIPCION:             Llenar los controles con los datos del giro seleccionado
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              04/Noviembre/2010 17:51
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Llena_Datos_Controles()
        {
            try
            {
                Txt_Giro_ID.Text = Grid_Giros.SelectedRow.Cells[1].Text.Trim();
                Txt_Nombre.Text = Grid_Giros.SelectedRow.Cells[2].Text;

                //Verificar el nombre del estatus
                if (Grid_Giros.SelectedRow.Cells[3].Text.Trim() == "ACTIVO")
                    Cmb_Estatus.SelectedIndex = 1;
                else
                    Cmb_Estatus.SelectedIndex = 0;

                Txt_Comentarios.Text = Grid_Giros.SelectedRow.Cells[4].Text.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Habilita_Controles
        ///DESCRIPCION:             Habilitar los controles del formulario de acuerdo al modo
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              22/Octubre/2010 13:11
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Habilita_Controles(string Modo)
        {
            Boolean Habilitar = false;
            try
            {
                switch (Modo)
                {
                    case "Inicial":
                        Habilitar = false;
                        Btn_Nuevo.Enabled = true;
                        Btn_Modificar.Enabled = true;
                        Btn_Eliminar.Enabled = true;
                        Btn_Nuevo.ImageUrl = "../imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.ImageUrl = "../imagenes/paginas/icono_modificar.png";
                        Btn_Eliminar.ImageUrl = "../imagenes/paginas/icono_eliminar.png";
                        Btn_Salir.ImageUrl = "../imagenes/paginas/icono_salir.png";
                        Btn_Nuevo.ToolTip = "Nuevo";
                        Btn_Modificar.ToolTip = "Modificar";
                        Btn_Eliminar.ToolTip = "Eliminar";
                        Btn_Salir.ToolTip = "Salir";

                        Configuracion_Acceso("Frm_Cat_Com_Giros.aspx");
                        break;

                    case "Nuevo":
                    case "Modificar":
                        Habilitar = true;

                        //Verificar el modo
                        if (Modo == "Nuevo")
                        {
                            Btn_Nuevo.Enabled = true;
                            Btn_Modificar.Enabled = false;
                            Btn_Nuevo.ImageUrl = "../imagenes/paginas/icono_guardar.png";
                            Btn_Modificar.ImageUrl = "../imagenes/paginas/icono_modificar_deshabilitado.png";
                            Btn_Nuevo.ToolTip = "Guardar";
                            Btn_Modificar.ToolTip = "Modificar";
                        }
                        else
                        {
                            Btn_Nuevo.Enabled = false;
                            Btn_Modificar.Enabled = true;
                            Btn_Nuevo.ImageUrl = "../imagenes/paginas/icono_nuevo_deshabilitado.png";
                            Btn_Modificar.ImageUrl = "../imagenes/paginas/icono_guardar.png";
                            Btn_Nuevo.ToolTip = "Nuevo";
                            Btn_Modificar.ToolTip = "Guardar";
                        }

                        Btn_Eliminar.Enabled = false;
                        Btn_Eliminar.ImageUrl = "../imagenes/paginas/icono_eliminar_deshabilitado.png";
                        Btn_Salir.ImageUrl = "../imagenes/paginas/icono_cancelar.png";
                        Btn_Salir.ToolTip = "Cancelar";
                        break;

                    default: break;
                }

                Txt_Nombre.Enabled = Habilitar;
                Cmb_Estatus.Enabled = Habilitar;
                Txt_Comentarios.Enabled = Habilitar;
                Txt_Busqueda.Enabled = !Habilitar;
                Btn_Buscar.Enabled = !Habilitar;
                Grid_Giros.Enabled = !Habilitar;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    #endregion

    #region (Grid)
        protected void Grid_Giros_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Colocar los valores en los controles
                Llena_Datos_Controles();
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Grid_Giros_SelectedIndexChanged)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }

        protected void Grid_Giros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //Llenar el grid con la pagina nueva
                Llena_Grid_Giros("", e.NewPageIndex);
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Grid_Giros_PageIndexChanging)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }
    #endregion

    #region (Eventos)
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            //Declaracion de variables
            String Validacion = String.Empty; //Variable que contiene el resultado de la validacion

            try
            {
                //Verificar el tooltip del boton
                if (Btn_Nuevo.ToolTip == "Nuevo")
                {
                    //Habilitar y limpiar
                    Habilita_Controles("Nuevo");
                    Limpiar_Controles();
                }
                else
                {
                    //Verificar si la validacion es correcta
                    Validacion = Valida_Datos();
                    if (Validacion == "" || Validacion == String.Empty)
                        Alta_Giros();
                    else
                    {
                        Lbl_Informacion.Visible = true;
                        Img_Warning.Visible = true;
                        Lbl_Informacion.Text = Validacion;
                    }
                }
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Btn_Nuevo_Click)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }

        protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
        {
            //Declaracion de variables
            String Validacion = String.Empty; //Variable que contiene el resultado de la validacion

            try
            {
                //Verificar el tooltip del boton
                if (Btn_Modificar.ToolTip == "Modificar")
                    //Verificar si se ha seleccionado un elemento
                    if (Grid_Giros.SelectedIndex > -1)
                        Habilita_Controles("Modificar");
                    else
                    {
                        Lbl_Informacion.Visible = true;
                        Img_Warning.Visible = true;
                        Lbl_Informacion.Text = "Favor de seleccionar el giro a modificar";
                    }
                else
                {
                    //Verificar si se ha seleccionado un elementos
                    if (Grid_Giros.SelectedIndex > -1)
                    {
                        //Verificar si la validacion es correcta
                        Validacion = Valida_Datos();
                        if (Validacion == "" || Validacion == String.Empty)
                            Cambio_Giros();
                        else
                        {
                            Lbl_Informacion.Visible = true;
                            Img_Warning.Visible = true;
                            Lbl_Informacion.Text = Validacion;
                        }
                    }
                    else
                    {
                        Lbl_Informacion.Visible = true;
                        Img_Warning.Visible = true;
                        Lbl_Informacion.Text = "Favor de seleccionar el giro a modificar";
                    }
                }
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Btn_Modificar_Click)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }

        protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
        {
            Cls_Cat_Com_Giros_Negocio Giros_Negocio = new Cls_Cat_Com_Giros_Negocio();
            try
            {
                //Verificar si se ha seleccionado un elemento
                if (Grid_Giros.SelectedIndex > -1)
                    Baja_Giros();
                else
                {
                    Lbl_Informacion.Visible = true;
                    Img_Warning.Visible = true;
                    Lbl_Informacion.Text = "Favor de seleccionar el giro a eliminar";
                }
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Btn_Eliminar_Click)" + ex.ToString();
                Mostrar_Informacion(1);
            }
            Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Baja, "Frm_Cat_Com_Giros.aspx", Giros_Negocio.P_Giro_ID, "");
        }

        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Verificar el mensaje de tooltip del boton
                if (Btn_Salir.ToolTip == "Cancelar")
                    Estado_Inicial();
                else
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Btn_Salir_Click)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }

        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Verificar si esta vacio
                if (Txt_Busqueda.Text.Trim() == "" || Txt_Busqueda.Text.Trim() == String.Empty)
                    Llena_Grid_Giros("Todos", -1);
                else
                    Llena_Grid_Giros(Txt_Busqueda.Text.Trim(), -1);
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Btn_Buscar_Click)" + ex.ToString();
                Mostrar_Informacion(1);
            }
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
                Botones.Add(Btn_Buscar);

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
