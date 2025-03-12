using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameList.API.Client
{
    internal interface IClient<T>
    {
        Task<T[]> GetAsync(object item);
    }
}
