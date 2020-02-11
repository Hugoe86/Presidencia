using System;
using System.Data;
using Presidencia.Catalogo_Atencion_Ciudadana_Parametros_Correo.Datos;

namespace Presidencia.Catalogo_Atencion_Ciudadana_Parametros_Correo.Negocio
{
    public class Cls_Cat_Ate_Parametros_Correo_Negocio
    {
        public Cls_Cat_Ate_Parametros_Correo_Negocio()
        {
        }

        #region Variables Privadas
        private String Correo_Servidor="";
        private String Correo_Puerto = "";
        private String Correo_Remitente = "";
        private String Password_Usuario_Correo = "";
        private String Correo_Cuerpo = "";
        private String Correo_Despedida = "";
        private String Correo_Firma = "";
        private String Correo_Saludo = "";
        private String Tipo_Correo = "";
        private String Usuario_Creo_Modifico = "";
        #endregion

        #region Variables publicas
        
        public String P_Correo_Servidor
        {
            get { return Correo_Servidor; }
            set { Correo_Servidor = value; }
        }
        public String P_Correo_Puerto
        {
            get { return Correo_Puerto; }
            set { Correo_Puerto = value; }
        }
        public String P_Correo_Remitente
        {
            get { return Correo_Remitente; }
            set { Correo_Remitente = value; }
        }
        public String P_Password_Usuario_Correo
        {
            get { return Password_Usuario_Correo; }
            set { Password_Usuario_Correo = value; }
        }
        public String P_Correo_Cuerpo
        {
            get { return Correo_Cuerpo; }
            set { Correo_Cuerpo = value; }
        }
        public String P_Correo_Despedida
        {
            get { return Correo_Despedida; }
            set { Correo_Despedida = value; }
        }
        public String P_Correo_Firma
        {
            get { return Correo_Firma; }
            set { Correo_Firma = value; }
        }
        public String P_Correo_Saludo
        {
            get { return Correo_Saludo; }
            set { Correo_Saludo = value; }
        }
        public String P_Tipo_Correo
        {
            get { return Tipo_Correo; }
            set { Tipo_Correo = value; }
        }
        public String P_Usuario_Creo_Modifico
        {
            get { return Usuario_Creo_Modifico; }
            set { Usuario_Creo_Modifico = value; }
        }
        #endregion


        #region Metodos
        public int Actualizar_Parametros_Correo()
        {
            return Cls_Cat_Ate_Parametros_Correo_Datos.Actualizar_Parametros_Correo(this);
        }
        public DataTable Consultar_Parametros_Correo()
        {
            return Cls_Cat_Ate_Parametros_Correo_Datos.Consultar_Parametros_Correo(this);
        }
        #endregion
    }
}
