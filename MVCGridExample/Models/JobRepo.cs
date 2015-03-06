using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCGrid.Web.Models
{
    public class JobRepo
    {
        const string CacheKey = "JobRepo";

        public IEnumerable<Job> GetData(out int totalRecords, int? limitOffset, int? limitRowCount, string orderBy, bool desc)
        {
            if (HttpContext.Current.Cache[CacheKey] == null)
            {
                List<Job> items = new List<Job>();
                int contactId = 0;
                for (int i = 1; i < 1087; i++)
                {
                    var j = new Job() { JobId = i, Name = RandomString(10) };

                    bool addContact = _rng.NextDouble() > 0.5;

                    if (addContact)
                    {
                        Contact c = new Contact();

                        contactId++;
                        j.Contact = c;
                        c.FullName = RandomString(5);
                        c.Id = contactId;
                    }

                    items.Add(j);
                }
                HttpContext.Current.Cache.Insert(CacheKey, items);
            }

            List<Job> data = (List<Job>)HttpContext.Current.Cache[CacheKey];
            totalRecords = data.Count;

            var q = data.AsQueryable();

            q = q.OrderBy(p => p.JobId);

            if (!String.IsNullOrWhiteSpace(orderBy))
            {
                //switch (orderBy.ToLower())
                //{
                //    case "col1":
                //        if (!desc)
                //            q = q.OrderBy(p => p.Col1);
                //        else
                //            q = q.OrderByDescending(p => p.Col1);
                //        break;
                //    case "col2":
                //        if (!desc)
                //            q = q.OrderBy(p => p.Col2);
                //        else
                //            q = q.OrderByDescending(p => p.Col2);
                //        break;
                //    case "col3":
                //        if (!desc)
                //            q = q.OrderBy(p => p.Col3);
                //        else
                //            q = q.OrderByDescending(p => p.Col3);
                //        break;
                //}
            }

            if (limitOffset.HasValue)
            {
                q = q.Skip(limitOffset.Value).Take(limitRowCount.Value);
            }

            return q.ToList();
        }

        private readonly Random _rng = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private string RandomString(int size)
        {
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }
    }

    public class Job
    {
        public int JobId { get; set; }
        public string Name { get; set; }

        public Contact Contact { get; set; }
    }

    public class Contact
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}