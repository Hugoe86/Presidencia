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
using Presidencia.Cat_Parametros_Nomina.Datos;

namespace Presidencia.Cat_Parametros_Nomina.Negocio
{
    public class Cls_Cat_Nom_Parametros_Negocio
    {
        #region(Variables Internas)
        private String Parametro_ID;
        private String Zona_ID;
        private Double Porcentaje_Prima_Vacacional;
        private Double Porcentaje_Fondo_Retiro;
        private Double Porcentaje_Prima_Dominical;
        private String Fecha_Prima_Vacacional_1;
        private String Fecha_Prima_Vacacional_2;

        private String Tipo_IMSS;
        private String Minutos_Dia;
        private String Minutos_Retardo;
        private String ISSEG_Porcentaje_Prevision_Social_Multiple;
        private String ISSEG_Porcentaje_Aplicar_Empleado;

        private String Usuario_Creo;
        private String Usuario_Modifico;
        //Percepciones
        private String Percepcion_Quinquenio;
        private String Percepcion_Prima_Vacacional;
        private String Percepcion_Prima_Dominical;
        private String Percepcion_Aguinaldo;
        private String Percepcion_Dias_Festivos;
        private String Percepcion_Horas_Extra;
        private String Percepcion_Dia_Doble;
        private String Percepcion_Dia_Domingo;
        private String Percepcion_Ajuste_ISR;
        private String Percepcion_Incapacidades;
        private String Percepcion_Subsidio;
        private String Percepcion_Despensa;
        private String Percepcion_Sueldo_Normal;
        private String Percepcion_Prima_Antiguedad;
        private String Percepcion_Indemnizacion;
        private String Percepcion_Vacaciones_Pendientes_Pagar;
        private String Percepcion_Vacaciones;
        private String Percepcion_Fondo_Retiro;
        private String Percepcion_Prevision_Social_Multiple;
        //Deducciones
        private String Deduccion_Faltas;
        private String Deduccion_Retardos;
        private String Deduccion_Fondo_Retiro;
        private String Deduccion_ISR;
        private Double Salario_Limite_Prestamo;
        private String Deduccion_IMSS;
        private String Deduccion_Vacaciones_Tomadas_Mas;
        private String Deduccion_Aguinaldo_Pagado_Mas;
        private String Deduccion_Prima_Vac_Pagada_Mas;
        private String Deduccion_Sueldo_Pagado_Mas;
        private String Deduccion_Tipo_Desc_Orden_Judicial;
        private String Deduccion_Orden_Judicial_Aguinaldo;
        private String Deduccion_Orden_Judicial_Prima_Vacacional;
        private String Deduccion_Orden_Judicial_Indemnizacion;
        private String Deduccion_ISSEG;
        //Calculo de IMSS
        private Double Salario_Mensual_Maximo;
        private Double Salario_Diario_Integrado_Topado;
        //Reloj Checador
        private String IP_Servidor;
        private String Nombre_Base_Datos;
        private String Usuario_SQL;
        private String Password_Base_Datos;
        private String Dias_IMSS;
        private String Tope_ISSEG;
        private String Proveedor_Fonacot;
        #endregion

