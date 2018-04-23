using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Xml;
namespace WebApplication1
{
    public partial class _Default : System.Web.UI.Page
    { 
  protected void Page_Load(object sender, EventArgs e)
        {
            string userName = "administrator";
            string password = "Password";
            string auth = "secEnterprise";
            string baseURL = "http://localhost:6405/biprws/";
            string LogonURI = baseURL + "logon/long";
            string InfoStoreURI = baseURL + "raylight/v1/documents/5385"; //5385 is the document id
            string rwsLogonToken;
            try
            {
                //Making GET Request to  /logon/long to receive XML template.
                WebRequest myWebRequest = WebRequest.Create(LogonURI);
                myWebRequest.ContentType = "application/xml";
                myWebRequest.Method = "GET";

                //Returns the response to the request made
                WebResponse myWebResponse = myWebRequest.GetResponse();
                //Creating an instance of StreamReader to read the data stream from the resource
                StreamReader sr = new StreamReader(myWebResponse.GetResponseStream());
                //Reads all the characters from the current position to the end of the stream and store it as string
                string output = sr.ReadToEnd();
                //Initialize a new instance of the XmlDocument class
                XmlDocument doc = new XmlDocument();
                //Loads the document from the specified URI
                doc.LoadXml(output);

                //Returns an XmlNodeList containing a list of all descendant elements 
                //that match the specified name i.e. attr
                XmlNodeList nodelist = doc.GetElementsByTagName("attr");
                //  Add the logon parameters to the attribute nodes of the document
                foreach (XmlNode node in nodelist)
                {
                    if (node.Attributes["name"].Value == "userName")
                        node.InnerText = userName;
                    if (node.Attributes["name"].Value == "password")
                        node.InnerText = password;
                    if (node.Attributes["name"].Value == "auth")
                        node.InnerText = auth;
                }
                //Making POST request to /logon/long to receive a logon token
                WebRequest myWebRequest1 = WebRequest.Create(LogonURI);
                myWebRequest1.ContentType = "application/xml";
                myWebRequest1.Method = "POST";
                byte[] reqBodyBytes = System.Text.Encoding.Default.GetBytes(doc.OuterXml);
                Stream reqStream = myWebRequest1.GetRequestStream();
                reqStream.Write(reqBodyBytes, 0, reqBodyBytes.Length);
                reqStream.Close();
                try
                {
                    WebResponse myWebResponse1 = myWebRequest1.GetResponse();
                    //Finding the value of the X-SAP-LogonToken
                    rwsLogonToken = myWebResponse1.Headers["X-SAP-LogonToken"].ToString();
                    //Making GET request to /infostore to retrieve the contents of top level of BI Platform repository.
                    HttpWebRequest myWebRequest2 = (HttpWebRequest)WebRequest.Create(InfoStoreURI);
                    myWebRequest2.Accept = "application/pdf";
                    myWebRequest2.Headers.Add("X-SAP-LogonToken", rwsLogonToken);
                    myWebRequest2.Method = "GET";
                    WebResponse myWebResponse2 = myWebRequest2.GetResponse();
                    FileStream stream = new FileStream(Request.PhysicalApplicationPath + "output.pdf", FileMode.Create);
                    myWebResponse2.GetResponseStream().CopyTo(stream);
                    stream.Close();
                    Response.Redirect("output.pdf");
               }
                catch (WebException ex)
                {
                    //error while accessing the network through a pluggable protocol
                    Response.Write("<b>" + ex.Message + "</b>");
                }
                catch (Exception ex)
                {
                    //generic error
                    Response.Write("<b>" + ex.Message + "</b>");
                }
            }
            catch (WebException ex)
            {
                //error while accessing the network through a pluggable protocol
                Response.Write("<b>" + ex.Message + "</b>");
            }
            catch (Exception ex)
            {
                //generic error
                Response.Write("<b>" + ex.Message + "</b>");
            }
  }
 }
}
