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
using Presidencia.Catalogo_Cajeros.Negocio;

namespace Presidencia.Catalogo_Cajeros.Datos
{

    public class Cls_Cat_Pre_Cajeros_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Buscar_Cajeros
        ///DESCRIPCIÓN: Obtiene los datos del Cajero de acuerdo a la busqueda solicitada
        ///             por el usurio (Numero de Empleado).
        ///PARAMETROS:   
        ///             1. Cajero.   Datos para realizar la busqueda solicitada.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 07/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Buscar_Cajeros(Cls_Cat_Pre_Cajeros_Negocio Cajero)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT " + "E." + Cat_Empleados.Campo_No_Empleado + ", ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Empleado_ID + ", ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE_EMPLEADO" + ", ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Estatus + ", ";
            Mi_SQL = Mi_SQL + "R." + Apl_Cat_Roles.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " E";
            Mi_SQL = Mi_SQL + " JOIN " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles + " R";
            Mi_SQL = Mi_SQL + " ON " + "R." + Apl_Cat_Roles.Campo_Rol_ID + " = ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Rol_ID;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + Cajero.P_Busqueda + "'";
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
        ///DESCRIPCIÓN: Obtiene todas los Cajeros que estan dadas de alta en la Base de Datos
        ///PARAMETROS:      
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 04/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cajeros()
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT " + "E." + Cat_Empleados.Campo_No_Empleado + ", ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Empleado_ID + ", ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Materno+ " AS NOMBRE_EMPLEADO" + ", ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Estatus + ", ";
            Mi_SQL = Mi_SQL + "R." + Apl_Cat_Roles.Campo_Nombre; 
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cajas
        ///DESCRIPCIÓN: Obtiene el numero de Caja de la tabla de Cajas.
        ///PARAMETROS:   
        ///             1. Cajero.   Campos necesarios para realizar la consulta.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 05/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Cajas(Cls_Cat_Pre_Cajeros_Negocio Cajero)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT " + Cat_Pre_Cajas.Campo_Caja_Id;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cajas.Campo_Numero_De_Caja;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cajas.Campo_Modulo_Id;
            Mi_SQL = Mi_SQL + " = " + "'" + Cajero.P_Modulo_ID + "'";
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Cajas.Campo_Numero_De_Caja;
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
                String Mensaje = "Error al intentar consultar las Cajas Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Cajero
        ///DESCRIPCIÓN: Obtiene los datos del Cajero.
        ///PARAMETROS:   
        ///             1. P_Cajero_ID.   ID del Cajero que se consutara.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 04/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Pre_Cajeros_Negocio Consultar_Datos_Cajero(Cls_Cat_Pre_Cajeros_Negocio P_Cajero_ID)
        {
            String Mi_SQL = "SELECT " + "E." + Cat_Empleados.Campo_No_Empleado + ", ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Empleado_ID + ", ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Nombre + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE_EMPLEADO" + ", ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Estatus + ", ";
            Mi_SQL = Mi_SQL + "R." + Apl_Cat_Roles.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " E";
            Mi_SQL = Mi_SQL + " JOIN " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles + " R";
            Mi_SQL = Mi_SQL + " ON " + "R." + Apl_Cat_Roles.Campo_Rol_ID + " = ";
            Mi_SQL = Mi_SQL + "E." + Cat_Empleados.Campo_Rol_ID;
            Mi_SQL = Mi_SQL + " WHERE " + "E." + Cat_Empleados.Campo_No_Empleado + " = " + "'" + P_Cajero_ID.P_No_Empleado + "'";
            Mi_SQL = Mi_SQL + " ORDER BY " + "E." + Cat_Empleados.Campo_No_Empleado;
            OracleDataReader Data_Reader;
            try
            {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read())
                {
                    P_Cajero_ID.P_Empleado_ID = Data_Reader[Cat_Empleados.Campo_Empleado_ID].ToString();
                    P_Cajero_ID.P_Nombre = Data_Reader["NOMBRE_EMPLEADO"].ToString();
                    P_Cajero_ID.P_Tipo = Data_Reader[Apl_Cat_Roles.Campo_Nombre].ToString();
                    P_Cajero_ID.P_Estatus = Data_Reader[Cat_Empleados.Campo_Estatus].ToString();
                }
                Data_Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el Cajero Error: [" + Ex.Message + "]";
                throw new Exception(Mensaje);
            }
            return P_Cajero_ID;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Modulos
        ///DESCRIPCIÓN: Obtiene todos los nombres de los Modulos que estan dados de alta 
        ///             en la Base de Datos
        ///PARAMETROS:      
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 04/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Modulos()
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT " + Cat_Pre_Modulos.Campo_Modulo_Id;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Modulos.Campo_Ubicacion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo;
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Modulos.Campo_Modulo_Id;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Turnos
        ///DESCRIPCIÓN: Obtiene todos los nombres de los Turnos que estan dados de alta 
        ///             en la Base de Datos.
        ///PARAMETROS:      
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 04/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Turnos()
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "SELECT " + Cat_Pre_Turnos.Campo_Turno_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_Pre_Turnos.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Turnos.Tabla_Cat_Pre_Turnos;
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Turnos.Campo_Turno_ID;
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
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Cajas_Detalles
        ///DESCRIPCIÓN: Llena la tabla de cajas detalles.
        ///PARAMETROS:   
        ///             1. Cajero.   Campos necesarios para llenar la tabla de cajas detalles.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 06/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Llenar_Tabla_Cajas_Detalles(Cls_Cat_Pre_Cajeros_Negocio Cajero)
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
                for (int cont = 0; cont < Cajero.P_Cajas.Rows.Count; cont++)
                {
                    String Mi_SQL = "INSERT INTO " + Ope_Pre_Cajas_Detalles.Tabla_Ope_Pre_Cajas_Detalles;
                    Mi_SQL = Mi_SQL + " (" + Ope_Pre_Cajas_Detalles.Campo_Caja_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pre_Cajas_Detalles.Campo_Empleado_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pre_Cajas_Detalles.Campo_Turno_ID;
                    Mi_SQL = Mi_SQL + " ) VALUES (";
                    Mi_SQL = Mi_SQL + "'" + Cajero.P_Cajas.Rows[cont]["CAJA_ID"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Cajero.P_Cajas.Rows[cont]["EMPLEADO_ID"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Cajero.P_Cajas.Rows[cont]["TURNO_ID"].ToString().Trim() + "')";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                Mensaje = "Error al intentar dar de Atla un Detalle de Turno. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            finally
            {
                Cn.Close();
            }
        }

    }

}
