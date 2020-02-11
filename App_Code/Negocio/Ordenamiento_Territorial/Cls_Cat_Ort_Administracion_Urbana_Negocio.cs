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
using Presidencia.Orden_Territorial_Administracion_Urbana.Datos;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;
using System.Data.OracleClient;

namespace Presidencia.Orden_Territorial_Administracion_Urbana.Negocio
{
    public class Cls_Cat_Ort_Administracion_Urbana_Negocio
    {
        #region Variables Internas

        //  para la consulta de los formatos a llenar por medio del empleado id
        private String Empleado_Id;
        private String Cuenta_Predial;

        //  para los id
        private String Administracion_Urbana_ID;
        private String TRAMITE_ID;
        private String SOLICITUD_ID;
        private String SUBPROCESO_ID;
        private String ESTATUS;
        private String INSPECTOR_ID;

        //  para la area
        private String AREA_INSPECCION;
        private String AREA_CALLE;
        private String AREA_COLONIA;
        private String AREA_NO_FISICO;
        private String AREA_MANZANA;
        private String AREA_LOTE;
        private String AREA_ZONA;
        private String AREA_USO_SOLICITADO;

        //  para los tipos de supervision
        private String TIPO_SUPERVISION_ID;

        //  para los tipos de condicion del inmueble
        private String CONDICION_INMUEBLE_ID;

        //  para el avance de la obra
        private String AVANCE_OBRA_ID;
        private String AVANCE_BARDEO_APROX;
        private String AVANCE_NIVELES_ACTUALES;
        private String AVANCES_NIVELES_CONSTRUCCION;
        private String AVANCE_PROYECTO_ACORDE;

        //  para las vias publicas y areas de donacion
        private String VIA_PUBLICA_INVASION_MATERIAL;
        private String VIA_PUBLICA_INVASION_DONACION;
        private String VIA_PUBLICA_SOBRE_MARQUESINA;
        private String VIA_PUBLICA_PARAMENTO;
        private String AREA_VIA_ID;
        private String AREA_VIA_ESPECIF_RESTRICCION;

        //  para lo referente a las inspecciones
        private String INSPECCION_NOTIFICADO;
        private String INSPECCION_FOLIO_NOTIFICADO;
        private String INSPECCION_ACTA;
        private String INSPECCION_FOLIO_ACTA;
        private String INSPECCION_CLAUSURADO;
        private String INSPECCION_FOLIO_CLAUSURADO;
        private String INSPECCION_MULTADO;
        private String INSPECCION_FOLIO_MULTADO;

        //  para el uso actual
        private String USO_ACTUAL_ID;
        private String USO_ACORDE_SOLICITADO;
        private String ESPECIFICAR_TIPO_USO;

        //  para el uso predominantes
        private String USOS_PRE_COLINDANTES;
        private String USOS_PRE_FRENTE_INMUEBLE;
        private String USOS_PRE_IMPACTO_CONSIDERAR;

        //  para el funcionamiento
        private String FUNCIONAMIENTO_ACTIVIDAD;
        private String FUNCIONAMIENTO_METROS;
        private String FUNCIONAMIENTO_MAQUINARIA;
        private String FUNCIONAMIENTO_ID;
        private String FUNCIONAMIENTO_NO_PERSONAL;
        private String FUNCIONAMIENTO_NO_CLIENTES;

        //  para los anuncions
        private String ANUNCIO_TIPO_1;
        private String ANUNCIO_DIMENSIONES_1;
        private String ANUNCIO_TIPO_2;
        private String ANUNCIO_DIMENSIONES_2;
        private String ANUNCIO_TIPO_3;
        private String ANUNCIO_DIMENSIONES_3;
        private String ANUNCIO_TIPO_4;
        private String ANUNCIO_DIMENSIONES_4;

