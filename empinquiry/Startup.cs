﻿using Microsoft.Owin;
using Owin;
[assembly: OwinStartupAttribute(typeof(empinquiry.Startup))]
namespace empinquiry

{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Loggers.Log("Owin Startup Configuration called.");
            ConfigureAuth(app);
        }
    }
}
