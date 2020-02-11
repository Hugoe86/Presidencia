using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Ordenamiento_Territorial_Ficha_Revision_Depto.Datos;

/// <summary>
/// Summary description for Cls_Ope_Ort_Ficha_Revision_Depto_Negocio
/// </summary>
namespace Presidencia.Ordenamiento_Territorial_Ficha_Revision_Depto.Negocio
{
    public class Cls_Ope_Ort_Ficha_Revision_Depto_Negocio
    {
        #region Variables Internas

            private String Ficha_Revision_ID = String.Empty;
            private String Tipo_Tramite = String.Empty;
            private String Nombre_Propietario = String.Empty;
            private String Calle_Ubicacion = String.Empty;
            private String Colonia_Ubicacion = String.Empty;
            private String Codigo_Postal = String.Empty;
            private String Ciudad_Ubicacion = String.Empty;
            private String Estado_Ubicacion = String.Empty;
            private String Documentos_Propiedad = String.Empty;
            private String Observacion_Juridica = String.Empty;
            private String Observacion_Tecnica = String.Empty;
            private String Avance_Obra = String.Empty;
            private String Documentos_Dictamen = String.Empty;
            private String Cumplimiento_Norma = String.Empty;
            private String Ubicacion_Construccion = String.Empty;
            private String Tramite = String.Empty;
            private String Solicitud_ID = String.Empty;
            private DateTime Inicio_Permiso = new DateTime();
            private DateTime Fin_Permiso = new DateTime();
            private String Perito = String.Empty;
            private String Usuario_Creo = String.Empty;
            private DateTime Fecha_Creo = new DateTime();
            private String Usuario_Modifico = String.Empty;
            private DateTime Fecha_Modifico = new DateTime();

        #endregion

        #region Variables Publicas

            public String P_Ficha_Revision_ID
            {
                get { return Ficha_Revision_ID; }
                set { Ficha_Revision_ID = value; }
            }
            public String P_Tipo_Tramite
            {
                get { return Tipo_Tramite; }
                set { Tipo_Tramite = value; }
            }
            public String P_Nombre_Propietario
            {
                get { return Nombre_Propietario; }
                set { Nombre_Propietario = value; }
            }
            public String P_Calle_Ubicacion
            {
                get { return Calle_Ubicacion; }
                set { Calle_Ubicacion = value; }
            }
            public String P_Colonia_Ubicacion
            {
                get { return Colonia_Ubicacion; }
                set { Colonia_Ubicacion = value; }
            }
            public String P_Codigo_Postal
            {
                get { return Codigo_Postal; }
                set { Codigo_Postal = value; }
            }
            public String P_Ciudad_Ubicacion
            {
                get { return Ciudad_Ubicacion; }
                set { Ciudad_Ubicacion = value; }
            }
            public String P_Estado_Ubicacion
            {
                get { return Estado_Ubicacion; }
                set { Estado_Ubicacion = value; }
            }
            public String P_Documentos_Propiedad
            {
                get { return Documentos_Propiedad; }
                set { Documentos_Propiedad = value; }
            }
            public String P_Observacion_Juridica
            {
                get { return Observacion_Juridica; }
                set { Observacion_Juridica = value; }
            }
            public String P_Observacion_Tecnica
            {
                get { return Observacion_Tecnica; }
                set { Observacion_Tecnica = value; }
            }
            public String P_Avance_Obra
            {
                get { return Avance_Obra; }
                set { Avance_Obra = value; }
            }
            public String P_Documentos_Dictamen
            {
                get { return Documentos_Dictamen; }
                set { Documentos_Dictamen = value; }
            }
            public String P_Cumplimiento_Norma
            {
                get { return Cumplimiento_Norma; }
                set { Cumplimiento_Norma = value; }
            }
            public String P_Ubicacion_Construccion
            {
                get { return Ubicacion_Construccion; }
                set { Ubicacion_Construccion = value; }
            }
            public String P_Tramite
            {
                get { return Tramite; }
                set { Tramite = value; }
            }
            public String P_Solicitud_ID
            {
                get { return Solicitud_ID; }
                set { Solicitud_ID = value; }
            }
            public DateTime P_Inicio_Permiso
            {
                get { return Inicio_Permiso; }
                set { Inicio_Permiso = value; }
            }
            public DateTime P_Fin_Permiso
            {
                get { return Fin_Permiso; }
                set { Fin_Permiso = value; }
            }
            public String P_Perito
            {
                get { return Perito; }
                set { Perito = value; }
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

            public void Alta_Ficha_Revision_Depto()
            {
                Cls_Ope_Ort_Ficha_Revision_Depto_Datos.Alta_Ficha_Revision_Depto(this);
            }

            public void Modificar_Ficha_Revision_Depto()
            {
                Cls_Ope_Ort_Ficha_Revision_Depto_Datos.Modificar_Ficha_Revision_Depto(this);
            }

            public System.Data.DataTable Consultar_Tabla_Ficha_Revision_Depto()
            {
                return Cls_Ope_Ort_Ficha_Revision_Depto_Datos.Consultar_Tabla_Ficha_Revision_Depto(this);
            }

            public Cls_Ope_Ort_Ficha_Revision_Depto_Negocio Consultar_Ficha_Revision_Depto()
            {
                return Cls_Ope_Ort_Ficha_Revision_Depto_Datos.Consultar_Ficha_Revision_Depto(this);
            }

            public void Eliminar_Ficha_Revision_Depto()
            {
                Cls_Ope_Ort_Ficha_Revision_Depto_Datos.Eliminar_Ficha_Revision_Depto(this);
            }

        #endregion
    }
}