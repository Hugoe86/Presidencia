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
using Presidencia.Catalogo_Movimientos.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Movimientos_Datos
/// </summary>
/// 
namespace Presidencia.Catalogo_Movimientos.Datos
{
    public class Cls_Cat_Pre_Movimientos_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Movimiento
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nuevo Movimiento
        ///PARAMETROS:     
        ///             1. Movimiento.    Instancia de la Clase de Negocio de Cls_Cat_Pre_Movimiento_Negocio
        ///                             con los datos del Movimiento que va a ser dado de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Movimiento(Cls_Cat_Pre_Movimientos_Negocio Movimiento)
        {
            String Mensaje = "";
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
                String Movimiento_ID = Obtener_ID_Consecutivo(Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos, Cat_Pre_Movimientos.Campo_Movimiento_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Movimientos.Campo_Movimiento_ID + ", " + Cat_Pre_Movimientos.Campo_Identificador;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Movimientos.Campo_Aplica;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Movimientos.Campo_Cargar_Modulos;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Movimientos.Campo_Descripcion + ", " + Cat_Pre_Movimientos.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Movimientos.Campo_Grupo_Id;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Movimientos.Campo_Usuario_Creo + ", " + Cat_Pre_Movimientos.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Movimiento_ID + "', '" + Movimiento.P_Identificador + "'";
                Mi_SQL = Mi_SQL + ",'" + Movimiento.P_Aplica + "'";
                Mi_SQL = Mi_SQL + ",'" + Movimiento.P_Cargar_Modulos + "'";
                Mi_SQL = Mi_SQL + ",'" + Movimiento.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Movimiento.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Movimiento.P_Grupo_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Movimiento.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Movimientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Cn.State == ConnectionState.Open)
                {
                    Cn.Close();
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Movimiento
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Movimiento
        ///PARAMETROS:     
        ///             1. Movimiento.  Instancia de la Clase de Negocio de Movimiento con los datos 
        ///                              del Registro que va a ser Actualizado.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Movimiento(Cls_Cat_Pre_Movimientos_Negocio Movimiento)
        {
            String Mensaje = "";
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " SET " + Cat_Pre_Movimientos.Campo_Identificador + " = '" + Movimiento.P_Identificador + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Movimientos.Campo_Estatus + " = '" + Movimiento.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Movimientos.Campo_Aplica + " = '" + Movimiento.P_Aplica + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Movimientos.Campo_Cargar_Modulos + " = '" + Movimiento.P_Cargar_Modulos + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Movimientos.Campo_Descripcion + " = '" + Movimiento.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Movimientos.Campo_Grupo_Id + " = '" + Movimiento.P_Grupo_ID + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Movimientos.Campo_Usuario_Modifico + " = '" + Movimiento.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Movimientos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = '" + Movimiento.P_Movimiento_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
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
                    Mensaje = "Error al intentar modificar un Registro de Movimiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Movimientos
        ///DESCRIPCIÓN: Obtiene todos Movimientos que estan dadas de alta en la Base de Datos
        ///PARAMETROS:   
        ///             1.  Movimiento.   Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                               caso el filtro es el Identificador.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 14/Diciembre/2010
        ///CAUSA_MODIFICACIÓN   : Adecuar funcionalidad para posibilitar la consulta armada por campos del select, filtros y ordenamientos
        ///*******************************************************************************
        public static DataTable Consultar_Movimientos(Cls_Cat_Pre_Movimientos_Negocio Movimiento)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            String Mi_SQL_Campos_Foraneos = "";

            try
            {
                //DESCOMENTAR EL SIGUIENTE BLOQUE IF SI SE AGREGAN CAMPOS FORANEOS EN EL CATÁLOGO
                //if (Multa.P_Incluir_Campos_Foraneos)
                //{
                //    Mi_SQL_Campos_Foraneos = Mi_SQL_Campos_Foraneos + "(SELECT " + " FROM " + " WHERE " + " = " + "." + ") AS IDENTIFICADOR, ";
                //}
                if (Movimiento.P_Campos_Dinamicos != null && Movimiento.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos + Movimiento.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT " + Mi_SQL_Campos_Foraneos;
                    Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Movimiento_ID + " AS MOVIMIENTO_ID, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Identificador + " AS IDENTIFICADOR, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Estatus + " AS ESTATUS, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Aplica + " AS APLICA, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Descripcion + " AS DESCRIPCION, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Movimientos.Campo_Cargar_Modulos;
                }
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                if (Movimiento.P_Filtros_Dinamicos != null && Movimiento.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Movimiento.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Movimientos.Campo_Identificador + " LIKE '%" + Movimiento.P_Identificador + "%' OR " + Cat_Pre_Movimientos.Campo_Descripcion + " LIKE '%" + Movimiento.P_Identificador + "%'";
                    //DESCOMENTAR EL SIGUIENTE BLOQUE IF SI SE AGREGAN FILTROS EN ESTA SECCIÓN
                    //if (Mi_SQL.EndsWith(" AND "))
                    //{
                    //    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    //}
                    //DESCOMENTAR EL SIGUIENTE BLOQUE WHERE SI SE QUITA EL CAMPO CONCEPTO_PREDIAL_ID DE LA LÍNEA DEL WHERE
                    //if (Mi_SQL.EndsWith(" WHERE "))
                    //{
                    //    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    //}
                }
                if (Movimiento.P_Agrupar_Dinamico != null && Movimiento.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " GROUP BY " + Movimiento.P_Agrupar_Dinamico;
                }
                if (Movimiento.P_Ordenar_Dinamico != null && Movimiento.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Movimiento.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Movimientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Movimiento
        ///DESCRIPCIÓN: Obtiene a detalle un Movimiento.
        ///PARAMETROS:   
        ///             1. P_Movimiento.   Movimiento que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Movimientos_Negocio Consultar_Datos_Movimiento(Cls_Cat_Pre_Movimientos_Negocio P_Movimiento)
        {
            String Mi_SQL = "SELECT " + Cat_Pre_Movimientos.Campo_Identificador + ", " + Cat_Pre_Movimientos.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Movimientos.Campo_Aplica + ", " + Cat_Pre_Movimientos.Campo_Cargar_Modulos;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Movimientos.Campo_Grupo_Id;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = '" + P_Movimiento.P_Movimiento_ID + "'";
            Cls_Cat_Pre_Movimientos_Negocio R_Movimiento = new Cls_Cat_Pre_Movimientos_Negocio();
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Movimiento.P_Movimiento_ID = P_Movimiento.P_Movimiento_ID;
                while (Data_Reader.Read())
                {
                    R_Movimiento.P_Identificador = Data_Reader[Cat_Pre_Movimientos.Campo_Identificador].ToString();
                    R_Movimiento.P_Estatus = Data_Reader[Cat_Pre_Movimientos.Campo_Estatus].ToString();
                    R_Movimiento.P_Aplica = Data_Reader[Cat_Pre_Movimientos.Campo_Aplica].ToString();
                    R_Movimiento.P_Cargar_Modulos = Data_Reader[Cat_Pre_Movimientos.Campo_Cargar_Modulos].ToString();
                    R_Movimiento.P_Grupo_ID = Data_Reader[Cat_Pre_Movimientos.Campo_Grupo_Id].ToString();
                    R_Movimiento.P_Descripcion = Data_Reader[Cat_Pre_Movimientos.Campo_Descripcion].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Movimiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Movimiento;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre_Id_Movimientos
        ///DESCRIPCIÓN: Obtiene los Movimientos para asignarselos
        ///             a un combo.
        ///PARAMENTROS:
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 11/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Nombre_Id_Movimientos(Cls_Cat_Pre_Movimientos_Negocio Movimientos)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Movimientos.Campo_Movimiento_ID + ", " + Cat_Pre_Movimientos.Campo_Identificador + "||' - '||" + Cat_Pre_Movimientos.Campo_Descripcion + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Movimientos.Campo_Estatus + "='VIGENTE'";
                if (Movimientos.P_Cargar_Modulos.Trim() != "")
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Movimientos.Campo_Cargar_Modulos + Validar_Operador_Comparacion(Movimientos.P_Cargar_Modulos);
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Movimientos.Campo_Identificador;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Movimientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre_Id_Movimientos
        ///DESCRIPCIÓN: Obtiene los Movimientos para asignarselos
        ///             a un combo que solo inicien con el identificador en B para la Cancelacion
        ///PARAMENTROS:
        ///CREO: Jacqueline Ramirez Sierra
        ///FECHA_CREO: 13/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Movimientos_Cancelacion(Cls_Cat_Pre_Movimientos_Negocio Movimientos)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Movimientos.Campo_Movimiento_ID + ", " + Cat_Pre_Movimientos.Campo_Identificador + "||' - '||" + Cat_Pre_Movimientos.Campo_Descripcion + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Movimientos.Campo_Estatus + "='VIGENTE'";
                //Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Movimientos.Campo_Identificador + " LIKE '%B%'";
                if (Movimientos.P_Cargar_Modulos.Trim() != "")
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Movimientos.Campo_Cargar_Modulos + Validar_Operador_Comparacion(Movimientos.P_Cargar_Modulos);
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Movimientos.Campo_Identificador;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Movimientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN     : Consultar_Movimientos_Bajas_Directas
        ///DESCRIPCIÓN              : Obtiene los Movimientos que solo inicien con el identificador en B para las Bajas
        ///PARAMENTROS:
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Movimientos_Bajas_Directas(Cls_Cat_Pre_Movimientos_Negocio Movimientos)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Movimientos.Campo_Movimiento_ID + ", " + Cat_Pre_Movimientos.Campo_Identificador + "||' - '||" + Cat_Pre_Movimientos.Campo_Descripcion + " AS IDENTIFICADOR";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Movimientos.Campo_Estatus + "='VIGENTE'";
                //Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Movimientos.Campo_Identificador + " LIKE '%B%'";
                if (Movimientos.P_Cargar_Modulos.Trim() != "")
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Movimientos.Campo_Cargar_Modulos + Validar_Operador_Comparacion(Movimientos.P_Cargar_Modulos);
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Movimientos.Campo_Identificador;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Movimientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Movimiento
        ///DESCRIPCIÓN: Elimina un Movmimento de la Base de Datos.
        ///PARAMETROS:   
        ///             1. Movimiento.   Registro que se va a eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Movimiento(Cls_Cat_Pre_Movimientos_Negocio Movimiento)
        {
            String Mensaje = "";
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                Mi_SQL += " SET " + Cat_Pre_Movimientos.Campo_Estatus + "='BAJA'";
                Mi_SQL += ", " + Cat_Pre_Movimientos.Campo_Usuario_Modifico + "='" + Movimiento.P_Usuario + "'";
                Mi_SQL += ", " + Cat_Pre_Movimientos.Campo_Fecha_Modifico + "=SYSDATE";
                Mi_SQL += " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = '" + Movimiento.P_Movimiento_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar eliminar el registro de Movimiento. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Movimiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
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
                if (Filtro.Trim().ToUpper().StartsWith("NULL")
                    || Filtro.Trim().ToUpper().StartsWith("NOT NULL"))
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Existe
        ///DESCRIPCIÓN          : Devuelve un DataTable si encontró registros con los parámetros proporcionados
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 14/Diciembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Validar_Existe(Cls_Cat_Pre_Movimientos_Negocio Movimiento)
        {
            DataTable Dt_Movimientos = null;
            DataSet Ds_Movimientos = null;
            Boolean Existente = false;
            String Mi_SQL;

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += Cat_Pre_Movimientos.Campo_Movimiento_ID + ", " + Cat_Pre_Movimientos.Campo_Identificador;
                Mi_SQL += " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                Mi_SQL += " WHERE ";
                if (Movimiento.P_Movimiento_ID != null)
                {
                    if (Movimiento.P_Movimiento_ID.Trim() != "")
                    {
                        Mi_SQL += Cat_Pre_Movimientos.Campo_Movimiento_ID + " != '" + Movimiento.P_Movimiento_ID + "' AND ";
                    }
                }
                //Mi_SQL += Cat_Pre_Movimientos.Campo_Estatus + " = '" + Movimiento.P_Estatus + "' AND ";
                Mi_SQL += Cat_Pre_Movimientos.Campo_Identificador + " = '" + Movimiento.P_Identificador + "' AND ";
                Mi_SQL += Cat_Pre_Movimientos.Campo_Aplica + " = '" + Movimiento.P_Aplica + "'";

                Ds_Movimientos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Movimientos != null)
                {
                    if (Ds_Movimientos.Tables[0] != null)
                    {
                        if (Ds_Movimientos.Tables[0].Rows.Count > 0)
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
    }
}