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
using Presidencia.Ordenamiento_Territorial_Inspectores.Datos;

namespace Presidencia.Ordenamiento_Territorial_Inspectores.Negocio
{
    public class Cls_Cat_Ort_Inspectores_Negocio
    {

        #region Variables internas
        private String Inspector_ID;
        private String Nombre;
        private String Usuario;

        private String CEDULA_PROFESIONAL;
        private String TITULO;
        private String AFILIADO;

        private String CALLE_OFICINA;
        private String COLONIA_OFICINA;
        private String NUMERO_OFICINA;
        private String TELEFONO;
        private String Email;

        private String CALLE_PARTICULAR;
        private String COLONIA_PARTICULAR;
        private String NUMERO_PARTICULAR;
        private String CODIGO_POSTAL;
        private String TELEFONO_PARTICULAR;
        private String ESPECIALIDAD;

        private String DOCUMENTO_TITULO;
        private String DOCUMENTO_CEDULA;
        private String DOCUMENTO_CURRICULUM;
        private String DOCUMENTO_CONSTANCIA;
        private String DOCUMENTO_REFRENDO;
        private String DOCUMENTO_ESPECIALIDAD;
        private String Tipo_Perito;
        private String Telefono_Celular; 
        private String Comentario;
        private String Documento_Conformidad;
        private String Documento_Curso;
        #endregion

        #region Variables Publicas
        public String P_Inspector_ID
        {
            get { return Inspector_ID; }
            set { Inspector_ID = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Cedula_Profesional
        {
            get { return CEDULA_PROFESIONAL; }
            set { CEDULA_PROFESIONAL = value; }
        }
        public String P_Titulo
        {
            get { return TITULO; }
            set { TITULO = value; }
        }
        public String P_Afiliado
        {
            get { return AFILIADO; }
            set { AFILIADO = value; }
        }

        public String P_Calle_Oficina
        {
            get { return CALLE_OFICINA; }
            set { CALLE_OFICINA = value; }
        }
        public String P_Colonia_Oficina
        {
            get { return COLONIA_OFICINA; }
            set { COLONIA_OFICINA = value; }
        }
        public String P_Numero_Oficina
        {
            get { return NUMERO_OFICINA; }
            set { NUMERO_OFICINA = value; }
        }
        public String P_Telefono_Oficina
        {
            get { return TELEFONO; }
            set { TELEFONO = value; }
        }
        public String P_Email
        {
            get { return Email; }
            set { Email = value; }
        }

        public String P_Calle_Particular
        {
            get { return CALLE_PARTICULAR; }
            set { CALLE_PARTICULAR = value; }
        }
        public String P_Colonia_Particular
        {
            get { return COLONIA_PARTICULAR; }
            set { COLONIA_PARTICULAR = value; }
        }
        public String P_Numero_Particular
        {
            get { return NUMERO_PARTICULAR; }
            set { NUMERO_PARTICULAR = value; }
        }
        public String P_Codigo_Postal
        {
            get { return CODIGO_POSTAL; }
            set { CODIGO_POSTAL = value; }
        }
        public String P_Telefono_Particular
        {
            get { return TELEFONO_PARTICULAR; }
            set { TELEFONO_PARTICULAR = value; }
        }
        public String P_Especialidad
        {
            get { return ESPECIALIDAD; }
            set { ESPECIALIDAD = value; }
        }

        public String P_Documento_Titulo
        {
            get { return DOCUMENTO_TITULO; }
            set { DOCUMENTO_TITULO = value; }
        }
        public String P_Documento_Cedula
        {
            get { return DOCUMENTO_CEDULA; }
            set { DOCUMENTO_CEDULA = value; }
        }
        public String P_Documento_Curriculum
        {
            get { return DOCUMENTO_CURRICULUM; }
            set { DOCUMENTO_CURRICULUM = value; }
        }
        public String P_Documento_Constancia
        {
            get { return DOCUMENTO_CONSTANCIA; }
            set { DOCUMENTO_CONSTANCIA = value; }
        }
        public String P_Documento_Refrendo
        {
            get { return DOCUMENTO_REFRENDO; }
            set { DOCUMENTO_REFRENDO = value; }
        }
        public String P_Documento_Especialidad
        {
            get { return DOCUMENTO_ESPECIALIDAD; }
            set { DOCUMENTO_ESPECIALIDAD = value; }
        }


        public String P_Tipo_Perito
        {
            get { return Tipo_Perito; }
            set { Tipo_Perito = value; }
        }
        public String P_Telefono_Celular
        {
            get { return Telefono_Celular; }
            set { Telefono_Celular = value; }
        }
        public String P_Comentario
        {
            get { return Comentario; }
            set { Comentario = value; }
        }
        public String P_Documento_Conformidad
        {
            get { return Documento_Conformidad; }
            set { Documento_Conformidad = value; }
        }
        public String P_Documento_Curso
        {
            get { return Documento_Curso; }
            set { Documento_Curso = value; }
        }
        #endregion

        #region Consultas
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Inspectores
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Inspectores()
        {
            return Cls_Cat_Ort_Inspectores_Datos.Consultar_Inspectores(this);
        }
        #endregion

        #region Alta-Modificacion-Eliminar
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Condicion_Inmueble
        ///DESCRIPCIÓN          : Metodo para guardar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Alta()
        {
            return Cls_Cat_Ort_Inspectores_Datos.Alta(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Condicion_Inmueble
        ///DESCRIPCIÓN          : Metodo para modificar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Modificar()
        {
            return Cls_Cat_Ort_Inspectores_Datos.Modificar(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Condicion_Inmueble
        ///DESCRIPCIÓN          : Metodo para eliminar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Eliminar()
        {
            return Cls_Cat_Ort_Inspectores_Datos.Eliminar(this);
        }
        #endregion
    }
}
