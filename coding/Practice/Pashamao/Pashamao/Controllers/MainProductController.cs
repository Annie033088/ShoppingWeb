using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Service;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.SelectProduct | UserPermission.CreateProduct | UserPermission.EditProductName | UserPermission.EditProductDescription | UserPermission.EditProductPrice | UserPermission.EditProductQuantity | UserPermission.EditProductCategory | UserPermission.EditProductStatus | UserPermission.DelProduct)]
    public class MainProductController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        MainProductService mainProductService;

        public MainProductController()
        {
            mainProductService = new MainProductService();
        }
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult ProductDetail()
        {

            return View();
        }

        [HttpPost]
        public ActionResult GetAllProduct(string page)
        {
            // ProductSimple productSimple = mainProductService.GetAllProduct(page);
            return View();
        }

        public ActionResult CreateProduct(ProductDetail productDetail, List<CreateProductStyleViewModel> StyleList, List<CreateProductImageViewModel> ImageList)
        {
            return View();
        }

        public ActionResult SubmitCreateProduct(ProductDetail productDetail, List<CreateProductStyleViewModel> StyleList, List<CreateProductImageViewModel> ImageList)
        {
            mainProductService.CreateProduct(productDetail, StyleList, ImageList);  
            return View();
        }

        [HttpPost]
        public ActionResult EditProduct() { return View(); }

        [HttpPost]
        public ActionResult DeleteProduct() { return View(); }

    }
}