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
using Presidencia.Ordenamiento_Territorial_Tipo_Supervision.Datos;


namespace Presidencia.Ordenamiento_Territorial_Tipo_Supervision.Negocio
{
    public class Cls_Cat_Ort_Tipo_Supervision_Negocio
    {
        #region Variables internas
        private String Tipo_Supervision_ID;
        private String Nombre;
        private String Usuario;
        #endregion

        #region Variables Publicas
        public String P_Tipo_Supervision_ID
        {
            get { return Tipo_Supervision_ID; }
            set { Tipo_Supervision_ID = value; }
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
        #endregion

        #region Consultas
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tipo_Supervision
        ///DESCRIPCIÓN          : Metodo para consultar los datos de los tipos de supervision
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Tipo_Supervision()
        {
            return Cls_Cat_Ort_Tipo_Supervision_Datos.Consultar_Tipo_Supervision(this);
        }
        #endregion

        #region Alta-Modificacion-Eliminar
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Tipo_Supervision
        ///DESCRIPCIÓN          : Metodo para guardar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Alta_Tipo_Supervision()
        {
            return Cls_Cat_Ort_Tipo_Supervision_Datos.Alta_Tipo_Supervision(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Tipo_Supervision
        ///DESCRIPCIÓN          : Metodo para modificar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Modificar_Tipo_Supervision()
        {
            return Cls_Cat_Ort_Tipo_Supervision_Datos.Modificar_Tipo_Supervision(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Tipo_Supervision
        ///DESCRIPCIÓN          : Metodo para eliminar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Eliminar_Tipo_Supervision()
        {
            return Cls_Cat_Ort_Tipo_Supervision_Datos.Eliminar_Tipo_Supervision(this);
        }
        #endregion
    }
}
