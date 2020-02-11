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
using Presidencia.Constantes;
using Presidencia.Administrar_Requisiciones.Negocios;
using Presidencia.Sessiones;
using Presidencia.Generar_Requisicion.Datos;

namespace Presidencia.Administrar_Requisiciones.Datos
{
    public class Cls_Ope_Com_Administrar_Requisiciones_Datos
    {
        public Cls_Ope_Com_Administrar_Requisiciones_Datos()
        {
        }

        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Requisicion
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para modificar una requisicion
        ///PARAMETROS:   1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Requisicion(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "";
           
            //DE ACUERDO AL ESTATUS MODIFICAMOS LA REQUISICION
            switch (Requisicion_Negocio.P_Estatus.Trim())
            {
                case "AUTORIZADA":
                    
                        Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                                Ope_Com_Requisiciones.Campo_Estatus + " = '" + Requisicion_Negocio.P_Estatus + "', " +
                                Ope_Com_Requisiciones.Campo_Empleado_Autorizacion_ID + " = '" + Requisicion_Negocio.P_Empleado_ID + "', " +
                                Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + " = SYSDATE" +
                                " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
                    
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                break;
                
                case "RECHAZADA":
                    Mi_SQL ="UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                            Ope_Com_Requisiciones.Campo_Estatus + "= 'EN CONSTRUCCION', " +
                            Ope_Com_Requisiciones.Campo_Fecha_Rechazo +" =SYSDATE, " +
                            " ALERTA='AMARILLO', " +
                            Ope_Com_Requisiciones.Campo_Empleado_Rechazo_ID +"='" + Requisicion_Negocio.P_Empleado_ID+"'" +
                            " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                break;

                case "CANCELADA":
                //    Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                //            Ope_Com_Requisiciones.Campo_Estatus + "= '"+ Requisicion_Negocio.P_Estatus+"', " +
                //            Ope_Com_Requisiciones.Campo_Fecha_Cancelada + " =SYSDATE, " +
                //            Ope_Com_Requisiciones.Campo_Empleado_Cancelada_ID + "='" + Requisicion_Negocio.P_Empleado_ID + "'" +
                //            " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
                //    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                ////Primero se consultan los productos de esta requisicion
                //    //rEALIZAMOS LA CONSULTA PARA OBTENER TODOS LOS PRODUCTOS DE LA REQUISICION CON SU PARTIDA CORRESPONDIENTE
                //    Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                //             ", " + Ope_Com_Req_Producto.Campo_Monto_Total +
                //             ", " + Ope_Com_Req_Producto.Campo_Partida_ID +
                //             ", " + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID +
                //             ", " + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID +
                //             " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                //             " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + "='" + Requisicion_Negocio.P_Requisicion_ID + "'";
                //    DataTable Dt_Partidas_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                ////llamamos el metodo para liberar presupuestos
                //    Liberar_Presupuesto_Cancelada(Dt_Partidas_Productos,Requisicion_Negocio.P_Dependencia_ID,Requisicion_Negocio);               
                //   if(Requisicion_Negocio.P_Tipo_Articulo =="PRODUCTO" && Requisicion_Negocio.P_Tipo=="STOCK")
                //   {
                //       Liberar_Productos(Requisicion_Negocio);
                //   }                   
                Cls_Ope_Com_Requisiciones_Datos.Cancelar_Requisicion_Stock_Transitoria
                    (Requisicion_Negocio.P_Requisicion_ID, Requisicion_Negocio.P_Tipo, Cls_Sessiones.Nombre_Empleado, Requisicion_Negocio.P_Comentario);

                break;
                case "CONFIRMADA":
                       Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                            Ope_Com_Requisiciones.Campo_Estatus + "= '" + Requisicion_Negocio.P_Estatus + "', " +
                            Ope_Com_Requisiciones.Campo_Fecha_Confirmacion + " =SYSDATE, " +
                            Ope_Com_Requisiciones.Campo_Empleado_Confirmacion_ID + "='" + Requisicion_Negocio.P_Empleado_ID + "'" +
                            " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
                       OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Modificamos el presupuesto
                       Modificar_Presupuesto_Partidas(Requisicion_Negocio);
                break;
                case "COTIZADA-RECHAZADA":

                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SET " +
                        Ope_Com_Requisiciones.Campo_Estatus + "= 'PROVEEDOR', " +
                        Ope_Com_Requisiciones.Campo_Fecha_Cotizada_Rechazada + " =SYSDATE, " +
                        Ope_Com_Requisiciones.Campo_Empleado_Cotizada_Rechazada_ID + "='" + Requisicion_Negocio.P_Empleado_ID + "'," +
                        Ope_Com_Requisiciones.Campo_Alerta + "='AMARILLO2'" +
                        " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
               
                
                //Regresamos los valores de los productos pertenecientes a esta requisicion a nulos solo los cotizados y el proveedor para que sean cotizados nuevamente

                Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                     " SET " + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                     "=NULL" +
                     ", " + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                     "=NULL" +
                     ", " + Ope_Com_Req_Producto.Campo_IVA_Cotizado +
                     "=NULL" +
                     ", " + Ope_Com_Req_Producto.Campo_IEPS_Cotizado +
                     "=NULL" +
                     ", " + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                     "=NULL" +
                     ", " + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                     "=NULL" +
                     ", " + Ope_Com_Req_Producto.Campo_Proveedor_ID +
                     "=NULL" +
                     ", " + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                     "=NULL" +
                     " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                     "='" + Requisicion_Negocio.P_Requisicion_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
               
               //AHORA MODIFICAMOS LOS DETALLES DE LA REQUISICION, COMO LO ES EL MONTO COTIZADO PARA ESTA REQUISICION
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " SET " + Ope_Com_Requisiciones.Campo_IVA_Cotizado +
                    "=NULL" +
                    ", " + Ope_Com_Requisiciones.Campo_IEPS_Cotizado +
                    "=NULL" +
                    ", " + Ope_Com_Requisiciones.Campo_Subtotal_Cotizado +
                    "=NULL" +
                    ", " + Ope_Com_Requisiciones.Campo_Total_Cotizado +
                    "=NULL" +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                    "='" + Requisicion_Negocio.P_Requisicion_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Modificamos todas las propuestas de Cotizacion a EN CONSTRUCCION para poder modificar los montos en caso de ser necesario
                Mi_SQL = "UPDATE " + Ope_Com_Propuesta_Cotizacion.Tabla_Ope_Com_Propuesta_Cotizacion;
                Mi_SQL = Mi_SQL + " SET " + Ope_Com_Propuesta_Cotizacion.Campo_Estatus;
                Mi_SQL = Mi_SQL + "='EN CONSTRUCCION'";
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Propuesta_Cotizacion.Campo_Resultado + "=NULL";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Propuesta_Cotizacion.Campo_No_Requisicion + "='" + Requisicion_Negocio.P_Requisicion_ID.ToString() + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                break;            
        }
            //Sentencia que ejecuta el query
            
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Liberar_Presupuesto_Cancelada
        ///DESCRIPCIÓN: Metodo que consulta los productos que se encunentran en reorden 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 24/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Liberar_Presupuesto_Cancelada(DataTable Dt_Partidas_Productos,String Dependencia_ID,Cls_Ope_Com_Administrar_Requisiciones_Negocio Clase_Negocio)
        {
            String Partida_ID = "";
            String Proyecto_ID = "";
            String FF = "";
            String Monto_Total = "";
            String Mi_SQL = "";
            
            //Creamos for para recorrer todos los productos de la requisicion 

            for (int i = 0; i < Dt_Partidas_Productos.Rows.Count; i++)
            {
                //Obtenemos la Partida, Programa y Fuente de dinanciamiento correspondiente al Producto o servicio
                Partida_ID = Dt_Partidas_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Partida_ID].ToString();
                Proyecto_ID = Dt_Partidas_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID].ToString();
                FF = Dt_Partidas_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID].ToString();
                Monto_Total = Dt_Partidas_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Monto_Total].ToString(); 

                //liberamos el presupuesto que era para este listado
                Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                                " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                                " =" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " + " + Monto_Total +
                                "," + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                                "=" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " - " + Monto_Total +
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
                                "=(SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID  +
                                " FROM "+ Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                                " WHERE "+ Ope_Com_Requisiciones.Campo_Requisicion_ID + "='"+ Clase_Negocio.P_Requisicion_ID +"')";
                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //ACTUALIZAMOS LOS PRESUPUESTOS DE LA PARTIDA
                //Mi_SQL = "UPDATE " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida +
                //    " SET " + Ope_Com_Pres_Partida.Campo_Monto_Disponible +
                //    " =" + Ope_Com_Pres_Partida.Campo_Monto_Disponible + " + " + P_Monto_Total +
                //    "," + Ope_Com_Pres_Partida.Campo_Monto_Comprometido +
                //    "=" + Ope_Com_Pres_Partida.Campo_Monto_Comprometido + " - " + P_Monto_Total +
                //    " WHERE " + Ope_Com_Pres_Partida.Campo_Partida_ID +
                //    "='" + Partida_ID + "'" +
                //    " AND " + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto +
                //    "= TO_CHAR(SYSDATE,'YYYY')";
                //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //ACUTUALIZAMOS LOS PRESUPUESTOS DEL PROYECTO
                //Mi_SQL = "UPDATE " + Ope_Com_Pres_Prog_Proy.Tabla_Ope_Com_Pres_Prog_Proy +
                //    " SET " + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible +
                //    " =" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible + " + " + P_Monto_Total +
                //    "," + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido +
                //    "=" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido + " - " + P_Monto_Total +
                //    " WHERE " + Ope_Com_Pres_Prog_Proy.Campo_Pres_Prog_Proy_ID +
                //    "='" + Proyecto_ID + "'" +
                //    " AND " + Ope_Com_Pres_Prog_Proy.Campo_Anio_Presupuesto +
                //    "= TO_CHAR(SYSDATE,'YYYY')";
                //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }//fin del For


        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Liberar_Productos
        ///DESCRIPCIÓN: Metodo que libera los productos de la requisicion 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 24/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Liberar_Productos(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {

            //seleccionamos todos los productos 
            String Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                ", " + Ope_Com_Req_Producto.Campo_Cantidad +
                " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + "='" + Requisicion_Negocio.P_Requisicion_ID.Trim() + "'";
            DataTable Dt_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            for (int i = 0; i < Dt_Productos.Rows.Count; i++)
            {
                Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                        " SET " + Cat_Com_Productos.Campo_Disponible +
                        " =" + Cat_Com_Productos.Campo_Disponible + " + " + Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Cantidad].ToString() +
                        ", " + Cat_Com_Productos.Campo_Comprometido +
                        " =" + Cat_Com_Productos.Campo_Comprometido + " - " + Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Cantidad].ToString() +
                        " WHERE " + Cat_Com_Productos.Campo_Producto_ID +
                        "='" + Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Prod_Serv_ID].ToString() + "'";
                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Procesos_Compra
        ///DESCRIPCIÓN: Metodo que modifica el monto del proceso de compra dependiendo en que proceso se encuentre la requisicion 
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 26/enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Procesos_Compra(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {             
            String Mi_SQL = "SELECT * " +
                         " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                         " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                         " = '" +Requisicion_Negocio.P_Requisicion_ID +"'";
            DataTable Dt_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            String Tipo_Compra = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Tipo_Compra].ToString();
            double Monto_Requisicion = double.Parse(Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Total].ToString());
            double Monto_Cotizado_Requisicion = double.Parse(Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Total_Cotizado].ToString());
            //Monto que se quedara en el proceso, despues de haber restado el monto de la requisicion
            double Monto_Compra_Directa= 0;
            double Monto_Cotizado =0;
            double Monto_Final = 0;
            String Lista_Requisiciones = "";
            
            switch(Tipo_Compra)
            {
                case "COMPRA DIRECTA":
                   //Cuando es compra directa no se hace nada. 
                    break;
                case "COTIZACION":
                    //Consultamos el Total de la Compra Directa
                    Mi_SQL = " SELECT " + Ope_Com_Cotizaciones.Campo_Total +
                             ", " + Ope_Com_Cotizaciones.Campo_Total_Cotizado +
                             ", " + Ope_Com_Cotizaciones.Campo_Lista_Requisiciones +
                             " FROM " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                             " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                             " ='" + Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_No_Cotizacion].ToString() + "'";
                    DataTable Dt_Cotizacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    Monto_Compra_Directa = double.Parse(Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Total].ToString());
                    //Es el monto de la requisicion que se restara al proceso, ya que se elimino de este proceso por decicion del jefe de dependencia
                    Monto_Final = Monto_Compra_Directa - Monto_Requisicion;
                    Monto_Cotizado = double.Parse(Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Total_Cotizado].ToString());
                    Monto_Cotizado = Monto_Cotizado - Monto_Cotizado_Requisicion;
                    //Modificamos el listado de requisiciones ya que esta se eliminara de ese listado 
                    Lista_Requisiciones = Generar_Nueva_Lista_Requisiciones(Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Lista_Requisiciones].ToString(), Requisicion_Negocio.P_Requisicion_ID.Trim());
                    //Ya obtenido el monto que se quedara actualizamos el monto del proceso de Compra_Directa
                    Mi_SQL = "UPDATE " + Ope_Com_Cotizaciones.Tabla_Ope_Com_Cotizaciones +
                             "SET " + Ope_Com_Cotizaciones.Campo_Total +
                             "='" + Monto_Final.ToString() + "'" +
                             ", " + Ope_Com_Cotizaciones.Campo_Total_Cotizado +
                             "='" + Monto_Cotizado.ToString() + "'" +
                             ", " + Ope_Com_Cotizaciones.Campo_Lista_Requisiciones +
                             "='" + Lista_Requisiciones + "'" +
                             " WHERE " + Ope_Com_Cotizaciones.Campo_No_Cotizacion +
                             "='" + Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_No_Cotizacion].ToString() + "'";
                    break;
                
                case "COMITE DE COMPRAS":
                    //Consultamos el Total de la Compra Directa
                    Mi_SQL = " SELECT " + Ope_Com_Comite_Compras.Campo_Monto_Total +
                             ", " + Ope_Com_Comite_Compras.Campo_Total_Cotizado + 
                             ", " + Ope_Com_Comite_Compras.Campo_Lista_Requisiciones +
                             " FROM " + Ope_Com_Comite_Compras.Tabla_Ope_Com_Comite_Compras +
                             " WHERE " + Ope_Com_Comite_Compras.Campo_No_Comite_Compras +
                             " ='" + Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_No_Comite_Compras].ToString() + "'";
                    DataTable Dt_Comite_Compras = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    Monto_Compra_Directa = double.Parse(Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_Monto_Total].ToString());
                    //Es el monto de la requisicion que se restara al proceso, ya que se elimino de este proceso por decicion del jefe de dependencia
                    Monto_Final = Monto_Compra_Directa - Monto_Requisicion;
                    Monto_Cotizado = double.Parse(Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_Total_Cotizado].ToString());
                    Monto_Cotizado = Monto_Cotizado - Monto_Cotizado_Requisicion;
                    //Modificamos el listado de requisiciones ya que esta se eliminara de ese listado 
                    Lista_Requisiciones = Generar_Nueva_Lista_Requisiciones(Dt_Comite_Compras.Rows[0][Ope_Com_Comite_Compras.Campo_Lista_Requisiciones].ToString(), Requisicion_Negocio.P_Requisicion_ID.Trim());
                    //Ya obtenido el monto que se quedara actualizamos el monto del proceso de Compra_Directa
                    Mi_SQL = "UPDATE " + Ope_Com_Comite_Compras.Tabla_Ope_Com_Comite_Compras +
                             "SET " + Ope_Com_Comite_Compras.Campo_Monto_Total +
                             "='" + Monto_Final.ToString() + "'" +
                             ", " + Ope_Com_Comite_Compras.Campo_Total_Cotizado + 
                             "='" + Monto_Cotizado.ToString() + "'" + 
                             ", " + Ope_Com_Comite_Compras.Campo_Lista_Requisiciones +
                             "='" + Lista_Requisiciones + "'" +
                             " WHERE " + Ope_Com_Comite_Compras.Campo_No_Comite_Compras +
                             "='" + Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_No_Comite_Compras].ToString() + "'";
                    break;
            }//fin del switch

        }//Fin de Modificar_Proceso Compra

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Nueva_Lista_Requisiciones
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar requisisciones
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public String Generar_Nueva_Lista_Requisiciones(String Lista, String Requisicion_Eliminada)
        {
            String Lista_Generada = "";
            String[] Lista_Aux = Lista.Split(',');
            for (int i = 0; i < Lista_Aux.Length; i++)
            {
                if (Lista_Aux[i] != Requisicion_Eliminada)
                {
                    if (i != Lista_Aux.Length - 1)
                        Lista_Generada = Lista_Generada + Lista_Aux[i] + ",";
                    else
                        Lista_Generada = Lista_Generada + Lista_Aux[i];
                }//Fin del if
            }//fin del for
            return Lista_Generada;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Requisiciones
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar requisisciones
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public DataSet Consulta_Requisiciones(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL =
            "SELECT " + Ope_Com_Requisiciones.Campo_Folio +
            ", " + Ope_Com_Requisiciones.Campo_Tipo +
            ", " + Ope_Com_Requisiciones.Campo_Estatus +
            ", " + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
            ", TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Generacion + ",'DD/MON/YYYY') AS FECHA_GENERACION" +
            ", " + Ope_Com_Requisiciones.Campo_Total +
            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
            " WHERE "; 


            if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Req_Especiales) && Requisicion_Negocio.P_Req_Especiales == "SI")
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Especial_Ramo_33 + " = 'SI' ";
            else
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Especial_Ramo_33 + " = 'NO' ";

            if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Dependencia_ID))
            {
                Mi_SQL += " AND " + Ope_Com_Requisiciones.Campo_Dependencia_ID + " = '" + Requisicion_Negocio.P_Dependencia_ID + "'";
            }

            if (Requisicion_Negocio.P_Estatus_Busqueda != null)
            {

                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Estatus + "=" + "'" + Requisicion_Negocio.P_Estatus_Busqueda + "'";
            }
            else
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Estatus + " IN ('GENERADA','COTIZADA')";
            }
            
            if (Requisicion_Negocio.P_Campo_Busqueda != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Folio + " LIKE '%" + Requisicion_Negocio.P_Campo_Busqueda + "%'"; 
            }

            if (Requisicion_Negocio.P_Fecha_Inicial != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Fecha_Generacion + " BETWEEN '" + Requisicion_Negocio.P_Fecha_Inicial + "'" +
                    " AND '" + Requisicion_Negocio.P_Fecha_Final + "'";
            }

            if (Requisicion_Negocio.P_Dependencia_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Dependencia_ID + " ='" + Requisicion_Negocio.P_Dependencia_ID + "'";
            }
            
            //if (Requisicion_Negocio.P_Area_ID != null)
            //{
            //    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Area_ID + " ='" +Requisicion_Negocio.P_Area_ID+ "'";
            //}

 
            Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Listado_Almacen + " IS NULL " +
                    " OR " + Ope_Com_Requisiciones.Campo_Listado_Almacen + "='NO'" +        
                    " ORDER BY " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " ASC";

            if (Requisicion_Negocio.P_Folio != null)
            {               
                Mi_SQL = "SELECT "+
                         " DEPENDENCIA." + Cat_Dependencias.Campo_Nombre +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Tipo +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Folio +
                         ", TO_CHAR( REQUISICION." + Ope_Com_Requisiciones.Campo_Fecha_Generacion + ",'DD/MON/YYYY') AS FECHA_GENERACION" +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Estatus +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Subtotal +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_IEPS +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_IVA +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Total +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Justificacion_Compra +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Especificacion_Prod_Serv +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Verificaion_Entrega +  
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
                         ", REQUISICION." + Ope_Com_Requisiciones.Campo_Total_Cotizado +
                         " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICION " +
                         " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA " +
                         " ON REQUISICION." + Ope_Com_Requisiciones.Campo_Dependencia_ID + "= DEPENDENCIA." +
                         Cat_Dependencias.Campo_Dependencia_ID +
                         " WHERE " + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "'";
                if (Requisicion_Negocio.P_Req_Especiales == "SI")
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Especial_Ramo_33 + " = 'SI' ";
                else
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Especial_Ramo_33 + " = 'NO' ";
            }

            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;            
        }

        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar los productos 
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public DataSet Consulta_Productos_Requisicion(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "(SELECT " +
                   " PRODUCTOS." + Cat_Com_Productos.Campo_Clave +
                   ", PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO" +
                   ", PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion +
                   ", REQUISICION_DET." + Ope_Com_Req_Producto.Campo_Cantidad + " AS CANTIDAD" +
                   ", PRODUCTOS." + Cat_Com_Productos.Campo_Costo + " AS PRECIO_UNITARIO" +
                   ", REQUISICION_DET." + Ope_Com_Req_Producto.Campo_Monto_Total + " AS IMPORTE_S_I" +
                   " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                   " REQUISICION_DET" +
                   " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                   " PRODUCTOS ON PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID +
                   " = REQUISICION_DET." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                   " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICION ON " +
                   "REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "= REQUISICION_DET." +
                   Ope_Com_Req_Producto.Campo_Requisicion_ID +
                   " WHERE REQUISICION." + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "')" +
                   
                   " UNION ALL " +
                   "(SELECT " +
                   " SERVICIOS." + Cat_Com_Servicios.Campo_Clave +
                   ", SERVICIOS." + Cat_Com_Servicios.Campo_Nombre + " AS PRODUCTO" +
                   ", NULL AS DESCRIPCION" +
                   ", REQUISICION_DET." + Ope_Com_Req_Producto.Campo_Cantidad + " AS CANTIDAD" +
                   ", SERVICIOS." + Cat_Com_Servicios.Campo_Costo + " AS PRECIO_UNITARIO" +
                   ", REQUISICION_DET." + Ope_Com_Req_Producto.Campo_Monto_Total + " AS IMPORTE_S_I" +
                   " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                   " REQUISICION_DET" +
                   " JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios +
                   " SERVICIOS ON SERVICIOS." + Cat_Com_Servicios.Campo_Servicio_ID +
                   " = REQUISICION_DET." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                   " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICION ON " +
                   "REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "= REQUISICION_DET." +
                   Ope_Com_Req_Producto.Campo_Requisicion_ID +
                   " WHERE REQUISICION." + Ope_Com_Requisiciones.Campo_Folio + "='" + Requisicion_Negocio.P_Folio + "')" +
                   " ORDER BY PRODUCTO";
                    
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Cotizados
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar los productos que ya fueron consolidadas
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Productos_Cotizados(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "";

            switch (Requisicion_Negocio.P_Tipo_Articulo)
            {
                case "PRODUCTO":
                    Mi_SQL = "SELECT PRODUCTO." + Cat_Com_Productos.Campo_Nombre +
                            ", PRODUCTO." + Cat_Com_Productos.Campo_Clave +
                            ", PRODUCTO." + Cat_Com_Productos.Campo_Descripcion +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET_REQ" +
                            " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTO" +
                            " ON PRODUCTO." + Cat_Com_Productos.Campo_Producto_ID + " =" +
                            " DET_REQ." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            " WHERE DET_REQ." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            "='" + Requisicion_Negocio.P_Requisicion_ID + "'" +
                            " ORDER BY PRODUCTO." + Cat_Com_Productos.Campo_Nombre;
                    break;
                case "SERVICIO":
                    Mi_SQL = "SELECT SERVICIO." + Cat_Com_Servicios.Campo_Nombre +
                            ", SERVICIO." + Cat_Com_Servicios.Campo_Clave +
                            ", NULL AS DESCRIPCION " +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Total_Cotizado +
                            ", DET_REQ." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
                            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DET_REQ" +
                            " JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SERVICIO" +
                            " ON SERVICIO." + Cat_Com_Servicios.Campo_Servicio_ID + " =" +
                            " DET_REQ." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            " WHERE DET_REQ." + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            "='" + Requisicion_Negocio.P_Requisicion_ID + "'" +
                            " ORDER BY SERVICIO." + Cat_Com_Servicios.Campo_Nombre; 
                       
                    break;

            }
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultamos_Presupuesto_Existente
        ///DESCRIPCIÓN: Metodo que consulta si existe presupuesto para los productos seleccionados
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public bool Consultamos_Presupuesto_Existente(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {
            bool Existe_Presupuesto = false;
            String Mi_SQL = "";
            //Primero Obtenemos todos lo productos pertenecientes a la requisicion:
                    Mi_SQL = "SELECT REQ_DET." + Ope_Com_Req_Producto.Campo_Partida_ID +
                    ",REQ_DET." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID +
                    ", (SELECT " + Ope_Com_Requisiciones.Campo_Total +
                    " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                    "= REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") AS TOTAL" +
                     ", (SELECT " + Ope_Com_Requisiciones.Campo_Total_Cotizado +
                     " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                     " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                     "= REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") AS TOTAL_COTIZADO" +
                     ", (SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                     " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                     " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                     "= REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") AS DEPENDENCIA_ID" +
                     ", REQ_DET." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID +
                     " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_DET" +
                     " WHERE REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + "='" + Requisicion_Negocio.P_Requisicion_ID + "'";
            DataTable Dt_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            //Guardamos en la variable Monto_Restante la diferencia de Total_Cotizado y P_Monto_Total inicial del producto
            //Primero checamos si se va a restar o a aumentar el presupuesto
            double Monto_Cotizado = double.Parse(Dt_Requisicion.Rows[0]["TOTAL_COTIZADO"].ToString().Trim());
            double Monto_Anterior = double.Parse(Dt_Requisicion.Rows[0]["TOTAL"].ToString().Trim());
            double Diferencia = 0;
            double Disponible = 0;
            String Partida_ID = Dt_Requisicion.Rows[0][Ope_Com_Req_Producto.Campo_Partida_ID].ToString().Trim();
            String Proyecto_ID = Dt_Requisicion.Rows[0][Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID].ToString().Trim();
            String Dependencia_ID = Dt_Requisicion.Rows[0]["DEPENDENCIA_ID"].ToString().Trim();
            String FF = Dt_Requisicion.Rows[0][Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID].ToString().Trim();
           
            // PASO 1 VERIFICAMOS CUAL DE LOS 2 MONTOS ES MAYOR SI EL COTIZADO O  EL ANTERIOR
            if (Monto_Cotizado > Monto_Anterior)
            {
                //Obtenemos la resta
                Diferencia = Monto_Cotizado - Monto_Anterior;
                Mi_SQL = "";
                //Consultamos el Monto presupuestal para ver si es suficiente 
                Mi_SQL = "SELECT " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                Mi_SQL = Mi_SQL + "='" + Partida_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                Mi_SQL = Mi_SQL + "='" + Proyecto_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID;
                Mi_SQL = Mi_SQL + "='" + FF + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio;
                Mi_SQL = Mi_SQL + " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                Mi_SQL = Mi_SQL + "='" + Partida_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                Mi_SQL = Mi_SQL + "='" + Proyecto_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + "='" + Dependencia_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                Mi_SQL = Mi_SQL + "= TO_CHAR(SYSDATE,'YYYY'))";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                Mi_SQL = Mi_SQL + "= TO_CHAR(SYSDATE,'YYYY')";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + "='" + Dependencia_ID + "'";

                //Sentencia que ejecuta el query
                DataTable Dt_Presupuestos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                Disponible = double.Parse(Dt_Presupuestos.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString().Trim());
                if (Disponible >= Diferencia)
                    Existe_Presupuesto = true;
                else
                    Existe_Presupuesto = false;
            }
            else
                Existe_Presupuesto = true; //Existe presupuesto ya que el monto cotizado es menor que el k se aparto 

           
            return Existe_Presupuesto;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Presupuesto_Partidas
        ///DESCRIPCIÓN: Metodo que consulta si existe presupuesto 
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Modificar_Presupuesto_Partidas(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "";
            //consultamos los detalles pertenecientes a esta requisicion
            Mi_SQL = "SELECT REQ_DET." + Ope_Com_Req_Producto.Campo_Partida_ID +
                    ",REQ_DET." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID +
                    ", (SELECT " + Ope_Com_Requisiciones.Campo_Total +
                    " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                    "= REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") AS TOTAL" +
                     ", (SELECT " + Ope_Com_Requisiciones.Campo_Total_Cotizado +
                     " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                     " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                     "= REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") AS TOTAL_COTIZADO" +
                     ", (SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                     " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                     " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                     "= REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ") AS DEPENDENCIA_ID" +
                     ", REQ_DET." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID +
                     " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_DET" +
                     " WHERE REQ_DET." + Ope_Com_Req_Producto.Campo_Requisicion_ID + "='" + Requisicion_Negocio.P_Requisicion_ID + "'";
            DataTable Dt_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            //Guardamos en la variable Monto_Restante la diferencia de Total_Cotizado y P_Monto_Total inicial del producto
            //Primero checamos si se va a restar o a aumentar el presupuesto
            double Monto_Cotizado = double.Parse(Dt_Requisicion.Rows[0]["TOTAL_COTIZADO"].ToString());
            double Monto_Anterior = double.Parse(Dt_Requisicion.Rows[0]["TOTAL"].ToString());
            double Diferencia = 0;
            String Partida_ID = Dt_Requisicion.Rows[0][Ope_Com_Req_Producto.Campo_Partida_ID].ToString().Trim();
            String Proyecto_ID = Dt_Requisicion.Rows[0][Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID].ToString().Trim();
            String Dependencia_ID = Dt_Requisicion.Rows[0]["DEPENDENCIA_ID"].ToString().Trim();
            String FF = Dt_Requisicion.Rows[0][Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID].ToString().Trim();
            bool Suma_Diferencia = false;

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
            //modificamos el monto dependiendo de si se resta la dir¿ferencia o se suma
            if (Suma_Diferencia == true)
            {
                        //se ACTUALIZAN LOS PRESUPUESTOS DE LA DEPENDENCIA, CORRESPONDIENTE AL PROYECTO Y PARTIDA

                        Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                        Mi_SQL = Mi_SQL + " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible;
                        Mi_SQL = Mi_SQL + " =" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " - " + Diferencia.ToString();
                        Mi_SQL = Mi_SQL + "," + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido;
                        Mi_SQL = Mi_SQL + "=" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " + " + Diferencia.ToString();
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                        Mi_SQL = Mi_SQL + "='" + Partida_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                        Mi_SQL = Mi_SQL + "='" + Proyecto_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID;
                        Mi_SQL = Mi_SQL + "='" + FF + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                        Mi_SQL = Mi_SQL + "= TO_CHAR(SYSDATE,'YYYY')";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + "='" + Dependencia_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio;
                        Mi_SQL = Mi_SQL + " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                        Mi_SQL = Mi_SQL + "='" + Partida_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                        Mi_SQL = Mi_SQL + "='" + Proyecto_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + "='" + Dependencia_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                        Mi_SQL = Mi_SQL + "= TO_CHAR(SYSDATE,'YYYY'))";
                       
                        //Sentencia que ejecuta el query
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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

                }
             
        }

      

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisicion_Consolidada
        ///DESCRIPCIÓN: Metodo que permite consultar si la requisicion esta consolidad y regresa un booleano
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 25/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public bool Consultar_Requisicion_Consolidada(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = " SELECT " + Ope_Com_Requisiciones.Campo_No_Consolidacion +
                            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                            " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                            " ='" +Requisicion_Negocio.P_Requisicion_ID + "'";
            DataTable Dt_Consolidacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            bool Consolidada = false;
            if (Dt_Consolidacion.Rows.Count != 0)
            {
                if (Dt_Consolidacion.Rows[0][0] != null)
                    Consolidada = true;
                else
                    Consolidada = false;
            }
            return Consolidada;
        }

        #region Observaciones

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consecutivo
        ///DESCRIPCIÓN: Metodo que verfifica el consecutivo en la tabla y ayuda a generar el nuevo Id. 
        ///PARAMETROS: 
        ///CREO:
        ///FECHA_CREO: 24/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Consecutivo()
        {
            String Consecutivo = "";
            String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
            Object Asunto_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

            Mi_SQL = "SELECT NVL(MAX (" + Ope_Com_Req_Observaciones.Campo_Observacion_ID + "),'0') ";
            Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones;
            Asunto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Asunto_ID))
            {
                Consecutivo = "1";
            }
            else
            {
                Consecutivo = string.Format("{0:0}", Convert.ToInt32(Asunto_ID) + 1);
            }
            return Consecutivo;
        }//fin de consecutivo

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar Observaciones 
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Observaciones(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "SELECT " + Ope_Com_Req_Observaciones.Campo_Observacion_ID +
                            ", " + Ope_Com_Req_Observaciones.Campo_Comentario +
                            ", " + Ope_Com_Req_Observaciones.Campo_Estatus +
                            "," + Ope_Com_Req_Observaciones.Campo_Fecha_Creo + 
                            ", " + Ope_Com_Req_Observaciones.Campo_Usuario_Creo +
                            " FROM " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones +
                            " WHERE " + Ope_Com_Req_Observaciones.Campo_Requisicion_ID + " = '" + Requisicion_Negocio.P_Requisicion_ID + "'" +
                            " ORDER BY " + Ope_Com_Req_Observaciones.Campo_Observacion_ID + " ASC";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Observaciones
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para dar de alta observaciones
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Alta_Observaciones(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {
            //String ID = Consecutivo();
            String Mi_SQL = "INSERT INTO " + Ope_Com_Req_Observaciones.Tabla_Ope_Com_Req_Observaciones +
            " (" +
            " " + Ope_Com_Req_Observaciones.Campo_Requisicion_ID +
            ", " + Ope_Com_Req_Observaciones.Campo_Comentario +
            ", " + Ope_Com_Req_Observaciones.Campo_Estatus +
            ", " + Ope_Com_Req_Observaciones.Campo_Usuario_Creo +
            ", " + Ope_Com_Req_Observaciones.Campo_Fecha_Creo +
            ") VALUES (" +
            //"SECUENCIA_OBSERVACION_REQ_ID.NEXTVAL,'" +
             "'" +
            Requisicion_Negocio.P_Requisicion_ID + "','" +
            Requisicion_Negocio.P_Comentario + "','" +
            Requisicion_Negocio.P_Estatus + "','" +
            Requisicion_Negocio.P_Usuario + "',SYSTIMESTAMP)";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);            
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Areas
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar las Areas
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Areas(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "SELECT "+ Cat_Areas.Campo_Area_ID +
                            ", "+Cat_Areas.Campo_Nombre + 
                            " FROM " + Cat_Areas.Tabla_Cat_Areas +
                            " WHERE " + Cat_Areas.Campo_Dependencia_ID + " ='"+
                            Cls_Sessiones.Dependencia_ID_Empleado + "'" +    
                            " ORDER BY " + Cat_Areas.Campo_Nombre;
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencias
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar las Areas
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Dependencias(Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID +
                            ", " + Cat_Dependencias.Campo_Clave + "||' '||" + Cat_Dependencias.Campo_Nombre +
                            " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                            " ORDER BY " + Cat_Dependencias.Campo_Nombre;
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencias
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para consultar las Areas
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Gustavo AC
        ///FECHA_CREO: 13/Marzo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Dependencias_Con_Programas_Especiales_yRamo33
            (Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = 
            "SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID +
            ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "||' '||" + 
            Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS CLAVE_NOMBRE " +
            " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " JOIN " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
            " ON " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " = " +
            Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
            " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " IN (" +
                " SELECT " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento +
                " WHERE " + Cat_SAP_Fuente_Financiamiento.Campo_Especiales_Ramo_33 + " = 'SI')" +
            " AND " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + " = " +
            DateTime.Now.Year +
            " GROUP BY (" +
            Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "," +
            Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "," +
            Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") " +
            " ORDER BY " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave;
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
        #endregion
        
        #endregion
    }
}