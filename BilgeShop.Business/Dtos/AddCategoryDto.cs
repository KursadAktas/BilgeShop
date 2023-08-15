using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Dtos
{
    public class AddCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        // Id taşımıyorum çünkü yeni eklenen bir veride Id zater her zaman 0
    }
}
