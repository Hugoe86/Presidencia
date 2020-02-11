using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Tipos_Construccion.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Tipos_Construccion_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Tipos_Construccion.Negocio
{
    public class Cls_Cat_Cat_Tipos_Construccion_Negocio
    {
        #region Variables Internas

        private String Tipo_Construccion_Id;
        private String Identificador;
        private String Estatus;

        #endregion

        #region Variables Publicas

        public String P_Tipo_Construccion_Id
        {
            get { return Tipo_Construccion_Id; }
            set { Tipo_Construccion_Id = value; }
        }

        public String P_Identificador
        {
            get { return Identificador; }
            set { Identificador = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Tipo_Construccion()
        {
            return Cls_Cat_Cat_Tipos_Construccion_Datos.Alta_Tipo_Construccion(this);
        }

        public Boolean Modificar_Tipo_Construccion()
        {
            return Cls_Cat_Cat_Tipos_Construccion_Datos.Modificar_Tipo_Construccion(this);
        }

        public DataTable Consultar_Tipos_Construccion()
        {
            return Cls_Cat_Cat_Tipos_Construccion_Datos.Consultar_Tipos_Construccion(this);
        }

        public DataTable Consultar_Tipos_Construccion_Uso()
        {
            return Cls_Cat_Cat_Tipos_Construccion_Datos.Consultar_Tipos_Construccion_Uso(this);
        }

        #endregion
    }
}