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
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Definir_Cotizadores.Negocio;


/// <summary>
/// Summary description for Cls_Ope_Com_Definir_Cotizadores_Datos
/// </summary>
/// 
namespace Presidencia.Definir_Cotizadores.Datos
{
public class Cls_Ope_Com_Definir_Cotizadores_Datos
{
    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Alta_Cotizadores_Asignados
    ///DESCRIPCIÓN: Metodo que Asigna los cotizadores seleccionados y agregados al Grid_Requisiciones
    ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static bool Alta_Cotizadores_Asignados(Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios)
    {
        bool Realizada= false;
        String Mi_SQL = "";
        try{
        for (int i = 0; i < Clase_Negocios.P_Dt_Requisiciones.Rows.Count; i++)
        {
            if (Clase_Negocios.P_Dt_Requisiciones.Rows[i]["COTIZADOR_ID"].ToString().Trim() != String.Empty)
            {
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " SET " + Ope_Com_Requisiciones.Campo_Cotizador_ID;
                Mi_SQL = Mi_SQL + " ='" + Clase_Negocios.P_Dt_Requisiciones.Rows[i]["COTIZADOR_ID"].ToString().Trim() + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Estatus + "='FILTRADA'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " ='" + Clase_Negocios.P_Dt_Requisiciones.Rows[i]["NO_REQUISICION"].ToString().Trim() + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //Realizamos el alta del Historial
                Cls_Util.Registrar_Historial("FILTRADA", Clase_Negocios.P_Dt_Requisiciones.Rows[i]["NO_REQUISICION"].ToString().Trim());

            }//fin del IF
         }//fin del for

            Realizada = true;
        }
        catch (Exception EX)
        {
            Realizada = false;
        }
        return Realizada;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Alta_Cotizadores_Asignados
    ///DESCRIPCIÓN: Metodo que Asigna los cotizadores seleccionados y agregados al Grid_Requisiciones
    ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static bool Reasignar_Cotizadores(Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios)
    {
        bool Realizada = false;
        String Mi_SQL = "";
        try
        {
            for (int i = 0; i < Clase_Negocios.P_Dt_Requisiciones.Rows.Count; i++)
            {
                if (Clase_Negocios.P_Dt_Requisiciones.Rows[i]["COTIZADOR_ID"].ToString().Trim() != String.Empty)
                {
                    Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Requisiciones.Campo_Cotizador_ID;
                    Mi_SQL = Mi_SQL + " ='" + Clase_Negocios.P_Dt_Requisiciones.Rows[i]["COTIZADOR_ID"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Estatus + "='FILTRADA'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Campo_Leido_Por_Cotizador + " = NULL ";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                    Mi_SQL = Mi_SQL + " ='" + Clase_Negocios.P_Dt_Requisiciones.Rows[i]["NO_REQUISICION"].ToString().Trim() + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Cotizador_ID + " != '" + Clase_Negocios.P_Dt_Requisiciones.Rows[i]["COTIZADOR_ID"].ToString().Trim() + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            

                }//fin del IF
            }//fin del for

            Realizada = true;
        }
        catch (Exception EX)
        {
            Realizada = false;
        }
        return Realizada;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Consultar_Productos_Servicios
    ///DESCRIPCIÓN: Metodo que Consulta los detalles de la Requisicion seleccionada, ya sea Producto o servicio.
    ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static DataTable Consultar_Productos_Servicios(Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocio)
    {
        String Mi_SQL = "";
        switch (Clase_Negocio.P_Tipo_Articulo)
        {
            case "PRODUCTO":
                Mi_SQL = "SELECT PRO." + Cat_Com_Productos.Campo_Clave;
                Mi_SQL = Mi_SQL + ", PRO." + Cat_Com_Productos.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", PRO." + Cat_Com_Productos.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Cantidad;
                Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Monto_Total;
                Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Importe;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ";
                Mi_SQL = Mi_SQL + " INNER JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRO ";
                Mi_SQL = Mi_SQL + " ON REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "=";
                Mi_SQL = Mi_SQL + " REQ_PRO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRO";
                Mi_SQL = Mi_SQL + " ON PRO." + Cat_Com_Productos.Campo_Producto_ID + "=";
                Mi_SQL = Mi_SQL + " REQ_PRO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                Mi_SQL = Mi_SQL + " WHERE REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='"+Clase_Negocio.P_No_Requisicion.Trim()+"'";
                Mi_SQL = Mi_SQL + " ORDER BY PRO." + Cat_Com_Productos.Campo_Nombre;
            break;
            case "SERVICIO":
                Mi_SQL = "SELECT SER." + Cat_Com_Servicios.Campo_Clave;
                Mi_SQL = Mi_SQL + ", SER." + Cat_Com_Servicios.Campo_Nombre;
                Mi_SQL = Mi_SQL + ", SER." + Cat_Com_Servicios.Campo_Comentarios + " AS DESCRIPCION";
                Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Cantidad;
                Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Monto_Total;
                Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Importe;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ";
                Mi_SQL = Mi_SQL + " INNER JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRO ";
                Mi_SQL = Mi_SQL + " ON REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "=";
                Mi_SQL = Mi_SQL + " REQ_PRO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " SER";
                Mi_SQL = Mi_SQL + " ON SER." + Cat_Com_Servicios.Campo_Servicio_ID + "=";
                Mi_SQL = Mi_SQL + " REQ_PRO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                Mi_SQL = Mi_SQL + " WHERE REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "='" + Clase_Negocio.P_No_Requisicion.Trim() + "'";
                Mi_SQL = Mi_SQL + " ORDER BY SER." + Cat_Com_Servicios.Campo_Nombre;
            break;
        
        }

        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Consultar_Cotizadores
    ///DESCRIPCIÓN: Metodo que Consulta los cotizadores dados de alta, y que tenglan el concepto de las requisiciones dentro del Grid_Requisiciones
    ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static DataTable Consultar_Cotizadores()
    {
        String Mi_SQL = "SELECT " + Cat_Com_Cotizadores.Campo_Empleado_ID +
        ", " + Cat_Com_Cotizadores.Campo_Nombre_Completo +
        " FROM " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores +
        " GROUP BY (" + Cat_Com_Cotizadores.Campo_Empleado_ID+ ", "+ Cat_Com_Cotizadores.Campo_Nombre_Completo + ")" +
        " ORDER BY " + Cat_Com_Cotizadores.Campo_Nombre_Completo;


        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Consultar_Conceptos
    ///DESCRIPCIÓN: Metodo que consulta los conceptos que contiene las requisicion de la cotizacion
    ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static DataTable Consultar_Partidas_Especificas(Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios)
    {

        String Mi_SQL = "SELECT PARTIDA." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
        Mi_SQL = Mi_SQL + ", PARTIDA." + Cat_Sap_Partidas_Especificas.Campo_Clave;
        Mi_SQL = Mi_SQL + "||' '|| PARTIDA." + Cat_Sap_Partidas_Especificas.Campo_Nombre;
        Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDA";
        Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRO";
        Mi_SQL = Mi_SQL + " ON REQ_PRO." + Ope_Com_Req_Producto.Campo_Partida_ID;
        Mi_SQL = Mi_SQL + "= PARTIDA." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
        Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ";
        Mi_SQL = Mi_SQL + " ON REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
        Mi_SQL = Mi_SQL + " = REQ_PRO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
        Mi_SQL = Mi_SQL + " WHERE REQ." + Ope_Com_Requisiciones.Campo_Tipo;
        Mi_SQL = Mi_SQL + "='TRANSITORIA'";
        Mi_SQL = Mi_SQL + " AND REQ." + Ope_Com_Requisiciones.Campo_Estatus;
        Mi_SQL = Mi_SQL + " IN ('FILTRADA','PROCESAR')";


        if (Clase_Negocios.P_Reasignar == false)
        {
            Mi_SQL = Mi_SQL + " AND REQ." + Ope_Com_Requisiciones.Campo_Cotizador_ID;
            Mi_SQL = Mi_SQL + " IS NULL";
        }

        if (Clase_Negocios.P_Reasignar == true)
        {
            Mi_SQL = Mi_SQL + " AND REQ." + Ope_Com_Requisiciones.Campo_Cotizador_ID;
            Mi_SQL = Mi_SQL + " IS NOT NULL";
        }

        Mi_SQL = Mi_SQL + " GROUP BY PARTIDA." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
        Mi_SQL = Mi_SQL + ", PARTIDA." + Cat_Sap_Partidas_Especificas.Campo_Clave;
        Mi_SQL = Mi_SQL + ", PARTIDA." + Cat_Sap_Partidas_Especificas.Campo_Nombre;
        Mi_SQL = Mi_SQL + " ORDER BY PARTIDA." + Cat_Sap_Partidas_Especificas.Campo_Clave;

    
        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Consultar_Requisiciones
    ///DESCRIPCIÓN: Metodo que consulta las requisiciones listas para ser distribuidas a los cotizadores
    ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static DataTable Consultar_Requisiciones(Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios)
    {
        String Mi_SQL = "SELECT REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
        Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Folio;
        Mi_SQL = Mi_SQL + ", DEP." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
        Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Partida_ID;

        Mi_SQL = Mi_SQL + ", (SELECT " + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio + " FROM ";
        Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " WHERE ";
        Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
        Mi_SQL = Mi_SQL + " AND ROWNUM = 1) AS PRODUCTO";

        Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Sap_Partidas_Especificas.Campo_Clave;
        Mi_SQL = Mi_SQL + "||' '||" + Cat_Sap_Partidas_Especificas.Campo_Nombre;
        Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
        Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
        Mi_SQL = Mi_SQL + "=REQ_PRO." + Ope_Com_Req_Producto.Campo_Partida_ID + ") AS PARTIDA_ESPECIFICA";
        
        Mi_SQL = Mi_SQL + ", TO_CHAR(REQ." + Ope_Com_Requisiciones.Campo_Fecha_Filtrado + ",'DD/MON/YYYY') AS FECHA";
        Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Total ;
        Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Cotizador_ID;
        Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno;
        Mi_SQL = Mi_SQL + "||' '||"+ Cat_Empleados.Campo_Apellido_Materno + " FROM ";
        Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID;
        Mi_SQL = Mi_SQL + "=REQ." + Ope_Com_Requisiciones.Campo_Cotizador_ID + ") AS NOMBRE_COTIZADOR";
        Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQ";
        Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEP";
        Mi_SQL = Mi_SQL + " ON DEP." + Cat_Dependencias.Campo_Dependencia_ID + "=";
        Mi_SQL = Mi_SQL + " REQ." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
        Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRO";
        Mi_SQL = Mi_SQL + " ON REQ_PRO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
        Mi_SQL = Mi_SQL + "= REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
        Mi_SQL = Mi_SQL + " WHERE REQ." + Ope_Com_Requisiciones.Campo_Tipo + "='TRANSITORIA'";
        Mi_SQL = Mi_SQL + " AND REQ." + Ope_Com_Requisiciones.Campo_Estatus + " IN ('PROCESAR','FILTRADA')";
        //Agregamos este filtro para cargar a un inicio solo los que no se han asignado y en caso de hacer reasignacion solo las que ya se asiganron
        if (Clase_Negocios.P_Reasignar == true)
        {
            Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Cotizador_ID +" IS NOT NULL";

        }
        else
        {
            Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Campo_Cotizador_ID + " IS NULL";
        }

        Mi_SQL = Mi_SQL + " GROUP BY ";
        Mi_SQL = Mi_SQL + " REQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
        Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Folio;
        Mi_SQL = Mi_SQL + ", DEP." + Cat_Dependencias.Campo_Nombre;
        Mi_SQL = Mi_SQL + ", REQ_PRO." + Ope_Com_Req_Producto.Campo_Partida_ID;
        Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Fecha_Filtrado;
        Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Total;
        Mi_SQL = Mi_SQL + ", REQ." + Ope_Com_Requisiciones.Campo_Cotizador_ID;
        Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Requisiciones.Campo_Requisicion_ID;


        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Consultar_Detalle_Requisicion
    ///DESCRIPCIÓN: Metodo que consulta los detalles de la requisicion seleccionada en el Grid_Requisiciones
    ///PARAMETROS: 1.- Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios, objeto de la clase de negocios
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public static DataTable Consultar_Detalle_Requisicion(Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios)
    {
        String Mi_SQL = "SELECT ";
        Mi_SQL = Mi_SQL + " DEPENDENCIA." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
        Mi_SQL = Mi_SQL + ",(SELECT " + Cat_Sap_Concepto.Campo_Clave + " ||' '|| " + Cat_Sap_Concepto.Campo_Descripcion + " FROM ";
        Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID + "=(SELECT ";
        Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + " FROM ";
        Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " WHERE ";
        Mi_SQL = Mi_SQL + "CAT_SAP_PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
        Mi_SQL = Mi_SQL + "=(SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
        Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = REQUISICION." + Ope_Com_Requisiciones.Campo_Partida_ID + "))) AS CONCEPTO";
        Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Tipo;
        Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Folio;
        Mi_SQL = Mi_SQL + ", TO_CHAR( REQUISICION." + Ope_Com_Requisiciones.Campo_Fecha_Generacion + ",'DD/MON/YYYY') AS FECHA_GENERACION";
        Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Estatus;
        Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Subtotal;
        Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_IEPS;
        Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_IVA;
        Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Total;
        Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
        Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Justificacion_Compra;
        Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Especificacion_Prod_Serv;
        Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Verificaion_Entrega;
        Mi_SQL = Mi_SQL + ", REQUISICION." + Ope_Com_Requisiciones.Campo_Tipo_Articulo;
        Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICION ";
        Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA ";
        Mi_SQL = Mi_SQL + " ON REQUISICION." + Ope_Com_Requisiciones.Campo_Dependencia_ID + "= DEPENDENCIA.";
        Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID;
        Mi_SQL = Mi_SQL + " WHERE REQUISICION." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
        Mi_SQL = Mi_SQL + "='" + Clase_Negocios.P_No_Requisicion.Trim() + "'";
        

        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
    }

    #endregion
}
}