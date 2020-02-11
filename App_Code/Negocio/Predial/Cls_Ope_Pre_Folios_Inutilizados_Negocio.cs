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
using Presidencia.Operacion_Folios_Inutilizados.Datos;

namespace Presidencia.Operacion_Folios_Inutilizados.Negocio{
    
    public class Cls_Ope_Pre_Folios_Inutilizados_Negocio
    {
        #region Variables Internas

        private String No_Pago_ID;
        private String No_Recibo;
        private String Caja_ID;
        private String Fecha;
        private String Motivo;
        private String Observaciones;
        private String Estatus;
        private String Usuario;
        private String Cajero;
        private String Empleado_ID;
        private String No_Folio_Fin;
        private String No_Turno;
        private DataTable Dt_Folios;

        #endregion

        #region Variables Publicas

        public String P_No_Pago_ID
        {
            get { return No_Pago_ID; }
            set { No_Pago_ID = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public DataTable P_Dt_Folios {
            get { return Dt_Folios; }
            set { Dt_Folios = value; }
        }

        public DataTable  P_Folios
        {
            get { return Dt_Folios; }
            set { Dt_Folios  = value; }
        }
        public String P_No_Folio_Fin
        {
            get { return No_Folio_Fin ; }
            set { No_Folio_Fin = value; }
        }
        public String P_No_Recibo
        {
            get { return No_Recibo; }
            set { No_Recibo = value; }
        }
        
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Caja_ID
        {
            get { return Caja_ID; }
            set { Caja_ID = value; }
        }

        public String P_Fecha
        {
            get { return Fecha; }
            set { Fecha = value; }
        }

        public String P_Motivo
        {
            get { return Motivo; }
            set { Motivo = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Cajero
        {
            get { return Cajero; }
            set { Cajero = value; }
        }

        public String P_No_Turno
        {
            get { return No_Turno; }
            set { No_Turno = value; }
        }
        #endregion

        #region Metodos

        public void Alta_Folio_Inutilizado()
        {
            Cls_Ope_Pre_Folios_Inutilizados_Datos.Alta_Folio_Inutilizado(this);
        }

        public DataTable Consultar_Recibos()
        {
            return Cls_Ope_Pre_Folios_Inutilizados_Datos.Consultar_Recibos(this);
        }

        public DataTable Consultar_Folios_Utilizados()
        {
            return Cls_Ope_Pre_Folios_Inutilizados_Datos.Consultar_Folios_Utilizados(this);
        }

        public DataTable Consultar_Recibos_Busqueda()
        {
            return Cls_Ope_Pre_Folios_Inutilizados_Datos.Consultar_Recibos_Busqueda(this);
        }

        public String Consultar_Caja()
        {
            return Cls_Ope_Pre_Folios_Inutilizados_Datos.Consultar_Caja(this);
        }

        public DataTable Consulta_Caja_Empleado()
        {
            return Cls_Ope_Pre_Folios_Inutilizados_Datos.Consulta_Caja_Empleado(this);
        }

        public DataTable Consultar_Motivos()
        {
            return Cls_Ope_Pre_Folios_Inutilizados_Datos.Consultar_Motivos();
        }

        #endregion
    }
}