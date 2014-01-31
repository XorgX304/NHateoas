﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHateoas.Configuration;

namespace NHateoas.Routes
{
    internal interface IRoutesBuilder
    {
        Dictionary<string, object> Build(IEnumerable<MappingRule> mappingRules, IRouteValueSubstitution routeValueSubstitution, Object data);
    }
}