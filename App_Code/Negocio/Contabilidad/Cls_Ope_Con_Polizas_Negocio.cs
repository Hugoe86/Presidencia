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
using System.Collections.Generic;
using Presidencia.Polizas.Datos;
using System.Data.OracleClient;

namespace Presidencia.Polizas.Negocios
{
    public class Cls_Ope_Con_Polizas_Negocio
    {
        #region (Variables Internas)
            #region (TABLA POLIZAS)
            private String No_Poliza;
            private String Tipo_Poliza_ID;
            private String Empleado_ID;
            private String Cuenta_Contable_ID;
            private String Mes_Ano;
            private DateTime Fecha_Poliza;
            private String Concepto;
            private Double Total_Haber;
            private Double Total_Debe;
            private Int32 No_Partida;
            private DataTable Dt_Detalles_Polizas;
            private String Nombre_Usuario;
            private String Empleado_ID_Creo;
            private String Empleado_ID_Autorizo;
            private String Mes_Inicio;
            private String Mes_Fin;
            #endregion
            #region (TABLA EMPLEADOS)
            private String Nombre;
            #endregion
            #region (TABLA PARTIDAS ESPECIFICAS)
            private string Partida_ID;
            private string Clave;
            private string Cuenta;
            #endregion
            #region (TABLA AREA FUNCIONAL)
            private string Area_Funcional_ID;
            #endregion
            #region (TABLA DEPENDENCIAS)
            private string Dependencia_ID;
            #endregion
            #region (TABLA PRESUPUESTO)
                private string Fuente_Financiamiento_ID;
                private string Programa_ID;
                private string Disponible;
                private string Comprometido;
            #endregion
                #region tabla ingresos
                private string Anio;
                #endregion
                #region Claves_Carga_Masiva
                private String Clave_Fte_Financiamiento;
                private String Clave_Dependencia;
                private String Clave_Programa;
                private String Clave_Cuenta_Contable;
                #endregion
                private String Momento;
                private String Momento_Final;
                private String No_Reserva;
                private String No_Solicitud_Pago;
                private String Referencia;
                private String Fecha_Inicial;
                private String Fecha_Final;
                private String Validar_Saldo;
                private String Capitulo;
                private OracleCommand Cmmd;

        #endregion

                #region (Variables Publicas)
                #region (TABLA POLIZAS)
                public String P_Empleado_ID_Creo
            {
                get { return Empleado_ID_Creo; }
                set { Empleado_ID_Creo = value; }
            }
                public String P_Referencia
                {
                    get { return Referencia; }
                    set { Referencia = value; }
                }
                public String P_Fecha_Inicial
                {
                    get { return Fecha_Inicial; }
                    set { Fecha_Inicial = value; }
                }
                public String P_Fecha_Final
                {
                    get { return Fecha_Final; }
                    set { Fecha_Final = value; }
                }
            public String P_Mes_Inicio
            {
                get { return Mes_Inicio; }
                set { Mes_Inicio = value; }
            }
            public String P_Mes_Fin
            {
                get { return Mes_Fin; }
                set { Mes_Fin = value; }
            }
            public String P_Cuenta_Contable_ID
            {
                get { return Cuenta_Contable_ID; }
                set { Cuenta_Contable_ID = value; }
            }
            public String P_Empleado_ID_Autorizo
            {
                get { return Empleado_ID_Autorizo; }
                set { Empleado_ID_Autorizo = value; }
            }
            public String P_No_Poliza
            {
                get { return No_Poliza; }
                set { No_Poliza = value; }
            }
            public String P_Tipo_Poliza_ID
            {
                get { return Tipo_Poliza_ID; }
                set { Tipo_Poliza_ID = value; }
            }            
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_Mes_Ano
            {
                get { return Mes_Ano; }
                set { Mes_Ano = value; }
            }            
            public DateTime P_Fecha_Poliza
            {
                get { return Fecha_Poliza; }
                set { Fecha_Poliza = value; }
            }
            public String P_Concepto
            {
                get { return Concepto; }
                set { Concepto = value; }
            }            
            public Double P_Total_Haber
            {
                get { return Total_Haber; }
                set { Total_Haber = value; }
            }            
            public Double P_Total_Debe
            {
                get { return Total_Debe; }
                set { Total_Debe = value; }
            }
            public Int32 P_No_Partida
            {
                get { return No_Partida; }
                set { No_Partida = value; }
            }        
            public DataTable P_Dt_Detalles_Polizas
            {
                get { return Dt_Detalles_Polizas; }
                set { Dt_Detalles_Polizas = value; }
            }
            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }
        #endregion

