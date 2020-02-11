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
using Presidencia.Operacion_Cat_Avaluo_Urbano_Av.Datos;

/// <summary>
/// Summary description for Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio
/// </summary>
/// 


namespace Presidencia.Operacion_Cat_Avaluo_Urbano_Av.Negocio
{

    public class Cls_Ope_Cat_Avaluo_Urbano_Av_Negocio
    {

        #region Variables Internas
        //Cabecera
        private String No_Avaluo;
        private String Anio_Avaluo;
        private String Folio;
        private String Motivo_Avaluo_Id;
        private String Cuenta_Predial_Id;
        private String Region;
        private String Manzana;
        private String Lote;
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
        private String Estatus;
        private String Observaciones_Rechazo;
        private String Veces_Rechazo;
        private String Coord_Oriente;
        private String Coord_Sur;
        private String Coord_Norte;
        private String Coord_Poniente;
        private String Permitir_Revision;
        private String No_Asignacion;
        private String Comentarios_Revisor;
        private String No_Renglones;
        private String Tipo_Avaluo;
        private String Anio_Tasa;
        private String Identificador_Tasa;

        //Detalles
        private DataTable Dt_Caracteristicas_Terreno_Av;
        private DataTable Dt_Construccion_Av;
        private DataTable Dt_Elementos_Construccion_Av;
        private DataTable Dt_Calculo_Valor_Construccion_Av;
        private DataTable Dt_Calculo_Valor_Terreno_Av;
        private DataTable Dt_Clasificacion_Zona_Av;
        private DataTable Dt_Servicios_Zona_Av;
        private DataTable Dt_Construccion_Dominante_Av;
        private DataTable Dt_Observaciones_Av;
        private DataTable Dt_Colindancias;
        private DataTable Dt_Documentos;


        #endregion

        #region Variables Publicas

        public String P_Coord_Oriente
        {
            get { return Coord_Oriente; }
            set { Coord_Oriente = value; }
        }
        public String P_Coord_Sur
        {
            get { return Coord_Sur; }
            set { Coord_Sur = value; }
        }
        public String P_Coord_Poniente
        {
            get { return Coord_Poniente; }
            set { Coord_Poniente = value; }
        }
        public String P_Coord_Norte
        {
            get { return Coord_Norte; }
            set { Coord_Norte = value; }
        }
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

        public String P_Veces_Rechazo
        {
            get { return Veces_Rechazo; }
            set { Veces_Rechazo = value; }
        }

        public String P_Observaciones_Rechazo
        {
            get { return Observaciones_Rechazo; }
            set { Observaciones_Rechazo = value; }
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

        public String P_Tipo_Avaluo
        {
            get { return Tipo_Avaluo; }
            set { Tipo_Avaluo = value; }
        }

        public String P_No_Renglones
        {
            get { return No_Renglones; }
            set { No_Renglones = value; }
        }

        public String P_Permitir_Revision
        {
            get { return Permitir_Revision; }
            set { Permitir_Revision = value; }
        }

        public String P_Comentarios_Revisor
        {
            get { return Comentarios_Revisor; }
            set { Comentarios_Revisor = value; }
        }

        public String P_No_Asignacion
        {
            get { return No_Asignacion; }
            set { No_Asignacion = value; }
        }

        public String P_Anio_Tasa
        {
            get { return Anio_Tasa; }
            set { Anio_Tasa = value; }
        }

        public String P_Identificador_Tasa
        {
            get { return Identificador_Tasa; }
            set { Identificador_Tasa = value; }
        }

        public DataTable P_Dt_Caracteristicas_Terreno_Av
        {
            get { return Dt_Caracteristicas_Terreno_Av; }
            set { Dt_Caracteristicas_Terreno_Av = value; }
        }

        public DataTable P_Dt_Construccion_Av
        {
            get { return Dt_Construccion_Av; }
            set { Dt_Construccion_Av = value; }
        }

        public DataTable P_Dt_Elementos_Construccion_Av
        {
            get { return Dt_Elementos_Construccion_Av; }
            set { Dt_Elementos_Construccion_Av = value; }
        }

        public DataTable P_Dt_Calculo_Valor_Construccion_Av
        {
            get { return Dt_Calculo_Valor_Construccion_Av; }
            set { Dt_Calculo_Valor_Construccion_Av = value; }
        }

        public DataTable P_Dt_Calculo_Valor_Terreno_Av
        {
            get { return Dt_Calculo_Valor_Terreno_Av; }
            set { Dt_Calculo_Valor_Terreno_Av = value; }
        }

        public DataTable P_Dt_Clasificacion_Zona_Av
        {
            get { return Dt_Clasificacion_Zona_Av; }
            set { Dt_Clasificacion_Zona_Av = value; }
        }

        public DataTable P_Dt_Servicios_Zona_Av
        {
            get { return Dt_Servicios_Zona_Av; }
            set { Dt_Servicios_Zona_Av = value; }
        }

        public DataTable P_Dt_Construccion_Dominante_Av
        {
            get { return Dt_Construccion_Dominante_Av; }
            set { Dt_Construccion_Dominante_Av = value; }
        }

        public DataTable P_Dt_Observaciones_Av
        {
            get { return Dt_Observaciones_Av; }
            set { Dt_Observaciones_Av = value; }
        }

        public DataTable P_Dt_Colindancias
        {
            get { return Dt_Colindancias; }
            set { Dt_Colindancias = value; }
        }

        public DataTable P_Dt_Documentos
        {
            get { return Dt_Documentos; }
            set { Dt_Documentos = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Valor_Construccion_Av()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Alta_Avaluo_Urbano_Av(this);
        }

        public Boolean Asignar_Perito_Interno()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Asignar_Perito_Interno(this);
        }

        public Boolean Modificar_Valor_Construccion_Av()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Modificar_Avaluo_Urbano_Av(this);
        }

        public Boolean Modificar_Observaciones_Avaluo_Av()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Modificar_Observaciones_Avaluo_Av(this);
        }

        public DataTable Consultar_Avaluo_Urbano_Av()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Consultar_Avaluo_Urbano_Av(this);
        }

        public DataTable Consultar_Tabla_Valores_Construccion_Av()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Consultar_Tabla_Valores_Construccion_Av(this);
        }

        public DataTable Consultar_Motivos_Rechazo_Avaluo_Av()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Consultar_Motivos_Rechazo_Avaluo_Av(this);
        }

        public DataTable Consultar_Tabla_Elementos_Construccion_Av()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Consultar_Tabla_Elementos_Construccion_Av(this);
        }

        public DataTable Consultar_Tabla_Clasificacion_Zona_Av()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Consultar_Tabla_Clasificacion_Zona_Av(this);
        }

        public DataTable Consultar_Tabla_Servicios_Zona_Av()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Consultar_Tabla_Servicios_Zona_Av(this);
        }

        public DataTable Consultar_Tabla_Const_Dominante_Av()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Consultar_Tabla_Const_Dominante_Av(this);
        }

        public DataTable Consultar_Avaluo_Urbano_Asignados()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Consultar_Avaluo_Urbano_Asignados(this);
        }

        public DataTable Consulta_Firmante()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Consultar_Firmante();
        }

        public DataTable Consultar_Tasas_Anuales()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Av_Datos.Consultar_Tasas_Anuales(this);
        }
        #endregion
    }
}
