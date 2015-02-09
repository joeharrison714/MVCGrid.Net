using MVCGrid.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCGrid.Web.Models
{
    public interface IPersonRepository
    {
        IEnumerable<Person> GetData(out int totalRecords, int? limitOffset, int? limitRowCount, string orderBy, bool desc);
    }
    public class PersonRepository : IPersonRepository
    {
        public IEnumerable<Person> GetData(out int totalRecords, int? limitOffset, int? limitRowCount, string orderBy, bool desc)
        {
            using (var db = new SampleDatabaseEntities())
            {
                var query = db.People.AsQueryable();

                totalRecords = query.Count();

                if (!String.IsNullOrWhiteSpace(orderBy))
                {
                    switch (orderBy.ToLower())
                    {
                        case "firstname":
                            if (!desc)
                                query = query.OrderBy(p => p.FirstName);
                            else
                                query = query.OrderByDescending(p => p.FirstName);
                            break;
                        case "lastname":
                            if (!desc)
                                query = query.OrderBy(p => p.LastName);
                            else
                                query = query.OrderByDescending(p => p.LastName);
                            break;
                    }
                }

                if (limitOffset.HasValue)
                {
                    query = query.Skip(limitOffset.Value).Take(limitRowCount.Value);
                }

                return query.ToList();
            }
        }
    }
}