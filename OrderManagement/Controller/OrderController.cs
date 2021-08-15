using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement
{
    public class OrderController : Controller
    {
        //Declaration
        private IConfiguration _config;

        //Constructor
        public OrderController(IConfiguration configuration)
        {
            _config = configuration;
        }

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Order Details
        /// <summary>
        /// Order Details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult OrderDetails()
        {
            return View();
        }
        #endregion

        #region Get Orders List Json
        /// <summary>
        /// Get Orders List Json
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetOrdersListJson()
        {
            List<Order> orderList = new List<Order>();
            int totalCount = 0;
            using (var httpClient = new HttpClient())
            {
                //Base address
                httpClient.BaseAddress = new Uri(_config.GetSection("OrderManagementAPIUrl").Value);
                string uri = "api/Order/GetOrderList";
                var response = httpClient.GetAsync(uri).Result;
                if (response != null && response.IsSuccessStatusCode)
                {
                    orderList = JsonConvert.DeserializeObject<List<Order>>(response.Content.ReadAsStringAsync().Result);
                }
            }
            totalCount = orderList.Count;
            var result = (from r in orderList
                          select new[] {
                          r.OrderId.ToString(), //0
                          r.OrderDate.ToString("dd MMM yyyy"), //1
                          r.OrderNumber.ToString(), //2
                          r.OrderAmount.ToString(), //3
                          r.VendorName, //4
                          r.Shop.ToString(), //5
                          });
            return Json(new { iTotalRecords = totalCount, iTotalDisplayRecords = totalCount, aaData = result });
        }

        #endregion

        #region Add Order
        /// <summary>
        /// Add Order - Dialog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult _AddOrder(long? id)
        {
            Order orderDetails = null;
            if (id > 0)
            {
                orderDetails = new Order();
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_config.GetSection("OrderManagementAPIUrl").Value);
                    string uri = "api/Order/GetOrderById";
                    var response = httpClient.GetAsync(uri + "/?orderId=" + id).Result;
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        orderDetails = JsonConvert.DeserializeObject<Order>(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
            return PartialView(orderDetails);
        }

        #endregion

        #region Save Order
        /// <summary>
        /// Save Order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveOrder(Order order)
        {
            Order orderResponse = new Order();
            bool isSaved = false;
            if (order != null)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_config.GetSection("OrderManagementAPIUrl").Value);
                    string uri = string.Empty;
                    StringContent content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

                    if (order.OrderId > 0)
                    {
                        uri = "api/Order/UpdateOrder";
                        var response = httpClient.PutAsync(uri, content).Result;
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            orderResponse = JsonConvert.DeserializeObject<Order>(response.Content.ReadAsStringAsync().Result);
                            if (orderResponse.OrderId > 0 && orderResponse.Status == "SUCCESS")
                            {
                                isSaved = true;
                            }
                        }
                    }
                    else
                    {
                        uri = "api/Order/SaveOrder";
                        var response = httpClient.PostAsync(uri, content).Result;
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            orderResponse = JsonConvert.DeserializeObject<Order>(response.Content.ReadAsStringAsync().Result);
                            if (orderResponse.OrderId > 0 && orderResponse.Status == "SUCCESS")
                            {
                                isSaved = true;
                            }
                        }
                    }

                }
            }
            return Json(isSaved);
        }
        #endregion

        #region Delete Order
        /// <summary>
        /// Delete Order
        /// </summary>
        /// <param name="OrderIdList"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteOrder(long[] OrderIdList)
        {
            bool isDeleted = false;
            if (OrderIdList != null && OrderIdList.Length > 0)
            {
                DeleteOrder deleteOrderRequest = new DeleteOrder();
                deleteOrderRequest.OrderIds = OrderIdList;
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_config.GetSection("OrderManagementAPIUrl").Value);
                    string uri = "api/Order/DeleteOrders";
                    StringContent content = new StringContent(JsonConvert.SerializeObject(deleteOrderRequest), Encoding.UTF8, "application/json");
                    var response = httpClient.PostAsync(uri, content).Result;
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        var deleteOrderResponse = JsonConvert.DeserializeObject<DeleteOrder>(response.Content.ReadAsStringAsync().Result);
                        if (deleteOrderResponse != null && deleteOrderResponse.Status == "SUCCESS")
                        {
                            isDeleted = true;
                        }
                    }
                }
            }
            return Json(isDeleted);
        }

        #endregion

    }
}
