using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAPIClient
{
    public class APIClient
    {

        HttpClient vClient = null;

        public APIClient(string endpoint)
        {
            vClient = new HttpClient();
            vClient.BaseAddress = new System.Uri(endpoint);
            vClient.DefaultRequestHeaders.Accept.Clear();
            vClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public QueryResult executeQuery(string req, string token)
        {
            QueryResult ret = null;
            try
            {
                var requestUri = new Uri(vClient.BaseAddress, "Query/execute");
                using (var requestWriter = new System.IO.StringWriter())
                {
                    requestWriter.Write(req);
                    var content = new StringContent(requestWriter.ToString(), System.Text.Encoding.UTF8, "application/json");
                    vClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var responseMessage = this.vClient.PostAsync(requestUri, content).Result;
                    responseMessage.EnsureSuccessStatusCode();
                    var stream = responseMessage.Content.ReadAsStreamAsync().Result;
                    using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
                    {
                        var serializer = new JsonSerializer();
                        ret = serializer.Deserialize<QueryResult>(jsonReader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        public AuthToken getAuthToken(AuthRequest req)
        {
            AuthToken ret = null;
            try
            {
                var requestUri = new Uri(vClient.BaseAddress, "Users/getAuthToken");
                using (var requestWriter = new System.IO.StringWriter())
                {
                    var requestSerializer = JsonSerializer.Create();
                    requestSerializer.Serialize(requestWriter, req);
                    var content = new StringContent(requestWriter.ToString(), System.Text.Encoding.UTF8, "application/json");
                    var responseMessage = this.vClient.PostAsync(requestUri, content).Result;
                    responseMessage.EnsureSuccessStatusCode();
                    var stream = responseMessage.Content.ReadAsStreamAsync().Result;
                    using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
                    {
                        var serializer = new JsonSerializer();
                        ret = serializer.Deserialize<AuthToken>(jsonReader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        public StreamContent getCreatives(string token, string medio, string codigo)
        {
            StreamContent ret = null;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(vClient.BaseAddress);
                builder.Append("creatives/getFile?");
                builder.Append("access_token=");
                builder.Append(token);
                builder.Append("&med=");
                builder.Append(System.Web.HttpUtility.UrlEncode(medio));
                builder.Append("&cod=");
                builder.Append(codigo);
                var responseAPI = vClient.GetAsync(builder.ToString()).Result;
                if (responseAPI.IsSuccessStatusCode)
                {
                    ret = (StreamContent)responseAPI.Content;
                }
                else
                {
                    throw new Exception("Error al recuperar la creatividad (" + responseAPI.StatusCode.ToString() + ")");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        public VariableFiltro getFilterValues(Filter_Request req, string token)
        {
            VariableFiltro ret = null;
            try
            {
                var requestUri = new Uri(vClient.BaseAddress, "Query/getFilterValues");
                using (var requestWriter = new System.IO.StringWriter())
                {
                    var requestSerializer = JsonSerializer.Create();
                    requestSerializer.Serialize(requestWriter, req);
                    var content = new StringContent(requestWriter.ToString(), System.Text.Encoding.UTF8, "application/json");
                    vClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var responseMessage = this.vClient.PostAsync(requestUri, content).Result;
                    responseMessage.EnsureSuccessStatusCode();
                    var stream = responseMessage.Content.ReadAsStreamAsync().Result;
                    using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
                    {
                        var serializer = new JsonSerializer();
                        ret = serializer.Deserialize<VariableFiltro>(jsonReader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        public List<Variable> getVariables(string token)
        {
            List<Variable> ret = null;
            try
            {
                var requestUri = new Uri(vClient.BaseAddress, "Query/getVariables");
                using (var requestWriter = new System.IO.StringWriter())
                {
                    vClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var responseMessage = this.vClient.PostAsync(requestUri, null).Result;
                    responseMessage.EnsureSuccessStatusCode();
                    var stream = responseMessage.Content.ReadAsStreamAsync().Result;
                    using (JsonReader jsonReader = new JsonTextReader(new System.IO.StreamReader(stream)))
                    {
                        var serializer = new JsonSerializer();
                        ret = serializer.Deserialize<List<Variable>>(jsonReader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }


    }
}
