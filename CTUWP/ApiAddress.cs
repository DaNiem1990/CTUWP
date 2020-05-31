using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTUWP
{
    public class ApiAddress
    {
        //fqdn - full qualified domain name
        private string fqdn;
        private string endpoint;
        private string argument;

        public ApiAddress(string fqdn, string endpoint, string argument = "")
        {
            this.fqdn = fqdn;
            this.endpoint = endpoint;
            this.argument = argument;
        }

        public string generate()
        {
            return fqdn
                +endpoint
                + (!String.IsNullOrEmpty(argument) 
                    ? "?"
                    : "")
                + argument;
        }

        public override string ToString()
        {
            return generate();
        }
    }
}
