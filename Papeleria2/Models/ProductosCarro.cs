using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Papeleria2.Models;

namespace Papeleria2.Models
{
    public class ProductosCarro
    {
        private PapeleriaContext db = new PapeleriaContext();
        private List<Productos> products;

        public ProductosCarro(){
            products = db.Productos.ToList();
        }
        public List<Productos> findAll()
        {
            return this.products;
        }
        public Productos find(int id)
        {
            Productos pp = this.products.Single(p => p.id.Equals(id));
                return pp;
        } 
    
    }
    
}