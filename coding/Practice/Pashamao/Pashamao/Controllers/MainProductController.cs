using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Service;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.SelectProduct | UserPermission.CreateProduct | UserPermission.EditProductName | UserPermission.EditProductDescription | UserPermission.EditProductPrice | UserPermission.EditProductQuantity | UserPermission.EditProductCategory | UserPermission.EditProductStatus | UserPermission.DelProduct)]
    public class MainProductController : Controller
    {
        MainProductService mainProductService;

        public MainProductController()
        {
            mainProductService = new MainProductService();
        }
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult GetAllProduct()
        {
            ProductSimple productSimple = mainProductService.GetAllProduct();
            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct() { return View(); }

        [HttpPost]
        public ActionResult EditProduct() { return View(); }

        [HttpPost]
        public ActionResult DeleteProduct() { return View(); }

    }
}