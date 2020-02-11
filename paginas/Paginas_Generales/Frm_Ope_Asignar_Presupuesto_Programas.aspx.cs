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
using Presidencia.Asignar_Presupuesto_Programas.Negocio;
using System.Collections.Generic;

public partial class paginas_Paginas_Generales_Frm_Ope_Asignar_Presupuesto_Programas : System.Web.UI.Page
{
    #region PAGE LOAD / INIT
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

        if (!IsPostBack)
        {
            Llenar_Grid_Presupuesto_Programa();
            Habilitar_Forma(false);
            Estado_Botones("inicial");
            Limpiar_Formulario();
            Llenar_Combo_Programa();
        }
    }
    #endregion

    #region METODOS
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Presupuesto_Programa
    ///DESCRIPCIÓN: Metodo que llena el GridView
    ///PARAMETROS: GridView que se llenara
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 01/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************            
    public void Llenar_Grid_Presupuesto_Programa()
    {
        DataTable Dt_Presupuesto_Programa = null;//Lista de presupuestos
        Cls_Ope_Asignar_Presupuesto_Programas_Negocio Presupuesto_Programa = new Cls_Ope_Asignar_Presupuesto_Programas_Negocio();//Variable de conexion con la capa de negocios.

        if (!Txt_Busqueda.Text.Trim().Equals(""))
        {
            Presupuesto_Programa.P_Nombre = Txt_Busqueda.Text.Trim();
            Presupuesto_Programa.P_Clave = Txt_Busqueda.Text.Trim();
        }

        Dt_Presupuesto_Programa = Presupuesto_Programa.Consultar_Presupuesto_Programas().Tables[0];

        if (Dt_Presupuesto_Programa is DataTable)
        {
            Grid_Presupuesto_Programa.Columns[1].Visible = true;
            Grid_Presupuesto_Programa.Columns[2].Visible = true;
            Grid_Presupuesto_Programa.Columns[3].Visible = true;
            Grid_Presupuesto_Programa.Columns[4].Visible = true;
            Grid_Presupuesto_Programa.Columns[5].Visible = true;
            Grid_Presupuesto_Programa.Columns[6].Visible = true;
            Grid_Presupuesto_Programa.Columns[7].Visible = true;
            Grid_Presupuesto_Programa.Columns[8].Visible = true;
            Grid_Presupuesto_Programa.Columns[9].Visible = true;
            Grid_Presupuesto_Programa.Columns[10].Visible = true;
            Grid_Presupuesto_Programa.DataSource = Dt_Presupuesto_Programa;
            Grid_Presupuesto_Programa.DataBind();
            Grid_Presupuesto_Programa.Columns[6].Visible = false;
            Grid_Presupuesto_Programa.Columns[7].Visible = false;
            Grid_Presupuesto_Programa.Columns[8].Visible = false;
            Grid_Presupuesto_Programa.Columns[9].Visible = false;
            Grid_Presupuesto_Programa.Columns[10].Visible = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_forma
    ///DESCRIPCIÓN: es un metodo generico para habilitar todos los campos de la 
    ///forma que pueden ser editados
    ///PARAMETROS: 
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 01/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Habilitar_Forma(Boolean Estatus)
    {
        Txt_Fecha.Enabled = Estatus;
        Cmb_Programa.Enabled = Estatus;
        Txt_Anio.Enabled = Estatus;
        Txt_Presupuesto.Enabled = Estatus;
        Txt_Ejercido.Enabled = Estatus;
        Txt_Disponible.Enabled = Estatus;
        Txt_Comprometido.Enabled = Estatus;
        Grid_Presupuesto_Programa.Enabled = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones
    ///DESCRIPCIÓN: metodo que muestra los botones de acuerdo al estado en el que se encuentre
    ///PARAMETROS:   1.- String Estado: El estado de los botones solo puede tomar 
    ///                 + inicial
    ///                 + nuevo
    ///                 + modificar
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 01/Marzo/2011 
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
                //Boton Salir
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.Enabled = true;
                Btn_Salir.Visible = true;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                Configuracion_Acceso("Frm_Ope_Asignar_Presupuesto_Programas.aspx");
                break;
            case "nuevo":
                //Boton Nuevo
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Nuevo.Enabled = true;
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                //Boton Modificar
                Btn_Modificar.Visible = false;
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
    ///FECHA_CREO: 01/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Limpiar_Formulario()
    {
        Txt_Fecha.Text = "";
        Cmb_Programa.SelectedIndex = 0;
        Txt_Anio.Text = "";
        Txt_Presupuesto.Text = "";
        Txt_Ejercido.Text = "";
        Txt_Disponible.Text = "";
        Txt_Comprometido.Text = "";
    }//fin de limpiar formulario

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Programa
    ///DESCRIPCIÓN          : Llena el Combo de tipos.
    ///PROPIEDADES          
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 01/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Combo_Programa()
    {
        Cls_Ope_Asignar_Presupuesto_Programas_Negocio Presupuesto_Compra = new Cls_Ope_Asignar_Presupuesto_Programas_Negocio();

        Cmb_Programa.DataBind();
        DataTable Programa = Presupuesto_Compra.Consultar_Programa();
        DataRow Fila_Programa = Programa.NewRow();
        Fila_Programa["PROYECTO_PROGRAMA_ID"] = HttpUtility.HtmlDecode("0000000000");
        Fila_Programa["CLAVE||''||NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
        Programa.Rows.InsertAt(Fila_Programa, 0);
        Cmb_Programa.DataSource = Programa;
        Cmb_Programa.DataValueField = "PROYECTO_PROGRAMA_ID";
        Cmb_Programa.DataTextField = "CLAVE||''||NOMBRE";
        Cmb_Programa.DataBind();
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Campos
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO                 : Leslie González Vázquez
    /// FECHA_CREO           : 02/marzo/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Campos()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        // Validar que el campo de fecha inicio no este vacio y se introduzca un numero
        if (Txt_Fecha.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha <br>";
            Datos_Validos = false;
        }
        // Validar que el campo de compra año no este vacio y se introduzca un numero
        if (Txt_Anio.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Año de presupuesto <br>";
            Datos_Validos = false;
        }
        // Validar que el campo de presupuesto no este vacio y el valor introducido sea un número
        if (Txt_Presupuesto.Text.Equals(""))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Presupuesto <br>";
            Datos_Validos = false;
        } 
        if(Cmb_Programa.SelectedIndex == (-1))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Programa <br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Alta_Presuspuesto_Programa
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO                 : Leslie González Vázquez
    /// FECHA_CREO           : 03/marzo/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Alta_Presuspuesto_Programa()
    {
        Boolean Registro_Valido = true;//Variable que almacenara si el registro a insertar corresponde a un registro válido 
        String Programa = "";//Identificador del programa
        String Anio = "";

        try
        {
            for (Int32 Contador_Filas = 0; Contador_Filas < Grid_Presupuesto_Programa.Rows.Count; Contador_Filas++) //recorre el grid para comparar las claves
            {
                Programa = Cmb_Programa.SelectedValue;
                Anio = Grid_Presupuesto_Programa.Rows[Contador_Filas].Cells[3].Text.Trim();
                if ((Txt_Anio.Text.Trim().Equals(Anio)) && (Cmb_Programa.SelectedValue.Equals(Programa)))
                {
                    Registro_Valido = false;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar que no se den de alta presupuestos iguales. Error: [" + Ex.Message + "]");
        }
        return Registro_Valido;
    }
    #endregion

    #region EVENTOS
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Img_buscar_Click
    ///DESCRIPCIÓN: Evento del boton Buscar 
    ///PARAMETROS:    
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 02/marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Img_buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Grid_Presupuesto_Programa();
        Estado_Botones("inicial");
        Habilitar_Forma(false);
        Limpiar_Formulario();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del Boton salir 
    ///PARAMETROS:   
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 01/Marzo/2011  
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        Cls_Ope_Asignar_Presupuesto_Programas_Negocio Presupuesto_Programa = new Cls_Ope_Asignar_Presupuesto_Programas_Negocio(); //Variable de conexion con la capa de negocios.
        switch (Btn_Salir.ToolTip)
        {
            case "Cancelar":
                Lbl_Mensaje_Error.Text = "";
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Estado_Botones("inicial");
                Limpiar_Formulario();
                Habilitar_Forma(false);
                Llenar_Grid_Presupuesto_Programa();
                break;

            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                break;
        }//fin del switch
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Boton que tiene la funcion de insertar un elemento en la tabla OPE_SAP_PRES_PROG_PROY
    ///PARAMETROS:   
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 02/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        Cls_Ope_Asignar_Presupuesto_Programas_Negocio Presupuesto_Programa = new Cls_Ope_Asignar_Presupuesto_Programas_Negocio();
        //Validacion para crear un nuevo registro y para habilitar los controles que se requieran
        switch (Btn_Nuevo.ToolTip)
        {
            case "Nuevo":
                Estado_Botones("nuevo");
                Limpiar_Formulario();
                Habilitar_Forma(true);
                Txt_Disponible.Enabled = false;
                Txt_Ejercido.Enabled = false;
                Txt_Comprometido.Enabled = false;
                Txt_Fecha.Enabled = false;
                Txt_Fecha.Text = System.DateTime.Now.ToString("yyyy/MM/dd");
                Txt_Anio.Text = System.DateTime.Now.ToString("yyyy");
                break;
            case "Dar de Alta":
                Lbl_Mensaje_Error.Text = "";
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;

                if (Validar_Campos())
                {
                    if (Validar_Alta_Presuspuesto_Programa())
                    {
                        Presupuesto_Programa.P_Proyecto_Programa_ID = Cmb_Programa.SelectedValue.Trim();
                        Presupuesto_Programa.P_Monto_Presupuestal = Txt_Presupuesto.Text.Trim();
                        Presupuesto_Programa.P_Monto_Ejercido = Txt_Ejercido.Text.Trim();
                        Presupuesto_Programa.P_Monto_Disponible = Txt_Disponible.Text.Trim();
                        Presupuesto_Programa.P_Monto_Comprometido = Txt_Comprometido.Text.Trim();
                        Presupuesto_Programa.P_Anio_Presupuesto = Txt_Anio.Text.Trim();
                        Presupuesto_Programa.P_Pres_Prog_Proy_ID = Presupuesto_Programa.Generar_ID().ToString();
                        Presupuesto_Programa.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;

                        if (Presupuesto_Programa.Insertar_Presupuesto_Programas())
                        {
                            //cargamos los datos a la clase de negocio
                            Estado_Botones("inicial");
                            Llenar_Grid_Presupuesto_Programa();
                            Habilitar_Forma(false);
                            Limpiar_Formulario();

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Alta Presupuesto proyecto", "alert('Operacion Completa');", true);
                            //Registramos la accion en la bitacora
                            Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Asignar_Presupuesto_Programas.aspx", Presupuesto_Programa.P_Pres_Prog_Proy_ID, "");
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text += "+ Ya existe un presupuesto para el programa en ese año. <br />";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true; 
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;   
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
        Cls_Ope_Asignar_Presupuesto_Programas_Negocio Presupuesto_Programa = new Cls_Ope_Asignar_Presupuesto_Programas_Negocio();//Variable de conexion con la capa de negocios.
        if (Grid_Presupuesto_Programa.SelectedIndex > (-1))
        {
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
           if (Txt_Ejercido.Text.Equals("0") && Txt_Comprometido.Text.Equals("0"))
           {
               switch (Btn_Modificar.ToolTip)
               {
                   //Validacion para actualizar un registro y para habilitar los controles que se requieran
                   case "Modificar":
                       Estado_Botones("modificar");
                       Habilitar_Forma(false);
                       Txt_Presupuesto.Enabled = true;
                       Llenar_Grid_Presupuesto_Programa();
                       break;
                   case "Actualizar":
                       Lbl_Mensaje_Error.Text = "";
                       Lbl_Mensaje_Error.Visible = false;
                       Img_Error.Visible = false;

                       //Obtengo el dataset antes de modificar
                       DataSet Datos_No_Modificados = Presupuesto_Programa.Consultar_Presupuesto_Programas();
                       GridViewRow selectedRow = Grid_Presupuesto_Programa.Rows[Grid_Presupuesto_Programa.SelectedIndex];
                       String Presupuesto_Programa_ID = HttpUtility.HtmlDecode(selectedRow.Cells[10].Text).ToString().Trim();

                       if (!Validar_Campos())
                       {
                           Lbl_Mensaje_Error.Visible = true;
                           Img_Error.Visible = true;
                       }
                       else
                       {
                           Presupuesto_Programa.P_Pres_Prog_Proy_ID = Presupuesto_Programa_ID;
                           Presupuesto_Programa.P_Monto_Presupuestal = Txt_Presupuesto.Text;
                           Presupuesto_Programa.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                           Presupuesto_Programa.P_Monto_Disponible = Txt_Disponible.Text;
                           if (Presupuesto_Programa.Actualizar_Presupuesto_Programas())
                           {
                               //Obtengo otro dataset con los datos ya modificados 
                               DataSet Datos_Modificados = Presupuesto_Programa.Consultar_Presupuesto_Programas();
                               //Genero la descripcion de las modificaciones realizadas 
                               String Descripcion_Bitacora = Cls_Bitacora.Revisar_Actualizaciones(Datos_No_Modificados, Datos_Modificados);
                               Habilitar_Forma(false);
                               Estado_Botones("inicial");
                               //Registro la accf ion de modificar en la bitacora 
                               Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Asignar_Presupuesto_Programas.aspx", Presupuesto_Programa.P_Pres_Prog_Proy_ID, Descripcion_Bitacora);
                               Llenar_Grid_Presupuesto_Programa();
                               ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Actualizar Presupuesto Programa", "alert('Operacion Completa');", true);
                           }
                       }
                       break;
               }//fin del switch
           }
           else
           {
               if (!Txt_Comprometido.Text.Equals("0"))
                   Lbl_Mensaje_Error.Text += "+ No se puede modificar por que Comprometido es mayor a cero. <br />";
               if (!Txt_Ejercido.Text.Equals("0"))
                   Lbl_Mensaje_Error.Text += "+  No se puede modificar por que Ejercido es mayor a cero. <br />";

               Lbl_Mensaje_Error.Visible = true;
               Img_Error.Visible = true;
           }
        }
        else
        {
            Lbl_Mensaje_Error.Text += " +  Favor de seleccionar un presupuesto. <br />";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }//fin de Modificar

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Presupuesto_TextChangedo 
    ///DESCRIPCIÓN: Evento de la caja de texto
    ///PARAMETROS:    
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 02/Marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Presupuesto_TextChanged(object sender, EventArgs e)
    {
        String Disponible = Txt_Presupuesto.Text.Trim();
        Txt_Ejercido.Text = "0";
        Txt_Comprometido.Text = "0";
        Txt_Disponible.Text = Disponible;      
    }
    #endregion

    #region GRID
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Presupuesto_Programa_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo para cargar los datos del elemento seleccionado
    ///PARAMETROS:   
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 04/marzo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Presupuesto_Programa_SelectedIndexChanged(object sender, EventArgs e)
    {
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        Cls_Ope_Asignar_Presupuesto_Programas_Negocio Presupuesto_Programa = new Cls_Ope_Asignar_Presupuesto_Programas_Negocio();
        if (Grid_Presupuesto_Programa.SelectedIndex > (-1))
        {
            Limpiar_Formulario();
            GridViewRow selectedRow = Grid_Presupuesto_Programa.Rows[Grid_Presupuesto_Programa.SelectedIndex];

            String Clave = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
            String Programa = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
            String Anio = HttpUtility.HtmlDecode(selectedRow.Cells[3].Text).ToString();
            String Presupuesto = HttpUtility.HtmlDecode(selectedRow.Cells[4].Text).ToString();
            String Disponible = HttpUtility.HtmlDecode(selectedRow.Cells[5].Text).ToString();
            String Clave_Programa = HttpUtility.HtmlDecode(selectedRow.Cells[6].Text).ToString();
            String Ejercido = HttpUtility.HtmlDecode(selectedRow.Cells[7].Text).ToString();
            String Fecha = HttpUtility.HtmlDecode(selectedRow.Cells[8].Text).ToString();
            String Comprometido = HttpUtility.HtmlDecode(selectedRow.Cells[9].Text).ToString();

            Txt_Fecha.Text = Fecha;
            Cmb_Programa.SelectedIndex = Cmb_Programa.Items.IndexOf(Cmb_Programa.Items.FindByValue(Clave_Programa));
            Txt_Anio.Text = Anio;
            Txt_Presupuesto.Text = Presupuesto;
            Txt_Ejercido.Text = Ejercido;
            Txt_Disponible.Text = Disponible;
            Txt_Comprometido.Text = Comprometido;

            Estado_Botones("Inicial");
            Habilitar_Forma(false);
            Llenar_Grid_Presupuesto_Programa();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Presupuesto_Programa_PageIndexChanging
    ///DESCRIPCIÓN: Metodo para manejar la paginacion del Grid_Presupuesto_Programa
    ///PARAMETROS:   
    ///CREO: Leslie González Vázquez
    ///FECHA_CREO: 04/Marzo/2011  
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Presupuesto_Programa_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Presupuesto_Programa.PageIndex = e.NewPageIndex;
        Llenar_Grid_Presupuesto_Programa();
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
}
