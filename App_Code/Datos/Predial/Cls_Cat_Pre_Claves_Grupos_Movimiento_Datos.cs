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
using Presidencia.Constantes;
using Presidencia.Catalogo_Claves_Grupos_Movimiento.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Catalogo_Claves_Grupos_Movimiento.Datos
{

    public class Cls_Cat_Pre_Claves_Grupos_Movimiento_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Sector
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nuevo registro de Sector
        ///PARAMETROS:     
        ///             1. Sector.  Objeto con las propiedades necesarias para dar
        ///                         de alta el Sector.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Grupo_Movimiento(Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Grupo_Movimiento)
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
                String Grupo_Movimiento_ID = Obtener_ID_Consecutivo(Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento, Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento;
                Mi_SQL = Mi_SQL + "( " + Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + ", " + Cat_Pre_Grupos_Movimiento.Campo_Clave + ", " + Cat_Pre_Grupos_Movimiento.Campo_Nombre + ", " + Cat_Pre_Grupos_Movimiento.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos_Movimiento.Campo_Usuario_Creo + ", " + Cat_Pre_Grupos_Movimiento.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ") VALUES ( '" + Grupo_Movimiento_ID + "', '" + Grupo_Movimiento.P_Clave + "', '" + Grupo_Movimiento.P_Nombre + "', '" + Grupo_Movimiento.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", '" + Grupo_Movimiento.P_Usuario + "', SYSDATE )";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                if (Grupo_Movimiento.P_Dt_Detalles_Folios != null)
                {
                    foreach (DataRow Dr_Detalles_Folios in Grupo_Movimiento.P_Dt_Detalles_Folios.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Pre_Grupos_Movimiento_Detalles.Tabla_Cat_Pre_Grupos_Movimiento_Detalles + " (";
                        Mi_SQL += Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID;
                        Mi_SQL += ", " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID;
                        Mi_SQL += ", " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año;
                        Mi_SQL += ", " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Folio_Inicial;
                        Mi_SQL += ") VALUES (";
                        Mi_SQL += "'" + Grupo_Movimiento_ID + "'";
                        Mi_SQL += ", '" + Dr_Detalles_Folios[Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID].ToString() + "'";
                        Mi_SQL += ", " + Dr_Detalles_Folios[Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año].ToString();
                        Mi_SQL += ", " + Dr_Detalles_Folios[Cat_Pre_Grupos_Movimiento_Detalles.Campo_Folio_Inicial].ToString();
                        Mi_SQL += ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

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
                    Mensaje = "Error al intentar dar de Alta un Registro del Detalle. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        /////*******************************************************************************
        /////NOMBRE DE LA FUNCIÓN: Obtener_Clave_Maxima
        /////DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        /////PARÁMETROS:   
        /////             1. Tabla: Tabla a la que hace referencia el campo.
        /////             2. Campo: Campo que se examinara para obtener el ultimo valor ingresado.
        /////             3. Id:    ID del campo que se quiere obtener la clave siguiente
        /////CREO: José Alfredo García Pichardo.
        /////FECHA_CREO: 15/Julio/2011 
        /////MODIFICO             : 
        /////FECHA_MODIFICO       : 
        /////CAUSA_MODIFICACIÓN   : 
        /////*******************************************************************************
        //public static String Obtener_Ultimo_Sector()
        //{
        //    String Id = "";
        //    try
        //    {
        //        String Mi_SQL = "SELECT MAX(" + Cat_Pre_Sectores.Campo_Sector_ID + ") FROM " + Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores;
        //        Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        //        if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
        //        {
        //            Id = Obj_Temp.ToString();
        //        }
        //    }
        //    catch (OracleException Ex)
        //    {
        //        new Exception(Ex.Message);
        //    }
        //    return Id;
        //}

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Sector
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Registro de Sector
        ///PARAMETROS:     
        ///             1. Sector.  Objeto con las propiedades necesarias para dar
        ///                         de actualizar el Sector.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Grupo_Movimiento(Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Grupo_Movimiento)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Grupos_Movimiento.Campo_Nombre + " = '" + Grupo_Movimiento.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos_Movimiento.Campo_Clave + " = '" + Grupo_Movimiento.P_Clave + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos_Movimiento.Campo_Estatus + " = '" + Grupo_Movimiento.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos_Movimiento.Campo_Usuario_Modifico + " = '" + Grupo_Movimiento.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos_Movimiento.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + " = '" + Grupo_Movimiento.P_Grupo_Movimiento_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();


                Mi_SQL = "DELETE FROM " + Cat_Pre_Grupos_Movimiento_Detalles.Tabla_Cat_Pre_Grupos_Movimiento_Detalles;
                Mi_SQL += " WHERE " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID + " = '" + Grupo_Movimiento.P_Grupo_Movimiento_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                if (Grupo_Movimiento.P_Dt_Detalles_Folios != null)
                {
                    foreach (DataRow Dr_Detalles_Folios in Grupo_Movimiento.P_Dt_Detalles_Folios.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Cat_Pre_Grupos_Movimiento_Detalles.Tabla_Cat_Pre_Grupos_Movimiento_Detalles + " (";
                        Mi_SQL += Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID;
                        Mi_SQL += ", " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID;
                        Mi_SQL += ", " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año;
                        Mi_SQL += ", " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Folio_Inicial;
                        Mi_SQL += ") VALUES (";
                        Mi_SQL += "'" + Dr_Detalles_Folios[Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID].ToString() + "'";
                        Mi_SQL += ", '" + Dr_Detalles_Folios[Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID].ToString() + "'";
                        Mi_SQL += ", " + Dr_Detalles_Folios[Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año].ToString();
                        Mi_SQL += ", " + Dr_Detalles_Folios[Cat_Pre_Grupos_Movimiento_Detalles.Campo_Folio_Inicial].ToString();
                        Mi_SQL += ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

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
                    Mensaje = "Error al intentar modificar un Registro de Claves de Grupos de Movimiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
        ///DESCRIPCIÓN: Obtiene todos Registros de un tipo de consulta y las devueve en 
        ///             un DataTable.
        ///PARAMETROS:   
        ///             1. Sector.  Objeto con las propiedades necesarias para 
        ///                         hacer la consulta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Grupo_Movimiento)
        {
            String Mi_SQL = "";
            DataTable Tabla = new DataTable();
            try
            {
                DataSet dataSet = null;
                if (Grupo_Movimiento.P_Tipo_DataTable.Equals("GRUPOS_MOVIMIENTO"))
                {
                    Mi_SQL = "SELECT " + Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + " AS GRUPO_MOVIMIENTO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos_Movimiento.Campo_Clave + " AS CLAVE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos_Movimiento.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos_Movimiento.Campo_Estatus + " AS ESTATUS";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Grupos_Movimiento.Campo_Clave + " LIKE '%" + Grupo_Movimiento.P_Nombre + "%'";
                    Mi_SQL = Mi_SQL + " OR " + Cat_Pre_Grupos_Movimiento.Campo_Nombre + " LIKE '%" + Grupo_Movimiento.P_Nombre + "%'";
                }
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Folios
        ///DESCRIPCIÓN          : Obtiene el Detalle de los Folios para una Clave indicada
        ///PARAMETROS           : Grupo_Movimiento.  Instancia de la clase actual
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 22/Marzo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Detalles_Folios(Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Grupo_Movimiento)
        {
            DataTable Dt_Detalles_Folios = new DataTable();
            try
            {
                DataSet Ds_Detalles_Folios = null;
                String Mi_SQL = "SELECT " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID;
                Mi_SQL += ", " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID;
                Mi_SQL += ", " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año;
                Mi_SQL += ", " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Folio_Inicial;
                Mi_SQL += ", (SELECT " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " WHERE " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + "." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = " + Cat_Pre_Grupos_Movimiento_Detalles.Tabla_Cat_Pre_Grupos_Movimiento_Detalles + "." + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID + ") AS TIPO_PREDIO";
                Mi_SQL += " FROM " + Cat_Pre_Grupos_Movimiento_Detalles.Tabla_Cat_Pre_Grupos_Movimiento_Detalles;
                Mi_SQL += " WHERE " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID + " = '" + Grupo_Movimiento.P_Grupo_Movimiento_ID + "'";
                Ds_Detalles_Folios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Detalles_Folios != null)
                {
                    if (Ds_Detalles_Folios.Tables.Count > 0)
                    {
                        Dt_Detalles_Folios = Ds_Detalles_Folios.Tables[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros del Detalle. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Detalles_Folios;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Grupos_Movimientos
        ///DESCRIPCIÓN          : Obtiene los Grupos de Movimientos
        ///PARAMETROS           : Cuenta, instancia de Cls_Cat_Pre_Cuentas_Predial_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 07/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Grupos_Movimientos(Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Grupo_Movimiento)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            try
            {
                if (Grupo_Movimiento.P_Campos_Dinamicos != null && Grupo_Movimiento.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Grupo_Movimiento.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT ";
                    Mi_SQL += Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + ", ";
                    Mi_SQL += Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Clave + ", ";
                    Mi_SQL += Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Nombre + ", ";
                    Mi_SQL += Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Estatus + ", ";
                    if (Mi_SQL.EndsWith(", "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 2);
                    }
                }
                Mi_SQL += " FROM " + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento;
                if (Grupo_Movimiento.P_Join != null && Grupo_Movimiento.P_Join != "")
                {
                    Mi_SQL += " " + Grupo_Movimiento.P_Join;
                }
                if (Grupo_Movimiento.P_Filtros_Dinamicos != null && Grupo_Movimiento.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Grupo_Movimiento.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Grupo_Movimiento.P_Grupo_Movimiento_ID != null && Grupo_Movimiento.P_Grupo_Movimiento_ID != "")
                    {
                        Mi_SQL += Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + " = '" + Grupo_Movimiento.P_Grupo_Movimiento_ID + "' AND ";
                    }
                    if (Grupo_Movimiento.P_Clave != null && Grupo_Movimiento.P_Clave != "")
                    {
                        Mi_SQL += Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Clave + " LIKE '%" + Grupo_Movimiento.P_Clave + "%' AND ";
                    }
                    if (Grupo_Movimiento.P_Estatus != null && Grupo_Movimiento.P_Estatus != "")
                    {
                        Mi_SQL += Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Estatus + Validar_Operador_Comparacion(Grupo_Movimiento.P_Estatus) + " AND ";
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
                if (Grupo_Movimiento.P_Agrupar_Dinamico != null && Grupo_Movimiento.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Grupo_Movimiento.P_Agrupar_Dinamico;
                }
                if (Grupo_Movimiento.P_Ordenar_Dinamico != null && Grupo_Movimiento.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Grupo_Movimiento.P_Ordenar_Dinamico;
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

        /////*******************************************************************************
        /////NOMBRE DE LA FUNCIÓN: Consultar_Clave_Colonia
        /////DESCRIPCIÓN: Obtiene todas las Colonias almacenadas en la base de datos.
        /////PARAMETROS:   
        /////CREO: José Alfredo García Pichardo.
        /////FECHA_CREO: 08/Agosto/2011 
        /////MODIFICO:
        /////FECHA_MODIFICO
        /////CAUSA_MODIFICACIÓN
        /////*******************************************************************************
        //public static DataSet Consultar_Clave_Colonia(Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Colonia)
        //{
        //    DataSet dataset;
        //    try
        //    {
        //        String Mi_SQL = "SELECT " + Cat_Ate_Colonias.Campo_Clave;
        //        Mi_SQL = Mi_SQL + " FROM  " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
        //        Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Colonia.P_Colonia_ID + "'";
        //        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Ate_Colonias.Campo_Colonia_ID;
        //        dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        //    }
        //    catch (Exception Ex)
        //    {
        //        String Mensaje = "Error al intentar consultar los registros de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
        //        throw new Exception(Mensaje);
        //    }
        //    return dataset;
        //}

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Sector
        ///DESCRIPCIÓN: Elimina un Registro de Sectores de la Base de Datos
        ///PARAMETROS:    
        ///             1. Sector.  Objeto con las propiedades necesarias para
        ///                         eliminar el Sector.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Grupo_Movimiento(Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Grupo_Movimiento)
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
                String Mi_SQL;

                Mi_SQL = "SELECT " + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                Mi_SQL += " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + " = '" + Grupo_Movimiento.P_Grupo_Movimiento_ID + "'";
                Mi_SQL += " AND ROWNUM = 1";
                Cmd.CommandText = Mi_SQL;

                if (Cmd.ExecuteReader().Read())
                {
                    Mi_SQL = "UPDATE " + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento;
                    Mi_SQL += " SET " + Cat_Pre_Grupos_Movimiento.Campo_Estatus + " = 'BAJA'";
                    Mi_SQL += " WHERE " + Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + " = '" + Grupo_Movimiento.P_Grupo_Movimiento_ID + "'";
                }
                else
                {
                    Mi_SQL = "DELETE FROM " + Cat_Pre_Grupos_Movimiento_Detalles.Tabla_Cat_Pre_Grupos_Movimiento_Detalles;
                    Mi_SQL += " WHERE " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID + " = '" + Grupo_Movimiento.P_Grupo_Movimiento_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    Mi_SQL = "DELETE FROM " + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento;
                    Mi_SQL += " WHERE " + Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + " = '" + Grupo_Movimiento.P_Grupo_Movimiento_ID + "'";
                }
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar eliminar el registro de Sector. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Trans.Rollback();
                Mensaje = "Error al intentar eliminar el registro de Claves de Grupos de Movimiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Maxima
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:   
        ///             1. Tabla: Tabla a la que hace referencia el campo.
        ///             2. Campo: Campo que se examinara para obtener el ultimo valor ingresado.
        ///             3. Id:    ID del campo que se quiere obtener la clave siguiente
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_Clave_Maxima()
        {
            String Id = "";
            try
            {
                String Mi_SQL = "SELECT MAX(" + Cat_Pre_Sectores.Campo_Clave + ") FROM " + Cat_Pre_Sectores.Tabla_Cat_Pre_Sectores;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = (Convert.ToInt32(Obj_Temp) + 1).ToString();
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
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
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
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