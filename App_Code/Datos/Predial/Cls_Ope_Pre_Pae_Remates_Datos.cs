using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Predial_Pae_Remates.Negocio;

namespace Presidencia.Predial_Pae_Remates.Datos
{
    public class Cls_Ope_Pre_Pae_Remates_Datos
    {
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
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Ejecuta_Consulta
        ///DESCRIPCIÓN: Ejecuta la consulta que se acaba de crear
        ///PARÁMETROS:     
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 16/Febrero/2012 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        private static void Ejecuta_Consulta(String Mi_SQL)
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
                    Mensaje = "Error al intentar dar de Alta un Registro. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
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
        ///NOMBRE DE LA FUNCIÓN: Alta_Pae_Remates
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nueva Almoneda de una determinada cuenta
        ///PARAMENTROS:     
        ///CREO: Armando Zavala Moreno
        ///FECHA_CREO: 26/Abril/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Pae_Remates(Cls_Ope_Pre_Pae_Remates_Negocio Remates)
        {
            StringBuilder MI_SQL = new StringBuilder();
            try
            {
                String No_Remate = Obtener_ID_Consecutivo(Ope_Pre_Pae_Remates.Tabla_Ope_Pre_Pae_Remates, Ope_Pre_Pae_Remates.Campo_No_Remate, 10);
                MI_SQL.Append("INSERT INTO " + Ope_Pre_Pae_Remates.Tabla_Ope_Pre_Pae_Remates);
                MI_SQL.Append(" (" + Ope_Pre_Pae_Remates.Campo_No_Remate + ", " + Ope_Pre_Pae_Remates.Campo_No_Detalle_Etapa + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Remates.Campo_Lugar_Remate + ", " + Ope_Pre_Pae_Remates.Campo_Fecha_Hora_Remate + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Remates.Campo_Inicio_Publicacion + ", " + Ope_Pre_Pae_Remates.Campo_Fin_Publicacion + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Remates.Campo_Adeudo_Actual + ", " + Ope_Pre_Pae_Remates.Campo_Adeudo_Cubierto + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Remates.Campo_Adeudo_Restante + ", ");
                MI_SQL.Append(Ope_Pre_Pae_Remates.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Remates.Campo_Fecha_Creo + ")");
                MI_SQL.Append(" VALUES ('" + No_Remate + "' ");
                MI_SQL.Append(",'" + Remates.P_No_Detalle_Etapa + "' ");
                MI_SQL.Append(",'" + Remates.P_Lugar_Remate + "' ");
                MI_SQL.Append(",'" + Remates.P_Fecha_Hora_Remate + "' ");
                MI_SQL.Append(",'" + Remates.P_Inicio_Publicacion + "' ");
                MI_SQL.Append(",'" + Remates.P_Fin_Publicacion + "' ");
                MI_SQL.Append(",'" + Remates.P_Adeudo_Actual + "' ");
                MI_SQL.Append(",'" + Remates.P_Adeudo_Cubierto + "' ");
                MI_SQL.Append(",'" + Remates.P_Adeudo_Restante + "' ");
                MI_SQL.Append(",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'");
                MI_SQL.Append(", sysdate");
                MI_SQL.Append(")");
                Ejecuta_Consulta(MI_SQL.ToString());
            }
            catch (Exception Ex)
            {
                new Exception(Ex.Message);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Detalles_Remate
        ///DESCRIPCIÓN: Devuelve los detalles de los bienes que estan listos para remtar
        ///PARAMETROS:      
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 21/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Detalles_Remate(Cls_Ope_Pre_Pae_Remates_Negocio Remates)
        {
            DataTable Tabla = new DataTable();
            StringBuilder Mi_SQL = new StringBuilder();
            Boolean Segunda_Condicion = false;
            try
            {
                Mi_SQL.Append("SELECT ");

                Mi_SQL.Append(" B." + Ope_Pre_Pae_Bienes.Campo_No_Bien);
                Mi_SQL.Append(", R." + Ope_Pre_Pae_Remates.Campo_Lugar_Remate);
                Mi_SQL.Append(", R." + Ope_Pre_Pae_Remates.Campo_Fecha_Hora_Remate);
                Mi_SQL.Append(", R." + Ope_Pre_Pae_Remates.Campo_Inicio_Publicacion);
                Mi_SQL.Append(", R." + Ope_Pre_Pae_Remates.Campo_Fin_Publicacion);
                Mi_SQL.Append(", B." + Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id);
                Mi_SQL.Append(", B." + Ope_Pre_Pae_Bienes.Campo_Descripcion);
                Mi_SQL.Append(", B." + Ope_Pre_Pae_Bienes.Campo_Valor);
                Mi_SQL.Append(", I." + Ope_Pre_Pae_Imagenes_Bienes.Campo_Ruta_Imagen);

                Mi_SQL.Append(" FROM "+Ope_Pre_Pae_Remates.Tabla_Ope_Pre_Pae_Remates+" R");
                Mi_SQL.Append(" INNER JOIN " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + " P");
                Mi_SQL.Append(" ON R." + Ope_Pre_Pae_Remates.Campo_No_Detalle_Etapa + "=P." + Ope_Pre_Pae_Peritajes.Campo_No_Detalle_Etapa);                Mi_SQL.Append(" INNER JOIN " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + " B");
                Mi_SQL.Append(" ON P." + Ope_Pre_Pae_Peritajes.Campo_No_Peritaje + "=B." + Ope_Pre_Pae_Bienes.Campo_No_Peritaje);
                Mi_SQL.Append(" INNER JOIN " + Ope_Pre_Pae_Imagenes_Bienes.Tabla_Ope_Pre_Pae_Imagenes_Bienes + " I");
                Mi_SQL.Append(" ON B." + Ope_Pre_Pae_Bienes.Campo_No_Bien + "=I." + Ope_Pre_Pae_Imagenes_Bienes.Campo_No_Bien);

                if (!String.IsNullOrEmpty(Remates.P_Tipo_Bien))
                {
                    if (Segunda_Condicion)
                    {
                        Mi_SQL.Append(" AND ");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE ");
                    }
                    Mi_SQL.Append("B." + Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id + "='" + Remates.P_Tipo_Bien + "'");
                    Segunda_Condicion = true;
                }

                if (!String.IsNullOrEmpty(Remates.P_No_Detalle_Etapa)) 
                {
                    if (Segunda_Condicion)
                    {
                        Mi_SQL.Append(" AND ");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE ");
                    }
                    Mi_SQL.Append("R." + Ope_Pre_Pae_Remates.Campo_No_Detalle_Etapa + "='" + Remates.P_No_Detalle_Etapa + "'");
                    Segunda_Condicion = true;
                }

                if (!String.IsNullOrEmpty(Remates.P_Fecha_Actual))
                {
                    if (Segunda_Condicion)
                    {
                        Mi_SQL.Append(" AND ");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE ");
                    }
                    Mi_SQL.Append("R." + Ope_Pre_Pae_Remates.Campo_Fecha_Hora_Remate + ">=" + Remates.P_Fecha_Actual );
                    Segunda_Condicion = true;
                }
                Mi_SQL.Append(" ORDER BY B." + Ope_Pre_Pae_Bienes.Campo_No_Bien);
                DataSet Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                if (Ds_Consulta != null && Ds_Consulta.Tables.Count > 0)
                {
                    Tabla = Ds_Consulta.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de los Remates. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }
    }
}
