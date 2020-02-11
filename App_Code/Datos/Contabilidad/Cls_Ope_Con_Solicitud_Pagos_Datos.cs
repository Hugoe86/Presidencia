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
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Text;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Solicitud_Pagos.Negocio;
using Presidencia.Manejo_Presupuesto.Datos;
using Presidencia.Sessiones;

namespace Presidencia.Solicitud_Pagos.Datos
{
    public class Cls_Ope_Con_Solicitud_Pagos_Datos
    {
        #region (Métodos Operación)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Solicitud_Pago
            /// DESCRIPCION : 1.Consulta el último No dado de alta para poder ingresar el siguiente
            ///               2. Da de Alta La Solictud de Pago en la BD con los datos 
            ///                  proporcionados por elusuario
            /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 17-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Alta_Solicitud_Pago(Cls_Ope_Con_Solicitud_Pagos_Negocio Datos)
            {
                String Mes_Anio = String.Format("{0:MMyy}", DateTime.Today); //Obtiene el mes y año que se le asiganara a la póliza
                StringBuilder Mi_SQL = new StringBuilder();      //Obtiene los datos de la inserción a realizar a la base de datos
                Object No_Solicitud_Pago;                        //Obtiene el último número de registro que fue dado de alta en la base de datos
                Object No_Poliza = null;                         //Obtiene el No con la cual se guardo los datos en la base de datos
                Object Consecutivo = null;                       //Obtiene el consecutivo con la cual se guardo los datos en la base de datos
                Object Saldo;                                    //Obtiene el saldo de la cuenta contable                
                OracleCommand Comando_SQL = new OracleCommand(); //Sirve para la ejecución de las operaciones a la base de datos
                OracleTransaction Transaccion_SQL;               //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
                OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        

                try
                {
                    if (Conexion_Base.State != ConnectionState.Open)
                    {
                        Conexion_Base.Open(); //Abre la conexión a la base de datos            
                    }
                    Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
                    Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
                    Comando_SQL.Transaction = Transaccion_SQL;

                    //Consulta el último No que fue dato de alta en la base de datos
                    Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + "),'0000000000')");
                    Mi_SQL.Append(" FROM " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos);

                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Realiza la ejecuón de la obtención del ID del empleado
                    No_Solicitud_Pago = Comando_SQL.ExecuteScalar();

                    if (Convert.IsDBNull(No_Solicitud_Pago))
                    {
                        Datos.P_No_Solicitud_Pago = "0000000001";
                    }
                    else
                    {
                        Datos.P_No_Solicitud_Pago = String.Format("{0:0000000000}", Convert.ToInt32(No_Solicitud_Pago) + 1);
                    }

                    Mi_SQL.Length = 0;

                    //Inserta un nuevo registro en la base de datos con los datos obtenidos por el usuario
                    Mi_SQL.Append("INSERT INTO " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos);
                    Mi_SQL.Append(" (" + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + ", " + Ope_Con_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_No_Reserva + ", " + Ope_Con_Solicitud_Pagos.Campo_Concepto + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Fecha_Solicitud + ", " + Ope_Con_Solicitud_Pagos.Campo_Monto + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_No_Factura + ", " + Ope_Con_Solicitud_Pagos.Campo_Fecha_Factura + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Proveedor_ID + ", " + Ope_Con_Solicitud_Pagos.Campo_Estatus + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Usuario_Creo + ", " + Ope_Con_Solicitud_Pagos.Campo_Fecha_Creo + ")");
                    Mi_SQL.Append(" VALUES ('" + Datos.P_No_Solicitud_Pago + "', '" + Datos.P_Tipo_Solicitud_Pago_ID + "'," + Convert.ToInt32(Datos.P_No_Reserva)+ ",");
                    Mi_SQL.Append(" '" + Datos.P_Concepto + "', SYSDATE, " + Datos.P_Monto + ", '" + Datos.P_No_Factura + "',");
                    Mi_SQL.Append(" TO_DATE('" + Datos.P_Fecha_Factura + "', 'DD/MM/YY'), '" + Datos.P_Proveedor_ID + "', '" + Datos.P_Estatus + "',");
                    Mi_SQL.Append(" '" + Datos.P_Nombre_Usuario + "', SYSDATE)");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();               //Ejecuta la inserción en memoria antes de pasarla a la base de datos  

                    Mi_SQL.Length = 0;
                    //Actualiza el saldo de la reserva
                    Mi_SQL.Append("UPDATE " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas);
                    Mi_SQL.Append(" SET " + Ope_Psp_Reservas.Campo_Saldo + " = " + Ope_Psp_Reservas.Campo_Saldo + " - " + Datos.P_Monto + ", ");
                    Mi_SQL.Append(Ope_Psp_Reservas.Campo_Beneficiario + " = '" + Datos.P_Beneficiario + "'");
                    Mi_SQL.Append(" WHERE " + Ope_Psp_Reservas.Campo_No_Reserva + " = " + Convert.ToInt32(Datos.P_No_Reserva));
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();               //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL.Length = 0;
                    //Consulta para la obtención del último ID dado de alta 
                    Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Con_Polizas.Campo_No_Poliza + "),'000000000')");
                    Mi_SQL.Append(" FROM " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas);
                    Mi_SQL.Append(" WHERE " + Ope_Con_Polizas.Campo_Mes_Ano + " = '" + Mes_Anio + "'");
                    Mi_SQL.Append(" AND " + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " = '00001'");
                    Mi_SQL.Append(" ORDER BY " + Ope_Con_Polizas.Campo_No_Poliza + " DESC");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Realiza la ejecuón de la obtención del ID del empleado
                    No_Poliza = Comando_SQL.ExecuteScalar();
                    //Valida si el ID es nulo para asignarle automaticamente el primer registro
                    if (Convert.IsDBNull(No_Poliza))
                    {
                        No_Poliza = "0000000001";
                    }
                    //Si no esta vacio el registro entonces al registro que se obtenga se le suma 1 para poder obtener el último registro
                    else
                    {
                        No_Poliza = String.Format("{0:0000000000}", Convert.ToInt32(No_Poliza) + 1);
                    }
                    Mi_SQL.Length = 0;
                    //Consulta para la inserción de la póliza con los datos proporcionados por el usuario
                    Mi_SQL.Append("INSERT INTO " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas);
                    Mi_SQL.Append(" (" + Ope_Con_Polizas.Campo_No_Poliza + ", " + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + ", ");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Mes_Ano + ", " + Ope_Con_Polizas.Campo_Fecha_Poliza + ", ");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Concepto + ", " + Ope_Con_Polizas.Campo_Total_Debe + ", ");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Total_Haber + ", " + Ope_Con_Polizas.Campo_No_Partidas + ", ");
                    Mi_SQL.Append(Cat_Empleados.Campo_Usuario_Creo + ", " + Cat_Empleados.Campo_Fecha_Creo + ", ");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Empleado_ID_Creo + ", " + Ope_Con_Polizas.Campo_Empleado_ID_Autorizo + ")");
                    Mi_SQL.Append(" VALUES ('" + No_Poliza + "', '00001', '" + Mes_Anio + "',");
                    Mi_SQL.Append(" TO_DATE('" + String.Format("{0:dd/MM/yyyy}", DateTime.Today) + "','DD/MM/YYYY'),");
                    Mi_SQL.Append(" '" + Datos.P_Concepto + "', " + Datos.P_Monto + ", " + Datos.P_Monto + ", 2, ");
                    Mi_SQL.Append("'" + Datos.P_Nombre_Usuario + "', SYSDATE, '" + Cls_Sessiones.Empleado_ID + "', ");
                    Mi_SQL.Append("'" + Cls_Sessiones.Empleado_ID + "')");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();               //Ejecuta la inserción en memoria antes de pasarla a la base de datos  

                    //Da de alta los detalles de la póliza
                    foreach (DataRow Renglon in Datos.P_Dt_Detalles_Poliza.Rows)
                    {
                        Mi_SQL.Length = 0;
                        //consulta el saldo de la cuenta contable
                        Mi_SQL.Append("SELECT (NVL(SUM(" + Ope_Con_Polizas_Detalles.Campo_Debe + "),'0') - ");
                        Mi_SQL.Append(" NVL(SUM(" + Ope_Con_Polizas_Detalles.Campo_Haber + "),'0')) AS Saldo");
                        Mi_SQL.Append(" FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles);
                        Mi_SQL.Append(" WHERE " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString() + "'");
                        Comando_SQL.CommandText = Mi_SQL.ToString(); //Realiza la ejecución para obtener el Saldo
                        Saldo = Comando_SQL.ExecuteScalar();
                        if (Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Debe].ToString()) > 0)
                        {
                            Saldo = Convert.ToDouble(Saldo) + Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Debe].ToString());
                        }
                        if (Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Haber].ToString()) > 0)
                        {
                            Saldo = Convert.ToDouble(Saldo) - Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Haber].ToString());
                        }

                        Mi_SQL.Length = 0;
                        //Consulta para la obtención del último consecutivo dado de alta en la tabla de detalles de poliza
                        Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Con_Polizas_Detalles.Campo_Consecutivo + "),'0000000000')");
                        Mi_SQL.Append(" FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles);
                        Comando_SQL.CommandText = Mi_SQL.ToString(); //Realiza la ejecución de la obtención del consecutivo
                        Consecutivo = Comando_SQL.ExecuteScalar();

                        //Valida si el ID es nulo para asignarle automaticamente el primer registro
                        if (Convert.IsDBNull(Consecutivo))
                        {
                            Consecutivo = "1";
                        }
                        //Si no esta vacio el registro entonces al registro que se obtenga se le suma 1 para poder obtener el último registro
                        else
                        {
                            Consecutivo = Convert.ToInt32(Consecutivo) + 1;
                        }
                        Mi_SQL.Length = 0;
                        //Inserta el registro del detalle de la póliza en la base de datos
                        Mi_SQL.Append("INSERT INTO " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles);
                        Mi_SQL.Append("(" + Ope_Con_Polizas_Detalles.Campo_No_Poliza + ", " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID + ", ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Mes_Ano + ", " + Ope_Con_Polizas_Detalles.Campo_Partida + ", ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", " + Ope_Con_Polizas_Detalles.Campo_Concepto + ", ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Debe + ", " + Ope_Con_Polizas_Detalles.Campo_Haber + ", ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Saldo + ", " + Ope_Con_Polizas_Detalles.Campo_Fecha + ", ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Consecutivo + ")");
                        Mi_SQL.Append(" VALUES('" + No_Poliza + "', '00001', '" + Mes_Anio + "', ");
                        Mi_SQL.Append(Renglon[Ope_Con_Polizas_Detalles.Campo_Partida].ToString() + ",");
                        Mi_SQL.Append(" '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString() + "',");
                        Mi_SQL.Append(" '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Concepto].ToString() + "', ");
                        Mi_SQL.Append(Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Debe].ToString()) + ", ");
                        Mi_SQL.Append(Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Haber].ToString()) + ", ");
                        Mi_SQL.Append(Convert.ToDouble(Saldo) + ", SYSDATE, " + Consecutivo + ")");   
                        Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                        Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos            
                    }
                    Transaccion_SQL.Commit();             //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos                    

                    Cls_Ope_Psp_Manejo_Presupuesto.Actualizar_Momentos_Presupuestales(Convert.ToString(Datos.P_No_Reserva), "DEVENGADO", "COMPROMETIDO", Datos.P_Monto); //Actualiza el impote de la partida presupuestal
                    Cls_Ope_Psp_Manejo_Presupuesto.Registro_Movimiento_Presupuestal(Convert.ToString(Datos.P_No_Reserva), "DEVENGADO", "COMPROMETIDO", Datos.P_Monto, Convert.ToString(No_Poliza), "00001", Mes_Anio, "1"); //Agrega el historial del movimiento de la partida presupuestal
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
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Solicitud_Pago
            /// DESCRIPCION : Modifica los datos de la Solicitud de Pago con lo que fueron 
            ///               introducidos por el usuario
            /// PARAMETROS  :  Datos: Datos que son enviados de la capa de Negocios y que fueron 
            ///                       proporcionados por el usuario y van a sustituir a los datos que se
            ///                       encuentran en la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 17-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Modificar_Solicitud_Pago(Cls_Ope_Con_Solicitud_Pagos_Negocio Datos)
            {
                Double Total_Poliza = 0;                         //Obtiene el monto total del debe y haber de la póliza
                String Mes_Anio = String.Format("{0:MMyy}", DateTime.Today); //Obtiene el mes y año que se le asiganara a la póliza
                StringBuilder Mi_SQL = new StringBuilder();      //Obtiene los datos de la inserción a realizar a la base de datos
                Object No_Poliza = null;                         //Obtiene el No con la cual se guardo los datos en la base de datos
                Object Consecutivo = null;                       //Obtiene el consecutivo con la cual se guardo los datos en la base de datos
                Object Saldo;                                    //Obtiene el saldo de la cuenta contable                
                OracleCommand Comando_SQL = new OracleCommand(); //Sirve para la ejecución de las operaciones a la base de datos
                OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
                OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        

                try
                {
                    if (Conexion_Base.State != ConnectionState.Open)
                    {
                        Conexion_Base.Open(); //Abre la conexión a la base de datos            
                    }
                    Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
                    Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
                    Comando_SQL.Transaction = Transaccion_SQL;

                    Mi_SQL.Append("UPDATE " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos);
                    Mi_SQL.Append(" SET " + Ope_Con_Solicitud_Pagos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ");
                    if (!String.IsNullOrEmpty(Datos.P_No_Poliza)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_No_Poliza + " = '" + Datos.P_No_Poliza + "', ");
                    if (!String.IsNullOrEmpty(Datos.P_Tipo_Poliza_ID)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Tipo_Poliza_ID + " = '" + Datos.P_Tipo_Poliza_ID + "', ");
                    if (!String.IsNullOrEmpty(Datos.P_Mes_Ano)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Mes_Ano + " = '" + Datos.P_Mes_Ano + "', ");
                    if (!String.IsNullOrEmpty(Datos.P_Tipo_Solicitud_Pago_ID)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + " = '" + Datos.P_Tipo_Solicitud_Pago_ID + "', ");
                    if (!String.IsNullOrEmpty(Datos.P_Empleado_ID_Jefe_Area)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Empleado_ID_Jefe_Area + " = '" + Datos.P_Empleado_ID_Jefe_Area + "', ");
                    if (!String.IsNullOrEmpty(Datos.P_Empleado_ID_Contabilidad)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Empleado_ID_Contabilidad + " = '" + Datos.P_Empleado_ID_Contabilidad + "', ");
                    if (!String.IsNullOrEmpty(Datos.P_Concepto)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Concepto + " = '" + Datos.P_Concepto + "', ");
                    if(Datos.P_Monto > 0) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Monto + " = " + Datos.P_Monto + ", ");
                    if (!String.IsNullOrEmpty(Datos.P_No_Factura )) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_No_Factura + " = '" + Datos.P_No_Factura + "', ");
                    if (!String.IsNullOrEmpty(Datos.P_Fecha_Factura)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Fecha_Factura + " = TO_DATE('" + String.Format("{0:dd/MM/yy}", Datos.P_Fecha_Factura) + "','DD/MM/YY'), ");
                    if (!String.IsNullOrEmpty(Datos.P_Fecha_Autorizo_Rechazo_Jefe)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Fecha_Autorizo_Rechazo_Jefe + " = TO_DATE('" + String.Format("{0:dd/MM/yy}", Datos.P_Fecha_Autorizo_Rechazo_Jefe) + "','DD/MM/YY'), ");
                    if (!String.IsNullOrEmpty(Datos.P_Fecha_Autorizo_Rechazo_Contabi)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Fecha_Autorizo_Rechazo_Contabilidad + " = TO_DATE('" + String.Format("{0:dd/MM/yy}", Datos.P_Fecha_Autorizo_Rechazo_Contabi) + "','DD/MM/YY'), ");
                    if (!String.IsNullOrEmpty(Datos.P_Comentarios_Jefe_Area)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Comentarios_Jefe_Area + " = '" + Datos.P_Comentarios_Jefe_Area + "', ");
                    if (!String.IsNullOrEmpty(Datos.P_Comentarios_Contabilidad)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Comentarios_Contabilidad + " = '" + Datos.P_Comentarios_Contabilidad + "', ");
                    if (!String.IsNullOrEmpty(Datos.P_Proveedor_ID)) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "', ");
                    if (Datos.P_No_Reserva > 0) Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_No_Reserva + " = " + Datos.P_No_Reserva + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + " = '" + Datos.P_No_Solicitud_Pago + "'");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();               //Ejecuta la inserción en memoria antes de pasarla a la base de datos  

                    Mi_SQL.Length = 0;
                    //Actualiza el saldo de la reserva
                    Mi_SQL.Append("UPDATE " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas);
                    Mi_SQL.Append(" SET " + Ope_Psp_Reservas.Campo_Saldo + " = " + Ope_Psp_Reservas.Campo_Saldo + " + " + Datos.P_Monto_Anterior);
                    Mi_SQL.Append(" WHERE " + Ope_Psp_Reservas.Campo_No_Reserva + " = " + Convert.ToInt32(Datos.P_No_Reserva_Anterior));
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();               //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    Mi_SQL.Length = 0;

                    if (String.IsNullOrEmpty(Datos.P_Empleado_ID_Jefe_Area) && String.IsNullOrEmpty(Datos.P_Comentarios_Contabilidad))
                    {
                        //Actualiza el saldo de la reserva
                        Mi_SQL.Append("UPDATE " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas);
                        Mi_SQL.Append(" SET " + Ope_Psp_Reservas.Campo_Saldo + " = " + Ope_Psp_Reservas.Campo_Saldo + " - " + Datos.P_Monto + ", ");
                        Mi_SQL.Append(Ope_Psp_Reservas.Campo_Beneficiario + " = '" + Datos.P_Beneficiario + "'");
                        Mi_SQL.Append(" WHERE " + Ope_Psp_Reservas.Campo_No_Reserva + " = " + Convert.ToInt32(Datos.P_No_Reserva));
                        Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                        Comando_SQL.ExecuteNonQuery();               //Ejecuta la inserción en memoria antes de pasarla a la base de datos  
                    }
                    Mi_SQL.Length = 0;
                    //Consulta para la obtención del último ID dado de alta 
                    Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Con_Polizas.Campo_No_Poliza + "),'000000000')");
                    Mi_SQL.Append(" FROM " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas);
                    Mi_SQL.Append(" WHERE " + Ope_Con_Polizas.Campo_Mes_Ano + " = '" + Mes_Anio + "'");
                    Mi_SQL.Append(" AND " + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " = '00001'");
                    Mi_SQL.Append(" ORDER BY " + Ope_Con_Polizas.Campo_No_Poliza + " DESC");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Realiza la ejecuón de la obtención del ID del empleado
                    No_Poliza = Comando_SQL.ExecuteScalar();
                    //Valida si el ID es nulo para asignarle automaticamente el primer registro
                    if (Convert.IsDBNull(No_Poliza))
                    {
                        No_Poliza = "0000000001";
                    }
                    //Si no esta vacio el registro entonces al registro que se obtenga se le suma 1 para poder obtener el último registro
                    else
                    {
                        No_Poliza = String.Format("{0:0000000000}", Convert.ToInt32(No_Poliza) + 1);
                    }
                    Mi_SQL.Length = 0;
                    Total_Poliza += Datos.P_Monto_Anterior + Datos.P_Monto;
                    //Consulta para la inserción de la póliza con los datos proporcionados por el usuario
                    Mi_SQL.Append("INSERT INTO " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas);
                    Mi_SQL.Append(" (" + Ope_Con_Polizas.Campo_No_Poliza + ", " + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + ", ");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Mes_Ano + ", " + Ope_Con_Polizas.Campo_Fecha_Poliza + ", ");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Concepto + ", " + Ope_Con_Polizas.Campo_Total_Debe + ", ");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Total_Haber + ", " + Ope_Con_Polizas.Campo_No_Partidas + ", ");
                    Mi_SQL.Append(Cat_Empleados.Campo_Usuario_Creo + ", " + Cat_Empleados.Campo_Fecha_Creo + ", ");
                    Mi_SQL.Append(Ope_Con_Polizas.Campo_Empleado_ID_Creo + ", " + Ope_Con_Polizas.Campo_Empleado_ID_Autorizo + ")");
                    Mi_SQL.Append(" VALUES ('" + No_Poliza + "', '00001', '" + Mes_Anio + "',");
                    Mi_SQL.Append(" TO_DATE('" + String.Format("{0:dd/MM/yyyy}", DateTime.Today) + "','DD/MM/YYYY'),");
                    Mi_SQL.Append(" '" + Datos.P_Concepto + "', " + Total_Poliza + ", " + Total_Poliza + ", 2, ");
                    Mi_SQL.Append("'" + Datos.P_Nombre_Usuario + "', SYSDATE, '" + Cls_Sessiones.Empleado_ID + "', ");
                    Mi_SQL.Append("'" + Cls_Sessiones.Empleado_ID + "')");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();               //Ejecuta la inserción en memoria antes de pasarla a la base de datos  

                    //Da de alta los detalles de la póliza
                    foreach (DataRow Renglon in Datos.P_Dt_Detalles_Poliza.Rows)
                    {
                        Mi_SQL.Length = 0;
                        //consulta el saldo de la cuenta contable
                        Mi_SQL.Append("SELECT (NVL(SUM(" + Ope_Con_Polizas_Detalles.Campo_Debe + "),'0') - ");
                        Mi_SQL.Append(" NVL(SUM(" + Ope_Con_Polizas_Detalles.Campo_Haber + "),'0')) AS Saldo");
                        Mi_SQL.Append(" FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles);
                        Mi_SQL.Append(" WHERE " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString() + "'");
                        Comando_SQL.CommandText = Mi_SQL.ToString(); //Realiza la ejecución para obtener el Saldo
                        Saldo = Comando_SQL.ExecuteScalar();
                        if (Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Debe].ToString()) > 0)
                        {
                            Saldo = Convert.ToDouble(Saldo) + Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Debe].ToString());
                        }
                        if (Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Haber].ToString()) > 0)
                        {
                            Saldo = Convert.ToDouble(Saldo) - Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Haber].ToString());
                        }

                        Mi_SQL.Length = 0;
                        //Consulta para la obtención del último consecutivo dado de alta en la tabla de detalles de poliza
                        Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Con_Polizas_Detalles.Campo_Consecutivo + "),'0000000000')");
                        Mi_SQL.Append(" FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles);
                        Comando_SQL.CommandText = Mi_SQL.ToString(); //Realiza la ejecución de la obtención del consecutivo
                        Consecutivo = Comando_SQL.ExecuteScalar();

                        //Valida si el ID es nulo para asignarle automaticamente el primer registro
                        if (Convert.IsDBNull(Consecutivo))
                        {
                            Consecutivo = "1";
                        }
                        //Si no esta vacio el registro entonces al registro que se obtenga se le suma 1 para poder obtener el último registro
                        else
                        {
                            Consecutivo = Convert.ToInt32(Consecutivo) + 1;
                        }
                        Mi_SQL.Length = 0;
                        //Inserta el registro del detalle de la póliza en la base de datos
                        Mi_SQL.Append("INSERT INTO " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles);
                        Mi_SQL.Append("(" + Ope_Con_Polizas_Detalles.Campo_No_Poliza + ", " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID + ", ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Mes_Ano + ", " + Ope_Con_Polizas_Detalles.Campo_Partida + ", ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", " + Ope_Con_Polizas_Detalles.Campo_Concepto + ", ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Debe + ", " + Ope_Con_Polizas_Detalles.Campo_Haber + ", ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Saldo + ", " + Ope_Con_Polizas_Detalles.Campo_Fecha + ", ");
                        Mi_SQL.Append(Ope_Con_Polizas_Detalles.Campo_Consecutivo + ")");
                        Mi_SQL.Append(" VALUES('" + No_Poliza + "', '00001', '" + Mes_Anio + "', ");
                        Mi_SQL.Append(Renglon[Ope_Con_Polizas_Detalles.Campo_Partida].ToString() + ",");
                        Mi_SQL.Append(" '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString() + "',");
                        Mi_SQL.Append(" '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Concepto].ToString() + "', ");
                        Mi_SQL.Append(Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Debe].ToString()) + ", ");
                        Mi_SQL.Append(Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Haber].ToString()) + ", ");
                        Mi_SQL.Append(Convert.ToDouble(Saldo) + ", SYSDATE, " + Consecutivo + ")");
                        Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                        Comando_SQL.ExecuteNonQuery();               //Ejecuta la inserción en memoria antes de pasarla a la base de datos            
                    }
                    Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
                    if (Datos.P_Monto > 0)
                    {
                            Cls_Ope_Psp_Manejo_Presupuesto.Actualizar_Momentos_Presupuestales(Convert.ToString(Datos.P_No_Reserva), "DEVENGADO", "COMPROMETIDO", Datos.P_Monto); //Actualiza el impote de la partida presupuestal
                            Cls_Ope_Psp_Manejo_Presupuesto.Registro_Movimiento_Presupuestal(Convert.ToString(Datos.P_No_Reserva), "DEVENGADO", "COMPROMETIDO", Datos.P_Monto, Convert.ToString(No_Poliza), "00001", Mes_Anio, "1"); //Agrega el historial del movimiento de la partida presupuestal
                    }
                    if(Datos.P_Monto_Anterior > 0)
                    {

                        if (!String.IsNullOrEmpty(Datos.P_Comentarios_Jefe_Area) || !String.IsNullOrEmpty(Datos.P_Comentarios_Contabilidad))
                        {
                            Cls_Ope_Psp_Manejo_Presupuesto.Actualizar_Momentos_Presupuestales(Convert.ToString(Datos.P_No_Reserva_Anterior), "COMPROMETIDO", "DEVENGADO", Datos.P_Monto_Anterior); //Actualiza el impote de la partida presupuestal
                            Cls_Ope_Psp_Manejo_Presupuesto.Registro_Movimiento_Presupuestal(Convert.ToString(Datos.P_No_Reserva_Anterior), "COMPROMETIDO", "DEVENGADO", Datos.P_Monto_Anterior, Convert.ToString(No_Poliza), "00001", Mes_Anio, "1"); //Agrega el historial del movimiento de la partida presupuestal
                        }
                        else
                        {                            
                            Cls_Ope_Psp_Manejo_Presupuesto.Actualizar_Momentos_Presupuestales(Convert.ToString(Datos.P_No_Reserva_Anterior), "COMPROMETIDO", "DEVENGADO", Datos.P_Monto_Anterior); //Actualiza el impote de la partida presupuestal
                            Cls_Ope_Psp_Manejo_Presupuesto.Registro_Movimiento_Presupuestal(Convert.ToString(Datos.P_No_Reserva_Anterior), "COMPROMETIDO", "DEVENGADO", Datos.P_Monto_Anterior, Convert.ToString(No_Poliza), "00001", Mes_Anio, "3"); //Agrega el historial del movimiento de la partida presupuestal
                          }
                    }
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
            /// NOMBRE DE LA FUNCION: Eliminar_Solicitud_Pago
            /// DESCRIPCION : Elimina la Solicitud de pago que fue seleccionada por el usuario de la BD
            /// PARAMETROS  : Datos: Obtiene que Cuenta Contable desea eliminar de la BD
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 17-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Eliminar_Solicitud_Pago(Cls_Ope_Con_Solicitud_Pagos_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene los datos de la inserción a realizar a la base de datos
                OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
                OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
                OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        

                try
                {
                    if (Conexion_Base.State != ConnectionState.Open)
                    {
                        Conexion_Base.Open(); //Abre la conexión a la base de datos            
                    }
                    Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
                    Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
                    Comando_SQL.Transaction = Transaccion_SQL;

                    //Elimina el registro al cual pertenece la solicitud de pago que fue seleccionado por el usuario
                    Mi_SQL.Append("DELETE FROM " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos);
                    Mi_SQL.Append(" WHERE " + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + " = '" + Datos.P_No_Solicitud_Pago + "'");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();               //Ejecuta la inserción en memoria antes de pasarla a la base de datos  

                    Mi_SQL.Length = 0;
                    //Actualiza el saldo de la reserva
                    Mi_SQL.Append("UPDATE " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas);
                    Mi_SQL.Append(" SET " + Ope_Psp_Reservas.Campo_Saldo + " = " + Ope_Psp_Reservas.Campo_Saldo + " + " + Datos.P_Monto);
                    Mi_SQL.Append(" WHERE " + Ope_Psp_Reservas.Campo_No_Reserva + " = " + Convert.ToInt32(Datos.P_No_Reserva));
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();               //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Transaccion_SQL.Commit();                    //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
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
        #endregion
        #region(Consultas)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consultar_Solicitud_Pago
            /// DESCRIPCION : Consulta todos los datos que se tienen registrados en la base
            ///               de datos ya sea de acuerdo a los parametros proporcionado pos el
            ///               usuario o todos
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 17-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consultar_Solicitud_Pago(Cls_Ope_Con_Solicitud_Pagos_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
                try
                {   
                    Mi_SQL.Append("SELECT " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Estatus + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_No_Factura + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Fecha_Factura + ", ");
                    Mi_SQL.Append(Cat_Con_Tipo_Solicitud_Pagos.Tabla_Cat_Con_Tipo_Solicitud_Pago + "." + Cat_Con_Tipo_Solicitud_Pagos.Campo_Descripcion + " AS Solicitud, ");
                    Mi_SQL.Append(Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Beneficiario);
                    Mi_SQL.Append(" FROM " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + ", ");
                    Mi_SQL.Append(Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + ", " + Cat_Con_Tipo_Solicitud_Pagos.Tabla_Cat_Con_Tipo_Solicitud_Pago);
                    Mi_SQL.Append(" WHERE " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_No_Reserva);
                    Mi_SQL.Append(" = " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_No_Reserva);
                    Mi_SQL.Append(" AND " + Cat_Con_Tipo_Solicitud_Pagos.Tabla_Cat_Con_Tipo_Solicitud_Pago + "." + Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID);
                    Mi_SQL.Append(" = " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID);
                    Mi_SQL.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Dependencia_ID);
                    Mi_SQL.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                    if (!String.IsNullOrEmpty(Datos.P_No_Solicitud_Pago)) Mi_SQL.Append(" AND " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + " = '" + Datos.P_No_Solicitud_Pago + "'");
                    if (!String.IsNullOrEmpty(Datos.P_Estatus)) Mi_SQL.Append(" AND " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Estatus + " = '" + Datos.P_Estatus + "'");
                    if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID)) Mi_SQL.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'");
                    if (!String.IsNullOrEmpty(Datos.P_Tipo_Solicitud_Pago_ID)) Mi_SQL.Append(" AND " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + " = '" + Datos.P_Tipo_Solicitud_Pago_ID + "'");
                    if (Datos.P_No_Reserva > 0) Mi_SQL.Append(" AND " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_No_Reserva + " = " + Datos.P_No_Reserva);
                    if (!String.IsNullOrEmpty(Datos.P_Fecha_Inicial) && !String.IsNullOrEmpty(Datos.P_Fecha_Final))
                    {
                        Mi_SQL.Append(" AND " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Fecha_Solicitud);
                        Mi_SQL.Append(" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicial + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                        Mi_SQL.Append(" AND TO_DATE ('" + Datos.P_Fecha_Final + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");
                    }
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
            /// NOMBRE DE LA FUNCION: Consulta_Reservas
            /// DESCRIPCION : Consulta todas las reservas que tengan un saldo mayor a 0
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 18-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Reservas(Cls_Ope_Con_Solicitud_Pagos_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
                try
                {
                    Mi_SQL.Append("SELECT " + Ope_Psp_Reservas.Campo_No_Reserva + ",");
                    Mi_SQL.Append(" (" + Ope_Psp_Reservas.Campo_No_Reserva + "||'-'||" + Ope_Psp_Reservas.Campo_Concepto + ") AS Reserva");
                    Mi_SQL.Append(" FROM " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas);
                    Mi_SQL.Append(" WHERE " + Ope_Psp_Reservas.Campo_Saldo + " > 0");
                    if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID)) Mi_SQL.Append(" AND " + Ope_Psp_Reservas.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'");
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
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Reserva
            /// DESCRIPCION : Consulta todos los datos de la reserva que fue seleccionada por
            ///               el usuario
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 18-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Datos_Reserva(Cls_Ope_Con_Solicitud_Pagos_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
                try
                {
                    Mi_SQL.Append("SELECT " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Concepto + " AS Reservado, ");
                    Mi_SQL.Append(Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Beneficiario + ", ");
                    Mi_SQL.Append(Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Saldo + ", ");
                    Mi_SQL.Append("(" + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave);
                    Mi_SQL.Append("||'-'||" + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Clave);
                    Mi_SQL.Append("||'-'||" + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Clave);
                    Mi_SQL.Append("||'-'||" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave);
                    Mi_SQL.Append("||'-'||" + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + ") AS Codigo_Programatico, ");
                    Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS Fuente_Financiamiento, ");
                    Mi_SQL.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Descripcion + " AS Area_Funcional, ");
                    Mi_SQL.Append(Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Descripcion + " AS Proyectos_Programas, ");
                    Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS Dependencia, ");
                    Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Descripcion + " AS Partida, ");
                    Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Cuenta_Contable_ID);
                    Mi_SQL.Append(" FROM " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + ", " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + ", ");
                    Mi_SQL.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + ", " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + ", ");
                    Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + ", " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                    Mi_SQL.Append(" WHERE " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Dependencia_ID);
                    Mi_SQL.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                    Mi_SQL.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Fte_Financimiento_ID);
                    Mi_SQL.Append(" = " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                    Mi_SQL.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Proyecto_Programa_ID);
                    Mi_SQL.Append(" = " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas + "." + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id);
                    Mi_SQL.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Partida_ID);
                    Mi_SQL.Append(" = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                    Mi_SQL.Append(" AND " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Area_Funcional_ID);
                    Mi_SQL.Append(" = " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID);
                    Mi_SQL.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_No_Reserva + " = " + Convert.ToInt32(Datos.P_No_Reserva));
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
            /// NOMBRE DE LA FUNCION: Consulta_Datos_Solicitud_Pago
            /// DESCRIPCION : Consulta todos los datos de la solicitud de pago que selecciono
            ///               el usuario
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 19-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Datos_Solicitud_Pago(Cls_Ope_Con_Solicitud_Pagos_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
                try
                {
                    Mi_SQL.Append("SELECT " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + ", ");
                    Mi_SQL.Append(Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Cuenta_Contable_ID + ", ");
                    Mi_SQL.Append(Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + ", ");
                    Mi_SQL.Append(Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Estatus + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Fecha_Solicitud + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_No_Factura + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Fecha_Factura + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Monto + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Concepto + ", ");
                    Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_No_Reserva + ", ");
                    Mi_SQL.Append(" (" + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_No_Reserva);
                    Mi_SQL.Append("||'-'||" + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Concepto + ") AS Reserva, ");
                    Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Cuenta_Contable_ID + " AS Cuenta_Contable_Reserva");
                    Mi_SQL.Append(" FROM " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ", ");
                    Mi_SQL.Append(Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + ", " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                    Mi_SQL.Append(" WHERE " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + " = '" + Datos.P_No_Solicitud_Pago + "'");
                    Mi_SQL.Append(" AND " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_Proveedor_ID + " = ");
                    Mi_SQL.Append(Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID);
                    Mi_SQL.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_No_Reserva);
                    Mi_SQL.Append(" = " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + "." + Ope_Con_Solicitud_Pagos.Campo_No_Reserva);
                    Mi_SQL.Append(" AND " + Ope_Psp_Reservas.Tabla_Ope_Psp_Reservas + "." + Ope_Psp_Reservas.Campo_Partida_ID);
                    Mi_SQL.Append(" = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
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
            /// NOMBRE DE LA FUNCION: Consulta_Cuenta_Contable_Proveedor
            /// DESCRIPCION : Consulta si el proveedor proporcionado tiene una cuenta contable
            ///               asiganada
            /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 23-Noviembre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Cuenta_Contable_Proveedor(Cls_Ope_Con_Solicitud_Pagos_Negocio Datos)
            {
                StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
                try
                {
                    Mi_SQL.Append("SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID + ", " + Cat_Com_Proveedores.Campo_Nombre + ", ");
                    Mi_SQL.Append(Cat_Com_Proveedores.Campo_Cuenta_Contable_ID);
                    Mi_SQL.Append(" FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores);
                    Mi_SQL.Append(" WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "'");
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
        #endregion
    }
}