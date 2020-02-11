using System;
using Presidencia.Nodo_Atributos;

namespace Presidencia.Nodo_Arbol
{
    public class Cls_Nodo_Arbol
    {
        #region VARIABLES

            String texto_;
            String id_;
            String state_;
            Cls_Atributos_TreeGrid attributes_;
            Boolean selected_;
            String _parentId;
            String descripcion1_;
            String descripcion2_;
            String descripcion3_;
            String descripcion4_;
            String descripcion5_;

        #endregion

        #region METODOS

        public Boolean selected
        {
            get { return selected_; }
            set { selected_ = value; }
        }

        public Cls_Atributos_TreeGrid attributes
        {
            get { return attributes_; }
            set { attributes_ = value; }
        }

        public String texto
        {
            get { return texto_; }
            set { texto_ = value; }
        }

        public String id
        {
            get { return id_; }
            set { id_ = value; }
        }

        public String state
        {
            get { return state_; }
            set { state_ = value; }
        }

        public String parentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        public String descripcion1
        {
            get { return descripcion1_; }
            set { descripcion1_ = value; }
        }

        public String descripcion2
        {
            get { return descripcion2_; }
            set { descripcion2_ = value; }
        }

        public String descripcion3
        {
            get { return descripcion3_; }
            set { descripcion3_ = value; }
        }

        public String descripcion4
        {
            get { return descripcion4_; }
            set { descripcion4_ = value; }
        }

        public String descripcion5
        {
            get { return descripcion5_; }
            set { descripcion5_ = value; }
        }
        #endregion
    }
}