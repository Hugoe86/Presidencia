using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Operacion_Predial_Empleados_Activos.Datos;
using System.Data;

namespace Presidencia.Operacion_Predial_Empleados_Activos.Negocio
{

    public class Cls_Ope_Pre_Empleado_Activos_Negocio
    {
        public Cls_Ope_Pre_Empleado_Activos_Negocio()
        {
        }
        #region variables locales
        private String Empleado_ID;
        private String No_Empleado;        
        private String Empleado_Nombre;        
        private String Estatus;
        private int Consecutivo;
        private String No_Movimiento;        
        private DataTable Dt_Empleados;        
        private String Usuario;
        #endregion

        #region Variables Globales

        public String P_No_Movimiento
        {
            get { return No_Movimiento; }
            set { No_Movimiento = value; }
        }

        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_Empleado_Nombre
        {
            get { return Empleado_Nombre; }
            set { Empleado_Nombre = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public int P_Consecutivo
        {
            get { return Consecutivo; }
            set { Consecutivo = value; }
        }
        public DataTable P_Dt_Empleados
        {
            get { return Dt_Empleados; }
            set { Dt_Empleados = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        #endregion

        #region Metodos
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Asignar_Pendiente
        ///DESCRIPCIÓN: Se obtiene el ID del empleado siguiente en la lista de empleados activos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 05/12/2011 05:39:57 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public String Asignar_Pendiente()
        {
            return Cls_Ope_Pre_Empleados_Activos_Datos.Asignar_Pendiente();
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Empleados_Activos
        ///DESCRIPCIÓN: Consulta la lista de empleados con el rol de translado de domino
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 05/12/2011 05:40:29 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataTable Consulta_Empleados_Activos()
        {
            return Cls_Ope_Pre_Empleados_Activos_Datos.Consulta_Empleados_Activos();
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Modificar_Empleados_Activos
        ///DESCRIPCIÓN: Cambia el estatus del empleado seleccionado
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 05/12/2011 05:41:19 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public String Modificar_Empleados_Activos()
        {
            return Cls_Ope_Pre_Empleados_Activos_Datos.Modificar_Empleado_Activo(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Generar_Tabla_Empleados_Activos
        ///DESCRIPCIÓN: Realiza una consulta de los empleados con 
        ///             el rol de transalado de dominio registrados 
        ///             en la tabla de empleados y hace una incercion 
        ///             de estos en la tabla de empleados activos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 05/12/2011 05:42:14 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Generar_Tabla_Empleados_Activos()
        {
            Cls_Ope_Pre_Empleados_Activos_Datos.Generar_Tabla_Empleados_Activos();
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consulta_Movimientos
        ///DESCRIPCIÓN: Consulta los movimientos o pendientes del usuario seleccionado
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 05/12/2011 05:45:22 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public DataTable Consulta_Movimientos()
        {
            return Cls_Ope_Pre_Empleados_Activos_Datos.Consulta_Movimientos(this);
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Reasignar_Movimiento
        ///DESCRIPCIÓN: Se obtiene el ID del empleado siguiente en la lista de empleados activos
        ///y se actualiza el Empleado encargado del movimiento
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 05/12/2011 05:39:57 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public void Reasignar_Movimiento()
        {
            Cls_Ope_Pre_Empleados_Activos_Datos.Reasignar_Movimiento(this);
        }
        #endregion
    }
}