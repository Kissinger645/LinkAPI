
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using LinkAPI.Models;

namespace LinkAPI.Controllers
{
    public class LinkController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Link
        public IQueryable<Link> GetLinks()
        {
            return db.Links.Where(l => l.Public == true);
        }

        // GET: api/Link/5
        [ResponseType(typeof(Link))]
        public IHttpActionResult GetLink(int id)
        {
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return NotFound();
            }

            return Ok(link);
        }

        // PUT: api/Link/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLink(int id, Link link)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != link.Id)
            {
                return BadRequest();
            }

            db.Entry(link).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinkExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Link
        [ResponseType(typeof(Link))]
        public IHttpActionResult PostLink(Link link)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var thisUser = User.Identity.GetUserId();
            if (thisUser == null)
            {
                link.UserName = null;
                link.Public = true;
            }
            else
            {
                link.UserName = User.Identity.GetUserId();
            }

            link.ShortUrl = Encrypt256(link.Url);
            link.Created = DateTime.Now;
            db.Links.Add(link);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = link.Id }, link);
        }

        static string Encrypt256(string input)
        {
            string shortUrl;
            byte[] byteData = Encoding.ASCII.GetBytes(input);
            Stream inputStream = new MemoryStream(byteData);

            using (SHA256 shaM = new SHA256Managed())
            {
                var result = shaM.ComputeHash(inputStream);
                shortUrl = BitConverter.ToString(result);
            }
            return shortUrl.Replace("-", "").Substring(0, 5);
        }


        // DELETE: api/Link/5
        [ResponseType(typeof(Link))]
        public IHttpActionResult DeleteLink(int id)
        {
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return NotFound();
            }

            db.Links.Remove(link);
            db.SaveChanges();

            return Ok(link);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LinkExists(int id)
        {
            return db.Links.Count(e => e.Id == id) > 0;
        }
    }
}
