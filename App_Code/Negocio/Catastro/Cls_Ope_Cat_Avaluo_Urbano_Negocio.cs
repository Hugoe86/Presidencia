using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Cat_Avaluo_Urbano.Datos;
using System.Data;

/// <summary>
/// Summary description for Cls_Ope_Cat_Avaluo_Urbano_Negocio
/// </summary>

namespace Presidencia.Operacion_Cat_Avaluo_Urbano.Negocio
{
    public class Cls_Ope_Cat_Avaluo_Urbano_Negocio
    {
        #region Variables Internas
        //Cabecera
        private String No_Avaluo;
        private String Anio_Avaluo;
        private String Folio;
        private String Motivo_Avaluo_Id;
        private String Cuenta_Predial_Id;
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
        private String Estatus_Anterior;
        private String Solicitud_Id;
        private String Comentarios_Perito;
        private String Permitir_Revision;
        private Boolean Es_Primer_Revision;

        private String Cuenta_Predial;
        private String Region;
        private String Manzana;
        private String Lote;
        private String Ubicacion;
        private String Colonia;
        private String Localidad;
        private String Municipio;
        private String Propietario;
        private String Domicilio_Notificar;
        private String Colonia_Notificar;
        private String Localidad_Notificar;
        private String Municipio_Notificar;
        private String Perito_Externo;
        private String Observaciones_Perito;
        private String Coord_Norte;
        private String Coord_Sur;
        private String Coord_Oriente;
        private String Coord_Poniente;
        private String Veces_Rechazo;
        private String Importe_Avaluo;

        //Detalles
        private DataTable Dt_Caracteristicas_Terreno;
        private DataTable Dt_Construccion;
        private DataTable Dt_Elementos_Construccion;
        private DataTable Dt_Calculo_Valor_Construccion;
        private DataTable Dt_Calculo_Valor_Terreno;
        private DataTable Dt_Clasificacion_Zona;
        private DataTable Dt_Servicios_Zona;
        private DataTable Dt_Construccion_Dominante;
        private DataTable Dt_Observaciones;
        private DataTable Dt_Archivos;
        private DataTable Dt_Medidas;

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

        public String P_Estatus_Anterior
        {
            get { return Estatus_Anterior; }
            set { Estatus_Anterior = value; }
        }

        public String P_Comentarios_Perito
        {
            get { return Comentarios_Perito; }
            set { Comentarios_Perito = value; }
        }

        public String P_Permitir_Revision
        {
            get { return Permitir_Revision; }
            set { Permitir_Revision = value; }
        }

        public String P_Perito_Externo
        {
            get { return Perito_Externo; }
            set { Perito_Externo = value; }
        }

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
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

        public String P_Ubicacion
        {
            get { return Ubicacion; }
            set { Ubicacion = value; }
        }

        public String P_Colonia
        {
            get { return Colonia; }
            set { Colonia = value; }
        }

        public String P_Localidad
        {
            get { return Localidad; }
            set { Localidad = value; }
        }

        public String P_Municipio
        {
            get { return Municipio; }
            set { Municipio = value; }
        }

        public String P_Observaciones_Perito
        {
            get { return Observaciones_Perito; }
            set { Observaciones_Perito = value; }
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
        public String P_Veces_Rechazo
        {
            get { return Veces_Rechazo; }
            set { Veces_Rechazo = value; }
        }

        public String P_Propietario
        {
            get { return Propietario; }
            set { Propietario = value; }
        }

        public String P_Importe_Avaluo
        {
            get { return Importe_Avaluo; }
            set { Importe_Avaluo = value; }
        }

        public String P_Domicilio_Notificar
        {
            get { return Domicilio_Notificar; }
            set { Domicilio_Notificar = value; }
        }

        public String P_Colonia_Notificar
        {
            get { return Colonia_Notificar; }
            set { Colonia_Notificar = value; }
        }

        public String P_Solicitud_Id
        {
            get { return Solicitud_Id; }
            set { Solicitud_Id = value; }
        }

        public String P_Localidad_Notificar
        {
            get { return Localidad_Notificar; }
            set { Localidad_Notificar = value; }
        }

        public String P_Municipio_Notificar
        {
            get { return Municipio_Notificar; }
            set { Municipio_Notificar = value; }
        }

        public Boolean P_Es_Primer_Revision
        {
            get { return Es_Primer_Revision; }
            set { Es_Primer_Revision = value; }
        }

        public DataTable P_Dt_Caracteristicas_Terreno
        {
            get { return Dt_Caracteristicas_Terreno; }
            set { Dt_Caracteristicas_Terreno = value; }
        }

        public DataTable P_Dt_Construccion
        {
            get { return Dt_Construccion; }
            set { Dt_Construccion = value; }
        }

        public DataTable P_Dt_Elementos_Construccion
        {
            get { return Dt_Elementos_Construccion; }
            set { Dt_Elementos_Construccion = value; }
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

        public DataTable P_Dt_Servicios_Zona
        {
            get { return Dt_Servicios_Zona; }
            set { Dt_Servicios_Zona = value; }
        }

        public DataTable P_Dt_Construccion_Dominante
        {
            get { return Dt_Construccion_Dominante; }
            set { Dt_Construccion_Dominante = value; }
        }

        public DataTable P_Dt_Observaciones
        {
            get { return Dt_Observaciones; }
            set { Dt_Observaciones = value; }
        }

        public DataTable P_Dt_Archivos
        {
            get { return Dt_Archivos; }
            set { Dt_Archivos = value; }
        }

        public DataTable P_Dt_Medidas
        {
            get { return Dt_Medidas; }
            set { Dt_Medidas = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Valor_Construccion()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Datos.Alta_Avaluo_Urbano(this);
        }

        public Boolean Modificar_Valor_Construccion()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Datos.Modificar_Avaluo_Urbano(this);
        }

        public Boolean Modificar_Observaciones_Avaluo_Au()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Datos.Modificar_Observaciones_Avaluo_Au(this);
        }

        public Boolean Asignar_Perito_Interno()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Datos.Asignar_Perito_Interno(this);
        }

        public DataTable Consultar_Avaluo_Urbano()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Datos.Consultar_Avaluo_Urbano(this);
        }

        public DataTable Consultar_Tabla_Valores_Construccion()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Datos.Consultar_Tabla_Valores_Construccion(this);
        }

        public DataTable Consultar_Motivos_Rechazo_Avaluo()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Datos.Consultar_Motivos_Rechazo_Avaluo(this);
        }

        public DataTable Consultar_Tabla_Elementos_Construccion()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Datos.Consultar_Tabla_Elementos_Construccion(this);
        }

        public DataTable Consultar_Tabla_Clasificacion_Zona()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Datos.Consultar_Tabla_Clasificacion_Zona(this);
        }

        public DataTable Consultar_Tabla_Servicios_Zona()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Datos.Consultar_Tabla_Servicios_Zona(this);
        }

        public DataTable Consultar_Tabla_Const_Dominante()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Datos.Consultar_Tabla_Const_Dominante(this);
        }

        public DataTable Consultar_Avaluo_Urbano_Asignados()
        {
            return Cls_Ope_Cat_Avaluo_Urbano_Datos.Consultar_Avaluo_Urbano_Asignados(this);
        }

        #endregion
    }
}