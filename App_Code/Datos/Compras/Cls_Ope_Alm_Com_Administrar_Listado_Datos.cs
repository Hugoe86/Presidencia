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
using Presidencia.Administrar_Listado.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;



/// <summary>
/// Summary description for Cls_Ope_Alm_Com_Administrar_Listado_Datos
/// </summary>
/// 
namespace Presidencia.Administrar_Listado.Datos
{
    public class Cls_Ope_Alm_Com_Administrar_Listado_Datos
    {
        #region Metodos
        public Cls_Ope_Alm_Com_Administrar_Listado_Datos()
        {
        }

        public DataTable Consulta_Listado_Almacen(Cls_Ope_Alm_Com_Administrar_Listado_Negocio Listado_Negocio)
        {
            String Mi_SQL = "SELECT LISTADO." + Ope_Com_Listado.Campo_Folio +
                            ",  TO_CHAR(LISTADO." + Ope_Com_Listado.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA_CREO" +
                            ", LISTADO." + Ope_Com_Listado.Campo_Tipo +
                            ", LISTADO." + Ope_Com_Listado.Campo_Estatus +
                            ", LISTADO." + Ope_Com_Listado.Campo_Total +
                            " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado + " LISTADO";                            
         
            if (Listado_Negocio.P_Estatus_Busqueda == null)

            {
                Mi_SQL = Mi_SQL + " WHERE LISTADO." + Ope_Com_Listado.Campo_Estatus +
                            " IN('GENERADA')";
            }
            else
            {
                Mi_SQL = Mi_SQL + " WHERE LISTADO." + Ope_Com_Listado.Campo_Estatus +
                            "='" + Listado_Negocio.P_Estatus_Busqueda + "'";
            }

            if (Listado_Negocio.P_Folio != null)
            {
                Mi_SQL = "SELECT LISTADO." + Ope_Com_Listado.Campo_Folio +
                         ", (SELECT " + Cat_Com_Proyectos_Programas.Campo_Nombre + 
                         " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + 
                         " WHERE " + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " = " +
                         " LISTADO." + Ope_Com_Listado.Campo_No_Proyecto_ID + " )AS PROYECTO" +
                         ", ( SELECT "+ Cat_Com_Partidas.Campo_Nombre +
                         " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                         " WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = " +
                         "LISTADO." + Ope_Com_Listado.Campo_No_Partida_ID + ") AS PARTIDA " + 
                         ",  TO_CHAR(LISTADO." + Ope_Com_Listado.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA_CREO" +
                         ", LISTADO." + Ope_Com_Listado.Campo_Estatus +
                         ", LISTADO." + Ope_Com_Listado.Campo_Tipo +
                         ", LISTADO." + Ope_Com_Listado.Campo_Total +
                         ", LISTADO." + Ope_Com_Listado.Campo_Listado_ID +
                         " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado + " LISTADO "+
                         " WHERE " + Ope_Com_Listado.Campo_Folio + " like '%" + Listado_Negocio.P_Folio + "%'";
            }
            if (Listado_Negocio.P_Fecha_Inicial != null)
            {
                Mi_SQL = Mi_SQL + " AND LISTADO." + Ope_Com_Listado.Campo_Fecha_Creo + " BETWEEN '" + Listado_Negocio.P_Fecha_Inicial + "'" +
                    " AND '" + Listado_Negocio.P_Fecha_Final + "'";
            }
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }


        public DataTable Consulta_Listado_Detalle(Cls_Ope_Alm_Com_Administrar_Listado_Negocio Listado_Negocio)
        {

            String Mi_SQL = "SELECT LISTADO." + Ope_Com_Listado_Detalle.Campo_No_Producto_ID + " AS PRODUCTO_ID" +
                         ", PRODUCTO." + Cat_Com_Productos.Campo_Clave +
                         ", PRODUCTO." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO_NOMBRE" +
                         ", PRODUCTO." + Cat_Com_Productos.Campo_Descripcion + 
                         ", PRODUCTO." + Cat_Com_Productos.Campo_Existencia +
                         ", PRODUCTO." + Cat_Com_Productos.Campo_Reorden +
                         ", (SELECT " + Cat_Com_Unidades.Campo_Nombre +
                         " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + 
                         " WHERE " + Cat_Com_Unidades.Campo_Unidad_ID +
                         "=PRODUCTO." + Cat_Com_Productos.Campo_Unidad_ID + ") AS UNIDAD" + 
                         ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Cantidad +
                         ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Costo_Compra + " AS PRECIO_UNITARIO" +
                         ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Importe +
                         ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Monto_IVA +
                         ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Monto_IEPS +
                         ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Porcentaje_IVA +
                         ", LISTADO." + Ope_Com_Listado_Detalle.Campo_Porcentaje_IEPS +
                         " FROM " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle + " LISTADO" +
                         " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTO" +
                         " ON PRODUCTO." + Cat_Com_Productos.Campo_Producto_ID + " = LISTADO." + Ope_Com_Listado_Detalle.Campo_No_Producto_ID +
                         " WHERE LISTADO." + Ope_Com_Listado_Detalle.Campo_No_Listado_ID + " = '" + Listado_Negocio.P_Listado_ID + "'" +
                         " ORDER BY PRODUCTO." + Cat_Com_Productos.Campo_Nombre;

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }


