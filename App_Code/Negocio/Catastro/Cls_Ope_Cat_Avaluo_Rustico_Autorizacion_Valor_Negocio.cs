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
using System.Data;
using Presidencia.Operacion_Cat_Avaluo_Rustico_Autorizacion_Valor.Datos;

/// <summary>
/// Summary description for Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio
/// </summary>
namespace Presidencia.Operacion_Cat_Avaluo_Rustico_Autorizacion_Valor.Negocio
{
    public class Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Negocio
    {
        #region Variables Internas
        //Cabecera
        private String No_Avaluo;
        private String Anio_Avaluo;
        private String Folio;
        private String Motivo_Avaluo_Id;
        private String Cuenta_Predial_Id;
        private String Solicitante;
        private String Propietario;
        private String Clave_Catastral;
        private String Domicilio_Notificar;
        private String Municipio_Notificar;
        private String Ubicacion;
        private String Localidad_Municipio;
        private String Nombre_Predio;
        private String Grados_X;
        private String Minutos_X;
        private String Segundos_X;
        private String Orientacion_X;
        private String Grados_Y;
        private String Minutos_Y;
        private String Segundos_Y;
        private String Orientacion_Y;
        private String Tipo;
        private String Base_Gravable;
        private String Impuesto_Bimestral;
        private String Valor_Total_Predio;
        private String Coord_Norte;
        private String Coord_Sur;
        private String Coord_Oriente;
        private String Coord_Poniente;
        private String Observaciones;
        private String Observaciones_Perito;
        private String Veces_Rechazo;
        private DateTime Fecha_Autorizo;
        private String Perito_Externo_Id;
        private String Perito_Interno_Id;
        private String Estatus;
        private String Perito_Externo;
        private String Lote;
        private String Manzana;
        private String Region;
        private String Coordenadas_UTM;
        private String Coordenadas_UTM_Y;
        private String Uso;
        private String No_Asignacion;
        private String Permitir_Revision;
        private String Comentarios_Revisor;

        //Detalles
        private DataTable Dt_Elementos_Construccion;
        private DataTable Dt_Calculo_Valor_Construccion;
        private DataTable Dt_Calculo_Valor_Terreno;
        private DataTable Dt_Clasificacion_Zona;
        private DataTable Dt_Observaciones;
        private DataTable Dt_Medidas;
        private DataTable Dt_Documentos;

        #endregion

        #region Variables Publicas
        public DataTable P_Dt_Elementos_Construccion
        {
            get { return Dt_Elementos_Construccion; }
            set { Dt_Elementos_Construccion = value; }
        }

