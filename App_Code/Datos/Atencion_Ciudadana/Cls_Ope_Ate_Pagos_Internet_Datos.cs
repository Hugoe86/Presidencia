using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using System.Net;
using System.IO;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Operacion_Atencion_Ciudadana_Pagos_Internet.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;
using Presidencia.Ope_Con_Poliza_Ingresos.Datos;
using Presidencia.Polizas.Negocios;

namespace Presidencia.Operacion_Atencion_Ciudadana_Pagos_Internet.Datos
{
    public class Cls_Ope_Ate_Pagos_Internet_Datos
    {
        public Cls_Ope_Ate_Pagos_Internet_Datos()
        {
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Pago_Internet
        /// DESCRIPCION : 
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Miguel Angel Bedolla Moreno
        /// FECHA_CREO  : 16/Diciembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static int Alta_Pago_Internet(Cls_Ope_Ate_Pagos_Internet_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Object Obj_No_Pago;      //Consecutivo del registro de la tabla en la base de datos
            Object No_Pasivo; //Consecutivo del registro de la tabla en la base de datos
            Object No_Operacion; //Obtiene el número de operacion que fue realizada durante en día de la caja
            Object Consecutivo;  //Obtiene el número de registro con el cual se va a dar de alta el detalle del pago en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;
            Cls_Ope_Ate_Pagos_Internet_Negocio Caja = new Cls_Ope_Ate_Pagos_Internet_Negocio();
            String Cuenta_Predial_ID = "";
            String Clave_Solicitud = "";
            DataTable Dt_Clave = new DataTable();
            int Filas_Afectadas = 0;

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction();  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL.Length = 0;
                //Consulta el último no de pago que fue registrado en la base de datos
                Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos.Campo_No_Pago + "),'0')");
                Mi_SQL.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Comando_SQL.CommandText = Mi_SQL.ToString();
                Obj_No_Pago = Comando_SQL.ExecuteOracleScalar().ToString();
                if (Convert.IsDBNull(Obj_No_Pago))
                {
                    Obj_No_Pago = "0000000001";
                }
                else
                {
                    Obj_No_Pago = String.Format("{0:0000000000}", Convert.ToInt32(Obj_No_Pago) + 1);
                }
                Datos.P_No_Pago = Obj_No_Pago.ToString();

