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
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Bienes_Clase_Maestra.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pat_Resguardos_Bienes_Negocio
/// </summary>

namespace Presidencia.Control_Patrimonial_Bienes_Clase_Maestra.Negocio {

    public class Cls_Ope_Pat_Bienes_Clase_Maestra_Negocio
    {

        #region "Variables Internas"

            private String Bien_ID = null;
            private String No_Inventario_Anterior = null;
            private String No_Inventario_SIAS = null;
            private String Tipo_Bien = null;
            private Cls_Ope_Pat_Com_Cemovientes_Negocio Animal = null;
            private Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = null;
            private Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = null;

        #endregion

        #region "Variables Publicas"

            public String P_Bien_ID {
                get { return Bien_ID; }
                set { Bien_ID = value; }
            }
            public String P_No_Inventario_Anterior {
                get { return No_Inventario_Anterior; }
                set { No_Inventario_Anterior = value; }
            }
            public String P_No_Inventario_SIAS
            {
                get { return No_Inventario_SIAS; }
                set { No_Inventario_SIAS = value; }
            }
            public String P_Tipo_Bien
            {
                get { return Tipo_Bien; }
                set { Tipo_Bien = value; }
            }
            public Cls_Ope_Pat_Com_Cemovientes_Negocio P_Animal
            {
                get { return Animal; }
                set { Animal = value; }
            }
            public Cls_Ope_Pat_Com_Bienes_Muebles_Negocio P_Bien_Mueble
            {
                get { return Bien_Mueble; }
                set { Bien_Mueble = value; }
            }
            public Cls_Ope_Pat_Com_Vehiculos_Negocio P_Vehiculo
            {
                get { return Vehiculo; }
                set { Vehiculo = value; }
            }

        #endregion

        #region "Metodos"

            public DataTable Consultar_Animales_General() {
                return Cls_Ope_Pat_Bienes_Clase_Maestra_Datos.Consultar_Animales_General(P_Animal);
            }
            public DataTable Consultar_Bienes_Muebles_General() {
                return Cls_Ope_Pat_Bienes_Clase_Maestra_Datos.Consultar_Bienes_Muebles_General(P_Bien_Mueble);
            }
            public DataTable Consultar_Vehiculos_General() {
                return Cls_Ope_Pat_Bienes_Clase_Maestra_Datos.Consultar_Vehiculos_General(P_Vehiculo);
            }

        #endregion
    
    }

}
