﻿namespace DiConfigurationTest
{
    using System;
    using Microsoft.Extensions.Options;

    public interface IWritableOptions<out T> : IOptionsSnapshot<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }
}
