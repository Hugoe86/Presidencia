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
using Presidencia.Operacion_Cat_Avaluo_Urbano_Reg.Negocio;
using Presidencia.Sessiones;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;


/// <summary>
/// Summary description for Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos
/// </summary>
/// 
namespace Presidencia.Operacion_Cat_Avaluo_Urbano_Reg.Datos
{
    public class Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos
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
        public static Boolean Alta_Avaluo_Urbano_Re(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Avaluo)
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
            String No_Avaluo = Obtener_ID_Consecutivo(Ope_Cat_Avaluo_Urbano_Re.Tabla_Ope_Cat_Avaluo_Urbano_Re, Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo, Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo + "=" + Avaluo.P_Anio_Avaluo, 10);
            String No_Colindancia = Obtener_ID_Consecutivo(Ope_Cat_Colindancias_Aur.Tabla_Ope_Cat_Colindancias_Aur, Ope_Cat_Colindancias_Aur.Campo_No_Colindancia, "", 10);
            String No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Documentos_Avaluo_Re.Tabla_Ope_Cat_Doc_Avaluo_Ur_Re, Ope_Cat_Documentos_Avaluo_Re.Campo_No_Documento, Ope_Cat_Documentos_Avaluo_Re.Campo_Anio_Documento + "=" + Avaluo.P_Anio_Avaluo, 10);
            try
            {
                Mi_sql = "INSERT INTO " + Ope_Cat_Avaluo_Urbano_Re.Tabla_Ope_Cat_Avaluo_Urbano_Re + "(";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Cuenta_Predial_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Motivo_Avaluo_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Solicitante + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Observaciones + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Ruta_Fachada_Inmueble + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Valor_Total_Predio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Inpr + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Inpa + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Valor_VR + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Fecha_Autorizo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Perito_Interno_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_No_Oficio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Region + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Manzana + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Lote + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Estatus + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Solicitud_Id + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Usuario_Creo + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Fecha_Creo;
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
                Mi_sql += Avaluo.P_Fecha_Autorizo.ToString("d-M-yyyy") + "', '";
                Mi_sql += Avaluo.P_Perito_Interno_Id + "', ";
                if (Avaluo.P_No_Oficio.Trim() != "")
                {
                    Mi_sql += "'" + Avaluo.P_No_Oficio + "', '";
                }
                else
                {
                    Mi_sql += "NULL, '";
                }
                Mi_sql += Avaluo.P_Region + "', '";
                Mi_sql += Avaluo.P_Manzana + "', '";
                Mi_sql += Avaluo.P_Lote + "', '";
                Mi_sql += Avaluo.P_Estatus + "', '";
                Mi_sql += Avaluo.P_Solicitud_Id + "', '";
                Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += "SYSDATE";
                Mi_sql += ")";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Caracteristicas_Terreno_Re.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Caract_Terreno_Re.Tabla_Ope_Cat_Caract_Terreno_Re + "(";
                    Mi_sql += Ope_Cat_Caract_Terreno_Re.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Re.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Re.Campo_Vias_Acceso + ", ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Re.Campo_Fotografia + ", ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Re.Campo_Dens_Const + ", ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Re.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Re.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Clasificacion_Zona_Re.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Re.Tabla_Ope_Cat_Clasificacion_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Re.Tabla_Ope_Cat_Clasificacion_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Re.Tabla_Ope_Cat_Clasificacion_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Re.Tabla_Ope_Cat_Clasificacion_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Servicios_Zona_Re.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Re.Tabla_Ope_Cat_Servicio_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Re.Tabla_Ope_Cat_Servicio_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Re.Tabla_Ope_Cat_Servicio_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Re.Tabla_Ope_Cat_Servicio_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Construccion_Dominante_Re.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Re.Tabla_Ope_Cat_Const_Dominante_Re + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Construccion_Dominante_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Valor_Const_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Re.Tabla_Ope_Cat_Const_Dominante_Re + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Construccion_Dominante_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Valor_Const_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Re.Tabla_Ope_Cat_Const_Dominante_Re + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Construccion_Dominante_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Valor_Const_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Construccion_Re.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Construccion_Re.Tabla_Ope_Cat_Construccion_Re + "(";
                    Mi_sql += Ope_Cat_Construccion_Re.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Construccion_Re.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Construccion_Re.Campo_Tipo_Construccion + ", ";
                    Mi_sql += Ope_Cat_Construccion_Re.Campo_Calidad_Proyecto + ", ";
                    Mi_sql += Ope_Cat_Construccion_Re.Campo_Uso_Construccion + ", ";
                    Mi_sql += Ope_Cat_Construccion_Re.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Construccion_Re.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Elementos_Construccion_Re.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Elem_Construccion_Re.Tabla_Ope_Cat_Elem_Construccion_Re + "(";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elementos_Construccion_Id + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_A + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_B + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_C + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_D + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_E + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_F + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_G + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_H + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_I + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_J + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_K + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_L + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_M + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_N + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_O + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Construccion_Re.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Const_Re.Tabla_Ope_Cat_Calc_Valor_Const_Re + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Referencia + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Valor_Construccion_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Terreno_Re.Rows)
                {
                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Terreno_Re.Tabla_Ope_Cat_Calc_Valor_Terreno_Re + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Seccion + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Valor_Tramo_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Factor_EF + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Fecha_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Orden;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Archivos_Re.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Documentos_Avaluo_Re.Tabla_Ope_Cat_Doc_Avaluo_Ur_Re + "(";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_Anio_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Documentos_Avaluo_Re.Campo_Documento].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos_Avaluo_Re/" + Avaluo.P_Anio_Avaluo + "_" + No_Avaluo + "/" + Dr_Renglon[Ope_Cat_Documentos_Avaluo_Re.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Documento = (Convert.ToInt32(No_Documento) + 1).ToString("0000000000");
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Medidas_Re.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Colindancias_Aur.Tabla_Ope_Cat_Colindancias_Aur + "(";
                        Mi_sql += Ope_Cat_Colindancias_Aur.Campo_No_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aur.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aur.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aur.Campo_Medida_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aur.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aur.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Colindancia + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += No_Avaluo + "', '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Colindancias_Aur.Campo_Medida_Colindancia].ToString() + "', '";
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
                    // establecer parámetros para actualizar solicitud
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
                throw new Exception("Alta_Avaluo_Urbano: " + E.Message);
            }
            Avaluo.P_No_Avaluo = No_Avaluo;
            Trans.Commit();
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
        public static Boolean Modificar_Avaluo_Urbano_Re(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Avaluo)
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
            String No_Documento = Obtener_ID_Consecutivo(Ope_Cat_Documentos_Avaluo_Re.Tabla_Ope_Cat_Doc_Avaluo_Ur_Re, Ope_Cat_Documentos_Avaluo_Re.Campo_No_Documento, Ope_Cat_Documentos_Avaluo_Re.Campo_Anio_Documento + "=" + Avaluo.P_Anio_Avaluo, 10);
            String No_Colindancia = Obtener_ID_Consecutivo(Ope_Cat_Colindancias_Aur.Tabla_Ope_Cat_Colindancias_Aur, Ope_Cat_Colindancias_Aur.Campo_No_Colindancia, "", 10);
            try
            {

                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Urbano_Re.Tabla_Ope_Cat_Avaluo_Urbano_Re + " SET ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Motivo_Avaluo_Id + "= '" + Avaluo.P_Motivo_Avaluo_Id + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Solicitante + "= '" + Avaluo.P_Solicitante + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Observaciones + "= '" + Avaluo.P_Observaciones + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Ruta_Fachada_Inmueble + "= '" + Avaluo.P_Ruta_Fachada_Inmueble + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Valor_Total_Predio + "= " + Avaluo.P_Valor_Total_Predio + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Inpr + "= " + Avaluo.P_Valor_Inpr + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Inpa + "= " + Avaluo.P_Valor_Inpa + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Valor_VR + "= " + Avaluo.P_Valor_Vr + ", ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Fecha_Autorizo + "= '" + Avaluo.P_Fecha_Autorizo.ToString("d-M-yyyy") + "', ";                
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_No_Oficio + "= '" + Avaluo.P_No_Oficio + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Region + "= '" + Avaluo.P_Region + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Manzana + "= '" + Avaluo.P_Manzana + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Lote + "= '" + Avaluo.P_Lote + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Estatus + "= '" + Avaluo.P_Estatus + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Usuario_Modifico + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Fecha_Modifico + "= SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Caracteristicas_Terreno_Re.Rows)
                {
                    Mi_sql = "UPDATE " + Ope_Cat_Caract_Terreno_Re.Tabla_Ope_Cat_Caract_Terreno_Re + " SET ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Re.Campo_Vias_Acceso + "= '" + Dr_Renglon["VIAS_ACCESO"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Re.Campo_Fotografia + "= '" + Dr_Renglon["FOTOGRAFIA"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Re.Campo_Dens_Const + "= '" + Dr_Renglon["DENS_CONST"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Re.Campo_Usuario_Modifico + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += Ope_Cat_Caract_Terreno_Re.Campo_Fecha_Modifico + "= SYSDATE";
                    Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Construccion_Re.Rows)
                {
                    Mi_sql = "UPDATE " + Ope_Cat_Construccion_Re.Tabla_Ope_Cat_Construccion_Re + " SET ";
                    Mi_sql += Ope_Cat_Construccion_Re.Campo_Tipo_Construccion + "= '" + Dr_Renglon["TIPO_CONSTRUCCION"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Construccion_Re.Campo_Calidad_Proyecto + "= '" + Dr_Renglon["CALIDAD_PROYECTO"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Construccion_Re.Campo_Uso_Construccion + "= '" + Dr_Renglon["USO_CONSTRUCCION"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Construccion_Re.Campo_Usuario_Modifico + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += Ope_Cat_Construccion_Re.Campo_Fecha_Modifico + "= SYSDATE";
                    Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                    Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Elementos_Construccion_Re.Rows)
                {
                    Mi_sql = "UPDATE " + Ope_Cat_Elem_Construccion_Re.Tabla_Ope_Cat_Elem_Construccion_Re + " SET ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_A + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_A"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_B + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_B"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_C + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_C"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_D + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_D"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_E + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_E"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_F + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_F"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_G + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_G"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_H + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_H"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_I + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_I"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_J + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_J"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_K + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_K"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_L + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_L"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_M + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_M"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_N + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_N"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_O + "= '" + Dr_Renglon["ELEMENTO_CONSTRUCCION_O"].ToString() + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Usuario_Creo + "= '" + Cls_Sessiones.Nombre_Empleado + "', ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Fecha_Creo + "= SYSDATE";
                    Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + " AND ";
                    Mi_sql += Ope_Cat_Elem_Construccion_Re.Campo_Elementos_Construccion_Id + "= '" + Dr_Renglon["ELEMENTOS_CONSTRUCCION_ID"].ToString() + "'";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                }

                Mi_sql = "DELETE " + Ope_Cat_Calc_Valor_Const_Re.Tabla_Ope_Cat_Calc_Valor_Const_Re + " WHERE " + Ope_Cat_Calc_Valor_Const_Re.Campo_No_Avaluo
                + " = '" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Calc_Valor_Const_Re.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Construccion_Re.Rows)
                {

                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Const_Re.Tabla_Ope_Cat_Calc_Valor_Const_Re + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Referencia + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Valor_Construccion_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Const_Re.Campo_Fecha_Creo;
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

                Mi_sql = "DELETE " + Ope_Cat_Calc_Valor_Terreno_Re.Tabla_Ope_Cat_Calc_Valor_Terreno_Re + " WHERE " + Ope_Cat_Calc_Valor_Terreno_Re.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Calc_Valor_Terreno_Re.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Calculo_Valor_Terreno_Re.Rows)
                {

                    Mi_sql = "INSERT INTO " + Ope_Cat_Calc_Valor_Terreno_Re.Tabla_Ope_Cat_Calc_Valor_Terreno_Re + "(";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_No_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Anio_Avaluo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Seccion + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Superficie_M2 + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Valor_Tramo_Id + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Factor + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Factor_EF + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Valor_Parcial + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Usuario_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Fecha_Creo + ", ";
                    Mi_sql += Ope_Cat_Calc_Valor_Terreno_Re.Campo_Orden;
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

                if (Avaluo.P_Dt_Observaciones_Re != null && Avaluo.P_Dt_Observaciones_Re.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Observaciones_Re.Rows)
                    {
                        Mi_sql = "UPDATE " + Ope_Cat_Seguimiento_Avaluo_Re.Tabla_Ope_Cat_Seguimiento_Avaluo_Re;
                        Mi_sql += " SET " + Ope_Cat_Seguimiento_Avaluo_Re.Campo_Estatus + " = 'BAJA', ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Ope_Cat_Seguimiento_Avaluo_Re.Campo_No_Seguimiento + "='" + Dr_Renglon["NO_SEGUIMIENTO"].ToString() + "' AND ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo;
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Mi_sql = "DELETE " + Ope_Cat_Clasificacion_Zona_Re.Tabla_Ope_Cat_Clasificacion_Zona_Re + " WHERE " + Ope_Cat_Clasificacion_Zona_Re.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Clasificacion_Zona_Re.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Clasificacion_Zona_Re.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Re.Tabla_Ope_Cat_Clasificacion_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Re.Tabla_Ope_Cat_Clasificacion_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Re.Tabla_Ope_Cat_Clasificacion_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Clasificacion_Zona_Re.Tabla_Ope_Cat_Clasificacion_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Clasificacion_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Valor_Clasificacion_Zona + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Clasificacion_Zona_Re.Campo_Fecha_Creo;
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

                Mi_sql = "DELETE " + Ope_Cat_Servicio_Zona_Re.Tabla_Ope_Cat_Servicio_Zona_Re + " WHERE " + Ope_Cat_Servicio_Zona_Re.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Servicio_Zona_Re.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Servicios_Zona_Re.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Re.Tabla_Ope_Cat_Servicio_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Re.Tabla_Ope_Cat_Servicio_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Re.Tabla_Ope_Cat_Servicio_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Servicio_Zona_Re.Tabla_Ope_Cat_Servicio_Zona_Re + "(";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Servicios_Zona_Id + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Valor_Servicio_Zona + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Servicio_Zona_Re.Campo_Fecha_Creo;
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

                Mi_sql = "DELETE " + Ope_Cat_Const_Dominante_Re.Tabla_Ope_Cat_Const_Dominante_Re + " WHERE " + Ope_Cat_Const_Dominante_Re.Campo_No_Avaluo + " ='" + Avaluo.P_No_Avaluo + "' AND " + Ope_Cat_Const_Dominante_Re.Campo_Anio_Avaluo + "= " + Avaluo.P_Anio_Avaluo + "";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Construccion_Dominante_Re.Rows)
                {
                    if (Dr_Renglon["COLUMNA_A_ID"].ToString().Trim() != "")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Re.Tabla_Ope_Cat_Const_Dominante_Re + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Construccion_Dominante_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Valor_Const_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Re.Tabla_Ope_Cat_Const_Dominante_Re + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Construccion_Dominante_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Valor_Const_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "INSERT INTO " + Ope_Cat_Const_Dominante_Re.Tabla_Ope_Cat_Const_Dominante_Re + "(";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Construccion_Dominante_Id + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Valor_Const_Dominante + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Const_Dominante_Re.Campo_Fecha_Creo;
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

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Archivos_Re.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Documentos_Avaluo_Re.Tabla_Ope_Cat_Doc_Avaluo_Ur_Re + "(";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_No_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_Anio_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_Ruta_Documento + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Documentos_Avaluo_Re.Campo_Fecha_Creo;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Documento + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Avaluo.P_No_Avaluo + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Documentos_Avaluo_Re.Campo_Documento].ToString() + "', '";
                        Mi_sql += "../Catastro/Archivos_Avaluo_Re/" + Avaluo.P_Anio_Avaluo + "_" + Avaluo.P_No_Avaluo + "/" + Dr_Renglon[Ope_Cat_Documentos_Avaluo_Re.Campo_Ruta_Documento].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Documento = (Convert.ToInt32(No_Documento) + 1).ToString("0000000000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon["NO_DOCUMENTO"].ToString().Trim() != "")
                    {
                        Mi_sql = "DELETE " + Ope_Cat_Documentos_Avaluo_Re.Tabla_Ope_Cat_Doc_Avaluo_Ur_Re + " WHERE " + Ope_Cat_Documentos_Avaluo_Re.Campo_No_Documento;
                        Mi_sql += "='" + Dr_Renglon[Ope_Cat_Documentos_Avaluo_Re.Campo_No_Documento].ToString() + "' AND " + Ope_Cat_Documentos_Avaluo_Re.Campo_Anio_Documento + "=" + Avaluo.P_Anio_Avaluo;
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (DataRow Dr_Renglon in Avaluo.P_Dt_Medidas_Re.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Colindancias_Aur.Tabla_Ope_Cat_Colindancias_Aur + "(";
                        Mi_sql += Ope_Cat_Colindancias_Aur.Campo_No_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aur.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aur.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aur.Campo_Medida_Colindancia + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aur.Campo_Usuario_Modifico + ", ";
                        Mi_sql += Ope_Cat_Colindancias_Aur.Campo_Fecha_Modifico;
                        Mi_sql += ") VALUES ('";
                        Mi_sql += No_Colindancia + "', ";
                        Mi_sql += Avaluo.P_Anio_Avaluo + ", '";
                        Mi_sql += Avaluo.P_No_Avaluo + "', '";
                        Mi_sql += Dr_Renglon[Ope_Cat_Colindancias_Aur.Campo_Medida_Colindancia].ToString() + "', '";
                        Mi_sql += Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += "SYSDATE";
                        Mi_sql += ")";
                        Cmd.CommandText = Mi_sql;
                        Cmd.ExecuteNonQuery();
                        No_Colindancia = (Convert.ToInt32(No_Colindancia) + 1).ToString("0000000000");
                    }
                    else if (Dr_Renglon["ACCION"].ToString() == "BAJA" && Dr_Renglon[Ope_Cat_Colindancias_Aur.Campo_No_Colindancia].ToString().Trim() != "")
                    {
                        Mi_sql = "DELETE " + Ope_Cat_Colindancias_Aur.Tabla_Ope_Cat_Colindancias_Aur + " WHERE " + Ope_Cat_Colindancias_Aur.Campo_No_Colindancia;
                        Mi_sql += "='" + Dr_Renglon[Ope_Cat_Colindancias_Aur.Campo_No_Colindancia].ToString() + "' AND " + Ope_Cat_Colindancias_Aur.Campo_Anio_Avaluo + "=" + Avaluo.P_Anio_Avaluo;
                        Mi_sql += " AND " + Ope_Cat_Colindancias_Aur.Campo_No_Avaluo + "= '" + Avaluo.P_No_Avaluo + "'";
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Observaciones_Avaluo_Re
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
        public static Boolean Modificar_Observaciones_Avaluo_Re(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Aval)
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
            No_Seguimiento = Obtener_ID_Consecutivo(Ope_Cat_Seguimiento_Avaluo_Re.Tabla_Ope_Cat_Seguimiento_Avaluo_Re, Ope_Cat_Seguimiento_Avaluo_Re.Campo_No_Seguimiento, "", 10);
            try
            {
                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Urbano_Re.Tabla_Ope_Cat_Avaluo_Urbano_Re;
                Mi_sql += " SET " + Ope_Cat_Avaluo_Urbano_Re.Campo_Estatus + " = '" + Aval.P_Estatus + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo + "=" + Aval.P_Anio_Avaluo;
                Mi_sql += " AND " + Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo + "='" + Aval.P_No_Avaluo + "'";
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                foreach (DataRow Dr_Renglon in Aval.P_Dt_Observaciones_Re.Rows)
                {
                    if (Dr_Renglon["ACCION"].ToString() == "ALTA")
                    {
                        Mi_sql = "INSERT INTO " + Ope_Cat_Seguimiento_Avaluo_Re.Tabla_Ope_Cat_Seguimiento_Avaluo_Re + "(";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_No_Seguimiento + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_No_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_Anio_Avaluo + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_Motivo_Id + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_Estatus + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_Usuario_Creo + ", ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_Fecha_Creo;
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
                        Mi_sql = "UPDATE " + Ope_Cat_Seguimiento_Avaluo_Re.Tabla_Ope_Cat_Seguimiento_Avaluo_Re;
                        Mi_sql += " SET " + Ope_Cat_Seguimiento_Avaluo_Re.Campo_Estatus + " = '" + Dr_Renglon["ESTATUS"].ToString() + "', ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_Usuario_Modifico + " = '" + Cls_Sessiones.Nombre_Empleado + "', ";
                        Mi_sql += Ope_Cat_Seguimiento_Avaluo_Re.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_sql += " WHERE " + Ope_Cat_Seguimiento_Avaluo_Re.Campo_No_Seguimiento + "='" + Dr_Renglon["NO_SEGUIMIENTO"].ToString() + "'";
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
                        Neg_Actualizar_Solicitud.P_Solicitud_ID = Aval.P_Solicitud_Id;
                        Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Consultar_Datos_Solicitud(); // Se obtienen los Datos a Detalle de la Solicitud Seleccionada

                        if (Neg_Actualizar_Solicitud.P_Tipo_Actividad == "CONDICION")
                        {
                            Neg_Actualizar_Solicitud.P_Respuesta_Condicion = Aval.P_Generar_Cobro; // SI ó NO 
                            if (Aval.P_Generar_Cobro == "SI")
                                Neg_Actualizar_Solicitud.P_Condicion_Si = Convert.ToDouble(Neg_Actualizar_Solicitud.P_Condicion_Si);//NUMERO DE LA ACTIVIDAD
                            else
                                Neg_Actualizar_Solicitud.P_Condicion_No = Convert.ToDouble(Neg_Actualizar_Solicitud.P_Condicion_No);//NUMERO DE LA ACTIVIDAD
                        }
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
                throw new Exception("Modificar_Observaciones_Avaluo_Au: " + E.Message);
            }
            Trans.Commit();
            return Alta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Avaluo_Urbano_Re
        ///DESCRIPCIÓN: Obtiene la tabla con los datos de avalúos urbanos
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 23/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Avaluo_Urbano_Re(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Avaluo)
        {
            DataTable Dt_Avaluo = new DataTable();
            String Mi_SQL = "";

            try
            {
                Mi_SQL = "SELECT " + Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo + "||'/'||TO_NUMBER(" + Ope_Cat_Avaluo_Urbano.Campo_No_Avaluo + ") AS AVALUO"
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Motivo_Avaluo_Id
                    + ", (SELECT " + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Descripcion + " FROM " + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + " WHERE "
                    + Cat_Cat_Motivos_Avaluo.Tabla_Cat_Cat_Motivos_Avaluo + "." + Cat_Cat_Motivos_Avaluo.Campo_Motivo_Avaluo_Id + "= "
                    + Ope_Cat_Avaluo_Urbano_Re.Tabla_Ope_Cat_Avaluo_Urbano_Re + "." + Ope_Cat_Avaluo_Urbano.Campo_Motivo_Avaluo_Id + ") AS MOTIVO_AVALUO"
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Cuenta_Predial_Id
                    + ", (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "= "
                    + Ope_Cat_Avaluo_Urbano_Re.Tabla_Ope_Cat_Avaluo_Urbano_Re + "." + Ope_Cat_Avaluo_Urbano.Campo_Cuenta_Predial_Id + ") AS CUENTA_PREDIAL"
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Solicitante
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Observaciones
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Ruta_Fachada_Inmueble
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Valor_Total_Predio
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Inpr
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Inpa
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Valor_VR
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Fecha_Autorizo
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Perito_Externo_Id
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Perito_Interno_Id
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Estatus
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Usuario_Modifico
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_No_Oficio
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Region
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Manzana
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Lote
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Solicitud_Id
                    + ", (SELECT " + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " WHERE "
                    + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID + "= "
                    + Ope_Cat_Avaluo_Urbano_Re.Tabla_Ope_Cat_Avaluo_Urbano_Re + "." + Ope_Cat_Avaluo_Urbano.Campo_Solicitud_Id + ") AS CLAVE_SOLICITUD"
                    + ", " + Ope_Cat_Avaluo_Urbano_Re.Campo_Fecha_Modifico
                    + " FROM  " + Ope_Cat_Avaluo_Urbano_Re.Tabla_Ope_Cat_Avaluo_Urbano_Re
                    + " WHERE ";
                if (Avaluo.P_No_Avaluo != null && Avaluo.P_No_Avaluo.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND ";
                }
                if (Avaluo.P_Solicitud_Id != null && Avaluo.P_Solicitud_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Re.Campo_Solicitud_Id + " = '" + Avaluo.P_Solicitud_Id + "' AND ";
                }
                if (Avaluo.P_Anio_Avaluo != null && Avaluo.P_Anio_Avaluo.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo + " AND ";
                }
                if (Avaluo.P_Perito_Externo_Id != null && Avaluo.P_Perito_Externo_Id.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Re.Campo_Perito_Externo_Id + " = '" + Avaluo.P_Perito_Externo_Id + "' AND ";
                }
                if (Avaluo.P_Folio != null && Avaluo.P_Folio.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo + "||'/'||TO_NUMBER(" + Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo + ") = '" + Avaluo.P_Folio + "' AND ";
                }
                if (Avaluo.P_Estatus != null && Avaluo.P_Estatus.Trim() != "")
                {
                    Mi_SQL += Ope_Cat_Avaluo_Urbano_Re.Campo_Estatus + " " + Avaluo.P_Estatus + " AND ";
                }
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Cat_Avaluo_Urbano_Re.Campo_Fecha_Creo + " DESC ";
                // agregar filtro y orden a la consulta
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Dt_Avaluo = dataset.Tables[0];
                }
                if ((Avaluo.P_No_Avaluo != null && Avaluo.P_No_Avaluo.Trim() != "") && (Avaluo.P_Anio_Avaluo != null && Avaluo.P_Anio_Avaluo.Trim() != ""))
                {

                    Mi_SQL = "SELECT " + Ope_Cat_Caract_Terreno_Re.Campo_Vias_Acceso
                    + ", " + Ope_Cat_Caract_Terreno_Re.Campo_Fotografia
                    + ", " + Ope_Cat_Caract_Terreno_Re.Campo_Dens_Const
                    + ", " + Ope_Cat_Caract_Terreno_Re.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Caract_Terreno_Re.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Caract_Terreno_Re.Tabla_Ope_Cat_Caract_Terreno_Re
                    + " WHERE "
                    + Ope_Cat_Caract_Terreno_Re.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Caract_Terreno_Re.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Caracteristicas_Terreno_Re = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Construccion_Re.Campo_Tipo_Construccion
                    + ", " + Ope_Cat_Construccion_Re.Campo_Calidad_Proyecto
                    + ", " + Ope_Cat_Construccion_Re.Campo_Uso_Construccion
                    + ", " + Ope_Cat_Construccion_Re.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Construccion_Re.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Construccion_Re.Tabla_Ope_Cat_Construccion_Re
                    + " WHERE "
                    + Ope_Cat_Construccion_Re.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Construccion_Re.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Construccion_Re = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Elem_Construccion_Re.Campo_Elementos_Construccion_Id
                    + ", (SELECT EC." + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion + " FROM " + Cat_Cat_Elementos_Construccion.Tabla_Cat_Cat_Elementos_Construccion + " EC WHERE EC." + Cat_Cat_Elementos_Construccion.Campo_Elemento_Construccion_Id + "=" + Ope_Cat_Elem_Construccion_Re.Tabla_Ope_Cat_Elem_Construccion_Re + "." + Ope_Cat_Elem_Construccion_Re.Campo_Elementos_Construccion_Id + ") AS ELEMENTOS_CONSTRUCCION"
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_A
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_B
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_C
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_D
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_E
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_F
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_G
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_H
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_I
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_J
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_K
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_L
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_M
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_N
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Elemento_Construccion_O
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Elem_Construccion_Re.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Elem_Construccion_Re.Tabla_Ope_Cat_Elem_Construccion_Re
                    + " WHERE "
                    + Ope_Cat_Elem_Construccion_Re.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Elem_Construccion_Re.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Elementos_Construccion_Re = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT VC." + Ope_Cat_Calc_Valor_Const_Re.Campo_Referencia
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Re.Campo_Superficie_M2
                    + ", NVL(VC." + Ope_Cat_Calc_Valor_Const_Re.Campo_Valor_Construccion_Id + ", '') AS " + Ope_Cat_Calc_Valor_Const_Au.Campo_Valor_Construccion_Id
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Re.Campo_Factor
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Re.Campo_Valor_Parcial
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Re.Campo_Referencia
                    + ", NVL(TV." + Cat_Cat_Tab_Val_Construccion.Campo_Clave_Valor + ",0) AS CON_SERV"
                    + ", NVL(CC." + Cat_Cat_Calidad_Construccion.Campo_Clave_Calidad + ",0) AS TIPO"
                    + ", NVL(TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_M2 + ",0.00) AS VALOR_M2"
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Re.Campo_Usuario_Creo
                    + ", VC." + Ope_Cat_Calc_Valor_Const_Re.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Calc_Valor_Const_Re.Tabla_Ope_Cat_Calc_Valor_Const_Re + " VC"
                    + " LEFT OUTER JOIN " + Cat_Cat_Tab_Val_Construccion.Tabla_Cat_Cat_Tab_Val_Construccion + " TV"
                    + " ON VC." + Ope_Cat_Calc_Valor_Const_Re.Campo_Valor_Construccion_Id + "=TV." + Cat_Cat_Tab_Val_Construccion.Campo_Valor_Construccion_Id
                    + " LEFT OUTER JOIN " + Cat_Cat_Calidad_Construccion.Tabla_Cat_Cat_Calidad_Construccion + " CC"
                    + " ON TV." + Cat_Cat_Tab_Val_Construccion.Campo_Calidad_Id + "=CC." + Cat_Cat_Calidad_Construccion.Campo_Calidad_Id
                    + " WHERE "
                    + Ope_Cat_Elem_Construccion_Re.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Elem_Construccion_Re.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo
                    + " ORDER BY VC." + Ope_Cat_Calc_Valor_Const_Re.Campo_Referencia + " ASC";

                    Avaluo.P_Dt_Calculo_Valor_Construccion_Re = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT VC." + Ope_Cat_Calc_Valor_Terreno_Re.Campo_Seccion
                        + ", VC." + Ope_Cat_Calc_Valor_Terreno_Re.Campo_Seccion
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Re.Campo_Superficie_M2
                    + ", NVL(VT." + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo + ",0) AS " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo
                    + ", NVL(VT." + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id + ",'') AS " + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Re.Campo_Factor
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Re.Campo_Factor_EF
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Re.Campo_Valor_Parcial
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Re.Campo_Usuario_Creo
                    + ", VC." + Ope_Cat_Calc_Valor_Terreno_Re.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Calc_Valor_Terreno_Re.Tabla_Ope_Cat_Calc_Valor_Terreno_Re + " VC"
                    + " LEFT OUTER JOIN " + Cat_Cat_Tabla_Valores_Tramos.Tabla_Cat_Cat_Tabla_Valores_Tramos + " VT"
                    + " ON VC." + Ope_Cat_Calc_Valor_Terreno_Re.Campo_Valor_Tramo_Id + "=VT." + Cat_Cat_Tabla_Valores_Tramos.Campo_Valor_Tramo_Id
                    + " WHERE "
                    + Ope_Cat_Elem_Construccion_Re.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Elem_Construccion_Re.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo
                    + " ORDER BY VC." + Ope_Cat_Calc_Valor_Terreno_Re.Campo_Orden + " ASC";

                    Avaluo.P_Dt_Calculo_Valor_Terreno_Re = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT OCZ." + Ope_Cat_Clasificacion_Zona_Re.Campo_Clasificacion_Zona_Id
                    + ", OCZ." + Ope_Cat_Clasificacion_Zona_Re.Campo_Valor_Clasificacion_Zona
                    + ", NVL(CZ." + Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona + ",'') AS " + Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona
                    + ", OCZ." + Ope_Cat_Clasificacion_Zona_Re.Campo_Usuario_Creo
                    + ", OCZ." + Ope_Cat_Clasificacion_Zona_Re.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Clasificacion_Zona_Re.Tabla_Ope_Cat_Clasificacion_Zona_Re + " OCZ"
                    + " LEFT OUTER JOIN " + Cat_Cat_Clasificacion_Zona.Tabla_Cat_Cat_Clasificacion_Zona + " CZ"
                    + " ON OCZ." + Ope_Cat_Clasificacion_Zona_Re.Campo_Clasificacion_Zona_Id + "=CZ." + Cat_Cat_Clasificacion_Zona.Campo_Clasificacion_Zona_Id
                    + " WHERE "
                    + Ope_Cat_Clasificacion_Zona_Re.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Clasificacion_Zona_Re.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Clasificacion_Zona_Re = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT OSZ." + Ope_Cat_Servicio_Zona_Re.Campo_Servicios_Zona_Id
                    + ", OSZ." + Ope_Cat_Servicio_Zona_Re.Campo_Valor_Servicio_Zona
                    + ", NVL(SZ." + Cat_Cat_Servicios_Zona.Campo_Servicio_Zona + ",'') AS " + Cat_Cat_Servicios_Zona.Campo_Servicio_Zona
                    + ", OSZ." + Ope_Cat_Servicio_Zona_Re.Campo_Usuario_Creo
                    + ", OSZ." + Ope_Cat_Servicio_Zona_Re.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Servicio_Zona_Re.Tabla_Ope_Cat_Servicio_Zona_Re + " OSZ"
                    + " LEFT OUTER JOIN " + Cat_Cat_Servicios_Zona.Tabla_Cat_Cat_Servicios_Zona + " SZ"
                    + " ON OSZ." + Ope_Cat_Servicio_Zona_Re.Campo_Servicios_Zona_Id + "=SZ." + Cat_Cat_Servicios_Zona.Campo_Servicio_Zona_Id
                    + " WHERE "
                    + Ope_Cat_Servicio_Zona_Re.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Servicio_Zona_Re.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Servicios_Zona_Re = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT OSZ." + Ope_Cat_Const_Dominante_Re.Campo_Construccion_Dominante_Id
                    + ", OSZ." + Ope_Cat_Const_Dominante_Re.Campo_Valor_Const_Dominante
                    + ", NVL(SZ." + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante + ",'') AS " + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante
                    + ", OSZ." + Ope_Cat_Const_Dominante_Re.Campo_Usuario_Creo
                    + ", OSZ." + Ope_Cat_Const_Dominante_Re.Campo_Fecha_Creo
                    + " FROM  " + Ope_Cat_Const_Dominante_Re.Tabla_Ope_Cat_Const_Dominante_Re + " OSZ"
                    + " LEFT OUTER JOIN " + Cat_Cat_Construccion_Dominante.Tabla_Cat_Cat_Construccion_Dominante + " SZ"
                    + " ON OSZ." + Ope_Cat_Const_Dominante_Re.Campo_Construccion_Dominante_Id + "=SZ." + Cat_Cat_Construccion_Dominante.Campo_Construccion_Dominante_Id
                    + " WHERE "
                    + Ope_Cat_Const_Dominante_Re.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Const_Dominante_Re.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Construccion_Dominante_Re = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Documentos_Avaluo_Re.Campo_No_Documento
                    + ", " + Ope_Cat_Documentos_Avaluo_Re.Campo_Anio_Documento
                    + ", " + Ope_Cat_Documentos_Avaluo_Re.Campo_Documento
                    + ", " + Ope_Cat_Documentos_Avaluo_Re.Campo_Ruta_Documento
                    + ", " + Ope_Cat_Documentos_Avaluo_Re.Campo_Fecha_Creo
                    + ", " + Ope_Cat_Documentos_Avaluo_Re.Campo_Usuario_Creo
                    + ", " + Ope_Cat_Documentos_Avaluo_Re.Campo_Fecha_Modifico
                    + ", " + Ope_Cat_Documentos_Avaluo_Re.Campo_Usuario_Modifico
                    + ", 'NADA' AS ACCION"
                    + " FROM  " + Ope_Cat_Documentos_Avaluo_Re.Tabla_Ope_Cat_Doc_Avaluo_Ur_Re
                    + " WHERE "
                    + Ope_Cat_Documentos_Avaluo_Re.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                    + Ope_Cat_Documentos_Avaluo_Re.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Archivos_Re = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "SELECT " + Ope_Cat_Colindancias_Aur.Campo_No_Colindancia
                   + ", " + Ope_Cat_Colindancias_Aur.Campo_Anio_Avaluo
                   + ", " + Ope_Cat_Colindancias_Aur.Campo_No_Avaluo
                   + ", " + Ope_Cat_Colindancias_Aur.Campo_Medida_Colindancia
                   + ", " + Ope_Cat_Colindancias_Aur.Campo_Fecha_Creo
                   + ", " + Ope_Cat_Colindancias_Aur.Campo_Usuario_Creo
                   + ", " + Ope_Cat_Colindancias_Aur.Campo_Fecha_Modifico
                   + ", " + Ope_Cat_Colindancias_Aur.Campo_Usuario_Modifico
                   + ", 'NADA' AS ACCION"
                   + " FROM  " + Ope_Cat_Colindancias_Aur.Tabla_Ope_Cat_Colindancias_Aur
                   + " WHERE "
                   + Ope_Cat_Colindancias_Aur.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' AND "
                   + Ope_Cat_Colindancias_Aur.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;

                    Avaluo.P_Dt_Medidas_Re = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tipos_Construccion
        ///DESCRIPCIÓN: Obtiene todos los tipos de construcción que estan dados de alta en la Base de Datos
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Valores_Construccion_Re(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Avaluo)
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
        public static DataTable Consultar_Tabla_Elementos_Construccion_Re(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Avaluo)
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
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Clasificacion_Zona
        ///DESCRIPCIÓN: Obtiene la tabla inicial para las clasificaciones de la zona del avalúo urbano
        ///PARAMETROS:
        ///CREO: Miguel Angel Bedolla Moreno
        ///FECHA_CREO: 08/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Clasificacion_Zona_Re(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Avaluo)
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
        public static DataTable Consultar_Tabla_Servicios_Zona_Re(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Avaluo)
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
        public static DataTable Consultar_Tabla_Const_Dominante_Re(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Avaluo)
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
        public static DataTable Consultar_Motivos_Rechazo_Avaluo_Re(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Avaluo)
        {
            DataTable Dt_Tramos = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT SE." + Ope_Cat_Seguimiento_Avaluo_Re.Campo_No_Seguimiento + " AS NO_SEGUIMIENTO";
                My_Sql += ", SE." + Ope_Cat_Seguimiento_Avaluo_Re.Campo_Motivo_Id + " AS MOTIVO_ID";
                My_Sql += ", SE." + Ope_Cat_Seguimiento_Avaluo_Re.Campo_Estatus + " AS ESTATUS";
                My_Sql += ", MR." + Cat_Cat_Motivos_Rechazo.Campo_Motivo_Descripcion + " AS MOTIVO_DESCRIPCION";
                My_Sql += ", 'NADA' AS ACCION";
                My_Sql += " FROM " + Ope_Cat_Seguimiento_Avaluo_Re.Tabla_Ope_Cat_Seguimiento_Avaluo_Re + " SE";
                My_Sql += " LEFT OUTER JOIN " + Cat_Cat_Motivos_Rechazo.Tabla_Cat_Cat_Motivos_Rechazo + " MR";
                My_Sql += " ON SE." + Ope_Cat_Seguimiento_Avaluo_Re.Campo_Motivo_Id + " = MR." + Cat_Cat_Motivos_Rechazo.Campo_Motivo_Id;
                My_Sql += " WHERE SE." + Ope_Cat_Seguimiento_Avaluo_Re.Campo_Anio_Avaluo + " = " + Avaluo.P_Anio_Avaluo;
                My_Sql += " AND SE." + Ope_Cat_Seguimiento_Avaluo_Re.Campo_No_Avaluo + " = '" + Avaluo.P_No_Avaluo + "' ";
                if (Avaluo.P_Estatus != null && Avaluo.P_Estatus.Trim() != "")
                {
                    My_Sql += "AND SE." + Ope_Cat_Seguimiento_Avaluo_Re.Campo_Estatus + " " + Avaluo.P_Estatus;
                }
                My_Sql += " ORDER BY SE." + Ope_Cat_Seguimiento_Avaluo_Re.Campo_Estatus + " DESC";
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
        public static DataTable Consultar_Solicitud_Tramite(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Tramite)
        {
            DataTable Dt_Tramites = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT TS." + Ope_Tra_Solicitud.Campo_Solicitud_ID
                 //+ ", TS." + Ope_Tra_Solicitud.Campo_Apellido_Materno
                 //+ ", TS." + Ope_Tra_Solicitud.Campo_Apellido_Paterno
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

                 + ", TS." + Ope_Tra_Solicitud.Campo_Apellido_Paterno 
                 + "||' '|| TS." + Ope_Tra_Solicitud.Campo_Apellido_Materno 
                 + "||' '||TS." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " AS NOMBRE_COMPLETO"
                 + ", TDMA." + Ope_Tra_Datos.Campo_Valor + " AS MOTIVO_AVALUO"
                 
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Estatus_Avaluo_Urbano_Regularizacion
        ///DESCRIPCIÓN: Modificara el estatus del avaluo cuando sea autorizado
        ///PARAMETROS:     
        ///           
        ///CREO: Alejandro Leyva Alvarado
        ///FECHA_CREO: 29/Agosto/2012 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Estatus_Avaluo_Urbano_Regularizacion(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Estatus)
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

                Mi_sql = "UPDATE " + Ope_Cat_Avaluo_Urbano_Re.Tabla_Ope_Cat_Avaluo_Urbano_Re + " SET ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Estatus + "= '" + Estatus.P_Estatus + "', ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Fecha_Autorizo + "= SYSDATE";
                Mi_sql += " WHERE " + Ope_Cat_Avaluo_Urbano_Re.Campo_No_Avaluo + " ='" + Estatus.P_No_Avaluo + "' AND ";
                Mi_sql += Ope_Cat_Avaluo_Urbano_Re.Campo_Anio_Avaluo + "= " + Estatus.P_Anio_Avaluo;
                Cmd.CommandText = Mi_sql;
                Cmd.ExecuteNonQuery();

                if (Estatus.P_Estatus == "AUTORIZADO")
                {
                    Mi_sql = "UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud;
                    Mi_sql += " SET " + Ope_Tra_Solicitud.Campo_Costo_Base + " = " + Estatus.P_Importe_Avaluo + ", ";
                    Mi_sql += Ope_Tra_Solicitud.Campo_Cantidad + " = 1, ";
                    Mi_sql += Ope_Tra_Solicitud.Campo_Costo_Total + " = " + Estatus.P_Importe_Avaluo;
                    Mi_sql += " WHERE TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ") = '" + Estatus.P_Solicitud_Id + "'";
                    Cmd.CommandText = Mi_sql;
                    Cmd.ExecuteNonQuery();
                    Cls_Ope_Bandeja_Tramites_Negocio Neg_Actualizar_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                    System.Text.StringBuilder My_SQL = new System.Text.StringBuilder();
                    My_SQL.Append("SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                        + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                        + "," + Ope_Tra_Solicitud.Campo_Estatus
                        + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                        + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ")) = UPPER(TRIM('" + Estatus.P_Solicitud_Id + "'))");
                    Cmd.CommandText = My_SQL.ToString();
                    Cmd.CommandType = CommandType.Text;
                    OracleDataReader Dtr_Datos_Solicitud = Cmd.ExecuteReader();

                    // si hay datos para leer, agregar pasivo
                    if (Dtr_Datos_Solicitud.Read())
                    {
                        Neg_Actualizar_Solicitud.P_Comando_Oracle = Cmd;
                        Neg_Actualizar_Solicitud.P_Solicitud_ID = Estatus.P_Solicitud_Id;
                        Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Consultar_Datos_Solicitud(); // Se obtienen los Datos a Detalle de la Solicitud Seleccionada
                        if (Neg_Actualizar_Solicitud.P_Tipo_Actividad == "CONDICION")
                        {

                            Neg_Actualizar_Solicitud.P_Respuesta_Condicion = Estatus.P_Generar_Cobro; // SI ó NO 
                            if (Estatus.P_Generar_Cobro == "SI")
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Solicitudes_Regularizaciones
        ///DESCRIPCIÓN: Obtiene las solicitudes de regularizaciones
        ///PARAMETROS:
        ///CREO: David Herrera Rincon
        ///FECHA_CREO: 23/Octubre/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Solicitudes_Regularizaciones(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Avaluo)
        {
            DataTable Dt_Regularizaciones = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT (S." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " ||' '|| S." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " ||''|| S." + Ope_Tra_Solicitud.Campo_Apellido_Materno + ") AS Nombre,";
                My_Sql += " S." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " AS Fecha_Ingreso, Ro." + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Recepcion + ", Ro." + Ope_Cat_Recepcion_Oficios.Campo_Fecha_Recepcion + ",";
                My_Sql += " Ro." + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio_Respuesta + ", Ro." + Ope_Cat_Recepcion_Oficios.Campo_Fecha_Respuesta + ", UPPER(D." + Ope_Tra_Datos.Campo_Valor + ") AS Motivo_Regularizacion";
                My_Sql += " FROM " + Ope_Cat_Recepcion_Oficios.Tabla_Ope_Cat_Recepcion_Oficios + " Ro LEFT OUTER JOIN " + Ope_Cat_Avaluo_Urbano_Re.Tabla_Ope_Cat_Avaluo_Urbano_Re + " Au";
                My_Sql += " ON Ro." + Ope_Cat_Recepcion_Oficios.Campo_No_Oficio + " = Au." + Ope_Cat_Avaluo_Urbano_Re.Campo_No_Oficio + " LEFT OUTER JOIN " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " S";
                My_Sql += " ON Au." + Ope_Cat_Avaluo_Urbano_Re.Campo_Solicitud_Id + " = S." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " T";
                My_Sql += " ON S." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = T." + Cat_Tra_Tramites.Campo_Tramite_ID + " LEFT OUTER JOIN " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + " Tr";
                My_Sql += " ON T." + Cat_Tra_Tramites.Campo_Tramite_ID + " = Tr." + Cat_Tra_Datos_Tramite.Campo_Tramite_ID + " LEFT OUTER JOIN " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + " D";
                My_Sql += " ON Tr." + Cat_Tra_Datos_Tramite.Campo_Dato_ID + " = D." + Ope_Tra_Datos.Campo_Dato_ID + "";
                My_Sql += " WHERE upper(tr." + Cat_Tra_Datos_Tramite.Campo_Nombre + ") LIKE '%REGULARIZA%'";
                My_Sql += " AND (S." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " BETWEEN TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Avaluo.P_Valor_Inpa) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')  AND TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Avaluo.P_Valor_Inpr) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS'))";

                DataSet Ds_Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, My_Sql);
                if (Ds_Tabla != null)
                {
                    Dt_Regularizaciones = Ds_Tabla.Tables[0];
                }
            }
            catch (Exception E)
            {
                throw new Exception("Consultar_Solicitudes_Inconformidad: Error al consultar las Solicitudes de Inconformidades.");
            }
            return Dt_Regularizaciones;
        }

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
        public static DataTable Consultar_Solicitud_Tramite_Avaluos(Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio Tramite)
        {
            DataTable Dt_Tramites = new DataTable();
            String My_Sql = "";
            try
            {
                My_Sql = "SELECT TS." + Ope_Tra_Solicitud.Campo_Solicitud_ID                   
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

                 + ", TS." + Ope_Tra_Solicitud.Campo_Apellido_Paterno
                 + "||' '|| TS." + Ope_Tra_Solicitud.Campo_Apellido_Materno
                 + "||' '||TS." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " AS NOMBRE_COMPLETO"
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
