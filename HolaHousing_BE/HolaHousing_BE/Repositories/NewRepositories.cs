﻿using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace HolaHousing_BE.Repositories
{
    public class NewRepositories : INewInterface
    {
        private readonly EXE201Context _context;
        public NewRepositories(EXE201Context context)
        {
                _context = context;
        }
        public New GetNew(int id)
        {
            return _context.News.Include(n=>n.CreatedByNavigation)
                .Include(n=>n.Tags)
                .Include(n=>n.PartContents)
                .FirstOrDefault(n => n.NewId == id);
        }

        public ICollection<New> GetNews()
        {
            return _context.News
                .Include(n => n.CreatedByNavigation)
                .Include(n => n.Tags)
                .Include(n => n.PartContents).ToList();
        }

        public List<New> GetRandomNews(int id, int n)
        {
            return _context.News.Where(n => n.NewId != id)
                                .OrderBy(n => Guid.NewGuid())
                                .Include(n => n.CreatedByNavigation)
                                .Include(n => n.Tags)
                                .Include(n => n.PartContents)
                                .Take(n)
                                .ToList();
        }

        public ICollection<Tag> GetTagsByNewId(int id)
        {
            return _context.News.Include(n => n.Tags).FirstOrDefault(n=>n.NewId==id).Tags.ToList();
        }

        public bool IsExisted(int id)
        {
            return _context.News.FirstOrDefault(n => n.NewId == id) != null ? true : false;
        }
    }
}
