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
using Presidencia.Catalogo_Bloqueo_Cuentas_Predial.Datos;
/// <summary>
/// Summary description for Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio
/// </summary>

namespace Presidencia.Catalogo_Bloqueo_Cuentas_Predial.Negocio
{
    public class Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio
    {

        #region Variables Internas

            private String Bloque_Cuenta_Predial_ID;
            private String Cuenta_Predial;
            private String Estatus;
            private String Tipo_Bloqueo;
            private String Usuario;

        #endregion

        #region Variables Publicas

            public String P_Bloque_Cuenta_Predial_ID
            {
                get { return Bloque_Cuenta_Predial_ID; }
                set { Bloque_Cuenta_Predial_ID = value; }
            }

            public String P_Cuenta_Predial
            {
                get { return Cuenta_Predial; }
                set { Cuenta_Predial = value; }
            }

            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }

            public String P_Tipo_Bloqueo
            {
                get { return Tipo_Bloqueo; }
                set { Tipo_Bloqueo = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Bloqueo_Cuentas_Predial()
            {
                Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Datos.Alta_Bloqueo_Cuentas_Predial(this);
            }

            public void Modificar_Bloqueo_Cuentas_Predial()
            {
                Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Datos.Modificar_Bloqueo_Cuentas_Predial(this);
            }

            public void Eliminar_Bloqueo_Cuentas_Predial()
            {
                Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Datos.Eliminar_Bloqueo_Cuentas_Predial(this);
            }

            public Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Negocio Consultar_Datos_Bloqueo_Cuentas_Predial()
            {
                return Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Datos.Consultar_Datos_Bloqueo_Cuentas_Predial(this);
            }

            public DataTable Consultar_Bloqueo_Cuentas_Predial()
            {
                return Cls_Cat_Pre_Bloqueo_Cuentas_Predial_Datos.Consultar_Bloqueo_Cuentas_Predial(this);
            }

        #endregion

    }
}