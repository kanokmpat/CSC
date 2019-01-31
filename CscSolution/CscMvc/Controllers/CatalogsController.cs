using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CscMvc.Models;

namespace CscMvc.Controllers
{
    public class CatalogsController : Controller
    {
        private DATA_CONN db = new DATA_CONN();

        // GET: Catalogs
        public ActionResult Index(string searchby, string Search)
        {

            if (Search != null)
            {
                return View(db.tbSystemCatalog.Where(x => x.CatalogName.Contains(Search) || Search == null).ToList());
            }
            else
            {
                return View(db.tbSystemCatalog.ToList());
            }

            //return View(db.tbSystemCatalog.ToList());
        }

        // GET: Catalogs/Details/5
        public ActionResult Details(int? id, int? CatalogID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbSystemCatalog tbSystemCatalog = db.tbSystemCatalog.Find(id, CatalogID);
            if (tbSystemCatalog == null)
            {
                return HttpNotFound();
            }
            return View(tbSystemCatalog);
        }

        // GET: Catalogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Catalogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CatalogID,CatalogCode,CatalogPrefix,CatalogName,OrderSeq,TypeID,ParentID,Remark,EditFlag,DeleteFlag,CreateDate,CreateBy,ModifyDate,ModifyBy,ReferenceCode")] tbSystemCatalog tbSystemCatalog)
        {
            if (ModelState.IsValid)
            {
                db.tbSystemCatalog.Add(tbSystemCatalog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbSystemCatalog);
        }

        // GET: Catalogs/Edit/5
        public ActionResult Edit(int? id ,int? CatalogID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbSystemCatalog tbSystemCatalog = db.tbSystemCatalog.Find(id,CatalogID);
            if (tbSystemCatalog == null)
            {
                return HttpNotFound();
            }
            return View(tbSystemCatalog);
        }

        // POST: Catalogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CatalogID,CatalogCode,CatalogPrefix,CatalogName,OrderSeq,TypeID,ParentID,Remark,EditFlag,DeleteFlag,CreateDate,CreateBy,ModifyDate,ModifyBy,ReferenceCode")] tbSystemCatalog tbSystemCatalog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbSystemCatalog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbSystemCatalog);
        }

        // GET: Catalogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbSystemCatalog tbSystemCatalog = db.tbSystemCatalog.Find(id);
            if (tbSystemCatalog == null)
            {
                return HttpNotFound();
            }
            return View(tbSystemCatalog);
        }

        // POST: Catalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbSystemCatalog tbSystemCatalog = db.tbSystemCatalog.Find(id);
            db.tbSystemCatalog.Remove(tbSystemCatalog);
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
