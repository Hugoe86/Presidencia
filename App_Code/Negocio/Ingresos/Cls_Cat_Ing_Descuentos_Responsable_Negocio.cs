using System;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Presidencia.Cls_Cat_Ing_Descuentos_Responsable.Datos;

namespace Presidencia.Cls_Cat_Ing_Descuentos_Responsable.Negocio
{

    public class Cls_Cat_Ing_Descuentos_Responsable_Negocio
    {
        #region Variables Internas

        private String Descuento_Responsable_ID;
        private String Empleado_ID;
        private String Estatus;
        private Decimal Porcentaje;
        private String Descripcion;
        private String Tipo;
        private String Usuario;
        private DataTable Dt_Descuentos_Responsable;

        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private String Join;

        private OracleCommand Cmmd;
        #endregion

        #region Variables Publicas

        public String P_Descuento_Responsable_ID
        {
            get { return Descuento_Responsable_ID; }
            set { Descuento_Responsable_ID = value; }
        }

        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public Decimal P_Porcentaje
        {
            get { return Porcentaje; }
            set { Porcentaje = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public string P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public DataTable P_Dt_Descuentos_Responsable
        {
            get { return Dt_Descuentos_Responsable; }
            set { Dt_Descuentos_Responsable = value; }
        }

        
        public String P_Campos_Dinamicos
        {
            get { return Campos_Dinamicos; }
            set { Campos_Dinamicos = value; }
        }

        public String P_Filtros_Dinamicos
        {
            get { return Filtros_Dinamicos; }
            set { Filtros_Dinamicos = value; }
        }

        public String P_Agrupar_Dinamico
        {
            get { return Agrupar_Dinamico; }
            set { Agrupar_Dinamico = value; }
        }

        public String P_Ordenar_Dinamico
        {
            get { return Ordenar_Dinamico; }
            set { Ordenar_Dinamico = value; }
        }

        public String P_Join
        {
            get { return Join; }
            set { Join = value; }
        }


        public OracleCommand P_Cmmd
        {
            get { return Cmmd; }
            set { Cmmd = value; }
        }
        #endregion

        #region Metodos

        public Boolean Alta_Descuento_Responsable()
        {
            return Cls_Cat_Ing_Descuentos_Responsable_Datos.Alta_Descuento_Responsable(this);
        }

        public Boolean Alta_Descuentos_Responsables()
        {
            return Cls_Cat_Ing_Descuentos_Responsable_Datos.Alta_Descuentos_Responsables(this);
        }

        public Boolean Modificar_Descuento_Responsable()
        {
            return Cls_Cat_Ing_Descuentos_Responsable_Datos.Modificar_Descuento_Responsable(this);
        }

        public Boolean Eliminar_Descuento_Responsable()
        {
            return Cls_Cat_Ing_Descuentos_Responsable_Datos.Eliminar_Descuento_Responsable(this);
        }

        public DataTable Consultar_Descuentos_Responsable()
        {
            return Cls_Cat_Ing_Descuentos_Responsable_Datos.Consultar_Descuentos_Responsable(this);
        }

        public Decimal Consultar_Porcentaje_Descuento()
        {
            return Cls_Cat_Ing_Descuentos_Responsable_Datos.Consultar_Porcentaje_Descuento(this);
        }

        public Boolean Validar_Existe()
        {
            return Cls_Cat_Ing_Descuentos_Responsable_Datos.Validar_Existe(this);
        }
        #endregion

    }
}