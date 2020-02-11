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
using Presidencia.Catalogo_Modulos.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Modulos_Datos
/// </summary>

namespace Presidencia.Catalogo_Modulos.Datos
{
    public class Cls_Cat_Pre_Modulos_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Modulos
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Modulo
        ///PARAMENTROS:     
        ///             1. Modulo.          Instancia de la Clase de Negocio de Modulos 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 28/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Modulos(Cls_Cat_Pre_Modulos_Negocio Modulo)
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

                String Modulo_ID = Obtener_ID_Consecutivo(Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo, Cat_Pre_Modulos.Campo_Modulo_Id, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Modulos.Campo_Modulo_Id + ", " + Cat_Pre_Modulos.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Clave + ", " + Cat_Pre_Modulos.Campo_Ubicacion;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Descripcion + ", " + Cat_Pre_Modulos.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Fecha_Creo + ", " + Cat_Pre_Modulos.Campo_Usuario_Modifico;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Fecha_Modifico + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Modulo_ID + "', '" + Modulo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Modulo.P_Clave + "'";
                Mi_SQL = Mi_SQL + ",'" + Modulo.P_Ubicacion + "'";
                Mi_SQL = Mi_SQL + ",'" + Modulo.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "', sysdate";
                Mi_SQL = Mi_SQL + ",''";
                Mi_SQL = Mi_SQL + ",''";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Módulo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Caja
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Módulo
        ///PARAMENTROS:     
        ///             1. Modulo.          Instancia de la Clase de Modulos 
        ///                                 con los datos del Registro 
        ///                                 que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 29/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Modulos(Cls_Cat_Pre_Modulos_Negocio Modulo)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Modulos.Campo_Clave + " = '" + Modulo.P_Clave + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Modulos.Campo_Descripcion + " = '" + Modulo.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Modulos.Campo_Estatus + " = '" + Modulo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Modulos.Campo_Ubicacion + " = '" + Modulo.P_Ubicacion + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Modulos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Modulos.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Modulos.Campo_Modulo_Id + " = '" + Modulo.P_Id_Modulo + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Módulos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Modulos
        ///DESCRIPCIÓN: Obtiene todos los Modulos que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 29/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Modulos(Cls_Cat_Pre_Modulos_Negocio modulo)
        {
             DataTable tabla = new DataTable();
            if (modulo.P_Filtro.Trim().Length == 0)
            {
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Modulos.Campo_Modulo_Id;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Clave + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Ubicacion + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Descripcion + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Modulos.Campo_Modulo_Id;
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
            }
            else 
            {
                try
                {
                    String Mi_SQL = "SELECT " + Cat_Pre_Modulos.Campo_Modulo_Id;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Clave + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Ubicacion + "";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Descripcion + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Modulos.Campo_Clave +" LIKE '%" + modulo.P_Filtro+ "%'";
                    Mi_SQL = Mi_SQL + " OR " + Cat_Pre_Modulos.Campo_Ubicacion + " LIKE '%" + modulo.P_Filtro + "%'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Modulos.Campo_Modulo_Id;
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
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre_Modulos
        ///DESCRIPCIÓN: Obtiene los módulos para asignarselos
        ///             a un combo.
        ///PARAMENTROS:
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Nombre_Modulos()
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Modulos.Campo_Modulo_Id + ", " + Cat_Pre_Modulos.Campo_Descripcion + "" + ", " + Cat_Pre_Modulos.Campo_Ubicacion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Modulos.Campo_Modulo_Id;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Modulo
        ///DESCRIPCIÓN: Obtiene a detalle una Caja.
        ///PARAMENTROS:   
        ///             1. P_Modulo.   Módulo que se va ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 29/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Modulos_Negocio Consultar_Datos_Modulos(Cls_Cat_Pre_Modulos_Negocio P_Modulo)
        {
            Cls_Cat_Pre_Modulos_Negocio R_Modulos = new Cls_Cat_Pre_Modulos_Negocio();
            String Mi_SQL = "SELECT " + Cat_Pre_Modulos.Campo_Modulo_Id + ", " + Cat_Pre_Modulos.Campo_Clave;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Ubicacion;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Modulos.Campo_Modulo_Id + " = '" + P_Modulo.P_Id_Modulo + "'";
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Modulos.P_Id_Modulo = P_Modulo.P_Id_Modulo;
                while (Data_Reader.Read())
                {
                    R_Modulos.P_Id_Modulo = Data_Reader[Cat_Pre_Modulos.Campo_Modulo_Id].ToString();
                    R_Modulos.P_Clave = Data_Reader[Cat_Pre_Modulos.Campo_Clave].ToString();
                    R_Modulos.P_Estatus = Data_Reader[Cat_Pre_Cajas.Campo_Estatus].ToString();
                    R_Modulos.P_Ubicacion = Data_Reader[Cat_Pre_Modulos.Campo_Ubicacion].ToString();
                    R_Modulos.P_Descripcion = Data_Reader[Cat_Pre_Modulos.Campo_Descripcion].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Módulos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Modulos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Modulo
        ///DESCRIPCIÓN: Elimina un Módulo de la Base de Datos.
        ///PARAMENTROS:   
        ///             1. Modulo.   Registro que se va a eliminar de la Base de Datos.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 29/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Modulo(Cls_Cat_Pre_Modulos_Negocio Modulo)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL += " SET " + Cat_Pre_Modulos.Campo_Estatus+"='BAJA'";
                Mi_SQL += ", " + Cat_Pre_Modulos.Campo_Usuario_Modifico+"='"+Cls_Sessiones.Nombre_Empleado.ToUpper()+"'";
                Mi_SQL += "," + Cat_Pre_Modulos.Campo_Fecha_Modifico + " = ";
                Mi_SQL += " SYSDATE";
                Mi_SQL += " WHERE " + Cat_Pre_Modulos.Campo_Modulo_Id + " = '" + Modulo.P_Id_Modulo + "'";
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
                    Mensaje = "Error al intentar eliminar el registro de Módulos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Módulos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre_Modulo
        ///DESCRIPCIÓN: Obtiene a detalle una Caja.
        ///PARAMENTROS:   
        ///             1. Modulo.   Módulo que se va buscar.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 01/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Modulos_Negocio Consultar_Nombre_Modulo(Cls_Cat_Pre_Modulos_Negocio Modulo)
        {
            String Mi_SQL = "SELECT m." + Cat_Pre_Modulos.Campo_Descripcion + " AS " + Cat_Pre_Modulos.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + " m";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + " c";
            Mi_SQL = Mi_SQL + " ON m." + Cat_Pre_Modulos.Campo_Modulo_Id + "=c." + Cat_Pre_Cajas.Campo_Modulo_Id;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cajas.Campo_Caja_Id + " = '" + Modulo.P_Filtro + "'";
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Modulos.Campo_Descripcion;
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read())
                {
                    Modulo.P_Filtro = Data_Reader[Cat_Pre_Modulos.Campo_Descripcion].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Módulos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Modulo;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre_Modulos
        ///DESCRIPCIÓN: Obtiene todos los modulos que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Modulo.     Variable para consultar la tabla de la base de datos.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 24/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Nombre_Modulos(Cls_Cat_Pre_Modulos_Negocio Caja)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Modulos.Campo_Modulo_Id;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Ubicacion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
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



    }
}