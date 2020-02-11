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
using System.Data.SqlClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Incidencias_Checadas.Negocios;

namespace Presidencia.Incidencias_Checadas.Datos
{
    public class Cls_Ope_Nom_Incidencias_Checadas_Datos
    {
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Checadas_Empleados_SQL
        /// DESCRIPCION : Consulta las checadas que tuvieron los empleados de presidencia
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 03-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Checadas_Empleados_SQL()
        {
            String IP_Servidor = "";                             //Varieble a contener la dirección IP del servidor de SQL
            String Nombre_Base_Datos = "";                       //Variable a contener el nombre de la base de datos en donde se estan guardando los registros de checadas de los empleados
            String Usuario_Base_Datos = "";                      //Variable a conterne el usuario para acceder a la base de datos de SQL
            String Password_Base_Datos = "";                     //Variable a contener la contraseña de la base de datos de SQL
            StringBuilder Mi_SQL= new StringBuilder();           //Varieble para la consulta de las asistencias
            DataTable Dt_Parametros;                             //Obtiene los datos de la consulta para la obtención de parámetros
            DataTable Dt_Incidencias_Checadas = new DataTable(); //Obtiene las checadas que tuvo el empleado

            try
            {
                //Consulta los parametros de conexión hacia la base de datos de SQL
                Mi_SQL.Append("SELECT NVL(" + Cat_Nom_Parametros.Campo_Nombre_Base_Datos + ", '') AS " + Cat_Nom_Parametros.Campo_Nombre_Base_Datos + ",");
                Mi_SQL.Append(" NVL(" + Cat_Nom_Parametros.Campo_IP_Servidor + ", '') AS " + Cat_Nom_Parametros.Campo_IP_Servidor + ",");
                Mi_SQL.Append(" NVL(" + Cat_Nom_Parametros.Campo_Usuario_SQL + ", '') AS " + Cat_Nom_Parametros.Campo_Usuario_SQL + ",");
                Mi_SQL.Append(" NVL(" + Cat_Nom_Parametros.Campo_Password_Base_Datos + ", '') AS " + Cat_Nom_Parametros.Campo_Password_Base_Datos);
                Mi_SQL.Append(" FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros);
                Dt_Parametros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                if (Dt_Parametros.Rows.Count > 0)
                {
                    //Agrega los valores de los campos a los controles correspondientes de la forma
                    foreach (DataRow Registro in Dt_Parametros.Rows)
                    {
                        IP_Servidor = Registro[Cat_Nom_Parametros.Campo_IP_Servidor].ToString();
                        Nombre_Base_Datos = Registro[Cat_Nom_Parametros.Campo_Nombre_Base_Datos].ToString();
                        Usuario_Base_Datos = Registro[Cat_Nom_Parametros.Campo_Usuario_SQL].ToString();
                        Password_Base_Datos = Registro[Cat_Nom_Parametros.Campo_Password_Base_Datos].ToString();
                    }
                }
                if (IP_Servidor != "" && Nombre_Base_Datos != "" && Usuario_Base_Datos != "" && Password_Base_Datos != "")
                {
                    String Conexion_SQL = "";
                    //Realiza la conexión a la base de datos de SQL
                    Conexion_SQL = "Data Source=" + IP_Servidor + "; Persist Security Info=True; Initial Catalog=" + 
                        Nombre_Base_Datos + ";User ID=" + Usuario_Base_Datos + ";Password=" + Password_Base_Datos;

                    Mi_SQL.Length = 0;
                    //Consulta todas las checadas que se tuvieron
                    Mi_SQL.Append("SELECT CHECKINOUT.CHECKTIME, CHECKINOUT.USERID, CHECKINOUT.SENSORID, USERINFO.BADGENUMBER FROM CHECKINOUT, USERINFO");
                    Mi_SQL.Append(" WHERE CHECKINOUT.USERID = USERINFO.USERID ORDER BY USERINFO.BADGENUMBER, CHECKINOUT.CHECKTIME");
                    DataTable Dt_Checadas = SqlHelper.ExecuteDataset(Conexion_SQL, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                    if(Dt_Checadas.Rows.Count>0)
                    {
                        DataTable Dt_Empleados = new DataTable(); //Obtiene el nombre y ID del empleado que se esta consultando
                        String Empleado_ID_Checada = ""; //Obtiene el ID del empleado de las checadas
                        String No_Empleado="";           //Obtiene el No_Empleado del que se esta consultando
                        String Empleado = "";            //Obtiene el nombre del empleado que se esta consultando
                        Object Reloj_ID = "";            //Obtiene el ID del Reloj Checador

                        //Se realiza la estructura a contener los datos
                        Dt_Incidencias_Checadas.Columns.Add("Empleado_ID", typeof(System.String));
                        Dt_Incidencias_Checadas.Columns.Add("Reloj_ID", typeof(System.String));
                        Dt_Incidencias_Checadas.Columns.Add("No_Empleado", typeof(System.String));
                        Dt_Incidencias_Checadas.Columns.Add("Empleado", typeof(System.String));
                        Dt_Incidencias_Checadas.Columns.Add("Clave", typeof(System.String));
                        Dt_Incidencias_Checadas.Columns.Add("Fecha_Hora_Checada", typeof(System.DateTime));
                        Dt_Incidencias_Checadas.Columns.Add("USERID", typeof(System.String));
                        DataRow Renglon;
                        //Obtiene la información de la consulta y la procesa para poder obtener la información tal cual se requiere para su visualización
                        foreach (DataRow Registro_Checada in Dt_Checadas.Rows)
                        {
                            if (No_Empleado != Registro_Checada["BADGENUMBER"].ToString())
                            {
                                No_Empleado = Registro_Checada["BADGENUMBER"].ToString();
                                
                                Mi_SQL.Length = 0;
                                Mi_SQL.Append("SELECT " + Cat_Empleados.Campo_Empleado_ID + ",");
                                Mi_SQL.Append(" (" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" + Cat_Empleados.Campo_Apellido_Materno);
                                Mi_SQL.Append("||' '||" + Cat_Empleados.Campo_Nombre + ") AS Empleado");
                                Mi_SQL.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                                Mi_SQL.Append(" WHERE (" + Cat_Empleados.Campo_No_Empleado + " = '" + No_Empleado + "'");
                                Mi_SQL.Append(" OR " + Cat_Empleados.Campo_No_Empleado + " = '" + String.Format("{0:0000000000}", Convert.ToInt32(No_Empleado)) + "'");
                                Mi_SQL.Append(" OR " + Cat_Empleados.Campo_No_Empleado + " = '" + String.Format("{0:000000}", Convert.ToInt32(No_Empleado)) + "')");
                                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                                if (Dt_Empleados is DataTable)
                                {
                                    foreach (DataRow Datos_Empleado in Dt_Empleados.Rows)
                                    {
                                        Empleado = Datos_Empleado["Empleado"].ToString();
                                        Empleado_ID_Checada = Datos_Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                                    }
                                }
                            }
                            //Consulta el ID del reloj Checador que pernece a la clave
                            if (!String.IsNullOrEmpty(Registro_Checada["SENSORID"].ToString()))
                            {
                                Mi_SQL.Length = 0;
                                Mi_SQL.Append("SELECT " + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID);
                                Mi_SQL.Append(" FROM " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador);
                                Mi_SQL.Append(" WHERE " + Cat_Nom_Reloj_Checador.Campo_Clave + " = '" + Registro_Checada["SENSORID"].ToString() + "'");
                                Reloj_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                            }

                            if ((Reloj_ID != null) && !String.IsNullOrEmpty(Empleado_ID_Checada) && !String.IsNullOrEmpty(No_Empleado) &&
                                !String.IsNullOrEmpty(Empleado))
                            {
                                //Agrega los datos encontrados en el DataTable para poder retornar los valores y poder ser visualizados por el empleado
                                Renglon = Dt_Incidencias_Checadas.NewRow();
                                Renglon["Empleado_ID"] = Empleado_ID_Checada.ToString();
                                Renglon["Reloj_ID"] = Reloj_ID.ToString();
                                Renglon["No_Empleado"] = No_Empleado.ToString();
                                Renglon["Empleado"] = Empleado.ToString();
                                Renglon["Clave"] = Registro_Checada["SENSORID"].ToString();
                                Renglon["Fecha_Hora_Checada"] = Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Registro_Checada["CHECKTIME"].ToString()));
                                Renglon["USERID"] = Registro_Checada["USERID"].ToString();
                                Dt_Incidencias_Checadas.Rows.Add(Renglon);
                            }
                        }
                    }                    
                }
                return Dt_Incidencias_Checadas;
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
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Incidencias_Checadas
        /// DESCRIPCION : 1.Consulta el último No dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta la checada del Empleado en la BD con los datos
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 05-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Incidencias_Checadas(Cls_Ope_Nom_Incidencias_Checadas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la cadena de inserción hacía la base de datos
            DataTable Dt_Incidencia = new DataTable();  //Obtiene el No con la cual se guardo los datos en la base de datos
            Object No_Incidencia_Checada;               //Obtiene el No con el cual se va a guardar el registro
            String IP_Servidor = "";                    //Varieble a contener la dirección IP del servidor de SQL
            String Nombre_Base_Datos = "";              //Variable a contener el nombre de la base de datos en donde se estan guardando los registros de checadas de los empleados
            String Usuario_Base_Datos = "";             //Variable a conterne el usuario para acceder a la base de datos de SQL
            String Password_Base_Datos = "";            //Variable a contener la contraseña de la base de datos de SQL

            DataTable Dt_Parametros; //Obtiene los datos de la consulta para la obtención de parámetros

            try
            {
                //Consulta los parametros de conexión hacia la base de datos de SQL
                Mi_SQL.Append("SELECT NVL(" + Cat_Nom_Parametros.Campo_Nombre_Base_Datos + ", '') AS " + Cat_Nom_Parametros.Campo_Nombre_Base_Datos + ",");
                Mi_SQL.Append(" NVL(" + Cat_Nom_Parametros.Campo_IP_Servidor + ", '') AS " + Cat_Nom_Parametros.Campo_IP_Servidor + ",");
                Mi_SQL.Append(" NVL(" +Cat_Nom_Parametros.Campo_Usuario_SQL + ",'') AS " + Cat_Nom_Parametros.Campo_Usuario_SQL + ",");
                Mi_SQL.Append(" NVL(" + Cat_Nom_Parametros.Campo_Password_Base_Datos + ", '') AS " + Cat_Nom_Parametros.Campo_Password_Base_Datos);
                Mi_SQL.Append(" FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros);
                Dt_Parametros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                if (Dt_Parametros.Rows.Count > 0)
                {
                    //Agrega los valores de los campos a los controles correspondientes de la forma
                    foreach (DataRow Registro in Dt_Parametros.Rows)
                    {
                        IP_Servidor = Registro[Cat_Nom_Parametros.Campo_IP_Servidor].ToString();
                        Nombre_Base_Datos = Registro[Cat_Nom_Parametros.Campo_Nombre_Base_Datos].ToString();
                        Usuario_Base_Datos = Registro[Cat_Nom_Parametros.Campo_Usuario_SQL].ToString();
                        Password_Base_Datos = Registro[Cat_Nom_Parametros.Campo_Password_Base_Datos].ToString();
                    }
                }
                String Conexion_SQL = ""; //Variable para la conxión a la base de datos de SQL
                //Realiza la conexión a la base de datos de SQL
                Conexion_SQL = "Data Source=" + IP_Servidor + "; Persist Security Info=True; Initial Catalog=" + Nombre_Base_Datos + ";User ID=" + Usuario_Base_Datos + ";Password=" + Password_Base_Datos;

                //Da de alta la checada del empleado en la base de datos
                foreach (DataRow Registro_Checada in Datos.P_Dt_Checadas.Rows)
                {
                    Mi_SQL.Length = 0;
                    //Consulta si el registro ya fue dado de alta con anticipación en la base de datos de ORACLE
                    Mi_SQL.Append("SELECT * FROM " + Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas);
                    Mi_SQL.Append(" WHERE " + Ope_Nom_Incidencias_Checadas.Campo_Empleado_ID + " = '" + Registro_Checada["Empleado_ID"].ToString() + "'");
                    Mi_SQL.Append(" AND " + Ope_Nom_Incidencias_Checadas.Campo_Reloj_Checador_ID + " = '" + Registro_Checada["Reloj_ID"].ToString() + "'");
                    Mi_SQL.Append(" AND " + Ope_Nom_Incidencias_Checadas.Campo_Fecha_Hora_Checada + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Convert.ToDateTime(Registro_Checada["Fecha_Hora_Checada"].ToString())) + "', 'DD/MM/YYYY HH24:MI:SS')");
                    Dt_Incidencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];                    

                    //Valida que no exista el registro ya dado de alta en la base de datos
                    if (Dt_Incidencia.Rows.Count < 1)
                    {
                        Mi_SQL.Length = 0;
                        //Consulta el último No de Asistencia que fue agregado a la base de datos
                        Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Nom_Incidencias_Checadas.Campo_No_Incidencia_Checada + "),'0000000000')");
                        Mi_SQL.Append(" FROM " + Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas);
                        Mi_SQL.Append(" WHERE " + Ope_Nom_Incidencias_Checadas.Campo_Empleado_ID + " = '" + Registro_Checada["Empleado_ID"].ToString() + "'");
                        No_Incidencia_Checada = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                        if (Convert.IsDBNull(No_Incidencia_Checada))
                        {
                            No_Incidencia_Checada = "0000000001";
                        }
                        else
                        {
                            No_Incidencia_Checada = String.Format("{0:0000000000}", Convert.ToInt32(No_Incidencia_Checada) + 1);
                        }
                        Mi_SQL.Length = 0;
                        //Consulta para la inserción de los datos
                        Mi_SQL.Append("INSERT INTO " + Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas);
                        Mi_SQL.Append(" (" + Ope_Nom_Incidencias_Checadas.Campo_No_Incidencia_Checada + ", " + Ope_Nom_Incidencias_Checadas.Campo_Empleado_ID + ", ");
                        Mi_SQL.Append(Ope_Nom_Incidencias_Checadas.Campo_Reloj_Checador_ID + ", " + Ope_Nom_Incidencias_Checadas.Campo_Fecha_Hora_Checada + ", ");
                        Mi_SQL.Append(Ope_Nom_Incidencias_Checadas.Campo_Usuario_Creo + ", " + Ope_Nom_Asistencias.Campo_Fecha_Creo + ")");
                        Mi_SQL.Append(" VALUES ('" + No_Incidencia_Checada.ToString() + "', '" + Registro_Checada["Empleado_ID"].ToString() + "',");
                        Mi_SQL.Append(" '" + Registro_Checada["Reloj_ID"].ToString() + "',");
                        Mi_SQL.Append(" TO_TIMESTAMP ('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Convert.ToDateTime(Registro_Checada["Fecha_Hora_Checada"].ToString())) + "', 'DD/MM/YYYY HH24: MI: SS'),");
                        Mi_SQL.Append(" '" + Datos.P_Nombre_Usuario + "', SYSDATE)");
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    }

                    ///////////////
                    Mi_SQL.Length = 0;//Se borran los registros de asistencias de para no guardar basura!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    //Elimina la falta que fue insertada en la base de datos
                    //Mi_SQL.Append("DELETE FROM CHECKINOUT WHERE SENSORID = '" + Registro_Checada["Clave"].ToString() + "'");
                    //Mi_SQL.Append(" AND CHECKTIME = '" + String.Format("{0:MM/dd/yyyy HH:mm:ss}", Convert.ToDateTime(Registro_Checada["Fecha_Hora_Checada"].ToString())) + "'");
                    //Mi_SQL.Append(" AND USERID = '" + Convert.ToInt16(Registro_Checada["USERID"].ToString()) + "'");
                    //SqlHelper.ExecuteNonQuery(Conexion_SQL, CommandType.Text, Mi_SQL.ToString());
                    ///////////////////
                }
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
    }
}
