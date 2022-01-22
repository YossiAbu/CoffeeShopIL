using ShoppingCartMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCartMVC.Controllers
{
    public class SitController : Controller
    {
        CoffeeShopDBEntities db = new CoffeeShopDBEntities();

        public ActionResult Index()
        {
            var query = db.getTableSits.ToList();
            return View(query);
        }

        #region Creating sits
        public ActionResult Create()
        {
            List<Tables> list = db.Tables.ToList();
            ViewBag.TableList = new SelectList(list, "tableId","tableId");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Tables tbl)
        {
            List<Tables> list = db.Tables.ToList();
            ViewBag.TableList = new SelectList(list, "tableId", "tableId");
            if (ModelState.IsValid)
            {
                Sits s = new Sits();
                s.available = 1;
                s.tableId = tbl.tableId;
                db.Sits.Add(s);
                var query = db.Tables.SingleOrDefault(m => m.tableId == s.tableId);
                query.numSeats += 1;
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

        #region Save/Release Sit
        public ActionResult SaveSit(int SitId,int userId)
        {
            var query = db.Sits.SingleOrDefault(m => m.sitId == SitId);
            if (query.available == 1)
            {
                query.userId = userId;
                query.available = 0;
                db.SaveChanges();
            }
            else
            {
                TempData["msg"] = "Didn't save sit";
            }
            return RedirectToAction("Checkout", "Home");
        }

        public ActionResult ReleaseSit(int SitId)
        {
            var query = db.Sits.SingleOrDefault(m => m.sitId == SitId);
            if (query.available == 0)
            {
                query.available = 1;
                query.userId = null;
                db.SaveChanges();
            }
            else
            {
                TempData["msg"] = "Didn't release sit";
            }
            return RedirectToAction("Index", "Table");
        }
        #endregion

        #region Remove Sit
        public ActionResult Delete(int id)
        {
            
            var query = db.Sits.SingleOrDefault(m => m.sitId == id);
            var TablesQwery = db.Tables.SingleOrDefault(m => m.tableId == query.tableId);
            TablesQwery.numSeats -= 1;
            db.Sits.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}