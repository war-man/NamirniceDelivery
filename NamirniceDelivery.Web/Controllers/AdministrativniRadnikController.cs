﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NamirniceDelivery.Data.Entities;
using NamirniceDelivery.Services.Interfaces;
using NamirniceDelivery.Web.ViewModels.AdministrativniRadnik;

namespace NamirniceDelivery.Web.Controllers
{
    public class AdministrativniRadnikController : Controller
    {
        private static SignInManager<ApplicationUser> _signInManager;
        private readonly IKategorija _kategorijaService;
        private readonly INamirnica _namirnicaService;

        public AdministrativniRadnikController(SignInManager<ApplicationUser> signInManager, IKategorija kategorijaService, INamirnica namirnicaService)
        {
            _signInManager = signInManager;
            _kategorijaService = kategorijaService;
            _namirnicaService = namirnicaService;
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DemoLogin()
        {
            await _signInManager.PasswordSignInAsync("testRadnik", "password", false, lockoutOnFailure: true);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult KreirajKategoriju(string returnUrl = "")
        {
            return View(new KreirajKategorijuViewModel
            {
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult KreirajKategoriju(KreirajKategorijuViewModel model)
        {
            if (ModelState.IsValid)
            {
                _kategorijaService.KreirajKategoriju(new Kategorija
                {
                    Naziv = model.Naziv
                });
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction(nameof(PregledKategorija));
            }
            return View(model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult PregledKategorija(string returnUrl = "")
        {
            return View(new PregledKategorijaViewModel
            {
                ReturnUrl = returnUrl,
                KategorijaList = _kategorijaService.GetKategorije()
            });
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult EditKategorija(int kategorijaId, string returnUrl = "")
        {
            var kategorija = _kategorijaService.GetKategorija(kategorijaId);
            return View(new EditKategorijaViewModel
            {
                KategorijaId = kategorija.Id,
                Naziv = kategorija.Naziv,
                ReturnUrl = returnUrl
            });
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        [HttpPost]
        public IActionResult EditKategorija(EditKategorijaViewModel model)
        {
            if (ModelState.IsValid)
            {
                _kategorijaService.EditKategorija(new Kategorija
                {
                    Id = model.KategorijaId,
                    Naziv = model.Naziv
                });
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult KreirajNamirnica(string returnUrl = "")
        {
            return View(new KreirajNamirnicaViewModel
            {
                ReturnUrl = returnUrl,
                KategorijaList = _kategorijaService.GetKategorije()
            });
        }
        [HttpPost]
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult KreirajNamirnica(KreirajNamirnicaViewModel model)
        {
            if (ModelState.IsValid)
            {
                _namirnicaService.KreirajNamirnica(new Namirnica
                {
                    Naziv = model.Naziv,
                    KategorijaId = model.KategorijaId
                });
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction(nameof(PregledKategorija));
            }
            model.KategorijaList = _kategorijaService.GetKategorije();
            return View(model);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult PregledNamirnica(string returnUrl = "", int kategorijaId=0)
        {
            var v = new PregledNamirnicaViewModel
            {
                ReturnUrl = returnUrl,
                KategorijaList = _kategorijaService.GetKategorije()
            };
            if (kategorijaId!=0)
            {
                var k = _kategorijaService.GetKategorija(kategorijaId);
                v.NamirnicaList = _namirnicaService.GetNamirnicePoKategorijama(k);
                v.KategorijaId = kategorijaId;
            }
            else
            {
                v.NamirnicaList = _namirnicaService.GetNamirnice();
                v.KategorijaId = 0;
            }
            return View(v);
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        public IActionResult EditNamirnica(int namirnicaId, string returnUrl = "")
        {
            var namirnica = _namirnicaService.GetNamirnica(namirnicaId);
            return View(new EditNamirnicaViewModel
            {
                KategorijaId = namirnica.Kategorija.Id,
                Naziv = namirnica.Naziv,
                ReturnUrl = returnUrl,
                NamirnicaId = namirnica.Id,
                KategorijaList = _kategorijaService.GetKategorije()
            });
        }
        [Authorize(Roles = "AdministrativniRadnik")]
        [HttpPost]
        public IActionResult EditNamirnica(EditNamirnicaViewModel model)
        {
            if (ModelState.IsValid)
            {
                _namirnicaService.EditNamirnica(new Namirnica
                {
                    Id = model.NamirnicaId,
                    Naziv = model.Naziv,
                    KategorijaId = model.KategorijaId
                });
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction(nameof(PregledNamirnica));
            }
            model.KategorijaList = _kategorijaService.GetKategorije();
            return View(model);
        }
    }
}