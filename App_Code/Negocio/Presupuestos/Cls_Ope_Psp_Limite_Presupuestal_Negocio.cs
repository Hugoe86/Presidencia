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
using Presidencia.Limite_Presupuestal.Datos;
/// <summary>
/// Summary description for Cls_Ope_Psp_Limite_Presupuestal_Negocio
/// </summary>
/// 
namespace Presidencia.Limite_Presupuestal.Negocio
{
    public class Cls_Ope_Psp_Limite_Presupuestal_Negocio
    {
        #region VARIABLES INTERNAS
            private String Dependencia_ID;
            private String Limite_Presupuestal;
            private String Anio_Presupuestal;
            private DataTable Dt_Capitulos;
            private String Fuente_Financiamiento_ID;
            private String Programa_ID;
            private String Accion;
        #endregion

        #region VARIABLES PUBLICAS
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Limite_Presupuestal
            {
                get { return Limite_Presupuestal; }
                set { Limite_Presupuestal = value; }
            }

            public String P_Anio_Presupuestal
            {
                get { return Anio_Presupuestal; }
                set { Anio_Presupuestal = value; }
            }
            public String P_Accion
            {
                get { return Accion; }
                set { Accion = value; }
            }
            public DataTable P_Dt_Capitulos
            {
                get { return Dt_Capitulos; }
                set { Dt_Capitulos = value; }
            }
            public String P_Fuente_Financiamiento_ID
            {
                get { return Fuente_Financiamiento_ID; }
                set { Fuente_Financiamiento_ID = value; }
            }
            public String P_Programa_ID
            {
                get { return Programa_ID; }
                set { Programa_ID = value; }
            }
        #endregion

        #region MÉTODOS
            //Constructor
            public Cls_Ope_Psp_Limite_Presupuestal_Negocio()
            {

            }

            public DataTable Consultar_Unidades_Asignadas()
            {
                return Cls_Ope_Psp_Limite_Presupuestal_Datos.Consultar_Unidades_Asignadas(this);
            }

            public DataTable Consultar_Capitulos_Asignados_A_Unidad_Asignada()
            {
                return Cls_Ope_Psp_Limite_Presupuestal_Datos.Consultar_Capitulos_Asignados_A_Unidad_Asignada(this);
            }

            public DataTable Consultar_Unidades_Responsables_Sin_Asignar()
            {
                return Cls_Ope_Psp_Limite_Presupuestal_Datos.Consultar_Unidades_Responsables_Sin_Asignar(this);
            }

            public String Guardar_Limites()
            {
                return Cls_Ope_Psp_Limite_Presupuestal_Datos.Guardar_Limites(this);
            }

            public Boolean Actualizar_Limites()
            {
                return Cls_Ope_Psp_Limite_Presupuestal_Datos.Actualizar_Limites(this);
            }

            public Boolean Eliminar_Limites()
            {
                return Cls_Ope_Psp_Limite_Presupuestal_Datos.Eliminar_Limites(this);
            }

            public DataTable Consultar_Programa_Unidades_Responsables()
            {
                return Cls_Ope_Psp_Limite_Presupuestal_Datos.Consultar_Programa_Unidades_Responsables(this);
            }
        #endregion
    }
}