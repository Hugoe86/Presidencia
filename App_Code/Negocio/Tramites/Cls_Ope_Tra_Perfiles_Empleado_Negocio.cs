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
using Presidencia.Tramites_Perfiles_Empleados.Datos;
using System.Data.OracleClient;

namespace Presidencia.Tramites_Perfiles_Empleados.Negocio
{
    public class Cls_Ope_Tra_Perfiles_Empleado_Negocio
    {
        #region Variables Privadas
        private String Emplead_ID;
        private String Perfil_ID;
        private String Nombre_Empleado;
        private String Unidad_Responsable_ID;
        private String Numero_Empleado;
        private String Nombre_Perfil;
        private DataTable Dt_Perfil_Empleado; 
        private OracleCommand Cmmd;
        #endregion

        #region Variables publicas
        public String P_Empleado_ID
        {
            get { return Emplead_ID; }
            set { Emplead_ID = value; }
        }
        public String P_Perfil_ID
        {
            get { return Perfil_ID; }
            set { Perfil_ID = value; }
        }
        public String P_Nombre_Empleado
        {
            get { return Nombre_Empleado; }
            set { Nombre_Empleado = value; }
        }
        public String P_Unidad_Responsable_ID
        {
            get { return Unidad_Responsable_ID; }
            set { Unidad_Responsable_ID = value; }
        }
        public String P_Numero_Empleado
        {
            get { return Numero_Empleado; }
            set { Numero_Empleado = value; }
        }
        public DataTable P_Dt_Perfil_Empleado
        {
            get { return Dt_Perfil_Empleado; }
            set { Dt_Perfil_Empleado = value; }
        }
        public String P_Nombre_Perfil
        {
            get { return Nombre_Perfil; }
            set { Nombre_Perfil = value; }
        }
        public OracleCommand P_Cmmd
        {
            get { return Cmmd; }
            set { Cmmd = value; }
        }
        #endregion

        #region Metodos Alta_Modificar_Eliminar
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Guardar_Perfil
        ///DESCRIPCIÓN          : Metodo para guardar los datos del perfil
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Mayo/2012 
        ///*********************************************************************************************************
        public Boolean Guardar_Perfil()
        {
            return Cls_Ope_Tra_Perfiles_Empleado_Datos.Alta_Perfil_Empleado(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Perfil
        ///DESCRIPCIÓN          : Metodo que modificara los datos del perfil
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Mayo/2012 
        ///*********************************************************************************************************
        public Boolean Modificar_Perfil()
        {
            return Cls_Ope_Tra_Perfiles_Empleado_Datos.Modificar_Perfil_Empleado(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Eliminar_Perfil
        ///DESCRIPCIÓN          : Metodo que eliminara los datos del perfil
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Mayo/2012 
        ///*********************************************************************************************************
        public Boolean Eliminar_Perfil()
        {
            return Cls_Ope_Tra_Perfiles_Empleado_Datos.Eliminar_Perfil_Empleado(this);
        }

        #endregion

        #region Metodos Consulta
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Empleado
        ///DESCRIPCIÓN          : Metodo que consultara la informacion del empleado
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Mayo/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Empleado()
        {
            return Cls_Ope_Tra_Perfiles_Empleado_Datos.Consultar_Empleado(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Perfil
        ///DESCRIPCIÓN          : Metodo que consultara los registros que tiene el perfil
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Mayo/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Perfil()
        {
            return Cls_Ope_Tra_Perfiles_Empleado_Datos.Consultar_Perfil(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Perfil_Existentes
        ///DESCRIPCIÓN          : Metodo que consultara los perfiles
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 26/Mayo/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Perfil_Existentes()
        {
            return Cls_Ope_Tra_Perfiles_Empleado_Datos.Consultar_Perfil_Existentes(this);
        }
        #endregion

    }
}
