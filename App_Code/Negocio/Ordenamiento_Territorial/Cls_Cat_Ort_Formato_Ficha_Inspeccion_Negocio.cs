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
using Presidencia.Orden_Territorial_Formato_Ficha_Inspeccion.Datos;

namespace Presidencia.Orden_Territorial_Formato_Ficha_Inspeccion.Negocio
{
    public class Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio
    {
        #region Variables Internas
        //  para los ids
        private String Ficha_Inspeccion_ID; 
        private String TRAMITE_ID;
        private String SOLICITUD_ID;
        private String SUBPROCESO_ID;
        private String INSPECTOR_ID;
        //  para las fechas
        private DateTime FECHA_ENTREGA;
        private String TIEMPO_RESPUESTA;
        private DateTime FECHA_INSPECCION;
        //  para los datos del inmueble
        private String INMUEBLE_NOMBRE;
        private String INMUEBLE_TELEFONO;
        private String INMUEBLE_CALLE;
        private String INMUEBLE_COLONIA;
        private String INMUEBLE_NUMERO;
        //  para los datos del solicitante
        private String SOLICITANTE_NOMBRE;
        private String SOLICITANTE_TELEFONO;
        private String SOLICITANTE_COLONIA;
        private String SOLICITANTE_CALLE;
        private String SOLICITANTE_NUMERO;
        //  para los datos del manifiesto de impacto ambiental
        private String IMPACTO_AFECTACIONES;
        private String IMPACTO_COLINDANCIAS;
        private String IMPACTO_SUPERFICIE;
        private String IMPACTO_TIPO_PROYECTO;
        //  para los datos de la licencia de autorizacion de funcionamiento
        private String LICENCIA_TIPO_EQUIPO;
        private String LICENCIA_TIPO_EMISION;
        private String LICENCIA_HORARIO_FUNCIONAM;
        private String LICENCIA_TIPO_COMBUSTIBLE;
        private String LICENCIA_GASTO_COMBUSTIBLE;
        //  para los datos de la poda de arboles
        private String PODA_ALTURA;
        private String PODA_DIAMETRO_TRONCO;
        private String PODA_DIAMETRO_FRONDA;
        private String PODA_ESPECIE;
        private String PODA_CONDICIONES;
        //  para los datos del banco de materiales
        private String MATERIAL_PERMISO_ECOLOGIA;
        private String MATERIAL_PERMISO_SUELO;
        private String MATERIAL_SUPERFICIE_TOTAL;
        private String MATERIAL_PROFUNDIDAD;
        private String MATERIAL_INCLINACION;
        private String MATERIAL_FLORA;
        private String MATERIAL_ACCESO_VEHICULOS;
        private String MATERIAL_PETREO;
        //  para los datos de la autorizacion
        private String AUTORIZA_SUELOS;
        private String AUTORIZA_AREA_RESIDUOS;
        private String AUTORIZA_SEPARACION;
        private String AUTORIZA_METODO_SEPARACION;
        private String AUTORIZA_SERVICIO_RECOLEC;
        private String AUTORIZA_REVUELVEN_SOLD_LIQU;
        private String AUTORIZA_TIPO_CONTENEDOR;
        private String AUTORIZA_DRENAJE;
        private String AUTORIZA_TIPO_DRENAJE;
        private String AUTORIZA_TIPO_RUIDO;
        private String AUTORIZA_NIVEL_RUIDO;
        private String AUTORIZA_HORARIO_LABORES;
        private String AUTORIZA_LUNES;
        private String AUTORIZA_MARTES;
        private String AUTORIZA_MIERCOLES;
        private String AUTORIZA_JUEVES;
        private String AUTORIZA_VIERNES;
        private String AUTORIZA_COLINDANCIA;
        private String AUTORIZA_EMISIONES;
        //  para las observaciones
        private String OBSERVACION_DEL_INSPEC;
        private String OBSERVACION_PARA_INSPEC;

        //  para el usuario
        private String Usuario_Creo;

        private DataTable Dt_Residuos;
        private String Plantilla_ID;
        private String Tabla;
        #endregion

