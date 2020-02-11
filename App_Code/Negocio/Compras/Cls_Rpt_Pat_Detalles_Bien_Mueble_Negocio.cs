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
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;

/// <summary>
/// Summary description for Cls_Rpt_Pat_Detalles_Bien_Mueble_Negocio
/// </summary>

namespace Presidencia.Control_Patrimonial_Reporte_Detalles_Bien.Negocio { 

public class Cls_Rpt_Pat_Detalles_Bien_Mueble_Negocio
{

    #region Variables Internas

        private String No_Inventario = null;
        private Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble_Detalles = null;
        private Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo_Detalles = null;
        private Cls_Ope_Pat_Com_Cemovientes_Negocio Cemoviente_Detalles = null;
    
    #endregion

    #region Variables Publicas

        public String P_No_Inventario
        {
            get { return No_Inventario; }
            set { No_Inventario = value; }
        }

        public Cls_Ope_Pat_Com_Bienes_Muebles_Negocio P_Bien_Mueble_Detalles
        {
            get { return Bien_Mueble_Detalles; }
            set { Bien_Mueble_Detalles = value; }
        }

        public Cls_Ope_Pat_Com_Vehiculos_Negocio P_Vehiculo_Detalles
        {
            get { return Vehiculo_Detalles; }
            set { Vehiculo_Detalles = value; }
        }

        public Cls_Ope_Pat_Com_Cemovientes_Negocio P_Cemoviente_Detalles
        {
            get { return Cemoviente_Detalles; }
            set { Cemoviente_Detalles = value; }
        }

    #endregion

    #region Metodos

        public String Obtener_Tipo_Bien() {
            String Tipo = null;
            try {
                if (Tipo == null) { 
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Bien_Mueble.P_Numero_Inventario = P_No_Inventario;
                    Bien_Mueble.P_Buscar_Numero_Inventario = true;
                    Bien_Mueble = Bien_Mueble.Consultar_Detalles_Bien_Mueble();
                    if(Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length>0){
                        Bien_Mueble_Detalles = Bien_Mueble;
                        Tipo = "BIEN_MUEBLE";
                    }
                }
                if (Tipo == null) {
                    Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                    Vehiculo.P_Numero_Inventario = Convert.ToInt32(P_No_Inventario);
                    Vehiculo.P_Buscar_Numero_Inventario = true;
                    Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                    if(Vehiculo.P_Vehiculo_ID != null && Vehiculo.P_Vehiculo_ID.Trim().Length>0){
                        Vehiculo_Detalles = Vehiculo;
                        Tipo = "VEHICULO";
                    }
                }
                if (Tipo == null) {
                    Cls_Ope_Pat_Com_Cemovientes_Negocio Animal = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
                    Animal.P_Numero_Inventario = Convert.ToInt32(P_No_Inventario);
                    Animal.P_Buscar_Numero_Inventario = true;
                    Animal = Animal.Consultar_Detalles_Cemoviente();
                    if(Animal.P_Cemoviente_ID != null && Animal.P_Cemoviente_ID.Trim().Length>0){
                        Animal.P_Producto_Almacen = false;
                        Cemoviente_Detalles = Animal;
                        Tipo = "ANIMAL";
                    }
                }
                if (Tipo == null) {
                    throw new Exception("El Bien con el No. de Inventario '" + P_No_Inventario + "' No fue encontrado en la Base de Datos");
                }
            } catch (Exception Ex) {
                throw new Exception("Error: [" + Ex.Message + "]");
            }
            return Tipo;
        }

    #endregion

}
}
