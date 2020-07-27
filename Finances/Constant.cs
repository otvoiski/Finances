﻿using Microsoft.Extensions.Configuration;

namespace Finances
{
    public class Constant : IConstant
    {
        private readonly IConfiguration _configuration;

        public Constant(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }
    }

    public interface IConstant
    {
    }
}