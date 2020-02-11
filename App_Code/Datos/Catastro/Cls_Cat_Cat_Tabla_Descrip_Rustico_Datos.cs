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
using Presidencia.Catalogo_Cat_Tabla_Descrip_Rustico.Negocio;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using System.Data;
using System.Data.OracleClient;
using Presidencia.Sessiones;


/// <summary>
/// Summary description for Cls_Cat_Cat_Tabla_Descrip_Rustico_Datos
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Tabla_Descrip_Rustico.Datos
{
    public class Cls_Cat_Cat_Tabla_Descrip_Rustico_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Valor_Construccion_Rustico
        ///DESCRIPCIÓN: Da de alta en la Base de Datos el tipo de construccion Rústico con su tabla de valores
        ///PARAMENTROS:     
        ///             1. Tabla_Val.       Instancia de la Clase de Negocio de Tabla de valores de construcción Rústico
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Valor_Rustico(Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio Tabla_Val)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            String Descrip_Rustico_Id = "";
            Descrip_Rustico_Id = Obtener_ID_Consecutivo(Cat_Cat_Tabla_Descrip_Rustico.Tabla_Cat_Cat_Tabla_Descrip_Rustico, Cat_Cat_Tabla_Descrip_Rustico.Campo_Descrip_Rustico_Id, "", 5);
            try
            {

                foreach (DataRow Dr_Renglon in Tabla_Val.P_Dt_Tabla_Descrip_Rustico.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["DESCRIPCION_RUSTICO_ID"].ToString().Trim().Replace("&nbsp;", "") != "")
                    {
                        Mi_sql = "DELETE " + Cat_Cat_Tabla_Descrip_Rustico.Tabla_Cat_Cat_Tabla_Descrip_Rustico + " WHERE " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Descrip_Rustico_Id;
                        Mi_sql += "='" + Dr_Renglon["DESCRIPCION_RUSTICO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Cat_Cat_Tabla_Descrip_Rustico.Tabla_Cat_Cat_Tabla_Descrip_Rustico + "(";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Descrip_Rustico_Id + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Des_Constru_Rustico_Id + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Anio + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A + ", ";
                        //Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_B + ", ";
                        //Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_C + ", ";
                        //Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_D + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Usuario_Creo + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Descrip_Rustico_Id + "', '";
                        Mi_sql += Tabla_Val.P_Des_Constru_Rustico_Id + "', ";
                        Mi_sql += Dr_Renglon["ANIO"].ToString() + ", ";
                        Mi_sql += Dr_Renglon["VALOR_INDICE"].ToString() + ", '";
                        Mi_sql += Dr_Renglon["INDICADOR_A"].ToString() + "', '";
                        //Mi_sql += Dr_Renglon["INDICADOR_B"].ToString() + "', '";
                        //Mi_sql += Dr_Renglon["INDICADOR_C"].ToString() + "', '";
                        //Mi_sql += Dr_Renglon["INDICADOR_D"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        Descrip_Rustico_Id = (Convert.ToInt16(Descrip_Rustico_Id) + 1).ToString("00000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ACTUALIZAR")
                    {
                        Mi_sql = "UPDATE " + Cat_Cat_Tabla_Descrip_Rustico.Tabla_Cat_Cat_Tabla_Descrip_Rustico;
                        Mi_sql += " SET " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Anio + " = " + Dr_Renglon["ANIO"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice + " = " + Dr_Renglon["VALOR_INDICE"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A + " = '" + Dr_Renglon["INDICADOR_A"].ToString() + "', ";
                        //Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_B + " = '" + Dr_Renglon["INDICADOR_B"].ToString() + "', ";
                        //Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_C + " = '" + Dr_Renglon["INDICADOR_C"].ToString() + "', ";
                        //Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_D + " = '" + Dr_Renglon["INDICADOR_D"].ToString() + "', ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Descrip_Rustico_Id + "='" + Dr_Renglon["DESCRIPCION_RUSTICO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Descripcion_Rustico: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Valor_Construccion_Rustico
        ///DESCRIPCIÓN: Modifica en la Base de Datos el tipo de construcción Rústico y elimina, agrega y/o modifica la tabla de valores
        ///PARAMENTROS:     
        ///             1. Tabla_Val.       Instancia de la Clase de Negocio de Tabla de valores de construcción Rústico
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta, eliminados y/o modificados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Valor_Rustico(Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio Tabla_Val)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            String Valor_Rustico_Id = "";
            Valor_Rustico_Id = Obtener_ID_Consecutivo(Cat_Cat_Tabla_Descrip_Rustico.Tabla_Cat_Cat_Tabla_Descrip_Rustico, Cat_Cat_Tabla_Descrip_Rustico.Campo_Descrip_Rustico_Id, "", 5);
            try
            {

                //Mi_sql = "UPDATE " + Cat_Cat_Descrip_Const_Rustico.Tabla_Cat_Cat_Descrip_Const_Rustico;
                //Mi_sql += " SET " + Cat_Cat_Descrip_Const_Rustico.Campo_Identificador + " = '" + Tabla_Val.P_Identificador + "', ";
                //Mi_sql += Cat_Cat_Descrip_Const_Rustico.Campo_Estatus + " = '" + Tabla_Val.P_Estatus + "', ";
                //Mi_sql += Cat_Cat_Descrip_Const_Rustico.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                //Mi_sql += Cat_Cat_Descrip_Const_Rustico.Campo_Fecha_Modifico + " = SYSDATE";
                //Mi_sql += " WHERE " + Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id + " = '" + Tabla_Val.P_Des_Constru_Rustico_Id + "'";
                //Cmd.CommandText = Mi_sql;
                //Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Tabla_Val.P_Dt_Tabla_Descrip_Rustico.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["DESCRIPCION_RUSTICO_ID"].ToString().Trim().Replace("&nbsp;", "") != "")
                    {
                        Mi_sql = "DELETE " + Cat_Cat_Tabla_Descrip_Rustico.Tabla_Cat_Cat_Tabla_Descrip_Rustico + " WHERE " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Descrip_Rustico_Id;
                        Mi_sql += "='" + Dr_Renglon["DESCRIPCION_RUSTICO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Cat_Cat_Tabla_Descrip_Rustico.Tabla_Cat_Cat_Tabla_Descrip_Rustico + "(";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Descrip_Rustico_Id + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Des_Constru_Rustico_Id + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Anio + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A + ", ";
                        //Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_B + ", ";
                        //Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_C + ", ";
                        //Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_D + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Usuario_Creo + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Valor_Rustico_Id + "', '";
                        Mi_sql += Tabla_Val.P_Des_Constru_Rustico_Id + "', ";
                        Mi_sql += Dr_Renglon["ANIO"].ToString() + ", ";
                        Mi_sql += Dr_Renglon["VALOR_INDICE"].ToString() + ", '";
                        Mi_sql += Dr_Renglon["INDICADOR_A"].ToString() + "', '";
                        //Mi_sql += Dr_Renglon["INDICADOR_B"].ToString() + "', '";
                        //Mi_sql += Dr_Renglon["INDICADOR_C"].ToString() + "', '";
                        //Mi_sql += Dr_Renglon["INDICADOR_D"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        Valor_Rustico_Id = (Convert.ToInt16(Valor_Rustico_Id) + 1).ToString("00000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ACTUALIZAR")
                    {
                        Mi_sql = "UPDATE " + Cat_Cat_Tabla_Descrip_Rustico.Tabla_Cat_Cat_Tabla_Descrip_Rustico;
                        Mi_sql += " SET " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Anio + " = " + Dr_Renglon["ANIO"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice + " = " + Dr_Renglon["VALOR_INDICE"].ToString() + ", ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A + " = '" + Dr_Renglon["INDICADOR_A"].ToString() + "', ";
                        //Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_B + " = '" + Dr_Renglon["INDICADOR_B"].ToString() + "', ";
                        //Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_C + " = '" + Dr_Renglon["INDICADOR_C"].ToString() + "', ";
                        //Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_D + " = '" + Dr_Renglon["INDICADOR_D"].ToString() + "', ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Cat_Cat_Tabla_Descrip_Rustico.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Descrip_Rustico_Id + "='" + Dr_Renglon["DESCRIPCION_RUSTICO_ID"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Valor_Indice: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Valores_Construccion_Rustico
        ///DESCRIPCIÓN: Obtiene la tabla de valores que estan dados de alta en la Base de Datos de un tipo de construcción Rustico
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Valores_Rustico(Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio Tabla_Val)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Descrip_Rustico_Id
                    + ", " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Des_Constru_Rustico_Id
                    + ", " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Anio
                    + ", 'NADA' AS ACCION"
                    + ", " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice
                    + ", " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A
                    //+ ", " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_B
                    //+ ", " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_C
                    //+ ", " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_D
                    + ", " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Usuario_Creo
                    + ", " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Fecha_Creo
                    + " FROM  " + Cat_Cat_Tabla_Descrip_Rustico.Tabla_Cat_Cat_Tabla_Descrip_Rustico
                    + " WHERE ";
                if (Tabla_Val.P_Des_Constru_Rustico_Id != null && Tabla_Val.P_Des_Constru_Rustico_Id.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tabla_Descrip_Rustico.Campo_Des_Constru_Rustico_Id + " = '" + Tabla_Val.P_Des_Constru_Rustico_Id + "' AND ";
                }
                if (Tabla_Val.P_Descripcion_Rustico_Id != null && Tabla_Val.P_Descripcion_Rustico_Id.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tabla_Descrip_Rustico.Campo_Descrip_Rustico_Id + " = '" + Tabla_Val.P_Descripcion_Rustico_Id + "' AND ";
                }
                if (Tabla_Val.P_Anio != null && Tabla_Val.P_Anio.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tabla_Descrip_Rustico.Campo_Anio + " = " + Tabla_Val.P_Anio + " AND ";
                }
                if (Tabla_Val.P_Valor_Indice != null && Tabla_Val.P_Valor_Indice.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice + " = " + Tabla_Val.P_Valor_Indice + " AND ";
                }
                if (Tabla_Val.P_Indicador_A != null && Tabla_Val.P_Indicador_A.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A + " = " + Tabla_Val.P_Indicador_A + " AND ";
                }
                //if (Tabla_Val.P_Indicador_B != null && Tabla_Val.P_Indicador_B.Trim() != "")

                //{
                //    Mi_SQL += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_B + " = " + Tabla_Val.P_Indicador_B + " AND ";
                //}
                //if (Tabla_Val.P_Indicador_C != null && Tabla_Val.P_Indicador_C.Trim() != "")
                //{
                //    Mi_SQL += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_C + " = " + Tabla_Val.P_Indicador_C + " AND ";
                //}
                //if (Tabla_Val.P_Indicador_D != null && Tabla_Val.P_Indicador_D.Trim() != "")
                //{
                //    Mi_SQL += Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_D + " = " + Tabla_Val.P_Indicador_D + " AND ";
                //}
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                // agregar filtro y orden a la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Tabla de Descripción Rústica . Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tipos_Construccion_Rustico
        ///DESCRIPCIÓN: Consulta los motivos de Avaluo
        ///PARAMENTROS:     
        ///             1. Tipo_Construccion.         Instancia de la Clase de Negocio de Tipos de construccion Rústico
        ///                                 con los datos que servirán de
        ///                                 filtro.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Descripcion_Construccion_Rustico(Cls_Cat_Cat_Tabla_Descrip_Rustico_Negocio Descripcion_Construccion)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id
                    + ", " + Cat_Cat_Descrip_Const_Rustico.Campo_Identificador
                    //+ ", " + Cat_Cat_Descrip_Const_Rustico.Campo_Estatus
                    //+ ", " + Cat_Cat_Descrip_Const_Rustico.Campo_Usuario_Creo
                    //+ ", " + Cat_Cat_Descrip_Const_Rustico.Campo_Fecha_Creo
                    + " FROM  " + Cat_Cat_Descrip_Const_Rustico.Tabla_Cat_Cat_Descrip_Const_Rustico
                    + " WHERE ";
                if (Descripcion_Construccion.P_Identificador != null && Descripcion_Construccion.P_Identificador.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Descrip_Const_Rustico.Campo_Identificador + " LIKE '%" + Descripcion_Construccion.P_Identificador + "%' AND ";
                }
                if (Descripcion_Construccion.P_Des_Constru_Rustico_Id != null && Descripcion_Construccion.P_Des_Constru_Rustico_Id.Trim() != "")
                {
                    Mi_SQL += Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id + " = '" + Descripcion_Construccion.P_Des_Constru_Rustico_Id + "' AND ";
                }
                //if (Descripcion_Construccion.P_Estatus != null && Descripcion_Construccion.P_Estatus.Trim() != "")
                //{
                //    Mi_SQL += Cat_Cat_Descrip_Const_Rustico.Campo_Estatus + " " + Descripcion_Construccion.P_Estatus;
                //}
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                // agregar filtro y orden a la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Consultar_Descrip_Const_Rustico: [" + Ex.Message + "]."; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
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
            catch (Exception Ex)
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
    }
}
