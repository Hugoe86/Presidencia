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
using Presidencia.Operacion_Predial_Traslado.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pre_Traslado_Dominio_Negocio
/// </summary>

namespace Presidencia.Operacion_Predial_Traslado.Negocio
{

    public class Cls_Ope_Pre_Traslado_Negocio {

        #region Varibles Internas

            private String Cuenta_Predial_ID = null;
            public string Orden_Variacion_ID = null;
            public Int16 Contrarecibo_Anio = 0;
            public Int32 Año = 0;
            //private String Cuenta_Predial = null;
            private String No_Contrarecibo = null;
            private String Estatus = null;
            private String Estatus_Anterior = null;            
            private Boolean Buscar_Estatus = false;            
            private DateTime Fecha_Escritura;
            private Boolean Buscar_Fecha_Escritura = false;
            private string No_Escritura;            
            private Boolean Buscar_No_Escritura = false;            
            private DateTime Fecha_Liberacion;
            private Boolean Buscar_Fecha_Liberacion = false;
            private String Listado_ID = null;
            private DateTime Fecha_Generacion;
            private Boolean Buscar_Fecha_Generacion = false;
            private String Notario_ID = null;
            private String Tipo_DataTable = null;
            private String Usuario = null;
            private Boolean Con_Cuenta_Predial = false;
            private Boolean Contrarecibos_Sin_Calculo = false;
            private String Traslado = null;

        #endregion

        #region Varibles Publicas
            public Int32 P_Año
            {
                get { return Año; }
                set { Año = value; }
            }
            public Int16 P_Contrarecibo_Anio
            {
                get { return Contrarecibo_Anio; }
                set { Contrarecibo_Anio = value; }
            }
            public String P_Orden_Variacion_ID
            {
                get { return Orden_Variacion_ID; }
                set { Orden_Variacion_ID = value; }
            }
            public String P_Traslado
            {
                get { return Traslado; }
                set { Traslado = value; }
            }
            public string P_No_Escritura
            {
                get { return No_Escritura; }
                set { No_Escritura = value; }
            }
            public Boolean P_Buscar_No_Escritura
            {
                get { return Buscar_No_Escritura; }
                set { Buscar_No_Escritura = value; }
            }
            public Boolean P_Buscar_Estatus
            {
                get { return Buscar_Estatus; }
                set { Buscar_Estatus = value; }
            }
            public String P_Estatus_Anterior
            {
                get { return Estatus_Anterior; }
                set { Estatus_Anterior = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Cuenta_Predial_ID
            {
                get { return Cuenta_Predial_ID; }
                set { Cuenta_Predial_ID = value; }
            }

            //public String P_Cuenta_Predial
            //{
            //    get { return Cuenta_Predial; }
            //    set { Cuenta_Predial = value; }
            //}

            public String P_No_Contrarecibo
            {
                get { return No_Contrarecibo; }
                set { No_Contrarecibo = value; }
            }

            public DateTime P_Fecha_Escritura
            {
                get { return Fecha_Escritura; }
                set { Fecha_Escritura = value; }
            }

            public Boolean P_Buscar_Fecha_Escritura
            {
                get { return Buscar_Fecha_Escritura; }
                set { Buscar_Fecha_Escritura = value; }
            }

            public DateTime P_Fecha_Liberacion
            {
                get { return Fecha_Liberacion; }
                set { Fecha_Liberacion = value; }
            }

            public Boolean P_Buscar_Fecha_Liberacion
            {
                get { return Buscar_Fecha_Liberacion; }
                set { Buscar_Fecha_Liberacion = value; }
            }           

            public String P_Listado_ID
            {
                get { return Listado_ID; }
                set { Listado_ID = value; }
            }

            public DateTime P_Fecha_Generacion
            {
                get { return Fecha_Generacion; }
                set { Fecha_Generacion = value; }
            }

            public Boolean P_Buscar_Fecha_Generacion
            {
                get { return Buscar_Fecha_Generacion; }
                set { Buscar_Fecha_Generacion = value; }
            }

            public String P_Notario_ID
            {
                get { return Notario_ID; }
                set { Notario_ID = value; }
            }

            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public Boolean P_Con_Cuenta_Predial
            {
                get { return Con_Cuenta_Predial; }
                set { Con_Cuenta_Predial = value; }
            }

            public Boolean P_Contrarecibos_Sin_Calculo
            {
                get { return Contrarecibos_Sin_Calculo; }
                set { Contrarecibos_Sin_Calculo = value; }
            }

        #endregion

        #region Metodos

            public DataTable Consultar_DataTable() {
                return Cls_Ope_Pre_Traslado_Datos.Consultar_DataTable(this);
            }

            public void Modificar_Contrarecibo() {
                Cls_Ope_Pre_Traslado_Datos.Modificar_Contrarecibo(this);
            }

            public void Modificar_Contrarecibo_Estatus()
            {
                Cls_Ope_Pre_Traslado_Datos.Modificar_Contrarecibo_Estatus(this);
            }
  
            public string Consultar_Recepcion(String No_Contrarecibo)
            {
                return Cls_Ope_Pre_Traslado_Datos.Consultar_Recepcion(No_Contrarecibo);
            }
        #endregion
            
    }
}   