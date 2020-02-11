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
using Presidencia.Control_Patrimonial_Bienes_Sin_Inventario.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Collections.Generic;

/// <summary>
/// Summary description for Cls_Ope_Pat_Bienes_Sin_Inv_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Bienes_Sin_Inventario.Datos { 

    public class Cls_Ope_Pat_Bienes_Sin_Inv_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Bien_Sin_Inventario
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos un nuevo registro 
        ///                       de un Bien que no lleva Inventario.
        ///PARAMETROS           : Parametros.   Contiene los parametros que se van a dar de
        ///                                     Alta en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 03/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Alta_Bien_Sin_Inventario(Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Parametros) {
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
                String Bien_ID = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv, Ope_Pat_Bienes_Sin_Inv.Campo_Bien_ID, 15);
                if (Bien_ID == null || Bien_ID.Trim().Length == 0) {
                    Parametros.P_Bien_ID = 1;
                } else {
                    Parametros.P_Bien_ID = Convert.ToInt32(Bien_ID);
                }
                String Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv;
                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_ID  +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Tipo_Parent +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Nombre +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Marca_ID +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Modelo +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Costo_Inicial +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Material_ID +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Color_ID +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Estado +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Estatus +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Comentarios +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Fecha_Adquisicion +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Producto_ID +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Numero_Serie +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Usuario_Creo +
                                  ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ") VALUES ('" + Parametros.P_Bien_ID.ToString() + "'";
                Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Parent_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Parametros.P_Tipo_Parent + "'";
                Mi_SQL = Mi_SQL + ", '" + Parametros.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", '" + Parametros.P_Marca + "'";
                Mi_SQL = Mi_SQL + ", '" + Parametros.P_Modelo + "'";
                Mi_SQL = Mi_SQL + ", '" + Parametros.P_Costo_Inicial + "'";
                Mi_SQL = Mi_SQL + ", '" + Parametros.P_Material + "'";
                Mi_SQL = Mi_SQL + ", '" + Parametros.P_Color + "'";
                Mi_SQL = Mi_SQL + ", '" + Parametros.P_Estado + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", '" + Parametros.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion) + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Producto_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Parametros.P_Numero_Serie + "'";
                Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE)"; 
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Trans.Commit();
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
                    Mensaje = "Error al intentar dar de Alta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
            return Parametros;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modifica_Bien_Sin_Inventario
        ///DESCRIPCIÓN          : Modifica en la Base de Datos un registro de un Bien que 
        ///                       no lleva Inventario.
        ///PARAMETROS           : Parametros. Contiene los parametros que se van a Modificar
        ///                                   en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 03/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static void Modifica_Bien_Sin_Inventario(Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Parametros) {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            String Mi_SQL = "";
            try {

                Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv;
                Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID + " = '" + Parametros.P_Bien_Parent_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Tipo_Parent + " = '" + Parametros.P_Tipo_Parent + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Marca_ID + " = '" + Parametros.P_Marca + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Costo_Inicial + " = '" + Parametros.P_Costo_Inicial + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Material_ID + " = '" + Parametros.P_Material + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Color_ID + " = '" + Parametros.P_Color + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Fecha_Adquisicion + " = '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Adquisicion) + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Estado + " = '" + Parametros.P_Estado + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Comentarios + " = '" + Parametros.P_Comentarios + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Modelo + " = '" + Parametros.P_Modelo + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Numero_Serie + " = '" + Parametros.P_Numero_Serie + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Usuario_Modifico + " = '" + Parametros.P_Usuario_Nombre + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Fecha_Modifico + " = SYSDATE";
                if (Parametros.P_Motivo_Baja != null && Parametros.P_Motivo_Baja.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Campo_Motivo_Baja + " = '" + Parametros.P_Motivo_Baja + "'";
                }
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_ID +" = '" + Parametros.P_Bien_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
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
                    Mensaje = "Error al intentar Modificar. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Bienes_Sin_Inventario
        ///DESCRIPCIÓN          : Carga un listado de los bienes sin inventario, pasandolos
        ///                       por filtro.
        ///PARAMETROS           : Parametros. Contiene los parametros que se van a Consultar
        ///                                   en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 03/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Bienes_Sin_Inventario(Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Parametros) {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            try {
                if (Parametros.P_No_Empleado_Resguardante != null && Parametros.P_No_Empleado_Resguardante.Trim().Length > 0) {
                    Parametros.P_No_Empleado_Resguardante = Convertir_A_Formato_ID(Convert.ToInt32(Parametros.P_No_Empleado_Resguardante), 6);
                }
                Mi_SQL = "SELECT " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_ID + " AS BIEN_ID";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID + " AS BIEN_PARENT_ID";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Tipo_Parent + " AS TIPO_PARENT";
                Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Tipo_Parent + "";
                Mi_SQL = Mi_SQL + ", 'BIEN_MUEBLE', 'BIEN MUEBLE', 'VEHICULO', 'VEHÍCULO') AS TIPO_PARENT_DECODIFICADO";                
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Nombre + " AS NOMBRE";
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Modelo + " AS MODELO";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " AS COLOR";
                Mi_SQL = Mi_SQL + ", " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + " AS MATERIAL";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Numero_Serie+ " AS NUMERO_SERIE";
                Mi_SQL = Mi_SQL + ", 'MARCA: '|| NVL(" + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + ", '-')";
                Mi_SQL = Mi_SQL + " ||', MODELO: '|| NVL(" + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Modelo + ", '-')";
                Mi_SQL = Mi_SQL + " ||', COLOR: '|| NVL(" + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + ", '-')";
                Mi_SQL = Mi_SQL + " ||', MATERIAL: '|| NVL(" + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + ", '-')";
                Mi_SQL = Mi_SQL + " ||', NO. SERIE: '|| NVL(" + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Numero_Serie + ", 'S/S') AS CARACTERISTICAS";
                Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Estatus + "";
                Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Marca_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales;
                Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Material_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Color_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Nombre;
                Mi_SQL = Mi_SQL + " LIKE '%" + Parametros.P_Nombre.Trim() + "%'";
                if (Parametros.P_Marca != null && Parametros.P_Marca.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Marca.Trim() + "'";
                }
                if (Parametros.P_Modelo != null && Parametros.P_Modelo.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Modelo;
                    Mi_SQL = Mi_SQL + " LIKE '%" + Parametros.P_Modelo.Trim() + "%'";
                }
                if (Parametros.P_Material != null && Parametros.P_Material.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Material_ID;
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Material.Trim() + "'";
                }
                if (Parametros.P_Color != null && Parametros.P_Color.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Color_ID;
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Color.Trim() + "'";
                }
                if (Parametros.P_Numero_Serie != null && Parametros.P_Numero_Serie.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Numero_Serie;
                    Mi_SQL = Mi_SQL + " LIKE '%" + Parametros.P_Numero_Serie.Trim() + "%'";
                }
                if (Parametros.P_Estado != null && Parametros.P_Estado.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Estado;
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Estado.Trim() + "'";
                }
                if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Estatus;
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Estatus.Trim() + "'";
                }
                if (Parametros.P_Tipo_Parent != null && Parametros.P_Tipo_Parent.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Tipo_Parent;
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Tipo_Parent.Trim() + "'";
                    if (Parametros.P_Tipo_Parent != null && Parametros.P_Tipo_Parent.Trim().Equals("BIEN_MUEBLE")) {
                        if (Parametros.P_No_Inventario_Parent != null && Parametros.P_No_Inventario_Parent.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " LIKE  '%" + Parametros.P_No_Inventario_Parent.Trim() + "%'";
                            Mi_SQL = Mi_SQL + " OR " + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " =  '" + Parametros.P_No_Inventario_Parent.Trim() + "')";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + " =  '" + Parametros.P_Dependencia_ID.Trim() + "')";
                        }
                        if (Parametros.P_RFC_Resguardante != null && Parametros.P_RFC_Resguardante.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "";
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_RFC + " LIKE '%" + Parametros.P_RFC_Resguardante + "%')";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                            Mi_SQL = Mi_SQL + " UNION SELECT " + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + "";
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_RFC + " LIKE '%" + Parametros.P_RFC_Resguardante + "%')";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = 'VIGENTE'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE')";
                        }
                        if (Parametros.P_No_Empleado_Resguardante != null && Parametros.P_No_Empleado_Resguardante.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "";
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + Parametros.P_No_Empleado_Resguardante + "')";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                            Mi_SQL = Mi_SQL + " UNION SELECT " + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + "";
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + Parametros.P_No_Empleado_Resguardante + "')";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = 'VIGENTE'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE')";
                        }
                        if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Resguardante_ID + "'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE'";
                            Mi_SQL = Mi_SQL + " UNION SELECT " + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + "";
                            Mi_SQL = Mi_SQL + " = '" + Parametros.P_Resguardante_ID + "'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = 'VIGENTE'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE')";
                        }
                    }
                    if (Parametros.P_Tipo_Parent != null && Parametros.P_Tipo_Parent.Trim().Equals("VEHICULO")) { 
                        if (Parametros.P_No_Inventario_Parent != null && Parametros.P_No_Inventario_Parent.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " =  '" + Parametros.P_No_Inventario_Parent.Trim() + "')";
                        }
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Campo_Dependencia_ID + " =  '" + Parametros.P_Dependencia_ID.Trim() + "')";
                        }
                        if (Parametros.P_RFC_Resguardante != null && Parametros.P_RFC_Resguardante.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "";
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_RFC + " LIKE '%" + Parametros.P_RFC_Resguardante + "%')";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO')";
                        }
                        if (Parametros.P_No_Empleado_Resguardante != null && Parametros.P_No_Empleado_Resguardante.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + "";
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado + " = '" + Parametros.P_No_Empleado_Resguardante + "')";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO')";
                        }
                        if (Parametros.P_Resguardante_ID != null && Parametros.P_Resguardante_ID.Trim().Length > 0) {
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv + "." + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID;
                            Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Parametros.P_Resguardante_ID + "'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO')";
                        }
                    }
                }
                //FALTA LA BUSQUEDA POR LOS DATOS DEL PARENT...
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count>0) {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            } catch (Exception Ex) {
                throw new Exception("Consultar_Bienes_Sin_Inventario::: [" + Ex.Message + "]");
            }
            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
        ///DESCRIPCIÓN          : Carga una Tabla con los datos requeridos en los parametros.
        ///PARAMETROS           : Parametros. Contiene los parametros que se van a Consultar
        ///                                   en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 04/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Parametros) {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            Boolean Entro_Where = false;
            try {
                if (Parametros.P_Tipo_DataTable != null) {
                    if (Parametros.P_Tipo_DataTable.Trim().Equals("MARCAS")) {
                        Mi_SQL = "SELECT " + Cat_Com_Marcas.Campo_Marca_ID + " AS IDENTIFICADOR";
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Campo_Nombre + " AS TEXTO";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                        if (!String.IsNullOrEmpty(Parametros.P_Identificador_Generico)) {
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Marcas.Campo_Marca_ID + " = '" + Parametros.P_Identificador_Generico + "'";
                        }
                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Marcas.Campo_Nombre;
                    } else if (Parametros.P_Tipo_DataTable.Trim().Equals("MATERIALES")) {
                        Mi_SQL = "SELECT " + Cat_Pat_Materiales.Campo_Material_ID + " AS IDENTIFICADOR";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Materiales.Campo_Descripcion + " AS TEXTO";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales;
                        if (!String.IsNullOrEmpty(Parametros.P_Identificador_Generico)) {
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Materiales.Campo_Material_ID + " = '" + Parametros.P_Identificador_Generico + "'";
                        }
                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Materiales.Campo_Descripcion;
                    } else if (Parametros.P_Tipo_DataTable.Trim().Equals("COLORES")) {
                        Mi_SQL = "SELECT " + Cat_Pat_Colores.Campo_Color_ID + " AS IDENTIFICADOR";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Campo_Descripcion + " AS TEXTO";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                        if (!String.IsNullOrEmpty(Parametros.P_Identificador_Generico)) {
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Colores.Campo_Color_ID + " = '" + Parametros.P_Identificador_Generico + "'";
                        }
                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Colores.Campo_Descripcion;
                    } else if (Parametros.P_Tipo_DataTable.Trim().Equals("DEPENDENCIAS")) {
                        Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " AS IDENTIFICADOR";
                        Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Campo_Clave + " || ' - ' || " + Cat_Dependencias.Campo_Nombre + " AS TEXTO";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                        if (!String.IsNullOrEmpty(Parametros.P_Identificador_Generico)) {
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = '" + Parametros.P_Identificador_Generico + "'";
                        }
                        Mi_SQL = Mi_SQL + " ORDER BY TEXTO";
                    } else if (Parametros.P_Tipo_DataTable.Trim().Equals("TIPOS_VEHICULO")) {
                        Mi_SQL = "SELECT " + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + " AS IDENTIFICADOR";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " AS TEXTO";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo;
                        if (!String.IsNullOrEmpty(Parametros.P_Identificador_Generico)) {
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + " = '" + Parametros.P_Identificador_Generico + "'";
                        }
                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion;
                    } else if (Parametros.P_Tipo_DataTable.Trim().Equals("EMPLEADOS")) {
                        Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID";
                        Mi_SQL += ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEDO";
                        Mi_SQL += ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " AS APELLIDO_PATERNO";
                        Mi_SQL += ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " AS APELLIDO_MATERNO";
                        Mi_SQL += ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE";
                        Mi_SQL += ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL += " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL += " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_COMPLETO";
                        Mi_SQL += ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "";
                        Mi_SQL += " ||' - '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL += " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL += " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS EMPLEADO";
                        Mi_SQL += " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                        if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                            if (Entro_Where) {
                                Mi_SQL += " AND ";
                            } else {
                                Mi_SQL += " WHERE ";
                            }
                            Mi_SQL += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " ='" + Parametros.P_Dependencia_ID + "'";
                        }
                        Mi_SQL += " ORDER BY NOMBRE_COMPLETO";
                    } 
                }
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) { 
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null && Ds_Datos.Tables.Count>0) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                }
            } catch (Exception Ex) {
                throw new Exception("Consultar_Bienes_Sin_Inventario::: [" + Ex.Message + "]");
            }
            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Bien_Sin_Inventario
        ///DESCRIPCIÓN          : Carga un los Detalles de un Bien Sin Inventario.
        ///PARAMETROS           : Parametros. Contiene los parametros que se van a Consultar
        ///                                   en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 03/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Consultar_Detalles_Bien_Sin_Inventario(Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Parametros) {
            String Mi_SQL = "SELECT * FROM " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv +
                            " WHERE " + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_ID + " = '" + Parametros.P_Bien_ID + "'"; 
            Cls_Ope_Pat_Bienes_Sin_Inv_Negocio Bien = new Cls_Ope_Pat_Bienes_Sin_Inv_Negocio();
            OracleDataReader Data_Reader;
            try {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read()) {
                    Bien.P_Bien_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Bien_ID].ToString())) ? Convert.ToInt32(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Bien_ID]) : 0;
                    Bien.P_Bien_Parent_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Bien_Parent_ID].ToString() : "";
                    Bien.P_Tipo_Parent = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Tipo_Parent].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Tipo_Parent].ToString() : "";
                    Bien.P_Producto_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Producto_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Producto_ID].ToString() : "";
                    Bien.P_Nombre = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Nombre].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Nombre].ToString() : "";
                    Bien.P_Marca = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Marca_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Marca_ID].ToString() : "";
                    Bien.P_Modelo = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Modelo].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Modelo].ToString() : "";
                    Bien.P_Costo_Inicial = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Costo_Inicial].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Costo_Inicial]) : 0.0;
                    Bien.P_Material = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Material_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Material_ID].ToString() : "";
                    Bien.P_Color = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Color_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Color_ID].ToString() : "";
                    Bien.P_Estado = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Estado].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Estado].ToString() : "";
                    Bien.P_Estatus = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Estatus].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Estatus].ToString() : "";
                    Bien.P_Comentarios = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Comentarios].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Comentarios].ToString() : "";
                    Bien.P_Motivo_Baja = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Motivo_Baja].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Motivo_Baja].ToString() : "";
                    Bien.P_Fecha_Adquisicion = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Fecha_Adquisicion].ToString())) ? Convert.ToDateTime(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Fecha_Adquisicion]) : new DateTime();
                    Bien.P_Numero_Serie = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Numero_Serie].ToString())) ? Data_Reader[Ope_Pat_Bienes_Sin_Inv.Campo_Numero_Serie].ToString() : "";
                }
                if (Bien.P_Bien_ID > (-1)) {
                    Mi_SQL = "SELECT (" + Ope_Pat_Bienes_Sin_Inv.Campo_Usuario_Creo + " ||' ['|| TO_CHAR(" + Ope_Pat_Bienes_Sin_Inv.Campo_Fecha_Creo + ", 'DD/MM/YYYY') ||']') AS CREO";
                    Mi_SQL = Mi_SQL + ", (" + Ope_Pat_Bienes_Sin_Inv.Campo_Usuario_Modifico + " ||' ['|| TO_CHAR(" + Ope_Pat_Bienes_Sin_Inv.Campo_Fecha_Modifico + ", 'DD/MM/YYYY') ||']') AS MODIFICO";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Sin_Inv.Tabla_Ope_Pat_Bienes_Sin_Inv;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Sin_Inv.Campo_Bien_ID + " = '" + Bien.P_Bien_ID + "'";
                     Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                     while (Data_Reader.Read()) {
                         Bien.P_Dato_Creo = (!String.IsNullOrEmpty(Data_Reader["CREO"].ToString())) ? Data_Reader["CREO"].ToString() : "";
                         Bien.P_Dato_Modifico = (!String.IsNullOrEmpty(Data_Reader["MODIFICO"].ToString())) ? Data_Reader["MODIFICO"].ToString() : "";
                     }
                     Data_Reader.Close();
                }
                Data_Reader.Close();
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los datos de la Parte. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Bien;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
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
        ///PARAMETROS:     
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
            for (Int32 Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }

    }

}
