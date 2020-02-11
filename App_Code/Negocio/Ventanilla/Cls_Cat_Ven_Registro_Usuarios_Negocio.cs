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
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Datos;

namespace Presidencia.Cls_Cat_Ven_Registro_Usuarios.Negocio
{
    public class Cls_Cat_Ven_Registro_Usuarios_Negocio
    {
        #region VARIABLES INTERNAS
        private String Nombre;
        private String Apellido_Paterno;
        private String Apellido_Materno;
        private String Nombre_Completo;
        private String Email;
        private String Password;
        private String Estatus;
        private String Calle;
        private String Calle_ID;
        private String Colonia;
        private String Colonia_ID;
        private String Codigo_Postal;
        private String Ciudad;
        private String Estado;
        private String Telefono_Casa;
        private String Celular;
        private String Fecha_Nacimiento;
        private String Edad;
        private String Sexo;
        private String Rfc;
        private String Curp;
        private String Fecha_Registro;
        private String Comentarios;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private String Pregunta_Secreta;
        private String Respuesta_Secreta;
        private String Ciudadano_ID;
        private String Correo_Puerto;
        private String Correo_Servidor;
        private String Correo_Notificador;
        private String Password_Correo_Not;
        private String Estado_ID;
        #endregion

        #region VARIABLES PUBLICAS

        //get y set de P_Pregunta_Secreta
        public String P_Pregunta_Secreta
        {
            get { return Pregunta_Secreta; }
            set { Pregunta_Secreta = value; }
        }
        //get y set de P_Respuesta_Secreta
        public String P_Respuesta_Secreta
        {
            get { return Respuesta_Secreta; }
            set { Respuesta_Secreta = value; }
        }
        //get y set de P_Nombre
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        //get y set de P_Apellido_Paterno
        public String P_Apellido_Paterno
        {
            get { return Apellido_Paterno; }
            set { Apellido_Paterno = value; }
        }

        //get y set de P_Apellido_Materno
        public String P_Apellido_Materno
        {
            get { return Apellido_Materno; }
            set { Apellido_Materno = value; }
        }

        //get y set de P_Nombre_Completo
        public String P_Nombre_Completo
        {
            get { return Nombre_Completo; }
            set { Nombre_Completo = value; }
        }

        //get y set de Email
        public String P_Email
        {
            get { return Email; }
            set { Email = value; }
        }

        //get y set de P_Password
        public String P_Password
        {
            get { return Password; }
            set { Password = value; }
        }

        //get y set de P_Estatus
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        //get y set de P_Calle
        public String P_Calle
        {
            get { return Calle; }
            set { Calle = value; }
        }

        //get y set de P_Colonia
        public String P_Colonia
        {
            get { return Colonia; }
            set { Colonia = value; }
        }

        //get y set de P_Calle
        public String P_Calle_ID
        {
            get { return Calle_ID; }
            set { Calle_ID = value; }
        }

        //get y set de P_Colonia
        public String P_Colonia_ID
        {
            get { return Colonia_ID; }
            set { Colonia_ID = value; }
        }

        //get y set de P_Codigo_Postal
        public String P_Codigo_Postal
        {
            get { return Codigo_Postal; }
            set { Codigo_Postal = value; }
        }

        //get y set de P_Ciudad
        public String P_Ciudad
        {
            get { return Ciudad; }
            set { Ciudad = value; }
        }

        //get y set de P_Estado
        public String P_Estado
        {
            get { return Estado; }
            set { Estado = value; }
        }

        //get y set de P_Telefono_Casa
        public String P_Telefono_Casa
        {
            get { return Telefono_Casa; }
            set { Telefono_Casa = value; }
        }

        //get y set de P_Celular
        public String P_Celular
        {
            get { return Celular; }
            set { Celular = value; }
        }

        //get y set de P_Fecha_Nacimiento
        public String P_Fecha_Nacimiento
        {
            get { return Fecha_Nacimiento; }
            set { Fecha_Nacimiento = value; }
        }

        //get y set de P_Edad
        public String P_Edad
        {
            get { return Edad; }
            set { Edad = value; }
        }

        //get y set de P_Sexo
        public String P_Sexo
        {
            get { return Sexo; }
            set { Sexo = value; }
        }

        //get y set de P_Rfc
        public String P_Rfc
        {
            get { return Rfc; }
            set { Rfc = value; }
        }

        //get y set de P_Curp
        public String P_Curp
        {
            get { return Curp; }
            set { Curp = value; }
        }

