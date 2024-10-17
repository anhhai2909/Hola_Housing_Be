using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace HolaHousing_BE.Repositories
{
    public class PropertyRepositories : IPropertyInterface
    {
        private readonly EXE201Context _context;
        private readonly IPropertyImageInterface _propertyImageInterface;
        public PropertyRepositories(EXE201Context context, IPropertyImageInterface propertyImageInterface)
        {
            _context = context;
            _propertyImageInterface = propertyImageInterface;
        }
        public ICollection<PropertyDeclineReason> GetReasonsByPro(int proId)
        {
            return _context.PropertyDeclineReasons.Include(p => p.Reason).Where(p => p.PropertyId == proId).ToList();
        }
        public ICollection<Property> SearchProperty(int? sortBy, String? searchString
            , String? propertyType
            , String? address, String? city
            , String? district, String? ward
            , decimal? priceFrom, decimal? priceTo)
        {
            var query = _context.Properties.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Content.ToLower().Contains(searchString.ToLower()));
            }
            if (!string.IsNullOrEmpty(propertyType))
            {
                query = query.Where(p => p.PropertyType.ToLower().Equals(propertyType.ToLower()));
            }
            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(p => p.Address.ToLower().Equals(address.ToLower()));
            }
            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(p => p.City.ToLower().Equals(city.ToLower()));
            }
            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(p => p.District.ToLower().Equals(district.ToLower()));
            }
            if (!string.IsNullOrEmpty(ward))
            {
                query = query.Where(p => p.Ward.ToLower().Equals(ward.ToLower()));
            }
            if (priceFrom >= 0 && priceTo > 0)
            {
                query = query.Where(p => p.Price >= priceFrom && p.Price <= priceTo);
            }
            switch (sortBy)
            {
                case 1:
                    query = query.OrderBy(p => p.Price);
                    break;
                case 2:
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case 3:
                    query = query.OrderByDescending(p => p.PostTime);
                    break;
                default:
                    query = query.OrderBy(p => p.PostTime);
                    break;
            }

            return query.ToList();
        }
        public ICollection<Property> GetProperties()
        {
            return _context.Properties
                .Include(p => p.Amentities)
                .Include(p => p.PropertyImages)
                .Include(p => p.PostPrices)
                .Where(p => p.Status == 0).ToList();
        }
        public IEnumerable<Property> GetPropertiesNear(double latitude, double longitude, int pid, double radiusInMeters)
        {
            double radiusInKm = radiusInMeters / 1000.0;
            const double EarthRadiusKm = 6371;

            return _context.Properties
                .Where(p => p.Lat.HasValue && p.Lng.HasValue)
                .Where(p => p.PropertyId != pid)
                .Where(p =>
                    EarthRadiusKm *
                    2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin((p.Lat.Value - latitude) * Math.PI / 180 / 2), 2) +
                    Math.Cos(latitude * Math.PI / 180) * Math.Cos(p.Lat.Value * Math.PI / 180) *
                    Math.Pow(Math.Sin((p.Lng.Value - longitude) * Math.PI / 180 / 2), 2)))
                    <= radiusInKm)
                .Include(p => p.Amentities)
                .Include(p => p.PropertyImages)
                .Include(p => p.PostPrices)
                .ToList();
        }
        public Property GetPropertyByID(int id)
        {
            if (IsExisted(id))
            {
                return _context.Properties
                    .Include(p => p.Amentities)
                    .Include(p => p.PropertyImages)
                    .Include(p => p.PostPrices)
                    .Include(p => p.PropertyDeclineReasons)
                    .FirstOrDefault(p => p.PropertyId == id);
            }
            else
            {
                return null;
            }

        }
        public string GetFirstImage(int id)
        {
            var img = _context.Properties.FirstOrDefault(p => p.PropertyId == id).PropertyImages.First().Image;
            return img != null ? img : "";

        }

        public bool IsExisted(int id)
        {
            return _context.Properties.FirstOrDefault(p => p.PropertyId == id) != null;
        }
        public ICollection<Property> GetPropertiesByAmentities(List<int> amentities)
        {
            var properties = _context.Properties
                .Include(p => p.PropertyImages)
                .Include(p => p.Amentities)
                .Include(p => p.PostPrices).ToList();
            if (amentities.Count > 0)
            {
                var matchingProperties = new List<Property>();
                foreach (var property in properties)
                {
                    if (amentities.All(a => property.Amentities.Any(amen => amen.AmentityId == a)))
                    {
                        matchingProperties.Add(property);
                    }
                }
                return matchingProperties;
            }
            else
            {
                return properties;
            }
        }
        public virtual User? Poster { get; set; }
        public virtual ICollection<PropertyDeclineReason>? PropertyDeclineReasons { get; set; }
        public virtual ICollection<PropertyImage>? PropertyImages { get; set; }

        public virtual ICollection<Amentity>? Amentities { get; set; }
        public virtual ICollection<PostPrice>? PostPrices { get; set; }
        public int CreateProperty(Property property)
        {
            property.PropertyImages ??= new HashSet<PropertyImage>();

            if (property.Amentities != null)
            {
                property.Amentities = new HashSet<Amentity>(
                    _context.Amentities
                        .Where(a => property.Amentities.Select(p => p.AmentityId).Contains(a.AmentityId))
                        .ToList());
            }

            if (property.PostPrices != null)
            {
                property.PostPrices = new HashSet<PostPrice>(
                    _context.PostPrices
                        .Where(p => property.PostPrices.Select(pr => pr.PostPriceId).Contains(p.PostPriceId))
                        .ToList());
            }

            if (property.PropertyDeclineReasons != null)
            {
                property.PropertyDeclineReasons = new HashSet<PropertyDeclineReason>(
                    _context.PropertyDeclineReasons
                        .Where(r => property.PropertyDeclineReasons.Select(pr => pr.ReasonId).Contains(r.ReasonId))
                        .ToList());
            }
            DateTime dateTime = DateTime.Now;
            property.CreatedAt = dateTime;
            property.UpdatedAt = dateTime;
            _context.Properties.Add(property);
            _context.SaveChanges();

            return property.PropertyId;
        }

        public bool SaveChanged()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }

        public int UpdateProperty(Property property)
        {
            var existingProperty = _context.Properties
               .Include(p => p.Amentities)
               .Include(p => p.PostPrices)
               .Include(p => p.PropertyDeclineReasons)
               .FirstOrDefault(p => p.PropertyId == property.PropertyId);

            if (existingProperty == null)
            {
                return 0;
            }

            existingProperty.Content = property.Content;
            existingProperty.Description = property.Description;
            existingProperty.Price = property.Price;
            existingProperty.PostTime = property.PostTime;
            existingProperty.Address = property.Address;
            existingProperty.City = property.City;
            existingProperty.District = property.District;
            existingProperty.Ward = property.Ward;
            existingProperty.PropertyType = property.PropertyType;
            existingProperty.Area = property.Area;
            existingProperty.Lat = property.Lat;
            existingProperty.Lng = property.Lng;
            existingProperty.PhoneNum = property.PhoneNum;
            existingProperty.Owner = property.Owner;
            existingProperty.Status = property.Status;
            existingProperty.UpdatedAt = DateTime.Now;

            if (property.Amentities != null)
            {
                existingProperty.Amentities = new HashSet<Amentity>(
                    _context.Amentities
                        .Where(a => property.Amentities.Select(p => p.AmentityId).Contains(a.AmentityId))
                        .ToList());
            }

            if (property.PostPrices != null)
            {
                existingProperty.PostPrices = new HashSet<PostPrice>(
                    _context.PostPrices
                        .Where(p => property.PostPrices.Select(pr => pr.PostPriceId).Contains(p.PostPriceId))
                        .ToList());
            }

            if (property.PropertyDeclineReasons != null)
            {
                existingProperty.PropertyDeclineReasons = new HashSet<PropertyDeclineReason>(
                    _context.PropertyDeclineReasons
                        .Where(r => property.PropertyDeclineReasons.Select(pr => pr.ReasonId).Contains(r.ReasonId))
                        .ToList());
            }
            _context.SaveChanges();

            return property.PropertyId;
        }

        public bool DeleteProperty(Property property)
        {
            foreach (var item in _context.Amentities.Include(a => a.Properties).ToList())
            {
                item.Properties = item.Properties
                    .Where(amen => amen.PropertyId != property.PropertyId)
                    .ToList();
            }
            foreach (var item in _context.PostPrices.ToList())
            {
                foreach (var p in item.Properties)
                {
                    if (p.PropertyId == property.PropertyId)
                    {
                        item.Properties.Remove(p);
                    }
                }
            }
            _context.PropertyImages
            .Where(item => item.PropertyId == property.PropertyId)
            .ToList()
            .ForEach(item => _propertyImageInterface.DeletePropertyImage(item));
            _context.Properties.Remove(property);
            return SaveChanged();
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public ICollection<Property> GetPropertiesByPoster(int posterId, int pid)
        {
            return _context.Properties
                .Include(p => p.PropertyImages)
                .Include(p => p.Amentities)
                .Include(p => p.PostPrices)
                .Where(p => p.PosterId == posterId)
                .Where(p => p.PropertyId != pid)
                .ToList();
        }

        public string GetPhone(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId).PhoneNum;
        }

        public ICollection<Property> paging(int pageSize, int pageNumber)
        {
            var properties = GetProperties();

            var pagedProperties = properties
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return pagedProperties;
        }

        public ICollection<SmallPropertyDTO> paging(List<SmallPropertyDTO> list, int pageSize, int pageNumber)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
            }

            if (pageNumber < 1)
            {
                throw new ArgumentException("Page number must be greater than or equal to one.", nameof(pageNumber));
            }
            int skip = (pageNumber - 1) * pageSize;
            var pagedList = list.Skip(skip).Take(pageSize).ToList();
            return pagedList;
        }

        public ICollection<Property> GetPropertiesByPosterAndStatus(int userId, int status)
        {
            return _context.Properties.Where(p => p.PosterId == userId && p.Status == status).ToList();
        }

        public bool UpdateStatus(int propertyId, int status)
        {
            var pro = _context.Properties.FirstOrDefault(p => p.PropertyId == propertyId);
            pro.Status = status;
            _context.Properties.Update(pro);
            return SaveChanged();
        }

        public bool AddPropertyDeclineReason(int proId, int? reasonId, string others)
        {
            PropertyDeclineReason p = new PropertyDeclineReason();
            p.PropertyId = proId;
            if (reasonId != null)
            {
                p.ReasonId = reasonId;
            }
            p.Others = others;
            _context.PropertyDeclineReasons.Add(p);
            return SaveChanged();
        }

        public bool DeletePropertyDeclineReasons(int proId)
        {
            var item = _context.PropertyDeclineReasons.Where(p => p.PropertyId == proId).ToList();
            _context.PropertyDeclineReasons.RemoveRange(item);
            return SaveChanged();
        }

        public PropertyDeclineReason GetPropertyDeclineReason(int proId, int? reasonId)
        {
            return _context.PropertyDeclineReasons.FirstOrDefault(p => p.PropertyId == proId && p.ReasonId == reasonId);
        }
    }
}
