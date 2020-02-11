using System;
using System.Data;
using Presidencia.Operacion_Predial_Recep_Docs_Movs.Datos;

namespace Presidencia.Operacion_Predial_Recep_Docs_Movs.Negocio
{
    public class Cls_Ope_Pre_Recep_Docs_Movs_Negocio
    {
        public Cls_Ope_Pre_Recep_Docs_Movs_Negocio()
        {
        }

/// --------------------------------------- PROPIEDADES ---------------------------------------
#region PROPIEDADES

        private String No_Movimiento;
        private String No_Recepcion_Documento;
        private String Numero_Escritura;
        private String Fecha_Escritura;
        private String Cuenta_Predial_ID;
        private String Estatus;
        private String Observaciones;
        private String Empleado_ID;
        private String No_Contrarecibo;
        private String Nombre_Usuario;

#endregion


/// --------------------------------------- Propiedades públicas ---------------------------------------
#region (Propiedades Publicas)
        public String P_No_Movimiento
        {
            get { return No_Movimiento; }
            set { No_Movimiento = value; }
        }
        public String P_No_Recepcion_Documento
        {
            get { return No_Recepcion_Documento; }
            set { No_Recepcion_Documento = value; }
        }
        public String P_Numero_Escritura
        {
            get { return Numero_Escritura; }
            set { Numero_Escritura = value; }
        }
        public String P_Fecha_Escritura
        {
            get { return Fecha_Escritura; }
            set { Fecha_Escritura = value; }
        }
        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_No_Contrarecibo
        {
            get { return No_Contrarecibo; }
            set { No_Contrarecibo = value; }
        }
        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }

#endregion


/// --------------------------------------- Metodos ---------------------------------------
#region (Metodos)
        public int Alta_Movimiento()
        {
            return Cls_Ope_Pre_Recep_Docs_Movs_Datos.Alta_Movimiento(this);
        }
        public int Modificar_Movimiento()
        {
            return Cls_Ope_Pre_Recep_Docs_Movs_Datos.Modificar_Movimiento(this);
        }
        public DataTable Consulta_Datos_Movimiento()
        {
            return Cls_Ope_Pre_Recep_Docs_Movs_Datos.Consulta_Datos_Movimiento(this);
        }
        public DataTable Consulta_Movimiento()
        {
            return Cls_Ope_Pre_Recep_Docs_Movs_Datos.Consulta_Movimiento(this);
        }

#endregion (Metodos)

    }
}