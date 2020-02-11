using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Cat_Avaluo_Urbano.Negocio;
using System.Data;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Sessiones;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Cat_Avaluo_Urbano_Datos
/// </summary>

namespace Presidencia.Operacion_Cat_Avaluo_Urbano.Datos
{
    public class Cls_Ope_Cat_Avaluo_Urbano_Datos
    {
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Avaluo_Urbano
        ///DESCRIPCIÓN: Da de alta en la Base de Datos Todos los datos del Avalúo
        ///PARAMENTROS:     
        ///             1. Avaluo.       Instancia de la Clase de Negocio de Avalúos Urbanos
        ///                              con los datos del que van a ser
        ///                              dados de Alta.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 18/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Avaluo_Urbano(Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo)
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
            String No_Avaluo = Obtener_ID_Consecutivo(Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano, Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo, Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + "=" + Avaluo.P_Anio_Avaluo, 10);
            String No_Colindancia = Obtener_ID_Consecutivo(Ope_Cat_Colindancias_Aua.Tabla_Ope_Cat_Colindancias_Aua, Ope_Cat_Colindancias_Aua.Campo_No_Colindancia, "", 10);
            String No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Documentos_Avaluo_Au.Tabla_Ope_Cat_Doc_Avaluo_Ur_Au, Ope_Cat_Documentos_Avaluo_Au.Campo_No_Documento, Ope_Cat_Documentos_Avaluo_Au.Campo_Anio_Documento + "=" + Avaluo.P_Anio_Avaluo, 10);
            try
            {

                Mi_sql = "INSERT INTO " + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano + "(";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Cuenta_Predial_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Motivo_Avaluo_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Solicitante + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Observaciones + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Ruta_Fachada_Inmueble + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Valor_Total_Predio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Inpr + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Inpa + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Valor_VR + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Fecha_Autorizo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Solicitud_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Estatus + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Cuenta_Predial + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Region + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Manzana + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Lote + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Ubicacion + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Colonia + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Localidad + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Municipio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Propietario + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Domicilio_Notificacion + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Colonia_Notificacion + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Localidad_Notificacion + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Municipio_Notificacion + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Veces_Rechazo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Coord_Norte + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Coord_Oriente + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Coord_Poniente + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Coord_Sur + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Comentarios_Perito + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Permitir_Revision + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Usuario_Creo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Fecha_Creo;
                Mi_sql += ") VALUES ('";
                Mi_sql += No_Avaluo + "', ";
                Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                Mi_sql += Avaluo.P_Cuenta_Predial_Id + "', '";
                Mi_sql += Avaluo.P_Motivo_Avaluo_Id + "', '";
                Mi_sql += Avaluo.P_Solicitante + "', '";
                Mi_sql += Avaluo.P_Observaciones + "', '";
                Mi_sql += Avaluo.P_Ruta_Fachada_Inmueble + "', ";
                Mi_sql += Avaluo.P_Valor_Total_Predio + ", ";
                Mi_sql += Avaluo.P_Valor_Inpr + ", ";
                Mi_sql += Avaluo.P_Valor_Inpa + ", ";
                Mi_sql += Avaluo.P_Valor_Vr + ", ";
                Mi_sql += "NULL, '";
                Mi_sql += Avaluo.P_Solicitud_Id + "', '";
                Mi_sql += Avaluo.P_Estatus + "', '";
                Mi_sql += Avaluo.P_Cuenta_Predial + "', '";
                Mi_sql += Avaluo.P_Region + "', '";
                Mi_sql += Avaluo.P_Manzana + "', '";
                Mi_sql += Avaluo.P_Lote + "', '";
                Mi_sql += Avaluo.P_Ubicacion + "', '";
                Mi_sql += Avaluo.P_Colonia + "', '";
                Mi_sql += Avaluo.P_Localidad + "', '";
                Mi_sql += Avaluo.P_Municipio + "', '";
                Mi_sql += Avaluo.P_Propietario + "', '";
                Mi_sql += Avaluo.P_Domicilio_Notificar + "', '";
                Mi_sql += Avaluo.P_Colonia_Notificar + "', '";
                Mi_sql += Avaluo.P_Localidad_Notificar + "', '";
                Mi_sql += Avaluo.P_Municipio_Notificar + "', ";
                Mi_sql +="0, '";
                Mi_sql += Avaluo.P_Coord_Norte + "', '";
                Mi_sql += Avaluo.P_Coord_Oriente + "', '";
                Mi_sql += Avaluo.P_Coord_Poniente + "', '";
                Mi_sql += Avaluo.P_Coord_Sur + "', '";
                Mi_sql += Avaluo.P_Comentarios_Perito + "', '";
                Mi_sql += Avaluo.P_Permitir_Revision + "', '";
                Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += "SYSDATE";
                Mi_sql += ")";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Caracteristicas_Terreno.Rows)
                {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Caract_Terreno_Au.Tabla_Ope_Cat_Caract_Terreno_Au + "(";
                        Mi_sql += Ope_Cat_Caract_Terreno_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caract_Terreno_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Caract_Terreno_Au.Campo_Vias_Acceso + ", ";
                        Mi_sql += Ope_Cat_Caract_Terreno_Au.Campo_Fotografia + ", ";
                        Mi_sql += Ope_Cat_Caract_Terreno_Au.Campo_Dens_Const + ", ";
                        Mi_sql += Ope_Cat_Caract_Terreno_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Caract_Terreno_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["VIAS_ACCESO"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["FOTOGRAFIA"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["DENS_CONST"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Clasificacion_Zona.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Au.Tabla_Ope_Cat_Clasificacion_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_A_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_A_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_B_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Au.Tabla_Ope_Cat_Clasificacion_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_B_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_B_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_C_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Au.Tabla_Ope_Cat_Clasificacion_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_C_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_C_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_D_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Au.Tabla_Ope_Cat_Clasificacion_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_D_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_D_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Servicios_Zona.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicios_Zona_Au.Tabla_Ope_Cat_Servicios_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Servicio_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Valor_Servicios_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_A_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_A_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_B_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicios_Zona_Au.Tabla_Ope_Cat_Servicios_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Servicio_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Valor_Servicios_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_B_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_B_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_C_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicios_Zona_Au.Tabla_Ope_Cat_Servicios_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Servicio_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Valor_Servicios_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_C_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_C_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_D_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicios_Zona_Au.Tabla_Ope_Cat_Servicios_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Servicio_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Valor_Servicios_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_D_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_D_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Construccion_Dominante.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Au.Tabla_Ope_Cat_Const_Dominante_Au + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Construccion_Dominantea_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Valor_Construccion_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_A_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_A_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_B_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Au.Tabla_Ope_Cat_Const_Dominante_Au + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Construccion_Dominantea_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Valor_Construccion_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_B_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_B_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_C_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Au.Tabla_Ope_Cat_Const_Dominante_Au + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Construccion_Dominantea_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Valor_Construccion_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_C_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_C_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Construccion.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Construccion_Au.Tabla_Ope_Cat_Construccion_Au + "(";
                    Mi_sql += Ope_Cat_Construccion_Au.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Construccion_Au.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Construccion_Au.Campo_Tipo_Construccion + ", ";
                    Mi_sql += Ope_Cat_Construccion_Au.Campo_Calidad_Proyecto + ", ";
                    Mi_sql += Ope_Cat_Construccion_Au.Campo_Uso_Construccion + ", ";
                    Mi_sql += Ope_Cat_Construccion_Au.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Construccion_Au.Campo_Fecha_Creo;
                    Mi_sql += ") VALUES ('";
                    Mi_sql += No_Avaluo + "', ";
                    Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                    Mi_sql += Dr_Renglon["TIPO_CONSTRUCCION"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["CALIDAD_PROYECTO"].ToString() + "', '";
                    Mi_sql += Dr_Renglon["USO_CONSTRUCCION"].ToString() + "', '";
                    Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += "SYSDATE";
                    Mi_sql += ")";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Elementos_Construccion.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Elem_Construccion_Au.Tabla_Ope_Cat_Elem_Construccion_Au + "(";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elementos_Contruccion_Id + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_A + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_B + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_C + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_D + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_E + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_F + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_G + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_H + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_I + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_J + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_K + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_L + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_M + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_N + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_O + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Fecha_Creo;
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
                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Const_Au.Tabla_Ope_Cat_Calc_Valor_Const_Au + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Referencia + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Valor_Construccion_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Fecha_Creo;
                    Mi_sql += ") VALUES ('";
                    Mi_sql += No_Avaluo + "', ";
                    Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                    Mi_sql += Dr_Renglon["REFERENCIA"].ToString() + "', ";
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
                    Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += "SYSDATE";
                    Mi_sql += ")";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Terreno.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Terreno_Au.Tabla_Ope_Cat_Calc_Valor_Terreno_Au + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Seccion + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Valor_Tramo_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Factor_EF + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Fecha_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Orden;
                    Mi_sql += ") VALUES ('";
                    Mi_sql += No_Avaluo + "', ";
                    Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                    Mi_sql += Dr_Renglon["SECCION"].ToString() + "', ";
                    Mi_sql += Dr_Renglon["SUPERFICIE_M2"].ToString() + ", ";
                    if (Dr_Renglon["VALOR_TRAMO_ID"].ToString().Trim() != "")
                    {
                        Mi_sql += "'" + Dr_Renglon["VALOR_TRAMO_ID"].ToString() + "', ";
                    }
                    else
                    {
                        Mi_sql += "NULL, ";
                    }
                    Mi_sql += "" + Dr_Renglon["FACTOR"].ToString() + ", ";
                    Mi_sql += Dr_Renglon["FACTOR_EF"].ToString() + ", ";
                    Mi_sql += Dr_Renglon["VALOR_PARCIAL"].ToString() + ", '";
                    Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += "SYSDATE, ";
                    if (Dr_Renglon["SECCION"].ToString().Trim() == "I")
                    {
                        Mi_sql += "1";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "II")
                    {
                        Mi_sql += "2";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "III")
                    {
                        Mi_sql += "3";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "IV")
                    {
                        Mi_sql += "4";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "V")
                    {
                        Mi_sql += "5";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "VI")
                    {
                        Mi_sql += "6";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "VII")
                    {
                        Mi_sql += "7";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "VIII")
                    {
                        Mi_sql += "8";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "INQ.ESQ.")
                    {
                        Mi_sql += "9";
                    }
                    else
                    {
                        Mi_sql += "10";
                    }
                    Mi_sql += ")";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Archivos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Documentos_Avaluo_Au.Tabla_Ope_Cat_Doc_Avaluo_Ur_Au + "(";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_Anio_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Documentos_Avaluo_Au.Campo_Documento].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos_Avaluo_Au/" + Avaluo.P_Anio_Avaluo + "_" + No_Avaluo + "/" + Dr_Renglon[Ope_Cat_Doc_Perito_Externo.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Documento = (Convert.ToInt32(No_Documento) + 1).ToString("0000000000");
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Medidas.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Colindancias_Aua.Tabla_Ope_Cat_Colindancias_Aua + "(";
                        Mi_sql += Ope_Cat_Colindancias_Aua.Campo_No_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aua.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aua.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aua.Campo_Medida_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aua.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aua.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Colindancia + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += No_Avaluo + "', '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Colindancias_Aua.Campo_Medida_Colindancia].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Colindancia = (Convert.ToInt32(No_Colindancia) + 1).ToString("0000000000");
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
                    Neg_Actualizar_Solicitud.P_Comando_Oracle = Cmd;
                    // establecer parámetros para actualizar solicitud
                    Neg_Actualizar_Solicitud.P_Solicitud_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString().Trim();
                    Neg_Actualizar_Solicitud.P_Estatus = "APROBAR";
                    Neg_Actualizar_Solicitud.P_Subproceso_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString().Trim();
                    Neg_Actualizar_Solicitud.P_Comentarios = "AVALÚO URBANO REGISTRADO";
                    Neg_Actualizar_Solicitud.P_Usuario = Cls_Sessiones.Nombre_Ciudadano;
                    // llamar método que actualizar la solicitud
                    Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Evaluar_Solicitud();
                }

                Alta = true;
                Trans.Commit();
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Avaluo_Urbano: " + E.Message);
            }
            finally
            {
                Cn.Close();
            }
            Avaluo.P_No_Avaluo = No_Avaluo;
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Valor_Construccion
        ///DESCRIPCIÓN: Modifica en la Base de Datos el tipo de construcción y elimina, agrega y/o modifica la tabla de valores
        ///PARAMENTROS:     
        ///             1. Tabla_Val.       Instancia de la Clase de Negocio de Tabla de valores de construcción
        ///                                 con los datos del que van a ser
        ///                                 dados de Alta, eliminados y/o modificados.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 03/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Modificar_Avaluo_Urbano(Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo)
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
            String No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Documentos_Avaluo_Au.Tabla_Ope_Cat_Doc_Avaluo_Ur_Au, Ope_Cat_Documentos_Avaluo_Au.Campo_No_Documento, Ope_Cat_Documentos_Avaluo_Au.Campo_Anio_Documento + "=" + Avaluo.P_Anio_Avaluo, 10);
            String No_Colindancia = Obtener_ID_Consecutivo(Ope_Cat_Colindancias_Aua.Tabla_Ope_Cat_Colindancias_Aua, Ope_Cat_Colindancias_Aua.Campo_No_Colindancia, "", 10);
            try
            {

                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano + " SET ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Motivo_Avaluo_Id + "= '" + Avaluo.P_Motivo_Avaluo_Id + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Solicitante + "= '" + Avaluo.P_Solicitante + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Observaciones + "= '" + Avaluo.P_Observaciones + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Cuenta_Predial + "= '" + Avaluo.P_Cuenta_Predial + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Region + "= '" + Avaluo.P_Region + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Manzana + "= '" + Avaluo.P_Manzana + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Lote + "= '" + Avaluo.P_Lote + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Ubicacion + "= '" + Avaluo.P_Ubicacion + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Colonia + "= '" + Avaluo.P_Colonia + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Localidad + "= '" + Avaluo.P_Localidad + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Municipio + "= '" + Avaluo.P_Municipio + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Propietario + "= '" + Avaluo.P_Propietario + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Domicilio_Notificacion + "= '" + Avaluo.P_Domicilio_Notificar + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Colonia_Notificacion + "= '" + Avaluo.P_Colonia_Notificar + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Localidad_Notificacion + "= '" + Avaluo.P_Localidad_Notificar + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Municipio_Notificacion + "= '" + Avaluo.P_Municipio_Notificar + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Ruta_Fachada_Inmueble + "= '" + Avaluo.P_Ruta_Fachada_Inmueble + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Valor_Total_Predio + "= " + Avaluo.P_Valor_Total_Predio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Inpr + "= " + Avaluo.P_Valor_Inpr + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Inpa + "= " + Avaluo.P_Valor_Inpa + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Valor_VR + "= " + Avaluo.P_Valor_Vr + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Estatus + "= '" + Avaluo.P_Estatus + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Permitir_Revision + "= '" + Avaluo.P_Permitir_Revision + "', ";

                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Coord_Norte + "= '" + Avaluo.P_Coord_Norte + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Coord_Oriente + "= '" + Avaluo.P_Coord_Oriente + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Coord_Poniente + "= '" + Avaluo.P_Coord_Poniente + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Coord_Sur + "= '" + Avaluo.P_Coord_Sur + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Comentarios_Perito + "= '" + Avaluo.P_Comentarios_Perito + "', ";

                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Usuario_Modifico + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Fecha_Modifico + "= SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Caracteristicas_Terreno.Rows)
                {
                    Mi_sql = "UPDATE " + Ope_Cat_Caract_Terreno_Au.Tabla_Ope_Cat_Caract_Terreno_Au + " SET ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Au.Campo_Vias_Acceso + "= '" + Dr_Renglon["VIAS_ACCESO"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Au.Campo_Fotografia + "= '" + Dr_Renglon["FOTOGRAFIA"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Au.Campo_Dens_Const + "= '" + Dr_Renglon["DENS_CONST"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Au.Campo_Usuario_Modifico + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Au.Campo_Fecha_Modifico + "= SYSDATE";
                    Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Construccion.Rows)
                {
                    Mi_sql = "UPDATE " + Ope_Cat_Construccion_Au.Tabla_Ope_Cat_Construccion_Au + " SET ";
                    Mi_sql += Ope_Cat_Construccion_Au.Campo_Tipo_Construccion + "= '" + Dr_Renglon["TIPO_CONSTRUCCION"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Construccion_Au.Campo_Calidad_Proyecto + "= '" + Dr_Renglon["CALIDAD_PROYECTO"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Construccion_Au.Campo_Uso_Construccion + "= '" + Dr_Renglon["USO_CONSTRUCCION"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Construccion_Au.Campo_Usuario_Modifico + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += Ope_Cat_Construccion_Au.Campo_Fecha_Modifico + "= SYSDATE";
                    Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Elementos_Construccion.Rows)
                {
                    Mi_sql = "UPDATE " + Ope_Cat_Elem_Construccion_Au.Tabla_Ope_Cat_Elem_Construccion_Au + " SET ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_A + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_A"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_B + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_B"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_C + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_C"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_D + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_D"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_E + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_E"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_F + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_F"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_G + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_G"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_H + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_H"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_I + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_I"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_J + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_J"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_K + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_K"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_L + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_L"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_M + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_M"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_N + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_N"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_O + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_O"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Usuario_Creo + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Fecha_Creo + "= SYSDATE";
                    Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + " AND ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Au.Campo_Elementos_Contruccion_Id + "= '" + Dr_Renglon["ELEMENTOS_CONSTRUCCION_ID"].ToString() + "'";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                Mi_sql = "DELETE " + Ope_Cat_Calc_Valor_Const_Au.Tabla_Ope_Cat_Calc_Valor_Const_Au + " WHERE " + Ope_Cat_Calc_Valor_Const_Au.Campo_No_Avaluo
                + " = '" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Calc_Valor_Const_Au.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Construccion.Rows)
                {

                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Const_Au.Tabla_Ope_Cat_Calc_Valor_Const_Au + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Referencia + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Valor_Construccion_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Au.Campo_Fecha_Creo;
                    Mi_sql += ") VALUES ('";
                    Mi_sql += Avaluo.P_No_Avaluo + "', ";
                    Mi_sql += Avaluo.P_Anio_Avaluo + ", ";
                    Mi_sql += "'" + Dr_Renglon["REFERENCIA"].ToString() + "', ";
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
                    Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += "SYSDATE";
                    Mi_sql += ")";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                Mi_sql = "DELETE " + Ope_Cat_Calc_Valor_Terreno_Au.Tabla_Ope_Cat_Calc_Valor_Terreno_Au + " WHERE " + Ope_Cat_Calc_Valor_Terreno_Au.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Calc_Valor_Terreno_Au.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Terreno.Rows)
                {

                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Terreno_Au.Tabla_Ope_Cat_Calc_Valor_Terreno_Au + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Seccion + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Valor_Tramo_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Factor_EF + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Fecha_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Au.Campo_Orden;
                    Mi_sql += ") VALUES ('";
                    Mi_sql += Avaluo.P_No_Avaluo + "', ";
                    Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                    Mi_sql += Dr_Renglon["SECCION"].ToString() + "', ";
                    Mi_sql += Dr_Renglon["SUPERFICIE_M2"].ToString() + ", ";
                    if (Dr_Renglon["VALOR_TRAMO_ID"].ToString().Trim() != "")
                    {
                        Mi_sql += "'" + Dr_Renglon["VALOR_TRAMO_ID"].ToString() + "', ";
                    }
                    else
                    {
                        Mi_sql += "NULL, ";
                    }
                    Mi_sql += "" + Dr_Renglon["FACTOR"].ToString() + ", ";
                    Mi_sql += Dr_Renglon["FACTOR_EF"].ToString() + ", ";
                    Mi_sql += Dr_Renglon["VALOR_PARCIAL"].ToString() + ", '";
                    Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += "SYSDATE, ";
                    if (Dr_Renglon["SECCION"].ToString().Trim() == "I")
                    {
                        Mi_sql += "1";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "II")
                    {
                        Mi_sql += "2";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "III")
                    {
                        Mi_sql += "3";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "IV")
                    {
                        Mi_sql += "4";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "V")
                    {
                        Mi_sql += "5";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "VI")
                    {
                        Mi_sql += "6";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "VII")
                    {
                        Mi_sql += "7";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "VIII")
                    {
                        Mi_sql += "8";
                    }
                    else if (Dr_Renglon["SECCION"].ToString().Trim() == "INQ.ESQ.")
                    {
                        Mi_sql += "9";
                    }
                    else
                    {
                        Mi_sql += "10";
                    }
                    Mi_sql += ")";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                if (Avaluo.P_Dt_Observaciones != null && Avaluo.P_Dt_Observaciones.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Observaciones.Rows)
                    {
                        Mi_sql = "UPDATE " + Ope_Cat_Seguimiento_Avaluo_Au.Tabla_Ope_Cat_Seguimiento_Avaluo_Au;
                        Mi_sql += " SET " + Ope_Cat_Seguimiento_Avaluo_Au.Campo_Estatus + " = 'BAJA', ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Ope_Cat_Seguimiento_Avaluo_Au.Campo_No_Seguimiento + "='" + Dr_Renglon["NO_SEGUIMIENTO"].ToString() + "' AND ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Mi_sql = "DELETE " + Ope_Cat_Clasificacion_Zona_Au.Tabla_Ope_Cat_Clasificacion_Zona_Au + " WHERE " + Ope_Cat_Clasificacion_Zona_Au.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Clasificacion_Zona_Au.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Clasificacion_Zona.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Au.Tabla_Ope_Cat_Clasificacion_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_A_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_A_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_B_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Au.Tabla_Ope_Cat_Clasificacion_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_B_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_B_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_C_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Au.Tabla_Ope_Cat_Clasificacion_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_C_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_C_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_D_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Au.Tabla_Ope_Cat_Clasificacion_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_D_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_D_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Mi_sql = "DELETE " + Ope_Cat_Servicios_Zona_Au.Tabla_Ope_Cat_Servicios_Zona_Au + " WHERE " + Ope_Cat_Servicios_Zona_Au.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Servicios_Zona_Au.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Servicios_Zona.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicios_Zona_Au.Tabla_Ope_Cat_Servicios_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Servicio_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Valor_Servicios_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_A_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_A_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_B_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicios_Zona_Au.Tabla_Ope_Cat_Servicios_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Servicio_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Valor_Servicios_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_B_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_B_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_C_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicios_Zona_Au.Tabla_Ope_Cat_Servicios_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Servicio_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Valor_Servicios_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_C_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_C_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_D_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicios_Zona_Au.Tabla_Ope_Cat_Servicios_Zona_Au + "(";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Servicio_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Valor_Servicios_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicios_Zona_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_D_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_D_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Mi_sql = "DELETE " + Ope_Cat_Const_Dominante_Au.Tabla_Ope_Cat_Const_Dominante_Au + " WHERE " + Ope_Cat_Const_Dominante_Au.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Const_Dominante_Au.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Construccion_Dominante.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Au.Tabla_Ope_Cat_Const_Dominante_Au + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Construccion_Dominantea_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Valor_Construccion_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_A_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_A_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_B_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Au.Tabla_Ope_Cat_Const_Dominante_Au + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Construccion_Dominantea_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Valor_Construccion_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_B_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_B_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Dr_Renglon["COLUMNA_C_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Au.Tabla_Ope_Cat_Const_Dominante_Au + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Construccion_Dominantea_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Valor_Construccion_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon["COLUMNA_C_ID"].ToString() + "', '";
                        Mi_sql += Dr_Renglon["COLUMNA_C_VALOR"].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Archivos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Documentos_Avaluo_Au.Tabla_Ope_Cat_Doc_Avaluo_Ur_Au + "(";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_Anio_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Au.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Documentos_Avaluo_Au.Campo_Documento].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos_Avaluo_Au/" + Avaluo.P_Anio_Avaluo + "_" + Avaluo.P_No_Avaluo + "/" + Dr_Renglon[Ope_Cat_Doc_Perito_Externo.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Documento = (Convert.ToInt32(No_Documento) + 1).ToString("0000000000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["NO_DOCUMENTO"].ToString().Trim() != "")
                    {
                        Mi_sql = "DELETE " + Ope_Cat_Documentos_Avaluo_Au.Tabla_Ope_Cat_Doc_Avaluo_Ur_Au + " WHERE " + Ope_Cat_Documentos_Avaluo_Au.Campo_No_Documento;
                        Mi_sql += "='" + Dr_Renglon[Ope_Cat_Documentos_Avaluo_Au.Campo_No_Documento].ToString() + "' AND " + Ope_Cat_Documentos_Avaluo_Au.Campo_Anio_Documento + "=" + Avaluo.P_Anio_Avaluo;
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Medidas.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Colindancias_Aua.Tabla_Ope_Cat_Colindancias_Aua + "(";
                        Mi_sql += Ope_Cat_Colindancias_Aua.Campo_No_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aua.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aua.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aua.Campo_Medida_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aua.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aua.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Colindancia + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Avaluo.P_No_Avaluo + "', '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Colindancias_Aua.Campo_Medida_Colindancia].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Colindancia = (Convert.ToInt32(No_Colindancia) + 1).ToString("0000000000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon[Ope_Cat_Colindancias_Aua.Campo_No_Colindancia].ToString().Trim() != "")
                    {
                        Mi_sql = "DELETE " + Ope_Cat_Colindancias_Aua.Tabla_Ope_Cat_Colindancias_Aua + " WHERE " + Ope_Cat_Colindancias_Aua.Campo_No_Colindancia;
                        Mi_sql += "='" + Dr_Renglon[Ope_Cat_Colindancias_Aua.Campo_No_Colindancia].ToString() + "' AND " + Ope_Cat_Colindancias_Aua.Campo_Anio_Avaluo + "=" + Avaluo.P_Anio_Avaluo;
                        Mi_sql += " AND " + Ope_Cat_Colindancias_Aua.Campo_No_Avaluo + "= '" + Avaluo.P_No_Avaluo + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Alta = true;
                Trans.Commit();
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Avaluo_Urbano: " + E.Message);
            }
            finally
            {
                Cn.Close();
            }
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Observaciones_Avaluo_Au
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
        public static Boolean Modificar_Observaciones_Avaluo_Au(Cls_Ope_Cat_Avaluo_Urbano_Negocio Aval)
        {
            //if (Aval.P_Estatus == "AUTORIZADO")
            //{
            //    Cambiar_Importe_Tramite(Aval);
            //}
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
            No_Seguimiento = Obtener_ID_Consecutivo(Ope_Cat_Seguimiento_Avaluo_Au.Tabla_Ope_Cat_Seguimiento_Avaluo_Au, Ope_Cat_Seguimiento_Avaluo_Au.Campo_No_Seguimiento, "", 10);
            try
            {

                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano;
                Mi_sql += " SET " + Ope_Cat_Avaluo_Urbano.Campo_Estatus + " = '" + Aval.P_Estatus + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                if (Aval.P_Estatus == "AUTORIZADO")
                {
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Fecha_Autorizo + " = SYSDATE, ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Observaciones_Perito + " = '', ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Permitir_Revision + " = 'NO', ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Comentarios_Perito + " = '" + Aval.P_Comentarios_Perito + "', ";
                }
                //else if (Aval.P_Estatus == "POR AUTORIZAR" || Aval.P_Estatus == "POR VALIDAR")
                //{
                //    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Observaciones_Perito + " = '" + Aval.P_Observaciones_Perito + "', ";
                //    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Permitir_Revision + " = 'SI', ";
                //}
                else if (Aval.P_Estatus == "RECHAZADO")
                {
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Veces_Rechazo + " = " + Aval.P_Veces_Rechazo + ", ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Fecha_Rechazo + " = SYSDATE, ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Permitir_Revision + " = 'NO', ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Observaciones_Perito + " = '" + Aval.P_Observaciones_Perito + "', ";
                }
                    else if(Aval.P_Estatus == "RÉGIMEN")
                {
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Permitir_Revision + " = 'SI', ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Observaciones_Perito + " = '" + Aval.P_Observaciones_Perito + "', ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Estatus_Anterior + " = '" + Aval.P_Estatus_Anterior + "', ";
                }
                else
                {
                    if (Aval.P_Es_Primer_Revision)
                    {
                        Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Primer_Revision + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                    }
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Permitir_Revision + " = 'SI', ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Observaciones_Perito + " = '" + Aval.P_Observaciones_Perito + "', ";
                }
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + "=" + Aval.P_Anio_Avaluo;
                Mi_sql += " AND " + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + "='" + Aval.P_No_Avaluo + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Aval.P_Dt_Observaciones.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Seguimiento_Avaluo_Au.Tabla_Ope_Cat_Seguimiento_Avaluo_Au + "(";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_No_Seguimiento + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_Motivo_Id + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_Estatus + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_Fecha_Creo;
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
                        Mi_sql = "UPDATE " + Ope_Cat_Seguimiento_Avaluo_Au.Tabla_Ope_Cat_Seguimiento_Avaluo_Au;
                        Mi_sql += " SET " + Ope_Cat_Seguimiento_Avaluo_Au.Campo_Estatus + " = '" + Dr_Renglon["ESTATUS"].ToString() + "', ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Au.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Ope_Cat_Seguimiento_Avaluo_Au.Campo_No_Seguimiento + "='" + Dr_Renglon["NO_SEGUIMIENTO"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                if (Aval.P_Estatus == "AUTORIZADO")
                {
                    Mi_sql = "UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud;
                    Mi_sql += " SET " + Ope_Tra_Solicitud.Campo_Costo_Base + " = " + Aval.P_Importe_Avaluo + ", ";
                    Mi_sql += Ope_Tra_Solicitud.Campo_Cantidad + " = 1, ";
                    Mi_sql += Ope_Tra_Solicitud.Campo_Costo_Total + " = " + Aval.P_Importe_Avaluo;
                    Mi_sql += " WHERE TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ") = '" + Aval.P_Solicitud_Id + "'";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();

                    Cls_Ope_Bandeja_Tramites_Negocio Neg_Actualizar_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                    System.Text.StringBuilder My_SQL = new System.Text.StringBuilder();
                    My_SQL.Append("SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                        + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                        + "," + Ope_Tra_Solicitud.Campo_Estatus
                        + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                        + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ")) = UPPER(TRIM('" + Aval.P_Solicitud_Id + "'))");
                    Cmd.CommandText = My_SQL.ToString();
                    Cmd.CommandType = CommandType.Text;
                    OracleDataReader Dtr_Datos_Solicitud = Cmd.ExecuteReader();

                    // si hay datos para leer, agregar pasivo
                    if (Dtr_Datos_Solicitud.Read())
                    {
                        Neg_Actualizar_Solicitud.P_Comando_Oracle = Cmd;
                        // establecer parámetros para actualizar solicitud
                        Neg_Actualizar_Solicitud.P_Solicitud_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString().Trim();
                        Neg_Actualizar_Solicitud.P_Estatus = "APROBAR";
                        Neg_Actualizar_Solicitud.P_Subproceso_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString().Trim();
                        Neg_Actualizar_Solicitud.P_Comentarios = "AVALÚO URBANO AUTORIZADO";
                        Neg_Actualizar_Solicitud.P_Usuario = Cls_Sessiones.Nombre_Ciudadano;
                        // llamar método que actualizar la solicitud
                        Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Evaluar_Solicitud();
                    }
                }
                Alta = true;
                Trans.Commit();
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Observaciones_Avaluo_Au: " + E.Message);
            }
            finally
            {
                Cn.Close();
            }
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Avaluo_Regimen
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
        public static Boolean Modificar_Avaluo_Regimen(Cls_Ope_Cat_Avaluo_Urbano_Negocio Aval)
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
                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano;
                Mi_sql += " SET " + Ope_Cat_Avaluo_Urbano.Campo_Estatus + " = '" + Aval.P_Estatus + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Estatus_Anterior + " = '" + Aval.P_Estatus_Anterior + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Fecha_Modifico + " = SYSDATE, ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "'";
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + "=" + Aval.P_Anio_Avaluo;
                Mi_sql += " AND " + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + " = '" + Aval.P_No_Avaluo + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
                Trans.Commit();
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Avaluo_Regimen: " + E.Message);
            }
            finally
            {
                Cn.Close();
            }
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cambiar_Importe_Tramite
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
        public static Boolean Cambiar_Importe_Tramite(Cls_Ope_Cat_Avaluo_Urbano_Negocio Aval)
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
                Mi_sql = "UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud;
                Mi_sql += " SET " + Ope_Tra_Solicitud.Campo_Costo_Base + " = " + Aval.P_Importe_Avaluo + ", ";
                Mi_sql += Ope_Tra_Solicitud.Campo_Cantidad + " = 1, ";
                Mi_sql += Ope_Tra_Solicitud.Campo_Costo_Total + " = " + Aval.P_Importe_Avaluo;
                Mi_sql += " WHERE TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ") = '" + Aval.P_Solicitud_Id + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
                Trans.Commit();
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Cambiar_Importe_Tramite: " + E.Message);
            }
            finally
            {
                Cn.Close();
            }
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Observaciones_Avaluo_Au
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
        public static Boolean Asignar_Perito_Interno(Cls_Ope_Cat_Avaluo_Urbano_Negocio Aval)
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

                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano;
                Mi_sql += " SET " + Ope_Cat_Avaluo_Urbano.Campo_Perito_Interno_Id + " = '" + Aval.P_Perito_Interno_Id + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + "=" + Aval.P_Anio_Avaluo;
                Mi_sql += " AND " + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + "='" + Aval.P_No_Avaluo + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Asignar_Perito_Interno: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluo_Urbano
        ///DESCRIPCIÓN: Obtiene la tabla con los datos de avalúos urbanos
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 23/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Avaluo_Urbano(Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo)
        {
            DataTable Dt_Avaluo = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + "||'/'||TO_NUMBER(" + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + ") AS AVALUO"
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Motivo_Avaluo_Id
                    + ", (SELECT " + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion + " FROM " + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + " WHERE "
                    + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + "." + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id + "= "
                    + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano + "." + Ope_Cat_Avaluo_Urbano.Campo_Motivo_Avaluo_Id + ") AS MOTIVO_AVALUO"
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Cuenta_Predial_Id
                    //+ ", (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " 
                    //+ Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "= " 
                    //+ Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano + "." + Ope_Cat_Avaluo_Urbano.Campo_Cuenta_Predial_Id + ") AS CUENTA_PREDIAL"
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Solicitante
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Observaciones
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Ruta_Fachada_Inmueble
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Valor_Total_Predio
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Inpr
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Inpa
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Valor_VR
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Cuenta_Predial
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Region
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Manzana
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Lote
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Ubicacion
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Colonia
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Localidad
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Municipio
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Propietario
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Domicilio_Notificacion
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Colonia_Notificacion
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Localidad_Notificacion
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Municipio_Notificacion
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Fecha_Autorizo
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Fecha_Rechazo
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Veces_Rechazo
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Observaciones_Perito
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Coord_Norte
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Coord_Oriente
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Estatus_Anterior
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Coord_Poniente
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Solicitud_Id
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Coord_Sur
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Perito_Externo_Id
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Perito_Interno_Id
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Comentarios_Perito
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Estatus
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Permitir_Revision
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Primer_Revision
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Usuario_Modifico
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Fecha_Modifico
                    + " FROM  " + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano
                    + " WHERE ";
                if (Avaluo.P_No_Avaluo != null && Avaluo.P_No_Avaluo.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND ";
                }
                if (Avaluo.P_Anio_Avaluo != null && Avaluo.P_Anio_Avaluo.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo + " AND ";
                }
                if (Avaluo.P_Perito_Externo_Id != null && Avaluo.P_Perito_Externo_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_Perito_Externo_Id + " = '" + Avaluo.P_Perito_Externo_Id + "' AND ";
                }
                if (Avaluo.P_Solicitud_Id != null && Avaluo.P_Solicitud_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_Solicitud_Id + " = '" + Avaluo.P_Solicitud_Id + "' AND ";
                }
                if (Avaluo.P_Perito_Interno_Id != null && Avaluo.P_Perito_Interno_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_Perito_Interno_Id + " = '" + Avaluo.P_Perito_Interno_Id + "' AND ";
                }
                if (Avaluo.P_Cuenta_Predial != null && Avaluo.P_Cuenta_Predial.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_Cuenta_Predial + " = '" + Avaluo.P_Cuenta_Predial + "' AND ";
                }
                if (Avaluo.P_Permitir_Revision != null && Avaluo.P_Permitir_Revision.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_Permitir_Revision + " = '" + Avaluo.P_Permitir_Revision + "' AND ";
                }
                if (Avaluo.P_Perito_Externo != null && Avaluo.P_Perito_Externo.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_Perito_Externo_Id + " IN (SELECT PE." + Cat_Cat_Peritos_Externos.Campo_Perito_Externo_Id + " FROM " + Cat_Cat_Peritos_Externos.Tabla_Cat_Cat_Peritos_Externos + " PE WHERE PE." + Cat_Cat_Peritos_Externos.Campo_Nombre + "||' '||PE." + Cat_Cat_Peritos_Externos.Campo_Apellido_Paterno + "||' '||PE." + Cat_Cat_Peritos_Externos.Campo_Apellido_Materno + " LIKE '%" + Avaluo.P_Perito_Externo + "%') AND ";
                }
                if (Avaluo.P_Ubicacion != null && Avaluo.P_Ubicacion.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_Ubicacion + " LIKE '%" + Avaluo.P_Ubicacion + "%' AND ";
                }
                if (Avaluo.P_Folio != null && Avaluo.P_Folio.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + "||'/'||TO_NUMBER(" + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + ") = '" + Avaluo.P_Folio + "' AND ";
                }
                if (Avaluo.P_Estatus != null && Avaluo.P_Estatus.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_Estatus + " " + Avaluo.P_Estatus + " AND ";
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
                Mi_SQL += " ORDER BY " + Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + " DESC, " + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + " DESC";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Dt_Avaluo = dataset.Tables[0];
                }
                if ((Avaluo.P_No_Avaluo != null && Avaluo.P_No_Avaluo.Trim() != "") && (Avaluo.P_Anio_Avaluo != null && Avaluo.P_Anio_Avaluo.Trim() != ""))
                {

                    Mi_SQL = "SELECT " + Ope_Cat_Caract_Terreno_Au.Campo_Vias_Acceso
                    + ", " + Ope_Cat_Caract_Terreno_Au.Campo_Fotografia
                    + ", " + Ope_Cat_Caract_Terreno_Au.Campo_Dens_Const
                    + ", " + Ope_Cat_Caract_Terreno_Au.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Caract_Terreno_Au.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Caract_Terreno_Au.Tabla_Ope_Cat_Caract_Terreno_Au
                    + " WHERE "
                    + Ope_Cat_Caract_Terreno_Au.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Caract_Terreno_Au.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Caracteristicas_Terreno = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Construccion_Au.Campo_Tipo_Construccion
                    + ", " + Ope_Cat_Construccion_Au.Campo_Calidad_Proyecto
                    + ", " + Ope_Cat_Construccion_Au.Campo_Uso_Construccion
                    + ", " + Ope_Cat_Construccion_Au.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Construccion_Au.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Construccion_Au.Tabla_Ope_Cat_Construccion_Au
                    + " WHERE "
                    + Ope_Cat_Construccion_Au.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Construccion_Au.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Construccion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Elem_Construccion_Au.Campo_Elementos_Contruccion_Id
                    + ", (SELECT EC." + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion + " FROM " + Cat_Cat_Elementos_Construccion.Tabla_Cat_Cat_Elementos_Construccion + " EC WHERE EC." + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion_Id + "=" + Ope_Cat_Elem_Construccion_Au.Tabla_Ope_Cat_Elem_Construccion_Au + "." + Ope_Cat_Elem_Construccion_Au.Campo_Elementos_Contruccion_Id + ") AS ELEMENTOS_CONSTRUCCION"
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_A
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_B
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_C
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_D
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_E
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_F
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_G
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_H
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_I
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_J
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_K
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_L
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_M
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_N
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_O
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Elem_Construccion_Au.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Elem_Construccion_Au.Tabla_Ope_Cat_Elem_Construccion_Au
                    + " WHERE "
                    + Ope_Cat_Elem_Construccion_Au.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Elem_Construccion_Au.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Elementos_Construccion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT VC." + Ope_Cat_Calc_Valor_Const_Au.Campo_Referencia
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Au.Campo_Superficie_M2
                    + ", NVL(VC." + Ope_Cat_Calc_Valor_Const_Au.Campo_Valor_Construccion_Id + ", '') AS " + Ope_Cat_Calc_Valor_Const_Au.Campo_Valor_Construccion_Id
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Au.Campo_Factor
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Au.Campo_Valor_Parcial
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Au.Campo_Referencia
                    + ", NVL(TV." + Cat_Cat_Tab_Val_Construccion.Campo_Clave_Valor + ",0) AS CON_SERV"
                    + ", NVL(CC." + Cat_Cat_Calidad_Construccion.Campo_Clave_Calidad + ",0) AS TIPO"
                    + ", NVL(TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_M2 + ",0.00) AS VALOR_M2"
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Au.Campo_Usuario_Creo
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Au.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Calc_Valor_Const_Au.Tabla_Ope_Cat_Calc_Valor_Const_Au + " VC"
                    + " LEFT OUTER JOIN " + Cat_Cat_Tab_Val_Construccion.Tabla_Cat_Cat_Tab_Val_Construccion + " TV"
                    + " ON VC." + Ope_Cat_Calc_Valor_Const_Au.Campo_Valor_Construccion_Id + "=TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_Construccion_Id
                    + " LEFT OUTER JOIN " + Cat_Cat_Calidad_Construccion.Tabla_Cat_Cat_Calidad_Construccion + " CC"
                    + " ON TV." + Cat_Cat_Tab_Val_Construccion.Campo_Calidad_Id + "=CC." + Cat_Cat_Calidad_Construccion.Campo_Calidad_Id
                    + " WHERE "
                    + Ope_Cat_Elem_Construccion_Au.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Elem_Construccion_Au.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo
                    + " ORDER BY VC." + Ope_Cat_Calc_Valor_Const_Au.Campo_Referencia + " ASC";

                    Avaluo.P_Dt_Calculo_Valor_Construccion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT VC." + Ope_Cat_Calc_Valor_Terreno_Au.Campo_Seccion
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Au.Campo_Superficie_M2
                    + ", NVL(VT." + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo + ",0) AS " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo
                    + ", NVL(VT." + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id + ",'') AS " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Au.Campo_Factor
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Au.Campo_Factor_EF
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Au.Campo_Valor_Parcial
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Au.Campo_Usuario_Creo
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Au.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Calc_Valor_Terreno_Au.Tabla_Ope_Cat_Calc_Valor_Terreno_Au + " VC"
                    + " LEFT OUTER JOIN " + Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos + " VT"
                    + " ON VC." + Ope_Cat_Calc_Valor_Terreno_Au.Campo_Valor_Tramo_Id + "=VT." + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id
                    + " WHERE "
                    + Ope_Cat_Elem_Construccion_Au.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Elem_Construccion_Au.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo
                    + " ORDER BY VC." + Ope_Cat_Calc_Valor_Terreno_Au.Campo_Orden + " ASC";

                    Avaluo.P_Dt_Calculo_Valor_Terreno = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    //--





                    Mi_SQL = "SELECT OCZ." + Ope_Cat_Clasificacion_Zona_Au.Campo_Clasificacion_Zona_Id
                    + ", OCZ." + Ope_Cat_Clasificacion_Zona_Au.Campo_Valor_Clasificacion_Zona
                    + ", NVL(CZ." + Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona + ",'') AS " + Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona
                    + ", OCZ." + Ope_Cat_Clasificacion_Zona_Au.Campo_Usuario_Creo
                    + ", OCZ." + Ope_Cat_Clasificacion_Zona_Au.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Clasificacion_Zona_Au.Tabla_Ope_Cat_Clasificacion_Zona_Au + " OCZ"
                    + " LEFT OUTER JOIN " + Cat_Cat_Clasificacion_Zona.Tabla_Cat_Cat_Clasificacion_Zona + " CZ"
                    + " ON OCZ." + Ope_Cat_Clasificacion_Zona_Au.Campo_Clasificacion_Zona_Id + "=CZ." + Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona_Id
                    + " WHERE "
                    + Ope_Cat_Clasificacion_Zona_Au.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Clasificacion_Zona_Au.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Clasificacion_Zona = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT OSZ." + Ope_Cat_Servicios_Zona_Au.Campo_Servicio_Zona_Id
                    + ", OSZ." + Ope_Cat_Servicios_Zona_Au.Campo_Valor_Servicios_Zona
                    + ", NVL(SZ." + Cat_Cat_Servicios_Zona.Campo_Servicio_Zona + ",'') AS " + Cat_Cat_Servicios_Zona.Campo_Servicio_Zona
                    + ", OSZ." + Ope_Cat_Servicios_Zona_Au.Campo_Usuario_Creo
                    + ", OSZ." + Ope_Cat_Servicios_Zona_Au.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Servicios_Zona_Au.Tabla_Ope_Cat_Servicios_Zona_Au + " OSZ"
                    + " LEFT OUTER JOIN " + Cat_Cat_Servicios_Zona.Tabla_Cat_Cat_Servicios_Zona + " SZ"
                    + " ON OSZ." + Ope_Cat_Servicios_Zona_Au.Campo_Servicio_Zona_Id + "=SZ." + Cat_Cat_Servicios_Zona.Campo_Servicio_Zona_Id
                    + " WHERE "
                    + Ope_Cat_Servicios_Zona_Au.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Servicios_Zona_Au.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Servicios_Zona = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT OSZ." + Ope_Cat_Const_Dominante_Au.Campo_Construccion_Dominantea_Id
                    + ", OSZ." + Ope_Cat_Const_Dominante_Au.Campo_Valor_Construccion_Dominante
                    + ", NVL(SZ." + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante + ",'') AS " + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante
                    + ", OSZ." + Ope_Cat_Const_Dominante_Au.Campo_Usuario_Creo
                    + ", OSZ." + Ope_Cat_Const_Dominante_Au.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Const_Dominante_Au.Tabla_Ope_Cat_Const_Dominante_Au + " OSZ"
                    + " LEFT OUTER JOIN " + Cat_Cat_Construccion_Dominante.Tabla_Cat_Cat_Construccion_Dominante + " SZ"
                    + " ON OSZ." + Ope_Cat_Const_Dominante_Au.Campo_Construccion_Dominantea_Id + "=SZ." + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante_Id
                    + " WHERE "
                    + Ope_Cat_Const_Dominante_Au.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Const_Dominante_Au.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Construccion_Dominante = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Documentos_Avaluo_Au.Campo_No_Documento
                    + ", " + Ope_Cat_Documentos_Avaluo_Au.Campo_Anio_Documento
                    + ", " + Ope_Cat_Documentos_Avaluo_Au.Campo_Documento
                    + ", " + Ope_Cat_Documentos_Avaluo_Au.Campo_Ruta_Documento
                    + ", " + Ope_Cat_Documentos_Avaluo_Au.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Documentos_Avaluo_Au.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Documentos_Avaluo_Au.Campo_Fecha_Modifico
                    + ", " + Ope_Cat_Documentos_Avaluo_Au.Campo_Usuario_Modifico
                    + ", 'NADA' AS ACCION"
                    + " FROM  " + Ope_Cat_Documentos_Avaluo_Au.Tabla_Ope_Cat_Doc_Avaluo_Ur_Au
                    + " WHERE "
                    + Ope_Cat_Documentos_Avaluo_Au.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Documentos_Avaluo_Au.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Archivos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Colindancias_Aua.Campo_No_Colindancia
                    + ", " + Ope_Cat_Colindancias_Aua.Campo_Anio_Avaluo
                    + ", " + Ope_Cat_Colindancias_Aua.Campo_No_Avaluo
                    + ", " + Ope_Cat_Colindancias_Aua.Campo_Medida_Colindancia
                    + ", " + Ope_Cat_Colindancias_Aua.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Colindancias_Aua.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Colindancias_Aua.Campo_Fecha_Modifico
                    + ", " + Ope_Cat_Colindancias_Aua.Campo_Usuario_Modifico
                    + ", 'NADA' AS ACCION"
                    + " FROM  " + Ope_Cat_Colindancias_Aua.Tabla_Ope_Cat_Colindancias_Aua
                    + " WHERE "
                    + Ope_Cat_Colindancias_Aua.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Colindancias_Aua.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Medidas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Avalúos Urbanos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Avaluo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluo_Urbano_Asignados
        ///DESCRIPCIÓN: Obtiene la tabla con los datos de avalúos urbanos asignados a peritos internos
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 23/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Avaluo_Urbano_Asignados(Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo)
        {
            DataTable Dt_Avaluo = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + "||'/'||TO_NUMBER(" + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + ") AS AVALUO"
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Motivo_Avaluo_Id
                    + ", (SELECT " + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion + " FROM " + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + " WHERE "
                    + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + "." + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id + "= "
                    + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano + "." + Ope_Cat_Avaluo_Urbano.Campo_Motivo_Avaluo_Id + ") AS MOTIVO_AVALUO"
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Cuenta_Predial_Id
                    + ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||"+Cat_Empleados.Campo_Apellido_Paterno+"||' '||"+Cat_Empleados.Campo_Apellido_Materno+" FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE "
                    + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " IN (SELECT "
                    + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + "." + Cat_Cat_Peritos_Internos.Campo_Empleado_Id + " FROM "+Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos+" WHERE "+Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos+"."+Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id+" IN (SELECT "+Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano+"."+Ope_Cat_Avaluo_Urbano.Campo_Perito_Interno_Id+" FROM "+Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano+" WHERE "+Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano+"."+Ope_Cat_Avaluo_Urbano.Campo_Perito_Interno_Id+" IS NOT NULL AND "+Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano+"."+Ope_Cat_Avaluo_Urbano.Campo_Estatus+" IN ('POR VALIDAR','RECHAZADO')))) AS PERITO_INTERNO"
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Solicitante
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Observaciones
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Ruta_Fachada_Inmueble
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Valor_Total_Predio
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Inpr
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Inpa
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Valor_VR
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Fecha_Autorizo
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Perito_Externo_Id
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Perito_Interno_Id
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Estatus
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Usuario_Modifico
                    + ", " + Ope_Cat_Avaluo_Urbano.Campo_Fecha_Modifico
                    + " FROM  " + Ope_Cat_Avaluo_Urbano.Tabla_Ope_Cat_Avaluo_Urbano
                    + " WHERE "
                    + Ope_Cat_Avaluo_Urbano.Campo_Perito_Interno_Id + " IS NOT NULL AND ";
                if (Avaluo.P_Folio != null && Avaluo.P_Folio.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_Anio_Avaluo + "||'/'||TO_NUMBER(" + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + ") LIKE '" + Avaluo.P_Folio + "' AND ";
                }
                    Mi_SQL += Ope_Cat_Avaluo_Urbano.Campo_Estatus + " IN ('POR VALIDAR','RECHAZADO')";
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                // agregar filtro y orden a la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Dt_Avaluo = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Avalúos Urbanos asignados. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Avaluo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tipos_Construccion
        ///DESCRIPCIÓN: Obtiene todos los tipos de construcción que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Valores_Construccion(Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo)
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
                My_Sql += " ON TV." + Cat_Cat_Tab_Val_Construccion.Campo_Calidad_Id + " = CC." +Cat_Cat_Calidad_Construccion.Campo_Calidad_Id;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Elementos_Construccion
        ///DESCRIPCIÓN: Obtiene la tabla inicial para los elementos de construcción del avalúo urbano
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Elementos_Construccion(Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT " + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion_Id;
                My_Sql += ", " + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_A;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_B;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_C;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_D;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_E;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_F;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_G;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_H;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_I;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_J;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_K;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_L;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_M;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_N;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Au.Campo_Elemento_Construccion_O;
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
                throw new Exception("Consultar_Tabla_Elementos_Construccion: "+E.Message);
            }
            return Dt_Tramos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Clasificacion_Zona
        ///DESCRIPCIÓN: Obtiene la tabla inicial para las clasificaciones de la zona del avalúo urbano
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Clasificacion_Zona(Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT " + Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona_Id;
                My_Sql += ", " + Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona;
                My_Sql += ", '' AS VALOR_CLASIFICACION_ZONA";
                My_Sql += " FROM " + Cat_Cat_Clasificacion_Zona.Tabla_Cat_Cat_Clasificacion_Zona;
                My_Sql += " WHERE " + Cat_Cat_Clasificacion_Zona.Campo_Estatus + " = 'VIGENTE'";
                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Tramos = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Tabla_Clasificacion_Zona: " + E.Message);
            }
            return Dt_Tramos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Servicios_Zona
        ///DESCRIPCIÓN: Obtiene la tabla inicial para los Servicios de la zona del avalúo urbano
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Servicios_Zona(Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT " + Cat_Cat_Servicios_Zona.Campo_Servicio_Zona_Id;
                My_Sql += ", " + Cat_Cat_Servicios_Zona.Campo_Servicio_Zona;
                My_Sql += ", '' AS VALOR_SERVICIO_ZONA";
                My_Sql += " FROM " + Cat_Cat_Servicios_Zona.Tabla_Cat_Cat_Servicios_Zona;
                My_Sql += " WHERE " + Cat_Cat_Servicios_Zona.Campo_Estatus + " = 'VIGENTE'";
                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Tramos = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Tabla_Servicios_Zona: " + E.Message);
            }
            return Dt_Tramos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Const_Dominante
        ///DESCRIPCIÓN: Obtiene la tabla inicial para las construcciones dominantes zona del avalúo urbano
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Const_Dominante(Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT " + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante_Id;
                My_Sql += ", " + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante;
                My_Sql += ", '' AS VALOR_CONST_DOMINANTE";
                My_Sql += " FROM " + Cat_Cat_Construccion_Dominante.Tabla_Cat_Cat_Construccion_Dominante;
                My_Sql += " WHERE " + Cat_Cat_Construccion_Dominante.Campo_Estatus + " = 'VIGENTE'";
                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Tramos = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Tabla_Const_Dominante: " + E.Message);
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
        public static DataTable Consultar_Motivos_Rechazo_Avaluo(Cls_Ope_Cat_Avaluo_Urbano_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT SE." + Ope_Cat_Seguimiento_Avaluo_Au.Campo_No_Seguimiento + " AS NO_SEGUIMIENTO";
                My_Sql += ", SE." + Ope_Cat_Seguimiento_Avaluo_Au.Campo_Motivo_Id + " AS MOTIVO_ID";
                My_Sql += ", SE." + Ope_Cat_Seguimiento_Avaluo_Au.Campo_Estatus + " AS ESTATUS";
                My_Sql += ", SE." + Ope_Cat_Seguimiento_Avaluo_Au.Campo_Usuario_Creo + " AS USUARIO_CREO";
                My_Sql += ", SE." + Ope_Cat_Seguimiento_Avaluo_Au.Campo_Fecha_Creo + " AS FECHA_CREO";
                My_Sql += ", MR." + Cat_Cat_Motivos_Rechazo.Campo_Motivo_Descripcion + " AS MOTIVO_DESCRIPCION";
                My_Sql += ", 'NADA' AS ACCION";
                My_Sql += " FROM " + Ope_Cat_Seguimiento_Avaluo_Au.Tabla_Ope_Cat_Seguimiento_Avaluo_Au + " SE";
                My_Sql += " LEFT OUTER JOIN " + Cat_Cat_Motivos_Rechazo.Tabla_Cat_Cat_Motivos_Rechazo + " MR";
                My_Sql += " ON SE." + Ope_Cat_Seguimiento_Avaluo_Au.Campo_Motivo_Id + " = MR." + Cat_Cat_Motivos_Rechazo.Campo_Motivo_Id;
                My_Sql += " WHERE SE." + Ope_Cat_Seguimiento_Avaluo_Au.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;
                My_Sql += " AND SE." + Ope_Cat_Seguimiento_Avaluo_Au.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' ";
                if (Avaluo.P_Estatus != null && Avaluo.P_Estatus.Trim() != "")
                {
                    My_Sql += "AND SE." + Ope_Cat_Seguimiento_Avaluo_Au.Campo_Estatus + " " + Avaluo.P_Estatus;
                }
                My_Sql += " ORDER BY SE." + Ope_Cat_Seguimiento_Avaluo_Au.Campo_Fecha_Creo + " DESC";
                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Tramos = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Motivos_Rechazo_Avaluo: Error al consultar los motivos de rechazo de Avalúo Urbano.");
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

    }
}