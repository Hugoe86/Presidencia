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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;
using Presidencia.Orden_Territorial_Administracion_Urbana.Negocio;

namespace Presidencia.Orden_Territorial_Administracion_Urbana.Datos
{
    public class Cls_Cat_Ort_Administracion_Urbana_Datos
    {
        #region Alta

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Formato
            ///DESCRIPCIÓN          : guardara el formato de administracion urbana
            ///PARAMETROS           1 Negocio: conexion con la capa de negocios
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 05/Junio/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
        internal static void Alta_Formato(Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio)
        {
            Cls_Ope_Bandeja_Tramites_Negocio Neg_Actualizar_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            String Cedula_Visita_Unificada = "";
            String Detalle_Residuo_ID = "";
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;

            try
            {
                if (Negocio.P_Cmmd != null)
                {
                    Comando = Negocio.P_Cmmd;
                }
                else
                {
                    Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Conexion;
                    Comando.Transaction = Transaccion;
                }

                Cedula_Visita_Unificada = Consecutivo_ID(ref Comando ,Ope_Ort_Formato_Admon_Urbana.Campo_Administracion_Urbana_ID, Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana, 10);

                Mi_SQL.Append("INSERT INTO " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + "(");
                //  para los datos de los id generales
                Mi_SQL.Append(Ope_Ort_Formato_Admon_Urbana.Campo_Administracion_Urbana_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Tramite_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Subproceso_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Estatus);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspector_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Cuenta_Predial);
                //  para los datos de la area 
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Inspeccion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Calle);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Colonia);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Numero_Fisico);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Manzana);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Lote);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Zona);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Uso_Solicitado);
                //  para los datos del tipo de supervision
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Tipo_Supervision_ID);
                //  para los datos de las condiciones del inmueble
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Condiciones_Inmueble);
                //  para el avance de la obra
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Obra_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Bardeo_Aproximado);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Niveles_Actuales);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Avances_Niveles_Construccion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Proyecto_Acorde_Solicitado);
                //  para las vias publicas y donaciones
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Invasion_Donacion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Invasion_Material);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Paramento);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Sobre_Marquesina);
                //Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Via_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Via_Especificar_Restriccion);
                //  para las datos referentes a las inspecciones 
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Notificacion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Notificacion_Folio);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Acta);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Acta_Folio);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Clausurado);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Clausurado_Folio);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Multado);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Multado_Folio);
                //  para el uso actual
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Actual_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Acorde_Solicitado);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Especificar_Tipo_Uso);
                //  para el uso predominante de la zona   
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Predominante_Colindantes);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Predominante_Frente_Inmueble);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Predominante_Impacto_Considarar);
                //  para el uso del funcionamiento 
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Actividad);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Metros);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Maquinaria);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_ID);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Numero_Personal);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Numero_Clientes);
                //  para los campos de anuncios
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_1);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_1);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_2);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_2);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_3);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_3);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_4);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_4);
                //  para los servicios
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Cuenta);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_WC);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Lavabo);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Letrina);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Mixto);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Numero_Sanitarios_Hombres);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Numero_Sanitarios_Mujeres);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Potable);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Abast_Particular);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Abast_Japami);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Drenaje);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Fosa_Septica);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Propio);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Rentado);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Numero_Cajones);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Area_Descarga);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Domicilio);
                //  para los materiales empleados
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Material_Empleado_Muros);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Material_Empleado_Techo);
                //  para las medidas de seguridad
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Medidas);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Equipo);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Material_Flamable);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Especificar);
                //  para la poda de arboles
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Altura);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Diametro_Tronco);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Diametro_Fronda);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Estado);
                //  para los campos generales
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Recepcion_Inspector);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Fecha_Realizada_Campo);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Recepcion_Coordinacion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Observaciones_Para_Insepctor);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Observaciones_Del_Insepctor);
                //  para los campos de auditoria
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Usuario_Creo);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Fecha_Creo);


                //  para los datos del manifiesto de impacto ambiental
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Afectaciones);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Colindancias);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Superficie);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Tipo_Proyecto);

                //  para los datos de la licencia ambiental de funcionamiento
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Equipo_Emisor);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Emision);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Horario_Funcionamiento);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Conbustible);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Gasto_Combustible);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Almacenaje);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Cantidad_Combustible);

                //  para los datos del banco de materiales                
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Permiso_Ecologia);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Permiso_Suelo);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Superficie_Total);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Profundidad);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Inclinacion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Flora);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Acceso_Vehiculoas);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Petreo);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Arboles_Especie);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Tipo_Poda);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Cantidad_Poda);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Tipo_Tala);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Cantidad_Tala);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Tipo_Trasplante);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Cantidad_Trasplante);

                //  para los datos de la autorizacion de aprovechamiento ambiental              
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Suelos);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Area_Residuos);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Separacion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Metodo_Separacion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Servicio_Recoloccion);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Revuelven_Solidos_Liquidos);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Tipo_Contenedor);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Tipo_Ruido);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Nivel_Ruido);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Horario_Labores);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Lunes);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Martes);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Miercoles);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Jueves);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Viernes);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Sabado);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Domingo);
                Mi_SQL.Append(", " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Emisiones);


                Mi_SQL.Append(") VALUES( ");

                //  para los datos de los id generales
                Mi_SQL.Append("  '" + Cedula_Visita_Unificada + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Tramite_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Solicitud_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Subproceso_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Estatus + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Inspector_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Cuenta_Predial + "' ");
                //  para los datos de la area 
                Mi_SQL.Append(", '" + Negocio.P_Area_Inspeccion + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Area_Calle + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Area_Colonia + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Area_Numero_Fisico + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Area_Manzana + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Area_Lote + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Area_Zona + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Area_Uso_Solicitado + "' ");
                //  para los datos del tipo de supervision
                Mi_SQL.Append(", '" + Negocio.P_Tipo_Supervision_ID + "' ");
                //  para los datos de las condiciones del inmueble
                Mi_SQL.Append(", '" + Negocio.P_Condiciones_Inmueble_ID + "' ");
                //  para el avance de la obra
                Mi_SQL.Append(", '" + Negocio.P_Avance_Obra_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Avance_Bardeo_Aproximado + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Avance_Niveles_Actuales + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Avance_Niveles_Construccion + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Avance_Proyecto_Acorde + "' ");
                //  para las vias publicas y donaciones
                Mi_SQL.Append(", '" + Negocio.P_VIA_PUBLICA_INVASION_DONACION + "' ");
                Mi_SQL.Append(", '" + Negocio.P_VIA_PUBLICA_INVASION_MATERIAL + "' ");
                Mi_SQL.Append(", '" + Negocio.P_VIA_PUBLICA_PARAMENTO + "' ");
                Mi_SQL.Append(", '" + Negocio.P_VIA_PUBLICA_SOBRE_MARQUESINA + "' ");
                //Mi_SQL.Append(", '" + Negocio.P_Area_Via_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Area_Via_Especificar_Restricciones + "' ");
                //  para las datos referentes a las inspecciones 
                Mi_SQL.Append(", '" + Negocio.P_Inspeccion_Notificacion + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Inspeccion_Notificacion_Folio + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Inspeccion_Acta + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Inspeccion_Acta_Folio + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Inspeccion_Clausurado + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Inspeccion_Clausurado_Folio + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Inspeccion_Multado + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Inspeccion_Multado_Folio + "' ");
                //  para el uso actual
                Mi_SQL.Append(", '" + Negocio.P_Uso_Actual_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Uso_Actual_Acorde_Solicitado + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Uso_Actual_Especificar_Tipo_Uso + "' ");
                //  para el uso predominante de la zona  
                Mi_SQL.Append(", '" + Negocio.P_Uso_Predominante_Colindantes + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Uso_Predominante_Frente_Inmueble + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Uso_Predominante_Impacto + "' ");
                //  para el uso del funcionamiento
                Mi_SQL.Append(", '" + Negocio.P_Funcionamiento_Actividad + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Funcionamiento_Metros_Cuadrados + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Funcionamiento_Maquinaria + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Funcionamiento_ID + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Funcionamiento_No_Personas + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Funcionamiento_No_Clientes + "' ");
                //  para los campos de anuncios 
                Mi_SQL.Append(", '" + Negocio.P_Anuncio_1 + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Anuncio_1_Dimensiones + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Anuncio_2 + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Anuncio_2_Dimensiones + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Anuncio_3 + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Anuncio_3_Dimensiones + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Anuncio_4 + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Anuncio_4_Dimensiones + "' ");
                //  para los servicios
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Cuenta_Sanitarios + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_WC + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Lavabo + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Letrina + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Mixto + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Numero_Sanitarios_Hombres + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Numero_Sanitarios_Mujeres + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Agua_Potable + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Agua_Abastecimiento_Particular + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Agua_Abastecimiento_Japami + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Drenaje + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Fosa_Septica + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Estacionamiento + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Estacionamiento_Propio + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Estacionamiento_Rentado + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Estacionamiento_Numero_Cajones + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Estacionamiento_Area_Descarga + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Servicios_Estacionamiento_Domicilio + "' ");
                //  para los materiales empleados
                Mi_SQL.Append(", '" + Negocio.P_Materiales_Empleado_Muros + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Materiales_Empleado_Techos + "' ");
                //  para las medidas de seguridad
                Mi_SQL.Append(", '" + Negocio.P_Seguridad_Medidas + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Seguridad_Equipo + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Seguridad_Material_Flamable + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Seguridad_Especificar + "' ");
                //  para la poda de arboles
                Mi_SQL.Append(", '" + Negocio.P_Poda_Altura + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Poda_Diametro_Tronco + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Poda_Fronda + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Poda_Estado + "' ");
                //  para los campos generales  
                Mi_SQL.Append(", TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Negocio.P_Generales_Recepcion_Inspector) + "','DD/MM/YY') ");
                Mi_SQL.Append(", TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Negocio.P_Generales_Fecha_Revision_Campo) + "','DD/MM/YY') ");
                Mi_SQL.Append(", TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Negocio.P_Generales_Recepcion_Coordinacion) + "','DD/MM/YY') ");
                Mi_SQL.Append(", '" + Negocio.P_Generales_Observaciones_Para_Inspector + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Generales_Observaciones_Inspector + "' ");
                //  para los campos de auditoria
                Mi_SQL.Append(", '" + Negocio.P_Usuario + "' ");
                Mi_SQL.Append(", SYSDATE");

                //  para los datos del manifiesto de impacto ambiental
                Mi_SQL.Append(", '" + Negocio.P_Impacto_Afectables + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Impacto_Colindancias + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Impacto_Superficie + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Impacto_Tipo_Proyecto + "' ");
                //  para los datos de la licencia ambiental de funcionamiento
                Mi_SQL.Append(", '" + Negocio.P_Licencia_Tipo_Equipo + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Licencia_Tipo_Emision + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Licencia_Horario_Funcionamiento + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Licencia_Tipo_Combustible + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Licencia_Tipo_Gastos_Combustible + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Licencia_Almacenaje + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Licencia_Cantidad_Combustible + "' ");
                //  para los datos del banco de materiales 
                Mi_SQL.Append(", '" + Negocio.P_Material_Permiso_Ecologico + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Permiso_Suelo + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Superficie_Total + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Profundidad + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Inclinacion + "'  ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Flora + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Acceso_Vehiculos + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Petreo + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Especie_Arbol + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Tipo_Poda + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Cantidad_Poda + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Tipo_Tala + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Cantidad_Tala + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Tipo_Trasplante + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Material_Cantidad_Trasplante + "' ");

                //  para los datos de la autorizacion de aprovechamiento ambiental
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Suelos + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Area_Residuos + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Separacion + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Metodo_Separacion + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Servicio_Recoleccion + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Revuelven_Solidos_Liquidos + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Tipo_Contenedor + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Tipo_Ruido + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Nivel_Ruido + "'  ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Horario_Labores + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Lunes + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Martes + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Miercoles + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Jueves + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Viernes + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Sabado + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Domingo + "' ");
                Mi_SQL.Append(", '" + Negocio.P_Autoriza_Emisiones + "'  ");

                Mi_SQL.Append(" ) ");

                Comando.CommandText = Mi_SQL.ToString();
                Comando.ExecuteNonQuery();

                if (Negocio.P_Dt_Residuos != null && Negocio.P_Dt_Residuos.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Negocio.P_Dt_Residuos.Rows)
                    {
                        Mi_SQL = new StringBuilder();
                        Detalle_Residuo_ID = Consecutivo_ID(ref Comando ,Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID, Ope_Ort_Det_Residuos.Tabla_Ope_Ort_Det_Residuos, 10);
                        
                        Mi_SQL.Append("INSERT INTO " + Ope_Ort_Det_Residuos.Tabla_Ope_Ort_Det_Residuos + "(");
                        Mi_SQL.Append(Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID);
                        Mi_SQL.Append(", " + Ope_Ort_Det_Residuos.Campo_Ficha_Inspeccion);
                        Mi_SQL.Append(", " + Ope_Ort_Det_Residuos.Campo_Tipo_Residuo_ID);
                        Mi_SQL.Append(") VALUES( ");
                        Mi_SQL.Append("'" + Detalle_Residuo_ID + "' ");
                        Mi_SQL.Append(", '" + Cedula_Visita_Unificada + "' ");
                        Mi_SQL.Append(", '" + Registro["TIPO_RESIDUO_ID"].ToString() + "' ");
                        Mi_SQL.Append(" ) ");
                        Comando.CommandText = Mi_SQL.ToString();
                        Comando.ExecuteNonQuery();
                    }
                }

                //  Se llenara el comentario y cambiara de estatus
                // consultar el trámite con el folio igual a la referencia del pasivo
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                    + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                    + "," + Ope_Tra_Solicitud.Campo_Estatus
                    + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                    + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ")) = UPPER(TRIM('" + Negocio.P_Solicitud_ID + "'))");
                Comando.CommandText = Mi_SQL.ToString();
                Comando.CommandType = CommandType.Text;
                OracleDataReader Dtr_Datos_Solicitud = Comando.ExecuteReader();

                // si hay datos para leer, agregar pasivo
                if (Dtr_Datos_Solicitud.Read())
                {
                    // establecer parámetros para actualizar solicitud
                    Neg_Actualizar_Solicitud.P_Solicitud_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString().Trim();
                    Neg_Actualizar_Solicitud.P_Estatus = "APROBAR";
                    Neg_Actualizar_Solicitud.P_Subproceso_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString().Trim();
                    Neg_Actualizar_Solicitud.P_Comentarios = "";
                    Neg_Actualizar_Solicitud.P_Usuario = Negocio.P_Usuario;
                    Neg_Actualizar_Solicitud.P_Comando_Oracle = Comando;
                    // llamar método que actualizar la solicitud
                    Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Evaluar_Solicitud();
                }

                Transaccion.Commit();
                Operacion_Completa = true;
            }
            catch (OracleException Ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                }
                String Mensaje = "Error: ";
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "No existe un registro relacionado con esta operacion";
                        break;
                    case "923":
                        Mensaje = "Consulta SQL";
                        break;
                    case "12170":
                        Mensaje = "Conexion con el Servidor";
                        break;
                    default:
                        Mensaje = Ex.Message;
                        break;
                }
                throw new Exception(Mensaje + "[" + Ex.ToString() + "]");
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception(Ex.ToString());
            }
            finally
            {
                Conexion.Close();

            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consecutivo_ID
        ///DESCRIPCIÓN          : consulta para obtener el consecutivo de una tabla
        ///PARAMETROS           1 Campo_Id: campo del que se obtendra el consecutivo
        ///                     2 Tabla: tabla del que se obtendra el consecutivo
        ///                     3 Tamaño: longitud del campo 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static String Consecutivo_ID(ref OracleCommand Comando, String Campo_Id, String Tabla, int Longitud)
        {
            String Consecutivo = "";
            StringBuilder Mi_SQL = new StringBuilder();
            object Obj = null; //Obtiene el ID con la cual se guardo los datos en la base de datos

            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;

            try
            {
                if (Comando == null)
                {
                    // crear transaccion
                    Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Cn.Open();
                    Trans = Cn.BeginTransaction();
                    Cmd.Connection = Cn;
                    Cmd.Transaction = Trans;
                }
                else
                {
                    Cmd = Comando;
                }

                Mi_SQL.Append("SELECT NVL(MAX (" + Campo_Id + "), '" + Convertir_A_Formato_ID(0, Longitud) + "')");
                Mi_SQL.Append(" FROM " + Tabla);
                
                Cmd.CommandText = Mi_SQL.ToString();
                Obj = Cmd.ExecuteScalar();

                if (Convert.IsDBNull(Obj))
                {
                    Consecutivo = Convertir_A_Formato_ID(1, Longitud);
                }
                else
                {
                    Consecutivo = Convertir_A_Formato_ID((Convert.ToInt32(Obj) + 1), Longitud);
                }

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }

            return Consecutivo;
        }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
            ///DESCRIPCIÓN         : Pasa un numero entero a Formato de ID.
            ///PARAMETROS          : 1. Dato_ID. Dato que se desea pasar al Formato de ID.
            ///                      2. Longitud_ID. Longitud que tendra el ID. 
            ///CREO                : Salvador Vázquez Camacho
            ///FECHA_CREO          : 30/Julio/2010
            ///MODIFICO            : 
            ///FECHA_MODIFICO      : 
            ///CAUSA_MODIFICACIÓN  : 
            ///*******************************************************************************
            private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
            {
                String Retornar = "";
                String Dato = "" + Dato_ID;
                for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
                {
                    Retornar = Retornar + "0";
                }
                Retornar = Retornar + Dato;
                return Retornar;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modificar_Tarjetas_Gasolina
            ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado
            ///PARAMETROS           : 1.Parametros. Contiene los parametros que se van hacer la
            ///                       Modificación en la Base de Datos.
            ///CREO                 : Salvador Vázquez Camacho
            ///FECHA_CREO           : 30/Julio/2010
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Modificar_Formato(Cls_Cat_Ort_Administracion_Urbana_Negocio Parametros)
            {
                Cls_Ope_Bandeja_Tramites_Negocio Neg_Actualizar_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
                String Mensaje = "";

                OracleConnection Conexion = new OracleConnection();
                OracleCommand Comando = new OracleCommand();
                OracleTransaction Transaccion = null;
             
                try
                {
                    if (Parametros.P_Cmmd != null)
                    {
                        Comando = Parametros.P_Cmmd;
                    }
                    else
                    {
                        Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
                        Conexion.Open();
                        Transaccion = Conexion.BeginTransaction();
                        Comando.Connection = Conexion;
                        Comando.Transaction = Transaccion;
                    }


                    String Mi_SQL = "UPDATE " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana;
                    Mi_SQL += " SET " + Ope_Ort_Formato_Admon_Urbana.Campo_Tramite_ID + " = '" + Parametros.P_Tramite_ID;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID + " = '" + Parametros.P_Solicitud_ID;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Subproceso_ID + " = '" + Parametros.P_Subproceso_ID;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Estatus + " = '" + Parametros.P_Estatus;

                    //  para los datos de la area
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Inspeccion + " = '" + Parametros.P_Area_Inspeccion;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Calle + " = '" + Parametros.P_Area_Calle;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Colonia + " = '" + Parametros.P_Area_Colonia;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Numero_Fisico + " = '" + Parametros.P_Area_Numero_Fisico;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Manzana + " = '" + Parametros.P_Area_Manzana;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Lote + " = '" + Parametros.P_Area_Lote;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Zona + " = '" + Parametros.P_Area_Zona;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Uso_Solicitado + " = '" + Parametros.P_Area_Uso_Solicitado;

                    //  para los datos del tipo de supervision// 
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Tipo_Supervision_ID + " = '" + Parametros.P_Tipo_Supervision_ID;

                    //  para los datos de las condiciones del inmueble//  
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Condiciones_Inmueble + " = '" + Parametros.P_Condiciones_Inmueble_ID;

                    //  para el avance de la obra
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Obra_ID + " = '" + Parametros.P_Avance_Obra_ID;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Niveles_Actuales + " = '" + Parametros.P_Avance_Niveles_Actuales;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Avances_Niveles_Construccion + " = '" + Parametros.P_Avance_Niveles_Construccion;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Proyecto_Acorde_Solicitado + " = '" + Parametros.P_Avance_Proyecto_Acorde;

                    //  para las vias publicas y donaciones
                    //Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Via_ID + " = '" + Parametros.P_Area_Via_ID;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Invasion_Donacion + " = '" + Parametros.P_VIA_PUBLICA_INVASION_DONACION;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Invasion_Material + " = '" + Parametros.P_VIA_PUBLICA_INVASION_MATERIAL;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Paramento + " = '" + Parametros.P_VIA_PUBLICA_PARAMENTO;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Via_Publica_Sobre_Marquesina + " = '" + Parametros.P_VIA_PUBLICA_SOBRE_MARQUESINA;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Via_Especificar_Restriccion + " = '" + Parametros.P_Area_Via_Especificar_Restricciones;

                    //  para las datos referentes a las inspecciones 
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Notificacion + " = '" + Parametros.P_Inspeccion_Notificacion;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Notificacion_Folio + " = '" + Parametros.P_Inspeccion_Notificacion_Folio;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Acta + " = '" + Parametros.P_Inspeccion_Acta;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Acta_Folio + " = '" + Parametros.P_Inspeccion_Acta_Folio;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Clausurado + " = '" + Parametros.P_Inspeccion_Clausurado;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Clausurado_Folio + " = '" + Parametros.P_Inspeccion_Clausurado_Folio;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Multado + " = '" + Parametros.P_Inspeccion_Multado;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Multado_Folio + " = '" + Parametros.P_Inspeccion_Multado_Folio;

                    //  para el uso actual
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Actual_ID + " = '" + Parametros.P_Uso_Actual_ID;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Acorde_Solicitado + " = '" + Parametros.P_Uso_Actual_Acorde_Solicitado;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Especificar_Tipo_Uso + " = '" + Parametros.P_Uso_Actual_Especificar_Tipo_Uso;

                    //  para el uso predominante de la zona  
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Predominante_Colindantes + " = '" + Parametros.P_Uso_Predominante_Colindantes;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Predominante_Frente_Inmueble + " = '" + Parametros.P_Uso_Predominante_Frente_Inmueble;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Predominante_Impacto_Considarar + " = '" + Parametros.P_Uso_Predominante_Impacto;

                    //  para el uso del funcionamiento 
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Actividad + " = '" + Parametros.P_Funcionamiento_Actividad;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Metros + " = '" + Parametros.P_Funcionamiento_Metros_Cuadrados;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Maquinaria + " = '" + Parametros.P_Funcionamiento_Maquinaria;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_ID + " = '" + Parametros.P_Funcionamiento_ID;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Numero_Personal + " = '" + Parametros.P_Funcionamiento_No_Personas;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_Numero_Clientes + " = '" + Parametros.P_Funcionamiento_No_Clientes;

                    //  para los campos de anuncios
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_1 + " = '" + Parametros.P_Anuncio_1;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_1 + " = '" + Parametros.P_Anuncio_1_Dimensiones;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_2 + " = '" + Parametros.P_Anuncio_2;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_2 + " = '" + Parametros.P_Anuncio_2_Dimensiones;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_3 + " = '" + Parametros.P_Anuncio_3;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_3 + " = '" + Parametros.P_Anuncio_3_Dimensiones;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_4 + " = '" + Parametros.P_Anuncio_4;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Anuncio_Dimensiones_4 + " = '" + Parametros.P_Anuncio_4_Dimensiones;

                    //  para los servicios
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Cuenta + " = '" + Parametros.P_Servicios_Cuenta_Sanitarios;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_WC + " = '" + Parametros.P_Servicios_WC;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Lavabo + " = '" + Parametros.P_Servicios_Lavabo;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Letrina + " = '" + Parametros.P_Servicios_Letrina;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Mixto + " = '" + Parametros.P_Servicios_Mixto;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Numero_Sanitarios_Hombres + " = '" + Parametros.P_Servicios_Numero_Sanitarios_Hombres;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Numero_Sanitarios_Mujeres + " = '" + Parametros.P_Servicios_Numero_Sanitarios_Mujeres;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Potable + " = '" + Parametros.P_Servicios_Agua_Potable;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Abast_Particular + " = '" + Parametros.P_Servicios_Agua_Abastecimiento_Particular;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Agua_Abast_Japami + " = '" + Parametros.P_Servicios_Agua_Abastecimiento_Japami;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Drenaje + " = '" + Parametros.P_Servicios_Drenaje;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Fosa_Septica + " = '" + Parametros.P_Servicios_Fosa_Septica;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento + " = '" + Parametros.P_Servicios_Estacionamiento;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Propio + " = '" + Parametros.P_Servicios_Estacionamiento_Propio;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Rentado + " = '" + Parametros.P_Servicios_Estacionamiento_Rentado;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Numero_Cajones + " = '" + Parametros.P_Servicios_Estacionamiento_Numero_Cajones;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Area_Descarga + " = '" + Parametros.P_Servicios_Estacionamiento_Area_Descarga;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Servicio_Estacionamiento_Domicilio + " = '" + Parametros.P_Servicios_Estacionamiento_Domicilio;

                    //  para los materiales empleados
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Material_Empleado_Muros + " = '" + Parametros.P_Materiales_Empleado_Muros;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Material_Empleado_Techo + " = '" + Parametros.P_Materiales_Empleado_Techos;

                    //  para las medidas de seguridad
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Medidas + " = '" + Parametros.P_Seguridad_Medidas;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Equipo + " = '" + Parametros.P_Seguridad_Equipo;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Especificar + " = '" + Parametros.P_Seguridad_Especificar;

                    //  para la poda de arboles
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Altura + " = '" + Parametros.P_Poda_Altura;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Diametro_Tronco + " = '" + Parametros.P_Poda_Diametro_Tronco;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Diametro_Fronda + " = '" + Parametros.P_Poda_Fronda;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Poda_Estado + " = '" + Parametros.P_Poda_Estado;

                    //  para los campos generales
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Recepcion_Inspector + " = '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Generales_Recepcion_Inspector);
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Fecha_Realizada_Campo + " = '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Generales_Fecha_Revision_Campo);
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Recepcion_Coordinacion + " = '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Generales_Recepcion_Coordinacion);
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Observaciones_Para_Insepctor + " = '" + Parametros.P_Generales_Observaciones_Para_Inspector;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Generales_Observaciones_Del_Insepctor + " = '" + Parametros.P_Generales_Observaciones_Inspector;

                    //  para los campos de auditoria
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Usuario_Modifico + " = '" + Parametros.P_Usuario;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL += " , " + Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Bardeo_Aproximado + " = '" + Parametros.P_Avance_Bardeo_Aproximado;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Seguridad_Material_Flamable + " = '" + Parametros.P_Seguridad_Material_Flamable;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspector_ID + " = '" + Parametros.P_Inspector_ID;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Cuenta_Predial + " = '" + Parametros.P_Cuenta_Predial;



                    //  para los datos de la licencia ambiental de funcionamiento
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Equipo_Emisor + " = '" + Parametros.P_Licencia_Tipo_Equipo;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Emision + " = '" + Parametros.P_Licencia_Tipo_Emision;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Horario_Funcionamiento + " = '" + Parametros.P_Licencia_Horario_Funcionamiento;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Tipo_Conbustible + " = '" + Parametros.P_Licencia_Tipo_Combustible;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Licencia_Gasto_Combustible + " = '" + Parametros.P_Licencia_Tipo_Gastos_Combustible;

                    //  para los datos del manifiesto de impacto ambiental
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Afectaciones + " = '" + Parametros.P_Impacto_Afectables;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Colindancias + " = '" + Parametros.P_Impacto_Colindancias;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Superficie + " = '" + Parametros.P_Impacto_Superficie;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Impacto_Tipo_Proyecto + " = '" + Parametros.P_Impacto_Tipo_Proyecto;

                    //  para los datos del banco de materiales
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Permiso_Ecologia + " = '" + Parametros.P_Material_Permiso_Ecologico;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Permiso_Suelo + " = '" + Parametros.P_Material_Permiso_Suelo;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Superficie_Total + " = '" + Parametros.P_Material_Superficie_Total;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Profundidad + " = '" + Parametros.P_Material_Profundidad;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Inclinacion + " = '" + Parametros.P_Material_Inclinacion;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Flora + " = '" + Parametros.P_Material_Flora;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Acceso_Vehiculoas + " = '" + Parametros.P_Material_Acceso_Vehiculos;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Petreo + " = '" + Parametros.P_Material_Petreo;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Arboles_Especie + " = '" + Parametros.P_Material_Especie_Arbol;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Tipo_Poda + " = '" + Parametros.P_Material_Tipo_Poda;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Cantidad_Poda + " = '" + Parametros.P_Material_Cantidad_Poda;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Tipo_Tala + " = '" + Parametros.P_Material_Tipo_Tala;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Cantidad_Tala + " = '" + Parametros.P_Material_Cantidad_Tala;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Tipo_Trasplante + " = '" + Parametros.P_Material_Tipo_Trasplante;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Materiales_Cantidad_Trasplante + " = '" + Parametros.P_Material_Cantidad_Trasplante;

                    //  para los datos de la autorizacion de aprovechamiento ambiental
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Suelos + " = '" + Parametros.P_Autoriza_Suelos;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Area_Residuos + " = '" + Parametros.P_Autoriza_Area_Residuos;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Separacion + " = '" + Parametros.P_Autoriza_Separacion;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Metodo_Separacion + " = '" + Parametros.P_Autoriza_Metodo_Separacion;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Servicio_Recoloccion + " = '" + Parametros.P_Autoriza_Servicio_Recoleccion;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Revuelven_Solidos_Liquidos + " = '" + Parametros.P_Autoriza_Revuelven_Solidos_Liquidos;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Tipo_Contenedor + " = '" + Parametros.P_Autoriza_Tipo_Contenedor;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Tipo_Ruido + " = '" + Parametros.P_Autoriza_Tipo_Ruido;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Nivel_Ruido + " = '" + Parametros.P_Autoriza_Nivel_Ruido;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Horario_Labores + " = '" + Parametros.P_Autoriza_Horario_Labores;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Lunes + " = '" + Parametros.P_Autoriza_Lunes;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Martes + " = '" + Parametros.P_Autoriza_Martes;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Miercoles + " = '" + Parametros.P_Autoriza_Miercoles;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Jueves + " = '" + Parametros.P_Autoriza_Jueves;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Viernes + " = '" + Parametros.P_Autoriza_Viernes;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Sabado + " = '" + Parametros.P_Autoriza_Sabado;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Domingo + " = '" + Parametros.P_Autoriza_Domingo;
                    Mi_SQL += "', " + Ope_Ort_Formato_Admon_Urbana.Campo_Autoriza_Emisiones + " = '" + Parametros.P_Autoriza_Emisiones;

                    Mi_SQL += "' WHERE " + Ope_Ort_Formato_Admon_Urbana.Campo_Administracion_Urbana_ID + " = '" + Parametros.P_Administracion_Urbana_ID + "'";

                    Comando.CommandText = Mi_SQL;
                    Comando.ExecuteNonQuery();

                    DataSet Ds_Datos = null;
                    DataTable Dt_Datos = new DataTable();
                    /// Primer registro que se va a eliminar
                    Int32 Inicial = 0;
                    /// Ultimo registro que se va a eliminar
                    Int32 Final = 0;
                    /// Variable que recorre cada registro
                    Int32 Sigiente;

                    Mi_SQL = "SELECT * FROM " + Ope_Ort_Det_Residuos.Tabla_Ope_Ort_Det_Residuos + " WHERE ";
                    Mi_SQL += Ope_Ort_Det_Residuos.Campo_Ficha_Inspeccion + " = '" + Parametros.P_Administracion_Urbana_ID + "'";
                    Mi_SQL += " ORDER BY " + Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID + " ASC";
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                    {
                        Ds_Datos = OracleHelper.ExecuteDataset(Comando, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Datos != null)
                    {
                        Dt_Datos = Ds_Datos.Tables[0];
                        /// Bandera para la primera asignacion de valores
                        Boolean Primero = true;
                        foreach (DataRow Dr_Renglon in Dt_Datos.Rows)
                        {
                            Sigiente = Convert.ToInt32(Dr_Renglon[Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID].ToString());
                            if (Primero)/// Valida si es la primera iteracion
                            {
                                /// si es la primera iteracion el inicial va a ser igual al final
                                Inicial = Final = Sigiente;
                                Primero = false;
                            }
                            else//para todas las demas iteraciones
                            {
                                if (Sigiente > Final)
                                    Final = Sigiente;
                                if (Sigiente < Inicial)
                                    Inicial = Sigiente;
                            }
                        }
                        if (Inicial == 0 && Final == 0)
                        {
                            object Id;
                            Mi_SQL = "SELECT NVL(MAX (" + Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID + "), '0000000000') FROM " + Ope_Ort_Det_Residuos.Tabla_Ope_Ort_Det_Residuos;

                            Comando.CommandText = Mi_SQL;
                            Id = Comando.ExecuteScalar();
                            Inicial = Final = Convert.ToInt32(Id) + 1;
                        }
                    }
                    Mi_SQL = "DELETE FROM " + Ope_Ort_Det_Residuos.Tabla_Ope_Ort_Det_Residuos + " WHERE ";
                    Mi_SQL += Ope_Ort_Det_Residuos.Campo_Ficha_Inspeccion + " = '" + Parametros.P_Administracion_Urbana_ID + "'";
                    Comando.CommandText = Mi_SQL;
                    Comando.ExecuteNonQuery();

                    Mi_SQL = "SELECT * FROM " + Ope_Ort_Det_Residuos.Tabla_Ope_Ort_Det_Residuos + " WHERE ";
                    Mi_SQL += Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID + " > TO_NUMBER('" + Final.ToString() + "')";
                    Mi_SQL += " ORDER BY " + Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID + " ASC";

                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                    {
                        Ds_Datos = OracleHelper.ExecuteDataset(Comando, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Datos != null)
                    {
                        Dt_Datos = Ds_Datos.Tables[0];
                        foreach (DataRow Dr_Renglon in Dt_Datos.Rows)
                        {
                            Mi_SQL = "UPDATE " + Ope_Ort_Det_Residuos.Tabla_Ope_Ort_Det_Residuos;
                            Mi_SQL += " SET " + Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID + " = '" + String.Format("{0:0000000000}", Inicial);
                            Mi_SQL += "' WHERE " + Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID + " = '" + Dr_Renglon[Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID].ToString() + "'";
                            Comando.CommandText = Mi_SQL;
                            Comando.ExecuteNonQuery();
                            Inicial++;
                        }
                    }

                    if (Inicial == 0)
                        Inicial++;

                    String Detalle_Residuo_ID = "";
                    if (Parametros.P_Dt_Residuos != null && Parametros.P_Dt_Residuos.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Parametros.P_Dt_Residuos.Rows)
                        {
                            Detalle_Residuo_ID = Convertir_A_Formato_ID(Inicial, 10);
                            Mi_SQL = "INSERT INTO " + Ope_Ort_Det_Residuos.Tabla_Ope_Ort_Det_Residuos + "(";
                            Mi_SQL += Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID;
                            Mi_SQL += ", " + Ope_Ort_Det_Residuos.Campo_Ficha_Inspeccion;
                            Mi_SQL += ", " + Ope_Ort_Det_Residuos.Campo_Tipo_Residuo_ID;
                            Mi_SQL += ") VALUES( ";
                            Mi_SQL += "'" + Detalle_Residuo_ID + "' ";
                            Mi_SQL += ", '" + Parametros.P_Administracion_Urbana_ID + "' ";
                            Mi_SQL += ", '" + Registro["TIPO_RESIDUO_ID"].ToString() + "') ";
                            Comando.CommandText = Mi_SQL.ToString();
                            Comando.ExecuteNonQuery();
                            Inicial++;
                        }
                    }

                    if (Parametros.P_Estatus != "ACTUALIZAR")
                    {
                        Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                            + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                            + "," + Ope_Tra_Solicitud.Campo_Estatus
                            + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                            + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ")) = UPPER(TRIM('" + Parametros.P_Solicitud_ID + "'))";
                        Comando.CommandText = Mi_SQL.ToString();
                        Comando.CommandType = CommandType.Text;
                        OracleDataReader Dtr_Datos_Solicitud = Comando.ExecuteReader();

                        // si hay datos para leer, agregar pasivo
                        if (Dtr_Datos_Solicitud.Read())
                        {
                            // establecer parámetros para actualizar solicitud
                            Neg_Actualizar_Solicitud.P_Solicitud_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString().Trim();
                            Neg_Actualizar_Solicitud.P_Estatus = Parametros.P_Estatus;
                            Neg_Actualizar_Solicitud.P_Subproceso_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString().Trim();
                            Neg_Actualizar_Solicitud.P_Comentarios = "";
                            Neg_Actualizar_Solicitud.P_Usuario = Parametros.P_Usuario;
                            Neg_Actualizar_Solicitud.P_Comando_Oracle = Comando;
                            // llamar método que actualizar la solicitud
                            Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Evaluar_Solicitud();
                        }
                    }
                    Transaccion.Commit();
                }

                catch (OracleException Ex)
                {
                    if (Transaccion != null)
                    {
                        Transaccion.Rollback();
                    }
                    Mensaje = "Error: ";
                    switch (Ex.Code.ToString())
                    {
                        case "2291":
                            Mensaje = "No existe un registro relacionado con esta operacion";
                            break;
                        case "923":
                            Mensaje = "Consulta SQL";
                            break;
                        case "12170":
                            Mensaje = "Conexion con el Servidor";
                            break;
                        default:
                            Mensaje = Ex.Message;
                            break;
                    }
                    throw new Exception(Mensaje + "[" + Ex.ToString() + "]");
                }
                catch (Exception Ex)
                {
                    Transaccion.Rollback();

                    throw new Exception(Ex.ToString());
                }
                finally
                {
                    Conexion.Close();

                }
            }

        #endregion

        #region Consultas

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Zonas
        ///DESCRIPCIÓN          : Metodo para consultar los datos de las zonas
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Zonas(Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio)
        {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            Boolean Entro_Where = false;
            try
            {
                Mi_SQL = "SELECT * FROM " + Cat_Ort_Zona.Tabla_Cat_Ort_Zona;
                if (!String.IsNullOrEmpty(Negocio.P_Empleado_Id))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += Cat_Ort_Zona.Tabla_Cat_Ort_Zona + "." + Cat_Ort_Zona.Campo_Empleado_ID + " = '" + Negocio.P_Empleado_Id + "'";
                }
                if (!String.IsNullOrEmpty(Negocio.P_Area_Zona))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += Cat_Ort_Zona.Tabla_Cat_Ort_Zona + "." + Cat_Ort_Zona.Campo_Zona_ID + " = '" + Negocio.P_Area_Zona + "'";
                }
                Mi_SQL += " ORDER BY " + Cat_Ort_Zona.Campo_Nombre;
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Datos != null)
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tipo_Supervision
        ///DESCRIPCIÓN          : Metodo para consultar los datos de las zonas
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Tipo_Supervision(Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ort_Tipo_Supervision.Tabla_Cat_Ort_Tipo_Supervision);
                Mi_Sql.Append(" ORDER BY " + Cat_Ort_Tipo_Supervision.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Condiciones_Inmueble
        ///DESCRIPCIÓN          : Metodo para consultar los datos de los tipos inmuebles
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Condiciones_Inmueble(Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ort_Condi_Inmueble.Tabla_Cat_Ort_Condi_Inmueble);
                Mi_Sql.Append(" ORDER BY " + Cat_Ort_Condi_Inmueble.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Avance_Obra
        ///DESCRIPCIÓN          : Metodo para consultar los estatus de las obras
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Avance_Obra(Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ort_Avance_Obra.Tabla_Cat_Ort_Avance_Obran);
                Mi_Sql.Append(" ORDER BY " + Cat_Ort_Avance_Obra.Campo_Avance_Obra_ID);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Condiciones_Via_Publica_Donacion
        ///DESCRIPCIÓN          : Metodo para consultar los estatus de las obras
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Condiciones_Via_Publica_Donacion(Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ort_Area_Public_Donac.Tabla_Cat_Ort_Area_Publica_Donacion);
                Mi_Sql.Append(" ORDER BY " + Cat_Ort_Area_Public_Donac.Campo_Descripcion);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Uso_Actual_Terreno
        ///DESCRIPCIÓN          : Metodo para consultar el tipo de terreno (comercial, habitacional)
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Uso_Actual_Terreno(Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ort_Uso_Actual.Tabla_Cat_Ort_Uso_Actual);
                Mi_Sql.Append(" ORDER BY " + Cat_Ort_Uso_Actual.Campo_Descripcion);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Tipos_Materiales
        ///DESCRIPCIÓN          : Metodo para consultar los tipos de materiales
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Tipos_Materiales(Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ort_Tipo_Material.Tabla_Cat_Ort_Tipo_Material);
                Mi_Sql.Append(" ORDER BY " + Cat_Ort_Tipo_Material.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Inspectores
        ///DESCRIPCIÓN          : Metodo que consultara  los inspectores que se encuentran registrados
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Inspectores(Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores);
                Mi_Sql.Append(" ORDER BY " + Cat_Ort_Inspectores.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Problemas_Funcionamiento
        ///DESCRIPCIÓN          : Metodo para consultar los tipos de problemas con
        ///                         respecto al funcionamiento
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Problemas_Funcionamiento(Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ort_Funcionamiento.Tabla_Cat_Ort_Funcionamiento);
                Mi_Sql.Append(" ORDER BY " + Cat_Ort_Funcionamiento.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Llenado_Solicitud_Formato
        ///DESCRIPCIÓN          : Metodo que consultara los formatos que se deben de llenar
        ///PARAMETROS           : clase de negocio
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 07/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Llenado_Solicitud_Formato(Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            DataTable Dt_Actividades_Con_Formato = new DataTable();
            try
            {
                Mi_Sql.Append("SELECT * FROM " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato);
                Dt_Actividades_Con_Formato= OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];

                Mi_Sql = new StringBuilder();
                Mi_Sql.Append("SELECT ");
                //  para los id de la solicitud
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID);
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID);
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID);
                //  para la informacion de la solicitud
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud);
                Mi_Sql.Append(", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre + " as Nombre_Actividad ");
                Mi_Sql.Append(", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre + " as Nombre_Tramite ");

                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Consecutivo);

                Mi_Sql.Append(" FROM ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);

                Mi_Sql.Append(" left outer join  " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " ON ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID + "=");
                Mi_Sql.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID);

                Mi_Sql.Append(" left outer join  " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + " ON ");
                Mi_Sql.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID + "=");
                Mi_Sql.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID);

                Mi_Sql.Append(" left outer join  " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " ON ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID + "=");
                Mi_Sql.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_Sql.Append(" left outer join  " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + " ON ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID + "=");
                Mi_Sql.Append(Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + "." + Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID);

                Mi_Sql.Append(" WHERE ");
                Mi_Sql.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Plantilla_ID + "='" + Negocio.P_Plantilla_ID + "' ");
                
                //  bloque para comprobar que no se encuentre registrado el formato dentro de la tabla Ope_Ort_Formato_Admon_Urbana
                int Contador_OR = 0;
                if (Dt_Actividades_Con_Formato != null && Dt_Actividades_Con_Formato.Rows.Count > 0)
                {
                    Mi_Sql.Append(" and (");
                    foreach (DataRow Registro in Dt_Actividades_Con_Formato.Rows)
                    {
                        if (Contador_OR > 0)
                        {
                            Mi_Sql.Append(" or ");
                        }
                        Mi_Sql.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID + "='" );
                        Mi_Sql.Append(Registro[Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID].ToString() + "' ");
                        Contador_OR++;
                    }
                    Mi_Sql.Append(" )");
                }

                //  filtro para el empleado id que pertenece a la zona
                //Mi_Sql.Append(" and (" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Empleado_ID + "='" + Negocio.P_Empleado_Id + "')");

                Mi_Sql.Append(" Order by ");
                Mi_Sql.Append(" Nombre_Tramite asc, Nombre_Actividad asc ");
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " asc ");
               
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Formato_ID
        ///DESCRIPCIÓN          : Metodo para consultar el id del formato
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 01/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Formato_ID(Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido);
                Mi_Sql.Append(" WHERE upper(" + Cat_Tra_Formato_Predefinido.Campo_Nombre + ")= upper('" + Negocio.P_Nombre_Plantilla + "')");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Administracion_Urbana
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 
        ///                     1.Parametros.Contiene los parametros que se van a utilizar para
        ///                       hacer la consulta de la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Administracion_Urbana(Cls_Cat_Ort_Administracion_Urbana_Negocio Parametros)
        {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            Boolean Entro_Where = false;
            try
            {
                Mi_SQL = "SELECT ADMON_URBANA."+Ope_Ort_Formato_Admon_Urbana.Campo_Administracion_Urbana_ID;
                Mi_SQL += ", ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Tramite_ID;
                Mi_SQL += ", ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID;
                Mi_SQL += ", ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Subproceso_ID;
                Mi_SQL += ", TRAMITES." + Cat_Tra_Tramites.Campo_Clave_Tramite;
                Mi_SQL += ", TRAMITES." + Cat_Tra_Tramites.Campo_Nombre;
                Mi_SQL += ", SOLICITUD." + Ope_Tra_Solicitud.Campo_Clave_Solicitud;
                Mi_SQL += ", SOLICITUD." + Ope_Tra_Solicitud.Campo_Folio;
                Mi_SQL += ", SOLICITUD." + Ope_Tra_Solicitud.Campo_Consecutivo;
                Mi_SQL += ", SUBPROCESOS." + Cat_Tra_Subprocesos.Campo_Nombre;
                Mi_SQL += " FROM " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + " ADMON_URBANA ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITES ";
                Mi_SQL += "ON ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Tramite_ID + " = TRAMITES." + Cat_Tra_Tramites.Campo_Tramite_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD ";
                Mi_SQL += "ON ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID + " = SOLICITUD." + Ope_Tra_Solicitud.Campo_Solicitud_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " SUBPROCESOS ";
                Mi_SQL += "ON ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Subproceso_ID + " = SUBPROCESOS." + Cat_Tra_Subprocesos.Campo_Subproceso_ID;
                if (!String.IsNullOrEmpty(Parametros.P_Administracion_Urbana_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Administracion_Urbana_ID + " = '" + Parametros.P_Administracion_Urbana_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Tramite_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Tramite_ID + " = '" + Parametros.P_Tramite_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Solicitud_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID + " = '" + Parametros.P_Solicitud_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Subproceso_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Subproceso_ID + " = '" + Parametros.P_Subproceso_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Tipo_Supervision_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Tipo_Supervision_ID + " = '" + Parametros.P_Tipo_Supervision_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Condiciones_Inmueble_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Condiciones_Inmueble + " = '" + Parametros.P_Condiciones_Inmueble_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Avance_Obra_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Obra_ID + " = '" + Parametros.P_Avance_Obra_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Area_Zona))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Zona + " = '" + Parametros.P_Area_Zona + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Area_Via_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Via_ID + " = '" + Parametros.P_Area_Via_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Uso_Actual_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Actual_ID + " = '" + Parametros.P_Uso_Actual_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Funcionamiento_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_ID + " = '" + Parametros.P_Funcionamiento_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Inspector_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Inspector_ID + " = '" + Parametros.P_Inspector_ID + "'";
                }
                Mi_SQL += " ORDER BY " + Ope_Ort_Formato_Admon_Urbana.Campo_Administracion_Urbana_ID + " ASC";
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Datos != null)
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Administracion_Urbana
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 
        ///                     1.Parametros.Contiene los parametros que se van a utilizar para
        ///                       hacer la consulta de la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Administracion_Urbana(Cls_Cat_Ort_Administracion_Urbana_Negocio Parametros)
        {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            Boolean Entro_Where = false;
            try
            {
                Mi_SQL = "SELECT * FROM " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana + " ADMON_URBANA ";
                if (!String.IsNullOrEmpty(Parametros.P_Administracion_Urbana_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Administracion_Urbana_ID + " = '" + Parametros.P_Administracion_Urbana_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Tramite_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Tramite_ID + " = '" + Parametros.P_Tramite_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Solicitud_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID + " = '" + Parametros.P_Solicitud_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Subproceso_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Subproceso_ID + " = '" + Parametros.P_Subproceso_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Tipo_Supervision_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Tipo_Supervision_ID + " = '" + Parametros.P_Tipo_Supervision_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Condiciones_Inmueble_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Condiciones_Inmueble + " = '" + Parametros.P_Condiciones_Inmueble_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Avance_Obra_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Obra_ID + " = '" + Parametros.P_Avance_Obra_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Area_Via_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Area_Via_ID + " = '" + Parametros.P_Area_Via_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Uso_Actual_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Uso_Actual_ID + " = '" + Parametros.P_Uso_Actual_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Funcionamiento_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Funcionamiento_ID + " = '" + Parametros.P_Funcionamiento_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Inspector_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " ADMON_URBANA." + Ope_Ort_Formato_Admon_Urbana.Campo_Inspector_ID + " = '" + Parametros.P_Inspector_ID + "'";
                }
                Mi_SQL += " ORDER BY " + Ope_Ort_Formato_Admon_Urbana.Campo_Administracion_Urbana_ID + " ASC";
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Datos != null)
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Datos;
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
        public static DataTable Consultar_Residuos(Cls_Cat_Ort_Administracion_Urbana_Negocio Parametros)
        {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            Boolean Entro_Where = false;
            try
            {
                Mi_SQL = "SELECT RESIDUOS." + Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID + " DETALLE_ID, ";
                Mi_SQL += "RESIDUOS." + Ope_Ort_Det_Residuos.Campo_Ficha_Inspeccion + " FICHA, ";
                Mi_SQL += "RESIDUOS." + Ope_Ort_Det_Residuos.Campo_Tipo_Residuo_ID + " TIPO_RESIDUO_ID, ";
                Mi_SQL += "DETALLES_RESIDUOS." + Cat_Ort_Tipo_Residuos.Campo_Nombre + " NOMBRE ";
                Mi_SQL += " FROM " + Ope_Ort_Det_Residuos.Tabla_Ope_Ort_Det_Residuos + " RESIDUOS ";
                Mi_SQL += "LEFT OUTER JOIN " + Cat_Ort_Tipo_Residuos.Tabla_Cat_Ort_Tipo_Residuos + " DETALLES_RESIDUOS ";
                Mi_SQL += "ON RESIDUOS." + Ope_Ort_Det_Residuos.Campo_Tipo_Residuo_ID + " = DETALLES_RESIDUOS." + Cat_Ort_Tipo_Residuos.Campo_Residuo_ID;
                if (!String.IsNullOrEmpty(Parametros.P_Administracion_Urbana_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " RESIDUOS." + Ope_Ort_Det_Residuos.Campo_Ficha_Inspeccion + " = '" + Parametros.P_Administracion_Urbana_ID + "'";
                }
                Mi_SQL += " ORDER BY " + Ope_Ort_Det_Residuos.Campo_Detalle_Residuos_ID+ " ASC";
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Datos != null)
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Datos;
        }

        #endregion
    }
}

