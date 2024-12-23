using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Service
{
    public class MainProductService
    {
        MainProductRepository mainProductRepository;
        public MainProductService() {
            mainProductRepository = new MainProductRepository();
        }

        public ProductSimple GetAllProduct()
        {
            return null;
        }

    }
}