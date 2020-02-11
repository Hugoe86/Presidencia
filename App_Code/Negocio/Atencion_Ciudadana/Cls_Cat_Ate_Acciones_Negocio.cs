using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Acciones_AC.Datos;

namespace Presidencia.Acciones_AC.Negocio
{
    public class Cls_Cat_Ate_Acciones_Negocio
    {
        #region VARIABLES INTERNAS
        private String ID;
        private String Clave;
        private String Nombre;
        private int Tiempo_Estimado_Solucion;
        private String Descripcion;
        private String Estatus;
        #endregion

        #region VARIABLES PUBLICAS

        public String P_ID
        {
            get { return ID; }
            set { ID = value; }
        }
        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public int P_Tiempo_Estimado_Solucion
        {
            get { return Tiempo_Estimado_Solucion; }
            set { Tiempo_Estimado_Solucion = value; }
        }
        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        #endregion

        #region METODOS
        public Cls_Cat_Ate_Acciones_Negocio()
        {
        }
        public int Guardar_Registro()
        {
            return Cls_Cat_Ate_Acciones_Datos.Guardar_Registro(this);
        }

        public int Actualizar_Registro()
        {
            return Cls_Cat_Ate_Acciones_Datos.Actualizar_Registro(this);
        }

        public int Eliminar_Registro()
        {
            return Cls_Cat_Ate_Acciones_Datos.Eliminar_Registro(this);
        }

        public DataTable Consultar_Registros()
        {
            return Cls_Cat_Ate_Acciones_Datos.Consultar_Registros(this);
        }

        public bool Clave_Duplicada()
        {
            return Cls_Cat_Ate_Acciones_Datos.Clave_Duplicada(this); ;
        }

        #endregion

    }
}