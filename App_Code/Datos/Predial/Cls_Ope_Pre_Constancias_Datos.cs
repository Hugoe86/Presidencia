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
using Presidencia.Operacion_Predial_Constancias.Negocio;
using Presidencia.Catalogo_Tipos_Constancias.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio ;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Pre_Constancias_Datos
/// </summary>

namespace Presidencia.Operacion_Predial_Constancias.Datos
{

    public class Cls_Ope_Pre_Constancias_Datos
    {
        #region Metodos
        internal static DateTime Calcular_Fecha(String P_Fecha, String Dias_Habiles)
        {
            String Dia = "";
            DateTime Fecha_Inicial;
            DateTime Fecha;
            Int32 Cantidad_Dias_Habiles;
            DataTable Dt_Dia_Festivo;
            Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Negocio = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
            try
            {
                Fecha_Inicial = Convert.ToDateTime(P_Fecha);
                Fecha = Fecha_Inicial;

                Cantidad_Dias_Habiles = Convert.ToInt32(Dias_Habiles);
                for (int i = 1; i <= Cantidad_Dias_Habiles; i++)
                {
                    Fecha = Fecha.AddDays(1);
                    Dia = Fecha.ToString("dddd");
                    Dias_Negocio.P_Anio = "3000";
                    Dias_Negocio.P_Fecha_Inicial_Busqueda = Fecha.ToString("dd/MM/yyyy");
                    Dias_Negocio.P_Fecha_Final_Busqueda = Fecha.ToString("dd/MM/yyyy");

                    Dt_Dia_Festivo = Dias_Negocio.Consultar_Dias();

                    if (Dia == "sábado" || Dia == "sabado" || Dia == "domingo" || Dia == "saturday" || Dia == "sunday" || Dt_Dia_Festivo.Rows.Count > 0)
                    {
                        i--;
                    }
                }
                return Fecha;
            }
            catch (Exception Ex)
            {
                throw new Exception("Ocurrio un Error al calcular la Fecha Error: " + Ex.Message);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Constancia
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de Constancia
        ///PARAMETROS           : 1. Constancia.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Constancias_Negocio
        ///                                 con los datos de Constancias que va a ser dado de Alta.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 29/Junio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Constancia(Cls_Ope_Pre_Constancias_Negocio Constancia)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Alta = false;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String No_Constancia = Obtener_ID_Consecutivo(Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias, Ope_Pre_Constancias.Campo_No_Constancia, Ope_Pre_Constancias.Campo_Tipo_Constancia_ID + " = '" + Constancia.P_Tipo_Constancia_ID + "' AND " + Ope_Pre_Constancias.Campo_Anio + "=" + DateTime.Now.Year, 10);
            //String Folio = Obtener_ID_Consecutivo(Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias, Ope_Pre_Constancias.Campo_Folio, Ope_Pre_Constancias.Campo_Tipo_Constancia_ID + " = " + Constancia.P_Tipo_Constancia_ID, 10);
            String Folio = Obtener_Clave_Tipo_Constancia(Constancia.P_Tipo_Constancia_ID) + Convert.ToString(Convert.ToInt32(No_Constancia)) + Convert.ToString(DateTime.Now.Year);
            //String No_Recibo = Obtener_ID_Consecutivo(Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias, Ope_Pre_Constancias.Campo_No_Recibo, 10);
            try
            {
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + " (";
                Mi_SQL += Ope_Pre_Constancias.Campo_No_Constancia + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Tipo_Constancia_ID + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Propietario_ID + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Realizo + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Confronto + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Documento_ID + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Folio + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_No_Recibo + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Fecha + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Fecha_Vencimiento  + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Periodo_Año + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Periodo_Bimestre + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Periodo_Hasta_Anio + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Periodo_Hasta_Bimestre + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Estatus + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Observaciones + ", ";
                //Mi_SQL += Ope_Pre_Constancias.Campo_Leyenda_Certificacion + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Solicitante + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Solicitante_RFC + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Domicilio + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Usuario_Creo + ", ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Fecha_Creo + ", "+Ope_Pre_Constancias.Campo_Anio+") ";
                Mi_SQL += "VALUES ('";
                Mi_SQL += No_Constancia + "', '";
                Mi_SQL += Constancia.P_Tipo_Constancia_ID + "', ";
                if (Constancia.P_Cuenta_Predial_ID != "" && Constancia.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += "'" + Constancia.P_Cuenta_Predial_ID + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Propietario_ID != "" && Constancia.P_Propietario_ID != null)
                {
                    Mi_SQL += "'" + Constancia.P_Propietario_ID + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Realizo != "" && Constancia.P_Realizo != null)
                {
                    Mi_SQL += "'" + Constancia.P_Realizo + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Confronto != "" && Constancia.P_Confronto != null)
                {
                    Mi_SQL += "'" + Constancia.P_Confronto + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Documento_ID != "" && Constancia.P_Documento_ID != null)
                {
                    Mi_SQL += "'" + Constancia.P_Documento_ID + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Folio != "" && Constancia.P_Folio != null)
                {
                    Mi_SQL += "'" + Constancia.P_Folio + "', ";
                }
                else
                {
                    Mi_SQL += "'" + Folio + "', ";
                }
                if (Constancia.P_No_Recibo != "" && Constancia.P_No_Recibo != null)
                {
                    Mi_SQL += "'" + Constancia.P_No_Recibo + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Fecha.ToString() != "" && Constancia.P_Fecha.ToString() != null)
                {
                    Mi_SQL += "'" + String.Format("{0:d-M-yyyy}", Constancia.P_Fecha) + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Fecha_Vencimiento.ToString() != "" && Constancia.P_Fecha_Vencimiento.ToString() != null)
                {
                    Mi_SQL += "'" + String.Format("{0:d-M-yyyy}", Constancia.P_Fecha_Vencimiento) + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Periodo_Año != 0 && Constancia.P_Periodo_Año != null)
                {
                    Mi_SQL += Constancia.P_Periodo_Año + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Periodo_Bimestre != 0 && Constancia.P_Periodo_Bimestre != null)
                {
                    Mi_SQL += Constancia.P_Periodo_Bimestre + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Periodo_Hasta_Anio != 0 && Constancia.P_Periodo_Hasta_Anio != null)
                {
                    Mi_SQL += Constancia.P_Periodo_Hasta_Anio + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Periodo_Hasta_Bimestre != 0 && Constancia.P_Periodo_Hasta_Bimestre != null)
                {
                    Mi_SQL += Constancia.P_Periodo_Hasta_Bimestre + ", ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += "'" + Constancia.P_Estatus + "', '";
                Mi_SQL += Constancia.P_Observaciones + "', ";
                //if (Constancia.P_Leyenda_Certificacion != "" && Constancia.P_Leyenda_Certificacion != null)
                //{
                //    Mi_SQL += "'" + Constancia.P_Leyenda_Certificacion + "', ";
                //}
                //else
                //{
                //    Mi_SQL += "NULL, ";
                //}
                if (Constancia.P_Solicitante != "" && Constancia.P_Solicitante != null)
                {
                    Mi_SQL += "'" + Constancia.P_Solicitante + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Solicitante_RFC != "" && Constancia.P_Solicitante_RFC != null)
                {
                    Mi_SQL += "'" + Constancia.P_Solicitante_RFC + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Constancia.P_Domicilio != null)
                {
                    Mi_SQL += "'" + Constancia.P_Domicilio + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += "'" + Constancia.P_Usuario + "', SYSDATE, "+DateTime.Now.Year+")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Alta = true;
                Constancia.P_Folio = Folio;
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Constancia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Constancia
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Constancia
        ///PARAMETROS          : 1. Constancia.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Constancias_Negocio
        ///                                 con los datos del Constancias que va a ser Modificado.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 29/Junio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Constancia(Cls_Ope_Pre_Constancias_Negocio Constancia)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Actualizar = false;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + " SET ";
                //Mi_SQL += Ope_Pre_Constancias.Campo_Tipo_Constancia_ID + " = '" + Constancia.P_Tipo_Constancia_ID + "', ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + " = '" + Constancia.P_Cuenta_Predial_ID + "', ";
                //Mi_SQL += Ope_Pre_Constancias.Campo_Propietario_ID + " = '" + Constancia.P_Propietario_ID + "', ";
                if (Constancia.P_Realizo != "" && Constancia.P_Realizo != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Realizo + " = '" + Constancia.P_Realizo + "', ";
                }
                if (Constancia.P_Confronto != "" && Constancia.P_Confronto != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Confronto + " = '" + Constancia.P_Confronto + "', ";
                }
                if (Constancia.P_Documento_ID != "" && Constancia.P_Documento_ID != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Documento_ID + " = '" + Constancia.P_Documento_ID + "', ";
                }
                if (Constancia.P_Solicitante != "" && Constancia.P_Solicitante != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Solicitante + " = '" + Constancia.P_Solicitante + "', ";
                }
                if (Constancia.P_Solicitante_RFC != "" && Constancia.P_Solicitante_RFC != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Solicitante_RFC + " = '" + Constancia.P_Solicitante_RFC + "', ";
                }
                //if (Constancia.P_Folio != "" && Constancia.P_Folio != null)
                //{
                //    Mi_SQL += Ope_Pre_Constancias.Campo_Folio + " = '" + Constancia.P_Folio + "', ";
                //}
                if (Constancia.P_No_Recibo != "" && Constancia.P_No_Recibo != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_No_Recibo + " = '" + Constancia.P_No_Recibo + "', ";
                }
                //Mi_SQL += Ope_Pre_Constancias.Campo_Fecha + " = '" + Constancia.P_Fecha.ToShortDateString() + "', ";
                if (Constancia.P_Periodo_Año != 0 && Constancia.P_Periodo_Año != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Periodo_Año + " = " + Constancia.P_Periodo_Año + ", ";
                }
                if (Constancia.P_Periodo_Hasta_Anio != 0 && Constancia.P_Periodo_Hasta_Anio!=null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Periodo_Hasta_Anio + " = " + Constancia.P_Periodo_Hasta_Anio + ", ";
                }
                if (Constancia.P_Periodo_Bimestre != 0 && Constancia.P_Periodo_Bimestre != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Periodo_Bimestre + " = " + Constancia.P_Periodo_Bimestre + ", ";
                }
                if (Constancia.P_Periodo_Hasta_Bimestre != 0 && Constancia.P_Periodo_Hasta_Bimestre != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Periodo_Hasta_Bimestre + " = " + Constancia.P_Periodo_Hasta_Bimestre + ", ";
                }
                Mi_SQL += Ope_Pre_Constancias.Campo_Estatus + " = '" + Constancia.P_Estatus + "', ";
                if (Constancia.P_Observaciones != "" && Constancia.P_Observaciones != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Observaciones + " = '" + Constancia.P_Observaciones + "', ";
                }
                if (Constancia.P_Domicilio != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Domicilio + " = '" + Constancia.P_Domicilio + "', ";
                }
                //if (Constancia.P_Leyenda_Certificacion != "" && Constancia.P_Leyenda_Certificacion != null)
                //{
                //    Mi_SQL += Ope_Pre_Constancias.Campo_Leyenda_Certificacion + " = '" + Constancia.P_Leyenda_Certificacion + "', ";
                //}
                Mi_SQL += Ope_Pre_Constancias.Campo_Usuario_Modifico + " = '" + Constancia.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE " + Ope_Pre_Constancias.Campo_Folio + " = '" + Constancia.P_Folio + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Actualizar = true;
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
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
                    Mensaje = "Error al intentar modificar un Registro de Constancia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Constancia_Impresa
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Constancia en el campo estatus, para indicar que ya se imprimio la constancia.
        ///PARAMETROS          : 1. Constancia.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Constancias_Negocio
        ///                                 con los datos del Constancias que va a ser Modificado.
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 23/Noviembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Constancia_Impresa(Cls_Ope_Pre_Constancias_Negocio Constancia)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Actualizar = false;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + " SET ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Estatus + " = 'IMPRESA' ";
                Mi_SQL += "WHERE " + Ope_Pre_Constancias.Campo_Folio + " = '" + Constancia.P_Folio + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Actualizar = true;
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
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
                    Mensaje = "Error al intentar modificar un Registro de Constancia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Incrementar_No_Impresiones_Constancia
        ///DESCRIPCIÓN          : Incrementa el número de Imopresiones realizadas en uno
        ///PARAMETROS          : 1. Constancia.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Constancias_Negocio
        ///                                 con los datos del Constancias que va a ser Modificado.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 08/Septiembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Incrementar_No_Impresiones_Constancia(Cls_Ope_Pre_Constancias_Negocio Constancia)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Actualizar = false;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + " SET ";
                Mi_SQL += Ope_Pre_Constancias.Campo_No_Impresiones + " = NVL(" + Ope_Pre_Constancias.Campo_No_Impresiones + ", 0) + 1, ";
                //Mi_SQL += Ope_Pre_Constancias.Campo_Estatus + " = '" + Constancia.P_Estatus + "', ";
                if (Constancia.P_Observaciones != "" && Constancia.P_Observaciones != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Observaciones + " = '" + Constancia.P_Observaciones + "', ";
                }
                Mi_SQL += Ope_Pre_Constancias.Campo_Usuario_Modifico + " = '" + Constancia.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Constancias.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += "WHERE ";
                if (Constancia.P_No_Constancia != "" && Constancia.P_No_Constancia != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_No_Constancia + " = '" + Constancia.P_No_Constancia + "' AND ";
                }
                if (Constancia.P_Folio != "" && Constancia.P_Folio != null)
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_Folio + " = '" + Constancia.P_Folio + "' AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Actualizar = true;
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
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
                    Mensaje = "Error al intentar modificar un Registro de Constancia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Constancias
        ///DESCRIPCIÓN          : Obtiene todos las Constancia que estan dadas de alta en la base de datos
        ///PARAMETROS           : 1. Constancia.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Constancias_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 29/Junio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Constancias(Cls_Ope_Pre_Constancias_Negocio Constancia)
        {
            DataTable Dt_Constancias = new DataTable();
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT ";
                if (Constancia.P_Campos_Foraneos)
                {
                    Mi_SQL += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ") AS Cuenta_Predial, ";
                    Mi_SQL += "(SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Propietario_ID + " = " + Ope_Pre_Constancias .Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Propietario_ID + " AND "+Cat_Pre_Propietarios.Campo_Tipo+" IN('PROPIETARIO','POSEEDOR'))) AS Nombre_Propietario, ";
                    //Mi_SQL += "(SELECT " + Cat_Pre_Contribuyentes.Campo_Domicilio + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Propietario_ID + " = " + Ope_Pre_Constancias .Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Propietario_ID + ")) AS Domicilio_Propietario, ";
                    Mi_SQL += "(SELECT " + Cat_Pre_Contribuyentes.Campo_RFC + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Propietario_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Propietario_ID + ")) AS RFC_Propietario, ";
                    Mi_SQL += "(SELECT " + Cat_Empleados.Campo_Nombre + " || ' ' || " + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " + Cat_Empleados.Campo_Apellido_Materno + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Realizo + ") AS Nombre_Realizo, ";
                    Mi_SQL += "(SELECT " + Cat_Empleados.Campo_Nombre + " || ' ' || " + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || " + Cat_Empleados.Campo_Apellido_Materno + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Confronto + ") AS Nombre_Confronto, ";
                    Mi_SQL += "(SELECT " + Cat_Pre_Tipos_Documento.Campo_Nombre_Documento + " FROM " + Cat_Pre_Tipos_Documento.Tabla_Cat_Pre_Tipos_Documento + " WHERE " + Cat_Pre_Tipos_Documento.Campo_Documento_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Documento_ID + ") AS Nombre_Documento, ";
                }
                if (Constancia.P_Campos_Dinamicos != null && Constancia.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Constancia.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Constancias.Campo_No_Constancia + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_Tipo_Constancia_ID + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_Propietario_ID + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_Realizo + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_Confronto + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_Documento_ID + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_Folio + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_No_Recibo + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_No_Impresiones+ ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_Fecha + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_Periodo_Año + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_Periodo_Bimestre + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_Estatus + ", ";
                    Mi_SQL += Ope_Pre_Constancias.Campo_Observaciones + ",";
                    //Mi_SQL += Ope_Pre_Constancias.Campo_Leyenda_Certificacion + " AS Leyenda_Certificacion";
                }
                Mi_SQL += " FROM " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias;
                if (Constancia.P_Filtros_Dinamicos != null && Constancia.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Constancia.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Constancia.P_No_Constancia != "" && Constancia.P_No_Constancia != null)
                    {
                        Mi_SQL += Ope_Pre_Constancias.Campo_No_Constancia + " = '" + Constancia.P_No_Constancia + "'";
                    }
                    if (Constancia.P_Tipo_Constancia_ID != "" && Constancia.P_Tipo_Constancia_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Constancias.Campo_Tipo_Constancia_ID + " = '" + Constancia.P_Tipo_Constancia_ID + "'";
                    }
                    if (Constancia.P_Cuenta_Predial_ID != "" && Constancia.P_Cuenta_Predial_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + " = '" + Constancia.P_Cuenta_Predial_ID + "'";
                    }
                    if (Constancia.P_Propietario_ID != "" && Constancia.P_Propietario_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Constancias.Campo_Propietario_ID + " = '" + Constancia.P_Propietario_ID + "'";
                    }
                    if (Constancia.P_Folio != "" && Constancia.P_Folio != null)
                    {
                        Mi_SQL += Ope_Pre_Constancias.Campo_Folio + Validar_Operador_Comparacion(Constancia.P_Folio);
                    }
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    //DESCOMENTAR EL SIGUIENTE BLOQUE WHERE SI SE QUITA EL CAMPO CONCEPTO_PREDIAL_ID DE LA LÍNEA DEL WHERE
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                }
                if (Constancia.P_Agrupar_Dinamico != null && Constancia.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Constancia.P_Agrupar_Dinamico;
                }
                if (Constancia.P_Ordenar_Dinamico != null && Constancia.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Constancia.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Constancias.Campo_Folio;
                }

                DataSet Ds_Constancias = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Constancias != null)
                {
                    Dt_Constancias = Ds_Constancias.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Constancia. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Constancias;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, String Filtro, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                if (Filtro != "" && Filtro != null)
                {
                    Mi_SQL += " WHERE " + Filtro;
                }
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
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
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
        ///NOMBRE DE LA FUNCIÓN : Obtener_Clave_Tipo_Constaqncia
        ///DESCRIPCIÓN          : Obtiene la Clave del Tipo de Constancia indicado
        ///PARAMETROS           : 1. Tipo_Constancia_ID.   Id del Tipo de Constancia a buscar
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 21/Julio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private static String Obtener_Clave_Tipo_Constancia(String Tipo_Constancia_ID)
        {
            DataTable Dt_Tipo_Constancia;
            String Clave = "";
            Cls_Cat_Pre_Tipos_Constancias_Negocio Tipo_Constancia = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
            Tipo_Constancia.P_Campos_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Clave;
            Tipo_Constancia.P_Filtros_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + " = '" + Tipo_Constancia_ID + "'";
            Dt_Tipo_Constancia = Tipo_Constancia.Consultar_Tipos_Constancias();

            if (Dt_Tipo_Constancia.Rows.Count > 0)
            {
                Clave = Dt_Tipo_Constancia.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Clave].ToString().Trim();
            }

            return Clave;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Operador_Comparacion
        ///DESCRIPCIÓN          : Devuelve una cadena adecuada al operador indicado en la capa de Negocios
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 11/Agosto/2011
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
                Cadena_Validada = " = '" + Filtro + "' ";
            }
            return Cadena_Validada;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Alta_Pasivo
        /// DESCRIPCIÓN: Insercion de adeudo en tabla Ope_Ing_Pasivo
        /// PARÁMETROS:
        /// 	1. Datos: Instancia de la clase de negocio con los datos que serán insertados en la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 24-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Alta_Pasivo(Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Datos, Cls_Ope_Pre_Constancias_Negocio Contribuyente)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Int32 Filas_Afectadas = 0;
            String Mensaje = "";

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            try
            {
                // abrir conexion con la base de datos si no llego una conexion como parametro 
                if (Datos.P_Cmd_Calculo != null)
                {
                    Comando = Datos.P_Cmd_Calculo;
                }
                else
                {
                    Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Conexion;
                    Comando.Transaction = Transaccion;
                }

                //Consulta para la inserción del Adeudo como pasivo
                Mi_SQL = "INSERT INTO " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                Mi_SQL = Mi_SQL + "(" + Ope_Ing_Pasivo.Campo_No_Pasivo;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Referencia;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Monto;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Recargos;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ", " + Ope_Ing_Pasivo.Campo_Contribuyente;
                Mi_SQL = Mi_SQL + ") VALUES(" +
                    Obtener_Consecutivo_Pasivos(Ope_Ing_Pasivo.Campo_No_Pasivo, Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo, Datos.P_Cmd_Calculo);
                Mi_SQL = Mi_SQL + ",'" + Datos.P_Referencia + "'";
                Mi_SQL = Mi_SQL + ",'" + Datos.P_Clave_Ingreso_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Datos.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Datos.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Tramite.Trim())) + "'";
                Mi_SQL = Mi_SQL + ",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Vencimiento_Pasivo.Trim())) + "'";
                Mi_SQL = Mi_SQL + ",'" + Datos.P_Monto_Total_Pagar.Trim() + "',0";
                Mi_SQL = Mi_SQL + ",'" + Datos.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Datos.P_Dependencia_ID
                    + "', SYSDATE, '" +
                    Datos.P_Nombre_Usuario + "','"+Contribuyente.P_Solicitante+"')";

                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                // aplicar cambios en base de datos
                if (Datos.P_Cmd_Calculo == null)
                {
                    Transaccion.Commit();
                }

            }
            catch (OracleException Ex)
            {
                if (Datos.P_Cmd_Calculo == null)
                {
                    Transaccion.Rollback();
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
                    Mensaje = "Error al intentar actualizar un Registro de Cálculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Datos.P_Cmd_Calculo == null)
                {
                    Conexion.Close();
                }
            }
            //regresar el número de inserciones realizadas
            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Consecutivo_Pasivos
        /// DESCRIPCIÓN: Obtiene el numero consecutivo para la trabla de pasivos
        /// PARÁMETROS:
        /// 	1. Campo_ID: Nombre del campo del que se obtendra consecutivo
        /// 	2. Tabla: Nombre de la tabla
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 24-ago-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Obtener_Consecutivo_Pasivos(String Campo_ID, String Tabla, OracleCommand Cmd)
        {
            int Consecutivo = 0;
            String Mi_Sql;
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),0) FROM " + Tabla;
            if (Cmd != null)
            {
                Cmd.CommandText = Mi_Sql;
                int.TryParse(Cmd.ExecuteOracleScalar().ToString(), out Consecutivo);
            }
            else
            {
                Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                int.TryParse(Obj.ToString(), out Consecutivo);
            }

            return Consecutivo + 1;
        }

        #endregion

    }

    
}