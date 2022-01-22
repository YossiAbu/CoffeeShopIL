using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCartMVC.Models;
using System.IO;
using System.Data;
namespace ShoppingCartMVC.Controllers
{
    public class ProductsController : Controller
    {
        CoffeeShopDBEntities db = new CoffeeShopDBEntities();

        #region showing all products for admin 

        public ActionResult Index()
        {
            var query = db.viewallproduct.ToList();
            return View(query);
        }

        #endregion


        #region products add for admin

        public ActionResult Create()
        {
            List<Categories> list = db.Categories.ToList();
            ViewBag.CatList = new SelectList(list, "CatId", "Name");
            return View();
        }



        [HttpPost]
        public ActionResult Create(Products p , HttpPostedFileBase Image)
        {
            List<Categories> list = db.Categories.ToList();
            ViewBag.CatList = new SelectList(list, "CatId", "Name");

            
            if (ModelState.IsValid)
            {
                

                Products pro = new Products();
                pro.Name = p.Name;
                pro.Description = p.Description;
                pro.Unit = p.Unit;
                pro.Image = Image.FileName.ToString();
                pro.CatId = p.CatId;

                //image upload
                var folder = Server.MapPath("~/Uploads/");
                Image.SaveAs(Path.Combine(folder, Image.FileName.ToString()));

                db.Products.Add(pro);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                TempData["msg"] = "Product Not Upload";
            }
            return View();
        }


        #endregion


        #region edit products

        public ActionResult Edit(int id)
        {

            List<Categories> list = db.Categories.ToList();
            ViewBag.CatList = new SelectList(list, "CatId", "Name");

            var query = db.Products.SingleOrDefault(m => m.ProID == id);

            return View(query);
        }

    
        [HttpPost]
        public ActionResult Edit(Products p, HttpPostedFileBase Image)
        {
                  List<Categories> list = db.Categories.ToList();
                  ViewBag.CatList = new SelectList(list, "CatId", "Name");

                  try
                  {
                        if (p.Image != null)
                        {
                            p.Image = Image.FileName.ToString();
                            var folder = Server.MapPath("~/Uploads/");
                            Image.SaveAs(Path.Combine(folder, Image.FileName.ToString()));
                        }
                      db.Entry(p).State = (System.Data.Entity.EntityState)EntityState.Modified;
                      db.SaveChanges();
                  }
                 catch(Exception ex){
                     TempData["msg"] = ex;
                 }
     
              return RedirectToAction("Index");
                 
        }

        #endregion


        #region delete product 

        public ActionResult Delete(int id)
        {
                var query = db.Products.SingleOrDefault(m => m.ProID == id); 
                db.Products.Remove(query);
                
                db.SaveChanges();

            
            return RedirectToAction("Index");

        }

        #endregion

    }
}