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
using Presidencia.Caja_Cierre_Turno.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pre_Cierre_Turno_Negocio
/// </summary>
namespace Presidencia.Caja_Cierre_Turno.Negocio
{
    public class Cls_Ope_Pre_Cierre_Turno_Negocio
    {
        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas

        private String No_Turno;
        private String Caja_ID;
        private String Estatus;
        private String Total_Bancos;
        private String Total_Cheques;
        private String Total_Transferencias;
        private String Total_Efectivo_Sistema;
        private String Dependencia_ID;

        private String Cant_Diez_Cent;
        private String Cant_Veinte_Cent;
        private String Cant_Cinc_Cent;
        private String Cant_Un_P;
        private String Cant_Dos_P;
        private String Cant_Cinco_P;
        private String Cant_Diez_P;
        private String Cant_Veinte_P;
        private String Cant_Cincuenta_P;
        private String Cant_Cien_P;
        private String Cant_Doscientos_P;
        private String Cant_Quinientos_P;
        private String Cant_Mil_P;
        private String Cant_Monto_Total;
        #endregion

        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas

        public String P_No_Turno
        {
            get { return No_Turno; }
            set { No_Turno = value; }
        }

        public String P_Caja_ID
        {
            get { return Caja_ID; }
            set { Caja_ID = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Total_Bancos
        {
            get { return Total_Bancos; }
            set { Total_Bancos = value; }
        }

        public String P_Total_Cheques
        {
            get { return Total_Cheques; }
            set { Total_Cheques = value; }
        }

        public String P_Total_Transferencias
        {
            get { return Total_Transferencias; }
            set { Total_Transferencias = value; }
        }

        public String P_Total_Efectivo_Sistema
        {
            get { return Total_Efectivo_Sistema; }
            set { Total_Efectivo_Sistema = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }


        public String P_Cant_Diez_Cent {
            get { return Cant_Diez_Cent; }
            set { Cant_Diez_Cent = value; }
        }

        public String P_Cant_Veinte_Cent
        {
            get { return Cant_Veinte_Cent; }
            set { Cant_Veinte_Cent = value; }
        }

        public String P_Cant_Cinc_Cent
        {
            get { return Cant_Cinc_Cent; }
            set { Cant_Cinc_Cent = value; }
        }

        public String P_Cant_Un_P
        {
            get { return Cant_Un_P; }
            set { Cant_Un_P = value; }
        }

        public String P_Cant_Dos_P
        {
            get { return Cant_Dos_P; }
            set { Cant_Dos_P = value; }
        }

        public String P_Cant_Cinco_P
        {
            get { return Cant_Cinco_P; }
            set { Cant_Cinco_P = value; }
        }

        public String P_Cant_Diez_P
        {
            get { return Cant_Diez_P; }
            set { Cant_Diez_P = value; }
        }

        public String P_Cant_Veinte_P
        {
            get { return Cant_Veinte_P; }
            set { Cant_Veinte_P = value; }
        }

        public String P_Cant_Cincuenta_P
        {
            get { return Cant_Cincuenta_P; }
            set { Cant_Cincuenta_P = value; }
        }

        public String P_Cant_Cien_P
        {
            get { return Cant_Cien_P; }
            set { Cant_Cien_P = value; }
        }

        public String P_Cant_Doscientos_P
        {
            get { return Cant_Doscientos_P; }
            set { Cant_Doscientos_P = value; }
        }

        public String P_Cant_Quinientos_P
        {
            get { return Cant_Quinientos_P; }
            set { Cant_Quinientos_P = value; }
        }

        public String P_Cant_Mil_P
        {
            get { return Cant_Mil_P; }
            set { Cant_Mil_P = value; }
        }

        public String P_Cant_Monto_Total
        {
            get { return Cant_Monto_Total; }
            set { Cant_Monto_Total = value; }
        }
        #endregion

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        public DataTable Consultar_Caj_Turno()
        {

            return Cls_Ope_Pre_Cierre_Turno_Datos.Consultar_Caj_Turno(this);
        }

        public DataTable Consultar_Detalle_Pagos()
        {
            return Cls_Ope_Pre_Cierre_Turno_Datos.Consultar_Detalle_Pagos(this);
        }

        public DataTable Consultar_Dependencias()
        {
            return Cls_Ope_Pre_Cierre_Turno_Datos.Consultar_Dependencias(this);
        }

        public DataTable Consultar_Totales_Caj_Turno()
        {
            return Cls_Ope_Pre_Cierre_Turno_Datos.Consultar_Totales_Caj_Turno(this);
        }

        public bool Cerrar_Caja()
        {
            return Cls_Ope_Pre_Cierre_Turno_Datos.Cerrar_Caja(this);
        }

        public bool Generar_Poliza()
        {
            return Cls_Ope_Pre_Cierre_Turno_Datos.Generar_Poliza(this);
        }


        public DataSet Rpt_Caj_Ingresos()
        {
            return Cls_Ope_Pre_Cierre_Turno_Datos.Rpt_Caj_Ingresos(this);
        }
        #endregion



    }//Fin del Class
}//Fin del namespace