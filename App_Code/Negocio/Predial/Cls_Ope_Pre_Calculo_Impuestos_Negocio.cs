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
using Presidencia.Operacion_Predial_Calculo_Impuestos.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pre_Calculo_Impuestos_Negocio
/// </summary>

namespace Presidencia.Operacion_Predial_Calculo_Impuestos.Negocio
{

    public class Cls_Ope_Pre_Calculo_Impuestos_Negocio {

        #region Varibles Internas

            private String Cuenta_Predial = null;
            private String No_Contrarecibo = null;
            private DateTime Fecha_Escritura;
            private Boolean Buscar_Fecha_Escritura = false;
            private DateTime Fecha_Liberacion;
            private Boolean Buscar_Fecha_Liberacion = false;
            private String Listado_ID = null;
            private DateTime Fecha_Generacion;
            private Boolean Buscar_Fecha_Generacion = false;
            private String Notario_ID = null;
            private String Tipo_DataTable = null;
            private String Usuario = null;

        #endregion

        #region Varibles Publicas

            public String P_Cuenta_Predial
            {
                get { return Cuenta_Predial; }
                set { Cuenta_Predial = value; }
            }

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

        #endregion

        #region Metodos

            public DataTable Consultar_DataTable() {
                return Cls_Ope_Pre_Calculo_Impuestos_Datos.Consultar_DataTable(this);
            }

            public void Modificar_Contrarecibo() {
                Cls_Ope_Pre_Calculo_Impuestos_Datos.Modificar_Contrarecibo(this);
            }
        
        #endregion

    }
}   