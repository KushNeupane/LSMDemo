using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class GroupsViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int weight { get; set; }
    }
}