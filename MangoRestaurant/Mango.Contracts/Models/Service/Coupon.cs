﻿using System.ComponentModel.DataAnnotations;

namespace Mango.Contracts.Models.Service
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }

        public string CouponCode { get; set; }

        public double DiscountAmount { get; set; }
    }
}
