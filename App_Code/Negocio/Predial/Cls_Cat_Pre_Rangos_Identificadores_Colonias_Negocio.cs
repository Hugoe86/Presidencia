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
using Presidencia.Catalogo_Rangos_Identificadores_Colonias.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio
/// </summary>

namespace Presidencia.Catalogo_Rangos_Identificadores_Colonias.Negocio{
    public class Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio
    {

        #region Variables Internas

            private String Rango_Identificador_Colonia_ID;
            private String Tipo_Colonia_ID;
            private int Rango_Inicial;
            private int Rango_Final;
            private String Estatus;
            private String Usuario;
            private String Tipo_DataTable;

        #endregion

        #region Variables Publicas

            public String P_Rango_Identificador_Colonia_ID
            {
                get { return Rango_Identificador_Colonia_ID; }
                set { Rango_Identificador_Colonia_ID = value; }
            }

            public String P_Tipo_Colonia_ID
            {
                get { return Tipo_Colonia_ID; }
                set { Tipo_Colonia_ID = value; }
            }

            public int P_Rango_Inicial
            {
                get { return Rango_Inicial; }
                set { Rango_Inicial = value; }
            }

            public int P_Rango_Final
            {
                get { return Rango_Final; }
                set { Rango_Final = value; }
            }

            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }

        #endregion

        #region Metodos

            public Boolean Alta_Rango_Identificador_Colonia() {
                return Cls_Cat_Pre_Rangos_Identificadores_Colonias_Datos.Alta_Rango_Identificador_Colonia(this);
            }

            public Boolean Modificar_Rango_Identificador_Colonia() {
                return Cls_Cat_Pre_Rangos_Identificadores_Colonias_Datos.Modificar_Rango_Identificador_Colonia(this);
            }

            public Boolean Eliminar_Rango_Identificador_Colonia() {
                return Cls_Cat_Pre_Rangos_Identificadores_Colonias_Datos.Eliminar_Rango_Identificador_Colonia(this);
            }

            public Cls_Cat_Pre_Rangos_Identificadores_Colonias_Negocio Consultar_Datos_Rango_Identificador_Colonia() {
                return Cls_Cat_Pre_Rangos_Identificadores_Colonias_Datos.Consultar_Datos_Rango_Identificador_Colonia(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pre_Rangos_Identificadores_Colonias_Datos.Consultar_DataTable(this);
            }

        #endregion

    }
}
