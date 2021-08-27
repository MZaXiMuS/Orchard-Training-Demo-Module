﻿using Lombiq.TrainingDemo.GraphQL.Services;
using Lombiq.TrainingDemo.Models;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Apis;
using OrchardCore.ContentManagement.GraphQL.Queries;
using OrchardCore.Modules;

namespace Lombiq.TrainingDemo.GraphQL
{
    [RequireFeatures("OrchardCore.Apis.GraphQL")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddObjectGraphType<PersonPart, PersonPartObjectGraphType>();
            services.AddInputObjectGraphType<PersonPart, PersonPartWhereInputObjectGraphType>();
            services.AddTransient<IIndexAliasProvider, PersonPartIndexAliasProvider>();
        }
    }
}
