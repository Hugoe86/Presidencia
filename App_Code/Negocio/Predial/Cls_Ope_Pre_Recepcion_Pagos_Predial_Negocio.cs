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
using Presidencia.Operacion_Predial_Recepcion_Pagos.Datos;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Catalogo_Tabulador_Recargos.Negocio;
using System.Collections.Generic;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Caja_Pagos.Negocio;
using Presidencia.Sessiones;

/// <summary>
/// Summary description for Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio
/// </summary>
namespace Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio
{

    public class Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio
    {

        #region "Variables Internas"

        private String Cuenta_Predial = null;
        private String Cuenta_Predial_ID = null;
        private String Clave_Ingreso = null;
        private String Tipo_Clave_Ingreso = null;
        private String Descripcion = null;
        private DateTime Fecha_Tramite;
        private DateTime Fecha_Vencimiento;
        private Double Monto = 0.0;
        private String Estatus = null;
        private String No_Recibo = null;
        private String Dependencia = null;
        private String Usuario = null;
        private Int32 Anio_Filtro = 0;
        private Int32 Bimestre_Filtro = 0;
        private String No_Convenio = null;
        private String No_Pagos = null;
        private String No_Descuento = null;
        private Boolean Convenio_Incumplido;
        private String Contribuyente = null;
        #endregion

