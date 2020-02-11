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
using Presidencia.Generar_Faltas_Retardos_Empleados.Negocio;
using Presidencia.DateDiff;
using Presidencia.Ayudante_Informacion;
using Presidencia.Cat_Parametros_Nomina.Negocio;

namespace Presidencia.Generar_Faltas_Retardos_Empleados.Datos
{
    public class Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Datos
    {
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Lista_Faltas_Retardos
        /// DESCRIPCION : Consulta las faltas y retardos que serán aplicados al empleado
        ///               validando para estas sus vacaciones, faltas que fueron registradas
        ///               con anterioridad e incapacidades
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 08-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Lista_Faltas_Retardos(Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio Datos)
        {
            //Object Minutos;
            Int64 Parametros_Minutos = 0;           //Parametro de minutos a considerar para que se le descuente al empleado como retardo y no como falta
            Int64 Minutos_Retardo_Entrada = 0;      //Obtiene los minutos de retardo que tuvo el empleado
            Int64 Minutos_Retardo_Salida = 0;       //Obtiene los minutos de retardo que tuvo el empleado
            String Empleado_ID = "";                //Obtiene el ID del empleado del cual se esta generando sus incidencias
            String Dependencia_ID = "";             //Obtiene el ID de la dependencia a la cual pertenece el empleado
            String No_Empleado = "";                //Obtiene el No Empleado que se esta consultando
            String Nombre_Empleado = "";            //Obtiene el nombre del empleado que se esta consultando
            Int64 Dias_Asistencias = 0;             //Obtiene el número de días a consultar el registro de asistencias del empleado
            Int64 Dia = 0;                          //Contador del número de días de asistencias
            DateTime Fecha_Consulta;                //Obtiene la fecha de consulta de los valores
            DateTime Hora_Entrada_Turno;            //Obtiene la hora de entra del turno
            DateTime Hora_Salida_Turno;             //Obtiene la fecha de salida del turno
            DateTime Fecha_Inicio_Checada_Empleado; //Indica cuando empieza a registrar su asistencia el empleado
            StringBuilder Mi_SQL = new StringBuilder();           //Variable que obtendra la consulta de los datos
            DataTable Dt_Empleados = new DataTable();             //Variable a contener la lista de los empleados que checan
            DataTable Dt_Turnos = new DataTable();                //Obtiene el horario de entrada y salida del empleado
            DataTable Dt_Lista_Faltas_Retardos = new DataTable(); //Obtiene todas las faltas y retardos de los empleados
            DataTable Dt_Dia_Festivo = new DataTable();           //Indica si el día que se esta consultando no esa festivo
            DataTable Dt_Falta_Empleado = new DataTable();        //Indica si el empleado ya tiene asignado una falta o retardo
            DataTable Dt_Incapacidades = new DataTable();         //Indica si el empleado tuvo alguna incapacidad
            DataTable Dt_Vacaciones = new DataTable();            //Indica si el empleado tuvo vacaciones
            DataTable Dt_Asistencias = new DataTable();           //Consulta si el empleado asistio a trabajar
            DataTable Dt_Horarios = new DataTable();              //Obtiene el horario de entrada y salida del empleado de acuerdo a su registro de horario

            try
            {
                //Consulta los datos generales de empleado
                Mi_SQL.Append("SELECT " + Cat_Empleados.Campo_Empleado_ID + ", " + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(Cat_Empleados.Campo_Dependencia_ID + ", " + Cat_Empleados.Campo_Turno_ID + ", ");
                Mi_SQL.Append(Cat_Empleados.Campo_Fecha_Inicio_Reloj_Checador + ",");
                Mi_SQL.Append("(" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append("||' '||" + Cat_Empleados.Campo_Nombre + ") AS Empleado");
                Mi_SQL.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE " + Cat_Empleados.Campo_Reloj_Checador + " = 'SI'");
                Mi_SQL.Append(" AND " + Cat_Empleados.Campo_Estatus + " = 'ACTIVO'");
                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                //Obtiene los empleados que checan dentro de presidencia y su horario de entrada y salidaz
                if (Dt_Empleados.Rows.Count > 0)
                {
                    //Se realiza la estructura a contener los datos
                    Dt_Lista_Faltas_Retardos.Columns.Add("Empleado_ID", typeof(System.String));
                    Dt_Lista_Faltas_Retardos.Columns.Add("Dependencia_ID", typeof(System.String));
                    Dt_Lista_Faltas_Retardos.Columns.Add("No_Empleado", typeof(System.String));
                    Dt_Lista_Faltas_Retardos.Columns.Add("Empleado", typeof(System.String));
                    Dt_Lista_Faltas_Retardos.Columns.Add("Fecha", typeof(System.DateTime));
                    Dt_Lista_Faltas_Retardos.Columns.Add("Retardo", typeof(System.String));
                    Dt_Lista_Faltas_Retardos.Columns.Add("Cantidad", typeof(System.Int64));
                    Dt_Lista_Faltas_Retardos.Columns.Add("Tipo_Falta", typeof(System.String));
                    DataRow Renglon;

                    //  para obtener el parametro de minutos de retardo
                    Cls_Cat_Nom_Parametros_Negocio INF_NOMINA = null;
                    Int64 Minutos_Retardo = 0;
                    INF_NOMINA = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

                    if (INF_NOMINA is Cls_Cat_Nom_Parametros_Negocio)
                    {
                        if (!string.IsNullOrEmpty(INF_NOMINA.P_Minutos_Retardo))
                        {
                            Minutos_Retardo=Convert.ToInt64(INF_NOMINA.P_Minutos_Retardo);
                        }
                    }

                    //Obtiene los mitunos de retardos que se tienen como límite al empleado para que no se considere como falta
                    Parametros_Minutos = Minutos_Retardo;

                    Fecha_Inicio_Checada_Empleado = Datos.P_Fecha_Inicio;
                    foreach (DataRow Registro in Dt_Empleados.Rows)
                    {
                        Empleado_ID = Registro[Cat_Empleados.Campo_Empleado_ID].ToString();
                        No_Empleado = Registro[Cat_Empleados.Campo_No_Empleado].ToString();
                        Nombre_Empleado = Registro["Empleado"].ToString();
                        Dependencia_ID = Registro[Cat_Empleados.Campo_Dependencia_ID].ToString();
                        Hora_Entrada_Turno = Convert.ToDateTime("00:00:00");
                        Hora_Salida_Turno = Convert.ToDateTime("23:59:59");
                        Datos.P_Fecha_Inicio = Fecha_Inicio_Checada_Empleado;
                        if (!String.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Fecha_Inicio_Reloj_Checador].ToString()))
                        {
                            if (Cls_DateAndTime.DateDiff(DateInterval.Day, Datos.P_Fecha_Inicio, Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", Registro[Cat_Empleados.Campo_Fecha_Inicio_Reloj_Checador]))) > 0)
                            {
                                Datos.P_Fecha_Inicio = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", Registro[Cat_Empleados.Campo_Fecha_Inicio_Reloj_Checador]));
                            }
                        }
                        if (!String.IsNullOrEmpty(Registro[Cat_Empleados.Campo_Turno_ID].ToString()))
                        {
                            Mi_SQL.Length = 0;
                            //Consulta los horarios de entrada y salida del turno
                            Mi_SQL.Append("SELECT " + Cat_Turnos.Campo_Hora_Entrada + ", " + Cat_Turnos.Campo_Hora_Salida);
                            Mi_SQL.Append(" FROM " + Cat_Turnos.Tabla_Cat_Turnos);
                            Mi_SQL.Append(" WHERE " + Cat_Turnos.Campo_Turno_ID + " = '" + Registro[Cat_Empleados.Campo_Turno_ID].ToString() + "'");
                            Dt_Turnos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                            foreach (DataRow Registro_Turno in Dt_Turnos.Rows)
                            {
                                Hora_Entrada_Turno = Convert.ToDateTime(String.Format("{0:00:00:00}", Registro_Turno[Cat_Turnos.Campo_Hora_Entrada].ToString()));
                                Hora_Salida_Turno = Convert.ToDateTime(String.Format("{0:00:00:00}", Registro_Turno[Cat_Turnos.Campo_Hora_Salida].ToString()));
                            }
                        }

                        //Obtiene la fecha de inicio de la consulta y el número de días a consultar
                        if (Dias_Asistencias == 0)
                        {
                            Dias_Asistencias = Convert.ToInt64(Datos.P_Fecha_Termino.Subtract(Datos.P_Fecha_Inicio).Days.ToString());
                        }
                        else
                        {
                            Dias_Asistencias = Convert.ToInt64(Datos.P_Fecha_Termino.Subtract(Datos.P_Fecha_Inicio).Days.ToString());//Sumar 1 ya que no recorrera 14 dias si no 13
                        }
                        //Consulta que se tengan registro de asistencias en los días a consultar para el usuario
                        for (Dia = 0; Dia <= Dias_Asistencias; Dia++)
                        {
                            Fecha_Consulta = Datos.P_Fecha_Inicio.AddDays(Dia); //Se asigna la fecha de consulta de los valores
                            //Se valida que el día de la semana a consulta no sea sábado o domingo para poder realizar las operaciones
                            if (Fecha_Consulta.DayOfWeek != DayOfWeek.Saturday && Fecha_Consulta.DayOfWeek != DayOfWeek.Sunday)
                            {
                                Mi_SQL.Length = 0;
                                //Consulta si el día que se esta consultando no sea festivo
                                Mi_SQL.Append("SELECT " + Tab_Nom_Dias_Festivos.Campo_Fecha + " FROM " + Tab_Nom_Dias_Festivos.Tabla_Tab_Nom_Dias_Festivos);
                                Mi_SQL.Append(" WHERE " + Tab_Nom_Dias_Festivos.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                                Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " 23:59:00', 'DD/MM/YYYY HH24:MI:SS')");
                                Dt_Dia_Festivo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                                if (Dt_Dia_Festivo.Rows.Count == 0)
                                {
                                    Mi_SQL.Length = 0;
                                    //Consulta que no esta dada de alta algun registro en la base de datos con falta o registro del empleado del día que se esta consultando
                                    Mi_SQL.Append("SELECT " + Ope_Nom_Faltas_Empleado.Campo_Fecha + " FROM " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado);
                                    Mi_SQL.Append(" WHERE " + Ope_Nom_Faltas_Empleado.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                                    Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " 23:59:00', 'DD/MM/YYYY HH24:MI:SS') ");
                                    Mi_SQL.Append(" AND " + Ope_Nom_Faltas_Empleado.Campo_Estatus + " = 'Autorizado'");
                                    Mi_SQL.Append(" AND " + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + " = '" + Empleado_ID.ToString() + "'");
                                    Dt_Falta_Empleado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                                    if (Dt_Falta_Empleado.Rows.Count == 0)
                                    {
                                        Mi_SQL.Length = 0;
                                        //Consulta que el empleado no haya tenido incapacidades en el día que se esta consultando
                                        Mi_SQL.Append("SELECT " + Ope_Nom_Incapacidades.Campo_Fecha_Inicio + " FROM " + Ope_Nom_Incapacidades.Tabla_Ope_Nom_Incapacidades);
                                        Mi_SQL.Append(" WHERE (" + Ope_Nom_Incapacidades.Campo_Fecha_Inicio + " <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                                        Mi_SQL.Append(" AND " + Ope_Nom_Incapacidades.Campo_Fecha_Fin + " >= TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " 23:59:00', 'DD/MM/YYYY HH24:MI:SS')) ");
                                        Mi_SQL.Append(" AND " + Ope_Nom_Faltas_Empleado.Campo_Estatus + " = 'Autorizado'");
                                        Mi_SQL.Append(" AND " + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + " = '" + Empleado_ID.ToString() + "'");
                                        Dt_Incapacidades = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                                        if (Dt_Incapacidades.Rows.Count == 0)
                                        {
                                            Mi_SQL.Length = 0;
                                            Mi_SQL.Append("SELECT " + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada + ", " + Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida);
                                            Mi_SQL.Append(" FROM " + Ope_Nom_Asistencias.Tabla_Ope_Nom_Asistencias);
                                            Mi_SQL.Append(" WHERE " + Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada + " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                                            Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " 23:59:00', 'DD/MM/YYYY HH24:MI:SS') ");
                                            Mi_SQL.Append(" AND " + Ope_Nom_Asistencias.Campo_Empleado_ID + " = '" + Empleado_ID.ToString() + "'");
                                            Dt_Asistencias = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                                            if (Dt_Asistencias.Rows.Count > 0)
                                            {
                                                foreach (DataRow Registro_Asistencias in Dt_Asistencias.Rows)
                                                {
                                                    //Valida que tenga la hora de salida del empleado para poder consultar la diferencia de minutos de su
                                                    //checada con respecto a la hora de entrada
                                                    if (!String.IsNullOrEmpty(Registro_Asistencias[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida].ToString()))
                                                    {
                                                        Mi_SQL.Length = 0;
                                                        //Consulta el horario del empleado que tiene registrado para el día que se esta consultando
                                                        Mi_SQL.Append("SELECT " + Ope_Nom_Horarios_Empleados.Campo_Hora_Entrada + ", " + Ope_Nom_Horarios_Empleados.Campo_Hora_Salida);
                                                        Mi_SQL.Append(" FROM " + Ope_Nom_Horarios_Empleados.Tabla_Ope_Nom_Horarios_Empleados);
                                                        Mi_SQL.Append(" WHERE (" + Ope_Nom_Horarios_Empleados.Campo_Fecha_Inicio + " <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                                                        Mi_SQL.Append(" AND " + Ope_Nom_Horarios_Empleados.Campo_Fecha_Termino + " >= TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " 23:59:00', 'DD/MM/YYYY HH24:MI:SS')) ");
                                                        Mi_SQL.Append(" AND " + Ope_Nom_Horarios_Empleados.Campo_Empleado_ID + " = '" + Empleado_ID.ToString() + "'");
                                                        Dt_Horarios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                                                        if (Dt_Horarios.Rows.Count > 0)
                                                        {
                                                            foreach (DataRow Registro_Horarios in Dt_Horarios.Rows)
                                                            {
                                                                //Obtiene los minutos de diferencia que huvo entre la hora de checada de la entrada y la hora de entrada
                                                                Fecha_Consulta = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " " + String.Format("{0:HH:mm:ss}", Convert.ToDateTime(Registro_Horarios[Ope_Nom_Horarios_Empleados.Campo_Hora_Entrada].ToString())));
                                                                //Minutos_Retardo_Entrada = Convert.ToInt64(Convert.ToDateTime(String.Format("{0:dd/MM/yyyy  HH:mm:ss}", Fecha_Consulta)).Subtract(Convert.ToDateTime(String.Format("{0:dd/MM/yyyy HH:mm:ss}", Convert.ToDateTime(Registro_Asistencias[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada].ToString())))).Minutes.ToString());
                                                                Minutos_Retardo_Entrada = Cls_DateAndTime.DateDiff(DateInterval.Minute, Fecha_Consulta, ((DateTime)Registro_Asistencias[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada]));

                                                                //Obtiene los minutos de diferencia que huvo entre la hora de checada de salida y la hora de salida
                                                                Fecha_Consulta = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " " + String.Format("{0:HH:mm:ss}", Convert.ToDateTime(Registro_Horarios[Ope_Nom_Horarios_Empleados.Campo_Hora_Salida].ToString())));
                                                                //Minutos_Retardo_Salida = Convert.ToInt64(Convert.ToDateTime(String.Format("{0:dd/MM/yyyy  HH:mm:ss}", Fecha_Consulta)).Subtract(Convert.ToDateTime(String.Format("{0:dd/MM/yyyy HH:mm:ss}", Convert.ToDateTime(Registro_Asistencias[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida].ToString())))).Minutes.ToString());
                                                                Minutos_Retardo_Salida = Cls_DateAndTime.DateDiff(DateInterval.Minute, ((DateTime)Registro_Asistencias[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida]), Fecha_Consulta);
                                                                //Verifica que los minutos de diferencia entre la hora de entrada y su registro no
                                                                //sea mayor a los minutos de toleracia que se le dan al empleado
                                                                if (Minutos_Retardo_Entrada <= Parametros_Minutos)
                                                                {
                                                                    if (Minutos_Retardo_Entrada < 0) Minutos_Retardo_Entrada = 0;
                                                                    if (Minutos_Retardo_Salida > 0) Minutos_Retardo_Entrada += Convert.ToInt64(Minutos_Retardo_Salida);
                                                                    if (Minutos_Retardo_Entrada > 0)
                                                                    {
                                                                        //Agrega los minutos de retardo del empleado
                                                                        Renglon = Dt_Lista_Faltas_Retardos.NewRow();
                                                                        Renglon["Empleado_ID"] = Empleado_ID;
                                                                        Renglon["Dependencia_ID"] = Dependencia_ID;
                                                                        Renglon["No_Empleado"] = No_Empleado;
                                                                        Renglon["Empleado"] = Nombre_Empleado;
                                                                        Renglon["Fecha"] = String.Format("{0:dd/MM/yyyy}", Fecha_Consulta);
                                                                        Renglon["Retardo"] = "SI";
                                                                        Renglon["Cantidad"] = Minutos_Retardo_Entrada;
                                                                        Renglon["Tipo_Falta"] = "RETARDO";
                                                                        Dt_Lista_Faltas_Retardos.Rows.Add(Renglon);
                                                                    }
                                                                }
                                                                else //Si fueron mayores los minutos de tolerancia de entrada entonces asigna una falta al empleado
                                                                {
                                                                    //Agrega la falta del empleado
                                                                    Renglon = Dt_Lista_Faltas_Retardos.NewRow();
                                                                    Renglon["Empleado_ID"] = Empleado_ID;
                                                                    Renglon["Dependencia_ID"] = Dependencia_ID;
                                                                    Renglon["No_Empleado"] = No_Empleado;
                                                                    Renglon["Empleado"] = Nombre_Empleado;
                                                                    Renglon["Fecha"] = String.Format("{0:dd/MM/yyyy}", Fecha_Consulta);
                                                                    Renglon["Retardo"] = "NO";
                                                                    Renglon["Cantidad"] = 1;
                                                                    Renglon["Tipo_Falta"] = "JUSTIFICADA";
                                                                    Dt_Lista_Faltas_Retardos.Rows.Add(Renglon);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            
                                                            //Obtiene los minutos de diferencia que huvo entre la hora de checada de la entrada y la hora de entrada
                                                            Fecha_Consulta = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " " + String.Format("{0:HH:mm:ss}", Convert.ToDateTime(Hora_Entrada_Turno)));
                                                            Minutos_Retardo_Entrada = Cls_DateAndTime.DateDiff(DateInterval.Minute, Fecha_Consulta, ((DateTime)Registro_Asistencias[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada]));
                                                            //Minutos_Retardo_Entrada = Convert.ToInt64(Fecha_Consulta.Subtract(((DateTime)Registro_Asistencias[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada])).Minutes.ToString());
                                                            //Obtiene los minutos de diferencia que huvo entre la hora de checada de salida y la hora de salida
                                                            Fecha_Consulta = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " " + String.Format("{0:HH:mm:ss}", Convert.ToDateTime(Hora_Salida_Turno)));
                                                            Minutos_Retardo_Salida = Cls_DateAndTime.DateDiff(DateInterval.Minute, ((DateTime)Registro_Asistencias[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida]), Fecha_Consulta);
                                                            //Minutos_Retardo_Salida = Convert.ToInt64(Fecha_Consulta.Subtract(((DateTime)Registro_Asistencias[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida])).Minutes.ToString());
                                                            //Verifica que los minutos de diferencia entre la hora de entrada y su registro no
                                                            //sea mayor a los minutos de toleracia que se le dan al empleado
                                                            if (Minutos_Retardo_Entrada <= Parametros_Minutos)
                                                            {
                                                                if (Minutos_Retardo_Entrada < 0) Minutos_Retardo_Entrada = 0;
                                                                if (Minutos_Retardo_Salida > 0) Minutos_Retardo_Entrada += Convert.ToInt64(Minutos_Retardo_Salida);
                                                                //Agrega los minutos de retardo del empleado
                                                                if (Minutos_Retardo_Entrada > 0)
                                                                {
                                                                    Renglon = Dt_Lista_Faltas_Retardos.NewRow();
                                                                    Renglon["Empleado_ID"] = Empleado_ID;
                                                                    Renglon["Dependencia_ID"] = Dependencia_ID;
                                                                    Renglon["No_Empleado"] = No_Empleado;
                                                                    Renglon["Empleado"] = Nombre_Empleado;
                                                                    Renglon["Fecha"] = String.Format("{0:dd/MM/yyyy}", Fecha_Consulta);
                                                                    Renglon["Retardo"] = "SI";
                                                                    Renglon["Cantidad"] = Minutos_Retardo_Entrada;
                                                                    Renglon["Tipo_Falta"] = "RETARDO";
                                                                    Dt_Lista_Faltas_Retardos.Rows.Add(Renglon);
                                                                }
                                                            }
                                                            else //Si fueron mayores los minutos de tolerancia de entrada entonces asigna una falta al empleado
                                                            {
                                                                //Agrega la falta del empleado
                                                                Renglon = Dt_Lista_Faltas_Retardos.NewRow();
                                                                Renglon["Empleado_ID"] = Empleado_ID;
                                                                Renglon["Dependencia_ID"] = Dependencia_ID;
                                                                Renglon["No_Empleado"] = No_Empleado;
                                                                Renglon["Empleado"] = Nombre_Empleado;
                                                                Renglon["Fecha"] = String.Format("{0:dd/MM/yyyy}", Fecha_Consulta);
                                                                Renglon["Retardo"] = "NO";
                                                                Renglon["Cantidad"] = 1;
                                                                Renglon["Tipo_Falta"] = "JUSTIFICADA";
                                                                Dt_Lista_Faltas_Retardos.Rows.Add(Renglon);
                                                            }
                                                        }
                                                    }
                                                    else //Si no se registro salida registra una falta al empleado
                                                    {
                                                        //Agrega la falta del empleado
                                                        Renglon = Dt_Lista_Faltas_Retardos.NewRow();
                                                        Renglon["Empleado_ID"] = Empleado_ID;
                                                        Renglon["Dependencia_ID"] = Dependencia_ID;
                                                        Renglon["No_Empleado"] = No_Empleado;
                                                        Renglon["Empleado"] = Nombre_Empleado;
                                                        Renglon["Fecha"] = String.Format("{0:dd/MM/yyyy}", Fecha_Consulta);
                                                        Renglon["Retardo"] = "NO";
                                                        Renglon["Cantidad"] = 1;
                                                        Renglon["Tipo_Falta"] = "JUSTIFICADA";
                                                        Dt_Lista_Faltas_Retardos.Rows.Add(Renglon);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Mi_SQL.Length = 0;
                                                //Consulta si el empleado pidio vacaciones durante la fecha que se esta consultando
                                                Mi_SQL.Append("SELECT " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID);
                                                Mi_SQL.Append(" FROM " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado);
                                                Mi_SQL.Append(" WHERE (" + OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Inicio + " <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                                                Mi_SQL.Append(" AND " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Termino + " >= TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Fecha_Consulta) + " 23:59:00', 'DD/MM/YYYY HH24:MI:SS')) ");
                                                Mi_SQL.Append(" AND " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + " = 'Autorizado'");
                                                Mi_SQL.Append(" AND " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + " = '" + Empleado_ID.ToString() + "'");
                                                Dt_Vacaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                                                if (Dt_Vacaciones.Rows.Count == 0)
                                                {
                                                    //Agrega la falta del empleado
                                                    Renglon = Dt_Lista_Faltas_Retardos.NewRow();
                                                    Renglon["Empleado_ID"] = Empleado_ID;
                                                    Renglon["Dependencia_ID"] = Dependencia_ID;
                                                    Renglon["No_Empleado"] = No_Empleado;
                                                    Renglon["Empleado"] = Nombre_Empleado;
                                                    Renglon["Fecha"] = String.Format("{0:dd/MM/yyyy}", Fecha_Consulta);
                                                    Renglon["Retardo"] = "NO";
                                                    Renglon["Cantidad"] = 1;
                                                    Renglon["Tipo_Falta"] = "INASISTENCIA";
                                                    Dt_Lista_Faltas_Retardos.Rows.Add(Renglon);

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return Dt_Lista_Faltas_Retardos;
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message + ":::::" + Empleado_ID + ":::::" + No_Empleado);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]" + ":::::" + Empleado_ID + ":::::" + No_Empleado);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message + ":::::" + Empleado_ID + ":::::" + No_Empleado);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Faltas_Retardos
        /// DESCRIPCION : 1.Consulta el último No dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta la falta o retardo del Empleado en la BD con los datos
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 09-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Faltas_Retardos(Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();    //Obtiene la cadena de inserción hacía la base de datos
            DataTable Dt_Falta_Retardo = new DataTable(); //Obtiene el No con la cual se guardo los datos en la base de datos
            Object No_Falta;                              //Obtiene el No con el cual se va a guardar el registro

            try
            {
                //Da de alta la asistencia del empleado en la base de datos
                foreach (DataRow Registro_Faltas_Retardos in Datos.P_Dt_Lista_Faltas_Retardos.Rows)
                {
                    Mi_SQL.Length = 0;
                    Mi_SQL.Append("SELECT * FROM " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado);
                    Mi_SQL.Append(" WHERE " + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + " = '" + Registro_Faltas_Retardos["Empleado_ID"].ToString() + "'");
                    Mi_SQL.Append(" AND " + Ope_Nom_Faltas_Empleado.Campo_Fecha + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Convert.ToDateTime(Registro_Faltas_Retardos["Fecha"].ToString())) + "', 'DD/MM/YYYY HH24:MI:SS')");
                    Mi_SQL.Append(" AND " + Ope_Nom_Faltas_Empleado.Campo_Estatus + " = 'Autorizado'");
                    Dt_Falta_Retardo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                    //Valida que no exista el registro ya dado de alta en la base de datos
                    if (Dt_Falta_Retardo.Rows.Count < 1)
                    {
                        Mi_SQL.Length = 0;
                        //Consulta el último No de Falta que fue agregado a la base de datos
                        Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Nom_Faltas_Empleado.Campo_No_Falta + "),'00000')");
                        Mi_SQL.Append(" FROM " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado);
                        No_Falta = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                        if (Convert.IsDBNull(No_Falta))
                        {
                            No_Falta = "00001";
                        }
                        else
                        {
                            No_Falta = String.Format("{0:00000}", Convert.ToInt32(No_Falta) + 1);
                        }

                        Mi_SQL.Length = 0;
                        //Consulta para la inserción de los datos
                        Mi_SQL.Append("INSERT INTO " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "(");
                        Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_No_Falta + ", " + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + ", ");
                        Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_Dependencia_ID + ", " + Ope_Nom_Faltas_Empleado.Campo_Fecha + ", ");
                        Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_Nomina_ID + ", " + Ope_Nom_Faltas_Empleado.Campo_No_Nomina + ",");
                        Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_Estatus + ", " + Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta + ", ");
                        Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_Retardo + ", " + Ope_Nom_Faltas_Empleado.Campo_Cantidad + ", ");
                        Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_Comentarios + ", " + Ope_Nom_Asistencias.Campo_Usuario_Creo + ", ");
                        Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Campo_Fecha_Creo + ")");
                        Mi_SQL.Append(" VALUES ('" + No_Falta.ToString() + "', '" + Registro_Faltas_Retardos["Empleado_ID"].ToString() + "', '");
                        Mi_SQL.Append(Registro_Faltas_Retardos["Dependencia_ID"].ToString() + "', ");
                        Mi_SQL.Append("TO_TIMESTAMP ('" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", Convert.ToDateTime(Registro_Faltas_Retardos["Fecha"].ToString())) + "', 'DD/MM/YYYY HH24:MI:SS'), '");
                        Mi_SQL.Append(Datos.P_Nomina_ID + "', " + Datos.P_No_Nomina + ", 'Autorizado', '");
                        Mi_SQL.Append(Registro_Faltas_Retardos["Tipo_Falta"] + "', '" + Registro_Faltas_Retardos["Retardo"] + "', ");
                        Mi_SQL.Append(Registro_Faltas_Retardos["Cantidad"] + ", 'GENERACION DE LISTA DE FALTAS Y RETARDOS POR RELOJ CHECADOR', '");
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
    }
}
