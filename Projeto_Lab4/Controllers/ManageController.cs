using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Projeto_Lab4.Models;
using System.Collections.Generic;
using System.Net.Mime;

namespace Projeto_Lab4.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            var db = new DBImobiliariaDataContext();
            Administrador admin = db.Administradors.Where(X => X.ID_Administrador == User.Identity.GetUserId()).FirstOrDefault();
            if (admin != null)
            {
                return RedirectToAction("IndexAdmin", "Home");
            }
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            
            Inquilino inq = new Inquilino();
            Proprietario prop = new Proprietario();
            inq = db.Inquilinos.Where(X => X.ID_Inquilino == User.Identity.GetUserId()).FirstOrDefault();
            prop = db.Proprietarios.Where(X => X.ID_Proprietario == User.Identity.GetUserId()).FirstOrDefault();
            if (inq != null)
            {
                ViewBag.foto = inq.Foto_perfil;
                TempData["tipo"] = "inq";
            }
            if (prop!= null)
            {
                ViewBag.foto = prop.Foto_perfil;
                TempData["tipo"] = "Prop";
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Index(HttpPostedFileBase file1)
        {
            var db = new DBImobiliariaDataContext();
            var caminho = Server.MapPath("/");
            Foto photo = new Foto();
            Inquilino inq = db.Inquilinos.Where(X => X.ID_Inquilino == User.Identity.GetUserId()).FirstOrDefault();
            Proprietario prop = db.Proprietarios.Where(X => X.ID_Proprietario == User.Identity.GetUserId()).FirstOrDefault();
            if (file1 != null)
            {
                //vereficar se o ficheiro é suportado 
                if (!file1.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                && !file1.ContentType.Equals("image/png")
                && !file1.ContentType.Equals(MediaTypeNames.Image.Gif)
                )
                {
                    ModelState.AddModelError("Foto", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                    return View();
                }
                //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file1.FileName;
                photo.Nome_Foto = "Content//" + caminho_restante;

                //guardar o ficheiro
                file1.SaveAs(caminho + photo.Nome_Foto);
                //guardar o caminho para a bd
                photo.Nome_Foto = "../../Content/" + caminho_restante;//meter o nome para usar na view
                
                if (inq != null)
                {
                    inq.Foto_perfil = photo.Nome_Foto;
                }
                if (prop != null)
                {
                    prop.Foto_perfil = photo.Nome_Foto;
                }
                db.SubmitChanges();

            }
            else
            {
                if (inq != null)
                {
                    ViewBag.foto = inq.Foto_perfil;
                    TempData["tipo"] = "inq";
                }
                if (prop != null)
                {
                    ViewBag.foto = prop.Foto_perfil;
                    TempData["tipo"] = "Prop";
                }
                ModelState.AddModelError("Foto", "Não inseriu nenhuma foto!!!");
            }

            if (inq != null)
            {
                ViewBag.foto = inq.Foto_perfil;
                TempData["tipo"] = "inq";
            }
            if (prop != null)
            {
                ViewBag.foto = prop.Foto_perfil;
                TempData["tipo"] = "Prop";
            }
            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }
        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            int containquilinos = 0;
            int contaproprietarios = 0;
            int contaadmins = 0;
            var db = new DBImobiliariaDataContext();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                    var userid = user.Id;
                    Proprietario prop = new Proprietario();
                    Inquilino inq = new Inquilino();
                    Administrador admin = new Administrador();

                    foreach(Inquilino _inq in db.Inquilinos)
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
                    if (containquilinos == 0&& contaproprietarios == 0)
                    {
                        foreach (Administrador _admin in db.Administradors)
                        {
                            if (user.Id == _admin.ID_Administrador)
                            {
                                contaadmins++;
                            }
                        }
                    }

                    
                    if (contaproprietarios>0)
                    {
                        prop = db.Proprietarios.Where(X => X.ID_Proprietario == user.Id).FirstOrDefault();

                        prop.Password = user.PasswordHash;
                        db.SubmitChanges();
                    }
                    else
                    {
                        if (containquilinos>0)
                        {
                            inq = db.Inquilinos.Where(X => X.ID_Inquilino == user.Id).FirstOrDefault();
                            inq.Password = user.PasswordHash;
                            db.SubmitChanges();
                        }
                        else
                        {
                            if (contaadmins>0)
                            {
                                admin = db.Administradors.Where(X => X.ID_Administrador == user.Id).FirstOrDefault();
                                admin.Password = user.PasswordHash;
                                
                                db.SubmitChanges();
                            }
                        }
                    }


                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
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

        


     

        public ActionResult AlterarDadosPessoais(string id) {
            Inquilino inq;
            Proprietario prop;
            Administrador admin; 
            var db = new DBImobiliariaDataContext();
            inq = db.Inquilinos.Where(X => X.ID_Inquilino == id).FirstOrDefault();


            if (inq != default(Inquilino))
            {    
                return View("AlterarDadosPessoaisInquilino",inq);
             
            }
            else
            {
                prop = db.Proprietarios.Where(X => X.ID_Proprietario == id).FirstOrDefault();
                if (prop != default(Proprietario))
                {
                   return View("AlterarDadosPessoaisProprietario",prop);
                    

                }
                else
                {
                    admin = db.Administradors.Where(X => X.ID_Administrador == id).FirstOrDefault();
                    if (admin != default(Administrador))
                    {
                       
                                return View("AlterarDadosPessoaisAdmin",admin);
                       

                    }
                    else
                    {
                        ModelState.AddModelError("", "Ups ocorreu um erro a alterar oss seus dados.");
                    }
                }
            }

            return View();
        }



        [HttpPost]
        public ActionResult AlterarDadosPessoais(string id,FormCollection form)
        {
            

            Inquilino inq = default(Inquilino);
            Proprietario prop = default(Proprietario);
            Administrador admin = default(Administrador);
            var db = new DBImobiliariaDataContext();

            inq = db.Inquilinos.Where(X => X.ID_Inquilino == id).FirstOrDefault();

            
            if (inq != default(Inquilino))
            {   foreach(Inquilino _inq in db.Inquilinos)
                {
                    if (_inq.ID_Inquilino == id)
                    {
                        if (form["Nome"] != null)
                            _inq.Nome = form["Nome"];

                        if (form["Data_Nascimento"] != null)
                            _inq.Data_Nascimento = Convert.ToDateTime(form["Data_Nascimento"]);

                        if (form["Numero_de_Telemovel"] != null)
                            _inq.Numero_de_Telemovel = form["Numero_de_Telemovel"];


                        if (form["Cartao_de_Cidadao"] != null)
                            _inq.Cartao_de_Cidadao = Convert.ToInt32(form["Cartao_de_Cidadao"]);
                        db.SubmitChanges();
                        TempData["dados"] = "Os seus dados foram alterados com sucesso";
                        return View("AlterarDadosPessoaisInquilino",_inq);
                    }
                }
               
            }
            else
            {
                prop = db.Proprietarios.Where(X => X.ID_Proprietario == id).FirstOrDefault();
                if (prop != default(Proprietario))
                {
                    foreach (Proprietario _prop in db.Proprietarios)
                    {
                        if (_prop.ID_Proprietario == id)
                        {
                            if(form["Nome"]!=null)
                            _prop.Nome = form["Nome"];


                            if (form["Data_Nascimento"] != null)
                                _prop.Data_Nascimento = Convert.ToDateTime(form["Data_Nascimento"]);
                            if (form["Numero_de_Telemovel"] != null)
                                _prop.Numero_de_Telemovel = form["Numero_de_Telemovel"];

                            if (form["Cartao_de_Cidadao"] != null)
                                _prop.Cartao_de_Cidadao = Convert.ToInt32(form["Cartao_de_Cidadao"]);
                            db.SubmitChanges();

                            TempData["dados"] = "Os seus dados foram alterados com sucesso";
                            return View("AlterarDadosPessoaisProprietario",_prop);

                        }
                    }

                }
                else
                {
                    admin = db.Administradors.Where(X => X.ID_Administrador == id).FirstOrDefault();
                    if (admin != default(Administrador))
                    {
                        foreach (Administrador _admin in db.Administradors)
                        {
                            if (_admin.ID_Administrador == id)
                            {
                                _admin.ID_Administrador = form["ID_Inquilino"];
                                _admin.Nome = form["Nome"];
                                _admin.Email = form["Email"];
                                _admin.Data_Nascimento = Convert.ToDateTime(form["Data_Nascimento"]);
                                _admin.Password = form["Password"];
                                _admin.Username = form["Username"];
                                _admin.Tipo_Utilizador = form["Tipo_Utilizador"];
                                
                                _admin.Cartao_de_Cidadao = Convert.ToInt32(form["Cartao_de_Cidadao"]);
                                db.SubmitChanges();

                                TempData["dados"] = "Os seus dados foram alterados com sucesso";
                                return View("AlterarDadosPessoaisAdmin",_admin);


                            }
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Ups ocorreu um erro a alterar oss seus dados.");
                    }
                }
            }

            return View();
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

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}