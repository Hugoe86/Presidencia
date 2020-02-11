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
using Presidencia.Listado_Almacen.Negocio;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Ope_Com_Listado_Datos
/// </summary>

namespace Presidencia.Listado_Almacen.Datos
{
    public class Cls_Ope_Com_Listado_Datos
    {

        #region Variables

        #endregion
        public Cls_Ope_Com_Listado_Datos()
        {
            
        }

        #region Metodos

        #region Metodos Proyectos_Partidas
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Proyectos
        ///DESCRIPCIÓN: Metodo que consulta la tabla de Proyectos_Programas
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 22/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public DataTable Consulta_Proyectos(Cls_Ope_Com_Listado_Negocio Datos_Listado)
        {
            String Mi_SQL = "SELECT DET." + Cat_Sap_Det_Prog_Dependencias.Campo_Proyecto_Programa_ID +
                            ", (SELECT " + Cat_Com_Proyectos_Programas.Campo_Nombre + " FROM " +
                            Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " WHERE " +
                            Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID +
                            "=DET." + Cat_Sap_Det_Prog_Dependencias.Campo_Proyecto_Programa_ID + ")" +
                            " FROM " + Cat_Sap_Det_Prog_Dependencias.Tabla_Cat_Sap_Det_Prog_Dependencias + " DET " +
                            " WHERE DET." + Cat_Sap_Det_Prog_Dependencias.Campo_Dependencia_ID +
                            "='" + Cls_Sessiones.Dependencia_ID_Empleado + "'" +
                            " ORDER BY (SELECT " + Cat_Com_Proyectos_Programas.Campo_Nombre + " FROM " +
                            Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " WHERE " +
                            Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID +
                            "=DET." + Cat_Sap_Det_Prog_Dependencias.Campo_Proyecto_Programa_ID + ")";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
      
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Partidas
        ///DESCRIPCIÓN: Metodo que consulta la tabla de Partidas
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 22/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Partidas(Cls_Ope_Com_Listado_Negocio Datos_Listado)
        {
            String Mi_SQL = "SELECT PRO." + Cat_Com_Productos.Campo_Partida_ID +
                            ", (SELECT " + Cat_Com_Partidas.Campo_Clave + "||' '||"+ Cat_Com_Partidas.Campo_Nombre +
                            " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                            " WHERE " + Cat_Com_Partidas.Campo_Partida_ID +
                            " = PRO." + Cat_Com_Productos.Campo_Partida_ID + ") AS NOMBRE" +
                            " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRO" +
                            " WHERE PRO." + Cat_Com_Productos.Campo_Estatus + "='ACTIVO'" +
                            " GROUP BY (PRO. " + Cat_Com_Productos.Campo_Partida_ID + ")" +
                            " ORDER BY (SELECT " + Cat_Com_Partidas.Campo_Clave + "||' '||"+ Cat_Com_Partidas.Campo_Nombre +
                            " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                            " WHERE " + Cat_Com_Partidas.Campo_Partida_ID +
                            " = PRO." + Cat_Com_Productos.Campo_Partida_ID + ")";                            
            
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
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
        public DataTable Consultar_Presupuesto_Partidas(Cls_Ope_Com_Listado_Negocio Datos_Listado)
        {
            //String Mi_SQL = "SELECT " + Cat_Com_Parametros.Campo_Partida_Esp_Almacen_Global +
            //        ", " + Cat_Com_Parametros.Campo_Programa_Almacen +
            //        " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros;
            //DataTable Dt_Almacen = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            String Mi_SQL = "SELECT " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                            ", " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio +
                            " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                            " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + "= (SELECT " + Cat_Com_Parametros.Campo_Partida_Esp_Almacen_Global +
                            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "=(SELECT " + Cat_Com_Parametros.Campo_Programa_Almacen +
                            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "=(SELECT " + Cat_Com_Parametros.Campo_Dependencia_ID_Almacen +
                            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + "= TO_CHAR(SYSDATE,'YYYY')" +
                            " ORDER BY " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + " DESC";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Giro
        ///DESCRIPCIÓN: Metodo que consulta la tabla de Giros
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 22/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Giro()
        {
            String Mi_SQL = "SELECT " + Cat_Com_Giros.Campo_Giro_ID +
                            ", " + Cat_Com_Giros.Campo_Nombre +
                            " FROM " + Cat_Com_Giros.Tabla_Cat_Com_Giros +
                            " ORDER BY " + Cat_Com_Giros.Campo_Nombre;
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        #endregion fin Metodos Proyectos Partidas

        #region Manejo de las tablas Listado

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Listado
        ///DESCRIPCIÓN: Metodo que consulta la tabla de listado
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 22/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Listado(Cls_Ope_Com_Listado_Negocio Datos_Listado)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT LISTADO." + Ope_Com_Listado.Campo_Folio +
                     ",  TO_CHAR(LISTADO." + Ope_Com_Listado.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA_CREO" +
                     ", LISTADO." + Ope_Com_Listado.Campo_Tipo +
                     ", LISTADO." + Ope_Com_Listado.Campo_Estatus +
                     ", LISTADO." + Ope_Com_Listado.Campo_Total +
                     ", LISTADO." + Ope_Com_Listado.Campo_Listado_ID +
                     ", (SELECT " + Cat_Sap_Partidas_Especificas.Campo_Clave + "||' '||" +
                     Cat_Sap_Partidas_Especificas.Campo_Nombre + " FROM " +
                     Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " WHERE " +
                     Cat_Sap_Partidas_Especificas.Campo_Partida_ID + "=LISTADO." + Ope_Com_Listado.Campo_No_Partida_ID + ") AS PARTIDA" +
                     " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado + " LISTADO";
                     

            if (Datos_Listado.P_Estatus_Busqueda != null)
            {

                Mi_SQL = Mi_SQL + " WHERE LISTADO." + Ope_Com_Listado.Campo_Estatus + "=" + "'" + Datos_Listado.P_Estatus_Busqueda + "'";
            }
            else
            {
                Mi_SQL = Mi_SQL + " WHERE LISTADO." + Ope_Com_Listado.Campo_Estatus + " IN(" +
                     "'EN CONSTRUCCION')";
            }

            if (Datos_Listado.P_Fecha_Inicial != null)
            {
                Mi_SQL = Mi_SQL + " AND LISTADO." + Ope_Com_Listado.Campo_Fecha_Creo + " BETWEEN '" + Datos_Listado.P_Fecha_Inicial + "'" +
                    " AND '" + Datos_Listado.P_Fecha_Final + "'";
            }            
            if (Datos_Listado.P_Folio_Busqueda != null)
            {
                Mi_SQL = Mi_SQL + " AND UPPER(LISTADO." + Ope_Com_Listado.Campo_Folio +
                    ") LIKE UPPER('%" + Datos_Listado.P_Folio_Busqueda + "%')";
            }

            if (Datos_Listado.P_Folio != null)
            {

                Mi_SQL = "SELECT " + Ope_Com_Listado.Campo_Folio +
                         ",  TO_CHAR(" + Ope_Com_Listado.Campo_Fecha_Creo + ",'DD/MON/YYYY') AS FECHA_CREO" +
                         ", " + Ope_Com_Listado.Campo_Estatus +
                         ", " + Ope_Com_Listado.Campo_Tipo +
                         ", " + Ope_Com_Listado.Campo_Total +
                         ", " + Ope_Com_Listado.Campo_No_Partida_ID +
                         ", " + Ope_Com_Listado.Campo_Listado_ID +
                         " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado +
                         " WHERE " + Ope_Com_Listado.Campo_Folio + "='" + Datos_Listado.P_Folio + "'";

            }

            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Listado
        ///DESCRIPCIÓN: Metodo que da de alta un listado 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 22/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public String Alta_Listado(Cls_Ope_Com_Listado_Negocio Datos_Listado)
        {
            //VAriable que almacena si la operacion fue exitosa o no
            String Mensaje_Operacion = "";
                //OBTENEMOS EL ID DEL PROYECTO Y PARTIDA QUE SON GENERICAS DEL ALMACEN
                String Mi_SQL = "SELECT " + Cat_Com_Parametros.Campo_Partida_Esp_Almacen_Global +
                    ", " + Cat_Com_Parametros.Campo_Programa_Almacen +
                    " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros;
                DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                //Datos_Listado.P_Partida_ID = Data_Table.Rows[0][Cat_Com_Parametros.Campo_Partida_Esp_Almacen_Global].ToString();
                Datos_Listado.P_Proyecto_ID = Data_Table.Rows[0][Cat_Com_Parametros.Campo_Programa_Almacen].ToString();

                //Generamos el Id y folio del listado nuevo
                Datos_Listado.P_Listado_ID = Consecutivo(Ope_Com_Listado.Campo_Listado_ID, Ope_Com_Listado.Tabla_Ope_Com_Listado);
                Datos_Listado.P_Folio = "LA-" + int.Parse(Datos_Listado.P_Listado_ID).ToString();
                //Sentencia que insertara el nuevo registro en la tabla
                Mi_SQL = " INSERT INTO " + Ope_Com_Listado.Tabla_Ope_Com_Listado +
                                " (" + Ope_Com_Listado.Campo_Listado_ID +
                                ", " + Ope_Com_Listado.Campo_No_Partida_ID +
                                ", " + Ope_Com_Listado.Campo_No_Proyecto_ID +
                                ", " + Ope_Com_Listado.Campo_Folio +
                                ", " + Ope_Com_Listado.Campo_Fecha_Creo +
                                ", " + Ope_Com_Listado.Campo_Estatus;
                //Verificamos el Estatus para insertar el id del empleado y la fecha correspondiente al campo del estatus

                if (Datos_Listado.P_Estatus == "EN CONSTRUCCION")
                {
                    Mi_SQL += ", " + Ope_Com_Listado.Campo_Empleado_Construccion_ID +
                              ", " + Ope_Com_Listado.Campo_Fecha_Construccion;

                }
                if (Datos_Listado.P_Estatus == "GENERADA")
                {
                    Mi_SQL += ", " + Ope_Com_Listado.Campo_Empleado_Construccion_ID +
                             ", " + Ope_Com_Listado.Campo_Fecha_Construccion;
                    Mi_SQL += ", " + Ope_Com_Listado.Campo_Empleado_Generacion_ID +
                              ", " + Ope_Com_Listado.Campo_Fecha_Generacion;
                }
                if (Datos_Listado.P_Estatus == "CANCELADA")
                {
                    Mi_SQL += ", " + Ope_Com_Listado.Campo_Empleado_Cancelacion_ID +
                              "', " + Ope_Com_Listado.Campo_Fecha_Cancelacion;
                }

                Mi_SQL = Mi_SQL + ", " + Ope_Com_Listado.Campo_Tipo +
                                ", " + Ope_Com_Listado.Campo_Total + ") " +
                                " VALUES ('" + Datos_Listado.P_Listado_ID +
                                "', '" + Datos_Listado.P_Partida_ID +
                                "', '" + Datos_Listado.P_Proyecto_ID +
                                "', '" + Datos_Listado.P_Folio +
                                "', SYSDATE" +
                                ", '" + Datos_Listado.P_Estatus +
                                "', '" + Datos_Listado.P_Usuario_ID +
                                "', SYSDATE";
                if (Datos_Listado.P_Estatus == "GENERADA")
                {
                    Mi_SQL = Mi_SQL + ", '" + Datos_Listado.P_Usuario_ID +
                                "', SYSDATE";
                }
            Mi_SQL = Mi_SQL +", '" + Datos_Listado.P_Tipo +
                              
                                "', '" + Datos_Listado.P_Total + "')";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //Damos de alta los detalles del listado 


                if (Datos_Listado.P_Productos_Seleccionados.Rows.Count != 0)
                {

                    for (int i = 0; i < Datos_Listado.P_Productos_Seleccionados.Rows.Count; i++)
                    {
                        Mi_SQL = " INSERT INTO " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle +
                                 " (" + Ope_Com_Listado_Detalle.Campo_No_Listado_ID +
                                 ", " + Ope_Com_Listado_Detalle.Campo_No_Producto_ID +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Cantidad +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Costo_Compra +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Importe +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Monto_IVA +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Monto_IEPS +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Porcentaje_IVA +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Porcentaje_IEPS +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Usuario_Creo +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Fecha_Creo +
                                 ")" +
                                 " VALUES ('" + Datos_Listado.P_Listado_ID +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["PRODUCTO_ID"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["CANTIDAD"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["PRECIO_UNITARIO"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["IMPORTE"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["MONTO_IVA"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["MONTO_IEPS"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["PORCENTAJE_IVA"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["PORCENTAJE_IEPS"] +
                                 "', '" + Cls_Sessiones.Nombre_Empleado +
                                 "', SYSDATE) ";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }//fin del for


                }
                Mensaje_Operacion = "Se dio de alta el listado " + Datos_Listado.P_Folio;            
           
            return Mensaje_Operacion;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Listado
        ///DESCRIPCIÓN: Metodo que verifica el consecutivo en la tabla y ayuda a generar el nuevo Id. 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 24/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Modificar_Listado(Cls_Ope_Com_Listado_Negocio Datos_Listado)
        {
            String Mi_SQL = "";
            String Mensaje="";
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
                     " WHERE " + Ope_Com_Listado.Campo_Folio + " ='" + Datos_Listado.P_Folio + "'";

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            //Guardamos los valores de total para restarcelo al presupuesto 
           // double Presupuesto_Anterior = double.Parse(Data_Table.Rows[0][Ope_Com_Listado.Campo_Total].ToString());
            //String Estatus_Anterior = Data_Table.Rows[0][Ope_Com_Listado.Campo_Estatus].ToString();
            //ASIGNAMOS EL valor de Listado_Id para tenerlo siempre 
            Datos_Listado.P_Listado_ID = Data_Table.Rows[0][Ope_Com_Listado.Campo_Listado_ID].ToString();
            //los unicos datos que se pueden modificar en un listado son el estatus, el tipo, el total, el comentario
            Mi_SQL = "UPDATE " + Ope_Com_Listado.Tabla_Ope_Com_Listado +
                            " SET " + Ope_Com_Listado.Campo_Estatus +
                            " = '" + Datos_Listado.P_Estatus +
                            "', " + Ope_Com_Listado.Campo_Tipo +
                            " = '" + Datos_Listado.P_Tipo +
                            "', " + Ope_Com_Listado.Campo_Total +
                            " = '" + Datos_Listado.P_Total + "'";
            //Dependiendo del estatus se actualiza el id del empleado que cambio el estatus y la fecha
            if (Datos_Listado.P_Estatus == "EN CONSTRUCCION")
            {
                Mi_SQL += ", " + Ope_Com_Listado.Campo_Empleado_Construccion_ID +
                            " = '" + Datos_Listado.P_Usuario_ID + "'" +
                            ", " + Ope_Com_Listado.Campo_Fecha_Construccion +
                            " = SYSDATE";
            }
            if (Datos_Listado.P_Estatus == "GENERADA")
            {
                Mi_SQL += ", " + Ope_Com_Listado.Campo_Empleado_Generacion_ID +
                            " = '" + Datos_Listado.P_Usuario_ID + "'" +
                            ", " + Ope_Com_Listado.Campo_Fecha_Generacion +
                            " = SYSDATE";
                //Modificamos el presupuesto asignado para este listado
                //Modificar_Presupuestos(Datos_Listado);
            }
            if (Datos_Listado.P_Estatus == "CANCELADA")
            {
                Mi_SQL += ", " + Ope_Com_Listado.Campo_Empleado_Cancelacion_ID +
                            " = '" + Datos_Listado.P_Usuario_ID + "'" +
                            ", " + Ope_Com_Listado.Campo_Fecha_Cancelacion +
                            " = SYSDATE";
            }
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Listado.Campo_Listado_ID +
                            " = '" + Datos_Listado.P_Listado_ID + "'";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //###################################
            if (Datos_Listado.P_Estatus != "CANCELADA")
            {
            //PASO 2
            //Consultamos los productos anteriores para ver si a se modificaron los productos del Listado 
            Mi_SQL = "SELECT * FROM " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle +
                     " WHERE " + Ope_Com_Listado_Detalle.Campo_No_Listado_ID +
                     "='" + Datos_Listado.P_Listado_ID + "'";
            DataTable Productos_Anteriores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            
                //Recorremos el listado de los productos anteriores y lo comparamos con el nuevo si no lo encuentra lo elimina
                bool existe_producto = false;
                for (int i = 0; i < Productos_Anteriores.Rows.Count; i++)
                {

                    for (int j = 0; j < Datos_Listado.P_Productos_Seleccionados.Rows.Count; j++)
                    {
                        if ((Productos_Anteriores.Rows[i][Ope_Com_Listado_Detalle.Campo_No_Producto_ID].ToString()) == (Datos_Listado.P_Productos_Seleccionados.Rows[j][0].ToString()))
                        {
                            existe_producto = true;
                            Mi_SQL = "UPDATE " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle +
                                 " SET " + Ope_Com_Listado_Detalle.Campo_Cantidad +
                                 " ='" + Datos_Listado.P_Productos_Seleccionados.Rows[j]["CANTIDAD"] + "'" +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Costo_Compra +
                                 " ='" + Datos_Listado.P_Productos_Seleccionados.Rows[j]["PRECIO_UNITARIO"] + "'" +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Importe +
                                 " ='" + Datos_Listado.P_Productos_Seleccionados.Rows[j]["IMPORTE"] + "'" +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Monto_IVA +
                                 " ='" + Datos_Listado.P_Productos_Seleccionados.Rows[j]["MONTO_IVA"] + "'" +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Monto_IEPS +
                                 " ='" + Datos_Listado.P_Productos_Seleccionados.Rows[j]["MONTO_IEPS"] + "'" +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Porcentaje_IVA +
                                 " ='" + Datos_Listado.P_Productos_Seleccionados.Rows[j]["PORCENTAJE_IVA"] + "'" +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Porcentaje_IEPS +
                                 " ='" + Datos_Listado.P_Productos_Seleccionados.Rows[j]["PORCENTAJE_IEPS"] + "'" +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Usuario_Modifico +
                                 " ='" + Datos_Listado.P_Usuario + "'" +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Fecha_Modifico +
                                 "= SYSDATE" +
                                 " WHERE " + Ope_Com_Listado_Detalle.Campo_No_Listado_ID +
                                 " ='" + Datos_Listado.P_Listado_ID + "'" +
                                 " AND " + Ope_Com_Listado_Detalle.Campo_No_Producto_ID +
                                 " ='" + Datos_Listado.P_Productos_Seleccionados.Rows[j]["PRODUCTO_ID"] + "'";
                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                            break;
                        }
                        else
                            existe_producto = false;
                    }//fin for j

                    //en caso de no existir el producto se elimina de la base de datos
                    if (existe_producto == false)
                    {
                        Mi_SQL = "DELETE FROM " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle +
                                 " WHERE " + Ope_Com_Listado_Detalle.Campo_No_Producto_ID +
                                 "='" + Productos_Anteriores.Rows[i][Ope_Com_Listado_Detalle.Campo_No_Producto_ID].ToString() + "'" +
                                 " AND " + Ope_Com_Listado_Detalle.Campo_No_Listado_ID +
                                 "='" + Datos_Listado.P_Listado_ID + "'";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }//Fin del if

                }//fin for i
                //AGREGAMOS LOS NUEVOS PRODUCTOS 
                //ahora recorremos los dos data table pero a la inversa en busca de nuevos productos agregados 
                existe_producto = true;
                for (int i = 0; i < Datos_Listado.P_Productos_Seleccionados.Rows.Count; i++)
                {
                    for (int j = 0; j < Productos_Anteriores.Rows.Count; j++)
                    {
                        if ((Productos_Anteriores.Rows[j][Ope_Com_Listado_Detalle.Campo_No_Producto_ID].ToString()) == (Datos_Listado.P_Productos_Seleccionados.Rows[i][0].ToString()))
                        {
                            existe_producto = true;
                            break;
                        }
                        else
                            existe_producto = false;
                    }//fin for j 
                    //Insertamos el nuevo producto agregado
                    if (existe_producto == false)
                    {
                        Mi_SQL = " INSERT INTO " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle +
                                 " (" + Ope_Com_Listado_Detalle.Campo_No_Listado_ID +
                                 ", " + Ope_Com_Listado_Detalle.Campo_No_Producto_ID +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Cantidad +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Costo_Compra +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Importe +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Monto_IVA +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Monto_IEPS +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Porcentaje_IVA +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Porcentaje_IEPS +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Usuario_Creo +
                                 ", " + Ope_Com_Listado_Detalle.Campo_Fecha_Creo +

                                 ")" +
                                 " VALUES ('" + Datos_Listado.P_Listado_ID +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["PRODUCTO_ID"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["CANTIDAD"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["PRECIO_UNITARIO"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["IMPORTE"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["MONTO_IVA"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["MONTO_IEPS"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["PORCENTAJE_IVA"] +
                                 "', '" + Datos_Listado.P_Productos_Seleccionados.Rows[i]["PORCENTAJE_IEPS"] +
                                 "', '" + Datos_Listado.P_Usuario +
                                 "', SYSDATE) ";
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                }//fin for i


                //AHORA MODIFICAMOS EL PRESUPUESTO RESTANDO EL ANTERIOR DEL COMPROMETIDO Y ASIGNANDOLE EL NUEVO
                //Modificar_Presupuestos(Datos_Listado);
                //Y al final se modifica el nuevo presupuesto
                Mi_SQL = "UPDATE " + Ope_Com_Listado.Tabla_Ope_Com_Listado +
                        " SET " + Ope_Com_Listado.Campo_Total +
                        " = '" + Datos_Listado.P_Total + "'" +
                        " WHERE " + Ope_Com_Listado.Campo_Listado_ID +
                        " = '" + Datos_Listado.P_Listado_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Mensaje = "Se modifico satisfactoriamente el Listado de Almacen " + Datos_Listado.P_Folio;
            }
            else
            {
                //Cancelamos el presupuesto asignado 
                //Liberar_Presupuesto_Cancelada(Datos_Listado);
                Mensaje = "Se cancelo satisfactoriamente el listado";
            }

            return Mensaje;


        }//Fin de Modificar_Listado

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
        public void Liberar_Presupuesto_Cancelada(Cls_Ope_Com_Listado_Negocio Datos_Listado)
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
                     " WHERE " + Ope_Com_Listado.Campo_Listado_ID + " ='" + Datos_Listado.P_Listado_ID + "'";

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            //Como ya se obtuvieron los detalles d la licitacion antes de ser modificada
            //Usamos los datos del monto y la partida para liberar presupuestos correspondientes
            double Monto = double.Parse(Data_Table.Rows[0][Ope_Com_Listado.Campo_Total].ToString());
            String Partida = Data_Table.Rows[0][Ope_Com_Listado.Campo_No_Partida_ID].ToString();
            String Programa = Data_Table.Rows[0][Ope_Com_Listado.Campo_No_Proyecto_ID].ToString();

          
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
        ///NOMBRE DE LA FUNCIÓN: Modificar_Presupuestos
        ///DESCRIPCIÓN: Metodo que modifica los presupuestos en caso de tener monto disponible 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 08/Febrero/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public bool Modificar_Presupuestos(Cls_Ope_Com_Listado_Negocio Datos_Listado)
        {
            DataTable Data_Table = Consultar_Presupuesto_Partidas(Datos_Listado);
            String No_Asignacion_Anio = Data_Table.Rows[0][Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio].ToString();
            double Monto_Disponible = double.Parse(Data_Table.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Disponible].ToString());
            double Monto_Total = double.Parse(Datos_Listado.P_Total);
            String Mi_SQL = "";
            bool Existe_Presupuesto = false;

            if (Monto_Disponible >= Monto_Total)
            {
            
                //se ACTUALIZAN LOS PRESUPUESTOS DE LA DEPENDENCIA, CORRESPONDIENTE AL PROYECTO Y PARTIDA

                Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                    " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                    " =" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " - " + Monto_Total +
                    "," + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                    "=" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " + " + Monto_Total +
                    " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + "= (SELECT " + Cat_Com_Parametros.Campo_Partida_Esp_Almacen_Global +
                            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "=(SELECT " + Cat_Com_Parametros.Campo_Programa_Almacen +
                            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "=(SELECT " + Cat_Com_Parametros.Campo_Dependencia_ID_Almacen +
                            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")" +
                    " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio +
                    " = '" + No_Asignacion_Anio + "'" +
                    " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                    "= TO_CHAR(SYSDATE,'YYYY')";
                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                ////ACTUALIZAMOS LOS PRESUPUESTOS DE LA PARTIDA
                //Mi_SQL = "UPDATE " + Ope_Com_Pres_Partida.Tabla_Ope_Com_Pres_Partida +
                //    " SET " + Ope_Com_Pres_Partida.Campo_Monto_Disponible +
                //    " =" + Ope_Com_Pres_Partida.Campo_Monto_Disponible + " - " + Monto_Total +
                //    "," + Ope_Com_Pres_Partida.Campo_Monto_Comprometido +
                //    "=" + Ope_Com_Pres_Partida.Campo_Monto_Comprometido + " + " + Monto_Total +
                //    " WHERE " + Ope_Com_Pres_Partida.Campo_Partida_ID +
                //    "='" + Datos_Listado.P_Partida_ID + "'" +
                //    " AND " + Ope_Com_Pres_Partida.Campo_Anio_Presupuesto +
                //    "= TO_CHAR(SYSDATE,'YYYY')";
                //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                ////ACUTUALIZAMOS LOS PRESUPUESTOS DEL PROYECTO
                //Mi_SQL = "UPDATE " + Ope_Com_Pres_Prog_Proy.Tabla_Ope_Com_Pres_Prog_Proy +
                //    " SET " + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible +
                //    " =" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Disponible + " + " + Monto_Total +
                //    "," + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido +
                //    "=" + Ope_Com_Pres_Prog_Proy.Campo_Monto_Comprometido + " - " + Monto_Total +
                //    " WHERE " + Ope_Com_Pres_Prog_Proy.Campo_Pres_Prog_Proy_ID +
                //    "='" + Datos_Listado.P_Proyecto_ID + "'" +
                //    " AND " + Ope_Com_Pres_Prog_Proy.Campo_Anio_Presupuesto +
                //    "= TO_CHAR(SYSDATE,'YYYY')";
                //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Existe_Presupuesto = true;
            }
            else
            {
                Existe_Presupuesto = false;

            }
            return Existe_Presupuesto;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Afectar_Presupuesto
        ///DESCRIPCIÓN: Metodo que afecta el presupuesto ya ocupado.  
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 24/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public bool Afectar_Presupuesto(Cls_Ope_Com_Listado_Negocio Datos_Listado)
        {
            //variable que indica si afecta o no el presupuesto
            bool Afecta_Presupuesto = false;
            bool Suma_Diferencia = false;
            //consultamos el total anterior al cual ya se le asigno presupuesto
            String Mi_SQL = "SELECT " + Ope_Com_Listado.Campo_Total +
                " FROM " + Ope_Com_Listado.Tabla_Ope_Com_Listado + 
                " WHERE " + Ope_Com_Listado.Campo_Listado_ID + 
                "='" +Datos_Listado.P_Listado_ID + "'";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            double Total_Ultimo_Presupuestado = double.Parse(Data_Table.Rows[0][Ope_Com_Listado.Campo_Total].ToString());
            
            //PASO 1
            //Verificamos si existe alguna diferencia de los totales el anterior ya presupuestado y el nuevo 
            // PASO 1 VERIFICAMOS CUAL DE LOS 2 MONTOS ES MAYOR SI EL COTIZADO O  EL ANTERIOR

            //Variable del Monto nuevo a cotizar
            double Total_Nuevo_A_Presupuestar = double.Parse(Datos_Listado.P_Total);
            //VAriable del Diferencia de los montos el cual se cotizara o no 
            double Diferencia = 0;
            //Variable que indica si existe una diferencia ositiva a la cual se tiene que solicitar mas 
            if (Total_Nuevo_A_Presupuestar > Total_Ultimo_Presupuestado)
            {
                //Obtenemos la resta
                Diferencia = Total_Nuevo_A_Presupuestar - Total_Ultimo_Presupuestado;
                Suma_Diferencia = true;
            }
            if (Total_Nuevo_A_Presupuestar < Total_Ultimo_Presupuestado)
            {
                //obtener resta
                Diferencia = Total_Ultimo_Presupuestado - Total_Nuevo_A_Presupuestar;
                Suma_Diferencia = false;
            }

            //PASO 2 AFECTAMOS PRESUPUESTO
            //Consultamos el presupuesto que aun se tiene
            DataTable Data_Partida = Consultar_Presupuesto_Partidas(Datos_Listado);
            double Presupuesto_Disponible = double.Parse(Data_Partida.Rows[0][Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString());
            String No_Asignacion_Anio = Data_Partida.Rows[0][Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio].ToString();
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
                        " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + "= (SELECT " + Cat_Com_Parametros.Campo_Partida_Esp_Almacen_Global +
                            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "=(SELECT " + Cat_Com_Parametros.Campo_Programa_Almacen +
                            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "=(SELECT " + Cat_Com_Parametros.Campo_Dependencia_ID_Almacen +
                            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + "= TO_CHAR(SYSDATE,'YYYY')" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio +
                            "= " + No_Asignacion_Anio + "'" +
                            
                    //Sentencia que ejecuta el query
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    Afecta_Presupuesto = true;    
                 

                }
                else
                {
                    //Si no existe presupuesto modificamos Mandamos hacemos negativa la variable de afectar_Presupuesto, para porsteriormente realizar validaciones 
                    Afecta_Presupuesto = false;                  
                }

            }//fin if SumaDiferencia
            else
            {
                //Modificamos el presupuesto, ya que se resta el monto que sobro pues el valor Total nuevo de la licitacion es menor k el anterior
                //se ACTUALIZAN LOS PRESUPUESTOS DE LA DEPENDENCIA, CORRESPONDIENTE AL PROYECTO Y PARTIDA DE ALMACEN

                Mi_SQL = "UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                    " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible +
                    " =" + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " + " + Diferencia.ToString() +
                    "," + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido +
                    "=" + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " - " + Diferencia.ToString() +
                    " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + "= (SELECT " + Cat_Com_Parametros.Campo_Partida_Esp_Almacen_Global +
                            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + "=(SELECT " + Cat_Com_Parametros.Campo_Programa_Almacen +
                            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "=(SELECT " + Cat_Com_Parametros.Campo_Dependencia_ID_Almacen +
                            " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros + ")" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + "= TO_CHAR(SYSDATE,'YYYY')" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + "='" +No_Asignacion_Anio + "'";
                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Afecta_Presupuesto = true;
               

            }


            //En caso de ser falso la variable de Afectar_Presupuesto es por k no existe presupuesto 


            return Afecta_Presupuesto;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Productos_Reorden
        ///DESCRIPCIÓN: Metodo que consulta los productos que se encunentran en reorden 
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 24/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public DataTable Consultar_Productos_Reorden(Cls_Ope_Com_Listado_Negocio Datos_Listado)
        {
            String Mi_SQL = "";

            if (Datos_Listado.P_Tipo == "AUTOMATICO")
            {
                Mi_SQL = "SELECT PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Clave +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO_NOMBRE" +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion +
                         ", (SELECT " + Cat_Com_Unidades.Campo_Nombre + 
                         " FROM " +Cat_Com_Unidades.Tabla_Cat_Com_Unidades +
                         " WHERE " + Cat_Com_Unidades.Campo_Unidad_ID + 
                         "= PRODUCTOS." + Cat_Com_Productos.Campo_Unidad_ID + ") AS UNIDAD"+
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Existencia +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Reorden +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Maximo + "-" +
                         " PRODUCTOS." + Cat_Com_Productos.Campo_Existencia + " AS CANTIDAD" +
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
                         " WHERE PRODUCTOS." + Cat_Com_Productos.Campo_Stock + " = 'SI'" +
                         " AND PRODUCTOS." + Cat_Com_Productos.Campo_Partida_ID + "='" + Datos_Listado.P_Partida_ID + "'" +
                         " AND ((PRODUCTOS." + Cat_Com_Productos.Campo_Existencia + "<=" +
                         " PRODUCTOS." + Cat_Com_Productos.Campo_Reorden +
                         ") OR (PRODUCTOS." + Cat_Com_Productos.Campo_Existencia + "<=" +
                         " PRODUCTOS." + Cat_Com_Productos.Campo_Minimo +
                         ")) AND PRODUCTOS." + Cat_Com_Productos.Campo_Estatus + "='ACTIVO'" + 
                         " AND PRODUCTOS." + Cat_Com_Productos.Campo_Maximo + "> 0" +
                         " AND PRODUCTOS." +Cat_Com_Productos.Campo_Maximo + " - " +
                         " PRODUCTOS." + Cat_Com_Productos.Campo_Disponible + " > 0"+
                         " ORDER BY PRODUCTOS." + Cat_Com_Productos.Campo_Nombre;

            }

            if (Datos_Listado.P_Producto_ID != null)
            {
                
                Mi_SQL = "SELECT PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Clave +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO_NOMBRE" +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion +
                         ", (SELECT " + Cat_Com_Unidades.Campo_Nombre +
                         " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades +
                         " WHERE " + Cat_Com_Unidades.Campo_Unidad_ID +
                         "=PRODUCTOS." + Cat_Com_Productos.Campo_Unidad_ID + ") AS UNIDAD" +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Existencia +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Reorden +
                         ", PRODUCTOS." + Cat_Com_Productos.Campo_Maximo + "-" +
                         " PRODUCTOS." + Cat_Com_Productos.Campo_Disponible + " AS CANTIDAD" +
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
                         " WHERE PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + "='" +
                         Datos_Listado.P_Producto_ID + "'";

            }


            if (Datos_Listado.P_Folio != null)
            {
                Mi_SQL = "SELECT LISTADO." + Ope_Com_Listado_Detalle.Campo_No_Producto_ID + " AS PRODUCTO_ID" +
                         ", PRODUCTO." + Cat_Com_Productos.Campo_Clave +
                         ", PRODUCTO." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO_NOMBRE" +
                         ", PRODUCTO." + Cat_Com_Productos.Campo_Descripcion +
                         ", (SELECT " + Cat_Com_Unidades.Campo_Nombre +
                         " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades +
                         " WHERE " + Cat_Com_Unidades.Campo_Unidad_ID +
                         "=PRODUCTO." + Cat_Com_Productos.Campo_Unidad_ID + ") AS UNIDAD" +
                         ", PRODUCTO." + Cat_Com_Productos.Campo_Existencia +
                         ", PRODUCTO." + Cat_Com_Productos.Campo_Reorden +
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
                         " WHERE LISTADO." + Ope_Com_Listado_Detalle.Campo_No_Listado_ID + " = '" + Datos_Listado.P_Listado_ID + "'" +
                         " AND PRODUCTO." + Cat_Com_Productos.Campo_Estatus + "='ACTIVO'" +
                         " ORDER BY PRODUCTO." + Cat_Com_Productos.Campo_Nombre;
            }
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }
      
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Modelos
        ///DESCRIPCIÓN: Metodo que consulta de los modelos en existencia de los productos
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 02/Marzo/11
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Modelos(Cls_Ope_Com_Listado_Negocio Datos_Listado)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Modelos.Campo_Modelo_ID +
                            ", " + Cat_Com_Modelos.Campo_Nombre +
                            " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos +
                            " ORDER BY " + Cat_Com_Subfamilias.Campo_Nombre;
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:  Consultar_Productos
        ///DESCRIPCIÓN: Metodo que consulta de los modelos en existencia de los productos
        ///PARAMETROS: 1.- Cls_Ope_Com_Listado_Negocio Datos_Listado objeto de la clase negocio
        ///CREO:
        ///FECHA_CREO: 02/Marzo/11
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Productos(Cls_Ope_Com_Listado_Negocio Datos_Listado)
        {
            String Mi_SQL = "";

            Mi_SQL = "SELECT PRODUCTO." + Cat_Com_Productos.Campo_Clave + ", PRODUCTO." + Cat_Com_Productos.Campo_Producto_ID +
                 " AS PRODUCTO_SERVICIO_ID, PRODUCTO." + Cat_Com_Productos.Campo_Nombre +
                 " AS PRODUCTO_SERVICIO, PRODUCTO." + Cat_Com_Productos.Campo_Descripcion +
                 " AS DESCRIPCION, (SELECT " + Cat_Com_Unidades.Campo_Nombre +
                 " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades +
                 " WHERE " + Cat_Com_Unidades.Campo_Unidad_ID +
                 "=PRODUCTO." + Cat_Com_Productos.Campo_Unidad_ID + ") AS UNIDAD" + 
                 ", PRODUCTO." + Cat_Com_Productos.Campo_Costo +
                 " AS PRECIO_UNITARIO, PRODUCTO." + Cat_Com_Productos.Campo_Existencia + " FROM " +
                 Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTO " +
                 " WHERE PRODUCTO." + Cat_Com_Productos.Campo_Stock + " = 'SI'" +
                 " AND PRODUCTO." + Cat_Com_Productos.Campo_Estatus + "='ACTIVO'";
                 
                  
            if (Datos_Listado.P_Partida_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND PRODUCTO." + Cat_Com_Productos.Campo_Partida_ID +
                    " = '" + Datos_Listado.P_Partida_ID + "'";
            }
            if (Datos_Listado.P_Nombre_Producto != null)
            {
                Mi_SQL = Mi_SQL + " AND UPPER(PRODUCTO." + Cat_Com_Productos.Campo_Nombre +
                    ") LIKE UPPER('%" +Datos_Listado.P_Nombre_Producto +"%')";
            }
            Mi_SQL = Mi_SQL + " ORDER BY PRODUCTO." + Cat_Com_Productos.Campo_Nombre;


            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
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
        ///NOMBRE DE LA FUNCIÓN: Alta_Observaciones
        ///DESCRIPCIÓN: Metodo que ejecuta la sentencia SQL para dar de alta observaciones
        ///PARAMETROS:  1.- Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Alta_Observaciones_Listado(Cls_Ope_Com_Listado_Negocio Listado_Negocio)
        {
            String Mi_SQL = "INSERT INTO " + Ope_Alm_Com_Obs_Listado.Tabla_Ope_Alm_Com_Obs_Listados +
                            " (" + Ope_Alm_Com_Obs_Listado.Campo_Obs_listado_ID +
                            ", " + Ope_Alm_Com_Obs_Listado.Campo_No_Listado_ID +
                            ", " + Ope_Alm_Com_Obs_Listado.Campo_Comentario +
                            ", " + Ope_Alm_Com_Obs_Listado.Campo_Estatus +
                            ", " + Ope_Alm_Com_Obs_Listado.Campo_Usuario_Creo +
                            ", " + Ope_Alm_Com_Obs_Listado.Campo_Fecha_Creo +
                            ") VALUES ('" + Obtener_Consecutivo(Ope_Alm_Com_Obs_Listado.Campo_Obs_listado_ID, Ope_Alm_Com_Obs_Listado.Tabla_Ope_Alm_Com_Obs_Listados).ToString() + "','" +
                                Listado_Negocio.P_Listado_ID + "','" +
                                Listado_Negocio.P_Comentarios + "','" +
                                Listado_Negocio.P_Estatus + "','" +
                                Listado_Negocio.P_Usuario + "',SYSDATE)";
            //Sentencia que ejecuta el query
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        }//fin de Alta_Observacion

        public DataTable Consultar_Observaciones_Listado(Cls_Ope_Com_Listado_Negocio Listado_Negocio)
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

        #endregion Fin_Metodos

    }//fin del class
}//fin del namespace