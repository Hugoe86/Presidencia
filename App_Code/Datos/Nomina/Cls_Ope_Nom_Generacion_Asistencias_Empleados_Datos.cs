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
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Generacion_Asistencias.Negocio;
using Presidencia.DateDiff;

namespace Presidencia.Generacion_Asistencias.Datos
{
    public class Cls_Ope_Nom_Generacion_Asistencias_Empleados_Datos
    {
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Lista_Asistencias_Empleados
        /// DESCRIPCION : Consulta las checadas que tuvieron los empleados de presidencia
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 08-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Lista_Asistencias_Empleados(Cls_Ope_Nom_Generacion_Asistencias_Empleados_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();        //Variable que obtendra la consulta de los datos
            DataTable Dt_Empleados = new DataTable();          //Variable a contener la lista de los empleados que checan
            DataTable Dt_Incidencia = new DataTable();         //Obtiene los datos generales de la primer checada del empleado
            DataTable Dt_Salida = new DataTable();             //Obtiene la última checada del empleado durante el día
            DataTable Dt_Asistencias = new DataTable();        //Obtiene todas las asistencias de los empleados
            Double No_Incidencias = 0;                           //Obtiene el número total de checadas que se tienens
            String Empleado_ID = "";                           //Obtiene el ID del empleado que se esta consultando
            String No_Empleado = "";                           //Obtiene el No Empleado que se esta consultando
            String Empleado = "";                              //Obtiene el nombre del empleado que se esta consultando
            String Reloj_Checador_ID = "";                     //Obtiene el Id del reloj checador en donde el empleado registro su primera entrada
            String Clave = "";                                 //Obtiene la clave del reloj checador
            String Fecha_Hora_Entrada = "";                    //Obtiene la fecha y hora de entrada de checada del empleado
            String Fecha_Hora_Salida = "";                     //Obtiene la fecha y hora de salida de checada del empleado
            DateTime Fecha_Entrada;
            DateTime Fecha_Salida;
            Int16 Dias_Asistencias = 0;                        //Obtiene el número de días a consultar el registro de asistencias del empleado
            Int16 Dia = 0;                                     //Contador del número de días de asistencias

            try
            {
                Fecha_Entrada = Datos.P_Fecha_Hora_Entrada;
                Fecha_Salida = Datos.P_Fecha_Hora_Salida;

                //Consulta que haya registros en el periodo de fechas seleccionado por el usuario
                Mi_SQL.Append("SELECT COUNT(*) AS Registros FROM " + Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas);
                Mi_SQL.Append(" WHERE " + Ope_Nom_Incidencias_Checadas.Campo_Fecha_Hora_Checada);
                Mi_SQL.Append(" BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Datos.P_Fecha_Hora_Entrada) + "', 'DD/MM/YYYY HH24:MI:SS')");
                Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Datos.P_Fecha_Hora_Salida) + "', 'DD/MM/YYYY HH24:MI:SS')");
                Dt_Incidencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                foreach (DataRow Registro_Incidencias in Dt_Incidencia.Rows)
                {
                    No_Incidencias = Convert.ToDouble(Registro_Incidencias["Registros"].ToString());
                }
                Dt_Incidencia = new DataTable();
                if (No_Incidencias > 0)//Si se encontraron checadas durante el periodo indicando entonces realiza el proceso de consulta de obtención de asistencias
                {
                    //Se realiza la estructura a contener los datos
                    Dt_Asistencias.Columns.Add("EMPLEADO_ID", typeof(System.String));
                    Dt_Asistencias.Columns.Add("RELOJ_CHECADOR_ID", typeof(System.String));
                    Dt_Asistencias.Columns.Add("NO_EMPLEADO", typeof(System.String));
                    Dt_Asistencias.Columns.Add("EMPLEADO", typeof(System.String));
                    Dt_Asistencias.Columns.Add("CLAVE", typeof(System.String));
                    Dt_Asistencias.Columns.Add("FECHA_HORA_ENTRADA", typeof(System.DateTime));
                    Dt_Asistencias.Columns.Add("FECHA_HORA_SALIDA", typeof(System.DateTime));
                    DataRow Renglon;

                    Mi_SQL.Length = 0;
                    //Consulta los datos generales de empleado
                    Mi_SQL.Append("SELECT " + Cat_Empleados.Campo_Empleado_ID + ", " + Cat_Empleados.Campo_No_Empleado + ", ");
                    Mi_SQL.Append("(" + Cat_Empleados.Campo_Apellido_Paterno);
                    Mi_SQL.Append("||' '||" + Cat_Empleados.Campo_Apellido_Materno + "||' '||");
                    Mi_SQL.Append(Cat_Empleados.Campo_Nombre + ") AS EMPLEADO");
                    Mi_SQL.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                    Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Reloj_Checador + " = 'SI' ");
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + " = 'ACTIVO'");

