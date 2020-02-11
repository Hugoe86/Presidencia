using System;
using System.Data;
using System.Data.OracleClient;
using Presidencia.Operacion_Predial_Pagos_Instit_Externas.Datos;

namespace Presidencia.Operacion_Predial_Pagos_Instit_Externas.Negocio
{

    public class Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio
    {

#region Varibles Internas

        private String Cuenta_Predial;
        private String Cuenta_Predial_Id;
        private String Linea_Captura;
        private String Institucion_Id;
        private String Columna;
        private String Anio;
        private String No_Captura;
        private String Caja_ID;
        private OracleCommand Comando_Oracle;
        private DataTable Dt_Capturas;
        private DataTable Dt_Detalles_Captura;
        private DataTable Dt_Adeudos_Totales;
        private DataTable Dt_Adeudo_Detallado_Predial;
        private DataTable Dt_Pasivos_Pago;
        private String Usuario = "";
        private String Filtro_Dinamico = "";

#endregion Varibles Internas

#region Varibles Publicas

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }

        public String P_Cuenta_Predial_Id
        {
            get { return Cuenta_Predial_Id; }
            set { Cuenta_Predial_Id = value; }
        }

        public String P_Linea_Captura
        {
            get { return Linea_Captura; }
            set { Linea_Captura = value; }
        }

        public String P_Institucion_Id
        {
            get { return Institucion_Id; }
            set { Institucion_Id = value; }
        }

        public String P_Columna_Linea_Captura
        {
            get { return Columna; }
            set { Columna = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_No_Captura
        {
            get { return No_Captura; }
            set { No_Captura = value; }
        }

        public String P_Caja_ID
        {
            get { return Caja_ID; }
            set { Caja_ID = value; }
        }

        public OracleCommand P_Comando_Oracle
        {
            get { return Comando_Oracle; }
            set { Comando_Oracle = value; }
        }

        public string P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public string P_Filtro_Dinamico
        {
            get { return Filtro_Dinamico; }
            set { Filtro_Dinamico = value; }
        }

        public DataTable P_Dt_Capturas
        {
            get { return Dt_Capturas; }
            set { Dt_Capturas = value; }
        }

        public DataTable P_Dt_Detalles_Captura
        {
            get { return Dt_Detalles_Captura; }
            set { Dt_Detalles_Captura = value; }
        }

        public DataTable P_Dt_Adeudos_Totales
        {
            get { return Dt_Adeudos_Totales; }
            set { Dt_Adeudos_Totales = value; }
        }

        public DataTable P_Dt_Adeudo_Detallado_Predial
        {
            get { return Dt_Adeudo_Detallado_Predial; }
            set { Dt_Adeudo_Detallado_Predial = value; }
        }

        public DataTable P_Dt_Pasivos_Pago
        {
            get { return Dt_Pasivos_Pago; }
            set { Dt_Pasivos_Pago = value; }
        }

#endregion Varibles Publicas

#region Metodos

        public int Alta_Captura_Pagos()
            {
                return Cls_Ope_Pre_Pagos_Instituciones_Externas_Datos.Alta_Captura_Pagos(this);
            }

        public DataTable Consultar_Cuenta_Predial_ID()
            {
                return Cls_Ope_Pre_Pagos_Instituciones_Externas_Datos.Consultar_Cuenta_Predial_ID(this);
            }

        public DataTable Consultar_Linea_Captura()
            {
                return Cls_Ope_Pre_Pagos_Instituciones_Externas_Datos.Consultar_Linea_Captura(this);
            }

        public DataTable Consultar_Anio_Captura()
            {
                return Cls_Ope_Pre_Pagos_Instituciones_Externas_Datos.Consultar_Anio_Captura();
            }

        public DataTable Consultar_Capturas_Pagos()
            {
                return Cls_Ope_Pre_Pagos_Instituciones_Externas_Datos.Consultar_Capturas_Pagos(this);
            }

        public DataTable Consultar_Detalles_Captura()
            {
                return Cls_Ope_Pre_Pagos_Instituciones_Externas_Datos.Consultar_Detalles_Captura(this);
            }

        public DataTable Consultar_Claves_Ingreso()
            {
                return Cls_Ope_Pre_Pagos_Instituciones_Externas_Datos.Consultar_Claves_Ingreso(this);
            }

        public DataTable Consultar_Propietario()
            {
                return Cls_Ope_Pre_Pagos_Instituciones_Externas_Datos.Consultar_Propietario(this);
            }

#endregion Metodos

    }
}   