        public DataTable Consultar_Observaciones_Listado(Cls_Ope_Alm_Com_Administrar_Listado_Negocio Listado_Negocio)
        {
            String Mi_SQL = "SELECT " + Ope_Alm_Com_Obs_Listado.Campo_Comentario +
                            ", " + Ope_Alm_Com_Obs_Listado.Campo_Estatus +
                            ", TO_CHAR(" + Ope_Alm_Com_Obs_Listado.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA_CREO" +
                            ", " + Ope_Alm_Com_Obs_Listado.Campo_Usuario_Creo +
                            " FROM " + Ope_Alm_Com_Obs_Listado.Tabla_Ope_Alm_Com_Obs_Listados +
                            " WHERE " + Ope_Alm_Com_Obs_Listado.Campo_No_Listado_ID +
                            " = '" + Listado_Negocio.P_Listado_ID + "'";

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }


        public String Modificar_Listado(Cls_Ope_Alm_Com_Administrar_Listado_Negocio Listado_Negocio)
        {
            
            String Mi_SQL = "";
            //PASO 1
            //Obtenemos los datos anteriores del listado para en caso de ser necesario restar el presupuesto y sumar el nuevo
            Mi_SQL = "SELECT " + Ope_Com_Listado.Campo_Listado_ID +
                     ", " + Ope_Com_Listado.Campo_Folio +
                     ", " + Ope_Com_Listado.Campo_No_Proyecto_ID +
                     ", " + Ope_Com_Listado.Campo_No_Partida_ID +
                     ", " + Ope_Com_Listado.Campo_Tipo +
                     ", " + Ope_Com_Listado.Campo_Estatus +
                     ", " + Ope_Com_Listado.Campo_Total +
                     " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado +
                     " WHERE " + Ope_Com_Listado.Campo_Folio + " ='" + Listado_Negocio.P_Folio + "'";

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            //Guardamos el id del listado 
            Listado_Negocio.P_Listado_ID = Data_Table.Rows[0][Ope_Com_Listado.Campo_Listado_ID].ToString();
            Listado_Negocio.P_Partida_ID = Data_Table.Rows[0][Ope_Com_Listado.Campo_No_Partida_ID].ToString();
            //Guardamos los valores de total para restarcelo al presupuesto 
            double Presupuesto_Anterior = double.Parse(Data_Table.Rows[0][Ope_Com_Listado.Campo_Total].ToString());
            
            //PASO 2 
            //Creamos la consulta oracle para modificar el listado 
            //Solo se puede modificara el P_Estatus 
            
            //Modificamos el P_Estatus
            
              
            //De acuerdo al estatus se modifican los campos de empelado_Id y fecha 
            switch (Listado_Negocio.P_Estatus)
            {
                case "AUTORIZADA":
                    Mi_SQL = "UPDATE " + Ope_Com_Listado.Tabla_Ope_Com_Listado +
                            " SET " + Ope_Com_Listado.Campo_Estatus +
                            " = '" + Listado_Negocio.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Listado.Campo_Empleado_Autorizacion_ID +
                             " = '" + Listado_Negocio.P_Usuario_ID + "'" +
                             ", " + Ope_Com_Listado.Campo_Fecha_Autorizacion +
                             " = SYSDATE ";
                    break;
                case "RECHAZADA":
                    Mi_SQL = "UPDATE " + Ope_Com_Listado.Tabla_Ope_Com_Listado +
                             " SET " + Ope_Com_Listado.Campo_Estatus +
                             " = 'EN CONSTRUCCION'";
                    
                    break;
                case "CANCELADA":
                    Mi_SQL = "UPDATE " + Ope_Com_Listado.Tabla_Ope_Com_Listado +
                             " SET " + Ope_Com_Listado.Campo_Estatus +
                             " = 'CANCELADA'";
                    
                    break;
            }
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Listado.Campo_Listado_ID +
                     " = '"+ Listado_Negocio.P_Listado_ID+ "'";

            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            //PASO 3 DAMOS DE ALTA LOS COMENTARIOS 
            Alta_Observaciones_Listado(Listado_Negocio);
            //PASO 4 EN CASO DE SER AUTORIZADO EL LISTADO SE CONVIERTE A REQUISICION TRANSITORIA
            String No_Requisicion_Trancitoria = "";
           
            if (Listado_Negocio.P_Estatus == "CANCELADA")
            {
                Liberar_Presupuesto_Cancelada(Listado_Negocio);
            }
            String Algo="";

            return Algo;
        }//fin de Modificar_Listado