        #region Variables Publicas
        //  para los ids
        public String P_Ficha_Inspeccion_ID
        {
            get { return Ficha_Inspeccion_ID; }
            set { Ficha_Inspeccion_ID = value; }
        }
        public String P_Tramite_ID
        {
            get { return TRAMITE_ID; }
            set { TRAMITE_ID = value; }
        }
        public String P_Solicitud_ID
        {
            get { return SOLICITUD_ID; }
            set { SOLICITUD_ID = value; }
        }
        public String P_Subproceso_ID
        {
            get { return SUBPROCESO_ID; }
            set { SUBPROCESO_ID = value; }
        }
        public String P_Inspector_ID
        {
            get { return INSPECTOR_ID; }
            set { INSPECTOR_ID = value; }
        }
        //  para las fechas
        public DateTime P_Fecha_Entrega
        {
            get { return FECHA_ENTREGA; }
            set { FECHA_ENTREGA = value; }
        }
        public String P_Tiempo_Respuesta
        {
            get { return TIEMPO_RESPUESTA; }
            set { TIEMPO_RESPUESTA = value; }
        }
        public DateTime P_Fecha_Inspeccion
        {
            get { return FECHA_INSPECCION; }
            set { FECHA_INSPECCION = value; }
        }
        //  para los datos del inmueble
        public String P_Inmueble_Nombre
        {
            get { return INMUEBLE_NOMBRE; }
            set { INMUEBLE_NOMBRE = value; }
        }
        public String P_Inmueble_Telefono
        {
            get { return INMUEBLE_TELEFONO; }
            set { INMUEBLE_TELEFONO = value; }
        }
        public String P_Inmueble_Colonia
        {
            get { return INMUEBLE_COLONIA; }
            set { INMUEBLE_COLONIA = value; }
        }
        public String P_Inmueble_Calle
        {
            get { return INMUEBLE_CALLE; }
            set { INMUEBLE_CALLE = value; }
        }
        public String P_Inmueble_Numero
        {
            get { return INMUEBLE_NUMERO; }
            set { INMUEBLE_NUMERO = value; }
        }
        //  para los datos del solicitante
        public String P_Solicitante_Nombre
        {
            get { return SOLICITANTE_NOMBRE; }
            set { SOLICITANTE_NOMBRE = value; }
        }
        public String P_Solicitante_Telefono
        {
            get { return SOLICITANTE_TELEFONO; }
            set { SOLICITANTE_TELEFONO = value; }
        }
        public String P_Solicitante_Colonia
        {
            get { return SOLICITANTE_COLONIA; }
            set { SOLICITANTE_COLONIA = value; }
        }
        public String P_Solicitante_Calle
        {
            get { return SOLICITANTE_CALLE; }
            set { SOLICITANTE_CALLE = value; }
        }
        public String P_Solicitante_Numero
        {
            get { return SOLICITANTE_NUMERO; }
            set { SOLICITANTE_NUMERO = value; }
        }
        //  para los datos del manifiesto de impacto ambiental
        public String P_Impacto_Afectables
        {
            get { return IMPACTO_AFECTACIONES; }
            set { IMPACTO_AFECTACIONES = value; }
        }
        public String P_Impacto_Colindancias
        {
            get { return IMPACTO_COLINDANCIAS; }
            set { IMPACTO_COLINDANCIAS = value; }
        }
        public String P_Impacto_Superficie
        {
            get { return IMPACTO_SUPERFICIE; }
            set { IMPACTO_SUPERFICIE = value; }
        }
        public String P_Impacto_Tipo_Proyecto
        {
            get { return IMPACTO_TIPO_PROYECTO; }
            set { IMPACTO_TIPO_PROYECTO = value; }
        }
        //  para los datos de la licencia de autorizacion de funcionamiento
        public String P_Licencia_Tipo_Equipo
        {
            get { return LICENCIA_TIPO_EQUIPO; }
            set { LICENCIA_TIPO_EQUIPO = value; }
        }
        public String P_Licencia_Tipo_Emision
        {
            get { return LICENCIA_TIPO_EMISION; }
            set { LICENCIA_TIPO_EMISION = value; }
        }
        public String P_Licencia_Horario_Funcionamiento
        {
            get { return LICENCIA_HORARIO_FUNCIONAM; }
            set { LICENCIA_HORARIO_FUNCIONAM = value; }
        }
        public String P_Licencia_Tipo_Combustible
        {
            get { return LICENCIA_TIPO_COMBUSTIBLE; }
            set { LICENCIA_TIPO_COMBUSTIBLE = value; }
        }
        public String P_Licencia_Tipo_Gastos_Combustible
        {
            get { return LICENCIA_GASTO_COMBUSTIBLE; }
            set { LICENCIA_GASTO_COMBUSTIBLE = value; }
        }
        //  para los datos de la poda de arboles
        public String P_Poda_Altura
        {
            get { return PODA_ALTURA; }
            set { PODA_ALTURA = value; }
        }
        public String P_Poda_Diametro_Tronco
        {
            get { return PODA_DIAMETRO_TRONCO; }
            set { PODA_DIAMETRO_TRONCO = value; }
        }
        public String P_Poda_Fronda
        {
            get { return PODA_DIAMETRO_FRONDA; }
            set { PODA_DIAMETRO_FRONDA = value; }
        }
        public String P_Poda_Especie
        {
            get { return PODA_ESPECIE; }
            set { PODA_ESPECIE = value; }
        }
        public String P_Poda_Condiciones
        {
            get { return PODA_CONDICIONES; }
            set { PODA_CONDICIONES = value; }
        }
        //  para los datos del banco de materiales
        public String P_Material_Permiso_Ecologico
        {
            get { return MATERIAL_PERMISO_ECOLOGIA; }
            set { MATERIAL_PERMISO_ECOLOGIA = value; }
        }
        public String P_Material_Permiso_Suelo
        {
            get { return MATERIAL_PERMISO_SUELO; }
            set { MATERIAL_PERMISO_SUELO = value; }
        }
        public String P_Material_Superficie_Total
        {
            get { return MATERIAL_SUPERFICIE_TOTAL; }
            set { MATERIAL_SUPERFICIE_TOTAL = value; }
        }
        public String P_Material_Profundidad
        {
            get { return MATERIAL_PROFUNDIDAD; }
            set { MATERIAL_PROFUNDIDAD = value; }
        }
        public String P_Material_Inclinacion
        {
            get { return MATERIAL_INCLINACION; }
            set { MATERIAL_INCLINACION = value; }
        }
        public String P_Material_Flora
        {
            get { return MATERIAL_FLORA; }
            set { MATERIAL_FLORA = value; }
        }
        public String P_Material_Acceso_Vehiculos
        {
            get { return MATERIAL_ACCESO_VEHICULOS; }
            set { MATERIAL_ACCESO_VEHICULOS = value; }
        }
        public String P_Material_Petreo
        {
            get { return MATERIAL_PETREO; }
            set { MATERIAL_PETREO = value; }
        }

