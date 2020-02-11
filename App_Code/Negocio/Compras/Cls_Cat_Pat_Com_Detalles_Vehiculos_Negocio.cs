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
using Presidencia.Control_Patrimonial_Catalogo_Detalles_Vehiculos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Detalles_Vehiculos_Negocio
/// </summary>

namespace Presidencia.Control_Patrimonial_Catalogo_Detalles_Vehiculos.Negocio {

    public class Cls_Cat_Pat_Com_Detalles_Vehiculos_Negocio {

        #region Variables Internas

            private String Detalle_Vehiculo_ID = null;
            private String Nombre = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;

        #endregion

        #region Variables Publicas

            public String P_Detalle_Vehiculo_ID
            {
                get { return Detalle_Vehiculo_ID; }
                set { Detalle_Vehiculo_ID = value; }
            }
            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
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
            public String P_Tipo_DataTable {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Detalle_Vehiculos() {
                Cls_Cat_Pat_Com_Detalles_Vehiculos_Datos.Alta_Detalle_Vehiculos(this);
            }

            public void Modificar_Detalle_Vehiculos() {
                Cls_Cat_Pat_Com_Detalles_Vehiculos_Datos.Modificar_Detalle_Vehiculos(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pat_Com_Detalles_Vehiculos_Datos.Consultar_DataTable(this);
            }

            public void Eliminar_Detalle_Vehiculos() {
                Cls_Cat_Pat_Com_Detalles_Vehiculos_Datos.Eliminar_Detalle_Vehiculos(this);
            }

            public Cls_Cat_Pat_Com_Detalles_Vehiculos_Negocio Consultar_Detalle_Vehiculos() {
                return Cls_Cat_Pat_Com_Detalles_Vehiculos_Datos.Consultar_Detalle_Vehiculos(this);
            }

        #endregion

    }
}