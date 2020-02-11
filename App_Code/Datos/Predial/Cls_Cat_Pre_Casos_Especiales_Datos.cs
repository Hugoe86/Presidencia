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
using Presidencia.Catalogo_Casos_Especiales.Negocio;
/// <summary>
/// Summary description for Cls_Cat_Pre_Casos_Especiales_Datos
/// </summary>

namespace Presidencia.Catalogo_Casos_Especiales.Datos
{
    public class Cls_Cat_Pre_Casos_Especiales_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Caso_Especial
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nuevo Caso Especial
        ///PARAMETROS:     
        ///             1. Caso_Especial.   Instancia de la Clase de Negocio de Caso Especial
        ///                                 con los datos del Caso Especial que va a ser dado de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Caso_Especial(Cls_Cat_Pre_Casos_Especiales_Negocio Caso_Especial)
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
                String Caso_Especial_ID = Obtener_ID_Consecutivo(Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales, Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + ", " + Cat_Pre_Casos_Especiales.Campo_Identificador;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Casos_Especiales.Campo_Descripcion + ", " + Cat_Pre_Casos_Especiales.Campo_Articulo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Casos_Especiales.Campo_Inciso;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Casos_Especiales.Campo_Tipo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Casos_Especiales.Campo_Porcentaje;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Casos_Especiales.Campo_Observaciones + ", " + Cat_Pre_Casos_Especiales.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Casos_Especiales.Campo_Usuario_Creo + ", " + Cat_Pre_Casos_Especiales.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Caso_Especial_ID + "', '" + Caso_Especial.P_Identificador + "'";
                Mi_SQL = Mi_SQL + ",'" + Caso_Especial.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Caso_Especial.P_Articulo + "'";
                Mi_SQL = Mi_SQL + ",'" + Caso_Especial.P_Inciso + "'";
                Mi_SQL = Mi_SQL + ",'" + Caso_Especial.P_Tipo + "'";
                if (Caso_Especial.P_Porcentaje != null && Caso_Especial.P_Porcentaje != "")
                {
                    Mi_SQL = Mi_SQL + "," + Caso_Especial.P_Porcentaje + "";
                }
                else
                {
                    Mi_SQL = Mi_SQL + ", NULL";
                }
                Mi_SQL = Mi_SQL + ",'" + Caso_Especial.P_Observaciones + "'";
                Mi_SQL = Mi_SQL + ",'" + Caso_Especial.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Caso_Especial.P_Usuario + "'";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Caso Especial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre_Beneficios
        ///DESCRIPCIÓN: Consultar los Beneficios por su Descripcion
        ///PARAMETROS:  
        ///CREO: Jacqueline Ramírez Sierra
        ///FECHA_CREO: 16-agosto-2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Nombre_Beneficios(Cls_Cat_Pre_Casos_Especiales_Negocio Caso_Especial)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + ", " + Cat_Pre_Casos_Especiales.Campo_Descripcion + "";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Módulos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Caso_Especial
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Caso Especial
        ///PARAMETROS:     
        ///             1. Caso_Especial.   Instancia de la Clase de Negocio de Caso Especial con los datos 
        ///                                 del Registro que va a ser Actualizado.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Caso_Especial(Cls_Cat_Pre_Casos_Especiales_Negocio Caso_Especial)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + " SET " + Cat_Pre_Casos_Especiales.Campo_Identificador + " = '" + Caso_Especial.P_Identificador + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Casos_Especiales.Campo_Descripcion + " = '" + Caso_Especial.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Casos_Especiales.Campo_Articulo + " = '" + Caso_Especial.P_Articulo + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Casos_Especiales.Campo_Inciso + " = '" + Caso_Especial.P_Inciso + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Casos_Especiales.Campo_Tipo + " = '" + Caso_Especial.P_Tipo + "'";
                if (Caso_Especial.P_Porcentaje != null)
                {
                    Mi_SQL = Mi_SQL + "," + Cat_Pre_Casos_Especiales.Campo_Porcentaje + " = " + Caso_Especial.P_Porcentaje + "";
                }
                else 
                {
                    Mi_SQL = Mi_SQL + "," + Cat_Pre_Casos_Especiales.Campo_Porcentaje + " = NULL";
                }
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Casos_Especiales.Campo_Observaciones + " = '" + Caso_Especial.P_Observaciones + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Casos_Especiales.Campo_Estatus + " = '" + Caso_Especial.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Casos_Especiales.Campo_Usuario_Modifico + " = '" + Caso_Especial.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Casos_Especiales.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = '" + Caso_Especial.P_Caso_Especial_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Caso Especial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Casos_Especiales
        ///DESCRIPCIÓN: Obtiene todos los Casos Especiales que estan dadas de alta en la Base de Datos
        ///PARAMETROS:   
        ///             1.  Caso_Especial.  Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es el Identificador.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 17/Diciembre/2010
        ///CAUSA_MODIFICACIÓN   : Adecuar funcionalidad dinámica para consultas
        ///*******************************************************************************
        public static DataTable Consultar_Casos_Especiales(Cls_Cat_Pre_Casos_Especiales_Negocio Caso_Especial)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;

            try
            {
                if (Caso_Especial.P_Campos_Dinamicos != null && Caso_Especial.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Caso_Especial.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " AS CASO_ESPECIAL_ID, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Identificador + " AS IDENTIFICADOR, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Descripcion + " AS DESCRIPCION, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Estatus + " AS ESTATUS, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Porcentaje + " AS PORCENTAJE, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Tipo + " AS TIPO";
                }
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales;
                if (Caso_Especial.P_Filtros_Dinamicos != null && Caso_Especial.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Caso_Especial.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Casos_Especiales.Campo_Identificador + " LIKE '%" + Caso_Especial.P_Identificador + "%' ";
                    Mi_SQL = Mi_SQL + " OR " + Cat_Pre_Casos_Especiales.Campo_Descripcion + " LIKE '%" + Caso_Especial.P_Identificador + "%'";
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
                if (Caso_Especial.P_Agrupar_Dinamico != null && Caso_Especial.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " GROUP BY " + Caso_Especial.P_Agrupar_Dinamico;
                }
                if (Caso_Especial.P_Ordenar_Dinamico != null && Caso_Especial.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Caso_Especial.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID;
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Casos Especiales. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Caso_Especial
        ///DESCRIPCIÓN: Obtiene a detalle un Caso Especial.
        ///PARAMETROS:   
        ///             1. P_Caso_Especial.   Caso Especial que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Casos_Especiales_Negocio Consultar_Datos_Caso_Especial(Cls_Cat_Pre_Casos_Especiales_Negocio P_Caso_Especial)
        {
            String Mi_SQL = "SELECT " + Cat_Pre_Casos_Especiales.Campo_Identificador + ", " + Cat_Pre_Casos_Especiales.Campo_Descripcion;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Casos_Especiales.Campo_Articulo + ", " + Cat_Pre_Casos_Especiales.Campo_Inciso;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Casos_Especiales.Campo_Observaciones;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Casos_Especiales.Campo_Porcentaje;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Casos_Especiales.Campo_Estatus + ", " + Cat_Pre_Casos_Especiales.Campo_Tipo + " FROM " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = '" + P_Caso_Especial.P_Caso_Especial_ID + "'";
            Cls_Cat_Pre_Casos_Especiales_Negocio R_Caso_Especial = new Cls_Cat_Pre_Casos_Especiales_Negocio();
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Caso_Especial.P_Caso_Especial_ID = P_Caso_Especial.P_Caso_Especial_ID;
                while (Data_Reader.Read())
                {
                    R_Caso_Especial.P_Identificador = Data_Reader[Cat_Pre_Casos_Especiales.Campo_Identificador].ToString();
                    R_Caso_Especial.P_Descripcion = Data_Reader[Cat_Pre_Casos_Especiales.Campo_Descripcion].ToString();
                    R_Caso_Especial.P_Articulo = Data_Reader[Cat_Pre_Casos_Especiales.Campo_Articulo].ToString();
                    R_Caso_Especial.P_Inciso = Data_Reader[Cat_Pre_Casos_Especiales.Campo_Inciso].ToString();
                    R_Caso_Especial.P_Observaciones = Data_Reader[Cat_Pre_Casos_Especiales.Campo_Observaciones].ToString();
                    R_Caso_Especial.P_Estatus = Data_Reader[Cat_Pre_Casos_Especiales.Campo_Estatus].ToString();
                    R_Caso_Especial.P_Tipo = Data_Reader[Cat_Pre_Casos_Especiales.Campo_Tipo].ToString();
                    R_Caso_Especial.P_Porcentaje = Data_Reader[Cat_Pre_Casos_Especiales.Campo_Porcentaje].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Caso Especial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Caso_Especial;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Reporte
        ///DESCRIPCIÓN          : Obtiene las Descrpciones de los Casos Especiales de acuerdo a los filtros establecidos en la interfaz
        ///PARAMETROS           : Casos_Especiales, instancia de Cls_Cat_Pre_Casos_Especiales_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 11/Enero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Datos_Reporte(Cls_Cat_Pre_Casos_Especiales_Negocio Casos_Especiales)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            try
            {
                if (Casos_Especiales.P_Campos_Dinamicos != null && Casos_Especiales.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Casos_Especiales.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT ";
                    Mi_SQL += Cat_Pre_Casos_Especiales.Campo_Identificador + ", ";
                    Mi_SQL += Cat_Pre_Casos_Especiales.Campo_Descripcion + ", ";
                    Mi_SQL += Cat_Pre_Casos_Especiales.Campo_Articulo + ", ";
                    Mi_SQL += Cat_Pre_Casos_Especiales.Campo_Inciso + ", ";
                    Mi_SQL += Cat_Pre_Casos_Especiales.Campo_Aplicar_Descuento + ", ";
                    Mi_SQL += Cat_Pre_Casos_Especiales.Campo_Tipo + ", ";
                    Mi_SQL += Cat_Pre_Casos_Especiales.Campo_Porcentaje + ", ";
                    Mi_SQL += Cat_Pre_Casos_Especiales.Campo_Estatus;
                }
                Mi_SQL += " FROM " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales;
                //if (Casos_Especiales.P_Unir_Tablas != null && Casos_Especiales.P_Unir_Tablas != "")
                //{
                //    Mi_SQL += ", " + Casos_Especiales.P_Unir_Tablas;
                //}
                //else
                //{
                //    if (Casos_Especiales.P_Join != null && Casos_Especiales.P_Join != "")
                //    {
                //        Mi_SQL += " " + Casos_Especiales.P_Join;
                //    }
                //}
                if (Casos_Especiales.P_Filtros_Dinamicos != null && Casos_Especiales.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Casos_Especiales.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Casos_Especiales.P_Caso_Especial_ID != null && Casos_Especiales.P_Caso_Especial_ID != "")
                    {
                        Mi_SQL += Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = '" + Casos_Especiales.P_Caso_Especial_ID + "' AND ";
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
                if (Casos_Especiales.P_Agrupar_Dinamico != null && Casos_Especiales.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Casos_Especiales.P_Agrupar_Dinamico;
                }
                if (Casos_Especiales.P_Ordenar_Dinamico != null && Casos_Especiales.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Casos_Especiales.P_Ordenar_Dinamico;
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
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Caso_Especial
        ///DESCRIPCIÓN: Elimina un Caso Especial de la Base de Datos.
        ///PARAMETROS:   
        ///             1. Caso_Especial.   Registro que se va a eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Caso_Especial(Cls_Cat_Pre_Casos_Especiales_Negocio Caso_Especial)
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = '" + Caso_Especial.P_Caso_Especial_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos";
                }
                else
                {
                    Mensaje = "Error al intentar eliminar el registro de Bloqueo Cuentas Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Bloqueo Cuentas Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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

    }
}