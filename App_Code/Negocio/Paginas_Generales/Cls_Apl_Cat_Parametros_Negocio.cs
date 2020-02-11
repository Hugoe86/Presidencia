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
using Presidencia.Parametros.Datos;

/// <summary>
/// Summary description for Cls_Apl_Cat_Parametros
/// </summary>

namespace Presidencia.Parametros.Negocios
{
    public class Cls_Apl_Cat_Parametros_Negocio
    {

        #region Variables Internas 
        
        private String Correo_Saliente;
        private String Servidor_Correo;
        private String Usuario_Correo;
        private String Password_Correo;
        //variable que indica el usuario logueado
        private String Usuario;
        //variable de la clase de Datos
        private Cls_Apl_Cat_Parametros_Datos Datos_Parametros;
        
        #endregion

        #region Variables Publicas

        public String P_Servidor_Correo
        {
            get { return Servidor_Correo; }
            set { Servidor_Correo = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Usuario_Correo
        {
            get { return Usuario_Correo; }
            set { Usuario_Correo = value; }
        }
        

        public String P_Password_Correo
        {
            get { return Password_Correo; }
            set { Password_Correo = value; }
        }


        public String P_Correo_Saliente
        {
            get { return Correo_Saliente; }
            set { Correo_Saliente = value; }
        }
        #endregion

        #region Metodos
        public Cls_Apl_Cat_Parametros_Negocio()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Parametros
        ///DESCRIPCIÓN: 
        ///PARAMETROS: Metodo que manda llamar el metodo de Modificar en la clase de datos
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 07/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public void Modificar_Parametros()
        {
            Datos_Parametros = new Cls_Apl_Cat_Parametros_Datos();
            Datos_Parametros.Modificar_Paramentros(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Parametros
        ///DESCRIPCIÓN: 
        ///PARAMETROS: Metodo que manda llamar el metodo de Consultar en la clase de datos
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 07/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public DataSet Consulta_Parametros()
        {
            Datos_Parametros = new Cls_Apl_Cat_Parametros_Datos();
            DataSet Data_Set = Datos_Parametros.Consulta_Parametros();
            return Data_Set;
        }
        #endregion

    }//fin del class
}