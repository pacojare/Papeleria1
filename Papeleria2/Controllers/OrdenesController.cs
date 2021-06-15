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
    public class OrdenesController : Controller
    {
        private PapeleriaContext db = new PapeleriaContext();

        // GET: Ordenes
        public ActionResult Index()
        {
            var ordenes = db.Ordenes.Where(o => o.fecha_envio == null).OrderBy(o => o.fecha_creacion).Include(o => o.Clientes).Include(o => o.Paqueterias);
            return View(ordenes.ToList());
        }

        public ActionResult Index1()
        {
            var ordenes = db.Ordenes.Where(o => o.fecha_entrega == null && o.fecha_envio != null).OrderBy(o => o.fecha_creacion).Include(o => o.Clientes).Include(o => o.Paqueterias);
            return View(ordenes.ToList());
        }

        // GET: Ordenes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordenes ordenes = db.Ordenes.Find(id);
            if (ordenes == null)
            {
                return HttpNotFound();
            }
            return View(ordenes);
        }

        // GET: Ordenes/Create
        public ActionResult Create()
        {
            ViewBag.id_cliente = new SelectList(db.Clientes, "id", "nombre");
            ViewBag.id_paqueteria = new SelectList(db.Paqueterias, "id", "nombre");
            return View();
        }

        // POST: Ordenes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fecha_creacion,total,num_guia,fecha_entrega,fecha_envio,id_cliente,dir_entrega,id_paqueteria,status")] Ordenes ordenes)
        {
            if (ModelState.IsValid)
            {
                db.Ordenes.Add(ordenes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cliente = new SelectList(db.Clientes, "id", "nombre", ordenes.id_cliente);
            ViewBag.id_paqueteria = new SelectList(db.Paqueterias, "id", "nombre", ordenes.id_paqueteria);
            return View(ordenes);
        }

        // GET: Ordenes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordenes ordenes = db.Ordenes.Find(id);
            if (ordenes == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente = new SelectList(db.Clientes, "id", "nombre", ordenes.id_cliente);
            ViewBag.id_paqueteria = new SelectList(db.Paqueterias, "id", "nombre", ordenes.id_paqueteria);
            return View(ordenes);
        }

        // GET: Ordenes/Edit/5
        public ActionResult Edit1(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordenes ordenes = db.Ordenes.Find(id);
            if (ordenes == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente = new SelectList(db.Clientes, "id", "nombre", ordenes.id_cliente);
            ViewBag.id_paqueteria = new SelectList(db.Paqueterias, "id", "nombre", ordenes.id_paqueteria);
            return View(ordenes);
        }

        // POST: Ordenes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,num_guia,fecha_envio,id_paqueteria, status")] Ordenes ordenes)
        {
            if (ModelState.IsValid)
            {
                Ordenes o = db.Ordenes.Find(ordenes.id);
                o.id_paqueteria = ordenes.id_paqueteria;
                o.num_guia = ordenes.num_guia;
                o.fecha_envio = ordenes.fecha_envio;
                o.status = ordenes.status;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cliente = new SelectList(db.Clientes, "id", "nombre", ordenes.id_cliente);
            ViewBag.id_paqueteria = new SelectList(db.Paqueterias, "id", "nombre", ordenes.id_paqueteria);
            return View(ordenes);
        }

        // POST: Ordenes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit1([Bind(Include = "id,fecha_entrega, id_paqueteria")] Ordenes ordenes)
        {
            if (ModelState.IsValid)
            {
                Ordenes o = db.Ordenes.Find(ordenes.id);
                o.fecha_entrega = ordenes.fecha_entrega;
                db.SaveChanges();
                return RedirectToAction("Index1");
            }
            ViewBag.id_cliente = new SelectList(db.Clientes, "id", "nombre", ordenes.id_cliente);
            ViewBag.id_paqueteria = new SelectList(db.Paqueterias, "id", "nombre", ordenes.id_paqueteria);
            return View(ordenes);
        }


        // GET: Ordenes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordenes ordenes = db.Ordenes.Find(id);
            if (ordenes == null)
            {
                return HttpNotFound();
            }
            return View(ordenes);
        }

        // POST: Ordenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ordenes ordenes = db.Ordenes.Find(id);
            db.Ordenes.Remove(ordenes);
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
