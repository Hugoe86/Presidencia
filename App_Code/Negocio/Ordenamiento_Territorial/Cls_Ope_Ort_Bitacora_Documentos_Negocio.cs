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
using Presidencia.Orden_Territorial_Bitacora_Documentos.Datos;

namespace Presidencia.Orden_Territorial_Bitacora_Documentos.Negocio
{
    public class Cls_Ope_Ort_Bitacora_Documentos_Negocio
    {
        #region Variables Internas
        private String Bitacora_ID;
        private String Detalle_Bitacora_ID;
        private String Solicitud_ID;
        private String Subproceso_ID;
        private String Documento_ID;
        private String[] Estatus;
        private String Fecha;
        private DataTable Dt_Bitacora;
        private String Clave_Solicitud;
        private String Estatus_Busqueda;
        private String Documento;
        private DateTime Dtime_Fecha_Prestamo;
        private DateTime Dtime_Fecha_Devolucion;
        private String Ubicacion;
        private String Usuario;
        private String Observaciones;
        private String Estatus_Prestamo;

        private String Pregunta_Satisfaccion;
        private String Tiempo_Espera;
        private String Nombre_Usuario;
        private String Dependencia_id;        
        #endregion

        #region Variables Publicas
        public String P_Bitacora_ID
        {
            get { return Bitacora_ID; }
            set { Bitacora_ID = value; }
        }
        public String P_Detalle_Bitacora_ID
        {
            get { return Detalle_Bitacora_ID; }
            set { Detalle_Bitacora_ID = value; }
        }
        
        public String P_Solicitud_ID
        {
            get { return Solicitud_ID; }
            set { Solicitud_ID = value; }
        }
        public String P_Subproceso_ID
        {
            get { return Subproceso_ID; }
            set { Subproceso_ID = value; }
        }
        public String P_Documento_ID
        {
            get { return Documento_ID; }
            set { Documento_ID = value; }
        }
        public String[] P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }
        public String P_Clave_Solicitud
        {
            get { return Clave_Solicitud; }
            set { Clave_Solicitud = value; }
        }
        public DataTable P_Dt_Bitacora
        {
            get { return Dt_Bitacora; }
            set { Dt_Bitacora = value; }
        }
        public String P_Estatus_Busqueda
        {
            get { return Estatus_Busqueda; }
            set { Estatus_Busqueda = value; }
        }
        public String P_Documento
        {
            get { return Documento; }
            set { Documento = value; }
        }
        public DateTime P_Dtime_Fecha_Prestamo
        {
            get { return Dtime_Fecha_Prestamo; }
            set { Dtime_Fecha_Prestamo = value; }
        }
        public DateTime P_Dtime_Fecha_Devolucion
        {
            get { return Dtime_Fecha_Devolucion; }
            set { Dtime_Fecha_Devolucion = value; }
        }
        public String P_Ubicacion
        {
            get { return Ubicacion; }
            set { Ubicacion = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }
        public String P_Estatus_Prestamo
        {
            get { return Estatus_Prestamo; }
            set { Estatus_Prestamo = value; }
        }
        public String P_Pregunta_Satisfaccion
        {
            get { return Pregunta_Satisfaccion; }
            set { Pregunta_Satisfaccion = value; }
        }
        public String P_Tiempo_Espera
        {
            get { return Tiempo_Espera; }
            set { Tiempo_Espera = value; }
        }
        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        public String P_Dependencia_id
        {
            get { return Dependencia_id; }
            set { Dependencia_id = value; }
        }
        #endregion

        #region Consultas
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Solicitudes
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Solicitudes()
        {
            return Cls_Ope_Ort_Bitacora_Documentos_Datos.Consultar_Solicitudes(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Solicitudes
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Solicitudes_Filtros()
        {
            return Cls_Ope_Ort_Bitacora_Documentos_Datos.Consultar_Solicitudes_Filtros(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Solicitudes
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Bitacora()
        {
            return Cls_Ope_Ort_Bitacora_Documentos_Datos.Consultar_Bitacora(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Bitacora_Consecutivo_ID
        ///DESCRIPCIÓN          : Metodo para consultar el id de la bitacora
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012 
        ///*********************************************************************************************************
        public String Consultar_Bitacora_Consecutivo_ID()
        {
            return Cls_Ope_Ort_Bitacora_Documentos_Datos.Consultar_Bitacora_Consecutivo_ID(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Documentos
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Documentos()
        {
            return Cls_Ope_Ort_Bitacora_Documentos_Datos.Consultar_Documentos(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Documentos
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Documentos_Prestados_Devueltos()
        {
            return Cls_Ope_Ort_Bitacora_Documentos_Datos.Consultar_Documentos_Prestados_Devueltos(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Documentos
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 29/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Personal()
        {
            return Cls_Ope_Ort_Bitacora_Documentos_Datos.Consultar_Personal(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Informacion_Reporte_Bitacora
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 31/Julio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Informacion_Reporte_Bitacora()
        {
            return Cls_Ope_Ort_Bitacora_Documentos_Datos.Consultar_Informacion_Reporte_Bitacora(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Documentos_Repetidos
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Documentos_Repetidos()
        {
            return Cls_Ope_Ort_Bitacora_Documentos_Datos.Consultar_Documentos_Repetidos(this);
        }  
        #endregion

        #region Alta-Modificacion
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta
        ///DESCRIPCIÓN          : Metodo para guardar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Alta()
        {
            return Cls_Ope_Ort_Bitacora_Documentos_Datos.Alta(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Detalle
        ///DESCRIPCIÓN          : Metodo para guardar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 27/Junio/2012 
        ///*********************************************************************************************************
        public String Alta_Detalle()
        {
            return Cls_Ope_Ort_Bitacora_Documentos_Datos.Alta_Detalle(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Estatus_Prestamo
        ///DESCRIPCIÓN          : Metodo para modificara los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 28/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Modificar_Estatus_Prestamo()
        {
            return Cls_Ope_Ort_Bitacora_Documentos_Datos.Modificar_Estatus_Prestamo(this);
        }
        #endregion
    }
}
