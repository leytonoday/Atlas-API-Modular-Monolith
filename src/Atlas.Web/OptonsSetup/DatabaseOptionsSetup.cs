﻿using Atlas.Shared.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Atlas.Web.OptonsSetup;

public class DatabaseOptionsSetup(IConfiguration configuration) : IConfigureOptions<DatabaseOptions>
{
    private const string SectionName = "DatabaseOptions";

    public void Configure(DatabaseOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
