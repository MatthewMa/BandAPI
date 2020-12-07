using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        [HttpGet("{bandId}", Name = "GetBand")]
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

        [HttpGet(Name = "GetBands")]
        [HttpHead]
        public ActionResult<IEnumerable<BandDTO>> GetBands([FromQuery] BandsResourceParameters bandsResourceParameter)
        {
            var bands = _bandAlbumRepository.GetBands(bandsResourceParameter);
            var previousLink = bands.HasPrevious ? CreateBandsUri(bandsResourceParameter, UriType.PreviousPage) : null;
            var nextLink = bands.HasNext ? CreateBandsUri(bandsResourceParameter, UriType.NextPage) : null;
            var metaData = new
            {
                totalCount = bands.Count,
                pageSize = bands.PageSize,
                currentPage = bands.CurrentPage,
                totalPages = bands.TotalPages,
                previousLink,
                nextLink
            };
            Response.Headers.Add("Pagination", JsonSerializer.Serialize(metaData));
            return Ok(_mapper.Map<IEnumerable<Band>, IEnumerable<BandDTO>>(bands));
        }

        [HttpPost]
        public ActionResult<BandDTO> CreateBand([FromBody] BandCreateDTO band)
        {
            var bandEnity = _mapper.Map<Band>(band);
            _bandAlbumRepository.AddBand(bandEnity);
            _bandAlbumRepository.Save();
            var bandToReturn = _mapper.Map<BandDTO>(bandEnity);
            return CreatedAtRoute("GetBand", new { bandId = bandToReturn.Id }, bandToReturn);
        }

        [HttpOptions]
        public IActionResult GetBandsOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,DELETE,HEAD,OPTIONS");
            return Ok();
        }

        [HttpDelete("{bandId}")]
        public ActionResult DeleteBand(Guid bandId)
        {
            if (!_bandAlbumRepository.BandExists(bandId))
                return NotFound();
            // Look for all related albums
            IEnumerable<Album> albums = _bandAlbumRepository.GetAlbums(bandId);
            // Delete all related albums
            _bandAlbumRepository.DeleteAlbums(albums);
            // Look for band
            var band = _bandAlbumRepository.GetBand(bandId);
            // Delete band
            _bandAlbumRepository.DeleteBand(band);
            _bandAlbumRepository.Save();
            return NoContent();
        }

        private string CreateBandsUri(BandsResourceParameters bandsResourceParameters, UriType uriType)
        {
            switch(uriType)
            {
                case UriType.PreviousPage:
                    return Url.Link("GetBands", new
                    {
                        pageNumber = bandsResourceParameters.PageNumber - 1,
                        pageSize = bandsResourceParameters.PageSize,
                        mainGenre = bandsResourceParameters.MainGenre,
                        searchQuery = bandsResourceParameters.SearchQuery
                    });
                case UriType.NextPage:
                    return Url.Link("GetBands", new
                    {
                        pageNumber = bandsResourceParameters.PageNumber + 1,
                        pageSize = bandsResourceParameters.PageSize,
                        mainGenre = bandsResourceParameters.MainGenre,
                        searchQuery = bandsResourceParameters.SearchQuery
                    });
                default:
                    return Url.Link("GetBands", new
                    {
                        pageNumber = bandsResourceParameters.PageNumber + 1,
                        pageSize = bandsResourceParameters.PageSize,
                        mainGenre = bandsResourceParameters.MainGenre,
                        searchQuery = bandsResourceParameters.SearchQuery
                    });

            }         
        }
    }
}
