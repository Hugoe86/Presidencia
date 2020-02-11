using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Generar_Adeudo_Folio.Negocio;

namespace Presidencia.Operacion_Predial_Generar_Adeudo_Folio.Datos
{
    public class Cls_Ope_Pre_Generar_Adeudo_Folio_Datos
    {
        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Adeudo
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de Adeudo
        ///PARAMETROS           : 1. Adeudo.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Generar_Adeudo_Folio_Negocio
        ///                                 con los datos de Adeudos que va a ser dado de Alta.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 25/Julio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Adeudo(Cls_Ope_Pre_Generar_Adeudo_Folio_Negocio Adeudo)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Alta = false;

            if (Adeudo.P_Cmd_Adeudo != null)
            {
                Cmd = Adeudo.P_Cmd_Adeudo;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
            }
            String No_Adeudo = Obtener_ID_Consecutivo(Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio, Ope_Pre_Adeudos_Folio.Campo_No_Adeudo, "", 10);
            Adeudo.P_No_Adeudo = No_Adeudo;
            try
            {
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + " (";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Adeudo + ", ";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Folio + ", ";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Fecha + ", ";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Monto + ", ";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Concepto + ", ";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Pago + ", ";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Estatus + ", ";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Convenio + ", ";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Usuario_Creo + ", ";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Fecha_Creo + ") ";
                Mi_SQL += "VALUES ('";
                Mi_SQL += No_Adeudo + "', ";
                if (Adeudo.P_Folio != "" && Adeudo.P_Folio != null)
                {
                    Mi_SQL += "'" + Adeudo.P_Folio + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Adeudo.P_Fecha.ToString() != "" && Adeudo.P_Fecha != null)
                {
                    Mi_SQL += "'" + Adeudo.P_Fecha.ToString("d-M-yyyy") + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += Adeudo.P_Monto + ", ";
                if (Adeudo.P_Concepto != "" && Adeudo.P_Concepto != null)
                {
                    Mi_SQL += "'" + Adeudo.P_Concepto + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Adeudo.P_No_Pago != "" && Adeudo.P_No_Pago != null)
                {
                    Mi_SQL += "'" + Adeudo.P_No_Pago + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Adeudo.P_Estatus != "" && Adeudo.P_Estatus != null)
                {
                    Mi_SQL += "'" + Adeudo.P_Estatus + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Adeudo.P_Cuenta_Predial_ID != "" && Adeudo.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += "'" + Adeudo.P_Cuenta_Predial_ID + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                if (Adeudo.P_No_Convenio != "" && Adeudo.P_No_Convenio != null)
                {
                    Mi_SQL += "'" + Adeudo.P_No_Convenio + "', ";
                }
                else
                {
                    Mi_SQL += "NULL, ";
                }
                Mi_SQL += "'" + Adeudo.P_Usuario + "', SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Adeudo.P_Cmd_Adeudo == null)
                {
                    Trans.Commit();
                }
                Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Adeudo.P_Cmd_Adeudo == null)
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Adeudo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Adeudo.P_Cmd_Adeudo == null)
                {
                    Cn.Close();
                }
            }
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Adeudo
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Adeudo
        ///PARAMETROS           : 1. Adeudo.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Generar_Adeudo_Folio_Negocio
        ///                                 con los datos del Adeudos que va a ser Modificado.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 25/Julio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Adeudo(Cls_Ope_Pre_Generar_Adeudo_Folio_Negocio Adeudo)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Actualizar = false;