        //  para los servicios
        private String SERVICIO_CUENTA;
        private String SERVICIO_WC;
        private String SERVICIO_LAVABO;
        private String SERVICIO_LETRINA;
        private String SERVICIO_MIXTO;
        private String SERVICIO_NO_SANITARIO_HOMBRE;
        private String SERVICIO_NO_SANITARIO_MUJER;
        private String SERVICIO_AGUA_POTABLE;
        private String SERVICIO_AGUA_ABASTC_PARTI;
        private String SERVICIO_AGUA_ABASTC_JAPAMI;
        private String SERVICIO_DRENAJE;
        private String SERVICIO_FOSA_SEPTICA;
        private String SERVICIO_ESTACIONAMIENTO;
        private String SERVICIO_ESTAC_PROPIO;
        private String SERVICIO_ESTAC_RENTADO;
        private String SERVICIO_ESTAC_NO_CAJONES;
        private String SERVICIO_ESTAC_AREA_DESCARGA;
        private String SERVICIO_ESTAC_DOMICILIO;

        //  para materiales
        private String MATERIAL_EMPLEADO_MUROS;
        private String MATERIAL_EMPLEADO_TECHO;

        //  para la seguridad
        private String SEGURIDAD_MEDIDAS;
        private String SEGURIDAD_EQUIPO;
        private String SEGURIDAD_MATERIAL_FLAMABLE;
        private String SEGURIDAD_ESPECIFICAR;

        //  para la poda de arboles
        private String PODA_ALTURA;
        private String PODA_DIAMETRO_TRONCO;
        private String PODA_DIAMETRO_FRONDA;
        private String PODA_ESTADO;
        private String PODA_TIPO_PODA;
        private String PODA_TIPO_CANTIDAD_PODA;
        private String PODA_TIPO_TALA;
        private String PODA_TIPO_CANTIDAD_TALA;
        private String PODA_TIPO_TRASPLANTE;
        private String PODA_TIPO_CANTIDAD_TRASPLANTE;

        //  para los generales    
        private DateTime GENERALES_RECEPC_INSPECTOR;
        private DateTime GENERALES_FECHA_CAMPO;
        private DateTime GENERALES_RECEPC_COORD;
        private String GENERALES_OBSERVACION_INSPEC;
        private String GENERALES_OBSERVACION_PARA;

        //  para el usuario
        private String Usuario_Creo;

        private String Plantilla_ID;
        private String Nombre_Plantilla;

        //  para los datos del manifiesto de impacto ambiental
        private String IMPACTO_AFECTACIONES;
        private String IMPACTO_COLINDANCIAS;
        private String IMPACTO_SUPERFICIE;
        private String IMPACTO_TIPO_PROYECTO;

        //  para los datos del banco de materiales
        private String MATERIAL_PERMISO_ECOLOGIA;
        private String MATERIAL_PERMISO_SUELO;
        private String MATERIAL_SUPERFICIE_TOTAL;
        private String MATERIAL_PROFUNDIDAD;
        private String MATERIAL_INCLINACION;
        private String MATERIAL_FLORA;
        private String MATERIAL_ACCESO_VEHICULOS;
        private String MATERIAL_PETREO;
        private String MATERIAL_ESPECIE_ARBOL;

        private String MATERIALES_TIPO_PODA;
        private String MATERIALES_CANTIDAD_PODA;
        private String MATERIALES_TIPO_TALA;
        private String MATERIALES_CANTIDAD_TALA;
        private String MATERIALES_TIPO_TRASPLANTE;
        private String MATERIALES_CANTIDAD_TRASPLAN;


