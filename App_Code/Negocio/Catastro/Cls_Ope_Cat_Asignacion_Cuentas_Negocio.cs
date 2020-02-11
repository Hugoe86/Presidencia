using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Operacion_Cat_Asignacion_Cuentas.Datos;

/// <summary>
/// Summary description for Cls_Ope_Cat_Asignacion_Cuentas_Negocio
/// </summary>

namespace Presidencia.Operacion_Cat_Asignacion_Cuentas.Negocio
{
    public class Cls_Ope_Cat_Asignacion_Cuentas_Negocio
    {
        #region Variables Internas

        private String Cuenta_Predial;
        private String Cuenta_Predial_Id;
        private String Estatus;
        private String Perito;
        private String Perito_Interno_Id;
        private String Calle;
        private String Colonia;
        private String No_Asignacion;
        private String No_Ext;
        private String No_Int;
        private String Superficie_Terreno;
        private String Superficie_Terreno_Menor;
        private String Superficie_Construccion;
        private String Superficie_Construccion_Menor;
        private String Efecto_Anio;
        private String Efecto_Bimestre;
        private String Propietario;
        private String Tipo_Predio;
        private Boolean Avaluo;
        private Boolean Anio_Nulo;
        private String No_Entrega;
        private String Folio_Predial;
        private String Anio;
        private String Max_Reg;

        //Cambio de fecha Avaluos
        private String Folio_Inicial;
        private String Folio_Final;
        private String Anio_Avaluo;
        private String Tipo_Reporte;
        private Boolean Con_Movimiento;
        private DateTime Fecha_Avaluo;

        private DataTable Dt_Cuentas;
        private DataTable Dt_Terreno;
        private DataTable Dt_Construccion;
        private DataTable Dt_Totales;

        //Ejercicio fiscal

        private String Anio_Ejercicio_Fiscal;

        #endregion

        #region Variables Publicas

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }

        public String P_Cuenta_Predial_Id
        {
            get { return Cuenta_Predial_Id; }
            set { Cuenta_Predial_Id = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Perito
        {
            get { return Perito; }
            set { Perito = value; }
        }

        public String P_Perito_Interno_Id
        {
            get { return Perito_Interno_Id; }
            set { Perito_Interno_Id = value; }
        }

        public String P_Calle
        {
            get { return Calle; }
            set { Calle = value; }
        }

        public String P_Colonia
        {
            get { return Colonia; }
            set { Colonia = value; }
        }

        public String P_No_Asignacion
        {
            get { return No_Asignacion; }
            set { No_Asignacion = value; }
        }

        public String P_No_Ext
        {
            get { return No_Ext; }
            set { No_Ext = value; }
        }

        public String P_No_Int
        {
            get { return No_Int; }
            set { No_Int = value; }
        }

        public String P_Superficie_Terreno
        {
            get { return Superficie_Terreno; }
            set { Superficie_Terreno = value; }
        }

        public String P_Superficie_Terreno_Menor
        {
            get { return Superficie_Terreno_Menor; }
            set { Superficie_Terreno_Menor = value; }
        }
        public String P_Superficie_Construccion
        {
            get { return Superficie_Construccion; }
            set { Superficie_Construccion = value; }
        }

        public String P_Superficie_Construccion_Menor
        {
            get { return Superficie_Construccion_Menor; }
            set { Superficie_Construccion_Menor = value; }
        }

        public String P_Efecto_Anio
        {
            get { return Efecto_Anio; }
            set { Efecto_Anio = value; }
        }
        public String P_Efecto_Bimestre
        {
            get { return Efecto_Bimestre; }
            set { Efecto_Bimestre = value; }
        }
        public String P_Propietario
        {
            get { return Propietario; }
            set { Propietario = value; }
        }
        public String P_Tipo_Predio
        {
            get { return Tipo_Predio; }
            set { Tipo_Predio = value; }
        }

        public Boolean P_Avaluo
        {
            get { return Avaluo; }
            set { Avaluo = value; }
        }

        public Boolean P_Con_Movimiento
        {
            get { return Con_Movimiento; }
            set { Con_Movimiento = value; }
        }

        public String P_Tipo_Reporte
        {
            get { return Tipo_Reporte; }
            set { Tipo_Reporte = value; }
        }

        public DataTable P_Dt_Cuentas
        {
            get { return Dt_Cuentas; }
            set { Dt_Cuentas = value; }
        }

        public Boolean P_Anio_Nulo
        {
            get { return Anio_Nulo; }
            set { Anio_Nulo = value; }
        }

        public String P_No_Entrega
        {
            get { return No_Entrega; }
            set { No_Entrega = value; }
        }

        public String P_Folio_Predial
        {
            get { return Folio_Predial; }
            set { Folio_Predial = value; }
        }

        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }

        public String P_Max_Reg
        {
            get { return Max_Reg; }
            set { Max_Reg = value; }
        }

        public String P_Folio_Inicial
        {
            get { return Folio_Inicial; }
            set { Folio_Inicial = value; }
        }

        public String P_Folio_Final
        {
            get { return Folio_Final; }
            set { Folio_Final = value; }
        }
        public String P_Anio_Avaluo
        {
            get { return Anio_Avaluo; }
            set { Anio_Avaluo = value; }
        }
        public DateTime P_Fecha_Avaluo
        {
            get { return Fecha_Avaluo; }
            set { Fecha_Avaluo = value; }
        }

        public DataTable P_Dt_Construccion
        {
            get { return Dt_Construccion; }
            set { Dt_Construccion = value; }
        }

        public DataTable P_Dt_Terreno
        {
            get { return Dt_Terreno; }
            set { Dt_Terreno = value; }
        }

        public DataTable P_Dt_Totales
        {
            get { return Dt_Totales; }
            set { Dt_Totales = value; }
        }
        public String P_Anio_Ejercicio_Fiscal
        {
            get { return Anio_Ejercicio_Fiscal; }
            set { Anio_Ejercicio_Fiscal = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Cuenta_Asignada()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Alta_Cuenta_Asignada(this);
        }

        public Boolean Modificar_Cuenta_Asignada()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Modificar_Cuenta_Asignada(this);
        }

        public Boolean Alta_Entregas()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Alta_Entregas(this);
        }

