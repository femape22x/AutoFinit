using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AutoFinit.Resources
{
    public class RegexHelper
    {
        public List<String> getValues(String cadena, String pattern)
        {
            List<String> estados = new List<String>();
            MatchCollection match = Regex.Matches(cadena, pattern);
            foreach (Match grupo in match)
            {
                foreach (Capture cap in grupo.Groups["valores"].Captures)
                {
                    estados.Add(Regex.Replace(cap.Value, @",?", ""));
                }
            }
            return estados;
        }
    }
}