        #region(Variables Publicas)
        public String P_Parametro_ID
        {
            get { return Parametro_ID; }
            set { Parametro_ID = value; }
        }
        public String P_Zona_ID
        {
            get { return Zona_ID; }
            set { Zona_ID = value; }
        }
        public Double P_Porcentaje_Prima_Vacacional
        {
            get { return Porcentaje_Prima_Vacacional; }
            set { Porcentaje_Prima_Vacacional = value; }
        }
        public Double P_Porcentaje_Fondo_Retiro
        {
            get { return Porcentaje_Fondo_Retiro; }
            set { Porcentaje_Fondo_Retiro = value; }
        }
        public Double P_Porcentaje_Prima_Dominical
        {
            get { return Porcentaje_Prima_Dominical; }
            set { Porcentaje_Prima_Dominical = value; }
        }
        public String P_Fecha_Prima_Vacacional_1
        {
            get { return Fecha_Prima_Vacacional_1; }
            set { Fecha_Prima_Vacacional_1 = value; }
        }
        public String P_Fecha_Prima_Vacacional_2
        {
            get { return Fecha_Prima_Vacacional_2; }
            set { Fecha_Prima_Vacacional_2 = value; }
        }
        public String P_Tipo_IMSS
        {
            get { return Tipo_IMSS; }
            set { Tipo_IMSS = value; }
        }
        public String P_Minutos_Dia
        {
            get { return Minutos_Dia; }
            set { Minutos_Dia = value; }
        }
        public String P_Minutos_Retardo {
            get { return Minutos_Retardo; }
            set { Minutos_Retardo = value; }
        }
        public String P_ISSEG_Porcentaje_Prevision_Social_Multiple {
            get { return ISSEG_Porcentaje_Prevision_Social_Multiple; }
            set { ISSEG_Porcentaje_Prevision_Social_Multiple = value; }
        }
        public String P_ISSEG_Porcentaje_Aplicar_Empleado {
            get { return ISSEG_Porcentaje_Aplicar_Empleado; }
            set { ISSEG_Porcentaje_Aplicar_Empleado = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }

        public String P_Percepcion_Quinquenio
        {
            get { return Percepcion_Quinquenio; }
            set { Percepcion_Quinquenio = value; }
        }
        public String P_Percepcion_Prima_Vacacional
        {
            get { return Percepcion_Prima_Vacacional; }
            set { Percepcion_Prima_Vacacional = value; }
        }
        public String P_Percepcion_Prima_Dominical
        {
            get { return Percepcion_Prima_Dominical; }
            set { Percepcion_Prima_Dominical = value; }
        }
        public String P_Percepcion_Aguinaldo
        {
            get { return Percepcion_Aguinaldo; }
            set { Percepcion_Aguinaldo = value; }
        }
        public String P_Percepcion_Dias_Festivos
        {
            get { return Percepcion_Dias_Festivos; }
            set { Percepcion_Dias_Festivos = value; }
        }
        public String P_Percepcion_Horas_Extra
        {
            get { return Percepcion_Horas_Extra; }
            set { Percepcion_Horas_Extra = value; }
        }
        public String P_Percepcion_Dia_Doble
        {
            get { return Percepcion_Dia_Doble; }
            set { Percepcion_Dia_Doble = value; }
        }
        public String P_Percepcion_Dia_Domingo
        {
            get { return Percepcion_Dia_Domingo; }
            set { Percepcion_Dia_Domingo = value; }
        }
        public String P_Percepcion_Ajuste_ISR
        {
            get { return Percepcion_Ajuste_ISR; }
            set { Percepcion_Ajuste_ISR = value; }
        }
        public String P_Percepcion_Incapacidades
        {
            get { return Percepcion_Incapacidades; }
            set { Percepcion_Incapacidades = value; }
        }
        public String P_Percepcion_Subsidio
        {
            get { return Percepcion_Subsidio; }
            set { Percepcion_Subsidio = value; }
        }
        public String P_Percepcion_Despensa
        {
            get { return Percepcion_Despensa; }
            set { Percepcion_Despensa = value; }
        }
        public String P_Percepcion_Sueldo_Normal
        {
            get { return Percepcion_Sueldo_Normal; }
            set { Percepcion_Sueldo_Normal = value; }
        }

        public String P_Percepcion_Prima_Antiguedad {
            get { return Percepcion_Prima_Antiguedad; }
            set { Percepcion_Prima_Antiguedad = value; }
        }

        public String P_Percepcion_Indemnizacion {
            get { return Percepcion_Indemnizacion; }
            set { Percepcion_Indemnizacion = value; }
        }

        public String P_Percepcion_Vacaciones_Pendientes_Pagar {
            get { return Percepcion_Vacaciones_Pendientes_Pagar; }
            set { Percepcion_Vacaciones_Pendientes_Pagar = value; }
        }

        public String P_Percepcion_Vacaciones {
            get { return Percepcion_Vacaciones; }
            set { Percepcion_Vacaciones = value; }
        }

        public String P_Percepcion_Prevision_Social_Multiple
        {
            get { return Percepcion_Prevision_Social_Multiple; }
            set { Percepcion_Prevision_Social_Multiple = value; }
        }

        public String P_Percepcion_Fondo_Retiro
        {
            get { return Percepcion_Fondo_Retiro; }
            set { Percepcion_Fondo_Retiro = value; }
        }

        public String P_Deduccion_Faltas
        {
            get { return Deduccion_Faltas; }
            set { Deduccion_Faltas = value; }
        }
        public String P_Deduccion_Retardos
        {
            get { return Deduccion_Retardos; }
            set { Deduccion_Retardos = value; }
        }
        public String P_Deduccion_Fondo_Retiro
        {
            get { return Deduccion_Fondo_Retiro; }
            set { Deduccion_Fondo_Retiro = value; }
        }
        public String P_Deduccion_ISR
        {
            get { return Deduccion_ISR; }
            set { Deduccion_ISR = value; }
        }
        public String P_Deduccion_Tipo_Desc_Orden_Judicial
        {
            get { return Deduccion_Tipo_Desc_Orden_Judicial; }
            set { Deduccion_Tipo_Desc_Orden_Judicial = value; }
        }

        public Double P_Salario_Limite_Prestamo {
            get { return Salario_Limite_Prestamo; }
            set { Salario_Limite_Prestamo = value; }
        }
        public Double P_Salario_Mensual_Maximo
        {
            get { return Salario_Mensual_Maximo; }
            set { Salario_Mensual_Maximo = value; }
        }
        public Double P_Salario_Diario_Integrado_Topado
        {
            get { return Salario_Diario_Integrado_Topado; }
            set { Salario_Diario_Integrado_Topado = value; }
        }

        public String P_Deduccion_IMSS {
            get { return Deduccion_IMSS; }
            set { Deduccion_IMSS = value; }
        }

        public String P_Deduccion_Vacaciones_Tomadas_Mas {
            get { return Deduccion_Vacaciones_Tomadas_Mas; }
            set { Deduccion_Vacaciones_Tomadas_Mas = value; }
        }

        public String P_Deduccion_Aguinaldo_Pagado_Mas {
            get { return Deduccion_Aguinaldo_Pagado_Mas; }
            set { Deduccion_Aguinaldo_Pagado_Mas = value; }
        }

        public String P_Deduccion_Prima_Vac_Pagada_Mas
        {
            get { return Deduccion_Prima_Vac_Pagada_Mas; }
            set { Deduccion_Prima_Vac_Pagada_Mas = value; }
        }

        public String P_Deduccion_Sueldo_Pagado_Mas
        {
            get { return Deduccion_Sueldo_Pagado_Mas; }
            set { Deduccion_Sueldo_Pagado_Mas = value; }
        }
        public String P_Deduccion_ISSEG
        {
            get { return Deduccion_ISSEG; }
            set { Deduccion_ISSEG = value; }
        }
        public String P_IP_Servidor
        {
            get { return IP_Servidor; }
            set { IP_Servidor = value; }
        }
        public String P_Nombre_Base_Datos
        {
            get { return Nombre_Base_Datos; }
            set { Nombre_Base_Datos = value; }
        }
        public String P_Usuario_SQL
        {
            get { return Usuario_SQL; }
            set { Usuario_SQL = value; }
        }
        public String P_Password_Base_Datos
        {
            get { return Password_Base_Datos; }
            set { Password_Base_Datos = value; }
        }
        public String P_Deduccion_Orden_Judicial_Aguinaldo
        {
            get { return Deduccion_Orden_Judicial_Aguinaldo; }
            set { Deduccion_Orden_Judicial_Aguinaldo = value; }
        }
        public String P_Deduccion_Orden_Judicial_Prima_Vacacional
        {
            get { return Deduccion_Orden_Judicial_Prima_Vacacional; }
            set { Deduccion_Orden_Judicial_Prima_Vacacional = value; }
        }
        public String P_Deduccion_Orden_Judicial_Indemnizacion
        {
            get { return Deduccion_Orden_Judicial_Indemnizacion; }
            set { Deduccion_Orden_Judicial_Indemnizacion = value; }
        }

        public String P_Dias_IMSS {
            get { return Dias_IMSS; }
            set { Dias_IMSS = value; }
        }

        public String P_Tope_ISSEG
        {
            get { return Tope_ISSEG; }
            set { Tope_ISSEG = value; }
        }

        public String P_Proveedor_Fonacot {
            get { return Proveedor_Fonacot; }
            set { Proveedor_Fonacot = value; }
        }
        #endregion

        #region(Metodos)
       public Boolean Alta_Parametro() {
           return Cls_Cat_Nom_Parametros_Datos.Alta_Parametro_Nomina(this);
       }
       public Boolean Modificar_Parametro()
       {
           return Cls_Cat_Nom_Parametros_Datos.Modificar_Parametro_Nomina(this);
       }
       public Boolean Eliminar_Parametro()
       {
           return Cls_Cat_Nom_Parametros_Datos.Elimnar_Parametro_Nomina(this);
       }
       public DataTable Consulta_Parametros()
       {
           return Cls_Cat_Nom_Parametros_Datos.Consultar_Parametros_Nomina(this);
       }
        #endregion
    }
}