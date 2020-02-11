using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Catalogo_Cat_Parametros.Negocio;
using Presidencia.Catalogo_Cat_Tabla_Valores_Tramos.Negocio;
using Presidencia.Sessiones;
using System.Data;
using Presidencia.Constantes;

public partial class paginas_Catastro_Frm_Cat_Cat_Parametros : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN:
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Formulario(false);
                Cargar_Datos();
            }
        }
        catch (Exception ex)
        {
            Lbl_Ecabezado_Mensaje.Text = ex.Message.ToString();
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Establece la configuración del formulario
    ///PROPIEDADES:     Enabled: Especifica si estara habilitado o no el componente
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Enabled)
    {
        Txt_Decimales_Redondeo.Enabled = Enabled;
        Txt_Decimales_Redondeo.Style["text-align"] = "right";
        Txt_Incremento_Valor.Enabled = Enabled;
        Txt_Incremento_Valor.Style["text-align"] = "right";
        Txt_Firmante.Enabled = Enabled;
        Txt_Firmante.Style["text-align"] = "left";
        Txt_Factor_Ef.Enabled = Enabled;
        Txt_Factor_Ef.Style["text-align"] = "right";
        Txt_Columnas.Enabled = Enabled;
        Txt_Columnas.Style["text-align"] = "right";
        Txt_Renglones.Enabled = Enabled;
        Txt_Renglones.Style["text-align"] = "right";
        Txt_Dias_Vigencia.Enabled = Enabled;
        Txt_Dias_Vigencia.Style["text-align"] = "right";
        Txt_Anio_Nuevo.Enabled = Enabled;
        Txt_Anio_Nuevo.Style["text-align"] = "right";
        Btn_Calcula_Anio.Enabled = Enabled;
        Txt_Correo_Autorizacion.Enabled = Enabled;
        Txt_Correo_Autorizacion.Style["text-align"] = "left";
        Txt_Correo_General.Enabled = Enabled;
        Txt_Correo_General.Style["text-align"] = "left";
        Txt_Firmante.Enabled = Enabled;
        Txt_Firmante.Style["text-align"] = "left";
        Txt_Puesto.Enabled = Enabled;
        Txt_Puesto.Style["text-align"] = "left";
        Txt_Firmante_2.Enabled = Enabled;
        Txt_Firmante_2.Style["text-align"] = "left";
        Txt_Puesto_2.Enabled = Enabled;
        Txt_Puesto_2.Style["text-align"] = "left";
        Txt_Fundamentacion_Legal.Enabled = Enabled;
        Txt_Fundamentacion_Legal.Style["text-align"] = "left";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: Carga los datos de parámetros
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Datos()
    {
        try
        {
            DataTable Dt_Parametros;
            Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
            Dt_Parametros = Parametros.Consultar_Parametros();
            Txt_Decimales_Redondeo.Text = Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Decimales_Redondeo].ToString();
            Txt_Incremento_Valor.Text = Convert.ToDouble(Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Incremento_Valor].ToString()).ToString("##0.00");
            Txt_Factor_Ef.Text = Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Factor_Ef].ToString();
            Txt_Columnas.Text = Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Columnas_Calc_Construccion].ToString();
            Txt_Renglones.Text = Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Renglones_Calc_Construccion].ToString();
            Txt_Dias_Vigencia.Text = Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Dias_Vigencia].ToString();
            Txt_Correo_Autorizacion.Text = Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Correo_Autorizacion].ToString();
            Txt_Correo_General.Text = Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Correo_General].ToString();
            Txt_Firmante.Text = Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Firmante].ToString();
            Txt_Puesto.Text = Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Puesto].ToString();
            Txt_Firmante_2.Text = Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Firmante_2].ToString();
            Txt_Puesto_2.Text = Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Puesto_2].ToString();
            Txt_Fundamentacion_Legal.Text = Dt_Parametros.Rows[0][Cat_Cat_Parametros.Campo_Fundamentacion_Legal].ToString();
        }
        catch (Exception E)
        {
            Lbl_Ecabezado_Mensaje.Text = "No hay registros.";
            Lbl_Ecabezado_Mensaje.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del botón modificar
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                Configuracion_Formulario(true);
                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Cat_Parametros_Negocio Parametros = new Cls_Cat_Cat_Parametros_Negocio();
                    Parametros.P_Decimales_Redondeo = Convert.ToDouble(Txt_Decimales_Redondeo.Text);
                    Parametros.P_Incremento_Valor = Convert.ToDouble(Txt_Incremento_Valor.Text);
                    Parametros.P_Factor_Ef = Txt_Factor_Ef.Text;
                    Parametros.P_Column_Calc_Const = Txt_Columnas.Text;
                    Parametros.P_Renglones_Calc_Const = Txt_Renglones.Text;
                    Parametros.P_Dias_Vigencia = Txt_Dias_Vigencia.Text;
                    Parametros.P_Correo_Autorizacion = Txt_Correo_Autorizacion.Text;
                    Parametros.P_Correo_General = Txt_Correo_General.Text;
                    Parametros.P_Firmante = Txt_Firmante.Text.ToUpper();
                    Parametros.P_Puesto = Txt_Puesto.Text.ToUpper();
                    Parametros.P_Firmante_2 = Txt_Firmante_2.Text.ToUpper();
                    Parametros.P_Puesto_2 = Txt_Puesto_2.Text.ToUpper();
                    Parametros.P_Fundamentacion_Legal = Txt_Fundamentacion_Legal.Text.ToUpper();
                    if ((Parametros.Modificar_Parametros()))
                    {
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Cargar_Datos();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Parámetros", "alert('Modificación de los Parámetros Exitosa.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Parámetros", "alert('Error al Modificar los Parámetros.');", true);
                    }
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del botón salir
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(false);
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Cargar_Datos();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Incremento_Valor_TextChanged
    ///DESCRIPCIÓN: Evento de la caja de texto de incremento de valor.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Incremento_Valor_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Incremento_Valor.Text.Trim() == "")
            {
                Txt_Incremento_Valor.Text = "0.00";
            }
            else
            {
                Txt_Incremento_Valor.Text = Convert.ToDouble(Txt_Incremento_Valor.Text).ToString("##0.00");
            }
        }
        catch
        {
            Txt_Incremento_Valor.Text = "0.00";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Factor_Ef_TextChanged
    ///DESCRIPCIÓN: Evento de la caja de texto de factor ef.
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Factor_Ef_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Incremento_Valor.Text.Trim() == "")
            {
                Txt_Incremento_Valor.Text = "0.00";
            }
            else
            {
                Txt_Incremento_Valor.Text = Convert.ToDouble(Txt_Incremento_Valor.Text).ToString("##0.00");
            }
        }
        catch
        {
            Txt_Incremento_Valor.Text = "0.00";
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Columnas_TextChanged
    ///DESCRIPCIÓN: Evento de la caja de texto Columnas
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Columnas_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Columnas.Text.Trim() == "")
            {
                Txt_Columnas.Text = "0";
            }
            else
            {
                Txt_Columnas.Text = Convert.ToDouble(Txt_Columnas.Text).ToString("#0");
            }
        }
        catch
        {
            Txt_Columnas.Text = "0";
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Renglones_TextChanged
    ///DESCRIPCIÓN: Evento de la caja de texto Renglones
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Renglones_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Renglones.Text.Trim() == "")
            {
                Txt_Renglones.Text = "0";
            }
            else
            {
                Txt_Renglones.Text = Convert.ToDouble(Txt_Renglones.Text).ToString("#0");
            }
        }
        catch
        {
            Txt_Renglones.Text = "0";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Dias_Vigencia_TextChanged
    ///DESCRIPCIÓN: Evento de la caja de texto Renglones
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Dias_Vigencia_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Dias_Vigencia.Text.Trim() == "")
            {
                Txt_Dias_Vigencia.Text = "0";
            }
            else
            {
                Txt_Dias_Vigencia.Text = Convert.ToDouble(Txt_Dias_Vigencia.Text).ToString("#0");
            }
        }
        catch
        {
            Txt_Dias_Vigencia.Text = "0";
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Valida los datos ingresados
    ///PROPIEDADES: 
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 11/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Componentes()
    {
        Boolean Valido = true;
        String Mensaje_Error = "";
        if (Txt_Decimales_Redondeo.Text.Trim() == "")
        {
            Mensaje_Error += "+ Ingrese lo decimales para tomar en cuenta al realizar el redondeo.";
            Valido = false;
        }
        if (Txt_Incremento_Valor.Text.Trim() == "")
        {
            if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
            Mensaje_Error += "+ Ingrese el incremento de valor.";
            Valido = false;
        }
        if (Txt_Factor_Ef.Text.Trim() == "")
        {
            if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
            Mensaje_Error += "+ Ingrese el Factor de Ef.";
            Valido = false;
        }
        if (Txt_Columnas.Text.Trim() == "")
        {
            if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
            Mensaje_Error += "+ Ingrese las Columnas.";
            Valido = false;
        }
        if (Txt_Renglones.Text.Trim() == "")
        {
            if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
            Mensaje_Error += "+ Ingrese los Renglones.";
            Valido = false;
        }
        if (Txt_Fundamentacion_Legal.Text.Trim() == "")
        {
            if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
            Mensaje_Error += "+ Fundamentacion.";
            Valido = false;
        }
        if (Txt_Firmante.Text.Trim() == "" || Txt_Firmante_2.Text.Trim() == "")
        {
            if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
            Mensaje_Error += "+ Ingrese Firmante.";
            Valido = false;
        }
        if (Txt_Puesto.Text.Trim() == "" || Txt_Puesto_2.Text.Trim() == "")
        {
            if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
            Mensaje_Error += "+ Ingrese Puesto.";
            Valido = false;
        }
        if (Txt_Dias_Vigencia.Text.Trim() == "")
        {
            if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
            Mensaje_Error += "+ Ingrese los Dias de Vigencia.";
            Valido = false;
        }
        if ((Txt_Correo_General.Text.Trim() != ""))// && (Txt_Correo_General.Text.Contains('@')))
        {
            if (!Txt_Correo_General.Text.Contains('@'))
            {
                if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
                Mensaje_Error += "+ Ingrese Correo General Valido.";
                Valido = false;
            }
        }
        else
        {
            if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
            Mensaje_Error += "+ Ingrese Correo General.";
            Valido = false;
        }
        if ((Txt_Correo_Autorizacion.Text.Trim() != ""))// && (Txt_Correo_General.Text.Contains('@')))
        {
            if (!Txt_Correo_Autorizacion.Text.Contains('@'))
            {
                if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
                Mensaje_Error += "+ Ingrese Correo Autorizacion Valido.";
                Valido = false;
            }
        }
        else
        {
            if (Mensaje_Error.Trim() != "") { Mensaje_Error += "<br/>"; }
            Mensaje_Error += "+ Ingrese Correo Autorizacion.";
            Valido = false;
        }
        if (!Valido)
        {
            Lbl_Ecabezado_Mensaje.Text = Mensaje_Error;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Valido;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Calcular_Anio_Click
    ///DESCRIPCIÓN: Evento del botón Calcular anio
    ///PROPIEDADES:     
    ///            
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 05/May_2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Calcular_Anio_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Anio_Nuevo.Text != "")
        {
            Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio Tramos = new Cls_Cat_Cat_Tabla_Valores_Tramos_Negocio();

            DataTable Dt_Tramos;
            DataTable Dt_Tramos_Calc = new DataTable();
            Dt_Tramos_Calc.Columns.Add("TRAMO_ID", typeof(String));
            Dt_Tramos_Calc.Columns.Add("VALOR_TRAMO", typeof(String));
            String Tramo_Id_Cal;
            Double Valor_Tramo_Cal;
            String Anio_Calc = Txt_Anio_Nuevo.Text;
            String Anio_Anterior = (Convert.ToDouble(Anio_Calc) - 1).ToString();
            Double Incremento_Valor = Convert.ToDouble(Txt_Incremento_Valor.Text);
            Double Valor_Nuevo = 0;
            Tramos.P_Anio = Anio_Calc;
            if (Tramos.Consultar_Tabla_Valores_Tramo().Rows.Count == 0)
            {
                Tramos.P_Anio = Anio_Anterior;
                Dt_Tramos = Tramos.Consultar_Tabla_Valores_Tramo();
                foreach (DataRow Dr_Renglon in Dt_Tramos.Rows)
                {
                    Tramo_Id_Cal = Dr_Renglon["TRAMO_ID"].ToString();
                    Valor_Tramo_Cal = Convert.ToDouble(Dr_Renglon["VALOR_TRAMO"]);
                    DataRow Dr_Valor_Nuevo = Dt_Tramos_Calc.NewRow();
                    Dr_Valor_Nuevo["TRAMO_ID"] = Tramo_Id_Cal;
                    //Dr_Valor_Nuevo["VALOR_TRAMO"] = (Math.Round(Valor_Tramo_Cal + ((Valor_Tramo_Cal * Convert.ToDouble(Txt_Incremento_Valor.Text) / 100)), 0)).ToString();
                    Valor_Nuevo = Convert.ToDouble((Valor_Tramo_Cal + ((Valor_Tramo_Cal * Convert.ToDouble(Txt_Incremento_Valor.Text) / 100))).ToString("#.00"));
                    if (Convert.ToDouble(Valor_Nuevo.ToString().Substring(Valor_Nuevo.ToString().Length - 3)) >= .51)
                    {
                        Dr_Valor_Nuevo["VALOR_TRAMO"] = Convert.ToDouble(Math.Round(Valor_Nuevo, 0)).ToString();
                    }
                    else
                    {
                        Dr_Valor_Nuevo["VALOR_TRAMO"] = Valor_Nuevo.ToString().Substring(0, Valor_Nuevo.ToString().Length - 3);
                    }
                    Dt_Tramos_Calc.Rows.Add(Dr_Valor_Nuevo);
                }
                Tramos.P_Dt_Tabla_Valores_Tramos = Dt_Tramos_Calc.Copy();
                Tramos.P_Anio = Anio_Calc;
                Tramos.Modificar_Tabla_Valores_Calculado();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Parámetros", "alert('Cálculo de valores para el año " + Txt_Anio_Nuevo.Text + " Realizada Correctamente.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Parametros", "alert('Ya existen  valores de Tramos para el año " + Txt_Anio_Nuevo.Text + "');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Parámetros", "alert('Introduzca un año a calcular.');", true);

        }
    } 
}