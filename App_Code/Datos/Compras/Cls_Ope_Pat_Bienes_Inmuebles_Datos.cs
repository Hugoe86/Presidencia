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
using Presidencia.Control_Patrimonial_Operacion_Bienes_Inmuebles.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Collections.Generic;

/// <summary>
/// Summary description for Cls_Ope_Pat_Bienes_Inmuebles_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Operacion_Bienes_Inmuebles.Datos { 
    
    public class Cls_Ope_Pat_Bienes_Inmuebles_Datos {

        #region "Metodos"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Bien_Inmueble
            ///DESCRIPCIÓN          : Da de alta en la Base de Datos un nuevo registro.
            ///PARAMETROS           : Parametros.   Contiene los parametros que se van a dar de
            ///                                     Alta en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Febrero/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static Cls_Ope_Pat_Bienes_Inmuebles_Negocio Alta_Bien_Inmueble(Cls_Ope_Pat_Bienes_Inmuebles_Negocio Parametros) {
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                String Mi_SQL = "";
                try {

                    Parametros.P_Bien_Inmueble_ID = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Inmuebles.Tabla_Ope_Pat_Bienes_Inmuebles, Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID, 10);
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Inmuebles.Tabla_Ope_Pat_Bienes_Inmuebles;
                    Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Calle_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Uso_ID;
                    if (Parametros.P_Superficie > (-1)) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Superficie;
                    }
                    if (Parametros.P_Construccion_Construida > (-1)) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Construccion;
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Manzana;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Cuenta_Predial_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Lote;
                    if (Parametros.P_Ocupacion > (-1)) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Porcentaje_Ocupacion;
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Efectos_Fiscales;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Sector_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Clasificacion_Zona_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Predio_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Vias_Acceso;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Estado;
                    if (Parametros.P_Densidad_Construccion > (-1)) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Densidad_Construccion;
                    }
                    if (Parametros.P_Valor_Comercial > (-1)) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Valor_Comercial;
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Exterior;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Interior;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Destino_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Origen_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Estatus;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Area_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Hoja;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Tomo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Acta;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Cartilla_Parcelaria;
                    if (Parametros.P_Superficie_Contable > (-1)) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Superficie_Contable;
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Unidad_Superficie;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Clase_Activo_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Bien;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Registro_Propiedad;
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Alta_Cuenta_Publica).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Alta_Cta_Pub;
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + ") VALUES ('" + Parametros.P_Bien_Inmueble_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Calle + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Colonia + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Uso_ID + "'";
                    if (Parametros.P_Superficie > (-1)) {
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Superficie + "'";
                    }
                    if (Parametros.P_Construccion_Construida > (-1)) {
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Construccion_Construida + "'";
                    }
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Manzana + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Cuenta_Predial_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Lote + "'";
                    if (Parametros.P_Ocupacion > (-1)) {
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Ocupacion + "'";
                    }
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Efectos_Fiscales + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Sector_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Clasificacion_Zona_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Tipo_Predio_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Vias_Acceso + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Estado + "'";
                    if (Parametros.P_Densidad_Construccion > (-1)) {
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Densidad_Construccion + "'";
                    }
                    if (Parametros.P_Valor_Comercial > (-1)) {
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Valor_Comercial + "'";
                    }
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Exterior + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Interior+ "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Destino_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Origen_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Area_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Registro) + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Hoja + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Tomo + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Numero_Acta + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Cartilla_Parcelaria + "'";
                    if (Parametros.P_Superficie_Contable > (-1)) {
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Superficie_Contable + "'";
                    }
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Unidad_Superficie + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Clase_Activo_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Tipo_Bien + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Registro_Propiedad + "'";
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Alta_Cuenta_Publica).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Alta_Cuenta_Publica) + "'";
                    }
                    Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                    Mi_SQL = Mi_SQL + ", SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    if (!String.IsNullOrEmpty(Parametros.P_Observaciones)) {
                        Int32 No_Observacion = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_B_Inm_Observaciones.Tabla_Ope_Pat_B_Inm_Observaciones, Ope_Pat_B_Inm_Observaciones.Campo_No_Observacion, 20));
                        Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Observaciones.Tabla_Ope_Pat_B_Inm_Observaciones;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Observaciones.Campo_No_Observacion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Bien_Inmueble_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Fecha_Observacion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Usuario_Observacion_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Observacion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES ('" + No_Observacion + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Observaciones + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }

                    if (Parametros.P_Dt_Medidas_Colindancias != null && Parametros.P_Dt_Medidas_Colindancias.Rows.Count > 0) {
                        Int32 No_Registro = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas, Ope_Pat_B_Inm_Medidas.Campo_No_Registro, 20));
                        foreach (DataRow Fila in Parametros.P_Dt_Medidas_Colindancias.Rows) {
                            Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas;
                            Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Medidas.Campo_No_Registro;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Campo_Bien_Inmueble_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Campo_Orientacion_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Campo_Medida;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Campo_Colindancia;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Campo_Usuario_Creo;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Campo_Fecha_Creo;
                            Mi_SQL = Mi_SQL + ") VALUES ('" + No_Registro + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'";
                            Mi_SQL = Mi_SQL + ", '" + Fila["ORIENTACION_ID"].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Fila["MEDIDA"].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Fila["COLINDANCIA"].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", SYSDATE)";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            No_Registro = No_Registro + 1;
                        }
                    }

                    if (Parametros.P_No_Escritura != null && Parametros.P_No_Escritura.Trim().Length > 0) {
                        Int32 No_Registro = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico, Ope_Pat_B_Inm_Juridico.Campo_No_Registro, 20));
                        Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Juridico.Campo_No_Registro;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Escritura;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Escritura;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_No_Notario;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Nombre_Notario;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Constancia_Registral;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Folio_Real;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Libertad_Gravament;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Antecedente_Registral;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Proveedor;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Observaciones;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_No_Contrato;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Movimiento;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES ('" + No_Registro + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'"; 
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Escritura + "'";
                        Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Escritura) + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Notario + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Notario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Constancia_Registral + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Folio_Real + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Libre_Gravament + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Antecedente + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Proveedor + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Observaciones + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Contrato_Juridico + "'";
                        Mi_SQL = Mi_SQL + ", 'ALTA'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }

                    if (Parametros.P_Archivo != null && Parametros.P_Archivo.Trim().Length > 0) {
                        Int32 No_Registro = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_B_Inm_Archivos.Tabla_Ope_Pat_B_Inm_Archivos, Ope_Pat_B_Inm_Archivos.Campo_No_Registro, 20));
                        Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Archivos.Tabla_Ope_Pat_B_Inm_Archivos;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Archivos.Campo_No_Registro;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Bien_Inmueble_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Tipo_Archivo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Descripcion_Archivo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Ruta_Archivo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Usuario_Cargo_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Fecha_Cargo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES ('" + No_Registro + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'"; 
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Tipo_Anexo + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Descripcion_Anexo + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Archivo + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_ID + "'";
                        Mi_SQL = Mi_SQL + ", 'ACTIVO'";
                        Mi_SQL = Mi_SQL + ", SYSDATE";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }

                    if (!String.IsNullOrEmpty(Parametros.P_Expropiacion)) {
                        Int32 No_Expropiacion = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_B_Inm_Expropiaciones.Tabla_Ope_Pat_B_Inm_Expropiaciones, Ope_Pat_B_Inm_Expropiaciones.Campo_No_Expropiacion, 20));
                        Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Expropiaciones.Tabla_Ope_Pat_B_Inm_Expropiaciones;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Expropiaciones.Campo_No_Expropiacion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Bien_Inmueble_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Fecha_Expropiacion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Usuario_Expropiacion_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Descripcion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES ('" + No_Expropiacion + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Expropiacion + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }

                    if (Parametros.P_Agregar_Afectacion) {
                        Int32 No_Registro = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_B_Inm_Afectaciones.Tabla_Ope_Pat_B_Inm_Afectaciones, Ope_Pat_B_Inm_Afectaciones.Campo_No_Registro, 20));
                        Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Afectaciones.Tabla_Ope_Pat_B_Inm_Afectaciones;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Afectaciones.Campo_No_Registro;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Bien_Inmueble_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Fecha_Afectacion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Fecha_Registro;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Usuario_Registro_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Propietario;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Session_Ayuntamiento;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Tramo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_No_Contrato;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES ('" + No_Registro + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Afectacion) + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Nuevo_Propietario + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Session_Ayuntamiento + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Tramo + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Contrato + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }

                    Trans.Commit();
                } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Code == 2627) {
                        if (Ex.Message.IndexOf("PRIMARY") != -1) {
                            Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                        } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                            Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                        } else {
                            Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                        }
                    } else if (Ex.Code == 547) {
                        Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                    } else if (Ex.Code == 515) {
                        Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error al intentar dar de Alta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
                return Parametros;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modifica_Bien_Inmueble
            ///DESCRIPCIÓN          : Actualiza el registro.
            ///PARAMETROS           : Parametros.   Contiene los parametros que se van a dar de
            ///                                     Alta en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 29/Febrero/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static Cls_Ope_Pat_Bienes_Inmuebles_Negocio Modifica_Bien_Inmueble(Cls_Ope_Pat_Bienes_Inmuebles_Negocio Parametros) {
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                String Mi_SQL = "";
                try {

                    Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Inmuebles.Tabla_Ope_Pat_Bienes_Inmuebles;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Inmuebles.Campo_Calle_ID + " = '" + Parametros.P_Calle + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID + " = '" + Parametros.P_Colonia + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Uso_ID + " = '" + Parametros.P_Uso_ID + "'";
                    if (Parametros.P_Superficie > (-1)) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Superficie + " = '" + Parametros.P_Superficie + "'";
                    } else {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Superficie + " = NULL";
                    }
                    if (Parametros.P_Construccion_Construida > (-1)) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Construccion + " = '" + Parametros.P_Construccion_Construida + "'";
                    } else {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Construccion + " = NULL";
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Manzana + " = '" + Parametros.P_Manzana + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Cuenta_Predial_ID + " = '" + Parametros.P_Cuenta_Predial_ID + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Lote + " = '" + Parametros.P_Lote + "'";
                    if (Parametros.P_Ocupacion > (-1)) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Porcentaje_Ocupacion + " = '" + Parametros.P_Ocupacion + "'";
                    } else {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Porcentaje_Ocupacion + " = NULL";
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Efectos_Fiscales + " = '" + Parametros.P_Efectos_Fiscales + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Sector_ID + " = '" + Parametros.P_Sector_ID + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Clasificacion_Zona_ID + " = '" + Parametros.P_Clasificacion_Zona_ID + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Predio_ID + " = '" + Parametros.P_Tipo_Predio_ID + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Vias_Acceso + " = '" + Parametros.P_Vias_Acceso + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Estado + " = '" + Parametros.P_Estado + "'";
                    if (Parametros.P_Densidad_Construccion > (-1)) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Densidad_Construccion + " = '" + Parametros.P_Densidad_Construccion + "'";
                    } else {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Densidad_Construccion + " = NULL";
                    }
                    if (Parametros.P_Valor_Comercial > (-1)) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Valor_Comercial + " = '" + Parametros.P_Valor_Comercial + "'";
                    } else {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Valor_Comercial + " = NULL";
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Exterior + " = '" + Parametros.P_No_Exterior + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Interior + " = '" + Parametros.P_No_Interior + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Destino_ID + " = '" + Parametros.P_Destino_ID + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Origen_ID + " = '" + Parametros.P_Origen_ID + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Area_ID + " = '" + Parametros.P_Area_ID + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro + " = '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Registro) + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Tomo + " = '" + Parametros.P_Tomo + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Hoja + " = '" + Parametros.P_Hoja + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Acta + " = '" + Parametros.P_Numero_Acta + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Cartilla_Parcelaria + " = '" + Parametros.P_Cartilla_Parcelaria + "'";
                    if (Parametros.P_Superficie_Contable > (-1)) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Superficie_Contable + " = '" + Parametros.P_Superficie_Contable + "'";
                    } else {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Superficie_Contable + " = NULL";
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Unidad_Superficie + " = '" + Parametros.P_Unidad_Superficie + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Clase_Activo_ID + " = '" + Parametros.P_Clase_Activo_ID + "'";
                    if (Parametros.P_Estado.Trim().Equals("BAJA")) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Baja + " = '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Baja) + "'";
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Bien + " = '" + Parametros.P_Tipo_Bien + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Registro_Propiedad + " = '" + Parametros.P_Registro_Propiedad + "'";
                    if (!String.Format("{0:ddMMyyyy}", Parametros.P_Fecha_Alta_Cuenta_Publica).Equals(String.Format("{0:ddMMyyyy}", new DateTime()))) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Alta_Cta_Pub + " = '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Alta_Cuenta_Publica) + "'";
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Usuario_Modifico + " =  '" + Parametros.P_Usuario_Nombre + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " = '" + Parametros.P_Bien_Inmueble_ID + "'";

                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    if (!String.IsNullOrEmpty(Parametros.P_Observaciones)) {
                        Int32 No_Observacion = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_B_Inm_Observaciones.Tabla_Ope_Pat_B_Inm_Observaciones, Ope_Pat_B_Inm_Observaciones.Campo_No_Observacion, 20));
                        Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Observaciones.Tabla_Ope_Pat_B_Inm_Observaciones;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Observaciones.Campo_No_Observacion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Bien_Inmueble_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Fecha_Observacion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Usuario_Observacion_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Observacion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES ('" + No_Observacion + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Observaciones + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }

                    Mi_SQL = "DELETE FROM " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas
                           + " WHERE " + Ope_Pat_B_Inm_Medidas.Campo_Bien_Inmueble_ID + " = '" + Parametros.P_Bien_Inmueble_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    if (Parametros.P_Dt_Medidas_Colindancias != null && Parametros.P_Dt_Medidas_Colindancias.Rows.Count > 0) {
                        Int32 No_Registro = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas, Ope_Pat_B_Inm_Medidas.Campo_No_Registro, 20));
                        foreach (DataRow Fila in Parametros.P_Dt_Medidas_Colindancias.Rows) {
                            Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas;
                            Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Medidas.Campo_No_Registro;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Campo_Bien_Inmueble_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Campo_Orientacion_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Campo_Medida;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Campo_Colindancia;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Campo_Usuario_Creo;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Campo_Fecha_Creo;
                            Mi_SQL = Mi_SQL + ") VALUES ('" + No_Registro + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'";
                            Mi_SQL = Mi_SQL + ", '" + Fila["ORIENTACION_ID"].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Fila["MEDIDA"].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Fila["COLINDANCIA"].ToString() + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", SYSDATE)";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            No_Registro = No_Registro + 1;
                        }
                    }

                    Int32 No_Registro_Tabla_Juridico = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico, Ope_Pat_B_Inm_Juridico.Campo_No_Registro, 20));
                    if (Parametros.P_No_Escritura != null && Parametros.P_No_Escritura.Trim().Length > 0) {
                        if (Parametros.P_No_Registro_Alta_Juridico.Trim().Length == 0) {
                            Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico;
                            Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Juridico.Campo_No_Registro;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Escritura;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Escritura;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_No_Notario;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Nombre_Notario;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Constancia_Registral;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Folio_Real;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Libertad_Gravament;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Antecedente_Registral;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Proveedor;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Observaciones;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_No_Contrato;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Movimiento;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Usuario_Creo;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Creo;
                            Mi_SQL = Mi_SQL + ") VALUES ('" + No_Registro_Tabla_Juridico + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Escritura + "'";
                            Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Escritura) + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Notario + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Notario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Constancia_Registral + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Folio_Real + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Libre_Gravament + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Antecedente + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Proveedor + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Observaciones + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Contrato_Juridico + "'";
                            Mi_SQL = Mi_SQL + ", 'ALTA'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", SYSDATE)";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            No_Registro_Tabla_Juridico = No_Registro_Tabla_Juridico + 1;
                        } else {
                            Mi_SQL = "UPDATE " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_B_Inm_Juridico.Campo_Escritura + " = '" + Parametros.P_No_Escritura + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Escritura + " = '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Escritura) + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_No_Notario + " = '" + Parametros.P_No_Notario + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Nombre_Notario + " = '" + Parametros.P_Notario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Constancia_Registral + " = '" + Parametros.P_Constancia_Registral + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Folio_Real + " = '" + Parametros.P_Folio_Real + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Libertad_Gravament + " = '" + Parametros.P_Libre_Gravament + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Antecedente_Registral + " = '" + Parametros.P_Antecedente + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Proveedor + " = '" + Parametros.P_Proveedor + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_No_Contrato + " = '" + Parametros.P_No_Contrato_Juridico + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Usuario_Modifico + " = '" + Parametros.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Juridico.Campo_No_Registro + "= '" + Parametros.P_No_Registro_Alta_Juridico + "'";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                    }

                    if (Parametros.P_Estado.Trim().Equals("BAJA")) {
                        if (Parametros.P_No_Registro_Baja_Juridico.Trim().Length == 0) {
                            Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico;
                            Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Juridico.Campo_No_Registro;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Escritura;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Escritura;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_No_Notario;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Nombre_Notario;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Constancia_Registral;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Folio_Real;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Nuevo_Propietario;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_No_Contrato;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Movimiento;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Usuario_Creo;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Creo;
                            Mi_SQL = Mi_SQL + ") VALUES ('" + No_Registro_Tabla_Juridico + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Escritura_Baja + "'";
                            Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Escritura_Baja) + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Notario_Baja + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Notario_Nombre_Baja + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Constancia_Registral_Baja + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Folio_Real_Baja + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Nuevo_Propietario_Juridico + "'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Contrato_Baja + "'";
                            Mi_SQL = Mi_SQL + ", 'BAJA'";
                            Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", SYSDATE)";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            No_Registro_Tabla_Juridico = No_Registro_Tabla_Juridico + 1;
                        } else {
                            Mi_SQL = "UPDATE " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_B_Inm_Juridico.Campo_Escritura + " = '" + Parametros.P_No_Escritura_Baja + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Escritura + " = '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Escritura_Baja) + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_No_Notario + " = '" + Parametros.P_No_Notario_Baja + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Nombre_Notario + " = '" + Parametros.P_Notario_Nombre_Baja + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Constancia_Registral + " = '" + Parametros.P_Constancia_Registral_Baja + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Folio_Real + " = '" + Parametros.P_Folio_Real_Baja + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Nuevo_Propietario + " = '" + Parametros.P_Nuevo_Propietario_Juridico + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_No_Contrato + " = '" + Parametros.P_No_Contrato_Baja + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Usuario_Modifico + " = '" + Parametros.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Juridico.Campo_No_Registro + "= '" + Parametros.P_No_Registro_Baja_Juridico + "'";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                    }

                    if (Parametros.P_Archivo != null && Parametros.P_Archivo.Trim().Length > 0) {
                        Int32 No_Registro = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_B_Inm_Archivos.Tabla_Ope_Pat_B_Inm_Archivos, Ope_Pat_B_Inm_Archivos.Campo_No_Registro, 20));
                        Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Archivos.Tabla_Ope_Pat_B_Inm_Archivos;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Archivos.Campo_No_Registro;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Bien_Inmueble_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Tipo_Archivo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Descripcion_Archivo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Ruta_Archivo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Usuario_Cargo_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Fecha_Cargo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES ('" + No_Registro + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'"; 
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Tipo_Anexo + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Descripcion_Anexo + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Archivo + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_ID + "'";
                        Mi_SQL = Mi_SQL + ", 'ACTIVO'";
                        Mi_SQL = Mi_SQL + ", SYSDATE";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }

                    if (Parametros.P_Dt_Anexos_Bajas != null && Parametros.P_Dt_Anexos_Bajas.Rows.Count > 0) {
                        foreach (DataRow Fila in Parametros.P_Dt_Anexos_Bajas.Rows) {
                            Mi_SQL = "UPDATE " + Ope_Pat_B_Inm_Archivos.Tabla_Ope_Pat_B_Inm_Archivos;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_B_Inm_Archivos.Campo_Estatus + " = 'INACTIVO'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Usuario_Modifico + " = '" + Parametros.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Archivos.Campo_No_Registro + " = '" + Fila["No_Registro"].ToString().Trim() + "'";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                    }

                    if (!String.IsNullOrEmpty(Parametros.P_Expropiacion)) {
                        Int32 No_Expropiacion = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_B_Inm_Expropiaciones.Tabla_Ope_Pat_B_Inm_Expropiaciones, Ope_Pat_B_Inm_Expropiaciones.Campo_No_Expropiacion, 20));
                        Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Expropiaciones.Tabla_Ope_Pat_B_Inm_Expropiaciones;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Expropiaciones.Campo_No_Expropiacion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Bien_Inmueble_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Fecha_Expropiacion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Usuario_Expropiacion_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Descripcion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES ('" + No_Expropiacion + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Expropiacion + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }

                    if (Parametros.P_Agregar_Afectacion) {
                        Int32 No_Registro = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_B_Inm_Afectaciones.Tabla_Ope_Pat_B_Inm_Afectaciones, Ope_Pat_B_Inm_Afectaciones.Campo_No_Registro, 20));
                        Mi_SQL = "INSERT INTO " + Ope_Pat_B_Inm_Afectaciones.Tabla_Ope_Pat_B_Inm_Afectaciones;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_B_Inm_Afectaciones.Campo_No_Registro;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Bien_Inmueble_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Fecha_Afectacion;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Fecha_Registro;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Usuario_Registro_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Propietario;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Session_Ayuntamiento;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Tramo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_No_Contrato;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Usuario_Creo;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES ('" + No_Registro + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Bien_Inmueble_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Afectacion) + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Nuevo_Propietario + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Session_Ayuntamiento + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Tramo + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_No_Contrato + "'";
                        Mi_SQL = Mi_SQL + ", '" + Parametros.P_Usuario_Nombre + "'";
                        Mi_SQL = Mi_SQL + ", SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }

                    Trans.Commit();
                } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Code == 2627) {
                        if (Ex.Message.IndexOf("PRIMARY") != -1) {
                            Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                        } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                            Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                        } else {
                            Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                        }
                    } else if (Ex.Code == 547) {
                        Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                    } else if (Ex.Code == 515) {
                        Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error al intentar dar de Alta. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
                return Parametros;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Bienes_Inmuebles
            ///DESCRIPCIÓN          : Carga un listado de los bienes.
            ///PARAMETROS           : Parametros. Contiene los parametros que se van a Consultar
            ///                                   en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Febrero/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Bienes_Inmuebles(Cls_Ope_Pat_Bienes_Inmuebles_Negocio Parametros) {
                String Mi_SQL = null;
                DataSet Ds_Datos = null;
                DataTable Dt_Datos = new DataTable();
                Boolean Entro_Where = false;
                try {
                    Mi_SQL = "SELECT BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " AS BIEN_INMUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS CALLE";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Exterior + " AS NUMERO_EXTERIOR";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Numero_Interior + " AS NUMERO_INTERIOR";
                    Mi_SQL = Mi_SQL + ", COLONIAS." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA";
                    Mi_SQL = Mi_SQL + ", CUENTAS_PREDIAL." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " AS CUENTA_PREDIAL";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Manzana + " AS MANZANA";
                    Mi_SQL = Mi_SQL + ", BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Lote + " AS LOTE";
                    Mi_SQL = Mi_SQL + ", CUENTAS_PREDIAL." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + " AS VALOR_FISCAL";
                    Mi_SQL = Mi_SQL + ", USOS_INMUEBLES." + Cat_Pat_Usos_Inmuebles.Campo_Descripcion + " AS USO_INMUEBLE";
                    Mi_SQL = Mi_SQL + ", NVL((SELECT T_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Escritura + " FROM " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + " T_JURIDICO WHERE T_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA' AND T_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID + " = BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " ), '') AS NO_ESCRITURA";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Inmuebles.Tabla_Ope_Pat_Bienes_Inmuebles + " BIENES_INMUEBLES";
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Calle_ID + " = CALLES." + Cat_Pre_Calles.Campo_Calle_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID + " = COLONIAS." + Cat_Ate_Colonias.Campo_Colonia_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTAS_PREDIAL";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Cuenta_Predial_ID + " = CUENTAS_PREDIAL." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Usos_Inmuebles.Tabla_Cat_Pat_Usos_Inmuebles + " USOS_INMUEBLES";
                    Mi_SQL = Mi_SQL + " ON BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Uso_ID + " = USOS_INMUEBLES." + Cat_Pat_Usos_Inmuebles.Campo_Uso_ID;
                    if (Parametros.P_No_Escritura != null && Parametros.P_No_Escritura.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " IN (SELECT T_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID + " FROM " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + " T_JURIDICO WHERE T_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " = 'ALTA' AND T_JURIDICO." + Ope_Pat_B_Inm_Juridico.Campo_Escritura + " LIKE '%" + Parametros.P_No_Escritura + "%')";
                    }
                    if (Parametros.P_Calle != null && Parametros.P_Calle.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Calle_ID + " = '" + Parametros.P_Calle + "'";
                    }
                    if (Parametros.P_Colonia != null && Parametros.P_Colonia.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID + " = '" + Parametros.P_Colonia + "'";
                    }
                    if (Parametros.P_Uso_ID != null && Parametros.P_Uso_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Uso_ID + " = '" + Parametros.P_Uso_ID + "'";
                    }
                    if (Parametros.P_Destino_ID != null && Parametros.P_Destino_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Destino_ID + " = '" + Parametros.P_Destino_ID + "'";
                    }
                    if (Parametros.P_Cuenta_Predial_ID != null && Parametros.P_Cuenta_Predial_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Cuenta_Predial_ID + " = '" + Parametros.P_Cuenta_Predial_ID + "'";
                    }
                    if (Parametros.P_Tipo_Predio_ID != null && Parametros.P_Tipo_Predio_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Predio_ID + " = '" + Parametros.P_Tipo_Predio_ID + "'";
                    }
                    if (Parametros.P_Bien_Inmueble_ID != null && Parametros.P_Bien_Inmueble_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " IN ('" + Parametros.P_Bien_Inmueble_ID + "')";
                    }
                    if (Parametros.P_Construccion_Desde > (-1)) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Superficie + " >= '" + Parametros.P_Construccion_Desde.ToString() + "'";
                    }
                    if (Parametros.P_Construccion_Hasta > (-1)) {
                        if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + " BIENES_INMUEBLES." + Ope_Pat_Bienes_Inmuebles.Campo_Superficie + " <= '" + (Parametros.P_Construccion_Hasta + 0.01).ToString() + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY NO_ESCRITURA";
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Datos != null && Ds_Datos.Tables.Count>0) {
                        Dt_Datos = Ds_Datos.Tables[0];
                    }
                } catch (Exception Ex) {
                    throw new Exception("Consultar_::: [" + Ex.Message + "]");
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Bien_Inmueble
            ///DESCRIPCIÓN          : Carga un los Detalles de un Bien.
            ///PARAMETROS           : Parametros. Contiene los parametros que se van a Consultar
            ///                                   en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 28/Febrero/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static Cls_Ope_Pat_Bienes_Inmuebles_Negocio Consultar_Detalles_Bien_Inmueble(Cls_Ope_Pat_Bienes_Inmuebles_Negocio Parametros) {
                String Mi_SQL = ""; 
                Cls_Ope_Pat_Bienes_Inmuebles_Negocio Bien = new Cls_Ope_Pat_Bienes_Inmuebles_Negocio();
                OracleDataReader Data_Reader;
                try {
                    Mi_SQL = "SELECT * FROM " + Ope_Pat_Bienes_Inmuebles.Tabla_Ope_Pat_Bienes_Inmuebles + " WHERE " + Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID + " = '" + Parametros.P_Bien_Inmueble_ID.Trim() + "'";
                    Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    while (Data_Reader.Read()) {
                        Bien.P_Bien_Inmueble_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Bien_Inmueble_ID].ToString() : null;
                        Bien.P_Calle = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Calle_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Calle_ID].ToString() : null;
                        Bien.P_Colonia = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Colonia_ID].ToString() : null;
                        Bien.P_Uso_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Uso_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Uso_ID].ToString() : null;
                        Bien.P_Superficie = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Superficie].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Superficie]) : (-1);
                        Bien.P_Construccion_Construida = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Construccion].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Construccion]) : (-1);
                        Bien.P_Manzana = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Manzana].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Manzana].ToString() : null;
                        Bien.P_Cuenta_Predial_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Cuenta_Predial_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Cuenta_Predial_ID].ToString() : null;
                        Bien.P_Lote = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Lote].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Lote].ToString() : null;
                        Bien.P_Ocupacion = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Porcentaje_Ocupacion].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Porcentaje_Ocupacion]) : (-1);
                        Bien.P_Efectos_Fiscales = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Efectos_Fiscales].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Efectos_Fiscales].ToString() : null;
                        Bien.P_Sector_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Sector_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Sector_ID].ToString() : null;
                        Bien.P_Clasificacion_Zona_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Clasificacion_Zona_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Clasificacion_Zona_ID].ToString() : null;
                        Bien.P_Tipo_Predio_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Predio_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Predio_ID].ToString() : null;
                        Bien.P_Vias_Acceso = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Vias_Acceso].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Vias_Acceso].ToString() : null;
                        Bien.P_Estado = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Estado].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Estado].ToString() : null;
                        Bien.P_Densidad_Construccion = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Densidad_Construccion].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Densidad_Construccion]) : (-1);
                        Bien.P_Valor_Comercial = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Valor_Comercial].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Valor_Comercial]) : (-1);
                        Bien.P_No_Exterior = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Numero_Exterior].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Numero_Exterior].ToString() : null;
                        Bien.P_No_Interior = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Numero_Interior].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Numero_Interior].ToString() : null;
                        Bien.P_Destino_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Destino_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Destino_ID].ToString() : null;
                        Bien.P_Origen_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Origen_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Origen_ID].ToString() : null;
                        Bien.P_Estatus = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Estatus].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Estatus].ToString() : null;
                        Bien.P_Fecha_Registro = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro].ToString())) ? Convert.ToDateTime(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Registro]) : new DateTime();
                        Bien.P_Area_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Area_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Area_ID].ToString() : null;

                        Bien.P_Hoja = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Hoja].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Hoja].ToString() : null;
                        Bien.P_Tomo = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Tomo].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Tomo].ToString() : null;
                        Bien.P_Numero_Acta = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Numero_Acta].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Numero_Acta].ToString() : null;
                        Bien.P_Cartilla_Parcelaria = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Cartilla_Parcelaria].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Cartilla_Parcelaria].ToString() : null;
                        Bien.P_Superficie_Contable = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Superficie_Contable].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Superficie_Contable]) : -1;
                        Bien.P_Unidad_Superficie = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Unidad_Superficie].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Unidad_Superficie].ToString() : null;
                        Bien.P_Clase_Activo_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Clase_Activo_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Clase_Activo_ID].ToString() : null;
                        Bien.P_Tipo_Bien = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Bien].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Tipo_Bien].ToString() : null;
                        Bien.P_Registro_Propiedad = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Registro_Propiedad].ToString())) ? Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Registro_Propiedad].ToString() : null;
                        
                        Bien.P_Fecha_Baja = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Baja].ToString())) ? Convert.ToDateTime(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Baja]) : new DateTime();
                        Bien.P_Fecha_Alta_Cuenta_Publica = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Alta_Cta_Pub].ToString())) ? Convert.ToDateTime(Data_Reader[Ope_Pat_Bienes_Inmuebles.Campo_Fecha_Alta_Cta_Pub]) : new DateTime();
                    }
                    Data_Reader.Close();
                    if (!String.IsNullOrEmpty(Bien.P_Bien_Inmueble_ID)) {
                        Mi_SQL = "SELECT " + Ope_Pat_B_Inm_Observaciones.Campo_No_Observacion + " AS NO_OBSERVACION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Fecha_Observacion + " AS FECHA_OBSERVACION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Usuario_Observacion_ID + " AS USUARIO_OBSERVACION_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Observacion + " AS OBSERVACION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Usuario_Creo + " AS USUARIO_CREO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Fecha_Creo + " AS FECHA_CREO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Usuario_Modifico + " AS USUARIO_MODIFICO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Observaciones.Campo_Fecha_Modifico + " AS FECHA_MODIFICO";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_B_Inm_Observaciones.Tabla_Ope_Pat_B_Inm_Observaciones;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Observaciones.Campo_Bien_Inmueble_ID + " = '" + Bien.P_Bien_Inmueble_ID + "'";
                        DataSet Ds_Tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Ds_Tmp != null && Ds_Tmp.Tables.Count > 0) {
                            Bien.P_Dt_Observaciones = Ds_Tmp.Tables[0];
                        }
                    }

                    if (!String.IsNullOrEmpty(Bien.P_Bien_Inmueble_ID)) {
                        Mi_SQL = "SELECT " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas + "." + Ope_Pat_B_Inm_Medidas.Campo_No_Registro + " AS NO_REGISTRO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas + "." + Ope_Pat_B_Inm_Medidas.Campo_Orientacion_ID + " AS ORIENTACION_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Orientaciones_Inm.Tabla_Cat_Pat_Orientaciones_Inm + "." + Cat_Pat_Orientaciones_Inm.Campo_Descripcion + " AS ORIENTACION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas + "." + Ope_Pat_B_Inm_Medidas.Campo_Medida + " AS MEDIDA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas + "." + Ope_Pat_B_Inm_Medidas.Campo_Colindancia + " AS COLINDANCIA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas + "." + Ope_Pat_B_Inm_Medidas.Campo_Usuario_Creo + " AS USUARIO_CREO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas + "." + Ope_Pat_B_Inm_Medidas.Campo_Fecha_Creo + " AS FECHA_CREO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas + "." + Ope_Pat_B_Inm_Medidas.Campo_Usuario_Modifico + " AS USUARIO_MODIFICO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas + "." + Ope_Pat_B_Inm_Medidas.Campo_Fecha_Modifico + " AS FECHA_MODIFICO";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas;
                        Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Orientaciones_Inm.Tabla_Cat_Pat_Orientaciones_Inm;
                        Mi_SQL = Mi_SQL + " ON " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas + "." + Ope_Pat_B_Inm_Medidas.Campo_Orientacion_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Orientaciones_Inm.Tabla_Cat_Pat_Orientaciones_Inm + "." + Cat_Pat_Orientaciones_Inm.Campo_Orientacion_ID;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Medidas.Tabla_Ope_Pat_B_Inm_Medidas + "." + Ope_Pat_B_Inm_Medidas.Campo_Bien_Inmueble_ID + " = '" + Bien.P_Bien_Inmueble_ID + "'";
                        DataSet Ds_Tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Ds_Tmp != null && Ds_Tmp.Tables.Count > 0) {
                            Bien.P_Dt_Medidas_Colindancias = Ds_Tmp.Tables[0];
                        }
                    }

                    if (!String.IsNullOrEmpty(Bien.P_Bien_Inmueble_ID)) {
                        Mi_SQL = "SELECT " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_No_Registro + " AS NO_REGISTRO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Escritura + " AS ESCRITURA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Escritura + " AS FECHA_ESCRITURA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_No_Notario + " AS NO_NOTARIO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Nombre_Notario + " AS NOMBRE_COMPLETO_NOTARIO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Constancia_Registral + " AS CONSTANCIA_REGISTRAL";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Folio_Real + " AS FOLIO_REAL";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Libertad_Gravament + " AS LIBERTAD_GRAVAMEN";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Antecedente_Registral + " AS ANTECEDENTE_REGISTRAL";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Proveedor + " AS PROVEEDOR";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Observaciones + " AS OBSERVACIONES";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_No_Contrato + " AS NO_CONTRATO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Usuario_Creo + " AS USUARIO_CREO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Creo + " AS FECHA_CREO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Usuario_Modifico + " AS USUARIO_MODIFICO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Fecha_Modifico + " AS FECHA_MODIFICO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Movimiento + " AS MOVIMIENTO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." + Ope_Pat_B_Inm_Juridico.Campo_Nuevo_Propietario + " AS NUEVO_PROPIETARIO";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Juridico.Tabla_Ope_Pat_B_Inm_Juridico + "." +  Ope_Pat_B_Inm_Juridico.Campo_Bien_Inmueble_ID + " = '" + Bien.P_Bien_Inmueble_ID + "'";
                        DataSet Ds_Tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Ds_Tmp != null && Ds_Tmp.Tables.Count > 0) {
                            Bien.P_Dt_Historico_Juridico = Ds_Tmp.Tables[0];
                        }
                    }

                    if (!String.IsNullOrEmpty(Bien.P_Bien_Inmueble_ID)) {
                        Mi_SQL = "SELECT " + Ope_Pat_B_Inm_Archivos.Campo_No_Registro + " AS NO_REGISTRO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Tipo_Archivo + " AS TIPO_ARCHIVO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Descripcion_Archivo + " AS DESCRIPCION_ARCHIVO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Ruta_Archivo + " AS RUTA_ARCHIVO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Usuario_Creo + " AS USUARIO_CREO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Archivos.Campo_Fecha_Cargo + " AS FECHA_CARGO";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_B_Inm_Archivos.Tabla_Ope_Pat_B_Inm_Archivos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Archivos.Campo_Bien_Inmueble_ID + " = '" + Bien.P_Bien_Inmueble_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_B_Inm_Archivos.Campo_Estatus + " = 'ACTIVO'";
                        DataSet Ds_Tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Ds_Tmp != null && Ds_Tmp.Tables.Count > 0) {
                            Bien.P_Dt_Anexos = Ds_Tmp.Tables[0];
                        }
                    }
                    if (!String.IsNullOrEmpty(Bien.P_Bien_Inmueble_ID)) {
                        Mi_SQL = "SELECT " + Ope_Pat_B_Inm_Expropiaciones.Campo_No_Expropiacion + " AS NO_EXPROPIACION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Fecha_Expropiacion + " AS FECHA_EXPROPIACION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Usuario_Expropiacion_ID + " AS USUARIO_EXPROPIACION_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Descripcion + " AS DESCRIPCION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Usuario_Creo + " AS USUARIO_CREO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Fecha_Creo + " AS FECHA_CREO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Usuario_Modifico + " AS USUARIO_MODIFICO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Expropiaciones.Campo_Fecha_Modifico + " AS FECHA_MODIFICO";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_B_Inm_Expropiaciones.Tabla_Ope_Pat_B_Inm_Expropiaciones;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Expropiaciones.Campo_Bien_Inmueble_ID + " = '" + Bien.P_Bien_Inmueble_ID + "'";
                        DataSet Ds_Tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Ds_Tmp != null && Ds_Tmp.Tables.Count > 0) {
                            Bien.P_Dt_Expropiaciones = Ds_Tmp.Tables[0];
                        }
                    }
                    if (!String.IsNullOrEmpty(Bien.P_Bien_Inmueble_ID)) {
                        Mi_SQL = "SELECT " + Ope_Pat_B_Inm_Afectaciones.Campo_No_Registro + " AS NO_REGISTRO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Fecha_Afectacion + " AS FECHA_AFECTACION";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Fecha_Registro + " AS FECHA_REGISTRO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Usuario_Registro_ID + " AS USUARIO_REGISTRO_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Propietario + " AS PROPIETARIO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Session_Ayuntamiento + " AS SESSION_AYUNTAMIENTO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Tramo + " AS TRAMO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_No_Contrato + " AS NO_CONTRATO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Usuario_Creo + " AS USUARIO_CREO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Fecha_Creo + " AS FECHA_CREO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Usuario_Modifico + " AS USUARIO_MODIFICO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_B_Inm_Afectaciones.Campo_Fecha_Modifico + " AS FECHA_MODIFICO";
                        Mi_SQL = Mi_SQL + ", 'GUARDADO' AS ESTATUS";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_B_Inm_Afectaciones.Tabla_Ope_Pat_B_Inm_Afectaciones;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_B_Inm_Afectaciones.Campo_Bien_Inmueble_ID + " = '" + Bien.P_Bien_Inmueble_ID + "'";
                        DataSet Ds_Tmp = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Ds_Tmp != null && Ds_Tmp.Tables.Count > 0) {
                            Bien.P_Dt_Afectaciones = Ds_Tmp.Tables[0];
                        }
                    }

                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los datos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Bien;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
            ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 10/Marzo/2010 
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID) {
                String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
                try {
                    String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                    Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals("")) {
                        Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                    }
                } catch (OracleException Ex) {
                    new Exception(Ex.Message);
                }
                return Id;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
            ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
            ///PARAMETROS:     
            ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
            ///             2. Longitud_ID. Longitud que tendra el ID. 
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 10/Marzo/2010 
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID) {
                String Retornar = "";
                String Dato = "" + Dato_ID;
                for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                    Retornar = Retornar + "0";
                }
                Retornar = Retornar + Dato;
                return Retornar;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_Complementos
            ///DESCRIPCIÓN: Se obtienen los complementos.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 29/Febrero/2012 
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static Cls_Ope_Pat_Bienes_Inmuebles_Negocio Obtener_Complementos(Cls_Ope_Pat_Bienes_Inmuebles_Negocio Parametros) {
                try{
                    Boolean Entro_Where = false;
                    if (Parametros.P_Tipo_Complento.Trim().Equals("COLONIA_DE_CALLE")) {
                        String Mi_SQL = "SELECT CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS CALLE_NOMBRE"
                                      + ", CALLES." + Cat_Pre_Calles.Campo_Colonia_ID + " AS COLONIA_ID"
                                      + ", COLONIAS." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA_NOMBRE"
                                      + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CALLES"
                                      + " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " COLONIAS" 
                                      + " ON CALLES." + Cat_Pre_Calles.Campo_Colonia_ID + " = COLONIAS." + Cat_Ate_Colonias.Campo_Colonia_ID
                                      + " WHERE CALLES." + Cat_Pre_Calles.Campo_Calle_ID + " = '" + Parametros.P_Calle + "'";
                        OracleDataReader Lector = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        while (Lector.Read()) {
                            Parametros.P_Colonia = Lector["COLONIA_ID"].ToString();
                            Parametros.P_Tipo_Complento = Lector["COLONIA_NOMBRE"].ToString();
                            Parametros.P_Calle = Lector["CALLE_NOMBRE"].ToString();
                        }
                        Lector.Close();
                    } else if (Parametros.P_Tipo_Complento.Trim().Equals("NOTARIOS")) {
                        String Mi_SQL = "SELECT " + Cat_Pre_Notarios.Campo_Notario_ID + " AS NOTARIO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Notarios.Campo_RFC + " AS RFC";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pre_Notarios.Campo_Numero_Notaria + " AS NO_NOTARIA";
                        Mi_SQL = Mi_SQL + ", (" + Cat_Pre_Notarios.Campo_Apellido_Paterno + "|| ' ' ||" + Cat_Pre_Notarios.Campo_Apellido_Materno + "|| ' ' ||" + Cat_Pre_Notarios.Campo_Nombre + ") AS NOMBRE_COMPLETO";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pre_Notarios.Tabla_Cat_Pre_Notarios;
                        if (Parametros.P_No_Notario != null && Parametros.P_No_Notario.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL = Mi_SQL + " AND "; } else { Mi_SQL = Mi_SQL + " WHERE "; Entro_Where=true; }
                            Mi_SQL = Mi_SQL + Cat_Pre_Notarios.Campo_Notario_ID + " = '" + Parametros.P_No_Notario + "'";
                        }
                        if (Parametros.P_Notario_Nombre != null && Parametros.P_Notario_Nombre.Trim().Length > 0) {
                            if (Entro_Where) { Mi_SQL = Mi_SQL + " AND ("; } else { Mi_SQL = Mi_SQL + " WHERE ("; Entro_Where=true; }
                            Mi_SQL = Mi_SQL + " (" + Cat_Pre_Notarios.Campo_Apellido_Paterno + "|| ' ' ||" + Cat_Pre_Notarios.Campo_Apellido_Materno + "|| ' ' ||" + Cat_Pre_Notarios.Campo_Nombre + ") LIKE '%" + Parametros.P_Notario_Nombre + "%'";
                            Mi_SQL = Mi_SQL + " OR (" + Cat_Pre_Notarios.Campo_Nombre + "|| ' ' ||" + Cat_Pre_Notarios.Campo_Apellido_Paterno + "|| ' ' ||" + Cat_Pre_Notarios.Campo_Apellido_Materno + ") LIKE '%" + Parametros.P_Notario_Nombre + "%'";
                            Mi_SQL = Mi_SQL + " OR (" + Cat_Pre_Notarios.Campo_Numero_Notaria + ") = '" + Parametros.P_Notario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ")";
                        }
                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pre_Notarios.Campo_Apellido_Paterno + ", " + Cat_Pre_Notarios.Campo_Apellido_Materno + ", " + Cat_Pre_Notarios.Campo_Nombre;
                        DataSet Ds_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Ds_Temporal != null && Ds_Temporal.Tables.Count > 0) {
                            Parametros.P_Dt_Historico_Juridico = Ds_Temporal.Tables[0];
                        }
                    }
                } catch (OracleException Ex) {
                    throw new Exception(Ex.Message);
                }
                return Parametros;  
            }

        #endregion

    }

}

