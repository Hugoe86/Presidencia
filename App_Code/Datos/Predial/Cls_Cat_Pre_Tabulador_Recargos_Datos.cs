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
using Presidencia.Catalogo_Tabulador_Recargos.Negocio;

namespace Presidencia.Catalogo_Tabulador_Recargos.Datos
{

    public class Cls_Cat_Pre_Tabulador_Recargos_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Recargos
        ///DESCRIPCIÓN: Obtiene todos los Recargos que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 22/Julio/2011 
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 01-mar-2012
        ///CAUSA_MODIFICACIÓN: Se agregan filtros opcionales para año y bimestre
        ///*******************************************************************************
        public static DataTable Consultar_Recargos(Cls_Cat_Pre_Tabulador_Recargos_Negocio Anio)
        {
         
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Recargos.Campo_Recargo_ID;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Anio + ", '0') " + Cat_Pre_Recargos.Campo_Anio;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Bimestre + ", '0') " + Cat_Pre_Recargos.Campo_Bimestre;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Enero + ", '0') " + Cat_Pre_Recargos.Campo_Enero;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Febrero + ", '0') " + Cat_Pre_Recargos.Campo_Febrero;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Marzo + ", '0') " + Cat_Pre_Recargos.Campo_Marzo;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Abril + ", '0') " + Cat_Pre_Recargos.Campo_Abril;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Mayo + ", '0') " + Cat_Pre_Recargos.Campo_Mayo;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Junio + ", '0') " + Cat_Pre_Recargos.Campo_Junio;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Julio + ", '0') " + Cat_Pre_Recargos.Campo_Julio;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Agosto + ", '0') " + Cat_Pre_Recargos.Campo_Agosto;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Septiembre + ", '0') " + Cat_Pre_Recargos.Campo_Septiembre;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Octubre + ", '0') " + Cat_Pre_Recargos.Campo_Octubre;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Noviembre + ", '0') " + Cat_Pre_Recargos.Campo_Noviembre;
                Mi_SQL += ", NVL(" + Cat_Pre_Recargos.Campo_Diciembre + ", '0') " + Cat_Pre_Recargos.Campo_Diciembre;
                Mi_SQL += " FROM " + Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos;
                Mi_SQL += " WHERE " + Cat_Pre_Recargos.Campo_Anio_Tabulador + " LIKE '%";
                Mi_SQL += Anio.P_Anio_Tabulador + "%'";

                // filtros opcionales
                if (!string.IsNullOrEmpty(Anio.P_Anio))
                {
                    Mi_SQL += " AND " + Cat_Pre_Recargos.Campo_Anio + " = '";
                    Mi_SQL += Anio.P_Anio + "'";
                }
                if (!string.IsNullOrEmpty(Anio.P_Bimestre))
                {
                    Mi_SQL += " AND " + Cat_Pre_Recargos.Campo_Bimestre + " = '";
                    Mi_SQL += Anio.P_Bimestre + "'";
                }

                Mi_SQL += " ORDER BY " + Cat_Pre_Recargos.Campo_Anio + " ASC, ";
                Mi_SQL += Cat_Pre_Recargos.Campo_Bimestre + " ASC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Recargos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Recargo
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Recargo
        ///PARAMETROS:     
        ///             1. Recargo. Instancia de la Clase de Negocio de Impuestos con los datos 
        ///                          del Impuesto que va a ser dado de Alta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 22/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Recargo(Cls_Cat_Pre_Tabulador_Recargos_Negocio Recargo)
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
                String Recargo_ID = Obtener_ID_Consecutivo(Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos, Cat_Pre_Recargos.Campo_Recargo_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Recargos.Campo_Recargo_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Anio_Tabulador;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Anio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Enero;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Febrero;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Marzo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Abril;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Mayo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Junio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Julio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Agosto;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Septiembre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Octubre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Noviembre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Diciembre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Bimestre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ") VALUES ('" + Recargo_ID + "' ";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Anio_Tabulador + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Anio + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Enero + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Febrero + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Marzo + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Abril + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Mayo + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Junio + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Julio + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Agosto + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Septiembre + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Octubre + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Noviembre + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Diciembre + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Bimestre + "'";
                Mi_SQL = Mi_SQL + ",'" + Recargo.P_Usuario + "'";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Recargos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Recargo
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Recargo
        ///PARAMETROS:     
        ///             1. Recargo. Instancia de la Clase de Recargos con 
        ///                          los datos del Recargo que va a ser Actualizado.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 22/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Recargo(Cls_Cat_Pre_Tabulador_Recargos_Negocio Recargo)
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
                for (int cont = 0; cont < Recargo.DT_Recargos.Rows.Count; cont++)
                {
                    String Mi_SQL = "UPDATE " + Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos;
                    Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Recargos.Campo_Anio + " = '" + Recargo.DT_Recargos.Rows[cont]["ANIO"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Bimestre + " = '" + Recargo.DT_Recargos.Rows[cont]["BIMESTRE"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Enero + " = '" + Recargo.DT_Recargos.Rows[cont]["ENERO"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Febrero + " = '" + Recargo.DT_Recargos.Rows[cont]["FEBRERO"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Marzo + " = '" + Recargo.DT_Recargos.Rows[cont]["MARZO"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Abril + " = '" + Recargo.DT_Recargos.Rows[cont]["ABRIL"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Mayo + " = '" + Recargo.DT_Recargos.Rows[cont]["MAYO"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Junio + " = '" + Recargo.DT_Recargos.Rows[cont]["JUNIO"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Julio + " = '" + Recargo.DT_Recargos.Rows[cont]["JULIO"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Agosto + " = '" + Recargo.DT_Recargos.Rows[cont]["AGOSTO"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Septiembre + " = '" + Recargo.DT_Recargos.Rows[cont]["SEPTIEMBRE"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Octubre + " = '" + Recargo.DT_Recargos.Rows[cont]["OCTUBRE"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Noviembre + " = '" + Recargo.DT_Recargos.Rows[cont]["NOVIEMBRE"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Diciembre + " = '" + Recargo.DT_Recargos.Rows[cont]["DICIEMBRE"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Usuario_Modifico + " = '" + Recargo.P_Usuario + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Recargos.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos.Campo_Recargo_ID+ " = '" + Recargo.DT_Recargos.Rows[cont]["RECARGO_ID"].ToString().Trim() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
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
                    Mensaje = "Error al intentar Agregar un Registro de Tabulador de Recagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Recargo
        ///DESCRIPCIÓN: Eliminar un recargo dado su id
        ///PARAMETROS:     
        ///             1. Recargo. Instancia de la Clase de negocio con 
        ///                          el ID del Recargo que va a ser eliminado.
        ///CREO: Roberto González Oseguera
        ///FECHA_CREO: 01-mar-2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Eliminar_Recargo(Cls_Cat_Pre_Tabulador_Recargos_Negocio Recargo)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            int Registros_Afectados = 0;

            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                if (!string.IsNullOrEmpty(Recargo.P_Recargo_ID))
                {
                    String Mi_SQL = "DELETE FROM "
                        + Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos
                        + " WHERE " + Cat_Pre_Recargos.Campo_Recargo_ID + " = '" + Recargo.P_Recargo_ID + "'";

                    Cmd.CommandText = Mi_SQL;
                    Registros_Afectados = Cmd.ExecuteNonQuery();

                    Trans.Commit();
                }
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
                    Mensaje = "Error al intentar Agregar un Registro de Tabulador de Recagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }

            return Registros_Afectados;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Anio
        ///DESCRIPCIÓN: Obtiene los datos del Recargo solicitado.
        ///PARAMETROS:   
        ///             1. Recargo.   Recargo que se va ver a Detalle.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 22/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Anio(Cls_Cat_Pre_Tabulador_Recargos_Negocio Recargo) //Busqueda
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Recargos.Campo_Recargo_ID;
                //Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Anio_Tabulador + ", '0') AS ANIO_TABULADOR";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Anio + ", '0') AS ANIO";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Bimestre + ", '0') AS BIMESTRE";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Enero + ", '0') AS ENERO";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Febrero + ", '0') AS FEBRERO";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Marzo + ", '0') AS MARZO";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Abril + ", '0') AS ABRIL";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Mayo + ", '0') AS MAYO";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Junio + ", '0') AS JUNIO";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Julio + ", '0') AS JULIO";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Agosto + ", '0') AS AGOSTO";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Septiembre + ", '0') AS SEPTIEMBRE";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Octubre + ", '0') AS OCTUBRE";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Noviembre + ", '0') AS NOVIEMBRE";
                Mi_SQL = Mi_SQL + ", NVL(" + Cat_Pre_Recargos.Campo_Diciembre + ", '0') AS DICIEMBRE";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Recargos.Campo_Anio_Tabulador + " = '" + Recargo.P_Anio + "' ";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Recargos.Campo_Anio + " ASC, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Recargos.Campo_Bimestre + " ASC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Tabulador de Recrgos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Anios
        ///DESCRIPCIÓN: Obtiene los años de los Recargos.
        ///PARAMETROS:   
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 22/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Anios() //Llenar Combo de Años Recargo
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT "
                    + " DISTINCT(" + Cat_Pre_Recargos.Campo_Anio_Tabulador + ")"
                    + " FROM " + Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos
                    + " WHERE " + Cat_Pre_Recargos.Campo_Anio_Tabulador + " IS NOT NULL"
                    + " ORDER BY " + Cat_Pre_Recargos.Campo_Anio_Tabulador + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros del Tabulador de Recargos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///             1. Tabla: Tabla de referencia a la que se sacara el ultimo ID
        ///             2. Campo: Compo al que se le asignara la ultima ID
        ///             3. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
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
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
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
        /// NOMBRE DE LA FUNCIÓN: Consultar_Tabulador_Recargos
        /// DESCRIPCIÓN: Consulta el tabulador de recargos de un determinado anio
        /// PARÁMETROS:     
        ///             1. Mes: mes que se va a consultar
        ///             2. Anio_Tabulador: Tabulador a consultar
        /// CREO: Roberto
        /// FECHA_CREO: 14-sep-2011
        /// MODIFICO             : 
        /// FECHA_MODIFICO       : 
        /// CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static DataTable Consultar_Tabulador_Recargos(String Mes, Int32 Anio_Tabulador)
        {
            String Mi_SQL; //Variable para la consulta SQL

            //Formar consulta
            Mi_SQL = "SELECT ";
            Mi_SQL += Cat_Pre_Recargos.Campo_Anio + ", ";
            Mi_SQL += Cat_Pre_Recargos.Campo_Bimestre + ", ";
            Mi_SQL += Mes + " AS TASA";
            Mi_SQL += " FROM ";
            Mi_SQL += Cat_Pre_Recargos.Tabla_Cat_Pre_Recargos;
            if (Anio_Tabulador > 0)
            {
                Mi_SQL += " WHERE " + Cat_Pre_Recargos.Campo_Anio_Tabulador + " = " + Anio_Tabulador;
            }
            Mi_SQL += " ORDER BY ";
            Mi_SQL += Cat_Pre_Recargos.Campo_Anio + ", " + Cat_Pre_Recargos.Campo_Bimestre;

            try
            {
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

    }
}