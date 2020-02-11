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
using Presidencia.Catalogo_Descuentos_Recargos.Negocio;

/// <summary>
/// Summary description for  Cls_Cat_Pre_Descuentos_Recargos_Datos
/// </summary>
namespace Presidencia.Catalogo_Descuentos_Recargos.Datos{
    public class Cls_Cat_Pre_Descuentos_Recargos_Datos{
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Descuento_Recargos
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Descuento Recargos
        ///PARAMETROS:     
        ///             1. Descuento_Recargo.   Instancia de la Clase de Negocio de Cls_Cat_Pre_Descuento_Recargos_Negocio
        ///                                     con los datos del Descuento Recargo que va a ser dada de Alta.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Descuento_Recargos(Cls_Cat_Pre_Descuentos_Recargos_Negocio Descuento_Recargo) {
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
                String Descuento_Recargo_ID = Obtener_ID_Consecutivo(Cat_Pre_Descuentos_Recargos.Tabla_Cat_Pre_Descuentos_Recargos, Cat_Pre_Descuentos_Recargos.Campo_Descuento_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Descuentos_Recargos.Tabla_Cat_Pre_Descuentos_Recargos;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Descuentos_Recargos.Campo_Descuento_ID + ", " + Cat_Pre_Descuentos_Recargos.Campo_Año;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Enero + ", " + Cat_Pre_Descuentos_Recargos.Campo_Febrero;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Marzo + ", " + Cat_Pre_Descuentos_Recargos.Campo_Abril;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Mayo + ", " + Cat_Pre_Descuentos_Recargos.Campo_Junio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Julio + ", " + Cat_Pre_Descuentos_Recargos.Campo_Agosto;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Septiembre + ", " + Cat_Pre_Descuentos_Recargos.Campo_Octubre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Noviembre + ", " + Cat_Pre_Descuentos_Recargos.Campo_Diciembre;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Usuario_Creo + ", " + Cat_Pre_Descuentos_Recargos.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Descuento_Recargo_ID + "', " + Descuento_Recargo.P_Anio;
                Mi_SQL = Mi_SQL + ", " + Descuento_Recargo.P_Enero + ", " + Descuento_Recargo.P_Febrero;
                Mi_SQL = Mi_SQL + ", " + Descuento_Recargo.P_Marzo + ", " + Descuento_Recargo.P_Abril;
                Mi_SQL = Mi_SQL + ", " + Descuento_Recargo.P_Mayo + ", " + Descuento_Recargo.P_Junio;
                Mi_SQL = Mi_SQL + ", " + Descuento_Recargo.P_Julio + ", " + Descuento_Recargo.P_Agosto;
                Mi_SQL = Mi_SQL + ", " + Descuento_Recargo.P_Septiembre + ", " + Descuento_Recargo.P_Octubre;
                Mi_SQL = Mi_SQL + ", " + Descuento_Recargo.P_Noviembre + ", " + Descuento_Recargo.P_Diciembre;
                Mi_SQL = Mi_SQL + ",'" + Descuento_Recargo.P_Usuario + "'" + ", SYSDATE";
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Descuentos Recargos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }finally {
                 Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Descuento_Recargos
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Descuento Recargos
        ///PARAMETROS:     
        ///             1. Descuento_Recargo.   Instancia de la Clase de Negocio de Descuentos Recargos con los datos 
        ///                                     del Registro que va a ser Actualizado.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Descuento_Recargos(Cls_Cat_Pre_Descuentos_Recargos_Negocio Descuento_Recargo){
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Descuentos_Recargos.Tabla_Cat_Pre_Descuentos_Recargos + " SET " + Cat_Pre_Descuentos_Recargos.Campo_Año + " = " + Descuento_Recargo.P_Anio;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Recargos.Campo_Enero + " = " + Descuento_Recargo.P_Enero + "," + Cat_Pre_Descuentos_Recargos.Campo_Febrero + " = " + Descuento_Recargo.P_Febrero;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Recargos.Campo_Marzo + " = " + Descuento_Recargo.P_Marzo + "," + Cat_Pre_Descuentos_Recargos.Campo_Abril + " = " + Descuento_Recargo.P_Abril;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Recargos.Campo_Mayo + " = " + Descuento_Recargo.P_Mayo + "," + Cat_Pre_Descuentos_Recargos.Campo_Junio + " = " + Descuento_Recargo.P_Junio;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Recargos.Campo_Julio + " = " + Descuento_Recargo.P_Julio + "," + Cat_Pre_Descuentos_Recargos.Campo_Agosto + " = " + Descuento_Recargo.P_Agosto;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Recargos.Campo_Septiembre + " = " + Descuento_Recargo.P_Septiembre + "," + Cat_Pre_Descuentos_Recargos.Campo_Octubre + " = " + Descuento_Recargo.P_Octubre;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Recargos.Campo_Noviembre + " = " + Descuento_Recargo.P_Noviembre + "," + Cat_Pre_Descuentos_Recargos.Campo_Diciembre + " = " + Descuento_Recargo.P_Diciembre;
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Recargos.Campo_Usuario_Modifico + " = '" + Descuento_Recargo.P_Usuario + "'";
                Mi_SQL = Mi_SQL + "," + Cat_Pre_Descuentos_Recargos.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Descuentos_Recargos.Campo_Descuento_ID + " = '" + Descuento_Recargo.P_Descuento_ID + "'";
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
                    Mensaje = "Error al intentar Modificar el Registro de Descuentos Recargos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Descuentos_Recargos
        ///DESCRIPCIÓN: Obtiene todos los Descuentos Recargos que estan dadas de alta en la Base de Datos
        ///PARAMETROS:   
        ///             1.  Descuentos_Recargo.     Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                         caso el filtro es el año.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Descuentos_Recargos(Cls_Cat_Pre_Descuentos_Recargos_Negocio Descuentos_Recargo) {
            DataTable Tabla = new DataTable();
            try {                
                String Mi_SQL = "SELECT " + Cat_Pre_Descuentos_Recargos.Campo_Descuento_ID + " AS DESCUENTO_ID, " + Cat_Pre_Descuentos_Recargos.Campo_Año + " AS ANIO";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Descuentos_Recargos.Tabla_Cat_Pre_Descuentos_Recargos;
                if (!Descuentos_Recargo.P_Anio.ToString().Trim().Equals("") && Descuentos_Recargo.P_Anio != (-1)) {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Descuentos_Recargos.Campo_Año + " = " + Descuentos_Recargo.P_Anio;        
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Descuentos_Recargos.Campo_Descuento_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null) {
                    Tabla = dataset.Tables[0];
                }
            }catch(Exception Ex){
                String Mensaje = "Error al intentar consultar los Registros de Descuentos Recargos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Descuento_Recargos
        ///DESCRIPCIÓN: Obtiene a detalle una Descuento Recargo.
        ///PARAMETROS:   
        ///             1. P_Descuento_Recargo.   Descuento Recargo que se va ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Descuentos_Recargos_Negocio Consultar_Datos_Descuento_Recargos(Cls_Cat_Pre_Descuentos_Recargos_Negocio P_Descuento_Recargo) {
            String Mi_SQL = "SELECT " + Cat_Pre_Descuentos_Recargos.Campo_Año;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Enero + ", " + Cat_Pre_Descuentos_Recargos.Campo_Febrero;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Marzo + ", " + Cat_Pre_Descuentos_Recargos.Campo_Abril;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Mayo + ", " + Cat_Pre_Descuentos_Recargos.Campo_Junio;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Julio + ", " + Cat_Pre_Descuentos_Recargos.Campo_Agosto;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Septiembre + ", " + Cat_Pre_Descuentos_Recargos.Campo_Octubre;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Descuentos_Recargos.Campo_Noviembre + ", " + Cat_Pre_Descuentos_Recargos.Campo_Diciembre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Descuentos_Recargos.Tabla_Cat_Pre_Descuentos_Recargos;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Descuentos_Recargos.Campo_Descuento_ID + " = '" + P_Descuento_Recargo.P_Descuento_ID + "'";
            Cls_Cat_Pre_Descuentos_Recargos_Negocio R_Descuento_Recargo = new Cls_Cat_Pre_Descuentos_Recargos_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Descuento_Recargo.P_Descuento_ID = P_Descuento_Recargo.P_Descuento_ID;
                while (Data_Reader.Read()){
                    R_Descuento_Recargo.P_Anio = Convert.ToInt32(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Año]);
                    R_Descuento_Recargo.P_Enero = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Enero]);
                    R_Descuento_Recargo.P_Febrero = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Febrero]);
                    R_Descuento_Recargo.P_Marzo = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Marzo]);
                    R_Descuento_Recargo.P_Abril = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Abril]);
                    R_Descuento_Recargo.P_Mayo = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Mayo]);
                    R_Descuento_Recargo.P_Junio = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Junio]);
                    R_Descuento_Recargo.P_Julio = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Julio]);
                    R_Descuento_Recargo.P_Agosto = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Agosto]);
                    R_Descuento_Recargo.P_Septiembre = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Septiembre]);
                    R_Descuento_Recargo.P_Octubre = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Octubre]);
                    R_Descuento_Recargo.P_Noviembre = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Noviembre]);
                    R_Descuento_Recargo.P_Diciembre = Convert.ToDouble(Data_Reader[Cat_Pre_Descuentos_Recargos.Campo_Diciembre]); 
                }
                Data_Reader.Close();
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar el Registros de Descuentos Recargos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return R_Descuento_Recargo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Descuento_Recargo
        ///DESCRIPCIÓN: Elimina un Descuento Recargo de la Base de Datos.
        ///PARAMETROS:   
        ///             1. Descuento_Recargo.   Registro que se va a eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Descuento_Recargo(Cls_Cat_Pre_Descuentos_Recargos_Negocio Descuento_Recargo){
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Descuentos_Recargos.Tabla_Cat_Pre_Descuentos_Recargos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Descuentos_Recargos.Campo_Descuento_ID + " = '" + Descuento_Recargo.P_Descuento_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            } catch (OracleException Ex) {
                if (Ex.Code == 547){
                    Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar Eliminar los registros de Descuentos Recargos. Error: [" + Ex.Message + "]"; 
                    throw new Exception(Mensaje);
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex) {
                Mensaje = "Error al intentar Eliminar los registros de Descuentos Recargos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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

    }
}