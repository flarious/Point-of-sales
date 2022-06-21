using PoSBackend.Context;
using PoSBackend.Models;
using PoSBackend.ViewModels;

namespace PoSBackend.Repositories
{
    public class ItemRepository
    {
        readonly PoSDbContext dbContext;

        public ItemRepository(PoSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Response<List<ItemViewModel>> FindAll()
        {
            List<ItemViewModel> list = (from item in dbContext.items
                                    join unit in dbContext.units
                                    on item.unit equals unit.id
                                    orderby item.id ascending
                                    select new ItemViewModel
                                    {
                                        Id = item.id,
                                        Code = item.code,
                                        Name = item.name,
                                        Price = item.price,
                                        Unit = unit.name,
                                        Unit_id = unit.id,
                                    }).ToList();

            return new Response<List<ItemViewModel>>
            {
                Code = 200,
                Message = "Success",
                Data = list,
            };
        }

        public Response<ItemViewModel> Create(Item item)
        {
            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var isDuplicated = dbContext.items.Any(data => data.name == item.Name);

                    if (isDuplicated)
                    {
                        throw new ArgumentException("Item duplicated");
                    }

                    if (item.Code == "")
                    {
                        throw new ArgumentException("Item's code is empty");
                    }

                    if (item.Name == "")
                    {
                        throw new ArgumentException("Item's name is empty");
                    }

                    if (item.Unit_id == 0)
                    {
                        throw new ArgumentException("Item's unit is empty");
                    }

                    item data = new item
                    {
                        code = item.Code,
                        name = item.Name,
                        price = item.Price,
                        unit = item.Unit_id,
                    };

                    dbContext.items.Add(data);
                    dbContext.SaveChanges();

                    dbContextTransaction.Commit();

                    return new Response<ItemViewModel>
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

                    if (message.Contains("Cannot add or update a child row: a foreign key constraint fails"))
                    {
                        message = "Unit used not exist";
                    }

                    return new Response<ItemViewModel>
                    {
                        Code = code,
                        Message = "Error: " + message,
                        Data = null,
                    };
                }
            }
        }

        public Response<ItemViewModel> Update(int id, Item item)
        {
            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var isDuplicated = dbContext.items.Any(data => data.id != id && data.name == item.Name);

                    if (isDuplicated)
                    {
                        throw new ArgumentException("Item duplicated");
                    }

                    var row = dbContext.items.FirstOrDefault(data => data.id == id);
                    if (row != null)
                    {
                        row.name = item.Name;
                        row.price = item.Price;
                        row.unit = item.Unit_id;

                        dbContext.SaveChanges();

                        dbContextTransaction.Commit();

                        return new Response<ItemViewModel>
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

                    if (message.Contains("Cannot add or update a child row: a foreign key constraint fails"))
                    {
                        message = "Unit used not exist";
                    }

                    return new Response<ItemViewModel>
                    {
                        Code = code,
                        Message = "Error: " + message,
                        Data = null,
                    };
                }
            }
        }

        public Response<ItemViewModel> Delete(int id)
        {
            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var row = dbContext.items.FirstOrDefault(data => data.id == id);

                    if (row != null)
                    {
                        dbContext.items.Remove(row);

                        dbContext.SaveChanges();

                        dbContextTransaction.Commit();

                        return new Response<ItemViewModel>
                        {
                            Code = 200,
                            Message = "Success",
                            Data = null,
                        };
                    }
                    else
                    {
                        throw new ArgumentNullException("id: " + id.ToString(), "Item not found");
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();

                    var code = 500;
                    var message = e.InnerException == null ? e.Message : e.InnerException.Message;

                    if (message.Contains("Cannot delete or update a parent row: a foreign key constraint fails"))
                    {
                        message = "Item is already in used";
                    }

                    return new Response<ItemViewModel>
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
