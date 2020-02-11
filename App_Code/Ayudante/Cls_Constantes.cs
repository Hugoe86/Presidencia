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

/// <summary>
/// Summary description for Cls_Constantes
/// </summary>fig


namespace Presidencia.Constantes
{

    public class Cls_Constantes
    {
        //public const string Str_Conexion = "Data Source=172.16.0.103;Persist Security Info=True;User ID=system;Password=jromero;Unicode=True;";
        //public const string Str_Conexion = "Data Source=200.33.34.70;Persist Security Info=True;User ID=system;Password=jromero;Unicode=True";
        //public const string Str_Conexion = "Data Source=localhost;Persist Security Info=True;User ID=system;Password=jromero;Unicode=True";
        // public const string Str_Conexion = "Data Source=172.16.1.64;Persist Security Info=True;User ID=system;Password=jromero;Unicode=True;";
        public static string Str_Conexion = ConfigurationManager.ConnectionStrings["Irapuato"].ConnectionString;
    }

    ///**********************************************************************************************************************************
    ///                                                           ATENCION CIUDADANA
    ///**********************************************************************************************************************************

    #region Atencion Ciudadana
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Ate_Acciones
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_ATE_ACCIONES
    /// PARÁMETROS :     
    /// CREO       : Gustavo Angeles Cruz
    /// FECHA_CREO : 19 Abril 2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Ate_Acciones
    {
        public const String Tabla_Cat_Ate_Acciones = "CAT_ATE_ACCIONES";
        public const String Campo_Accion_ID = "ACCION_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Tiempo_Estimado_Solucion = "TIEMPO_ESTIMADO_SOLUCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Ate_Programas
    /// DESCRIPCIÓN: Clase que contiene los c-ampos de la tabla CAT_ATE_PROGRAMAS
    /// PARÁMETROS :     
    /// CREO       : Gustavo Angeles Cruz
    /// FECHA_CREO : 19 Abril 2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Ate_Programas
    {
        public const String Tabla_Cat_Ate_Programas = "CAT_ATE_PROGRAMAS";
        public const String Campo_Programa_ID = "PROGRAMA_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Prefijo_Folio = "PREFIJO_FOLIO";
        public const String Campo_Folio_Anual = "FOLIO_ANUAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Ate_Parametros_Correo
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_ATE_PARAMETROS_CORREO
    /// PARÁMETROS :     
    /// CREO       : Roberto González Oseguera
    /// FECHA_CREO : 15-oct-2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Ate_Parametros_Correo
    {
        public const String Tabla_Cat_Ate_Parametros_Correo = "CAT_ATE_PARAMETROS_CORREO ";
        public const String Campo_Correo_Puerto = "CORREO_PUERTO";
        public const String Campo_Correo_Servidor = "CORREO_SERVIDOR";
        public const String Campo_Correo_Remitente = "CORREO_REMITENTE";
        public const String Campo_Password_Usuario_Correo = "PASSWORD_USUARIO_CORREO";
        public const String Campo_Correo_Saludo = "CORREO_SALUDO";
        public const String Campo_Correo_Cuerpo = "CORREO_CUERPO";
        public const String Campo_Correo_Despedida = "CORREO_DESPEDIDA";
        public const String Campo_Correo_Firma = "CORREO_FIRMA";
        public const String Campo_Tipo_Correo = "TIPO_CORREO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Ate_Peticiones
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_ATE_PETICIONES
    /// PARÁMETROS :     
    /// CREO       : Silvia Morales Portuhondo
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Ate_Peticiones
    {
        public const String Tabla_Ope_Ate_Peticiones = "OPE_ATE_PETICIONES";
        public const String Campo_Peticion_ID = "NO_PETICION";    //eliminar cuando ya no tenga referencias
        public const String Campo_No_Peticion = "NO_PETICION";
        public const String Campo_Anio_Peticion = "ANIO_PETICION";
        public const String Campo_Programa_ID = "PROGRAMA_ID";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Peticion = "FECHA_PETICION";
        public const String Campo_Fecha_Solucion_Probable = "FECHA_SOLUCION_PROBABLE";
        public const String Campo_Nivel_Importancia = "NIVEL_IMPORTANCIA";
        public const String Campo_Genera_Noticia = "GENERA_NOTICIA";
        public const String Campo_Nombre_Solicitante = "NOMBRE";
        public const String Campo_Apellido_Paterno = "APELLIDO_PATERNO";
        public const String Campo_Apellido_Materno = "APELLIDO_MATERNO";
        public const String Campo_Edad = "EDAD";
        public const String Campo_Fecha_Nacimiento = "FECHA_NACIMIENTO";
        public const String Campo_Sexo = "SEXO";
        public const String Campo_Calle_No = "CALLE_NO";    //eliminar cuando ya no tenga referencias (ya no existe en la base de datos)
        public const String Campo_Colonia_ID = "COLONIA_ID";
        public const String Campo_Localidad_ID = "LOCALIDAD_ID";    //eliminar cuando ya no tenga referencias (ya no existe en la base de datos)
        public const String Campo_Calle_ID = "CALLE_ID";
        public const String Campo_Numero_Exterior = "NUMERO_EXTERIOR";
        public const String Campo_Numero_Interior = "NUMERO_INTERIOR";
        public const String Campo_Referencia = "REFERENCIA";
        public const String Campo_Codigo_Postal = "CODIGO_POSTAL";
        public const String Campo_Telefono = "TELEFONO";
        public const String Campo_Email = "E_MAIL";
        public const String Campo_Descripcion_Peticion = "DESCRIPCION_PETICION";
        public const String Campo_Asunto_ID = "ASUNTO_ID";
        public const String Campo_Accion_ID = "ACCION_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Area_ID = "AREA_ID";
        public const String Campo_Origen_De_Registro = "ORIGEN_DE_REGISTRO";
        public const String Campo_Descripcion_Solucion = "DESCRIPCION_SOLUCION";
        public const String Campo_Fecha_Solucion_Real = "FECHA_SOLUCION_REAL";
        public const String Campo_Asignado = "ASIGNADO";
        public const String Campo_Contribuyente_ID = "CONTRIBUYENTE_ID";
        public const String Campo_Tipo_Solucion = "TIPO_SOLUCION";
        public const String Campo_Tipo_Consecutivo = "TIPO_CONSECUTIVO";
        public const String Campo_Por_Validar = "POR_VALIDAR";
        public const String Campo_Nombre_Atendio = "NOMBRE_ATENDIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    public class Ope_Ate_Seguimiento_Peticiones
    {
        public const String Tabla_Ope_Ate_Seguimiento_Peticiones = "OPE_ATE_SEGUIMIENTO_PETICIONES";
        public const String Campo_Seguimiento_ID = "SEGUIMIENTO_ID";    //eliminar cuando ya no tenga referencias (ya no existe en la base de datos)
        public const String Campo_No_Seguimiento = "NO_SEGUIMIENTO";
        public const String Campo_Peticion_ID = "NO_PETICION";    //eliminar cuando ya no tenga referencias
        public const String Campo_No_Peticion = "NO_PETICION";
        public const String Campo_Anio_Peticion = "ANIO_PETICION";
        public const String Campo_Programa_ID = "PROGRAMA_ID";
        public const String Campo_Asunto_ID = "ASUNTO_ID";
        public const String Campo_Area_ID = "AREA_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Fecha_Asignacion = "FECHA_ASIGNACION";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Ate_Asuntos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_ATE_ASUNTOS
    /// PARÁMETROS :     
    /// CREO       : Silvia Morales Portuhondo
    /// FECHA_CREO : 19/Agosto/2010 
    /// MODIFICO          :Gustavo AC
    /// FECHA_MODIFICO    :19 ABRIL 2012
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Ate_Asuntos
    {
        public const String Tabla_Cat_Ate_Asuntos = "CAT_ATE_ASUNTOS";
        public const String Campo_AsuntoID = "ASUNTO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_DependenciaID = "DEPENDENCIA_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_AreaID = "AREA_ID";
        public const String Campo_UsuarioCreo = "USUARIO_CREO";
        public const String Campo_FechaCreo = "FECHA_CREO";
        public const String Campo_UsuarioModifico = "USUARIO_MODIFICO";
        public const String Campo_FechaModifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
  NOMBRE DE LA CLASE:Ope_Ate_Asigna_Pet_Empleado
  DESCRIPCIÓN: Clase que contiene los datos de la Tabla Ope_Ate_Asigna_Pet_Empleado
  PARÁMETROS :
  CREO       : Silvia Morales Portuhondo
  FECHA_CREO : 30/Septiembre/2010
  MODIFICO   :
  FECHA_MODIFICO:
  CAUSA_MODIFICACIÓN:
 *******************************************************************************/
    public class Ope_Ate_Asigna_Pet_Empleado
    {
        public const String Tabla_Ope_Asigna_Pet_Empleado = "OPE_ATE_ASIGNA_PET_EMPLEADO";
        public const String Campo_Asignacion_ID = "ASIGNACION_ID";    //eliminar cuando ya no tenga referencias (ya no existe en la base de datos)
        public const String Campo_No_Asignacion = "NO_ASIGNACION";
        public const String Campo_Peticion_ID = "NO_PETICION";    //eliminar cuando ya no tenga referencias
        public const String Campo_No_Peticion = "NO_PETICION";
        public const String Campo_Anio_Peticion = "ANIO_PETICION";
        public const String Campo_Programa_ID = "PROGRAMA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Vigente = "VIGENTE";
        public const String Campo_UsuarioCreo = "USUARIO_CREO";
        public const String Campo_FechaCreo = "FECHA_CREO";
        public const String Campo_UsuarioModifico = "USUARIO_MODIFICO";
        public const String Campo_FechaModifico = "FECHA_MODIFICO";

    }//fin de Ope_Ate_Asigna_Pet_Empleado




    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Detalle_Ate_Peticiones
    ///       DESCRIPCIÓN: Clase que contiene los campos de la tabla Detalle_Ate_Peticiones
    ///              CREO: Alberto Pantoja Hernández
    ///        FECHA_CREO: 26/8/2010
    ///          MODIFICO: 
    ///    FECHA_MODIFICO: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Detalle_Ate_Peticiones
    {
        public const String Tabla_Detalle_Ate_Peticiones = "DETALLE_ATE_PETICIONES";
        public const String Campo_Detalle_Peticion_ID = "DETALLE_PETICION_ID";
        public const String Campo_Peticion_ID = "PETICION_ID";
        public const String Campo_Dependencia_ID_Origen = "DEPENDENCIA_ID_ORIGEN";
        public const String Campo_Dependencia_ID_Destino = "DEPENDENCIA_ID_DESTINO";
        public const String Campo_Descripcion_Cambio = "DESCRIPCION_CAMBIO";
        public const String Campo_Fecha_Asignacion_Cambio = "FECHA_ASIGNACION_CAMBIO";
        public const String Campo_Area_ID = "AREA_ID";
        public const String Campo_UsuarioCreo = "USUARIO_CREO";
        public const String Campo_FechaCreo = "FECHA_CREO";
        public const String Campo_UsuarioModifico = "USUARIO_MODIFICO";
        public const String Campo_FechaModifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Ate_Colonias
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_ATE_COLONIAS
    /// PARÁMETROS :
    /// CREO       : Susana Trigueros Armenta
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Ate_Colonias
    {
        public const String Tabla_Cat_Ate_Colonias = "CAT_ATE_COLONIAS";
        public const String Campo_Colonia_ID = "COLONIA_ID";
        public const String Campo_Tipo_Colonia_ID = "TIPO_COLONIA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Costo_Construccion = "COSTO_CONSTRUCCION";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Deducible = "DEDUCIBLE_20_SALARIOS";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Sector_ID = "SECTOR_ID";
        public const String Campo_Estatus = "ESTATUS";
    }

    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Ate_Observaciones_Peticiones
    DESCRIPCIÓN: Clase que contiene los datos de la Tabla OPE_ATE_OBSERVACIONES_PETIC
    PARÁMETROS : NO APLICA
    CREO       : Roberto González Oseguera
    FECHA_CREO : 23-may-2012
    MODIFICO   :
    FECHA_MODIFICO:
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Ate_Observaciones_Peticiones
    {
        public const String Tabla_Ope_Ate_Observaciones_Peticiones = "OPE_ATE_OBSERVACIONES_PETIC";
        public const String Campo_No_Observacion = "NO_OBSERVACION";
        public const String Campo_No_Peticion = "NO_PETICION";
        public const String Campo_Anio_Peticion = "ANIO_PETICION";
        public const String Campo_Programa_ID = "PROGRAMA_ID";
        public const String Campo_Observacion = "OBSERVACION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }//fin de Ope_Ate_Observaciones_Peticiones

    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Ate_Vacantes
    DESCRIPCIÓN: Clase que contiene los datos de la Tabla OPE_ATE_VACANTES
    PARÁMETROS : NO APLICA
    CREO       : Roberto González Oseguera
    FECHA_CREO : 29-may-2012
    MODIFICO   :
    FECHA_MODIFICO:
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Ate_Vacantes
    {
        public const String Tabla_Ope_Ate_Vacantes = "OPE_ATE_VACANTES";
        public const String Campo_No_Vacante = "NO_VACANTE";
        public const String Campo_Nombre_Vacante = "NOMBRE_VACANTE";
        public const String Campo_Edad = "EDAD";
        public const String Campo_Sexo = "SEXO";
        public const String Campo_Escolaridad = "ESCOLARIDAD";
        public const String Campo_Experiencia = "EXPERIENCIA";
        public const String Campo_Sueldo = "SUELDO";
        public const String Campo_Contacto = "CONTACTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }//fin de Ope_Ate_Vacantes

    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Ate_Archivos_Peticiones
    DESCRIPCIÓN: Clase que contiene los datos de la Tabla OPE_ATE_ARCHIVOS_PETICIONES
    PARÁMETROS : 
    CREO       : Roberto González Oseguera
    FECHA_CREO : 20-jun-2012
    MODIFICO   :
    FECHA_MODIFICO:
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Ate_Archivos_Peticiones
    {
        public const String Tabla_Ope_Ate_Archivos_Peticiones = "OPE_ATE_ARCHIVOS_PETICIONES";
        public const string Campo_No_Archivo = "NO_ARCHIVO";
        public const string Campo_No_Peticion = "NO_PETICION";
        public const string Campo_Anio_Peticion = "ANIO_PETICION";
        public const string Campo_Programa_Id = "PROGRAMA_ID";
        public const string Campo_Ruta_Archivo = "RUTA_ARCHIVO";
        public const string Campo_Fecha = "FECHA";
        public const string Campo_Estatus_Peticion = "ESTATUS_PETICION";
        public const string Campo_Estatus_Archivo = "ESTATUS_ARCHIVO";
        public const string Campo_Usuario_Creo = "USUARIO_CREO";
        public const string Campo_Fecha_Creo = "FECHA_CREO";
        public const string Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const string Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }//fin de Ope_Ate_Archivos_Peticiones

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Ate_Programas_Empleados
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_ATE_PROGRAMAS_EMPLEADOS
    /// PARÁMETROS :
    /// CREO       : Roberto González Oseguera
    /// FECHA_CREO : 30-jun-2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Ate_Programas_Empleados
    {
        public const String Tabla_Cat_Ate_Programas_Empleados = "CAT_ATE_PROGRAMAS_EMPLEADOS";
        public const String Campo_Programa_Empleado_ID = "PROGRAMA_EMPLEADO_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Programa_ID = "PROGRAMA_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Ate_Correos_Enviados
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_ATE_CORREOS_ENVIADOS
    /// PARÁMETROS :
    /// CREO       : Roberto González Oseguera
    /// FECHA_CREO : 30-jun-2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Ate_Correos_Enviados
    {
        public const String Tabla_Ope_Ate_Correos_Enviados = "OPE_ATE_CORREOS_ENVIADOS";
        public const String Campo_No_Envio = "NO_ENVIO";
        public const String Campo_Destinatario = "DESTINATARIO";
        public const String Campo_Motivo = "MOTIVO";
        public const String Campo_Fecha_Notificacion = "FECHA_NOTIFICACION";
        public const String Campo_Contribuyente_ID = "CONTRIBUYENTE_ID";
        public const String Campo_Nombre_Ciudadano = "NOMBRE_CIUDADANO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    #endregion

    ///**********************************************************************************************************************************
    ///                                                                TRAMITES
    ///**********************************************************************************************************************************

    #region Tramites
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Tra_Parametros
    ///DESCRIPCIÓN          : Clase con los datos de la tabla CAT_TRA_PARAMETROS
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 02-Agosto-2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Tra_Parametros
    {
        public const String Tabla_Cat_Tra_Parametros = "CAT_TRA_PARAMETROS";
        public const String Campo_Correo_Encabezado = "CORREO_ENCABEZADO";
        public const String Campo_Correo_Cuerpo = "CORREO_CUERPO";
        public const String Campo_Correo_Despedida = "CORREO_DESPEDIDA";
        public const String Campo_Correo_Firma = "CORREO_FIRMA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:OPE_TRA_MATRIZ_COSTO
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_TRA_MATRIZ_COSTO
    /// PARÁMETROS :
    /// CREO       : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO : 10/octubre/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Tra_Matriz_Costo
    {
        public const String Tabla_Ope_Tra_Matriz_Costo = "OPE_TRA_MATRIZ_COSTO";
        public const String Campo_Matriz_ID = "MATRIZ_ID";
        public const String Campo_Tramite_ID = "TRAMITE_ID";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Costo_Base = "COSTO_BASE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Tra_Det_Solicitud
    ///DESCRIPCIÓN: Clase que contiene los campos
    /// PARÁMETROS :     
    /// CREO       : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO : 21/Junio/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Tra_Det_Solicitud
    {
        public const String Tabla_Ope_Tra_Det_Solicitud = "OPE_TRA_DET_SOLICITUD";
        public const String Campo_Detalle_Solicitud_ID = "DETALLE_SOLICITUD";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Subproceso_ID = "SUBPROCESO_ID";
        public const String Campo_Estatus = "ESTATUS ";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Fecha = "FECHA";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Tra_Det_Solicitud
    ///DESCRIPCIÓN: Clase que contiene los campos
    /// PARÁMETROS :     
    /// CREO       : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO : 21/Junio/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Tra_Det_Solicitud_Interna
    {
        public const String Tabla_Ope_Tra_Det_Solicitud_Interna = "OPE_TRA_DET_SOLICITUD_INTERNA";
        public const String Campo_Detalle_Solicitud_Interna_ID = "DETALLE_SOLICITUD_INTERNA";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Subproceso_ID = "SUBPROCESO_ID";
        public const String Campo_Estatus = "ESTATUS ";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Tra_Actividades
    ///DESCRIPCIÓN: Clase que contiene los campos
    /// PARÁMETROS :     
    /// CREO       : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO : 20/Junio/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Tra_Actividades
    {
        public const String Tabla_Cat_Tra_Actividades = "CAT_TRA_ACTIVIDADES";
        public const String Campo_Actividad_ID = "ACTIVIDAD_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Tra_Subprocesos_Perfiles
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla TRA_SUBPROCESOS_PERFILES
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 06/Octubre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Tra_Subprocesos_Perfiles
    {
        public const String Tabla_Tra_Subprocesos_Perfiles = "TRA_SUBPROCESOS_PERFILES";
        public const String Campo_Subproceso_ID = "SUBPROCESO_ID";
        public const String Campo_Perfil_ID = "PERFIL_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Tra_Tramites
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_TRA_TRAMITES
    /// PARÁMETROS :
    /// CREO       : JESUS TOLEDO
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Tra_Tramites
    {
        public const String Tabla_Cat_Tra_Tramites = "CAT_TRA_TRAMITES";
        public const String Campo_Tramite_ID = "TRAMITE_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Cuenta_ID = "NUMERO_CUENTA";
        public const String Campo_Clave_Tramite = "CLAVE_TRAMITE";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Tiempo_Estimado = "TIEMPO_ESTIMADO";
        public const String Campo_Costo = "COSTO";
        public const String Campo_Solicitar_Internet = "SOLICITAR_INTERNET";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Clave_Ingreso_ID = "CLAVE_INGRESO_ID";
        public const String Campo_Area_Dependencia = "AREA_DEPENDENCIA";
        public const String Campo_Estatus_Tramite = "ESTATUS_TRAMITE";
        public const String Campo_Parametro1 = "PARAMETRO1";
        public const String Campo_Parametro2 = "PARAMETRO2";
        public const String Campo_Parametro3 = "PARAMETRO3";
        public const String Campo_Operador1 = "OPERADOR1";
        public const String Campo_Operador2 = "OPERADOR2";
        public const String Campo_Operador3 = "OPERADOR3";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Tra_Datos_Tramite
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_TRA_DATOS_TRAMITE
    /// PARÁMETROS :
    /// CREO       : JESUS TOLEDO
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Tra_Datos_Tramite
    {
        public const String Tabla_Cat_Tra_Datos_Tramite = "CAT_TRA_DATOS_TRAMITE";
        public const String Campo_Dato_ID = "DATO_ID";
        public const String Campo_Tramite_ID = "TRAMITE_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Dato_Requerido = "DATO_REQUERIDO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Tipo_Dato = "TIPO_DATO";
        public const String Campo_Orden = "ORDEN";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Tra_Subprocesos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_TRA_SUBPROCESOS
    /// PARÁMETROS :
    /// CREO       :JESUS TOLEDO
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Tra_Subprocesos
    {
        public const String Tabla_Cat_Tra_Subprocesos = "CAT_TRA_SUBPROCESOS";
        public const String Campo_Subproceso_ID = "SUBPROCESO_ID";
        public const String Campo_Tramite_ID = "TRAMITE_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Valor = "VALOR";
        public const String Campo_Orden = "ORDEN";
        public const String Campo_Plantilla = "PLANTILLA_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Tipo_Actividad = "TIPO_ACTIVIDAD";

        public const String Campo_Condicion_Si = "CONDICION_SI";
        public const String Campo_Condicion_No = "CONDICION_NO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Tra_Plantillas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_TRA_PLANTILLAS
    /// PARÁMETROS :
    /// CREO       :Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 22/Octubre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Tra_Plantillas
    {
        public const String Tabla_Cat_Tra_Plantillas = "CAT_TRA_PLANTILLAS";
        public const String Campo_Plantilla_ID = "PLANTILLA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Archivo = "ARCHIVO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Tra_Datos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_TRA_DATOS
    /// PARÁMETROS :
    /// CREO       : JESUS TOLEDO
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Tra_Datos
    {
        public const String Tabla_Ope_Tra_Datos = "OPE_TRA_DATOS";
        public const String Campo_Ope_Dato_ID = "OPE_DATO_ID";
        public const String Campo_Dato_ID = "DATO_ID";
        public const String Campo_Tramite_ID = "TRAMITE_ID";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Valor = "VALOR";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Tipo_Dato = "TIPO_DATO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Tra_Solicitud
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_TRA_SOLICITUD
    /// PARÁMETROS :
    /// CREO       :JESUS TOLEDO
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Tra_Solicitud
    {
        public const String Tabla_Ope_Tra_Solicitud = "OPE_TRA_SOLICITUD";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Tramite_ID = "TRAMITE_ID";
        public const String Campo_Subproceso_ID = "SUBPROCESO_ID";
        public const String Campo_Clave_Solicitud = "CLAVE_SOLICITUD";
        public const String Campo_Porcentaje_Avance = "PORCENTAJE_AVANCE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Nombre_Solicitante = "NOMBRE";
        public const String Campo_Apellido_Paterno = "APELLIDO_PATERNO";
        public const String Campo_Apellido_Materno = "APELLIDO_MATERNO";
        public const String Campo_Correo_Electronico = "CORREO_ELECTRONICO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Fecha_Entrega = "FECHA_ENTREGA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Cuenta_Predial = "CUENTA_PREDIAL";
        public const String Campo_Inspector_ID = "INSPECTOR_ID";
        public const String Campo_Zona_ID = "ZONA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Costo_Base = "COSTO_BASE";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Costo_Total = "COSTO_TOTAL";
        public const String Campo_Actividad_Anterior = "SUBPROCESO_ANTERIOR";
        public const String Campo_Contribuyente_Id = "CONTRIBUYENTE_ID";
        public const String Campo_Consecutivo = "CONSECUTIVO";
        public const String Campo_Direccion_Predio = "DIRECCION_PREDIO";
        public const String Campo_Propietario_Predio = "PROPIETARIO_PREDIO";
        public const String Campo_Calle_Predio = "CALLE_PREDIO";
        public const String Campo_Numero_Predio = "NUMERO_PREDIO";
        public const String Campo_Manzana_Predio = "MANZANA_PREDIO";
        public const String Campo_Lote_Predio = "LOTE_PREDIO";
        public const String Campo_Parametro1 = "PARAMETRO1";
        public const String Campo_Parametro2 = "PARAMETRO2";
        public const String Campo_Operador1 = "OPERADOR1";
        public const String Campo_Operador2 = "OPERADOR2";
        public const String Campo_Otros_Predio = "OTROS_PREDIO";
        public const String Campo_Fecha_Vigencia_Inicio = "FECHA_VIG_INICIO";
        public const String Campo_Fecha_Vigencia_Fin = "FECHA_VIG_FIN";
        public const String Campo_Persona_Inspecciona = "PERSONA_INSPECCIONA";
        public const String Campo_Fecha_Condicion_Documento_Vigencia_Inicio = "FECHA_DOC_VIG_INICIO";
        public const String Campo_Fecha_Condicion_Documento_Vigencia_Fin = "FECHA_DOC_VIG_FIN";
        public const String Campo_Ubicacion_Expediente = "UBICACION_ARCHIVO";
        public const String Campo_Complemento = "COMPLEMENTO";
        public const String Campo_No_Poliza = "NO_POLIZA";
        public const String Campo_Tipo_Poliza_Id = "TIPO_POLIZA_ID";
        public const String Campo_Mes_Ano = "MES_ANO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Tra_Detalle_Documentos 
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla TRA_DETALLE_DOCUMENTOS
    /// PARÁMETROS :
    /// CREO       : Alberto Pantoja Hernández 
    /// FECHA_CREO : 20/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Tra_Detalle_Documentos
    {
        public const String Tabla_Tra_Detalle_Documentos = "TRA_DETALLE_DOCUMENTOS";
        public const String Campo_Detalle_Documento_ID = "DETALLE_DOCUMENTO_ID";
        public const String Campo_Documento_ID = "DOCUMENTO_ID";
        public const String Campo_Tramite_ID = "TRAMITE_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Documento_Requerido = "DOCUMENTO_REQUERIDO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Tra_Detalle_Autorizaciones
    /// ESCRIPCIÓN : Clase que contiene los campos de la tabla TRA_DETALLE_AUTORIZACIONES
    /// PARÁMETROS :
    /// CREO       : Alberto Pantoja Hernández 
    /// FECHA_CREO : 20/8/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Tra_Detalle_Autorizaciones
    {
        public const String Tabla_Tra_Detalle_Autorizaciones = "TRA_DETALLE_AUTORIZACIONES";
        public const String Campo_Detalle_Autorizacion_ID = "DETALLE_AUTORIZACION_ID";
        public const String Campo_Perfil_ID = "PERFIL_ID";
        public const String Campo_Tramite_ID = "TRAMITE_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Tra_Puestos_Perfiles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_TRA_PUESTOS_PERFILES
    /// PARÁMETROS :     
    /// CREO       : Silvia Morales Portuhondo
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Tra_Puestos_Perfiles
    {
        public const String Tabla_Cat_Tra_Puestos_Perfiles = "CAT_TRA_PUESTOS_PERFILES";
        public const String Campo_Perfil_ID = "PERFIL_ID";
        public const String Campo_Puesto_ID = "PUESTO_ID";
        public const String Campo_UsuarioCreo = "USUARIO_CREO";
        public const String Campo_FechaCreo = "FECHA_CREO";
        public const String Campo_UsuarioModifico = "USUARIO_MODIFICO";
        public const String Campo_FechaModifico = "FECHA_MODIFICO";
    }




    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Tra_Documentos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_TRA_DOCUMENTOS
    /// PARÁMETROS :
    /// CREO       : Alberto Pantoja Hernández 
    /// FECHA_CREO : 20/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Tra_Documentos
    {
        public const String Tabla_Cat_Tra_Documentos = "CAT_TRA_DOCUMENTOS";
        public const String Campo_Documento_ID = "DOCUMENTO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Tra_Perfiles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_TRA_PERFILES
    /// PARÁMETROS :
    /// CREO       : Alberto Pantoja Hernández 
    /// FECHA_CREO : 20/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Tra_Perfiles
    {
        public const String Tabla_Cat_Tra_Perfiles = "CAT_TRA_PERFILES";
        public const String Campo_Perfil_ID = "PERFIL_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Autoriza = "AUTORIZA";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Tra_Subproceso
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_TRA_SUBPROCESO
    /// PARÁMETROS :
    /// CREO       : Alberto Pantoja Hernández 
    /// FECHA_CREO : 20/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Tra_Subproceso
    {
        public const String Tabla_Ope_Tra_Subproceso = "OPE_TRA_SUBPROCESO";
        public const String Campo_Ope_Subproceso_ID = "OPE_SUBPROCESO_ID";
        public const String Campo_Subproceso_ID = "SUBPROCESO_ID";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Termino = "FECHA_TERMINO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Tra_Documentos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_TRA_DOCUMENTOS
    /// PARÁMETROS :
    /// CREO       : Susana Trigueros Armenta
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Tra_Documentos
    {
        public const String Tabla_Ope_Tra_Documentos = "OPE_TRA_DOCUMENTOS";
        public const String Campo_Ope_Documento_ID = "OPE_DOCUMENTO_ID";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Detalle_Documento_ID = "DETALLE_DOCUMENTO_ID";
        public const String Campo_URL = "URL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Tra_Formato_Predefinido
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_TRA_FORMATO_PREDEFINIDO
    /// PARÁMETROS :
    /// CREO       : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Tra_Formato_Predefinido
    {
        public const String Tabla_Cat_Tra_Formato_Predefinido = "CAT_TRA_FORMATO_PREDEFINIDO";
        public const String Campo_Formato_ID = "FORMATO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Archivo = "ARCHIVO";
        //public const String Campo_URL = "URL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Tra_Det_Sproc_Plantilla
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_TRA_DET_SPROC_PLANTILLA
    /// PARÁMETROS :
    /// CREO       : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Tra_Det_Sproc_Plantilla
    {
        public const String Tabla_Ope_Tra_Det_Sproc_Plantilla = "OPE_TRA_DET_SPROC_PLANTILLA";
        public const String Campo_Subproceso_ID = "SUBPROCESO_ID";
        public const String Campo_Tramite_ID = "TRAMITE_ID";
        public const String Campo_Plantilla_ID = "PLANTILLA_ID";
        public const String Campo_Detalle_Plantilla_ID = "DETALLE_PLANTILLA_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Tra_Det_Sproc_Formato
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_TRA_DET_SPROC_FORMATO
    /// PARÁMETROS :
    /// CREO       : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Tra_Det_Sproc_Formato
    {
        public const String Tabla_Ope_Tra_Det_Sproc_Formato = "OPE_TRA_DET_SPROC_FORMATO";
        public const String Campo_Subproceso_ID = "SUBPROCESO_ID";
        public const String Campo_Tramite_ID = "TRAMITE_ID";
        public const String Campo_Plantilla_ID = "PLANTILLA_ID";
        public const String Campo_Detalle_Formato_ID = "DETALLE_FORMATO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Tra_Perfiles_Empleado
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_TRA_PERFILES_EMPLEADO
    /// PARÁMETROS :
    /// CREO       : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO : 26/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Tra_Perfiles_Empleado
    {
        public const String Tabla_Ope_Tra_Perfiles_Empleado = "OPE_TRA_PERFILES_EMPLEADO";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Perfil_ID = "PERFIL_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    #endregion

    ///*************************************************************************************************************************
    ///                                                                CATASTRO
    ///*************************************************************************************************************************
    #region Catastro

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Asignacion_Cuentas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_ASIGNACION_CUENTAS
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Asignacion_Cuentas
    {
        public const String Tabla_Ope_Cat_Asignacion_Cuentas = "OPE_CAT_ASIGNACION_CUENTAS";
        public const String Campo_No_Asignacion = "NO_ASIGNACION";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Perito_Interno_Id = "PERITO_INTERNO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Anio = "ANIO";
        public const String Campo_No_Entrega = "NO_ENTREGA";
        public const String Campo_Folio_Predial = "FOLIO_PREDIAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Avaluo_Rustico_V
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_AVALUO_RUSTICO_V
    /// PARÁMETROS :     
    /// CREO       : Jose Guadalupe Guerrero Muñoz
    /// FECHA_CREO : 31/Agosto/2012  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Avaluo_Rustico_V
    {
        public const String Tabla_Ope_Cat_Avaluo_Rustico_V = "OPE_CAT_AVALUO_RUSTICO_V";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Motivo_Avaluo_Id = "MOTIVO_AVALUO_ID";
        public const String Campo_Cuenta_Predial = "CUENTA_PREDIAL";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Propietario = "PROPIETARIO";
        public const String Campo_Solicitante = "SOLICTANTE";
        public const String Campo_Clave_Catastral = "CLAVE_CATASTRAL";
        public const String Campo_Domicilio_Notificacion = "DOMICILIO_NOTIFICACION";
        public const String Campo_Municipio_Notificacion = "MUNICIPIO_NOTIFICACION";
        public const String Campo_Ubicacion = "UBICACION";
        public const String Campo_Localidad_Municipio = "LOCALIDAD_MUNICIPIO";
        public const String Campo_Nombre_Predio = "NOMBRE_PREDIO";
        public const String Campo_Coord_X_Grados = "COORD_X_GRADOS";
        public const String Campo_Coord_X_Minutos = "COORD_X_MINUTOS";
        public const String Campo_Coord_X_Segundos = "COORD_X_SEGUNDOS";
        public const String Campo_Orientacion_X = "ORIENTACION_X";
        public const String Campo_Coord_Y_Grados = "COORD_Y_GRADOS";
        public const String Campo_Coord_Y_Minutos = "COORD_Y_MINUTOS";
        public const String Campo_Coord_Y_Segundos = "COORD_Y_SEGUNDOS";
        public const String Campo_Orientacion_Y = "ORIENTACION_Y";
        public const String Campo_Base_Gravable = "BASE_GRAVABLE";
        public const String Campo_Impuesto_Bimestral = "IMPUESTO_BIMESTRAL";
        public const String Campo_Valor_Total_Predio = "VALOR_TOTAL_PREDIO";
        public const String Campo_Coord_Norte = "COORD_NORTE";
        public const String Campo_Coord_Sur = "COORD_SUR";
        public const String Campo_Coord_Oriente = "COORD_ORIENTE";
        public const String Campo_Coord_Poniente = "COORD_PONIENTE";
        public const String Campo_Veces_Rechazo = "VECES_RECHAZO";
        public const String Campo_Fecha_Rechazo = "FECHA_RECHAZO";
        public const String Campo_Fecha_Autorizo = "FECHA_AUTORIZO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Observaciones_Perito = "OBSERVACIONES_PERITO";
        public const String Campo_Perito_Externo_Id = "PERITO_EXTERNO_ID";
        public const String Campo_Perito_Interno_Id = "PERITO_INTERNO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Coordenadas_UTM = "COORDENADAS_UTM";
        public const String Campo_Coordenadas_UTM_Y = "COORDENADAS_Y_UTM";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Uso = "USO";
        public const String Campo_No_Asignacion = "NO_ASIGNACION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Lote = "LOTE";
        public const String Campo_Manzana = "MANZANA";
        public const String Campo_Region = "REGION";
        public const String Campo_Permitir_Revision = "PERMITIR_REVISION";
        public const String Campo_Comentarios_Revisor = "COMENTARIOS_REVISOR";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Calendario_Entregas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_CALENDARIO_ENTREGAS
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 19/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Calendario_Entregas
    {
        public const String Tabla_Cat_Cat_Calendario_Entregas = "CAT_CAT_CALENDARIO_ENTREGAS";
        public const String Campo_Fecha_Entrega_Id = "FECHA_ENTREGA_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Fecha_Primera_Entrega = "FECHA_PRIMERA_ENTREGA";
        public const String Campo_Fecha_Primera_Entrega_Real = "FECHA_PRIMERA_ENTREGA_REAL";
        public const String Campo_Fecha_Segunda_Entrega = "FECHA_SEGUNDA_ENTREGA";
        public const String Campo_Fecha_Segunda_Entrega_Real = "FECHA_SEGUNDA_ENTREGA_REAL";
        public const String Campo_Fecha_Tercera_Entrega = "FECHA_TERCERA_ENTREGA";
        public const String Campo_Fecha_Tercera_Entrega_Real = "FECHA_TERCERA_ENTREGA_REAL";
        public const String Campo_Fecha_Cuarta_Entrega = "FECHA_CUARTA_ENTREGA";
        public const String Campo_Fecha_Cuarta_Entrega_Real = "FECHA_CUARTA_ENTREGA_REAL";
        public const String Campo_Fecha_Quinta_Entrega = "FECHA_QUINTA_ENTREGA";
        public const String Campo_Fecha_Quinta_Entrega_Real = "FECHA_QUINTA_ENTREGA_REAL";
        public const String Campo_Fecha_Sexta_Entrega = "FECHA_SEXTA_ENTREGA";
        public const String Campo_Fecha_Sexta_Entrega_Real = "FECHA_SEXTA_ENTREGA_REAL";
        public const String Campo_Fecha_Septima_Entrega = "FECHA_SEPTIMA_ENTREGA";
        public const String Campo_Fecha_Septima_Entrega_Real = "FECHA_SEPTIMA_ENTREGA_REAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Documentos_Ara
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_DOCUMENTOS_ARA
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Documentos_Arv
    {
        public const String Tabla_Ope_Cat_Documentos_Arv = "OPE_CAT_DOCUMENTOS_ARV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Documento = "NO_DOCUMENTO";
        public const String Campo_Anio_Documento = "ANIO_DOCUMENTO";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Tabla_Ope_Cat_Calc_Terreno_Arv
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_TERRENO_ARV
    /// PARÁMETROS :     
    /// CREO       : Jose Guadalupe Guerrero Muñoz
    /// FECHA_CREO : 31/Agosto/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Terreno_Arv
    {
        public const String Tabla_Ope_Cat_Calc_Terreno_Arv = "OPE_CAT_CALC_TERRENO_ARV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Tipo_Constru_Rustico_Id = "TIPO_CONSTRU_RUSTICO_ID";
        public const String Campo_Valor_Constru_Rustico_Id = "VALOR_CONSTRU_RUSTICO_ID";
        public const String Campo_Superficie = "SUPERFICIE";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Valor_Const
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_VALOR_CONST
    /// PARÁMETROS :     
    /// CREO       : Jose Guadalupe Guerrero Muñoz
    /// FECHA_CREO : 31/Agosto/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Const
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Const = "OPE_CAT_CALC_VALOR_CONST";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Croquis = "CROQUIS";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Construccion_Id = "VALOR_CONSTRUCCION_ID";
        public const String Campo_Edad_Constru = "EDAD_CONSTRU";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Uso_Contru = "USO_CONSTRU";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Valor_Const_Arv
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_VALOR_CONST_ARV
    /// PARÁMETROS :     
    /// CREO       : Jose Guadalupe Guerrero Muñoz
    /// FECHA_CREO : 31/Agosto/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Const_Arv
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Const_Arv = "OPE_CAT_CALC_VALOR_CONST_ARV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Croquis = "CROQUIS";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Construccion_Id = "VALOR_CONSTRUCCION_ID";
        public const String Campo_Edad_Constru = "EDAD_CONSTRU";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Uso_Contru = "USO_CONTRU";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Caracteristicas_Arv
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CARACTERISTICAS_ARV
    /// PARÁMETROS :     
    //// CREO       : Jose Guadalupe Guerrero Muñoz
    /// FECHA_CREO : 31/Agosto/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Caracteristicas_Arv
    {
        public const String Tabla_Ope_Cat_Caracteristicas_Arv = "OPE_CAT_CARACTERISTICAS_ARV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Des_Constru_Rustico_Id = "DESC_CONSTRU_RUSTICO_ID";
        public const String Campo_Descripcion_Rustico_Id = "DESCRIPCION_RUSTICO_ID";
        public const String Campo_Valor_Indicador_A = "VALOR_INDICADOR_A";
        public const String Campo_Valor_Indicador_B = "VALOR_INDICADOR_B";
        public const String Campo_Valor_Indicador_C = "VALOR_INDICADOR_C";
        public const String Campo_Valor_Indicador_D = "VALOR_INDICADOR_D";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Elem_Construccion_Arv
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_ELEM_CONSTRUCCION_ARV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Elem_Construccion_Arv
    {
        public const String Tabla_Ope_Cat_Elem_Construccion_Arv = "OPE_CAT_ELEM_CONSTRUCCION_ARV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Elementos_Contruccion_Id = "ELEMENTOS_CONSTRUCCION_ID";
        public const String Campo_Elemento_Construccion_A = "ELEMENTO_CONSTRUCCION_A";
        public const String Campo_Elemento_Construccion_B = "ELEMENTO_CONSTRUCCION_B";
        public const String Campo_Elemento_Construccion_C = "ELEMENTO_CONSTRUCCION_C";
        public const String Campo_Elemento_Construccion_D = "ELEMENTO_CONSTRUCCION_D";
        public const String Campo_Elemento_Construccion_E = "ELEMENTO_CONSTRUCCION_E";
        public const String Campo_Elemento_Construccion_F = "ELEMENTO_CONSTRUCCION_F";
        public const String Campo_Elemento_Construccion_G = "ELEMENTO_CONSTRUCCION_G";
        public const String Campo_Elemento_Construccion_H = "ELEMENTO_CONSTRUCCION_H";
        public const String Campo_Elemento_Construccion_I = "ELEMENTO_CONSTRUCCION_I";
        public const String Campo_Elemento_Construccion_J = "ELEMENTO_CONSTRUCCION_J";
        public const String Campo_Elemento_Construccion_K = "ELEMENTO_CONSTRUCCION_K";
        public const String Campo_Elemento_Construccion_L = "ELEMENTO_CONSTRUCCION_L";
        public const String Campo_Elemento_Construccion_M = "ELEMENTO_CONSTRUCCION_M";
        public const String Campo_Elemento_Construccion_N = "ELEMENTO_CONSTRUCCION_N";
        public const String Campo_Elemento_Construccion_O = "ELEMENTO_CONSTRUCCION_O";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Seguimiento_Arv
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_SEGUIMIENTO_ARV
    /// PARÁMETROS :     
    //// CREO       : Jose Guadalupe Guerrero Muñoz
    /// FECHA_CREO : 31/Agosto/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Seguimiento_Arv
    {
        public const String Tabla_Ope_Cat_Seguimiento_Arv = "OPE_CAT_SEGUIMIENTO_ARV";
        public const String Campo_No_Seguimiento = "NO_SEGUIMIENTO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Motivo_Id = "MOTIVO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Colindancias_Arv
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_COLINDANCIAS_ARV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Colindancias_Arv
    {
        public const String Tabla_Ope_Cat_Colindancias_Arv = "OPE_CAT_COLINDANCIAS_ARV";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_No_Colindancia = "NO_COLINDANCIA";
        public const String Campo_Medida_Colindancia = "MEDIDA_COLINDANCIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Tramos_Calles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_TRAMOS_CALLE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 03/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Tramos_Calles
    {
        public const String Tabla_Cat_Cat_Tramos_Calle = "CAT_CAT_TRAMOS_CALLE";
        public const String Campo_Tramo_Id = "TRAMO_ID";
        public const String Campo_Tramo_Descripcion = "TRAMO_DESCRIPCION";
        public const String Campo_Calle_Id = "CALLE_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Motivos_Rechazo
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_MOTIVOS_RECHAZO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 05/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Motivos_Rechazo
    {
        public const String Tabla_Cat_Cat_Motivos_Rechazo = "CAT_CAT_MOTIVOS_RECHAZO";
        public const String Campo_Motivo_Id = "MOTIVO_ID";
        public const String Campo_Motivo_Descripcion = "MOTIVO_DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Motivos_Avaluo
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_MOTIVOS_AVALUO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 08/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Motivos_Avaluo
    {
        public const String Tabla_Cat_Cat_Motivos_Avaluo = "CAT_CAT_MOTIVOS_AVALUO";
        public const String Campo_Motivo_Avaluo_Id = "MOTIVO_AVALUO_ID";
        public const String Campo_Motivo_Avaluo_Descripcion = "MOTIVO_AVALUO_DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Factores_Cobro_Memorias_Descriptivas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_FACT_COBRO_MEMO_DESC
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 08/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Factores_Cobro_Memorias_Descriptivas
    {
        public const String Tabla_Cat_Cat_Factores_Cobro_Memorias_Descriptivas = "CAT_CAT_FACT_COBRO_MEMO_DESC";
        public const String Campo_Factor_Cobro_Id = "FACTOR_COBRO_MEM_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Cantidad_Cobro_1 = "CANTIDAD_COBRO_1";
        public const String Campo_Cantidad_Cobro_2 = "CANTIDAD_COBRO_2";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Tasas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_TASAS
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 08/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public class Cat_Cat_Tasas
    {
        public const String Tabla_Cat_Cat_Tasas = "CAT_CAT_TASAS";
        public const String Campo_Id_Tasa = "ID_TASA";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Con_Edificacion = "CON_EDIFICACION";
        public const String Campo_Sin_Edificacion = "SIN_EDIFICACION";
        public const String Campo_Valor_Rustico = "VALOR_RUSTICO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_CREO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Tipos_Construccion
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_TIPOS_CONSTRUCCION
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 09/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Tipos_Construccion
    {
        public const String Tabla_Cat_Cat_Tipos_Construccion = "CAT_CAT_TIPOS_CONSTRUCCION";
        public const String Campo_Tipo_Construccion_Id = "TIPO_CONSTRUCCION_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    public class Cat_Cat_Reg_Condominio
    {
        public const String Tabla_Cat_Cat_Reg_Condominio = "CAT_CAT_REG_CONDOMINIO";
        public const String Campo_Regimen_Condominio_ID = "REGIMEN_CONDOMINIO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Nombre_Documento = "NOMBRE_DOCUMENTO";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Tipos_Constru_Rustico
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_TIPOS_CONSTRU_RUSTICO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 29/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Tipos_Constru_Rustico
    {
        public const String Tabla_Cat_Cat_Tipos_Constru_Rustico = "CAT_CAT_TIPOS_CONSTRU_RUSTICO";
        public const String Campo_Tipo_Constru_Rustico_Id = "TIPO_CONSTRU_RUSTICO_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Tab_Val_Const_Rustico
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_TAB_VAL_CONST_RUSTICO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 09/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Tab_Val_Const_Rustico
    {
        public const String Tabla_Cat_Cat_Tab_Val_Const_Rustico = "CAT_CAT_TAB_VAL_CONST_RUSTICO";
        public const String Campo_Valor_Constru_Rustico_Id = "VALOR_CONSTRU_RUSTICO_ID";
        public const String Campo_Tipo_Constru_Rustico_Id = "TIPO_CONSTRU_RUSTICO_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Valor_M2 = "VALOR_M2";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Calidad_Construccion
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_CALIDAD_CONSTRUCCION
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 19/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Calidad_Construccion
    {
        public const String Tabla_Cat_Cat_Calidad_Construccion = "CAT_CAT_CALIDAD_CONSTRUCCION";
        public const String Campo_Calidad_Id = "CALIDAD_ID";
        public const String Campo_Tipo_Construccion_Id = "TIPO_CONSTRUCCION_ID";
        public const String Campo_Calidad = "CALIDAD";
        public const String Campo_Clave_Calidad = "CLAVE_CALIDAD";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Tab_Val_Inpr
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_TAB_VAL_INPR
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 19/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Tab_Val_Inpr
    {
        public const String Tabla_Cat_Cat_Tab_Val_Inpr = "CAT_CAT_TAB_VAL_INPR";
        public const String Campo_Valor_Inpr_Id = "VALOR_INPR_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Valor_Inpr = "VALOR_INPR";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Tab_Val_Inpa
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_TAB_VAL_INPA
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 19/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Tab_Val_Inpa
    {
        public const String Tabla_Cat_Cat_Tab_Val_Inpa = "CAT_CAT_TAB_VAL_INPA";
        public const String Campo_Valor_Inpa_Id = "VALOR_INPA_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Valor_Inpa = "VALOR_INPA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///Añadir campos a la tabla de cat_pre_cuentas_predial. region, manzana y lote

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Motivos_Avaluo
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_MOTIVOS_AVALUO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 08/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Tab_Val_Construccion
    {
        public const String Tabla_Cat_Cat_Tab_Val_Construccion = "CAT_CAT_TAB_VAL_CONSTRUCCION";
        public const String Campo_Calidad_Id = "CALIDAD_ID";
        public const String Campo_Valor_Construccion_Id = "VALOR_CONSTRUCCION_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Valor_M2 = "VALOR_M2";
        public const String Campo_Clave_Valor = "CLAVE_VALOR";
        public const String Campo_Estado_Conservacion = "ESTADO_CONSERVACION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Tab_Val_Catastrales
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_TAB_VAL_CATASTRALES
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 09/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Tab_Val_Catastrales
    {
        public const String Tabla_Cat_Cat_Tab_Val_Catastrales = "CAT_CAT_TAB_VAL_CATASTRALES";
        public const String Campo_Valor_Catastral_Id = "VALOR_CATASTRAL_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Cantidad_1 = "CANTIDAD_1";
        public const String Campo_Cantidad_2 = "CANTIDAD_2";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Tabla_Valores_Tramos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_MOTIVOS_AVALUO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 10/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Tabla_Valores_Tramos
    {
        public const String Tabla_Cat_Cat_Tabla_Valores_Tramos = "CAT_CAT_TABLA_VALORES_TRAMOS";
        public const String Campo_Valor_Tramo_Id = "VALOR_TRAMO_ID";
        public const String Campo_Tramo_Id = "TRAMO_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Valor_Tramo = "VALOR_TRAMO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Descri_Const_Rustico
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_DESCRIP_CONST_RUSTICO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 18/Junio/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public class Cat_Cat_Descrip_Const_Rustico
    {
        public const String Tabla_Cat_Cat_Descrip_Const_Rustico = "CAT_CAT_DESCRIP_CONST_RUSTICO";
        public const String Campo_Desc_Constru_Rustico_Id = "DESC_CONSTRU_RUSTICO_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Tabla_Descrip_Rustico
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_TABLA_DESCRIP_RUSTICO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Tabla_Descrip_Rustico
    {
        public const String Tabla_Cat_Cat_Tabla_Descrip_Rustico = "CAT_CAT_TABLA_DESCRIP_RUSTICO";
        public const String Campo_Descrip_Rustico_Id = "DESCRIPCION_RUSTICO_ID";
        public const String Campo_Des_Constru_Rustico_Id = "DESC_CONSTRU_RUSTICO_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Valor_Indice = "VALOR_INDICE";
        public const String Campo_Indicador_A = "INDICADOR_A";
        public const String Campo_Indicador_B = "INDICADOR_B";
        public const String Campo_Indicador_C = "INDICADOR_C";
        public const String Campo_Indicador_D = "INDICADOR_D";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Tabla_Valores_Tramos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_MOTIVOS_AVALUO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 10/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Parametros
    {
        public const String Tabla_Cat_Cat_Parametros = "CAT_CAT_PARAMETROS";
        public const String Campo_Decimales_Redondeo = "DECIMALES_REDONDEO";
        public const String Campo_Incremento_Valor = "INCREMENTO_VALOR";
        public const String Campo_Columnas_Calc_Construccion = "COLUMNAS_CALCU_CONSTRUCCION";
        public const String Campo_Renglones_Calc_Construccion = "RENGLONES_CALCU_CONSTRUCCION";
        public const String Campo_Factor_Ef = "FACTOR_EF";
        public const String Campo_Firmante = "FIRMANTE";
        public const String Campo_Puesto = "PUESTO";
        public const String Campo_Firmante_2 = "FIRMANTE_2";
        public const String Campo_Puesto_2 = "PUESTO_2";
        public const String Campo_Dias_Vigencia = "DIAS_VIGENCIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Correo_Autorizacion = "CORREO_AUTORIZACION";
        public const String Campo_Correo_General = "CORREO_GENERAL";
		public const String Campo_Fundamentacion_Legal = "FUNDAMENTACION_LEGAL";
		
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Claves_Catastrales
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_CLAVES_CATASTRALES
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 10/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************


    public class Cat_Cat_Claves_Catastrales
    {
        public const String Tabla_Cat_Cat_Claves_Catastrales = "CAT_CAT_CLAVES_CATASTRALES";
        public const String Campo_Claves_Catastrales_ID = "CLAVES_CATASTRALES_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Peritos_Externos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_PERITOS_EXTERNOS
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 12/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Peritos_Externos
    {
        public const String Tabla_Cat_Cat_Peritos_Externos = "CAT_CAT_PERITOS_EXTERNOS";
        public const String Campo_Perito_Externo_Id = "PERITO_EXTERNO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Apellido_Paterno = "APELLIDO_PATERNO";
        public const String Campo_Apellido_Materno = "APELLIDO_MATERNO";
        public const String Campo_Calle = "CALLE";
        public const String Campo_Colonia = "COLONIA";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Ciudad = "CIUDAD";
        public const String Campo_Telefono = "TELEFONO";
        public const String Campo_Celular = "CELULAR";
        public const String Campo_Usuario = "USUARIO";
        public const String Campo_Password = "PASSWORD";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Informacion = "INFORMACION";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Solicitud_Id = "SOLICITUD_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Fecha_Aceptacion = "FECHA_ACEPTACION";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Temp_Peritos_Externos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_TEMP_PERITOS_EXTERNOS
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 12/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Temp_Peritos_Externos
    {
        public const String Tabla_Cat_Cat_Temp_Peritos_Externos = "CAT_CAT_TEMP_PERITOS_EXTERNOS";
        public const String Campo_Temp_Perito_Externo_Id = "TEMP_PERITO_EXTERNO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Apellido_Paterno = "APELLIDO_PATERNO";
        public const String Campo_Apellido_Materno = "APELLIDO_MATERNO";
        public const String Campo_Calle = "CALLE";
        public const String Campo_Colonia = "COLONIA";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Ciudad = "CIUDAD";
        public const String Campo_Telefono = "TELEFONO";
        public const String Campo_Celular = "CELULAR";
        public const String Campo_E_Mail = "E_MAIL";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Informacion = "INFORMACION";
        public const String Campo_Solicitud_id = "SOLICITUD_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Peritos_Internos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_PERITOS_INTERNOS
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 14/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Peritos_Internos
    {
        public const String Tabla_Cat_Cat_Peritos_Internos = "CAT_CAT_PERITOS_INTERNOS";
        public const String Campo_Perito_Interno_Id = "PERITO_INTERNO_ID";
        public const String Campo_Empleado_Id = "EMPLEADO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Factores_Cobro_Avaluos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_PERITOS_INTERNOS
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 14/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Factores_Cobro_Avaluos
    {
        public const String Tabla_Cat_Cat_Factores_Cobro_Avaluos = "CAT_CAT_FACTORES_COBRO_AVALUOS";
        public const String Campo_Factor_Cobro_Id = "FACTOR_COBRO_ID";
        public const String Campo_Factor_Cobro_2 = "FACTOR_COBRO_2";
        public const String Campo_Base_Cobro = "BASE_COBRO";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Porcentaje_PE = "PORCENTAJE_PE";
        public const String Campo_Factor_Menor_1_Ha = "FACTOR_MENOR_1_HA";
        public const String Campo_Factor_Mayor_1_Ha = "FACTOR_MAYOR_1_HA";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Avaluo_Urbano
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_AVALUO_URBANO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Avaluo_Urbano
    { 
        public const String Tabla_Ope_Cat_Avaluo_Urbano = "OPE_CAT_AVALUO_URBANO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Motivo_Avaluo_Id = "MOTIVO_AVALUO_ID";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Solicitante = "SOLICTANTE";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Ruta_Fachada_Inmueble = "RUTA_FACHADA_INMUEBLE";
        public const String Campo_Valor_Total_Predio = "VALOR_TOTAL_PREDIO";
        public const String Campo_Inpr = "INPR";
        public const String Campo_Inpa = "INPA";
        public const String Campo_Valor_VR = "VALOR_VR";
        public const String Campo_Fecha_Autorizo = "FECHA_AUTORIZO";
        public const String Campo_Perito_Interno_Id = "PERITO_INTERNO_ID";
        public const String Campo_Perito_Externo_Id = "PERITO_EXTERNO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Cuenta_Predial = "CUENTA_PREDIAL";
        public const String Campo_Ubicacion = "UBICACION";
        public const String Campo_Localidad = "LOCALIDAD";
        public const String Campo_Propietario = "PROPIETARIO";
        public const String Campo_Municipio = "MUNICIPIO";
        public const String Campo_Colonia = "COLONIA";
        public const String Campo_Lote = "LOTE";
        public const String Campo_Manzana = "MANZANA";
        public const String Campo_Region = "REGION";
        public const String Campo_Domicilio_Notificacion = "DOMICILIO_NOTIFICACION";
        public const String Campo_Localidad_Notificacion = "LOCALIDAD_NOTIFICACION";
        public const String Campo_Colonia_Notificacion = "COLONIA_NOTIFICACION";
        public const String Campo_Municipio_Notificacion = "MUNICIPIO_NOTIFICACION";
        public const String Campo_Observaciones_Perito = "OBSERVACIONES_PERITO";
        public const String Campo_Coord_Norte = "COORD_NORTE";
        public const String Campo_Coord_Sur = "COORD_SUR";
        public const String Campo_Coord_Oriente = "COORD_ORIENTE";
        public const String Campo_Coord_Poniente = "COORD_PONIENTE";
        public const String Campo_Veces_Rechazo = "VECES_RECHAZO";
        public const String Campo_Fecha_Rechazo = "FECHA_RECHAZO";
        public const String Campo_Solicitud_Id = "SOLICITUD_ID";
        public const String Campo_Comentarios_Perito = "COMENTARIOS_PERITO";
        public const String Campo_Permitir_Revision = "PERMITIR_REVISION";
        public const String Campo_Estatus_Anterior = "ESTATUS_ANTERIOR";
        public const String Campo_Primer_Revision = "PRIMER_REVISION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Caract_Terreno_Au
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CARACT_TERRENO_AU
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Caract_Terreno_Au
    {
        public const String Tabla_Ope_Cat_Caract_Terreno_Au = "OPE_CAT_CARACT_TERRENO_AU";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Vias_Acceso = "VIAS_ACCESO";
        public const String Campo_Fotografia = "FOTOGRAFIA";
        public const String Campo_Dens_Const = "DENS_CONST";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Peritos_Externos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CONSTRUCCION_AU
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Construccion_Au
    {
        public const String Tabla_Ope_Cat_Construccion_Au = "OPE_CAT_CONSTRUCCION_AU";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Tipo_Construccion = "TIPO_CONSTRUCCION";
        public const String Campo_Calidad_Proyecto = "CALIDAD_PROYECTO";
        public const String Campo_Uso_Construccion = "USO_CONSTRUCCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Peritos_Externos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_ELEM_CONSTRUCCION_AU
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Elem_Construccion_Au
    {
        public const String Tabla_Ope_Cat_Elem_Construccion_Au = "OPE_CAT_ELEM_CONSTRUCCION_AU";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Elementos_Contruccion_Id = "ELEMENTOS_CONSTRUCCION_ID";
        public const String Campo_Elemento_Construccion_A = "ELEMENTO_CONSTRUCCION_A";
        public const String Campo_Elemento_Construccion_B = "ELEMENTO_CONSTRUCCION_B";
        public const String Campo_Elemento_Construccion_C = "ELEMENTO_CONSTRUCCION_C";
        public const String Campo_Elemento_Construccion_D = "ELEMENTO_CONSTRUCCION_D";
        public const String Campo_Elemento_Construccion_E = "ELEMENTO_CONSTRUCCION_E";
        public const String Campo_Elemento_Construccion_F = "ELEMENTO_CONSTRUCCION_F";
        public const String Campo_Elemento_Construccion_G = "ELEMENTO_CONSTRUCCION_G";
        public const String Campo_Elemento_Construccion_H = "ELEMENTO_CONSTRUCCION_H";
        public const String Campo_Elemento_Construccion_I = "ELEMENTO_CONSTRUCCION_I";
        public const String Campo_Elemento_Construccion_J = "ELEMENTO_CONSTRUCCION_J";
        public const String Campo_Elemento_Construccion_K = "ELEMENTO_CONSTRUCCION_K";
        public const String Campo_Elemento_Construccion_L = "ELEMENTO_CONSTRUCCION_L";
        public const String Campo_Elemento_Construccion_M = "ELEMENTO_CONSTRUCCION_M";
        public const String Campo_Elemento_Construccion_N = "ELEMENTO_CONSTRUCCION_N";
        public const String Campo_Elemento_Construccion_O = "ELEMENTO_CONSTRUCCION_O";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Valor_Const_Au
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_VALOR_CONST_AU
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Const_Au
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Const_Au = "OPE_CAT_CALC_VALOR_CONST_AU";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Referencia = "REFERENCIA";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Construccion_Id = "VALOR_CONSTRUCCION_ID";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Peritos_Externos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_VALOR_TERRENO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Terreno_Au
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Terreno_Au = "OPE_CAT_CALC_VALOR_TERRENO_AU";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Seccion = "SECCION";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Tramo_Id = "VALOR_TRAMO_ID";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Factor_EF = "FACTOR_EF";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Orden = "ORDEN";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Clasificacion_Zona_Au
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CLASIFICACION_ZONA_AU
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Clasificacion_Zona_Au
    {
        public const String Tabla_Ope_Cat_Clasificacion_Zona_Au = "OPE_CAT_CLASIFICACION_ZONA_AU";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Clasificacion_Zona_Id = "CLASIFICACION_ZONA_ID";
        public const String Campo_Valor_Clasificacion_Zona = "VALOR_CLASIFICACION_ZONA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Servicio_Zona_Au
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_SERVICIO_ZONA_AU
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Servicios_Zona_Au
    {
        public const String Tabla_Ope_Cat_Servicios_Zona_Au = "OPE_CAT_SERVICIO_ZONA_AU";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Servicio_Zona_Id = "SERVICIOS_ZONA_ID";
        public const String Campo_Valor_Servicios_Zona = "VALOR_SERVICIO_ZONA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Const_Dominante_Au
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CONST_DOMINANTE_AU
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Const_Dominante_Au
    {
        public const String Tabla_Ope_Cat_Const_Dominante_Au = "OPE_CAT_CONST_DOMINANTE_AU";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Construccion_Dominantea_Id = "CONSTRUCCION_DOMINANTE_ID";
        public const String Campo_Valor_Construccion_Dominante = "VALOR_CONST_DOMINANTE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Seguimiento_Avaluo_Au
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CARACT_TERRENO_AU
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Seguimiento_Avaluo_Au
    {
        public const String Tabla_Ope_Cat_Seguimiento_Avaluo_Au = "OPE_CAT_SEGUIMIENTO_AVALUO_UR";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Seguimiento = "NO_SEGUIMIENTO";
        public const String Campo_Motivo_Id = "MOTIVO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Seguimiento_Avaluo_Au
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CARACT_TERRENO_AU
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Documentos_Avaluo_Au
    {
        public const String Tabla_Ope_Cat_Doc_Avaluo_Ur_Au = "OPE_CAT_DOC_AVALUO_UR_AU";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Documento = "NO_DOCUMENTO";
        public const String Campo_Anio_Documento = "ANIO_DOCUMENTO";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Avaluo_Urbano_Av
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_AVALUO_URBANO_AV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Avaluo_Urbano_Av
    {
        public const String Tabla_Ope_Cat_Avaluo_Urbano_Av = "OPE_CAT_AVALUO_URBANO_AV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Motivo_Avaluo_Id = "MOTIVO_AVALUO_ID";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Lote = "LOTE";
        public const String Campo_Manzana = "MANZANA";
        public const String Campo_Region = "REGION";
        public const String Campo_Solicitante = "SOLICTANTE";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Ruta_Fachada_Inmueble = "RUTA_FACHADA_INMUEBLE";
        public const String Campo_Valor_Total_Predio = "VALOR_TOTAL_PREDIO";
        public const String Campo_Valor_VR = "VALOR_VR";
        public const String Campo_Fecha_Autorizo = "FECHA_AUTORIZO";
        public const String Campo_Perito_Interno_Id = "PERITO_INTERNO_ID";
        public const String Campo_Perito_Externo_Id = "PERITO_EXTERNO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Inpr = "INPR";
        public const String Campo_Inpa = "INPA";
        public const String Campo_Coord_Norte = "COORD_NORTE";
        public const String Campo_Coord_Sur = "COORD_SUR";
        public const String Campo_Coord_Oriente = "COORD_ORIENTE";
        public const String Campo_Coord_Poniente = "COORD_PONIENTE";
        public const String Campo_Veces_Rechazo = "VECES_RECHAZO";
        public const String Campo_Fecha_Rechazo = "FECHA_RECHAZO";
        public const String Campo_Observaciones_Rechazo = "OBSERVACIONES_RECHAZO";
        public const String Campo_No_Renglones = "NO_RENGLONES";
        public const String Campo_No_Asignacion = "NO_ASIGNACION";
        public const String Campo_Permitir_Revision = "PERMITIR_REVISION";
        public const String Campo_Comentarios_Revisor = "COMENTARIOS_REVISOR";
        public const String Campo_Tipo_Avaluo = "TIPO_AVALUO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Documentos_Avaluo_Av
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_DOC_AVALUO_UR_AV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Documentos_Avaluo_Av
    {
        public const String Tabla_Ope_Cat_Doc_Avaluo_Ur_Av = "OPE_CAT_DOC_AVALUO_UR_AV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Documento = "NO_DOCUMENTO";
        public const String Campo_Anio_Documento = "ANIO_DOCUMENTO";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Valor_Const_Av
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_VALOR_CONST_AV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Const_Av
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Const_Av = "OPE_CAT_CALC_VALOR_CONST_AV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Construccion_Id = "VALOR_CONSTRUCCION_ID";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Referencia = "REFERENCIA";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Valor_Terreno_Av
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_VALOR_TERRENO_AV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Terreno_Av
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Terreno_Av = "OPE_CAT_CALC_VALOR_TERRENO_AV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Seccion = "SECCION";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Tramo_Id = "VALOR_TRAMO_ID";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Factor_EF = "FACTOR_EF";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Orden = "ORDEN";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Caract_Terreno_Av
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CARACT_TERRENO_AV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Caract_Terreno_Av
    {
        public const String Tabla_Ope_Cat_Caract_Terreno_Av = "OPE_CAT_CARACT_TERRENO_AV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Vias_Acceso = "VIAS_ACCESO";
        public const String Campo_Fotografia = "FOTOGRAFIA";
        public const String Campo_Dens_Const = "DENS_CONST";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Const_Dominante_Av
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CONST_DOMINANTE_AV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Const_Dominante_Av
    {
        public const String Tabla_Ope_Cat_Const_Dominante_Av = "OPE_CAT_CONST_DOMINANTE_AV";
        public const String Campo_Construccion_Dominante_Id = "CONSTRUCCION_DOMINANTE_ID";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Valor_Const_Dominante = "VALOR_CONST_DOMINANTE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Construccion_Av
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CONSTRUCCION_AV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Construccion_Av
    {
        public const String Tabla_Ope_Cat_Construccion_Av = "OPE_CAT_CONSTRUCCION_AV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Tipo_Construccion = "TIPO_CONSTRUCCION";
        public const String Campo_Calidad_Proyecto = "CALIDAD_PROYECTO";
        public const String Campo_Uso_Construccion = "USO_CONSTRUCCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Elem_Construccion_Av
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_ELEM_CONSTRUCCION_AV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Elem_Construccion_Av
    {
        public const String Tabla_Ope_Cat_Elem_Construccion_Av = "OPE_CAT_ELEM_CONSTRUCCION_AV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Elemento_Construccion_A = "ELEMENTO_CONSTRUCCION_A";
        public const String Campo_Elemento_Construccion_B = "ELEMENTO_CONSTRUCCION_B";
        public const String Campo_Elemento_Construccion_C = "ELEMENTO_CONSTRUCCION_C";
        public const String Campo_Elemento_Construccion_D = "ELEMENTO_CONSTRUCCION_D";
        public const String Campo_Elemento_Construccion_E = "ELEMENTO_CONSTRUCCION_E";
        public const String Campo_Elemento_Construccion_F = "ELEMENTO_CONSTRUCCION_F";
        public const String Campo_Elemento_Construccion_G = "ELEMENTO_CONSTRUCCION_G";
        public const String Campo_Elemento_Construccion_H = "ELEMENTO_CONSTRUCCION_H";
        public const String Campo_Elemento_Construccion_I = "ELEMENTO_CONSTRUCCION_I";
        public const String Campo_Elemento_Construccion_J = "ELEMENTO_CONSTRUCCION_J";
        public const String Campo_Elemento_Construccion_K = "ELEMENTO_CONSTRUCCION_K";
        public const String Campo_Elemento_Construccion_L = "ELEMENTO_CONSTRUCCION_L";
        public const String Campo_Elemento_Construccion_M = "ELEMENTO_CONSTRUCCION_M";
        public const String Campo_Elemento_Construccion_N = "ELEMENTO_CONSTRUCCION_N";
        public const String Campo_Elemento_Construccion_O = "ELEMENTO_CONSTRUCCION_O";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Elementos_Construccion_Id = "ELEMENTOS_CONSTRUCCION_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Seguimiento_Avaluo_Av
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_SEGUIMIENTO_AVALUO_AV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Seguimiento_Avaluo_Av
    {
        public const String Tabla_Ope_Cat_Seguimiento_Avaluo_Av = "OPE_CAT_SEGUIMIENTO_AVALUO_AV";
        public const String Campo_No_Seguimiento = "NO_SEGUIMIENTO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Motivo_Id = "MOTIVO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Servicios_Zona_Av
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_SERVICIOS_ZONA_AV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Servicio_Zona_Av
    {
        public const String Tabla_Ope_Cat_Servicios_Zona_Av = "OPE_CAT_SERVICIO_ZONA_AV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Valor_Servicio_Zona = "VALOR_SERVICIO_ZONA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Servicios_Zona_Id = "SERVICIOS_ZONA_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Clasificacion_Zona_Av
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CLASIFICACION_ZONA_AV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Clasificacion_Zona_Av
    {
        public const String Tabla_Ope_Cat_Clasificacion_Zona_Av = "OPE_CAT_CLASIFICACION_ZONA_AV";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Clasificacion_Zona_Id = "CLASIFICACION_ZONA_ID";
        public const String Campo_Valor_Clasificacion_Zona = "VALOR_CLASIFICACION_ZONA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Avaluo_Urbano_In
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_AVALUO_URBANO_IN
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Avaluo_Urbano_In
    {
        public const String Tabla_Ope_Cat_Avaluo_Urbano_In = "OPE_CAT_AVALUO_URBANO_IN";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Motivo_Avaluo_Id = "MOTIVO_AVALUO_ID";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Solicitante = "SOLICTANTE";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Ruta_Fachada_Inmueble = "RUTA_FACHADA_INMUEBLE";
        public const String Campo_Valor_Total_Predio = "VALOR_TOTAL_PREDIO";
        public const String Campo_Valor_VR = "VALOR_VR";
        public const String Campo_Fecha_Autorizo = "FECHA_AUTORIZO";
        public const String Campo_Perito_Interno_Id = "PERITO_INTERNO_ID";
        public const String Campo_Perito_Externo_Id = "PERITO_EXTERNO_ID";
        public const String Campo_Region = "REGION";
        public const String Campo_Manzana = "MANZANA";
        public const String Campo_Lote = "LOTE";
        public const String Campo_No_Oficio = "NO_OFICIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Inpr = "INPR";
        public const String Campo_Inpa = "INPA";
        public const String Campo_No_Avaluo_Av = "NO_AVALUO_AV";
        public const String Campo_Anio_Avaluo_Av = "ANIO_AVALUO_AV";
        public const String Campo_Solicitud_Id= "SOLICITUD_ID"; 
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Seguimiento_Avaluo_In
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CARACT_TERRENO_IN
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Documentos_Avaluo_In
    {
        public const String Tabla_Ope_Cat_Doc_Avaluo_Ur_In = "OPE_CAT_DOC_AVALUO_UR_IN";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Documento = "NO_DOCUMENTO";
        public const String Campo_Anio_Documento = "ANIO_DOCUMENTO";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Valor_Const_In
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_VALOR_CONST_IN
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Const_In
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Const_In = "OPE_CAT_CALC_VALOR_CONST_IN";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Construccion_Id = "VALOR_CONSTRUCCION_ID";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Referencia = "REFERENCIA";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Valor_Terreno_iN
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_VALOR_TERRENO_IN
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Terreno_In
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Terreno_In = "OPE_CAT_CALC_VALOR_TERRENO_IN";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Seccion = "SECCION";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Tramo_Id = "VALOR_TRAMO_ID";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Factor_EF = "FACTOR_EF";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Orden = "ORDEN";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Caract_Terreno_In
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CARACT_TERRENO_IN
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Caract_Terreno_In
    {
        public const String Tabla_Ope_Cat_Caract_Terreno_In = "OPE_CAT_CARACT_TERRENO_IN";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Vias_Acceso = "VIAS_ACCESO";
        public const String Campo_Fotografia = "FOTOGRAFIA";
        public const String Campo_Dens_Const = "DENS_CONST";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Const_Dominante_In
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CONST_DOMINANTE_IN
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Const_Dominante_In
    {
        public const String Tabla_Ope_Cat_Const_Dominante_In = "OPE_CAT_CONST_DOMINANTE_IN";
        public const String Campo_Construccion_Dominante_Id = "CONSTRUCCION_DOMINANTE_ID";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Valor_Const_Dominante = "VALOR_CONST_DOMINANTE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Construccion_In
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CONSTRUCCION_IN
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Construccion_In
    {
        public const String Tabla_Ope_Cat_Construccion_In = "OPE_CAT_CONSTRUCCION_IN";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Tipo_Construccion = "TIPO_CONSTRUCCION";
        public const String Campo_Calidad_Proyecto = "CALIDAD_PROYECTO";
        public const String Campo_Uso_Construccion = "USO_CONSTRUCCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Elem_Construccion_In
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_ELEM_CONSTRUCCION_IN
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Elem_Construccion_In
    {
        public const String Tabla_Ope_Cat_Elem_Construccion_In = "OPE_CAT_ELEM_CONSTRUCCION_IN";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Elemento_Construccion_A = "ELEMENTO_CONSTRUCCION_A";
        public const String Campo_Elemento_Construccion_B = "ELEMENTO_CONSTRUCCION_B";
        public const String Campo_Elemento_Construccion_C = "ELEMENTO_CONSTRUCCION_C";
        public const String Campo_Elemento_Construccion_D = "ELEMENTO_CONSTRUCCION_D";
        public const String Campo_Elemento_Construccion_E = "ELEMENTO_CONSTRUCCION_E";
        public const String Campo_Elemento_Construccion_F = "ELEMENTO_CONSTRUCCION_F";
        public const String Campo_Elemento_Construccion_G = "ELEMENTO_CONSTRUCCION_G";
        public const String Campo_Elemento_Construccion_H = "ELEMENTO_CONSTRUCCION_H";
        public const String Campo_Elemento_Construccion_I = "ELEMENTO_CONSTRUCCION_I";
        public const String Campo_Elemento_Construccion_J = "ELEMENTO_CONSTRUCCION_J";
        public const String Campo_Elemento_Construccion_K = "ELEMENTO_CONSTRUCCION_K";
        public const String Campo_Elemento_Construccion_L = "ELEMENTO_CONSTRUCCION_L";
        public const String Campo_Elemento_Construccion_M = "ELEMENTO_CONSTRUCCION_M";
        public const String Campo_Elemento_Construccion_N = "ELEMENTO_CONSTRUCCION_N";
        public const String Campo_Elemento_Construccion_O = "ELEMENTO_CONSTRUCCION_O";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Elementos_Construccion_Id = "ELEMENTOS_CONSTRUCCION_ID";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Seguimiento_Avaluo_In
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_SEGUIMIENTO_AVALUO_IN
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Seguimiento_Avaluo_In
    {
        public const String Tabla_Ope_Cat_Seguimiento_Avaluo_In = "OPE_CAT_SEGUIMIENTO_AVALUO_IN";
        public const String Campo_No_Seguimiento = "NO_SEGUIMIENTO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Motivo_Id = "MOTIVO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Servicio_Zona_In
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_SERVICIO_ZONA_IN
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Servicio_Zona_In
    {
        public const String Tabla_Ope_Cat_Servicios_Zona_In = "OPE_CAT_SERVICIO_ZONA_IN";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Valor_Servicio_Zona = "VALOR_SERVICIO_ZONA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Servicios_Zona_Id = "SERVICIOS_ZONA_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Clasificacion_Zona_In
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CLASIFICACION_ZONA_IN
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Clasificacion_Zona_In
    {
        public const String Tabla_Ope_Cat_Clasificacion_Zona_In = "OPE_CAT_CLASIFICACION_ZONA_IN";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Clasificacion_Zona_Id = "CLASIFICACION_ZONA_ID";
        public const String Campo_Valor_Clasificacion_Zona = "VALOR_CLASIFICACION_ZONA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Avaluo_Urbano_Re
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_AVALUO_URBANO_RE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Avaluo_Urbano_Re
    {
        public const String Tabla_Ope_Cat_Avaluo_Urbano_Re = "OPE_CAT_AVALUO_URBANO_RE";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Motivo_Avaluo_Id = "MOTIVO_AVALUO_ID";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Solicitante = "SOLICTANTE";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Ruta_Fachada_Inmueble = "RUTA_FACHADA_INMUEBLE";
        public const String Campo_Valor_Total_Predio = "VALOR_TOTAL_PREDIO";
        public const String Campo_Valor_VR = "VALOR_VR";
        public const String Campo_Fecha_Autorizo = "FECHA_AUTORIZO";
        public const String Campo_Perito_Interno_Id = "PERITO_INTERNO_ID";
        public const String Campo_Perito_Externo_Id = "PERITO_EXTERNO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Region = "REGION";
        public const String Campo_Manzana = "MANZANA";
        public const String Campo_Lote = "LOTE";
        public const String Campo_No_Oficio = "NO_OFICIO";
        public const String Campo_Solicitud_Id = "SOLICITUD_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Inpr = "INPR";
        public const String Campo_Inpa = "INPA";
        public const String Campo_Comentarios_Perito = "COMENTARIOS_PERITO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Seguimiento_Avaluo_Re
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CARACT_TERRENO_RE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Documentos_Avaluo_Re
    {
        public const String Tabla_Ope_Cat_Doc_Avaluo_Ur_Re = "OPE_CAT_DOC_AVALUO_UR_RE";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Documento = "NO_DOCUMENTO";
        public const String Campo_Anio_Documento = "ANIO_DOCUMENTO";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Valor_Const_Re
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_VALOR_CONST_RE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Const_Re
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Const_Re = "OPE_CAT_CALC_VALOR_CONST_RE";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Construccion_Id = "VALOR_CONSTRUCCION_ID";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Referencia = "REFERENCIA";
    }

    public class Ope_Cat_Digitalizacion_Avaluos
    {
        public const String Tabla_Ope_Cat_Digitalizacion_Avaluos = "OPE_CAT_DIGITALIZACION_AVALUOS";
        public const String Campo_Digitalizacion_Avaluo_Id = "NO_DIGIT_DOC_AVALUO";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Valor_Terreno_Re
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_VALOR_TERRENO_RE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Terreno_Re
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Terreno_Re = "OPE_CAT_CALC_VALOR_TERRENO_RE";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Seccion = "SECCION";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Tramo_Id = "VALOR_TRAMO_ID";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Factor_EF = "FACTOR_EF";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Orden = "ORDEN";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Caract_Terreno_Re
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CARACT_TERRENO_RE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Caract_Terreno_Re
    {
        public const String Tabla_Ope_Cat_Caract_Terreno_Re = "OPE_CAT_CARACT_TERRENO_RE";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Vias_Acceso = "VIAS_ACCESO";
        public const String Campo_Fotografia = "FOTOGRAFIA";
        public const String Campo_Dens_Const = "DENS_CONST";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Const_Dominante_Re
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CONST_DOMINANTE_RE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Const_Dominante_Re
    {
        public const String Tabla_Ope_Cat_Const_Dominante_Re = "OPE_CAT_CONST_DOMINANTE_RE";
        public const String Campo_Construccion_Dominante_Id = "CONSTRUCCION_DOMINANTE_ID";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Valor_Const_Dominante = "VALOR_CONST_DOMINANTE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Construccion_Re
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CONSTRUCCION_RE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Construccion_Re
    {
        public const String Tabla_Ope_Cat_Construccion_Re = "OPE_CAT_CONSTRUCCION_RE";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Tipo_Construccion = "TIPO_CONSTRUCCION";
        public const String Campo_Calidad_Proyecto = "CALIDAD_PROYECTO";
        public const String Campo_Uso_Construccion = "USO_CONSTRUCCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Elem_Construccion_Re
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_ELEM_CONSTRUCCION_RE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Elem_Construccion_Re
    {
        public const String Tabla_Ope_Cat_Elem_Construccion_Re = "OPE_CAT_ELEM_CONSTRUCCION_RE";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Elemento_Construccion_A = "ELEMENTO_CONSTRUCCION_A";
        public const String Campo_Elemento_Construccion_B = "ELEMENTO_CONSTRUCCION_B";
        public const String Campo_Elemento_Construccion_C = "ELEMENTO_CONSTRUCCION_C";
        public const String Campo_Elemento_Construccion_D = "ELEMENTO_CONSTRUCCION_D";
        public const String Campo_Elemento_Construccion_E = "ELEMENTO_CONSTRUCCION_E";
        public const String Campo_Elemento_Construccion_F = "ELEMENTO_CONSTRUCCION_F";
        public const String Campo_Elemento_Construccion_G = "ELEMENTO_CONSTRUCCION_G";
        public const String Campo_Elemento_Construccion_H = "ELEMENTO_CONSTRUCCION_H";
        public const String Campo_Elemento_Construccion_I = "ELEMENTO_CONSTRUCCION_I";
        public const String Campo_Elemento_Construccion_J = "ELEMENTO_CONSTRUCCION_J";
        public const String Campo_Elemento_Construccion_K = "ELEMENTO_CONSTRUCCION_K";
        public const String Campo_Elemento_Construccion_L = "ELEMENTO_CONSTRUCCION_L";
        public const String Campo_Elemento_Construccion_M = "ELEMENTO_CONSTRUCCION_M";
        public const String Campo_Elemento_Construccion_N = "ELEMENTO_CONSTRUCCION_N";
        public const String Campo_Elemento_Construccion_O = "ELEMENTO_CONSTRUCCION_O";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Elementos_Construccion_Id = "ELEMENTOS_CONSTRUCCION_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Seguimiento_Avaluo_Re
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_SEGUIMIENTO_AVALUO_RE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Seguimiento_Avaluo_Re
    {
        public const String Tabla_Ope_Cat_Seguimiento_Avaluo_Re = "OPE_CAT_SEGUIMIENTO_AVALUO_RE";
        public const String Campo_No_Seguimiento = "NO_SEGUIMIENTO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Motivo_Id = "MOTIVO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Servicio_Zona_Re
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_SERVICIO_ZONA_RE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Servicio_Zona_Re
    {
        public const String Tabla_Ope_Cat_Servicio_Zona_Re = "OPE_CAT_SERVICIO_ZONA_RE";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Valor_Servicio_Zona = "VALOR_SERVICIO_ZONA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Servicios_Zona_Id = "SERVICIOS_ZONA_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Clasificacion_Zona_Re
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CLASIFICACION_ZONA_RE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Clasificacion_Zona_Re
    {
        public const String Tabla_Ope_Cat_Clasificacion_Zona_Re = "OPE_CAT_CLASIFICACION_ZONA_RE";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Clasificacion_Zona_Id = "CLASIFICACION_ZONA_ID";
        public const String Campo_Valor_Clasificacion_Zona = "VALOR_CLASIFICACION_ZONA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Doc_Peritos_Vigentes
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_DOC_PERITOS_VIGENTES
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Doc_Peritos_Vigentes
    {
        public const String Tabla_Ope_Cat_Doc_Peritos_Vigentes = "OPE_CAT_DOC_PERITOS_VIGENTES";
        public const String Campo_No_Documento = "NO_DOCUMENTO";
        public const String Campo_Perito_Externo_Id = "PERITO_EXTERNO_ID";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Doc_Perito_Externo
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_DOC_PERITO_EXTERNO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Doc_Perito_Externo
    {
        public const String Tabla_Ope_Cat_Doc_Perito_Externo = "OPE_CAT_DOC_PERITO_EXTERNO";
        public const String Campo_No_Documento = "NO_DOCUMENTO";
        public const String Campo_Temp_Perito_Externo_Id = "TEMP_PERITO_EXTERNO_ID";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Servicios_Zona
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_SERVICIOS_ZONA
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Servicios_Zona
    {
        public const String Tabla_Cat_Cat_Servicios_Zona = "CAT_CAT_SERVICIOS_ZONA";
        public const String Campo_Servicio_Zona_Id = "SERVICIOS_ZONA_ID";
        public const String Campo_Servicio_Zona = "SERVICIOS_ZONA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Clasificacion_Zona
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_CLASIFICACION_ZONA
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Clasificacion_Zona
    {
        public const String Tabla_Cat_Cat_Clasificacion_Zona = "CAT_CAT_CLASIFICACION_ZONA";
        public const String Campo_Clasificacion_Zona_Id = "CLASIFICACION_ZONA_ID";
        public const String Campo_Clasificacion_Zona = "CLASIFICACION_ZONA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Construccion_Dominante
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_CONSTRUCCION_DOMINANTE
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Construccion_Dominante
    {
        public const String Tabla_Cat_Cat_Construccion_Dominante = "CAT_CAT_CONSTRUCCION_DOMINANTE";
        public const String Campo_Construccion_Dominante_Id = "CONSTRUCCION_DOMINANTE_ID";
        public const String Campo_Construccion_Dominante = "CONSTRUCCION_DOMINANTE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Elementos_Construccion
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_ELEMENTOS_CONSTRUCCION
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Elementos_Construccion
    {
        public const String Tabla_Cat_Cat_Elementos_Construccion = "CAT_CAT_ELEMENTOS_CONSTRUCCION";
        public const String Campo_Elemento_Construccion_Id = "ELEMENTOS_CONSTRUCCION_ID";
        public const String Campo_Elemento_Construccion = "ELEMENTOS_CONSTRUCCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    public class Ope_Cat_Recepcion_Oficios
    {
        public const String Tabla_Ope_Cat_Recepcion_Oficios = "OPE_CAT_RECEPCION_OFICIOS";
        public const String Campo_No_Oficio = "NO_OFICIO";
        public const String Campo_No_Oficio_Recepcion = "NO_OFICIO_RECEPCION";
        public const String Campo_Fecha_Recepcion = "FECHA_RECEPCION";
        public const String Campo_Hora_Recepcion = "HORA_RECEPCION";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_No_Oficio_Respuesta = "NO_OFICIO_RESPUESTA";
        public const String Campo_Fecha_Respuesta = "FECHA_RESPUESTA";
        public const String Campo_Hora_Respuesta = "HORA_RESPUESTA";
        public const String Campo_Dependencia = "DEPENDENCIA";
        public const String Campo_Dep_Catastro = "DEP_CATASTRO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Mem_Descript
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_ELEMENTOS_CONSTRUCCION
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Mem_Descript
    {
        public const String Tabla_Ope_Cat_Mem_Descript = "OPE_CAT_MEM_DESCRIPT";
        public const String Campo_No_Mem_Descript = "NO_MEM_DESCRIPT";
        public const String Campo_Cantidad_Mem_Descript = "CANTIDAD_MEM_DESCRIPT";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Solicitante = "SOLICITANTE";
        public const String Campo_Fraccionamiento = "FRACCIONAMIENTO";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Ubicacion = "UBICACION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Tipo_Horientacion = "TIPO_HORIENTACION";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Perito_Externo_ID = "PERITO_EXTERNO_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Doc_Mem_Descript
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_ELEMENTOS_CONSTRUCCION
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Doc_Mem_Descript
    {
        public const String Tabla_Ope_Cat_Doc_Mem_Descript = "OPE_CAT_DOC_MEM_DESCRIPT";
        public const String Campo_No_Documento = "NO_DOCUMENTO";
        public const String Campo_Regimen_Condominio_Id = "REGIMEN_CONDOMINIO_ID";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_No_Mem_Descript = "NO_MEM_DESCRIPT";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Anio_Documento = "ANIO_DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Claves_Catastrales
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CLAVES_CATASTRALES
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 08/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Claves_Catastrales
    {
        public const String Tabla_Ope_Cat_Claves_Catastrales = "OPE_CAT_CLAVES_CATASTRALES";
        public const String Campo_No_Claves_Catastrales = "NO_CLAVES_CATASTRALES";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Cantidad_Claves_Catastrales = "CANTIDAD_CLAVES_CATASTRALES";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Ruta_Documento = "DOCUMENTO";
        public const String Campo_Correo = "CORREO";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Solicitante = "SOLICITANTE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Doc_Clave_Catastral
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_DOC_CLAVE_CATASTRAL
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 08/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Doc_Clave_Catastral
    {
        public const String Tabla_Ope_Cat_Doc_Clave_Catastral = "OPE_CAT_DOC_CLAVE_CATASTRAL";
        public const String Campo_No_Documento = "NO_DOCUMENTO";
        public const String Campo_Claves_Catastrales_id = "CLAVES_CATASTRALES_ID";
        public const String Campo_No_Claves_Catastrales = "NO_CLAVES_CATASTRALES";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Anio_Documento = "ANIO_DOCUMENTO";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Avaluo_Rustico
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_AVALUO_RUSTICO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Avaluo_Rustico
    {
        public const String Tabla_Ope_Cat_Avaluo_Rustico = "OPE_CAT_AVALUO_RUSTICO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Motivo_Avaluo_Id = "MOTIVO_AVALUO_ID";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Solicitante = "SOLICTANTE";
        public const String Campo_Propietario = "PROPIETARIO";
        public const String Campo_Clave_Catastral = "CLAVE_CATASTRAL";
        public const String Campo_Domicilio_Notificacion = "DOMICILIO_NOTIFICACION";
        public const String Campo_Municipio_Notificacion = "MUNICIPIO_NOTIFICACION";
        public const String Campo_Ubicacion = "UBICACION";
        public const String Campo_Localidad_Municipio = "LOCALIDAD_MUNICIPIO";
        public const String Campo_Nombre_Predio = "NOMBRE_PREDIO";
        public const String Campo_Coord_X_Grados = "COORD_X_GRADOS";
        public const String Campo_Coord_X_Minutos = "COORD_X_MINUTOS";
        public const String Campo_Coord_X_Segundos = "COORD_X_SEGUNDOS";
        public const String Campo_Orientacion_X = "ORIENTACION_X";
        public const String Campo_Coord_Y_Grados = "COORD_Y_GRADOS";
        public const String Campo_Coord_Y_Minutos = "COORD_Y_MINUTOS";
        public const String Campo_Coord_Y_Segundos = "COORD_Y_SEGUNDOS";
        public const String Campo_Orientacion_Y = "ORIENTACION_Y";
        public const String Campo_Valor_Total_Predio = "VALOR_TOTAL_PREDIO";
        public const String Campo_Coord_Norte = "COORD_NORTE";
        public const String Campo_Coord_Sur = "COORD_SUR";
        public const String Campo_Coord_Oriente = "COORD_ORIENTE";
        public const String Campo_Coord_Poniente = "COORD_PONIENTE";
        public const String Campo_Veces_Rechazo = "VECES_RECHAZO";
        public const String Campo_Fecha_Rechazo = "FECHA_RECHAZO";
        public const String Campo_Fecha_Autorizo = "FECHA_AUTORIZO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Observaciones_Perito = "OBSERVACIONES_PERITO";
        public const String Campo_Perito_Interno_Id = "PERITO_INTERNO_ID";
        public const String Campo_Perito_Externo_Id = "PERITO_EXTERNO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Coordenadas_UTM = "COORDENADAS_UTM";
        public const String Campo_Coordenadas_UTM_Y = "COORDENADAS_Y_UTM";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Uso = "USO";
        public const String Campo_Permitir_Revision = "PERMITIR_REVISION";
        public const String Campo_Solicitud_Id = "SOLICITUD_ID";
        public const String Campo_Comentarios_Perito = "COMENTARIOS_PERITO";
        public const String Campo_Estatus_Anterior = "ESTATUS_ANTERIOR";
        public const String Campo_Primer_Revision = "PRIMER_REVISION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Colindancias_Ara
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_COLINDANCIAS_ARA
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Colindancias_Aua
    {
        public const String Tabla_Ope_Cat_Colindancias_Aua = "OPE_CAT_COLINDANCIAS_AUA";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_No_Colindancia = "NO_COLINDANCIA";
        public const String Campo_Medida_Colindancia = "MEDIDA_COLINDANCIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Colindancias_Ara
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_COLINDANCIAS_ARA
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Colindancias_Ara
    {
        public const String Tabla_Ope_Cat_Colindancias_Ara = "OPE_CAT_COLINDANCIAS_ARA";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_No_Colindancia = "NO_COLINDANCIA";
        public const String Campo_Medida_Colindancia = "MEDIDA_COLINDANCIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Documentos_Ara
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_DOCUMENTOS_ARA
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Documentos_Ara
    {
        public const String Tabla_Ope_Cat_Documentos_Ara = "OPE_CAT_DOCUMENTOS_ARA";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Documento = "NO_DOCUMENTO";
        public const String Campo_Anio_Documento = "ANIO_DOCUMENTO";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Terreno_Ara
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_TERRENO_ARA
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Terreno_Ara
    {
        public const String Tabla_Ope_Cat_Calc_Terreno_Ara = "OPE_CAT_CALC_TERRENO_ARA";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Tipo_Constru_Rustico_Id = "TIPO_CONSTRU_RUSTICO_ID";
        public const String Campo_Valor_Constru_Rustico_Id = "VALOR_CONSTRU_RUSTICO_ID";
        public const String Campo_Superficie = "SUPERFICIE";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Valor_Const_Ara
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_TERRENO_ARA
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Const_Ara
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Const_Ara = "OPE_CAT_CALC_VALOR_CONST_ARA";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Croquis = "CROQUIS";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Construccion_Id = "VALOR_CONSTRUCCION_ID";
        public const String Campo_Edad_Constru = "EDAD_CONSTRU";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Uso_Constru = "USO_CONTRU";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Caracteristicas_Ara
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_TERRENO_ARA
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Caracteristicas_Ara
    {
        public const String Tabla_Ope_Cat_Caracteristicas_Ara = "OPE_CAT_CARACTERISTICAS_ARA";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Desc_Constru_Rustico_Id = "DESC_CONSTRU_RUSTICO_ID";
        public const String Campo_Descripcion_Rustico_Id = "DESCRIPCION_RUSTICO_ID";
        public const String Campo_Valor_Indicador_A = "VALOR_INDICADOR_A";
        public const String Campo_Valor_Indicador_B = "VALOR_INDICADOR_B";
        public const String Campo_Valor_Indicador_C = "VALOR_INDICADOR_C";
        public const String Campo_Valor_Indicador_D = "VALOR_INDICADOR_D";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Elem_Construccion_Ara
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_ELEM_CONSTRUCCION_AU
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Elem_Construccion_Ara
    {
        public const String Tabla_Ope_Cat_Elem_Construccion_Ara = "OPE_CAT_ELEM_CONSTRUCCION_ARA";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Elementos_Contruccion_Id = "ELEMENTOS_CONSTRUCCION_ID";
        public const String Campo_Elemento_Construccion_A = "ELEMENTO_CONSTRUCCION_A";
        public const String Campo_Elemento_Construccion_B = "ELEMENTO_CONSTRUCCION_B";
        public const String Campo_Elemento_Construccion_C = "ELEMENTO_CONSTRUCCION_C";
        public const String Campo_Elemento_Construccion_D = "ELEMENTO_CONSTRUCCION_D";
        public const String Campo_Elemento_Construccion_E = "ELEMENTO_CONSTRUCCION_E";
        public const String Campo_Elemento_Construccion_F = "ELEMENTO_CONSTRUCCION_F";
        public const String Campo_Elemento_Construccion_G = "ELEMENTO_CONSTRUCCION_G";
        public const String Campo_Elemento_Construccion_H = "ELEMENTO_CONSTRUCCION_H";
        public const String Campo_Elemento_Construccion_I = "ELEMENTO_CONSTRUCCION_I";
        public const String Campo_Elemento_Construccion_J = "ELEMENTO_CONSTRUCCION_J";
        public const String Campo_Elemento_Construccion_K = "ELEMENTO_CONSTRUCCION_K";
        public const String Campo_Elemento_Construccion_L = "ELEMENTO_CONSTRUCCION_L";
        public const String Campo_Elemento_Construccion_M = "ELEMENTO_CONSTRUCCION_M";
        public const String Campo_Elemento_Construccion_N = "ELEMENTO_CONSTRUCCION_N";
        public const String Campo_Elemento_Construccion_O = "ELEMENTO_CONSTRUCCION_O";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Elem_Construccion_Ari
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_ELEM_CONSTRUCCION_ARI
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Elem_Construccion_Ari
    {
        public const String Tabla_Ope_Cat_Elem_Construccion_Ari = "OPE_CAT_ELEM_CONSTRUCCION_ARI";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Elementos_Contruccion_Id = "ELEMENTOS_CONSTRUCCION_ID";
        public const String Campo_Elemento_Construccion_A = "ELEMENTO_CONSTRUCCION_A";
        public const String Campo_Elemento_Construccion_B = "ELEMENTO_CONSTRUCCION_B";
        public const String Campo_Elemento_Construccion_C = "ELEMENTO_CONSTRUCCION_C";
        public const String Campo_Elemento_Construccion_D = "ELEMENTO_CONSTRUCCION_D";
        public const String Campo_Elemento_Construccion_E = "ELEMENTO_CONSTRUCCION_E";
        public const String Campo_Elemento_Construccion_F = "ELEMENTO_CONSTRUCCION_F";
        public const String Campo_Elemento_Construccion_G = "ELEMENTO_CONSTRUCCION_G";
        public const String Campo_Elemento_Construccion_H = "ELEMENTO_CONSTRUCCION_H";
        public const String Campo_Elemento_Construccion_I = "ELEMENTO_CONSTRUCCION_I";
        public const String Campo_Elemento_Construccion_J = "ELEMENTO_CONSTRUCCION_J";
        public const String Campo_Elemento_Construccion_K = "ELEMENTO_CONSTRUCCION_K";
        public const String Campo_Elemento_Construccion_L = "ELEMENTO_CONSTRUCCION_L";
        public const String Campo_Elemento_Construccion_M = "ELEMENTO_CONSTRUCCION_M";
        public const String Campo_Elemento_Construccion_N = "ELEMENTO_CONSTRUCCION_N";
        public const String Campo_Elemento_Construccion_O = "ELEMENTO_CONSTRUCCION_O";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Seguimiento_Avaluo_Au
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CARACT_TERRENO_AU
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Seguimiento_Ara
    {
        public const String Tabla_Ope_Cat_Seguimiento_Ara = "OPE_CAT_SEGUIMIENTO_ARA";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Seguimiento = "NO_SEGUIMIENTO";
        public const String Campo_Motivo_Id = "MOTIVO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Avaluo_Rustico_R
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_AVALUO_RUSTICO_R
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Avaluo_Rustico_R
    {
        public const String Tabla_Ope_Cat_Avaluo_Rustico_R = "OPE_CAT_AVALUO_RUSTICO_R";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Motivo_Avaluo_Id = "MOTIVO_AVALUO_ID";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Solicitante = "SOLICTANTE";
        public const String Campo_Propietario = "PROPIETARIO";
        public const String Campo_Clave_Catastral = "CLAVE_CATASTRAL";
        public const String Campo_Domicilio_Notificacion = "DOMICILIO_NOTIFICACION";
        public const String Campo_Municipio_Notificacion = "MUNICIPIO_NOTIFICACION";
        public const String Campo_Ubicacion = "UBICACION";
        public const String Campo_Localidad_Municipio = "LOCALIDAD_MUNICIPIO";
        public const String Campo_Nombre_Predio = "NOMBRE_PREDIO";
        public const String Campo_Coord_X_Grados = "COORD_X_GRADOS";
        public const String Campo_Coord_X_Minutos = "COORD_X_MINUTOS";
        public const String Campo_Coord_X_Segundos = "COORD_X_SEGUNDOS";
        public const String Campo_Orientacion_X = "ORIENTACION_X";
        public const String Campo_Coord_Y_Grados = "COORD_Y_GRADOS";
        public const String Campo_Coord_Y_Minutos = "COORD_Y_MINUTOS";
        public const String Campo_Coord_Y_Segundos = "COORD_Y_SEGUNDOS";
        public const String Campo_Orientacion_Y = "ORIENTACION_Y";
        public const String Campo_Base_Gravable = "BASE_GRAVABLE";
        public const String Campo_Impuesto_Bimestral = "IMPUESTO_BIMESTRAL";
        public const String Campo_Valor_Total_Predio = "VALOR_TOTAL_PREDIO";
        public const String Campo_Coord_Norte = "COORD_NORTE";
        public const String Campo_Coord_Sur = "COORD_SUR";
        public const String Campo_Coord_Oriente = "COORD_ORIENTE";
        public const String Campo_Coord_Poniente = "COORD_PONIENTE";
        public const String Campo_Veces_Rechazo = "VECES_RECHAZO";
        public const String Campo_Fecha_Rechazo = "FECHA_RECHAZO";
        public const String Campo_Fecha_Autorizo = "FECHA_AUTORIZO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Observaciones_Perito = "OBSERVACIONES_PERITO";
        public const String Campo_Perito_Interno_Id = "PERITO_INTERNO_ID";
        public const String Campo_Perito_Externo_Id = "PERITO_EXTERNO_ID";
        public const String Campo_Solicitud_Id = "SOLICITUD_ID";
        public const String Campo_Coordenadas_UTM = "COORDENADAS_UTM";
        public const String Campo_Coordenadas_UTM_Y = "COORDENADAS_Y_UTM";
		public const String Campo_No_Oficio = "NO_OFICIO";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Uso_Constru = "USO_CONSTRU";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Elem_Construccion_Arr
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_ELEM_CONSTRUCCION_ARR
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Elem_Construccion_Arr
    {
        public const String Tabla_Ope_Cat_Elem_Construccion_Arr = "OPE_CAT_ELEM_CONSTRUCCION_ARR";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_Elementos_Contruccion_Id = "ELEMENTOS_CONSTRUCCION_ID";
        public const String Campo_Elemento_Construccion_A = "ELEMENTO_CONSTRUCCION_A";
        public const String Campo_Elemento_Construccion_B = "ELEMENTO_CONSTRUCCION_B";
        public const String Campo_Elemento_Construccion_C = "ELEMENTO_CONSTRUCCION_C";
        public const String Campo_Elemento_Construccion_D = "ELEMENTO_CONSTRUCCION_D";
        public const String Campo_Elemento_Construccion_E = "ELEMENTO_CONSTRUCCION_E";
        public const String Campo_Elemento_Construccion_F = "ELEMENTO_CONSTRUCCION_F";
        public const String Campo_Elemento_Construccion_G = "ELEMENTO_CONSTRUCCION_G";
        public const String Campo_Elemento_Construccion_H = "ELEMENTO_CONSTRUCCION_H";
        public const String Campo_Elemento_Construccion_I = "ELEMENTO_CONSTRUCCION_I";
        public const String Campo_Elemento_Construccion_J = "ELEMENTO_CONSTRUCCION_J";
        public const String Campo_Elemento_Construccion_K = "ELEMENTO_CONSTRUCCION_K";
        public const String Campo_Elemento_Construccion_L = "ELEMENTO_CONSTRUCCION_L";
        public const String Campo_Elemento_Construccion_M = "ELEMENTO_CONSTRUCCION_M";
        public const String Campo_Elemento_Construccion_N = "ELEMENTO_CONSTRUCCION_N";
        public const String Campo_Elemento_Construccion_O = "ELEMENTO_CONSTRUCCION_O";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }




    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Terreno_Arr
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_TERRENO_ARR
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Terreno_Arr
    {
        public const String Tabla_Ope_Cat_Calc_Terreno_Arr = "OPE_CAT_CALC_TERRENO_ARR";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Tipo_Constru_Rustico_Id = "TIPO_CONSTRU_RUSTICO_ID";
        public const String Campo_Valor_Constru_Rustico_Id = "VALOR_CONSTRU_RUSTICO_ID";
        public const String Campo_Superficie = "SUPERFICIE";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Valor_Const_Arr
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_TERRENO_ARR
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Const_Arr
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Const_Arr = "OPE_CAT_CALC_VALOR_CONST_ARR";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Croquis = "CROQUIS";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Construccion_Id = "VALOR_CONSTRUCCION_ID";
        public const String Campo_Edad_Constru = "EDAD_CONSTRU";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Uso_Constru = "USO_CONTRU";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Caracteristicas_Arr
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_TERRENO_ARR
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Caracteristicas_Arr
    {
        public const String Tabla_Ope_Cat_Caracteristicas_Arr = "OPE_CAT_CARACTERISTICAS_ARR";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Desc_Constru_Rustico_Id = "DESC_CONSTRU_RUSTICO_ID";
        public const String Campo_Descripcion_Rustico_Id = "DESCRIPCION_RUSTICO_ID";
        public const String Campo_Valor_Indicador_A = "VALOR_INDICADOR_A";
        public const String Campo_Valor_Indicador_B = "VALOR_INDICADOR_B";
        public const String Campo_Valor_Indicador_C = "VALOR_INDICADOR_C";
        public const String Campo_Valor_Indicador_D = "VALOR_INDICADOR_D";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Seguimiento_Avaluo_Ar
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CARACT_TERRENO_ARR
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Seguimiento_Arr
    {
        public const String Tabla_Ope_Cat_Seguimiento_Arr = "OPE_CAT_SEGUIMIENTO_ARR";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Seguimiento = "NO_SEGUIMIENTO";
        public const String Campo_Motivo_Id = "MOTIVO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }












    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Avaluo_Rustico_I
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_AVALUO_RUSTICO_I
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Avaluo_Rustico_I
    {
        public const String Tabla_Ope_Cat_Avaluo_Rustico_I = "OPE_CAT_AVALUO_RUSTICO_I";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Motivo_Avaluo_Id = "MOTIVO_AVALUO_ID";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Solicitante = "SOLICTANTE";
        public const String Campo_Propietario = "PROPIETARIO";
        public const String Campo_Clave_Catastral = "CLAVE_CATASTRAL";
        public const String Campo_Domicilio_Notificacion = "DOMICILIO_NOTIFICACION";
        public const String Campo_Municipio_Notificacion = "MUNICIPIO_NOTIFICACION";
        public const String Campo_Ubicacion = "UBICACION";
        public const String Campo_Localidad_Municipio = "LOCALIDAD_MUNICIPIO";
        public const String Campo_Nombre_Predio = "NOMBRE_PREDIO";
        public const String Campo_Coord_X_Grados = "COORD_X_GRADOS";
        public const String Campo_Coord_X_Minutos = "COORD_X_MINUTOS";
        public const String Campo_Coord_X_Segundos = "COORD_X_SEGUNDOS";
        public const String Campo_Orientacion_X = "ORIENTACION_X";
        public const String Campo_Coord_Y_Grados = "COORD_Y_GRADOS";
        public const String Campo_Coord_Y_Minutos = "COORD_Y_MINUTOS";
        public const String Campo_Coord_Y_Segundos = "COORD_Y_SEGUNDOS";
        public const String Campo_Orientacion_Y = "ORIENTACION_Y";
        public const String Campo_Base_Gravable = "BASE_GRAVABLE";
        public const String Campo_Impuesto_Bimestral = "IMPUESTO_BIMESTRAL";
        public const String Campo_Valor_Total_Predio = "VALOR_TOTAL_PREDIO";
        public const String Campo_Coord_Norte = "COORD_NORTE";
        public const String Campo_Coord_Sur = "COORD_SUR";
        public const String Campo_Coord_Oriente = "COORD_ORIENTE";
        public const String Campo_Coord_Poniente = "COORD_PONIENTE";
        public const String Campo_Veces_Rechazo = "VECES_RECHAZO";
        public const String Campo_Fecha_Rechazo = "FECHA_RECHAZO";
        public const String Campo_Fecha_Autorizo = "FECHA_AUTORIZO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Observaciones_Perito = "OBSERVACIONES_PERITO";
        public const String Campo_Perito_Interno_Id = "PERITO_INTERNO_ID";
        public const String Campo_Perito_Externo_Id = "PERITO_EXTERNO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Coordenadas_UTM = "COORDENADAS_UTM";
        public const String Campo_Coordenadas_UTM_Y = "COORDENADAS_Y_UTM";
        public const String Campo_Uso_Constru = "USO";
        public const String Campo_Solicitud_Id = "SOLICITUD_ID";
        public const String Campo_No_Oficio = "NO_OFICIO";
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Terreno_Ari
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_TERRENO_ARI
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Terreno_Ari
    {
        public const String Tabla_Ope_Cat_Calc_Terreno_Ari = "OPE_CAT_CALC_TERRENO_ARI";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Tipo_Constru_Rustico_Id = "TIPO_CONSTRU_RUSTICO_ID";
        public const String Campo_Valor_Constru_Rustico_Id = "VALOR_CONSTRU_RUSTICO_ID";
        public const String Campo_Superficie = "SUPERFICIE";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Documentos_Arr
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_DOCUMENTOS_ARR
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Documentos_Ari
    {
        public const String Tabla_Ope_Cat_Documentos_Ari = "OPE_CAT_DOCUMENTOS_ARI";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Documento = "NO_DOCUMENTO";
        public const String Campo_Anio_Documento = "ANIO_DOCUMENTO";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Calc_Valor_Const_Ari
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_TERRENO_ARI
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Calc_Valor_Const_Ari
    {
        public const String Tabla_Ope_Cat_Calc_Valor_Const_Ari = "OPE_CAT_CALC_VALOR_CONST_ARI";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Croquis = "CROQUIS";
        public const String Campo_Superficie_M2 = "SUPERFICIE_M2";
        public const String Campo_Valor_Construccion_Id = "VALOR_CONSTRUCCION_ID";
        public const String Campo_Edad_Constru = "EDAD_CONSTRU";
        public const String Campo_Factor = "FACTOR";
        public const String Campo_Valor_Parcial = "VALOR_PARCIAL";
        public const String Campo_Uso_Constru = "USO_CONTRU";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Caracteristicas_Ari
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CALC_TERRENO_ARI
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Caracteristicas_Ari
    {
        public const String Tabla_Ope_Cat_Caracteristicas_Ari = "OPE_CAT_CARACTERISTICAS_ARI";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Desc_Constru_Rustico_Id = "DESC_CONSTRU_RUSTICO_ID";
        public const String Campo_Descripcion_Rustico_Id = "DESCRIPCION_RUSTICO_ID";
        public const String Campo_Valor_Indicador_A = "VALOR_INDICADOR_A";
        public const String Campo_Valor_Indicador_B = "VALOR_INDICADOR_B";
        public const String Campo_Valor_Indicador_C = "VALOR_INDICADOR_C";
        public const String Campo_Valor_Indicador_D = "VALOR_INDICADOR_D";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Colindancias_Ari
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_COLINDANCIAS_ARI
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Colindancias_Ari
    {
        public const String Tabla_Ope_Cat_Colindancias_Ari = "OPE_CAT_COLINDANCIAS_ARI";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_No_Colindancia = "NO_COLINDANCIA";
        public const String Campo_Medida_Colindancia = "MEDIDA_COLINDANCIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Colindancias_Arr
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_COLINDANCIAS_ARR
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Colindancias_Arr
    {
        public const String Tabla_Ope_Cat_Colindancias_Arr = "OPE_CAT_COLINDANCIAS_ARR";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_No_Colindancia = "NO_COLINDANCIA";
        public const String Campo_Medida_Colindancia = "MEDIDA_COLINDANCIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Documentos_Arr
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_DOCUMENTOS_ARR
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Documentos_Arr
    {
        public const String Tabla_Ope_Cat_Documentos_Arr = "OPE_CAT_DOCUMENTOS_ARR";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Documento = "NO_DOCUMENTO";
        public const String Campo_Anio_Documento = "ANIO_DOCUMENTO";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Seguimiento_Avaluo_Ari
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CARACT_TERRENO_ARI
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Seguimiento_Ari
    {
        public const String Tabla_Ope_Cat_Seguimiento_Ari = "OPE_CAT_SEGUIMIENTO_ARI";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Seguimiento = "NO_SEGUIMIENTO";
        public const String Campo_Motivo_Id = "MOTIVO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Colindancias_Aur
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_COLINDANCIAS_AUR
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Colindancias_Aur
    {
        public const String Tabla_Ope_Cat_Colindancias_Aur = "OPE_CAT_COLINDANCIAS_AUR";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_No_Colindancia = "NO_COLINDANCIA";
        public const String Campo_Medida_Colindancia = "MEDIDA_COLINDANCIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Colindancias_Aui
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_COLINDANCIAS_AUI
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Colindancias_Aui
    {
        public const String Tabla_Ope_Cat_Colindancias_Aui = "OPE_CAT_COLINDANCIAS_AUI";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_No_Colindancia = "NO_COLINDANCIA";
        public const String Campo_Medida_Colindancia = "MEDIDA_COLINDANCIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Cat_Colindancias_Auv
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_COLINDANCIAS_AUV
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Cat_Colindancias_Auv
    {
        public const String Tabla_Ope_Cat_Colindancias_Auv = "OPE_CAT_COLINDANCIAS_AUV";
        public const String Campo_Anio_Avaluo = "ANIO_AVALUO";
        public const String Campo_No_Avaluo = "NO_AVALUO";
        public const String Campo_No_Colindancia = "NO_COLINDANCIA";
        public const String Campo_Medida_Colindancia = "MEDIDA_COLINDANCIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
	 ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cat_Cuotas_Perito
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CAT_CUOTAS_PERITO
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 17/Mayo/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cat_Cuotas_Perito
    {
        public const String Tabla_Cat_Cat_Cuotas_Perito = "CAT_CAT_CUOTAS_PERITO";

        public const String Campo_Anio = "ANIO";
        public const String Campo_Cuota_Perito_Id = "CUOTA_PERITO_ID";
        public const String Campo_Perito_Interno_Id = "PERITO_INTERNO_ID";
        public const String Campo_1_Entrega = "PRIMERA_ENTREGA";
        public const String Campo_2_Entrega = "SEGUNDA_ENTREGA";
        public const String Campo_3_Entrega = "TERCERA_ENTREGA";
        public const String Campo_4_Entrega = "CUARTA_ENTREGA";
        public const String Campo_5_Entrega = "QUINTA_ENTREGA";
        public const String Campo_6_Entrega = "SEXTA_ENTREGA";
        public const String Campo_7_Entrega = "SEPTIMA_ENTREGA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }



    #endregion



    ///**********************************************************************************************************************************
    ///                                                                GENERALES
    ///**********************************************************************************************************************************
    #region Generales
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Grupos_Dependencias
    /// DESCRIPCIÓN: Clase que contiene los datos de la tabla CAT_GRUPOS_DEPENDENCIAS
    /// PARÁMETROS :
    /// CREO       : Susana Trigueros Armenta
    /// FECHA_CREO : 01/Junio/2011
    /// MODIFICO   :
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Grupos_Dependencias
    {
        public const String Tabla_Cat_Grupos_Dependencias = "CAT_GRUPOS_DEPENDENCIAS";
        public const String Campo_Grupo_Dependencia_ID = "GRUPO_DEPENDENCIA_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Registro_Patronal = "REGISTRO_PATRONAL";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Apl_Verifica_Ingreso
    /// DESCRIPCIÓN: Clase que contiene los datos de la tabla Apl_Bitacora
    /// PARÁMETROS :
    /// CREO       : Gustavo Angeles Cruz
    /// FECHA_CREO : 20/Sep/2010
    /// MODIFICO   :
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Apl_Verifica_Ingreso
    {
        public const String Tabla_Apl_Verifica_Ingreso = "APL_VERIFICA_INGRESO";
        public const String Campo_Verifica_ID = "VERIFICA_ID";
        public const String Campo_Fecha_Primer_Ingreso = "FECHA";
    }//fin de Apl_Verifica_Ingreso

    /*******************************************************************************
  NOMBRE DE LA CLASE: Apl_Parametros
  DESCRIPCIÓN: Clase que contiene los datos de la Tabla Apl_Parametros
  PARÁMETROS :
  CREO       : Silvia Morales Portuhondo
  FECHA_CREO : 30/Septiembre/2010
  MODIFICO   :Susana Trigueros Armenta
  FECHA_MODIFICO:11/Octubre/2010
  CAUSA_MODIFICACIÓN: Estaba mal escritos algunos campos y se borraron 3 campos que ya no se ocuparan
     los cuales son: 
     * public const String Campo_Administrador_Sistema = "ADMINISTRADOR_SISTEMA";
     * public const String Campo_Administrador_Modulo = "ADMINISTRADOR_MODULO";
     * public const String Campo_Jefe_Dependencia = "JEFE_DEPENDENCIA";
     * public const String Campo_Jefe_Area = "JEFE_AREA";
     * public const String Campo_Empleado = "EMPLEADO";
 *******************************************************************************/
    public class Apl_Parametros
    {
        public const String Tabla_Apl_Parametros = "APL_PARAMETROS";
        public const String Campo_Correo_Saliente = "CORREO_SALIENTE";
        public const String Campo_Servidor_Correo = "SERVIDOR_CORREO";
        public const String Campo_Usuario_Correo = "USUARIO_CORREO";
        public const String Campo_Password_Correo = "PASSWORD_CORREO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }//fin de Apl_Parametros
    ///******************************************************************************
    /// NOMBRE DE LA CLASE: Apl_Bitacora
    /// DESCRIPCIÓN: Clase que contiene los datos de la tabla Apl_Bitacora
    /// PARÁMETROS :
    /// CREO       : Susana Trigueros Armenta
    /// FECHA_CREO : 27/Agosto/2010
    /// MODIFICO   :
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Apl_Bitacora
    {
        public const String Tabla_Apl_Bitacora = "APL_BITACORA";
        public const String Campo_Bitacora_ID = "BITACORA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Fecha_Hora = "FECHA_HORA";
        public const String Campo_Accion = "ACCION";
        public const String Campo_Recurso = "RECURSO";
        public const String Campo_Recurso_ID = "RECURSO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";

    }//fin de Apl_Bitacora 


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Ate_Localidades
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla Cat_Ate_Localidades
    /// PARÁMETROS :     
    /// CREO       : Silvia Morales Portuhondo
    /// FECHA_CREO : 23/Septiembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Ate_Localidades
    {
        public const String Tabla_Cat_Ate_Localidades = "CAT_ATE_LOCALIDADES";
        public const String Campo_LocalidadID = "LOCALIDAD_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_UsuarioCreo = "USUARIO_CREO";
        public const String Campo_FechaCreo = "FECHA_CREO";
        public const String Campo_UsuarioModifico = "USUARIO_MODIFICO";
        public const String Campo_FechaModifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Bitacora
    /// DESCRIPCIÓN: Clase que contiene el listado de los catalogos que tiene el proyecto de presidencia
    /// y las operaciones. 
    /// PARÁMETROS :
    /// CREO       : Susana Trigueros Armenta
    /// FECHA_CREO : 30/Agosto/2010
    /// MODIFICO   :
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Bitacora
    {
        public static String[] Array_Catalogos = { "Asuntos", "Areas", "Colonias", "Bitacora de Eventos", "Dependencias", "Tramites", };
        public static String[] Array_Operaciones = new String[] { "Peticiones", "Solicitudes" };
        public const String Accion_Alta = "Alta";
        public const String Accion_Modificar = "Modificar";
        public const String Accion_Consultar = "Consultar";
        public const String Accion_Imprimir = "Imprimir";
        public const String Accion_Eliminar = "Eliminar";
        public const String Accion_Reporte = "Reporte";
        public const String Accion_Estadistica = "Estadistica";
        public const String Accion_Acceso = "Acceso";
        public const String Accion_Baja = "Baja";

    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Roles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla APL_CAT_ROLES
    /// PARÁMETROS :
    /// CREO       : Susana Trigueros Armenta
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Apl_Cat_Roles
    {
        public const String Tabla_Apl_Cat_Roles = "APL_CAT_ROLES";
        public const String Campo_Rol_ID = "ROL_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Grupo_Roles_ID = "GRUPO_ROLES_ID";


    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Apl_Grupos_Roles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla APL_GRUPOS_ROLES
    /// PARÁMETROS :
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 06/Oct/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Apl_Grupos_Roles
    {
        public const String Tabla_Apl_Grupos_Roles = "APL_GRUPOS_ROLES";
        public const String Campo_Grupo_Roles_ID = "GRUPO_ROLES_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Apl_Cat_Accesos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla APL_CAT_ACCESOS
    /// PARÁMETROS :
    /// CREO       : Susana Trigueros Armenta
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Apl_Cat_Accesos
    {
        public const String Tabla_Apl_Cat_Accesos = "APL_CAT_ACCESOS";
        public const String Campo_Menu_ID = "MENU_ID";
        public const String Campo_Rol_ID = "ROL_ID";
        public const String Campo_Habilitado = "HABILITADO";
        public const String Campo_Alta = "ALTA";
        public const String Campo_Cambio = "CAMBIO";
        public const String Campo_Eliminar = "ELIMINAR";
        public const String Campo_Consultar = "CONSULTAR";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Apl_Cat_Menus
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla APL_CAT_MENUS
    /// PARÁMETROS :
    /// CREO       : Susana Trigueros Armenta
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Apl_Cat_Menus
    {
        public const String Tabla_Apl_Cat_Menus = "APL_CAT_MENUS";
        public const String Campo_Menu_ID = "MENU_ID";
        public const String Campo_Parent_ID = "PARENT_ID";
        public const String Campo_Menu_Descripcion = "MENU_DESCRIPCION";
        public const String Campo_URL_Link = "URL_LINK";
        public const String Campo_Orden = "ORDEN";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Clasificacion = "CLASIFICACION";
        public const String Campo_Pagina = "PAGINA";
        public const String Campo_Modulo_ID = "MODULO_ID";
    }
    #endregion

    ///**********************************************************************************************************************************
    ///                                                                PREDIAL
    ///**********************************************************************************************************************************

    #region Predial
    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Ope_Caj_Empleados_Folios
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAJ_EMPLEADOS_FOLIOS
    ///PARAMETROS : 
    ///CREO       : Yazmin Delgado Gómez
    ///FECHA_CREO : 16-Octubre-2011
    ///MODIFICO          : 
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Caj_Empleados_Folios
    {
        public const String Tabla_Ope_Caj_Empleados_Folios = "OPE_CAJ_EMPLEADOS_FOLIOS";
        public const String Campo_No_Folio = "NO_FOLIO";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Folio_Inicial = "FOLIO_INICIAL";
        public const String Campo_Folio_Final = "FOLIO_FINAL";
        public const String Campo_Ultimo_Folio_Utilizado = "ULTIMO_FOLIO_UTILIZADO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Ope_Pre_Parametros
    ///DESCRIPCIÓN: clase que contiene los campos de la tabla ope_pre_parametros
    ///PARAMETROS: 
    ///CREO: jesus toledo
    ///FECHA_CREO: 06/24/2011 11:14:25 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Parametros
    {
        public const String Tabla_Ope_Pre_Parametros = "OPE_PRE_PARAMETROS";
        public const String Campo_Recargas_Traslado = "RECARGAS_TRASLADO";
        public const String Campo_Constancia_No_Adeudo = "CONSTANCIA_NO_ADEUDO";
        public const String Campo_Respaldo_Descuento_Tras = "RESPALDO_DESCUENTO_TRAS";
        public const String Campo_Anio_Vigencia = "ANIO_VIGENCIA";
        public const String Campo_Porcentaje_Cobro_Honorarios = "PORCENTAJE_COBRO_HONORARIOS";
        public const String Campo_Tope_Salario = "TOPE_SALARIO_MINIMO";
        public const String Campo_Tolerancia_Pago_Superior = "TOLERANCIA_PAGO_SUPERIOR";
        public const String Campo_Tolerancia_Pago_Inferior = "TOLERANCIA_PAGO_INFERIOR";
        public const String Campo_Dias_Vencimiento = "DIAS_VENCIMIENTO";
        public const String Campo_Cajero_General_ID = "CAJERO_GENERAL_ID";
        public const String Campo_Clave_Ing_Ajuste_Tarifa_ID = "CLAVE_ING_AJUSTE_TARIFA_ID";
        public const String Campo_SubConcepto_Ajuste_Tarifa_ID = "SUBCONCEPTO_AJUSTE_TARIFA_ID";
        public const String Campo_Anio_Corriente = "ANIO_CORRIENTE";
        public const String Campo_Caja_Id_Pago_Internet = "CAJA_ID_PAGO_INTERNET";
        public const String Campo_Caja_Id_Pago_Pae = "CAJA_ID_PAGO_PAE";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Pre_Formas_Pago
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla Ope_Pre_Formas_Pago
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vazquez
    ///FECHA_CREO           : 12/Octubre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Pre_Formas_Pago
    {
        public const String Tabla_Ope_Pre_Formas_Pago = "OPE_PRE_FORMAS_PAGO";
        public const String Campo_No_Arqueo = "NO_ARQUEO";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Ope_Pre_Dias_Inhabiles
    ///DESCRIPCIÓN: clase que contiene los campos de la tabla Ope_Pre_Dias_Inhabiles
    ///PARAMETROS : 
    ///CREO       : Jesús Toledo
    ///FECHA_CREO : 24/Junio/2011 11:14:25 a.m.
    ///MODIFICO          : Yazmin Delgado Gómez 
    ///FECHA_MODIFICO    : 08-Octubre-2011 14:29
    ///CAUSA_MODIFICACIÓN: Porque falto agregar la fecha de aplicación de acuerdo al
    ///                    día inhabil
    ///*******************************************************************************
    public class Ope_Pre_Dias_Inhabiles
    {
        public const String Tabla_Ope_Pre_Dias_Inhabiles = "OPE_PRE_DIAS_INHABILES";
        public const String Campo_Dia_Inhabil_ID = "NO_DIA_INHABIL";
        public const String Campo_Dia_ID = "DIA_ID";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Fecha_Aplicacion = "FECHA_APLICACION";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Motivo = "MOTIVO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Ope_Pre_Diferencias
    ///DESCRIPCIÓN: clase que contiene los campos de la tabla Ope_Pre_Diferencias
    ///PARAMETROS: 
    ///CREO: jesus toledo
    ///FECHA_CREO: 10/Ago/2011 11:14:25 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Diferencias
    {
        public const String Tabla_Ope_Pre_Diferencias = "OPE_PRE_DIFERENCIAS";
        public const String Campo_No_Diferencia = "NO_DIFERENCIA";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_No_Orden_Variacion = "NO_ORDEN_VARIACION";
        public const String Campo_Total_Recargos = "TOTAL_RECARGOS";
        public const String Campo_Total_Corriente = "TOTAL_CORRIENTE";
        public const String Campo_Total_Rezago = "TOTAL_REZAGO";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Ope_Pre_Diferencias_Detalle
    ///DESCRIPCIÓN: clase que contiene los campos de la tabla Ope_Pre_Diferencias
    ///PARAMETROS: 
    ///CREO: jesus toledo
    ///FECHA_CREO: 10/Ago/2011 11:14:25 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Diferencias_Detalle
    {
        public const String Tabla_Ope_Pre_Diferencias_Detalle = "OPE_PRE_DIFERENCIAS_DETALLES";
        public const String Campo_No_Diferencias_Detalles = "NO_DIFERENCIAS_DETALLES";
        public const String Campo_No_Diferencia = "NO_DIFERENCIA";
        public const String Campo_Valor_Fiscal = "VALOR_FISCAL";
        public const String Campo_Tasa_Predial_ID = "TASA_PREDIAL_ID";
        public const String Campo_Tipo_Diferencia = "ALTA_BAJA";
        public const String Campo_Tipo_Periodo = "TIPO_PERIODO";
        public const String Campo_Cuota_Bimestral = "CUOTA_BIMESTRAL";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Periodo = "PERIODO";
        public const String Campo_Bimestre_1 = "BIMESTRE_1";
        public const String Campo_Bimestre_2 = "BIMESTRE_2";
        public const String Campo_Bimestre_3 = "BIMESTRE_3";
        public const String Campo_Bimestre_4 = "BIMESTRE_4";
        public const String Campo_Bimestre_5 = "BIMESTRE_5";
        public const String Campo_Bimestre_6 = "BIMESTRE_6";
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Ope_Pre_Coprop_Cuenta
    ///DESCRIPCIÓN: clase que contiene los campos de la tabla Ope_Pre_Coprop_Cuenta
    ///PARAMETROS: 
    ///CREO: jesus toledo
    ///FECHA_CREO: 10/Ago/2011 11:14:25 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Coprop_Cuenta
    {
        public const String Tabla_Ope_Pre_Coprop_Cuenta = "OPE_PRE_COPROP_CUENTA";
        public const String Campo_No_Copropietarios_Cuenta = "NO_COPROPIETARIOS_CUENTA";
        public const String Campo_Cuenta_ID = "CUENTA_ID";
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Ope_Pre_Coprop_Detalle
    ///DESCRIPCIÓN: clase que contiene los campos de la tabla Ope_Pre_Coprop_Detalle
    ///PARAMETROS: 
    ///CREO: jesus toledo
    ///FECHA_CREO: 10/Ago/2011 11:14:25 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Coprop_Detalle
    {
        public const String Tabla_Ope_Pre_Coprop_Detalle = "OPE_PRE_COPROP_DETALLE";
        public const String Campo_No_Copropietarios_Cuenta = "NO_COPROPIETARIOS_CUENTA";
        public const String Campo_Contribuyente = "CONTRIBUYENTE_ID";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Cajas
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla Cat_Pre_Cajas
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 22/Junio/2011 
    ///MODIFICO          : Miguel Angel Bedolla Moreno.
    ///FECHA_MODIFICO    : 29/Junio/2011
    ///CAUSA_MODIFICACIÓN: La tabla se ha implementado en la base de datos, contiene campos nuevos por lo qu es necesario realizar el ajuste.
    ///*******************************************************************************
    public class Cat_Pre_Cajas
    {
        public const String Tabla_Cat_Pre_Caja = "CAT_PRE_CAJAS";
        public const String Tabla_Cat_Pre_Cajas = "CAT_PRE_CAJAS";
        public const String Campo_Caja_Id = "CAJA_ID";

        public const String Campo_Caja_ID = "CAJA_ID";

        public const String Campo_Clave = "CLAVE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentario = "COMENTARIOS";
        public const String Campo_Numero_De_Caja = "NO_CAJA";
        public const String Campo_Foranea = "FORANEA";
        public const String Campo_Modulo_Id = "MODULO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Pre_Cajas_Empleados
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_CAJAS_EMPLEADOS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 10/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Pre_Cajas_Empleados
    {
        public const String Tabla_Cat_Pre_Cajas_Empleados = "CAT_PRE_CAJAS_EMPLEADOS";
        public const String Campo_Caja_ID = "CAJA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Cat_Pre_Clav_Ing_Presupuestos
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAJ_EMPLEADOS_FOLIOS
    ///PARAMETROS : 
    ///CREO       : Miguel Angel Bedolla Moreno
    ///FECHA_CREO : 10-Enero-2012
    ///MODIFICO          : 
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Clav_Ing_Presupuestos
    {
        public const String Tabla_Cat_Pre_Clav_Ing_Presupuestos = "CAT_PRE_CLAV_ING_PRESUPUESTOS";
        public const String Campo_Presupuesto_Id = "PRESUPUESTO_ID";
        public const String Campo_Clave_Ingreso_Id = "CLAVE_INGRESO_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Modulos
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla Cat_Pre_Modulos
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 22/Junio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Modulos
    {
        public const String Tabla_Cat_Pre_Modulo = "CAT_PRE_MODULOS";
        public const String Campo_Modulo_Id = "MODULO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Ubicacion = "UBICACION";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Ope_Pre_Convenios_Predial
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_CONVENIOS_PREDIAL
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 22/Agosto/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Convenios_Predial
    {
        public const String Tabla_Ope_Pre_Convenios_Predial = "OPE_PRE_CONVENIOS_PREDIAL";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Contribuyente_Id = "CONTRIBUYENTE_ID";
        public const String Campo_Realizo = "REALIZO";
        public const String Campo_No_Reestructura = "NO_REESTRUCTURA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Estatus_Cancelacion_Cuenta = "ESTATUS_CANCELACION_CUENTA";
        public const String Campo_Solicitante = "SOLICITANTE";
        public const String Campo_RFC = "RFC";
        public const String Campo_Numero_Parcialidades = "NUMERO_PARCIALIDADES";
        public const String Campo_Periodicidad_Pago = "PERIODICIDAD_PAGO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Descuento_Recargos_Ordinarios = "DESCUENTO_RECARGOS_ORDINARIOS";
        public const String Campo_Descuento_Recargos_Moratorios = "DESCUENTO_RECARGOS_MORATORIOS";
        public const String Campo_Descuento_Multas = "DESCUENTO_MULTAS";
        public const String Campo_No_Descuento = "NO_DESCUENTO";
        public const String Campo_Hasta_Periodo = "HASTA_PERIODO";
        public const String Campo_Total_Predial = "TOTAL_PREDIAL";
        public const String Campo_Total_Recargos = "TOTAL_RECARGOS";
        public const String Campo_Total_Moratorios = "TOTAL_MORATORIOS";
        public const String Campo_Total_Honorarios = "TOTAL_HONORARIOS";
        public const String Campo_Total_Adeudo = "TOTAL_ADEUDO";
        public const String Campo_Total_Descuento = "TOTAL_DESCUENTO";
        public const String Campo_Total_Convenio = "TOTAL_CONVENIO";
        public const String Campo_Sub_Total = "SUB_TOTAL";
        public const String Campo_Porcentaje_Anticipo = "PORCENTAJE_ANTICIPO";
        public const String Campo_Total_Anticipo = "TOTAL_ANTICIPO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Anticipo = "ANTICIPO";
        public const String Campo_Anticipo_Reestructura = "ANTICIPO_REESTRUCTURA";
        public const String Campo_Adeudo_Corriente = "ADEUDO_CORRIENTE";
        public const String Campo_Adeudo_Rezago = "ADEUDO_REZAGO";
        public const String Campo_Ruta_Convenio_Escaneado = "RUTA_CONVENIO_ESCANEADO";
        public const String Campo_Parcialidades_Manual = "PARCIALIDADES_MANUAL";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Rangos_De_Descuento_Por_Rol
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla Cat_Pre_Rangos_De_Descuento_Por_Rol
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 11/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Rangos_De_Descuento_Por_Rol
    {
        public const String Tabla_Cat_Pre_Rangos_De_Descuento_Por_Rol = "CAT_PRE_DESCUENTOS_POR_ROL";
        public const String Campo_Rangos_De_Descuento_Por_Rol_Id = "RANGOS_DE_DESCUENTO_POR_ROL_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Porcentaje = "PORCENTAJE";
        public const String Campo_Empleado_Id = "EMPLEADO_ID";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Descuentos_Generales
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla Cat_Pre_Descuentos_Generales
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 11/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Descuentos_Generales
    {
        public const String Tabla_Cat_Pre_Descuentos_Generales = "CAT_PRE_DESCUENTOS_GENERALES";
        public const String Campo_Descuentos_Generales_Id = "DESCUENTOS_GENERALES_ID";
        public const String Campo_Tipo_De_Descuento = "TIPO_DE_DESCUENTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Vigencia_Desde = "VIGENCIA_DESDE";
        public const String Campo_Vigencia_Hasta = "VIGENCIA_HASTA";
        public const String Campo_Porcentaje_Descuento = "PORCENTAJE_DESCUENTO";
        public const String Campo_Motivo = "MOTIVO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Instituciones_Que_Reciben_Pagos
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla 
    ///             Cat_Pre_Cajas_Instituciones_Que_Reciben_Pagos
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 15/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Instituciones_Que_Reciben_Pagos
    {
        public const String Tabla_Cat_Pre_Instituciones_Que_Reciben_Pagos = "CAT_PRE_INST_RECIBEN_PAGOS";
        public const String Campo_Institucion_Id = "INSTITUCION_ID";
        public const String Campo_Institucion = "INSTITUCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Caja = "CAJA_ID";
        public const String Campo_Mes = "MES";
        public const String Campo_Texto = "TEXTO";
        public const String Campo_Campos = "CAMPOS";
        public const String Campo_Linea_Captura_Enero = "LINEA_CAPTURA_ENERO";
        public const String Campo_Linea_Captura_Febrero = "LINEA_CAPTURA_FEBRERO";
        public const String Campo_Convenio = "CONVENIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Instituciones_Que_Reciben_Pagos
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla 
    ///             Cat_Pre_Cajas_Instituciones_Que_Reciben_Pagos
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 15/Julio/2011 
    ///MODIFICO          :Christian Perez Ibarra
    ///FECHA_MODIFICO    :19/Agosto/2011
    ///CAUSA_MODIFICACIÓN:Se cambio la referencia de la tabla por ope_caj_pagos
    ///*******************************************************************************
    public class Ope_Pre_Pagos
    {
        public const String Tabla_Ope_Pre_Pagos = "OPE_CAJ_PAGOS";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_No_Recibo = "NO_RECIBO";
        public const String Campo_No_Operacion = "NO_OPERACION";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Caja_Id = "CAJA_ID";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Clave_Banco = "CLAVE_BANCO";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Motivo_Cancelacion_Id = "MOTIVO_CANCELACION_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Periodo_Corriente_Inicial = "PERIODO_CORRIENTE_INICIAL";
        public const String Campo_Periodo_Corriente_Final = "PERIODO_CORRIENTE_FINAL";
        public const String Campo_Periodo_Rezago_Inicial = "PERIODO_REZAGO_INICIAL";
        public const String Campo_Periodo_Rezago_Final = "PERIODO_REZAGO_FINAL";
        public const String Campo_Monto_Corriente = "MONTO_CORRIENTE";
        public const String Campo_Monto_Rezago = "MONTO_REZAGO";
        public const String Campo_Monto_Recargos = "MONTO_RECARGOS";
        public const String Campo_Monto_Recargos_Ordinarios = "MONTO_RECARGOS_ORDINARIOS";
        public const String Campo_Monto_Recargos_Moratorios = "MONTO_RECARGOS_MORATORIOS";
        public const String Campo_Honorarios = "HONORARIOS";
        public const String Campo_Multas = "MULTAS";
        public const String Campo_Gastos_Ejecucion = "GASTOS_EJECUCION";
        public const String Campo_Descuento_Recargos = "DESCUENTO_RECARGOS";
        public const String Campo_Descuento_Honorarios = "DESCUENTO_HONORARIOS";
        public const String Campo_Descuento_Pronto_Pago = "DESCUENTO_PRONTO_PAGO";
        public const String Campo_Descuento_Multas = "DESCUENTO_MULTAS";
        public const String Campo_Ajuste_Tarifario = "AJUSTE_TARIFARIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Concepto = "CONCEPTO";
        public const String Campo_Empleado_Id = "EMPLEADO_ID";
        public const String Campo_No_Convenio = "NO_CONVENIO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Despachos_Externos
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla Cat_Pre_Despachos_Externos
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 16/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Despachos_Externos
    {
        public const String Tabla_Cat_Pre_Despachos_Externos = "CAT_PRE_DESPACHOS_EXTERNOS";
        public const String Campo_Despacho_Id = "DESPACHO_ID";
        public const String Campo_Despacho = "DESPACHO";
        public const String Campo_Contacto = "CONTACTO";
        public const String Campo_Calle = "CALLE_ID";
        public const String Campo_No_Exterior = "NO_EXTERIOR";
        public const String Campo_Telefono = "TELEFONO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Correo = "CORREO";
        public const String Campo_Colonia = "COLONIA_ID";
        public const String Campo_No_Interior = "NO_INTERIOR";
        public const String Campo_Fax = "FAX";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Salarios_Minimos
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla Cat_Pre_Salarios_Minimos
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 20/Julio/2011 
    ///MODIFICO          : 
    ///FECHA_MODIFICO    : 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Cat_Pre_Salarios_Minimos
    {
        public const String Tabla_Cat_Pre_Salarios_Minimos = "CAT_PRE_SALARIOS_MINIMOS";
        public const String Campo_Salario_Id = "SALARIO_MINIMO_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Ope_Caj_Cierre_Dia
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAJ_CIERRE_DIA
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 07/Agosto/2011 
    ///MODIFICO          : 
    ///FECHA_MODIFICO    : 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Ope_Caj_Cierre_Dia
    {
        public const String Tabla_Ope_Caj_Cierre_Dia = "OPE_CAJ_CIERRE_DIA";
        public const String Campo_No_Cierre_Dia = "NO_CIERRE_DIA";
        public const String Campo_Modulo_Id = "MODULO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Cierre_Dia = "FECHA_CIERRE_DIA";
        public const String Campo_Fecha_Reapertura_Dia = "FECHA_REAPERTURA_DIA";
        public const String Campo_Fecha_Reapertura_Cierre_Dia = "FECHA_REAPERTURA_CIERRE_DIA";
        public const String Campo_Empleado_Reabrio = "EMPLEADO_REABRIO_TURNO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Tasas_Predial
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_TASAS_PREDIAL
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 21/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************/
    public class Cat_Pre_Tasas_Predial
    {
        public const String Tabla_Cat_Pre_Tasas_Predial = "CAT_PRE_TASAS_PREDIAL";
        public const String Campo_Tasa_Predial_ID = "TASA_PREDIAL_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Tasas_Predial_Anual
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_TASAS
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 21/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************/
    public class Cat_Pre_Tasas_Predial_Anual
    {
        public const String Tabla_Cat_Pre_Tasas_Predial_Anual = "CAT_PRE_TASAS_PREDIAL_ANUAL";
        public const String Campo_Tasa_ID = "TASA_ID";
        public const String Campo_Tasa_Predial_ID = "TASA_PREDIAL_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Tasa_Anual = "TASA_ANUAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Fechas_Aplicacion
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_FECHAS_APLICACION
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 21/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************/
    public class Cat_Pre_Fechas_Aplicacion
    {
        public const String Tabla_Cat_Pre_Fechas_Aplicacion = "CAT_PRE_FECHAS_APLICACION";
        public const String Campo_Fecha_Aplicacion_ID = "FECHA_APLICACION_ID";
        public const String Campo_Fecha_Alta = "FECHA_ALTA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Movimiento = "FECHA_MOVIMIENTO";
        public const String Campo_Fecha_Aplicacion = "FECHA_APLICACION";
        public const String Campo_Motivo = "MOTIVO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Tipos_Bienes
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla Cat_Pre_Tipos_Bienes
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 23/Julio/2011 
    ///MODIFICO          : 
    ///FECHA_MODIFICO    : 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Cat_Pre_Tipos_Bienes
    {
        public const String Tabla_Cat_Pre_Tipos_Bienes = "CAT_PRE_TIPOS_BIENES";
        public const String Campo_Tipo_Bien_Id = "TIPO_BIEN_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    //*******************************************************************************
    //NOMBRE DE LA CLASE: Cat_Pre_Recargos_Traslado
    //DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_RECARGOS_TRASLADO
    //PARÁMETROS :     
    //CREO       : Miguel Angel Bedolla Moreno.
    //FECHA_CREO : 05/Agosto/2011 
    //MODIFICO          : 
    //FECHA_MODIFICO    : 
    //CAUSA_MODIFICACIÓN: 
    //*******************************************************************************
    public class Cat_Pre_Recargos_Traslado
    {
        public const String Tabla_Cat_Pre_Recargos_Traslado = "CAT_PRE_RECARGOS_TRASLADO";
        public const String Campo_Recargo_Traslado_Id = "RECARGO_TRASLADO_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Cuota = "TASA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Conceptos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CONCEPTOS
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Conceptos
    {
        public const String Tabla_Cat_Pre_Conceptos = "CAT_PRE_CONCEPTOS";
        public const String Campo_Concepto_Predial_ID = "CONCEPTO_PREDIAL_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Tipo_Concepto = "TIPO_CONCEPTO";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Conceptos_Imp_Predia
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CONCEPTOS_IMP_PREDIA
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          : Miguel Angel Bedolla Moreno
    /// FECHA_MODIFICO    : 12/Agosto/2011
    /// CAUSA_MODIFICACIÓN: La tabla contiene el campo IMPUESTO_ID_PREDIAL y la constante tiene el campo 'IMPUESTO_PREDIAL_ID', se camio al primero.
    ///*******************************************************************************
    public class Cat_Pre_Conceptos_Imp_Predia
    {
        public const String Tabla_Cat_Pre_Conceptos_Imp_Predia = "CAT_PRE_CONCEPTOS_IMP_PREDIA";
        public const String Campo_Impuesto_ID_Predial = "IMPUESTO_ID_PREDIAL";
        public const String Campo_Concepto_Predial_ID = "CONCEPTO_PREDIAL_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Tasa = "TASA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Conceptos_Imp_Trasl
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CONCEPTOS_IMP_TRASL
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Conceptos_Imp_Trasl
    {
        public const String Tabla_Cat_Pre_Conceptos_Imp_Trasl = "CAT_PRE_CONCEPTOS_IMP_TRASL";
        public const String Campo_Impuesto_ID_Traslacion = "IMPUESTO_ID_TRASLACION";
        public const String Campo_Concepto_Predial_ID = "CONCEPTO_PREDIAL_ID";
        public const String Campo_Deducible_Normal = "DEDUCIBLE_NORMAL";
        public const String Campo_Deducible_Interes_Social = "DEDUCIBLE_INTERES_SOCIAL";
        public const String Campo_Deducible_20_Salarios = "DEDUCIBLE_20_SALARIOS";
        public const String Campo_Año = "ANIO";
        public const String Campo_Tasa = "TASA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Divisiones
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_DIVISIONES
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Divisiones
    {
        public const String Tabla_Cat_Pre_Divisiones = "CAT_PRE_DIVISIONES";
        public const String Campo_Division_ID = "DIVISION_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Divisiones_Impuestos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_DIVISIONES_IMPUESTOS
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Divisiones_Impuestos
    {
        public const String Tabla_Cat_Pre_Divisiones_Impuestos = "CAT_PRE_DIVISIONES_IMPUESTOS";
        public const String Campo_Impuesto_Division_Lot_ID = "IMPUESTO_DIVISION_LOT_ID";
        public const String Campo_Division_ID = "DIVISION_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Tasa = "TASA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Fraccionamientos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_FRACCIONAMIENTOS
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Fraccionamientos
    {
        public const String Tabla_Cat_Pre_Fraccionamientos = "CAT_PRE_FRACCIONAMIENTOS";
        public const String Campo_Fraccionamiento_ID = "FRACCIONAMIENTO_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Fracc_Impuestos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_FRACC_IMPUESTOS
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Fracc_Impuestos
    {
        public const String Tabla_Cat_Pre_Fracc_Impuestos = "CAT_PRE_FRACC_IMPUESTOS";
        public const String Campo_Impuesto_Fraccionamiento_ID = "IMPUESTO_FRACCIONAMIENTO_ID";
        public const String Campo_Fraccionamiento_ID = "FRACCIONAMIENTO_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Otros_Documentos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_OTROS_DOCUMENTOS
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Otros_Documentos
    {
        public const String Tabla_Cat_Pre_Otros_Documentos = "CAT_PRE_OTROS_DOCUMENTOS";
        public const String Campo_Otro_Documento_ID = "OTRO_DOCUMENTO_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Clave_Generada = "CLAVE_GENERADA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Otros_Doc_Cuotas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_OTROS_DOC_CUOTAS
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Otros_Doc_Cuotas
    {
        public const String Tabla_Cat_Pre_Otros_Doc_Cuotas = "CAT_PRE_OTROS_DOC_CUOTAS";
        public const String Campo_Otro_Documento_Cuota_ID = "OTRO_DOCUMENTO_CUOTA_ID";
        public const String Campo_Otro_Documento_ID = "OTRO_DOCUMENTO_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Cuota = "CUOTA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Gastos_Ejecucion
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_GASTOS_EJECUCION
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************/
    public class Cat_Pre_Gastos_Ejecucion
    {
        public const String Tabla_Cat_Pre_Gastos_Ejecucion = "CAT_PRE_GASTOS_EJECUCION";
        public const String Campo_Gasto_Ejecucion_ID = "GASTO_EJECUCION_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Gastos_Ejec_Tasas
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_GASTOS_EJEC_TASAS
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Gastos_Ejec_Tasas
    {
        public const String Tabla_Cat_Pre_Gastos_Ejec_Tasas = "CAT_PRE_GASTOS_EJEC_TASAS";
        public const String Campo_Gasto_Ejecucion_Tasa_ID = "GASTO_EJECUCION_TASA_ID";
        public const String Campo_Gasto_Ejecucion_ID = "GASTO_EJECUCION_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Tasa = "COSTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Recargos
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_RECARGOS
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :    José Alfredo García Pichardo
     FECHA_MODIFICO    :    22/Julio/2011
     CAUSA_MODIFICACIÓN:    Se agregaron los campos de año y los meses.
    *******************************************************************************/
    public class Cat_Pre_Recargos
    {
        public const String Tabla_Cat_Pre_Recargos = "CAT_PRE_RECARGOS";
        public const String Campo_Recargo_ID = "RECARGO_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Anio_Tabulador = "ANIO_TABULADOR";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Bimestre = "BIMESTRE";
        public const String Campo_Enero = "ENERO";
        public const String Campo_Febrero = "FEBRERO";
        public const String Campo_Marzo = "MARZO";
        public const String Campo_Abril = "ABRIL";
        public const String Campo_Mayo = "MAYO";
        public const String Campo_Junio = "JUNIO";
        public const String Campo_Julio = "JULIO";
        public const String Campo_Agosto = "AGOSTO";
        public const String Campo_Septiembre = "SEPTIEMBRE";
        public const String Campo_Octubre = "OCTUBRE";
        public const String Campo_Noviembre = "NOVIEMBRE";
        public const String Campo_Diciembre = "DICIEMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Recargos_Tasas
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_RECARGOS_TASAS
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Recargos_Tasas
    {
        public const String Tabla_Cat_Pre_Recargos_Tasas = "CAT_PRE_RECARGOS_TASAS";
        public const String Campo_Recargo_Tasa_ID = "RECARGO_TASA_ID";
        public const String Campo_Recargo_ID = "RECARGO_ID";
        public const String Campo_No_Bimestro = "NO_BIMESTRO";
        public const String Campo_Enero = "ENERO";
        public const String Campo_Febrero = "FEBRERO";
        public const String Campo_Marzo = "MARZO";
        public const String Campo_Abril = "ABRIL";
        public const String Campo_Mayo = "MAYO";
        public const String Campo_Junio = "JUNIO";
        public const String Campo_Julio = "JULIO";
        public const String Campo_Agosto = "AGOSTO";
        public const String Campo_Septiembre = "SEPTIEMBRE";
        public const String Campo_Octubre = "OCTUBRE";
        public const String Campo_Noviembre = "NOVIEMBRE";
        public const String Campo_Diciembre = "DICIEMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Multas
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_MULTAS
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Multas
    {
        public const String Tabla_Cat_Pre_Multas = "CAT_PRE_MULTAS";
        public const String Campo_Multa_ID = "MULTA_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Desde_Anios = "DESDE_ANIOS";
        public const String Campo_Hasta_Anios = "HASTA_ANIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Multas_Cuotas
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_MULTAS_CUOTAS
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Multas_Cuotas
    {
        public const String Tabla_Cat_Pre_Multas_Cuotas = "CAT_PRE_MULTAS_CUOTAS";
        public const String Campo_Multa_Cuota_ID = "MULTA_CUOTA_ID";
        public const String Campo_Multa_ID = "MULTA_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Descuentos_Predial
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_DESCUENTOS_PREDIAL
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Descuentos_Predial
    {
        public const String Tabla_Cat_Pre_Descuentos_Predial = "CAT_PRE_DESCUENTOS_PREDIAL";
        public const String Campo_Descuento_ID = "DESCUENTO_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Enero = "ENERO";
        public const String Campo_Febrero = "FEBRERO";
        public const String Campo_Marzo = "MARZO";
        public const String Campo_Abril = "ABRIL";
        public const String Campo_Mayo = "MAYO";
        public const String Campo_Junio = "JUNIO";
        public const String Campo_Julio = "JULIO";
        public const String Campo_Agosto = "AGOSTO";
        public const String Campo_Septiembre = "SEPTIEMBRE";
        public const String Campo_Octubre = "OCTUBRE";
        public const String Campo_Noviembre = "NOVIEMBRE";
        public const String Campo_Diciembre = "DICIEMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Descuentos_Recargos
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_DESCUENTOS_RECARGOS
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Descuentos_Recargos
    {
        public const String Tabla_Cat_Pre_Descuentos_Recargos = "CAT_PRE_DESCUENTOS_RECARGOS";
        public const String Campo_Descuento_ID = "DESCUENTO_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Enero = "ENERO";
        public const String Campo_Febrero = "FEBRERO";
        public const String Campo_Marzo = "MARZO";
        public const String Campo_Abril = "ABRIL";
        public const String Campo_Mayo = "MAYO";
        public const String Campo_Junio = "JUNIO";
        public const String Campo_Julio = "JULIO";
        public const String Campo_Agosto = "AGOSTO";
        public const String Campo_Septiembre = "SEPTIEMBRE";
        public const String Campo_Octubre = "OCTUBRE";
        public const String Campo_Noviembre = "NOVIEMBRE";
        public const String Campo_Diciembre = "DICIEMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Diferencias
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_DIFERENCIAS
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Diferencias
    {
        public const String Tabla_Cat_Pre_Diferencias = "CAT_PRE_DIFERENCIAS";
        public const String Campo_Diferencia_ID = "DIFERENCIA_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Diferencias_Tasas
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_DIFERENCIAS_TASAS
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Diferencias_Tasas
    {
        public const String Tabla_Cat_Pre_Diferencias_Tasas = "CAT_PRE_DIFERENCIAS_TASAS";
        public const String Campo_Diferencia_Tasa_ID = "DIFERENCIA_TASA_ID";
        public const String Campo_Diferencia_ID = "DIFERENCIA_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Tasa = "TASA";
        public const String Campo_Tasa_Anual = "TASA_ANUAL";
        public const String Campo_Tasa_Bimestral = "TASA_BIMESTRAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Derechos_Supervision
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CONCEPTOS
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Derechos_Supervision
    {
        public const String Tabla_Cat_Pre_Derechos_Supervision = "CAT_PRE_DERECHOS_SUPERVISION";
        public const String Campo_Derecho_Supervision_ID = "DERECHO_SUPERVISION_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Der_Super_Tasas
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_DER_SUPER_TASAS
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Der_Super_Tasas
    {
        public const String Tabla_Cat_Pre_Der_Super_Tasas = "CAT_PRE_DER_SUPER_TASAS";
        public const String Campo_Derecho_Supervision_Tasa_ID = "DERECHO_SUPERVISION_TASA_ID";
        public const String Campo_Derecho_Supervision_ID = "DERECHO_SUPERVISION_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Tasa = "TASA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Casos_Especiales
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CASOS_ESPECIALES
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Casos_Especiales
    {
        public const String Tabla_Cat_Pre_Casos_Especiales = "CAT_PRE_CASOS_ESPECIALES";
        public const String Campo_Caso_Especial_ID = "CASO_ESPECIAL_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Articulo = "ARTICULO";
        public const String Campo_Inciso = "INCISO";
        public const String Campo_Aplicar_Descuento = "APLICA_DESCUENTO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Porcentaje = "PORCENTAJE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Cuotas_Minimas
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CUOTAS_MINIMAS
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Cuotas_Minimas
    {
        public const String Tabla_Cat_Pre_Cuotas_Minimas = "CAT_PRE_CUOTAS_MINIMAS";
        public const String Campo_Cuota_Minima_ID = "CUOTA_MINIMA_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Cuota = "CUOTA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Bloq_Cuentas_Predial
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_BLOQ_CUENTAS_PREDIAL
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Bloq_Cuentas_Predial
    {
        public const String Tabla_Cat_Pre_Bloq_Cuentas_Predial = "CAT_PRE_BLOQ_CUENTAS_PREDIAL";
        public const String Campo_Bloque_Cuenta_Predial_ID = "BLOQUE_CUENTA_PREDIAL_ID";
        public const String Campo_Cuenta_Predial = "CUENTA_PREDIAL";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Tipo_Bloqueo = "TIPO_BLOQUEO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Movimientos
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_MOVIMIENTOS
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Movimientos
    {
        public const String Tabla_Cat_Pre_Movimientos = "CAT_PRE_MOVIMIENTOS";
        public const String Campo_Movimiento_ID = "MOVIMIENTO_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Grupo_Id = "GRUPO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Traslado = "TRASLADO";
        public const String Campo_Aplica = "APLICA";
        public const String Campo_Cargar_Modulos = "CARGAR_MODULOS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Uso_Suelo
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_USO_SUELO
     PARÁMETROS :     
     CREO       : Francisco Antonio Gallardo Castañeda.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    ///*******************************************************************************/
    public class Cat_Pre_Uso_Suelo
    {
        public const String Tabla_Cat_Pre_Uso_Suelo = "CAT_PRE_USO_SUELO";
        public const String Campo_Uso_Suelo_ID = "USO_SUELO_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Estados_Predio
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_ESTADOS_PREDIO
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 28/Octubre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Estados_Predio
    {
        public const String Tabla_Cat_Pre_Estados_Predio = "CAT_PRE_ESTADOS_PREDIO";
        public const String Campo_Estado_Predio_ID = "ESTADO_PREDIO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Tipos_Predio
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_TIPOS_PREDIO
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 28/Octubre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Tipos_Predio
    {
        public const String Tabla_Cat_Pre_Tipos_Predio = "CAT_PRE_TIPOS_PREDIO";
        public const String Campo_Tipo_Predio_ID = "TIPO_PREDIO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Sectores
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_SECTORES
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 29/Octubre/2010 
    /// MODIFICO          : José Alfredo García Pichardo
    /// FECHA_MODIFICO    : José Alfredo García Pichardo
    /// CAUSA_MODIFICACIÓN: Se agrego el campo de Clave
    ///*******************************************************************************
    public class Cat_Pre_Sectores
    {
        public const String Tabla_Cat_Pre_Sectores = "CAT_PRE_SECTORES";
        public const String Campo_Sector_ID = "SECTOR_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Clave = "CLAVE";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Contribuyentes
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CONTRIBUYENTES
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 13/Septiembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Contribuyentes
    {
        public const String Tabla_Cat_Pre_Contribuyentes = "CAT_PRE_CONTRIBUYENTES";
        public const String Campo_Contribuyente_ID = "CONTRIBUYENTE_ID";
        public const String Campo_Apellido_Paterno = "APELLIDO_PATERNO";
        public const String Campo_Apellido_Materno = "APELLIDO_MATERNO";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Sexo = "SEXO";
        public const String Campo_Estado_Civil = "ESTADO_CIVIL";
        public const String Campo_Fecha_Nacimiento = "FECHA_NACIMIENTO";
        public const String Campo_RFC = "RFC";
        public const String Campo_CURP = "CURP";
        public const String Campo_IFE = "IFE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Tipo_Pesona = "TIPO_PERSONA";
        public const String Campo_Representante_Legal = "REPRESENTANTE_LEGAL";
        public const String Campo_Tipo_Propietario = "TIPO_PROPIETARIO";
        public const String Campo_Domicilio = "DOMICILIO";
        public const String Campo_Interior = "INTERIOR";
        public const String Campo_Exterior = "EXTERIOR";
        public const String Campo_Colonia = "COLONIA";
        public const String Campo_Ciudad = "CIUDAD";
        public const String Campo_Codigo_Postal = "CODIGO_POSTAL";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Domicilio_Foraneo = "DOMICILIO_FORANEO";
        public const String Campo_Interior_Foraneo = "INTERIOR_FORANEO";
        public const String Campo_Exterior_Foraneo = "EXTERIOR_FORANEO";
        public const String Campo_Colonia_Foraneo = "COLONIA_FORANEO";
        public const String Campo_Ciudad_Foraneo = "CIUDAD_FORANEO";
        public const String Campo_Codigo_Postal_Foraneo = "CODIGO_POSTAL_FORANEO";
        public const String Campo_Estado_Foraneo = "ESTADO_FORANEO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Nombre_Completo = "NOMBRE_COMPLETO";
        public const String Campo_Email = "EMAIL";
        public const String Campo_Password = "PASSWORD";
        public const String Campo_Calle_Ubicacion = "CALLE_UBICACION";
        public const String Campo_Colonia_Ubicacion = "COLONIA_UBICACION";
        public const String Campo_Ciudad_Ubicacion = "CIUDAD_UBICACION";
        public const String Campo_Estado_Ubicacion = "ESTADO_UBICACION";
        public const String Campo_Calle_ID = "CALLE_ID";
        public const String Campo_Colonia_ID = "COLONIA_ID";
        public const String Campo_Telefono_Casa = "TELEFONO";
        public const String Campo_Celular = "CELULAR";
        public const String Campo_Edad = "EDAD";
        public const String Campo_Fecha_Registro = "FECHA_REGISTRO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Pregunta_Secreta = "PREGUNTA_SECRETA";
        public const String Campo_Respuesta_Secreta = "RESPUESTA_SECRETA";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Calles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CALLES
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 17/Septiembre/2010 
    /// MODIFICO          : José Alfredo García Pichardo
    /// FECHA_MODIFICO    : 20/Julio/2011
    /// CAUSA_MODIFICACIÓN: Se agregaron aulgunos campos a la tabla de CAT_PRE_CALLES
    ///*******************************************************************************
    public class Cat_Pre_Calles
    {
        public const String Tabla_Cat_Pre_Calles = "CAT_PRE_CALLES";
        public const String Campo_Calle_ID = "CALLE_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Tipo_Calle = "TIPO_CALLE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Colonia_ID = "COLONIA_ID";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Estatus = "ESTATUS";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Calles_Colonias
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CALLES_COLONIAS
    /// PARÁMETROS :     
    /// CREO       : Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO : 17/Septiembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Calles_Colonias
    {
        public const String Tabla_Cat_Pre_Calles_Colonias = "CAT_PRE_CALLES_COLONIAS";
        public const String Campo_Calle_Colonia_ID = "CALLE_COLONIA_ID";
        public const String Campo_Calle_ID = "CALLE_ID";
        public const String Campo_Colonia_ID = "COLONIA_ID";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE : Cat_Pre_Notarios
    /// DESCRIPCIÓN        : Clase que contiene los campos de la tabla CAT_PRE_NOTARIOS
    /// PARÁMETROS         : 
    /// CREO               : Antonio Salvador Benavides Guardado
    /// FECHA_CREO         : 26/Octubre/2010 
    /// MODIFICO           :
    /// FECHA_MODIFICO     :
    /// CAUSA_MODIFICACIÓN :
    ///*******************************************************************************
    public class Cat_Pre_Notarios
    {
        public const String Tabla_Cat_Pre_Notarios = "CAT_PRE_NOTARIOS";
        public const String Campo_Notario_ID = "NOTARIO_ID";
        public const String Campo_Apellido_Paterno = "APELLIDO_PATERNO";
        public const String Campo_Apellido_Materno = "APELLIDO_MATERNO";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Sexo = "SEXO";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Ciudad = "CIUDAD";
        public const String Campo_Colonia = "COLONIA";
        public const String Campo_Calle = "CALLE";
        public const String Campo_Numero_Exterior = "NUMERO_EXTERIOR";
        public const String Campo_Numero_Interior = "NUMERO_INTERIOR";
        public const String Campo_Codigo_Postal = "CODIGO_POSTAL";
        public const String Campo_RFC = "RFC";
        public const String Campo_CURP = "CURP";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Telefono = "TELEFONO";
        public const String Campo_Fax = "FAX";
        public const String Campo_Celular = "CELULAR";
        public const String Campo_E_Mail = "E_MAIL";
        public const String Campo_Numero_Notaria = "NUMERO_NOTARIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE : Cat_Pre_Tipos_Colonias
    /// DESCRIPCIÓN        : Clase que contiene los campos de la tabla CAT_PRE_TIPOS_COLONIAS
    /// PARÁMETROS         : 
    /// CREO               : Antonio Salvador Benavides Guardado
    /// FECHA_CREO         : 27/Octubre/2010 
    /// MODIFICO           :
    /// FECHA_MODIFICO     :
    /// CAUSA_MODIFICACIÓN :
    ///*******************************************************************************
    public class Cat_Pre_Tipos_Colonias
    {
        public const String Tabla_Cat_Pre_Tipos_Colonias = "CAT_PRE_TIPOS_COLONIAS";
        public const String Campo_Tipo_Colonia_ID = "TIPO_COLONIA_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE : Cat_Pre_Rangos_Identificadores_Colonias
    /// DESCRIPCIÓN        : Clase que contiene los campos de la tabla CAT_PRE_RANGOS_IDENTIFICADORES_COLONIAS
    /// PARÁMETROS         : 
    /// CREO               : Antonio Salvador Benavides Guardado
    /// FECHA_CREO         : 28/Octubre/2010 
    /// MODIFICO           :
    /// FECHA_MODIFICO     :
    /// CAUSA_MODIFICACIÓN :
    ///*******************************************************************************
    public class Cat_Pre_Rangos_Identificadores_Colonias
    {
        public const String Tabla_Cat_Pre_Rangos_Identificadores_Colonias = "CAT_PRE_RAN_IDENT_COLONIAS";
        public const String Campo_Rango_Identificador_Colonia_ID = "RANGO_IDENTIFICADOR_COLONIA_ID";
        public const String Campo_Tipo_Colonia_ID = "TIPO_COLONIA_ID";
        public const String Campo_Rango_Inicial = "RANGO_INICIAL";
        public const String Campo_Rango_Final = "RANGO_FINAL";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Historial_Obs_Recep
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_HISTORIAL_OBS_RECEP
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 08/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Historial_Obs_Recep
    {
        public const String Tabla_Ope_Pre_Historial_Obs_Recep = "OPE_PRE_HISTORIAL_OBS_RECEP";
        public const String Campo_No_Observacion_ID = "NO_OBSERVACION_ID";
        public const String Campo_No_Movimiento = "NO_MOVIMIENTO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Contrarecibos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_CONTRARECIBOS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 08/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Contrarecibos
    {
        public const String Tabla_Ope_Pre_Contrarecibos = "OPE_PRE_CONTRARECIBOS";
        public const String Campo_No_Contrarecibo = "NO_CONTRARECIBO";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Notario_ID = "NOTARIO_ID";
        public const String Campo_Listado_ID = "LISTADO_ID";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_No_Escritura = "NO_ESCRITURA";
        public const String Campo_Fecha_Escritura = "FECHA_ESCRITURA";
        public const String Campo_Fecha_Liberacion = "FECHA_LIBERACION";
        public const String Campo_Fecha_Pago = "FECHA_PAGO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Recepcion_Documentos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_RECEPCION_DOCUMENTOS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 08/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Recepcion_Documentos
    {
        public const String Tabla_Ope_Pre_Recepcion_Documentos = "OPE_PRE_RECEPCION_DOCUMENTOS";
        public const String Campo_No_Recepcion_Documento = "NO_RECEPCION_DOCUMENTO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Clave_Tramite = "CLAVE_TRAMITE";
        public const String Campo_Notario_ID = "NOTARIO_ID";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Listados
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_LISTADOS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 10/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Listados
    {
        public const String Tabla_Ope_Pre_Listados = "OPE_PRE_LISTADOS";
        public const String Campo_Listado_ID = "LISTADO_ID";
        public const String Campo_Notario_ID = "NOTARIO_ID";
        public const String Campo_Fecha_Generacion = "FECHA_GENERACION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Cat_Pre_Cuentas_Predial
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla CAT_PRE_CUENTAS_PREDIAL
    /// PARÁMETROS :     
    /// CREO       			: Antonio Salvador Benavides Guardado
    /// FECHA_CREO 			: 01/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Cuentas_Predial
    {
        public const String Tabla_Cat_Pre_Cuentas = "CAT_PRE_CUENTAS_PREDIAL";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Cuenta_Predial = "CUENTA_PREDIAL";
        public const String Campo_Calle_ID = "CALLE_ID";
        public const String Campo_Propietario_ID = "PROPIETARIO_ID";
        public const String Campo_Copropietario_ID = "COPROPIETARIO_ID";
        public const String Campo_Estado_Predio_ID = "ESTADO_PREDIO_ID";
        public const String Campo_Tipo_Predio_ID = "TIPO_PREDIO_ID";
        public const String Campo_Uso_Suelo_ID = "USO_SUELO_ID";
        public const String Campo_Tasa_Predial_ID = "TASA_PREDIAL_ID";
        public const String Campo_Tasa_ID = "TASA_ID";
        public const String Campo_Cuota_Minima_ID = "CUOTA_MINIMA_ID";
        public const String Campo_Cuenta_Origen = "CUENTA_ORIGEN";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_No_Exterior = "NO_EXTERIOR";
        public const String Campo_No_Interior = "NO_INTERIOR";
        public const String Campo_Superficie_Construida = "SUPERFICIE_CONSTRUIDA";
        public const String Campo_Superficie_Total = "SUPERFICIE_TOTAL";
        public const String Campo_Clave_Catastral = "CLAVE_CATASTRAL";
        public const String Campo_Valor_Fiscal = "VALOR_FISCAL";
        public const String Campo_Efectos = "EFECTOS";
        public const String Campo_Periodo_Corriente = "PERIODO_CORRIENTE";
        public const String Campo_Cuota_Anual = "CUOTA_ANUAL";
        public const String Campo_Porcentaje_Exencion = "PORCENTAJE_EXENCION";
        public const String Campo_Cuota_Fija = "CUOTA_FIJA";
        public const String Campo_Termino_Exencion = "TERMINO_EXENCION";
        public const String Campo_Fecha_Avaluo = "FECHA_AVALUO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Diferencia_Construccion = "DIFERENCIA_CONSTRUCCION";
        public const String Campo_Costo_m2 = "COSTO_M2";
        public const String Campo_Tipo_Beneficio = "TIPO_BENEFICIO";
        public const String Campo_No_Couta_Fija = "NO_CUOTA_FIJA";
        public const String Campo_No_Diferencia = "NO_DIFERENCIA";
        public const String Campo_Calle_ID_Notificacion = "CALLE_ID_NOTIFICACION";
        public const String Campo_Colonia_ID = "COLONIA_ID";
        public const String Campo_Colonia_ID_Notificacion = "COLONIA_ID_NOTIFICACION";
        public const String Campo_Estado_ID_Notificacion = "ESTADO_ID_NOTIFICACION";
        public const String Campo_Ciudad_ID_Notificacion = "CIUDAD_ID_NOTIFICACION";
        public const String Campo_Estado_Notificacion = "ESTADO_NOTIFICACION";
        public const String Campo_Ciudad_Notificacion = "CIUDAD_NOTIFICACION";
        public const String Campo_Domicilio_Foraneo = "DOMICILIO_FORANEO";
        public const String Campo_Calle_Notificacion = "CALLE_NOTIFICACION";
        public const String Campo_Colonia_Notificacion = "COLONIA_NOTIFICACION";
        public const String Campo_No_Exterior_Notificacion = "NO_EXTERIOR_NOTIFICACION";
        public const String Campo_No_Interior_Notificacion = "NO_INTERIOR_NOTIFICACION";
        public const String Campo_Codigo_Postal = "CODIGO_POSTAL";
        public const String Campo_Tipo_Suspencion = "TIPO_SUSPENCION";
        public const String Campo_Region = "REGION";
        public const String Campo_Manzana = "MANZANA";
        public const String Campo_Lote = "LOTE";
        public const String Campo_Horas_X = "HORAS_X";
        public const String Campo_Minutos_X = "MINUTOS_X";
        public const String Campo_Segundos_X = "SEGUNDOS_X";
        public const String Campo_Orientacion_X = "ORIENTACION_X";
        public const String Campo_Horas_Y = "HORAS_Y";
        public const String Campo_Minutos_Y = "MINUTOS_Y";
        public const String Campo_Segundos_Y = "SEGUNDOS_Y";
        public const String Campo_Orientacion_Y = "ORIENTACION_Y";
        public const String Campo_Coordenadas_UTM = "COORDENADAS_UTM";
        public const String Campo_Coordenadas_UTM_Y = "COORDENADAS_Y_UTM";
        public const String Campo_Tipo = "TIPO";


    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Orden_Variacion
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_ORDEN_VARIACION
    /// PARÁMETROS :     
    /// CREO       			: Jesus Toledo Rodriguez
    /// FECHA_CREO 			: 10/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Orden_Variacion
    {
        public const String Tabla_Ope_Pre_Orden_Variacion = "OPE_PRE_ORDEN_VARIACION";
        public const String Campo_No_Orden_Variacion = "NO_ORDEN_VARIACION";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Movimiento_ID = "MOVIMIENTO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_No_Contrarecibo = "NO_CONTRARECIBO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_No_Nota = "NO_NOTA";
        public const String Campo_Fecha_Nota = "FECHA_NOTA";
        public const String Campo_Numero_Nota_Impreso = "NUMERO_NOTA_IMPRESO";
        public const String Campo_Grupo_Movimiento_ID = "GRUPO_MOVIMIENTO_ID";
        public const String Campo_Tipo_Predio_ID = "TIPO_PREDIO_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Orden_Detalles
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_ORDEN_DETALLES
    /// PARÁMETROS :     
    /// CREO       			: Jesus Toledo Rodriguez
    /// FECHA_CREO 			: 10/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Orden_Detalles
    {
        public const String Tabla_Ope_Pre_Orden_Detalles = "OPE_PRE_ORDEN_DETALLES";
        public const String Campo_No_Orden_Detalles = "NO_ORDEN_DETALLES";
        public const String Campo_No_Orden_Variacion = "NO_ORDEN_VARIACION";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Campo = "CAMPO";
        public const String Campo_Dato_Anterior = "DATO_ANTERIOR";
        public const String Campo_Dato_Nuevo = "DATO_NUEVO";

    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Cuotas_Fijas
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_CUOTAS_FIJAS
    /// PARÁMETROS :     
    /// CREO       			: Jesus Toledo Rodriguez
    /// FECHA_CREO 			: 10/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Cuotas_Fijas
    {
        public const String Tabla_Ope_Pre_Cuotas_Fijas = "OPE_PRE_CUOTAS_FIJAS";
        public const String Campo_No_Cuota_Fija = "NO_CUOTA_FIJA";
        public const String Campo_Plazo_Financiamiento = "PLAZO_FINANCIAMIENTO";
        public const String Campo_Exedente_Construccion = "EXCEDENTE_CONTRUCCION";
        public const String Campo_Exedente_Valor = "EXCEDENTE_VALOR";
        public const String Campo_Total_Cuota_Fija = "TOTAL_CUOTA_FIJA";
        public const String Campo_Caso_Especial_Id = "CASO_ESPECIAL_ID";
        public const String Campo_Tasa_ID = "TASA_ID";
        public const String Campo_Tasa_Valor = "TASA_VALOR";
        public const String Campo_Cuota_Minima = "CUOTA_MINIMA";
        public const String Campo_Excedente_Construccion_Total = "EXCEDENTE_CONSTRUCCION_TOTAL";
        public const String Campo_Excedente_Valor_Total = "EXCEDENTE_VALOR_TOTAL";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_No_Orden_Variacion = "NO_ORDEN_VARIACION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Ordenes_Variacion
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_ORDENES_VARIACION
    /// PARÁMETROS :     
    /// CREO       			: Antonio Salvador Benavides Guardado
    /// FECHA_CREO 			: 20/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Ordenes_Variacion
    {
        public const String Tabla_Ope_Pre_Ordenes_Variacion = "OPE_PRE_ORDENES_VARIACION";
        public const String Campo_No_Orden_Variacion = "NO_ORDEN_VARIACION";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Tipo_Predio_ID = "TIPO_PREDIO_ID";
        public const String Campo_Estado_Predio_ID = "ESTADO_PREDIO_ID";
        public const String Campo_Tasa_ID = "TASA_ID";
        public const String Campo_Tasa_Predial_ID = "TASA_PREDIAL_ID";
        public const String Campo_Uso_Suelo_ID = "USO_SUELO_ID";
        public const String Campo_Cuota_Minima_ID = "CUOTA_MINIMA_ID";
        public const String Campo_Grupo_Movimiento_ID = "GRUPO_MOVIMIENTO_ID";
        public const String Campo_Movimiento_ID = "MOVIMIENTO_ID";
        public const String Campo_No_Contrarecibo = "NO_CONTRARECIBO";
        public const String Campo_No_Cuota_Fija = "NO_CUOTA_FIJA";
        public const String Campo_Cuenta_Predial = "CUENTA_PREDIAL";
        public const String Campo_Cuenta_Origen = "CUENTA_ORIGEN";
        public const String Campo_Clave_Catastral = "CLAVE_CATASTRAL";
        public const String Campo_Estatus_Orden = "ESTATUS_ORDEN";
        public const String Campo_Estatus_Cuenta = "ESTATUS_CUENTA";
        public const String Campo_Valor_Fiscal = "VALOR_FISCAL";
        public const String Campo_Porcentaje_Exencion = "PORCENTAJE_EXENCION";
        public const String Campo_Superficie_Construida = "SUPERFICIE_CONSTRUIDA";
        public const String Campo_Superficie_Total = "SUPERFICIE_TOTAL";
        public const String Campo_Termino_Exencion = "TERMINO_EXENCION";
        public const String Campo_Fecha_Avaluo = "FECHA_AVALUO";
        public const String Campo_Domicilio_Foraneo = "DOMICILIO_FORANEO";
        public const String Campo_Calle_ID = "CALLE_ID";
        public const String Campo_Calle_ID_Notificacion = "CALLE_ID_NOTIFICACION";
        public const String Campo_Calle_Notificacion = "CALLE_NOTIFICACION";
        public const String Campo_Colonia_ID = "COLONIA_ID";
        public const String Campo_Colonia_ID_Notificacion = "COLONIA_ID_NOTIFICACION";
        public const String Campo_Colonia_Notificacion = "COLONIA_NOTIFICACION";
        public const String Campo_Ciudad_ID_Notificacion = "CIUDAD_ID_NOTIFICACION";
        public const String Campo_Ciudad_Notificacion = "CIUDAD_NOTIFICACION";
        public const String Campo_Estado_ID_Notificacion = "ESTADO_ID_NOTIFICACION";
        public const String Campo_Estado_Notificacion = "ESTADO_NOTIFICACION";
        public const String Campo_No_Exterior = "NO_EXTERIOR";
        public const String Campo_No_Exterior_Notificacion = "NO_EXTERIOR_NOTIFICACION";
        public const String Campo_No_Interior = "NO_INTERIOR";
        public const String Campo_No_Interior_Notificacion = "NO_INTERIOR_NOTIFICACION";
        public const String Campo_Codigo_Postal = "CODIGO_POSTAL";
        public const String Campo_Efectos = "EFECTOS";
        public const String Campo_Periodo_Corriente = "PERIODO_CORRIENTE";
        public const String Campo_Costo_M2 = "COSTO_M2";
        public const String Campo_Cuota_Anual = "CUOTA_ANUAL";
        public const String Campo_Cuota_Fija = "CUOTA_FIJA";
        public const String Campo_Diferencia_Construccion = "DIFERENCIA_CONSTRUCCION";
        public const String Campo_Tipo_Suspencion = "TIPO_SUSPENCION";
        public const String Campo_No_Nota = "NO_NOTA";
        public const String Campo_Fecha_Nota = "FECHA_NOTA";
        public const String Campo_Numero_Nota_Impreso = "NUMERO_NOTA_IMPRESO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Aplico_Convenio = "APLICO_CONVENIO";
        public const String Campo_Usuario_Realizo = "USUARIO_REALIZO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Usuario_Valido = "USUARIO_VALIDO";
        public const String Campo_Fecha_Valido = "FECHA_VALIDO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Observaciones
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_OBSERVACIONES
    /// PARÁMETROS :     
    /// CREO       			: Antonio Salvador Benavides Guardado
    /// FECHA_CREO 			: 21/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Observaciones
    {
        public const String Tabla_Ope_Pre_Observaciones_Orden_Variacion = "OPE_PRE_OBSERV_ORDEN_VARIACION";
        public const String Campo_Observaciones_ID = "OBSERVACIONES_ID";
        public const String Campo_No_Orden_Variacion = "NO_ORDEN_VARIACION";
        public const String Campo_Año = "ANIO";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Copropietarios_Orde_Variacion
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_COPROP_ORDEN_VARIAC
    /// PARÁMETROS :     
    /// CREO       			: Antonio Salvador Benavides Guardado
    /// FECHA_CREO 			: 14/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Copropietarios_Orde_Variacion
    {
        public const String Tabla_Ope_Pre_Copropietarios_Orden_Variacion = "OPE_PRE_COPROP_ORDEN_VARIAC";
        public const String Campo_Copropietario_Orden_Variacion_ID = "COPROPIETARIO_ORDEN_VARIAC_ID";
        public const String Campo_No_Orden_Variacion = "NO_ORDEN_VARIACION";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Contribuyente_ID = "CONTRIBUYENTE_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Cat_Pre_Tipos_Documento
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla CAT_PRE_TIPOS_DOCUMENTO
    /// PARÁMETROS :     
    /// CREO       			: Roberto González Oseguera
    /// FECHA_CREO 			: 22-mar-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Tipos_Documento
    {
        public const String Tabla_Cat_Pre_Tipos_Documento = "CAT_PRE_TIPOS_DOCUMENTO";
        public const String Campo_Documento_ID = "DOCUMENTO_ID";
        public const String Campo_Nombre_Documento = "NOMBRE_DOCUMENTO";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Recep_Docs_Anexos
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_RECEP_DOCS_ANEXOS
    /// PARÁMETROS :     
    /// CREO       			: Roberto González Oseguera
    /// FECHA_CREO 			: 24-mar-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Recep_Docs_Anexos
    {
        public const String Tabla_Ope_Pre_Recep_Docs_Anexos = "OPE_PRE_RECEP_DOCS_ANEXOS";
        public const String Campo_No_Anexo = "NO_ANEXO";
        public const String Campo_No_Movimiento = "NO_MOVIMIENTO";
        public const String Campo_Ruta = "RUTA";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Documento_ID = "DOCUMENTO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Recep_Docs_Movs
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_RECEP_DOCS_MOVS
    /// PARÁMETROS :     
    /// CREO       			: Roberto González Oseguera
    /// FECHA_CREO 			: 24-mar-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Recep_Docs_Movs
    {
        public const String Tabla_Ope_Pre_Recep_Docs_Movs = "OPE_PRE_RECEP_DOCS_MOVS";
        public const String Campo_No_Movimiento = "NO_MOVIMIENTO";
        public const String Campo_No_Recepcion_Documento = "NO_RECEPCION_DOCUMENTO";
        public const String Campo_Numero_Escritura = "NUMERO_ESCRITURA";
        public const String Campo_Fecha_Escritura = "FECHA_ESCRITURA";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_No_Contrarecibo = "NO_CONTRARECIBO";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Recep_Docs_Observ
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_RECEP_DOCS_OBSERV
    /// PARÁMETROS :     
    /// CREO       			: Roberto González Oseguera
    /// FECHA_CREO 			: 18-may-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Recep_Docs_Observ
    {
        public const String Tabla_Ope_Pre_Recep_Docs_Observ = "OPE_PRE_RECEP_DOCS_OBSERV";
        public const String Campo_No_Observacion = "NO_OBSERVACION";
        public const String Campo_No_Movimiento = "NO_MOVIMIENTO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Fecha_Hora = "FECHA_HORA";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Cat_Pre_Propietarios
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla CAT_PRE_PROPIETARIOS
    /// PARÁMETROS :     
    /// CREO       			: Roberto González Oseguera
    /// FECHA_CREO 			: 30-mar-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Propietarios
    {
        public const String Tabla_Cat_Pre_Propietarios = "CAT_PRE_PROPIETARIOS";
        public const String Campo_Propietario_ID = "PROPIETARIO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Contribuyente_ID = "CONTRIBUYENTE_ID";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pre_Turnos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_TURNOS
    /// PARÁMETROS :
    /// CREO       : José Alfredo García Pichardo
    /// FECHA_CREO : 28/Junio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Turnos
    {
        public const String Tabla_Cat_Pre_Turnos = "CAT_TURNOS";
        public const String Campo_Turno_ID = "TURNO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Hora_Inicio = "HORA_INICIO";
        public const String Campo_Hora_Fin = "HORA_FIN";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Cajas_Detalles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAT_CAJAS_DETALLES
    /// PARÁMETROS :
    /// CREO       : José Alfredo García Pichardo
    /// FECHA_CREO : 04/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Cajas_Detalles
    {
        public const String Tabla_Ope_Pre_Cajas_Detalles = "OPE_PRE_CAJAS_DETALLES";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Caja_ID = "CAJA_ID";
        public const String Campo_Turno_ID = "TURNO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Cat_Pre_Tipos_Constancias
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla CAT_PRE_TIPOS_CONSTANCIAS
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 29/Junio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pre_Tipos_Constancias
    {
        public const String Tabla_Cat_Pre_Tipos_Constancias = "CAT_PRE_TIPOS_CONSTANCIAS";
        public const String Campo_Tipo_Constancia_ID = "TIPO_CONSTANCIA_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Año = "AÑO";
        public const String Campo_Costo = "COSTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Certificacion = "CERTIFICACION";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Constancias
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_CONSTANCIAS
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 29/Junio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Constancias
    {
        public const String Tabla_Ope_Pre_Constancias = "OPE_PRE_CONSTANCIAS";
        public const String Campo_No_Constancia = "NO_CONSTANCIA";
        public const String Campo_Tipo_Constancia_ID = "TIPO_CONSTANCIA_ID";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Propietario_ID = "PROPIETARIO_ID";
        public const String Campo_Realizo = "REALIZO";
        public const String Campo_Confronto = "CONFRONTO";
        public const String Campo_Documento_ID = "DOCUMENTO_ID";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_No_Recibo = "NO_RECIBO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Periodo_Año = "PERIODO_AÑO";
        public const String Campo_Periodo_Bimestre = "PERIODO_BIMESTRE";
        public const String Campo_Periodo_Hasta_Anio = "HASTA_ANIO";
        public const String Campo_Periodo_Hasta_Bimestre = "HASTA_BIMESTRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Leyenda_Certificacion = "LEYENDA_CERTIFICACION";
        public const String Campo_No_Impresiones = "NO_IMPRESIONES";
        public const String Campo_Proteccion_Pago = "PROTECCION_PAGO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Solicitante = "SOLICITANTE";
        public const String Campo_Solicitante_RFC = "SOLICITANTE_RFC";
        public const String Campo_Domicilio = "DOMICILIO";
        public const String Campo_Anio = "ANIO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Impuestos_Fraccionamientos
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_IMPUESTO_FRACC
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 22/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Impuestos_Fraccionamientos
    {
        public const String Tabla_Ope_Pre_Impuestos_Fraccionamientos = "OPE_PRE_IMPUESTO_FRACC";
        public const String Campo_No_Impuesto_Fraccionamiento = "NO_IMPUESTO_FRACCIONAMIENTO";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Fecha_Oficio = "FECHA_OFICIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Detalles_Impuestos_Fraccionamientos
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_DET_IMPUESTO_FRACC
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 22/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Detalles_Impuestos_Fraccionamientos
    {
        public const String Tabla_Ope_Pre_Detalles_Impuestos_Fraccionamientos = "OPE_PRE_DET_IMPUESTO_FRACC";
        public const String Campo_No_Impuesto_Fraccionamiento = "NO_IMPUESTO_FRACCIONAMIENTO";
        public const String Campo_Impuesto_Fraccionamiento_ID = "IMPUESTO_FRACCIONAMIENTO_ID";
        public const String Campo_Superficie_Fraccionar = "SUPERFICIE_FRACCIONAR";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Recargos = "RECARGOS";
        public const String Campo_Multas_Id = "MULTAS_ID";
        public const String Campo_Total = "TOTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Impuestos_Derechos_Supervision
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_IMPUESTO_DER_SUP
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 22/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Impuestos_Derechos_Supervision
    {
        public const String Tabla_Ope_Pre_Impuestos_Derechos_Supervision = "OPE_PRE_IMPUESTO_DER_SUP";
        public const String Campo_No_Impuesto_Derecho_Supervision = "NO_IMPUESTO_DERECHO_SUPERVISIO";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Fecha_Oficio = "FECHA_OFICIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Detalles_Impuestos_Derechos_Supervision
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_DET_IMPUESTO_DER_SUP
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 22/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Detalles_Impuestos_Derechos_Supervision
    {
        public const String Tabla_Ope_Pre_Detalles_Impuestos_Derechos_Supervision = "OPE_PRE_DET_IMPUESTO_DER_SUP";
        public const String Campo_No_Impuesto_Derecho_Supervision = "NO_IMPUESTO_DERECHO_SUPERVISIO";
        public const String Campo_Derecho_Supervision_Tasa_ID = "DERECHO_SUPERVISION_TASA_ID";
        public const String Campo_Valor_Estimado_Obra = "VALOR_ESTIMADO_OBRA";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Recargos = "RECARGOS";
        public const String Campo_Multas_Id = "MULTAS_ID";
        public const String Campo_Total = "TOTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Descuentos_Predial
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_DESCUENTOS_PREDIAL
    /// PARÓMETROS :     
    /// CREO       : Jacqueline Ramirez Sierra
    /// FECHA_CREO : 12/Agosto/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Ope_Pre_Descuentos_Predial
    {
        public const String Tabla_Ope_Pre_Descuentos_Predial = "OPE_PRE_DESCUENTOS_PREDIAL";
        public const String Campo_No_Descuento_Predial = "NO_DESCUENTO_PREDIAL";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Anio_Inicial = "DESDE_PERIODO_ANIO";
        public const String Campo_Bimestre_Inicial = "DESDE_PERIODO_BIMESTRE";
        public const String Campo_Anio_Final = "HASTA_PERIODO_ANIO";
        public const String Campo_Bimestre_Final = "HASTA_PERIODO_BIMESTRE";
        public const String Campo_Desde_Periodo = "DESDE_PRERIODO";
        public const String Campo_Asta_Periodo = "ASTA_PRERIODO";
        public const String Campo_Porcentaje_Multa = "PORCENTAJE_MULTA";
        public const String Campo_Porcentaje_Recargo = "PORCENTAJE_RECARGO";
        public const String Campo_Porcentaje_Recargo_Moratorio = "PORCENTAJE_RECARGO_MORATORIO";
        public const String Campo_Porcentaje_Pronto_Pago = "PORCENTAJE_PRONTO_PAGO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Recargos = "RECARGOS";
        public const String Campo_Recargos_Moratorios = "RECARGOS_MORATORIOS";
        public const String Campo_Honorarios = "HONORARIOS";
        public const String Campo_Total_Impuesto = "TOTAL_IMPUESTO";
        public const String Campo_Desc_Recargo = "DESC_RECARGO";
        public const String Campo_Desc_Recargo_Moratorio = "DESC_RECARGO_MORATORIO";
        public const String Campo_Descuento_Pronto_Pago = "DESCUENTO_PRONTO_PAGO";
        public const String Campo_Desc_Multa = "DESC_MULTA";
        public const String Campo_Total_a_Pagar = "TOTAL_A_PAGAR";
        public const String Campo_Realizo = "REALIZO";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Fundamento_Legal = "FUNDAMENTO_LEGAL";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_Constribuyente_ID = "CONTRIBUYENTE_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Rezagos = "REZAGOS";
        public const String Campo_Corriente = "CORRIENTE";
        public const String Campo_Multas = "MULTAS";
    }
    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Grupos
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_GRUPOS
     PARÁMETROS :     
     CREO       : Miguel Angel Bedolla Moreno
     FECHA_CREO : 07/Julio/2011 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *********************************************************************************/
    public class Cat_Pre_Grupos
    {
        public const String Tabla_Cat_Pre_Grupos = "CAT_PRE_GRUPOS";
        public const String Campo_Grupo_ID = "GRUPO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Rama_ID = "RAMA_ID";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Ramas
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_RAMAS
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno
    ///FECHA_CREO : 07/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Pre_Ramas
    {
        public const String Tabla_Cat_Pre_Ramas = "CAT_PRE_RAMAS";
        public const String Campo_Rama_ID = "RAMA_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Otros_Pagos
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_OTROS_PAGOS
    ///PARÁMETROS :     
    ///CREO       : José Alfredo García Pichardo
    ///FECHA_CREO : 14/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Pre_Otros_Pagos
    {
        public const String Tabla_Cat_Pre_Otros_Pagos = "CAT_PRE_OTROS_PAGOS";
        public const String Campo_Pago_ID = "PAGO_ID";
        public const String Campo_Concepto = "CONCEPTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Motivos
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_MOTIVOS_CANCELA_RECIBO
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno
    ///FECHA_CREO : 27/Julio/2011 
    ///MODIFICO          :    
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Pre_Motivos
    {
        public const String Tabla_Cat_Pre_Motivos = "CAT_PRE_MOTIVOS_CANCELA_RECIBO";
        public const String Campo_Motivo_ID = "MOTIVO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Detalles_Convenios_Predial
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_DET_CONV_PREDIAL
    /// PARÁMETROS :     
    /// CREO       			: Miguel Angel Bedolla Moreno
    /// FECHA_CREO 			: 22/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Detalles_Convenios_Predial
    {
        public const String Tabla_Ope_Pre_Detalles_Convenios_Predial = "OPE_PRE_DET_CONV_PREDIAL";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_Recargos_Ordinarios = "RECARGOS_ORDINARIOS";
        public const String Campo_Recargos_Moratorios = "RECARGOS_MORATORIOS";
        public const String Campo_Monto_Impuesto = "MONTO_IMPUESTO";
        public const String Campo_Monto_Honorarios = "MONTO_HONORARIOS";
        public const String Campo_Periodo = "PERIODO";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_No_Reestructura = "NO_REESTRUCTURA";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Desglose_Detalle_Convenios_Predial
    /// DESCRIPCÓN: Clase que contiene los campos de la tabla OPE_PRE_DES_DET_CONV_PREDIAL
    /// PARÁMETROS :     
    /// CREO       : Roberto González Oseguera
    /// FECHA_CREO : 26-oct-2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Ope_Pre_Desglose_Detalle_Convenios_Predial
    {
        public const String Tabla_Ope_Pre_Des_Det_Conv_Predial = "OPE_PRE_DES_DET_CONV_PREDIAL";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Bimestre_1 = "BIMESTRE_1";
        public const String Campo_Bimestre_2 = "BIMESTRE_2";
        public const String Campo_Bimestre_3 = "BIMESTRE_3";
        public const String Campo_Bimestre_4 = "BIMESTRE_4";
        public const String Campo_Bimestre_5 = "BIMESTRE_5";
        public const String Campo_Bimestre_6 = "BIMESTRE_6";
        public const String Campo_No_Reestructura = "NO_REESTRUCTURA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Estados
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_ESTADOS
    ///PARÁMETROS :     
    ///CREO       : José Alfredo García Pichardo
    ///FECHA_CREO : 20/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Pre_Estados
    {
        public const String Tabla_Cat_Pre_Estados = "CAT_PRE_ESTADOS";
        public const String Campo_Estado_ID = "ESTADO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Ciudades
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CIUDADES
    ///PARÁMETROS :     
    ///CREO       : José Alfredo García Pichardo
    ///FECHA_CREO : 21/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Pre_Ciudades
    {
        public const String Tabla_Cat_Pre_Ciudades = "CAT_PRE_CIUDADES";
        public const String Campo_Ciudad_ID = "CIUDAD_ID";
        public const String Campo_Estado_ID = "ESTADO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Impuestos_Traslado_Dominio
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_TASAS_TRASLADO
    ///PARÁMETROS :     
    ///CREO       : José Alfredo García Pichardo
    ///FECHA_CREO : 21/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Pre_Impuestos_Traslado_Dominio
    {
        public const String Tabla_Cat_Pre_Impuestos_Traslado_Dominio = "CAT_PRE_TASAS_TRASLADO";
        public const String Campo_Tasa_ID = "TASA_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Valor_Inicial = "VALOR_INICIAL";
        public const String Campo_Valor_Final = "VALOR_FINAL";
        public const String Campo_Tasa = "TASA";
        public const String Campo_Deducible_Uno = "DEDUCIBLE_UNO";
        public const String Campo_Deducible_Dos = "DEDUCIBLE_DOS";
        public const String Campo_Deducible_Tres = "DEDUCIBLE_TRES";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Adeudos_Predial
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_ADEUDOS_PREDIAL
    /// PARÁMETROS :     
    /// CREO       			: Roberto González Oseguera
    /// FECHA_CREO 			: 22-jul-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Adeudos_Predial
    {
        public const String Tabla_Ope_Pre_Adeudos_Predial = "OPE_PRE_ADEUDOS_PREDIAL";
        public const String Campo_No_Adeudo = "NO_ADEUDO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Cuota_Anual = "CUOTA_ANUAL";
        public const String Campo_Bimestre_1 = "BIMESTRE_1";
        public const String Campo_Bimestre_2 = "BIMESTRE_2";
        public const String Campo_Bimestre_3 = "BIMESTRE_3";
        public const String Campo_Bimestre_4 = "BIMESTRE_4";
        public const String Campo_Bimestre_5 = "BIMESTRE_5";
        public const String Campo_Bimestre_6 = "BIMESTRE_6";
        public const String Campo_Pago_Bimestre_1 = "PAGO_BIMESTRE_1";
        public const String Campo_Pago_Bimestre_2 = "PAGO_BIMESTRE_2";
        public const String Campo_Pago_Bimestre_3 = "PAGO_BIMESTRE_3";
        public const String Campo_Pago_Bimestre_4 = "PAGO_BIMESTRE_4";
        public const String Campo_Pago_Bimestre_5 = "PAGO_BIMESTRE_5";
        public const String Campo_Pago_Bimestre_6 = "PAGO_BIMESTRE_6";
        public const String Campo_Monto_Por_Pagar = "MONTO_POR_PAGAR";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_No_Adeudo_Origen = "NO_ADEUDO_ORIGEN";
        public const String Campo_No_Descuento = "NO_DESCUENTO";
        public const String Campo_Descuento_ID = "DESCUENTO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Tmp_Pre_Adeudos_Predial
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla TMP_PRE_ADEUDOS_PREDIAL 
    /// 	            Adeudos temporales
    /// PARÁMETROS :     
    /// CREO       			: Roberto González Oseguera
    /// FECHA_CREO 			: 22-jul-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Tmp_Pre_Adeudos_Predial
    {
        public const String Tabla_Tmp_Pre_Adeudos_Predial = "TMP_PRE_ADEUDOS_PREDIAL";
        public const String Campo_No_Adeudo = "NO_ADEUDO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Cuota_Anual = "CUOTA_ANUAL";
        public const String Campo_Bimestre_1 = "BIMESTRE_1";
        public const String Campo_Bimestre_2 = "BIMESTRE_2";
        public const String Campo_Bimestre_3 = "BIMESTRE_3";
        public const String Campo_Bimestre_4 = "BIMESTRE_4";
        public const String Campo_Bimestre_5 = "BIMESTRE_5";
        public const String Campo_Bimestre_6 = "BIMESTRE_6";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Claves_Ingreso
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CLAVES_INGRESO
    ///PARÁMETROS :     
    ///CREO       : José Alfredo García Pichardo
    ///FECHA_CREO : 21/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Pre_Claves_Ingreso
    {
        public const String Tabla_Cat_Pre_Claves_Igreso = "CAT_PRE_CLAVES_INGRESO";
        public const String Campo_Clave_Ingreso_ID = "CLAVE_INGRESO_ID";
        public const String Campo_Grupo_ID = "GRUPO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Fundamento = "FUNDAMENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Claves_Ingreso_Det
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CLAVES_INGRESO_DET
    ///PARÁMETROS :     
    ///CREO       : José Alfredo García Pichardo
    ///FECHA_CREO : 26/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Pre_Claves_Ingreso_Det
    {
        public const String Tabla_Cat_Pre_Claves_Igreso_Det = "CAT_PRE_CLAVES_INGRESO_DET";
        public const String Campo_Detalle_ID = "DETALLE_ID";
        public const String Campo_Clave_Ingreso_ID = "CLAVE_INGRESO_ID";
        public const String Campo_Movimiento_ID = "MOVIMIENTO_ID";
        public const String Campo_Pago_ID = "PAGO_ID";
        public const String Campo_Documento_ID = "CONSTANCIA_ID";
        public const String Campo_Gasto_ID = "GASTO_ID";
        public const String Campo_Puesto = "PUESTO";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Tipo_Predial_Traslado = "TIPO_PREDIAL_TRASLADO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Claves_Ing_Costos
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CLAVES_ING_COSTOS
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno
    ///FECHA_CREO : 17/Noviembre/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Pre_Claves_Ing_Costos
    {
        public const String Tabla_Cat_Pre_Claves_Ing_Costos = "CAT_PRE_CLAVES_ING_COSTOS";
        public const String Campo_Costo_Clave_ID = "COSTO_CLAVE_ID";
        public const String Campo_Clave_Ingreso_ID = "CLAVE_INGRESO_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Costo = "COSTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Adeudos_Folio
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_ADEUDOS_FOLIO
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 25/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Adeudos_Folio
    {
        public const String Tabla_Ope_Pre_Adeudos_Folio = "OPE_PRE_ADEUDOS_FOLIO";
        public const String Campo_No_Adeudo = "NO_ADEUDO";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Concepto = "CONCEPTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_No_Descuento = "NO_DESCUENTO";
        public const String Campo_Desc_Multa = "DESC_MULTA";
        public const String Campo_Desc_Recargo = "DESC_RECARGO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Convenios_Derechos_Supervision
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_CONVENIOS_DER_SUP
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 25/Julio/2011
    /// MODIFICO          :     Miguel Angel Bedolla
    /// FECHA_MODIFICO    :     08/Septiembre/2011
    /// CAUSA_MODIFICACIÓN:     Se necesitan más campos para consultar de la tabla
    ///*******************************************************************************
    public class Ope_Pre_Convenios_Derechos_Supervision
    {
        public const String Tabla_Ope_Pre_Convenios_Derechos_Supervision = "OPE_PRE_CONVENIOS_DER_SUP";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Propietario_ID = "PROPIETARIO_ID";
        public const String Campo_Realizo = "REALIZO";
        public const String Campo_No_Reestructura = "NO_REESTRUCTURA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Estatus_Cancelacion_Cuenta = "ESTATUS_CANCELACION_CUENTA";
        public const String Campo_Solicitante = "SOLICITANTE";
        public const String Campo_RFC = "RFC";
        public const String Campo_Numero_Parcialidades = "NUMERO_PARCIALIDADES";
        public const String Campo_Periodicidad_Pago = "PERIODICIDAD_PAGO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Descuento_Recargos_Ordinarios = "DESCUENTO_RECARGOS_ORDINARIOS";
        public const String Campo_Descuento_Recargos_Moratorios = "DESCUENTO_RECARGOS_MORATORIOS";
        public const String Campo_Descuento_Multas = "DESCUENTO_MULTAS";
        public const String Campo_Total_Adeudo = "TOTAL_ADEUDO";
        public const String Campo_Total_Descuento = "TOTAL_DESCUENTO";
        public const String Campo_Sub_Total = "SUB_TOTAL";
        public const String Campo_Porcentaje_Anticipo = "PORCENTAJE_ANTICIPO";
        public const String Campo_Total_Anticipo = "TOTAL_ANTICIPO";
        public const String Campo_Total_Convenio = "TOTAL_CONVENIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_No_Impuesto_Dereho_Supervisio = "NO_IMPUESTO_DERECHO_SUPERVISIO";
        public const String Campo_Anticipo = "ANTICIPO";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Anticipo_Reestructura = "ANTICIPO_REESTRUCTURA";
        public const String Campo_No_Descuento = "NO_DESCUENTO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Detalles_Convenios_Derechos_Supervision
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_DET_CONV_DER_SUP
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 25/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Detalles_Convenios_Derechos_Supervision
    {
        public const String Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision = "OPE_PRE_DET_CONV_DER_SUP";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_Monto_Multas = "MONTO_MULTAS";
        public const String Campo_Recargos_Ordinarios = "RECARGOS_ORDINARIOS";
        public const String Campo_Recargos_Moratorios = "RECARGOS_MORATORIOS";
        public const String Campo_Monto_Impuesto = "MONTO_IMPUESTO";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_No_Reestructura = "NO_REESTRUCTURA";
        public const String Campo_No_Pago_Aplicado = "NO_PAGO_APLICADO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Convenios_Fraccionamientos
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_CONVENIOS_FRACC
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 25/Julio/2011
    /// MODIFICO          :     Miguel Angel Bedolla
    /// FECHA_MODIFICO    :     08/Septiembre/2011
    /// CAUSA_MODIFICACIÓN:     Se necesitan más campos para consultar de la tabla
    ///*******************************************************************************
    public class Ope_Pre_Convenios_Fraccionamientos
    {
        public const String Tabla_Ope_Pre_Convenios_Fraccionamientos = "OPE_PRE_CONVENIOS_FRACC";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Propietario_ID = "PROPIETARIO_ID";
        public const String Campo_Realizo = "REALIZO";
        public const String Campo_No_Reestructura = "NO_REESTRUCTURA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Estatus_Cancelacion_Cuenta = "ESTATUS_CANCELACION_CUENTA";
        public const String Campo_Solicitante = "SOLICITANTE";
        public const String Campo_RFC = "RFC";
        public const String Campo_Numero_Parcialidades = "NUMERO_PARCIALIDADES";
        public const String Campo_Periodicidad_Pago = "PERIODICIDAD_PAGO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Descuento_Recargos_Ordinarios = "DESCUENTO_RECARGOS_ORDINARIOS";
        public const String Campo_Descuento_Recargos_Moratorios = "DESCUENTO_RECARGOS_MORATORIOS";
        public const String Campo_Descuento_Multas = "DESCUENTO_MULTAS";
        public const String Campo_Total_Adeudo = "TOTAL_ADEUDO";
        public const String Campo_Total_Descuento = "TOTAL_DESCUENTO";
        public const String Campo_Sub_Total = "SUB_TOTAL";
        public const String Campo_Porcentaje_Anticipo = "PORCENTAJE_ANTICIPO";
        public const String Campo_Total_Anticipo = "TOTAL_ANTICIPO";
        public const String Campo_Total_Convenio = "TOTAL_CONVENIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_No_Impuesto_Fraccionamiento = "NO_IMPUESTO_FRACCIONAMIENTO";
        public const String Campo_Anticipo = "ANTICIPO";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Anticipo_Reestructura = "ANTICIPO_REESTRUCTURA";
        public const String Campo_No_Descuento = "NO_DESCUENTO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Detalles_Convenios_Fraccionamientos
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_DET_CONVENIOS_FRACC
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 25/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Detalles_Convenios_Fraccionamientos
    {
        public const String Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos = "OPE_PRE_DET_CONVENIOS_FRACC";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_Monto_Multas = "MONTO_MULTAS";
        public const String Campo_Recargos_Ordinarios = "RECARGOS_ORDINARIOS";
        public const String Campo_Recargos_Moratorios = "RECARGOS_MORATORIOS";
        public const String Campo_Monto_Impuesto = "MONTO_IMPUESTO";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_No_Reestructura = "NO_REESTRUCTURA";
        public const String Campo_No_Pago_Aplicado = "NO_PAGO_APLICADO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Convenios_Traslados_Dominio
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_CONVENIOS_FRACC
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 25/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Convenios_Traslados_Dominio
    {
        public const String Tabla_Ope_Pre_Convenios_Traslados_Dominio = "OPE_PRE_CONVENIOS_TRAS_DOM";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_No_Contrarecibo = "NO_CONTRARECIBO";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Propietario_ID = "PROPIETARIO_ID";
        public const String Campo_Realizo = "REALIZO";
        public const String Campo_No_Reestructura = "NO_REESTRUCTURA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Estatus_Cancelacion_Cuenta = "ESTATUS_CANCELACION_CUENTA";
        public const String Campo_Anticipo = "ANTICIPO";
        public const String Campo_Anticipo_Reestructura = "ANTICIPO_REESTRUCTURA";
        public const String Campo_Solicitante = "SOLICITANTE";
        public const String Campo_RFC = "RFC";
        public const String Campo_Numero_Parcialidades = "NUMERO_PARCIALIDADES";
        public const String Campo_Periodicidad_Pago = "PERIODICIDAD_PAGO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Descuento_Recargos_Ordinarios = "DESCUENTO_RECARGOS_ORDINARIOS";
        public const String Campo_Descuento_Recargos_Moratorios = "DESCUENTO_RECARGOS_MORATORIOS";
        public const String Campo_Descuento_Multas = "DESCUENTO_MULTAS";
        public const String Campo_Total_Adeudo = "TOTAL_ADEUDO";
        public const String Campo_Total_Descuento = "TOTAL_DESCUENTO";
        public const String Campo_Sub_Total = "SUB_TOTAL";
        public const String Campo_Porcentaje_Anticipo = "PORCENTAJE_ANTICIPO";
        public const String Campo_Total_Anticipo = "TOTAL_ANTICIPO";
        public const String Campo_Total_Convenio = "TOTAL_CONVENIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_No_Calculo = "NO_CALCULO";
        public const String Campo_No_Descuento = "NO_DESCUENTO";
        public const String Campo_Anio = "ANIO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE	: Ope_Pre_Detalles_Convenios_Traslados_Dominio
    /// DESCRIPCIÓN			: Clase que contiene los campos de la tabla OPE_PRE_DET_CONV_TRAS_DOM
    /// PARÁMETROS :     
    /// CREO       			: Antonion Salvador Benavides Guardado
    /// FECHA_CREO 			: 25/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Detalles_Convenios_Traslados_Dominio
    {
        public const String Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio = "OPE_PRE_DET_CONV_TRAS_DOM";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Monto_Impuesto = "MONTO_IMPUESTO";
        public const String Campo_Recargos_Moratorios = "RECARGOS_MORATORIOS";
        public const String Campo_Recargos_Ordinarios = "RECARGOS_ORDINARIOS";
        public const String Campo_Monto_Multas = "MONTO_MULTAS";
        public const String Campo_No_Reestructura = "NO_REESTRUCTURA";
        public const String Campo_No_Pago_Aplicado = "NO_PAGO_APLICADO";
        public const String Campo_Monto_Impuesto_Division = "MONTO_IMPUESTO_DIVISION";
        public const String Campo_Monto_Constancia = "MONTO_CONSTANCIA";

    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Ope_Pre_Recolecciones
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAJ_RECOLECCIONES
    ///PARÁMETROS :     
    ///CREO       : José Alfredo García Pichardo
    ///FECHA_CREO : 01/Agosto/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Ope_Pre_Recolecciones
    {
        public const String Tabla_Ope_Pre_Recolecciones = "OPE_CAJ_RECOLECCIONES";
        public const String Campo_Recoleccion_ID = "NO_RECOLECCION";
        public const String Campo_Caja_ID = "CAJA_ID";
        public const String Campo_Cajero_ID = "EMPLEADO_ID";
        public const String Campo_Num_Recoleccion = "NUM_RECOLECCION";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Mnt_Recoleccion = "MONTO_RECOLECTADO";
        public const String Campo_No_Turno = "NO_TURNO";
        public const String Campo_Recibe_Efectivo = "RECIBE_EFECTIVO";
        public const String Campo_Conteo_Tarjeta = "CONTEO_TARJETA";
        public const String Campo_Monto_Tarjeta = "MONTO_TARJETA";
        public const String Campo_Conteo_Cheque = "CONTEO_CHEQUE";
        public const String Campo_Monto_Cheque = "MONTO_CHEQUE";
        public const String Campo_Conteo_Transferencia = "CONTEO_TRANSFERENCIA";
        public const String Campo_Monto_Transferencia = "MONTO_TRANSFERENCIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Ope_Caj_Recolecciones_Detalles
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAJ_RECOLECCIONES_DET
    ///PARÁMETROS :     
    ///CREO       : Yazmin A Delgado Gómez
    ///FECHA_CREO : 13/Octubre/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///************************************************w*********************************
    public class Ope_Caj_Recolecciones_Detalles
    {
        public const String Tabla_Ope_Caj_Recolecciones_Detalles = "OPE_CAJ_RECOLECCIONES_DET";
        public const String Campo_No_Recoleccion = "NO_RECOLECCION";
        public const String Campo_Caja_ID = "CAJA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_No_Turno = "NO_TURNO";
        public const String Campo_Billete_1000 = "BIILLETE_1000";
        public const String Campo_Billete_500 = "BIILLETE_500";
        public const String Campo_Billete_200 = "BIILLETE_200";
        public const String Campo_Billete_100 = "BIILLETE_100";
        public const String Campo_Billete_50 = "BIILLETE_50";
        public const String Campo_Billete_20 = "BIILLETE_20";
        public const String Campo_Moneda_20 = "MONEDA_20";
        public const String Campo_Moneda_10 = "MONEDA_10";
        public const String Campo_Moneda_5 = "MONEDA_5";
        public const String Campo_Moneda_2 = "MONEDA_2";
        public const String Campo_Moneda_1 = "MONEDA_1";
        public const String Campo_Moneda_050 = "MONEDA_050";
        public const String Campo_Moneda_020 = "MONEDA_020";
        public const String Campo_Moneda_010 = "MONEDA_010";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Ope_Pre_Modificacion_Folio
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_MODIFICACION_FOLIO
    ///PARÁMETROS :     
    ///CREO       : José Alfredo García Pichardo
    ///FECHA_CREO : 03/Agosto/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Ope_Pre_Modifica_Folio
    {
        public const String Tabla_Ope_Pre_Modifica_Folio = "OPE_CAJ_MODIFICACION_FOLIO";
        public const String Campo_Modifica_ID = "NO_MODIFICACION";
        public const String Campo_No_Pago_ID = "NO_PAGO";
        public const String Campo_Folio_Actual = "FOLIO_ANTERIOR";
        public const String Campo_Folio_Nuevo = "FOLIO_NUEVO";
        public const String Campo_Motivo = "MOTIVO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Ope_Caj_Turnos
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAJ_TURNOS
    ///PARÁMETROS :     
    ///CREO       : Miguel Angel Bedolla Moreno.
    ///FECHA_CREO : 30/Julio/2011 
    ///MODIFICO          : Yazmin A Delgado Gómez
    ///FECHA_MODIFICO    : 10-Octubre-2011
    ///CAUSA_MODIFICACIÓN: Porque no estaba la tabla completa de acuedo a los campos
    ///                    que se tienen en la base de datos
    ///*******************************************************************************
    public class Ope_Caj_Turnos
    {
        public const String Tabla_Ope_Caj_Turnos = "OPE_CAJ_TURNOS";
        public const String Campo_No_Turno = "NO_TURNO";
        public const String Campo_No_Turno_Dia = "NO_TURNO_DIA";
        public const String Campo_Caja_Id = "CAJA_ID";

        public const String Campo_Caja_ID = "CAJA_ID";

        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Recibo_Inicial = "RECIBO_INICIAL";
        public const String Campo_Recibo_Final = "RECIBO_FINAL";
        public const String Campo_Contador_Recibo = "CONTADOR_RECIBO";
        public const String Campo_Fecha_Turno = "FECHA_TURNO";
        public const String Campo_Aplicacion_Pago = "APLICACION_PAGO";
        public const String Campo_Fondo_Inicial = "FONDO_INICIAL";
        public const String Campo_Hora_Apertura = "HORA_APERTURA";
        public const String Campo_Fecha_Cierre = "FECHA_CIERRE";
        public const String Campo_Hora_Cierre = "HORA_CIERRE";
        public const String Campo_Total_Bancos = "TOTAL_BANCOS";
        public const String Campo_Total_Cheques = "TOTAL_CHEQUES";
        public const String Campo_Total_Transferencias = "TOTAL_TRANSFERENCIAS";
        public const String Campo_Total_Recolectado = "TOTAL_RECOLECTADO";
        public const String Campo_Total_Efectivo_Sistema = "TOTAL_EFECTIVO_SISTEMA";
        public const String Campo_Total_Efectivo_Caja = "TOTAL_EFECTIVO_CAJA";
        public const String Campo_Faltante = "FALTANTE";
        public const String Campo_Sobrante = "SOBRANTE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Reapertura = "FECHA_REAPERTURA";
        public const String Campo_Hora_Reapertura = "HORA_REAPERTURA";
        public const String Campo_Conteo_Bancos = "CONTEO_BANCOS";
        public const String Campo_Conteo_Cheques = "CONTEO_CHEQUES";
        public const String Campo_Conteo_Transferencias = "CONTEO_TRANSFERENCIAS";
        public const String Campo_Total_Recolectado_Bancos = "TOTAL_RECOLEC_BANCOS";
        public const String Campo_Total_Recolectado_Cheques = "TOTAL_RECOLEC_CHEQUES";
        public const String Campo_Total_Recolectado_Transferencias = "TOTAL_RECOLEC_TRANSFERENCIAS";
        public const String Campo_Conteo_Recolectado_Bancos = "TOTAL_RECOLEC_CONTEO_BANCOS";
        public const String Campo_Conteo_Recolectado_Cheques = "TOTAL_RECOLEC_CONTEO_CHEQUES";
        public const String Campo_Conteo_Recolectado_Transferencias = "TOTAL_RECOLEC_CONTEO_TRANSFER";
        public const String Campo_ReApertura_Empleado_ID_Autorizo = "REAPERTURA_EMP_ID_AUTORIZO";
        public const String Campo_ReApertura_Nombre_Autorizo = "REAPERTURA_NOMBRE_AUTORIZO";
        public const String Campo_ReApertura_Observaciones_Autorizo = "REAPERTURA_OBSERVA_AUTORIZO";
        public const String Campo_ReApertura_Fecha_Autorizo = "REAPERTURA_FECHA_AUTORIZO";
        public const String Campo_ReApertura_Autorizo = "REAPERTURA_AUTORIZADA";
        public const String Campo_Nombre_Recibe = "RECIBE_EFECTIVO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Ope_Caj_Turnos_Detalles
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAJ_TURNOS_DETALLES
    ///PARÁMETROS :     
    ///CREO       : Yazmin A Delgado Gómez
    ///FECHA_CREO : 23-Octubre-2011
    ///MODIFICO          : 
    ///FECHA_MODIFICO    : 
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Caj_Turnos_Detalles
    {
        public const String Tabla_Ope_Caj_Turnos_Detalles = "OPE_CAJ_TURNOS_DETALLES";
        public const String Campo_No_Turno = "NO_TURNO";
        public const String Campo_Billete_1000 = "BIILLETE_1000";
        public const String Campo_Billete_500 = "BIILLETE_500";
        public const String Campo_Billete_200 = "BIILLETE_200";
        public const String Campo_Billete_100 = "BIILLETE_100";
        public const String Campo_Billete_50 = "BIILLETE_50";
        public const String Campo_Billete_20 = "BIILLETE_20";
        public const String Campo_Moneda_20 = "MONEDA_20";
        public const String Campo_Moneda_10 = "MONEDA_10";
        public const String Campo_Moneda_5 = "MONEDA_5";
        public const String Campo_Moneda_2 = "MONEDA_2";
        public const String Campo_Moneda_1 = "MONEDA_1";
        public const String Campo_Moneda_050 = "MONEDA_050";
        public const String Campo_Moneda_020 = "MONEDA_020";
        public const String Campo_Moneda_010 = "MONEDA_010";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Ope_Caj_Turnos_Dia
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAJ_TURNOS_DIA
    ///PARÁMETROS :     
    ///CREO       : Ismael Prieto Sánchez
    ///FECHA_CREO : 19/Octubre/2011 
    ///MODIFICO          : Yazmin A Delgado Gómez
    ///FECHA_MODIFICO    : 25/Octubre/2011
    ///CAUSA_MODIFICACIÓN: Se agregeo el campo Fecha_Cierre
    ///*******************************************************************************
    public class Ope_Caj_Turnos_Dia
    {
        public const String Tabla_Ope_Caj_Turnos_Dia = "OPE_CAJ_TURNOS_DIA";
        public const String Campo_No_Turno = "NO_TURNO_DIA";
        public const String Campo_Fecha_Turno = "FECHA_TURNO";
        public const String Campo_Hora_Apertura = "HORA_APERTURA";
        public const String Campo_Hora_Cierre = "HORA_CIERRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Reapertura = "USUARIO_REAPERTURA";
        public const String Campo_Fecha_Reapertura = "FECHA_REAPERTURA";
        public const String Campo_Fecha_Reapertura_Cierre = "FECHA_REAPERTURA_CIERRE_DIA";
        public const String Campo_Empleado_Reabrio_Turno = "EMPLEADO_REABRIO_TURNO";
        public const String Campo_Fecha_Cierre = "FECHA_CIERRE";
        public const String Campo_Autorizo_Reapertura = "AUTORIZO_REAPERTURA";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Recargos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_RECARGOS
    /// PARÁMETROS :     
    /// CREO       : Roberto González Oseguera
    /// FECHA_CREO : 08-ago-2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Ope_Pre_Recargos
    {
        public const String Tabla_Ope_Pre_Recargos = "OPE_PRE_RECARGOS";
        public const String Campo_No_Recargo = "NO_RECARGO";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Anio_Inicial = "ANIO_INICIAL";
        public const String Campo_Bimestre_Inicial = "BIMESTRE_INICIAL";
        public const String Campo_Bimestre_Final = "BIMESTRE_FINAL";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Rezago = "REZAGO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Calculo_Imp_Traslado
    /// DESCRIPCÓN: Clase que contiene los campos de la tabla OPE_PRE_CALCULO_IMP_TRASLADO
    /// PARÁMETROS :     
    /// CREO       : Roberto González Oseguera
    /// FECHA_CREO : 11-ago-2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Ope_Pre_Calculo_Imp_Traslado
    {
        public const String Tabla_Ope_Pre_Calculo_Imp_Traslado = "OPE_PRE_CALCULO_IMP_TRASLADO";
        public const String Campo_No_Calculo = "NO_CALCULO";
        public const String Campo_Anio_Calculo = "ANIO_CALCULO";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Multa_ID = "MULTA_ID";
        public const String Campo_No_Orden_Variacion = "NO_ORDEN_VARIACION";
        public const String Campo_No_Adeudo = "NO_ADEUDO";
        public const String Campo_Anio_Orden = "ANIO_ORDEN";
        public const String Campo_Predio_Colindante = "PREDIO_COLINDANTE";
        public const String Campo_Base_Impuesto = "BASE_IMPUESTO";
        public const String Campo_Minimo_Elevado_Anio = "MINIMO_ELEVADO_ANIO";
        public const String Campo_Tasa_ID = "TASA_ID";
        public const String Campo_Fecha_Escritura = "FECHA_ESCRITURA";
        public const String Campo_Costo_Constancia = "COSTO_CONSTANCIA";
        public const String Campo_Base_Impuesto_Division = "BASE_IMPUESTO_DIVISION";
        public const String Campo_Impuesto_Division_Lot_Id = "IMPUESTO_DIVISION_LOT_ID";
        public const String Campo_Monto_Traslado = "MONTO_TRASLADO";
        public const String Campo_Monto_Division = "MONTO_DIVISION";
        public const String Campo_Monto_Multa = "MONTO_MULTA";
        public const String Campo_Monto_Recargos = "MONTO_RECARGOS";
        public const String Campo_Monto_Total_Pagar = "MONTO_TOTAL_PAGAR";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Fundamento = "FUNDAMENTO";
        public const String Campo_Realizo_Calculo = "REALIZO_CALCULO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Calc_Imp_Tras_Det
    /// DESCRIPCÓN: Clase que contiene los campos de la tabla OPE_PRE_CALC_IMP_TRAS_DET
    /// PARÁMETROS :     
    /// CREO       : Roberto González Oseguera
    /// FECHA_CREO : 11-ago-2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Ope_Pre_Calc_Imp_Tras_Det
    {
        public const String Tabla_Ope_Pre_Ope_Pre_Calc_Imp_Tras_Det = "OPE_PRE_DET_CALC_IMP_TRAS";
        public const String Campo_No_Detalle_Calculo = "NO_DETALLE_CALCULO";
        public const String Campo_No_Calculo = "NO_CALCULO";
        public const String Campo_Anio_Calculo = "ANIO_CALCULO";
        public const String Campo_Realizo_Observacion = "REALIZO_OBSERVACION";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Fecha_Hora = "FECHA_HORA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Descuento_Traslado
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_DESCUENTO_TRASLADO
    /// PARÁMETROS :     
    /// CREO       : José Alfredo García Pichardo
    /// FECHA_CREO : 12/Agosto/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Ope_Pre_Descuento_Traslado
    {
        public const String Tabla_Ope_Pre_Descuento_Traslado = "OPE_PRE_DESCUENTO_TRASLADO";
        public const String Campo_No_Descuento = "NO_DESCUENTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Inicial = "FECHA";
        public const String Campo_Desc_Multa = "DESC_MULTA";
        public const String Campo_Desc_Recargo = "DESC_RECARGO";
        public const String Campo_Total_Por_Pagar = "TOTAL_POR_PAGAR";
        public const String Campo_Realizo = "REALIZO";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Fundamento_Legal = "FUNDAMENTO_LEGAL";
        public const String Campo_No_Calculo = "NO_CALCULO";
        public const String Campo_Anio_Calculo = "ANIO_CALCULO";
        public const String Campo_Referencia = "REFERENCIA";
        public const String Campo_No_Adeudo = "NO_ADEUDO";
        public const String Campo_Monto_Recargos = "MONTO_RECARGOS";
        public const String Campo_Monto_Multas = "MONTO_MULTAS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Descuento_Der_Sup
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_DESCUENTO_DER_SUP
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 24/Octubre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Ope_Pre_Descuento_Der_Sup
    {
        public const String Tabla_Ope_Pre_Descuento_Der_Sup = "OPE_PRE_DESCUENTO_DER_SUP";
        public const String Campo_No_Descuento = "NO_DESCUENTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Inicial = "FECHA";
        public const String Campo_Desc_Multa = "DESC_MULTA";
        public const String Campo_Desc_Recargo = "DESC_RECARGO";
        public const String Campo_Total_Por_Pagar = "TOTAL_POR_PAGAR";
        public const String Campo_Realizo = "REALIZO";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Fundamento_Legal = "FUNDAMENTO_LEGAL";
        public const String Campo_No_Impuesto_Derecho_Supervision = "NO_IMPUESTO_DERECHO_SUPERVISIO";
        public const String Campo_Referencia = "REFERENCIA";
        public const String Campo_No_Adeudo = "NO_ADEUDO";
        public const String Campo_Monto_Recargos = "MONTO_RECARGOS";
        public const String Campo_Monto_Multas = "MONTO_MULTAS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pre_Descuento_Der_Sup
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_DESCUENTO_DER_SUP
    /// PARÁMETROS :     
    /// CREO       : Miguel Angel Bedolla Moreno
    /// FECHA_CREO : 24/Octubre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Ope_Pre_Descuento_Fracc
    {
        public const String Tabla_Ope_Pre_Descuento_Fracc = "OPE_PRE_DESCUENTO_FRACC";
        public const String Campo_No_Descuento = "NO_DESCUENTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Inicial = "FECHA";
        public const String Campo_Desc_Multa = "DESC_MULTA";
        public const String Campo_Desc_Recargo = "DESC_RECARGO";
        public const String Campo_Total_Por_Pagar = "TOTAL_POR_PAGAR";
        public const String Campo_Realizo = "REALIZO";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Fundamento_Legal = "FUNDAMENTO_LEGAL";
        public const String Campo_No_Impuesto_fraccionamiento = "NO_IMPUESTO_FRACCIONAMIENTO";
        public const String Campo_Referencia = "REFERENCIA";
        public const String Campo_No_Adeudo = "NO_ADEUDO";
        public const String Campo_Monto_Recargos = "MONTO_RECARGOS";
        public const String Campo_Monto_Multas = "MONTO_MULTAS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Caj_Recolecciones
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_CAJ_RECOLECCIONES
    /// PARAMETROS :
    /// CREO       : Yazmin A Delgado Gómeza
    /// FECHA_CREO : 16/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Caj_Recolecciones
    {
        public const String Tabla_Ope_Caj_Recolecciones = "OPE_CAJ_RECOLECCIONES";
        public const String Campo_No_Recoleccion = "NO_RECOLECCION";
        public const String Campo_Caja_ID = "CAJA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_No_Turno = "NO_TURNO";
        public const String Campo_Num_Recoleccion = "NUM_RECOLECCION";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Monto_Recolectado = "MONTO_RECOLECTADO";
        public const String Campo_Conteo_Tarjeta = "CONTEO_TARJETA";
        public const String Campo_Monto_Tarjeta = "MONTO_TARJETA";
        public const String Campo_Conteo_Cheque = "CONTEO_CHEQUE";
        public const String Campo_Monto_Cheque = "MONTO_CHEQUE";
        public const String Campo_Conteo_Transferencia = "CONTEO_TRANSFERENCIA";
        public const String Campo_Monto_Transferencia = "MONTO_TRANSFERENCIA";
        public const String Campo_Recibe_Efectivo = "RECIBE_EFECTIVO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Caj_Pagos
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_CAJ_PAGOS
    /// PARAMETROS :
    /// CREO       : Yazmin A Delgado Gómez
    /// FECHA_CREO : 16/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Caj_Pagos
    {
        public const String Tabla_Ope_Caj_Pagos = "OPE_CAJ_PAGOS";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_No_Recibo = "NO_RECIBO";
        public const String Campo_No_Operacion = "NO_OPERACION";
        public const String Campo_No_Pasivo = "NO_PASIVO";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Caja_ID = "CAJA_ID";
        public const String Campo_No_Turno = "NO_TURNO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Clave_Banco = "CLAVE_BANCO";
        public const String Campo_Documento = "DOCUMENTO";
        public const String Campo_Motivo_Cancelacion_ID = "MOTIVO_CANCELACION_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Periodo_Corriente = "PERIODO_CORRIENTE";
        public const String Campo_Periodo_Rezago = "PERIODO_REZAGO";
        public const String Campo_Monto_Corriente = "MONTO_CORRIENTE";
        public const String Campo_Monto_Rezago = "MONTO_REZAGO";
        public const String Campo_Monto_Recargos = "MONTO_RECARGOS";
        public const String Campo_Monto_Recargos_Moratorios = "MONTO_RECARGOS_MORATORIOS";
        public const String Campo_Honorarios = "HONORARIOS";
        public const String Campo_Multas = "MULTAS";
        public const String Campo_Gastos_Ejecucion = "GASTOS_EJECUCION";
        public const String Campo_Descuento_Recargos = "DESCUENTO_RECARGOS";
        public const String Campo_Descuento_Moratorios = "DESCUENTO_RECARGOS_MORATORIOS";
        public const String Campo_Descuento_Honorarios = "DESCUENTO_HONORARIOS";
        public const String Campo_Descuento_Pronto_Pago = "DESCUENTO_PRONTO_PAGO";
        public const String Campo_Descuento_Multas = "DESCUENTO_MULTAS";
        public const String Campo_Ajuste_Tarifario = "AJUSTE_TARIFARIO";
        public const String Campo_Total = "TOTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Clave_Ingreso_ID = "CLAVE_INGRESO_ID";
        public const String Campo_Tipo_Pago = "TIPO_PAGO";
        public const String Campo_Fecha_Cancelacion = "FECHA_CANCELACION";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Monto_Impuesto_Division = "MONTO_IMPUESTO_DIVISION";
        public const String Campo_Monto_Constancia = "MONTO_CONSTANCIA";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Caj_Pagos
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_CAJ_PAGOS_ADEUDOS_PRED
    /// PARAMETROS :
    /// CREO       : Ismael Prieto Sánchez
    /// FECHA_CREO : 25/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Caj_Pagos_Adeudos_Predial
    {
        public const String Tabla_Ope_Caj_Pagos_Adeudos_Predial = "OPE_CAJ_PAGOS_ADEUDOS_PRED";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_No_Adeudo = "NO_ADEUDO";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Bimestre = "BIMESTRE";
        public const String Campo_Monto = "MONTO";
        public const String Campo_No_Convenio = "NO_CONVENIO";
        public const String Campo_No_Convenio_Pago = "NO_CONVENIO_PAGO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Caj_Pagos_Detalles
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_CAJ_PAGOS_DETALLES
    /// PARAMETROS :
    /// CREO       : Yazmin A Delgado Gómez
    /// FECHA_CREO : 16/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Caj_Pagos_Detalles
    {
        public const String Tabla_Ope_Caj_Pagos_Detalles = "OPE_CAJ_PAGOS_DETALLES";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_Banco_ID = "BANCO_ID";
        public const String Campo_Forma_Pago = "FORMA_PAGO";
        public const String Campo_No_Transaccion = "NO_TRANSACCION";
        public const String Campo_No_Autorizacion = "NO_AUTORIZACION";
        public const String Campo_Plan_Pago = "PLAN_PAGO";
        public const String Campo_Meses = "MESES";
        public const String Campo_No_Cheque = "NO_CHEQUE";
        public const String Campo_Referencia_Transferencia = "REFERENCIA_TRANSFERENCIA";
        public const String Campo_No_Tarjeta_Bancaria = "NO_TARJETA_BANCARIA";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Consecutivo = "CONSECUTIVO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Caj_Denominaciones
    /// DESCRIPCION: Clase con contiene los datos de la tabla Ope_Caj_Denominaciones
    /// PARAMETROS :
    /// CREO       : Yazmin A Delgado Gómez
    /// FECHA_CREO : 16/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Caj_Denominaciones
    {
        public const String Tabla_Ope_Caj_Denominaciones = "OPE_CAJ_DENOMINACIONES";
        public const String Campo_No_Turno = "NO_TURNO";
        public const String Campo_Caja_ID = "CAJA_ID";
        public const String Campo_Cant_Diez_Cent = "CANT_DIEZ_CENT";
        public const String Campo_Cant_Veinte_Cent = "CANT_VEINTE_CENT";
        public const String Campo_Cant_Cinc_Cent = "CANT_CINC_CENT";
        public const String Campo_Cant_Un_P = "CANT_UN_P";
        public const String Campo_Cant_Dos_P = "CANT_DOS_P";
        public const String Campo_Cant_Cinco_P = "CANT_CINCO_P";
        public const String Campo_Cant_Diez_P = "CANT_DIEZ_P";
        public const String Campo_Cant_Veinte_P = "CANT_VEINTE_P";
        public const String Campo_Cant_Cincuenta_P = "CANT_CINCUENTA_P";
        public const String Campo_Cant_Cien_P = "CANT_CIEN_P";
        public const String Campo_Cant_Doscientos_P = "CANT_DOSCIENTOS_P";
        public const String Campo_Cant_Quinientos_P = "CANT_QUINIENTOS_P";
        public const String Campo_Cant_Mil__P = "CANT_MIL__P";
        public const String Campo_Monto_Total = "MONTO_TOTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Ing_Pasivo
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_ING_PASIVO
    /// PARAMETROS :
    /// CREO       : Yazmin A Delgado Gómez
    /// FECHA_CREO : 22/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Ing_Pasivo
    {
        public const String Tabla_Ope_Ing_Pasivo = "OPE_ING_PASIVO";
        public const String Campo_Pasivo_ID = "NO_PASIVO";
        public const String Campo_No_Pasivo = "NO_PASIVO";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_Referencia = "REFERENCIA";
        public const String Campo_Clave_Ingreso_ID = "CLAVE_INGRESO_ID";
        public const String Campo_SubConcepto_Ing_ID = "SUBCONCEPTO_ING_ID";
        public const String Campo_Concepto_Ing_ID = "CONCEPTO_ING_ID";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Origen = "ORIGEN";
        public const String Campo_Fecha_Ingreso = "FECHA_INGRESO";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Recargos = "RECARGOS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_No_Recibo = "NO_RECIBO";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Contribuyente = "CONTRIBUYENTE";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Fecha_Pago = "FECHA_PAGO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Concepto_Ing_Id = "CONCEPTO_ING_ID";
        public const String Campo_No_Concepto = "NO_CONCEPTO";
    }


    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Multas_Derechos_Supervision
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_MUL_DER_SUPERVISION
    ///PARÁMETROS :     
    ///CREO       : José Alfredo García Pichardo
    ///FECHA_CREO : 20/Agosto/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Pre_Multas_Derechos_Supervision
    {
        public const String Tabla_Cat_Pre_Multas_Derechos_Supervision = "CAT_PRE_MUL_DER_SUPERVISION";
        public const String Campo_Multa_ID = "DER_MULTA_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Desde_Anios = "DESDE_ANIOS";
        public const String Campo_Hasta_Anios = "HASTA_ANIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Multas_Derechos_Supervision_Detalles
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_DET_MUL_DER_SUPER
     PARÁMETROS :     
     CREO       : José Alfredo García Pichardo
     FECHA_CREO : 20/Agosto/2011 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Multas_Derechos_Supervision_Detalles
    {
        public const String Tabla_Cat_Pre_Multas_Derechos_Supervision_Detalles = "CAT_PRE_MUL_DER_SUPERV_DET";
        public const String Campo_Multa_Cuota_ID = "DER_MULTA_CUOTA_ID";
        public const String Campo_Multa_ID = "DER_MULTA_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Multas_Fraccionamientos
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_MUL_FRACCIONA
    ///PARÁMETROS :     
    ///CREO       : José Alfredo García Pichardo
    ///FECHA_CREO : 20/Agosto/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Pre_Multas_Fraccionamientos
    {
        public const String Tabla_Cat_Pre_Multas_Fraccionamientos = "CAT_PRE_MUL_FRACCIONA";
        public const String Campo_Multa_ID = "FRA_MULTA_ID";
        public const String Campo_Identificador = "IDENTIFICADOR";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Desde_Anios = "DESDE_ANIOS";
        public const String Campo_Hasta_Anios = "HASTA_ANIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Multas_Fraccionamientos_Detalles
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_MUL_FRACCIONA_DET
     PARÁMETROS :     
     CREO       : José Alfredo García Pichardo.
     FECHA_CREO : 20/Agosto/2010 
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Multas_Fraccionamientos_Detalles
    {
        public const String Tabla_Cat_Pre_Multas_Fraccionamientos_Detalles = "CAT_PRE_MUL_FRACCIONA_DET";
        public const String Campo_Multa_Cuota_ID = "FRA_MULTA_CUOTA_ID";
        public const String Campo_Multa_ID = "FRA_MULTA_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    /*******************************************************************************
   NOMBRE DE LA CLASE: Ope_Pre_Quitar_Beneficio
   DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_QUITAR_BENEFICIO
   PARÁMETROS :     
   CREO       : Jacqueline Ramirez Sierra
   FECHA_CREO : 18-Nov-2011
   MODIFICO          :
   FECHA_MODIFICO    :
   CAUSA_MODIFICACIÓN:
  *******************************************************************************/
    public class Ope_Pre_Quitar_Beneficio
    {
        public const String Tabla_Ope_Pre_Quitar_Beneficio = "OPE_PRE_QUITAR_BENEFICIO";
        public const String Campo_No_Beneficio_ID = "NO_BENEFICIO_ID";
        public const String Campo_Caso_Especial_ID = "CASO_ESPECIAL_ID";
        public const String Campo_Fecha_Hora = "FECHA_HORA";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Quitar_Beneficio = "QUITAR_BENEFICIO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Ope_Caj_Arqueos
     DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAJ_ARQUEOS
     PARÁMETROS :     
     CREO       : José Alfredo García Pichardo.
     FECHA_CREO : 30/Agosto/2011
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Caj_Arqueos
    {
        public const String Tabla_Ope_Caj_Arqueos = "OPE_CAJ_ARQUEOS";
        public const String Campo_No_Arqueo = "NO_ARQUEO";
        public const String Campo_No_Turno = "NO_TURNO";
        public const String Campo_Realizo = "REALIZO";
        public const String Campo_Total_Cobrado = "TOTAL_COBRADO";
        public const String Campo_Total_Recolectado = "TOTAL_RECOLECTADO";
        public const String Campo_Fondo_Inicial = "FONDO_INICIAL";
        public const String Campo_Total_Efectivo = "TOTAL_EFECTIVO";
        public const String Campo_Total_Cheques = "TOTAL_CHEQUES";
        public const String Campo_Total_Tarjeta = "TOTAL_TARJETA";
        public const String Campo_Total_Transferencias = "TOTAL_TRANSFERENCIA";
        public const String Campo_Diferencia = "DIFERENCIA";
        public const String Campo_Arqueo = "ARQUEO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Ope_Caj_Arqueos_Det
     DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_CAJ_ARQUEOS_DET
     PARÁMETROS :     
     CREO       : José Alfredo García Pichardo.
     FECHA_CREO : 30/Agosto/2011
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Caj_Arqueos_Det
    {
        public const String Tabla_Ope_Caj_Arqueos_Det = "OPE_CAJ_ARQUEOS_DET";
        public const String Campo_No_Arqueo_Det = "NO_ARQUEO_DET";
        public const String Campo_No_Arqueo = "NO_ARQUEO";
        public const String Campo_Denom_10_Cent = "DENOM_10_CENT";
        public const String Campo_Denom_20_Cent = "DENOM_20_CENT";
        public const String Campo_Denom_50_Cent = "DENOM_50_CENT";
        public const String Campo_Denom_1_Peso = "DENOM_1_PESO";
        public const String Campo_Denom_2_Pesos = "DENOM_2_PESOS";
        public const String Campo_Denom_5_Pesos = "DENOM_5_PESOS";
        public const String Campo_Denom_10_Pesos = "DENOM_10_PESOS";
        public const String Campo_Denom_20_Pesos = "DENOM_20_PESOS";
        public const String Campo_Denom_50_Pesos = "DENOM_50_PESOS";
        public const String Campo_Denom_100_Pesos = "DENOM_100_PESOS";
        public const String Campo_Denom_200_Pesos = "DENOM_200_PESOS";
        public const String Campo_Denom_500_Pesos = "DENOM_500_PESOS";
        public const String Campo_Denom_1000_Pesos = "DENOM_1000_PESOS";
        public const String Campo_Monto_Total = "MONTO_TOTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Cat_Pre_Grupos_Movimiento
     DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_GRUPOS_MOVIMIENTO
     PARÁMETROS :     
     CREO       : José Alfredo García Pichardo.
     FECHA_CREO : 12/Septiembre/2011
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Grupos_Movimiento
    {
        public const String Tabla_Cat_Pre_Grupos_Movimiento = "CAT_PRE_GRUPOS_MOVIMIENTO";
        public const String Campo_Grupo_Movimiento_ID = "GRUPO_MOVIMIENTO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Habilita_Captura_No_Nota = "HABILITA_CAPTURA_NO_NOTA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE : Cat_Pre_Grupos_Movimiento_Detalles
     DESCRIPCIÓN        : Clase que contiene los campos de la tabla CAT_PRE_GRUPOS_MOVIMIENTO_DET
     PARÁMETROS:
     CREO               : Antonio Salvador Benavides Guardado
     FECHA_CREO         : 22/Marzo/2012
     MODIFICO:
     FECHA_MODIFICO:
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Cat_Pre_Grupos_Movimiento_Detalles
    {
        public const String Tabla_Cat_Pre_Grupos_Movimiento_Detalles = "CAT_PRE_GRUPOS_MOVIMIENTO_DET";
        public const String Campo_Grupo_Movimiento_ID = "GRUPO_MOVIMIENTO_ID";
        public const String Campo_Tipo_Predio_ID = "TIPO_PREDIO_ID";
        public const String Campo_Año = "ANIO";
        public const String Campo_Folio_Inicial = "FOLIO_INICIAL";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Ope_Pre_Lineas_Captura
     DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_LINEAS_CAPTURA
     PARÁMETROS :     
     CREO       : Roberto Gonzáles Oseguera
     FECHA_CREO : 19-ene-2011
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Lineas_Captura
    {
        public const String Tabla_Ope_Pre_Lineas_Captura = "OPE_PRE_LINEAS_CAPTURA";
        public const String Campo_No_Linea_Captura = "NO_LINEA_CAPTURA";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Institucion_Id = "INSTITUCION_ID";
        public const String Campo_Tipo_Predio_ID = "TIPO_PREDIO_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Linea_Captura_Enero = "LINEA_CAPTURA_ENERO";
        public const String Campo_Linea_Captura_Febrero = "LINEA_CAPTURA_FEBRERO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Ope_Pre_Capturas_Pagos_Instituciones_Externas
     DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_CAPTURAS_PAG_INS_EXT
     PARÁMETROS :     
     CREO       : Roberto Gonzáles Oseguera
     FECHA_CREO : 19-ene-2011
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Capturas_Pagos_Instituciones_Externas
    {
        public const String Tabla_Ope_Pre_Capturas_Pag_Ins_Ext = " OPE_PRE_CAPTURAS_PAG_INS_EXT";
        public const String Campo_No_Captura = "NO_CAPTURA";
        public const String Campo_Institucion_Id = "INSTITUCION_ID";
        public const String Campo_Fecha_Captura = "FECHA_CAPTURA";
        public const String Campo_Nombre_Archivo = "NOMBRE_ARCHIVO";
        public const String Campo_Fecha_Corte = "FECHA_CORTE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
     NOMBRE DE LA CLASE: Ope_Pre_Detalles_Captura_Pagos_Instit_Exter
     DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_DET_CAPT_PAG_INS_EXT
     PARÁMETROS :     
     CREO       : Roberto Gonzáles Oseguera
     FECHA_CREO : 19-ene-2011
     MODIFICO          :
     FECHA_MODIFICO    :
     CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Detalles_Captura_Pagos_Instit_Exter
    {
        public const String Tabla_Ope_Pre_Det_Capt_Pag_Ins_Ext = " OPE_PRE_DET_CAPT_PAG_INS_EXT";
        public const String Campo_No_Detalle_Captura = "NO_DETALLE_CAPTURA";
        public const String Campo_No_Captura = "NO_CAPTURA";
        public const String Campo_Linea_Captura = "LINEA_CAPTURA";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Fecha_Pago = "FECHA_PAGO";
        public const String Campo_Monto_Pagado = "MONTO_PAGADO";
        public const String Campo_Monto_Pagado_Original = "MONTO_PAGADO_ORIGINAL";
        public const String Campo_Cuenta_Predial = "CUENTA_PREDIAL";
        public const String Campo_Cuenta_Predial_Original = "CUENTA_PREDIAL_ORIGINAL";
        public const String Campo_Monto_Total_Adeudo = "MONTO_TOTAL_ADEUDO";
        public const String Campo_Adeudo_Rezago = "ADEUDO_REZAGO";
        public const String Campo_Adeudo_Corriente = "ADEUDO_CORRIENTE";
        public const String Campo_Adeudo_Recargos = "ADEUDO_RECARGOS";
        public const String Campo_Adeudo_Honorarios = "ADEUDO_HONORARIOS";
        public const String Campo_Descuento = "DESCUENTO";
        public const String Campo_Diferencia = "DIFERENCIA";
        public const String Campo_Sucursal = "SUCURSAL";
        public const String Campo_Tipo_Pago = "TIPO_PAGO";
        public const String Campo_Guia_Cie = "GUIA_CIE";
        public const String Campo_Cajero = "CAJERO";
        public const String Campo_Autorizacion = "AUTORIZACION";
        public const String Campo_Incluido = "INCLUIDO";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Pre_Pae_Etapas
    DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Etapas
    PARÁMETROS :     
    CREO       : Armando Zavala Moreno
    FECHA_CREO : 15-Feb-2012
    MODIFICO          :
    FECHA_MODIFICO    :
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Pae_Etapas
    {
        public const String Tabla_Ope_Pre_Pae_Etapas = "OPE_PRE_PAE_ETAPAS";
        public const String Campo_No_Etapa = "NO_ETAPA";
        public const String Campo_Despacho_Id = "DESPACHO_ID";
        public const String Campo_Numero_Entrega = "NUMERO_ENTREGA";
        public const String Campo_Total_Etapa = "TOTAL_ETAPA";
        public const String Campo_Modo_Generacion = "MODO_GENERACION";
        public const String Campo_Nombre_Archivo = "NOMBRE_ARCHIVO";
        public const String Campo_Fecha_Generacion = "FECHA_GENERACION";
        public const String Campo_Comentario = "COMENTARIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Pre_Pae_Det_Etapas
    DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Det_Etapas
    PARÁMETROS :     
    CREO       : Armando Zavala Moreno
    FECHA_CREO : 15-Feb-2012
    MODIFICO          :
    FECHA_MODIFICO    :
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Pae_Det_Etapas
    {
        public const String Tabla_Ope_Pre_Pae_Det_Etapas = "OPE_PRE_PAE_DET_ETAPAS";
        public const String Campo_No_Detalle_Etapa = "NO_DETALLE_ETAPA";
        public const String Campo_No_Etapa = "NO_ETAPA";
        public const String Campo_Cuenta_Predial_Id = "CUENTA_PREDIAL_ID";
        public const String Campo_Periodo_Corriente = "PERIODO_CORRIENTE";
        public const String Campo_Adeudo_Corriente = "ADEUDO_CORRIENTE";
        public const String Campo_Periodo_Rezago = "PERIODO_REZAGO";
        public const String Campo_Adeudo_Rezago = "ADEUDO_REZAGO";
        public const String Campo_Adeudo_Recargos_Ordinarios = "ADEUDO_RECARGOS_ORDINARIOS";
        public const String Campo_Adeudo_Recargos_Moratorios = "ADEUDO_RECARGOS_MORATORIOS";
        public const String Campo_Adeudo_Honorarios = "ADEUDO_HONORARIOS";
        public const String Campo_Multas = "MULTAS";
        public const String Campo_Adeudo_Total = "ADEUDO_TOTAL";
        public const String Campo_Proceso_Actual = "PROCESO_ACTUAL";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Omitida = "OMITIDA";
        public const String Campo_Motivo_Omision = "MOTIVO_OMISION";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Impresa = "IMPRESA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Fecha_Hora_Notificacion = "FECHA_HORA_NOTIFICACION";
        public const String Campo_Motivo_Cambio_Estatus = "MOTIVO_CAMBIO_ESTATUS";
        public const String Campo_Resolucion = "RESOLUCION";
    }
    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Pre_Pae_Detalles_Cuentas
    DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Detalles_Cuentas
    PARÁMETROS :     
    CREO       : Armando Zavala Moreno
    FECHA_CREO : 15-Feb-2012
    MODIFICO          :
    FECHA_MODIFICO    :
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Pae_Detalles_Cuentas
    {
        public const String Tabla_Ope_Pre_Pae_Detalles_Cuentas = "OPE_PRE_PAE_DETALLES_CUENTAS";
        public const String Campo_No_Detalle_Cuenta = "NO_DETALLE_CUENTA";
        public const String Campo_No_Detalle_Etapa = "NO_DETALLE_ETAPA";
        public const String Campo_Fecha_Proceso_Cambio = "FECHA_PROCESO_CAMBIO";
        public const String Campo_Proceso_Anterior = "PROCESO_ANTERIOR";
        public const String Campo_Proceso_Actual = "PROCESO_ACTUAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_No_Almoneda = "NO_ALMONEDA";
    }
    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Pre_Pae_Impresiones
    DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Impresiones
    PARÁMETROS :     
    CREO       : Armando Zavala Moreno
    FECHA_CREO : 15-Feb-2012
    MODIFICO          :
    FECHA_MODIFICO    :
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Pae_Impresiones
    {
        public const String Tabla_Ope_Pre_Pae_Impresiones = "OPE_PRE_PAE_IMPRESIONES";
        public const String Campo_No_Impresion = "NO_IMPRESION";
        public const String Campo_No_Detalle_Etapa = "NO_DETALLE_ETAPA";
        public const String Campo_Fecha_Impresion = "FECHA_IMPRESION";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Proceso = "PROCESO";
        public const String Campo_Total_Proceso = "TOTAL_PROCESO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Pre_Pae_Honorarios
    DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Honorarios
    PARÁMETROS :     
    CREO       : Armando Zavala Moreno
    FECHA_CREO : 07-Mar-2012
    MODIFICO          :
    FECHA_MODIFICO    :
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Pae_Honorarios
    {
        public const String Tabla_Ope_Pre_Pae_Honorarios = "OPE_PRE_PAE_HONORARIOS";
        public const String Campo_No_Honorario = "NO_HONORARIO";
        public const String Campo_No_Detalle_Etapa = "NO_DETALLE_ETAPA";
        public const String Campo_Gasto_Ejecucion_Id = "GASTO_EJECUCION_ID";
        public const String Campo_Fecha_Honorario = "FECHA_HONORARIO";
        public const String Campo_Proceso = "PROCESO";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Monto_Pagado = "MONTO_PAGADO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Pre_Pae_Notificaciones
    DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Notificaciones
    PARÁMETROS :     
    CREO       : Armando Zavala Moreno
    FECHA_CREO : 20-Mar-2012
    MODIFICO          :
    FECHA_MODIFICO    :
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Pae_Notificaciones
    {
        public const String Tabla_Ope_Pre_Pae_Notificaciones = "OPE_PRE_PAE_NOTIFICACIONES";
        public const String Campo_No_Notificacion = "NO_NOTIFICACION";
        public const String Campo_No_Detalle_Etapa = "NO_DETALLE_ETAPA";
        public const String Campo_Fecha_Hora = "FECHA_HORA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Notificador = "NOTIFICADOR";
        public const String Campo_Recibio = "RECIBIO";
        public const String Campo_Acuse_Recibo = "ACUSE_RECIBO";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Medio_Notificacion = "MEDIO_NOTIFICACION";
        public const String Campo_Proceso = "PROCESO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Pre_Pae_Publicaciones
    DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Publicacione
    PARÁMETROS :     
    CREO       : Armando Zavala Moreno
    FECHA_CREO : 20-Mar-2012
    MODIFICO          :
    FECHA_MODIFICO    :
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Pae_Publicaciones
    {
        public const String Tabla_Ope_Pre_Pae_Publicaciones = "OPE_PRE_PAE_PUBLICACIONES";
        public const String Campo_No_Publicacion = "NO_PUBLICACION";
        public const String Campo_No_Detalle_Etapa = "NO_DETALLE_ETAPA";
        public const String Campo_Fecha_Publicacion = "FECHA_PUBLICACION";
        public const String Campo_Medio_Publicacion = "MEDIO_PUBLICACION";
        public const String Campo_Pagina = "PAGINA";
        public const String Campo_Tomo = "TOMO";
        public const String Campo_Parte = "PARTE";
        public const String Campo_Foja = "FOJA";
        public const String Campo_Proceso = "PROCESO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Pre_Pae_Bienes
    DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Bienes
    PARÁMETROS :     
    CREO       : Armando Zavala Moreno
    FECHA_CREO : 20-Mar-2012
    MODIFICO          :
    FECHA_MODIFICO    :
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Pae_Bienes
    {
        public const String Tabla_Ope_Pre_Pae_Bienes = "OPE_PRE_PAE_BIENES";
        public const String Campo_No_Bien = "NO_BIEN";
        public const String Campo_No_Peritaje = "NO_PERITAJE";
        public const String Campo_Tipo_Bien_Id = "TIPO_BIEN_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Valor = "VALOR";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Pre_Pae_Peritajes 
    DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Peritajes
    PARÁMETROS :     
    CREO       : Armando Zavala Moreno
    FECHA_CREO : 20-Mar-2012
    MODIFICO          :
    FECHA_MODIFICO    :
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Pae_Peritajes
    {
        public const String Tabla_Ope_Pre_Pae_Peritajes = "OPE_PRE_PAE_PERITAJES";
        public const String Campo_No_Peritaje = "NO_PERITAJE";
        public const String Campo_No_Detalle_Etapa = "NO_DETALLE_ETAPA";
        public const String Campo_Avaluo = "AVALUO";
        public const String Campo_Fecha_Peritaje = "FECHA_PERITAJE";
        public const String Campo_Perito = "PERITO";
        public const String Campo_Valor = "VALOR";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Lugar_Almacenamiento = "LUGAR_ALMACENAMIENTO";
        public const String Campo_Costo_Metro_Cuadrado = "COSTO_METRO_CUADRADO";
        public const String Campo_Dimensiones = "DIMENSIONES";
        public const String Campo_Fecha_Ingreso = "FECHA_INGRESO";
        public const String Campo_Tiempo_Transcurrido = "TIEMPO_TRANSCURRIDO";
        public const String Campo_Costo_Almacenamiento = "COSTO_ALMACENAMIENTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Pre_Pae_Depositarios
    DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Depositarios
    PARÁMETROS :     
    CREO       : Angel Antonio Escamilla Trejo
    FECHA_CREO : 20-Mar-2012
    MODIFICO          :
    FECHA_MODIFICO    :
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Pae_Depositarios
    {
        public const String Tabla_Ope_Pre_Pae_Depositarios = "OPE_PRE_PAE_DEPOSITARIOS";
        public const String Campo_No_Cambio_Depositario = "NO_CAMBIO_DEPOSITARIO";
        public const String Campo_No_Detalle_Etapa = "NO_DETALLE_ETAPA";
        public const String Campo_Figura = "FIGURA";
        public const String Campo_Nombre_Depositario = "NOMBRE";
        public const String Campo_Domicilio_Depositario = "DOMICILIO";
        public const String Campo_Fecha_Remocion = "FECHA_REMOCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
   NOMBRE DE LA CLASE: Ope_Pre_Pae_Imagenes_Bienes
   DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_PAE_IMAGENES_BIENES
   PARÁMETROS :     
   CREO       : Roberto González Oseguera
   FECHA_CREO : 26-abr-2012
   MODIFICO          :
   FECHA_MODIFICO    :
   CAUSA_MODIFICACIÓN:
   *******************************************************************************/
    public class Ope_Pre_Pae_Imagenes_Bienes
    {
        public const String Tabla_Ope_Pre_Pae_Imagenes_Bienes = "OPE_PRE_PAE_IMAGENES_BIENES";
        public const String Campo_No_Imagen = "NO_IMAGEN";
        public const String Campo_No_Bien = "NO_BIEN";
        public const String Campo_Ruta_Imagen = "RUTA_IMAGEN";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Pre_Pae_Almonedas 
    DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Almonedas
    PARÁMETROS :     
    CREO       : Armando Zavala Moreno
    FECHA_CREO : 20-Mar-2012
    MODIFICO          :
    FECHA_MODIFICO    :
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Pae_Almonedas
    {
        public const String Tabla_Ope_Pre_Pae_Almonedas = "OPE_PRE_PAE_ALMONEDAS";
        public const String Campo_No_Almoneda = "NO_ALMONEDA";
        public const String Campo_No_Detalle_Etapa = "NO_DETALLE_ETAPA";
        public const String Campo_Numero_Almoneda_Cuenta = "NUMERO_ALMONEDA_CUENTA";
        public const String Campo_Valor_Avaluo = "VALOR_AVALUO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    /*******************************************************************************
   NOMBRE DE LA CLASE: Ope_Pre_Pae_Remates 
   DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Remates
   PARÁMETROS :     
   CREO       : Armando Zavala Moreno
   FECHA_CREO : 03-May-2012
   MODIFICO          :
   FECHA_MODIFICO    :
   CAUSA_MODIFICACIÓN:
   *******************************************************************************/
    public class Ope_Pre_Pae_Remates
    {
        public const String Tabla_Ope_Pre_Pae_Remates = "OPE_PRE_PAE_REMATES";
        public const String Campo_No_Remate = "NO_REMATE";
        public const String Campo_No_Detalle_Etapa = "NO_DETALLE_ETAPA";
        public const String Campo_Lugar_Remate = "LUGAR_REMATE";
        public const String Campo_Fecha_Hora_Remate = "FECHA_HORA_REMATE";
        public const String Campo_Inicio_Publicacion = "INICIO_PUBLICACION";
        public const String Campo_Fin_Publicacion = "FIN_PUBLICACION";
        public const String Campo_Adeudo_Actual = "ADEUDO_ACTUAL";
        public const String Campo_Adeudo_Cubierto = "ADEUDO_CUBIERTO";
        public const String Campo_Adeudo_Restante = "ADEUDO_RESTANTE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    /*******************************************************************************
    NOMBRE DE LA CLASE: Ope_Pre_Pae_Postores 
    DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Pre_Pae_Postores
    PARÁMETROS :     
    CREO       : Armando Zavala Moreno
    FECHA_CREO : 03-May-2012
    MODIFICO          :
    FECHA_MODIFICO    :
    CAUSA_MODIFICACIÓN:
    *******************************************************************************/
    public class Ope_Pre_Pae_Postores
    {
        public const String Tabla_Ope_Pre_Pae_Postores = "OPE_PRE_PAE_POSTORES";
        public const String Campo_No_Postor = "NO_POSTOR";
        public const String Campo_No_Detalle_Etapa = "NO_DETALLE_ETAPA";
        public const String Campo_Nombre_Postor = "NOMBRE_POSTOR";
        public const String Campo_Deposito = "DEPOSITO";
        public const String Campo_Porcentaje = "PORCENTAJE";
        public const String Campo_Domicilio = "DOMICILIO";
        public const String Campo_Telefono = "TELEFONO";
        public const String Campo_Rfc = "RFC";
        public const String Campo_No_Ife = "NO_IFE";
        public const String Campo_Sexo = "SEXO";
        public const String Campo_Estado_Civil = "ESTADO_CIVIL";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    #endregion


    ///*************************************************************************************************************************
    ///                                                                NOMINA
    ///*************************************************************************************************************************

    #region Nomina

    #region (Catalogos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Tabla_Cat_Organigrama
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_ORGANIGRAMA
    /// PARAMETROS :
    /// CREO       : Ramón Baeza Yépez
    /// FECHA_CREO : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Organigrama
    {
        public const String Tabla_Cat_Organigrama = "CAT_ORGANIGRAMA";
        public const String Campo_Parametro_ID = "PARAMETRO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Nombre_Empleado = "NOMBRE_EMPLEADO";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Modulo = "MODULO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Parametros_Prestamos
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_PARAMETROS_PRESTAMOS
    /// PARAMETROS :
    /// CREO       : Ramón Baeza Yépez
    /// FECHA_CREO : 22/Junio/2012
    /// MODIFICO          :Ramón Baeza Yépez
    /// FECHA_MODIFICO    :26/Junio/2012
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Parametros_Prestamos
    {
        public const String Tabla_Cat_Nom_Parametros_Prestamos = "CAT_NOM_PARAMETROS_PRESTAMOS";
        public const String Campo_Parametro_ID = "PARAMETRO_ID";
        public const String Campo_Prestamo_Corto_Plazo = "PRESTAMO_CORTO_PLAZO";
        public const String Campo_Prestamo_Mercado = "PRESTAMO_MERCADOS";
        public const String Campo_Prestamo_Tesoreria = "PRESTAMO_TESORERIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }
    public class Cat_Nom_Parametros_Desc
    {
        public const String Tabla_Cat_Nom_Parametros_Desc = "CAT_NOM_PARAMETROS_DESC";
        public const String Campo_Parametro_ID = "PARAMETRO_ID";
        public const String Campo_Desc_PMO_Mercados = "DESC_PMO_MERCADOS";
        public const String Campo_Desc_PMO_Tesoreria = "DESC_PMO_TESORERIA";
        public const String Campo_Desc_PMO_Corto_Plazo = "DESC_PMO_CORTO_PLAZO";
        public const String Campo_Desc_PMO_Pago_Aval = "DESC_PMO_PAGO_AVAL";
        public const String Campo_Desc_PMO_IMUVI = "DESC_PMO_IMUVI";
        public const String Campo_Desc_Llamadas_Tel = "DESC_LLAMADAS_TEL";
        public const String Campo_Desc_Perdida_Equipo = "DESC_PERDIDA_EQUIPO";
        public const String Campo_Desc_Otros_Fijos = "DESC_OTROS_FJOS";
        public const String Campo_Desc_Otros_Variables = "DESC_OTROS_VAR";
        public const String Campo_Desc_Agua = "DESC_AGUA";
        public const String Campo_Desc_Pago_Predial = "DESC_PAGO_PREDIAL";
    }

    ///****************************************************************************************************************************************************************
    ///NOMBRE:      Cat_Nom_Perc_Dedu_CC_Deta
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla CAT_NOM_PERC_DEDU_CC_DETA
    ///CREO:        Juan Alberto Hernandez Negrete
    ///FECHA CREÓ:  Marzo/2012
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Cat_Nom_Perc_Dedu_CC_Deta
    {
        public const String Tabla_Cat_Nom_Perc_Dedu_CC_Deta = "CAT_NOM_PERC_DEDU_CC_DETA";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
    }
    ///****************************************************************************************************************************************************************
    ///NOMBRE:      Cat_Nom_Claves_Cargo_Abono
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla CAT_NOM_CLAVE_CARGO_ABONO
    ///CREO:        Juan Alberto Hernandez Negrete
    ///FECHA CREÓ:  Marzo/2012
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Cat_Nom_Claves_Cargo_Abono
    {
        public const String Tabla_Cat_Nom_Claves_Cargo_Abono = "CAT_NOM_CLAVE_CARGO_ABONO";
        public const String Campo_Cargo_Abono_ID = "CARGO_ABONO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///****************************************************************************************************************************************************************
    ///NOMBRE:      Cat_Nom_Tipos_Desc_Esp
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla CAT_NOM_TIPOS_DESC_ESP
    ///CREO:        Juan Alberto Hernandez Negrete
    ///FECHA CREÓ:  Marzo/2012
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Cat_Nom_Tipos_Desc_Esp
    {
        public const String Tabla_Cat_Nom_Tipos_Desc_Esp = "CAT_NOM_TIPOS_DESC_ESP";
        public const String Campo_Tipo_Desc_Esp_ID = "TIPO_DESC_ESP_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Cargo_Abono_ID = "CARGO_ABONO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///****************************************************************************************************************************************************************
    ///NOMBRE:      Cat_Nom_Tipos_Pagos
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla Cat_Nom_Tipos_Pagos
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:  06/Enero/2012
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Cat_Nom_Tipos_Pagos
    {
        public const String Tabla_Cat_Nom_Tipos_Pagos = "CAT_NOM_TIPOS_PAGOS";
        public const String Campo_Tipo_Pago_ID = "TIPO_PAGO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Apl_Cat_Modulos_SIAG
    /// DESCRIPCION: Clase con contiene los datos de la tabla APL_CAT_MODULOS_SIAG
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernández Negrete.
    /// FECHA_CREO : 05/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    //public class Apl_Cat_Modulos_SIAG
    //{
    //    public const String Tabla_Apl_Cat_Modulos_SIAG = "APL_CAT_MODULOS_SIAG";
    //    public const String Campo_Modulo_ID = "MODULO_ID";
    //    public const String Campo_Nombre = "NOMBRE";
    //    public const String Campo_Usuario_Creo = "USUARIO_CREO";
    //    public const String Campo_Fecha_Creo = "FECHA_CREO";
    //    public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
    //    public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    //}

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Tab_Orden_Judicial
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_TAB_ORDEN_JUDICIAL
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernández Negrete.
    /// FECHA_CREO : 05/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Tab_Orden_Judicial
    {
        public const String Tabla_Cat_Nom_Tab_Orden_Judicial = "CAT_NOM_TAB_ORDEN_JUDICIAL";
        public const String Campo_Orden_Judicial_ID = "ORDEN_JUDICIAL_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Beneficiario = "BENEFICIARIO";
        public const String Campo_Tipo_Descuento_Orden_Judicial_Sueldo = "TIPO_DESC_OJ_SUELDO";
        public const String Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo = "CANTIDAD_PORC_SUELDO";
        public const String Campo_Bruto_Neto_Orden_Judicial_Sueldo = "OJ_BRUTO_NETO_SUELDO";
        public const String Campo_Tipo_Descuento_Orden_Judicial_Aguinaldo = "TIPO_DESC_OJ_AGUINALDO";
        public const String Campo_Cantidad_Porcentaje_Orden_Judicial_Aguinaldo = "CANTIDAD_PORC_AGUINALDO";
        public const String Campo_Bruto_Neto_Orden_Judicial_Aguinaldo = "OJ_BRUTO_NETO_AGUI";
        public const String Campo_Tipo_Descuento_Orden_Judicial_PV = "TIPO_DESC_OJ_PRIMA_VAC";
        public const String Campo_Cantidad_Porcentaje_Orden_Judicial_PV = "CANTIDAD_PORC_PRIMA_VAC";
        public const String Campo_Bruto_Neto_Orden_Judicial_PV = "OJ_BRUTO_NETO_PV";
        public const String Campo_Tipo_Descuento_Orden_Judicial_Indemnizacion = "TIPO_DESC_OJ_INDEMNIZACION";
        public const String Campo_Cantidad_Porcentaje_Orden_Judicial_Indemnizacion = "CANTIDAD_PORC_INDEMNIZACION";
        public const String Campo_Bruto_Neto_Orden_Judicial_Indemnizacion = "OJ_BRUTO_NETO_INDEMNIZACION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Indemnizacion
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_INDEMNIZACION
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernández Negrete.
    /// FECHA_CREO : 20/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Indemnizacion
    {
        public const String Tabla_Cat_Nom_Indemnizacion = "CAT_NOM_INDEMNIZACION";
        public const String Campo_Indemnizacion_ID = "INDEMNIZACION_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Dias = "DIAS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    public class Cat_Nom_Dep_Puestos_Det
    {
        public const String Tabla_Cat_Nom_Dep_Puestos_Det = "CAT_NOM_DEP_PUESTOS_DET";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Puesto_ID = "PUESTO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Tipo_Plaza = "TIPO_PLAZA";

        public const String Campo_S_FTE_FINANCIAMIENTO_ID = "S_FTE_FINANCIAMIENTO_ID";
        public const String Campo_S_AREA_FUNCIONAL_ID = "S_AREA_FUNCIONAL_ID";
        public const String Campo_S_PROGRAMA_ID = "S_PROGRAMA_ID";
        public const String Campo_S_DEPENDENCIA_ID = "S_DEPENDENCIA_ID";
        public const String Campo_S_PARTIDA_ID = "S_PARTIDA_ID";

        public const String Campo_PSM_FTE_FINANCIAMIENTO_ID = "PSM_FTE_FINANCIAMIENTO_ID";
        public const String Campo_PSM_AREA_FUNCIONAL_ID = "PSM_AREA_FUNCIONAL_ID";
        public const String Campo_PSM_PROGRAMA_ID = "PSM_PROGRAMA_ID";
        public const String Campo_PSM_DEPENDENCIA_ID = "PSM_DEPENDENCIA_ID";
        public const String Campo_PSM_PARTIDA_ID = "PSM_PARTIDA_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Dependencias
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_DEPENDENCIAS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Dependencias
    {
        public const String Tabla_Cat_Dependencias = "CAT_DEPENDENCIAS";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Responsable = "RESPONSABLE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Area_Funcional_ID = "AREA_FUNCIONAL_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Grupo_Dependencia_ID = "GRUPO_DEPENDENCIA_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Areas
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_AREAS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Areas
    {
        public const String Tabla_Cat_Areas = "CAT_AREAS";
        public const String Campo_Area_ID = "AREA_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Responsable = "RESPONSABLE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Turnos
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_TURNOS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Turnos
    {
        public const String Tabla_Cat_Turnos = "CAT_TURNOS";
        public const String Campo_Turno_ID = "TURNO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Hora_Entrada = "HORA_ENTRADA";
        public const String Campo_Hora_Salida = "HORA_SALIDA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Empleados
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_EMPLEADOS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// 
    ///         [MODIFICACION A]
    /// 
    /// MODIFICO          : Yazmin A. Delgado Gómez
    /// FECHA_MODIFICO    : 05/Noviembre/2010
    /// CAUSA_MODIFICACION: Porque se quito el campo de Grupo_Percepcion_Deduccion_ID
    ///                     ya que el catálogo va hacer eliminado por la composición
    ///                     de la nómina actual
    /// 
    ///         [MODIFICACION B]
    /// 
    /// MODIFICO          : Juan Alberto Hernandez Negrete
    /// FECHA_MODIFICO    : 05/Noviembre/2010
    /// CAUSA_MODIFICACION: Se agregaron dos Referencias:
    ///                     1.- CAT_NOM_TERCEROS        TERCERO_ID
    ///                     2.- CAT_NOM_TIPOS_NOMINAS   TIPO_NOMINA_ID
    ///                     
    ///     Se elimino el campo de TIPO_NOMINA, ya que este campo no hacia referencia a 
    ///     ninguna tabla                
    ///*******************************************************************************
    public class Cat_Empleados
    {
        public const String Tabla_Cat_Empleados = "CAT_EMPLEADOS";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Area_ID = "AREA_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Programa_ID = "PROGRAMA_ID";
        public const String Campo_Tipo_Contrato_ID = "TIPO_CONTRATO_ID";
        public const String Campo_Puesto_ID = "PUESTO_ID";
        public const String Campo_Escolaridad_ID = "ESCOLARIDAD_ID";
        public const String Campo_Sindicato_ID = "SINDICATO_ID";
        public const String Campo_Turno_ID = "TURNO_ID";
        public const String Campo_Zona_ID = "ZONA_ID";
        public const String Campo_Tipo_Trabajador_ID = "TIPO_TRABAJADOR_ID";
        public const String Campo_Rol_ID = "ROL_ID";
        public const String Campo_Tipo_Nomina_ID = "TIPO_NOMINA_ID";
        public const String Campo_Terceros_ID = "TERCERO_ID";
        public const String Campo_No_Empleado = "NO_EMPLEADO";
        public const String Campo_Password = "PASSWORD";
        public const String Campo_Apellido_Paterno = "APELLIDO_PATERNO";
        public const String Campo_Apellido_Materno = "APELLIDO_MATERNO";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Calle = "CALLE";
        public const String Campo_Colonia = "COLONIA";
        public const String Campo_Codigo_Postal = "CODIGO_POSTAL";
        public const String Campo_Ciudad = "CIUDAD";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Telefono_Casa = "TELEFONO_CASA";
        public const String Campo_Telefono_Oficina = "TELEFONO_OFICINA";
        public const String Campo_Extension = "EXTENSION";
        public const String Campo_Fax = "FAX";
        public const String Campo_Celular = "CELULAR";
        public const String Campo_Nextel = "NEXTEL";
        public const String Campo_Correo_Electronico = "CORREO_ELECTRONICO";
        public const String Campo_Sexo = "SEXO";
        public const String Campo_Fecha_Nacimiento = "FECHA_NACIMIENTO";
        public const String Campo_RFC = "RFC";
        public const String Campo_CURP = "CURP";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Ruta_Foto = "RUTA_FOTO";
        public const String Campo_Nombre_Foto = "NOMBRE_FOTO";
        public const String Campo_No_IMSS = "NO_IMSS";
        public const String Campo_Forma_Pago = "FORMA_PAGO";
        public const String Campo_No_Cuenta_Bancaria = "NO_CUENTA_BANCARIA";
        public const String Campo_Fecha_Inicio = "FECHA_INICIO";
        public const String Campo_Tipo_Finiquito = "TIPO_FINIQUITO";
        public const String Campo_Fecha_Termino_Contrato = "FECHA_TERMINO_CONTRATO";
        public const String Campo_Fecha_Baja = "FECHA_BAJA";
        public const String Campo_Salario_Diario = "SALARIO_DIARIO";
        public const String Campo_Salario_Diario_Integrado = "SALARIO_DIARIO_INTEGRADO";
        public const String Campo_Lunes = "LUNES";
        public const String Campo_Martes = "MARTES";
        public const String Campo_Miercoles = "MIERCOLES";
        public const String Campo_Jueves = "JUEVES";
        public const String Campo_Viernes = "VIERNES";
        public const String Campo_Sabado = "SABADO";
        public const String Campo_Domingo = "DOMINGO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_No_Licencia_Manejo = "NO_LICENCIA_MANEJO";
        public const String Campo_Fecha_Vencimiento_Licencia = "FECHA_VENCIMIENTO_LICENCIA";
        public const String Campo_Nombre_Beneficiario = "NOMBRE_BENEFICIARIO";
        public const String Campo_Cantidad_Porcentaje = "CANTIDAD_PORCENTAJE";
        public const String Campo_Aplica_Orden_Judicial = "APLICA_ORDEN_JUDICIAL";
        public const String Campo_Tipo_Desc_Orden_Judicial = "TIPO_DESC_ORDEN_JUDICIAL";
        public const String Campo_Tipo_Desc_Orden_Judicial_Aguinaldo = "TIPO_DESC_OJ_AGUINALDO";
        public const String Campo_Cantidad_Porcentaje_Aguinaldo = "CANTIDAD_PORC_AGUINALDO";
        public const String Campo_Tipo_Desc_Orden_Judicial_Prima_Vacacional = "TIPO_DESC_OJ_PRIMA_VAC";
        public const String Campo_Cantidad_Porcentaje_Prima_Vacacional = "CANTIDAD_PORC_PRIMA_VAC";
        public const String Campo_Salario_Mensual_Actual = "SALARIO_MENSUAL_ACTUAL";
        public const String Campo_Banco_ID = "BANCO_ID";
        public const String Campo_Reloj_Checador = "RELOJ_CHECADOR";
        public const String Campo_Orden_Judicial_Bruto_Neto_Sueldo_Normal = "OJ_BRUTO_NETO_SN";
        public const String Campo_Orden_Judicial_Bruto_Neto_Aguinaldo = "OJ_BRUTO_NETO_AGUI";
        public const String Campo_Orden_Judicial_Bruto_Neto_Prima_Vacacional = "OJ_BRUTO_NETO_PV";
        public const String Campo_No_Tarjeta = "NO_TARJETA";
        //SAP Código Programático.--------------------------------------------------
        public const String Campo_SAP_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        public const String Campo_SAP_Area_Responsable_ID = "AREA_FUNCIONAL_ID";
        public const String Campo_SAP_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_SAP_Unidad_Responsable_ID = "DEPENDENCIA_ID";
        public const String Campo_SAP_Partida_ID = "PARTIDA_ID";
        public const String Campo_SAP_Codigo_Programatico = "CODIGO_PROGRAMATICO";

        public const String Campo_No_Seguro = "NO_SEGURO";
        public const String Campo_Beneficiario = "BENEFICIARIO";
        public const String Campo_Indemnizacion_ID = "INDEMNIZACION_ID";

        public const String Campo_Tipo_Empleado = "TIPO_EMPLEADO";
        public const String Campo_Confronto = "CONFRONTO";
        public const String Campo_Aplica_ISSEG = "APLICA_ISSEG";

        public const String Campo_Reloj_Checador_ID = "RELOJ_CHECADO_ID";
        public const String Campo_Fecha_Inicio_Reloj_Checador = "FECHA_INICIO_RELOJ_CHECADOR";
        public const String Campo_Movimiento = "MOVIMIENTO";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Tipo_Licencia = "TIPO_LICENCIA";
        public const String Campo_Fecha_Alta_Isseg = "FECHA_ALTA_ISSEG";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Emp_Perc_Dedu_Deta
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_EMPL_PERC_DEDU_DETA
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernández Negrete
    /// FECHA_CREO : 05/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Emp_Perc_Dedu_Deta
    {
        public const String Tabla_Cat_Nom_Emp_Perc_Dedu_Det = "CAT_NOM_EMPL_PERC_DEDU_DETA";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Concepto = "CONCEPTO";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Saldo = "SALDO";
        public const String Campo_Cantidad_Retenida = "CANTIDAD_RETENIDA";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Referencia = "REFERENCIA";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Programas
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_PROGRAMAS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Programas
    {
        public const String Tabla_Cat_Nom_Programas = "CAT_NOM_PROGRAMAS";
        public const String Campo_Programa_ID = "PROGRAMA_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Tipos_Contratos
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_TIPOS_CONTRATOS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Tipos_Contratos
    {
        public const String Tabla_Cat_Nom_Tipos_Contratos = "CAT_NOM_TIPOS_CONTRATOS";
        public const String Campo_Tipo_Contrato_ID = "TIPO_CONTRATO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Escolaridad
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_ESCOLARIDAD
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Escolaridad
    {
        public const String Tabla_Cat_Nom_Escolaridad = "CAT_NOM_ESCOLARIDAD";
        public const String Campo_Escolaridad_ID = "ESCOLARIDAD_ID";
        public const String Campo_Escolaridad = "ESCOLARIDAD";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Sindicatos
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_SINDICATOS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Sindicatos
    {
        public const String Tabla_Cat_Nom_Sindicatos = "CAT_NOM_SINDICATOS";
        public const String Campo_Sindicato_ID = "SINDICATO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Responsable = "RESPONSABLE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Zona_Economica
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_ZONA_ECONOMICA
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          : Yazmin A. Delgado Gómez
    /// FECHA_MODIFICO    : 04/Noviembre/2010
    /// CAUSA_MODIFICACION: ^Porque se agrego el campo de Salario_Diario
    ///*******************************************************************************
    public class Cat_Nom_Zona_Economica
    {
        public const String Tabla_Cat_Nom_Zona_Economica = "CAT_NOM_ZONA_ECONOMICA";
        public const String Campo_Zona_ID = "ZONA_ID";
        public const String Campo_Zona_Economica = "ZONA_ECONOMICA";
        public const String Campo_Salario_Diario = "SALARIO_DIARIO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Tipo_Trabajador
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_TIPO_TRABAJADOR
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Tipo_Trabajador
    {
        public const String Tabla_Cat_Nom_Tipo_Trabajador = "CAT_NOM_TIPO_TRABAJADOR";
        public const String Campo_Tipo_Trabajador_ID = "TIPO_TRABAJADOR_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Percepcion_Deduccion
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_PERCEPCION_DEDUCCION
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          : Yazmin A. Delgado Gómez
    /// FECHA_MODIFICO    : 03/Noviembre/2010
    /// CAUSA_MODIFICACION: Porque las percepciones y las deducciones como son 
    ///                     realizadas actualmente en presidencia cuando son calculadas
    ///                     son codificadas directamente en código interno y no por
    ///                     el usuario
    ///*******************************************************************************
    public class Cat_Nom_Percepcion_Deduccion
    {
        public const String Tabla_Cat_Nom_Percepcion_Deduccion = "CAT_NOM_PERCEPCION_DEDUCCION";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Tipo_Asignacion = "TIPO_ASIGNACION";
        public const String Campo_Gravable = "GRAVABLE";
        public const String Campo_Porcentaje_Gravable = "PORCENTAJE_GRAVABLE";
        public const String Campo_Aplicar = "APLICAR";
        public const String Campo_Concepto = "CONCEPTO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Aplica_IMSS = "APLICA_IMSS";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Grupos_Percepcion_Deduccion
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_GRUPO_PERC_DEDU
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Grupos_Percepcion_Deduccion
    {
        public const String Tabla_Cat_Nom_Grupo_Perc_Dedu = "CAT_NOM_GRUPOS_PERC_DEDU";
        public const String Campo_Grupo_Percepcion_Deduccion_ID = "GRUPO_PERCEPCION_DEDUCCION_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Detalles_Nom_Grupo_Percepcion_Deduccion
    /// DESCRIPCION: Clase con contiene los datos de la tabla DETALLES_NOM_GRUPO_PERC_DEDU
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Detalles_Nom_Grupo_Percepcion_Deduccion
    {
        public const String Tabla_Detalles_Nom_Grupo_Perc_Dedu = "DETALLES_NOM_GRUPO_PERC_DEDU";
        public const String Campo_Grupo_Percepcion_Deduccion_ID = "GRUPO_PERCEPCION_DEDUCCION_ID";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Detalles_Nom_Empleado_Percepcion_Deduccion
    /// DESCRIPCION: Clase con contiene los datos de la tabla DETALLES_NOM_EMPL_PERC_DEDU
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Detalles_Nom_Empleado_Percepcion_Deduccion
    {
        public const String Tabla_Detalles_Nom_Empl_Perc_Dedu = "DETALLES_NOM_EMPL_PERC_DEDU";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Requisitos_Empleados
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_REQUISITOS_EMPLEADOS
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Requisitos_Empleados
    {
        public const String Tabla_Cat_Nom_Requisitos_Empleados = "CAT_NOM_REQUISITOS_EMPLEADO";
        public const String Campo_Requisito_ID = "REQUISITO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Archivo = "ARCHIVO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Calendario_Nominas
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_CALENDARIO_NOMINAS
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 20/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Calendario_Nominas
    {
        public const String Tabla_Cat_Nom_Calendario_Nominas = "CAT_NOM_CALENDARIO_NOMINAS";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_Fecha_Inicio = "FECHA_INICIO";
        public const String Campo_Fecha_Fin = "FECHA_FIN";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Anio = "ANIO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Nominas_Detalles
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_NOMINAS_DETALLES
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 21/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///******************************************************************************
    public class Cat_Nom_Nominas_Detalles
    {
        public const String Tabla_Cat_Nom_Nominas_Detalles = "CAT_NOM_NOMINAS_DETALLES";
        public const String Campo_Detalle_Nomina_ID = "DETALLE_NOMINA_ID";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Fecha_Inicio = "FECHA_INICIO";
        public const String Campo_Fecha_Fin = "FECHA_FIN";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Tabla_Cat_Nom_Periodo_Tipo_Nom
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_PERIODO_TIPO_NOM
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 24/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///******************************************************************************
    public class Cat_Nom_Periodo_Tipo_Nom
    {
        public const String Tabla_Cat_Nom_Periodo_Tipo_Nom = "CAT_NOM_PERIODO_TIPO_NOM";
        public const String Campo_Tipo_Nomina_ID = "TIPO_NOMINA_ID";
        public const String Campo_Detalle_Nomina_ID = "DETALLE_NOMINA_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Proveedores
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_PROVEEDORES
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 23/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///******************************************************************************
    public class Cat_Nom_Proveedores
    {
        public const String Tabla_Cat_Nom_Proveedores = "CAT_NOM_PROVEEDORES";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_RFC = "RFC";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Calle = "CALLE";
        public const String Campo_Numero = "NUMERO";
        public const String Campo_Colonia = "COLONIA";
        public const String Campo_Codigo_Postal = "CODIGO_POSTAL";
        public const String Campo_Ciudad = "CIUDAD";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Telefono = "TELEFONO";
        public const String Campo_Fax = "FAX";
        public const String Campo_Contacto = "CONTACTO";
        public const String Campo_Email = "EMAIL";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Aval = "AVAL";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Proveedores_Detalles
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_NOMINAS_DETALLES
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 21/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///******************************************************************************
    public class Cat_Nom_Proveedores_Detalles
    {
        public const String Tabla_Cat_Nom_Proveedores_Detalles = "CAT_NOM_PROVEEDORES_DETALLES";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_SIN_PER_DED_DET
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 03/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///******************************************************************************
    public class Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles
    {
        public const String Tabla_Cat_Nom_Sin_Per_Ded_Det = "CAT_NOM_SIN_PER_DED_DET";
        public const String Campo_Sindicato_ID = "SINDICATO_ID";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Cantidad = "CANTIDAD";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Tipos_Nominas
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_TIPOS_NOMINAS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 03/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///******************************************************************************
    public class Cat_Nom_Tipos_Nominas
    {
        public const String Tabla_Cat_Nom_Tipos_Nominas = "CAT_NOM_TIPOS_NOMINAS";
        public const String Campo_Tipo_Nomina_ID = "TIPO_NOMINA_ID";
        public const String Campo_Nomina = "NOMINA";
        public const String Campo_Dias_Prima_Vacacional_1 = "DIAS_PRIMA_VACACIONAL_1";
        public const String Campo_Dias_Prima_Vacacional_2 = "DIAS_PRIMA_VACACIONAL_2";
        public const String Campo_Dias_Aguinaldo = "DIAS_AGUINALDO";
        public const String Campo_Dias_Exenta_Prima_Vacacional = "DIAS_EXENTA_PRIMA_VACACIONAL";
        public const String Campo_Dias_Exenta_Aguinaldo = "DIAS_EXENTA_AGUINALDO";
        public const String Campo_Despensa = "DESPENSA";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Aplica_ISR = "APLICA_ISR";
        public const String Campo_Actualizar_Salario = "ACTUALIZAR_SALARIO";
        public const String Campo_Dias_Prima_Antiguedad = "DIAS_PRIMA_ANTIGUEDAD";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_TIP_NOM_PER_DED_DET
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 03/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///******************************************************************************
    public class Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles
    {
        public const String Tabla_Cat_Nom_Tip_Nom_Per_Ded_Det = "CAT_NOM_TIP_NOM_PER_DED_DET ";
        public const String Campo_Tipo_Nomina_ID = "TIPO_NOMINA_ID";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Aplica_Todos = "APLICA_TODOS";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Terceros
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_TERCEROS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 03/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///******************************************************************************
    public class Cat_Nom_Terceros
    {
        public const String Tabla_Cat_Nom_Terceros = "CAT_NOM_TERCEROS";
        public const String Campo_Tercero_ID = "TERCERO_ID";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Contacto = "CONTACTO";
        public const String Campo_Telefono = "TELEFONO";
        public const String Campo_Porcentaje_Retencion = "PORCENTAJE_RETENCION";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Parametros
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_PARAMETROS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 03/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///******************************************************************************
    public class Cat_Nom_Parametros
    {
        public const String Tabla_Cat_Nom_Parametros = "CAT_NOM_PARAMETROS";
        public const String Campo_Parametro_ID = "PARAMETRO_ID";
        public const String Campo_Zona_ID = "ZONA_ID";
        public const String Campo_Percepcion_Quinquenio = "PERCEPCION_QUINQUENIO";
        public const String Campo_Percepcion_Prima_Vacacional = "PERCEPCION_PRIMA_VACACIONAL";
        public const String Campo_Percepcion_Prima_Dominical = "PERCEPCION_PRIMA_DOMINICAL";
        public const String Campo_Percepcion_Aguinaldo = "PERCEPCION_AGUINALDO";
        public const String Campo_Percepcion_Dias_Festivos = "PERCEPCION_DIAS_FESTIVOS";
        public const String Campo_Percepcion_Horas_Extra = "PERCEPCION_HORAS_EXTRA";
        public const String Campo_Percepcion_Dia_Doble = "PERCEPCION_DIA_DOBLE";
        public const String Campo_Percepcion_Dia_Domingo = "PERCEPCION_DIA_DOMINGO";
        public const String Campo_Percepcion_Ajuste_ISR = "PERCEPCION_AJUSTE_ISR";
        public const String Campo_Percepcion_Incapacidades = "PERCEPCION_INCAPACIDADES";
        public const String Campo_Percepcion_Subsidio = "PERCEPCION_SUBSIDIO";
        public const String Campo_Percepcion_Despensa = "PERCEPCION_DESPENSA";
        public const String Campo_Percepcion_Sueldo_Normal = "PERCEPCION_SUELDO_NORMAL";
        public const String Campo_Percepcion_Prima_Antiguedad = "PERCEPCION_PRIMA_ANTIGUEDAD";
        public const String Campo_Percepcion_Indemnizacion = "PERCEPCION_INDEMNIZACION";
        public const String Campo_Percepcion_Vacaciones_Pendientes_Pagar = "PERCEPCION_VAC_PEND_PAGAR";
        public const String Campo_Percepcion_Vacaciones = "PERCEPCION_VACACIONES";
        public const String Campo_Percepcion_Fondo_Retiro = "PERCEPCION_FONDO_RETIRO";
        public const String Campo_Percepcion_Prevision_Social_Multiple = "PERCEPCION_PSM";

        public const String Campo_Deduccion_Faltas = "DEDUCCION_FALTAS";
        public const String Campo_Deduccion_Retardos = "DEDUCCION_RETARDOS";
        public const String Campo_Deduccion_Fondo_Retiro = "DEDUCCION_FONDO_RETIRO";
        public const String Campo_Deduccion_ISR = "DEDUCCION_ISR";
        public const String Campo_Deduccion_IMSS = "DEDUCCION_IMSS";
        public const String Campo_Deduccion_Tipo_Desc_Orden_Judicial = "DEDUCCION_ORDEN_JUDICIAL";
        public const String Campo_Deduccion_Vacaciones_Tomadas_Mas = "DEDUCCION_VAC_TOMADAS_MAS";
        public const String Campo_Deduccion_Aguinaldo_Pagado_Mas = "DEDUCCION_AGUINALDO_PAGADO_MAS";
        public const String Campo_Deduccion_Prima_Vacacional_Pagada_Mas = "DEDUCCION_PRIMA_VAC_PAGADA_MAS";
        public const String Campo_Deduccion_Sueldo_Pagado_Mas = "DEDUCCION_SUELDO_PAGADO_MAS";
        public const String Campo_Deduccion_Orden_Judicial_Aguinaldo = "DEDUCCION_OJ_AGUINALDO";
        public const String Campo_Deduccion_Orden_Judicial_Prima_Vacacional = "DEDUCCION_OJ_PRIMA_VACACIONAL";
        public const String Campo_Deduccion_Orden_Judicial_Indemnizacion = "DEDUCCION_OJ_INDEMNIZACION";
        public const String Campo_Deduccion_ISSEG = "DEDUCCION_ISSEG";

        public const String Campo_Porcentaje_Prima_Vacacional = "PORCENTAJE_PRIMA_VACACIONAL";
        public const String Campo_Porcentaje_Fondo_Retiro = "PORCENTAJE_FONDO_RETIRO";
        public const String Campo_Porcentaje_Prima_Dominical = "PORCENTAJE_PRIMA_DOMINICAL";
        public const String Campo_Fecha_Prima_Vacacional_1 = "FECHA_PRIMA_VACACIONAL_1";
        public const String Campo_Fecha_Prima_Vacacional_2 = "FECHA_PRIMA_VACACIONAL_2";
        public const String Campo_Salario_Limite_Prestamo = "SUELDO_LIMITE_PRESTAMO";
        public const String Campo_Salario_Mensual_Maximo = "SALARIO_MENSUAL_MAXIMO";
        public const String Campo_Salario_Diario_Int_Topado = "SALARIO_DIARIO_INT_TOPADO";

        public const String Campo_IP_Servidor = "IP_SERVIDOR";
        public const String Campo_Nombre_Base_Datos = "NOMBRE_BASE_DATOS";
        public const String Campo_Usuario_SQL = "USUARIO_SQL";
        public const String Campo_Password_Base_Datos = "PASSWORD_BASE_DATOS";

        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

        public const String Campo_Tipo_IMSS = "TIPO_IMSS";
        public const String Campo_Minutos_Dia = "MINUTOS_DIA";
        public const String Campo_Minutos_Retardo = "MINUTOS_RETARDO";
        public const String Campo_ISSEG_Porcentaje_Prevision_Social_Multiple = "PORCENTAJE_PSM";
        public const String Campo_ISSEG_Porcentaje_Aplicar_Empleado = "PORCENTAJE_FACTOR_SOCIAL";
        public const String Campo_Dias_IMSS = "DIAS_IMSS";

        public const String Campo_Tope_ISSEG = "TOPE_ISSEG";
        public const String Campo_Proveedor_Fonacot = "PROVEEDOR_FONACOT";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Emp_Movimientos_Det
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_EMP_MOVIMIENTOS_DET
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 11/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Emp_Movimientos_Det
    {
        public const String Tabla_Cat_Emp_Movimientos_Det = "CAT_EMP_MOVIMIENTOS_DET";
        public const String Campo_No_Movimiento = "NO_MOVIMIENTO";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Puesto_ID = "PUESTO_ID";
        public const String Campo_Tipo_Nomina_ID = "TIPO_NOMINA_ID";
        public const String Campo_Tipo_Movimiento = "TIPO_MOVIMIENTO";
        public const String Campo_Motivo_Movimiento = "MOTIVO_MOVIMIENTO";
        public const String Campo_Sueldo_Actual = "SUELDO_ACTUAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

        public const String Campo_Aplica_Baja_Licencia = "APLICA_BAJA_LIC";
        public const String Campo_Fecha_Inicio_Licencia = "FECHA_INICIA_LIC";
        public const String Campo_Fecha_Termino_Licencia = "FECHA_TERMINO_LIC";
        public const String Campo_Fecha_Baja_IMSS = "FECHA_BAJA_IMSS";
        public const String Campo_Tipo_Baja = "TIPO_BAJA";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Antiguedad_Sindicato
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_ANTIGUEDAD_SINDICATO
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 24/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Antiguedad_Sindicato
    {
        public const String Tabla_Cat_Nom_Antiguedad_Sindicato = "CAT_NOM_ANTIGUEDAD_SINDICATO";
        public const String Campo_Antiguedad_Sindicato_ID = "ANTIGUEDAD_SINDICATO_ID";
        public const String Campo_Anios = "ANIOS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Ant_Sin_Det 
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_ANT_SIN_DET
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 24/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Ant_Sin_Det
    {
        public const String Tabla_Cat_Nom_Ant_Sin_Det = "CAT_NOM_ANT_SIN_DET";
        public const String Campo_Sindicato_ID = "SINDICATO_ID";
        public const String Campo_Antiguedad_Sindicato_ID = "ANTIGUEDAD_SINDICATO_ID";
        public const String Campo_Monto = "MONTO";
    }
    ///****************************************************************************************************************************************************************
    ///NOMBRE: CAT_NOM_BANCOS
    ///
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla CAT_NOM_BANCOS
    ///
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA CREÓ: 16/Febrero/2011 17:33 pm.
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Cat_Nom_Bancos
    {
        public const String Tabla_Cat_Nom_Bancos = "CAT_NOM_BANCOS";
        public const String Campo_Banco_ID = "BANCO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_No_Cuenta = "NO_CUENTA";
        public const String Campo_Sucursal = "SUCURSAL";
        public const String Campo_Referencia = "REFERENCIA";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Plan_Pago = "PLAN_PAGO";
        public const String Campo_No_Meses = "NO_MESES";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Reloj_Checador
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_RELOJ_CHECADOR
    /// PARAMETROS :
    /// CREO       : Yazmin A Delgado Gómez
    /// FECHA_CREO : 14/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Reloj_Checador
    {
        public const String Tabla_Cat_Nom_Reloj_Checador = "CAT_NOM_RELOJ_CHECADOR";
        public const String Campo_Reloj_Checador_ID = "RELOJ_CHECADOR_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Ubicacion = "UBICACION";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Calendario_Reloj
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_CALENDARIO_RELOJ
    /// PARAMETROS :
    /// CREO       : Yazmin A Delgado Gómez
    /// FECHA_CREO : 16/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Calendario_Reloj
    {
        public const String Tabla_Cat_Nom_Calendario_Reloj = "CAT_NOM_CALENDARIO_RELOJ";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Fecha_Inicio = "FECHA_INICIO";
        public const String Campo_Fecha_Fin = "FECHA_FIN";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Consecutivo = "CONSECUTIVO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Nom_Parametros_Contables
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_NOM_PARAMETROS_CONTABLES
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Nom_Parametros_Contables
    {
        public const String Tabla_Cat_Nom_Parametros_Contables = "CAT_NOM_PARAMETROS_CONTABLES";
        public const String Campo_Parametro_ID = "PARAMETRO_ID";
        public const String Campo_Dietas = "DIETAS";
        public const String Campo_Sueldos_Base = "SUELDOS_BASE";
        public const String Campo_Honorarios_Asimilados = "HONORARIOS_ASIMILADOS";
        public const String Campo_Remuneraciones_Eventuales = "REMUNERACION_EVENTUALES";
        public const String Campo_Prima_Vacacional = "PRIMA_VACACIONAL";
        public const String Campo_Gratificaciones_Fin_Anio = "GRATIFICACIONES_FIN_ANIO";
        public const String Campo_Prevision_Social_Multiple = "PREVISION_SOCIAL_MULTIPLE";
        public const String Campo_Prima_Dominical = "PRIMA_DOMINICAL";
        public const String Campo_Horas_Extra = "HORAS_EXTRA";
        public const String Campo_Participacipaciones_Vigilancia = "PARTICIPACIONES_VIGILANCIA";
        public const String Campo_Aportaciones_ISSEG = "APORTACIONES_ISSEG";
        public const String Campo_Aportaciones_IMSS = "APORTACIONES_IMSS";
        public const String Campo_Cuotas_Fondo_Retiro = "FONDO_AHORRO";
        public const String Campo_Prestaciones_Establecidas_Condiciones_Trabajo = "PRES_EST_COND_TRABAJO";
        public const String Campo_Prestaciones = "PRESTACIONES";
        public const String Campo_Estimulos_Productividad_Eficiencia = "ESTIMULOS_PRODUCTIVIDAD";
        public const String Campo_Impuestos_Sobre_Nominas = "IMPUESTOS_SOBRE_NOMINAS";
        public const String Campo_Pensiones = "PENSIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Honorarios = "HONORARIOS";
        public const String Campo_Seguros = "SEGUROS";
        public const String Campo_Liquidacion_Indemnizacion = "LIQUIDACION_INDEMNIZACION";
        public const String Campo_Prestaciones_Retiro = "PRESTACIONES_RETIRO";
    }
    #endregion

    #region (Operacion [Control Incidencias de los Empleados Presidencia])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Asistencias
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_ASISTENCIAS
    /// PARAMETROS :
    /// CREO       : Yazmin A Delgado Gómez
    /// FECHA_CREO : 14/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Asistencias
    {
        public const String Tabla_Ope_Nom_Asistencias = "OPE_NOM_ASISTENCIAS";
        public const String Campo_No_Asistencia = "NO_ASISTENCIA";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Reloj_Checador_ID = "RELOJ_CHECADOR_ID";
        public const String Campo_Fecha_Hora_Entrada = "FECHA_HORA_ENTRADA";
        public const String Campo_Fecha_Hora_Salida = "FECHA_HORA_SALIDA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Incidencias_Checadas
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_INCIDENCIAS_CHECADAS
    /// PARAMETROS :
    /// CREO       : Yazmin A Delgado Gómez
    /// FECHA_CREO : 04/Agosto/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Incidencias_Checadas
    {
        public const String Tabla_Ope_Nom_Incidencias_Checadas = "OPE_NOM_INCIDENCIAS_CHECADAS";
        public const String Campo_No_Incidencia_Checada = "NO_INCIDENCIA_CHECADA";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Reloj_Checador_ID = "RELOJ_CHECADOR_ID";
        public const String Campo_Fecha_Hora_Checada = "FECHA_HORA_CHECADA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Horarios_Empleados
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_ASISTENCIAS
    /// PARAMETROS :
    /// CREO       : Yazmin A Delgado Gómez
    /// FECHA_CREO : 14/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Horarios_Empleados
    {
        public const String Tabla_Ope_Nom_Horarios_Empleados = "OPE_NOM_HORARIOS_EMPLEADOS";
        public const String Campo_No_Horario_Empleado = "NO_HORARIO_EMPLEADO";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Fecha_Inicio = "FECHA_INICIO";
        public const String Campo_Fecha_Termino = "FECHA_TERMINO";
        public const String Campo_Hora_Entrada = "HORA_ENTRADA";
        public const String Campo_Hora_Salida = "HORA_SALIDA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///****************************************************************************************************************************************************************
    ///NOMBRE: Ope_Nom_Nomina_Generada
    ///
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla OPE_NOM_NOMINA_GENERADA
    ///
    ///CREO: Juan Alberto Hernández Negrete.
    ///FECHA CREÓ: 12/Junio/2011.
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Ope_Nom_Nomina_Generada
    {
        public const String Tabla_Ope_Nom_Nomina_Generada = "OPE_NOM_NOMINA_GENERADA";
        public const String Campo_Nom_Generada_ID = "NOM_GENERADA_ID";
        public const String Campo_Tipo_Nomina_ID = "TIPO_NOMINA_ID";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Nomina_Generada = "NOMINA_GENERADA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
    }
    ///****************************************************************************************************************************************************************
    ///NOMBRE: Ope_Nom_Generar_Arch_Bancos
    ///
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla OPE_NOM_GENERAR_ARCH_BANCOS
    ///
    ///CREO: Juan Alberto Hernández Negrete.
    ///FECHA CREÓ: 20/Abril/2011.
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Ope_Nom_Generar_Arch_Bancos
    {
        public const String Tabla_Ope_Nom_Generar_Arch_Bancos = "OPE_NOM_GENERAR_ARCH_BANCOS";
        public const String Campo_No_Movimiento = "NO_MOVIMIENTO";
        public const String Campo_Banco_ID = "BANCO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///****************************************************************************************************************************************************************
    ///NOMBRE: Cat_Nom_Plazas
    ///
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla CAT_NOM_PLAZAS
    ///
    ///CREO: Francisco Antonio Gallardo Castañeda
    ///FECHA CREÓ: 04/Abril/2011 11:21 am.
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Cat_Nom_Plazas
    {
        public const String Tabla_Cat_Nom_Plazas = "CAT_NOM_PLAZAS";
        public const String Campo_Plaza_ID = "PLAZA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///****************************************************************************************************************************************************************
    ///NOMBRE: Ope_Nom_Actualizar_Salario
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla OPE_NOM_ACTUALIZAR_SALARIO
    ///CREO: Francisco Antonio Gallardo Castañeda
    ///FECHA CREÓ: 06/Abril/2011 4:43 pm.
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Ope_Nom_Actualizar_Salario
    {
        public const String Tabla_Ope_Nom_Actualizar_Salario = "OPE_NOM_ACTUALIZAR_SALARIO";
        public const String Campo_No_Actualizar_Salario = "NO_ACTUALIZAR_SALARIO";
        public const String Campo_Tipo_Nomina_ID = "TIPO_NOMINA_ID";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Fecha_Actualizacion = "FECHA_ACTUALIZACION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///****************************************************************************************************************************************************************
    ///NOMBRE: Ope_Nom_Act_Sal_Det
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla OPE_NOM_ACT_SAL_DET
    ///CREO: Francisco Antonio Gallardo Castañeda
    ///FECHA CREÓ: 06/Abril/2011 4:51 pm.
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Ope_Nom_Act_Sal_Det
    {
        public const String Tabla_Ope_Nom_Act_Sal_Det = "OPE_NOM_ACT_SAL_DET";
        public const String Campo_Act_Salario_Detalle_ID = "ACT_SALARIO_DETALLE_ID";
        public const String Campo_No_Actualizar_Salario = "NO_ACTUALIZAR_SALARIO";
        public const String Campo_Sindicato_ID = "SINDICATO_ID";
    }
    ///****************************************************************************************************************************************************************
    ///NOMBRE: Ope_Nom_Proveedores
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla OPE_NOM_PROVEEDORES
    ///CREO: Francisco Antonio Gallardo Castañeda
    ///FECHA CREÓ: 22/Abril/2011 8:19 am.
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Ope_Nom_Proveedores
    {
        public const String Tabla_Ope_Nom_Proveedores = "OPE_NOM_PROVEEDORES";
        public const String Campo_No_Movimiento = "NO_MOVIMIENTO";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina_Inicia = "NO_NOMINA_INICIA";
        public const String Campo_No_Periodos = "NO_PERIODOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Fecha_Autorizacion = "FECHA_AUTORIZACION";
    }

    ///****************************************************************************************************************************************************************
    ///NOMBRE: Ope_Nom_Proveedores_Detalles
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla OPE_NOM_PROVEEDORES_DETALLES
    ///CREO: Francisco Antonio Gallardo Castañeda
    ///FECHA CREÓ: 22/Abril/2011 8:27 am.
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Ope_Nom_Proveedores_Detalles
    {
        public const String Tabla_Ope_Nom_Proveedores_Detalles = "OPE_NOM_PROVEEDORES_DETALLES";
        public const String Campo_No_Movimiento_Detalle = "NO_MOVIMIENTO_DETALLE";
        public const String Campo_No_Movimiento = "NO_MOVIMIENTO";
        public const String Campo_No_Fonacot = "NO_FONACOT";
        public const String Campo_RFC = "RFC";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_No_Credito = "NO_CREDITO";
        public const String Campo_Retencion_Mensual = "RETENCION_MENSUAL";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Plazo = "PLAZO";
        public const String Campo_Cuotas_Pagadas = "CUOTAS_PAGADAS";
        public const String Campo_Retencion_Real = "RETENCION_REAL";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_Periodo = "PERIODO";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Estatus = "ESTATUS";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Puestos
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_PUESTOS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Puestos
    {
        public const String Tabla_Cat_Puestos = "CAT_PUESTOS";
        public const String Campo_Puesto_ID = "PUESTO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Salario_Mensual = "SALARIO_MENSUAL";
        public const String Campo_Plaza_ID = "PLAZA_ID";
        public const String Campo_Aplica_Fondo_Retiro = "APLICA_FONDO_RETIRO";
        public const String Campo_Aplica_PSM = "APLICA_PSM";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Domingos
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_DOMINGOS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 03/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Domingos_Empleado
    {
        public const String Tabla_Cat_Nom_Ope_Nom_Domingos = "OPE_NOM_DOMINGOS";
        public const String Campo_No_Domingo = "NO_DOMINGO";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Domingos_Empleado_Detalles
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_DOMINGOS_EMP_DET
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 12/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Domingos_Empleado_Detalles
    {
        public const String Tabla_Ope_Nom_Domingos_Empleados_Detalles = "OPE_NOM_DOMINGOS_EMP_DET";
        public const String Campo_No_Domingo = "NO_DOMINGO";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios_Estatus = "COMENTARIOS_ESTATUS";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Faltas_Empleado
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_FALTAS_EMPLEADO
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 03/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Faltas_Empleado
    {
        public const String Tabla_Ope_Nom_Faltas_Empleado = "OPE_NOM_FALTAS_EMPLEADO";
        public const String Campo_No_Falta = "NO_FALTA";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Tipo_Falta = "TIPO_FALTA";
        public const String Campo_Retardo = "RETARDO";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Estatus = "ESTATUS";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Tiempo_Extra
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_TIEMPO_EXTRA
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 03/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Tiempo_Extra
    {
        public const String Tabla_Ope_Nom_Tiempo_Extra = "OPE_NOM_TIEMPO_EXTRA";
        public const String Campo_No_Tiempo_Extra = "NO_TIEMPO_EXTRA";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Pago_Dia_Doble = "PAGO_DIA_DOBLE";
        public const String Campo_Horas = "HORAS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Tiempo_Extra_Emp_Det
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_TIEMPO_EXTRA_EMP_DET
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 18/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Tiempo_Extra_Emp_Det
    {
        public const String Tabla_Ope_Nom_Tiempo_Extra = "OPE_NOM_TIEMPO_EXTRA_EMP_DET";
        public const String Campo_No_Tiempo_Extra = "NO_TIEMPO_EXTRA";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios_Estatus = "COMENTARIOS_ESTATUS";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Dias_Festivos_Empleado
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_DIAS_FESTIVOS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 03/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Dias_Festivos
    {
        public const String Tabla_Ope_Nom_Dias_Festivos = "OPE_NOM_DIAS_FESTIVOS";
        public const String Campo_No_Dia_Festivo = "NO_DIA_FESTIVO";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Dia_ID = "DIA_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Dias_Festivo_Emp_Det 
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_DIAS_FESTIVO_EMP_DET
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 22/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Dias_Festivo_Emp_Det
    {
        public const String Tabla_Ope_Nom_Dias_Festivos_Emp_Det = "OPE_NOM_DIAS_FESTIVO_EMP_DET";
        public const String Campo_No_Dia_Festivo = "NO_DIA_FESTIVO";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios_Estatus = "COMENTARIOS_ESTATUS";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Vacaciones_Empleado
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_VACACIONES_EMPLEADO
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 03/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class OPE_NOM_VACACIONES_EMPLEADO
    {
        public const String Tabla_Ope_Nom_Vacaciones_Empleado = "OPE_NOM_VACACIONES_EMPLEADO";
        public const String Campo_No_Vacacion = "NO_VACACION";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Fecha_Inicio = "FECHA_INICIO";
        public const String Campo_Fecha_Termino = "FECHA_TERMINO";
        public const String Campo_Cantidad_Dias = "CANTIDAD_DIAS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios_Estatus = "COMENTARIOS_ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Fecha_Regreso_Laboral = "FECHA_REGRESO_VACACIONAL";
        public const String Campo_Estado = "ESTADO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Vacaciones_Empleado_Detalles 
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_VACACIONES_EMPL_DET
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernández Negrete
    /// FECHA_CREO : 1/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Vacaciones_Empleado_Detalles
    {
        public const String Tabla_Ope_Nom_Vacaciones_Empl_Det = "OPE_NOM_VACACIONES_EMPL_DET";
        public const String Campo_No_Vacacion_Detalle = "NO_VACACION_DETALLE";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Dias_Disponibles = "DIAS_DISPONIBLES";
        public const String Campo_Dias_Tomados = "DIAS_TOMADOS";
        public const String Campo_Periodo_Vacacional = "PERIODO_VACACIONAL";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Vacaciones_Dia_Det 
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_VACACIONES_DIA_DET
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 30/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Vacaciones_Dia_Det
    {
        public const String Tabla_Ope_Nom_Vacaciones_Dia_Det = "OPE_NOM_VACACIONES_DIA_DET";
        public const String Campo_No_Dia_Vacacion = "NO_DIA_VACACION";
        public const String Campo_No_Vacacion = "NO_VACACION";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Estado = "ESTADO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Percepciones_Var
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_PERCEPCIONES_VAR
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 28/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Percepciones_Var
    {
        public const String Tabla_Ope_Nom_Percepciones_Var = "OPE_NOM_PERCEPCIONES_VAR";
        public const String Campo_No_Percepcion = "NO_PERCEPCION";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Perc_Var_Emp_Det
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_PERC_VAR_EMP_DET
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 28/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Perc_Var_Emp_Det
    {
        public const String Tabla_Ope_Nom_Perc_Var_Emp_Det = "OPE_NOM_PERC_VAR_EMP_DET";
        public const String Campo_No_Percepcion = "NO_PERCEPCION";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios_Estatus = "COMENTARIOS_ESTATUS";
        public const String Campo_Cantidad = "CANTIDAD";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Deducciones_Var
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_DEDUCCIONES_VAR
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Deducciones_Var
    {
        public const String Tabla_Ope_Nom_Deducciones_Var = "OPE_NOM_DEDUCCIONES_VAR";
        public const String Campo_No_Deduccion = "NO_DEDUCCION";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Referencia = "REFERENCIA";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Deduc_Var_Emp_Det 
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_DEDUC_VAR_EMP_DET
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Deduc_Var_Emp_Det
    {
        public const String Tabla_Ope_Nom_Deduc_Var_Emp_Det = "OPE_NOM_DEDUC_VAR_EMP_DET";
        public const String Campo_No_Deduccion = "NO_DEDUCCION";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios_Estatus = "COMENTARIOS_ESTATUS";
        public const String Campo_Cantidad = "CANTIDAD";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Requisitos_Empleado
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_REQUISITOS_EMPLEADO
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 27/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///******************************************************************************
    public class Ope_Nom_Requisitos_Empleado
    {
        public const String Tabla_Ope_Nom_Requisitos_Empleado = "OPE_NOM_REQUISITOS_EMPLEADO";
        public const String Campo_Requisitos_Empleado_ID = "REQUISITO_EMPLEADO_ID";
        public const String Campo_Requisitos_ID = "REQUISITO_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Ruta_Documento = "RUTA_DOCUMENTO";
        public const String Campo_Entregado = "ENTREGADO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Recibos_Empleados
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_RECIBOS_EMPLEADOS
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 14/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Recibos_Empleados
    {
        public const String Tabla_Ope_Nom_Recibos_Empleados = "OPE_NOM_RECIBOS_EMPLEADOS";
        public const String Campo_No_Recibo = "NO_RECIBO";
        public const String Campo_Detalle_Nomina_ID = "DETALLE_NOMINA_ID";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Tipo_Nomina_ID = "TIPO_NOMINA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Puesto_ID = "PUESTO_ID";
        public const String Campo_Dias_Trabajados = "DIAS_TRABAJADOS";
        public const String Campo_Total_Percepciones = "TOTAL_PERCEPCIONES";
        public const String Campo_Total_Deducciones = "TOTAL_DEDUCCIONES";
        public const String Campo_Total_Nomina = "TOTAL_NOMINA";
        public const String Campo_Gravado = "GRAVADO";
        public const String Campo_Exento = "EXENTO";
        public const String Campo_Salario_Diario = "SALARIO_DIARIO";
        public const String Campo_Salario_Diario_Integrado = "SALARIO_DIARIO_INTEGRADO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Nomina_Generada = "NOMINA_GENERADA";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Recibos_Empleados_Det 
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_RECIBOS_EMPLEADOS_DET
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 14/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Recibos_Empleados_Det
    {
        public const String Tabla_Ope_Nom_Recibos_Empleados_Det = "OPE_NOM_RECIBO_EMPLEADO_DET";
        public const String Campo_No_Recibo_Detalles = "NO_RECIBO_DETALLES";
        public const String Campo_No_Recibo = "NO_RECIBO";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Gravado = "GRAVADO";
        public const String Campo_Exento = "EXENTO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Totales_Nomina
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_TOTALES_NOMINA
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 14/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Totales_Nomina
    {
        public const String Tabla_Ope_Nom_Totales_Nomina = "OPE_NOM_TOTALES_NOMINA";
        public const String Campo_No_Total_Nomina = "NO_TOTAL_NOMINA";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_Detalle_Nomina_ID = "DETALLE_NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Tipo_Nomina_ID = "TIPO_NOMINA_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Solicitud_Prestamo 
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_SOLICITUD_PRESTAMO
    /// PARAMETROS :
    /// CREO       : Juan ALberto Hernandez Negrete
    /// FECHA_CREO : 30/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Solicitud_Prestamo
    {
        public const String Tabla_Ope_Nom_Solicitud_Prestamo = "OPE_NOM_SOLICITUD_PRESTAMO";
        public const String Campo_No_Solicitud = "NO_SOLICITUD";
        public const String Campo_Solicita_Empleado_ID = "SOLICITA_EMPLEADO_ID";
        public const String Campo_Aval_Empleado_ID = "AVAL_EMPLEADO_ID";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Estatus_Solicitud = "ESTATUS_SOLICITUD";
        public const String Campo_Estatus_Pago = "ESTATUS_PAGO";
        public const String Campo_Fecha_Solicitud = "FECHA_SOLICITUD";
        public const String Campo_Fecha_Autorizacion = "FECHA_AUTORIZACION";
        public const String Campo_Fecha_Inicio_Pago = "FECHA_INICIO_PAGO";
        public const String Campo_Fecha_Termino_Pago = "FECHA_TERMINO_PAGO";
        public const String Campo_Motivo_Prestamo = "MOTIVO_PRESTAMO";
        public const String Campo_No_Pagos = "NO_PAGOS";
        public const String Campo_Importe_Prestamo = "IMPORTE_PRESTAMO";
        public const String Campo_Importe_Interes = "IMPORTE_INTERES";
        public const String Campo_Total_Prestamo = "TOTAL_PRESTAMO";
        public const String Campo_Monto_Abonado = "MONTO_ABONADO";
        public const String Campo_Saldo_Actual = "SALDO_ACTUAL";
        public const String Campo_Abono = "ABONO";
        public const String Campo_No_Abono = "NO_ABONO";
        public const String Campo_Comentarios_Cancelacion = "COMENTARIOS_CANCELACION";
        public const String Campo_Comentarios_Rechazo = "COMENTARIOS_RECHAZO";
        public const String Campo_Referencia_Recibo_Pago = "REFERENCIA_RECIBO_PAGO";
        public const String Campo_Aplica_Validaciones = "APLICA_VALIDACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Fecha_Finiquito_Prestamo = "FECHA_FINIQUITO_PRESTAMO";
        public const String Campo_Estado_Prestamo = "ESTADO_PRESTAMO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Ajuste_ISR
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_AJUSTE_ISR
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 14/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Ajuste_ISR
    {
        public const String Tabla_Ope_Nom_Ajuste_ISR = "OPE_NOM_AJUSTE_ISR";
        public const String Campo_No_Ajuste_ISR = "NO_AJUSTE_ISR";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Percepcion_Deduccion_ID = "PERCEPCION_DEDUCCION_ID";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Estatus_Ajuste_ISR = "ESTATUS_AJUSTE_ISR";
        public const String Campo_Fecha_Inicio_Pago = "FECHA_INICIO_PAGO";
        public const String Campo_Fecha_Termino_Pago = "FECHA_TERMINO_PAGO";
        public const String Campo_Total_ISR_Ajustar = "TOTAL_ISR_AJUSTAR";
        public const String Campo_Total_ISR_Ajustado = "TOTAL_ISR_AJUSTADO";
        public const String Campo_No_Catorcenas = "NO_CATORCENAS";
        public const String Campo_Pago_Catorcenal_ISR = "PAGO_CATORCENAL_ISR";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_Comentarios_Ajuste = "COMENTARIOS_AJUSTE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Incapacidades
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_INCAPACIDADES
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Incapacidades
    {
        public const String Tabla_Ope_Nom_Incapacidades = "OPE_NOM_INCAPACIDADES";
        public const String Campo_No_Incapacidad = "NO_INCAPACIDAD";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Tipo_Incapacidad = "TIPO_INCAPACIDAD";
        public const String Campo_Aplica_Pago_Cuarto_Dia = "APLICA_PAGO_CUARTO_DIA";
        public const String Campo_Porcentaje_Incapacidad = "PORCENTAJE_INCAPACIDAD";
        public const String Campo_Extencion_Incapacidad = "EXTENCION_INCAPACIDAD";
        public const String Campo_Fecha_Inicio = "FECHA_INICIO";
        public const String Campo_Fecha_Fin = "FECHA_FIN";
        public const String Campo_Comentario = "COMENTARIOS";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Dias_Incapacidad = "DIAS_INCAPACIDAD";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Nom_Reloj_Checador
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_NOM_RELOJ_CHECADOR
    /// PARAMETROS :
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 19/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Nom_Reloj_Checador
    {
        public const String Tabla_Ope_Nom_Reloj_Checador = "OPE_NOM_RELOJ_CHECADOR";
        public const String Campo_No_Movimiento = "NO_MOVIMIENTO";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Fecha_Subio_Informacion = "FECHA_SUBIO_INF";
        public const String Campo_Nomina_ID = "NOMINA_ID";
        public const String Campo_No_Nomina = "NO_NOMINA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Dias_Incapacidad = "DIAS_INCAPACIDAD";
    }
    #endregion

    #region (Tabuladores ISR, Subsidio, Antiguedad, Vacaciones, Dias Festivos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: His_Nom_Antiguedad_Empleados
    /// DESCRIPCION: Clase con contiene los datos de la tabla HIS_NOM_ANTIGUEDAD_EMPLEADOS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class His_Nom_Antiguedad_Empleados
    {
        public const String Tabla_His_Nom_Antiguedad_Empleados = "HIS_NOM_ANTIGUEDAD_EMPLEADOS";
        public const String Campo_No_Antiguedad = "NO_ANTIGUEDAD";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Antiguedad = "ANTIGUEDAD";
        public const String Campo_Fecha_Inicio = "FECHA_INICIO";
        public const String Campo_Fecha_Fin = "FECHA_FIN";
        public const String Campo_Dias_Tomados = "DIAS_TOMADOS";
        public const String Campo_Dias_Restan = "DIAS_RESTAN";
        public const String Campo_Dias_Tocan = "DIAS_TOCAN";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: His_Nom_Aumento_Sueldo
    /// DESCRIPCION: Clase con contiene los datos de la tabla HIS_NOM_AUMENTO_SUELDO
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class His_Nom_Aumento_Sueldo
    {
        public const String Tabla_His_Nom_Aumento_Sueldo = "HIS_NOM_AUMENTO_SUELDO";
        public const String Campo_No_Historial = "NO_HISTORIAL";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Salario_Diario = "SALARIO_DIARIO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Tab_Nom_Subsidio
    /// DESCRIPCION: Clase con contiene los datos de la tabla TAB_NOM_SUBSIDIO
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Tab_Nom_Subsidio
    {
        public const String Tabla_Tab_Nom_Subsidio = "TAB_NOM_SUBSIDIO";
        public const String Campo_Subsidio_ID = "SUBSIDIO_ID";
        public const String Campo_Limite_Inferior = "LIMITE_INFERIOR";
        public const String Campo_Subsidio = "SUBSIDIO";
        public const String Campo_Tipo_Nomina = "TIPO_NOMINA";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Tab_Nom_ISR
    /// DESCRIPCION: Clase con contiene los datos de la tabla TAB_NOM_ISR
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Tab_Nom_ISR
    {
        public const String Tabla_Tab_Nom_ISR = "TAB_NOM_ISR";
        public const String Campo_ISR_ID = "ISR_ID";
        public const String Campo_Limite_Inferior = "LIMITE_INFERIOR";
        public const String Campo_Cuota_Fija = "CUOTA_FIJA";
        public const String Campo_Porcentaje = "PORCENTAJE";
        public const String Campo_Tipo_Nomina = "TIPO_NOMINA";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Tab_Nom_INFONAVIT
    /// DESCRIPCION: Clase con contiene los datos de la tabla TAB_NOM_INFONAVIT
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Tab_Nom_INFONAVIT
    {
        public const String Tabla_Tab_Nom_INFONAVIT = "TAB_NOM_INFONAVIT";
        public const String Campo_INFONAVIT_ID = "INFONAVIT_ID";
        public const String Campo_Veces_SMGA = "VECES_SMGA";
        public const String Campo_Porcentaje = "PORCENTAJE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Tab_Nom_IMSS
    /// DESCRIPCION: Clase con contiene los datos de la tabla TAB_NOM_IMSS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Tab_Nom_IMSS
    {
        public const String Tabla_Tab_Nom_IMSS = "TAB_NOM_IMSS";
        public const String Campo_IMSS_ID = "IMSS_ID";
        public const String Campo_Porcentaje_Enf_Mat_Esp = "PORCENTAJE_ENF_MAT_ESP";
        public const String Campo_Porcentaje_Enf_Mat_Pes = "PORCENTAJE_ENF_MAT_PES";
        public const String Campo_Porcentaje_Invalidez_Vida = "PORCENTAJE_INVALIDEZ_VIDA";
        public const String Campo_Porcentaje_Cesantia_Vejez = "PORCENTAJE_CESANTIA_VEJEZ";
        public const String Campo_Excendete_3_SMG_DF = "EXCEDENTE_3_SMG_DF";
        public const String Campo_Prestaciones_Dinero = "PRESTACIONES_DINERO";
        public const String Campo_Gastos_Medicos = "GASTOS_MEDICOS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Tab_Nom_Vacaciones
    /// DESCRIPCION: Clase con contiene los datos de la tabla TAB_NOM_VACACIONES
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Tab_Nom_Vacaciones
    {
        public const String Tabla_Tab_Nom_Vacaciones = "TAB_NOM_VACACIONES";
        public const String Campo_Vacacion_ID = "VACACION_ID";
        public const String Campo_Antiguedad = "ANTIGUEDAD";
        public const String Campo_Dias = "DIAS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Tab_Nom_Dias_Festivos
    /// DESCRIPCION: Clase con contiene los datos de la tabla TAB_NOM_DIAS_FESTIVOS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 19/Agosto/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Tab_Nom_Dias_Festivos
    {
        public const String Tabla_Tab_Nom_Dias_Festivos = "TAB_NOM_DIAS_FESTIVOS";
        public const String Campo_Dia_ID = "DIA_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Nomina_ID = "NOMINA_ID";
    }
    #endregion

    #endregion

    ///*************************************************************************************************************************
    ///                                                                COMPRAS
    ///*************************************************************************************************************************

    #region Compras

    ///*************************************************************************************************************************
    ///                                                                SubRegion Compras
    ///*************************************************************************************************************************
    #region SubRegion_Compras
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Poliza_Stock_Para_Sap
    /// DESCRIPCION:           Clase que contiene los campos de la tabla Poliza_Stock_Para_Sap 
    /// PARAMETROS :     
    /// CREO       :           Gustavo Angeles Cruz
    /// FECHA_CREO :           27/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Poliza_Stock_Para_Sap
    {
        public const String Tabla_Poliza_Stock_Para_Sap = "POLIZA_STOCK_PARA_SAP";
        public const String Campo_No_Poliza_Stock = "NO_POLIZA_STOCK";
        public const String Campo_Salidas = "SALIDAS";
        public const String Campo_Hora = "HORA";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Directores
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_DIRECTORES 
    /// PARAMETROS :     
    /// CREO       :           Jesus Toledo Rodriguez
    /// FECHA_CREO :           06/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Com_Directores
    {
        public const String Tabla_Cat_Com_Directores = "CAT_COM_DIRECTORES";
        public const String Campo_Director_Adquisiciones = "DIRECTOR_ADQUISICIONES";
        public const String Campo_Oficial_Mayor = "OFICIAL_MAYOR";
        public const String Campo_Tesorero = "TESORERO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Com_Historial_Req
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla
    /// PARÁMETROS :     
    /// CREO       : Gustavo Angeles Cruz
    /// FECHA_CREO : 01/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Historial_Req
    {
        public const String Campo_No_Historial = "NO_HISTORIAL";
        public const String Campo_No_Requisicion = "NO_REQUISICION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Empleado = "EMPLEADO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Tabla_Ope_Com_Historial_Req = "OPE_COM_HISTORIAL_REQ";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Proveedores
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_PROVEEDORES
    /// PARAMETROS :     
    /// CREO       :           Noe Mosqueda Valadez
    /// FECHA_CREO :           27/Septiembre/2010 12:16 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Com_Proveedores
    {
        public const String Tabla_Cat_Com_Proveedores = "CAT_COM_PROVEEDORES";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Giro_ID = "GIRO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Compañia = "COMPANIA";
        public const String Campo_RFC = "RFC";
        public const String Campo_Contacto = "CONTACTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Direccion = "DIRECCION";
        public const String Campo_Colonia = "COLONIA";
        public const String Campo_Ciudad = "CIUDAD";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_CP = "CODIGO_POSTAL";
        public const String Campo_Telefono_1 = "TELEFONO1";
        public const String Campo_Telefono_2 = "TELEFONO2";
        public const String Campo_Nextel = "NEXTEL";
        public const String Campo_Fax = "FAX";
        public const String Campo_Correo_Electronico = "E_MAIL";
        public const String Campo_Password = "PASSWORD";
        public const String Campo_Tipo_Pago = "TIPO_PAGO";
        public const String Campo_Dias_Credito = "DIAS_CREDITO";
        public const String Campo_Forma_Pago = "FORMA_PAGO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario = "USUARIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Cuenta = "CUENTA";
        public const String Campo_Actualizacion = "ACTUALIZACION";
        public const String Campo_Fecha_Actualizacion = "FECHA_ACTUALIZACION";
        public const String Campo_Representante_Legal = "REPRESENTANTE_LEGAL";
        public const String Campo_Fecha_Registro = "FECHA_DE_REGISTRO";
        public const String Campo_Tipo_Fiscal = "TIPO_FISCAL";
        public const String Campo_Rol_ID = "ROL_ID";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Tipo = "TIPO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    OPE_COM_HIS_AUTOR_PROV
    /// DESCRIPCION:           Clase que contiene los campos de la tabla OPE_COM_HIS_AUTOR_PROV
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :           8/NOV/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Ope_Com_His_Autor_Prov
    {
        public const String Tabla_Ope_Com_His_Autor_Prov = "OPE_COM_HIS_AUTOR_PROV";
        public const String Campo_Historial_ID = "HISTORIAL_ID";
        public const String Campo_Fecha_Actualizacion = "FECHA_ACTUALIZACION";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";


    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Com_Det_Part_Prov
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_DET_PART_PROV 
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :           8/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Com_Det_Part_Prov
    {
        public const String Tabla_Cat_Com_Det_Part_Prov = "CAT_COM_DET_PART_PROV";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Partida_Generica_ID = "PARTIDA_GENERICA_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Familias
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_FAMILIAS
    /// PARAMETROS :     
    /// CREO       :           Luz Verónica Gómez García
    /// FECHA_CREO :           27/Septiembre/2010 4:47 PM 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Com_Familias
    {
        public const String Tabla_Cat_Com_Familias = "CAT_COM_FAMILIAS";
        public const String Campo_Familia_ID = "FAMILIA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Abreviatura = "ABREVIATURA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Documentos
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_DOCUMENTOS
    /// PARAMETROS :     
    /// CREO       :           José Antonio López Hernández
    /// FECHA_CREO :           04/Enero/2011 16:29
    /// MODIFICO          :    Salvador Hernández Ramírez
    /// FECHA_MODIFICO    :    15/Marzo/2011
    /// CAUSA_MODIFICACION:    Se agregó el campo "TIPO"
    ///*******************************************************************************/
    public class Cat_Com_Documentos
    {
        public const String Tabla_Cat_Com_Documentos = "CAT_COM_DOCUMENTOS";
        public const String Campo_Documento_ID = "DOCUMENTO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Tipo = "TIPO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Impuestos
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_IMPUESTOS
    /// PARAMETROS :     
    /// CREO       :           Luz Verónica Gómez García
    /// FECHA_CREO :           27/Septiembre/2010 5:00 PM 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Com_Impuestos
    {
        public const String Tabla_Cat_Impuestos = "CAT_COM_IMPUESTOS";
        public const String Campo_Impuesto_ID = "IMPUESTO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Porcentaje_Impuesto = "PORCENTAJE_IMPUESTO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Marcas_Productos
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_MARCAS_PRODUCTOS
    /// PARAMETROS :     
    /// CREO       :           Luz Verónica Gómez García
    /// FECHA_CREO :           27/Septiembre/2010 05:20 PM
    /// MODIFICO          :     
    /// FECHA_MODIFICO    :     
    /// CAUSA_MODIFICACION:     
    ///*******************************************************************************/
    public class Cat_Com_Marcas_Productos
    {
        public const String Tabla_Cat_Com_Marcas_Productos = "CAT_COM_MARCAS_PRODUCTOS";
        public const String Campo_Marca_ID = "MARCA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Modelos_Productos
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_MODELOS_PRODUCTOS
    /// PARAMETROS :     
    /// CREO       :           Luz Verónica Gómez García
    /// FECHA_CREO :           27/Septiembre/2010 5:35 PM
    /// MODIFICO          :    Salvador Hernández Ramírez
    /// FECHA_MODIFICO    :    18/Diciembre/ 2010
    /// CAUSA_MODIFICACION:    Le agregué las constantes "Campo_Costo" y "Campo_Producto_ID", "Campo_Proveedor_ID" , "Campo_Marca_ID"  y "Ruta_Foto" ya que son necesarias para realizar la consulta a la BD 
    ///*******************************************************************************/
    public class Cat_Com_Modelos_Productos
    {
        public const String Tabla_Cat_Com_Productos = "CAT_COM_PRODUCTOS";
        public const String Campo_Modelo_ID = "MODELO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Costo = "COSTO";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Marca_ID = "MARCA_ID";
        public const String Campo_Ruta_Foto = "RUTA_FOTO";
        public const String Campo_Modelo = "MODELO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Marcas
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_MARCAS_PRODUCTOS
    /// PARAMETROS :     
    /// CREO       :           Luz Verónica Gómez García
    /// FECHA_CREO :           27/Septiembre/2010 05:20 PM
    /// MODIFICO          :     José Antonio López Hernández
    /// FECHA_MODIFICO    :     07/Enero/2010 16:52
    /// CAUSA_MODIFICACION:     Agregar el campo de Estatus
    ///*******************************************************************************/
    public class Cat_Com_Marcas
    {
        public const String Tabla_Cat_Com_Marcas = "CAT_COM_MARCAS";
        public const String Campo_Marca_ID = "MARCA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Modelos
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_MODELOS_PRODUCTOS
    /// PARAMETROS :     
    /// CREO       :           Luz Verónica Gómez García
    /// FECHA_CREO :           27/Septiembre/2010 5:35 PM
    /// MODIFICO          : Salvador Hernandez Ramirez
    /// FECHA_MODIFICO    : 12/01/2011
    /// CAUSA_MODIFICACION: Se agregaron las constantes "Campo_Estatus y Campo_Subfamilia_ID"
    ///*******************************************************************************/
    public class Cat_Com_Modelos
    {
        public const String Tabla_Cat_Com_Modelos = "CAT_COM_MODELOS";
        public const String Campo_Modelo_ID = "MODELO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Subfamilia_ID = "SUBFAMILIA_ID";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Subfamilia_ID = "SUBFAMILIA_ID";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Productos
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_PRODUCTOS
    /// PARAMETROS :     
    /// CREO       :           Luz Verónica Gómez García
    /// FECHA_CREO :           27/Septiembre/2010 6:12 PM
    /// MODIFICO          :     Jesus Toledo Rdz
    /// FECHA_MODIFICO    :     04-Abril-2011
    /// CAUSA_MODIFICACION:     Modificaciones a la tabla debido a requerimientos
    ///*******************************************************************************/
    public class Cat_Com_Productos
    {
        public const String Tabla_Cat_Com_Productos = "CAT_COM_PRODUCTOS";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Impuesto_ID = "IMPUESTO_ID";
        public const String Campo_Impuesto_2_ID = "IMPUESTO_2_ID";
        public const String Campo_Familia_ID = "FAMILIA_ID";
        public const String Campo_Subfamilia_ID = "SUBFAMILIA_ID";
        public const String Campo_Modelo_ID = "MODELO_ID";
        public const String Campo_Marca_ID = "MARCA_ID";
        public const String Campo_Unidad_ID = "UNIDAD_ID";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Giro_ID = "GIRO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Stock = "STOCK";
        public const String Campo_Resguardo = "RESGUARDO";
        public const String Campo_Partida_Especifica_ID = "PARTIDA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Costo = "COSTO";
        public const String Campo_Costo_Promedio = "COSTO_PROMEDIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Existencia = "EXISTENCIA";
        public const String Campo_Comprometido = "COMPROMETIDO";
        public const String Campo_Disponible = "DISPONIBLE";
        public const String Campo_Maximo = "MAXIMO";
        public const String Campo_Minimo = "MINIMO";
        public const String Campo_Reorden = "REORDEN";
        public const String Campo_Ubicacion = "UBICACION";
        //public const String Campo_Fecha_Ultimo_Costo = "FECHA_ULTIMO_COSTO";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Ruta_Foto = "RUTA_FOTO";
        public const String Campo_Modelo = "MODELO";
        public const String Campo_Inicial = "INICIAL";

    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Servicios
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_SERVICIOS
    /// PARAMETROS :     
    /// CREO       :           Luz Verónica Gómez García
    /// FECHA_CREO :           27/Septiembre/2010 7:03 PM
    /// MODIFICO          :    Roberto González Oseguera
    /// FECHA_MODIFICO    :    05/Abril/2011
    /// CAUSA_MODIFICACION:    Se agregó la propiedad Campo_Partida_ID
    ///*******************************************************************************/
    public class Cat_Com_Servicios
    {
        public const String Tabla_Cat_Com_Servicios = "CAT_COM_SERVICIOS";
        public const String Campo_Servicio_ID = "SERVICIO_ID";
        public const String Campo_Impuesto_ID = "IMPUESTO_ID";
        public const String Campo_Giro_ID = "GIRO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Costo = "COSTO";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Tipos_Entradas_Salidas
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_TIPOS_ENT_SAL
    /// PARAMETROS :     
    /// CREO       :           Luz Verónica Gómez García
    /// FECHA_CREO :           27/Septiembre/2010 7:19 PM 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Com_Tipos_Ent_Sal
    {
        public const String Tabla_Cat_Com_Tipos_Ent_Sal = "CAT_COM_TIPOS_ENT_SAL";
        public const String Campo_Tipo_Movimiento_ID = "TIPO_MOVIMIENTO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Abreviatura = "ABREVIATURA";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Unidades
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_UNIDADES_PRODUCTOS
    /// PARAMETROS :     
    /// CREO       :           Luz Verónica Gómez García
    /// FECHA_CREO :           27/Septiembre/2010 7:30 PM 
    /// MODIFICO          :    Noe Mosqueda Valadez
    /// FECHA_MODIFICO    :    06/Noviembre/2010 13:00
    /// CAUSA_MODIFICACION:    Se cambio el nombre de la tabla de Cat_Com_Unidades_Productos 
    ///                        a Cat_Com_Unidades
    ///*******************************************************************************/
    public class Cat_Com_Unidades
    {
        public const String Tabla_Cat_Com_Unidades = "CAT_COM_UNIDADES";
        public const String Campo_Unidad_ID = "UNIDAD_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Abreviatura = "ABREVIATURA";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Giro_Proveedor
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_GIRO_PROVEEDOR
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta 
    /// FECHA_CREO :           29/Octubre/2010 05:00 pm  
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/

    public class Cat_Com_Giro_Proveedor
    {
        public const String Tabla_Cat_Com_Giro_Proveedor = "CAT_COM_GIRO_PROVEEDOR";
        public const String Campo_Giro_ID = "CONCEPTO_ID";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Proyectos_Programas
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_PROYECTOS_PROGRAMAS
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta 
    /// FECHA_CREO :           29/Octubre/2010 05:00 pm  
    /// MODIFICO          :Gustavo AC
    /// FECHA_MODIFICO    :28 Feb 2011
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Com_Proyectos_Programas
    {
        public const String Tabla_Cat_Com_Proyectos_Programas = "CAT_SAP_PROYECTOS_PROGRAMAS";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Nombre = "NOMBRE";//no debe ir
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Elemento_PEP = "ELEMENTO_PEP";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Sap_Det_Prog_Partidas
    /// DESCRIPCION:           
    /// PARAMETROS :     
    /// CREO       :           Gustavo AC
    /// FECHA_CREO :       28 Feb 2011    
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Sap_Det_Prog_Partidas
    {
        public const String Tabla_Cat_Sap_Det_Prog_Partidas = "CAT_SAP_DET_PROG_PARTIDAS";
        public const String Campo_Det_Prog_Partidas_ID = "DET_PROG_PARTIDAS_ID";
        public const String Campo_Det_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Det_Partida_ID = "PARTIDA_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:   Cat_Com_Dep_Presupuesto
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_PROYECTOS_PROGRAMAS
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta 
    /// FECHA_CREO :           29/Octubre/2010 05:00 pm  
    /// MODIFICO          :Gustavo AC
    /// FECHA_MODIFICO    :28 Feb 2011
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Com_Dep_Presupuesto
    {
        public const String Tabla_Cat_Com_Dep_Presupuesto = "OPE_SAP_DEP_PRESUPUESTO";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Anio_Presupuesto = "ANIO_PRESUPUESTO";
        public const String Campo_Monto_Presupuestal = "MONTO_PRESUPUESTAL";
        public const String Campo_Monto_Comprometido = "MONTO_COMPROMETIDO";
        public const String Campo_Monto_Disponible = "MONTO_DISPONIBLE";
        public const String Campo_Monto_Ejercido = "MONTO_EJERCIDO";
        public const String Campo_No_Asignacion_Anio = "NO_ASIGNACION_ANIO";
        public const String Campo_Fecha_Asignacion = "FECHA_ASIGNACION";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Presupuesto_ID = "PRESUPUESTO_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        //nuevos Campos
        public const String Campo_Monto_Ampliacion = "MONTO_AMPLIACION";
        public const String Campo_Monto_Reduccion = "MONTO_REDUCCION";
        public const String Campo_Monto_Modificado = "MONTO_MODIFICADO";
        public const String Campo_Monto_Devengado = "MONTO_DEVENGADO";
        public const String Campo_Monto_Pagado = "MONTO_PAGADO";
        public const String Campo_Monto_Devengado_Pagado = "MONTO_DEVEGANDO_PAGADO";
        public const String Campo_Monto_Comprometido_Real = "MONTO_COMPROMETIDO_REAL";
        public const String Campo_No_Presupuesto = "NO_PRESUPUESTO";
        public const String Campo_Capitulo_ID = "CAPITULO_ID";
        public const String Campo_Area_Funcional_ID = "AREA_FUNCIONAL_ID";
    }


    public class Ope_Sap_Dep_Presupuesto
    {
        public const String Tabla_Cat_Com_Dep_Presupuesto = "OPE_SAP_DEP_PRESUPUESTO";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Anio_Presupuesto = "ANIO_PRESUPUESTO";
        public const String Campo_Monto_Presupuestal = "MONTO_PRESUPUESTAL";
        public const String Campo_Monto_Comprometido = "MONTO_COMPROMETIDO";
        public const String Campo_Monto_Disponible = "MONTO_DISPONIBLE";
        public const String Campo_Monto_Ejercido = "MONTO_EJERCIDO";
        public const String Campo_No_Asignacion_Anio = "NO_ASIGNACION_ANIO";
        public const String Campo_Fecha_Asignacion = "FECHA_ASIGNACION";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Presupuesto_ID = "PRESUPUESTO_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        //nuevos Campos
        public const String Campo_Monto_Ampliacion = "MONTO_AMPLIACION";
        public const String Campo_Monto_Reduccion = "MONTO_REDUCCION";
        public const String Campo_Monto_Modificado = "MONTO_MODIFICADO";
        public const String Campo_Monto_Devengado = "MONTO_DEVENGADO";
        public const String Campo_Monto_Pagado = "MONTO_PAGADO";
        public const String Campo_Monto_Devengado_Pagado = "MONTO_DEVEGANDO_PAGADO";
        public const String Campo_Monto_Comprometido_Real = "MONTO_COMPROMETIDO_REAL";
        public const String Campo_No_Presupuesto = "NO_PRESUPUESTO";
        public const String Campo_Capitulo_ID = "CAPITULO_ID";
        public const String Campo_Area_Funcional_ID = "AREA_FUNCIONAL_ID";
        public const String Campo_Grupo_Dependencia_ID = "GRUPO_DEPENDENCIA_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:   Ope_Com_Pres_Prog_Proy
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_PROYECTOS_PROGRAMAS
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta 
    /// FECHA_CREO :           29/Octubre/2010 05:00 pm  
    /// MODIFICO          :Gustavo AC
    /// FECHA_MODIFICO    :28 feb 2011
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/

    public class Ope_Com_Pres_Prog_Proy
    {
        public const String Tabla_Ope_Com_Pres_Prog_Proy = "OPE_SAP_PRES_PROG_PROY";
        public const String Campo_Pres_Prog_Proy_ID = "PRES_PROG_PROY_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Tipo_Presupuesto = "TIPO_PRESUPUESTO";
        public const String Campo_Monto_Presupuestal = "MONTO_PRESUPUESTAL";
        public const String Campo_Monto_Comprometido = "MONTO_COMPROMETIDO";
        public const String Campo_Monto_Disponible = "MONTO_DISPONIBLE";
        public const String Campo_Monto_Ejercido = "MONTO_EJERCIDO";
        public const String Campo_Anio_Presupuesto = "ANIO_PRESUPUESTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Com_Partidas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_COM_PARTIDAS
    /// PARÁMETROS :     
    /// CREO       : Silvia Morales Portuhondo
    /// FECHA_CREO : 29/Octubre/2010 
    /// MODIFICO          :Gustavo Angeles
    /// FECHA_MODIFICO    :28 Feb 2011
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Com_Partidas
    {
        public const String Tabla_Cat_Com_Partidas = "CAT_SAP_PARTIDAS_ESPECIFICAS";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_PrOyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";//no debe ir
        public const String Campo_Giro_ID = "GIRO_ID";
        public const String Campo_Presupuesto_Prog_Poy_ID = "PRES_PROG_PROY_ID";//no debe ir
        public const String Campo_Nombre = "NOMBRE";//No debe ir
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Partida_Generica_ID = "PARTIDA_GENERICA_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Operacion = "OPERACION";
        public const String Campo_Cuenta_SAP = "CUENTA";
        public const String Campo_Clave_SAP = "CLAVE_SAP";
        public const String Campo_Centro_Aplicacion = "CENTRO_APLICACION";
        public const String Campo_Afecta_Area_Funcional = "AFECTA_AREA_FUNCIONAL";
        public const String Campo_Afecta_Partida = "AFECTA_PARTIDA";
        public const String Campo_Afecta_Elemento_PEP = "AFECTA_ELEMENTO_PEP";
        public const String Campo_Afecta_Fondo = "AFECTA_FONDO";
        public const String Campo_Descripcion_Especifica = "DESCRIPCION_ESPECIFICA";
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Com_Subfamilias
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_COM_SUBFAMILIAS
    /// PARÁMETROS :     
    /// CREO       : Silvia Morales Portuhondo
    /// FECHA_CREO : 9/nOVIEMBRE/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Com_Subfamilias
    {
        public const String Campo_Subfamilia_ID = "SUBFAMILIA_ID";
        public const String Campo_Familia_ID = "FAMILIA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Abreviatura = "ABREVIATURA";
        public const String Campo_Descripcion = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Tabla_Cat_Com_Subfamilias = "CAT_COM_SUBFAMILIAS";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Com_Pres_Partida
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_COM_PARTIDAS
    /// PARÁMETROS :     
    /// CREO       : Silvia Morales Portuhondo
    /// FECHA_CREO : 29/Octubre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Pres_Partida
    {
        public const String Campo_Presupuesto_Partida_ID = "PRESUPUESTO_PARTIDA_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Monto_Presupuestal = "MONTO_PRESUPUESTAL";
        public const String Campo_Monto_Comprometido = "MONTO_COMPROMETIDO";
        public const String Campo_Monto_Disponible = "MONTO_DISPONIBLE";
        public const String Campo_Monto_Ejercido = "MONTO_EJERCIDO";
        public const String Campo_Anio_Presupuesto = "ANIO_PRESUPUESTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Tabla_Ope_Com_Pres_Partida = "OPE_SAP_PRES_PARTIDAS";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Com_Req_Producto
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_COM_REQ_PRODUCTO
    /// PARÁMETROS :     
    /// CREO       : Silvia Morales Portuhondo
    /// FECHA_CREO : 01/Noviembre/2010 
    /// MODIFICO          :Susana Trigueros Armenta
    /// FECHA_MODIFICO    :24/Enero/2011
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Req_Producto
    {
        public const String Campo_Ope_Com_Req_Producto_ID = "OPE_COM_REQ_PRODUCTO_ID";
        public const String Campo_Requisicion_ID = "NO_REQUISICION";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Servicio_ID = "SERVICIO_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Importe = "MONTO";
        public const String Campo_Monto_IVA = "MONTO_IVA";
        public const String Campo_Monto_IEPS = "MONTO_IEPS";
        public const String Campo_Monto_Total = "MONTO_TOTAL";
        public const String Campo_Porcentaje_IVA = "PORCENTAJE_IVA";
        public const String Campo_Porcentaje_IEPS = "PORCENTAJE_IEPS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Tabla_Ope_Com_Req_Producto = "OPE_COM_REQ_PRODUCTO";
        public const String Campo_Precio_Unitario = "PRECIO_UNITARIO";
        //Campos nuevos 
        public const String Campo_Prod_Serv_ID = "PROD_SERV_ID";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Giro_ID = "CONCEPTO_ID";
        public const String Campo_Nombre_Giro = "NOMBRE_CONCEPTO";
        public const String Campo_Nombre_Producto_Servicio = "NOMBRE_PRODUCTO_SERVICIO";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Nombre_Proveedor = "NOMBRE_PROVEEDOR";
        public const String Campo_Precio_U_Sin_Imp_Cotizado = "PRECIO_U_SIN_IMP_COTIZADO";
        public const String Campo_Subtota_Cotizado = "SUBTOTAL_COTIZADO";
        public const String Campo_IVA_Cotizado = "IVA_COTIZADO";
        public const String Campo_IEPS_Cotizado = "IEPS_COTIZADO";
        public const String Campo_Total_Cotizado = "TOTAL_COTIZADO";
        public const String Campo_Empleado_Cotizador_ID = "EMPLEADO_COTIZADOR_ID";
        public const String Campo_Precio_U_Con_Imp_Cotizado = "PRECIO_U_CON_IMP_COTIZADO";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_No_Orden_Compra = "NO_ORDEN_COMPRA";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        public const String Campo_Resguardado = "RESGUARDADO";
        public const String Campo_Cantidad_Entregada = "CANTIDAD_ENTREGADA";
        public const String Campo_Nombre_Prod_Serv_Orden_Compra = "NOMBRE_PROD_SERV_OC";
        public const String Campo_Marca_OC = "MARCA_OC";
    }
    /////*******************************************************************************
    ///// NOMBRE DE LA CLASE: Ope_Com_Req_Producto
    ///// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_COM_REQ_PRODUCTO
    ///// PARÁMETROS :     
    ///// CREO       : Silvia Morales Portuhondo
    ///// FECHA_CREO : 01/Noviembre/2010 
    ///// MODIFICO          :
    ///// FECHA_MODIFICO    :
    ///// CAUSA_MODIFICACIÓN:
    /////*******************************************************************************
    //public class Ope_Com_Req_Producto
    //{
    //    public const String Campo_Ope_Com_Req_Producto_ID = "OPE_COM_REQ_PRODUCTO_ID";
    //    public const String Campo_Requisicion_ID = "NO_REQUISICION";
    //    public const String Campo_Producto_ID = "PRODUCTO_ID";
    //    public const String Campo_Servicio_ID = "SERVICIO_ID";
    //    public const String Campo_Partida_ID = "PARTIDA_ID";
    //    public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
    //    public const String Campo_Cantidad = "CANTIDAD";
    //    public const String Campo_Importe = "MONTO";
    //    public const String Campo_Monto_IVA = "MONTO_IVA";
    //    public const String Campo_Monto_IEPS = "MONTO_IEPS";
    //    public const String Campo_Monto_Total = "MONTO_TOTAL";
    //    public const String Campo_Porcentaje_IVA = "PORCENTAJE_IVA";
    //    public const String Campo_Porcentaje_IEPS = "PORCENTAJE_IEPS";
    //    public const String Campo_Usuario_Creo = "USUARIO_CREO";
    //    public const String Campo_Fecha_Creo = "FECHA_CREO";
    //    public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
    //    public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    //    public const String Tabla_Ope_Com_Req_Producto = "OPE_COM_REQ_PRODUCTO";
    //}

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Documentos_Requisicion
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_DOCUMENTOS_REQUISICION
    /// PARÁMETROS :     
    /// CREO       : Silvia Morales Portuhondo
    /// FECHA_CREO : 01/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Documentos_Requisicion
    {
        public const String Campo_Documento_ID = "DOCUMENTO_ID";
        public const String Campo_Requisicion_ID = "NO_REQUISICION";
        public const String Campo_Url = "URL";
        public const String Campo_Nombre_Documento = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Tabla_Ope_Documentos_Requisicion = "OPE_DOCUMENTOS_REQUISICION";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Com_Dep_Partidas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla
    /// PARÁMETROS :     
    /// CREO       : Silvia Morales Portuhondo
    /// FECHA_CREO : 01/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Dep_Partidas
    {
        public const String Campo_Dependencia_Partida_ID = "DEPENDENCIA_PARTIDA_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Monto_Presupuestal = "MONTO_PRESUPUESTAL";
        public const String Campo_Monto_Comprometido = "MONTO_COMPROMETIDO";
        public const String Campo_Monto_Disponible = "MONTO_DISPONIBLE";
        public const String Campo_Anio_Presupuesto = "ANIO_PRESUPUESTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Tabla_Ope_Com_Dep_Partidas = "OPE_COM_DEP_PARTIDAS";
    }
    /////*******************************************************************************
    ///// NOMBRE DE LA CLASE: Ope_Com_Propuesta_Cotizacion
    ///// DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Com_Propuesta_Cotizacion
    ///// PARÁMETROS :     
    ///// CREO       : Susana Trigueros Armenta 
    ///// FECHA_CREO : 03/Julio/2010 
    ///// MODIFICO          :
    ///// FECHA_MODIFICO    :
    ///// CAUSA_MODIFICACIÓN:
    /////*******************************************************************************
    public class Ope_Com_Propuesta_Cotizacion
    {
        public const String Tabla_Ope_Com_Propuesta_Cotizacion = "OPE_COM_PROPUESTA_COTIZACION";
        public const String Campo_No_Propuesta_Cotizacion = "NO_PROPUESTA_COTIZACION";
        public const String Campo_Ope_Com_Req_Producto_ID = "OPE_COM_REQ_PRODUCTO_ID";
        public const String Campo_No_Requisicion = "NO_REQUISICION";

        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Prod_Serv_ID = "PROD_SERV_ID";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Nombre_Producto_Servicion = "NOMBRE_PRODUCTO_SERVICIO";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";

        public const String Campo_Precio_U_Sin_Imp_Cotizado = "PRECIO_U_SIN_IMP_COTIZADO";
        public const String Campo_Subtota_Cotizado = "SUBTOTAL_COTIZADO";
        public const String Campo_IVA_Cotizado = "IVA_COTIZADO";
        public const String Campo_IEPS_Cotizado = "IEPS_COTIZADO";
        public const String Campo_Total_Cotizado = "TOTAL_COTIZADO";
        public const String Campo_Precio_U_Con_Imp_Cotizado = "PRECIO_U_CON_IMP_COTIZADO";
        public const String Campo_Nombre_Cotizador = "NOMBRE_COTIZADOR";

        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

        public const String Campo_IEPS_Cotizado_Req = "IEPS_COTIZADO_REQ";
        public const String Campo_IVA_Cotizado_Req = "IVA_COTIZADO_REQ";
        public const String Campo_Total_Cotizado_Requisicion = "TOTAL_COTIZADO_REQUISICION";
        public const String Campo_Subtotal_Cotizado_Requisicion = "SUBTOTAL_COTIZADO_REQUISICION";

        public const String Campo_Fecha_Elaboracion = "FECHA_ELABORACION";
        public const String Campo_Tiempo_Entrega = "TIEMPO_ENTREGA";
        public const String Campo_Garantia = "GARANTIA";
        public const String Campo_Vigencia_Propuesta = "VIGENCIA_PROPUESTA";
        public const String Campo_Registro_Padron_Prov = "REGISTRO_PADRON_PROV";
        public const String Campo_Descripcion_Producto = "DESCRIPCION_PRODUCTO";
        public const String Campo_Marca = "MARCA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Resultado = "RESULTADO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Com_Requisiciones
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Com_Requisiciones
    /// PARÁMETROS :     
    /// CREO       : Susana Trigueros Armenta 
    /// FECHA_CREO : 01/Noviembre/2010 
    /// MODIFICO          :Gustavo Angeles Cruz
    /// FECHA_MODIFICO    :21 ENE 2011
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public class Ope_Com_Requisiciones
    {
        public const String Tabla_Ope_Com_Requisiciones = "OPE_COM_REQUISICIONES";
        public const String Campo_Requisicion_ID = "NO_REQUISICION";
        public const String Campo_Area_ID = "AREA_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Codigo_Programatico = "CODIGO_PROGRAMATICO";
        public const String Campo_Subtotal = "SUBTOTAL";
        public const String Campo_IVA = "IVA";
        public const String Campo_IEPS = "IEPS";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Total = "TOTAL";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Fecha_Construccion = "FECHA_CONSTRUCCION";
        public const String Campo_Empleado_Construccion_ID = "EMPLEADO_CONSTRUCCION_ID";
        public const String Campo_Fecha_Generacion = "FECHA_GENERACION";
        public const String Campo_Empleado_Generacion_ID = "EMPLEADO_GENERACION_ID";
        public const String Campo_Fecha_Autorizacion = "FECHA_AUTORIZACION";
        public const String Campo_Empleado_Autorizacion_ID = "EMPLEADO_AUTORIZACION_ID";
        public const String Campo_Fecha_Surtido = "FECHA_SURTIDO";
        public const String Campo_Empleado_Surtido_ID = "EMPLEADO_SURTIDO_ID";
        public const String Campo_Fecha_Filtrado = "FECHA_FILTRADO";
        public const String Campo_Empleado_Filtrado_ID = "EMPLEADO_FILTRADO_ID";
        public const String Campo_Fecha_Distribucion = "FECHA_DISTRIBUCION";
        public const String Campo_Empleado_Distribucion_ID = "EMPLEADO_DISTRIBUCION_ID";
        public const String Campo_Fecha_Cotizacion = "FECHA_COTIZACION";
        public const String Campo_Empleado_Cotizacion_ID = "EMPLEADO_COTIZACION_ID";
        public const String Campo_Fecha_Confirmacion = "FECHA_CONFIRMACION";
        public const String Campo_Empleado_Confirmacion_ID = "EMPLEADO_CONFIRMACION_ID";
        public const String Campo_Fase = "FASE";
        public const String Campo_Justificacion_Compra = "JUSTIFICACION_COMPRA";
        public const String Campo_Especificacion_Prod_Serv = "ESPECIFICACION_PROD_SERV";
        public const String Campo_Verificaion_Entrega = "VERIFICACION_ENTREGA";
        public const String Campo_No_Consolidacion = "NO_CONSOLIDACION";
        public const String Campo_Consolidada = "CONSOLIDADA";
        public const String Campo_Tipo_Compra = "TIPO_COMPRA";
        public const String Campo_Fecha_Cancelada = "FECHA_CANCELADA";
        public const String Campo_Empleado_Cancelada_ID = "EMPLEADO_CANCELADA_ID";
        public const String Campo_Fecha_Rechazo = "FECHA_RECHAZADA";
        public const String Campo_Empleado_Rechazo_ID = "EMPLEADO_RECHAZO_ID";
        public const String Campo_Fecha_Cotizada_Rechazada = "FECHA_COTIZADA_RECHAZADA";
        public const String Campo_Empleado_Cotizada_Rechazada_ID = "EMPLEADO_COTIZA_RECHAZA_ID";
        public const String Campo_No_Cotizacion = "NO_COTIZACION";
        public const String Campo_No_Comite_Compras = "NO_COMITE_COMPRAS";
        public const String Campo_No_Licitacion = "NO_LICITACION";
        public const String Campo_No_Orden_Compra = "NO_ORDEN_COMPRA";
        public const String Campo_Subtotal_Cotizado = "SUBTOTAL_COTIZADO";
        public const String Campo_IEPS_Cotizado = "IEPS_COTIZADO";
        public const String Campo_IVA_Cotizado = "IVA_COTIZADO";
        public const String Campo_Total_Cotizado = "TOTAL_COTIZADO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Tipo_Articulo = "TIPO_ARTICULO";
        public const String Campo_Precio_Unitario = "PRECIO_UNITARIO";
        public const String Campo_Listado_Almacen = "LISTADO_ALMACEN";
        public const String Campo_Cotizador_ID = "COTIZADOR_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Elemento_PEP = "ELEMENTO_PEP";
        public const String Campo_Alerta = "ALERTA";
        public const String Campo_Especial_Ramo_33 = "ESPECIAL_RAMO_33";
        public const String Campo_Leido_Por_Cotizador = "LEIDO_POR_COTIZADOR";
    }


    //}
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Cat_Com_Giros
    /// DESCRIPCION:            Clase que contiene los campos de la tabla CAT_COM_GIROS
    /// PARAMETROS :     
    /// CREO       :            Noe Mosqueda Valadez
    /// FECHA_CREO :            03/Noviembre/2010 18:48 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Com_Giros
    {
        public const String Tabla_Cat_Com_Giros = "CAT_COM_GIROS";
        public const String Campo_Giro_ID = "GIRO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Req_Observaciones
    /// DESCRIPCION:            Clase que contiene los campos de la tabla OPE_COM_REQ_OBSERVACIONES
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :            08/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************

    public class Ope_Com_Req_Observaciones
    {
        public const String Tabla_Ope_Com_Req_Observaciones = "OPE_COM_REQ_OBSERVACIONES";
        public const String Campo_Observacion_ID = "OBSERVACION_ID";
        public const String Campo_Requisicion_ID = "NO_REQUISICION";
        public const String Campo_Comentario = "COMENTARIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:  Cat_Com_Monto_Proceso_Compra
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla  Cat_Com_Monto_Proceso_Compra
    /// PARÁMETROS :     
    /// CREO       : Susana Trigueros Armenta 
    /// FECHA_CREO : 01/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Com_Monto_Proceso_Compra
    {
        public const String Tabla_Cat_Com_Monto_Proceso_Compra = "CAT_COM_MONTO_PROCESO_COMPRA";
        public const String Campo_Parametro_ID = "PARAMETRO_ID";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Monto_Compra_Directa_Ini = "MONTO_COMPRA_DIRECTA_INI";
        public const String Campo_Monto_Compra_Directa_Fin = "MONTO_COMPRA_DIRECTA_FIN";
        public const String Campo_Monto_Cotizacion_Ini = "MONTO_COTIZACION_INI";
        public const String Campo_Monto_Cotizacion_Fin = "MONTO_COTIZACION_FIN";
        public const String Campo_Monto_Comite_Ini = "MONTO_COMITE_INI";
        public const String Campo_Monto_Comite_Fin = "MONTO_COMITE_FIN";
        public const String Campo_Monto_Licitacion_R_Ini = "MONTO_LICITACION_R_INI";
        public const String Campo_Monto_Licitacion_R_Fin = "MONTO_LICITACION_R_FIN";
        public const String Campo_Monto_Licitacion_P_Ini = "MONTO_LICITACION_P_INI";
        public const String Campo_Monto_Licitacion_P_Fin = "MONTO_LICITACION_P_FIN";
        public const String Campo_Fondo_Fijo_Ini = "FONDO_FIJO_INI";
        public const String Campo_Fondo_Fijo_Fin = "FONDO_FIJO__FIN";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Ordenes_Compra
    /// DESCRIPCIÓN:            Clase que contiene los campos de la tabla Ope_Com_Ordenes_Compra
    /// PARÁMETROS :     
    /// CREO       :            Noe Mosqueda Valadez 
    /// FECHA_CREO :            26/Noviembre/2010 12:09 
    /// MODIFICO           :Gustavo Angeles Cruz
    /// FECHA_MODIFICO     :18 Ene 2011
    /// CAUSA_MODIFICACIÓN :Se agregaron campos
    /// MODIFICO           :Salvador Hernández Ramírez
    /// FECHA_MODIFICO     :10/Febrero/2011
    /// CAUSA_MODIFICACIÓN :Se agregaron las constantes "Campo_Resguardada", "Campo_Usuario_Id_Resguardo" y "Campo_Recibo_Transitorio"
    ///*******************************************************************************
    public class Ope_Com_Ordenes_Compra
    {
        public const String Tabla_Ope_Com_Ordenes_Compra = "OPE_COM_ORDENES_COMPRA";
        public const String Campo_No_Orden_Compra = "NO_ORDEN_COMPRA";
        public const String Campo_No_Requisicion = "NO_REQUISICION";
        public const String Campo_No_Licitacion = "NO_LICITACION";
        public const String Campo_Tipo_Proceso = "TIPO_PROCESO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Nombre_Proveedor = "NOMBRE_PROVEEDOR";
        public const String Campo_Subtotal = "SUBTOTAL";
        public const String Campo_Total_IEPS = "TOTAL_IEPS";
        public const String Campo_Total_IVA = "TOTAL_IVA";
        public const String Campo_Total = "TOTAL";
        public const String Campo_Fecha_Ejercio = "FECHA_EJERCIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_No_Factura_Interno = "NO_CONTRA_RECIBO";
        public const String Campo_No_Cotizacion = "NO_COTIZACION";
        public const String Campo_Resguardada = "RESGUARDADA";
        public const String Campo_Usuario_Id_Resguardo = "USUARIO_ID_RESGUARDO";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Lista_Requisiciones = "LISTA_REQUISICIONES";
        public const String Campo_Tipo_Articulo = "TIPO_ARTICULO";
        public const String Campo_No_Comite_Compras = "NO_COMITE_COMPRAS";
        public const String Campo_Fecha_Entrega = "FECHA_ENTREGA";
        public const String Campo_Recibo_Transitorio = "RECIBO_TRANSITORIO";
        public const String Campo_No_Reserva = "NO_RESERVA";
        public const String Campo_Registrada = "REGISTRADA";
        public const String Campo_Fecha_Cancelacion = "FECHA_CANCELACION";
        public const String Campo_Empleado_ID_Cancelacion = "EMPLEADO_ID_CANCELACION";
        public const String Campo_Motivo_Cancelacion = "MOTIVO_CANCELACION";
        public const String Campo_Impresa = "IMPRESA";
        public const String Campo_Especiales_Ramo33 = "ESPECIALES_RAMO_33";
        public const String Campo_Condicion1 = "CONDICION1";
        public const String Campo_Condicion2 = "CONDICION2";
        public const String Campo_Condicion3 = "CONDICION3";
        public const String Campo_Condicion4 = "CONDICION4";
        public const String Campo_Condicion5 = "CONDICION5";
        public const String Campo_Condicion6 = "CONDICION6";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Detalle_Orden_Compra
    /// DESCRIPCIÓN:            Clase que contiene los campos de la tabla 
    /// Ope_Com_Detalle_Orden_Compra
    /// PARÁMETROS :     
    /// CREO       :            Gustavo Angeles Cruz
    /// FECHA_CREO :            18/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Detalle_Orden_Compra
    {
        public const String Tabla_Ope_Com_Ordenes_Compra = "OPE_COM_DETALLE_ORDEN_COMPRA";
        public const String Campo_No_Detalle = "NO_DETALLE";
        public const String Campo_No_Orden_Compra = "NO_ORDEN_COMPRA";
        public const String Campo_Prod_Serv_ID = "PROD_SERV_ID";
        public const String Campo_Nombre_Prod_Serv = "NOMBRE_PROD_SERV";
        public const String Campo_Precio_Unitario = "PRECIO_UNITARIO";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Subtotal = "SUBTOTAL";
        public const String Campo_Total_IEPS = "TOTAL_IEPS";
        public const String Campo_Total_IVA = "TOTAL_IVA";
        public const String Campo_Porcentaje_IEPS = "PORCENTAJE_IEPS";
        public const String Campo_Porcentaje_IVA = "PORCENTAJE_IVA";
        public const String Campo_Total = "TOTAL";
        public const String Campo_Giro_ID = "GIRO_ID";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Coment_orden_Comp
    /// DESCRIPCIÓN:            Clase que contiene los campos de la tabla 
    /// Ope_Com_Coment_orden_Comp
    /// PARÁMETROS :     
    /// CREO       :            Gustavo Angeles Cruz
    /// FECHA_CREO :            18/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Coment_orden_Comp
    {
        public const String Tabla_Ope_Com_Ordenes_Compra = "OPE_COM_COMENT_ORDEN_COMP";
        public const String Campo_No_Comentario = "NO_COMENTARIO";
        public const String Campo_No_Orden_Compra = "NO_ORDEN_COMPRA";
        public const String Campo_Comentario = "COMENTARIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Cotizaciones
    /// DESCRIPCIÓN:            Clase que contiene los campos de la tabla Ope_Com_Cotizaciones
    /// PARÁMETROS :     
    /// CREO       :            Noe Mosqueda Valadez 
    /// FECHA_CREO :            26/Noviembre/2010 12:22 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Cotizaciones
    {
        public const String Tabla_Ope_Com_Cotizaciones = "OPE_COM_COTIZACIONES";
        public const String Campo_No_Cotizacion = "NO_COTIZACION";
        public const String Campo_Empleado_Cotizador_ID = "EMPLEADO_COTIZADOR_ID";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Fecha_Precotizacion = "FECHA_PRECOTIZACION";
        public const String Campo_Fecha_Cotizacion = "FECHA_COTIZACION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Condiciones = "CONDICIONES";
        public const String Campo_Tiempo_Entrega = "TIEMPO_ENTREGA";
        public const String Campo_SubTotal_Con_Impuesto = "SUBTOTAL_CON_IMPUESTO";
        public const String Campo_SubTotal_Sin_Impuesto = "SUBTOTAL_SIN_IMPUESTO";
        public const String Campo_IVA = "IVA";
        public const String Campo_IEPS = "IEPS";
        public const String Campo_Total = "TOTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Lista_Requisiciones = "LISTA_REQUISICIONES";
        public const String Campo_Num_Proveedores = "NUM_PROVEEDORES";
        public const String Campo_Total_Cotizado = "TOTAL_COTIZADO";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Listado_Almacen = "LISTADO_ALMACEN";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Det_Cotizaciones
    /// DESCRIPCIÓN:            Clase que contiene los campos de la tabla OPE_COM_DET_COTIZACIONES
    /// PARÁMETROS :     
    /// CREO       :            Noe Mosqueda Valadez 
    /// FECHA_CREO :            26/Noviembre/2010 12:22 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Det_Cotizaciones
    {
        public const String Tabla_Ope_Com_Det_Cotizaciones = "OPE_COM_DET_COTIZACIONES";
        public const String Campo_No_Cotizacion = "NO_COTIZACION";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Giro_ID = "CONCEPTO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Cot_Detalles
    /// DESCRIPCIÓN:            Clase que contiene los campos de la tabla Ope_Com_Cot_Detalles
    /// PARÁMETROS :     
    /// CREO       :            Noe Mosqueda Valadez 
    /// FECHA_CREO :            26/Noviembre/2010 12:27 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Cot_Detalles
    {
        public const String Tabla_Ope_Com_Cot_Detalles = "OPE_COM_COT_DETALLES";
        public const String Campo_No_Cotizacion = "NO_COTIZACION";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Servicio_ID = "SERVICIO_ID";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Costo_Real = "COSTO_REAL";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Ultimo_Costo = "ULTIMO_COSTO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Consolidacion_Req
    /// DESCRIPCIÓN:            Clase que contiene los campos de la tabla Ope_Com_Consolidacion_Req
    /// PARÁMETROS :     
    /// CREO       :            Noe Mosqueda Valadez 
    /// FECHA_CREO :            26/Noviembre/2010 12:32 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Consolidacion_Req
    {
        public const String Tabla_Ope_Com_Consolidacion_Req = "OPE_COM_CONSOLIDACION_REQ";
        public const String Campo_No_Cotizacion = "NO_COTIZACION";
        public const String Campo_No_Requisicion = "NO_REQUISICION";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Servicio_ID = "SERVICIO_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Familias_Productos
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_FAMILIAS_PRODUCTOS
    /// PARAMETROS :     
    /// CREO       :           Luz Verónica Gómez García
    /// FECHA_CREO :           27/Septiembre/2010 4:47 PM 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Com_Familias_Productos
    {
        public const String Tabla_Cat_Com_Familias_Productos = "CAT_COM_FAMILIAS_PRODUCTOS";
        public const String Campo_Familia_ID = "FAMILIA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Parametros
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_PARAMETROS
    /// PARAMETROS :     
    /// CREO       :           Noe Mosqueda Valadez
    /// FECHA_CREO :           24/Diciembre/2010 14:54 
    /// MODIFICO          :
    /// FECHA_MODIFICO    : Susana Trigueros Armenta
    /// CAUSA_MODIFICACION: 2/Noviembre/2012
    ///*******************************************************************************/
    public class Cat_Com_Parametros
    {
        public const String Tabla_Cat_Com_Parametros = "CAT_COM_PARAMETROS";
        public const String Campo_Parametro_ID = "PARAMETRO_ID";
        public const String Campo_Cantidad_Sal_Min_Resguardo = "CANTIDAD_SAL_MIN_RESGUARDO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Plazo_Surtir_Orden_Compra = "PLAZO_SURTIR_ORDEN_COMPRA";
        public const String Campo_Partida_Gen_Almacen_Global = "PARTIDA_GEN_ALMACEN_GLOBAL";
        public const String Campo_Partida_Esp_Almacen_Global = "PARTIDA_ESP_ALMACEN_GLOBAL";
        public const String Campo_Dependencia_ID_Almacen = "DEPENDENCIA_ID_ALMACEN";
        public const String Campo_Programa_Almacen = "PROGRAMA_ALMACEN";
        public const String Campo_Rol_Proveedor_ID = "ROL_PROVEEDOR_ID";
        public const String Campo_Invitacion_Proveedores = "INVITACION_PROVEEDORES";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Licitaciones
    /// DESCRIPCION:            Clase que contiene los campos de la tabla OPE_COM_LICITACIONES
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :            27/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Com_Licitaciones
    {
        public const String Tabla_Ope_Com_Licitaciones = "OPE_COM_LICITACIONES";
        public const String Campo_No_Licitacion = "NO_LICITACION";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Inicio = "FECHA_INICIO";
        public const String Campo_Fecha_Fin = "FECHA_FIN";
        public const String Campo_Justificacion = "JUSTIFICACION";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Monto_Total = "MONTO_TOTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Lista_Requisiciones = "LISTA_REQUISICIONES";
        public const String Campo_Clasificacion = "CLASIFICACION";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Total_Cotizado = "TOTAL_COTIZADO";

    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Licitacion_Detalles
    /// DESCRIPCION:            Clase que contiene los campos de la tabla OPE_COM_LICITACION_DETALLES
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :            27/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Com_Licitacion_Detalle
    {
        public const String Tabla_Ope_Com_Licitacion_Detalle = "OPE_COM_LICITACION_DETALLE";
        public const String Campo_No_Licitacion = "NO_LICITACION";
        public const String Campo_No_Requisicion = "NO_REQUISICION";
        public const String Campo_Ope_Com_Req_Producto_ID = "OPE_COM_REQ_PRODUCTO_ID";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Monto_Detalle = "MONTO_DETALLE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Licitacion_Proveedor
    /// DESCRIPCION:            Clase que contiene los campos de la tabla OPE_COM_LICITACION_PROVEEDOR
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :            3/Ene/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Com_Licitacion_Proveedor
    {
        public const String Tabla_Ope_Com_Licitacion_Proveedor = "OPE_COM_LICITACION_PROVEEDOR";
        public const String Campo_No_Licitacion_Proveedor = "NO_LICITACION_PROVEEDOR";
        public const String Campo_No_Licitacion = "NO_LICITACION";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Licitacion_Proveedor
    /// DESCRIPCION:            Clase que contiene los campos de la tabla OPE_COM_LICITACION_PROVEEDOR
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :             3/Ene/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Com_Licitacion_Partidas
    {
        public const String Tabla_Ope_Com_Licitacion_Partidas = "OPE_COM_LICITACION_PARTIDAS";
        public const String No_Licitacion_Proveedor = "NO_LICITACION_PROVEEDOR";
        public const String Partida_ID = "PARTIDA_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Consolidaciones
    /// DESCRIPCION:            Clase que contiene los campos de la tabla OPE_COM_CONSOLIDACIONES
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :            28/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Com_Consolidaciones
    {
        public const String Tabla_Ope_Com_Consolidaciones = "OPE_COM_CONSOLIDACIONES";
        public const String Campo_No_Consolidacion = "NO_CONSOLIDACION";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Monto = "MONTO";
        public const String Campo_Lista_Requisiciones = "LISTA_REQUISICIONES";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Tipo = "TIPO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Det_Consolidacion
    /// DESCRIPCION:            Clase que contiene los campos de la tabla OPE_COM_DET_CONSOLIDACION
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :            28/Diciembre/2010 
    /// MODIFICO          :    Gustavo Angeles Cruz
    /// FECHA_MODIFICO    :    9 ene 2011
    /// CAUSA_MODIFICACION:    Cambiaron campos de la Base de Datos
    ///*******************************************************************************
    public class Ope_Com_Det_Consolidacion
    {
        public const String Tabla_Ope_Com_Det_Consolidacion = "OPE_COM_DET_CONSOLIDACION";
        public const String Campo_No_Detalle_Consolidacion = "NO_DETALLE_CONSOLIDACION";
        public const String Campo_No_Consolidacion = "NO_CONSOLIDACION";
        public const String Campo_Producto_Servicio_ID = "PRODUCTO_SERVICIO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Giro_ID = "GIRO_ID";
        public const String Campo_Nombre_Giro = "NOMBRE_GIRO";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Monto_Con_Impuesto = "MONTO_CON_IMPUESTO";
        public const String Campo_Clave_Prod_Serv = "CLAVE_PROD_SERV";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Ope_Com_Comite_Compras
    /// DESCRIPCION:           Clase que contiene los campos de la tabla OPE_COM_COMITE_COMPRAS 
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :           19/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Ope_Com_Comite_Compras
    {
        public const String Tabla_Ope_Com_Comite_Compras = "OPE_COM_COMITE_COMPRAS";
        public const String Campo_No_Comite_Compras = "NO_COMITE_COMPRAS";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Justificacion = "JUSTIFICACION";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Monto_Total = "MONTO_TOTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Lista_Requisiciones = "LISTA_REQUISICIONES";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Total_Cotizado = "TOTAL_COTIZADO";


    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Cotizadores
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_COTIZADORES 
    /// PARAMETROS :     
    /// CREO       :           Jacqueline Ramirez Sierra
    /// FECHA_CREO :           05/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Com_Cotizadores
    {
        public const String Tabla_Cat_Com_Cotizadores = "CAT_COM_COTIZADORES";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Nombre_Completo = "NOMBRE_COMPLETO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Correo = "CORREO";
        public const String Campo_Password_Correo = "PASSWORD_CORREO";
        public const String Campo_IP_Correo_Saliente = "IP_CORREO_SALIENTE";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Com_Det_Cotizadores
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_COM_DET_COTIZADORES 
    /// PARAMETROS :     
    /// CREO       :           Jacqueline Ramirez Sierra
    /// FECHA_CREO :           11/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Com_Det_Cotizadores
    {
        public const String Tabla_Cat_Com_Det_Cotizadores = "CAT_COM_DET_COTIZADORES";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Giro_ID = "CONCEPTO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    #endregion Fin_SubRegion_Compras

    ///*************************************************************************************************************************
    ///                                                                SubRegion Almacen
    ///*************************************************************************************************************************
    #region SubRegion_Almacen
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:   Ope_Alm_Ajustes_Inv_Stock
    /// DESCRIPCION:          Clase de constantes de la tabla Ope_Alm_Ajustes_Inv_Stock
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :           30/Septiembre/2011 6:48 PM
    /// MODIFICO          :    
    /// FECHA_MODIFICO    :    
    ///*******************************************************************************/
    public class Ope_Alm_Ajustes_Inv_Stock
    {
        public const String Campo_No_Ajuste = "NO_AJUSTE";
        public const String Campo_Fecha_Hora = "FECHA_HORA";
        public const String Campo_Motivo_Ajuste_Coor = "MOTIVO_AJUSTE_COOR";
        public const String Campo_Motivo_Ajuste_Dir = "MOTIVO_AJUSTE_DIR";
        public const String Campo_Empleado_Elaboro_ID = "EMPLEADO_ELABORO_ID";
        public const String Campo_Empleado_Autorizo_ID = "EMPLEADO_AUTORIZO_ID";
        public const String Campo_Empleado_Rechazo_ID = "EMPLEADO_RECHAZO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Elaboro = "FECHA_ELABORO";
        public const String Campo_Fecha_Autorizo = "FECHA_AUTORIZO";
        public const String Campo_Fecha_Rechazo = "FECHA_RECHAZO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Tabla_Ope_Alm_Ajustes_Inv_Stock = "OPE_ALM_AJUSTES_INV_STOCK";
    }


    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Ope_Alm_Ajustes_Detalles
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_ALM_AJUSTES_DETALLES
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda
    ///FECHA_CREO: 30/Septiembre/2011 06:47:25 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:  
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Alm_Ajustes_Detalles
    {
        public const String Tabla_Ope_Alm_Ajustes_Almacen = "OPE_ALM_AJUSTES_DETALLES";
        public const String Campo_No_Ajuste = "NO_AJUSTE";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Existencia_Sistema = "EXISTENCIA_SISTEMA";
        public const String Campo_Conteo_Fisico = "CONTEO_FISICO";
        public const String Campo_Diferencia = "DIFERENCIA";
        public const String Campo_Tipo_Movimiento = "TIPO_MOVIMIENTO";
        public const String Campo_Importe_Diferencia = "IMPORTE_DIFERENCIA";
        public const String Campo_Precio_Promedio = "PRECIO_PROMEDIO";
        public const String Campo_Nombre_Descipcion = "NOMBRE_DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Alm_Com_Salidas
    /// DESCRIPCIÓN:        Clase que contiene los campos de la tabla ALM_COM_SALIDAS
    /// PARÁMETROS :     
    /// CREO       :        Noe Mosqueda Valadez
    /// FECHA_CREO :        11/Noviembre/2010 16:44 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Alm_Com_Salidas
    {
        public const String Tabla_Alm_Com_Salidas = "ALM_COM_SALIDAS";
        public const String Campo_No_Salida = "NO_SALIDA";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Area_ID = "AREA_ID";
        public const String Campo_Empleado_Solicito_ID = "EMPLEADO_SOLICITO_ID";
        public const String Campo_Requisicion_ID = "NO_REQUISICION";
        public const String Campo_Tipo_Salida_ID = "TIPO_SALIDA_ID";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Empleado_Almacen_ID = "EMPLEADO_ALMACEN_ID";
        public const String Campo_Subtotal = "SUBTOTAL";
        public const String Campo_IVA = "IVA";
        public const String Campo_Total = "TOTAL";

        public const String Campo_Poliza_Stock_SAP = "POLIZA_SAP";
        public const String Campo_Contabilizado = "CONTABILIZADO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Alm_Com_Salidas_Detalles
    /// DESCRIPCIÓN:        Clase que contiene los campos de la tabla ALM_COM_SALIDAS_DETALLES
    /// PARÁMETROS :     
    /// CREO       :        Noe Mosqueda Valadez
    /// FECHA_CREO :        11/Noviembre/2010 16:50 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Alm_Com_Salidas_Detalles
    {
        public const String Tabla_Alm_Com_Salidas_Detalles = "ALM_COM_SALIDAS_DETALLES";
        public const String Campo_No_Salida = "NO_SALIDA";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Costo = "COSTO";
        public const String Campo_Costo_Promedio = "COSTO_PROMEDIO";
        public const String Campo_Subtotal = "SUBTOTAL";
        public const String Campo_IVA = "IVA";
        public const String Campo_Importe = "IMPORTE";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Ope_Com_Recibos
    /// DESCRIPCION:           Clase que contiene los campos de la tabla OPE_COM_RECIBOS
    /// PARAMETROS :     
    /// CREO       :           Noe Mosqueda Valadez
    /// FECHA_CREO :           16/Noviembre/2010 10:35
    /// MODIFICO          :    
    /// FECHA_MODIFICO    :    
    /// CAUSA_MODIFICACION:    
    ///*******************************************************************************/
    public class Ope_Com_Recibos
    {
        public const String Tabla_Ope_Com_Recibos = "OPE_COM_RECIBOS";
        public const String Campo_No_Recibo = "NO_RECIBO";
        public const String Campo_No_Salida = "NO_SALIDA";
        public const String Campo_Empleado_Recibo_ID = "EMPLEADO_RECIBO_ID";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Empleado_Almacen_ID = "EMPLEADO_ALMACEN_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Ope_Com_Resguardos
    /// DESCRIPCION:           Clase que contiene los campos de la tabla OPE_COM_RESGUARDOS
    /// PARAMETROS :     
    /// CREO       :           Noe Mosqueda Valadez
    /// FECHA_CREO :           16/Noviembre/2010 10:41
    /// MODIFICO          :    
    /// FECHA_MODIFICO    :    
    /// CAUSA_MODIFICACION:    
    ///*******************************************************************************/
    public class Ope_Com_Resguardos
    {
        public const String Tabla_Ope_Com_Resguardos = "OPE_COM_RESGUARDOS";
        public const String Campo_No_Resguardo = "NO_RESGUARDO";
        public const String Campo_No_Salida = "NO_SALIDA";
        public const String Campo_Empleado_Resguardo_ID = "EMPLEADO_RESGUARDO_ID";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Empleado_Almacen_ID = "EMPLEADO_ALMACEN_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Com_Listado
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Com_Listado
    /// PARÁMETROS :     
    /// CREO       : Susana Trigueros Armenta 
    /// FECHA_CREO : 20/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Listado
    {
        public const String Tabla_Ope_Com_Listado = "OPE_COM_LISTADO";
        public const String Campo_Listado_ID = "LISTADO_ID";
        public const String Campo_No_Requisicion_ID = "NO_REQUISICION";
        public const String Campo_No_Partida_ID = "NO_PARTIDA_ID";
        public const String Campo_No_Proyecto_ID = "NO_PROYECTO_ID";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Total = "TOTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Fecha_Construccion = "FECHA_CONSTRUCCION";
        public const String Campo_Empleado_Construccion_ID = "EMPLEADO_CONSTRUCCION_ID";
        public const String Campo_Fecha_Generacion = "FECHA_GENERACION";
        public const String Campo_Empleado_Generacion_ID = "EMPLEADO_GENERACION_ID";
        public const String Campo_Fecha_Autorizacion = "FECHA_AUTORIZACION";
        public const String Campo_Empleado_Autorizacion_ID = "EMPLEADO_AUTORIZACION_ID";
        public const String Campo_Fecha_Cancelacion = "FECHA_CANCELACION";
        public const String Campo_Empleado_Cancelacion_ID = "EMPLEADO_CANCELACION_ID";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Req_Observaciones
    /// DESCRIPCION:            Clase que contiene los campos de la tabla OPE_COM_REQ_OBSERVACIONES
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :            08/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Alm_Com_Obs_Listado
    {
        public const String Tabla_Ope_Alm_Com_Obs_Listados = "OPE_ALM_COM_OBS_LISTADO";
        public const String Campo_Obs_listado_ID = "OBS_LISTADO_ID";
        public const String Campo_No_Listado_ID = "NO_LISTADO_ID";
        public const String Campo_Comentario = "COMENTARIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Com_Listado_Detalle
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla Ope_Com_Listado_Detalle
    /// PARÁMETROS :     
    /// CREO       : Susana Trigueros Armenta 
    /// FECHA_CREO : 20/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Listado_Detalle
    {
        public const String Tabla_Ope_Com_Listado_Detalle = "OPE_COM_LISTADO_DETALLE";
        public const String Campo_No_Listado_ID = "NO_LISTADO_ID";
        public const String Campo_No_Producto_ID = "NO_PRODUCTO_ID";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Costo_Compra = "COSTO_COMPRA";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Monto_IVA = "MONTO_IVA";
        public const String Campo_Monto_IEPS = "MONTO_IEPS";
        public const String Campo_Porcentaje_IVA = "PORCENTAJE_IVA";
        public const String Campo_Porcentaje_IEPS = "PORCENTAJE_IEPS";
        public const String Campo_No_Requisicion = "NO_REQUISICION";
        public const String Campo_Borrado = "BORRADO";
        public const String Campo_Motivo_Borrado = "MOTIVO_BORRADO";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Alm_Com_Entradas
    /// DESCRIPCIÓN:            Clase que contiene los campos de la tabla Alm_Com_Entradas
    /// PARÁMETROS :     
    /// CREO       :            Noe Mosqueda Valadez 
    /// FECHA_CREO :            24/Noviembre/2010 17:56 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Alm_Com_Entradas
    {
        public const String Tabla_Alm_Com_Entradas = "ALM_COM_ENTRADAS";
        public const String Campo_No_Entrada = "NO_ENTRADA";
        public const String Campo_Tipo_Entrada_ID = "TIPO_ENTRADA_ID";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Total = "TOTAL";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_No_Factura_Interno = "NO_FACTURA_INTERNO";
        public const String Campo_Empleado_Almacen_ID = "EMPLEADO_ALMACEN_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Alm_Com_Entradas_Detalles
    /// DESCRIPCIÓN:            Clase que contiene los campos de la tabla Alm_Com_Entradas_Detalles
    /// PARÁMETROS :     
    /// CREO       :            Noe Mosqueda Valadez 
    /// FECHA_CREO :            24/Noviembre/2010 18:00 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Alm_Com_Entradas_Detalles
    {
        public const String Tabla_Alm_Com_Entradas_Detalles = "ALM_COM_ENTRADAS_DETALLES";
        public const String Campo_No_Entrada = "NO_ENTRADA";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Costo_Compra = "COSTO_COMPRA";
        public const String Campo_Costo_Promedio = "COSTO_PROMEDIO";
        public const String Campo_No_Resguardo = "NO_RESGUARDO";
        public const String Campo_No_Recibo = "NO_RECIBO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Com_Facturas_Proveedores
    /// DESCRIPCIÓN:            Clase que contiene los campos de la tabla Ope_Com_Facturas_Proveedores
    /// PARÁMETROS :     
    /// CREO       :            Noe Mosqueda Valadez 
    /// FECHA_CREO :            26/Noviembre/2010 12:02 
    /// MODIFICO          :     Salvador Hernández Ramírez
    /// FECHA_MODIFICO    :     01/Marzo/2011
    /// CAUSA_MODIFICACIÓN:     Se agregaron los campos "Empleado_Almacen_ID" y "No_ContraRecibo" 
    ///*******************************************************************************
    public class Ope_Com_Facturas_Proveedores
    {
        public const String Tabla_Ope_Com_Facturas_Proveedores = "OPE_COM_FACTURAS_PROVEEDORES";
        public const String Campo_No_Factura_Interno = "NO_CONTRA_RECIBO";
        public const String Campo_No_Factura_Proveedor = "NO_FACTURA_PROVEEDOR";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Fecha_Factura = "FECHA_FACTURA";
        public const String Campo_Fecha_Recepcion = "FECHA_RECEPCION";
        public const String Campo_Fecha_Pago = "FECHA_PAGO";
        public const String Campo_SubTotal_Con_Impuesto = "SUBTOTAL_CON_IMPUESTO";
        public const String Campo_SubTotal_Sin_Impuesto = "SUBTOTAL_SIN_IMPUESTO";
        public const String Campo_IVA = "IVA";
        public const String Campo_IEPS = "IEPS";
        public const String Campo_Total = "TOTAL";
        public const String Campo_Abono = "ABONO";
        public const String Campo_Saldo = "SALDO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Cancelacion = "FECHA_CANCELACION";
        public const String Campo_Motivo_Cancelacion = "MOTIVO_CANCELACION";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Empleado_Almacen_ID = "EMPLEADO_ALMACEN_ID";
        public const String Campo_No_ContraRecibo = "NO_CONTRARECIBO";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Alm_Com_Contrarecibos
    /// DESCRIPCION:           Clase que contiene los campos de la tabla ALM_COM_CONTRARECIBOS
    /// PARAMETROS :     
    /// CREO       :           Noe Mosqueda Valadez
    /// FECHA_CREO :           29/Diciembre/2010 11:51 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Alm_Com_Contrarecibos
    {
        public const String Tabla_Alm_Com_Contrarecibos = "ALM_COM_CONTRARECIBOS";
        public const String Campo_No_ContraRecibo = "NO_CONTRARECIBO";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Empleado_Almacen_ID = "EMPLEADO_ALMACEN_ID";
        public const String Campo_Fecha_Recepcion = "FECHA_RECEPCION";
        public const String Campo_Fecha_Pago = "FECHA_PAGO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Alm_Com_Contrarecibos_Detalles
    /// DESCRIPCION:           Clase que contiene los campos de la tabla ALM_COM_CONTRARECIBOS_DETALLES
    /// PARAMETROS :     
    /// CREO       :           Noe Mosqueda Valadez
    /// FECHA_CREO :           29/Diciembre/2010 11:51 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Alm_Com_Contrarecibos_Detalles
    {
        public const String Tabla_Alm_Com_Contrarecibos_Detalles = "ALM_COM_CONTRARECIBOS_DETALLES";
        public const String Campo_No_ContraRecibo = "NO_CONTRARECIBO";
        public const String Campo_No_Factura_Proveedor = "NO_FACTURA_PROVEEDOR";
        public const String Campo_No_Partida = "NO_PARTIDA";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Ope_Com_Det_Doc_Soporte
    /// DESCRIPCION:           Clase que contiene los campos de la tabla OPE_COM_DET_DOC_SOPORTE
    /// PARAMETROS :     
    /// CREO       :           Salvador Hernández Ramírez
    /// FECHA_CREO :           17/Marzo/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Ope_Com_Det_Doc_Soporte
    {
        public const String Tabla_Ope_Com_Det_Doc_Soporte = "OPE_COM_DET_DOC_SOPORTE";
        public const String Campo_Documento_ID = "DOCUMENTO_ID";
        public const String Campo_No_Factura_Interno = "NO_CONTRA_RECIBO";
        public const String Campo_Empleado_Almacen_ID = "EMPLEADO_ALMACEN_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Marbete = "MARBETE";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:  Ope_Com_Series_Productos
    /// DESCRIPCIÓN:         Clase que contiene los campos de la tabla OPE_COM_SERIES_PRODUCTOS
    /// PARÁMETROS :     
    /// CREO       :         Salvador Hernández Ramírez
    /// FECHA_CREO :         24/Marzo/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Series_Productos
    {
        public const String Tabla_Ope_Com_Series_Productos = "OPE_COM_SERIES_PRODUCTOS";
        public const String Campo_No_Serie = "NO_SERIE";
        public const String Campo_No_Orden_Compra = "NO_ORDEN_COMPRA";
        public const String Campo_Producto_Id = "PRODUCTO_ID";
        public const String Campo_Marca_Id = "MARCA_ID";
        public const String Campo_Modelo_Id = "MODELO_ID";
        public const String Campo_Serie = "SERIE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_No_Recibo = "NO_RECIBO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Ope_Alm_Productos_Contrarecibo   
    /// DESCRIPCION:           Clase que contiene los campos de la tabla OPE_ALM_PROD_CONTRARECIBO
    /// PARAMETROS :     
    /// CREO       :           Salvador Hernández Ramírez
    /// FECHA_CREO :           03/Julio/2011 
    /// MODIFICO          :    
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    ///

    public class Ope_Alm_Productos_Contrarecibo
    {
        public const String Tabla_Ope_Alm_Productos_Contrarecibo = "OPE_ALM_PROD_CONTRARECIBO";
        public const String Campo_No_Registro = "NO_REGISTRO";
        public const String Campo_No_Contra_Recibo = "NO_CONTRA_RECIBO";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Resguardo = "RESGUARDO";
        public const String Campo_Recibo = "RECIBO";
        public const String Campo_Unidad = "UNIDAD";
        public const String Campo_Totalidad = "TOTALIDAD";
        public const String Campo_Recibo_Transitorio = "RECIBO_TRANSITORIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Registrado = "REGISTRADO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Ope_Alm_Registro_Facturas   
    /// DESCRIPCION:           Clase que contiene los campos de la tabla OPE_ALM_REGISTRO_FACTURAS
    /// PARAMETROS :     
    /// CREO       :           Salvador Hernández Ramírez
    /// FECHA_CREO :           03/Julio/2011 
    /// MODIFICO          :    
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Ope_Alm_Registro_Facturas
    {
        public const String Tabla_Ope_Alm_Registro_Facturas = "OPE_ALM_REGISTRO_FACTURAS";
        public const String Campo_Factura_ID = "FACTURA_ID";
        public const String Campo_Factura_Proveedor = "NO_FACTURA_PROVEEDOR";
        public const String Campo_No_Contra_Recibo = "NO_CONTRA_RECIBO";
        public const String Campo_Importe_Factura = "IMPORTE_FACTURA";
        public const String Campo_Fecha_Factura = "FECHA_FACTURA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:  Ope_Alm_Pat_Inv_Bienes_Muebles
    /// DESCRIPCIÓN:         Clase que contiene los campos de la tabla OPE_ALM_PAT_INV_B_MUEBLES
    /// PARÁMETROS :     
    /// CREO       :         Salvador Hernández Ramírez
    /// FECHA_CREO :         04/Julio/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Alm_Pat_Inv_Bienes_Muebles
    {
        public const String Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles = "OPE_ALM_PAT_INV_B_MUEBLES";
        public const String Campo_No_Inventario = "NO_INVENTARIO";
        public const String Campo_Inventario = "INVENTARIO";
        public const String Campo_Producto_Id = "PRODUCTO_ID";
        public const String Campo_Marca_Id = "MARCA_ID";
        public const String Campo_Modelo_Id = "MODELO_ID";
        public const String Campo_Color_Id = "COLOR_ID";
        public const String Campo_Material_Id = "MATERIAL_ID";
        public const String Campo_No_Serie = "NO_SERIE";
        public const String Campo_No_Contra_Recibo = "NO_CONTRA_RECIBO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Resguardado = "RESGUARDADO";

        public const String Campo_Modelo = "MODELO";
        public const String Campo_Garantia = "GARANTIA";
        public const String Campo_Operacion = "OPERACION";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:  Ope_Alm_Pat_Inv_Vehiculos
    /// DESCRIPCIÓN:         Clase que contiene los campos de la tabla OPE_ALM_PAT_INV_VEHICULOS
    /// PARÁMETROS :     
    /// CREO       :         Salvador Hernández Ramírez
    /// FECHA_CREO :         04/Julio/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Alm_Pat_Inv_Vehiculos
    {
        public const String Tabla_Ope_Alm_Pat_Inv_Vehiculos = "OPE_ALM_PAT_INV_VEHICULOS";
        public const String Campo_No_Inventario = "NO_INVENTARIO";
        public const String Campo_Inventario = "INVENTARIO";
        public const String Campo_Producto_Id = "PRODUCTO_ID";
        public const String Campo_Marca_Id = "MARCA_ID";
        public const String Campo_Modelo_Id = "MODELO_ID";
        public const String Campo_Color_Id = "COLOR_ID";
        public const String Campo_Material_Id = "MATERIAL_ID";
        public const String Campo_No_Serie = "NO_SERIE";
        public const String Campo_No_Contra_Recibo = "NO_CONTRA_RECIBO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Modelo = "MODELO";
        public const String Campo_Descripcion = "DESCRIPCION";
    }


    #endregion




    ///*************************************************************************************************************************
    ///                                                                SubRegion Control Patrimonial
    ///*************************************************************************************************************************
    #region SubRegion_Control_Patrimonial
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Clases_Activo
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_CLASES_ACTIVO
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 23/Enero/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Clases_Activo
    {
        public const String Tabla_Cat_Pat_Clases_Activo = "CAT_PAT_CLASES_ACTIVO";
        public const String Campo_Clase_Activo_ID = "CLASE_ACTIVO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Tabla_Ope_Pat_Bienes_Sin_Inv
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_BIENES_SIN_INV
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 03/Noviembre/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///********************************************************************************
    public class Ope_Pat_Bienes_Sin_Inv
    {
        public const String Tabla_Ope_Pat_Bienes_Sin_Inv = "OPE_PAT_BIENES_SIN_INV";
        public const String Campo_Bien_ID = "BIEN_ID";
        public const String Campo_Bien_Parent_ID = "BIEN_PARENT_ID";
        public const String Campo_Tipo_Parent = "TIPO_PARENT";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Marca_ID = "MARCA_ID";
        public const String Campo_Costo_Inicial = "COSTO_INICIAL";
        public const String Campo_Material_ID = "MATERIAL_ID";
        public const String Campo_Color_ID = "COLOR_ID";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Motivo_Baja = "MOTIVO_BAJA";
        public const String Campo_Fecha_Adquisicion = "FECHA_ADQUISICION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Modelo = "MODELO";
        public const String Campo_Numero_Serie = "NUMERO_SERIE";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Zonas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_ZONAS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 23/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Zonas
    {
        public const String Tabla_Cat_Pat_Zonas = "CAT_PAT_ZONAS";
        public const String Campo_Zona_ID = "ZONA_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Clasificaciones
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_CLASIFICACIONES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 23/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Clasificaciones
    {
        public const String Tabla_Cat_Pat_Clasificaciones = "CAT_PAT_CLASIFICACIONES";
        public const String Campo_Clasificacion_ID = "CLASIFICACION_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Clave = "CLAVE";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Tipos_Bajas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_TIPOS_BAJAS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 23/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Tipos_Bajas
    {
        public const String Tabla_Cat_Pat_Tipos_Bajas = "CAT_PAT_TIPOS_BAJAS";
        public const String Campo_Tipo_Baja_ID = "TIPO_BAJA_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Tipos_Vehiculo
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_TIPOS_VEHICULO
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 23/Noviembre/2010 
    /// MODIFICO          :Franciso Antonio Gallardo Castañeda
    /// FECHA_MODIFICO    :11/Marzo/2011
    /// CAUSA_MODIFICACIÓN:Se le agrego la parte de Aseguradora a el tipo de Vehículo.
    ///*******************************************************************************
    public class Cat_Pat_Tipos_Vehiculo
    {
        public const String Tabla_Cat_Pat_Tipos_Vehiculo = "CAT_PAT_TIPOS_VEHICULO";
        public const String Campo_Tipo_Vehiculo_ID = "TIPO_VEHICULO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Aseguradora_ID = "ASEGURADORA_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Tipos_Combustible
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_TIPOS_COMBUSTIBLE
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 23/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Tipos_Combustible
    {
        public const String Tabla_Cat_Pat_Tipos_Combustible = "CAT_PAT_TIPOS_COMBUSTIBLE";
        public const String Campo_Tipo_Combustible_ID = "TIPO_COMBUSTIBLE_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Colores
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_COLORES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 23/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Colores
    {
        public const String Tabla_Cat_Pat_Colores = "CAT_PAT_COLORES";
        public const String Campo_Color_ID = "COLOR_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Materiales
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_MATERIALES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 24/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Materiales
    {
        public const String Tabla_Cat_Pat_Materiales = "CAT_PAT_MATERIALES";
        public const String Campo_Material_ID = "MATERIAL_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Aseguradora
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_ASEGURADORAS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 24/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Aseguradora
    {
        public const String Tabla_Cat_Pat_Aseguradora = "CAT_PAT_ASEGURADORAS";
        public const String Campo_Aseguradora_ID = "ASEGURADORA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Nombre_Fiscal = "NOMBRE_FISCAL";
        public const String Campo_Nombre_Comercial = "NOMBRE_COMERCIAL";
        public const String Campo_RFC = "RFC";
        public const String Campo_Cuenta_Contable = "CUENTA_CONTABLE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Aseguradora_Contacto
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_ASEGURADORA_CONTACTO
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 24/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Aseguradora_Contacto
    {
        public const String Tabla_Cat_Pat_Aseguradora_Contacto = "CAT_PAT_ASEGURADORA_CONTACTO";
        public const String Campo_Aseguradora_Contacto_ID = "ASEGURADORA_CONTACTO_ID";
        public const String Campo_Aseguradora_ID = "ASEGURADORA_ID";
        public const String Campo_Dato_Contacto = "DATO_CONTACTO";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Registrado = "REGISTRADO";
        public const String Campo_Telefono = "TELEFONO";
        public const String Campo_Fax = "FAX";
        public const String Campo_Celular = "CELULAR";
        public const String Campo_Correo_Electronico = "CORREO_ELECTRONICO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Aseg_Contacto
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_ASEG_DOMICILIO
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 24/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Aseg_Domicilio
    {
        public const String Tabla_Cat_Pat_Aseg_Domicilio = "CAT_PAT_ASEG_DOMICILIO";
        public const String Campo_Aseguradora_Domicilio_ID = "ASEGURADORA_DOMICILIO_ID";
        public const String Campo_Aseguradora_ID = "ASEGURADORA_ID";
        public const String Campo_Domicilio = "DOMICILIO";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Registrado = "REGISTRADO";
        public const String Campo_Calle = "CALLE";
        public const String Campo_Numero_Exterior = "NUMERO_EXTERIOR";
        public const String Campo_Numero_Interior = "NUMERO_INTERIOR";
        public const String Campo_Fax = "FAX";
        public const String Campo_Colonia = "COLONIA";
        public const String Campo_Codigo_Postal = "CODIGO_POSTAL";
        public const String Campo_Ciudad = "CUIDAD";
        public const String Campo_Municipio = "MUNICIPIO";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Pais = "PAIS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Aseguradora_Bancos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_ASEGURADORA_BANCOS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 24/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Aseguradora_Bancos
    {
        public const String Tabla_Cat_Pat_Aseguradora_Bancos = "CAT_PAT_ASEGURADORA_BANCOS";
        public const String Campo_Aseguradora_Banco_ID = "ASEGURADORA_BANCO_ID";
        public const String Campo_Aseguradora_ID = "ASEGURADORA_ID";
        public const String Campo_Producto_Bancario = "PRODUCTO_BANCARIO";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Registrado = "REGISTRADO";
        public const String Campo_Institucion_Bancaria = "INSTITUCION_BANCARIA";
        public const String Campo_Cuenta = "CUENTA";
        public const String Campo_Clabe_Institucion_Bancaria = "CLABE_INSTITUCION_BANCARIA";
        public const String Campo_Clabe_Plaza = "CLABE_PLAZA";
        public const String Campo_Clabe_Cuenta = "CLABE_CUENTA";
        public const String Campo_Clabe_Digito_Verificador = "CLABE_DIGITO_VERIFICADOR";
        public const String Campo_Clave_Cie = "CLAVE_CIE";
        public const String Campo_Numero_Tarjeta = "NUMERO_TARJETA";
        public const String Campo_Numero_Tarjeta_Reverso = "NUMERO_TARJETA_REVERSO";
        public const String Campo_Fecha_Vigencia = "FECHA_VIGENCIA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Bienes_Muebles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_BIENES_MUEBLES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 26/Noviembre/2010 
    /// MODIFICO          : Salvador Hernández Ramírez
    /// FECHA_MODIFICO    : 08/Enero/2011
    /// CAUSA_MODIFICACIÓN: Se agregaron las constantes "Campo_Cantidad, Campo_Fecha_Inventario, Campo_Proveniente,Campo_Donador_ID,Campo_Fecha_Adquisicion,Campo_Nombre,Campo_Marca_ID,Campo_Modelo_ID"
    ///*******************************************************************************
    public class Ope_Pat_Bienes_Muebles
    {
        public const String Tabla_Ope_Pat_Bienes_Muebles = "OPE_PAT_BIENES_MUEBLES";
        public const String Campo_Bien_Mueble_ID = "BIEN_MUEBLE_ID";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Area_ID = "AREA_ID";
        public const String Campo_Material_ID = "MATERIAL_ID";
        public const String Campo_Color_ID = "COLOR_ID";
        public const String Campo_Clasificacion_ID = "CLASIFICACION_ID";
        public const String Campo_Clase_Activo_ID = "CLASE_ACTIVO_ID";
        public const String Campo_Numero_Inventario = "NUMERO_INVENTARIO";
        public const String Campo_Clave_Sistema = "CLAVE_SISTEMA";
        public const String Campo_Clave_Inventario = "CLAVE_INVENTARIO";
        public const String Campo_Factura = "FACTURA";
        public const String Campo_Numero_Serie = "NUMERO_SERIE";
        public const String Campo_Costo_Alta = "COSTO_ALTA";
        public const String Campo_Costo_Actual = "COSTO_ACTUAL";
        public const String Campo_Costo_Baja = "COSTO_BAJA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Motivo_Baja = "MOTIVO_BAJA";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Observadores = "OBSERVACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Fecha_Inventario = "FECHA_INVENTARIO";
        public const String Campo_Procedencia = "PROCEDENCIA";
        public const String Campo_Proveniente = "PROVENIENTE";
        public const String Campo_Donador_ID = "DONADOR_ID";
        public const String Campo_Fecha_Adquisicion = "FECHA_ADQUISICION";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Marca_ID = "MARCA_ID";
        public const String Campo_Modelo_ID = "MODELO_ID";
        public const String Campo_Bien_Parent_ID = "BIEN_PARENT_ID";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Operacion = "OPERACION";
        public const String Campo_Modelo = "MODELO";
        public const String Campo_Garantia = "GARANTIA";
        public const String Campo_No_Inventario_Anterior = "NO_INVENTARIO_ANTERIOR";
        public const String Campo_Zona_ID = "ZONA_ID";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Bienes_Resguardos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_BIEN_MUE_RESGUARDOS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 29/Noviembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_Bienes_Resguardos
    {
        public const String Tabla_Ope_Pat_Bienes_Resguardos = "OPE_PAT_BIENES_RESGUARDOS";
        public const String Campo_Bien_Resguardo_ID = "BIEN_RESGUARDO_ID";
        public const String Campo_Bien_ID = "BIEN_ID";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Empleado_Resguardo_ID = "EMPLEADO_RESGUARDO_ID";
        public const String Campo_Fecha_Inicial = "FECHA_INICIAL";
        public const String Campo_Fecha_Final = "FECHA_FINAL";
        public const String Campo_Empleado_Almacen_ID = "EMPLEADO_ALMACEN_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

        public const String Campo_Movimiento_Alta = "MOVIMIENTO_ALTA";
        public const String Campo_Movimiento_Modificacion = "MOVIMIENTO_MODIFICACION";
        public const String Campo_Movimiento_Baja = "MOVIMIENTO_BAJA";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Observaciones = "OBSERVACIONES";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Vehiculos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_VEHICULOS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 03/Diciembre/2010 
    /// MODIFICO          : Salvador Hernández Ramírez
    /// FECHA_MODIFICO    : 28/diciembre/2010
    /// CAUSA_MODIFICACIÓN: Se agrego la constante "Campo_Cantidad"
    ///*******************************************************************************
    public class Ope_Pat_Vehiculos
    {
        public const String Tabla_Ope_Pat_Vehiculos = "OPE_PAT_VEHICULOS";
        public const String Campo_Vehiculo_ID = "VEHICULO_ID";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Tipo_Vehiculo_ID = "TIPO_VEHICULO_ID";
        public const String Campo_Tipo_Combustible_ID = "TIPO_COMBUSTIBLE_ID";
        public const String Campo_Color_ID = "COLOR_ID";
        public const String Campo_Zona_ID = "ZONA_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Numero_Inventario = "NUMERO_INVENTARIO";
        public const String Campo_Numero_Economico = "NUMERO_ECONOMICO";
        public const String Campo_Capacidad_Carga = "CAPACIDAD_CARGA";
        public const String Campo_Placas = "PLACAS";
        public const String Campo_Anio_Fabricacion = "ANIO_FABRICACION";
        public const String Campo_Serie_Carroceria = "SERIE_CARROCERIA";
        public const String Campo_Serie_Motor = "SERIE_MOTOR";
        public const String Campo_Numero_Cilindros = "NUMERO_CILINDROS";
        public const String Campo_Kilometraje = "KILOMETRAJE";
        public const String Campo_Numero_Ejes = "NUMERO_EJES";
        public const String Campo_Odometro = "ODOMETRO";
        public const String Campo_Fecha_Adquisicion = "FECHA_ADQUISICION";
        public const String Campo_Costo_Alta = "COSTO_ALTA";
        public const String Campo_Costo_Actual = "COSTO_ACTUAL";
        public const String Campo_Costo_Baja = "COSTO_BAJA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Clave_Programatica_Revision = "CLAVE_PROGRAMATICA_REVISION";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Motivo_Baja = "MOTIVO_BAJA";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Procedencia = "PROCEDENCIA";
        public const String Campo_Proveniente = "PROVENIENTE";
        public const String Campo_Donador_ID = "DONADOR_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Marca_ID = "MARCA_ID";
        public const String Campo_Modelo_ID = "MODELO_ID";
        public const String Campo_No_Factura = "NO_FACTURA";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Modelo = "MODELO";
        public const String Campo_Empleado_Operador_ID = "EMPLEADO_OPERADOR_ID";
        public const String Campo_Empleado_Recibe_ID = "EMPLEADO_RECIBE_ID";
        public const String Campo_Empleado_Autorizo_ID = "EMPLEADO_AUTORIZO_ID";
        public const String Campo_Clasificacion_ID = "CLASIFICACION_ID";
        public const String Campo_Clase_Activo_ID = "CLASE_ACTIVO_ID";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Vehiculo_Aseguradora
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_VEHICULO_ASEGURADORA
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 08/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_Vehiculo_Aseguradora
    {
        public const String Tabla_Ope_Pat_Vehiculo_Aseguradora = "OPE_PAT_VEHICULO_ASEGURADORA";
        public const String Campo_Vehiculo_Aseguradora_ID = "VEHICULO_ASEGURADORA_ID";
        public const String Campo_Aseguradora_ID = "ASEGURADORA_ID";
        public const String Campo_Tipo_Vehiculo_ID = "TIPO_VEHICULO_ID";
        public const String Campo_Descripcion_Seguro = "DESCRIPCION_SEGURO";
        public const String Campo_Cobertura = "COBERTURA";
        public const String Campo_No_Poliza = "NO_POLIZA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Vehiculo_ID = "VEHICULO_ID";
    }




    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Tipos_Alimentacion
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_TIPOS_ALIMENTACION
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 10/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Tipos_Alimentacion
    {
        public const String Tabla_Cat_Pat_Tipos_Alimentacion = "CAT_PAT_TIPOS_ALIMENTACION";
        public const String Campo_Tipo_Alimentacion_ID = "TIPO_ALIMENTACION_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Vacunas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_VACUNAS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 10/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Vacunas
    {
        public const String Tabla_Cat_Pat_Vacunas = "CAT_PAT_VACUNAS";
        public const String Campo_Vacuna_ID = "VACUNA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Fecha_Aplicacion = "FECHA_APLICACION";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Tipos_Adiestramiento
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_TIPOS_ADIESTRAMIENTO
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 10/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Tipos_Adiestramiento
    {
        public const String Tabla_Cat_Pat_Tipos_Adiestramiento = "CAT_PAT_TIPOS_ADIESTRAMIENTO";
        public const String Campo_Tipo_Adiestramiento_ID = "TIPO_ADIESTRAMIENTO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Razas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_RAZAS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 10/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Razas
    {
        public const String Tabla_Cat_Pat_Razas = "CAT_PAT_RAZAS";
        public const String Campo_Raza_ID = "RAZA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Tipos_Ascendencia
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_TIPOS_ASCENDENCIA
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 10/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Tipos_Ascendencia
    {
        public const String Tabla_Cat_Pat_Tipos_Ascendencia = "CAT_PAT_TIPOS_ASCENDENCIA";
        public const String Campo_Tipo_Ascendencia_ID = "TIPO_ASCENDENCIA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Funciones
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_FUNCIONES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 10/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Funciones
    {
        public const String Tabla_Cat_Pat_Funciones = "CAT_PAT_FUNCIONES";
        public const String Campo_Funcion_ID = "FUNCION_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Veterinarios
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_VETERINARIOS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 10/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Veterinarios
    {
        public const String Tabla_Cat_Pat_Veterinarios = "CAT_PAT_VETERINARIOS";
        public const String Campo_Veterinario_ID = "VETERINARIO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Apellido_Paterno = "APELLIDO_PATERNO";
        public const String Campo_Apellido_Materno = "APELLIDO_MATERNO";
        public const String Campo_Direccion = "DIRECCION";
        public const String Campo_Cuidad = "CUIDAD";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Telefono = "TELEFONO";
        public const String Campo_Celular = "CELULAR";
        public const String Campo_CURP = "CURP";
        public const String Campo_RFC = "RFC";
        public const String Campo_Cedula_Profesional = "CEDULA_PROFESIONAL";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Cemovientes
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_CEMOVIENTES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 10/Diciembre/2010 
    /// MODIFICO          :   Salvador Hernández Ramírez
    /// FECHA_MODIFICO    :  28/Diciembre/2010
    /// CAUSA_MODIFICACIÓN:  Se agrego la constante "Campo_Cantidad"
    ///*******************************************************************************
    public class Ope_Pat_Cemovientes
    {
        public const String Tabla_Ope_Pat_Cemovientes = "OPE_PAT_CEMOVIENTES";
        public const String Campo_Cemoviente_ID = "CEMOVIENTE_ID";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Tipo_Alimentacion_ID = "TIPO_ALIMENTACION_ID";
        public const String Campo_Tipo_Adiestramiento_ID = "TIPO_ADIESTRAMIENTO_ID";
        public const String Campo_Raza_ID = "RAZA_ID";
        public const String Campo_Funcion_ID = "FUNCION_ID";
        public const String Campo_Veterinario_ID = "VETERINARIO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Numero_Inventario = "NUMERO_INVENTARIO";
        public const String Campo_Color_ID = "COLOR_ID";
        public const String Campo_Costo_Alta = "COSTO_ALTA";
        public const String Campo_Costo_Actual = "COSTO_ACTUAL";
        public const String Campo_Costo_Baja = "COSTO_BAJA";
        public const String Campo_Tipo_Ascendencia = "TIPO_ASCENDENCIA";
        public const String Campo_Padre_ID = "PADRE_ID";
        public const String Campo_Madre_ID = "MADRE_ID";
        public const String Campo_Sexo = "SEXO";
        public const String Campo_Tipo_Cemoviente_ID = "TIPO_CEMOVIENTE_ID";
        public const String Campo_Fecha_Nacimiento = "FECHA_NACIMIENTO";
        public const String Campo_Fecha_Adquisicion = "FECHA_ADQUISICION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Motivo_Baja = "MOTIVO_BAJA";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Cantidad = "CANTIDAD";

        public const String Campo_Procedencia = "PROCEDENCIA";
        public const String Campo_Proveniente = "PROVENIENTE";
        public const String Campo_Donador_ID = "DONADOR_ID";

        public const String Campo_No_Factura = "NO_FACTURA";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_No_Inventario_Anterior = "NO_INVENTARIO_ANTERIOR";

        public const String Campo_Clasificacion_ID = "CLASIFICACION_ID";
        public const String Campo_Clase_Activo_ID = "CLASE_ACTIVO_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Tipos_Cemovientes
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_TIPOS_CEMOVIENTES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 21/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Tipos_Cemovientes
    {
        public const String Tabla_Cat_Pat_Tipos_Cemovientes = "CAT_PAT_TIPOS_CEMOVIENTES";
        public const String Campo_Tipo_Cemoviente_ID = "TIPO_CEMOVIENTE_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Vacunas_Cemovientes
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_VACUNAS_CEMOVIENTES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 21/Diciembre/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_Vacunas_Cemovientes
    {
        public const String Tabla_Ope_Pat_Vacunas_Cemovientes = "OPE_PAT_VACUNAS_CEMOVIENTES";
        public const String Campo_Vacuna_Cemoviente_ID = "VACUNA_CEMOVIENTE_ID";
        public const String Campo_Vacuna_ID = "VACUNA_ID";
        public const String Campo_Cemoviente_ID = "CEMOVIENTE_ID";
        public const String Campo_Veterinario_ID = "VETERINARIO_ID";
        public const String Campo_Fecha_Aplicacion = "FECHA_APLICACION";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Motivo_Cancelacion = "MOTIVO_CANCELACION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Tipos_Siniestros
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_TIPOS_SINIESTROS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 15/Enero/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Tipos_Siniestros
    {
        public const String Tabla_Cat_Pat_Tipos_Siniestros = "CAT_PAT_TIPOS_SINIESTROS";
        public const String Campo_Tipo_Siniestro_ID = "TIPO_SINIESTRO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Sinies_Observaciones
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_SINIES_OBSERVACIONES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 15/Enero/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_Sinies_Observaciones
    {
        public const String Tabla_Ope_Pat_Sinies_Observaciones = "OPE_PAT_SINIES_OBSERVACIONES";
        public const String Campo_Observacion_ID = "OBSERVACION_ID";
        public const String Campo_Siniestro_ID = "SINIESTRO_ID";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Observacion = "OBSERVACION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Siniestros
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_SINIESTROS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 15/Enero/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_Siniestros
    {
        public const String Tabla_Ope_Pat_Siniestros = "OPE_PAT_SINIESTROS";
        public const String Campo_Siniestro_ID = "SINIESTRO_ID";
        public const String Campo_Tipo_Siniestro_ID = "TIPO_SINIESTRO_ID";
        public const String Campo_Bien_ID = "BIEN_ID";
        public const String Campo_Aseguradora_ID = "ASEGURADORA_ID";
        public const String Campo_Tipo_Bien = "TIPO_BIEN";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Parte_Averiguacion = "PARTE_AVERIGUACION";
        public const String Campo_Reparacion = "REPARACION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Responsable_Municipio = "RESPONSABLE_MUNICIPIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Numero_Reporte = "NUMERO_REPORTE";
        public const String Campo_Consignado = "CONSIGNADO";
        public const String Campo_Pago_Danio_Sindicos = "PAGO_DANIO_SINDICOS";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Donadores
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_DONADORES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 21/enero/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Donadores
    {
        public const String Tabla_Cat_Pat_Donadores = "CAT_PAT_DONADORES";
        public const String Campo_Donador_ID = "DONADOR_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Apellido_Paterno = "APELLIDO_PATERNO";
        public const String Campo_Apellido_Materno = "APELLIDO_MATERNO";
        public const String Campo_Direccion = "DIRECCION";
        public const String Campo_Cuidad = "CUIDAD";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Telefono = "TELEFONO";
        public const String Campo_Celular = "CELULAR";
        public const String Campo_CURP = "CURP";
        public const String Campo_RFC = "RFC";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Bienes_Caja_Chica
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_BIENES_CAJA_CHICA
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 24/Enero/2010 
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************
    public class Ope_Pat_Bienes_Caja_Chica
    {
        public const String Tabla_Ope_Pat_Bienes_Caja_Chica = "OPE_PAT_BIENES_CAJA_CHICA";
        public const String Campo_Bien_ID = "BIEN_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Marca_ID = "MARCA_ID";
        public const String Campo_Modelo_ID = "MODELO_ID";
        public const String Campo_Costo = "COSTO";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Numero_Inventario = "NUMERO_INVENTARIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Material_ID = "MATERIAL_ID";
        public const String Campo_Color_ID = "COLOR_ID";
        public const String Campo_Motivo_Baja = "MOTIVO_BAJA";
        public const String Campo_Fecha_Adquisicion = "FECHA_ADQUISICION";
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Tabla_Ope_Pat_Partes_Vehiculos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_PARTES_VEHICULOS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 02/Febrero/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///********************************************************************************
    public class Ope_Pat_Partes_Vehiculos
    {
        public const String Tabla_Ope_Pat_Partes_Vehiculos = "OPE_PAT_PARTES_VEHICULOS";
        public const String Campo_Parte_ID = "PARTE_ID";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Vehiculo_ID = "VEHICULO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Marca_ID = "MARCA_ID";
        public const String Campo_Modelo_ID = "MODELO_ID";
        public const String Campo_Costo = "COSTO";
        public const String Campo_Material_ID = "MATERIAL_ID";
        public const String Campo_Color_ID = "COLOR_ID";
        public const String Campo_Cantidad = "CANTIDAD";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Numero_Inventario = "NUMERO_INVENTARIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Motivo_Baja = "MOTIVO_BAJA";
        public const String Campo_Fecha_Adquisicion = "FECHA_ADQUISICION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Modelo = "MODELO";
    }



    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Archivos_Bienes
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_ARCHIVOS_BIENES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 14/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_Archivos_Bienes
    {
        public const String Tabla_Ope_Pat_Archivos_Bienes = "OPE_PAT_ARCHIVOS_BIENES";
        public const String Campo_Archivo_Bien_ID = "ARCHIVO_BIEN_ID";
        public const String Campo_Bien_ID = "BIEN_ID";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Archivo = "ARCHIVO";
        public const String Campo_Tipo_Archivo = "TIPO_ARCHIVO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Ruta_Fisica_Archivos = "CONTROL_PATRIMONIAL_ARCHIVOS_BIENES";
        public const String Campo_Descripcion_Archivo = "DESCRIPCION_ARCHIVO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Tipo_Vehiculo_Archivo
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_TIPO_VEHICULO_ARCHIVO
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 01/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Tipo_Vehiculo_Archivo
    {
        public const String Tabla_Cat_Pat_Tipo_Vehiculo_Archivo = "CAT_PAT_TIPO_VEHICULO_ARCHIVO";
        public const String Campo_Archivo_ID = "ARCHIVO_ID";
        public const String Campo_Tipo_Vehiculo_ID = "TIPO_VEHICULO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Nombre_Archivo = "NOMBRE_ARCHIVO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Ruta_Fisica_Archivos = "CONTROL_PATRIMONIAL_ARCHIVOS_TIPOS_VEHICULOS";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Det_Vehiculos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_DET_VEHICULOS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 07/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Det_Vehiculos
    {
        public const String Tabla_Cat_Pat_Det_Vehiculos = "CAT_PAT_DET_VEHICULOS";
        public const String Campo_Detalle_Vehiculo_ID = "DETALLE_VEHICULO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Tipo_Veh_Detalles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_TIPO_VEH_DETALLES
    /// PARÁMETROS :      
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 07/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Tipo_Veh_Detalles
    {
        public const String Tabla_Cat_Pat_Tipo_Veh_Detalles = "CAT_PAT_TIPO_VEH_DETALLES";
        public const String Campo_Registro_ID = "REGISTRO_ID";
        public const String Campo_Detalle_Vehiculo_ID = "DETALLE_VEHICULO_ID";
        public const String Campo_Tipo_Vehiculo_ID = "TIPO_VEHICULO_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Procedencias
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_PROCEDENCIAS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 07/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Procedencias
    {
        public const String Tabla_Cat_Pat_Procedencias = "CAT_PAT_PROCEDENCIAS";
        public const String Campo_Procedencia_ID = "PROCEDENCIA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Vehiculo_Detalles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_TIPO_VEH_DETALLES
    /// PARÁMETROS :      
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 09/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Vehiculo_Detalles
    {
        public const String Tabla_Cat_Pat_Vehiculo_Detalles = "OPE_PAT_VEHICULO_DETALLES";
        public const String Campo_Registro_ID = "REGISTRO_ID";
        public const String Campo_Vehiculo_ID = "VEHICULO_ID";
        public const String Campo_Detalle_Vehiculo_ID = "DETALLE_VEHICULO_ID";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }







    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Usos_Inmuebles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_USOS_INMUEBLES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 23/Febrero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Usos_Inmuebles
    {
        public const String Tabla_Cat_Pat_Usos_Inmuebles = "CAT_PAT_USOS_INMUEBLES";
        public const String Campo_Uso_ID = "USO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Origenes_Inmuebles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_ORIGENES_INMUEBLES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 06/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Origenes_Inmuebles
    {
        public const String Tabla_Cat_Pat_Origenes_Inmuebles = "CAT_PAT_ORIGENES_INMUEBLES";
        public const String Campo_Origen_ID = "ORIGEN_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Destinos_Inmuebles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_DESTINOS_INMUEBLES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 23/Febrero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Destinos_Inmuebles
    {
        public const String Tabla_Cat_Pat_Destinos_Inmuebles = "CAT_PAT_DESTINOS_INMUEBLES";
        public const String Campo_Destino_ID = "DESTINO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Bienes_Inmuebles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_BIENES_INMUEBLES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 28/Febrero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_Bienes_Inmuebles
    {
        public const String Tabla_Ope_Pat_Bienes_Inmuebles = "OPE_PAT_BIENES_INMUEBLES";
        public const String Campo_Bien_Inmueble_ID = "BIEN_INMUEBLE_ID";
        public const String Campo_Calle_ID = "CALLE_ID";
        public const String Campo_Colonia_ID = "COLONIA_ID";
        public const String Campo_Uso_ID = "USO_ID";
        public const String Campo_Superficie = "SUPERFICIE";
        public const String Campo_Construccion = "CONSTRUCCION";
        public const String Campo_Manzana = "MANZANA";
        public const String Campo_Cuenta_Predial_ID = "CUENTA_PREDIAL_ID";
        public const String Campo_Lote = "LOTE";
        public const String Campo_Porcentaje_Ocupacion = "PORCENTAJE_OCUPACION";
        public const String Campo_Efectos_Fiscales = "EFECTOS_FISCALES";
        public const String Campo_Clasificacion_Zona_ID = "CLASIFICACION_ZONA";
        public const String Campo_Tipo_Predio_ID = "TIPO_PREDIO_ID";
        public const String Campo_Vias_Acceso = "VIAS_ACCESO";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Densidad_Construccion = "DENSIDAD_CONTRUCCION";
        public const String Campo_Valor_Comercial = "VALOR_COMERCIAL";
        public const String Campo_Numero_Exterior = "NUMERO_EXTERIOR";
        public const String Campo_Numero_Interior = "NUMERO_INTERIOR";
        public const String Campo_Destino_ID = "DESTINO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Origen_ID = "ORIGEN_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Registro = "FECHA_REGISTRO";
        public const String Campo_Sector_ID = "SECTOR_ID";
        public const String Campo_Area_ID = "AREA_ID";

        public const String Campo_Hoja = "HOJA";
        public const String Campo_Tomo = "TOMO";
        public const String Campo_Numero_Acta = "NUMERO_ACTA";
        public const String Campo_Cartilla_Parcelaria = "CARTILLA_PARCELARIA";
        public const String Campo_Superficie_Contable = "SUPERFICIE_CONTABLE";
        public const String Campo_Unidad_Superficie = "UNIDAD_SUPERFICIE";
        public const String Campo_Clase_Activo_ID = "CLASE_ACTIVO_ID";
        public const String Campo_Tipo_Bien = "TIPO_BIEN";
        public const String Campo_Fecha_Baja = "FECHA_BAJA";
        public const String Campo_Registro_Propiedad = "REGISTRO_PROPIEDAD";
        public const String Campo_Fecha_Alta_Cta_Pub = "FECHA_ALTA_CTA_PUB";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_B_Inm_Observaciones
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_B_INM_OBSERVACIONES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 1/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_B_Inm_Observaciones
    {
        public const String Tabla_Ope_Pat_B_Inm_Observaciones = "OPE_PAT_B_INM_OBSERVACIONES";
        public const String Campo_No_Observacion = "NO_OBSERVACION";
        public const String Campo_Bien_Inmueble_ID = "BIEN_INMUEBLE_ID";
        public const String Campo_Fecha_Observacion = "FECHA_OBSERVACION";
        public const String Campo_Usuario_Observacion_ID = "USUARIO_OBSERVACION_ID";
        public const String Campo_Observacion = "OBSERVACION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_B_Inm_Medidas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_B_INM_MEDIDAS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 1/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_B_Inm_Medidas
    {
        public const String Tabla_Ope_Pat_B_Inm_Medidas = "OPE_PAT_B_INM_MEDIDAS";
        public const String Campo_No_Registro = "NO_REGISTRO";
        public const String Campo_Bien_Inmueble_ID = "BIEN_INMUEBLE_ID";
        public const String Campo_Segun = "SEGUN";
        public const String Campo_Orientacion_ID = "ORIENTACION_ID";
        public const String Campo_Medida = "MEDIDA";
        public const String Campo_Colindancia = "COLINDANCIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_B_Inm_Archivos
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_B_INM_ARCHIVOS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 1/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_B_Inm_Archivos
    {
        public const String Tabla_Ope_Pat_B_Inm_Archivos = "OPE_PAT_B_INM_ARCHIVOS";
        public const String Campo_No_Registro = "NO_REGISTRO";
        public const String Campo_Bien_Inmueble_ID = "BIEN_INMUEBLE_ID";
        public const String Campo_Tipo_Archivo = "TIPO_ARCHIVO";
        public const String Campo_Descripcion_Archivo = "DESCRIPCION_ARCHIVO";
        public const String Campo_Ruta_Archivo = "RUTA_ARCHIVO";
        public const String Campo_Usuario_Cargo_ID = "USUARIO_CARGO_ID";
        public const String Campo_Fecha_Cargo = "FECHA_CARGO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Ruta_Archivos_Inmuebles = "../../CONTROL_PATRIMONIAL_ARCHIVOS_BIENES/BIENES_INMUEBLES/";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_B_Inm_Juridico
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_B_INM_ARCHIVOS
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 1/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_B_Inm_Juridico
    {
        public const String Tabla_Ope_Pat_B_Inm_Juridico = "OPE_PAT_B_INM_JURIDICO";
        public const String Campo_No_Registro = "NO_REGISTRO";
        public const String Campo_Bien_Inmueble_ID = "BIEN_INMUEBLE_ID";
        public const String Campo_Escritura = "ESCRITURA";
        public const String Campo_Fecha_Escritura = "FECHA_ESCRITURA";
        public const String Campo_No_Notario = "NO_NOTARIO";
        public const String Campo_Nombre_Notario = "NOMBRE_NOTARIO";
        public const String Campo_Constancia_Registral = "CONSTANCIA_REGISTRAL";
        public const String Campo_Folio_Real = "FOLIO_REAL";
        public const String Campo_Libertad_Gravament = "LIBERTAD_GRAVAMEN";
        public const String Campo_Antecedente_Registral = "ANTECEDENTE_REGISTRAL";
        public const String Campo_Proveedor = "PROVEEDOR";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_No_Contrato = "NO_CONTRATO";
        public const String Campo_Movimiento = "MOVIMIENTO";
        public const String Campo_Nuevo_Propietario = "NUEVO_PROPIETARIO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Sessiones
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_SESSIONES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 1/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_Sessiones
    {
        public const String Tabla_Ope_Pat_Sessiones = "OPE_PAT_SESSIONES";
        public const String Campo_No_Registro = "NO_REGISTRO";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Fecha_Session = "FECHA_SESSION";
        public const String Campo_Ruta_Archivo = "RUTA_ARCHIVO";
        public const String Campo_Usuario_Creo_ID = "USUARIO_CREO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico_ID = "USUARIO_MODIFICO_ID";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Orientaciones_Inm
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_ORIENTACIONES_INM
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 09/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Orientaciones_Inm
    {
        public const String Tabla_Cat_Pat_Orientaciones_Inm = "CAT_PAT_ORIENTACIONES_INM";
        public const String Campo_Orientacion_ID = "ORIENTACION_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Clasif_Zona_Inm
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_CLASIF_ZONA_INM
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 10/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Clasif_Zona_Inm
    {
        public const String Tabla_Cat_Pat_Clasif_Zona_Inm = "CAT_PAT_CLASIF_ZONA_INM";
        public const String Campo_Clasificacion_ID = "CLASIFICACION_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_B_Inm_Expropiaciones
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_B_INM_EXPROPIACIONES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 15/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_B_Inm_Expropiaciones
    {
        public const String Tabla_Ope_Pat_B_Inm_Expropiaciones = "OPE_PAT_B_INM_EXPROPIACIONES";
        public const String Campo_No_Expropiacion = "NO_EXPROPIACION";
        public const String Campo_Bien_Inmueble_ID = "BIEN_INMUEBLE_ID";
        public const String Campo_Fecha_Expropiacion = "FECHA_EXPROPIACION";
        public const String Campo_Usuario_Expropiacion_ID = "USUARIO_EXPROPIACION_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_B_Inm_Afectaciones
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PAT_B_INM_AFECTACIONES
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 15/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_B_Inm_Afectaciones
    {
        public const String Tabla_Ope_Pat_B_Inm_Afectaciones = "OPE_PAT_B_INM_AFECTACIONES";
        public const String Campo_No_Registro = "NO_REGISTRO";
        public const String Campo_Bien_Inmueble_ID = "BIEN_INMUEBLE_ID";
        public const String Campo_Fecha_Afectacion = "FECHA_AFECTACION";
        public const String Campo_Fecha_Registro = "FECHA_REGISTRO";
        public const String Campo_Usuario_Registro_ID = "USUARIO_REGISTRO_ID";
        public const String Campo_Propietario = "PROPIETARIO";
        public const String Campo_Session_Ayuntamiento = "SESSION_AYUNTAMIENTO";
        public const String Campo_Tramo = "TRAMO";
        public const String Campo_No_Contrato = "NO_CONTRATO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Pat_Areas_Donacion
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PAT_AREAS_DONACION
    /// PARÁMETROS :     
    /// CREO       : Franciso Antonio Gallardo Castañeda
    /// FECHA_CREO : 15/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Pat_Areas_Donacion
    {
        public const String Tabla_Cat_Pat_Areas_Donacion = "CAT_PAT_AREAS_DONACION";
        public const String Campo_Area_ID = "AREA_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    #endregion Control Patrimonial


    #endregion


    ///*************************************************************************************************************************
    ///                                                                ALMACÉN
    ///*************************************************************************************************************************

    #region Región Almacén
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Pat_Bienes_Recibos
    /// DESCRIPCIÓN:        Clase que contiene los campos de la tabla OPE_PAT_BIENES_RECIBOS
    /// PARÁMETROS :     
    /// CREO       :        Salvador Hernandez Ramirez
    /// FECHA_CREO :        26/Julio/20101
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pat_Bienes_Recibos
    {
        public const String Tabla_Ope_Pat_Bienes_Recibos = "OPE_PAT_BIENES_RECIBOS";
        public const String Campo_Bien_Recibo_ID = "BIEN_RECIBO_ID";
        public const String Campo_Bien_ID = "BIEN_ID";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Empleado_Recibo_ID = "EMPLEADO_RECIBO_ID";
        public const String Campo_Fecha_Inicial = "FECHA_INICIAL";
        public const String Campo_Fecha_Final = "FECHA_FINAL";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Empleado_Almacen_ID = "EMPLEADO_ALMACEN_ID";

        public const String Campo_Movimiento_Alta = "MOVIMIENTO_ALTA";
        public const String Campo_Movimiento_Modificacion = "MOVIMIENTO_MODIFICACION";
        public const String Campo_Movimiento_Baja = "MOVIMIENTO_BAJA";
        public const String Campo_Estado = "ESTADO";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Observaciones = "OBSERVACIONES";
    }




    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Ope_Alm_Recibos_Transitorios
    ///DESCRIPCIÓN:        Clase que contiene los campos de la tabla OPE_ALM_RECIBOS_TRANSITORIOS_T
    ///PARAMETROS: 
    ///CREO:               Salvador Hernández Ramírez
    ///FECHA_CREO:         26-Julio-11
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Alm_Recibos_Transitorios_Totalidad
    {
        public const String Tabla_Ope_Alm_Recibos_Transitorios_Totalidad = "OPE_ALM_RECIBOS_TRANSITORIOS_T";
        public const String Campo_No_Recibo = "NO_RECIBO";
        public const String Campo_No_Contra_Recibo = "NO_CONTRA_RECIBO";
        public const String Campo_Responsable_ID = "RESPONSABLE_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Tipo = "TIPO";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Ope_Alm_Recibos_Transitorios
    ///DESCRIPCIÓN:        Clase que contiene los campos de la tabla OPE_ALM_RECIBOS_TRANSITORIOS
    ///PARAMETROS: 
    ///CREO:               Salvador Hernández Ramírez
    ///FECHA_CREO:         09-Julio-11
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Alm_Recibos_Transitorios
    {
        public const String Tabla_Ope_Alm_Recibos_Transitorios = "OPE_ALM_RECIBOS_TRANSITORIOS";
        public const String Campo_No_Recibo = "NO_RECIBO";
        public const String Campo_No_Contra_Recibo = "NO_CONTRA_RECIBO";
        public const String Campo_Responsable_ID = "RESPONSABLE_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Tipo = "TIPO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Com_Cap_Inv_Stock
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_COM_CAP_INV_STOCK
    /// PARÁMETROS :     
    /// CREO       : Salvador Hernández Ramírez
    /// FECHA_CREO : 13/Enero/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public class Ope_Com_Cap_Inv_Stock
    {
        public const String Tabla_Ope_Com_Cap_Inv_Stock = "OPE_COM_CAP_INV_STOCK";
        public const String Campo_No_Inventario = "NO_INVENTARIO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Observaciones = "OBSERVACIONES";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Com_Cap_Stock_Detalles
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_COM_CAP_STOCK_DETALLES
    /// PARÁMETROS :     
    /// CREO       : Salvador Hernández Ramírez
    /// FECHA_CREO : 13/Enero/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Com_Cap_Stock_Detalles
    {
        public const String Tabla_Ope_Com_Cap_Stock_Detalles = "OPE_COM_CAP_STOCK_DETALLES";
        public const String Campo_No_Inventario = "NO_INVENTARIO";
        public const String Campo_Producto_Id = "PRODUCTO_ID";
        public const String Campo_Contados_Sistema = "CONTADOS_SISTEMA";
        public const String Campo_Contados_Usuario = "CONTADOS_USUARIO";
        public const String Campo_Diferencia = "DIFERENCIA";
        public const String Campo_Marbete = "MARBETE";
    }


    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Ope_Com_Ajustes_Inv_Stock
    /// DESCRIPCION:           Clase que contiene los campos de la tabla OPE_COM_AJUSTES_INV_STOCK
    /// PARAMETROS :     
    /// CREO       :           Salvador Hernández Ramírez
    /// FECHA_CREO :           25/Enero/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Ope_Com_Ajustes_Inv_Stock
    {
        public const String Tabla_Ope_Com_Ajustes_Inv_Stock = "OPE_COM_AJUSTES_INV_STOCK";
        public const String Campo_No_Inventario = "NO_INVENTARIO";
        public const String Campo_Tipo_Ajuste = "TIPO_AJUSTE";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Ajusto = "USUARIO_AJUSTO";
        public const String Campo_No_Empleado = "NO_EMPLEADO";
        public const String Campo_Justificacion = "JUSTIFICACION";
        public const String Campo_Marbete = "MARBETE";
    }



    #endregion Almacén

    ///*************************************************************************************************************************
    ///                                                                ARMONIZACIN
    ///*************************************************************************************************************************

    #region ARMONIZACION
    ///*******************************************************************************************************
    /// 	NOMBRE_CLASE: Cat_SAP_Fuente_Financiamiento
    /// 	DESCRIPCIÓN: La clase contiene los campos de la tabla CAT_SAP_FTE_FINANCIAMIENTO
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 25-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public class Cat_SAP_Fuente_Financiamiento
    {
        public const String Tabla_Cat_SAP_Fuente_Financiamiento = "CAT_SAP_FTE_FINANCIAMIENTO";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Especiales_Ramo_33 = "ESPECIALES_RAMO_33";
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_CLASE: Cat_SAP_Capitulos
    /// 	DESCRIPCIÓN: La clase contiene los campos de la tabla CAT_SAP_Capitulo
    /// 	PARÁMETROS:
    /// 	CREO: Jacqueline Ramírez Sierra
    /// 	FECHA_CREO: 25-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public class Cat_SAP_Capitulos
    {
        public const String Tabla_Cat_SAP_Capitulos = "CAT_SAP_CAPITULO";
        public const String Campo_Capitulo_ID = "CAPITULO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_CLASE: Cat_SAP_Concepto
    /// 	DESCRIPCIÓN: La clase contiene los campos de la tabla CAT_SAP_Capitulo
    /// 	PARÁMETROS:
    /// 	CREO:
    /// 	FECHA_CREO: 25-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public class Cat_Sap_Concepto
    {
        public const String Tabla_Cat_SAP_Concepto = "CAT_SAP_CONCEPTO";
        public const String Campo_Concepto_ID = "CONCEPTO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Capitulo_ID = "CAPITULO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_CLASE: Cat_SAP_Partida_Generica
    /// 	DESCRIPCIÓN: La clase contiene los campos de la tabla CAT_SAP_PARTIDA_GENERICA
    /// 	PARÁMETROS:
    /// 	CREO:	Roberto González Oseguera
    /// 	FECHA_CREO: 28-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public class Cat_SAP_Partida_Generica
    {
        public const String Tabla_Cat_SAP_Partida_Generica = "CAT_SAP_PARTIDA_GENERICA";
        public const String Campo_Partida_Generica_ID = "PARTIDA_GENERICA_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Concepto_ID = "CONCEPTO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Cat_Sap_Partidas_Especificas
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_SAP_PARTIDAS_ESPECIFICAS
    ///PARAMETROS: 
    ///CREO: jesus toledo
    ///FECHA_CREO: 03/03/2011 12:36:44 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public class Cat_Sap_Partidas_Especificas
    {
        public const String Tabla_Cat_SAP_Partidas_Especificas = "CAT_SAP_PARTIDAS_ESPECIFICAS";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Giro_ID = "GIRO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Partida_Generica_ID = "PARTIDA_GENERICA_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Cuenta = "CUENTA";
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_CLASE: Cat_SAP_Area_Funcional
    /// 	DESCRIPCIÓN: La clase contiene los campos de la tabla CAT_SAP_AREA_FUNCIONAL
    /// 	PARÁMETROS:
    /// 	CREO: Leslie González Vázquez
    /// 	FECHA_CREO: 25/Feb/2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    ///
    public class Cat_SAP_Area_Funcional
    {
        public const String Tabla_Cat_SAP_Area_Funcional = "CAT_SAP_AREA_FUNCIONAL";
        public const String Campo_Area_Funcional_ID = "AREA_FUNCIONAL_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Anio = "ANIO";
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_CLASE: Cat_OPE_SAP_PRES_PROG_PROY
    /// 	DESCRIPCIÓN: La clase contiene los campos de la tabla OPE_SAP_PRES_PROG_PROY
    /// 	PARÁMETROS:
    /// 	CREO: Leslie González Vázquez
    /// 	FECHA_CREO: 01/Marzo/2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public class Ope_SAP_Pres_Prog_Proy
    {
        public const String Tabla_Ope_SAP_Pres_Prog_Proy = "OPE_SAP_PRES_PROG_PROY";
        public const String Campo_Pres_Prog_Proy_ID = "PRES_PROG_PROY_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Monto_Presupuestal = "MONTO_PRESUPUESTAL";
        public const String Campo_Monto_Disponible = "MONTO_DISPONIBLE";
        public const String Campo_Monto_Comprometido = "MONTO_COMPROMETIDO";
        public const String Campo_Anio_Presupuesto = "ANIO_PRESUPUESTO";
        public const String Campo_Monto_Ejercido = "MONTO_EJERCIDO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Sap_Det_Prog_Dependencia
    /// DESCRIPCION:           
    /// PARAMETROS :     
    /// CREO       : Susana Trigueros Armenta
    /// FECHA_CREO :       28 Feb 2011    
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cat_Sap_Det_Prog_Dependencias
    {
        public const String Tabla_Cat_Sap_Det_Prog_Dependencias = "CAT_SAP_DET_PROG_DEPENDENCIA";
        public const String Campo_Prog_Dependencia_ID = "PROG_DEPENDENCIA_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Sap_Partidas_Genericas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_SAP_PARTIDA_GENERICA
    /// PARÁMETROS :
    /// CREO       : Juan Alberto Hernández Negrete
    /// FECHA_CREO : 25/Feebrero/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Sap_Partidas_Genericas
    {
        public const String Tabla_Cat_Sap_Partidas_Genericas = "CAT_SAP_PARTIDA_GENERICA";
        public const String Campo_Partida_Generica_ID = "PARTIDA_GENERICA_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Concepto_ID = "CONCEPTO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_CLASE: Cat_SAP_Det_Fte_Dependencia
    /// 	DESCRIPCIÓN: La clase contiene los campos de la tabla CAT_SAP_DET_FTE_DEPENDENCIA
    /// 	PARÁMETROS:
    /// 	CREO:	Roberto González Oseguera
    /// 	FECHA_CREO: 04-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public class Cat_SAP_Det_Fte_Dependencia
    {
        public const String Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia = "CAT_SAP_DET_FTE_DEPENDENCIA";
        public const String Campo_Financiamiento_Dependencia_ID = "FINANCIAMIENTO_DEPENDENCIA_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_CLASE: Cat_SAP_Det_Prog_Dependencia
    /// 	DESCRIPCIÓN: La clase contiene los campos de la tabla CAT_SAP_DET_PROG_DEPENDENCIA
    /// 	PARÁMETROS:
    /// 	CREO:	Roberto González Oseguera
    /// 	FECHA_CREO: 04-mar-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public class Cat_SAP_Det_Prog_Dependencia
    {
        public const String Tabla_Cat_SAP_Det_Programa_Dependencia = "CAT_SAP_DET_PROG_DEPENDENCIA";
        public const String Campo_Programa_Dependencia_ID = "PROG_DEPENDENCIA_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_CLASE: Ope_SAP_Parametros
    /// 	DESCRIPCIÓN: La clase contiene los campos de la tabla OPE_SAP_PARAMETROS
    /// 	PARÁMETROS:
    /// 	CREO:	Roberto González Oseguera
    /// 	FECHA_CREO: 18-abr-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public class Ope_SAP_Parametros
    {
        public const String Tabla_Ope_SAP_Parametros = "OPE_SAP_PARAMETROS";
        public const String Campo_Sociedad = "SOCIEDAD";
        public const String Campo_Fondo = "FONDO";
        public const String Campo_Division = "DIVISION";
    }
    #endregion


    #region GASTOS





    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Ope_Com_Gastos
    /// DESCRIPCION:           Clase que contiene los campos de la tabla OPE_COM_GASTOS 
    /// PARAMETROS :     
    /// CREO       :           Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO :           26/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Ope_Com_Gastos
    {
        public const String Tabla_Ope_Com_Gastos = "OPE_COM_GASTOS";
        public const String Campo_Gasto_ID = "GASTO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Fecha_Solicitud = "FECHA_SOLICITUD";
        public const String Campo_Costo_Total_Gasto = "COSTO_TOTAL_GASTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Numero_Reserva = "NUMERO_RESERVA";
        public const String Campo_Fecha_Asig_Numero_Reserva = "FECHA_ASIG_NUMERO_RESERVA";
        public const String Campo_Usuario_Asig_Numero_Reserva = "USUARIO_ASIG_NUM_RESERVA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Ieps = "IEPS";
        public const String Campo_Iva = "IVA";
        public const String Campo_Justificacion = "JUSTIFICACION";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_No_Factura = "NO_FACTURA";
        public const String Campo_Fecha_Factura = "FECHA_FACTURA";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Ope_Com_Gastos_Detalles
    /// DESCRIPCION:           Clase que contiene los campos de la tabla OPE_COM_GASTOS_DETALLES 
    /// PARAMETROS :     
    /// CREO       :           Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO :           26/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Ope_Com_Gastos_Detalles
    {
        public const String Tabla_Ope_Com_Gastos_Detalles = "OPE_COM_GASTOS_DETALLES";
        public const String Campo_Gasto_Detalle_ID = "GASTO_DETALLE_ID";
        public const String Campo_Gasto_ID = "GASTO_ID";
        //public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Costo = "COSTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        //public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Producto_Servicio = "PRODUCTO_SERVICIO";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Ieps = "IEPS";
        public const String Campo_Iva = "IVA";
        public const String Campo_Monto_Total = "MONTO_TOTAL";
        public const String Campo_Identificador = "IDENTIFICADOR";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Ope_Com_Gastos_Comentarios
    /// DESCRIPCION:           Clase que contiene los campos de la tabla 
    ///                        OPE_COM_GASTOS_DETALLES 
    /// PARAMETROS :     
    /// CREO       :           Francisco Antonio Gallardo Castañeda.
    /// FECHA_CREO :           26/Marzo/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Ope_Com_Gastos_Comentarios
    {
        public const String Tabla_Ope_Com_Gastos_Comentarios = "OPE_COM_GASTOS_COMENTARIOS";
        public const String Campo_Gasto_Comentario_ID = "GASTO_COMENTARIO_ID";
        public const String Campo_Gasto_ID = "GASTO_ID";
        public const String Campo_Fecha_Comentario = "FECHA_COMENTARIO";
        public const String Campo_Usuario_Comentario = "USUARIO_COMENTO";
        public const String Campo_Comentario = "COMENTARIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    #endregion

    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Ope_Pre_Empleados_Activos
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PRE_EMPLEADOS_ACTIVOS
    ///PARAMETROS: 
    ///CREO: jesus toledo
    ///FECHA_CREO: 03/31/2011 01:37:25 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:  
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Pre_Empleados_Activos
    {
        public const String Tabla_Ope_Pre_Empleados_Activos = "OPE_PRE_EMPLEADOS_ACTIVOS";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Consecutivo = "CONSECUTIVO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:   Ope_Com_Sincronizaciones_Presupuesto
    /// DESCRIPCION:           Clase que contiene los campos de la tabla OPE_COM_SINCRONIZACIONES_PRE
    /// PARAMETROS :     
    /// CREO       :           Roberto González OSeguera
    /// FECHA_CREO :           13-abr-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Ope_Com_Sincronizaciones_Presupuesto
    {
        public const String Tabla_Ope_Com_Sincronizaciones_Presupuesto = "OPE_COM_SINCRONIZACIONES_PRE";
        public const String Campo_No_Sincronizacion = "NO_SINCRONIZACION";
        public const String Campo_Nombre_Archivo = "NOMBRE_ARCHIVO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*************************************************************************************************************************
    ///                                                                CONTABILIDAD
    ///*************************************************************************************************************************
    ///

    #region Contabilidad
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Con_Cierre_Anual
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_CON_CIERRE_ANUAL
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 06/Junio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Con_Cierre_Anual
    {
        public const String Tabla_Ope_Con_Cierre_Anual = "OPE_CON_CIERRE_ANUAL";
        public const String Campo_No_Cierre_Anual = "NO_CIERRE_ANUAL";
        public const String Campo_Cuenta_Contable_ID_Inicio = "CUENTA_CONTABLE_ID_INICIO";
        public const String Campo_Cuenta_Contable_ID_Fin = "CUENTA_CONTABLE_ID_FIN";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Total_Debe = "TOTAL_DEBE";
        public const String Campo_Total_Haber = "TOTAL_HABER";
        public const String Campo_Diferencia = "DIFERENCIA";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Con_Tipo_Polizas
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_CON_TIPO_POLIZAS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 06/Junio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Con_Tipo_Polizas
    {
        public const String Tabla_Cat_Con_Tipo_Polizas = "CAT_CON_TIPO_POLIZAS";
        public const String Campo_Tipo_Poliza_ID = "TIPO_POLIZA_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Abreviatura = "ABREVIATURA";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Con_Tipo_Balance
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_CON_TIPO_BALANCE
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 06/Junio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Con_Tipo_Balance
    {
        public const String Tabla_Cat_Con_Tipo_Balance = "CAT_CON_TIPO_BALANCE";
        public const String Campo_Tipo_Balance_ID = "TIPO_BALANCE_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Tipo_Balance = "TIPO_BALANCE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Con_Niveles
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_CON_NIVELES
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 06/Junio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Con_Niveles
    {
        public const String Tabla_Cat_Con_Niveles = "CAT_CON_NIVELES";
        public const String Campo_Nivel_ID = "NIVEL_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Inicio_Nivel = "INICIO_NIVEL";
        public const String Campo_Final_Nivel = "FINAL_NIVEL";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Con_Bitacora_Polizas
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_CON_BITACORA_POLIZAS
    /// PARAMETROS :
    /// CREO       : Salvador L. Rea Ayala
    /// FECHA_CREO : 24/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Con_Bitacora_Polizas
    {
        public const String Tabla_Ope_Con_Bitacora_Polizas = "OPE_CON_BITACORA_POLIZAS";
        public const String Campo_No_Bitacora = "NO_BITACORA";
        public const String Campo_No_Poliza = "NO_POLIZA";
        public const String Campo_Tipo_Poliza_ID = "TIPO_POLIZA_ID";
        public const String Campo_Mes_Ano = "MES_ANO";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Debe = "DEBE";
        public const String Campo_Haber = "HABER";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Con_Tipo_Resultado
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_CON_TIPO_RESULTADO
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 06/Junio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Con_Tipo_Resultado
    {
        public const String Tabla_Cat_Con_Tipo_Resultado = "CAT_CON_TIPO_RESULTADO";
        public const String Campo_Tipo_Resultado_ID = "TIPO_RESULTADO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Con_Cuentas_Contables
    /// DESCRIPCION: Clase con contiene los datos de la tabla CAT_CON_CUENTAS_CONTABLES
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 06/Junio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Con_Cuentas_Contables
    {
        public const String Tabla_Cat_Con_Cuentas_Contables = "CAT_CON_CUENTAS_CONTABLES";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Tipo_Balance_ID = "TIPO_BALANCE_ID";
        public const String Campo_Nivel_ID = "NIVEL_ID";
        public const String Campo_Tipo_Resultado_ID = "TIPO_RESULTADO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Cuenta = "CUENTA";
        public const String Campo_Afectable = "AFECTABLE";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Tipo_Cuenta = "TIPO_CUENTA";
        public const String Campo_Cargo_Abono_ID = "CARGO_ABONO_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Con_Polizas
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_CON_POLIZAS
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 10/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Con_Polizas
    {
        public const String Tabla_Ope_Con_Polizas = "OPE_CON_POLIZAS";
        public const String Campo_No_Poliza = "NO_POLIZA";
        public const String Campo_Tipo_Poliza_ID = "TIPO_POLIZA_ID";
        public const String Campo_Mes_Ano = "MES_ANO";
        public const String Campo_Fecha_Poliza = "FECHA_POLIZA";
        public const String Campo_Concepto = "CONCEPTO";
        public const String Campo_Total_Debe = "TOTAL_DEBE";
        public const String Campo_Total_Haber = "TOTAL_HABER";
        public const String Campo_No_Partidas = "NO_PARTIDAS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Empleado_ID_Creo = "EMPLEADO_ID_CREO";
        public const String Campo_Empleado_ID_Autorizo = "EMPLEADO_ID_AUTORIZO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Con_Polizas_Detalles
    /// DESCRIPCION: Clase con contiene los datos de la tabla OPE_CON_POLIZAS_DETALLES
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 10/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Con_Polizas_Detalles
    {
        public const String Tabla_Ope_Con_Polizas_Detalles = "OPE_CON_POLIZAS_DETALLES";
        public const String Campo_No_Poliza = "NO_POLIZA";
        public const String Campo_Tipo_Poliza_ID = "TIPO_POLIZA_ID";
        public const String Campo_Mes_Ano = "MES_ANO";
        public const String Campo_Partida = "PARTIDA";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Concepto = "CONCEPTO";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        public const String Campo_Area_Funcional_ID = "AREA_FUNCIONAL_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Debe = "DEBE";
        public const String Campo_Haber = "HABER";
        public const String Campo_Saldo = "SALDO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Consecutivo = "CONSECUTIVO";
        public const String Campo_Compromiso_ID = "COMPROMISO_ID";
        public const String Campo_Referencia = "REFERENCIA";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Con_Polizas_Detalles
    /// DESCRIPCION: Clase con contiene los datos de la tabla TEM_CON_POLIZAS_DETALLES
    /// PARAMETROS :
    /// CREO       : Yazmin A. Delgado Gómez
    /// FECHA_CREO : 10/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Tem_Con_Polizas_Detalles
    {
        public const String Tabla_Tem_Con_Polizas_Detalles = "TEM_CON_POLIZAS_DETALLES";
        public const String Campo_Tipo_Poliza_ID = "TIPO_POLIZA_ID";
        public const String Campo_Mes_Ano = "MES_ANO";
        public const String Campo_Partida = "PARTIDA";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Concepto = "CONCEPTO";
        public const String Campo_Debe = "DEBE";
        public const String Campo_Haber = "HABER";
        public const String Campo_Saldo = "SALDO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Consecutivo = "CONSECUTIVO";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Con_Parametros
    /// DESCRIPCION: Clase que contiene los datos de la tabla CAT_CON_PARAMETROS
    /// PARAMETROS :
    /// CREO       : Salvador L. Rea Ayala
    /// FECHA_CREO : 15/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Con_Parametros
    {
        public const String Tabla_Cat_Con_Parametros = "CAT_CON_PARAMETROS";
        public const String Campo_Parametro_Contabilidad_ID = "PARAMETRO_CONTABILIDAD_ID";
        public const String Campo_Mascara_Cuenta_Contable = "MASCARA_CUENTA_CONTABLE";
        public const String Campo_Mes_Contable = "MES_CONTABLE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Con_Compromisos
    /// DESCRIPCION: Clase que contiene los datos de la tabla Ope_Con_Compromisos
    /// PARAMETROS :
    /// CREO       : Salvador L. Rea Ayala
    /// FECHA_CREO : 13/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Con_Compromisos
    {
        public const String Tabla_Ope_Con_Compromisos = "OPE_CON_COMPROMISOS";
        public const String Campo_No_Compromiso = "NO_COMPROMISO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Monto_Comprometido = "MONTO_COMPROMETIDO";
        public const String Campo_Concepto = "CONCEPTO";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Contratista_ID = "CONTRATISTA_ID";
        public const String Campo_Nombre_Beneficiario = "NOMBRE_BENEFICIARIO";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        public const String Campo_Area_Funcional_ID = "AREA_FUNCIONAL_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    /// NOMBRE DE LA FUNCION: Ope_Con_Cierre_Mensual
    /// DESCRIPCION: Clase que contiene los datos de la tabla OPE_CON_CIERRE_MENSUAL
    /// PARAMETROS :
    /// CREO       : Salvador L. Rea Ayala
    /// FECHA_CREO : 1/Octubre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Con_Cierre_Mensual
    {
        public const String Tabla_Ope_Con_Cierre_Mensual = "OPE_CON_CIERRE_MENSUAL";
        public const String Campo_Cierre_Mensual_ID = "CIERRE_MENSUAL_ID";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Mes_Anio = "MES_ANIO";
        public const String Campo_Fecha_Inicio = "FECHA_INICIO";
        public const String Campo_Fecha_Final = "FECHA_FINAL";
        public const String Campo_Saldo_Inicial = "SALDO_INICIAL";
        public const String Campo_Saldo_Final = "SALDO_FINAL";
        public const String Campo_Total_Debe = "TOTAL_DEBE";
        public const String Campo_Total_Haber = "TOTAL_HABER";
        public const String Campo_Diferencia = "DIFERENCIA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    /// NOMBRE DE LA FUNCION: Ope_Con_Cierre_Mensual_Gral
    /// DESCRIPCION: Clase que contiene los datos de la tabla OPE_CON_CIERRE_MENSUAL_GRAL
    /// PARAMETROS :
    /// CREO       : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO : 04/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Con_Cierre_Mensual_Gral
    {
        public const String Tabla_Ope_Con_Cierre_Mensual_Gral = "OPE_CON_CIERRE_MENSUAL_GRAL";
        public const String Campo_Cierre_Mensual_Gral_ID = "CIERRE_MENSUAL_GRAL_ID";
        public const String Campo_Mes = "MES";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Saldo = "SALDO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Con_Solicitud_Pagos
    /// DESCRIPCION: Clase que contiene los datos de la tabla OPE_CON_SOLICITUD_PAGOS
    /// PARAMETROS :
    /// CREO       : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO : 15/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Con_Solicitud_Pagos
    {
        public const String Tabla_Ope_Con_Solicitud_Pagos = "OPE_CON_SOLICITUD_PAGOS";
        public const String Campo_No_Solicitud_Pago = "NO_SOLICITUD_PAGO";
        public const String Campo_No_Reserva = "NO_RESERVA";
        public const String Campo_No_Poliza = "NO_POLIZA";
        public const String Campo_Tipo_Poliza_ID = "TIPO_POLIZA_ID";
        public const String Campo_Mes_Ano = "MES_ANO";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Tipo_Solicitud_Pago_ID = "TIPO_SOLICITUD_PAGO_ID";
        public const String Campo_Empleado_ID_Jefe_Area = "EMPLEADO_ID_JEFE_AREA";
        public const String Campo_Empleado_ID_Contabilidad = "EMPLEADO_ID_CONTABILIDAD";
        public const String Campo_Concepto = "CONCEPTO";
        public const String Campo_Fecha_Solicitud = "FECHA_SOLICITUD";
        public const String Campo_Monto = "MONTO";
        public const String Campo_No_Factura = "NO_FACTURA";
        public const String Campo_Fecha_Factura = "FECHA_FACTURA";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Autorizo_Rechazo_Jefe = "FECHA_AUTORIZO_RECHAZO_JEFE";
        public const String Campo_Comentarios_Jefe_Area = "COMENTARIOS_JEFE_AREA";
        public const String Campo_Fecha_Autorizo_Rechazo_Contabilidad = "FECHA_AUTORIZO_RECHAZO_CONTABI";
        public const String Campo_Comentarios_Contabilidad = "COMENTARIOS_CONTABILIDAD";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Documentos = "DOCUMENTOS";
        public const String Campo_Fecha_Recepcion_Documentos = "FECHA_RECEPCION_DOCUMENTOS";
        public const String Campo_Usuario_Recibio = "USUARIO_RECIBIO";
        public const String Campo_Comentarios_Recepcion_Doc = "COMENTARIOS_RECEPCION_DOC";
        public const String Campo_Fecha_Autoriza_Rechaza_Ejercido = "FECHA_AUTORIZA_RECHAZA_EJD";
        public const String Campo_Comentario_Ejercido = "COMENTARIO_EJERCIDO";
        public const String Campo_Monto_Aprobado = "MONTO_COMPROBADO";
        public const String Campo_Fecha_Comprobado = "FECHA_COMPROBADO";
        public const String Campo_Usuario_Comprobo = "USUARIO_COMPROBO";
        public const String Campo_Transferencia = "TRANSFERENCIA";
        public const String Campo_Cuenta_Transferencia_ID = "CUENTA_TRANSFERENCIA_ID";
        public const String Campo_Usuario_Autorizo_Area = "USUARIO_AUTORIZO_AREA";
        public const String Campo_Usuario_Recibio_Doc_Fisica = "USUARIO_RECIBIO_DOC_FISICA";
        public const String Campo_Usuario_Autorizo_Documentos = "USUARIO_AUTORIZO_DOCUMENTOS";
        public const String Campo_Usuario_Recibio_Contabilidad = "USUARIO_RECIBIO_CONTABILIDAD";
        public const String Campo_Usuario_Autorizo_Contabilidad = "USUARIO_AUTORIZO_CONTABILIDAD";
        public const String Campo_Usuario_Recibio_Ejercido = "USUARIO_RECIBIO_EJERCIDO";
        public const String Campo_Usuario_Autorizo_Ejercido = "USUARIO_AUTORIZO_EJERCIDO";
        public const String Campo_Usuario_Recibio_Pagado = "USUARIO_RECIBIO_PAGADO";
        public const String Campo_Fecha_Recibio_Doc_Fisica = "FECHA_RECIBIO_DOC_FISICA";
        public const String Campo_Fecha_Recibio_Contabilidad = "FECHA_RECIBIO_CONTABILIDAD";
        public const String Campo_Fecha_Recibio_Ejercido = "FECHA_RECIBIO_EJERCIDO";
        public const String Campo_Fecha_Recibio_Pagado = "FECHA_RECIBIO_PAGADO";
        public const String Campo_Fecha_Layout_Transferencia = "FECHA_LAYOUT_TRANS";
        public const String Campo_Forma_Pago = "FORMA_PAGO";
        public const String Campo_Cuenta_Banco_Pago_ID = "CUENTA_BANCO_PAGO_ID";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_Comentario_Finanzas = "COMENTARIOS_FINANZAS";
        public const String Campo_Factoraje_Proveedor_id = "FACTORAJE_PROVEEDOR_ID";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cat_Con_Tipo_Solicitud_Pagos
    /// DESCRIPCION: Clase que contiene los datos de la tabla CAT_CON_TIPO_SOLICITUD_PAGO
    /// PARAMETROS :
    /// CREO       : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO : 15/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Con_Tipo_Solicitud_Pagos
    {
        public const String Tabla_Cat_Con_Tipo_Solicitud_Pago = "CAT_CON_TIPO_SOLICITUD_PAGOS";
        public const String Campo_Tipo_Solicitud_Pago_ID = "TIPO_SOLICITUD_PAGO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Con_Pagos
    /// DESCRIPCION: Clase que contiene los datos de la tabla OPE_CON_PAGOS
    /// PARAMETROS :
    /// CREO       : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO : 21/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Con_Pagos
    {
        public const String Tabla_Ope_Con_Pagos = "OPE_CON_PAGOS";
        public const String Campo_No_Pago = "NO_PAGO";
        public const String Campo_No_Solicitud_Pago = "NO_SOLICITUD_PAGO";
        public const String Campo_No_poliza = "NO_POLIZA";
        public const String Campo_Tipo_Poliza_ID = "TIPO_POLIZA_ID";
        public const String Campo_Mes_Ano = "MES_ANO";
        public const String Campo_Fecha_Pago = "FECHA_PAGO";
        public const String Campo_Banco_ID = "BANCO_ID";
        public const String Campo_Beneficiario_Pago = "BENEFICIARIO_PAGO";
        public const String Campo_Forma_Pago = "FORMA_PAGO";
        public const String Campo_Referencia_Transferencia_Banca = "REFERENCIA_TRANSFERENCIA_BANCA";
        public const String Campo_No_Cheque = "NO_CHEQUE";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Motivo_Cancelacion = "MOTIVO_CANCELACION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    #endregion

    ///*************************************************************************************************************************
    ///                                                                VARIOS
    ///*************************************************************************************************************************
    #region VARIOS
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Cuentas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CUENTAS
    /// PARÁMETROS :     
    /// CREO       :
    /// FECHA_CREO : 20/Agosto/2010 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Cuentas
    {
        public const String Tabla_Cat_Cuentas = "CAT_CUENTAS";
        public const String Campo_Cuenta_ID = "CUENTA_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_No_Cuenta = "NUMERO_CUENTA";
        public const String Campo_Banco = "BANCO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_UsuarioCreo = "USUARIO_CREO";
        public const String Campo_FechaCreo = "FECHA_CREO";
        public const String Campo_UsuarioModifico = "USUARIO_MODIFICO";
        public const String Campo_FechaModifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Cat_Con_Cuentas_Fijas
    /// DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_CON_CUENTAS_FIJAS
    /// PARÁMETROS :     
    /// CREO       : Gustavo Angeles
    /// FECHA_CREO : 11/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Con_Cuentas_Fijas
    {
        public const String Tabla_Cat_Con_Cuentas_Fijas = "CAT_CON_CUENTAS_FIJAS";
        public const String Campo_Almacen_General = "ALMACEN_GENERAL";
        public const String Campo_Iva_Acreditable_Compras = "IVA_ACREDITABLE_COMPRAS";
    }
    public class Apl_Cat_Preg_Resp
    {
        public const String Tabla_Apl_Cat_Preg_Resp = "APL_CAT_PREG_RESP";
        public const String Campo_Preg_Resp_ID = "PREG_RESP_ID";
        public const String Campo_Pregunta = "PREGUNTA";
        public const String Campo_Respuesta = "RESPUESTA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cat_Det_Empleado_UR
    /// DESCRIPCION:           Clase que contiene los campos de la tabla CAT_DET_EMPLEADO_UR 
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armeta
    /// FECHA_CREO :           01/Sep/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************   
    public class Cat_Det_Empleado_UR
    {
        public const String Tabla_Cat_Det_Empleado_UR = "CAT_DET_EMPLEADO_UR";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
    }

    ///*******************************************************************************
    ///NOMBRE: CAT_PARAMETROS_LOGIN
    ///DESCRIPCIÓN: Clase que contiene los datos de la tabla CAT_PARAMETROS_LOGIN
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ: 15/Diciembre/2011
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///*******************************************************************************
    public class Cat_Parametros_login
    {
        public const String Tabla_Cat_Parametros_login = "CAT_PARAMETROS_LOGIN";
        public const String Campo_Intentos_Fallidos = "INTENTOS_FALLIDOS";
        public const String Campo_Dias_Activos = "DIAS_ACTIVOS";
        public const String Campo_Longitud_Password = "LONGITUD_PASSWORD";
        public const String Campo_Cantidad_Letras = "CANTIDAD_LETRAS";
        public const String Campo_Cantidad_Numeros = "CANTIDAD_NUMEROS";
        public const String Campo_Cantidad_Caracter_Especial = "CANTIDAD_CARACTER_ESPECIAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    #endregion

    #region INGRESOS

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE: Cat_Pre_Claves_Ingreso
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PRE_CLAVES_INGRESO
    ///PARÁMETROS :     
    ///CREO       : José Alfredo García Pichardo
    ///FECHA_CREO : 21/Julio/2011 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    //public class Cat_Pre_Claves_Ingreso
    //{
    //    public const String Tabla_Cat_Pre_Claves_Igreso = "CAT_PRE_CLAVES_INGRESO";
    //    public const String Campo_Clave_Ingreso_ID = "CLAVE_INGRESO_ID";
    //    public const String Campo_Grupo_ID = "GRUPO_ID";
    //    public const String Campo_Estatus = "ESTATUS";
    //    public const String Campo_Clave = "CLAVE";
    //    public const String Campo_Descripcion = "DESCRIPCION";
    //    public const String Campo_Fundamento = "FUNDAMENTO";
    //    public const String Campo_Usuario_Creo = "USUARIO_CREO";
    //    public const String Campo_Fecha_Creo = "FECHA_CREO";
    //    public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
    //    public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE   : Cat_Ing_Tipos_Pagos
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla CAT_ING_TIPOS_PAGOS
    ///PARÁMETROS:
    ///CREO                 : Antonio Salvador Benavides Guaradado
    ///FECHA_CREO           : 27/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Ing_Tipos_Pagos
    {
        public const String Tabla_Cat_Ing_Tipos_Pagos = "CAT_ING_TIPOS_PAGOS";
        public const String Campo_Tipo_Pago_ID = "TIPO_PAGO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE   : Cat_Ing_Conceptos_Descuentos_Pronto_Pago
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla CAT_ING_TIPOS_PAGOS
    ///PARÁMETROS:
    ///CREO                 : Antonio Salvador Benavides Guaradado
    ///FECHA_CREO           : 27/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Ing_Conceptos_Descuentos_Pronto_Pago
    {
        public const String Tabla_Cat_Ing_Conceptos_Descuentos_Pronto_Pago = "CAT_ING_CONCEP_DESC_PRONTO_PAG";
        public const String Campo_Concepto_Descuento_Pronto_Pago_ID = "CONCEP_DESCUENT_PRONTO_PAGO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE   : Cat_Ing_Procesos
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla CAT_ING_PROCESOS
    ///PARÁMETROS:
    ///CREO                 : Antonio Salvador Benavides Guaradado
    ///FECHA_CREO           : 30/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Ing_Procesos
    {
        public const String Tabla_Cat_Ing_Procesos = "CAT_ING_PROCESOS";
        public const String Campo_Proceso_ID = "PROCESO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE   : Cat_Ing_Garantias_Procesos
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla CAT_ING_GARANTIAS_PROCESOS
    ///PARÁMETROS:
    ///CREO                 : Antonio Salvador Benavides Guaradado
    ///FECHA_CREO           : 13/Julio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Ing_Garantias_Procesos
    {
        public const String Tabla_Cat_Ing_Garantias_Procesos = "CAT_ING_GARANTIAS_PROCESOS";
        public const String Campo_Garantia_Proceso_ID = "GARANTIA_PROCESO_ID";
        public const String Campo_Proceso_ID = "PROCESO_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE   : Cat_Ing_Descuentos_Pronto_Pago
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla CAT_ING_TIPOS_PAGOS
    ///PARÁMETROS:
    ///CREO                 : Antonio Salvador Benavides Guaradado
    ///FECHA_CREO           : 27/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Ing_Descuentos_Pronto_Pago
    {
        public const String Tabla_Cat_Ing_Descuentos_Pronto_Pago = "CAT_ING_DESC_PRONTO_PAGO";
        public const String Campo_Descuento_Pronto_Pago_ID = "DESCUENTO_PRONTO_PAGO_ID";
        public const String Campo_Proceso_ID = "PROCESO_ID";
        public const String Campo_Dias_Descuento = "DIAS_DESCUENTO";
        public const String Campo_Porcentaje = "PORCENTAJE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE   : Cat_Ing_Descuentos_Pronto_Pago_Mes
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla CAT_ING_DESC_PRONTO_PAGO_MES
    ///PARÁMETROS:
    ///CREO                 : Antonio Salvador Benavides Guaradado
    ///FECHA_CREO           : 13/Julio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Ing_Descuentos_Pronto_Pago_Mes
    {
        public const String Tabla_Cat_Ing_Descuentos_Pronto_Pago_Mes = "CAT_ING_DESC_PRONTO_PAGO_MES";
        public const String Campo_Descuento_Pronto_Pago_Mes_ID = "DESCUENTO_PRONTO_PAGO_MES_ID";
        public const String Campo_Proceso_ID = "PROCESO_ID";
        public const String Campo_Año = "AÑO";
        public const String Campo_Enero = "ENERO";
        public const String Campo_Febrero = "FEBRERO";
        public const String Campo_Marzo = "MARZO";
        public const String Campo_Abril = "ABRIL";
        public const String Campo_Mayo = "MAYO";
        public const String Campo_Junio = "JUNIO";
        public const String Campo_Julio = "JULIO";
        public const String Campo_Agosto = "AGOSTO";
        public const String Campo_Septiembre = "SEPTIEMBRE";
        public const String Campo_Octubre = "OCTUBRE";
        public const String Campo_Noviembre = "NOVIEMBRE";
        public const String Campo_Diciembre = "DICIEMBRE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE   : Cat_Ing_Descuentos_Responsable
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla CAT_ING_TIPOS_PAGOS
    ///PARÁMETROS:
    ///CREO                 : Antonio Salvador Benavides Guaradado
    ///FECHA_CREO           : 27/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Ing_Descuentos_Responsable
    {
        public const String Tabla_Cat_Ing_Descuentos_Responsable = "CAT_ING_DESCUENTOS_RESPONSABLE";
        public const String Campo_Descuento_Responsable_ID = "DESCUENTO_RESPONSABLE_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Porcentaje = "PORCENTAJE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE   : Cat_Ing_Claves_Desacarga
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla CAT_ING_CLAVES_DESCARGA
    ///PARÁMETROS:
    ///CREO                 : Antonio Salvador Benavides Guaradado
    ///FECHA_CREO           : 27/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Ing_Claves_Desacarga
    {
        public const String Tabla_Cat_Ing_Claves_Descarga = "CAT_ING_CLAVES_DESCARGA";
        public const String Campo_Clave_Descarga_ID = "CLAVE_DESCARGA_ID";
        public const String Campo_SubConcepto_Clave_Ing_ID = "SUBCONCEPTO_CLAVE_ING_ID";
        public const String Campo_SubConcepto_Clave_Des_ID = "SUBCONCEPTO_CLAVE_DES_ID";
        public const String Campo_Tipo = "TIPO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE   : Cat_Ing_Contribuyentes
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla CAT_ING_CLAVES_DESCARGA
    ///PARÁMETROS:
    ///CREO                 : Antonio Salvador Benavides Guaradado
    ///FECHA_CREO           : 27/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Cat_Ing_Contribuyentes
    {
        public const String Tabla_Cat_Ing_Contribuyentes = "CAT_ING_CONTRIBUYENTES";
        public const String Campo_Contribuyente_ID = "CONTRIBUYENTE_ID";
        public const String Campo_Apellido_Paterno = "APELLIDO_PATERNO";
        public const String Campo_Apellido_Materno = "APELLIDO_MATERNO";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Razon_Social = "RAZON_SOCIAL";
        public const String Campo_Fecha_Nacimiento = "FECHA_NACIMIENTO";
        public const String Campo_RFC = "RFC";
        public const String Campo_CURP = "CURP";
        public const String Campo_IFE = "IFE";
        public const String Campo_Sexo = "SEXO";
        public const String Campo_EMail = "EMAIL";
        public const String Campo_Telefono = "TELEFONO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Estado_Civil = "ESTADO_CIVIL";
        public const String Campo_Tipo_Persona = "TIPO_PERSONA";
        public const String Campo_Representate_Legal = "REPRESENTANTE_LEGAL";
        public const String Campo_Tipo_Contribuyente = "TIPO_CONTRIBUYENTE";
        public const String Campo_Calle_Fiscal = "CALLE_FISCAL";
        public const String Campo_Colonia_Fiscal = "COLONIA_FISCAL";
        public const String Campo_No_Interior_Fiscal = "NO_INTERIOR_FISCAL";
        public const String Campo_No_Exterior_Fiscal = "NO_EXTERIOR_FISCAL";
        public const String Campo_Estado_Fiscal = "ESTADO_FISCAL";
        public const String Campo_Ciudad_Fiscal = "CIUDAD_FISCAL";
        public const String Campo_Entre_Calles_Fiscal = "ENTRE_CALLES_FISCAL";
        public const String Campo_Calle_Ubicacion = "CALLE_UBICACION";
        public const String Campo_Colonia_Ubicacion = "COLONIA_UBICACION";
        public const String Campo_No_Interior_Ubicacion = "NO_INTERIOR_UBICACION";
        public const String Campo_No_Exterior_Ubicacion = "NO_EXTERIOR_UBICACION";
        public const String Campo_Estado_Ubicacion = "ESTADO_UBICACION";
        public const String Campo_Ciudad_Ubicacion = "CIUDAD_UBICACION";
        public const String Campo_Entre_Calles_Ubicacion = "ENTRE_CALLES_UBICACION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE   : Ope_Ing_Descuentos
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla OPE_ING_DESCUENTOS
    ///PARÁMETROS:
    ///CREO                 : Antonio Salvador Benavides Guaradado
    ///FECHA_CREO           : 08/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Ope_Ing_Descuentos
    {
        public const String Tabla_Ope_Ing_Descuentos = "OPE_ING_DESCUENTOS";
        public const String Campo_No_Descuento = "NO_DESCUENTO";
        public const String Campo_Concepto_Orden_Pago_ID = "CONCEPTO_ORDEN_PAGO_ID";
        public const String Campo_Referencia = "REFERENCIA";
        public const String Campo_Unidades = "UNIDADES";
        public const String Campo_Monto_Honorarios = "MONTO_HONORARIOS";
        public const String Campo_Monto_Multas = "MONTO_MULTAS";
        public const String Campo_Monto_Moratorios = "MONTO_MORATORIOS";
        public const String Campo_Monto_Recargos = "MONTO_RECARGOS";
        public const String Campo_Descuento_Honorarios = "DESCUENTO_HONORARIOS";
        public const String Campo_Descuento_Multas = "DESCUENTO_MULTAS";
        public const String Campo_Descuento_Moratorios = "DESCUENTO_MORATORIOS";
        public const String Campo_Descuento_Recargos = "DESCUENTO_RECARGOS";
        public const String Campo_Total_Pagar = "TOTAL_PAGAR";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Descuento = "FECHA_DESCUENTO";
        public const String Campo_Fecha_Vencimiento = "FECHA_VENCIMIENTO";
        public const String Campo_Fundamento_Legal = "FUNDAMENTO_LEGAL";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Realizo = "REALIZO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE   : Ope_Ing_Ordenes_Pago
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla OPE_ING_ORDENES_PAGO
    ///PARÁMETROS:
    ///CREO                 : Antonio Salvador Benavides Guaradado
    ///FECHA_CREO           : 07/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Ope_Ing_Ordenes_Pago
    {
        public const String Tabla_Ope_Ing_Ordenes_Pago = "OPE_ING_ORDENES_PAGO";
        public const String Campo_No_Orden_Pago = "NO_ORDEN_PAGO";
        public const String Campo_Año = "AÑO";
        public const String Campo_Contribuyente_ID = "CONTRIBUYENTE_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Total = "TOTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Proteccion = "PROTECCION";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA CLASE   : Ope_Ing_Conceptos_Ordenes_Pago
    ///DESCRIPCIÓN          : Clase que contiene los campos de la tabla OPE_ING_CONCEPTOS_ORDENES_PAGO
    ///PARÁMETROS:
    ///CREO                 : Antonio Salvador Benavides Guaradado
    ///FECHA_CREO           : 06/Agosto/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*********************************************************************************/
    public class Ope_Ing_Conceptos_Ordenes_Pago
    {
        public const String Tabla_Ope_Ing_Conceptos_Ordenes_Pago = "OPE_ING_CONCEPTOS_ORDENES_PAGO";
        public const String Campo_Concepto_Orden_Pago_ID = "CONCEPTO_ORDEN_PAGO_ID";
        public const String Campo_No_Orden_Pago = "NO_ORDEN_PAGO";
        public const String Campo_Año = "AÑO";
        public const String Campo_SubConcepto_Ing_ID = "SUBCONCEPTO_ING_ID";
        public const String Campo_Tipo_Pago_ID = "TIPO_PAGO_ID";
        public const String Campo_Garantia_Proceso_ID = "GARANTIA_PROCESO_ID";
        public const String Campo_Clave_Padron = "CLAVE_PADRON";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fecha_Multa = "FECHA_MULTA";
        public const String Campo_Unidades = "UNIDADES";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Descuento_Importe = "DESCUENTO_IMPORTE";
        public const String Campo_Monto_Importe = "MONTO_IMPORTE";
        public const String Campo_Honorarios = "HONORARIOS";
        public const String Campo_Multas = "MULTAS";
        public const String Campo_Moratorios = "MORATORIOS";
        public const String Campo_Recargos = "RECARGOS";
        public const String Campo_Ajuste_Tarifario = "AJUSTE_TARIFARIO";
        public const String Campo_Total = "TOTAL";
        public const String Campo_Referencia = "REFERENCIA";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Folio = "FOLIO";
        public const String Campo_Banco_ID = "BANCO_ID";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        public const String Campo_Numero_Garantia = "NUMERO_GARANTIA";
    }
    #endregion

    #region PRESUPUESTOS
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : OPE_PSP_HIST_CALENDAR_PRESU
    ///DESCRIPCIÓN          : Clase con contiene los datos de la tabla OPE_PSP_HIST_CALENDAR_PRESU
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 23/Noviembre/2011
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Psp_Hist_Calendar_Presu
    {
        public const String Tabla_Ope_Psp_Hist_Calendar_Presu = "OPE_PSP_HIST_CALENDAR_PRESU";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Comentario = "COMENTARIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///****************************************************************************************************************************************************************
    ///NOMBRE: Ope_Psp_Comentarios_Mov
    ///
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla OPE_PSP_COMENTARIO_MOV
    ///
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ: 17/Noviembre/2011
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Ope_Psp_Comentarios_Mov
    {
        public const String Tabla_Ope_Psp_Comentarios_Mov = "OPE_PSP_COMENTARIOS_MOV";
        public const String Campo_Numero_Solicitud = "NO_SOLICITUD";
        public const String Campo_Comentario = "COMENTARIO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///****************************************************************************************************************************************************************
    ///NOMBRE: Ope_Psp_Cierre_Presup
    ///
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla OPE_PSP_CIERRE_PRESUP
    ///
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ: 17/Noviembre/2011
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Ope_Psp_Cierre_Presup
    {
        public const String Tabla_Ope_Psp_Cierre_Presup = "OPE_PSP_CIERRE_PRESUP";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Enero = "ENERO";
        public const String Campo_Febrero = "FEBRERO";
        public const String Campo_Marzo = "MARZO";
        public const String Campo_Abril = "ABRIL";
        public const String Campo_Mayo = "MAYO";
        public const String Campo_Junio = "JUNIO";
        public const String Campo_Julio = "JULIO";
        public const String Campo_Agosto = "AGOSTO";
        public const String Campo_Septiembre = "SEPTIEMBRE";
        public const String Campo_Octubre = "OCTUBRE";
        public const String Campo_Noviembre = "NOVIEMBRE";
        public const String Campo_Diciembre = "DICIEMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : OPE_PSP_CALENDARIZACION_PRESU
    ///DESCRIPCIÓN          : Clase con contiene los datos de la tabla OPE_COM_SOLICITUD_TRANSF
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 14/Noviembre/2011
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Psp_Calendarizacion_Presu
    {
        public const String Tabla_Ope_Psp_Calendarizacion_Presu = "OPE_PSP_CALENDARIZACION_PRESU";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Fte_Financiamiento_ID = "FTE_FINANCIAMIENTO_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Producto_ID = "PRODUCTO_ID";
        public const String Campo_Area_Funcional_ID = "AREA_FUNCIONAL_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Justificacion = "JUSTIFICACION";
        public const String Campo_Cantidad_Enero = "CANTIDAD_ENERO";
        public const String Campo_Cantidad_Febrero = "CANTIDAD_FEBRERO";
        public const String Campo_Cantidad_Marzo = "CANTIDAD_MARZO";
        public const String Campo_Cantidad_Abril = "CANTIDAD_ABRIL";
        public const String Campo_Cantidad_Mayo = "CANTIDAD_MAYO";
        public const String Campo_Cantidad_Junio = "CANTIDAD_JUNIO";
        public const String Campo_Cantidad_Julio = "CANTIDAD_JULIO";
        public const String Campo_Cantidad_Agosto = "CANTIDAD_AGOSTO";
        public const String Campo_Cantidad_Septiembre = "CANTIDAD_SEPTIEMBRE";
        public const String Campo_Cantidad_Octubre = "CANTIDAD_OCTUBRE";
        public const String Campo_Cantidad_Noviembre = "CANTIDAD_NOVIEMBRE";
        public const String Campo_Cantidad_Diciembre = "CANTIDAD_DICIEMBRE";
        public const String Campo_Importe_Enero = "IMPORTE_ENERO";
        public const String Campo_Importe_Febrero = "IMPORTE_FEBRERO";
        public const String Campo_Importe_Marzo = "IMPORTE_MARZO";
        public const String Campo_Importe_Abril = "IMPORTE_ABRIL";
        public const String Campo_Importe_Mayo = "IMPORTE_MAYO";
        public const String Campo_Importe_Junio = "IMPORTE_JUNIO";
        public const String Campo_Importe_Julio = "IMPORTE_JULIO";
        public const String Campo_Importe_Agosto = "IMPORTE_AGOSTO";
        public const String Campo_Importe_Septiembre = "IMPORTE_SEPTIEMBRE";
        public const String Campo_Importe_Octubre = "IMPORTE_OCTUBRE";
        public const String Campo_Importe_Noviembre = "IMPORTE_NOVIEMBRE";
        public const String Campo_Importe_Diciembre = "IMPORTE_DICIEMBRE";
        public const String Campo_Importe_Total = "IMPORTE_TOTAL";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Capitulo_ID = "CAPITULO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Cat_Parametros_Ejercer_Psp
    /// DESCRIPCION:            Clase que contiene los campos de la tabla CAT_PARAMETROS_EJERCER_PSP
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :            16/NOV/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Cat_Parametros_Ejercer_Psp
    {
        public const String Tabla_Cat_Parametros_Ejercer_Psp = "CAT_PARAMETROS_EJERCER_PSP";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Tipo_De_Consulta = "TIPO_DE_CONSULTA";

    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Psp_Reservas
    /// DESCRIPCION:            Clase que contiene los campos de la tabla OPE_PSP_RESERVAS
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :            15/NOV/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Psp_Reservas
    {
        public const String Tabla_Ope_Psp_Reservas = "OPE_PSP_RESERVAS";
        public const String Campo_No_Reserva = "NO_RESERVA";
        public const String Campo_Concepto = "CONCEPTO";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Fte_Financimiento_ID = "FTE_FINANCIAMIENTO_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Capitulo_ID = "CAPITULO_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Importe_Inicial = "IMPORTE_INICIAL ";
        public const String Campo_Saldo = "SALDO";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Beneficiario = "BENEFICIARIO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Proveedor_ID = "PROVEEDOR_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Tipo_Solicitud_Pago_ID = "TIPO_SOLICITUD_PAGO_ID";
        public const String Campo_Tipo_Reserva = "TIPO_RESERVA";
        public const String Campo_Recurso = "RECURSO";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Ope_Psp_Registro_Movimientos
    /// DESCRIPCION:            Clase que contiene los campos de la tabla OPE_PSP_REGISTRO_MOVIMIETOS
    /// PARAMETROS :     
    /// CREO       :           Susana Trigueros Armenta
    /// FECHA_CREO :            15/NOV/2011 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Psp_Registro_Movimientos
    {
        public const String Tabla_Ope_Psp_Registro_Movimientos = "OPE_PSP_REGISTRO_MOVIMIENTOS";
        public const String Campo_No_Reserva = "NO_RESERVA";
        public const String Campo_Cargo = "CARGO";
        public const String Campo_Abono = "ABONO";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Usuario = "USUARIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_No_Poliza = "NO_POLIZA";
        public const String Campo_Tipo_Poliza_ID = "TIPO_POLIZA_ID";
        public const String Campo_Mes_Ano = "MES_ANO";
        public const String Campo_Partida = "PARTIDA";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: OPE_PSP_PRESUPUESTO_APROBADO
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla OPE_PSP_PRESUPUESTO_APROBADO
    ///PARAMETROS: 
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 15/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:  
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Psp_Presupuesto_Aprobado
    {
        public const String Tabla_Ope_Psp_Presupuesto_Aprobado = "OPE_PSP_PRESUPUESTO_APROBADO";
        public const String Campo_No_Presupuesto = "NO_PRESUPUESTO";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Fte_Financiamiento_ID = "FTE_FINANCIAMIENTO_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Capitulo_ID = "CAPITULO_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Area_Funcional_ID = "AREA_FUNCIONAL_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Aprobado = "APROBADO";
        public const String Campo_Ampliacion = "AMPLIACION";
        public const String Campo_Reduccion = "REDUCCION";
        public const String Campo_Modificado = "MODIFICADO";
        public const String Campo_Devengado = "DEVENGADO";
        public const String Campo_Ejercido = "EJERCIDO";
        public const String Campo_Pagado = "PAGADO";
        public const String Campo_Pre_Comprometido = "PRE_COMPROMETIDO";
        public const String Campo_Comprometido = "COMPROMETIDO";
        public const String Campo_Disponible = "DISPONIBLE";
        public const String Campo_Saldo = "SALDO";
        public const String Campo_Importe_Enero = "IMPORTE_ENERO";
        public const String Campo_Importe_Febrero = "IMPORTE_FEBRERO";
        public const String Campo_Importe_Marzo = "IMPORTE_MARZO";
        public const String Campo_Importe_Abril = "IMPORTE_ABRIL";
        public const String Campo_Importe_Mayo = "IMPORTE_MAYO";
        public const String Campo_Importe_Junio = "IMPORTE_JUNIO";
        public const String Campo_Importe_Julio = "IMPORTE_JULIO";
        public const String Campo_Importe_Agosto = "IMPORTE_AGOSTO";
        public const String Campo_Importe_Septiembre = "IMPORTE_SEPTIEMBRE";
        public const String Campo_Importe_Octubre = "IMPORTE_OCTUBRE";
        public const String Campo_Importe_Noviembre = "IMPORTE_NOVIEMBRE";
        public const String Campo_Importe_Diciembre = "IMPORTE_DICIEMBRE";
        public const String Campo_Afectado_Enero = "AFECTADO_ENERO";
        public const String Campo_Afectado_Febrero = "AFECTADO_FEBRERO";
        public const String Campo_Afectado_Marzo = "AFECTADO_MARZO";
        public const String Campo_Afectado_Abril = "AFECTADO_ABRIL";
        public const String Campo_Afectado_Mayo = "AFECTADO_MAYO";
        public const String Campo_Afectado_Junio = "AFECTADO_JUNIO";
        public const String Campo_Afectado_Julio = "AFECTADO_JULIO";
        public const String Campo_Afectado_Agosto = "AFECTADO_AGOSTO";
        public const String Campo_Afectado_Septiembre = "AFECTADO_SEPTIEMBRE";
        public const String Campo_Afectado_Octubre = "AFECTADO_OCTUBRE";
        public const String Campo_Afectado_Noviembre = "AFECTADO_NOVIEMBRE";
        public const String Campo_Afectado_Diciembre = "AFECTADO_DICIEMBRE";
        public const String Campo_Importe_Total = "IMPORTE_TOTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Cat_Psp_Parametros
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PSP_PARAMETROS
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda
    ///FECHA_CREO: 18/Octubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:  
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Psp_Parametros
    {
        public const String Tabla_Cat_Psp_Parametros = "CAT_PSP_PARAMETROS";
        public const String Campo_Parametro_ID = "PARAMETRO_ID";
        public const String Campo_Fecha_Apertura = "FECHA_APERTURA";
        public const String Campo_Fecha_Cierre = "FECHA_CIERRE";
        public const String Campo_Anio_Presupuestar = "ANIO_PRESUPUESTAR";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Fte_Financiamiento_ID = "FTE_FINANCIAMIENTO_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Cat_Psp_Parametros_Detalles
    ///DESCRIPCIÓN: Clase que contiene los campos de la tabla CAT_PSP_PARAMETROS_DETALLES
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda
    ///FECHA_CREO: 18/Octubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:  
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Cat_Psp_Parametros_Detalles
    {
        public const String Tabla_Cat_Psp_Parametros_Detalles = "CAT_PSP_PARAMETROS_DETALLES";
        public const String Campo_Parametro_ID = "PARAMETRO_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Psp_Limmite_presupuestal
    /// DESCRIPCION: 
    /// PARAMETROS :
    /// CREO       : Gustavo Angeles Cruz
    /// FECHA_CREO : 19 / oct /2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Psp_Limite_presupuestal
    {
        public const String Tabla_Ope_Psp_Limite_presupuestal = "OPE_PSP_LIMITE_PRESUPUESTAL";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Limite_Presupuestal = "LIMITE_PRESUPUESTAL";
        public const String Campo_Anio_presupuestal = "ANIO_PRESUPUESTAL";
        public const String Campo_Fte_Financiamiento_ID = "FTE_FINANCIAMIENTO_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Ope_Psp_Detalle_Lim_Presup
    /// DESCRIPCION: 
    /// PARAMETROS :
    /// CREO       : Gustavo Angeles Cruz
    /// FECHA_CREO : 19 / oct /2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public class Ope_Psp_Detalle_Lim_Presup
    {
        public const String Tabla_Ope_Psp_Limite_presupuestal = "OPE_PSP_DETALLE_LIM_PRESUP";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Capitulo_ID = "CAPITULO_ID";
        public const String Campo_Anio_presupuestal = "ANIO_PRESUPUESTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///****************************************************************************************************************************************************************
    ///NOMBRE: Cat_Sap_Proyectos_Programas
    ///
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla CAT_SAP_PROYECTOS_PROGRAMAS
    ///
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA CREÓ: 17/Octubra/2011
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Cat_Sap_Proyectos_Programas
    {
        public const String Tabla_Cat_Sap_Proyectos_Programas = "CAT_SAP_PROYECTOS_PROGRAMAS";
        public const String Campo_Proyecto_Programa_Id = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Elemento_Pep = "ELEMENTO_PEP";
        public const String Campo_Nombre = "NOMBRE";
    }

    ///****************************************************************************************************************************************************************
    ///NOMBRE: Cat_Ope_Com_Solicitud_Transf
    ///
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla OPE_COM_SOLICITUD_TRANSF
    ///
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ: 18/Octubre/2011
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Cat_Ope_Com_Solicitud_Transf
    {
        public const String Tabla_Cat_Ope_Com_Solicitud_Transf = "OPE_COM_SOLICITUD_TRANSF";
        public const String Campo_No_Solicitud = "NO_SOLICITUD";
        public const String Campo_Codigo1 = "CODIGO1";
        public const String Campo_Codigo2 = "CODIGO2";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Justificacion = "JUSTIFICACION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Justificacion_Solicitud = "JUSTIFICACION_SOLICITUD";
        public const String Campo_Tipo_Operacion = "TIPO_OPERACION";
        public const String Campo_Origen_Fuente_Financiamiento_Id = "ORIGEN_FTE_FINANCIAMIENTO_ID";
        public const String Campo_Destino_Fuente_Financiamiento_Id = "DESTINO_FTE_FINANCIAMIENTO_ID";
        public const String Campo_Origen_Area_Funcional_Id = "ORIGEN_AREA_FUNCIONAL_ID";
        public const String Campo_Destino_Area_Funcional_Id = "DESTINO_AREA_FUNCIONAL_ID";
        public const String Campo_Origen_Programa_Id = "ORIGEN_PROGRAMA_ID";
        public const String Campo_Destino_Programa_Id = "DESTINO_PROGRAMA_ID";
        public const String Campo_Origen_Partida_Id = "ORIGEN_PARTIDA_ID";
        public const String Campo_Destino_Partida_Id = "DESTINO_PARTIDA_ID";
        public const String Campo_Origen_Dependencia_Id = "ORIGEN_DEPENDENCIA_ID";
        public const String Campo_Destino_Dependencia_Id = "DESTINO_DEPENDENCIA_ID";
        public const String Campo_Mes_Origen = "MES_ORIGEN";
        public const String Campo_Mes_Destino = "MES_DESTINO";
        public const String Campo_No_Reserva = "NO_RESERVA";
        public const String Campo_Anio = "ANIO";
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Com_Solicitud_Transf_Det
    ///DESCRIPCIÓN          : Clase con contiene los datos de la tabla Ope_Com_Solicitud_Transf_Det
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 23/Noviembre/2011
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Com_Solicitud_Transf_Det
    {
        public const String Tabla_Ope_Psp_Hist_Calendar_Presu = "OPE_COM_SOLICITUD_TRANSF_DET";
        public const String Campo_No_Solicitud_Detalle = "NO_SOLICITUD_DETALLE";
        public const String Campo_No_Solicitud = "NO_SOLICITUD";
        public const String Campo_Importe_Enero = "IMPORTE_ENERO";
        public const String Campo_Importe_Febrero = "IMPORTE_FEBRERO";
        public const String Campo_Importe_Marzo = "IMPORTE_MARZO";
        public const String Campo_Importe_Abril = "IMPORTE_ABRIL";
        public const String Campo_Importe_Mayo = "IMPORTE_MAYO";
        public const String Campo_Importe_Junio = "IMPORTE_JUNIO";
        public const String Campo_Importe_Julio = "IMPORTE_JULIO";
        public const String Campo_Importe_Agosto = "IMPORTE_AGOSTO";
        public const String Campo_Importe_Septiembre = "IMPORTE_SEPTIEMBRE";
        public const String Campo_Importe_Octubre = "IMPORTE_OCTUBRE";
        public const String Campo_Importe_Noviembre = "IMPORTE_NOVIEMBRE";
        public const String Campo_Importe_Diciembre = "IMPORTE_DICIEMBRE";
        public const String Campo_Tipo = "TIPO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Psp_Reservas_Detalles
    ///DESCRIPCIÓN          : Clase con contiene los datos de la tabla Ope_Psp_Reservas_Detalles
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 20/ENERO/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Psp_Reservas_Detalles
    {
        public const String Tabla_Ope_Psp_Reservas_Detalles = "OPE_PSP_RESERVAS_DETALLES";
        public const String Campo_No_Reserva_Detalle = "NO_RESERVA_DETALLE";
        public const String Campo_No_Reserva = "NO_RESERVA";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Importe_Inicial = "IMPORTE_INICIAL";
        public const String Campo_Saldo = "SALDO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Fte_Financimiento_ID = "FTE_FINANCIAMIENTO_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : CAT_PSP_RUBRO
    ///DESCRIPCIÓN          : Clase con contiene los datos de la tabla CAT_PSP_RUBRO
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 15/Marzo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Psp_Rubro
    {
        public const String Tabla_Cat_Psp_Rubro = "CAT_PSP_RUBRO";
        public const String Campo_Rubro_ID = "RUBRO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : CAT_PSP_TIPO
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_PSP_TIPO
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 15/Marzo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Psp_Tipo
    {
        public const String Tabla_Cat_Psp_Tipo = "CAT_PSP_TIPO";
        public const String Campo_Tipo_ID = "TIPO_ID";
        public const String Campo_Rubro_ID = "RUBRO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Psp_Clase_Ing
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Cat_Psp_Clase_Ing
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 20/Marzo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Psp_Clase_Ing
    {
        public const String Tabla_Cat_Psp_Clase_Ing = "CAT_PSP_CLASE_ING";
        public const String Campo_Clase_Ing_ID = "CLASE_ING_ID";
        public const String Campo_Tipo_ID = "TIPO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Psp_Concepto_Ing
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Cat_Psp_Concepto_Ing
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 20/Marzo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Psp_Concepto_Ing
    {
        public const String Tabla_Cat_Psp_Concepto_Ing = "CAT_PSP_CONCEPTO_ING";
        public const String Campo_Concepto_Ing_ID = "CONCEPTO_ING_ID";
        public const String Campo_Clase_Ing_ID = "CLASE_ING_ID";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Banco_ID = "BANCO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Psp_SubConcepto_Ing
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Cat_Psp_SubConcepto_Ing
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           :  07/Mayo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Psp_SubConcepto_Ing
    {
        public const String Tabla_Cat_Psp_SubConcepto_Ing = "CAT_PSP_SUBCONCEPTO_ING";
        public const String Campo_SubConcepto_Ing_ID = "SUBCONCEPTO_ING_ID";
        public const String Campo_Concepto_Ing_ID = "CONCEPTO_ING_ID";
        public const String Campo_Cuenta_Contable_ID = "CUENTA_CONTABLE_ID";
        public const String Campo_Proceso_ID = "PROCESO_ID";
        public const String Campo_Clave = "CLAVE";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Fundamento = "FUNDAMENTO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Requiere_Folio = "REQUIERE_FOLIO";
        public const String Campo_Mostrar_Tipo_Pago = "MOSTRAR_TIPO_PAGO";
        public const String Campo_Aplica_Descuento_Tipo_Pago = "APLICA_DESCUENTO_PRONTO_PAGO";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Psp_Pronostico_Ingresos
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Cat_Psp_Pronostico_Ingresos
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 21/Marzo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Psp_Pronostico_Ingresos
    {
        public const String Tabla_Ope_Psp_Pronostico_Ingresos = "OPE_PSP_PRONOSTICO_INGRESOS";
        public const String Campo_Pronostico_Ing_ID = "PRONOSTICO_ING_ID";
        public const String Campo_Rubro_ID = "RUBRO_ID";
        public const String Campo_Tipo_ID = "TIPO_ID";
        public const String Campo_Clase_Ing_ID = "CLASE_ING_ID";
        public const String Campo_Concepto_Ing_ID = "CONCEPTO_ING_ID";
        public const String Campo_SubConcepto_Ing_ID = "SUBCONCEPTO_ING_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Justificacion = "JUSTIFICACION";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Importe_Enero = "IMPORTE_ENERO";
        public const String Campo_Importe_Febrero = "IMPORTE_FEBRERO";
        public const String Campo_Importe_Marzo = "IMPORTE_MARZO";
        public const String Campo_Importe_Abril = "IMPORTE_ABRIL";
        public const String Campo_Importe_Mayo = "IMPORTE_MAYO";
        public const String Campo_Importe_Junio = "IMPORTE_JUNIO";
        public const String Campo_Importe_Julio = "IMPORTE_JULIO";
        public const String Campo_Importe_Agosto = "IMPORTE_AGOSTO";
        public const String Campo_Importe_Septiembre = "IMPORTE_SEPTIEMBRE";
        public const String Campo_Importe_Octubre = "IMPORTE_OCTUBRE";
        public const String Campo_Importe_Noviembre = "IMPORTE_NOVIEMBRE";
        public const String Campo_Importe_Diciembre = "IMPORTE_DICIEMBRE";
        public const String Campo_Importe_Total = "IMPORTE_TOTAL";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Psp_Presupuesto_Ingresos
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Ope_Psp_Presupuesto_Ingresos
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 12/Abril/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Psp_Presupuesto_Ingresos
    {
        public const String Tabla_Ope_Psp_Presupuesto_Ingresos = "OPE_PSP_PRESUPUESTO_INGRESOS";
        public const String Campo_Presupuesto_Ing_ID = "PRESUPUESTO_ING_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        public const String Campo_Rubro_ID = "RUBRO_ID";
        public const String Campo_Tipo_ID = "TIPO_ID";
        public const String Campo_Clase_Ing_ID = "CLASE_ING_ID";
        public const String Campo_Concepto_Ing_ID = "CONCEPTO_ING_ID";
        public const String Campo_SubConcepto_Ing_ID = "SUBCONCEPTO_ING_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Importe_Enero = "IMPORTE_ENERO";
        public const String Campo_Importe_Febrero = "IMPORTE_FEBRERO";
        public const String Campo_Importe_Marzo = "IMPORTE_MARZO";
        public const String Campo_Importe_Abril = "IMPORTE_ABRIL";
        public const String Campo_Importe_Mayo = "IMPORTE_MAYO";
        public const String Campo_Importe_Junio = "IMPORTE_JUNIO";
        public const String Campo_Importe_Julio = "IMPORTE_JULIO";
        public const String Campo_Importe_Agosto = "IMPORTE_AGOSTO";
        public const String Campo_Importe_Septiembre = "IMPORTE_SEPTIEMBRE";
        public const String Campo_Importe_Octubre = "IMPORTE_OCTUBRE";
        public const String Campo_Importe_Noviembre = "IMPORTE_NOVIEMBRE";
        public const String Campo_Importe_Diciembre = "IMPORTE_DICIEMBRE";
        public const String Campo_Importe_Total = "IMPORTE_TOTAL";
        public const String Campo_Aprobado = "APROBADO";
        public const String Campo_Ampliacion = "AMPLIACION";
        public const String Campo_Reduccion = "REDUCCION";
        public const String Campo_Modificado = "MODIFICADO";
        public const String Campo_Devengado = "DEVENGADO";
        public const String Campo_Recaudado = "RECAUDADO";
        public const String Campo_Devengado_Recaudado = "DEVENGADO_RECAUDADO";
        public const String Campo_Compromiso = "COMPROMISO";
        public const String Campo_Por_Recaudar = "POR_RECAUDAR";
        public const String Campo_Saldo = "SALDO";
        public const String Campo_Acumulado_Enero = "ACUMULADO_ENERO";
        public const String Campo_Acumulado_Febrero = "ACUMULADO_FEBRERO";
        public const String Campo_Acumulado_Marzo = "ACUMULADO_MARZO";
        public const String Campo_Acumulado_Abril = "ACUMULADO_ABRIL";
        public const String Campo_Acumulado_Mayo = "ACUMULADO_MAYO";
        public const String Campo_Acumulado_Junio = "ACUMULADO_JUNIO";
        public const String Campo_Acumulado_Julio = "ACUMULADO_JULIO";
        public const String Campo_Acumulado_Agosto = "ACUMULADO_AGOSTO";
        public const String Campo_Acumulado_Septiembre = "ACUMULADO_SEPTIEMBRE";
        public const String Campo_Acumulado_Octubre = "ACUMULADO_OCTUBRE";
        public const String Campo_Acumulado_Noviembre = "ACUMULADO_NOVIEMBRE";
        public const String Campo_Acumulado_Diciembre = "ACUMULADO_DICIEMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Psp_Presupuesto_Ing_Esp
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Ope_Psp_Presupuesto_Ing_Esp
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 09/Mayo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Psp_Presupuesto_Ing_Esp
    {
        public const String Tabla_Ope_Psp_Presupuesto_Ing_Esp = "OPE_PSP_PRESUPUESTO_ING_ESP";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Rubro_ID = "RUBRO_ID";
        public const String Campo_Tipo_ID = "TIPO_ID";
        public const String Campo_Clase_Ing_ID = "CLASE_ING_ID";
        public const String Campo_Concepto_Ing_ID = "CONCEPTO_ING_ID";
        public const String Campo_SubConcepto_Ing_ID = "SUBCONCEPTO_ING_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_No_Modificacion = "NO_MODIFICACION";
        public const String Campo_Importe_Enero = "IMPORTE_ENERO";
        public const String Campo_Importe_Febrero = "IMPORTE_FEBRERO";
        public const String Campo_Importe_Marzo = "IMPORTE_MARZO";
        public const String Campo_Importe_Abril = "IMPORTE_ABRIL";
        public const String Campo_Importe_Mayo = "IMPORTE_MAYO";
        public const String Campo_Importe_Junio = "IMPORTE_JUNIO";
        public const String Campo_Importe_Julio = "IMPORTE_JULIO";
        public const String Campo_Importe_Agosto = "IMPORTE_AGOSTO";
        public const String Campo_Importe_Septiembre = "IMPORTE_SEPTIEMBRE";
        public const String Campo_Importe_Octubre = "IMPORTE_OCTUBRE";
        public const String Campo_Importe_Noviembre = "IMPORTE_NOVIEMBRE";
        public const String Campo_Importe_Diciembre = "IMPORTE_DICIEMBRE";
        public const String Campo_Importe_Total = "IMPORTE_TOTAL";
        public const String Campo_Aprobado = "APROBADO";
        public const String Campo_Ampliacion = "AMPLIACION";
        public const String Campo_Reduccion = "REDUCCION";
        public const String Campo_Modificado = "MODIFICADO";
        public const String Campo_Devengado = "DEVENGADO";
        public const String Campo_Recaudado = "RECAUDADO";
        public const String Campo_Devengado_Recaudado = "DEVENGADO_RECAUDADO";
        public const String Campo_Compromiso = "COMPROMISO";
        public const String Campo_Por_Recaudar = "POR_RECAUDAR";
        public const String Campo_Saldo = "SALDO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Psp_Movimiento_Ing_Det
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Ope_Psp_Movimiento_Ing_Det
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 16/Abril/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Psp_Movimiento_Ing_Det
    {
        public const String Tabla_Ope_Psp_Movimiento_Ing_Det = "OPE_PSP_MOVIMIENTO_ING_DET";
        public const String Campo_Movimiento_Ing_ID = "MOVIMIENTO_ING_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Rubro_ID = "RUBRO_ID";
        public const String Campo_Tipo_ID = "TIPO_ID";
        public const String Campo_Clase_Ing_ID = "CLASE_ING_ID";
        public const String Campo_Concepto_Ing_ID = "CONCEPTO_ING_ID";
        public const String Campo_SubConcepto_Ing_ID = "SUBCONCEPTO_ING_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_No_Movimiento_Ing = "NO_MOVIMIENTO_ING";
        public const String Campo_Importe_Enero = "IMPORTE_ENERO";
        public const String Campo_Importe_Febrero = "IMPORTE_FEBRERO";
        public const String Campo_Importe_Marzo = "IMPORTE_MARZO";
        public const String Campo_Importe_Abril = "IMPORTE_ABRIL";
        public const String Campo_Importe_Mayo = "IMPORTE_MAYO";
        public const String Campo_Importe_Junio = "IMPORTE_JUNIO";
        public const String Campo_Importe_Julio = "IMPORTE_JULIO";
        public const String Campo_Importe_Agosto = "IMPORTE_AGOSTO";
        public const String Campo_Importe_Septiembre = "IMPORTE_SEPTIEMBRE";
        public const String Campo_Importe_Octubre = "IMPORTE_OCTUBRE";
        public const String Campo_Importe_Noviembre = "IMPORTE_NOVIEMBRE";
        public const String Campo_Importe_Diciembre = "IMPORTE_DICIEMBRE";
        public const String Campo_Importe_Total = "IMPORTE_TOTAL";
        public const String Campo_Importe_Aprobado = "IMPORTE_APROBADO";
        public const String Campo_Importe_Ampliacion = "IMPORTE_AMPLIACION";
        public const String Campo_Importe_Reduccion = "IMPORTE_REDUCCION";
        public const String Campo_Importe_Modificado = "IMPORTE_MODIFICADO";
        public const String Campo_Tipo_Movimiento = "TIPO_MOVIMIENTO";
        public const String Campo_Estatus = "Estatus";
        public const String Campo_Justificacion = "JUSTIFICACION";
        public const String Campo_Comentario = "COMENTARIO";
        public const String Campo_Tipo_Concepto = "TIPO_CONCEPTO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Psp_Movimiento_Ing
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Ope_Psp_Movimiento_Ing
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 09/MAYO/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Psp_Movimiento_Ing
    {
        public const String Tabla_Ope_Psp_Movimiento_Ing = "OPE_PSP_MOVIMIENTO_ING";
        public const String Campo_No_Movimiento_Ing = "NO_MOVIMIENTO_ING";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Total_Modificado = "TOTAL_MODIFICADO";
        public const String Campo_Total_Modificado_Egr = "TOTAL_MODIFICADO_EGR";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Estatus_Ingresos = "ESTATUS_INGRESOS";
        public const String Campo_Estatus_Egr_Municipal = "ESTATUS_EGR_MUNICIPAL";
        public const String Campo_Estatus_Egr_Ramo33 = "ESTATUS_EGR_RAMO33";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Psp_Presupuesto_Egr_Esp
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Ope_Psp_Presupuesto_Egr_Esp
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 12/MAYO/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Psp_Presupuesto_Egr_Esp
    {
        public const String Tabla_Ope_Psp_Presupuesto_Egr_Esp = "OPE_PSP_PRESUPUESTO_EGR_ESP";
        public const String Campo_Fte_Financiamiento_ID = "FTE_FINANCIAMIENTO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Area_Funcional_ID = "AREA_FUNCIONAL_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Tipo_Egreso = "TIPO_EGRESO";
        public const String Campo_No_Modificacion = "NO_MOVIMIENTO";
        public const String Campo_Aprobado = "APROBADO";
        public const String Campo_Ampliacion = "AMPLIACION";
        public const String Campo_Reduccion = "REDUCCION";
        public const String Campo_Modificado = "MODIFICADO";
        public const String Campo_Devengado = "DEVENGADO";
        public const String Campo_Ejercido = "EJERCIDO";
        public const String Campo_Pagado = "PAGADO";
        public const String Campo_Pre_Comprometido = "PRE_COMPROMETIDO";
        public const String Campo_Comprometido = "COMPROMETIDO";
        public const String Campo_Disponible = "DISPONIBLE";
        public const String Campo_Saldo = "SALDO";
        public const String Campo_Importe_Enero = "IMPORTE_ENERO";
        public const String Campo_Importe_Febrero = "IMPORTE_FEBRERO";
        public const String Campo_Importe_Marzo = "IMPORTE_MARZO";
        public const String Campo_Importe_Abril = "IMPORTE_ABRIL";
        public const String Campo_Importe_Mayo = "IMPORTE_MAYO";
        public const String Campo_Importe_Junio = "IMPORTE_JUNIO";
        public const String Campo_Importe_Julio = "IMPORTE_JULIO";
        public const String Campo_Importe_Agosto = "IMPORTE_AGOSTO";
        public const String Campo_Importe_Septiembre = "IMPORTE_SEPTIEMBRE";
        public const String Campo_Importe_Octubre = "IMPORTE_OCTUBRE";
        public const String Campo_Importe_Noviembre = "IMPORTE_NOVIEMBRE";
        public const String Campo_Importe_Diciembre = "IMPORTE_DICIEMBRE";
        public const String Campo_Importe_Total = "IMPORTE_TOTAL";
        public const String Campo_Actualizado = "ACTUALIZADO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Psp_Movimiento_Egr
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Ope_Psp_Movimiento_Egr
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 12/MAYO/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Psp_Movimiento_Egr
    {
        public const String Tabla_Ope_Psp_Movimiento_Egr = "OPE_PSP_MOVIMIENTO_EGR";
        public const String Campo_No_Movimiento_Egr = "NO_MOVIMIENTO_EGR";
        public const String Campo_Anio = "ANIO";
        public const String Campo_Tipo_Egreso = "TIPO_EGRESO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Total_Modificado = "TOTAL_MODIFICADO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Psp_Movimiento_Egr_Det
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Ope_Psp_Movimiento_Egr_Det
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 12/Mayo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Psp_Movimiento_Egr_Det
    {
        public const String Tabla_Ope_Psp_Movimiento_Egr_Det = "OPE_PSP_MOVIMIENTO_EGR_DET";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Movimiento_ID = "MOVIMIENTO_ID";
        public const String Campo_Fuente_Financiamiento_ID = "FUENTE_FINANCIAMIENTO_ID";
        public const String Campo_Dependencia_ID = "DEPENDENCIA_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Area_Funcional_ID = "AREA_FUNCIONAL_ID";
        public const String Campo_Partida_ID = "PARTIDA_ID";
        public const String Campo_Anio = "ANIO";
        public const String Campo_No_Movimiento_Egr = "NO_MOVIMIENTO_EGR";
        public const String Campo_Importe_Enero = "IMPORTE_ENERO";
        public const String Campo_Importe_Febrero = "IMPORTE_FEBRERO";
        public const String Campo_Importe_Marzo = "IMPORTE_MARZO";
        public const String Campo_Importe_Abril = "IMPORTE_ABRIL";
        public const String Campo_Importe_Mayo = "IMPORTE_MAYO";
        public const String Campo_Importe_Junio = "IMPORTE_JUNIO";
        public const String Campo_Importe_Julio = "IMPORTE_JULIO";
        public const String Campo_Importe_Agosto = "IMPORTE_AGOSTO";
        public const String Campo_Importe_Septiembre = "IMPORTE_SEPTIEMBRE";
        public const String Campo_Importe_Octubre = "IMPORTE_OCTUBRE";
        public const String Campo_Importe_Noviembre = "IMPORTE_NOVIEMBRE";
        public const String Campo_Importe_Diciembre = "IMPORTE_DICIEMBRE";
        public const String Campo_Importe_Total = "IMPORTE_TOTAL";
        public const String Campo_Importe_Aprobado = "IMPORTE_APROBADO";
        public const String Campo_Importe_Ampliacion = "IMPORTE_AMPLIACION";
        public const String Campo_Importe_Reduccion = "IMPORTE_REDUCCION";
        public const String Campo_Importe_Modificado = "IMPORTE_MODIFICADO";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Tipo_Operacion = "TIPO_OPERACION";
        public const String Campo_Tipo_Partida = "TIPO_PARTIDA";
        public const String Campo_Tipo_Egreso = "TIPO_EGRESO";
        public const String Campo_Tipo_Movimiento = "TIPO_MOVIMIENTO";
        public const String Campo_Tipo_Usuario = "TIPO_USUARIO";
        public const String Campo_Justificacion = "JUSTIFICACION";
        public const String Campo_Comentario = "COMENTARIO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Sap_Det_Fte_Programa
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Cat_Sap_Det_Fte_Programa
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Sap_Det_Fte_Programa
    {
        public const String Tabla_Cat_Sap_Det_Fte_Programa = "CAT_SAP_DET_FTE_PROGRAMA";
        public const String Campo_Fuente_Financiamiento_ID = "FTE_FINANCIAMIENTO_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Importe_Calendarizado = "IMPORTE_CALENDARIZADO";
        public const String Campo_Importe_Modificado = "IMPORTE_MODIFICADO";
        public const String Campo_Porcentaje = "PORCENTAJE";
        public const String Campo_Concepto_Ing_ID = "CONCEPTO_ING_ID";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Sap_Det_Fte_Concepto
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Cat_Sap_Det_Fte_Concepto
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 22/Junio/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Sap_Det_Fte_Concepto
    {
        public const String Tabla_Cat_Sap_Det_Fte_Concepto = "CAT_SAP_DET_FTE_CONCEPTO";
        public const String Campo_Fuente_Financiamiento_ID = "FTE_FINANCIAMIENTO_ID";
        public const String Campo_Concepto_Ing_ID = "CONCEPTO_ING_ID";
    }
    #endregion

    ///****************************************************************************************************************************************************************
    ///NOMBRE:      Apl_Cat_Modulos_Siag
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla Apl_Cat_Modulos_Siag
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:  11/Enero/2012
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    public class Apl_Cat_Modulos_Siag
    {
        public const String Tabla_Apl_Cat_Modulos_Siag = "APL_CAT_MODULOS_SIAG";
        public const String Tabla_Apl_Cat_Modulos_SIAG = "APL_CAT_MODULOS_SIAG";
        public const String Campo_Modulo_ID = "MODULO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Psp_Registro_Mov_Ingreso
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Ope_Psp_Registro_Mov_Ingreso
    ///PARAMETROS           :
    ///CREO                 : Leslie Gonzalez Vázquez
    ///FECHA_CREO           : 28/Agosto/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Psp_Registro_Mov_Ingreso
    {
        public const String Tabla_Ope_Psp_Registro_Mov_Ingreso = "OPE_PSP_REGISTRO_MOV_INGRESO";
        public const String Campo_No_Movimiento = "NO_MOVIMIENTO";
        public const String Campo_Cargo = "CARGO";
        public const String Campo_Abono = "ABONO";
        public const String Campo_Importe = "IMPORTE";
        public const String Campo_Fecha = "FECHA";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Referencia = "REFERENCIA";
        public const String Campo_No_Poliza = "NO_POLIZA";
        public const String Campo_Tipo_Poliza_ID = "TIPO_POLIZA_ID";
        public const String Campo_Mes_Ano = "MES_ANO";
        public const String Campo_Fte_Financiamiento_ID = "FTE_FINANCIAMIENTO_ID";
        public const String Campo_Proyecto_Programa_ID = "PROYECTO_PROGRAMA_ID";
        public const String Campo_Rubro_ID = "RUBRO_ID";
        public const String Campo_Tipo_ID = "TIPO_ID";
        public const String Campo_Clase_Ing_ID = "CLASE_ING_ID";
        public const String Campo_Concepto_Ing_ID = "CONCEPTO_ING_ID";
        public const String Campo_SubConcepto_Ing_ID = "SUBCONCEPTO_ING_ID";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }


    ///****************************************************************************************************************************************************************
    ///NOMBRE:      Apl_Cat_Modulos_Siag
    ///DESCRIPCIÓN: Clase con contiene los datos de la tabla Apl_Cat_Modulos_Siag
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:  11/Enero/2012
    ///MODIFICO:
    ///FECHA MODIFICO:
    ///CAUSA MODIFICACION:
    ///****************************************************************************************************************************************************************
    //public class Apl_Cat_Modulos_Siag
    //{
    //    public const String Tabla_Apl_Cat_Modulos_SIAG = "APL_CAT_MODULOS_SIAG";
    //    public const String Tabla_Apl_Cat_Modulos_SIAG = "APL_CAT_MODULOS_SIAG";
    //    public const String Campo_Modulo_ID = "MODULO_ID";
    //    public const String Campo_Nombre = "NOMBRE";
    //    public const String Campo_Usuario_Creo = "USUARIO_CREO";
    //    public const String Campo_Fecha_Creo = "FECHA_CREO";
    //    public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
    //    public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    //}

    #region VETANILLA

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Ven_Usuarios
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_VEN_USUARIOS
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 01/Mayo/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ven_Usuarios
    {
        public const String Tabla_Cat_Ven_Usuarios = "CAT_VEN_USUARIOS";
        public const String Campo_Usuario_ID = "USUARIO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Apellido_Paterno = "APELLIDO_PATERNO";
        public const String Campo_Apellido_Materno = "APELLIDO_MATERNO";
        public const String Campo_Nombre_Completo = "NOMBRE_COMPLETO";
        public const String Campo_Email = "EMAIL";
        public const String Campo_Calle = "CALLE_UBICACION";
        public const String Campo_Colonia = "COLONIA_UBICACION";
        public const String Campo_Ciudad = "CIUDAD_UBICACION";
        public const String Campo_Estado = "ESTADO_UBICACION";
        public const String Campo_Telefono_Casa = "TELEFONO"; 
        public const String Campo_Password = "PASSWORD";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Calle_ID = "CALLE_ID";
        public const String Campo_Colonia_ID = "COLONIA_ID";
        public const String Campo_Codigo_Postal = "CODIGO_POSTAL";
        public const String Campo_Celular = "CELULAR";
        public const String Campo_Fecha_Nacimiento = "FECHA_NACIMIENTO";
        public const String Campo_Edad = "EDAD";
        public const String Campo_Sexo = "SEXO";
        public const String Campo_Rfc = "RFC";
        public const String Campo_Curp = "CURP";
        public const String Campo_Fecha_Registro = "FECHA_REGISTRO";
        public const String Campo_Comentarios = "COMENTARIOS";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Pregunta_Secreta = "PREGUNTA_SECRETA";
        public const String Campo_Respuesta_Secreta = "RESPUESTA_SECRETA";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Ven_Param_Correo
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_VEN_PARAM_CORREO
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Leslie Gonzalez Vazquez
    ///FECHA CREÓ:          : 02/Mayo/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ven_Param_Correo
    {
        public const String Tabla_Cat_Ven_Param_Correo = "CAT_VEN_PARAM_CORREO";
        public const String Campo_Correo_Puerto = "CORREO_PUERTO";
        public const String Campo_Correo_Servidor = "CORREO_SERVIDOR";
        public const String Campo_Correo_Notificador = "CORREO_NOTIFICADOR";
        public const String Campo_Password_Correo_Notificador = "PASSWORD_CORREO_NOTIFICADOR";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Ven_Parametros
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_VEN_PARAMETROS
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Roberto González Oseguera
    ///FECHA CREÓ:          : 21-may-12 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ven_Parametros
    {
        public const String Tabla_Cat_Ven_Parametros = "CAT_VEN_PARAMETROS";
        public const String Campo_Programa_ID_Web = "PROGRAMA_ID_WEB";
        public const String Campo_Programa_ID_Ventanilla = "PROGRAMA_ID_VENTANILLA";
        public const String Campo_Programa_ID_Genera_Consecutivo = "PROGRAMA_ID_GENERA_CONSECUTIVO";
        public const String Campo_Programa_ID_Atiende_Direccion = "PROGRAMA_ID_ATIENDE_DIRECCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    #endregion


    #region ORDENAMIENTO TERRITORIAL

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Ort_Det_Bitacora
    ///DESCRIPCIÓN: Clase que contiene los campos
    /// PARÁMETROS :     
    /// CREO       : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO : 27/Junio/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Ort_Det_Bitacora
    {
        public const String Tabla_Ope_Ort_Det_Bitacora = "OPE_ORT_DET_BITACORA";
        public const String Campo_Detalle_Bitacora_ID = "DETALLE_BITACORA_ID";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Documento_ID = "DOCUMENTO_ID";
        public const String Campo_Ubicacion = "UBICACION_EXPEDIENTE";
        public const String Campo_Fecha_Prestamo = "FECHA_PRESTAMO";
        public const String Campo_Usuario_Prestamo = "USUARIO_PRESTAMO";
        public const String Campo_Observaciones = "OBSERVACIONES";
        public const String Campo_Fecha_Devolucion = "FECHA_DEVOLUCION";
        public const String Campo_Encuesta_Pregunta_Satisfaccion = "PREGUNTA_SATISFACCION";
        public const String Campo_Encuesta_Tiempo_Espera = "TIEMPO_ESPERA";
        public const String Campo_Encuesta_Fecha_Encuesta = "FECHA_ENCUESTA";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA CLASE: Ope_Ort_Bitacora_Documento
    ///DESCRIPCIÓN: Clase que contiene los campos
    /// PARÁMETROS :     
    /// CREO       : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO : 26/Junio/2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public class Ope_Ort_Bitacora_Documento
    {
        public const String Tabla_Ope_Ort_Bitacora_Documento = "OPE_ORT_BITACORA_DOCUMENTO";
        public const String Campo_Bitacora_ID = "BITACORA_ID";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Subproceso_ID = "SUBPROCESO_ID";
        public const String Campo_Estatus = "ESTATUS";
        public const String Campo_Documento_ID = "DOCUMENTO_ID";
        public const String Campo_Fecha_Entrega_Documento = "FECHA_ENTREGA_DOC";
        public const String Campo_Estatus_Prestamo = "ESTATUS_PRESTAMO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : cat_ort_tipo_material
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_ORT_TIPO_MATERIAL
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 01/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ort_Tipo_Material
    {
        public const String Tabla_Cat_Ort_Tipo_Material = "CAT_ORT_TIPO_MATERIAL";
        public const String Campo_Material_ID = "MATERIAL_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : CAT_ORT_AREA_PUBLIC_DONAC
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_ORT_AREA_PUBLIC_DONAC
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 01/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ort_Area_Public_Donac
    {
        public const String Tabla_Cat_Ort_Area_Publica_Donacion = "CAT_ORT_AREA_PUBLIC_DONAC";
        public const String Campo_Area_ID = "AREA_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Ort_Avance_Obra
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_ORT_AVANCE_OBRA
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 01/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ort_Avance_Obra
    {
        public const String Tabla_Cat_Ort_Avance_Obran = "CAT_ORT_AVANCE_OBRA";
        public const String Campo_Avance_Obra_ID = "AVANCE_OBRA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Ort_Condi_Inmueble
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_ORT_CONDI_INMUEBLE
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 01/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ort_Condi_Inmueble
    {
        public const String Tabla_Cat_Ort_Condi_Inmueble = "CAT_ORT_CONDI_INMUEBLE";
        public const String Campo_Condicion_Inmueble_ID = "CONDICION_INMUEBLE_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Ort_Funcionamiento
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_ORT_FUNCIONAMIENTO
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 01/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ort_Funcionamiento
    {
        public const String Tabla_Cat_Ort_Funcionamiento = "CAT_ORT_FUNCIONAMIENTO";
        public const String Campo_Funcionamiento_ID = "FUNCIONAMIENTO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : CAT_ORT_TIPO_SUPERVISION
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_ORT_TIPO_SUPERVISION
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 01/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ort_Tipo_Supervision
    {
        public const String Tabla_Cat_Ort_Tipo_Supervision = "CAT_ORT_TIPO_SUPERVISION";
        public const String Campo_Tipo_Supervision_ID = "TIPO_SUPERVISION_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : CAT_ORT_USO_ACTUAL
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_ORT_USO_ACTUAL
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 01/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ort_Uso_Actual
    {
        public const String Tabla_Cat_Ort_Uso_Actual = "CAT_ORT_USO_ACTUAL";
        public const String Campo_Uso_Actual_ID = "USO_ACTUAL_ID";
        public const String Campo_Descripcion = "DESCRIPCION";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : CAT_ORT_ZONA
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_ORT_ZONA
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 01/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ort_Zona
    {
        public const String Tabla_Cat_Ort_Zona = "CAT_ORT_ZONA";
        public const String Campo_Zona_ID = "ZONA_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Responsable_Zona = "RESPONSABLE_ZONA";
        public const String Campo_Empleado_ID = "EMPLEADO_ID";
        public const String Campo_Area = "AREA_ID";
        public const String Campo_Nombre_Area = "AREA";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Ort_Parametros
    ///DESCRIPCIÓN          : Clase con los datos de la tabla CAT_ORT_PARAMETROS
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Roberto González Oseguera
    ///FECHA CREÓ:          : 17-jul-12 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ort_Parametros
    {
        public const String Tabla_Cat_Ort_Parametros = "CAT_ORT_PARAMETROS";
        public const String Campo_Dependencia_Id_Ordenamiento = "DEPENDENCIA_ID_ORDENAMIENTO";
        public const String Campo_Dependencia_Id_Ambiental = "DEPENDENCIA_ID_AMBIENTAL";
        public const String Campo_Dependencia_Id_Urbanistico = "DEPENDENCIA_ID_URBANISTICO";
        public const String Campo_Dependencia_Id_Inmobiliario = "DEPENDENCIA_ID_INMOBILIARIO";
        public const String Campo_Dependencia_Id_Catastro = "DEPENDENCIA_ID_CATASTRO";
        public const String Campo_Rol_Director_Ordenamiento = "ROL_DIRECTOR_ORDENAMIENTO";
        public const String Campo_Rol_Director_Ambiental = "ROL_DIRECTOR_AMBIENTAL";
        public const String Campo_Rol_Director_Fraccionamientos = "ROL_DIRECTOR_FRACCIONAMIENTOS";
        public const String Campo_Rol_Director_Urbana = "ROL_DIRECTOR_URBANA";
        public const String Campo_Rol_Inspectores = "ROL_INSPECTORES_ORDENAMINETO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO"; 
        public const String Campo_Costo_Bitacora = "COSTO_BITACORA";
        public const String Campo_Costo_Perito = "COSTO_PERITO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : OPE_ORT_FORMATO_ADMON_URBANA
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla OPE_ORT_FORMATO_ADMON_URBANA
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 05/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Ort_Formato_Admon_Urbana
    {
        public const String Tabla_Ope_Ort_Formato_Admon_Urbana = "OPE_ORT_FORMATO_ADMON_URBANA";
        //  para los id principales
        public const String Campo_Administracion_Urbana_ID = "ADMINISTRACION_URBANA_ID";
        public const String Campo_Tramite_ID = "TRAMITE_ID";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Subproceso_ID = "SUBPROCESO_ID";
        public const String Campo_Inspector_ID = "INSPECTOR_ID";
        public const String Campo_Cuenta_Predial = "CUENTA_PREDIAL";
        public const String Campo_Estatus = "ESTATUS";
        //  para los datos de la area 
        public const String Campo_Area_Inspeccion = "AREA_INSPECCION";
        public const String Campo_Area_Calle = "AREA_CALLE";
        public const String Campo_Area_Colonia = "AREA_COLONIA";
        public const String Campo_Area_Numero_Fisico = "AREA_NO_FISICO";
        public const String Campo_Area_Manzana = "AREA_MANZANA";
        public const String Campo_Area_Lote = "AREA_LOTE";
        public const String Campo_Area_Zona = "AREA_ZONA";
        public const String Campo_Area_Uso_Solicitado = "AREA_USO_SOLICITADO";
        //  para los datos del tipo de supervision
        public const String Campo_Tipo_Supervision_ID = "TIPO_SUPERVISION_ID";
        //  para los datos de las condiciones del inmueble
        public const String Campo_Condiciones_Inmueble = "CONDICION_INMUEBLE_ID";
        //  para el avance de la obra
        public const String Campo_Avance_Obra_ID = "AVANCE_OBRA_ID";
        public const String Campo_Avance_Bardeo_Aproximado = "AVANCE_BARDEO_APROX";
        public const String Campo_Avance_Niveles_Actuales = "AVANCE_NIVELES_ACTUALES";
        public const String Campo_Avances_Niveles_Construccion = "AVANCES_NIVELES_CONSTRUCCION";
        public const String Campo_Avance_Proyecto_Acorde_Solicitado = "AVANCE_PROYECTO_ACORDE";
        //  para las vias publicas y donaciones
        public const String Campo_Via_Publica_Invasion_Material = "VIA_PUBLICA_INVASION_MATERIAL";
        public const String Campo_Via_Publica_Invasion_Donacion = "VIA_PUBLICA_INVASION_DONACION";
        public const String Campo_Via_Publica_Sobre_Marquesina = "VIA_PUBLICA_SOBRE_MARQUESINA";
        public const String Campo_Via_Publica_Paramento = "VIA_PUBLICA_PARAMENTO";
        public const String Campo_Area_Via_ID = "AREA_VIA_ID";
        public const String Campo_Area_Via_Especificar_Restriccion = "AREA_VIA_ESPECIF_RESTRICCION";
        //  para las datos referentes a las inspecciones 
        public const String Campo_Inspeccion_Notificacion = "INSPECCION_NOTIFICADO";
        public const String Campo_Inspeccion_Notificacion_Folio = "INSPECCION_FOLIO_NOTIFICADO";
        public const String Campo_Inspeccion_Acta = "INSPECCION_ACTA";
        public const String Campo_Inspeccion_Acta_Folio = "INSPECCION_FOLIO_ACTA";
        public const String Campo_Inspeccion_Clausurado = "INSPECCION_CLAUSURADO";
        public const String Campo_Inspeccion_Clausurado_Folio = "INSPECCION_FOLIO_CLAUSURADO";
        public const String Campo_Inspeccion_Multado = "INSPECCION_MULTADO";
        public const String Campo_Inspeccion_Multado_Folio = "INSPECCION_FOLIO_MULTADO";
        //  para el uso actual
        public const String Campo_Uso_Actual_ID = "USO_ACTUAL_ID";
        public const String Campo_Uso_Acorde_Solicitado = "USO_ACORDE_SOLICITADO";
        public const String Campo_Uso_Especificar_Tipo_Uso = "ESPECIFICAR_TIPO_USO";
        //  para el uso predominante de la zona        
        public const String Campo_Uso_Predominante_Colindantes = "USOS_PRE_COLINDANTES";
        public const String Campo_Uso_Predominante_Frente_Inmueble = "USOS_PRE_FRENTE_INMUEBLE";
        public const String Campo_Uso_Predominante_Impacto_Considarar = "USOS_PRE_IMPACTO_CONSIDERAR";
        //  para el uso del funcionamiento 
        public const String Campo_Funcionamiento_Actividad = "FUNCIONAMIENTO_ACTIVIDAD";
        public const String Campo_Funcionamiento_Metros = "FUNCIONAMIENTO_METROS";
        public const String Campo_Funcionamiento_Maquinaria = "FUNCIONAMIENTO_MAQUINARIA";
        public const String Campo_Funcionamiento_ID = "FUNCIONAMIENTO_ID";
        public const String Campo_Funcionamiento_Numero_Personal = "FUNCIONAMIENTO_NO_PERSONAL";
        public const String Campo_Funcionamiento_Numero_Clientes = "FUNCIONAMIENTO_NO_CLIENTES";
        //  para los campos de anuncios
        public const String Campo_Anuncio_1 = "ANUNCIO_TIPO_1";
        public const String Campo_Anuncio_Dimensiones_1 = "ANUNCIO_DIMENSIONES_1";
        public const String Campo_Anuncio_2 = "ANUNCIO_TIPO_2";
        public const String Campo_Anuncio_Dimensiones_2 = "ANUNCIO_DIMENSIONES_2";
        public const String Campo_Anuncio_3 = "ANUNCIO_TIPO_3";
        public const String Campo_Anuncio_Dimensiones_3 = "ANUNCIO_DIMENSIONES_3";
        public const String Campo_Anuncio_4 = "ANUNCIO_TIPO_4";
        public const String Campo_Anuncio_Dimensiones_4 = "ANUNCIO_DIMENSIONES_4";
        //  para los servicios
        public const String Campo_Servicio_Cuenta = "SERVICIO_CUENTA";
        public const String Campo_Servicio_WC = "SERVICIO_WC";
        public const String Campo_Servicio_Lavabo = "SERVICIO_LAVADO";
        public const String Campo_Servicio_Letrina = "SERVICIO_LETRINA";
        public const String Campo_Servicio_Mixto = "SERVICIO_MIXTO";
        public const String Campo_Servicio_Numero_Sanitarios_Hombres = "SERVICIO_NO_SANITARIO_HOMBRE";
        public const String Campo_Servicio_Numero_Sanitarios_Mujeres = "SERVICIO_NO_SANITARIO_MUJER";
        public const String Campo_Servicio_Agua_Potable = "SERVICIO_AGUA_POTABLE";
        public const String Campo_Servicio_Agua_Abast_Particular = "SERVICIO_AGUA_ABASTC_PARTI";
        public const String Campo_Servicio_Agua_Abast_Japami = "SERVICIO_AGUA_ABASTC_JAPAMI";
        public const String Campo_Servicio_Drenaje = "SERVICIO_DRENAJE";
        public const String Campo_Servicio_Fosa_Septica = "SERVICIO_FOSA_SEPTICA";
        public const String Campo_Servicio_Estacionamiento = "SERVICIO_ESTACIONAMIENTO";
        public const String Campo_Servicio_Estacionamiento_Propio = "SERVICIO_ESTAC_PROPIO";
        public const String Campo_Servicio_Estacionamiento_Rentado = "SERVICIO_ESTAC_RENTADO";
        public const String Campo_Servicio_Estacionamiento_Numero_Cajones = "SERVICIO_ESTAC_NO_CAJONES";
        public const String Campo_Servicio_Estacionamiento_Area_Descarga = "SERVICIO_ESTAC_AREA_DESCARGA";
        public const String Campo_Servicio_Estacionamiento_Domicilio = "SERVICIO_ESTAC_DOMICILIO";
        //  para los materiales empleados
        public const String Campo_Material_Empleado_Muros = "MATERIAL_EMPLEADO_MUROS";
        public const String Campo_Material_Empleado_Techo = "MATERIAL_EMPLEADO_TECHO";
        //  para las medidas de seguridad
        public const String Campo_Seguridad_Medidas = "SEGURIDAD_MEDIDAS";
        public const String Campo_Seguridad_Equipo = "SEGURIDAD_EQUIPOS";
        public const String Campo_Seguridad_Material_Flamable = "SEGURIDAD_MATERIAL_FLAMABLE";
        public const String Campo_Seguridad_Especificar = "SEGURIDAD_ESPECIFICAR";
        //  para la poda de arboles
        public const String Campo_Poda_Altura = "PODA_ALTURA";
        public const String Campo_Poda_Diametro_Tronco = "PODA_DIAMETRO_TRONCO";
        public const String Campo_Poda_Diametro_Fronda = "PODA_DIAMETRO_FRONDA";
        public const String Campo_Poda_Estado = "PODA_ESTADO";
        //  para los campos generales
        public const String Campo_Generales_Recepcion_Inspector = "GENERALES_RECEPC_INSPECTOR";
        public const String Campo_Generales_Fecha_Realizada_Campo = "GENERALES_FECHA_CAMPO";
        public const String Campo_Generales_Recepcion_Coordinacion = "GENERALES_RECEPC_COORD";
        public const String Campo_Generales_Observaciones_Del_Insepctor = "GENERALES_OBSERVACION_INSPEC";
        public const String Campo_Generales_Observaciones_Para_Insepctor = "GENERALES_OBSERVACION_PARA";
        //  para los campos de auditoria
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        //  para los datos del manifiesto de impacto ambiental
        public const String Campo_Impacto_Afectaciones = "IMPACTO_AFECTACIONES";
        public const String Campo_Impacto_Colindancias = "IMPACTO_COLINDANCIAS";
        public const String Campo_Impacto_Superficie = "IMPACTO_SUPERFICIE";
        public const String Campo_Impacto_Tipo_Proyecto = "IMPACTO_TIPO_PROYECTO";
        //  para los datos de la licencia ambiental de funcionamiento
        public const String Campo_Licencia_Tipo_Equipo_Emisor = "LICENCIA_TIPO_EQUIPO";
        public const String Campo_Licencia_Tipo_Emision = "LICENCIA_TIPO_EMISION";
        public const String Campo_Licencia_Tipo_Horario_Funcionamiento = "LICENCIA_HORARIO_FUNCIONAM";
        public const String Campo_Licencia_Tipo_Conbustible = "LICENCIA_TIPO_COMBUSTIBLE";
        public const String Campo_Licencia_Gasto_Combustible = "LICENCIA_GASTO_COMBUSTIBLE";
        public const String Campo_Licencia_Almacenaje = "LICENCIA_ALMACENAJE";
        public const String Campo_Licencia_Cantidad_Combustible = "LICENCIA_CANTIDAD_COMBUSTIBLE";
        //  para los datos del banco de materiales
        public const String Campo_Materiales_Permiso_Ecologia = "MATERIAL_PERMISO_ECOLOGIA";
        public const String Campo_Materiales_Permiso_Suelo = "MATERIAL_PERMISO_SUELO";
        public const String Campo_Materiales_Superficie_Total = "MATERIAL_SUPERFICIE_TOTAL";
        public const String Campo_Materiales_Profundidad = "MATERIAL_PROFUNDIDAD";
        public const String Campo_Materiales_Inclinacion = "MATERIAL_INCLINACION";
        public const String Campo_Materiales_Flora = "MATERIAL_FLORA";
        public const String Campo_Materiales_Acceso_Vehiculoas = "MATERIAL_ACCESO_VEHICULOS";
        public const String Campo_Materiales_Petreo = "MATERIAL_PETREO";
        public const String Campo_Materiales_Arboles_Especie = "MATERIAL_ARBOLES_ESPECIE";
        public const String Campo_Materiales_Tipo_Poda = "MATERIALES_TIPO_PODA";
        public const String Campo_Materiales_Cantidad_Poda = "MATERIALES_CANTIDAD_PODA";
        public const String Campo_Materiales_Tipo_Tala = "MATERIALES_TIPO_TALA";
        public const String Campo_Materiales_Cantidad_Tala = "MATERIALES_CANTIDAD_TALA";
        public const String Campo_Materiales_Tipo_Trasplante = "MATERIALES_TIPO_TRASPLANTE";
        public const String Campo_Materiales_Cantidad_Trasplante = "MATERIALES_CANTIDAD_TRASPLAN";

        //  para los datos de la autorizacion de aprovechamiento ambiental
        public const String Campo_Autoriza_Suelos = "AUTORIZA_SUELOS";
        public const String Campo_Autoriza_Area_Residuos = "AUTORIZA_AREA_RESIDUOS";
        public const String Campo_Autoriza_Separacion = "AUTORIZA_SEPARACION";
        public const String Campo_Autoriza_Metodo_Separacion = "AUTORIZA_METODO_SEPARACION";
        public const String Campo_Autoriza_Servicio_Recoloccion = "AUTORIZA_SERVICIO_RECOLEC";
        public const String Campo_Autoriza_Revuelven_Solidos_Liquidos = "AUTORIZA_REVUELVEN_SOLD_LIQU";
        public const String Campo_Autoriza_Tipo_Contenedor = "AUTORIZA_TIPO_CONTENEDOR";
        public const String Campo_Autoriza_Tipo_Ruido = "AUTORIZA_TIPO_RUIDO";
        public const String Campo_Autoriza_Nivel_Ruido = "AUTORIZA_NIVEL_RUIDO";
        public const String Campo_Autoriza_Horario_Labores = "AUTORIZA_HORARIO_LABORES";
        public const String Campo_Autoriza_Lunes = "AUTORIZA_LUNES";
        public const String Campo_Autoriza_Martes = "AUTORIZA_MARTES";
        public const String Campo_Autoriza_Miercoles = "AUTORIZA_MIERCOLES";
        public const String Campo_Autoriza_Jueves = "AUTORIZA_JUEVES";
        public const String Campo_Autoriza_Viernes = "AUTORIZA_VIERNES";
        public const String Campo_Autoriza_Sabado = "AUTORIZA_SABADO";
        public const String Campo_Autoriza_Domingo = "AUTORIZA_DOMINGO";
        public const String Campo_Autoriza_Emisiones = "AUTORIZA_EMISIONES";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Ort_Inspectores
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_ORT_INSPECTORES
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 07/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ort_Inspectores
    {
        public const String Tabla_Cat_Ort_Inspectores = "CAT_ORT_INSPECTORES";
        public const String Campo_Inspector_ID = "INSPECTOR_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
        public const String Campo_Cedula_Profesional= "CEDULA_PROFESIONAL";
        public const String Campo_Titulo = "TITULO";
        public const String Campo_Afiliado = "AFILIADO";
        public const String Campo_Calle_Oficina = "CALLE_OFICINA";
        public const String Campo_Colonia_Oficina = "COLONIA_OFICINA";
        public const String Campo_Numero_Oficina = "NUMERO_OFICINA";
        public const String Campo_Telefono_Oficina = "TELEFONO";
        public const String Campo_Email = "EMAIL";
        public const String Campo_Calle_Particular = "CALLE_PARTICULAR";
        public const String Campo_Colonia_Particular = "COLONIA_PARTICULAR";
        public const String Campo_Numero_Particular= "NUMERO_PARTICULAR";
        public const String Campo_Codigo_Postal = "CODIGO_POSTAL";
        public const String Campo_Telefono_Particular = "TELEFONO_PARTICULAR";
        public const String Campo_Especialidad = "ESPECIALIDAD";
        public const String Campo_Documento_Titulo = "DOCUMENTO_TITULO";
        public const String Campo_Documento_Cedula = "DOCUMENTO_CEDULA";
        public const String Campo_Documento_Curriculum = "DOCUMENTO_CURRICULUM";
        public const String Campo_Documento_Constancia = "DOCUMENTO_CONSTANCIA";
        public const String Campo_Documento_Refrendo = "DOCUMENTO_REFRENDO";
        public const String Campo_Documento_Especialidad = "DOCUMENTO_ESPECIALIDAD";       
        public const String Campo_Tipo_Perito = "TIPO_PERITO";
        public const String Campo_Telefono_Celular = "TELEFONO_CELULAR";
        public const String Campo_Comentario = "COMENTARIO";
        public const String Campo_Documento_Conformidad = "DOCUMENTO_CONFORMIDAD";
        public const String Campo_Documento_Curso = "DOCUMENTO_CURSO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cat_Ort_Tipo_Residuos
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla CAT_ORT_TIPO_RESIDUOS
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 07/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Cat_Ort_Tipo_Residuos
    {
        public const String Tabla_Cat_Ort_Tipo_Residuos = "CAT_ORT_TIPO_RESIDUOS";
        public const String Campo_Residuo_ID= "TIPO_RESIDUO_ID";
        public const String Campo_Nombre = "NOMBRE";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Ort_Det_Residuos 
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla OPE_ORT_DET_RESIDUOS
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 07/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Ort_Det_Residuos
    {
        public const String Tabla_Ope_Ort_Det_Residuos = "OPE_ORT_DET_RESIDUOS";
        public const String Campo_Detalle_Residuos_ID = "DETALLE_RESIDUOS_ID";
        public const String Campo_Ficha_Inspeccion = "FICHA_INSPECCION_ID";
        public const String Campo_Tipo_Residuo_ID = "TIPO_RESIDUO_ID";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Ort_Formato_Ficha_Inspec 
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla OPE_ORT_FORMATO_FICHA_INSPEC
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 07/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Ort_Formato_Ficha_Inspec
    {
        public const String Tabla_Ope_Ort_Formato_Ficha_Inspec = "OPE_ORT_FORMATO_FICHA_INSPEC";        
        //  para los id
        public const String Campo_Ficha_Inspeccion_ID = "FICHA_INSPECCION_ID"; 
        public const String Campo_Tramite_ID = "TRAMITE_ID";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Subproceso_ID = "SUBPROCESO_ID";
        public const String Campo_Inspector_ID = "INSPECTOR_ID";
        public const String Campo_Fecha_Entrega = "FECHA_ENTREGA";
        public const String Campo_Tiempo_Respuesta = "TIEMPO_RESPUESTA";
        public const String Campo_Fecha_Inspeccion = "FECHA_INSPECCION";
        //  para los datos del inmueble
        public const String Campo_Inmueble_Nombre = "INMUEBLE_NOMBRE";
        public const String Campo_Inmueble_Telefono = "INMUEBLE_TELEFONO";
        public const String Campo_Inmueble_Colonia = "INMUEBLE_COLONIA";
        public const String Campo_Inmueble_Calle = "INMUEBLE_CALLE";
        public const String Campo_Inmueble_Numero = "INMUEBLE_NUMERO";
        //  para los datos del solicitante
        public const String Campo_Solicitante_Nombre = "SOLICITANTE_NOMBRE";
        public const String Campo_Solicitante_Telefono = "SOLICITANTE_TELEFONO";
        public const String Campo_Solicitante_Colonia = "SOLICITANTE_COLONIA";
        public const String Campo_Solicitante_Calle = "SOLICITANTE_CALLE";
        public const String Campo_Solicitante_Numero = "SOLICITANTE_NUMERO";
        //  para los datos del manifiesto de impacto ambiental
        public const String Campo_Impacto_Afectaciones = "IMPACTO_AFECTACIONES";
        public const String Campo_Impacto_Colindancias = "IMPACTO_COLINDANCIAS";
        public const String Campo_Impacto_Superficie = "IMPACTO_SUPERFICIE";
        public const String Campo_Impacto_Tipo_Proyecto = "IMPACTO_TIPO_PROYECTO";
        //  para los datos de la licencia ambiental de funcionamiento
        public const String Campo_Licencia_Tipo_Equipo_Emisor = "LICENCIA_TIPO_EQUIPO";
        public const String Campo_Licencia_Tipo_Emision = "LICENCIA_TIPO_EMISION";
        public const String Campo_Licencia_Tipo_Horario_Funcionamiento = "LICENCIA_HORARIO_FUNCIONAM";
        public const String Campo_Licencia_Tipo_Conbustible = "LICENCIA_TIPO_COMBUSTIBLE";
        public const String Campo_Licencia_Gasto_Combustible = "LICENCIA_GASTO_COMBUSTIBLE";
        //  para los datos de la autorizacion de poda
        public const String Campo_Poda_Altura = "PODA_ALTURA";
        public const String Campo_Poda_Diametro_Tronco = "PODA_DIAMETRO_TRONCO";
        public const String Campo_Poda_Diametro_Fronda= "PODA_DIAMETRO_FRONDA";
        public const String Campo_Poda_Especie = "PODA_ESPECIE";
        public const String Campo_Poda_Condiciones = "PODA_CONDICIONES";
        //  para los datos del banco de materiales
        public const String Campo_Materiales_Permiso_Ecologia = "MATERIAL_PERMISO_ECOLOGIA";
        public const String Campo_Materiales_Permiso_Suelo = "MATERIAL_PERMISO_SUELO";
        public const String Campo_Materiales_Superficie_Total = "MATERIAL_SUPERFICIE_TOTAL";
        public const String Campo_Materiales_Profundidad = "MATERIAL_PROFUNDIDAD";
        public const String Campo_Materiales_Inclinacion= "MATERIAL_INCLINACION";
        public const String Campo_Materiales_Flora = "MATERIAL_FLORA";
        public const String Campo_Materiales_Acceso_Vehiculoas = "MATERIAL_ACCESO_VEHICULOS";
        public const String Campo_Materiales_Petreo = "MATERIAL_PETREO";
        //  para los datos de la autorizacion de aprovechamiento ambiental
        public const String Campo_Autoriza_Suelos = "AUTORIZA_SUELOS";
        public const String Campo_Autoriza_Area_Residuos = "AUTORIZA_AREA_RESIDUOS";
        public const String Campo_Autoriza_Separacion = "AUTORIZA_SEPARACION";
        public const String Campo_Autoriza_Metodo_Separacion = "AUTORIZA_METODO_SEPARACION";
        public const String Campo_Autoriza_Servicio_Recoloccion = "AUTORIZA_SERVICIO_RECOLEC";
        public const String Campo_Autoriza_Revuelven_Solidos_Liquidos = "AUTORIZA_REVUELVEN_SOLD_LIQU";
        public const String Campo_Autoriza_Tipo_Contenedor = "AUTORIZA_TIPO_CONTENEDOR";
        public const String Campo_Autoriza_Drenaje = "AUTORIZA_DRENAJE";
        public const String Campo_Autoriza_Tipo_Drenaje = "AUTORIZA_TIPO_DRENAJE";
        public const String Campo_Autoriza_Tipo_Ruido= "AUTORIZA_TIPO_RUIDO";
        public const String Campo_Autoriza_Nivel_Ruido = "AUTORIZA_NIVEL_RUIDO";
        public const String Campo_Autoriza_Horario_Labores = "AUTORIZA_HORARIO_LABORES";
        public const String Campo_Autoriza_Lunes= "AUTORIZA_LUNES";
        public const String Campo_Autoriza_Martes = "AUTORIZA_MARTES";
        public const String Campo_Autoriza_Miercoles = "AUTORIZA_MIERCOLES";
        public const String Campo_Autoriza_Jueves = "AUTORIZA_JUEVES";
        public const String Campo_Autoriza_Viernes = "AUTORIZA_VIERNES";
        public const String Campo_Autoriza_Colindancia = "AUTORIZA_COLINDANCIA";
        public const String Campo_Autoriza_Emisiones = "AUTORIZA_EMISIONES";
        //  para los datos de la autorizacion de aprovechamiento ambiental
        public const String Campo_Observaciones_Del_Insepector = "OBSERVACION_DEL_INSPEC";
        public const String Campo_Observaciones_Para_Insepector = "OBSERVACION_PARA_INSPEC";        
        //  para los datos de auditoria
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Ort_Ficha_Revision
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Ope_Ort_Ficha_Revision
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 07/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Ort_Ficha_Revision
    {
        public const String Tabla_Ope_Ort_Ficha_Revision = "OPE_ORT_FICHA_REVISION";
        public const String Campo_Solicitud_Interna_ID = "SOLICITUD_INTERNA_ID";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Zona_ID = "ZONA_ID";
        public const String Campo_Area_ID = "AREA_ID";
        public const String Campo_Observacion = "OBSERVACION";
        public const String Campo_Respuesta = "RESPUESTA";
        public const String Campo_Fecha_Solicitud = "FECHA_SOLICITUD";
        public const String Campo_Fecha_Respuesta = "FECHA_RESPUESTA";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Ope_Ort_Ficha_Revision_Depto
    ///DESCRIPCIÓN          : Clase contiene los datos de la tabla Ope_Ort_Ficha_Revision_Depto
    ///PARAMETROS           :
    ///USUARIO CREÓ:        : Hugo Enrique Ramírez Aguilera
    ///FECHA CREÓ:          : 07/Junio/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    public class Ope_Ort_Ficha_Revision_Depto
    {
        public const String Tabla_Ope_Ort_Ficha_Revision_Depto = "OPE_ORT_FICHA_REVISION_DEPTO";
        public const String Campo_Ficha_Revision_ID = "FICHA_REVISION_ID";
        public const String Campo_Tipo_Tramite = "TIPO_TRAMITE";
        public const String Campo_Nombre_Propietario = "NOMBRE_PROPIETARIO";
        public const String Campo_Calle_Ubicacion = "CALLE_UBICACION";
        public const String Campo_Colonia_Ubicacion = "COLONIA_UBICACION";
        public const String Campo_Codigo_Postal = "CODIGO_POSTAL";
        public const String Campo_Ciudad_Ubicacion = "CIUDAD_UBICACION";
        public const String Campo_Estado_Ubicacion = "ESTADO_UBICACION";
        public const String Campo_Documentos_Propiedad = "DOCUMENTOS_PROPIEDAD";
        public const String Campo_Observacion_Juridica = "OBSERVACION_JURIDICA";
        public const String Campo_Observacion_Tecnica = "OBSERVACION_TECNICA";
        public const String Campo_Avance_Obra = "AVANCE_OBRA";
        public const String Campo_Documentos_Dictamen = "DOCUMENTOS_DICTAMEN";
        public const String Campo_Cumplimiento_Norma = "CUMPLIMIENTO_NORMAS";
        public const String Campo_Ubicacion_Construccion = "UBICACION_CONSTRUCCION";
        public const String Campo_Tramite = "TRAMITE";
        public const String Campo_Solicitud_ID = "SOLICITUD_ID";
        public const String Campo_Inicio_Permiso = "INICIO_PERMISO";
        public const String Campo_Fin_Permiso = "FIN_PERMISO";
        public const String Campo_Perito = "PERITO";
        public const String Campo_Usuario_Creo = "USUARIO_CREO";
        public const String Campo_Fecha_Creo = "FECHA_CREO";
        public const String Campo_Usuario_Modifico = "USUARIO_MODIFICO";
        public const String Campo_Fecha_Modifico = "FECHA_MODIFICO";
    }

    #endregion
}
