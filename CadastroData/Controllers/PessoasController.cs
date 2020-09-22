using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CadastroData.Data;
using CadastroData.Models;

namespace CadastroData.Controllers
{
    public class PessoasController : Controller
    {
        private CadastroDataContext db = new CadastroDataContext();

        private static bool IsBeforeNow(DateTime now, DateTime dateTime)
        {
            return dateTime.Month < now.Month
                || (dateTime.Month == now.Month && dateTime.Day < now.Day);
        }
        // GET: Pessoas
        [HttpGet]
        public ActionResult Index()
        {
            var ordenarAniversario = db.Pessoas.ToList();
           
            var now = DateTime.Now;
            var ordenar = from dt in ordenarAniversario
                            orderby IsBeforeNow(now, dt.DataNascimento), dt.DataNascimento.Month, dt.DataNascimento.Day 
                            select dt;

            return View(ordenar);
        }

        [HttpPost]
        public ActionResult Index(string Nome, string Sobrenome)
        {

            var pesquisa = from m in db.Pessoas
                           select m;

            if (!String.IsNullOrEmpty(Nome))
            {
                pesquisa = pesquisa.Where(s => s.Nome.Contains(Nome) && s.Sobrenome.Contains(Sobrenome));
            }

            return View(pesquisa);
        }

        [HttpGet]
        public ActionResult IndexAniverssario()
        {
            DateTime data = DateTime.Now;
            var pesquisa = from m in db.Pessoas
                           select m;

            pesquisa = pesquisa.Where(s => ((s.DataNascimento.Month == data.Month) && (s.DataNascimento.Day == data.Day)));

            return View(pesquisa);
        }

        // GET: Pessoas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = db.Pessoas.Find(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // GET: Pessoas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pessoas1/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Sobrenome,DataNascimento")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                db.Pessoas.Add(pessoa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pessoa);
        }
        // GET: Pessoas1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = db.Pessoas.Find(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // POST: Pessoas1/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Sobrenome,DataNascimento")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pessoa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pessoa);
        }

        // GET: Pessoas1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = db.Pessoas.Find(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // POST: Pessoas1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pessoa pessoa = db.Pessoas.Find(id);
            db.Pessoas.Remove(pessoa);
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
