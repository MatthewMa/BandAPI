﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BandAPI.Entities;
using BandAPI.Helpers;
using BandAPI.Models;
using BandAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BandAPI.Controllers
{
    [ApiController]
    [Route("api/bands")]
    public class BandsController : ControllerBase
    {
        private readonly IBandAlbumRepository _bandAlbumRepository;
        private readonly IMapper _mapper;
        public BandsController(IBandAlbumRepository bandAlbumRepository, IMapper mapper)
        {
            _bandAlbumRepository = bandAlbumRepository ?? 
                throw new ArgumentNullException(nameof(bandAlbumRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet("{bandId}")]
        public IActionResult GetBand(Guid bandId)
        {
            if (bandId == null)
            {
                return BadRequest();
            }          
            Band band = _bandAlbumRepository.GetBand(bandId);
            if (band == null)
            {
                return NotFound();
            }
            return Ok(band);
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<BandDTO>> GetBands([FromQuery] BandsResourceParameters bandsResourceParameter)
        {
            IEnumerable<Band> bands = _bandAlbumRepository.GetBands(bandsResourceParameter);
            if (bands == null)
                return NotFound();
            return Ok(_mapper.Map<IEnumerable<Band>, IEnumerable<BandDTO>>(bands));
        }

    }
}
