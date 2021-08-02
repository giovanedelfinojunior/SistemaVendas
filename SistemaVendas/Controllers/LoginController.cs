using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVendas.DAL;
using SistemaVendas.Helpers;
using SistemaVendas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVendas.Controllers
{
    public class LoginController : Controller
    {
        protected ApplicationDbContext mContext;
        protected IHttpContextAccessor HttpContextAccessor;

        public LoginController(ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            mContext = context;
            HttpContextAccessor = httpContext;
        }

        public IActionResult Index(int? id)
        {
            if(id != null) {
                if(id == 0){
                    HttpContextAccessor.HttpContext.Session.Clear();
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {

            ViewData[ "ErroLogin" ] = string.Empty;

            if(ModelState.IsValid) {
                var usuario = mContext.Usuario.Where(x => x.Email == model.Email && x.Senha == model.Senha).FirstOrDefault();
                if(usuario == null) {
                    ViewData[ "ErroLogin" ] = "O Email ou Senha informado não existe no sistema!";
                    return View(model);
                } else {
                    //colocar os dados do usuário na sessão
                    HttpContextAccessor.HttpContext.Session.SetString(Sessao.NOME_USUARIO, usuario.Nome);
                    HttpContextAccessor.HttpContext.Session.SetString(Sessao.EMAIL_USUARIO, usuario.Email);
                    HttpContextAccessor.HttpContext.Session.SetInt32(Sessao.CODIGO_USUARIO, (int)usuario.Codigo);
                    HttpContextAccessor.HttpContext.Session.SetInt32(Sessao.LOGADO, 1);

                    return RedirectToAction("Index", "Home");
                }
            } else {
                return View(model);
            }
        }
    }
}
