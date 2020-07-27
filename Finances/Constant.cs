using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

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