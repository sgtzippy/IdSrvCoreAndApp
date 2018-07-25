using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application1.Models
{
    public class AboutViewModel
    {
        public string Address { get; private set; } = string.Empty;
        public string Data { get; private set; } = string.Empty;

        public AboutViewModel(string address, string data)
        {
            Address = address;
            Data = data;
        }
    }
}