                    //Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    Dt_Empleados = Datos.P_Dt_Empleados;//Linea agregada jahn

                    //Obtiene los empleados que checan dentro de presidencia y su horario de entrada y salida
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        //Agrega los valores de los campos a los controles correspondientes de la forma
                        foreach (DataRow Registro in Dt_Empleados.Rows)
                        {
                            Empleado_ID = Registro["Empleado_ID"].ToString();
                            No_Empleado = Registro["No_Empleado"].ToString();
                            Empleado = Registro["Empleado"].ToString();

                            Datos.P_Fecha_Hora_Entrada = Fecha_Entrada;
                            Datos.P_Fecha_Hora_Salida = Fecha_Salida;

                            Mi_SQL.Remove(0, Mi_SQL.Length);

                            //CONSULTA PARA OBTENER TODAS LAS CHECADAS DEL EMPLEADO ACTUAL.
                            Mi_SQL.Append("SELECT * FROM ");
                            Mi_SQL.Append(Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas);
                            Mi_SQL.Append(" WHERE " + Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas + "." + Ope_Nom_Incidencias_Checadas.Campo_Empleado_ID + " = '" + Empleado_ID + "'");
                            Mi_SQL.Append(" AND " + Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas + "." + Ope_Nom_Incidencias_Checadas.Campo_Fecha_Hora_Checada + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Hora_Entrada) + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                            Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Hora_Salida) + " 23:59:59', 'DD/MM/YYYY HH24:MI:SS')");

