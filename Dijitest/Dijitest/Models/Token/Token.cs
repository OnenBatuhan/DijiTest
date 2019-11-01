using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dijitest.Models.DataBase;

namespace Dijitest.Models.Token
{
    public class Token
    {
        public static Get_LoginResult GetToken(string Username , string Password)
        {
            Get_LoginResult Users = new Get_LoginResult();
            if (Username != "" && Password != "")
            {
                using (DijiArticleDataContext db = new DijiArticleDataContext())
                {
                    Users = db.Get_Login(Username, Password).FirstOrDefault();
                }
            }
            return Users;
        }
    }
}