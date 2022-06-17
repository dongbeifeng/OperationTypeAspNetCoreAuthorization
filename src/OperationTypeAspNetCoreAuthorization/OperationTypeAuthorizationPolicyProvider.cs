// Copyright 2022 王建军
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static OperationTypeAspNetCoreAuthorization.Constants;

namespace OperationTypeAspNetCoreAuthorization;

public class OperationTypeAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    readonly ILogger<OperationTypeAuthorizationPolicyProvider> _logger;
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }


    public OperationTypeAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, ILogger<OperationTypeAuthorizationPolicyProvider> logger)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        _logger = logger;
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

    /// <summary>
    /// 指定授权策略，获取 <see cref="AuthorizationPolicy"/>。
    /// </summary>
    /// <param name="policyName">由 <see cref="OperationTypeAttribute"/> 传递的授权策略名。</param>
    /// <returns></returns>
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        _logger.LogDebug("授权策略名称: {policyName}", policyName);
        if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
        {
            string operationType = policyName[POLICY_PREFIX.Length..];
            _logger.LogDebug("从授权策略名称得到操作类型: {operationType}", operationType);

            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                    context.User.IsInRole("admin")      // 允许管理员做任何操作
                    || context.User.HasClaim(ClaimTypes.AllowedOperationType, operationType)   // 非管理员需要具备当前操作类型的权限
                )
                .Build();

            _logger.LogDebug("已为操作类型 {operationType} 生成授权策略", operationType);
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }
        
        _logger.LogDebug("授权策略名称不包含操作类型");
        return FallbackPolicyProvider.GetPolicyAsync(policyName);
    }
}