                Mi_SQL.Length = 0;
                //Consulta el último no de operación que fue registrado en la base de datos
                Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos.Campo_No_Operacion + "),'0')");
                Mi_SQL.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Mi_SQL.Append(" WHERE " + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Datos.P_Caja_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Pago.ToString("dd-MM-yyyy") + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                Mi_SQL.Append(" AND TO_DATE ('" + Datos.P_Fecha_Pago.ToString("dd-MM-yyyy") + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                Comando_SQL.CommandText = Mi_SQL.ToString();
                No_Operacion = Comando_SQL.ExecuteOracleScalar().ToString();

                if (Convert.IsDBNull(No_Operacion))
                {
                    No_Operacion = Convert.ToInt32("1");
                }
                else
                {
                    No_Operacion = Convert.ToInt32(No_Operacion) + 1;
                }
                Mi_SQL.Length = 0;

                //Asigna la cuenta predial
                Cuenta_Predial_ID = Datos.P_Cuenta_Predial_ID;

                Mi_SQL.Length = 0;
                //Inserta los datos en la tabla con los datos proporcionados por el usuario
                Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Mi_SQL.Append("(" + Ope_Caj_Pagos.Campo_No_Pago + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_No_Recibo + ", " + Ope_Caj_Pagos.Campo_No_Operacion + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Caja_ID + ", " + Ope_Caj_Pagos.Campo_No_Turno + ", " + Ope_Caj_Pagos.Campo_Fecha + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Estatus + ", " + Ope_Caj_Pagos.Campo_Monto_Corriente + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Monto_Recargos + ", " + Ope_Caj_Pagos.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Tipo_Pago + ", " + Ope_Caj_Pagos.Campo_Total + ", " + Ope_Caj_Pagos.Campo_Ajuste_Tarifario + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Documento + ", " + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Empleado_ID + ", " + Ope_Caj_Pagos.Campo_Fecha_Creo + ")");
                Mi_SQL.Append(" VALUES ('" + Obj_No_Pago.ToString() + "', NULL, " + No_Operacion + ", '");
                Mi_SQL.Append(Datos.P_Caja_ID + "', NULL, TO_DATE('" + Datos.P_Fecha_Pago.ToString("dd-MM-yyyy") + "','DD-MM-YYYY'), 'PAGADO', ");
                Mi_SQL.Append(Datos.P_Monto_Corriente + ", " + Datos.P_Monto_Recargos + ", ");
                Mi_SQL.Append("NULL, ");
                Mi_SQL.Append("'" + Datos.P_Tipo_Pago + "', ");
                Mi_SQL.Append(Datos.P_Total_Pagar + ", ");
                Mi_SQL.Append(Datos.P_Ajuste_Tarifario + ", ");
                Mi_SQL.Append("'" + Datos.P_Referencia + "', ");
                Mi_SQL.Append("'" + Cuenta_Predial_ID + "', ");
                Mi_SQL.Append("NULL, ");
                Mi_SQL.Append("SYSDATE)");
                Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                Filas_Afectadas += Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos            

                // guardar dato de forma temporal
                string No_Pago = Datos.P_No_Pago;
                Datos.P_No_Pago = "";
                Datos.P_Estatus = "POR PAGAR";
                // consultar datos de pasivo
                DataTable Dt_Pasivo = Consulta_Datos_Pasivo(Datos);
                Datos.P_No_Pago = No_Pago;

                Mi_SQL.Length = 0;
                //Actualiza el estatus del ingreso pasivo en la base de datos
                Mi_SQL.Append("UPDATE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" SET " + Ope_Ing_Pasivo.Campo_Estatus + " = 'PAGADO', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_No_Pago + " = '" + No_Pago + "', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Recargos + " = " + Datos.P_Monto_Recargos + ", ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Fecha_Pago + " = '" + Datos.P_Fecha_Pago.ToString("dd-MM-yyyy") + "', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Usuario_Modifico + " = NULL, ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Datos.P_Referencia + "'");
                Mi_SQL.Append(" AND TRIM(" + Ope_Ing_Pasivo.Campo_Estatus + ") = 'POR PAGAR'");
                Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                Filas_Afectadas += Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                // validar que la tabla de pasivos contenga datos
                if (Dt_Pasivo != null && Dt_Pasivo.Rows.Count > 0)
                {
                    // para cada fila en la tabla de pasivos, si contiene texto en el campo origen, llamar al método que hace afectaciones dependiendo del tipo de pasivo
                    foreach (DataRow Dr_Fila_Pasivo in Dt_Pasivo.Rows)
                    {
                        if (!string.IsNullOrEmpty(Dr_Fila_Pasivo[Ope_Ing_Pasivo.Campo_Origen].ToString()))
                        {
                            Filas_Afectadas += Afectaciones_Por_Tipo_Pasivo(Dr_Fila_Pasivo[Ope_Ing_Pasivo.Campo_Origen].ToString(), Dr_Fila_Pasivo[Ope_Ing_Pasivo.Campo_Referencia].ToString(), "", Comando_SQL);
                        }

                         if (!string.IsNullOrEmpty(Dr_Fila_Pasivo[Ope_Ing_Pasivo.Campo_Referencia].ToString()))
                            Clave_Solicitud = Dr_Fila_Pasivo[Ope_Ing_Pasivo.Campo_Referencia].ToString();

                         break;
                    }
                }

                DataTable Dt_Partidas_Poliza = Crear_Dt_Partidas_Poliza();
                DataTable Dt_Psp = Crear_Dt_Afectacion_Presupuestal();
                string Concepto_Poliza = "";
                // obtener detalles del pasivo para insertar póliza
                Mi_SQL.Length = 0;
                Mi_SQL.Append("SELECT PASIVOS." + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID);
                Mi_SQL.Append(", PASIVOS." + Ope_Ing_Pasivo.Campo_Concepto_Ing_ID);
                Mi_SQL.Append(", (NVL(PASIVOS." + Ope_Ing_Pasivo.Campo_Monto + ", 0) + NVL(PASIVOS." + Ope_Ing_Pasivo.Campo_Recargos + ", 0)) AS " + Ope_Ing_Pasivo.Campo_Monto);
                Mi_SQL.Append(", CONCEPTOS." + Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(", CONCEPTOS." + Cat_Psp_Concepto_Ing.Campo_Dependencia_ID);
                Mi_SQL.Append(", CONCEPTOS." + Cat_Psp_Concepto_Ing.Campo_Descripcion + " AS DESCRIPCION_CONCEPTO");
                Mi_SQL.Append(", SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_Descripcion + " AS DESCRIPCION_SUBCONCEPTO");
                Mi_SQL.Append(", CUENTAS_CONTABLES." + Cat_Con_Cuentas_Contables.Campo_Cuenta);
                Mi_SQL.Append(", PASIVOS." + Ope_Ing_Pasivo.Campo_Origen);
                Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PASIVOS");
                Mi_SQL.Append(", " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + " CONCEPTOS");
                Mi_SQL.Append(", " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + " SUBCONCEPTOS");
                Mi_SQL.Append(", " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + " CUENTAS_CONTABLES");
                Mi_SQL.Append(" WHERE PASIVOS." + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID + " = SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID);
                Mi_SQL.Append(" AND SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_Concepto_Ing_ID + " = CONCEPTOS." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID);
                Mi_SQL.Append(" AND CONCEPTOS." + Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID + " = CUENTAS_CONTABLES." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(" AND PASIVOS." + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Datos.P_Referencia + "'");
                Mi_SQL.Append(" AND PASIVOS." + Ope_Ing_Pasivo.Campo_Estatus + " = 'POR PAGAR'");
                DataSet Ds_Pasivos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());


                DataTable Dt_Costo = Datos.Consulta_Total_Pasivo();
                String Monto_Total = "";
                if (Dt_Costo is DataTable)
                {
                    if (Dt_Costo != null)
                    {
                        if (Dt_Costo.Rows.Count > 0)
                        {
                            foreach (DataRow Registro in Dt_Costo.Rows)
                            {
                                if (Registro is DataRow)
                                {
                                    if (Registro["MONTO"].ToString() != "")
                                    {
                                        Monto_Total = Registro["MONTO"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }

                if (Ds_Pasivos != null)
                {
                    if (Ds_Pasivos.Tables.Count > 0)
                    {
                        DataTable Dt_Pasivos = Ds_Pasivos.Tables[0];
                        DataRow Dr_Conceptos;
                        DataRow Fila;
                        foreach (DataRow Dr_Pasivos in Dt_Pasivos.Rows)
                        {
                            Dr_Conceptos = Dt_Partidas_Poliza.NewRow();
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Partida] = Dt_Partidas_Poliza.Rows.Count + 1;
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = "00261";
                            Dr_Conceptos[Cat_Con_Cuentas_Contables.Campo_Cuenta] = "112200001";
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Concepto] = Dr_Pasivos["DESCRIPCION_SUBCONCEPTO"];
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Debe] = 0;
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Haber] = Monto_Total;
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID] = "";
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Proyecto_Programa_ID] = "";
                            Dt_Partidas_Poliza.Rows.Add(Dr_Conceptos);
                            if (Concepto_Poliza.Length <= 0)
                            {
                                Concepto_Poliza = Dr_Pasivos[Ope_Ing_Pasivo.Campo_Origen].ToString();
                            }
                            // agregar datos a la fila para afectación presupuestal
                            Fila = Dt_Psp.NewRow();
                            Fila["Fte_Financiamiento_ID"] = "00022";
                            Fila["Proyecto_Programa_ID"] = "0000000654";
                            Fila["Concepto_Ing_ID"] = Dr_Pasivos[Ope_Ing_Pasivo.Campo_Concepto_Ing_ID];
                            Fila["SubConcepto_Ing_ID"] = Dr_Pasivos[Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID];
                            Fila["Anio"] = DateTime.Now.Year;
                            Fila["Importe"] = Datos.P_Total_Pagar;
                            Dt_Psp.Rows.Add(Fila);
                            break;
                        }

                        Dr_Conceptos = Dt_Partidas_Poliza.NewRow();
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Partida] = Dt_Partidas_Poliza.Rows.Count + 1;
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = "00003";// Dr_Pasivos[Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID];
                        Dr_Conceptos[Cat_Con_Cuentas_Contables.Campo_Cuenta] = " 1127976747";
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Concepto] = "BANORTE";
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Debe] = Datos.P_Total_Pagar;
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Haber] = 0;
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID] = "";
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Dependencia_ID] = "";
                        Dt_Partidas_Poliza.Rows.Add(Dr_Conceptos);
                    }
                }
                // llamar método alta_poliza
                if (Dt_Partidas_Poliza != null && Dt_Partidas_Poliza.Rows.Count > 0)
                {
                    Alta_Poliza(Datos, Concepto_Poliza, Dt_Partidas_Poliza, Dt_Psp, Comando_SQL);
                }

