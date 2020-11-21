using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BandAPI.Entities;
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
        public BandsController(IBandAlbumRepository bandAlbumRepository)
        {
            _bandAlbumRepository = bandAlbumRepository ?? 
                throw new ArgumentNullException(nameof(bandAlbumRepository));
        }
        [HttpGet]
        public IActionResult GetBands()
        {
            var bandsFromRepo = _bandAlbumRepository.GetBands();
            if (bandsFromRepo == null)
            {
                return NotFound();
            }
            var bandsDTO = new List<BandDTO>();
            foreach (var obj in bandsFromRepo)
            {
                bandsDTO.Add(new BandDTO{ 
                    Id = obj.Id,
                    Name = obj.Name,
                    MainGenre = obj.MainGenre,
                    // FoundedYearsAgo = $"{obj.Founded.ToString()} ({obj.Founded.GetYearsAgo()})"
                });
            }
            return Ok(bandsFromRepo);
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

    }
}