        #region "Variables Publicas"

        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }
        public String P_Clave_Ingreso
        {
            get { return Clave_Ingreso; }
            set { Clave_Ingreso = value; }
        }
        public String P_Tipo_Clave_Ingreso
        {
            get { return Tipo_Clave_Ingreso; }
            set { Tipo_Clave_Ingreso = value; }
        }
        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }
        public DateTime P_Fecha_Tramite
        {
            get { return Fecha_Tramite; }
            set { Fecha_Tramite = value; }
        }
        public DateTime P_Fecha_Vencimiento
        {
            get { return Fecha_Vencimiento; }
            set { Fecha_Vencimiento = value; }
        }
        public Double P_Monto
        {
            get { return Monto; }
            set { Monto = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_No_Recibo
        {
            get { return No_Recibo; }
            set { No_Recibo = value; }
        }
        public String P_Dependencia
        {
            get { return Dependencia; }
            set { Dependencia = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        public Int32 P_Anio_Filtro
        {
            get { return Anio_Filtro; }
            set { Anio_Filtro = value; }
        }
        public Int32 P_Bimestre_Filtro
        {
            get { return Bimestre_Filtro; }
            set { Bimestre_Filtro = value; }
        }
        public String P_No_Convenio
        {
            get { return No_Convenio; }
            set { No_Convenio = value; }
        }
        public String P_No_Pagos
        {
            get { return No_Pagos; }
            set { No_Pagos = value; }
        }
        public String P_No_Descuento
        {
            get { return No_Descuento; }
            set { No_Descuento = value; }
        }
        public Boolean P_Convenio_Incumplido
        {
            get { return Convenio_Incumplido; }
            set { Convenio_Incumplido = value; }
        }
        public String P_Contribuyente
        {
            get { return Contribuyente; }
            set { Contribuyente = value; }
        }
        #endregion

        #region "Metodos"

        public DataTable Consultar_Cuentas_Predial()
        {
            return Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos.Consultar_Cuentas_Predial(this);
        }
        public DataTable Consultar_Adeudos_Cuentas_Predial()
        {
            DataTable Dt_Estructura = new DataTable();
            Dt_Estructura.Columns.Add("NO_ADEUDO", typeof(string));
            Dt_Estructura.Columns.Add("CONCEPTO", typeof(string));
            Dt_Estructura.Columns.Add("ANIO", typeof(int));
            Dt_Estructura.Columns.Add("BIMESTRE", typeof(int));
            Dt_Estructura.Columns.Add("DESCUENTO", typeof(double));
            Dt_Estructura.Columns.Add("ESTATUS", typeof(string));
            Dt_Estructura.Columns.Add("CORRIENTE", typeof(double));
            Dt_Estructura.Columns.Add("REZAGOS", typeof(double));
            Dt_Estructura.Columns.Add("RECARGOS_ORDINARIOS", typeof(double));
            Dt_Estructura.Columns.Add("RECARGOS_MORATORIOS", typeof(double));
            Dt_Estructura.Columns.Add("HONORARIOS", typeof(double));
            Dt_Estructura.Columns.Add("MULTAS", typeof(double));
            Dt_Estructura.Columns.Add("DESCUENTOS", typeof(double));
            Dt_Estructura.Columns.Add("MONTO", typeof(double));

            Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio GAP_Negocio = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
            DataTable dt = GAP_Negocio.Calcular_Recargos_Predial(P_Cuenta_Predial);
            String Perido_Actual = GAP_Negocio.p_Periodo_Corriente;
            String[] Periodo_Actual_Tmp = new String[2];
            Periodo_Actual_Tmp = (Perido_Actual.Trim() + "  0-0").Replace(" ", "*").Split('*');
            Int32 Anio_Periodo_Actual_1 = Convert.ToInt32((Periodo_Actual_Tmp[0].Trim().Split('-'))[1].Trim());
            Int32 Anio_Periodo_Actual_2 = Convert.ToInt32((Periodo_Actual_Tmp[2].Trim().Split('-'))[1].Trim());
            Int32 Bimestre_Periodo_Actual_1 = Convert.ToInt32((Periodo_Actual_Tmp[0].Trim().Split('-'))[0].Trim());
            Int32 Bimestre_Periodo_Actual_2 = Convert.ToInt32((Periodo_Actual_Tmp[2].Trim().Split('-'))[0].Trim());

            //Se sacan los valores de los recargos
            Int32 Mes_Actual = DateTime.Now.Month;
            Int32 Anio_Actual = DateTime.Now.Year;
            Dictionary<String, Decimal> Dicc_Tabulador_recargos = new Dictionary<String, Decimal>();
            Cls_Cat_Pre_Tabulador_Recargos_Negocio Rs_Recargos_Cuentas = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
            Dicc_Tabulador_recargos = Rs_Recargos_Cuentas.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Actual);

            //Se crea la Tabla con los Adeudos
            DataTable Dt_Listado_Adeudos = Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos.Consultar_Adeudos_Cuenta_Predial(this);
            for (Int32 Contador = 0; Contador < Dt_Listado_Adeudos.Rows.Count; Contador++)
            {
                //SACA EL REGISTRO DEL PRIMER BIMESTRE
                if (!string.IsNullOrEmpty(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_1"].ToString().Trim()))
                {
                    if (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_1"]) != 0)
                    {
                        Int32 Bimestre = 1;
                        Boolean Entra_Filtrado = true;
                        if (Anio_Filtro > 0)
                        {
                            if (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) < Anio_Filtro)
                            {
                                Entra_Filtrado = true;
                            }
                            else
                            {
                                if (Bimestre_Filtro > 0)
                                {
                                    if (Bimestre <= Bimestre_Filtro)
                                    {
                                        Entra_Filtrado = true;
                                    }
                                    else
                                    {
                                        Entra_Filtrado = false;
                                    }
                                }
                            }
                        }
                        if (Entra_Filtrado)
                        {
                            String Tipo_Cargo = "REZAGO";
                            if ((Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) == Anio_Periodo_Actual_1) || (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) == Anio_Periodo_Actual_2))
                            {
                                //if ((Bimestre_Periodo_Actual_1 == Bimestre) || (Bimestre_Periodo_Actual_2 == Bimestre)) {
                                Tipo_Cargo = "CORRIENTE";
                                //}
                            }
                            DataRow Dr_Fila_Adeudo_Bimestre_1 = Dt_Estructura.NewRow();
                            Dr_Fila_Adeudo_Bimestre_1["NO_ADEUDO"] = Dt_Listado_Adeudos.Rows[Contador]["NO_ADEUDO"].ToString();
                            Dr_Fila_Adeudo_Bimestre_1["CONCEPTO"] = Tipo_Cargo;
                            Dr_Fila_Adeudo_Bimestre_1["ANIO"] = Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString());
                            Dr_Fila_Adeudo_Bimestre_1["BIMESTRE"] = Bimestre;
                            Dr_Fila_Adeudo_Bimestre_1["DESCUENTO"] = 0;
                            Dr_Fila_Adeudo_Bimestre_1["ESTATUS"] = "POR PAGAR";
                            if (Tipo_Cargo.Trim().Equals("REZAGO"))
                            {
                                Dr_Fila_Adeudo_Bimestre_1["CORRIENTE"] = 0;
                                Dr_Fila_Adeudo_Bimestre_1["REZAGOS"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_1"]);
                            }
                            else
                            {
                                Dr_Fila_Adeudo_Bimestre_1["CORRIENTE"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_1"]);
                                Dr_Fila_Adeudo_Bimestre_1["REZAGOS"] = 0;
                            }
                            if (Dicc_Tabulador_recargos.ContainsKey(Bimestre.ToString() + Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()))
                            {
                                Dr_Fila_Adeudo_Bimestre_1["RECARGOS_ORDINARIOS"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_1"]) * ((Convert.ToDouble(Dicc_Tabulador_recargos[Bimestre.ToString() + Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()])) / 100);
                            }
                            else
                            {
                                Dr_Fila_Adeudo_Bimestre_1["RECARGOS_ORDINARIOS"] = 0;
                            }
                            Dr_Fila_Adeudo_Bimestre_1["RECARGOS_MORATORIOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_1["HONORARIOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_1["MULTAS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_1["DESCUENTOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_1["MONTO"] = Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_1["CORRIENTE"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_1["REZAGOS"]) +
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_1["RECARGOS_ORDINARIOS"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_1["RECARGOS_MORATORIOS"]) +
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_1["HONORARIOS"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_1["MULTAS"]) -
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_1["DESCUENTOS"]);
                            Dt_Estructura.Rows.Add(Dr_Fila_Adeudo_Bimestre_1);
                        }
                    }
                }
                //SACA EL REGISTRO DEL SEGUNDO BIMESTRE
                if (!string.IsNullOrEmpty(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_2"].ToString().Trim()))
                {
                    if (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_2"]) != 0)
                    {
                        Int32 Bimestre = 2;
                        Boolean Entra_Filtrado = true;
                        if (Anio_Filtro > 0)
                        {
                            if (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) < Anio_Filtro)
                            {
                                Entra_Filtrado = true;
                            }
                            else
                            {
                                if (Bimestre_Filtro > 0)
                                {
                                    if (Bimestre <= Bimestre_Filtro)
                                    {
                                        Entra_Filtrado = true;
                                    }
                                    else
                                    {
                                        Entra_Filtrado = false;
                                    }
                                }
                            }
                        }
                        if (Entra_Filtrado)
                        {
                            String Tipo_Cargo = "REZAGO";
                            if ((Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) == Anio_Periodo_Actual_1) || (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) == Anio_Periodo_Actual_2))
                            {
                                //if ((Bimestre_Periodo_Actual_1 == Bimestre) || (Bimestre_Periodo_Actual_2 == Bimestre)) {
                                Tipo_Cargo = "CORRIENTE";
                                //}
                            }
                            DataRow Dr_Fila_Adeudo_Bimestre_2 = Dt_Estructura.NewRow();
                            Dr_Fila_Adeudo_Bimestre_2["NO_ADEUDO"] = Dt_Listado_Adeudos.Rows[Contador]["NO_ADEUDO"].ToString();
                            Dr_Fila_Adeudo_Bimestre_2["CONCEPTO"] = Tipo_Cargo;
                            Dr_Fila_Adeudo_Bimestre_2["ANIO"] = Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString());
                            Dr_Fila_Adeudo_Bimestre_2["BIMESTRE"] = Bimestre;
                            Dr_Fila_Adeudo_Bimestre_2["DESCUENTO"] = 0;
                            Dr_Fila_Adeudo_Bimestre_2["ESTATUS"] = "POR PAGAR";
                            if (Tipo_Cargo.Trim().Equals("REZAGO"))
                            {
                                Dr_Fila_Adeudo_Bimestre_2["CORRIENTE"] = 0;
                                Dr_Fila_Adeudo_Bimestre_2["REZAGOS"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_2"]);
                            }
                            else
                            {
                                Dr_Fila_Adeudo_Bimestre_2["CORRIENTE"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_2"]);
                                Dr_Fila_Adeudo_Bimestre_2["REZAGOS"] = 0;
                            }

                            if (Dicc_Tabulador_recargos.ContainsKey(Bimestre.ToString() + Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()))
                            {
                                Dr_Fila_Adeudo_Bimestre_2["RECARGOS_ORDINARIOS"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_2"]) * ((Convert.ToDouble(Dicc_Tabulador_recargos[Bimestre.ToString() + Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()])) / 100);
                            }
                            else
                            {
                                Dr_Fila_Adeudo_Bimestre_2["RECARGOS_ORDINARIOS"] = 0;
                            }
                            Dr_Fila_Adeudo_Bimestre_2["RECARGOS_MORATORIOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_2["HONORARIOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_2["MULTAS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_2["DESCUENTOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_2["MONTO"] = Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_2["CORRIENTE"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_2["REZAGOS"]) +
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_2["RECARGOS_ORDINARIOS"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_2["RECARGOS_MORATORIOS"]) +
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_2["HONORARIOS"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_2["MULTAS"]) -
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_2["DESCUENTOS"]);
                            Dt_Estructura.Rows.Add(Dr_Fila_Adeudo_Bimestre_2);
                        }
                    }
                }
                //SACA EL REGISTRO DEL TERCER BIMESTRE
                if (!string.IsNullOrEmpty(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_3"].ToString().Trim()))
                {
                    if (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_3"]) != 0)
                    {
                        Int32 Bimestre = 3;
                        Boolean Entra_Filtrado = true;
                        if (Anio_Filtro > 0)
                        {
                            if (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) < Anio_Filtro)
                            {
                                Entra_Filtrado = true;
                            }
                            else
                            {
                                if (Bimestre_Filtro > 0)
                                {
                                    if (Bimestre <= Bimestre_Filtro)
                                    {
                                        Entra_Filtrado = true;
                                    }
                                    else
                                    {
                                        Entra_Filtrado = false;
                                    }
                                }
                            }
                        }
                        if (Entra_Filtrado)
                        {
                            String Tipo_Cargo = "REZAGO";
                            if ((Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) == Anio_Periodo_Actual_1) || (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) == Anio_Periodo_Actual_2))
                            {
                                //if ((Bimestre_Periodo_Actual_1 == Bimestre) || (Bimestre_Periodo_Actual_2 == Bimestre)) {
                                Tipo_Cargo = "CORRIENTE";
                                //}
                            }
                            DataRow Dr_Fila_Adeudo_Bimestre_3 = Dt_Estructura.NewRow();
                            Dr_Fila_Adeudo_Bimestre_3["NO_ADEUDO"] = Dt_Listado_Adeudos.Rows[Contador]["NO_ADEUDO"].ToString();
                            Dr_Fila_Adeudo_Bimestre_3["CONCEPTO"] = Tipo_Cargo;
                            Dr_Fila_Adeudo_Bimestre_3["ANIO"] = Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString());
                            Dr_Fila_Adeudo_Bimestre_3["BIMESTRE"] = Bimestre;
                            Dr_Fila_Adeudo_Bimestre_3["DESCUENTO"] = 0;
                            Dr_Fila_Adeudo_Bimestre_3["ESTATUS"] = "POR PAGAR";
                            if (Tipo_Cargo.Trim().Equals("REZAGO"))
                            {
                                Dr_Fila_Adeudo_Bimestre_3["CORRIENTE"] = 0;
                                Dr_Fila_Adeudo_Bimestre_3["REZAGOS"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_3"]);
                            }
                            else
                            {
                                Dr_Fila_Adeudo_Bimestre_3["CORRIENTE"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_3"]);
                                Dr_Fila_Adeudo_Bimestre_3["REZAGOS"] = 0;
                            }
                            if (Dicc_Tabulador_recargos.ContainsKey(Bimestre.ToString() + Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()))
                            {
                                Dr_Fila_Adeudo_Bimestre_3["RECARGOS_ORDINARIOS"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_3"]) * ((Convert.ToDouble(Dicc_Tabulador_recargos[Bimestre.ToString() + Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()])) / 100);
                            }
                            else
                            {
                                Dr_Fila_Adeudo_Bimestre_3["RECARGOS_ORDINARIOS"] = 0;
                            }
                            Dr_Fila_Adeudo_Bimestre_3["RECARGOS_MORATORIOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_3["HONORARIOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_3["MULTAS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_3["DESCUENTOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_3["MONTO"] = Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_3["CORRIENTE"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_3["REZAGOS"]) +
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_3["RECARGOS_ORDINARIOS"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_3["RECARGOS_MORATORIOS"]) +
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_3["HONORARIOS"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_3["MULTAS"]) -
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_3["DESCUENTOS"]);
                            Dt_Estructura.Rows.Add(Dr_Fila_Adeudo_Bimestre_3);
                        }
                    }
                }
                //SACA EL REGISTRO DEL CUARTO BIMESTRE
                if (!string.IsNullOrEmpty(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_4"].ToString().Trim()))
                {
                    if (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_4"]) != 0)
                    {
                        Int32 Bimestre = 4;
                        Boolean Entra_Filtrado = true;
                        if (Anio_Filtro > 0)
                        {
                            if (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) < Anio_Filtro)
                            {
                                Entra_Filtrado = true;
                            }
                            else
                            {
                                if (Bimestre_Filtro > 0)
                                {
                                    if (Bimestre <= Bimestre_Filtro)
                                    {
                                        Entra_Filtrado = true;
                                    }
                                    else
                                    {
                                        Entra_Filtrado = false;
                                    }
                                }
                            }
                        }
                        if (Entra_Filtrado)
                        {
                            String Tipo_Cargo = "REZAGO";
                            if ((Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) == Anio_Periodo_Actual_1) || (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) == Anio_Periodo_Actual_2))
                            {
                                //if ((Bimestre_Periodo_Actual_1 == Bimestre) || (Bimestre_Periodo_Actual_2 == Bimestre)) {
                                Tipo_Cargo = "CORRIENTE";
                                //}
                            }
                            DataRow Dr_Fila_Adeudo_Bimestre_4 = Dt_Estructura.NewRow();
                            Dr_Fila_Adeudo_Bimestre_4["NO_ADEUDO"] = Dt_Listado_Adeudos.Rows[Contador]["NO_ADEUDO"].ToString();
                            Dr_Fila_Adeudo_Bimestre_4["CONCEPTO"] = Tipo_Cargo;
                            Dr_Fila_Adeudo_Bimestre_4["ANIO"] = Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString());
                            Dr_Fila_Adeudo_Bimestre_4["BIMESTRE"] = Bimestre;
                            Dr_Fila_Adeudo_Bimestre_4["DESCUENTO"] = 0;
                            Dr_Fila_Adeudo_Bimestre_4["ESTATUS"] = "POR PAGAR";
                            if (Tipo_Cargo.Trim().Equals("REZAGO"))
                            {
                                Dr_Fila_Adeudo_Bimestre_4["CORRIENTE"] = 0;
                                Dr_Fila_Adeudo_Bimestre_4["REZAGOS"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_4"]);
                            }
                            else
                            {
                                Dr_Fila_Adeudo_Bimestre_4["CORRIENTE"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_4"]);
                                Dr_Fila_Adeudo_Bimestre_4["REZAGOS"] = 0;
                            }
                            if (Dicc_Tabulador_recargos.ContainsKey(Bimestre.ToString() + Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()))
                            {
                                Dr_Fila_Adeudo_Bimestre_4["RECARGOS_ORDINARIOS"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_4"]) * ((Convert.ToDouble(Dicc_Tabulador_recargos[Bimestre.ToString() + Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()])) / 100);
                            }
                            else
                            {
                                Dr_Fila_Adeudo_Bimestre_4["RECARGOS_ORDINARIOS"] = 0;
                            }
                            Dr_Fila_Adeudo_Bimestre_4["RECARGOS_MORATORIOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_4["HONORARIOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_4["MULTAS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_4["DESCUENTOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_4["MONTO"] = Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_4["CORRIENTE"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_4["REZAGOS"]) +
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_4["RECARGOS_ORDINARIOS"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_4["RECARGOS_MORATORIOS"]) +
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_4["HONORARIOS"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_4["MULTAS"]) -
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_4["DESCUENTOS"]);
                            Dt_Estructura.Rows.Add(Dr_Fila_Adeudo_Bimestre_4);
                        }
                    }
                }
                //SACA EL REGISTRO DEL QUINTO BIMESTRE
                if (!string.IsNullOrEmpty(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_5"].ToString().Trim()))
                {
                    if (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_5"]) != 0)
                    {
                        Int32 Bimestre = 5;
                        Boolean Entra_Filtrado = true;
                        if (Anio_Filtro > 0)
                        {
                            if (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) < Anio_Filtro)
                            {
                                Entra_Filtrado = true;
                            }
                            else
                            {
                                if (Bimestre_Filtro > 0)
                                {
                                    if (Bimestre <= Bimestre_Filtro)
                                    {
                                        Entra_Filtrado = true;
                                    }
                                    else
                                    {
                                        Entra_Filtrado = false;
                                    }
                                }
                            }
                        }
                        if (Entra_Filtrado)
                        {
                            String Tipo_Cargo = "REZAGO";
                            if ((Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) == Anio_Periodo_Actual_1) || (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) == Anio_Periodo_Actual_2))
                            {
                                //if ((Bimestre_Periodo_Actual_1 == Bimestre) || (Bimestre_Periodo_Actual_2 == Bimestre)) {
                                Tipo_Cargo = "CORRIENTE";
                                //}
                            }
                            DataRow Dr_Fila_Adeudo_Bimestre_5 = Dt_Estructura.NewRow();
                            Dr_Fila_Adeudo_Bimestre_5["NO_ADEUDO"] = Dt_Listado_Adeudos.Rows[Contador]["NO_ADEUDO"].ToString();
                            Dr_Fila_Adeudo_Bimestre_5["CONCEPTO"] = Tipo_Cargo;
                            Dr_Fila_Adeudo_Bimestre_5["ANIO"] = Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString());
                            Dr_Fila_Adeudo_Bimestre_5["BIMESTRE"] = Bimestre;
                            Dr_Fila_Adeudo_Bimestre_5["DESCUENTO"] = 0;
                            Dr_Fila_Adeudo_Bimestre_5["ESTATUS"] = "POR PAGAR";
                            if (Tipo_Cargo.Trim().Equals("REZAGO"))
                            {
                                Dr_Fila_Adeudo_Bimestre_5["CORRIENTE"] = 0;
                                Dr_Fila_Adeudo_Bimestre_5["REZAGOS"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_5"]);
                            }
                            else
                            {
                                Dr_Fila_Adeudo_Bimestre_5["CORRIENTE"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_5"]);
                                Dr_Fila_Adeudo_Bimestre_5["REZAGOS"] = 0;
                            }
                            if (Dicc_Tabulador_recargos.ContainsKey(Bimestre.ToString() + Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()))
                            {
                                Dr_Fila_Adeudo_Bimestre_5["RECARGOS_ORDINARIOS"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_5"]) * ((Convert.ToDouble(Dicc_Tabulador_recargos[Bimestre.ToString() + Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()])) / 100);
                            }
                            else
                            {
                                Dr_Fila_Adeudo_Bimestre_5["RECARGOS_ORDINARIOS"] = 0;
                            }
                            Dr_Fila_Adeudo_Bimestre_5["RECARGOS_MORATORIOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_5["HONORARIOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_5["MULTAS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_5["DESCUENTOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_5["MONTO"] = Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_5["CORRIENTE"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_5["REZAGOS"]) +
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_5["RECARGOS_ORDINARIOS"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_5["RECARGOS_MORATORIOS"]) +
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_5["HONORARIOS"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_5["MULTAS"]) -
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_5["DESCUENTOS"]);
                            Dt_Estructura.Rows.Add(Dr_Fila_Adeudo_Bimestre_5);
                        }
                    }
                }
                //SACA EL REGISTRO DEL SEXTO BIMESTRE
                if (!string.IsNullOrEmpty(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_6"].ToString().Trim()))
                {
                    if (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_6"]) != 0)
                    {
                        Int32 Bimestre = 6;
                        Boolean Entra_Filtrado = true;
                        if (Anio_Filtro > 0)
                        {
                            if (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) < Anio_Filtro)
                            {
                                Entra_Filtrado = true;
                            }
                            else
                            {
                                if (Bimestre_Filtro > 0)
                                {
                                    if (Bimestre <= Bimestre_Filtro)
                                    {
                                        Entra_Filtrado = true;
                                    }
                                    else
                                    {
                                        Entra_Filtrado = false;
                                    }
                                }
                            }
                        }
                        if (Entra_Filtrado)
                        {
                            String Tipo_Cargo = "REZAGO";
                            if ((Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) == Anio_Periodo_Actual_1) || (Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()) == Anio_Periodo_Actual_2))
                            {
                                //if ((Bimestre_Periodo_Actual_1 == Bimestre) || (Bimestre_Periodo_Actual_2 == Bimestre)) {
                                Tipo_Cargo = "CORRIENTE";
                                //}
                            }
                            DataRow Dr_Fila_Adeudo_Bimestre_6 = Dt_Estructura.NewRow();
                            Dr_Fila_Adeudo_Bimestre_6["NO_ADEUDO"] = Dt_Listado_Adeudos.Rows[Contador]["NO_ADEUDO"].ToString();
                            Dr_Fila_Adeudo_Bimestre_6["CONCEPTO"] = Tipo_Cargo;
                            Dr_Fila_Adeudo_Bimestre_6["ANIO"] = Convert.ToInt32(Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString());
                            Dr_Fila_Adeudo_Bimestre_6["BIMESTRE"] = Bimestre;
                            Dr_Fila_Adeudo_Bimestre_6["DESCUENTO"] = 0;
                            Dr_Fila_Adeudo_Bimestre_6["ESTATUS"] = "POR PAGAR";
                            if (Tipo_Cargo.Trim().Equals("REZAGO"))
                            {
                                Dr_Fila_Adeudo_Bimestre_6["CORRIENTE"] = 0;
                                Dr_Fila_Adeudo_Bimestre_6["REZAGOS"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_6"]);
                            }
                            else
                            {
                                Dr_Fila_Adeudo_Bimestre_6["CORRIENTE"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_6"]);
                                Dr_Fila_Adeudo_Bimestre_6["REZAGOS"] = 0;
                            }
                            if (Dicc_Tabulador_recargos.ContainsKey(Bimestre.ToString() + Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()))
                            {
                                Dr_Fila_Adeudo_Bimestre_6["RECARGOS_ORDINARIOS"] = Convert.ToDouble(Dt_Listado_Adeudos.Rows[Contador]["ADEUDO_BIMESTRE_6"]) * ((Convert.ToDouble(Dicc_Tabulador_recargos[Bimestre.ToString() + Dt_Listado_Adeudos.Rows[Contador]["ANIO"].ToString()])) / 100);
                            }
                            else
                            {
                                Dr_Fila_Adeudo_Bimestre_6["RECARGOS_ORDINARIOS"] = 0;
                            }
                            Dr_Fila_Adeudo_Bimestre_6["RECARGOS_MORATORIOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_6["HONORARIOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_6["MULTAS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_6["DESCUENTOS"] = 0;
                            Dr_Fila_Adeudo_Bimestre_6["MONTO"] = Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_6["CORRIENTE"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_6["REZAGOS"]) +
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_6["RECARGOS_ORDINARIOS"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_6["RECARGOS_MORATORIOS"]) +
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_6["HONORARIOS"]) + Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_6["MULTAS"]) -
                                                                 Convert.ToDouble(Dr_Fila_Adeudo_Bimestre_6["DESCUENTOS"]);
                            Dt_Estructura.Rows.Add(Dr_Fila_Adeudo_Bimestre_6);
                        }
                    }
                }
            }

            return Dt_Estructura;
        }
        public DataTable Consultar_Clave_Ingreso()
        {
            return Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos.Consultar_Clave_Ingreso(this);
        }
        public DataTable Obtener_Dato_Consulta(String Cuenta_Predial, Int32 Anio)
        {
            return Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos.Obtener_Dato_Consulta(Cuenta_Predial, Anio);
        }
        public DataTable Consultar_Dependencia()
        {
            return Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos.Consultar_Dependencia(this);
        }
        public void Alta_Pasivo()
        {
            Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos.Alta_Pasivo(this);
        }
        public String Obtener_Menu_De_Ruta(String Ruta)
        {
            return Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos.Obtener_Menu_De_Ruta(Ruta);
        }
        public void Eliminar_Pasivos_No_Pagados_Anteriormente()
        {
            Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos.Eliminar_Pasivos_No_Pagados_Anteriormente(this);
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Convenio_Cuenta_Predia
        ///DESCRIPCIÓN: Consulta el Convenio.
        ///PARAMETROS: 
        ///CREO: Francisco Antonio Gallardo Castañeda.  
        ///FECHA_CREO: 28 Octubre 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Convenio_Cuenta_Predia()
        {

            //Se crea el Dt_Convenio con su estructura
            DataTable Dt_Convenio = new DataTable();
            Dt_Convenio.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
            Dt_Convenio.Columns.Add("NO_PAGO", Type.GetType("System.String"));
            Dt_Convenio.Columns.Add("NO_DESCUENTO", Type.GetType("System.String"));
            Dt_Convenio.Columns.Add("ESTADO_PAGO", Type.GetType("System.String"));
            Dt_Convenio.Columns.Add("PARCIALIDAD", Type.GetType("System.String"));
            Dt_Convenio.Columns.Add("ANIO", Type.GetType("System.Int32"));
            Dt_Convenio.Columns.Add("PERIODO", Type.GetType("System.String"));
            Dt_Convenio.Columns.Add("ESTATUS", Type.GetType("System.String"));
            Dt_Convenio.Columns.Add("CORRIENTE", Type.GetType("System.Double"));
            Dt_Convenio.Columns.Add("REZAGOS", Type.GetType("System.Double"));
            Dt_Convenio.Columns.Add("RECARGOS_ORDINARIOS", Type.GetType("System.Double"));
            Dt_Convenio.Columns.Add("RECARGOS_MORATORIOS", Type.GetType("System.Double"));
            Dt_Convenio.Columns.Add("HONORARIOS", Type.GetType("System.Double"));
            Dt_Convenio.Columns.Add("MONTO", Type.GetType("System.Double"));
            Dt_Convenio.Columns.Add("FECHA_VENCIMIENTO", Type.GetType("System.DateTime"));

            //Se consulta y recorre el resultado para generar el DataTable de Convenio
            DataTable Dt_Tmp_Generales_Convenio = Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos.Consultar_Convenio_Cuenta_Predia(this);
            if (Dt_Tmp_Generales_Convenio != null && Dt_Tmp_Generales_Convenio.Rows.Count != 0)
            {
                //Se obtiene el Desglose de Pagos de Convenios
                P_No_Convenio = Dt_Tmp_Generales_Convenio.Rows[0]["NO_CONVENIO"].ToString().Trim();
                P_No_Pagos = "";
                P_No_Descuento = Dt_Tmp_Generales_Convenio.Rows[0]["NO_DESCUENTO"].ToString().Trim();
                P_Fecha_Vencimiento = Convert.ToDateTime(Dt_Tmp_Generales_Convenio.Rows[0]["FECHA_VENCIMIENTO"].ToString().Trim());
                Boolean Convenio_Incumplido = Validar_Convenio_No_Imcumplido(Dt_Tmp_Generales_Convenio);

                for (Int32 Cnt_Pagos = 0; Cnt_Pagos < Dt_Tmp_Generales_Convenio.Rows.Count; Cnt_Pagos++)
                {
                    if (P_No_Pagos.Trim().Length > 0)
                    {
                        P_No_Pagos = P_No_Pagos + ", ";
                    }
                    P_No_Pagos = P_No_Pagos + Dt_Tmp_Generales_Convenio.Rows[Cnt_Pagos]["NO_PAGO"].ToString().Trim();
                }
                DataTable Dt_Tmp_Detalles_Convenio = Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos.Consultar_Detalle_Parcialidades_Convenio(this);
                String No_Pago_Anterior = "";
                String Parcialidad_Anterior = "";
                Int16 Cont_Parcialidades = 1;
                Int16 Inc_Parcialidades;
                for (Int32 Cnt_Deslose_Pagos = 0; Cnt_Deslose_Pagos < Dt_Tmp_Detalles_Convenio.Rows.Count; Cnt_Deslose_Pagos++)
                {
                    String No_Pago = Dt_Tmp_Detalles_Convenio.Rows[Cnt_Deslose_Pagos]["NO_PAGO"].ToString().Trim();
                    String Anio = (!String.IsNullOrEmpty(Dt_Tmp_Detalles_Convenio.Rows[Cnt_Deslose_Pagos]["ANIO"].ToString())) ? Dt_Tmp_Detalles_Convenio.Rows[Cnt_Deslose_Pagos]["ANIO"].ToString() : "0";
                    Int32 Fila_Actualizar = Buscar_Desglose_Pago_Convenio_Detalles(Dt_Convenio, No_Pago, Anio);
                    if (Fila_Actualizar > (-1))
                    {
                        Dt_Convenio.DefaultView.AllowEdit = true;
                        Dt_Convenio.Rows[Fila_Actualizar].BeginEdit();
                        for (Inc_Parcialidades = 0; Inc_Parcialidades < Cont_Parcialidades; Inc_Parcialidades++)
                        {
                            if (Convert.ToInt32(Anio) >= DateTime.Today.Year)
                            {
                                Double Corriente_Actual = (!String.IsNullOrEmpty(Dt_Convenio.Rows[Fila_Actualizar - Inc_Parcialidades]["CORRIENTE"].ToString())) ? Convert.ToDouble(Dt_Convenio.Rows[Fila_Actualizar - Inc_Parcialidades]["CORRIENTE"]) : 0.0;
                                Double Corriente_Agregar = (!String.IsNullOrEmpty(Dt_Tmp_Detalles_Convenio.Rows[Cnt_Deslose_Pagos]["MONTO_ANIO"].ToString())) ? Convert.ToDouble(Dt_Tmp_Detalles_Convenio.Rows[Cnt_Deslose_Pagos]["MONTO_ANIO"]) : 0.0;
                                if (Inc_Parcialidades == 0)
                                {
                                    Dt_Convenio.Rows[Fila_Actualizar - Inc_Parcialidades]["CORRIENTE"] = ((Corriente_Actual + Corriente_Agregar) / Cont_Parcialidades).ToString();
                                }
                                else
                                {
                                    Dt_Convenio.Rows[Fila_Actualizar - Inc_Parcialidades]["CORRIENTE"] = (Convert.ToDecimal(Dt_Convenio.Rows[Fila_Actualizar - Inc_Parcialidades]["CORRIENTE"]) / Cont_Parcialidades).ToString();
                                }
                            }
                            else
                            {
                                Double Rezago_Actual = (!String.IsNullOrEmpty(Dt_Convenio.Rows[Fila_Actualizar - Inc_Parcialidades]["REZAGOS"].ToString())) ? Convert.ToDouble(Dt_Convenio.Rows[Fila_Actualizar - Inc_Parcialidades]["REZAGOS"]) : 0.0;
                                Double Rezago_Agregar = (!String.IsNullOrEmpty(Dt_Tmp_Detalles_Convenio.Rows[Cnt_Deslose_Pagos]["MONTO_ANIO"].ToString())) ? Convert.ToDouble(Dt_Tmp_Detalles_Convenio.Rows[Cnt_Deslose_Pagos]["MONTO_ANIO"]) : 0.0;
                                if (Inc_Parcialidades == 0)
                                {
                                    Dt_Convenio.Rows[Fila_Actualizar - Inc_Parcialidades]["REZAGOS"] = ((Rezago_Actual + Rezago_Agregar) / Cont_Parcialidades).ToString();
                                }
                                else
                                {
                                    Dt_Convenio.Rows[Fila_Actualizar - Inc_Parcialidades]["REZAGOS"] = (Convert.ToDecimal(Dt_Convenio.Rows[Fila_Actualizar - Inc_Parcialidades]["REZAGOS"]) / Cont_Parcialidades).ToString();
                                }
                            }
                        }
                        Dt_Convenio.Rows[Fila_Actualizar].EndEdit();
                    }
                    else
                    {
                        DataRow Fila_Datos_Generales = Buscar_Pago_Detalle(Dt_Tmp_Generales_Convenio, No_Pago);
                        if (Fila_Datos_Generales != null)
                        {
                            DataRow Fila_Nueva = Dt_Convenio.NewRow();
                            Fila_Nueva["NO_CONVENIO"] = P_No_Convenio;
                            Fila_Nueva["NO_PAGO"] = No_Pago;
                            Fila_Nueva["NO_DESCUENTO"] = No_Descuento;
                            Fila_Nueva["FECHA_VENCIMIENTO"] = Fecha_Vencimiento;
                            if (Convenio_Incumplido)
                            {
                                Fila_Nueva["ESTADO_PAGO"] = "OBLIGATORIO";
                            }
                            else
                            {
                                if (Dt_Tmp_Generales_Convenio.Rows[0]["NO_PAGO"].ToString().Trim().Equals(No_Pago))
                                {
                                    Fila_Nueva["ESTADO_PAGO"] = "OBLIGATORIO";
                                }
                                else
                                {
                                    Fila_Nueva["ESTADO_PAGO"] = "OPCIONAL";
                                }
                            }
                            Fila_Nueva["PARCIALIDAD"] = (!String.IsNullOrEmpty(Fila_Datos_Generales["PARCIALIDAD"].ToString())) ? Fila_Datos_Generales["PARCIALIDAD"].ToString().Trim() : "";
                            if (Parcialidad_Anterior != Fila_Nueva["PARCIALIDAD"].ToString())
                            {
                                Parcialidad_Anterior = Fila_Nueva["PARCIALIDAD"].ToString();
                                Cont_Parcialidades = 1;
                            }
                            else
                            {
                                Cont_Parcialidades += 1;
                            }
                            Fila_Nueva["ANIO"] = Anio;
                            Fila_Nueva["PERIODO"] = Obtener_Bimestres_Cubre_Pago(Dt_Tmp_Detalles_Convenio.Rows[Cnt_Deslose_Pagos]);
                            Fila_Nueva["ESTATUS"] = (!String.IsNullOrEmpty(Fila_Datos_Generales["ESTATUS"].ToString())) ? Fila_Datos_Generales["ESTATUS"].ToString().Trim() : "";

                            if (Convert.ToInt32(Anio) >= DateTime.Today.Year)
                            {
                                Fila_Nueva["CORRIENTE"] = Convert.ToDouble(Dt_Tmp_Detalles_Convenio.Rows[Cnt_Deslose_Pagos]["MONTO_ANIO"].ToString().Trim());
                                Fila_Nueva["REZAGOS"] = 0.0;
                            }
                            else
                            {
                                Fila_Nueva["CORRIENTE"] = 0.0;
                                Fila_Nueva["REZAGOS"] = Convert.ToDouble(Dt_Tmp_Detalles_Convenio.Rows[Cnt_Deslose_Pagos]["MONTO_ANIO"].ToString().Trim());
                            }
                            if (No_Pago != No_Pago_Anterior)
                            {
                                Fila_Nueva["RECARGOS_ORDINARIOS"] = (!String.IsNullOrEmpty(Fila_Datos_Generales["RECARGOS_ORDINARIOS"].ToString())) ? Convert.ToDouble(Fila_Datos_Generales["RECARGOS_ORDINARIOS"].ToString().Trim()) : 0.0;
                                Fila_Nueva["RECARGOS_MORATORIOS"] = (!String.IsNullOrEmpty(Fila_Datos_Generales["RECARGOS_MORATORIOS"].ToString())) ? Convert.ToDouble(Fila_Datos_Generales["RECARGOS_MORATORIOS"].ToString().Trim()) : 0.0;
                                Fila_Nueva["HONORARIOS"] = (!String.IsNullOrEmpty(Fila_Datos_Generales["HONORARIOS"].ToString())) ? Convert.ToDouble(Fila_Datos_Generales["HONORARIOS"].ToString().Trim()) : 0.0;
                            }
                            else
                            {
                                Fila_Nueva["RECARGOS_ORDINARIOS"] = 0.00;
                                Fila_Nueva["RECARGOS_MORATORIOS"] = 0.00;
                                Fila_Nueva["HONORARIOS"] = 0.00;
                            }
                            No_Pago_Anterior = No_Pago;
                            Fila_Nueva["MONTO"] = 0.0;
                            Dt_Convenio.Rows.Add(Fila_Nueva);
                        }

                    }
                }

                //Se Calculan los Montos finales de los pagos
                for (Int32 Cnt_Convenio = 0; Cnt_Convenio < Dt_Convenio.Rows.Count; Cnt_Convenio++)
                {
                    Double Monto_Corriente = (!String.IsNullOrEmpty(Dt_Convenio.Rows[Cnt_Convenio]["CORRIENTE"].ToString())) ? Convert.ToDouble(Dt_Convenio.Rows[Cnt_Convenio]["CORRIENTE"].ToString()) : 0.0;
                    Double Monto_Rezago = (!String.IsNullOrEmpty(Dt_Convenio.Rows[Cnt_Convenio]["REZAGOS"].ToString())) ? Convert.ToDouble(Dt_Convenio.Rows[Cnt_Convenio]["REZAGOS"].ToString()) : 0.0;
                    Double Monto_R_Ordinarios = (!String.IsNullOrEmpty(Dt_Convenio.Rows[Cnt_Convenio]["RECARGOS_ORDINARIOS"].ToString())) ? Convert.ToDouble(Dt_Convenio.Rows[Cnt_Convenio]["RECARGOS_ORDINARIOS"].ToString()) : 0.0;
                    Double Monto_R_Moratorios = (!String.IsNullOrEmpty(Dt_Convenio.Rows[Cnt_Convenio]["RECARGOS_MORATORIOS"].ToString())) ? Convert.ToDouble(Dt_Convenio.Rows[Cnt_Convenio]["RECARGOS_MORATORIOS"].ToString()) : 0.0;
                    Double Monto_Honorarios = (!String.IsNullOrEmpty(Dt_Convenio.Rows[Cnt_Convenio]["HONORARIOS"].ToString())) ? Convert.ToDouble(Dt_Convenio.Rows[Cnt_Convenio]["HONORARIOS"].ToString()) : 0.0;
                    Dt_Convenio.DefaultView.AllowEdit = true;
                    Dt_Convenio.Rows[Cnt_Convenio].BeginEdit();
                    Dt_Convenio.Rows[Cnt_Convenio]["MONTO"] = Monto_Corriente + Monto_Rezago + Monto_R_Ordinarios + Monto_R_Moratorios + Monto_Honorarios;
                    Dt_Convenio.Rows[Cnt_Convenio].EndEdit();
                }
            }
            return Dt_Convenio;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Buscar_Desglose_Pago_Convenio_Detalles
        ///DESCRIPCIÓN: Busca el No. de fila para el desglose.
        ///PARAMETROS: 1.Dt_Generales. Tabla en la que se buscara.
        ///            2.No_Pago. Pago a Buscar. 
        ///            2.Anio. Anio a Buscar. 
        ///CREO: Francisco Antonio Gallardo Castañeda.  
        ///FECHA_CREO: 28 Octubre 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private Int32 Buscar_Desglose_Pago_Convenio_Detalles(DataTable Dt_Convenio, String No_Pago, String Anio)
        {
            Int32 Fila = (-1);
            if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
            {
                for (Int32 Cnt_Tmp = 0; Cnt_Tmp < Dt_Convenio.Rows.Count; Cnt_Tmp++)
                {
                    if (Dt_Convenio.Rows[Cnt_Tmp]["NO_PAGO"].ToString().Trim().Equals(No_Pago))
                    {
                        if (Dt_Convenio.Rows[Cnt_Tmp]["ANIO"].ToString().Trim().Equals(Anio))
                        {
                            Fila = Cnt_Tmp;
                            break;
                        }
                    }
                }
            }
            return Fila;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Buscar_Pago_Detalle
        ///DESCRIPCIÓN: Busca la Fila de la tabla para el Pago del Detalle.
        ///PARAMETROS: 1.Dt_Generales. Tabla en la que se buscara.
        ///            2.No_Pago. Pago a Buscar.
        ///CREO: Francisco Antonio Gallardo Castañeda.  
        ///FECHA_CREO: 28 Octubre 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private DataRow Buscar_Pago_Detalle(DataTable Dt_Generales, String No_Pago)
        {
            for (Int32 Cnt_Generales = 0; Cnt_Generales < Dt_Generales.Rows.Count; Cnt_Generales++)
            {
                if (Dt_Generales.Rows[Cnt_Generales]["NO_PAGO"].ToString().Trim().Equals(No_Pago.Trim()))
                {
                    return Dt_Generales.Rows[Cnt_Generales];
                }
            }
            return null;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Validar_Convenio_No_Imcumplido
        ///DESCRIPCIÓN: Valdia el Incumplimiento de un Convenio de Predial.
        ///PARAMETROS: 
        ///CREO: Francisco Antonio Gallardo Castañeda.  
        ///FECHA_CREO: 22 Octubre 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private Boolean Validar_Convenio_No_Imcumplido(DataTable Dt_Parcialidades)
        {
            Cls_Ope_Caj_Pagos_Negocio Rs_Consulta_Datos_Turno = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de Negocios
            Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio(); //Variable de conexión hacia la capa de Negocios
            DataTable Dt_Caja; //Variable que obtendra los datos de la consulta  de la caja del empleado
            DataTable Dt_Turno; //Variable que obtendra los datos de la consulta de la fecha de aplicacion
            Boolean Convenio_Incumplido = false; //Almacena para saber si se incumplio o no la parcialidad
            DateTime Fecha_Actual = DateTime.Today; //Almacena la fecha de aplicacion o actual
            String Caja_ID = "";    //Almacena el id de la caja del empleado

            //Obtiene la consulta de la caja del empleado
            Rs_Consulta_Datos_Turno.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Dt_Caja = Rs_Consulta_Datos_Turno.Consulta_Caja_Empleado();
            if (Dt_Caja.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Caja.Rows)
                {
                    Caja_ID = Registro["CAJA_ID"].ToString();
                }
            }

            //Obtiene la fecha de aplicacion
            Rs_Consulta_Datos_Turno.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Rs_Consulta_Datos_Turno.P_Caja_ID = Caja_ID;
            Dt_Turno = Rs_Consulta_Datos_Turno.Consulta_Datos_Turno();
            if (Dt_Turno.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Turno.Rows)
                {
                    Fecha_Actual = Convert.ToDateTime(Registro["APLICACION_PAGO"].ToString());
                }
            }
            else
            {
                Fecha_Actual = DateTime.Today;
            }

            //Valida que tenga parcialidades
            if (Dt_Parcialidades != null && Dt_Parcialidades.Rows.Count > 0)
            {
                for (Int32 Contador = 0; Contador < Dt_Parcialidades.Rows.Count; Contador++)
                {
                    if (!String.IsNullOrEmpty(Dt_Parcialidades.Rows[Contador]["FECHA_VENCIMIENTO"].ToString()))
                    {
                        DateTime Fecha_Vencimiento = Convert.ToDateTime(Dt_Parcialidades.Rows[Contador]["FECHA_VENCIMIENTO"].ToString());
                        Fecha_Vencimiento = Convert.ToDateTime(Dias_Inhabiles.Calcular_Fecha(Fecha_Vencimiento.ToShortDateString(), "10"));
                        if (Fecha_Vencimiento < Fecha_Actual)
                        {
                            Convenio_Incumplido = true;
                            break;
                        }
                    }
                }
            }
            return Convenio_Incumplido;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Obtener_Bimestres_Cubre_Pago
        ///DESCRIPCIÓN: Obtiene los bimestres que cubre cada linea de pago.
        ///PARAMETROS: 
        ///CREO: Francisco Antonio Gallardo Castañeda.  
        ///FECHA_CREO: 22 Octubre 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private String Obtener_Bimestres_Cubre_Pago(DataRow Dr_Pago)
        {
            String Bimestres_Cubre = "";
            Boolean Poner_Coma = false;
            for (Int32 Cnt = 1; Cnt <= 6; Cnt++)
            {
                if (Dr_Pago["BIMESTRE_" + Cnt.ToString()] != null && Dr_Pago["BIMESTRE_" + Cnt.ToString()].ToString().Trim().Length > 0)
                {
                    if (Convert.ToDouble(Dr_Pago["BIMESTRE_" + Cnt.ToString()]) > 0)
                    {
                        if (Poner_Coma)
                        {
                            Bimestres_Cubre = Bimestres_Cubre + ", ";
                        }
                        Bimestres_Cubre = Bimestres_Cubre + Cnt.ToString();
                        Poner_Coma = true;
                    }
                }
            }
            return Bimestres_Cubre;
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Obtener_Biemestres_A_Pagar
        ///DESCRIPCIÓN: Obtiene los bimestres que cubre el pago.
        ///PARAMETROS: 
        ///CREO: Francisco Antonio Gallardo Castañeda.  
        ///FECHA_CREO: 29 Octubre 2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Obtener_Biemestres_A_Pagar()
        {

            //Se crea la Tabla con la estructura
            DataTable Dt_Bimestres_Pagar = new DataTable();
            Dt_Bimestres_Pagar.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
            Dt_Bimestres_Pagar.Columns.Add("NO_PAGO", Type.GetType("System.String"));
            Dt_Bimestres_Pagar.Columns.Add("BIMESTRE", Type.GetType("System.Int32"));
            Dt_Bimestres_Pagar.Columns.Add("ANIO", Type.GetType("System.Int32"));
            Dt_Bimestres_Pagar.Columns.Add("MONTO", Type.GetType("System.Double"));

            //Se consultan los registros
            DataTable Dt_Tmp_Consulta = Cls_Ope_Pre_Recepcion_Pagos_Predial_Datos.Consultar_Detalle_Parcialidades_Convenio(this);
            if (Dt_Tmp_Consulta != null && Dt_Tmp_Consulta.Rows.Count > 0)
            {
                for (Int32 Cnt_Datos = 0; Cnt_Datos < Dt_Tmp_Consulta.Rows.Count; Cnt_Datos++)
                {

                    //Bimestre 1
                    if (!String.IsNullOrEmpty(Dt_Tmp_Consulta.Rows[Cnt_Datos]["BIMESTRE_1"].ToString()))
                    {
                        Double Monto = Convert.ToDouble(Dt_Tmp_Consulta.Rows[Cnt_Datos]["BIMESTRE_1"]);
                        if (Monto > 0)
                        {
                            DataRow Fila_Nueva = Dt_Bimestres_Pagar.NewRow();
                            Fila_Nueva["NO_CONVENIO"] = Dt_Tmp_Consulta.Rows[Cnt_Datos]["NO_CONVENIO"].ToString();
                            Fila_Nueva["NO_PAGO"] = Dt_Tmp_Consulta.Rows[Cnt_Datos]["NO_PAGO"].ToString();
                            Fila_Nueva["BIMESTRE"] = 1;
                            Fila_Nueva["ANIO"] = Convert.ToInt32(Dt_Tmp_Consulta.Rows[Cnt_Datos]["ANIO"].ToString());
                            Fila_Nueva["MONTO"] = Monto;
                            Dt_Bimestres_Pagar.Rows.Add(Fila_Nueva);
                        }
                    }
                    //Bimestre 2
                    if (!String.IsNullOrEmpty(Dt_Tmp_Consulta.Rows[Cnt_Datos]["BIMESTRE_2"].ToString()))
                    {
                        Double Monto = Convert.ToDouble(Dt_Tmp_Consulta.Rows[Cnt_Datos]["BIMESTRE_2"]);
                        if (Monto > 0)
                        {
                            DataRow Fila_Nueva = Dt_Bimestres_Pagar.NewRow();
                            Fila_Nueva["NO_CONVENIO"] = Dt_Tmp_Consulta.Rows[Cnt_Datos]["NO_CONVENIO"].ToString();
                            Fila_Nueva["NO_PAGO"] = Dt_Tmp_Consulta.Rows[Cnt_Datos]["NO_PAGO"].ToString();
                            Fila_Nueva["BIMESTRE"] = 2;
                            Fila_Nueva["ANIO"] = Convert.ToInt32(Dt_Tmp_Consulta.Rows[Cnt_Datos]["ANIO"].ToString());
                            Fila_Nueva["MONTO"] = Monto;
                            Dt_Bimestres_Pagar.Rows.Add(Fila_Nueva);
                        }
                    }

                    //Bimestre 3
                    if (!String.IsNullOrEmpty(Dt_Tmp_Consulta.Rows[Cnt_Datos]["BIMESTRE_3"].ToString()))
                    {
                        Double Monto = Convert.ToDouble(Dt_Tmp_Consulta.Rows[Cnt_Datos]["BIMESTRE_3"]);
                        if (Monto > 0)
                        {
                            DataRow Fila_Nueva = Dt_Bimestres_Pagar.NewRow();
                            Fila_Nueva["NO_CONVENIO"] = Dt_Tmp_Consulta.Rows[Cnt_Datos]["NO_CONVENIO"].ToString();
                            Fila_Nueva["NO_PAGO"] = Dt_Tmp_Consulta.Rows[Cnt_Datos]["NO_PAGO"].ToString();
                            Fila_Nueva["BIMESTRE"] = 3;
                            Fila_Nueva["ANIO"] = Convert.ToInt32(Dt_Tmp_Consulta.Rows[Cnt_Datos]["ANIO"].ToString());
                            Fila_Nueva["MONTO"] = Monto;
                            Dt_Bimestres_Pagar.Rows.Add(Fila_Nueva);
                        }
                    }

                    //Bimestre 4
                    if (!String.IsNullOrEmpty(Dt_Tmp_Consulta.Rows[Cnt_Datos]["BIMESTRE_4"].ToString()))
                    {
                        Double Monto = Convert.ToDouble(Dt_Tmp_Consulta.Rows[Cnt_Datos]["BIMESTRE_4"]);
                        if (Monto > 0)
                        {
                            DataRow Fila_Nueva = Dt_Bimestres_Pagar.NewRow();
                            Fila_Nueva["NO_CONVENIO"] = Dt_Tmp_Consulta.Rows[Cnt_Datos]["NO_CONVENIO"].ToString();
                            Fila_Nueva["NO_PAGO"] = Dt_Tmp_Consulta.Rows[Cnt_Datos]["NO_PAGO"].ToString();
                            Fila_Nueva["BIMESTRE"] = 4;
                            Fila_Nueva["ANIO"] = Convert.ToInt32(Dt_Tmp_Consulta.Rows[Cnt_Datos]["ANIO"].ToString());
                            Fila_Nueva["MONTO"] = Monto;
                            Dt_Bimestres_Pagar.Rows.Add(Fila_Nueva);
                        }
                    }

                    //Bimestre 5
                    if (!String.IsNullOrEmpty(Dt_Tmp_Consulta.Rows[Cnt_Datos]["BIMESTRE_5"].ToString()))
                    {
                        Double Monto = Convert.ToDouble(Dt_Tmp_Consulta.Rows[Cnt_Datos]["BIMESTRE_5"]);
                        if (Monto > 0)
                        {
                            DataRow Fila_Nueva = Dt_Bimestres_Pagar.NewRow();
                            Fila_Nueva["NO_CONVENIO"] = Dt_Tmp_Consulta.Rows[Cnt_Datos]["NO_CONVENIO"].ToString();
                            Fila_Nueva["NO_PAGO"] = Dt_Tmp_Consulta.Rows[Cnt_Datos]["NO_PAGO"].ToString();
                            Fila_Nueva["BIMESTRE"] = 5;
                            Fila_Nueva["ANIO"] = Convert.ToInt32(Dt_Tmp_Consulta.Rows[Cnt_Datos]["ANIO"].ToString());
                            Fila_Nueva["MONTO"] = Monto;
                            Dt_Bimestres_Pagar.Rows.Add(Fila_Nueva);
                        }
                    }

                    //Bimestre 6
                    if (!String.IsNullOrEmpty(Dt_Tmp_Consulta.Rows[Cnt_Datos]["BIMESTRE_6"].ToString()))
                    {
                        Double Monto = Convert.ToDouble(Dt_Tmp_Consulta.Rows[Cnt_Datos]["BIMESTRE_6"]);
                        if (Monto > 0)
                        {
                            DataRow Fila_Nueva = Dt_Bimestres_Pagar.NewRow();
                            Fila_Nueva["NO_CONVENIO"] = Dt_Tmp_Consulta.Rows[Cnt_Datos]["NO_CONVENIO"].ToString();
                            Fila_Nueva["NO_PAGO"] = Dt_Tmp_Consulta.Rows[Cnt_Datos]["NO_PAGO"].ToString();
                            Fila_Nueva["BIMESTRE"] = 6;
                            Fila_Nueva["ANIO"] = Convert.ToInt32(Dt_Tmp_Consulta.Rows[Cnt_Datos]["ANIO"].ToString());
                            Fila_Nueva["MONTO"] = Monto;
                            Dt_Bimestres_Pagar.Rows.Add(Fila_Nueva);
                        }
                    }

                }
            }

            return Dt_Bimestres_Pagar;
        }

        #endregion

    }

}

