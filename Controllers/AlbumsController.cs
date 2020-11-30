using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BandAPI.Entities;
using BandAPI.Models;
using BandAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BandAPI.Controllers
{
    [ApiController]
    [Route("api/bands/{bandId}/albums")]
    public class AlbumsController : ControllerBase
    {
        private readonly IBandAlbumRepository _bandAlbumRepository;
        private readonly IMapper _mapper;
        public AlbumsController(IBandAlbumRepository bandAlbumRepository, IMapper mapper)
        {
            _bandAlbumRepository = bandAlbumRepository ??
                throw new ArgumentNullException(nameof(bandAlbumRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<AlbumDTO>> GetAlbumsForBand(Guid bandId)
        {
            if (!_bandAlbumRepository.BandExists(bandId))
            {
                return NotFound();
            }
            var albumsFromRepo = _bandAlbumRepository.GetAlbums(bandId);
            return Ok(_mapper.Map<IEnumerable<Album>, IEnumerable<AlbumDTO>>(albumsFromRepo));
        }

        [HttpGet("{albumId}", Name = "GetAlbum")]
        public ActionResult<AlbumDTO> GetAlbumForBand(Guid bandId, Guid albumId)
        {
            if (!_bandAlbumRepository.BandExists(bandId))
            {
                return NotFound();
            }
            Album album = _bandAlbumRepository.GetAlbum(bandId, albumId);
            if (album == null)
                return NotFound();
            return Ok(_mapper.Map<Album, AlbumDTO>(album));
        }

        [HttpPost]
        public ActionResult<AlbumDTO> CreateAlbumForBand(Guid bandId, [FromBody] AlbumCreateDTO album)
        {
            if (!_bandAlbumRepository.BandExists(bandId))
            {
                return NotFound();
            }
            var albumEntity = _mapper.Map<Entities.Album>(album);
            _bandAlbumRepository.AddAlbum(bandId, albumEntity);
            _bandAlbumRepository.Save();
            var albumReturn = _mapper.Map<AlbumDTO>(albumEntity);
            return CreatedAtRoute("GetAlbum", new { bandId = bandId, albumId = albumReturn.Id }, albumReturn);
        }


    }
}
