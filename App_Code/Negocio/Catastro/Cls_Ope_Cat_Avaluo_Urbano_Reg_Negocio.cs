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
using Presidencia.Operacion_Cat_Avaluo_Urbano_Reg.Datos;

/// <summary>
/// Summary description for Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio
/// </summary>
/// 
namespace Presidencia.Operacion_Cat_Avaluo_Urbano_Reg.Negocio
{
    public class Cls_Ope_Cat_Avaluo_Urbano_Reg_Negocio
    {
        #region Variables Internas
        //Cabecera
        private String No_Avaluo;
        private String Anio_Avaluo;
        private String Folio;
        private String Motivo_Avaluo_Id;
        private String Cuenta_Predial_Id;
        private String Cuenta_Predial;//cambio 10
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
        private String Solicitud_Id;
        private String Importe_Avaluo;
        private String Generar_Cobro;
        //Detalles
        private DataTable Dt_Caracteristicas_Terreno_Re;
        private DataTable Dt_Construccion_Re;
        private DataTable Dt_Elementos_Construccion_Re;
        private DataTable Dt_Calculo_Valor_Construccion_Re;
        private DataTable Dt_Calculo_Valor_Terreno_Re;
        private DataTable Dt_Clasificacion_Zona_Re;
        private DataTable Dt_Servicios_Zona_Re;
        private DataTable Dt_Construccion_Dominante_Re;
        private DataTable Dt_Observaciones_Re;
        private DataTable Dt_Archivos_Re;
        private DataTable Dt_Medidas_Re;


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
            set { Cuenta_Predial = value; }//cambio 10
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

        public String P_Importe_Avaluo
        {
            get { return Importe_Avaluo; }
            set { Importe_Avaluo = value; }
        }

        public String P_Solicitud_Id
        {
            get { return Solicitud_Id; }
            set { Solicitud_Id = value; }
        }

        public String P_Generar_Cobro
        {
            get { return Generar_Cobro; }
            set { Generar_Cobro = value; }
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

        public DataTable P_Dt_Caracteristicas_Terreno_Re
        {
            get { return Dt_Caracteristicas_Terreno_Re; }
            set { Dt_Caracteristicas_Terreno_Re = value; }
        }

        public DataTable P_Dt_Construccion_Re
        {
            get { return Dt_Construccion_Re; }
            set { Dt_Construccion_Re = value; }
        }

        public DataTable P_Dt_Elementos_Construccion_Re
        {
            get { return Dt_Elementos_Construccion_Re; }
            set { Dt_Elementos_Construccion_Re = value; }
        }

        public DataTable P_Dt_Calculo_Valor_Construccion_Re
        {
            get { return Dt_Calculo_Valor_Construccion_Re; }
            set { Dt_Calculo_Valor_Construccion_Re = value; }
        }

        public DataTable P_Dt_Calculo_Valor_Terreno_Re
        {
            get { return Dt_Calculo_Valor_Terreno_Re; }
            set { Dt_Calculo_Valor_Terreno_Re = value; }
        }

        public DataTable P_Dt_Clasificacion_Zona_Re
        {
            get { return Dt_Clasificacion_Zona_Re; }
            set { Dt_Clasificacion_Zona_Re = value; }
        }

        public DataTable P_Dt_Servicios_Zona_Re
        {
            get { return Dt_Servicios_Zona_Re; }
            set { Dt_Servicios_Zona_Re = value; }
        }

        public DataTable P_Dt_Construccion_Dominante_Re
        {
            get { return Dt_Construccion_Dominante_Re; }
            set { Dt_Construccion_Dominante_Re = value; }
        }

        public DataTable P_Dt_Observaciones_Re
        {
            get { return Dt_Observaciones_Re; }
            set { Dt_Observaciones_Re = value; }
        }
        public DataTable P_Dt_Archivos_Re
        {
            get { return Dt_Archivos_Re; }
            set { Dt_Archivos_Re = value; }
        }

        public DataTable P_Dt_Medidas_Re
        {
            get { return Dt_Medidas_Re; }
            set { Dt_Medidas_Re = value; }
        }
        
        #endregion

        #region Metodos

        public Boolean Alta_Valor_Construccion_Re()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Alta_Avaluo_Urbano_Re(this);
        }

        public Boolean Modificar_Valor_Construccion_Re()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Modificar_Avaluo_Urbano_Re(this);
        }

        public Boolean Modificar_Observaciones_Avaluo_Re()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Modificar_Observaciones_Avaluo_Re(this);
        }

        public DataTable Consultar_Avaluo_Urbano_Re()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Consultar_Avaluo_Urbano_Re(this);
        }

        public DataTable Consultar_Tabla_Valores_Construccion_Re()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Consultar_Tabla_Valores_Construccion_Re(this);
        }

        public DataTable Consultar_Motivos_Rechazo_Avaluo_Re()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Consultar_Motivos_Rechazo_Avaluo_Re(this);
        }

        public DataTable Consultar_Tabla_Elementos_Construccion_Re()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Consultar_Tabla_Elementos_Construccion_Re(this);
        }

        public DataTable Consultar_Tabla_Clasificacion_Zona_Re()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Consultar_Tabla_Clasificacion_Zona_Re(this);
        }

        public DataTable Consultar_Tabla_Servicios_Zona_Re()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Consultar_Tabla_Servicios_Zona_Re(this);
        }

        public DataTable Consultar_Tabla_Const_Dominante_Re()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Consultar_Tabla_Const_Dominante_Re(this);
        }
        public DataTable Consultar_Solicitud_Tramite()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Consultar_Solicitud_Tramite(this);
        }
        public Boolean Modificar_Estatus_Avaluo_Urbano_Regularizacion()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Modificar_Estatus_Avaluo_Urbano_Regularizacion(this);
        }

        public DataTable Consultar_Solicitudes_Regularizaciones()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Consultar_Solicitudes_Regularizaciones(this);
        }
        //public DataTable Consultar_Solicitudes_Regularizaciones()
        //{
        //    return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Consultar_Solicitudes_Regularizaciones(this);
        //}
        public DataTable Consultar_Solicitud_Tramite_Avaluos()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Reg_Datos.Consultar_Solicitud_Tramite_Avaluos(this);
        }
        

        #endregion
	
    }
}
