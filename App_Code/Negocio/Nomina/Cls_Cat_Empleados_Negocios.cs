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
using Presidencia.Empleados.Datos;
using Presidencia.Aportacion.Datos;


namespace Presidencia.Empleados.Negocios
{
    public class Cls_Cat_Empleados_Negocios
    {
        #region (Variables Internas)
        private String Empleado_ID;
        private String Area_ID;
        private String Dependencia_ID;
        private String Programa_ID;
        private String Tipo_Contrato_ID;
        private String Puesto_ID;
        private String Escolaridad_ID;
        private String Sindicado_ID;
        private String Turno_ID;
        private String Zona_ID;
        private String Tipo_Trabajador_ID;
        private String Rol_ID;
        private String Tipo_Nomina_ID;
        private String Tercero_ID;
        private String No_Empleado;
        private String Password;
        private String Apellido_Paterno;
        private String Apelldo_Materno;
        private String Nombre;
        private String Calle;
        private String Colonia;
        private Int32 Codigo_Postal;
        private String Ciudad;
        private String Estado;
        private String Telefono_Casa;
        private String Telefono_Oficina;
        private String Extension;
        private String Fax;
        private String Celular;
        private String Nextel;
        private String Correo_Electronico;
        private String Sexo;
        private DateTime Fecha_Nacimiento;
        private String RFC;
        private String CURP;
        private String Estatus;
        private String Ruta_Foto;
        private String Nombre_Foto;
        private String No_IMSS;
        private String Forma_Pago;
        private String No_Cuenta_Bancaria;
        private DateTime Fecha_Inicio;
        private String Tipo_Finiquito;
        private DateTime Fecha_Termino_Contrato;
        private DateTime Fecha_Baja;
        private Double Salario_Diario;
        private Double Salario_Diario_Integrado;
        private String Lunes;
        private String Martes;
        private String Miercoles;
        private String Jueves;
        private String Viernes;
        private String Sabado;
        private String Domingo;
        private String Comentarios;
        private String Nombre_Usuario;
        private DataTable Documentos_Anexos_Empleado;
        private String No_Licencia;
        private DateTime Fecha_Vigencia_Licencia;
        private Double Salario_Mensual_Actual;
        private String Banco_ID;
        private String Reloj_Checador;
        private String No_Tarjeta;
        private String No_Seguro_Poliza;
        private String Beneficiario_Seguro;
        //Movimientos del Empleado
        private String No_Movimiento;
        private String Tipo_Movimiento;
        private String Motivo_Movimiento;
        private Double Sueldo_Actual;
        private String Fecha_Inicio_Busqueda;
        private String Fecha_Fin_Busqueda;
        //Percepciones Deducciones
        private DataTable Dt_Tipo_Nomina_Lista_Percepciones;
        private DataTable Dt_Tipo_Nomina_Lista_Deducciones;
        private DataTable Dt_Sindicato_Lista_Percepciones;
        private DataTable Dt_Sindicato_Lista_Deducciones;
        private String Concepto;
        //SAP Código Programático.
        private String SAP_Unidad_Responsable_ID;
        private String SAP_Fuente_Financiamiento_ID;
        private String SAP_Area_Responsable_ID;
        private String SAP_Programa_ID;
        private String SAP_Partida;
        private String SAP_Codigo_Programatico;

        private String Nomina_ID;
        private String No_Nomina;
        private String Estatus_Incidencia;
        private String Tipo_Incidencia;
        private String Fecha_Inicio_Busqueda_Incidencia;
        private String Fecha_Fin_Busqueda_Incidencia;

        private String Clave;
        private String Tipo_Empleado;
        private String Confronto;
        private String Aplica_ISSEG;

        private String Reloj_Checator_ID;
        private DateTime Fecha_Inicio_Reloj_Checador;
        private String Movimiento;
        private String Cuenta_Contable_ID;

        private String Fecha_Alta_ISSEG;

        //registro de movimientos.
        private String Aplica_Baja_Licencia;
        private String Fecha_Inicio_Licencia;
        private String Fecha_Termino_Licencia;
        private String Fecha_Baja_IMSS;
        private String Tipo_Baja;
        #endregion

