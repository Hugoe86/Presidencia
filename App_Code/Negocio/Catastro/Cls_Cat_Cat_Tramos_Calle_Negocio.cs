using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Cat_Cat_Tramos_Calle.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Tramos_Calle_Negocio
/// </summary>

namespace Presidencia.Cat_Cat_Tramos_Calle.Negocio
{
    public class Cls_Cat_Cat_Tramos_Calle_Negocio
    {
        #region Variables Internas

        private String Calle_ID;
        private String Calle_Busqueda;
        private String Colonia_Busqueda;
        private DataTable Dt_Tramos;

        #endregion

        #region Variables Publicas

        public String P_Calle_ID
        {
            get { return Calle_ID; }
            set { Calle_ID = value; }
        }

        public String P_Calle_Busqueda
        {
            get { return Calle_Busqueda; }
            set { Calle_Busqueda = value; }
        }

        public String P_Colonia_Busqueda
        {
            get { return Colonia_Busqueda; }
            set { Colonia_Busqueda = value; }
        }

        public DataTable P_Dt_Tramos
        {
            get { return Dt_Tramos; }
            set { Dt_Tramos = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Tramos()
        {
             return Cls_Cat_Cat_Tramos_Calle_Datos.Alta_Tramos(this);
        }

        public Boolean Modificar_Tramos()
        {
            return Cls_Cat_Cat_Tramos_Calle_Datos.Modificar_Tramos(this);
        }

        public DataTable Consultar_Calles()
        {
            return Cls_Cat_Cat_Tramos_Calle_Datos.Consultar_Calles(this);
        }

        public DataTable Consultar_Tramos()
        {
            return Cls_Cat_Cat_Tramos_Calle_Datos.Consultar_Tramos(this);
        }

        #endregion
    }
}