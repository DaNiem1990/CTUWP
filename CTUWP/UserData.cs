using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Web.Http;

namespace CTUWP
{
    public static class UserData
    {
        private static HttpMultipartFormDataContent postContents;

        private static string apiToken { get; set; } = "";
        
        private static Dictionary<string, string> payLoadFields = new Dictionary<string, string>
            {
                { "email", "" },
                { "password", ""},
                { "name", ""},
                { "c_password", ""},
                { "id", ""},
                };

        static UserData()
        {

        }

        public static void setEmailAndPassword(string email, string password)
        {
            payLoadFields["email"] = email;
            payLoadFields["password"] = password;
        }

        public static void setEmail(string email)
        {
            payLoadFields["email"] = email;
        }

        public static string getEmail()
        {
            return payLoadFields["email"];
        }

        public static void setToken(string token)
        {
            if(!String.IsNullOrEmpty(token) )
            {
                apiToken = token;
            }
            else if (!String.IsNullOrEmpty(getToken()))
            {
                clearToken();
            }
            
        }

        public static void clearToken()
        {
            apiToken = "";
        }

        public static string getField(string fieldName)
        {
            return payLoadFields[fieldName];
        }
        
        public static string getToken()
        {
            return apiToken;
        }
        
        public static void initFields(string email, string password)
        {
            setEmailAndPassword(email, password);
            setNameAndCPassword("", "");
            setId("");
        }
        
        public static void setNameAndCPassword(string userName, string cPassword)
        {
            payLoadFields["name"] = userName;
            payLoadFields["c_password"] = cPassword;
        }

        public static void setId(string id)
        {
            payLoadFields["id"] = id;
        }

        public static string getId()
        {
            return payLoadFields["id"];
        }

        public static void setFields(JsonObject dataArray)
        {
            setAllFields(
                dataArray.GetNamedString("email"),
                dataArray.GetNamedString("password"), 
                dataArray.GetNamedString("name"), 
                dataArray.GetNamedString("c_password"), 
                dataArray.GetNamedString("id")
                );
        }

        public static void setUserDetails(JsonObject dataArray)
        {
            setId(dataArray.GetNamedValue("id").ToString());
            
            setInfoFields(dataArray.GetNamedString("email"), dataArray.GetNamedString("name"));
        }

        public static void setInfoFields(string userName, string email)
        {
            payLoadFields["name"] = userName;
            payLoadFields["email"] = email;
        }

        public static void setAllFields(string email, string password, string userName, string cPassword, string id)
        {
            setEmailAndPassword(email, password);
            setNameAndCPassword(userName, cPassword);
            setId(id);
        }
        
        private static HttpStringContent prepareField(string field)
        {
            return new HttpStringContent(field);            
        }

        private static void addFieldToContent(HttpStringContent fieldValue, string fieldName)
        {
            if (!String.IsNullOrEmpty(fieldValue.ToString()))
            {
                postContents.Add(fieldValue, fieldName);
            }
        }

        public static HttpMultipartFormDataContent getPayLoad()
        {
            postContents = new HttpMultipartFormDataContent();

            foreach(KeyValuePair<string, string> field in payLoadFields)
            {
                addFieldToContent(
                    prepareField(field.Value),
                    field.Key
                );
            }
            
            return postContents;
        }

        public static string getBasicData()
        {
            return "email: " + payLoadFields["name"]
            +"\n" + "name: " + payLoadFields["email"];
        }
    }
}
