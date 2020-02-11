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
using Presidencia.Catalogo_Compras_Servicios.Datos;

namespace Presidencia.Catalogo_Compras_Servicios.Negocio
{
    public class Cls_Cat_Com_Servicios_Negocio
    {
#region Variables Locales

        private String Servicio_ID;
        private String Impuesto_ID;
        private String Nombre;
        private Double Costo;
        private DateTime Fecha_Ultimo_Costo;
        private String Comentarios;
        private String Usuario_Creo;
        private String Giro_ID;
        private String Partida_ID;
        private String Clave_Partida;
        private String Clave;

#endregion

#region Variables Publicas

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Giro_ID
        {
            get { return Giro_ID; }
            set { Giro_ID = value; }
        }

        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }

        public String P_Clave_Partida
        {
            get { return Clave_Partida; }
            set { Clave_Partida = value; }
        }

        public String P_Servicio_ID
        {
            get { return Servicio_ID; }
            set { Servicio_ID = value; }
        }

        public String P_Impuesto_ID
        {
            get { return Impuesto_ID; }
            set { Impuesto_ID = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public Double P_Costo
        {
            get { return Costo; }
            set { Costo = value; }
        }

        public DateTime P_Fecha_Ultimo_Costo
        {
            get { return Fecha_Ultimo_Costo; }
            set { Fecha_Ultimo_Costo = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
#endregion

#region Metodos

        public DataTable Consulta_Datos_A_Exportar_Excel()
        {
            return Cls_Cat_Com_Servicios_Datos.Consulta_Datos_Servicios_Para_Excel();
        }

        public void Alta_Servicio()
        {
            Cls_Cat_Com_Servicios_Datos.Alta_Servicios(this);
        }

        public DataTable Consulta_Servicios()
        {
            return Cls_Cat_Com_Servicios_Datos.Consulta_Servicios(this);
        }
        public void Baja_Servicio()
        {
            Cls_Cat_Com_Servicios_Datos.Baja_Servicios(this);
        }

        public void Modificar_Servicio()
        {
            Cls_Cat_Com_Servicios_Datos.Modificar_Servicio(this);
        }

        public DataTable Consultar_Servicio_en_Proceso()
        {
            return Cls_Cat_Com_Servicios_Datos.Consultar_Servicio_en_Proceso(this);
        }

#endregion
      
    }
}