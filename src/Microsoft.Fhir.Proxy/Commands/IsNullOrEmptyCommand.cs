﻿using Microsoft.Fhir.Proxy.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Fhir.Proxy.Commands
{
    public class IsNullOrEmptyCommand : IExceptionCommand
    {
        public IsNullOrEmptyCommand(JToken token, string jpath)
        {
            this.token = token;
            this.jpath = jpath;
        }

        private readonly JToken token;
        private readonly string jpath;

        public void Execute()
        {
            if(token.IsNullOrEmpty(jpath))
            {
                return;
            }

            throw new CommandException($"Fault IsNullOrEmpty command with {jpath}.");
        }
    }
}
