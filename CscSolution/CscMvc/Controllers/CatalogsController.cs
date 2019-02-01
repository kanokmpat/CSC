using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CscMvc.Models;
using PagedList;
using Newtonsoft.Json.Linq;

namespace CscMvc.Controllers
{
    public class CatalogsController : Controller
    {
        private DATA_CONN db = new DATA_CONN();


        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        // GET: Catalogs
        public ActionResult Index(int? page,int? pageSize,string Search_Name)
        {

            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            //Ddefault size is 5 otherwise take pageSize value  
            int defaSize = (pageSize ?? 10);
            ViewBag.psize = defaSize;
            TotalItems = 0;
            CurrentPage = 0;
            PageSize = 0;
            TotalPages = 0;
            StartPage = 0;
            EndPage = 0;
            ViewBag.PageSize = new List<SelectListItem>()
         {
            
             new SelectListItem() { Value="10", Text= "10" },
             new SelectListItem() { Value="15", Text= "15" },
             new SelectListItem() { Value="25", Text= "25" },
             new SelectListItem() { Value="50", Text= "50" },
         };
         
            IPagedList<tbSystemCatalog> Catalog_Table = null;
           Catalog_Table = db.tbSystemCatalog.OrderBy(r => r.CatalogID).ToPagedList(pageIndex, defaSize);
            System.Diagnostics.Debug.WriteLine("set_"+ Catalog_Table.Count());
             if (Search_Name != null)
             {
                var table_catalog = from data in db.tbSystemCatalog
                                   orderby data.CatalogCode ascending
                                    where data.CatalogCode.Contains(Search_Name) 
                                    select data;
                if (table_catalog.Count()==0)
                 {
                      table_catalog = from data in db.tbSystemCatalog
                                      orderby data.CatalogName ascending
                                      where data.CatalogName.Contains(Search_Name)
                                         select data;

                 }
                 if (table_catalog.Count() == 0)
                 {
                     table_catalog = from data in db.tbSystemCatalog
                                    orderby data.CatalogPrefix ascending
                                     where data.CatalogPrefix.Contains(Search_Name)
                                     select data;

                 }
                if (table_catalog.Count() == 0)
                {
                    table_catalog = from data in db.tbSystemCatalog
                                    orderby data.Remark ascending
                                    where data.Remark.Contains(Search_Name)
                                    select data;

                }
                ViewBag.Page = pageIndex;
                ViewBag.PageMax = pageSize;
                ViewBag.TotalPage = table_catalog.Count();
                return  View(table_catalog.ToPagedList(pageIndex, defaSize));
             }
             else
             {

                ViewBag.Page = pageIndex;
                ViewBag.PageMax = pageSize;
                ViewBag.TotalPage = Catalog_Table.Count();
                return View(Catalog_Table);
            }
            ViewBag.Page = pageIndex;
            ViewBag.PageMax = pageSize;
            ViewBag.TotalPage = Catalog_Table.Count();
            return View(Catalog_Table);
            //return View(Catalog_Table, Catalog_Table);
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
            var table_catalog = db.tbSystemCatalog.Max(u => u.CatalogID);
            System.Diagnostics.Debug.WriteLine("set_e"+ table_catalog.ToString());
            return View();
        }

        // POST: Catalogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CatalogID,CatalogCode,CatalogPrefix,CatalogName,OrderSeq,TypeID,ParentID,Remark,EditFlag,DeleteFlag,CreateDate,CreateBy,ModifyDate,ModifyBy,ReferenceCode")] tbSystemCatalog tbSystemCatalog)
        {

           

            //  var table_catalog = from data in db.tbSystemCatalog
            //     orderby data.CatalogPrefix ascending
            //   select data;


            //  System.Diagnostics.Debug.WriteLine("set_e_a" + Json(table_catalog, table_catalog.AllowGet));

            if (ModelState.IsValid)
            {
                db.tbSystemCatalog.Add(new tbSystemCatalog
                {
                    ID = 0,
                    CatalogCode = "",
                    CatalogName = "",
                    CreateDate = ""
                });
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
