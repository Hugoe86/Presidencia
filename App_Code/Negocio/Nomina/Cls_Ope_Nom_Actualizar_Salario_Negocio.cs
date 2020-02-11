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
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Nomina_Actualizar_Salario.Datos;

namespace Presidencia.Nomina_Actualizar_Salario.Negocio {

    public class Cls_Ope_Nom_Actualizar_Salario_Negocio
    {

        #region Variables Internas

            private String No_Actualizar_Salario = null;
            private String Tipo_Nomina_ID = null;
            private String Nomina_ID = null;
            private Int32 No_Nomina;
            private DateTime Fecha_Actualizacion;
            private String Usuario = null;
            private List<Cls_Cat_Nom_Sindicatos_Negocio> Listado_Sindicatos = null;

        #endregion

        #region Variables Publicas

            public String P_No_Actualizar_Salario
            {
                get { return No_Actualizar_Salario; }
                set { No_Actualizar_Salario = value; }
            }
            public String P_Tipo_Nomina_ID
            {
                get { return Tipo_Nomina_ID; }
                set { Tipo_Nomina_ID = value; }
            }
            public String P_Nomina_ID
            {
                get { return Nomina_ID; }
                set { Nomina_ID = value; }
            }
            public Int32 P_No_Nomina
            {
                get { return No_Nomina; }
                set { No_Nomina = value; }
            }
            public DateTime P_Fecha_Actualizacion
            {
                get { return Fecha_Actualizacion; }
                set { Fecha_Actualizacion = value; }
            }
            public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
            public List<Cls_Cat_Nom_Sindicatos_Negocio> P_Listado_Sindicatos
            {
                get { return Listado_Sindicatos; }
                set { Listado_Sindicatos = value; }
            }
            
        #endregion

        #region Metodos

            public void Registrar_Actualizacion() {
                Cls_Ope_Nom_Actualizar_Salario_Datos.Registrar_Actualizacion(this);
            }

            private void Validar_Registro_Actualizacion() { 
                
            }

            public DataTable Obtener_Listado_Sindicatos() {
                return Cls_Ope_Nom_Actualizar_Salario_Datos.Obtener_Listado_Sindicatos(this);
            }

        #endregion

    }

}