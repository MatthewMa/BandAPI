using BandAPI.DbContexts;
using BandAPI.Entities;
using BandAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.Services
{
    public class BandAlbumRepository : IBandAlbumRepository
    {
        private readonly BandAlbumContext _context;
        public BandAlbumRepository(BandAlbumContext context)
        {          
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void AddAlbum(Guid bandId, Album album)
        {
            if (bandId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bandId));
            }
            if (album == null)
            {
                throw new ArgumentNullException(nameof(album));
            }
            album.BandId = bandId;
            _context.Albums.Add(album);
        }

        public void AddBand(Band band)
        {
            if (band == null)
            {
                throw new ArgumentNullException(nameof(band));
            }
            _context.Bands.Add(band);
        }

        public bool AlbumExists(Guid AlbumId)
        {
            if (AlbumId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(AlbumId));
            }
            return _context.Albums.Any(a => a.Id == AlbumId);
        }

        public bool BandExists(Guid bandId)
        {
            if (bandId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bandId));
            }
            return _context.Bands.Any(a => a.Id == bandId);
        }

        public void DeleteAlbum(Album album)
        {
            if (album == null)
            {
                throw new ArgumentNullException(nameof(album));
            }
            _context.Albums.Remove(album);
        }

        public void DeleteAlbums(IEnumerable<Album> albums)
        {
            if (albums == null )
            {
                throw new ArgumentNullException(nameof(albums));
            }
            _context.RemoveRange(albums);
        }

        public void DeleteBand(Band band)
        {
            if (band == null)
            {
                throw new ArgumentNullException(nameof(band));
            }
            _context.Bands.Remove(band);
        }

        public Album GetAlbum(Guid bandId, Guid albumId)
        {
            if (bandId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bandId));
            }
            if (albumId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(albumId));
            }
            return _context.Albums.Where(a => a.Id == albumId && a.BandId == bandId).FirstOrDefault();
        }

        public IEnumerable<Album> GetAlbums(Guid bandId)
        {
            if (bandId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bandId));
            }
            return _context.Albums.Where(a => a.BandId == bandId).OrderBy(a => a.Title).ToList();

        }

        public Band GetBand(Guid bandId)
        {
            if (bandId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bandId));
            }
            return _context.Bands.Find(bandId);
        }

        public IEnumerable<Band> GetBands()
        {
            return _context.Bands.ToList();
        }

        public IEnumerable<Band> GetBands(IEnumerable<Guid> bandIds)
        {
            if (bandIds == null)
            {
                throw new ArgumentNullException(nameof(bandIds));
            }
            return _context.Bands.Where(b => bandIds.Contains(b.Id)).OrderBy(b => b.Name);
        }

        public PagedList<Band> GetBands(BandsResourceParameters bandsResourceParameter)
        {
            if (bandsResourceParameter == null)
            {
                throw new ArgumentException(nameof(bandsResourceParameter));
            }
            var mainGenre = bandsResourceParameter.MainGenre;
            var searchQuery = bandsResourceParameter.SearchQuery;
            int pageNumber = bandsResourceParameter.PageNumber;
            int pageSize = bandsResourceParameter.PageSize;          
            var collection = _context.Bands as IQueryable<Band>;          
            if (!string.IsNullOrWhiteSpace(mainGenre))
            {
                mainGenre = mainGenre.Trim();
                collection = collection.Where(b => b.MainGenre.ToUpper() == mainGenre.ToUpper());
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(b => b.Name.Contains(searchQuery));
            }
            return PagedList<Band>.Create(collection, pageNumber, pageSize);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateAlbum(Album album)
        {
            _context.Albums.Update(album);
        }

        public void UpdateBand(Band band)
        {
            _context.Bands.Update(band);
        }
    }
}
