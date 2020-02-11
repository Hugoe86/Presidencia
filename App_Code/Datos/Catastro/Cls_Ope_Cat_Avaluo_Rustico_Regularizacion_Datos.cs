using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Cat_Avaluo_Rustico_Regularizacion.Negocio;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Solicitud_Tramites.Negocios;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos
/// </summary>
/// 

namespace Presidencia.Operacion_Cat_Avaluo_Rustico_Regularizacion.Datos
{
public class Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos
{
	    ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Avaluo_Rustico_Regularizacion
        ///DESCRIPCIÓN: Da de alta en la Base de Datos Todos los datos del Avalúo
        ///PARAMENTROS:     
        ///             1. Avaluo.       Instancia de la Clase de Negocio de Avalúos Rústicos
        ///                              con los datos del que van a ser
        ///                              dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 18/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Avaluo_Rustico_Regularizacion(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Avaluo)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            String No_Avaluo = Obtener_ID_Consecutivo(Ope_Cat_Avaluo_Rustico_R.Tabla_Ope_Cat_Avaluo_Rustico_R, Ope_Cat_Avaluo_Rustico_R.Campo_No_Avaluo, Ope_Cat_Avaluo_Rustico_R.Campo_Anio_Avaluo + "=" + Avaluo.P_Anio_Avaluo, 10);
            String No_Colindancia = Obtener_ID_Consecutivo(Ope_Cat_Colindancias_Arr.Tabla_Ope_Cat_Colindancias_Arr, Ope_Cat_Colindancias_Arr.Campo_No_Colindancia, "", 10);
            String No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Documentos_Arr.Tabla_Ope_Cat_Documentos_Arr, Ope_Cat_Documentos_Arr.Campo_No_Documento, Ope_Cat_Documentos_Arr.Campo_Anio_Documento + "=" + Avaluo.P_Anio_Avaluo, 10);
            try
            {   
                Mi_sql = "INSERT INTO " + Ope_Cat_Avaluo_Rustico_R.Tabla_Ope_Cat_Avaluo_Rustico_R + "(";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_No_Avaluo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Anio_Avaluo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Cuenta_Predial_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Motivo_Avaluo_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Propietario + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Solicitante + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Clave_Catastral + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Domicilio_Notificacion + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Municipio_Notificacion + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Ubicacion + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Localidad_Municipio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Nombre_Predio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_X_Grados + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_X_Minutos + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_X_Segundos + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Orientacion_X + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Y_Grados + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Y_Minutos + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Y_Segundos + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Orientacion_Y + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Tipo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coordenadas_UTM + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coordenadas_UTM_Y + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Valor_Total_Predio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Norte + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Oriente + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Poniente + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Sur + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Observaciones + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Perito_Interno_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_No_Oficio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Solicitud_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Estatus + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Veces_Rechazo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Uso_Constru + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Usuario_Creo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Fecha_Creo;
                Mi_sql += ") VALUES ('";
                Mi_sql += No_Avaluo + "', ";
                Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                Mi_sql += Avaluo.P_Cuenta_Predial_Id + "', '";
                Mi_sql += Avaluo.P_Motivo_Avaluo_Id + "', '";
                Mi_sql += Avaluo.P_Propietario + "', '";
                Mi_sql += Avaluo.P_Solicitante + "', '";
                Mi_sql += Avaluo.P_Clave_Catastral + "', '";
                Mi_sql += Avaluo.P_Domicilio_Notificar + "', '";
                Mi_sql += Avaluo.P_Municipio_Notificar + "', '";
                Mi_sql += Avaluo.P_Ubicacion + "', '";
                Mi_sql += Avaluo.P_Localidad_Municipio + "', '";
                Mi_sql += Avaluo.P_Nombre_Predio + "', '";
                Mi_sql += Avaluo.P_Grados_X + "', '";
                Mi_sql += Avaluo.P_Minutos_X + "', '";
                Mi_sql += Avaluo.P_Segundos_X + "', '";
                Mi_sql += Avaluo.P_Orientacion_X + "', '";
                Mi_sql += Avaluo.P_Grados_Y + "', '";
                Mi_sql += Avaluo.P_Minutos_Y + "', '";
                Mi_sql += Avaluo.P_Segundos_Y + "', '";
                Mi_sql += Avaluo.P_Orientacion_Y + "', '";
                Mi_sql += Avaluo.P_Tipo + "', '";                
                Mi_sql += Avaluo.P_Coordenadas_UTM + "', '";
                Mi_sql += Avaluo.P_Coordenadas_UTM_Y + "', ";
                Mi_sql += Avaluo.P_Valor_Total_Predio + ", '";
                Mi_sql += Avaluo.P_Coord_Norte + "', '";
                Mi_sql += Avaluo.P_Coord_Oriente + "', '";
                Mi_sql += Avaluo.P_Coord_Poniente + "', '";
                Mi_sql += Avaluo.P_Coord_Sur + "', '";
                Mi_sql += Avaluo.P_Observaciones + "', '";
                Mi_sql += Avaluo.P_Perito_Interno_Id + "', ";
                if (Avaluo.P_No_Oficio.Trim() != "")
                {
                    Mi_sql += "'" + Avaluo.P_No_Oficio + "', '";
                }
                else
                {
                    Mi_sql += "NULL, '";
                }
                Mi_sql += Avaluo.P_Solicitud_Id + "', '";
                Mi_sql += Avaluo.P_Estatus + "', ";
                Mi_sql += "0, '";
                Mi_sql += Avaluo.P_Uso_Constru + "', '";
                Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += "SYSDATE";
                Mi_sql += ")";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Clasificacion_Zona.Rows)
                {
                    if (Dr_Renglon["DESCRIPCION_RUSTICO_ID1"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Caracteristicas_Arr.Tabla_Ope_Cat_Caracteristicas_Arr + "(";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Desc_Constru_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Descripcion_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Valor_Indicador_A + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["DESC_CONSTRU_RUSTICO_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["DESCRIPCION_RUSTICO_ID1"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["VALOR_INDICADOR_A"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["DESCRIPCION_RUSTICO_ID2"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Caracteristicas_Arr.Tabla_Ope_Cat_Caracteristicas_Arr + "(";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Desc_Constru_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Descripcion_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Valor_Indicador_A + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["DESC_CONSTRU_RUSTICO_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["DESCRIPCION_RUSTICO_ID2"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["VALOR_INDICADOR_B"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["DESCRIPCION_RUSTICO_ID3"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Caracteristicas_Arr.Tabla_Ope_Cat_Caracteristicas_Arr + "(";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Desc_Constru_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Descripcion_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Valor_Indicador_A + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["DESC_CONSTRU_RUSTICO_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["DESCRIPCION_RUSTICO_ID2"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["VALOR_INDICADOR_B"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["DESCRIPCION_RUSTICO_ID4"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Caracteristicas_Arr.Tabla_Ope_Cat_Caracteristicas_Arr + "(";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Desc_Constru_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Descripcion_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Valor_Indicador_A + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["DESC_CONSTRU_RUSTICO_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["DESCRIPCION_RUSTICO_ID4"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["VALOR_INDICADOR_D"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Elementos_Construccion.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Elem_Construccion_Arr.Tabla_Ope_Cat_Elem_Construccion_Arr + "(";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elementos_Contruccion_Id + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_A + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_B + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_C + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_D + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_E + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_F + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_G + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_H + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_I + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_J + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_K + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_L + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_M + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_N + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_O + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Fecha_Creo;
                    Mi_sql += ") VALUES ('";
                    Mi_sql += No_Avaluo + "', ";
                    Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                    Mi_sql += Dr_Renglon["ELEMENTOS_CONSTRUCCION_ID"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_A"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_B"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_C"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_D"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_E"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_F"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_G"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_H"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_I"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_J"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_K"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_L"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_M"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_N"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["ELEMENTO_CONSTRUCCION_O"].ToString() + "', '";
                    Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += "SYSDATE";
                    Mi_sql += ")";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Construccion.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Const_Arr.Tabla_Ope_Cat_Calc_Valor_Const_Arr + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Croquis + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Valor_Construccion_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Edad_Constru + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Uso_Constru + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Fecha_Creo;
                    Mi_sql += ") VALUES ('";
                    Mi_sql += No_Avaluo + "', ";
                    Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                    Mi_sql += Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arr.Campo_Croquis].ToString() + "', ";
                    Mi_sql += Dr_Renglon["SUPERFICIE_M2"].ToString() + ", ";
                    if (Dr_Renglon["VALOR_CONSTRUCCION_ID"].ToString().Trim() != "")
                    {
                        Mi_sql += "'" + Dr_Renglon["VALOR_CONSTRUCCION_ID"].ToString() + "', ";
                    }
                    else
                    {
                        Mi_sql += "NULL, ";
                    }
                    Mi_sql += "" + Dr_Renglon["FACTOR"].ToString() + ", ";
                    Mi_sql += Dr_Renglon["VALOR_PARCIAL"].ToString() + ", '";
                    Mi_sql += "', '";
                    Mi_sql += "', '";
                    Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += "SYSDATE";
                    Mi_sql += ")";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Terreno.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Terreno_Arr.Tabla_Ope_Cat_Calc_Terreno_Arr + "(";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Tipo_Constru_Rustico_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Superficie + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Constru_Rustico_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Fecha_Creo;
                    Mi_sql += ") VALUES ('";
                    Mi_sql += No_Avaluo + "', ";
                    Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                    Mi_sql += Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Tipo_Constru_Rustico_Id].ToString() + "', ";
                    Mi_sql += Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Superficie].ToString() + ", ";
                    if (Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Constru_Rustico_Id].ToString().Trim() != "")
                    {
                        Mi_sql += "'" + Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Constru_Rustico_Id].ToString() + "', ";
                    }
                    else
                    {
                        Mi_sql += "NULL, ";
                    }
                    Mi_sql += "" + Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Factor].ToString() + ", ";
                    Mi_sql += Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Parcial].ToString() + ", '";
                    Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += "SYSDATE";
                    Mi_sql += ")";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Medidas.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Colindancias_Arr.Tabla_Ope_Cat_Colindancias_Arr + "(";
                        Mi_sql += Ope_Cat_Colindancias_Arr.Campo_No_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Arr.Campo_Medida_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Colindancia + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += No_Avaluo + "', '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Colindancias_Arr.Campo_Medida_Colindancia].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Colindancia = (Convert.ToInt32(No_Colindancia) + 1).ToString("0000000000");
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Archivos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Documentos_Arr.Tabla_Ope_Cat_Documentos_Arr + "(";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_Anio_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Documentos_Arr.Campo_Documento].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos_Arr/" + Avaluo.P_Anio_Avaluo + "_" + No_Avaluo + "/" + Dr_Renglon[Ope_Cat_Documentos_Arr.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Documento = (Convert.ToInt32(No_Documento) + 1).ToString("0000000000");
                    }
                }

                Cls_Ope_Bandeja_Tramites_Negocio Neg_Actualizar_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                System.Text.StringBuilder My_SQL = new System.Text.StringBuilder();
                My_SQL.Append("SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                    + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                    + "," + Ope_Tra_Solicitud.Campo_Estatus
                    + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                    + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ")) = UPPER(TRIM('" + Avaluo.P_Solicitud_Id + "'))");
                Cmd.CommandText = My_SQL.ToString();
                Cmd.CommandType = CommandType.Text;
                OracleDataReader Dtr_Datos_Solicitud = Cmd.ExecuteReader();

                // si hay datos para leer, agregar pasivo
                if (Dtr_Datos_Solicitud.Read())
                {
                    // establecer parámetros para actualizar solicitud
                    Neg_Actualizar_Solicitud.P_Comando_Oracle = Cmd;
                    Neg_Actualizar_Solicitud.P_Solicitud_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString().Trim();
                    Neg_Actualizar_Solicitud.P_Estatus = "APROBAR";
                    Neg_Actualizar_Solicitud.P_Subproceso_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString().Trim();
                    Neg_Actualizar_Solicitud.P_Comentarios = "REGULARIZACION REGISTRADA";
                    Neg_Actualizar_Solicitud.P_Usuario = Cls_Sessiones.Nombre_Ciudadano;
                    // llamar método que actualizar la solicitud
                    Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Evaluar_Solicitud();
                }

                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Avaluo_Rustico_Regularizacion: " + E.Message);
            }
            Avaluo.P_No_Avaluo = No_Avaluo;
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Avaluo_Rustico_Regularizacion
        ///DESCRIPCIÓN: Modifica en la Base de Datos el Avalúo Rústico
        ///PARAMENTROS:     
        ///             1. Avaluo.       Instancia de la Clase de Negocio de Avalúos Rústicos
        ///                              con los datos del que van a ser
        ///                              Modificados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Avaluo_Rustico_Regularizacion(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Avaluo)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            String No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Documentos_Arr.Tabla_Ope_Cat_Documentos_Arr, Ope_Cat_Documentos_Arr.Campo_No_Documento, Ope_Cat_Documentos_Arr.Campo_Anio_Documento + "=" + Avaluo.P_Anio_Avaluo, 10);
            String No_Colindancia = Obtener_ID_Consecutivo(Ope_Cat_Colindancias_Arr.Tabla_Ope_Cat_Colindancias_Arr, Ope_Cat_Colindancias_Arr.Campo_No_Colindancia, "", 10);
            try
            {
                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Rustico_R.Tabla_Ope_Cat_Avaluo_Rustico_R + " SET ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Cuenta_Predial_Id + "= '" + Avaluo.P_Cuenta_Predial_Id + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Motivo_Avaluo_Id + "= '" + Avaluo.P_Motivo_Avaluo_Id + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Propietario + "= '" + Avaluo.P_Propietario + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Solicitante + "= '" + Avaluo.P_Solicitante + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Clave_Catastral + "= '" + Avaluo.P_Clave_Catastral + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Domicilio_Notificacion + "= '" + Avaluo.P_Domicilio_Notificar + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Municipio_Notificacion + "= '" + Avaluo.P_Municipio_Notificar + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Ubicacion + "= '" + Avaluo.P_Ubicacion + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Localidad_Municipio + "= '" + Avaluo.P_Localidad_Municipio + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Nombre_Predio + "= '" + Avaluo.P_Nombre_Predio + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_X_Grados + "= '" + Avaluo.P_Grados_X + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_X_Minutos + "= '" + Avaluo.P_Minutos_X + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_X_Segundos + "= '" + Avaluo.P_Segundos_X + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Orientacion_X + "= '" + Avaluo.P_Orientacion_X + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Y_Grados + "= '" + Avaluo.P_Grados_Y + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Y_Minutos + "= '" + Avaluo.P_Minutos_Y + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Y_Segundos + "= '" + Avaluo.P_Segundos_Y + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Orientacion_Y + "= '" + Avaluo.P_Orientacion_Y + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coordenadas_UTM + "= '" + Avaluo.P_Coordenadas_UTM + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coordenadas_UTM_Y + "= '" + Avaluo.P_Coordenadas_UTM_Y + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Tipo + "= '" + Avaluo.P_Tipo + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Uso_Constru + "= '" + Avaluo.P_Uso_Constru + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Valor_Total_Predio + "= " + Avaluo.P_Valor_Total_Predio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Norte + "= '" + Avaluo.P_Coord_Norte + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Oriente + "= '" + Avaluo.P_Coord_Oriente + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Poniente + "= '" + Avaluo.P_Coord_Poniente + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Sur + "= '" + Avaluo.P_Coord_Sur + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Solicitud_Id + "= '" + Avaluo.P_Solicitud_Id + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_No_Oficio + "= '" + Avaluo.P_No_Oficio + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Observaciones + "= '" + Avaluo.P_Observaciones + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Estatus + "= '" + Avaluo.P_Estatus + "', ";                
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Usuario_Modifico + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Fecha_Modifico + "= SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Rustico_R.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                Mi_sql = "DELETE " + Ope_Cat_Caracteristicas_Arr.Tabla_Ope_Cat_Caracteristicas_Arr + " WHERE " + Ope_Cat_Caracteristicas_Arr.Campo_No_Avaluo
                + " = '" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Caracteristicas_Arr.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Clasificacion_Zona.Rows)
                {
                    if (Dr_Renglon["DESCRIPCION_RUSTICO_ID1"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Caracteristicas_Arr.Tabla_Ope_Cat_Caracteristicas_Arr + "(";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Desc_Constru_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Descripcion_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Valor_Indicador_A + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["DESC_CONSTRU_RUSTICO_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["DESCRIPCION_RUSTICO_ID1"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["VALOR_INDICADOR_A"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["DESCRIPCION_RUSTICO_ID2"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Caracteristicas_Arr.Tabla_Ope_Cat_Caracteristicas_Arr + "(";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Desc_Constru_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Descripcion_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Valor_Indicador_A + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["DESC_CONSTRU_RUSTICO_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["DESCRIPCION_RUSTICO_ID2"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["VALOR_INDICADOR_B"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["DESCRIPCION_RUSTICO_ID3"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Caracteristicas_Arr.Tabla_Ope_Cat_Caracteristicas_Arr + "(";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Desc_Constru_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Descripcion_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Valor_Indicador_A + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["DESC_CONSTRU_RUSTICO_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["DESCRIPCION_RUSTICO_ID3"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["VALOR_INDICADOR_C"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["DESCRIPCION_RUSTICO_ID4"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Caracteristicas_Arr.Tabla_Ope_Cat_Caracteristicas_Arr + "(";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Desc_Constru_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Descripcion_Rustico_Id + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Valor_Indicador_A + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Caracteristicas_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["DESC_CONSTRU_RUSTICO_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["DESCRIPCION_RUSTICO_ID4"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["VALOR_INDICADOR_D"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Elementos_Construccion.Rows)
                {
                    Mi_sql = "UPDATE " + Ope_Cat_Elem_Construccion_Arr.Tabla_Ope_Cat_Elem_Construccion_Arr + " SET ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_A + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_A"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_B + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_B"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_C + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_C"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_D + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_D"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_E + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_E"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_F + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_F"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_G + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_G"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_H + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_H"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_I + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_I"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_J + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_J"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_K + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_K"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_L + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_L"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_M + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_M"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_N + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_N"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_O + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_O"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Usuario_Creo + "= '" + Cls_Sessiones.Nombre_Ciudadano + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Fecha_Creo + "= SYSDATE";
                    Mi_sql += " WHERE " + Ope_Cat_Elem_Construccion_Arr.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + " AND ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Arr.Campo_Elementos_Contruccion_Id + "= '" + Dr_Renglon["ELEMENTOS_CONSTRUCCION_ID"].ToString() + "'";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                Mi_sql = "DELETE " + Ope_Cat_Calc_Valor_Const_Arr.Tabla_Ope_Cat_Calc_Valor_Const_Arr + " WHERE " + Ope_Cat_Calc_Valor_Const_Arr.Campo_No_Avaluo
                + " = '" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Calc_Valor_Const_Arr.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Construccion.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Const_Arr.Tabla_Ope_Cat_Calc_Valor_Const_Arr + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Croquis + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Valor_Construccion_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Edad_Constru + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Uso_Constru + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Arr.Campo_Fecha_Creo;
                    Mi_sql += ") VALUES ('";
                    Mi_sql += Avaluo.P_No_Avaluo + "', ";
                    Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                    Mi_sql += Dr_Renglon[Ope_Cat_Calc_Valor_Const_Arr.Campo_Croquis].ToString() + "', ";
                    Mi_sql += Dr_Renglon["SUPERFICIE_M2"].ToString() + ", ";
                    if (Dr_Renglon["VALOR_CONSTRUCCION_ID"].ToString().Trim() != "")
                    {
                        Mi_sql += "'" + Dr_Renglon["VALOR_CONSTRUCCION_ID"].ToString() + "', ";
                    }
                    else
                    {
                        Mi_sql += "NULL, ";
                    }
                    Mi_sql += "" + Dr_Renglon["FACTOR"].ToString() + ", ";
                    Mi_sql += Dr_Renglon["VALOR_PARCIAL"].ToString() + ", '";
                    Mi_sql += "', '";
                    Mi_sql += "', '";
                    Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += "SYSDATE";
                    Mi_sql += ")";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                Mi_sql = "DELETE " + Ope_Cat_Calc_Terreno_Arr.Tabla_Ope_Cat_Calc_Terreno_Arr + " WHERE " + Ope_Cat_Calc_Terreno_Arr.Campo_No_Avaluo
                + " = '" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Calc_Terreno_Arr.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Terreno.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Terreno_Arr.Tabla_Ope_Cat_Calc_Terreno_Arr + "(";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Tipo_Constru_Rustico_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Superficie + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Constru_Rustico_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Terreno_Arr.Campo_Fecha_Creo;
                    Mi_sql += ") VALUES ('";
                    Mi_sql += Avaluo.P_No_Avaluo + "', ";
                    Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                    Mi_sql += Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Tipo_Constru_Rustico_Id].ToString() + "', ";
                    Mi_sql += Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Superficie].ToString() + ", ";
                    if (Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Constru_Rustico_Id].ToString().Trim() != "")
                    {
                        Mi_sql += "'" + Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Constru_Rustico_Id].ToString() + "', ";
                    }
                    else
                    {
                        Mi_sql += "NULL, ";
                    }
                    Mi_sql += "" + Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Factor].ToString() + ", ";
                    Mi_sql += Dr_Renglon[Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Parcial].ToString() + ", '";
                    Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += "SYSDATE";
                    Mi_sql += ")";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                if (Avaluo.P_Dt_Observaciones != null && Avaluo.P_Dt_Observaciones.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Observaciones.Rows)
                    {
                        Mi_sql = "UPDATE " + Ope_Cat_Seguimiento_Arr.Tabla_Ope_Cat_Seguimiento_Arr;
                        Mi_sql += " SET " + Ope_Cat_Seguimiento_Arr.Campo_Estatus + " = 'BAJA', ";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Ope_Cat_Seguimiento_Arr.Campo_No_Seguimiento + "='" + Dr_Renglon["NO_SEGUIMIENTO"].ToString() + "' AND ";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Medidas.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Colindancias_Arr.Tabla_Ope_Cat_Colindancias_Arr + "(";
                        Mi_sql += Ope_Cat_Colindancias_Arr.Campo_No_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Arr.Campo_Medida_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Colindancia + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Avaluo.P_No_Avaluo + "', '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Colindancias_Arr.Campo_Medida_Colindancia].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Colindancia = (Convert.ToInt32(No_Colindancia) + 1).ToString("0000000000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon[Ope_Cat_Colindancias_Arr.Campo_No_Colindancia].ToString().Trim() != "")
                    {
                        Mi_sql = "DELETE " + Ope_Cat_Colindancias_Arr.Tabla_Ope_Cat_Colindancias_Arr + " WHERE " + Ope_Cat_Colindancias_Arr.Campo_No_Colindancia;
                        Mi_sql += "='" + Dr_Renglon[Ope_Cat_Colindancias_Arr.Campo_No_Colindancia].ToString() + "' AND " + Ope_Cat_Colindancias_Arr.Campo_Anio_Avaluo + "=" + Avaluo.P_Anio_Avaluo;
                        Mi_sql += " AND " + Ope_Cat_Colindancias_Arr.Campo_No_Avaluo + "= '" + Avaluo.P_No_Avaluo + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Archivos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Documentos_Arr.Tabla_Ope_Cat_Documentos_Arr + "(";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_Anio_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Documentos_Arr.Campo_Documento].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos_Arr/" + Avaluo.P_Anio_Avaluo + "_" + Avaluo.P_No_Avaluo + "/" + Dr_Renglon[Ope_Cat_Documentos_Arr.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Documento = (Convert.ToInt32(No_Documento) + 1).ToString("0000000000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["NO_DOCUMENTO"].ToString().Trim() != "")
                    {
                        Mi_sql = "DELETE " + Ope_Cat_Documentos_Arr.Tabla_Ope_Cat_Documentos_Arr + " WHERE " + Ope_Cat_Documentos_Arr.Campo_No_Documento;
                        Mi_sql += "='" + Dr_Renglon[Ope_Cat_Documentos_Arr.Campo_No_Documento].ToString() + "' AND " + Ope_Cat_Documentos_Arr.Campo_Anio_Documento + "=" + Avaluo.P_Anio_Avaluo;
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Avaluo_Rustico: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Observaciones_Arr
        ///DESCRIPCIÓN: Da de alta o modifica los registros de observaciones
        ///PARAMENTROS:     
        ///             1. Tabla_Val.       Instancia de la Clase de Negocio de Tabla de valores de construcción 
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Observaciones_Arr(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Aval)
        {
            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
            String No_Seguimiento = "";
            No_Seguimiento = Obtener_ID_Consecutivo(Ope_Cat_Seguimiento_Arr.Tabla_Ope_Cat_Seguimiento_Arr, Ope_Cat_Seguimiento_Arr.Campo_No_Seguimiento, "", 10);
            try
            {
                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Rustico_R.Tabla_Ope_Cat_Avaluo_Rustico_R;
                Mi_sql += " SET " + Ope_Cat_Avaluo_Rustico_R.Campo_Estatus + " = '" + Aval.P_Estatus + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                if (Aval.P_Estatus == "AUTORIZADO")
                {
                    Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Fecha_Autorizo + " = SYSDATE, ";
                }
                else
                {                   
                    Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Fecha_Rechazo + " = SYSDATE, ";
                    Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Observaciones_Perito + " = '" + Aval.P_Observaciones_Perito + "', ";
                }
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Rustico_R.Campo_Anio_Avaluo + "=" + Aval.P_Anio_Avaluo;
                Mi_sql += " AND " + Ope_Cat_Avaluo_Rustico_R.Campo_No_Avaluo + "='" + Aval.P_No_Avaluo + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Aval.P_Dt_Observaciones.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Seguimiento_Arr.Tabla_Ope_Cat_Seguimiento_Arr + "(";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_No_Seguimiento + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_Motivo_Id + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_Estatus + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Seguimiento + "', '";
                        Mi_sql += Aval.P_No_Avaluo + "', ";
                        Mi_sql += Aval.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["MOTIVO_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["ESTATUS"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Seguimiento = (Convert.ToInt16(No_Seguimiento) + 1).ToString("0000000000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "ACTUALIZAR")
                    {
                        Mi_sql = "UPDATE " + Ope_Cat_Seguimiento_Arr.Tabla_Ope_Cat_Seguimiento_Arr;
                        Mi_sql += " SET " + Ope_Cat_Seguimiento_Arr.Campo_Estatus + " = '" + Dr_Renglon["ESTATUS"].ToString() + "', ";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Ope_Cat_Seguimiento_Arr.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Ope_Cat_Seguimiento_Arr.Campo_No_Seguimiento + "='" + Dr_Renglon["NO_SEGUIMIENTO"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Observaciones_Ara: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluo_Rustico
        ///DESCRIPCIÓN: Obtiene la tabla con los datos de avalúos rústicos
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 23/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Avaluo_Rustico_Regularizacion(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Avaluo)
        {
            DataTable Dt_Avaluo = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Avaluo_Rustico_R.Campo_No_Avaluo
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Anio_Avaluo
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Anio_Avaluo + "||'/'||TO_NUMBER(" + Ope_Cat_Avaluo_Rustico.Campo_No_Avaluo + ") AS AVALUO"
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Motivo_Avaluo_Id
                    + ", (SELECT " + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion + " FROM " + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + " WHERE "
                    + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + "." + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id + "= "
                    + Ope_Cat_Avaluo_Rustico_R.Tabla_Ope_Cat_Avaluo_Rustico_R + "." + Ope_Cat_Avaluo_Rustico_R.Campo_Motivo_Avaluo_Id + ") AS MOTIVO_AVALUO"
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Cuenta_Predial_Id
                    + ", (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "= "
                    + Ope_Cat_Avaluo_Rustico_R.Tabla_Ope_Cat_Avaluo_Rustico_R + "." + Ope_Cat_Avaluo_Rustico_R.Campo_Cuenta_Predial_Id + ") AS CUENTA_PREDIAL"
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Solicitante
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Observaciones
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Base_Gravable
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Valor_Total_Predio
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Clave_Catastral
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Coord_X_Grados
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Coord_X_Minutos
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Coord_X_Segundos
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Orientacion_X
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Y_Grados
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Y_Minutos
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Y_Segundos
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Orientacion_Y
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Ubicacion
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Impuesto_Bimestral
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Localidad_Municipio
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Nombre_Predio
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Propietario
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Domicilio_Notificacion
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Municipio_Notificacion
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Fecha_Autorizo
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Fecha_Rechazo
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Veces_Rechazo
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Observaciones_Perito
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Norte
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Oriente
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Poniente
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Coord_Sur
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Perito_Externo_Id
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Perito_Interno_Id
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Solicitud_Id
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_No_Oficio
                    + ", (SELECT " + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " WHERE "
                    + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID + "= "
                    + Ope_Cat_Avaluo_Rustico_R.Tabla_Ope_Cat_Avaluo_Rustico_R + "." + Ope_Cat_Avaluo_Rustico_R.Campo_Solicitud_Id + ") AS CLAVE_SOLICITUD"
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Tipo
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Uso_Constru
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Coordenadas_UTM
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Coordenadas_UTM_Y
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Estatus
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Usuario_Modifico
                    + ", " + Ope_Cat_Avaluo_Rustico_R.Campo_Fecha_Modifico
                    + " FROM  " + Ope_Cat_Avaluo_Rustico_R.Tabla_Ope_Cat_Avaluo_Rustico_R
                    + " WHERE ";
                if (Avaluo.P_No_Avaluo != null && Avaluo.P_No_Avaluo.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Rustico_R.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND ";
                }
                if (Avaluo.P_Anio_Avaluo != null && Avaluo.P_Anio_Avaluo.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Rustico_R.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo + " AND ";
                }
                if (Avaluo.P_Perito_Externo_Id != null && Avaluo.P_Perito_Externo_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Rustico_R.Campo_Perito_Externo_Id + " = '" + Avaluo.P_Perito_Externo_Id + "' AND ";
                }
                if (Avaluo.P_Perito_Interno_Id != null && Avaluo.P_Perito_Interno_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Rustico_R.Campo_Perito_Interno_Id + " = '" + Avaluo.P_Perito_Interno_Id + "' AND ";
                }
                if (Avaluo.P_Perito_Externo != null && Avaluo.P_Perito_Externo.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Rustico_R.Campo_Perito_Externo_Id + " IN (SELECT PE." + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + " FROM " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos + " PE WHERE PE." + Cat_Cat_Peritos_Externos.Campo_Nombre + "||' '||PE." + Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno + "||' '||PE." + Cat_Cat_Peritos_Externos.Campo_Apellido_Materno + " LIKE '%" + Avaluo.P_Perito_Externo + "%') AND ";
                }
                if (Avaluo.P_Ubicacion != null && Avaluo.P_Ubicacion.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Rustico_R.Campo_Ubicacion + " LIKE '%" + Avaluo.P_Ubicacion + "%' AND ";
                }
                if (Avaluo.P_Folio != null && Avaluo.P_Folio.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Rustico_R.Campo_Anio_Avaluo + "||'/'||TO_NUMBER(" + Ope_Cat_Avaluo_Rustico.Campo_No_Avaluo + ") = '" + Avaluo.P_Folio + "' AND ";
                }
                if (Avaluo.P_Estatus != null && Avaluo.P_Estatus.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Rustico_R.Campo_Estatus + " " + Avaluo.P_Estatus + " AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                // agregar filtro y orden a la consulta
                Mi_SQL += " ORDER BY " + Ope_Cat_Avaluo_Rustico_R.Campo_Anio_Avaluo + " DESC, " + Ope_Cat_Avaluo_Rustico_R.Campo_No_Avaluo + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Dt_Avaluo = dataset.Tables[0];
                }
                if ((Avaluo.P_No_Avaluo != null && Avaluo.P_No_Avaluo.Trim() != "") && (Avaluo.P_Anio_Avaluo != null && Avaluo.P_Anio_Avaluo.Trim() != ""))
                {
                    Mi_SQL = "SELECT VC." + Ope_Cat_Calc_Valor_Const_Arr.Campo_Croquis
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Arr.Campo_Superficie_M2
                    + ", NVL(VC." + Ope_Cat_Calc_Valor_Const_Arr.Campo_Valor_Construccion_Id + ", '') AS " + Ope_Cat_Calc_Valor_Const_Ara.Campo_Valor_Construccion_Id
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Arr.Campo_Edad_Constru
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Arr.Campo_Uso_Constru
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Arr.Campo_Factor
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Arr.Campo_Valor_Parcial
                    + ", NVL(TV." + Cat_Cat_Tab_Val_Construccion.Campo_Clave_Valor + ",0) AS CON_SERV"
                    + ", NVL(CC." + Cat_Cat_Calidad_Construccion.Campo_Clave_Calidad + ",0) AS TIPO"
                    + ", NVL(TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_M2 + ",0.00) AS VALOR_M2"
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Arr.Campo_Usuario_Creo
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Arr.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Calc_Valor_Const_Arr.Tabla_Ope_Cat_Calc_Valor_Const_Arr + " VC"
                    + " LEFT OUTER JOIN " + Cat_Cat_Tab_Val_Construccion.Tabla_Cat_Cat_Tab_Val_Construccion + " TV"
                    + " ON VC." + Ope_Cat_Calc_Valor_Const_Arr.Campo_Valor_Construccion_Id + "=TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_Construccion_Id
                    + " LEFT OUTER JOIN " + Cat_Cat_Calidad_Construccion.Tabla_Cat_Cat_Calidad_Construccion + " CC"
                    + " ON TV." + Cat_Cat_Tab_Val_Construccion.Campo_Calidad_Id + "=CC." + Cat_Cat_Calidad_Construccion.Campo_Calidad_Id
                    + " WHERE "
                    + Ope_Cat_Calc_Valor_Const_Arr.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Calc_Valor_Const_Arr.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Calculo_Valor_Construccion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    
                    Mi_SQL = "SELECT " + Ope_Cat_Elem_Construccion_Arr.Campo_Elementos_Contruccion_Id
                    + ", (SELECT EC." + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion + " FROM " + Cat_Cat_Elementos_Construccion.Tabla_Cat_Cat_Elementos_Construccion + " EC WHERE EC." + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion_Id + "=" + Ope_Cat_Elem_Construccion_Arr.Tabla_Ope_Cat_Elem_Construccion_Arr + "." + Ope_Cat_Elem_Construccion_Arr.Campo_Elementos_Contruccion_Id + ") AS ELEMENTOS_CONSTRUCCION"
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_A
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_B
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_C
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_D
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_E
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_F
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_G
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_H
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_I
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_J
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_K
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_L
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_M
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_N
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Elemento_Construccion_O
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Elem_Construccion_Arr.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Elem_Construccion_Arr.Tabla_Ope_Cat_Elem_Construccion_Arr
                    + " WHERE "
                    + Ope_Cat_Elem_Construccion_Ara.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Elem_Construccion_Ara.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Elementos_Construccion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                                     
                    Mi_SQL = "SELECT VC." + Ope_Cat_Calc_Terreno_Arr.Campo_Tipo_Constru_Rustico_Id
                    + ", VC." + Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Constru_Rustico_Id
                    + ", NVL(TC." + Cat_Cat_Tipos_Constru_Rustico.Campo_Identificador + ",'') AS " + Cat_Cat_Tipos_Constru_Rustico.Campo_Identificador
                    + ", NVL(CR." + Cat_Cat_Tab_Val_Const_Rustico.Campo_Valor_M2 + ",0.00) AS " + Cat_Cat_Tab_Val_Const_Rustico.Campo_Valor_M2
                    + ", VC." + Ope_Cat_Calc_Terreno_Arr.Campo_Superficie
                    + ", VC." + Ope_Cat_Calc_Terreno_Arr.Campo_Factor
                    + ", VC." + Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Parcial
                    + ", VC." + Ope_Cat_Calc_Terreno_Arr.Campo_Usuario_Creo
                    + ", VC." + Ope_Cat_Calc_Terreno_Arr.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Calc_Terreno_Arr.Tabla_Ope_Cat_Calc_Terreno_Arr + " VC"
                    + " LEFT OUTER JOIN " + Cat_Cat_Tipos_Constru_Rustico.Tabla_Cat_Cat_Tipos_Constru_Rustico + " TC"
                    + " ON VC." + Ope_Cat_Calc_Terreno_Arr.Campo_Tipo_Constru_Rustico_Id + "=TC." + Cat_Cat_Tipos_Constru_Rustico.Campo_Tipo_Constru_Rustico_Id
                    + " LEFT OUTER JOIN " + Cat_Cat_Tab_Val_Const_Rustico.Tabla_Cat_Cat_Tab_Val_Const_Rustico + " CR"
                    + " ON VC." + Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Constru_Rustico_Id + "=CR." + Cat_Cat_Tab_Val_Const_Rustico.Campo_Valor_Constru_Rustico_Id
                    + " WHERE "
                    + Ope_Cat_Calc_Terreno_Arr.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Calc_Terreno_Arr.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Calculo_Valor_Terreno = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT OCZ." + Ope_Cat_Caracteristicas_Arr.Campo_Desc_Constru_Rustico_Id
                    + ", OCZ." + Ope_Cat_Caracteristicas_Arr.Campo_Descripcion_Rustico_Id
                    + ", CR." + Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice
                    + ", CZ." + Cat_Cat_Descrip_Const_Rustico.Campo_Identificador
                    + ", NVL(CR." + Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A + ",'') AS " + Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A
                    + ", OCZ." + Ope_Cat_Caracteristicas_Arr.Campo_Valor_Indicador_A
                    + ", OCZ." + Ope_Cat_Caracteristicas_Arr.Campo_Usuario_Creo
                    + ", OCZ." + Ope_Cat_Caracteristicas_Arr.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Caracteristicas_Arr.Tabla_Ope_Cat_Caracteristicas_Arr + " OCZ"
                    + " LEFT OUTER JOIN " + Cat_Cat_Descrip_Const_Rustico.Tabla_Cat_Cat_Descrip_Const_Rustico + " CZ"
                    + " ON OCZ." + Ope_Cat_Caracteristicas_Arr.Campo_Desc_Constru_Rustico_Id + "=CZ." + Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id
                    + " LEFT OUTER JOIN " + Cat_Cat_Tabla_Descrip_Rustico.Tabla_Cat_Cat_Tabla_Descrip_Rustico + " CR"
                    + " ON OCZ." + Ope_Cat_Caracteristicas_Arr.Campo_Descripcion_Rustico_Id + "=CR." + Cat_Cat_Tabla_Descrip_Rustico.Campo_Descrip_Rustico_Id
                    + " WHERE "
                    + Ope_Cat_Caracteristicas_Arr.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Caracteristicas_Arr.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Clasificacion_Zona = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Documentos_Arr.Campo_No_Documento
                    + ", " + Ope_Cat_Documentos_Arr.Campo_Anio_Documento
                    + ", " + Ope_Cat_Documentos_Arr.Campo_Documento
                    + ", " + Ope_Cat_Documentos_Arr.Campo_Ruta_Documento
                    + ", " + Ope_Cat_Documentos_Arr.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Documentos_Arr.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Documentos_Arr.Campo_Fecha_Modifico
                    + ", " + Ope_Cat_Documentos_Arr.Campo_Usuario_Modifico
                    + ", 'NADA' AS ACCION"
                    + " FROM  " + Ope_Cat_Documentos_Arr.Tabla_Ope_Cat_Documentos_Arr
                    + " WHERE "
                    + Ope_Cat_Documentos_Arr.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Documentos_Arr.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Archivos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Colindancias_Arr.Campo_No_Colindancia
                    + ", " + Ope_Cat_Colindancias_Arr.Campo_Anio_Avaluo
                    + ", " + Ope_Cat_Colindancias_Arr.Campo_No_Avaluo
                    + ", " + Ope_Cat_Colindancias_Arr.Campo_Medida_Colindancia
                    + ", " + Ope_Cat_Colindancias_Arr.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Colindancias_Arr.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Colindancias_Arr.Campo_Fecha_Modifico
                    + ", " + Ope_Cat_Colindancias_Arr.Campo_Usuario_Modifico
                    + ", 'NADA' AS ACCION"
                    + " FROM  " + Ope_Cat_Colindancias_Arr.Tabla_Ope_Cat_Colindancias_Arr
                    + " WHERE "
                    + Ope_Cat_Colindancias_Arr.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Colindancias_Arr.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Medidas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Avalúos Rústicos Regularizacion. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Avaluo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Valores_Construccion_Regularizacion
        ///DESCRIPCIÓN: Obtiene todos los tipos de construcción que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 LUMIA 900
        ///
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Valores_Construccion_Regularizacion(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT TV." + Cat_Cat_Tab_Val_Construccion.Campo_Clave_Valor + " AS CON_SERV";
                My_Sql += ", CC." + Cat_Cat_Calidad_Construccion.Campo_Clave_Calidad + " AS TIPO";
                My_Sql += ", TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_M2;
                My_Sql += ", TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_Construccion_Id;
                My_Sql += " FROM " + Cat_Cat_Tab_Val_Construccion.Tabla_Cat_Cat_Tab_Val_Construccion + " TV";
                My_Sql += " LEFT OUTER JOIN " + Cat_Cat_Calidad_Construccion.Tabla_Cat_Cat_Calidad_Construccion + " CC";
                My_Sql += " ON TV." + Cat_Cat_Tab_Val_Construccion.Campo_Calidad_Id + " = CC." + Cat_Cat_Calidad_Construccion.Campo_Calidad_Id;
                My_Sql += " AND  TV." + Cat_Cat_Tab_Val_Construccion.Campo_Anio + " = " + Avaluo.P_Anio_Avaluo;
                My_Sql += " LEFT OUTER JOIN " + Cat_Cat_Tipos_Construccion.Tabla_Cat_Cat_Tipos_Construccion + " TC";
                My_Sql += " ON CC." + Cat_Cat_Calidad_Construccion.Campo_Tipo_Construccion_Id + " = TC." + Cat_Cat_Tipos_Construccion.Campo_Tipo_Construccion_Id;
                My_Sql += " AND  TC." + Cat_Cat_Tipos_Construccion.Campo_Estatus + " = 'VIGENTE'";
                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Tramos = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Tipos_Construccion: Error al consultar los Tipos de contruccion.");
            }
            return Dt_Tramos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Caracteristicas_Terreno
        ///DESCRIPCIÓN: Obtiene todos los tipos de construcción que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Caracteristicas_Terreno_Regularizacion(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT DC." + Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id;
                My_Sql += ", DC." + Cat_Cat_Descrip_Const_Rustico.Campo_Identificador;
                My_Sql += ", TD." + Cat_Cat_Tabla_Descrip_Rustico.Campo_Descrip_Rustico_Id;
                My_Sql += ", TD." + Cat_Cat_Tabla_Descrip_Rustico.Campo_Valor_Indice;
                My_Sql += ", TD." + Cat_Cat_Tabla_Descrip_Rustico.Campo_Indicador_A;
                My_Sql += ", '' AS " + Ope_Cat_Caracteristicas_Arr.Campo_Valor_Indicador_A;
                My_Sql += " FROM " + Cat_Cat_Descrip_Const_Rustico.Tabla_Cat_Cat_Descrip_Const_Rustico + " DC";
                My_Sql += " LEFT OUTER JOIN " + Cat_Cat_Tabla_Descrip_Rustico.Tabla_Cat_Cat_Tabla_Descrip_Rustico + " TD";
                My_Sql += " ON DC." + Cat_Cat_Descrip_Const_Rustico.Campo_Desc_Constru_Rustico_Id + "=TD." + Cat_Cat_Tabla_Descrip_Rustico.Campo_Des_Constru_Rustico_Id + " AND TD." + Cat_Cat_Tabla_Descrip_Rustico.Campo_Anio + "=" + Avaluo.P_Anio_Avaluo;
                My_Sql += " WHERE DC." + Cat_Cat_Descrip_Const_Rustico.Campo_Estatus + "='VIGENTE'";

                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Tramos = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Tipos_Construccion: Error al consultar los Tipos de contruccion.");
            }
            return Dt_Tramos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Elementos_Construccion
        ///DESCRIPCIÓN: Obtiene la tabla inicial para los elementos de construcción del avalúo urbano
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Elementos_Construccion_Regularizacion(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT " + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion_Id;
                My_Sql += ", " + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_A;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_B;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_C;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_D;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_E;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_F;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_G;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_H;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_I;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_J;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_K;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_L;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_M;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_N;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_O;
                My_Sql += " FROM " + Cat_Cat_Elementos_Construccion.Tabla_Cat_Cat_Elementos_Construccion;
                My_Sql += " WHERE " + Cat_Cat_Elementos_Construccion.Campo_Estatus + " = 'VIGENTE'";
                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Tramos = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Tabla_Elementos_Construccion: " + E.Message);
            }
            return Dt_Tramos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Terreno_Regularizacion
        ///DESCRIPCIÓN: Obtiene todos los tipos de construcción que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Terreno_Regularizacion(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT TC." + Cat_Cat_Tipos_Constru_Rustico.Campo_Tipo_Constru_Rustico_Id;
                My_Sql += ", TC." + Cat_Cat_Tipos_Constru_Rustico.Campo_Identificador;
                My_Sql += ", TV." + Cat_Cat_Tab_Val_Const_Rustico.Campo_Valor_Constru_Rustico_Id;
                My_Sql += ", TV." + Cat_Cat_Tab_Val_Const_Rustico.Campo_Valor_M2;
                My_Sql += ", 0.00 AS " + Ope_Cat_Calc_Terreno_Arr.Campo_Superficie;
                My_Sql += ", 1.00 AS " + Ope_Cat_Calc_Terreno_Arr.Campo_Factor;
                My_Sql += ", 0.00 AS " + Ope_Cat_Calc_Terreno_Arr.Campo_Valor_Parcial;
                My_Sql += " FROM " + Cat_Cat_Tipos_Constru_Rustico.Tabla_Cat_Cat_Tipos_Constru_Rustico + " TC";
                My_Sql += " LEFT OUTER JOIN " + Cat_Cat_Tab_Val_Const_Rustico.Tabla_Cat_Cat_Tab_Val_Const_Rustico + " TV";
                My_Sql += " ON TC." + Cat_Cat_Tipos_Constru_Rustico.Campo_Tipo_Constru_Rustico_Id + "=TV." + Cat_Cat_Tab_Val_Const_Rustico.Campo_Tipo_Constru_Rustico_Id + " AND TV." + Cat_Cat_Tab_Val_Const_Rustico.Campo_Anio + "=" + Avaluo.P_Anio_Avaluo;
                My_Sql += " WHERE TC." + Cat_Cat_Tipos_Constru_Rustico.Campo_Estatus + " ='VIGENTE'";

                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Tramos = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Tipos_Construccion: Error al consultar los Tipos de contruccion.");
            }
            return Dt_Tramos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Motivos_Rechazo_Avaluo
        ///DESCRIPCIÓN: Obtiene todos los Motivos de rechazo del avalúo
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Motivos_Rechazo_Avaluo_Regularizacion(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT SE." + Ope_Cat_Seguimiento_Arr.Campo_No_Seguimiento + " AS NO_SEGUIMIENTO";
                My_Sql += ", SE." + Ope_Cat_Seguimiento_Arr.Campo_Motivo_Id + " AS MOTIVO_ID";
                My_Sql += ", SE." + Ope_Cat_Seguimiento_Arr.Campo_Estatus + " AS ESTATUS";
                My_Sql += ", MR." + Cat_Cat_Motivos_Rechazo.Campo_Motivo_Descripcion + " AS MOTIVO_DESCRIPCION";
                My_Sql += ", 'NADA' AS ACCION";
                My_Sql += " FROM " + Ope_Cat_Seguimiento_Arr.Tabla_Ope_Cat_Seguimiento_Arr + " SE";
                My_Sql += " LEFT OUTER JOIN " + Cat_Cat_Motivos_Rechazo.Tabla_Cat_Cat_Motivos_Rechazo + " MR";
                My_Sql += " ON SE." + Ope_Cat_Seguimiento_Arr.Campo_Motivo_Id + " = MR." + Cat_Cat_Motivos_Rechazo.Campo_Motivo_Id;
                My_Sql += " WHERE SE." + Ope_Cat_Seguimiento_Arr.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;
                My_Sql += " AND SE." + Ope_Cat_Seguimiento_Arr.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' ";
                if (Avaluo.P_Estatus != null && Avaluo.P_Estatus.Trim() != "")
                {
                    My_Sql += "AND SE." + Ope_Cat_Seguimiento_Arr.Campo_Estatus + " " + Avaluo.P_Estatus;
                }
                My_Sql += " ORDER BY SE." + Ope_Cat_Seguimiento_Arr.Campo_Estatus + " DESC";
                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Tramos = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Motivos_Rechazo_Avaluo: Error al consultar los motivos de rechazo de Avalúo Rústico.");
            }
            return Dt_Tramos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, String Filtro, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                if (Filtro != "" && Filtro != null)
                {
                    Mi_SQL += " WHERE " + Filtro;
                }
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            }
            catch (Exception Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 26/Octubre/2010
        ///CAUSA_MODIFICACIÓN   : Estandarizar variables usadas
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
    ///NOMBRE DE LA FUNCIÓN: Consultar_Solicitud_Tramite
    ///DESCRIPCIÓN: consultar tipos de tramites de solicitudes
    ///PARAMETROS:     
    ///           
    ///CREO: Alejandro Leyva Alvarado
    ///FECHA_CREO: 29/Agosto/2012 
    ///MODIFICO             : 
    ///FECHA_MODIFICO       : 
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
        public static DataTable Consultar_Solicitud_Tramite(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Tramite)
        {
            DataTable Dt_Tramites = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT TS." + Ope_Tra_Solicitud.Campo_Solicitud_ID
                 + ", TS." + Ope_Tra_Solicitud.Campo_Apellido_Materno
                 + ", TS." + Ope_Tra_Solicitud.Campo_Apellido_Paterno
                 + ", TS." + Ope_Tra_Solicitud.Campo_Cantidad
                 + ", TS." + Ope_Tra_Solicitud.Campo_Clave_Solicitud
                 + ", TS." + Ope_Tra_Solicitud.Campo_Comentarios
                 + ", TS." + Ope_Tra_Solicitud.Campo_Correo_Electronico
                 + ", TS." + Ope_Tra_Solicitud.Campo_Costo_Base
                 + ", TS." + Ope_Tra_Solicitud.Campo_Costo_Total
                 + ", TS." + Ope_Tra_Solicitud.Campo_Cuenta_Predial
                 + ", TS." + Ope_Tra_Solicitud.Campo_Empleado_ID
                 + ", TS." + Ope_Tra_Solicitud.Campo_Estatus
                 + ", TS." + Ope_Tra_Solicitud.Campo_Fecha_Creo
                 + ", TS." + Ope_Tra_Solicitud.Campo_Fecha_Entrega
                 + ", TS." + Ope_Tra_Solicitud.Campo_Fecha_Modifico
                 + ", TS." + Ope_Tra_Solicitud.Campo_Folio
                 + ", TS." + Ope_Tra_Solicitud.Campo_Inspector_ID
                 + ", TS." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante
                 + ", TS." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance
                 + ", TS." + Ope_Tra_Solicitud.Campo_Subproceso_ID
                 + ", TS." + Ope_Tra_Solicitud.Campo_Tramite_ID
                 + ", TS." + Ope_Tra_Solicitud.Campo_Usuario_Creo
                 + ", TS." + Ope_Tra_Solicitud.Campo_Usuario_Modifico
                 + ", TS." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + "||' '|| TS." + Ope_Tra_Solicitud.Campo_Apellido_Materno + "||' '||TS." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " AS NOMBRE_COMPLETO"
                 + ", TS." + Ope_Tra_Solicitud.Campo_Zona_ID
                 + ", TDMA." + Ope_Tra_Datos.Campo_Valor + " AS MOTIVO_REGULARIZACION"
                 + " FROM  " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " TS"
                 + " LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TT"
                 + " ON TS." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = TT." + Cat_Tra_Tramites.Campo_Tramite_ID

                 + " LEFT OUTER JOIN " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + " CTDMA ON  TS." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = CTDMA." + Cat_Tra_Datos_Tramite.Campo_Tramite_ID
                 + " AND UPPER(CTDMA." + Cat_Tra_Datos_Tramite.Campo_Nombre + ") = 'MOTIVO DE LA REGULARIZACIÓN'"
                 + " LEFT OUTER JOIN " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + " TDMA ON TS." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = TDMA." + Ope_Tra_Datos.Campo_Solicitud_ID
                 + " AND TS." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = TDMA." + Ope_Tra_Datos.Campo_Tramite_ID + " AND TDMA." + Ope_Tra_Datos.Campo_Dato_ID + " = CTDMA." + Cat_Tra_Datos_Tramite.Campo_Dato_ID

                 + " WHERE UPPER(TT." + (Cat_Tra_Tramites.Campo_Nombre) + ") = 'REGULARIZACION' "
                 + " AND TS." + (Ope_Tra_Solicitud.Campo_Estatus).ToUpper() + " = '" + "PENDIENTE" + "' ";
                if (Tramite.P_Solicitud_Id != null && Tramite.P_Solicitud_Id.Trim() != "")
                {
                    My_Sql += " AND TS." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Tramite.P_Solicitud_Id + "' AND ";
                }
                if (Tramite.P_Cuenta_Predial != null && Tramite.P_Cuenta_Predial.Trim() != "")
                {
                    My_Sql +=" AND TS." + Ope_Tra_Solicitud.Campo_Cuenta_Predial + " = '" + Tramite.P_Cuenta_Predial + "' AND ";
                }
                if (Tramite.P_Solicitante != null && Tramite.P_Solicitante.Trim() != "")
                {
                    My_Sql += " TS." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + "||' '||" + " TS." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + "||' '||" + " TS." + Ope_Tra_Solicitud.Campo_Apellido_Materno + " LIKE" + " '%" + Tramite.P_Solicitante + "%'";
                }
                if (My_Sql.EndsWith(" AND "))
                {
                    My_Sql = My_Sql.Substring(0, My_Sql.Length - 5);
                }
                if (My_Sql.EndsWith(" WHERE "))
                {
                    My_Sql = My_Sql.Substring(0, My_Sql.Length - 7);
                }       
                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Tramites = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Tramites_Avaluo_Rustico_Regularizacion: Error al consultar tramites de Avalúo Rústico.");
            }
            return Dt_Tramites;

        }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Modificar_Estatus_Avaluo_Rustico_Regularizacion
    ///DESCRIPCIÓN: Modificara el estatus del avaluo cuando sea autorizado
    ///PARAMETROS:     
    ///           
    ///CREO: Alejandro Leyva Alvarado
    ///FECHA_CREO: 29/Agosto/2012 
    ///MODIFICO             : 
    ///FECHA_MODIFICO       : 
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public static Boolean Modificar_Estatus_Avaluo_Rustico_Regularizacion(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Avaluo)
        {

            Boolean Alta = false;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_sql = "";
           
            try
            {
                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Rustico_R.Tabla_Ope_Cat_Avaluo_Rustico_R + " SET ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Estatus + "= '" + Avaluo.P_Estatus + "', ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Fecha_Autorizo + "= SYSDATE";               
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Rustico_R.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                Mi_sql += Ope_Cat_Avaluo_Rustico_R.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                if (Avaluo.P_Estatus == "AUTORIZADO")
                {
                    Mi_sql = "UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud;
                    Mi_sql += " SET " + Ope_Tra_Solicitud.Campo_Costo_Base + " = " + Avaluo.P_Importe_Avaluo + ", ";
                    Mi_sql += Ope_Tra_Solicitud.Campo_Cantidad + " = 1, ";
                    Mi_sql += Ope_Tra_Solicitud.Campo_Costo_Total + " = " + Avaluo.P_Importe_Avaluo;
                    Mi_sql += " WHERE TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ") = '" + Avaluo.P_Solicitud_Id + "'";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();

                    Cls_Ope_Bandeja_Tramites_Negocio Neg_Actualizar_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                    System.Text.StringBuilder My_SQL = new System.Text.StringBuilder();
                    My_SQL.Append("SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                        + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                        + "," + Ope_Tra_Solicitud.Campo_Estatus
                        + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                        + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ")) = UPPER(TRIM('" + Avaluo.P_Solicitud_Id + "'))");
                    Cmd.CommandText = My_SQL.ToString();
                    Cmd.CommandType = CommandType.Text;
                    OracleDataReader Dtr_Datos_Solicitud = Cmd.ExecuteReader();

                    // si hay datos para leer, agregar pasivo
                    if (Dtr_Datos_Solicitud.Read())
                    {
                        Neg_Actualizar_Solicitud.P_Comando_Oracle = Cmd;
                        Neg_Actualizar_Solicitud.P_Solicitud_ID = Avaluo.P_Solicitud_Id;
                        Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Consultar_Datos_Solicitud(); // Se obtienen los Datos a Detalle de la Solicitud Seleccionada

                        if (Neg_Actualizar_Solicitud.P_Tipo_Actividad == "CONDICION")
                        {
                            Neg_Actualizar_Solicitud.P_Respuesta_Condicion = Avaluo.P_Generar_Cobro; // SI ó NO 
                            if (Avaluo.P_Generar_Cobro == "SI")
                                Neg_Actualizar_Solicitud.P_Condicion_Si = Convert.ToDouble(Neg_Actualizar_Solicitud.P_Condicion_Si);//NUMERO DE LA ACTIVIDAD
                            else
                                Neg_Actualizar_Solicitud.P_Condicion_No = Convert.ToDouble(Neg_Actualizar_Solicitud.P_Condicion_No);//NUMERO DE LA ACTIVIDAD
                        }
                        Neg_Actualizar_Solicitud.P_Comando_Oracle = Cmd;
                        // establecer parámetros para actualizar solicitud
                        Neg_Actualizar_Solicitud.P_Solicitud_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString().Trim();
                        Neg_Actualizar_Solicitud.P_Estatus = "APROBAR";
                        Neg_Actualizar_Solicitud.P_Subproceso_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString().Trim();
                        Neg_Actualizar_Solicitud.P_Comentarios = "REGULARIZACIÓN AUTORIZADO";
                        Neg_Actualizar_Solicitud.P_Usuario = Cls_Sessiones.Nombre_Ciudadano;
                        // llamar método que actualizar la solicitud
                        Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Evaluar_Solicitud();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Estatus_Avaluo_Rustico: " + E.Message);
            }
            Trans.Commit();
            return Alta;        
        }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Consultar_Solicitudes_Regularizaciones
    /////DESCRIPCIÓN: Obtiene las solicitudes de regularizaciones
    /////PARAMETROS:
    /////CREO: David Herrera Rincon
    /////FECHA_CREO: 23/Octubre/2012 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //public static DataTable Consultar_Solicitudes_Regularizaciones(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Avaluo)
    //{
    //    DataTable Dt_Regularizaciones = new DataTable();
    //    String My_Sql = "";
    //    try
    //    {
    //        My_Sql = "SELECT (S." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " ||' '|| S." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " ||''|| S." + Ope_Tra_Solicitud.Campo_Apellido_Materno + ") AS Nombre,";
    //        My_Sql += " S." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " AS Fecha_Ingreso, Ro." + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Recepcion + ", Ro." + Ope_Cat_Recepcion_Oficios.Campo_Fecha_Recepcion + ",";
    //        My_Sql += " Ro." + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Respuesta + ", Ro." + Ope_Cat_Recepcion_Oficios.Campo_Fecha_Respuesta + ", UPPER(D." + Ope_Tra_Datos.Campo_Valor + ") AS Motivo_Regularizacion";
    //        My_Sql += " FROM " + Ope_Cat_Recepcion_Oficios.Tabla_Ope_Cat_Recepcion_Oficios + " Ro LEFT OUTER JOIN " + Ope_Cat_Avaluo_Rustico_R.Tabla_Ope_Cat_Avaluo_Rustico_R + " Au";
    //        My_Sql += " ON Ro." + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio + " = Au." + Ope_Cat_Avaluo_Rustico_R.Campo_No_Oficio + " LEFT OUTER JOIN " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " S";
    //        My_Sql += " ON Au." + Ope_Cat_Avaluo_Rustico_R.Campo_Solicitud_Id + " = S." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " T";
    //        My_Sql += " ON S." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = T." + Cat_Tra_Tramites.Campo_Tramite_ID + " LEFT OUTER JOIN " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + " Tr";
    //        My_Sql += " ON T." + Cat_Tra_Tramites.Campo_Tramite_ID + " = Tr." + Cat_Tra_Datos_Tramite.Campo_Tramite_ID + " LEFT OUTER JOIN " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + " D";
    //        My_Sql += " ON Tr." + Cat_Tra_Datos_Tramite.Campo_Dato_ID + " = D." + Ope_Tra_Datos.Campo_Dato_ID + "";
    //        My_Sql += " WHERE upper(tr." + Cat_Tra_Datos_Tramite.Campo_Nombre + ") LIKE '%REGULARIZA%'";
    //        My_Sql += " AND (S." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " BETWEEN TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Avaluo.P_Valor_Inpa) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')  AND TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Avaluo.P_Valor_Inpr) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS'))";

    //        DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
    //        if (Ds_Tabla != null)
    //        {
    //            Dt_Regularizaciones = Ds_Tabla.Tables[0];
    //        }
    //    }
    //    catch (Exception E)
    //    {
    //        throw new Exception("Consultar_Solicitudes_Inconformidad: Error al consultar las Solicitudes de Inconformidades.");
    //    }
    //    return Dt_Regularizaciones;
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Solicitud_Tramite_Avaluos
    ///DESCRIPCIÓN: consultar tipos de tramites de solicitudes
    ///PARAMETROS:     
    ///           
    ///CREO: Alejandro Leyva Alvarado
    ///FECHA_CREO: 29/Agosto/2012 
    ///MODIFICO             : 
    ///FECHA_MODIFICO       : 
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public static DataTable Consultar_Solicitud_Tramite_Avaluos(Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio Tramite)
    {
        DataTable Dt_Tramites = new DataTable();
        String My_Sql = "";
        try
        {
            My_Sql = "SELECT TS." + Ope_Tra_Solicitud.Campo_Solicitud_ID
             + ", TS." + Ope_Tra_Solicitud.Campo_Apellido_Materno
             + ", TS." + Ope_Tra_Solicitud.Campo_Apellido_Paterno
             + ", TS." + Ope_Tra_Solicitud.Campo_Cantidad
             + ", TS." + Ope_Tra_Solicitud.Campo_Clave_Solicitud
             + ", TS." + Ope_Tra_Solicitud.Campo_Comentarios
             + ", TS." + Ope_Tra_Solicitud.Campo_Correo_Electronico
             + ", TS." + Ope_Tra_Solicitud.Campo_Costo_Base
             + ", TS." + Ope_Tra_Solicitud.Campo_Costo_Total
             + ", TS." + Ope_Tra_Solicitud.Campo_Cuenta_Predial
             + ", TS." + Ope_Tra_Solicitud.Campo_Empleado_ID
             + ", TS." + Ope_Tra_Solicitud.Campo_Estatus
             + ", TS." + Ope_Tra_Solicitud.Campo_Fecha_Creo
             + ", TS." + Ope_Tra_Solicitud.Campo_Fecha_Entrega
             + ", TS." + Ope_Tra_Solicitud.Campo_Fecha_Modifico
             + ", TS." + Ope_Tra_Solicitud.Campo_Folio
             + ", TS." + Ope_Tra_Solicitud.Campo_Inspector_ID
             + ", TS." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante
             + ", TS." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance
             + ", TS." + Ope_Tra_Solicitud.Campo_Subproceso_ID
             + ", TS." + Ope_Tra_Solicitud.Campo_Tramite_ID
             + ", TS." + Ope_Tra_Solicitud.Campo_Usuario_Creo
             + ", TS." + Ope_Tra_Solicitud.Campo_Usuario_Modifico
             + ", TS." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + "||' '|| TS." + Ope_Tra_Solicitud.Campo_Apellido_Materno + "||' '||TS." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " AS NOMBRE_COMPLETO"
             + ", TS." + Ope_Tra_Solicitud.Campo_Zona_ID
             + ", TDMA." + Ope_Tra_Datos.Campo_Valor + " AS MOTIVO_REGULARIZACION"
             + " FROM  " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " TS"
             + " LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TT"
             + " ON TS." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = TT." + Cat_Tra_Tramites.Campo_Tramite_ID

             + " LEFT OUTER JOIN " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + " CTDMA ON  TS." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = CTDMA." + Cat_Tra_Datos_Tramite.Campo_Tramite_ID
             + " AND UPPER(CTDMA." + Cat_Tra_Datos_Tramite.Campo_Nombre + ") = 'MOTIVO DE LA REGULARIZACIÓN'"
             + " LEFT OUTER JOIN " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + " TDMA ON TS." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = TDMA." + Ope_Tra_Datos.Campo_Solicitud_ID
             + " AND TS." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = TDMA." + Ope_Tra_Datos.Campo_Tramite_ID + " AND TDMA." + Ope_Tra_Datos.Campo_Dato_ID + " = CTDMA." + Cat_Tra_Datos_Tramite.Campo_Dato_ID

             + " WHERE UPPER(TT." + (Cat_Tra_Tramites.Campo_Nombre) + ") = 'REGULARIZACION' "
             + " AND TS." + (Ope_Tra_Solicitud.Campo_Estatus).ToUpper() + " = '" + "PROCESO" + "' ";
            if (Tramite.P_Solicitud_Id != null && Tramite.P_Solicitud_Id.Trim() != "")
            {
                My_Sql += " AND TS." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Tramite.P_Solicitud_Id + "' AND ";
            }
            if (Tramite.P_Cuenta_Predial != null && Tramite.P_Cuenta_Predial.Trim() != "")
            {
                My_Sql += " AND TS." + Ope_Tra_Solicitud.Campo_Cuenta_Predial + " = '" + Tramite.P_Cuenta_Predial + "' AND ";
            }
            if (Tramite.P_Solicitante != null && Tramite.P_Solicitante.Trim() != "")
            {
                My_Sql += " TS." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + "||' '||" + " TS." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + "||' '||" + " TS." + Ope_Tra_Solicitud.Campo_Apellido_Materno + " LIKE" + " '%" + Tramite.P_Solicitante + "%'";
            }
            if (My_Sql.EndsWith(" AND "))
            {
                My_Sql = My_Sql.Substring(0, My_Sql.Length - 5);
            }
            if (My_Sql.EndsWith(" WHERE "))
            {
                My_Sql = My_Sql.Substring(0, My_Sql.Length - 7);
            }
            DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
            if (Ds_Tabla != null)
            {
                Dt_Tramites = Ds_Tabla.Tables[0];
            }
        }
        catch (Exception E)
        {
            throw new Exception("Consultar_Tramites_Avaluo_Rustico_Regularizacion: Error al consultar tramites de Avalúo Rústico.");
        }
        return Dt_Tramites;

    }
    }
}
