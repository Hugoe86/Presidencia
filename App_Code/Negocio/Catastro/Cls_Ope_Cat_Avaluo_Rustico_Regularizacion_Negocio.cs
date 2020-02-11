using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Cat_Avaluo_Rustico_Regularizacion.Datos;
using System.Data;


/// <summary>
/// Summary description for Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio
/// </summary>
/// 
namespace Presidencia.Operacion_Cat_Avaluo_Rustico_Regularizacion.Negocio
{
    public class Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Negocio
    {
        #region Variables Internas
        //Cabecera
        private String No_Avaluo;
        private String Anio_Avaluo;
        private String Folio;
        private String Motivo_Avaluo_Id;
        private String Cuenta_Predial_Id;
        private String Cuenta_Predial;
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
        private String Solicitud_Id;
        private String Uso_Constru;
        private String Tipo;
        private String Coordenadas_UTM;
        private String Coordenadas_UTM_Y;
        private String Importe_Avaluo;
        private String Generar_Cobro;
        private String No_Oficio;

        //Detalles
        private DataTable Dt_Elementos_Construccion;
        private DataTable Dt_Calculo_Valor_Construccion;
        private DataTable Dt_Calculo_Valor_Terreno;
        private DataTable Dt_Clasificacion_Zona;
        private DataTable Dt_Observaciones;
        private DataTable Dt_Medidas;
        private DataTable Dt_Archivos;

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
            set { Cuenta_Predial = value; }
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

        public String P_Importe_Avaluo
        {
            get { return Importe_Avaluo; }
            set { Importe_Avaluo = value; }
        }

        public String P_Generar_Cobro
        {
            get { return Generar_Cobro; }
            set { Generar_Cobro = value; }
        }

        public String P_Perito_Externo
        {
            get { return Perito_Externo; }
            set { Perito_Externo = value; }
        }
        public String P_Solicitud_Id
        {
            get { return Solicitud_Id; }
            set { Solicitud_Id = value; }
        }
        public String P_Uso_Constru
        {
            get { return Uso_Constru; }
            set { Uso_Constru = value; }
        }
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        public String P_Coordenadas_UTM
        {
            get { return Coordenadas_UTM; }
            set { Coordenadas_UTM = value; }
        }
        public String P_Coordenadas_UTM_Y
        {
            get { return Coordenadas_UTM_Y; }
            set { Coordenadas_UTM_Y = value; }
        }

        public String P_No_Oficio
        {
            get { return No_Oficio; }
            set { No_Oficio = value; }
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
        public DataTable P_Dt_Elementos_Construccion
        {
            get { return Dt_Elementos_Construccion; }
            set { Dt_Elementos_Construccion = value; }
        }
        public DataTable P_Dt_Archivos
        {
            get { return Dt_Archivos; }
            set { Dt_Archivos = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Avaluo_Rustico_Regularizacion()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos.Alta_Avaluo_Rustico_Regularizacion(this);
        }

        public Boolean Modificar_Avaluo_Rustico_Regularizacion()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos.Modificar_Avaluo_Rustico_Regularizacion(this);
        }

        public Boolean Modificar_Observaciones_Arr()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos.Modificar_Observaciones_Arr(this);
        }

        public DataTable Consultar_Avaluo_Rustico_Regularizacion()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos.Consultar_Avaluo_Rustico_Regularizacion(this);
        }

        public DataTable Consultar_Tabla_Valores_Construccion_Regularizacion()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos.Consultar_Tabla_Valores_Construccion_Regularizacion(this);
        }

        public DataTable Consultar_Motivos_Rechazo_Avaluo_Regularizacion()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos.Consultar_Motivos_Rechazo_Avaluo_Regularizacion(this);
        }

        public DataTable Consultar_Tabla_Terreno_Regularizacion()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos.Consultar_Tabla_Terreno_Regularizacion(this);
        }

        public DataTable Consultar_Tabla_Caracteristicas_Terreno_Regularizacion()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos.Consultar_Tabla_Caracteristicas_Terreno_Regularizacion(this);
        }
        public DataTable Consultar_Solicitud_Tramite()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos.Consultar_Solicitud_Tramite(this);
        }

        public DataTable Consultar_Tabla_Elementos_Construccion_Regularizacion()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos.Consultar_Tabla_Elementos_Construccion_Regularizacion(this);
        }

        public Boolean Modificar_Estatus_Avaluo_Rustico_Regularizacion()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos.Modificar_Estatus_Avaluo_Rustico_Regularizacion(this);
        }
        public DataTable Consultar_Solicitud_Tramite_Avaluos()
        {
            return Cls_Ope_Cat_Avaluo_Rustico_Regularizacion_Datos.Consultar_Solicitud_Tramite_Avaluos(this);
        }
        
        #endregion
    }
}