                ////Efectuar pago por internet!!!!! al banco Banorte
                ////Variables de envío obligatorio
                ////string header = "Name=predio&Password=predio2005&ClientId=461&Mode=P&TransType=Auth&Total=" + Datos.P_Banco_Total_Pagar + "&Number=" + Datos.P_Banco_No_Tarjeta + "&Expires=" + Datos.P_Banco_Expira_Tarjeta + "&Cvv2Indicator=1&Cvv2Val=" + Datos.P_Banco_Codigo_Seguridad + "&BillToFirstName=" + Datos.P_Cuenta_Predial + "&CardType=" + Datos.P_Banco_3D_Tipo_Tarjeta + "&XID=" + Datos.P_Banco_3D_XID + "&CAVV=" + Datos.P_Banco_3D_CAVV + "&ECI=" + Datos.P_Banco_3D_ECI + "&Status=200";
                //string header = "Name=user_test&Password=user01&ClientId=19&Mode=Y&TransType=Auth&Total=" + Datos.P_Banco_Total_Pagar + "&Number=" + Datos.P_Banco_No_Tarjeta + "&Expires=" + Datos.P_Banco_Expira_Tarjeta + "&Cvv2Indicator=1&Cvv2Val=" + Datos.P_Banco_Codigo_Seguridad + "&BillToFirstName=" + Datos.P_Cuenta_Predial + "&CardType=" + Datos.P_Banco_3D_Tipo_Tarjeta + "&XID=" + Datos.P_Banco_3D_XID + "&CAVV=" + Datos.P_Banco_3D_CAVV + "&ECI=" + Datos.P_Banco_3D_ECI + "&Status=200";

                ////Url para envío de los datos
                //string uri = "https://eps.banorte.com/recibo";

                ////Crea un request
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                //request.KeepAlive = false;
                //request.ProtocolVersion = HttpVersion.Version10;
                //request.Method = "POST";

                ////Convierte la cadena de string en bytes
                //byte[] postBytes = Encoding.ASCII.GetBytes(header);

                ////Configura el tipo de contenido del request
                //request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentLength = postBytes.Length;
                //Stream requestStream = request.GetRequestStream();

                ////Envia el request
                //requestStream.Write(postBytes, 0, postBytes.Length);
                //requestStream.Close();

                ////Obtiene el codigo de respuesta
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //String[] Respuesta = response.Headers.ToString().Split(new String[] { "\r\n" }, System.StringSplitOptions.None); //En la posición 15 se encuentra el error...
                //int i;
                //for (i = 0; i < Respuesta.Length; i++)
                //{
                //    if (Respuesta[i].Contains("CcErrCode"))
                //    {
                //        break;
                //    }
                //}
                //String Codigo = Respuesta[i].Split(':')[1].Trim();

                ////Obtiene el codigo de autorizacion
                //for (i = 0; i < Respuesta.Length; i++)
                //{
                //    if (Respuesta[i].Contains("AuthCode"))
                //    {
                //        break;
                //    }
                //}
                //String Codigo_Autorizacion = Respuesta[i].Split(':')[1].Trim();

                ////*********** Quitar código de prueba al descomentar pago al banco
                String Codigo = "1";
                String Codigo_Autorizacion = "1234567890";
                ////***********

                ////Cierra la respuesta
                //response.Close();
                String Mensaje;

                //Valida el tipo de codigo
                switch (Codigo)
                {
                    case "1":
                        Mensaje = "Pago correcto .";
                        break;
                    case "50":
                        Mensaje = "Transacción declinada .";
                        throw new Exception(Mensaje);
                    case "54":
                        Mensaje = "Conexión fuera de tiempo .";
                        throw new Exception(Mensaje);
                    case "500":
                        Mensaje = "Fallo en el tiempo para respuesta .";
                        throw new Exception(Mensaje);
                    case "1002":
                        Mensaje = "Declinada - Transacción fraudulenta .";
                        throw new Exception(Mensaje);
                    case "1007":
                        Mensaje = "Monto no válido.";
                        throw new Exception(Mensaje);
                    case "1011":
                        Mensaje = "No. de tarjeta no válido .";
                        throw new Exception(Mensaje);
                    case "1050":
                        Mensaje = "Declinado - Fondos insuficientes .";
                        throw new Exception(Mensaje);
                    case "1051":
                        Mensaje = "Tarjeta del cliente vencida .";
                        throw new Exception(Mensaje);
                    case "2078":
                        Mensaje = "Tarjeta no activa .";
                        throw new Exception(Mensaje);
                    default:
                        Mensaje = "TRANSACCION RECHAZADA - Error desconocido... favor de comunicarse con su banco .";
                        throw new Exception(Mensaje);
                }
                //Asigna el codigo del banco
                Datos.P_Banco_Clave_Operacion = Codigo_Autorizacion;
                //Fin pago por internet

