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
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;
using System.IO;

/// <summary>
/// Summary description for Cls_Ope_Pat_Com_Cemovientes_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Operacion_Cemovientes.Datos {

    public class Cls_Ope_Pat_Com_Cemovientes_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Cemoviente
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo Cemoviente.
        ///PARAMETROS           : 
        ///                     1.  Cemoviente. Contiene los parametros que se van a dar de
        ///                                     Alta en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 16/Diciembre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Cemovientes_Negocio Alta_Cemoviente(Cls_Ope_Pat_Com_Cemovientes_Negocio Cemoviente)
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
            String Mi_SQL = "";

            try
            {
                if (Cemoviente.P_No_Requisicion != null)  // Se actuaaliza el Producto
                {
                    Mi_SQL = " UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Req_Producto.Campo_Resguardado + " = 'SI'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = '" + Cemoviente.P_Producto_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = '" + Cemoviente.P_No_Requisicion + "'";

                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }


                Int64 No_Invetario = Consulta_Consecutivo_Inventario("RESGUARDO");
                Cemoviente.P_Numero_Inventario = No_Invetario;

                String Cemoviente_ID = Obtener_ID_Consecutivo(Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes, Ope_Pat_Cemovientes.Campo_Cemoviente_ID, 10);
                Mi_SQL = "INSERT INTO " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes;
                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Cemovientes.Campo_Cemoviente_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Producto_ID + ", " + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Procedencia + ", " + Ope_Pat_Cemovientes.Campo_Proveniente + ", " + Ope_Pat_Cemovientes.Campo_Donador_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Dependencia_ID + ", " + Ope_Pat_Cemovientes.Campo_Tipo_Alimentacion_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Tipo_Adiestramiento_ID + ", " + Ope_Pat_Cemovientes.Campo_Raza_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Funcion_ID + ", " + Ope_Pat_Cemovientes.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Numero_Inventario + ", " + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Estatus + ", " + Ope_Pat_Cemovientes.Campo_Observaciones;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Color_ID + ", " + Ope_Pat_Cemovientes.Campo_Costo_Alta + ", " + Ope_Pat_Cemovientes.Campo_Costo_Actual;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia + ", " + Ope_Pat_Cemovientes.Campo_Sexo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + ", " + Ope_Pat_Cemovientes.Campo_Fecha_Nacimiento;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Padre_ID + ", " + Ope_Pat_Cemovientes.Campo_Madre_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Cantidad;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_No_Factura;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Clasificacion_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Clase_Activo_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Usuario_Creo + ", " + Ope_Pat_Cemovientes.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Cemoviente_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Producto_ID + "', '" + Cemoviente.P_Numero_Inventario.ToString() + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Procedencia + "', '" + Cemoviente.P_Proveniente + "', '" + Cemoviente.P_Donador_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Dependencia_ID + "','" + Cemoviente.P_Tipo_Alimentacion_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Tipo_Adiestramiento_ID + "','" + Cemoviente.P_Raza_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Funcion_ID + "','" + Cemoviente.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", " + Cemoviente.P_Numero_Inventario + ",'" + String.Format("{0:dd/MM/yyyy}", Cemoviente.P_Fecha_Adquisicion) + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Estatus + "','" + Cemoviente.P_Observaciones + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Color_ID + "','" + Cemoviente.P_Costo_Actual + "','" + Cemoviente.P_Costo_Actual + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Tipo_Ascendencia + "','" + Cemoviente.P_Sexo + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Tipo_Cemoviente_ID + "','" + String.Format("{0:dd/MM/yyyy}", Cemoviente.P_Fecha_Nacimiento) + "'";
                if (Cemoviente.P_Padre_ID != null && Cemoviente.P_Padre_ID.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Padre_ID + "'";
                } else {
                    Mi_SQL = Mi_SQL + ", NULL";
                }
                if (Cemoviente.P_Madre_ID != null && Cemoviente.P_Madre_ID.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Madre_ID + "'";
                } else {
                    Mi_SQL = Mi_SQL + ", NULL";
                }
                Mi_SQL = Mi_SQL + ", " + Cemoviente.P_Cantidad.ToString() + "";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_No_Factura + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Proveedor_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Clasificacion_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Clase_Activo_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Usuario_Nombre + "', SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Cemoviente.P_Resguardantes != null && Cemoviente.P_Resguardantes.Rows.Count > 0)
                {
                    String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 50);
                    for (Int32 Cnt = 0; Cnt < Cemoviente.P_Resguardantes.Rows.Count; Cnt++)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estado + ", " + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta + ", " + Ope_Pat_Bienes_Resguardos.Campo_Observaciones;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Cemoviente_ID + "', 'CEMOVIENTE','" + Cemoviente.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                        Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Usuario_ID + "', 'VIGENTE', '" + Cemoviente.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                        Mi_SQL = Mi_SQL + ",'','" + Cemoviente.P_Dependencia_ID + "'";
                        Mi_SQL = Mi_SQL + ",'SI','" + Cemoviente.P_Observaciones + "'";
                        Mi_SQL = Mi_SQL + ",'" + Cemoviente.P_Usuario_Nombre + "', SYSDATE)";
                        ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }
                if (Cemoviente.P_Dt_Vacunas != null && Cemoviente.P_Dt_Vacunas.Rows.Count > 0)
                {
                    String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes, Ope_Pat_Vacunas_Cemovientes.Campo_Vacuna_Cemoviente_ID, 50);
                    for (Int32 Cnt = 0; Cnt < Cemoviente.P_Dt_Vacunas.Rows.Count; Cnt++)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_Vacunas_Cemovientes.Campo_Vacuna_Cemoviente_ID + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Vacuna_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Cemoviente_ID + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Veterinario_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Fecha_Aplicacion + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Comentarios;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Estatus;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Usuario_Creo + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Fecha_Creo + ")";
                        Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Cemoviente.P_Dt_Vacunas.Rows[Cnt][1].ToString() + "', '" + Cemoviente_ID + "','" + Cemoviente.P_Dt_Vacunas.Rows[Cnt][3].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", (DateTime)Cemoviente.P_Dt_Vacunas.Rows[Cnt][5]) + "', '" + Cemoviente.P_Dt_Vacunas.Rows[Cnt][6].ToString() + "', 'APLICADA'";
                        Mi_SQL = Mi_SQL + ",'" + Cemoviente.P_Usuario_Nombre + "', SYSDATE)";
                        ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                String ID_Consecutivo_Archivo = "";
                if (Cemoviente.P_Archivo != null && Cemoviente.P_Archivo.Trim().Length > 0)
                {
                    ID_Consecutivo_Archivo = Obtener_ID_Consecutivo(Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes, Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID, 50);
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes + " ( " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha_Creo + " ) VALUES ( " + Convert.ToInt32(ID_Consecutivo_Archivo) + ", '" + Cemoviente_ID + "'";
                    Mi_SQL = Mi_SQL + " , 'CEMOVIENTE', SYSDATE, '" + Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Cemoviente.P_Archivo) + "', 'NORMAL', '" + Cemoviente.P_Usuario_Nombre + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }



                // Asignar consulta para ingresar la factura
                Mi_SQL = "INSERT INTO " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles + " (";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario + ", "; // Es el No de  contra recibo
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Operacion + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES(" + Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles, Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario, 10)) + ", ";
                Mi_SQL = Mi_SQL + No_Invetario + ", ";
                Mi_SQL = Mi_SQL + "'" + Cemoviente.P_Producto_ID + "', ";
                Mi_SQL = Mi_SQL + "'RESGUARDO', ";
                Mi_SQL = Mi_SQL + "'" + Cemoviente.P_Usuario_ID + "', ";
                Mi_SQL = Mi_SQL + "SYSDATE)";

                //Ejecutar consulta
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery(); // Se ejecuta la operación

                Trans.Commit();
                Cemoviente.P_Cemoviente_ID = Cemoviente_ID;
                if (Cemoviente.P_Archivo != null && Cemoviente.P_Archivo.Trim().Length > 0) {
                    Cemoviente.P_Archivo = Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Cemoviente.P_Archivo);
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
                    Mensaje = "Error al intentar dar de Alta el Cemoviente. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
            return Cemoviente;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 
        ///                     1.  Cemoviente. Contiene los parametros que se van a utilizar para
        ///                                     hacer la consulta de la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 16/Diciembre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Ope_Pat_Com_Cemovientes_Negocio Cemoviente) {
            String Mi_SQL = null;
            DataSet Ds_Cemovientes = null;
            DataTable Dt_Cemovientes = new DataTable();
            try {
                if(Cemoviente.P_Tipo_DataTable.Equals("PRODUCTO")){
                    Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Nombre + ", " + Cat_Com_Productos.Campo_Clave + ", " + Cat_Com_Productos.Campo_Costo;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Cemoviente.P_Producto_ID + "'";
                } else if (Cemoviente.P_Tipo_DataTable.Equals("DEPENDENCIAS")) {
                    Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID, " + Cat_Dependencias.Campo_Clave + "||'-'|| " + Cat_Dependencias.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " + Cat_Dependencias.Campo_Estatus + " = 'ACTIVO' ORDER BY " + Cat_Dependencias.Campo_Nombre;
                } else if (Cemoviente.P_Tipo_DataTable.Equals("TIPOS_ALIMENTACION")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Tipos_Alimentacion.Campo_Tipo_Alimentacion_ID + " AS TIPO_ALIMENTACION_ID, " + Cat_Pat_Tipos_Alimentacion.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Tipos_Alimentacion.Tabla_Cat_Pat_Tipos_Alimentacion + " WHERE " + Cat_Pat_Tipos_Alimentacion.Campo_Estatus + " = 'VIGENTE' ORDER BY " + Cat_Pat_Tipos_Alimentacion.Campo_Nombre;
                } else if (Cemoviente.P_Tipo_DataTable.Equals("TIPOS_ASCENDENCIA")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Tipos_Ascendencia.Campo_Tipo_Ascendencia_ID + " AS TIPO_ASCENDENCIA_ID, " + Cat_Pat_Tipos_Ascendencia.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Tipos_Ascendencia.Tabla_Cat_Pat_Tipos_Ascendencia + " WHERE " + Cat_Pat_Tipos_Ascendencia.Campo_Estatus + " = 'VIGENTE' ORDER BY " + Cat_Pat_Tipos_Ascendencia.Campo_Nombre;
                } else if (Cemoviente.P_Tipo_DataTable.Equals("COLORES")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Colores.Campo_Color_ID + " AS COLOR_ID, " + Cat_Pat_Colores.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " WHERE " + Cat_Pat_Colores.Campo_Estatus + " = 'VIGENTE' ORDER BY " + Cat_Pat_Colores.Campo_Descripcion;
                } else if (Cemoviente.P_Tipo_DataTable.Equals("VACUNAS")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Vacunas.Campo_Vacuna_ID + " AS VACUNA_ID, " + Cat_Pat_Vacunas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Vacunas.Tabla_Cat_Pat_Vacunas + " WHERE " + Cat_Pat_Vacunas.Campo_Estatus + " = 'VIGENTE' ORDER BY " + Cat_Pat_Vacunas.Campo_Nombre;
                } else if (Cemoviente.P_Tipo_DataTable.Equals("TIPOS_ADIESTRAMIENTO")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Tipos_Adiestramiento.Campo_Tipo_Adiestramiento_ID + " AS TIPO_ADIESTRAMIENTO_ID, " + Cat_Pat_Tipos_Adiestramiento.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Tipos_Adiestramiento.Tabla_Cat_Pat_Tipos_Adiestramiento + " WHERE " + Cat_Pat_Tipos_Adiestramiento.Campo_Estatus + " = 'VIGENTE' ORDER BY " + Cat_Pat_Tipos_Adiestramiento.Campo_Nombre;
                } else if (Cemoviente.P_Tipo_DataTable.Equals("RAZAS")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Razas.Campo_Raza_ID + " AS RAZA_ID, " + Cat_Pat_Razas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + " WHERE " + Cat_Pat_Razas.Campo_Estatus + " = 'VIGENTE' ORDER BY " + Cat_Pat_Razas.Campo_Nombre;
                } else if (Cemoviente.P_Tipo_DataTable.Equals("FUNCIONES")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Funciones.Campo_Funcion_ID + " AS FUNCION_ID, " + Cat_Pat_Funciones.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Funciones.Tabla_Cat_Pat_Funciones + " WHERE " + Cat_Pat_Funciones.Campo_Estatus + " = 'VIGENTE' ORDER BY " + Cat_Pat_Funciones.Campo_Nombre;
                } else if (Cemoviente.P_Tipo_DataTable.Equals("VETERINARIOS")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Veterinarios.Campo_Veterinario_ID + " AS VETERINARIO_ID, " + Cat_Pat_Veterinarios.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' ' || " + Cat_Pat_Veterinarios.Campo_Apellido_Materno + " ||' '|| " + Cat_Pat_Veterinarios.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Veterinarios.Tabla_Cat_Pat_Veterinarios + " WHERE " + Cat_Pat_Veterinarios.Campo_Estatus + " = 'VIGENTE' ORDER BY " + Cat_Pat_Veterinarios.Campo_Nombre;
                } else if (Cemoviente.P_Tipo_DataTable.Equals("TIPOS_CEMOVIENTES")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID + " AS TIPO_CEMOVIENTE_ID, " + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + " WHERE " + Cat_Pat_Tipos_Cemovientes.Campo_Estatus + " = 'VIGENTE' ORDER BY " + Cat_Pat_Tipos_Cemovientes.Campo_Nombre;
                } else if (Cemoviente.P_Tipo_DataTable.Equals("EMPLEADOS")) {
                    Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID, " + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Campo_Nombre + " AS NOMBRE FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Cemoviente.P_Dependencia_ID + "' AND " + Cat_Empleados.Campo_Estatus + " = 'ACTIVO' ORDER BY NOMBRE";
                } else if (Cemoviente.P_Tipo_DataTable.Equals("EMPLEADOS_CEMOVIENTE")) {
                    Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID, " + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Campo_Nombre + " AS NOMBRE FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + " IN ( SELECT " + Ope_Pat_Cemovientes.Campo_Dependencia_ID + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " = '" + Cemoviente.P_Cemoviente_ID + "')" + " ORDER BY NOMBRE";
                } else if (Cemoviente.P_Tipo_DataTable.Equals("CEMOVIENTES_PADRES")) {
                    Mi_SQL = "SELECT " + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS CEMOVIENTE_ID, " + Ope_Pat_Cemovientes.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + " WHERE " + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + " = '" + Cemoviente.P_Tipo_Cemoviente_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Campo_Sexo + " = '" + Cemoviente.P_Sexo + "'";
                    Mi_SQL = Mi_SQL + "  ORDER BY NOMBRE";
                } else if (Cemoviente.P_Tipo_DataTable.Equals("SEMOVIENTES")) {
                    Mi_SQL = "SELECT " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " AS CEMOVIENTE_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior + " AS NUMERO_INVENTARIO_ANTERIOR";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Numero_Inventario + " AS NUMERO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + " AS TIPO_CEMOVIENTE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Nombre + " AS RAZA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Sexo + " AS SEXO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre + " AS CEMOVIENTE";
                    Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + ", " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + "." + Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Raza_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + "." + Cat_Pat_Razas.Campo_Raza_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Producto_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID;
                    if (Cemoviente.P_Dependencia_ID != null && Cemoviente.P_Dependencia_ID.Trim().Length > 0) { 
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = '" + Cemoviente.P_Dependencia_ID + "'";
                    }
                    if (Cemoviente.P_Numero_Inventario > (-1)) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Numero_Inventario;
                        Mi_SQL = Mi_SQL + " = '" + Cemoviente.P_Numero_Inventario + "'";
                    }
                    if (Cemoviente.P_Tipo_Ascendencia != null && Cemoviente.P_Tipo_Ascendencia.Trim().Length > 0) { 
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia;
                        Mi_SQL = Mi_SQL + " = '" + Cemoviente.P_Tipo_Ascendencia + "'";
                    }
                    if (Cemoviente.P_Tipo_Alimentacion_ID != null && Cemoviente.P_Tipo_Alimentacion_ID.Trim().Length > 0) { 
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Alimentacion_ID;
                        Mi_SQL = Mi_SQL + " = '" + Cemoviente.P_Tipo_Alimentacion_ID + "'";
                    }
                    if (Cemoviente.P_Tipo_Adiestramiento_ID != null && Cemoviente.P_Tipo_Adiestramiento_ID.Trim().Length > 0) { 
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Tipo_Adiestramiento_ID;
                        Mi_SQL = Mi_SQL + " = '" + Cemoviente.P_Tipo_Adiestramiento_ID + "'";
                    }
                    if (Cemoviente.P_Funcion_ID != null && Cemoviente.P_Funcion_ID.Trim().Length > 0) { 
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Funcion_ID;
                        Mi_SQL = Mi_SQL + " = '" + Cemoviente.P_Funcion_ID + "'";
                    }
                    if (Cemoviente.P_Color_ID != null && Cemoviente.P_Color_ID.Trim().Length > 0) { 
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " = '" + Cemoviente.P_Color_ID + "'";
                    }
                    if (Cemoviente.P_Raza_ID != null && Cemoviente.P_Raza_ID.Trim().Length > 0) { 
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Raza_ID;
                        Mi_SQL = Mi_SQL + " = '" + Cemoviente.P_Raza_ID + "'";
                    }
                    if (Cemoviente.P_Estatus != null && Cemoviente.P_Estatus.Trim().Length > 0) { 
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = '" + Cemoviente.P_Estatus + "'";
                    }
                    if (Cemoviente.P_RFC_Resguardante != null && Cemoviente.P_RFC_Resguardante.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Campo_RFC + " LIKE '%" + Cemoviente.P_RFC_Resguardante + "%' )";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'" + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CEMOVIENTE')";
                    }
                    if (Cemoviente.P_Resguardante_ID != null && Cemoviente.P_Resguardante_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Cemoviente.P_Resguardante_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'" + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'CEMOVIENTE')";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Nombre;
                }
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                    Ds_Cemovientes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Cemovientes != null) {
                    Dt_Cemovientes = Ds_Cemovientes.Tables[0];
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Cemovientes;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Cemoviente
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Cemoviente.
        ///PARAMETROS           :     
        ///                     1. Cemoviente.  Contiene los parametros para actualizar el 
        ///                                     registro en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO           : 16/Diciembre/2010 
        ///MODIFICO             : Francisco Antonio Gallardo Castañeda
        ///FECHA_MODIFICO       : 03/Enero/2011 
        ///CAUSA_MODIFICACIÓN   : Se le añadierón campos para hacer mas especificos los
        ///                       detalles del Cemoviente.
        ///*******************************************************************************
        public static void Modificar_Cemoviente(Cls_Ope_Pat_Com_Cemovientes_Negocio Cemoviente) {
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
                Cls_Ope_Pat_Com_Cemovientes_Negocio Temporal_1 = Consultar_Detalles_Cemoviente(Cemoviente);
                String Mi_SQL = "UPDATE " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes;
                Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Cemovientes.Campo_Tipo_Alimentacion_ID + " = '" + Cemoviente.P_Tipo_Alimentacion_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Dependencia_ID + " = '" + Cemoviente.P_Dependencia_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Tipo_Adiestramiento_ID + " = '" + Cemoviente.P_Tipo_Adiestramiento_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia + " = '" + Cemoviente.P_Tipo_Ascendencia + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Funcion_ID + " = '" + Cemoviente.P_Funcion_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Estatus + " = '" + Cemoviente.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_No_Factura + " = '" + Cemoviente.P_No_Factura + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Proveedor_ID + " = '" + Cemoviente.P_Proveedor_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Clasificacion_ID + " = '" + Cemoviente.P_Clasificacion_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Clase_Activo_ID + " = '" + Cemoviente.P_Clase_Activo_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Observaciones + " = '" + Cemoviente.P_Observaciones + "'";
                if (Cemoviente.P_Veterinario_ID != null && Cemoviente.P_Veterinario_ID.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Veterinario_ID + " = '" + Cemoviente.P_Veterinario_ID + "'";
                } else {
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Veterinario_ID + " = NULL";
                }
                if (Cemoviente.P_Padre_ID != null && Cemoviente.P_Padre_ID.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Padre_ID + " = '" + Cemoviente.P_Padre_ID + "'";
                } else {
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Padre_ID + " = NULL";
                }
                if (Cemoviente.P_Madre_ID != null && Cemoviente.P_Madre_ID.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Madre_ID + " = '" + Cemoviente.P_Madre_ID + "'";
                } else {
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Madre_ID + " = NULL";
                }
                if (!Cemoviente.P_Estatus.Equals("VIGENTE")) {
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Motivo_Baja + " = '" + Cemoviente.P_Motivo_Baja + "'";
                }
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Nombre + " = '" + Cemoviente.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID + " = '" + Cemoviente.P_Tipo_Cemoviente_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Raza_ID + " = '" + Cemoviente.P_Raza_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Color_ID + " = '" + Cemoviente.P_Color_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Fecha_Nacimiento + " = '" + String.Format("{0:dd/MM/yyyy}", Cemoviente.P_Fecha_Nacimiento) + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion + " = '" + String.Format("{0:dd/MM/yyyy}", Cemoviente.P_Fecha_Adquisicion) + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Sexo + " = '" + Cemoviente.P_Sexo + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Costo_Actual + " = '" + Cemoviente.P_Costo_Actual + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Usuario_Modifico + " = '" + Cemoviente.P_Usuario_Nombre + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " = '" + Cemoviente.P_Cemoviente_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Cemoviente.P_Dt_Vacunas != null && Cemoviente.P_Dt_Vacunas.Rows.Count > 0) {
                    String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes, Ope_Pat_Vacunas_Cemovientes.Campo_Vacuna_Cemoviente_ID, 50);
                    for (Int32 Cnt = 0; Cnt < Cemoviente.P_Dt_Vacunas.Rows.Count; Cnt++) {
                        if (!Cemoviente.P_Dt_Vacunas.Rows[Cnt]["ESTATUS"].ToString().Equals("APLICADA")) {
                            if (Cemoviente.P_Dt_Vacunas.Rows[Cnt]["ESTATUS"].ToString().Equals("NUEVA")) {
                                Mi_SQL = "INSERT INTO " + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes;
                                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Vacunas_Cemovientes.Campo_Vacuna_Cemoviente_ID + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Vacuna_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Cemoviente_ID + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Veterinario_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Fecha_Aplicacion + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Comentarios;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Estatus;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Usuario_Creo + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Fecha_Creo + ")";
                                Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Cemoviente.P_Dt_Vacunas.Rows[Cnt][1].ToString() + "', '" + Cemoviente.P_Cemoviente_ID + "','" + Cemoviente.P_Dt_Vacunas.Rows[Cnt][3].ToString() + "'";
                                Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", (DateTime)Cemoviente.P_Dt_Vacunas.Rows[Cnt][5]) + "', '" + Cemoviente.P_Dt_Vacunas.Rows[Cnt][6].ToString() + "', 'APLICADA'";
                                Mi_SQL = Mi_SQL + ",'" + Cemoviente.P_Usuario_Nombre + "', SYSDATE)";
                                ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);
                            }else {
                                Mi_SQL = "UPDATE " + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes;
                                Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Vacunas_Cemovientes.Campo_Estatus + "= 'CANCELADA'";
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vacunas_Cemovientes.Campo_Motivo_Cancelacion + " = '" + Cemoviente.P_Dt_Vacunas.Rows[Cnt]["MOTIVO_CANCELACION"].ToString().Trim() + "'";
                                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vacunas_Cemovientes.Campo_Vacuna_Cemoviente_ID + " = '" + Cemoviente.P_Dt_Vacunas.Rows[Cnt]["VACUNA_CEMOVIENTE_ID"].ToString().Trim() + "'";
                            }
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();                        
                        }
                    }
                }
                if (Cemoviente.P_Estatus.Trim().Equals("VIGENTE")) {

                    Cls_Ope_Pat_Com_Cemovientes_Negocio Temporal = Obtener_Diferencia_Resguardos(Temporal_1, Cemoviente);

                    //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                    for (Int32 Contador = 0; Contador < Temporal.P_Resguardantes.Rows.Count; Contador++) {
                        Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Cemoviente.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal.P_Resguardantes.Rows[Contador][0].ToString() + "'";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }

                    Cls_Ope_Pat_Com_Cemovientes_Negocio Temporal_2 = Obtener_Diferencia_Resguardos(Cemoviente, Temporal_1);

                    //SE DAN DE ALTA LOS NUEVOS RESGUARDANTES
                    if (Temporal_2.P_Resguardantes != null && Temporal_2.P_Resguardantes.Rows.Count > 0) {
                        for (Int32 Cnt = 0; Cnt < Temporal_2.P_Resguardantes.Rows.Count; Cnt++) {
                            String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 50);
                            Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estado + ", " + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Modificacion + ", " + Ope_Pat_Bienes_Resguardos.Campo_Observaciones;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Cemoviente.P_Cemoviente_ID + "', 'CEMOVIENTE','" + Temporal_2.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Usuario_ID + "', 'VIGENTE', '" + Temporal_2.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                            Mi_SQL = Mi_SQL + ",'','" + Cemoviente.P_Dependencia_ID + "'";
                            Mi_SQL = Mi_SQL + ",'SI','" + Cemoviente.P_Observaciones + "'";
                            Mi_SQL = Mi_SQL + ",'" + Cemoviente.P_Usuario_Nombre + "', SYSDATE)";
                            ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                    }
                } else {
                    for (Int32 Contador = 0; Contador < Temporal_1.P_Resguardantes.Rows.Count; Contador++) {
                        Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja + " = 'SI'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Cemoviente.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString() + "'";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                }

                String ID_Consecutivo_Archivo = "";
                if (Cemoviente.P_Archivo != null && Cemoviente.P_Archivo.Trim().Length > 0) {
                    ID_Consecutivo_Archivo = Obtener_ID_Consecutivo(Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes, Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID, 50);
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes + " ( " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha_Creo + " ) VALUES ( " + Convert.ToInt32(ID_Consecutivo_Archivo) + ", '" + Cemoviente.P_Cemoviente_ID + "'";
                    Mi_SQL = Mi_SQL + " , 'CEMOVIENTE', SYSDATE, '" + Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Cemoviente.P_Archivo) + "', 'NORMAL', '" + Cemoviente.P_Usuario_Nombre + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
                if (Cemoviente.P_Archivo != null && Cemoviente.P_Archivo.Trim().Length > 0) {
                    Cemoviente.P_Archivo = Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Cemoviente.P_Archivo);
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
                    Mensaje = "Error al intentar Modificar el Cemoviente. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }
                
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Cemoviente
        ///DESCRIPCIÓN          : Obtiene los Datos a Detalle de un Cemoviente en Especifico.
        ///PARAMETROS           :   
        ///                     1. Parametros.   Cemoviente que se va a ver a Detalle.
        ///CREO                 : Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO           : 16/Diciembre/2010 
        ///MODIFICO             : Francisco Antonio Gallardo Castañeda
        ///FECHA_MODIFICO       : 03/Enero/2011 
        ///CAUSA_MODIFICACIÓN   : Se le añadierón campos para hacer mas especificos los
        ///                       detalles del Cemoviente.
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Cemovientes_Negocio Consultar_Detalles_Cemoviente(Cls_Ope_Pat_Com_Cemovientes_Negocio Parametros) {
            Boolean Entro_where = false;
            String Mi_SQL = "SELECT * FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes;
            if (Parametros.P_Cemoviente_ID != null && Parametros.P_Cemoviente_ID.Trim().Length > 0) {
                if (!Entro_where) {
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + "";
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Cemoviente_ID + "'";
                    Entro_where = true;
                } else {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + "";
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Cemoviente_ID + "'";
                }
            }
            if (Parametros.P_Numero_Inventario > (-1)) {
                if (!Entro_where) {
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Numero_Inventario + "";
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Inventario + "'";
                    Entro_where = true;
                } else {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_Numero_Inventario + "";
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Inventario + "'";
                }
            }
            if (Parametros.P_No_Inventario_Anterior != null && Parametros.P_No_Inventario_Anterior.Trim().Length > 0) {
                if (!Entro_where) {
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior + "";
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_No_Inventario_Anterior + "'";
                    Entro_where = true;
                } else {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes + "." + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior + "";
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_No_Inventario_Anterior + "'";
                }
            }
            Cls_Ope_Pat_Com_Cemovientes_Negocio Cemoviente = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
            OracleDataReader Data_Reader;
            try {
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read()) {
                    Cemoviente.P_Cemoviente_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Cemoviente_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Cemoviente_ID].ToString() : "";
                    Cemoviente.P_Producto_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Producto_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Producto_ID].ToString() : "";
                    Cemoviente.P_Numero_Inventario = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Numero_Inventario].ToString())) ? Convert.ToInt32(Data_Reader[Ope_Pat_Cemovientes.Campo_Numero_Inventario]) : 0;
                    Cemoviente.P_No_Inventario_Anterior = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior].ToString() : "";
                    Cemoviente.P_Raza_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Raza_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Raza_ID].ToString() : "";
                    Cemoviente.P_Tipo_Adiestramiento_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Tipo_Adiestramiento_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Tipo_Adiestramiento_ID].ToString() : "";
                    Cemoviente.P_Tipo_Ascendencia = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia].ToString() : "";
                    Cemoviente.P_Funcion_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Funcion_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Funcion_ID].ToString() : "";
                    Cemoviente.P_Dependencia_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Dependencia_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Dependencia_ID].ToString() : "";
                    Cemoviente.P_Color_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Color_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Color_ID].ToString() : "";
                    Cemoviente.P_Tipo_Alimentacion_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Tipo_Alimentacion_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Tipo_Alimentacion_ID].ToString() : "";
                    Cemoviente.P_Nombre = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Nombre].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Nombre].ToString() : "";
                    Cemoviente.P_Veterinario_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Veterinario_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Veterinario_ID].ToString() : "";
                    Cemoviente.P_Costo_Actual = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Costo_Actual].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Cemovientes.Campo_Costo_Actual]) : 0;
                    Cemoviente.P_Costo_Inicial = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Costo_Alta].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Cemovientes.Campo_Costo_Alta]) : 0;
                    Cemoviente.P_Fecha_Nacimiento = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Fecha_Nacimiento].ToString())) ? Convert.ToDateTime(Data_Reader[Ope_Pat_Cemovientes.Campo_Fecha_Nacimiento]) : new DateTime();
                    Cemoviente.P_Fecha_Adquisicion = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion].ToString())) ? Convert.ToDateTime(Data_Reader[Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion]) : new DateTime();
                    Cemoviente.P_Estatus = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Estatus].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Estatus].ToString() : "";
                    Cemoviente.P_Motivo_Baja = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Motivo_Baja].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Motivo_Baja].ToString() : "";
                    Cemoviente.P_Observaciones = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Observaciones].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Observaciones].ToString() : "";
                    Cemoviente.P_Cantidad = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Cantidad].ToString())) ? Convert.ToInt32(Data_Reader[Ope_Pat_Cemovientes.Campo_Cantidad]) : 0;
                    Cemoviente.P_Sexo = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Sexo].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Sexo].ToString() : "";
                    Cemoviente.P_Padre_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Padre_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Padre_ID].ToString() : "";
                    Cemoviente.P_Madre_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Madre_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Madre_ID].ToString() : "";
                    Cemoviente.P_Tipo_Cemoviente_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID].ToString() : "";
                    Cemoviente.P_No_Factura = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_No_Factura].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_No_Factura].ToString() : "";
                    Cemoviente.P_Proveedor_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Proveedor_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Proveedor_ID].ToString() : "";
                    Cemoviente.P_Clasificacion_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Clasificacion_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Clasificacion_ID].ToString() : "";
                    Cemoviente.P_Clase_Activo_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Cemovientes.Campo_Clase_Activo_ID].ToString())) ? Data_Reader[Ope_Pat_Cemovientes.Campo_Clase_Activo_ID].ToString() : "";
                }
                Data_Reader.Close();
                Data_Reader = null;
                if (Cemoviente.P_Cemoviente_ID != null && Cemoviente.P_Cemoviente_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT (" + Ope_Pat_Cemovientes.Campo_Usuario_Creo + " ||' ['|| TO_CHAR(" + Ope_Pat_Cemovientes.Campo_Fecha_Creo + ", 'DD/MM/YYYY') ||']') AS CREO";
                    Mi_SQL = Mi_SQL + ", (" + Ope_Pat_Cemovientes.Campo_Usuario_Modifico + " ||' ['|| TO_CHAR(" + Ope_Pat_Cemovientes.Campo_Fecha_Modifico + ", 'DD/MM/YYYY') ||']') AS MODIFICO";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Cemovientes.Campo_Cemoviente_ID + " = '" + Cemoviente.P_Cemoviente_ID + "'";
                     Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                     while (Data_Reader.Read()) {
                         Cemoviente.P_Dato_Creacion = (!String.IsNullOrEmpty(Data_Reader["CREO"].ToString())) ? Data_Reader["CREO"].ToString() : "";
                         Cemoviente.P_Dato_Modificacion = (!String.IsNullOrEmpty(Data_Reader["MODIFICO"].ToString())) ? Data_Reader["MODIFICO"].ToString() : "";
                     }
                     Data_Reader.Close();
                }
                //if (Cemoviente.P_Producto_ID != null && Cemoviente.P_Producto_ID.Trim().Length > 0) {
                //    Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Costo + " AS COSTO_INICIAL FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                //    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Cemoviente.P_Producto_ID + "'";
                //}
                //Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //while (Data_Reader.Read()){
                //    Cemoviente.P_Costo_Inicial = (Data_Reader["COSTO_INICIAL"] != null) ? Convert.ToDouble(Data_Reader["COSTO_INICIAL"]) : 0;
                //}
                DataSet Ds_Cemoviente = null;
                if (Cemoviente.P_Cemoviente_ID != null && Cemoviente.P_Cemoviente_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + " = '" + Cemoviente.P_Cemoviente_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                    Mi_SQL = Mi_SQL + " = 'VIGENTE'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                    Mi_SQL = Mi_SQL + " = 'CEMOVIENTE'";
                    Ds_Cemoviente = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Cemoviente == null) {
                    Cemoviente.P_Resguardantes = new DataTable();
                } else {
                    Cemoviente.P_Resguardantes = Ds_Cemoviente.Tables[0];
                }
                Ds_Cemoviente = null;
                if (Cemoviente.P_Cemoviente_ID != null && Cemoviente.P_Cemoviente_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " AS FECHA_INICIAL";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " AS FECHA_FINAL";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + " = '" + Cemoviente.P_Cemoviente_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                    Mi_SQL = Mi_SQL + " = 'BAJA'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                    Mi_SQL = Mi_SQL + " = 'CEMOVIENTE'";
                    Ds_Cemoviente = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Cemoviente == null) {
                    Cemoviente.P_Historial_Resguardos = new DataTable();
                }  else {
                    Cemoviente.P_Historial_Resguardos = Ds_Cemoviente.Tables[0];
                }
                Ds_Cemoviente = null;
                if (Cemoviente.P_Cemoviente_ID != null && Cemoviente.P_Cemoviente_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + "." + Ope_Pat_Vacunas_Cemovientes.Campo_Vacuna_Cemoviente_ID + " AS VACUNA_CEMOVIENTE_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + "." + Ope_Pat_Vacunas_Cemovientes.Campo_Vacuna_ID + " AS VACUNA_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Vacunas.Tabla_Cat_Pat_Vacunas + "." + Cat_Pat_Vacunas.Campo_Nombre + " AS VACUNA_NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + "." + Ope_Pat_Vacunas_Cemovientes.Campo_Veterinario_ID + " AS VETERINARIO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Veterinarios.Tabla_Cat_Pat_Veterinarios + "." + Cat_Pat_Veterinarios.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pat_Veterinarios.Tabla_Cat_Pat_Veterinarios + "." + Cat_Pat_Veterinarios.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Pat_Veterinarios.Tabla_Cat_Pat_Veterinarios + "." + Cat_Pat_Veterinarios.Campo_Nombre + " AS VETERINARIO_NOMBRE";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + "." + Ope_Pat_Vacunas_Cemovientes.Campo_Fecha_Aplicacion + " AS FECHA_APLICACION";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + "." + Ope_Pat_Vacunas_Cemovientes.Campo_Comentarios + " AS COMENTARIOS";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + "." + Ope_Pat_Vacunas_Cemovientes.Campo_Estatus + " AS ESTATUS";
                    Mi_SQL = Mi_SQL + "," + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + "." + Ope_Pat_Vacunas_Cemovientes.Campo_Motivo_Cancelacion + " AS MOTIVO_CANCELACION";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + ", " + Cat_Pat_Vacunas.Tabla_Cat_Pat_Vacunas + ", " + Cat_Pat_Veterinarios.Tabla_Cat_Pat_Veterinarios;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + "." + Ope_Pat_Vacunas_Cemovientes.Campo_Vacuna_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Vacunas.Tabla_Cat_Pat_Vacunas + "." + Cat_Pat_Vacunas.Campo_Vacuna_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + "." + Ope_Pat_Vacunas_Cemovientes.Campo_Veterinario_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Veterinarios.Tabla_Cat_Pat_Veterinarios + "." + Cat_Pat_Veterinarios.Campo_Veterinario_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + "." + Ope_Pat_Vacunas_Cemovientes.Campo_Cemoviente_ID;
                    Mi_SQL = Mi_SQL + " = '" + Cemoviente.P_Cemoviente_ID + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + "." + Ope_Pat_Vacunas_Cemovientes.Campo_Fecha_Aplicacion + " DESC";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vacunas_Cemovientes.Tabla_Ope_Pat_Vacunas_Cemovientes + "." + Ope_Pat_Vacunas_Cemovientes.Campo_Estatus + " ASC";
                    Ds_Cemoviente = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Cemoviente == null) {
                    Cemoviente.P_Dt_Vacunas = new DataTable();
                } else {
                    Cemoviente.P_Dt_Vacunas = Ds_Cemoviente.Tables[0];
                }
                Ds_Cemoviente = null;
                if (Cemoviente.P_Cemoviente_ID != null && Cemoviente.P_Cemoviente_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID + " AS ARCHIVO_BIEN_ID, " + Ope_Pat_Archivos_Bienes.Campo_Fecha + " AS FECHA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + " AS ARCHIVO, '' AS DESCRIPCION FROM " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Archivos_Bienes.Campo_Tipo + " = 'CEMOVIENTE' AND " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + "='" + Cemoviente.P_Cemoviente_ID + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Archivos_Bienes.Campo_Fecha + " DESC";
                    Ds_Cemoviente = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Cemoviente == null) {
                    Cemoviente.P_Dt_Historial_Archivos = new DataTable();
                } else {
                    Cemoviente.P_Dt_Historial_Archivos = Ds_Cemoviente.Tables[0];
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los datos del Cemoviente. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Cemoviente;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Diferencia_Resguardos
        ///DESCRIPCIÓN: Saca la diferencia de unos resguardantes a otros.
        ///PARAMETROS:     
        ///             1. Actuales.        Cemoviente como esta actualmente en la Base de Datos.
        ///             2. Actualizados.    Cemoviente como quiere que quede al Actualizarlo.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 16/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Cls_Ope_Pat_Com_Cemovientes_Negocio Obtener_Diferencia_Resguardos(Cls_Ope_Pat_Com_Cemovientes_Negocio Comparar, Cls_Ope_Pat_Com_Cemovientes_Negocio Base_Comparacion) {
            Cls_Ope_Pat_Com_Cemovientes_Negocio Resguardos = new Cls_Ope_Pat_Com_Cemovientes_Negocio();

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

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Consecutivo_Inventario
        /// DESCRIPCION:            Método utilizado para consultar las ordenes de compra que se encuentren en estatus "SURTIDA"
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene la información para realizar la consulta
        ///                         
        /// CREO       :            Salvador Hernandez Ramirez
        /// FECHA_CREO :            14/Marzo/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static Int64 Consulta_Consecutivo_Inventario(String Operacion) {
            String Mi_SQL = String.Empty; //Variable para las consultas
            Object Consecutivo;
            Int64 No_Consecutivo;

            try {
                // Consulta 
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario + "), 0) ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Operacion + "= '" + Operacion + "'";

                //Ejecutar consulta
                Consecutivo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Verificar si no es nulo
                if (Consecutivo != null && Convert.IsDBNull(Consecutivo) == false)
                    No_Consecutivo = Convert.ToInt64(Consecutivo) + 1;
                else
                    No_Consecutivo = 1;

                return No_Consecutivo;
            } catch (OracleException ex){ 
                throw new Exception("Error: " + ex.Message);
            } catch (DBConcurrencyException ex) {
                throw new Exception("Error: " + ex.Message);
            } catch (Exception ex) {
                throw new Exception("Error: " + ex.Message);
            } finally {
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Cemoviente
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo Cemoviente.
        ///PARAMETROS           : 
        ///                     1.  Cemoviente. Contiene los parametros que se van a dar de
        ///                                     Alta en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 16/Diciembre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Cemovientes_Negocio Alta_Migrar_Cemoviente(Cls_Ope_Pat_Com_Cemovientes_Negocio Cemoviente)
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
            String Mi_SQL = "";

            try {

                Int64 No_Invetario = Consulta_Consecutivo_Inventario("RESGUARDO");
                Cemoviente.P_Numero_Inventario = No_Invetario;

                Cemoviente.P_Cemoviente_ID = Obtener_ID_Consecutivo(Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes, Ope_Pat_Cemovientes.Campo_Cemoviente_ID, 10);
                Mi_SQL = "INSERT INTO " + Ope_Pat_Cemovientes.Tabla_Ope_Pat_Cemovientes;
                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Cemovientes.Campo_Cemoviente_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Producto_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Tipo_Alimentacion_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Tipo_Adiestramiento_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Raza_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Funcion_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Numero_Inventario;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_No_Inventario_Anterior;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Fecha_Adquisicion;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Observaciones;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Color_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Costo_Actual;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Tipo_Ascendencia;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Sexo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Tipo_Cemoviente_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Procedencia;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_No_Factura;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Usuario_Creo;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Cemovientes.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ") VALUES ('" + Cemoviente.P_Cemoviente_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Producto_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Dependencia_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Tipo_Alimentacion_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Tipo_Adiestramiento_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Raza_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Funcion_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Nombre + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Numero_Inventario.ToString() + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_No_Inventario_Anterior + "'";
                Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Cemoviente.P_Fecha_Adquisicion) + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Observaciones + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Color_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Costo_Actual + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Tipo_Ascendencia + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Sexo + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Tipo_Cemoviente_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Procedencia + "'";
                Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_No_Factura + "'";
                Mi_SQL = Mi_SQL + ", 'FERNANDO AYALA CASTAÑEDA'";
                Mi_SQL = Mi_SQL + ", SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                
                //Alta de los Resguardos
                if (Cemoviente.P_Resguardantes != null && Cemoviente.P_Resguardantes.Rows.Count > 0){
                    Int32 ID_Consecutivo = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 20));
                    for (Int32 Cnt = 0; Cnt < Cemoviente.P_Resguardantes.Rows.Count; Cnt++) {
                        Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                        if (Cemoviente.P_Estatus.Trim().Equals("DEFINITIVA")) {
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final;
                        }
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES ('" + ID_Consecutivo + "'";
                        Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Cemoviente_ID + "'";
                        Mi_SQL = Mi_SQL + ", 'CEMOVIENTE'";
                        Mi_SQL = Mi_SQL + ", '" + Cemoviente.P_Resguardantes.Rows[Cnt]["EMPLEADO_ID"].ToString() + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE";
                        Mi_SQL = Mi_SQL + ", 'LA FECHA INICIAL DEL RESGUARDO ES LA FECHA EN QUE SE DIO DE ALTA EN EL SISTEMA SIAS LA FECHA INICIAL REAL.'";
                        if (Cemoviente.P_Estatus.Trim().Equals("DEFINITIVA")) {
                            Mi_SQL = Mi_SQL + ", SYSDATE";
                            Mi_SQL = Mi_SQL + ", 'BAJA'";
                        } else {
                            Mi_SQL = Mi_SQL + ", 'VIGENTE'";
                        }
                        Mi_SQL = Mi_SQL + ", 'FERNANDO AYALA CASTAÑEDA'";
                        Mi_SQL = Mi_SQL + ", SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        ID_Consecutivo = ID_Consecutivo + 1;
                    }
                }

                // Asignar consulta para ingresar la factura
                Int64 Inventario_ID = Convert.ToInt64(Obtener_ID_Consecutivo(Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles, Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario, 25));
                Mi_SQL = "INSERT INTO " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles + " (";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario + ", "; // Es el No de  contra recibo
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Operacion + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES(" + Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles, Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario, 10)) + ", ";
                Mi_SQL = Mi_SQL + Cemoviente.P_Numero_Inventario + ", ";
                Mi_SQL = Mi_SQL + "'" + Cemoviente.P_Producto_ID + "', ";
                Mi_SQL = Mi_SQL + "'RESGUARDO', ";
                Mi_SQL = Mi_SQL + "'" + Cemoviente.P_Usuario_ID + "', ";
                Mi_SQL = Mi_SQL + "SYSDATE)";

                //Ejecutar consulta
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery(); // Se ejecuta la operación

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
                    Mensaje = "Error al intentar dar de Alta el Cemoviente. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
            return Cemoviente;
        }
         ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Empleados_Resguardos
        ///DESCRIPCIÓN          : Obtiene empleados de la Base de Datos y los regresa en un 
        ///                       DataTable de acuerdo a los filtros pasados.
        ///PARAMETROS           : Parametros.  Contiene los parametros que se van a utilizar para
        ///                                         hacer la consulta de la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 24/Octubre/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static DataTable Consultar_Empleados_Resguardos(Cls_Ope_Pat_Com_Cemovientes_Negocio Parametros) {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            try {
                Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                Mi_SQL = Mi_SQL + ", TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS NOMBRE";
                Mi_SQL = Mi_SQL + ", TRIM(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS DEPENDENCIA";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = Mi_SQL + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + " = 'ACTIVO'";
                if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID.Trim() + "'";
                }
                if (Parametros.P_RFC_Resguardante != null && Parametros.P_RFC_Resguardante.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + " = '" + Parametros.P_RFC_Resguardante.Trim() + "'";
                }
                if (Parametros.P_No_Empleado_Resguardante != null && Parametros.P_No_Empleado_Resguardante.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Parametros.P_No_Empleado_Resguardante.Trim()), 6) + "'";
                }
                if (Parametros.P_Nombre_Resguardante != null && Parametros.P_Nombre_Resguardante.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND (TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") LIKE '%" + Parametros.P_Nombre_Resguardante.Trim() + "%'";
                    Mi_SQL = Mi_SQL + " OR TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") LIKE '%" + Parametros.P_Nombre_Resguardante.Trim() + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY NOMBRE";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null) {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Datos;
        }

    }
}