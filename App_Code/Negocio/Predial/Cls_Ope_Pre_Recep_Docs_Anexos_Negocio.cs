using System;
using System.Data;
using Presidencia.Operacion_Predial_Recep_Docs_Anexos.Datos;

namespace Presidencia.Operacion_Predial_Recep_Docs_Anexos.Negocio
{
    public class Cls_Ope_Pre_Recep_Docs_Anexos_Negocio
    {
        public Cls_Ope_Pre_Recep_Docs_Anexos_Negocio()
        {
        }

/// --------------------------------------- PROPIEDADES ---------------------------------------
#region PROPIEDADES

        private String No_Anexo;
        private String No_Movimiento;
        private String Ruta;
        private String Comentarios;
        private String Documento_ID;
        private String Nombre_Usuario;

#endregion PROPIEDADES


/// --------------------------------------- Propiedades públicas ---------------------------------------
#region (Propiedades Publicas)
        public String P_No_Anexo
        {
            get { return No_Anexo; }
            set { No_Anexo = value; }
        }
        public String P_No_Movimiento
        {
            get { return No_Movimiento; }
            set { No_Movimiento = value; }
        }
        public String P_Ruta
        {
            get { return Ruta; }
            set { Ruta = value; }
        }
        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }
        public String P_Documento_ID
        {
            get { return Documento_ID; }
            set { Documento_ID = value; }
        }
        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }

#endregion


/// --------------------------------------- Metodos ---------------------------------------
#region (Metodos)
        public int Alta_Anexo()
        {
            return Cls_Ope_Pre_Recep_Docs_Anexos_Datos.Alta_Anexo(this);
        }
        public int Modificar_Anexo()
        {
            return Cls_Ope_Pre_Recep_Docs_Anexos_Datos.Modificar_Anexo(this);
        }
        public DataTable Consulta_Datos_Anexo()
        {
            return Cls_Ope_Pre_Recep_Docs_Anexos_Datos.Consulta_Datos_Anexo(this);
        }

#endregion (Metodos)

    }
}