        public String P_Coordenadas_UTM
        {
            get {return Coordenadas_UTM;}
            set { Coordenadas_UTM = value; }
        }
        public String P_Coordenadas_UTM_Y
        {
            get { return Coordenadas_UTM_Y; }
            set { Coordenadas_UTM_Y = value; }
        }
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        public String P_Lote
        {
            get { return Lote; }
            set { Lote = value; }
        }
        public String P_Manzana
        {
            get { return Manzana; }
            set { Manzana = value; }
        }
        public String P_Region
        {
            get { return Region; }
            set { Region = value; }
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

        public String P_Propietario
        {
            get { return Propietario; }
            set { Propietario = value; }
        }

        public String P_Clave_Catastral
        {
            get { return Clave_Catastral; }
            set { Clave_Catastral = value; }
        }

        public String P_Domicilio_Notificar
        {
            get { return Domicilio_Notificar; }
            set { Domicilio_Notificar = value; }
        }

        public String P_Municipio_Notificar
        {
            get { return Municipio_Notificar; }
            set { Municipio_Notificar = value; }
        }

        public String P_Ubicacion
        {
            get { return Ubicacion; }
            set { Ubicacion = value; }
        }

        public String P_Localidad_Municipio
        {
            get { return Localidad_Municipio; }
            set { Localidad_Municipio = value; }
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

        public String P_Nombre_Predio
        {
            get { return Nombre_Predio; }
            set { Nombre_Predio = value; }
        }

        public String P_Grados_X
        {
            get { return Grados_X; }
            set { Grados_X = value; }
        }
        public String P_Minutos_X
        {
            get { return Minutos_X; }
            set { Minutos_X = value; }
        }
        public String P_Segundos_X
        {
            get { return Segundos_X; }
            set { Segundos_X = value; }
        }
        public String P_Orientacion_X
        {
            get { return Orientacion_X; }
            set { Orientacion_X = value; }
        }

        public String P_Grados_Y
        {
            get { return Grados_Y; }
            set { Grados_Y = value; }
        }
        public String P_Minutos_Y
        {
            get { return Minutos_Y; }
            set { Minutos_Y = value; }
        }
        public String P_Segundos_Y
        {
            get { return Segundos_Y; }
            set { Segundos_Y = value; }
        }
        public String P_Orientacion_Y
        {
            get { return Orientacion_Y; }
            set { Orientacion_Y = value; }
        }

        public String P_Base_Gravable
        {
            get { return Base_Gravable; }
            set { Base_Gravable = value; }
        }
        public String P_Impuesto_Bimestral
        {
            get { return Impuesto_Bimestral; }
            set { Impuesto_Bimestral = value; }
        }
        public String P_Valor_Total_Predio
        {
            get { return Valor_Total_Predio; }
            set { Valor_Total_Predio = value; }
        }

        public String P_Coord_Norte
        {
            get { return Coord_Norte; }
            set { Coord_Norte = value; }
        }
        public String P_Coord_Oriente
        {
            get { return Coord_Oriente; }
            set { Coord_Oriente = value; }
        }
        public String P_Coord_Poniente
        {
            get { return Coord_Poniente; }
            set { Coord_Poniente = value; }
        }
        public String P_Coord_Sur
        {
            get { return Coord_Sur; }
            set { Coord_Sur = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public DateTime P_Fecha_Autorizo
        {
            get { return Fecha_Autorizo; }
            set { Fecha_Autorizo = value; }
        }

        public String P_Observaciones_Perito
        {
            get { return Observaciones_Perito; }
            set { Observaciones_Perito = value; }
        }

        public String P_Veces_Rechazo
        {
            get { return Veces_Rechazo; }
            set { Veces_Rechazo = value; }
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

        public String P_Perito_Externo
        {
            get { return Perito_Externo; }
            set { Perito_Externo = value; }
        }

        public String P_Uso
        {
            get { return Uso; }
            set { Uso = value; }
        }

        public String P_No_Asignacion
        {
            get { return No_Asignacion; }
            set { No_Asignacion = value; }
        }

        public DataTable P_Dt_Calculo_Valor_Construccion
        {
            get { return Dt_Calculo_Valor_Construccion; }
            set { Dt_Calculo_Valor_Construccion = value; }
        }

        public DataTable P_Dt_Calculo_Valor_Terreno
        {
            get { return Dt_Calculo_Valor_Terreno; }
            set { Dt_Calculo_Valor_Terreno = value; }
        }

        public DataTable P_Dt_Clasificacion_Zona
        {
            get { return Dt_Clasificacion_Zona; }
            set { Dt_Clasificacion_Zona = value; }
        }

        public DataTable P_Dt_Observaciones
        {
            get { return Dt_Observaciones; }
            set { Dt_Observaciones = value; }
        }

        public DataTable P_Dt_Medidas
        {
            get { return Dt_Medidas; }
            set { Dt_Medidas = value; }
        }

        public DataTable P_Dt_Documentos
        {
            get { return Dt_Documentos; }
            set { Dt_Documentos = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Avaluo_Rustico()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Datos.Alta_Avaluo_Rustico(this);
        }

        public Boolean Modificar_Avaluo_Rustico()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Datos.Modificar_Avaluo_Rustico(this);
        }

        public Boolean Modificar_Observaciones_Ara()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Datos.Modificar_Observaciones_Ara(this);
        }

        public DataTable Consultar_Avaluo_Rustico()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Datos.Consultar_Avaluo_Rustico(this);
        }

        public DataTable Consultar_Tabla_Valores_Construccion()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Datos.Consultar_Tabla_Valores_Construccion(this);
        }

        public DataTable Consultar_Motivos_Rechazo_Avaluo()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Datos.Consultar_Motivos_Rechazo_Avaluo(this);
        }

        public DataTable Consultar_Tabla_Terreno()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Datos.Consultar_Tabla_Terreno(this);
        }

        public DataTable Consultar_Tabla_Caracteristicas_Terreno()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Datos.Consultar_Tabla_Caracteristicas_Terreno(this);
        }

        public DataTable Consultar_Tabla_Elementos_Construccion()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Datos.Consultar_Tabla_Elementos_Construccion(this);
        }
        public DataTable Consulta_Firmante()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Autorizacion_Valor_Datos.Consultar_Firmante();
        }

        #endregion
    }
}