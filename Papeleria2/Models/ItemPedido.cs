using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Papeleria2.Models
{
    public class ItemPedido
    {
        public int idOrd
        {
            get;
            set;
        }

        public Productos Product
        {
            get;
            set;
        }

        public int cantidad
        {
            get;
            set;
        }

    }
}