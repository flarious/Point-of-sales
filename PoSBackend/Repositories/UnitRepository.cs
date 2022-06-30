using PoSBackend.Context;
using PoSBackend.Models;
using PoSBackend.ViewModels;

namespace PoSBackend.Repositories
{
    public class UnitRepository
    {
        readonly PoSDbContext dbContext;

        public UnitRepository(PoSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Response<List<UnitViewModel>> FindAll()
        {
            List<UnitViewModel> list = (from unit in dbContext.units
                                        orderby unit.id ascending
                                        select new UnitViewModel
                                        {
                                            Id = unit.id,
                                            Name = unit.name,
                                        }).ToList();

            return new Response<List<UnitViewModel>>
            {
                Code = 200,
                Message = "Success",
                Data = list,
            };
        }

        public Response<UnitViewModel> Create(Unit unit)
        {
            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    /*if (string.IsNullOrEmpty(unit.Name))
                    {
                        throw new ArgumentException("Unit's name is empty");
                    }*/

                    var isDuplicated = dbContext.units.Any(data => data.name == unit.Name);

                    if (isDuplicated)
                    {
                        throw new ArgumentException("Unit duplicated");
                    }

                    unit data = new unit
                    {
                        name = unit.Name,
                    };

                    dbContext.units.Add(data);
                    dbContext.SaveChanges();

                    dbContextTransaction.Commit();

                    return new Response<UnitViewModel>
                    {
                        Code = 200,
                        Message = "Success",
                        Data = null,
                    };
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();

                    var code = 500;
                    var message = e.InnerException == null ? e.Message : e.InnerException.Message;

                    return new Response<UnitViewModel>
                    {
                        Code = code,
                        Message = "Error: " + message,
                        Data = null,
                    };
                }
            }
        }

        public Response<UnitViewModel> Update(int id, Unit unit)
        {
            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    /*if (string.IsNullOrEmpty(unit.Name))
                    {
                        throw new ArgumentException("Unit's name is empty");
                    }*/

                    var isDuplicated = dbContext.units.Any(data => data.id != id && data.name == unit.Name);

                    if (isDuplicated)
                    {
                        throw new ArgumentException("Unit duplicated");
                    }

                    var row = dbContext.units.SingleOrDefault(data => data.id == id);
                    if (row != null)
                    {
                        row.name = unit.Name;

                        dbContext.SaveChanges();

                        dbContextTransaction.Commit();

                        return new Response<UnitViewModel>
                        {
                            Code = 200,
                            Message = "Success",
                            Data = null,
                        };
                    }
                    else
                    {
                        throw new ArgumentNullException("id: " + id.ToString(), "Unit not found");
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();

                    var code = 500;
                    var message = e.InnerException == null ? e.Message : e.InnerException.Message;

                    return new Response<UnitViewModel>
                    {
                        Code = code,
                        Message = "Error: " + message,
                        Data = null,
                    };
                }
            }
        }

        public Response<UnitViewModel> Delete(int id)
        {
            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var isInUsed = dbContext.items.Any(data => data.unit == id);

                    if (isInUsed)
                    {
                        throw new ArgumentException("Unit is already in used");
                    }

                    var row = dbContext.units.SingleOrDefault(data => data.id == id);

                    if (row != null)
                    {
                        dbContext.units.Remove(row);

                        dbContext.SaveChanges();

                        dbContextTransaction.Commit();

                        return new Response<UnitViewModel>
                        {
                            Code = 200,
                            Message = "Success",
                            Data = null,
                        };
                    }
                    else
                    {
                        throw new ArgumentNullException("id: " + id.ToString(), "Unit not found");
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();

                    var code = 500;
                    var message = e.InnerException == null ? e.Message : e.InnerException.Message;

                    /*if (message.Contains("Cannot delete or update a parent row: a foreign key constraint fails"))
                    {
                        message = "Unit is already in used";
                    }*/

                    return new Response<UnitViewModel>
                    {
                        Code = code,
                        Message = "Error: " + message,
                        Data = null,
                    };
                }
            }
        }
    }
}
