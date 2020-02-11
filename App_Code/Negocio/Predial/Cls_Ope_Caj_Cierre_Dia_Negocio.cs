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
using Presidencia.Operacion_Caj_Cierre_Dia.Datos;

/// <summary>
/// Summary description for Cls_Ope_Caj_Cierre_Dia_Negocio
/// </summary>

namespace Presidencia.Operacion_Caj_Cierre_Dia.Negocio
{
    public class Cls_Ope_Caj_Cierre_Dia_Negocio
    {
        #region Variables Internas
            private String No_Cierre_Dia;
            private String Modulo_Id;
            private String Caja_Id;
            private String Modulo;
            private String Estatus;
            private DateTime Fecha_Cierre_Dia;
            private DateTime Fecha_Reapertura_Dia;
            private String Usuario;
            private String filtro;
        #endregion
        #region Variables Publicas
            public String P_No_Cierre_Dia
            {
                get { return No_Cierre_Dia; }
                set { No_Cierre_Dia = value; }
            }
            public String P_Filtro
            {
                get { return filtro; }
                set { filtro = value; }
            }
            public String P_Modulo_Id
            {
                get { return Modulo_Id; }
                set { Modulo_Id = value; }
            }
            public String P_Modulo
            {
                get { return Modulo; }
                set { Modulo = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public DateTime P_Fecha_Cierre_Dia
            {
                get { return Fecha_Cierre_Dia; }
                set { Fecha_Cierre_Dia = value; }
            }
            public DateTime P_Fecha_Reapertura_Dia
            {
                get { return Fecha_Reapertura_Dia; }
                set { Fecha_Reapertura_Dia = value; }
            }
            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }
            public String P_Caja_Id
            {
                get { return Caja_Id; }
                set { Caja_Id = value; }
            }
        #endregion
        #region Metodos
            public void Alta_Cierre()
            {
                Cls_Ope_Caj_Cierre_Dia_Datos.Alta_Cierre(this);
            }
            public void Modificar_Cierre()
            {
                Cls_Ope_Caj_Cierre_Dia_Datos.Modificar_Cierre(this);
            }
            public void Modificar_Cierre_Dia()
            {
                Cls_Ope_Caj_Cierre_Dia_Datos.Modificar_Cierre_Dia(this);
            }
            public Cls_Ope_Caj_Cierre_Dia_Negocio Consultar_Datos_Cierre()
            {
                return Cls_Ope_Caj_Cierre_Dia_Datos.Consultar_Datos_Cierres(this);
            }
            public DataTable Consultar_Cierre()
            {
                return Cls_Ope_Caj_Cierre_Dia_Datos.Consultar_Cierres(this);
            }
            public DataTable Consultar_Movimientos_Cajas()
            {
                return Cls_Ope_Caj_Cierre_Dia_Datos.Consultar_Movimientos_Cajas(this);
            }
            public DataTable Consulta_Datos_Cierre_Dia()
            {
                return Cls_Ope_Caj_Cierre_Dia_Datos.Consulta_Datos_Cierre_Dia(this);
            }
            public DataTable Consulta_Turnos_Abiertos()
            {
                return Cls_Ope_Caj_Cierre_Dia_Datos.Consulta_Turnos_Abiertos(this);
            }
            public DataTable Consultar_Formas_Pago_Turno_Dia()
            {
                return Cls_Ope_Caj_Cierre_Dia_Datos.Consultar_Formas_Pago_Turno_Dia(this);
            }
        #endregion
    }
}