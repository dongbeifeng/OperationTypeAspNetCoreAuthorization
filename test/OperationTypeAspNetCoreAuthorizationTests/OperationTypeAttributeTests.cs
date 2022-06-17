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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Xunit;
using static OperationTypeAspNetCoreAuthorization.Constants;
using static NSubstitute.Substitute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace OperationTypeAspNetCoreAuthorization.Tests;

public class OperationTypeAttributeTests
{
    [Fact()]
    public void OnActionExecutingTest()
    {
        OperationTypeAttribute sut = new OperationTypeAttribute("TEST");

        var actionContext = new ActionContext()
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };
        ActionExecutingContext actionExecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            For<Controller>()
            );

        sut.OnActionExecuting(actionExecutingContext);
        Assert.Equal("TEST", actionContext.HttpContext.Items[typeof(OperationTypeAttribute)]);
    }

    [Fact()]
    public void OperationTypeTest()
    {
        OperationTypeAttribute sut = new OperationTypeAttribute("TEST");
        Assert.Equal($"{POLICY_PREFIX}TEST", sut.Policy);
        Assert.Equal("TEST", sut.OperationType);
    }

}
