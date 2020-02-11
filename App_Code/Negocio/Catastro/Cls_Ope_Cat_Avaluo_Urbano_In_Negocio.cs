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
using Presidencia.Operacion_Cat_Avaluo_Urbano_In.Datos;

/// <summary>
/// Summary description for Cls_Ope_Cat_Avaluo_Urbano_In_Negocio
/// </summary>
namespace Presidencia.Operacion_Cat_Avaluo_Urbano_In.Negocio
{

    public class Cls_Ope_Cat_Avaluo_Urbano_In_Negocio
    {

        #region Variables Internas
        //Cabecera
        private String No_Avaluo;
        private String Anio_Avaluo;
        private String No_Avaluo_Av;
        private String Anio_Avaluo_Av;
        private String Folio;
        private String Motivo_Avaluo_Id;
        private String Cuenta_Predial_Id;
        private String Solicitud_Id;
        private String Cuenta_Predial;
        private String Solicitante;
        private String Observaciones;
        private String Ruta_Fachada_Inmueble;
        private Double Valor_Total_Predio;
        private String Valor_Inpr;
        private String Valor_Inpa;
        private Double Valor_Vr;
        private DateTime Fecha_Autorizo;
        private String Perito_Externo_Id;
        private String Perito_Interno_Id;
        private String No_Oficio;
        private String Estatus;
        private String Region;
        private String Manzana;
        private String Lote;
        //private String No_Oficio;
        //Detalles
        private DataTable Dt_Caracteristicas_Terreno_In;
        private DataTable Dt_Construccion_In;
        private DataTable Dt_Elementos_Construccion_In;
        private DataTable Dt_Calculo_Valor_Construccion_In;
        private DataTable Dt_Calculo_Valor_Terreno_In;
        private DataTable Dt_Clasificacion_Zona_In;
        private DataTable Dt_Servicios_Zona_In;
        private DataTable Dt_Construccion_Dominante_In;
        private DataTable Dt_Observaciones_In;
        private DataTable Dt_Archivos_In;
        private DataTable Dt_Medidas_In;


        #endregion

        #region Variables Publicas

        public String P_No_Avaluo
        {
            get { return No_Avaluo; }
            set { No_Avaluo = value; }
        }

        public String P_Anio_Avaluo
        {
            get { return Anio_Avaluo; }
            set { Anio_Avaluo = value; }
        }
        public String P_No_Avaluo_Av
        {
            get { return No_Avaluo_Av; }
            set { No_Avaluo_Av = value; }
        }

        public String P_Anio_Avaluo_Av
        {
            get { return Anio_Avaluo_Av; }
            set { Anio_Avaluo_Av = value; }
        }

        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }

        public String P_Motivo_Avaluo_Id
        {
            get { return Motivo_Avaluo_Id; }
            set { Motivo_Avaluo_Id = value; }
        }

