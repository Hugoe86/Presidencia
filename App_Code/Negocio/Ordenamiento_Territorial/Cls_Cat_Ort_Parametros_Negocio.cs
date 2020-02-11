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
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Datos;

namespace Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio
{
    public class Cls_Cat_Ort_Parametros_Negocio
    {

        #region Variables Privadas
        private String Dependencia_ID_Ordenamiento;
        private String Dependencia_ID_Ambiental;
        private String Dependencia_ID_Urbanistico;
        private String Dependencia_ID_Inmobiliario;
        private String Dependencia_ID_Catastro;
        private String Rol_Director_Ordenamiento;
        private String Rol_Director_Ambiental;
        private String Rol_Director_Fraccionamientos;
        private String Rol_Director_Urbana;
        private String Rol_Inspector_Ordenamiento;
        private String Usuario;
        private String Costo_Bitacora;
        private String Costo_Perito;
        #endregion

        #region Variables publicas

        public String P_Dependencia_ID_Ordenamiento
        {
            get { return Dependencia_ID_Ordenamiento; }
            set { Dependencia_ID_Ordenamiento = value; }
        }
        public String P_Dependencia_ID_Ambiental
        {
            get { return Dependencia_ID_Ambiental; }
            set { Dependencia_ID_Ambiental = value; }
        }
        public String P_Dependencia_ID_Urbanistico
        {
            get { return Dependencia_ID_Urbanistico; }
            set { Dependencia_ID_Urbanistico = value; }
        }
        public String P_Dependencia_ID_Inmobiliario
        {
            get { return Dependencia_ID_Inmobiliario; }
            set { Dependencia_ID_Inmobiliario = value; }
        }
        public String P_Dependencia_ID_Catastro
        {
            get { return Dependencia_ID_Catastro; }
            set { Dependencia_ID_Catastro = value; }
        }
        public String P_Rol_Director_Ordenamiento
        {
            get { return Rol_Director_Ordenamiento; }
            set { Rol_Director_Ordenamiento = value; }
        }
        public String P_Rol_Director_Ambiental
        {
            get { return Rol_Director_Ambiental; }
            set { Rol_Director_Ambiental = value; }
        }
        public String P_Rol_Director_Fraccionamientos
        {
            get { return Rol_Director_Fraccionamientos; }
            set { Rol_Director_Fraccionamientos = value; }
        }
        public String P_Rol_Director_Urbana
        {
            get { return Rol_Director_Urbana; }
            set { Rol_Director_Urbana = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        public String P_Rol_Inspector_Ordenamiento
        {
            get { return Rol_Inspector_Ordenamiento; }
            set { Rol_Inspector_Ordenamiento = value; }
        }
        public String P_Costo_Bitacora
        {
            get { return Costo_Bitacora; }
            set { Costo_Bitacora = value; }
        }
        public String P_Costo_Perito
        {
            get { return Costo_Perito; }
            set { Costo_Perito = value; }
        }
        #endregion


        #region Metodos
        public int Actualizar_Parametros()
        {
            return Cls_Cat_Ort_Parametros_Datos.Actualizar_Parametros(this);
        }
        public DataTable Consultar_Parametros()
        {
            return Cls_Cat_Ort_Parametros_Datos.Consultar_Parametros(this);
        }
        public DataTable Consultar_Rol()
        {
            return Cls_Cat_Ort_Parametros_Datos.Consultar_Rol(this);
        }
        #endregion
    }
}
