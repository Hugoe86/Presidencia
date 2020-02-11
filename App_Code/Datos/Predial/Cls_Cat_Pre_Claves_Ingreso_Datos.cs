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
using Presidencia.Catalogo_Claves_Ingreso.Negocio;

namespace Presidencia.Catalogo_Claves_Ingreso.Datos
{

    public class Cls_Cat_Pre_Claves_Ingreso_Datos
    {

        #region Dar de Alta

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Clave_Ingreso
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Clave de Igreso
        ///PARAMETROS:     
        ///             1. Clave.  Instancia de la Clase de Negocio de Claves de Ingreso con los datos 
        ///                        de la Clave de Ingreso que va a ser dada de Alta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Clave_Ingreso(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
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
                String Clave_ID = Obtener_ID_Consecutivo(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso, Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + ", " + Cat_Pre_Claves_Ingreso.Campo_Grupo_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso.Campo_Estatus + ", " + Cat_Pre_Claves_Ingreso.Campo_Clave;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso.Campo_Cuenta_Contable_ID + ", " + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso.Campo_Descripcion + ", " + Cat_Pre_Claves_Ingreso.Campo_Fundamento;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso.Campo_Usuario_Creo + ", " + Cat_Pre_Claves_Ingreso.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Clave_ID + "', '" + Clave.P_Grupo_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Clave.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Clave.P_Clave + "'";
                Mi_SQL = Mi_SQL + ",'" + Clave.P_Cuenta_Contable_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Clave.P_Dependencia_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Clave.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ",'" + Clave.P_Fundamento + "'";
                Mi_SQL = Mi_SQL + ",'" + Clave.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
                Clave.P_Clave_Ingreso_ID = Clave_ID;
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
                    Mensaje = "Error al intentar dar de Alta una P_Clave de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Alta_Costo_Clave
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo costo de clave
        ///PARAMETROS:     
        ///             1. Clave.  Instancia de la Clase de Negocio de Claves de Ingreso con los datos 
        ///                        del costo de la Clave que va a ser dada de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Costo_Clave(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
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
                String Clave_ID = Obtener_ID_Consecutivo(Cat_Pre_Claves_Ing_Costos.Tabla_Cat_Pre_Claves_Ing_Costos, Cat_Pre_Claves_Ing_Costos.Campo_Costo_Clave_ID, 5);
                String Mi_SQL = "INSERT INTO " + Cat_Pre_Claves_Ing_Costos.Tabla_Cat_Pre_Claves_Ing_Costos;
                Mi_SQL = Mi_SQL + " (" + Cat_Pre_Claves_Ing_Costos.Campo_Costo_Clave_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ing_Costos.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ing_Costos.Campo_Anio;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ing_Costos.Campo_Costo;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ing_Costos.Campo_Usuario_Creo + ", " + Cat_Pre_Claves_Ingreso.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Clave_ID + "', '" + Clave.P_Clave_Ingreso_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Clave.P_Anio + "'"; 
                Mi_SQL = Mi_SQL + "," + Convert.ToDouble(Clave.P_Costo) + "";
                Mi_SQL = Mi_SQL + ",'" + Clave.P_Usuario + "'";
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
                    Mensaje = "Error al intentar dar de Alta una P_Clave de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Alta_Tabla_Movimientos_Detalles
        ///DESCRIPCIÓN: Llena la tabla de Claves de Ingreso Detalles con los detalles de Movimientos
        ///             de esa Clave de Ingreso.
        ///PARAMETROS:   
        ///             1. Clave.   Campos necesarios para llenar la tabla de Claves de Ingreso Detalles.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO: Miguel Angel Bedolla Moreno
        ///FECHA_MODIFICO: 18/Noviembre/2011
        ///CAUSA_MODIFICACIÓN: Se cambio para costos de claves.
        ///*******************************************************************************
        public static void Alta_Tabla_Movimientos_Detalles(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();      
            Cmd.Connection = Cn;     
            try
            {
                for (int cont = 0; cont < Clave.P_Movimientos.Rows.Count; cont++)
                {
                    Trans = Cn.BeginTransaction();
                    Cmd.Transaction = Trans;
                    String Detalle_ID = Obtener_ID_Consecutivo(Cat_Pre_Claves_Ing_Costos.Tabla_Cat_Pre_Claves_Ing_Costos, Cat_Pre_Claves_Ing_Costos.Campo_Costo_Clave_ID, 5);
                    String Mi_SQL = "INSERT INTO " + Cat_Pre_Claves_Ing_Costos.Tabla_Cat_Pre_Claves_Ing_Costos;
                    Mi_SQL = Mi_SQL + " (" + Cat_Pre_Claves_Ing_Costos.Campo_Costo_Clave_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ing_Costos.Campo_Clave_Ingreso_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ing_Costos.Campo_Anio;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ing_Costos.Campo_Costo;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ing_Costos.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ing_Costos.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + " ) VALUES (";
                    Mi_SQL = Mi_SQL + "'" + Detalle_ID + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Clave_Ingreso_ID + "', ";
                    Mi_SQL = Mi_SQL + "" + Clave.P_Movimientos.Rows[cont]["ANIO"].ToString().Trim() + ", ";
                    Mi_SQL = Mi_SQL + "" + Clave.P_Movimientos.Rows[cont]["COSTO"].ToString().Trim() + ", ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Usuario + "', SYSDATE" + ")";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                }
                
            }
            catch (OracleException Ex)
            {
                //Trans.Rollback();
                Mensaje = "Error al intentar dar de Atla un Detalle de Movimiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Tabla_Otros_Pagos_Detalles
        ///DESCRIPCIÓN: Llena la tabla de Claves de Ingreso Detalles con los detalles de Otros Pagos
        ///             de esa Clave de Ingreso.
        ///PARAMETROS:   
        ///             1. Clave.   Campos necesarios para llenar la tabla de Claves de Ingreso Detalles.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Tabla_Otros_Pagos_Detalles(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();       
            Cmd.Connection = Cn;  
            try
            {
                if (Clave.P_Otros_Pagos.Rows[0]["CLAVE_INGRESO_ID"].ToString().Trim().Equals(""))
                {
                    for (int cont = 0; cont < Clave.P_Otros_Pagos.Rows.Count; cont++)
                    {
                        Trans = Cn.BeginTransaction();
                        Cmd.Transaction = Trans;
                        String Detalle_ID = Obtener_ID_Consecutivo(Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det, Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID, 5);
                        String Clave_Ingreso_ID = Obtener_Ultima_Clave_Ingreso();
                        String Mi_SQL = "INSERT INTO " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                        Mi_SQL = Mi_SQL + " (" + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Pago_ID;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + " ) VALUES (";
                        Mi_SQL = Mi_SQL + "'" + Detalle_ID + "', ";
                        Mi_SQL = Mi_SQL + "'" + Clave_Ingreso_ID + "', ";
                        Mi_SQL = Mi_SQL + "'" + Clave.P_Otros_Pagos.Rows[cont]["CLAVE"].ToString().Trim() + "', ";
                        Mi_SQL = Mi_SQL + "'" + Clave.P_Usuario + "', SYSDATE" + ")"; Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Trans.Commit();
                    }
                }
                else{
                for (int cont = 0; cont < Clave.P_Otros_Pagos.Rows.Count; cont++)
                {
                    Trans = Cn.BeginTransaction();
                    Cmd.Transaction = Trans;
                    String Detalle_ID = Obtener_ID_Consecutivo(Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det, Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID, 5);
                    String Mi_SQL = "INSERT INTO " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                    Mi_SQL = Mi_SQL + " (" + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Pago_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + " ) VALUES (";
                    Mi_SQL = Mi_SQL + "'" + Detalle_ID + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Otros_Pagos.Rows[cont]["CLAVE_INGRESO_ID"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Otros_Pagos.Rows[cont]["CLAVE"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Usuario + "', SYSDATE" + ")"; Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                }
                    }
               
            }
            catch (OracleException Ex)
            {
                //Trans.Rollback();
                Mensaje = "Error al intentar dar de Atla un Detalle de Otros Pagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        public static String Obtener_Ultima_Clave_Ingreso()
        {
            String Id = "";
            try
            {
                String Mi_SQL = "SELECT MAX(" + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + ") FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Obj_Temp.ToString();
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Tabla_Documentos_Detalles
        ///DESCRIPCIÓN: Llena la tabla de Claves de Ingreso Detalles con los detalles de Documentos
        ///             de esa Clave de Ingreso.
        ///PARAMETROS:   
        ///             1. Clave.   Campos necesarios para llenar la tabla de Claves de Ingreso Detalles.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Tabla_Documentos_Detalles(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();       
            Cmd.Connection = Cn;
            try
            {
                for (int cont = 0; cont < Clave.P_Documentos.Rows.Count; cont++)
                {
                    Trans = Cn.BeginTransaction();
                    Cmd.Transaction = Trans;
                    String Detalle_ID = Obtener_ID_Consecutivo(Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det, Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID, 5);
                    String Mi_SQL = "INSERT INTO " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                    Mi_SQL = Mi_SQL + " (" + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Documento_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + " ) VALUES (";
                    Mi_SQL = Mi_SQL + "'" + Detalle_ID + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Documentos.Rows[cont]["CLAVE_INGRESO_ID"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Documentos.Rows[cont]["CLAVE"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Usuario + "', SYSDATE" + ")"; Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                }
              
            }
            catch (OracleException Ex)
            {
                //Trans.Rollback();
                Mensaje = "Error al intentar dar de Atla un Detalle de Tipos de Constancias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Tabla_Gastos_Ejecucion_Detalles
        ///DESCRIPCIÓN: Llena la tabla de Claves de Ingreso Detalles con los detalles de Gastos de Ejecucion
        ///             de esa Clave de Ingreso.
        ///PARAMETROS:   
        ///             1. Clave.   Campos necesarios para llenar la tabla de Claves de Ingreso Detalles.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Tabla_Gastos_Ejecucion_Detalles(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();         
            Cmd.Connection = Cn;
            try
            {
                for (int cont = 0; cont < Clave.P_Gastos_Ejecucion.Rows.Count; cont++)
                {
                    Trans = Cn.BeginTransaction();
                    Cmd.Transaction = Trans;
                    String Detalle_ID = Obtener_ID_Consecutivo(Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det, Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID, 5);
                    String Mi_SQL = "INSERT INTO " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                    Mi_SQL = Mi_SQL + " (" + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Gasto_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + " ) VALUES (";
                    Mi_SQL = Mi_SQL + "'" + Detalle_ID + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Gastos_Ejecucion.Rows[cont]["CLAVE_INGRESO_ID"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Gastos_Ejecucion.Rows[cont]["CLAVE"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Usuario + "', SYSDATE" + ")"; Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                }
               
            }
            catch (OracleException Ex)
            {
                //Trans.Rollback();
                Mensaje = "Error al intentar dar de Atla un Detalle de Gasto de Ejecucion. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Tabla_Predial_Traslado_Detalles
        ///DESCRIPCIÓN: Llena la tabla de Claves de Ingreso Detalles con los detalles de Predial Traslado
        ///             de esa Clave de Ingreso.
        ///PARAMETROS:   
        ///             1. Clave.   Campos necesarios para llenar la tabla de Claves de Ingreso Detalles.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Tabla_Predial_Traslado_Detalles(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            //Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            //Cmd.Transaction = Trans;
            try
            {
                for (int cont = 0; cont < Clave.P_Predial_Traslado.Rows.Count; cont++)
                {
                    Trans = Cn.BeginTransaction();
                    Cmd.Transaction = Trans;
                    String Detalle_ID = Obtener_ID_Consecutivo(Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det, Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID, 5);
                    String Mi_SQL = "INSERT INTO " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                    Mi_SQL = Mi_SQL + " (" + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo_Predial_Traslado;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + " ) VALUES (";
                    Mi_SQL = Mi_SQL + "'" + Detalle_ID + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Predial_Traslado.Rows[cont]["CLAVE_INGRESO_ID"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Predial_Traslado.Rows[cont]["TIPO"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Predial_Traslado.Rows[cont]["TIPO_PREDIAL_TRASLADO"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Clave.P_Usuario + "', SYSDATE" + ")"; Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                }
                
                
            }
            catch (OracleException Ex)
            {
                //Trans.Rollback();
                Mensaje = "Error al intentar dar de Atla un Detalle de Gasto de Ejecucion. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        #endregion

        #region Consultar Detalles

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Buscar_Clave_Ingreso
        ///DESCRIPCIÓN: Realiza una consulta para obtener un detalle especifico de la Clave de Ingreso
        ///PARAMETROS:
        ///             1. Detalle. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el detalle que se quiere buscar
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO: Roberto Gonzalez
        ///FECHA_MODIFICO: 26/ago/2011
        ///CAUSA_MODIFICACIÓN: Se agregó condición para buscar por ID de constancia (antes documento ID)
        ///*******************************************************************************
        public static DataTable Consultar_Clave_Ingreso(Cls_Cat_Pre_Claves_Ingreso_Negocio Tipo)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT  C." + Cat_Pre_Claves_Ingreso.Campo_Clave + " AS CLAVE";
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID + " AS DEPENDENCIA_ID";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso  + " C LEFT JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det + " D ON ";
                Mi_SQL = Mi_SQL + " C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " = D." + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID; 
                Mi_SQL = Mi_SQL + " WHERE ";
                if(!String.IsNullOrEmpty(Tipo.P_Tipo) && !String.IsNullOrEmpty(Tipo.P_Tipo_Predial_Traslado))
                {
                    Mi_SQL = Mi_SQL + "(D." +Cat_Pre_Claves_Ingreso_Det.Campo_Tipo + " = " + "'" + Tipo.P_Tipo + "'" + " AND ";
                    Mi_SQL = Mi_SQL + "D." +Cat_Pre_Claves_Ingreso_Det.Campo_Tipo_Predial_Traslado + " = " + "'" + Tipo.P_Tipo_Predial_Traslado + "')";

                }
                // revisar si se proporciono un id de documento (cambio a constancia)
                else if (!String.IsNullOrEmpty(Tipo.P_Documento_ID))
                {
                    Mi_SQL += "D." + Cat_Pre_Claves_Ingreso_Det.Campo_Documento_ID + " = " + "'" + Tipo.P_Documento_ID + "'";
                }
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Clave_Ingreso_Por_ID
        ///DESCRIPCIÓN: Realiza una consulta para obtener los datos de la clave por id
        ///PARAMETROS:
        ///CREO: Ismael Prieto Sánchez
        ///FECHA_CREO: 16/Octubre/2011 
        ///MODIFICO: 
        ///FECHA_MODIFICO: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        public static DataTable Consultar_Clave_Ingreso_Por_ID(Cls_Cat_Pre_Claves_Ingreso_Negocio Tipo)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT  " + Cat_Pre_Claves_Ingreso.Campo_Clave + " AS CLAVE";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID + " AS DEPENDENCIA_ID";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " = '" + Tipo.P_Clave_Ingreso_ID + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Clave_Movimiento
        ///DESCRIPCIÓN: Llena el Grid con las Claves de Ingreso existentes en la base de datos
        ///PARAMETROS:
        ///             1. Movimiento. Instancia de la Clase de Claves de Ingreso  con 
        ///                            la clave de la clave de ingreso para consultar sus detalles
        ///                            de Movimiento.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataSet Consultar_Clave_Movimiento(Cls_Cat_Pre_Claves_Ingreso_Negocio Movimiento)
        {
            DataTable Tabla = new DataTable();
            DataSet dataset = new DataSet();
            try
            {
                String Mi_SQL = "SELECT CONCAT(" + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , " + Cat_Pre_Movimientos.Campo_Descripcion + "))";
                Mi_SQL = Mi_SQL + " AS NOMBRE ";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Movimientos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + "'" + Movimiento.P_Movimiento_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Grupos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return dataset;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Clave_Documento
        ///DESCRIPCIÓN: Llena el Grid de Tipos de Documentos con los tipos de documento existentes en la base de datos
        ///PARAMETROS:
        ///             1. Documento. Instancia de la Clase de Claves de Ingreso  con 
        ///                            la clave de la clave de ingreso para consultar sus detalles
        ///                            de Tipos de Documento.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataSet Consultar_Clave_Documento(Cls_Cat_Pre_Claves_Ingreso_Negocio Documento)
        {
            DataTable Tabla = new DataTable();
            DataSet dataset = new DataSet();
            try
            {
                String Mi_SQL = "SELECT CONCAT(" + Cat_Pre_Tipos_Constancias.Campo_Clave;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , " + Cat_Pre_Tipos_Constancias.Campo_Descripcion + "))";
                Mi_SQL = Mi_SQL + " AS NOMBRE ";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Constancias.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + " = " + "'" + Documento.P_Documento_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID;
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Constancias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return dataset;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Clave_Otro_Pago
        ///DESCRIPCIÓN: Llena el grid de Otros Pagos con los otros pagos existentes en la base de datos
        ///PARAMETROS:
        ///             1. Pago. Instancia de la Clase de Claves de Ingreso  con 
        ///                      la clave de la clave de ingreso para consultar sus detalles
        ///                      de Otros Pagos.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataSet Consultar_Clave_Otro_Pago(Cls_Cat_Pre_Claves_Ingreso_Negocio Pago)
        {
            DataTable Tabla = new DataTable();
            DataSet dataset = new DataSet();
            try
            {
                String Mi_SQL = "SELECT CONCAT(" + Cat_Pre_Otros_Pagos.Campo_Pago_ID;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , " + Cat_Pre_Otros_Pagos.Campo_Descripcion + "))";
                Mi_SQL = Mi_SQL + " AS NOMBRE ";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Otros_Pagos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Otros_Pagos.Tabla_Cat_Pre_Otros_Pagos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Otros_Pagos.Campo_Pago_ID + " = " + "'" + Pago.P_Pago_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Otros_Pagos.Campo_Pago_ID;
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Otros Pagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return dataset;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Clave_Gasto_Ejecucion
        ///DESCRIPCIÓN: Llena el Grid de Gastos de Ejecucion con los Gastos de Ejecucion existentes en la base de datos
        ///PARAMETROS:
        ///             1. Gasto. Instancia de la Clase de Claves de Ingreso  con 
        ///                       la clave de la clave de ingreso para consultar sus detalles
        ///                       de Gastos de Ejecucion.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataSet Consultar_Clave_Gasto_Ejecucion(Cls_Cat_Pre_Claves_Ingreso_Negocio Gasto)
        {
            DataTable Tabla = new DataTable();
            DataSet dataset = new DataSet();
            try
            {
                String Mi_SQL = "SELECT CONCAT(" + Cat_Pre_Gastos_Ejecucion.Campo_Clave;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , " + Cat_Pre_Gastos_Ejecucion.Campo_Descripcion + "))";
                Mi_SQL = Mi_SQL + " AS NOMBRE ";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Gastos_Ejecucion.Tabla_Cat_Pre_Gastos_Ejecucion;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID + " = " + "'" + Gasto.P_Gasto_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID;
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Grupos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return dataset;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Buscar_Campo_Detalle_Movimiento
        ///DESCRIPCIÓN: Realiza una consulta para obtener un detalle especifico de la Clave de Ingreso
        ///PARAMETROS:
        ///             1. Detalle. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el detalle que se quiere buscar
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 11/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Buscar_Campo_Detalle_Movimiento(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
        {
            DataTable Tabla =  new DataTable();
            try
            {
                String Mi_SQL = "SELECT * FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " = " + "'" + Detalle.P_Clave_Ingreso_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Movimiento_ID + " = " + "'" + Detalle.P_Clave + "'" ;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Buscar_Campo_Detalle_Otro_Pago
        ///DESCRIPCIÓN: Realiza una consulta para obtener un detalle especifico de la Clave de Ingreso
        ///PARAMETROS:
        ///             1. Detalle. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el detalle que se quiere buscar
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 11/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Buscar_Campo_Detalle_Otro_Pago(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT * FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " = " + "'" + Detalle.P_Clave_Ingreso_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Pago_ID + " = " + "'" + Detalle.P_Clave + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Buscar_Campo_Detalle_Documento
        ///DESCRIPCIÓN: Realiza una consulta para obtener un detalle especifico de la Clave de Ingreso
        ///PARAMETROS:
        ///             1. Detalle. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el detalle que se quiere buscar
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 11/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Buscar_Campo_Detalle_Documento(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT * FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " = " + "'" + Detalle.P_Clave_Ingreso_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Documento_ID + " = " + "'" + Detalle.P_Clave + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Buscar_Campo_Detalle_Gasto
        ///DESCRIPCIÓN: Realiza una consulta para obtener un detalle especifico de la Clave de Ingreso
        ///PARAMETROS:
        ///             1. Detalle. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el detalle que se quiere buscar
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 11/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Buscar_Campo_Detalle_Gasto(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT * FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " = " + "'" + Detalle.P_Clave_Ingreso_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Gasto_ID + " = " + "'" + Detalle.P_Clave + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Buscar_Campo_Detalle_Gasto
        ///DESCRIPCIÓN: Realiza una consulta para obtener un detalle especifico de la Clave de Ingreso
        ///PARAMETROS:
        ///             1. Detalle. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el detalle que se quiere buscar
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 11/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Buscar_Campo_Predial_Traslado(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT * FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " = " + "'" + Detalle.P_Clave_Ingreso_ID + "'";
                Mi_SQL = Mi_SQL + " AND (" + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo + " = " + "'" + Detalle.P_Tipo + "' AND ";
                Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo_Predial_Traslado + " = '" + Detalle.P_Tipo_Predial_Traslado + "')";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Buscar_Clave_Ingreso
        ///DESCRIPCIÓN: Realiza una consulta para obtener un detalle especifico de la Clave de Ingreso
        ///PARAMETROS:
        ///             1. Detalle. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el detalle que se quiere buscar
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataSet Buscar_Clave_Ingreso(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
        {
            DataTable Tabla = new DataTable();
            DataSet dataset = new DataSet();
            try
            {
                String Mi_SQL = "SELECT * FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Clave + " = " + "'" + Detalle.P_Clave + "'";
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return dataset;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Maxima
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:   
        ///             1. Detalle: Campo del cual se quiere obtener la Ultima Clave
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_Clave_Maxima(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
        {
            String Id = "";
            try
            {
                String Mi_SQL = "SELECT MAX(" + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID + ") FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
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
        ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Maxima_Costos
        ///DESCRIPCIÓN: Obtiene el ID Consecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 18/Noviembre/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_Clave_Maxima_Costos()
        {
            String Id = "";
            try
            {
                String Mi_SQL = "SELECT MAX(" + Cat_Pre_Claves_Ing_Costos.Campo_Costo_Clave_ID + ") FROM " + Cat_Pre_Claves_Ing_Costos.Tabla_Cat_Pre_Claves_Ing_Costos;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = (Convert.ToInt32(Obj_Temp) + 1).ToString("00000");
                }
                else
                {
                    Id = "00001";
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        #endregion

        #region Llenar Tablas

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Clave
        ///DESCRIPCIÓN: Obtiene todas las Claves de Ingreso que estan dados de alta en la Base de Datos
        ///             de acuerdo a la Busqueda.
        ///PARAMETROS:
        ///             1. Clave. Instancia de la Clase de Claves de Ingreso  con 
        ///                       la clave ingresada por el usuario para la busqueda.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Clave(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso.Campo_Clave;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", CONCAT(R." + Cat_Pre_Ramas.Campo_Clave;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , R." + Cat_Pre_Grupos.Campo_Nombre + "))";
                Mi_SQL = Mi_SQL + " AS NOMBRE_RAMA";
                Mi_SQL = Mi_SQL + ", R." + Cat_Pre_Ramas.Campo_Rama_ID;
                Mi_SQL = Mi_SQL + ", R." + Cat_Pre_Ramas.Campo_Nombre + " AS RAMA";
                Mi_SQL = Mi_SQL + ", CONCAT(G." + Cat_Pre_Grupos.Campo_Clave;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , G." + Cat_Pre_Grupos.Campo_Nombre + "))";
                Mi_SQL = Mi_SQL + " AS NOMBRE_GRUPO";
                Mi_SQL = Mi_SQL + ", G." + Cat_Pre_Grupos.Campo_Grupo_ID;
                Mi_SQL = Mi_SQL + ", G." + Cat_Pre_Grupos.Campo_Nombre + " AS GRUPO";
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso.Campo_Fundamento;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " C ";
                Mi_SQL = Mi_SQL + " JOIN  " + Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + " G ";
                Mi_SQL = Mi_SQL + " ON C." + Cat_Pre_Claves_Ingreso.Campo_Grupo_ID + " = G." + Cat_Pre_Grupos.Campo_Grupo_ID;
                Mi_SQL = Mi_SQL + " JOIN  " + Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + " R ";
                Mi_SQL = Mi_SQL + " ON G." + Cat_Pre_Grupos.Campo_Rama_ID + " = R." + Cat_Pre_Ramas.Campo_Rama_ID;
                Mi_SQL = Mi_SQL + " WHERE C." + Cat_Pre_Claves_Ingreso.Campo_Clave + " LIKE '%" + Clave.P_Clave + "%'";
                Mi_SQL = Mi_SQL + " ORDER BY C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        } // Para la busqueda

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Claves_Ingreso
        ///DESCRIPCIÓN: Obtiene todas las Claves de Ingreso que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Llenar_Tabla_Claves_Ingreso()
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso.Campo_Clave;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", CONCAT(R." + Cat_Pre_Ramas.Campo_Clave;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , R." + Cat_Pre_Grupos.Campo_Nombre + "))";
                Mi_SQL = Mi_SQL + " AS NOMBRE_RAMA";
                Mi_SQL = Mi_SQL + ", R." + Cat_Pre_Ramas.Campo_Rama_ID;
                Mi_SQL = Mi_SQL + ", R." + Cat_Pre_Ramas.Campo_Nombre + " AS RAMA";
                Mi_SQL = Mi_SQL + ", CONCAT(G." + Cat_Pre_Grupos.Campo_Clave;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , G." + Cat_Pre_Grupos.Campo_Nombre + "))";
                Mi_SQL = Mi_SQL + " AS NOMBRE_GRUPO";
                Mi_SQL = Mi_SQL + ", G." + Cat_Pre_Grupos.Campo_Grupo_ID;
                Mi_SQL = Mi_SQL + ", G." + Cat_Pre_Grupos.Campo_Nombre + " AS GRUPO";
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso.Campo_Fundamento;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso.Campo_Cuenta_Contable_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " C ";
                Mi_SQL = Mi_SQL + " JOIN  " + Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + " G ";
                Mi_SQL = Mi_SQL + " ON C." + Cat_Pre_Claves_Ingreso.Campo_Grupo_ID + " = G." + Cat_Pre_Grupos.Campo_Grupo_ID;
                Mi_SQL = Mi_SQL + " JOIN  " + Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + " R ";
                Mi_SQL = Mi_SQL + " ON G." + Cat_Pre_Grupos.Campo_Rama_ID + " = R." + Cat_Pre_Ramas.Campo_Rama_ID;
                Mi_SQL = Mi_SQL + " ORDER BY C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Movimientos
        ///DESCRIPCIÓN: Obtiene todas los Movimientos que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///             1. Clave. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el Id de Clave de Ingreso para consultar los detalles que le pertenecen
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Llenar_Tabla_Movimientos(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave) //Llenar la tabla de grupos de movimientos
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT"; 
                Mi_SQL = Mi_SQL + " C." + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso_Det.Campo_Movimiento_ID + " AS CLAVE";
                Mi_SQL = Mi_SQL + ", M." + Cat_Pre_Movimientos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", CONCAT(M." + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , M." + Cat_Pre_Movimientos.Campo_Descripcion + "))";
                Mi_SQL = Mi_SQL + " AS CLAVE_MOVIMIENTO ";
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                Mi_SQL = Mi_SQL + " M JOIN " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " C ON M." + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = C." + Cat_Pre_Claves_Ingreso_Det.Campo_Movimiento_ID;
                Mi_SQL = Mi_SQL + " WHERE C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " = " + "'" + Clave.P_Clave_Ingreso_ID +"'"; 
                Mi_SQL = Mi_SQL + " ORDER BY C." + Cat_Pre_Claves_Ingreso_Det.Campo_Movimiento_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Movimientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Otros_Pagos
        ///DESCRIPCIÓN: Obtiene todas los Otros Pagos que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///             1. Clave. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el Id de Clave de Ingreso para consultar los detalles que le pertenecen
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Llenar_Tabla_Otros_Pagos(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave) //Llenar la tabla de claves de otros pagos
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT"; 
                Mi_SQL = Mi_SQL + " C." + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso_Det.Campo_Pago_ID + " AS CLAVE";
                Mi_SQL = Mi_SQL + ", O." + Cat_Pre_Otros_Pagos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", CONCAT(O." + Cat_Pre_Otros_Pagos.Campo_Pago_ID;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , O." + Cat_Pre_Otros_Pagos.Campo_Descripcion + "))";
                Mi_SQL = Mi_SQL + " AS CLAVE_OTRO_PAGO";
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Otros_Pagos.Tabla_Cat_Pre_Otros_Pagos;
                Mi_SQL = Mi_SQL + " O JOIN " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " C ON O." + Cat_Pre_Otros_Pagos.Campo_Pago_ID + " = C." + Cat_Pre_Claves_Ingreso_Det.Campo_Pago_ID;
                Mi_SQL = Mi_SQL + " WHERE C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " = " + "'" + Clave.P_Clave_Ingreso_ID + "'"; 
                Mi_SQL = Mi_SQL + " ORDER BY C." + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Otros Pagos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Documentos
        ///DESCRIPCIÓN: Obtiene todas los Documentos que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///             1. Clave. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el Id de Clave de Ingreso para consultar los detalles que le pertenecen
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Llenar_Tabla_Documentos(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave) //Llenar la tabla de claves de documentos
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT";
                Mi_SQL = Mi_SQL + " C." + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso_Det.Campo_Documento_ID + " AS CLAVE";
                Mi_SQL = Mi_SQL + ", D." + Cat_Pre_Tipos_Constancias.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", CONCAT(D." + Cat_Pre_Tipos_Constancias.Campo_Clave;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , D." + Cat_Pre_Tipos_Constancias.Campo_Descripcion + "))";
                Mi_SQL = Mi_SQL + " AS CLAVE_DOCUMENTO";
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias;
                Mi_SQL = Mi_SQL + " D JOIN " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " C ON D." + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + " = C." + Cat_Pre_Claves_Ingreso_Det.Campo_Documento_ID;
                Mi_SQL = Mi_SQL + " WHERE C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " = " + "'" + Clave.P_Clave_Ingreso_ID + "'"; 
                Mi_SQL = Mi_SQL + " ORDER BY C." + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Constancias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Gastos_Ejecucion
        ///DESCRIPCIÓN: Obtiene todas los Gastos de Ejecucion que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///             1. Clave. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el Id de Clave de Ingreso para consultar los detalles que le pertenecen
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Llenar_Tabla_Gastos_Ejecucion(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave) //Llenar la tabla de gastos de ejecucion
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT" ;
                Mi_SQL = Mi_SQL + " C." + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso_Det.Campo_Gasto_ID + " AS GASTO_EJECUCION_ID";
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Claves_Ingreso_Det.Campo_Gasto_ID + " AS CLAVE";
                Mi_SQL = Mi_SQL + ", G." + Cat_Pre_Gastos_Ejecucion.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", CONCAT(G." + Cat_Pre_Gastos_Ejecucion.Campo_Clave;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , G." + Cat_Pre_Gastos_Ejecucion.Campo_Nombre + "))";
                Mi_SQL = Mi_SQL + " AS CLAVE_GASTOS";
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Gastos_Ejecucion.Tabla_Cat_Pre_Gastos_Ejecucion;
                Mi_SQL = Mi_SQL + " G JOIN " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " C ON G." + Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID + " = C." + Cat_Pre_Claves_Ingreso_Det.Campo_Gasto_ID;
                Mi_SQL = Mi_SQL + " WHERE C." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " = " + "'" + Clave.P_Clave_Ingreso_ID + "'"; 
                Mi_SQL = Mi_SQL + " ORDER BY C." + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Ramas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Predial_Traslado
        ///DESCRIPCIÓN: Obtiene los campos de Tipos-Traslado que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///             1. Clave. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el Id de Clave de Ingreso para consultar los detalles que le pertenecen
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Llenar_Tabla_Predial_Traslado(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave) //Llenar la tabla de gastos de ejecucion
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " +  Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID ;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo + " AS TIPO";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo_Predial_Traslado + " AS TIPO_PREDIAL_TRASLADO";
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " = " + "'" + Clave.P_Clave_Ingreso_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo + " != ' '"    ;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Ramas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        #endregion

        #region Llenar Combos 

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Grupos
        ///DESCRIPCIÓN: Llena el combo de Grupos con los Grupos existentes en la base de datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Grupos() //Llenar el combo de grupos
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT CONCAT(" + Cat_Pre_Grupos.Campo_Grupo_ID;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , " + Cat_Pre_Grupos.Campo_Nombre + "))";
                Mi_SQL = Mi_SQL + " AS NOMBRE ";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Grupo_ID;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Grupos.Campo_Grupo_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Grupos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }   

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Grupos
        ///DESCRIPCIÓN: Llena el combo de Grupos con los Grupos existentes en la base de datos
        ///PARAMETROS:
        ///             1. Grupo. Instancia de la Clase de Claves de Ingreso  con 
        ///                       el Id de Clave para consultar los Grupos
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Grupos(Cls_Cat_Pre_Claves_Ingreso_Negocio Grupo) //Llenar el combo de grupos
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT CONCAT(" + Cat_Pre_Grupos.Campo_Clave;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , " + Cat_Pre_Grupos.Campo_Nombre + "))";
                Mi_SQL = Mi_SQL + " AS NOMBRE ";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Grupos.Campo_Grupo_ID;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Grupos.Campo_Rama_ID + " = " + "'" + Grupo.P_Rama_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Grupos.Campo_Grupo_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Grupos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Ramas
        ///DESCRIPCIÓN: Llena el combo de Ramas con las ranas existentes en la base de datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Ramas() //Llenar el combo de ramas
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT CONCAT(" + Cat_Pre_Ramas.Campo_Clave;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , " + Cat_Pre_Ramas.Campo_Nombre + "))";
                Mi_SQL = Mi_SQL + " AS NOMBRE ";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Ramas.Campo_Rama_ID;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Ramas.Campo_Rama_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Ramas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Movimientos
        ///DESCRIPCIÓN: Llena el combo de Movimientos con los movimientos existentes en la base de datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Movimientos() //Llenar el combo de grupos de movimientos
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT CONCAT(" + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , " + Cat_Pre_Movimientos.Campo_Descripcion + "))";
                Mi_SQL = Mi_SQL + " AS DESCRIPCION ";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Movimientos.Campo_Movimiento_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Movimientos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Otros_Pagos
        ///DESCRIPCIÓN: Llena el combo de Otros Pagos con los otros pagos existentes en la base de datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Otros_Pagos() //Llenar el combo de claves de otros pagos
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT CONCAT(" + Cat_Pre_Otros_Pagos.Campo_Pago_ID;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , " + Cat_Pre_Otros_Pagos.Campo_Descripcion + "))";
                Mi_SQL = Mi_SQL + " AS DESCRIPCION ";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Otros_Pagos.Campo_Pago_ID;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Otros_Pagos.Tabla_Cat_Pre_Otros_Pagos;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Otros_Pagos.Campo_Pago_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Ramas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Documentos
        ///DESCRIPCIÓN: Llena el combo de Tipos de Documentos con los tipos de documentos 
        ///             existentes en la base de datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Documentos() //Llenar el combo de claves de documentos
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT CONCAT(" + Cat_Pre_Tipos_Constancias.Campo_Clave;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , " + Cat_Pre_Tipos_Constancias.Campo_Descripcion + "))";
                Mi_SQL = Mi_SQL + " AS DESCRIPCION ";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Ramas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Gastos_Ejecucion
        ///DESCRIPCIÓN: Llena el combo de Gastos de Ejecucion con los gastos de ejecucion 
        ///             existentes en la base de datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Gastos_Ejecucion() //Llenar el combo de gastos de ejecucion
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT CONCAT(" + Cat_Pre_Gastos_Ejecucion.Campo_Clave;
                Mi_SQL = Mi_SQL + ", CONCAT(" + " ' - ' , " + Cat_Pre_Gastos_Ejecucion.Campo_Nombre + "))";
                Mi_SQL = Mi_SQL + " AS NOMBRE ";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Gastos_Ejecucion.Tabla_Cat_Pre_Gastos_Ejecucion;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Ramas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Gastos_Ejecucion
        ///DESCRIPCIÓN: Llena el combo de Gastos de Ejecucion con los gastos de ejecucion 
        ///             existentes en la base de datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cuentas_Contables() //Llenar el combo de cuentas contables
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Con_Cuentas_Contables.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Con_Cuentas_Contables.Campo_Descripcion;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cuentas Contables. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Gastos_Ejecucion
        ///DESCRIPCIÓN: Llena el combo de Gastos de Ejecucion con los gastos de ejecucion 
        ///             existentes en la base de datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Unidad_Responsable() //Llenar el combo de unidad responsable
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Dependencias.Campo_Nombre;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Dependencias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Costos_Claves
        ///DESCRIPCIÓN: Llena el grid de costos de claves
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Costos_Claves( Cls_Cat_Pre_Claves_Ingreso_Negocio Clave_Ingreso) //Llenar el combo de claves de documentos
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT "+Cat_Pre_Claves_Ing_Costos.Campo_Costo_Clave_ID+", ";
                Mi_SQL += Cat_Pre_Claves_Ing_Costos.Campo_Anio + ", ";
                Mi_SQL += Cat_Pre_Claves_Ing_Costos.Campo_Costo;
                Mi_SQL += " FROM  " + Cat_Pre_Claves_Ing_Costos.Tabla_Cat_Pre_Claves_Ing_Costos;
                Mi_SQL += " WHERE " + Cat_Pre_Claves_Ing_Costos.Campo_Clave_Ingreso_ID + "='" + Clave_Ingreso.P_Clave_Ingreso_ID + "'";
                if (Clave_Ingreso.P_Anio != null && Clave_Ingreso.P_Anio != "")
                {
                    Mi_SQL += " AND " + Cat_Pre_Claves_Ing_Costos.Campo_Anio + "=" + Clave_Ingreso.P_Anio;
                }
                Mi_SQL += " ORDER BY " + Cat_Pre_Claves_Ing_Costos.Campo_Anio;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Ramas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
        

        #endregion

        #region Modificar

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Clave_Ingreso
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Clave de Ingreso
        ///PARAMETROS:     
        ///             1. Clave. Instancia de la Clase de Claves de Ingreso  con 
        ///                       los datos de la Clave de Ingreso que va a ser Actualizada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Clave_Ingreso(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
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
                String Mi_SQL = "UPDATE " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " SET ";
                Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso.Campo_Estatus + " = '" + Clave.P_Estatus + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso.Campo_Grupo_ID + " = '" + Clave.P_Grupo_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso.Campo_Clave + " = '" + Clave.P_Clave + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso.Campo_Cuenta_Contable_ID + " = '" + Clave.P_Cuenta_Contable_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID + " = '" + Clave.P_Dependencia_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso.Campo_Fundamento + " = '" + Clave.P_Fundamento + "', ";
                Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " = '" + Clave.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso.Campo_Usuario_Modifico + " = '" + Clave.P_Usuario + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Claves_Ingreso.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " = '" + Clave.P_Clave_Ingreso_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Colonia
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Colonia
        ///PARAMETROS:     
        ///             1. Colonia. Instancia de la Clase de Colonias  con 
        ///                         los datos de la Colonia que va a ser Actualizada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        //public static void Modificar_Movimiento(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
        //{
        //    String Mensaje = "";
        //    OracleConnection Cn = new OracleConnection();
        //    OracleCommand Cmd = new OracleCommand();
        //    OracleTransaction Trans;
        //    Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
        //    Cn.Open();
        //    Trans = Cn.BeginTransaction();
        //    Cmd.Connection = Cn;
        //    Cmd.Transaction = Trans;
        //    try
        //    {
        //        String Mi_SQL = "UPDATE " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det + " SET ";
        //        Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso_Det.Campo_Movimiento_ID + " = '" + Clave.P_Movimiento_ID + "'";
        //        Mi_SQL = Mi_SQL + "," + Cat_Pre_Claves_Ingreso_Det.Campo_Usuario_Modifico + " = '" + Clave.P_Usuario + "'";
        //        Mi_SQL = Mi_SQL + "," + Cat_Pre_Claves_Ingreso_Det.Campo_Fecha_Modifico + " = SYSDATE";
        //        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID + " = '" + Clave.P_Detalle_ID + "'";
        //        Cmd.CommandText = Mi_SQL;
        //        Cmd.ExecuteNonQuery();
        //        Trans.Commit();
        //    }
        //    catch (OracleException Ex)
        //    {
        //        Trans.Rollback();
        //        //variable para el mensaje 
        //        //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
        //        if (Ex.Code == 8152)
        //        {
        //            Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //        }
        //        else if (Ex.Code == 2627)
        //        {
        //            if (Ex.Message.IndexOf("PRIMARY") != -1)
        //            {
        //                Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //            }
        //            else if (Ex.Message.IndexOf("UNIQUE") != -1)
        //            {
        //                Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
        //            }
        //            else
        //            {
        //                Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
        //            }
        //        }
        //        else if (Ex.Code == 547)
        //        {
        //            Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
        //        }
        //        else if (Ex.Code == 515)
        //        {
        //            Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //        }
        //        else
        //        {
        //            Mensaje = "Error al intentar modificar un Registro de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
        //        }
        //        //Indicamos el mensaje 
        //        throw new Exception(Mensaje);
        //    }
        //    finally
        //    {
        //        Cn.Close();
        //    }
        //}

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Colonia
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Colonia
        ///PARAMETROS:     
        ///             1. Colonia. Instancia de la Clase de Colonias  con 
        ///                         los datos de la Colonia que va a ser Actualizada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        //public static void Modificar_Otro_Pago(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
        //{
        //    String Mensaje = "";
        //    OracleConnection Cn = new OracleConnection();
        //    OracleCommand Cmd = new OracleCommand();
        //    OracleTransaction Trans;
        //    Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
        //    Cn.Open();
        //    Trans = Cn.BeginTransaction();
        //    Cmd.Connection = Cn;
        //    Cmd.Transaction = Trans;
        //    try
        //    {
        //        String Mi_SQL = "UPDATE " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det + " SET ";
        //        Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso_Det.Campo_Pago_ID + " = '" + Clave.P_Pago_ID + "'";
        //        Mi_SQL = Mi_SQL + "," + Cat_Pre_Claves_Ingreso_Det.Campo_Usuario_Modifico + " = '" + Clave.P_Usuario + "'";
        //        Mi_SQL = Mi_SQL + "," + Cat_Pre_Claves_Ingreso_Det.Campo_Fecha_Modifico + " = SYSDATE";
        //        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID + " = '" + Clave.P_Detalle_ID + "'";
        //        Cmd.CommandText = Mi_SQL;
        //        Cmd.ExecuteNonQuery();
        //        Trans.Commit();
        //    }
        //    catch (OracleException Ex)
        //    {
        //        Trans.Rollback();
        //        //variable para el mensaje 
        //        //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
        //        if (Ex.Code == 8152)
        //        {
        //            Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //        }
        //        else if (Ex.Code == 2627)
        //        {
        //            if (Ex.Message.IndexOf("PRIMARY") != -1)
        //            {
        //                Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //            }
        //            else if (Ex.Message.IndexOf("UNIQUE") != -1)
        //            {
        //                Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
        //            }
        //            else
        //            {
        //                Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
        //            }
        //        }
        //        else if (Ex.Code == 547)
        //        {
        //            Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
        //        }
        //        else if (Ex.Code == 515)
        //        {
        //            Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //        }
        //        else
        //        {
        //            Mensaje = "Error al intentar modificar un Registro de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
        //        }
        //        //Indicamos el mensaje 
        //        throw new Exception(Mensaje);
        //    }
        //    finally
        //    {
        //        Cn.Close();
        //    }
        //}

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Colonia
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Colonia
        ///PARAMETROS:     
        ///             1. Colonia. Instancia de la Clase de Colonias  con 
        ///                         los datos de la Colonia que va a ser Actualizada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        //public static void Modificar_Documento(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
        //{
        //    String Mensaje = "";
        //    OracleConnection Cn = new OracleConnection();
        //    OracleCommand Cmd = new OracleCommand();
        //    OracleTransaction Trans;
        //    Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
        //    Cn.Open();
        //    Trans = Cn.BeginTransaction();
        //    Cmd.Connection = Cn;
        //    Cmd.Transaction = Trans;
        //    try
        //    {
        //        String Mi_SQL = "UPDATE " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det + " SET ";
        //        Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso_Det.Campo_Documento_ID + " = '" + Clave.P_Documento_ID + "'";
        //        Mi_SQL = Mi_SQL + "," + Cat_Pre_Claves_Ingreso_Det.Campo_Usuario_Modifico + " = '" + Clave.P_Usuario + "'";
        //        Mi_SQL = Mi_SQL + "," + Cat_Pre_Claves_Ingreso_Det.Campo_Fecha_Modifico + " = SYSDATE";
        //        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID + " = '" + Clave.P_Detalle_ID + "'";
        //        Cmd.CommandText = Mi_SQL;
        //        Cmd.ExecuteNonQuery();
        //        Trans.Commit();
        //    }
        //    catch (OracleException Ex)
        //    {
        //        Trans.Rollback();
        //        //variable para el mensaje 
        //        //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
        //        if (Ex.Code == 8152)
        //        {
        //            Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //        }
        //        else if (Ex.Code == 2627)
        //        {
        //            if (Ex.Message.IndexOf("PRIMARY") != -1)
        //            {
        //                Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //            }
        //            else if (Ex.Message.IndexOf("UNIQUE") != -1)
        //            {
        //                Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
        //            }
        //            else
        //            {
        //                Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
        //            }
        //        }
        //        else if (Ex.Code == 547)
        //        {
        //            Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
        //        }
        //        else if (Ex.Code == 515)
        //        {
        //            Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //        }
        //        else
        //        {
        //            Mensaje = "Error al intentar modificar un Registro de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
        //        }
        //        //Indicamos el mensaje 
        //        throw new Exception(Mensaje);
        //    }
        //    finally
        //    {
        //        Cn.Close();
        //    }
        //}

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Colonia
        ///DESCRIPCIÓN: Actualiza en la Base de Datos una Colonia
        ///PARAMETROS:     
        ///             1. Colonia. Instancia de la Clase de Colonias  con 
        ///                         los datos de la Colonia que va a ser Actualizada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        //public static void Modificar_Gasto(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
        //{
        //    String Mensaje = "";
        //    OracleConnection Cn = new OracleConnection();
        //    OracleCommand Cmd = new OracleCommand();
        //    OracleTransaction Trans;
        //    Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
        //    Cn.Open();
        //    Trans = Cn.BeginTransaction();
        //    Cmd.Connection = Cn;
        //    Cmd.Transaction = Trans;
        //    try
        //    {
        //        String Mi_SQL = "UPDATE " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det + " SET ";
        //        Mi_SQL = Mi_SQL + Cat_Pre_Claves_Ingreso_Det.Campo_Gasto_ID + " = '" + Clave.P_Gasto_ID + "'";
        //        Mi_SQL = Mi_SQL + "," + Cat_Pre_Claves_Ingreso_Det.Campo_Usuario_Modifico + " = '" + Clave.P_Usuario + "'";
        //        Mi_SQL = Mi_SQL + "," + Cat_Pre_Claves_Ingreso_Det.Campo_Fecha_Modifico + " = SYSDATE";
        //        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID + " = '" + Clave.P_Detalle_ID + "'";
        //        Cmd.CommandText = Mi_SQL;
        //        Cmd.ExecuteNonQuery();
        //        Trans.Commit();
        //    }
        //    catch (OracleException Ex)
        //    {
        //        Trans.Rollback();
        //        //variable para el mensaje 
        //        //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
        //        if (Ex.Code == 8152)
        //        {
        //            Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //        }
        //        else if (Ex.Code == 2627)
        //        {
        //            if (Ex.Message.IndexOf("PRIMARY") != -1)
        //            {
        //                Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //            }
        //            else if (Ex.Message.IndexOf("UNIQUE") != -1)
        //            {
        //                Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
        //            }
        //            else
        //            {
        //                Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
        //            }
        //        }
        //        else if (Ex.Code == 547)
        //        {
        //            Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
        //        }
        //        else if (Ex.Code == 515)
        //        {
        //            Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
        //        }
        //        else
        //        {
        //            Mensaje = "Error al intentar modificar un Registro de Detalles de Claves de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
        //        }
        //        //Indicamos el mensaje 
        //        throw new Exception(Mensaje);
        //    }
        //    finally
        //    {
        //        Cn.Close();
        //    }
        //}

        #endregion

        #region Eliminar

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Costo_Clave
        ///DESCRIPCIÓN: Elimina un Costo de Clave
        ///PARAMETROS:   
        ///             1. Costo_Clave.   Costo de Clave que se va eliminar.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 18/Noviembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Costo_Clave(Cls_Cat_Pre_Claves_Ingreso_Negocio Costo_Clave)
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
                String Mi_SQL = "DELETE ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ing_Costos.Tabla_Cat_Pre_Claves_Ing_Costos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ing_Costos.Campo_Costo_Clave_ID;
                Mi_SQL = Mi_SQL + " = '" + Costo_Clave.P_Costo_Clave_ID + "'";
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
                    Mensaje = "Error al intentar eliminar el registro de Costos de Claves. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Costos de Claves. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Clave_Ingreso
        ///DESCRIPCIÓN: Elimina una Clave de Ingreso
        ///PARAMETROS:   
        ///             1. Clave.   Clave de Ingreso que se va eliminar.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 23/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Clave_Ingreso(Cls_Cat_Pre_Claves_Ingreso_Negocio Clave)
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
                String Mi_SQL = "DELETE FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " = '" + Clave.P_Clave_Ingreso_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();


                Mi_SQL = " DELETE FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + " = '" + Clave.P_Clave_Ingreso_ID + "'";
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
                    Mensaje = "Error al intentar modificar un Registro de Claves de Ingreso. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }

        }

        public static void Eliminar_Detalle(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
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
                String Mi_SQL = "DELETE ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Detalle_ID;
                Mi_SQL = Mi_SQL + " = '" + Detalle.P_Detalle_ID + "'";
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
                    Mensaje = "Error al intentar eliminar el registro de Detalles. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Detalles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        public static void Eliminar_Detalle_Otro_Pago(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
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
                String Mi_SQL = "DELETE ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " = '" + Detalle.P_Clave_Ingreso_ID ;
                Mi_SQL = Mi_SQL + "' AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Pago_ID + " = '" + Detalle.P_Pago_ID;
                Mi_SQL = Mi_SQL + "'";
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
                    Mensaje = "Error al intentar eliminar el registro de Detalles. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Detalles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        public static void Eliminar_Detalle_Movimiento(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
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
                String Mi_SQL = "DELETE ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " = '" + Detalle.P_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + "' AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Movimiento_ID + " = '" + Detalle.P_Movimiento_ID;
                Mi_SQL = Mi_SQL + "'";
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
                    Mensaje = "Error al intentar eliminar el registro de Detalles. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Detalles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        public static void Eliminar_Detalle_Documento(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
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
                String Mi_SQL = "DELETE ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " = '" + Detalle.P_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + "' AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Documento_ID + " = '" + Detalle.P_Documento_ID;
                Mi_SQL = Mi_SQL + "'";
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
                    Mensaje = "Error al intentar eliminar el registro de Detalles. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Detalles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        public static void Eliminar_Detalle_Gasto(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
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
                String Mi_SQL = "DELETE ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " = '" + Detalle.P_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + "' AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Gasto_ID + " = '" + Detalle.P_Gasto_ID;
                Mi_SQL = Mi_SQL + "'";
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
                    Mensaje = "Error al intentar eliminar el registro de Detalles. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Detalles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

        public static void Eliminar_Detalle_Predial_Traslado(Cls_Cat_Pre_Claves_Ingreso_Negocio Detalle)
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
                String Mi_SQL = "DELETE ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Claves_Ingreso_Det.Tabla_Cat_Pre_Claves_Igreso_Det;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID + " = '" + Detalle.P_Clave_Ingreso_ID;
                Mi_SQL = Mi_SQL + "' AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo + " = '" + Detalle.P_Tipo;
                Mi_SQL = Mi_SQL + "' AND " + Cat_Pre_Claves_Ingreso_Det.Campo_Tipo_Predial_Traslado + " = '" + Detalle.P_Tipo_Predial_Traslado;
                Mi_SQL = Mi_SQL + "'";
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
                    Mensaje = "Error al intentar eliminar el registro de Detalles. Error: [" + Ex.Message + "]";
                }
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Mensaje = "Error al intentar eliminar el registro de Detalles. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }


        #endregion

        #region ID Consecutivo

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///             1. Tabla: Tabla de referencia a la que se sacara el ultimo ID
        ///             2. Campo: Compo al que se le asignara la ultima ID
        ///             3. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 15/Julio/2011 
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
        ///FECHA_CREO: 15/Julio/2011 
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

        #endregion

    }
}