        //  para los datos de la autorizacion
        private String AUTORIZA_SUELOS;
        private String AUTORIZA_AREA_RESIDUOS;
        private String AUTORIZA_SEPARACION;
        private String AUTORIZA_METODO_SEPARACION;
        private String AUTORIZA_SERVICIO_RECOLEC;
        private String AUTORIZA_REVUELVEN_SOLD_LIQU;
        private String AUTORIZA_TIPO_CONTENEDOR;
        private String AUTORIZA_TIPO_RUIDO;
        private String AUTORIZA_NIVEL_RUIDO;
        private String AUTORIZA_HORARIO_LABORES;
        private String AUTORIZA_LUNES;
        private String AUTORIZA_MARTES;
        private String AUTORIZA_MIERCOLES;
        private String AUTORIZA_JUEVES;
        private String AUTORIZA_VIERNES;
        private String AUTORIZA_SABADO;
        private String AUTORIZA_DOMINGO;
        private String AUTORIZA_EMISIONES;

        //  para los datos de la licencia de autorizacion de funcionamiento
        private String LICENCIA_TIPO_EQUIPO;
        private String LICENCIA_TIPO_EMISION;
        private String LICENCIA_HORARIO_FUNCIONAM;
        private String LICENCIA_TIPO_COMBUSTIBLE;
        private String LICENCIA_GASTO_COMBUSTIBLE;
        private String LICENCIA_ALMACENAJE;
        private String LICENCIA_CANTIDAD_COMBUSTIBLE;

        private String Evaluacion_Solicitud;
        
        private DataTable Dt_Residuos;

        private OracleCommand Cmmd;

        #endregion

