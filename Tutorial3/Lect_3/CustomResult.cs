using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lect_3
{
    public class CustomResult : OkResult
    {
        private readonly string Reason;

        public CustomResult(string reason) : base()
        {
            Reason = reason;
        }

        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = Reason;
            context.HttpContext.Response.StatusCode = StatusCode;
        }
    }
}
