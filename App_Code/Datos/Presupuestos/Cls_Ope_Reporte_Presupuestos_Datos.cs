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
using Presidencia.Reporte_Presupuestos.Datos;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Reporte_Presupuestos.Negocios;

/// <summary>
/// Summary description for Cls_Ope_Reporte_Presupuestos_Datos
/// </summary>
/// 
namespace Presidencia.Reporte_Presupuestos.Datos
{
    public class Cls_Ope_Reporte_Presupuestos_Datos
    {

        public static DataTable Consultar_Dependencias(Cls_Ope_Reporte_Presupuestos_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Campo_Clave;
            Mi_SQL = Mi_SQL + "||' '|| " + Cat_Dependencias.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Dependencias.Campo_Nombre;

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


        public static DataTable Consultar_Programas(Cls_Ope_Reporte_Presupuestos_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id;
            Mi_SQL = Mi_SQL + ", " + Cat_Sap_Proyectos_Programas.Campo_Clave;
            Mi_SQL = Mi_SQL + "||' '|| " + Cat_Sap_Proyectos_Programas.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Proyectos_Programas.Tabla_Cat_Sap_Proyectos_Programas;
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Sap_Proyectos_Programas.Campo_Nombre;

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


        public static DataTable Consultar_Fuentes_Financiamiento(Cls_Ope_Reporte_Presupuestos_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_SAP_Fuente_Financiamiento.Campo_Clave;
            Mi_SQL = Mi_SQL + " ||' '|| " + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento;
            if(Clase_Negocio.P_Dependencia_ID != null)
            {
                Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
                Mi_SQL = Mi_SQL + " =(SELECT " + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + "='" +Clase_Negocio.P_Dependencia_ID.Trim() +"'";
            }
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion;

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        }

        public static DataTable Consultar_Capitulos(Cls_Ope_Reporte_Presupuestos_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_SAP_Capitulos.Campo_Capitulo_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_SAP_Capitulos.Campo_Clave;
            Mi_SQL = Mi_SQL + " ||' '|| " + Cat_SAP_Capitulos.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM  " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos;
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_SAP_Capitulos.Campo_Clave + " ASC";

            
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Conceptos(Cls_Ope_Reporte_Presupuestos_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Sap_Concepto.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_Sap_Concepto.Campo_Clave;
            Mi_SQL = Mi_SQL + "||' '|| " + Cat_Sap_Concepto.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;

            if (Clase_Negocio.P_Capitulo_ID != null)
            {
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Concepto.Campo_Capitulo_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Capitulo_ID.Trim() + "'";
            }

            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Sap_Concepto.Campo_Clave + " ASC";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Partidas_Genericas(Cls_Ope_Reporte_Presupuestos_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_SAP_Partida_Generica.Campo_Clave;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_SAP_Partida_Generica.Campo_Descripcion;
            Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica;

            if(Clase_Negocio.P_Concepto_ID != null)
            {

                Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Partida_Generica.Campo_Concepto_ID;
                Mi_SQL = Mi_SQL + "='" +Clase_Negocio.P_Concepto_ID.Trim() +"'";

            }

            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_SAP_Partida_Generica.Campo_Clave + " ASC";



            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Partida_Especifica(Cls_Ope_Reporte_Presupuestos_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
            Mi_SQL = Mi_SQL + ", " + Cat_Sap_Partidas_Especificas.Campo_Clave;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Sap_Partidas_Especificas.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;

            if(Clase_Negocio.P_Partida_Generica_ID != null)
            {
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Partida_Generica_ID.Trim() + "'";
            }
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Sap_Partidas_Especificas.Campo_Clave + " ASC";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Presupuestos(Cls_Ope_Reporte_Presupuestos_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT ";
            Mi_SQL = Mi_SQL +Cat_Dependencias.Tabla_Cat_Dependencias+ "." + Cat_Dependencias.Campo_Nombre + " AS UR,";

            Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "."+ Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " FUENTE, ";
            Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Descripcion + " PROGRAMA, ";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Clave + " ||' '||";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_Sap_Partidas_Genericas.Campo_Descripcion + " PARTIDA_GENERICA,";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " ||' '||";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Descripcion + " PARTIDA,";
            Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Clave + "||' '||";
            Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Descripcion +" CAPITULO," ;
            Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Clave + "||' '||";
            Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Descripcion + " CONCEPTO,";
            Mi_SQL = Mi_SQL + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Aprobado + " ASIGNADO, ";
            Mi_SQL = Mi_SQL + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Ampliacion + " AMPLIACION, ";
            Mi_SQL = Mi_SQL + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Reduccion + " REDUCCION, ";
            Mi_SQL = Mi_SQL + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Modificado + " MODIFICADO, ";
            Mi_SQL = Mi_SQL + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Devengado + " DEVENGADO, ";
            Mi_SQL = Mi_SQL + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Pagado + " PAGADO, ";
            Mi_SQL = Mi_SQL + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Comprometido + " COMPROMETIDO, ";
            Mi_SQL = Mi_SQL + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Disponible + " DISPONIBLE, ";
            Mi_SQL = Mi_SQL + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Ejercido + " EJERCIDO ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado;
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
            Mi_SQL = Mi_SQL + " ON " +Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID ;
            Mi_SQL = Mi_SQL + "=" + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " JOIN " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento;
            Mi_SQL = Mi_SQL + " ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado  + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID;
            Mi_SQL = Mi_SQL + "=" + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas;
            Mi_SQL = Mi_SQL + " ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Proyecto_Programa_ID;
            Mi_SQL = Mi_SQL + "=" + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
            Mi_SQL = Mi_SQL + " ON " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "." + Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID;
            Mi_SQL = Mi_SQL + " = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;

            Mi_SQL = Mi_SQL + " JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica;
            Mi_SQL = Mi_SQL + " ON " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + "." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL +"=" + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
            Mi_SQL = Mi_SQL + " ON " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_SAP_Partida_Generica.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + "=" + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + " JOIN " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos;
            Mi_SQL = Mi_SQL + " ON " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID;
            Mi_SQL = Mi_SQL + " = " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID;
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado + "."+ Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID + " IS NOT NULL ";

            if (Clase_Negocio.P_Dependencia_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Cat_Dependencias.Tabla_Cat_Dependencias+ "."+ Cat_Dependencias.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Dependencia_ID.Trim() + "'";
            }
            if (Clase_Negocio.P_Programa_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Programa_ID.Trim() + "'";
            }

            if(Clase_Negocio.P_Fuente_Financiamiento_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Fuente_Financiamiento_ID.Trim() + "'";
             }

            if (Clase_Negocio.P_Concepto_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Concepto_ID.Trim() + "'";
            }

            if (Clase_Negocio.P_Capitulo_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Capitulo_ID.Trim() + "'";
            }

            if (Clase_Negocio.P_Partida_Generica_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + "." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Partida_Generica_ID.Trim() + "'";
            }

            if (Clase_Negocio.P_Partida_Especifica_ID != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Partida_Especifica_ID.Trim() + "'";
            }

            if (Clase_Negocio.P_Anio != null)
            {
                Mi_SQL = Mi_SQL + " AND " + Ope_Psp_Presupuesto_Aprobado.Tabla_Ope_Psp_Presupuesto_Aprobado+"."+ Ope_Psp_Presupuesto_Aprobado.Campo_Anio;
                Mi_SQL = Mi_SQL + "= " + Clase_Negocio.P_Anio;
            }

            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave;

            //"SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UR,";
            //Mi_SQL = Mi_SQL + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + 

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


    }
}//fin del namespace
