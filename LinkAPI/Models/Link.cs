using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LinkAPI.Models
{
    public class Link
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ShortUrl { get; set; }
        public DateTime Created { get; set; }
        public bool Public { get; set; }

        public string UserName { get; set; } //Guid

        [ForeignKey("UserName")]
        protected virtual ApplicationUser Owner { get; set; }
    }

    public class ApplicationContext
    {
        public DbSet<Link> Links { get; set; }
    }
}