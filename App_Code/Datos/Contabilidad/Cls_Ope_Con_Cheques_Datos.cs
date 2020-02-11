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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Cheque.Negocio;
using Presidencia.Sessiones;
using Presidencia.Manejo_Presupuesto.Datos;

/// <summary>
/// Summary description for Cls_Ope_Con_Autoriza_Solicitud_Pago_Datos
/// </summary>

namespace Presidencia.Cheque.Datos
{
    public class Cls_Ope_Con_Cheques_Datos
    {
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Solicitudes_Autorizadas
        /// DESCRIPCION : Consulta las solicitudes de pago que estan Autorizadas o pagadas
        /// PARAMETROS  : Datos: Recibe los datos proporcionados por el usuario.
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 15/Noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Solicitudes_Autorizadas(Cls_Ope_Con_Cheques_Negocio Datos)
        {
            String Mi_SQL;  //Almacenara la Query de Consulta.
            try
            {
                //Consulta los movimientos de las cuentas contables.
                Mi_SQL = "SELECT Solicitud." + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + ", Solicitud." + Ope_Con_Solicitud_Pagos.Campo_No_Reserva ;
                Mi_SQL += ", Solicitud."+ Ope_Con_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID +", Solicitud."+ Ope_Con_Solicitud_Pagos.Campo_Concepto ;
                Mi_SQL += ", Solicitud." + Ope_Con_Solicitud_Pagos.Campo_Estatus  + ", Solicitud." + Ope_Con_Solicitud_Pagos.Campo_Fecha_Solicitud ;
                Mi_SQL += ", Solicitud.MES_ANO, Solicitud." + Ope_Con_Solicitud_Pagos.Campo_No_Poliza;
                Mi_SQL += ", Solicitud." + Ope_Con_Solicitud_Pagos.Campo_No_Factura + ", Solicitud." + Ope_Con_Solicitud_Pagos.Campo_Fecha_Factura;
                Mi_SQL += ", Solicitud." + Ope_Con_Solicitud_Pagos.Campo_Monto + ", Tipo." + Cat_Con_Tipo_Solicitud_Pagos.Campo_Descripcion + " as Tipo_Pago FROM ";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos+" Solicitud, "+ Cat_Con_Tipo_Solicitud_Pagos.Tabla_Cat_Con_Tipo_Solicitud_Pago +" Tipo ";
                Mi_SQL += " WHERE Solicitud." + Ope_Con_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID +"= Tipo."+Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID;
                if (Datos.P_Estatus == "" || Datos.P_Estatus == null)
                {
                    Mi_SQL += " AND (solicitud." + Ope_Con_Solicitud_Pagos.Campo_Estatus + " = 'PORPAGAR')";
                }
                else
                {
                    Mi_SQL += " AND (solicitud." + Ope_Con_Solicitud_Pagos.Campo_Estatus + " = 'PORPAGAR' OR solicitud." + Ope_Con_Solicitud_Pagos.Campo_Estatus + " = 'PAGADO')";
                
                }
                if (!string.IsNullOrEmpty(Datos.P_No_Solicitud_Pago) && Datos.P_No_Solicitud_Pago != "0")
                {
                Mi_SQL += " AND " + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago +
                " = '" + Datos.P_No_Solicitud_Pago+ "'";
                Mi_SQL += " AND TO_DATE(TO_CHAR(" + Ope_Con_Solicitud_Pagos.Campo_Fecha_Solicitud + ",'DD-MM-YYYY'))" +
                        " >= '" + Datos.P_Fecha_Inicio + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Con_Solicitud_Pagos.Campo_Fecha_Solicitud + ",'DD-MM-YYYY'))" +
                        " <= '" + Datos.P_Fecha_Final + "'";
              
               }
                if (!string.IsNullOrEmpty(Datos.P_Tipo_Solicitud_Pago_ID))
                {
                    Mi_SQL +=
                    " AND Tipo." + Ope_Con_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID +
                    " = '" + Datos.P_Tipo_Solicitud_Pago_ID+"'";
                    if (string.IsNullOrEmpty(Datos.P_No_Solicitud_Pago) || Datos.P_No_Solicitud_Pago == "0" )
                    {
                        Mi_SQL += " AND TO_DATE(TO_CHAR(" + Ope_Con_Solicitud_Pagos.Campo_Fecha_Solicitud + ",'DD-MM-YYYY'))" +
                            " >= '" + Datos.P_Fecha_Inicio + "' AND " +
                    "TO_DATE(TO_CHAR(" + Ope_Con_Solicitud_Pagos.Campo_Fecha_Solicitud + ",'DD-MM-YYYY'))" +
                            " <= '" + Datos.P_Fecha_Final + "'";
                    }
                }
                if ((string.IsNullOrEmpty(Datos.P_No_Solicitud_Pago) || Datos.P_No_Solicitud_Pago == "0") && string.IsNullOrEmpty(Datos.P_Tipo_Solicitud_Pago_ID))
                {
                    Mi_SQL += " AND TO_DATE(TO_CHAR(" + Ope_Con_Solicitud_Pagos.Campo_Fecha_Solicitud + ",'DD-MM-YYYY'))" +
                        " >= '" + Datos.P_Fecha_Inicio + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Con_Solicitud_Pagos.Campo_Fecha_Solicitud + ",'DD-MM-YYYY'))" +
                        " <= '" + Datos.P_Fecha_Final + "'";
                }
                Mi_SQL += " ORDER BY " + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago  + " ASC";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Tipos_Solicitudes
        /// DESCRIPCION : Consulta los tipos de solicitudes de pago que existen
        /// PARAMETROS  : Datos: Recibe los datos proporcionados por el usuario.
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 15/Noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Tipos_Solicitudes(Cls_Ope_Con_Cheques_Negocio Datos)
        {
            String Mi_SQL;  //Almacenara la Query de Consulta.
            try
            {
                //Consulta los movimientos de las cuentas contables.
                Mi_SQL = "SELECT " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + ", " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Descripcion + " FROM  " + Cat_Con_Tipo_Solicitud_Pagos.Tabla_Cat_Con_Tipo_Solicitud_Pago;
                Mi_SQL += " ORDER BY " + Cat_Con_Tipo_Solicitud_Pagos.Campo_Tipo_Solicitud_Pago_ID + " ASC";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Pago
        /// DESCRIPCION : Consulta los tipos de solicitudes de pago que existen
        /// PARAMETROS  : Datos: Recibe los datos proporcionados por el usuario.
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 15/Noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Pago(Cls_Ope_Con_Cheques_Negocio Datos)
        {
            String Mi_SQL;  //Almacenara la Query de Consulta.
            try
            {
                //Consulta los movimientos de las cuentas contables.
                Mi_SQL = "SELECT * FROM  " + Ope_Con_Pagos.Tabla_Ope_Con_Pagos;
                Mi_SQL += " WHERE "+ Ope_Con_Pagos.Campo_No_Solicitud_Pago+"='"+Datos.P_No_Solicitud_Pago +"'";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Pago
        /// DESCRIPCION : Consulta todos los datos de la solicitud de pago que selecciono
        ///               el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 25/Noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Pago(Cls_Ope_Con_Cheques_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
            try
            {
                Mi_SQL.Append("SELECT " + Ope_Con_Pagos.Tabla_Ope_Con_Pagos + "." + Ope_Con_Pagos.Campo_No_Pago+ ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Cuenta_Contable_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos  + "." + Cat_Nom_Bancos.Campo_Banco_ID  + ", ");
                Mi_SQL.Append(Ope_Con_Pagos.Tabla_Ope_Con_Pagos + "." + Ope_Con_Pagos.Campo_Estatus + ", ");
                Mi_SQL.Append(Ope_Con_Pagos.Tabla_Ope_Con_Pagos + "." + Ope_Con_Pagos.Campo_Fecha_Pago  + ", ");
                Mi_SQL.Append(Ope_Con_Pagos.Tabla_Ope_Con_Pagos + "." + Ope_Con_Pagos.Campo_Beneficiario_Pago + ", ");
                Mi_SQL.Append(Ope_Con_Pagos.Tabla_Ope_Con_Pagos + "." + Ope_Con_Pagos.Campo_Motivo_Cancelacion + ", ");
                Mi_SQL.Append(Ope_Con_Pagos.Tabla_Ope_Con_Pagos + "." + Ope_Con_Pagos.Campo_No_Cheque + ", ");
                Mi_SQL.Append(Ope_Con_Pagos.Tabla_Ope_Con_Pagos + "." + Ope_Con_Pagos.Campo_Forma_Pago + ", ");
                Mi_SQL.Append(Ope_Con_Pagos.Tabla_Ope_Con_Pagos + "." + Ope_Con_Pagos.Campo_Referencia_Transferencia_Banca + ", ");
                Mi_SQL.Append(Ope_Con_Pagos.Tabla_Ope_Con_Pagos + "." + Ope_Con_Pagos.Campo_Comentarios );
                Mi_SQL.Append(" FROM " + Ope_Con_Pagos.Tabla_Ope_Con_Pagos + ", " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos);
                Mi_SQL.Append(" WHERE " + Ope_Con_Pagos.Tabla_Ope_Con_Pagos + "." + Ope_Con_Pagos.Campo_No_Solicitud_Pago  + " = '" + Datos.P_No_Solicitud_Pago  + "'");
                Mi_SQL.Append(" AND " + Ope_Con_Pagos.Tabla_Ope_Con_Pagos + "." + Ope_Con_Pagos.Campo_Estatus  + " ='PAGADO'");
                Mi_SQL.Append(" AND " + Ope_Con_Pagos.Tabla_Ope_Con_Pagos + "." + Ope_Con_Pagos.Campo_Banco_ID + " = ");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID);
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
        /// NOMBRE DE LA FUNCION: Consulta_Bancos
        /// DESCRIPCION : Consulta los tipos de solicitudes de pago que existen
        /// PARAMETROS  : Datos: Recibe los datos proporcionados por el usuario.
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 15/Noviembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Bancos(Cls_Ope_Con_Cheques_Negocio Datos)
        {
            String Mi_SQL;  //Almacenara la Query de Consulta.
            try
            {
                //Consulta los movimientos de las cuentas contables.
                Mi_SQL = "SELECT " + Cat_Nom_Bancos.Campo_Banco_ID + ", " + Cat_Nom_Bancos.Campo_Nombre + " FROM  " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos ;
                Mi_SQL += " ORDER BY " + Cat_Nom_Bancos.Campo_Banco_ID + " ASC";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Cuenta_Contable_Banco
        /// DESCRIPCION : Consulta si el banco proporcionado tiene una cuenta contable
        ///               asiganada
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 23-Noviembre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Cuenta_Contable_Banco(Cls_Ope_Con_Cheques_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la consulta a realizar a la base de datos
            try
            {
                Mi_SQL.Append("SELECT " + Cat_Nom_Bancos.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(" FROM " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos );
                Mi_SQL.Append(" WHERE " + Cat_Nom_Bancos.Campo_Banco_ID + " = '" + Datos.P_Banco_ID  + "'");
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
        /// NOMBRE DE LA FUNCION: Alta_Cheque
        /// DESCRIPCION : Inserta en nuevo Cheque en la BD
        /// PARAMETROS  : Datos: Contiene los datos de los filtros
        /// CREO        : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO  : 21/Noviembre/2011
        /// MODIFICO          : 
        /// FECHA_MODIFICO    : 
        /// CAUSA_MODIFICACION: 
        ///*******************************************************************************
        public static void Alta_Cheque(Cls_Ope_Con_Cheques_Negocio  Datos)
        {
            try
            {
                String Mi_SQL;
                Object Compromisos_ID; //Variable que contendrá el ID de la consulta
                Object No_Poliza = null;                         //Obtiene el No con la cual se guardo los datos en la base de datos
                Object Consecutivo = null;                       //Obtiene el consecutivo con la cual se guardo los datos en la base de datos
                Object Saldo;                                    //Obtiene el saldo de la cuenta contable                
                String Mes_Anio = String.Format("{0:MMyy}", DateTime.Today); //Obtiene el mes y año que se le asiganara a la póliza

                //Busca el maximo ID de la tabla Compromisos
                Mi_SQL = "SELECT NVL(MAX (" + Ope_Con_Pagos.Campo_No_Pago + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Con_Pagos.Tabla_Ope_Con_Pagos;
                Compromisos_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Compromisos_ID)) //Si no existen valores en la tabla, asigna el primer valor manualmente.
                {
                    Datos.P_No_Pago = "00001";
                }
                else // Si ya existen registros, toma el valor maximo y le suma 1 para el nuevo registro.
                {
                    Datos.P_No_Pago = String.Format("{0:00000}", Convert.ToInt32(Compromisos_ID) + 1);
                }

                Mi_SQL = "INSERT INTO " + Ope_Con_Pagos.Tabla_Ope_Con_Pagos  + "(";
                Mi_SQL += Ope_Con_Pagos.Campo_No_Pago  + ", ";
                Mi_SQL += Ope_Con_Pagos.Campo_No_Solicitud_Pago + ", ";
                Mi_SQL += Ope_Con_Pagos.Campo_No_poliza + ", ";
                Mi_SQL += Ope_Con_Pagos.Campo_Tipo_Poliza_ID  + ", ";
                Mi_SQL += Ope_Con_Pagos.Campo_Mes_Ano + ", ";
                Mi_SQL += Ope_Con_Pagos.Campo_Banco_ID  + ", ";
                Mi_SQL += Ope_Con_Pagos.Campo_Fecha_Pago + ", ";
                Mi_SQL += Ope_Con_Pagos.Campo_Beneficiario_Pago + ", ";
                Mi_SQL += Ope_Con_Pagos.Campo_Forma_Pago + ", ";
                if (Datos.P_Tipo_Pago == "CHEQUE")
                {
                    Mi_SQL += Ope_Con_Pagos.Campo_No_Cheque + ", ";
                }
                else
                {
                    Mi_SQL += Ope_Con_Pagos.Campo_Referencia_Transferencia_Banca + ", ";
                }           
                Mi_SQL += Ope_Con_Pagos.Campo_Estatus + ", ";
                Mi_SQL += Ope_Con_Pagos.Campo_Comentarios + ", ";
                Mi_SQL += Ope_Con_Pagos.Campo_Usuario_Creo + ", ";
                Mi_SQL += Ope_Con_Pagos.Campo_Fecha_Creo + ") VALUES('";
                Mi_SQL += Datos.P_No_Pago + "', '";
                Mi_SQL += Datos.P_No_Solicitud_Pago + "', '";
                Mi_SQL += Datos.P_No_Poliza + "', '";
                Mi_SQL += Datos.P_Tipo_Poliza_ID +"', '";
                Mi_SQL += Datos.P_Mes_Ano + "', '";
                Mi_SQL += Datos.P_Banco_ID + "','";
                Mi_SQL += Datos.P_Fecha_Pago + "','";
                Mi_SQL += Datos.P_Beneficiario_Pago + "', '";
                Mi_SQL += Datos.P_Tipo_Pago + "', '";
                if (Datos.P_Tipo_Pago == "CHEQUE")
                {
                    Mi_SQL += Datos.P_No_Cheque + "', '";
                }
                else
                {
                    Mi_SQL += Datos.P_Referencia + "', '";
                }
                Mi_SQL += Datos.P_Estatus + "', '";
                Mi_SQL += Datos.P_Comentario + "', '";
                Mi_SQL += Datos.P_Usuario_Creo + "',  SYSDATE )";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                Mi_SQL = "UPDATE " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + " SET ";
                //Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Empleado_ID_Contabilidad  + "='" + Datos.P_Empleado_ID_Contabilidad + "',";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Estatus + "='" + Datos.P_Estatus + "',";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Creo + "',";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Fecha_Modifico + "= SYSDATE " + " WHERE ";
                Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + " ='" + Datos.P_No_Solicitud_Pago + "'";
                OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Consulta para la obtención del último ID dado de alta 
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Con_Polizas.Campo_No_Poliza + "),'000000000')" +
                " FROM " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas +
                " WHERE " + Ope_Con_Polizas.Campo_Mes_Ano + " = '" + Mes_Anio + "'" +
                " AND " + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " = '00001'" +
                " ORDER BY " + Ope_Con_Polizas.Campo_No_Poliza + " DESC";
                No_Poliza = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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

                //Consulta para la inserción de la póliza con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas +
                " (" + Ope_Con_Polizas.Campo_No_Poliza + ", " + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + ", " +
                Ope_Con_Polizas.Campo_Mes_Ano + ", " + Ope_Con_Polizas.Campo_Fecha_Poliza + ", " +
                Ope_Con_Polizas.Campo_Concepto + ", " + Ope_Con_Polizas.Campo_Total_Debe + ", " +
                Ope_Con_Polizas.Campo_Total_Haber + ", " + Ope_Con_Polizas.Campo_No_Partidas + ", " +
                Cat_Empleados.Campo_Usuario_Creo + ", " + Cat_Empleados.Campo_Fecha_Creo + ", " +
                Ope_Con_Polizas.Campo_Empleado_ID_Creo + ", " + Ope_Con_Polizas.Campo_Empleado_ID_Autorizo + ")" +
                " VALUES ('" + No_Poliza + "', '00001', '" + Mes_Anio + "'," +
                " TO_DATE('" + String.Format("{0:dd/MM/yyyy}", DateTime.Today) + "','DD/MM/YYYY')," +
                " '" + "CHEQUE " + Datos.P_No_Pago + "', " + Datos.P_Monto + ", " + Datos.P_Monto + ", 2, " +
                "'" + Datos.P_Usuario_Creo + "', SYSDATE, '" + Cls_Sessiones.Empleado_ID + "', " +
                "'" + Cls_Sessiones.Empleado_ID + "')";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL); 

                //Da de alta los detalles de la póliza
                foreach (DataRow Renglon in Datos.P_Dt_Detalles_Poliza.Rows)
                {
                    //consulta el saldo de la cuenta contable
                    Mi_SQL = "SELECT (NVL(SUM(" + Ope_Con_Polizas_Detalles.Campo_Debe + "),'0') - " +
                    " NVL(SUM(" + Ope_Con_Polizas_Detalles.Campo_Haber + "),'0')) AS Saldo" +
                    " FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles +
                    " WHERE " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " = '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString() + "'";
                    Saldo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Debe].ToString()) > 0)
                    {
                        Saldo = Convert.ToDouble(Saldo) + Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Debe].ToString());
                    }
                    if (Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Haber].ToString()) > 0)
                    {
                        Saldo = Convert.ToDouble(Saldo) - Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Haber].ToString());
                    }

                    //Consulta para la obtención del último consecutivo dado de alta en la tabla de detalles de poliza
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Con_Polizas_Detalles.Campo_Consecutivo + "),'0000000000')" +
                    " FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                    Consecutivo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

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

                    //Inserta el registro del detalle de la póliza en la base de datos
                    Mi_SQL = "INSERT INTO " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles +
                    "(" + Ope_Con_Polizas_Detalles.Campo_No_Poliza + ", " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID + ", " +
                    Ope_Con_Polizas_Detalles.Campo_Mes_Ano + ", " + Ope_Con_Polizas_Detalles.Campo_Partida + ", " +
                    Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", " + Ope_Con_Polizas_Detalles.Campo_Concepto + ", " +
                    Ope_Con_Polizas_Detalles.Campo_Debe + ", " + Ope_Con_Polizas_Detalles.Campo_Haber + ", " +
                    Ope_Con_Polizas_Detalles.Campo_Saldo + ", " + Ope_Con_Polizas_Detalles.Campo_Fecha + ", " +
                    Ope_Con_Polizas_Detalles.Campo_Consecutivo + ")" +
                    " VALUES('" + No_Poliza + "', '00001', '" + Mes_Anio + "', " +
                    Renglon[Ope_Con_Polizas_Detalles.Campo_Partida].ToString() + "," +
                    " '" + Renglon[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID].ToString() + "'," +
                    " '" + "CHEQUE " + Datos.P_No_Pago + "', " +
                    Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Debe].ToString()) + ", " +
                    Convert.ToDouble(Renglon[Ope_Con_Polizas_Detalles.Campo_Haber].ToString()) + ", " +
                    Convert.ToDouble(Saldo) + ", SYSDATE, " + Consecutivo + ")";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);      
                }
                Cls_Ope_Psp_Manejo_Presupuesto.Actualizar_Momentos_Presupuestales(Convert.ToString(Datos.P_No_Reserva), "PAGADO", "EJERCIDO", Convert.ToDouble(Datos.P_Monto)); //Actualiza el impote de la partida presupuestal
                Cls_Ope_Psp_Manejo_Presupuesto.Registro_Movimiento_Presupuestal(Convert.ToString(Datos.P_No_Reserva), "PAGADO", "EJERCIDO", Convert.ToDouble(Datos.P_Monto), Convert.ToString(No_Poliza), "00001", Mes_Anio, "1"); //Agrega el historial del movimiento de la partida presupuestal
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.Message, ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Pago
        /// DESCRIPCION : Modifica los datos del  Pago con lo que fueron 
        ///               introducidos por el usuario
        /// PARAMETROS  :  Datos: Datos que son enviados de la capa de Negocios y que fueron 
        ///                       proporcionados por el usuario y van a sustituir a los datos que se
        ///                       encuentran en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 17-Noviembre-2011
        /// MODIFICO          :sergio Manuel Gallardo Andrade
        /// FECHA_MODIFICO    :25/nov/2011
        /// CAUSA_MODIFICACION:se adacto el metodo para la cancelacion del pago 
        ///*******************************************************************************
        public static void Modificar_Pago(Cls_Ope_Con_Cheques_Negocio Datos)
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

                Mi_SQL.Append("UPDATE " + Ope_Con_Pagos.Tabla_Ope_Con_Pagos);
                Mi_SQL.Append(" SET " + Ope_Con_Pagos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ");
                if (!String.IsNullOrEmpty(Datos.P_Motivo_Cancelacion)) Mi_SQL.Append(Ope_Con_Pagos.Campo_Motivo_Cancelacion + " = '" + Datos.P_Motivo_Cancelacion  + "', ");
                if (!String.IsNullOrEmpty(Datos.P_Comentario)) Mi_SQL.Append(Ope_Con_Pagos.Campo_Comentarios + " = '" + Datos.P_Comentario + "', ");
                Mi_SQL.Append(Ope_Con_Pagos.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Modifico  + "', ");
                Mi_SQL.Append(Ope_Con_Pagos.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Con_Pagos.Campo_No_Pago + " = '" + Datos.P_No_Pago + "'");
                Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();               //Ejecuta la inserción en memoria antes de pasarla a la base de datos  

                Mi_SQL.Length = 0;
                Mi_SQL.Append("UPDATE " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos);
                Mi_SQL.Append(" SET " + Ope_Con_Solicitud_Pagos.Campo_Estatus + " = 'PORPAGAR', ");
                Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Modifico  + "', ");
                Mi_SQL.Append(Ope_Con_Solicitud_Pagos.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + " = '" + Datos.P_No_Solicitud_Pago + "'");
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
                Total_Poliza +=Convert.ToDouble(Datos.P_Monto);
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
                Mi_SQL.Append(" '" + Datos.P_Comentario  + "', " + Total_Poliza + ", " + Total_Poliza + ", 2, ");
                Mi_SQL.Append("'" + Datos.P_Usuario_Modifico + "', SYSDATE, '" + Cls_Sessiones.Empleado_ID + "', ");
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
                if (Convert.ToDouble(Datos.P_Monto) > 0)
                {
                    Cls_Ope_Psp_Manejo_Presupuesto.Actualizar_Momentos_Presupuestales(Convert.ToString(Datos.P_No_Reserva), "EJERCIDO", "PAGADO", Convert.ToDouble(Datos.P_Monto)); //Actualiza el impote de la partida presupuestal
                    Cls_Ope_Psp_Manejo_Presupuesto.Registro_Movimiento_Presupuestal(Convert.ToString(Datos.P_No_Reserva), "EJERCIDO", "PAGADO", Convert.ToDouble(Datos.P_Monto), Convert.ToString(No_Poliza), "00001", Mes_Anio, "1"); //Agrega el historial del movimiento de la partida presupuestal
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
        /////*******************************************************************************
        ///// NOMBRE DE LA FUNCION: Modificar_Cheque
        ///// DESCRIPCION : Modifica el Compromiso seleccionado
        ///// PARAMETROS  : Datos: Contiene los datos proporcionados
        ///// CREO        : Sergio Manuel Gallardo Andrade
        ///// FECHA_CREO  : 22/Noviembre/2011
        ///// MODIFICO          : 
        ///// FECHA_MODIFICO    : 
        ///// CAUSA_MODIFICACION: 
        /////*******************************************************************************
        //public static void Modificar_Cheque(Cls_Ope_Con_Cheques_Negocio  Datos)
        //{
        //    try
        //    {
        //        String Mi_SQL;  //Almacena la sentencia de modificacion.

        //        Mi_SQL = "UPDATE " + Ope_Con_Pagos.Tabla_Ope_Con_Pagos  + " SET ";
        //        Mi_SQL += Ope_Con_Pagos.Campo_Banco_ID + " = '" + Datos.P_Banco_ID + "', ";
        //        Mi_SQL += Ope_Con_Pagos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
        //        if (Datos.P_Estatus == "CANCELADO") {
        //            Mi_SQL += Ope_Con_Pagos.Campo_Motivo_Cancelacion  + " = '" + Datos.P_Motivo_Cancelacion  + "', ";
        //        }
        //        Mi_SQL += Ope_Con_Pagos.Campo_Beneficiario_Pago  + " = '" + Datos.P_Beneficiario_Pago + "', ";
        //        Mi_SQL += Ope_Con_Pagos.Campo_Comentarios + " = '" + Datos.P_Comentario + "', ";
        //        Mi_SQL += Ope_Con_Pagos.Campo_Forma_Pago + " = '" + Datos.P_Tipo_Pago  + "', ";
        //        if (Datos.P_Tipo_Pago == "CHEQUE")
        //        {
        //            Mi_SQL += Ope_Con_Pagos.Campo_No_Cheque  + " = '" + Datos.P_No_Cheque + "', ";
        //            Mi_SQL += Ope_Con_Pagos.Campo_Referencia_Transferencia_Banca + " = '', ";
        //        }
        //        else
        //        {
        //            Mi_SQL += Ope_Con_Pagos.Campo_No_Cheque + " = '', ";
        //            Mi_SQL += Ope_Con_Pagos.Campo_Referencia_Transferencia_Banca + " = '" + Datos.P_Referencia + "', ";
        //        }
        //        Mi_SQL += Ope_Con_Pagos.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Modifico + "',";
        //        Mi_SQL += Ope_Con_Pagos.Campo_Fecha_Modifico + " =SYSDATE";
        //        Mi_SQL += " WHERE " + Ope_Con_Pagos.Campo_No_Pago + " = '" + Datos.P_No_Pago  + "'";

        //        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        //        if (Datos.P_Estatus == "CANCELADO")
        //        {
        //            Mi_SQL = "UPDATE " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + " SET ";
        //            //Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Empleado_ID_Contabilidad  + "='" + Datos.P_Empleado_ID_Contabilidad + "',";
        //            Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Estatus + "='AUTORIZADO',";
        //            Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Creo + "',";
        //            Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Fecha_Modifico + "= SYSDATE " + " WHERE ";
        //            Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + " ='" + Datos.P_No_Solicitud_Pago + "'";
        //            OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        //        }
        //    }
        //    catch (OracleException Ex)
        //    {
        //        throw new Exception("Error: " + Ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error : " + ex.Message, ex);
        //    }
        //}
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCION: Cambiar_Estatus_Solicitud_Pago
            /// DESCRIPCION : Autoriza o rechaza la solicitud de pago 
            /// PARAMETROS  : 
            /// CREO        : Sergio Manuel Gallardo Andrade
            /// FECHA_CREO  : 15/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
        public static void Cambiar_Estatus_Solicitud_Pago(Cls_Ope_Con_Cheques_Negocio Datos)
            {
                String Mi_SQL;
                try
                {
                    //Da de Alta los datos del Cierre Mensual con los datos proporcionados por el usuario
                    Mi_SQL = "UPDATE " + Ope_Con_Solicitud_Pagos.Tabla_Ope_Con_Solicitud_Pagos + " SET ";
                    Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Comentarios_Contabilidad   + "='" + Datos.P_Comentario  + "',";
                    //Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Empleado_ID_Contabilidad  + "='" + Datos.P_Empleado_ID_Contabilidad + "',";
                    Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Estatus + "='" + Datos.P_Estatus + "',";
                    Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "',";
                    Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_Fecha_Autorizo_Rechazo_Contabilidad + "= SYSDATE " + " WHERE ";
                    Mi_SQL += Ope_Con_Solicitud_Pagos.Campo_No_Solicitud_Pago + " ='" + Datos.P_No_Solicitud_Pago + "'";
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                catch (OracleException Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error: " + Ex.Message);
                }
            }
        
    }
}

