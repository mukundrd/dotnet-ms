﻿using Mango.Contracts.Dtos;
using Mango.Web.Models;

namespace Mango.Web.Services
{
    public interface IBaseService : IDisposable
    {
        ResponseDto responseModel { get; set; }

        Task<T?> SendAsync<T>(APIRequest apiRequest);
    }
}
