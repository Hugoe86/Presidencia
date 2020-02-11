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
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Datos;

namespace Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios
{
    public class Cls_Cat_Nom_Percepciones_Deducciones_Business
    {

        #region VARIABLES INTERNAS
        private String PERCEPCION_DEDUCCION_ID;
        private String NOMBRE;
        private String ESTATUS;
        private String TIPO;
        private String APLICAR;
        private String TIPO_ASIGNACION;
        private String Concepto;
        private Double GRAVABLE;
        private Double PORCENTAJE_GRAVABLE;
        private String COMENTARIOS;
        private String USUARIO_CREO;
        private String FECHA_CREO;
        private String USUARIO_MODIFICO;
        private String FECHA_MODIFICO;
        private String APLICA_IMSS;
        private String CLAVE;
        private String CUENTA_CONTABLE_ID;

        private String Tipo_Nomina_ID;
        private DataTable Dt_Percepciones;
        private DataTable Dt_Deducciones;
        #endregion

        #region VARIABLES PUBLICAS
        public String P_PERCEPCION_DEDUCCION_ID
        {
            get { return PERCEPCION_DEDUCCION_ID; }
            set { PERCEPCION_DEDUCCION_ID = value; }
        }

        public String P_NOMBRE
        {
            get { return NOMBRE; }
            set { NOMBRE = value; }
        }

        public String P_ESTATUS
        {
            get { return ESTATUS; }
            set { ESTATUS = value; }
        }

        public String P_TIPO
        {
            get { return TIPO; }
            set { TIPO = value; }
        }

        public String P_APLICAR
        {
            get { return APLICAR; }
            set { APLICAR = value; }
        }

        public String P_TIPO_ASIGNACION
        {
            get { return TIPO_ASIGNACION; }
            set { TIPO_ASIGNACION = value; }
        }

        public Double P_GRAVABLE
        {
            get { return GRAVABLE; }
            set { GRAVABLE = value; }
        }

        public Double P_PORCENTAJE_GRAVABLE
        {
            get { return PORCENTAJE_GRAVABLE; }
            set { PORCENTAJE_GRAVABLE = value; }
        }

        public String P_COMENTARIOS
        {
            get { return COMENTARIOS; }
            set { COMENTARIOS = value; }
        }

        public String P_USUARIO_CREO
        {
            get { return USUARIO_CREO; }
            set { USUARIO_CREO = value; }
        }

        public String P_FECHA_CREO
        {
            get { return FECHA_CREO; }
            set { FECHA_CREO = value; }
        }

        public String P_USUARIO_MODIFICO
        {
            get { return USUARIO_MODIFICO; }
            set { USUARIO_MODIFICO = value; }
        }

        public String P_FECHA_MODIFICO
        {
            get { return FECHA_MODIFICO; }
            set { FECHA_MODIFICO = value; }
        }

        public String P_Concepto
        {
            get { return Concepto; }
            set { Concepto = value; }
        }

        public String P_APLICA_IMSS {
            get { return APLICA_IMSS; }
            set { APLICA_IMSS = value; }
        }

        public String P_CLAVE {
            get { return CLAVE; }
            set { CLAVE = value; }
        }

        public String P_Tipo_Nomina_ID {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }

        public DataTable P_Dt_Percepciones {
            get { return Dt_Percepciones; }
            set { Dt_Percepciones = value; }
        }

        public DataTable P_Dt_Deducciones
        {
            get { return Dt_Deducciones; }
            set { Dt_Deducciones = value; }
        }

        public String P_CUENTA_CONTABLE_ID
        {
            get { return CUENTA_CONTABLE_ID; }
            set { CUENTA_CONTABLE_ID = value; }
        }
        #endregion

        #region METODOS
                            
        #region METODOS DE CONSULTA
        public DataTable Fill_Grid_Percepciones_Deducciones()
        {
            return Cls_Cat_Nom_Percepciones_Deducciones_Data.Fill_Grid_Percepciones_Deducciones();
        }

        public void Busqueda_Percepcion_Deduccion_Por_Nombre(String Nombre, GridView Grid_Percepciones_Deducciones)
        {
            Cls_Cat_Nom_Percepciones_Deducciones_Data.Busqueda_Percepcion_Deduccion_Por_Nombre(Nombre, Grid_Percepciones_Deducciones);
        }

        public DataTable Busqueda_Percepcion_Deduccion_Por_ID(String Percepcion_Deduccion_ID)
        {
            return Cls_Cat_Nom_Percepciones_Deducciones_Data.Busqueda_Percepcion_Deduccion_Por_ID(Percepcion_Deduccion_ID);
        }
        public DataTable Consulta_Percepciones_Deducciones()
        {
            return Cls_Cat_Nom_Percepciones_Deducciones_Data.Consulta_Percepciones_Deducciones(this);
        }
        public DataTable Consulta_Avanzada_Percepciones_Deducciones()
        {
            return Cls_Cat_Nom_Percepciones_Deducciones_Data.Consulta_Avanzada_Percepciones_Deducciones(this);
        }

        public DataTable Consultar_Percepciones_Deducciones_General() {
            return Cls_Cat_Nom_Percepciones_Deducciones_Data.Consultar_Percepciones_Deducciones_General(this);
        }

        public DataTable Consultar_Percepciones_Deducciones_Tipo_Nomina() {
            return Cls_Cat_Nom_Percepciones_Deducciones_Data.Consultar_Percepciones_Deducciones_Tipo_Nomina(this);
        }

        public DataTable Consultar_Maxima_Clave() {
            return Cls_Cat_Nom_Percepciones_Deducciones_Data.Consultar_Maxima_Clave(this);
        }

        public DataTable Consultar_Cuentas_Contables() {
            return Cls_Cat_Nom_Percepciones_Deducciones_Data.Consultar_Cuentas_Contables(this);
        }
        #endregion

        #region METODOS [ ALTA - MODIFICAR - BAJA ]
        public String Alta_Percepcion_Deduccion()
        {
            return Cls_Cat_Nom_Percepciones_Deducciones_Data.Alta(this);
        }

        public String Actualizar_Percepcion_Deduccion()
        {
            return Cls_Cat_Nom_Percepciones_Deducciones_Data.Actualizar(this);
        }

        public String Baja_Percepcion_Deduccion()
        {
            return Cls_Cat_Nom_Percepciones_Deducciones_Data.Baja(this);
        }

        #endregion

        #endregion

    }//End Class
}//End NameSpace
