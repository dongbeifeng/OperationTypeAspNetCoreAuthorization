﻿// Copyright 2022 王建军
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

using Microsoft.AspNetCore.Http;

namespace OperationTypeAspNetCoreAuthorization;

public static class HttpContextExtensions
{
    public static string? GetOperationType(this HttpContext httpContext)
    {
        if (httpContext.Items.ContainsKey(typeof(OperationTypeAttribute)))
        {
            return httpContext.Items[typeof(OperationTypeAttribute)] as string;
        }
        return null;
    }
}