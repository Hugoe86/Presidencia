using System;
using System.Data;
using Presidencia.Bandeja_Solicitudes_Tramites.Datos;
using System.Data.OracleClient;

namespace Presidencia.Bandeja_Solicitudes_Tramites.Negocio
{
    public class Cls_Ope_Bandeja_Tramites_Negocio
    {
        #region Variables Internas

        private String Solicitud_ID;
        private String Clave_Solicitud;
        private Double Porcentaje_Avance;
        private String Tramite;
        private String Solicito;
        private String Subproceso_ID;
        private String Subproceso_Nombre;
        private String Estatus;
        private String Comentarios;
        private DateTime Fecha_Solicitud;
        private String Tipo_DataTable;
        private DataTable Datos_Solicitud;
        private DataTable Documentos_Solicitud;
        private DataTable Plantillas_Subproceso;
        private Boolean Enviar_Correo_Electronico;
        private String Correo_Electronico;
        private String Empleado_ID;
        private String Usuario;
        private String Dependencia_ID;
        private String Tramite_id;
        private String Consecutivo;
        private String Cuenta_Predial;
        private String Inspector_ID;
        private String Zona_ID;
        private String Rol_ID;
        private OracleCommand Comando_Oracle;
        private String Comentarios_Internos;
        private DateTime Fecha_Entraga;
        private Double Tiempo_Estimado;
        private String Tipo_Actividad;
        private Double Condicion_Si;
        private Double Condicion_No;
        private String Respuesta_Condicion;
        private String Folio;
        private String Area_Dependencia;
        private String Nombre;
        private String Apellido_Paterno;
        private String Apellido_Materno;
        private String Tipo_Dato;
        private DataTable Dt_Datos;
        private String[,] Datos;
        private String SubProceso_Anterior;
        private String Orden;
        private Double Costo_Total;
        private Double Costo_Base;
        private Double Unidades;
        private String Email;
        private String Contribuyente_Id;
        private Double Orden_Actividad;
        private String DIRECCION_PREDIO;
        private String PROPIETARIO_PREDIO;
        private String Calle_Predio;
        private String Nuemro_Predio;
        private String Manzana_Predio;
        private String Lote_Predio;
        private String Fecha_Inicio;
        private String Fecha_Fin;
        private String Otros;
        private String Fecha_Vigencia_Inicio;
        private String Fecha_Vigencia_Fin;
        private String Fecha_Documento_Vigencia_inicio;
        private String Fecha_Documento_Vigencia_Fin;
        private String Persona_Inspecciona;
        private String Estatus_Persona_Inspecciona;
        private DateTime Fecha_Date_Vigencia_Inicio;
        private DateTime Fecha_Date_Vigencia_Fin;
        private DateTime Date_Fecha_Documento_Vigencia_Inicio;
        private DateTime Date_Fecha_Documento_Vigencia_Fin;
        private String Ubicacion_Expediente;
        private String Complemento;
        private DataTable Solicitudes_Complementarias;

        #endregion

        #region Variables Publicas
        public Double P_Porcentaje_Avance
        {
            get { return Porcentaje_Avance; }
            set { Porcentaje_Avance = value; }
        }

        public String P_Tramite
        {
            get { return Tramite; }
            set { Tramite = value; }
        }

        public String P_Solicito
        {
            get { return Solicito; }
            set { Solicito = value; }
        }

        public String P_Subproceso_ID
        {
            get { return Subproceso_ID; }
            set { Subproceso_ID = value; }
        }

