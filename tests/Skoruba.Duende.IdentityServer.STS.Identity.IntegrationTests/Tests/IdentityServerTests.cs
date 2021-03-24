﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Skoruba.Duende.IdentityServer.STS.Identity.Configuration.Test;
using Skoruba.Duende.IdentityServer.STS.Identity.IntegrationTests.Tests.Base;
using Xunit;

namespace Skoruba.Duende.IdentityServer.STS.Identity.IntegrationTests.Tests
{
    public class IdentityServerTests : BaseClassFixture
    {
        public IdentityServerTests(WebApplicationFactory<StartupTest> factory) : base(factory)
        {
        }

        [Fact]
        public async Task CanShowDiscoveryEndpoint()
        {
            var disco = await Client.GetDiscoveryDocumentAsync("http://localhost");

            disco.Should().NotBeNull();
            disco.IsError.Should().Be(false);

            disco.KeySet.Keys.Count.Should().Be(1);
        }
    }
}
