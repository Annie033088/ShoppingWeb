using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Service
{
    public class ManUserService
    {
        private UserRepository userRepository;
        public ManUserService() { 
            userRepository = new UserRepository();
        }

        public List<Models.User> GetAllUsers() {
            return userRepository.GetAll().ToList();
        }

    }
}