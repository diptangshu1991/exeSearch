# exeSearch

using System.Net;
using System.Web;
using System.Xml;
using System.IO;
XmlDocument login = new XmlDocument();
login.LoadXml(“<attrs xmlns=’http://www.sap.com/rws/bip/‘><attr name=’cms’ type=’string’>localhost</attr><attr name=’userName’ type=’string’>”+userName+“</attr><attr name=’password’ type=’string’>”+pw+“</attr><attr name=’auth’ type=’string’ possibilities=’secEnterprise,secLDAP,secWinAD’>secEnterprise</attr></attrs>”;);
byte[] dataByte = Encoding.Default.GetBytes(login.OuterXml);
HttpWebRequest POSTRequest = (HttpWebRequest)WebRequest.Create(server);
POSTRequest.Method = “POST”;
POSTRequest.ContentType = “application/xml”;
POSTRequest.Timeout = 5000;
POSTRequest.KeepAlive = false;
POSTRequest.ContentLength = dataByte.Length;Stream POSTstream = POSTRequest.GetRequestStream();
POSTstream.Write(dataByte, 0, dataByte.Length);
Server Response

HttpWebResponse POSTResponse = (HttpWebResponse)POSTRequest.GetResponse();
StreamReader reader = new StreamReader(POSTResponse.GetResponseStream(), Encoding.UTF8);
loginToken = reader.ReadToEnd().ToString();
string loginToken2 = loginToken.Replace(“&amp;”, “&”); using (XmlReader xreader = XmlReader.Create(new StringReader(loginToken)))
{
       xreader.ReadToFollowing(“attr”);
ltoken = “\”” + xreader.ReadElementContentAsString() + “\””;
}
