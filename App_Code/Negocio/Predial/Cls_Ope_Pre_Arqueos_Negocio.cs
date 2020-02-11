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
using Presidencia.Operacion_Arqueos.Datos;

namespace Presidencia.Operacion_Arqueos.Negocio{
    
    public class Cls_Ope_Pre_Arqueos_Negocio
    {
        #region Variables Internas

        //  Variables publicas para la tabla de Recolecciones
        private String Caja_ID;
        private String Cajero_ID;
        private String Usuario;
        private String Recoleccion_ID;
        private String Num_Recoleccion;
        private String Mnt_Recoleccion;
        private String Fecha;
        private string Dependencia_ID;

        //  Variables publicas para la Autenticación
        private String No_Empleado;
        private String Password;

        //  Variables publicas para la tabla de Arqueos
        private String No_Arqueo;
        private String No_Turno;
        private String Realizo;

        private Double Total_Cobrado;
        private Double Total_Recolectado;
        private Double Fondo_Inicial;
        private Double Total_Efectivo;
        private Double Total_Tarjeta;
        private Double Total_Cheques;
        private Double Total_Transferencia;
        private Double Diferencia;
        private Double Arqueo;

        private String Comentarios;

        // Variables publicas para la tabla de Arqueos Detalles
        private Int32 Denom_10_Cent;
        private Int32 Denom_20_Cent;
        private Int32 Denom_50_Cent;
        private Int32 Denom_1_Peso;
        private Int32 Denom_2_Pesos;
        private Int32 Denom_5_Pesos;
        private Int32 Denom_10_Pesos;
        private Int32 Denom_20_Pesos;
        private Int32 Denom_50_Pesos;
        private Int32 Denom_100_Pesos;
        private Int32 Denom_200_Pesos;
        private Int32 Denom_500_Pesos;
        private Int32 Denom_1000_Pesos;
        private Double Monto_Total;

        #endregion

        #region Variables Publicas

        public String P_Num_Recoleccion
        {
            get { return Num_Recoleccion; }
            set { Num_Recoleccion = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID ; }
            set { Dependencia_ID = value; }
        }

        public String P_Recoleccion_ID
        {
            get { return Recoleccion_ID; }
            set { Recoleccion_ID = value; }
        }

