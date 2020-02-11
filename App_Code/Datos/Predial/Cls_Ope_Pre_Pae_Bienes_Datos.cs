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
using Presidencia.Sessiones;
using Presidencia.Predial_Pae_Bienes.Negocio;

namespace Presidencia.Predial_Pae_Bienes.Datos
{
    public class Cls_Ope_Pre_Pae_Bienes_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Pae_Bienes
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Bien de una determinada cuenta
        ///PARAMENTROS:     
        ///CREO: Armando Zavala Moreno
        ///FECHA_CREO: 20/Marzo/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO 
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        public static void Alta_Pae_Bienes(Cls_Ope_Pre_Pae_Bienes_Negocio Pae_Bienes)
        {
            try
            {
                String No_Bien = Obtener_ID_Consecutivo(Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes, Ope_Pre_Pae_Bienes.Campo_No_Bien, 10);
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes;
                Mi_SQL += " (" + Ope_Pre_Pae_Bienes.Campo_No_Bien + ", " + Ope_Pre_Pae_Bienes.Campo_No_Peritaje;
                Mi_SQL += ", " + Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id + ", " + Ope_Pre_Pae_Bienes.Campo_Descripcion;
                Mi_SQL += ", " + Ope_Pre_Pae_Bienes.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Bienes.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES ('" + No_Bien + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_No_Peritaje + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Tipo_Bien_ID + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Descripcion + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Valor_Bien + "'";
                Mi_SQL += ",'" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
                Mi_SQL += ", sysdate";
                Mi_SQL += ")";
                Ejecuta_Consulta(Mi_SQL);
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Pae_Peritajes
        ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo peritaje de un determinado bien
        ///PARAMENTROS:     
        ///CREO: Armando Zavala Moreno
        ///FECHA_CREO: 20/Marzo/2012
        ///MODIFICO: Roberto González Oseguera
        ///FECHA_MODIFICO: 26-abr-2012
        ///CAUSA_MODIFICACIÓN: Se agrega transacción y se agrega inserción de bienes y archivos
        ///*******************************************************************************
        public static int Alta_Pae_Peritajes(Cls_Ope_Pre_Pae_Bienes_Negocio Pae_Bienes)
        {
            Int32 Filas_Afectadas = 0;
            DataTable Dt_Bienes;

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            try
            {
                // si llegó un Comando como parámetro, utilizarlo
                if (Pae_Bienes.P_Comando_Transaccion != null)    // si la conexion llego como parametro, establecer como comando para utilizar
                {
                    Comando = Pae_Bienes.P_Comando_Transaccion;
                }
                else    // si no, crear nueva conexion y transaccion
                {
                    Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Conexion;
                    Comando.Transaction = Transaccion;
                }

                String No_Peritaje = Obtener_ID_Consecutivo(Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes, Ope_Pre_Pae_Peritajes.Campo_No_Peritaje, 10);
                String Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes;
                Mi_SQL += " (" + Ope_Pre_Pae_Peritajes.Campo_No_Peritaje + ", " + Ope_Pre_Pae_Peritajes.Campo_No_Detalle_Etapa;
                Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Campo_Avaluo + ", " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Peritaje;
                Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Campo_Perito + ", " + Ope_Pre_Pae_Peritajes.Campo_Valor;
                Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Campo_Observaciones + ", " + Ope_Pre_Pae_Peritajes.Campo_Lugar_Almacenamiento + ", " + Ope_Pre_Pae_Peritajes.Campo_Costo_Metro_Cuadrado;
                Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Campo_Dimensiones + ", " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Ingreso;
                Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Campo_Tiempo_Transcurrido + ", " + Ope_Pre_Pae_Peritajes.Campo_Costo_Almacenamiento;
                Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES ('" + No_Peritaje + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_No_Detalle_Etapa + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Avaluo + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Fecha_Peritaje.ToString("d-M-yyyy") + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Perito + "'";
                Mi_SQL += ", " + Pae_Bienes.P_Valor_Peritaje;
                Mi_SQL += ", '" + Pae_Bienes.P_Observaciones + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Lugar_Almacenamiento + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Costo_Metro_Cuadrado + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Dimensiones + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Fecha_Ingreso.ToString("d-M-yyyy") + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Tiempo_Transcurrido + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Costo_Almacenamiento + "'";
                Mi_SQL += ",'" + Pae_Bienes.P_Nombre_Usuario + "'";
                Mi_SQL += ", sysdate";
                Mi_SQL += ")";

                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                // obtener el número de bien
                int No_Bien;
                Mi_SQL = "SELECT TO_NUMBER(NVL(MAX(" + Ope_Pre_Pae_Bienes.Campo_No_Bien + "),0)) FROM " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes;
                if (!int.TryParse(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).ToString(), out No_Bien))
                {
                    No_Bien = 0;
                }
                // obtener el número de archivo
                int No_Imagen;
                Mi_SQL = "SELECT TO_NUMBER(NVL(MAX(" + Ope_Pre_Pae_Imagenes_Bienes.Campo_No_Imagen + "),0)) FROM " + Ope_Pre_Pae_Imagenes_Bienes.Tabla_Ope_Pre_Pae_Imagenes_Bienes;
                if (!int.TryParse(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).ToString(), out No_Imagen))
                {
                    No_Imagen = 0;
                }

                if (Pae_Bienes.P_Dt_Bienes != null)
                {
                    String[] Arr_Nombres_Archivos;
                    Dt_Bienes = Pae_Bienes.P_Dt_Bienes;
                    // insertar un registro por cada fila en la tabla
                    foreach (DataRow Fila_Bien in Dt_Bienes.Rows)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes
                            + " (" + Ope_Pre_Pae_Bienes.Campo_No_Bien
                            + ", " + Ope_Pre_Pae_Bienes.Campo_No_Peritaje
                            + ", " + Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id
                            + ", " + Ope_Pre_Pae_Bienes.Campo_Descripcion
                            + ", " + Ope_Pre_Pae_Bienes.Campo_Valor
                            + ", " + Ope_Pre_Pae_Peritajes.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Creo + ")"
                            + " VALUES ('" + (++No_Bien).ToString().PadLeft(10, '0') + "'"
                            + ", '" + No_Peritaje + "'"
                            + ", '" + Fila_Bien[Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id] + "'"
                            + ", '" + Fila_Bien[Ope_Pre_Pae_Bienes.Campo_Descripcion] + "'"
                            + ", '" + Fila_Bien[Ope_Pre_Pae_Bienes.Campo_Valor] + "'"
                            + ",'" + Pae_Bienes.P_Nombre_Usuario + "'"
                            + ", sysdate)";

                        Comando.CommandText = Mi_SQL;
                        Filas_Afectadas += Comando.ExecuteNonQuery();

                        Arr_Nombres_Archivos = Fila_Bien["FOTOGRAFIAS"].ToString().Split(',');
                        for (int Contador_Fila = 0; Contador_Fila < Arr_Nombres_Archivos.Length; Contador_Fila++)   //recorrer el arreglo de nombres de archivo
                        {
                            Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Imagenes_Bienes.Tabla_Ope_Pre_Pae_Imagenes_Bienes
                                + " (" + Ope_Pre_Pae_Imagenes_Bienes.Campo_No_Imagen
                                + ", " + Ope_Pre_Pae_Imagenes_Bienes.Campo_No_Bien
                                + ", " + Ope_Pre_Pae_Imagenes_Bienes.Campo_Ruta_Imagen
                                + ", " + Ope_Pre_Pae_Peritajes.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Creo + ")"
                                + " VALUES ('" + (++No_Imagen).ToString().PadLeft(10, '0') + "'"
                                + ", '" + No_Bien.ToString().PadLeft(10, '0') + "'"
                                + ", '" + Arr_Nombres_Archivos[Contador_Fila] + "'"
                                + ",'" + Pae_Bienes.P_Nombre_Usuario + "'"
                                + ", sysdate)";

                            Comando.CommandText = Mi_SQL;
                            Filas_Afectadas += Comando.ExecuteNonQuery();
                        }
                    }
                }

                // aplicar cambios en base de datos
                if (Pae_Bienes.P_Comando_Transaccion == null)
                {
                    Transaccion.Commit();
                }

            }
            catch (Exception Ex)
            {
                if (Pae_Bienes.P_Comando_Transaccion == null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception(Ex.Message);
            }
            finally
            {
                Conexion.Close();
            }

            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Modificar_Peritajes
        /// DESCRIPCIÓN: Actualizar registros de peritajes
        /// PARÁMETROS:
        /// 		1. Pae_Bienes: Instancia de la clase de negocio con datos a actualizar
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 04-may-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Modificar_Peritajes(Cls_Ope_Pre_Pae_Bienes_Negocio Pae_Bienes)
        {
            Int32 Filas_Afectadas = 0;
            DataTable Dt_Bienes;
            String Mi_SQL;

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            // si no se especificó un peritaje, regresar 0
            if (string.IsNullOrEmpty(Pae_Bienes.P_No_Peritaje))
                return 0;

            try
            {
                // si llegó un Comando como parámetro, utilizarlo
                if (Pae_Bienes.P_Comando_Transaccion != null)    // si la conexion llego como parametro, establecer como comando para utilizar
                {
                    Comando = Pae_Bienes.P_Comando_Transaccion;
                }
                else    // si no, crear nueva conexion y transaccion
                {
                    Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Conexion;
                    Comando.Transaction = Transaccion;
                }

                // si hay bienes a eliminar, eliminar imágenes y bienes
                if (Pae_Bienes.P_Bienes_Eliminar != null)
                {
                    foreach (string No_Bien_Eliminar in Pae_Bienes.P_Bienes_Eliminar)
                    {
                        Mi_SQL = "DELETE FROM " + Ope_Pre_Pae_Imagenes_Bienes.Tabla_Ope_Pre_Pae_Imagenes_Bienes
                            + " WHERE " + Ope_Pre_Pae_Imagenes_Bienes.Campo_No_Bien + "="
                            + "'" + No_Bien_Eliminar + "'";

                        Comando.CommandText = Mi_SQL;
                        Filas_Afectadas += Comando.ExecuteNonQuery();

                        Mi_SQL = "DELETE FROM " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes
                            + " WHERE " + Ope_Pre_Pae_Bienes.Campo_No_Bien + " = "
                                + "'" + No_Bien_Eliminar + "'";

                        Comando.CommandText = Mi_SQL;
                        Filas_Afectadas += Comando.ExecuteNonQuery();
                    }
                }

                Mi_SQL = "UPDATE " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + " SET "
                    + Ope_Pre_Pae_Peritajes.Campo_Avaluo + "= '" + Pae_Bienes.P_Avaluo + "'"
                    + ", " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Peritaje + "= '" + Pae_Bienes.P_Fecha_Peritaje.ToString("d-M-yyyy") + "'"
                    + ", " + Ope_Pre_Pae_Peritajes.Campo_Perito + "= '" + Pae_Bienes.P_Perito + "'"
                    + ", " + Ope_Pre_Pae_Peritajes.Campo_Valor + "= " + Pae_Bienes.P_Valor_Peritaje
                    + ", " + Ope_Pre_Pae_Peritajes.Campo_Observaciones + "= '" + Pae_Bienes.P_Observaciones + "'"
                    + ", " + Ope_Pre_Pae_Peritajes.Campo_Lugar_Almacenamiento + "= '" + Pae_Bienes.P_Lugar_Almacenamiento + "'"
                    + ", " + Ope_Pre_Pae_Peritajes.Campo_Costo_Metro_Cuadrado + "= '" + Pae_Bienes.P_Costo_Metro_Cuadrado + "'"
                    + ", " + Ope_Pre_Pae_Peritajes.Campo_Dimensiones + "= '" + Pae_Bienes.P_Dimensiones + "'";
                // validar fecha_ingreso
                if (Pae_Bienes.P_Fecha_Ingreso == DateTime.MinValue)
                {
                    Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Ingreso + "= ''";
                }
                else
                {
                    Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Ingreso + "= '" + Pae_Bienes.P_Fecha_Ingreso.ToString("d-M-yyyy") + "'";
                }
                Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Campo_Tiempo_Transcurrido + "= '" + Pae_Bienes.P_Tiempo_Transcurrido + "'"
                    + ", " + Ope_Pre_Pae_Peritajes.Campo_Costo_Almacenamiento + "= '" + Pae_Bienes.P_Costo_Almacenamiento + "'"
                    + ", " + Ope_Pre_Pae_Peritajes.Campo_Usuario_Modifico + "= '" + Pae_Bienes.P_Nombre_Usuario + "'"
                    + ", " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Modifico + "= sysdate"
                    + " WHERE " + Ope_Pre_Pae_Peritajes.Campo_No_Peritaje + "='" + Pae_Bienes.P_No_Peritaje + "'";

                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                // obtener el número de bien
                int No_Bien;
                Mi_SQL = "SELECT TO_NUMBER(NVL(MAX(" + Ope_Pre_Pae_Bienes.Campo_No_Bien + "),0)) FROM " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes;
                if (!int.TryParse(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).ToString(), out No_Bien))
                {
                    No_Bien = 0;
                }
                // obtener el número de archivo
                int No_Imagen;
                Mi_SQL = "SELECT TO_NUMBER(NVL(MAX(" + Ope_Pre_Pae_Imagenes_Bienes.Campo_No_Imagen + "),0)) FROM " + Ope_Pre_Pae_Imagenes_Bienes.Tabla_Ope_Pre_Pae_Imagenes_Bienes;
                if (!int.TryParse(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).ToString(), out No_Imagen))
                {
                    No_Imagen = 0;
                }

                // si la tabla Bienes contiene registros, insertar
                if (Pae_Bienes.P_Dt_Bienes != null)
                {
                    int Numero_Bien;
                    String[] Arr_Nombres_Archivos;
                    Dt_Bienes = Pae_Bienes.P_Dt_Bienes;
                    // insertar un registro por cada fila en la tabla
                    foreach (DataRow Fila_Bien in Dt_Bienes.Rows)
                    {
                        // si el bien del renglón ya tiene un número de bien (NO_BIEN) pasar al siguiente (el bien ya existe en la base de datos)
                        if (int.TryParse(Fila_Bien[Ope_Pre_Pae_Bienes.Campo_No_Bien].ToString(), out Numero_Bien) || Numero_Bien > 0)
                        {
                            continue;
                        }
                        // generar consulta para insertar bien
                        Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes
                            + " (" + Ope_Pre_Pae_Bienes.Campo_No_Bien
                            + ", " + Ope_Pre_Pae_Bienes.Campo_No_Peritaje
                            + ", " + Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id
                            + ", " + Ope_Pre_Pae_Bienes.Campo_Descripcion
                            + ", " + Ope_Pre_Pae_Bienes.Campo_Valor
                            + ", " + Ope_Pre_Pae_Peritajes.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Creo + ")"
                            + " VALUES ('" + (++No_Bien).ToString().PadLeft(10, '0') + "'"
                            + ", '" + Pae_Bienes.P_No_Peritaje + "'"
                            + ", '" + Fila_Bien[Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id] + "'"
                            + ", '" + Fila_Bien[Ope_Pre_Pae_Bienes.Campo_Descripcion] + "'"
                            + ", '" + Fila_Bien[Ope_Pre_Pae_Bienes.Campo_Valor] + "'"
                            + ",'" + Pae_Bienes.P_Nombre_Usuario + "'"
                            + ", sysdate)";

                        Comando.CommandText = Mi_SQL;
                        Filas_Afectadas += Comando.ExecuteNonQuery();

                        Arr_Nombres_Archivos = Fila_Bien["FOTOGRAFIAS"].ToString().Split(',');
                        for (int Contador_Fila = 0; Contador_Fila < Arr_Nombres_Archivos.Length; Contador_Fila++)   //recorrer el arreglo de nombres de archivo
                        {
                            Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Imagenes_Bienes.Tabla_Ope_Pre_Pae_Imagenes_Bienes
                                + " (" + Ope_Pre_Pae_Imagenes_Bienes.Campo_No_Imagen
                                + ", " + Ope_Pre_Pae_Imagenes_Bienes.Campo_No_Bien
                                + ", " + Ope_Pre_Pae_Imagenes_Bienes.Campo_Ruta_Imagen
                                + ", " + Ope_Pre_Pae_Peritajes.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Creo + ")"
                                + " VALUES ('" + (++No_Imagen).ToString().PadLeft(10, '0') + "'"
                                + ", '" + No_Bien.ToString().PadLeft(10, '0') + "'"
                                + ", '" + Arr_Nombres_Archivos[Contador_Fila] + "'"
                                + ",'" + Pae_Bienes.P_Nombre_Usuario + "'"
                                + ", sysdate)";

                            Comando.CommandText = Mi_SQL;
                            Filas_Afectadas += Comando.ExecuteNonQuery();
                        }
                    }
                }

                // aplicar cambios en base de datos
                if (Pae_Bienes.P_Comando_Transaccion == null)
                {
                    Transaccion.Commit();
                }

            }
            catch (Exception Ex)
            {
                if (Pae_Bienes.P_Comando_Transaccion == null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception(Ex.Message);
            }
            finally
            {
                Conexion.Close();
            }

            return Filas_Afectadas;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Eliminar_Peritaje_Bienes
        /// DESCRIPCIÓN: Dado un número de peritaje, se eliminan los bienes e imagenes del peritaje y luego el mismo peritaje
        /// PARÁMETROS:
        /// 		1. Pae_Bienes: Instancia de la clase de negocio con No_Peritaje
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 04-may-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static int Eliminar_Peritaje_Bienes(Cls_Ope_Pre_Pae_Bienes_Negocio Pae_Bienes)
        {
            Int32 Filas_Afectadas = 0;
            DataTable Dt_Bienes;
            String Mi_SQL;
            DataSet Ds_Resultados_Consulta;

            // variables para transaccion
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            // si no se especificó un peritaje, regresar 0
            if (string.IsNullOrEmpty(Pae_Bienes.P_No_Peritaje))
                return 0;

            try
            {
                // si llegó un Comando como parámetro, utilizarlo
                if (Pae_Bienes.P_Comando_Transaccion != null)    // si la conexion llego como parametro, establecer como comando para utilizar
                {
                    Comando = Pae_Bienes.P_Comando_Transaccion;
                }
                else    // si no, crear nueva conexion y transaccion
                {
                    Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Conexion;
                    Comando.Transaction = Transaccion;
                }

                // consultar los bienes del peritaje
                Mi_SQL = "SELECT " + Ope_Pre_Pae_Bienes.Campo_No_Bien
                    + " FROM " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes
                    + " WHERE " + Ope_Pre_Pae_Bienes.Campo_No_Peritaje + "='" + Pae_Bienes.P_No_Peritaje + "'";

                Ds_Resultados_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Resultados_Consulta != null && Ds_Resultados_Consulta.Tables.Count > 0)
                {
                    // si la consulta arrojó resultados, para cada bien: eliminar las imágenes de Ope_Pre_Pae_Imagenes_Bienes y cada Bien
                    Dt_Bienes = Ds_Resultados_Consulta.Tables[0];
                    foreach (DataRow Dr_Bien in Dt_Bienes.Rows)
                    {
                        Mi_SQL = "DELETE FROM " + Ope_Pre_Pae_Imagenes_Bienes.Tabla_Ope_Pre_Pae_Imagenes_Bienes
                            + " WHERE " + Ope_Pre_Pae_Imagenes_Bienes.Campo_No_Bien + "="
                            + "'" + Dr_Bien[0].ToString() + "'";

                        Comando.CommandText = Mi_SQL;
                        Filas_Afectadas += Comando.ExecuteNonQuery();

                        Mi_SQL = "DELETE FROM " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes
                            + " WHERE " + Ope_Pre_Pae_Bienes.Campo_No_Bien + " = "
                                + "'" + Dr_Bien[0].ToString() + "'";

                        Comando.CommandText = Mi_SQL;
                        Filas_Afectadas += Comando.ExecuteNonQuery();
                    }
                }

                // eliminar el peritaje
                Mi_SQL = "DELETE FROM " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes
                    + " WHERE " + Ope_Pre_Pae_Peritajes.Campo_No_Peritaje + " = "
                        + "'" + Pae_Bienes.P_No_Peritaje + "'";

                Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Comando.ExecuteNonQuery();

                // aplicar cambios en base de datos
                if (Pae_Bienes.P_Comando_Transaccion == null)
                {
                    Transaccion.Commit();
                }

            }
            catch (Exception Ex)
            {
                if (Pae_Bienes.P_Comando_Transaccion == null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception(Ex.Message);
            }
            finally
            {
                Conexion.Close();
            }

            return Filas_Afectadas;
        }

        /*******************************************************************************
        NOMBRE DE LA CLASE: Alta_Pae_Det_Etapas_Depositario
        DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Depositarios
        PARÁMETROS :     
        CREO       : Angel Antonio Escamilla Trejo
        FECHA_CREO : 20-Mar-2012
        MODIFICO          :
        FECHA_MODIFICO    :
        CAUSA_MODIFICACIÓN:
        *******************************************************************************/
        public static void Alta_Pae_Det_Etapas_Depositario(Cls_Ope_Pre_Pae_Bienes_Negocio Pae_Bienes)
        {
            try
            {
                String Mi_SQL = "";
                String No_Cambio_Depositario = Obtener_ID_Consecutivo(Ope_Pre_Pae_Depositarios.Tabla_Ope_Pre_Pae_Depositarios, Ope_Pre_Pae_Depositarios.Campo_No_Detalle_Etapa, 5);
                Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Depositarios.Tabla_Ope_Pre_Pae_Depositarios;
                Mi_SQL += " (" + Ope_Pre_Pae_Depositarios.Campo_No_Cambio_Depositario + ", " + Ope_Pre_Pae_Depositarios.Campo_No_Detalle_Etapa;
                Mi_SQL += ", " + Ope_Pre_Pae_Depositarios.Campo_Figura + ", " + Ope_Pre_Pae_Depositarios.Campo_Nombre_Depositario;
                Mi_SQL += ", " + Ope_Pre_Pae_Depositarios.Campo_Domicilio_Depositario + ", " + Ope_Pre_Pae_Depositarios.Campo_Fecha_Remocion;
                Mi_SQL += ", " + Ope_Pre_Pae_Depositarios.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Depositarios.Campo_Fecha_Creo + ")";
                Mi_SQL += " VALUES ('" + No_Cambio_Depositario + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_No_Detalle_Etapa + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Figura + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Nombre_Depositario + "'";
                Mi_SQL += ", '" + Pae_Bienes.P_Domicilio_Depositario + "'";
                Mi_SQL += ", SYSDATE";
                Mi_SQL += ", '" + Cls_Sessiones.No_Empleado.ToUpper() + "'";
                Mi_SQL += ", SYSDATE";
                Mi_SQL += ")";
                Ejecuta_Consulta(Mi_SQL);
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Bienes_Peritajes
        ///DESCRIPCIÓN: Obtiene registro de las tablas Ope_Pre_Pae_Bienes y Ope_Pre_Pae_Peritajes
        ///PARAMETROS:      
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 21/Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consulta_Bienes_Peritajes(Cls_Ope_Pre_Pae_Bienes_Negocio Pae_Bienes)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            Boolean Segundo_Filtro = false;
            try
            {
                if (Pae_Bienes.P_Campos_Dinamicos != null && Pae_Bienes.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Pae_Bienes.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + "." + Ope_Pre_Pae_Bienes.Campo_No_Bien + ", " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + "." + Ope_Pre_Pae_Bienes.Campo_No_Peritaje;
                    Mi_SQL += ", " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + "." + Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id + ", " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + "." + Ope_Pre_Pae_Bienes.Campo_Descripcion;
                    Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_No_Peritaje;
                    Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Avaluo + ", " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Fecha_Peritaje;
                    Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Perito + ", " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Valor;
                    Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Observaciones;
                    Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Lugar_Almacenamiento;
                    Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Costo_Metro_Cuadrado + ", " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Dimensiones;
                    Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Fecha_Ingreso + ", " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Tiempo_Transcurrido;
                    Mi_SQL += ", " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Costo_Almacenamiento;
                }
                Mi_SQL += " FROM " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes; //// CORREGIR CONSULTA
                if (Pae_Bienes.P_No_Bien != null && Pae_Bienes.P_No_Bien != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + "." + Ope_Pre_Pae_Bienes.Campo_No_Bien + " = '" + Pae_Bienes.P_No_Bien + "'";
                }
                if (Pae_Bienes.P_No_Peritaje != null && Pae_Bienes.P_No_Peritaje != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + "." + Ope_Pre_Pae_Bienes.Campo_No_Peritaje + " = '" + Pae_Bienes.P_No_Peritaje + "'";
                }
                if (Pae_Bienes.P_Tipo_Bien_ID != null && Pae_Bienes.P_Tipo_Bien_ID != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + "." + Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id + " = '" + Pae_Bienes.P_Tipo_Bien_ID + "'";
                }
                if (Pae_Bienes.P_Descripcion != null && Pae_Bienes.P_Descripcion != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + "." + Ope_Pre_Pae_Bienes.Campo_Descripcion + " LIKE '%" + Pae_Bienes.P_Descripcion + "%'";
                }
                if (Pae_Bienes.P_Lugar_Almacenamiento != null && Pae_Bienes.P_Lugar_Almacenamiento != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Lugar_Almacenamiento + " LIKE '%" + Pae_Bienes.P_Lugar_Almacenamiento + "%'";
                }
                if (Pae_Bienes.P_Fecha_Ingreso != DateTime.MinValue)
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Fecha_Ingreso + " = '" + Pae_Bienes.P_Fecha_Ingreso.ToString("d-M-yyyy") + "'";
                }
                if (Pae_Bienes.P_No_Peritaje != null && Pae_Bienes.P_No_Peritaje != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_No_Peritaje + " = '" + Pae_Bienes.P_No_Peritaje + "'";
                }
                if (Pae_Bienes.P_Fecha_Peritaje != DateTime.MinValue)
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Fecha_Peritaje + " = '" + Pae_Bienes.P_Fecha_Peritaje.ToString("d-M-yyyy") + "'";
                }
                if (Pae_Bienes.P_Perito != null && Pae_Bienes.P_Perito != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + "." + Ope_Pre_Pae_Peritajes.Campo_Perito + " = '" + Pae_Bienes.P_Perito + "'";
                }
                if (Pae_Bienes.P_Agrupar_Dinamico != null && Pae_Bienes.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Pae_Bienes.P_Agrupar_Dinamico;
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Bienes
        /// DESCRIPCIÓN: Forma la consulta de bienes con filtros opcionales por número de bien, peritaje y detalle_etapa
        /// PARÁMETROS:
        /// 		1. Pae_Bienes: instancia de la clase de negocio con filtros para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 26-abr-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Bienes(Cls_Ope_Pre_Pae_Bienes_Negocio Pae_Bienes)
        {
            DataTable Tabla = null;
            String Mi_SQL;

            try
            {
                if (Pae_Bienes.P_Campos_Dinamicos != null && Pae_Bienes.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Pae_Bienes.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT "
                        + Ope_Pre_Pae_Bienes.Campo_No_Bien
                        + ", " + Ope_Pre_Pae_Bienes.Campo_No_Peritaje
                        + ", " + Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id
                        + ", " + Ope_Pre_Pae_Bienes.Campo_Descripcion
                        + ", " + Ope_Pre_Pae_Bienes.Campo_Valor;
                    Mi_SQL += ", (SELECT " + Ope_Pre_Pae_Peritajes.Campo_Avaluo + " FROM "  // subconsulta AVALUO
                        + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + " WHERE "
                        + Ope_Pre_Pae_Peritajes.Campo_No_Peritaje + " = "
                        + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + "." + Ope_Pre_Pae_Bienes.Campo_No_Peritaje
                        + ") " + Ope_Pre_Pae_Peritajes.Campo_Avaluo;
                    Mi_SQL += ", (SELECT " + Cat_Pre_Tipos_Bienes.Campo_Descripcion + " FROM "  // subconsulta TIPO_BIEN
                        + Cat_Pre_Tipos_Bienes.Tabla_Cat_Pre_Tipos_Bienes + " WHERE "
                        + Cat_Pre_Tipos_Bienes.Campo_Tipo_Bien_Id + " = "
                        + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + "." + Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id
                        + ") TIPO_BIEN";
                    Mi_SQL += ", (SELECT LISTAGG(" + Ope_Pre_Pae_Imagenes_Bienes.Campo_Ruta_Imagen + ", ',') WITHIN GROUP (ORDER BY "  // subconsulta FOTOGRAFIAS
                        + Ope_Pre_Pae_Imagenes_Bienes.Campo_No_Imagen + ") FROM "
                        + Ope_Pre_Pae_Imagenes_Bienes.Tabla_Ope_Pre_Pae_Imagenes_Bienes + " WHERE "
                        + Ope_Pre_Pae_Imagenes_Bienes.Campo_No_Bien + " = "
                        + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + "." + Ope_Pre_Pae_Bienes.Campo_No_Bien
                        + ") FOTOGRAFIAS";
                }

                Mi_SQL += " FROM " + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + " WHERE ";

                // filtrar por NO_BIEN
                if (!string.IsNullOrEmpty(Pae_Bienes.P_No_Bien))
                {
                    Mi_SQL += Ope_Pre_Pae_Bienes.Campo_No_Bien + " = '" + Pae_Bienes.P_No_Bien + "' AND ";
                }
                // filtrar por NO_DETALLE_ETAPA
                if (!string.IsNullOrEmpty(Pae_Bienes.P_No_Detalle_Etapa))
                {
                    Mi_SQL += Ope_Pre_Pae_Bienes.Campo_No_Peritaje + " IN (SELECT "
                        + Ope_Pre_Pae_Peritajes.Campo_No_Peritaje + " FROM "
                        + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + " WHERE "
                        + Ope_Pre_Pae_Peritajes.Campo_No_Detalle_Etapa
                        + " = '" + Pae_Bienes.P_No_Detalle_Etapa + "') AND ";
                }
                // filtrar por NO_PERITAJE
                if (!string.IsNullOrEmpty(Pae_Bienes.P_No_Peritaje))
                {
                    Mi_SQL += Ope_Pre_Pae_Bienes.Campo_No_Peritaje + " = '" + Pae_Bienes.P_No_Peritaje + "' AND ";
                }
                // filtrar por TIPO_BIEN_ID
                if (!string.IsNullOrEmpty(Pae_Bienes.P_Tipo_Bien_ID))
                {
                    Mi_SQL += Ope_Pre_Pae_Bienes.Campo_Tipo_Bien_Id + " = '" + Pae_Bienes.P_Tipo_Bien_ID + "' AND ";
                }
                // filtrar por DESCRIPCION
                if (!string.IsNullOrEmpty(Pae_Bienes.P_Descripcion))
                {
                    Mi_SQL += Ope_Pre_Pae_Bienes.Campo_Descripcion + " LIKE '%" + Pae_Bienes.P_Descripcion + "%'";
                }

                // eliminar AND o WHERE al final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                // ORDER BY
                if (!string.IsNullOrEmpty(Pae_Bienes.P_Ordenar_Dinamico))
                {
                    Mi_SQL += " ORDER BY " + Pae_Bienes.P_Ordenar_Dinamico;
                }

                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Peritajes
        /// DESCRIPCIÓN: Forma la consulta de Peritajes con filtros opcionales por número de peritaje y detalle_etapa
        /// PARÁMETROS:
        /// 		1. Pae_Bienes: instancia de la clase de negocio con filtros para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 26-abr-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Peritajes(Cls_Ope_Pre_Pae_Bienes_Negocio Pae_Bienes)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;

            try
            {
                if (Pae_Bienes.P_Campos_Dinamicos != null && Pae_Bienes.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Pae_Bienes.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT " + Ope_Pre_Pae_Peritajes.Campo_No_Peritaje
                        + ", " + Ope_Pre_Pae_Peritajes.Campo_Avaluo
                        + ", NULLIF(TO_CHAR(" + Ope_Pre_Pae_Peritajes.Campo_Fecha_Peritaje + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"'),'01-Ene-0001') " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Peritaje
                        + ", " + Ope_Pre_Pae_Peritajes.Campo_Perito
                        + ", " + Ope_Pre_Pae_Peritajes.Campo_Valor
                        + ", " + Ope_Pre_Pae_Peritajes.Campo_Observaciones
                        + ", " + Ope_Pre_Pae_Peritajes.Campo_Lugar_Almacenamiento
                        + ", " + Ope_Pre_Pae_Peritajes.Campo_Costo_Metro_Cuadrado
                        + ", " + Ope_Pre_Pae_Peritajes.Campo_Dimensiones
                        + ", NULLIF(TO_CHAR(" + Ope_Pre_Pae_Peritajes.Campo_Fecha_Ingreso + ", 'dd-Mon-yyyy', 'NLS_LANGUAGE = \"MEXICAN SPANISH\"'),'01-Ene-0001') " + Ope_Pre_Pae_Peritajes.Campo_Fecha_Ingreso
                        + ", " + Ope_Pre_Pae_Peritajes.Campo_Tiempo_Transcurrido
                        + ", " + Ope_Pre_Pae_Peritajes.Campo_No_Detalle_Etapa;
                }

                Mi_SQL += " FROM " + Ope_Pre_Pae_Peritajes.Tabla_Ope_Pre_Pae_Peritajes + " WHERE ";

                // filtro por LUGAR ALMACENAMIENTO
                if (!string.IsNullOrEmpty(Pae_Bienes.P_Lugar_Almacenamiento))
                {
                    Mi_SQL += Ope_Pre_Pae_Peritajes.Campo_Lugar_Almacenamiento + " LIKE '%" + Pae_Bienes.P_Lugar_Almacenamiento + "%' AND ";
                }
                // filtro por FECHA_INGRESO
                if (Pae_Bienes.P_Fecha_Ingreso != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Pre_Pae_Peritajes.Campo_Fecha_Ingreso + " = '" + Pae_Bienes.P_Fecha_Ingreso.ToString("d-M-yyyy") + "' AND ";
                }
                // filtro por NO_PERITAJE
                if (!string.IsNullOrEmpty(Pae_Bienes.P_No_Peritaje))
                {
                    Mi_SQL += Ope_Pre_Pae_Peritajes.Campo_No_Peritaje + " = '" + Pae_Bienes.P_No_Peritaje + "' AND ";
                }
                // filtro por NO_DETALLE_ETAPA
                if (!string.IsNullOrEmpty(Pae_Bienes.P_No_Detalle_Etapa))
                {
                    Mi_SQL += Ope_Pre_Pae_Peritajes.Campo_No_Detalle_Etapa + " = '" + Pae_Bienes.P_No_Detalle_Etapa + "' AND ";
                }
                // filtro por FECHA_PERITAJE
                if (Pae_Bienes.P_Fecha_Peritaje != DateTime.MinValue)
                {
                    Mi_SQL += Ope_Pre_Pae_Peritajes.Campo_Fecha_Peritaje + " = '" + Pae_Bienes.P_Fecha_Peritaje.ToString("d-M-yyyy") + "' AND ";
                }
                // filtro por NO_BIEN
                if (!string.IsNullOrEmpty(Pae_Bienes.P_No_Bien))
                {
                    Mi_SQL += Ope_Pre_Pae_Peritajes.Campo_No_Peritaje + " = (SELECT "
                        + Ope_Pre_Pae_Bienes.Campo_No_Peritaje + " FROM "
                        + Ope_Pre_Pae_Bienes.Tabla_Ope_Pre_Pae_Bienes + " WHERE "
                        + Ope_Pre_Pae_Bienes.Campo_No_Bien + " = '" + Pae_Bienes.P_No_Bien + "') AND ";
                }
                // filtro por PERITO
                if (!string.IsNullOrEmpty(Pae_Bienes.P_Perito))
                {
                    Mi_SQL += Ope_Pre_Pae_Peritajes.Campo_Perito + " = '" + Pae_Bienes.P_Perito + "'";
                }

                // eliminar AND o WHERE al final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                // ORDER BY
                if (!string.IsNullOrEmpty(Pae_Bienes.P_Ordenar_Dinamico))
                {
                    Mi_SQL += " ORDER BY " + Pae_Bienes.P_Ordenar_Dinamico;
                }


                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Detalles_Etapas
        /// DESCRIPCIÓN: Generar consulta de Ope_Pre_Pae_Det_Etapas con los filtros que se reciben como parámetro
        /// PARÁMETROS:
        /// 		1. Pae_Etapas: Filtros para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 25-abr-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Detalles_Etapas(Cls_Ope_Pre_Pae_Bienes_Negocio Pae_Etapas)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;

            try
            {
                // si la propiedad Campos_Dinamicos contiene texto, formar consulta con este valor, si no, con campos por defecto
                if (!string.IsNullOrEmpty(Pae_Etapas.P_Campos_Dinamicos))
                {
                    Mi_SQL = "SELECT " + Pae_Etapas.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + ".* "
                        + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial
                        + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID
                        + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID
                        + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID
                        + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion
                        + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion;
                    // subconsulta número de entrega
                    Mi_SQL += ", (SELECT " + Ope_Pre_Pae_Etapas.Campo_Numero_Entrega + " FROM "
                        + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + " WHERE "
                        + Ope_Pre_Pae_Etapas.Campo_No_Etapa + " = "
                        + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa + ") ENTREGA";
                    // subconsulta despacho asignado
                    Mi_SQL += ", (SELECT " + Cat_Pre_Despachos_Externos.Campo_Despacho + " FROM "
                        + Cat_Pre_Despachos_Externos.Tabla_Cat_Pre_Despachos_Externos + " WHERE "
                        + Cat_Pre_Despachos_Externos.Campo_Despacho_Id + " = (SELECT " + Ope_Pre_Pae_Etapas.Campo_Despacho_Id + " FROM "
                        + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + " WHERE "
                        + Ope_Pre_Pae_Etapas.Campo_No_Etapa + " = "
                        + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa + ")) ASIGNADO";
                    // subconsulta fecha notificación
                    Mi_SQL += ", (SELECT " + Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora + " FROM "
                        + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + " WHERE "
                        + Ope_Pre_Pae_Notificaciones.Campo_No_Detalle_Etapa + " = "
                        + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " AND "
                        + Ope_Pre_Pae_Notificaciones.Campo_Proceso + " = '" + Pae_Etapas.P_Proceso_Actual + "') FECHA_NOTIFICACION";
                    // subconsulta honorarios
                    Mi_SQL += ", (SELECT NVL(SUM(" + Ope_Pre_Pae_Honorarios.Campo_Importe + "),0) FROM "
                        + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + " WHERE "
                        + Ope_Pre_Pae_Honorarios.Campo_No_Detalle_Etapa + " = "
                        + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + ") SUMA_HONORARIOS";
                    Mi_SQL += ", (" + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente + " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago;
                    Mi_SQL += " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios + " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios;
                    Mi_SQL += " + (SELECT NVL(SUM(" + Ope_Pre_Pae_Honorarios.Campo_Importe + "),0) FROM " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + " WHERE " + Ope_Pre_Pae_Honorarios.Campo_No_Detalle_Etapa + " = " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + ")";
                    Mi_SQL += " + " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Multas;
                    Mi_SQL += ") TOTAL";
                }

                Mi_SQL += " FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas;
                Mi_SQL += " INNER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL += " ON " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + "="
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " WHERE ";

                // filtro PROCESO_ACTUAL
                if (!string.IsNullOrEmpty(Pae_Etapas.P_Proceso_Actual))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Proceso_Actual + " = '" + Pae_Etapas.P_Proceso_Actual + "' AND ";
                }

                // filtro CUENTA_PREDIAL
                if (!string.IsNullOrEmpty(Pae_Etapas.P_Cuenta_Predial))
                {
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Pae_Etapas.P_Cuenta_Predial + "' AND ";
                }

                // filtro DESACHO_ID
                if (!string.IsNullOrEmpty(Pae_Etapas.P_Despacho_ID))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa
                        + " IN (SELECT " + Ope_Pre_Pae_Det_Etapas.Campo_No_Etapa + " FROM "
                        + Ope_Pre_Pae_Etapas.Tabla_Ope_Pre_Pae_Etapas + " WHERE "
                        + Ope_Pre_Pae_Etapas.Campo_Despacho_Id + "='"
                        + Pae_Etapas.P_Despacho_ID + "') AND ";
                }

                // filtro ESTATUS_ESTAPA
                if (!string.IsNullOrEmpty(Pae_Etapas.P_Estatus_Etapa))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Estatus + " = '" + Pae_Etapas.P_Estatus_Etapa + "' AND ";
                }

                // filtro ETAPA_OMITIDA
                if (!string.IsNullOrEmpty(Pae_Etapas.P_Etapa_Omitida))
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Omitida + " = '" + Pae_Etapas.P_Etapa_Omitida + "' AND ";
                }

                // filtro FOLIO_INICIAL
                if (Pae_Etapas.P_Folio_Inicial > 0)
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + " >= " + Pae_Etapas.P_Folio_Inicial + " AND ";
                }

                // filtro FOLIO_FINAL
                if (Pae_Etapas.P_Folio_Final > 0)
                {
                    Mi_SQL += Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_Folio + " <= " + Pae_Etapas.P_Folio_Final + " AND ";
                }

                // filtro FECHA_INICIAL
                if (Pae_Etapas.P_Fecha_Inicial != DateTime.MinValue)
                {
                    Mi_SQL += "(SELECT " + Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora + " FROM "
                        + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + " WHERE "
                        + Ope_Pre_Pae_Notificaciones.Campo_No_Detalle_Etapa + " = "
                        + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " AND "
                        + Ope_Pre_Pae_Notificaciones.Campo_Proceso + " = '" + Pae_Etapas.P_Proceso_Actual + "') >= TO_DATE('" + Pae_Etapas.P_Fecha_Inicial.ToString("dd/MM/yyyy") + "', 'DD/MM/YYYY') AND ";
                }

                // filtro FECHA_FINAL
                if (Pae_Etapas.P_Fecha_Final != DateTime.MinValue)
                {
                    Mi_SQL += "(SELECT " + Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora + " FROM "
                        + Ope_Pre_Pae_Notificaciones.Tabla_Ope_Pre_Pae_Notificaciones + " WHERE "
                        + Ope_Pre_Pae_Notificaciones.Campo_No_Detalle_Etapa + " = "
                        + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + "." + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " AND "
                        + Ope_Pre_Pae_Notificaciones.Campo_Proceso + " = '" + Pae_Etapas.P_Proceso_Actual + "') <= TO_DATE('" + Pae_Etapas.P_Fecha_Final.ToString("dd/MM/yyyy") + "', 'DD/MM/YYYY') AND ";
                }

                // eliminar AND o WHERE al final de la consulta
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                else if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }

                // agregar ORDER BY a la consulta
                if (!string.IsNullOrEmpty(Pae_Etapas.P_Ordenar_Dinamico))
                {
                    Mi_SQL += " ORDER BY " + Pae_Etapas.P_Ordenar_Dinamico;
                }

                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Depositarios
        ///DESCRIPCIÓN: Obtiene registro de la tablas Ope_Pre_Pae_Depositarios
        ///PARAMETROS:      
        ///CREO: Armando Zavala Moreno.
        ///FECHA_CREO: 21/Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consulta_Depositarios(Cls_Ope_Pre_Pae_Bienes_Negocio Pae_Bienes)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            Boolean Segundo_Filtro = false;
            try
            {
                if (Pae_Bienes.P_Campos_Dinamicos != null && Pae_Bienes.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Pae_Bienes.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT " + Ope_Pre_Pae_Depositarios.Campo_No_Cambio_Depositario + ", " + Ope_Pre_Pae_Depositarios.Campo_Figura;
                    Mi_SQL += ", " + Ope_Pre_Pae_Depositarios.Campo_Nombre_Depositario + ", " + Ope_Pre_Pae_Depositarios.Campo_Domicilio_Depositario;
                    Mi_SQL += ", " + Ope_Pre_Pae_Depositarios.Campo_Fecha_Remocion;
                }
                Mi_SQL += " FROM " + Ope_Pre_Pae_Depositarios.Tabla_Ope_Pre_Pae_Depositarios;
                if (Pae_Bienes.P_No_Depositario != null && Pae_Bienes.P_No_Depositario != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Depositarios.Campo_No_Cambio_Depositario + " = '" + Pae_Bienes.P_No_Depositario + "'";
                }
                if (Pae_Bienes.P_No_Detalle_Etapa != null && Pae_Bienes.P_No_Detalle_Etapa != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Depositarios.Campo_No_Detalle_Etapa + " = '" + Pae_Bienes.P_No_Detalle_Etapa + "'";
                }
                if (Pae_Bienes.P_Figura != null && Pae_Bienes.P_Figura != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Depositarios.Campo_Figura + " = '" + Pae_Bienes.P_Figura + "'";
                }
                if (Pae_Bienes.P_Nombre_Depositario != null && Pae_Bienes.P_Nombre_Depositario != "")
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Depositarios.Campo_Nombre_Depositario + " = '" + Pae_Bienes.P_Nombre_Depositario + "'";
                }
                if (Pae_Bienes.P_Fecha_Remocion != DateTime.MinValue)
                {
                    if (Segundo_Filtro == true)
                    {
                        Mi_SQL += " AND ";
                    }
                    else
                    {
                        Mi_SQL += " WHERE ";
                    }
                    Segundo_Filtro = true;
                    Mi_SQL += Ope_Pre_Pae_Depositarios.Campo_Fecha_Remocion + " = '" + Pae_Bienes.P_Fecha_Remocion.ToString("d-M-yyyy") + "'";
                }
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet != null)
                {
                    Tabla = dataSet.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la Cuentas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
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
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID)
        {
            String Id = "0000000001";
            try
            {
                String Mi_SQL = "SELECT TO_NUMBER(NVL(MAX(" + Campo + "),0))+1 FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Obj_Temp.ToString().PadLeft(10, '0');
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
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
    }
}