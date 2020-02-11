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
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Polizas.Negocios;
using Presidencia.Sessiones;


namespace Presidencia.Stock
{    
    public class Cls_Ope_Alm_Stock
    {
        public static String COMPROMETER_PRODUCTO = "COMPROMETIDO";
        public static String DESCOMPROMETER_PRODUCTO = "DESCOMPROMETIDO";
        public static String SALIDA_PRODUCTO = "SALIDA";
        public static String ENTRADA_PRODUCTO = "ENTRADA";
 
        public Cls_Ope_Alm_Stock(){ }
    #region MÉTODOS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Comprometer_Producto
        ///DESCRIPCIÓN: Compromete un producto de stock
        ///PARAMETROS: 1.-Producto_ID
        ///            2.-Cantidad
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Comprometer_Producto(String Producto_ID, int Cantidad)
        {
            String Mi_SQL = "";
            int Registros = 0;
            try
            {
                //SENTENCIA SQL PARA COMPROMETER
                Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " SET " +
                    Cat_Com_Productos.Campo_Comprometido + " = " + Cat_Com_Productos.Campo_Comprometido + " + " + Cantidad + "," +
                    Cat_Com_Productos.Campo_Disponible + " = " + Cat_Com_Productos.Campo_Disponible + " - " + Cantidad +
                    " WHERE " +
                    Cat_Com_Productos.Campo_Producto_ID + " = '" + Producto_ID + "'" +
                    " AND " + Cat_Com_Productos.Campo_Disponible + " >= " + Cantidad;
                Registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                Registros = 0;
                throw new Exception(Ex.ToString());
            }
            return Registros;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Comprometer_Producto
        ///DESCRIPCIÓN: Compromete varios productos de stock
        ///PARAMETROS: 1.-DataTable con los productos a comprometer
        ///            2.-Columna que contiene ID de los productos
        ///            3.-Columna que contiene la Cantidad de producto con la que se realizará
        ///               la operación
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Comprometer_Producto(DataTable Dt_Productos, String Nombre_Columna_ID, String Nombre_Columna_Cantidad)
        {
            String Mi_SQL = "";
            int Registros = 0;
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
                foreach (DataRow Dr_Producto in Dt_Productos.Rows)
                {
                    //SENTENCIA SQL PARA COMPROMETER
                    Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                        " SET " +
                        Cat_Com_Productos.Campo_Comprometido + " = " + Cat_Com_Productos.Campo_Comprometido + " + " +
                        Dr_Producto[Nombre_Columna_Cantidad].ToString() + "," +
                        Cat_Com_Productos.Campo_Disponible + " = " + Cat_Com_Productos.Campo_Disponible + " - " +
                        Dr_Producto[Nombre_Columna_Cantidad].ToString() +
                        " WHERE " +
                        Cat_Com_Productos.Campo_Producto_ID + " = '" + Dr_Producto[Nombre_Columna_ID].ToString() + "'" +
                        " AND " + Cat_Com_Productos.Campo_Disponible + " >= " + Dr_Producto[Nombre_Columna_Cantidad].ToString();
                    Cmd.CommandText = Mi_SQL;
                    Registros = Cmd.ExecuteNonQuery();
                    if (Registros == 0)
                    {
                        break;
                    }
                }
                //validar que todos los 
                if (Registros > 0)
                {
                    Trans.Commit();
                }
                else 
                {
                    Trans.Rollback();                                           
                }
            }
            catch (Exception Ex)
            {
                Trans.Rollback();                                              
                throw new Exception(Ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
            return Registros;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Desomprometer_Producto
        ///DESCRIPCIÓN: Descompromete un producto de stock
        ///PARAMETROS: 1.-Producto_ID
        ///            2.-Cantidad
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Descomprometer_Producto(String Producto_ID, int Cantidad)
        {
            String Mi_SQL = "";
            int Registros = 0;
            try
            {
                //SENTENCIA SQL PARA DESCOMPROMETER
                Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " SET " +
                    Cat_Com_Productos.Campo_Comprometido + " = " + Cat_Com_Productos.Campo_Comprometido + " - " + Cantidad + "," +
                    Cat_Com_Productos.Campo_Disponible + " = " + Cat_Com_Productos.Campo_Disponible + " + " + Cantidad +
                    " WHERE " +
                    Cat_Com_Productos.Campo_Producto_ID + " = '" + Producto_ID + "'" +
                    " AND " + Cat_Com_Productos.Campo_Comprometido + " >= " + Cantidad;
                Registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                Registros = 0;
                throw new Exception(Ex.ToString());
            }
            return Registros;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Descomprometer_Producto
        ///DESCRIPCIÓN: Descompromete varios productos de stock
        ///PARAMETROS: 1.-DataTable con los productos a comprometer
        ///            2.-Columna que contiene ID de los productos
        ///            3.-Columna que contiene la Cantidad de producto con la que se realizará
        ///               la operación
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Descomprometer_Producto(DataTable Dt_Productos, String Nombre_Columna_ID, String Nombre_Columna_Cantidad)
        {
            String Mi_SQL = "";
            int Registros = 0;
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
                foreach (DataRow Dr_Producto in Dt_Productos.Rows)
                {
                    //SENTENCIA SQL PARA DESCOMPROMETER
                    Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                        " SET " +
                        Cat_Com_Productos.Campo_Comprometido + " = " + Cat_Com_Productos.Campo_Comprometido + " - " +
                        Dr_Producto[Nombre_Columna_Cantidad].ToString() + "," +
                        Cat_Com_Productos.Campo_Disponible + " = " + Cat_Com_Productos.Campo_Disponible + " + " +
                        Dr_Producto[Nombre_Columna_Cantidad].ToString() +
                        " WHERE " +
                        Cat_Com_Productos.Campo_Producto_ID + " = '" + Dr_Producto[Nombre_Columna_ID].ToString() + "'" +
                        " AND " + Cat_Com_Productos.Campo_Comprometido + " >= " + Dr_Producto[Nombre_Columna_Cantidad].ToString();
                    Cmd.CommandText = Mi_SQL;
                    Registros = Cmd.ExecuteNonQuery();
                    if (Registros == 0)
                    {
                        break;
                    }
                }
                //validar que todos los 
                if (Registros > 0)
                {
                    Trans.Commit();
                }
                else
                {
                    Trans.Rollback();
                }            
            }
            catch (Exception Ex)
            {
                Trans.Rollback();
                throw new Exception(Ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
            return Registros;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Entrada_Producto
        ///DESCRIPCIÓN: Da entrada de existencia a un solo producto
        ///PARAMETROS: 1.-Producto_ID
        ///            2.-Cantidad
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Entrada_Producto(String Producto_ID, int Cantidad)
        {
            String Mi_SQL = "";
            int Registros = 0;
            try
            {
                //SENTENCIA SQL PARA ENTRADA
                Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " SET " +
                    Cat_Com_Productos.Campo_Existencia + " = " + Cat_Com_Productos.Campo_Existencia + " + " + Cantidad + "," +
                    Cat_Com_Productos.Campo_Disponible + " = " + Cat_Com_Productos.Campo_Disponible + " + " + Cantidad +
                    " WHERE " +
                    Cat_Com_Productos.Campo_Producto_ID + " = '" + Producto_ID + "'";
                Registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                Registros = 0;
                throw new Exception(Ex.ToString());
            }
            return Registros;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Entrada_Producto
        ///DESCRIPCIÓN: Da entrada a varios productos de stock
        ///PARAMETROS: 1.-DataTable con los productos 
        ///            2.-Columna que contiene ID de los productos
        ///            3.-Columna que contiene la Cantidad de producto con la que se realizará
        ///               la operación
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Entrada_Producto(DataTable Dt_Productos, String Nombre_Columna_ID, String Nombre_Columna_Cantidad)
        {
            String Mi_SQL = "";
            int Registros = 0;
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
                foreach (DataRow Dr_Producto in Dt_Productos.Rows)
                {
                    //SENTENCIA SQL PARA DAR ENTRADAS
                    Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                        " SET " +
                        Cat_Com_Productos.Campo_Comprometido + " = " + Cat_Com_Productos.Campo_Existencia + " + " +
                        Dr_Producto[Nombre_Columna_Cantidad].ToString() + "," +
                        Cat_Com_Productos.Campo_Disponible + " = " + Cat_Com_Productos.Campo_Disponible + " + " +
                        Dr_Producto[Nombre_Columna_Cantidad].ToString() +
                        " WHERE " +
                        Cat_Com_Productos.Campo_Producto_ID + " = '" + Dr_Producto[Nombre_Columna_ID].ToString() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Registros = Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
            }
            catch (Exception Ex)
            {
                Trans.Rollback();
                throw new Exception(Ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
            return Registros;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Salida_Producto
        ///DESCRIPCIÓN: Da salida a un solo producto
        ///PARAMETROS: 1.-Producto_ID
        ///            2.-Cantidad
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Salida_Producto(String Producto_ID, int Cantidad)
        {
            String Mi_SQL = "";
            int Registros = 0;
            try
            {
                //SENTENCIA SQL PARA SALIDA
                Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " SET " +
                    Cat_Com_Productos.Campo_Existencia + " = " + Cat_Com_Productos.Campo_Existencia + " - " + Cantidad + "," +
                    Cat_Com_Productos.Campo_Disponible + " = " + Cat_Com_Productos.Campo_Disponible + " - " + Cantidad +
                    " WHERE " +
                    Cat_Com_Productos.Campo_Producto_ID + " = '" + Producto_ID + "'";
                Registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                Registros = 0;
                throw new Exception(Ex.ToString());
            }
            return Registros;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Salida_Producto
        ///DESCRIPCIÓN: Da salida a varios productos de stock
        ///PARAMETROS: 1.-DataTable con los productos 
        ///            2.-Columna que contiene ID de los productos
        ///            3.-Columna que contiene la Cantidad de producto con la que se realizará
        ///               la operación
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Salida_Producto(DataTable Dt_Productos, String Nombre_Columna_ID, String Nombre_Columna_Cantidad)
        {
            String Mi_SQL = "";
            int Registros = 0;
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
                foreach (DataRow Dr_Producto in Dt_Productos.Rows)
                {
                    //SENTENCIA SQL PARA DAR ENTRADAS
                    Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                        " SET " +
                        Cat_Com_Productos.Campo_Comprometido + " = " + Cat_Com_Productos.Campo_Existencia + " - " +
                        Dr_Producto[Nombre_Columna_Cantidad].ToString() + "," +
                        Cat_Com_Productos.Campo_Disponible + " = " + Cat_Com_Productos.Campo_Disponible + " - " +
                        Dr_Producto[Nombre_Columna_Cantidad].ToString() +
                        " WHERE " +
                        Cat_Com_Productos.Campo_Producto_ID + " = '" + Dr_Producto[Nombre_Columna_ID].ToString() + "'";
                    Cmd.CommandText = Mi_SQL;
                    Registros = Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
            }
            catch (Exception Ex)
            {
                Trans.Rollback();
                throw new Exception(Ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
            return Registros;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_EX_DISP_CMP
        ///DESCRIPCIÓN: Consulta existencia, disponible y comprometido de un producto
        /// Devuelve DataTable
        ///PARAMETROS: 1.-Producto_ID
        ///            2.-Cantidad
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_EX_DISP_CMP(String Producto_ID)
        {
            String Mi_SQL = "";
            DataTable _DataTable = null;
            try
            {
                //SENTENCIA SQL PARA consultar
                Mi_SQL = "SELECT " + 
                    Cat_Com_Productos.Campo_Existencia + ", " +
                    Cat_Com_Productos.Campo_Disponible + ", " +
                    Cat_Com_Productos.Campo_Comprometido + " FROM " +
                    Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " WHERE " +
                    Cat_Com_Productos.Campo_Producto_ID + " = '" + Producto_ID + "'";
                _DataTable = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                _DataTable = null;
                throw new Exception(Ex.ToString());
            }
            return _DataTable;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_EX_DISP_CMP_Arreglo
        ///DESCRIPCIÓN: Consulta existencia, disponible y comprometido de un producto
        /// Devuelve DataTable
        ///PARAMETROS: 1.-Producto_ID
        ///            2.-Cantidad
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int [] Consultar_EX_DISP_CMP_Arreglo(String Producto_ID)
        {
            String Mi_SQL = "";
            int[] Arreglo = new int[3];
            try
            {
                //SENTENCIA SQL PARA consultar
                Mi_SQL = "SELECT " +
                    Cat_Com_Productos.Campo_Existencia + ", " +
                    Cat_Com_Productos.Campo_Disponible + ", " +
                    Cat_Com_Productos.Campo_Comprometido + " FROM " +
                    Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " WHERE " +
                    Cat_Com_Productos.Campo_Producto_ID + " = '" + Producto_ID + "'";
                DataTable _DataTable = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                Arreglo[0] = int.Parse(_DataTable.Rows[0]["EXISTENCIA"].ToString());
                Arreglo[1] = int.Parse(_DataTable.Rows[0]["DISPONIBLE"].ToString());
                Arreglo[2] = int.Parse(_DataTable.Rows[0]["COMPROMETIDO"].ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Arreglo;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Existencia
        ///DESCRIPCIÓN: Consulta existencia
        ///PARAMETROS: 1.-Producto_ID
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Consultar_Existencia(String Producto_ID)
        {
            String Mi_SQL = "";
            int Numero = 0;
            try
            {
                //SENTENCIA SQL PARA CONSULTAR
                Mi_SQL = "SELECT " +
                    Cat_Com_Productos.Campo_Existencia + " FROM " +
                    Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " WHERE " +
                    Cat_Com_Productos.Campo_Producto_ID + " = '" + Producto_ID + "'";
                Object _Object = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Numero = int.Parse(_Object.ToString());
            }
            catch (Exception Ex)
            {                
                throw new Exception(Ex.ToString());
            }
            return Numero;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Disponible
        ///DESCRIPCIÓN: Consulta disponible de un producto
        ///PARAMETROS: 1.-Producto_ID
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Consultar_Disponible(String Producto_ID)
        {
            String Mi_SQL = "";
            int Numero = 0;
            try
            {
                //SENTENCIA SQL PARA CONSULTAR
                Mi_SQL = "SELECT " +
                    Cat_Com_Productos.Campo_Disponible + " FROM " +
                    Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " WHERE " +
                    Cat_Com_Productos.Campo_Producto_ID + " = '" + Producto_ID + "'";
                Object _Object = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Numero = int.Parse(_Object.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Numero;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Comprometido
        ///DESCRIPCIÓN: Consulta Comprometido de un producto
        ///PARAMETROS: 1.-Producto_ID
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 08/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Consultar_Comprometido(String Producto_ID)
        {
            String Mi_SQL = "";
            int Numero = 0;
            try
            {
                //SENTENCIA SQL PARA CONSULTAR
                Mi_SQL = "SELECT " +
                    Cat_Com_Productos.Campo_Comprometido + " FROM " +
                    Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " WHERE " +
                    Cat_Com_Productos.Campo_Producto_ID + " = '" + Producto_ID + "'";
                Object _Object = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Numero = int.Parse(_Object.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Numero;
        }




        #endregion


        ///*******************************************************************************
        ///*******************************************************************************
        #region POLIZAS


        public static bool Crear_Poliza_Compra_Stock( String No_Requisicion )
        {
            DataTable Dt_Poliza = Consultar_Datos_Para_Poliza(No_Requisicion);
            bool Respuesta = true;
            if (Dt_Poliza != null)
            {
                double Importe = double.Parse(Dt_Poliza.Rows[0]["TOTAL"].ToString());
                String Partida_ID = Dt_Poliza.Rows[0]["PARTIDA_ID"].ToString();
                String Cuenta_ID_Cargo = Dt_Poliza.Rows[0]["CUENTA_CONTABLE_ID"].ToString();
                String Cuenta_Cargo = Dt_Poliza.Rows[0]["CUENTA"].ToString();
                String Codigo_Programatico = Dt_Poliza.Rows[0]["CODIGO_PROGRAMATICO"].ToString();
                String Cuenta_ID_Abono = "00281";
                String Cuenta_Abono = "115110001";
                Cls_Ope_Con_Polizas_Negocio Rs_Alta_Ope_Con_Polizas = new Cls_Ope_Con_Polizas_Negocio();
                Rs_Alta_Ope_Con_Polizas.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                Rs_Alta_Ope_Con_Polizas = new Cls_Ope_Con_Polizas_Negocio();
                Rs_Alta_Ope_Con_Polizas.P_Tipo_Poliza_ID = "00002";//COMPRA
                Rs_Alta_Ope_Con_Polizas.P_Mes_Ano = DateTime.Now.ToString("MMyy");
                Rs_Alta_Ope_Con_Polizas.P_Fecha_Poliza = DateTime.Now;
                Rs_Alta_Ope_Con_Polizas.P_Concepto = "REQUISICION STOCK: " + No_Requisicion;
                Rs_Alta_Ope_Con_Polizas.P_Total_Debe = Importe;
                Rs_Alta_Ope_Con_Polizas.P_Total_Haber = Importe;
                Rs_Alta_Ope_Con_Polizas.P_No_Partida = 2;
                Rs_Alta_Ope_Con_Polizas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                Rs_Alta_Ope_Con_Polizas.P_Empleado_ID_Creo = Cls_Sessiones.Empleado_ID;
                //SE CREAN LOS DETALLES
                DataTable Dt_Partidas_Polizas = Crear_Tabla_Detalles_Poliza();
                //Renglon de DEBE
                DataRow Dr_Partida = Dt_Partidas_Polizas.NewRow();
                Dr_Partida["PARTIDA_ID"] = Partida_ID;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Partida] = Dt_Partidas_Polizas.Rows.Count + 1;
                Dr_Partida["CODIGO_PROGRAMATICO"] = Codigo_Programatico;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Cuenta_ID_Cargo;
                Dr_Partida[Cat_Con_Cuentas_Contables.Campo_Cuenta] = Cuenta_Cargo;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Concepto] = "STOCK";
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Debe] = Importe;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Haber] = 0.0;
                //Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Compromiso_ID] = "00001";
                Dt_Partidas_Polizas.Rows.Add(Dr_Partida);
                Dt_Partidas_Polizas.AcceptChanges();
                //Renglon HABER
                Dr_Partida = Dt_Partidas_Polizas.NewRow();
                Dr_Partida["PARTIDA_ID"] = "";
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Partida] = Dt_Partidas_Polizas.Rows.Count + 1;
                Dr_Partida["CODIGO_PROGRAMATICO"] = "";
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Cuenta_ID_Abono;
                Dr_Partida[Cat_Con_Cuentas_Contables.Campo_Cuenta] = Cuenta_Abono;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Concepto] = "STOCK";// No_Requisicion;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Debe] = 0.0;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Haber] = Importe;
                //Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Compromiso_ID] = "00001";
                Dt_Partidas_Polizas.Rows.Add(Dr_Partida);
                Dt_Partidas_Polizas.AcceptChanges();    
            //Guardar Poliza
                Rs_Alta_Ope_Con_Polizas.P_Dt_Detalles_Polizas = Dt_Partidas_Polizas;
                string[] Datos_Poliza = Rs_Alta_Ope_Con_Polizas.Alta_Poliza();
            }
            else
            {
                Respuesta = false;
            }
            return Respuesta;
        }


        public static bool Crear_Poliza_Compra_Transitoria(String No_Requisicion)
        {
            DataTable Dt_Poliza = Consultar_Datos_Para_Poliza(No_Requisicion);
            bool Respuesta = true;
            if (Dt_Poliza != null)
            {
                double Importe = double.Parse(Dt_Poliza.Rows[0]["TOTAL"].ToString());
                String Partida_ID = Dt_Poliza.Rows[0]["PARTIDA_ID"].ToString();
                String Cuenta_ID_Cargo = Dt_Poliza.Rows[0]["CUENTA_CONTABLE_ID"].ToString();
                String Cuenta_Cargo = Dt_Poliza.Rows[0]["CUENTA"].ToString();
                String Codigo_Programatico = Dt_Poliza.Rows[0]["CODIGO_PROGRAMATICO"].ToString();
                String No_Orden_Compra = Dt_Poliza.Rows[0]["NO_ORDEN_COMPRA"].ToString();
                String Cuenta_ID_Abono = "00281";
                String Cuenta_Abono = "215110012";
                Cls_Ope_Con_Polizas_Negocio Rs_Alta_Ope_Con_Polizas = new Cls_Ope_Con_Polizas_Negocio();
                Rs_Alta_Ope_Con_Polizas.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                Rs_Alta_Ope_Con_Polizas = new Cls_Ope_Con_Polizas_Negocio();
                Rs_Alta_Ope_Con_Polizas.P_Tipo_Poliza_ID = "00002";//COMPRA
                Rs_Alta_Ope_Con_Polizas.P_Mes_Ano = DateTime.Now.ToString("MMyy");
                Rs_Alta_Ope_Con_Polizas.P_Fecha_Poliza = DateTime.Now;
                Rs_Alta_Ope_Con_Polizas.P_Concepto = "ORDEN COMPRA: " + No_Orden_Compra;
                Rs_Alta_Ope_Con_Polizas.P_Total_Debe = Importe;
                Rs_Alta_Ope_Con_Polizas.P_Total_Haber = Importe;
                Rs_Alta_Ope_Con_Polizas.P_No_Partida = 2;
                Rs_Alta_Ope_Con_Polizas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                Rs_Alta_Ope_Con_Polizas.P_Empleado_ID_Creo = Cls_Sessiones.Empleado_ID;
                //SE CREAN LOS DETALLES
                DataTable Dt_Partidas_Polizas = Crear_Tabla_Detalles_Poliza();
                //Renglon de DEBE
                DataRow Dr_Partida = Dt_Partidas_Polizas.NewRow();
                Dr_Partida["PARTIDA_ID"] = Partida_ID;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Partida] = Dt_Partidas_Polizas.Rows.Count + 1;
                Dr_Partida["CODIGO_PROGRAMATICO"] = Codigo_Programatico;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Cuenta_ID_Cargo;
                Dr_Partida[Cat_Con_Cuentas_Contables.Campo_Cuenta] = Cuenta_Cargo;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Concepto] = "COMPRA";
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Debe] = Importe;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Haber] = 0.0;
                //Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Compromiso_ID] = "00001";
                Dt_Partidas_Polizas.Rows.Add(Dr_Partida);
                Dt_Partidas_Polizas.AcceptChanges();
                //Renglon HABER
                Dr_Partida = Dt_Partidas_Polizas.NewRow();
                Dr_Partida["PARTIDA_ID"] = "";
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Partida] = Dt_Partidas_Polizas.Rows.Count + 1;
                Dr_Partida["CODIGO_PROGRAMATICO"] = "";
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Cuenta_ID_Abono;
                Dr_Partida[Cat_Con_Cuentas_Contables.Campo_Cuenta] = Cuenta_Abono;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Concepto] = "COMPRA";// No_Requisicion;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Debe] = 0.0;
                Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Haber] = Importe;
                //Dr_Partida[Ope_Con_Polizas_Detalles.Campo_Compromiso_ID] = "00001";
                Dt_Partidas_Polizas.Rows.Add(Dr_Partida);
                Dt_Partidas_Polizas.AcceptChanges();
                //Guardar Poliza
                Rs_Alta_Ope_Con_Polizas.P_Dt_Detalles_Polizas = Dt_Partidas_Polizas;
                string[] Datos_Poliza = Rs_Alta_Ope_Con_Polizas.Alta_Poliza();
            }
            else
            {
                Respuesta = false;
            }
            return Respuesta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Para_Poliza
        ///DESCRIPCIÓN: Consulta Datos_Para_Poliza
        ///PARAMETROS: 1.-requisicion
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Nov/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static DataTable Consultar_Datos_Para_Poliza(String No_Requisicion)
        {
            String Mi_SQL = "";
            DataTable Dt_Datos = null;
            try
            {
                //SENTENCIA SQL PARA CONSULTAR
                Mi_SQL =
                "SELECT RQ.NO_REQUISICION, RQ.FOLIO, RQ.TOTAL,RQ.CODIGO_PROGRAMATICO, RQ.NO_ORDEN_COMPRA, RQ.PARTIDA_ID, PARTIDAS.CUENTA_CONTABLE_ID, " +
                "PARTIDAS.CUENTA FROM OPE_COM_REQUISICIONES RQ JOIN CAT_SAP_PARTIDAS_ESPECIFICAS PARTIDAS " +
                "ON RQ.PARTIDA_ID = PARTIDAS.PARTIDA_ID " +
                "WHERE RQ.NO_REQUISICION = " + No_Requisicion;
                Dt_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                Ex.ToString();
                Dt_Datos = null;
                throw new Exception(Ex.ToString() + ":DATOS DE POLIZA INCORRECTOS");
            }
            return Dt_Datos;
        }
        private static DataTable Crear_Tabla_Detalles_Poliza()
        {
            DataTable Dt_Partidas_Polizas = new DataTable();
            //Agrega los campos que va a contener el DataTable
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Partida, typeof(System.Int32));
            Dt_Partidas_Polizas.Columns.Add("CODIGO_PROGRAMATICO", typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add("DEPENDENCIA_ID", typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add("FUENTE_FINANCIAMIENTO_ID", typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add("AREA_FUNCIONAL_ID", typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add("PROYECTO_PROGRAMA_ID", typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add("PARTIDA_ID", typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add("COMPROMISO_ID", typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID, typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add(Cat_Con_Cuentas_Contables.Campo_Cuenta, typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Concepto, typeof(System.String));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Debe, typeof(System.Double));
            Dt_Partidas_Polizas.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Haber, typeof(System.Double));
            return Dt_Partidas_Polizas;
        }
        #endregion
    }

}