        public String P_Subproceso_Nombre
        {
            get { return Subproceso_Nombre; }
            set { Subproceso_Nombre = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public DateTime P_Fecha_Solicitud
        {
            get { return Fecha_Solicitud; }
            set { Fecha_Solicitud = value; }
        }
        public DateTime P_Fecha_Entraga
        {
            get { return Fecha_Entraga; }
            set { Fecha_Entraga = value; }
        }

        public String P_Solicitud_ID
        {
            get { return Solicitud_ID; }
            set { Solicitud_ID = value; }
        }

        public String P_Clave_Solicitud
        {
            get { return Clave_Solicitud; }
            set { Clave_Solicitud = value; }
        }

        public String P_Tipo_DataTable
        {
            get { return Tipo_DataTable; }
            set { Tipo_DataTable = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public DataTable P_Datos_Solicitud
        {
            get { return Datos_Solicitud; }
            set { Datos_Solicitud = value; }
        }

        public DataTable P_Documentos_Solicitud
        {
            get { return Documentos_Solicitud; }
            set { Documentos_Solicitud = value; }
        }

        public DataTable P_Plantillas_Subproceso
        {
            get { return Plantillas_Subproceso; }
            set { Plantillas_Subproceso = value; }
        }

        public Boolean P_Enviar_Correo_Electronico
        {
            get { return Enviar_Correo_Electronico; }
            set { Enviar_Correo_Electronico = value; }
        }

        public String P_Correo_Electronico
        {
            get { return Correo_Electronico; }
            set { Correo_Electronico = value; }
        }

        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Tramite_id
        {
            get { return Tramite_id; }
            set { Tramite_id = value; }
        }

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Inspector_ID
        {
            get { return Inspector_ID; }
            set { Inspector_ID = value; }
        }
        public String P_Zona_ID
        {
            get { return Zona_ID; }
            set { Zona_ID = value; }
        }
        public String P_Rol_ID
        {
            get { return Rol_ID; }
            set { Rol_ID = value; }
        }

        public OracleCommand P_Comando_Oracle
        {
            get { return Comando_Oracle; }
            set { Comando_Oracle = value; }
        }

        public String P_Comentarios_Internos
        {
            get { return Comentarios_Internos; }
            set { Comentarios_Internos = value; }
        }


        public Double P_Tiempo_Estimado
        {
            get { return Tiempo_Estimado; }
            set { Tiempo_Estimado = value; }
        }
        public Double P_Orden_Actividad
        {
            get { return Orden_Actividad; }
            set { Orden_Actividad = value; }
        }

        public String P_Tipo_Actividad
        {
            get { return Tipo_Actividad; }
            set { Tipo_Actividad = value; }
        }
        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public String P_Respuesta_Condicion
        {
            get { return Respuesta_Condicion; }
            set { Respuesta_Condicion = value; }
        }
        public String P_Area_Dependencia
        {
            get { return Area_Dependencia; }
            set { Area_Dependencia = value; }
        }

        public Double P_Condicion_Si
        {
            get { return Condicion_Si; }
            set { Condicion_Si = value; }
        }
        public Double P_Condicion_No
        {
            get { return Condicion_No; }
            set { Condicion_No = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_Apellido_Paterno
        {
            get { return Apellido_Paterno; }
            set { Apellido_Paterno = value; }
        }
        public String P_Apellido_Materno
        {
            get { return Apellido_Materno; }
            set { Apellido_Materno = value; }
        }
        public String P_Tipo_Dato
        {
            get { return Tipo_Dato; }
            set { Tipo_Dato = value; }
        }
        public DataTable P_Dt_Datos
        {
            get { return Dt_Datos; }
            set { Dt_Datos = value; }
        }
        public String[,] P_Datos
        {
            get { return Datos; }
            set { Datos = value; }
        }
        public String P_SubProceso_Anterior
        {
            get { return SubProceso_Anterior; }
            set { SubProceso_Anterior = value; }
        }

        public String P_Orden
        {
            get { return Orden; }
            set { Orden = value; }
        }
        public Double P_Costo_Total
        {
            get { return Costo_Total; }
            set { Costo_Total = value; }
        }
        public Double P_Costo_Base
        {
            get { return Costo_Base; }
            set { Costo_Base = value; }
        }
        public String P_Contribuyente_Id
        {
            get { return Contribuyente_Id; }
            set { Contribuyente_Id = value; }
        }
        public String P_Email
        {
            get { return Email; }
            set { Email = value; }
        }
        public Double P_Unidades
        {
            get { return Unidades; }
            set { Unidades = value; }
        }
        public String P_Consecutivo
        {
            get { return Consecutivo; }
            set { Consecutivo = value; }
        }

        public String P_Direccion_Predio
        {
            get { return DIRECCION_PREDIO; }
            set { DIRECCION_PREDIO = value; }
        }
        public String P_Propietario_Predio
        {
            get { return PROPIETARIO_PREDIO; }
            set { PROPIETARIO_PREDIO = value; }
        }
        public String P_Calle_Predio
        {
            get { return Calle_Predio; }
            set { Calle_Predio = value; }
        }
        public String P_Nuemro_Predio
        {
            get { return Nuemro_Predio; }
            set { Nuemro_Predio = value; }
        }
        public String P_Manzana_Predio
        {
            get { return Manzana_Predio; }
            set { Manzana_Predio = value; }
        }
        public String P_Lote_Predio
        {
            get { return Lote_Predio; }
            set { Lote_Predio = value; }
        }
        public String P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }
        public String P_Fecha_Fin
        {
            get { return Fecha_Fin; }
            set { Fecha_Fin = value; }
        }
        public String P_Otros_Predio
        {
            get { return Otros; }
            set { Otros = value; }
        }

        public String P_Fecha_Vigencia_Inicio
        {
            get { return Fecha_Vigencia_Inicio; }
            set { Fecha_Vigencia_Inicio = value; }
        }
        public String P_Fecha_Vigencia_Fin
        {
            get { return Fecha_Vigencia_Fin; }
            set { Fecha_Vigencia_Fin = value; }
        }
        public String P_Persona_Inspecciona
        {
            get { return Persona_Inspecciona; }
            set { Persona_Inspecciona = value; }
        }

        public String P_Fecha_Documento_Vigencia_inicio
        {
            get { return Fecha_Documento_Vigencia_inicio; }
            set { Fecha_Documento_Vigencia_inicio = value; }
        }
        public String P_Fecha_Documento_Vigencia_Fin
        {
            get { return Fecha_Documento_Vigencia_Fin; }
            set { Fecha_Documento_Vigencia_Fin = value; }
        }

        public String P_Estatus_Persona_Inspecciona
        {
            get { return Estatus_Persona_Inspecciona; }
            set { Estatus_Persona_Inspecciona = value; }
        }
        public String P_Ubicacion_Expediente
        {
            get { return Ubicacion_Expediente; }
            set { Ubicacion_Expediente = value; }
        }
        public DateTime P_Fecha_Date_Vigencia_Inicio
        {
            get { return Fecha_Date_Vigencia_Inicio; }
            set { Fecha_Date_Vigencia_Inicio = value; }
        }
        public DateTime P_Fecha_Date_Vigencia_Fin
        {
            get { return Fecha_Date_Vigencia_Fin; }
            set { Fecha_Date_Vigencia_Fin = value; }
        }

        public DateTime P_Date_Fecha_Documento_Vigencia_Inicio
        {
            get { return Date_Fecha_Documento_Vigencia_Inicio; }
            set { Date_Fecha_Documento_Vigencia_Inicio = value; }
        }
        public DateTime P_Date_Fecha_Documento_Vigencia_Fin
        {
            get { return Date_Fecha_Documento_Vigencia_Fin; }
            set { Date_Fecha_Documento_Vigencia_Fin = value; }
        }
        public String P_Complemento
        {
            get { return Complemento; }
            set { Complemento = value; }
        }
        public DataTable P_Solicitudes_Complementarias
        {
            get { return Solicitudes_Complementarias; }
            set { Solicitudes_Complementarias = value; }
        }

        #endregion

        #region Metodos

        public Cls_Ope_Bandeja_Tramites_Negocio Consultar_Datos_Solicitud()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Datos_Solicitud(this);
        }

        public DataTable Consultar_Datos_Cedula_Visita()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Datos_Cedula_Visita(this);
        }
        public DataTable Consultar_Datos_Ficha_Revision()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Datos_Ficha_Revision(this);
        }

        public DataTable Consultar_Solicitud_Director_Ordenamiento()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Solicitud_Director_Ordenamiento(this);
        }

        public Cls_Ope_Bandeja_Tramites_Negocio Consultar_Solicitud()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Solicitud(this);
        }

