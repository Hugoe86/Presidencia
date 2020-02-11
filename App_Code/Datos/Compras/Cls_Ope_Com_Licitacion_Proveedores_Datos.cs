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
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Licitacion_Proveedores.Negocio;
using Presidencia.Sessiones;


/// <summary>
/// Summary description for Cls_Ope_Com_Licitacion_Proveedores_Datos
/// </summary>
namespace Presidencia.Licitacion_Proveedores.Datos
{
    public class Cls_Ope_Com_Licitacion_Proveedores_Datos
    {

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos
        public Cls_Ope_Com_Licitacion_Proveedores_Datos()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        ///*******************************************************************************
        /// MODIFICACIONES
        ///*******************************************************************************
        #region Modificaciones
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Licitacion_Detalles
        ///DESCRIPCIÓN: Metodo que permite actualizar los detalles de la licitacion
        ///PARAMETROS:1.- Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 17/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Licitacion_Detalles(Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro)
        {
         String Mi_SQL = "";
         //RECORREMOS LOS DETALLES DE LA LICITACION,   
         for (int i=0; i < Datos_Lic_Pro.P_Dt_Productos.Rows.Count; i ++)
         {
            
             Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                      " SET " + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                      "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado] + "'" +
                      ", " + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                      "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado] + "'" +
                      ", " + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                      "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IVA_Cotizado] + "'" +
                      ", " + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                      "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IEPS_Cotizado] + "'" +
                      ", " + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                      "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Subtota_Cotizado] + "'" +
                      ", " + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                      "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado] + "'" +
                      ", " + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                      "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Proveedor_ID] + "'" +
                      ", " + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                      "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Nombre_Proveedor] + "'" +
                      ", " + Ope_Com_Req_Producto.Campo_Giro_ID +
                      "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Giro_ID] + "'" +
                      " WHERE " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                      "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID] +"'";
             OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
         }//fIN FOR I
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Licitacion
        ///DESCRIPCIÓN: Metodo que permite actualizar los detalles de la licitacion
        ///PARAMETROS:1.- Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 17/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Licitacion(Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro)
        {
            String Mi_SQL = "UPDATE " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones +
                     " SET " + Ope_Com_Licitaciones.Campo_Estatus +
                     "='" + Datos_Lic_Pro.P_Estatus + "'" +
                     ", " + Ope_Com_Licitaciones.Campo_Total_Cotizado +
                     "='" + Datos_Lic_Pro.P_Monto_Total + "'" +
                     " WHERE " + Ope_Com_Licitaciones.Campo_No_Licitacion +
                     "='" + Datos_Lic_Pro.P_No_Licitacion + "'";
             OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
             Modificar_Licitacion_Detalles(Datos_Lic_Pro);
            //En caso de que el estatus sea TERMINADA se realiza la modificacion de las requisiciones correspondistes a esta licitacion
             if (Datos_Lic_Pro.P_Estatus == "TERMINADA")
             {
                 //Modificamos los presupuestos 
                 Modificar_Presupuesto(Datos_Lic_Pro);
                 //primero se obtiene el listado de requisiciones pertenecientes a esta cotizacion 
                 Mi_SQL = "SELECT " + Ope_Com_Licitaciones.Campo_Lista_Requisiciones +
                         " FROM " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones +
                         " WHERE " + Ope_Com_Licitaciones.Campo_No_Licitacion +
                         "= '" + Datos_Lic_Pro.P_No_Licitacion + "'";
                 DataTable Dt_Req = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                 String[] Arr_Requisiciones = Dt_Req.Rows[0][Ope_Com_Cotizaciones.Campo_Lista_Requisiciones].ToString().Split(',');
                 for (int i = 0; i < Arr_Requisiciones.Length; i++)
                 {
                     Modificar_Montos_Cotizados_Requisiciones(Arr_Requisiciones[i]);
                 }
             }
             if (Datos_Lic_Pro.P_Estatus == "CANCELADA")
             {
                 Liberar_Requisiciones(Datos_Lic_Pro);
             }

        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:Modificar_Montos_Cotizados_Requisiciones
        ///DESCRIPCIÓN: Metodo que permite actualizar los los montos de cotizacion de las requisiciones
        ///PARAMETROS:1.- String No_Requisicion
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 17/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Montos_Cotizados_Requisiciones(String No_Requisicion)
        {
            String Mi_SQL = "SELECT * FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                "='" + No_Requisicion + "'";
            DataTable Dt_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            double IVA_COTIZADO = 0;
            double IEPS_COTIZADO = 0;
            double SUBTOTAL_COTIZADO = 0;
            double TOTAL_COTIZADO = 0;
            for (int i = 0; i < Dt_Productos.Rows.Count; i++)
            {
                IVA_COTIZADO += double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IVA_Cotizado].ToString());
                IEPS_COTIZADO += double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IEPS_Cotizado].ToString());
                SUBTOTAL_COTIZADO += double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Subtota_Cotizado].ToString());
                TOTAL_COTIZADO += double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString());
            }//fin del For

            //POR ULTIMO MODIFICAMOS LOS MONTOS COTIZADOS 
            Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " SET " + Ope_Com_Requisiciones.Campo_Estatus +
                "='COTIZADA'" +
                ", " + Ope_Com_Requisiciones.Campo_IVA_Cotizado +
                "='" + IVA_COTIZADO.ToString() + "'" +
                ", " + Ope_Com_Requisiciones.Campo_IEPS_Cotizado +
                "='" + IEPS_COTIZADO.ToString() + "'" +
                ", " + Ope_Com_Requisiciones.Campo_Subtotal_Cotizado +
                "='" + SUBTOTAL_COTIZADO.ToString() + "'" +
                ", " + Ope_Com_Requisiciones.Campo_Total_Cotizado +
                "='" + TOTAL_COTIZADO.ToString() + "'" +
                ", " + Ope_Com_Requisiciones.Campo_Empleado_Cotizacion_ID +
                "='" + Cls_Sessiones.Empleado_ID + "'" +
                ", " + Ope_Com_Requisiciones.Campo_Fecha_Cotizacion +
                "=SYSDATE" +
                " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                "='" + No_Requisicion +"'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Presupuesto
        ///DESCRIPCIÓN: Metodo que permite modificar el presupuesto de las partidas
        ///PARAMETROS:1.- Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 17/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Presupuesto(Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro)
        {
            String Mi_SQL = "";
            double Diferencia = 0;
            String Partida_ID = "";
            String Dependencia_ID = "";
            String Proyecto_ID = "";
            String FF = "";//Fuente de financiamiento 
            double Presupuesto_Disponible = 0;
            double Monto_Anterior = 0;
            bool Suma_Diferencia = false;
            //Recorremos el Dt_Productos perteneciente al registro de Comite de compras
            for (int i = 0; i < Datos_Lic_Pro.P_Dt_Productos.Rows.Count; i++)
            {
                double Monto_Cotizado = double.Parse(Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString().Trim());
                //REalizamos la consulta que nos traera la dependencia, la partida y el proyecto al que pertenece el producto actual
                Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Campo_Partida_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID +
                        ", (SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                        " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                        " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        " =(SELECT " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                        " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                        " WHERE " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                        "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID].ToString().Trim() + "'))" +
                        ", " + Ope_Com_Req_Producto.Campo_Monto_Total +
                        ", " + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID +
                        " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                        " WHERE " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                        "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID].ToString().Trim() + "'";
                DataTable Dt_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                Partida_ID = Dt_Datos.Rows[0][Ope_Com_Req_Producto.Campo_Partida_ID].ToString();
                Proyecto_ID = Dt_Datos.Rows[0][Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID].ToString();
                Dependencia_ID = Dt_Datos.Rows[0][2].ToString();
                FF = Dt_Datos.Rows[0][Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID].ToString();
                Monto_Anterior = double.Parse(Dt_Datos.Rows[0][Ope_Com_Req_Producto.Campo_Monto_Total].ToString());
                // PASO 1 VERIFICAMOS CUAL DE LOS 2 MONTOS ES MAYOR SI EL COTIZADO O  EL ANTERIOR
                if (Monto_Cotizado > Monto_Anterior)
                {
                    //Obtenemos la resta
                    Diferencia = Monto_Cotizado - Monto_Anterior;
                    Suma_Diferencia = true;
                }
                if (Monto_Cotizado < Monto_Anterior)
                {
                    //obtener resta
                    Diferencia = Monto_Anterior - Monto_Cotizado;
                    Suma_Diferencia = false;
                }

                DataTable Dt_Presupuestos = Consultar_Presupuesto_Partidas(Partida_ID, Dependencia_ID, Proyecto_ID, FF);
                Presupuesto_Disponible = double.Parse(Dt_Presupuestos.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString());

                //MODIFICAMOS LOS PRESUPUESTOS DE ACUERDO AL CASO EN EL QUE ENTRE EL MONTO RESTANTE DE LO COTIZADO 
                //Es true cuando necesitamos pedir mas presupuesto
                //Es false si sobra dinero, osea que se necesita liberar presupuesto ps este presupuesto sobro
                if (Suma_Diferencia == true)
                {
                    if (Diferencia < Presupuesto_Disponible)
                    {
                        //se ACTUALIZAN LOS PRESUPUESTOS DE LA DEPENDENCIA, CORRESPONDIENTE AL PROYECTO Y PARTIDA

                        Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                            " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                            " =" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " - " + Diferencia.ToString() +
                            "," + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                            "=" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " + " + Diferencia.ToString() +
                            " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                            "='" + Partida_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                            "='" + Proyecto_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
                            "='" + FF + "'" + 
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio +
                            " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")" +
                            " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                            " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                            "='" + Partida_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                            "='" + Proyecto_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                            "= TO_CHAR(SYSDATE,'YYYY'))" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                            "= TO_CHAR(SYSDATE,'YYYY')" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                            "='" + Dependencia_ID + "'";
                        //Sentencia que ejecuta el query
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                        //ACTUALIZAMOS LOS PRESUPUESTOS DE LA PARTIDA
                        //Mi_SQL = "UPDATE " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida +
                        //    " SET " + Ope_Com_Pres_Partida.Campo_Monto_Disponible +
                        //    " =" + Ope_Com_Pres_Partida.Campo_Monto_Disponible + " - " + Diferencia.ToString() +
                        //    "," + Ope_Com_Pres_Partida.Campo_Monto_Comprometido +
                        //    "=" + Ope_Com_Pres_Partida.Campo_Monto_Comprometido + " + " + Diferencia.ToString() +
                        //    " WHERE " + Ope_Com_Pres_Partida.Campo_Partida_ID +
                        //    "='" + Partida_ID + "'" +
                        //    " AND " + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto +
                        //    "= TO_CHAR(SYSDATE,'YYYY')";
                        //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        //ACUTUALIZAMOS LOS PRESUPUESTOS DEL PROYECTO
                        //Mi_SQL = "UPDATE " + Ope_Com_Pres_Prog_Proy.Tabla_Ope_Com_Pres_Prog_Proy +
                        //    " SET " + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible +
                        //    " =" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible + " - " + Diferencia.ToString() +
                        //    "," + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido +
                        //    "=" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido + " + " + Diferencia.ToString() +
                        //    " WHERE " + Ope_Com_Pres_Prog_Proy.Campo_Pres_Prog_Proy_ID +
                        //    "='" + Proyecto_ID + "'" +
                        //    " AND " + Ope_Com_Pres_Prog_Proy.Campo_Anio_Presupuesto +
                        //    "= TO_CHAR(SYSDATE,'YYYY')";
                        //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    }
                    else
                    {
                        //Si no existe presupuesto modificamos a cero todos los valores cotizados 

                        Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                         " SET " + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                         " ='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                         " ='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                         "='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                         "='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                         "='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                         "='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                         "='0'" +
                         ", " + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                         "='0'" +
                         " WHERE " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                         "='" + Datos_Lic_Pro.P_Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID].ToString().Trim() + "'";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }

                }//fin if SumaDiferencia
                else
                {
                    //Modificamos el presupuesto, ya que se resta el monto que sobro pues el valor cotizado es menor k el anterior
                    //se ACTUALIZAN LOS PRESUPUESTOS DE LA DEPENDENCIA, CORRESPONDIENTE AL PROYECTO Y PARTIDA

                    Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                        " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                        " =" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " + " + Diferencia.ToString() +
                        "," + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                        "=" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " - " + Diferencia.ToString() +
                        " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                        "='" + Partida_ID + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                        "='" + Proyecto_ID + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
                        "='" + FF + "'" + 
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio +
                        " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")" +
                        " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                        " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                        "='" + Partida_ID + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                        "='" + Proyecto_ID + "'" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                        "= TO_CHAR(SYSDATE,'YYYY'))" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                        "= TO_CHAR(SYSDATE,'YYYY')" +
                        " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                        "='" + Dependencia_ID + "'";
                    //Sentencia que ejecuta el query
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    //ACTUALIZAMOS LOS PRESUPUESTOS DE LA PARTIDA
                    //Mi_SQL = "UPDATE " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida +
                    //    " SET " + Ope_Com_Pres_Partida.Campo_Monto_Disponible +
                    //    " =" + Ope_Com_Pres_Partida.Campo_Monto_Disponible + " + " + Diferencia.ToString() +
                    //    "," + Ope_Com_Pres_Partida.Campo_Monto_Comprometido +
                    //    "=" + Ope_Com_Pres_Partida.Campo_Monto_Comprometido + " - " + Diferencia.ToString() +
                    //    " WHERE " + Ope_Com_Pres_Partida.Campo_Partida_ID +
                    //    "='" + Partida_ID + "'" +
                    //    " AND " + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto +
                    //    "= TO_CHAR(SYSDATE,'YYYY')";
                    //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //ACUTUALIZAMOS LOS PRESUPUESTOS DEL PROYECTO
                    //Mi_SQL = "UPDATE " + Ope_Com_Pres_Prog_Proy.Tabla_Ope_Com_Pres_Prog_Proy +
                    //    " SET " + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible +
                    //    " =" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible + " + " + Diferencia.ToString() +
                    //    "," + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido +
                    //    "=" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido + " - " + Diferencia.ToString() +
                    //    " WHERE " + Ope_Com_Pres_Prog_Proy.Campo_Pres_Prog_Proy_ID +
                    //    "='" + Proyecto_ID + "'" +
                    //    " AND " + Ope_Com_Pres_Prog_Proy.Campo_Anio_Presupuesto +
                    //    "= TO_CHAR(SYSDATE,'YYYY')";
                    //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                }
                //Como se puede observar no se agrego caso para cuando el monto cotizado y el monto anterior sean iguales ya que no se afecta nada en este caso

            }//Fin del FOR

            //

        }

        #endregion Modificaciones

        ///*******************************************************************************
        /// CONSULTAS
        ///*******************************************************************************
        #region Consultas

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Presupuesto_Partidas
        ///DESCRIPCIÓN: Metodo que consulta la tabla de Presupuestos partidas
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 22/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Presupuesto_Partidas(String Partida, String Dependencia, String Proyecto, String FF)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                            " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                            " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + "= '" + Partida + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "='" + Proyecto + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "='" + Dependencia + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + "='" + FF +"'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + "= TO_CHAR(SYSDATE,'YYYY')" +
                            " ORDER BY " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + " DESC";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Licitaciones
        ///DESCRIPCIÓN: Metodo que genera la consulta de las licitaciones segun los parametros
        ///PARAMETROS:1.- Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 15/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Licitaciones(Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro)
        {
            String Mi_SQL = "SELECT " + Ope_Com_Licitaciones.Campo_No_Licitacion +
                     ", " + Ope_Com_Licitaciones.Campo_Folio +
                     ", " + Ope_Com_Licitaciones.Campo_Estatus +
                     ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Inicio + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Inicio +
                     ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Fin + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Fin +
                     ", " + Ope_Com_Licitaciones.Campo_Tipo +
                     ", " + Ope_Com_Licitaciones.Campo_Clasificacion +
                     " FROM " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones;
            if (Datos_Lic_Pro.P_Estatus == null)
            {
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Licitaciones.Campo_Estatus +
                        " IN ('GENERADA','ASIGNADA')";
            }
            if (Datos_Lic_Pro.P_Estatus != null)
            {
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Licitaciones.Campo_Estatus +
                        " ='" + Datos_Lic_Pro.P_Estatus + "'";
            }
            if (Datos_Lic_Pro.P_No_Licitacion != null)
            {
                Mi_SQL = "SELECT " + Ope_Com_Licitaciones.Campo_Folio +
                        ", " + Ope_Com_Licitaciones.Campo_Estatus +
                        ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Inicio + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Inicio +
                        ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Fin + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Fin +
                        ", " + Ope_Com_Licitaciones.Campo_Justificacion +
                        ", " + Ope_Com_Licitaciones.Campo_Comentarios +
                        ", " + Ope_Com_Licitaciones.Campo_Monto_Total +
                        ", " + Ope_Com_Licitaciones.Campo_Tipo +
                        ", " + Ope_Com_Licitaciones.Campo_Clasificacion +
                        ", " + Ope_Com_Licitaciones.Campo_Total_Cotizado +
                        ", " + Ope_Com_Licitaciones.Campo_Lista_Requisiciones +
                        " FROM " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones +
                        " WHERE " + Ope_Com_Licitaciones.Campo_No_Licitacion +                
                        " ='" + Datos_Lic_Pro.P_No_Licitacion + "'";
            }
            if (Datos_Lic_Pro.P_Folio != null)
            {
                Mi_SQL = "SELECT " + Ope_Com_Licitaciones.Campo_No_Licitacion +
                     ", " + Ope_Com_Licitaciones.Campo_Folio +
                     ", " + Ope_Com_Licitaciones.Campo_Estatus +
                     ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Inicio + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Inicio +
                     ", TO_CHAR(" + Ope_Com_Licitaciones.Campo_Fecha_Fin + ",'DD/MON/YYYY') AS " + Ope_Com_Licitaciones.Campo_Fecha_Fin +
                     ", " + Ope_Com_Licitaciones.Campo_Tipo +
                     ", " + Ope_Com_Licitaciones.Campo_Clasificacion +
                     " FROM " + Ope_Com_Licitaciones.Tabla_Ope_Com_Licitaciones +
                     " WHERE " + Ope_Com_Licitaciones.Campo_Estatus +
                     " IN ('GENERADA','ASIGNADA')" +
                     " AND UPPER(" + Ope_Com_Licitaciones.Campo_Folio +
                        ") LIKE UPPER('%" + Datos_Lic_Pro.P_Folio + "%')";
            }

            if (Datos_Lic_Pro.P_Fecha_Inicio != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Licitaciones.Campo_Fecha_Inicio +
                    " BETWEEN '" + Datos_Lic_Pro.P_Fecha_Inicio +
                    "' AND '" + Datos_Lic_Pro.P_Fecha_Fin + "'" +
                    " AND " + Ope_Com_Licitaciones.Campo_Fecha_Fin +
                    " BETWEEN '" + Datos_Lic_Pro.P_Fecha_Inicio +
                    "' AND '" + Datos_Lic_Pro.P_Fecha_Fin + "'";
            }


            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Detalle
        ///DESCRIPCIÓN: Metodo que consulta los detalles de la licitacion pero enfocandose a los productos.
        ///PARAMETROS:1.- Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 15/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Productos_Detalle(Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro)
        {
            String Mi_SQL = "";
            switch (Datos_Lic_Pro.P_Tipo)
            {
                case "PRODUCTO":
                    Mi_SQL = "SELECT DET." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                            ", PRO." + Cat_Com_Productos.Campo_Producto_ID + " AS PROD_SERV_ID" +
                            ", PRO." + Cat_Com_Productos.Campo_Nombre + " AS NOMBRE_PROD_SERV" +
                            ", DET." + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", DET." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                            ", CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET" +
                            " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRO" +
                            " ON PRO." + Cat_Com_Productos.Campo_Producto_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ" +
                            " ON REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDA_ESP" +
                            " ON PARTIDA_ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + "=" +
                            "DET." + Ope_Com_Req_Producto.Campo_Partida_ID +
                            " JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " PARTIDA_GEN" +
                            " ON PARTIDA_GEN." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID +
                            " = PARTIDA_ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID +
                            " JOIN " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " CONCEPTO" +
                            " ON CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID +
                            " = PARTIDA_GEN." + Cat_SAP_Partida_Generica.Campo_Concepto_ID +
                            " WHERE REQ." + Ope_Com_Requisiciones.Campo_No_Licitacion +
                            " = '" + Datos_Lic_Pro.P_No_Licitacion + "'";
                    break;
                case "SERVICIO":
                    Mi_SQL = "SELECT DET." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                            ", SER." + Cat_Com_Servicios.Campo_Servicio_ID + " AS PROD_SER_ID" +
                            ", SER." + Cat_Com_Servicios.Campo_Nombre + " AS NOMBRE_PROD_SERV" +
                            ", DET." + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", DET." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                            ", CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                            ", DET." + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                            ", DET." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET" +
                            " JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SER" +
                            " ON SER." + Cat_Com_Servicios.Campo_Servicio_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ" +
                            " ON REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " = DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDA_ESP" +
                            " ON PARTIDA_ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + "=" +
                            "DET." + Ope_Com_Req_Producto.Campo_Partida_ID +
                            " JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " PARTIDA_GEN" +
                            " ON PARTIDA_GEN." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID +
                            " = PARTIDA_ESP." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID +
                            " JOIN " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " CONCEPTO" +
                            " ON CONCEPTO." + Cat_Sap_Concepto.Campo_Concepto_ID +
                            " = PARTIDA_GEN." + Cat_SAP_Partida_Generica.Campo_Concepto_ID +
                            " WHERE REQ." + Ope_Com_Requisiciones.Campo_No_Licitacion +
                            " = '" + Datos_Lic_Pro.P_No_Licitacion + "'";

                    break;

            }
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Giro_Proveedor
        ///DESCRIPCIÓN: Metodo que consulta los detalles de la licitacion pero enfocandose a los productos.
        ///PARAMETROS:1.- Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 15/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Concepto_Proveedor(Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro)
        {
            String Mi_SQL = "SELECT PROVEEDOR." + Cat_Com_Proveedores.Campo_Proveedor_ID +
                            ", PROVEEDOR." + Cat_Com_Proveedores.Campo_Nombre +
                            " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDOR" +
                            " JOIN " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor + " GIRO" + 
                            " ON GIRO." + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID +
                            "= PROVEEDOR." + Cat_Com_Proveedores.Campo_Proveedor_ID +
                            " WHERE GIRO." + Cat_Com_Giro_Proveedor.Campo_Giro_ID + "='" + Datos_Lic_Pro.P_Concepto_ID + "'";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Impuesto_Producto
        ///DESCRIPCIÓN: Metodo que consulta los impuestos de los productos
        ///PARAMETROS:1.- Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 15/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Impuesto_Producto(Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro)
        {
            String Mi_SQL = "SELECT PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Clave +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO_NOMBRE" +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Costo + " AS PRECIO_UNITARIO" +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_ID +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_2_ID +
                         ",(SELECT " + Cat_Com_Impuestos.Campo_Nombre +
                         " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                         " WHERE PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_ID + "=" +
                         Cat_Com_Impuestos.Campo_Impuesto_ID + ") AS TIPO_IMPUESTO_1 " +
                         ",(SELECT " + Cat_Com_Impuestos.Campo_Nombre +
                         " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                         " WHERE PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_2_ID + "=" +
                         Cat_Com_Impuestos.Campo_Impuesto_ID + ") AS TIPO_IMPUESTO_2 " +
                         ",(SELECT " + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto +
                         " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                         " WHERE PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_ID + "=" +
                         Cat_Com_Impuestos.Campo_Impuesto_ID + ") AS IMPUESTO_PORCENTAJE_1 " +
                         ",(SELECT " + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto +
                         " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                         " WHERE PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_2_ID + "=" +
                         Cat_Com_Impuestos.Campo_Impuesto_ID + ") AS IMPUESTO_PORCENTAJE_2 " +
                         " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS" +
                         " JOIN " + Cat_Com_Impuestos.Tabla_Cat_Impuestos + " IMPUESTOS" +
                         " ON IMPUESTOS." + Cat_Com_Impuestos.Campo_Impuesto_ID + "= PRODUCTOS." + Cat_Com_Productos.Campo_Impuesto_ID +
                         " WHERE PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID  +
                         " ='" + Datos_Lic_Pro.P_Producto_ID+"'";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        #endregion Consultas


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Liberar_Requisiciones
        ///DESCRIPCIÓN: Metodo que permite liberar las requisiciones que pertenecen a una 
        ///licitacion cuando esta es cancelada. Pasa todos los valores a null para dejar 
        ///disponible las requisiciones para proximos procesos.
        ///PARAMETROS:1.- Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 17/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Liberar_Requisiciones(Cls_Ope_Com_Licitacion_Proveedores_Negocio Datos_Lic_Pro)
        {
            //CAmbiamos el estatus de las requisiciones a FILTRADA Y EL 
            String Mi_Sql = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " SET " + Ope_Com_Requisiciones.Campo_Estatus +
                    "='FILTRADA'" +
                    ", " + Ope_Com_Requisiciones.Campo_Tipo_Compra +
                    "=NULL " +
                    ", " + Ope_Com_Requisiciones.Campo_No_Licitacion +
                    "= NULL" +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                    " IN (" + Datos_Lic_Pro.P_Lista_Requisiciones + ")";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            //MODIFICAMOS LOS DETALLES DE LA REQUISICION
            Mi_Sql = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                " SET " + Ope_Com_Req_Producto.Campo_IVA_Cotizado + "=NULL," +
                Ope_Com_Req_Producto.Campo_IEPS_Cotizado + "=NULL," +
                Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado + "=NULL," +
                Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + "=NULL," +
                Ope_Com_Req_Producto.Campo_Subtota_Cotizado + "=NULL," +
                Ope_Com_Req_Producto.Campo_Total_Cotizado + "=NULL," +
                Ope_Com_Req_Producto.Campo_Proveedor_ID + "=NULL," +
                Ope_Com_Req_Producto.Campo_Nombre_Proveedor + "=NULL," +
                Ope_Com_Req_Producto.Campo_Empleado_Cotizador_ID + "=NULL" +
                " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                " IN(" + Datos_Lic_Pro.P_Lista_Requisiciones + ")";

            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
        }
        #endregion Fin_Metodos

    }//fin del class
}//fin del namespace