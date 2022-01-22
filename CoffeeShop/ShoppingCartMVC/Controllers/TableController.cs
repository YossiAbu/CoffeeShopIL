using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCartMVC.Models;

namespace ShoppingCartMVC.Controllers
{
    public class TableController : Controller
    {
        CoffeeShopDBEntities db = new CoffeeShopDBEntities();
        
        #region Showing tables for admin
        public ActionResult Index()
        {
            var query = db.Tables.ToList();
            return View(query);
        }
        #endregion

        #region add categories
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Inside", Value = "inside" });
            items.Add(new SelectListItem { Text = "Outside", Value = "ouside" });
            ViewBag.AreaList = items;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Tables t)
        {
            if (ModelState.IsValid)
            {
                Tables tbl = new Tables();
                //tbl.area = table.area;
                tbl.area = t.area;
                tbl.numSeats = 0;
                db.Tables.Add(tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["msg"] = "Didn't create table";
            }
            return View();
        }
        #endregion

        #region Edit table
        public ActionResult Edit(int id)
        {
            var query = db.Tables.SingleOrDefault(m => m.tableId == id);
            return View(query);
        }

        [HttpPost]
        public ActionResult Edit(Tables tbl,string areas)
        {
            try
            {
                var query = db.Tables.SingleOrDefault(m => m.tableId == tbl.tableId);
                //if (tbl.numSeats != null)
                //    query.numSeats = tbl.numSeats;
                query.area = tbl.area;
                db.Entry(query).State = (System.Data.Entity.EntityState)EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex;
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region delete category
        public ActionResult Delete(int id)
        {
            var query = db.Tables.SingleOrDefault(m => m.tableId == id);
            db.Tables.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}