using AutoMapper;
using RO.Chat.IO.Domain.Usuario.Entities;
using RO.Chat.IO.Domain.Usuario.Interfaces;
using RO.Chat.IO.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace RO.Chat.IO.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        public AccountController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrarViewModel registrarViewModel)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = _mapper.Map<Usuario>(registrarViewModel);
                usuario = _usuarioService.AdicionarUsuario(usuario);
                if (usuario.Erros.Count() <= 0)
                {
                    AutenticarUsuario(usuario);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(usuario.Erros.ToList());
            }

            return View(registrarViewModel);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = _usuarioService.ObterUsuarioPorEmailEhSenha(loginViewModel.Email, loginViewModel.Senha);
                if (usuario.Erros.Count() <= 0)
                {
                    AutenticarUsuario(usuario);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(usuario.Erros.ToList());
            }

            return View(loginViewModel);
        }

        private void AutenticarUsuario(Usuario usuario)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name,usuario.NomeUsuario),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id_Usuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
            },
            "ChatCookie");

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(identity);
        }

        private void AddErrors(List<string> result)
        {
            foreach (var error in result)
            {
                ModelState.AddModelError("", error);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ChatCookie");

            return RedirectToAction("Index", "Home");
        }
    }
}