        #region (Variables Publicas)
        public String P_Movimiento {
            get { return Movimiento; }
            set { Movimiento = value; }
        }

        public String P_Reloj_Checator_ID
        {
            get { return Reloj_Checator_ID; }
            set { Reloj_Checator_ID = value; }
        }

        public DateTime P_Fecha_Inicio_Reloj_Checador
        {
            get { return Fecha_Inicio_Reloj_Checador; }
            set { Fecha_Inicio_Reloj_Checador = value; }
        }

        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_Area_ID
        {
            get { return Area_ID; }
            set { Area_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Programa_ID
        {
            get { return Programa_ID; }
            set { Programa_ID = value; }
        }

        public String P_Tipo_Contrato_ID
        {
            get { return Tipo_Contrato_ID; }
            set { Tipo_Contrato_ID = value; }
        }

        public String P_Puesto_ID
        {
            get { return Puesto_ID; }
            set { Puesto_ID = value; }
        }

        public String P_Escolaridad_ID
        {
            get { return Escolaridad_ID; }
            set { Escolaridad_ID = value; }
        }

        public String P_Sindicado_ID
        {
            get { return Sindicado_ID; }
            set { Sindicado_ID = value; }
        }

        public String P_Turno_ID
        {
            get { return Turno_ID; }
            set { Turno_ID = value; }
        }

        public String P_Zona_ID
        {
            get { return Zona_ID; }
            set { Zona_ID = value; }
        }

        public String P_Tipo_Trabajador_ID
        {
            get { return Tipo_Trabajador_ID; }
            set { Tipo_Trabajador_ID = value; }
        }

        public String P_Rol_ID
        {
            get { return Rol_ID; }
            set { Rol_ID = value; }
        }

        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }

        public String P_Terceros_ID
        {
            get { return Tercero_ID; }
            set { Tercero_ID = value; }
        }

        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }

        public String P_Password
        {
            get { return Password; }
            set { Password = value; }
        }

        public String P_Apellido_Paterno
        {
            get { return Apellido_Paterno; }
            set { Apellido_Paterno = value; }
        }

