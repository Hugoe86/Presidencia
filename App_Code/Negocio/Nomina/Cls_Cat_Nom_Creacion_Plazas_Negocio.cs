using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presidencia.Creacion_Plazas.Datos;
using System.Data;

namespace Presidencia.Creacion_Plazas.Negocio
{
    public class Cls_Cat_Nom_Creacion_Plazas_Negocio
    {
        #region (Variables Internas)
        private String Nombre;
        private String Estatus;
        private String Nombre_Usuario;
        private String Clave;
        private String Tipo_Plaza;
        private String Empleado_ID;
        private String No_Empleado;
        private String Puesto_ID;
        private String Fte_Financiamiento_ID;
        private String Area_Funcional_ID;
        private String Proyecto_Programa_ID;
        private String Unidad_Responsable_ID;
        private String Partida_ID;

        private String S_Fte_Financiamiento_ID;
        private String S_Area_Funcional_ID;
        private String S_Proyecto_Programa_ID;
        private String S_Unidad_Responsable_ID;
        private String S_Partida_ID;

        private String PSM_Fte_Financiamiento_ID;
        private String PSM_Area_Funcional_ID;
        private String PSM_Proyecto_Programa_ID;
        private String PSM_Unidad_Responsable_ID;
        private String PSM_Partida_ID;
        private String ur_q;
        #endregion

        #region (Variables Públicas)
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Tipo_Plaza
        {
            get { return Tipo_Plaza; }
            set { Tipo_Plaza = value; }
        }

        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }

        public String P_Puesto_ID
        {
            get { return Puesto_ID; }
            set { Puesto_ID = value; }
        }

        public String P_Fte_Financiamiento_ID
        {
            get { return Fte_Financiamiento_ID; }
            set { Fte_Financiamiento_ID = value; }
        }

        public String P_Area_Funcional_ID
        {
            get { return Area_Funcional_ID; }
            set { Area_Funcional_ID = value; }
        }

        public String P_Proyecto_Programa_ID
        {
            get { return Proyecto_Programa_ID; }
            set { Proyecto_Programa_ID = value; }
        }

        public String P_Unidad_Responsable_ID
        {
            get { return Unidad_Responsable_ID; }
            set { Unidad_Responsable_ID = value; }
        }

        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }

        public String P_S_Fte_Financiamiento_ID
        {
            get { return S_Fte_Financiamiento_ID; }
            set { S_Fte_Financiamiento_ID = value; }
        }

        public String P_S_Area_Funcional_ID
        {
            get { return S_Area_Funcional_ID; }
            set { S_Area_Funcional_ID = value; }
        }

        public String P_S_Proyecto_Programa_ID
        {
            get { return S_Proyecto_Programa_ID; }
            set { S_Proyecto_Programa_ID = value; }
        }

        public String P_S_Unidad_Responsable_ID
        {
            get { return S_Unidad_Responsable_ID; }
            set { S_Unidad_Responsable_ID = value; }
        }

        public String P_S_Partida_ID
        {
            get { return S_Partida_ID; }
            set { S_Partida_ID = value; }
        }

        public String P_PSM_Fte_Financiamiento_ID
        {
            get { return PSM_Fte_Financiamiento_ID; }
            set { PSM_Fte_Financiamiento_ID = value; }
        }

        public String P_PSM_Area_Funcional_ID
        {
            get { return PSM_Area_Funcional_ID; }
            set { PSM_Area_Funcional_ID = value; }
        }

        public String P_PSM_Proyecto_Programa_ID
        {
            get { return PSM_Proyecto_Programa_ID; }
            set { PSM_Proyecto_Programa_ID = value; }
        }

        public String P_PSM_Unidad_Responsable_ID
        {
            get { return PSM_Unidad_Responsable_ID; }
            set { PSM_Unidad_Responsable_ID = value; }
        }

        public String P_PSM_Partida_ID
        {
            get { return PSM_Partida_ID; }
            set { PSM_Partida_ID = value; }
        }

        public String P_ur_q {
            get { return ur_q; }
            set { ur_q = value; }
        }
        #endregion

        #region (Metodos)
        public DataTable Consulta_Dependencias() { return Cls_Cat_Nom_Creacion_Plazas_Datos.Consulta_Dependencias(this); }
        public DataTable Consultar_Sap_Det_Fte_Dependencia() { return Cls_Cat_Nom_Creacion_Plazas_Datos.Consultar_Sap_Det_Fte_Dependencia(this); }
        public DataTable Consultar_Sap_Det_Prog_Dependencia() { return Cls_Cat_Nom_Creacion_Plazas_Datos.Consultar_Sap_Det_Prog_Dependencia(this); }
        public DataTable Consultar_Partidas(String Programa_ID) { return Cls_Cat_Nom_Creacion_Plazas_Datos.Consultar_Partidas(Programa_ID); }
        public DataTable Consulta_Area_Funcional() { return Cls_Cat_Nom_Creacion_Plazas_Datos.Consulta_Area_Funcional(); }
        public DataTable Consultar_Puestos_UR() { return Cls_Cat_Nom_Creacion_Plazas_Datos.Consultar_Puestos_UR(this); }
        public Double Consultar_Comprometido_Sueldos() { return Cls_Cat_Nom_Creacion_Plazas_Datos.Consultar_Comprometido_Sueldos(this); }
        public Boolean Alta_Plaza() { return Cls_Cat_Nom_Creacion_Plazas_Datos.Alta_Plaza(this); }
        public Boolean Eliminar_Plaza() { return Cls_Cat_Nom_Creacion_Plazas_Datos.Eliminar_Plaza(this); }
        #endregion
    }
}
