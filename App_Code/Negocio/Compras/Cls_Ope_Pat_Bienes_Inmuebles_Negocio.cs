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
using Presidencia.Control_Patrimonial_Operacion_Bienes_Inmuebles.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pat_Bienes_Inmuebles_Negocio
/// </summary>

namespace Presidencia.Control_Patrimonial_Operacion_Bienes_Inmuebles.Negocio {
 
    public class Cls_Ope_Pat_Bienes_Inmuebles_Negocio {

        #region "Variables Internas"

            private String Bien_Inmueble_ID = String.Empty;
            private String Calle = String.Empty;
            private String No_Exterior = String.Empty;
            private String No_Interior = String.Empty;
            private String Colonia = String.Empty;
            private String Uso_ID = String.Empty;
            private String Destino_ID = String.Empty;
            private Double Superficie = -1;
            private Double Construccion_Construida = -1;
            private String Manzana = String.Empty;
            private String Lote = String.Empty;
            private Double Ocupacion = -1;
            private String Cuenta_Predial_ID = String.Empty;
            private String Propietario = String.Empty;
            private String Efectos_Fiscales = String.Empty;
            private String Sector_ID = String.Empty;
            private String Clasificacion_Zona_ID = String.Empty;
            private String Tipo_Predio_ID = String.Empty;
            private String Vias_Acceso = String.Empty;
            private String Estado = String.Empty;
            private Double Densidad_Construccion = -1;
            private Double Valor_Comercial = -1;
            private String Observaciones_Generales = String.Empty;
            private String Tipo_Complento = String.Empty;
            private String Usuario_ID = String.Empty;
            private String Usuario_Nombre = String.Empty;
            private String Observaciones = String.Empty;
            private DataTable Dt_Observaciones = new DataTable();
            private DataTable Dt_Medidas_Colindancias = new DataTable();
            private DataTable Dt_Historico_Juridico = new DataTable();
            private DataTable Dt_Anexos = new DataTable();
            private DataTable Dt_Anexos_Bajas = new DataTable();
            private DataTable Dt_Expropiaciones = new DataTable();
            private DataTable Dt_Afectaciones = new DataTable();
            private String Origen_ID = null;
            private String Estatus = null;
            private DateTime Fecha_Registro = new DateTime();
            private Double Construccion_Desde = -1;
            private Double Construccion_Hasta = -1;
            private String Expropiacion = String.Empty;
            private String Area_ID = String.Empty;

            //Sustento Juridico
            private String No_Registro_Alta_Juridico = String.Empty;
            private String No_Escritura = String.Empty;
            private DateTime Fecha_Escritura = new DateTime();
            private String No_Notario = String.Empty;
            private String Notario_Nombre = String.Empty;
            private String Constancia_Registral = String.Empty;
            private String Folio_Real = String.Empty;
            private String Libre_Gravament = String.Empty;
            private String Antecedente = String.Empty;
            private String Proveedor = String.Empty;
            private String Observaciones_Juridicas = String.Empty;
            private String No_Contrato_Juridico = String.Empty;

            private String No_Registro_Baja_Juridico = String.Empty;
            private String No_Escritura_Baja = String.Empty;
            private DateTime Fecha_Escritura_Baja = new DateTime();
            private String No_Notario_Baja = String.Empty;
            private String Notario_Nombre_Baja = String.Empty;
            private String Constancia_Registral_Baja = String.Empty;
            private String Folio_Real_Baja = String.Empty;
            private String Nuevo_Propietario_Juridico = String.Empty;
            private String No_Contrato_Baja = String.Empty;
            private DateTime Fecha_Baja = new DateTime();


            //Anexos
            private String Tipo_Anexo = String.Empty;
            private String Archivo = String.Empty;
            private String Descripcion_Anexo = String.Empty;

            //Afectaciones
            private DateTime Fecha_Afectacion = new DateTime();
            private String Nuevo_Propietario = String.Empty;
            private String Session_Ayuntamiento = String.Empty;
            private String Tramo = String.Empty;
            private String No_Contrato = String.Empty;
            private Boolean Agregar_Afectacion = false;