        #region Variables Publicas
        //  para la consulta de los formatos a llenar por medio del empleado id
        public String P_Empleado_Id
        {
            get { return Empleado_Id; }
            set { Empleado_Id = value; }
        }
        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        
        //  para los id
        public String P_Administracion_Urbana_ID
        {
            get { return Administracion_Urbana_ID; }
            set { Administracion_Urbana_ID = value; }
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
        public String P_Estatus
        {
            get { return ESTATUS; }
            set { ESTATUS = value; }
        }        
        public String P_Inspector_ID
        {
            get { return INSPECTOR_ID; }
            set { INSPECTOR_ID = value; }
        }
        

        //  para la area
        public String P_Area_Inspeccion
        {
            get { return AREA_INSPECCION; }
            set { AREA_INSPECCION = value; }
        }
        public String P_Area_Calle
        {
            get { return AREA_CALLE; }
            set { AREA_CALLE = value; }
        }
        public String P_Area_Colonia
        {
            get { return AREA_COLONIA; }
            set { AREA_COLONIA = value; }
        }
        public String P_Area_Numero_Fisico
        {
            get { return AREA_NO_FISICO; }
            set { AREA_NO_FISICO = value; }
        }
        public String P_Area_Manzana
        {
            get { return AREA_MANZANA; }
            set { AREA_MANZANA = value; }
        }
        public String P_Area_Lote
        {
            get { return AREA_LOTE; }
            set { AREA_LOTE = value; }
        }
        public String P_Area_Zona
        {
            get { return AREA_ZONA; }
            set { AREA_ZONA = value; }
        }
        public String P_Area_Uso_Solicitado
        {
            get { return AREA_USO_SOLICITADO; }
            set { AREA_USO_SOLICITADO = value; }
        }

        //  para los tipos de supervision
        public String P_Tipo_Supervision_ID
        {
            get { return TIPO_SUPERVISION_ID; }
            set { TIPO_SUPERVISION_ID = value; }
        }

        //  para los tipos de condicion del inmueble
        public String P_Condiciones_Inmueble_ID
        {
            get { return CONDICION_INMUEBLE_ID; }
            set { CONDICION_INMUEBLE_ID = value; }
        }

        //  para el avance de la obra
        public String P_Avance_Obra_ID
        {
            get { return AVANCE_OBRA_ID; }
            set { AVANCE_OBRA_ID = value; }
        }
         public String P_Avance_Bardeo_Aproximado
        {
            get { return AVANCE_BARDEO_APROX; }
            set { AVANCE_BARDEO_APROX = value; }
        }     
        public String P_Avance_Niveles_Actuales
        {
            get { return AVANCE_NIVELES_ACTUALES; }
            set { AVANCE_NIVELES_ACTUALES = value; }
        }
        public String P_Avance_Niveles_Construccion
        {
            get { return AVANCES_NIVELES_CONSTRUCCION; }
            set { AVANCES_NIVELES_CONSTRUCCION = value; }
        }
        public String P_Avance_Proyecto_Acorde
        {
            get { return AVANCE_PROYECTO_ACORDE; }
            set { AVANCE_PROYECTO_ACORDE = value; }
        }

        //  para las vias publicas y areas de donacion
        public String P_VIA_PUBLICA_INVASION_MATERIAL
        {
            get { return VIA_PUBLICA_INVASION_MATERIAL; }
            set { VIA_PUBLICA_INVASION_MATERIAL = value; }
        }
        public String P_VIA_PUBLICA_INVASION_DONACION
        {
            get { return VIA_PUBLICA_INVASION_DONACION; }
            set { VIA_PUBLICA_INVASION_DONACION = value; }
        }
        public String P_VIA_PUBLICA_SOBRE_MARQUESINA
        {
            get { return VIA_PUBLICA_SOBRE_MARQUESINA; }
            set { VIA_PUBLICA_SOBRE_MARQUESINA = value; }
        }
        public String P_VIA_PUBLICA_PARAMENTO
        {
            get { return VIA_PUBLICA_PARAMENTO; }
            set { VIA_PUBLICA_PARAMENTO = value; }
        }
        public String P_Area_Via_ID
        {
            get { return AREA_VIA_ID; }
            set { AREA_VIA_ID = value; }
        }
        public String P_Area_Via_Especificar_Restricciones
        {
            get { return AREA_VIA_ESPECIF_RESTRICCION; }
            set { AREA_VIA_ESPECIF_RESTRICCION = value; }
        }

        //  para lo referente a las inspecciones
        public String P_Inspeccion_Notificacion
        {
            get { return INSPECCION_NOTIFICADO; }
            set { INSPECCION_NOTIFICADO = value; }
        }
        public String P_Inspeccion_Notificacion_Folio
        {
            get { return INSPECCION_FOLIO_NOTIFICADO; }
            set { INSPECCION_FOLIO_NOTIFICADO = value; }
        }
        public String P_Inspeccion_Acta
        {
            get { return INSPECCION_ACTA; }
            set { INSPECCION_ACTA = value; }
        }
        public String P_Inspeccion_Acta_Folio
        {
            get { return INSPECCION_FOLIO_ACTA; }
            set { INSPECCION_FOLIO_ACTA = value; }
        }
        public String P_Inspeccion_Clausurado
        {
            get { return INSPECCION_CLAUSURADO; }
            set { INSPECCION_CLAUSURADO = value; }
        }
        public String P_Inspeccion_Clausurado_Folio
        {
            get { return INSPECCION_FOLIO_CLAUSURADO; }
            set { INSPECCION_FOLIO_CLAUSURADO = value; }
        }
        public String P_Inspeccion_Multado
        {
            get { return INSPECCION_MULTADO; }
            set { INSPECCION_MULTADO = value; }
        }
        public String P_Inspeccion_Multado_Folio
        {
            get { return INSPECCION_FOLIO_MULTADO; }
            set { INSPECCION_FOLIO_MULTADO = value; }
        }

        //  para el uso actual
        public String P_Uso_Actual_ID
        {
            get { return USO_ACTUAL_ID; }
            set { USO_ACTUAL_ID = value; }
        }
        public String P_Uso_Actual_Acorde_Solicitado
        {
            get { return USO_ACORDE_SOLICITADO; }
            set { USO_ACORDE_SOLICITADO = value; }
        }
        public String P_Uso_Actual_Especificar_Tipo_Uso
        {
            get { return ESPECIFICAR_TIPO_USO; }
            set { ESPECIFICAR_TIPO_USO = value; }
        }

        //  para el uso predominantes
        public String P_Uso_Predominante_Colindantes
        {
            get { return USOS_PRE_COLINDANTES; }
            set { USOS_PRE_COLINDANTES = value; }
        }
        public String P_Uso_Predominante_Frente_Inmueble
        {
            get { return USOS_PRE_FRENTE_INMUEBLE; }
            set { USOS_PRE_FRENTE_INMUEBLE = value; }
        }
        public String P_Uso_Predominante_Impacto
        {
            get { return USOS_PRE_IMPACTO_CONSIDERAR; }
            set { USOS_PRE_IMPACTO_CONSIDERAR = value; }
        }

        //  para el funcionamiento
        public String P_Funcionamiento_Actividad
        {
            get { return FUNCIONAMIENTO_ACTIVIDAD; }
            set { FUNCIONAMIENTO_ACTIVIDAD = value; }
        }
        public String P_Funcionamiento_Metros_Cuadrados
        {
            get { return FUNCIONAMIENTO_METROS; }
            set { FUNCIONAMIENTO_METROS = value; }
        }
        public String P_Funcionamiento_Maquinaria
        {
            get { return FUNCIONAMIENTO_MAQUINARIA; }
            set { FUNCIONAMIENTO_MAQUINARIA = value; }
        }
        public String P_Funcionamiento_ID
        {
            get { return FUNCIONAMIENTO_ID; }
            set { FUNCIONAMIENTO_ID = value; }
        }
        public String P_Funcionamiento_No_Personas
        {
            get { return FUNCIONAMIENTO_NO_PERSONAL; }
            set { FUNCIONAMIENTO_NO_PERSONAL = value; }
        }
        public String P_Funcionamiento_No_Clientes
        {
            get { return FUNCIONAMIENTO_NO_CLIENTES; }
            set { FUNCIONAMIENTO_NO_CLIENTES = value; }
        }

        //  para los anuncions
        public String P_Anuncio_1
        {
            get { return ANUNCIO_TIPO_1; }
            set { ANUNCIO_TIPO_1 = value; }
        }
        public String P_Anuncio_1_Dimensiones
        {
            get { return ANUNCIO_DIMENSIONES_1; }
            set { ANUNCIO_DIMENSIONES_1 = value; }
        }
        public String P_Anuncio_2
        {
            get { return ANUNCIO_TIPO_2; }
            set { ANUNCIO_TIPO_2 = value; }
        }
        public String P_Anuncio_2_Dimensiones
        {
            get { return ANUNCIO_DIMENSIONES_2; }
            set { ANUNCIO_DIMENSIONES_2 = value; }
        }
        public String P_Anuncio_3
        {
            get { return ANUNCIO_TIPO_3; }
            set { ANUNCIO_TIPO_3 = value; }
        }
        public String P_Anuncio_3_Dimensiones
        {
            get { return ANUNCIO_DIMENSIONES_3; }
            set { ANUNCIO_DIMENSIONES_3 = value; }
        }
        public String P_Anuncio_4
        {
            get { return ANUNCIO_TIPO_4; }
            set { ANUNCIO_TIPO_4 = value; }
        }
        public String P_Anuncio_4_Dimensiones
        {
            get { return ANUNCIO_DIMENSIONES_4; }
            set { ANUNCIO_DIMENSIONES_4 = value; }
        }

        //  para los servicios
        public String P_Servicios_Cuenta_Sanitarios
        {
            get { return SERVICIO_CUENTA; }
            set { SERVICIO_CUENTA = value; }
        }
        public String P_Servicios_WC
        {
            get { return SERVICIO_WC; }
            set { SERVICIO_WC = value; }
        }
        public String P_Servicios_Lavabo
        {
            get { return SERVICIO_LAVABO; }
            set { SERVICIO_LAVABO = value; }
        }
        public String P_Servicios_Letrina
        {
            get { return SERVICIO_LETRINA; }
            set { SERVICIO_LETRINA = value; }
        }
        public String P_Servicios_Mixto
        {
            get { return SERVICIO_MIXTO; }
            set { SERVICIO_MIXTO = value; }
        }
        public String P_Servicios_Numero_Sanitarios_Hombres
        {
            get { return SERVICIO_NO_SANITARIO_HOMBRE; }
            set { SERVICIO_NO_SANITARIO_HOMBRE = value; }
        }
        public String P_Servicios_Numero_Sanitarios_Mujeres
        {
            get { return SERVICIO_NO_SANITARIO_MUJER; }
            set { SERVICIO_NO_SANITARIO_MUJER = value; }
        }
        public String P_Servicios_Agua_Potable
        {
            get { return SERVICIO_AGUA_POTABLE; }
            set { SERVICIO_AGUA_POTABLE = value; }
        }
        public String P_Servicios_Agua_Abastecimiento_Particular
        {
            get { return SERVICIO_AGUA_ABASTC_PARTI; }
            set { SERVICIO_AGUA_ABASTC_PARTI = value; }
        }
        public String P_Servicios_Agua_Abastecimiento_Japami
        {
            get { return SERVICIO_AGUA_ABASTC_JAPAMI; }
            set { SERVICIO_AGUA_ABASTC_JAPAMI = value; }
        }
        public String P_Servicios_Drenaje
        {
            get { return SERVICIO_DRENAJE; }
            set { SERVICIO_DRENAJE = value; }
        }
        public String P_Servicios_Fosa_Septica
        {
            get { return SERVICIO_FOSA_SEPTICA; }
            set { SERVICIO_FOSA_SEPTICA = value; }
        }
        public String P_Servicios_Estacionamiento
        {
            get { return SERVICIO_ESTACIONAMIENTO; }
            set { SERVICIO_ESTACIONAMIENTO = value; }
        }
        public String P_Servicios_Estacionamiento_Propio
        {
            get { return SERVICIO_ESTAC_PROPIO; }
            set { SERVICIO_ESTAC_PROPIO = value; }
        }
        public String P_Servicios_Estacionamiento_Rentado
        {
            get { return SERVICIO_ESTAC_RENTADO; }
            set { SERVICIO_ESTAC_RENTADO = value; }
        }
        public String P_Servicios_Estacionamiento_Numero_Cajones
        {
            get { return SERVICIO_ESTAC_NO_CAJONES; }
            set { SERVICIO_ESTAC_NO_CAJONES = value; }
        }
        public String P_Servicios_Estacionamiento_Area_Descarga
        {
            get { return SERVICIO_ESTAC_AREA_DESCARGA; }
            set { SERVICIO_ESTAC_AREA_DESCARGA = value; }
        }
        public String P_Servicios_Estacionamiento_Domicilio
        {
            get { return SERVICIO_ESTAC_DOMICILIO; }
            set { SERVICIO_ESTAC_DOMICILIO = value; }
        }

        //  para los materiales
        public String P_Materiales_Empleado_Muros
        {
            get { return MATERIAL_EMPLEADO_MUROS; }
            set { MATERIAL_EMPLEADO_MUROS = value; }
        }
        public String P_Materiales_Empleado_Techos
        {
            get { return MATERIAL_EMPLEADO_TECHO; }
            set { MATERIAL_EMPLEADO_TECHO = value; }
        }

        //  para los aspectos de seguridad
        public String P_Seguridad_Medidas
        {
            get { return SEGURIDAD_MEDIDAS; }
            set { SEGURIDAD_MEDIDAS = value; }
        }
        public String P_Seguridad_Equipo
        {
            get { return SEGURIDAD_EQUIPO; }
            set { SEGURIDAD_EQUIPO = value; }
        }
        public String P_Seguridad_Material_Flamable
        {
            get { return SEGURIDAD_MATERIAL_FLAMABLE; }
            set { SEGURIDAD_MATERIAL_FLAMABLE = value; }
        }
        public String P_Seguridad_Especificar
        {
            get { return SEGURIDAD_ESPECIFICAR; }
            set { SEGURIDAD_ESPECIFICAR = value; }
        }

        //  para la poda de arboles
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
        public String P_Poda_Estado
        {
            get { return PODA_ESTADO; }
            set { PODA_ESTADO = value; }
        }

        //  para los generales
        public DateTime P_Generales_Recepcion_Inspector
        {
            get { return GENERALES_RECEPC_INSPECTOR; }
            set { GENERALES_RECEPC_INSPECTOR = value; }
        }
        public DateTime P_Generales_Fecha_Revision_Campo
        {
            get { return GENERALES_FECHA_CAMPO; }
            set { GENERALES_FECHA_CAMPO = value; }
        }
        public DateTime P_Generales_Recepcion_Coordinacion
        {
            get { return GENERALES_RECEPC_COORD; }
            set { GENERALES_RECEPC_COORD = value; }
        }
        public String P_Generales_Observaciones_Inspector
        {
            get { return GENERALES_OBSERVACION_INSPEC; }
            set { GENERALES_OBSERVACION_INSPEC = value; }
        }
        public String P_Generales_Observaciones_Para_Inspector
        {
            get { return GENERALES_OBSERVACION_PARA; }
            set { GENERALES_OBSERVACION_PARA = value; }
        }

        //  para el usuario
        public String P_Usuario
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }

