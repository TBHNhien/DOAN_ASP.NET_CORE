﻿//using Model.EF;
using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CategoryDao
    {
        OnlineShop10Context db = null;
        public CategoryDao()
        {
            db = new OnlineShop10Context();
        }

        public List<Category> ListAll()
        {
            //var connectionString = ConfigurationManager.ConnectionStrings["OnlineShopDbContext"].ToString();

            return db.Categories.Where(x => x.Status == true).ToList();
        }

        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
    }
}
