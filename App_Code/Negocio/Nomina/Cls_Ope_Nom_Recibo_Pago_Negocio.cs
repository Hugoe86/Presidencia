using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Recibo_Pago.Datos;
using System.Data;

namespace Presidencia.Recibo_Pago.Negocio
{
    public class Cls_Ope_Nom_Recibo_Pago_Negocio
    {

        #region (Variables Internas)
        private String No_Recibo;
        private String Empleado_ID;
        private String Rfc;
        private String Nombre_Empleado;
        private String Periodo;
        private String Categoria;
        private String Curp;
        private String Departamento;
        private Int32 No_Afiliacion;
        private String Codigo_Programatico;
        private Int32 Dias_Trabajados;
        private Double Total_Percepciones;
        private Double Total_Deducciones;
        private Double Total_Nomina;
        private String Usuario;
        private String Fecha_Inicio;        
        private String Fecha_Fin;
        private String Nomina_ID;
        private String Tipo_Nomina_ID;
        private String Banco_ID;
        #endregion

        #region (Variables Publicas)

        public String P_No_Recibo
        {
            get { return No_Recibo; }
            set { No_Recibo = value; }
        }
        public String P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_Fecha_Fin
        {
            get { return Fecha_Fin; }
            set { Fecha_Fin = value; }
        }

        public String P_Rfc
        {
            get { return Rfc; }
            set { Rfc = value; }
        }
        public String P_Nombre_Empleado
        {
            get { return Nombre_Empleado; }
            set { Nombre_Empleado = value; }
        }
        public String P_Periodo
        {
            get { return Periodo; }
            set { Periodo = value; }
        }
        public String P_Categoria
        {
            get { return Categoria; }
            set { Categoria = value; }
        }
        

        public String P_Curp
        {
            get { return Curp; }
            set { Curp = value; }
        }
        

        public String P_Departamento
        {
            get { return Departamento; }
            set { Departamento = value; }
        }
        

        public Int32 P_No_Afiliacion
        {
            get { return No_Afiliacion; }
            set { No_Afiliacion = value; }
        }
        
        public String P_Codigo_Programatico
        {
            get { return Codigo_Programatico; }
            set { Codigo_Programatico = value; }
        }
        

        public Int32 P_Dias_Trabajados
        {
            get { return Dias_Trabajados; }
            set { Dias_Trabajados = value; }
        }
        

        public Double P_Total_Percepciones
        {
            get { return Total_Percepciones; }
            set { Total_Percepciones = value; }
        }
        

        public Double P_Total_Deducciones
        {
            get { return Total_Deducciones; }
            set { Total_Deducciones = value; }
        }
        

        public Double P_Total_Nomina
        {
            get { return Total_Nomina; }
            set { Total_Nomina = value; }
        }
        

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        DataTable Dt_Percepciones;

        public DataTable P_Dt_Percepciones
        {
            get { return Dt_Percepciones; }
            set { Dt_Percepciones = value; }
        }
        DataTable Dt_Deducciones;

        public DataTable P_Dt_Deducciones
        {
            get { return Dt_Deducciones; }
            set { Dt_Deducciones = value; }
        }

        public String P_Nomina_ID {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }
        public String P_Tipo_Nomina_ID {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }
        public String P_Banco_ID
        {
            get { return Banco_ID; }
            set { Banco_ID = value; }
        }
        #endregion

        #region (Metodos)
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Generar_Recibo
        ///DESCRIPCIÓN: se genera el recibo filtrado por los metodos de busqueda especificados en la vista de usuario
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 04/21/2011 02:37:19 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Generar_Recibo()
        {
            return Cls_Ope_Nom_Recibo_Pago_Datos.Generar_Recibo(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Percepciones_Recibo_Pago
        ///DESCRIPCIÓN: consulta las percepciones del empleado para el recibo a imprimir
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 04/23/2011 12:15:08 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Percepciones_Recibo_Pago()
        {
            return Cls_Ope_Nom_Recibo_Pago_Datos.Consulta_Percepciones_Recibo_Pago(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Deducciones_Recibo_Pago
        ///DESCRIPCIÓN: consulta las percepciones del empleado para el recibo a imprimir
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 04/23/2011 12:15:08 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Deducciones_Recibo_Pago()
        {
            return Cls_Ope_Nom_Recibo_Pago_Datos.Consulta_Deducciones_Recibo_Pago(this);
        }

        public DataTable Consultar_Recibos_Empleados() {
            return Cls_Ope_Nom_Recibo_Pago_Datos.Consultar_Recibos_Empleados(this);
        }

        public DataTable Consultar_Percepciones_Recibo_Empleado() {
            return Cls_Ope_Nom_Recibo_Pago_Datos.Consultar_Percepciones_Recibo_Empleado(this);
        }

        public DataTable Consultar_Deducciones_Recibo_Empleado()
        {
            return Cls_Ope_Nom_Recibo_Pago_Datos.Consultar_Deducciones_Recibo_Empleado(this);
        }
        #endregion

    }
}