        public DataTable Consultar_DataTable()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_DataTable(this);
        }

        public Cls_Ope_Bandeja_Tramites_Negocio Evaluar_Solicitud()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Evaluar_Solicitud(this);
        }

        public DataTable Consultar_Actividad_Condicional()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Actividad_Condicional(this);
        }

        public DataTable Consultar_Siguiente_Actividad()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Siguiente_Actividad(this);
        }

        public DataTable Consultar_Comentarios_Internos()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Comentarios_Internos(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Solicitud
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:
        ///CREO: Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO: 12/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public Boolean Modificar_Datos_Dictamen()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Modificar_Datos_Dictamen(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Solicitud
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:
        ///CREO: Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO: 12/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public Boolean Alta_Datos_Dictamen()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Alta_Datos_Dictamen(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Solicitud
        ///DESCRIPCIÓN: Llama la clase de datos para realizar la consulta y la conexion a la bd
        ///PARAMETROS:
        ///CREO: Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO: 12/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public Boolean Modificar_Costo_Solicitud()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Modificar_Costo_Solicitud(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Solicitudes_Dependencia
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 1.Parametros.Contiene los parametros que se van a utilizar para
        ///                       hacer la consulta de la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************  
        public DataTable Consultar_Solicitudes_Dependencia()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Solicitudes_Dependencia(this);
        }


        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Plantillas
        ///DESCRIPCIÓN          : Metodo que consultara la informacion de los detalles de las plantillas
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 24/Julio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Valor_Subproceso_ID()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Valor_Subproceso_ID(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Costo_Complemento
        ///DESCRIPCIÓN          : Metodo que consultara la informacion del costo de las solicitudes complemento
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 10/Noviembre/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Costo_Complemento()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Costo_Complemento(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Costo_Complemento
        ///DESCRIPCIÓN          : Metodo que consultara la informacion del costo de las solicitudes complemento
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 10/Noviembre/2012 
        ///*********************************************************************************************************
        public String Consultar_Penalizaciones()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Penalizaciones(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Costo_Principal
        ///DESCRIPCIÓN          : Metodo que consultara la informacion del costo de la solicitud principal
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 10/Noviembre/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Costo_Principal()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Costo_Principal(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Solicitudes_Hija
        ///DESCRIPCIÓN          : Metodo que consultara la informacion de los detalles de las solicitudes hijas
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 06/Noviembre/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Solicitudes_Hija()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Solicitudes_Hija(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Contribuyente
        ///DESCRIPCIÓN          : Metodo que consultara la informacion del contribuyente
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 20/Septiembre/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Datos_Contribuyente()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Datos_Contribuyente(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Plantillas
        ///DESCRIPCIÓN          : Metodo que consultara la informacion de los detalles de las plantillas
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 24/Julio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Tipo_Actividad()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Tipo_Actividad(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Plantillas
        ///DESCRIPCIÓN          : Metodo que consultara la informacion de los detalles de las plantillas
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 24/Julio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Anterior_Actividades_Condicional()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Anterior_Actividades_Condicional(this);
        }


        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Plantillas
        ///DESCRIPCIÓN          : Metodo que consultara la informacion de los detalles de las plantillas
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 24/Julio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Datos_Finales()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Datos_Finales(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Plantillas
        ///DESCRIPCIÓN          : consultara el valor de la matriz de costo
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 15/Octubre/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Costo_Matriz()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Costo_Matriz(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Plantillas
        ///DESCRIPCIÓN          : Metodo que consultara la informacion de los detalles de las plantillas
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 24/Julio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Datos_Finales_Operacion()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Datos_Finales_Operacion(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Plantillas
        ///DESCRIPCIÓN          : Metodo que consultara la informacion de los detalles de las plantillas
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 28/Mayo/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Detalles_Plantillas()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Detalles_Plantillas(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Fechas_Solicitud
        ///DESCRIPCIÓN          : Metodo para modificar las fechas de 
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 24/Julio/2012 
        ///*********************************************************************************************************
        public Boolean Modificar_Fechas_Solicitud()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Modificar_Fechas_Solicitud(this);
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Fechas_Solicitud
        ///DESCRIPCIÓN          : Metodo para modificar las fechas de 
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Agosto/2012 
        ///*********************************************************************************************************
        public Boolean Modificar_Zona()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Modificar_Zona(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Formatos
        ///DESCRIPCIÓN          : Metodo que consultara la informacion de los detalles de las formatos
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 28/Mayo/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Detalles_Formatos()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Detalles_Formatos(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Actividades_Realizadas
        ///DESCRIPCIÓN          : Metodo que consultara las actividades realizadas
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 20/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Actividades_Realizadas()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Actividades_Realizadas(this);
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tiempo_Estimado
        ///DESCRIPCIÓN          : Metodo que consultara el tiempo estimado del proceso de la solicitud
        ///PROPIEDADES          :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 25/Junio/2012 
        ///*********************************************************************************************************
        public DataTable Consultar_Tiempo_Estimado()
        {
            return Cls_Ope_Bandeja_Tramites_Datos.Consultar_Tiempo_Estimado(this);
        }

        public void Alta_Detalles_Solicitud(Cls_Ope_Bandeja_Tramites_Negocio Solicitud)
        {
            Cls_Ope_Bandeja_Tramites_Datos.Alta_Detalles_Solicitud(this);
        }

        #endregion

    }

}