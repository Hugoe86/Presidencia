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
using Presidencia.Control_Patrimonial_Operacion_Siniestros.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pat_Com_Siniestros_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Operacion_Siniestros.Negocio {

    public class Cls_Ope_Pat_Com_Siniestros_Negocio {

        #region Variables Internas

            private String Siniestro_ID = null;
            private String Tipo_Siniestro_ID = null;
            private String Bien_ID = null;
            private String Aseguradora_ID = null;
            private String Tipo_Bien = null;
            private String Descripcion = null;
            private DateTime Fecha;
            private String Parte_Averiguacion = null;
            private String Reparacion = null;
            private String Estatus = null;
            private Boolean Responsable_Municipio = false;
            private String Observacion = null;
            private String Tipo_Actualizacion = null;
            private String Usuario_ID = null;
            private String Usuario_Nombre = null;
            private String Tipo_DataTable = null;
            private DataTable Dt_Observaciones = null;
            private Int32 Numero_Inventario = (-1);
            private Int32 Clave_Sistema = (-1);
            private String Dependencia = null;
            private Boolean Buscar_Fecha = false;
            private String No_Reporte = null;
            private Boolean Consignado = false;
            private Boolean Pago_Danos_Sindicos = false;
             
        #endregion

        #region Variables Publicas

            public String P_Siniestro_ID
            {
                get { return Siniestro_ID; }
                set { Siniestro_ID = value; }
            }
            public String P_Tipo_Siniestro_ID
            {
                get { return Tipo_Siniestro_ID; }
                set { Tipo_Siniestro_ID = value; }
            }
            public String P_Bien_ID
            {
                get { return Bien_ID; }
                set { Bien_ID = value; }
            }
            public String P_Aseguradora_ID
            {
                get { return Aseguradora_ID; }
                set { Aseguradora_ID = value; }
            }
            public String P_Tipo_Bien
            {
                get { return Tipo_Bien; }
                set { Tipo_Bien = value; }
            }
            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
            public DateTime P_Fecha
            {
                get { return Fecha; }
                set { Fecha = value; }
            }
            public String P_Parte_Averiguacion
            {
                get { return Parte_Averiguacion; }
                set { Parte_Averiguacion = value; }
            }
            public String P_Reparacion
            {
                get { return Reparacion; }
                set { Reparacion = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public Boolean P_Responsable_Municipio
            {
                get { return Responsable_Municipio; }
                set { Responsable_Municipio = value; }
            }
            public String P_Observacion
            {
                get { return Observacion; }
                set { Observacion = value; }
            }
            public String P_Tipo_Actualizacion
            {
                get { return Tipo_Actualizacion; }
                set { Tipo_Actualizacion = value; }
            }
            public String P_Usuario_Nombre
            {
                get { return Usuario_Nombre; }
                set { Usuario_Nombre = value; }
            }
            public String P_Usuario_ID
            {
                get { return Usuario_ID; }
                set { Usuario_ID = value; }
            }
            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }
            public DataTable P_Dt_Observaciones
            {
                get { return Dt_Observaciones; }
                set { Dt_Observaciones = value; }
            }
            public Int32 P_Numero_Inventario
            {
                get { return Numero_Inventario; }
                set { Numero_Inventario = value; }
            }
            public Int32 P_Clave_Sistema
            {
                get { return Clave_Sistema; }
                set { Clave_Sistema = value; }
            }
            public String P_Dependencia
            {
                get { return Dependencia; }
                set { Dependencia = value; }
            }
            public Boolean P_Buscar_Fecha
            {
                get { return Buscar_Fecha; }
                set { Buscar_Fecha = value; }
            }
            public String P_No_Reporte
            {
                get { return No_Reporte; }
                set { No_Reporte = value; }
            }
            public Boolean P_Consignado
            {
                get { return Consignado; }
                set { Consignado = value; }
            }
            public Boolean P_Pago_Danos_Sindicos
            {
                get { return Pago_Danos_Sindicos; }
                set { Pago_Danos_Sindicos = value; }
            }

        #endregion

        #region Metodos

            public Cls_Ope_Pat_Com_Siniestros_Negocio Alta_Siniestro() {
                return Cls_Ope_Pat_Com_Siniestros_Datos.Alta_Siniestro(this);
            }

            public void Modificar_Siniestro() {
                Cls_Ope_Pat_Com_Siniestros_Datos.Modificar_Siniestro(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Ope_Pat_Com_Siniestros_Datos.Consultar_DataTable(this);
            }

            public Cls_Ope_Pat_Com_Siniestros_Negocio Consultar_Datos_Siniestro() {
                return Cls_Ope_Pat_Com_Siniestros_Datos.Consultar_Datos_Siniestro(this);
            }

        #endregion

    }

}