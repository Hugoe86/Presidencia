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
using Presidencia.Catalogo_Estados.Negocio;

namespace Presidencia.Catalogo_Estados.Datos
{

    public class Cls_Cat_Pre_Estados_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Estados
        ///DESCRIPCIÓN: Obtiene todas los Estados que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Estados()
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Estados.Campo_Estado_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Estados.Campo_Clave;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Estados.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Estados.Campo_Estatus;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Estados.Campo_Estado_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Estados. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Estado
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Estado
        ///PARAMETROS:     
        ///             1. Estado.  Instancia de la Clase de Negocio de Estados con los datos 
        ///                          del Estado que va a ser dada de Alta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Estado(Cls_Cat_Pre_Estados_Negocio Estado)
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
                String Estado_ID = Obtener_ID_Consecutivo(Cat_Pre_Estados.Tabla_Cat_Pre_Estados, Cat_Pre_Estados.Campo_Estado_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Estados.Campo_Estado_ID + ", " + Cat_Pre_Estados.Campo_Clave;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Estados.Campo_Nombre + ", " + Cat_Pre_Estados.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Estados.Campo_Usuario_Creo + ", " + Cat_Pre_Estados.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Estado_ID + "' ";
                Mi_SQL = Mi_SQL + ",'" + Estado.P_Clave + "'";
                Mi_SQL = Mi_SQL + ",'" + Estado.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ",'" + Estado.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Estado.P_Usuario + "'";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Estados. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Estado
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Estado
        ///PARAMETROS:     
        ///             1. Estado. Instancia de la Clase de Estados  con 
        ///                       los datos del Estado que va a ser Actualizada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Estado(Cls_Cat_Pre_Estados_Negocio Estado)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados+ " SET ";
                Mi_SQL = Mi_SQL + Cat_Pre_Estados.Campo_Nombre + " = '" + Estado.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Estados.Campo_Estatus + " = '" + Estado.P_Estatus + "' ";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Estados.Campo_Usuario_Modifico + " = '" + Estado.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Estados.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Estados.Campo_Estado_ID + " = '" + Estado.P_Estado_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Estados. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Nombre
        ///DESCRIPCIÓN: Obtiene el nombre de la Calle solicitada.
        ///PARAMETROS:   
        ///             1. Estado.   Nombre que se va ver a Detalle.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Nombre(Cls_Cat_Pre_Estados_Negocio Estado) //Busqueda
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Estados.Campo_Estado_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Estados.Campo_Clave;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Estados.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Estados.Campo_Estatus;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Estados.Campo_Nombre + " LIKE '%" + Estado.P_Nombre + "%' OR "+Cat_Pre_Estados.Campo_Clave+" LIKE '%"+Estado.P_Nombre+"%'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Estados.Campo_Nombre;                
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Estados. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Estado
        ///DESCRIPCIÓN: Elimina un Estado
        ///PARAMETROS:   
        ///             1. Estado.   Estado que se va eliminar.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Estado(Cls_Cat_Pre_Estados_Negocio Estado)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Estados.Campo_Estatus + " = 'BAJA'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Estados.Campo_Usuario_Modifico + " = '" + Estado.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Estados.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Estados.Campo_Estado_ID + " = '"+Estado.P_Estado_ID+"'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
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
                    Mensaje = "Error al intentar modificar un Registro de Estados. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Ultima_Clave
        ///DESCRIPCIÓN: Obtiene la ultima clave para el campo de Clave
        ///PARAMETROS:     
        ///             1. Estado.  Instancia de la Clase de Negocio de Estados con los datos.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Ultima_Clave(Cls_Cat_Pre_Estados_Negocio Estado)
        {
            return Obtener_ID_Consecutivo(Cat_Pre_Estados.Tabla_Cat_Pre_Estados, Cat_Pre_Estados.Campo_Clave, 10);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///             1. Tabla: Tabla de referencia a la que se sacara el ultimo ID
        ///             2. Campo: Compo al que se le asignara la ultima ID
        ///             3. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 20/Julio/2011 
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
        ///FECHA_CREO: 20/Julio/2011 
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