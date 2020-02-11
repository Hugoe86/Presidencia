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
using Presidencia.Catalogo_Compras_Proveedores.Negocio;
using Presidencia.Catalogo_Compras_Giro_Proveedor.Negocio;
using Presidencia.Catalogo_Compras_Giros.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Xml.Linq;
using Presidencia.Catalogo_SAP_Conceptos.Negocio;
using System.Collections.Generic;
using Presidencia.Catalogo_Compras_Proveedores.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text.RegularExpressions;
using System.Collections.Generic;



public partial class paginas_Compras_Frm_Cat_Com_Proveedores : System.Web.UI.Page
{

    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region PAGE_LOAD

    protected void Page_Load(object sender, EventArgs e)
    {

        Txt_Password.Attributes.Add("value", Txt_Password.Text);
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "ASC";
            Configurar_Formulario("Inicio");
        }
        Txt_Password.Attributes.Add("value", Txt_Password.Text);
    }
    #endregion

    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Configurar_Formulario
    ///DESCRIPCIÓN: Metodo que ayuda a configuarar el formulario 
    ///PARAMETROS:Estatus, Puede Ser Inicio, Nuevo, Modificar 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 4/NOV/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    ///
    public void Configurar_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicio":
                //Limpiar_Componentes();
                Habilitar_Componentes(false);
                Div_Busqueda_Avanzada.Visible = false;
                Grid_Proveedores.Enabled = true;
                Btn_Busqueda_Avanzada.Enabled = true;
                //Boton Nuevo
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Div_Proveedores.Visible = false;
                Tab_Giros.Visible = true;
                Configuracion_Acceso("Frm_Cat_Com_Proveedores.aspx");
                break;
            case "Nuevo":
                Limpiar_Componentes();
                Habilitar_Componentes(true);
                Div_Busqueda_Avanzada.Visible = false;
                Grid_Proveedores.Enabled = false;
                Btn_Busqueda_Avanzada.Enabled = false;
                //Boton Nuevo
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                //Boton Modificar
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Div_Proveedores.Visible = false;
                Configuracion_Acceso("Frm_Cat_Com_Proveedores.aspx");
                break;
            case "Modificar":
                
                Habilitar_Componentes(true);
                Div_Busqueda_Avanzada.Visible = false;
                Grid_Proveedores.Enabled = false;
                Btn_Busqueda_Avanzada.Enabled = false;
                //Boton Nuevo
                Btn_Nuevo.Visible = false;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Div_Proveedores.Visible = false;

                Configuracion_Acceso("Frm_Cat_Com_Proveedores.aspx");
                break;


        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Limpiar_Componentes
    ///DESCRIPCIÓN: Metodo que limpia las cajas de texto del catalogo de Proveedores
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 4/NOV/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Limpiar_Componentes()
    {
        Txt_Proveedor_ID.Text = "";
        Txt_Fecha_Registro.Text = "";
        Txt_Razon_Social.Text = "";
        Txt_Nombre_Comercial.Text = "";
        Txt_Representante_Legal.Text = "";
        Txt_Contacto.Text = "";
        Txt_RFC.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Tipo.SelectedIndex = 0;
        Chk_Fisica.Checked = false;
        Chk_Moral.Checked = false;
        Txt_Direccion.Text = "";
        Txt_Colonia.Text = "";
        Txt_Ciudad.Text = "";
        Txt_Estado.Text = "";
        Txt_Codigo_Postal.Text = "";
        Txt_Telefono1.Text = "";
        Txt_Telefono2.Text = "";
        Txt_Nextel.Text = "";
        Txt_Fax.Text = "";
        Cmb_Tipo_Pago.SelectedIndex = 0;
        Txt_Dias_Credito.Text = "";
        Cmb_Forma_Pago.SelectedIndex = 0;
        Txt_Correo.Text = "";
        Txt_Password.Text = "";
        Txt_Comentarios.Text = "";
        Txt_Ultima_Actualizacion.Text = "";
        Chk_Actualizacion.Checked = false;
        //Limpiamos los Grid
        Grid_Conceptos_Proveedor.DataSource = new DataTable();
        Grid_Conceptos_Proveedor.DataBind();

        Grid_Partidas_Generales.DataSource = new DataTable();
        Grid_Partidas_Generales.DataBind();
        //Variables de Session
        Session["Proveedor_ID"] = null;
        Session["Dt_Conceptos_Proveedor"] = null;
        Session["Dt_Consulta_Partidas_Proveedores"] = null;
        
    }

    public void Limpiar_Controles_Busqueda_Avanzada()
    {
        //Limpiamos las Cajas y variable de session del Div de Busqueda avanzada
        Txt_Busqueda_Padron_Proveedor.Text = "";
        Txt_Busqueda_Nombre_Comercial.Text = "";
        Txt_Busqueda_RFC.Text = "";
        Txt_Busqueda_Razon_Social.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Tipo.SelectedIndex = 0;
          
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Habilitar_Componentes
    ///DESCRIPCIÓN: Metodo que  Habilita o Deshabilita los componentes
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 4/NOV/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Componentes(bool Habilitar)
    {

        Txt_Proveedor_ID.Enabled = false;
        Txt_Fecha_Registro.Enabled = false;
        Txt_Razon_Social.Enabled = Habilitar;
        Txt_Nombre_Comercial.Enabled = Habilitar;
        Txt_Representante_Legal.Enabled = Habilitar;
        Txt_Contacto.Enabled = Habilitar;
        Txt_RFC.Enabled = Habilitar;
        Cmb_Estatus.Enabled = Habilitar;
        Cmb_Tipo.Enabled = Habilitar;
        Chk_Fisica.Enabled = Habilitar;
        Chk_Moral.Enabled = Habilitar;
        Txt_Direccion.Enabled = Habilitar;
        Txt_Colonia.Enabled =Habilitar;
        Txt_Ciudad.Enabled = Habilitar;
        Txt_Estado.Enabled = Habilitar;
        Txt_Codigo_Postal.Enabled = Habilitar;
        Txt_Telefono1.Enabled = Habilitar;
        Txt_Telefono2.Enabled = Habilitar;
        Txt_Nextel.Enabled = Habilitar;
        Txt_Fax.Enabled = Habilitar;
        Cmb_Tipo_Pago.Enabled= Habilitar;
        Txt_Dias_Credito.Enabled = Habilitar;
        Cmb_Forma_Pago.Enabled = Habilitar;
        Txt_Correo.Enabled = Habilitar;
        Txt_Password.Enabled = Habilitar;
        Txt_Comentarios.Enabled = Habilitar;
        Txt_Ultima_Actualizacion.Enabled = false;
        Chk_Actualizacion.Enabled = Habilitar;
        Cmb_Conceptos.Enabled = Habilitar;
        Cmb_Partidas_Generales.Enabled = Habilitar;
        Grid_Conceptos_Proveedor.Enabled = Habilitar;
        Grid_Partidas_Generales.Enabled = Habilitar;
        //Tab_Giros.Visible = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Habilitar_Componentes_Tipo
    ///DESCRIPCIÓN: Metodo que  Habilita o Deshabilita los componentes para el tipo
    ///PARAMETROS:
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 4/NOV/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Componentes_Tipo(bool Habilitar)
    {

        Txt_Proveedor_ID.Enabled = false;
        Txt_Fecha_Registro.Enabled = false;
        Txt_Razon_Social.Enabled = Habilitar;
        Txt_Nombre_Comercial.Enabled = !Habilitar;
        Txt_Representante_Legal.Enabled = !Habilitar;
        Txt_Contacto.Enabled = !Habilitar;
        Txt_RFC.Enabled = Habilitar;
        Cmb_Estatus.Enabled = Habilitar;
        Cmb_Tipo.Enabled = Habilitar;
        Chk_Fisica.Enabled = Habilitar;
        Chk_Moral.Enabled = Habilitar;
        Txt_Direccion.Enabled = !Habilitar;
        Txt_Colonia.Enabled = !Habilitar;
        Txt_Ciudad.Enabled = !Habilitar;
        Txt_Estado.Enabled = !Habilitar;
        Txt_Codigo_Postal.Enabled = !Habilitar;
        Txt_Telefono1.Enabled = !Habilitar;
        Txt_Telefono2.Enabled = !Habilitar;
        Txt_Nextel.Enabled = !Habilitar;
        Txt_Fax.Enabled = !Habilitar;
        Cmb_Tipo_Pago.Enabled = !Habilitar;
        Txt_Dias_Credito.Enabled= !Habilitar;
        Cmb_Forma_Pago.Enabled = !Habilitar;
        Txt_Correo.Enabled = !Habilitar;
        Txt_Password.Enabled = !Habilitar;
        Txt_Comentarios.Enabled = !Habilitar;
        Txt_Ultima_Actualizacion.Enabled = false;
        Chk_Actualizacion.Enabled = !Habilitar;
        Cmb_Conceptos.Enabled = !Habilitar;
        Cmb_Partidas_Generales.Enabled = !Habilitar;
        Grid_Conceptos_Proveedor.Enabled = !Habilitar;
        Grid_Partidas_Generales.Enabled = !Habilitar;
        Tab_Giros.Visible = !Habilitar;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Conceptos
    ///DESCRIPCIÓN          : Llena el Combo de Conceptos con los existentes en la Base de Datos.
    ///PARAMETROS           :
    ///CREO: Susana Trigueros A.
    ///FECHA_CREO: 7/NOV/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    public void Llenar_Combo_Conceptos()
    {
        try
        {
            Cls_Cat_Com_Proveedores_Negocio Negocio = new Cls_Cat_Com_Proveedores_Negocio();
            DataTable Data_Table = Negocio.Consultar_Conceptos();
            Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Conceptos, Data_Table);
            
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Partidas
    ///DESCRIPCIÓN          : Llena el Combo de Partidas con los existentes en la Base de Datos.
    ///PARAMETROS           :
    ///CREO: Susana Trigueros A.
    ///FECHA_CREO: 7/NOV/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    public void Llenar_Combo_Partidas()
    {
        Cls_Cat_Com_Proveedores_Negocio Clase_Negocio = new Cls_Cat_Com_Proveedores_Negocio();
        Clase_Negocio.P_Concepto_ID = Cmb_Conceptos.SelectedValue;
        DataTable Dt_Partidas = Clase_Negocio.Consultar_Partidas_Especificas();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Partidas_Generales,Dt_Partidas);


    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Contenido_Controles
    ///DESCRIPCIÓN          : Verifica si la informacion ingresada en las cajas de texto por el usuario es valida
    ///PARAMETROS           :
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/NOV/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Validar_Contenido_Controles()
    {
        if (Cmb_Tipo.SelectedValue != "COMPRAS")
        {
            if (Txt_Razon_Social.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar la Razon Social<br/>";
            }
            if (Txt_RFC.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar el RFC<br/>";
            }
            if (Chk_Fisica.Checked == false && Chk_Moral.Checked == false)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar si es persona Fisica o Moral<br/>";
            }
        }

        else
        {
            if (Txt_Razon_Social.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar la Razon Social<br/>";
            }

            if (Txt_Nombre_Comercial.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar el Nombre Comercial<br/>";
            }

            if (Txt_Representante_Legal.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar el Representante Legal<br/>";
            }

            if (Txt_Contacto.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar el Contacto<br/>";
            }

            if (Txt_RFC.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar el RFC<br/>";
            }

            if (Chk_Fisica.Checked == false && Chk_Moral.Checked == false)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar si es persona Fisica o Moral<br/>";
            }

            if (Txt_Direccion.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar la dirección<br/>";
            }

            if (Txt_Colonia.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar la Colonia<br/>";
            }

            if (Txt_Ciudad.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar la Ciudad<br/>";
            }
            if (Txt_Estado.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar el Estado<br/>";
            }

            if (Txt_Codigo_Postal.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar la CP<br/>";
            }
            if (Txt_Telefono1.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario indicar el Telefono <br/>";
            }
            //Validamos el Correo del Proveedor que el correo sea correcto
            Validar_Email();
            if (Txt_Password.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario asignar un Password<br/>";
            }
        }
        Lbl_Mensaje_Error.Visible = true;

    }

    public void Agregar_Concepto_Proveedor()
    {
        DataTable Dt_Conceptos_Proveedor = new DataTable();
        if (Session["Dt_Conceptos_Proveedor"] != null)
        {
            Dt_Conceptos_Proveedor = (DataTable)Session["Dt_Conceptos_Proveedor"];
            DataRow[] Fila_Nueva;
            Fila_Nueva = Dt_Conceptos_Proveedor.Select("CONCEPTO_ID='" + Cmb_Conceptos.SelectedValue.Trim() + "'");
            if (Fila_Nueva.Length > 0)
            {

            }
            else
            {
                DataRow Fila = Dt_Conceptos_Proveedor.NewRow();
                Fila["CONCEPTO_ID"] = Cmb_Conceptos.SelectedValue.ToString();
                Fila["CONCEPTO"] = Cmb_Conceptos.SelectedItem.Text;
                Session["Dt_Conceptos_Proveedor"] = Dt_Conceptos_Proveedor;
                Dt_Conceptos_Proveedor.Rows.Add(Fila);
                Dt_Conceptos_Proveedor.AcceptChanges();
                Grid_Conceptos_Proveedor.DataSource = Dt_Conceptos_Proveedor;
                Session["Dt_Conceptos_Proveedor"] = Dt_Conceptos_Proveedor;
                Grid_Conceptos_Proveedor.DataBind();
                //Agregamos el Concepto al Grid de Conceptos 


            }
        }
        else
        {
            Dt_Conceptos_Proveedor.Columns.Add("CONCEPTO_ID", typeof(System.String));
            Dt_Conceptos_Proveedor.Columns.Add("CONCEPTO", typeof(System.String));
            Session["Dt_Conceptos_Proveedor"] = Dt_Conceptos_Proveedor;
            DataRow[] Fila_Nueva;
            Fila_Nueva = Dt_Conceptos_Proveedor.Select("CONCEPTO_ID='" + Cmb_Conceptos.SelectedValue.Trim() + "'");
            if (Fila_Nueva.Length > 0)
            {

            }
            else
            {
                DataRow Fila = Dt_Conceptos_Proveedor.NewRow();
                Fila["CONCEPTO_ID"] = Cmb_Conceptos.SelectedValue.ToString();
                Fila["CONCEPTO"] = Cmb_Conceptos.SelectedItem.Text;
                Session["Dt_Conceptos_Proveedor"] = Dt_Conceptos_Proveedor;
                Dt_Conceptos_Proveedor.Rows.Add(Fila);
                Dt_Conceptos_Proveedor.AcceptChanges();
                Grid_Conceptos_Proveedor.DataSource = Dt_Conceptos_Proveedor;
                Session["Dt_Conceptos_Proveedor"] = Dt_Conceptos_Proveedor;
                Grid_Conceptos_Proveedor.DataBind();
            }

        }

    }//fin del metodo de Agregar_Concepto_Proveedor

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Email
    ///DESCRIPCIÓN: 
    ///PARAMETROS: Metodo que permite validar el correo ingresado por el usuario
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Validar_Email()
    {
        if (Txt_Correo.Text.Trim().Length > 0)
        {
            Regex Exp_Regular = new Regex("^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+$");
            Match Comparar = Exp_Regular.Match(Txt_Correo.Text);

            if (!Comparar.Success)
            {
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ El contenido del correo electronico es incorrecto <br />";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    public Cls_Cat_Com_Proveedores_Negocio Cargar_Datos_Proveedor(Cls_Cat_Com_Proveedores_Negocio Clase_Negocio)
    {
        if (Cmb_Tipo.SelectedValue != "COMPRAS")
        {
            if (Txt_Proveedor_ID.Text.Trim() != String.Empty)
                Clase_Negocio.P_Proveedor_ID = Txt_Proveedor_ID.Text.Trim();

            Clase_Negocio.P_Razon_Social = Txt_Razon_Social.Text.Trim();
            Clase_Negocio.P_RFC = Txt_RFC.Text.Trim();
            Clase_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
            if (Chk_Fisica.Checked == true)
            {
                Clase_Negocio.P_Tipo_Persona_Fiscal = "FISICA";
            }
            if (Chk_Moral.Checked == true)
            {
                Clase_Negocio.P_Tipo_Persona_Fiscal = "MORAL";
            }
            Clase_Negocio.P_Tipo = Cmb_Tipo.SelectedValue;
            //Clase_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
        }

        else
        {
            if (Txt_Proveedor_ID.Text.Trim() != String.Empty)
                Clase_Negocio.P_Proveedor_ID = Txt_Proveedor_ID.Text.Trim();
            Clase_Negocio.P_Razon_Social = Txt_Razon_Social.Text.Trim();
            Clase_Negocio.P_Nombre_Comercial = Txt_Nombre_Comercial.Text.Trim();
            Clase_Negocio.P_Representante_Legal = Txt_Representante_Legal.Text.Trim();
            Clase_Negocio.P_Contacto = Txt_Contacto.Text.Trim();
            Clase_Negocio.P_RFC = Txt_RFC.Text.Trim();
            Clase_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
            if (Chk_Fisica.Checked == true)
            {
                Clase_Negocio.P_Tipo_Persona_Fiscal = "FISICA";
            }
            if (Chk_Moral.Checked == true)
            {
                Clase_Negocio.P_Tipo_Persona_Fiscal = "MORAL";
            }
            Clase_Negocio.P_Tipo = Cmb_Tipo.SelectedValue;
            Clase_Negocio.P_Direccion = Txt_Direccion.Text.Trim();
            Clase_Negocio.P_Colonia = Txt_Colonia.Text.Trim();
            Clase_Negocio.P_Ciudad = Txt_Ciudad.Text.Trim();
            Clase_Negocio.P_Estado = Txt_Estado.Text.Trim();
            Clase_Negocio.P_CP = int.Parse(Txt_Codigo_Postal.Text.Trim());
            Clase_Negocio.P_Telefono_1 = Txt_Telefono1.Text.Trim();
            if (Txt_Telefono2.Text.Trim() != String.Empty)
            {
                Clase_Negocio.P_Telefono_2 = Txt_Telefono2.Text.Trim();
            }
            if (Txt_Nextel.Text.Trim() != String.Empty)
            {
                Clase_Negocio.P_Nextel = Txt_Nextel.Text.Trim();
            }
            if (Txt_Fax.Text.Trim() != String.Empty)
            {
                Clase_Negocio.P_Fax = Txt_Fax.Text.Trim();
            }
            if (Cmb_Tipo_Pago.SelectedIndex != 0)
            {
                Clase_Negocio.P_Tipo_Pago = Cmb_Tipo_Pago.SelectedValue;
            }
            if (Cmb_Forma_Pago.SelectedIndex != 0)
            {
                Clase_Negocio.P_Forma_Pago = Cmb_Forma_Pago.SelectedValue;
            }
            if (Txt_Dias_Credito.Text != String.Empty)
            {
                Clase_Negocio.P_Dias_Credito = int.Parse(Txt_Dias_Credito.Text.Trim());
            }
            if (Txt_Correo.Text.Trim() != String.Empty)
            {
                Clase_Negocio.P_Correo_Electronico = Txt_Correo.Text.Trim();
            }
            if (Txt_Password.Text.Trim() != String.Empty)
            {
                Clase_Negocio.P_Password = Txt_Password.Text.Trim();
            }
            if (Txt_Comentarios.Text.Trim() != String.Empty)
            {
                Clase_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();
            }

            // Agregamos a la clase de negocio si existe una nueva Actualizacion 
            if (Chk_Actualizacion.Checked == true)
            {
                Clase_Negocio.P_Nueva_Actualizacion = true;
            }
            else
            {
                Clase_Negocio.P_Nueva_Actualizacion = false;
            }
            //Cargamos el Dt de Conceptos y el Dt de partidas
            Clase_Negocio.P_Dt_Partidas_Proveedor = (DataTable)Session["Dt_Consulta_Partidas_Proveedores"];
            Clase_Negocio.P_Dt_Conceptos_Proveedor = (DataTable)Session["Dt_Conceptos_Proveedor"];
        }

        return Clase_Negocio;
    }
    #endregion


    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    #region Grid_Proveedores


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Habilitar_Componentes
    ///DESCRIPCIÓN: Metodo que  Habilita o Deshabilita los componentes
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 4/NOV/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Proveedores_SelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        //Consultamos el Proveedor ID que selecciono
        Cls_Cat_Com_Proveedores_Negocio Clase_Negocio = new Cls_Cat_Com_Proveedores_Negocio();
        Clase_Negocio.P_Proveedor_ID = Grid_Proveedores.SelectedDataKey["Proveedor_ID"].ToString();
        //Consultamos los datos dep Proveedor seleccionado 

        DataTable Dt_Datos_Proveedor = Clase_Negocio.Consulta_Proveedores();
        Session["Proveedor_ID"]= Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Proveedor_ID].ToString().Trim();
        Txt_Proveedor_ID.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Proveedor_ID].ToString().Trim();

        if (Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Proveedor_ID].ToString().Trim() != String.Empty)
        {
            try
            {
                Txt_Fecha_Registro.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Fecha_Registro].ToString().Trim()));
            }
            catch
            {
                Txt_Fecha_Registro.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Fecha_Registro].ToString().Trim();
            }
        }
        Txt_Razon_Social.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Nombre].ToString().Trim();
        Txt_Nombre_Comercial.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Compañia].ToString().Trim();
        Txt_Representante_Legal.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Representante_Legal].ToString().Trim();
        Txt_Contacto.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Contacto].ToString().Trim();
        Txt_RFC.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_RFC].ToString().Trim();
        //  para el combo de tipo(compras o patrimonio)
        Cmb_Tipo.SelectedIndex = Cmb_Tipo.Items.IndexOf(Cmb_Tipo.Items.FindByValue(Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Tipo].ToString()));
        switch (Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Tipo_Fiscal].ToString().Trim())
        {
            case "FISICA":
                Chk_Fisica.Checked = true;
                Chk_Moral.Checked = false;
                break;
            case "MORAL":
                Chk_Fisica.Checked = false;
                Chk_Moral.Checked = true;
                break;
        }        
        Txt_Direccion.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Direccion].ToString().Trim();
        Txt_Colonia.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Colonia].ToString().Trim();
        Txt_Ciudad.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Ciudad].ToString().Trim();
        Txt_Estado.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Estado].ToString().Trim();
        Txt_Codigo_Postal.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_CP].ToString().Trim();
        Txt_Telefono1.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Telefono_1].ToString().Trim();
        Txt_Telefono2.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Telefono_2].ToString().Trim();
        Txt_Nextel.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Nextel].ToString().Trim();
        Txt_Fax.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Fax].ToString().Trim();
        Txt_Dias_Credito.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Dias_Credito].ToString().Trim();
        
        Txt_Correo.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Correo_Electronico].ToString().Trim();
        Txt_Password.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Password].ToString().Trim();
        Txt_Password.Attributes.Add("value", Txt_Password.Text);
        Txt_Comentarios.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Comentarios].ToString().Trim();
        Txt_Ultima_Actualizacion.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Fecha_Actualizacion].ToString().Trim();
        if(Txt_Ultima_Actualizacion.Text.Trim() != String.Empty)
            Session["Ultima_Actualizacion"] = Txt_Ultima_Actualizacion.Text.Trim();
        else
            Session["Ultima_Actualizacion"] = null;
        Chk_Actualizacion.Checked = false;

       
        //Asignamos valor del combo Estatus
        switch (Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Estatus].ToString().Trim())
        {
            case "ACTIVO":
                Cmb_Estatus.SelectedValue = "ACTIVO";
            break;
            case "INACTIVO":
                Cmb_Estatus.SelectedValue = "INACTIVO";
            break;
        }
        // Asignamos valor del combo Tipo de Pago
        switch (Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Tipo_Pago].ToString().Trim())
        {
            case "CREDITO":
                Cmb_Tipo_Pago.SelectedValue = "CREDITO";
                Txt_Dias_Credito.Text = Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Dias_Credito].ToString().Trim();
                break;
            case "CONTADO":
                Cmb_Tipo_Pago.SelectedValue = "CONTADO";
                Txt_Dias_Credito.Text = "";
                break;
        }
        //Asignar el valor del combo Forma de pago
        switch (Dt_Datos_Proveedor.Rows[0][Cat_Com_Proveedores.Campo_Forma_Pago].ToString().Trim())
        {
            case "TRANSFERENCIA":
                Cmb_Forma_Pago.SelectedValue = "TRANSFERENCIA";
                break;
            case "CHEQUE":
                Cmb_Forma_Pago.SelectedValue = "CHEQUE";
                break;
            case "EFECTIVO":
                Cmb_Forma_Pago.SelectedValue = "EFECTIVO";
                break;
        }
        //Consultamos los Conceptos asignado al Proveedor.
        Clase_Negocio.P_Proveedor_ID = Session["Proveedor_ID"].ToString().Trim();
        DataTable Dt_Conceptos_Proveedor = Clase_Negocio.Consultar_Detalles_Conceptos();
        if (Dt_Conceptos_Proveedor.Rows.Count != 0)
        {
            Grid_Conceptos_Proveedor.DataSource = Dt_Conceptos_Proveedor;
            Grid_Conceptos_Proveedor.DataBind();
            Session["Dt_Conceptos_Proveedor"] = Dt_Conceptos_Proveedor;
        }
        else
        {
            Grid_Conceptos_Proveedor.EmptyDataText = "No se encontraron conceptos de este Proveedor";
            Grid_Conceptos_Proveedor.DataSource = new DataTable();
            Grid_Conceptos_Proveedor.DataBind();
        }
        //Consultar las Partidas asignadas al Proveedor.
        DataTable Dt_Partidas_Proveedor = Clase_Negocio.Consultar_Detalle_Partidas();
        if (Dt_Partidas_Proveedor.Rows.Count != 0)
        {
            Grid_Partidas_Generales.DataSource = Dt_Partidas_Proveedor;
            Grid_Partidas_Generales.DataBind();
            Session["Dt_Consulta_Partidas_Proveedores"] = Dt_Partidas_Proveedor;
        }
        else
        {
            Grid_Partidas_Generales.EmptyDataText = "No se encontraron partidas de este Proveedor";
            Grid_Partidas_Generales.DataSource = new DataTable();
            Grid_Partidas_Generales.DataBind();
        }
        //Consultamos si hay Historial de Actualizaciones
        DataTable Dt_Historia_Actualizaciones = Clase_Negocio.Consultar_Actualizaciones_Proveedores();
        if (Dt_Historia_Actualizaciones.Rows.Count != 0)
        {
            Grid_Historial_Act.DataSource = Dt_Historia_Actualizaciones;
            Grid_Historial_Act.DataBind();
        }
        else
        {
            Grid_Historial_Act.EmptyDataText = "No se encontro historial de actualizaciones de este Proveedor";
            Grid_Historial_Act.DataSource = new DataTable();
            Grid_Historial_Act.DataBind();
        }


        //llenamos los combos de Conceptos y Partidas
        Llenar_Combo_Conceptos();
        Llenar_Combo_Partidas();

        Div_Proveedores.Visible = false;
        Session["Dt_Proveedores"] = null;

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Llenar_Grid_Proveedores
    ///DESCRIPCIÓN: Metodo que  llena el Grid de Proveedores
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 4/NOV/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Proveedores(Cls_Cat_Com_Proveedores_Negocio Clase_Negocio)
    {
        DataTable Dt_Proveedores = Clase_Negocio.Consulta_Proveedores();
        if (Dt_Proveedores.Rows.Count != 0)
        {
            Grid_Proveedores.DataSource = Dt_Proveedores;
            Grid_Proveedores.DataBind();
            Session["Dt_Proveedores"] = Dt_Proveedores;
        }
        else
        {
            Grid_Proveedores.EmptyDataText = "No se han encontrado registros.";
            //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            Grid_Proveedores.DataSource = new DataTable();
            Grid_Proveedores.DataBind();
        }

    }

    protected void Grid_Partidas_Generales_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataRow[] Renglones;
        DataRow[] Renglon_Concepto;
        DataRow Renglon;
        //Obtenemos el Id del producto seleccionado
        GridViewRow selectedRow = Grid_Partidas_Generales.Rows[Grid_Partidas_Generales.SelectedIndex];
        String Id = Convert.ToString(selectedRow.Cells[1].Text);
        int num_fila = Grid_Partidas_Generales.SelectedIndex;
        DataTable Dt_Consulta_Giros_Proveedore = (DataTable)Session["Dt_Conceptos_Proveedor"];
        DataTable Dt_Consulta_Partidas_Proveedores = (DataTable)Session["Dt_Consulta_Partidas_Proveedores"];

        String Concepto_ID = Dt_Consulta_Partidas_Proveedores.Rows[num_fila]["CONCEPTO_ID"].ToString().Trim();

        Renglones = ((DataTable)Session["Dt_Consulta_Partidas_Proveedores"]).Select(Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + "='" + Id + "'");
        Renglon_Concepto = ((DataTable)Session["Dt_Consulta_Partidas_Proveedores"]).Select(Cat_SAP_Partida_Generica.Campo_Concepto_ID + "='" + Concepto_ID + "'");
        if (Renglones.Length > 0)
        {
            Renglon = Renglones[0];
            DataTable Tabla = (DataTable)Session["Dt_Consulta_Partidas_Proveedores"];
            Tabla.Rows.Remove(Renglon);
            Session["Dt_Consulta_Partidas_Proveedores"] = Tabla;
            Grid_Partidas_Generales.SelectedIndex = (-1);
            Grid_Partidas_Generales.DataSource = Tabla;
            Grid_Partidas_Generales.DataBind();

        }

        if (Renglon_Concepto.Length == 1)
        {
            //Eliminamos el concepto
            DataTable Tabla = (DataTable)Session["Dt_Conceptos_Proveedor"];
            Renglon_Concepto = ((DataTable)Session["Dt_Conceptos_Proveedor"]).Select(Cat_SAP_Partida_Generica.Campo_Concepto_ID + "='" + Concepto_ID + "'");
            Renglon = Renglon_Concepto[0];
            Tabla.Rows.Remove(Renglon);
            Session["Dt_Conceptos_Proveedor"] = Tabla;
            Grid_Conceptos_Proveedor.SelectedIndex = (-1);
            Grid_Conceptos_Proveedor.DataSource = Tabla;
            Grid_Conceptos_Proveedor.DataBind();

        }
    }

    protected void Grid_Proveedores_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable Dt_Proveedores = (DataTable)Session["Dt_Proveedores"];

        if (Dt_Proveedores != null)
        {
            DataView Dv_Proveedores = new DataView(Dt_Proveedores);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Proveedores.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Proveedores.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Proveedores.DataSource = Dv_Proveedores;
            Grid_Proveedores.DataBind();
            //Guardamos el cambio dentro de la variable de session de Dt_Requisiciones
            Session["Dt_Proveedores"] = (DataTable)Dv_Proveedores.Table;
            Dt_Proveedores = (DataTable)Session["Dt_Proveedores"];

        }
    }

    #endregion


    #endregion


    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos

    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Nuevo.ToolTip)
        {
            case "Nuevo":
                Configurar_Formulario("Nuevo");
                Limpiar_Componentes();
                Habilitar_Componentes(true);
                //Asignamos por default el Password con el valor 123456 cuando es un proveedor nuevo
                Txt_Password.Text = "123456";
                Txt_Password.Attributes.Add("value", Txt_Password.Text);
                //Llenamos el combo de Conceptos
                Llenar_Combo_Conceptos();
                //Deshabilitamos el combo de Partidas
                Cmb_Partidas_Generales.Enabled = false;
                Div_Proveedores.Visible = false;
                Grid_Proveedores.DataSource = new DataTable();
                Grid_Proveedores.DataBind();
                Session["Dt_Proveedores"] = null;
                //No se debe permir modificar el Password
                Txt_Password.Enabled = false;
                break;
            case "Dar de Alta":
                //Validamos que se llenen todos los campos requeridos
                Validar_Contenido_Controles();
                //En caso de que pase las validaciones
                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    //Como primer paso cargamos los datos del Proveedor
                    Cls_Cat_Com_Proveedores_Negocio Clase_Negocio = new Cls_Cat_Com_Proveedores_Negocio();
                    Clase_Negocio = Cargar_Datos_Proveedor(Clase_Negocio);
                    String Mensaje = "";
                    //Damos de Alta el Proveedor
                    if (!Clase_Negocio.Clave_RFC_Duplicada())
                    {
                        Mensaje = Clase_Negocio.Alta_Proveedor();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Proveedores", "alert('" + Mensaje + "');", true);
                        Configurar_Formulario("Inicio");
                    }
                    else 
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Proveedores", "alert('" + "Datos de proveedor duplicados, verifique información" + "');", true);
                    }
                }


                break;
        }
    }




    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Modificar.ToolTip)
        {
            case "Modificar":
                if (Txt_Proveedor_ID.Text.Trim() == String.Empty)
                {

                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario seleccionar un Proveedor";
                    Lbl_Mensaje_Error.Visible = true;
                }
                else
                {
                    Configurar_Formulario("Modificar");
                    //  para habilitar las cajas de texto de patrimonio
                    if (Cmb_Tipo.SelectedValue != "COMPRAS")
                        Habilitar_Componentes_Tipo(true);

                    else
                        Habilitar_Componentes(true);
                    
                }
                break;
            case "Actualizar":
                //Validamos que se llenen todos los campos requeridos
                Validar_Contenido_Controles();
                //En caso de que pase las validaciones
                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    //Como primer paso cargamos los datos del Proveedor
                    Cls_Cat_Com_Proveedores_Negocio Clase_Negocio = new Cls_Cat_Com_Proveedores_Negocio();

                    Clase_Negocio = Cargar_Datos_Proveedor(Clase_Negocio);

                    //Modificamos el Proveedor
                    if (!Clase_Negocio.Clave_RFC_Duplicada())
                    {
                        String Mensaje = Clase_Negocio.Modificar_Proveedor();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Proveedores", "alert('" + Mensaje + "');", true);
                        Configurar_Formulario("Inicio");
                    }
                    else 
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Proveedores", "alert('" + "Datos de proveedor duplicados, verifique información" + "');", true);
                    }
                }


                break;
        }
    }

    protected void  Chk_Actualizacion_CheckedChanged(object sender, EventArgs e)
    {
        if(Chk_Actualizacion.Checked == true)
        {
            if(Txt_Ultima_Actualizacion.Text != String.Empty)
            {
                Session["Ultima_Actualizacion"]= Txt_Ultima_Actualizacion.Text.Trim();
            }
            Txt_Ultima_Actualizacion.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }

        if(Chk_Actualizacion.Checked == false)
        {
            if(Session["Ultima_Actualizacion"] != null)
                Txt_Ultima_Actualizacion.Text = Session["Ultima_Actualizacion"].ToString().Trim();
            else 
                Txt_Ultima_Actualizacion.Text = "";
        }
    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Salir.ToolTip)
        {
            case "Cancelar":
                Configurar_Formulario("Inicio");
                Limpiar_Componentes();
                Habilitar_Componentes(false);


                break;
            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                Limpiar_Componentes();
                Habilitar_Componentes(false);

                break;
        }
    }

    protected void Cmb_Conceptos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        //LLENAMOS EL GRID DE PARTIDAS PARA AGREGARLAS AL PROVEEDOR
        Cls_Cat_Com_Proveedores_Negocio Clase_Negocio = new Cls_Cat_Com_Proveedores_Negocio();
        Clase_Negocio.P_Concepto_ID = Cmb_Conceptos.SelectedValue;
        DataTable Dt_Partidas = Clase_Negocio.Consultar_Partidas_Especificas();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Partidas_Generales, Dt_Partidas);
        if (Cmb_Conceptos.SelectedIndex == 0)
        {
            //Deshabilitamos el combo de Partidas 
            Cmb_Partidas_Generales.Enabled = false;
            Cmb_Partidas_Generales.Items.Clear();

        }
        else
        {
            Cmb_Partidas_Generales.Enabled = true;
        }
    }
    protected void Cmb_Tipo_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Tipo.SelectedValue != "COMPRAS")
                Habilitar_Componentes_Tipo(true);

            else
                Habilitar_Componentes(true);

            
        }
        catch (Exception ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Cmb_Partidas_Generales_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Si se selecciona se agrega al grid.
        Cls_Cat_Com_Proveedores_Negocio Clase_Negocio = new Cls_Cat_Com_Proveedores_Negocio();
        DataTable Dt_Consulta_Partidas_Proveedores = new DataTable();
        if (Session["Dt_Consulta_Partidas_Proveedores"] != null)
        {
            Dt_Consulta_Partidas_Proveedores = (DataTable)Session["Dt_Consulta_Partidas_Proveedores"];
            DataRow[] Fila_Nueva;
            Fila_Nueva = Dt_Consulta_Partidas_Proveedores.Select("PARTIDA_GENERICA_ID='" + Cmb_Partidas_Generales.SelectedValue.Trim() + "'");
            if (Fila_Nueva.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se puede agregar la dependencia, ya se ha agregado');", true);
            }
            else
            {
                DataRow Fila = Dt_Consulta_Partidas_Proveedores.NewRow();
                Fila["PARTIDA_GENERICA_ID"] = Cmb_Partidas_Generales.SelectedValue.ToString();
                Fila["PARTIDA"] = Cmb_Partidas_Generales.SelectedItem.Text;
                Fila["CONCEPTO_ID"] = Cmb_Conceptos.SelectedValue.ToString();
                Session["Dt_Consulta_Partidas_Proveedores"] = Dt_Consulta_Partidas_Proveedores;
                Dt_Consulta_Partidas_Proveedores.Rows.Add(Fila);
                Dt_Consulta_Partidas_Proveedores.AcceptChanges();
                Grid_Partidas_Generales.DataSource = Dt_Consulta_Partidas_Proveedores;
                Session["Dt_Consulta_Partidas_Proveedores"] = Dt_Consulta_Partidas_Proveedores;
                Grid_Partidas_Generales.DataBind();
                //Agregamos el Concepto al Grid de Conceptos 
                Agregar_Concepto_Proveedor();
            }
        }
        else
        {
            Dt_Consulta_Partidas_Proveedores.Columns.Add("PARTIDA_GENERICA_ID", typeof(System.String));
            Dt_Consulta_Partidas_Proveedores.Columns.Add("PARTIDA", typeof(System.String));
            Dt_Consulta_Partidas_Proveedores.Columns.Add("CONCEPTO_ID", typeof(System.String));
            Session["Dt_Consulta_Partidas_Proveedores"] = Dt_Consulta_Partidas_Proveedores;
            DataRow[] Fila_Nueva;
            Fila_Nueva = Dt_Consulta_Partidas_Proveedores.Select("PARTIDA_GENERICA_ID='" + Cmb_Partidas_Generales.SelectedValue.Trim() + "'");
            if (Fila_Nueva.Length > 0)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "+ La partida ya fué agregada";
            }
            else
            {
                DataRow Fila = Dt_Consulta_Partidas_Proveedores.NewRow();
                Fila["PARTIDA_GENERICA_ID"] = Cmb_Partidas_Generales.SelectedValue.ToString();
                Fila["PARTIDA"] = Cmb_Partidas_Generales.SelectedItem.Text;
                Fila["CONCEPTO_ID"] = Cmb_Conceptos.SelectedValue.ToString();
                Session["Dt_Consulta_Partidas_Proveedores"] = Dt_Consulta_Partidas_Proveedores;
                Dt_Consulta_Partidas_Proveedores.Rows.Add(Fila);
                Dt_Consulta_Partidas_Proveedores.AcceptChanges();
                Grid_Partidas_Generales.DataSource = Dt_Consulta_Partidas_Proveedores;
                Session["Dt_Consulta_Partidas_Proveedores"] = Dt_Consulta_Partidas_Proveedores;
                Grid_Partidas_Generales.DataBind();
                Agregar_Concepto_Proveedor();
            }

        }
        Cmb_Partidas_Generales.SelectedIndex = 0;
    }

    protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
    {
        Div_Busqueda_Avanzada.Visible = true;
    }

    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            
            // Consulta_Proveedores(); //Consultar los proveedores que coincidan con el nombre porporcionado por el usuario
            Cls_Cat_Com_Proveedores_Negocio RS_Consulta_Cat_Com_Proveedores = new Cls_Cat_Com_Proveedores_Negocio(); //Variable de conexión hacia la capa de Negocios
            DataTable Dt_Proveedores; //Variable que obtendrá los datos de la consulta 
            if (Txt_Busqueda_Nombre_Comercial.Text != "")
            {
                RS_Consulta_Cat_Com_Proveedores.P_Nombre_Comercial = Txt_Busqueda_Nombre_Comercial.Text;
            }
            if (Txt_Busqueda_Padron_Proveedor.Text.Trim() != String.Empty)
            {
                RS_Consulta_Cat_Com_Proveedores.P_Proveedor_ID = String.Format("{0:0000000000}", Convert.ToInt32(Txt_Busqueda_Padron_Proveedor.Text.Trim()));
            }
            if (Txt_Busqueda_Razon_Social.Text.Trim() != String.Empty)
            {
                RS_Consulta_Cat_Com_Proveedores.P_Razon_Social = Txt_Busqueda_Razon_Social.Text.Trim();
            }
            if (Txt_Busqueda_RFC.Text.Trim() != String.Empty)
            {
                RS_Consulta_Cat_Com_Proveedores.P_RFC = Txt_Busqueda_RFC.Text.Trim();
            }
            if (Cmb_Busqueda_Estatus.SelectedIndex != 0)
            {
                RS_Consulta_Cat_Com_Proveedores.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();
            }
            //Consulta los Proveedores con sus datos generales
            Dt_Proveedores = RS_Consulta_Cat_Com_Proveedores.Consulta_Avanzada_Proveedor();
            if (Dt_Proveedores.Rows.Count != 0)
            {
                Grid_Proveedores.DataSource = Dt_Proveedores;
                Grid_Proveedores.DataBind();
                Session["Dt_Proveedores"] = Dt_Proveedores;
            }
            else
            {
                Grid_Proveedores.EmptyDataText = "No se han encontrado registros.";
                //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
                Grid_Proveedores.DataSource = new DataTable();
                Grid_Proveedores.DataBind();
            }


            Limpiar_Controles_Busqueda_Avanzada(); //Limpia los controles de la forma
            Limpiar_Componentes();
            //Si no se encontraron Proveedores con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
            Btn_Salir.ToolTip = "Regresar";
            if (Grid_Proveedores.Rows.Count <= 0)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron proveedores con el nombre proporcionado <br />";
                Div_Proveedores.Visible = false;
            }
            else
            {
                Div_Proveedores.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        Div_Busqueda_Avanzada.Visible = false;
    }

    protected void Btn_Limpiar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Limpiar_Controles_Busqueda_Avanzada();
    }

    protected void Btn_Cerrar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        Div_Busqueda_Avanzada.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Limpiar_Controles_Busqueda_Avanzada();
    }


    protected void Chk_Fisica_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Fisica.Checked == true)
            Chk_Moral.Checked = false;

    }

    protected void Chk_Moral_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Moral.Checked == true)
            Chk_Fisica.Checked = false;
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
