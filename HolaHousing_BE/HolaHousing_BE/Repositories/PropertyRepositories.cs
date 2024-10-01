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
            return _context.Properties.Include(p=>p.PropertyImages).Where(p=>p.Status==0).ToList();
        }

        public Property GetPropertyByID(int id)
        {
            if (IsExisted(id))
            {
                return _context.Properties.FirstOrDefault(p => p.PropertyId == id);
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
            var properties = _context.Properties.Include(p => p.Amentities).ToList();
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
