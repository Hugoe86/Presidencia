using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text;
using Presidencia.Ope_Pre_Impresion_Recibo_Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Catalogo_Casos_Especiales.Negocio;
using Presidencia.Cls_Ope_Ing_Descuentos.Negocio;

namespace Presidencia.Ope_Pre_Impresion_Recibo_Datos
{
    public class Cls_Ope_Pre_Impresion_Recibo_Datos
    {
        #region CONSULTAS
        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Recibos
        //DESCRIPCIÓN          : Consulta para obtener los datos que se mostraran en el recibo de pago
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 28/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Recibos(Cls_Ope_Pre_Impresion_Recibo_Negocio Recibo_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Encabezado = new DataTable(); //datatable para almacenar los datos del encabezado del recibo
            DataTable Dt_Descuento_Tras = new DataTable(); //datatable para almacenar los datos del encabezado del recibo
            DataTable Dt_Detalles = new DataTable(); //datatable para almacenar los datos de los detalles del pago
            DataTable Dt_Proteccion = new DataTable(); //datatable para almacenar los datos de la proteccion del pago
            DataTable Dt_Datos_Recibo = new DataTable(); // datatable para obtener todos los datos que se insertaran en el recibo
            DataTable Dt_Convenios = new DataTable();//Datatable para obtener los datos de los convenios
            String Consulta = "";

            try
            {
                // Consultamos los datos del encabezado
                Mi_Sql.Append("SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ");
                Mi_Sql.Append(Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' " + "" + "'|| ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' " + "" + "'|| ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS CONTRIBUYENTE, ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_RFC + ", ");
                Mi_Sql.Append(Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Base_Impuesto + ", ");
                Mi_Sql.Append(Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Minimo_Elevado_Anio);
                Mi_Sql.Append(" FROM " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);
                Mi_Sql.Append(" ON " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id);
                Mi_Sql.Append(" = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios);
                Mi_Sql.Append(" ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID);
                Mi_Sql.Append(" = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes);
                Mi_Sql.Append(" ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID);
                Mi_Sql.Append(" = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID);
                Mi_Sql.Append(" WHERE " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo);
                Mi_Sql.Append(" = '" + Recibo_Negocio.P_No_Calculo + "'");
                Mi_Sql.Append(" AND " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo);
                Mi_Sql.Append(" = '" + Recibo_Negocio.P_Anio_Calculo + "'");
                Mi_Sql.Append(" AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO','POSEEDOR')");

                Dt_Encabezado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                // Obtenemos los datos de los detalles
                if (Dt_Encabezado.Rows.Count > 0)
                {
                    Dt_Detalles = Consultar_Detalles_Recibo(Recibo_Negocio.P_Referencia, Recibo_Negocio.P_No_Pago);
                    //Si es un Traslado consultar su descuento, a menos que este en un convenio
                    if (Recibo_Negocio.P_Referencia.StartsWith("TD"))
                    {
                        String No_Calculo = Recibo_Negocio.P_Referencia.Replace("TD", "");
                        String Anio_Calculo = No_Calculo.Substring(No_Calculo.Length - 4);
                        No_Calculo = Convert.ToInt32(No_Calculo.Substring(0, No_Calculo.Length - 4)).ToString("0000000000");
                        Consulta = Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Calculo + "='" + No_Calculo + "' AND " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Anio + "=" + Anio_Calculo + " AND " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " IN ('ACTIVO','PENDIENTE','PAGADO','INCUMPLIDO')";
                        if (Obtener_Dato_Consulta(Consulta) == "")
                        {
                            Dt_Descuento_Tras = Consultar_Datos_Decuentos_Traslado(Recibo_Negocio.P_Referencia);
                            if (Dt_Descuento_Tras != null && Dt_Descuento_Tras.Rows.Count > 0)
                            {
                                String Descuento = "";
                                Int16 indice = 0;
                                Int16 monto = 0;
                                Int16 descu = 0;
                                if (Convert.ToDouble(Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Traslado.Campo_Desc_Multa].ToString()) > 0)
                                {
                                    DataRow Dr_Renglon = Dt_Detalles.NewRow();
                                    foreach (DataRow Dr_Renglon_Recargos in Dt_Detalles.Rows)
                                    {
                                        if (Dr_Renglon_Recargos["DESCRIPCION"].ToString().Contains("MULTA"))
                                        {
                                            if (Dr_Renglon_Recargos["MONTO"].ToString().Trim().StartsWith("-"))
                                            {
                                                descu = indice;
                                            }
                                            else
                                            {
                                                monto = indice;
                                            }
                                        }
                                        indice++;
                                    }
                                    if (descu > monto)
                                    {
                                        Dt_Detalles.Rows.RemoveAt(descu);
                                        Dt_Detalles.Rows.RemoveAt(monto);
                                    }
                                    else
                                    {
                                        Dt_Detalles.Rows.RemoveAt(monto);
                                        Dt_Detalles.Rows.RemoveAt(descu);
                                    }
                                    Descuento = "MULTAS " + string.Format("{0:#,###,##0.00}", Convert.ToDouble(Dt_Descuento_Tras.Rows[0]["CAL_MULTAS"].ToString())) + " - " + Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Traslado.Campo_Desc_Multa].ToString() + "% DESCUENTO AUTORIZADO POR " + Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Traslado.Campo_Usuario_Creo].ToString();
                                    Dr_Renglon["DESCRIPCION"] = Descuento;
                                    Dr_Renglon["MONTO"] = Convert.ToDouble(Dt_Descuento_Tras.Rows[0]["MONTO_MULTA"].ToString());
                                    Dt_Detalles.Rows.Add(Dr_Renglon);
                                }
                                if (Convert.ToDouble(Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Traslado.Campo_Desc_Recargo].ToString()) > 0)
                                {
                                    DataRow Dr_Renglon = Dt_Detalles.NewRow();
                                    indice = 0;
                                    foreach (DataRow Dr_Renglon_Recargos in Dt_Detalles.Rows)
                                    {
                                        if (Dr_Renglon_Recargos["DESCRIPCION"].ToString().Contains("RECARGO"))
                                        {
                                            if (Dr_Renglon_Recargos["MONTO"].ToString().Trim().StartsWith("-"))
                                            {
                                                descu = indice;
                                            }
                                            else
                                            {
                                                monto = indice;
                                            }
                                        }
                                        indice++;
                                    }
                                    if (descu > monto)
                                    {
                                        Dt_Detalles.Rows.RemoveAt(descu);
                                        Dt_Detalles.Rows.RemoveAt(monto);
                                    }
                                    else
                                    {
                                        Dt_Detalles.Rows.RemoveAt(monto);
                                        Dt_Detalles.Rows.RemoveAt(descu);
                                    }
                                    Descuento = "RECARGOS " + string.Format("{0:#,###,##0.00}", Convert.ToDouble(Dt_Descuento_Tras.Rows[0]["CAL_RECARGOS"].ToString())) + " - " + Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Traslado.Campo_Desc_Recargo].ToString() + "% DESCUENTO AUTORIZADO POR " + Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Traslado.Campo_Usuario_Creo].ToString();
                                    Dr_Renglon["DESCRIPCION"] = Descuento;
                                    Dr_Renglon["MONTO"] = Convert.ToDouble(Dt_Descuento_Tras.Rows[0]["MONTO_RECARGO"].ToString());
                                    Dt_Detalles.Rows.Add(Dr_Renglon);
                                }
                            }
                        }
                    }
                    //Fraccionamientos
                    if (Recibo_Negocio.P_Referencia.StartsWith("IMP"))
                    {
                        String No_Calculo = Recibo_Negocio.P_Referencia.Replace("IMP", "");
                        String Anio_Calculo = "20" + No_Calculo.Substring(0, 2);
                        No_Calculo = Convert.ToInt32(No_Calculo.Substring(2)).ToString("0000000000");
                        Consulta = Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + "='" + No_Calculo + "' AND " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anio + "=" + Anio_Calculo + " AND " + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " IN ('ACTIVO','PENDIENTE','PAGADO','INCUMPLIDO')";
                        if (Obtener_Dato_Consulta(Consulta) == "")
                        {
                            Dt_Descuento_Tras = Consultar_Datos_Decuentos_Fracc(No_Calculo);
                            if (Dt_Descuento_Tras != null && Dt_Descuento_Tras.Rows.Count > 0)
                            {
                                String Descuento = "";
                                Int16 indice = 0;
                                Int16 monto = 0;
                                Int16 descu = 0;
                                Double Multas = 0;
                                if (Convert.ToDouble(Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Desc_Multa].ToString()) > 0)
                                {
                                    DataRow Dr_Renglon = Dt_Detalles.NewRow();
                                    foreach (DataRow Dr_Renglon_Recargos in Dt_Detalles.Rows)
                                    {
                                        if (Dr_Renglon_Recargos["DESCRIPCION"].ToString().Contains("MULTA"))
                                        {
                                            if (Dr_Renglon_Recargos["MONTO"].ToString().Trim().StartsWith("-"))
                                            {
                                                descu = indice;
                                            }
                                            else
                                            {
                                                monto = indice;
                                                Multas = Convert.ToDouble(Dr_Renglon_Recargos["MONTO"].ToString());
                                            }
                                        }
                                        indice++;
                                    }
                                    if (descu > monto)
                                    {
                                        Dt_Detalles.Rows.RemoveAt(descu);
                                        Dt_Detalles.Rows.RemoveAt(monto);
                                    }
                                    else
                                    {
                                        Dt_Detalles.Rows.RemoveAt(monto);
                                        Dt_Detalles.Rows.RemoveAt(descu);
                                    }
                                    Descuento = "MULTAS " + string.Format("{0:#,###,##0.00}", Multas) + " - " + Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Desc_Multa].ToString() + "% DESCUENTO AUTORIZADO POR " + Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Usuario_Creo].ToString();
                                    Dr_Renglon["DESCRIPCION"] = Descuento;
                                    Dr_Renglon["MONTO"] = Multas - Convert.ToDouble(Dt_Descuento_Tras.Rows[0]["MONTO_MULTAS"].ToString());
                                    Dt_Detalles.Rows.Add(Dr_Renglon);
                                }
                                if (Convert.ToDouble(Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Desc_Recargo].ToString()) > 0)
                                {
                                    DataRow Dr_Renglon = Dt_Detalles.NewRow();
                                    indice = 0;
                                    Double Recargos = 0;
                                    foreach (DataRow Dr_Renglon_Recargos in Dt_Detalles.Rows)
                                    {
                                        if (Dr_Renglon_Recargos["DESCRIPCION"].ToString().Contains("RECARGO"))
                                        {
                                            if (Dr_Renglon_Recargos["MONTO"].ToString().Trim().StartsWith("-"))
                                            {
                                                descu = indice;
                                            }
                                            else
                                            {
                                                monto = indice;
                                                Recargos = Convert.ToDouble(Dr_Renglon_Recargos["MONTO"].ToString());
                                            }
                                        }
                                        indice++;
                                    }
                                    if (descu > monto)
                                    {
                                        Dt_Detalles.Rows.RemoveAt(descu);
                                        Dt_Detalles.Rows.RemoveAt(monto);
                                    }
                                    else
                                    {
                                        Dt_Detalles.Rows.RemoveAt(monto);
                                        Dt_Detalles.Rows.RemoveAt(descu);
                                    }
                                    Descuento = "RECARGOS " + string.Format("{0:#,###,##0.00}", Recargos) + " - " + Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Desc_Recargo].ToString() + "% DESCUENTO AUTORIZADO POR " + Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Fracc.Campo_Usuario_Creo].ToString();
                                    Dr_Renglon["DESCRIPCION"] = Descuento;
                                    Dr_Renglon["MONTO"] = Recargos - Convert.ToDouble(Dt_Descuento_Tras.Rows[0]["MONTO_RECARGOS"].ToString());
                                    Dt_Detalles.Rows.Add(Dr_Renglon);
                                }
                            }
                        }
                    }
                    if (Recibo_Negocio.P_Referencia.StartsWith("DER"))
                    {
                        String No_Calculo = Recibo_Negocio.P_Referencia.Replace("DER", "");
                        String Anio_Calculo = "20" + No_Calculo.Substring(0, 2);
                        No_Calculo = Convert.ToInt32(No_Calculo.Substring(2)).ToString("0000000000");
                        Consulta = Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " FROM " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + " WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Impuesto_Dereho_Supervisio + "='" + No_Calculo + "' AND " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Anio + "=" + Anio_Calculo + " AND " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus + " IN ('ACTIVO','PENDIENTE','PAGADO','INCUMPLIDO')";
                        if (Obtener_Dato_Consulta(Consulta) == "")
                        {
                            Dt_Descuento_Tras = Consultar_Datos_Decuentos_Derechos(No_Calculo);
                            if (Dt_Descuento_Tras != null && Dt_Descuento_Tras.Rows.Count > 0)
                            {
                                String Descuento = "";
                                Int16 indice = 0;
                                Int16 monto = 0;
                                Int16 descu = 0;
                                Double Multas = 0;
                                if (Convert.ToDouble(Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Der_Sup.Campo_Desc_Multa].ToString()) > 0)
                                {
                                    DataRow Dr_Renglon = Dt_Detalles.NewRow();
                                    foreach (DataRow Dr_Renglon_Recargos in Dt_Detalles.Rows)
                                    {
                                        if (Dr_Renglon_Recargos["DESCRIPCION"].ToString().Contains("MULTA"))
                                        {
                                            if (Dr_Renglon_Recargos["MONTO"].ToString().Trim().StartsWith("-"))
                                            {
                                                descu = indice;
                                            }
                                            else
                                            {
                                                monto = indice;
                                                Multas = Convert.ToDouble(Dr_Renglon_Recargos["MONTO"].ToString());
                                            }
                                        }
                                        indice++;
                                    }
                                    if (descu > monto)
                                    {
                                        Dt_Detalles.Rows.RemoveAt(descu);
                                        Dt_Detalles.Rows.RemoveAt(monto);
                                    }
                                    else
                                    {
                                        Dt_Detalles.Rows.RemoveAt(monto);
                                        Dt_Detalles.Rows.RemoveAt(descu);
                                    }
                                    Descuento = "MULTAS " + string.Format("{0:#,###,##0.00}", Multas) + " - " + Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Der_Sup.Campo_Desc_Multa].ToString() + "% DESCUENTO AUTORIZADO POR " + Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Der_Sup.Campo_Usuario_Creo].ToString();
                                    Dr_Renglon["DESCRIPCION"] = Descuento;
                                    Dr_Renglon["MONTO"] = Multas - Convert.ToDouble(Dt_Descuento_Tras.Rows[0]["MONTO_MULTAS"].ToString());
                                    Dt_Detalles.Rows.Add(Dr_Renglon);
                                }
                                if (Convert.ToDouble(Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Der_Sup.Campo_Desc_Recargo].ToString()) > 0)
                                {
                                    DataRow Dr_Renglon = Dt_Detalles.NewRow();
                                    indice = 0;
                                    Double Recargos = 0;
                                    foreach (DataRow Dr_Renglon_Recargos in Dt_Detalles.Rows)
                                    {
                                        if (Dr_Renglon_Recargos["DESCRIPCION"].ToString().Contains("RECARGO"))
                                        {
                                            if (Dr_Renglon_Recargos["MONTO"].ToString().Trim().StartsWith("-"))
                                            {
                                                descu = indice;
                                            }
                                            else
                                            {
                                                monto = indice;
                                                Recargos = Convert.ToDouble(Dr_Renglon_Recargos["MONTO"].ToString());
                                            }
                                        }
                                        indice++;
                                    }
                                    if (descu > monto)
                                    {
                                        Dt_Detalles.Rows.RemoveAt(descu);
                                        Dt_Detalles.Rows.RemoveAt(monto);
                                    }
                                    else
                                    {
                                        Dt_Detalles.Rows.RemoveAt(monto);
                                        Dt_Detalles.Rows.RemoveAt(descu);
                                    }
                                    Descuento = "RECARGOS " + string.Format("{0:#,###,##0.00}", Recargos) + " - " + Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Der_Sup.Campo_Desc_Recargo].ToString() + "% DESCUENTO AUTORIZADO POR " + Dt_Descuento_Tras.Rows[0][Ope_Pre_Descuento_Der_Sup.Campo_Usuario_Creo].ToString();
                                    Dr_Renglon["DESCRIPCION"] = Descuento;
                                    Dr_Renglon["MONTO"] = Recargos - Convert.ToDouble(Dt_Descuento_Tras.Rows[0]["MONTO_RECARGOS"].ToString());
                                    Dt_Detalles.Rows.Add(Dr_Renglon);
                                }
                            }
                        }
                    }
                    if (Dt_Detalles.Rows.Count > 0)
                    {
                        Dt_Convenios = Consultar_Datos_Convenios_Traslado(Dt_Encabezado.Rows[0]["CUENTA_PREDIAL_ID"].ToString(), Recibo_Negocio.P_No_Pago);
                        //obtenemos los datos de la proteccion del recibo
                        Dt_Proteccion = Consultar_Proteccion_Pago(Recibo_Negocio.P_No_Pago);
                        if (Dt_Proteccion.Rows.Count > 0)
                        {
                            Dt_Datos_Recibo = Crear_Dt_Datos_Recibo(Dt_Encabezado, Dt_Detalles, Dt_Proteccion, Dt_Convenios);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos del encabezado del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_Recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Recibo
        //DESCRIPCIÓN          : Consulta para obtener los detalles de un pago 
        //PARAMETROS           1 Referencia: por la cual obtendremos los detalles del pago
        //                     2 No_Pago: por la cual obtendremos los detalles del pago
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 28/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Detalles_Recibo(String Referencia, String No_Pago)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            DataTable Dt_Detalles = new DataTable();
            DataTable Dt_Pagos = new DataTable();


            try
            {
                // Consultamos los datos de los detalles
                Mi_Sql.Append("SELECT " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago + ", ");
                Mi_Sql.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Monto + ", ");
                Mi_Sql.Append("NVL(" + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Fundamento + ", '') AS FUNDAMENTO, ");
                Mi_Sql.Append(Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Descripcion + ", ");
                Mi_Sql.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Descripcion + " AS PERIODO,");
                Mi_Sql.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID + ", ");
                Mi_Sql.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Cantidad + ", ");
                Mi_Sql.Append("NVL(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Contribuyente + ", '') AS CONTRIBUYENTE, ");
                Mi_Sql.Append("NVL(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Observaciones + ", '') AS OBSERVACIONES ");
                Mi_Sql.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing);
                Mi_Sql.Append(" ON " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID);
                Mi_Sql.Append(" = " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID);
                Mi_Sql.Append(" WHERE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Referencia + " IN ('" + Referencia.Replace(",", "','") + "')");
                Mi_Sql.Append(" AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pago + " IN ('" + No_Pago.Replace(",", "','") + "')");
                Mi_Sql.Append(" ORDER BY " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pasivo + " ASC");
                //Asigna la consulta en el datatable
                Dt_Detalles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos de los detalles del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Detalles; //Regresa el datatable en la funcion
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        internal static String Obtener_Dato_Consulta(String Consulta)
        {
            String Mi_SQL;
            String Dato_Consulta = "";

            try
            {
                Mi_SQL = "SELECT " + Consulta;

                OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Dr_Dato.Read())
                {
                    if (Dr_Dato[0] != null)
                    {
                        Dato_Consulta = Dr_Dato[0].ToString();
                    }
                    else
                    {
                        Dato_Consulta = "";
                    }
                    Dr_Dato.Close();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato = null;
            }
            catch
            {
            }
            finally
            {
            }

            return Dato_Consulta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 19/Abril/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Obtener_Dato_Consulta(ref OracleCommand Cmmd, String Campo, String Tabla, String Condiciones)
        {
            return Obtener_Dato_Consulta(ref Cmmd, Campo, Tabla, Condiciones, "");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
        ///DESCRIPCIÓN          : Sobrecarga para consultar el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Obtener_Dato_Consulta(ref OracleCommand Cmmd, String Campo, String Tabla, String Condiciones, String Dato_Salida_Default)
        {
            String Mi_SQL;
            String Dato_Consulta = "";
            OracleDataReader Dr_Dato = null;

            try
            {
                Mi_SQL = "SELECT " + Campo;
                if (Tabla != "")
                {
                    Mi_SQL += " FROM " + Tabla;
                }
                if (Condiciones != "")
                {
                    Mi_SQL += " WHERE " + Condiciones;
                }

                if (Cmmd != null)
                {
                    Cmmd.CommandText = Mi_SQL;
                    Dr_Dato = Cmmd.ExecuteReader();
                }
                else
                {
                    Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }

                if (Dr_Dato.Read())
                {
                    if (Dr_Dato[0] != null)
                    {
                        Dato_Consulta = Dr_Dato[0].ToString();
                    }
                    else
                    {
                        Dato_Consulta = "";
                    }
                    Dr_Dato.Close();
                }
                else
                {
                    Dato_Consulta = "";
                }
                if (Dr_Dato != null)
                {
                    Dr_Dato.Close();
                }
                Dr_Dato = null;
            }
            catch
            {
            }
            finally
            {
            }

            if (Dato_Consulta == "")
            {
                Dato_Consulta = Dato_Salida_Default;
            }

            return Dato_Consulta;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Proteccion_Pago
        //DESCRIPCIÓN          : Consulta para obtener los detalles de un pago 
        //PARAMETROS           1 No_Pago: numero por el cual consultaremos los datos del pago
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 28/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Proteccion_Pago(String No_Pago)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            DataTable Dt_Proteccion = new DataTable();

            try
            {
                // Consultamos los datos de los detalles
                Mi_Sql.Append("SELECT " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Estatus + ", ");
                Mi_Sql.Append("NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Documento + ", '') AS Documento, ");
                Mi_Sql.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Operacion + ", ");
                Mi_Sql.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha + ", ");
                Mi_Sql.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha_Creo + ", ");
                Mi_Sql.Append("NVL(" + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha_Cancelacion + ",'') AS Fecha_Cancelacion, ");
                Mi_Sql.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Total + ", ");
                Mi_Sql.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Recibo + ", ");
                Mi_Sql.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Descuento_Pronto_Pago + ", ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",'') as Cuenta_Predial, ");
                Mi_Sql.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + ", ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos + "." + Cat_Pre_Motivos.Campo_Nombre + ", '') AS Motivo_Cancelacion, ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Confronto);
                Mi_Sql.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);
                Mi_Sql.Append(" ON " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID);
                Mi_Sql.Append(" = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja);
                Mi_Sql.Append(" ON " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID);
                Mi_Sql.Append(" = " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Mi_Sql.Append(" ON " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Turno);
                Mi_Sql.Append(" = " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_Sql.Append(" ON " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Empleado_ID);
                Mi_Sql.Append(" = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos);
                Mi_Sql.Append(" ON " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Motivo_Cancelacion_ID);
                Mi_Sql.Append(" = " + Cat_Pre_Motivos.Tabla_Cat_Pre_Motivos + "." + Cat_Pre_Motivos.Campo_Motivo_ID);
                Mi_Sql.Append(" WHERE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + " IN ('" + No_Pago.Replace(",", "','") + "')");

                Dt_Proteccion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos de la proteccion de pago. Error: [" + Ex.Message + "]");
            }
            return Dt_Proteccion;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Predial
        //DESCRIPCIÓN          : Consulta para obtener los datos que se mostraran en el recibo de pago de una cuenta predial
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 2/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Cuenta_Predial(Cls_Ope_Pre_Impresion_Recibo_Negocio Recibo_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Encabezado = new DataTable(); //datatable para almacenar los datos del encabezado del recibo
            DataTable Dt_Contribuyente = new DataTable(); //datatable para almacenar los datos del contribuyente del encabezado del recibo
            DataTable Dt_Detalles = new DataTable(); //datatable para almacenar los datos de los detalles del pago
            DataTable Dt_Descuentos = new DataTable(); //datatable para almacenar los datos de los descuentos del pago
            DataTable Dt_Proteccion = new DataTable(); //datatable para almacenar los datos de la proteccion del pago
            DataTable Dt_Convenios = new DataTable();//Datatable para obtener los datos de los convenios
            DataTable Dt_Datos_Recibo = new DataTable(); // datatable para obtener todos los datos que se insertaran en el recibo
            String Clave_Movimientos = String.Empty;

            try
            {
                // Consultamos los datos del encabezado
                Mi_Sql.Append("SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", '') AS NO_EXTERIOR, ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", '') AS NO_INTERIOR, ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", '') AS CUOTA_ANUAL, ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", '') AS EFECTOS, ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + ", '') AS CUOTA_MINIMA, ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", '') AS NO_CUOTA_FIJA, ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", '') AS CUOTA_FIJA, ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", '') AS VALOR_FISCAL, ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Nombre + ", '') AS UBICACION, ");
                Mi_Sql.Append("NVL(" + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre + ", '') AS COLONIA, ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + ", '') AS TASA_ANUAL, ");
                Mi_Sql.Append(Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Descripcion);
                Mi_Sql.Append(" FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles);
                Mi_Sql.Append(" ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID);
                Mi_Sql.Append(" = " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Calle_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias);
                Mi_Sql.Append(" ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID);
                Mi_Sql.Append(" = " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Colonia_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual);
                Mi_Sql.Append(" ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID);
                Mi_Sql.Append(" = " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio);
                Mi_Sql.Append(" ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID);
                Mi_Sql.Append(" = " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID);
                Mi_Sql.Append(" WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID);
                Mi_Sql.Append(" = '" + Recibo_Negocio.P_Referencia + "'");

                Dt_Encabezado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                // Obtenemos los datos de los detalles
                if (Dt_Encabezado.Rows.Count > 0)
                {
                    Dt_Contribuyente = Consultar_Datos_Contribuyente(Recibo_Negocio.P_Referencia);
                    if (Dt_Contribuyente.Rows.Count > 0)
                    {
                        Dt_Detalles = Consultar_Detalles_Recibo(Dt_Encabezado.Rows[0]["CUENTA_PREDIAL"].ToString(), Recibo_Negocio.P_No_Pago);
                        if (Dt_Detalles.Rows.Count > 0)
                        {
                            Dt_Descuentos = Consultar_Datos_Decuentos(Recibo_Negocio.P_Referencia);
                            Dt_Convenios = Consultar_Datos_Convenios(Recibo_Negocio.P_Referencia, Recibo_Negocio.P_No_Pago);
                            Clave_Movimientos = Consultar_Clave_Movimiento(Recibo_Negocio.P_Referencia);

                            //obtenemos los datos de la proteccion del recibo
                            Dt_Proteccion = Consultar_Proteccion_Pago(Recibo_Negocio.P_No_Pago);
                            if (Dt_Proteccion.Rows.Count > 0)
                            {
                                Dt_Datos_Recibo = Crear_Dt_Datos_Recibo_Cuenta(Dt_Encabezado, Dt_Contribuyente, Dt_Detalles, Dt_Descuentos, Dt_Convenios, Clave_Movimientos, Dt_Proteccion, Recibo_Negocio.P_No_Pago, Recibo_Negocio.P_Referencia);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos del encabezado del recibo de cuenta predial. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_Recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Predial
        //DESCRIPCIÓN          : Consulta para obtener los datos que se mostraran en el recibo de pago de una cuenta predial
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 2/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Contribuyente(String Cuenta_Predial_Id)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Contribuyente = new DataTable(); //datatable para almacenar los datos del contribuyente

            try
            {
                // Consultamos los datos del encabezado
                Mi_Sql.Append("SELECT NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + ", ");
                Mi_Sql.Append(Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Nombre + ") ||' " + " NO. " + "'|| ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", '') ||' " + "" + "'|| ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + ", ");
                Mi_Sql.Append(Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre + ") AS DOMICILIO, ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + ", ");
                Mi_Sql.Append(Cat_Pre_Estados.Tabla_Cat_Pre_Estados + "." + Cat_Pre_Estados.Campo_Nombre + ") AS ESTADO, ");
                Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + ", ");
                Mi_Sql.Append(Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + "." + Cat_Pre_Ciudades.Campo_Nombre + ") AS CIUDAD, ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' " + "" + "'|| ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' " + "" + "'|| ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS CONTRIBUYENTE, ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_RFC);
                Mi_Sql.Append(" FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios);
                Mi_Sql.Append(" ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID);
                Mi_Sql.Append(" = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes);
                Mi_Sql.Append(" ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID);
                Mi_Sql.Append(" = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles);
                Mi_Sql.Append(" ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion);
                Mi_Sql.Append(" = " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Calle_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias);
                Mi_Sql.Append(" ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion);
                Mi_Sql.Append(" = " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Colonia_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados);
                Mi_Sql.Append(" ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion);
                Mi_Sql.Append(" = " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados + "." + Cat_Pre_Estados.Campo_Estado_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades);
                Mi_Sql.Append(" ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion);
                Mi_Sql.Append(" = " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + "." + Cat_Pre_Ciudades.Campo_Ciudad_ID);
                Mi_Sql.Append(" WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_Id + "'");
                Mi_Sql.Append(" AND (" + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO'");
                Mi_Sql.Append(" OR " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " = 'POSEEDOR')");

                Dt_Contribuyente = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos del contribuyente del encabezado del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Contribuyente;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Decuentos
        //DESCRIPCIÓN          : Consulta para obtener los datos de los descuentos que se mostraran en el recibo 
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 2/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Decuentos(String Cuenta_Predial_Id)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Descuentos = new DataTable(); //datatable para almacenar los datos del contribuyente

            try
            {
                // Consultamos los datos del encabezado
                Mi_Sql.Append("SELECT NVL(" + Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo + ", '') AS DESC_RECARGO, ");
                Mi_Sql.Append("NVL(" + Ope_Pre_Descuentos_Predial.Campo_Desc_Recargo_Moratorio + ", '') AS DESC_RECARGO_MORATORIO, ");
                Mi_Sql.Append("NVL(" + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo + ", '') AS POR_RECARGO, ");
                Mi_Sql.Append("NVL(" + Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo_Moratorio + ", '') AS POR_RECARGO_MORATORIO, ");
                Mi_Sql.Append(Ope_Pre_Descuentos_Predial.Campo_Usuario_Creo);
                Mi_Sql.Append(" FROM " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial);
                Mi_Sql.Append(" WHERE " + Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_Id + "'");

                Dt_Descuentos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos de los descuentos del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Descuentos;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Decuentos_Traslado
        //DESCRIPCIÓN          : Consulta para obtener los datos de los descuentos que se mostraran en el recibo 
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 2/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Decuentos_Traslado(String Referencia)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Descuentos = new DataTable(); //datatable para almacenar los datos del contribuyente

            try
            {
                // Consultamos los datos del encabezado
                Mi_Sql.Append("SELECT NVL(DESCU." + Ope_Pre_Descuento_Traslado.Campo_Desc_Recargo + ", 0) AS DESC_RECARGO, ");
                Mi_Sql.Append("(CAL." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos + " - NVL(DESCU." + Ope_Pre_Descuento_Traslado.Campo_Monto_Recargos + ", 0)) AS MONTO_RECARGO, ");
                Mi_Sql.Append("NVL(DESCU." + Ope_Pre_Descuento_Traslado.Campo_Desc_Multa + ", 0) AS DESC_MULTA, ");
                Mi_Sql.Append("(CAL." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa + " - NVL(DESCU." + Ope_Pre_Descuento_Traslado.Campo_Monto_Multas + ", 0)) AS MONTO_MULTA, ");
                Mi_Sql.Append("DESCU." + Ope_Pre_Descuento_Traslado.Campo_Usuario_Creo + ", ");
                Mi_Sql.Append("CAL." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos + " AS CAL_RECARGOS, ");
                Mi_Sql.Append("CAL." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa + " AS CAL_MULTAS");
                Mi_Sql.Append(" FROM " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + " DESCU ");
                Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " CAL ON DESCU." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo + "=CAL." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " AND DESCU." + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo + "=CAL." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo);
                Mi_Sql.Append(" WHERE DESCU." + Ope_Pre_Descuento_Traslado.Campo_Referencia + " = '" + Referencia + "' AND DESCU." + Ope_Pre_Descuento_Traslado.Campo_Estatus + " NOT IN('CANCELADO')");

                Dt_Descuentos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos de los descuentos del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Descuentos;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Decuentos_Derechos
        //DESCRIPCIÓN          : Consulta para obtener los datos de los descuentos que se mostraran en el recibo 
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 2/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Decuentos_Derechos(String No_Calculo)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Descuentos = new DataTable(); //datatable para almacenar los datos del contribuyente

            try
            {
                // Consultamos los datos del encabezado
                Mi_Sql.Append("SELECT NVL(DESCU." + Ope_Pre_Descuento_Der_Sup.Campo_Desc_Recargo + ", '0') AS DESC_RECARGO, ");
                Mi_Sql.Append("NVL(DESCU." + Ope_Pre_Descuento_Der_Sup.Campo_Monto_Recargos + ", '0') AS MONTO_RECARGOS, ");
                Mi_Sql.Append("NVL(DESCU." + Ope_Pre_Descuento_Der_Sup.Campo_Desc_Multa + ", '0') AS DESC_MULTA, ");
                Mi_Sql.Append("NVL(DESCU." + Ope_Pre_Descuento_Der_Sup.Campo_Monto_Multas + ", '0') AS MONTO_MULTAS, ");
                Mi_Sql.Append("DESCU." + Ope_Pre_Descuento_Der_Sup.Campo_Usuario_Creo + "");
                //Mi_Sql.Append("NVL(SUM((SELECT CMUL." + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Monto + " FROM " + Cat_Pre_Multas_Derechos_Supervision_Detalles.Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles + " CMUL WHERE CMUL" + Cat_Pre_Multas_Derechos_Supervision_Detalles.Campo_Multa_Cuota_ID + " = DET." + Ope_Pre_Detalles_Impuestos_Derechos_Supervision.Campo_Multas_Id + ")), 0.00) AS CAL_MULTA");
                Mi_Sql.Append(" FROM " + Ope_Pre_Descuento_Der_Sup.Tabla_Ope_Pre_Descuento_Der_Sup + " DESCU ");
                Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + " CAL ON DESCU." + Ope_Pre_Descuento_Der_Sup.Campo_No_Impuesto_Derecho_Supervision + "=CAL." + Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision);
                Mi_Sql.Append(" WHERE DESCU." + Ope_Pre_Descuento_Der_Sup.Campo_No_Impuesto_Derecho_Supervision + " = '" + No_Calculo + "' AND DESCU." + Ope_Pre_Descuento_Der_Sup.Campo_Estatus + " NOT IN ('CANCELADO','BAJA')");

                Dt_Descuentos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos de los descuentos del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Descuentos;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Decuentos_Fracc
        //DESCRIPCIÓN          : Consulta para obtener los datos de los descuentos que se mostraran en el recibo 
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 2/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Decuentos_Fracc(String No_Calculo)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Descuentos = new DataTable(); //datatable para almacenar los datos del contribuyente

            try
            {
                // Consultamos los datos del encabezado
                Mi_Sql.Append("SELECT NVL(DESCU." + Ope_Pre_Descuento_Fracc.Campo_Desc_Recargo + ", 0) AS DESC_RECARGO, ");
                Mi_Sql.Append("NVL(DESCU." + Ope_Pre_Descuento_Fracc.Campo_Monto_Recargos + ", 0) AS MONTO_RECARGOS, ");
                Mi_Sql.Append("NVL(DESCU." + Ope_Pre_Descuento_Fracc.Campo_Desc_Multa + ", 0) AS DESC_MULTA, ");
                Mi_Sql.Append("NVL(DESCU." + Ope_Pre_Descuento_Fracc.Campo_Monto_Multas + ", 0) AS MONTO_MULTAS, ");
                Mi_Sql.Append("DESCU." + Ope_Pre_Descuento_Fracc.Campo_Usuario_Creo + ", ");
                Mi_Sql.Append(" FROM " + Ope_Pre_Descuento_Fracc.Tabla_Ope_Pre_Descuento_Fracc + " DESCU");
                Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + " CAL ON DESCU." + Ope_Pre_Descuento_Fracc.Campo_No_Impuesto_fraccionamiento + "=CAL." + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento);
                Mi_Sql.Append(" WHERE DESCU." + Ope_Pre_Descuento_Fracc.Campo_No_Impuesto_fraccionamiento + " = '" + No_Calculo + "' AND DESCU." + Ope_Pre_Descuento_Fracc.Campo_Estatus + " NOT IN ('CANCELADO','BAJA')");

                Dt_Descuentos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos de los descuentos del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Descuentos;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Convenios
        //DESCRIPCIÓN          : Consulta para obtener los datos de los convenios que se mostraran en el recibo 
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 2/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Convenios(String Cuenta_Predial_Id, String No_Pago)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Contvenio = new DataTable(); //datatable para almacenar los datos del contribuyente
            DataTable Dt_Pagos = new DataTable();
            Double Saldo = 0.00;
            DataTable Dt_Datos_Convenio = new DataTable();
            DataRow Fila;

            Dt_Datos_Convenio.Columns.Add("Datos_Convenio");
            Dt_Datos_Convenio.Columns.Add("Pago");
            Dt_Datos_Convenio.Columns.Add("Saldo");

            try
            {

                Mi_Sql.Append("SELECT DISTINCT " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + ", ");
                Mi_Sql.Append(Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades);
                Mi_Sql.Append(" FROM " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial);
                Mi_Sql.Append(" ," + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial);
                Mi_Sql.Append(" ," + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial);
                Mi_Sql.Append(" WHERE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + " = '" + Cuenta_Predial_Id + "'");
                Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'PAGADO'");
                Mi_Sql.Append(" AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + " = '" + No_Pago + "'");
                Mi_Sql.Append(" AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio);
                Mi_Sql.Append(" = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio);
                Mi_Sql.Append(" AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago);
                Mi_Sql.Append(" = " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago);
                Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio);
                Mi_Sql.Append(" = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio);
                Mi_Sql.Append(" ORDER BY " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " ASC");

                Dt_Contvenio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                if (Dt_Contvenio.Rows.Count > 0)
                {
                    Fila = Dt_Datos_Convenio.NewRow();
                    Fila["Datos_Convenio"] = "PAGO " + Dt_Contvenio.Rows[0]["NO_PAGO"] + " DE " + Dt_Contvenio.Rows[0]["NUMERO_PARCIALIDADES"];

                    Mi_Sql = new StringBuilder();
                    Mi_Sql.Append("SELECT DISTINCT " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios);
                    Mi_Sql.Append(" FROM " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial);
                    Mi_Sql.Append(" ," + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial);
                    Mi_Sql.Append(" ," + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial);
                    Mi_Sql.Append(" WHERE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + " = '" + Cuenta_Predial_Id + "'");
                    Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR'");
                    Mi_Sql.Append(" AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + " = '" + No_Pago + "'");
                    Mi_Sql.Append(" AND " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + "." + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio);
                    Mi_Sql.Append(" = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio);
                    Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + "." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio);
                    Mi_Sql.Append(" = " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + "." + Ope_Pre_Convenios_Predial.Campo_No_Convenio);
                    Mi_Sql.Append(" ORDER BY " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " ASC");

                    Dt_Pagos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                    if (Dt_Pagos.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Dt_Pagos.Rows)
                        {
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO_HONORARIOS"].ToString()) ? "0" : Dr["MONTO_HONORARIOS"]);
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO_IMPUESTO"].ToString()) ? "0" : Dr["MONTO_IMPUESTO"]);
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["RECARGOS_MORATORIOS"].ToString()) ? "0" : Dr["RECARGOS_MORATORIOS"]);
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["RECARGOS_ORDINARIOS"].ToString()) ? "0" : Dr["RECARGOS_ORDINARIOS"]);
                        }
                        Fila["Pago"] = "PROX PAGO: " + String.Format("{0:dd/MMMM/yyyy}", Dt_Pagos.Rows[0]["FECHA_VENCIMIENTO"]);

                    }
                    else
                    {
                        Saldo = 0.00;
                        Fila["Pago"] = "ULTIMO PAGO";
                    }
                    Fila["Saldo"] = String.Format("{0:c}", Saldo);
                    Dt_Datos_Convenio.Rows.Add(Fila);
                }
                else
                {
                    Mi_Sql = new StringBuilder();
                    Mi_Sql.Append("SELECT DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + ", DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + ", DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ", DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ", DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + ", CPRE." + Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades);
                    Mi_Sql.Append(" FROM " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " CPRE LEFT OUTER JOIN " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " DPRE ON CPRE." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + "=DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio);
                    Mi_Sql.Append(" WHERE CPRE." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + "='" + Cuenta_Predial_Id + "' AND DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + "='PAGADO' ORDER BY CPRE." + Ope_Pre_Convenios_Predial.Campo_Fecha_Modifico + " DESC, DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " ASC");

                    Dt_Contvenio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                    if (Dt_Contvenio.Rows.Count > 0)
                    {
                        Fila = Dt_Datos_Convenio.NewRow();
                        Fila["Datos_Convenio"] = "PAGO " + Dt_Contvenio.Rows[0]["NO_PAGO"] + " DE " + Dt_Contvenio.Rows[0]["NUMERO_PARCIALIDADES"];

                        Mi_Sql = new StringBuilder();
                        Mi_Sql.Append("SELECT DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + ", DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios + ", DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto + ", DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios + ", DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios + ", CPRE." + Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades + ", DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento);
                        Mi_Sql.Append(" FROM " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " CPRE LEFT OUTER JOIN " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial + " DPRE ON CPRE." + Ope_Pre_Convenios_Predial.Campo_No_Convenio + "=DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio);
                        Mi_Sql.Append(" WHERE CPRE." + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + "='" + Cuenta_Predial_Id + "' AND DPRE." + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + "='POR PAGAR'");

                        Dt_Pagos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                        if (Dt_Pagos.Rows.Count > 0)
                        {
                            foreach (DataRow Dr in Dt_Pagos.Rows)
                            {
                                Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO_HONORARIOS"].ToString()) ? "0" : Dr["MONTO_HONORARIOS"]);
                                Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO_IMPUESTO"].ToString()) ? "0" : Dr["MONTO_IMPUESTO"]);
                                Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["RECARGOS_MORATORIOS"].ToString()) ? "0" : Dr["RECARGOS_MORATORIOS"]);
                                Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["RECARGOS_ORDINARIOS"].ToString()) ? "0" : Dr["RECARGOS_ORDINARIOS"]);
                            }
                            Fila["Pago"] = "PROX PAGO: " + String.Format("{0:dd/MMMM/yyyy}", Dt_Pagos.Rows[0]["FECHA_VENCIMIENTO"]);

                        }
                        else
                        {
                            Saldo = 0.00;
                            Fila["Pago"] = "ULTIMO PAGO";
                        }
                        Fila["Saldo"] = String.Format("{0:c}", Saldo);
                        Dt_Datos_Convenio.Rows.Add(Fila);
                    }
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos de los convenios del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_Convenio;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Convenios_Traslado
        //DESCRIPCIÓN          : Consulta para obtener los datos de los convenios de traslado que se mostraran en el recibo 
        //PARAMETROS           :   
        //CREO                 : Ismael Prieto Sánchez
        //FECHA_CREO           : 14/Diciembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Convenios_Traslado(String Cuenta_Predial_Id, String No_Pago)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Contvenio = new DataTable(); //datatable para almacenar los datos del contribuyente
            DataTable Dt_Pagos = new DataTable();
            Double Saldo = 0.00;
            DataTable Dt_Datos_Convenio = new DataTable();
            DataRow Fila;

            Dt_Datos_Convenio.Columns.Add("Datos_Convenio");
            Dt_Datos_Convenio.Columns.Add("Pago");
            Dt_Datos_Convenio.Columns.Add("Saldo");

            try
            {

                Mi_Sql.Append("SELECT DISTINCT " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago + ", ");
                Mi_Sql.Append(Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + ", ");
                Mi_Sql.Append(Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_Numero_Parcialidades);
                Mi_Sql.Append(" FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio);
                Mi_Sql.Append(" ," + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio);
                Mi_Sql.Append(" WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_Id + "'");
                Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'PAGADO'");
                Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago_Aplicado + " = '" + No_Pago + "'");
                Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio);
                Mi_Sql.Append(" = " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio);
                Mi_Sql.Append(" ORDER BY " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago + " ASC");

                Dt_Contvenio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                if (Dt_Contvenio.Rows.Count > 0)
                {
                    Fila = Dt_Datos_Convenio.NewRow();
                    Fila["Datos_Convenio"] = "PAGO " + Dt_Contvenio.Rows[0]["NO_PAGO"] + " DE " + Dt_Contvenio.Rows[0]["NUMERO_PARCIALIDADES"];

                    Mi_Sql = new StringBuilder();
                    Mi_Sql.Append("SELECT DISTINCT " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Fecha_Vencimiento + ", ");
                    //Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Honorarios + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Impuesto + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Moratorios + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Ordinarios);
                    Mi_Sql.Append(" FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio);
                    Mi_Sql.Append(" ," + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio);
                    Mi_Sql.Append(" WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_Id + "'");
                    Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'POR PAGAR'");
                    Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + Dt_Contvenio.Rows[0]["NO_CONVENIO"] + "'");
                    Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + "." + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio);
                    Mi_Sql.Append(" = " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio);
                    Mi_Sql.Append(" ORDER BY " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago + " ASC");

                    Dt_Pagos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                    if (Dt_Pagos.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Dt_Pagos.Rows)
                        {
                            //Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO_HONORARIOS"].ToString()) ? "0" : Dr["MONTO_HONORARIOS"]);
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO_IMPUESTO"].ToString()) ? "0" : Dr["MONTO_IMPUESTO"]);
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["RECARGOS_MORATORIOS"].ToString()) ? "0" : Dr["RECARGOS_MORATORIOS"]);
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["RECARGOS_ORDINARIOS"].ToString()) ? "0" : Dr["RECARGOS_ORDINARIOS"]);
                        }
                        Fila["Pago"] = "PROX PAGO: " + String.Format("{0:dd/MMMM/yyyy}", Dt_Pagos.Rows[0]["FECHA_VENCIMIENTO"]);

                    }
                    else
                    {
                        Saldo = 0.00;
                        Fila["Pago"] = "ULTIMO PAGO";
                    }
                    Fila["Saldo"] = String.Format("{0:c}", Saldo);
                    Dt_Datos_Convenio.Rows.Add(Fila);
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos de los convenios del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_Convenio;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Convenios_Fraccionamientos
        //DESCRIPCIÓN          : Consulta para obtener los datos de los convenios de fraccionamiento que se mostraran en el recibo 
        //PARAMETROS           :   
        //CREO                 : Ismael Prieto Sánchez
        //FECHA_CREO           : 14/Diciembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Convenios_Fraccionamientos(String Cuenta_Predial_Id, String No_Pago)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Contvenio = new DataTable(); //datatable para almacenar los datos del contribuyente
            DataTable Dt_Pagos = new DataTable();
            Double Saldo = 0.00;
            DataTable Dt_Datos_Convenio = new DataTable();
            DataRow Fila;

            Dt_Datos_Convenio.Columns.Add("Datos_Convenio");
            Dt_Datos_Convenio.Columns.Add("Pago");
            Dt_Datos_Convenio.Columns.Add("Saldo");

            try
            {

                Mi_Sql.Append("SELECT DISTINCT " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + ", ");
                Mi_Sql.Append(Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + ", ");
                Mi_Sql.Append(Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_Numero_Parcialidades);
                Mi_Sql.Append(" FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos);
                Mi_Sql.Append(" ," + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos);
                Mi_Sql.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_Id + "'");
                Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'PAGADO'");
                Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago_Aplicado + " = '" + No_Pago + "'");
                Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio);
                Mi_Sql.Append(" = " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio);
                Mi_Sql.Append(" ORDER BY " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " ASC");

                Dt_Contvenio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                if (Dt_Contvenio.Rows.Count > 0)
                {
                    Fila = Dt_Datos_Convenio.NewRow();
                    Fila["Datos_Convenio"] = "PAGO " + Dt_Contvenio.Rows[0]["NO_PAGO"] + " DE " + Dt_Contvenio.Rows[0]["NUMERO_PARCIALIDADES"];

                    Mi_Sql = new StringBuilder();
                    Mi_Sql.Append("SELECT DISTINCT " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento + ", ");
                    //Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Honorarios + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios);
                    Mi_Sql.Append(" FROM " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos);
                    Mi_Sql.Append(" ," + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos);
                    Mi_Sql.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_Id + "'");
                    Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR'");
                    Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Dt_Contvenio.Rows[0]["NO_CONVENIO"] + "'");
                    Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + "." + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio);
                    Mi_Sql.Append(" = " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + "." + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio);
                    Mi_Sql.Append(" ORDER BY " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " ASC");

                    Dt_Pagos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                    if (Dt_Pagos.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Dt_Pagos.Rows)
                        {
                            //Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO_HONORARIOS"].ToString()) ? "0" : Dr["MONTO_HONORARIOS"]);
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO_IMPUESTO"].ToString()) ? "0" : Dr["MONTO_IMPUESTO"]);
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["RECARGOS_MORATORIOS"].ToString()) ? "0" : Dr["RECARGOS_MORATORIOS"]);
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["RECARGOS_ORDINARIOS"].ToString()) ? "0" : Dr["RECARGOS_ORDINARIOS"]);
                        }
                        Fila["Pago"] = "PROX PAGO: " + String.Format("{0:dd/MMMM/yyyy}", Dt_Pagos.Rows[0]["FECHA_VENCIMIENTO"]);

                    }
                    else
                    {
                        Saldo = 0.00;
                        Fila["Pago"] = "ULTIMO PAGO";
                    }
                    Fila["Saldo"] = String.Format("{0:c}", Saldo);
                    Dt_Datos_Convenio.Rows.Add(Fila);
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos de los convenios del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_Convenio;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Convenios_Derechos_Supervision
        //DESCRIPCIÓN          : Consulta para obtener los datos de los convenios de derechos de supervision que se mostraran en el recibo 
        //PARAMETROS           :   
        //CREO                 : Ismael Prieto Sánchez
        //FECHA_CREO           : 14/Diciembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Convenios_Derechos_Supervision(String Cuenta_Predial_Id, String No_Pago)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Contvenio = new DataTable(); //datatable para almacenar los datos del contribuyente
            DataTable Dt_Pagos = new DataTable();
            Double Saldo = 0.00;
            DataTable Dt_Datos_Convenio = new DataTable();
            DataRow Fila;

            Dt_Datos_Convenio.Columns.Add("Datos_Convenio");
            Dt_Datos_Convenio.Columns.Add("Pago");
            Dt_Datos_Convenio.Columns.Add("Saldo");

            try
            {

                Mi_Sql.Append("SELECT DISTINCT " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + ", ");
                Mi_Sql.Append(Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + ", ");
                Mi_Sql.Append(Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_Numero_Parcialidades);
                Mi_Sql.Append(" FROM " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision);
                Mi_Sql.Append(" ," + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision);
                Mi_Sql.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_Id + "'");
                Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'PAGADO'");
                Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago_Aplicado + " = '" + No_Pago + "'");
                Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio);
                Mi_Sql.Append(" = " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio);
                Mi_Sql.Append(" ORDER BY " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " ASC");

                Dt_Contvenio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                if (Dt_Contvenio.Rows.Count > 0)
                {
                    Fila = Dt_Datos_Convenio.NewRow();
                    Fila["Datos_Convenio"] = "PAGO " + Dt_Contvenio.Rows[0]["NO_PAGO"] + " DE " + Dt_Contvenio.Rows[0]["NUMERO_PARCIALIDADES"];

                    Mi_Sql = new StringBuilder();
                    Mi_Sql.Append("SELECT DISTINCT " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Vencimiento + ", ");
                    //Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Monto_Honorarios + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Monto_Impuesto + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Moratorios + ", ");
                    Mi_Sql.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Recargos_Ordinarios);
                    Mi_Sql.Append(" FROM " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision);
                    Mi_Sql.Append(" ," + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision);
                    Mi_Sql.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_Id + "'");
                    Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR'");
                    Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + Dt_Contvenio.Rows[0]["NO_CONVENIO"] + "'");
                    Mi_Sql.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + "." + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio);
                    Mi_Sql.Append(" = " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + "." + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio);
                    Mi_Sql.Append(" ORDER BY " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " ASC");

                    Dt_Pagos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                    if (Dt_Pagos.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Dt_Pagos.Rows)
                        {
                            //Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO_HONORARIOS"].ToString()) ? "0" : Dr["MONTO_HONORARIOS"]);
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO_IMPUESTO"].ToString()) ? "0" : Dr["MONTO_IMPUESTO"]);
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["RECARGOS_MORATORIOS"].ToString()) ? "0" : Dr["RECARGOS_MORATORIOS"]);
                            Saldo += Convert.ToDouble(string.IsNullOrEmpty(Dr["RECARGOS_ORDINARIOS"].ToString()) ? "0" : Dr["RECARGOS_ORDINARIOS"]);
                        }
                        Fila["Pago"] = "PROX PAGO: " + String.Format("{0:dd/MMMM/yyyy}", Dt_Pagos.Rows[0]["FECHA_VENCIMIENTO"]);

                    }
                    else
                    {
                        Saldo = 0.00;
                        Fila["Pago"] = "ULTIMO PAGO";
                    }
                    Fila["Saldo"] = String.Format("{0:c}", Saldo);
                    Dt_Datos_Convenio.Rows.Add(Fila);
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos de los convenios del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_Convenio;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Clave_Movimiento
        //DESCRIPCIÓN          : Consulta para obtener los datos de la clave de movimiento 
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 2/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static String Consultar_Clave_Movimiento(String Cuenta_Predial_Id)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Orden_Variacion = new DataTable(); //datatable para almacenar los datos de las ordenes de variacion
            String Clave_Movimiento = String.Empty;

            try
            {
                // Consultamos los datos del encabezado
                Mi_Sql.Append("SELECT " + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID);
                Mi_Sql.Append(" FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion);
                Mi_Sql.Append(" WHERE " + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_Id + "'");
                Mi_Sql.Append(" ORDER BY " + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + " DESC");

                Dt_Orden_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                Mi_Sql = new StringBuilder();
                Mi_Sql.Append("SELECT " + Cat_Pre_Movimientos.Campo_Identificador);
                Mi_Sql.Append(" FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos);
                Mi_Sql.Append(" WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = '" + Dt_Orden_Variacion.Rows[0]["MOVIMIENTO_ID"].ToString() + "'");
                Clave_Movimiento = (String)OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos de la clase de movimiento del recibo. Error: [" + Ex.Message + "]");
            }
            return Clave_Movimiento;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Clave_Movimiento
        //DESCRIPCIÓN          : Consulta para obtener los datos de la clave de movimiento 
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 2/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static DataTable Consultar_Parcialidades_Convenio_Pagadas(String No_Pago)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Orden_Variacion = new DataTable(); //datatable para almacenar los datos de las ordenes de variacion
            String Clave_Movimiento = String.Empty;

            try
            {
                // Consultamos los datos del encabezado
                Mi_Sql.Append("SELECT " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago);
                Mi_Sql.Append(" FROM " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial);
                Mi_Sql.Append(" WHERE " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + " = '" + No_Pago + "'");
                Mi_Sql.Append(" ORDER BY " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago + " ASC");

                Dt_Orden_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos de las parcialidades pagadas. Error: [" + Ex.Message + "]");
            }
            return Dt_Orden_Variacion;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Recibos_Constancias
        //DESCRIPCIÓN          : Consulta para obtener los datos que se mostraran en el recibo de pago
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 03/NOVIEMBRE/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Recibos_Constancias(Cls_Ope_Pre_Impresion_Recibo_Negocio Recibo_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Encabezado = new DataTable(); //datatable para almacenar los datos del encabezado del recibo
            DataTable Dt_Detalles = new DataTable(); //datatable para almacenar los datos de los detalles del pago
            DataTable Dt_Proteccion = new DataTable(); //datatable para almacenar los datos de la proteccion del pago
            DataTable Dt_Datos_Recibo = new DataTable(); // datatable para obtener todos los datos que se insertaran en el recibo

            try
            {
                Dt_Detalles = Consultar_Detalles_Recibo(Recibo_Negocio.P_Referencia, Recibo_Negocio.P_No_Pago);

                if (Dt_Detalles.Rows.Count > 0)
                {
                    // Consultamos los datos del encabezado
                    Mi_Sql.Append("SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ");
                    Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", '') AS NO_EXTERIOR, ");
                    Mi_Sql.Append("NVL(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", '') AS NO_INTERIOR, ");
                    Mi_Sql.Append("NVL(" + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Nombre + ", '') AS UBICACION, ");
                    Mi_Sql.Append("NVL(" + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre + ", '') AS COLONIA, ");
                    Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' " + "" + "'|| ");
                    Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' " + "" + "'|| ");
                    Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS CONTRIBUYENTE, ");
                    Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_RFC + ", ");
                    Mi_Sql.Append(Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Solicitante + ", ");
                    Mi_Sql.Append(Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Domicilio + ", ");
                    Mi_Sql.Append(Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Solicitante_RFC + " ");
                    Mi_Sql.Append(" FROM " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);
                    Mi_Sql.Append(" ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID);
                    Mi_Sql.Append(" = '" + Dt_Detalles.Rows[0]["CUENTA_PREDIAL_ID"].ToString() + "'");
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles);
                    Mi_Sql.Append(" ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID);
                    Mi_Sql.Append(" = " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Calle_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias);
                    Mi_Sql.Append(" ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID);
                    Mi_Sql.Append(" = " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Colonia_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios);
                    Mi_Sql.Append(" ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID);
                    Mi_Sql.Append(" = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes);
                    Mi_Sql.Append(" ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID);
                    Mi_Sql.Append(" = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID);
                    Mi_Sql.Append(" AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO','POSEEDOR')");
                    Mi_Sql.Append(" WHERE " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Folio + " = '" + Recibo_Negocio.P_Referencia + "'");
                    Dt_Encabezado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }


                // Obtenemos los datos de los detalles
                if (Dt_Encabezado.Rows.Count > 0)
                {
                    //obtenemos los datos de la proteccion del recibo
                    Dt_Proteccion = Consultar_Proteccion_Pago(Recibo_Negocio.P_No_Pago);
                    if (Dt_Proteccion.Rows.Count > 0)
                    {
                        Dt_Datos_Recibo = Crear_Dt_Datos_Recibo_Constancia(Dt_Encabezado, Dt_Detalles, Dt_Proteccion, Recibo_Negocio.P_Referencia);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos del encabezado del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_Recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Recibos_Impuestos
        //DESCRIPCIÓN          : Consulta para obtener los datos que se mostraran en el recibo de pago
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 04/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Recibos_Impuestos(Cls_Ope_Pre_Impresion_Recibo_Negocio Recibo_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Encabezado = new DataTable(); //datatable para almacenar los datos del encabezado del recibo
            DataTable Dt_Detalles = new DataTable(); //datatable para almacenar los datos de los detalles del pago
            DataTable Dt_Proteccion = new DataTable(); //datatable para almacenar los datos de la proteccion del pago
            DataTable Dt_Datos_Recibo = new DataTable(); // datatable para obtener todos los datos que se insertaran en el recibo
            DataTable Dt_Convenios = new DataTable();//Datatable para obtener los datos de los convenios

            try
            {
                Dt_Detalles = Consultar_Detalles_Recibo(Recibo_Negocio.P_Referencia, Recibo_Negocio.P_No_Pago);

                if (Dt_Detalles.Rows.Count > 0)
                {
                    // Consultamos los datos del encabezado
                    Mi_Sql.Append("SELECT " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ");
                    Mi_Sql.Append(Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ");
                    Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' " + "" + "'|| ");
                    Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' " + "" + "'|| ");
                    Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS CONTRIBUYENTE, ");
                    Mi_Sql.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_RFC);
                    Mi_Sql.Append(" FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios);
                    Mi_Sql.Append(" ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID);
                    Mi_Sql.Append(" = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID);
                    Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes);
                    Mi_Sql.Append(" ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID);
                    Mi_Sql.Append(" = " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID);
                    Mi_Sql.Append(" WHERE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID);
                    Mi_Sql.Append(" = '" + Dt_Detalles.Rows[0]["CUENTA_PREDIAL_ID"].ToString() + "'");
                    Mi_Sql.Append(" AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO','POSEEDOR')");

                    Dt_Encabezado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                    // Obtenemos los datos de los detalles
                    if (Dt_Encabezado.Rows.Count > 0)
                    {
                        if (Recibo_Negocio.P_Referencia.StartsWith("DER"))
                        {
                            Dt_Convenios = Consultar_Datos_Convenios_Derechos_Supervision(Dt_Encabezado.Rows[0]["CUENTA_PREDIAL_ID"].ToString(), Recibo_Negocio.P_No_Pago);
                        }
                        else if (Recibo_Negocio.P_Referencia.StartsWith("IMP"))
                        {
                            Dt_Convenios = Consultar_Datos_Convenios_Fraccionamientos(Dt_Encabezado.Rows[0]["CUENTA_PREDIAL_ID"].ToString(), Recibo_Negocio.P_No_Pago);
                        }

                        //obtenemos los datos de la proteccion del recibo
                        Dt_Proteccion = Consultar_Proteccion_Pago(Recibo_Negocio.P_No_Pago);
                        if (Dt_Proteccion.Rows.Count > 0)
                        {
                            Dt_Datos_Recibo = Crear_Dt_Datos_Recibo_Impuestos(Dt_Encabezado, Dt_Detalles, Dt_Proteccion, Dt_Convenios);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos del encabezado del recibo de impuestos. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_Recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Recibos_Otros_Pagos
        //DESCRIPCIÓN          : Consulta para obtener los datos que se mostraran en el recibo de pago
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 04/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Recibos_Otros_Pagos(Cls_Ope_Pre_Impresion_Recibo_Negocio Recibo_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Detalles = new DataTable(); //datatable para almacenar los datos de los detalles del pago
            DataTable Dt_Proteccion = new DataTable(); //datatable para almacenar los datos de la proteccion del pago
            DataTable Dt_Datos_Recibo = new DataTable(); // datatable para obtener todos los datos que se insertaran en el recibo

            try
            {
                Dt_Detalles = Consultar_Detalles_Recibo(Recibo_Negocio.P_Referencia, Recibo_Negocio.P_No_Pago);

                if (Dt_Detalles.Rows.Count > 0)
                {
                    //obtenemos los datos de la proteccion del recibo
                    Dt_Proteccion = Consultar_Proteccion_Pago(Recibo_Negocio.P_No_Pago);
                    if (Dt_Proteccion.Rows.Count > 0)
                    {
                        Dt_Datos_Recibo = Crear_Dt_Datos_Recibo_Otros_Pagos(Dt_Detalles, Dt_Proteccion);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos del encabezado del recibo de impuestos. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_Recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Recibos_Ingresos
        //DESCRIPCIÓN          : Consulta para obtener los datos que se mostraran en el recibo de pago
        //PARAMETROS           :   
        //CREO                 : Antonio Salvador Benavides Guardado
        //FECHA_CREO           : 17/Junio/2012
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Recibos_Ingresos(Cls_Ope_Pre_Impresion_Recibo_Negocio Recibo_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Detalles = new DataTable(); //datatable para almacenar los datos de los detalles del pago
            DataTable Dt_Proteccion = new DataTable(); //datatable para almacenar los datos de la proteccion del pago
            DataTable Dt_Datos_Recibo = new DataTable(); // datatable para obtener todos los datos que se insertaran en el recibo

            try
            {
                Dt_Detalles = Consultar_Detalles_Recibo(Recibo_Negocio.P_Referencia, Recibo_Negocio.P_No_Pago);

                if (Dt_Detalles.Rows.Count > 0)
                {
                    //obtenemos los datos de la proteccion del recibo
                    Dt_Proteccion = Consultar_Proteccion_Pago(Recibo_Negocio.P_No_Pago);
                    if (Dt_Proteccion.Rows.Count > 0)
                    {
                        Dt_Datos_Recibo = Crear_Dt_Datos_Recibo_Ingresos(Dt_Detalles, Dt_Proteccion, Recibo_Negocio.P_Referencia);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos del encabezado del recibo de impuestos. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_Recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Recibos_Tramites
        //DESCRIPCIÓN          : Consulta para obtener los datos que se mostraran en el recibo de pago
        //PARAMETROS           :   
        //CREO                 : Antonio Salvador Benavides Guardado
        //FECHA_CREO           : 04/Agosto/2012
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Recibos_Tramites(Cls_Ope_Pre_Impresion_Recibo_Negocio Recibo_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Detalles = new DataTable(); //datatable para almacenar los datos de los detalles del pago
            DataTable Dt_Proteccion = new DataTable(); //datatable para almacenar los datos de la proteccion del pago
            DataTable Dt_Datos_Recibo = new DataTable(); // datatable para obtener todos los datos que se insertaran en el recibo

            try
            {
                Dt_Detalles = Consultar_Detalles_Recibo(Recibo_Negocio.P_Referencia, Recibo_Negocio.P_No_Pago);

                if (Dt_Detalles.Rows.Count > 0)
                {
                    //obtenemos los datos de la proteccion del recibo
                    Dt_Proteccion = Consultar_Proteccion_Pago(Recibo_Negocio.P_No_Pago);
                    if (Dt_Proteccion.Rows.Count > 0)
                    {
                        Dt_Datos_Recibo = Crear_Dt_Datos_Recibo_Ingresos(Dt_Detalles, Dt_Proteccion, Recibo_Negocio.P_Referencia);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos del encabezado del recibo de impuestos. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_Recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Consultar_Datos_Recibos_Cancelado
        //DESCRIPCIÓN          : Consulta para obtener los datos que se mostraran en el recibo de pago
        //PARAMETROS           :   
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 04/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Consultar_Datos_Recibos_Cancelado(Cls_Ope_Pre_Impresion_Recibo_Negocio Recibo_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder(); //string para almacenar la consulta
            DataTable Dt_Proteccion = new DataTable(); //datatable para almacenar los datos de la proteccion del pago
            DataTable Dt_Datos_Recibo = new DataTable(); // datatable para obtener todos los datos que se insertaran en el recibo

            try
            {
                Dt_Proteccion = Consultar_Proteccion_Pago(Recibo_Negocio.P_No_Pago);
                if (Dt_Proteccion.Rows.Count > 0)
                {
                    Dt_Datos_Recibo = Crear_Dt_Datos_Recibo_Cancelacion(Dt_Proteccion);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los datos del recibo de cancelacion. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_Recibo;
        }
        #endregion

        #region METODOS
        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Crear_Dt_Datos_Recibo
        //DESCRIPCIÓN          : Metodo para juntar los datos del recibo en un solo datatable
        //PARAMETROS           1 Dt_Encabezado: datatable que contiene los datos del encabezado del recibo
        //                     2 Dt_Detalles: datatable que contiene los datos del detalle del pago del recibo
        //                     3 Dt_Proteccion: datatable que contiene los datos de la proteccion del recibo
        //                     4 Dt_Convenios: datatable que contiene los datos del convenio
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 29/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Crear_Dt_Datos_Recibo(DataTable Dt_Encabezado, DataTable Dt_Detalles, DataTable Dt_Proteccion, DataTable Dt_Convenios)
        {
            DataTable Dt_Datos_recibo = new DataTable();
            DataRow Fila;
            Double Subtotal = 0.00;
            Double Ajuste_Tarifario = 0.00;
            Double Base_Gravable = 0.00;
            Double Base_Impuesto = 0.00;
            Double Minimo_Elevado_Anio = 0.00;
            String Proteccion = String.Empty;
            Boolean Ajuste = false;

            // creamos las columnas del datatable
            Dt_Datos_recibo.Columns.Add("Tipo"); //tipo de elemento del recino (encabezado, detalle, pie de pagina, proteccion)
            Dt_Datos_recibo.Columns.Add("Nombre");//nombre del campo
            Dt_Datos_recibo.Columns.Add("Descripcion");//descripcion del campo
            try
            {
                //obtenemos los datos del encabezado
                if (!String.IsNullOrEmpty(Dt_Proteccion.Rows[0]["FECHA"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "FECHA:";
                    Fila["Descripcion"] = Crear_Fecha(String.Format("{0:dd/MM/yyyy}", Dt_Proteccion.Rows[0]["FECHA"]));
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["CUENTA_PREDIAL"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CUENTA:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["CUENTA_PREDIAL"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }


                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["CONTRIBUYENTE"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CONTRIBUYENTE:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["CONTRIBUYENTE"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["RFC"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "R.F.C.:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["RFC"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                //Obtenemos los detalles del pago
                foreach (DataRow Dr in Dt_Detalles.Rows)
                {
                    if (Dr["DESCRIPCION"].ToString() != "AJUSTE TARIFARIO")
                    {
                        Fila = Dt_Datos_recibo.NewRow();
                        Fila["Tipo"] = "Detalle";
                        if (Dr["FUNDAMENTO"].ToString() != "")
                        {
                            Fila["Nombre"] = Dr["DESCRIPCION"].ToString().ToUpper() + "(" + Dr["FUNDAMENTO"].ToString().ToUpper() + ")";
                        }
                        else
                        {
                            Fila["Nombre"] = Dr["DESCRIPCION"].ToString().ToUpper();
                        }
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Dr["MONTO"]);
                        Dt_Datos_recibo.Rows.Add(Fila);

                        Subtotal = Subtotal + Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                    }
                    else
                    {
                        Ajuste_Tarifario = Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                        Ajuste = true;
                    }
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "SUBTOTAL";
                Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Subtotal);
                Dt_Datos_recibo.Rows.Add(Fila);

                if (Ajuste)
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    if (Ajuste_Tarifario > 0)
                    {
                        Fila["Descripcion"] = string.Format("{0:+ #,###,##0.00}", Ajuste_Tarifario);
                    }
                    else
                    {
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);
                }
                else
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "<div class='Leyenta_Total'>TOTAL</div>";
                Fila["Descripcion"] = string.Format("<div class='Monto_Total'>{0:#,###,##0.00}</div>", Dt_Proteccion.Rows[0]["TOTAL"]);
                Dt_Datos_recibo.Rows.Add(Fila);

                //obtenemos el pie de pagina
                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["BASE_IMPUESTO"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Pie_Pagina";
                    Fila["Nombre"] = "BASE IMPUESTO:";
                    Fila["Descripcion"] = string.Format("{0:c}", Dt_Encabezado.Rows[0]["BASE_IMPUESTO"]);
                    Dt_Datos_recibo.Rows.Add(Fila);
                }


                Base_Impuesto = Convert.ToDouble(string.IsNullOrEmpty(Dt_Encabezado.Rows[0]["BASE_IMPUESTO"].ToString()) ? "0" : Dt_Encabezado.Rows[0]["BASE_IMPUESTO"].ToString());
                Minimo_Elevado_Anio = Convert.ToDouble(string.IsNullOrEmpty(Dt_Encabezado.Rows[0]["MINIMO_ELEVADO_ANIO"].ToString()) ? "0" : Dt_Encabezado.Rows[0]["MINIMO_ELEVADO_ANIO"].ToString());
                Base_Gravable = Base_Impuesto - Minimo_Elevado_Anio; //a la base impuesto se le resta el minimo elevado al año
                if (Base_Gravable < 0)
                {
                    Base_Gravable = 0;
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Pie_Pagina";
                Fila["Nombre"] = "BASE GRAVABLE:";
                Fila["Descripcion"] = string.Format("{0:c}", Base_Gravable);
                Dt_Datos_recibo.Rows.Add(Fila);

                //OBTENEMOS LOS DATOS DEL COMVENIO SI ES QUE EXISTE
                if (Dt_Convenios.Rows.Count > 0)
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Convenio";
                    Fila["Nombre"] = "DATOS DEL CONVENIO:";
                    Fila["Descripcion"] = Dt_Convenios.Rows[0]["Datos_Convenio"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);

                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Convenio";
                    Fila["Nombre"] = Dt_Convenios.Rows[0]["Pago"].ToString();
                    Fila["Descripcion"] = "SALDO: " + Dt_Convenios.Rows[0]["Saldo"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                //obtenemos la proteccion del recibo
                Proteccion = "<b>" + Dt_Proteccion.Rows[0]["ESTATUS"].ToString() + "</b>";
                Proteccion += "/" + Dt_Proteccion.Rows[0]["CONFRONTO"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_CAJA"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_OPERACION"].ToString() + "/";
                Proteccion += String.Format("{0:yyyy.MM.dd}", Dt_Proteccion.Rows[0]["FECHA"]) + "/";
                Proteccion += String.Format("{0:HH:mm:ss}", Dt_Proteccion.Rows[0]["FECHA_CREO"]) + "/";
                Proteccion += string.Format("{0:c}", Dt_Proteccion.Rows[0]["TOTAL"]) + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_RECIBO"].ToString() + "/";
                if (Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString() == "")
                {
                    Proteccion += Dt_Proteccion.Rows[0]["DOCUMENTO"].ToString();
                }
                else
                {
                    Proteccion += Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString();
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Proteccion";
                Fila["Nombre"] = "proteccion";
                Fila["Descripcion"] = Proteccion;
                Dt_Datos_recibo.Rows.Add(Fila);

                Dt_Datos_recibo = Validar_Importes_Vacios(Dt_Datos_recibo.Copy());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el datatable de los datos del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Crear_Fecha
        //DESCRIPCIÓN          : Consulta para obtener los detalles de un pago 
        //PARAMETROS           1 Fecha: fecha a la cual le daremos formato
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 29/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static String Crear_Fecha(String Fecha)
        {
            String Fecha_Formateada = String.Empty;
            String Mes = String.Empty;
            String[] Fechas;

            try
            {
                Fechas = Fecha.Split('/');
                Mes = Fechas[1].ToString();
                switch (Mes)
                {
                    case "01":
                        Mes = "ENERO";
                        break;
                    case "02":
                        Mes = "FEBRERO";
                        break;
                    case "03":
                        Mes = "MARZO";
                        break;
                    case "04":
                        Mes = "ABRIL";
                        break;
                    case "05":
                        Mes = "MAYO";
                        break;
                    case "06":
                        Mes = "JUNIO";
                        break;
                    case "07":
                        Mes = "JULIO";
                        break;
                    case "08":
                        Mes = "AGOSTO";
                        break;
                    case "09":
                        Mes = "SEPTIEMBRE";
                        break;
                    case "10":
                        Mes = "OCTUBRE";
                        break;
                    case "11":
                        Mes = "NOVIEMBRE";
                        break;
                    default:
                        Mes = "DICIEMBRE";
                        break;
                }
                Fecha_Formateada = Fechas[0].ToString() + " DE " + Mes + " DE " + Fechas[2].ToString();
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al crear el fromato de fecha. Error: [" + Ex.Message + "]");
            }
            return Fecha_Formateada;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Crear_Dt_Datos_Recibo_Cuenta
        //DESCRIPCIÓN          : Metodo para juntar los datos del recibo en un solo datatable
        //PARAMETROS           1 Dt_Encabezado: datatable que contiene los datos del encabezado del recibo
        //                     2 Dt_Detalles: datatable que contiene los datos del detalle del pago del recibo
        //                     3 Dt_Proteccion: datatable que contiene los datos de la proteccion del recibo
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 03/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Crear_Dt_Datos_Recibo_Cuenta(DataTable Dt_Encabezado, DataTable Dt_Contribuyente, DataTable Dt_Detalles, DataTable Dt_Descuentos, DataTable Dt_Convenios, String Clave_Movimiento, DataTable Dt_Proteccion, String No_Pago, String Cuenta_Predial_Id)
        {
            DataTable Dt_Datos_recibo = new DataTable();
            DataRow Fila;
            Double Subtotal = 0.00;
            Double Ajuste_Tarifario = 0.00;
            String Proteccion = String.Empty;
            Boolean Ajuste = false;
            Boolean Recargo_Ordinario = false;
            Boolean Recargo_Moratorio = false;
            Boolean Honorario = false;
            Double Recargos_Moratorios = 0.00;
            Double Recargos_Ordinarios = 0.00;
            Double Honorarios_Cobranza = 0.00;
            Double Recargo = 0.00;
            String Periodos;

            // creamos las columnas del datatable
            Dt_Datos_recibo.Columns.Add("Tipo"); //tipo de elemento del recino (encabezado, detalle, pie de pagina, proteccion)
            Dt_Datos_recibo.Columns.Add("Nombre");//nombre del campo
            Dt_Datos_recibo.Columns.Add("Descripcion");//descripcion del campo
            try
            {
                //obtenemos los datos del encabezado
                if (!String.IsNullOrEmpty(Dt_Proteccion.Rows[0]["FECHA"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "FECHA:";
                    Fila["Descripcion"] = Crear_Fecha(String.Format("{0:dd/MM/yyyy}", Dt_Proteccion.Rows[0]["FECHA"]));
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["DESCRIPCION"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "TITULO:";
                    Fila["Descripcion"] = "IMPUESTO PREDIAL " + Dt_Encabezado.Rows[0]["DESCRIPCION"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["CUENTA_PREDIAL"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CUENTA:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["CUENTA_PREDIAL"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["COLONIA"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "COLONIA:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["COLONIA"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["UBICACION"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "UBICACION:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["UBICACION"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["NO_INTERIOR"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "INTERIOR:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["NO_INTERIOR"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }


                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["NO_EXTERIOR"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "EXTERIOR:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["NO_EXTERIOR"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["TASA_ANUAL"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "TASA:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["TASA_ANUAL"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["VALOR_FISCAL"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "VALOR FISCAL:";
                    Fila["Descripcion"] = string.Format("{0:c}", Dt_Encabezado.Rows[0]["VALOR_FISCAL"]);
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["CUOTA_ANUAL"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CUOTA BIMESTRAL:";
                    Fila["Descripcion"] = string.Format("{0:c}", (Convert.ToDouble(Dt_Encabezado.Rows[0]["CUOTA_ANUAL"]) / 6));
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["CUOTA_ANUAL"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CUOTA ANUAL:";
                    Fila["Descripcion"] = string.Format("{0:c}", Dt_Encabezado.Rows[0]["CUOTA_ANUAL"]);
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["EFECTOS"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "EFECTOS:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["EFECTOS"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Clave_Movimiento))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CLAVE MOVIMIENTO:";
                    Fila["Descripcion"] = Clave_Movimiento;
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Contribuyente.Rows[0]["CONTRIBUYENTE"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CONTRIBUYENTE:";
                    Fila["Descripcion"] = Dt_Contribuyente.Rows[0]["CONTRIBUYENTE"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }


                if (!String.IsNullOrEmpty(Dt_Contribuyente.Rows[0]["RFC"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "R.F.C.:";
                    Fila["Descripcion"] = Dt_Contribuyente.Rows[0]["RFC"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Contribuyente.Rows[0]["DOMICILIO"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "DOMICILIO:";
                    Fila["Descripcion"] = Dt_Contribuyente.Rows[0]["DOMICILIO"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Contribuyente.Rows[0]["CIUDAD"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CIUDAD:";
                    if (Dt_Contribuyente.Rows[0]["CIUDAD"].ToString() == "IRAPUATO")
                    {
                        Fila["Descripcion"] = Dt_Contribuyente.Rows[0]["CIUDAD"].ToString();
                    }
                    else
                    {
                        Fila["Descripcion"] = Dt_Contribuyente.Rows[0]["CIUDAD"].ToString() + " " + Dt_Contribuyente.Rows[0]["ESTADO"].ToString();
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                //Obtenemos los detalles del pago
                foreach (DataRow Dr in Dt_Detalles.Rows)
                {
                    if (Dr["DESCRIPCION"].ToString() == "AJUSTE TARIFARIO")
                    {
                        Ajuste_Tarifario = Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                        Ajuste = true;
                    }
                    else if (Dr["DESCRIPCION"].ToString().Contains("RECARGOS ORDINARIOS") || Dr["PERIODO"].ToString().Contains("RECARGOS ORDINARIOS"))
                    {
                        Recargos_Ordinarios = Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                        Recargo_Ordinario = true;
                        Subtotal = Subtotal + Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                    }
                    else if (Dr["DESCRIPCION"].ToString().Contains("RECARGOS MORATORIOS"))
                    {
                        Recargos_Moratorios = Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                        Recargo_Moratorio = true;
                        Subtotal = Subtotal + Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                    }
                    else if (Dr["DESCRIPCION"].ToString().Contains("HONORARIOS"))
                    {
                        Honorarios_Cobranza = Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                        Honorario = true;
                        Subtotal = Subtotal + Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                    }
                    else
                    {
                        Periodos = Dr["PERIODO"].ToString().Substring(Dr["PERIODO"].ToString().IndexOf("[") + 1);

                        if (Periodos != "DESCUENTO")
                        {


                            Fila = Dt_Datos_recibo.NewRow();
                            Fila["Tipo"] = "Detalle";
                            Fila["Nombre"] = "PERIODO";
                            Fila["Descripcion"] = Periodos.Substring(0, Periodos.Length - 1);
                            Dt_Datos_recibo.Rows.Add(Fila);

                            Fila = Dt_Datos_recibo.NewRow();
                            Fila["Tipo"] = "Detalle";
                            Fila["Nombre"] = Dr["DESCRIPCION"].ToString().ToUpper();
                            Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Dr["MONTO"]);
                            Dt_Datos_recibo.Rows.Add(Fila);

                            Subtotal = Subtotal + Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                        }
                    }
                }

                if (Convert.ToDouble(string.IsNullOrEmpty(Dt_Proteccion.Rows[0]["DESCUENTO_PRONTO_PAGO"].ToString()) ? "0" : Dt_Proteccion.Rows[0]["DESCUENTO_PRONTO_PAGO"]) > 0)
                {
                    Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
                    DataTable Dt_Descuento_Pronto_Pago = Resumen_Predio.Consultar_Descuentos_Pronto_Pago();
                    String Porcentaje_Descuento_Corriente = "%";

                    if (Dt_Descuento_Pronto_Pago.Rows.Count > 0)
                    {
                        if (Dt_Descuento_Pronto_Pago.Rows[0][DateTime.Now.ToString("MMMM")].ToString().Trim() != "0")
                        {
                            Porcentaje_Descuento_Corriente = Dt_Descuento_Pronto_Pago.Rows[0][DateTime.Now.ToString("MMMM")].ToString().Trim() + " %";
                        }
                    }

                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    //Fila["Nombre"] = "DESCUENTO PRONTO PAGO";
                    Fila["Nombre"] = "DESCUENTO " + Porcentaje_Descuento_Corriente;
                    Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Dt_Proteccion.Rows[0]["DESCUENTO_PRONTO_PAGO"]);
                    Dt_Datos_recibo.Rows.Add(Fila);
                    Subtotal = Subtotal - Convert.ToDouble(string.IsNullOrEmpty(Dt_Proteccion.Rows[0]["DESCUENTO_PRONTO_PAGO"].ToString()) ? "0" : Dt_Proteccion.Rows[0]["DESCUENTO_PRONTO_PAGO"].ToString());

                }

                if (Recargo_Moratorio)
                {
                    if (Dt_Descuentos.Rows.Count > 0)
                    {
                        //verificamos si existe descuento
                        if (Convert.ToDouble(string.IsNullOrEmpty(Dt_Descuentos.Rows[0]["DESC_RECARGO_MORATORIO"].ToString()) ? "0" : Dt_Descuentos.Rows[0]["DESC_RECARGO_MORATORIO"]) != 0)
                        {
                            Recargo = Recargos_Moratorios + Convert.ToDouble(string.IsNullOrEmpty(Dt_Descuentos.Rows[0]["DESC_RECARGO_MORATORIO"].ToString()) ? "0" : Dt_Descuentos.Rows[0]["DESC_RECARGO_MORATORIO"]);
                            Fila = Dt_Datos_recibo.NewRow();
                            Fila["Tipo"] = "Detalle_Pago";
                            Fila["Nombre"] = "RECARGOS MORATORIOS " + string.Format("{0:#,###,##0.00}", Recargo) + " - " + Dt_Descuentos.Rows[0]["POR_RECARGO_MORATORIO"].ToString() + "% DESCUENTO AUTORIZADO POR: " + Dt_Descuentos.Rows[0]["USUARIO_CREO"].ToString();
                            Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Recargos_Moratorios) + "<BR/>";
                            Dt_Datos_recibo.Rows.Add(Fila);
                        }
                        else
                        {
                            Fila = Dt_Datos_recibo.NewRow();
                            Fila["Tipo"] = "Detalle_Pago";
                            Fila["Nombre"] = "RECARGOS MORATORIOS";
                            Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Recargos_Moratorios);
                            Dt_Datos_recibo.Rows.Add(Fila);
                        }
                    }
                    else
                    {
                        Fila = Dt_Datos_recibo.NewRow();
                        Fila["Tipo"] = "Detalle_Pago";
                        Fila["Nombre"] = "RECARGOS MORATORIOS";
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Recargos_Moratorios);
                        Dt_Datos_recibo.Rows.Add(Fila);
                    }
                }

                if (Recargo_Ordinario)
                {
                    if (Dt_Descuentos.Rows.Count > 0)
                    {
                        //verificamos si existe descuento
                        if (Convert.ToDouble(string.IsNullOrEmpty(Dt_Descuentos.Rows[0]["DESC_RECARGO"].ToString()) ? "0" : Dt_Descuentos.Rows[0]["DESC_RECARGO"]) != 0)
                        {
                            Recargo = Recargos_Ordinarios + Convert.ToDouble(string.IsNullOrEmpty(Dt_Descuentos.Rows[0]["DESC_RECARGO"].ToString()) ? "0" : Dt_Descuentos.Rows[0]["DESC_RECARGO"]);
                            Fila = Dt_Datos_recibo.NewRow();
                            Fila["Tipo"] = "Detalle_Pago";
                            Fila["Nombre"] = "RECARGOS ORDINARIOS " + string.Format("{0:#,###,##0.00}", Recargo) + " - " + Dt_Descuentos.Rows[0]["POR_RECARGO"].ToString() + "% DESCUENTO AUTORIZADO POR: " + Dt_Descuentos.Rows[0]["USUARIO_CREO"].ToString();
                            Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Recargos_Ordinarios) + "<BR/>";
                            Dt_Datos_recibo.Rows.Add(Fila);
                        }
                        else
                        {
                            Fila = Dt_Datos_recibo.NewRow();
                            Fila["Tipo"] = "Detalle_Pago";
                            Fila["Nombre"] = "RECARGOS ORDINARIOS";
                            Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Recargos_Ordinarios);
                            Dt_Datos_recibo.Rows.Add(Fila);
                        }
                    }
                    else
                    {
                        Fila = Dt_Datos_recibo.NewRow();
                        Fila["Tipo"] = "Detalle_Pago";
                        Fila["Nombre"] = "RECARGOS ORDINARIOS";
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Recargos_Ordinarios);
                        Dt_Datos_recibo.Rows.Add(Fila);
                    }
                }
                if (Recargo_Ordinario == false && Recargo_Moratorio == false && Dt_Descuentos.Rows.Count > 0)
                {
                    try
                    {
                        if (Dt_Descuentos.Rows.Count > 0)
                        {
                            //verificamos si existe descuento
                            if (Convert.ToDouble(string.IsNullOrEmpty(Dt_Descuentos.Rows[0]["DESC_RECARGO_MORATORIO"].ToString()) ? "0" : Dt_Descuentos.Rows[0]["DESC_RECARGO_MORATORIO"]) != 0)
                            {
                                Recargo = Recargos_Moratorios + Convert.ToDouble(string.IsNullOrEmpty(Dt_Descuentos.Rows[0]["DESC_RECARGO_MORATORIO"].ToString()) ? "0" : Dt_Descuentos.Rows[0]["DESC_RECARGO_MORATORIO"]);
                                Fila = Dt_Datos_recibo.NewRow();
                                Fila["Tipo"] = "Detalle_Pago";
                                Fila["Nombre"] = "RECARGOS MORATORIOS " + string.Format("{0:#,###,##0.00}", Recargo) + " - " + Dt_Descuentos.Rows[0]["POR_RECARGO_MORATORIO"].ToString() + "% DESCUENTO AUTORIZADO POR: " + Dt_Descuentos.Rows[0]["USUARIO_CREO"].ToString();
                                Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Recargos_Moratorios) + "<BR/>";
                                Dt_Datos_recibo.Rows.Add(Fila);
                            }
                        }
                    }
                    catch
                    { }
                    try
                    {
                        if (Dt_Descuentos.Rows.Count > 0)
                        {
                            //verificamos si existe descuento
                            if (Convert.ToDouble(string.IsNullOrEmpty(Dt_Descuentos.Rows[0]["DESC_RECARGO"].ToString()) ? "0" : Dt_Descuentos.Rows[0]["DESC_RECARGO"]) != 0)
                            {
                                Recargo = Recargos_Ordinarios + Convert.ToDouble(string.IsNullOrEmpty(Dt_Descuentos.Rows[0]["DESC_RECARGO"].ToString()) ? "0" : Dt_Descuentos.Rows[0]["DESC_RECARGO"]);
                                Fila = Dt_Datos_recibo.NewRow();
                                Fila["Tipo"] = "Detalle_Pago";
                                Fila["Nombre"] = "RECARGOS ORDINARIOS " + string.Format("{0:#,###,##0.00}", Recargo) + " - " + Dt_Descuentos.Rows[0]["POR_RECARGO"].ToString() + "% DESCUENTO AUTORIZADO POR: " + Dt_Descuentos.Rows[0]["USUARIO_CREO"].ToString();
                                Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Recargos_Ordinarios) + "<BR/>";
                                Dt_Datos_recibo.Rows.Add(Fila);
                            }
                        }
                    }
                    catch
                    { }
                }

                if (Honorario)
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "HONORARIOS DE COBRANZA";
                    Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Honorarios_Cobranza);
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "SUBTOTAL";
                Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Subtotal);
                Dt_Datos_recibo.Rows.Add(Fila);

                if (Ajuste)
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    if (Ajuste_Tarifario > 0)
                    {
                        Fila["Descripcion"] = string.Format("{0:+ #,###,##0.00}", Ajuste_Tarifario);
                    }
                    else
                    {
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);
                }
                else
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "<div class='Leyenta_Total'>TOTAL</div>";
                Fila["Descripcion"] = string.Format("<div class='Monto_Total'>{0:#,###,##0.00}</div>", Dt_Proteccion.Rows[0]["TOTAL"]);
                Dt_Datos_recibo.Rows.Add(Fila);

                //obtenemos el pie de pagina que en este caso sera si el predio cuenta con cuota minima
                if (!string.IsNullOrEmpty(Dt_Encabezado.Rows[0]["CUOTA_FIJA"].ToString()) && Dt_Encabezado.Rows[0]["CUOTA_FIJA"].ToString() == "SI")
                {
                    if (Dt_Encabezado.Rows[0]["NO_CUOTA_FIJA"].ToString().Trim().Length == 10)
                    {
                        Cls_Cat_Pre_Casos_Especiales_Negocio Caso_Especial = new Cls_Cat_Pre_Casos_Especiales_Negocio();
                        String Consulta;
                        Consulta = "SELECT " + Cat_Pre_Casos_Especiales.Campo_Descripcion + " FROM " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + " WHERE " + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " IN (SELECT " + Ope_Pre_Cuotas_Fijas.Campo_Caso_Especial_Id + " FROM " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + " WHERE " + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + "='" + Dt_Encabezado.Rows[0]["NO_CUOTA_FIJA"].ToString() + "')";
                        Caso_Especial.P_Campos_Dinamicos = "NVL(" + Cat_Pre_Casos_Especiales.Campo_Observaciones + ",' ') AS " + Cat_Pre_Casos_Especiales.Campo_Observaciones;
                        Caso_Especial.P_Filtros_Dinamicos = Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " IN (SELECT " + Ope_Pre_Cuotas_Fijas.Campo_Caso_Especial_Id + " FROM " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + " WHERE " + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + "='" + Dt_Encabezado.Rows[0]["NO_CUOTA_FIJA"].ToString().Trim() + "')";
                        DataTable Dt_Caso = Caso_Especial.Consultar_Casos_Especiales();
                        Fila = Dt_Datos_recibo.NewRow();
                        Fila["Tipo"] = "Pie_Pagina";
                        Fila["Nombre"] = "CUOTA MINIMA:";
                        Fila["Descripcion"] = Dt_Caso.Rows[0][Cat_Pre_Casos_Especiales.Campo_Observaciones].ToString();
                        Dt_Datos_recibo.Rows.Add(Fila);
                    }
                    else if (Dt_Encabezado.Rows[0]["NO_CUOTA_FIJA"].ToString().Trim().Length == 5)
                    {
                        Cls_Cat_Pre_Casos_Especiales_Negocio Caso_Especial = new Cls_Cat_Pre_Casos_Especiales_Negocio();
                        //String Consulta;
                        //Consulta = "SELECT " + Cat_Pre_Casos_Especiales.Campo_Observaciones + " FROM " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + " WHERE " + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " IN (SELECT " + Ope_Pre_Cuotas_Fijas.Campo_Caso_Especial_Id + " FROM " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + " WHERE " + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + "='" + Dt_Encabezado.Rows[0]["NO_CUOTA_FIJA"].ToString() + "')";
                        Caso_Especial.P_Campos_Dinamicos = "NVL(" + Cat_Pre_Casos_Especiales.Campo_Descripcion + ",' ') AS " + Cat_Pre_Casos_Especiales.Campo_Observaciones;
                        Caso_Especial.P_Filtros_Dinamicos = Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + "='" + Dt_Encabezado.Rows[0]["NO_CUOTA_FIJA"].ToString().Trim() + "'";
                        DataTable Dt_Caso = Caso_Especial.Consultar_Casos_Especiales();
                        Fila = Dt_Datos_recibo.NewRow();
                        Fila["Tipo"] = "Pie_Pagina";
                        Fila["Nombre"] = "CUOTA MINIMA:";
                        Fila["Descripcion"] = Dt_Caso.Rows[0][Cat_Pre_Casos_Especiales.Campo_Observaciones].ToString();
                        Dt_Datos_recibo.Rows.Add(Fila);
                    }
                }
                else
                {
                    String Valores = "";
                    Valores = Obtener_Dato_Consulta_Valor(Cuenta_Predial_Id);
                    if (Valores != "")
                    {
                        String[] valores_cuota_minima = Valores.Split('-');
                        String Valor_Periodo1 = "";
                        String Valor_Periodo2 = "";
                        foreach (DataRow Dr_Renglon in Dt_Datos_recibo.Rows)
                        {
                            if (Dr_Renglon["Nombre"].ToString().Contains("PERIODO") && !Dr_Renglon["Descripcion"].ToString().Contains("RECARGOS"))
                            {
                                if (Valor_Periodo1 == "")
                                {
                                    Valor_Periodo1 = Dr_Renglon["Descripcion"].ToString().Substring(Dr_Renglon["Descripcion"].ToString().Length - 4);
                                }
                                else
                                {
                                    Valor_Periodo2 = Dr_Renglon["Descripcion"].ToString().Substring(Dr_Renglon["Descripcion"].ToString().Length - 4);
                                }
                            }
                        }
                        if (Valor_Periodo2 == "")
                        {
                            Valor_Periodo2 = "0";
                        }
                        if (Valor_Periodo1 == "")
                        {
                            Valor_Periodo1 = "0";
                        }
                        if (valores_cuota_minima[1].Trim() == "")
                        {
                            if (Convert.ToInt32(Valor_Periodo1) < Convert.ToInt32(Valor_Periodo2))
                            {
                                valores_cuota_minima[1] = Obtener_Dato_Consulta_Cuota(Valor_Periodo2);
                            }
                            else
                            {
                                valores_cuota_minima[1] = Obtener_Dato_Consulta_Cuota(Valor_Periodo1);
                            }
                        }
                        if (Convert.ToDouble(valores_cuota_minima[0]) < Convert.ToDouble(valores_cuota_minima[1]))
                        {
                            Fila = Dt_Datos_recibo.NewRow();
                            Fila["Tipo"] = "Pie_Pagina";
                            Fila["Nombre"] = "CUOTA MINIMA:";
                            Fila["Descripcion"] = "FUND. ART. 164 INC. E LEY DE HACIENDA.";
                            Dt_Datos_recibo.Rows.Add(Fila);
                        }
                    }
                }

                //OBTENEMOS LOS DATOS DEL COMVENIO SI ES QUE EXISTE
                if (Dt_Convenios.Rows.Count > 0)
                {
                    int No_Parcialidad = 0;

                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Convenio";
                    Fila["Nombre"] = "DATOS DEL CONVENIO:";
                    DataTable Dt_Pagos_Convenio = Consultar_Parcialidades_Convenio_Pagadas(No_Pago);
                    Fila["Descripcion"] = "PAGO ";
                    if (Dt_Pagos_Convenio.Rows.Count == 1)
                    {
                        Fila["Descripcion"] = Dt_Convenios.Rows[0]["Datos_Convenio"].ToString();
                    }
                    else
                    {
                        foreach (DataRow Dr_Actual in Dt_Pagos_Convenio.Rows)
                        {
                            if (No_Parcialidad != Convert.ToInt16(Dr_Actual[Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago].ToString()))
                            {
                                Fila["Descripcion"] = Fila["Descripcion"].ToString() + Dr_Actual[Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago].ToString() + "-";
                                No_Parcialidad = Convert.ToInt16(Dr_Actual[Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago].ToString());
                            }
                        }
                        String[] Ultima_Parcialidad_Conv = Dt_Convenios.Rows[0]["Datos_Convenio"].ToString().Split(' ');
                        Fila["Descripcion"] = Fila["Descripcion"].ToString().Substring(0, Fila["Descripcion"].ToString().Length - 1);
                        Fila["Descripcion"] = Fila["Descripcion"].ToString() + " DE " + Ultima_Parcialidad_Conv[3];
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);

                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Convenio";
                    Fila["Nombre"] = Dt_Convenios.Rows[0]["Pago"].ToString();
                    Fila["Descripcion"] = "SALDO: " + Dt_Convenios.Rows[0]["Saldo"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }
                else
                {
                    DataTable Dt_Adeudos_Predial = Obtener_Dato_Consulta(Cuenta_Predial_Id, "");
                    if (Dt_Adeudos_Predial.Rows.Count == 0)
                    {
                        Fila = Dt_Datos_recibo.NewRow();
                        Fila["Tipo"] = "Convenio";
                        Fila["Nombre"] = " ";
                        Fila["Descripcion"] = "PAGO TOTAL";
                        Dt_Datos_recibo.Rows.Add(Fila);
                    }
                    else
                    {
                        Fila = Dt_Datos_recibo.NewRow();
                        Fila["Tipo"] = "Convenio";
                        Fila["Nombre"] = " ";
                        Fila["Descripcion"] = "PAGO PARCIAL";
                        Dt_Datos_recibo.Rows.Add(Fila);
                    }
                }
                //obtenemos la proteccion del recibo
                Proteccion = "<span class='Proteccion_Negrita'>" + Dt_Proteccion.Rows[0]["ESTATUS"].ToString() + "</span>";
                Proteccion += "/" + Dt_Proteccion.Rows[0]["CONFRONTO"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_CAJA"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_OPERACION"].ToString() + "/";
                Proteccion += String.Format("{0:yyyy.MM.dd}", Dt_Proteccion.Rows[0]["FECHA"]) + "/";
                Proteccion += String.Format("{0:HH:mm:ss}", Dt_Proteccion.Rows[0]["FECHA_CREO"]) + "/";
                Proteccion += string.Format("{0:c}", Dt_Proteccion.Rows[0]["TOTAL"]) + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_RECIBO"].ToString() + "/";
                if (Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString() == "")
                {
                    Proteccion += Dt_Proteccion.Rows[0]["DOCUMENTO"].ToString();
                }
                else
                {
                    Proteccion += Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString();
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Proteccion";
                Fila["Nombre"] = "proteccion";
                Fila["Descripcion"] = Proteccion;
                Dt_Datos_recibo.Rows.Add(Fila);

                Dt_Datos_recibo = Validar_Importes_Vacios(Dt_Datos_recibo.Copy());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el datatable de los datos del recibo. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Crear_Dt_Datos_Recibo_Constancia
        //DESCRIPCIÓN          : Metodo para juntar los datos del recibo en un solo datatable
        //PARAMETROS           1 Dt_Encabezado: datatable que contiene los datos del encabezado del recibo
        //                     2 Dt_Detalles: datatable que contiene los datos del detalle del pago del recibo
        //                     3 Dt_Proteccion: datatable que contiene los datos de la proteccion del recibo
        //                     4 Folio es la referencia
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 29/octubre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Crear_Dt_Datos_Recibo_Constancia(DataTable Dt_Encabezado, DataTable Dt_Detalles, DataTable Dt_Proteccion, String Folio)
        {
            DataTable Dt_Datos_recibo = new DataTable();
            DataRow Fila;
            Double Subtotal = 0.00;
            Double Ajuste_Tarifario = 0.00;
            String Proteccion = String.Empty;
            Boolean Ajuste = false;

            // creamos las columnas del datatable
            Dt_Datos_recibo.Columns.Add("Tipo"); //tipo de elemento del recino (encabezado, detalle, pie de pagina, proteccion)
            Dt_Datos_recibo.Columns.Add("Nombre");//nombre del campo
            Dt_Datos_recibo.Columns.Add("Descripcion");//descripcion del campo
            try
            {
                //obtenemos los datos del encabezado
                if (!String.IsNullOrEmpty(Dt_Proteccion.Rows[0]["FECHA"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "FECHA:";
                    Fila["Descripcion"] = Crear_Fecha(String.Format("{0:dd/MM/yyyy}", Dt_Proteccion.Rows[0]["FECHA"]));
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Folio))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "FOLIO:";
                    Fila["Descripcion"] = Folio;
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["CUENTA_PREDIAL"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CUENTA:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["CUENTA_PREDIAL"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["COLONIA"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "COLONIA:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["COLONIA"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["UBICACION"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "UBICACION:";
                    if (Dt_Encabezado.Rows[0]["UBICACION"].ToString() != "")
                    {
                        Fila["Descripcion"] = Dt_Encabezado.Rows[0]["UBICACION"].ToString();
                    }
                    else
                    {
                        Fila["Descripcion"] = Dt_Encabezado.Rows[0]["DOMICILIO"].ToString();
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);
                }
                else
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "UBICACION:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["DOMICILIO"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["NO_INTERIOR"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "INTERIOR:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["NO_INTERIOR"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["NO_EXTERIOR"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "EXTERIOR:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["NO_EXTERIOR"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["CONTRIBUYENTE"].ToString().Trim()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CONTRIBUYENTE:";
                    if (Dt_Encabezado.Rows[0]["CONTRIBUYENTE"].ToString().Trim() != "")
                    {
                        Fila["Descripcion"] = Dt_Encabezado.Rows[0]["CONTRIBUYENTE"].ToString();
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["SOLICITANTE"].ToString().Trim()))
                        {
                            Fila["Descripcion"] = Dt_Encabezado.Rows[0]["SOLICITANTE"].ToString();
                        }
                        else
                        {
                            Fila["Descripcion"] = " ";
                        }
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);
                }
                else
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CONTRIBUYENTE:";
                    if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["SOLICITANTE"].ToString().Trim()))
                    {
                        Fila["Descripcion"] = Dt_Encabezado.Rows[0]["SOLICITANTE"].ToString();
                    }
                    else
                    {
                        Fila["Descripcion"] = " ";
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["RFC"].ToString().Trim()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "R.F.C.:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["RFC"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }
                else
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "R.F.C.:";
                    if (!string.IsNullOrEmpty(Dt_Encabezado.Rows[0]["SOLICITANTE_RFC"].ToString().Trim()))
                    {
                        Fila["Descripcion"] = Dt_Encabezado.Rows[0]["SOLICITANTE_RFC"].ToString().Trim();
                    }
                    else
                    {
                        Fila["Descripcion"] = " ";
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);
                }


                //Obtenemos los detalles del pago
                foreach (DataRow Dr in Dt_Detalles.Rows)
                {
                    if (Dr["DESCRIPCION"].ToString() != "AJUSTE TARIFARIO")
                    {
                        Fila = Dt_Datos_recibo.NewRow();
                        Fila["Tipo"] = "Detalle";
                        if (Dr["FUNDAMENTO"].ToString() != "")
                        {
                            Fila["Nombre"] = Dr["DESCRIPCION"].ToString().ToUpper() + "(" + Dr["FUNDAMENTO"].ToString().ToUpper() + ")";
                        }
                        else
                        {
                            Fila["Nombre"] = Dr["DESCRIPCION"].ToString().ToUpper();
                        }
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Dr["MONTO"]);
                        Dt_Datos_recibo.Rows.Add(Fila);

                        Subtotal = Subtotal + Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                    }
                    else
                    {
                        Ajuste_Tarifario = Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                        Ajuste = true;
                    }
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "SUBTOTAL";
                Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Subtotal);
                Dt_Datos_recibo.Rows.Add(Fila);

                if (Ajuste)
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    if (Ajuste_Tarifario > 0)
                    {
                        Fila["Descripcion"] = string.Format("{0:+ #,###,##0.00}", Ajuste_Tarifario);
                    }
                    else
                    {
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);
                }
                else
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "<div class='Leyenta_Total'>TOTAL</div>";
                Fila["Descripcion"] = string.Format("<div class='Monto_Total'>{0:#,###,##0.00}</div>", Dt_Proteccion.Rows[0]["TOTAL"]);
                Dt_Datos_recibo.Rows.Add(Fila);

                //obtenemos la proteccion del recibo
                Proteccion = "<b>" + Dt_Proteccion.Rows[0]["ESTATUS"].ToString() + "</b>";
                Proteccion += "/" + Dt_Proteccion.Rows[0]["CONFRONTO"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_CAJA"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_OPERACION"].ToString() + "/";
                Proteccion += String.Format("{0:yyyy.MM.dd}", Dt_Proteccion.Rows[0]["FECHA"]) + "/";
                Proteccion += String.Format("{0:HH:mm:ss}", Dt_Proteccion.Rows[0]["FECHA_CREO"]) + "/";
                Proteccion += string.Format("{0:c}", Dt_Proteccion.Rows[0]["TOTAL"]) + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_RECIBO"].ToString() + "/";
                if (Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString() == "")
                {
                    Proteccion += Dt_Proteccion.Rows[0]["DOCUMENTO"].ToString();
                }
                else
                {
                    Proteccion += Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString();
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Proteccion";
                Fila["Nombre"] = "proteccion";
                Fila["Descripcion"] = Proteccion;
                Dt_Datos_recibo.Rows.Add(Fila);

                Dt_Datos_recibo = Validar_Importes_Vacios(Dt_Datos_recibo.Copy());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el datatable de los datos del recibo de constancias. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Crear_Dt_Datos_Recibo_Impuestos
        //DESCRIPCIÓN          : Metodo para juntar los datos del recibo en un solo datatable
        //PARAMETROS           1 Dt_Encabezado: datatable que contiene los datos del encabezado del recibo
        //                     2 Dt_Detalles: datatable que contiene los datos del detalle del pago del recibo
        //                     3 Dt_Proteccion: datatable que contiene los datos de la proteccion del recibo
        //                     4 Dt_Convenios: datatable que contiene los datos del convenio
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 04/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Crear_Dt_Datos_Recibo_Impuestos(DataTable Dt_Encabezado, DataTable Dt_Detalles, DataTable Dt_Proteccion, DataTable Dt_Convenios)
        {
            DataTable Dt_Datos_recibo = new DataTable();
            DataRow Fila;
            Double Subtotal = 0.00;
            Double Ajuste_Tarifario = 0.00;
            String Proteccion = String.Empty;
            Boolean Ajuste = false;

            // creamos las columnas del datatable
            Dt_Datos_recibo.Columns.Add("Tipo"); //tipo de elemento del recino (encabezado, detalle, pie de pagina, proteccion)
            Dt_Datos_recibo.Columns.Add("Nombre");//nombre del campo
            Dt_Datos_recibo.Columns.Add("Descripcion");//descripcion del campo
            try
            {
                //obtenemos los datos del encabezado
                if (!String.IsNullOrEmpty(Dt_Proteccion.Rows[0]["FECHA"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "FECHA:";
                    Fila["Descripcion"] = Crear_Fecha(String.Format("{0:dd/MM/yyyy}", Dt_Proteccion.Rows[0]["FECHA"]));
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["CUENTA_PREDIAL"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CUENTA:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["CUENTA_PREDIAL"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["CONTRIBUYENTE"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "CONTRIBUYENTE:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["CONTRIBUYENTE"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Encabezado.Rows[0]["RFC"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "R.F.C.:";
                    Fila["Descripcion"] = Dt_Encabezado.Rows[0]["RFC"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                //Obtenemos los detalles del pago
                foreach (DataRow Dr in Dt_Detalles.Rows)
                {
                    if (Dr["DESCRIPCION"].ToString() != "AJUSTE TARIFARIO")
                    {
                        Fila = Dt_Datos_recibo.NewRow();
                        Fila["Tipo"] = "Detalle";
                        if (Dr["FUNDAMENTO"].ToString() != "")
                        {
                            Fila["Nombre"] = Dr["DESCRIPCION"].ToString().ToUpper() + "(" + Dr["FUNDAMENTO"].ToString().ToUpper() + ")";
                        }
                        else
                        {
                            Fila["Nombre"] = Dr["DESCRIPCION"].ToString().ToUpper();
                        }
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Dr["MONTO"]);
                        Dt_Datos_recibo.Rows.Add(Fila);

                        Subtotal = Subtotal + Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                    }
                    else
                    {
                        Ajuste_Tarifario = Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                        Ajuste = true;
                    }
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "SUBTOTAL";
                Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Subtotal);
                Dt_Datos_recibo.Rows.Add(Fila);

                if (Ajuste)
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    if (Ajuste_Tarifario > 0)
                    {
                        Fila["Descripcion"] = string.Format("{0:+ #,###,##0.00}", Ajuste_Tarifario);
                    }
                    else
                    {
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);
                }
                else
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "<div class='Leyenta_Total'>TOTAL</div>";
                Fila["Descripcion"] = string.Format("<div class='Monto_Total'>{0:#,###,##0.00}</div>", Dt_Proteccion.Rows[0]["TOTAL"]);

                Dt_Datos_recibo.Rows.Add(Fila);

                //OBTENEMOS LOS DATOS DEL COMVENIO SI ES QUE EXISTE
                if (Dt_Convenios.Rows.Count > 0)
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Convenio";
                    Fila["Nombre"] = "DATOS DEL CONVENIO:";
                    Fila["Descripcion"] = Dt_Convenios.Rows[0]["Datos_Convenio"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);

                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Convenio";
                    Fila["Nombre"] = Dt_Convenios.Rows[0]["Pago"].ToString();
                    Fila["Descripcion"] = "SALDO: " + Dt_Convenios.Rows[0]["Saldo"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                //obtenemos la proteccion del recibo
                Proteccion = "<b>" + Dt_Proteccion.Rows[0]["ESTATUS"].ToString() + "</b>";
                Proteccion += "/" + Dt_Proteccion.Rows[0]["CONFRONTO"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_CAJA"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_OPERACION"].ToString() + "/";
                Proteccion += String.Format("{0:yyyy.MM.dd}", Dt_Proteccion.Rows[0]["FECHA"]) + "/";
                Proteccion += String.Format("{0:HH:mm:ss}", Dt_Proteccion.Rows[0]["FECHA_CREO"]) + "/";
                Proteccion += string.Format("{0:c}", Dt_Proteccion.Rows[0]["TOTAL"]) + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_RECIBO"].ToString() + "/";
                if (Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString() == "")
                {
                    Proteccion += Dt_Proteccion.Rows[0]["DOCUMENTO"].ToString();
                }
                else
                {
                    Proteccion += Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString();
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Proteccion";
                Fila["Nombre"] = "proteccion";
                Fila["Descripcion"] = Proteccion;
                Dt_Datos_recibo.Rows.Add(Fila);

                Dt_Datos_recibo = Validar_Importes_Vacios(Dt_Datos_recibo.Copy());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el datatable de los datos del recibo de impuestos. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Crear_Dt_Datos_Recibo_Otros_Pagos
        //DESCRIPCIÓN          : Metodo para juntar los datos del recibo en un solo datatable
        //PARAMETROS           1 Dt_Detalles: datatable que contiene los datos del detalle del pago del recibo
        //                     2 Dt_Proteccion: datatable que contiene los datos de la proteccion del recibo
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 04/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Crear_Dt_Datos_Recibo_Otros_Pagos(DataTable Dt_Detalles, DataTable Dt_Proteccion)
        {
            DataTable Dt_Datos_recibo = new DataTable();
            DataRow Fila;
            Double Subtotal = 0.00;
            Double Ajuste_Tarifario = 0.00;
            String Proteccion = String.Empty;
            Boolean Ajuste = false;
            Decimal Total = 0;

            // creamos las columnas del datatable
            Dt_Datos_recibo.Columns.Add("Tipo"); //tipo de elemento del recino (encabezado, detalle, pie de pagina, proteccion)
            Dt_Datos_recibo.Columns.Add("Nombre");//nombre del campo
            Dt_Datos_recibo.Columns.Add("Descripcion");//descripcion del campo
            try
            {
                //obtenemos los datos del encabezado
                if (!String.IsNullOrEmpty(Dt_Proteccion.Rows[0]["FECHA"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "FECHA:";
                    Fila["Descripcion"] = Crear_Fecha(String.Format("{0:dd/MM/yyyy}", Dt_Proteccion.Rows[0]["FECHA"]));
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Detalles.Rows[0]["CONTRIBUYENTE"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "NOMBRE:";
                    Fila["Descripcion"] = Dt_Detalles.Rows[0]["CONTRIBUYENTE"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                //Obtenemos los detalles del pago
                foreach (DataRow Dr in Dt_Detalles.Rows)
                {
                    if (Dr["DESCRIPCION"].ToString() != "AJUSTE TARIFARIO")
                    {
                        Fila = Dt_Datos_recibo.NewRow();
                        Fila["Tipo"] = "Detalle";
                        Fila["Nombre"] = "CANTIDAD";
                        Fila["Descripcion"] = Dr["CANTIDAD"].ToString();
                        Dt_Datos_recibo.Rows.Add(Fila);

                        Fila = Dt_Datos_recibo.NewRow();
                        Fila["Tipo"] = "Detalle";
                        if (Dr["FUNDAMENTO"].ToString() != "")
                        {
                            Fila["Nombre"] = Dr["DESCRIPCION"].ToString().ToUpper() + "(" + Dr["FUNDAMENTO"].ToString().ToUpper() + ")";
                        }
                        else
                        {
                            Fila["Nombre"] = Dr["DESCRIPCION"].ToString().ToUpper();
                        }
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Dr["MONTO"]);
                        Dt_Datos_recibo.Rows.Add(Fila);

                        Subtotal = Subtotal + Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                    }
                    else
                    {
                        Ajuste_Tarifario = Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                        Ajuste = true;
                    }
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "SUBTOTAL";
                Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Subtotal);
                Dt_Datos_recibo.Rows.Add(Fila);

                if (Ajuste)
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    if (Ajuste_Tarifario > 0)
                    {
                        Fila["Descripcion"] = string.Format("{0:+ #,###,##0.00}", Ajuste_Tarifario);
                    }
                    else
                    {
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);
                }
                else
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                Total = Convert.ToDecimal(Subtotal + Ajuste_Tarifario);

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "<div class='Leyenta_Total'>TOTAL</div>";
                Fila["Descripcion"] = string.Format("<div class='Monto_Total'>{0:#,###,##0.00}</div>", Total);
                Dt_Datos_recibo.Rows.Add(Fila);

                if (!string.IsNullOrEmpty(Dt_Detalles.Rows[0]["OBSERVACIONES"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Observaciones";
                    Fila["Nombre"] = "OBSERVACIONES:";
                    Fila["Descripcion"] = Dt_Detalles.Rows[0]["OBSERVACIONES"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                //obtenemos la proteccion del recibo
                Proteccion = "<span class='Proteccion_Negrita'>" + Dt_Proteccion.Rows[0]["ESTATUS"].ToString() + "</span>";
                Proteccion += "/" + Dt_Proteccion.Rows[0]["CONFRONTO"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_CAJA"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_OPERACION"].ToString() + "/";
                Proteccion += String.Format("{0:yyyy.MM.dd}", Dt_Proteccion.Rows[0]["FECHA"]) + "/";
                Proteccion += String.Format("{0:HH:mm:ss}", Dt_Proteccion.Rows[0]["FECHA_CREO"]) + "/";
                Proteccion += string.Format("{0:c}", Total) + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_RECIBO"].ToString() + "/";
                if (Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString() == "")
                {
                    Proteccion += Dt_Proteccion.Rows[0]["DOCUMENTO"].ToString();
                }
                else
                {
                    Proteccion += Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString();
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Proteccion";
                Fila["Nombre"] = "proteccion";
                Fila["Descripcion"] = Proteccion;
                Dt_Datos_recibo.Rows.Add(Fila);

                Dt_Datos_recibo = Validar_Importes_Vacios(Dt_Datos_recibo.Copy());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el datatable de los datos del recibo de otros pagos. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Crear_Dt_Datos_Recibo_Ingresos
        //DESCRIPCIÓN          : Metodo para juntar los datos del recibo en un solo datatable
        //PARAMETROS           1 Dt_Detalles: datatable que contiene los datos del detalle del pago del recibo
        //                     2 Dt_Proteccion: datatable que contiene los datos de la proteccion del recibo
        //CREO                 : Antonio Salvador Benavides Guardado
        //FECHA_CREO           : 17/Junio/2012
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Crear_Dt_Datos_Recibo_Ingresos(DataTable Dt_Detalles, DataTable Dt_Proteccion, String Referencia)
        {
            Cls_Ope_Ing_Descuentos_Negocio Descuentos = new Cls_Ope_Ing_Descuentos_Negocio();
            DataTable Dt_Datos_recibo = new DataTable();
            DataRow Fila;
            Double Subtotal = 0.00;
            Double Ajuste_Tarifario = 0.00;
            String Proteccion = String.Empty;
            Boolean Ajuste = false;
            String Descripcion_Descuento = "";
            String Porcentaje_Descuento = "";
            String Usuario_Autorizo_Descuento = "";
            String Insertar_Salto_Linea = "";

            // creamos las columnas del datatable
            Dt_Datos_recibo.Columns.Add("Tipo"); //tipo de elemento del recino (encabezado, detalle, pie de pagina, proteccion)
            Dt_Datos_recibo.Columns.Add("Nombre");//nombre del campo
            Dt_Datos_recibo.Columns.Add("Descripcion");//descripcion del campo
            try
            {
                //obtenemos los datos del encabezado
                if (!String.IsNullOrEmpty(Dt_Proteccion.Rows[0]["FECHA"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "FECHA:";
                    Fila["Descripcion"] = Crear_Fecha(String.Format("{0:dd/MM/yyyy}", Dt_Proteccion.Rows[0]["FECHA"]));
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Detalles.Rows[0]["CONTRIBUYENTE"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "NOMBRE:";
                    Fila["Descripcion"] = Dt_Detalles.Rows[0]["CONTRIBUYENTE"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                DataTable Dt_Decuentos;
                Descuentos.P_Campos_Dinamicos = "";
                Descuentos.P_Campos_Dinamicos += Ope_Ing_Descuentos.Campo_Descuento_Honorarios;
                Descuentos.P_Campos_Dinamicos += ", " + Ope_Ing_Descuentos.Campo_Descuento_Multas;
                Descuentos.P_Campos_Dinamicos += ", " + Ope_Ing_Descuentos.Campo_Descuento_Moratorios;
                Descuentos.P_Campos_Dinamicos += ", " + Ope_Ing_Descuentos.Campo_Descuento_Recargos;
                Descuentos.P_Campos_Dinamicos += ", " + Ope_Ing_Descuentos.Campo_Usuario_Creo;
                Descuentos.P_Referencia = Referencia;
                Dt_Decuentos = Descuentos.Consultar_Descuentos();

                //Obtenemos los detalles del pago
                foreach (DataRow Dr in Dt_Detalles.Rows)
                {
                    Insertar_Salto_Linea = "";
                    if (Dr["DESCRIPCION"].ToString() != "AJUSTE TARIFARIO")
                    {
                        if (Dr["DESCRIPCION"].ToString().Trim() != "")
                        {
                            Fila = Dt_Datos_recibo.NewRow();
                            Fila["Tipo"] = "Detalle";
                            if (Dr["PERIODO"].ToString().Trim().Contains("DESCUENTO"))
                            {
                                Descripcion_Descuento = Dr["PERIODO"].ToString().Trim().ToUpper();
                                if (Descripcion_Descuento.Contains("INGRESO"))
                                {
                                    Descripcion_Descuento = "(DESCUENTO POR PRONTO PAGO)";
                                }
                                if (Dt_Decuentos != null)
                                {
                                    if (Dt_Decuentos.Rows.Count > 0)
                                    {
                                        if (Descripcion_Descuento.Contains("HONORARIO"))
                                        {
                                            Porcentaje_Descuento = Dt_Decuentos.Rows[0][Ope_Ing_Descuentos.Campo_Descuento_Honorarios].ToString();
                                        }
                                        if (Descripcion_Descuento.Contains("MULTA"))
                                        {
                                            Porcentaje_Descuento = Dt_Decuentos.Rows[0][Ope_Ing_Descuentos.Campo_Descuento_Multas].ToString();
                                        }
                                        if (Descripcion_Descuento.Contains("MORATORIO"))
                                        {
                                            Porcentaje_Descuento = Dt_Decuentos.Rows[0][Ope_Ing_Descuentos.Campo_Descuento_Moratorios].ToString();
                                        }
                                        if (Descripcion_Descuento.Contains("RECARGO"))
                                        {
                                            Porcentaje_Descuento = Dt_Decuentos.Rows[0][Ope_Ing_Descuentos.Campo_Descuento_Recargos].ToString();
                                        }
                                        Usuario_Autorizo_Descuento = Dt_Decuentos.Rows[0][Ope_Ing_Descuentos.Campo_Usuario_Creo].ToString();
                                        Insertar_Salto_Linea = "<BR/>";
                                        Descripcion_Descuento = "(" + Porcentaje_Descuento + "% " + Descripcion_Descuento + " AUTORIZADO POR " + Usuario_Autorizo_Descuento + ")";
                                    }
                                }
                            }
                            if (Dr["FUNDAMENTO"].ToString() != "")
                            {
                                Fila["Nombre"] = Dr["DESCRIPCION"].ToString().ToUpper() + "(" + Dr["FUNDAMENTO"].ToString().ToUpper() + ")";
                            }
                            else
                            {
                                Fila["Nombre"] = Dr["DESCRIPCION"].ToString().ToUpper() + Descripcion_Descuento + Insertar_Salto_Linea;
                            }
                            Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Dr["MONTO"]) + Insertar_Salto_Linea;
                            Dt_Datos_recibo.Rows.Add(Fila);

                            Subtotal = Subtotal + Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                        }
                    }
                    else
                    {
                        Ajuste_Tarifario = Convert.ToDouble(string.IsNullOrEmpty(Dr["MONTO"].ToString()) ? "0" : Dr["MONTO"].ToString());
                        Ajuste = true;
                    }
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "SUBTOTAL";
                Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Subtotal);
                Dt_Datos_recibo.Rows.Add(Fila);

                if (Ajuste)
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    if (Ajuste_Tarifario > 0)
                    {
                        Fila["Descripcion"] = string.Format("{0:+ #,###,##0.00}", Ajuste_Tarifario);
                    }
                    else
                    {
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);
                }
                else
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "<div class='Leyenta_Total'>TOTAL</div>";
                Fila["Descripcion"] = string.Format("<div class='Monto_Total'>{0:#,###,##0.00}</div>", Dt_Proteccion.Rows[0]["TOTAL"]);
                Dt_Datos_recibo.Rows.Add(Fila);

                if (!string.IsNullOrEmpty(Dt_Detalles.Rows[0]["OBSERVACIONES"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Observaciones";
                    Fila["Nombre"] = "OBSERVACIONES:";
                    Fila["Descripcion"] = Dt_Detalles.Rows[0]["OBSERVACIONES"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                //obtenemos la proteccion del recibo
                Proteccion = "<span class='Proteccion_Negrita'>" + Dt_Proteccion.Rows[0]["ESTATUS"].ToString() + "</span>";
                Proteccion += "/" + Dt_Proteccion.Rows[0]["CONFRONTO"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_CAJA"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_OPERACION"].ToString() + "/";
                Proteccion += String.Format("{0:yyyy.MM.dd}", Dt_Proteccion.Rows[0]["FECHA"]) + "/";
                Proteccion += String.Format("{0:HH:mm:ss}", Dt_Proteccion.Rows[0]["FECHA_CREO"]) + "/";
                Proteccion += string.Format("{0:c}", Dt_Proteccion.Rows[0]["TOTAL"]) + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_RECIBO"].ToString() + "/";
                if (Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString() == "")
                {
                    Proteccion += Dt_Proteccion.Rows[0]["DOCUMENTO"].ToString();
                }
                else
                {
                    Proteccion += Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString();
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Proteccion";
                Fila["Nombre"] = "proteccion";
                Fila["Descripcion"] = Proteccion;
                Dt_Datos_recibo.Rows.Add(Fila);

                Dt_Datos_recibo = Validar_Importes_Vacios(Dt_Datos_recibo.Copy());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el datatable de los datos del recibo de otros pagos. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Crear_Dt_Datos_Recibo_Tramites
        //DESCRIPCIÓN          : Metodo para juntar los datos del recibo en un solo datatable
        //PARAMETROS           1 Dt_Detalles: datatable que contiene los datos del detalle del pago del recibo
        //                     2 Dt_Proteccion: datatable que contiene los datos de la proteccion del recibo
        //CREO                 : Antonio Salvador Benavides Guardado
        //FECHA_CREO           : 08/Agosto/2012
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Crear_Dt_Datos_Recibo_Tramites(DataTable Dt_Detalles, DataTable Dt_Proteccion, String Referencia)
        {
            DataTable Dt_Datos_recibo = new DataTable();
            DataRow Fila;
            Double Subtotal = 0.00;
            Double Ajuste_Tarifario = 0.00;
            String Proteccion = String.Empty;
            Boolean Ajuste = false;
            String Descripcion_Descuento = "";
            String Porcentaje_Descuento = "";
            String Usuario_Autorizo_Descuento = "";
            String Insertar_Salto_Linea = "";

            // creamos las columnas del datatable
            Dt_Datos_recibo.Columns.Add("Tipo"); //tipo de elemento del recino (encabezado, detalle, pie de pagina, proteccion)
            Dt_Datos_recibo.Columns.Add("Nombre");//nombre del campo
            Dt_Datos_recibo.Columns.Add("Descripcion");//descripcion del campo
            try
            {
                //obtenemos los datos del encabezado
                if (!String.IsNullOrEmpty(Dt_Proteccion.Rows[0]["FECHA"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "FECHA:";
                    Fila["Descripcion"] = Crear_Fecha(String.Format("{0:dd/MM/yyyy}", Dt_Proteccion.Rows[0]["FECHA"]));
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                if (!String.IsNullOrEmpty(Dt_Detalles.Rows[0]["CONTRIBUYENTE"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Encabezado";
                    Fila["Nombre"] = "NOMBRE:";
                    Fila["Descripcion"] = Dt_Detalles.Rows[0]["CONTRIBUYENTE"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "SUBTOTAL";
                Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Subtotal);
                Dt_Datos_recibo.Rows.Add(Fila);

                if (Ajuste)
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    if (Ajuste_Tarifario > 0)
                    {
                        Fila["Descripcion"] = string.Format("{0:+ #,###,##0.00}", Ajuste_Tarifario);
                    }
                    else
                    {
                        Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    }
                    Dt_Datos_recibo.Rows.Add(Fila);
                }
                else
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Detalle_Pago";
                    Fila["Nombre"] = "AJUSTE TARIFARIO";
                    Fila["Descripcion"] = string.Format("{0:#,###,##0.00}", Ajuste_Tarifario);
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Detalle_Pago";
                Fila["Nombre"] = "<div class='Leyenta_Total'>TOTAL</div>";
                Fila["Descripcion"] = string.Format("<div class='Monto_Total'>{0:#,###,##0.00}</div>", Dt_Proteccion.Rows[0]["TOTAL"]);
                Dt_Datos_recibo.Rows.Add(Fila);

                if (!string.IsNullOrEmpty(Dt_Detalles.Rows[0]["OBSERVACIONES"].ToString()))
                {
                    Fila = Dt_Datos_recibo.NewRow();
                    Fila["Tipo"] = "Observaciones";
                    Fila["Nombre"] = "OBSERVACIONES:";
                    Fila["Descripcion"] = Dt_Detalles.Rows[0]["OBSERVACIONES"].ToString();
                    Dt_Datos_recibo.Rows.Add(Fila);
                }

                //obtenemos la proteccion del recibo
                Proteccion = "<span class='Proteccion_Negrita'>" + Dt_Proteccion.Rows[0]["ESTATUS"].ToString() + "</span>";
                Proteccion += "/" + Dt_Proteccion.Rows[0]["CONFRONTO"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_CAJA"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_OPERACION"].ToString() + "/";
                Proteccion += String.Format("{0:yyyy.MM.dd}", Dt_Proteccion.Rows[0]["FECHA"]) + "/";
                Proteccion += String.Format("{0:HH:mm:ss}", Dt_Proteccion.Rows[0]["FECHA_CREO"]) + "/";
                Proteccion += string.Format("{0:c}", Dt_Proteccion.Rows[0]["TOTAL"]) + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_RECIBO"].ToString() + "/";
                if (Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString() == "")
                {
                    Proteccion += Dt_Proteccion.Rows[0]["DOCUMENTO"].ToString();
                }
                else
                {
                    Proteccion += Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString();
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Proteccion";
                Fila["Nombre"] = "proteccion";
                Fila["Descripcion"] = Proteccion;
                Dt_Datos_recibo.Rows.Add(Fila);

                Dt_Datos_recibo = Validar_Importes_Vacios(Dt_Datos_recibo.Copy());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el datatable de los datos del recibo de otros pagos. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_recibo;
        }

        //****************************************************************************************************
        //NOMBRE DE LA FUNCIÓN : Crear_Dt_Datos_Recibo_Cancelacion
        //DESCRIPCIÓN          : Metodo para juntar los datos del recibo en un solo datatable
        //PARAMETROS           1  Dt_Proteccion: datatable que contiene los datos de la proteccion del recibo
        //CREO                 : Leslie González Vázquez
        //FECHA_CREO           : 04/Noviembre/2011 
        //MODIFICO             :
        //FECHA_MODIFICO       :
        //CAUSA_MODIFICACIÓN   :
        //****************************************************************************************************
        internal static System.Data.DataTable Crear_Dt_Datos_Recibo_Cancelacion(DataTable Dt_Proteccion)
        {
            DataTable Dt_Datos_recibo = new DataTable();
            DataRow Fila;
            String Proteccion = String.Empty;

            // creamos las columnas del datatable
            Dt_Datos_recibo.Columns.Add("Tipo"); //tipo de elemento del recino (encabezado, detalle, pie de pagina, proteccion)
            Dt_Datos_recibo.Columns.Add("Nombre");//nombre del campo
            Dt_Datos_recibo.Columns.Add("Descripcion");//descripcion del campo

            try
            {
                //obtenemos la proteccion del recibo
                Proteccion = "<span class='Proteccion_Negrita'>CANCELADO</span>";
                Proteccion += "/" + Dt_Proteccion.Rows[0]["CONFRONTO"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_CAJA"].ToString() + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_OPERACION"].ToString() + "/";
                Proteccion += String.Format("{0:yyyy.MM.dd}", Dt_Proteccion.Rows[0]["FECHA_CANCELACION"]) + "/";
                Proteccion += String.Format("{0:HH:mm:ss}", Dt_Proteccion.Rows[0]["FECHA_CREO"]) + "/";
                Proteccion += string.Format("{0:c}", Dt_Proteccion.Rows[0]["TOTAL"]) + "/";
                Proteccion += Dt_Proteccion.Rows[0]["NO_RECIBO"].ToString() + "/";
                if (Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString() == "")
                {
                    Proteccion += Dt_Proteccion.Rows[0]["DOCUMENTO"].ToString();
                }
                else
                {
                    Proteccion += Dt_Proteccion.Rows[0]["CUENTA_PREDIAL"].ToString();
                }

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Cancelacion";
                Fila["Nombre"] = "Cancela";
                Fila["Descripcion"] = Proteccion;
                Dt_Datos_recibo.Rows.Add(Fila);

                Fila = Dt_Datos_recibo.NewRow();
                Fila["Tipo"] = "Cancelacion";
                Fila["Nombre"] = "Motivo";
                Fila["Descripcion"] = "<span class='Motivo_Cancelacion'>" + Dt_Proteccion.Rows[0]["MOTIVO_CANCELACION"].ToString() + "</span>";
                Dt_Datos_recibo.Rows.Add(Fila);

                Dt_Datos_recibo = Validar_Importes_Vacios(Dt_Datos_recibo.Copy());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el datatable de los datos del recibo de cancelación. Error: [" + Ex.Message + "]");
            }
            return Dt_Datos_recibo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private String Obtener_Dato_Consulta(String Consulta, String Tabla, String Condiciones)
        {
            String Mi_SQL;
            String Dato_Consulta = "";

            try
            {
                Mi_SQL = Consulta;

                OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Dr_Dato.Read())
                {
                    if (Dr_Dato[0] != null)
                    {
                        Dato_Consulta = Dr_Dato[0].ToString();
                    }
                    else
                    {
                        Dato_Consulta = "";
                    }
                    Dr_Dato.Close();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato = null;
            }
            catch
            {
            }
            finally
            {
            }

            return Dato_Consulta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Importes_Vacios
        ///DESCRIPCIÓN          : Recorre el DataTable indicado para buscar campos vacios en la columna de Importe para formatearla para su ajuste en la Impresión del Recibo
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 01/Abril/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        internal static DataTable Validar_Importes_Vacios(DataTable Dt_Datos_Recibo)
        {
            if (Dt_Datos_Recibo != null)
            {
                if (Dt_Datos_Recibo.Rows.Count > 0)
                {
                    if (Dt_Datos_Recibo.Columns.Contains("Descripcion"))
                    {
                        Boolean Formatear;
                        foreach (DataRow Dr_Datos_Recibo in Dt_Datos_Recibo.Rows)
                        {
                            Formatear = false;
                            if (Dr_Datos_Recibo["Descripcion"] == null)
                            {
                                Formatear = true;
                            }
                            else
                            {
                                if (Dr_Datos_Recibo["Descripcion"].ToString().Trim() == "")
                                {
                                    Formatear = true;
                                }
                            }
                            if (Formatear)
                            {
                                Int16 Longitud_Celda = 38;
                                if (Dr_Datos_Recibo["Nombre"].ToString().Length <= Longitud_Celda)
                                {
                                    Dr_Datos_Recibo["Descripcion"] = "<BR/>";
                                }
                                else
                                {
                                    for (Int16 Cont_Espacios = 0; Cont_Espacios < Dr_Datos_Recibo["Nombre"].ToString().Length / Longitud_Celda; Cont_Espacios++)
                                    {
                                        Dr_Datos_Recibo["Descripcion"] += "<BR/>";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Dt_Datos_Recibo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
        ///DESCRIPCIÓN          : Consulta los montos de un convenio o reestructura según sea el caso
        ///PARAMETROS:     
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 21/Diciembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static DataTable Obtener_Dato_Consulta(String cuenta_predial, String No_Pago)
        {
            String Mi_SQL;
            DataTable Dt_Montos = new DataTable();

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0) as PAGO_1, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0) as PAGO_2, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0) as PAGO_3, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0) as PAGO_4, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0) as PAGO_5, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0) as PAGO_6 ";
                Mi_SQL += "FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + "='" + cuenta_predial + "' AND (";
                Mi_SQL += "(NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0)) + ";
                Mi_SQL += "(NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0)) + ";
                Mi_SQL += "(NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0)) + ";
                Mi_SQL += "(NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0)) + ";
                Mi_SQL += "(NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0)) + ";
                Mi_SQL += "(NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0)))>0 ";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Dt_Montos = dataset.Tables[0];
                }
            }
            catch
            {
            }
            finally
            {
            }

            return Dt_Montos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta_Valor
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Obtener_Dato_Consulta_Valor(String cuenta_predial_id)
        {
            String Mi_SQL;
            String Dato_Consulta = "";

            try
            {
                //select cuenta.valor_fiscal*(select tasa.tasa_anual from cat_pre_tasas_predial_anual tasa where tasa.tasa_PREDIAL_id=cuenta.tasa_PREDIAL_id 
                //and tasa.ANIO=2012)/1000||' - '||(SELECT MINIMA.CUOTA FROM CAT_PRE_CUOTAS_MINIMAS MINIMA WHERE MINIMA.CUOTA_MINIMA_ID=CUENTA.CUOTA_MINIMA_ID) 
                //from cat_pre_cuentas_predial cuenta where cuenta.cuenta_predial_id='0000031126';
                Mi_SQL = "SELECT CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + " * (SELECT TASA." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " FROM " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " TASA WHERE  TASA." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + "=CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ")/1000||' - '||(SELECT MINIMA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " MINIMA WHERE MINIMA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + "=CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + ") FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTA WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + cuenta_predial_id + "'";

                OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Dr_Dato.Read())
                {
                    if (Dr_Dato[0] != null)
                    {
                        Dato_Consulta = Dr_Dato[0].ToString();
                    }
                    else
                    {
                        Dato_Consulta = "";
                    }
                    Dr_Dato.Close();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato = null;
            }
            catch
            {
            }
            finally
            {
            }

            return Dato_Consulta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta_Valor
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Obtener_Dato_Consulta_Cuota(String Anio)
        {
            String Mi_SQL;
            String Dato_Consulta = "";

            try
            {
                //select cuenta.valor_fiscal*(select tasa.tasa_anual from cat_pre_tasas_predial_anual tasa where tasa.tasa_PREDIAL_id=cuenta.tasa_PREDIAL_id 
                //and tasa.ANIO=2012)/1000||' - '||(SELECT MINIMA.CUOTA FROM CAT_PRE_CUOTAS_MINIMAS MINIMA WHERE MINIMA.CUOTA_MINIMA_ID=CUENTA.CUOTA_MINIMA_ID) 
                //from cat_pre_cuentas_predial cuenta where cuenta.cuenta_predial_id='0000031126';
                Mi_SQL = "SELECT " + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + "  WHERE " + Cat_Pre_Cuotas_Minimas.Campo_Año + "=" + Anio;

                OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Dr_Dato.Read())
                {
                    if (Dr_Dato[0] != null)
                    {
                        Dato_Consulta = Dr_Dato[0].ToString();
                    }
                    else
                    {
                        Dato_Consulta = "";
                    }
                    Dr_Dato.Close();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato = null;
            }
            catch
            {
            }
            finally
            {
            }

            return Dato_Consulta;
        }

        #endregion
    }
}