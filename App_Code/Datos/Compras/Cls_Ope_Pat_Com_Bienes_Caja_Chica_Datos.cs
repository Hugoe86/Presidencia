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
using Presidencia.Control_Patrimonial_Operacion_Bienes_Caja_Chica.Negocio;
using System.IO;

/// <summary>
/// Summary description for Cls_Ope_Pat_Com_Bienes_Caja_Chica_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Operacion_Bienes_Caja_Chica.Datos
{

    public class Cls_Ope_Pat_Com_Bienes_Caja_Chica_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Bien_Caja_Chica
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo Bien de Caja 
        ///                       Chica.
        ///PARAMETROS           : 
        ///                     1.  Bien. Contiene los parametros que se van a dar de
        ///                                     Alta en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 24/Enero/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Alta_Bien_Caja_Chica(Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Bien)
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
            try {
                String Bien_ID = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica, Ope_Pat_Bienes_Caja_Chica.Campo_Bien_ID, 10);
                String Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica;
                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Caja_Chica.Campo_Bien_ID + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Nombre + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Material_ID + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Color_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Marca_ID + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Modelo_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Numero_Inventario + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Costo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Estatus + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Estado;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Comentarios + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Fecha_Adquisicion + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Cantidad;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Bien_ID + "', '" + Bien.P_Nombre + "', '" + Bien.P_Dependencia_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien.P_Material_ID + "','" + Bien.P_Color_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien.P_Marca_ID + "','" + Bien.P_Modelo_ID + "'";
                Mi_SQL = Mi_SQL + "," + Bien.P_Numero_Inventario + ", '" + Bien.P_Costo + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien.P_Estatus + "', '" + Bien.P_Estado + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien.P_Comentarios + "', '" + String.Format("{0:dd/MM/yyyy}",Bien.P_Fecha_Adquisicion) + "', " + Bien.P_Cantidad + "";
                Mi_SQL = Mi_SQL + ",'" + Bien.P_Usuario_Nombre + "', SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Bien.P_Resguardantes != null && Bien.P_Resguardantes.Rows.Count > 0){
                    String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 50);
                    for (Int32 Cnt = 0; Cnt < Bien.P_Resguardantes.Rows.Count; Cnt++) {
                        Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Bien_ID + "', 'CAJA_CHICA','" + Bien.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                        Mi_SQL = Mi_SQL + ", '" + Bien.P_Usuario_ID + "', 'VIGENTE', '" + Bien.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                        Mi_SQL = Mi_SQL + ",'" + Bien.P_Usuario_Nombre + "', SYSDATE)";
                        ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                String ID_Consecutivo_Archivo = "";
                if (Bien.P_Archivo != null && Bien.P_Archivo.Trim().Length > 0)
                {
                    ID_Consecutivo_Archivo = Obtener_ID_Consecutivo(Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes, Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID, 50);
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes + " ( " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha_Creo + " ) VALUES ( " + Convert.ToInt32(ID_Consecutivo_Archivo) + ", '" + Bien_ID + "'";
                    Mi_SQL = Mi_SQL + " , 'CAJA_CHICA', SYSDATE, '" + Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Bien.P_Archivo) + "', 'NORMAL', '" + Bien.P_Usuario_Nombre + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                Bien.P_Bien_ID = Bien_ID;
                Trans.Commit();
                if (Bien.P_Archivo != null && Bien.P_Archivo.Trim().Length > 0)
                {
                    Bien.P_Archivo = Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Bien.P_Archivo);
                }
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]"; 
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]"; 
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else {
                    Mensaje = "Error al intentar dar de Alta un Bien. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
              
            }finally {
                 Cn.Close();
            }
            return Bien;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 
        ///                     1.  Bien.    Contiene los parametros que se van a utilizar para
        ///                                         hacer la consulta de la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 24/Enero/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Bien) {
            String Mi_SQL = null;
            DataSet Ds_Bien = null;
            DataTable Dt_Bien = new DataTable();
            try {
                if (Bien.P_Tipo_DataTable.Equals("DEPENDENCIAS")) {
                    Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID, " + Cat_Dependencias.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                } else if (Bien.P_Tipo_DataTable.Equals("EMPLEADOS")) {
                    Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID, " + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Campo_Nombre + " AS NOMBRE FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Bien.P_Dependencia_ID + "'";
                } else if (Bien.P_Tipo_DataTable.Equals("EMPLEADOS_BIEN")) {
                    Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID, " + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Campo_Nombre + " AS NOMBRE FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + " IN ( SELECT " + Ope_Pat_Bienes_Caja_Chica.Campo_Dependencia_ID + " FROM " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Caja_Chica.Campo_Bien_ID + " = '" + Bien.P_Bien_ID + "')";
                }
                else if (Bien.P_Tipo_DataTable.Equals("MATERIALES"))
                {
                    Mi_SQL = "SELECT " + Cat_Pat_Materiales.Campo_Material_ID + " AS MATERIAL_ID, " + Cat_Pat_Materiales.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales;
                }
                else if (Bien.P_Tipo_DataTable.Equals("MODELOS"))
                {
                    Mi_SQL = "SELECT " + Cat_Com_Modelos.Campo_Modelo_ID + " AS MODELO_ID, " + Cat_Com_Modelos.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos;
                }
                else if (Bien.P_Tipo_DataTable.Equals("MARCAS"))
                {
                    Mi_SQL = "SELECT " + Cat_Com_Marcas.Campo_Marca_ID + " AS MARCA_ID, " + Cat_Com_Marcas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                }
                else if (Bien.P_Tipo_DataTable.Equals("COLORES"))
                {
                    Mi_SQL = "SELECT " + Cat_Pat_Colores.Campo_Color_ID + " AS COLOR_ID, " + Cat_Pat_Colores.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                }
                else if (Bien.P_Tipo_DataTable.Equals("BIENES"))
                {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Bien_ID + " AS BIEN_ID";
                    Mi_SQL = Mi_SQL + ", " +  Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " +  Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Modelo_ID + " AS MODELO";
                    Mi_SQL = Mi_SQL + ", " +  Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Material_ID + " AS MATERIAL";
                    Mi_SQL = Mi_SQL + ", " +  Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Estatus + " AS ESTATUS";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                    if (Bien.P_Tipo_Filtro_Busqueda.Equals("DATOS_GENERALES")) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Nombre + " LIKE '%" + Bien.P_Nombre + "%'";
                        if (Bien.P_Material_ID != null && Bien.P_Material_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Material_ID + " = '" + Bien.P_Material_ID + "'";
                        }
                        if (Bien.P_Color_ID != null && Bien.P_Color_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Color_ID + " = '" + Bien.P_Color_ID + "'";
                        }
                        if (Bien.P_Marca_ID != null && Bien.P_Marca_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Marca_ID + " = '" + Bien.P_Marca_ID + "'";
                        }
                        if (Bien.P_Modelo_ID != null && Bien.P_Modelo_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Modelo_ID + " = '" + Bien.P_Modelo_ID + "'";
                        }
                        if (Bien.P_Estatus != null && Bien.P_Estatus.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Estatus + " = '" + Bien.P_Estatus + "'";    
                        }
                        if (Bien.P_Buscar_Fecha_Adquisicion) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Bien.P_Fecha_Adquisicion) + "'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Bien.P_Fecha_Adquisicion).AddDays(1).Date) + "'";
                        }
                        if (Bien.P_Dependencia_ID != null && Bien.P_Dependencia_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Campo_Dependencia_ID + " = '" + Bien.P_Dependencia_ID + "'";
                        }
                    } else if (Bien.P_Tipo_Filtro_Busqueda.Equals("RESGUARDANTES")) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + " IN ( SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CAJA_CHICA' AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                        if (Bien.P_RFC_Resguardante != null && Bien.P_RFC_Resguardante.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN ( SELECT " + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_RFC + " LIKE '%" + Bien.P_RFC_Resguardante + "%')";
                        }
                        Mi_SQL = Mi_SQL + ")";
                        if (Bien.P_Resguardante_ID != null && Bien.P_Resguardante_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Bien_ID;
                            Mi_SQL = Mi_SQL + " IN ( SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CAJA_CHICA' AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Bien.P_Resguardante_ID + "')";
                        }
                        if (Bien.P_Dependencia_ID != null && Bien.P_Dependencia_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Dependencia_ID;
                           Mi_SQL = Mi_SQL + " ='" + Bien.P_Dependencia_ID + "'";
                        }

                    }
                }
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                    Ds_Bien = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Bien != null) {
                    Dt_Bien = Ds_Bien.Tables[0];
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Bien;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Bien_Caja_Chica
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Bien de Caja 
        ///                       Chica.
        ///PARAMETROS:     
        ///             1. Bien. Contiene los parametros para actualizar el registro
        ///                         en la Base de Datos.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Bien_Caja_Chica(Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Bien) {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {
                Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Temporal_1 = Consultar_Datos_Bien(Bien);
                String Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica;
                Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Caja_Chica.Campo_Nombre + " = '" + Bien.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Cantidad + " = " + Bien.P_Cantidad + "";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Caja_Chica.Campo_Material_ID + " = '" + Bien.P_Material_ID  + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Caja_Chica.Campo_Color_ID + " = '" + Bien.P_Color_ID + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Caja_Chica.Campo_Marca_ID + " = '" + Bien.P_Marca_ID + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Caja_Chica.Campo_Modelo_ID + " = '" + Bien.P_Modelo_ID + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Caja_Chica.Campo_Estatus + " = '" + Bien.P_Estatus + "'";
                if (!Bien.P_Estatus.Trim().Equals("VIGENTE")) { 
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Caja_Chica.Campo_Motivo_Baja + " = '" + Bien.P_Motivo_Baja + "'";
                }
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Campo_Estado + " = '" + Bien.P_Estado + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Caja_Chica.Campo_Comentarios + " = '" + Bien.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Caja_Chica.Campo_Usuario_Modifico + " = '" + Bien.P_Usuario_Nombre + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Caja_Chica.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Caja_Chica.Campo_Bien_ID + " = '" + Bien.P_Bien_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery(); 
                if (Bien.P_Estatus.Trim().Equals("VIGENTE")) {
                    Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Temporal = Obtener_Diferencia_Resguardos(Temporal_1, Bien);
                    
                    //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                    for (Int32 Contador = 0; Contador < Temporal.P_Resguardantes.Rows.Count; Contador++) {
                        Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Bien.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal.P_Resguardantes.Rows[Contador][0].ToString() + "'";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }

                    Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Temporal_2 = Obtener_Diferencia_Resguardos(Bien, Temporal_1);

                    //SE DAN DE ALTA LOS NUEVOS RESGUARDANTES
                    if (Temporal_2.P_Resguardantes != null && Temporal_2.P_Resguardantes.Rows.Count > 0)
                    {
                        String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 50);
                        for (Int32 Cnt = 0; Cnt < Temporal_2.P_Resguardantes.Rows.Count; Cnt++)
                        {
                            Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Bien.P_Bien_ID + "', 'CAJA_CHICA','" + Temporal_2.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ", '" + Bien.P_Usuario_ID + "', 'VIGENTE', '" + Temporal_2.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                            Mi_SQL = Mi_SQL + ",'" + Bien.P_Usuario_Nombre + "', SYSDATE)";
                            ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                    }
                } else {
                    //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                    for (Int32 Contador = 0; Contador < Temporal_1.P_Resguardantes.Rows.Count; Contador++)
                    {
                        Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Bien.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString() + "'";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                String ID_Consecutivo_Archivo = "";
                if (Bien.P_Archivo != null && Bien.P_Archivo.Trim().Length > 0)
                {
                    ID_Consecutivo_Archivo = Obtener_ID_Consecutivo(Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes, Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID, 50);
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes + " ( " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha_Creo + " ) VALUES ( " + Convert.ToInt32(ID_Consecutivo_Archivo) + ", '" + Bien.P_Bien_ID + "'";
                    Mi_SQL = Mi_SQL + " , 'CAJA_CHICA', SYSDATE, '" + Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Bien.P_Archivo) + "', 'NORMAL', '" + Bien.P_Usuario_Nombre + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
                if (Bien.P_Archivo != null && Bien.P_Archivo.Trim().Length > 0)
                {
                    Bien.P_Archivo = Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Bien.P_Archivo);
                }
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]"; 
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]"; 
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else {
                    Mensaje = "Error al intentar Modificar el Bien. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }
                
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Bien
        ///DESCRIPCIÓN: Obtiene los Datos a Detalle de un Bien en Especifico.
        ///PARAMETROS:   
        ///             1. Parametros.   Bien que se va a ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Consultar_Datos_Bien(Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Parametros) {
            String Mi_SQL = "SELECT " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Bien_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Nombre;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Material_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Color_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Marca_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Modelo_ID;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Numero_Inventario;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Costo;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Estatus;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Motivo_Baja;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Estado;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Comentarios;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Fecha_Adquisicion;
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Cantidad;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + ", " + Cat_Dependencias.Tabla_Cat_Dependencias;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias  + "." + Cat_Dependencias.Campo_Dependencia_ID;
            if (!Parametros.P_Buscar_Numero_Inventario) {
                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Bien_ID + "";
                Mi_SQL = Mi_SQL + " = '" + Parametros.P_Bien_ID + "'";
            } else {
                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Caja_Chica.Tabla_Ope_Pat_Bienes_Caja_Chica + "." + Ope_Pat_Bienes_Caja_Chica.Campo_Numero_Inventario + "";
                Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Inventario + "'";
            }
            Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Bien   = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read()){
                    Bien.P_Bien_ID = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Bien_ID] != null) ? Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Bien_ID].ToString().Trim() : "";
                    Bien.P_Nombre = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Nombre] != null) ? Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Nombre].ToString().Trim() : "";
                    Bien.P_Dependencia_ID = (Data_Reader["DEPENDENCIA"] != null) ? Data_Reader["DEPENDENCIA"].ToString().Trim() : "";
                    Bien.P_Material_ID = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Material_ID] != null) ? Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Material_ID].ToString().Trim() : "";
                    Bien.P_Color_ID = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Color_ID] != null) ? Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Color_ID].ToString().Trim() : "";
                    Bien.P_Marca_ID = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Marca_ID] != null) ? Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Marca_ID].ToString().Trim() : "";
                    Bien.P_Modelo_ID = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Modelo_ID] != null) ? Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Modelo_ID].ToString().Trim() : "";
                    Bien.P_Numero_Inventario = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Numero_Inventario] != null) ? Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Numero_Inventario].ToString().Trim() : "";
                    Bien.P_Costo = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Costo] != null) ? Convert.ToDouble(Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Costo]) : 0.0;
                    Bien.P_Estatus = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Estatus] != null) ? Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Estatus].ToString().Trim() : "";
                    Bien.P_Motivo_Baja = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Motivo_Baja] != null) ? Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Motivo_Baja].ToString().Trim() : "";
                    Bien.P_Estado = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Estado] != null) ? Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Estado].ToString().Trim() : "";
                    Bien.P_Comentarios = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Comentarios] != null) ? Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Comentarios].ToString().Trim() : "";
                    Bien.P_Fecha_Adquisicion = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Fecha_Adquisicion] != null) ? Convert.ToDateTime(Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Fecha_Adquisicion]) : new DateTime();
                    Bien.P_Cantidad = (Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Cantidad] != null) ? Convert.ToInt32(Data_Reader[Ope_Pat_Bienes_Caja_Chica.Campo_Cantidad]) : 0;
                }
                Data_Reader.Close();
                DataSet Ds_Bien = null;
                if (Bien.P_Bien_ID != null && Bien.P_Bien_ID.Trim().Length > 0)
                {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                    Mi_SQL = Mi_SQL + " = 'VIGENTE'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                    Mi_SQL = Mi_SQL + " = 'CAJA_CHICA'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + " = " + Bien.P_Bien_ID + "";
                    Ds_Bien = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Bien == null)
                {
                    Bien.P_Resguardantes = new DataTable();
                }
                else
                {
                    Bien.P_Resguardantes = Ds_Bien.Tables[0];
                }
                Ds_Bien = null;
                if (Bien.P_Bien_ID != null && Bien.P_Bien_ID.Trim().Length > 0)
                {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " AS FECHA_INICIAL";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " AS FECHA_FINAL";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                    Mi_SQL = Mi_SQL + " = 'BAJA'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                    Mi_SQL = Mi_SQL + " = 'CAJA_CHICA'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + " = " + Bien.P_Bien_ID + "";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final;
                    Ds_Bien = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Bien == null)
                {
                    Bien.P_Historial_Resguardos = new DataTable();
                }
                else
                {
                    Bien.P_Historial_Resguardos = Ds_Bien.Tables[0];
                }

                Ds_Bien = null;
                if (Bien.P_Bien_ID != null && Bien.P_Bien_ID.Trim().Length > 0)
                {
                    Mi_SQL = "SELECT " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID + " AS ARCHIVO_BIEN_ID, " + Ope_Pat_Archivos_Bienes.Campo_Fecha + " AS FECHA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + " AS ARCHIVO, '' AS DESCRIPCION FROM " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Archivos_Bienes.Campo_Tipo + " = 'CAJA_CHICA' AND " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + "='" + Bien.P_Bien_ID + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Archivos_Bienes.Campo_Fecha + " DESC";
                    Ds_Bien = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Bien == null)
                {
                    Bien.P_Dt_Historial_Archivos = new DataTable();
                }
                else
                {
                    Bien.P_Dt_Historial_Archivos = Ds_Bien.Tables[0];
                }
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar los datos del Bien. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Bien;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Diferencia_Resguardos
        ///DESCRIPCIÓN: Saca la diferencia de unos resguardantes a otros.
        ///PARAMETROS:     
        ///             1. Actuales.        Bien Mueble como esta actualmente en la Base de Datos.
        ///             2. Actualizados.    Bien Mueble como quiere que quede al Actualizarlo.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 24/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Obtener_Diferencia_Resguardos(Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Comparar, Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Base_Comparacion) {
            Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio Resguardos = new Cls_Ope_Pat_Com_Bienes_Caja_Chica_Negocio();

            //SE OBTIENEN LOS NUEVOS RESGUARDANTES QUE SE DIERON DE ALTA PARA UN BIEN 
            DataTable Dt_Tabla = new DataTable();
            Dt_Tabla.Columns.Add("BIEN_RESGUARDO_ID", Type.GetType("System.Int32"));
            Dt_Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
            Dt_Tabla.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
            Dt_Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));
            for (int Contador_1 = 0; Contador_1 < Comparar.P_Resguardantes.Rows.Count; Contador_1++) {
                Boolean Eliminar = true;
                for (int Contador_2 = 0; Contador_2 < Base_Comparacion.P_Resguardantes.Rows.Count; Contador_2++) {
                    if (Comparar.P_Resguardantes.Rows[Contador_1][1].ToString().Equals(Base_Comparacion.P_Resguardantes.Rows[Contador_2][1].ToString())) {
                        Eliminar = false;
                        break;
                    }
                }
                if (Eliminar) {
                    DataRow Fila = Dt_Tabla.NewRow();
                    Fila["BIEN_RESGUARDO_ID"] = Convert.ToInt32(Comparar.P_Resguardantes.Rows[Contador_1][0]);
                    Fila["EMPLEADO_ID"] = Comparar.P_Resguardantes.Rows[Contador_1][1].ToString();
                    Fila["NOMBRE_EMPLEADO"] = Comparar.P_Resguardantes.Rows[Contador_1][2].ToString();
                    Fila["COMENTARIOS"] = Comparar.P_Resguardantes.Rows[Contador_1][3].ToString();
                    Dt_Tabla.Rows.Add(Fila);
                }
            }
            Resguardos.P_Resguardantes = Dt_Tabla;
            return Resguardos;
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
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID) {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals("")) {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            } catch (OracleException Ex) {
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
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID) {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }

    }
}