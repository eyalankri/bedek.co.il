using System;

namespace api.Dtos
{
    public class ServiceInHandymanDto
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int WarrantyPeriodInMonths { get; set; }
        public Guid? UserId { get; set; }
        public string InsertOrDelete { get; set; }

    }
}
