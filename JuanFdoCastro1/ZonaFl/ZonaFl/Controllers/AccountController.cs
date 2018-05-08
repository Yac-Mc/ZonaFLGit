using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using ZonaFl.Models;
using ZonaFl.Providers;
using ZonaFl.Results;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http.Results;
using Omu.ValueInjecter;
using ZonaFl.Persistence.Entities;
using ZonaFl.Services;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.Owin.Testing;
using System.Web.Script.Serialization;
using ZonaFl.Business.SubSystems;
using ZonaFl.Controllers.Filters;

namespace ZonaFl.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : BaseApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
       
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize(Roles = "Admin")]
        [Route("users")]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        [AllowAnonymous]
        [Route("CheckUser")]
        [HttpPost]
        public async Task<IHttpActionResult>  CheckUser(string username)
        {
            IdentityUser user = await UserManager.FindByNameAsync(username);

            if (user == null)
            {
                return Json(new { success = true, issue = user, errors = "", tipo = user, UserEmail = "" });
            }
            else
            {
               string mensaje= "<font color='red'>El nombre de usuario <STRONG>" + user.UserName + "</STRONG> esta siendo usado.</font>";
                return Json(new { success = true, issue = user, errors = mensaje, tipo = user, UserEmail = "" });

            }
           
        }

        
        [AllowAnonymous]
        [Route("RecoverPassword")]
        [HttpPost]
        public async Task<IHttpActionResult> RecoverPassword(string email)
        {
            // EmailService email = new EmailService();
            IdentityMessage im = new IdentityMessage();

            SUser suser = new SUser();
           var user= suser.GetUserByEmail(email);
            
            //string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            string code = this.AppUserManager.GenerateUserToken(DateTime.Today.ToShortDateString(), user.Id);
            //var callbackUrl = new Uri(Url.Link("ChangePassword", new { email = email }));
            string host = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            var callbackUrl = host+"/Home/index?code="+ code +"#ChangePassword";
            //im.Body = "Por favor confirme su cuenta haciendo Click <a href =\"" + callbackUrl + "\">Aquí</a>";
            //im.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to comlete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", user.UserName, new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code })));
            string body = user.UserName + ","+ @"<br /><br />Porfavor haga click en el siguiente link para resuperar la contraseña"+@"<a href = '" + callbackUrl + "'> Recuperar contraseña </a>"+"<br /><br /> si usted no a solicitado cambio de contraseña por favor haga caso omiso de este mensaje.Gracias por ponerse en contacto con Zona FL";
           
            im.Body = body;
            

            var smail = SMail.Instance;
            try
            {
                MailMessage mail = new MailMessage("contact@zonafl.com", email, "Recuperar Password", body);
                mail.IsBodyHtml = true;
                smail.Send(mail);
            }
            catch(Exception  er)
            {
                ExceptionResult ere = InternalServerError(er);
                return Json(new { success = false, issue = email, errors = ere.Exception.Message, tipo = email, UserEmail = "" });

            }

            return Json(new { success = true, issue = email, errors = "", tipo = email, UserEmail = email, Url = Url.Content("/Freelance/Logout").ToString() });


        }


        [AllowAnonymous]
        [Route("CheckEmail")]
        [HttpPost]
        public async Task<IHttpActionResult> CheckEmail(string email)
        {
            
           
            IdentityUser user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(new { success = true, issue = user, errors = "", tipo = user, UserEmail = "", noskills = "" });
            }
            else
            {

                SUser suser = new SUser();
                var userskills = suser.GetUserByEmail(email);
                string noskills = userskills.Skills.Count().ToString();
                string mensaje = "<font color='red'>El email de usuario <STRONG>" + user.Email + "</STRONG> esta siendo usado, favor ingresar con la cuenta existente.</font>";
                return Json(new { success = true, issue = user, errors = mensaje, tipo = user, UserEmail = "" , noskills = noskills });

            }

        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [AllowAnonymous]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/ChangePassword
        [AllowAnonymous]
        [Route("ChangePasswordEmail")]
        public async Task<IHttpActionResult> ChangePasswordEmail(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SUser suser = new SUser();
         var user=   suser.GetUserByEmail(model.Email);
            
            if (AppUserManager.VerifyUserToken(user.Id, DateTime.Today.ToShortDateString(), model.Token.Replace("#ChangePassword", "")))
            {
             string settok=  UserManager.GeneratePasswordResetToken(user.Id);

                IdentityResult result = UserManager.ResetPassword(user.Id, settok, model.NewPassword);
                   
                return Json(new { success = true, issue = model.Email, errors = "", tipo = model.Email, UserEmail = model.Email, Url = Url.Content("/Freelance/Logout").ToString() });
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            else
            { 
                    return Json(new { success = false, issue = model.Email, errors = "", tipo = model.Email, UserEmail = model.Email, Url = Url.Content("/Freelance/Logout").ToString() });





            }

           
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

       

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "iD del usuario y codigo son obligatorios");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Redirect(Url.Content("~/Home/index#iniciar"));
               // return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {

            string url = "/Static/Register.html?UserEmail=" + model.Email + "&freelance=" + model.Freelance;
            if (model.PasswordHash == "")
            {
                RegisterExternalBindingModel rebm = new RegisterExternalBindingModel();
                rebm.Email = model.Email;
                rebm.UserName = model.UserName;
                rebm.Empresa = model.Empresa;
                rebm.Freelance = model.Freelance;
                rebm.EmailConfirmed=  true;
                rebm.FirstMiddleName = model.FirstMiddleName;
                 await this.RegisterExternal(rebm);
            }
            else
            {


                if (!ModelState.IsValid)
                {

                    string messages = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));


                    return Json(new { success = false, issue = model, errors = messages });
                }



                var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, Empresa = model.Empresa, Freelance = model.Freelance, FirstMiddleName = model.FirstMiddleName, DateCreate = DateTime.Now };
                //if(!user.Freelance)
                //    url= "Register.html?UserEmail=" + model.Email;
                try
                {
                    IdentityResult result = await UserManager.CreateAsync(user, model.PasswordHash);

                    if (!result.Succeeded)
                    {

                        IHttpActionResult resultF = GetErrorResult(result);
                        return Json(new { success = false, issue = user, errors = "Favor verificar el usuario o el correo de usuario", tipo = user, UserEmail = "" });


                    }
                    else
                    {
                        Business.Log4NetLogger logger2 = new Business.Log4NetLogger();

                        logger2.Info("Registro Usuario:" + user.Id + "," + "FechaCreación:" + user.DateCreate + ",Email:" + user.Email);


                        // EmailService email = new EmailService();
                        IdentityMessage im = new IdentityMessage();
                        string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

                        //im.Body = "Por favor confirme su cuenta haciendo Click <a href =\"" + callbackUrl + "\">Aquí</a>";
                        //im.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to comlete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", user.UserName, new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code })));
                        string body = "Hola " + user.UserName + ",";
                        body += "<br /><br />Porfavor haga click en el siguiente link para activar su cuenta";
                        body += "<br /><a href = '" + callbackUrl + "'>Click aqui para activar esta cuenta.</a>";
                        body += "<br /><br />Gracias";
                        im.Body = body;

                        var smail = SMail.Instance;
                        MailMessage mail = new MailMessage("contact@zonafl.com", user.Email, "Activación Cuenta", im.Body);
                        mail.IsBodyHtml = true;
                        smail.Send(mail);


                      




                    }

                }
                catch (Exception er)
                {

                    ExceptionResult ere = InternalServerError(er);
                    return Json(new { success = false, issue = user, errors = ere.Exception.Message, tipo = user, UserEmail = "", Url = url });


                }
            }

           

         


            return Json(new { success = true, issue = model, errors = "", tipo= model, UserEmail= model.Email, Url=url });

            
        }


        //// POST api/User/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Login")]
        //public async Task<IHttpActionResult> Login(RegisterBindingModel model)
        //{
        //    if (model == null)
        //    {
        //        return this.BadRequest("Invalid user data");
        //    }

        //    // Invoke the "token" OWIN service to perform the login (POST /api/token)
        //    // Use Microsoft.Owin.Testing.TestServer to perform in-memory HTTP POST request
        //    var testServer = TestServer.Create<Startup>();
        //    var requestParams = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("grant_type", "password"),
        //        new KeyValuePair<string, string>("username", model.UserName),
        //        new KeyValuePair<string, string>("password", model.Password)
        //    };
        //    var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
        //    var tokenServiceResponse = await testServer.HttpClient.PostAsync(
        //        Startup.TokenEndpointPath, requestParamsFormUrlEncoded);

        //    if (tokenServiceResponse.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        // Sucessful login --> create user session in the database
        //        var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
        //        var jsSerializer = new JavaScriptSerializer();
        //        var responseData =
        //            jsSerializer.Deserialize<Dictionary<string, string>>(responseString);
        //        var authToken = responseData["access_token"];
        //        var username = responseData["username"];
        //        var userSessionManager = new UserSessionManager();
        //        userSessionManager.CreateUserSession(username, authToken);

        //        // Cleanup: delete expired sessions fromthe database
        //        userSessionManager.DeleteExpiredSessions();
        //    }

        //    return this.ResponseMessage(tokenServiceResponse);
        //}



        [AllowAnonymous]
        [Route("LoginUser")]
        [HttpPost]
        public async Task<IHttpActionResult> LoginUser(RegisterBindingModel model)
        {


            var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, Empresa = model.Empresa, Freelance = model.Freelance };


            AspNetUsers aspuser = new AspNetUsers();
            try
            {


                ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
                ApplicationUser useru = null;
                if (user.UserName.Contains("@"))
                {
                    useru = UserManager.FindByEmail(user.UserName);
                }
                else
                {

                    useru = UserManager.FindByName(user.UserName);
                }


                if (UserManager.IsEmailConfirmed(useru.Id))
                {

                    bool validuser = false;
                    if (useru != null)
                    {
                        validuser = UserManager.CheckPassword(useru, model.PasswordHash);
                        if (validuser)
                        {
                            var identity = new System.Security.Principal.GenericIdentity(useru.UserName);
                            SetPrincipal(new System.Security.Principal.GenericPrincipal(identity, null));
                        }
                        else
                        {

                            return Json(new { success = false, issue = user, errors = "Contraseña o usuario incorrecto, favor volver a ingresar los datos", tipo = user, UserEmail = aspuser.Email });
                        }

                        var useri = HttpContext.Current.User;
                        HttpContext.Current.User = useri;

                        //RegisterBindingModel regbm = new RegisterBindingModel();
                        //regbm.InjectFrom(useru);
                        //HttpSessionStateBase session = new HttpSessionStateWrapper(HttpContext.Current.Session);
                        //SessionBag.Current.User = regbm;

                    }

                    if (useru.Freelance)
                    {
                        return Json(new { success = validuser, issue = useru, errors = "", tipo = useru, UserEmail = useru.Email, Url = Url.Content("/Offer/Index/" + useru.Id).ToString() });

                    }
                    else if (useru.Empresa)
                    {
                        return Json(new { success = validuser, issue = useru, errors = "", tipo = useru, UserEmail = useru.Email, Url = Url.Content("/Projects/Index/" + useru.Id).ToString() });
                    }
                    else
                    {

                        return Json(new { success = validuser, issue = useru, errors = "", tipo = useru, UserEmail = useru.Email, Url = Url.Content("/Administration/Users/Index?email=" + useru.Email).ToString() });
                    }
                }
                else
                {
                    return Json(new { success = false, issue = user, errors = "Usuario con correo electrónico no confirmado, favor revisar su correo electronico para confirmar la creación de la cuenta", tipo = user, UserEmail = aspuser.Email });
                }
            }

            catch (Exception ex)
            {
                return Json(new { success = false, issue = user, errors ="No es posible autenticarse con el usuario digitado, favor verificar usuario y contraseña", tipo = user, UserEmail = aspuser.Email });

            }

            
            return Json(new { success = true, issue = model, errors = "", tipo = model, UserEmail = user.Email, Url = Url.Content("/Freelance/Logout").ToString() });
        }



        [AllowAnonymous]
        [Route("LoginUser")]
        [HttpPost]
        [HttpGet]
        public async Task<IHttpActionResult> LoginUser(string UserName, string Email, string PasswordHash, bool Empresa, bool Freelance)
        {
            //string UserName = "juanfercas2002@gmail.com";
            //string Email = "juanfercas2002@gmail.com";
            //string  PasswordHash = "j7948810";
            //bool Empresa = true;
            //bool Freelance = false;

            var user = new ApplicationUser() { UserName = UserName, Email = Email, Empresa = Empresa, Freelance = Freelance };


            AspNetUsers aspuser = new AspNetUsers();
            try
            {


                ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
                ApplicationUser useru = null;
                if (user.UserName.Contains("@"))
                {
                    useru = UserManager.FindByEmail(user.UserName);
                }
                else
                {

                    useru = UserManager.FindByName(user.UserName);
                }

                if (useru != null)
                {
                    if (UserManager.IsEmailConfirmed(useru.Id) )
                    {

                        bool validuser = false;
                        if (useru != null)
                        {
                            validuser = UserManager.CheckPassword(useru, PasswordHash);
                            if (validuser)
                            {
                                var identity = new System.Security.Principal.GenericIdentity(useru.UserName);
                                SetPrincipal(new System.Security.Principal.GenericPrincipal(identity, null));
                            }
                            else
                            {

                                return Json(new { success = false, issue = user, errors = "Contraseña o usuario incorrecto, favor volver a ingresar los datos", tipo = user, UserEmail = aspuser.Email });
                            }

                            var useri = HttpContext.Current.User;
                            HttpContext.Current.User = useri;

                            //RegisterBindingModel regbm = new RegisterBindingModel();
                            //regbm.InjectFrom(useru);
                            //HttpSessionStateBase session = new HttpSessionStateWrapper(HttpContext.Current.Session);
                            //SessionBag.Current.User = regbm;

                        }

                        if (useru.Freelance)
                        {
                            return Json(new { success = validuser, issue = useru, errors = "", tipo = useru, UserEmail = useru.Email, Url = Url.Content("/Offer/Index/" + useru.Id).ToString() });

                        }
                        else if (useru.Empresa)
                        {
                            return Json(new { success = validuser, issue = useru, errors = "", tipo = useru, UserEmail = useru.Email, Url = Url.Content("/Projects/Index/" + useru.Id).ToString() });
                        }
                        else
                        {

                            return Json(new { success = validuser, issue = useru, errors = "", tipo = useru, UserEmail = useru.Email, Url = Url.Content("/Administration/Users/Index?email=" + useru.Email).ToString() });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, issue = user, errors = "Usuario con correo electrónico no confirmado, favor revisar su correo electronico para confirmar la creación de la cuenta", tipo = user, UserEmail = aspuser.Email });
                    }
                }
                else
                {
                    return Json(new { success = false, issue = user, errors = "Usuario o password incorecto, favor digitar correctamente sus credenciales", tipo = user, UserEmail = aspuser.Email });
                }
            }

            catch (Exception ex)
            {
                return Json(new { success = false, issue = user, errors = ex.Message, tipo = user, UserEmail = aspuser.Email,Trace=ex.StackTrace });

            }


            return Json(new { success = true, issue = "Registro de usuario", errors = "", tipo = "Registro de usuario", UserEmail = user.Email, Url = Url.Content("/Freelance/Logout").ToString() });
        }



        private static void SetPrincipal(System.Security.Principal.IPrincipal principal)
        {
            System.Threading.Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }





        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            string url = "/Static/Register.html?UserEmail=" + model.Email + "&freelance=" + model.Freelance;
            //var info = await Authentication.GetExternalLoginInfoAsync();
            //if (info == null)
            //{
            //    return InternalServerError();
            //}

            var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, EmailConfirmed=model.EmailConfirmed, Freelance= model.Freelance,Empresa=model.Empresa, FirstMiddleName=model.FirstMiddleName };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            //result = await UserManager.AddLoginAsync(user.Id, info.Login);
            //if (!result.Succeeded)
            //{
            //    return GetErrorResult(result); 
            //}
            //return Ok();

            return Json(new { success = true, issue = model, errors = "", tipo = model, UserEmail = model.Email, Url = url });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await this.UserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await UserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.UserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.UserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("InsertSkills")]
        [HttpPost]
        public async Task<IHttpActionResult> InsertSkills(List<RegisterBindingModel> userl)
        {
           
         
            //if (!ModelState.IsValid)
            //{
            //string messages = string.Join("; ", ModelState.Values
            //                        .SelectMany(x => x.Errors)
            //                        .Select(x => x.ErrorMessage));


            //return Json(new { success = false, issue = "", errors = messages });


            //}
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            ZonaFl.Business.SubSystems.SCategory usersk = new Business.SubSystems.SCategory();
            ZonaFl.Business.SubSystems.SSkill sskill = new Business.SubSystems.SSkill();
            RegisterBindingModel user = userl[0];

            if(user.Empresa==null)
            {
                user.Empresa = false;
            }

            if (user.Freelance == null)
            {
                user.Freelance = false;
            }

            RegisterBindingModel rmb = new RegisterBindingModel();
            rmb.Skills = user.Skills;
            rmb.Company = user.Company;
            Persistence.Entities.Company company = new Persistence.Entities.Company();
            if(user.Company!=null)
            company.InjectFrom(user.Company.FirstOrDefault());
            List<Persistence.Entities.Skill> skills = rmb.Skills.Select(e => new Persistence.Entities.Skill().InjectFrom(e)).Cast<Persistence.Entities.Skill>().ToList();
            AspNetUsers aspuser = new AspNetUsers();
           
                 var useru = UserManager.FindByEmail(user.Email);

          
            for (int i= 0;i< skills.Count();i++)        
            {
                ZonaFl.Persistence.Entities.Category category = null;
              var skill=  sskill.FindSkillByName(user.Skills[i].Name);
                string[] stringSeparators = new string[] { "\n" };
               string result= user.Skills[i].CategorySkill.Split(stringSeparators, StringSplitOptions.None)[0];
               
                category = usersk.FindCategoryByName(result);

                if(category==null)
                {
                    try
                    {
                        category = usersk.InsertCategory(result);
                    }
                    catch (Exception er)
                    {
                        return Json(new { success = false, issue = user, errors = er.Message, tipo = user, UserEmail = useru.Email });
                    }
                }

                skills[i].IdHtml = user.Skills[i].IdHtml;
                skills[i].Category = category;
                skills[i].CategoryId = category.Id;
            }

            useru.City = user.City;
            useru.Country = user.Country;
            useru.UserName = user.UserName;
            useru.DescUser = user.DescUser;
            useru.PhoneNumber = user.PhoneNumber.ToString();
            useru.FirstMiddleName = user.FirstMiddleName;
            useru.Image = user.Image;
            useru.Empresa = user.Empresa;

            aspuser.InjectFrom(useru);
            aspuser.NoTrabajadores = user.NumeroEmp;
            
            try
            {
                Business.Log4NetLogger logger2 = new Business.Log4NetLogger();
                if (!useru.Empresa)
                {
                    if (skills.Count > 0)
                    {
                        skills = usern.InsertSkillsByUser(skills, aspuser);
                        logger2.Info("Inserción Usuario Categoria:" + useru.Id + "," + "UsuarioCategoria:" + skills.FirstOrDefault().CategoryId + ",Email:" + aspuser.Email);
                    }
                }


            }
            catch (Exception er)
            {
                return Json(new { success = false, issue = user, errors = er.Message, tipo = aspuser, UserEmail = useru.Email });
            }
            
           try
            {
                SUser suser = new SUser();
                aspuser.Empresa = useru.Empresa;
                aspuser.Freelance = useru.Freelance;
                  IdentityResult result = await UserManager.UpdateAsync(useru);
               
               var userupdate= suser.UpdateUser(aspuser);
                if (useru.Empresa)
                {
                   
                    company.IdUser = useru.Id;
                    company.Name = userl[0].Company.Last().Name;
                    company.Mail = userl[0].Company.Last().Mail;
                    company.Site = user.UrlEmpresa;
                    company.Sector = userl[0].Company.Last().Sector;
                    company.RazonSocial = userl[0].Company.Last().RazonSocial;
                    company.Telefono = userl[0].Company.Last().Telefono;
                    company.Nit = userl[0].Company.Last().Nit;
                    company.NumeroEmp = aspuser.NoTrabajadores == null ? 0 : (int)aspuser.NoTrabajadores;
                    company.Description = userl[0].Company.Last().Description;

                    Persistence.Entities.Company companyp = usern.InsertCompany(company);
                }

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            catch (Exception er)
            {
                return Json(new { success = false, issue = user, errors = er.Message, tipo = user, UserEmail = useru.Email });
                //return InternalServerError(er);

            }
            aspuser.Empresa = useru.Empresa;
            aspuser.Freelance = useru.Freelance;
            aspuser.Id = useru.Id;
            return Json(new { success = true, issue = user, errors = "", tipo = aspuser, UserEmail = useru.Email });
            //return Ok();
        }
        [AllowAnonymous]
        [Route("UploadImageUser")]
        [HttpPost]
        public KeyValuePair<bool, string> UploadImageUser()
        {
            try
            {
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    // Get the uploaded image from the Files collection
                    var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];

                    if (httpPostedFile != null)
                    {
                        // Validate the uploaded image(optional)

                        // Get the complete file path
                        var fileSavePath = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), httpPostedFile.FileName);

                        // Save the uploaded file to "UploadedFiles" folder
                        httpPostedFile.SaveAs(fileSavePath);
                        //AspNetUsers aspuser = new AspNetUsers();
                        //var useru = UserManager.FindByEmail(user.Email);

                        return new KeyValuePair<bool, string>(true, "Arhivo descargado correctamente.");
                    }

                    return new KeyValuePair<bool, string>(true, "No se pudo descargar el archivo correctamente.");
                }

                return new KeyValuePair<bool, string>(true, "No se ha encontrado archivo ha descargar.");
            }
            catch (Exception ex)
            {
                return new KeyValuePair<bool, string>(false, "An error occurred while uploading the file. Error Message: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("UpdateImageUser")]
        [HttpPost]
        [CustomExceptionFilter]
        public async Task<IHttpActionResult> UpdateImageUser(List<RegisterBindingModel> userl)
        {
            RegisterBindingModel user = userl[0];
            AspNetUsers aspuser = new AspNetUsers();
            try
            {
                ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
              
               
                var useru = UserManager.FindByEmail(user.Email);
                useru.City = user.City;
                useru.Country = user.Country;
                useru.DescUser = user.DescUser;
                useru.PhoneNumber = user.PhoneNumber!=null?user.PhoneNumber.ToString():"";
                useru.FirstMiddleName = user.FirstMiddleName!=null? user.FirstMiddleName:"";
                useru.Image = user.Image != null ? user.Image : ""; 
                aspuser.InjectFrom(useru);
                IdentityResult result = await UserManager.UpdateAsync(useru);
                //aspuser =  usern.UpdateUser(aspuser);
                return Json(new { success = true, issue = user, errors = "", tipo = user, UserEmail = useru.Email });
                //return new KeyValuePair<bool, string>(true, "se ha guardado la información correctamente");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, issue = user, errors = ex.Message, tipo = user, UserEmail = aspuser.Email });
                //return new KeyValuePair<bool, string>(false, "Ha ocurrido un error al actualizar la imagen del usuario. Error Message: " + ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("GetuserNameByEmail")]
        [HttpPost]
        public async Task<IHttpActionResult> GetuserNameByEmail(List<RegisterBindingModel> userl)
        {
            RegisterBindingModel user = userl[0];
            AspNetUsers aspuser = new AspNetUsers();
            try
            {
                ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();


                var useru = UserManager.FindByEmail(user.Email);
               
                
                //aspuser =  usern.UpdateUser(aspuser);
                return Json(new { success = true, issue = user, errors = "", tipo = useru, UserEmail = useru.Email });
                //return new KeyValuePair<bool, string>(true, "se ha guardado la información correctamente");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, issue = user, errors = ex.Message, tipo = user, UserEmail = aspuser.Email });
                //return new KeyValuePair<bool, string>(false, "Ha ocurrido un error al actualizar la imagen del usuario. Error Message: " + ex.Message);
            }
        }

        #endregion
    }


}
