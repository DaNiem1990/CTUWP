using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace CTUWP
{
    class ApiCommunicator
    {
        ApiAddress apiConfig;
        string requestUri;
        string responseKey = "success";
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage response;

        public ApiCommunicator(ApiAddress apiConfig)
        {
            this.apiConfig = apiConfig;
            this.requestUri = apiConfig.ToString();
            setHeaders();
        }

        public async Task<JsonObject> post()
        {
            setAuthHeader();
            HttpMultipartFormDataContent payLoad = prepareData();
            string responseString = await postData(payLoad);
            JsonObject uData = returnDataIfSuccess(responseString);
            return uData;
        }

        public async Task<JsonObject> get()
        {
            setAuthHeader();
            string responseString = await getData();
            return returnDataIfSuccess(responseString);
        }
        
       private HttpMultipartFormDataContent prepareData()
       {
            return UserData.getPayLoad();
       }

        public async Task<string> postData(HttpMultipartFormDataContent payLoad)
        {
            response = await httpClient.PostAsync(new Uri(requestUri), payLoad);
            string responString = await response.Content.ReadAsStringAsync();
            return responString;
        }

        public async Task<string> getData()
        {
            response = await httpClient.GetAsync(new Uri(requestUri));
            string responString = await response.Content.ReadAsStringAsync();
            return responString;
        }

        public JsonObject returnDataIfSuccess(string responseString)
        {
            JsonObject dataArray = new JsonObject();

            if ((int)response.StatusCode == 200)
            {
                try
                {
                    JsonObject user = JsonObject.Parse(responseString);
                    dataArray = JsonObject.Parse(
                           user.GetNamedValue(responseKey).ToString()
                       );

                    UserData.setToken(getTokenFromJson(dataArray));
                }
                catch
                {

                }
            }
            else
            {
                throw new Exception("Podany email bądź hasło jest nieprawidłowe.");
            }
            return dataArray;
        }

        private void setHeaders()
        {
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        private void setAuthHeader()
        {
            string token = UserData.getToken();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+ token);
        }
        private string getTokenFromJson(JsonObject jResponse)
        {
            string token = "";
            try
            {
                token = jResponse.GetNamedString("token");           
            }
            catch
            {

            }
            return token;
        }

    }
}
