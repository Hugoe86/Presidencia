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
using Presidencia.Control_Patrimonial_Reporte_Siniestros.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Reporte_Siniestros.Datos {

    public class Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Datos {

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Listado_Siniestros
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Parametros.   Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 05/Mayo/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Obtener_Listado_Siniestros(Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Negocio Parametros){
                String Mi_SQL = null;
                DataSet Ds_Siniestros = null;
                DataTable Dt_Siniestros = new DataTable();
                Boolean Entro_Where = false;
                try {

                    Mi_SQL = "SELECT " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Siniestro_ID + " AS FOLIO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Siniestros.Tabla_Cat_Pat_Tipos_Siniestros + "." + Cat_Pat_Tipos_Siniestros.Campo_Descripcion + " AS TIPO_SINIESTRO";
                    Mi_SQL = Mi_SQL + ", (SELECT " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre;
                    Mi_SQL = Mi_SQL + " ||' / '|| " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario;
                    Mi_SQL = Mi_SQL + " ||' / '|| " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre;
                    Mi_SQL = Mi_SQL + " ||' / '|| " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Modelo;
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Marca_ID + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                    if (Parametros.P_No_Inventario != null && Parametros.P_No_Inventario.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " = '" + Parametros.P_No_Inventario + "'";
                    }
                    if (Parametros.P_Tipo_Vehiculo != null && Parametros.P_Tipo_Vehiculo.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Tipo_Vehiculo_ID + " = '" + Parametros.P_Tipo_Vehiculo + "'";
                    }
                    if (Parametros.P_Dependencia != null && Parametros.P_Dependencia.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia + "'";
                    }
                    if (Parametros.P_Resguardante != null && Parametros.P_Resguardante.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " IN (";
                        Mi_SQL = Mi_SQL + " SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'VEHICULO' AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Parametros.P_Resguardante + "')";
                    }
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " = " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Bien_ID + ") AS VEHICULO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora + "." + Cat_Pat_Aseguradora.Campo_Nombre + " AS ASEGURADORA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Bien_ID + " AS VEHICULO_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Fecha + " AS FECHA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Estatus + " AS ESTATUS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Responsable_Municipio + " AS MUNICIPIO_RESPONSABLE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Consignado + " AS CONSIGNADO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Pago_Danio_Sindicos + " AS PAGO_SINDICOS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Parte_Averiguacion + " AS NO_AVERIGUACION";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Reparacion + " AS REPARACION";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Numero_Reporte + " AS NO_REPORTE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Tipos_Siniestros.Tabla_Cat_Pat_Tipos_Siniestros;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Tipo_Siniestro_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Siniestros.Tabla_Cat_Pat_Tipos_Siniestros + "." + Cat_Pat_Tipos_Siniestros.Campo_Tipo_Siniestro_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Aseguradora_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora + "." + Cat_Pat_Aseguradora.Campo_Aseguradora_ID;
                    if (Parametros.P_Aseguradora != null && Parametros.P_Aseguradora.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Aseguradora_ID + " = '" + Parametros.P_Aseguradora + "'";
                    }
                    if (Parametros.P_Tipo_Siniestro != null && Parametros.P_Tipo_Siniestro.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Tipo_Siniestro_ID + " = '" + Parametros.P_Tipo_Siniestro + "'";
                    }
                    if (Parametros.P_Reparacion != null && Parametros.P_Reparacion.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Reparacion + " = '" + Parametros.P_Reparacion + "'";
                    }
                    if (Parametros.P_Estatus != null && Parametros.P_Estatus.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                    }
                    if (Parametros.P_Mpio_Responsable != null && Parametros.P_Mpio_Responsable.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Responsable_Municipio + " = '" + Parametros.P_Mpio_Responsable + "'";
                    }
                    if (Parametros.P_Pago_Sindicos != null && Parametros.P_Pago_Sindicos.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Pago_Danio_Sindicos + " = '" + Parametros.P_Pago_Sindicos + "'";
                    }
                    if (Parametros.P_Consignado != null && Parametros.P_Consignado.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Consignado + " = '" + Parametros.P_Consignado + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Inicial) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Fecha + " >= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Inicial) + "'";
                    }
                    if (Parametros.P_Tomar_Fecha_Final) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Fecha + " <= '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Final) + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Siniestro_ID;
                    Ds_Siniestros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    
                    if (Ds_Siniestros != null) {
                        Dt_Siniestros = Ds_Siniestros.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Siniestros;
            }

            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Listado_Siniestros
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Parametros.   Contiene los parametros que se van a utilizar para
            ///                                     hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 05/Mayo/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Obtener_Listado_Siniestros_Observaciones(Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Negocio Parametros)
            {
                String Mi_SQL = null;
                DataSet Ds_Siniestros = null;
                DataTable Dt_Siniestros = new DataTable();
                try {

                    Mi_SQL = "SELECT " + Ope_Pat_Sinies_Observaciones.Tabla_Ope_Pat_Sinies_Observaciones + "." + Ope_Pat_Sinies_Observaciones.Campo_Siniestro_ID + " AS FOLIO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Sinies_Observaciones.Tabla_Ope_Pat_Sinies_Observaciones + "." + Ope_Pat_Sinies_Observaciones.Campo_Observacion_ID + " AS OBSERVACION_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Sinies_Observaciones.Tabla_Ope_Pat_Sinies_Observaciones + "." + Ope_Pat_Sinies_Observaciones.Campo_Fecha + " AS FECHA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Sinies_Observaciones.Tabla_Ope_Pat_Sinies_Observaciones + "." + Ope_Pat_Sinies_Observaciones.Campo_Observacion + " AS OBSERVACION";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Sinies_Observaciones.Tabla_Ope_Pat_Sinies_Observaciones + "." + Ope_Pat_Sinies_Observaciones.Campo_Usuario_Creo + " AS AUTOR";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Sinies_Observaciones.Tabla_Ope_Pat_Sinies_Observaciones;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Sinies_Observaciones.Campo_Observacion_ID;

                    Ds_Siniestros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    if (Ds_Siniestros != null)
                    {
                        Dt_Siniestros = Ds_Siniestros.Tables[0];
                    }
                }
                catch (Exception Ex)
                {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Siniestros;
            }

    }

}