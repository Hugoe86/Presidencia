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
using System.Collections.Generic;
using Presidencia.Catalogo_Tipos_Colonias.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Tipos_Colonias_Negocio
/// </summary>

namespace Presidencia.Catalogo_Tipos_Colonias.Negocio{
    public class Cls_Cat_Pre_Tipos_Colonias_Negocio {

        #region Variables Internas

            private String Tipo_Colonia_ID;
            private String Descripcion;
            private String Estatus;
            private String Usuario;
            private String Tabla;
            private String Campo;
            private String ID;
            private String Campo_ID;
            private String Colonia_ID;

        #endregion

        #region Variables Publicas

            public String P_Tipo_Colonia_ID
            {
                get { return Tipo_Colonia_ID; }
                set { Tipo_Colonia_ID = value.Trim(); }
            }

            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value.Trim(); }
            }

            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value.Trim(); }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value.Trim(); }
            }

            public String P_Tabla
            {
                get { return Tabla; }
                set { Tabla = value.Trim(); }
            }

            public String P_Campo
            {
                get { return Campo; }
                set { Campo = value.Trim(); }
            }

            public String P_ID
            {
                get { return ID; }
                set { ID = value.Trim(); }
            }

            public String P_Campo_ID
            {
                get { return Campo_ID; }
                set { Campo_ID = value.Trim(); }
            }

            public String P_Colonia_ID
            {
                get { return Colonia_ID; }
                set { Colonia_ID = value.Trim(); }
            }
        #endregion

        #region Metodos

            public Boolean Alta_Tipo_Colonia() {
                return Cls_Cat_Pre_Tipos_Colonias_Datos.Alta_Tipo_Colonia(this);
            }

            public Boolean Modificar_Tipo_Colonia() {
                return Cls_Cat_Pre_Tipos_Colonias_Datos.Modificar_Tipo_Colonia(this);
            }

            public Boolean Eliminar_Tipo_Colonia() {
                return Cls_Cat_Pre_Tipos_Colonias_Datos.Eliminar_Tipo_Colonia(this);
            }

            public Cls_Cat_Pre_Tipos_Colonias_Negocio Consultar_Datos_Tipo_Colonia() {
                return Cls_Cat_Pre_Tipos_Colonias_Datos.Consultar_Datos_Tipo_Colonia(this);
            }

            public DataTable Consultar_Tipos_Colonias() {
                return Cls_Cat_Pre_Tipos_Colonias_Datos.Consultar_Tipos_Colonias(this);
            }
           
            public DataSet Validar_Descripcion() 
            {
                return Cls_Cat_Pre_Tipos_Colonias_Datos.Validar_Descripcion(this);
            }
  
             public DataSet Validar_Descripcion_Calles() 
            {
                return Cls_Cat_Pre_Tipos_Colonias_Datos.Validar_Descripcion_Calles(this);
            }
        #endregion

    }
}
