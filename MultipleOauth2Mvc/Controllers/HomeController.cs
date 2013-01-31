using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Oauth2Login;
using Oauth2Login.Client;

namespace MultipleOauth2Mvc.Controllers
{
    public class HomeController : Controller
    {

        private static Oauth2LoginContext _context;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string id = "", string bindpage = "")
        {
            string url = "";
            AbstractClientProvider client = null;
            try
            {
                switch (id.ToLower())
                {
                    case "google":
                        client = Oauth2LoginFactory.CreateClient<GoogleClinet>("Google");
                        break;
                    case "facebook":
                        client = Oauth2LoginFactory.CreateClient<FacebookClient>("Facebook");
                        break;
                    case "windowslive":
                        client = Oauth2LoginFactory.CreateClient<WindowsLiveClient>("WindowsLive");
                        break;
                    default:
                        return RedirectToAction("index");
                }
                if (client != null)
                {
                    if (bindpage.Equals("1"))
                        client.CallBackUrl += "?bindpage=1";
                    _context = Oauth2LoginContext.Create(client);
                    url = _context.BeginAuth();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Json(new { Url = url });

            #region old
            // private static MultiOAuthContext _context;
            //string url = "";
            //IClientProvider client;
            //try
            //{
            //    switch (id.ToLower())
            //    {
            //        case "google":
            //            client = MultiOAuthFactroy.CreateClient<GoogleClinet>("Google");
            //            break;
            //        case "facebook":
            //            client = MultiOAuthFactroy.CreateClient<FacebookClient>("Facebook");
            //            break;
            //        case "windowslive":
            //            client = MultiOAuthFactroy.CreateClient<WindowsLiveClient>("WindowsLive");
            //            break;
            //        default:
            //            return RedirectToAction("index");
            //    }
            //    if (client != null)
            //    {
            //        if (bindpage.Equals("1"))
            //            client.CallBackUrl += "?bindpage=1";
            //        _context = MultiOAuthContext.Create(client);
            //        url = _context.BeginAuth();
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

            //return Json(new { Url = url });
            #endregion
        }

        public ActionResult Succes(/*string email = "", */string bindpage = "")
        {
            try
            {
                var token = _context.Token;
                var result = _context.Profile;
                var strResult =_context.Client.ProfileJsonString;
                //if (email != "")
                //    result.Add("email", email);
                
                return Content("<script type=\"text/javascript\">window.opener.abc('" + JsonConvert.SerializeObject(result) + "');self.close();</script>");
            }
            catch (Exception)
            {
                throw;
                //RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }

        #region twitter
        //public ActionResult Twitter(FormCollection from)
        //{
        //    if (string.IsNullOrEmpty(from["useremail"]))
        //    {
        //        return View();
        //    }

        //    string url = "";

        //    try
        //    {

        //        var client = MultiOAuthFactroy.CreateClient<TwitterClient>("Twitter");
        //        client.CallBackUrl += "?email=" + from["useremail"];
        //        _context = MultiOAuthContext.Create(client);
        //        url = _context.BeginAuth();

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Redirect(url);
        //}
        #endregion

    }
}
