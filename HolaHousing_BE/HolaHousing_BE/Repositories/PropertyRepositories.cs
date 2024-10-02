using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace HolaHousing_BE.Repositories
{
    public class PropertyRepositories : IPropertyInterface
    {
        private readonly EXE201Context _context;
        public PropertyRepositories(EXE201Context context)
        {
            _context = context;
        }
        public ICollection<Property> GetProperties()
        {
            return _context.Properties
                .Include(p=>p.Amentities)
                .Include(p=>p.PropertyImages)
                .Where(p=>p.Status==0).ToList();
        }
        public IEnumerable<Property> GetPropertiesNear(double latitude, double longitude, double radiusInMeters)
        {
            double radiusInKm = radiusInMeters / 1000.0;
            const double EarthRadiusKm = 6371; 

            return _context.Properties
                .Where(p => p.Lat.HasValue && p.Lng.HasValue)
                .Where(p =>
                    EarthRadiusKm *
                    2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin((p.Lat.Value - latitude) * Math.PI / 180 / 2), 2) +
                    Math.Cos(latitude * Math.PI / 180) * Math.Cos(p.Lat.Value * Math.PI / 180) *
                    Math.Pow(Math.Sin((p.Lng.Value - longitude) * Math.PI / 180 / 2), 2)))
                    <= radiusInKm)
                .Include(p => p.Amentities)
                .Include(p => p.PropertyImages)
                .ToList();
        }
        public Property GetPropertyByID(int id)
        {
            if (IsExisted(id))
            {
                return _context.Properties
                    .Include(p => p.Amentities)
                    .Include(p => p.PropertyImages)
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
                .Include(p => p.Amentities).ToList();
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
    }
}