        public DataTable Consultar_Cuentas_Prediales()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultar_Cuentas_Prediales(this);
        }

        public DataTable Consultar_Cuentas_Entregar()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultar_Cuentas_Entregar(this);
        }

        public DataTable Consultar_Cuentas_Asignadas()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultar_Cuentas_Asignadas(this);
        }

        public Boolean Modificar_Avaluos_Fecha()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Modificar_Avaluos_Fecha(this);
        }

        public DataTable Consultar_Cuentas_Determinaciones()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultar_Cuentas_Determinaciones(this);
        }

        public DataTable Consultas_Colonias_Actualizar()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultas_Colonias_Actualizar(this);
        }

        public DataTable Consultar_Predios_Estrategicos()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultar_Predios_Estrategicos(this);
        }
        public DataTable Consultas_Ejercicio_Fiscal()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultas_Ejercicio_Fiscal(this);
        }
        public DataTable Consultas_Ejercicio_Fiscal_Segunda_Entrega()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultas_Ejercicio_Fiscal_Segunda_Entrega(this);
        }

        public DataTable Consultas_Ejercicio_Fiscal_Tercera_Entrega()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultas_Ejercicio_Fiscal_Tercera_Entrega(this);
        }
        public DataTable Consultas_Ejercicio_Fiscal_Cuarta_Entrega()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultas_Ejercicio_Fiscal_Cuarta_Entrega(this);
        }
        public DataTable Consultas_Ejercicio_Fiscal_Quinta_Entrega()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultas_Ejercicio_Fiscal_Quinta_Entrega(this);
        }
        public DataTable Consultas_Ejercicio_Fiscal_Sexta_Entrega()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultas_Ejercicio_Fiscal_Sexta_Entrega(this);
        }
        public DataTable Consultas_Ejercicio_Fiscal_Septima_Entrega()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultas_Ejercicio_Fiscal_Septima_Entrega(this);
        }
        public DataTable Consultas_Avaluos_Asisgnados_Atendidos()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultas_Avaluos_Asisgnados_Atendidos(this);
        }
        public DataTable Consultar_Veces_Rechazo_1()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultar_Veces_Rechazo_1(this);
        }
        public DataTable Consultar_Veces_Rechazo_2()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultar_Veces_Rechazo_2(this);
        }
        public DataTable Consultar_Veces_Rechazo_3()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultar_Veces_Rechazo_3(this);
        }
        public DataTable Consultar_Veces_Rechazo_4()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultar_Veces_Rechazo_4(this);
        }
        public DataTable Consultar_Veces_Rechazo_5()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultar_Veces_Rechazo_5(this);
        }
        public DataTable Consultar_Veces_Rechazo_6()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultar_Veces_Rechazo_6(this);
        }
        public DataTable Consultar_Veces_Rechazo_7()
        {
            return Cls_Ope_Cat_Asignacion_Cuentas_Datos.Consultar_Veces_Rechazo_7(this);
        }

        #endregion
    }
}