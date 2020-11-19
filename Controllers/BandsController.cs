﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BandAPI.Entities;
using BandAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BandAPI.Controllers
{
    [ApiController]
    [Route("api/bands")]
    public class BandsController : ControllerBase
    {
        private readonly IBandAlbumRepository _bandAlbumRepository;
        public BandsController(IBandAlbumRepository bandAlbumRepository)
        {
            _bandAlbumRepository = bandAlbumRepository ?? 
                throw new ArgumentNullException(nameof(bandAlbumRepository));
        }
        [HttpGet]
        public IActionResult GetBands()
        {
            var bandsFromRepo = _bandAlbumRepository.GetBands();
            return new JsonResult(bandsFromRepo);
        }
        [HttpGet("{bandId: Guid}")]
        public IActionResult GetBand(Guid bandId)
        {
            if (bandId == null)
            {
                return NotFound();
            }
            Band band = _bandAlbumRepository.GetBand(bandId);
            return new JsonResult(band);
        }
    }
}
