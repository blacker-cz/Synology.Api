﻿using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Blacker.Synology.Api.Tests
{
    public static class AssertHelpers
    {
        public static async Task ThrowsAsync<TException>(Func<Task> func) where TException : class
        {
            await ThrowsAsync<TException>(func, exception => { });
        }

        public static async Task ThrowsAsync<TException>(Func<Task> func, Action<TException> action) where TException : class
        {
            var exception = default(TException);
            var expected = typeof(TException);
            Type actual = null;
            try
            {
                await func();
            }
            catch (Exception e)
            {
                exception = e as TException;
                actual = e.GetType();
            }

            Assert.AreEqual(expected, actual);
            action(exception);
        }
    }
}