            //Afectaciones
            private String Hoja = String.Empty;
            private String Tomo = String.Empty;
            private String Numero_Acta = String.Empty;
            private String Cartilla_Parcelaria = String.Empty;
            private Double Superficie_Contable = -1;
            private String Unidad_Superficie = String.Empty;
            private String Clase_Activo_ID = String.Empty;
            private String Tipo_Bien = String.Empty;
            private String Registro_Propiedad = String.Empty;
            private DateTime Fecha_Alta_Cuenta_Publica = new DateTime();
        #endregion

        #region "Variables Publicas"

            public String P_Bien_Inmueble_ID {
                get { return Bien_Inmueble_ID; }
                set { Bien_Inmueble_ID = value; }
            }
            public String P_Calle
            {
                get { return Calle; }
                set { Calle = value; }
            }
            public String P_No_Exterior
            {
                get { return No_Exterior; }
                set { No_Exterior = value; }
            }
            public String P_No_Interior
            {
                get { return No_Interior; }
                set { No_Interior = value; }
            }
            public String P_Colonia
            {
                get { return Colonia; }
                set { Colonia = value; }
            }
            public String P_Uso_ID
            {
                get { return Uso_ID; }
                set { Uso_ID = value; }
            }
            public String P_Destino_ID
            {
                get { return Destino_ID; }
                set { Destino_ID = value; }
            }
            public Double P_Superficie
            {
                get { return Superficie; }
                set { Superficie = value; }
            }
            public Double P_Construccion_Construida
            {
                get { return Construccion_Construida; }
                set { Construccion_Construida = value; }
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
            public Double P_Ocupacion
            {
                get { return Ocupacion; }
                set { Ocupacion = value; }
            }
            public String P_Cuenta_Predial_ID
            {
                get { return Cuenta_Predial_ID; }
                set { Cuenta_Predial_ID = value; }
            }
            public String P_Propietario
            {
                get { return Propietario; }
                set { Propietario = value; }
            }
            public String P_Efectos_Fiscales
            {
                get { return Efectos_Fiscales; }
                set { Efectos_Fiscales = value; }
            }
            public String P_Sector_ID
            {
                get { return Sector_ID; }
                set { Sector_ID = value; }
            }
            public String P_Clasificacion_Zona_ID
            {
                get { return Clasificacion_Zona_ID; }
                set { Clasificacion_Zona_ID = value; }
            }
            public String P_Tipo_Predio_ID
            {
                get { return Tipo_Predio_ID; }
                set { Tipo_Predio_ID = value; }
            }
            public String P_Vias_Acceso
            {
                get { return Vias_Acceso; }
                set { Vias_Acceso = value; }
            }
            public String P_Estado
            {
                get { return Estado; }
                set { Estado = value; }
            }
            public Double P_Densidad_Construccion
            {
                get { return Densidad_Construccion; }
                set { Densidad_Construccion = value; }
            }
            public Double P_Valor_Comercial
            {
                get { return Valor_Comercial; }
                set { Valor_Comercial = value; }
            }
            public String P_Observaciones_Generales
            {
                get { return Observaciones_Generales; }
                set { Observaciones_Generales = value; }
            }
            public String P_Tipo_Complento {
                get { return Tipo_Complento; }
                set { Tipo_Complento = value; }
            }
            public String P_Usuario_ID
            {
                get { return Usuario_ID; }
                set { Usuario_ID = value; }
            }
            public String P_Usuario_Nombre
            {
                get { return Usuario_Nombre; }
                set { Usuario_Nombre = value; }
            }
            public String P_Observaciones
            {
                get { return Observaciones; }
                set { Observaciones = value; }
            }
            public DataTable P_Dt_Observaciones
            {
                get { return Dt_Observaciones; }
                set { Dt_Observaciones = value; }
            }
            public DataTable P_Dt_Medidas_Colindancias
            {
                get { return Dt_Medidas_Colindancias; }
                set { Dt_Medidas_Colindancias = value; }
            }
            public DataTable P_Dt_Historico_Juridico
            {
                get { return Dt_Historico_Juridico; }
                set { Dt_Historico_Juridico = value; }
            }
            public DataTable P_Dt_Anexos
            {
                get { return Dt_Anexos; }
                set { Dt_Anexos = value; }
            }
            public DataTable P_Dt_Anexos_Bajas
            {
                get { return Dt_Anexos_Bajas; }
                set { Dt_Anexos_Bajas = value; }
            }
            public DataTable P_Dt_Expropiaciones
            {
                get { return Dt_Expropiaciones; }
                set { Dt_Expropiaciones = value; }
            }
            public DataTable P_Dt_Afectaciones {
                get { return Dt_Afectaciones; }
                set { Dt_Afectaciones = value; }
            }
            public DateTime P_Fecha_Registro
            {
                get { return Fecha_Registro; }
                set { Fecha_Registro = value; }
            }
            public Double P_Construccion_Desde
            {
                get { return Construccion_Desde; }
                set { Construccion_Desde = value; }
            }
            public Double P_Construccion_Hasta
            {
                get { return Construccion_Hasta; }
                set { Construccion_Hasta = value; }
            }
            public String P_Expropiacion
            {
                get { return Expropiacion; }
                set { Expropiacion = value; }
            }
            public String P_Area_ID
            {
                get { return Area_ID; }
                set { Area_ID = value; }
            }
            //Sustento Juridico
            public String P_No_Registro_Alta_Juridico
            {
                get { return No_Registro_Alta_Juridico; }
                set { No_Registro_Alta_Juridico = value; }
            }
            public String P_No_Escritura
            {
                get { return No_Escritura; }
                set { No_Escritura = value; }
            }
            public DateTime P_Fecha_Escritura
            {
                get { return Fecha_Escritura; }
                set { Fecha_Escritura = value; }
            }
            public String P_No_Notario
            {
                get { return No_Notario; }
                set { No_Notario = value; }
            }
            public String P_Notario_Nombre
            {
                get { return Notario_Nombre; }
                set { Notario_Nombre = value; }
            }
            public String P_Constancia_Registral
            {
                get { return Constancia_Registral; }
                set { Constancia_Registral = value; }
            }
            public String P_Folio_Real
            {
                get { return Folio_Real; }
                set { Folio_Real = value; }
            }
            public String P_Libre_Gravament
            {
                get { return Libre_Gravament; }
                set { Libre_Gravament = value; }
            }
            public String P_Antecedente
            {
                get { return Antecedente; }
                set { Antecedente = value; }
            }
            public String P_Proveedor
            {
                get { return Proveedor; }
                set { Proveedor = value; }
            }
            public String P_Observaciones_Juridicas
            {
                get { return Observaciones_Juridicas; }
                set { Observaciones_Juridicas = value; }
            }
            public String P_No_Contrato_Juridico
            {
                get { return No_Contrato_Juridico; }
                set { No_Contrato_Juridico = value; }
            }
            public String P_No_Registro_Baja_Juridico
            {
                get { return No_Registro_Baja_Juridico; }
                set { No_Registro_Baja_Juridico = value; }
            }
            public String P_No_Escritura_Baja
            {
                get { return No_Escritura_Baja; }
                set { No_Escritura_Baja = value; }
            }
            public DateTime P_Fecha_Escritura_Baja
            {
                get { return Fecha_Escritura_Baja; }
                set { Fecha_Escritura_Baja = value; }
            }
            public DateTime P_Fecha_Baja
            {
                get { return Fecha_Baja; }
                set { Fecha_Baja = value; }
            }
            public String P_No_Notario_Baja
            {
                get { return No_Notario_Baja; }
                set { No_Notario_Baja = value; }
            }
            public String P_Notario_Nombre_Baja
            {
                get { return Notario_Nombre_Baja; }
                set { Notario_Nombre_Baja = value; }
            }
            public String P_Constancia_Registral_Baja
            {
                get { return Constancia_Registral_Baja; }
                set { Constancia_Registral_Baja = value; }
            }
            public String P_Folio_Real_Baja
            {
                get { return Folio_Real_Baja; }
                set { Folio_Real_Baja = value; }
            }
            public String P_Nuevo_Propietario_Juridico
            {
                get { return Nuevo_Propietario_Juridico; }
                set { Nuevo_Propietario_Juridico = value; }
            }
            public String P_No_Contrato_Baja
            {
                get { return No_Contrato_Baja; }
                set { No_Contrato_Baja = value; }
            }

            //Anexos
            public String P_Tipo_Anexo
            {
                get { return Tipo_Anexo; }
                set { Tipo_Anexo = value; }
            }
            public String P_Archivo
            {
                get { return Archivo; }
                set { Archivo = value; }
            }
            public String P_Descripcion_Anexo
            {
                get { return Descripcion_Anexo; }
                set { Descripcion_Anexo = value; }
            }
            public String P_Origen_ID
            {
                get { return Origen_ID; }
                set { Origen_ID = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }

            //Afectaciones
            public DateTime P_Fecha_Afectacion
            {
                get { return Fecha_Afectacion; }
                set { Fecha_Afectacion = value; }
            }
            public String P_Nuevo_Propietario
            {
                get { return Nuevo_Propietario; }
                set { Nuevo_Propietario = value; }
            }
            public String P_Session_Ayuntamiento
            {
                get { return Session_Ayuntamiento; }
                set { Session_Ayuntamiento = value; }
            }
            public String P_Tramo
            {
                get { return Tramo; }
                set { Tramo = value; }
            }
            public String P_No_Contrato
            {
                get { return No_Contrato; }
                set { No_Contrato = value; }
            }

            public Boolean P_Agregar_Afectacion {
                get { return Agregar_Afectacion; }
                set { Agregar_Afectacion = value; }
            }

        //Contabilidad

            public String P_Hoja
            {
                get { return Hoja; }
                set { Hoja = value; }
            }
            public String P_Tomo
            {
                get { return Tomo; }
                set { Tomo = value; }
            }
            public String P_Numero_Acta
            {
                get { return Numero_Acta; }
                set { Numero_Acta = value; }
            }
            public String P_Cartilla_Parcelaria
            {
                get { return Cartilla_Parcelaria; }
                set { Cartilla_Parcelaria = value; }
            }
            public Double P_Superficie_Contable
            {
                get { return Superficie_Contable; }
                set { Superficie_Contable = value; }
            }
            public String P_Unidad_Superficie
            {
                get { return Unidad_Superficie; }
                set { Unidad_Superficie = value; }
            }
            public String P_Clase_Activo_ID
            {
                get { return Clase_Activo_ID; }
                set { Clase_Activo_ID = value; }
            }
            public String P_Tipo_Bien
            {
                get { return Tipo_Bien; }
                set { Tipo_Bien = value; }
            }
            public String P_Registro_Propiedad
            {
                get { return Registro_Propiedad; }
                set { Registro_Propiedad = value; }
            }
            public DateTime P_Fecha_Alta_Cuenta_Publica
            {
                get { return Fecha_Alta_Cuenta_Publica; }
                set { Fecha_Alta_Cuenta_Publica = value; }
            }

        #endregion

        #region "Metodos"

            public Cls_Ope_Pat_Bienes_Inmuebles_Negocio Alta_Bien_Inmueble() {
                return Cls_Ope_Pat_Bienes_Inmuebles_Datos.Alta_Bien_Inmueble(this);
            }

            public void Modifica_Bien_Inmueble() {
                Cls_Ope_Pat_Bienes_Inmuebles_Datos.Modifica_Bien_Inmueble(this);
            }
            
            public DataTable Consultar_Bienes_Inmuebles() {
                return Cls_Ope_Pat_Bienes_Inmuebles_Datos.Consultar_Bienes_Inmuebles(this);
            }

            public Cls_Ope_Pat_Bienes_Inmuebles_Negocio Consultar_Detalles_Bien_Inmueble() {
                return Cls_Ope_Pat_Bienes_Inmuebles_Datos.Consultar_Detalles_Bien_Inmueble(this);
            }

            public Cls_Ope_Pat_Bienes_Inmuebles_Negocio Obtener_Complemento() {
                return Cls_Ope_Pat_Bienes_Inmuebles_Datos.Obtener_Complementos(this);
            }

        #endregion
    
    }

}