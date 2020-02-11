using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Calidad_Construccion.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Calidad_Construccion_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Calidad_Construccion.Negocio
{
    public class Cls_Cat_Cat_Calidad_Construccion_Negocio
    {
        #region Variables Internas

        private String Calidad_Id;
        private String Tipo_Construccion_Id;
        private String Calidad;
        private String Clave_Calidad;

        #endregion

        #region Variables Publicas

        public String P_Calidad_Id
        {
            get { return Calidad_Id; }
            set { Calidad_Id = value; }
        }

        public String P_Tipo_Construccion_Id
        {
            get { return Tipo_Construccion_Id; }
            set { Tipo_Construccion_Id = value; }
        }

        public String P_Calidad
        {
            get { return Calidad; }
            set { Calidad = value; }
        }

        public String P_Clave_Calidad
        {
            get { return Clave_Calidad; }
            set { Clave_Calidad = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Calidad_Construccion()
        {
            return Cls_Cat_Cat_Calidad_Construccion_Datos.Alta_Calidad_Construccion(this);
        }

        public Boolean Modificar_Calidad_Construccion()
        {
            return Cls_Cat_Cat_Calidad_Construccion_Datos.Modificar_Calidad_Construccion(this);
        }

        public DataTable Consultar_Calidad_Construccion()
        {
            return Cls_Cat_Cat_Calidad_Construccion_Datos.Consultar_Calidad_Construccion(this);
        }

        #endregion
    }
}