using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Papeleria2.Models;

namespace Papeleria2.Controllers
{
    public class CarroController : Controller
    {
        
        // GET: Carro
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult Agregar(int id)
        {
            
            ProductosCarro carro = new ProductosCarro();
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                Productos p = carro.find(id);
                String nam = p.nombre;
                cart.Add(new Item { Producto = carro.find(id), Cantidad = 1 });
                Session["cart"] = cart;
                
            }
            else
            {

                List<Item> cart = (List<Item>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Cantidad++;
                }
                else
                {
                    Productos p = carro.find(id);
                    String nam = p.nombre;
                    
                    cart.Add(new Item { Producto = carro.find(id), Cantidad = 1 });
                }
                Session["cart"] = cart;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Quitar(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            int index = isExist(id);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Index");
        }
        private int isExist(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Producto.id.Equals(id))
                    return i;
            return -1;
        }
    }
}