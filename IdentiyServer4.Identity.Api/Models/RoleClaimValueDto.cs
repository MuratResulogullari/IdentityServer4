using System.Collections.Generic;

namespace IdentiyServer4.Identity.Api.Models
{
    public  static class RoleClaimTypes
    {
        public const string SelectionAll = "all";
        public const string SelectionPartially = "partially";
        public const string SelectionNone = "none";
    }

    public class RoleClaimValueDto
    {
        public string SelectionType { get; set; }
        public List<string> PermittedValues { get; set; } // yetk verilen actionlar

        public RoleClaimValueDto()
        {
            PermittedValues = new List<string>();
        }

    }
   
}