        //  para los datos de la autorizacion
        public String P_Autoriza_Suelos
        {
            get { return AUTORIZA_SUELOS; }
            set { AUTORIZA_SUELOS = value; }
        }
        public String P_Autoriza_Area_Residuos
        {
            get { return AUTORIZA_AREA_RESIDUOS; }
            set { AUTORIZA_AREA_RESIDUOS = value; }
        }
        public String P_Autoriza_Separacion
        {
            get { return AUTORIZA_SEPARACION; }
            set { AUTORIZA_SEPARACION = value; }
        }
        public String P_Autoriza_Metodo_Separacion
        {
            get { return AUTORIZA_METODO_SEPARACION; }
            set { AUTORIZA_METODO_SEPARACION = value; }
        }
        public String P_Autoriza_Servicio_Recoleccion
        {
            get { return AUTORIZA_SERVICIO_RECOLEC; }
            set { AUTORIZA_SERVICIO_RECOLEC = value; }
        }
        public String P_Autoriza_Revuelven_Solidos_Liquidos
        {
            get { return AUTORIZA_REVUELVEN_SOLD_LIQU; }
            set { AUTORIZA_REVUELVEN_SOLD_LIQU = value; }
        }
        public String P_Autoriza_Tipo_Contenedor
        {
            get { return AUTORIZA_TIPO_CONTENEDOR; }
            set { AUTORIZA_TIPO_CONTENEDOR = value; }
        }
        public String P_Autoriza_Drenaje
        {
            get { return AUTORIZA_DRENAJE; }
            set { AUTORIZA_DRENAJE = value; }
        }
        public String P_Autoriza_Tipo_Drenaje
        {
            get { return AUTORIZA_TIPO_DRENAJE; }
            set { AUTORIZA_TIPO_DRENAJE = value; }
        }
        public String P_Autoriza_Tipo_Ruido
        {
            get { return AUTORIZA_TIPO_RUIDO; }
            set { AUTORIZA_TIPO_RUIDO = value; }
        }
        public String P_Autoriza_Nivel_Ruido
        {
            get { return AUTORIZA_NIVEL_RUIDO; }
            set { AUTORIZA_NIVEL_RUIDO = value; }
        }
        public String P_Autoriza_Horario_Labores
        {
            get { return AUTORIZA_HORARIO_LABORES; }
            set { AUTORIZA_HORARIO_LABORES = value; }
        }
        public String P_Autoriza_Lunes
        {
            get { return AUTORIZA_LUNES; }
            set { AUTORIZA_LUNES = value; }
        }
        public String P_Autoriza_Martes
        {
            get { return AUTORIZA_MARTES; }
            set { AUTORIZA_MARTES = value; }
        }
        public String P_Autoriza_Miercoles
        {
            get { return AUTORIZA_MIERCOLES; }
            set { AUTORIZA_MIERCOLES = value; }
        }
        public String P_Autoriza_Jueves
        {
            get { return AUTORIZA_JUEVES; }
            set { AUTORIZA_JUEVES = value; }
        }
        public String P_Autoriza_Viernes
        {
            get { return AUTORIZA_VIERNES; }
            set { AUTORIZA_VIERNES = value; }
        }
        public String P_Autoriza_Colindancia
        {
            get { return AUTORIZA_COLINDANCIA; }
            set { AUTORIZA_COLINDANCIA = value; }
        }
        public String P_Autoriza_Emisiones
        {
            get { return AUTORIZA_EMISIONES; }
            set { AUTORIZA_EMISIONES = value; }
        }
        //  para las observaciones
        public String P_Observaciones_Del_Inspector
        {
            get { return OBSERVACION_DEL_INSPEC; }
            set { OBSERVACION_DEL_INSPEC = value; }
        }
        public String P_Observaciones_Para_Inspector
        {
            get { return OBSERVACION_PARA_INSPEC; }
            set { OBSERVACION_PARA_INSPEC = value; }
        }
        //  para el usuario
        public String P_Usuario
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }

