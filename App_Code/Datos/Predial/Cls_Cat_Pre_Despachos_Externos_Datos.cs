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
using Presidencia.Catalogo_Despachos_Externos.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Despachos_Externos_Datos
/// </summary>

namespace Presidencia.Catalogo_Despachos_Externos.Datos
{
    public class Cls_Cat_Pre_Despachos_Externos_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Instituciones_Recepcion_Pago_Predial
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Institucion.
        ///PARAMENTROS:     
        ///             1. Despacho_Externo.        Instancia de la Clase de Negocio de descpachos Externos 
        ///                                         con los datos del que van a ser
        ///                                         dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 16/Julio/2011 
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Despachos_Externos(Cls_Cat_Pre_Despachos_Externos_Negocio Despacho_Externo)
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
                String Despacho_ID = Obtener_ID_Consecutivo(Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos, Cat_Pre_Despachos_Externos.Campo_Despacho_Id, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Despachos_Externos.Campo_Calle + ", " + Cat_Pre_Despachos_Externos.Campo_Colonia;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Despachos_Externos.Campo_Contacto + ", " + Cat_Pre_Despachos_Externos.Campo_Correo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Despachos_Externos.Campo_Despacho + ", " + Cat_Pre_Despachos_Externos.Campo_Despacho_Id;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Despachos_Externos.Campo_Estatus + ", " + Cat_Pre_Despachos_Externos.Campo_Fax;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Despachos_Externos.Campo_No_Exterior + ", " + Cat_Pre_Despachos_Externos.Campo_No_Interior;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Despachos_Externos.Campo_Telefono;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Despachos_Externos.Campo_Usuario_Creo + ", " + Cat_Pre_Despachos_Externos.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Despacho_Externo.P_Calle + "'";
                Mi_SQL = Mi_SQL + ", '" + Despacho_Externo.P_Colonia + "'";
                Mi_SQL = Mi_SQL + ", '" + Despacho_Externo.P_Contacto + "'";
                Mi_SQL = Mi_SQL + ", '" + Despacho_Externo.P_Correo_Electronico + "'";
                Mi_SQL = Mi_SQL + ", '" + Despacho_Externo.P_Despacho + "'";
                Mi_SQL = Mi_SQL + ", '" + Despacho_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Despacho_Externo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", '" + Despacho_Externo.P_Fax + "'";
                Mi_SQL = Mi_SQL + ", '" + Despacho_Externo.P_No_Exterior + "'";
                Mi_SQL = Mi_SQL + ", '" + Despacho_Externo.P_No_Interior + "'";
                Mi_SQL = Mi_SQL + ", '" + Despacho_Externo.P_Telefono + "'";
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
                    Mensaje = "Error al intentar dar de Alta un Registro del Despacho Externo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Despachos_Externos
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un despacho Externo
        ///PARAMENTROS:     
        ///             1. Despacho_Externo.    Instancia de la Clase de Despachos Externos 
        ///                                     con los datos del Registro 
        ///                                     que va a ser Actualizado.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 18/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Despachos_Externos(Cls_Cat_Pre_Despachos_Externos_Negocio Despacho_Externo)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Despachos_Externos.Campo_Calle + " = '" + Despacho_Externo.P_Calle + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Colonia + " = '" + Despacho_Externo.P_Colonia + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Contacto + " = '" + Despacho_Externo.P_Contacto + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Correo + " = '" + Despacho_Externo.P_Correo_Electronico + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Despacho + " = '" + Despacho_Externo.P_Despacho + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Estatus + " = '" + Despacho_Externo.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Fax + " = '" + Despacho_Externo.P_Fax + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_No_Exterior + " = '" + Despacho_Externo.P_No_Exterior + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_No_Interior + " = '" + Despacho_Externo.P_No_Interior + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Telefono + " = '" + Despacho_Externo.P_Telefono + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Despachos_Externos.Campo_Despacho_Id + " = '" + Despacho_Externo.P_Despacho_Id + "'";
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
                    Mensaje = "Error al intentar modificar un Registro del Despacho externo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Despachos_Externos
        ///DESCRIPCIÓN: Obtiene todos los despachos externos que estan dados de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:
        ///                 Despacho_Interno    Contiene los campos necesarios para hacer un filtrado de 
        ///                                     información, si es que se
        ///                                     introdujeron datos de busqueda.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 18/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Despachos_Externos(Cls_Cat_Pre_Despachos_Externos_Negocio Despacho_Externo)
        {
            DataTable tabla = new DataTable();
                try
                {
                    String Mi_SQL = "SELECT d." + Cat_Pre_Despachos_Externos.Campo_Despacho_Id;
                    Mi_SQL = Mi_SQL + ", d." + Cat_Pre_Despachos_Externos.Campo_Despacho + "";
                    Mi_SQL = Mi_SQL + ", d." + Cat_Pre_Despachos_Externos.Campo_Contacto + "";
                    Mi_SQL = Mi_SQL + ", d." + Cat_Pre_Despachos_Externos.Campo_Calle + "";
                    Mi_SQL = Mi_SQL + ", d." + Cat_Pre_Despachos_Externos.Campo_Colonia + "";
                    Mi_SQL = Mi_SQL + ", CONCAT( (SELECT ca." + Cat_Pre_Calles.Campo_Nombre + " FROM "+Cat_Pre_Calles.Tabla_Cat_Pre_Calles+" ca WHERE ca."+Cat_Pre_Calles.Campo_Calle_ID+"=d."+Cat_Pre_Despachos_Externos.Campo_Calle +"), CONCAT(', ', CONCAT(" + Cat_Pre_Despachos_Externos.Campo_No_Exterior + ", CONCAT(', ', CONCAT(" + Cat_Pre_Despachos_Externos.Campo_No_Interior + ", CONCAT(', ', (SELECT co." + Cat_Ate_Colonias.Campo_Nombre + " FROM "+Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias+" co WHERE co."+Cat_Ate_Colonias.Campo_Colonia_ID+"=d."+Cat_Pre_Despachos_Externos.Campo_Colonia+"))))))) AS DOMICILIO";
                    Mi_SQL = Mi_SQL + ", d." + Cat_Pre_Despachos_Externos.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos + " d";
                    if (Despacho_Externo.P_Filtro.Trim().Length!= 0)
                    {
                        Mi_SQL = Mi_SQL + " WHERE d." + Cat_Pre_Despachos_Externos.Campo_Despacho + " like '%" + Despacho_Externo.P_Filtro + "%'";
                        Mi_SQL = Mi_SQL + " OR d." + Cat_Pre_Despachos_Externos.Campo_Contacto + " like '%" + Despacho_Externo.P_Filtro + "%'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY d." + Cat_Pre_Despachos_Externos.Campo_Despacho_Id;
                    DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (dataset != null)
                    {
                        tabla = dataset.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros de Despachos Externos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Despacho_Externo
        ///DESCRIPCIÓN: Obtiene a detalle un Despacho Externo.
        ///PARAMENTROS:   
        ///             1. P_Despacho_Externo   Despacho Externo que se va a ver a Detalle.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 18/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Despachos_Externos_Negocio Consultar_Datos_Despacho_Externo(Cls_Cat_Pre_Despachos_Externos_Negocio P_Despacho_Externo)
        {
            Cls_Cat_Pre_Despachos_Externos_Negocio R_Despacho_Externo = new Cls_Cat_Pre_Despachos_Externos_Negocio();
            String Mi_SQL = "SELECT " +  Cat_Pre_Despachos_Externos.Campo_Despacho_Id;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Calle;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Colonia;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Contacto;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Correo;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Despacho;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Estatus;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Fax;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_No_Exterior;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_No_Interior;
            Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Telefono;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos;
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Despacho_Externo.P_Despacho_Id = P_Despacho_Externo.P_Despacho_Id;
                while (Data_Reader.Read())
                {
                    R_Despacho_Externo.P_Despacho_Id = Data_Reader[Cat_Pre_Despachos_Externos.Campo_Despacho_Id].ToString();
                    R_Despacho_Externo.P_Calle = Data_Reader[Cat_Pre_Despachos_Externos.Campo_Calle].ToString();
                    R_Despacho_Externo.P_Colonia = Data_Reader[Cat_Pre_Despachos_Externos.Campo_Colonia].ToString();
                    R_Despacho_Externo.P_Contacto = Data_Reader[Cat_Pre_Despachos_Externos.Campo_Contacto].ToString();
                    R_Despacho_Externo.P_Correo_Electronico = Data_Reader[Cat_Pre_Despachos_Externos.Campo_Correo].ToString();
                    R_Despacho_Externo.P_Despacho = Data_Reader[Cat_Pre_Despachos_Externos.Campo_Despacho].ToString();
                    R_Despacho_Externo.P_Estatus = Data_Reader[Cat_Pre_Despachos_Externos.Campo_Estatus].ToString();
                    R_Despacho_Externo.P_Fax = Data_Reader[Cat_Pre_Despachos_Externos.Campo_Fax].ToString();
                    R_Despacho_Externo.P_No_Exterior = Data_Reader[Cat_Pre_Despachos_Externos.Campo_No_Exterior].ToString();
                    R_Despacho_Externo.P_No_Interior = Data_Reader[Cat_Pre_Despachos_Externos.Campo_No_Interior].ToString();
                    R_Despacho_Externo.P_Telefono = Data_Reader[Cat_Pre_Despachos_Externos.Campo_Telefono].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Despachos Externos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Despacho_Externo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Despacho_Externo
        ///DESCRIPCIÓN: Elimina un Despacho Externo de la Base de Datos, modifica su estatus a 'INACTIVO'.
        ///PARAMENTROS:   
        ///             1. Despacho_Externo.   Registro que se va a eliminar de la Base de Datos.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 18/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Despacho_Externo(Cls_Cat_Pre_Despachos_Externos_Negocio Despacho)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos;
                Mi_SQL = Mi_SQL + " SET " + Cat_Pre_Despachos_Externos.Campo_Estatus + " = 'BAJA'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Despachos_Externos.Campo_Fecha_Modifico + " = sysdate";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Despachos_Externos.Campo_Despacho_Id + " = '" + Despacho.P_Despacho_Id + "'";
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
                Mensaje = "Error al intentar eliminar el registro de Despachos Externos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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