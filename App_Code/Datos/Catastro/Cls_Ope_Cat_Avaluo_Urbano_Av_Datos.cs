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
using Presidencia.Constantes;
using Presidencia.Operacion_Cat_Avaluo_Urbano_Av.Negocio;
using Presidencia.Sessiones;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Ope_Cat_Avaluo_Urbano_Av_Datos
/// </summary>
/// 
namespace Presidencia.Operacion_Cat_Avaluo_Urbano_Av.Datos
{
    public class Cls_Ope_Cat_Avaluo_Urbano_Av_Datos
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
        public static Boolean Alta_Avaluo_Urbano_Av(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo)
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
            String No_Avaluo = Obtener_ID_Consecutivo(Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av, Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo, Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + "=" + Avaluo.P_Anio_Avaluo, 10);
            String No_Colindancia = Obtener_ID_Consecutivo(Ope_Cat_Colindancias_Auv.Tabla_Ope_Cat_Colindancias_Auv, Ope_Cat_Colindancias_Auv.Campo_No_Colindancia, "", 10);
            String No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Documentos_Avaluo_Av.Tabla_Ope_Cat_Doc_Avaluo_Ur_Av, Ope_Cat_Documentos_Avaluo_Av.Campo_No_Documento, Ope_Cat_Documentos_Avaluo_Av.Campo_Anio_Documento + "=" + Avaluo.P_Anio_Avaluo, 10);
            try
            {

                Mi_sql = "INSERT INTO " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + "(";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Cuenta_Predial_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Motivo_Avaluo_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Solicitante + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Observaciones + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Ruta_Fachada_Inmueble + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Valor_Total_Predio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Inpr + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Inpa + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Valor_VR + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Region + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Manzana + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Lote + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Norte + ",";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Sur + ",";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Oriente + ",";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Poniente + ",";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Permitir_Revision + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Comentarios_Revisor + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_No_Renglones + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Tipo_Avaluo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Usuario_Creo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Creo;
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
                Mi_sql += Avaluo.P_Valor_Vr + ", '";
                Mi_sql += Avaluo.P_Perito_Interno_Id + "', '";
                Mi_sql += Avaluo.P_Estatus + "', '";
                Mi_sql += Avaluo.P_Region + "', '";
                Mi_sql += Avaluo.P_Manzana + "', '";
                Mi_sql += Avaluo.P_Lote + "', '";
                Mi_sql += Avaluo.P_Coord_Norte + "', '";
                Mi_sql += Avaluo.P_Coord_Sur + "', '";
                Mi_sql += Avaluo.P_Coord_Oriente + "', '";
                Mi_sql += Avaluo.P_Coord_Poniente + "', ";
                Mi_sql += "0, '";
                Mi_sql += Avaluo.P_No_Asignacion + "', '";
                Mi_sql += Avaluo.P_Permitir_Revision + "', '";
                Mi_sql += Avaluo.P_Comentarios_Revisor + "', ";
                Mi_sql += Avaluo.P_No_Renglones + ", '";
                Mi_sql += Avaluo.P_Tipo_Avaluo + "', '";
                Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += "SYSDATE";
                Mi_sql += ")";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Caracteristicas_Terreno_Av.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Caract_Terreno_Av.Tabla_Ope_Cat_Caract_Terreno_Av + "(";
                    Mi_sql += Ope_Cat_Caract_Terreno_Av.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Av.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Av.Campo_Vias_Acceso + ", ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Av.Campo_Fotografia + ", ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Av.Campo_Dens_Const + ", ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Av.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Av.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Clasificacion_Zona_Av.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Av.Tabla_Ope_Cat_Clasificacion_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Av.Tabla_Ope_Cat_Clasificacion_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Av.Tabla_Ope_Cat_Clasificacion_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Av.Tabla_Ope_Cat_Clasificacion_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Servicios_Zona_Av.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Av.Tabla_Ope_Cat_Servicios_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Av.Tabla_Ope_Cat_Servicios_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Av.Tabla_Ope_Cat_Servicios_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Av.Tabla_Ope_Cat_Servicios_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Construccion_Dominante_Av.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Av.Tabla_Ope_Cat_Const_Dominante_Av + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Construccion_Dominante_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Valor_Const_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Av.Tabla_Ope_Cat_Const_Dominante_Av + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Construccion_Dominante_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Valor_Const_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Av.Tabla_Ope_Cat_Const_Dominante_Av + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Construccion_Dominante_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Valor_Const_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Construccion_Av.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Construccion_Av.Tabla_Ope_Cat_Construccion_Av + "(";
                    Mi_sql += Ope_Cat_Construccion_Av.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Construccion_Av.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Construccion_Av.Campo_Tipo_Construccion + ", ";
                    Mi_sql += Ope_Cat_Construccion_Av.Campo_Calidad_Proyecto + ", ";
                    Mi_sql += Ope_Cat_Construccion_Av.Campo_Uso_Construccion + ", ";
                    Mi_sql += Ope_Cat_Construccion_Av.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Construccion_Av.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Elementos_Construccion_Av.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Elem_Construccion_Av.Tabla_Ope_Cat_Elem_Construccion_Av + "(";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elementos_Construccion_Id + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_A + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_B + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_C + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_D + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_E + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_F + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_G + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_H + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_I + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_J + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_K + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_L + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_M + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_N + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_O + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Construccion_Av.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Const_Av.Tabla_Ope_Cat_Calc_Valor_Const_Av + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Referencia + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Valor_Construccion_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Terreno_Av.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Terreno_Av.Tabla_Ope_Cat_Calc_Valor_Terreno_Av + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Seccion + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Valor_Tramo_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Factor_EF + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Fecha_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Orden;
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
                    if (Dr_Renglon["SECCION"].ToString().Trim()=="I")
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Documentos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Documentos_Avaluo_Av.Tabla_Ope_Cat_Doc_Avaluo_Ur_Av + "(";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_Anio_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Documentos_Avaluo_Av.Campo_Documento].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos_Avaluo_Av/" + Avaluo.P_Anio_Avaluo + "_" + No_Avaluo + "/" + Dr_Renglon[Ope_Cat_Documentos_Avaluo_Av.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Documento = (Convert.ToInt32(No_Documento) + 1).ToString("0000000000");
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Colindancias.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Colindancias_Auv.Tabla_Ope_Cat_Colindancias_Auv + "(";
                        Mi_sql += Ope_Cat_Colindancias_Auv.Campo_No_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Auv.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Auv.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Auv.Campo_Medida_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Auv.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Auv.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Colindancia + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += No_Avaluo + "', '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Colindancias_Auv.Campo_Medida_Colindancia].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Colindancia = (Convert.ToInt32(No_Colindancia) + 1).ToString("0000000000");
                    }
                }

                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Alta_Avaluo_Urbano: " + E.Message);
            }
            Trans.Commit();
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
        public static Boolean Modificar_Avaluo_Urbano_Av(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo)
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
            String No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Documentos_Avaluo_Av.Tabla_Ope_Cat_Doc_Avaluo_Ur_Av, Ope_Cat_Documentos_Avaluo_Av.Campo_No_Documento, Ope_Cat_Documentos_Avaluo_Av.Campo_Anio_Documento + "=" + Avaluo.P_Anio_Avaluo, 10);
            String No_Colindancia = Obtener_ID_Consecutivo(Ope_Cat_Colindancias_Auv.Tabla_Ope_Cat_Colindancias_Auv, Ope_Cat_Colindancias_Auv.Campo_No_Colindancia, "", 10);
            try
            {

                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " SET ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Motivo_Avaluo_Id + "= '" + Avaluo.P_Motivo_Avaluo_Id + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Solicitante + "= '" + Avaluo.P_Solicitante + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Observaciones + "= '" + Avaluo.P_Observaciones + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Ruta_Fachada_Inmueble + "= '" + Avaluo.P_Ruta_Fachada_Inmueble + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Valor_Total_Predio + "= " + Avaluo.P_Valor_Total_Predio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Inpr + "= " + Avaluo.P_Valor_Inpr + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Inpa + "= " + Avaluo.P_Valor_Inpa + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_No_Renglones + "= " + Avaluo.P_No_Renglones + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Valor_VR + "= " + Avaluo.P_Valor_Vr + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + "= '" + Avaluo.P_Perito_Interno_Id + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + "= '" + Avaluo.P_Estatus + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Region + "= '" + Avaluo.P_Region + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Manzana + "= '" + Avaluo.P_Manzana + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Lote + "= '" + Avaluo.P_Lote + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Tipo_Avaluo + "= '" + Avaluo.P_Tipo_Avaluo + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Norte + "= '" + Avaluo.P_Coord_Norte + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Sur + "= '" + Avaluo.P_Coord_Sur + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Oriente + "= '" + Avaluo.P_Coord_Oriente + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Poniente + "= '" + Avaluo.P_Coord_Poniente + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Permitir_Revision + "= '" + Avaluo.P_Permitir_Revision + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Comentarios_Revisor + "= '" + Avaluo.P_Comentarios_Revisor + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Usuario_Modifico + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Modifico + "= SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Caracteristicas_Terreno_Av.Rows)
                {
                    Mi_sql = "UPDATE " + Ope_Cat_Caract_Terreno_Av.Tabla_Ope_Cat_Caract_Terreno_Av + " SET ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Av.Campo_Vias_Acceso + "= '" + Dr_Renglon["VIAS_ACCESO"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Av.Campo_Fotografia + "= '" + Dr_Renglon["FOTOGRAFIA"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Av.Campo_Dens_Const + "= '" + Dr_Renglon["DENS_CONST"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Av.Campo_Usuario_Modifico + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Av.Campo_Fecha_Modifico + "= SYSDATE";
                    Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Construccion_Av.Rows)
                {
                    Mi_sql = "UPDATE " + Ope_Cat_Construccion_Av.Tabla_Ope_Cat_Construccion_Av + " SET ";
                    Mi_sql += Ope_Cat_Construccion_Av.Campo_Tipo_Construccion + "= '" + Dr_Renglon["TIPO_CONSTRUCCION"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Construccion_Av.Campo_Calidad_Proyecto + "= '" + Dr_Renglon["CALIDAD_PROYECTO"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Construccion_Av.Campo_Uso_Construccion + "= '" + Dr_Renglon["USO_CONSTRUCCION"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Construccion_Av.Campo_Usuario_Modifico + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += Ope_Cat_Construccion_Av.Campo_Fecha_Modifico + "= SYSDATE";
                    Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Elementos_Construccion_Av.Rows)
                {
                    Mi_sql = "UPDATE " + Ope_Cat_Elem_Construccion_Av.Tabla_Ope_Cat_Elem_Construccion_Av + " SET ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_A + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_A"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_B + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_B"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_C + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_C"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_D + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_D"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_E + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_E"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_F + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_F"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_G + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_G"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_H + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_H"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_I + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_I"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_J + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_J"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_K + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_K"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_L + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_L"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_M + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_M"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_N + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_N"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_O + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_O"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Usuario_Creo + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Fecha_Creo + "= SYSDATE";
                    Mi_sql += " WHERE " + Ope_Cat_Elem_Construccion_Av.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + " AND ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Av.Campo_Elementos_Construccion_Id + "= '" + Dr_Renglon["ELEMENTOS_CONSTRUCCION_ID"].ToString() + "'";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                Mi_sql = "DELETE " + Ope_Cat_Calc_Valor_Const_Av.Tabla_Ope_Cat_Calc_Valor_Const_Av + " WHERE " + Ope_Cat_Calc_Valor_Const_Av.Campo_No_Avaluo
                + " = '" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Calc_Valor_Const_Av.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Construccion_Av.Rows)
                {

                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Const_Av.Tabla_Ope_Cat_Calc_Valor_Const_Av + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Referencia + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Valor_Construccion_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Av.Campo_Fecha_Creo;
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

                Mi_sql = "DELETE " + Ope_Cat_Calc_Valor_Terreno_Av.Tabla_Ope_Cat_Calc_Valor_Terreno_Av + " WHERE " + Ope_Cat_Calc_Valor_Terreno_Av.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Terreno_Av.Rows)
                {

                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Terreno_Av.Tabla_Ope_Cat_Calc_Valor_Terreno_Av + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Seccion + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Valor_Tramo_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Factor_EF + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Fecha_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Av.Campo_Orden;
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

                if (Avaluo.P_Dt_Observaciones_Av != null && Avaluo.P_Dt_Observaciones_Av.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Observaciones_Av.Rows)
                    {
                        Mi_sql = "UPDATE " + Ope_Cat_Seguimiento_Avaluo_Av.Tabla_Ope_Cat_Seguimiento_Avaluo_Av;
                        Mi_sql += " SET " + Ope_Cat_Seguimiento_Avaluo_Av.Campo_Estatus + " = 'BAJA', ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Ope_Cat_Seguimiento_Avaluo_Av.Campo_No_Seguimiento + "='" + Dr_Renglon["NO_SEGUIMIENTO"].ToString() + "' AND ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Mi_sql = "DELETE " + Ope_Cat_Clasificacion_Zona_Av.Tabla_Ope_Cat_Clasificacion_Zona_Av + " WHERE " + Ope_Cat_Clasificacion_Zona_Av.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Clasificacion_Zona_Av.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Clasificacion_Zona_Av.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Av.Tabla_Ope_Cat_Clasificacion_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Av.Tabla_Ope_Cat_Clasificacion_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Av.Tabla_Ope_Cat_Clasificacion_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Av.Tabla_Ope_Cat_Clasificacion_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Av.Campo_Fecha_Creo;
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

                Mi_sql = "DELETE " + Ope_Cat_Servicio_Zona_Av.Tabla_Ope_Cat_Servicios_Zona_Av + " WHERE " + Ope_Cat_Servicio_Zona_Av.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Servicio_Zona_Av.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Servicios_Zona_Av.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Av.Tabla_Ope_Cat_Servicios_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Av.Tabla_Ope_Cat_Servicios_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Av.Tabla_Ope_Cat_Servicios_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Av.Tabla_Ope_Cat_Servicios_Zona_Av + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Av.Campo_Fecha_Creo;
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

                Mi_sql = "DELETE " + Ope_Cat_Const_Dominante_Av.Tabla_Ope_Cat_Const_Dominante_Av + " WHERE " + Ope_Cat_Const_Dominante_Av.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Const_Dominante_Av.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Construccion_Dominante_Av.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Av.Tabla_Ope_Cat_Const_Dominante_Av + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Construccion_Dominante_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Valor_Const_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Av.Tabla_Ope_Cat_Const_Dominante_Av + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Construccion_Dominante_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Valor_Const_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Av.Tabla_Ope_Cat_Const_Dominante_Av + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Construccion_Dominante_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Valor_Const_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Av.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Documentos.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Documentos_Avaluo_Av.Tabla_Ope_Cat_Doc_Avaluo_Ur_Av + "(";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_Anio_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Av.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Documentos_Avaluo_Av.Campo_Documento].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos_Avaluo_Av/" + Avaluo.P_Anio_Avaluo + "_" + Avaluo.P_No_Avaluo + "/" + Dr_Renglon[Ope_Cat_Documentos_Avaluo_Av.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Documento = (Convert.ToInt32(No_Documento) + 1).ToString("0000000000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["NO_DOCUMENTO"].ToString().Trim() != "")
                    {
                        Mi_sql = "DELETE " + Ope_Cat_Documentos_Avaluo_Av.Tabla_Ope_Cat_Doc_Avaluo_Ur_Av + " WHERE " + Ope_Cat_Documentos_Avaluo_Av.Campo_No_Documento;
                        Mi_sql += "='" + Dr_Renglon[Ope_Cat_Documentos_Avaluo_Av.Campo_No_Documento].ToString() + "' AND " + Ope_Cat_Documentos_Avaluo_Av.Campo_Anio_Documento + "=" + Avaluo.P_Anio_Avaluo;
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Colindancias.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Colindancias_Auv.Tabla_Ope_Cat_Colindancias_Auv + "(";
                        Mi_sql += Ope_Cat_Colindancias_Auv.Campo_No_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Auv.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Auv.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Auv.Campo_Medida_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Auv.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Auv.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Colindancia + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Avaluo.P_No_Avaluo + "', '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Colindancias_Auv.Campo_Medida_Colindancia].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Colindancia = (Convert.ToInt32(No_Colindancia) + 1).ToString("0000000000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon[Ope_Cat_Colindancias_Auv.Campo_No_Colindancia].ToString().Trim() != "")
                    {
                        Mi_sql = "DELETE " + Ope_Cat_Colindancias_Auv.Tabla_Ope_Cat_Colindancias_Auv + " WHERE " + Ope_Cat_Colindancias_Auv.Campo_No_Colindancia;
                        Mi_sql += "='" + Dr_Renglon[Ope_Cat_Colindancias_Auv.Campo_No_Colindancia].ToString() + "' AND " + Ope_Cat_Colindancias_Auv.Campo_Anio_Avaluo + "=" + Avaluo.P_Anio_Avaluo;
                        Mi_sql += " AND " + Ope_Cat_Colindancias_Auv.Campo_No_Avaluo + "= '" + Avaluo.P_No_Avaluo + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Avaluo_Urbano: " + E.Message);
            }
            Trans.Commit();
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
        public static Boolean Modificar_Observaciones_Avaluo_Av(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Aval)
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
            No_Seguimiento = Obtener_ID_Consecutivo(Ope_Cat_Seguimiento_Avaluo_Av.Tabla_Ope_Cat_Seguimiento_Avaluo_Av, Ope_Cat_Seguimiento_Avaluo_Av.Campo_No_Seguimiento, "", 10);
            try
            {

                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av;
                Mi_sql += " SET " + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + " = '" + Aval.P_Estatus + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Modifico + " = SYSDATE, ";
                if (Aval.P_Estatus == "AUTORIZADO")
                {
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Autorizo + " = SYSDATE, ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Observaciones_Rechazo + " = '', ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Permitir_Revision + " = 'SI' ";
                }
                else if (Aval.P_Estatus == "POR AUTORIZAR")
                {
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Observaciones_Rechazo + " = '"+Aval.P_Observaciones_Rechazo+"', ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Permitir_Revision + " = 'SI' ";
                }
                else
                {
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo + " = " + Aval.P_Veces_Rechazo + ", ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Rechazo + " = SYSDATE, ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Observaciones_Rechazo + " = '" + Aval.P_Observaciones_Rechazo + "', ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Permitir_Revision + " = 'NO' ";
                }
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + "=" + Aval.P_Anio_Avaluo;
                Mi_sql += " AND " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + "='" + Aval.P_No_Avaluo + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Aval.P_Dt_Observaciones_Av.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Seguimiento_Avaluo_Av.Tabla_Ope_Cat_Seguimiento_Avaluo_Av + "(";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_No_Seguimiento + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_Motivo_Id + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_Estatus + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_Fecha_Creo;
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
                        Mi_sql = "UPDATE " + Ope_Cat_Seguimiento_Avaluo_Av.Tabla_Ope_Cat_Seguimiento_Avaluo_Av;
                        Mi_sql += " SET " + Ope_Cat_Seguimiento_Avaluo_Av.Campo_Estatus + " = '" + Dr_Renglon["ESTATUS"].ToString() + "', ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Av.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Ope_Cat_Seguimiento_Avaluo_Av.Campo_No_Seguimiento + "='" + Dr_Renglon["NO_SEGUIMIENTO"].ToString() + "'";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Alta = true;
            }
            catch (Exception E)
            {
                Trans.Rollback();
                throw new Exception("Modificar_Observaciones_Avaluo_Au: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Firmante
        ///DESCRIPCIÓN: Consulta los motivos de Avaluo
        ///PARAMENTROS: selecciona de la tabla firmantes con su puesto respectivamente     
        ///            
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Firmante()
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Parametros.Campo_Firmante + " ||' - '||";
                Mi_SQL += " " + Cat_Cat_Parametros.Campo_Puesto + " AS FIRMANTE_1";
                Mi_SQL += ", " + Cat_Cat_Parametros.Campo_Firmante_2 + " ||' - '||";
                Mi_SQL += " " + Cat_Cat_Parametros.Campo_Puesto_2 + " AS FIRMANTE_2";
                Mi_SQL += " FROM " + Cat_Cat_Parametros.Tabla_Cat_Cat_Parametros;
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                // AGREGAR FILTRO Y ORDEN A LA CONSULTA.
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception e)
            {
                String Mensaje = "Error al intentar consultar el firmante de Avaluo. Error: [" + e.Message + "]."; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
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
        public static Boolean Asignar_Perito_Interno(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Aval)
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

                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av;
                Mi_sql += " SET " + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " = '" + Aval.P_Perito_Interno_Id + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + "=" + Aval.P_Anio_Avaluo;
                Mi_sql += " AND " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + "='" + Aval.P_No_Avaluo + "'";
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
        public static DataTable Consultar_Avaluo_Urbano_Av(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo)
        {
            DataTable Dt_Avaluo = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo
                    + ", TO_NUMBER(" + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + ")||'/'||TO_NUMBER(" + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + ") AS AVALUO"
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Motivo_Avaluo_Id
                    + ", (SELECT " + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion + " FROM " + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + " WHERE "
                    + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + "." + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id + "= "
                    + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + "." + Ope_Cat_Avaluo_Urbano_Av.Campo_Motivo_Avaluo_Id + ") AS MOTIVO_AVALUO"
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Cuenta_Predial_Id
                    + ", (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "= "
                    + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + "." + Ope_Cat_Avaluo_Urbano.Campo_Cuenta_Predial_Id + ") AS CUENTA_PREDIAL"
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Solicitante
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Observaciones
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Ruta_Fachada_Inmueble
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Valor_Total_Predio
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Inpr
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Inpa
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Valor_VR
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Region
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Manzana
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Lote
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Renglones
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Norte
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Sur
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Oriente
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Poniente
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Autorizo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Rechazo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Externo_Id
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Veces_Rechazo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Observaciones_Rechazo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Permitir_Revision
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Comentarios_Revisor
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Asignacion
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Tipo_Avaluo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Usuario_Modifico
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Modifico
                    + " FROM  " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av
                    + " WHERE ";
                if (Avaluo.P_No_Avaluo != null && Avaluo.P_No_Avaluo.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND ";
                }
                if (Avaluo.P_Anio_Avaluo != null && Avaluo.P_Anio_Avaluo.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo + " AND ";
                }
                if (Avaluo.P_Perito_Externo_Id != null && Avaluo.P_Perito_Externo_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Externo_Id + " = '" + Avaluo.P_Perito_Externo_Id + "' AND ";
                }
                if (Avaluo.P_Coord_Norte != null && Avaluo.P_Coord_Norte.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Norte + " = '" + Avaluo.P_Coord_Norte + "' AND ";
                }
                if (Avaluo.P_Permitir_Revision != null && Avaluo.P_Permitir_Revision.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_Permitir_Revision + " = '" + Avaluo.P_Permitir_Revision + "' AND ";
                }
                if (Avaluo.P_Coord_Oriente != null && Avaluo.P_Coord_Oriente.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Oriente + " = '" + Avaluo.P_Coord_Oriente + "' AND ";
                }
                if (Avaluo.P_Coord_Poniente != null && Avaluo.P_Coord_Poniente.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Externo_Id + " = '" + Avaluo.P_Perito_Externo_Id + "' AND ";
                }
                if (Avaluo.P_Coord_Sur != null && Avaluo.P_Coord_Sur.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_Coord_Sur + " = '" + Avaluo.P_Coord_Sur + "' AND ";
                }
                if (Avaluo.P_Perito_Interno_Id != null && Avaluo.P_Perito_Interno_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " = '" + Avaluo.P_Perito_Interno_Id + "' AND ";
                }
                if (Avaluo.P_Folio != null && Avaluo.P_Folio.Trim() != "")
                {
                    Mi_SQL += "TO_NUMBER(" + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + ")||'/'||TO_NUMBER(" + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + ") = '" + Avaluo.P_Folio + "' AND ";
                }
                if (Avaluo.P_Cuenta_Predial_Id != null && Avaluo.P_Cuenta_Predial_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_Cuenta_Predial_Id + " = '" + Avaluo.P_Cuenta_Predial_Id + "' AND ";
                }
                if (Avaluo.P_Estatus != null && Avaluo.P_Estatus.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + " " + Avaluo.P_Estatus + " AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                //Ordena la consulta por el id de la tabla de manera que el mas reciente dato se coloca al inicio de la lista

                Mi_SQL += " ORDER BY " + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + " DESC, " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + " DESC";

                // agregar filtro y orden a la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Dt_Avaluo = dataset.Tables[0];
                }
                if ((Avaluo.P_No_Avaluo != null && Avaluo.P_No_Avaluo.Trim() != "") && (Avaluo.P_Anio_Avaluo != null && Avaluo.P_Anio_Avaluo.Trim() != ""))
                {

                    Mi_SQL = "SELECT " + Ope_Cat_Caract_Terreno_Av.Campo_Vias_Acceso
                    + ", " + Ope_Cat_Caract_Terreno_Av.Campo_Fotografia
                    + ", " + Ope_Cat_Caract_Terreno_Av.Campo_Dens_Const
                    + ", " + Ope_Cat_Caract_Terreno_Av.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Caract_Terreno_Av.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Caract_Terreno_Av.Tabla_Ope_Cat_Caract_Terreno_Av
                    + " WHERE "
                    + Ope_Cat_Caract_Terreno_Av.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Caract_Terreno_Av.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Caracteristicas_Terreno_Av = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Construccion_Av.Campo_Tipo_Construccion
                    + ", " + Ope_Cat_Construccion_Av.Campo_Calidad_Proyecto
                    + ", " + Ope_Cat_Construccion_Av.Campo_Uso_Construccion
                    + ", " + Ope_Cat_Construccion_Av.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Construccion_Av.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Construccion_Av.Tabla_Ope_Cat_Construccion_Av
                    + " WHERE "
                    + Ope_Cat_Construccion_Av.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Construccion_Av.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Construccion_Av = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Elem_Construccion_Av.Campo_Elementos_Construccion_Id
                    + ", (SELECT EC." + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion + " FROM " + Cat_Cat_Elementos_Construccion.Tabla_Cat_Cat_Elementos_Construccion + " EC WHERE EC." + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion_Id + "=" + Ope_Cat_Elem_Construccion_Av.Tabla_Ope_Cat_Elem_Construccion_Av + "." + Ope_Cat_Elem_Construccion_Av.Campo_Elementos_Construccion_Id + ") AS ELEMENTOS_CONSTRUCCION"
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_A
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_B
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_C
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_D
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_E
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_F
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_G
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_H
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_I
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_J
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_K
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_L
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_M
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_N
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_O
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Elem_Construccion_Av.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Elem_Construccion_Av.Tabla_Ope_Cat_Elem_Construccion_Av
                    + " WHERE "
                    + Ope_Cat_Elem_Construccion_Av.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Elem_Construccion_Av.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Elementos_Construccion_Av = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Referencia
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Superficie_M2
                    + ", NVL(VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Valor_Construccion_Id + ", '') AS " + Ope_Cat_Calc_Valor_Const_Av.Campo_Valor_Construccion_Id
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Factor
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Valor_Parcial
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Referencia
                    + ", NVL(TV." + Cat_Cat_Tab_Val_Construccion.Campo_Clave_Valor + ",0) AS CON_SERV"
                    + ", NVL(CC." + Cat_Cat_Calidad_Construccion.Campo_Clave_Calidad + ",0) AS TIPO"
                    + ", NVL(TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_M2 + ",0.00) AS VALOR_M2"
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Usuario_Creo
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Calc_Valor_Const_Av.Tabla_Ope_Cat_Calc_Valor_Const_Av + " VC"
                    + " LEFT OUTER JOIN " + Cat_Cat_Tab_Val_Construccion.Tabla_Cat_Cat_Tab_Val_Construccion + " TV"
                    + " ON VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Valor_Construccion_Id + "=TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_Construccion_Id
                    + " LEFT OUTER JOIN " + Cat_Cat_Calidad_Construccion.Tabla_Cat_Cat_Calidad_Construccion + " CC"
                    + " ON TV." + Cat_Cat_Tab_Val_Construccion.Campo_Calidad_Id + "=CC." + Cat_Cat_Calidad_Construccion.Campo_Calidad_Id
                    + " WHERE "
                    + Ope_Cat_Calc_Valor_Const_Av.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Calc_Valor_Const_Av.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo
                    + " ORDER BY VC." + Ope_Cat_Calc_Valor_Const_Av.Campo_Referencia + " ASC";

                    Avaluo.P_Dt_Calculo_Valor_Construccion_Av = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Seccion
                        + ", VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Seccion
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Superficie_M2
                    + ", NVL(VT." + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo + ",0) AS " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo
                    + ", NVL(VT." + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id + ",'') AS " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Factor
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Factor_EF
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Valor_Parcial
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Usuario_Creo
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Calc_Valor_Terreno_Av.Tabla_Ope_Cat_Calc_Valor_Terreno_Av + " VC"
                    + " LEFT OUTER JOIN " + Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos + " VT"
                    + " ON VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Valor_Tramo_Id + "=VT." + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id
                    + " WHERE "
                    + Ope_Cat_Calc_Valor_Terreno_Av.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo
                    + " ORDER BY VC." + Ope_Cat_Calc_Valor_Terreno_Av.Campo_Orden + " ASC";

                    Avaluo.P_Dt_Calculo_Valor_Terreno_Av = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    //--





                    Mi_SQL = "SELECT OCZ." + Ope_Cat_Clasificacion_Zona_Av.Campo_Clasificacion_Zona_Id
                    + ", OCZ." + Ope_Cat_Clasificacion_Zona_Av.Campo_Valor_Clasificacion_Zona
                    + ", NVL(CZ." + Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona + ",'') AS " + Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona
                    + ", OCZ." + Ope_Cat_Clasificacion_Zona_Av.Campo_Usuario_Creo
                    + ", OCZ." + Ope_Cat_Clasificacion_Zona_Av.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Clasificacion_Zona_Av.Tabla_Ope_Cat_Clasificacion_Zona_Av + " OCZ"
                    + " LEFT OUTER JOIN " + Cat_Cat_Clasificacion_Zona.Tabla_Cat_Cat_Clasificacion_Zona + " CZ"
                    + " ON OCZ." + Ope_Cat_Clasificacion_Zona_Av.Campo_Clasificacion_Zona_Id + "=CZ." + Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona_Id
                    + " WHERE "
                    + Ope_Cat_Clasificacion_Zona_Av.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Clasificacion_Zona_Av.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Clasificacion_Zona_Av = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT OSZ." + Ope_Cat_Servicio_Zona_Av.Campo_Servicios_Zona_Id
                    + ", OSZ." + Ope_Cat_Servicio_Zona_Av.Campo_Valor_Servicio_Zona
                    + ", NVL(SZ." + Cat_Cat_Servicios_Zona.Campo_Servicio_Zona + ",'') AS " + Cat_Cat_Servicios_Zona.Campo_Servicio_Zona
                    + ", OSZ." + Ope_Cat_Servicio_Zona_Av.Campo_Usuario_Creo
                    + ", OSZ." + Ope_Cat_Servicio_Zona_Av.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Servicio_Zona_Av.Tabla_Ope_Cat_Servicios_Zona_Av + " OSZ"
                    + " LEFT OUTER JOIN " + Cat_Cat_Servicios_Zona.Tabla_Cat_Cat_Servicios_Zona + " SZ"
                    + " ON OSZ." + Ope_Cat_Servicio_Zona_Av.Campo_Servicios_Zona_Id + "=SZ." + Cat_Cat_Servicios_Zona.Campo_Servicio_Zona_Id
                    + " WHERE "
                    + Ope_Cat_Servicio_Zona_Av.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Servicio_Zona_Av.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Servicios_Zona_Av = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT OSZ." + Ope_Cat_Const_Dominante_Av.Campo_Construccion_Dominante_Id
                    + ", OSZ." + Ope_Cat_Const_Dominante_Av.Campo_Valor_Const_Dominante
                    + ", NVL(SZ." + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante + ",'') AS " + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante
                    + ", OSZ." + Ope_Cat_Const_Dominante_Av.Campo_Usuario_Creo
                    + ", OSZ." + Ope_Cat_Const_Dominante_Av.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Const_Dominante_Av.Tabla_Ope_Cat_Const_Dominante_Av + " OSZ"
                    + " LEFT OUTER JOIN " + Cat_Cat_Construccion_Dominante.Tabla_Cat_Cat_Construccion_Dominante + " SZ"
                    + " ON OSZ." + Ope_Cat_Const_Dominante_Av.Campo_Construccion_Dominante_Id + "=SZ." + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante_Id
                    + " WHERE "
                    + Ope_Cat_Const_Dominante_Av.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Const_Dominante_Av.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Construccion_Dominante_Av = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Documentos_Avaluo_Av.Campo_No_Documento
                    + ", " + Ope_Cat_Documentos_Avaluo_Av.Campo_Anio_Documento
                    + ", " + Ope_Cat_Documentos_Avaluo_Av.Campo_Documento
                    + ", " + Ope_Cat_Documentos_Avaluo_Av.Campo_Ruta_Documento
                    + ", " + Ope_Cat_Documentos_Avaluo_Av.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Documentos_Avaluo_Av.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Documentos_Avaluo_Av.Campo_Fecha_Modifico
                    + ", " + Ope_Cat_Documentos_Avaluo_Av.Campo_Usuario_Modifico
                    + ", 'NADA' AS ACCION"
                    + " FROM  " + Ope_Cat_Documentos_Avaluo_Av.Tabla_Ope_Cat_Doc_Avaluo_Ur_Av
                    + " WHERE "
                    + Ope_Cat_Documentos_Avaluo_Av.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Documentos_Avaluo_Av.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Documentos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Colindancias_Auv.Campo_No_Colindancia
                    + ", " + Ope_Cat_Colindancias_Auv.Campo_Anio_Avaluo
                    + ", " + Ope_Cat_Colindancias_Auv.Campo_No_Avaluo
                    + ", " + Ope_Cat_Colindancias_Auv.Campo_Medida_Colindancia
                    + ", " + Ope_Cat_Colindancias_Auv.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Colindancias_Auv.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Colindancias_Auv.Campo_Fecha_Modifico
                    + ", " + Ope_Cat_Colindancias_Auv.Campo_Usuario_Modifico
                    + ", 'NADA' AS ACCION"
                    + " FROM  " + Ope_Cat_Colindancias_Auv.Tabla_Ope_Cat_Colindancias_Auv
                    + " WHERE "
                    + Ope_Cat_Colindancias_Auv.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Colindancias_Auv.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Colindancias = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        public static DataTable Consultar_Avaluo_Urbano_Asignados(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo)
        {
            DataTable Dt_Avaluo = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + "||'/'||TO_NUMBER(" + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + ") AS AVALUO"
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Motivo_Avaluo_Id
                    + ", (SELECT " + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion + " FROM " + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + " WHERE "
                    + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + "." + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id + "= "
                    + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + "." + Ope_Cat_Avaluo_Urbano_Av.Campo_Motivo_Avaluo_Id + ") AS MOTIVO_AVALUO"
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Cuenta_Predial_Id
                    + ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" + Cat_Empleados.Campo_Apellido_Materno + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE "
                    + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " IN (SELECT "
                    + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + "." + Cat_Cat_Peritos_Internos.Campo_Empleado_Id + " FROM " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + " WHERE " + Cat_Cat_Peritos_Internos.Tabla_Cat_Cat_Peritos_Internos + "." + Cat_Cat_Peritos_Internos.Campo_Perito_Interno_Id + " IN (SELECT " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + "." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " FROM " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + " WHERE " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + "." + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " IS NOT NULL AND " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av + "." + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + " IN ('POR VALIDAR','RECHAZADO')))) AS PERITO_INTERNO"
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Solicitante
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Observaciones
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Ruta_Fachada_Inmueble
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Valor_Total_Predio
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Inpr
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Inpa
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Valor_VR
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Autorizo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Externo_Id
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Usuario_Modifico
                    + ", " + Ope_Cat_Avaluo_Urbano_Av.Campo_Fecha_Modifico
                    + " FROM  " + Ope_Cat_Avaluo_Urbano_Av.Tabla_Ope_Cat_Avaluo_Urbano_Av
                    + " WHERE "
                    + Ope_Cat_Avaluo_Urbano_Av.Campo_Perito_Interno_Id + " IS NOT NULL AND ";
                if (Avaluo.P_Folio != null && Avaluo.P_Folio.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_Anio_Avaluo + "||'/'||TO_NUMBER(" + Ope_Cat_Avaluo_Urbano_Av.Campo_No_Avaluo + ") LIKE '" + Avaluo.P_Folio + "' AND ";
                }
                Mi_SQL += Ope_Cat_Avaluo_Urbano_Av.Campo_Estatus + " IN ('POR VALIDAR','RECHAZADO')";
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
        public static DataTable Consultar_Tabla_Valores_Construccion_Av(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo)
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Elementos_Construccion
        ///DESCRIPCIÓN: Obtiene la tabla inicial para los elementos de construcción del avalúo urbano
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Elementos_Construccion_Av(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT " + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion_Id;
                My_Sql += ", " + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_A;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_B;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_C;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_D;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_E;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_F;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_G;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_H;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_I;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_J;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_K;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_L;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_M;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_N;
                My_Sql += ", '' AS " + Ope_Cat_Elem_Construccion_Av.Campo_Elemento_Construccion_O;
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Clasificacion_Zona
        ///DESCRIPCIÓN: Obtiene la tabla inicial para las clasificaciones de la zona del avalúo urbano
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Clasificacion_Zona_Av(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo)
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
        public static DataTable Consultar_Tabla_Servicios_Zona_Av(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo)
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
        public static DataTable Consultar_Tabla_Const_Dominante_Av(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo)
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
        public static DataTable Consultar_Motivos_Rechazo_Avaluo_Av(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT SE." + Ope_Cat_Seguimiento_Avaluo_Av.Campo_No_Seguimiento + " AS NO_SEGUIMIENTO";
                My_Sql += ", SE." + Ope_Cat_Seguimiento_Avaluo_Av.Campo_Motivo_Id + " AS MOTIVO_ID";
                My_Sql += ", SE." + Ope_Cat_Seguimiento_Avaluo_Av.Campo_Estatus + " AS ESTATUS";
                My_Sql += ", MR." + Cat_Cat_Motivos_Rechazo.Campo_Motivo_Descripcion + " AS MOTIVO_DESCRIPCION";
                My_Sql += ", 'NADA' AS ACCION";
                My_Sql += ", SE." + Ope_Cat_Seguimiento_Avaluo_Av.Campo_Fecha_Creo + " AS FECHA_CREO";
                My_Sql += ", SE." + Ope_Cat_Seguimiento_Avaluo_Av.Campo_Usuario_Creo + " AS USUARIO_CREO";
                My_Sql += " FROM " + Ope_Cat_Seguimiento_Avaluo_Av.Tabla_Ope_Cat_Seguimiento_Avaluo_Av + " SE";
                My_Sql += " LEFT OUTER JOIN " + Cat_Cat_Motivos_Rechazo.Tabla_Cat_Cat_Motivos_Rechazo + " MR";
                My_Sql += " ON SE." + Ope_Cat_Seguimiento_Avaluo_Av.Campo_Motivo_Id + " = MR." + Cat_Cat_Motivos_Rechazo.Campo_Motivo_Id;
                My_Sql += " WHERE SE." + Ope_Cat_Seguimiento_Avaluo_Av.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;
                My_Sql += " AND SE." + Ope_Cat_Seguimiento_Avaluo_Av.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' ";
                if (Avaluo.P_Estatus != null && Avaluo.P_Estatus.Trim() != "")
                {
                    My_Sql += "AND SE." + Ope_Cat_Seguimiento_Avaluo_Av.Campo_Estatus + " " + Avaluo.P_Estatus;
                }
                My_Sql += " ORDER BY SE." + Ope_Cat_Seguimiento_Avaluo_Av.Campo_Estatus + " DESC";
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Firmante
        ///DESCRIPCIÓN: Consulta los motivos de Avaluo
        ///PARAMENTROS: selecciona de la tabla firmantes con su puesto respectivamente     
        ///            
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Firmante()
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL = "SELECT " + Cat_Cat_Parametros.Campo_Firmante + " ||' - '||";
                Mi_SQL += " " + Cat_Cat_Parametros.Campo_Puesto + " AS FIRMANTE_1";
                Mi_SQL += ", " + Cat_Cat_Parametros.Campo_Firmante_2 + " ||' - '||";
                Mi_SQL += " " + Cat_Cat_Parametros.Campo_Puesto_2 + " AS FIRMANTE_2";

                Mi_SQL += "," + Cat_Cat_Parametros.Campo_Fundamentacion_Legal + " AS LEYENDA" ;
                Mi_SQL += " FROM " + Cat_Cat_Parametros.Tabla_Cat_Cat_Parametros;
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                // AGREGAR FILTRO Y ORDEN A LA CONSULTA.
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception e)
            {
                String Mensaje = "Error al intentar consultar el firmante de Avaluo. Error: [" + e.Message + "]."; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tasas_Anuales
        ///DESCRIPCIÓN: Realizar la consulta y llenar el grid con estos datos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 21/Ago/2011 12:14:35 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Tasas_Anuales(Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL                        

            try
            {
                Mi_SQL = "";
                Mi_SQL += "SELECT ";
                Mi_SQL += " TASA." +
                Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " AS TASA_ID, " + "TASA." +
                Cat_Pre_Tasas_Predial.Campo_Identificador + " AS IDENTIFICADOR, " + "TASA." +
                Cat_Pre_Tasas_Predial.Campo_Descripcion + " AS DESCRIPCION, ";
                Mi_SQL += " ANUAL." +
                Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " AS TASA_ANUAL_ID, " + "ANUAL." +
                Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID + " AS TASA_PREDIAL_ID, " + "ANUAL." +
                Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " AS TASA_ANUAL, " + "ANUAL." +
                Cat_Pre_Tasas_Predial_Anual.Campo_Año + " AS ANIO ";
                Mi_SQL += " FROM " + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + " TASA LEFT OUTER JOIN " +
                    Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " ANUAL ON ANUAL." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID +
                    " = TASA." + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID;
                Mi_SQL += " WHERE ";
                    if (Datos.P_Anio_Tasa != "" && Datos.P_Anio_Tasa != null)
                    {
                        Mi_SQL += Cat_Pre_Tasas_Predial_Anual.Campo_Año + " = '" + Datos.P_Anio_Tasa + "' AND ";

                    }
                    if (Datos.P_Identificador_Tasa != "" && Datos.P_Identificador_Tasa != null)
                    {
                        Mi_SQL += Cat_Pre_Tasas_Predial.Campo_Identificador + " = '" + Datos.P_Identificador_Tasa + "' AND ";
                    }
                    Mi_SQL += "TASA." + Cat_Pre_Tasas_Predial.Campo_Estatus + " NOT IN ('BAJA') ";
                    Mi_SQL += " ORDER BY ANUAL." + Cat_Pre_Tasas_Predial_Anual.Campo_Año + " DESC";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Mi_SQL + Ex.Message);
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
    }
}