        public String P_Cuenta_Predial_Id
        {
            get { return Cuenta_Predial_Id; }
            set { Cuenta_Predial_Id = value; }
        }

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }

        public String P_Solicitud_Id
        {
            get { return Solicitud_Id; }
            set { Solicitud_Id = value; }
        }

        public String P_Solicitante
        {
            get { return Solicitante; }
            set { Solicitante = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public String P_Ruta_Fachada_Inmueble
        {
            get { return Ruta_Fachada_Inmueble; }
            set { Ruta_Fachada_Inmueble = value; }
        }

        public Double P_Valor_Total_Predio
        {
            get { return Valor_Total_Predio; }
            set { Valor_Total_Predio = value; }
        }

        public String P_Valor_Inpr
        {
            get { return Valor_Inpr; }
            set { Valor_Inpr = value; }
        }

        public String P_Valor_Inpa
        {
            get { return Valor_Inpa; }
            set { Valor_Inpa = value; }
        }

        public Double P_Valor_Vr
        {
            get { return Valor_Vr; }
            set { Valor_Vr = value; }
        }

        public DateTime P_Fecha_Autorizo
        {
            get { return Fecha_Autorizo; }
            set { Fecha_Autorizo = value; }
        }

        public String P_Perito_Externo_Id
        {
            get { return Perito_Externo_Id; }
            set { Perito_Externo_Id = value; }
        }

        public String P_Perito_Interno_Id
        {
            get { return Perito_Interno_Id; }
            set { Perito_Interno_Id = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_No_Oficio
        {
            get { return No_Oficio; }
            set { No_Oficio = value; }
        }
        public String P_Region
        {
            get { return Region; }
            set { Region = value; }
        }
        public String P_Manzana
        {
            get { return Manzana; }
            set { Manzana = value; }
        }

        public String P_Lote
        {
            get { return Lote; }
            set { Lote = value; }
        }

        public DataTable P_Dt_Caracteristicas_Terreno_In
        {
            get { return Dt_Caracteristicas_Terreno_In; }
            set { Dt_Caracteristicas_Terreno_In = value; }
        }

        public DataTable P_Dt_Construccion_In
        {
            get { return Dt_Construccion_In; }
            set { Dt_Construccion_In = value; }
        }

        public DataTable P_Dt_Elementos_Construccion_In
        {
            get { return Dt_Elementos_Construccion_In; }
            set { Dt_Elementos_Construccion_In = value; }
        }

        public DataTable P_Dt_Calculo_Valor_Construccion_In
        {
            get { return Dt_Calculo_Valor_Construccion_In; }
            set { Dt_Calculo_Valor_Construccion_In = value; }
        }

        public DataTable P_Dt_Calculo_Valor_Terreno_In
        {
            get { return Dt_Calculo_Valor_Terreno_In; }
            set { Dt_Calculo_Valor_Terreno_In = value; }
        }

        public DataTable P_Dt_Clasificacion_Zona_In
        {
            get { return Dt_Clasificacion_Zona_In; }
            set { Dt_Clasificacion_Zona_In = value; }
        }

        public DataTable P_Dt_Servicios_Zona_In
        {
            get { return Dt_Servicios_Zona_In; }
            set { Dt_Servicios_Zona_In = value; }
        }

        public DataTable P_Dt_Construccion_Dominante_In
        {
            get { return Dt_Construccion_Dominante_In; }
            set { Dt_Construccion_Dominante_In = value; }
        }

        public DataTable P_Dt_Observaciones_In
        {
            get { return Dt_Observaciones_In; }
            set { Dt_Observaciones_In = value; }
        }

        public DataTable P_Dt_Archivos_In
        {
            get { return Dt_Archivos_In; }
            set { Dt_Archivos_In = value; }
        }

        public DataTable P_Dt_Medidas_In
        {
            get { return Dt_Medidas_In; }
            set { Dt_Medidas_In = value; }
        }


        #endregion

        #region Metodos

        public Boolean Alta_Valor_Construccion_In()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Alta_Avaluo_Urbano_In(this);
        }

        public Boolean Modificar_Valor_Construccion_In()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Modificar_Avaluo_Urbano_In(this);
        }

        public Boolean Modificar_Estatus_Avaluo_Rustico_Regularizacion()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Modificar_Estatus_Avaluo_Rustico_Regularizacion(this);
        }

        public Boolean Modificar_Observaciones_Avaluo_In()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Modificar_Observaciones_Avaluo_In(this);
        }

        public DataTable Consultar_Avaluo_Urbano_In()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Consultar_Avaluo_Urbano_In(this);
        }

        public DataTable Consultar_Tabla_Valores_Construccion_In()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Consultar_Tabla_Valores_Construccion_In(this);
        }

        public DataTable Consultar_Motivos_Rechazo_Avaluo_In()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Consultar_Motivos_Rechazo_Avaluo_In(this);
        }

        public DataTable Consultar_Tabla_Elementos_Construccion_In()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Consultar_Tabla_Elementos_Construccion_In(this);
        }

        public DataTable Consultar_Tabla_Clasificacion_Zona_In()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Consultar_Tabla_Clasificacion_Zona_In(this);
        }

        public DataTable Consultar_Tabla_Servicios_Zona_In()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Consultar_Tabla_Servicios_Zona_In(this);
        }

        public DataTable Consultar_Tabla_Const_Dominante_In()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Consultar_Tabla_Const_Dominante_In(this);
        }

        public DataTable Consultar_Solicitud_Tramite()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Consultar_Solicitud_Tramite(this);
        }

        public DataTable Consultar_Totales_Inconformidad()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Consultar_Totales_Inconformidad(this);
        }

        public DataTable Consultar_Solicitudes_Inconformidad()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Consultar_Solicitudes_Inconformidad(this);
        }

        public DataTable Consultar_Solicitud_Tramite_Avaluos()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_In_Datos.Consultar_Solicitud_Tramite_Avaluos(this);
        }

        #endregion
    }
}
