using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Papeleria2.Models;

namespace Papeleria2.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        private PapeleriaContext db = new PapeleriaContext();

        // GET: Producto
        public ActionResult Index()
        {
            var productos = db.Productos.Include(p => p.Categorias);
            return View(productos.ToList());
        }

        // GET: Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            ViewBag.id_categoria = new SelectList(db.Categorias, "id", "nombre");
            return View();
        }

        // POST: Producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,descripcion,precio,imagen,existencia,stock,id_categoria")] Productos productos)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(productos);
                db.SaveChanges();
                int id = productos.id;
                var prod = db.Productos.Find(id);
                DateTime hoy = DateTime.Now;
                prod.ult_actualizacion = hoy;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_categoria = new SelectList(db.Categorias, "id", "nombre", productos.id_categoria);
            return View(productos);
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_categoria = new SelectList(db.Categorias, "id", "nombre", productos.id_categoria);
            return View(productos);
        }

        // POST: Producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,descripcion,precio,ult_actualizacion,imagen,existencia,stock,id_categoria")] Productos productos)
        {
            if (ModelState.IsValid)
            {
                int id = productos.id;
                var prod = db.Productos.Find(id);
                decimal precio_ant = prod.precio;
                decimal precio_act = productos.precio;

                prod.nombre = productos.nombre;
                prod.descripcion = productos.descripcion;
                prod.precio = productos.precio;
                prod.imagen = productos.imagen;
                prod.existencia = productos.existencia;
                prod.stock = productos.stock;
                prod.id_categoria = productos.id_categoria;

                if (precio_act != precio_ant)
                {
                    DateTime hoy = DateTime.Now;
                    prod.ult_actualizacion = hoy;
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.id_categoria = new SelectList(db.Categorias, "id", "nombre", productos.id_categoria);
            return View(productos);
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Productos productos = db.Productos.Find(id);
            db.Productos.Remove(productos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
