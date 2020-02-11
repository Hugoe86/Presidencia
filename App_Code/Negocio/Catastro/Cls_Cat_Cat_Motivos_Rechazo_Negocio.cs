using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Motivos_Rechazo.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Motivos_Rechazo_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Motivos_Rechazo.Negocio
{
    public class Cls_Cat_Cat_Motivos_Rechazo_Negocio
    {
        #region Variables Internas

        private String Motivo_ID;
        private String Motivo_Descripcion;
        private String Estatus;

        #endregion

        #region Variables Publicas

        public String P_Motivo_ID
        {
            get { return Motivo_ID; }
            set { Motivo_ID = value; }
        }

        public String P_Motivo_Descripcion
        {
            get { return Motivo_Descripcion; }
            set { Motivo_Descripcion = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Motivo_Rechazo()
        {
            return Cls_Cat_Cat_Motivos_Rechazo_Datos.Alta_Motivo_Rechazo(this);
        }

        public Boolean Modificar_Motivo_Rechazo()
        {
            return Cls_Cat_Cat_Motivos_Rechazo_Datos.Modificar_Motivo_Rechazo(this);
        }

        public DataTable Consultar_Motivos_Rechazo()
        {
            return Cls_Cat_Cat_Motivos_Rechazo_Datos.Consultar_Motivos_Rechazo(this);
        }

        #endregion
    }
}