﻿using OngProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IUserService
    {
        public Task<bool> DeleteUser(int Id);
        public bool UserExists(int Id);
    }
}