                //Agrega los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Datos.P_Dt_Formas_Pago.Rows)
                {
                    Mi_SQL.Length = 0;
                    //Consulta el último no de pago que fue registrado en la base de datos
                    Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + "),0)");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles);
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Consecutivo = Comando_SQL.ExecuteOracleScalar().ToString();
                    //Consecutivo = OracleHelper.ExecuteScalar(Transaccion_SQL, CommandType.Text, Mi_SQL.ToString());

                    if (Convert.IsDBNull(Consecutivo))
                    {
                        Consecutivo = 1;
                    }
                    else
                    {
                        Consecutivo = Convert.ToInt64(Consecutivo) + 1;
                    }
                    Mi_SQL.Length = 0;
                    if (Registro["Forma_Pago"].ToString() == "INTERNET") //Forma de Pago en Efectivo
                    {
                        //Inserción de forma de pago en la base de datos
                        Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(");
                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ");
                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Autorizacion + ", ");
                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")");
                        Mi_SQL.Append(" VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', '");
                        Mi_SQL.Append(Datos.P_Banco_Clave_Operacion + "', " + Convert.ToDecimal(Registro["Monto"].ToString()) + ", " + Consecutivo + ")");

                    }
                    else if (Registro["Forma_Pago"].ToString() == "AJUSTE TARIFARIO") //Ajuste tarifario
                    {
                        //Inserción de forma de pago en la base de datos
                        Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(");
                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ");
                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")");
                        Mi_SQL.Append(" VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', ");
                        Mi_SQL.Append(Convert.ToDecimal(Registro["Monto"].ToString()) + ", " + Consecutivo + ")");
                    }
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Filas_Afectadas += Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                }

                Avaluo_Pagado(ref Comando_SQL, Datos);
                Soolicitud_Registro_Pagado(ref Comando_SQL, Datos);

                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos

                Cambiar_Estatus_Complemento_Solicitud(Clave_Solicitud);

            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                Filas_Afectadas = 0;
                throw new Exception("Error: " + Ex.Message + " SQL: " + Mi_SQL);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                Filas_Afectadas = 0;
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]" + " SQL: " + Mi_SQL);
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                Filas_Afectadas = 0;
                throw new Exception("Error: " + Ex.Message + " SQL: " + Mi_SQL);
            }
            finally
            {
                Conexion_Base.Close();
            }

            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Afectaciones_Por_Tipo_Pasivo
        /// DESCRIPCIÓN: Dependiendo del tipo de pasivo (campo ORIGEN) llama al método para hacer la afectación correspondiente
        ///             (p.ej.: pasivos de trámites: insertar registro de pago en bitácora del trámite)
        /// PARÁMETROS:
        /// 		1. Tipo_Pasivo: cadena de caracteres para identificar el pasivo y las afectaciones a hacer
        /// 		2. Referencia: referencia del pasivo para encontrar el id del trámite
        /// 		3. Nombre_Usuario: parámetro para insertar en el campos de control USUARIO_CREO
        /// 		4. Cmd: Conexión a la base de datos (para utilizar la misma transacción)
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 28-jun-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Afectaciones_Por_Tipo_Pasivo(string Tipo_Pasivo, string Referencia, string Nombre_Usuario, OracleCommand Cmd)
        {
            String Mi_SQL;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Int32 Filas_Afectadas = 0;
            int No_Detalle_Solicitud;

            // validar que Tipo_Pasivo contenga texto
            if (string.IsNullOrEmpty(Tipo_Pasivo))
            {
                return 0;
            }

            try
            {
                // si llego un Comando como parámetro, utilizarlo
                if (Cmd != null)    // si la conexión llego como parámetro, establecer como comando para utilizar
                {
                    Comando = Cmd;
                }
                else    // si no, crear nueva conexión y transacción
                {
                    Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Conexion;
                    Comando.Transaction = Transaccion;
                }
                Comando.CommandType = CommandType.Text;

                // si es una solicitud de trámite, insertar registro de pago en bitácora del trámite
                if (Tipo_Pasivo.Trim() == "SOLICITUD TRAMITE")
                {
                    Cls_Ope_Bandeja_Tramites_Negocio Neg_Actualizar_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();

                    // consultar el trámite con el folio igual a la referencia del pasivo
                    Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                        + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                        + "," + Ope_Tra_Solicitud.Campo_Estatus
                        + "," + Ope_Tra_Solicitud.Campo_Complemento
                        + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                        + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ")) = UPPER(TRIM('" + Referencia + "'))";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.CommandType = CommandType.Text;
                    OracleDataReader Dtr_Datos_Solicitud = Cmd.ExecuteReader();

                    // si hay datos para leer, agregar pasivo
                    if (Dtr_Datos_Solicitud.Read())
                    {
                        if (Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Complemento].ToString() == "")
                        {
                            // establecer parámetros para actualizar solicitud
                            Neg_Actualizar_Solicitud.P_Solicitud_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString().Trim();
                            Neg_Actualizar_Solicitud.P_Estatus = "APROBAR";
                            Neg_Actualizar_Solicitud.P_Subproceso_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString().Trim();
                            Neg_Actualizar_Solicitud.P_Comentarios = "RECEPCION DE PAGO";
                            Neg_Actualizar_Solicitud.P_Usuario = Nombre_Usuario;
                            // pasar comando de oracle para utilizar la misma transacción
                            Neg_Actualizar_Solicitud.P_Comando_Oracle = Cmd;
                            // llamar método que actualizar la solicitud
                            Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Evaluar_Solicitud();
                        }
                        //// ya se inserta bitácora en la llamada a Evaluar_Solicitud()
                        //Filas_Afectadas += Alta_Bitacora_Solicitud_Tramite(Obj_Solicitud.ToString(), "PAGADO", "RECEPCION DE PAGO", Cmd);
                    }
                }

                // si la conexión no llego como parámetro, aplicar consultas
                if (Cmd == null)
                {
                    Transaccion.Commit();
                }

                return Filas_Afectadas;
            }
            catch (OracleException Ex)
            {
                if (Cmd == null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Alta_Bitacora_Solicitud_Tramite Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Cmd == null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Cmd == null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Alta_Bitacora_Solicitud_Tramite Error: " + Ex.Message);
            }
            finally
            {
                if (Cmd == null)
                {
                    Conexion.Close();
                }
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Alta_Bitacora_Solicitud_Tramite
        /// DESCRIPCIÓN: Forma y ejecuta una consulta para insertar un pasivo de solicitud de trámite
        /// PARÁMETROS:
        /// 		1. No_Solicitud: número de solicitud a insertar
        /// 		2. Estatus: Estatus de la solicitud
        /// 		3. Comentario: string a insertar en la tabla como comentario
        /// 		4. Cmd: Conexión a la base de datos (para utilizar la misma transacción)
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 28-jun-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Alta_Bitacora_Solicitud_Tramite(string No_Solicitud, string Estatus, string Comentario, OracleCommand Cmd)
        {
            String Mi_SQL;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Int32 Filas_Afectadas = 0;
            int No_Detalle_Solicitud;

            try
            {
                // si llego un Comando como parámetro, utilizarlo
                if (Cmd != null)    // si la conexión llego como parámetro, establecer como comando para utilizar
                {
                    Comando = Cmd;
                }
                else    // si no, crear nueva conexión y transacción
                {
                    Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Conexion;
                    Comando.Transaction = Transaccion;
                }
                Comando.CommandType = CommandType.Text;

                // obtener el máximo contador en ope_tra_det_solicitud
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Tra_Det_Solicitud.Campo_Detalle_Solicitud_ID + "), 0) FROM "
                    + Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud;
                Comando.CommandText = Mi_SQL;
                int.TryParse(Comando.ExecuteScalar().ToString(), out No_Detalle_Solicitud);
                No_Detalle_Solicitud++;

                // consultar detalles de la solicitud
                Mi_SQL = "INSERT INTO " + Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud
                    + "(" + Ope_Tra_Det_Solicitud.Campo_Detalle_Solicitud_ID
                    + ", " + Ope_Tra_Det_Solicitud.Campo_Solicitud_ID
                    + ", " + Ope_Tra_Det_Solicitud.Campo_Estatus
                    + ", " + Ope_Tra_Det_Solicitud.Campo_Comentarios
                    + ", " + Ope_Tra_Det_Solicitud.Campo_Fecha
                    + ") VALUES ("
                    + "'" + No_Detalle_Solicitud.ToString().PadLeft(10, '0') + "'"
                    + ", '" + No_Solicitud + "'"
                    + ", '" + Estatus + "'"
                    + ", '" + Comentario + "'"
                    + ", SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Filas_Afectadas += Cmd.ExecuteNonQuery();

                if (Cmd == null)    // si la conexión no llego como parámetro, aplicar consultas
                {
                    Transaccion.Commit();
                }

                return Filas_Afectadas;
            }
            catch (OracleException Ex)
            {
                if (Cmd == null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Alta_Bitacora_Solicitud_Tramite Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Cmd == null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Cmd == null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Alta_Bitacora_Solicitud_Tramite Error: " + Ex.Message);
            }
            finally
            {
                if (Cmd == null)
                {
                    Conexion.Close();
                }
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION    : Alta_Poliza
        /// DESCRIPCION             : Da de Alta la poliza con los datos de los Pasivos
        /// PARAMETROS: 
        /// CREO                    : Antonio Salvador Benavides Guardado
        /// FECHA_CREO              : 15/Junio/2012
        /// MODIFICO:
        /// FECHA_MODIFICO:
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private static void Alta_Poliza(Cls_Ope_Ate_Pagos_Internet_Negocio Datos, String Concepto_Poliza, DataTable Dt_Partidas_Poliza, DataTable Dt_Psp, OracleCommand Comando_Oracle)
        {
            Cls_Ope_Con_Polizas_Negocio Polizas = new Cls_Ope_Con_Polizas_Negocio();
            DataTable Dt_Jefe_Dependencia = null; 
            StringBuilder Mi_SQL = new StringBuilder();
            String Referencia = "";
            try
            {
                Polizas.P_Empleado_ID = Datos.P_Empleado_ID;
                Dt_Jefe_Dependencia = Polizas.Consulta_Empleado_Jefe_Dependencia();
                Polizas = null;

                Polizas = new Cls_Ope_Con_Polizas_Negocio();
                Polizas.P_Tipo_Poliza_ID = "00001";
                Polizas.P_Mes_Ano = Datos.P_Fecha_Pago.ToString("MMyy");
                Polizas.P_Fecha_Poliza = Datos.P_Fecha_Pago;
                Polizas.P_Concepto = Concepto_Poliza;
                Polizas.P_Total_Debe = Convert.ToDouble(Datos.P_Total_Pagar);
                Polizas.P_Total_Haber = Convert.ToDouble(Datos.P_Total_Pagar);
                Polizas.P_No_Partida = Dt_Partidas_Poliza.Rows.Count;
                Polizas.P_Nombre_Usuario = Datos.P_Nombre_Usuario;
                Polizas.P_Dt_Detalles_Polizas = Dt_Partidas_Poliza;
                Polizas.P_Empleado_ID_Creo = Datos.P_Empleado_ID;
                Polizas.P_Empleado_ID_Autorizo = "";
                Polizas.P_Cmmd = Comando_Oracle;
                string[] Datos_Poliza = Polizas.Alta_Poliza(); //Da de alta los datos de la Póliza proporcionados por el usuario en la BD

                Cls_Ope_Con_Poliza_Ingresos_Datos.Alta_Movimientos_Presupuestales(
                    Dt_Psp,
                    Comando_Oracle,
                    String.Empty,
                    Datos_Poliza[0],
                    Datos_Poliza[1],
                    Datos_Poliza[2],
                    Ope_Psp_Presupuesto_Ingresos.Campo_Devengado,
                    Ope_Psp_Presupuesto_Ingresos.Campo_Recaudado);

                if (Datos.P_Referencia != "")
                {
                    Referencia = Datos.P_Referencia;
                }

                //Mi_SQL.Append("Update " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " Set ");
                //Mi_SQL.Append(Ope_Ing_Pasivo.Campo_No_Poliza + "='" + Datos_Poliza[0] + "'");
                //Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Tipo_Poliza_Id + "='" + Datos_Poliza[1] + "'");
                //Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Mes_Ano + "='" + Datos_Poliza[2] + "'");
                //Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Campo_Referencia + "='" + Referencia + "'");
                //Comando_Oracle.CommandText = Mi_SQL.ToString();
                //Comando_Oracle.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Alta_Poliza " + ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Crear_Dt_Partidas_Poliza
        ///DESCRIPCIÓN          : Metodo que devuelve un DataTable con los campos indicados
        ///PROPIEDADES:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 15/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************        
        private static DataTable Crear_Dt_Partidas_Poliza()
        {
            DataTable Dt_Partidas_Poliza = new DataTable();

            //Agrega los campos que va a contener el DataTable
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Proyecto_Programa_ID, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Concepto, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Partida, typeof(System.Int32));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add(Cat_Con_Cuentas_Contables.Campo_Cuenta, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Debe, typeof(System.Double));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Haber, typeof(System.Double));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Partida_ID, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Dependencia_ID, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add("MOMENTO_INICIAL", typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add("MOMENTO_FINAL", typeof(System.String));

            return Dt_Partidas_Poliza;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Crear_Dt_Afectacion_Presupuestal
        ///DESCRIPCIÓN          : Metodo que devuelve un DataTable con los campos necesarios
        ///                     para pasar al método que realiza la afectación presupuestal
        ///PROPIEDADES:     
        ///CREO                 : Roberto González Oseguera
        ///FECHA_CREO           : 19-sep-2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************        
        private static DataTable Crear_Dt_Afectacion_Presupuestal()
        {
            DataTable Dt_Datos = new DataTable();

            //Agrega los campos que va a contener el DataTable
            Dt_Datos.Columns.Add("Fte_Financiamiento_ID", typeof(System.String));
            Dt_Datos.Columns.Add("Proyecto_Programa_ID", typeof(System.String));
            Dt_Datos.Columns.Add("Concepto_Ing_ID", typeof(System.String));
            Dt_Datos.Columns.Add("SubConcepto_Ing_ID", typeof(System.String));
            Dt_Datos.Columns.Add("Anio", typeof(System.String));
            Dt_Datos.Columns.Add("Importe", typeof(System.String));

            return Dt_Datos;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Pasivo
        /// DESCRIPCION : Consulta todos los datos del recibo para poder realizar el pago
        ///               de acuerdo a la referencia que proporciono el usuario
        /// PARAMETROS  : Datos: Contiene los datos de los parametros para la realización
        ///                      de la consulta
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 22-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Pasivo(Cls_Ope_Ate_Pagos_Internet_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            try
            {
                //Consulta los datos generales del pasivo que se quiere consultar
                Mi_SQL.Append("SELECT " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + ".*, ");
                Mi_SQL.Append("(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Monto + " + ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Recargos + ") AS Total_Pagar");
                Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" WHERE TRIM(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Referencia + ") = '" + Datos.P_Referencia + "'");
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Origen + " IS NOT NULL");
                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    Mi_SQL.Append(" AND TRIM(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + ".Estatus) = TRIM('" + Datos.P_Estatus + "')");
                }
                if (!string.IsNullOrEmpty(Datos.P_No_Pago))
                {
                    Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + ".No_Pago = '" + Datos.P_No_Pago + "'");
                }
                Mi_SQL.Append(" ORDER BY " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pasivo);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consulta_Solicitud_Por_Pasivo
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para obtener los datos de una Solicitud dada una referencia de pasivo
        ///PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los parámetros para la consulta
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 19-jul-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Solicitud_Por_Pasivo(Cls_Ope_Ate_Pagos_Internet_Negocio Datos)
        {
            string Mi_SQL;

            try
            {
                //Consulta los datos generales del pasivo que se quiere consultar
                Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                        + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                        + "," + Ope_Tra_Solicitud.Campo_Clave_Solicitud
                        + "," + Ope_Tra_Solicitud.Campo_Estatus
                        + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                        + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ")) = UPPER(TRIM('" + Datos.P_Referencia + "'))";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }


        ///*******************************************************************************************************
        ///NOMBRE_FUNCIÓN: Consulta_Total_Pasivo
        ///DESCRIPCIÓN: Forma y ejecuta una consulta para obtener los datos de una Solicitud dada una referencia de pasivo
        ///PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con los parámetros para la consulta
        ///CREO: Hugo Enrique Ramirez Aguilera
        ///FECHA_CREO: 27-Nov-2012
        ///MODIFICÓ: 
        ///FECHA_MODIFICÓ: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Total_Pasivo(Cls_Ope_Ate_Pagos_Internet_Negocio Datos)
        {
            string Mi_SQL;

            try
            {
                Mi_SQL = "SELECT SUM(" + Ope_Ing_Pasivo.Campo_Monto + " + " + Ope_Ing_Pasivo.Campo_Recargos+ ") as MONTO " +
                    " from " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo +
                    " Where trim(" + Ope_Ing_Pasivo.Campo_Referencia + ") = upper(trim('" + Datos.P_Referencia + "'))";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cambiar_Estatus_Complemento_Solicitud
        ///DESCRIPCIÓN          : para cambiar el estatus de las solicitudes que se derivan de la principal  
        ///PARAMETROS:          : String Clave_Solicitud la clave de la solicitud   
        ///CREO                 : Ismael Prieto Sánchez
        ///FECHA_CREO           : 08/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Cambiar_Estatus_Complemento_Solicitud(String Clave_Solicitud)
        {
            String Solicitud_ID = "";

            try
            {
                // consultar el trámite con el folio igual a la referencia del pasivo
                String Mi_SQL_Consulta = "SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                    + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                    + "," + Ope_Tra_Solicitud.Campo_Estatus
                    + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                    + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ")) = UPPER(TRIM('" + Clave_Solicitud + "'))";
                DataTable Dt_Consulta_Solicitud_ID = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL_Consulta).Tables[0];

                if (Dt_Consulta_Solicitud_ID != null && Dt_Consulta_Solicitud_ID.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Consulta_Solicitud_ID.Rows)
                    {
                        if (!String.IsNullOrEmpty(Registro[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString()))
                        {
                            Solicitud_ID = Registro[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString();

                            Cls_Ope_Bandeja_Tramites_Negocio Negocio_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                            Negocio_Solicitud.P_Solicitud_ID = Solicitud_ID;
                            Negocio_Solicitud = Negocio_Solicitud.Consultar_Datos_Solicitud();

                            Negocio_Solicitud.P_Solicitud_ID = Solicitud_ID;
                            Negocio_Solicitud.P_Subproceso_ID = Negocio_Solicitud.P_Subproceso_ID;
                            Negocio_Solicitud.P_Estatus = Negocio_Solicitud.P_Estatus;
                            Negocio_Solicitud.P_Porcentaje_Avance = Negocio_Solicitud.P_Porcentaje_Avance;

                            StringBuilder Mi_Sql = new StringBuilder();

                            Mi_Sql.Append("UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SET ");
                            Mi_Sql.Append(Ope_Tra_Solicitud.Campo_Subproceso_ID + "='" + Negocio_Solicitud.P_Subproceso_ID + "' ");
                            Mi_Sql.Append(", " + Ope_Tra_Solicitud.Campo_Estatus + "='" + Negocio_Solicitud.P_Estatus + "' ");
                            Mi_Sql.Append(", " + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + "='" + Negocio_Solicitud.P_Porcentaje_Avance + "' ");
                            Mi_Sql.Append(" WHERE " + Ope_Tra_Solicitud.Campo_Complemento + "='" + Negocio_Solicitud.P_Solicitud_ID + "' ");

                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString());
                        }
                    }
                }
            }
            catch (Exception Ex)
            {

                throw new Exception("Error: " + Ex.Message);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Aplica_Traslado_Convenio
        ///DESCRIPCIÓN          : Realiza la aplicacion de los adeudos de predial
        ///PARAMETROS:          : Cmmd, pasa el command para ejecutar    
        ///CREO                 : Ismael Prieto Sánchez
        ///FECHA_CREO           : 08/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Avaluo_Pagado(ref OracleCommand Cmmd, Cls_Ope_Ate_Pagos_Internet_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            String Solicitud_Id = "";
            Mi_SQL.Length = 0;
            String Consulta = "SELECT AU." + Ope_Cat_Avaluo_Urbano.Campo_Solicitud_Id + " FROM " + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano + " AU LEFT OUTER JOIN " +
                Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " TS ON AU." + Ope_Cat_Avaluo_Urbano.Campo_Solicitud_Id + "=TS." + Ope_Tra_Solicitud.Campo_Solicitud_ID +
                " LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " CT ON TS." + Ope_Tra_Solicitud.Campo_Tramite_ID + "=CT." + Cat_Tra_Tramites.Campo_Tramite_ID +
                " WHERE UPPER(CT." + Cat_Tra_Tramites.Campo_Nombre + ")='AVALUO' AND TRIM(UPPER(TS." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ")) = '" + Datos.P_Referencia + "'";
            Solicitud_Id = Obtener_Dato_Consulta(Consulta);
            if (Solicitud_Id != "")
            {
                Mi_SQL.Append("UPDATE " + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano + " SET " + Ope_Cat_Avaluo_Urbano.Campo_Estatus + " = 'PAGADO'");
                Mi_SQL.Append(", " + Ope_Cat_Avaluo_Urbano.Campo_Permitir_Revision + "= 'NO' ");
                Mi_SQL.Append(" WHERE " + Ope_Cat_Avaluo_Urbano.Campo_Solicitud_Id + " = '" + Solicitud_Id + "'");
                Cmmd.CommandText = Mi_SQL.ToString();
                Cmmd.ExecuteNonQuery();
            }
            else
            {
                Consulta = "SELECT AU." + Ope_Cat_Avaluo_Rustico.Campo_Solicitud_Id + " FROM " + Ope_Cat_Avaluo_Rustico.Tabla_Ope_Cat_Avaluo_Rustico + " AU LEFT OUTER JOIN " +
                Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " TS ON AU." + Ope_Cat_Avaluo_Rustico.Campo_Solicitud_Id + "=TS." + Ope_Tra_Solicitud.Campo_Solicitud_ID +
                " LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " CT ON TS." + Ope_Tra_Solicitud.Campo_Tramite_ID + "=CT." + Cat_Tra_Tramites.Campo_Tramite_ID +
                " WHERE UPPER(CT." + Cat_Tra_Tramites.Campo_Nombre + ")='AVALUO' AND TRIM(UPPER(TS." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ")) = '" + Datos.P_Referencia + "'";
                Solicitud_Id = Obtener_Dato_Consulta(Consulta);
                if (Solicitud_Id != "")
                {
                    //Actualiza el calculo
                    Mi_SQL.Append("UPDATE " + Ope_Cat_Avaluo_Rustico.Tabla_Ope_Cat_Avaluo_Rustico + " SET " + Ope_Cat_Avaluo_Rustico.Campo_Estatus + " = 'PAGADO'");
                    Mi_SQL.Append(", " + Ope_Cat_Avaluo_Rustico.Campo_Permitir_Revision + "= 'NO' ");
                    Mi_SQL.Append(" WHERE " + Ope_Cat_Avaluo_Rustico.Campo_Solicitud_Id + " = '" + Solicitud_Id + "'");
                    Cmmd.CommandText = Mi_SQL.ToString();
                    Cmmd.ExecuteNonQuery();
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Soolicitud_Registro_Pagado
        ///DESCRIPCIÓN          : Aplica 
        ///PARAMETROS:          : Cmmd, pasa el command para ejecutar    
        ///CREO                 : Ismael Prieto Sánchez
        ///FECHA_CREO           : 08/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Soolicitud_Registro_Pagado(ref OracleCommand Cmmd, Cls_Ope_Ate_Pagos_Internet_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            String Solicitud_Id = "";
            Mi_SQL.Length = 0;
            String Consulta = "SELECT TS." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " TS LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TT ";
            Consulta += "ON TS." + Ope_Tra_Solicitud.Campo_Tramite_ID + "=TT." + Cat_Tra_Tramites.Campo_Tramite_ID + " WHERE UPPER(TT." + Cat_Tra_Tramites.Campo_Nombre + ") = 'SOLICITUD DE REGISTRO' AND TRIM(UPPER(TS." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ")) = '" + Datos.P_Referencia + "'";
            Solicitud_Id = Obtener_Dato_Consulta(Consulta);
            if (Solicitud_Id != "")
            {
                Consulta = "SELECT * FROM " + Cat_Cat_Temp_Peritos_Externos.Tabla_Cat_Cat_Temp_Peritos_Externos + " WHERE " + Cat_Cat_Temp_Peritos_Externos.Campo_Solicitud_id + "= '" + Solicitud_Id + "'";
                Cmmd.CommandText = Consulta;
                DataTable Dt_Perito_Externo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Consulta.ToString()).Tables[0];

                if (Dt_Perito_Externo.Rows.Count > 0)
                {
                    Mi_SQL.Append("SELECT NVL(MAX(" + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + "),'0')");
                    Mi_SQL.Append(" FROM " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos);
                    Cmmd.CommandText = Mi_SQL.ToString();
                    Object Perito_Externo_Id = Cmmd.ExecuteOracleScalar().ToString();
                    if (Convert.IsDBNull(Perito_Externo_Id))
                    {
                        Perito_Externo_Id = "00001";
                    }
                    else
                    {
                        Perito_Externo_Id = String.Format("{0:00000}", Convert.ToInt32(Perito_Externo_Id) + 1);
                    }

                    Mi_SQL.Length = 0;

                    Mi_SQL.Append("INSERT INTO " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos + " (" + Cat_Cat_Peritos_Externos.Campo_Apellido_Materno);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Calle);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Celular);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Ciudad);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Colonia);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Estado);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Estatus);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Fecha);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Fecha_Aceptacion);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Fecha_Creo);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Informacion);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Nombre);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Observaciones);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Telefono);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Usuario);
                    Mi_SQL.Append(", " + Cat_Cat_Peritos_Externos.Campo_Usuario_Creo + ") VALUES ('");
                    Mi_SQL.Append(Dt_Perito_Externo.Rows[0][Cat_Cat_Temp_Peritos_Externos.Campo_Apellido_Materno].ToString() + "', '");
                    Mi_SQL.Append(Dt_Perito_Externo.Rows[0][Cat_Cat_Temp_Peritos_Externos.Campo_Apellido_Paterno].ToString() + "', '");
                    Mi_SQL.Append(Dt_Perito_Externo.Rows[0][Cat_Cat_Temp_Peritos_Externos.Campo_Calle].ToString() + "', '");
                    Mi_SQL.Append(Dt_Perito_Externo.Rows[0][Cat_Cat_Temp_Peritos_Externos.Campo_Celular].ToString() + "', '");
                    Mi_SQL.Append(Dt_Perito_Externo.Rows[0][Cat_Cat_Temp_Peritos_Externos.Campo_Ciudad].ToString() + "', '");
                    Mi_SQL.Append(Dt_Perito_Externo.Rows[0][Cat_Cat_Temp_Peritos_Externos.Campo_Colonia].ToString() + "', '");
                    Mi_SQL.Append(Dt_Perito_Externo.Rows[0][Cat_Cat_Temp_Peritos_Externos.Campo_Estado].ToString() + "', '");
                    Mi_SQL.Append("VIGENTE', ");
                    Mi_SQL.Append("'31-12-" + DateTime.Now.Year.ToString() + "', ");
                    Mi_SQL.Append("SYSDATE, ");
                    Mi_SQL.Append("SYSDATE, '");
                    Mi_SQL.Append(Dt_Perito_Externo.Rows[0][Cat_Cat_Temp_Peritos_Externos.Campo_Informacion].ToString() + "', '");
                    Mi_SQL.Append(Dt_Perito_Externo.Rows[0][Cat_Cat_Temp_Peritos_Externos.Campo_Nombre].ToString() + "', '");
                    Mi_SQL.Append(Dt_Perito_Externo.Rows[0][Cat_Cat_Temp_Peritos_Externos.Campo_Observaciones].ToString() + "', '");
                    Mi_SQL.Append(Perito_Externo_Id.ToString() + "', '");
                    Mi_SQL.Append(Dt_Perito_Externo.Rows[0][Cat_Cat_Temp_Peritos_Externos.Campo_Telefono].ToString() + "', '");
                    Mi_SQL.Append(Dt_Perito_Externo.Rows[0][Cat_Cat_Temp_Peritos_Externos.Campo_E_Mail].ToString() + "', '");
                    Mi_SQL.Append("')");
                    Cmmd.CommandText = Mi_SQL.ToString();
                    Cmmd.ExecuteNonQuery();
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Obtener_Dato_Consulta(String Consulta)
        {
            String Dato_Consulta = "";

            try
            {
                OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Consulta);
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
            { }
            finally
            { }
            return Dato_Consulta;
        }

    }
}