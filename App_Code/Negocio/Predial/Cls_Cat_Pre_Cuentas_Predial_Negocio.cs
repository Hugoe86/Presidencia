using System;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Catalogo_Cuentas_Predial.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Cuentas_Predial_Negocio
/// </summary>

namespace Presidencia.Catalogo_Cuentas_Predial.Negocio
{
    public class Cls_Cat_Pre_Cuentas_Predial_Negocio
    {
        public Cls_Cat_Pre_Cuentas_Predial_Negocio()
        {
            Incluir_Propietarios = true;
            Incluir_Copropietarios = true;
        }

        #region Variables Internas

        private String Cuenta_Predial_ID;
        private String Cuenta_Predial;
        private String Calle_ID;
        private String Colonia_ID;
        private String Propietario_ID;
        private String Pro_Propietario_ID;
        private String RFC_Propietario;
        private String Nombre_Propietario;
        private String Nombre_Calle;
        private String Nombre_Colonia;
        private String Copropietario_ID;
        private String Estado_Predio_ID;
        private String Tipo_Predio_ID;
        private String Uso_Suelo_ID;
        private String Tasa_Predial_ID;
        private String Tasa_ID;
        private String Cuota_Minima_ID;
        private String Cuenta_Origen;
        private String Estatus;
        private String No_Exterior;
        private String No_Interior;
        private String RFC;
        private Decimal Superficie_Construida;
        private Decimal Superficie_Total;
        private Decimal Diferencia_Construccion;
        private String Clave_Catastral;
        private Decimal Valor_Fiscal;
        private Decimal Costo_M2;
        private String Efectos;
        private String Periodo_Corriente;
        private Decimal Cuota_Anual;
        private Decimal Porcentaje_Exencion;
        private String Cuota_Fija;
        private DateTime Termino_Exencion;
        private DateTime Fecha_Avaluo;
        private String No_Cuota_Fija;
        private String Calle_ID_Notificacion;
        private String Colonia_ID_Notificacion;
        private String Estado_ID_Notificacion;
        private String Ciudad_ID_Notificacion;
        private String Domicilio_Foraneo;
        private String Calle_Notificacion;
        private String No_Exterior_Notificacion;
        private String Codigo_Postal;
        private String No_Interior_Notificacion;
        private String Colonia_Notificacion;
        private String No_Diferencia;
        private String Estado_Notificacion;
        private String Ciudad_Notificacion;
        private String Usuario;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private Boolean Incluir_Campos_Foraneos;
        private Boolean Incluir_Propietarios;
        private Boolean Incluir_Copropietarios;
        private String Unir_Tablas;
        private String Join;
        private OracleTransaction Trans;
        private OracleCommand Cmmd;
        private String Tipo_Suspencion;
        private String Tipo_Propietario;

        //Campos para Adeudos_Predial
        private String Adeudo_Predial_No_Adeudo;
        private int Adeudo_Predial_Anio = 0;
        private String Adeudo_Predial_Estatus;
        private String Adeudo_Predial_Cuenta_Predial_ID;
        private String Adeudo_Predial_No_Convenio;
        private String Adeudo_Predial_No_Adeudo_Origen;
        private String Adeudo_Predial_No_Descuento;
        private String Adeudo_Predial_Descuento_ID;

        //DataTable con Variación de la Orden
        private DataTable Dt_Variacion_Cuenta;

        #endregion

        #region Variables Publicas

        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }

