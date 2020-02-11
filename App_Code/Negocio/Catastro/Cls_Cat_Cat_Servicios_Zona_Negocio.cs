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
using Presidencia.Catalogo_Cat_Servicios_Zona.Datos;

/// <summary>
/// Summary description for Cls_Cat_Cat_Servicios_Zona_Negocio
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Serivicios_Zona.Negocio
{
    public class Cls_Cat_Cat_Servicios_Zona_Negocio
    {

        #region Variables Internas

            private String Serivicio_Zona_ID;
            private String Servicio_Zona;
            private String Estatus;

        #endregion

        #region Variables Publicas

            public String P_Serivicio_Zona_ID
            {
                get { return Serivicio_Zona_ID; }
                set { Serivicio_Zona_ID = value; }
            }

            public String P_Servicio_Zona
            {
                get { return Servicio_Zona; }
                set { Servicio_Zona = value; }
            }

            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }

        #endregion

        #region Metodos

            public Boolean Alta_Servicio_Zona()
            {
                return Cls_Cat_Cat_Servicios_Zona_Datos.Alta_Servicios_Zona(this);
            }

            public Boolean Modificar_Servicio_Zona()
            {
                return Cls_Cat_Cat_Servicios_Zona_Datos.Modificar_Servicios_Zona(this);
            }

            public DataTable Consultar_Servicio_Zona()
            {
                return Cls_Cat_Cat_Servicios_Zona_Datos.Consultar_Servicios_Zona(this);
            }

        #endregion

    }
}