        //get y set de P_Fecha_Registro
        public String P_Fecha_Registro
        {
            get { return Fecha_Registro; }
            set { Fecha_Registro = value; }
        }

        //get y set de P_Comentarios
        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        //get y set de P_Usuario_Creo
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }

        //get y set de P_Usuario_Modifico
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }

        //get y set de P_Correo_Notificador
        public String P_Correo_Notificador
        {
            get { return Correo_Notificador; }
            set { Correo_Notificador = value; }
        }

        //get y set de P_Correo_Puerto
        public String P_Correo_Puerto
        {
            get { return Correo_Puerto; }
            set { Correo_Puerto = value; }
        }

        //get y set de P_Correo_Servidor
        public String P_Correo_Servidor
        {
            get { return Correo_Servidor; }
            set { Correo_Servidor = value; }
        }

        //get y set de P_Password_Correo_Not
        public String P_Password_Correo_Not
        {
            get { return Password_Correo_Not; }
            set { Password_Correo_Not = value; }
        }
        //get y set de Ciudadano_ID
        public String P_Ciudadano_ID
        {
            get { return Ciudadano_ID; }
            set { Ciudadano_ID = value; }
        }
         //get y set de Ciudadano_ID
        public String P_Estado_ID
        {
            get { return Estado_ID; }
            set { Estado_ID = value; }
        }
        #endregion

        #region MÉTODOS
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Guardar_Usuario
        ///DESCRIPCIÓN          : Metodo para guardar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 02/Mayo/2012 
        ///*********************************************************************************************************
        public Boolean Guardar_Usuario()
        {
            return Cls_Cat_Ven_Registro_Usuarios_Datos.Alta_Usuario(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Guardar_Usuario
        ///DESCRIPCIÓN          : Metodo para guardar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 02/Mayo/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Usuarios()
        {
            return Cls_Cat_Ven_Registro_Usuarios_Datos.Consultar_Usuario(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Enviar_Correo
        ///DESCRIPCIÓN          : Metodo para enviar correos
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 02/Mayo/2012 
        ///*********************************************************************************************************
        public void Enviar_Correo(String Email, String Password, String Nombre)
        {
            Cls_Cat_Ven_Registro_Usuarios_Datos.Enviar_Correo(Email, Password, Nombre);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Guardar_Parametros
        ///DESCRIPCIÓN          : Metodo para guardar los parametros del correo
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 03/Mayo/2012 
        ///*********************************************************************************************************
        public Boolean Guardar_Parametros()
        {
            return Cls_Cat_Ven_Registro_Usuarios_Datos.Alta_Parametros(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Actualizar_Parametros
        ///DESCRIPCIÓN          : Metodo para actualizar los parametros del correo
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 03/Mayo/2012 
        ///*********************************************************************************************************
        public Boolean Actualizar_Parametros()
        {
            return Cls_Cat_Ven_Registro_Usuarios_Datos.Actualizar_Parametros(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Parametros
        ///DESCRIPCIÓN          : Metodo para consultar los datos de los parametros
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 03/Mayo/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Parametros()
        {
            return Cls_Cat_Ven_Registro_Usuarios_Datos.Consultar_Parametros();
        }


        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Usuario_Soliucitante
        ///DESCRIPCIÓN          : Metodo para consultar los datos de los los usuarios
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 18/Mayo/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Usuario_Soliucitante()
        {
            return Cls_Cat_Ven_Registro_Usuarios_Datos.Consultar_Usuario_Soliucitante(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Colonia
        ///DESCRIPCIÓN          : Metodo para consultar los datos de las colonias
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 23/Mayo/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Colonia()
        {
            return Cls_Cat_Ven_Registro_Usuarios_Datos.Consultar_Colonia(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Calles
        ///DESCRIPCIÓN          : Metodo para consultar los datos de las calles
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 23/Mayo/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Calles()
        {
            return Cls_Cat_Ven_Registro_Usuarios_Datos.Consultar_Calles(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Estados
        ///DESCRIPCIÓN          : Metodo para consultar los datos de los estados
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 31/Mayo/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Estados()
        {
            return Cls_Cat_Ven_Registro_Usuarios_Datos.Consultar_Estados(this);
        }


        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Ciudades
        ///DESCRIPCIÓN          : Metodo para consultar los datos de las ciudades
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 31/Mayo/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Ciudades()
        {
            return Cls_Cat_Ven_Registro_Usuarios_Datos.Consultar_Ciudades(this);
        }


        #endregion
    }
}