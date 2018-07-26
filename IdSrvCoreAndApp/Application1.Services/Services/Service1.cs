using Application1.Repositories.Contract.Interfaces;
using Application1.Services.Contract.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application1.Services.Services
{
    public class Service1 : IService1
    {
        private readonly IRepository1 _repository1;
        private readonly IRepository2 _repository2;

        public Service1(IRepository1 repository1, IRepository2 repository2)
        {
            _repository1 = repository1;
            _repository2 = repository2;
        }

        public IEnumerable<string> GetMeSomeNames()
        {
            return _repository1.GetRepository1Names().Concat(_repository2.GetRepository2Names());
        }
    }
}
