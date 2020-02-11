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
using Presidencia.Catalogo_Cuentas_Predial.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Cuentas_Predial_Datos
/// </summary>
namespace Presidencia.Catalogo_Cuentas_Predial.Datos
{
    public class Cls_Cat_Pre_Cuentas_Predial_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Cuenta
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nueva cuenta
        ///PARAMETROS           : Cuenta, instancia de Cls_Cat_Pre_Cuentas_Datos
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 06/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Alta_Cuenta(Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Alta;

            Alta = false;

            if (Cuenta.P_Cmmd != null)
            {
                //Trans = Cuenta.P_Trans;
                Cmd = Cuenta.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }
            //Cmd.Connection = Trans.Connection;
            //Cmd.Transaction = Trans;

            try
            {
                String Cuenta_Predial_ID = Obtener_ID_Consecutivo(Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas, Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID, 10);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "(";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Efectos + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Diferencia + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Usuario_Creo + ", ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES (";
                if (Cuenta_Predial_ID != null && Cuenta_Predial_ID != "")
                {
                    Mi_SQL += "'" + Cuenta_Predial_ID + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Cuenta_Predial != null && Cuenta.P_Cuenta_Predial != "")
                {
                    Mi_SQL += "UPPER('" + Cuenta.P_Cuenta_Predial + "'), ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Calle_ID != null && Cuenta.P_Calle_ID != "")
                {
                    Mi_SQL += Cuenta.P_Calle_ID + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Estado_Predio_ID != null && Cuenta.P_Estado_Predio_ID != "")
                {
                    Mi_SQL += Cuenta.P_Estado_Predio_ID + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Tipo_Predio_ID != null && Cuenta.P_Tipo_Predio_ID != "")
                {
                    Mi_SQL += Cuenta.P_Tipo_Predio_ID + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Uso_Suelo_ID != null && Cuenta.P_Uso_Suelo_ID != "")
                {
                    Mi_SQL += Cuenta.P_Uso_Suelo_ID + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Tasa_Predial_ID != null && Cuenta.P_Tasa_Predial_ID != "")
                {
                    Mi_SQL += Cuenta.P_Tasa_Predial_ID + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Tasa_ID != null && Cuenta.P_Tasa_ID != "")
                {
                    Mi_SQL += Cuenta.P_Tasa_ID + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Cuota_Minima_ID != null && Cuenta.P_Cuota_Minima_ID != "")
                {
                    Mi_SQL += Cuenta.P_Cuota_Minima_ID + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Cuenta_Origen != null && Cuenta.P_Cuenta_Origen != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Cuenta_Origen + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Estatus != null && Cuenta.P_Estatus != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Estatus + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_No_Exterior != null && Cuenta.P_No_Exterior != "")
                {
                    Mi_SQL += "'" + Cuenta.P_No_Exterior + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_No_Interior != null && Cuenta.P_No_Interior != "")
                {
                    Mi_SQL += "'" + Cuenta.P_No_Interior + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Superficie_Construida != null)
                {
                    Mi_SQL += Cuenta.P_Superficie_Construida + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Superficie_Total != null)
                {
                    Mi_SQL += Cuenta.P_Superficie_Total + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Clave_Catastral != null && Cuenta.P_Clave_Catastral != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Clave_Catastral + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Valor_Fiscal != null)
                {
                    Mi_SQL += Cuenta.P_Valor_Fiscal + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Efectos != null && Cuenta.P_Efectos != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Efectos + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Periodo_Corriente != null && Cuenta.P_Periodo_Corriente != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Periodo_Corriente + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Cuota_Anual != null)
                {
                    Mi_SQL += Cuenta.P_Cuota_Anual + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Cuota_Fija != null && Cuenta.P_Cuota_Fija != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Cuota_Fija + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Termino_Exencion != null)
                {
                    Mi_SQL += "'" + Cuenta.P_Termino_Exencion.ToString("d-M-yyyy") + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Fecha_Avaluo != null)
                {
                    Mi_SQL += "'" + Cuenta.P_Fecha_Avaluo.ToString("d-M-yyyy") + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Diferencia_Construccion != null)
                {
                    Mi_SQL += Cuenta.P_Diferencia_Construccion + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Costo_M2 != null)
                {
                    Mi_SQL += Cuenta.P_Costo_M2 + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Porcentaje_Exencion != null)
                {
                    Mi_SQL += Cuenta.P_Porcentaje_Exencion + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_No_Cuota_Fija != null && Cuenta.P_No_Cuota_Fija != "")
                {
                    Mi_SQL += Cuenta.P_No_Cuota_Fija + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Calle_ID_Notificacion != null && Cuenta.P_Calle_ID_Notificacion != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Calle_ID_Notificacion + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Colonia_ID != null && Cuenta.P_Colonia_ID != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Colonia_ID + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Colonia_ID_Notificacion != null && Cuenta.P_Colonia_ID_Notificacion != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Colonia_ID_Notificacion + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Estado_ID_Notificacion != null && Cuenta.P_Estado_ID_Notificacion != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Estado_ID_Notificacion + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Ciudad_ID_Notificacion != null && Cuenta.P_Ciudad_ID_Notificacion != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Ciudad_ID_Notificacion + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Domicilio_Foraneo != null && Cuenta.P_Domicilio_Foraneo != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Domicilio_Foraneo + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Calle_Notificacion != null && Cuenta.P_Calle_Notificacion != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Calle_Notificacion + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_No_Exterior_Notificacion != null && Cuenta.P_No_Exterior_Notificacion != "")
                {
                    Mi_SQL += "'" + Cuenta.P_No_Exterior_Notificacion + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Codigo_Postal != null && Cuenta.P_Codigo_Postal != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Codigo_Postal + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_No_Interior_Notificacion != null && Cuenta.P_No_Interior_Notificacion != "")
                {
                    Mi_SQL += "'" + Cuenta.P_No_Interior_Notificacion + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Colonia_Notificacion != null && Cuenta.P_Colonia_Notificacion != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Colonia_Notificacion + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_No_Diferencia != null && Cuenta.P_No_Diferencia != "")
                {
                    Mi_SQL += "'" + Cuenta.P_No_Diferencia + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Estado_Notificacion != null && Cuenta.P_Estado_Notificacion != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Estado_Notificacion + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Ciudad_Notificacion != null && Cuenta.P_Ciudad_Notificacion != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Ciudad_Notificacion + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Cuenta.P_Tipo_Suspencion != null && Cuenta.P_Tipo_Suspencion != "")
                {
                    Mi_SQL += "'" + Cuenta.P_Tipo_Suspencion + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += "'" + Cuenta.P_Usuario + "', SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Cuenta.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Cuenta.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar dar de Alta la Cuenta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Cuenta.P_Cmmd == null)
                {
                    if (Cn.State == ConnectionState.Open)
                    {
                        Cn.Close();
                    }
                }
            }
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Cuenta
        ///DESCRIPCIÓN          : Actualiza en la Base de Datos una Cuenta de acuerdo a los parámetros indicados en la interfaz
        ///PARAMETROS           : Cuenta, instancia de Cls_Cat_Pre_Cuentas_Predial_Negocio
        ///CREO                 : Antonio Salvador Benavides Guarddado
        ///FECHA_CREO           : 07/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Modificar_Cuenta(Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Modificar;

            if (Cuenta.P_Cmmd != null)
            {
                //Trans = Cuenta.P_Trans;
                Cmd = Cuenta.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }
            //Cmd.Connection = Trans.Connection;
            //Cmd.Transaction = Trans;

            Modificar = false;
            try
            {
                String Mi_SQL = "UPDATE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL += " SET ";
                if (Cuenta.P_Cuenta_Predial != null && Cuenta.P_Cuenta_Predial != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Cuenta.P_Cuenta_Predial + "', ";
                }
                if (Cuenta.P_Colonia_ID != null && Cuenta.P_Colonia_ID != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = '" + Cuenta.P_Colonia_ID + "', ";
                }
                if (Cuenta.P_Calle_ID != null && Cuenta.P_Calle_ID != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = '" + Cuenta.P_Calle_ID + "', ";
                }
                if (Cuenta.P_Propietario_ID != null && Cuenta.P_Propietario_ID != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Propietario_ID + " = '" + Cuenta.P_Propietario_ID + "', ";
                }
                if (Cuenta.P_Copropietario_ID != null && Cuenta.P_Copropietario_ID != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Copropietario_ID + " = '" + Cuenta.P_Copropietario_ID + "', ";
                }
                if (Cuenta.P_Estado_Predio_ID != null && Cuenta.P_Estado_Predio_ID != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + " = '" + Cuenta.P_Estado_Predio_ID + "', ";
                }
                if (Cuenta.P_Tipo_Predio_ID != null && Cuenta.P_Tipo_Predio_ID != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = '" + Cuenta.P_Tipo_Predio_ID + "', ";
                }
                if (Cuenta.P_Uso_Suelo_ID != null && Cuenta.P_Uso_Suelo_ID != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + " = '" + Cuenta.P_Uso_Suelo_ID + "', ";
                }
                if (Cuenta.P_Tasa_Predial_ID != null && Cuenta.P_Tasa_Predial_ID != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + " = '" + Cuenta.P_Tasa_Predial_ID + "', ";
                }
                if (Cuenta.P_Tasa_ID != null && Cuenta.P_Tasa_ID != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + " = '" + Cuenta.P_Tasa_ID + "', ";
                }
                if (Cuenta.P_Cuota_Minima_ID != null && Cuenta.P_Cuota_Minima_ID != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + " = '" + Cuenta.P_Cuota_Minima_ID + "', ";
                }
                if (Cuenta.P_Cuenta_Origen != null && Cuenta.P_Cuenta_Origen != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + " = '" + Cuenta.P_Cuenta_Origen + "', ";
                }
                if (Cuenta.P_Estatus != null && Cuenta.P_Estatus != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estatus + " = '" + Cuenta.P_Estatus + "', ";
                }
                if (Cuenta.P_No_Exterior != null && Cuenta.P_No_Exterior != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " = '" + Cuenta.P_No_Exterior + "', ";
                }
                if (Cuenta.P_No_Interior != null && Cuenta.P_No_Interior != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Interior + " = '" + Cuenta.P_No_Interior + "', ";
                }
                if (Cuenta.P_Superficie_Construida != 0)
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " = " + Cuenta.P_Superficie_Construida + ", ";
                }
                if (Cuenta.P_Superficie_Total != 0)
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " = " + Cuenta.P_Superficie_Total + ", ";
                }
                if (Cuenta.P_Clave_Catastral != null && Cuenta.P_Clave_Catastral != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + " = '" + Cuenta.P_Clave_Catastral + "', ";
                }
                if (Cuenta.P_Valor_Fiscal != 0)
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + " = " + Cuenta.P_Valor_Fiscal + ", ";
                }
                if (Cuenta.P_Efectos != null && Cuenta.P_Efectos != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Efectos + " = '" + Cuenta.P_Efectos + "', ";
                }
                if (Cuenta.P_Periodo_Corriente != null && Cuenta.P_Periodo_Corriente != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + " = '" + Cuenta.P_Periodo_Corriente + "', ";
                }
                if (Cuenta.P_Cuota_Anual != 0)
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + " = " + Cuenta.P_Cuota_Anual + ", ";
                }
                if (Cuenta.P_Porcentaje_Exencion != 0)
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + " = " + Cuenta.P_Porcentaje_Exencion + ", ";
                }
                if (Cuenta.P_Cuota_Fija != null && Cuenta.P_Cuota_Fija != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " = '" + Cuenta.P_Cuota_Fija + "', ";
                }
                if (Cuenta.P_Termino_Exencion != null)
                {
                    if (Cuenta.P_Termino_Exencion > DateTime.MinValue)
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + " = '" + Cuenta.P_Termino_Exencion.ToString("d-M-yyyy") + "', ";
                    }
                }
                if (Cuenta.P_Fecha_Avaluo != null)
                {
                    if (Cuenta.P_Fecha_Avaluo > DateTime.MinValue)
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + " = '" + Cuenta.P_Fecha_Avaluo.ToString("d-M-yyyy") + "', ";
                    }
                }
                if (Cuenta.P_Diferencia_Construccion != 0)
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + " = " + Cuenta.P_Diferencia_Construccion + ", ";
                }
                if (Cuenta.P_Costo_M2 != 0)
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + " = " + Cuenta.P_Costo_M2 + ", ";
                }
                if (Cuenta.P_No_Cuota_Fija != null && Cuenta.P_No_Cuota_Fija != "")
                {
                    if (Cuenta.P_No_Cuota_Fija != "NULL")
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + " = '" + Cuenta.P_No_Cuota_Fija + "', ";
                    }
                    else
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + " = NULL,";
                    }
                }

                if (Cuenta.P_Calle_ID_Notificacion != null && Cuenta.P_Calle_ID_Notificacion != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = '" + Cuenta.P_Calle_ID_Notificacion + "', ";
                }
                if (Cuenta.P_Colonia_ID_Notificacion != null && Cuenta.P_Colonia_ID_Notificacion != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = '" + Cuenta.P_Colonia_ID_Notificacion + "', ";
                }
                if (Cuenta.P_Estado_ID_Notificacion != null && Cuenta.P_Estado_ID_Notificacion != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = '" + Cuenta.P_Estado_ID_Notificacion + "', ";
                }
                if (Cuenta.P_Ciudad_ID_Notificacion != null && Cuenta.P_Ciudad_ID_Notificacion != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " = '" + Cuenta.P_Ciudad_ID_Notificacion + "', ";
                }
                if (Cuenta.P_Domicilio_Foraneo != null && Cuenta.P_Domicilio_Foraneo != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + " = '" + Cuenta.P_Domicilio_Foraneo + "', ";
                }
                if (Cuenta.P_Calle_Notificacion != null && Cuenta.P_Calle_Notificacion != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + " = '" + Cuenta.P_Calle_Notificacion + "', ";
                }
                if (Cuenta.P_No_Exterior_Notificacion != null && Cuenta.P_No_Exterior_Notificacion != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + " = '" + Cuenta.P_No_Exterior_Notificacion + "', ";
                }
                if (Cuenta.P_Codigo_Postal != null && Cuenta.P_Codigo_Postal != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + " = '" + Cuenta.P_Codigo_Postal + "', ";
                }
                if (Cuenta.P_No_Interior_Notificacion != null && Cuenta.P_No_Interior_Notificacion != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + " = '" + Cuenta.P_No_Interior_Notificacion + "', ";
                }
                if (Cuenta.P_Colonia_Notificacion != null && Cuenta.P_Colonia_Notificacion != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + " = '" + Cuenta.P_Colonia_Notificacion + "', ";
                }
                if (Cuenta.P_No_Diferencia != null && Cuenta.P_No_Diferencia != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Diferencia + " = '" + Cuenta.P_No_Diferencia + "', ";
                }
                if (Cuenta.P_Estado_Notificacion != null && Cuenta.P_Estado_Notificacion != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + " = '" + Cuenta.P_Estado_Notificacion + "', ";
                }
                if (Cuenta.P_Ciudad_Notificacion != null && Cuenta.P_Ciudad_Notificacion != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + " = '" + Cuenta.P_Ciudad_Notificacion + "', ";
                }
                if (Cuenta.P_Tipo_Suspencion != null && Cuenta.P_Tipo_Suspencion != "")
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + " = '" + Cuenta.P_Tipo_Suspencion + "', ";
                }
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Usuario_Modifico + " = '" + Cuenta.P_Usuario + "', ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL += " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta.P_Cuenta_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Cuenta.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Modificar = true;
            }
            catch (OracleException Ex)
            {
                if (Cuenta.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar modificar la Cuenta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Cuenta.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Modificar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Aplicar_Variacion_Cuenta
        ///DESCRIPCIÓN          : Actualiza en la Base de Datos una Cuenta de acuerdo a la Variación de la Orden
        ///PARAMETROS           : Cuenta, instancia de Cls_Cat_Pre_Cuentas_Predial_Negocio
        ///CREO                 : Antonio Salvador Benavides Guarddado
        ///FECHA_CREO           : 13/Marzo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Aplicar_Variacion_Cuenta(Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Modificar;
            Boolean Estatus_Cuenta_Preasignado = false;

            if (Cuenta.P_Cmmd != null)
            {
                //Trans = Cuenta.P_Trans;
                Cmd = Cuenta.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }
            //Cmd.Connection = Trans.Connection;
            //Cmd.Transaction = Trans;

            Modificar = false;
            try
            {
                if (Cuenta.P_Dt_Variacion_Cuenta != null)
                {
                    DataRow[] Arr_Dr_Variacion_Cuenta;
                    String Mi_SQL = "UPDATE " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                    Mi_SQL += " SET ";

                    Arr_Dr_Variacion_Cuenta = Cuenta.P_Dt_Variacion_Cuenta.Select("NOMBRE_CAMPO = '" + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta + "'");
                    if (Arr_Dr_Variacion_Cuenta.Length > 0)
                    {
                        if (Arr_Dr_Variacion_Cuenta[0]["DATO_NUEVO"] != null)
                        {
                            if (Arr_Dr_Variacion_Cuenta[0]["DATO_NUEVO"].ToString() == "PENDIENTE")
                            {
                                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estatus + " = 'VIGENTE', ";
                                Estatus_Cuenta_Preasignado = true;
                            }
                        }
                    }

                    Arr_Dr_Variacion_Cuenta = Cuenta.P_Dt_Variacion_Cuenta.Select("DIFERENTE = True AND NOT NOMBRE_CAMPO IN ('" + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial + "', '" + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Anio + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + "', '" + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Nota + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + "', 'FECHA_ORDEN')");
                    if (Arr_Dr_Variacion_Cuenta.Length > 0)
                    {
                        Cuenta.P_Dt_Variacion_Cuenta = Arr_Dr_Variacion_Cuenta.CopyToDataTable();
                        foreach (DataRow Dr_Variacion_Cuenta in Cuenta.P_Dt_Variacion_Cuenta.Rows)
                        {
                            switch (Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString())
                            {
                                case Ope_Pre_Ordenes_Variacion.Campo_Calle_ID:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_No_Exterior:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_No_Interior:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Interior + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Estado_ID_Notificacion:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Ciudad_ID_Notificacion:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Estado_Notificacion:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Ciudad_Notificacion:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Estado_Predio_ID:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Uso_Suelo_ID:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Cuota_Minima_ID:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Origen:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta:
                                    if (!Estatus_Cuenta_Preasignado)
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estatus + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    }
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Superficie_Construida:
                                    if (Dr_Variacion_Cuenta["DATO_NUEVO"] != DBNull.Value)
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " = " + Convert.ToDecimal(Dr_Variacion_Cuenta["DATO_NUEVO"]) + ", ";
                                    }
                                    else
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + " = NULL, ";
                                    }
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Superficie_Total:
                                    if (Dr_Variacion_Cuenta["DATO_NUEVO"] != DBNull.Value)
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " = " + Convert.ToDecimal(Dr_Variacion_Cuenta["DATO_NUEVO"]) + ", ";
                                    }
                                    else
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + " = NULL, ";
                                    }
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Clave_Catastral:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Valor_Fiscal:
                                    if (Dr_Variacion_Cuenta["DATO_NUEVO"] != DBNull.Value)
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + " = " + Convert.ToDecimal(Dr_Variacion_Cuenta["DATO_NUEVO"]) + ", ";
                                    }
                                    else
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + " = NULL, ";
                                    }
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Efectos:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Efectos + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Periodo_Corriente:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Cuota_Anual:
                                    if (Dr_Variacion_Cuenta["DATO_NUEVO"] != DBNull.Value)
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + " = " + Convert.ToDecimal(Dr_Variacion_Cuenta["DATO_NUEVO"]) + ", ";
                                    }
                                    else
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + " = NULL, ";
                                    }
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Cuota_Fija:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_No_Cuota_Fija:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Diferencia_Construccion:
                                    if (Dr_Variacion_Cuenta["DATO_NUEVO"] != DBNull.Value)
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + " = " + Convert.ToDecimal(Dr_Variacion_Cuenta["DATO_NUEVO"]) + ", ";
                                    }
                                    else
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + " = NULL, ";
                                    }
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Termino_Exencion:
                                    if (Dr_Variacion_Cuenta["DATO_NUEVO"] != DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(Dr_Variacion_Cuenta["DATO_NUEVO"]) > DateTime.MinValue)
                                        {
                                            Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + " = '" + Convert.ToDateTime(Dr_Variacion_Cuenta["DATO_NUEVO"]).ToString("d-M-yyyy") + "', ";
                                        }
                                        else
                                        {
                                            Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + " = NULL, ";
                                        }
                                    }
                                    else
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + " = NULL, ";
                                    }
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Fecha_Avaluo:
                                    if (Dr_Variacion_Cuenta["DATO_NUEVO"] != DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(Dr_Variacion_Cuenta["DATO_NUEVO"]) > DateTime.MinValue)
                                        {
                                            Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + " = '" + Convert.ToDateTime(Dr_Variacion_Cuenta["DATO_NUEVO"]).ToString("d-M-yyyy") + "', ";
                                        }
                                        else
                                        {
                                            Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + " = NULL, ";
                                        }
                                    }
                                    else
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + " = NULL, ";
                                    }
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Costo_M2:
                                    if (Dr_Variacion_Cuenta["DATO_NUEVO"] != DBNull.Value)
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + " = " + Convert.ToDecimal(Dr_Variacion_Cuenta["DATO_NUEVO"]) + ", ";
                                    }
                                    else
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + " = NULL, ";
                                    }
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Porcentaje_Exencion:
                                    if (Dr_Variacion_Cuenta["DATO_NUEVO"] != DBNull.Value)
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + " = " + Convert.ToDecimal(Dr_Variacion_Cuenta["DATO_NUEVO"]) + ", ";
                                    }
                                    else
                                    {
                                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + " = NULL, ";
                                    }
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Codigo_Postal:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Tasa_Predial_ID:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                                case Ope_Pre_Ordenes_Variacion.Campo_Tasa_ID:
                                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "', ";
                                    break;
                            }
                        }
                    }
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Usuario_Modifico + " = '" + Cuenta.P_Usuario + "', ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL += " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta.P_Cuenta_Predial_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }

                //if (Cuenta.P_Cuenta_Predial != null && Cuenta.P_Cuenta_Predial != "")
                //{
                //}
                //if (Cuenta.P_Colonia_ID != null && Cuenta.P_Colonia_ID != "")
                //{
                //}
                //if (Cuenta.P_Calle_ID != null && Cuenta.P_Calle_ID != "")
                //{
                //}
                //if (Cuenta.P_Propietario_ID != null && Cuenta.P_Propietario_ID != "")
                //{
                //    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Propietario_ID + " = '" + Cuenta.P_Propietario_ID + "', ";
                //}
                //if (Cuenta.P_Copropietario_ID != null && Cuenta.P_Copropietario_ID != "")
                //{
                //    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Copropietario_ID + " = '" + Cuenta.P_Copropietario_ID + "', ";
                //}
                //if (Cuenta.P_Estado_Predio_ID != null && Cuenta.P_Estado_Predio_ID != "")
                //{
                //}
                //if (Cuenta.P_Tipo_Predio_ID != null && Cuenta.P_Tipo_Predio_ID != "")
                //{
                //}
                //if (Cuenta.P_Uso_Suelo_ID != null && Cuenta.P_Uso_Suelo_ID != "")
                //{
                //}
                //if (Cuenta.P_Tasa_Predial_ID != null && Cuenta.P_Tasa_Predial_ID != "")
                //{
                //}
                //if (Cuenta.P_Tasa_ID != null && Cuenta.P_Tasa_ID != "")
                //{
                //}
                //if (Cuenta.P_Cuota_Minima_ID != null && Cuenta.P_Cuota_Minima_ID != "")
                //{
                //}
                //if (Cuenta.P_Cuenta_Origen != null && Cuenta.P_Cuenta_Origen != "")
                //{
                //}
                //if (Cuenta.P_Estatus != null && Cuenta.P_Estatus != "")
                //{
                //}
                //if (Cuenta.P_No_Exterior != null && Cuenta.P_No_Exterior != "")
                //{
                //}
                //if (Cuenta.P_No_Interior != null && Cuenta.P_No_Interior != "")
                //{
                //}
                //if (Cuenta.P_Superficie_Construida != 0)
                //{
                //}
                //if (Cuenta.P_Superficie_Total != 0)
                //{
                //}
                //if (Cuenta.P_Clave_Catastral != null && Cuenta.P_Clave_Catastral != "")
                //{
                //}
                //if (Cuenta.P_Valor_Fiscal != 0)
                //{
                //}
                //if (Cuenta.P_Efectos != null && Cuenta.P_Efectos != "")
                //{
                //}
                //if (Cuenta.P_Periodo_Corriente != null && Cuenta.P_Periodo_Corriente != "")
                //{
                //}
                //if (Cuenta.P_Cuota_Anual != 0)
                //{
                //}
                //if (Cuenta.P_Porcentaje_Exencion != 0)
                //{
                //}
                //if (Cuenta.P_Cuota_Fija != null && Cuenta.P_Cuota_Fija != "")
                //{
                //}
                //if (Cuenta.P_Termino_Exencion != null)
                //{
                //    if (Cuenta.P_Termino_Exencion > DateTime.MinValue)
                //    {
                //    }
                //}
                //if (Cuenta.P_Fecha_Avaluo != null)
                //{
                //    if (Cuenta.P_Fecha_Avaluo > DateTime.MinValue)
                //    {
                //    }
                //}
                //if (Cuenta.P_Diferencia_Construccion != 0)
                //{
                //}
                //if (Cuenta.P_Costo_M2 != 0)
                //{
                //}
                //if (Cuenta.P_No_Cuota_Fija != null && Cuenta.P_No_Cuota_Fija != "")
                //{
                //    if (Cuenta.P_No_Cuota_Fija == "NULL")
                //        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + " = NULL,";
                //    else
                //        ;
                //}

                //if (Cuenta.P_Calle_ID_Notificacion != null && Cuenta.P_Calle_ID_Notificacion != "")
                //{
                //}
                //if (Cuenta.P_Colonia_ID_Notificacion != null && Cuenta.P_Colonia_ID_Notificacion != "")
                //{
                //}
                //if (Cuenta.P_Estado_ID_Notificacion != null && Cuenta.P_Estado_ID_Notificacion != "")
                //{
                //}
                //if (Cuenta.P_Ciudad_ID_Notificacion != null && Cuenta.P_Ciudad_ID_Notificacion != "")
                //{
                //}
                //if (Cuenta.P_Domicilio_Foraneo != null && Cuenta.P_Domicilio_Foraneo != "")
                //{
                //}
                //if (Cuenta.P_Calle_Notificacion != null && Cuenta.P_Calle_Notificacion != "")
                //{
                //}
                //if (Cuenta.P_No_Exterior_Notificacion != null && Cuenta.P_No_Exterior_Notificacion != "")
                //{
                //}
                //if (Cuenta.P_Codigo_Postal != null && Cuenta.P_Codigo_Postal != "")
                //{
                //}
                //if (Cuenta.P_No_Interior_Notificacion != null && Cuenta.P_No_Interior_Notificacion != "")
                //{
                //}
                //if (Cuenta.P_Colonia_Notificacion != null && Cuenta.P_Colonia_Notificacion != "")
                //{
                //}
                //if (Cuenta.P_No_Diferencia != null && Cuenta.P_No_Diferencia != "")
                //{
                //    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_No_Diferencia + " = '" + Cuenta.P_No_Diferencia + "', ";
                //}
                //if (Cuenta.P_Estado_Notificacion != null && Cuenta.P_Estado_Notificacion != "")
                //{
                //}
                //if (Cuenta.P_Ciudad_Notificacion != null && Cuenta.P_Ciudad_Notificacion != "")
                //{
                //}
                //if (Cuenta.P_Tipo_Suspencion != null && Cuenta.P_Tipo_Suspencion != "")
                //{
                //    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + " = '" + Cuenta.P_Tipo_Suspencion + "', ";
                //}
                if (Cuenta.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Modificar = true;
            }
            catch (OracleException Ex)
            {
                if (Cuenta.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar modificar la Cuenta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Cuenta.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Modificar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Estatus_Adeudos
        ///DESCRIPCIÓN          : Revisa la diferencia entre lo Pagado contra lo Adeudado para determinar el Estatus correcto del Adeudo.
        ///PARAMETROS           : Cuenta, instancia de Cls_Cat_Pre_Cuentas_Predial_Negocio
        ///CREO                 : Antonio Salvador Benavides Guarddado
        ///FECHA_CREO           : 16/Enero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Validar_Estatus_Adeudos(Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Modificar;
            String Mi_SQL;
            //DataTable Tabla;

            if (Cuenta.P_Cmmd != null)
            {
                Cmd = Cuenta.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }

            Modificar = false;
            try
            {
                Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " ";
                Mi_SQL += "SET " + Ope_Pre_Adeudos_Predial.Campo_Estatus + " = CASE WHEN (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ", 0)) > (NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ", 0)) THEN 'POR PAGAR' ELSE 'PAGADO' END ";
                Mi_SQL += "WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta.P_Adeudo_Predial_Cuenta_Predial_ID + "' ";
                if (Cuenta.P_Adeudo_Predial_Anio != 0)
                {
                    Mi_SQL += "AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Cuenta.P_Adeudo_Predial_Anio;
                }
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Cuenta.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                //Mi_SQL = "SELECT ";
                //Mi_SQL += "(NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ", 0)) AS SUM_ADEUDOS, ";
                //Mi_SQL += "(NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ", 0) + NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ", 0)) AS SUM_PAGOS, ";
                //Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Monto_Por_Pagar;
                //Mi_SQL += "FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                //Mi_SQL += "WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta.P_Adeudo_Predial_Cuenta_Predial_ID + "'";
                //Mi_SQL += "AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Cuenta.P_Adeudo_Predial_Anio;

                //DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //if (dataSet != null)
                //{
                //    Tabla = dataSet.Tables[0];
                //    if (Tabla.Rows != null)
                //    {
                //        if (Tabla.Rows.Count > 0)
                //        {
                //            Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                //            if (Convert.ToDecimal(Tabla.Rows[0]["SUM_ADEUDOS"]) > Convert.ToDecimal(Tabla.Rows[0]["SUM_PAGOS"]))
                //            {
                //                Mi_SQL += " SET " + Ope_Pre_Adeudos_Predial.Campo_Estatus + " = 'POR PAGAR'";
                //            }
                //            else
                //            {
                //                Mi_SQL += " SET " + Ope_Pre_Adeudos_Predial.Campo_Estatus + " = 'PAGADO'";
                //            }
                //            Mi_SQL += "WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta.P_Adeudo_Predial_Cuenta_Predial_ID + "'";
                //            Mi_SQL += "AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Cuenta.P_Adeudo_Predial_Anio;
                //            Cmd.CommandText = Mi_SQL;
                //            Cmd.ExecuteNonQuery();
                //            if (Cuenta.P_Cmmd == null)
                //            {
                //                Trans.Commit();
                //            }
                //        }
                //    }
                //}
                Modificar = true;
            }
            catch (OracleException Ex)
            {
                if (Cuenta.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar modificar el Estatus del Adeudo de la Cuenta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Cuenta.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Modificar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Cuentas
        ///DESCRIPCIÓN          : Obtiene las cuentas de acuerdo a los filtros establecidos en la interfaz
        ///PARAMETROS           : Cuenta, instancia de Cls_Cat_Pre_Cuentas_Predial_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 07/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cuentas(Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            String Mi_SQL_Campos_Foraneos = "";
            try
            {
                if (Cuenta.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL_Campos_Foraneos += "CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                    Mi_SQL_Campos_Foraneos += "COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                    Mi_SQL_Campos_Foraneos += "CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE_NOTIFICACION, ";
                    Mi_SQL_Campos_Foraneos += "COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA_NOTIFICACION, ";
                    Mi_SQL_Campos_Foraneos += "ESTADOS." + Cat_Pre_Estados.Campo_Nombre + " AS NOMBRE_ESTADO_CUENTA, ";
                    Mi_SQL_Campos_Foraneos += "CIUDADES." + Cat_Pre_Ciudades.Campo_Nombre + " AS NOMBRE_CIUDAD_CUENTA, ";
                    if (Cuenta.P_Incluir_Propietarios)
                    {
                        Mi_SQL_Campos_Foraneos += "PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS PROPIETARIO_ID, ";
                        Mi_SQL_Campos_Foraneos += "(PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_PROPIETARIO, ";
                    }
                    if (Cuenta.P_Incluir_Copropietarios)
                    {
                        Mi_SQL_Campos_Foraneos += "COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS COPROPIETARIO_ID, ";
                        Mi_SQL_Campos_Foraneos += "(COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_COPROPIETARIO, ";
                    }
                    Mi_SQL_Campos_Foraneos += Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + "." + Cat_Pre_Estados_Predio.Campo_Descripcion + " AS DESCRIPCION_ESTADO_PREDIO, ";
                    Mi_SQL_Campos_Foraneos += Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS DESCRIPCION_TIPO_PREDIO, ";
                    Mi_SQL_Campos_Foraneos += Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo + "." + Cat_Pre_Uso_Suelo.Campo_Descripcion + " AS DESCRIPCION_USO_SUELO, ";
                    Mi_SQL_Campos_Foraneos += Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " AS IMPUESTO_PREDIAL, ";
                    Mi_SQL_Campos_Foraneos += Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + "." + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " AS CUOTA_MINIMA, ";
                    if (Cuenta.P_Join == null)
                    {
                        Cuenta.P_Join = "";
                    }
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_PREDIOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = CALLES_PREDIOS." + Cat_Pre_Calles.Campo_Calle_ID + " ";
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_NOTIFICACION ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Calle_ID + " ";
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_PREDIOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = COLONIAS_PREDIOS." + Cat_Ate_Colonias.Campo_Colonia_ID + " ";
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS_NOTIFICACION ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = COLONIAS_NOTIFICACION." + Cat_Ate_Colonias.Campo_Colonia_ID + " ";
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " ESTADOS ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = ESTADOS." + Cat_Pre_Estados.Campo_Estado_ID + " ";
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + " CIUDADES ON ESTADOS." + Cat_Pre_Estados.Campo_Estado_ID + " = CIUDADES." + Cat_Pre_Ciudades.Campo_Estado_ID + " ";
                    Cuenta.P_Join += " AND CIUDADES." + Cat_Pre_Ciudades.Campo_Ciudad_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " ";
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " ";
                    if (Cuenta.P_Tipo_Propietario != null && Cuenta.P_Tipo_Propietario != "")
                    {
                        Cuenta.P_Join += " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + Validar_Operador_Comparacion(Cuenta.P_Tipo_Propietario);
                    }
                    if (Cuenta.P_Incluir_Propietarios)
                    {
                        Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " PROPIETARIOS ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')";
                    }
                    if (Cuenta.P_Incluir_Copropietarios)
                    {
                        Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " COPROPIETARIOS ON " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = COPROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " = 'COPROPIETARIO'";
                    }
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + " = " + Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + "." + Cat_Pre_Estados_Predio.Campo_Estado_Predio_ID + " ";
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " = " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " ";
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + " = " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo + "." + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + " ";
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + " = " + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + "." + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " ";
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " ON " + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + "." + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " = " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID + " AND " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + " = " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " AND " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + "." + Cat_Pre_Tasas_Predial_Anual.Campo_Año + " = " + DateTime.Now.Year.ToString();
                    Cuenta.P_Join += "LEFT OUTER JOIN " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " ON " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + " = " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + "." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " ";
                }
                if (Cuenta.P_Campos_Dinamicos != null && Cuenta.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos + Cuenta.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos;
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Diferencia + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ", ";
                    if (Mi_SQL.EndsWith(", "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 2);
                    }
                }
                Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                if (Cuenta.P_Join != null && Cuenta.P_Join != "")
                {
                    Mi_SQL += " " + Cuenta.P_Join;
                }
                if (Cuenta.P_Filtros_Dinamicos != null && Cuenta.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Cuenta.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Cuenta.P_Cuenta_Predial_ID != null && Cuenta.P_Cuenta_Predial_ID != "")
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta.P_Cuenta_Predial_ID + "' AND ";
                    }
                    if (Cuenta.P_Cuenta_Predial != null && Cuenta.P_Cuenta_Predial != "")
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Cuenta.P_Cuenta_Predial + "%' AND ";
                    }
                    if (Cuenta.P_Estatus != null && Cuenta.P_Estatus != "")
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + Validar_Operador_Comparacion(Cuenta.P_Estatus) + " AND ";
                    }
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                }
                if (Cuenta.P_Agrupar_Dinamico != null && Cuenta.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Cuenta.P_Agrupar_Dinamico;
                }
                if (Cuenta.P_Ordenar_Dinamico != null && Cuenta.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Cuenta.P_Ordenar_Dinamico;
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Reporte
        ///DESCRIPCIÓN          : Obtiene las cuentas de acuerdo a los filtros establecidos en la interfaz
        ///PARAMETROS           : Cuenta, instancia de Cls_Cat_Pre_Cuentas_Predial_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 11/Enero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Datos_Reporte(Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            try
            {
                if (Cuenta.P_Campos_Dinamicos != null && Cuenta.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Cuenta.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal;
                }
                Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                if (Cuenta.P_Unir_Tablas != null && Cuenta.P_Unir_Tablas != "")
                {
                    Mi_SQL += ", " + Cuenta.P_Unir_Tablas;
                }
                else
                {
                    if (Cuenta.P_Join != null && Cuenta.P_Join != "")
                    {
                        Mi_SQL += " " + Cuenta.P_Join;
                    }
                }
                if (Cuenta.P_Filtros_Dinamicos != null && Cuenta.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Cuenta.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Cuenta.P_Cuenta_Predial_ID != null && Cuenta.P_Cuenta_Predial_ID != "")
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta.P_Cuenta_Predial_ID + "' AND ";
                    }
                    if (Cuenta.P_Cuenta_Predial != null && Cuenta.P_Cuenta_Predial != "")
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + Validar_Operador_Comparacion(Cuenta.P_Cuenta_Predial) + " AND ";
                    }
                    if (Cuenta.P_Estatus != null && Cuenta.P_Estatus != "")
                    {
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + Validar_Operador_Comparacion(Cuenta.P_Estatus) + " AND ";
                    }
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                }
                if (Cuenta.P_Agrupar_Dinamico != null && Cuenta.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Cuenta.P_Agrupar_Dinamico;
                }
                if (Cuenta.P_Ordenar_Dinamico != null && Cuenta.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Cuenta.P_Ordenar_Dinamico;
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Cuenta
        ///DESCRIPCIÓN          : Elimina una Cuenta según el id indicado
        ///PARAMETROS           : Cuenta, instancia de Cls_Cat_Pre_Cuentas_Predial_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 07/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Eliminar_Cuenta(Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Eliminar;

            Eliminar = false;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL += " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta.P_Cuenta_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Eliminar = true;
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos";
                }
                else
                {
                    Mensaje = "Error al intentar eliminar la Cuenta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar la Cuenta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos" //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Eliminar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Propietario
        ///DESCRIPCIÓN: Obtiene a detalle una Caja.
        ///PARAMENTROS:   
        ///             1. P_Caja.   Caja que se va ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Cuentas_Predial_Negocio Consultar_Datos_Propietario(Cls_Cat_Pre_Cuentas_Predial_Negocio P_Cuenta)
        {
            Cls_Cat_Pre_Cuentas_Predial_Negocio R_Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            String Mi_SQL = "SELECT c." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", c." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID;
            Mi_SQL = Mi_SQL + ", c." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " AS " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior;
            Mi_SQL = Mi_SQL + ", c." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " AS " + Cat_Pre_Cuentas_Predial.Campo_No_Interior;
            Mi_SQL = Mi_SQL + ", e." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "||' '||e." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "||' '||e." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS " + Cat_Pre_Contribuyentes.Campo_Nombre;
            Mi_SQL = Mi_SQL + ", e." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID;
            Mi_SQL = Mi_SQL + ", p." + Cat_Pre_Propietarios.Campo_Propietario_ID;
            Mi_SQL = Mi_SQL + ", e." + Cat_Pre_Contribuyentes.Campo_RFC + " AS " + Cat_Pre_Contribuyentes.Campo_RFC;
            Mi_SQL = Mi_SQL + ", ca." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE";
            Mi_SQL = Mi_SQL + ", co." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " c";
            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " ca ON c." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = ca." + Cat_Pre_Calles.Campo_Calle_ID + "";
            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " co ON c." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = co." + Cat_Ate_Colonias.Campo_Colonia_ID + "";
            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " p ON c." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = p." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + "";
            Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " e ON p." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = e." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "";
            Mi_SQL = Mi_SQL + " WHERE p." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO','POSEEDOR')";
            Mi_SQL = Mi_SQL + " AND c." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + P_Cuenta.P_Cuenta_Predial_ID + "'";

            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Cuenta.P_Cuenta_Predial_ID = P_Cuenta.P_Cuenta_Predial_ID;
                while (Data_Reader.Read())
                {
                    R_Cuenta.P_No_Exterior = Data_Reader[Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
                    R_Cuenta.P_No_Interior = Data_Reader[Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
                    R_Cuenta.P_Nombre_Calle = Data_Reader["NOMBRE_CALLE"].ToString();
                    R_Cuenta.P_Nombre_Colonia = Data_Reader["NOMBRE_COLONIA"].ToString();
                    R_Cuenta.P_Nombre_Propietario = Data_Reader[Cat_Pre_Contribuyentes.Campo_Nombre].ToString();
                    R_Cuenta.P_RFC_Propietario = Data_Reader[Cat_Pre_Contribuyentes.Campo_RFC].ToString();
                    R_Cuenta.P_Propietario_ID = Data_Reader[Cat_Pre_Contribuyentes.Campo_Contribuyente_ID].ToString();
                    R_Cuenta.P_Pro_Propietario_ID = Data_Reader[Cat_Pre_Propietarios.Campo_Propietario_ID].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Cuenta predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Cuenta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Cuenta_Existente
        ///DESCRIPCIÓN          : Obtiene Si hay una cuenta en la tabla con la misma cuenta predial.
        ///PARAMETROS           : Cuenta, instancia de Cls_Cat_Pre_Cuentas_Predial_Negocio
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 22/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Consultar_Cuenta_Existente(Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            Boolean Existe = false;
            try
            {
                ////////////////////////////////////////////////////////////////
                Mi_SQL = "SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + "='" + Cuenta.P_Cuenta_Predial + "'";
                ////////////////////////////////////////////////////////////////
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                    if (Tabla.Rows.Count != 0)
                    {
                        Existe = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Existe;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARÁMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
        {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
            {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Operador_Comparacion
        ///DESCRIPCIÓN          : Devuelve una cadena adecuada al operador indicado en la capa de Negocios
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Validar_Operador_Comparacion(String Filtro)
        {
            String Cadena_Validada;
            if (Filtro.Trim().StartsWith("<")
               || Filtro.Trim().StartsWith(">")
               || Filtro.Trim().StartsWith("<>")
               || Filtro.Trim().StartsWith("<=")
               || Filtro.Trim().StartsWith(">=")
               || Filtro.Trim().StartsWith("=")
               || Filtro.Trim().ToUpper().StartsWith("BETWEEN")
               || Filtro.Trim().ToUpper().StartsWith("LIKE")
               || Filtro.Trim().ToUpper().StartsWith("IN"))
            {
                Cadena_Validada = " " + Filtro + " ";
            }
            else
            {
                if (Filtro.Trim().ToUpper().StartsWith("NULL"))
                {
                    Cadena_Validada = " IS " + Filtro + " ";
                }
                else
                {
                    Cadena_Validada = " = '" + Filtro + "' ";
                }
            }
            return Cadena_Validada;
        }
    }
}