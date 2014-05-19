﻿// <copyright file="AddCookieCommandHandler.cs" company="Salesforce.com">
//
// Copyright (c) 2014 Salesforce.com, Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the
// following conditions are met:
//
//    Redistributions of source code must retain the above copyright notice, this list of conditions and the following
//    disclaimer.
//
//    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
//    following disclaimer in the documentation and/or other materials provided with the distribution.
//
//    Neither the name of Salesforce.com nor the names of its contributors may be used to endorse or promote products
//    derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
// USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;

namespace WindowsPhoneDriverBrowser.CommandHandlers
{
    /// <summary>
    /// Provides handling for the get all cookies command.
    /// </summary>
    internal class AddCookieCommandHandler : CommandHandler
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="environment">The <see cref="CommandEnvironment"/> to use in executing the command.</param>
        /// <param name="parameters">The <see cref="Dictionary{string, object}"/> containing the command parameters.</param>
        /// <returns>The JSON serialized string representing the command response.</returns>
        public override Response Execute(CommandEnvironment environment, Dictionary<string, object> parameters)
        {
            object cookieDescriptor;
            if (!parameters.TryGetValue("cookie", out cookieDescriptor))
            {
                return Response.CreateMissingParametersResponse("cookie");
            }

            Dictionary<string, object> cookie = cookieDescriptor as Dictionary<string, object>;
            if (cookie != null)
            {
                StringBuilder cookieBuilder = new StringBuilder();
                cookieBuilder.AppendFormat("{0}={1}; ", cookie["name"], cookie["value"]);

                if (cookie.ContainsKey("secure"))
                {
                    bool isSecure = Convert.ToBoolean(cookie["secure"]);
                    if (isSecure)
                    {
                        cookieBuilder.Append("secure; ");
                    }
                }

                if (cookie.ContainsKey("expiry"))
                {
                    double expirationOffset = Convert.ToDouble(cookie["expiry"]);
                    DateTime expires = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expirationOffset);
                    cookieBuilder.AppendFormat("expires={0:ddd, d MMM yyyy HH:mm:ss} GMT; ", expires);
                }

                if (cookie.ContainsKey("path"))
                {
                    cookieBuilder.AppendFormat("path={0}; ", cookie["path"].ToString());
                }

                if (cookie.ContainsKey("domain"))
                {
                    cookieBuilder.AppendFormat("domain={0}; ", cookie["domain"].ToString());
                }

                string result = this.EvaluateAtom(environment, "function() { document.cookie = '" + cookieBuilder.ToString() + "'; }");
            }

            return Response.CreateSuccessResponse();
        }
    }
}
