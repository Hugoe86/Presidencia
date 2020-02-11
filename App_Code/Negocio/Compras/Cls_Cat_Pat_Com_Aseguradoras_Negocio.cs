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
using Presidencia.Control_Patrimonial_Catalogo_Aseguradoras.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Aseguradoras_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Aseguradoras.Negocio {

    public class Cls_Cat_Pat_Com_Aseguradoras_Negocio
    {

        #region Variables Internas

            private String Aseguradora_ID = null;
            private String Nombre = null;
            private String Nombre_Fiscal= null;
            private String Nombre_Comercial = null;
            private String RFC = null;
            private String Cuenta_Contable = null;
            private String Estatus = null;
            private DataTable Domicilios = null;
            private DataTable Contactos = null;
            private DataTable Bancos = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;
             
        #endregion

        #region Variables Publicas

            public String P_Aseguradora_ID
            {
                get { return Aseguradora_ID; }
                set { Aseguradora_ID = value; }
            }

            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }

            public String P_Nombre_Fiscal
            {
                get { return Nombre_Fiscal; }
                set { Nombre_Fiscal = value; }
            }

            public String P_Nombre_Comercial
            {
                get { return Nombre_Comercial; }
                set { Nombre_Comercial = value; }
            }

            public String P_RFC
            {
                get { return RFC; }
                set { RFC = value; }
            }

            public String P_Cuenta_Contable
            {
                get { return Cuenta_Contable; }
                set { Cuenta_Contable = value; }
            }

            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }

            public DataTable P_Domicilios
            {
                get { return Domicilios; }
                set { Domicilios = value; }
            }

            public DataTable P_Contactos
            {
                get { return Contactos; }
                set { Contactos = value; }
            }

            public DataTable P_Bancos
            {
                get { return Bancos; }
                set { Bancos = value; }
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

            public void Alta_Aseguradora() {
                Cls_Cat_Pat_Com_Aseguradoras_Datos.Alta_Aseguradora(this);
            }

            public void Modificar_Aseguradora() {
                Cls_Cat_Pat_Com_Aseguradoras_Datos.Modificar_Aseguradora(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pat_Com_Aseguradoras_Datos.Consultar_DataTable(this);
            }

            public Cls_Cat_Pat_Com_Aseguradoras_Negocio Consultar_Datos_Aseguradora() {
                return Cls_Cat_Pat_Com_Aseguradoras_Datos.Consultar_Datos_Aseguradora(this);
            }

            public void Eliminar_Aseguradora() {
                Cls_Cat_Pat_Com_Aseguradoras_Datos.Eliminar_Aseguradora(this);
            }

        #endregion

    }

}