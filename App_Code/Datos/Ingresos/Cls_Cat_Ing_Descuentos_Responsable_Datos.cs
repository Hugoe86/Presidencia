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
using Presidencia.Cls_Cat_Ing_Descuentos_Responsable.Negocio;

namespace Presidencia.Cls_Cat_Ing_Descuentos_Responsable.Datos
{

    public class Cls_Cat_Ing_Descuentos_Responsable_Datos
    {

        #region Altas

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Descuento_Responsable
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos los datos del Descuento por Responsable
        ///PARAMETROS           : Descuento_Responsable, instancia de Cls_Cat_Ing_Descuentos_Responsable_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 25/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Alta_Descuento_Responsable(Cls_Cat_Ing_Descuentos_Responsable_Negocio Descuento_Responsable)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Estatus_Alta = false;

            if (Descuento_Responsable.P_Cmmd != null)
            {
                Cmmd = Descuento_Responsable.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmmd.Connection = Trans.Connection;
                Cmmd.Transaction = Trans;
            }

            try
            {
                Descuento_Responsable.P_Descuento_Responsable_ID = Obtener_ID_Consecutivo(ref Cmmd, Cat_Ing_Descuentos_Responsable.Campo_Descuento_Responsable_ID, 10);
                String Mi_SQL = "INSERT INTO " + Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable;
                Mi_SQL += " (" + Cat_Ing_Descuentos_Responsable.Campo_Descuento_Responsable_ID;
                Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Empleado_ID;
                Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Estatus;
                Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Porcentaje;
                Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Descripcion;
                Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Tipo;
                Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Usuario_Creo;
                Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES ('" + Descuento_Responsable.P_Descuento_Responsable_ID + "'";
                Mi_SQL += ", '" + Descuento_Responsable.P_Empleado_ID + "'";
                Mi_SQL += ", '" + Descuento_Responsable.P_Estatus + "'";
                Mi_SQL += ", " + Descuento_Responsable.P_Porcentaje;
                Mi_SQL += ", '" + Descuento_Responsable.P_Descripcion.Trim().ToUpper() + "'";
                Mi_SQL += ", '" + Descuento_Responsable.P_Tipo + "'";
                Mi_SQL += ", '" + Descuento_Responsable.P_Usuario + "'";
                Mi_SQL += ", SYSDATE";
                Mi_SQL += ")";
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();

                if (Descuento_Responsable.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Estatus_Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Descuento_Responsable.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2727)
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
                    Mensaje = "Error al intentar dar de Alta una P_Clave de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Descuento_Responsable.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }

            return Estatus_Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Descuentos_Responsables
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos los datos del Descuento por Responsable
        ///PARAMETROS           : Descuento_Responsable, instancia de Cls_Cat_Ing_Descuentos_Responsable_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 27/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Alta_Descuentos_Responsables(Cls_Cat_Ing_Descuentos_Responsable_Negocio Descuento_Responsable)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Estatus_Alta = false;

            if (Descuento_Responsable.P_Cmmd != null)
            {
                Cmmd = Descuento_Responsable.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmmd.Connection = Trans.Connection;
                Cmmd.Transaction = Trans;
            }

            try
            {
                if (Descuento_Responsable.P_Dt_Descuentos_Responsable != null)
                {
                    if (Descuento_Responsable.P_Dt_Descuentos_Responsable.Rows.Count > 0)
                    {
                        foreach (DataRow Dr_P_Dt_Descuentos_Responsable in Descuento_Responsable.P_Dt_Descuentos_Responsable.Rows)
                        {
                            String Descuento_Responsable_ID = Obtener_ID_Consecutivo(ref Cmmd, Cat_Ing_Descuentos_Responsable.Campo_Descuento_Responsable_ID, 10);
                            String Mi_SQL = "INSERT INTO " + Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable;
                            Mi_SQL += " (" + Cat_Ing_Descuentos_Responsable.Campo_Descuento_Responsable_ID;
                            Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Empleado_ID;
                            Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Estatus;
                            Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Porcentaje;
                            Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Descripcion;
                            Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Tipo;
                            Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Usuario_Creo;
                            Mi_SQL += ", " + Cat_Ing_Descuentos_Responsable.Campo_Fecha_Creo + ")";
                            Mi_SQL += " VALUES ('" + Descuento_Responsable_ID + "'";
                            Mi_SQL += ", '" + Dr_P_Dt_Descuentos_Responsable[Cat_Ing_Descuentos_Responsable.Campo_Empleado_ID].ToString() + "'";
                            Mi_SQL += ", '" + Dr_P_Dt_Descuentos_Responsable[Cat_Ing_Descuentos_Responsable.Campo_Estatus].ToString() + "'";
                            Mi_SQL += ", '" + Dr_P_Dt_Descuentos_Responsable[Cat_Ing_Descuentos_Responsable.Campo_Porcentaje].ToString() + "'";
                            Mi_SQL += ", '" + Dr_P_Dt_Descuentos_Responsable[Cat_Ing_Descuentos_Responsable.Campo_Descripcion].ToString().Trim().ToUpper() + "'";
                            Mi_SQL += ", '" + Dr_P_Dt_Descuentos_Responsable[Cat_Ing_Descuentos_Responsable.Campo_Tipo].ToString() + "'";
                            Mi_SQL += ", '" + Descuento_Responsable.P_Usuario + "'";
                            Mi_SQL += ", SYSDATE";
                            Mi_SQL += ")";
                            Cmmd.CommandText = Mi_SQL;
                            Cmmd.ExecuteNonQuery();
                        }
                    }
                }

                if (Descuento_Responsable.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Estatus_Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Descuento_Responsable.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2727)
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
                    Mensaje = "Error al intentar dar de Alta una P_Clave de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Descuento_Responsable.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }

            return Estatus_Alta;
        }

        #endregion

        #region Modificaciones

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Descuento_Responsable
        ///DESCRIPCIÓN          : Modifica en la Base de Datos los datos del Descuento por Responsable
        ///PARAMETROS           : Descuento_Responsable, instancia de Cls_Cat_Ing_Descuentos_Responsable_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 25/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Modificar_Descuento_Responsable(Cls_Cat_Ing_Descuentos_Responsable_Negocio Descuento_Responsable)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Estatus_Modificar = false;

            if (Descuento_Responsable.P_Cmmd != null)
            {
                Cmmd = Descuento_Responsable.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmmd.Connection = Trans.Connection;
                Cmmd.Transaction = Trans;
            }

            try
            {
                String Mi_SQL = "UPDATE " + Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable;
                Mi_SQL += " SET ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Campo_Empleado_ID + " = '" + Descuento_Responsable.P_Empleado_ID + "', ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Campo_Estatus + " = '" + Descuento_Responsable.P_Estatus + "', ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Campo_Porcentaje + " = '" + Descuento_Responsable.P_Porcentaje + "', ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Campo_Descripcion + " = '" + Descuento_Responsable.P_Descripcion.Trim().ToUpper() + "', ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Campo_Tipo + " = '" + Descuento_Responsable.P_Tipo + "', ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Campo_Usuario_Modifico + " = '" + Descuento_Responsable.P_Usuario + "', ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL += " WHERE " + Cat_Ing_Descuentos_Responsable.Campo_Descuento_Responsable_ID + " = '" + Descuento_Responsable.P_Descuento_Responsable_ID + "'";
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();

                if (Descuento_Responsable.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Estatus_Modificar = true;
            }
            catch (OracleException Ex)
            {
                if (Descuento_Responsable.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2727)
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
                    Mensaje = "Error al intentar modificar un Registro de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Descuento_Responsable.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }

            return Estatus_Modificar;
        }

        #endregion

        #region Eliminaciones

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Descuento_Responsable
        ///DESCRIPCIÓN          : Elimina de la Base de Datos los registros de Descuento por Responsable
        ///PARAMETROS           : Descuento_Responsable, instancia de Cls_Cat_Ing_Descuentos_Responsable_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 27/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Eliminar_Descuento_Responsable(Cls_Cat_Ing_Descuentos_Responsable_Negocio Descuento_Responsable)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Estatus_Eliminar = false;

            if (Descuento_Responsable.P_Cmmd != null)
            {
                Cmmd = Descuento_Responsable.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmmd.Connection = Trans.Connection;
                Cmmd.Transaction = Trans;
            }

            try
            {
                String Mi_SQL = "";

                try
                {
                    Mi_SQL = "DELETE FROM " + Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable;
                    Mi_SQL += " WHERE " + Cat_Ing_Descuentos_Responsable.Campo_Descuento_Responsable_ID + " = '" + Descuento_Responsable.P_Descuento_Responsable_ID + "'";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }
                catch (OracleException Ex)
                {
                    if (Ex.Code == 547 || Ex.Code == 2292)
                    {
                        Mi_SQL = "UPDATE " + Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable;
                        Mi_SQL += " SET " + Cat_Ing_Descuentos_Responsable.Campo_Estatus + " = 'CANCELADO'";
                        Mi_SQL += " WHERE " + Cat_Ing_Descuentos_Responsable.Campo_Descuento_Responsable_ID + " = '" + Descuento_Responsable.P_Descuento_Responsable_ID + "'";
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                if (Descuento_Responsable.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Estatus_Eliminar = true;
            }
            catch (OracleException Ex)
            {
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar eliminar el registro de Tipos de Pagos. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Tipos de Pagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Descuento_Responsable.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }

            return Estatus_Eliminar;
        }
        #endregion

        #region Consultas

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Descuentos_Responsable
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos los datos del Descuento por Responsable
        ///PARAMETROS           : Descuento_Responsable, instancia de Cls_Cat_Ing_Descuentos_Responsable_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 25/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Descuentos_Responsable(Cls_Cat_Ing_Descuentos_Responsable_Negocio Descuento_Responsable)
        {
            DataTable Dt_Tipos_Pagos = new DataTable();
            String Mi_SQL;
            try
            {
                if (Descuento_Responsable.P_Campos_Dinamicos != null && Descuento_Responsable.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Descuento_Responsable.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT ";
                    Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Descuento_Responsable_ID + ", ";
                    Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Empleado_ID + ", ";
                    Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Estatus + ", ";
                    Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Porcentaje + ", ";
                    Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Descripcion + ", ";
                    Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Tipo + ", ";
                    if (Mi_SQL.EndsWith(", "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 2);
                    }
                }
                Mi_SQL += " FROM " + Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable;
                if (Descuento_Responsable.P_Join != null && Descuento_Responsable.P_Join != "")
                {
                    Mi_SQL += " " + Descuento_Responsable.P_Join;
                }
                if (Descuento_Responsable.P_Filtros_Dinamicos != null && Descuento_Responsable.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Descuento_Responsable.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Descuento_Responsable.P_Descuento_Responsable_ID != null && Descuento_Responsable.P_Descuento_Responsable_ID != "")
                    {
                        Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Descuento_Responsable_ID + " = '" + Descuento_Responsable.P_Descuento_Responsable_ID + "' AND ";
                    }
                    if (Descuento_Responsable.P_Empleado_ID != null && Descuento_Responsable.P_Empleado_ID != "")
                    {
                        Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Empleado_ID + " = '" + Descuento_Responsable.P_Empleado_ID + "' AND ";
                    }
                    if (Descuento_Responsable.P_Estatus != null && Descuento_Responsable.P_Estatus != "")
                    {
                        Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Estatus + " = '" + Descuento_Responsable.P_Estatus + "' AND ";
                    }
                    if (Descuento_Responsable.P_Tipo != null && Descuento_Responsable.P_Tipo != "")
                    {
                        Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Tipo + " = '" + Descuento_Responsable.P_Tipo + "' AND ";
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
                if (Descuento_Responsable.P_Agrupar_Dinamico != null && Descuento_Responsable.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Descuento_Responsable.P_Agrupar_Dinamico;
                }
                if (Descuento_Responsable.P_Ordenar_Dinamico != null && Descuento_Responsable.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Descuento_Responsable.P_Ordenar_Dinamico;
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Dt_Tipos_Pagos = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Tipos_Pagos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Porcentaje_Descuento
        ///DESCRIPCIÓN          : Consulta el Porcentaje Autorizado
        ///PARAMETROS           : Descuento_Responsable, instancia de Cls_Cat_Ing_Descuentos_Responsable_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 14/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Decimal Consultar_Porcentaje_Descuento(Cls_Cat_Ing_Descuentos_Responsable_Negocio Descuento_Responsable)
        {
            Decimal Porcentaje_Descuento = 0;
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Porcentaje;
                Mi_SQL += " FROM " + Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable;
                Mi_SQL += " WHERE ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Empleado_ID + " = '" + Descuento_Responsable.P_Empleado_ID + "' AND ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Tipo + " = '" + Descuento_Responsable.P_Tipo + "' AND ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable + "." + Cat_Ing_Descuentos_Responsable.Campo_Estatus + " = 'VIGENTE'";
                object Obj_Porcentaje_Descuento = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Obj_Porcentaje_Descuento != null)
                {
                    Porcentaje_Descuento = Convert.ToDecimal(Obj_Porcentaje_Descuento);
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Porcentaje_Descuento;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Existe
        ///DESCRIPCIÓN          : Devuelve un Boleano si encontró registros con los parámetros proporcionados
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 13/Agosto/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Validar_Existe(Cls_Cat_Ing_Descuentos_Responsable_Negocio Descuentos_Responsables)
        {
            DataSet Ds_Descuentos_Responsables = null;
            Boolean Existente = false;
            String Mi_SQL;

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Campo_Descuento_Responsable_ID + ", " + Cat_Ing_Descuentos_Responsable.Campo_Tipo;
                Mi_SQL += " FROM " + Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable;
                Mi_SQL += " WHERE ";
                if (Descuentos_Responsables.P_Descuento_Responsable_ID != null)
                {
                    if (Descuentos_Responsables.P_Descuento_Responsable_ID.Trim() != "")
                    {
                        Mi_SQL += Cat_Ing_Descuentos_Responsable.Campo_Descuento_Responsable_ID + " != '" + Descuentos_Responsables.P_Descuento_Responsable_ID + "' AND ";
                    }
                }
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Campo_Empleado_ID + " = '" + Descuentos_Responsables.P_Empleado_ID + "' AND ";
                Mi_SQL += Cat_Ing_Descuentos_Responsable.Campo_Tipo + " = '" + Descuentos_Responsables.P_Tipo + "'";

                Ds_Descuentos_Responsables = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Descuentos_Responsables != null)
                {
                    if (Ds_Descuentos_Responsables.Tables[0] != null)
                    {
                        if (Ds_Descuentos_Responsables.Tables[0].Rows.Count > 0)
                        {
                            Existente = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Movimientos. Error: [" + ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Existente;
        }

        #endregion

        #region Consulta de ID Consecutivo

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_ID_Consecutivo
        ///DESCRIPCIÓN          : Método para generar un nuevo Id del Catálogo
        ///PARAMETROS           : Cmmd, Referencia del Comando de la transacción avierta previamente por el método que lo crea.
        ///                       Campos, Columnas de la tabla en la base de datos a ser conusultadas
        ///                       Longitud_ID, tamaño final con formato del Nuevo_Valor_ID a generar
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 27/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Obtener_ID_Consecutivo(ref OracleCommand Cmmd, String Campos, Int32 Longitud_ID)
        {
            String Nuevo_Valor_ID = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campos + ") FROM " + Cat_Ing_Descuentos_Responsable.Tabla_Cat_Ing_Descuentos_Responsable;
                Cmmd.CommandText = Mi_SQL;
                Object Obj_Temp = Cmmd.ExecuteScalar();
                if (Obj_Temp != null)
                {
                    if (Obj_Temp.ToString() != "")
                    {
                        Nuevo_Valor_ID = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp.ToString()) + 1), Longitud_ID);
                    }
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Nuevo_Valor_ID;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Convertir_A_Formato_ID
        ///DESCRIPCIÓN          : Método para dar el formato y tamaño a la cadena del nuevo Id del Catálogo
        ///PARAMETROS           : Dato_ID, valor que va a ser formateado
        ///                       Longitud_ID, valore que se usará para dar la longitud final del nuevo Id del Catálogo
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 27/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
        {
            String Cadena_Formateada = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
            {
                Cadena_Formateada = Cadena_Formateada + "0";
            }
            Cadena_Formateada = Cadena_Formateada + Dato;
            return Cadena_Formateada;
        }

        #endregion

    }
}