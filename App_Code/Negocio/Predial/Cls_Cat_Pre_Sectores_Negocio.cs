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
using Presidencia.Catalogo_Sectores.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Estados_Predio_Negocio
/// </summary>

namespace Presidencia.Catalogo_Sectores.Negocio
{
    public class Cls_Cat_Pre_Sectores_Negocio
    {

        #region Variables Internas

            private String Sector_ID;
            private String Nombre;
            private String Comentarios;
            private String Usuario;
            private String Tipo_DataTable;
            private String Clave;
            private String Colonia_ID;
            private DataTable Colonias;

        #endregion

        #region Variables Publicas

            public String P_Sector_ID
            {
                get { return Sector_ID; }
                set { Sector_ID = value; }
            }

            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }

            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
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

            public String P_Clave
            {
                get { return Clave; }
                set { Clave = value; }
            }

            public String P_Colonia_ID
            {
                get { return Colonia_ID; }
                set { Colonia_ID = value; }
            }

            public DataTable P_Colonias
            {
                get { return Colonias; }
                set { Colonias = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Sector() {
                Cls_Cat_Pre_Sectores_Datos.Alta_Sector(this);
            }

            public void Alta_Colonias()
            {
                Cls_Cat_Pre_Sectores_Datos.Alta_Colonias(this);
            }

            public void Modificar_Sector() {
                Cls_Cat_Pre_Sectores_Datos.Modificar_Sector(this);
            }

            public void Eliminar_Sector() {
                Cls_Cat_Pre_Sectores_Datos.Eliminar_Sector(this);
            }

            public void Eliminar_Colonia()
            {
                Cls_Cat_Pre_Sectores_Datos.Eliminar_Colonia(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pre_Sectores_Datos.Consultar_DataTable(this);
            }

            public DataTable Llenar_Combo_Colonias()
            {
                return Cls_Cat_Pre_Sectores_Datos.Llenar_Combo_Colonias();
            }

            public DataTable Consultar_Colonias()
            {
                return Cls_Cat_Pre_Sectores_Datos.Consultar_Colonias(this);
            }

            public String Obtener_Clave_Maxima() 
            {
                return Cls_Cat_Pre_Sectores_Datos.Obtener_Clave_Maxima();
            }

            public DataSet Llenar_Clave_Colonia() 
            {
                return Cls_Cat_Pre_Sectores_Datos.Consultar_Clave_Colonia(this);
            }

        #endregion

    }
}