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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Predial_Convenios_Derechos_Supervision.Negocio;
using Presidencia.Operacion_Predial_Convenios_Fraccionamientos.Negocio;
using Presidencia.Operacion_Predial_Convenios_Impuestos_Traslado_Dominio.Negocio;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;

namespace Operacion_Predial_Ordenes_Variacion.Datos
{

    public class Cls_Ope_Pre_Ordenes_Variacion_Datos
    {
        public Cls_Ope_Pre_Ordenes_Variacion_Datos()
        {
        }
        #region [Metodos]

        #region Consulta_Combos
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Combos
        ///DESCRIPCIÓN: consulta los tipos de movimientos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: Tres/Agosto/2011 10:25:28 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static DataSet Consulta_Combos()
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            DataSet Ds_Resultado = new DataSet();

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + " MOVS." + Cat_Pre_Movimientos.Campo_Movimiento_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + " MOVS." + Cat_Pre_Movimientos.Campo_Identificador + " ||'   -   '|| ";
                Mi_SQL = Mi_SQL + " MOVS." + Cat_Pre_Movimientos.Campo_Descripcion + " AS DESCRIPCION, ";
                Mi_SQL = Mi_SQL + " MOVS." + Cat_Pre_Movimientos.Campo_Cargar_Modulos + " ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " MOVS ";
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + "MOVS." + Cat_Pre_Movimientos.Campo_Estatus + " = 'VIGENTE'";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + "MOVS." + Cat_Pre_Movimientos.Campo_Aplica + " = 'TRASLADO'";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + "MOVS." + Cat_Pre_Movimientos.Campo_Cargar_Modulos + " is NULL ";
                Mi_SQL = Mi_SQL + " ORDER BY MOVS." + Cat_Pre_Movimientos.Campo_Identificador;
                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[0].TableName = "Dt_Tipos_Movimiento";


                //Mi_SQL = "";
                //Mi_SQL = "SELECT ";
                //Mi_SQL = Mi_SQL + " MOVS." + Cat_Pre_Movimientos.Campo_Movimiento_ID + " AS ID, ";
                //Mi_SQL = Mi_SQL + " MOVS." + Cat_Pre_Movimientos.Campo_Identificador + " ||'   -   '|| ";
                //Mi_SQL = Mi_SQL + " MOVS." + Cat_Pre_Movimientos.Campo_Descripcion + " AS DESCRIPCION ";
                //Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " MOVS ";
                //Mi_SQL = Mi_SQL + " WHERE ";
                //Mi_SQL = Mi_SQL + "MOVS." + Cat_Pre_Movimientos.Campo_Estatus + " = 'VIGENTE'";
                //Mi_SQL = Mi_SQL + " AND ";
                //Mi_SQL = Mi_SQL + "MOVS." + Cat_Pre_Movimientos.Campo_Cargar_Modulos + " is NULL ";
                //Mi_SQL = Mi_SQL + " ORDER BY MOVS." + Cat_Pre_Movimientos.Campo_Identificador;
                //Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                //Ds_Resultado.Tables[0].TableName = "Dt_Tipos_Movimiento";

                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS DESCRIPCION ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio;
                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[1].TableName = "Dt_Tipos_Predio";

                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Uso_Suelo.Campo_Identificador + " ||' '|| ";
                Mi_SQL = Mi_SQL + Cat_Pre_Uso_Suelo.Campo_Descripcion + " AS DESCRIPCION ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo;
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Pre_Uso_Suelo.Campo_Estatus + " = 'VIGENTE' OR ";
                Mi_SQL = Mi_SQL + Cat_Pre_Uso_Suelo.Campo_Estatus + " = 'ACTIVO'";
                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[2].TableName = "Dt_Usos_Predio";

                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Estados_Predio.Campo_Estado_Predio_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Estados_Predio.Campo_Descripcion + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio;
                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[3].TableName = "Dt_Estados_Predio";

                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Colonia_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Nombre + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[4].TableName = "Dt_Colonias";

                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Estados.Campo_Estado_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Estados.Campo_Nombre + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados;
                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[5].TableName = "Dt_Estados";

                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + " ANUAL.";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " AS ID, ANUAL.";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Campo_Año + ", ANUAL.";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID + ", TASA.";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial.Campo_Identificador + " || ' - ' || TASA.";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial.Campo_Descripcion + " || ' - ' || ANUAL.";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " AS DESCRIPCION ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " ANUAL JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + " TASA ON TASA.";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " = ANUAL." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID;

                Mi_SQL = Mi_SQL + " WHERE ANUAL." + Cat_Pre_Tasas_Predial_Anual.Campo_Año + " = '" + DateTime.Today.Year.ToString() + "'";

                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[6].TableName = "Dt_Tasas";


                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuotas_Minimas.Campo_Año + " || ' - ' ||";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas;
                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[7].TableName = "Dt_Cuotas_Minimas";

                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Descripcion + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Casos_Especiales.Campo_Tipo + " = 'FINANCIAMIENTO'";


                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[8].TableName = "Dt_Casos_Especiales_Financiamiento";

                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Descripcion + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Casos_Especiales.Campo_Tipo + " = 'SOLICITANTE'";

                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[9].TableName = "Dt_Casos_Especiales_Solicitante";

                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + " MOVS." + Cat_Pre_Movimientos.Campo_Movimiento_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + " MOVS." + Cat_Pre_Movimientos.Campo_Identificador + " ||'   -   '|| ";
                Mi_SQL = Mi_SQL + " MOVS." + Cat_Pre_Movimientos.Campo_Descripcion + " AS DESCRIPCION, ";
                Mi_SQL = Mi_SQL + " MOVS." + Cat_Pre_Movimientos.Campo_Cargar_Modulos + " ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " MOVS ";
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + "MOVS." + Cat_Pre_Movimientos.Campo_Estatus + " = 'VIGENTE'";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + "MOVS." + Cat_Pre_Movimientos.Campo_Aplica + " = 'PREDIAL'";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + "MOVS." + Cat_Pre_Movimientos.Campo_Cargar_Modulos + " is NULL ";
                Mi_SQL = Mi_SQL + " ORDER BY MOVS." + Cat_Pre_Movimientos.Campo_Identificador;
                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[10].TableName = "Dt_Tipos_Movimiento_Predial";

