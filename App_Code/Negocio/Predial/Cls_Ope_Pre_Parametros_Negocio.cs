using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Predial_Parametros.Datos;
using System.Data;

namespace Presidencia.Operacion_Predial_Parametros.Negocio
{

    public class Cls_Ope_Pre_Parametros_Negocio
    {
        public Cls_Ope_Pre_Parametros_Negocio()
        {
        }
            #region Variables Internas
                private String Recargas_Traslado;
                private String Constancia_No_Adeudo;
                private String Respaldo_Des_Tras;
                private String Anio_Vigencia;
                private String Porcentaje_Cobro;
                private String Tope_Salario;
                private Double Tolerancia_Pago_Superior;
                private Double Tolerancia_Pago_Inferior;
                //Constancias
                private String Constancia_Nombre;                
                private String Constancia_Costo;                
                private String Constancia_ID;                
                private String Constancia_Comentarios;
                
            #endregion

            #region Variables Publicas

                public Double P_Tolerancia_Pago_Superior
                {
                    get { return Tolerancia_Pago_Superior; }
                    set { Tolerancia_Pago_Superior = value; }
                }

                public Double P_Tolerancia_Pago_Inferior
                {
                    get { return Tolerancia_Pago_Inferior; }
                    set { Tolerancia_Pago_Inferior = value; }
                }

                public String P_Porcentaje_Cobro
                {
                    get { return Porcentaje_Cobro; }
                    set { Porcentaje_Cobro = value; }
                }

                public String P_Tope_Salario
                {
                    get { return Tope_Salario; }
                    set { Tope_Salario = value; }
                }
                                                 
                public String P_Recargas_Traslado
                {
                    get { return Recargas_Traslado; }
                    set { Recargas_Traslado = value; }
                }
                public String P_Constancia_No_Adeudo
                {
                    get { return Constancia_No_Adeudo; }
                    set { Constancia_No_Adeudo = value; }
                }
                public String P_Respaldo_Des_Tras
                {
                    get { return Respaldo_Des_Tras; }
                    set { Respaldo_Des_Tras = value; }
                }
                public String P_Anio_Vigencia
                {
                    get { return Anio_Vigencia; }
                    set { Anio_Vigencia = value; }
                }
                public String P_Constancia_Nombre
                {
                    get { return Constancia_Nombre; }
                    set { Constancia_Nombre = value; }
                }
                public String P_Constancia_Costo
                {
                    get { return Constancia_Costo; }
                    set { Constancia_Costo = value; }
                }
                public String P_Constancia_ID
                {
                    get { return Constancia_ID; }
                    set { Constancia_ID = value; }
                }
                public String P_Constancia_Comentarios
                {
                    get { return Constancia_Comentarios; }
                    set { Constancia_Comentarios = value; }
                }

            #endregion

            #region Metodos
                public DataTable Consultar_Parametros()
                {
                    return Cls_Ope_Pre_Parametros_Datos.Consultar_Parametros();                    
                }

                public DataTable Consultar_Parametro_Caja_Pagos_Internet()
                {
                    return Cls_Ope_Pre_Parametros_Datos.Consultar_Parametro_Caja_Pagos_Internet();
                }
                public DataTable Consultar_Parametro_Caja_Pagos_Pae()
                {
                    return null;
                    //return Cls_Ope_Pre_Parametros_Datos.Consultar_Parametro_Caja_Pagos_Pae();
                }

                public Int32 Consultar_Dias_Vencimiento()
                {
                    return Cls_Ope_Pre_Parametros_Datos.Consultar_Dias_Vencimiento();
                }

                public void Modificar_Parametros()
                {
                    Cls_Ope_Pre_Parametros_Datos.Modificar_Parametros(this);
                }

                public DataTable Consulta_Constancias()
                {
                    return Cls_Ope_Pre_Parametros_Datos.Consulta_Constancias(this);
                }

                public DataTable Consultar_Parametros_Cajas()
                {
                    return Cls_Ope_Pre_Parametros_Datos.Consultar_Parametros_Cajas();
                }

                public void Modificar_Parametros_Cajas()
                {
                    Cls_Ope_Pre_Parametros_Datos.Modificar_Parametros_Cajas(this);
                }

                public Int32 Consultar_Anio_Corriente()
                {
                    return Cls_Ope_Pre_Parametros_Datos.Consultar_Anio_Corriente();
                }

                public Int32 Modificar_Anio_Corriente(Int32 Anio_Corriente)
                {
                    return Cls_Ope_Pre_Parametros_Datos.Modificar_Anio_Corriente(Anio_Corriente);
                }


            #endregion
    }
}