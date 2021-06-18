using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Papeleria2.Models;

namespace Papeleria2.Controllers
{
    public class CatalogoController : Controller
    {
        private PapeleriaContext db = new PapeleriaContext();
        
        // GET: Catalogo
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuscaProd (string nomBuscar)
        {
            ViewBag.SearchKey = nomBuscar;
            using (db)
            {
                var query = from st in db.Productos
                            where st.nombre.Contains(nomBuscar)
                            select st;
                var listProd = query.ToList();
                ViewBag.Listado = listProd;

            }
            return View();
        }

        public ActionResult prodCategoria(int idCat, String nombre)
        {
            using (db)
            {
                List<Productos> mercancia = null;
                var query = from p in db.Productos
                            where p.id_categoria == idCat
                            select p;
                ViewBag.Catego = nombre;
                mercancia = query.ToList();
                ViewBag.productos = mercancia;
            }
            return View();
            /* if (idCat == 1)
             {
                 List<Productos> oficina = query.ToList();
                 mercancia = query.ToList();
                 ViewBag.Catego = "Oficina";
             }
             if(idCat == 2)
             {
                // List<Productos> escolares = query.ToList();
                 mercancia = query.ToList(); 
                 ViewBag.Catego = "Escolares";

             }*/
        }
    }
}