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
using Presidencia.Operaciones_Apertura_Turnos.Datos;

namespace Presidencia.Operaciones_Apertura_Turnos.Negocio
{
    public class Cls_Ope_Pre_Apertura_Turno_Negocio
    {
        #region Variables Internas
            private String No_Turno;
            private String No_Turno_Dia;
            private String Caja_Id;
            private String Recibo_Inicial;            
            private String Contador_Recibo;
            private String Aplicacion_Pago;
            private Double Fondo_Inicial;
            private String Hora_Apertura;
            private String Fecha_Turno;
            private String Foranea;
            private String Modulo;
            private String Filtro;
            private String Estatus;
            private String Empleado_ID;            
            private String Nombre_Empleado;
            private String Nombre_Recibe;
            private String ReApertua_Nombre_Autorizo;
            private String ReApertua_Observaciones_Autorizo;
            //Datos del cierre del turno
            private String Hora_Cierre;
            private String Fecha_Cierre;
            private String Recibo_Final;
            private Decimal Total_Bancos;
            private Decimal Total_Cheques;
            private Decimal Total_Trasnferencias;
            private Decimal Total_Recolectado;
            private Decimal Total_Efectivo_Sistema;
            private Decimal Total_Efectivo_Caja;
            private Int32 Conteo_Bancos;
            private Int32 Conteo_Cheques;
            private Int32 Conteo_Transferencias;
            private Decimal Total_Recolectado_Bancos;
            private Decimal Total_Recolectado_Cheques;
            private Decimal Total_Recolectado_Trasnferencias;
            private Int32 Conteo_Recolectado_Bancos;
            private Int32 Conteo_Recolectado_Cheques;
            private Int32 Conteo_Recolectado_Transferencias;
            private Decimal Sobrante;
            private Decimal Faltante;                      
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
        #region Variables Publicas
            public String P_No_Turno
            {
                get { return No_Turno; }
                set { No_Turno = value; }
            }
            public String P_No_Turno_Dia
            {
                get { return No_Turno_Dia; }
                set { No_Turno_Dia = value; }
            }
            public String P_Caja_Id
            {
                get { return Caja_Id; }
                set { Caja_Id = value; }
            }
            public String P_Recibo_Inicial
            {
                get { return Recibo_Inicial; }
                set { Recibo_Inicial = value; }
            }
            public String P_Contador_Recibo
            {
                get { return Contador_Recibo; }
                set { Contador_Recibo = value; }
            }
            public String P_Aplicacion_Pago
            {
                get { return Aplicacion_Pago; }
                set { Aplicacion_Pago = value; }
            }
            public Double P_Fondo_Inicial
            {
                get { return Fondo_Inicial; }
                set { Fondo_Inicial = value; }
            }
            public String P_Hora_Apertura
            {
                get { return Hora_Apertura; }
                set { Hora_Apertura = value; }
            }
            public String P_Hora_Cierre
            {
                get { return Hora_Cierre; }
                set { Hora_Cierre = value; }
            }
            public String P_Fecha_Turno
            {
                get { return Fecha_Turno; }
                set { Fecha_Turno = value; }
            }
            public String P_Foranea
            {
                get { return Foranea; }
                set { Foranea = value; }
            }
            public String P_Modulo
            {
                get { return Modulo; }
                set { Modulo = value; }
            }
            public String P_Filtro
            {
                get { return Filtro; }
                set { Filtro = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_Nombre_Empleado
            {
                get { return Nombre_Empleado; }
                set { Nombre_Empleado = value; }
            }
            public String P_Nombre_Recibe
            {
                get { return Nombre_Recibe; }
                set { Nombre_Recibe = value; }
            }
            //Datos del cierre del turno
            public String P_Fecha_Cierre
            {
                get { return Fecha_Cierre; }
                set { Fecha_Cierre = value; }
            }
            public String P_Recibo_Final
            {
                get { return Recibo_Final; }
                set { Recibo_Final = value; }
            }
            public Decimal P_Total_Bancos
            {
                get { return Total_Bancos; }
                set { Total_Bancos = value; }
            }
            public Decimal P_Total_Cheques
            {
                get { return Total_Cheques; }
                set { Total_Cheques = value; }
            }
            public Decimal P_Total_Trasnferencias
            {
                get { return Total_Trasnferencias; }
                set { Total_Trasnferencias = value; }
            }
            public Decimal P_Total_Recolectado
            {
                get { return Total_Recolectado; }
                set { Total_Recolectado = value; }
            }
            public Decimal P_Total_Efectivo_Sistema
            {
                get { return Total_Efectivo_Sistema; }
                set { Total_Efectivo_Sistema = value; }
            }
            public Decimal P_Total_Efectivo_Caja
            {
                get { return Total_Efectivo_Caja; }
                set { Total_Efectivo_Caja = value; }
            }
            public Int32 P_Conteo_Bancos
            {
                get { return Conteo_Bancos; }
                set { Conteo_Bancos = value; }
            }
            public Int32 P_Conteo_Cheques
            {
                get { return Conteo_Cheques; }
                set { Conteo_Cheques = value; }
            }
            public Int32 P_Conteo_Transferencias
            {
                get { return Conteo_Transferencias; }
                set { Conteo_Transferencias = value; }
            }
            public Decimal P_Total_Recolectado_Bancos
            {
                get { return Total_Recolectado_Bancos; }
                set { Total_Recolectado_Bancos = value; }
            }
            public Decimal P_Total_Recolectado_Cheques
            {
                get { return Total_Recolectado_Cheques; }
                set { Total_Recolectado_Cheques = value; }
            }
            public Decimal P_Total_Recolectado_Trasnferencias
            {
                get { return Total_Recolectado_Trasnferencias; }
                set { Total_Recolectado_Trasnferencias = value; }
            }
            public Int32 P_Conteo_Recolectado_Bancos
            {
                get { return Conteo_Recolectado_Bancos; }
                set { Conteo_Recolectado_Bancos = value; }
            }
            public Int32 P_Conteo_Recolectado_Cheques
            {
                get { return Conteo_Recolectado_Cheques; }
                set { Conteo_Recolectado_Cheques = value; }
            }
            public Int32 P_Conteo_Recolectado_Transferencias
            {
                get { return Conteo_Recolectado_Transferencias; }
                set { Conteo_Recolectado_Transferencias = value; }
            }
            public Decimal P_Sobrante
            {
                get { return Sobrante; }
                set { Sobrante = value; }
            }
            public Decimal P_Faltante
            {
                get { return Faltante; }
                set { Faltante = value; }
            }  
            //Datos de reapertura
            public String P_ReApertua_Nombre_Autorizo
            {
                get { return ReApertua_Nombre_Autorizo; }
                set { ReApertua_Nombre_Autorizo = value; }
            }
            public String P_ReApertua_Observaciones_Autorizo
            {
                get { return ReApertua_Observaciones_Autorizo; }
                set { ReApertua_Observaciones_Autorizo = value; }
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
        #region Metodos
            public void Alta_Apertura_Turno()
            {
                Cls_Ope_Pre_Apertura_Turnos_Datos.Alta_Apertura_Turno(this);
            }
            public void Modifica_Apertura_Turno()
            {
                Cls_Ope_Pre_Apertura_Turnos_Datos.Modifica_Apertura_Turno(this);
            }
            public void Modificar_Datos_Turno()
            {
                Cls_Ope_Pre_Apertura_Turnos_Datos.Modificar_Datos_Turno(this);
            }
            public void Modificar_Turno_Reapertura()
            {
                Cls_Ope_Pre_Apertura_Turnos_Datos.Modificar_Turno_Reapertura(this);
            }
            public void Autoriza_ReAPertura_Turno()
            {
                Cls_Ope_Pre_Apertura_Turnos_Datos.Autoriza_ReAPertura_Turno(this);
            }
            public DataTable Consulta_Datos_Generales_Turno()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consulta_Datos_Generales_Turno(this);
            }
            public DataTable Consulta_Datos_Turno_Empleado()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consulta_Datos_Turno_Empleado(this);
            }
            public DataTable Consulta_Turno_Abierto_Caja()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consulta_Turno_Abierto_Caja(this);
            }
            public DataTable Consulta_Turno_Abierto_Dia()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consulta_Turno_Abierto_Dia(this);
            }
            public DataTable Consultar_Apertura()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consultar_Turnos(this);
            }
            public DataTable Consultar_Datos_Turno()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consultar_Datos_Turno(this);
            }
            public DataTable Consultar_Formas_Pago_Turno()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consultar_Formas_Pago_Turno(this);
            }
            public DataTable Consultar_Monto_Recolectado()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consultar_Monto_Recolectado(this);
            }
            public DataTable Consultar_Monto_Recolectado_Detalles()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consultar_Monto_Recolectado_Detalles(this);
            }
            public DataTable Consulta_Detalles_Turno()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consulta_Detalles_Turno(this);
            }
            public DataTable Consulta_Datos_Generales_Turno_Fecha()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consulta_Datos_Generales_Turno_Fecha(this);
            }
            public String Consulta_Datos_Cierre_Dia()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consulta_Datos_Cierre_Dia(this);
            }
            public Boolean Valida_Recibo_Inicial()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Valida_Recibo_Inicial(this);
            }
            public String Consulta_Ultimo_Folio()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consulta_Ultimo_Folio(this);
            }
            public DataTable Consulta_Turnos_Cerrados()
            {
                return Cls_Ope_Pre_Apertura_Turnos_Datos.Consulta_Turnos_Cerrados(this);
            }
        #endregion
    }
}