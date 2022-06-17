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
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;
using static NSubstitute.Substitute;
using static OperationTypeAspNetCoreAuthorization.Constants;

namespace OperationTypeAspNetCoreAuthorization.Tests
{
    public class OperationTypeAuthorizationPolicyProviderTests
    {
        [Fact]
        public async Task GetPolicyAsyncTest()
        {
            OperationTypeAuthorizationPolicyProvider sut = new OperationTypeAuthorizationPolicyProvider(
                For<IOptions<AuthorizationOptions>>(),
                For<ILogger<OperationTypeAuthorizationPolicyProvider>>()
                );

            var policy = await sut.GetPolicyAsync($"{POLICY_PREFIX}TEST");
            Assert.NotNull(policy);
            Assert.Equal(2, policy.Requirements.Count);
            Assert.IsType<DenyAnonymousAuthorizationRequirement>(policy.Requirements[0]);
            Assert.IsType<AssertionRequirement>(policy.Requirements[1]);
        }
    }
}