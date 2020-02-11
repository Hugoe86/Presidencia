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
using Presidencia.Tramites_Actividades.Datos;

namespace Presidencia.Tramites_Actividades.Negocio
{
    public class Cls_Cat_Tra_Actividades_Negocio
    {
        #region Variables internas
        private String Actividad_ID;
        private String Nombre;
        private String Usuario;
        #endregion

        #region Variables Publicas
        public String P_Actividad_ID
        {
            get { return Actividad_ID; }
            set { Actividad_ID = value; }
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
        ///NOMBRE DE LA FUNCIÓN : Consultar_Inspectores
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Actividades()
        {
            return Cls_Cat_Tra_Actividades_Datos.Consultar_Actividades(this);
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
            return Cls_Cat_Tra_Actividades_Datos.Alta(this);
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
            return Cls_Cat_Tra_Actividades_Datos.Modificar(this);
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
            return Cls_Cat_Tra_Actividades_Datos.Eliminar(this);
        }
        #endregion
    }
}