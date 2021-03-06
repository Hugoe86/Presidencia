﻿using System;
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
using Presidencia.Percepciones_Variables.Datos;

namespace Presidencia.Percepciones_Variables.Negocio
{
    public class Cls_Ope_Nom_Percepciones_Var_Negocio
    {
        #region (Variables Internas)
        private String No_Percepcion;
        private String Dependencia_ID;
        private String Percepcion_Deduccion_ID;
        private String Empleado_ID;
        private String Estatus;
        private String Comentarios;
        private String Comentarios_Estatus;
        private DataTable Dt_Ope_Nom_Percepciones_Var;
        private DataTable Dt_Empleados;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private String Tipo_Percepcion_Deduccion;
        private Double Cantidad;
        private String Nomina_ID;
        private Int32 No_Nomina;
        private String No_Empleado;
        private String Nombre_Empleado;
        #endregion

        #region (Variables Publicas)
        public String P_No_Percepcion
        {
            get { return No_Percepcion; }
            set { No_Percepcion = value; }
        }
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Percepcion_Deduccion_ID
        {
            get { return Percepcion_Deduccion_ID; }
            set { Percepcion_Deduccion_ID = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }
        public String P_Comentarios_Estatus
        {
            get { return Comentarios_Estatus; }
            set { Comentarios_Estatus = value; }
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
        public DataTable P_Dt_Ope_Nom_Percepciones_Var
        {
            get { return Dt_Ope_Nom_Percepciones_Var; }
            set { Dt_Ope_Nom_Percepciones_Var = value; }
        }
        public DataTable P_Dt_Empleados
        {
            get { return Dt_Empleados; }
            set { Dt_Empleados = value; }
        }
        public String P_Tipo_Percepcion_Deduccion
        {
            get { return Tipo_Percepcion_Deduccion; }
            set { Tipo_Percepcion_Deduccion = value; }
        }
        public Double P_Cantidad
        {
            get { return Cantidad; }
            set { Cantidad = value; }
        }
        public String P_Nomina_ID
        {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }
        public Int32 P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }
        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
        public String P_Nombre_Empleado
        {
            get { return Nombre_Empleado; }
            set { Nombre_Empleado = value; }
        }
        #endregion

        #region (Metodos)
        public Boolean Alta_Percepcion_Variable() {
            return Cls_Ope_Nom_Percepciones_Var_Datos.Alta_Percepciones_Variables(this);
        }
        public Boolean Modificar_Percepcion_Empleado() {
            return Cls_Ope_Nom_Percepciones_Var_Datos.Modificar_Ope_Nom_Percepciones_Var(this);
        }
        public Boolean Eliminar_Percepcion_Variable() {
            return Cls_Ope_Nom_Percepciones_Var_Datos.Eliminar_Percepciones_Variables(this);
        }
        public Cls_Ope_Nom_Percepciones_Var_Negocio Consulta_Percepciones_Variables() {
            return Cls_Ope_Nom_Percepciones_Var_Datos.Consultar_Percepciones_Variables(this);
        }
        public DataTable Consultar_Percepciones_Variable_Opcionales() {
            return Cls_Ope_Nom_Percepciones_Var_Datos.Consultar_Percepciones_Variable_Opcionales(this);
        }
        public Boolean Cambiar_Estatus_Percepciones_Variables() {
            return Cls_Ope_Nom_Percepciones_Var_Datos.Cambiar_Estatus_Percepciones_Variables(this);
        }
        public DataTable Consultar_Empleados_Aplica_Percepcion_Dependencia() {
            return Cls_Ope_Nom_Percepciones_Var_Datos.Consultar_Empleados_Aplica_Percepcion_Dependencia(this);
        }
        #endregion
    }
}
