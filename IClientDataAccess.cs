using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    interface IClientDataAccess
    {
        void GetAll();
        void CreateUser();
        void GetClient(Guid guid);
        void UpdateClient(Client c);
        void DeleteClient(Guid guid);
    }
}
