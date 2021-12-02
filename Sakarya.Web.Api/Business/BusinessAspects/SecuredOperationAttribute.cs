using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Business.Constants;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encyption;
using Core.Utilities.Security.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.BusinessAspects
{
    /// <summary>
    ///     This Aspect control the user's roles in HttpContext by inject the IHttpContextAccessor.
    ///     It is checked by writing as [SecuredOperation] on the handler.
    ///     If a valid authorization cannot be found in aspect, it throws an exception.
    /// </summary>
    public class SecuredOperationAttribute : MethodInterceptionAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly OperationClaimCrypto _operationClaimCrypto;

        public SecuredOperationAttribute()
        {
            Configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _operationClaimCrypto = Configuration.GetSection("OperationClaimCrypto").Get<OperationClaimCrypto>();
        }

        public IConfiguration Configuration { get; }

        protected override void OnBefore(IInvocation invocation)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
            if (userId == null) throw new UnauthorizedAccessException(Messages.UnauthorizedAccess);


            var oprClaims = _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type.EndsWith("role")).ToList();
            var ocNameList = new List<string>();
            foreach (var item in oprClaims)
            {
                var itemDecryptValue = SecurityKeyHelper.DecryptString(_operationClaimCrypto.Key, item.Value);
                ocNameList.Add(itemDecryptValue);
            }

            var operationName = invocation.TargetType.ReflectedType.Name;
            if (!ocNameList.Contains(operationName))
                throw new SecurityException(Messages.AuthorizationsDenied);
        }
    }
}