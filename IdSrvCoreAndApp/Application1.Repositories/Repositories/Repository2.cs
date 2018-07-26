using Application1.Repositories.Contract.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application1.Repositories.Repositories
{
    public class Repository2 : IRepository2
    {
        public IEnumerable<string> GetRepository2Names()
        {
            return new List<string> { "Rachel", "Jeff", "Lisa" };
        }
    }
}
