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
using Presidencia.Bitacora_Eventos;
using Presidencia.Area_Funcional.Negocio;


public partial class paginas_Paginas_Generales_Frm_Cat_SAP_Area_Funcional_ : System.Web.UI.Page
{
    #region PAGE LOAD / INIT
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Cls_Cat_SAP_Area_Funcional_Negocio Area_Funcional = new Cls_Cat_SAP_Area_Funcional_Negocio();
                Llenar_Grid_Area_Funcional();
                Habilitar_Forma(false);
                Estado_Botones("inicial");
                Limpiar_Formulario();
                Llenar_Combo_Estatus();
                ViewState["SortDirection"] = "DESC";
            }
        }
    #endregion
    #region METODOS
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Area_Funcional
    ///DESCRIPCIÓN: Metodo que llena el GridView
    ///PARAMETROS: GridView que se llenara
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 04/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************            
    public void Llenar_Grid_Area_Funcional()
    {
        DataTable Dt_Area_Funcional = null;//Lista de provedores
        Cls_Cat_SAP_Area_Funcional_Negocio Area_Funcional = new Cls_Cat_SAP_Area_Funcional_Negocio();//Variable de conexion con la capa de negocios.

        if (!Txt_Busqueda.Text.Trim().Equals(""))
        {
            Area_Funcional.P_Clave = Txt_Busqueda.Text.Trim();
        }

        Dt_Area_Funcional = Area_Funcional.Consultar_Area_Funcional().Tables[0];
        Session["Cat_Area_Funcional"] = Dt_Area_Funcional;
        if (Dt_Area_Funcional is DataTable)
        {
            Grid_Areas_Funcionales.Columns[1].Visible = true;
            Grid_Areas_Funcionales.Columns[2].Visible = true;
            Grid_Areas_Funcionales.Columns[3].Visible = true;
            Grid_Areas_Funcionales.DataSource = Dt_Area_Funcional;
            Grid_Areas_Funcionales.DataBind();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_forma
    ///DESCRIPCIÓN: es un metodo generico para habilitar todos los campos de la 
    ///forma que pueden ser editados
    ///PARAMETROS: 
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 26/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Habilitar_Forma(Boolean Estatus)
    {
        Txt_Clave.Enabled = Estatus;
        Txt_Descripcion.Enabled = Estatus;
        Txt_Anio.Enabled = Estatus;
        Cmb_Estatus.Enabled = Estatus;
        Grid_Areas_Funcionales.Enabled = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones
    ///DESCRIPCIÓN: metodo que muestra los botones de acuerdo al estado en el que se encuentre
    ///PARAMETROS:   1.- String Estado: El estado de los botones solo puede tomar 
    ///                 + inicial
    ///                 + nuevo
    ///                 + modificar
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 08/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estado_Botones(String Estado)
    {
        switch (Estado)
        {
            case "inicial":
                //Boton Nuevo
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.Enabled = true;
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                //Boton Modificar
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.Enabled = true;
                Btn_Modificar.Visible = true;
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Eliminar
                Btn_Eliminar.Enabled = true;
                Btn_Eliminar.Visible = true;
                //Boton Salir
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.Enabled = true;
                Btn_Salir.Visible = true;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                Configuracion_Acceso("Frm_Cat_SAP_Area_Funcional.aspx");
                break;
            case "nuevo":
                //Boton Nuevo
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Nuevo.Enabled = true;
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                //Boton Modificar
                Btn_Modificar.Visible = false;
                //Boton Eliminar
                Btn_Eliminar.Visible = false;
                //Boton Salir
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.Enabled = true;
                Btn_Salir.Visible = true;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                break;
            case "modificar":
                //Boton Nuevo
                Btn_Nuevo.Visible = false;
                //Boton Modificar
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.Enabled = true;
                Btn_Modificar.Visible = true;
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                //Boton Eliminar
                Btn_Eliminar.Visible = false;
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
    ///FECHA_CREO: 26/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Limpiar_Formulario()
    {
        Txt_Clave.Text = "";
        Txt_Descripcion.Text = "";
        Txt_Anio.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
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
    private void Llenar_Combo_Estatus()
    {
        Cmb_Estatus.DataBind();
        Cmb_Estatus.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
        Cmb_Estatus.Items.Insert(1, new ListItem("ACTIVO", "ACTIVO"));
        Cmb_Estatus.Items.Insert(2, new ListItem("INACTIVO", "INACTIVO"));
        Cmb_Estatus.SelectedIndex = -1;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Area_Funcional_Agregada
    ///DESCRIPCIÓN          : Validar la clave del area funcional agregada.
    ///PROPIEDADES          
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 26/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    private Boolean Validar_Area_Funcional_Agregada()
    {
        Boolean Registro_Valido = true;//Variable que almacenara si el registro a insertar corresponde a un registro válido 
        String Clave = "";//Identificador de la clave

        try
        {
            for (Int32 Contador_Filas = 0; Contador_Filas < Grid_Areas_Funcionales.Rows.Count; Contador_Filas++) //recorre el grid para comparar las claves
            {
                Clave = Grid_Areas_Funcionales.Rows[Contador_Filas].Cells[1].Text.Trim();
                if (Txt_Clave.Text.Trim().Equals(Clave))
                {
                    Registro_Valido = false;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar que no se den alta claves iguales. Error: [" + Ex.Message + "]");
        }
        return Registro_Valido;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Area_Funcional_Modificada
    ///DESCRIPCIÓN          : Validar la clave del area funcional modificada.
    ///PROPIEDADES          : 1. Clave del área seleccionada
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 28/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    private Boolean Validar_Area_Funcional_Modificada(String Clave_Actual)
    { 
        Boolean Registro_Valido = true;//Variable que almacenara si el registro a insertar corresponde a un registro válido 
        String Clave = "";//Identificador de la clave a comparar
        String Clave_Modificada = ""; // Identificador de la clave modificada

        try
        {
            Clave_Modificada = Txt_Clave.Text.Trim();
            //Se compararn las clavez para ver si se hiso algun cambio
            if (Clave_Actual.Equals(Clave_Modificada))
            {
                Registro_Valido = true;
            }
            else //si se modifico la clave busca que la nueva clave no sea igual a las de mas claves 
            {
                for (Int32 Contador_Filas = 0; Contador_Filas < Grid_Areas_Funcionales.Rows.Count; Contador_Filas++)
                {
                    Clave = Grid_Areas_Funcionales.Rows[Contador_Filas].Cells[1].Text.Trim();
                    if (Clave_Modificada.Equals(Clave))
                    {
                        Registro_Valido = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar que no se den alta claves iguales. Error: [" + Ex.Message + "]");
        }
        return Registro_Valido;
    }
    #endregion
    #region EVENTOS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_proveedor_Click
        ///DESCRIPCIÓN: Evento del boton Buscar 
        ///PARAMETROS:    
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Img_buscar_Click(object sender, ImageClickEventArgs e)
        {
            Llenar_Grid_Area_Funcional();
            Estado_Botones("inicial");
            Habilitar_Forma(false);
            Limpiar_Formulario();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Evento del Boton salir 
        ///PARAMETROS:   
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 26/Febrero/2011  
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e)
        {
            Cls_Cat_SAP_Area_Funcional_Negocio Area_Funcional = new Cls_Cat_SAP_Area_Funcional_Negocio(); //Variable de conexion con la capa de negocios.
            switch (Btn_Salir.ToolTip)
            {
                case "Cancelar":
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Estado_Botones("inicial");
                    Limpiar_Formulario();
                    Habilitar_Forma(false);
                    Llenar_Grid_Area_Funcional();
                    break;

                case "Inicio":
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    break;
            }//fin del switch
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Evento del boton Eliminar 
        ///PARAMETROS:    
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 26/Febrero/2011  
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e)
        {
            Cls_Cat_SAP_Area_Funcional_Negocio Area_Funcional = new Cls_Cat_SAP_Area_Funcional_Negocio();//Variable de conexion con la capa de negocios.
            if (Grid_Areas_Funcionales.SelectedIndex > (-1))
            {
                GridViewRow selectedRow = Grid_Areas_Funcionales.Rows[Grid_Areas_Funcionales.SelectedIndex];
                String clave = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString().Trim();
                Area_Funcional.P_Clave = clave;

                if (Area_Funcional.Eliminar_Area_Funcional())
                {
                    Estado_Botones("inicial");
                    Llenar_Grid_Area_Funcional();
                    Habilitar_Forma(false);
                    //Limpiar_Formulario();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Eliminar Area Funcional", "alert('Operacion Completa: El estatus cambio a INACTIVO');", true);
                    Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Baja, "Frm_Cat_SAP_Area_Funcional.aspx", Area_Funcional.P_Clave, "");
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text += " +  Favor de seleccionar un área. <br />";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Boton que tiene la funcion de insertar un elemento en la tabla Cat_SAP_Area_Funcional
        ///PARAMETROS:   
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e)
        {
            Cls_Cat_SAP_Area_Funcional_Negocio Area_Funcional = new Cls_Cat_SAP_Area_Funcional_Negocio();
            //Validacion para crear un nuevo registro y para habilitar los controles que se requieran
            switch (Btn_Nuevo.ToolTip)
            {
                case "Nuevo":
                    Estado_Botones("nuevo");
                    Limpiar_Formulario();
                    Habilitar_Forma(true);
                    Cmb_Estatus.Enabled = false;
                    Cmb_Estatus.SelectedIndex = 1;
                    break;
                case "Dar de Alta":
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    if(Txt_Clave.Text.Equals("") || Txt_Descripcion.Text.Equals(""))
                    {
                        if(Txt_Clave.Text.Equals(""))
                            Lbl_Mensaje_Error.Text += "+  Favor de introducir la clave del área. <br />";
                        else
                            Lbl_Mensaje_Error.Text += "+  Favor de introducir la descripción del área. <br />";
                       
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                    else
                    {
                        if (Validar_Area_Funcional_Agregada())
                        {
                            Area_Funcional.P_Clave = Txt_Clave.Text.Trim();
                            Area_Funcional.P_Descripcion = Txt_Descripcion.Text;
                            Area_Funcional.P_Estatus = Cmb_Estatus.SelectedValue;
                            Area_Funcional.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                            Area_Funcional.P_Area_Funcional_ID = Area_Funcional.Generar_ID().ToString();
                            Area_Funcional.P_Anio = Txt_Anio.Text;

                            if (Area_Funcional.Alta_Area_Funcional())
                            {
                                //cargamos los datos a la clase de negocio
                                Estado_Botones("inicial");
                                Llenar_Grid_Area_Funcional();
                                Habilitar_Forma(false);
                                Limpiar_Formulario();

                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Alta Area Funcional", "alert('Operacion Completa');", true);
                                //Registramos la accion en la bitacora
                                Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Cat_SAP_Area_Funcional.aspx", Area_Funcional.P_Area_Funcional_ID, "");
                            }
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Text += "+  El registro a insertar ya esta dado de alta. <br />";
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                        }
                    } 
                    break;
            }//fin del swirch
        }//fin del boton Nuevo
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Evento del boton de modificar
        ///PARAMETROS:    
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 09/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e)
        {
            Cls_Cat_SAP_Area_Funcional_Negocio Area_Funcional = new Cls_Cat_SAP_Area_Funcional_Negocio();//Variable de conexion con la capa de negocios.
            if (Grid_Areas_Funcionales.SelectedIndex > (-1))
            {
                switch (Btn_Modificar.ToolTip)
                {
                    //Validacion para actualizar un registro y para habilitar los controles que se requieran
                    case "Modificar":
                        Estado_Botones("modificar");
                        Habilitar_Forma(true);
                        Llenar_Grid_Area_Funcional();
                        break;
                    case "Actualizar":
                        Lbl_Mensaje_Error.Text = "";
                        Lbl_Mensaje_Error.Visible = false;
                        Img_Error.Visible = false;

                        //Obtengo el dataset antes de modificar
                        DataSet Datos_No_Modificados = Area_Funcional.Consultar_Area_Funcional();
                        GridViewRow selectedRow = Grid_Areas_Funcionales.Rows[Grid_Areas_Funcionales.SelectedIndex];
                        String clave = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString().Trim();

                        if (Txt_Clave.Text.Equals("") || Txt_Descripcion.Text.Equals(""))
                        {
                            if (Txt_Clave.Text.Equals(""))
                                Lbl_Mensaje_Error.Text += "+  Favor de introducir la clave del área. <br />";
                            else if (Txt_Descripcion.Text.Equals(""))
                                Lbl_Mensaje_Error.Text += "+  Favor de introducir la descripción del área. <br />";

                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                        }
                        else
                        {
                            if (Validar_Area_Funcional_Modificada(clave))
                            {
                                Area_Funcional.P_Clave = Txt_Clave.Text.Trim();
                                Area_Funcional.P_Descripcion = Txt_Descripcion.Text;
                                Area_Funcional.P_Estatus = Cmb_Estatus.SelectedValue;
                                Area_Funcional.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                                Area_Funcional.P_Anio = Txt_Anio.Text;
                                if (Area_Funcional.Modificar_Area_Funcional(clave))
                                {
                                    //Obtengo otro dataset con los datos ya modificados 
                                    DataSet Datos_Modificados = Area_Funcional.Consultar_Area_Funcional();
                                    //Genero la descripcion de las modificaciones realizadas 
                                    String Descripcion_Bitacora = Cls_Bitacora.Revisar_Actualizaciones(Datos_No_Modificados, Datos_Modificados);
                                    Habilitar_Forma(false);
                                    Estado_Botones("inicial");
                                    //Registro la accf ion de modificar en la bitacora 
                                    Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Cat_SAP_Area_Funcional.aspx", Area_Funcional.P_Area_Funcional_ID, Descripcion_Bitacora);
                                    Llenar_Grid_Area_Funcional();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Actualizar Area Funcional", "alert('Operacion Completa');", true);
                                }
                            }
                            else
                            {
                                Lbl_Mensaje_Error.Text += "+  La clave a modificar ya esta dada de alta. <br />";
                                Lbl_Mensaje_Error.Visible = true;
                                Img_Error.Visible = true;
                            }
                        }
                        break;
                }//fin del switch
            }
            else
            {
                Lbl_Mensaje_Error.Text += " +  Favor de seleccionar un área. <br />";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }//fin de Modificar
    #endregion

    #region GRID
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Areas_Funcionales_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo para cargar los datos del elemento seleccionado
    ///PARAMETROS:   
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 26/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Areas_Funcionales_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_SAP_Area_Funcional_Negocio Area_Funcional = new Cls_Cat_SAP_Area_Funcional_Negocio();
        
        DataTable Dt_Area_Funcional = new DataTable();
        if (Grid_Areas_Funcionales.SelectedIndex > (-1))
        {
            Limpiar_Formulario();
            GridViewRow selectedRow = Grid_Areas_Funcionales.Rows[Grid_Areas_Funcionales.SelectedIndex];

            String Clave = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
            String Descripcion = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
            String Estatus = HttpUtility.HtmlDecode(selectedRow.Cells[3].Text).ToString();
            String Anio = HttpUtility.HtmlDecode(selectedRow.Cells[4].Text).ToString();
            Area_Funcional.P_Clave = Clave;
            
            Txt_Clave.Text = Clave;
            Txt_Descripcion.Text = Descripcion;
            Txt_Anio.Text = Anio;
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Estatus));
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Estatus));

            Grid_Areas_Funcionales.Columns[3].Visible = false;
           
            Estado_Botones("Inicial");
            Btn_Eliminar.Enabled = true;
            Btn_Modificar.Enabled = true;
            Habilitar_Forma(false);
            Llenar_Grid_Area_Funcional();
            System.Threading.Thread.Sleep(1000);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Areas_Funcionales_PageIndexChanging
    ///DESCRIPCIÓN: Metodo para manejar la paginacion del Grid_Area_Funcional
    ///PARAMETROS:   
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 26/Febrero/2011  
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Areas_Funcionales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Areas_Funcionales.PageIndex = e.NewPageIndex;
        Llenar_Grid_Area_Funcional();
    }
    #endregion}

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
            Botones.Add(Btn_Img_buscar);

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

    #region ORDENAR GRIDS
    /// ******************************************************************************************
    /// NOMBRE: Grid_Requisiciones_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// ******************************************************************************************

    protected void Grid_Areas_Funcionales_Sorting(object sender, GridViewSortEventArgs e)
    {
        Grid_Sorting(Grid_Areas_Funcionales, ((DataTable)Session["Cat_Area_Funcional"]), e);
    }
    /// *****************************************************************************************
    /// NOMBRE: Grid_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************
    private void Grid_Sorting(GridView Grid, DataTable Dt_Table, GridViewSortEventArgs e)
    {
        if (Dt_Table != null)
        {
            DataView Dv_Vista = new DataView(Dt_Table);
            String Orden = ViewState["SortDirection"].ToString();
            if (Orden.Equals("ASC"))
            {
                Dv_Vista.Sort = e.SortExpression + " DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Vista.Sort = e.SortExpression + " ASC";
                ViewState["SortDirection"] = "ASC";
            }
            Grid.DataSource = Dv_Vista;
            Grid.DataBind();
        }
    }
    #endregion
}
