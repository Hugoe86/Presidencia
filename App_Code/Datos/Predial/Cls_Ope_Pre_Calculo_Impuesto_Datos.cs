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
using Presidencia.Operacion_Predial_Calculo_Impuestos.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Ope_Pre_Calculo_Impuestos_Datos
/// </summary>

namespace Presidencia.Operacion_Predial_Calculo_Impuestos.Datos
{

    public class Cls_Ope_Pre_Calculo_Impuestos_Datos
    {
        #region Metodos
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
            ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
            ///             DataTable.
            ///PARAMETROS:     
            ///             1.  Calculo_Impuestos.   Contiene la propiedad para conocer que tipo de
            ///                                     consulta se quiere ejecutar.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 09/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static DataTable Consultar_DataTable(Cls_Ope_Pre_Calculo_Impuestos_Negocio Calculo_Impuestos){
                DataTable Dt_Calculo_Impuestos = new DataTable();
                String Mi_SQL = null;
                try{
                    DataSet Ds_Calculo_Impuestos = null;
                    if (Calculo_Impuestos.P_Tipo_DataTable.Equals("LISTAR_CONTRARECIBOS")) {
                        Boolean Primer_Filtro = true;
                        Mi_SQL = "SELECT " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " AS NO_CONTRARECIBO, " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pre_Contrarecibos.Campo_No_Escritura + " AS NO_ESCRITURA, " + Ope_Pre_Contrarecibos.Campo_Fecha_Escritura + " AS FECHA_ESCRITURA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pre_Contrarecibos.Campo_Fecha_Liberacion + " AS FECHA_LIBERACION, " + Ope_Pre_Contrarecibos.Campo_Fecha_Pago + " AS FECHA_PAGO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pre_Contrarecibos.Campo_Estatus + " AS ESTATUS";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos;
                        if (Calculo_Impuestos.P_Cuenta_Predial != null && Calculo_Impuestos.P_Cuenta_Predial.Trim().Length > 0)
                        {
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " = '" + Calculo_Impuestos.P_Cuenta_Predial + "'";
                            Primer_Filtro = false;
                        }
                        if (Calculo_Impuestos.P_No_Contrarecibo != null && Calculo_Impuestos.P_No_Contrarecibo.Trim().Length > 0) {
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Calculo_Impuestos.P_No_Contrarecibo), 10) + "'";
                                Primer_Filtro = false;
                            } else {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Calculo_Impuestos.P_No_Contrarecibo), 10) + "'";
                            }
                        }
                        if (Calculo_Impuestos.P_Buscar_Fecha_Escritura) {
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_Fecha_Escritura + " = '" + String.Format("{0:dd/MM/yyyy}",Calculo_Impuestos.P_Fecha_Escritura) + "'";
                                Primer_Filtro = false;
                            } else {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_Fecha_Escritura + " = '" + String.Format("{0:dd/MM/yyyy}", Calculo_Impuestos.P_Fecha_Escritura) + "'";
                            }
                        }
                        if (Calculo_Impuestos.P_Buscar_Fecha_Liberacion) {
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_Fecha_Liberacion + " = '" + String.Format("{0:dd/MM/yyyy}", Calculo_Impuestos.P_Fecha_Liberacion) + "'";
                                Primer_Filtro = false;
                            } else {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_Fecha_Liberacion + " = '" + String.Format("{0:dd/MM/yyyy}", Calculo_Impuestos.P_Fecha_Liberacion) + "'";
                            }
                        }
                        if (Calculo_Impuestos.P_Listado_ID!= null && Calculo_Impuestos.P_Listado_ID.Trim().Length > 0){
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_Listado_ID + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Calculo_Impuestos.P_Listado_ID), 10) + "'";
                                Primer_Filtro = false;
                            } else {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_Listado_ID + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Calculo_Impuestos.P_Listado_ID), 10) + "'";
                            }
                        }
                        if (Calculo_Impuestos.P_Buscar_Fecha_Generacion) {
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_Listado_ID + " IN (SELECT " + Ope_Pre_Listados.Campo_Listado_ID + " FROM  " + Ope_Pre_Listados.Tabla_Ope_Pre_Listados;
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Listados.Campo_Fecha_Generacion + " >= '" + String.Format("{0:dd/MM/yyyy}", Calculo_Impuestos.P_Fecha_Generacion) + "'";
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Listados.Campo_Fecha_Generacion + " < '" + String.Format("{0:dd/MM/yyyy}", (Calculo_Impuestos.P_Fecha_Generacion).AddDays(1).Date) + "')";
                                Primer_Filtro = false;
                            } else {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_Listado_ID + " IN (SELECT " + Ope_Pre_Listados.Campo_Listado_ID + " FROM  " + Ope_Pre_Listados.Tabla_Ope_Pre_Listados;
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Listados.Campo_Fecha_Generacion + " >= '" + String.Format("{0:dd/MM/yyyy}", Calculo_Impuestos.P_Fecha_Generacion) + "'";
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Listados.Campo_Fecha_Generacion + " < '" + String.Format("{0:dd/MM/yyyy}", (Calculo_Impuestos.P_Fecha_Generacion).AddDays(1).Date) + "')";
                            }
                        }
                        if (Calculo_Impuestos.P_Notario_ID != null  && Calculo_Impuestos.P_Notario_ID.Trim().Length > 0) {
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_Notario_ID + " = '" + Calculo_Impuestos.P_Notario_ID.Trim() + "'";
                                Primer_Filtro = false;
                            } else {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_Notario_ID + " = '" + Calculo_Impuestos.P_Notario_ID.Trim() + "'";
                            }
                        }
                        Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " DESC ";
                    } else if(Calculo_Impuestos.P_Tipo_DataTable.Equals("LISTAR_NOTARIOS")){
                        Mi_SQL = "SELECT " + Cat_Pre_Notarios.Campo_Notario_ID + " AS NOTARIO_ID, " + Cat_Pre_Notarios.Campo_Apellido_Paterno;
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pre_Notarios.Campo_Apellido_Materno + " ||' '|| " + Cat_Pre_Notarios.Campo_Nombre + " AS NOMBRE";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios;
                    }
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Calculo_Impuestos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Calculo_Impuestos != null) {
                        Dt_Calculo_Impuestos = Ds_Calculo_Impuestos.Tables[0];
                    }
                }catch(Exception Ex){
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Calculo_Impuestos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Modificar_Contrarecibo
            ///DESCRIPCIÓN: Se actualiza de un Contrarecibo la Cuenta_Predial.
            ///PARÁMETROS:     
            ///             1.  Calculo_Impuestos.   Contiene las propiedades (Cuenta_Predial y 
            ///                                     No_Contrarecibo) para actualizar el contrarecibo.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 10/Noviembre/2010 
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static void Modificar_Contrarecibo(Cls_Ope_Pre_Calculo_Impuestos_Negocio Calculo_Impuestos) {
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                try {
                    String Mi_SQL = "UPDATE " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " = '" + Calculo_Impuestos.P_Cuenta_Predial + "'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Calculo_Impuestos.P_No_Contrarecibo + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Code == 2627) {
                        if (Ex.Message.IndexOf("PRIMARY") != -1) {
                            Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                        } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                            Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                        } else {
                            Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                        }
                    } else if (Ex.Code == 547) {
                        Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                    } else if (Ex.Code == 515) {
                        Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error al intentar modificar el Registro. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }            
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
            private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID) {
                String Retornar = "";
                String Dato = "" + Dato_ID;
                for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                    Retornar = Retornar + "0";
                }
                Retornar = Retornar + Dato;
                return Retornar;
            }

        #endregion

    }

    
}