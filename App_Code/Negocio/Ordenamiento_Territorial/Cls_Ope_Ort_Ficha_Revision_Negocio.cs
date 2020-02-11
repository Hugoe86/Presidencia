using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Ordenamiento_Territorial_Ficha_Revision.Datos;

/// <summary>
/// Summary description for Cls_Ope_Ort_Ficha_Revision_Negocio
/// </summary>
namespace Presidencia.Ordenamiento_Territorial_Ficha_Revision.Negocio
{
    public class Cls_Ope_Ort_Ficha_Revision_Negocio
    {
        #region Variables Internas

            private String Solicitud_Interna_ID = String.Empty;
            private String Solicitud_ID = String.Empty;
            private String Zona_ID = String.Empty;
            private String Area_ID = String.Empty;
            private String Observacion = String.Empty;
            private String Respuesta = String.Empty;
            private DateTime Fecha_Solicitud = new DateTime();
            private DateTime Fecha_Respuesta = new DateTime();
            private String Usuario_Creo = String.Empty;
            private DateTime Fecha_Creo = new DateTime();
            private String Usuario_Modifico = String.Empty;
            private DateTime Fecha_Modifico = new DateTime();

        #endregion

        #region Variables Publicas

            public String P_Solicitud_Interna_ID
            {
                get { return Solicitud_Interna_ID; }
                set { Solicitud_Interna_ID = value; }
            }
            public String P_Solicitud_ID
            {
                get { return Solicitud_ID; }
                set { Solicitud_ID = value; }
            }
            public String P_Zona_ID
            {
                get { return Zona_ID; }
                set { Zona_ID = value; }
            }
            public String P_Area_ID
            {
                get { return Area_ID; }
                set { Area_ID = value; }
            }
            public String P_Observacion
            {
                get { return Observacion; }
                set { Observacion = value; }
            }
            public String P_Respuesta
            {
                get { return Respuesta; }
                set { Respuesta = value; }
            }
            public DateTime P_Fecha_Solicitud
            {
                get { return Fecha_Solicitud; }
                set { Fecha_Solicitud = value; }
            }
            public DateTime P_Fecha_Respuesta
            {
                get { return Fecha_Respuesta; }
                set { Fecha_Respuesta = value; }
            }
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
            public DateTime P_Fecha_Creo
            {
                get { return Fecha_Creo; }
                set { Fecha_Creo = value; }
            }
            public String P_Usuario_Modifico
            {
                get { return Usuario_Modifico; }
                set { Usuario_Modifico = value; }
            }
            public DateTime P_Fecha_Modifico
            {
                get { return Fecha_Modifico; }
                set { Fecha_Modifico = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Ficha_Revision()
            {
                Cls_Ope_Ort_Ficha_Revision_Datos.Alta_Ficha_Revision(this);
            }

            public void Modificar_Ficha_Revision()
            {
                Cls_Ope_Ort_Ficha_Revision_Datos.Modificar_Ficha_Revision(this);
            }

            public System.Data.DataTable Consultar_Tabla_Ficha_Revision()
            {
                return Cls_Ope_Ort_Ficha_Revision_Datos.Consultar_Tabla_Ficha_Revision(this);
            }

            public Cls_Ope_Ort_Ficha_Revision_Negocio Consultar_Ficha_Revision()
            {
                return Cls_Ope_Ort_Ficha_Revision_Datos.Consultar_Ficha_Revision(this);
            }

            public void Eliminar_Ficha_Revision()
            {
                Cls_Ope_Ort_Ficha_Revision_Datos.Eliminar_Ficha_Revision(this);
            }

            public void Respuesta_Ficha_Revision()
            {
                Cls_Ope_Ort_Ficha_Revision_Datos.Respuesta_Ficha_Revision(this);
            }

        #endregion
    }
}