using AutoMapper;
using BandAPI.Entities;
using BandAPI.Helpers;
using BandAPI.Models;
using BandAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.Controllers
{
    [ApiController]
    [Route("api/bandcollections")]
    public class BandCollectionsController: ControllerBase
    {
        private readonly IBandAlbumRepository _bandAlbumRepository;
        private readonly IMapper _mapper;
        public BandCollectionsController(IBandAlbumRepository bandAlbumRepository, IMapper mapper)
        {
            _bandAlbumRepository = bandAlbumRepository ??
                throw new ArgumentNullException(nameof(bandAlbumRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({ids})", Name = "GetBandCollection")]
        public ActionResult<IEnumerable<BandDTO>> GetBandCollection([FromRoute] 
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
                return BadRequest();
            var bandEntities = _bandAlbumRepository.GetBands(ids);
            if (ids.Count() != bandEntities.Count())
                return NotFound();
            IEnumerable<BandDTO> bandsForReturn = _mapper.Map<IEnumerable<BandDTO>>(bandEntities);
            return Ok(bandsForReturn);
        }

        [HttpPost]
        public ActionResult<IEnumerable<BandDTO>> CreateBandCollection(
            [FromBody] IEnumerable<BandCreateDTO> bandCollection)
        {
            var bandEntities = _mapper.Map<IEnumerable<Band>>(bandCollection);
            foreach (var band in bandEntities)
            {
                _bandAlbumRepository.AddBand(band);
            }
            _bandAlbumRepository.Save();
            var bandCollectionToReturn = _mapper.Map<IEnumerable<BandDTO>>(bandEntities);
            var IdsString = string.Join(",", bandCollectionToReturn.Select(a => a.Id));
            return CreatedAtRoute("GetBandCollection", new { ids = IdsString }, bandCollectionToReturn);
        }
    }
}
