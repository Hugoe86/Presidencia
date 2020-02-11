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
using Presidencia.Catalogo_Cuotas_Minimas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Cuotas_Minimas_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cuotas_Minimas.Negocio{
    public class Cls_Cat_Pre_Cuotas_Minimas_Negocio
    {

        #region Variables Internas

            private String Cuota_Minima_ID;
            private String Anio;
            private Double Cuota;
            private String Usuario;
            private String Cantidad_Cuota;
        
        #endregion

        #region Variables Publicas

            public String P_Cantidad_Cuota
            {
                get { return Cantidad_Cuota; }
                set { Cantidad_Cuota = value; }
            }
            public String P_Cuota_Minima_ID
            {
                get { return Cuota_Minima_ID; }
                set { Cuota_Minima_ID = value; }
            }

            public String P_Anio
            {
                get { return Anio; }
                set { Anio = value; }
            }

            public Double P_Cuota
            {
                get { return Cuota; }
                set { Cuota = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

        #endregion

        #region Metodos
        
            public void Alta_Cuota_Minima() {
                Cls_Cat_Pre_Cuotas_Minimas_Datos.Alta_Cuota_Minima(this);
            }

            public void Modificar_Cuota_Minima(){
                Cls_Cat_Pre_Cuotas_Minimas_Datos.Modificar_Cuota_Minima(this);
            }

            public void Eliminar_Cuota_Minima(){
                Cls_Cat_Pre_Cuotas_Minimas_Datos.Eliminar_Cuota_Minima(this);
            }

            public Cls_Cat_Pre_Cuotas_Minimas_Negocio Consultar_Datos_Cuota_Minima(){
                return Cls_Cat_Pre_Cuotas_Minimas_Datos.Consultar_Datos_Cuota_Minima(this);
            }

            public DataTable Consultar_Cuotas_Minimas(){
                return Cls_Cat_Pre_Cuotas_Minimas_Datos.Consultar_Cuotas_Minimas(this);
            }

            public Decimal Consultar_Cuota_Minima_Anio(String Anio)
            {
                return Cls_Cat_Pre_Cuotas_Minimas_Datos.Consultar_Cuota_Minima_Anio(Anio);
            }

            public DataTable Consultar_Cuotas_Minimas_Ventana_Emergente()
            {
                return Cls_Cat_Pre_Cuotas_Minimas_Datos.Consultar_Cuotas_Minimas_Ventana_Emergente(this);
            }

            public Boolean Validar_Existe()
            {
                return Cls_Cat_Pre_Cuotas_Minimas_Datos.Validar_Existe(this);
            }
        #endregion
    }
}