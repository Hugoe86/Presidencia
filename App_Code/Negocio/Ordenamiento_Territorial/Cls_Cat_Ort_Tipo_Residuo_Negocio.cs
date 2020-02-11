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
using Presidencia.Ordenamiento_Territorial_Tipos_Residuos.Datos;

namespace Presidencia.Ordenamiento_Territorial_Tipos_Residuos.Negocio
{
    public class Cls_Cat_Ort_Tipo_Residuo_Negocio
    {
        #region Variables internas
        private String Tipo_Residuo_ID;
        private String Nombre;
        private String Usuario;
        #endregion

        #region Variables Publicas
        public String P_Tipo_Residuo_ID
        {
            get { return Tipo_Residuo_ID; }
            set { Tipo_Residuo_ID = value; }
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
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tipos_Residuos
        ///DESCRIPCIÓN          : Metodo para consultar los datos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 12/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Tipos_Residuos()
        {
            return Cls_Cat_Ort_Tipo_Residuo_Datos.Consultar_Tipos_Residuos(this);
        }
        #endregion

        #region Alta-Modificacion-Eliminar
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Condicion_Inmueble
        ///DESCRIPCIÓN          : Metodo para guardar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 12/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Alta()
        {
            return Cls_Cat_Ort_Tipo_Residuo_Datos.Alta(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Condicion_Inmueble
        ///DESCRIPCIÓN          : Metodo para modificar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 12/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Modificar()
        {
            return Cls_Cat_Ort_Tipo_Residuo_Datos.Modificar(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Condicion_Inmueble
        ///DESCRIPCIÓN          : Metodo para eliminar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 12/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Eliminar()
        {
            return Cls_Cat_Ort_Tipo_Residuo_Datos.Eliminar(this);
        }
        #endregion
    }
}
