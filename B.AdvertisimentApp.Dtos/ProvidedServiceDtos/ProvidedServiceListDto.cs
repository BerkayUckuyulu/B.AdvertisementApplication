
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.AdvertisementApp.Dtos
{
    public class ProvidedServiceListDto:IDto
    {
        public string? Title { get; set; }
        public string? ImagePath { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