            if (Adeudo.P_Cmd_Adeudo != null)
            {
                Cmd = Adeudo.P_Cmd_Adeudo;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
            }
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + " SET ";
                if (Adeudo.P_Folio != "" && Adeudo.P_Folio != null)
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Folio + " = '" + Adeudo.P_Folio + "', ";
                }
                if (Adeudo.P_Fecha.ToString() != "" && Adeudo.P_Fecha != null)
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Fecha + " = '" + Adeudo.P_Fecha.ToShortDateString() + "', ";
                }
                if (Adeudo.P_Monto != null)
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Monto + " = '" + Adeudo.P_Monto + "', ";
                }
                if (Adeudo.P_Concepto != "" && Adeudo.P_Concepto != null)
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Concepto + " = '" + Adeudo.P_Concepto + "', ";
                }
                if (Adeudo.P_No_Pago != "" && Adeudo.P_No_Pago != null)
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Pago + " = '" + Adeudo.P_No_Pago + "', ";
                }
                if (Adeudo.P_Estatus != "" && Adeudo.P_Estatus != null)
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Estatus + " = '" + Adeudo.P_Estatus + "', ";
                }
                if (Adeudo.P_Cuenta_Predial_ID != "" && Adeudo.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Cuenta_Predial_ID + " = '" + Adeudo.P_Cuenta_Predial_ID + "', ";
                }
                if (Adeudo.P_No_Convenio != "" && Adeudo.P_No_Convenio != null)
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Convenio + " = '" + Adeudo.P_No_Convenio + "', ";
                }
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Usuario_Modifico + " = '" + Adeudo.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL += " WHERE ";
                if (Adeudo.P_No_Adeudo != "")
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Adeudo + " = '" + Adeudo.P_No_Adeudo + "' AND ";
                }
                if (Adeudo.P_Folio != "")
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Folio + " = '" + Adeudo.P_Folio + "' AND ";
                }
                if (Adeudo.P_No_Pago != "")
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Pago + " = '" + Adeudo.P_No_Pago + "' AND ";
                }
                if (Adeudo.P_No_Convenio != "")
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Convenio + " = '" + Adeudo.P_No_Convenio + "' AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Adeudo.P_Cmd_Adeudo == null)
                {
                    Trans.Commit();
                }
                Actualizar = true;
            }
            catch (OracleException Ex)
            {
                if (Adeudo.P_Cmd_Adeudo == null)
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
                    Mensaje = "Error al intentar modificar un Registro de Adeudo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Adeudo.P_Cmd_Adeudo == null)
                {
                    Cn.Close();
                }
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Constancias
        ///DESCRIPCIÓN          : Obtiene todos las Adeudo que estan dadas de alta en la base de datos
        ///PARAMETROS           : 1. Adeudo.   Instancia de la Clase de Negocio de Cls_Ope_Pre_Constancias_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 25/Julio/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Adeudo(Cls_Ope_Pre_Generar_Adeudo_Folio_Negocio Adeudo)
        {
            DataTable Dt_Constancias = new DataTable();
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT ";
                if (Adeudo.P_Campos_Dinamicos != null && Adeudo.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Adeudo.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Adeudo + " AS No_Adeudo, ";
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Folio + " AS Folio, ";
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Fecha + " AS Fecha, ";
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Monto + " AS Monto, ";
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Concepto + " AS Concepto, ";
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Pago + " AS No_Pago, ";
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Estatus + " AS Estatus, ";
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Cuenta_Predial_ID + " AS Cuenta_Predial_ID, ";
                    Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Convenio + " AS No_Convenio, ";
                }
                Mi_SQL += " FROM " + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio;
                if (Adeudo.P_Filtros_Dinamicos != null && Adeudo.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Adeudo.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Adeudo.P_No_Convenio != "" && Adeudo.P_No_Convenio != null)
                    {
                        Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Convenio + " LIKE '%" + Adeudo.P_No_Convenio + "%'";
                    }
                    if (Adeudo.P_Folio != "" && Adeudo.P_Folio != null)
                    {
                        Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Folio + " LIKE '%" + Adeudo.P_Folio + "%'";
                    }
                    if (Adeudo.P_Fecha.ToString() != "" && Adeudo.P_Fecha != null)
                    {
                        Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Fecha + " >= '" + Adeudo.P_Fecha.ToShortDateString() + "'";
                        Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Fecha + " < '" + Adeudo.P_Fecha.AddDays(1).ToShortDateString() + "'";
                    }
                    if (Adeudo.P_Concepto != "" && Adeudo.P_Concepto != null)
                    {
                        Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Concepto + " LIKE '%" + Adeudo.P_Concepto + "%'";
                    }
                    if (Adeudo.P_No_Pago != "" && Adeudo.P_No_Pago != null)
                    {
                        Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Pago + " = '" + Adeudo.P_No_Pago + "'";
                    }
                    if (Adeudo.P_Estatus != "" && Adeudo.P_Estatus != null)
                    {
                        Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Estatus + " LIKE '%" + Adeudo.P_Estatus + "%'";
                    }
                    if (Adeudo.P_Cuenta_Predial_ID != "" && Adeudo.P_Cuenta_Predial_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_Cuenta_Predial_ID + " = '" + Adeudo.P_Cuenta_Predial_ID + "'";
                    }
                    if (Adeudo.P_No_Convenio != "" && Adeudo.P_No_Convenio != null)
                    {
                        Mi_SQL += Ope_Pre_Adeudos_Folio.Campo_No_Convenio + " = '" + Adeudo.P_No_Convenio + "'";
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
                if (Adeudo.P_Agrupar_Dinamico != null && Adeudo.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Adeudo.P_Agrupar_Dinamico;
                }
                if (Adeudo.P_Ordenar_Dinamico != null && Adeudo.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Adeudo.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Adeudos_Folio.Campo_Folio;
                }

                DataSet Ds_Constancias = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Constancias != null)
                {
                    Dt_Constancias = Ds_Constancias.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Adeudo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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

        #endregion
    }
}
