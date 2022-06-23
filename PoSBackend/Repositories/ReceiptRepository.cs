using PoSBackend.Context;
using PoSBackend.Models;
using PoSBackend.ViewModels;
using System.Globalization;

namespace PoSBackend.Repositories
{
    public class ReceiptRepository
    {
        readonly PoSDbContext dbContext;

        public ReceiptRepository(PoSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /*public List<ReceiptViewModel> FindAll()
        {
            *//*select
            a.*,
            (select sum(c.price_total)
            from(
            select((b.item_quantity * b.item_price) * (1 - (b.item_discount_percent / 100))) price_total from pointofsale.order b where b.receipt = a.id
            ) c) -a.trade_discount grand_total
            from pointofsale.receipt a where a.id = 8*/

            /*var grandTotalQuery = from a in dbContext.orders
                                  select new { 
                                      receiptId = a.receipt,
                                      price_total = (a.item_quantity * a.item_price) * (1 - (a.item_discount_percent / 100)),
                                  };*/

            /*List<order> orders = dbContext.orders.ToList();
            List<receipt> receipts = (from receipt in dbContext.receipts
                                     orderby receipt.id ascending
                                     select receipt).ToList();
            List<ReceiptViewModel> list = new List<ReceiptViewModel>();

            foreach (var receipt in receipts)
            {
                var item = new ReceiptViewModel();
                item.Id = receipt.id;
                decimal sum = 0;
                foreach (var order in orders)
                {
                    if (order.receipt != receipt.id)
                    {
                        continue;
                    }

                    sum += (order.item_quantity * order.item_price) * (1 - (order.item_discount_percent / 100));
                }

                item.Grand_total = sum - receipt.trade_discount;

                list.Add(item);
            }*/

            /* List < ReceiptViewModel > list = (from receipt in dbContext.receipts

                                               orderby receipt.id ascending
                                               select new ReceiptViewModel
                                               {
                                                   Id = receipt.id,
                                                   Code = receipt.code,
                                                   Date = receipt.date.ToString("dd/MM/yyyy"),
                                                   Grand_total = grandTotalQuery.Where(x => x.receiptId == receipt.id).Select(x => x.price_total).Sum() - receipt.trade_discount,
                                               }).ToList();*//*

            List<ReceiptViewModel> list = (from receipt in dbContext.receipts
                                           orderby receipt.id ascending
                                           select new ReceiptViewModel
                                           {
                                               Id = receipt.id,
                                               Code = receipt.code,
                                               Date = receipt.date.ToString("dd/MM/yyyy"),
                                               Grand_total = receipt.grand_total,
                                           }).ToList();

            return list;
        }*/

        public Response<List<ReceiptViewModel>> FindAll()
        {
            try
            {
                List<ReceiptViewModel> list = (from receipt in dbContext.receipts
                                               orderby receipt.id ascending
                                               select new ReceiptViewModel
                                               {
                                                   Id = receipt.id,
                                                   Code = receipt.code,
                                                   Date = receipt.date.ToString("dd/MM/yyyy"),
                                                   Grand_total = receipt.grand_total,
                                               }).ToList();

                return new Response<List<ReceiptViewModel>>
                {
                    Code = 200,
                    Message = "Success",
                    Data = list,
                };
            }
            catch (Exception e)
            {
                var code = 500;
                var message = e.InnerException == null ? e.Message : e.InnerException.Message;

                return new Response<List<ReceiptViewModel>>
                {
                    Code = code,
                    Message = "Error: " + message,
                    Data = null,
                };
            }
        }

        public Response<List<ReceiptViewModel>> FindByDateRange(string start, string end)
        {
            try
            {
                DateOnly startDate = start == "NaN/NaN/NaN" ? DateOnly.MinValue : DateOnly.ParseExact(start, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateOnly endDate = end == "NaN/NaN/NaN" ? DateOnly.MaxValue : DateOnly.ParseExact(end, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                List<ReceiptViewModel> list = (from receipt in dbContext.receipts
                                               where receipt.date >= startDate && receipt.date < endDate
                                               select new ReceiptViewModel
                                               {
                                                   Id = receipt.id,
                                                   Code = receipt.code,
                                                   Date = receipt.date.ToString("dd/MM/yyyy"),
                                                   Grand_total = receipt.grand_total,
                                               }).ToList();

                return new Response<List<ReceiptViewModel>>
                {
                    Code = 200,
                    Message = "Success",
                    Data = list,
                };
            }
            catch (Exception e)
            {
                var code = 500;
                var message = e.InnerException == null ? e.Message : e.InnerException.Message;

                return new Response<List<ReceiptViewModel>>
                {
                    Code = code,
                    Message = "Error: " + message,
                    Data = null,
                };
            }
        }

        public Response<ReceiptDetailViewModel> FindOne(int id)
        {
            try
            {
                /*ReceiptDetailViewModel data = (from receipt in dbContext.receipts
                                               join order in dbContext.orders
                                               on receipt.id equals order.receipt
                                               where receipt.id == id
                                               orderby receipt.id ascending
                                               select new ReceiptDetailViewModel
                                               {
                                                   Id = receipt.id,
                                                   Code = receipt.code,
                                                   Date = receipt.date.ToString("dd/MM/yyyy"),
                                                   Total_price = Math.Round(receipt.orders.Sum(order => order.item_price * order.item_quantity), 2),
                                                   Total_discount = Math.Round(receipt.orders.Sum(order => order.item_price * order.item_quantity * (order.item_discount_percent / 100)), 2),
                                                   Subtotal = Math.Round(receipt.orders.Sum(order => order.item_price * order.item_quantity) -
                                                        receipt.orders.Sum(order => order.item_price * order.item_quantity * (order.item_discount_percent / 100)), 2),
                                                   Trade_discount = receipt.trade_discount,
                                                   Grand_total = Math.Round(receipt.orders.Sum(order => order.item_price * order.item_quantity) -
                                                        receipt.orders.Sum(order => order.item_price * order.item_quantity * (order.item_discount_percent / 100)) - receipt.trade_discount, 2),
                                                   Orders = receipt.orders.Select(order => new OrderViewModel
                                                   {
                                                       Id = order.id,
                                                       Item_code = order.item_code,
                                                       Item_name = order.item_name,
                                                       Item_unit = order.item_unit,
                                                       Item_price = order.item_price,
                                                       Item_quantity = order.item_quantity,
                                                       Item_discount_percent = order.item_discount_percent,
                                                       Item_discount = Math.Round(order.item_price * order.item_quantity * (order.item_discount_percent / 100), 2),
                                                       Item_total_price = Math.Round(order.item_price * order.item_quantity * ((100 - order.item_discount_percent) / 100), 2),
                                                   }).ToList(),
                                               }).FirstOrDefault();*/

                
                receipt receiptData = (from receipt in dbContext.receipts where receipt.id == id select receipt).FirstOrDefault();
                List<order> ordersData = (from order in dbContext.orders where order.receipt_id == id select order).ToList();

                if (receiptData != null)
                {
                    ReceiptDetailViewModel receipt = new ReceiptDetailViewModel();
                    List<OrderViewModel> orders = new List<OrderViewModel>();

                    foreach (var order in ordersData)
                    {
                        OrderViewModel orderModel = new OrderViewModel();
                        orderModel.Id = order.id;
                        orderModel.Item_code = order.item_code;
                        orderModel.Item_name = order.item_name;
                        orderModel.Item_unit = order.item_unit;
                        orderModel.Item_price = order.item_price;
                        orderModel.Item_quantity = order.item_quantity;
                        orderModel.Item_discount_percent = order.item_discount_percent;
                        orderModel.Item_discount_amount = order.item_discount_amount;
                        orderModel.Item_total_amount = order.item_total_amount;

                        orders.Add(orderModel);
                    }

                    receipt.Id = receiptData.id;
                    receipt.Code = receiptData.code;
                    receipt.Date = receiptData.date.ToString("dd/MM/yyyy");
                    receipt.Total_amount = receiptData.total_amount;
                    receipt.Total_discount_amount = receiptData.total_discount_amount;
                    receipt.Subtotal = receiptData.subtotal;
                    receipt.Trade_discount = receiptData.trade_discount;
                    receipt.Grand_total = receiptData.grand_total;
                    receipt.Orders = orders;

                    return new Response<ReceiptDetailViewModel>
                    {
                        Code = 0,
                        Message = "Success",
                        Data = receipt,
                    };
                }
                else
                {
                    throw new ArgumentNullException("id: " + id.ToString(), "Receipt not found");
                }

                /*List<ReceiptDetailViewModel> data = (from receipt in dbContext.receipts
                                                     join order in dbContext.orders
                                                     on receipt.id equals order.receipt_id
                                                     where receipt.id == id
                                                     orderby receipt.id ascending
                                                     select new ReceiptDetailViewModel
                                                     {
                                                         Id = receipt.id,
                                                         Code = receipt.code,
                                                         Date = receipt.date.ToString("dd/MM/yyyy"),
                                                         Total_amount = receipt.total_amount,
                                                         Total_discount_amount = receipt.total_discount_amount,
                                                         Subtotal = receipt.subtotal,
                                                         Trade_discount = receipt.trade_discount,
                                                         Grand_total = receipt.grand_total,
                                                         Orders = receipt.orders.Select(order => new OrderViewModel
                                                         {
                                                             Id = order.id,
                                                             Item_code = order.item_code,
                                                             Item_name = order.item_name,
                                                             Item_unit = order.item_unit,
                                                             Item_price = order.item_price,
                                                             Item_quantity = order.item_quantity,
                                                             Item_discount_percent = order.item_discount_percent,
                                                             Item_discount_amount = order.item_discount_amount,
                                                             Item_total_amount = order.item_total_amount,
                                                         }).ToList(),
                                                     }).ToList();

                if (data != null)
                {
                    return new Response<ReceiptDetailViewModel>
                    {
                        Code = 200,
                        Message = "Success",
                        Data = data[0],
                    };
                }
                else
                {
                    throw new ArgumentNullException("id: " + id.ToString(), "Receipt not found");
                }*/
            }
            catch (Exception e)
            {
                var code = 500;
                var message = e.InnerException == null ? e.Message : e.InnerException.Message;

                if (message.Contains("Cannot add or update a child row: a foreign key constraint fails"))
                {
                    message = "Unit used not exist";
                }

                return new Response<ReceiptDetailViewModel>
                {
                    Code = code,
                    Message = "Error: " + message,
                    Data = null,
                };
            }
            
        }

        public Response<ReceiptDetailViewModel> Create(Receipt receipt)
        {
            const string prefix = "T";

            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (Order order in receipt.Orders)
                    {
                        if (order.Item_quantity == 0)
                        {
                            throw new ArgumentException("Item's quantity is empty");
                        }
                    }

                    var lastItem = dbContext.receipts.OrderByDescending(data => data.id).FirstOrDefault();
                    var newBillCode = "";

                    if (lastItem != null)
                    {
                        string[] codeSplitted = lastItem.code.Split(prefix);

                        string runningPart = codeSplitted[codeSplitted.Length - 1];

                        int newRunningPart = Int32.Parse(runningPart) + 1;

                        newBillCode = $"{prefix}{newRunningPart:D4}";
                    }
                    else
                    {
                        newBillCode = $"{prefix}{1:D4}";
                    }

                    /*if (lastItem != null)
                    {
                        var receiptCode = lastItem.code;
                        var isCarried = false;
                        
                        for (int i = receiptCode.Length - 1; i >= 0; i--)
                        {
                            var curChar = receiptCode[i];

                            if(IsDigit(curChar))
                            {
                                var curCharInt = ToInt(curChar);

                                if (isCarried || i + 1 == receiptCode.Length)
                                {
                                    curCharInt += 1;
                                    isCarried = false;

                                    if (curCharInt >= 10)
                                    {
                                        isCarried = true;
                                        curCharInt %= 10;
                                    }

                                    curChar = ToChar(curCharInt);
                                }
                            }

                            newBillCode = curChar + newBillCode;
                        }
                    }
                    else
                    {
                        newBillCode = $"{prefix}{1:D4}";
                    }*/

                    /*int id;

                    if (lastItem != null)
                    {
                        id = lastItem.id + 1;
                    }
                    else
                    {
                        id = 1;
                    }

                    var numPart = id.ToString();

                    if (id < 10)
                    {
                        numPart = "000" + numPart;
                    }
                    else if (id < 100)
                    {
                        numPart = "00" + numPart;
                    }
                    else if (id < 1000)
                    {
                        numPart = "0" + numPart;
                    }

                    newBillCode = "T" + numPart;*/

                    receipt bill = new receipt
                    {
                        code = newBillCode,
                        date = DateOnly.ParseExact(receipt.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        total_amount = receipt.Total_amount,
                        total_discount_amount = receipt.Total_discount_amount,
                        subtotal = receipt.Subtotal,
                        trade_discount = receipt.Trade_discount,
                        grand_total = receipt.Grand_total,
                    };
                    
                    dbContext.receipts.Add(bill);
                    dbContext.SaveChanges();

                    int newBillId = bill.id;

                    foreach (Order order in receipt.Orders)
                    {
                        order newOrder = new order
                        {
                            receipt_id = newBillId,
                            item_id = order.Item_id,
                            item_code = order.Item_code,
                            item_name = order.Item_name,
                            item_unit = order.Item_unit,
                            unit_id = order.Item_unit_id,
                            item_quantity = order.Item_quantity,
                            item_price = order.Item_price,
                            item_discount_percent = order.Item_discount_percent,
                            item_discount_amount = order.Item_discount_amount,
                            item_total_amount = order.Item_total_amount,
                        };

                        dbContext.orders.Add(newOrder);
                        dbContext.SaveChanges();
                    }

                    dbContextTransaction.Commit();

                    return new Response<ReceiptDetailViewModel>
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

                    return new Response<ReceiptDetailViewModel>
                    {
                        Code = code,
                        Message = "Error: " + message,
                        Data = null,
                    };
                }
            }

            /*bool IsDigit(char letter)
            {
                return (letter >= '0' && letter <= '9');
            }

            int ToInt(char letter)
            {
                return letter - '0';
            }

            char ToChar(int digit)
            {
                return (char)(digit + '0');
            }*/
        }
    }
}