        public String P_Pro_Propietario_ID
        {
            get { return Pro_Propietario_ID; }
            set { Pro_Propietario_ID = value; }
        }

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }

        public String P_Calle_ID
        {
            get
            {
                return Calle_ID;
            }
            set { Calle_ID = value; }
        }

        public String P_Colonia_ID
        {
            get
            {
                return Colonia_ID;
            }
            set { Colonia_ID = value; }
        }

        public String P_Propietario_ID
        {
            get
            {
                return Propietario_ID;
            }
            set { Propietario_ID = value; }
        }

        public String P_Nombre_Propietario
        {
            get { return Nombre_Propietario; }
            set { Nombre_Propietario = value; }
        }

        public String P_RFC_Propietario
        {
            get { return RFC_Propietario; }
            set { RFC_Propietario = value; }
        }

        public String P_Nombre_Calle
        {
            get { return Nombre_Calle; }
            set { Nombre_Calle = value; }
        }

        public String P_Nombre_Colonia
        {
            get { return Nombre_Colonia; }
            set { Nombre_Colonia = value; }
        }

        public String P_Copropietario_ID
        {
            get
            {
                return Copropietario_ID;
            }
            set { Copropietario_ID = value; }
        }

        public String P_Estado_Predio_ID
        {
            get
            {
                return Estado_Predio_ID;
            }
            set { Estado_Predio_ID = value; }
        }

        public String P_Tipo_Predio_ID
        {
            get
            {
                return Tipo_Predio_ID;
            }
            set { Tipo_Predio_ID = value; }
        }

        public String P_Uso_Suelo_ID
        {
            get
            {
                return Uso_Suelo_ID;
            }
            set { Uso_Suelo_ID = value; }
        }

        public String P_Tasa_ID
        {
            get
            {
                return Tasa_ID;
            }
            set { Tasa_ID = value; }
        }

        public String P_Cuota_Minima_ID
        {
            get
            {
                return Cuota_Minima_ID;
            }
            set { Cuota_Minima_ID = value; }
        }

        public String P_Cuenta_Origen
        {
            get { return Cuenta_Origen; }
            set { Cuenta_Origen = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
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

        public Decimal P_Superficie_Construida
        {
            get { return Superficie_Construida; }
            set { Superficie_Construida = value; }
        }

        public Decimal P_Superficie_Total
        {
            get { return Superficie_Total; }
            set { Superficie_Total = value; }
        }

        public Decimal P_Diferencia_Construccion
        {
            get { return Diferencia_Construccion; }
            set { Diferencia_Construccion = value; }
        }

        public String P_Clave_Catastral
        {
            get { return Clave_Catastral; }
            set { Clave_Catastral = value; }
        }

        public Decimal P_Valor_Fiscal
        {
            get { return Valor_Fiscal; }
            set { Valor_Fiscal = value; }
        }

        public Decimal P_Costo_M2
        {
            get { return Costo_M2; }
            set { Costo_M2 = value; }
        }

        public String P_Efectos
        {
            get { return Efectos; }
            set { Efectos = value; }
        }

        public String P_Periodo_Corriente
        {
            get { return Periodo_Corriente; }
            set { Periodo_Corriente = value; }
        }

        public Decimal P_Cuota_Anual
        {
            get { return Cuota_Anual; }
            set { Cuota_Anual = value; }
        }

        public Decimal P_Porcentaje_Exencion
        {
            get { return Porcentaje_Exencion; }
            set { Porcentaje_Exencion = value; }
        }

        public String P_Cuota_Fija
        {
            get { return Cuota_Fija; }
            set { Cuota_Fija = value; }
        }

        public DateTime P_Termino_Exencion
        {
            get { return Termino_Exencion; }
            set { Termino_Exencion = value; }
        }

        public DateTime P_Fecha_Avaluo
        {
            get { return Fecha_Avaluo; }
            set { Fecha_Avaluo = value; }
        }

        public String P_No_Cuota_Fija
        {
            get { return No_Cuota_Fija; }
            set { No_Cuota_Fija = value; }
        }

        public String P_Calle_ID_Notificacion
        {
            get { return Calle_ID_Notificacion; }
            set { Calle_ID_Notificacion = value; }
        }

        public String P_Colonia_ID_Notificacion
        {
            get { return Colonia_ID_Notificacion; }
            set { Colonia_ID_Notificacion = value; }
        }

        public String P_Estado_ID_Notificacion
        {
            get { return Estado_ID_Notificacion; }
            set { Estado_ID_Notificacion = value; }
        }

        public String P_Ciudad_ID_Notificacion
        {
            get { return Ciudad_ID_Notificacion; }
            set { Ciudad_ID_Notificacion = value; }
        }

        public String P_Domicilio_Foraneo
        {
            get { return Domicilio_Foraneo; }
            set { Domicilio_Foraneo = value; }
        }

        public String P_Calle_Notificacion
        {
            get { return Calle_Notificacion; }
            set { Calle_Notificacion = value; }
        }

        public String P_No_Exterior_Notificacion
        {
            get { return No_Exterior_Notificacion; }
            set { No_Exterior_Notificacion = value; }
        }

        public String P_Codigo_Postal
        {
            get { return Codigo_Postal; }
            set { Codigo_Postal = value; }
        }

        public String P_No_Interior_Notificacion
        {
            get { return No_Interior_Notificacion; }
            set { No_Interior_Notificacion = value; }
        }

        public String P_Colonia_Notificacion
        {
            get { return Colonia_Notificacion; }
            set { Colonia_Notificacion = value; }
        }

        public String P_No_Diferencia
        {
            get { return No_Diferencia; }
            set { No_Diferencia = value; }
        }

        public String P_Tasa_Predial_ID
        {
            get { return Tasa_Predial_ID; }
            set { Tasa_Predial_ID = value; }
        }

        public String P_Estado_Notificacion
        {
            get { return Estado_Notificacion; }
            set { Estado_Notificacion = value; }
        }

        public String P_Ciudad_Notificacion
        {
            get { return Ciudad_Notificacion; }
            set { Ciudad_Notificacion = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Campos_Dinamicos
        {
            get { return Campos_Dinamicos; }
            set { Campos_Dinamicos = value; }
        }

        public String P_Filtros_Dinamicos
        {
            get { return Filtros_Dinamicos; }
            set { Filtros_Dinamicos = value; }
        }

        public String P_Agrupar_Dinamico
        {
            get { return Agrupar_Dinamico; }
            set { Agrupar_Dinamico = value; }
        }

        public String P_Ordenar_Dinamico
        {
            get { return Ordenar_Dinamico; }
            set { Ordenar_Dinamico = value; }
        }

        public Boolean P_Incluir_Campos_Foraneos
        {
            get { return Incluir_Campos_Foraneos; }
            set { Incluir_Campos_Foraneos = value; }
        }

        public Boolean P_Incluir_Propietarios
        {
            get { return Incluir_Propietarios; }
            set { Incluir_Propietarios = value; }
        }

        public Boolean P_Incluir_Copropietarios
        {
            get { return Incluir_Copropietarios; }
            set { Incluir_Copropietarios = value; }
        }

        public String P_Unir_Tablas
        {
            get { return Unir_Tablas; }
            set { Unir_Tablas = value; }
        }

        public String P_Join
        {
            get { return Join; }
            set { Join = value; }
        }

        public OracleTransaction P_Trans
        {
            get { return Trans; }
            set { Trans = value; }
        }

        public OracleCommand P_Cmmd
        {
            get { return Cmmd; }
            set { Cmmd = value; }
        }

        public String P_Tipo_Suspencion
        {
            get { return Tipo_Suspencion; }
            set { Tipo_Suspencion = value; }
        }

        public String P_Tipo_Propietario
        {
            get { return Tipo_Propietario; }
            set { Tipo_Propietario = value; }
        }

        //Propiedades para Adeudos_Predial
        public String P_Adeudo_Predial_No_Adeudo
        {
            get { return Adeudo_Predial_No_Adeudo; }
            set { Adeudo_Predial_No_Adeudo = value; }
        }

        public int P_Adeudo_Predial_Anio
        {
            get { return Adeudo_Predial_Anio; }
            set { Adeudo_Predial_Anio = value; }
        }

        public String P_Adeudo_Predial_Estatus
        {
            get { return Adeudo_Predial_Estatus; }
            set { Adeudo_Predial_Estatus = value; }
        }

        public String P_Adeudo_Predial_Cuenta_Predial_ID
        {
            get { return Adeudo_Predial_Cuenta_Predial_ID; }
            set { Adeudo_Predial_Cuenta_Predial_ID = value; }
        }

        public String P_Adeudo_Predial_No_Convenio
        {
            get { return Adeudo_Predial_No_Convenio; }
            set { Adeudo_Predial_No_Convenio = value; }
        }

        public String P_Adeudo_Predial_No_Adeudo_Origen
        {
            get { return Adeudo_Predial_No_Adeudo_Origen; }
            set { Adeudo_Predial_No_Adeudo_Origen = value; }
        }

        public String P_Adeudo_Predial_No_Descuento
        {
            get { return Adeudo_Predial_No_Descuento; }
            set { Adeudo_Predial_No_Descuento = value; }
        }

        public String P_Adeudo_Predial_Descuento_ID
        {
            get { return Adeudo_Predial_Descuento_ID; }
            set { Adeudo_Predial_Descuento_ID = value; }
        }

        public DataTable P_Dt_Variacion_Cuenta
        {
            get { return Dt_Variacion_Cuenta; }
            set { Dt_Variacion_Cuenta = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Cuenta()
        {
            return Cls_Cat_Pre_Cuentas_Predial_Datos.Alta_Cuenta(this);
        }

        public Boolean Modifcar_Cuenta()
        {
            return Cls_Cat_Pre_Cuentas_Predial_Datos.Modificar_Cuenta(this);
        }

        public Boolean Aplicar_Variacion_Cuenta()
        {
            return Cls_Cat_Pre_Cuentas_Predial_Datos.Aplicar_Variacion_Cuenta(this);
        }

        public Boolean Validar_Estatus_Adeudos()
        {
            return Cls_Cat_Pre_Cuentas_Predial_Datos.Validar_Estatus_Adeudos(this);
        }

        public DataTable Consultar_Cuenta()
        {
            return Cls_Cat_Pre_Cuentas_Predial_Datos.Consultar_Cuentas(this);
        }

        public DataTable Consultar_Datos_Reporte()
        {
            return Cls_Cat_Pre_Cuentas_Predial_Datos.Consultar_Datos_Reporte(this);
        }

        public Boolean Consultar_Cuenta_Existente()
        {
            return Cls_Cat_Pre_Cuentas_Predial_Datos.Consultar_Cuenta_Existente(this);
        }

        public Boolean Eliminar_Cuenta()
        {
            return Cls_Cat_Pre_Cuentas_Predial_Datos.Eliminar_Cuenta(this);
        }

        public Cls_Cat_Pre_Cuentas_Predial_Negocio Consultar_Datos_Propietario()
        {
            return Cls_Cat_Pre_Cuentas_Predial_Datos.Consultar_Datos_Propietario(this);
        }

        #endregion

    }
}