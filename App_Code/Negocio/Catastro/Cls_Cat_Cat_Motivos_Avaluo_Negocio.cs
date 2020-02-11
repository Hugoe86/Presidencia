using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Motivos_Avaluo.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Motivos_Avaluo_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cat_Motivos_Avaluo.Negocio
{
    public class Cls_Cat_Cat_Motivos_Avaluo_Negocio
    {
        #region Variables Internas

        private String Motivo_Avaluo_ID;
        private String Motivo_Avaluo_Descripcion;
        private String Estatus;

        #endregion

        #region Variables Publicas

        public String P_Motivo_Avaluo_ID
        {
            get { return Motivo_Avaluo_ID; }
            set { Motivo_Avaluo_ID = value; }
        }

        public String P_Motivo_Avaluo_Descripcion
        {
            get { return Motivo_Avaluo_Descripcion; }
            set { Motivo_Avaluo_Descripcion = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Motivo_Avaluo()
        {
            return Cls_Cat_Cat_Motivos_Avaluo_Datos.Alta_Motivo_Avaluo(this);
        }

        public Boolean Modificar_Motivo_Avaluo()
        {
            return Cls_Cat_Cat_Motivos_Avaluo_Datos.Modificar_Motivo_Avaluo(this);
        }

        public DataTable Consultar_Motivos_Avaluo()
        {
            return Cls_Cat_Cat_Motivos_Avaluo_Datos.Consultar_Motivos_Avaluo(this);
        }
        public DataTable Consulta_Firmante()
        {
            return Cls_Cat_Cat_Motivos_Avaluo_Datos.Consulta_Firmante();
        }

        #endregion
    }
}