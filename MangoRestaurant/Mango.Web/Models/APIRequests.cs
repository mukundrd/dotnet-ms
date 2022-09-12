﻿using Microsoft.AspNetCore.Mvc;
using static MangoWeb.SD;

namespace MangoWeb.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;

        public string Url { get; set; }

        public object Data { get; set; }

        public string AccessToken { get; set; }
    }
}