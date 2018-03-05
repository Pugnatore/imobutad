using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Projeto_Lab4.Models;
using System.Net.Mime;
using System.Net;
using System.Net.Mail;

namespace Projeto_Lab4.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {


            Inquilino inq = default(Inquilino);
            Proprietario prop = default(Proprietario);
            BloqueiaInqulino bi = new BloqueiaInqulino();
            BloqueiaProprietario bp = new BloqueiaProprietario();
            var db = new DBImobiliariaDataContext();

            inq = db.Inquilinos.Where(X => X.Email == model.Email).FirstOrDefault();
            prop = db.Proprietarios.Where(X => X.Email == model.Email).FirstOrDefault();
            if (inq != null)
            {
                if (inq.Estado == 3)
                {
                    TempData["pendente"] = "A sua conta ainda não foi confirmada por favor verifique o seu e-mail!!!";

                }
                if (inq.Estado != 2)
                {

                    if (!ModelState.IsValid)
                    {
                        return View(model);
                    }
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, change to shouldLockout: true
                    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            return RedirectToLocal2(returnUrl, model);
                        case SignInStatus.LockedOut:
                            return View("Lockout");
                        case SignInStatus.RequiresVerification:
                            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "Erro a fazer login.");


                            return View(model);
                    }
                }
                else
                {

                    bi = db.BloqueiaInqulinos.Where(x => x.ID_Inquilino == inq.ID_Inquilino).FirstOrDefault();
                    return View("Bloqueado", bi);
                }
            }
            if (prop != null)
            {
                if (prop.Estado == 3)
                {
                    TempData["pendente"] = "A sua conta ainda não foi confirmada por favor verifique o seu e-mail!!!";

                }
                if (prop.Estado != 2)
                {

                    if (!ModelState.IsValid)
                    {
                        return View(model);
                    }
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, change to shouldLockout: true
                    var result1 = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                    switch (result1)
                    {
                        case SignInStatus.Success:
                            return RedirectToLocal2(returnUrl, model);
                        case SignInStatus.LockedOut:
                            return View("Lockout");
                        case SignInStatus.RequiresVerification:
                            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "Erro a fazer login.");


                            return View(model);
                    }
                }
                else
                {

                    bp = db.BloqueiaProprietarios.Where(x => x.ID_Proprietario == prop.ID_Proprietario).FirstOrDefault();
                    return View("Bloqueadoprop", bp);
                }
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result2 = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result2)
            {
                case SignInStatus.Success:
                    return RedirectToLocal2(returnUrl, model);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Erro a fazer login.");


                    return View(model);
            }


        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Codigo Invalido");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegistoInfo model, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ModelState.AddModelError("", "Insira uma foto.");
                return View(model);
            }
            var db = new DBImobiliariaDataContext();

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {


                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    var userid = user.Id;
                    Proprietario prop = new Proprietario();
                    Inquilino inq = new Inquilino();
                    Administrador admin = new Administrador();
                    var s = model.Utilizador;
                    //obter o caminho fisico da raiz do servidor
                    var caminho = Server.MapPath("/");
                    if (s == "Proprietario")
                    {

                        prop.ID_Proprietario = userid;
                        prop.Tipo_Utilizador = "Proprietario";
                        prop.Email = user.Email;
                        prop.Nome = model.Nome;
                        prop.Data_de_Registo = DateTime.Now.Date;
                        prop.Data_Nascimento = model.Data_Nascimento;
                        prop.Numero_de_Telemovel = model.Numero_de_Telemovel;
                        prop.Cartao_de_Cidadao = model.Cartao_de_Cidadao;
                        prop.Username = model.Username;
                        prop.Password = user.PasswordHash;
                        prop.Estado = 3;
                        if (file != null)
                        {
                            //vereficar se o ficheiro é suportado 
                            if (!file.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                            && !file.ContentType.Equals("image/png")
                            && !file.ContentType.Equals(MediaTypeNames.Image.Gif)
                            )
                            {
                                ModelState.AddModelError("Foto_perfil", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                                return View();
                            }
                            //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                            //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                            //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                            string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file.FileName;
                            //guardar o ficheiro
                            file.SaveAs(caminho + "Content//" + caminho_restante);
                            //guardar o caminho para a bd
                            prop.Foto_perfil = "../../Content/" + caminho_restante;//meter o nome para usar na view
                            db.Proprietarios.InsertOnSubmit(prop);
                            db.SubmitChanges();
                            // return RedirectToAction("Index", "Home");

                        }
                        
                    }
                    else
                    {
                        if (s == "Inquilino")
                        {
                            inq.ID_Inquilino = userid;
                            inq.Tipo_Utilizador = "Inquilino";
                            inq.Email = user.Email;
                            inq.Nome = model.Nome;
                            inq.Data_de_Registo = DateTime.Now.Date;
                            inq.Data_Nascimento = model.Data_Nascimento;
                            inq.Numero_de_Telemovel = model.Numero_de_Telemovel;
                            inq.Cartao_de_Cidadao = model.Cartao_de_Cidadao;
                            inq.Username = model.Username;
                            inq.Password = user.PasswordHash;
                            inq.Estado = 3;
                            if (file != null)
                            {
                                //vereficar se o ficheiro é suportado 
                                if (!file.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                                && !file.ContentType.Equals("image/png")
                                && !file.ContentType.Equals(MediaTypeNames.Image.Gif)
                                )
                                {
                                    ModelState.AddModelError("Foto_perfil", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                                    return View();
                                }
                                //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                                //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                                //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                                string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file.FileName;
                                //guardar o ficheiro
                                file.SaveAs(caminho + "Content//" + caminho_restante);
                                //guardar o caminho para a bd
                                inq.Foto_perfil = "../../Content/" + caminho_restante;//meter o nome para usar na view
                                db.Inquilinos.InsertOnSubmit(inq);
                                db.SubmitChanges();
                                // return RedirectToAction("Index", "Home");
                            }
                           
                        }
                        else
                        {
                            if (s == "Administrador")
                            {
                                admin.ID_Administrador = userid;
                                admin.Tipo_Utilizador = "Administrador";
                                admin.Email = user.Email;
                                admin.Nome = model.Nome;
                                admin.Data_Nascimento = model.Data_Nascimento;
                                admin.Cartao_de_Cidadao = model.Cartao_de_Cidadao;
                                admin.Username = model.Username;
                                admin.Password = user.PasswordHash;
                                db.Administradors.InsertOnSubmit(admin);
                                db.SubmitChanges();
                                return RedirectToAction("IndexAdmin", "Home");
                            }
                        }




                    //ou user.Id


                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");


                }

                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(user.Email));  // replace with valid value 
                message.From = new MailAddress("imobutad@gmail.com");  // replace with valid value
                message.Subject = "Confirmação de e-mail";
                message.Body = string.Format(body, "Imobutad", "imobutad@gmail.com", "Para confirmar o e-mail clique no seguinte link: http://localhost:4914/Home/ConfirmarEmail/?id=" + user.Id);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "imobutad@gmail.com",  // replace with valid value
                        Password = "Aaaaaaa1$"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);

                }
                TempData["pendente"] = "A sua conta ainda não foi confirmada por favor verifique o seu e-mail!!!";
                return RedirectToAction("Index", "Home");

            }
            AddErrors(result);
        }
            
            // If we got this far, something failed, redisplay form
            return View(model);
    }
            

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                /*if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }*/

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

                int containquilinos = 0;
                int contaproprietarios = 0;
                int contaadmins = 0;
                var db = new DBImobiliariaDataContext();
                
                
                var result = await UserManager.ChangePasswordAsync(user.Id, "Aaaaa1$", "Bbbbb1$");
                if (result.Succeeded)
                {
                    var user1 = await UserManager.FindByIdAsync(user.Id);
                    if (user1 != null)
                    {
                        await SignInManager.SignInAsync(user1, isPersistent: false, rememberBrowser: false);


                        var userid = user1.Id;
                        Proprietario prop = new Proprietario();
                        Inquilino inq = new Inquilino();
                        Administrador admin = new Administrador();

                        foreach (Inquilino _inq in db.Inquilinos)
                        {
                            if (user.Id == _inq.ID_Inquilino)
                            {
                                containquilinos++;
                            }
                        }
                        if (containquilinos == 0)
                        {
                            foreach (Proprietario _prop in db.Proprietarios)
                            {
                                if (user.Id == _prop.ID_Proprietario)
                                {
                                    contaproprietarios++;
                                }
                            }
                        }
                        if (containquilinos == 0 && contaproprietarios == 0)
                        {
                            foreach (Administrador _admin in db.Administradors)
                            {
                                if (user.Id == _admin.ID_Administrador)
                                {
                                    contaadmins++;
                                }
                            }
                        }


                        if (contaproprietarios > 0)
                        {
                            prop = db.Proprietarios.Where(X => X.ID_Proprietario == user.Id).FirstOrDefault();

                            prop.Password = user.PasswordHash;
                            db.SubmitChanges();
                        }
                        else
                        {
                            if (containquilinos > 0)
                            {
                                inq = db.Inquilinos.Where(X => X.ID_Inquilino == user.Id).FirstOrDefault();
                                inq.Password = user.PasswordHash;
                                db.SubmitChanges();
                            }
                            else
                            {
                                if (contaadmins > 0)
                                {
                                    admin = db.Administradors.Where(X => X.ID_Administrador == user.Id).FirstOrDefault();
                                    admin.Password = user.PasswordHash;

                                    db.SubmitChanges();
                                }
                            }
                        }
                        var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                        var message = new MailMessage();
                        message.To.Add(new MailAddress(model.Email));  // replace with valid value 
                        message.From = new MailAddress("imobutad@gmail.com");  // replace with valid value
                        message.Subject = "Recuperação de password";
                        message.Body = string.Format(body, "Imobutad", "imobutad@gmail.com", "A sua nova nova password é: Bbbbb1$");
                        message.IsBodyHtml = true;

                        using (var smtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = "imobutad@gmail.com",  // replace with valid value
                                Password = "Aaaaaaa1$"  // replace with valid value
                            };
                            smtp.Credentials = credential;
                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.EnableSsl = true;
                            await smtp.SendMailAsync(message);


                        }

                        return RedirectToAction("ForgotPasswordConfirmation", "Account");
                    }
                }
            }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
        

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        private ActionResult RedirectToLocal2(string returnUrl, LoginViewModel model)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            Inquilino inq = default(Inquilino);
            Proprietario prop = default(Proprietario);
            Administrador admin = default(Administrador);
            BloqueiaInqulino bi = new BloqueiaInqulino();
            var db = new DBImobiliariaDataContext();

            inq = db.Inquilinos.Where(X => X.Email == model.Email).FirstOrDefault();


            if (inq!=default(Inquilino))
            {
                //FAZER A VIEW DO INQUILINO
                
                    return RedirectToAction("Index", "Home");
               
            }
            else
            {
                prop = db.Proprietarios.Where(X => X.Email == model.Email).FirstOrDefault();
                if (prop!=default(Proprietario))
                {
                    //FAZER A VIEW DO Proprietario
                    
                        return RedirectToAction("Index", "Home");
                  
                }
                else
                {
                    admin = db.Administradors.Where(X => X.Email == model.Email).FirstOrDefault();
                    if (admin!=default(Administrador))
                    {
                        //FAZER A VIEW DO Proprietario
                        return RedirectToAction("IndexAdmin", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}