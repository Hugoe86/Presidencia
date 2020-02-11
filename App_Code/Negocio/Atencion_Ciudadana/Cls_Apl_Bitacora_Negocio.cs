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
using Presidencia.Bitacora.Datos;



/// <summary>
/// Summary description for Cls_Apl_Bitacora_Negocio
/// </summary>
namespace Presidencia.Bitacora.Negocios
{
    public class Cls_Apl_Bitacora_Negocio
    {
        #region Variables Internas
        
        //Variable que contiene el string con los nombres de los catalogos seleccionados a filtrar en el reporte
        private String Catalogos;
        //Variable que contiene la accion a filtrar en el reporte
        private String Accion;
        //Variable que contiene el usuario del que se requiere el reporte 
        private String Usuario;
        //Variable que contiene las Fechas de inicio y fin del reporte solicitado
        private String Fecha;
        //Variable que indica el orden en el que se mostraran los datos del reporte
        private String Orden;
        //Variable de la calse de datos 
        private Cls_Apl_Bitacora_Datos Bitacora_Datos;

        #endregion

        #region Variables Publicas

        public String P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Accion
        {
            get { return Accion; }
            set { Accion = value; }
        }

        public String P_Catalogos
        {
            get { return Catalogos; }
            set { Catalogos = value; }
        }

        public String P_Orden
        {
            get { return Orden; }
            set { Orden = value; }
        }

        #endregion


        public Cls_Apl_Bitacora_Negocio()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Metodos
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Bitacora
        ///DESCRIPCIÓN: Manda llamar la clase de Cls_Cat_Apl_Bitacora_Datos y recibe el data_set con los  
        ///             datos solicitados para generar los reportes
        ///PARAMETROS:  1.- String Opcion es el caso de tipo de reporte solicitado por el usuario
        ///             
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consultar_Bitacora(int opcion)
        {
            Bitacora_Datos = new Cls_Apl_Bitacora_Datos();
            DataSet Data_Set = Bitacora_Datos.Consultar_Bitacora(this, opcion);                
            return Data_Set;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pagina
        ///DESCRIPCIÓN: Metodo que obtiene el dataset de la clase de negocios para llenar el dridview
        ///PARAMETROS:  1.- Grid_View es el Grid que se llenara
        ///         
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public DataSet Consultar_Pagina()
        {
            Bitacora_Datos = new Cls_Apl_Bitacora_Datos();
            DataSet Data_set = Bitacora_Datos.Consultar_Pagina();
            return Data_set;

        }//fin de Consultar_Pagina

        #endregion
    }
}
