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
using Presidencia.Operacion_Recolecciones.Negocio;
using Presidencia.Operacion_Arqueos.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Predial_Frm_Ope_Pre_Arqueos : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 29/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {

                Configuracion_Acceso("Frm_Ope_Pre_Arqueos.aspx");
                Configuracion_Formulario(true);
                //Llenar_Combo_Numeros_Caja();
                //Llenar_Tabla_Arqueos();
                //Llenar_Tabla_Recolecciones();
                Arqueo_Inicio();
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos
    ///***************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Arqueo_Inicio
    ///DESCRIPCIÓN          : Metodo con el que iniciara la carga de los datos del formulario
    ///PARAMETROS           :
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 16/Octubres/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///***************************************************************************************************
    private void Arqueo_Inicio() {
        Cls_Ope_Pre_Arqueos_Negocio Arqueos_Negocio = new Cls_Ope_Pre_Arqueos_Negocio();
        DataTable Dt_Datos = new DataTable();
        try
        {
            Arqueos_Negocio.P_No_Empleado =  Cls_Sessiones.Empleado_ID;
            Dt_Datos = Arqueos_Negocio.Consultar_Datos_Arqueo_Todos().Tables[0];
            Hf_No_Turno.Value = "";
            Hf_Caja_ID.Value = "";
            Hf_No_Caja.Value = "";
            if(Dt_Datos.Columns .Count > 0){
                if (Dt_Datos.Rows.Count > 0)
                {
                    ////Hf_No_Empleado.Value = Dt_Datos.Rows[0]["NO_EMPLEADO"].ToString();
                    Hf_No_Turno.Value = Dt_Datos.Rows[0]["NO_TURNO"].ToString();
                    Hf_Caja_ID.Value  = Dt_Datos.Rows[0]["CAJA_ID"].ToString();
                    Hf_No_Caja.Value = Dt_Datos.Rows[0]["NO_CAJA"].ToString();
                    Llenar_Tabla_Arqueos();
                    Llenar_Tabla_Recolecciones(Hf_No_Turno.Value);
                    Div_Arqueos.Visible = true;
                    Div_Arqueos_Detalles.Visible = false ;
                }  
            }
            //Llenar_Combo_Numeros_Caja();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }   

    ///****************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        Btn_Nuevo.Visible = true;
        Btn_Nuevo.ToolTip = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.ToolTip = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        
        //Cmb_Numero_Caja.Enabled = !Estatus;
        Txt_No_Caja.Enabled = !Estatus;
        Txt_Modulo.Enabled = !Estatus;
        Txt_Cajero.Enabled = !Estatus;
        Txt_Realizo.Enabled = !Estatus;
        Txt_Total_Cobrado.Enabled = !Estatus; 
        Txt_Total_Recolectado.Enabled = !Estatus;
        Txt_Fondo_Inicial.Enabled = !Estatus;
        Txt_Total_Tarjeta.Enabled = !Estatus;
        Txt_Total_Cheques.Enabled = !Estatus;
        Txt_Total_Transferencias.Enabled = !Estatus;
        Txt_Comentarios.Enabled = !Estatus;
        Txt_Denom_1_Peso.Enabled = !Estatus;
        Txt_Denom_10_Cent.Enabled = !Estatus;
        Txt_Denom_10_Pesos.Enabled = !Estatus;
        Txt_Denom_100_Pesos.Enabled = !Estatus;
        Txt_Denom_1000_Pesos.Enabled = !Estatus;
        Txt_Denom_2_Pesos.Enabled = !Estatus;
        Txt_Denom_20_Cent.Enabled = !Estatus;
        Txt_Denom_20_Pesos.Enabled = !Estatus;
        Txt_Denom_200_Pesos.Enabled = !Estatus;
        Txt_Denom_5_Pesos.Enabled = !Estatus;
        Txt_Denom_50_Cent.Enabled = !Estatus;
        Txt_Denom_50_Pesos.Enabled = !Estatus;
        Txt_Denom_500_Pesos.Enabled = !Estatus;
        Txt_Total.Enabled = !Estatus;
        Btn_Realizo.Enabled = !Estatus;
        Txt_Total_Caja.Enabled = !Estatus;
        Txt_Total_General.Enabled = !Estatus;

        Grid_Arqueos.SelectedIndex = (-1);
        Grid_Arqueos.SelectedIndex = (-1);
        Btn_Buscar.Enabled = Estatus;
        Txt_Busqueda.Enabled = Estatus;

        Div_Arqueos.Visible = Estatus;
        Div_Arqueos_Detalles.Visible = !Estatus;
        
    }

    ///****************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Habilitar_Denominaciones() 
    {
        Txt_Denom_1_Peso.Enabled = true;
        Txt_Denom_10_Cent.Enabled = true;
        Txt_Denom_10_Pesos.Enabled = true;
        Txt_Denom_100_Pesos.Enabled = true;
        Txt_Denom_1000_Pesos.Enabled = true;
        Txt_Denom_2_Pesos.Enabled = true;
        Txt_Denom_20_Cent.Enabled = true;
        Txt_Denom_20_Pesos.Enabled = true;
        Txt_Denom_200_Pesos.Enabled = true;
        Txt_Denom_5_Pesos.Enabled = true;
        Txt_Denom_50_Cent.Enabled = true;
        Txt_Denom_50_Pesos.Enabled = true;
        Txt_Denom_500_Pesos.Enabled = true;
        //Cmb_Numero_Caja.Enabled = true;
        //Txt_No_Caja.Enabled = true;
        Txt_Comentarios.Enabled = true;
        Btn_Realizo.Enabled = true;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Caja_Modulo
    ///DESCRIPCIÓN: Metodo que llena el Combo de Modulos con los modulos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Caja_Modulo()
    {
        try
        {
            Cls_Ope_Pre_Recolecciones_Negocio Modulo = new Cls_Ope_Pre_Recolecciones_Negocio();
            Modulo.P_Caja_ID = Hf_Caja_ID.Value.Trim(); 
            DataSet Tabla = Modulo.Consultar_Modulos();
            Txt_Modulo.Text = Tabla.Tables[0].Rows[0]["CLAVE"].ToString() + " " + Tabla.Tables[0].Rows[0]["UBICACION"].ToString();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Cajeros
    ///DESCRIPCIÓN: Metodo que llena el Combo de Cajeros con los cajeros existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Caja_Cajero()
    {
        try
        {
            Cls_Ope_Pre_Recolecciones_Negocio Cajero = new Cls_Ope_Pre_Recolecciones_Negocio();
            Cajero.P_Cajero_ID = Cls_Sessiones.Empleado_ID.Trim();
            DataTable Dt_Cajeros = Cajero.Llenar_Combo_Cajeros();
            if (Dt_Cajeros.Rows.Count > 0)
            {
                //Muestra todos los datos que tiene el folio que proporciono el usuario
                foreach (DataRow Registro in Dt_Cajeros.Rows)
                {
                    //Txt_Caja_ID.Text = Registro[Ope_Caj_Turnos.Campo_Caja_Id].ToString();
                    Txt_Cajero.Text = Registro["NOMBRE"].ToString();
                    //Hf_Realizo.Value = Registro[Cat_Empleados.Campo_Empleado_ID].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Caja_Modulo
    ///DESCRIPCIÓN: Metodo que llena el Combo de Modulos con los modulos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Caja_Apertura()
    {
        try
        {
            Cls_Ope_Pre_Arqueos_Negocio Apertura = new Cls_Ope_Pre_Arqueos_Negocio();
            Apertura.P_Caja_ID = Hf_Caja_ID.Value.Trim();
            DataSet Turnos = Apertura.Consultar_Turnos();
            Hf_No_Turno.Value = Turnos.Tables[0].Rows[0]["NO_TURNO"].ToString();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Caja_Modulo
    ///DESCRIPCIÓN: Metodo que llena el Combo de Modulos con los modulos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Caja_Total_Recolectado()
    {
        try
        {
            Cls_Ope_Pre_Arqueos_Negocio Recolecciones = new Cls_Ope_Pre_Arqueos_Negocio();
            Recolecciones.P_No_Turno = Hf_No_Turno.Value.Trim();
            DataSet Ds_Total_Recolectado = Recolecciones.Consultar_Total_Recolectado();
            String Total = Ds_Total_Recolectado.Tables[0].Rows[0]["TOTAL_RECOLECTADO"].ToString();
            Txt_Total_Recolectado.Text = Decimal.Round(Convert.ToDecimal(Total), 2).ToString("#,###,###,##0.00");
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Caja_Modulo
    ///DESCRIPCIÓN: Metodo que llena el Combo de Modulos con los modulos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Caja_Total_Cobrado()
    {
        try
        {
            Cls_Ope_Pre_Arqueos_Negocio Pagos = new Cls_Ope_Pre_Arqueos_Negocio();
            Pagos.P_No_Turno = Hf_No_Turno .Value .Trim();
            DataTable Dt_Total_Cobrado = Pagos.Consultar_Total_Cobrado();
            String Total = Dt_Total_Cobrado.Rows[0]["TOTAL_COBRADO"].ToString();
            Txt_Total_Cobrado.Text = Decimal.Round(Convert.ToDecimal(Total),2).ToString("$#,###,###,##0.00");
            Total = Dt_Total_Cobrado.Rows[0]["TOTAL_TARJETA"].ToString();
            Txt_Total_Tarjeta.Text = Decimal.Round(Convert.ToDecimal(Total), 2).ToString("$#,###,###,##0.00");
            Total = Dt_Total_Cobrado.Rows[0]["TOTAL_CHEQUE"].ToString();
            Txt_Total_Cheques.Text = Decimal.Round(Convert.ToDecimal(Total), 2).ToString("$#,###,###,##0.00");
            Total = Dt_Total_Cobrado.Rows[0]["TOTAL_TRANSFERENCIA"].ToString();
            Txt_Total_Transferencias.Text = Decimal.Round(Convert.ToDecimal(Total), 2).ToString("$#,###,###,##0.00");
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Caja_Modulo
    ///DESCRIPCIÓN: Metodo que llena el Combo de Modulos con los modulos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Caja_Fondo_Inicial()
    {
        try
        {
            Cls_Ope_Pre_Arqueos_Negocio Fondo_Inical = new Cls_Ope_Pre_Arqueos_Negocio();
            Fondo_Inical.P_No_Turno = Hf_No_Turno.Value;
            DataSet Ds_Fondo_Inicial = Fondo_Inical.Consultar_Fondo_Inicial();
            if (Ds_Fondo_Inicial.Tables[0].Rows.Count > 0)
            {
                String Total = Ds_Fondo_Inicial.Tables[0].Rows[0]["FONDO_INICIAL"].ToString();
                Txt_Fondo_Inicial.Text = Decimal.Round(Convert.ToDecimal(Total), 2).ToString("$#,###,###,##0.00");
            }
            else
            {
                Txt_Fondo_Inicial.Text = "$0.00";
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Caja_Modulo
    ///DESCRIPCIÓN: Metodo que llena el Combo de Modulos con los modulos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Calcular_Arqueo()
    {
        Double Cobrado = Convert.ToDouble(Txt_Total_Cobrado.Text.Trim().ToString().Replace("$","").Trim());
        Double Recolectado = Convert.ToDouble(Txt_Total_Recolectado.Text.Trim().ToString().Replace("$", "").Trim());
        Double Fondo_Inicial = Convert.ToDouble(Txt_Fondo_Inicial.Text.Trim().ToString().Replace("$", "").Trim());
        Decimal Arqueo = Convert.ToDecimal(((Cobrado + Recolectado) - Fondo_Inicial)); 
    }

    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Modulo.Text = "";
        Txt_Realizo.Text = "";
        Txt_Total_Recolectado.Text = "";
        Txt_Total_Cobrado.Text = "";
        Txt_Fondo_Inicial.Text = "";
        Txt_Total_Tarjeta.Text = "";
        Txt_Total_Cheques.Text = "";
        Txt_Total_Transferencias.Text = "";
        Txt_Comentarios.Text = "";
        Txt_Total_Caja.Text = "";
        Txt_Total_General.Text = "";
        Txt_Diferencia.Text = "";
        //Hf_No_Turno.Value = "";
        //Hf_Realizo.Value = "";

        Txt_Denom_1_Peso.Text = "0";
        Txt_Denom_10_Cent.Text = "0";
        Txt_Denom_10_Pesos.Text = "0";
        Txt_Denom_100_Pesos.Text = "0";
        Txt_Denom_1000_Pesos.Text = "0";
        Txt_Denom_2_Pesos.Text = "0";
        Txt_Denom_20_Cent.Text = "0";
        Txt_Denom_20_Pesos.Text = "0";
        Txt_Denom_200_Pesos.Text = "0";
        Txt_Denom_5_Pesos.Text = "0";
        Txt_Denom_50_Cent.Text = "0";
        Txt_Denom_50_Pesos.Text = "0";
        Txt_Denom_500_Pesos.Text = "0";
        Txt_Total.Text = "0.00";
        
        Txt_Cajero.Text = "";
        Txt_No_Caja.Text = "";
        Txt_No_Empleado.Text = "";
       // Cmb_Numero_Caja.SelectedIndex = 0;
        //Grid_Arqueos.DataSource = new DataTable();
        //Grid_Arqueos.DataBind();
    }

    ///***************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Generar_Dt_Arqueos
    ///DESCRIPCIÓN          : Metodo para generar el datatable con los datos del arqueo que se esta dando de alta
    ///PARAMETROS           :
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 16/Octubres/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///***************************************************************************************************
    protected void Generar_Dt_Arqueos(string No_Arqueo)
    {
        DataTable Dt_Arqueos = new DataTable();
        DataRow Fila_Arqueo;

        try
        {
            Dt_Arqueos.Columns.Add("NO_TURNO");
            Dt_Arqueos.Columns.Add("CAJA");
            Dt_Arqueos.Columns.Add("CAJERO");
            Dt_Arqueos.Columns.Add("FECHA");
            Dt_Arqueos.Columns.Add("TOTAL_RECOLECTADO", typeof(System.Double));
            Dt_Arqueos.Columns.Add("NO_ARQUEO");
            Dt_Arqueos.Columns.Add("DENOM_1000_PESOS");
            Dt_Arqueos.Columns.Add("DENOM_10_CENT");
            Dt_Arqueos.Columns.Add("DENOM_1_PESO");
            Dt_Arqueos.Columns.Add("DENOM_10_PESOS");
            Dt_Arqueos.Columns.Add("DENOM_20_CENT");
            Dt_Arqueos.Columns.Add("DENOM_2_PESOS");
            Dt_Arqueos.Columns.Add("DENOM_20_PESOS");
            Dt_Arqueos.Columns.Add("DENOM_200_PESOS");
            Dt_Arqueos.Columns.Add("DENOM_50_CENT");
            Dt_Arqueos.Columns.Add("DENOM_5_PESOS");
            Dt_Arqueos.Columns.Add("DENOM_500_PESOS");
            Dt_Arqueos.Columns.Add("DENOM_50_PESOS");
            Dt_Arqueos.Columns.Add("DENOM_100_PESOS");
            Dt_Arqueos.Columns.Add("REALIZO");
            Dt_Arqueos.Columns.Add("DIFERENCIA", typeof(System.Double));

            Fila_Arqueo = Dt_Arqueos.NewRow();
            Fila_Arqueo["NO_TURNO"] = Hf_No_Turno.Value.Trim();
            Fila_Arqueo["CAJA"] = Txt_No_Caja.Text.Trim();
            Fila_Arqueo["CAJERO"] = Txt_Cajero.Text.Trim();
            Fila_Arqueo["FECHA"] = string.Format("{0:MM/dd/yyyy hh:mm:ss}", DateTime.Now.Date);
            Fila_Arqueo["TOTAL_RECOLECTADO"] = Convert.ToDouble(Txt_Total_Recolectado.Text.Trim().Replace("$", ""));
            Fila_Arqueo["NO_ARQUEO"] = No_Arqueo;
            Fila_Arqueo["DENOM_1000_PESOS"] = Txt_Denom_1000_Pesos.Text.Trim();
            Fila_Arqueo["DENOM_10_CENT"] = Txt_Denom_10_Cent.Text.Trim();
            Fila_Arqueo["DENOM_1_PESO"] = Txt_Denom_1_Peso.Text.Trim();
            Fila_Arqueo["DENOM_10_PESOS"] = Txt_Denom_10_Pesos.Text.Trim();
            Fila_Arqueo["DENOM_20_CENT"] = Txt_Denom_20_Cent.Text.Trim();
            Fila_Arqueo["DENOM_2_PESOS"] = Txt_Denom_2_Pesos.Text.Trim();
            Fila_Arqueo["DENOM_20_PESOS"] = Txt_Denom_20_Pesos.Text.Trim();
            Fila_Arqueo["DENOM_200_PESOS"] = Txt_Denom_200_Pesos.Text.Trim();
            Fila_Arqueo["DENOM_50_CENT"] = Txt_Denom_50_Cent.Text.Trim();
            Fila_Arqueo["DENOM_5_PESOS"] = Txt_Denom_5_Pesos.Text.Trim();
            Fila_Arqueo["DENOM_500_PESOS"] = Txt_Denom_500_Pesos.Text.Trim();
            Fila_Arqueo["DENOM_50_PESOS"] = Txt_Denom_50_Pesos.Text.Trim();
            Fila_Arqueo["DENOM_100_PESOS"] = Txt_Denom_100_Pesos.Text.Trim();
            Fila_Arqueo["REALIZO"] = Txt_Realizo.Text.Trim();
            Fila_Arqueo["DIFERENCIA"] = Convert.ToDouble(Hfd_Diferencia.Value);
            Dt_Arqueos.Rows.Add(Fila_Arqueo);
            Session["Dt_Arqueos"] = Dt_Arqueos;
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    ///***************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Suma_Total_Caja
    ///DESCRIPCIÓN          : Metodo para generar la suma del total de cajas
    ///PARAMETROS           :
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 16/Octubres/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///***************************************************************************************************
    protected void Suma_Total_Caja()
    {
        double Total = 0.00;
        double Total_Parcial = 0.00;
        Double Total_Cobrado = 0.00;
        Double Total_Tarjeta = 0.00;
        Double Total_Cheque = 0.00;
        Double Total_Transferencia = 0.00;
        Double Fondo_Inicial = 0.00;
        try
        {
            Total_Cobrado = Convert.ToDouble(string.IsNullOrEmpty(Txt_Total_Cobrado.Text.Trim().Replace("$", "")) ? "0" : Txt_Total_Cobrado.Text.Trim().Replace("$", ""));
            Total_Parcial = Convert.ToDouble(string.IsNullOrEmpty(Txt_Total_Recolectado.Text.Trim().Replace("$", "")) ? "0" : Txt_Total_Recolectado.Text.Trim().Replace("$", ""));
            Fondo_Inicial = Convert.ToDouble(string.IsNullOrEmpty(Txt_Fondo_Inicial.Text.Trim().Replace("$", "")) ? "0" : Txt_Fondo_Inicial.Text.Trim().Replace("$", ""));
            Total_Tarjeta = Convert.ToDouble(string.IsNullOrEmpty(Txt_Total_Tarjeta.Text.Trim().Replace("$", "")) ? "0" : Txt_Total_Tarjeta.Text.Trim().Replace("$", ""));
            Total_Cheque = Convert.ToDouble(string.IsNullOrEmpty(Txt_Total_Cheques.Text.Trim().Replace("$", "")) ? "0" : Txt_Total_Cheques.Text.Trim().Replace("$", ""));
            Total_Transferencia = Convert.ToDouble(string.IsNullOrEmpty(Txt_Total_Transferencias.Text.Trim().Replace("$", "")) ? "0" : Txt_Total_Transferencias.Text.Trim().Replace("$", ""));
            Total = Total_Cobrado - Total_Parcial;
            Txt_Total_Caja.Text = string.Format("{0:#,###,##0.00}", Total);

            Total = (Total_Cobrado + Fondo_Inicial+Total_Tarjeta+Total_Cheque+Total_Transferencia) - Total_Parcial;
            Txt_Total_General.Text = string.Format("{0:#,###,##0.00}", Total);

            Txt_Diferencia.Text = string.Format("{0:#,###,##0.00}",Convert.ToDouble(Txt_Total.Text) - Convert.ToDouble(Txt_Total_Caja.Text));
            Hfd_Diferencia.Value = string.Format("{0:#,###,##0.00}", Convert.ToDouble(Txt_Total.Text) - Convert.ToDouble(Txt_Total_Caja.Text));
            if (Convert.ToDouble(Txt_Diferencia.Text) < 0)
            {
                Lbl_Diferencia.Text = "Faltante";
                Lbl_Diferencia.Style.Add("color", "red");
                Txt_Diferencia.Style.Add("color", "red");
            }
            else
            {
                Lbl_Diferencia.Text = "Sobrante";
                Lbl_Diferencia.Style.Add("color", "green");
                Txt_Diferencia.Style.Add("color", "green");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar la suma del total en caja Error:[" + Ex.Message + "]");
        }
    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación que se vea afectada en la basae de datos.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        //if (Cmb_Numero_Caja.SelectedIndex == 0)
        //{
        //    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Numero de Caja.";
        //    Validacion = false;
        //}
        if (Txt_Cajero.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Cajero.";
            Validacion = false;
        }
        if (Txt_Modulo.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Modulo.";
            Validacion = false;
        }
        if (Txt_Realizo.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Es necesario estar Logeado.";
            Validacion = false;
        }
        if (Txt_Total_Cobrado.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Total Cobrado.";
            Validacion = false;
        }
        if (Txt_Total_Recolectado.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Total Recolectado.";
            Validacion = false;
        }
        if (Txt_Fondo_Inicial.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Fondo Inicial.";
            Validacion = false;
        }
        if (Txt_Comentarios.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir los Comentarios.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación que se vea afectada en la basae de datos.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Autenticacion()
    {
        if (Txt_No_Empleado.Text.Trim().Length == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Autenticación de Usuarios", "alert('Teclea tu número de empleado');", true);
            return false;       
        }
        return true;
    }

    #endregion

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Recolecciones
    ///DESCRIPCIÓN: Llena la tabla de Recolecciones
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_View
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Recolecciones(String Turno)
    {
        DataTable dt = new DataTable();
        try
        {
            Cls_Ope_Pre_Arqueos_Negocio Recolecciones = new Cls_Ope_Pre_Arqueos_Negocio();
            Recolecciones.P_No_Turno = Turno;
            dt = Recolecciones.Consultar_Recolecciones();
            Grid_Recolecciones.Columns[4].Visible = true;
            Grid_Recolecciones.DataSource = dt;
            Grid_Recolecciones.DataBind();
            Grid_Recolecciones.Columns[4].Visible = false;
            Session["Dt_Recolecciones"] = dt;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Recolecciones
    ///DESCRIPCIÓN: Llena la tabla de Recolecciones
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_View
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Arqueos()
    {
        try
        {
            Cls_Ope_Pre_Arqueos_Negocio Arqueos = new Cls_Ope_Pre_Arqueos_Negocio();
            Arqueos.P_No_Turno = Hf_No_Turno.Value.Trim();
            Grid_Arqueos.Columns[3].Visible = true;
            Grid_Arqueos.Columns[4].Visible = true;
            Grid_Arqueos.DataSource = Arqueos.Consultar_Arqueos();
            Grid_Arqueos.DataBind();
            Grid_Arqueos.Columns[3].Visible = false;
            Grid_Arqueos.Columns[4].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Recolecciones_Busqueda
    ///DESCRIPCIÓN: Llena la tabla de Recolecciones de auerdo a la busqueda introducida.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Arqueos_Busqueda(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Arqueos_Negocio Arqueos = new Cls_Ope_Pre_Arqueos_Negocio();
            Arqueos.P_No_Arqueo = Txt_Busqueda.Text.ToUpper().Trim();
            Arqueos.P_Cajero_ID = Txt_Busqueda.Text.ToUpper().Trim();
            Arqueos.P_Realizo = Txt_Busqueda.Text.ToUpper().Trim();
            Grid_Arqueos.DataSource = Arqueos.Consultar_Arqueos_Busqueda();
            Grid_Arqueos.PageIndex = Pagina;
            Grid_Arqueos.Columns[3].Visible = true;
            Grid_Arqueos.Columns[4].Visible = true;
            Grid_Arqueos.DataBind();
            Grid_Arqueos.Columns[3].Visible = false;
            Grid_Arqueos.Columns[4].Visible = false;
            Txt_Busqueda.Text = "";
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///***************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Generar_Datos_Reporte
    ///DESCRIPCIÓN          : Función para obtener y acomodar los datos que mostraremos en el reporte
    ///PARAMETROS           :
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 1/Octubre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///***************************************************************************************************
    private DataSet Generar_Datos_Reporte() {
        Cls_Ope_Pre_Arqueos_Negocio Arqueos_Negocio = new Cls_Ope_Pre_Arqueos_Negocio(); //Hacemos la instancia a la clase de negociosw
        Ds_Ope_Pre_Arqueos Ds_Arqueos = new Ds_Ope_Pre_Arqueos();
        
        //Declaramos los datatable que utilizaremos para manejar los datos de los arqueos
        DataTable Dt_Datos_Arqueo = new DataTable();
        DataTable Dt_Datos_Entrega_Parciales = new DataTable();
        DataTable Dt_Formas_Pago = new DataTable();
        string Dependencia = string.Empty;
        String Caja_General = string.Empty;
        DataTable Dt_Datos_Generales = new DataTable();
        DataTable Dt_Datos_Arqueos = new DataTable();
        DataRow Fila_General;
        DataRow Fila_Arqueo;
        DataTable Dt_Recibos = new DataTable();
        string Recibo_Inicial = string.Empty;
        string Recibo_Final = string.Empty;
        string Cajero_General = string.Empty;
        string No_Caja = string.Empty;
        string No_Arqueo = string.Empty;
        double TOTAL = 0.00;
        //double INGRESO_TOTAL = 0.00;
        DataRow Fila_Forma;
        double Total_Parcial = 0.00;
        string Parcial = string.Empty;

        try {
            //obtenemos los datos
            Arqueos_Negocio.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado;
            Dependencia = Arqueos_Negocio.Consultar_Dependencia();
            Dt_Datos_Arqueo = (DataTable)Session["Dt_Arqueos"];
            Dt_Datos_Entrega_Parciales = (DataTable)Session["Dt_Recolecciones"];
            Cajero_General = Arqueos_Negocio.Consultar_Cajero_General();
            //creamos los nuevos datatable donde manejaremos la informacion
            Dt_Datos_Generales.Columns.Add("No_Caja");
            Dt_Datos_Generales.Columns.Add("Departamento");
            Dt_Datos_Generales.Columns.Add("Nombre_Cajero");
            Dt_Datos_Generales.Columns.Add("Fecha");
            Dt_Datos_Generales.Columns.Add("Hora");
            Dt_Datos_Generales.Columns.Add("Recibo_Inicial");
            Dt_Datos_Generales.Columns.Add("Recibo_Final");
            Dt_Datos_Generales.Columns.Add("Total_Entregas_Parciales", typeof(System.Double));
            Dt_Datos_Generales.Columns.Add("Cajero_General");
            Dt_Datos_Generales.Columns.Add("Diferencia", typeof(System.Double));

            Dt_Datos_Arqueos.Columns.Add("No");
            Dt_Datos_Arqueos.Columns.Add("Denominacion");
            Dt_Datos_Arqueos.Columns.Add("Subtotal", typeof(System.Double));

            foreach (DataRow Registro in Dt_Datos_Arqueo.Rows)
            {
                Arqueos_Negocio.P_No_Turno = Registro["NO_TURNO"].ToString();
                Dt_Recibos = Arqueos_Negocio.Consultar_Datos_Recibos().Tables[0];
                if (Dt_Recibos.Rows.Count > 0) {
                    Recibo_Final = Dt_Recibos.Rows[0]["RECIBO_FINAL"].ToString();
                    Recibo_Inicial = Dt_Recibos.Rows[0]["RECIBO_INICIAL"].ToString();
                }

                Fila_General = Dt_Datos_Generales.NewRow();
                Fila_General["No_Caja"] = Registro["CAJA"].ToString();
                No_Caja = Registro["CAJA"].ToString();
                Fila_General["Departamento"] = Dependencia;
                Fila_General["Nombre_Cajero"] = Registro["CAJERO"].ToString();
                Fila_General["Fecha"] = Registro["FECHA"].ToString().Substring(0,10);
                Fila_General["Hora"] = Registro["FECHA"].ToString().Substring(11);
                Fila_General["Recibo_Inicial"] = Recibo_Inicial ;
                Fila_General["Recibo_Final"] = Recibo_Final;
                Fila_General["Total_Entregas_Parciales"] = Registro["TOTAL_RECOLECTADO"].ToString();
                Parcial = Registro["TOTAL_RECOLECTADO"].ToString();
                //Fila_General["Total_Entregas_Parciales"] = Txt_Total_Recolectado.Text.Trim().Replace("$", "");
                Fila_General["Cajero_General"] = Registro["REALIZO"].ToString();
                Fila_General["Diferencia"] = Registro["DIFERENCIA"].ToString();
                Dt_Datos_Generales.Rows.Add(Fila_General);

                if (!Registro["DENOM_1000_PESOS"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_1000_PESOS"].ToString();
                    Fila_Arqueo["Denominacion"] = "BILLETE DE $ 1,000.00";
                    Fila_Arqueo["Subtotal"] =  Convert.ToInt32(Registro["DENOM_1000_PESOS"].ToString()) *1000;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                if (!Registro["DENOM_500_PESOS"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_500_PESOS"].ToString();
                    Fila_Arqueo["Denominacion"] = "BILLETE DE $ 500.00";
                    Fila_Arqueo["Subtotal"] =Convert.ToInt32(Registro["DENOM_500_PESOS"].ToString()) * 500;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                if (!Registro["DENOM_200_PESOS"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_200_PESOS"].ToString();
                    Fila_Arqueo["Denominacion"] = "BILLETE DE $ 200.00";
                    Fila_Arqueo["Subtotal"] =  Convert.ToInt32(Registro["DENOM_200_PESOS"].ToString()) * 200;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                if (!Registro["DENOM_100_PESOS"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_100_PESOS"].ToString();
                    Fila_Arqueo["Denominacion"] = "BILLETE DE $ 100.00";
                    Fila_Arqueo["Subtotal"] = Convert.ToInt32(Registro["DENOM_100_PESOS"].ToString()) * 100;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                if (!Registro["DENOM_50_PESOS"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_50_PESOS"].ToString();
                    Fila_Arqueo["Denominacion"] = "BILLETE DE $ 50.00";
                    Fila_Arqueo["Subtotal"] = Convert.ToInt32(Registro["DENOM_50_PESOS"].ToString()) * 50;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                if (!Registro["DENOM_20_PESOS"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_20_PESOS"].ToString();
                    Fila_Arqueo["Denominacion"] = "BILLETE DE $ 20.00";
                    Fila_Arqueo["Subtotal"] = Convert.ToInt32(Registro["DENOM_20_PESOS"].ToString()) * 20;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                if (!Registro["DENOM_10_PESOS"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_10_PESOS"].ToString();
                    Fila_Arqueo["Denominacion"] = "MONEDA DE $ 10.00";
                    Fila_Arqueo["Subtotal"] = Convert.ToInt32(Registro["DENOM_10_PESOS"].ToString()) * 10;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                if (!Registro["DENOM_5_PESOS"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_5_PESOS"].ToString();
                    Fila_Arqueo["Denominacion"] = "MONEDA DE $ 5.00";
                    Fila_Arqueo["Subtotal"] = Convert.ToInt32(Registro["DENOM_5_PESOS"].ToString()) * 5;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                if (!Registro["DENOM_2_PESOS"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_2_PESOS"].ToString();
                    Fila_Arqueo["Denominacion"] = "MONEDA DE $ 2.00";
                    Fila_Arqueo["Subtotal"] = Convert.ToInt32(Registro["DENOM_2_PESOS"].ToString()) * 2;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                if (!Registro["DENOM_1_PESO"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_1_PESO"].ToString();
                    Fila_Arqueo["Denominacion"] = "MONEDA DE $ 1.00";
                    Fila_Arqueo["Subtotal"] = Convert.ToInt32(Registro["DENOM_1_PESO"].ToString()) * 1;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                if (!Registro["DENOM_50_CENT"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_50_CENT"].ToString();
                    Fila_Arqueo["Denominacion"] = "MONEDA DE $ 0.50";
                    Fila_Arqueo["Subtotal"] = Convert.ToInt32(Registro["DENOM_50_CENT"].ToString()) * 0.5;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                if (!Registro["DENOM_20_CENT"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_20_CENT"].ToString();
                    Fila_Arqueo["Denominacion"] = "MONEDA DE $ 0.20";
                    Fila_Arqueo["Subtotal"] = Convert.ToInt32(Registro["DENOM_20_CENT"].ToString()) * 0.2;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                if (!Registro["DENOM_10_CENT"].ToString().Equals("0"))
                {
                    Fila_Arqueo = Dt_Datos_Arqueos.NewRow();
                    Fila_Arqueo["No"] = Registro["DENOM_10_CENT"].ToString();
                    Fila_Arqueo["Denominacion"] = "MONEDA DE $ 0.10";
                    Fila_Arqueo["Subtotal"] = Convert.ToInt32(Registro["DENOM_10_CENT"].ToString()) * 0.1;
                    Dt_Datos_Arqueos.Rows.Add(Fila_Arqueo);
                }
                No_Arqueo = Registro["NO_ARQUEO"].ToString();
                Arqueos_Negocio.P_No_Arqueo = No_Arqueo;
                Dt_Formas_Pago = Arqueos_Negocio.Consultar_Formas_Pago().Tables[0];
            }

            //Fila_Forma = Dt_Formas_Pago.NewRow();
            //Fila_Forma["NOMBRE"] = "SUBTOTAL";
            //Fila_Forma["MONTO"] = Convert.ToDouble(Txt_Total_Caja.Text);
            //Dt_Formas_Pago.Rows.Add(Fila_Forma);

            if (Dt_Formas_Pago.Columns.Count > 0)
            {
                if (Dt_Formas_Pago.Rows.Count > 0) {
                    foreach (DataRow Dr_Formas in Dt_Formas_Pago.Rows)
                    {
                        //TOTAL = TOTAL + Convert.ToDouble((string.IsNullOrEmpty(Dr_Formas["MONTO"].ToString())) ? "0" : Dr_Formas["MONTO"].ToString());
                        if (Dr_Formas["NOMBRE"].ToString() == "EFECTIVO")
                        {
                            Dr_Formas["NOMBRE"] = "TOTAL " + Dr_Formas["NOMBRE"].ToString();
                            Dr_Formas["MONTO"] = Convert.ToDouble(Txt_Total_Caja.Text);
                        }
                        else
                        {
                            Dr_Formas["NOMBRE"] = "TOTAL " + Dr_Formas["NOMBRE"].ToString();
                            Dr_Formas["MONTO"] = Convert.ToDouble((string.IsNullOrEmpty(Dr_Formas["MONTO"].ToString())) ? "0" : Dr_Formas["MONTO"].ToString());
                        }
                    }
                }
            }
            

            //Parcial = Txt_Total_Recolectado.Text.Trim().Replace("$", "");
            //Parcial = Parcial.Replace(",", "");

            //Total_Parcial = Convert.ToDouble(Parcial);

            

            //Fila_Forma = Dt_Formas_Pago.NewRow();
            //Fila_Forma["NOMBRE"] = "INGRESO TOTAL";
            //Fila_Forma["MONTO"] = string.Format("{0:#,###,##0.00}", TOTAL);
            //Dt_Formas_Pago.Rows.Add(Fila_Forma);

            //creamos el dataset del reporte
            Dt_Datos_Generales.TableName = "Dt_Datos_Generales";
            Dt_Datos_Arqueos.TableName = "Dt_Arqueos";
            Dt_Formas_Pago.TableName = "Dt_Forma_Pago";
            Dt_Datos_Entrega_Parciales.TableName  = "Dt_Entregas_Parciales";
            Ds_Arqueos.Clear();
            Ds_Arqueos.Tables.Clear();
            Ds_Arqueos.Tables.Add(Dt_Datos_Generales.Copy());
            Ds_Arqueos.Tables.Add(Dt_Datos_Arqueos.Copy());
            Ds_Arqueos.Tables.Add(Dt_Formas_Pago.Copy());
            Ds_Arqueos.Tables.Add(Dt_Datos_Entrega_Parciales.Copy());
            
        }catch (Exception Ex){
            Lbl_Ecabezado_Mensaje.Text = "Error al generar los datos del reporte Error["+Ex.Message +"]";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Ds_Arqueos;
    }
    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Recolecciones_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Recolecciones
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Arqueos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Arqueos.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                //Session["Dt_Divisiones_Impuestos"] = null;
                String No_Arqueo = Grid_Arqueos.SelectedRow.Cells[1].Text;
                Cls_Ope_Pre_Arqueos_Negocio Arqueo = new Cls_Ope_Pre_Arqueos_Negocio();
                
                Arqueo.P_No_Arqueo = No_Arqueo;
                DataTable Dt_Arqueos = Arqueo.Consultar_Datos_Arqueos();
                string Turno = string.Empty;


                if (Dt_Arqueos.Rows.Count > 0)
                {
                    //Muestra todos los datos que tiene el arqueo que selecciono el empleado
                    foreach (DataRow Registro in Dt_Arqueos.Rows)
                    {
                        Hf_No_Arqueo.Value = Registro["NO_ARQUEO"].ToString();
                        Hf_Caja_ID.Value  = Registro["CAJA_ID"].ToString();
                        Txt_No_Caja.Text = Registro["CAJA"].ToString();
                        Txt_Modulo.Text = Registro["MODULO"].ToString();
                        Hf_Realizo.Value = Registro["REALIZO_ID"].ToString();
                        
                        Txt_Cajero.Text = Registro["CAJERO"].ToString();
                        Txt_Realizo.Text = Registro["REALIZO"].ToString();
                        Txt_Total_Cobrado.Text = Decimal.Round(Convert.ToDecimal(Registro["TOTAL_EFECTIVO"].ToString()), 2).ToString("$#,###,###,##0.00");
                        Txt_Total_Tarjeta.Text = Decimal.Round(Convert.ToDecimal(Registro["TOTAL_TARJETA"].ToString()), 2).ToString("$#,###,###,##0.00");
                        Txt_Total_Cheques.Text = Decimal.Round(Convert.ToDecimal(Registro["TOTAL_CHEQUES"].ToString()), 2).ToString("$#,###,###,##0.00");
                        Txt_Total_Transferencias.Text = Decimal.Round(Convert.ToDecimal(Registro["TOTAL_TRANSFERENCIAS"].ToString()), 2).ToString("$#,###,###,##0.00");
                        Txt_Total_Recolectado.Text = Decimal.Round(Convert.ToDecimal(Registro["TOTAL_RECOLECTADO"].ToString()), 2).ToString("$#,###,###,##0.00");
                        Llenar_Caja_Total_Recolectado();
                        Registro["TOTAL_RECOLECTADO"] =  Txt_Total_Recolectado.Text.Trim().Replace("$", "");
                        Txt_Comentarios.Text = Registro["COMENTARIOS"].ToString();
                        Txt_Fondo_Inicial.Text = Decimal.Round(Convert.ToDecimal(Registro["FONDO_INICIAL"].ToString()), 2).ToString("$#,###,###,##0.00");
                        Txt_Denom_1_Peso.Text = Registro["DENOM_1_PESO"].ToString();
                        Txt_Denom_10_Cent.Text = Registro["DENOM_10_CENT"].ToString();
                        Txt_Denom_10_Pesos.Text = Registro["DENOM_10_PESOS"].ToString();
                        Txt_Denom_100_Pesos.Text = Registro["DENOM_100_PESOS"].ToString();
                        Txt_Denom_1000_Pesos.Text = Registro["DENOM_1000_PESOS"].ToString();
                        Txt_Denom_2_Pesos.Text = Registro["DENOM_2_PESOS"].ToString();
                        Txt_Denom_20_Cent.Text = Registro["DENOM_20_CENT"].ToString();
                        Txt_Denom_20_Pesos.Text = Registro["DENOM_20_PESOS"].ToString();
                        Txt_Denom_200_Pesos.Text = Registro["DENOM_200_PESOS"].ToString();
                        Txt_Denom_5_Pesos.Text = Registro["DENOM_5_PESOS"].ToString();
                        Txt_Denom_50_Cent.Text = Registro["DENOM_50_CENT"].ToString();
                        Txt_Denom_50_Pesos.Text = Registro["DENOM_50_PESOS"].ToString();
                        Txt_Denom_500_Pesos.Text = Registro["DENOM_500_PESOS"].ToString();
                        Txt_Total.Text = Decimal.Round(Convert.ToDecimal(Registro["MONTO_TOTAL"].ToString()), 2).ToString("#,###,###,##0.00");
                        Session["Dt_Arqueos"] = Dt_Arqueos;
                        Turno = Registro["NO_TURNO"].ToString();
                    }
                    Llenar_Tabla_Recolecciones(Turno);
                    Suma_Total_Caja();
                    Div_Arqueos.Visible = false;
                    Div_Arqueos_Detalles.Visible = true;
                    Btn_Salir.ToolTip = "Regresar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Salir.Visible = true;
                    Btn_Nuevo.Visible = false;
                }
            }            
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Recolecciones_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Recolecciones
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Arqueos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Arqueos.PageIndex = e.NewPageIndex;
            Llenar_Tabla_Arqueos();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
       
    #endregion

    #region Eventos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Recoleccion
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        DataSet Ds_Arqueos = new DataSet();
        string No_Arqueo = string.Empty;
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Configuracion_Formulario(true);
                Habilitar_Denominaciones();
                Limpiar_Catalogo();
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Btn_Imprimir.Visible = false;
                Llenar_Caja_Modulo();
                Llenar_Caja_Cajero();
                //Llenar_Caja_Apertura();
                Txt_Cajero.Text = Cls_Sessiones.Nombre_Empleado;
                Llenar_Caja_Total_Recolectado();
                Llenar_Caja_Total_Cobrado();
                Llenar_Caja_Fondo_Inicial();
                Calcular_Arqueo();
                Txt_No_Caja.Text = Hf_No_Caja.Value;
                Llenar_Tabla_Recolecciones(Hf_No_Turno.Value.Trim());
                Div_Arqueos_Detalles.Visible = true;
                Div_Arqueos.Visible = false;
                Suma_Total_Caja();
            }
            else
            {
                if (Validar_Componentes_Generales())
                {
                    Cls_Ope_Pre_Arqueos_Negocio Arqueos = new Cls_Ope_Pre_Arqueos_Negocio();
                    //double Recolectado = 0.00;
                    Arqueos.P_No_Turno = Hf_No_Turno.Value.Trim();
                    Arqueos.P_Realizo = Hf_Realizo.Value.Trim();
                    Arqueos.P_Total_Efectivo = Convert.ToDouble(Txt_Total_Cobrado.Text.ToString().Replace("$", "").Trim());
                    Arqueos.P_Total_Tarjeta = Convert.ToDouble(Txt_Total_Tarjeta.Text.ToString().Replace("$", "").Trim());
                    Arqueos.P_Total_Cheques = Convert.ToDouble(Txt_Total_Cheques.Text.ToString().Replace("$", "").Trim());
                    Arqueos.P_Total_Transferencias = Convert.ToDouble(Txt_Total_Transferencias.Text.ToString().Replace("$", "").Trim());
                    Arqueos.P_Total_Cobrado = Convert.ToDouble(Txt_Total_Caja.Text.ToString().Replace("$", "").Trim());
                    
                    //Recolectado = Convert.ToDouble(Txt_Total_Recolectado.Text.ToString().Replace("$", "").Trim()) + ;
                    Arqueos.P_Arqueo = Convert.ToDouble(Txt_Total.Text.ToString().Replace("$", "").Trim());
                    Arqueos.P_Total_Recolectado = Convert.ToDouble(Txt_Total_Recolectado.Text.ToString().Replace("$", "").Trim());
                    Arqueos.P_Fondo_Inicial = Convert.ToDouble(Txt_Fondo_Inicial.Text.ToString().Replace("$", "").Trim());
                    Arqueos.P_Comentarios = Txt_Comentarios.Text.ToUpper().Trim();
                    Arqueos.P_Diferencia = Convert.ToDouble(Hfd_Diferencia.Value.ToString().Replace("$", "").Trim());

                    Arqueos.P_Denom_1_Peso = Convert.ToInt32(Txt_Denom_1_Peso.Text.ToString());
                    Arqueos.P_Denom_10_Cent = Convert.ToInt32(Txt_Denom_10_Cent.Text.ToString());
                    Arqueos.P_Denom_10_Pesos = Convert.ToInt32(Txt_Denom_10_Pesos.Text.ToString());
                    Arqueos.P_Denom_100_Pesos = Convert.ToInt32(Txt_Denom_100_Pesos.Text.ToString());
                    Arqueos.P_Denom_1000_Pesos = Convert.ToInt32(Txt_Denom_1000_Pesos.Text.ToString());
                    Arqueos.P_Denom_2_Pesos = Convert.ToInt32(Txt_Denom_2_Pesos.Text.ToString());
                    Arqueos.P_Denom_20_Cent = Convert.ToInt32(Txt_Denom_20_Cent.Text.ToString());
                    Arqueos.P_Denom_20_Pesos = Convert.ToInt32(Txt_Denom_20_Pesos.Text.ToString());
                    Arqueos.P_Denom_200_Pesos = Convert.ToInt32(Txt_Denom_200_Pesos.Text.ToString());
                    Arqueos.P_Denom_5_Pesos = Convert.ToInt32(Txt_Denom_5_Pesos.Text.ToString());
                    Arqueos.P_Denom_50_Cent = Convert.ToInt32(Txt_Denom_50_Cent.Text.ToString());
                    Arqueos.P_Denom_50_Pesos = Convert.ToInt32(Txt_Denom_50_Pesos.Text.ToString());
                    Arqueos.P_Denom_500_Pesos = Convert.ToInt32(Txt_Denom_500_Pesos.Text.ToString());
                    Arqueos.P_Monto_Total = Convert.ToDouble(Txt_Total.Text.ToString().Replace("$","").Trim());

                    Arqueos.P_Caja_ID = Hf_Caja_ID .Value.Trim();
                    Arqueos.P_Usuario = Cls_Sessiones.Nombre_Empleado;

                    Mpe_Autenticacion.Hide();
                    No_Arqueo = Arqueos.Alta_Arqueo();
                    Generar_Dt_Arqueos(No_Arqueo);

                    Llenar_Tabla_Recolecciones(Hf_No_Turno.Value);
                    Ds_Arqueos = Generar_Datos_Reporte();
                    Imprimir_Reporte(Ds_Arqueos, "Rpt_Caj_Arqueos.rpt", "Entrega Final de Caja");

                    Configuracion_Formulario(true);
                    Llenar_Tabla_Arqueos();


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Proceso de Arqueos", "$('#Mpe_Autenticacion').hide(); alert('Alta de Arqueo Exitosa');", true);
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Modificar.Visible = true;
                    Btn_Imprimir.Visible = true;
                    Grid_Arqueos.Enabled = true;
                    Grid_Arqueos.Visible = true;
                    Limpiar_Catalogo();
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Ciudad
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 21/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if (Grid_Arqueos.Rows.Count > 0 && Grid_Arqueos.SelectedIndex > (-1))
                {
                    Configuracion_Formulario(true);
                    Habilitar_Denominaciones();

                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Imprimir.Visible = false;
                   
                    //Hf_Realizo.Value = "";
                    //Txt_Realizo.Text = "";

                    Grid_Arqueos.Visible = false;
                    //Txt_Clave.Enabled = false;
                    Div_Arqueos_Detalles.Visible = true;
                    Div_Arqueos.Visible = false;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes_Generales())
                {
                    Cls_Ope_Pre_Arqueos_Negocio Arqueos = new Cls_Ope_Pre_Arqueos_Negocio();
                    Arqueos.P_No_Arqueo = Hf_No_Arqueo.Value.Trim(); 
                    Arqueos.P_No_Turno = Hf_No_Turno.Value.Trim();
                    Arqueos.P_Realizo = Hf_Realizo.Value.Trim();
                    Arqueos.P_Total_Cobrado = Convert.ToDouble(Txt_Total_Cobrado.Text.ToString().Replace("$", "").Trim());
                    Arqueos.P_Total_Recolectado = Convert.ToDouble(Txt_Total_Recolectado.Text.ToString().Replace("$", "").Trim());
                    Arqueos.P_Fondo_Inicial = Convert.ToDouble(Txt_Fondo_Inicial.Text.ToString().Replace("$", "").Trim());
                    Arqueos.P_Comentarios = Txt_Comentarios.Text.ToUpper().Trim();
                    Arqueos.P_Arqueo = Convert.ToDouble(Txt_Total.Text.ToString().Replace("$", "").Trim());
                    Arqueos.P_Denom_1_Peso = Convert.ToInt32(Txt_Denom_1_Peso.Text.ToString());
                    Arqueos.P_Denom_10_Cent = Convert.ToInt32(Txt_Denom_10_Cent.Text.ToString());
                    Arqueos.P_Denom_10_Pesos = Convert.ToInt32(Txt_Denom_10_Pesos.Text.ToString());
                    Arqueos.P_Denom_100_Pesos = Convert.ToInt32(Txt_Denom_100_Pesos.Text.ToString());
                    Arqueos.P_Denom_1000_Pesos = Convert.ToInt32(Txt_Denom_1000_Pesos.Text.ToString());
                    Arqueos.P_Denom_2_Pesos = Convert.ToInt32(Txt_Denom_2_Pesos.Text.ToString());
                    Arqueos.P_Denom_20_Cent = Convert.ToInt32(Txt_Denom_20_Cent.Text.ToString());
                    Arqueos.P_Denom_20_Pesos = Convert.ToInt32(Txt_Denom_20_Pesos.Text.ToString());
                    Arqueos.P_Denom_200_Pesos = Convert.ToInt32(Txt_Denom_200_Pesos.Text.ToString());
                    Arqueos.P_Denom_5_Pesos = Convert.ToInt32(Txt_Denom_5_Pesos.Text.ToString());
                    Arqueos.P_Denom_50_Cent = Convert.ToInt32(Txt_Denom_50_Cent.Text.ToString());
                    Arqueos.P_Denom_50_Pesos = Convert.ToInt32(Txt_Denom_50_Pesos.Text.ToString());
                    Arqueos.P_Denom_500_Pesos = Convert.ToInt32(Txt_Denom_500_Pesos.Text.ToString());
                    Arqueos.P_Monto_Total = Convert.ToDouble(Txt_Total.Text.ToString().Replace("$", "").Trim());

                    Arqueos.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Arqueos.Modificar_Arqueo();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Tabla_Arqueos();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Proceso de Arqueos", "alert('Actualización de Arqueo Exitosa');", true);
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.Visible = true;
                    Btn_Imprimir.Visible = true;
                    Grid_Arqueos.Enabled = true;
                    Grid_Arqueos.Visible = true;
                }

            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN: Imprime un documento con la informacion del Descuento de Traslado
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, EventArgs e)
    {
        DataSet Ds_Arqueos = new DataSet();
        try
        {
            if (!string.IsNullOrEmpty (Hf_No_Arqueo.Value.Trim()))
            {
                //obtenemos los datos que mostraremos en el reporte
                
                if (Grid_Arqueos.Rows.Count > 0 && Grid_Arqueos.SelectedIndex > (-1))
                {
                    Ds_Arqueos = Generar_Datos_Reporte();
                    Imprimir_Reporte(Ds_Arqueos, "Rpt_Caj_Arqueos.rpt", "Entrega Final de Caja");
                    //Div_Arqueos_Detalles.Visible = true;
                    //Div_Arqueos.Visible = true;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Imprimir.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "El registro que intento imprimir no tiene Arqueos";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;

            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Llena la Tabla de Recolecciones con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Llenar_Tabla_Arqueos_Busqueda(0);
            if (Grid_Arqueos.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron  todas las Recolecciones almacenadas)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                Llenar_Tabla_Arqueos();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip.Equals("Inicio"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Llenar_Tabla_Arqueos();
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Nuevo .ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.Visible = true;
                Btn_Imprimir.Visible = true;
                Btn_Nuevo.Visible = true;
                Grid_Arqueos.Visible = true;
                Grid_Arqueos.Enabled = true;
                Div_Arqueos.Visible = true;
                Div_Arqueos_Detalles.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Cerrar_Ventana_Click
    /// DESCRIPCION : Cierra la ventana de busqueda de empleados.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Cerrar_Ventana_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Autenticacion.Hide();
        Txt_No_Empleado.Text = "";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Cerrar_Ventana_Click
    /// DESCRIPCION : Cierra la ventana de busqueda de empleados.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Autenticarse_Click(object sender, EventArgs e)
    {
        String No_Empleado = string.Empty;
        Int32  No_Letras = 0;

        if (Validar_Autenticacion())
        {
            Cls_Ope_Pre_Arqueos_Negocio Autenticacion = new Cls_Ope_Pre_Arqueos_Negocio();

            No_Letras = Txt_No_Empleado.Text.Trim().Length; //obtenemos el no de letras que tiene el numero de empleado 

            if (No_Letras < 6) {
                for (Int32 i = 0; i < 6 - No_Letras; i++) {
                    No_Empleado += "0";
                } 
            }

            No_Empleado = No_Empleado + Txt_No_Empleado.Text.Trim();

            Autenticacion.P_No_Empleado = No_Empleado;
            DataSet Ds_Autenticar = Autenticacion.Consultar_Autenticacion();
            if (Ds_Autenticar.Tables[0].Rows.Count > 0)
            {
                Hf_Realizo.Value = Ds_Autenticar.Tables[0].Rows[0]["EMPLEADO_ID"].ToString();
                Txt_Realizo.Text = Ds_Autenticar.Tables[0].Rows[0]["EMPLEADO"].ToString();
                Mpe_Autenticacion.Hide();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Autenticación de Usuarios", "$('#Mpe_Autenticacion').hide();", true);
            }
            else 
            {
                Txt_Realizo.Text = "";
                Hf_Realizo.Value = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Autenticación de Usuarios", "alert('El No. Empleado es Incorrecto');", true);
            }
        }
        else 
        {
            Txt_Realizo.Text = "";
            Hf_Realizo.Value = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Autenticación de Usuarios", "alert('El No. Empleado es Incorrecto');", true);
        }
        Suma_Total_Caja();
    }

    #endregion

    #region Impresion Reportes

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Arqueos, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Cajas/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Arqueos);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error " + Ex.Message);
        }

        try
        {
            Mostrar_Reporte(Archivo_PDF, "PDF");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
    ///DESCRIPCIÓN          : Visualiza en pantalla el reporte indicado
    ///PARAMETROS           : Nombre_Reporte: cadena con el nombre del archivo.
    ///                     : Formato: Exensión del archivo a visualizar.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt", " $('#Mpe_Autenticacion').hide(); window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Descuentos_Traslado
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos del
    ///                     Descuento de Traslado Seleccionado en el GridView
    ///PARAMETROS: 
    ///CREO                 : José Alfredo García Pichardo
    ///FECHA_CREO           : 23/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Arqueos()
    {
        Ds_Ope_Pre_Arqueos Ds_Arqueos = new Ds_Ope_Pre_Arqueos();
        Cls_Ope_Pre_Arqueos_Negocio Arqueos = new Cls_Ope_Pre_Arqueos_Negocio();

        DataTable Dt_Temp = new DataTable();
        DataRow Dr_Arqueos;

        //String Arqueos_ID = "";

        foreach (DataTable Dt_Arqueos in Ds_Arqueos.Tables)
        {
            if (Dt_Arqueos.TableName == "Dt_Arqueos")
            {
                Arqueos.P_No_Arqueo = Hf_No_Arqueo.Value.Trim();
                Dt_Temp = Arqueos.Consultar_Datos_Arqueos();

                foreach (DataRow Dr_Temp in Dt_Temp.Rows)
                {
                    //Inserta los datos de los Impuestos de Derechos de Supervision en la Tabla
                    Dr_Arqueos = Dt_Arqueos.NewRow();
                    Dr_Arqueos["NO_ARQUEO"] = Dr_Temp[Ope_Caj_Arqueos.Campo_No_Arqueo];
                    Dr_Arqueos["NO_TURNO"] = Dr_Temp[Ope_Caj_Arqueos.Campo_No_Turno];
                    Dr_Arqueos["REALIZO"] = Dr_Temp["REALIZO"];
                    Dr_Arqueos["MODULO"] = Dr_Temp["MODULO"];
                    Dr_Arqueos["CAJA"] = Dr_Temp["CAJA"];
                    Dr_Arqueos["HORA_APERTURA"] = String.Format("{0:dd,MM,yyyy HH,mm,tt}", Dr_Temp["HORA_APERTURA"]);
                    Dr_Arqueos["CAJERO"] = Dr_Temp["CAJERO"];
                    Dr_Arqueos["ARQUEO"] = Decimal.Round(Convert.ToDecimal(Dr_Temp[Ope_Caj_Arqueos.Campo_Arqueo]), 2).ToString("$#,###,###,##0.00");
                    Dr_Arqueos["FECHA"] = String.Format("{0:dd/MMM/yyyy HH:mm:tt}", Dr_Temp["FECHA"]); 
                    Dr_Arqueos["CAJA_ID"] = Dr_Temp["CAJA_ID"];
                    Dr_Arqueos["REALIZO_ID"] = Dr_Temp["REALIZO_ID"];
                    Dr_Arqueos["TOTAL_COBRADO"] = Decimal.Round(Convert.ToDecimal(Dr_Temp[Ope_Caj_Arqueos.Campo_Total_Cobrado]), 2).ToString("$#,###,###,##0.00");
                    Dr_Arqueos["TOTAL_RECOLECTADO"] = Decimal.Round(Convert.ToDecimal(Dr_Temp[Ope_Caj_Arqueos.Campo_Total_Recolectado]), 2).ToString("$#,###,###,##0.00");
                    Dr_Arqueos["FONDO_INICIAL"] = Decimal.Round(Convert.ToDecimal(Dr_Temp[Ope_Caj_Arqueos.Campo_Fondo_Inicial]), 2).ToString("$#,###,###,##0.00");
                    Dr_Arqueos["AJUSTE_TARIFARIO"] = 0;
                    Dr_Arqueos["DIFERENCIA"] = Decimal.Round(Convert.ToDecimal(Dr_Temp[Ope_Caj_Arqueos.Campo_Diferencia]), 2).ToString("$#,###,###,##0.00");
                    Dr_Arqueos["COMENTARIOS"] = Dr_Temp[Ope_Caj_Arqueos.Campo_Comentarios];
                    Dr_Arqueos["DENOM_10_CENT"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_10_Cent];
                    Dr_Arqueos["DENOM_1_PESO"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_1_Peso];
                    Dr_Arqueos["DENOM_10_PESOS"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_10_Pesos];
                    Dr_Arqueos["DENOM_100_PESOS"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_100_Pesos];
                    Dr_Arqueos["DENOM_1000_PESOS"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_1000_Pesos];
                    Dr_Arqueos["DENOM_20_CENT"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_20_Cent];
                    Dr_Arqueos["DENOM_2_PESOS"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_2_Pesos];
                    Dr_Arqueos["DENOM_20_PESOS"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_20_Pesos];
                    Dr_Arqueos["DENOM_200_PESOS"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_200_Pesos];
                    Dr_Arqueos["DENOM_50_CENT"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_50_Cent];
                    Dr_Arqueos["DENOM_5_PESOS"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_5_Pesos];
                    Dr_Arqueos["DENOM_50_PESOS"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_50_Cent];
                    Dr_Arqueos["DENOM_500_PESOS"] = Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Denom_500_Pesos];
                    Dr_Arqueos["MONTO_TOTAL"] = Decimal.Round(Convert.ToDecimal(Dr_Temp[Ope_Caj_Arqueos_Det.Campo_Monto_Total]), 2).ToString("$#,###,###,##0.00");
                    
                    Dt_Arqueos.Rows.Add(Dr_Arqueos);
                }
            }
        }
        return Ds_Arqueos;
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
            Botones.Add(Btn_Imprimir);
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

    #region Denominaciones

    protected void Txt_Denom_10_Cent_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total();
    }
    protected void Txt_Denom_1_Peso_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total();
    }
    protected void Txt_Denom_10_Pesos_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total();
    }
    protected void Txt_Denom_100_Pesos_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total(); 
    }
    protected void Txt_Denom_1000_Pesos_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total(); 
    }
    protected void Txt_Denom_20_Cent_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total();
    }
    protected void Txt_Denom_2_Pesos_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total(); 
    }
    protected void Txt_Denom_20_Pesos_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total(); 
    }
    protected void Txt_Denom_200_Pesos_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total();  
    }
    protected void Txt_Denom_50_Cent_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total();
    }
    protected void Txt_Denom_5_Pesos_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total();
    }
    protected void Txt_Denom_50_Pesos_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Recolecciones_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Recolecciones
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Denom_500_Pesos_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Recolecciones_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Recolecciones
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Calcular_Total() 
    {
        Double Total = Convert.ToDouble(Convert.ToInt32(Txt_Denom_10_Cent.Text.Trim().ToString()) * .10) +
                       Convert.ToDouble(Convert.ToInt32(Txt_Denom_1_Peso.Text.Trim().ToString()) * 1) +
                       Convert.ToDouble(Convert.ToInt32(Txt_Denom_10_Pesos.Text.Trim().ToString()) * 10) +
                       Convert.ToDouble(Convert.ToInt32(Txt_Denom_100_Pesos.Text.Trim().ToString()) * 100) +
                       Convert.ToDouble(Convert.ToInt32(Txt_Denom_1000_Pesos.Text.Trim().ToString()) * 1000) +
                       Convert.ToDouble(Convert.ToInt32(Txt_Denom_20_Cent.Text.Trim().ToString()) * .20) +
                       Convert.ToDouble(Convert.ToInt32(Txt_Denom_2_Pesos.Text.Trim().ToString()) * 2) +
                       Convert.ToDouble(Convert.ToInt32(Txt_Denom_20_Pesos.Text.Trim().ToString()) * 20) +
                       Convert.ToDouble(Convert.ToInt32(Txt_Denom_200_Pesos.Text.Trim().ToString()) * 200) +
                       Convert.ToDouble(Convert.ToInt32(Txt_Denom_50_Cent.Text.Trim().ToString()) * .50) +
                       Convert.ToDouble(Convert.ToInt32(Txt_Denom_5_Pesos.Text.Trim().ToString()) * 5) +
                       Convert.ToDouble(Convert.ToInt32(Txt_Denom_50_Pesos.Text.Trim().ToString()) * 50) +
                       Convert.ToDouble(Convert.ToInt32(Txt_Denom_500_Pesos.Text.Trim().ToString()) * 500);
       Txt_Total.Text = Math.Round(Total, 2).ToString("#,###,###,##0.00");
    }

    #endregion
}
