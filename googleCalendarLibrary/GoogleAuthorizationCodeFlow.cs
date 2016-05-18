using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Json;
namespace googleCalendarLibrary
{

    public class GoogleAuthorizationCodeFlow : Google.Apis.Auth.OAuth2.Flows.AuthorizationCodeFlow
    {
        private readonly string revokeTokenUrl;

        /// <summary>Gets the token revocation URL.</summary>
        public string RevokeTokenUrl
        {
            get
            {
                return revokeTokenUrl;
            }
        }

        /// <summary>Constructs a new Google authorization code flow.</summary>
        public GoogleAuthorizationCodeFlow(Initializer initializer)
            : base(initializer)
        {
            revokeTokenUrl = initializer.RevokeTokenUrl;
        }

        public override AuthorizationCodeRequestUrl CreateAuthorizationCodeRequest(string redirectUri)
        {
            return new GoogleAuthorizationCodeRequestUrl(new Uri(AuthorizationServerUrl))
            {
                ClientId = ClientSecrets.ClientId,
                Scope = string.Join(" ", Scopes),
                RedirectUri = redirectUri
            };
        }

        public override async Task RevokeTokenAsync(string userId, string token,
            CancellationToken taskCancellationToken)
        {
            GoogleRevokeTokenRequest request = new GoogleRevokeTokenRequest(new Uri(RevokeTokenUrl))
            {
                Token = token
            };
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, request.Build());

            var response = await HttpClient.SendAsync(httpRequest, taskCancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var error = NewtonsoftJsonSerializer.Instance.Deserialize<TokenErrorResponse>(content);
                throw new TokenResponseException(error);
            }

            await DeleteTokenAsync(userId, taskCancellationToken);
        }

        /// <summary>An initializer class for Google authorization code flow. </summary>
        public new class Initializer : Google.Apis.Auth.OAuth2.Flows.AuthorizationCodeFlow.Initializer
        {
            /// <summary>Gets or sets the token revocation URL.</summary>
            public string RevokeTokenUrl
            {
                get;
                set;
            }

            /// <summary>
            /// Constructs a new initializer. Sets Authorization server URL to 
            /// <see cref="Google.Apis.Auth.OAuth2.GoogleAuthConsts.AuthorizationUrl"/>, and Token server URL to 
            /// <see cref="Google.Apis.Auth.OAuth2.GoogleAuthConsts.TokenUrl"/>.
            /// </summary>
            public Initializer()
                : base(GoogleAuthConsts.AuthorizationUrl, GoogleAuthConsts.TokenUrl)
            {
                RevokeTokenUrl = GoogleAuthConsts.RevokeTokenUrl;
            }
        }
    }
}

