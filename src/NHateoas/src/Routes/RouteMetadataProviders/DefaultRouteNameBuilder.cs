﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using NHateoas.Attributes;
using NHateoas.Configuration;

namespace NHateoas.Routes.RouteMetadataProviders
{
    internal class DefaultRouteNameBuilder : IRouteNameBuilder
    {
        public List<string> Build(MappingRule mappingRule, string method)
        {
            var methodName = method.ToLower();
            var name = new StringBuilder();
            
            var actionMethodInfo = mappingRule.MethodExpression.Method;

            var returnType = actionMethodInfo.ReturnType;

            if (typeof (HttpResponseMessage).IsAssignableFrom(returnType))
            {
                var attributes = actionMethodInfo.GetCustomAttributes<ResponseTypeAttribute>().ToList();
                if (attributes.Any())
                {
                    returnType = attributes.First().ResponseType;
                }
            }
                
            if (returnType.IsGenericType && typeof (IEnumerable<>).IsAssignableFrom(returnType.GetGenericTypeDefinition()))
            {
                returnType = returnType.GetGenericArguments()[0];

                methodName = "query";
            }

            name.Append(methodName);

            if (returnType != typeof(void))
                name.AppendFormat("_{0}", returnType.Name.ToLower());

            var parameters = actionMethodInfo.GetParameters();

            if (parameters.Any())
                name.AppendFormat("_by");

            foreach (var parameterInfo in parameters)
            {
                if (parameterInfo.GetType() == returnType)
                    continue;
                
                name.AppendFormat("_{0}", parameterInfo.Name.ToLower());
            }

            return new List<string> {name.ToString()};
        }
    }
}