        public String P_Apelldo_Materno
        {
            get { return Apelldo_Materno; }
            set { Apelldo_Materno = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
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

        public Int32 P_Codigo_Postal
        {
            get { return Codigo_Postal; }
            set { Codigo_Postal = value; }
        }

        public String P_Ciudad
        {
            get { return Ciudad; }
            set { Ciudad = value; }
        }

        public String P_Estado
        {
            get { return Estado; }
            set { Estado = value; }
        }

        public String P_Telefono_Casa
        {
            get { return Telefono_Casa; }
            set { Telefono_Casa = value; }
        }

        public String P_Telefono_Oficina
        {
            get { return Telefono_Oficina; }
            set { Telefono_Oficina = value; }
        }

        public String P_Extension
        {
            get { return Extension; }
            set { Extension = value; }
        }

        public String P_Fax
        {
            get { return Fax; }
            set { Fax = value; }
        }

        public String P_Celular
        {
            get { return Celular; }
            set { Celular = value; }
        }

        public String P_Nextel
        {
            get { return Nextel; }
            set { Nextel = value; }
        }

        public String P_Correo_Electronico
        {
            get { return Correo_Electronico; }
            set { Correo_Electronico = value; }
        }

        public String P_Sexo
        {
            get { return Sexo; }
            set { Sexo = value; }
        }

        public DateTime P_Fecha_Nacimiento
        {
            get { return Fecha_Nacimiento; }
            set { Fecha_Nacimiento = value; }
        }

        public String P_RFC
        {
            get { return RFC; }
            set { RFC = value; }
        }

        public String P_CURP
        {
            get { return CURP; }
            set { CURP = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Ruta_Foto
        {
            get { return Ruta_Foto; }
            set { Ruta_Foto = value; }
        }

        public String P_Nombre_Foto
        {
            get { return Nombre_Foto; }
            set { Nombre_Foto = value; }
        }

        public String P_No_IMSS
        {
            get { return No_IMSS; }
            set { No_IMSS = value; }
        }

        public String P_Forma_Pago
        {
            get { return Forma_Pago; }
            set { Forma_Pago = value; }
        }

        public String P_No_Cuenta_Bancaria
        {
            get { return No_Cuenta_Bancaria; }
            set { No_Cuenta_Bancaria = value; }
        }

        public DateTime P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }

        public String P_Tipo_Finiquito
        {
            get { return Tipo_Finiquito; }
            set { Tipo_Finiquito = value; }
        }

        public DateTime P_Fecha_Termino_Contrato
        {
            get { return Fecha_Termino_Contrato; }
            set { Fecha_Termino_Contrato = value; }
        }

        public DateTime P_Fecha_Baja
        {
            get { return Fecha_Baja; }
            set { Fecha_Baja = value; }
        }

        public Double P_Salario_Diario
        {
            get { return Salario_Diario; }
            set { Salario_Diario = value; }
        }

        public Double P_Salario_Diario_Integrado
        {
            get { return Salario_Diario_Integrado; }
            set { Salario_Diario_Integrado = value; }
        }

        public String P_Lunes
        {
            get { return Lunes; }
            set { Lunes = value; }
        }

        public String P_Martes
        {
            get { return Martes; }
            set { Martes = value; }
        }

        public String P_Miercoles
        {
            get { return Miercoles; }
            set { Miercoles = value; }
        }

        public String P_Jueves
        {
            get { return Jueves; }
            set { Jueves = value; }
        }

        public String P_Viernes
        {
            get { return Viernes; }
            set { Viernes = value; }
        }

        public String P_Sabado
        {
            get { return Sabado; }
            set { Sabado = value; }
        }

        public String P_Domingo
        {
            get { return Domingo; }
            set { Domingo = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        public DataTable P_Documentos_Anexos_Empleado
        {
            get { return Documentos_Anexos_Empleado; }
            set { Documentos_Anexos_Empleado = value; }
        }
        public String P_No_Licencia
        {
            get { return No_Licencia; }
            set { No_Licencia = value; }
        }
        public DateTime P_Fecha_Vigencia_Licencia
        {
            get { return Fecha_Vigencia_Licencia; }
            set { Fecha_Vigencia_Licencia = value; }
        }
        public String P_No_Movimiento
        {
            get { return No_Movimiento; }
            set { No_Movimiento = value; }
        }
        public String P_Tipo_Movimiento
        {
            get { return Tipo_Movimiento; }
            set { Tipo_Movimiento = value; }
        }
        public String P_Motivo_Movimiento
        {
            get { return Motivo_Movimiento; }
            set { Motivo_Movimiento = value; }
        }
        public Double P_Sueldo_Actual
        {
            get { return Sueldo_Actual; }
            set { Sueldo_Actual = value; }
        }
        public String P_Fecha_Inicio_Busqueda
        {
            get { return Fecha_Inicio_Busqueda; }
            set { Fecha_Inicio_Busqueda = value; }
        }
        public String P_Fecha_Fin_Busqueda
        {
            get { return Fecha_Fin_Busqueda; }
            set { Fecha_Fin_Busqueda = value; }
        }
        public DataTable P_Dt_Tipo_Nomina_Lista_Percepciones
        {
            get { return Dt_Tipo_Nomina_Lista_Percepciones; }
            set { Dt_Tipo_Nomina_Lista_Percepciones = value; }
        }
        public DataTable P_Dt_Tipo_Nomina_Lista_Deducciones
        {
            get { return Dt_Tipo_Nomina_Lista_Deducciones; }
            set { Dt_Tipo_Nomina_Lista_Deducciones = value; }
        }
        public DataTable P_Dt_Sindicato_Lista_Percepciones
        {
            get { return Dt_Sindicato_Lista_Percepciones; }
            set { Dt_Sindicato_Lista_Percepciones = value; }
        }
        public DataTable P_Dt_Sindicato_Lista_Deducciones
        {
            get { return Dt_Sindicato_Lista_Deducciones; }
            set { Dt_Sindicato_Lista_Deducciones = value; }
        }
        public String P_Concepto
        {
            get { return Concepto; }
            set { Concepto = value; }
        }

        public Double P_Salario_Mensual_Actual
        {
            get { return Salario_Mensual_Actual; }
            set { Salario_Mensual_Actual = value; }
        }

        public String P_Banco_ID
        {
            get { return Banco_ID; }
            set { Banco_ID = value; }
        }

        public String P_Reloj_Checador
        {
            get { return Reloj_Checador; }
            set { Reloj_Checador = value; }
        }

        public String P_No_Tarjeta
        {
            get { return No_Tarjeta; }
            set { No_Tarjeta = value; }
        }
        //Sap Código Programático.
        public String P_SAP_Unidad_Responsable
        {
            get { return SAP_Unidad_Responsable_ID; }
            set { SAP_Unidad_Responsable_ID = value; }
        }

        public String P_SAP_Fuente_Financiamiento
        {
            get { return SAP_Fuente_Financiamiento_ID; }
            set { SAP_Fuente_Financiamiento_ID = value; }
        }

        public String P_SAP_Area_Responsable_ID
        {
            get { return SAP_Area_Responsable_ID; }
            set { SAP_Area_Responsable_ID = value; }
        }

        public String P_SAP_Programa_ID
        {
            get { return SAP_Programa_ID; }
            set { SAP_Programa_ID = value; }
        }

        public String P_SAP_Partida_ID
        {
            get { return SAP_Partida; }
            set { SAP_Partida = value; }
        }

        public String P_SAP_Codigo_Programatico
        {
            get { return SAP_Codigo_Programatico; }
            set { SAP_Codigo_Programatico = value; }
        }

        public String P_Nomina_ID
        {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }

        public String P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }

        public String P_Estatus_Incidencia
        {
            get { return Estatus_Incidencia; }
            set { Estatus_Incidencia = value; }
        }

        public String P_Tipo_Incidencia
        {
            get { return Tipo_Incidencia; }
            set { Tipo_Incidencia = value; }
        }

        public String P_Fecha_Inicio_Busqueda_Incidencia
        {
            get { return Fecha_Inicio_Busqueda_Incidencia; }
            set { Fecha_Inicio_Busqueda_Incidencia = value; }
        }

        public String P_Fecha_Fin_Busqueda_Incidencia
        {
            get { return Fecha_Fin_Busqueda_Incidencia; }
            set { Fecha_Fin_Busqueda_Incidencia = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_No_Seguro_Poliza
        {
            get { return No_Seguro_Poliza; }
            set { No_Seguro_Poliza = value; }
        }
        public String P_Beneficiario_Seguro
        {
            get { return Beneficiario_Seguro; }
            set { Beneficiario_Seguro = value; }
        }

        public String P_Tipo_Empleado {
            get { return Tipo_Empleado; }
            set { Tipo_Empleado = value; }
        }

        public String P_Confronto {
            get { return Confronto; }
            set { Confronto = value; }
        }

        public String P_Aplica_ISSEG {
            get { return Aplica_ISSEG; }
            set { Aplica_ISSEG = value; }
        }

        public String P_Cuenta_Contable_ID
        {
            get { return Cuenta_Contable_ID; }
            set { Cuenta_Contable_ID = value; }
        }

        public String P_Fecha_Alta_ISSEG {
            get { return Fecha_Alta_ISSEG; }
            set { Fecha_Alta_ISSEG = value; }
        }

        public String P_Aplica_Baja_Licencia {
            get { return Aplica_Baja_Licencia; }
            set { Aplica_Baja_Licencia = value; }
        }

        public String P_Fecha_Inicio_Licencia
        {
            get { return Fecha_Inicio_Licencia; }
            set { Fecha_Inicio_Licencia = value; }
        }

        public String P_Fecha_Termino_Licencia
        {
            get { return Fecha_Termino_Licencia; }
            set { Fecha_Termino_Licencia = value; }
        }

        public String P_Fecha_Baja_IMSS
        {
            get { return Fecha_Baja_IMSS; }
            set { Fecha_Baja_IMSS = value; }
        }

        public String P_Tipo_Baja
        {
            get { return Tipo_Baja; }
            set { Tipo_Baja = value; }
        }
        #endregion

        #region (Metodos)
        public void Alta_Empleado()
        {
            Cls_Cat_Empleados_Datos.Alta_Empleado(this);
        }
        public void Modificar_Empleado()
        {
            Cls_Cat_Empleados_Datos.Modificar_Empleado(this);
        }
        public void Eliminar_Empleado()
        {
            Cls_Cat_Empleados_Datos.Eliminar_Empleado(this);
        }
        public DataTable Consulta_Empleados()
        {
            return Cls_Cat_Empleados_Datos.Consulta_Empleados(this);
        }
        public DataTable Consulta_Datos_Empleado()
        {
            return Cls_Cat_Empleados_Datos.Consulta_Datos_Empleado(this);
        }
        public DataTable Consulta_Usuario_Password()
        {
            return Cls_Cat_Empleados_Datos.Consulta_Usuario_Password(this);
        }
        public String Consulta_Id_Empleado()
        {
            return Cls_Cat_Empleados_Datos.Consulta_Id_Empleado(this);
        }
        public void Guardar_Documentos_Empleado(DataTable Documentos, String Empleado_ID)
        {
            Cls_Cat_Empleados_Datos.Guardar_Documentos_Empleado(Documentos, Empleado_ID);
        }
        public DataTable Consultar_Requisitos_Empleados()
        {
            return Cls_Cat_Empleados_Datos.Consultar_Requisitos_Empleados();
        }
        public DataTable Consultar_Requisitos_Empleados_Entregados()
        {
            return Cls_Cat_Empleados_Datos.Consultar_Requisitos_Empleados_Entregados(this);
        }
        public DataTable Consultar_Requisitos_Empleados_Entregados_Por_ID(String Empleado_ID, String Requisito_ID)
        {
            return Cls_Cat_Empleados_Datos.Consultar_Requisitos_Empleados_Entregados_Por_ID(Empleado_ID, Requisito_ID);
        }
        public DataTable Consultar_Tipos_Nomina()
        {
            return Cls_Cat_Empleados_Datos.Consultar_Tipos_Nomina();
        }
        public DataTable Consultar_Terceros()
        {
            return Cls_Cat_Empleados_Datos.Consultar_Terceros();
        }
        public DataTable Consulta_Empleados_Dependencia()
        {
            return Cls_Cat_Empleados_Datos.Consulta_Empleados_Dependencia(this);
        }
        public DataTable Consulta_Puestos_Empleados()
        {
            return Cls_Cat_Empleados_Datos.Consulta_Puestos_Empleados(this);
        }
        public DataTable Consulta_Empleados_General()
        {
            return Cls_Cat_Empleados_Datos.Consulta_Empleados_General(this);
        }
        public DataTable Consultar_Percepciones_Deducciones_Tipo_Nomina()
        {
            return Cls_Cat_Empleados_Datos.Consultar_Percepciones_Deducciones_Tipo_Nomina(this);
        }
        public DataTable Consultar_Informacion_Empleado_Incidencias()
        {
            return Cls_Cat_Empleados_Datos.Consultar_Informacion_Empleado_Incidencias(this);
        }
        public DataTable Consultar_Partidas(String Programa_ID)
        {
            return Cls_Cat_Empleados_Datos.Consultar_Partidas(Programa_ID);
        }

        public DataTable Consultar_Partida(String Partida_ID)
        {
            return Cls_Cat_Empleados_Datos.Consultar_Partida(Partida_ID);
        }

        public DataTable Consultar_Rpt_Empleados() {
            return Cls_Cat_Empleados_Datos.Consultar_Rpt_Empleados(this);
        }

        public DataTable Consulta_Rpt_Incidencias_Empleados() {
            return Cls_Cat_Empleados_Datos.Consulta_Rpt_Incidencias_Empleados(this);
        }

        public DataTable Consulta_Rpt_Vacaciones_Empleados() {
            return Cls_Cat_Empleados_Datos.Consulta_Rpt_Vacaciones_Empleados(this);
        }

        public DataTable Consulta_Rpt_Tiempo_Extra_Empleados() {
            return Cls_Cat_Empleados_Datos.Consulta_Rpt_Tiempo_Extra_Empleados(this);
        }

        public DataTable Consulta_Rpt_Conceptos_Empleados()
        {
            return Cls_Cat_Empleados_Datos.Consulta_Rpt_Conceptos_Empleados(this);
        }

        public DataTable Consulta_Rpt_Exentos_Gravados() {
            return Cls_Cat_Empleados_Datos.Consulta_Rpt_Exentos_Gravados(this);
        }

        public DataTable Consultar_Rpt_Totales_Nomina() {
            return Cls_Cat_Empleados_Datos.Consultar_Rpt_Totales_Nomina(this);
        }

        public DataTable Consultar_Movimientos_Empleados() {
            return Cls_Cat_Empleados_Datos.Consultar_Movimientos_Empleados(this);
        }

        public DataTable Consultar_Total_Pago_Bancos_Tipo_Nomina() {
            return Cls_Cat_Empleados_Datos.Consultar_Total_Pago_Bancos_Tipo_Nomina(this);
        }

        public DataTable Consultar_Nomina_Personal() {
            return Cls_Cat_Empleados_Datos.Consultar_Nomina_Personal(this);
        }

        public String Obtener_Puesto_ID(String Empleado_ID) {
            return Cls_Cat_Empleados_Datos.Obtener_Puesto_ID(Empleado_ID);
        }

        public DataTable Consultar_Puestos_Dependencia() {
            return Cls_Cat_Empleados_Datos.Consultar_Puestos_Dependencia(this);
        }

        public DataTable Consultar_Informacion_Mostrar_Finiquitos() {
            return Cls_Cat_Empleados_Datos.Consultar_Informacion_Mostrar_Finiquitos(this);
        }

        public DataTable Consultar_Informacion_Rpt_Finiquitos()
        {
            return Cls_Cat_Empleados_Datos.Consultar_Informacion_Rpt_Finiquitos(this);
        }
        public Boolean Cambiar_Password()
        {
            return Cls_Cat_Empleados_Datos.Cambiar_Password(this);
        }

        public DataTable JSON_Consulta_Empleados_General() {
            return Cls_Cat_Empleados_Datos.JSON_Consulta_Empleados_General(this);
        }

        public String Consultar_No_Empleado_Consecutivo() {
            return Cls_Cat_Empleados_Datos.Consultar_No_Empleado_Consecutivo();
        }

        public DataTable Consultar_Empleados_Resguardos()
        {
            return Cls_Cat_Empleados_Datos.Consultar_Empleados_Resguardos(this);
        }

        public Boolean Actualizar_Datos_Bancarios_Empleado() {
            return Cls_Cat_Empleados_Datos.Actualizar_Datos_Bancarios_Empleado(this);
        }

        public DataTable Consultar_Documentos() {
            return Cls_Cat_Empleados_Datos.Consultar_Documentos(this);
        }

        public DataTable Consultar_Aportacion()
        {
            return Cls_Rpt_Nom_Aportacion_Datos.Consultar_Aportacion(this);
        }
        #endregion

        #region (Reloj Checador)
        public DataTable Consulta_Datos_Reloj_Checador_Empleado()
        {
            return Cls_Cat_Empleados_Datos.Consulta_Datos_Reloj_Checador_Empleado(this);
        }
        public void Alta_Modificacion_Reloj_Checador()
        {
            Cls_Cat_Empleados_Datos.Alta_Modificacion_Reloj_Checador(this);
        }
        #endregion

        #region "ESPECIAL?"

        public DataTable Consulta_Empleados_Especial()
        {
            return Cls_Cat_Empleados_Datos.Consulta_Empleados_Especial(this);
        }

        #endregion
    }
}