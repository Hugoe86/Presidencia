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
using Presidencia.Movimiento_Presupuestal.Datos;

namespace Presidencia.Autorizar_Traspaso_Presupuestal.Negocio
{

    public class Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Negocio
    {
    
        #region(Variables Privadas)
            private String Numero_Solicitud;
            private String Codigo_Programatico_Origen;
            private String Codigo_Programatico_Destino;
            private String Estatus;
            private String Importe;
            private String Justificacion;
            private String Justificacion_Solicitud;
            private String Usuario_Creo;
            private String Tipo_Operacion;
            private String Comentario;
            private String Partida;
            
        #endregion

        #region(Variables Publicas)
            public String P_Numero_Solicitud
            {
            get { return Numero_Solicitud; }
            set { Numero_Solicitud = value; }
            }
            public String P_Codigo_Programatico_Origen
            {
            get { return Codigo_Programatico_Origen; }
            set { Codigo_Programatico_Origen = value; }
            }
            public String P_Codigo_Programatico_Destino
            {
            get { return Codigo_Programatico_Destino; }
            set { Codigo_Programatico_Destino = value; }
            }
            public String P_Estatus
            {
            get { return Estatus; }
            set { Estatus = value; }
            }
            public String P_Importe
            {
            get { return Importe; }
            set { Importe = value; }
            }
            public String P_Justificacion
            {
            get { return Justificacion; }
            set { Justificacion = value; }
            }
            public String P_Justificacion_Solicitud
            {
                get { return Justificacion_Solicitud; }
                set { Justificacion_Solicitud = value; }
            }
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
            public String P_Tipo_Operacion
            {
                get { return Tipo_Operacion; }
                set { Tipo_Operacion = value; }
            }
            public String P_Comentario
            {
                get { return Comentario; }
                set { Comentario = value; }
            }
            public String P_Partida
            {
                get { return Partida; }
                set { Partida = value; }
            }
        #endregion

        #region(Metodos)
            public Boolean Alta_Autorizacion_Traspaso()
            {
                return Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Datos.Alta_Autorizacion_Traspaso(this);
            }
            public Boolean Modificar_Comentario()
            {
                return Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Datos.Modificar_Comentario(this);
            }
            public Boolean Modificar_Autorizacion_Traspaso()
            {
                return Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Datos.Modificar_Autorizacion_Traspaso(this);
            }
            public Boolean Eliminar_Autorizacion_Traspaso()
            {
                return Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Datos.Eliminar_Autorizacion_Traspaso(this);
            }
            public DataTable Consulta_Autorizacion_Traspaso()
            {
                return Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Datos.Consulta_Autorizacion_Traspaso(this);
            }
            public DataTable Consulta_Datos_Partidas()
            {
                return Cls_Ope_Psp_Autorizar_Solicitud_Traspaso_Presupuestal_Datos.Consulta_Datos_Partidas(this);
            }
        
		#endregion
    
    }
}