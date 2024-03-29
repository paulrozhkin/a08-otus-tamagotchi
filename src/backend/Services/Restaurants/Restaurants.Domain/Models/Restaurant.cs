﻿using System;
using System.Collections.Generic;
using Domain.Core.Models;

namespace Restaurants.Domain.Models
{
    public class Restaurant : BaseEntity
    {
        public string Title { get; set; }

        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsParkingPresent { get; set; }

        public bool IsCardPaymentPresent { get; set; }

        public bool IsWiFiPresent { get; set; }

        public List<Guid> Photos { get; set; }
    }
}