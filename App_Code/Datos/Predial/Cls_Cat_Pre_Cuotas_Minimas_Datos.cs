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
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Pre_Cuotas_Minimas_Datos
/// </summary>
/// 

namespace Presidencia.Catalogo_Cuotas_Minimas.Datos{
    public class Cls_Cat_Pre_Cuotas_Minimas_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuotas_Minimas_Ventana_Emergente
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Cuota Minima
        ///PARAMETROS:     
        ///             1. Cuota_Minima.    Instancia de la Clase de Negocio de Cls_Cat_Pre_Cuotas_Minimas_Negocio
        ///                                 con los datos de la Cuota Minima que va a ser dada de Alta.
        ///CREO: Christian Perez Ibarra.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cuotas_Minimas_Ventana_Emergente(Cls_Cat_Pre_Cuotas_Minimas_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL                        

            try
            {
                Mi_SQL = "";
                Mi_SQL += "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " As Cuota_ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuotas_Minimas.Campo_Cuota  + " As Cuota, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuotas_Minimas.Campo_Año  + " As Anual ";

                Mi_SQL += " FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + "  ";


                if (Datos.P_Cuota_Minima_ID != "" && Datos.P_Cuota_Minima_ID != null)
                {
                    Mi_SQL += " WHERE "
                        + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " = '" + Datos.P_Cuota_Minima_ID + "'";
                }
                else
                {
                    if (Datos.P_Anio != "" && Datos.P_Anio != null)
                    {
                        Mi_SQL += " WHERE "
                            + Cat_Pre_Cuotas_Minimas.Campo_Año + " = '" + Datos.P_Anio + "'";
                        if (Datos.P_Cantidad_Cuota != "" && Datos.P_Cantidad_Cuota != null)
                            Mi_SQL += " AND " + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " = '" + Datos.P_Cantidad_Cuota + "'";

                    }
                    else if (Datos.P_Cantidad_Cuota != "" && Datos.P_Cantidad_Cuota != null)
                    {
                        Mi_SQL += " WHERE "
                            + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " = '" + Datos.P_Cantidad_Cuota + "'";
                    }
                   
                }
                Mi_SQL += " ORDER BY " + Cat_Pre_Cuotas_Minimas.Campo_Año + " DESC";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Mi_SQL + Ex.Message);
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
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Cuota_Minima
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Cuota Minima
        ///PARAMETROS:     
        ///             1. Cuota_Minima.    Instancia de la Clase de Negocio de Cls_Cat_Pre_Cuotas_Minimas_Negocio
        ///                                 con los datos de la Cuota Minima que va a ser dada de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Cuota_Minima(Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuota_Minima){
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {
                String Cuota_Minima_ID = Obtener_ID_Consecutivo(Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas, Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + ", " + Cat_Pre_Cuotas_Minimas.Campo_Año;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cuotas_Minimas.Campo_Cuota;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cuotas_Minimas.Campo_Usuario_Creo + ", " + Cat_Pre_Cuotas_Minimas.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Cuota_Minima_ID + "', " + Cuota_Minima.P_Anio;
                Mi_SQL = Mi_SQL + ", " + Cuota_Minima.P_Cuota;
                Mi_SQL = Mi_SQL + ",'" + Cuota_Minima.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Cuota Minima. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }finally {
                if (Cn.State == ConnectionState.Open) {
                    Cn.Close();
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Cuota_Minima
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Cuota Minima
        ///PARAMETROS:     
        ///             1. Cuota_Minima.  Instancia de la Clase de Negocio de Cuotas Minimas con los datos 
        ///                              del Registro que va a ser Actualizado.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Cuota_Minima(Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuota_Minima){
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {               
                String Mi_SQL = "UPDATE " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " SET " + Cat_Pre_Cuotas_Minimas.Campo_Año + " = " + Cuota_Minima.P_Anio;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " = " + Cuota_Minima.P_Cuota;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Cuotas_Minimas.Campo_Usuario_Modifico + " = '" + Cuota_Minima.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Cuotas_Minimas.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " = '" + Cuota_Minima.P_Cuota_Minima_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
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
                    Mensaje = "Error al intentar modificar un Registro de Cuota Minima. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuotas_Minimas
        ///DESCRIPCIÓN: Obtiene todas las Cuotas Minimas que estan dadas de alta en la Base de Datos
        ///PARAMETROS:   
        ///             1.  Cuota_Minima.   Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es el año.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cuotas_Minimas(Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuota_Minima){
            DataTable Tabla = new DataTable();
            try{
                String Mi_SQL = "SELECT " + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " AS CUOTA_MINIMA_ID, " + Cat_Pre_Cuotas_Minimas.Campo_Año + " AS ANIO";
                Mi_SQL = Mi_SQL + ", TO_CHAR("+ Cat_Pre_Cuotas_Minimas.Campo_Cuota +", '9,999,999,999,999.99') AS CUOTA, TO_CHAR("+Cat_Pre_Cuotas_Minimas.Campo_Cuota+"/6, '9,999,999,999,999.99') AS BIMESTRE";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas;
                if (!Cuota_Minima.P_Anio.Trim().Equals("")) {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cuotas_Minimas.Campo_Año + " = " + Cuota_Minima.P_Anio;        
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Cuotas_Minimas.Campo_Año + " DESC ";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null) {
                    Tabla = dataset.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los registros de Cuotas Minimas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Datos_Cuota_Minima
        ///DESCRIPCIÓN: Obtiene a detalle una Cuota Minima.
        ///PARAMETROS:   
        ///             1. P_Cuota_Minima.   Cuota Minima que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Cuotas_Minimas_Negocio Consultar_Datos_Cuota_Minima(Cls_Cat_Pre_Cuotas_Minimas_Negocio P_Cuota_Minima){
            String Mi_SQL = "SELECT " + Cat_Pre_Cuotas_Minimas.Campo_Año + ", " + Cat_Pre_Cuotas_Minimas.Campo_Cuota+", "+Cat_Pre_Cuotas_Minimas.Campo_Cuota+"/6 as BIMESTRE";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " = '" + P_Cuota_Minima.P_Cuota_Minima_ID + "'";
            Cls_Cat_Pre_Cuotas_Minimas_Negocio R_Cuota_Minima = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Cuota_Minima.P_Cuota_Minima_ID = P_Cuota_Minima.P_Cuota_Minima_ID;
                while (Data_Reader.Read()){
                    R_Cuota_Minima.P_Anio = Data_Reader[Cat_Pre_Cuotas_Minimas.Campo_Año].ToString();
                    R_Cuota_Minima.P_Cuota = Convert.ToDouble(Data_Reader[Cat_Pre_Cuotas_Minimas.Campo_Cuota]);
                }
                Data_Reader.Close();
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar el registro de Cuota Minima. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Cuota_Minima;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Cuota_Minima
        ///DESCRIPCIÓN: Elimina una Cuota Minima de la Base de Datos.
        ///PARAMETROS:   
        ///             1. Cuota_Minima.   Registro que se va a eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 31/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Cuota_Minima(Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuota_Minima){
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {               
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " = '" + Cuota_Minima.P_Cuota_Minima_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                if (Ex.Code == 547){
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar eliminar el registro de Cuota Minima. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje);
            } catch (Exception Ex) {
                Mensaje = "Error al intentar eliminar el registro de Cuota Minima. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            } finally {
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
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID) {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals("")) {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            } catch (OracleException Ex) {
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
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID) {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }


        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consultar_Cuota_Minima_Anio
        /// 	DESCRIPCIÓN: Consulta la cuota minima de un anio dado, si no encuentra la cuota regresa 0
        /// 	PARÁMETROS:
        /// 		1. Anio: Año del que se requiere la cuota minima
        /// 	CREO: Roberto González Oseguera
        /// 	FECHA_CREO: 23-jul-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Decimal Consultar_Cuota_Minima_Anio(String Anio)
        {
            String Mi_SQL;
            Decimal Cuota;
            object Resultado_Obj;
            
            // formar consulta
            Mi_SQL = "SELECT " + Cat_Pre_Cuotas_Minimas.Campo_Cuota;
            Mi_SQL += " FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas;
            Mi_SQL += " WHERE " + Cat_Pre_Cuotas_Minimas.Campo_Año + " = " + Anio;


            try
            {
                // ejecutar consulta
                Resultado_Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                // si no se obtuvo resultado de la consulta
                if (Convert.IsDBNull(Resultado_Obj) || Resultado_Obj == null)
                {
                    return (Decimal)0;              // regresar 0
                }
                else
                {
                    //si se obtiene un valor decimal de la consulta, regresar ese valor
                    if (decimal.TryParse(Resultado_Obj.ToString(), out Cuota))
                        return Cuota;
                    else
                        return (Decimal)0;           // si no, regresar 0
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el registro de Cuota Minima. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Existe
        ///DESCRIPCIÓN          : Devuelve un DataTable si encontró registros con los parámetros proporcionados
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 15/Diciembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Validar_Existe(Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuota_Minima)
        {
            DataSet Ds_Cuotas_Minimas = null;
            Boolean Existente = false;
            String Mi_SQL;

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + ", " + Cat_Pre_Cuotas_Minimas.Campo_Año;
                Mi_SQL += " FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas;
                Mi_SQL += " WHERE ";
                if (Cuota_Minima.P_Cuota_Minima_ID != null)
                {
                    if (Cuota_Minima.P_Cuota_Minima_ID.Trim() != "")
                    {
                        Mi_SQL += Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " != '" + Cuota_Minima.P_Cuota_Minima_ID + "' AND ";
                    }
                }
                Mi_SQL += Cat_Pre_Cuotas_Minimas.Campo_Año + " = '" + Cuota_Minima.P_Anio + "'";

                Ds_Cuotas_Minimas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Cuotas_Minimas != null)
                {
                    if (Ds_Cuotas_Minimas.Tables[0] != null)
                    {
                        if (Ds_Cuotas_Minimas.Tables[0].Rows.Count > 0)
                        {
                            Existente = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cuotas Mínimas. Error: [" + ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Existente;
        }
    }
}