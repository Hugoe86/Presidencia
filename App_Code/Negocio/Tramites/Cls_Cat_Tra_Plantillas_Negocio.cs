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
using Presidencia.Plantillas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Tra_Plantillas_Negocio
/// </summary>
/// 

namespace Presidencia.Plantillas.Negocio
{
    public class Cls_Cat_Tra_Plantillas_Negocio
    {

        #region Variables Internas

        private String Plantilla_ID;
        private String Nombre;
        private String Nombre_Archivo;
        private String Campo_Buscar;
        private String Usuario;

      
        private Cls_Cat_Tra_Plantillas_Datos Plantilla_Datos;

        #endregion

        #region Variables Publicas

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Plantilla_ID
        {
            get { return Plantilla_ID; }
            set { Plantilla_ID = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Nombre_Archivo
        {
            get { return Nombre_Archivo; }
            set { Nombre_Archivo = value; }
        }


        public String P_Campo_Buscar
        {
            get { return Campo_Buscar; }
            set { Campo_Buscar = value; }
        }

        #endregion

        public Cls_Cat_Tra_Plantillas_Negocio()
        {
            Plantilla_Datos = new Cls_Cat_Tra_Plantillas_Datos();
        }

        #region Metodos


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_ID
        ///DESCRIPCIÓN: Metodo que regresa un String con el ID Max_ID de la tabla de Cat_Tra_Plantillas
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 22/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Generar_ID()
        {
            String ID = Plantilla_Datos.Consecutivo();
            return ID;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Plantillas
        ///DESCRIPCIÓN: Metodo que consulta los registros de la tabla Cat_Tra_Plantillas
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 22/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Plantillas()
        {
            DataSet Data_Set = Plantilla_Datos.Consulta_Plantillas(this);
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Plantilla
        ///DESCRIPCIÓN: Metodo que llama el metodo de Alta_Plantillas de la clase de Datos
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 22/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Alta_Plantilla()
        {
            Plantilla_Datos.Alta_Plantilla(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Plantilla
        ///DESCRIPCIÓN: Metodo que llama el metodo de Modificar_Plantillas de la clase de Datos
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 22/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        
        public void Modificar_Plantilla()
        {
            Plantilla_Datos.Modificar_Plantilla(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Plantilla
        ///DESCRIPCIÓN: Metodo que llama el metodo de Eliminar_Plantillas de la clase de Datos
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 22/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        
        public void Eliminar_Plantilla()
        {
            Plantilla_Datos.Eliminar_Plantilla(this);

        }


        #endregion
    }
}