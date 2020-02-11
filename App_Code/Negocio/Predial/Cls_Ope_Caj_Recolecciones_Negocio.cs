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
using Presidencia.Recoleccion_Caja.Datos;

namespace Presidencia.Recoleccion_Caja.Negocio
{
    public class Cls_Ope_Caj_Recolecciones_Negocio
    {
        public Cls_Ope_Caj_Recolecciones_Negocio()
        {
        }
        #region (Variables Internas)
            private String No_Recoleccion;
            private String Caja_ID;
            private String Empleado_ID;
            private String No_Turno;
            private Decimal Monto_Recolectado;
            private Decimal Monto_Tarjeta;
            private Decimal Monto_Cheque;
            private Decimal Monto_Transferencia;
            private Int32 Conteo_Tarjeta;
            private Int32 Conteo_Cheque;
            private Int32 Conteo_Transferencia;
            private DateTime Fecha_Busqueda;
            private String Recibe_Efectivo;
            private Int32 Numero_Recoleccion;
            private String Nombre_Usuario;
            //Denominaciones de Billetes
            private Int32 Biillete_1000;
            private Int32 Biillete_500;
            private Int32 Biillete_200;
            private Int32 Biillete_100;            
            private Int32 Biillete_50;
            private Int32 Biillete_20;
            //Denominación  de Monedas
            private Int32 Moneda_20;
            private Int32 Moneda_10;
            private Int32 Moneda_5;
            private Int32 Moneda_2;
            private Int32 Moneda_1;
            private Int32 Moneda_050;
            private Int32 Moneda_020;
            private Int32 Moneda_010;
        #endregion
        #region(Variables Publicas)
            //Variables Públicas Generales
            public String P_No_Recoleccion
            {
                get { return No_Recoleccion; }
                set { No_Recoleccion = value; }
            }
            public String P_Caja_ID
            {
                get { return Caja_ID; }
                set { Caja_ID = value; }
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
            public Decimal P_Monto_Recolectado
            {
                get { return Monto_Recolectado; }
                set { Monto_Recolectado = value; }
            }
            public Decimal P_Monto_Tarjeta
            {
                get { return Monto_Tarjeta; }
                set { Monto_Tarjeta = value; }
            }
            public Decimal P_Monto_Cheque
            {
                get { return Monto_Cheque; }
                set { Monto_Cheque = value; }
            }
            public Decimal P_Monto_Transferencia
            {
                get { return Monto_Transferencia; }
                set { Monto_Transferencia = value; }
            }
            public Int32 P_Conteo_Tarjeta
            {
                get { return Conteo_Tarjeta; }
                set { Conteo_Tarjeta = value; }
            }
            public Int32 P_Conteo_Cheque
            {
                get { return Conteo_Cheque; }
                set { Conteo_Cheque = value; }
            }
            public Int32 P_Conteo_Transferencia
            {
                get { return Conteo_Transferencia; }
                set { Conteo_Transferencia = value; }
            }
            public DateTime P_Fecha_Busqueda
            {
                get { return Fecha_Busqueda; }
                set { Fecha_Busqueda = value; }
            }
            public String P_Recibe_Efectivo
            {
                get { return Recibe_Efectivo; }
                set { Recibe_Efectivo = value; }
            }
            public Int32 P_Numero_Recoleccion
            {
                get { return Numero_Recoleccion; }
                set { Numero_Recoleccion = value; }
            }
            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }
            //Variables Denominaciones de Billetes
            public Int32 P_Biillete_1000
            {
                get { return Biillete_1000; }
                set { Biillete_1000 = value; }
            }
            public Int32 P_Biillete_500
            {
                get { return Biillete_500; }
                set { Biillete_500 = value; }
            }
            public Int32 P_Biillete_200
            {
                get { return Biillete_200; }
                set { Biillete_200 = value; }
            }
            public Int32 P_Biillete_100
            {
                get { return Biillete_100; }
                set { Biillete_100 = value; }
            }
            public Int32 P_Biillete_50
            {
                get { return Biillete_50; }
                set { Biillete_50 = value; }
            }
            public Int32 P_Biillete_20
            {
                get { return Biillete_20; }
                set { Biillete_20 = value; }
            }
            //Variables Denominaciones de Monedas
            public Int32 P_Moneda_20
            {
                get { return Moneda_20; }
                set { Moneda_20 = value; }
            }
            public Int32 P_Moneda_10
            {
                get { return Moneda_10; }
                set { Moneda_10 = value; }
            }
            public Int32 P_Moneda_5
            {
                get { return Moneda_5; }
                set { Moneda_5 = value; }
            }
            public Int32 P_Moneda_1
            {
                get { return Moneda_1; }
                set { Moneda_1 = value; }
            }
            public Int32 P_Moneda_2
            {
                get { return Moneda_2; }
                set { Moneda_2 = value; }
            }
            public Int32 P_Moneda_050
            {
                get { return Moneda_050; }
                set { Moneda_050 = value; }
            }
            public Int32 P_Moneda_020
            {
                get { return Moneda_020; }
                set { Moneda_020 = value; }
            }
            public Int32 P_Moneda_010
            {
                get { return Moneda_010; }
                set { Moneda_010 = value; }
            }
        #endregion
        #region (Metodos)
            public void Alta_Recoleccion()
            {
                Cls_Ope_Caj_Recolecciones_Datos.Alta_Recoleccion(this);
            }
            public DataTable Consulta_Caja_Empleado()
            {
                return Cls_Ope_Caj_Recolecciones_Datos.Consulta_Caja_Empleado(this);
            }
            public DataTable Consulta_Recolecciones()
            {
                return Cls_Ope_Caj_Recolecciones_Datos.Consulta_Recolecciones(this);
            }
            public DataTable Consulta_Detalles_Recolecciones()
            {
                return Cls_Ope_Caj_Recolecciones_Datos.Consulta_Detalles_Recolecciones(this);
            }
            public DataTable Consultar_Caja_Total_Cobrado()
            {
                return Cls_Ope_Caj_Recolecciones_Datos.Consultar_Caja_Total_Cobrado(this);
            }
        #endregion
    }
}