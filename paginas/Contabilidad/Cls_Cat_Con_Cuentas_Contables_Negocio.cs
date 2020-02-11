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
using Presidencia.Cuentas_Contables.Datos;

namespace Presidencia.Cuentas_Contables.Negocio
{
    public class Cls_Cat_Con_Cuentas_Contables_Negocio
    {
        #region (Variables_Internas)
            private String Cuenta_Contable_ID;
            private String Nivel_ID;
            private String Tipo_Balance_ID;
            private String Tipo_Resultado_ID;
            private String Descripcion;
            private String Cuenta;
            private String Afectable;
            private String Comentarios;
            private String Nombre_Usuario;
            private String Partida_ID;
            private String Tipo_Cuenta;
        #endregion
        
        #region (Variables_Publicas)
            public String P_Partida_ID
            {
                get { return Partida_ID; }
                set { Partida_ID = value; }
            }
            public String P_Cuenta_Contable_ID
            {
                get { return Cuenta_Contable_ID; }
                set { Cuenta_Contable_ID = value; }
            }
            public String P_Nivel_ID
            {
                get { return Nivel_ID; }
                set { Nivel_ID = value; }
            }
            public String P_Tipo_Balance_ID
            {
                get { return Tipo_Balance_ID; }
                set { Tipo_Balance_ID = value; }
            }
            public String P_Tipo_Resultado_ID
            {
                get { return Tipo_Resultado_ID; }
                set { Tipo_Resultado_ID = value; }
            }
            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
            public String P_Afectable
            {
                get { return Afectable; }
                set { Afectable = value; }
            }
            public String P_Cuenta
            {
                get { return Cuenta; }
                set { Cuenta = value; }
            }
            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }
            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }
            public String P_Tipo_Cuenta
            {
                get { return Tipo_Cuenta; }
                set { Tipo_Cuenta = value; }
            }
        #endregion
        
        #region (Metodos)
            public void Alta_Cuenta_Contable()
            {
                Cls_Cat_Con_Cuentas_Contables_Datos.Alta_Cuenta_Contable(this);
            }
            public void Modificar_Cuenta_Contable()
            {
                Cls_Cat_Con_Cuentas_Contables_Datos.Modificar_Cuenta_Contable(this);
            }
            public void Eliminar_Cuenta_Contable()
            {
                Cls_Cat_Con_Cuentas_Contables_Datos.Eliminar_Cuenta_Contable(this);
            }
            public DataTable Consulta_Datos_Cuentas_Contables()
            {
                return Cls_Cat_Con_Cuentas_Contables_Datos.Consulta_Datos_Cuentas_Contables(this);
            }
            public DataTable Consulta_Cuentas_Contables()
            {
                return Cls_Cat_Con_Cuentas_Contables_Datos.Consulta_Cuentas_Contables(this);
            }
            public DataTable Consulta_Existencia_Cuenta_Contable()
            {
                return Cls_Cat_Con_Cuentas_Contables_Datos.Consulta_Existencia_Cuenta_Contable(this);
            }
        #endregion
    }
}