        public DataTable P_Dt_Residuos
        {
            get { return Dt_Residuos; }
            set { Dt_Residuos = value; }
        }
        public String P_Plantilla_ID
        {
            get { return Plantilla_ID; }
            set { Plantilla_ID = value; }
        }
        public String P_Tabla
        {
            get { return Tabla; }
            set { Tabla = value; }
        }
        
        #endregion

        #region Consultas
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tipos_Residuos
        ///DESCRIPCIÓN          : Metodo para consultar los datos de los tipos de residuos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Tipos_Residuos()
        {
            return Cls_Cat_Ort_Formato_Ficha_Inspeccion_Datos.Consultar_Tipos_Residuos(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Formato_Existente
        ///DESCRIPCIÓN          : Metodo para consultar si se encuentra registrado el formato
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 08/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Formato_Existente()
        {
            return Cls_Cat_Ort_Formato_Ficha_Inspeccion_Datos.Consultar_Formato_Existente(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Llenado_Solicitud_Formato
        ///DESCRIPCIÓN          : Metodo que consultara los formatos que se deben de llenar
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 08/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Llenado_Solicitud_Formato()
        {
            return Cls_Cat_Ort_Formato_Ficha_Inspeccion_Datos.Consultar_Llenado_Solicitud_Formato(this);
        }

        #endregion

        #region Alta
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Guardar_Formato
        ///DESCRIPCIÓN          : Metodo para guardar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012 
        ///*********************************************************************************************************
        public Boolean Guardar_Formato()
        {
            return Cls_Cat_Ort_Formato_Ficha_Inspeccion_Datos.Alta_Formato(this);
        }
        #endregion
    }

}