using System.Collections.Generic;

namespace API.Models
{
    public class IdentityModel
    {
        public string Id { get; set; }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
        public bool FullMatch = true;
    }
}
