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
        IEnumerable<Person> GetData(out int totalRecords, string filterFirstName, string filterLastName, bool? filterActive, int? limitOffset, int? limitRowCount, string orderBy, bool desc);
    }
    public class PersonRepository : IPersonRepository
    {
        public IEnumerable<Person> GetData(out int totalRecords, string filterFirstName, string filterLastName, bool? filterActive, int? limitOffset, int? limitRowCount, string orderBy, bool desc)
        {
            using (var db = new SampleDatabaseEntities())
            {
                var query = db.People.AsQueryable();

                if (!String.IsNullOrWhiteSpace(filterFirstName))
                {
                    query = query.Where(p => p.FirstName.Contains(filterFirstName));
                }
                if (!String.IsNullOrWhiteSpace(filterLastName))
                {
                    query = query.Where(p => p.LastName.Contains(filterLastName));
                }
                if (filterActive.HasValue)
                {
                    query = query.Where(p => p.Active == filterActive.Value);
                }

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
                        case "active":
                            if (!desc)
                                query = query.OrderBy(p => p.Active);
                            else
                                query = query.OrderByDescending(p => p.Active);
                            break;
                        case "email":
                            if (!desc)
                                query = query.OrderBy(p => p.Email);
                            else
                                query = query.OrderByDescending(p => p.Email);
                            break;
                        case "gender":
                            if (!desc)
                                query = query.OrderBy(p => p.Gender);
                            else
                                query = query.OrderByDescending(p => p.Gender);
                            break;
                        case "id":
                            if (!desc)
                                query = query.OrderBy(p => p.Id);
                            else
                                query = query.OrderByDescending(p => p.Id);
                            break;
                        case "startdate":
                            if (!desc)
                                query = query.OrderBy(p => p.StartDate);
                            else
                                query = query.OrderByDescending(p => p.StartDate);
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

        public IEnumerable<Person> GetData(out int totalRecords, int? limitOffset, int? limitRowCount, string orderBy, bool desc)
        {
            return GetData(out totalRecords, null, null, null, limitOffset, limitRowCount, orderBy, desc);
        }
    }
}