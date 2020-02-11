using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.PSP_SAP_2013.Negocio;
using System.Data;
using System.Text;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;

namespace Presidencia.PSP_SAP_2013.Datos
{
    
    public class Cls_Ope_Psp_Actualizar_Presupuesto_2013_Datos
    {
	    public Cls_Ope_Psp_Actualizar_Presupuesto_2013_Datos()
	    {
		    
	    }
        ///*******************************************************************************
        ///METODOS
        ///*******************************************************************************
        #region METODOS
        
        /// *************************************************************************************
        /// NOMBRE:              Actualizar_Presupuesto
        /// DESCRIPCIÓN:         Metodo que realiza las sentencias SQL para actualizar el Presupuesto.
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          16/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        public static String Actualizar_Presupuesto(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {
            DataTable Dt_Aux = new DataTable();
            String Resultado = "";
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Presupuesto = Clase_Negocio.P_Dt_Presupuesto;
            Int32 Num_Registros = 0;
            int registro = 0;
            int aux_i = 0;

            //INSERTAR LA REQUISICION   
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            Mi_SQL.Append("DELETE " + Ope_Sap_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto); 
            Cmd.CommandText = Mi_SQL.ToString();
            registro = Cmd.ExecuteNonQuery();

            try
            {
                for (int i = 0; i < Dt_Presupuesto.Rows.Count; i++)
                {
                    aux_i = i;//variable que sirve para identificar que linea del excel esta leyendo
                    if (Num_Registros == 2074)
                    {
                        String a = "";
                    }
                    if (Dt_Presupuesto.Rows[i][0].ToString().Trim() != String.Empty)
                    {
                        //MASCARA DEL CODIGO PROGRAMATICO
                        //Fuente Financiamiento- Area Funcional- Programa- Centro Gestor- Centro de Costos- Partida
                        //FF-AF-PRO-CG-CC-PARTIDA
                        Clase_Negocio = new Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio();
                        Clase_Negocio.P_Clave_Fuente_F = Dt_Presupuesto.Rows[i][0].ToString().Trim();
                        Clase_Negocio.P_Clave_Area_F = Dt_Presupuesto.Rows[i][1].ToString().Trim();
                        Clase_Negocio.P_Clave_Programa = Dt_Presupuesto.Rows[i][2].ToString().Trim();
                        Clase_Negocio.P_Clave_Centro_Gestor = Dt_Presupuesto.Rows[i][3].ToString().Trim();
                        Clase_Negocio.P_Clave_Centro_Costos = Dt_Presupuesto.Rows[i][4].ToString().Trim();
                        Clase_Negocio.P_Clave_Partida = Dt_Presupuesto.Rows[i][5].ToString().Trim();
                        //Asignamos los Montos del Presupuesto a variables a atributos de Clase_Negocio                    
                        Clase_Negocio.P_Monto_Presupuestal = (Dt_Presupuesto.Rows[i][7].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][7].ToString().Replace(",", "").Trim() : "0");
                        Clase_Negocio.P_Monto_Disponible = (Dt_Presupuesto.Rows[i][10].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][10].ToString().Replace(",", "").Trim() : "0");
                        Clase_Negocio.P_Monto_Comprometido = (Dt_Presupuesto.Rows[i][9].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][9].ToString().Replace(",", "").Trim() : "0");
                        Clase_Negocio.P_Monto_Ejercido = (Dt_Presupuesto.Rows[i][8].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][8].ToString().Replace(",", "").Trim() : "0");

                        //PASO 1.- Consultamos si ya Existe la Fuente de Financiamiento
                        Dt_Aux = Consultar_FF(Clase_Negocio);
                        if (Dt_Aux.Rows.Count > 0)
                        {
                            Clase_Negocio.P_Fuente_F_ID = Dt_Aux.Rows[0][0].ToString().Trim();
                        }
                        else
                        {
                            //insertamos la FF y nos traemos el ID
                            Clase_Negocio.P_Fuente_F_ID = Insertar_FF(Clase_Negocio);

                        }
                        //PASO 2.- Consultamos si ya Existe el Area Funcional
                        Dt_Aux = Consultar_AF(Clase_Negocio);
                        if (Dt_Aux.Rows.Count > 0)
                        {
                            Clase_Negocio.P_Area_F_ID = Dt_Aux.Rows[0][0].ToString().Trim();
                        }
                        else
                        {
                            //insertamos el AF y nos traemos el ID
                            Clase_Negocio.P_Area_F_ID = Insertar_AF(Clase_Negocio);
                        }
                        //PASO 3.- Consultamos si ya Existe el Programa
                        Dt_Aux = Consultar_Programa(Clase_Negocio);
                        if (Dt_Aux.Rows.Count > 0)
                        {
                            Clase_Negocio.P_Programa_ID = Dt_Aux.Rows[0][0].ToString().Trim();
                        }
                        else
                        {
                            //insertamos el Programa y nos traemos el ID
                            Clase_Negocio.P_Programa_ID = Insertar_Programa(Clase_Negocio);

                        }
                        //PASO 4.- Consultamos si ya Existe el Centro Gestor
                        Dt_Aux = Consultar_Centro_G(Clase_Negocio);
                        if (Dt_Aux.Rows.Count > 0)
                        {
                            Clase_Negocio.P_Centro_Gestor_ID = Dt_Aux.Rows[0][0].ToString().Trim();
                        }
                        else
                        {
                            //insertamos el Centro Gestor y nos traemos el ID
                            Clase_Negocio.P_Centro_Gestor_ID = Insertar_Centro_G(Clase_Negocio);
                        }
                        //PASO 5.- Consultamos si ya Existe el Centro de Costos
                        Dt_Aux = Consultar_Centro_C(Clase_Negocio);
                        if (Dt_Aux.Rows.Count > 0)
                        {
                            Clase_Negocio.P_Centro_Costos_ID = Dt_Aux.Rows[0][0].ToString().Trim();
                        }
                        else
                        {
                            //insertamos el Centro de Costos y nos traemos el ID
                            Clase_Negocio.P_Centro_Costos_ID = Insertar_Centro_C(Clase_Negocio);
                        }
                        //PASO 6.- Consultamos si ya Existe la Partida
                        Dt_Aux = Consultar_Partidas(Clase_Negocio);
                        if (Dt_Aux.Rows.Count > 0)
                        {
                            Clase_Negocio.P_Partida_ID = Dt_Aux.Rows[0][0].ToString().Trim();
                        }
                        else
                        {
                            //insertamos el Programa y nos traemos el ID
                            Clase_Negocio.P_Partida_ID = Insertar_Partida(Clase_Negocio);
                        }
                        //en este momento ya tenemos todo el codigo   Programatico guardado en Clase_Negocio
                        //consultamos si existe el registro en la tabla OPE_SAP_DEP_PRESUPUESTO si es asi solo actualizamos 
                        //Dt_Aux = Consultar_Presupuesto(Clase_Negocio);
                        Dt_Aux = new DataTable();
                        if (Dt_Aux.Rows.Count > 0)
                        {
                            Crear_Relaciones(Clase_Negocio);
                            //Actualizamos
                            Mi_SQL = new StringBuilder();
                            Mi_SQL.Append("UPDATE " + Ope_Sap_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto);
                            Mi_SQL.Append(" set " + Ope_Sap_Dep_Presupuesto.Campo_Monto_Comprometido_Real);
                            Mi_SQL.Append(" = " + Clase_Negocio.P_Monto_Comprometido);
                            Mi_SQL.Append(", " + Ope_Sap_Dep_Presupuesto.Campo_Monto_Ejercido);
                            Mi_SQL.Append(" = " + Clase_Negocio.P_Monto_Ejercido);
                            Mi_SQL.Append(", " + Ope_Sap_Dep_Presupuesto.Campo_Monto_Disponible);
                            Mi_SQL.Append(" = " + Clase_Negocio.P_Monto_Disponible);
                            Mi_SQL.Append(", " + Ope_Sap_Dep_Presupuesto.Campo_Monto_Presupuestal);
                            Mi_SQL.Append(" = " + Clase_Negocio.P_Monto_Presupuestal);
                            Mi_SQL.Append(" WHERE " + Ope_Sap_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + "='" + Clase_Negocio.P_Fuente_F_ID.Trim() + "'");
                            Mi_SQL.Append(" AND " + Ope_Sap_Dep_Presupuesto.Campo_Area_Funcional_ID + "='" + Clase_Negocio.P_Area_F_ID.Trim() + "'");
                            Mi_SQL.Append(" AND " + Ope_Sap_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "='" + Clase_Negocio.P_Programa_ID.Trim() + "'");
                            Mi_SQL.Append(" AND " + Ope_Sap_Dep_Presupuesto.Campo_Grupo_Dependencia_ID + "='" + Clase_Negocio.P_Centro_Gestor_ID.Trim() + "'");
                            Mi_SQL.Append(" AND " + Ope_Sap_Dep_Presupuesto.Campo_Dependencia_ID + "='" + Clase_Negocio.P_Centro_Costos_ID.Trim() + "'");
                            Mi_SQL.Append(" AND " + Ope_Sap_Dep_Presupuesto.Campo_Partida_ID + "='" + Clase_Negocio.P_Partida_ID.Trim() + "'");
                            Mi_SQL.Append(" AND " + Ope_Sap_Dep_Presupuesto.Campo_Anio_Presupuesto + "=" + DateTime.Now.Year);
                            Cmd.CommandText = Mi_SQL.ToString();
                            registro = Cmd.ExecuteNonQuery();
                            //registro = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                            Num_Registros = Num_Registros + registro;

                        }
                        else
                        {
                            //PASO 7.- En caso de no Existir hacemos el Insert
                            //Se crean las relaciones en las tablas de detalle
                            Crear_Relaciones(Clase_Negocio);

                            //Insertamos los valores del Presupuesto en OPE_SAP_DEP_PRESUPUESTO
                            Mi_SQL = new StringBuilder();
                            Mi_SQL.Append("INSERT INTO " + Ope_Sap_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto);
                            Mi_SQL.Append("(" + Ope_Sap_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Area_Funcional_ID);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Proyecto_Programa_ID);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Grupo_Dependencia_ID);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Dependencia_ID);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Partida_ID);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Anio_Presupuesto);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Monto_Presupuestal);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Monto_Disponible);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Monto_Comprometido);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Monto_Ejercido);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_No_Asignacion_Anio);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Fecha_Asignacion);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Fecha_Creo);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Usuario_Creo);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Monto_Ampliacion);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Monto_Reduccion);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Monto_Modificado);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Monto_Devengado);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Monto_Devengado_Pagado);
                            Mi_SQL.Append("," + Ope_Sap_Dep_Presupuesto.Campo_Monto_Comprometido_Real);


                            Mi_SQL.Append(") VALUES (");
                            Mi_SQL.Append("'" + Clase_Negocio.P_Fuente_F_ID + "'");
                            Mi_SQL.Append(",'" + Clase_Negocio.P_Area_F_ID + "'");
                            Mi_SQL.Append(",'" + Clase_Negocio.P_Programa_ID + "'");
                            Mi_SQL.Append(",'" + Clase_Negocio.P_Centro_Gestor_ID + "'");
                            Mi_SQL.Append(",'" + Clase_Negocio.P_Centro_Costos_ID + "'");
                            Mi_SQL.Append(",'" + Clase_Negocio.P_Partida_ID + "'");
                            Mi_SQL.Append("," + DateTime.Now.Year);
                            Mi_SQL.Append("," + Clase_Negocio.P_Monto_Presupuestal);
                            Mi_SQL.Append("," + Clase_Negocio.P_Monto_Disponible);
                            Mi_SQL.Append("," + Clase_Negocio.P_Monto_Comprometido);
                            Mi_SQL.Append("," + Clase_Negocio.P_Monto_Ejercido);
                            Mi_SQL.Append(",1");
                            Mi_SQL.Append(",SYSDATE");
                            Mi_SQL.Append(",SYSDATE");
                            Mi_SQL.Append(",'" + Cls_Sessiones.Nombre_Empleado + "'");
                            Mi_SQL.Append(",0");
                            Mi_SQL.Append(",0");
                            Mi_SQL.Append(",0");
                            Mi_SQL.Append(",0");
                            Mi_SQL.Append(",0");
                            Mi_SQL.Append("," + Clase_Negocio.P_Monto_Comprometido + ")");
                            Cmd.CommandText = Mi_SQL.ToString();
                            registro = Cmd.ExecuteNonQuery();
                            //registro = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                            Num_Registros = Num_Registros + registro;

                        }



                    }//Fin del If que valida si se tienen datos en FF



                }//fin del for
                //en caso de Ser actualizado el presupuesto
                Resultado = "Presupuesto Actualizado.  " + (Num_Registros + 1) + " Registros Actualizados.";
                Trans.Commit();
            }
            catch (Exception e)
            {
                Resultado = "No se pudo actualizar el presupuesto al 100% ver el registro en la linea " + (aux_i + 1) + ", solo se actualizaron " + (Num_Registros + 1) + " partidas . ERROR: " + e.Message.ToString();
                Trans.Commit();
                //Trans.Rollback();
            }
            finally
            {
                Cn.Close();

            }
            
            return Resultado;        
            
        }
        
          /// *************************************************************************************
        /// NOMBRE:              Crear_Relaciones
        /// DESCRIPCIÓN:         Metodo que ccrea las relaciones al presupuesto
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          16/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        public static bool Crear_Relaciones(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio )
        {
            bool Operacion_Realizada = false;
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Aux = new DataTable();
            int registros = 0;
            //1.- CAT_SAP_DET_PROG_DEPENDENCIA
            Mi_SQL.Append("SELECT * FROM CAT_SAP_DET_PROG_DEPENDENCIA");
            Mi_SQL.Append(" WHERE DEPENDENCIA_ID ='" + Clase_Negocio.P_Centro_Costos_ID + "'");
            Mi_SQL.Append(" AND PROYECTO_PROGRAMA_ID='" + Clase_Negocio.P_Programa_ID + "'");

            Dt_Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            if (Dt_Aux.Rows.Count == 0)
            {
                //Insertamos la relacion
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("INSERT INTO CAT_SAP_DET_PROG_DEPENDENCIA ");
                Mi_SQL.Append("(DEPENDENCIA_ID,PROYECTO_PROGRAMA_ID) VALUES ");
                Mi_SQL.Append("('" + Clase_Negocio.P_Centro_Costos_ID + "'");
                Mi_SQL.Append(",'" + Clase_Negocio.P_Programa_ID + "')");
                registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                if (registros > 0)
                    Operacion_Realizada = true;
                
            }

            //Relación entre Fuentes de Financiamiento y UR
            //2.- CAT_SAP_DET_FTE_DEPENDENCIA
            Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT * FROM CAT_SAP_DET_FTE_DEPENDENCIA");
            Mi_SQL.Append(" WHERE DEPENDENCIA_ID ='" + Clase_Negocio.P_Centro_Costos_ID + "'" );
            Mi_SQL.Append(" AND FUENTE_FINANCIAMIENTO_ID = '" + Clase_Negocio.P_Fuente_F_ID + "'");

            Dt_Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            if (Dt_Aux.Rows.Count == 0)
            {
                //Insertamos la relacion si no existe
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("INSERT INTO CAT_SAP_DET_FTE_DEPENDENCIA ");
                Mi_SQL.Append("(DEPENDENCIA_ID,FUENTE_FINANCIAMIENTO_ID) VALUES ");
                Mi_SQL.Append("('" + Clase_Negocio.P_Centro_Costos_ID + "'");
                Mi_SQL.Append(",'" + Clase_Negocio.P_Fuente_F_ID + "')");
                registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                if (registros > 0)
                {
                    Operacion_Realizada = true;
                }
            }
            //Relación entre Programas y Partidas
            //3.- CAT_SAP_DET_PROG_PARTIDAS
            Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT * FROM CAT_SAP_DET_PROG_PARTIDAS");
            Mi_SQL.Append(" WHERE PROYECTO_PROGRAMA_ID = '" + Clase_Negocio.P_Programa_ID + "'");
            Mi_SQL.Append(" AND PARTIDA_ID = '" + Clase_Negocio.P_Partida_ID + "'");
            Dt_Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            if (Dt_Aux.Rows.Count == 0)
            {
                //Insertamos la relacion si no existe
                Mi_SQL = new StringBuilder();
                String ID = Obtener_Consecutivo("DET_PROG_PARTIDAS_ID", "CAT_SAP_DET_PROG_PARTIDAS","00000");
                Mi_SQL.Append("INSERT INTO CAT_SAP_DET_PROG_PARTIDAS");
                Mi_SQL.Append("(DET_PROG_PARTIDAS_ID,PROYECTO_PROGRAMA_ID,PARTIDA_ID) VALUES");

                Mi_SQL.Append("('" + ID + "'");
                Mi_SQL.Append(",'" + Clase_Negocio.P_Programa_ID + "'");
                Mi_SQL.Append(",'" + Clase_Negocio.P_Partida_ID + "')");
                registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                if (registros > 0)
                {
                    Operacion_Realizada = true;
                }
            }  
            return Operacion_Realizada;
        }

        /// *************************************************************************************
        /// NOMBRE:              Consultar_FF
        /// DESCRIPCIÓN:         Metodo que consulta una FF
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          16/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        public static DataTable Consultar_Presupuesto(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT *  FROM " + Ope_Sap_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto);
            Mi_SQL.Append(" WHERE " + Ope_Sap_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + "='" + Clase_Negocio.P_Fuente_F_ID.Trim() + "'");
            Mi_SQL.Append(" AND " + Ope_Sap_Dep_Presupuesto.Campo_Area_Funcional_ID + "='" + Clase_Negocio.P_Area_F_ID.Trim() + "'");
            Mi_SQL.Append(" AND " + Ope_Sap_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "='" + Clase_Negocio.P_Programa_ID.Trim() + "'");
            Mi_SQL.Append(" AND " + Ope_Sap_Dep_Presupuesto.Campo_Grupo_Dependencia_ID + "='" + Clase_Negocio.P_Centro_Gestor_ID.Trim() + "'");
            Mi_SQL.Append(" AND " + Ope_Sap_Dep_Presupuesto.Campo_Dependencia_ID + "='" + Clase_Negocio.P_Centro_Costos_ID.Trim() + "'");
            Mi_SQL.Append(" AND " + Ope_Sap_Dep_Presupuesto.Campo_Partida_ID + "='" + Clase_Negocio.P_Partida_ID.Trim() + "'");
            Mi_SQL.Append(" AND " + Ope_Sap_Dep_Presupuesto.Campo_Anio_Presupuesto + "=" + DateTime.Now.Year);


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0]; ;
        }

        /// *************************************************************************************
        /// NOMBRE:              Consultar_FF
        /// DESCRIPCIÓN:         Metodo que consulta una FF
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          16/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        public static DataTable Consultar_FF(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {

            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
            Mi_SQL.Append(" WHERE " + Cat_SAP_Fuente_Financiamiento.Campo_Clave + "='" + Clase_Negocio.P_Clave_Fuente_F.Trim() +"'");


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0]; ;
        }

        /// *************************************************************************************
        /// NOMBRE:              Insertar_FF
        /// DESCRIPCIÓN:         Metodo que inserta una FF
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          16/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        public static String Insertar_FF(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {
            String Fuente_F_ID = Obtener_Consecutivo(Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID.ToString(), Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento.ToString(),"00000");

            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("INSERT INTO " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento );
            Mi_SQL.Append(" (" + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
            Mi_SQL.Append(", " + Cat_SAP_Fuente_Financiamiento.Campo_Clave);
            Mi_SQL.Append(", " + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion);
            Mi_SQL.Append(", " + Cat_SAP_Fuente_Financiamiento.Campo_Usuario_Creo);
            Mi_SQL.Append(", " + Cat_SAP_Fuente_Financiamiento.Campo_Fecha_Creo);
            Mi_SQL.Append(", " + Cat_SAP_Fuente_Financiamiento.Campo_Estatus);
            Mi_SQL.Append(", " + Cat_SAP_Fuente_Financiamiento.Campo_Anio);
            Mi_SQL.Append(", " + Cat_SAP_Fuente_Financiamiento.Campo_Especiales_Ramo_33);
            Mi_SQL.Append(") VALUES(");
            Mi_SQL.Append("'" + Fuente_F_ID + "'");
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Fuente_F.Trim() + "'");//clave
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Fuente_F.Trim() + "'");//Descripcion
            Mi_SQL.Append(",'" + Cls_Sessiones.Nombre_Empleado + "'");//Usuario_ Creo
            Mi_SQL.Append(",SYSDATE");//Fecha
            Mi_SQL.Append(",'ACTIVO'");//Estatus
            Mi_SQL.Append("," + DateTime.Now.Year);//aanio
            Mi_SQL.Append(",'NO')");
            
            
            int registros  = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (registros == 0)
            {
                Fuente_F_ID = "";
            }
            
            return Fuente_F_ID;
        }

        /// *************************************************************************************
        /// NOMBRE:              Consultar_AF
        /// DESCRIPCIÓN:         Metodo que consulta una AF
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          16/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        public static DataTable Consultar_AF(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {

            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT " + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID + " FROM " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);
            Mi_SQL.Append(" WHERE " + Cat_SAP_Area_Funcional.Campo_Clave + "='" + Clase_Negocio.P_Clave_Area_F.Trim() + "'");


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0]; ;
        }

        /// *************************************************************************************
        /// NOMBRE:              Insertar_AF
        /// DESCRIPCIÓN:         Metodo que inserta una AF
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          16/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        public static String Insertar_AF(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {
            String Area_F_ID = Obtener_Consecutivo(Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID.ToString(), Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional.ToString(),"00000");

            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("INSERT INTO " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional);
            Mi_SQL.Append(" (" + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID);
            Mi_SQL.Append(", " + Cat_SAP_Area_Funcional.Campo_Clave);
            Mi_SQL.Append(", " + Cat_SAP_Area_Funcional.Campo_Descripcion);
            Mi_SQL.Append(", " + Cat_SAP_Area_Funcional.Campo_Estatus);
            Mi_SQL.Append(", " + Cat_SAP_Area_Funcional.Campo_Usuario_Creo);
            Mi_SQL.Append(", " + Cat_SAP_Area_Funcional.Campo_Fecha_Creo);
            Mi_SQL.Append(", " + Cat_SAP_Area_Funcional.Campo_Anio);
            Mi_SQL.Append(") VALUES(");
            Mi_SQL.Append("'" + Area_F_ID + "'");
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Area_F.Trim() + "'");//clave
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Area_F.Trim() + "'");//Descripcion
            Mi_SQL.Append(",'ACTIVO'");//Estatus
            Mi_SQL.Append(",'" + Cls_Sessiones.Nombre_Empleado + "'");//Usuario_ Creo
            Mi_SQL.Append(",SYSDATE");//Fecha            
            Mi_SQL.Append("," + DateTime.Now.Year + ")");//aanio


            int registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (registros == 0)
            {
                Area_F_ID = "";
            }

            return Area_F_ID;
        }

        /// *************************************************************************************
        /// NOMBRE:              Consultar_Programa
        /// DESCRIPCIÓN:         Metodo que consulta un Programa
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          17/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        public static DataTable Consultar_Programa(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {

            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT " + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id + " FROM " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas);
            Mi_SQL.Append(" WHERE " + Cat_Sap_Proyectos_Programas.Campo_Clave + "='" + Clase_Negocio.P_Clave_Programa.Trim() + "'");


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0]; ;
        }

        /// *************************************************************************************
        /// NOMBRE:              Insertar_Programa
        /// DESCRIPCIÓN:         Metodo que inserta un programa
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          17/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        public static String Insertar_Programa(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {
            String Programa_ID = Obtener_Consecutivo(Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id.ToString(), Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas.ToString(), "0000000000");

            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("INSERT INTO " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas);
            Mi_SQL.Append(" (" + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id);
            Mi_SQL.Append(", " + Cat_Sap_Proyectos_Programas.Campo_Descripcion);
            Mi_SQL.Append(", " + Cat_Sap_Proyectos_Programas.Campo_Usuario_Creo);
            Mi_SQL.Append(", " + Cat_Sap_Proyectos_Programas.Campo_Fecha_Creo);
            Mi_SQL.Append(", " + Cat_Sap_Proyectos_Programas.Campo_Estatus);            
            Mi_SQL.Append(", " + Cat_Sap_Proyectos_Programas.Campo_Clave);
            Mi_SQL.Append(", " + Cat_Sap_Proyectos_Programas.Campo_Nombre);
            Mi_SQL.Append(") VALUES(");
            Mi_SQL.Append("'" + Programa_ID + "'");
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Programa.Trim() + "'");//Descripcion
            Mi_SQL.Append(",'" + Cls_Sessiones.Nombre_Empleado + "'");//Usuario_ Creo
            Mi_SQL.Append(",SYSDATE");//Fecha
            Mi_SQL.Append(",'ACTIVO'");//Estatus
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Programa.Trim() + "'");//Clave
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Programa.Trim() + "')");//Nombre
            int registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (registros == 0)
            {
                Programa_ID = "";
            }

            return Programa_ID;
        }


        /// *************************************************************************************
        /// NOMBRE:              Consultar_Centro_G
        /// DESCRIPCIÓN:         Metodo que consulta un Centro Gestor
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          17/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        public static DataTable Consultar_Centro_G(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {

            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT " + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID + " FROM " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias);
            Mi_SQL.Append(" WHERE " + Cat_Grupos_Dependencias.Campo_Clave + "='" + Clase_Negocio.P_Clave_Centro_Gestor.Trim() + "'");


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0]; ;
        }

        /// *************************************************************************************
        /// NOMBRE:              Insertar_Centro_G
        /// DESCRIPCIÓN:         Metodo que inserta un centro gestor(Grupo dependencia)
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          17/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        public static String Insertar_Centro_G(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {
            String Centro_G_ID = Obtener_Consecutivo(Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID.ToString(), Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias.ToString(), "00000");

            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("INSERT INTO " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias);
            Mi_SQL.Append(" (" + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID);
            Mi_SQL.Append(", " + Cat_Grupos_Dependencias.Campo_Clave);
            Mi_SQL.Append(", " + Cat_Grupos_Dependencias.Campo_Nombre);
            Mi_SQL.Append(", " + Cat_Grupos_Dependencias.Campo_Comentarios);
            Mi_SQL.Append(", " + Cat_Grupos_Dependencias.Campo_Usuario_Creo);
            Mi_SQL.Append(", " + Cat_Grupos_Dependencias.Campo_Fecha_Creo);
            Mi_SQL.Append(", " + Cat_Grupos_Dependencias.Campo_Estatus);
            Mi_SQL.Append(") VALUES(");
            Mi_SQL.Append("'" + Centro_G_ID + "'");
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Centro_Gestor.Trim() + "'");//clave
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Centro_Gestor.Trim() + "'");//Nombre
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Centro_Gestor.Trim() + "'");//Comentarios
            Mi_SQL.Append(",'" + Cls_Sessiones.Nombre_Empleado + "'");//Usuario_ Creo
            Mi_SQL.Append(",SYSDATE");//Fecha
            Mi_SQL.Append(",'ACTIVO')");//Estatus
            int registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (registros == 0)
            {
                Centro_G_ID = "";
            }

            return Centro_G_ID;
        }


        /// *************************************************************************************
        /// NOMBRE:              Consultar_Centro_C
        /// DESCRIPCIÓN:         Metodo que consulta un Centro de Costos(Unidad Responsable)
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          17/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        /// 
        public static DataTable Consultar_Centro_C(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {

            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias);
            Mi_SQL.Append(" WHERE " + Cat_Dependencias.Campo_Clave + "='" + Clase_Negocio.P_Clave_Centro_Costos.Trim() + "'");


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0]; ;
        }

        /// *************************************************************************************
        /// NOMBRE:              Insertar_Centro_C
        /// DESCRIPCIÓN:         Metodo que inserta un Centro de Costos(Unidad Responsable)
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          17/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        
        public static String Insertar_Centro_C(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {
            String Centro_C_ID = Obtener_Consecutivo(Cat_Dependencias.Campo_Dependencia_ID.ToString(), Cat_Dependencias.Tabla_Cat_Dependencias.ToString(), "00000");

            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("INSERT INTO " + Cat_Dependencias.Tabla_Cat_Dependencias);
            Mi_SQL.Append(" (" + Cat_Dependencias.Campo_Dependencia_ID);
            Mi_SQL.Append(", " + Cat_Dependencias.Campo_Clave);
            Mi_SQL.Append(", " + Cat_Dependencias.Campo_Nombre);
            Mi_SQL.Append(", " + Cat_Dependencias.Campo_Estatus);
            Mi_SQL.Append(", " + Cat_Dependencias.Campo_Comentarios);
            Mi_SQL.Append(", " + Cat_Dependencias.Campo_Usuario_Creo);
            Mi_SQL.Append(", " + Cat_Dependencias.Campo_Fecha_Creo);
            Mi_SQL.Append(", " + Cat_Dependencias.Campo_Area_Funcional_ID);
            Mi_SQL.Append(") VALUES(");
            Mi_SQL.Append("'" + Centro_C_ID + "'");
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Centro_Costos.Trim() + "'");//Clave
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Centro_Costos.Trim() + "'");//Nombre
            Mi_SQL.Append(",'ACTIVO'");//Estatus
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Centro_Costos.Trim() + "'");//Comentarios
            Mi_SQL.Append(",'" + Cls_Sessiones.Nombre_Empleado + "'");//Usuario_ Creo
            Mi_SQL.Append(",SYSDATE");//Fecha
            Mi_SQL.Append(",'" +Clase_Negocio.P_Area_F_ID +"')");
           
            int registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (registros == 0)
            {
                Centro_C_ID = "";
            }

            return Centro_C_ID;
        }


        /// *************************************************************************************
        /// NOMBRE:              Consultar_Partidas
        /// DESCRIPCIÓN:         Metodo que consulta las partidas especificas
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          17/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************
        /// 
        public static DataTable Consultar_Partidas(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {

            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
            Mi_SQL.Append(" WHERE " + Cat_Sap_Partidas_Especificas.Campo_Clave + "='" + Clase_Negocio.P_Clave_Partida.Trim() + "'");


            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0]; ;
        }

        /// *************************************************************************************
        /// NOMBRE:              Insertar_Partida
        /// DESCRIPCIÓN:         Metodo que inserta un las partidas especificas
        /// PARÁMETROS:          1.- Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio
        /// USUARIO CREO:        Susana Trigueros Armenta 
        /// FECHA CREO:          17/ENE/13
        /// USUARIO MODIFICO:    
        /// FECHA MODIFICO:      
        /// CAUSA MODIFICACIÓN:  
        /// *************************************************************************************

        public static String Insertar_Partida(Cls_Ope_Psp_Actualizar_Presupuesto_2013_Negocio Clase_Negocio)
        {
            String Partida_ID = Obtener_Consecutivo(Cat_Sap_Partidas_Especificas.Campo_Partida_ID.ToString(), Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas.ToString(), "0000000000");

            StringBuilder Mi_SQL = new StringBuilder();
            Mi_SQL.Append("INSERT INTO " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
            Mi_SQL.Append(" (" + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
            Mi_SQL.Append(", " + Cat_Sap_Partidas_Especificas.Campo_Estatus);
            Mi_SQL.Append(", " + Cat_Sap_Partidas_Especificas.Campo_Descripcion);
            Mi_SQL.Append(", " + Cat_Sap_Partidas_Especificas.Campo_Usuario_Creo);
            Mi_SQL.Append(", " + Cat_Sap_Partidas_Especificas.Campo_Fecha_Creo);
            Mi_SQL.Append(", " + Cat_Sap_Partidas_Especificas.Campo_Nombre);
            Mi_SQL.Append(", " + Cat_Sap_Partidas_Especificas.Campo_Clave);
            Mi_SQL.Append(") VALUES(");
            Mi_SQL.Append("'" + Partida_ID + "'");
            Mi_SQL.Append(",'ACTIVO'");//Estatus
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Partida.Trim() + "'");//Descripcion 
            Mi_SQL.Append(",'" + Cls_Sessiones.Nombre_Empleado + "'");//Usuario_ Creo
            Mi_SQL.Append(",SYSDATE");//Fecha
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Partida.Trim() + "'");//Nombre
            Mi_SQL.Append(",'" + Clase_Negocio.P_Clave_Partida.Trim() + "')");//Clave            

            int registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (registros == 0)
            {
                Partida_ID = "";
            }

            return Partida_ID;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-P_Nombre de la tabla
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 16/Enero/2013
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Obtener_Consecutivo(String Campo_ID, String Tabla,String Mascara)
        {
            int Consecutivo = 0;
            String Mi_Sql;
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),'" + Mascara+ "') FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            String ID = String.Format("{0:" + Mascara + "}", Consecutivo);
            
            return ID;
        }

        #endregion
    }
    

}