        public void Alta_Observaciones_Listado(Cls_Ope_Alm_Com_Administrar_Listado_Negocio Listado_Negocio)
        {
            String Id_Obs_Listado = Obtener_Consecutivo(Ope_Alm_Com_Obs_Listado.Campo_Obs_listado_ID, Ope_Alm_Com_Obs_Listado.Tabla_Ope_Alm_Com_Obs_Listados).ToString();
            String Mi_SQL = "INSERT INTO " + Ope_Alm_Com_Obs_Listado.Tabla_Ope_Alm_Com_Obs_Listados +
                            " (" + Ope_Alm_Com_Obs_Listado.Campo_Obs_listado_ID +
                            ", " + Ope_Alm_Com_Obs_Listado.Campo_No_Listado_ID +
                            ", " + Ope_Alm_Com_Obs_Listado.Campo_Comentario +
                            ", " + Ope_Alm_Com_Obs_Listado.Campo_Estatus +
                            ", " + Ope_Alm_Com_Obs_Listado.Campo_Usuario_Creo +
                            ", " + Ope_Alm_Com_Obs_Listado.Campo_Fecha_Creo +
                            ") VALUES ('"+ Id_Obs_Listado+"','" +
                            Listado_Negocio.P_Listado_ID + "','" +
                            Listado_Negocio.P_Comentario + "','" +
                            Listado_Negocio.P_Estatus + "','" +
                            Listado_Negocio.P_Usuario + "',SYSDATE)";
            //Sentencia que ejecuta el query
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        }//fin de Alta_Observacion

