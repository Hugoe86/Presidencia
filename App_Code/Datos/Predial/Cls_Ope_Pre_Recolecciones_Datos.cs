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
using Presidencia.Operacion_Recolecciones.Negocio;

namespace Presidencia.Operacion_Recolecciones.Datos{
    
    public class Cls_Ope_Pre_Recolecciones_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Numeros_Caja
        ///DESCRIPCIÓN: Llena el combo de Numeros de Caja con las cajas existentes en la base de datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Numeros_Caja() //Llenar el combo de numeros de caja
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Cat_Pre_Cajas.Campo_Caja_Id;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Numero_De_Caja;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Cajas.Campo_Caja_Id;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Modulos
        ///DESCRIPCIÓN: Obtiene todos los tipos de colonias existentes en la base de datos
        ///PARAMETROS:
        ///             1. Caja.  Instancia de la Clase de Negocio de Recolecciones con los datos 
        ///                       necesarios para llenar el combo de Modulos.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataSet Consultar_Modulos(Cls_Ope_Pre_Recolecciones_Negocio Caja) //Llenar el combo de modulos
        {
            DataSet dataset;
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT M." + Cat_Pre_Modulos.Campo_Modulo_Id;
                Mi_SQL = Mi_SQL + ", M." + Cat_Pre_Modulos.Campo_Clave;
                Mi_SQL = Mi_SQL + ", M." + Cat_Pre_Modulos.Campo_Ubicacion;
                Mi_SQL = Mi_SQL + " FROM  " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL = Mi_SQL + " M JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " C ON M." + Cat_Pre_Cajas.Campo_Modulo_Id + " = C." + Cat_Pre_Modulos.Campo_Modulo_Id;
                Mi_SQL = Mi_SQL + " WHERE C." + Cat_Pre_Cajas.Campo_Caja_Id + " = '" + Caja.P_Caja_ID + "'"; 
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Modulos.Campo_Modulo_Id;
                dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Tipos de Colonias. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return dataset;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajeros
        ///DESCRIPCIÓN: Obtiene todos los Cajeros dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cajeros() //Llenar el combo de cajeros
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Empleado_ID + ", ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " E";
            Mi_SQL = Mi_SQL + " JOIN " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles + " R";
            Mi_SQL = Mi_SQL + " ON " + "R." + Apl_Cat_Roles.Campo_Rol_ID + " = ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Rol_ID;
            Mi_SQL = Mi_SQL + " ORDER BY " + "E." + Cat_Empleados.Campo_No_Empleado;
            try
            {
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Cajeros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajeros
        ///DESCRIPCIÓN: Obtiene todos los tipos de colonias existentes en la base de datos
        ///PARAMETROS:
        ///             1. Caja.  Instancia de la Clase de Negocio de Recolecciones con los datos 
        ///                       de la Caja que va a ser consultada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cajeros(Cls_Ope_Pre_Recolecciones_Negocio Caja) //Llenar el combo de cajeros
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Empleado_ID + ", ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " E";
            Mi_SQL = Mi_SQL + " JOIN " + Ope_Pre_Cajas_Detalles.Tabla_Ope_Pre_Cajas_Detalles;
            Mi_SQL = Mi_SQL + " C ON " + "C." + Ope_Pre_Cajas_Detalles.Campo_Empleado_ID + " = E." + Cat_Empleados.Campo_Empleado_ID;
            Mi_SQL = Mi_SQL + " WHERE C." + Ope_Pre_Cajas_Detalles.Campo_Caja_ID + " = '" + Caja.P_Caja_ID + "'";
            Mi_SQL = Mi_SQL + " ORDER BY " + "E." + Cat_Empleados.Campo_No_Empleado;
            try
            {
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los Cajeros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Recoleccion
        ///DESCRIPCIÓN: Da de alta en la Base de Datos una nueva Colonia
        ///PARAMETROS:     
        ///             1. Recolecion.  Instancia de la Clase de Negocio de Recolecciones con los datos 
        ///                             de la Recoleccion que va a ser dada de Alta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Alta_Recoleccion(Cls_Ope_Pre_Recolecciones_Negocio Recoleccion)
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
                String Recoleccion_ID = Obtener_ID_Consecutivo(Ope_Pre_Recolecciones.Tabla_Ope_Pre_Recolecciones, Ope_Pre_Recolecciones.Campo_Recoleccion_ID, 10);
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Recolecciones.Tabla_Ope_Pre_Recolecciones;
                Mi_SQL = Mi_SQL + " (" + Ope_Pre_Recolecciones.Campo_Recoleccion_ID + ", " + Ope_Pre_Recolecciones.Campo_Caja_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Recolecciones.Campo_Cajero_ID + ", " + Ope_Pre_Recolecciones.Campo_Mnt_Recoleccion;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Recolecciones.Campo_Num_Recoleccion + ", " + Ope_Pre_Recolecciones.Campo_Fecha;
                Mi_SQL = Mi_SQL + ", " + Ope_Pre_Recolecciones.Campo_Usuario_Creo + ", " + Ope_Pre_Recolecciones.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Recoleccion_ID + "', '" + Recoleccion.P_Caja_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Recoleccion.P_Cajero_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Recoleccion.P_Mnt_Recoleccion + "'";
                Mi_SQL = Mi_SQL + ",'" + Recoleccion.P_Num_Recoleccion + "'";
                Mi_SQL = Mi_SQL + ",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Recoleccion.P_Fecha)) + "'";
                Mi_SQL = Mi_SQL + ",'" + Recoleccion.P_Usuario + "'";
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
                    Mensaje = "Error al intentar dar de Alta una Recoleccion. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Recolecciones
        ///DESCRIPCIÓN: Obtiene todas las Recolecciones almacenadas en la base de datos.
        ///PARAMETROS:   
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Recolecciones() 
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT R." + Ope_Pre_Recolecciones.Campo_Recoleccion_ID;
                Mi_SQL = Mi_SQL + ", R." + Ope_Pre_Recolecciones.Campo_Num_Recoleccion;
                Mi_SQL = Mi_SQL + ", M." + Cat_Pre_Modulos.Campo_Clave + " AS MODULO";
                Mi_SQL = Mi_SQL + ", R." + Ope_Pre_Recolecciones.Campo_Caja_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS CAJA";
                Mi_SQL = Mi_SQL + ", E." + Cat_Empleados.Campo_Empleado_ID + " AS CAJERO_ID";
                Mi_SQL = Mi_SQL + ",E." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
                Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
                Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Materno + " AS CAJERO";
                Mi_SQL = Mi_SQL + ", R." + Ope_Pre_Recolecciones.Campo_Mnt_Recoleccion + " AS MONTO";
                Mi_SQL = Mi_SQL + ", R." + Ope_Pre_Recolecciones.Campo_Fecha;
                Mi_SQL = Mi_SQL + " FROM  " + Ope_Pre_Recolecciones.Tabla_Ope_Pre_Recolecciones;
                Mi_SQL = Mi_SQL + " R LEFT JOIN " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " E ON " + "R." + Ope_Pre_Recolecciones.Campo_Cajero_ID+ " = E." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " C ON " + "C." + Cat_Pre_Cajas.Campo_Caja_Id + " = R." + Ope_Pre_Recolecciones.Campo_Caja_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL = Mi_SQL + " M ON " + "M." + Cat_Pre_Modulos.Campo_Modulo_Id + " = C." + Cat_Pre_Cajas.Campo_Modulo_Id;
                Mi_SQL = Mi_SQL + " ORDER BY R." + Ope_Pre_Recolecciones.Campo_Recoleccion_ID;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Recolecciones_Busqueda
        ///DESCRIPCIÓN: Obtiene todas las Recolecciones que coincidan con la Busqueda ingresada.
        ///PARAMETROS:   
        ///             1. Caja.   Numero de la Caja que se va ver a Detalle.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Recolecciones_Busqueda(Cls_Ope_Pre_Recolecciones_Negocio Caja)
        {

            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT R." + Ope_Pre_Recolecciones.Campo_Recoleccion_ID;
                Mi_SQL = Mi_SQL + ", R." + Ope_Pre_Recolecciones.Campo_Num_Recoleccion;
                Mi_SQL = Mi_SQL + ", M." + Cat_Pre_Modulos.Campo_Clave + " AS MODULO";
                Mi_SQL = Mi_SQL + ", R." + Ope_Pre_Recolecciones.Campo_Caja_ID;
                Mi_SQL = Mi_SQL + ", C." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS CAJA";
                Mi_SQL = Mi_SQL + ", E." + Cat_Empleados.Campo_Empleado_ID + " AS CAJERO_ID";
                Mi_SQL = Mi_SQL + ",E." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
                Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
                Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Materno + " AS CAJERO";
                Mi_SQL = Mi_SQL + ", R." + Ope_Pre_Recolecciones.Campo_Mnt_Recoleccion + " AS MONTO";
                Mi_SQL = Mi_SQL + ", R." + Ope_Pre_Recolecciones.Campo_Fecha;
                Mi_SQL = Mi_SQL + " FROM  " + Ope_Pre_Recolecciones.Tabla_Ope_Pre_Recolecciones;
                Mi_SQL = Mi_SQL + " R LEFT JOIN " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " E ON " + "R." + Ope_Pre_Recolecciones.Campo_Cajero_ID + " = E." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
                Mi_SQL = Mi_SQL + " C ON " + "C." + Cat_Pre_Cajas.Campo_Caja_Id + " = R." + Ope_Pre_Recolecciones.Campo_Caja_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
                Mi_SQL = Mi_SQL + " M ON " + "M." + Cat_Pre_Modulos.Campo_Modulo_Id + " = C." + Cat_Pre_Cajas.Campo_Modulo_Id;
                Mi_SQL = Mi_SQL + " WHERE " + "C." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " LIKE '%" + Caja.P_Caja_ID + "%'";
                Mi_SQL = Mi_SQL + " ORDER BY R." + Ope_Pre_Recolecciones.Campo_Recoleccion_ID;
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
        ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Maxima
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo de una Clave disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:   
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 24/Julio/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_Clave_Maxima()
        {
            String Id = "";
            try
            {
                String Mi_SQL = "SELECT MAX(" + Ope_Pre_Recolecciones.Campo_Num_Recoleccion + ") FROM " + Ope_Pre_Recolecciones.Tabla_Ope_Pre_Recolecciones;
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
    }
}