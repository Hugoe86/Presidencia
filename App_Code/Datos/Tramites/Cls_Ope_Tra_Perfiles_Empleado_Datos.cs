using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
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
using Presidencia.Sessiones;
using Presidencia.Tramites_Perfiles_Empleados.Negocio;

namespace Presidencia.Tramites_Perfiles_Empleados.Datos
{
    public class Cls_Ope_Tra_Perfiles_Empleado_Datos
    {
        #region Consultas
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Empleado
        /// COMENTARIOS:    consultara todos los empleados
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// CREO:           Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:      25/Mayo/2012
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Empleado(Cls_Ope_Tra_Perfiles_Empleado_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append(", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus);
                Mi_SQL.Append(", ( Select " );
                Mi_SQL.Append(Cat_Dependencias.Campo_Nombre + " From " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_SQL.Append(" Where " + Cat_Dependencias.Campo_Dependencia_ID + "=" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append(" ) AS NOMBRE_DEPENDENCIA");
                Mi_SQL.Append(", (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado ");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append("( " + Cat_Empleados.Campo_Estatus + " is not null )");

                //  FILTRO UNIDAD RESPONSABLE
                if (!String.IsNullOrEmpty(Datos.P_Unidad_Responsable_ID))
                {
                    Mi_SQL.Append(" and " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Unidad_Responsable_ID + "'");
                }
                //  FILTRO NUMERO DE EMPLEADO
                if (!String.IsNullOrEmpty(Datos.P_Numero_Empleado))
                {
                    Mi_SQL.Append(" and " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_Numero_Empleado + "'");
                }
                //  FILTRO NOMBRE EMPLEADO
                if (!String.IsNullOrEmpty(Datos.P_Nombre_Empleado))
                {
                    Mi_SQL.Append(" AND (");
                    Mi_SQL.Append( "upper(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||");
                    Mi_SQL.Append(Cat_Empleados.Campo_Apellido_Materno + ") LIKE upper('%" + Datos.P_Nombre_Empleado + "%') ");
                    Mi_SQL.Append(" OR upper(" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" + Cat_Empleados.Campo_Apellido_Materno + "||' '||");
                    Mi_SQL.Append(Cat_Empleados.Campo_Nombre + ") LIKE upper('%" + Datos.P_Nombre_Empleado + "%') ");
                    Mi_SQL.Append(" )");
                }

                Mi_SQL.Append(" order by ");
                Mi_SQL.Append(Cat_Empleados.Campo_Estatus + "," + Cat_Empleados.Campo_Apellido_Paterno + "," + Cat_Empleados.Campo_Apellido_Materno + "," + Cat_Empleados.Campo_Nombre);
                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        /// *******************************************************************************
        /// NOMBRE:         Consultar_Perfil
        /// COMENTARIOS:    consultara el perfil seleccionado
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// CREO:           Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:      26/Mayo/2012
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Perfil(Cls_Ope_Tra_Perfiles_Empleado_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT  ");
                Mi_SQL.Append(Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado + "." + Ope_Tra_Perfiles_Empleado.Campo_Empleado_ID);
                Mi_SQL.Append(", " + Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado + "." + Ope_Tra_Perfiles_Empleado.Campo_Perfil_ID);
                Mi_SQL.Append(", (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado ");
                Mi_SQL.Append(", " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + "." + Cat_Tra_Perfiles.Campo_Nombre + " as NOMBRE_PERFIL ");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado + "." + Ope_Tra_Perfiles_Empleado.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" left outer join " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + " on ");
                Mi_SQL.Append(Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado + "." + Ope_Tra_Perfiles_Empleado.Campo_Perfil_ID + "=");
                Mi_SQL.Append(Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + "." + Cat_Tra_Perfiles.Campo_Perfil_ID);

                //  filtro para el perfil id
                if (!String.IsNullOrEmpty(Datos.P_Perfil_ID))
                {
                    Mi_SQL.Append(" Where ");
                    Mi_SQL.Append(Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + "." + Cat_Tra_Perfiles.Campo_Perfil_ID + "='" + Datos.P_Perfil_ID + "'");
                }

                //  filtro para el empleado id
                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    String Auxiliar = Mi_SQL.ToString();

                    if (Auxiliar.Contains("Where"))
                    {
                        Mi_SQL.Append(" And ");
                        Mi_SQL.Append(Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado + "." + Ope_Tra_Perfiles_Empleado.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" Where ");
                        Mi_SQL.Append(Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado + "." + Ope_Tra_Perfiles_Empleado.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                    }
                }
                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        /// *******************************************************************************
        /// NOMBRE:         Consultar_Perfil_Existentes
        /// COMENTARIOS:    consultara el perfil seleccionado
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// CREO:           Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:      26/Mayo/2012
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Perfil_Existentes(Cls_Ope_Tra_Perfiles_Empleado_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT  * ");
                Mi_SQL.Append(" From " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles);
                if (!String.IsNullOrEmpty(Datos.P_Nombre_Perfil))
                {
                    Mi_SQL.Append(" Where ");
                    Mi_SQL.Append(" upper(" + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + "." + Cat_Tra_Perfiles.Campo_Nombre + ") like ( upper ('%" + Datos.P_Nombre_Perfil + "%') )");
                }
                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
        #endregion

        #region Alta-Modifciacion-eliminar
        /// *******************************************************************************
        /// NOMBRE:         Alta_Perfil_Empleado
        /// COMENTARIOS:    SE dara de alta el perfil
        /// PARÁMETROS:      
        /// CREO:           Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:      26/Mayo/2012
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static Boolean Alta_Perfil_Empleado(Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Operacion_Completa = false;
            DataTable Dt_Empleado_Perfil = new DataTable();
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
                if (Negocio.P_Dt_Perfil_Empleado != null)
                {
                    if (Negocio.P_Dt_Perfil_Empleado.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Negocio.P_Dt_Perfil_Empleado.Rows)
                        {
                            Negocio.P_Empleado_ID = Registro["EMPLEADO_ID"].ToString();
                            Negocio.P_Perfil_ID = Registro["PERFIL_ID"].ToString();
                            Dt_Empleado_Perfil = Negocio.Consultar_Perfil();

                            //  se consulta que no exista el registro
                            if ((Dt_Empleado_Perfil.Rows.Count == 0))
                            {
                                Mi_SQL = new StringBuilder();
                                Mi_SQL.Append("INSERT INTO " + Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado + " (");
                                Mi_SQL.Append(Ope_Tra_Perfiles_Empleado.Campo_Empleado_ID);
                                Mi_SQL.Append(", " + Ope_Tra_Perfiles_Empleado.Campo_Perfil_ID);
                                Mi_SQL.Append(", " + Ope_Tra_Perfiles_Empleado.Campo_Usuario_Creo);
                                Mi_SQL.Append(", " + Ope_Tra_Perfiles_Empleado.Campo_Fecha_Creo);
                                Mi_SQL.Append(") VALUES (");
                                Mi_SQL.Append("'" + Registro["EMPLEADO_ID"].ToString() + "'");
                                Mi_SQL.Append(", '" + Registro["PERFIL_ID"].ToString() + "'");
                                Mi_SQL.Append(", '" + Cls_Sessiones.Nombre_Empleado + "'");
                                Mi_SQL.Append(", SYSDATE)");
                                Cmd.CommandText = Mi_SQL.ToString();
                                Cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                Trans.Commit();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Error al ejecutar el alta del perfil. Error: [" + Ex.Message + "]");
            }
            finally
            {
                Cn.Close();
            }

            return Operacion_Completa;
        }


        /// *******************************************************************************
        /// NOMBRE:         Modificar_Perfil_Empleado
        /// COMENTARIOS:    SE modificaran los perfil
        /// PARÁMETROS:      
        /// CREO:           Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:      26/Mayo/2012
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static Boolean Modificar_Perfil_Empleado(Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Operacion_Completa = false;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
                Mi_SQL.Append("Delete " + Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Ope_Tra_Perfiles_Empleado.Campo_Empleado_ID + "='" + Negocio.P_Empleado_ID + "'");
                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();

                Mi_SQL = new StringBuilder();
                if (Negocio.P_Dt_Perfil_Empleado != null)
                {
                    if (Negocio.P_Dt_Perfil_Empleado.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Negocio.P_Dt_Perfil_Empleado.Rows)
                        {
                            Mi_SQL = new StringBuilder();
                            Mi_SQL.Append("INSERT INTO " + Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado + " (");
                            Mi_SQL.Append(Ope_Tra_Perfiles_Empleado.Campo_Empleado_ID);
                            Mi_SQL.Append(", " + Ope_Tra_Perfiles_Empleado.Campo_Perfil_ID);
                            Mi_SQL.Append(", " + Ope_Tra_Perfiles_Empleado.Campo_Usuario_Creo);
                            Mi_SQL.Append(", " + Ope_Tra_Perfiles_Empleado.Campo_Fecha_Creo);
                            Mi_SQL.Append(") VALUES (");
                            Mi_SQL.Append("'" + Registro["EMPLEADO_ID"].ToString() + "'");
                            Mi_SQL.Append(", '" + Registro["PERFIL_ID"].ToString() + "'");
                            Mi_SQL.Append(", '" + Cls_Sessiones.Nombre_Empleado + "'");
                            Mi_SQL.Append(", SYSDATE)");
                            Cmd.CommandText = Mi_SQL.ToString();
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }
                Trans.Commit();
                Operacion_Completa = true;

            }
            catch (OracleException Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Error al ejecutar la modificacion del perfil. Error: [" + Ex.Message + "]");
            }
            finally
            {
                Cn.Close();
            }
            return Operacion_Completa;
        }

        /// *******************************************************************************
        /// NOMBRE:         Eliminar_Perfil_Empleado
        /// COMENTARIOS:    SE eliminara el perfil
        /// PARÁMETROS:      
        /// CREO:           Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:      26/Mayo/2012
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static Boolean Eliminar_Perfil_Empleado(Cls_Ope_Tra_Perfiles_Empleado_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            Boolean Operacion_Completa = false;
            try
            {
                if (Negocio.P_Cmmd != null)
                {
                    Comando = Negocio.P_Cmmd;
                }
                else
                {
                    Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Transaccion.Connection;
                    Comando.Transaction = Transaccion;
                }

                Mi_SQL.Append("Delete " + Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado);
                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Ope_Tra_Perfiles_Empleado.Campo_Perfil_ID + "='" + Negocio.P_Perfil_ID + "'");
                Mi_SQL.Append(" And ");
                Mi_SQL.Append(Ope_Tra_Perfiles_Empleado.Campo_Empleado_ID + "='" + Negocio.P_Empleado_ID + "'");
                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();
                
                if (Negocio.P_Cmmd == null)
                {
                    Transaccion.Commit();
                }
                
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                } 
                throw new Exception("Error al ejecutar el metodo de elimiar perfil. Error: [" + Ex.Message + "]");
            }
            finally
            {
                if (Negocio.P_Cmmd == null)
                {
                    Conexion.Close();
                }
            }
            return Operacion_Completa;
        }
        #endregion
    }
}