            #region (TABLA EMPLEADOS)
            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }
            #endregion

            #region (TABLA PARTIDAS ESPECIFICAS)
                public string P_Partida_ID
                {
                    get { return Partida_ID; }
                    set { Partida_ID = value; }
                }
                public string P_Cuenta
                {
                    get { return Cuenta; }
                    set { Cuenta = value; }
                }
                public string P_Clave
                {
                    get { return Clave; }
                    set { Clave = value; }
                }
            #endregion

            #region (TABLA AREA FUNCIONAL)
                public string P_Area_Funcional_ID
                {
                    get { return Area_Funcional_ID; }
                    set { Area_Funcional_ID = value; }
                }
            #endregion

            #region (TABLA PRESUPUESTO)
                public string P_Dependencia_ID
                {
                    get { return Dependencia_ID; }
                    set { Dependencia_ID = value; }
                }
                public string P_Fuente_Financiamiento_ID
                {
                    get { return Fuente_Financiamiento_ID; }
                    set { Fuente_Financiamiento_ID = value; }
                }
                public string P_Programa_ID
                {
                    get { return Programa_ID; }
                    set { Programa_ID = value; }
                }
                public string P_Disponible
                {
                    get { return Disponible; }
                    set { Disponible = value; }
                }
                public string P_Comprometido
                {
                    get { return Comprometido; }
                    set { Comprometido = value; }
                }
            #endregion

                #region ingresos
                public string P_Anio
                {
                    get { return Anio; }
                    set { Anio = value; }
                }
                #endregion

                #region Claves_Carga_Masiva
                public String P_Clave_Fte_Financiamiento
                {
                    get { return Clave_Fte_Financiamiento; }
                    set { Clave_Fte_Financiamiento = value; }
                }
                public String P_Clave_Dependencia
                {
                    get { return Clave_Dependencia; }
                    set { Clave_Dependencia = value; }
                }
                public String P_Clave_Programa
                {
                    get { return Clave_Programa; }
                    set { Clave_Programa = value; }
                }
                public String P_Clave_Cuenta_Contable
                {
                    get { return Clave_Cuenta_Contable; }
                    set { Clave_Cuenta_Contable = value; }
                }
                #endregion
                public string P_Momento
                {
                    get { return Momento; }
                    set { Momento = value; }
                }
                public String P_Momento_Final
                {
                    get { return Momento_Final; }
                    set { Momento_Final = value; }
                }
                public String P_No_Reserva
                {
                    get { return No_Reserva; }
                    set { No_Reserva = value; }
                }
                public String P_No_Solicitud_Pago
                {
                    get { return No_Solicitud_Pago; }
                    set { No_Solicitud_Pago = value; }
                }
                public String P_Validar_Saldo
                {
                    get { return Validar_Saldo; }
                    set { Validar_Saldo = value; }
                }
                public String P_Capitulo
                {
                    get { return Capitulo; }
                    set { Capitulo = value; }
                }
                public OracleCommand P_Cmmd
                {
                    get { return Cmmd; }
                    set { Cmmd = value; }
                }

                #endregion

                #region (Metodos)
                #region (TABLA POLIZAS)
        public string[] Alta_Poliza()
        {
            return Cls_Ope_Con_Polizas_Datos.Alta_Poliza(this);
        }
        public void Modificar_Polizas()
        {
            Cls_Ope_Con_Polizas_Datos.Modificar_Polizas(this);
        }
        public void Eliminar_Poliza()
        {
            Cls_Ope_Con_Polizas_Datos.Eliminar_Poliza(this);
        }
        public DataTable Consulta_Poliza()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Poliza(this);
        }
        public DataTable Consulta_Detalles_Poliza()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Detalles_Poliza(this);
        }
        public DataTable Consulta_Poliza_Popup()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Poliza_Popup(this);
        }
        public DataTable Consulta_Detalles_Poliza_Por_Referencia()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Detalles_Poliza_Por_Referencia(this);
        }
        public DataTable Consulta_Detalles_Poliza_Seleccionada()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Detalles_Poliza_Seleccionada(this);
        }
        public DataTable Consulta_Detalles_Empleado_Aprobo()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Detalles_Empleado_Aprobo(this);
        }
        public DataTable Consulta_Detalles_Poliza_Cuenta_Contable()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Detalles_Poliza_Cuenta_Contable(this);
        }
        public DataTable Consulta_GrupoRol()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_GrupoRol(this);
        }
        public DataTable Consulta_Detalles_Empleado_Creo()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Detalles_Empleado_Creo(this);
        }
        public DataTable Consulta_Detalle_Poliza()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Detalle_Poliza(this);
        }        
        public Boolean Consulta_Fecha_Poliza()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Fecha_Poliza(this);
        }
        #endregion
        #region (TABLA EMPLEADOS)
        public DataTable Consulta_Empleados_Especial()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Empleados_Especial(this);
        }
        public DataTable Consulta_Empleado_Jefe_Dependencia()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Empleado_Jefe_Dependencia(this);
        }
        #endregion
        #region (TABLA PARTIDAS ESPECIFICAS)
        public DataTable Consulta_Partida_Especifica()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Partida_Especifica(this);
        }
        #endregion
        #region (TABLA AREA FUNCIONAL)
        public DataTable Consulta_Area_Funcional_Especial()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Area_Funcional_Especial(this);
        }
        #endregion
        #region (TABLA PROYECTOS PROGRAMAS)
        public DataTable Consulta_Programas_Especial()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Programas_Especial();
        }
        #endregion
        #region (TABLA PRESUPUESTOS)
        public void Actualizar_Montos_Presupuesto()
        {
            Cls_Ope_Con_Polizas_Datos.Actualizar_Montos_Presupuesto(this);
        }
        public DataTable Consulta_Dependencia_Partida_ID()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Dependencia_Partida_ID(this);
        }
        public DataTable Consulta_Fte_Area_Funcional_ID()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Fte_Area_Funcional_ID(this);
        }
        public DataTable Consulta_Dependencia_Programa_ID()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Dependencia_Programa_ID(this);
        }
        public DataTable Consulta_Programa_Fuente_ID()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Programa_Fuente_ID(this);
        }
        #endregion
        #region INGRESOS
        public DataTable Consulta_Fuente_Financiamiento()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Fuente_Financiamiento(this);
        }
        public DataTable Consulta_Programas_Ing()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Programas_Ing(this);
        }
        public DataTable Consulta_Cuentas_Contables_De_Conceptos()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Cuentas_Contables_De_Conceptos(this);
        }
        
        #endregion
        #region EGRESOS
        public DataTable Consulta_Fuente_Financiamiento_Egr()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Fuente_Financiamiento_Egr(this);
        }
        public DataTable Consulta_Dependencia()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Dependencia(this);
        }
        public DataTable Consulta_Programas()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Programas(this);
        }
        #endregion
        #region Claves_Carga_Masiva

        public DataTable Consulta_Cuenta_Partida_ID()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Cuenta_Partida_ID(this);
        }
        public DataTable Consulta_ID_Fte_Financiamiento()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_ID_Fte_Financiamiento(this);
        }
        public DataTable Consulta_ID_Fte_Financiamiento_Egr()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_ID_Fte_Financiamiento_Egr(this);
        }
        public Boolean Consulta_Saldo_Disponible_Poliza()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Saldo_Disponible_Poliza(this);
        }
        #endregion

        //se utiliza en el seguimiento de reserva
        public DataTable Consulta_Solicitud_Pago()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Solicitud_Pago(this);
        }
        public DataTable Consulta_Pago()
        {
            return Cls_Ope_Con_Polizas_Datos.Consulta_Pago(this);
        }
        #endregion
    }	
}