        public String P_Plantilla_ID
        {
            get { return Plantilla_ID; }
            set { Plantilla_ID = value; }
        }
        public String P_Nombre_Plantilla
        {
            get { return Nombre_Plantilla; }
            set { Nombre_Plantilla = value; }
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
        public String P_Licencia_Almacenaje
        {
            get { return LICENCIA_ALMACENAJE; }
            set { LICENCIA_ALMACENAJE = value; }
        }
        public String P_Licencia_Cantidad_Combustible
        {
            get { return LICENCIA_CANTIDAD_COMBUSTIBLE; }
            set { LICENCIA_CANTIDAD_COMBUSTIBLE = value; }
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
        public String P_Material_Especie_Arbol
        {
            get { return MATERIAL_ESPECIE_ARBOL; }
            set { MATERIAL_ESPECIE_ARBOL = value; }
        }


        public String P_Material_Tipo_Poda
        {
            get { return MATERIALES_TIPO_PODA; }
            set { MATERIALES_TIPO_PODA = value; }
        }

        public String P_Material_Cantidad_Poda
        {
            get { return MATERIALES_CANTIDAD_PODA; }
            set { MATERIALES_CANTIDAD_PODA = value; }
        }
        public String P_Material_Tipo_Tala
        {
            get { return MATERIALES_TIPO_TALA; }
            set { MATERIALES_TIPO_TALA = value; }
        }
        public String P_Material_Cantidad_Tala
        {
            get { return MATERIALES_CANTIDAD_TALA; }
            set { MATERIALES_CANTIDAD_TALA = value; }
        }
        public String P_Material_Tipo_Trasplante
        {
            get { return MATERIALES_TIPO_TRASPLANTE; }
            set { MATERIALES_TIPO_TRASPLANTE = value; }
        }
        public String P_Material_Cantidad_Trasplante
        {
            get { return MATERIALES_CANTIDAD_TRASPLAN; }
            set { MATERIALES_CANTIDAD_TRASPLAN = value; }
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
        public String P_Autoriza_Sabado
        {
            get { return AUTORIZA_SABADO; }
            set { AUTORIZA_SABADO = value; }
        }
        public String P_Autoriza_Domingo
        {
            get { return AUTORIZA_DOMINGO; }
            set { AUTORIZA_DOMINGO = value; }
        }

        public String P_Autoriza_Emisiones
        {
            get { return AUTORIZA_EMISIONES; }
            set { AUTORIZA_EMISIONES = value; }
        }


        public String P_Evaluacion_Solicitud
        {
            get { return Evaluacion_Solicitud; }
            set { Evaluacion_Solicitud = value; }
        }
        public DataTable P_Dt_Residuos
        {
            get { return Dt_Residuos; }
            set { Dt_Residuos = value; }
        }
        public OracleCommand P_Cmmd
        {
            get { return Cmmd; }
            set { Cmmd = value; }
        }
        #endregion

        #region Consultas
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Zonas
        ///DESCRIPCIÓN          : Metodo para consultar los datos de las zonas
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Zonas()
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Zonas(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tipo_Supervision
        ///DESCRIPCIÓN          : Metodo para consultar los tipos de supervision 
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Tipo_Supervision()
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Tipo_Supervision(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Condiciones_Inmueble
        ///DESCRIPCIÓN          : Metodo para consultar los tipos de condiciones del inmueble 
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Condiciones_Inmueble()
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Condiciones_Inmueble(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Condiciones_Inmueble
        ///DESCRIPCIÓN          : Metodo para consultar los tipos de condiciones del inmueble 
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Avance_Obra()
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Avance_Obra(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Condiciones_Inmueble
        ///DESCRIPCIÓN          : Metodo para consultar los tipos de condiciones de la via publica
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Condiciones_Via_Publica_Donacion()
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Condiciones_Via_Publica_Donacion(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Condiciones_Inmueble
        ///DESCRIPCIÓN          : Metodo para consultar los tipos de condiciones del terreno
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Uso_Actual_Terreno()
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Uso_Actual_Terreno(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Condiciones_Inmueble
        ///DESCRIPCIÓN          : Metodo para consultar los tipos de materiales
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Tipos_Materiales()
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Tipos_Materiales(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Problemas_Funcionamiento
        ///DESCRIPCIÓN          : Metodo para consultar los tipos de materiales
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Problemas_Funcionamiento()
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Problemas_Funcionamiento(this);
        }  
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Llenado_Solicitud_Formato
        ///DESCRIPCIÓN          : Metodo que consultara los formatos que se deben de llenar
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Llenado_Solicitud_Formato()
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Llenado_Solicitud_Formato(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Inspectores
        ///DESCRIPCIÓN          : Metodo que consultara  los inspectores que se encuentran registrados
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Inspectores()
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Inspectores(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Formato_ID
        ///DESCRIPCIÓN          : Metodo que consultara el id del formato
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Formato_ID()
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Formato_ID(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Administracion_Urbana
        ///DESCRIPCIÓN          : Metodo que consultara el id del formato
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Administracion_Urbana(Cls_Cat_Ort_Administracion_Urbana_Negocio Parametros)
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Administracion_Urbana(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tabla_Administracion_Urbana
        ///DESCRIPCIÓN          : Metodo que consultara el id del formato
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Tabla_Administracion_Urbana(Cls_Cat_Ort_Administracion_Urbana_Negocio Parametros)
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Tabla_Administracion_Urbana(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Residuos
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 1.- Parametros.Contiene los parametros que se van a utilizar para
        ///                       hacer la consulta de la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public DataTable Consultar_Residuos()
        {
            return Cls_Cat_Ort_Administracion_Urbana_Datos.Consultar_Residuos(this);
        }

        #endregion

        #region Alta
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Guardar_Usuario
        ///DESCRIPCIÓN          : Metodo para guardar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012 
        ///*********************************************************************************************************
        public void Guardar_Formato()
        {
            Cls_Cat_Ort_Administracion_Urbana_Datos.Alta_Formato(this);
        }
        #endregion

        public void Modificar_Formato()
        {
            Cls_Cat_Ort_Administracion_Urbana_Datos.Modificar_Formato(this);
        }
    }
}
