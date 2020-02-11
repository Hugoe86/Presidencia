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
using Presidencia.Sessiones;
using Presidencia.Catalogo_Instituciones_Recepcion_Pago_Predial.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Instituciones_Recepcion_Pago_Predial_Datos
/// </summary>

namespace Presidencia.Catalogo_Instituciones_Recepcion_Pago_Predial.Datos
{
    public class Cls_Cat_Pre_Instituciones_Recepcion_Pago_Predial_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Instituciones_Recepcion_Pago_Predial
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Institucion.
        ///PARAMENTROS:     
        ///             1. Institucion.             Instancia de la Clase de Negocio de Instituciones recepcion pago predial 
        ///                                         con los datos del que van a ser
        ///                                         dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Instituciones(Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Institucion)
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

                String Institucion_ID = Obtener_ID_Consecutivo(Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos, Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Caja;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Enero;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Febrero;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Convenio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Usuario_Creo + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Institucion_ID + "', '" + Institucion.P_Institucion + "', '" + Institucion.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", '" + Institucion.P_Caja_Id + "', '" + Institucion.P_Linea_Captura_Enero + "', '" + Institucion.P_Linea_Captura_Febrero
                     + "', '" + Institucion.P_Convenio + "'";
                Mi_SQL = Mi_SQL + ", '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + " , sysdate";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de la institución. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Institucion
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Institucion
        ///PARAMENTROS:     
        ///             1. Institucion.            Instancia de la Clase de Instituciones de recepcion de pagos predial 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Institucion(Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Institucion)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion + " = '" + Institucion.P_Institucion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Estatus + " = '" + Institucion.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Enero + " = '" + Institucion.P_Linea_Captura_Enero + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Febrero + " = '" + Institucion.P_Linea_Captura_Febrero + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Convenio + " = '" + Institucion.P_Convenio + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Caja + " = '" + Institucion.P_Caja_Id + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id + " = '" + Institucion.P_Institucion_Id + "'";
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
                        Mensaje = "Error general en la base de datos";
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
                    Mensaje = "Error al intentar modificar un Registro de Instituciones. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Instituciones
        ///DESCRIPCIÓN: Obtiene todos las Instituciones que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:
        ///                 Instituciones   Contiene los campos necesarios para hacer un filtrado de 
        ///                                 información, si es que se
        ///                                 introdujeron datos de busqueda.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Instituciones(Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Instituciones)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT I." + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id;
                Mi_SQL = Mi_SQL + ", I." + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion + "";
                Mi_SQL = Mi_SQL + ", I." + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Estatus + "";
                Mi_SQL = Mi_SQL + ", I." + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Caja + "";
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS " + Cat_Pre_Cajas.Campo_Clave;
                Mi_SQL = Mi_SQL + ", I." + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Enero + "";
                Mi_SQL = Mi_SQL + ", I." + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Febrero + "";
                Mi_SQL = Mi_SQL + ", I." + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Convenio + "";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos;
                Mi_SQL = Mi_SQL + " I LEFT JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " C ON C." + Cat_Pre_Cajas.Campo_Caja_ID + " = I." + Cat_Pre_Cajas.Campo_Caja_ID;
                if (Instituciones.P_Filtro.Trim().Length != 0)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion + " like '%" + Instituciones.P_Filtro + "%'";
                }
                else if (!String.IsNullOrEmpty(Instituciones.P_Estatus))
                {
                    Mi_SQL += " WHERE I." + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Estatus
                        + " = '" + Instituciones.P_Estatus + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Instituciones. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Instituciones_Reciben_Pago
        ///DESCRIPCIÓN          : Obtiene las Instituciones que estan dadas de en el catálogo sin ligar información
        ///PARAMENTROS          :
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Enero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Instituciones_Reciben_Pago(Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Instituciones)
        {
            //DataTable tabla = new DataTable();
            //try
            //{
            //    String Mi_SQL = "SELECT " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id;
            //    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion;
            //    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Estatus;
            //    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Caja;
            //    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Enero;
            //    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Febrero;
            //    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Convenio;
            //    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos;
            //    if (!String.IsNullOrEmpty(Instituciones.P_Filtro))
            //    {
            //        Mi_SQL = Mi_SQL + " WHERE " + Instituciones.P_Filtro;
            //    }
            //    else if (!String.IsNullOrEmpty(Instituciones.P_Estatus))
            //    {
            //        Mi_SQL += " WHERE " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Estatus + " = '" + Instituciones.P_Estatus + "'";
            //    }
            //    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion;
            //    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //    if (dataset != null)
            //    {
            //        tabla = dataset.Tables[0];
            //    }
            //}
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            try
            {
                if (Instituciones.P_Campos != null && Instituciones.P_Campos != "")
                {
                    Mi_SQL = "SELECT " + Instituciones.P_Campos;
                }
                else
                {
                    Mi_SQL = "SELECT ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Estatus;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Caja;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Enero;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Febrero;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Convenio;
                }
                Mi_SQL += " FROM " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos;
                if (Instituciones.P_Filtro != null && Instituciones.P_Filtro != "")
                {
                    Mi_SQL += " WHERE " + Instituciones.P_Filtro;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Instituciones.P_Estatus != null && Instituciones.P_Estatus != "")
                    {
                        Mi_SQL += Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos + "." + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Estatus + Validar_Operador_Comparacion(Instituciones.P_Estatus) + " AND ";
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
                if (Instituciones.P_Agrupar_Dinamico != null && Instituciones.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Instituciones.P_Agrupar_Dinamico;
                }
                if (Instituciones.P_Ordenar_Dinamico != null && Instituciones.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Instituciones.P_Ordenar_Dinamico;
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Instituciones. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Instituciones
        ///DESCRIPCIÓN: Obtiene a detalle una Institucion.
        ///PARAMENTROS:   
        ///             1. P_Institucion.   Institucion que se va ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Consultar_Datos_Instituciones(Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio P_Institucion)
        {
            Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio R_Institucion = new Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Caja;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Enero;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Febrero;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Convenio;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id + " = '" + P_Institucion.P_Institucion_Id + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Institucion.P_Institucion_Id = P_Institucion.P_Institucion_Id;
                while (Data_Reader.Read())
                {
                    R_Institucion.P_Institucion_Id = Data_Reader[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id].ToString();
                    R_Institucion.P_Institucion = Data_Reader[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion].ToString();
                    R_Institucion.P_Estatus = Data_Reader[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Estatus].ToString();
                    R_Institucion.P_Caja_Id = Data_Reader[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Caja].ToString();
                    R_Institucion.P_Linea_Captura_Enero = Data_Reader[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Enero].ToString();
                    R_Institucion.P_Linea_Captura_Febrero = Data_Reader[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Linea_Captura_Febrero].ToString();
                    R_Institucion.P_Convenio = Data_Reader[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Convenio].ToString();

                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Instituciones. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Institucion;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Institucion
        ///DESCRIPCIÓN: Elimina una Institucion de la Base de Datos, modifica su estatus a 'INACTIVO'.
        ///PARAMENTROS:   
        ///             1. Institucion.   Registro que se va a eliminar de la Base de Datos.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 15/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Institucion(Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Institucion)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Estatus + " = 'BAJA'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id + " = '" + Institucion.P_Institucion_Id + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]"; ;
                }
                else
                {
                    Mensaje = "Error al intentar eliminar el registro de Instituciones. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Instituciones. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajas_Nombre_Id
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///             alta en la Base de Datos. Solo regresa el nombre y el id para
        ///             llenar Combos.
        ///PARAMENTROS:
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 15/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cajas_Nombre_Id()
        {
            DataTable tabla = new DataTable();
            {
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Cajas.Campo_Caja_Id;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS " + Cat_Pre_Cajas.Campo_Clave;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Cajas.Campo_Clave;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_No_Repetir_Caja
        ///DESCRIPCIÓN: Obtiene si la caja ya se encuentra asignada a una institución.
        ///PARAMENTROS:
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 09/Enero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_No_Repetir_Caja(Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Institucion)
        {
            DataTable tabla = new DataTable();
            {
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Caja + "='" + Institucion.P_Caja_Id + "' ";
                    if (Institucion.P_Institucion_Id != null && Institucion.P_Institucion_Id != "")
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id + "!='" + Institucion.P_Institucion_Id + "' ";
                    }
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Cajas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            }
            return tabla;
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