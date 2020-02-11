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
using Presidencia.Operacion_Modifica_Folio_Pago.Datos;

namespace Presidencia.Operacion_Modifica_Folio_Pago.Negocio{
    
    public class Cls_Ope_Pre_Modifica_Folio_Pago_Negocio
    {

        #region Variables Internas

        private String No_Pago_ID;
        private String Modifica_ID;
        private String Usuario;
        private String Folio_Actual;
        private String Folio_Nuevo;
        private String Motivo;
        private String Recibo;
        private String Empleado_ID;
        private String No_Turno;
        #endregion

        #region Variables Publicas

        public String P_No_Pago_ID
        {
            get { return No_Pago_ID; }
            set { No_Pago_ID = value; }
        }

        public String P_Modifica_ID
        {
            get { return Modifica_ID; }
            set { Modifica_ID = value; }
        }
        
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Folio_Actual
        {
            get { return Folio_Actual; }
            set { Folio_Actual = value; }
        }

        public String P_Folio_Nuevo
        {
            get { return Folio_Nuevo; }
            set { Folio_Nuevo = value; }
        }

        public String P_Motivo
        {
            get { return Motivo; }
            set { Motivo = value; }
        }

        public String P_Recibo
        {
            get { return Recibo; }
            set { Recibo = value; }
        }

        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_No_Turno
        {
            get { return No_Turno; }
            set { No_Turno = value; }
        }
        #endregion

        #region Metodos

        public void Alta_Modificacion()
        {
            Cls_Ope_Pre_Modifica_Folio_Pago_Datos.Alta_Modificacion(this);
        }

        public DataTable Consultar_Folios()
        {
            return Cls_Ope_Pre_Modifica_Folio_Pago_Datos.Consultar_Folios(this);
        }

        public DataTable Consultar_Folios_Busqueda()
        {
            return Cls_Ope_Pre_Modifica_Folio_Pago_Datos.Consultar_Folios_Busqueda(this);
        }

        public DataTable Folio_Existe() 
        {
            return Cls_Ope_Pre_Modifica_Folio_Pago_Datos.Encontrar_Folios(this);
        }

        public DataTable Consulta_Caja_Empleado()
        {
            return Cls_Ope_Pre_Modifica_Folio_Pago_Datos.Consulta_Caja_Empleado(this);
        }
        #endregion

    }
}