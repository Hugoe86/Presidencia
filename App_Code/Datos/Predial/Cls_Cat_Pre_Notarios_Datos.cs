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
using Presidencia.Catalogo_Notarios.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Notarios_Datos
/// </summary>
/// 

namespace Presidencia.Catalogo_Notarios.Datos{
    public class Cls_Cat_Pre_Notarios_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Notario
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro de Notario
        ///PARÁMETROS          : 1. Notario.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Notarios_Negocio
        ///                                 con los datos del Notario que va a ser dado de Alta.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Alta_Notario(Cls_Cat_Pre_Notarios_Negocio Notario)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Alta = false;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Notario_ID = Obtener_ID_Consecutivo(Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios, Cat_Pre_Notarios.Campo_Notario_ID, 5);
            try
            {
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + " (";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Notario_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Apellido_Paterno + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Apellido_Materno + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Numero_Notaria + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Sexo + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Estado + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Ciudad + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Colonia + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Calle + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Numero_Exterior + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Numero_Interior + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Codigo_Postal + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_RFC + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_CURP + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Telefono + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Fax + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Celular + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_E_Mail + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + " VALUES ('";
                Mi_SQL = Mi_SQL + Notario_ID + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Apellido_Paterno + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Apellido_Materno + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Nombre + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Numero_Notaria + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Sexo + "',' ";
                Mi_SQL = Mi_SQL + Notario.P_Estado + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Ciudad + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Colonia + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Calle + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Numero_Exterior + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Numero_Interior + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Codigo_Postal + "', '";
                Mi_SQL = Mi_SQL + Notario.P_RFC + "', '";
                Mi_SQL = Mi_SQL + Notario.P_CURP + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Estatus + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Telefono + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Fax + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Celular + "', '";
                Mi_SQL = Mi_SQL + Notario.P_E_Mail + "', '";
                Mi_SQL = Mi_SQL + Notario.P_Usuario + "', SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Alta = true;
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar dar de Alta un Registro de Notario. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Notario
        ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Notario
        ///PARÁMETROS          : 1. Notario.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Notarios_Negocio
        ///                                 con los datos del Notario que va a ser Modificado.
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Notario(Cls_Cat_Pre_Notarios_Negocio Notario)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Actualizar = false;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios + " SET ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Apellido_Paterno + " = '" + Notario.P_Apellido_Paterno + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Apellido_Materno + " = '" + Notario.P_Apellido_Materno + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Nombre + " = '" + Notario.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Numero_Notaria + " = '" + Notario.P_Numero_Notaria + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Sexo + " = '" + Notario.P_Sexo + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Estado + " = '" + Notario.P_Estado + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Ciudad + " = '" + Notario.P_Ciudad + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Colonia + " = '" + Notario.P_Colonia + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Calle + " = '" + Notario.P_Calle + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Numero_Exterior + " = '" + Notario.P_Numero_Exterior + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Numero_Interior + " = '" + Notario.P_Numero_Interior + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Codigo_Postal + " = '" + Notario.P_Codigo_Postal + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_RFC + " = '" + Notario.P_RFC + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_CURP + " = '" + Notario.P_CURP + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Estatus + " = '" + Notario.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Telefono + " = '" + Notario.P_Telefono + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Fax + " = '" + Notario.P_Fax + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Celular + " = '" + Notario.P_Celular + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_E_Mail + " = '" + Notario.P_E_Mail + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Usuario_Modifico + " = '" + Notario.P_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Fecha_Modifico + " = SYSDATE ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Pre_Notarios.Campo_Notario_ID + " = '" + Notario.P_Notario_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Actualizar = true;
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
                    Mensaje = "Error al intentar modificar un Registro de Notario. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Notarios
        ///DESCRIPCIÓN          : Obtiene todos los Notario que estan dados de alta en la base de datos
        ///PARÁMETROS          : 1. Notario.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Notarios_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
        ///MODIFICO             : Roberto González Oseguera
        ///FECHA_MODIFICO       : 20-dic-2011
        ///CAUSA_MODIFICACIÓN   : Agregar campo NUMERO_NOTARIA y filtro dinamico
        ///*******************************************************************************
        public static DataTable Consultar_Notarios(Cls_Cat_Pre_Notarios_Negocio Notario)
        {
            DataTable Dt_Notarios = new DataTable();
            try{
                String Mi_SQL = "SELECT " + Cat_Pre_Notarios.Campo_Notario_ID + " AS NOTARIO_ID, "
                    + Cat_Pre_Notarios.Campo_RFC + " AS RFC, "
                    + Cat_Pre_Notarios.Campo_Numero_Notaria + " AS NUMERO_NOTARIA, "
                    + Cat_Pre_Notarios.Campo_Apellido_Paterno + "|| ' ' ||" 
                    + Cat_Pre_Notarios.Campo_Apellido_Materno + "|| ' ' ||" 
                    + Cat_Pre_Notarios.Campo_Nombre + " AS NOMBRE_COMPLETO" 
                    + " FROM " + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios;
                
                // si la propiedad filtro dinamico tiene valor asignado, agregar a consulta
                if (!String.IsNullOrEmpty(Notario.P_Filtro_Dinamico))
                {
                    Mi_SQL += " WHERE " + Notario.P_Filtro_Dinamico;
                }

                Mi_SQL += " ORDER BY " + Cat_Pre_Notarios.Campo_Apellido_Paterno + ", " 
                    + Cat_Pre_Notarios.Campo_Apellido_Materno 
                    + ", " + Cat_Pre_Notarios.Campo_Nombre;

                DataSet Ds_Notarios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Notarios != null)
                {
                    Dt_Notarios = Ds_Notarios.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Notarios. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Notarios;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Notario
        ///DESCRIPCIÓN          : Obtiene a detalle un Registro de un Notario
        ///PARÁMETROS          : 1. Notario.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Notarios_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Cls_Cat_Pre_Notarios_Negocio Consultar_Datos_Notario(Cls_Cat_Pre_Notarios_Negocio P_Notario)
        {
            String Mi_SQL = "SELECT " + Cat_Pre_Notarios.Campo_Apellido_Paterno + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Apellido_Materno + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Nombre + ", TO_NUMBER(";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Numero_Notaria + ") " 
                + Cat_Pre_Notarios.Campo_Numero_Notaria + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Sexo + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Estado + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Ciudad + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Colonia + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Calle + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Numero_Exterior + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Numero_Interior + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Codigo_Postal + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_RFC + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_CURP + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Estatus + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Telefono + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Fax + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Celular + ", ";
            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_E_Mail;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Notarios.Campo_Notario_ID + " = '" + P_Notario.P_Notario_ID + "'";
            Cls_Cat_Pre_Notarios_Negocio R_Notario = new Cls_Cat_Pre_Notarios_Negocio();
            OracleDataReader Dr_Notarios;
            try{
                Dr_Notarios = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Notario.P_Notario_ID = P_Notario.P_Notario_ID;
                while (Dr_Notarios.Read()) {
                    R_Notario.P_Apellido_Paterno = Dr_Notarios[Cat_Pre_Notarios.Campo_Apellido_Paterno].ToString();
                    R_Notario.P_Apellido_Materno = Dr_Notarios[Cat_Pre_Notarios.Campo_Apellido_Materno].ToString();
                    R_Notario.P_Nombre = Dr_Notarios[Cat_Pre_Notarios.Campo_Nombre].ToString();
                    R_Notario.P_Numero_Notaria = Dr_Notarios[Cat_Pre_Notarios.Campo_Numero_Notaria].ToString();
                    R_Notario.P_Sexo = Dr_Notarios[Cat_Pre_Notarios.Campo_Sexo].ToString();
                    R_Notario.P_Estado = Dr_Notarios[Cat_Pre_Notarios.Campo_Estado].ToString();
                    R_Notario.P_Ciudad = Dr_Notarios[Cat_Pre_Notarios.Campo_Ciudad].ToString();
                    R_Notario.P_Colonia = Dr_Notarios[Cat_Pre_Notarios.Campo_Colonia].ToString();
                    R_Notario.P_Calle = Dr_Notarios[Cat_Pre_Notarios.Campo_Calle].ToString();
                    R_Notario.P_Numero_Exterior = Dr_Notarios[Cat_Pre_Notarios.Campo_Numero_Exterior].ToString();
                    if (Dr_Notarios[Cat_Pre_Notarios.Campo_Numero_Interior] != null) {
                        R_Notario.P_Numero_Interior = Dr_Notarios[Cat_Pre_Notarios.Campo_Numero_Interior].ToString();
                    }
                    R_Notario.P_Codigo_Postal = Dr_Notarios[Cat_Pre_Notarios.Campo_Codigo_Postal].ToString();
                    R_Notario.P_RFC = Dr_Notarios[Cat_Pre_Notarios.Campo_RFC].ToString();
                    R_Notario.P_CURP = Dr_Notarios[Cat_Pre_Notarios.Campo_CURP].ToString();
                    R_Notario.P_Estatus = Dr_Notarios[Cat_Pre_Notarios.Campo_Estatus].ToString();
                    R_Notario.P_Telefono = Dr_Notarios[Cat_Pre_Notarios.Campo_Telefono].ToString();
                    if (Dr_Notarios[Cat_Pre_Notarios.Campo_Fax] != null){
                        R_Notario.P_Fax = Dr_Notarios[Cat_Pre_Notarios.Campo_Fax].ToString();
                    }
                    R_Notario.P_Celular = Dr_Notarios[Cat_Pre_Notarios.Campo_Celular].ToString();
                    R_Notario.P_E_Mail = Dr_Notarios[Cat_Pre_Notarios.Campo_E_Mail].ToString();
                    R_Notario.P_Fax = Dr_Notarios[Cat_Pre_Notarios.Campo_Fax].ToString();
                }
                Dr_Notarios.Close();
            }
            catch (OracleException Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Notario. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Notario;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Notario
        ///DESCRIPCIÓN          : Elimina un Registro de un Notario
        ///PARÁMETROS          : 1. Notario.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Notarios_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26/Octubre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Eliminar_Notario(Cls_Cat_Pre_Notarios_Negocio Notario)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Boolean Eliminar = false;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Notarios.Campo_Notario_ID + " = '" + Notario.P_Notario_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Eliminar = true;
            }
            catch (OracleException Ex)
            {
                if (Ex.Code == 547)
                {
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos.";
                }
                else
                {
                    Mensaje = "No se puede eliminar. El Notario tiene relacion con movimientos de Traslado";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Notarios. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
            return Eliminar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
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