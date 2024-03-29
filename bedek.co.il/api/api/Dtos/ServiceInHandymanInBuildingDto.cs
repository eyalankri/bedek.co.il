﻿using System;

namespace api.Dtos
{
    public class ServiceInHandymanInBuildingDto
    {
        public Guid? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ServiceName { get; set; }
        public int ServiceId { get; set; }
        public string Company { get; set; }
        public string InsertOrDelete { get; set; }
        public int? BuildingId { get; set; }
        public bool IsAssociated { get; set; }
    }
}

 