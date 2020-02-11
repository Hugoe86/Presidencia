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
using Presidencia.Operacion_Fechas_Aplicacion.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Fechas_Aplicacion_Negocio
/// </summary>

namespace Presidencia.Operacion_Fechas_Aplicacion.Negocio
{
    public class Cls_Ope_Pre_Fechas_Aplicacion_Negocio
    {
        #region Variables Internas
            private String Fecha_Aplicacion_ID;
            private String Fecha_Alta;
            private String Estatus;
            private String Fecha_Movimiento;
            private String Fecha_Aplicacion;
            private String Motivo;
            private String Filtro;
        #endregion 
        #region Variables Publicas
            public String P_Fecha_Aplicacion_ID
            {
                get { return Fecha_Aplicacion_ID; }
                set { Fecha_Aplicacion_ID = value; }
            }
            public String P_Fecha_Alta
            {
                get { return Fecha_Alta; }
                set { Fecha_Alta = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Fecha_Movimiento
            {
                get { return Fecha_Movimiento; }
                set { Fecha_Movimiento = value; }
            }
            public String P_Fecha_Aplicacion
            {
                get { return Fecha_Aplicacion; }
                set { Fecha_Aplicacion = value; }
            }
            public String P_Motivo
            {
                get { return Motivo; }
                set { Motivo = value; }
            }
            public String P_Filtro
            {
                get { return Filtro; }
                set { Filtro = value; }
            }
        #endregion
        #region Metodos
            public void Alta_Fecha()
            {
                Cls_Ope_Pre_Fechas_Aplicacion_Datos.Alta_Fecha(this);
            }
            public void Modificar_Fecha()
            {
                Cls_Ope_Pre_Fechas_Aplicacion_Datos.Modificar_Fecha(this);
            }
            public Cls_Ope_Pre_Fechas_Aplicacion_Negocio Consultar_Datos_Fecha()
            {
                return Cls_Ope_Pre_Fechas_Aplicacion_Datos.Consultar_Datos_Fechas(this);
            }
            public Cls_Ope_Pre_Fechas_Aplicacion_Negocio Obtener_Fecha_Aplicacion()
            {
                return Cls_Ope_Pre_Fechas_Aplicacion_Datos.Obtener_Fecha_Aplicacion(this);
            }
            public DataTable Consultar_Fechas()
            {
                return Cls_Ope_Pre_Fechas_Aplicacion_Datos.Consultar_Fechas(this);
            }
            public Boolean Consultar_Dias_Inhabiles()
            {
                return Cls_Ope_Pre_Fechas_Aplicacion_Datos.Consultar_Fechas_Inhabiles(this.Fecha_Aplicacion);
            }
            public DataTable Consulta_Fecha_Repetida()
            {
                return Cls_Ope_Pre_Fechas_Aplicacion_Datos.Consulta_Fecha_Repetida(this);
            }
        #endregion
    }
}