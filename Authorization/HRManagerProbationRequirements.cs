﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyBasedAuth.Authorization
{
    public class HRManagerProbationRequirement : IAuthorizationRequirement
    {
        public int Months { get; }
        public HRManagerProbationRequirement (int months)
        {
            this.Months = months;
        }
    }
    public class HRManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "EmploymentDate"))
                return Task.CompletedTask;

            var empDate = DateTime.Parse(context.User.FindFirst(x => x.Type == "EmploymentDate").Value);
            var period = DateTime.Now - empDate;
            if (period.Days > 30 * requirement.Months)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}