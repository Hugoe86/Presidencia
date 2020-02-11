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
using Presidencia.Tramites_Formatos.Datos;

namespace Presidencia.Tramites_Formatos.Negocio
{
    public class Cls_Cat_Tra_Formatos_Negocio
    {
        #region Variables internas
        private String Formato_ID;
        private String Nombre_Formato;
        private String Usuario;
        #endregion

        #region Variables Publicas
        public String P_Formato_ID
        {
            get { return Formato_ID; }
            set { Formato_ID = value; }
        }
        public String P_Nombre_Formato
        {
            get { return Nombre_Formato; }
            set { Nombre_Formato = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        #endregion

        #region Consultas
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Formatos
        ///DESCRIPCIÓN          : Metodo para consultar los datos de los formatos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Formatos()
        {
            return Cls_Cat_Tra_Formatos_Datos.Consultar_Formatos(this);
        }
        #endregion

        #region Alta-Modificacion-Eliminar
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Guardar_Formato
        ///DESCRIPCIÓN          : Metodo para guardar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Guardar_Formato()
        {
            return Cls_Cat_Tra_Formatos_Datos.Alta_Formato(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Formato
        ///DESCRIPCIÓN          : Metodo para modificar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Modificar_Formato()
        {
            return Cls_Cat_Tra_Formatos_Datos.Modificar_Formato(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Formato
        ///DESCRIPCIÓN          : Metodo para eliminar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Eliminar_Formato()
        {
            return Cls_Cat_Tra_Formatos_Datos.Eliminar_Formato(this);
        }
        #endregion
    }
}
