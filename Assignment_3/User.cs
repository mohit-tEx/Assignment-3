using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_3
{
    internal class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }

        public override string ToString()
        {
            return $"Name: {Name} && Username: {Username}";
        }
    }
}
