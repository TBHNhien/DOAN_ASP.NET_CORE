﻿using Model.Dao;
//using Model.EF;

using DOAN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using DOAN.Models;

namespace OnlineShop_10.Areas.Admin.Controllers
{
    public class UserController : Controller
    {

        public int page = 1;
        public int pageSize = 1;
        // GET: Admin/User
        public IActionResult Index(string searchString,int page = 1 , int pageSize = 10)
        {
            var dao = new UserDao();

            var model = dao.ListAllPaging(searchString,page, pageSize);
            
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();

                var encrytedMD5Pas = Encryptor.MD5Hash(user.Password);
                user.Password = encrytedMD5Pas;
                long id = dao.Insert(user);
                if (id > 0)
                {
                    //SetAlert("Thêm user thành công","success");//ASP.NET
                    TempData["SuccessMessage"] = "Thêm user thành công";
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user k thành công");
                }
            }
            return View("Index");
        }

        public static string oldPass;
        public IActionResult Edit(int id)
        {
            var user = new UserDao().ViewDetail(id);
            oldPass = user.Password;
            //if (Request.QueryString["errorMessage"] != null)
            //{
            //    ModelState.AddModelError("", Request.QueryString["errorMessage"].ToString());
            //}
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["errorMessage"]))
            {
                ModelState.AddModelError("", HttpContext.Request.Query["errorMessage"]);
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var encrytedMD5Pas = Encryptor.MD5Hash(user.Password);

                if (!string.IsNullOrEmpty(user.Password))
                {
                    //var encrytedMD5Pas = Encryptor.MD5Hash(user.Password);
                    user.Password = encrytedMD5Pas;
                }

                if (user.Password == oldPass)
                {
                    //var model = dao.ListAllPaging(searchString,page, pageSize);
                    //ModelState.AddModelError("", "Vui lòng nhập mật khẩu khác mật khẩu cũ");
                    //return View("Index",model);
                    return RedirectToAction("Edit", "User", new { errorMessage = "Vui lòng nhập mật khẩu khác mật khẩu cũ" });
                }




                var result = dao.Update(user, oldPass);
                if (result)
                {
                    //SetAlert("Sửa user thành công", "success");
                    TempData["SuccessMessage"] = "Sửa user thành công";

                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user Không thành công");
                }
            }
            return View("Index");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            new UserDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}