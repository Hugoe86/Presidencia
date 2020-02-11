using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Parametro_Invitacion.Datos;

namespace Presidencia.Parametro_Invitacion.Negocio
{
    public class Cls_Cat_Com_Parametro_Invitacion_Negocio
    {

        //Propiedades
        #region Variables Locales
        private String Invitacion_Proveedores;
        #endregion
        //Variables publicas
        #region Variables Publicas
        public String P_Invitacion_Proveedores
        {
            get { return Invitacion_Proveedores; }
            set { Invitacion_Proveedores = value; }
        }
        #endregion
        #region Metodos
        public DataTable Consular_Paremetros()
        {
            return Cls_Cat_Com_Parametro_Invitacion_Datos.Consular_Parametros(this);
        }
        public int Actualizar_Parametros()
        {
            return Cls_Cat_Com_Parametro_Invitacion_Datos.Actualizar_Parametros(this);
        }
        #endregion
    }//fin del class
}//fin del namespace