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
using Presidencia.Cierre_Mensual.Negocio;
using Presidencia.Polizas.Negocios;

namespace Presidencia.Cierre_Mensual.Datos
{
    public class Cls_Ope_Con_Cierre_Mensual_Datos
    {
        #region (Metodos Operacion)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Cierre_Mensual
            /// DESCRIPCION : Comienza la operacion del Cierre Mensual
            /// PARAMETROS  : Datos: Recibe los datos necesarios para efectuar la consulta.
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 30/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Cierre_Mensual(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta para la póliza
                try
                {
                    Mi_SQL = "SELECT " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", ";
                    Mi_SQL += Ope_Con_Polizas_Detalles.Campo_Debe + ", ";
                    Mi_SQL += Ope_Con_Polizas_Detalles.Campo_Haber;
                    Mi_SQL += " FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                    Mi_SQL += " WHERE " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + " = '" + Datos.P_Mes_Anio + "'";
                    Mi_SQL += " ORDER BY " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " ASC";
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
            /// NOMBRE DE LA FUNCION: Cuentas_Movimientos_Mes
            /// DESCRIPCION : Consulta las cuentas 
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 30/Septiembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Cuentas_Movimientos_Mes(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta para la póliza
                try
                {
                    Mi_SQL = "SELECT DISTINCT " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID;
                    Mi_SQL += " FROM " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                    Mi_SQL += " WHERE " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + " = '" + Datos.P_Mes_Anio + "'";
                    Mi_SQL += " ORDER BY " + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + " ASC";
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
            /// NOMBRE DE LA FUNCION: Cuentas_Contables_Afectables
            /// DESCRIPCION : Consulta las cuentas que se pueden afectar 
            /// PARAMETROS  : 
            /// CREO        : SErgio Manuel Gallardo 
            /// FECHA_CREO  : 08/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Cuentas_Contables_Afectables(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL; //Variable para la consulta para la póliza
                try
                {
                    Mi_SQL = "SELECT  " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID;
                    Mi_SQL += " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables ;
                    Mi_SQL += " WHERE " + Cat_Con_Cuentas_Contables.Campo_Afectable + " = 'SI'";
                    Mi_SQL += " ORDER BY " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " ASC";
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
            /// NOMBRE DE LA FUNCION: Saldo_Inicial_Cierre_Mensual
            /// DESCRIPCION : Consulta el Saldo Inicial de la cuenta con respecto al ultimo cierre
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 3/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Saldo_Inicial_Cierre_Mensual(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL;             //Variable de Consulta para el Saldo Final del Cierre Mensual anterior
                String Mes_Anio_Anterior;  //Variable que contendra el mes anterior al cierre.
                DateTime fecha;
                try
                {
                    //se establece el mes anterior para obtener su saldo
                    fecha = Convert.ToDateTime(String.Format("{0: dd/MM/yyyy}", "01/" + Datos.P_Mes_Anio.Substring(0, 2) + "/" + Datos.P_Mes_Anio.Substring(2, 2)));
                    Mes_Anio_Anterior = String.Format("{0:MMyy}",fecha.AddMonths(-1));
                    //Da de Alta los datos del Cierre Mensual con los datos proporcionados por el usuario
                    Mi_SQL = "SELECT " + Ope_Con_Cierre_Mensual.Campo_Saldo_Final;
                    Mi_SQL += " FROM " + Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual;
                    Mi_SQL += " WHERE " + Ope_Con_Cierre_Mensual.Campo_Mes_Anio + " = '" + Mes_Anio_Anterior + "'";
                    Mi_SQL += " AND " + Ope_Con_Cierre_Mensual.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "'";

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
        #endregion

        #region (Operaciones [Alta - Actualizar - Eliminar])
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Cierre_Mensual_Alta
            /// DESCRIPCION : Consulta las cuentas que tuvieron movimientos en el mes.
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 3/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Cierre_Mensual_Alta(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL;             //Variable de Consulta para la Alta del Cierre Mensual
                Object Cierre_Mensual_ID; //Variable que contendrá el ID de la consulta

                try
                {
                    //Obtiene el ID del ultimo Cierre Mensual.
                    Mi_SQL = "SELECT NVL(MAX (" + Ope_Con_Cierre_Mensual.Campo_Cierre_Mensual_ID + "),'00000') ";
                    Mi_SQL = Mi_SQL + "FROM " + Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual;
                    Cierre_Mensual_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    
                    if (Convert.IsDBNull(Cierre_Mensual_ID))
                        Datos.P_Cierre_Mensual_ID = "00001";                    
                    else
                        Datos.P_Cierre_Mensual_ID = String.Format("{0:00000}", Convert.ToInt32(Cierre_Mensual_ID) + 1);

                    //Da de Alta los datos del Cierre Mensual con los datos proporcionados por el usuario
                    Mi_SQL = "INSERT INTO " + Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual + "(";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Cierre_Mensual_ID + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Cuenta_Contable_ID + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Mes_Anio + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Saldo_Inicial + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Saldo_Final + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Total_Debe + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Total_Haber + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Diferencia + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Fecha_Inicio + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Fecha_Final + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Fecha_Creo + ") VALUES ('";
                    Mi_SQL += Datos.P_Cierre_Mensual_ID + "', '";
                    Mi_SQL += Datos.P_Cuenta_Contable_ID + "', '";
                    Mi_SQL += Datos.P_Mes_Anio + "', ";
                    Mi_SQL += Datos.P_Saldo_Inicial + ", ";
                    Mi_SQL += Datos.P_Saldo_Final + ", ";
                    Mi_SQL += Datos.P_Total_Debe + ", ";
                    Mi_SQL += Datos.P_Total_Haber + ", ";
                    Mi_SQL += Datos.P_Diferencia + ", '";
                    Mi_SQL += Datos.P_Fecha_Inicio + "', '";
                    Mi_SQL += Datos.P_Fecha_Final + "', '";
                    Mi_SQL += Datos.P_Usuario_Creo + "', SYSDATE)";

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
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Alta_Cierre_Mensual_General
            /// DESCRIPCION : realiza el cierre del mes con las cuentas.
            /// PARAMETROS  : 
            /// CREO        : Sergio Manuel Gallardo Andrade
            /// FECHA_CREO  : 07/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Alta_Cierre_Mensual_General(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL;             //Variable de Consulta para la Alta del Cierre Mensual
                Object Cierre_Mensual_ID; //Variable que contendrá el ID de la consulta

                try
                {
                    //Obtiene el ID del ultimo Cierre Mensual.
                    Mi_SQL = "SELECT NVL(MAX (" + Ope_Con_Cierre_Mensual_Gral.Campo_Cierre_Mensual_Gral_ID   + "),'00000') ";
                    Mi_SQL = Mi_SQL + "FROM " + Ope_Con_Cierre_Mensual_Gral.Tabla_Ope_Con_Cierre_Mensual_Gral ;
                    Cierre_Mensual_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Convert.IsDBNull(Cierre_Mensual_ID))
                        Datos.P_Cierre_Mensual_ID  = "00001";
                    else
                        Datos.P_Cierre_Mensual_ID = String.Format("{0:00000}", Convert.ToInt32(Cierre_Mensual_ID) + 1);

                    //Da de Alta los datos del Cierre Mensual con los datos proporcionados por el usuario
                    Mi_SQL = "INSERT INTO " + Ope_Con_Cierre_Mensual_Gral.Tabla_Ope_Con_Cierre_Mensual_Gral  + "(";
                    Mi_SQL += Ope_Con_Cierre_Mensual_Gral.Campo_Cierre_Mensual_Gral_ID   + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual_Gral.Campo_Mes   + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual_Gral.Campo_Anio   + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual_Gral.Campo_Estatus  + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual_Gral.Campo_Usuario_Creo   + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual_Gral.Campo_Fecha_Creo  + ") VALUES ('";
                    Mi_SQL += Datos.P_Cierre_Mensual_ID + "', '";
                    Mi_SQL += Datos.P_Mes  + "', '";
                    Mi_SQL += Datos.P_Anio  + "', ";
                    Mi_SQL += " 'CERRADO', '";
                    Mi_SQL += Datos.P_Usuario_Creo + "', SYSDATE)";
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
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Modifica_Cierre_Mensual_Gral
            /// DESCRIPCION : Afecta el cierre mensual de las cuentas y lo pone como afectado 
            /// PARAMETROS  : 
            /// CREO        : Sergio Manuel Gallardo Andrade
            /// FECHA_CREO  : 07/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Modifica_Cierre_Mensual_Gral(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL;             //Variable de Consulta para la Alta del Cierre Mensual


                try
                {
                    //Da de Alta los datos del Cierre Mensual con los datos proporcionados por el usuario
                    Mi_SQL = "UPDATE " + Ope_Con_Cierre_Mensual_Gral.Tabla_Ope_Con_Cierre_Mensual_Gral + " SET ";
                    Mi_SQL += Ope_Con_Cierre_Mensual_Gral.Campo_Estatus + "='" + Datos.P_Estatus +"',";
                    Mi_SQL += Ope_Con_Cierre_Mensual_Gral.Campo_Usuario_Modifico + "='"+ Datos.P_Usuario_Modifico +"',";
                    Mi_SQL += Ope_Con_Cierre_Mensual_Gral.Campo_Fecha_Modifico + "= SYSDATE " + " WHERE ";
                    Mi_SQL += Ope_Con_Cierre_Mensual_Gral.Campo_Mes + " ='" + Datos.P_Mes + "' AND " + Ope_Con_Cierre_Mensual_Gral.Campo_Anio + "='" + Datos.P_Anio+"'";
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
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Limpiar_Cierre_Mensual
            /// DESCRIPCION : Elimina los registros de las cuentas que se hayan cerrado con anterioridad
            /// PARAMETROS  : 
            /// CREO        : Sergio Manuel Gallardo Andrade
            /// FECHA_CREO  : 08/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static void Limpiar_Cierre_Mensual(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL;             //Variable de Consulta para la Alta del Cierre Mensual

                try
                {
                    Mi_SQL = "DELETE  FROM " + Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual ;
                    Mi_SQL += " WHERE " + Ope_Con_Cierre_Mensual.Campo_Mes_Anio + "='"+ Datos.P_Mes_Anio+"'";

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
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Cierre_Mensual_Actualizar
            /// DESCRIPCION : Consulta las cuentas que tuvieron movimientos en el mes.
            /// PARAMETROS  : 
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 3/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            /////*******************************************************************************
            //public static void Cierre_Mensual_Actualizar(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            //{
            //    String Mi_SQL;             //Variable de Consulta para la Alta del Cierre Mensual
            //    Object Cierre_Mensual_ID; //Variable que contendrá el ID de la consulta

            //    try
            //    {
            //        //Da de Alta los datos del Cierre Mensual con los datos proporcionados por el usuario
            //        Mi_SQL = "UPDATE " + Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual + " SET ";
            //        Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Saldo_Final + " = " + Datos.P_Saldo_Final + ", ";
            //        Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Total_Debe + " = " + Datos.P_Total_Debe + ", ";
            //        Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Total_Haber + " = " + Datos.P_Total_Haber + ", ";
            //        Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Diferencia  + " = " + Datos.P_Diferencia  + ", ";
            //        Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Fecha_Modifico + " = SYSDATE , ";
            //        Mi_SQL += Ope_Con_Cierre_Mensual.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Modifico+"' ";
            //        Mi_SQL += " WHERE " + Ope_Con_Cierre_Mensual.Campo_Cierre_Mensual_ID + " = '" + Datos.P_Cierre_Mensual_ID + "'";

            //        OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //    }
            //    catch (OracleException Ex)
            //    {
            //        throw new Exception("Error: " + Ex.Message);
            //    }
            //    catch (Exception Ex)
            //    {
            //        throw new Exception("Error: " + Ex.Message);
            //    }
            //}
             //*******************************************************************************
            //NOMBRE DE LA FUNCION: Abrir_Cierre_Mensual
             //DESCRIPCION : Re-abrir el mes que esta cerrado .
             //PARAMETROS  : 
             //CREO        : Sergio Manuel Gallardo Andrade
             //FECHA_CREO  : 09/Noviembre/2011
             //MODIFICO          :
             //FECHA_MODIFICO    :
             //CAUSA_MODIFICACION:
            //*******************************************************************************
            public static void Abrir_Cierre_Mensual(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL;             //Variable de Consulta para la Alta del Cierre Mensual

                try
                {
                    //Elimina el Registro que tiene el mes cerrado para que se visualice como abierto
                    Mi_SQL = "UPDATE " + Ope_Con_Cierre_Mensual_Gral.Tabla_Ope_Con_Cierre_Mensual_Gral + " SET ";
                    Mi_SQL += Ope_Con_Cierre_Mensual_Gral.Campo_Estatus + " = 'ABIERTO'";
                    Mi_SQL += " WHERE " + Ope_Con_Cierre_Mensual_Gral.Campo_Anio + " = '" + Datos.P_Anio + "' ";
                    Mi_SQL += " AND " + Ope_Con_Cierre_Mensual_Gral.Campo_Mes + " = '" + Datos.P_Mes + "'";
                    //Mi_SQL = "DELETE FROM " + Ope_Con_Cierre_Mensual_Gral.Tabla_Ope_Con_Cierre_Mensual_Gral + " ";
                    //Mi_SQL += " WHERE " + Ope_Con_Cierre_Mensual_Gral.Campo_Anio + " = '" + Datos.P_Anio  + "' ";
                    //Mi_SQL += " AND " + Ope_Con_Cierre_Mensual_Gral.Campo_Mes + " = '" + Datos.P_Mes  + "'";
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
        #endregion

        #region (Metodos Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Cierre_Mensual_Auxiliar
            /// DESCRIPCION : Consulta el Saldo Inicial de la cuenta con respecto al ultimo cierre
            /// PARAMETROS  : Datos: Recibe los datos proporcionados por el usuario.
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 18/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Cierre_Mensual_Auxiliar(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL;  //Almacenara la Query de Consulta.
                try
                {
                    //Consulta los movimientos de las cuentas contables.
                    Mi_SQL = "SELECT " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + ", ";
                    Mi_SQL += Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Descripcion + ", ";
                    Mi_SQL += Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual + "." + Ope_Con_Cierre_Mensual.Campo_Saldo_Final + " AS SALDO_FINAL";
                    Mi_SQL += " FROM " + Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual + " RIGHT OUTER JOIN " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + " ON " + Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual + "." + Ope_Con_Cierre_Mensual.Campo_Cuenta_Contable_ID + " = " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID;
                    Mi_SQL += " WHERE " + Ope_Con_Cierre_Mensual.Campo_Mes_Anio + " = '" + Datos.P_Mes_Anio + "'";
                    if (!String.IsNullOrEmpty(Datos.P_Descripcion))
                    {
                        Mi_SQL += " AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Descripcion + " LIKE UPPER ('%" + Datos.P_Descripcion + "%')";
                    }
                    Mi_SQL += " ORDER BY CUENTA ASC";

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
            /// NOMBRE DE LA FUNCION: Consulta_Cierre_Mensual
            /// DESCRIPCION : Consulta el Saldo Inicial de la cuenta con respecto al ultimo cierre
            /// PARAMETROS  : Datos: Recibe los datos proporcionados por el usuario.
            /// CREO        : Salvador L. Rea Ayala
            /// FECHA_CREO  : 18/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Cierre_Mensual(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL;  //Almacenara la Query de Consulta.
                try
                {
                    //Consulta los movimientos de las cuentas contables.
                    Mi_SQL = "SELECT * FROM " + Ope_Con_Cierre_Mensual.Tabla_Ope_Con_Cierre_Mensual;
                    Mi_SQL += " WHERE " + Ope_Con_Cierre_Mensual.Campo_Mes_Anio + " = '" + Datos.P_Mes_Anio + "'";
                    Mi_SQL += " ORDER BY " + Ope_Con_Cierre_Mensual.Campo_Cuenta_Contable_ID + " ASC";

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
            /// NOMBRE DE LA FUNCION: Consulta_Bitacora
            /// DESCRIPCION : Consulta las polizas que entraron en un mes cerrado
            /// PARAMETROS  : Datos: Recibe los datos proporcionados por el usuario.
            /// CREO        : Sergio Manuel Gallardo Andrade
            /// FECHA_CREO  : 09/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Bitacora(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL;  //Almacenara la Query de Consulta.
                String Mes_Anio;
                DateTime fecha;
                try
                {
                    fecha = Convert.ToDateTime(String.Format("{0: dd/MMMM/yyyy}", "01/" + Datos.P_Mes + "/" + Datos.P_Anio ));
                    Mes_Anio = String.Format("{0:MMyy}",fecha);

                    //Consulta los movimientos de las cuentas contables.
                    Mi_SQL = "SELECT Cuenta." + Cat_Con_Cuentas_Contables.Campo_Cuenta + ", Bitacora." + Ope_Con_Bitacora_Polizas.Campo_Usuario_Creo + ", ";
                    Mi_SQL += " Bitacora." + Ope_Con_Bitacora_Polizas.Campo_Fecha_Creo + ", Bitacora." + Ope_Con_Bitacora_Polizas.Campo_Debe + ", ";
                    Mi_SQL += " Bitacora." + Ope_Con_Bitacora_Polizas .Campo_Haber +", Tipo."+ Cat_Con_Tipo_Polizas.Campo_Descripcion + " FROM " + Ope_Con_Bitacora_Polizas.Tabla_Ope_Con_Bitacora_Polizas +" Bitacora, ";
                    Mi_SQL += Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables +" Cuenta,"+ Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas +" Tipo WHERE Bitacora."+Ope_Con_Bitacora_Polizas.Campo_Cuenta_Contable_ID +" = Cuenta.";
                    Mi_SQL += Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID+" AND Tipo."+ Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID +" = Bitacora."+Ope_Con_Bitacora_Polizas.Campo_Tipo_Poliza_ID + " AND Bitacora."+ Ope_Con_Bitacora_Polizas.Campo_Mes_Ano + " = '" + Mes_Anio + "'";
                    Mi_SQL += " ORDER BY " + Ope_Con_Bitacora_Polizas.Campo_No_Bitacora  + " ASC";

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
            /// NOMBRE DE LA FUNCION: Consulta_Cierre_General
            /// DESCRIPCION : Consulta los meses cerrados por el usuario 
            /// PARAMETROS  : Datos: Recibe los datos proporcionados por el usuario.
            /// CREO        : Sergio Manuel Gallardo Andrade
            /// FECHA_CREO  : 04/Noviembre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            public static DataTable Consulta_Cierre_General(Cls_Ope_Con_Cierre_Mensual_Negocio Datos)
            {
                String Mi_SQL;  //Almacenara la Query de Consulta.
                try
                {
                    //Consulta los movimientos de las cuentas contables.
                    Mi_SQL = "SELECT * FROM " + Ope_Con_Cierre_Mensual_Gral.Tabla_Ope_Con_Cierre_Mensual_Gral;
                    Mi_SQL += " WHERE " + Ope_Con_Cierre_Mensual_Gral.Campo_Anio  + " = '" + Datos.P_Anio  + "'";
                    if (Datos.P_Mes != null)
                    {
                        Mi_SQL += " AND " + Ope_Con_Cierre_Mensual_Gral.Campo_Mes  + " = '" + Datos.P_Mes  + "'";
                    }
                    Mi_SQL += " ORDER BY " + Ope_Con_Cierre_Mensual_Gral.Campo_Cierre_Mensual_Gral_ID  + " ASC";

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
        
        #endregion
    }
}