                return Ds_Resultado;
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
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Calles
        ///DESCRIPCIÓN: consulta las calles deacuerdo al ID de la colonia
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/11/2011 11:50:12 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        internal static DataTable Consulta_Calles(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Calle_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Nombre + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Calles.Campo_Colonia_ID + " = '" + Datos.P_Colonia_Cuenta + "'";

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
        internal static String Consulta_Nombre_Calle(String ID)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Calle_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Clave + " ||' '|| ";
                Mi_SQL = Mi_SQL + Cat_Pre_Calles.Campo_Nombre + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Calles.Campo_Calle_ID + " = '" + ID + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][1].ToString();

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
        internal static String Consulta_Nombre_Colonia(String ID)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Colonia_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Clave + " ||' '|| ";
                Mi_SQL = Mi_SQL + Cat_Ate_Colonias.Campo_Nombre + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + ID + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][1].ToString();

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
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Ciudades
        ///DESCRIPCIÓN: consulta las ciudades deacuerdo al ID del estadi
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/11/2011 11:50:12 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        internal static DataTable Consulta_Ciudades(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Pre_Ciudades.Campo_Ciudad_ID + " AS ID, ";
                Mi_SQL = Mi_SQL + Cat_Pre_Ciudades.Campo_Nombre + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Ciudades.Campo_Estado_ID + " = '" + Datos.P_Estado_Propietario + "'";

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

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Fundamento
        ///DESCRIPCIÓN: consulta las calles deacuerdo al ID de la colonia
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/11/2011 11:50:12 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        internal static String Consulta_Fundamento(String Caso_Especial)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT 'ARTICULO  ' || ";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Articulo + " || '  INCISO  ' ||";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Inciso + " ||' '|| ";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Descripcion + " ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = '" + Caso_Especial + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0].ToString();

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

        #endregion

        #region [Consultas Generales Orden de Variacion]
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Cuenta_Generales
        ///DESCRIPCIÓN: se consultan los datos generales de la cuenta predial
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/11/2011 11:49:25 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataSet Consulta_Datos_Cuenta_Generales(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            
            DataSet Ds_Resultado = new DataSet();
            string Propietario_ID;
            try
            {
                Mi_SQL = "";
                Mi_SQL += "SELECT TO_CHAR(CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ",'DD/Mon/yyyy') AS FECHA_AVALUO_FORMATEADA, ";
                Mi_SQL += " TO_CHAR(CUEN." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ",'DD/Mon/yyyy') AS TERMINO_EXENCION_FORMATEADA, ";
                Mi_SQL += " CUEN.*, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " AS ESTATUS_CUENTA, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ",";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ",";
                Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " AS CALLE_ID, ";
                Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Colonia_ID + ", ";
                Mi_SQL += " COL." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " AS CUOTA_MINIMA_ID, ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " AS CUOTA ";

                Mi_SQL += " FROM " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " CONT LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += " = CONT." + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID;

                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES ON CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " = CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL ON COL." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL += " = CALLES." + Cat_Pre_Calles.Campo_Colonia_ID + " LEFT OUTER JOIN " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " CUOTA ON ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " = ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + " ";

                Mi_SQL += " WHERE CONT." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Datos.P_Contrarecibo + "'";
                //Mi_SQL += " WHERE CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Datos.P_Cuenta_Predial + "'";

                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[0].TableName = "Dt_Generales";

                return Ds_Resultado;
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

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Cuenta
        ///DESCRIPCIÓN: se consultan los datos generales de la cuenta predial
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/11/2011 11:49:25 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataSet Consulta_Datos_Cuenta(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            
            DataSet Ds_Resultado = new DataSet();
            string Propietario_ID;
            try
            {
                Mi_SQL = "";
                Mi_SQL += "SELECT ";
                Mi_SQL += " CONT." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " AS CONTRARECIBO, ";
                Mi_SQL += " CONT." + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " AS ID, ";
                Mi_SQL += " CUEN.*, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " AS ESTATUS_CUENTA, ";
                Mi_SQL += " TIPO_PRE." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS TIPO_PREDIO_DESCRIPCION, ";
                Mi_SQL += " USO_PRE." + Cat_Pre_Uso_Suelo.Campo_Descripcion + " AS USO_SUELO_DESCRIPCION, ";
                Mi_SQL += " EDO_PRE." + Cat_Pre_Estados_Predio.Campo_Descripcion + " AS ESTADO_PREDIO_DESCRIPCION, ";
                Mi_SQL += " PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + ",";

                Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                Mi_SQL += " CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE_NOTIFICACION, ";
                Mi_SQL += " COL." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                Mi_SQL += " COL_NOTIFICACION." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA_NOTIFICACION, ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " AS CUOTA_MINIMA_ID, ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " AS CUOTA, ";

                Mi_SQL += " EDOS." + Cat_Pre_Estados.Campo_Nombre + " AS NOMBRE_ESTADO_CUENTA, ";
                Mi_SQL += " CD." + Cat_Pre_Ciudades.Campo_Nombre + " AS NOMBRE_CIUDAD_CUENTA ";

                Mi_SQL += " FROM " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " CONT LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += " = CONT." + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " TIPO_PRE ON TIPO_PRE." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID;
                Mi_SQL += " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo + " USO_PRE ON USO_PRE." + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID;
                Mi_SQL += " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + " EDO_PRE ON EDO_PRE." + Cat_Pre_Estados_Predio.Campo_Estado_Predio_ID;
                Mi_SQL += " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP ON PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                Mi_SQL += " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES ON CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " = CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_NOTIFICACION  ON CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Calle_ID + " = CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL ON COL." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL += " = CALLES." + Cat_Pre_Calles.Campo_Colonia_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL_NOTIFICACION  ON COL_NOTIFICACION." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL += " = CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Colonia_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " CUOTA ON ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " = ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " EDOS ON EDOS." + Cat_Pre_Estados.Campo_Estado_ID + " = CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + " CD ON CD." + Cat_Pre_Ciudades.Campo_Estado_ID + " = EDOS.";
                Mi_SQL += Cat_Pre_Estados.Campo_Estado_ID + " AND " + Cat_Pre_Ciudades.Campo_Ciudad_ID + " = CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " ";

                if (Datos.P_Contrarecibo != "" && Datos.P_Contrarecibo != null)
                {
                    Mi_SQL += " WHERE CONT." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + Validar_Operador_Comparacion(Datos.P_Contrarecibo);
                }
                else
                {
                    if (Datos.P_Cuenta_Predial != "" && Datos.P_Cuenta_Predial != null)
                    {
                        Mi_SQL += " WHERE CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Datos.P_Cuenta_Predial + "'";
                    }
                }
                if (Datos.P_Estatus_Cuenta != null)
                {
                    if (Datos.P_Agrupar_Dinamico == "True")
                    {
                        Mi_SQL += " AND (CONT." + Ope_Pre_Contrarecibos.Campo_Estatus + "='" + Datos.P_Estatus_Cuenta + "' or CONT." + Ope_Pre_Contrarecibos.Campo_Estatus + "='POR VALIDAR')";
                    }
                    else
                    {
                        Mi_SQL += " AND CONT." + Ope_Pre_Contrarecibos.Campo_Estatus + Validar_Operador_Comparacion(Datos.P_Estatus_Cuenta);
                    }

                }
                else
                {
                    Mi_SQL += " AND CONT." + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'GENERADO'";
                }
                if ((Datos.P_Contrarecibo != "" && Datos.P_Contrarecibo != null) || (Datos.P_Cuenta_Predial != "" && Datos.P_Cuenta_Predial != null))
                {
                    Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());

                    Ds_Resultado.Tables[0].TableName = "Dt_Generales";

                    if (Ds_Resultado.Tables[0].Rows.Count > 0)
                    {
                        Propietario_ID = Ds_Resultado.Tables["Dt_Generales"].Rows[0][Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString();

                        if (Propietario_ID != "" && Propietario_ID != null)
                        {
                            Mi_SQL = "";
                            Mi_SQL += "SELECT ";
                            Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Propietario_ID + " AS PROPIETARIO, ";
                            Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AS CUENTA_ID, ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_PROPIETARIO, ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario + " AS TIPO_PROPIETARIO, ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                            Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + " AS FORANEO, ";
                            Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + " AS COLONIA, ";
                            Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + " AS DOMICILIO, ";
                            Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + " AS INTERIOR, ";
                            Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + " AS EXTERIOR, ";
                            Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " AS ESTADO, ";
                            Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " AS CIUDAD, ";
                            Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + " AS CP ";
                            Mi_SQL += " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP LEFT OUTER JOIN ";
                            Mi_SQL += Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CON ON CON.";
                            Mi_SQL += Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                            Mi_SQL += " LEFT OUTER JOIN ";
                            Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN.";
                            Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;

                            Mi_SQL += " WHERE ";
                            Mi_SQL += "CON." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = " + Propietario_ID;

                            Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                            Ds_Resultado.Tables[1].TableName = "Dt_Propietarios";
                        }
                        else
                        {
                            Ds_Resultado.Tables.Add();
                            Ds_Resultado.Tables[1].TableName = "Dt_Propietarios";
                        }

                    }
                }
                else
                    throw new Exception("No se obtuvieron campos para especificar la búsqueda");
                return Ds_Resultado;
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
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Cuenta_Datos
        ///DESCRIPCIÓN: se consultan los datos generales de la cuenta predial
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/11/2011 11:49:25 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static DataSet Consulta_Datos_Cuenta_Datos(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {

            {
                String Mi_SQL = ""; //Variable para la consulta SQL            
                DataSet Ds_Resultado = new DataSet();
                string Propietario_ID;
                try
                {
                    Mi_SQL = "";
                    Mi_SQL += "SELECT ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AS ID, ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " AS ESTATUS_CUENTA, ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ", ";
                    Mi_SQL += " TO_CHAR(CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ",'DD/Mon/yyyy') AS FECHA_AVALUO_FORMATEADA, ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                    Mi_SQL += " TO_CHAR(CUEN." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ",'DD/Mon/yyyy') AS TERMINO_EXENCION_FORMATEADA, ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Costo_m2 + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Usuario_Creo + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Usuario_Modifico + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Modifico + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Creo + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ", ";
                    Mi_SQL += " PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + ",";
                    Mi_SQL += " PROP." + Cat_Pre_Propietarios.Campo_Tipo + ",";
                    Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " AS CALLE_ID, ";
                    Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                    Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Colonia_ID + ", ";
                    Mi_SQL += " COL." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                    Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " AS CUOTA_MINIMA_ID, ";
                    Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " AS CUOTA, ";

                    Mi_SQL += " USO." + Cat_Pre_Uso_Suelo.Campo_Descripcion + " AS USO_SUELO, ";
                    Mi_SQL += " TIPO." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS TIPO_PREDIO, ";
                    Mi_SQL += " ESTADO." + Cat_Pre_Estados_Predio.Campo_Descripcion + " AS ESTADO_PREDIO ";

                    Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ";
                    Mi_SQL += " LEFT OUTER JOIN ";
                    Mi_SQL += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP ON PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " LEFT OUTER JOIN ";
                    Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES ON CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " = CUEN.";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL ON COL." + Cat_Ate_Colonias.Campo_Colonia_ID;
                    Mi_SQL += " = CALLES." + Cat_Pre_Calles.Campo_Colonia_ID + " LEFT OUTER JOIN " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " CUOTA ON ";
                    Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " = ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + " ";
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo + " USO ON USO." + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " TIPO ON TIPO." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + " ESTADO ON ESTADO." + Cat_Pre_Estados_Predio.Campo_Estado_Predio_ID + " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID;
                    Mi_SQL += " WHERE (PROP." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO'";
                    Mi_SQL += " OR PROP." + Cat_Pre_Propietarios.Campo_Tipo + " = 'POSEEDOR')";
                    if (!String.IsNullOrEmpty(Datos.P_Cuenta_Predial))
                    {
                        Mi_SQL += " AND CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Datos.P_Cuenta_Predial + "'";
                    }

                    if (!String.IsNullOrEmpty(Datos.P_Filtros_Dinamicos))
                    {
                        Mi_SQL += " AND " + Datos.P_Filtros_Dinamicos;
                    }
                    Mi_SQL += " ORDER BY CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Modifico;
                    if ((Datos.P_Cuenta_Predial != "" && Datos.P_Cuenta_Predial != null) || (Datos.P_Filtros_Dinamicos != "" && Datos.P_Filtros_Dinamicos != null))
                        Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                    else
                        throw new Exception("No se obtuvieron campos para especificar la búsqueda");
                    Ds_Resultado.Tables[0].TableName = "Dt_Generales";

                    if (Ds_Resultado.Tables[0].Rows.Count == 1)
                    {
                        Propietario_ID = Ds_Resultado.Tables["Dt_Generales"].Rows[0][Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString();

                        if (Propietario_ID != "" && Propietario_ID != null)
                        {
                            Mi_SQL = "";
                            Mi_SQL += "SELECT ";
                            Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Propietario_ID + " AS PROPIETARIO, ";
                            Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " AS CONTRIBUYENTE, ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_PROPIETARIO, ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario + " AS TIPO_PROPIETARIO, ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC ";
                            //SE COMENTO DEVIDO A QUE ESTOS CAMPOS YA NO PERTENECEN A LA TABLA DE CONTRIBUYENTES
                            //Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Domicilio_Foraneo + " AS FORANEO, ";
                            //Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Colonia + " AS COLONIA, ";
                            //Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Domicilio + " AS DOMICILIO, ";
                            //Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Interior + " AS INTERIOR, ";
                            //Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Exterior + " AS EXTERIOR, ";
                            //Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Estado + " AS ESTADO, ";
                            //Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Ciudad + " AS CIUDAD, ";
                            //Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Codigo_Postal + " AS CP ";
                            Mi_SQL += " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP LEFT OUTER JOIN ";
                            Mi_SQL += Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CON ON CON.";
                            Mi_SQL += Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;

                            Mi_SQL += " WHERE ";
                            Mi_SQL += "CON." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = " + Propietario_ID;

                            Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                            Ds_Resultado.Tables[1].TableName = "Dt_Propietarios";
                        }
                        else
                        {
                            Ds_Resultado.Tables.Add();
                            Ds_Resultado.Tables[1].TableName = "Dt_Propietarios";
                        }

                    }

                    return Ds_Resultado;

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
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Cuenta
        ///DESCRIPCIÓN: se consultan los datos generales de la cuenta predial
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/11/2011 11:49:25 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataSet Consulta_Datos_Cuenta_Sin_Contrarecibo(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            
            DataSet Ds_Resultado = new DataSet();
            string Propietario_ID;
            try
            {
                Mi_SQL = "";
                Mi_SQL += "SELECT ";
                Mi_SQL += " TO_CHAR(CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ",'DD/Mon/yyyy') AS FECHA_AVALUO_FORMATEADA, ";
                Mi_SQL += " TO_CHAR(CUEN." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ",'DD/Mon/yyyy') AS TERMINO_EXENCION_FORMATEADA, ";
                Mi_SQL += " CUEN.*, ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " AS ESTATUS_CUENTA, ";
                Mi_SQL += " TIPO_PRE." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS TIPO_PREDIO_DESCRIPCION, ";
                Mi_SQL += " USO_PRE." + Cat_Pre_Uso_Suelo.Campo_Descripcion + " AS USO_SUELO_DESCRIPCION, ";
                Mi_SQL += " EDO_PRE." + Cat_Pre_Estados_Predio.Campo_Descripcion + " AS ESTADO_PREDIO_DESCRIPCION, ";
                Mi_SQL += " PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + ",";

                Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                Mi_SQL += " CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE_NOTIFICACION, ";
                Mi_SQL += " COL." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                Mi_SQL += " COL_NOTIFICACION." + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA_NOTIFICACION, ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " AS CUOTA_MINIMA_ID, ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " AS CUOTA, ";

                Mi_SQL += " EDOS." + Cat_Pre_Estados.Campo_Nombre + " AS NOMBRE_ESTADO_CUENTA, ";
                Mi_SQL += " CD." + Cat_Pre_Ciudades.Campo_Nombre + " AS NOMBRE_CIUDAD_CUENTA ";

                Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP ON PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                Mi_SQL += " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " TIPO_PRE ON TIPO_PRE." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID;
                Mi_SQL += " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo + " USO_PRE ON USO_PRE." + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID;
                Mi_SQL += " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + " EDO_PRE ON EDO_PRE." + Cat_Pre_Estados_Predio.Campo_Estado_Predio_ID;
                Mi_SQL += " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES ON CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " = CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES_NOTIFICACION ON CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Calle_ID + " = CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL ON COL." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL += " = CALLES." + Cat_Pre_Calles.Campo_Colonia_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL_NOTIFICACION  ON COL_NOTIFICACION ." + Cat_Ate_Colonias.Campo_Colonia_ID;
                Mi_SQL += " = CALLES_NOTIFICACION." + Cat_Pre_Calles.Campo_Colonia_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " CUOTA ON ";
                Mi_SQL += " CUOTA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + " = ";
                Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " EDOS ON EDOS." + Cat_Pre_Estados.Campo_Estado_ID + " = CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + " CD ON CD." + Cat_Pre_Ciudades.Campo_Estado_ID + " = EDOS.";
                Mi_SQL += Cat_Pre_Estados.Campo_Estado_ID + " AND " + Cat_Pre_Ciudades.Campo_Ciudad_ID + " = CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " ";

                if (Datos.P_Cuenta_Predial != "" && Datos.P_Cuenta_Predial != null)
                {
                    Mi_SQL += " WHERE CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Datos.P_Cuenta_Predial + "'";
                }
                else
                {
                    if (Datos.P_Cuenta_Predial_ID != "" && Datos.P_Cuenta_Predial_ID != null)
                    {
                        Mi_SQL += " WHERE CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                    }
                }

                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());

                Ds_Resultado.Tables[0].TableName = "Dt_Generales";

                if (Ds_Resultado.Tables[0].Rows.Count > 0)
                {
                    Propietario_ID = Ds_Resultado.Tables["Dt_Generales"].Rows[0][Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString();

                    if (Propietario_ID != "" && Propietario_ID != null)
                    {
                        Mi_SQL = "";
                        Mi_SQL += "SELECT ";
                        Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Propietario_ID + " AS PROPIETARIO, ";
                        Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AS CUENTA_ID, ";
                        Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                        Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| ";
                        Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_PROPIETARIO, ";
                        Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario + " AS TIPO_PROPIETARIO, ";
                        Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                        Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + " AS FORANEO, ";
                        Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + " AS COLONIA, ";
                        Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + " AS DOMICILIO, ";
                        Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + " AS INTERIOR, ";
                        Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + " AS EXTERIOR, ";
                        Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " AS ESTADO, ";
                        Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " AS CIUDAD, ";
                        Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + " AS CP ";
                        Mi_SQL += " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP LEFT OUTER JOIN ";
                        Mi_SQL += Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CON ON CON.";
                        Mi_SQL += Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                        Mi_SQL += " LEFT OUTER JOIN ";
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN.";
                        Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;

                        Mi_SQL += " WHERE ";
                        Mi_SQL += "CON." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = " + Propietario_ID;

                        Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                        Ds_Resultado.Tables[1].TableName = "Dt_Propietarios";
                    }
                    else
                    {
                        Ds_Resultado.Tables.Add();
                        Ds_Resultado.Tables[1].TableName = "Dt_Propietarios";
                    }
                }
                return Ds_Resultado;
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
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Valor_Orden
        ///DESCRIPCIÓN: Consulta un Valor en la Orden
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 22/Ago/2011 8:03:39 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        public static DataTable Consulta_Valor_Orden(String Campo_Consultar, String Cuenta_ID, String Anio)
        {
            //select d.campo,d.dato_nuevo,o.anio,O.NO_ORDEN_VARIACION from ope_pre_orden_detalles d join ope_pre_orden_variacion o
            //on o.NO_ORDEN_VARIACION = d.NO_ORDEN_VARIACION where d.campo = 'TASA_ID' AND o.anio <=2015 and CUENTA_PREDIAL_ID = '0000000008' 
            //ORDER BY O.ANIO DESC, O.NO_ORDEN_VARIACION DESC 

            String Mi_SQL = ""; //Variable para la consulta SQL
            DataTable Dt_Resultado;
            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL += " o." + Campo_Consultar;
                Mi_SQL += " ,o." + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                Mi_SQL += " ,o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " o ";
                Mi_SQL += " WHERE ";
                Mi_SQL += " o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID;
                Mi_SQL += " = '";
                Mi_SQL += Cuenta_ID + "'";
                if (!String.IsNullOrEmpty(Anio))
                {
                    Mi_SQL += " AND ";
                    Mi_SQL += "o." + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                    Mi_SQL += " <= '";
                    Mi_SQL += Anio + "'";
                }
                Mi_SQL += " AND ";
                Mi_SQL += " o." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden;
                Mi_SQL += " = 'ACEPTADA'";
                Mi_SQL += " AND ";
                Mi_SQL += " o." + Campo_Consultar;
                Mi_SQL += " IS NOT NULL";
                Mi_SQL += " ORDER BY o." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " DESC, o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC ";

                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                if (Dt_Resultado.Rows.Count <= 0)
                {
                    Mi_SQL = "";
                    Mi_SQL = "SELECT ";
                    Mi_SQL += " o." + Campo_Consultar;
                    Mi_SQL += " ,o." + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                    Mi_SQL += " ,o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;
                    Mi_SQL += " FROM ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " o ";
                    Mi_SQL += " WHERE ";
                    Mi_SQL += " o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " = '";
                    Mi_SQL += Cuenta_ID + "'";
                    if (!String.IsNullOrEmpty(Anio))
                    {
                        Mi_SQL += " AND ";
                        Mi_SQL += "o." + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                        Mi_SQL += " <= '";
                        Mi_SQL += Anio + "'";
                    }
                    Mi_SQL += " AND ";
                    Mi_SQL += " o." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden;
                    Mi_SQL += " = 'ACEPTADA'";
                    Mi_SQL += " AND ";
                    Mi_SQL += " o." + Campo_Consultar;
                    Mi_SQL += " IS NOT NULL";
                    Mi_SQL += " ORDER BY o." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " ASC, o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " ASC ";
                    Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
                return Dt_Resultado;
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
        public static DataTable Consulta_Valor_Orden(String Campo_Consultar, String Cuenta_ID, String Anio, String Condicion, String Ordenamieto)
        {
            //select d.campo,d.dato_nuevo,o.anio,O.NO_ORDEN_VARIACION from ope_pre_orden_detalles d join ope_pre_orden_variacion o
            //on o.NO_ORDEN_VARIACION = d.NO_ORDEN_VARIACION where d.campo = 'TASA_ID' AND o.anio <=2015 and CUENTA_PREDIAL_ID = '0000000008' 
            //ORDER BY O.ANIO DESC, O.NO_ORDEN_VARIACION DESC 

            String Mi_SQL = ""; //Variable para la consulta SQL
            DataTable Dt_Resultado;
            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL += " " + Campo_Consultar;
                Mi_SQL += " ," + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                Mi_SQL += " ," + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ";
                Mi_SQL += " WHERE ";
                Mi_SQL += " " + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID;
                Mi_SQL += " = '";
                Mi_SQL += Cuenta_ID + "'";
                if (!String.IsNullOrEmpty(Anio))
                {
                    Mi_SQL += " AND ";
                    Mi_SQL += " " + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                    Mi_SQL += " <= '";
                    Mi_SQL += Anio + "'";
                }
                Mi_SQL += " AND ";
                Mi_SQL += " " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden;
                Mi_SQL += " = 'ACEPTADA'";
                //Mi_SQL += " AND ";
                //Mi_SQL += " " + Campo_Consultar;
                //Mi_SQL += " IS NOT NULL";
                if (!String.IsNullOrEmpty(Condicion))
                {
                    Mi_SQL += " AND ";
                    Mi_SQL += " " + Condicion;
                }
                if (!String.IsNullOrEmpty(Ordenamieto))
                {
                    Mi_SQL += " " + Ordenamieto;
                }
                if (!Mi_SQL.Contains("ORDER"))
                    Mi_SQL += " ORDER BY " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " DESC, " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC ";

                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                if (Dt_Resultado.Rows.Count <= 0)
                {
                    Mi_SQL = "";
                    Mi_SQL = "SELECT ";
                    Mi_SQL += " " + Campo_Consultar;
                    Mi_SQL += " ," + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                    Mi_SQL += " ," + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;
                    Mi_SQL += " FROM ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ";
                    Mi_SQL += " WHERE ";
                    Mi_SQL += " " + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " = '";
                    Mi_SQL += Cuenta_ID + "'";
                    if (!String.IsNullOrEmpty(Anio))
                    {
                        Mi_SQL += " AND ";
                        Mi_SQL += " " + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                        Mi_SQL += " <= '";
                        Mi_SQL += Anio + "'";
                    }
                    Mi_SQL += " AND ";
                    Mi_SQL += " " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden;
                    Mi_SQL += " = 'ACEPTADA'";
                    //Mi_SQL += " AND ";
                    //Mi_SQL += " " + Campo_Consultar;
                    //Mi_SQL += " IS NOT NULL";
                    if (!String.IsNullOrEmpty(Condicion))
                    {
                        Mi_SQL += " AND ";
                        Mi_SQL += " " + Condicion;
                    }
                    if (!String.IsNullOrEmpty(Ordenamieto))
                    {
                        Mi_SQL += " " + Ordenamieto;
                    }

                    if (!Mi_SQL.Contains("ORDER"))
                        Mi_SQL += " ORDER BY " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " ASC, " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " ASC ";
                    Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
                return Dt_Resultado;
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
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Ordenes_Variacion
        ///DESCRIPCIÓN          : Devuelve un DataTable con los registros de las Órdenes de Variación
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Agosto/2011
        ///MODIFICO:            :Jesus Toledo Rdz
        ///FECHA_MODIFICO       :01/Feb/2012
        ///CAUSA_MODIFICACIÓN   :Cambio en la estructura de la tabla
        ///*******************************************************************************
        public static DataTable Consultar_Ordenes_Variacion(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            DataTable Dt_Ordenes_Variacion = new DataTable();
            String Mi_SQL = "";
            try
            {
                if (Ordenes_Variacion.P_Maximo_Registros > 0)
                {
                    Mi_SQL += "SELECT * FROM (";
                }
                Mi_SQL += "SELECT ";
                if (Ordenes_Variacion.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS Identificador_Movimiento, ";
                    Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS Descripcion_Movimiento, ";
                    Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Cargar_Modulos + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS Cargar_Modulos, ";
                }
                if (Ordenes_Variacion.P_Campos_Dinamicos != null && Ordenes_Variacion.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Ordenes_Variacion.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Anio + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Estado_Predio_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Tasa_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Tasa_Predial_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Uso_Suelo_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuota_Minima_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Cuota_Fija + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Origen + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Clave_Catastral + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Valor_Fiscal + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Porcentaje_Exencion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Superficie_Construida + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Superficie_Total + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Termino_Exencion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Avaluo + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Calle_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Ciudad_ID_Notificacion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Ciudad_Notificacion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Estado_ID_Notificacion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Estado_Notificacion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Exterior + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Interior + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Codigo_Postal + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Efectos + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Periodo_Corriente + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Costo_M2 + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuota_Anual + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuota_Fija + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Diferencia_Construccion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Tipo_Suspencion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Nota + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Nota + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Numero_Nota_Impreso + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Observaciones + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ", TO_CHAR(";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ", 'DD/Mon/YYYY HH:MI:SS PM') AS FECHA_ORDEN, ";

                    if (Ordenes_Variacion.P_Incluir_Campos_Detalles)
                    {
                        Mi_SQL += Ordenes_Variacion.P_Campos_Detalles;
                    }
                }
                if (Mi_SQL.Trim().EndsWith(","))
                {
                    Mi_SQL = Mi_SQL.Trim().Substring(0, Mi_SQL.Trim().Length - 1);
                }
                Mi_SQL += " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                if (Ordenes_Variacion.P_Filtros_Dinamicos != null && Ordenes_Variacion.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Ordenes_Variacion.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Ordenes_Variacion.P_Orden_Variacion_ID != "" && Ordenes_Variacion.P_Orden_Variacion_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + Validar_Operador_Comparacion(Ordenes_Variacion.P_Orden_Variacion_ID) + " AND ";
                    }
                    if (Ordenes_Variacion.P_Cuenta_Predial_ID != "" && Ordenes_Variacion.P_Cuenta_Predial_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '" + Ordenes_Variacion.P_Cuenta_Predial_ID + "' AND ";
                    }
                    if (Ordenes_Variacion.P_Cuenta_Predial != "" && Ordenes_Variacion.P_Cuenta_Predial != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE UPPER(" + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ")" + Validar_Operador_Comparacion(Ordenes_Variacion.P_Cuenta_Predial) + ") AND ";
                    }
                    if (Ordenes_Variacion.P_Generar_Orden_Movimiento_ID != "" && Ordenes_Variacion.P_Generar_Orden_Movimiento_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + Validar_Operador_Comparacion(Ordenes_Variacion.P_Generar_Orden_Movimiento_ID) + " AND ";
                    }
                    if (Ordenes_Variacion.P_Contrarecibo != "" && Ordenes_Variacion.P_Contrarecibo != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + Validar_Operador_Comparacion(Ordenes_Variacion.P_Contrarecibo) + " AND ";
                    }
                    if (Ordenes_Variacion.P_Generar_Orden_Anio != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Ordenes_Variacion.P_Generar_Orden_Anio + " AND ";
                    }
                    if (Ordenes_Variacion.P_Año != 0)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Ordenes_Variacion.P_Año + " AND ";
                    }
                    if (Ordenes_Variacion.P_Generar_Orden_Estatus != "" && Ordenes_Variacion.P_Generar_Orden_Estatus != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = '" + Ordenes_Variacion.P_Generar_Orden_Estatus + "' AND ";
                    }
                    if (Ordenes_Variacion.P_Fecha_Creo != "" && Ordenes_Variacion.P_Fecha_Creo != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + Validar_Operador_Comparacion(Ordenes_Variacion.P_Fecha_Creo) + " AND ";
                    }
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                }
                if (Ordenes_Variacion.P_Agrupar_Dinamico != null && Ordenes_Variacion.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Ordenes_Variacion.P_Agrupar_Dinamico;
                }
                if (Ordenes_Variacion.P_Ordenar_Dinamico != null && Ordenes_Variacion.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Ordenes_Variacion.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC";
                }
                if (Ordenes_Variacion.P_Maximo_Registros > 0)
                {
                    Mi_SQL += ") WHERE ROWNUM <= " + Ordenes_Variacion.P_Maximo_Registros.ToString();
                }

                DataSet Ds_Ordenes_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Ordenes_Variacion != null)
                {
                    Dt_Ordenes_Variacion = Ds_Ordenes_Variacion.Tables[0];

                    if (Dt_Ordenes_Variacion.Rows.Count > 0)
                    {
                        //Consulta las Observaciones de la Orden
                        Mi_SQL = "SELECT ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_Observaciones_ID + ", ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_No_Orden_Variacion + ", ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_Usuario_Creo + ", ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_Año + ", ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_Descripcion;
                        Mi_SQL += " FROM ";
                        Mi_SQL += Ope_Pre_Observaciones.Tabla_Ope_Pre_Observaciones_Orden_Variacion;
                        Mi_SQL += " WHERE ";
                        if (Ordenes_Variacion.P_Observaciones_Observacion_ID != null && Ordenes_Variacion.P_Observaciones_Observacion_ID != "")
                        {
                            Mi_SQL += Ope_Pre_Observaciones.Campo_Observaciones_ID + " = '" + Ordenes_Variacion.P_Observaciones_Observacion_ID + "' AND ";
                        }
                        if (Ordenes_Variacion.P_Año != null)
                        {
                            Mi_SQL += Ope_Pre_Observaciones.Campo_Año + " = " + Ordenes_Variacion.P_Año + " AND ";
                        }
                        if (Ordenes_Variacion.P_Observaciones_No_Orden_Variacion != null && Ordenes_Variacion.P_Observaciones_No_Orden_Variacion != "")
                        {
                            Mi_SQL += Ope_Pre_Observaciones.Campo_No_Orden_Variacion + " = '" + Ordenes_Variacion.P_Observaciones_No_Orden_Variacion + "' AND ";
                        }
                        else
                        {
                            Mi_SQL += Ope_Pre_Observaciones.Campo_No_Orden_Variacion + " = '" + Ds_Ordenes_Variacion.Tables[0].Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion] + "' AND ";
                        }
                        if (Mi_SQL.EndsWith(" AND "))
                        {
                            Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                        }
                        if (Mi_SQL.EndsWith(" WHERE "))
                        {
                            Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                        }
                        Mi_SQL += " ORDER BY " + Ope_Pre_Observaciones.Campo_Observaciones_ID + " DESC";
                        Ordenes_Variacion.P_Dt_Observaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Ordenes_Variacion;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Ordenes_Variacion_Contrarecibos
        ///DESCRIPCIÓN          : Devuelve un DataTable con los registros de las Órdenes de Variación filtrado por contrarecibos POR PAGAR
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 26-sep-2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Ordenes_Variacion_Contrarecibos(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            DataTable Dt_Ordenes_Variacion = new DataTable();
            String Mi_SQL = "";
            Boolean Cargar_Detalles_Orden = false;
            try
            {
                Mi_SQL += "SELECT ";
                if (Ordenes_Variacion.P_Incluir_Campos_Foraneos)
                {
                    Mi_SQL += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ") AS Cuenta_Predial, ";
                    Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS Identificador_Movimiento, ";
                    Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS Descripcion_Movimiento, ";
                }
                if (Ordenes_Variacion.P_Campos_Dinamicos != null && Ordenes_Variacion.P_Campos_Dinamicos != "")
                {
                    Mi_SQL += Ordenes_Variacion.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ".";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ".";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Nota + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Nota + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Numero_Nota_Impreso + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ".";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Anio + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ".";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Observaciones + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ".";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Usuario_Creo + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ".";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo;
                }
                //Consulta los Detalles de la Orden
                if (Ordenes_Variacion.P_Incluir_Campos_Detalles != null && Ordenes_Variacion.P_Campos_Detalles != "")
                {
                    Mi_SQL += Ordenes_Variacion.P_Campos_Detalles;
                }

                Mi_SQL += " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;

                if (Ordenes_Variacion.P_Join_Contrarecibo)
                {
                    Mi_SQL += " JOIN " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " CR ON "
                        + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                                + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " = CR."
                                + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " AND "
                                + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                                + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CR."
                                + Ope_Pre_Contrarecibos.Campo_Anio;
                    if (!String.IsNullOrEmpty(Ordenes_Variacion.P_Contrarecibo_Estatus))
                    {
                        Mi_SQL += " AND CR." + Ope_Pre_Contrarecibos.Campo_Estatus + " NOT IN (" + Ordenes_Variacion.P_Contrarecibo_Estatus + ") ";
                    }
                    else
                    {
                        Mi_SQL += " AND CR." + Ope_Pre_Contrarecibos.Campo_Estatus + " NOT IN ('PAGADO', 'PENDIENTE')";
                    }
                }

                if (Ordenes_Variacion.P_Filtros_Dinamicos != null && Ordenes_Variacion.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Ordenes_Variacion.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Ordenes_Variacion.P_Orden_Variacion_ID != "" && Ordenes_Variacion.P_Orden_Variacion_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion
                            + Validar_Operador_Comparacion(Ordenes_Variacion.P_Orden_Variacion_ID) + " AND ";
                        Cargar_Detalles_Orden = true;
                    }
                    if (Ordenes_Variacion.P_Cuenta_Predial_ID != "" && Ordenes_Variacion.P_Cuenta_Predial_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '"
                            + Ordenes_Variacion.P_Cuenta_Predial_ID + "' AND ";
                    }
                    if (Ordenes_Variacion.P_Cuenta_Predial != "" && Ordenes_Variacion.P_Cuenta_Predial != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " IN (SELECT "
                            + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM "
                            + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE UPPER("
                            + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ")"
                            + Validar_Operador_Comparacion(Ordenes_Variacion.P_Cuenta_Predial) + ") AND ";
                    }
                    if (Ordenes_Variacion.P_Generar_Orden_Movimiento_ID != "" && Ordenes_Variacion.P_Generar_Orden_Movimiento_ID != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + " = '"
                            + Ordenes_Variacion.P_Generar_Orden_Movimiento_ID + "' AND ";
                    }
                    if (Ordenes_Variacion.P_Contrarecibo != "" && Ordenes_Variacion.P_Contrarecibo != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo
                            + Validar_Operador_Comparacion(Ordenes_Variacion.P_Contrarecibo) + " AND ";
                    }
                    if (Ordenes_Variacion.P_Generar_Orden_Anio != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = "
                            + Ordenes_Variacion.P_Generar_Orden_Anio + " AND ";
                    }
                    if (Ordenes_Variacion.P_Generar_Orden_Estatus != "" && Ordenes_Variacion.P_Generar_Orden_Estatus != null)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = '"
                            + Ordenes_Variacion.P_Generar_Orden_Estatus + "' AND ";
                    }
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                }
                if (Ordenes_Variacion.P_Agrupar_Dinamico != null && Ordenes_Variacion.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Ordenes_Variacion.P_Agrupar_Dinamico;
                }
                if (Ordenes_Variacion.P_Ordenar_Dinamico != null && Ordenes_Variacion.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Ordenes_Variacion.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL += " ORDER BY " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                            + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC";
                }

                DataSet Ds_Ordenes_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Ordenes_Variacion != null)
                {
                    Dt_Ordenes_Variacion = Ds_Ordenes_Variacion.Tables[0];

                    if (Dt_Ordenes_Variacion.Rows.Count > 0 && Cargar_Detalles_Orden)
                    {
                        //Consulta las Observaciones de la Orden
                        Mi_SQL = "SELECT ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_Observaciones_ID + ", ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_No_Orden_Variacion + ", ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_Año + ", ";
                        Mi_SQL += Ope_Pre_Observaciones.Campo_Descripcion;
                        Mi_SQL += " FROM ";
                        Mi_SQL += Ope_Pre_Observaciones.Tabla_Ope_Pre_Observaciones_Orden_Variacion;
                        Mi_SQL += " WHERE ";
                        if (Ordenes_Variacion.P_Observaciones_Observacion_ID != null && Ordenes_Variacion.P_Observaciones_Observacion_ID != "")
                        {
                            Mi_SQL += Ope_Pre_Observaciones.Campo_Observaciones_ID + " = '" + Ordenes_Variacion.P_Observaciones_Observacion_ID + "' AND ";
                        }
                        if (Ordenes_Variacion.P_Año != null)
                        {
                            Mi_SQL += Ope_Pre_Observaciones.Campo_Año + " = " + Ordenes_Variacion.P_Año + " AND ";
                        }
                        if (Ordenes_Variacion.P_Observaciones_No_Orden_Variacion != null && Ordenes_Variacion.P_Observaciones_No_Orden_Variacion != "")
                        {
                            Mi_SQL += Ope_Pre_Observaciones.Campo_No_Orden_Variacion + " = '" + Ordenes_Variacion.P_Observaciones_No_Orden_Variacion + "' AND ";
                        }
                        else
                        {
                            Mi_SQL += Ope_Pre_Observaciones.Campo_No_Orden_Variacion + " = '" + Ds_Ordenes_Variacion.Tables[0].Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion] + "' AND ";
                        }
                        if (Mi_SQL.EndsWith(" AND "))
                        {
                            Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                        }
                        if (Mi_SQL.EndsWith(" WHERE "))
                        {
                            Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                        }
                        Mi_SQL += " ORDER BY " + Ope_Pre_Observaciones.Campo_Observaciones_ID + " DESC";
                        Ordenes_Variacion.P_Dt_Observaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Impuestos por Derechos de Supervisión. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Ordenes_Variacion;
        }


        #endregion

        #region [Consultas Cuotas Fijas]

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuota_Fija_Detalles
        ///DESCRIPCIÓN: cosnsulta los detalles de la couta fija
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/13/2011 10:04:43 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        internal static DataTable Consultar_Cuota_Fija_Detalles(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL           

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + "CUO.*,";
                Mi_SQL = Mi_SQL + "CA.*";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas + " CUO LEFT OUTER JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales + " CA ON CA.";
                Mi_SQL = Mi_SQL + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = CUO." + Ope_Pre_Cuotas_Fijas.Campo_Caso_Especial_Id;

                Mi_SQL = Mi_SQL + " WHERE ";
                if (Datos.P_No_Cuota_Fija.Trim().Length == 5)
                {
                    Mi_SQL = Mi_SQL + "CA." + Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = '" + Datos.P_No_Cuota_Fija + "'";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "CUO." + Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " = '" + Datos.P_No_Cuota_Fija + "'";
                }
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
        #endregion
        #region [Consulta de Propietarios y Copropietarios de La orden]
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN     : Consultar_Copropietarios_Variacion
        ///DESCRIPCIÓN              : Se consultan con el ID de la cuenta los datos de los Copropietarios en la Variación
        ///PARAMETROS: 
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 31/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        internal static DataSet Consultar_Copropietarios_Variacion(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = "";
            DataSet Ds_Copropietarios_Variacion = new DataSet();
            try
            {
                Mi_SQL = "";
                Mi_SQL += "SELECT ";
                Mi_SQL += " COP. " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + ", ";
                Mi_SQL += " COP. " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| ";
                Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_CONTRIBUYENTE, ";
                Mi_SQL += " COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + ", ";
                //Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario + " AS TIPO_PROPIETARIO, ";
                Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + ", ";
                Mi_SQL += " COP. " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " AS TIPO_PROPIETARIO, ";
                Mi_SQL += " o. " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " ";

                Mi_SQL += " FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + " COP LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CON ON CON.";
                Mi_SQL += Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " o ON o.";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion;

                Mi_SQL += " WHERE ";
                Mi_SQL += " COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                Mi_SQL += " AND ( COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " = 'COPROPIETARIO'";
                Mi_SQL += " AND COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + Datos.P_Orden_Variacion_ID + "')";
                if (Datos.P_Copropietario_Filtra_Estatus)
                {
                    Mi_SQL += " AND COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + " = 'ALTA'";
                }
                Mi_SQL += " AND o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = '" + Datos.P_Orden_Variacion_ID + "'";
                Mi_SQL += " AND o." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = '" + Datos.P_Año + "'";
                Mi_SQL += " AND COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Datos.P_Año;
                Mi_SQL += " ORDER BY COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Copropietario_Orden_Variacion_ID;
                Ds_Copropietarios_Variacion.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Copropietarios_Variacion.Tables[0].TableName = "Dt_Copropietarios_Variacion";

                return Ds_Copropietarios_Variacion;
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

        /////*******************************************************************************
        /////NOMBRE DE LA FUNCIÓN : Consultar_Variacion_Copropietarios
        /////DESCRIPCIÓN          : Consulta los Copropietarios de la Variación Indicada
        /////PARAMETROS:     
        /////CREO                 : Antonio Salvador Benavides Guardado
        /////FECHA_CREO           : 07/Febrero/2012
        /////MODIFICO:
        /////FECHA_MODIFICO
        /////CAUSA_MODIFICACIÓN
        /////*******************************************************************************
        //private static DataTable Consultar_Variacion_Copropietarios(Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion)
        //{
        //    DataTable Dt_Copropietarios_Cuenta;
        //    DataSet Ds_Copropietarios_Variacion;
        //    DataTable Dt_Copropietarios_Variacion;
        //    DataTable Dt_Temp_Copropietarios = new DataTable();
        //    DataRow Dr_Temp_Copropietario;
        //    try
        //    {
        //        Dt_Copropietarios_Cuenta = Consulta_Co_Propietarios(Orden_Variacion);
        //        Orden_Variacion.P_Copropietario_Filtra_Estatus = true;
        //        Ds_Copropietarios_Variacion = Consultar_Copropietarios_Variacion(Orden_Variacion);
        //        if (Ds_Copropietarios_Variacion != null)
        //        {
        //            if (Ds_Copropietarios_Variacion.Tables.Count > 0)
        //            {
        //                Dt_Copropietarios_Variacion = Ds_Copropietarios_Variacion.Tables[0];
        //                if (Dt_Copropietarios_Variacion.Rows.Count > 0)
        //                {
        //                    Dt_Temp_Copropietarios.Columns.Add(new DataColumn("CONTRIBUYENTE_ID", typeof(String)));
        //                    Dt_Temp_Copropietarios.Columns.Add(new DataColumn("RFC", typeof(String)));
        //                    Dt_Temp_Copropietarios.Columns.Add(new DataColumn("NOMBRE_CONTRIBUYENTE", typeof(String)));
        //                    Dt_Temp_Copropietarios.Columns.Add(new DataColumn("ESTATUS_VARIACION", typeof(String)));

        //                    foreach (DataRow Copropietario_Variacion in Dt_Copropietarios_Variacion.Rows)
        //                    {
        //                        Dr_Temp_Copropietario = Dt_Temp_Copropietarios.NewRow();
        //                        Dr_Temp_Copropietario["CONTRIBUYENTE_ID"] = Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString();
        //                        Dr_Temp_Copropietario["RFC"] = Copropietario_Variacion["RFC"].ToString();
        //                        Dr_Temp_Copropietario["NOMBRE_CONTRIBUYENTE"] = Copropietario_Variacion["NOMBRE_CONTRIBUYENTE"].ToString();
        //                        Dr_Temp_Copropietario["ESTATUS_VARIACION"] = "NUEVO";
        //                        Dt_Temp_Copropietarios.Rows.Add(Dr_Temp_Copropietario);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
        //        throw new Exception(Mensaje);
        //    }
        //    return Dt_Temp_Copropietarios;
        //}

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Co_Propietarios
        ///DESCRIPCIÓN: consulta los copropietarios que tiene la cuenta
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/11/2011 09:17:42 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        internal static DataTable Consulta_Co_Propietarios(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL           

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS CONTRIBUYENTE_ID,";
                Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| ";
                Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_CONTRIBUYENTE, ";
                Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                Mi_SQL = Mi_SQL + " PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + ", ";
                Mi_SQL = Mi_SQL + " PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + " PROP." + Cat_Pre_Propietarios.Campo_Tipo + ", ";
                Mi_SQL = Mi_SQL + " 'ACTUAL' ESTATUS_VARIACION ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CONT";
                Mi_SQL = Mi_SQL + " JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP ON PROP.";
                Mi_SQL = Mi_SQL + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = CONT." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID;
                Mi_SQL = Mi_SQL + " WHERE  PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND  PROP." + Cat_Pre_Propietarios.Campo_Tipo + " = 'COPROPIETARIO'";
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Historial_Estatus_Ordenes_Variacion_Cuenta
        ///DESCRIPCIÓN          : Devuelve un DataTable con los registros de las Órdenes de Variación
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Agosto/2011
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 10/Febrero/2012
        ///CAUSA_MODIFICACIÓN   : Adecuar a nueva estructura de tabla de Órdenes de Variación
        ///*******************************************************************************
        public static DataTable Consultar_Historial_Estatus_Ordenes_Variacion_Cuenta(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            DataTable Dt_Ordenes_Variacion = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL += "SELECT ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS " + Cat_Pre_Movimientos.Campo_Identificador + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS " + Cat_Pre_Movimientos.Campo_Descripcion + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " || ' - ' || " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS IDENTIFICADOR_DESCRIPCION, ";
                Mi_SQL += "NVL((SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
                Mi_SQL += " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = (SELECT " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR'))), ";
                Mi_SQL += "(SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
                Mi_SQL += " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = (SELECT " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + " FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + " = 'ALTA' AND " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')))) AS NOMBRE_PROPIETARIO, ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden;
                Mi_SQL += " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                //Mi_SQL += ", " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                //Mi_SQL += ", " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                Mi_SQL += " WHERE ";
                //Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AND ";
                //Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " AND ";
                //Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " AND ";
                //Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " AND ";
                if (Ordenes_Variacion.P_Cuenta_Predial_ID != "" && Ordenes_Variacion.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '" + Ordenes_Variacion.P_Cuenta_Predial_ID + "' AND ";
                }
                if (Ordenes_Variacion.P_Cuenta_Predial != "" && Ordenes_Variacion.P_Cuenta_Predial != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE UPPER(" + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ")" + Validar_Operador_Comparacion(Ordenes_Variacion.P_Cuenta_Predial) + ") AND ";
                }
                if (Ordenes_Variacion.P_Contrarecibo != "" && Ordenes_Variacion.P_Contrarecibo != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + Validar_Operador_Comparacion(Ordenes_Variacion.P_Contrarecibo) + " AND ";
                }
                if (Ordenes_Variacion.P_Año != 0)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Ordenes_Variacion.P_Año + " AND ";
                }
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + Validar_Operador_Comparacion(Ordenes_Variacion.P_Generar_Orden_Estatus) + " AND ";
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;

                DataSet Ds_Ordenes_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Ordenes_Variacion != null)
                {
                    Dt_Ordenes_Variacion = Ds_Ordenes_Variacion.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Órdenes de Variación. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Ordenes_Variacion;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Historial_Estatus_Ordenes_Estatus_Contrarecibo
        ///DESCRIPCIÓN          : Devuelve un DataTable con los registros de las Órdenes de Variación
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 03/Marzo/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO: 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        public static DataTable Consultar_Historial_Estatus_Ordenes_Estatus_Contrarecibo(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            DataTable Dt_Ordenes_Variacion = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL += "SELECT ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS " + Cat_Pre_Movimientos.Campo_Identificador + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS " + Cat_Pre_Movimientos.Campo_Descripcion + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " || ' - ' || " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS IDENTIFICADOR_DESCRIPCION, ";
                Mi_SQL += "NVL((SELECT " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Nombre + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
                Mi_SQL += " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = (SELECT " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + " FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + " = 'ALTA' AND " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + "." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR'))), ";
                Mi_SQL += "(SELECT " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Nombre + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes;
                Mi_SQL += " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = (SELECT " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AND " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')))) AS NOMBRE_PROPIETARIO, ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden;
                Mi_SQL += " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ", " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos;
                Mi_SQL += " WHERE ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " AND ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " = " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " AND ";
                if (Ordenes_Variacion.P_Cuenta_Predial_ID != "" && Ordenes_Variacion.P_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '" + Ordenes_Variacion.P_Cuenta_Predial_ID + "' AND ";
                }
                if (Ordenes_Variacion.P_Cuenta_Predial != "" && Ordenes_Variacion.P_Cuenta_Predial != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE UPPER(" + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ")" + Validar_Operador_Comparacion(Ordenes_Variacion.P_Cuenta_Predial) + ") AND ";
                }
                if (Ordenes_Variacion.P_Contrarecibo != "" && Ordenes_Variacion.P_Contrarecibo != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + Validar_Operador_Comparacion(Ordenes_Variacion.P_Contrarecibo) + " AND ";
                }
                if (Ordenes_Variacion.P_Año != 0)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Ordenes_Variacion.P_Año + " AND ";
                }
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + Validar_Operador_Comparacion(Ordenes_Variacion.P_Generar_Orden_Estatus) + " AND ";
                Mi_SQL += Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_Estatus + Validar_Operador_Comparacion(Ordenes_Variacion.P_Contrarecibo_Estatus) + " AND ";
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;

                DataSet Ds_Ordenes_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Ordenes_Variacion != null)
                {
                    Dt_Ordenes_Variacion = Ds_Ordenes_Variacion.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Órdenes de Variación. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Ordenes_Variacion;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Dato_Anterior
        ///DESCRIPCIÓN: se consulta el dato anterior del campo y cuenta especificado
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 11/Ago/2011 11:51:41 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static String Consulta_Dato_Anterior(String Cuenta, String Campo)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            DataSet Ds_Resultado = new DataSet();

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Campo;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0].ToString();
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

        public static DataTable Consultar_Adeudos_Predial(Cls_Ope_Pre_Orden_Variacion_Negocio Adeudo)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6;
                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Anio + " AS AÑO";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Adeudo.P_Cuenta_Predial_ID + "'";
                //Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Adeudos_Predial.Campo_Monto_Por_Pagar + " != 0";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Adeudos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }

        public static DataTable Consulta_Diferencias(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            DataSet Ds_Resultado = new DataSet();

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + " di." + Ope_Pre_Diferencias.Campo_No_Diferencia + ",";
                Mi_SQL = Mi_SQL + " di." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + ",";
                Mi_SQL = Mi_SQL + " di." + Ope_Pre_Diferencias.Campo_Total_Recargos + ",";
                Mi_SQL = Mi_SQL + " di." + Ope_Pre_Diferencias.Campo_Total_Corriente + ",";
                Mi_SQL = Mi_SQL + " di." + Ope_Pre_Diferencias.Campo_Total_Rezago + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Importe + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Tasa_Predial_ID + " AS TASA_ID,";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Diferencia + " AS TIPO,";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Periodo + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Valor_Fiscal + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Cuota_Bimestral + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_1 + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_2 + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_3 + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_4 + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_5 + ",";
                Mi_SQL = Mi_SQL + " de." + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_6 + ",";
                Mi_SQL = Mi_SQL + " ta." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " AS TASA, ";
                Mi_SQL = Mi_SQL + " ta_P." + Cat_Pre_Tasas_Predial.Campo_Descripcion + " AS DESCRIPCION_TASA";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + " di ";
                Mi_SQL = Mi_SQL + " JOIN " + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + " de ON de.";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencia + " = di.";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia + " LEFT OUTER JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " ta on ta.";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " = de." + Ope_Pre_Diferencias_Detalle.Campo_Tasa_Predial_ID + " LEFT OUTER JOIN ";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + " ta_P on ta_P.";
                Mi_SQL = Mi_SQL + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " = ta." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID + " ";

                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + "di." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + " = '" + Datos.P_Generar_Orden_No_Orden + "'";
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + "di." + Ope_Pre_Diferencias.Campo_Anio + " = '" + Datos.P_Generar_Orden_Anio + "'";

                if (Datos.P_Ordenar_Dinamico != null && Datos.P_Ordenar_Dinamico.Length != 0)
                {
                    Mi_SQL = Mi_SQL + " ORDER BY " + Datos.P_Ordenar_Dinamico;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " ORDER BY SUBSTR(de." + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ", LENGTH(de." + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ") - 3, 4), ";
                    Mi_SQL = Mi_SQL + "de." + Ope_Pre_Diferencias_Detalle.Campo_Periodo;
                }
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

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Obtener_Anio_Minimo_Maximo
        /// DESCRIPCIÓN: Breve descripción de lo que hace la función.
        /// PARÁMETROS:
        /// 		1. Desde_Anio: Variable que almacena el valor del año menor
        /// 		2. Hasta_Anio: Variable que almacena el valor del año mayor
        /// 		3. Dt_Adeudos: Datatable con periodos a procesar
        /// 		4. Nombre_Fila: Nombre de la fila con los periodos
        /// 		5. Indice: Valor entero desde el que se obtiene el año (formatos: 1/2011 y 12011)
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 27-sep-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Boolean Obtener_Anio_Minimo_Maximo(out Int32 Desde_Anio, out Int32 Hasta_Anio, DataTable Dt_Adeudos, String Nombre_Fila, Int32 Indice)
        {
            Int32 Anio = 0;
            Desde_Anio = 99999;
            Hasta_Anio = 0;

            // para cada fila en la tabla
            foreach (DataRow Fila_Tabla in Dt_Adeudos.Rows)
            {
                // obtener y parsear el año
                String St_Anio = Fila_Tabla[Nombre_Fila].ToString().Substring(Indice, 4);
                if (Int32.TryParse(St_Anio, out Anio))
                {
                    if (Anio < Desde_Anio)
                    {
                        Desde_Anio = Anio;
                    }
                    if (Anio > Hasta_Anio)
                    {
                        Hasta_Anio = Anio;
                    }
                }
            }

            // regresar verdadero si se asignaron años a las variables desde y hasta anio
            if (Desde_Anio < 99999 && Hasta_Anio > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN     : Consultar_Datos_Copropietarios_Variacion
        ///DESCRIPCIÓN              : Se consultan con el ID de la cuenta los datos de los Copropietarios en la Variación
        ///PARAMETROS: 
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 06/Septiembre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        internal static DataSet Consultar_Propietarios_Variacion(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = "";
            DataSet Ds_Copropietarios_Variacion = new DataSet();
            try
            {
                Mi_SQL = "";
                Mi_SQL += "SELECT ";
                Mi_SQL += " COP. " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + ", ";
                Mi_SQL += " COP. " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| ";
                Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE_CONTRIBUYENTE, ";
                Mi_SQL += " COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + ", ";
                //Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario + " AS TIPO_PROPIETARIO, ";
                Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + ", ";
                Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + ", ";


                //---------------------------------------------------------------------------------------------------------------------------------------
                Mi_SQL += " CALL. " + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                Mi_SQL += " COL. " + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                //---------------------------------------------------------------------------------------------------------------------------------------



                Mi_SQL += " COP. " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " AS TIPO_PROPIETARIO, ";
                Mi_SQL += " o. " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " ";

                Mi_SQL += " FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + " COP LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CON ON CON.";
                Mi_SQL += Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " o ON o.";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion;
                Mi_SQL += " AND o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID;
                Mi_SQL += " AND o." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio;
                if (!String.IsNullOrEmpty(Datos.P_Estatus_Orden))
                    Mi_SQL += " AND o." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = '" + Datos.P_Estatus_Orden + "'";

                //---------------------------------------------------------------------------------------------------------------------------------------
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALL ON CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = CALL." + Cat_Pre_Calles.Campo_Calle_ID;
                Mi_SQL += " LEFT OUTER JOIN ";
                Mi_SQL += Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL ON CUEN.";
                Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = COL." + Cat_Ate_Colonias.Campo_Colonia_ID;

                //---------------------------------------------------------------------------------------------------------------------------------------

                Mi_SQL += " WHERE ";
                Mi_SQL += " COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'";
                Mi_SQL += " AND (( COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " = 'PROPIETARIO'";
                Mi_SQL += " OR COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " = 'POSEEDOR')";
                if (Datos.P_Año != 0)
                {
                    Mi_SQL += " AND COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + Datos.P_Orden_Variacion_ID + "'";
                }
                Mi_SQL += ")";
                if (Datos.P_Propietario_Filtra_Estatus)
                {
                    Mi_SQL += " AND COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + " = 'ALTA'";
                }
                if (Datos.P_Año != 0)
                {
                    Mi_SQL += " AND o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = '" + Datos.P_Orden_Variacion_ID + "'";
                    Mi_SQL += " AND o." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Datos.P_Año;
                    Mi_SQL += " AND COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Datos.P_Año;
                }
                else
                {
                    //CUALQUIER MODIFICACIÓN A LA SIGUIENTE LÍNEA APLICARLA EN EL REPLACE DE ABAJO TAMBIÉN
                    Mi_SQL += " AND o." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " < '" + Datos.P_Fecha_Modifico + "'";
                    //Mi_SQL += " AND TO_NUMBER(o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ") <= " + Convert.ToInt64(Datos.P_Orden_Variacion_ID).ToString();
                }
                //CUALQUIER MODIFICACIÓN A LA SIGUIENTE LÍNEA EN EL PRIMER PARÁMETRO DEL ORDER (FECHA_CREO DESC) CONSIDERARLA EN EL REPLACE DE ABAJO TAMBIÉN
                Mi_SQL += " ORDER BY o." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " DESC, COP." + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus;

                Ds_Copropietarios_Variacion.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Copropietarios_Variacion.Tables[0].TableName = "Dt_Copropietarios_Variacion";
                if (Ds_Copropietarios_Variacion.Tables[0].Rows.Count == 0 && Datos.P_Año == 0)
                {
                    Mi_SQL = Mi_SQL.Replace(" AND o." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " < '" + Datos.P_Fecha_Modifico + "'", "");
                    Mi_SQL = Mi_SQL.Replace(" ORDER BY o." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " DESC", " ORDER BY o." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " ASC");

                    Ds_Copropietarios_Variacion.Tables.Remove("Dt_Copropietarios_Variacion");
                    Ds_Copropietarios_Variacion.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                    Ds_Copropietarios_Variacion.Tables[0].TableName = "Dt_Copropietarios_Variacion";
                }

                return Ds_Copropietarios_Variacion;
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Ultima_Orden_Con_Adeudos
        ///DESCRIPCIÓN          : Devuelve un DataTable con los registros de las Órdenes de Variación
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 13/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Ultima_Orden_Con_Adeudos(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            DataTable Dt_Ordenes_Variacion = new DataTable();
            String Mi_SQL = "";
            try
            {
                Mi_SQL += "SELECT * FROM (";
                Mi_SQL += "SELECT ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Anio + ", ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_No_Orden_Variacion;
                Mi_SQL += " FROM " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + ", " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                Mi_SQL += " WHERE ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AND ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Anio + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " AND ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " AND ";
                Mi_SQL += Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + " = '" + Ordenes_Variacion.P_Cuenta_Predial_ID + "' AND ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'ACEPTADA' AND ";
                if (Mi_SQL.EndsWith(" AND "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                }
                if (Mi_SQL.EndsWith(" WHERE "))
                {
                    Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                }
                Mi_SQL += " ORDER BY " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " DESC, " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + " DESC";
                Mi_SQL += ") WHERE ROWNUM = 1";

                DataSet Ds_Ordenes_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Ordenes_Variacion != null)
                {
                    Dt_Ordenes_Variacion = Ds_Ordenes_Variacion.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Órdenes de Variación. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Ordenes_Variacion;
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
        public static String Obtener_ID_Consecutivo(ref OracleCommand Cmmd, String Tabla, String Campo, String Filtro, Int32 Longitud_ID)
        {
            String Id = Convert.ToString(1).PadLeft(Longitud_ID, '0');
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                if (Filtro != "" && Filtro != null)
                {
                    Mi_SQL += " WHERE " + Filtro;
                }
                Cmmd.CommandText = Mi_SQL;
                Object Obj_Temp = Cmmd.ExecuteOracleScalar();
                if (Obj_Temp != null)
                {
                    if (Obj_Temp.ToString().Trim() != "")
                    {
                        Id = Convert.ToString((Convert.ToInt32(Obj_Temp.ToString()) + 1)).PadLeft(Longitud_ID, '0');
                    }
                }
                //Object Obj_Temp = OracleHelper.ExecuteScalar(Trans, CommandType.Text, Mi_SQL);
                //Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                //{
                //    Id = Convert.ToString((Convert.ToInt32(Obj_Temp) + 1)).PadLeft(Longitud_ID, '0');
                //}
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }
        #endregion

        #region [Consultas de Cuentas]

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Obtener_Campos_Tabla
        ///DESCRIPCIÓN: consulta los campos de una tabla en especifico
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 22/Ago/2011 8:03:39 a.m.
        ///MODIFICO: 16/Feb/2012
        ///FECHA_MODIFICO: Se ocupo un campo ESTATUS_CUENTA en los resultados de la consulta
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        internal static DataSet Obtener_Campos_Tabla(string Tabla)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL
            DataTable Dt_Nombres_Campos;
            DataTable Dt_Formar_Campos = new DataTable();
            DataSet Ds_Tabla_Campos = new DataSet();

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = '" + Tabla + "'  UNION SELECT 'ESTATUS_CUENTA' AS ESTATUS_CUENTA FROM" + Tabla;

                Dt_Nombres_Campos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                foreach (DataRow Dr_DataRow in Dt_Nombres_Campos.Rows)
                {
                    Dt_Formar_Campos.Columns.Add(Dr_DataRow["COLUMN_NAME"].ToString());
                }
                Dt_Formar_Campos.Columns.Add("DUMMIE_FIELD");
                Ds_Tabla_Campos.Tables.Add(Dt_Formar_Campos.Copy());

                DataRow myNewRow;
                myNewRow = Ds_Tabla_Campos.Tables[0].NewRow();
                myNewRow["DUMMIE_FIELD"] = "DUMMIE DATA";
                Ds_Tabla_Campos.Tables[0].Rows.Add(myNewRow);


                return Ds_Tabla_Campos;

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
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cuentas_Reactivadas
        ///DESCRIPCIÓN: Obtiene todos las Cajas que estan dadas de 
        ///             alta en la Base de Datos
        ///PARAMENTROS:   
        ///             1.  Orden.           Parametro de donde se sacara si habra o no un filtro de busqueda, en este
        ///                                 caso el filtro es la Clave.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 22/Junio/2011 
        ///MODIFICO: JESUS TOLEDO RDZ
        ///FECHA_MODIFICO: MARTES 06 DE SEP 2011
        ///CAUSA_MODIFICACIÓN: CONSULTAR CON DIFERENTES ESTAUS
        ///*******************************************************************************
        public static DataTable Consultar_Cuentas_Reactivadas(Cls_Ope_Pre_Orden_Variacion_Negocio Orden)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "";
                Mi_SQL = Mi_SQL + "SELECT * FROM (";
                Mi_SQL = Mi_SQL + "SELECT o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AS " + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + ", c." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                Mi_SQL = Mi_SQL + ", c." + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion;
                Mi_SQL = Mi_SQL + " AS Calle";
                Mi_SQL = Mi_SQL + ", c." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion;
                Mi_SQL = Mi_SQL + " AS No_Exterior";
                Mi_SQL = Mi_SQL + ",(Select cont." + Cat_Pre_Contribuyentes.Campo_Nombre + " ||''|| cont." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||''|| cont." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " as Nombre ";
                Mi_SQL = Mi_SQL + " from " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " cont inner join " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " Pre on ";
                Mi_SQL = Mi_SQL + " cont." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "=" + " Pre." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " left join " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " cuen ";
                Mi_SQL = Mi_SQL + " on Cuen. " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = Pre." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                Mi_SQL = Mi_SQL + " Where  Cuen." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AND Pre.";
                Mi_SQL = Mi_SQL + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO'";
                Mi_SQL = Mi_SQL + ") As Propietario ";
                Mi_SQL = Mi_SQL + ", o." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " AS " + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo;
                Mi_SQL = Mi_SQL + ", NVL((SELECT " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Total_Recargos + " FROM " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + " WHERE " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + " = o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AND " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Anio + " = o." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " AND " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + " = o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + "), 0) AS TOTAL_RECARGOS";
                Mi_SQL = Mi_SQL + ", NVL((SELECT " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Total_Corriente + " FROM " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + " WHERE " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + " = o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AND " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Anio + " = o." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " AND " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + " = o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + "), 0) AS TOTAL_CORRIENTE";
                Mi_SQL = Mi_SQL + ", NVL((SELECT " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Total_Rezago + " FROM " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + " WHERE " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + " = o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AND " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Anio + " = o." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " AND " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + " = o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + "), 0) AS TOTAL_REZAGO";
                Mi_SQL = Mi_SQL + ", NVL((SELECT NVL(" + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Total_Recargos + ", 0) + NVL(" + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Total_Corriente + ", 0) + NVL(" + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Total_Rezago + ", 0) FROM " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + " WHERE " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + " = o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AND " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_Anio + " = o." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " AND " + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + "." + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + " = o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + "), 0) AS ADEUDO_CANCELADO";
                //Mi_SQL = Mi_SQL + ", (SELECT SUM(a." + Ope_Pre_Adeudos_Predial.Campo_Monto_Por_Pagar + ") FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " a WHERE a." + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + "=o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ") AS ADEUDO_CANCELADO";
                Mi_SQL = Mi_SQL + ", o." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + "";
                Mi_SQL = Mi_SQL + ", (SELECT m." + Cat_Pre_Movimientos.Campo_Identificador + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " m WHERE m." + Cat_Pre_Movimientos.Campo_Movimiento_ID + "=o." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS TIPO_MOVIMIENTO";
                Mi_SQL = Mi_SQL + ", o." + Ope_Pre_Ordenes_Variacion.Campo_Observaciones + " AS OBSERVACIONES_CANCELACION";
                Mi_SQL = Mi_SQL + ", (SELECT OBSERVACIONES_ORDEN." + Ope_Pre_Observaciones.Campo_Descripcion + " FROM " + Ope_Pre_Observaciones.Tabla_Ope_Pre_Observaciones_Orden_Variacion + " OBSERVACIONES_ORDEN WHERE OBSERVACIONES_ORDEN.NO_ORDEN_VARIACION = o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " AND OBSERVACIONES_ORDEN.ANIO = o." + Ope_Pre_Ordenes_Variacion.Campo_Anio + ") AS OBSERVACIONES_VALIDACION";
                Mi_SQL = Mi_SQL + ", o." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " as Anio_Orden";
                Mi_SQL = Mi_SQL + ", o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " as No_Orden ";
                Mi_SQL = Mi_SQL + ", ROW_NUMBER() OVER (PARTITION BY o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " ORDER BY o." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " DESC) AS ROW_NUM";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " o";

                //JOIN CAT_PRE_CUENTAS_PREDIAL c ON c.CUENTA_PREDIAL_ID = o.CUENTA_PREDIAL_ID WHERE c.ESTATUS = 'CANCELADA' 
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " c ON c." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID;
                //Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Ope_Pre_Orden_Detalles.Tabla_Ope_Pre_Orden_Detalles + " d ON d." + Ope_Pre_Orden_Detalles.Campo_No_Orden_Variacion + " = o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;
                //Mi_SQL = Mi_SQL + " WHERE c." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " = 'CANCELADA' ";
                Mi_SQL = Mi_SQL + " WHERE TRIM(o." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + ") IN ('" + Orden.P_Generar_Orden_Estatus + "')";
                Mi_SQL = Mi_SQL + " AND o." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta + " = '" + Orden.P_Estatus_Cuenta + "'";
                Mi_SQL = Mi_SQL + " AND TRIM(c." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ") IN ( '" + Orden.P_Estatus_Cuenta + "')";
                //Mi_SQL = Mi_SQL + " AND o." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " IN (SELECT * FROM (SELECT o2." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Orden_Variacion + " o2, " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " c2 WHERE o2." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = c2." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " and TRIM(o2." + Ope_Pre_Ordenes_Variacion.Campo_Estatus + ") IN ( '" + Orden.P_Generar_Orden_Estatus + "') and TRIM(c2." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ") IN ('" + Orden.P_Estatus_Cuenta + "') ORDER BY o2." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC) where ROWNUM = 1)";
                //Añadir filtro
                if (Orden.P_Generar_Orden_Fecha_Inicial.Length != 0)
                {
                    Mi_SQL = Mi_SQL + " AND (o." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ">='" + Orden.P_Generar_Orden_Fecha_Inicial + "'";
                    if (Orden.P_Generar_Orden_Fecha_Final.Length != 0)
                    {
                        Mi_SQL = Mi_SQL + " AND o." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + "<'" + Orden.P_Generar_Orden_Fecha_Final + "')";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + ")";
                    }
                }
                else if (Orden.P_Generar_Orden_Fecha_Final.Length != 0)
                {
                    Mi_SQL = Mi_SQL + " AND (o." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + "<'" + Orden.P_Generar_Orden_Fecha_Final + "')";
                }
                if (Orden.P_Cuenta_Predial != null && Orden.P_Cuenta_Predial != "")
                {
                    Mi_SQL += " AND o." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " IN (SELECT c." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " c where c." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Orden.P_Cuenta_Predial + "%') ";
                }
                Mi_SQL = Mi_SQL + " ORDER BY o." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " DESC";
                Mi_SQL = Mi_SQL + ") TBL_TEMP";
                Mi_SQL = Mi_SQL + " WHERE ROW_NUM = 1";

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de Cancelación de Cuenta Predial. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }
        #endregion

        #region [Consultas de Contrarecibos]
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_General_Contrarecibo
        ///DESCRIPCIÓN: consulta de la tabla
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 20/Ago/2011 11:50:12 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        public static DataTable Consulta_General_Contrarecibo(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            

            try
            {
                Mi_SQL = "";
                Mi_SQL = "SELECT DISTINCT(";
                Mi_SQL += " CONT." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + ") AS CONTRARECIBO, ";
                Mi_SQL += " CONT." + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " AS CUENTA_ID, ";
                Mi_SQL += " CONT." + Ope_Pre_Contrarecibos.Campo_Estatus + " AS ESTATUS ";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " CONT ";
                Mi_SQL += " WHERE ";
                Mi_SQL += " CONT." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '";
                Mi_SQL += Datos.P_Contrarecibo + "'";
                if (Datos.P_Año != null)
                {
                    if (Datos.P_Año > 0)
                    {
                        Mi_SQL += " AND ";
                        Mi_SQL += " CONT." + Ope_Pre_Contrarecibos.Campo_Anio + " = '";
                        Mi_SQL += Datos.P_Año + "'";
                    }
                }
                //Mi_SQL += " AND (";
                //Mi_SQL = Mi_SQL + "CONT." + Ope_Pre_Contrarecibos.Campo_Estatus + "='GENERADO' OR ";
                //Mi_SQL = Mi_SQL + "CONT." + Ope_Pre_Contrarecibos.Campo_Estatus + "='POR VALIDAR' OR ";
                //Mi_SQL = Mi_SQL + "CONT." + Ope_Pre_Contrarecibos.Campo_Estatus + "='RECHAZADA' OR ";
                //Mi_SQL = Mi_SQL + "CONT." + Ope_Pre_Contrarecibos.Campo_Estatus + "='RECHAZADO') ";

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
        #endregion

        #region [Inserts Orden Variacion]
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Generar_Orden_Variacion
        ///DESCRIPCIÓN: se genera la orden de variacion con los datos de negocio
        ///             especificando el ID del movimiento, el ID de la cuenta a afectar
        ///             las observaciones, el estatus y una tabla con el nombre del campo 
        ///             o los campos su valor anterior y su valor modificado
        ///PARAMETROS: Objeto de la capa de Negocios de la Orden de Variacion
        ///CREO: jtoledo
        ///FECHA_CREO: 10/Ago/2011 12:24:27 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        internal static String Generar_Orden_Variacion(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;  //Variable para la sentencia SQL
            String Mi_SQL_2 = String.Empty;  //Variable para la sentencia SQL
            String Mensaje = String.Empty; //Variable para el mensaje de error
            String Diferencia_ID;
            String Diferencia_Detalle_ID;
            int Consecutivo;
            Object Aux; //Variable auxiliar para las consultas            
            String No_Detalle;
            DataTable Campo;
            String Consec = ""; //Variable para la consulta SQL
            DataTable Dt_Copropietarios_Variacion;
            Boolean Propietario_Poseedor_Eliminados;
            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Formar Sentencia para obtener el consecutivo de la orden                
                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + "),0000000000)";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = '";
                Mi_SQL = Mi_SQL + DateTime.Today.Year.ToString() + "' ";

                //Ejecutar consulta del consecutivo
                Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                {
                    Datos.P_Generar_Orden_No_Orden = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                }
                else
                    Datos.P_Generar_Orden_No_Orden = "0000000001";

                //Asignar consulta para la insercion de los datos generales de la orden a insertar
                Mi_SQL = "";
                Mi_SQL = "INSERT INTO ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ( ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Anio + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Observaciones + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Usuario_Creo + " ";

                //INSERTA LAS VARIACIONES ALMACENADAS EN EL DATATABLE DE VARIACIONES A LA TABLA DE DETALLES DE LA ORDEN
                foreach (DataRow Variacion in Datos.P_Generar_Orden_Dt_Detalles.Rows)
                {
                    //Formar Sentencia de consulta de NOMBRE DE COLUMNA PARA VERIFICAR SI EXISTE
                    Mi_SQL_2 = "";
                    Mi_SQL_2 = Mi_SQL_2 + "SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = ";
                    Mi_SQL_2 = Mi_SQL_2 + "'" + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "' AND COLUMN_NAME = ";
                    Mi_SQL_2 = Mi_SQL_2 + "'" + Variacion["CAMPO"].ToString() + "'";
                    //Ejecutar consulta
                    Campo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL_2).Tables[0];

                    if (Campo.Rows.Count > 0 && !Variacion["CAMPO"].ToString().Contains("CUENTA_PREDIAL") && !Variacion["CAMPO"].ToString().Contains("CREO") && !Variacion["CAMPO"].ToString().Contains("MODIFICO"))
                    {
                        Mi_SQL = Mi_SQL + "," + Variacion["CAMPO"].ToString() + " ";
                    }
                }

                Mi_SQL = Mi_SQL + ") VALUES('" + Datos.P_Generar_Orden_No_Orden + "','";
                Mi_SQL = Mi_SQL + DateTime.Today.Year.ToString() + "',";
                if (String.IsNullOrEmpty(Datos.P_Generar_Orden_Cuenta_ID))
                    Mi_SQL = Mi_SQL + "NULL" + ",'";
                else
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Generar_Orden_Cuenta_ID + "',";
                if (String.IsNullOrEmpty(Datos.P_Cuenta_Predial))
                    Mi_SQL = Mi_SQL + "NULL" + ",'";
                else
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Cuenta_Predial + "','";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Movimiento_ID + "','";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Estatus + "',UPPER('";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Obserbaciones + "'),'";
                Mi_SQL = Mi_SQL + Datos.P_Contrarecibo + "','";
                Mi_SQL = Mi_SQL + Datos.P_Grupo_Movimiento_ID;
                Mi_SQL = Mi_SQL + "',SYSDATE, '" + Cls_Sessiones.Nombre_Empleado + "'";

                foreach (DataRow Variacion in Datos.P_Generar_Orden_Dt_Detalles.Rows)
                {
                    //Formar Sentencia de consulta de NOMBRE DE COLUMNA PARA VERIFICAR SI EXISTE
                    Mi_SQL_2 = "";
                    Mi_SQL_2 = Mi_SQL_2 + "SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = ";
                    Mi_SQL_2 = Mi_SQL_2 + "'" + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "' AND COLUMN_NAME = ";
                    Mi_SQL_2 = Mi_SQL_2 + "'" + Variacion["CAMPO"].ToString() + "'";
                    //Ejecutar consulta
                    Campo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL_2).Tables[0];

                    if (Campo.Rows.Count > 0 && !Variacion["CAMPO"].ToString().Contains("CUENTA_PREDIAL") && !Variacion["CAMPO"].ToString().Contains("CREO") && !Variacion["CAMPO"].ToString().Contains("MODIFICO"))
                    {
                        if (!Variacion["DATO_NUEVO"].ToString().Trim().Contains("01/01/0001") && !String.IsNullOrEmpty(Variacion["DATO_NUEVO"].ToString()))
                        {
                            if (Variacion["CAMPO"].ToString().Trim().Contains("AVALUO") || Variacion["CAMPO"].ToString().Trim().Contains("TERMINO_EXENCION"))
                                Mi_SQL = Mi_SQL + ",'" + Convert.ToDateTime(Variacion["DATO_NUEVO"].ToString()).ToString("dd/MM/yyyy") + "'";
                            else
                                Mi_SQL = Mi_SQL + ",'" + Variacion["DATO_NUEVO"].ToString().Trim() + "' ";
                        }
                        else if (Variacion["CAMPO"].ToString().Trim() == Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion || Variacion["CAMPO"].ToString().Trim() == Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion || Variacion["CAMPO"].ToString().Trim() == Ope_Pre_Ordenes_Variacion.Campo_Estado_Notificacion || Variacion["CAMPO"].ToString().Trim() == Ope_Pre_Ordenes_Variacion.Campo_Estado_ID_Notificacion || Variacion["CAMPO"].ToString().Trim() == Ope_Pre_Ordenes_Variacion.Campo_Ciudad_ID_Notificacion || Variacion["CAMPO"].ToString().Trim() == Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija)
                        {
                            Mi_SQL = Mi_SQL + ", NULL";
                        }
                        else
                        {
                            Mi_SQL = Mi_SQL + ", NULL";
                        }
                    }
                }
                Mi_SQL = Mi_SQL + ")";
                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Insertar Analisis de Rezago
                if (Datos.P_Dt_Diferencias != null)
                {
                    if (Datos.P_Dt_Diferencias.Rows.Count > 0)
                    {
                        //Formar Sentencia de consulta de consecutivo de la tabla diferencias o rezago
                        Mi_SQL = "";
                        Mi_SQL = "SELECT NVL(MAX(";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia + "),0000000000)";
                        Mi_SQL = Mi_SQL + " FROM ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias;

                        //Ejecutar consulta

                        Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                        //Verificar si no es nulo
                        if (Convert.IsDBNull(Aux) == false)
                        {
                            Diferencia_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                        }
                        else
                            Diferencia_ID = "0000000001";

                        //Insertar Registro de Diferencias
                        Mi_SQL = "";
                        Mi_SQL = "INSERT INTO ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + " ( ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia + ",";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_Anio + ", ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + ") ";
                        Mi_SQL = Mi_SQL + "VALUES('";
                        Mi_SQL = Mi_SQL + Diferencia_ID + "','";
                        Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Cuenta_ID + "', ";
                        Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Anio + ", '";
                        Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_No_Orden + "') ";
                        //Ejecutar consulta
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Comando.ExecuteNonQuery();

                        //Formar Sentencia de consulta de consecutivo de la tabla de detalles diferencias o rezago
                        Mi_SQL = "";
                        Mi_SQL = "SELECT NVL(MAX(";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencias_Detalles + "),0000000000)";
                        Mi_SQL = Mi_SQL + " FROM ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle;

                        //Ejecutar consulta

                        Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                        //Verificar si no es nulo
                        if (Convert.IsDBNull(Aux) == false)
                        {
                            Diferencia_Detalle_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                        }
                        else
                            Diferencia_Detalle_ID = "0000000001";

                        foreach (DataRow Diferencia in Datos.P_Dt_Diferencias.Rows)
                        {
                            Mi_SQL = "";
                            Mi_SQL = "INSERT INTO ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + " ( ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencias_Detalles + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencia + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Valor_Fiscal + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tasa_Predial_ID + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Diferencia + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Periodo + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Importe + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Cuota_Bimestral + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ") VALUES('";
                            Mi_SQL = Mi_SQL + Diferencia_Detalle_ID + "','";
                            Mi_SQL = Mi_SQL + Diferencia_ID + "','" + Diferencia["VALOR_FISCAL"].ToString().Replace(",", "") + "','";
                            Mi_SQL = Mi_SQL + Diferencia["TASA_ID"].ToString() + "','" + Diferencia["TIPO"].ToString() + "','" + Diferencia["TIPO_PERIODO"].ToString() + "','";
                            Mi_SQL = Mi_SQL + Diferencia["IMPORTE"].ToString().Replace("$", "").Replace(",", "") + "','" + Diferencia["CUOTA_BIMESTRAL"].ToString().Replace("$", "").Replace(",", "") + "','" + Diferencia["PERIODO"].ToString() + "')";
                            Diferencia_Detalle_ID = String.Format("{0:0000000000}", Convert.ToInt32(Diferencia_Detalle_ID) + 1);
                            //Ejecutar consulta
                            Obj_Comando.CommandText = Mi_SQL;
                            Obj_Comando.ExecuteNonQuery();
                        }
                        //Agregar Variacion para ingresar el movimineto
                        //Datos.Agregar_Variacion(Cat_Pre_Cuentas_Predial.Campo_No_Diferencia, Diferencia_ID);
                    }
                }

                Mi_SQL = "";
                Mi_SQL = "UPDATE " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos;
                Mi_SQL += " SET " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'POR VALIDAR'";
                Mi_SQL += ", " + Ope_Pre_Contrarecibos.Campo_Usuario_Modifico + "='" + Cls_Sessiones.Nombre_Empleado + "'";
                Mi_SQL += ", " + Ope_Pre_Contrarecibos.Campo_Fecha_Modifico + "=SYSDATE";
                Mi_SQL += " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + "='" + Datos.P_Contrarecibo + "'";
                Mi_SQL += " AND " + Ope_Pre_Contrarecibos.Campo_Anio + "='" + Datos.P_Año + "'";
                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
                Obj_Conexion.Close();

                //Insertar Propietarios y Copropietarios de la Orden
                //Formar Sentencia para obtener el consecutivo de la orden      
                if (Datos.P_Dt_Contribuyentes != null)
                {
                    Mi_SQL = "";
                    Mi_SQL = "SELECT NVL(MAX(";
                    Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Copropietario_Orden_Variacion_ID + "),0000000000)";
                    Mi_SQL = Mi_SQL + " FROM ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                    //Ejecutar consulta del consecutivo
                    Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];
                    //Verificar si no es nulo
                    if (Convert.IsDBNull(Aux) == false)
                    {
                        Consec = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                    }
                    else
                    {
                        Consec = "0000000001";
                    }
                    Propietario_Poseedor_Eliminados = false;
                    foreach (DataRow Dr_Prop in Datos.P_Dt_Contribuyentes.Rows)
                    {
                        if ((Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() == "PROPIETARIO"
                             || Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() == "POSEEDOR")
                             && Propietario_Poseedor_Eliminados == false)
                        {
                            Mi_SQL = "";
                            Mi_SQL = "DELETE FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + Datos.P_Generar_Orden_No_Orden + "' ";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Datos.P_Generar_Orden_Anio + " ";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Generar_Orden_Cuenta_ID + "' ";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')";
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            Propietario_Poseedor_Eliminados = true;
                        }
                        Mi_SQL = "";
                        Mi_SQL = "SELECT " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Copropietario_Orden_Variacion_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + Datos.P_Generar_Orden_No_Orden + "' ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Datos.P_Generar_Orden_Anio + " ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Generar_Orden_Cuenta_ID + "' ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString() + "'";
                        if (Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() == "COPROPIETARIO")
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() + "'";
                        Dt_Copropietarios_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                        if (Dt_Copropietarios_Variacion.Rows.Count > 0)
                        {
                            Mi_SQL = "";
                            Mi_SQL = "UPDATE " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus].ToString() + "', ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() + "'";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + Datos.P_Generar_Orden_No_Orden + "' ";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Datos.P_Generar_Orden_Anio + " ";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Generar_Orden_Cuenta_ID + "' ";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString() + "'";
                            if (Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() == "COPROPIETARIO")
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() + "'";
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            Consec = String.Format("{0:0000000000}", Convert.ToInt32(Consec) + 1);
                        }
                        else
                        {
                            Mi_SQL = "";
                            Mi_SQL = "INSERT INTO ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + " ( ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Copropietario_Orden_Variacion_ID + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Usuario_Creo + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Fecha_Creo + ") VALUES('";
                            Mi_SQL = Mi_SQL + Consec + "','";
                            Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_No_Orden + "',";
                            if (String.IsNullOrEmpty(Datos.P_Generar_Orden_Cuenta_ID))
                                Mi_SQL = Mi_SQL + "NULL,'";
                            else
                                Mi_SQL = Mi_SQL + "'" + Datos.P_Generar_Orden_Cuenta_ID + "','";
                            Mi_SQL = Mi_SQL + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString() + "',";
                            Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Anio + ",'";
                            Mi_SQL = Mi_SQL + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus].ToString() + "','";
                            Mi_SQL = Mi_SQL + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() + "','";
                            Mi_SQL = Mi_SQL + Presidencia.Sessiones.Cls_Sessiones.Nombre_Empleado + "',SYSDATE )";
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            Consec = String.Format("{0:0000000000}", Convert.ToInt32(Consec) + 1);
                        }
                    }
                }

                return Datos.P_Generar_Orden_No_Orden;

            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Mi_SQL + "   ]" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;

            }
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN : Aplicar_Orden_Variacion
        ///DESCRIPCIÓN          : Guarda los datos de la Orden de Variación
        ///PARAMETROS: 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 02/Febrero/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        public static Boolean Aplicar_Variacion_Orden(Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion)
        {
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmmd = new OracleCommand();
            OracleTransaction Trans = null;
            DataTable Dt_Variacion_Cuenta;
            String Estatus_Cuenta_Nuevo = "";
            String Estatus_Cuenta_Anterior = "";
            String Cuota_Fija_Nueva = "";
            String Cuota_Fija_Anterior = "";
            //String Estatus_Orden_Nuevo = "";
            //String Estatus_Orden_Anterior = "";
            Boolean Variacion_Aceptada = false;
            Boolean Cancelando_Cuenta = false;
            Boolean Reactivando_Cuenta = false;
            //Boolean Estatus_Cuenta_Preasignado = false;

            if (Orden_Variacion.P_Cmmd != null)
            {
                Cmmd = Orden_Variacion.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmmd.Connection = Trans.Connection;
                Cmmd.Transaction = Trans;
            }

            try
            {
                Cuentas_Predial.P_Cuenta_Predial_ID = Orden_Variacion.P_Cuenta_Predial_ID;
                Orden_Variacion.P_Ignorar_Historial_Ordenes_Aceptadas = true;
                Dt_Variacion_Cuenta = Orden_Variacion.Obtener_Variacion_Cuenta();

                if (Dt_Variacion_Cuenta != null)
                {
                    if (Dt_Variacion_Cuenta.Rows.Count > 0)
                    {
                        DataRow[] Arr_Dr_Variacion_Cuenta;
                        Arr_Dr_Variacion_Cuenta = Dt_Variacion_Cuenta.Select("NOMBRE_CAMPO = '" + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta + "'");
                        if (Arr_Dr_Variacion_Cuenta.Length > 0)
                        {
                            if (Arr_Dr_Variacion_Cuenta[0]["DATO_NUEVO"] != DBNull.Value)
                            {
                                Estatus_Cuenta_Nuevo = Arr_Dr_Variacion_Cuenta[0]["DATO_NUEVO"].ToString();
                            }
                            if (Arr_Dr_Variacion_Cuenta[0]["DATO_ANTERIOR"] != DBNull.Value)
                            {
                                Estatus_Cuenta_Anterior = Arr_Dr_Variacion_Cuenta[0]["DATO_ANTERIOR"].ToString();
                            }
                        }
                        Arr_Dr_Variacion_Cuenta = null;
                        Arr_Dr_Variacion_Cuenta = Dt_Variacion_Cuenta.Select("NOMBRE_CAMPO = '" + Ope_Pre_Ordenes_Variacion.Campo_No_Cuota_Fija + "'");
                        if (Arr_Dr_Variacion_Cuenta.Length > 0)
                        {
                            if (Arr_Dr_Variacion_Cuenta[0]["DATO_NUEVO"] != DBNull.Value)
                            {
                                Cuota_Fija_Nueva = Arr_Dr_Variacion_Cuenta[0]["DATO_NUEVO"].ToString();
                            }
                            if (Arr_Dr_Variacion_Cuenta[0]["DATO_ANTERIOR"] != DBNull.Value)
                            {
                                Cuota_Fija_Anterior = Arr_Dr_Variacion_Cuenta[0]["DATO_ANTERIOR"].ToString();
                            }
                        }
                    }
                }

                Cuentas_Predial.P_Cmmd = Cmmd;
                Cuentas_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado;

                if (Orden_Variacion.P_Cuenta_Predial_ID.Trim() != "")
                {
                    Cuentas_Predial.P_Dt_Variacion_Cuenta = Dt_Variacion_Cuenta;
                    Variacion_Aceptada = Cuentas_Predial.Aplicar_Variacion_Cuenta();
                    //Variacion_Aceptada = Cuentas_Predial.Modifcar_Cuenta();
                }
                else
                {
                    Variacion_Aceptada = Cuentas_Predial.Alta_Cuenta();
                }
                Cuentas_Predial = null;

                if (Estatus_Cuenta_Nuevo == "CANCELADA")
                {
                    Cancelando_Cuenta = true;
                    Cancelar_Adeudos_Cuenta(Orden_Variacion);
                    Cancelar_Convenios_Cuenta(Orden_Variacion);
                }
                else
                {
                    if (Estatus_Cuenta_Nuevo == "VIGENTE"
                        && Estatus_Cuenta_Anterior == "CANCELADA")
                    {
                        Reactivando_Cuenta = true;
                        Reactivar_Convenios_Cuenta(Orden_Variacion);
                        Reactivar_Adeudos_Cuenta(Orden_Variacion);
                    }
                }

                if (Variacion_Aceptada
                    && (!Cancelando_Cuenta && !Reactivando_Cuenta))
                {
                    DataSet Ds_Propietarios;
                    DataSet Ds_Copropietarios_Variacion;
                    DataTable Dt_Copropietarios_Variacion;
                    DataTable Dt_Copropietarios = new DataTable();
                    DataTable Dt_Copropietarios_Recorridos = new DataTable();
                    DataTable Dt_Diferencias = new DataTable();

                    //Consulta los Datos de la Variación del Propietario.
                    Orden_Variacion.P_Propietario_Filtra_Estatus = true;
                    Ds_Propietarios = Consultar_Propietarios_Variacion(Orden_Variacion);
                    if (Ds_Propietarios != null)
                    {
                        if (Ds_Propietarios.Tables.Count > 0)
                        {
                            if (Ds_Propietarios.Tables[0].Rows.Count > 0)
                            {
                                Orden_Variacion.P_Propietario_Cuenta_Predial_ID = Orden_Variacion.P_Cuenta_Predial_ID;
                                Orden_Variacion.P_Propietario_Propietario_ID = Ds_Propietarios.Tables[0].Rows[0]["CONTRIBUYENTE_ID"].ToString().Trim();
                                Orden_Variacion.P_Propietario_Tipo = Ds_Propietarios.Tables[0].Rows[0]["TIPO_PROPIETARIO"].ToString().Trim();
                            }
                        }
                    }

                    //Consulta los Datos de la Variación de los Copropietarios.
                    Orden_Variacion.P_Copropietario_Filtra_Estatus = true;
                    Ds_Copropietarios_Variacion = Consultar_Copropietarios_Variacion(Orden_Variacion);
                    if (Ds_Copropietarios_Variacion != null)
                    {
                        if (Ds_Copropietarios_Variacion.Tables.Count > 0)
                        {
                            Dt_Copropietarios_Variacion = Ds_Copropietarios_Variacion.Tables[0];
                            if (Dt_Copropietarios_Variacion.Rows.Count > 0)
                            {
                                DataTable Dt_Temp_Copropietarios = new DataTable();
                                Dt_Temp_Copropietarios.Columns.Add(new DataColumn("CONTRIBUYENTE_ID", typeof(String)));
                                Dt_Temp_Copropietarios.Columns.Add(new DataColumn("RFC", typeof(String)));
                                Dt_Temp_Copropietarios.Columns.Add(new DataColumn("NOMBRE_CONTRIBUYENTE", typeof(String)));
                                Dt_Temp_Copropietarios.Columns.Add(new DataColumn("ESTATUS_VARIACION", typeof(String)));

                                DataRow Dr_Temp_Copropietario;
                                foreach (DataRow Copropietario_Variacion in Dt_Copropietarios_Variacion.Rows)
                                {
                                    Dr_Temp_Copropietario = Dt_Temp_Copropietarios.NewRow();
                                    Dr_Temp_Copropietario["CONTRIBUYENTE_ID"] = Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString();
                                    Dr_Temp_Copropietario["RFC"] = Copropietario_Variacion["RFC"].ToString();
                                    Dr_Temp_Copropietario["NOMBRE_CONTRIBUYENTE"] = Copropietario_Variacion["NOMBRE_CONTRIBUYENTE"].ToString();
                                    Dr_Temp_Copropietario["ESTATUS_VARIACION"] = "NUEVO";
                                    Dt_Temp_Copropietarios.Rows.Add(Dr_Temp_Copropietario);
                                }
                                Dt_Copropietarios_Recorridos = Dt_Temp_Copropietarios;
                                Dt_Copropietarios.Columns.Add(new DataColumn("CONTRIBUYENTE_ID", typeof(String)));

                                DataRow Dr_Copropietarios;
                                for (int Cont_Fila = 0; Cont_Fila < Dt_Copropietarios_Recorridos.Rows.Count; Cont_Fila++)
                                {
                                    Dr_Copropietarios = Dt_Copropietarios.NewRow();
                                    Dr_Copropietarios["CONTRIBUYENTE_ID"] = Dt_Copropietarios_Recorridos.Rows[Cont_Fila]["CONTRIBUYENTE_ID"].ToString().Trim();
                                    Dt_Copropietarios.Rows.Add(Dr_Copropietarios);
                                }
                                Orden_Variacion.P_Copropietario_Cuenta_Predial_ID = Orden_Variacion.P_Cuenta_Predial_ID;
                                Orden_Variacion.P_Dt_Copropietarios = Dt_Copropietarios;
                                Orden_Variacion.P_Copropietario_Tipo = Ds_Copropietarios_Variacion.Tables[0].Rows[0]["TIPO_PROPIETARIO"].ToString().Trim();
                            }
                        }
                    }

                    //Consulta los Datos de la Variación de las Diferencias
                    Dt_Diferencias.Columns.Add(new DataColumn("CUOTA_ANUAL", typeof(Decimal)));
                    Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_1", typeof(Decimal)));
                    Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_2", typeof(Decimal)));
                    Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_3", typeof(Decimal)));
                    Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_4", typeof(Decimal)));
                    Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_5", typeof(Decimal)));
                    Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_6", typeof(Decimal)));
                    Dt_Diferencias.Columns.Add(new DataColumn("AÑO", typeof(int)));
                    Dt_Diferencias.Columns.Add(new DataColumn("ALTA_BAJA", typeof(String)));

                    String Periodo = "";
                    Decimal Sum_Adeudos_Año = 0;
                    Decimal Sum_Adeudos_Periodo = 0;
                    int Cont_Cuotas_Minimas_Año = 0;
                    int Cont_Cuotas_Minimas_Periodo = 0;
                    int Cont_Adeudos_Año = 0;
                    int Cont_Adeudos_Periodo = 0;
                    int Desde_Bimestre = 0;
                    int Hasta_Bimestre = 0;
                    int Cont_Bimestres = 0;
                    int Año_Periodo = 0;
                    int Signo = 1;
                    Boolean Periodo_Corriente_Validado = false;
                    Boolean Periodo_Rezago_Validado = false;
                    Decimal Importe_Rezago = 0;
                    Decimal Cuota_Fija = 0;
                    Decimal Cuota_Minima_Año = 0;
                    Decimal Cuota_Anual = 0;
                    Boolean Nueva_Cuota_Fija = false;
                    Decimal Valor_Fiscal = 0;
                    Decimal Tasa_Diferencias = 0;
                    //Decimal Cuota_Fija_Nueva = 0;
                    //Decimal Cuota_Fija_Anterior = 0;
                    Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
                    Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
                    DataTable Dt_Adeudos_Cuenta = new DataTable();

                    Orden_Variacion.P_Generar_Orden_No_Orden = Orden_Variacion.P_Orden_Variacion_ID;
                    Orden_Variacion.P_Generar_Orden_Anio = Orden_Variacion.P_Año.ToString();
                    DataTable Dt_Agregar_Diferencias = Consulta_Diferencias(Orden_Variacion);// Orden_Variacion.Consulta_Diferencias();
                    if (Dt_Agregar_Diferencias != null)
                    {
                        Dt_Agregar_Diferencias.DefaultView.Sort = "TIPO DESC";
                        Dt_Agregar_Diferencias = Dt_Agregar_Diferencias.DefaultView.ToTable();
                    }

                    if (Cuota_Fija_Nueva != ""
                        && Cuota_Fija_Nueva != Cuota_Fija_Anterior)
                    {
                        Nueva_Cuota_Fija = true;
                    }

                    if (Dt_Agregar_Diferencias != null)
                    {
                        for (int Cont_Fila = 0; Cont_Fila < Dt_Agregar_Diferencias.Rows.Count; Cont_Fila++)
                        {
                            if (Dt_Agregar_Diferencias.Rows[Cont_Fila]["TIPO"] != DBNull.Value)
                            {
                                if (Dt_Agregar_Diferencias.Rows[Cont_Fila]["TIPO"].ToString().Trim() == "ALTA")
                                {
                                    Signo = 1;
                                }
                                else
                                {
                                    if (Dt_Agregar_Diferencias.Rows[Cont_Fila]["TIPO"].ToString().Trim() == "BAJA")
                                    {
                                        Signo = -1;
                                    }
                                }
                            }

                            Cuota_Anual = Convert.ToDecimal(Dt_Agregar_Diferencias.Rows[Cont_Fila]["Cuota_Bimestral"]) * 6;
                            Año_Periodo = Convert.ToInt32(Dt_Agregar_Diferencias.Rows[Cont_Fila]["Periodo"].ToString().Substring(Dt_Agregar_Diferencias.Rows[Cont_Fila]["Periodo"].ToString().Trim().Length - 4, 4));
                            Cuota_Minima_Año = Cuotas_Minimas.Consultar_Cuota_Minima_Anio(Año_Periodo.ToString());
                            Importe_Rezago = Convert.ToDecimal(Dt_Agregar_Diferencias.Rows[Cont_Fila]["Importe"].ToString().Replace("$", ""));
                            Valor_Fiscal = Convert.ToDecimal(Dt_Agregar_Diferencias.Rows[Cont_Fila][Ope_Pre_Diferencias_Detalle.Campo_Valor_Fiscal].ToString().Replace("$", ""));
                            Tasa_Diferencias = Convert.ToDecimal(Dt_Agregar_Diferencias.Rows[Cont_Fila]["TASA"].ToString().Replace("$", "")) / 1000;
                            Periodo = Obtener_Periodos_Bimestre(Dt_Agregar_Diferencias.Rows[Cont_Fila]["Periodo"].ToString().Trim(), out Periodo_Corriente_Validado, out Periodo_Rezago_Validado);
                            if (Periodo.Trim() != "")
                            {
                                Desde_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(0));
                                Hasta_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(1));

                                //Cuotas_Minimas_Encontradas_Año = false;
                                Cont_Cuotas_Minimas_Año = 0;
                                Cont_Adeudos_Año = 0;
                                Sum_Adeudos_Año = 0;
                                //Cuotas_Minimas_Encontradas_Periodo = false;
                                Cont_Cuotas_Minimas_Periodo = 0;
                                Cont_Adeudos_Periodo = 0;
                                Sum_Adeudos_Periodo = 0;

                                Dt_Adeudos_Cuenta = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Orden_Variacion.P_Cuenta_Predial_ID, null, Año_Periodo, Año_Periodo);
                                if (Dt_Adeudos_Cuenta != null)
                                {
                                    if (Dt_Adeudos_Cuenta.Rows.Count > 0)
                                    {
                                        //Contador de los Adeudos/Cuotas del Año
                                        for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                                        {
                                            if (Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres] != System.DBNull.Value)
                                            {
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) == Cuota_Minima_Año)
                                                {
                                                    Cont_Cuotas_Minimas_Año += 1;
                                                }
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) != 0)
                                                {
                                                    Cont_Adeudos_Año += 1;
                                                    Sum_Adeudos_Año += Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]);
                                                }
                                            }
                                        }
                                        //Contador de los Adeudos/Cuotas del Periodo indicado
                                        for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                        {
                                            if (Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres] != System.DBNull.Value)
                                            {
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) == Cuota_Minima_Año)
                                                {
                                                    Cont_Cuotas_Minimas_Periodo += 1;
                                                }
                                                if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) != 0)
                                                {
                                                    Cont_Adeudos_Periodo += 1;
                                                    Sum_Adeudos_Periodo += Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]);
                                                }
                                            }
                                        }
                                    }
                                }

                                DataRow Dr_Diferencias;
                                Dr_Diferencias = Dt_Diferencias.NewRow();
                                Dr_Diferencias["CUOTA_ANUAL"] = Cuota_Anual;
                                Dr_Diferencias["AÑO"] = Año_Periodo;
                                Dr_Diferencias["ALTA_BAJA"] = Dt_Agregar_Diferencias.Rows[Cont_Fila]["TIPO"].ToString().Trim();
                                //VALIDACIONES PARA CASOS DE CUOTAS MÍNIMAS Y APLICACIÓN DE ADEUDOS
                                //if (Cont_Cuotas_Minimas_Periodo == 1 && Importe_Rezago != Cuota_Minima_Año && !Nueva_Cuota_Fija)
                                //{
                                //    Dr_Diferencias["ALTA_BAJA"] = "SOB";
                                //    //SUMA LA CUOTA MÍNIMA AL IMPORTE Y EL RESULTADO LO PRORRATEA EN EL PERIODO INDICADO
                                //    for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                //    {
                                //        Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = ToDecimal((Importe_Rezago + Cuota_Minima_Año) / (Hasta_Bimestre - Desde_Bimestre + 1) * Signo);
                                //    }
                                //}
                                //else
                                {
                                    if (((Importe_Rezago == Cuota_Minima_Año)
                                            || (((Sum_Adeudos_Periodo - Importe_Rezago) == Cuota_Minima_Año && Signo < 0)))
                                        && !Nueva_Cuota_Fija
                                        && !(Importe_Rezago == Cuota_Minima_Año && (Hasta_Bimestre - Desde_Bimestre + 1) == 1))
                                    {
                                        //APLICA LA CUOTA MÍNIMA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CEROS
                                        if (Importe_Rezago == Cuota_Minima_Año || Signo > 0)
                                        {
                                            Dr_Diferencias["ALTA_BAJA"] = "SOB1";
                                            if (Signo > 0)
                                            {
                                                Dr_Diferencias["ALTA_BAJA"] = "SOB2";
                                            }
                                        }
                                        else
                                        {
                                            Dr_Diferencias["ALTA_BAJA"] = "SOB";
                                        }
                                        for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                        {
                                            if (Cont_Bimestres > Desde_Bimestre && Cuota_Minima_Año != 0)
                                            {
                                                Cuota_Minima_Año = 0;
                                            }
                                            if (Importe_Rezago == Cuota_Minima_Año || Signo > 0)
                                            {
                                                Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = Convert.ToDecimal(Convert.ToDecimal(Cuota_Minima_Año * Signo).ToString("0.00"));
                                            }
                                            else
                                            {
                                                Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = Convert.ToDecimal(Convert.ToDecimal(Cuota_Minima_Año).ToString("0.00"));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((Valor_Fiscal * Tasa_Diferencias) <= Cuota_Minima_Año
                                            && (Sum_Adeudos_Periodo + Importe_Rezago) == Cuota_Minima_Año
                                            && Signo > 0)
                                        {
                                            //APLICA LA CUOTA MÍNIMA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CEROS
                                            Dr_Diferencias["ALTA_BAJA"] = "SOB";
                                            for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                            {
                                                if (Cont_Bimestres > Desde_Bimestre && Cuota_Minima_Año != 0)
                                                {
                                                    Cuota_Minima_Año = 0;
                                                }
                                                Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = Convert.ToDecimal(Convert.ToDecimal(Cuota_Minima_Año * Signo).ToString("0.00"));
                                            }
                                        }
                                        else
                                        {
                                            if (Nueva_Cuota_Fija && Signo < 0)
                                            {
                                                //APLICA LA CUOTA FIJA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CERO
                                                Dr_Diferencias["ALTA_BAJA"] = "SOB";
                                                if (Cuota_Fija_Nueva != "")
                                                {
                                                    Cuota_Fija = Sum_Adeudos_Periodo - Importe_Rezago; //Convert.ToDecimal(Obtener_Dato_Consulta(Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija, Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas, Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " = '" + Cuota_Fija_Nueva + "'"));
                                                }
                                                for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                                {
                                                    if (Cont_Bimestres > Desde_Bimestre && Cuota_Minima_Año != 0)
                                                    {
                                                        Cuota_Fija = 0;
                                                    }
                                                    Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = Convert.ToDecimal(Convert.ToDecimal(Cuota_Fija).ToString("0.00"));
                                                }
                                            }
                                            else
                                            {
                                                //PRORRATEA EL IMPORTE EN EL PERIODO INDICADO
                                                for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                                {
                                                    Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = Convert.ToDecimal(Convert.ToDecimal(Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1) * Signo).ToString("0.00"));
                                                }
                                            }
                                        }
                                    }
                                }
                                Dt_Diferencias.Rows.Add(Dr_Diferencias);
                            }
                        }
                    }
                    Orden_Variacion.P_Dt_Diferencias = Dt_Diferencias;

                    Orden_Variacion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Orden_Variacion.P_Cmmd = Cmmd;
                    Aplicar_Variacion_Propietarios(Orden_Variacion);
                    Aplicar_Variacion_Copropietarios(Orden_Variacion);
                    Aplicar_Variacion_Diferencias(Orden_Variacion);

                    Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
                    Cuentas_Predial.P_Cmmd = Cmmd;
                    Cuentas_Predial.P_Adeudo_Predial_Cuenta_Predial_ID = Orden_Variacion.P_Cuenta_Predial_ID;
                    Cuentas_Predial.Validar_Estatus_Adeudos();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Aplicar la Variación de la Orden: " + ex.ToString());
            }

            return Variacion_Aceptada;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cancelar_Adeudos_Cuenta
        ///DESCRIPCIÓN          : Consulta los Adeudos de la Cuenta y los Inserta en Diferencias para su Baja
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 23/Octubre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Cancelar_Adeudos_Cuenta(Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion)
        {
            Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio GAP_Negocio = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
            DataTable Dt_Adeudos = null;
            DataTable Dt_Diferencias = null;
            DataRow Dr_Diferencias = null;
            Int16 Cont_Bimestres = 0;
            Int16 Desde_Bimestre = 0;
            Int16 Hasta_Bimestre = 0;
            Boolean Adeudos_Cancelados = false;

            try
            {
                Dt_Adeudos = Consultar_Adeudos_Predial(Orden_Variacion);
                if (Dt_Adeudos != null)
                {
                    if (Dt_Adeudos.Rows.Count > 0)
                    {
                        Dt_Diferencias = new DataTable();
                        Dt_Diferencias.Columns.Add(new DataColumn("VALOR_FISCAL", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("TIPO", typeof(String)));
                        Dt_Diferencias.Columns.Add(new DataColumn("IMPORTE", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("PERIODO", typeof(String)));
                        Dt_Diferencias.Columns.Add(new DataColumn("TASA_ID", typeof(String)));
                        Dt_Diferencias.Columns.Add(new DataColumn("TIPO_PERIODO", typeof(String)));
                        Dt_Diferencias.Columns.Add(new DataColumn("CUOTA_BIMESTRAL", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_1", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_2", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_3", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_4", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_5", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_6", typeof(Decimal)));

                        foreach (DataRow Dr_Adeudos in Dt_Adeudos.Rows)
                        {
                            Dr_Diferencias = Dt_Diferencias.NewRow();
                            if (Dr_Adeudos[Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual] != System.DBNull.Value)
                            {
                                Dr_Diferencias["VALOR_FISCAL"] = Convert.ToDecimal(Dr_Adeudos[Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual]);
                            }
                            Dr_Diferencias["TIPO"] = "ALTA";
                            if (Dr_Adeudos[Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual] != System.DBNull.Value)
                            {
                                Dr_Diferencias["IMPORTE"] = Convert.ToDecimal(Dr_Adeudos[Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual]);
                            }
                            if (Dr_Adeudos["AÑO"] != System.DBNull.Value)
                            {
                                for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                                {
                                    Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = Dr_Adeudos["BIMESTRE_" + Cont_Bimestres.ToString()];
                                    if (Dr_Adeudos["BIMESTRE_" + Cont_Bimestres.ToString()] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDecimal(Dr_Adeudos["BIMESTRE_" + Cont_Bimestres.ToString()]) != 0
                                            && Desde_Bimestre == 0)
                                        {
                                            Desde_Bimestre = Cont_Bimestres;
                                        }
                                    }
                                }
                                for (Cont_Bimestres = 6; Cont_Bimestres >= 1; Cont_Bimestres--)
                                {
                                    if (Dr_Adeudos["BIMESTRE_" + Cont_Bimestres.ToString()] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDecimal(Dr_Adeudos["BIMESTRE_" + Cont_Bimestres.ToString()]) != 0)
                                        {
                                            Hasta_Bimestre = Cont_Bimestres;
                                            break;
                                        }
                                    }
                                }
                                if (Desde_Bimestre > 0 && Hasta_Bimestre > 0)
                                {
                                    Dr_Diferencias["PERIODO"] = Desde_Bimestre.ToString() + "/" + Dr_Adeudos["AÑO"].ToString() + " - " + Hasta_Bimestre.ToString() + "/" + Dr_Adeudos["AÑO"].ToString();
                                }
                            }
                            Dt_Diferencias.Rows.Add(Dr_Diferencias);
                        }
                        Orden_Variacion.P_Generar_Orden_Cuenta_ID = Orden_Variacion.P_Cuenta_Predial_ID;
                        Orden_Variacion.P_Generar_Orden_Anio = DateTime.Now.Year.ToString();
                        Orden_Variacion.P_Generar_Orden_No_Orden = Orden_Variacion.P_Orden_Variacion_ID;
                        Orden_Variacion.P_Dt_Diferencias = Dt_Diferencias;
                        Orden_Variacion.P_Cancelando_Cuenta = true;
                        GAP_Negocio.Calcular_Recargos_Predial(Orden_Variacion.P_Cuenta_Predial_ID);
                        Orden_Variacion.P_Total_Recargos = GAP_Negocio.p_Total_Recargos_Generados;
                        Orden_Variacion.P_Total_Corriente = GAP_Negocio.p_Total_Corriente;
                        Orden_Variacion.P_Total_Rezago = GAP_Negocio.p_Total_Rezago;
                        Alta_Diferencias(Orden_Variacion);

                        Dt_Diferencias = new DataTable();
                        Dt_Diferencias.Columns.Add(new DataColumn("CUOTA_ANUAL", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_1", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_2", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_3", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_4", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_5", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_6", typeof(Decimal)));
                        Dt_Diferencias.Columns.Add(new DataColumn("AÑO", typeof(Int16)));

                        foreach (DataRow Dr_Adeudos in Dt_Adeudos.Rows)
                        {
                            Dr_Diferencias = Dt_Diferencias.NewRow();
                            if (Dr_Adeudos[Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual] != System.DBNull.Value)
                            {
                                Dr_Diferencias["CUOTA_ANUAL"] = Convert.ToDecimal(Dr_Adeudos[Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual]);
                            }
                            if (Dr_Adeudos["AÑO"] != System.DBNull.Value)
                            {
                                Dr_Diferencias["AÑO"] = Convert.ToInt16(Dr_Adeudos["AÑO"]);
                            }
                            for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                            {
                                Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] = 0;
                            }
                            Dt_Diferencias.Rows.Add(Dr_Diferencias);
                        }
                        Orden_Variacion.P_Diferencias_Estatus = "CANCELADO";
                        Orden_Variacion.P_Dt_Diferencias = Dt_Diferencias;
                        Orden_Variacion.P_Suma_Variacion_Diferencias = false;
                        Aplicar_Variacion_Diferencias(Orden_Variacion);
                    }
                }
                Adeudos_Cancelados = true;
            }
            catch (Exception ex)
            {
                Adeudos_Cancelados = false;
                throw new Exception(ex.Message);
            }
            return Adeudos_Cancelados;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Reactivar_Adeudos_Cuenta
        ///DESCRIPCIÓN          : Consulta las Diferencias de la Cuenta Cancelada y los Inserta en Adeudos para su Alta
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 23/Octubre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Reactivar_Adeudos_Cuenta(Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion)
        {
            DataTable Dt_Orden_Variacion = null;
            DataTable Dt_Diferencias = null;
            DataTable Dt_Adeudos = null;
            DataRow Dr_Adeudos = null;
            Int16 Cont_Bimestres = 0;
            Boolean Adeudos_Reactivados = false;
            String Periodo = "";
            Int16 Desde_Bimestre = 0;
            Int16 Hasta_Bimestre = 0;
            Boolean Periodo_Corriente_Validado = false;
            Boolean Periodo_Rezago_Validado = false;

            try
            {
                Dt_Orden_Variacion = Consultar_Ultima_Orden_Con_Adeudos(Orden_Variacion);
                if (Dt_Orden_Variacion != null)
                {
                    if (Dt_Orden_Variacion.Rows.Count > 0)
                    {
                        Orden_Variacion.P_Generar_Orden_No_Orden = Dt_Orden_Variacion.Rows[0][Ope_Pre_Diferencias.Campo_No_Orden_Variacion].ToString();
                        Orden_Variacion.P_Generar_Orden_Anio = Dt_Orden_Variacion.Rows[0][Ope_Pre_Diferencias.Campo_Anio].ToString();
                        Dt_Diferencias = Consulta_Diferencias(Orden_Variacion);
                        if (Dt_Diferencias != null)
                        {
                            if (Dt_Diferencias.Rows.Count > 0)
                            {
                                Dt_Adeudos = new DataTable();
                                Dt_Adeudos.Columns.Add(new DataColumn("CUOTA_ANUAL", typeof(Decimal)));
                                Dt_Adeudos.Columns.Add(new DataColumn("BIMESTRE_1", typeof(Decimal)));
                                Dt_Adeudos.Columns.Add(new DataColumn("BIMESTRE_2", typeof(Decimal)));
                                Dt_Adeudos.Columns.Add(new DataColumn("BIMESTRE_3", typeof(Decimal)));
                                Dt_Adeudos.Columns.Add(new DataColumn("BIMESTRE_4", typeof(Decimal)));
                                Dt_Adeudos.Columns.Add(new DataColumn("BIMESTRE_5", typeof(Decimal)));
                                Dt_Adeudos.Columns.Add(new DataColumn("BIMESTRE_6", typeof(Decimal)));
                                Dt_Adeudos.Columns.Add(new DataColumn("AÑO", typeof(Int16)));

                                foreach (DataRow Dr_Diferencias in Dt_Diferencias.Rows)
                                {
                                    Dr_Adeudos = Dt_Adeudos.NewRow();
                                    if (Dr_Diferencias["IMPORTE"] != System.DBNull.Value)
                                    {
                                        Dr_Adeudos["CUOTA_ANUAL"] = Convert.ToDecimal(Dr_Diferencias["IMPORTE"]);
                                    }
                                    if (Dr_Diferencias["PERIODO"] != System.DBNull.Value)
                                    {
                                        Dr_Adeudos["AÑO"] = Convert.ToInt16(Dr_Diferencias["PERIODO"].ToString().Substring(Dr_Diferencias["PERIODO"].ToString().Length - 4));
                                    }
                                    for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                                    {
                                        Dr_Adeudos["BIMESTRE_" + Cont_Bimestres.ToString()] = Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()];
                                    }
                                    Dt_Adeudos.Rows.Add(Dr_Adeudos);
                                }
                                Orden_Variacion.P_Diferencias_Estatus = "POR PAGAR";
                                Orden_Variacion.P_Dt_Diferencias = Dt_Adeudos;
                                Orden_Variacion.P_Suma_Variacion_Diferencias = false;
                                Orden_Variacion.P_Reactivando_Cuenta = true;
                                Orden_Variacion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                                Aplicar_Variacion_Diferencias(Orden_Variacion);
                            }
                        }
                    }
                }
                Adeudos_Reactivados = true;
            }
            catch (Exception ex)
            {
                Adeudos_Reactivados = false;
                throw new Exception(ex.Message);
            }
            return Adeudos_Reactivados;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cancelar_Convenios_Cuenta
        ///DESCRIPCIÓN          : Consulta los Convenios de la Cuenta, y en casos de encotnrar Convenios Activos los Cancela
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Marzo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Cancelar_Convenios_Cuenta(Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion)
        {
            Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio Convenios_Derechos_Supervision = new Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio();
            Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenios_Fraccionamientos = new Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio();
            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
            Cls_Ope_Pre_Convenios_Predial_Negocio Convenios_Predial = new Cls_Ope_Pre_Convenios_Predial_Negocio();
            Boolean Convenios_Cancelados = false;

            try
            {
                //Cancela los Convenios de Derechos de Supervisión
                //Convenios_Derechos_Supervision.P_Estatus = "CUENTA_CANCELADA";
                Convenios_Derechos_Supervision.P_Campos_Dinamicos = "";
                Convenios_Derechos_Supervision.P_Campos_Dinamicos += Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus_Cancelacion_Cuenta + " = " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus;
                Convenios_Derechos_Supervision.P_Campos_Dinamicos += ", " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus + " = 'CUENTA_CANCELADA'";
                Convenios_Derechos_Supervision.P_Filtros_Dinamicos = "";
                Convenios_Derechos_Supervision.P_Filtros_Dinamicos += Ope_Pre_Convenios_Derechos_Supervision.Campo_Cuenta_Predial_ID + " = '" + Orden_Variacion.P_Cuenta_Predial_ID + "' ";
                //Convenios_Derechos_Supervision.P_Filtros_Dinamicos += Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus + " = 'ACTIVO'";
                Convenios_Derechos_Supervision.P_Cmmd = Orden_Variacion.P_Cmmd;
                Convenios_Derechos_Supervision.Modificar_Estatus_Convenio_Reestructura();

                //Cancela los Convenios de Fraccionamientos
                //Convenios_Fraccionamientos.P_Estatus = "CUENTA_CANCELADA";
                Convenios_Fraccionamientos.P_Campos_Dinamicos = "";
                Convenios_Fraccionamientos.P_Campos_Dinamicos += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus_Cancelacion_Cuenta + " = " + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus;
                Convenios_Fraccionamientos.P_Campos_Dinamicos += ", " + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " = 'CUENTA_CANCELADA'";
                Convenios_Fraccionamientos.P_Filtros_Dinamicos = "";
                Convenios_Fraccionamientos.P_Filtros_Dinamicos += Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Orden_Variacion.P_Cuenta_Predial_ID + "' ";
                //Convenios_Fraccionamientos.P_Filtros_Dinamicos += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " = 'ACTIVO'";
                Convenios_Fraccionamientos.P_Cmmd = Orden_Variacion.P_Cmmd;
                Convenios_Fraccionamientos.Modificar_Estatus_Convenio_Reestructura();

                //Cancela los Convenios de Traslado de Dominio
                //Convenios_Traslado_Dominio.P_Estatus = "CUENTA_CANCELADA";
                Convenios_Traslado_Dominio.P_Campos_Dinamicos = "";
                Convenios_Traslado_Dominio.P_Campos_Dinamicos += Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus_Cancelacion_Cuenta + " = " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus;
                Convenios_Traslado_Dominio.P_Campos_Dinamicos += ", " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " = 'CUENTA_CANCELADA'";
                Convenios_Traslado_Dominio.P_Filtros_Dinamicos = "";
                Convenios_Traslado_Dominio.P_Filtros_Dinamicos += Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + " = '" + Orden_Variacion.P_Cuenta_Predial_ID + "' ";
                //Convenios_Traslado_Dominio.P_Filtros_Dinamicos += Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " = 'ACTIVO'";
                Convenios_Traslado_Dominio.P_Cmmd = Orden_Variacion.P_Cmmd;
                Convenios_Traslado_Dominio.Modificar_Estatus_Convenio_Reestructura();

                //Cancela los Convenios de Predial
                //Convenios_Predial.P_Estatus = "CUENTA_CANCELADA";
                Convenios_Predial.P_Campos_Dinamicos = "";
                Convenios_Predial.P_Campos_Dinamicos += Ope_Pre_Convenios_Predial.Campo_Estatus_Cancelacion_Cuenta + " = " + Ope_Pre_Convenios_Predial.Campo_Estatus;
                Convenios_Predial.P_Campos_Dinamicos += ", " + Ope_Pre_Convenios_Predial.Campo_Estatus + " = 'CUENTA_CANCELADA'";
                Convenios_Predial.P_Filtros_Dinamicos = "";
                Convenios_Predial.P_Filtros_Dinamicos += Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + " = '" + Orden_Variacion.P_Cuenta_Predial_ID + "' ";
                //Convenios_Predial.P_Filtros_Dinamicos += Ope_Pre_Convenios_Predial.Campo_Estatus + " = 'ACTIVO'";
                Convenios_Predial.P_Cmmd = Orden_Variacion.P_Cmmd;
                Convenios_Predial.Modificar_Estatus_Convenio_Reestructura();

                Convenios_Cancelados = true;
            }
            catch //(Exception ex)
            {
                //throw new Exception(ex.Message);
            }

            return Convenios_Cancelados;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Reactivar_Convenios_Cuenta
        ///DESCRIPCIÓN          : Consulta los Convenios de la Cuenta, y en casos de encotnrar Convenios Cancelados los Reactiva
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Marzo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Boolean Reactivar_Convenios_Cuenta(Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion)
        {
            Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio Convenios_Derechos_Supervision = new Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio();
            Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenios_Fraccionamientos = new Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio();
            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
            Cls_Ope_Pre_Convenios_Predial_Negocio Convenios_Predial = new Cls_Ope_Pre_Convenios_Predial_Negocio();
            Boolean Convenios_Reactivados = false;

            try
            {
                //Reactiva los Convenios de Derechos de Supervisión
                //Convenios_Derechos_Supervision.P_Estatus = "ACTIVO";
                Convenios_Derechos_Supervision.P_Campos_Dinamicos = "";
                Convenios_Derechos_Supervision.P_Campos_Dinamicos += Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus + " = " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus_Cancelacion_Cuenta;
                Convenios_Derechos_Supervision.P_Filtros_Dinamicos = "";
                Convenios_Derechos_Supervision.P_Filtros_Dinamicos += Ope_Pre_Convenios_Derechos_Supervision.Campo_Cuenta_Predial_ID + " = '" + Orden_Variacion.P_Cuenta_Predial_ID + "' ";
                //Convenios_Derechos_Supervision.P_Filtros_Dinamicos += Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus + " = 'CUENTA_CANCELADA'";
                Convenios_Derechos_Supervision.P_Cmmd = Orden_Variacion.P_Cmmd;
                Convenios_Derechos_Supervision.Modificar_Estatus_Convenio_Reestructura();

                //Reactiva los Convenios de Fraccionamientos
                //Convenios_Fraccionamientos.P_Estatus = "ACTIVO";
                Convenios_Fraccionamientos.P_Campos_Dinamicos = "";
                Convenios_Fraccionamientos.P_Campos_Dinamicos += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " = " + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus_Cancelacion_Cuenta;
                Convenios_Fraccionamientos.P_Filtros_Dinamicos = "";
                Convenios_Fraccionamientos.P_Filtros_Dinamicos += Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID + " = '" + Orden_Variacion.P_Cuenta_Predial_ID + "' ";
                //Convenios_Fraccionamientos.P_Filtros_Dinamicos += Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " = 'CUENTA_CANCELADA'";
                Convenios_Fraccionamientos.P_Cmmd = Orden_Variacion.P_Cmmd;
                Convenios_Fraccionamientos.Modificar_Estatus_Convenio_Reestructura();

                //Reactiva los Convenios de Traslado de Dominio
                //Convenios_Traslado_Dominio.P_Estatus = "ACTIVO";
                Convenios_Traslado_Dominio.P_Campos_Dinamicos = "";
                Convenios_Traslado_Dominio.P_Campos_Dinamicos += Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " = " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus_Cancelacion_Cuenta;
                Convenios_Traslado_Dominio.P_Filtros_Dinamicos = "";
                Convenios_Traslado_Dominio.P_Filtros_Dinamicos += Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + " = '" + Orden_Variacion.P_Cuenta_Predial_ID + "' ";
                //Convenios_Traslado_Dominio.P_Filtros_Dinamicos += Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " = 'CUENTA_CANCELADA'";
                Convenios_Traslado_Dominio.P_Cmmd = Orden_Variacion.P_Cmmd;
                Convenios_Traslado_Dominio.Modificar_Estatus_Convenio_Reestructura();

                //Reactiva los Convenios de Predial
                //Convenios_Predial.P_Estatus = "ACTIVO";
                Convenios_Predial.P_Campos_Dinamicos = "";
                Convenios_Predial.P_Campos_Dinamicos += Ope_Pre_Convenios_Predial.Campo_Estatus + " = " + Ope_Pre_Convenios_Predial.Campo_Estatus_Cancelacion_Cuenta;
                Convenios_Predial.P_Filtros_Dinamicos = "";
                Convenios_Predial.P_Filtros_Dinamicos += Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id + " = '" + Orden_Variacion.P_Cuenta_Predial_ID + "' ";
                //Convenios_Predial.P_Filtros_Dinamicos += Ope_Pre_Convenios_Predial.Campo_Estatus + " = 'CUENTA_CANCELADA'";
                Convenios_Predial.P_Cmmd = Orden_Variacion.P_Cmmd;
                Convenios_Predial.Modificar_Estatus_Convenio_Reestructura();

                Convenios_Reactivados = true;
            }
            catch
            {
            }

            return Convenios_Reactivados;
        }

        #region Aplicar Variacion

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Aplicar_Variacion
        ///DESCRIPCIÓN: por cada variacion en el datatable de variaciones actualiza la cuenta predial
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/13/2011 10:04:43 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************   
        internal static int Aplicar_Variacion(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            
            int Columnas_Afectadas = 0;

            try
            {
                foreach (DataRow Variacion in Datos.P_Generar_Orden_Dt_Detalles.Rows)
                {
                    Mi_SQL = "";
                    Mi_SQL = "UPDATE ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " SET ";
                    Mi_SQL = Mi_SQL + Variacion["CAMPO"] + " = '";
                    Mi_SQL = Mi_SQL + Variacion["DATO_NUEVO"] + "', ";
                    if (Datos.P_Tipo_Suspencion_Cuenta_Predial != null && Datos.P_Tipo_Suspencion_Cuenta_Predial.Trim().Length > 0)
                    {
                        Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + "='" + Datos.P_Tipo_Suspencion_Cuenta_Predial + "',";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + "= NULL,";
                    }
                    Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Fecha_Modifico + " = SYSDATE, ";
                    Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Campo_Usuario_Modifico + " ='";
                    Mi_SQL = Mi_SQL + Presidencia.Sessiones.Cls_Sessiones.Nombre_Empleado + "'";

                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Generar_Orden_Cuenta_ID + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    Columnas_Afectadas++;
                }
                return Columnas_Afectadas;
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
        #endregion
        #endregion

        #region[Modificaciones a Orden de Variacion]
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Orden
        ///DESCRIPCIÓN: Cambia estatuas de la orden a cancelado
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 22/Ago/2011 8:03:39 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        public static void Eliminar_Orden(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Actualizar = false;

            if (Datos.P_Cmmd != null)
            {
                Cmd = Datos.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                Mi_SQL += " SET " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'CANCELADA'";
                if (!String.IsNullOrEmpty(Datos.P_Orden_Variacion_ID))
                {
                    Mi_SQL += " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Datos.P_Orden_Variacion_ID + "' ";
                    Mi_SQL += " AND " + Ope_Pre_Contrarecibos.Campo_Anio + " = " + Datos.P_Año;

                }
                else if (!String.IsNullOrEmpty(Datos.P_Contrarecibo))
                {
                    Mi_SQL += " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Datos.P_Contrarecibo + "' ";
                    Mi_SQL += " AND " + Ope_Pre_Contrarecibos.Campo_Anio + " = " + Datos.P_Contrarecibo_Anio;
                }

                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                if (Datos.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Actualizar = true;
            }
            catch (OracleException Ex)
            {
                if (Datos.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
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
                    Mensaje = "Error al intentar modificar un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Datos.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Ordene_Variacion
        ///DESCRIPCIÓN          : Actualiza el resitro indicado de la Orden de Variación
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 21/Agosto/2011
        ///MODIFICO             : Antonio Salvador Benavides Guardado
        ///FECHA_MODIFICO       : 15/Febrero/2012
        ///CAUSA_MODIFICACIÓN   : Cambio en la estructura de la Tabla de Órdenes de Variación
        ///*******************************************************************************
        public static Boolean Modificar_Orden_Variacion(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Actualizar = false;

            if (Ordenes_Variacion.P_Cmmd != null)
            {
                //Trans = Ordenes_Variacion.P_Trans;
                Cmd = Ordenes_Variacion.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }
            //Cmd.Connection = Trans.Connection;
            //Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " SET ";
                if (Ordenes_Variacion.P_Generar_Orden_Anio != "" && Ordenes_Variacion.P_Generar_Orden_Anio != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Ordenes_Variacion.P_Generar_Orden_Anio + ", ";
                }
                if (Ordenes_Variacion.P_Generar_Orden_Estatus != "" && Ordenes_Variacion.P_Generar_Orden_Estatus != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = '" + Ordenes_Variacion.P_Generar_Orden_Estatus + "', ";
                }
                if (Ordenes_Variacion.P_Estatus_Cuenta != "" && Ordenes_Variacion.P_Estatus_Cuenta != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta + " = '" + Ordenes_Variacion.P_Estatus_Cuenta + "', ";
                }
                if (Ordenes_Variacion.P_Generar_Orden_Obserbaciones != "" && Ordenes_Variacion.P_Generar_Orden_Obserbaciones != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Observaciones + " = UPPER('" + Ordenes_Variacion.P_Generar_Orden_Obserbaciones + "'), ";
                }
                if (Ordenes_Variacion.P_Contrarecibo != "" && Ordenes_Variacion.P_Contrarecibo != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " = '" + Ordenes_Variacion.P_Contrarecibo + "', ";
                }
                if (Ordenes_Variacion.P_No_Nota != 0)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Nota + " = " + Ordenes_Variacion.P_No_Nota + ", ";
                }
                if (Ordenes_Variacion.P_Fecha_Nota != null)
                {
                    if (Ordenes_Variacion.P_Fecha_Nota > DateTime.MinValue)
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Nota + " = '" + Ordenes_Variacion.P_Fecha_Nota.ToString("d-M-yyyy") + "', ";
                    }
                }
                if (Ordenes_Variacion.P_Grupo_Movimiento_ID != "" && Ordenes_Variacion.P_Grupo_Movimiento_ID != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + " = '" + Ordenes_Variacion.P_Grupo_Movimiento_ID + "', ";
                }
                if (Ordenes_Variacion.P_Tipo_Predio_ID != "" && Ordenes_Variacion.P_Tipo_Predio_ID != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + " = '" + Ordenes_Variacion.P_Tipo_Predio_ID + "', ";
                }
                if (Ordenes_Variacion.P_No_Nota_Impreso != "" && Ordenes_Variacion.P_No_Nota_Impreso != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Numero_Nota_Impreso + " = '" + Ordenes_Variacion.P_No_Nota_Impreso + "', ";
                }
                if (Ordenes_Variacion.P_Ciudad_ID_Notificacion != "" && Ordenes_Variacion.P_Ciudad_ID_Notificacion != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Ciudad_ID_Notificacion + " = '" + Ordenes_Variacion.P_Ciudad_ID_Notificacion + "', ";
                }
                if (Ordenes_Variacion.P_Estado_ID_Notificacion != "" && Ordenes_Variacion.P_Estado_ID_Notificacion != null)
                {
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Estado_ID_Notificacion + " = '" + Ordenes_Variacion.P_Estado_ID_Notificacion + "', ";
                }
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Usuario_Modifico + " = '" + Ordenes_Variacion.P_Usuario + "', ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Modifico + " = SYSDATE, ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Usuario_Valido + " = '" + Ordenes_Variacion.P_Usuario_Valido + "' ";
                if (Ordenes_Variacion.P_Fecha_Valido != "" && Ordenes_Variacion.P_Fecha_Valido != null)
                {
                    Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Valido + " = " + Ordenes_Variacion.P_Fecha_Valido;
                }
                Mi_SQL += " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = '" + Ordenes_Variacion.P_Orden_Variacion_ID + "' AND ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Anio + " = '" + Ordenes_Variacion.P_Año + "' AND ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '" + Ordenes_Variacion.P_Cuenta_Predial_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Actualizar = true;
            }
            catch (OracleException Ex)
            {
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
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
                    Mensaje = "Error al intentar modificar un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Actualizar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Orden_Variacion_Generada
        ///DESCRIPCIÓN          : Actualiza el resitro indicado de la Orden de Variación
        ///PARAMETROS           : Ordenes_Variacion, instancia de la capa de Negocios
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 21/Agosto/2011
        ///MODIFICO:            : Jesus Toledo Rdz
        ///FECHA_MODIFICO       : 08/Feb/2012
        ///CAUSA_MODIFICACIÓN   : Cambio en la estructura de la tabla
        ///*******************************************************************************
        public static Boolean Modificar_Orden_Variacion_Generada(Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion)
        {
            String Mensaje = "";
            String Mi_SQL_2 = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Actualizar = false;
            Object Aux; //Variable auxiliar para las consultas            
            String No_Detalle;
            int Resultado_Count = 0;
            String Diferencia_ID = null;
            String Diferencia_Detalle_ID = null;
            DataTable Campo;
            String Consec = ""; //Variable para la consulta SQL
            DataTable Dt_Copropietarios_Variacion;
            Boolean Propietario_Poseedor_Eliminados;
            if (Ordenes_Variacion.P_Cmmd != null)
            {
                //Trans = Ordenes_Variacion.P_Trans;
                Cmd = Ordenes_Variacion.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Trans.Connection;
                Cmd.Transaction = Trans;
            }
            //Cmd.Connection = Trans.Connection;
            //Cmd.Transaction = Trans;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " SET ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + " = '" + Ordenes_Variacion.P_Generar_Orden_Movimiento_ID + "' ";
                if (Ordenes_Variacion.P_Generar_Orden_Anio != "" && Ordenes_Variacion.P_Generar_Orden_Anio != null)
                {
                    Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Ordenes_Variacion.P_Generar_Orden_Anio;
                }
                if (Ordenes_Variacion.P_Generar_Orden_Estatus != "" && Ordenes_Variacion.P_Generar_Orden_Estatus != null)
                {
                    Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = '" + Ordenes_Variacion.P_Generar_Orden_Estatus + "'";
                }
                if (Ordenes_Variacion.P_Generar_Orden_Obserbaciones != "" && Ordenes_Variacion.P_Generar_Orden_Obserbaciones != null)
                {
                    Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Observaciones + " = '" + Ordenes_Variacion.P_Generar_Orden_Obserbaciones + "'";
                }
                else
                {
                    Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Observaciones + " = NULL ";
                }
                if (Ordenes_Variacion.P_Contrarecibo != "" && Ordenes_Variacion.P_Contrarecibo != null)
                {
                    Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " = '" + Ordenes_Variacion.P_Contrarecibo + "'";
                }
                if (Ordenes_Variacion.P_Grupo_Movimiento_ID != "" && Ordenes_Variacion.P_Grupo_Movimiento_ID != null)
                {
                    Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + " = '" + Ordenes_Variacion.P_Grupo_Movimiento_ID + "'";
                }
                //if (Ordenes_Variacion.P_Tipo_Predio_ID != "" && Ordenes_Variacion.P_Tipo_Predio_ID != null)
                //{
                //    Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + " = '" + Ordenes_Variacion.P_Tipo_Predio_ID + "' ";
                //}
                //Segmento de Codigo para insertar detalles de la orden
                //INSERTA LAS VARIACIONES ALMACENADAS EN EL DATATABLE DE VARIACIONES A LA TABLA DE DETALLES DE LA ORDEN
                foreach (DataRow Variacion in Ordenes_Variacion.P_Generar_Orden_Dt_Detalles.Rows)
                {
                    //Formar Sentencia de consulta de NOMBRE DE COLUMNA PARA VERIFICAR SI EXISTE
                    Mi_SQL_2 = "";
                    Mi_SQL_2 = Mi_SQL_2 + "SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = ";
                    Mi_SQL_2 = Mi_SQL_2 + "'" + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "' AND COLUMN_NAME = ";
                    Mi_SQL_2 = Mi_SQL_2 + "'" + Variacion["CAMPO"].ToString() + "'";
                    //Ejecutar consulta
                    Campo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL_2).Tables[0];

                    if (Campo.Rows.Count > 0 && !Variacion["CAMPO"].ToString().Contains("CUENTA_PREDIAL") && !Variacion["CAMPO"].ToString().Contains("CREO") && !Variacion["CAMPO"].ToString().Contains("MODIFICO"))
                    {
                        if (!Variacion["DATO_NUEVO"].ToString().Trim().Contains("01/01/0001") && !String.IsNullOrEmpty(Variacion["DATO_NUEVO"].ToString()))
                        {
                            if (Variacion["CAMPO"].ToString().Trim().Contains("AVALUO") || Variacion["CAMPO"].ToString().Trim().Contains("TERMINO_EXENCION"))
                                Mi_SQL = Mi_SQL + "," + Variacion["CAMPO"].ToString() + " = '" + Convert.ToDateTime(Variacion["DATO_NUEVO"].ToString()).ToString("dd/MM/yyyy") + "'";
                            else
                                Mi_SQL = Mi_SQL + "," + Variacion["CAMPO"].ToString() + " = '" + Variacion["DATO_NUEVO"].ToString() + "'";
                        }
                        else if (Variacion["CAMPO"].ToString().Trim() == Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion || Variacion["CAMPO"].ToString().Trim() == Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion || Variacion["CAMPO"].ToString().Trim() == Ope_Pre_Ordenes_Variacion.Campo_Estado_Notificacion || Variacion["CAMPO"].ToString().Trim() == Ope_Pre_Ordenes_Variacion.Campo_Estado_ID_Notificacion || Variacion["CAMPO"].ToString().Trim() == Ope_Pre_Ordenes_Variacion.Campo_Ciudad_ID_Notificacion || Variacion["CAMPO"].ToString().Trim() == Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija)
                        {
                            Mi_SQL = Mi_SQL + "," + Variacion["CAMPO"].ToString() + " = NULL";
                        }
                        else
                        {
                            Mi_SQL = Mi_SQL + "," + Variacion["CAMPO"].ToString() + " = NULL";
                        }

                    }
                }

                Mi_SQL += "," + Ope_Pre_Ordenes_Variacion.Campo_Usuario_Modifico + " = '" + Ordenes_Variacion.P_Usuario + "' ";
                Mi_SQL += "," + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Modifico + " = SYSDATE ";

                Mi_SQL += "WHERE " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = '" + Ordenes_Variacion.P_Generar_Orden_No_Orden + "'";
                Mi_SQL += " AND  " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = '" + Ordenes_Variacion.P_Generar_Orden_Anio + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                if (Ordenes_Variacion.P_Dt_Diferencias.Rows.Count > 0)
                {
                    //Formar Sentencia de consulta de consecutivo de la tabla diferencias o rezago
                    Mi_SQL = "";
                    Mi_SQL = "SELECT COUNT(1) ";
                    Mi_SQL = Mi_SQL + " FROM ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias;
                    Mi_SQL = Mi_SQL + " WHERE ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Orden_Variacion;
                    Mi_SQL = Mi_SQL + " = '";
                    Mi_SQL = Mi_SQL + Ordenes_Variacion.P_Generar_Orden_No_Orden;
                    Mi_SQL = Mi_SQL + "' AND ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_Anio;
                    Mi_SQL = Mi_SQL + " = '";
                    Mi_SQL = Mi_SQL + Ordenes_Variacion.P_Generar_Orden_Anio + "'";


                    //Ejecutar consulta
                    Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                    //Verificar si no es nulo
                    if (Convert.IsDBNull(Aux) == false)
                    {
                        Resultado_Count = Convert.ToInt32(Aux);
                    }

                    if (Resultado_Count <= 0)
                    {
                        //Formar Sentencia de consulta de consecutivo de la tabla diferencias o rezago
                        Mi_SQL = "";
                        Mi_SQL = "SELECT NVL(MAX(";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia + "),0000000000)";
                        Mi_SQL = Mi_SQL + " FROM ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias;

                        //Ejecutar consulta

                        Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                        //Verificar si no es nulo
                        if (Convert.IsDBNull(Aux) == false)
                        {
                            Diferencia_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                        }
                        else
                            Diferencia_ID = "0000000001";
                        //Insertar Registro de Diferencias
                        Mi_SQL = "";
                        Mi_SQL = "INSERT INTO ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + " ( ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia + ",";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_Anio + ", ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Orden_Variacion + ") ";
                        Mi_SQL = Mi_SQL + "VALUES('";
                        Mi_SQL = Mi_SQL + Diferencia_ID + "','";
                        Mi_SQL = Mi_SQL + Ordenes_Variacion.P_Generar_Orden_Cuenta_ID + "', ";
                        Mi_SQL = Mi_SQL + Ordenes_Variacion.P_Generar_Orden_Anio + ", '";
                        Mi_SQL = Mi_SQL + Ordenes_Variacion.P_Generar_Orden_No_Orden + "') ";
                        //Ejecutar consulta
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();

                        //Formar Sentencia de consulta de consecutivo de la tabla de detalles diferencias o rezago
                        Mi_SQL = "";
                        Mi_SQL = "SELECT NVL(MAX(";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencias_Detalles + "),0000000000)";
                        Mi_SQL = Mi_SQL + " FROM ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle;

                        //Ejecutar consulta

                        Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                        //Verificar si no es nulo
                        if (Convert.IsDBNull(Aux) == false)
                        {
                            Diferencia_Detalle_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                        }
                        else
                            Diferencia_Detalle_ID = "0000000001";

                        foreach (DataRow Diferencia in Ordenes_Variacion.P_Dt_Diferencias.Rows)
                        {
                            Mi_SQL = "";
                            Mi_SQL = "INSERT INTO ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + " ( ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencias_Detalles + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencia + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Valor_Fiscal + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tasa_Predial_ID + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Diferencia + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Periodo + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Importe + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Cuota_Bimestral + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ") VALUES('";
                            Mi_SQL = Mi_SQL + Diferencia_Detalle_ID + "','";
                            Mi_SQL = Mi_SQL + Diferencia_ID + "','" + Diferencia["VALOR_FISCAL"].ToString().Replace(",", "") + "','";
                            Mi_SQL = Mi_SQL + Diferencia["TASA_ID"].ToString() + "','" + Diferencia["TIPO"].ToString() + "','" + Diferencia["TIPO_PERIODO"].ToString() + "','";
                            Mi_SQL = Mi_SQL + Diferencia["IMPORTE"].ToString().Replace("$", "").Replace(",", "") + "','" + Diferencia["CUOTA_BIMESTRAL"].ToString().Replace("$", "").Replace(",", "") + "','" + Diferencia["PERIODO"].ToString() + "')";
                            Diferencia_Detalle_ID = String.Format("{0:0000000000}", Convert.ToInt32(Diferencia_Detalle_ID) + 1);
                            //Ejecutar consulta
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                        //Agregar Variacion para ingresar el movimineto
                    }
                    else if (Resultado_Count > 0)//Modificar Diferencias Existentes
                    {
                        Mi_SQL = "";
                        Mi_SQL = "SELECT ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia;
                        Mi_SQL = Mi_SQL + " FROM ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias;
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Orden_Variacion;
                        Mi_SQL = Mi_SQL + " = '";
                        Mi_SQL = Mi_SQL + Ordenes_Variacion.P_Generar_Orden_No_Orden;
                        Mi_SQL = Mi_SQL + "' AND ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_Anio;
                        Mi_SQL = Mi_SQL + " = '";
                        Mi_SQL = Mi_SQL + Ordenes_Variacion.P_Generar_Orden_Anio + "'";


                        //Ejecutar consulta
                        Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                        //Verificar si no es nulo
                        if (Convert.IsDBNull(Aux) == false)
                        {
                            Diferencia_ID = String.Format("{0:0000000000}", (Aux));
                        }
                        Mi_SQL = "";
                        Mi_SQL = "DELETE FROM ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle;
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencia + " = '";
                        Mi_SQL = Mi_SQL + Diferencia_ID + "'";

                        //Ejecutar consulta
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        //Formar Sentencia de consulta de consecutivo de la tabla de detalles diferencias o rezago
                        Mi_SQL = "";
                        Mi_SQL = "SELECT NVL(MAX(";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencias_Detalles + "),0000000000)";
                        Mi_SQL = Mi_SQL + " FROM ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle;

                        //Ejecutar consulta

                        Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                        //Verificar si no es nulo
                        if (Convert.IsDBNull(Aux) == false)
                        {
                            Diferencia_Detalle_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                        }
                        else
                            Diferencia_Detalle_ID = "0000000001";

                        foreach (DataRow Diferencia in Ordenes_Variacion.P_Dt_Diferencias.Rows)
                        {
                            Mi_SQL = "";
                            Mi_SQL = "INSERT INTO ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + " ( ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencias_Detalles + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencia + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Valor_Fiscal + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tasa_Predial_ID + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Diferencia + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Periodo + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Importe + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Cuota_Bimestral + ",";
                            Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Periodo + ") VALUES('";
                            Mi_SQL = Mi_SQL + Diferencia_Detalle_ID + "','";
                            Mi_SQL = Mi_SQL + Diferencia_ID + "','" + Diferencia["VALOR_FISCAL"].ToString().Replace(",", "") + "','";
                            Mi_SQL = Mi_SQL + Diferencia["TASA_ID"].ToString() + "','" + Diferencia["TIPO"].ToString() + "','" + Diferencia["TIPO_PERIODO"].ToString() + "','";
                            Mi_SQL = Mi_SQL + Diferencia["IMPORTE"].ToString().Replace("$", "").Replace(",", "") + "','" + Diferencia["CUOTA_BIMESTRAL"].ToString().Replace("$", "").Replace(",", "") + "','" + Diferencia["PERIODO"].ToString() + "')";
                            Diferencia_Detalle_ID = String.Format("{0:0000000000}", Convert.ToInt32(Diferencia_Detalle_ID) + 1);
                            //Ejecutar consulta
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }

                    }
                }
                else//Elimina las ya existentes
                {
                    Mi_SQL = "";
                    Mi_SQL = "SELECT ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia;
                    Mi_SQL = Mi_SQL + " FROM ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias;
                    Mi_SQL = Mi_SQL + " WHERE ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Orden_Variacion;
                    Mi_SQL = Mi_SQL + " = '";
                    Mi_SQL = Mi_SQL + Ordenes_Variacion.P_Generar_Orden_No_Orden;
                    Mi_SQL = Mi_SQL + "' AND ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_Anio;
                    Mi_SQL = Mi_SQL + " = '";
                    Mi_SQL = Mi_SQL + Ordenes_Variacion.P_Generar_Orden_Anio + "'";


                    //Ejecutar consulta
                    DataTable Dt_Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    if (Dt_Aux.Rows.Count > 0)
                    {

                        //Verificar si no es nulo                    
                        Diferencia_ID = Dt_Aux.Rows[0][0].ToString();

                        Mi_SQL = "";
                        Mi_SQL = "DELETE FROM ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle;
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencia + " = '";
                        Mi_SQL = Mi_SQL + Diferencia_ID + "'";

                        //Ejecutar consulta
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();


                        Mi_SQL = "";
                        Mi_SQL = "DELETE FROM ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias;
                        Mi_SQL = Mi_SQL + " WHERE ";
                        Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia + " = '";
                        Mi_SQL = Mi_SQL + Diferencia_ID + "'";

                        //Ejecutar consulta
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();

                    }
                }

                //Actualizar Contrarecibo
                Mi_SQL = "";
                Mi_SQL = "UPDATE " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos;
                Mi_SQL += " SET " + Ope_Pre_Contrarecibos.Campo_Estatus + " = 'POR VALIDAR'";
                Mi_SQL += ", " + Ope_Pre_Contrarecibos.Campo_Usuario_Modifico + "='" + Cls_Sessiones.Nombre_Empleado + "'";
                Mi_SQL += ", " + Ope_Pre_Contrarecibos.Campo_Fecha_Modifico + "=SYSDATE";
                Mi_SQL += " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + "='" + Ordenes_Variacion.P_Contrarecibo + "'";
                //Ejecutar consulta
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                //Insertar Propietarios y Copropietarios de la Orden
                //Formar Sentencia para obtener el consecutivo de la orden      
                if (Ordenes_Variacion.P_Dt_Contribuyentes != null)
                {
                    Mi_SQL = "";
                    Mi_SQL = "SELECT NVL(MAX(";
                    Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Copropietario_Orden_Variacion_ID + "),0000000000)";
                    Mi_SQL = Mi_SQL + " FROM ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                    //Ejecutar consulta del consecutivo
                    Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];
                    //Verificar si no es nulo
                    if (Convert.IsDBNull(Aux) == false)
                    {
                        Consec = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                    }
                    else
                    {
                        Consec = "0000000001";
                    }
                    Propietario_Poseedor_Eliminados = false;
                    foreach (DataRow Dr_Prop in Ordenes_Variacion.P_Dt_Contribuyentes.Rows)
                    {
                        if ((Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() == "PROPIETARIO"
                             || Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() == "POSEEDOR")
                             && Propietario_Poseedor_Eliminados == false)
                        {
                            Mi_SQL = "";
                            Mi_SQL = "DELETE FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + Ordenes_Variacion.P_Generar_Orden_No_Orden + "' ";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Ordenes_Variacion.P_Generar_Orden_Anio + " ";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Ordenes_Variacion.P_Generar_Orden_Cuenta_ID + "' ";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " IN ('PROPIETARIO', 'POSEEDOR')";
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            Propietario_Poseedor_Eliminados = true;
                        }
                        Mi_SQL = "";
                        Mi_SQL = "SELECT " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Copropietario_Orden_Variacion_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + Ordenes_Variacion.P_Generar_Orden_No_Orden + "' ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Ordenes_Variacion.P_Generar_Orden_Anio + " ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Ordenes_Variacion.P_Generar_Orden_Cuenta_ID + "' ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString() + "'";
                        if (Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() == "COPROPIETARIO")
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() + "'";
                        Dt_Copropietarios_Variacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                        if (Dt_Copropietarios_Variacion.Rows.Count > 0)
                        {
                            Mi_SQL = "";
                            Mi_SQL = "UPDATE " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus].ToString() + "', ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() + "'";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + " = '" + Ordenes_Variacion.P_Generar_Orden_No_Orden + "' ";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + " = " + Ordenes_Variacion.P_Generar_Orden_Anio + " ";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + " = '" + Ordenes_Variacion.P_Generar_Orden_Cuenta_ID + "' ";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString() + "'";
                            if (Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() == "COPROPIETARIO")
                                Mi_SQL = Mi_SQL + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " = '" + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() + "'";
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            Consec = String.Format("{0:0000000000}", Convert.ToInt32(Consec) + 1);
                        }
                        else
                        {
                            Mi_SQL = "";
                            Mi_SQL = "INSERT INTO ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + " ( ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Copropietario_Orden_Variacion_ID + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Cuenta_Predial_ID + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Usuario_Creo + ", ";
                            Mi_SQL = Mi_SQL + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Fecha_Creo + ") VALUES('";
                            Mi_SQL = Mi_SQL + Consec + "','";
                            Mi_SQL = Mi_SQL + Ordenes_Variacion.P_Generar_Orden_No_Orden + "',";
                            if (String.IsNullOrEmpty(Ordenes_Variacion.P_Generar_Orden_Cuenta_ID))
                                Mi_SQL = Mi_SQL + "NULL,'";
                            else
                                Mi_SQL = Mi_SQL + "'" + Ordenes_Variacion.P_Generar_Orden_Cuenta_ID + "','";
                            Mi_SQL = Mi_SQL + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString() + "',";
                            Mi_SQL = Mi_SQL + Ordenes_Variacion.P_Generar_Orden_Anio + ",'";
                            Mi_SQL = Mi_SQL + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus].ToString() + "','";
                            Mi_SQL = Mi_SQL + Dr_Prop[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() + "','";
                            Mi_SQL = Mi_SQL + Presidencia.Sessiones.Cls_Sessiones.Nombre_Empleado + "',SYSDATE )";
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            Consec = String.Format("{0:0000000000}", Convert.ToInt32(Consec) + 1);
                        }
                    }
                }
                Actualizar = true;
            }
            catch (OracleException Ex)
            {
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
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
                    Mensaje = "Error al intentar modificar un Registro de Orden de Variacion. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Ordenes_Variacion.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Actualizar;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN     : Aplicar_Variacion_Propietarios
        ///DESCRIPCIÓN              : Afecta la variación de Propietarios
        ///PARAMETROS: 
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 06/Septiembre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        internal static Boolean Aplicar_Variacion_Propietarios(Cls_Ope_Pre_Orden_Variacion_Negocio Propietario)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Alta = false;

            if (Propietario.P_Cmmd != null)
            {
                //Cmd.Connection = Propietario.P_Trans.Connection;
                //Cmd.Transaction = Propietario.P_Trans;
                Cmd = Propietario.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
            }
            try
            {
                String Mi_SQL = "";

                Mi_SQL = "DELETE FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                Mi_SQL += " WHERE ";
                //if (Propietario.P_Propietario_Cuenta_Predial_ID != "" && Propietario.P_Propietario_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Propietario.P_Propietario_Cuenta_Predial_ID + "' AND ( ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO' OR ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Tipo + " = 'POSEEDOR')";
                }
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //Int32 Propietario_ID = 0;
                if ((Propietario.P_Propietario_Cuenta_Predial_ID != "" && Propietario.P_Propietario_Cuenta_Predial_ID != null)
                    && (Propietario.P_Propietario_Propietario_ID != "" && Propietario.P_Propietario_Propietario_ID != null))
                {
                    Int32 Propietario_ID = Convert.ToInt32(Obtener_ID_Consecutivo(ref Cmd, Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios, Cat_Pre_Propietarios.Campo_Propietario_ID, "", 10));
                    Mi_SQL = "INSERT INTO " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " (";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Propietario_ID + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Contribuyente_ID + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Tipo + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Fecha_Creo + ") ";
                    Mi_SQL += "VALUES (";
                    Mi_SQL += "'" + Propietario_ID.ToString("0000000000") + "', ";
                    if (Propietario.P_Propietario_Cuenta_Predial_ID != null)
                    {
                        Mi_SQL += "'" + Propietario.P_Propietario_Cuenta_Predial_ID + "', ";
                    }
                    else
                    {
                        Mi_SQL += "NULL, ";
                    }
                    if (Propietario.P_Propietario_Propietario_ID != null)
                    {
                        Mi_SQL += "'" + Propietario.P_Propietario_Propietario_ID + "', ";
                    }
                    else
                    {
                        Mi_SQL += "NULL, ";
                    }
                    if (Propietario.P_Propietario_Tipo != null)
                    {
                        Mi_SQL += "'" + Propietario.P_Propietario_Tipo + "', ";
                    }
                    else
                    {
                        Mi_SQL += "NULL, ";
                    }
                    Mi_SQL += "'" + Propietario.P_Usuario + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    //Propietario.P_Propietario_Propietario_ID = Propietario_ID.ToString("0000000000");
                }

                if (Propietario.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Propietario.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Propietario.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Alta;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN     : Aplicar_Variacion_Copropietarios
        ///DESCRIPCIÓN              : Afecta la variación de Copropietarios
        ///PARAMETROS: 
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 31/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        internal static Boolean Aplicar_Variacion_Copropietarios(Cls_Ope_Pre_Orden_Variacion_Negocio Copropietario)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Alta = false;

            if (Copropietario.P_Cmmd != null)
            {
                //Cmd.Connection = Copropietario.P_Trans.Connection;
                //Cmd.Transaction = Copropietario.P_Trans;
                Cmd = Copropietario.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
            }
            try
            {
                String Mi_SQL = "";

                Mi_SQL = "DELETE FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios;
                Mi_SQL += " WHERE ";
                //if (Copropietario.P_Copropietario_Cuenta_Predial_ID != "" && Copropietario.P_Copropietario_Cuenta_Predial_ID != null)
                {
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Copropietario.P_Copropietario_Cuenta_Predial_ID + "' AND ";
                    Mi_SQL += Cat_Pre_Propietarios.Campo_Tipo + " = 'COPROPIETARIO'";
                }
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                if (Copropietario.P_Dt_Copropietarios != null)
                {
                    if (Copropietario.P_Dt_Copropietarios.Rows.Count > 0)
                    {
                        Int32 Propietario_ID;
                        //if (Copropietario.P_Propietario_Propietario_ID != null && Copropietario.P_Propietario_Propietario_ID.Trim() != "")
                        //{
                        //    Propietario_ID = Convert.ToInt32(Copropietario.P_Propietario_Propietario_ID.Trim()) + 1;
                        //}
                        //else
                        //{
                        //    Propietario_ID = Convert.ToInt32(Obtener_ID_Consecutivo(ref Cmd, Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios, Cat_Pre_Propietarios.Campo_Propietario_ID, "", 10));
                        //}
                        foreach (DataRow Dr_Copropietario in Copropietario.P_Dt_Copropietarios.Rows)
                        {
                            Propietario_ID = Convert.ToInt32(Obtener_ID_Consecutivo(ref Cmd, Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios, Cat_Pre_Propietarios.Campo_Propietario_ID, "", 10));
                            Mi_SQL = "INSERT INTO " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " (";
                            Mi_SQL += Cat_Pre_Propietarios.Campo_Propietario_ID + ", ";
                            Mi_SQL += Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + ", ";
                            Mi_SQL += Cat_Pre_Propietarios.Campo_Contribuyente_ID + ", ";
                            Mi_SQL += Cat_Pre_Propietarios.Campo_Tipo + ", ";
                            Mi_SQL += Cat_Pre_Propietarios.Campo_Usuario_Creo + ", ";
                            Mi_SQL += Cat_Pre_Propietarios.Campo_Fecha_Creo + ") ";
                            Mi_SQL += "VALUES (";
                            Mi_SQL += "'" + Propietario_ID.ToString("0000000000") + "', ";
                            if (Copropietario.P_Copropietario_Cuenta_Predial_ID != null)
                            {
                                Mi_SQL += "'" + Copropietario.P_Copropietario_Cuenta_Predial_ID + "', ";
                            }
                            else
                            {
                                Mi_SQL += "NULL, ";
                            }
                            if (Dr_Copropietario["CONTRIBUYENTE_ID"] != null)
                            {
                                Mi_SQL += "'" + Dr_Copropietario["CONTRIBUYENTE_ID"].ToString() + "', ";
                            }
                            else
                            {
                                Mi_SQL += "NULL, ";
                            }
                            if (Copropietario.P_Copropietario_Tipo != null)
                            {
                                Mi_SQL += "'" + Copropietario.P_Copropietario_Tipo + "', ";
                            }
                            else
                            {
                                Mi_SQL += "NULL, ";
                            }
                            Mi_SQL += "'" + Copropietario.P_Usuario + "', SYSDATE)";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();

                            //Propietario_ID += 1;
                        }
                    }
                }

                if (Copropietario.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Copropietario.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Copropietario.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Alta;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN     : Aplicar_Variacion_Diferencias
        ///DESCRIPCIÓN              : Afecta los Adeudos con las Diferencias
        ///PARAMETROS: 
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 05/Septiembre/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        internal static Boolean Aplicar_Variacion_Diferencias(Cls_Ope_Pre_Orden_Variacion_Negocio Diferencias)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Alta = false;
            Boolean Suma_Primer_Bimestre = true;

            if (Diferencias.P_Cmmd != null)
            {
                //Cmd.Connection = Diferencias.P_Trans.Connection;
                //Cmd.Transaction = Diferencias.P_Trans;
                Cmd = Diferencias.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
            }
            try
            {
                String Mi_SQL = "";

                int Cont_Bimestres = 0;
                Int32 No_Adeudo = Convert.ToInt32(Obtener_ID_Consecutivo(ref Cmd, Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial, Ope_Pre_Adeudos_Predial.Campo_No_Adeudo, "", 10));
                if (Diferencias.P_Dt_Diferencias != null)
                {
                    foreach (DataRow Dr_Diferencias in Diferencias.P_Dt_Diferencias.Rows)
                    {
                        Mi_SQL = "SELECT " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID;
                        Mi_SQL += " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                        Mi_SQL += " WHERE ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Diferencias.P_Cuenta_Predial_ID + "' AND ";
                        Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Dr_Diferencias["AÑO"];
                        Cmd.CommandText = Mi_SQL;
                        //Cmd.ExecuteReader();
                        //if (OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows.Count > 0)
                        if (!Cmd.ExecuteReader().Read())
                        {
                            Mi_SQL = "INSERT INTO " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " (";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Anio + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Estatus + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Usuario_Creo + ", ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Creo + ") VALUES ('";
                            Mi_SQL += No_Adeudo.ToString("0000000000") + "', ";
                            Mi_SQL += "'" + DateTime.Now.ToString("d-M-yyyy") + "', ";
                            if (Dr_Diferencias["CUOTA_ANUAL"] != null)
                            {
                                Mi_SQL += Dr_Diferencias["CUOTA_ANUAL"].ToString() + ", ";
                            }
                            for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                            {
                                if (Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] != null)
                                {
                                    if (Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()].ToString() != "")
                                    {
                                        Mi_SQL += Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()].ToString() + ", ";
                                    }
                                    else
                                    {
                                        Mi_SQL += "NULL, ";
                                    }
                                }
                            }
                            Mi_SQL += Dr_Diferencias["AÑO"] + ", ";
                            if (Diferencias.P_Diferencias_Estatus != null
                                && Diferencias.P_Diferencias_Estatus != "")
                            {
                                Mi_SQL += "'" + Diferencias.P_Diferencias_Estatus + "', ";
                            }
                            else
                            {
                                Mi_SQL += "'POR PAGAR', ";
                            }
                            Mi_SQL += "'" + Diferencias.P_Cuenta_Predial_ID + "', ";
                            Mi_SQL += "'" + Diferencias.P_Usuario + "', SYSDATE)";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            No_Adeudo++;
                        }
                        else
                        {
                            Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " SET ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha + " = SYSDATE, ";
                            if (!Diferencias.P_Reactivando_Cuenta)
                            {
                                if (Dr_Diferencias["CUOTA_ANUAL"] != DBNull.Value)
                                {
                                    Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Cuota_Anual + " = " + Dr_Diferencias["CUOTA_ANUAL"].ToString() + ", ";
                                }
                            }
                            for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                            {
                                if (Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()] != DBNull.Value)
                                {
                                    if (Diferencias.P_Suma_Variacion_Diferencias)
                                    {
                                        if (Dr_Diferencias["ALTA_BAJA"] != DBNull.Value)
                                        {
                                            if (Dr_Diferencias["ALTA_BAJA"].ToString() != "SOB" && Suma_Primer_Bimestre)
                                            {
                                                Mi_SQL += "BIMESTRE_" + Cont_Bimestres.ToString() + " = NVL(" + "BIMESTRE_" + Cont_Bimestres.ToString() + ", 0) + " + Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()].ToString() + ", ";
                                                if (Dr_Diferencias["ALTA_BAJA"].ToString() == "SOB1")
                                                {
                                                    Suma_Primer_Bimestre = false;
                                                }
                                            }
                                            else
                                            {
                                                Mi_SQL += "BIMESTRE_" + Cont_Bimestres.ToString() + " = " + Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()].ToString() + ", ";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Mi_SQL += "BIMESTRE_" + Cont_Bimestres.ToString() + " = " + Dr_Diferencias["BIMESTRE_" + Cont_Bimestres.ToString()].ToString() + ", ";
                                    }
                                }
                            }
                            if (Diferencias.P_Diferencias_Estatus != null
                                && Diferencias.P_Diferencias_Estatus != "")
                            {
                                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Estatus + " = '" + Diferencias.P_Diferencias_Estatus + "', ";
                            }

                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Diferencias.P_Usuario + "', ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += " WHERE ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Diferencias.P_Cuenta_Predial_ID + "' AND ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Dr_Diferencias["AÑO"];
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                    }
                    //Valida los Adeudos en Negativo para dejarlos con valor Cero.
                    try
                    {
                        Corregir_Aplicacion_Adeudos_Negativos(Diferencias);
                    }
                    catch
                    {
                    }
                }

                if (Diferencias.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Alta = true;
            }
            catch (OracleException Ex)
            {
                if (Diferencias.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
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
                    Mensaje = "Error al intentar dar de Alta un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Diferencias.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Alta;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN     : Corregir_Aplicacion_Adeudos_Negativos
        ///DESCRIPCIÓN              : Valida Adeudos Negativos y los Corrige dejándolos en cero junto con sus Pagos
        ///PARAMETROS: 
        ///CREO                     : Antonio Salvador Benavides Guardado
        ///FECHA_CREO               : 24/Mayo/2012
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************              
        internal static Boolean Corregir_Aplicacion_Adeudos_Negativos(Cls_Ope_Pre_Orden_Variacion_Negocio Diferencias)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Boolean Correccion = false;

            if (Diferencias.P_Cmmd != null)
            {
                Cmd = Diferencias.P_Cmmd;
            }
            else
            {
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
            }
            try
            {
                String Mi_SQL = "";

                int Cont_Bimestres = 0;
                if (Diferencias.P_Dt_Diferencias != null)
                {
                    foreach (DataRow Dr_Diferencias in Diferencias.P_Dt_Diferencias.Rows)
                    {
                        for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                        {
                            Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                            Mi_SQL += " SET BIMESTRE_" + Cont_Bimestres.ToString() + " = 0,";
                            Mi_SQL += " PAGO_BIMESTRE_" + Cont_Bimestres.ToString() + " = 0, ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Diferencias.P_Usuario + "', ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += " WHERE ";
                            Mi_SQL += "BIMESTRE_" + Cont_Bimestres.ToString() + " < 0 AND ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Diferencias.P_Cuenta_Predial_ID + "' AND ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Dr_Diferencias["AÑO"];
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }

                if (Diferencias.P_Cmmd == null)
                {
                    Trans.Commit();
                }
                Correccion = true;
            }
            catch (OracleException Ex)
            {
                if (Diferencias.P_Cmmd == null)
                {
                    Trans.Rollback();
                }
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
                    Mensaje = "Error al intentar dar de Correccion un Registro de Impuesto de Fraccionamiento. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
                if (Diferencias.P_Cmmd == null)
                {
                    Cn.Close();
                }
            }
            return Correccion;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Cuenta
        ///DESCRIPCIÓN: se consultan los datos generales de la cuenta predial
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 19/Ago/2011 11:49:25 a.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************* 
        //Insertar detalles de las diferencias almacenadas en el datatable de la capa de negocio
        public static void Alta_Diferencias(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = ""; //Variable para la consulta SQL            
            DataSet Ds_Resultado = new DataSet();
            string Diferencia_ID = "";
            string Diferencia_Detalle_ID = "";
            object Aux;
            if (Datos.P_Cmmd != null)
            {
                Obj_Comando = Datos.P_Cmmd;
            }
            else
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;
            }

            try
            {
                //Formar Sentencia de consulta de consecutivo de la tabla diferencias o rezago
                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia + "),0000000000)";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias;

                //Ejecutar consulta

                Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                {
                    Diferencia_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                }
                else
                    Diferencia_ID = "0000000001";

                //Insertar Registro de Diferencias
                Mi_SQL = "";
                Mi_SQL = "INSERT INTO ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Tabla_Ope_Pre_Diferencias + " ( ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Diferencia + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_Anio + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias.Campo_No_Orden_Variacion;
                if (Datos.P_Cancelando_Cuenta)
                {
                    Mi_SQL = Mi_SQL + ", " + Ope_Pre_Diferencias.Campo_Total_Recargos;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pre_Diferencias.Campo_Total_Corriente;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pre_Diferencias.Campo_Total_Rezago;
                }
                Mi_SQL = Mi_SQL + ") ";
                Mi_SQL = Mi_SQL + "VALUES('";
                Mi_SQL = Mi_SQL + Diferencia_ID + "','";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Cuenta_ID + "', ";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_Anio + ", '";
                Mi_SQL = Mi_SQL + Datos.P_Generar_Orden_No_Orden + "'";
                if (Datos.P_Cancelando_Cuenta)
                {
                    Mi_SQL = Mi_SQL + ", " + Datos.P_Total_Recargos;
                    Mi_SQL = Mi_SQL + ", " + Datos.P_Total_Corriente;
                    Mi_SQL = Mi_SQL + ", " + Datos.P_Total_Rezago;
                }
                Mi_SQL = Mi_SQL + ") ";
                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Formar Sentencia de consulta de consecutivo de la tabla de detalles diferencias o rezago
                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencias_Detalles + "),0000000000)";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle;

                //Ejecutar consulta

                Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0];

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                {
                    Diferencia_Detalle_ID = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                }
                else
                    Diferencia_Detalle_ID = "0000000001";

                foreach (DataRow Diferencia in Datos.P_Dt_Diferencias.Rows)
                {
                    Mi_SQL = "";
                    Mi_SQL = "INSERT INTO ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Tabla_Ope_Pre_Diferencias_Detalle + " ( ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencias_Detalles + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_No_Diferencia + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Valor_Fiscal + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tasa_Predial_ID + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Diferencia + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Tipo_Periodo + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Importe + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Cuota_Bimestral + ",";
                    Mi_SQL = Mi_SQL + Ope_Pre_Diferencias_Detalle.Campo_Periodo;
                    if (Datos.P_Cancelando_Cuenta)
                    {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_1;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_2;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_3;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_4;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_5;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pre_Diferencias_Detalle.Campo_Bimestre_6;
                    }
                    Mi_SQL = Mi_SQL + ") VALUES('";
                    Mi_SQL = Mi_SQL + Diferencia_Detalle_ID + "','";
                    Mi_SQL = Mi_SQL + Diferencia_ID + "','" + Diferencia["VALOR_FISCAL"].ToString().Replace(",", "") + "','";
                    Mi_SQL = Mi_SQL + Diferencia["TASA_ID"].ToString() + "','" + Diferencia["TIPO"].ToString() + "','" + Diferencia["TIPO_PERIODO"].ToString() + "','";
                    Mi_SQL = Mi_SQL + Diferencia["IMPORTE"].ToString().Replace("$", "").Replace(",", "") + "','" + Diferencia["CUOTA_BIMESTRAL"].ToString().Replace("$", "").Replace(",", "") + "','" + Diferencia["PERIODO"].ToString() + "'";
                    if (Datos.P_Cancelando_Cuenta)
                    {
                        Mi_SQL = Mi_SQL + ", '" + Diferencia["BIMESTRE_1"].ToString().Replace("$", "").Replace(",", "");
                        Mi_SQL = Mi_SQL + "', '" + Diferencia["BIMESTRE_2"].ToString().Replace("$", "").Replace(",", "");
                        Mi_SQL = Mi_SQL + "', '" + Diferencia["BIMESTRE_3"].ToString().Replace("$", "").Replace(",", "");
                        Mi_SQL = Mi_SQL + "', '" + Diferencia["BIMESTRE_4"].ToString().Replace("$", "").Replace(",", "");
                        Mi_SQL = Mi_SQL + "', '" + Diferencia["BIMESTRE_5"].ToString().Replace("$", "").Replace(",", "");
                        Mi_SQL = Mi_SQL + "', '" + Diferencia["BIMESTRE_6"].ToString().Replace("$", "").Replace(",", "");
                        Mi_SQL = Mi_SQL + "'";
                    }
                    Mi_SQL = Mi_SQL + ")";
                    Diferencia_Detalle_ID = String.Format("{0:0000000000}", Convert.ToInt32(Diferencia_Detalle_ID) + 1);
                    //Ejecutar consulta
                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();
                }
                //Agregar Variacion para ingresar el movimineto
                Datos.Agregar_Variacion(Cat_Pre_Cuentas_Predial.Campo_No_Diferencia, Diferencia_ID);

                if (Datos.P_Cmmd == null)
                {
                    Obj_Transaccion.Commit();
                }
            }
            catch (OracleException Ex)
            {
                if (Datos.P_Cmmd == null)
                {
                    Obj_Transaccion.Rollback();
                }
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
        #endregion

        #region [Convenios, Restructuras, Impuestos]

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Domicilio_Y_Propietario
        ///DESCRIPCIÓN: Obtiene los id´s y el nombre del propietario.
        ///PARAMENTROS:   Orden: Contienen los campos para consultar la orden deseada.
        ///CREO: Miguel Angel Bedolla Moreno.
        ///FECHA_CREO: 03/Febrero/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Domicilio_Y_Propietario(Cls_Ope_Pre_Orden_Variacion_Negocio Orden)
        {
            DataTable tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT " + Ope_Pre_Ordenes_Variacion.Campo_Calle_ID + ", " + Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID + ", " + Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion + ", " + Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo + ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Exterior + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion + ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Interior + ", ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion + ", ";
                Mi_SQL += "(SELECT " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "||' '||" + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "||' '||" + Cat_Pre_Contribuyentes.Campo_Nombre + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN (SELECT " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + " FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + "=" + Orden.P_Año + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + "='" + Orden.P_Orden_Variacion_ID + "' AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " IN ('PROPIETARIO','POSEEDOR') AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + "='ALTA')) AS NOMBRE_CONTRIBUYENTE, ";
                Mi_SQL += "(SELECT " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN (SELECT " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + " FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + "=" + Orden.P_Año + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + "='" + Orden.P_Orden_Variacion_ID + "' AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " IN ('PROPIETARIO','POSEEDOR') AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + "='ALTA')) AS CONTRIBUYENTE_ID, ";
                Mi_SQL += "(SELECT " + Cat_Pre_Contribuyentes.Campo_RFC + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN (SELECT " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID + " FROM " + Ope_Pre_Copropietarios_Orde_Variacion.Tabla_Ope_Pre_Copropietarios_Orden_Variacion + " WHERE " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Anio + "=" + Orden.P_Año + " AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_No_Orden_Variacion + "='" + Orden.P_Orden_Variacion_ID + "' AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo + " IN ('PROPIETARIO','POSEEDOR') AND " + Ope_Pre_Copropietarios_Orde_Variacion.Campo_Estatus + "='ALTA')) AS RFC ";
                Mi_SQL += "FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + "='" + Orden.P_Orden_Variacion_ID + "' AND " + Ope_Pre_Ordenes_Variacion.Campo_Anio + "=" + Orden.P_Año;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros de la orden de variación. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return tabla;
        }



        #endregion

        #region [Utilidades Metodos]
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Operador_Comparacion
        ///DESCRIPCIÓN          : Devuelve una cadena adecuada al operador indicado en la capa de Negocios
        ///PARAMETROS           : 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Validar_Operador_Comparacion(String Filtro)
        {
            String Cadena_Validada;
            if (Filtro.Trim().StartsWith("<")
               || Filtro.Trim().StartsWith(">")
               || Filtro.Trim().StartsWith("<>")
               || Filtro.Trim().StartsWith("<=")
               || Filtro.Trim().StartsWith(">=")
               || Filtro.Trim().StartsWith("=")
               || Filtro.Trim().ToUpper().StartsWith("BETWEEN")
               || Filtro.Trim().ToUpper().StartsWith("LIKE")
               || Filtro.Trim().ToUpper().StartsWith("IN")
               || Filtro.Trim().ToUpper().StartsWith("NOT IN"))
            {
                Cadena_Validada = " " + Filtro + " ";
            }
            else
            {
                if (Filtro.Trim().ToUpper().StartsWith("NULL")
                    || Filtro.Trim().ToUpper().StartsWith("NOT NULL"))
                {
                    Cadena_Validada = " IS " + Filtro + " ";
                }
                else
                {
                    Cadena_Validada = " = '" + Filtro + "' ";
                }
            }
            return Cadena_Validada;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN : Obtener_Periodos_Bimestre
        ///DESCRIPCIÓN          : Valida la cadena indicada para obtener los periodos de la Bimestres quitando los Años
        ///PARAMETROS: 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 20/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        public static String Obtener_Periodos_Bimestre(String Periodos, out Boolean Periodo_Corriente_Validado, out Boolean Periodo_Rezago_Validado)
        {
            String Periodo = "";
            int Indice = 0;
            Periodo_Corriente_Validado = false;
            Periodo_Rezago_Validado = false;

            if (Periodos.IndexOf("-") >= 0)
            {
                if (Periodos.Split('-').Length == 2)
                {
                    //Valida el segundo nodo del arreglo
                    if (Periodos.Split('-').GetValue(1).ToString().IndexOf("/") >= 0)
                    {
                        Periodo = Periodos.Split('-').GetValue(0).ToString().Trim().Substring(0, 1);
                        Periodo += "-";
                        Periodo += Periodos.Split('-').GetValue(1).ToString().Trim().Substring(0, 1);
                        Periodo_Rezago_Validado = true;
                    }
                    else
                    {
                        Periodo = Periodos.Split('-').GetValue(0).ToString().Replace("/", "-").Trim();
                        Periodo_Corriente_Validado = true;
                    }
                }
                else
                {
                    if (Periodos.Contains("/"))
                    {
                        Indice = Periodos.IndexOf("/");
                        Periodo = Periodos.Substring(Indice - 1, 1);
                        Periodo += "-";
                        Indice = Periodos.IndexOf("/", Indice + 1);
                        Periodo += Periodos.Substring(Indice - 1, 1);
                        Periodo_Rezago_Validado = true;
                    }
                    else
                    {
                        Periodo = Periodos.Substring(0, 3);
                        Periodo_Corriente_Validado = true;
                    }
                }
            }
            else
            {
                if (Periodos.Trim().IndexOf(" ") >= 0)
                {
                    if (Periodos.Split(' ').GetValue(0).ToString().Contains("/"))
                    {
                        Periodo = Periodos.Split(' ').GetValue(0).ToString().Replace("/", "-").Trim();
                        Periodo_Corriente_Validado = true;
                    }
                    else
                    {
                        Periodo = Periodos.Substring(0, 3);
                        Periodo_Corriente_Validado = true;
                    }
                }
            }
            return Periodo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Obtener_Dato_Consulta(String Campo, String Tabla, String Condiciones)
        {
            String Mi_SQL;
            String Dato_Consulta = "";

            try
            {
                Mi_SQL = "SELECT " + Campo;
                if (Tabla != "")
                {
                    Mi_SQL += " FROM " + Tabla;
                }
                if (Condiciones != "")
                {
                    Mi_SQL += " WHERE " + Condiciones;
                }

                OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Dr_Dato.Read())
                {
                    if (Dr_Dato[0] != null)
                    {
                        Dato_Consulta = Dr_Dato[0].ToString();
                    }
                    else
                    {
                        Dato_Consulta = "";
                    }
                    Dr_Dato.Close();
                }
                else
                {
                    Dato_Consulta = "";
                }
                if (Dr_Dato != null)
                {
                    Dr_Dato.Close();
                }
                Dr_Dato = null;
            }
            catch
            {
            }
            finally
            {
            }

            return Dato_Consulta;
        }
        #endregion

        #region Reportes
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Reporte
        ///DESCRIPCIÓN: se consultan los datos generales de la cuenta predial
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 08/11/2011 11:49:25 a.m.
        ///MODIFICO: jtoledo
        ///FECHA_MODIFICO: 11/11/11
        ///CAUSA_MODIFICACIÓN: Se agrego filtro de año
        ///*******************************************************************************
        internal static DataSet Consulta_Datos_Reporte(Cls_Ope_Pre_Orden_Variacion_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL            
            DataSet Ds_Resultado = new DataSet();
            String Orden_Variacion;
            String Anio_Variacion;
            String[] Orden_Variacion_Compuesta;
            string Propietario_ID;
            string Cuenta_ID;
            Cls_Ope_Pre_Parametros_Negocio Anio_Corriente = new Cls_Ope_Pre_Parametros_Negocio();
            try
            {
                Orden_Variacion = Datos.P_Generar_Orden_No_Orden;
                Anio_Variacion = Datos.P_Generar_Orden_Anio;
                if (Orden_Variacion != null)
                {
                    if (Orden_Variacion.Contains("/"))
                    {
                        Orden_Variacion_Compuesta = Orden_Variacion.Split('/');
                        if (Orden_Variacion_Compuesta.Length > 1)
                        {
                            Orden_Variacion = Orden_Variacion_Compuesta[0];
                            Anio_Variacion = Orden_Variacion_Compuesta[1];
                        }
                    }
                }
                else
                {
                    Orden_Variacion = "0";
                }
                Mi_SQL = "";
                Mi_SQL += "SELECT";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;
                Mi_SQL += " ||'/'|| ";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " AS NO_ORDEN_VARIACION,";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + ",";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID,";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ",";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " AS ESTATUS_VARIACION,";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta + " AS ESTATUS_CUENTA,";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Observaciones + " AS OBSERVACIONES,";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " AS NO_CONTRARECIBO,";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ",";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Usuario_Creo + ",";
                Mi_SQL += " MOV." + Cat_Pre_Movimientos.Campo_Identificador + " ||' - '||";
                Mi_SQL += " MOV." + Cat_Pre_Movimientos.Campo_Descripcion + " AS MOVIMIENTO";
                Mi_SQL += " FROM ";
                Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ORDEN LEFT OUTER JOIN ";
                Mi_SQL += Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " MOV ON MOV.";
                Mi_SQL += Cat_Pre_Movimientos.Campo_Movimiento_ID + " = ORDEN." + Ope_Pre_Orden_Variacion.Campo_Movimiento_ID;
                Mi_SQL += " WHERE ";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = '" + Orden_Variacion + "' AND ";
                Mi_SQL += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = '" + Anio_Variacion + "'";

                Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                Ds_Resultado.Tables[0].TableName = "DT_ORDEN";

                if (Ds_Resultado.Tables[0].Rows.Count > 0 || Orden_Variacion == "0")
                {
                    Cuenta_ID = Datos.P_Cuenta_Predial_ID;
                    if (Orden_Variacion != "0")
                        Cuenta_ID = Ds_Resultado.Tables["DT_ORDEN"].Rows[0]["CUENTA_PREDIAL_ID"].ToString();

                    Mi_SQL = "";
                    Mi_SQL += "SELECT ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AS CUENTA_PREDIAL_ID, ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL, ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " AS ESTATUS, ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " AS NO_EXTERIOR, ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + " AS NO_INTERIOR, ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Superficie_Total + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Efectos + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion + ", ";
                    Mi_SQL += " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " AS NO_CUOTA_FIJA,  ";
                    Mi_SQL += " PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " AS CONTRIBUYENTE_ID,";
                    Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " AS CALLE_ID, ";
                    Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS CALLE, ";
                    Mi_SQL += " CALLES." + Cat_Pre_Calles.Campo_Colonia_ID + ", ";
                    Mi_SQL += " COL." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA, ";
                    Mi_SQL += " USO." + Cat_Pre_Uso_Suelo.Campo_Descripcion + " AS USO_SUELO, ";
                    Mi_SQL += " TIPO." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " AS TIPO_PREDIO, ";
                    Mi_SQL += " ESTADO." + Cat_Pre_Estados_Predio.Campo_Descripcion + " AS ESTADO_PREDIO, ";
                    Mi_SQL += " ROUND(TO_NUMBER(CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual + ")/6,2) AS CUOTA_BIMESTRAL";

                    Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ";
                    Mi_SQL += " LEFT OUTER JOIN ";
                    Mi_SQL += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP ON PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " LEFT OUTER JOIN ";
                    Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES ON CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " = CUEN.";
                    Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL ON COL." + Cat_Ate_Colonias.Campo_Colonia_ID;
                    Mi_SQL += " = CALLES." + Cat_Pre_Calles.Campo_Colonia_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo + " USO ON USO." + Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " TIPO ON TIPO." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio + " ESTADO ON ESTADO." + Cat_Pre_Estados_Predio.Campo_Estado_Predio_ID + " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID;

                    Mi_SQL += " WHERE CUEN." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_ID + "'";


                    Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());

                    Ds_Resultado.Tables[1].TableName = "DT_GENERALES";

                    if (Ds_Resultado.Tables[1].Rows.Count > 0)
                    {
                        Propietario_ID = Ds_Resultado.Tables["DT_GENERALES"].Rows[0][Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString();

                        if (!string.IsNullOrEmpty(Propietario_ID))
                        {
                            Mi_SQL = "";
                            Mi_SQL += "SELECT ";
                            Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Propietario_ID + " AS PROPIETARIO, ";
                            Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " AS CONTRIBUYENTE_ID, ";
                            Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " AS CUENTA_ID, ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE, ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_Tipo_Propietario + " AS TIPO_PROPIETARIO, ";
                            Mi_SQL += " CON. " + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                            //
                            Mi_SQL += " COL. " + Cat_Ate_Colonias.Campo_Nombre + " AS NOMBRE_COLONIA, ";
                            Mi_SQL += " CALL. " + Cat_Pre_Calles.Campo_Nombre + " AS NOMBRE_CALLE, ";
                            Mi_SQL += " (CASE WHEN NOT CUEN." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " IS NULL THEN (SELECT " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Nombre + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " WHERE " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Calle_ID + " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + ") ELSE CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion + " END) AS NOMBRE_CALLE_NOT, ";
                            Mi_SQL += " (CASE WHEN NOT CUEN." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " IS NULL THEN (SELECT " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Colonia_ID + " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + ") ELSE CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion + " END) AS NOMBRE_COLONIA_NOT, ";
                            Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion + " AS NO_EXTERIOR, ";
                            Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion + " AS NO_INTERIOR, ";
                            Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo + " AS FORANEO, ";
                            Mi_SQL += " CUEN. " + Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal + " AS CP, ";
                            Mi_SQL += " (CASE WHEN NOT CUEN." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " IS NULL THEN (SELECT " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + "." + Cat_Pre_Ciudades.Campo_Nombre + " FROM " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + " WHERE " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + "." + Cat_Pre_Ciudades.Campo_Ciudad_ID + " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + ") ELSE CUEN." + Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion + " END) AS NOMBRE_CIUDAD, ";
                            Mi_SQL += " (CASE WHEN NOT CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " IS NULL THEN (SELECT " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados + "." + Cat_Pre_Estados.Campo_Nombre + " FROM " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " WHERE " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados + "." + Cat_Pre_Estados.Campo_Estado_ID + " = CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + ") ELSE CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion + " END) AS NOMBRE_ESTADO, ";
                            Mi_SQL += " PROP. " + Cat_Pre_Propietarios.Campo_Tipo + " AS TIPO, ";
                            Mi_SQL += " EDO. " + Cat_Pre_Estados.Campo_Nombre + " AS NOMBRE_ESTADO_1, ";
                            Mi_SQL += " EDO. " + Cat_Pre_Ciudades.Campo_Nombre + " AS NOMBRE_CIUDAD_1 ";

                            Mi_SQL += " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP LEFT OUTER JOIN ";
                            Mi_SQL += Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CON ON CON.";
                            Mi_SQL += Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                            Mi_SQL += " LEFT OUTER JOIN ";
                            Mi_SQL += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUEN ON CUEN.";
                            Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                            Mi_SQL += " LEFT OUTER JOIN ";
                            Mi_SQL += Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COL ON CUEN.";
                            Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion + " = COL." + Cat_Ate_Colonias.Campo_Colonia_ID;
                            Mi_SQL += " LEFT OUTER JOIN ";
                            Mi_SQL += Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALL ON CUEN.";
                            Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion + " = CALL." + Cat_Pre_Calles.Campo_Calle_ID;
                            Mi_SQL += " LEFT OUTER JOIN ";
                            Mi_SQL += Cat_Pre_Estados.Tabla_Cat_Pre_Estados + " EDO ON CUEN.";
                            Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion + " = EDO." + Cat_Pre_Estados.Campo_Estado_ID;
                            Mi_SQL += " LEFT OUTER JOIN ";
                            Mi_SQL += Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades + " CD ON CUEN.";
                            Mi_SQL += Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion + " = CD." + Cat_Pre_Ciudades.Campo_Ciudad_ID;

                            Mi_SQL += " WHERE ";
                            Mi_SQL += "PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Cuenta_ID + "'";
                            Mi_SQL += "AND (PROP." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO'";
                            Mi_SQL += "OR PROP." + Cat_Pre_Propietarios.Campo_Tipo + " = 'POSEEDOR')";

                            Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                            Ds_Resultado.Tables[2].TableName = "DT_PROPIETARIO";
                        }
                        else
                        {
                            Ds_Resultado.Tables.Add();
                            Ds_Resultado.Tables[2].TableName = "DT_PROPIETARIO";
                        }
                    }

                    Mi_SQL = "";
                    Mi_SQL = "SELECT ";
                    Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " AS CONTRIBUYENTE_ID,";
                    Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| ";
                    Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " ||' '|| ";
                    Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_Nombre + " AS NOMBRE, ";
                    Mi_SQL = Mi_SQL + " CONT." + Cat_Pre_Contribuyentes.Campo_RFC + " AS RFC, ";
                    Mi_SQL = Mi_SQL + " PROP." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + ", ";
                    Mi_SQL = Mi_SQL + " PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL = Mi_SQL + " PROP." + Cat_Pre_Propietarios.Campo_Tipo + ", ";
                    Mi_SQL = Mi_SQL + " 'ACTUAL' ESTATUS_VARIACION ";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CONT";
                    Mi_SQL = Mi_SQL + " JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " PROP ON PROP.";
                    Mi_SQL = Mi_SQL + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = CONT." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID;
                    Mi_SQL = Mi_SQL + " WHERE  PROP." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Cuenta_ID + "'";
                    Mi_SQL = Mi_SQL + " AND  PROP." + Cat_Pre_Propietarios.Campo_Tipo + " = 'COPROPIETARIO'";
                    Ds_Resultado.Tables.Add(OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Copy());
                    Ds_Resultado.Tables[3].TableName = "DT_COPROPIETARIOS";


                }

                return Ds_Resultado;
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Reporte_Movimientos
        ///DESCRIPCIÓN          : Obtiene las órdenes de variación de acuerdo a los filtros establecidos en la interfaz
        ///PARAMETROS           : Orden, instancia de Cls_Ope_Pre_Orden_Variacion_Negocio
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 15/Enero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Datos_Reporte_Movimientos(Cls_Ope_Pre_Orden_Variacion_Negocio Orden)
        {
            DataTable Tabla = new DataTable();
            String Mi_SQL;
            try
            {
                if (Orden.P_Campos_Dinamicos != null && Orden.P_Campos_Dinamicos != "")
                {
                    Mi_SQL = "SELECT " + Orden.P_Campos_Dinamicos;
                }
                else
                {
                    Mi_SQL = "SELECT ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Observaciones + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Numero_Nota_Impreso + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Nota + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + ", ";
                    Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID;
                }
                Mi_SQL += " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                if (Orden.P_Unir_Tablas != null && Orden.P_Unir_Tablas != "")
                {
                    Mi_SQL += ", " + Orden.P_Unir_Tablas;
                }
                if (Orden.P_Filtros_Dinamicos != null && Orden.P_Filtros_Dinamicos != "")
                {
                    Mi_SQL += " WHERE " + Orden.P_Filtros_Dinamicos;
                }
                else
                {
                    Mi_SQL += " WHERE ";
                    if (Orden.P_Cuenta_Predial_ID != null && Orden.P_Cuenta_Predial_ID != "")
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '" + Orden.P_Cuenta_Predial_ID + "' AND ";
                    }
                    if (Orden.P_Generar_Orden_Estatus != null && Orden.P_Generar_Orden_Estatus != "")
                    {
                        Mi_SQL += Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + Validar_Operador_Comparacion(Orden.P_Generar_Orden_Estatus) + " AND ";
                    }
                    if (Mi_SQL.EndsWith(" AND "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 5);
                    }
                    if (Mi_SQL.EndsWith(" WHERE "))
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 7);
                    }
                }
                if (Orden.P_Agrupar_Dinamico != null && Orden.P_Agrupar_Dinamico != "")
                {
                    Mi_SQL += " GROUP BY " + Orden.P_Agrupar_Dinamico;
                }
                if (Orden.P_Ordenar_Dinamico != null && Orden.P_Ordenar_Dinamico != "")
                {
                    Mi_SQL += " ORDER BY " + Orden.P_Ordenar_Dinamico;
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
        #endregion

        internal static void Quitar_Beneficio_Agregar_Observacion(Cls_Ope_Pre_Orden_Variacion_Negocio cls_Ope_Pre_Orden_Variacion_Negocio)
        {
            String Mi_SQL = "";
            String Mensaje;
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();

            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;


                Mi_SQL = " SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " ";
                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                Mi_SQL = Mi_SQL + " left outer JOIN ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " ON ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + "=";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo;
                Mi_SQL = Mi_SQL + " AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_Anio + "=";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + "='ACEPTADA' and ( ";
                Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + "." + Ope_Pre_Contrarecibos.Campo_Estatus + "='PAGADO' OR " + Ope_Pre_Contrarecibos.Campo_Estatus + " IS NULL) AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + "='" + cls_Ope_Pre_Orden_Variacion_Negocio.P_Cuenta_Predial_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " DESC, " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC ";

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    String Observacion_ID = Obtener_ID_Consecutivo(ref Obj_Comando, Ope_Pre_Observaciones.Tabla_Ope_Pre_Observaciones_Orden_Variacion, Ope_Pre_Observaciones.Campo_Observaciones_ID, "", 5);
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Observaciones.Tabla_Ope_Pre_Observaciones_Orden_Variacion + " (";
                    Mi_SQL += Ope_Pre_Observaciones.Campo_Observaciones_ID + ", ";
                    Mi_SQL += Ope_Pre_Observaciones.Campo_No_Orden_Variacion + ", ";
                    Mi_SQL += Ope_Pre_Observaciones.Campo_Año + ", ";
                    Mi_SQL += Ope_Pre_Observaciones.Campo_Descripcion + ", ";
                    Mi_SQL += Ope_Pre_Observaciones.Campo_Usuario_Creo + ", ";
                    Mi_SQL += Ope_Pre_Observaciones.Campo_Fecha_Creo + ") ";
                    Mi_SQL += "VALUES (";
                    Mi_SQL += "'" + Observacion_ID + "', ";
                    Mi_SQL += "'" + dataset.Tables[0].Rows[0][0].ToString() + "', ";
                    Mi_SQL += dataset.Tables[0].Rows[0][1].ToString() + ", ";
                    if (cls_Ope_Pre_Orden_Variacion_Negocio.P_Observaciones_Descripcion != null && cls_Ope_Pre_Orden_Variacion_Negocio.P_Observaciones_Descripcion != "")
                    {
                        Mi_SQL += " UPPER('" + cls_Ope_Pre_Orden_Variacion_Negocio.P_Observaciones_Descripcion + "'), ";
                    }
                    else
                    {
                        Mi_SQL += "NULL, ";
                    }
                    Mi_SQL += "'" + cls_Ope_Pre_Orden_Variacion_Negocio.P_Observaciones_Usuraio + "', SYSDATE) ";

                    //Ejecutar consulta
                    Obj_Comando.CommandText = Mi_SQL;
                    Obj_Comando.ExecuteNonQuery();

                    //Ejecutar transaccion
                    Obj_Transaccion.Commit();
                    Obj_Conexion.Close();
                }
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Mi_SQL + "   ]" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;

            }
        }
        #endregion
    }
}