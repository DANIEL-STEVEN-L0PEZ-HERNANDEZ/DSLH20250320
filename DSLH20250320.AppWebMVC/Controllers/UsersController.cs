﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DSLH20250320.AppWebMVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace DSLH20250320.AppWebMVC.Controllers
{

    [Authorize(Roles = "ADMINISTRADOR")]

    public class UsersController : Controller
    {
        private readonly Test20250320DbContext _context;

        public UsersController(Test20250320DbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Email,PasswordHash,Role,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                user.PasswordHash = CalcularHashMD5(user.PasswordHash);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }






        public async Task<IActionResult> CerrarSession()
        {
            // Hola mundo
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(User usuario)
        {
            usuario.PasswordHash = CalcularHashMD5(usuario.PasswordHash);
            var usuarioAuth = await _context.
                Users.
                FirstOrDefaultAsync(s => s.Email == usuario.Email && s.PasswordHash == usuario.PasswordHash);
            if (usuarioAuth != null && usuarioAuth.UserId > 0 && usuarioAuth.Email == usuario.Email)
            {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, usuarioAuth.Email),
                    new Claim("UserId", usuarioAuth.UserId.ToString()),
                     new Claim("Username", usuarioAuth.Username),
                    new Claim(ClaimTypes.Role, usuarioAuth.Role)
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "El email o contraseña estan incorrectos");
                return View();
            }
        }







        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Email,PasswordHash,Role,Status")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }
           var usuarioUpdate = await _context.Users
               .FirstOrDefaultAsync(m => m.UserId == user.UserId);
            try
            {
                usuarioUpdate.Username = user.Username;
                usuarioUpdate.Email = user.Email;
                usuarioUpdate.Status = user.Status;
                usuarioUpdate.Role = user.Role;
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                    return View(user);
                }
            }
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Profile()
        {

            var idStr = User.FindFirst("UserId")?.Value;
            int id = int.Parse(idStr);
            var usuario = await _context.Users.FindAsync(id);
            return View(usuario);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(int id, [Bind("Id,Nombre,Email,Estatus,Rol")] User usuario)
        {
            if (id != usuario.UserId)
            {
                return NotFound();
            }
            var usuarioUpdate = await _context.Users
                 .FirstOrDefaultAsync(m => m.UserId == usuario.UserId);
            try
            {
                usuarioUpdate.Username = usuario.Username;
                usuarioUpdate.Email = usuario.Email;
                usuarioUpdate.Status = usuario.Status;
                usuarioUpdate.Role = usuario.Role;
                _context.Update(usuarioUpdate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(usuario.UserId))
                {
                    return NotFound();
                }
                else
                {
                    return View(usuario);
                }
            }
        }
        private string CalcularHashMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // "x2" convierte el byte en una cadena hexadecimal de dos caracteres.
                }
                return sb.ToString();
            }
        }
    }
}
