using Application1.Repositories.Contract.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application1.Repositories.Repositories
{
    public class Repository1 : IRepository1
    {
        public IEnumerable<string> GetRepository1Names()
        {
            return new List<string> { "Jim", "Ronald", "Veronica" };
        }
    }
}
