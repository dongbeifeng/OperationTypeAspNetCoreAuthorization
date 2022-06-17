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
using Microsoft.AspNetCore.Mvc.Filters;
using static OperationTypeAspNetCoreAuthorization.Constants;

namespace OperationTypeAspNetCoreAuthorization;

/// <summary>
/// 用于标记 Action 的操作类型。
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class OperationTypeAttribute : AuthorizeAttribute, IActionFilter
{

    /// <summary>
    /// 初始化新实例。
    /// </summary>
    /// <param name="operationType"></param>
    public OperationTypeAttribute(string operationType)
    {
        this.OperationType = operationType;
    }

    /// <summary>
    /// 获取或设置操作类型。
    /// </summary>
    public string OperationType
    {
        get
        {
            if (Policy == null)
            {
                throw new Exception();
            }
            return Policy[POLICY_PREFIX.Length..];
        }
        set
        {
            Policy = $"{POLICY_PREFIX}{value}";
        }
    }

    /// <summary>
    /// 实现 <see cref="IActionFilter.OnActionExecuted(ActionExecutedContext)"/> 方法。
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    /// <summary>
    /// 实现 <see cref="IActionFilter.OnActionExecuting(ActionExecutingContext)"/> 方法。
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items[typeof(OperationTypeAttribute)] = OperationType;
    }
}
