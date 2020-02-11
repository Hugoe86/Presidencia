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
using Presidencia.Ope_Pre_Impresion_Recibo_Negocio;
using Presidencia.Ayudante_JQuery;

public partial class paginas_Predial_Impresion_Recibos_Frm_Ope_Pre_Controlador_Impresion_Recibo : System.Web.UI.Page
{
    #region PAGELOAD
        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Page_Load
        //DESCRIPCIÓN          : Inicio de la pagina
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 28/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                if (!IsPostBack)
                {
                    Controlador_Inicio();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar los datos del recibo Controlador(Error[" + ex.Message + "])");
            }
        }
    #endregion

    #region METODOS
        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Controlador_Inicio
        //DESCRIPCIÓN          : Metodo para dar inicio a la pagina
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 28/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        protected void Controlador_Inicio() {
            Cls_Ope_Pre_Impresion_Recibo_Negocio Recibo_Negocio = new Cls_Ope_Pre_Impresion_Recibo_Negocio(); //Instancia con la capa de negocio
            String Referencia = String.Empty;
            String Tipo_Referencia = String.Empty;
            String Anio = String.Empty;
            String No_Calculo = String.Empty;
            DataTable Dt_Datos_Recibo = new DataTable();
            string Complemento = string.Empty;
            String Cadena_JSON = string.Empty;

            Response.Clear();
            try
            {
                if (Request.QueryString["Referencia"] != "" && Request.QueryString["No_Pago"] != "")
                {
                    Referencia = Request.QueryString["Referencia"].ToString().Trim(); //obtenemos la referencia
                    Recibo_Negocio.P_No_Pago = Request.QueryString["No_Pago"].ToString().Trim();//obtenemos el numero de pago
                    if (Recibo_Negocio.P_No_Pago.Length < 10) //complementamos el numero de pago
                    {
                        for (Int32 i = 0; i < 10 - Recibo_Negocio.P_No_Pago.Length; i++)
                        {
                            Complemento += "0";
                        }
                    }
                    Recibo_Negocio.P_No_Pago = Complemento + Request.QueryString["No_Pago"].ToString().Trim();
                    Complemento = string.Empty;

                    if (Request.QueryString["Estatus"] != "")
                    {
                        Recibo_Negocio.P_Referencia = Referencia;
                        Dt_Datos_Recibo = Recibo_Negocio.Consultar_Datos_Cancelado();
                    }
                    else 
                    {
                        if (Referencia.StartsWith("CDER") || Referencia.StartsWith("RDER") || Referencia.StartsWith("CFRA") || Referencia.StartsWith("RFRA") || Referencia.StartsWith("CPRE ") || Referencia.StartsWith("RPRE") || Referencia.StartsWith("CTRA ") || Referencia.StartsWith("RTRA"))
                        {
                            //obtenemos el tipo de referencia, el año y el numero de calculo para obtener los datos del pago
                        }
                        else if (Referencia.StartsWith("DER") || Referencia.StartsWith("IMP"))
                        {
                            //obtenemos el tipo de referencia, el año y el numero de calculo para obtener los datos del pago
                            Recibo_Negocio.P_Referencia = Referencia;
                            Dt_Datos_Recibo = Recibo_Negocio.Consultar_Datos_Recibos_Impuestos();
                        }
                        else if (Referencia.StartsWith("TD"))
                        {
                            //obtenemos el tipo de referencia, el año y el numero de calculo para obtener los datos del pago
                            Recibo_Negocio.P_Tipo_Referencia = Referencia.Substring(0, 2);
                            Recibo_Negocio.P_Anio_Calculo = Referencia.Substring(Referencia.Length - 4);

                            No_Calculo = Referencia.Substring(2, Referencia.Length - 6);
                            if (No_Calculo.Length < 10)
                            {
                                for (Int32 i = 0; i < 10 - No_Calculo.Length; i++)
                                {
                                    Complemento += "0";
                                }
                            }
                            Recibo_Negocio.P_No_Calculo = Complemento + No_Calculo;
                            Recibo_Negocio.P_Referencia = Referencia;
                            Dt_Datos_Recibo = Recibo_Negocio.Consultar_Datos_Recibos();
                        }
                        else if (Referencia.StartsWith("OTRPAG"))
                        {
                            //obtenemos el tipo de referencia, el año y el numero de calculo para obtener los datos del pago
                            Recibo_Negocio.P_Referencia = Referencia;
                            Dt_Datos_Recibo = Recibo_Negocio.Consultar_Datos_Otros_Pagos();
                        }
                        else if (Char.IsLetter(Referencia, 1))
                        {
                            Recibo_Negocio.P_Referencia = Referencia;
                            Dt_Datos_Recibo = Recibo_Negocio.Consultar_Datos_Constancias();
                        }
                        else
                        {
                            //es cuenta predial
                            Recibo_Negocio.P_Referencia = Referencia;
                            if (Recibo_Negocio.P_Referencia.Length < 10)
                            {
                                for (Int32 i = 0; i < 10 - Recibo_Negocio.P_Referencia.Length; i++)
                                {
                                    Complemento += "0";
                                }
                            }
                            Recibo_Negocio.P_Referencia = Complemento + Referencia;
                            Dt_Datos_Recibo = Recibo_Negocio.Consultar_Datos_Cuenta_Predial();
                        }
                    }
                }
                Dt_Datos_Recibo.TableName = "Recibos";

                Cadena_JSON = Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Datos_Recibo);
           
                Response.ContentType = "application/json";
                Response.Write(Cadena_JSON);
                Response.Flush();
                Response.Close();
                //Response.End();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar los datos del recibo Controlador_Inicio(Error[" + Ex.Message + "])");
            }
        } 
    #endregion
}
