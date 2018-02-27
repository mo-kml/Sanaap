using Bit.Test;
using Sanaap.Api;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sanaap.Test
{
    public class SanaapTestEnvironment : TestEnvironmentBase
    {
        public SanaapTestEnvironment(TestEnvironmentArgs args = null)
            : base(ApplyArgsDefaults(args))
        {

        }

        private static TestEnvironmentArgs ApplyArgsDefaults(TestEnvironmentArgs args)
        {
            args = args ?? new TestEnvironmentArgs();

            args.CustomAppModulesProvider = args.CustomAppModulesProvider ?? new SanaapAppModulesProvider();

            args.UseAspNetCore = true;

            return args;
        }

        protected override List<Func<TypeInfo, bool>> GetAutoProxyCreationIncludeRules()
        {
            List<Func<TypeInfo, bool>> baseList = base.GetAutoProxyCreationIncludeRules();

            baseList.Add(implementationType => implementationType.Assembly == typeof(SanaapAppModulesProvider).GetTypeInfo().Assembly);

            return baseList;
        }
    }
}