                            DataTable Dt_Aux_Checadas_Empleado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                            if (Dt_Aux_Checadas_Empleado is DataTable)
                            {

                                if (Dt_Aux_Checadas_Empleado.Rows.Count > 0)
                                {
                                    //Obtiene la fecha de inicio de la consulta y el número de días a consultar
                                    if (Dias_Asistencias == 0)
                                    {
                                        //OBTIENE LOS DÍAS DE LA CATORCENA MENOS 1 DÍA.
                                        Dias_Asistencias = Convert.ToInt16(Datos.P_Fecha_Hora_Salida.Subtract(Datos.P_Fecha_Hora_Entrada).Days.ToString());
                                    }
                                    else
                                    {
                                        //LE RESTA A LA FECHA FINAL DE LA CATORCENA 1 DÍA.
                                        Datos.P_Fecha_Hora_Entrada = Datos.P_Fecha_Hora_Entrada.AddDays(-Dias_Asistencias);
                                        //OBTIENE LOS DIAS QUE TIENE LA CATORCENA PERO ESTE MÉTODO DEVUELVE LA DIFF ENTRE LA FECHA INICIO Y FIN MENOS 1 DÍA.
                                        Dias_Asistencias = Convert.ToInt16(Datos.P_Fecha_Hora_Salida.Subtract(Datos.P_Fecha_Hora_Entrada).Days.ToString());//Falta sumar 1 día
                                    }

                                    //LE SUMA A LOS DÍAS DE ASISTENCIA 1 DÍA PORQUE LA INSTRUCCIÓN ANTERIOR DEVUELVE UN DÍA MENOS.
                                    Dias_Asistencias += 1;

                                    for (Dia = 1; Dia <= Dias_Asistencias; Dia++)
                                    {
                                        if (Dia != 1) Datos.P_Fecha_Hora_Entrada = Datos.P_Fecha_Hora_Entrada.AddDays(1);

                                        //LIMPIAMOS LA VARIABLE QUE ALMACENA LAS CONSULTAS.
                                        Mi_SQL.Remove(0, Mi_SQL.Length);

                                        //CONSULTA PARA OBTENER LA 1RA CHECADA DEL DÍA DEL EMPLEADO.
                                        Mi_SQL.Append("SELECT TO_CHAR(" + Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas + "." + Ope_Nom_Incidencias_Checadas.Campo_Fecha_Hora_Checada + ", 'DD-MON-YYYY HH24:MI:SS') AS Fecha_Hora_Checada, ");
                                        Mi_SQL.Append(Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID + ", ");
                                        Mi_SQL.Append(Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Clave);

                                        Mi_SQL.Append(" FROM " + Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas + " LEFT OUTER JOIN " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + " ON ");
                                        Mi_SQL.Append(Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas + "." + Ope_Nom_Incidencias_Checadas.Campo_Reloj_Checador_ID + "=");
                                        Mi_SQL.Append(Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID);

                                        Mi_SQL.Append(" WHERE " + Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas + "." + Ope_Nom_Incidencias_Checadas.Campo_Empleado_ID + " = '" + Registro["Empleado_ID"].ToString() + "'");
                                        Mi_SQL.Append(" AND " + Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas + "." + Ope_Nom_Incidencias_Checadas.Campo_Fecha_Hora_Checada + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Hora_Entrada) + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                                        Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Hora_Entrada) + " 23:59:59', 'DD/MM/YYYY HH24:MI:SS')");
                                        Mi_SQL.Append(" ORDER BY " + Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas + "." + Ope_Nom_Incidencias_Checadas.Campo_Fecha_Hora_Checada + " ASC");

                                        Dt_Incidencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                                        //Si se tiene checada del empleado entonces verifica que exista salida y sus datos generales de la entrada del empleado
                                        if (Dt_Incidencia.Rows.Count > 0)
                                        {
                                            foreach (DataRow Registro_Entrada in Dt_Incidencia.Rows)
                                            {
                                                Reloj_Checador_ID = Registro_Entrada[Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID].ToString();
                                                Clave = Registro_Entrada[Cat_Nom_Reloj_Checador.Campo_Clave].ToString();
                                                Fecha_Hora_Entrada = String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Registro_Entrada["Fecha_Hora_Checada"].ToString());
                                                break;
                                            }


                                            //LIMPIAMOS LA VARIABLE QUE ALMACENA LAS CONSULTAS.
                                            Mi_SQL.Remove(0, Mi_SQL.Length);

                                            //Consulta la última checada que tuvo el empleado durante el día
                                            Mi_SQL.Append("SELECT TO_CHAR(" + Ope_Nom_Incidencias_Checadas.Campo_Fecha_Hora_Checada + ", 'DD-MON-YYYY HH24:MI:SS') AS Fecha_Hora_Checada");
                                            Mi_SQL.Append(" FROM " + Ope_Nom_Incidencias_Checadas.Tabla_Ope_Nom_Incidencias_Checadas);
                                            Mi_SQL.Append(" WHERE " + Ope_Nom_Incidencias_Checadas.Campo_Empleado_ID + " = '" + Registro["Empleado_ID"].ToString() + "'");
                                            Mi_SQL.Append(" AND " + Ope_Nom_Incidencias_Checadas.Campo_Fecha_Hora_Checada + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Hora_Entrada) + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                                            Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Hora_Entrada) + " 23:59:59', 'DD/MM/YYYY HH24:MI:SS')");
                                            Mi_SQL.Append(" ORDER BY " + Ope_Nom_Incidencias_Checadas.Campo_Fecha_Hora_Checada + " DESC");

                                            Dt_Salida = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                                            if (Dt_Salida.Rows.Count > 0)
                                            {
                                                foreach (DataRow Registro_Salida in Dt_Salida.Rows)
                                                {
                                                    Fecha_Hora_Salida = String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Registro_Salida["Fecha_Hora_Checada"].ToString());
                                                    //Si la hora de salida del empleado es igual a la hora de entrada entonces el empleado no checo su salida
                                                    if (Fecha_Hora_Entrada.ToString() == Fecha_Hora_Salida.ToString()) Fecha_Hora_Salida = "";
                                                    break;
                                                }
                                            }


                                            //Agrega la asistencia del empleado
                                            Renglon = Dt_Asistencias.NewRow();
                                            Renglon["EMPLEADO_ID"] = Empleado_ID;
                                            Renglon["RELOJ_CHECADOR_ID"] = Reloj_Checador_ID;
                                            Renglon["NO_EMPLEADO"] = No_Empleado;
                                            Renglon["EMPLEADO"] = Empleado;
                                            Renglon["CLAVE"] = Clave;
                                            Renglon["FECHA_HORA_ENTRADA"] = Convert.ToDateTime(String.Format("{0:MMM/dd/yyyy HH:mm:ss}", Fecha_Hora_Entrada.ToString()));

                                            if (!String.IsNullOrEmpty(Fecha_Hora_Salida))
                                            {
                                                if (Fecha_Hora_Salida.ToString() != "") Renglon["FECHA_HORA_SALIDA"] = Convert.ToDateTime(String.Format("{0:MMM/dd/yyyy HH:mm:ss}", Fecha_Hora_Salida.ToString()));
                                            }

                                            Dt_Asistencias.Rows.Add(Renglon);
                                        }



                                    }
                                }
                            }
                        }
                    }
                }
                return Dt_Asistencias;
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
        /// NOMBRE DE LA FUNCION: Alta_Asistencias
        /// DESCRIPCION : 1.Consulta el último No dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta la asistencias del Empleado en la BD con los datos
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 08-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Asistencias(Cls_Ope_Nom_Generacion_Asistencias_Empleados_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la cadena de inserción hacía la base de datos
            DataTable Dt_Asistencia = new DataTable(); //Obtiene el No con la cual se guardo los datos en la base de datos
            Object No_Asistencia;                      //Obtiene el No con el cual se va a guardar el registro

            try
            {

                //Da de alta la asistencia del empleado en la base de datos
                foreach (DataRow Registro_Asistencia in Datos.P_Dt_Lista_Asistencia.Rows)
                {
                    Mi_SQL.Length = 0;
                    Mi_SQL.Append("SELECT * FROM " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias);
                    Mi_SQL.Append(" WHERE " + Ope_Nom_Asistencias.Campo_Empleado_ID + " = '" + Registro_Asistencia["EMPLEADO_ID"].ToString() + "'");
                    Mi_SQL.Append(" AND " + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Convert.ToDateTime(Registro_Asistencia["FECHA_HORA_ENTRADA"].ToString())) + "', 'DD/MM/YYYY HH24:MI:SS')");
                    Dt_Asistencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                    //Valida que no exista el registro ya dado de alta en la base de datos
                    if (Dt_Asistencia.Rows.Count < 1)
                    {
                        Mi_SQL.Length = 0;
                        //Consulta el último No de Asistencia que fue agregado a la base de datos
                        Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Nom_Asistencias.Campo_No_Asistencia + "),'0000000000')");
                        Mi_SQL.Append(" FROM " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias);
                        Mi_SQL.Append(" WHERE " + Ope_Nom_Asistencias.Campo_Empleado_ID + " = '" + Registro_Asistencia["EMPLEADO_ID"].ToString() + "'");
                        No_Asistencia = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                        if (Convert.IsDBNull(No_Asistencia))
                        {
                            No_Asistencia = "0000000001";
                        }
                        else
                        {
                            No_Asistencia = String.Format("{0:0000000000}", Convert.ToInt32(No_Asistencia) + 1);
                        }
                        Mi_SQL.Length = 0;
                        //Consulta para la inserción de los datos
                        Mi_SQL.Append("INSERT INTO " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias + "(");
                        Mi_SQL.Append(Ope_Nom_Asistencias.Campo_No_Asistencia + ", " + Ope_Nom_Asistencias.Campo_Empleado_ID + ", ");
                        Mi_SQL.Append(Ope_Nom_Asistencias.Campo_Reloj_Checador_ID + ", ");
                        Mi_SQL.Append(Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada + ", ");
                        Mi_SQL.Append(Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida + ", " + Ope_Nom_Asistencias.Campo_Usuario_Creo + ", ");
                        Mi_SQL.Append(Ope_Nom_Asistencias.Campo_Fecha_Creo + ") VALUES ('");
                        Mi_SQL.Append(No_Asistencia.ToString() + "', '" + Registro_Asistencia["EMPLEADO_ID"].ToString() + "', '" + Registro_Asistencia["RELOJ_CHECADOR_ID"].ToString() + "', ");
                        Mi_SQL.Append("TO_TIMESTAMP ('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Convert.ToDateTime(Registro_Asistencia["FECHA_HORA_ENTRADA"].ToString())) + "', 'DD/MM/YYYY HH24:MI:SS'), ");
                        if (!String.IsNullOrEmpty(Registro_Asistencia["FECHA_HORA_SALIDA"].ToString()))
                        {
                            Mi_SQL.Append("TO_TIMESTAMP ('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Convert.ToDateTime(Registro_Asistencia["FECHA_HORA_SALIDA"].ToString())) + "', 'DD/MM/YYYY HH24:MI:SS'), '");
                        }
                        else
                        {
                            Mi_SQL.Append("NULL, '");
                        }
                        Mi_SQL.Append(Datos.P_Nombre_Usuario + "', SYSDATE)");
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                    }
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
        /// *****************************************************************************************************************************
        /// Nombre: queryListAsistencia
        /// 
        /// Descripción: Consulta la lista de asistencia de los empleados que checan actualmente.
        /// 
        /// Parámetros: Datos.- Instacia transportadora de datos.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creo: 31/Mayo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *****************************************************************************************************************************
        public static DataTable queryListAsistencia(Cls_Ope_Nom_Generacion_Asistencias_Empleados_Negocio Datos)
        {
            DataTable Dt_Asistencias = new DataTable();//Tabla que almacenara la estructura de asistencias.
            DateTime? Fecha_Aux = null;//Fecha auxiliar para el control de las asistencias de los empleados.

            try
            {
                //Creamos la estructura de la tabla de asistencias
                Dt_Asistencias.Columns.Add("EMPLEADO_ID", typeof(System.String));
                Dt_Asistencias.Columns.Add("RELOJ_CHECADOR_ID", typeof(System.String));
                Dt_Asistencias.Columns.Add("NO_EMPLEADO", typeof(System.String));
                Dt_Asistencias.Columns.Add("EMPLEADO", typeof(System.String));
                Dt_Asistencias.Columns.Add("CLAVE", typeof(System.String));
                Dt_Asistencias.Columns.Add("FECHA_HORA_ENTRADA", typeof(System.DateTime));
                Dt_Asistencias.Columns.Add("FECHA_HORA_SALIDA", typeof(System.DateTime));

                if (Datos.P_Dt_Empleados is DataTable)
                {
                    var empleados = from item_empleado in Datos.P_Dt_Empleados.AsEnumerable()
                                    select new
                                    {
                                        Empleado_ID = item_empleado.IsNull("Empleado_ID") ? String.Empty : item_empleado.Field<String>("Empleado_ID"),
                                        No_Empleado = item_empleado.IsNull("No_Empleado") ? String.Empty : item_empleado.Field<String>("No_Empleado"),
                                        Empleado = item_empleado.IsNull("Empleado") ? String.Empty : item_empleado.Field<String>("Empleado")
                                    };

                    if (empleados != null)
                    {
                        foreach (var empleado in empleados)
                        {
                            if (empleado != null)
                            {
                                Fecha_Aux = Datos.P_Fecha_Hora_Entrada;

                                while (DateTime.Compare(((DateTime)Fecha_Aux), Datos.P_Fecha_Hora_Salida) <= 0)
                                {
                                    String Mi_SQL = "select TO_CHAR(fecha_hora_checada, 'DD-MON-YYYY HH24:MI:SS') as Fecha_Hora_Checada, " +
                                                    "reloj_checador_id, " +
                                                    "(select cat_nom_reloj_checador.clave from cat_nom_reloj_checador where cat_nom_reloj_checador.reloj_checador_id = ope_nom_incidencias_checadas.reloj_checador_id) as reloj " +
                                                    "from ope_nom_incidencias_checadas " +
                                                    "where " +
                                                    "empleado_id in (select empleado_id from cat_empleados where no_empleado='" + empleado.No_Empleado + "') " +
                                                    "and " +
                                                    "( " +
                                                    "no_incidencia_checada in " +
                                                    "(select  " +
                                                    "* " +
                                                    "from " +
                                                    "(SELECT chk_e.no_incidencia_checada " +
                                                    "  from ope_nom_incidencias_checadas Chk_E " +
                                                    "  where  " +
                                                    "  chk_e.fecha_hora_checada between TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", ((DateTime)Fecha_Aux)) + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS') and TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", ((DateTime)Fecha_Aux)) + " 23:59:59', 'DD/MM/YYYY HH24:MI:SS') " +
                                                    "  and " +
                                                    "  chk_e.empleado_id in (select empleado_id from cat_empleados where no_empleado='" + empleado.No_Empleado + "') order by Chk_E.fecha_hora_checada asc) " +
                                                    "where " +
                                                    "rownum=1) " +
                                                    "or " +
                                                    "no_incidencia_checada in " +
                                                    "(select " +
                                                    "* " +
                                                    "from " +
                                                    "(SELECT  chk_e.no_incidencia_checada " +
                                                    "  from ope_nom_incidencias_checadas Chk_E " +
                                                    "  where  " +
                                                    "  chk_e.fecha_hora_checada between TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", ((DateTime)Fecha_Aux)) + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS') and TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", ((DateTime)Fecha_Aux)) + " 23:59:59', 'DD/MM/YYYY HH24:MI:SS') " +
                                                    "  and " +
                                                    "  chk_e.empleado_id in (select empleado_id from cat_empleados where no_empleado='" + empleado.No_Empleado + "') order by Chk_E.fecha_hora_checada desc) " +
                                                    "where " +
                                                    "rownum=1) " +
                                                    ") order by fecha_hora_checada asc";

                                    DataTable Dt_Chk_Dia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                                    String Checada_Entrada = String.Empty;
                                    String Checada_Salida = String.Empty;
                                    Int32 Contador = 1;
                                    var clave = string.Empty;
                                    var reloj_checador_id = string.Empty;

                                    if (Dt_Chk_Dia is DataTable)
                                    {
                                        if (Dt_Chk_Dia.Rows.Count > 0)
                                        {
                                            foreach (DataRow CHECADA in Dt_Chk_Dia.Rows)
                                            {
                                                if (CHECADA is DataRow)
                                                {
                                                    if (!String.IsNullOrEmpty(CHECADA["reloj_checador_id"].ToString()))
                                                        reloj_checador_id = CHECADA["reloj_checador_id"].ToString();

                                                    if (!String.IsNullOrEmpty(CHECADA["reloj"].ToString()))
                                                        clave = CHECADA["reloj"].ToString();


                                                    if (Contador == 1)
                                                    {
                                                        if (!String.IsNullOrEmpty(CHECADA["Fecha_Hora_Checada"].ToString()))
                                                            Checada_Entrada = CHECADA["Fecha_Hora_Checada"].ToString();
                                                    }

                                                    if (Contador == 2)
                                                        if (!String.IsNullOrEmpty(CHECADA["Fecha_Hora_Checada"].ToString()))
                                                            Checada_Salida = CHECADA["Fecha_Hora_Checada"].ToString();

                                                    Contador = Contador + 1;
                                                }
                                            }
                                        }
                                    }

                                    Fecha_Aux = ((DateTime)(Fecha_Aux)).AddDays(1);

                                    //Agrega la asistencia del empleado
                                    DataRow Renglon = Dt_Asistencias.NewRow();
                                    Renglon["EMPLEADO_ID"] = empleado.Empleado_ID.Trim();
                                    Renglon["RELOJ_CHECADOR_ID"] = reloj_checador_id.Trim();
                                    Renglon["NO_EMPLEADO"] = empleado.No_Empleado.Trim();
                                    Renglon["EMPLEADO"] = empleado.Empleado.Trim();
                                    Renglon["CLAVE"] = clave.Trim();
                                    if (!String.IsNullOrEmpty(Checada_Entrada))
                                        Renglon["FECHA_HORA_ENTRADA"] = Convert.ToDateTime(Checada_Entrada);
                                    if (!String.IsNullOrEmpty(Checada_Salida))
                                        Renglon["FECHA_HORA_SALIDA"] = Convert.ToDateTime(Checada_Salida);

                                    if (!String.IsNullOrEmpty(clave))
                                        Dt_Asistencias.Rows.Add(Renglon);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener la lista de asistencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Asistencias; 
        }
    }
}