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
using Presidencia.Nomina_Percepciones_Deducciones;
using System.Collections.Generic;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Text;
using Presidencia.Proveedores.Negocios;
using Presidencia.Cat_Parametros_Nomina.Negocio;

namespace Presidencia.Fonacot
{
    public class Cls_Nom_Fonacot
    {
        /// *******************************************************************************************************************
        /// Nombre: getCollectionListFonacot
        /// 
        /// Descripción: Método que obtiene una lista de créditos fonacot que se descontaran en esta catorcena al empleado.
        /// 
        /// Parámetros: Empleado_ID.- Identificador interno del empleado.
        ///             Nomina_ID.- Nomina de la cuál se esta generando la nómina.
        ///             No_Nomina.- Periodo del cuál se esta generando la nómina.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete. 
        /// Fecha Creó: 03/Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Maodificación:
        /// *******************************************************************************************************************
        public static List<Cls_Percepciones_Deducciones> getCollectionListFonacot(
            String Empleado_ID,
            String Nomina_ID,
            String No_Nomina)
        {
            List<Cls_Percepciones_Deducciones> Lista_Creditos_Fonacot = new List<Cls_Percepciones_Deducciones>();//Lista de créditos que se le descontaran al empleado por concepto de fonacot.
            DataTable Dt_Resultado = null;//Variable que almacena un listado de creditos fonacot.
            Cls_Percepciones_Deducciones newCredito = null;//Objeto que se utilizara como clase de entidad para transaportar las deducciones que corresponde a un prestamo de fonacot.

            try
            {
                //Consultamos los créditos de fonacot que actualmente tiene el empleado.
                Dt_Resultado = Consultar_Creditos(Empleado_ID, Nomina_ID, No_Nomina);

                //Validamos que la consulta tenga resultados.
                if (Dt_Resultado != null)
                {
                    //Utilizamos una expresión LinQ para obtener la información de la tabla de créditos FONACOT.
                    var Creditos = from credito in Dt_Resultado.AsEnumerable()
                                   select new
                                   {
                                       Clave = credito.Field<String>(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID),
                                       Nombre = credito.Field<String>(Cat_Nom_Percepcion_Deduccion.Campo_Nombre),
                                       Cantidad = credito.IsNull(Ope_Nom_Proveedores_Detalles.Campo_Cantidad) ? 0 : credito.Field<Decimal>(Ope_Nom_Proveedores_Detalles.Campo_Cantidad),
                                       Aplica = credito.Field<String>(Cat_Nom_Percepcion_Deduccion.Campo_Aplicar),
                                       Gravable = credito.IsNull(Cat_Nom_Percepcion_Deduccion.Campo_Gravable) ? 0 : credito.Field<Decimal>(Cat_Nom_Percepcion_Deduccion.Campo_Gravable),
                                       Porcentaje_Gravable = credito.IsNull(Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable) ? 0 : credito.Field<Decimal>(Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable)
                                   };

                    //Validamos que la expresión LinQ tenga registros.
                    if (Creditos != null)
                    {
                        foreach (var item_credito in Creditos)
                        {
                            //Creamos una instancia de la clase de Percepciones y/o Deducciones.
                            newCredito = new Cls_Percepciones_Deducciones();

                            newCredito.P_Clabe = item_credito.Clave;
                            newCredito.P_Nombre = item_credito.Nombre;
                            newCredito.P_Monto = Convert.ToDouble(!String.IsNullOrEmpty(item_credito.Cantidad.ToString()) ? item_credito.Cantidad.ToString() : "0");
                            newCredito.P_Aplica = item_credito.Aplica;
                            newCredito.P_Gravable = Convert.ToDouble(!String.IsNullOrEmpty(item_credito.Gravable.ToString()) ? item_credito.Gravable.ToString() : "0");
                            newCredito.P_Porcentaje_Gravable = Convert.ToDouble(!String.IsNullOrEmpty(item_credito.Porcentaje_Gravable.ToString()) ? item_credito.Porcentaje_Gravable.ToString() : "0");

                            //Agregamos la retención que se le hará al empleado por concepto de fonacot.
                            Lista_Creditos_Fonacot.Add(newCredito);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al crear la estructura para devolver los creditos fonacot. Error: [" + Ex.Message + "]");
            }
            return Lista_Creditos_Fonacot;
        }
        /// *******************************************************************************************************************
        /// Nombre: Consultar_Creditos
        /// 
        /// Descripción: Método que consulta los créditos fonacot que se descontaran en esta catorcena al empleado.
        /// 
        /// Parámetros: Empleado_ID.- Identificador interno del empleado.
        ///             Nomina_ID.- Nomina de la cuál se esta generando la nómina.
        ///             No_Nomina.- Periodo del cuál se esta generando la nómina.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete. 
        /// Fecha Creó: 03/Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Maodificación:
        /// *******************************************************************************************************************
        private static DataTable Consultar_Creditos(
            String Empleado_ID,
            String Nomina_ID,
            String No_Nomina)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataSet Ds_Resultado = new DataSet();//Variable que almacenara el resultado.
            DataTable Dt_Resultado = null;//Variable que almacenara el resultado.

            OracleConnection Conexion = new OracleConnection();//Variable que almacenara la conexión.
            OracleCommand Comando = new OracleCommand();//Variable que almacenara el comando que ejecutara las consultas.
            OracleTransaction Transaccion = null;//Variable que controlara las transacciones contra la base de datos.
            OracleDataAdapter Adaptador = new OracleDataAdapter();//Variable que se utilizara para cargar la tabla de resultado.

            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;//establecemos la cadena de conexión.
            Conexion.Open();//Abrimos la conexión.

            Transaccion = Conexion.BeginTransaction();
            Comando.Connection = Conexion;
            Comando.Transaction = Transaccion;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Nombre + ", ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Cantidad + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Aplicar + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Gravable + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Porcentaje_Gravable);

                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles);
                Mi_SQL.Append(" left outer join " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " on ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Percepcion_Deduccion_ID + "=");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);

                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Nomina_ID + "='" + Nomina_ID + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Periodo + "=" + No_Nomina);
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles + "." + Ope_Nom_Proveedores_Detalles.Campo_Estatus + "='ACEPTADO'");

                Comando.CommandText = Mi_SQL.ToString();
                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(Ds_Resultado);
                Dt_Resultado = Ds_Resultado.Tables[0];

                Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception("Error al consultar los créditos fonacot. Error: [" + Ex.Message + "]");
            }
            finally { Conexion.Close(); }
            return Dt_Resultado;
        }
        /// *******************************************************************************************************************
        /// Nombre: Quitar_Deducciones_Fonacot
        /// 
        /// Descripción: Método que consulta un listado de deducciones que tiene asiganadas el proveedor de fonacot.
        ///              recorre la lista que es pasada como parámetro y que ademas contiene un listado de deducciones fijas que
        ///              el empleado tiene asiganadas. Una vez con esta información removemos las deducciones fijas que le pertenecen
        ///              al proveedor fonacot.
        ///             
        /// Parámetros: Lista.- Estructura que almacena un listado de deducciones fijas que tiene asigandas el empleado. 
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete. 
        /// Fecha Creó: 03/Marzo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Maodificación:
        /// *******************************************************************************************************************
        public static void Quitar_Deducciones_Fonacot(ref List<Cls_Percepciones_Deducciones> Lista)
        {
            Cls_Cat_Nom_Parametros_Negocio INF_PARAMETROS = null;
            Cls_Cat_Nom_Proveedores_Negocio Obj_Proveedores = new Cls_Cat_Nom_Proveedores_Negocio();
            DataTable Dt_Deducciones_Fonacot = null;

            try
            {
                INF_PARAMETROS = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

                Obj_Proveedores.P_Proveedor_ID = INF_PARAMETROS.P_Proveedor_Fonacot;
                Dt_Deducciones_Fonacot = Obj_Proveedores.Consultar_Deducciones_Proveedor();

                if (Dt_Deducciones_Fonacot != null)
                {
                    var Conceptos = from concepto in Dt_Deducciones_Fonacot.AsEnumerable()
                                    select new
                                    {
                                        Clave = concepto.Field<String>(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID)
                                    };

                    foreach (var item_concepto in Conceptos)
                    {
                        var items = from item in Lista
                                    where item.P_Clabe == item_concepto.Clave
                                    select item;

                        foreach (Cls_Percepciones_Deducciones registro in items)
                        {
                            Lista.Remove(registro);
                            break;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al remover las deducciones que le pertencen a fonacot. Error: [" + Ex.Message + "]");
            }
        }
        /// *******************************************************************************************************************
        /// Nombre: Consultar_Creditos_Liquidados
        /// 
        /// Descripción: Método que consulta los créditos de fonacot ya tienen un saldo menor a 1. Los cuales se tomaran como
        ///              prestamos liquidados.
        ///
        /// Parámetros: Empleado_ID.- Identificador del empleado que utiliza el sistema.
        ///             No_Fonacot.- No de fonacot del empleado.
        ///             No_Crédito.- Identificador de fonacot de la compra del empleado.
        ///             Estatus.- Indica si el estatus del prestamo.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete. 
        /// Fecha Creó: Mayo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Maodificación:
        /// *******************************************************************************************************************
        public static Credito_Fonacot Consultar_Creditos_Liquidados(
            String Empleado_ID, String No_Fonacot, String No_Credito, String Estatus)
        {
            Credito_Fonacot Credito = new Credito_Fonacot();
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Empleados = null;

            try
            {
                Mi_SQL.Append("select ");
                Mi_SQL.Append("sum(" + Ope_Nom_Proveedores_Detalles.Campo_Cantidad + ") as CANTIDAD_RETENIDA, ");
                Mi_SQL.Append("(" + Ope_Nom_Proveedores_Detalles.Campo_Plazo + " * " + Ope_Nom_Proveedores_Detalles.Campo_Retencion_Mensual + ") as IMPORTE, ");
                Mi_SQL.Append("((" + Ope_Nom_Proveedores_Detalles.Campo_Plazo + " * " + Ope_Nom_Proveedores_Detalles.Campo_Retencion_Mensual + ") - (sum(" + 
                    Ope_Nom_Proveedores_Detalles.Campo_Cantidad + "))) as SALDO ");

                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Tabla_Ope_Nom_Proveedores_Detalles);

                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Campo_Empleado_ID + "='" + Empleado_ID + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Campo_No_Fonacot + "='" + No_Fonacot + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Campo_No_Credito + "='" + No_Credito + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Ope_Nom_Proveedores_Detalles.Campo_Estatus + "='" + Estatus + "'");

                Mi_SQL.Append(" group by " + Ope_Nom_Proveedores_Detalles.Campo_Plazo + ", " + Ope_Nom_Proveedores_Detalles.Campo_Retencion_Mensual);

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                //Validamos que el crédito para hacer un descuento de mas al empleado.
                Verificar_Si_Credito_Termino_Pagar(ref Credito, Dt_Empleados);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los prestamos liquidados. Error: [" + Ex.Message + "]");
            }
            return Credito;
        }
        /// *******************************************************************************************************************
        /// Nombre: Verificar_Si_Credito_Termino_Pagar
        /// 
        /// Descripción: Método que verifica si es prestamo tiene un saldo menor a 1. Los cuales se tomaran como
        ///              prestamos liquidados.
        ///
        /// Parámetros: Credito.- Objeto que utilizaremos para retornar los resultados a la capa de usuario.
        ///             Dt_Resultado.- Estructura que almacenara el resultado de la consulta.
        /// 
        /// Usuario Creó: Juan Alberto Hernández Negrete. 
        /// Fecha Creó: Mayo/2012
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Maodificación:
        /// *******************************************************************************************************************
        private static void Verificar_Si_Credito_Termino_Pagar(ref Credito_Fonacot Credito, DataTable Dt_Resultado)
        {
            Boolean Credito_Pagado = false;

            try
            {
                if (Dt_Resultado is DataTable) {
                    var items_creditos = from item_credito in Dt_Resultado.AsEnumerable()
                                         select new
                                         {
                                             Cantidad_Retenida = (item_credito.IsNull("CANTIDAD_RETENIDA") ? 0 :
                                                Convert.ToDouble(item_credito.Field<Decimal>("CANTIDAD_RETENIDA"))),

                                             Importe = (item_credito.IsNull("IMPORTE") ? 0 :
                                                 Convert.ToDouble(item_credito.Field<Decimal>("IMPORTE"))),

                                             Saldo = (item_credito.IsNull("SALDO") ? 0 :
                                                Convert.ToDouble(item_credito.Field<Decimal>("SALDO")))
                                         };

                    if (items_creditos != null) {
                        foreach (var item in items_creditos) {
                            if (item.Saldo < 1)
                                Credito_Pagado = true;                         
                            else if (item.Cantidad_Retenida == item.Importe)
                                Credito_Pagado = true;

                            if (Credito_Pagado)
                            {
                                Credito.Cantidad_Retenida = item.Cantidad_Retenida;
                                Credito.Importe = item.Importe;
                                Credito.Saldo = item.Saldo;
                                Credito.Estatus = "PAGADO";
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al verificar si el crédito ya se termino de pagar y por lo tanto esta carga ya no es valida. Error: [" + Ex.Message + "]");
            }
        }
    }

    public class Credito_Fonacot
    {
        public Double Cantidad_Retenida = 0;
        public Double Importe = 0;
        public Double Saldo = 0;
        public String Estatus = "PROCESO";
    }
}