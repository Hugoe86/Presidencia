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
using Presidencia.Operacion_Predial_Dias_Inhabiles.Datos;

namespace Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio
{
    public class Cls_Ope_Pre_Dias_Inhabiles_Negocio
    {
        #region (Variables Internas)
            private String No_Dia_Inhabil;
            private String Anio;
            private String Dia_ID;
            private DateTime Fecha;
            private String Descripcion;
            private String Estatus;
            private String Motivo;
            private String Nombre_Usuario;
            private String Fecha_Consulta;
            private String Fecha_Inicial_Busqueda;
            private String Fecha_Final_Busqueda;
        #endregion
        #region (Variables Publicas)
            public String P_No_Dia_Inhabil
            {
                get { return No_Dia_Inhabil; }
                set { No_Dia_Inhabil = value; }
            }
            public String P_Anio
            {
                get { return Anio; }
                set { Anio = value; }
            } 
            public String P_Dia_ID
            {
                get { return Dia_ID; }
                set { Dia_ID = value; }
            }
            public DateTime P_Fecha
            {
                get { return Fecha; }
                set { Fecha = value; }
            }
            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Motivo
            {
                get { return Motivo; }
                set { Motivo = value; }
            }
            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }
            public String P_Fecha_Consulta
            {
                get { return Fecha_Consulta; }
                set { Fecha_Consulta = value; }
            }
            public String P_Fecha_Inicial_Busqueda
            {
                get { return Fecha_Inicial_Busqueda; }
                set { Fecha_Inicial_Busqueda = value; }
            }
            public String P_Fecha_Final_Busqueda
            {
                get { return Fecha_Final_Busqueda; }
                set { Fecha_Final_Busqueda = value; }
            }
        #endregion
        #region(Metodos)
            public void Alta_Dia_Inhabil()
            {
                Cls_Ope_Pre_Dias_Inhabiles_Datos.Alta_Dia_Inhabil(this);
            }
            public void Modiicar_Dia_Inhabil()
            {
                Cls_Ope_Pre_Dias_Inhabiles_Datos.Modiicar_Dia_Inhabil(this);
            }
            public void Eliminar_Dia_Inhabil()
            {
                Cls_Ope_Pre_Dias_Inhabiles_Datos.Eliminar_Dia_Inhabil(this);
            }
            public DataTable Consulta_Dias_Inhabiles()
            {
                return Cls_Ope_Pre_Dias_Inhabiles_Datos.Consulta_Dias_Inhabiles(this);
            }
            public DataTable Consultar_Dias()
            {
                return Cls_Ope_Pre_Dias_Inhabiles_Datos.Consultar_Dias(this);
            }
            public DateTime Calcular_Fecha(String Fecha, String Dias)
            {
                return Cls_Ope_Pre_Dias_Inhabiles_Datos.Calcular_Fecha(Fecha, Dias);
            }
        #endregion
    }
}