        public String Convertir_Requisicion_Transitoria(Cls_Ope_Alm_Com_Administrar_Listado_Negocio Listado_Negocio)
        {
            
            //Generamos el id de la requisiciion 
            String Id_Requisicion = Obtener_Consecutivo(Ope_Com_Requisiciones.Campo_Requisicion_ID, Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones).ToString();
            //Consultamos el id de la dependencia de Almacen que se encuentra en los parametros 
            String Mi_SQL = "SELECT " + Cat_Com_Parametros.Campo_Dependencia_ID_Almacen +
                ", " + Cat_Com_Parametros.Campo_Partida_Esp_Almacen_Global +
                ", " + Cat_Com_Parametros.Campo_Programa_Almacen + 
                " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros;
            DataTable Dt_Parametros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            String Partida_Esp_Almacen_Global = Dt_Parametros.Rows[0][Cat_Com_Parametros.Campo_Partida_Esp_Almacen_Global].ToString();
            String Programa_ID_Almacen = Dt_Parametros.Rows[0][Cat_Com_Parametros.Campo_Programa_Almacen].ToString();
            String Dependencia_ID_Almacen = Dt_Parametros.Rows[0][Cat_Com_Parametros.Campo_Dependencia_ID_Almacen].ToString();
            //CONSULTAMOS LA FUENTE DE FINANCIAMIENTO 
            Mi_SQL = "SELECT " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
                " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                " ='" + Partida_Esp_Almacen_Global + "'" +
                " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                "='" + Programa_ID_Almacen + "'" +
                " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                "='" + Dependencia_ID_Almacen + "'" +
                " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio +
                " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")" +
                " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                "='" + Partida_Esp_Almacen_Global + "'" +
                " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                "='" + Programa_ID_Almacen + "'" +
                " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                "='" + Dependencia_ID_Almacen + "')";
            DataTable Dt_F_Financiamiento = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            String Fuente_Financiamiento = Dt_F_Financiamiento.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID].ToString();
            Mi_SQL = "INSERT INTO " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " (" + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                ", " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                ", " + Ope_Com_Requisiciones.Campo_Area_ID +
                ", " + Ope_Com_Requisiciones.Campo_Folio +
                ", " + Ope_Com_Requisiciones.Campo_Estatus +
                ", " + Ope_Com_Requisiciones.Campo_Codigo_Programatico +
                ", " + Ope_Com_Requisiciones.Campo_Tipo +
                ", " + Ope_Com_Requisiciones.Campo_Fase +
                ", " + Ope_Com_Requisiciones.Campo_Total +
                ", " + Ope_Com_Requisiciones.Campo_Usuario_Creo +
                ", " + Ope_Com_Requisiciones.Campo_Fecha_Creo +
                ", " + Ope_Com_Requisiciones.Campo_Empleado_Filtrado_ID +
                ", " + Ope_Com_Requisiciones.Campo_Fecha_Filtrado +
                ", " + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
                ", " + Ope_Com_Requisiciones.Campo_Empleado_Construccion_ID +
                ", " + Ope_Com_Requisiciones.Campo_Fecha_Construccion +
                ", " + Ope_Com_Requisiciones.Campo_Empleado_Generacion_ID +
                ", " + Ope_Com_Requisiciones.Campo_Fecha_Generacion +
                ", " + Ope_Com_Requisiciones.Campo_Empleado_Autorizacion_ID +
                ", " + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion +
                ", " + Ope_Com_Requisiciones.Campo_Listado_Almacen +
                ") VALUES ('" + Id_Requisicion + "','" +
                Dependencia_ID_Almacen + "','" + 
                Cls_Sessiones.Area_ID_Empleado + "','" +
                "RQ-" + Id_Requisicion + "','" + 
                "FILTRADA','" +
                "CODIGO PROGRAMATICO"+ "','" +
                "TRANSITORIA','" +
                "REQUISICION','" +
                Listado_Negocio.P_Total + "','" +
                Cls_Sessiones.Nombre_Empleado + "',SYSDATE," +
                "'" + Cls_Sessiones.Empleado_ID + "',SYSDATE,'PRODUCTO'," +
                "'" + Cls_Sessiones.Empleado_ID + "',SYSDATE," + 
                "'" + Cls_Sessiones.Empleado_ID + "',SYSDATE," + 
                "'" + Cls_Sessiones.Empleado_ID + "',SYSDATE,'SI')";

            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //Ahora asignamos el id de la requisicion al listado de almacen, esto para realizar la relacion en caso de ser necesario
            Mi_SQL = "UPDATE " + Ope_Com_Listado.Tabla_Ope_Com_Listado +
                    " SET " + Ope_Com_Listado.Campo_No_Requisicion_ID +
                    " = '" + Id_Requisicion + "'" +
                    " WHERE " + Ope_Com_Listado.Campo_Folio + "='" +
                    Listado_Negocio.P_Folio + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //Ahora recorremos el data de los productos del listado y los pasamos a la requisicion
            Mi_SQL = "SELECT * FROM " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle +
                     " WHERE " + Ope_Com_Listado_Detalle.Campo_No_Listado_ID + "='" + Listado_Negocio.P_Listado_ID +"'";
            DataTable Data_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
           
            //Calculamos el IEPS, IVA y Subtotal de la requisicion de acuerdo a los productos que le pertenecen a esta
            //Variable que almacena la suma de todos los valores del IVA que tiene cada producto del detalle
            double IVA_Acumulado = 0;
            //Variable que almacena la suma de todos los valores del IEPS que tiene cada producto del detalle
            double IEPS_Acumulado = 0;
            //Variable que almacena la suma del costo compra sin tomar en cuenta el aumento por impuestos
            double Subtotal = 0;
            if(Data_Productos.Rows.Count != 0)
            {
                for (int i = 0; i < Data_Productos.Rows.Count; i++)
                {
                    //Consultamos el numbre y giro correspondiente al producto
                    //Mi_SQL = "SELECT PRO." + Cat_Com_Productos.Campo_Giro_ID +
                    //        ",(SELECT " + Cat_Sap_Concepto.Campo_Descripcion +
                    //        " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + 
                    //        " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID + "=PRO." + Cat_Com_Productos.Campo_Giro_ID + ")" +
                    //        ", PRO." + Cat_Com_Productos.Campo_Nombre +
                    //        " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRO" +
                    //        " WHERE PRO." + Cat_Com_Productos.Campo_Producto_ID +
                    //        " ='" + Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_No_Producto_ID].ToString() + "'";
                    //DataTable Data_Concepto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    Mi_SQL = "INSERT INTO " +
                        Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                        " (" + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Partida_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Cantidad +
                        ", " + Ope_Com_Req_Producto.Campo_Usuario_Creo +
                        ", " + Ope_Com_Req_Producto.Campo_Fecha_Creo +
                        ", " + Ope_Com_Req_Producto.Campo_Importe +
                        ", " + Ope_Com_Req_Producto.Campo_Monto_IVA +
                        ", " + Ope_Com_Req_Producto.Campo_Monto_IEPS +
                        ", " + Ope_Com_Req_Producto.Campo_Porcentaje_IVA +
                        ", " + Ope_Com_Req_Producto.Campo_Porcentaje_IEPS +
                        ", " + Ope_Com_Req_Producto.Campo_Monto_Total +
                        ", " + Ope_Com_Req_Producto.Campo_Tipo +
                        ", " + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID +
                        " ) VALUES " +
                        "('" + Obtener_Consecutivo(Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID, Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto) +
                        "','" + Id_Requisicion +
                        "','" + Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_No_Producto_ID].ToString() +
                        "','" + Partida_Esp_Almacen_Global +
                        "','" + Programa_ID_Almacen +
                        "', '" + Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Cantidad].ToString() +
                        "','" + Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Usuario_Creo].ToString() +
                        "',SYSDATE" +
                        ",'" + Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Costo_Compra].ToString() +
                        "','" + Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Monto_IVA].ToString() +
                        "','" + Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Monto_IEPS].ToString() +
                        "','" + Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Porcentaje_IVA].ToString() +
                        "','" + Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Porcentaje_IEPS].ToString() +
                        "','" + Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Importe].ToString() + 
                        "','PRODUCTO','"+ Fuente_Financiamiento.Trim() +"')";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Calculamos el IVA_Acumulado
                    String Monto_IVA = Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Monto_IVA].ToString();
                    String Monto_IEPS = Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Monto_IEPS].ToString();
                    //Validamos para cuando sean nulos los valores de Monto_IEPS y MONTO_IVA
                    if(Monto_IVA.Trim()!=String.Empty)
                        IVA_Acumulado = IVA_Acumulado + double.Parse(Monto_IVA);
                    if(Monto_IEPS.Trim()!=String.Empty)
                        IEPS_Acumulado = IEPS_Acumulado + double.Parse(Monto_IEPS);
                    Subtotal = Subtotal + double.Parse(Data_Productos.Rows[i][Ope_Com_Listado_Detalle.Campo_Costo_Compra].ToString());
                }
                //Actualizamos la requisicion con los nuevos valores de IVA, IEPs y subtotal 
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                        " SET " + Ope_Com_Requisiciones.Campo_IVA + " ='" + IVA_Acumulado.ToString() + 
                        "', " + Ope_Com_Requisiciones.Campo_IEPS + "='" + IEPS_Acumulado.ToString() +
                        "', " + Ope_Com_Requisiciones.Campo_Subtotal + "='" +  Subtotal.ToString() +
                        "' WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + Id_Requisicion + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);


            }//fin del if


            return Id_Requisicion;
        }//fin de Convertir_Requisicion_Transitoria



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
        public void Liberar_Presupuesto_Cancelada(Cls_Ope_Alm_Com_Administrar_Listado_Negocio Listado_Negocio)
        {
            //Obtenemos los datos anteriores del listado para en caso de ser necesario restar el presupuesto y sumar el nuevo
            String Mi_SQL = "SELECT " + Ope_Com_Listado.Campo_Listado_ID +
                     ", " + Ope_Com_Listado.Campo_Folio +
                     ", " + Ope_Com_Listado.Campo_No_Proyecto_ID +
                     ", " + Ope_Com_Listado.Campo_No_Partida_ID +
                     ", " + Ope_Com_Listado.Campo_Tipo +
                     ", " + Ope_Com_Listado.Campo_Estatus +
                     ", " + Ope_Com_Listado.Campo_Total +
                     " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado +
                     " WHERE " + Ope_Com_Listado.Campo_Listado_ID + " ='" + Listado_Negocio.P_Listado_ID + "'";

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            //Como ya se obtuvieron los detalles d la licitacion antes de ser modificada
            //Usamos los datos del monto y la partida para liberar presupuestos correspondientes
            double Monto = double.Parse(Data_Table.Rows[0][Ope_Com_Listado.Campo_Total].ToString());
            String Partida = Data_Table.Rows[0][Ope_Com_Listado.Campo_No_Partida_ID].ToString();
            String Programa = Data_Table.Rows[0][Ope_Com_Listado.Campo_No_Proyecto_ID].ToString();
            
            //Modificamos el estatus del listado 
            Mi_SQL = "UPDATE " + Ope_Com_Listado.Tabla_Ope_Com_Listado +
                            " SET " + Ope_Com_Listado.Campo_Estatus +
                            "='" + Listado_Negocio.P_Estatus + "'" +
                            ", " + Ope_Com_Listado.Campo_Empleado_Cancelacion_ID +
                            " = '" + Listado_Negocio.P_Usuario_ID + "'" +
                            ", " + Ope_Com_Listado.Campo_Fecha_Cancelacion +
                            " = SYSDATE" +
                            " WHERE " + Ope_Com_Listado.Campo_Listado_ID +
                            "='" + Listado_Negocio.P_Listado_ID + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //liberamos el presupuesto que era para este listado
            //se ACTUALIZAN LOS PRESUPUESTOS DE LA DEPENDENCIA, CORRESPONDIENTE AL PROYECTO Y PARTIDA

            Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                " =" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " + " + Monto +
                "," + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                "=" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " - " + Monto +
                " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                "='" + Partida + "'" +
                " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                "='" + Programa + "'" +
                " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio +
                " = (SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")" +
                " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                "='" + Partida + "'" +
                " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                "='" + Programa + "'" +
                " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                "= TO_CHAR(SYSDATE,'YYYY'))" +
                " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                "= TO_CHAR(SYSDATE,'YYYY')";
            //Sentencia que ejecuta el query
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            //ACTUALIZAMOS LOS PRESUPUESTOS DE LA PARTIDA
            //Mi_SQL = "UPDATE " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida +
            //    " SET " + Ope_Com_Pres_Partida.Campo_Monto_Disponible +
            //    " =" + Ope_Com_Pres_Partida.Campo_Monto_Disponible + " + " + Monto +
            //    "," + Ope_Com_Pres_Partida.Campo_Monto_Comprometido +
            //    "=" + Ope_Com_Pres_Partida.Campo_Monto_Comprometido + " - " + Monto +
            //    " WHERE " + Ope_Com_Pres_Partida.Campo_Partida_ID +
            //    "='" + Partida + "'" +
            //    " AND " + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto +
            //    "= TO_CHAR(SYSDATE,'YYYY')";
            //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            ////ACUTUALIZAMOS LOS PRESUPUESTOS DEL PROYECTO
            //Mi_SQL = "UPDATE " + Ope_Com_Pres_Prog_Proy.Tabla_Ope_Com_Pres_Prog_Proy +
            //    " SET " + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible +
            //    " =" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible + " + " + Monto +
            //    "," + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido +
            //    "=" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido + " - " + Monto +
            //    " WHERE " + Ope_Com_Pres_Prog_Proy.Campo_Pres_Prog_Proy_ID +
            //    "='" + Programa + "'" +
            //    " AND " + Ope_Com_Pres_Prog_Proy.Campo_Anio_Presupuesto +
            //    "= TO_CHAR(SYSDATE,'YYYY')";
            //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);


        }

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
        public static String Consecutivo(String Campo_ID, String Tabla)
        {
            String Consecutivo = "";
            String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos

            Mi_SQL = "SELECT NVL(MAX (" + Campo_ID + "),'0000000000') ";
            Mi_SQL = Mi_SQL + "FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Obj))
            {
                Consecutivo = "0000000001";
            }
            else
            {
                Consecutivo = string.Format("{0:0000000000}", Convert.ToInt32(Obj) + 1);
            }
            return Consecutivo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-Nombre de la tabla
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Obtener_Consecutivo(String Campo_ID, String Tabla)
        {
            int Consecutivo = 0;
            String Mi_Sql;
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),'00000') FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            return Consecutivo;
        }
        #endregion

    }//fin del Class
}//fin del Namespace