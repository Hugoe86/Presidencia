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
using Presidencia.Tipo_Solicitud_Pagos.Datos;

namespace Presidencia.Tipo_Solicitud_Pagos.Negocios
{
    public class Cls_Cat_Con_Tipo_Solicitud_Pagos_Negocio
    {
        #region(Variables Internas)
            private String Tipo_Solicitud_Pago_ID;
            private String Descripcion;
            private String Estatus;
            private String Comentarios;
            private String Nombre_Usuario;            
        #endregion
        #region(Variables Publicas)
            public String P_Tipo_Solicitud_Pago_ID
            {
                get { return Tipo_Solicitud_Pago_ID; }
                set { Tipo_Solicitud_Pago_ID = value; }
            }
            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
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
        #endregion
        #region(Metodos)
            public void Alta_Tipo_Solicitud_Pago()
            {
                Cls_Cat_Con_Tipo_Solicitud_Pagos_Datos.Alta_Tipo_Solicitud_Pago(this);
            }
            public void Modificar_Tipo_Solicitud_Pago()
            {
                Cls_Cat_Con_Tipo_Solicitud_Pagos_Datos.Modificar_Tipo_Solicitud_Pago(this);
            }
            public void Eliminar_Tipo_Solicitud_Pago()
            {
                Cls_Cat_Con_Tipo_Solicitud_Pagos_Datos.Eliminar_Tipo_Solicitud_Pago(this);
            }
            public DataTable Consulta_Tipo_Solicitud_Pagos()
            {
                return Cls_Cat_Con_Tipo_Solicitud_Pagos_Datos.Consulta_Tipo_Solicitud_Pagos(this);
            }
            public DataTable Consulta_Tipo_Solicitud_Pagos_Combo()
            {
                return Cls_Cat_Con_Tipo_Solicitud_Pagos_Datos.Consulta_Tipo_Solicitud_Pagos_Combo(this);
            }
        #endregion
    }
}