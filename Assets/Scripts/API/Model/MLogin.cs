using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.API.Model
{
    [System.Serializable]
    public class MLogin
    {
        public int id;
        public string name;
        public string passwd;

        public MLogin(string user, string pass)
        {
            name = user;
            passwd = pass;
        }
    }



    [System.Serializable]
    public class MLoginList
    {
        public List<MLogin> logins;
    }

}
