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
using Presidencia.Catalogo_Salarios_Minimos.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Salarios_Minimos_Datos
/// </summary>

namespace Presidencia.Catalogo_Salarios_Minimos.Datos
{
    public class Cls_Cat_Pre_Salarios_Minimos_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Salario_Minimo
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Salario Minimo.
        ///PARAMENTROS:     
        ///             1. Salario.         Instancia de la Clase de Negocio de Salarios Minimos 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Salario_Minimo(Cls_Cat_Pre_Salarios_Minimos_Negocio Salario)
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

                String Salario_ID = Obtener_ID_Consecutivo(Cat_Pre_Salarios_Minimos.Tabla_Cat_Pre_Salarios_Minimos, Cat_Pre_Salarios_Minimos.Campo_Salario_Id, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Salarios_Minimos.Tabla_Cat_Pre_Salarios_Minimos;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Salarios_Minimos.Campo_Salario_Id + ", " + Cat_Pre_Salarios_Minimos.Campo_Monto;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Salarios_Minimos.Campo_Anio + "";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Salarios_Minimos.Campo_Estatus + "";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Salarios_Minimos.Campo_Usuario_Creo + ", " + Cat_Pre_Salarios_Minimos.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Salario_ID + "', '" + Salario.P_Monto + "'";
                Mi_SQL = Mi_SQL + ", '" + Salario.P_Anio + "'";
                Mi_SQL = Mi_SQL + ", '" + Salario.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + " ,sysdate";
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
                    Mensaje = "Error al intentar dar de Alta un Registro del Salario Mínimo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Salarios_Anteriores
        ///DESCRIPCIÓN: Actualiza en la Base de Datos los Salarios anteriores
        ///PARAMENTROS:     
        ///             1. Salario.         Instancia de la Clase de Salarios Mínimos 
        ///                                 con los datos de los Registros 
        ///                                 que van a ser Actualizados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 18/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Salarios_Anteriores(Cls_Cat_Pre_Salarios_Minimos_Negocio Salario)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Salarios_Minimos.Tabla_Cat_Pre_Salarios_Minimos;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Salarios_Minimos.Campo_Estatus + " = 'BAJA'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Salarios_Minimos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Salarios_Minimos.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Salarios_Minimos.Campo_Anio + " < '" + Salario.P_Anio + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Salarios Minimos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Salario
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Salario
        ///PARAMENTROS:     
        ///             1. Salario.         Instancia de la Clase de Salarios Mínimos 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Salario(Cls_Cat_Pre_Salarios_Minimos_Negocio Salario)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Salarios_Minimos.Tabla_Cat_Pre_Salarios_Minimos;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Salarios_Minimos.Campo_Anio + " = '" + Salario.P_Anio + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Salarios_Minimos.Campo_Estatus + " = '" + Salario.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Salarios_Minimos.Campo_Monto + " = '" + Salario.P_Monto + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Salarios_Minimos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Salarios_Minimos.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Salarios_Minimos.Campo_Salario_Id + " = '" + Salario.P_Salario_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Salarios Minimos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Salarios
        ///DESCRIPCIÓN: Obtiene todos los Salarios Minimos que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:
        ///                 Instituciones   Contiene los campos necesarios para hacer un filtrado de 
        ///                                 información, si es que se
        ///                                 introdujeron datos de busqueda.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Salarios(Cls_Cat_Pre_Salarios_Minimos_Negocio Salario)
        {
            DataTable tabla = new DataTable();
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Salarios_Minimos.Campo_Salario_Id;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Salarios_Minimos.Campo_Anio + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Salarios_Minimos.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Salarios_Minimos.Campo_Monto + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Salarios_Minimos.Tabla_Cat_Pre_Salarios_Minimos;
                    if (Salario.P_Filtro.Length != 0) {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Salarios_Minimos.Campo_Anio + " like '%" + Salario.P_Filtro + "%'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Salarios_Minimos.Campo_Anio +" DESC";
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Salarios
        ///DESCRIPCIÓN: Obtiene a detalle un Salarios Minimo.
        ///PARAMENTROS:   
        ///             1. P_salario.   Salario minimo que se va ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Salarios_Minimos_Negocio Consultar_Datos_Salarios(Cls_Cat_Pre_Salarios_Minimos_Negocio P_Salario)
        {
            Cls_Cat_Pre_Salarios_Minimos_Negocio R_Salario = new Cls_Cat_Pre_Salarios_Minimos_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Salarios_Minimos.Campo_Salario_Id;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Salarios_Minimos.Campo_Anio + "";
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Salarios_Minimos.Campo_Estatus + "";
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Salarios_Minimos.Campo_Monto + "";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Salarios_Minimos.Tabla_Cat_Pre_Salarios_Minimos;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Salarios_Minimos.Campo_Salario_Id + " = '" + P_Salario.P_Salario_ID + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Salario.P_Salario_ID = P_Salario.P_Salario_ID;
                while (Data_Reader.Read())
                {
                    R_Salario.P_Salario_ID = Data_Reader[Cat_Pre_Salarios_Minimos.Campo_Salario_Id].ToString();
                    R_Salario.P_Anio = Data_Reader[Cat_Pre_Salarios_Minimos.Campo_Anio].ToString();
                    R_Salario.P_Estatus = Data_Reader[Cat_Pre_Salarios_Minimos.Campo_Estatus].ToString();
                    R_Salario.P_Monto = Data_Reader[Cat_Pre_Salarios_Minimos.Campo_Monto].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Instituciones. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Salario;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Salario
        ///DESCRIPCIÓN: Elimina unSalario Minimo de la Base de Datos, modifica su estatus a 'INACTIVO'.
        ///PARAMENTROS:   
        ///             1. Institucion.   Registro que se va a eliminar de la Base de Datos.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 15/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Salario(Cls_Cat_Pre_Salarios_Minimos_Negocio Salario)
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
                String Mi_SQL = "DELETE " + Cat_Pre_Salarios_Minimos.Tabla_Cat_Pre_Salarios_Minimos;
                //Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Salarios_Minimos.Campo_Estatus + " = 'BAJA'";
                //Mi_SQL = Mi_SQL + "," + Cat_Pre_Salarios_Minimos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                //Mi_SQL = Mi_SQL + "," + Cat_Pre_Salarios_Minimos.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Salarios_Minimos.Campo_Salario_Id + " = '" + Salario.P_Salario_ID + "'";
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
                    Mensaje = "Error al intentar eliminar el registro de Salarios Minimos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Salarios Minimos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Anio
        ///DESCRIPCIÓN: Identifica si ya habia un salario minimo para ese año
        ///PARAMENTROS:   
        ///             1.  Caja.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es la Clave.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static bool Consultar_Anio(String Anio)
        {
            bool Fecha = false;
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Salarios_Minimos.Campo_Anio;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Salarios_Minimos.Tabla_Cat_Pre_Salarios_Minimos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Salarios_Minimos.Campo_Anio + " = '" + Anio + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset.Tables[0].Rows[0].ItemArray.Length > 0)
                {
                    Fecha = true;
                }
                if (dataset != null)
                {
                    Fecha = true;
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Días Festivos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Fecha;
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consultar_Salario_Anio
        /// 	DESCRIPCIÓN: Consulta el salario minimo de un anio dado
        /// 	PARÁMETROS:
        /// 		1. Anio: Año del que se requiere el salario minimo
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 23-jul-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Decimal Consultar_Salario_Anio(String Anio)
        {
            String Mi_SQL; //Variable para la consulta SQL
            Decimal Salario;
            Object SalarioDB;

            try
            {
                Mi_SQL = "SELECT " + Cat_Pre_Salarios_Minimos.Campo_Monto;
                Mi_SQL += " FROM " + Cat_Pre_Salarios_Minimos.Tabla_Cat_Pre_Salarios_Minimos;
                Mi_SQL += " WHERE " + Cat_Pre_Salarios_Minimos.Campo_Anio + " = " + Anio;

                SalarioDB = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(SalarioDB) || SalarioDB == null)    // si no se obtuvo resultado de la consulta
                {
                    return (Decimal)0;              // regresar 0
                }
                else
                {
                    if (decimal.TryParse(SalarioDB.ToString(), out Salario))    //si se obtiene un valor decimal de la consulta, regresar ese valor
                        return Salario;
                    else
                        return (Decimal)0;           // si no, regresar 0
                }
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
        }   //FUNCIÓN: Consultar_Salario_Anio


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