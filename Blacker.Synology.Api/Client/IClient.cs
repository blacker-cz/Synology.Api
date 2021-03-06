﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blacker.Synology.Api.Client
{
    internal interface IClient : IDisposable
    {
        Task<T> GetAsync<T>(string path, IDictionary<string, object> parameters) where T : class;
        Task GetAsync(string path, IDictionary<string, object> parameters);

        Task<T> PostAsync<T>(string path, IDictionary<string, object> parameters) where T : class;
        Task PostAsync(string path, IDictionary<string, object> parameters);
    }
}