        public String P_Cajero_ID
        {
            get { return Cajero_ID; }
            set { Cajero_ID = value; }
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

        public String P_Mnt_Recoleccion
        {
            get { return Mnt_Recoleccion; }
            set { Mnt_Recoleccion = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_No_Arqueo
        {
            get { return No_Arqueo; }
            set { No_Arqueo = value; }
        }

        public String P_No_Turno
        {
            get { return No_Turno; }
            set { No_Turno = value; }
        }

        public String P_Realizo
        {
            get { return Realizo; }
            set { Realizo = value; }
        }

        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }

        public String P_Password
        {
            get { return Password; }
            set { Password = value; }
        }

        public Double P_Total_Cobrado
        {
            get { return Total_Cobrado; }
            set { Total_Cobrado = value; }
        }

        public Double P_Total_Recolectado
        {
            get { return Total_Recolectado; }
            set { Total_Recolectado = value; }
        }

        public Double P_Fondo_Inicial
        {
            get { return Fondo_Inicial; }
            set { Fondo_Inicial = value; }
        }

        public Double P_Total_Efectivo
        {
            get { return Total_Efectivo; }
            set { Total_Efectivo = value; }
        }

        public Double P_Total_Tarjeta
        {
            get { return Total_Tarjeta; }
            set { Total_Tarjeta = value; }
        }

        public Double P_Total_Cheques
        {
            get { return Total_Cheques; }
            set { Total_Cheques = value; }
        }

        public Double P_Total_Transferencias
        {
            get { return Total_Transferencia; }
            set { Total_Transferencia = value; }
        }

        public Double P_Diferencia
        {
            get { return Diferencia; }
            set { Diferencia = value; }
        }

        public Double P_Arqueo
        {
            get { return Arqueo; }
            set { Arqueo = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public Int32 P_Denom_10_Cent
        {
            get { return Denom_10_Cent; }
            set { Denom_10_Cent = value; }
        }

        public Int32 P_Denom_20_Cent
        {
            get { return Denom_20_Cent; }
            set { Denom_20_Cent = value; }
        }

        public Int32 P_Denom_50_Cent
        {
            get { return Denom_50_Cent; }
            set { Denom_50_Cent = value; }
        }

        public Int32 P_Denom_1_Peso
        {
            get { return Denom_1_Peso; }
            set { Denom_1_Peso = value; }
        }

        public Int32 P_Denom_2_Pesos
        {
            get { return Denom_2_Pesos; }
            set { Denom_2_Pesos = value; }
        }

        public Int32 P_Denom_5_Pesos
        {
            get { return Denom_5_Pesos; }
            set { Denom_5_Pesos = value; }
        }

        public Int32 P_Denom_10_Pesos
        {
            get { return Denom_10_Pesos; }
            set { Denom_10_Pesos = value; }
        }

        public Int32 P_Denom_20_Pesos
        {
            get { return Denom_20_Pesos; }
            set { Denom_20_Pesos = value; }
        }

        public Int32 P_Denom_50_Pesos
        {
            get { return Denom_50_Pesos; }
            set { Denom_50_Pesos = value; }
        }

        public Int32 P_Denom_100_Pesos
        {
            get { return Denom_100_Pesos; }
            set { Denom_100_Pesos = value; }
        }

        public Int32 P_Denom_200_Pesos
        {
            get { return Denom_200_Pesos; }
            set { Denom_200_Pesos = value; }
        }

        public Int32 P_Denom_500_Pesos
        {
            get { return Denom_500_Pesos; }
            set { Denom_500_Pesos = value; }
        }

        public Int32 P_Denom_1000_Pesos
        {
            get { return Denom_1000_Pesos; }
            set { Denom_1000_Pesos = value; }
        }

        public Double P_Monto_Total
        {
            get { return Monto_Total; }
            set { Monto_Total = value; }
        }

        #endregion

        #region Metodos

        public string Alta_Arqueo()
        {
           return Cls_Ope_Pre_Arqueos_Datos.Alta_Arqueo(this);
        }

        public Double Consultar_Ajuste()
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Ajuste_Tarifario(this);
        }

        public DataSet Consultar_Datos_Arqueo_Todos()
        {
           return Cls_Ope_Pre_Arqueos_Datos.Consultar_Datos_Arqueos_Todos(this);
        }

        public DataSet Consultar_Formas_Pago()
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Formas_Pago(this);
        }

        public String Consultar_Cajero_General()
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Cajero_General();
        }

        public DataSet Consultar_Datos_Recibos()
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Recibo_Inicial_Final(this);
        }

        public string Consultar_Dependencia()
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Dependencia(this);
        }

        public void Modificar_Arqueo()
        {
            Cls_Ope_Pre_Arqueos_Datos.Modificar_Arqueo(this);
        }

        public DataSet Consultar_Autenticacion()
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Autenticacion(this);
        }

        public DataSet Consultar_Total_Recolectado()
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Total_Recolectado(this);
        }

        public DataTable Consultar_Total_Cobrado()
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Total_Cobrado(this);
        }

        public DataSet Consultar_Fondo_Inicial()
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Fondo_Inicial(this);
        }

        public DataTable Consultar_Datos_Arqueos() 
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Datos_Arqueos(this);
        }

        public DataSet Consultar_Turnos()
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Turnos(this);
        }

        public DataTable Consultar_Arqueos()
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Arqueos(this);
        }

        public DataTable Consultar_Arqueos_Busqueda() //Busqueda
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Arqueos_Busqueda(this);
        }

        public DataTable Consultar_Recolecciones()
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Recolecciones(this);
        }

        public String Obtener_Numero_Recoleccion() 
        {
            return Cls_Ope_Pre_Arqueos_Datos.Obtener_Clave_Maxima();   
        }

        public DataTable Llenar_Combo_Caja() 
        {
            return Cls_Ope_Pre_Arqueos_Datos.Consultar_Numeros_Caja();
        }

        #endregion
    }
}