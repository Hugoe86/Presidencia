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
using Presidencia.Operacion_Predial_Traslado.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Ope_Pre_Traslado_Dominio_Datos
/// </summary>

namespace Presidencia.Operacion_Predial_Traslado.Datos
{

    public class Cls_Ope_Pre_Traslado_Datos
    {
        #region Metodos
        
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
            ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y obtiene los datos en un
            ///             DataTable.
            ///PARAMETROS:     
            ///             1.  Traslado_Dominio.   Contiene la propiedad para conocer que tipo de
            ///                                     consulta se quiere ejecutar.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 09/Noviembre/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static DataTable Consultar_DataTable(Cls_Ope_Pre_Traslado_Negocio Traslado_Dominio){
                DataTable Dt_Traslado_Dominio = new DataTable();
                String Mi_SQL = null;
                try{
                    DataSet Ds_Traslado_Dominio = null;
                    if (Traslado_Dominio.P_Tipo_DataTable.Equals("LISTAR_CONTRARECIBOS")) {
                        Boolean Primer_Filtro = true;
                        Mi_SQL = "SELECT " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " ||'/'|| " + Ope_Pre_Contrarecibos.Campo_Anio + " AS NO_CONTRARECIBO, ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID, ";
                        Mi_SQL = Mi_SQL + "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + ") AS CUENTA_PREDIAL, ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_No_Escritura + " AS NO_ESCRITURA, ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Fecha_Escritura + " AS FECHA_ESCRITURA, ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Fecha_Liberacion + " AS FECHA_LIBERACION, ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Fecha_Pago + " AS FECHA_PAGO, ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_Estatus + " AS ESTATUS";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos;
                        
                        if (Traslado_Dominio.P_Cuenta_Predial_ID != null && Traslado_Dominio.P_Cuenta_Predial_ID.Trim().Length > 0)
                        {
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " in (SELECT c." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM ";
                            Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " c WHERE UPPER(" + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ") LIKE UPPER('%" + Traslado_Dominio.P_Cuenta_Predial_ID +"%'))";
                            Primer_Filtro = false;
                        }
                        if (Primer_Filtro)
                        {
                            Mi_SQL = Mi_SQL + " WHERE (" + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'GENERADO'";
                            if (String.IsNullOrEmpty(Traslado_Dominio.P_Traslado))
                            {
                                Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'VALIDADO'";
                                Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " LIKE 'CANCELADO%'";
                            }
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'LISTO'";
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'PENDIENTE'";
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'CALCULADO'";
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'PAGADO'";
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'POR VALIDAR'";
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'POR PAGAR'";
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'RECHAZADO')";
                            Primer_Filtro = false;
                        }
                        else
                        {
                            Mi_SQL = Mi_SQL + " AND (" + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'GENERADO'";
                            if (String.IsNullOrEmpty(Traslado_Dominio.P_Traslado))
                            {
                                Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'VALIDADO'";
                                Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " LIKE 'CANCELADO%'";
                            }
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'PENDIENTE'";
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'CALCULADO'";
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'PAGADO'";
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'POR VALIDAR'";
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'POR PAGAR'";
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'RECHAZADO')";
                        }
                        if (Traslado_Dominio.P_No_Contrarecibo != null && Traslado_Dominio.P_No_Contrarecibo.Trim().Length > 0)
                        {
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Traslado_Dominio.P_No_Contrarecibo + "'";
                                Primer_Filtro = false;
                            } else {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Traslado_Dominio.P_No_Contrarecibo + "'";
                            }
                        }
                        if (Traslado_Dominio.P_Buscar_Fecha_Escritura) {
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_Fecha_Escritura + " = '" + String.Format("{0:dd/MM/yyyy}",Traslado_Dominio.P_Fecha_Escritura) + "'";
                                Primer_Filtro = false;
                            } else {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_Fecha_Escritura + " = '" + String.Format("{0:dd/MM/yyyy}", Traslado_Dominio.P_Fecha_Escritura) + "'";
                            }
                        }
                        if (Traslado_Dominio.P_Buscar_Estatus)
                        {
                            if (Primer_Filtro)
                            {
                                Mi_SQL = Mi_SQL + " WHERE UPPER(" + Ope_Pre_Contrarecibos.Campo_Estatus + ") LIKE UPPER('" + Traslado_Dominio.P_Estatus + "%')";
                                Primer_Filtro = false;
                            }
                            else
                            {
                                Mi_SQL = Mi_SQL + " AND UPPER(" + Ope_Pre_Contrarecibos.Campo_Estatus + ") LIKE UPPER('" + Traslado_Dominio.P_Estatus + "%')";
                            }
                        }
                        if (Traslado_Dominio.P_Buscar_Fecha_Escritura) {
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_Fecha_Escritura + " = '" + String.Format("{0:dd/MM/yyyy}",Traslado_Dominio.P_Fecha_Escritura) + "'";
                                Primer_Filtro = false;
                            } else {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_Fecha_Escritura + " = '" + String.Format("{0:dd/MM/yyyy}", Traslado_Dominio.P_Fecha_Escritura) + "'";
                            }
                        }
                        if (Traslado_Dominio.P_Buscar_No_Escritura) {
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Escritura + " = '" + Traslado_Dominio.P_No_Escritura + "'";
                                Primer_Filtro = false;
                            }
                            else
                            {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_No_Escritura + " = '" + Traslado_Dominio.P_No_Escritura + "'";
                            }
                        }
                        if (Traslado_Dominio.P_Listado_ID!= null && Traslado_Dominio.P_Listado_ID.Trim().Length > 0){
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_Listado_ID + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Traslado_Dominio.P_Listado_ID), 10) + "'";
                                Primer_Filtro = false;
                            } else {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_Listado_ID + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Traslado_Dominio.P_Listado_ID), 10) + "'";
                            }
                        }
                        if (Traslado_Dominio.P_Buscar_Fecha_Generacion) {
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_Listado_ID + " IN (SELECT " + Ope_Pre_Listados.Campo_Listado_ID + " FROM  " + Ope_Pre_Listados.Tabla_Ope_Pre_Listados;
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Listados.Campo_Fecha_Generacion + " >= '" + String.Format("{0:dd/MM/yyyy}", Traslado_Dominio.P_Fecha_Generacion) + "'";
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Listados.Campo_Fecha_Generacion + " < '" + String.Format("{0:dd/MM/yyyy}", (Traslado_Dominio.P_Fecha_Generacion).AddDays(1).Date) + "')";
                                Primer_Filtro = false;
                            } else {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_Listado_ID + " IN (SELECT " + Ope_Pre_Listados.Campo_Listado_ID + " FROM  " + Ope_Pre_Listados.Tabla_Ope_Pre_Listados;
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Listados.Campo_Fecha_Generacion + " >= '" + String.Format("{0:dd/MM/yyyy}", Traslado_Dominio.P_Fecha_Generacion) + "'";
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Listados.Campo_Fecha_Generacion + " < '" + String.Format("{0:dd/MM/yyyy}", (Traslado_Dominio.P_Fecha_Generacion).AddDays(1).Date) + "')";
                            }
                        }
                        if (Traslado_Dominio.P_Notario_ID != null  && Traslado_Dominio.P_Notario_ID.Trim().Length > 0) {
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_Notario_ID + " = '" + Traslado_Dominio.P_Notario_ID.Trim() + "'";
                                Primer_Filtro = false;
                            } else {
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_Notario_ID + " = '" + Traslado_Dominio.P_Notario_ID.Trim() + "'";
                            }
                        }
                        if (Traslado_Dominio.P_Con_Cuenta_Predial) {
                            if (Primer_Filtro) {
                                Mi_SQL = Mi_SQL + " WHERE NOT " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " IS NULL";
                                Primer_Filtro = false;
                            }
                            else {
                                Mi_SQL = Mi_SQL + " AND NOT " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " IS NULL";
                            }
                        }                        
                        else
                        {
                            //Mi_SQL = Mi_SQL + " AND NOT " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " IS NULL";
                        }
                        if (Traslado_Dominio.P_Contrarecibos_Sin_Calculo)
                        {
                            if (Primer_Filtro)
                            {
                                Mi_SQL = Mi_SQL + " WHERE NOT " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " IN (";
                                Mi_SQL = Mi_SQL + "SELECT NVL(" + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_No_Contrarecibo;
                                Mi_SQL = Mi_SQL + ",'NULL') FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ", " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion + " = " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion;
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden;
                                Mi_SQL = Mi_SQL + ")";
                                Primer_Filtro = false;
                            }
                            else
                            {
                                Mi_SQL = Mi_SQL + " AND NOT " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " IN (";
                                Mi_SQL = Mi_SQL + "SELECT  NVL(" + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo;
                                Mi_SQL = Mi_SQL + ",'NULL') FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ", " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion + " = " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion;
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden;
                                Mi_SQL = Mi_SQL + ")";
                            }
                        }


                        Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " DESC ";
                    } else if(Traslado_Dominio.P_Tipo_DataTable.Equals("LISTAR_NOTARIOS")){
                        Mi_SQL = "SELECT " + Cat_Pre_Notarios.Campo_Notario_ID + " AS NOTARIO_ID, " + Cat_Pre_Notarios.Campo_Apellido_Paterno;
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pre_Notarios.Campo_Apellido_Materno + " ||' '|| " + Cat_Pre_Notarios.Campo_Nombre + " AS NOMBRE";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios;
                    }
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                         Ds_Traslado_Dominio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Traslado_Dominio != null) {
                        Dt_Traslado_Dominio = Ds_Traslado_Dominio.Tables[0];
                    }
                }catch(Exception Ex){
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Traslado_Dominio;
            }

                
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Modificar_Contrarecibo
            ///DESCRIPCIÓN: Se actualiza de un Contrarecibo la Cuenta_Predial.
            ///PARÁMETROS:     
            ///             1.  Traslado_Dominio.   Contiene las propiedades (Cuenta_Predial y 
            ///                                     No_Contrarecibo) para actualizar el contrarecibo.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 10/Noviembre/2010 
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static void Modificar_Contrarecibo(Cls_Ope_Pre_Traslado_Negocio Traslado_Dominio) {
                String Mensaje = "";
                String Anio = "0";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                try {
                    if (Traslado_Dominio.P_No_Contrarecibo.Contains('/'))
                    {
                        Anio = Traslado_Dominio.P_No_Contrarecibo.Substring(11, 4);
                        Traslado_Dominio.P_No_Contrarecibo = Traslado_Dominio.P_No_Contrarecibo.Substring(0,10);                        
                    }
                    String Mi_SQL = "UPDATE " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " = '" + Traslado_Dominio.P_Cuenta_Predial_ID + "'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Traslado_Dominio.P_No_Contrarecibo + "'";
                    Mi_SQL = Mi_SQL + " AND " +  Ope_Pre_Contrarecibos.Campo_Anio + " = '" + Anio + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiado extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
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
            ///NOMBRE DE LA FUNCIÓN: Modificar_Contrarecibo
            ///DESCRIPCIÓN: Se actualiza de un Contrarecibo la Cuenta_Predial.
            ///PARÁMETROS:     
            ///             1.  Traslado_Dominio.   Contiene las propiedades (Cuenta_Predial y 
            ///                                     No_Contrarecibo) para actualizar el contrarecibo.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 10/Noviembre/2010 
            ///MODIFICO             : Jesus Toledo Rdz
            ///FECHA_MODIFICO       : 14/Nov/2011
            ///CAUSA_MODIFICACIÓN   : Consultar con año
            ///*******************************************************************************
            public static void Modificar_Contrarecibo_Estatus(Cls_Ope_Pre_Traslado_Negocio Traslado_Dominio)
            {
                String Mi_SQL;
                String Mensaje = "";
                String Contrarecibo = "";
                String Anio = "";
                Object Aux_Orden = null;
                Object Aux_Orden_Anio = null;
                DataTable Dt_Aux_Orden = null;
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                
                try
                {
                    if (!String.IsNullOrEmpty(Traslado_Dominio.P_No_Contrarecibo))
                    {
                        if (Traslado_Dominio.P_No_Contrarecibo.Contains('/'))
                        {
                            Contrarecibo = Traslado_Dominio.P_No_Contrarecibo.Split('/')[0].ToString();
                            Anio = Traslado_Dominio.P_No_Contrarecibo.Split('/')[1].ToString();
                        }
                        else
                        {
                            Contrarecibo = Traslado_Dominio.P_No_Contrarecibo;
                        }
                    }
                    Mi_SQL = "UPDATE " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Pre_Contrarecibos.Campo_Estatus + " = '" + Traslado_Dominio.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Contrarecibo + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Contrarecibos.Campo_Anio + " = '" + Anio + "'";

                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    //Cancelar OV
                    if (Traslado_Dominio.P_Estatus == "GENERADO" || Traslado_Dominio.P_Estatus == "CANCELADO")
                    {
                        Mi_SQL = "UPDATE " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                        Mi_SQL += " SET " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'CANCELADA'";
                        if (!String.IsNullOrEmpty(Traslado_Dominio.P_Orden_Variacion_ID))
                        {
                            Mi_SQL += " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Traslado_Dominio.P_Orden_Variacion_ID + "' ";
                            Mi_SQL += " AND " + Ope_Pre_Contrarecibos.Campo_Anio + " = " + Traslado_Dominio.P_Año;

                        }
                        else if (!String.IsNullOrEmpty(Traslado_Dominio.P_No_Contrarecibo))
                        {
                            Mi_SQL += " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Contrarecibo + "' ";
                            Mi_SQL += " AND " + Ope_Pre_Contrarecibos.Campo_Anio + " = " + Anio;
                        }
                        if (!String.IsNullOrEmpty(Traslado_Dominio.P_No_Contrarecibo) || !String.IsNullOrEmpty(Traslado_Dominio.P_Orden_Variacion_ID))
                        {
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                        Traslado_Dominio.P_Estatus = "GENERADO";
                    }
                    if (!Traslado_Dominio.P_Estatus.Contains("CANCELADO"))
                    {
                        //Borrar Calculo
                        if ((Traslado_Dominio.P_Estatus_Anterior == "CALCULADO" || Traslado_Dominio.P_Estatus_Anterior == "LISTO" || Traslado_Dominio.P_Estatus_Anterior == "POR PAGAR") && Traslado_Dominio.P_Estatus != "CALCULADO" && Traslado_Dominio.P_Estatus != "LISTO")
                        {
                            Mi_SQL = "";
                            Mi_SQL = "SELECT ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                            Mi_SQL = Mi_SQL + " FROM ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " = '" + Contrarecibo + "'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = '" + Anio + "'";
                            //Ejecutar consulta del consecutivo               
                            Dt_Aux_Orden = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                            if (Dt_Aux_Orden.Rows.Count > 0)
                            {
                                Aux_Orden = Dt_Aux_Orden.Rows[0][0];
                                Aux_Orden_Anio = Dt_Aux_Orden.Rows[0][1];

                                Mi_SQL = "DELETE " + Ope_Pre_Calc_Imp_Tras_Det.Tabla_Ope_Pre_Ope_Pre_Calc_Imp_Tras_Det;
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Calc_Imp_Tras_Det.Campo_No_Calculo + " IN( SELECT ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo;
                                Mi_SQL = Mi_SQL + " FROM ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden.ToString();
                                Mi_SQL = Mi_SQL + "'";
                                Mi_SQL = Mi_SQL + " AND ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden_Anio.ToString();
                                Mi_SQL = Mi_SQL + "')";
                                Mi_SQL = Mi_SQL + " AND ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calc_Imp_Tras_Det.Campo_Anio_Calculo;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden_Anio.ToString();
                                Mi_SQL = Mi_SQL + "'";


                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();

                                Mi_SQL = "DELETE " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado;
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Descuento_Traslado.Campo_No_Calculo + " IN( SELECT ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo;
                                Mi_SQL = Mi_SQL + " FROM ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden.ToString();
                                Mi_SQL = Mi_SQL + "'";
                                Mi_SQL = Mi_SQL + " AND ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden_Anio.ToString();
                                Mi_SQL = Mi_SQL + "')";
                                Mi_SQL = Mi_SQL + " AND ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden_Anio.ToString();
                                Mi_SQL = Mi_SQL + "'";

                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();

                                Mi_SQL = "DELETE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio;
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio + " IN( SELECT " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio;
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Calculo + " IN( SELECT ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo;
                                Mi_SQL = Mi_SQL + " FROM ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden.ToString();
                                Mi_SQL = Mi_SQL + "'";
                                Mi_SQL = Mi_SQL + " AND ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden_Anio.ToString();
                                Mi_SQL = Mi_SQL + "')";
                                Mi_SQL = Mi_SQL + " AND ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Traslados_Dominio.Campo_Anio;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden_Anio.ToString();
                                Mi_SQL = Mi_SQL + "')";

                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();

                                Mi_SQL = "DELETE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio;
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Calculo + " IN( SELECT ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo;
                                Mi_SQL = Mi_SQL + " FROM ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden.ToString();
                                Mi_SQL = Mi_SQL + "'";
                                Mi_SQL = Mi_SQL + " AND ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden_Anio.ToString();
                                Mi_SQL = Mi_SQL + "')";
                                Mi_SQL = Mi_SQL + " AND ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Convenios_Traslados_Dominio.Campo_Anio;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden_Anio.ToString();
                                Mi_SQL = Mi_SQL + "'";

                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();

                                Mi_SQL = "DELETE " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden.ToString();
                                Mi_SQL = Mi_SQL + "'";
                                Mi_SQL = Mi_SQL + " AND ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden_Anio.ToString();
                                Mi_SQL = Mi_SQL + "'";

                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();

                            }
                            if (Traslado_Dominio.P_Estatus != "GENERADO")
                            {
                                Mi_SQL = "UPDATE " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                                Mi_SQL += " SET " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden;
                                if (Traslado_Dominio.P_Estatus == "VALIDADO")
                                {
                                    Mi_SQL += " = 'ACEPTADA'";
                                }
                                else if (Traslado_Dominio.P_Estatus == "RECHAZADO")
                                {
                                    Mi_SQL += " = 'RECHAZADA'";
                                }
                                else if (Traslado_Dominio.P_Estatus == "POR VALIDAR")
                                {
                                    Mi_SQL += " = 'POR VALIDAR'";
                                }
                                if (!String.IsNullOrEmpty(Traslado_Dominio.P_Orden_Variacion_ID))
                                {
                                    Mi_SQL += " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Traslado_Dominio.P_Orden_Variacion_ID + "' ";
                                    Mi_SQL += " AND " + Ope_Pre_Contrarecibos.Campo_Anio + " = " + Traslado_Dominio.P_Año;

                                }
                                else if (!String.IsNullOrEmpty(Traslado_Dominio.P_No_Contrarecibo))
                                {
                                    Mi_SQL += " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Contrarecibo + "' ";
                                    Mi_SQL += " AND " + Ope_Pre_Contrarecibos.Campo_Anio + " = " + Anio;
                                }
                                if (!String.IsNullOrEmpty(Traslado_Dominio.P_No_Contrarecibo) || !String.IsNullOrEmpty(Traslado_Dominio.P_Orden_Variacion_ID))
                                {
                                    Cmd.CommandText = Mi_SQL;
                                    Cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        //Se actualiza calculo
                        if ((Traslado_Dominio.P_Estatus == "CALCULADO") && (Traslado_Dominio.P_Estatus_Anterior == "LISTO" || Traslado_Dominio.P_Estatus_Anterior == "POR PAGAR"))
                        {
                            Mi_SQL = "";
                            Mi_SQL = "SELECT ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                            Mi_SQL = Mi_SQL + " FROM ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " = '" + Contrarecibo + "'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = '" + Anio + "'";
                            //Ejecutar consulta del consecutivo               
                            Dt_Aux_Orden = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                            if (Dt_Aux_Orden.Rows.Count > 0)
                            {
                                Aux_Orden = Dt_Aux_Orden.Rows[0][0];
                                Aux_Orden_Anio = Dt_Aux_Orden.Rows[0][1];

                                Mi_SQL = "UPDATE " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                                Mi_SQL = Mi_SQL + " SET ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus;
                                Mi_SQL = Mi_SQL + " = '" + Traslado_Dominio.P_Estatus + "'";
                                Mi_SQL = Mi_SQL + " WHERE ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden.ToString();
                                Mi_SQL = Mi_SQL + "'";
                                Mi_SQL = Mi_SQL + " AND ";
                                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden;
                                Mi_SQL = Mi_SQL + " ='";
                                Mi_SQL = Mi_SQL + Aux_Orden_Anio.ToString();
                                Mi_SQL = Mi_SQL + "'";

                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();

                            }
                        }
                    }
                    Trans.Commit();
                    if (String.IsNullOrEmpty(Anio))
                        throw new Exception("No se actualizó el estatus del contrarecibo, No se especificó el año de éste");
                }
                catch (OracleException Ex)
                {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152)
                    {
                        Mensaje = "Existen datos demasiado extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
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
                        Mensaje = "Error al intentar modificar el Registro. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                }
                finally
                {
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
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Recepcion
            ///DESCRIPCIÓN: Consulta de datos de Recepcion del contrarecibo
            ///PARÁMETROS: No_Contrarecibo Identificador del contrarecibo
            ///CREO: Jesus Toledo Rodriguez
            ///FECHA_CREO: 11/Nov/2011 
            ///MODIFICO: 
            ///FECHA_MODIFICO: 
            ///CAUSA_MODIFICACIÓN: 
            ///*******************************************************************************
            internal static string Consultar_Recepcion(String No_Contrarecibo)
            {
                String Mi_SQL = null;
                String No_Movimiento = null;
                String Contrarecibo = "";
                String Anio = "";
                try
                {
                    if (!String.IsNullOrEmpty(No_Contrarecibo))
                    {
                        if (No_Contrarecibo.Contains('/'))
                        {
                            Contrarecibo = No_Contrarecibo.Split('/')[0].ToString();
                            Anio = No_Contrarecibo.Split('/')[1].ToString();
                        }
                        else
                        {
                            Contrarecibo = No_Contrarecibo;
                        }
                    }
                    Mi_SQL = "SELECT " + Ope_Pre_Recep_Docs_Movs.Campo_No_Recepcion_Documento + "||'-'||" + Ope_Pre_Recep_Docs_Movs.Campo_No_Movimiento + " AS NO_MOVIMIENTO ";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Recep_Docs_Movs.Tabla_Ope_Pre_Recep_Docs_Movs;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Recep_Docs_Movs.Campo_No_Contrarecibo + " = '" + Contrarecibo + "'";
                    if(!string.IsNullOrEmpty(Anio))
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Recep_Docs_Movs.Campo_Anio + " = '" + Anio + "'";

                        No_Movimiento = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0].ToString();

                }catch(Exception Ex){
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return No_Movimiento;
            }
        #endregion


            
    }

    
}