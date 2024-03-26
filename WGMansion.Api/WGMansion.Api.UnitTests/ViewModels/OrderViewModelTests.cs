using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGMansion.Api.Models;
using WGMansion.Api.Models.Ticker;
using WGMansion.Api.ViewModels;

namespace WGMansion.Api.UnitTests.ViewModels
{
    [TestFixture]
    internal class OrderViewModelTests
    {
        private OrderViewModel _sut;
        private Mock<IAccountsViewModel> _accountsViewModel;
        private Mock<ITickerViewModel> _tickerViewModel;

        [SetUp]
        public void Setup()
        {
            _accountsViewModel = new Mock<IAccountsViewModel>();
            _tickerViewModel = new Mock<ITickerViewModel>();
            _sut = new OrderViewModel(_accountsViewModel.Object,_tickerViewModel.Object);
        }

        [Test]
        public async Task TestAddOrder()
        {
            var account = new Account
            {
                Id = "123",
                Portfolio = new Portfolio()
            };
            var order = new Order
            {
                OrderType = OrderType.MarketBuy,
                OwnerId = "123",
                Symbol = "ABC",
            };
            var ticker = new Ticker
            {
                Symbol = "ABC",
                BuyOrders = new List<Order>(),
                SellOrders = new List<Order>(),
            };

            _accountsViewModel.Setup(x => x.GetAccount("123")).ReturnsAsync(account);
            _tickerViewModel.Setup(x => x.GetTicker("ABC")).ReturnsAsync(ticker);
            
            var result = await _sut.AddOrder(order, "123");

            Assert.That(result,Is.Not.Null);
            Assert.That(ticker.BuyOrders.Contains(order));
            Assert.That(account.Portfolio.Stocks.Any(x=>x.Symbol == "ABC"));
            Assert.That(account.Portfolio.Stocks.First(x => x.Symbol == "ABC").Orders.Contains(order.Id));
        }

        [Test]
        public async Task TestAddOrderMarketBuy()
        {
            var sellOrderOne = new Order
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OwnerId = "321",
                Quantity = 50,
                Price = 100,
                Symbol = "ABC",
                OrderType = OrderType.LimitSell
            };

            var sellOrderTwo = new Order
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OwnerId = "321",
                Quantity = 75,
                Price = 100,
                Symbol = "ABC",
                OrderType = OrderType.LimitSell
            };


            var account = new Account
            {
                Id = "123",
                Portfolio = new Portfolio()
            };

            var sellerAccount = new Account
            {
                Id = "321",
                Portfolio = new Portfolio()
                {
                    Stocks = new List<Stock>
                    {
                        new Stock
                        {
                            Symbol = "ABC",
                            Orders = new List<string>
                            {
                                sellOrderOne.Id,
                                sellOrderTwo.Id
                            }
                        }
                    }
                }
            };

            var order = new Order
            {
                OrderType = OrderType.MarketBuy,
                OwnerId = "123",
                Symbol = "ABC",
                Quantity = 100,
                Price = 100,
            };
            var ticker = new Ticker
            {
                Symbol = "ABC",
                BuyOrders = new List<Order>(),
                SellOrders = new List<Order>
                {
                    sellOrderOne,
                    sellOrderTwo,
                },
            };

            _accountsViewModel.Setup(x => x.GetAccount("123")).ReturnsAsync(account);
            _accountsViewModel.Setup(x => x.GetAccount("321")).ReturnsAsync(sellerAccount);
            _tickerViewModel.Setup(x => x.GetTicker("ABC")).ReturnsAsync(ticker);

            var result = await _sut.AddOrder(order, "123");

            Assert.That(result, Is.Not.Null);
            Assert.That(ticker.BuyOrders.Count == 0);
            Assert.That(ticker.SellOrders.Count == 1);
            Assert.That(ticker.SellOrders.First().Quantity, Is.EqualTo(25));
            Assert.That(account.Portfolio.Stocks.Any(x => x.Symbol == "ABC"));
            Assert.That(account.Portfolio.Stocks.First(x => x.Symbol == "ABC").Orders.Count == 0);
            Assert.That(sellerAccount.Portfolio.Stocks.First(x => x.Symbol == "ABC").Orders.Count == 1);
        }

        [Test]
        public async Task TestAddOrderLimitBuy()
        {
            var sellOrderOne = new Order
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OwnerId = "321",
                Quantity = 50,
                Price = 100,
                Symbol = "ABC",
                OrderType = OrderType.LimitSell
            };

            var sellOrderTwo = new Order
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OwnerId = "321",
                Quantity = 75,
                Price = 200,
                Symbol = "ABC",
                OrderType = OrderType.LimitSell
            };


            var account = new Account
            {
                Id = "123",
                Portfolio = new Portfolio()
            };

            var sellerAccount = new Account
            {
                Id = "321",
                Portfolio = new Portfolio()
                {
                    Stocks = new List<Stock>
                    {
                        new Stock
                        {
                            Symbol = "ABC",
                            Orders = new List<string>
                            {
                                sellOrderOne.Id,
                                sellOrderTwo.Id
                            }
                        }
                    }
                }
            };

            var order = new Order
            {
                OrderType = OrderType.LimitBuy,
                OwnerId = "123",
                Symbol = "ABC",
                Quantity = 100,
                Price = 100,
            };
            var ticker = new Ticker
            {
                Symbol = "ABC",
                BuyOrders = new List<Order>(),
                SellOrders = new List<Order>
                {
                    sellOrderOne,
                    sellOrderTwo,
                },
            };

            _accountsViewModel.Setup(x => x.GetAccount("123")).ReturnsAsync(account);
            _accountsViewModel.Setup(x => x.GetAccount("321")).ReturnsAsync(sellerAccount);
            _tickerViewModel.Setup(x => x.GetTicker("ABC")).ReturnsAsync(ticker);

            var result = await _sut.AddOrder(order, "123");

            Assert.That(result, Is.Not.Null);
            Assert.That(ticker.BuyOrders.Count,Is.EqualTo(1));
            Assert.That(ticker.SellOrders.Count, Is.EqualTo(1));
            Assert.That(ticker.SellOrders.First().Quantity, Is.EqualTo(75));
            Assert.That(ticker.BuyOrders.First().Quantity, Is.EqualTo(50));
            Assert.That(account.Portfolio.Stocks.Any(x => x.Symbol == "ABC"));
            Assert.That(account.Portfolio.Stocks.First(x => x.Symbol == "ABC").Orders.Count, Is.EqualTo(1));
            Assert.That(sellerAccount.Portfolio.Stocks.First(x => x.Symbol == "ABC").Orders.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task TestAddOrderMarketSell()
        {
            var buyOrderOne = new Order
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OwnerId = "321",
                Quantity = 50,
                Price = 80,
                Symbol = "ABC",
                OrderType = OrderType.LimitBuy
            };

            var buyOrderTwo = new Order
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OwnerId = "321",
                Quantity = 75,
                Price = 100,
                Symbol = "ABC",
                OrderType = OrderType.LimitBuy
            };


            var account = new Account
            {
                Id = "123",
                Portfolio = new Portfolio()
            };

            var sellerAccount = new Account
            {
                Id = "321",
                Portfolio = new Portfolio()
                {
                    Stocks = new List<Stock>
                    {
                        new Stock
                        {
                            Symbol = "ABC",
                            Orders = new List<string>
                            {
                                buyOrderOne.Id,
                                buyOrderTwo.Id
                            }
                        }
                    }
                }
            };

            var order = new Order
            {
                OrderType = OrderType.MarketSell,
                OwnerId = "123",
                Symbol = "ABC",
                Quantity = 100,
            };
            var ticker = new Ticker
            {
                Symbol = "ABC",
                BuyOrders = new List<Order>
                {
                    buyOrderOne,
                    buyOrderTwo,
                },
                SellOrders = new List<Order>()
            };

            _accountsViewModel.Setup(x => x.GetAccount("123")).ReturnsAsync(account);
            _accountsViewModel.Setup(x => x.GetAccount("321")).ReturnsAsync(sellerAccount);
            _tickerViewModel.Setup(x => x.GetTicker("ABC")).ReturnsAsync(ticker);

            var result = await _sut.AddOrder(order, "123");

            Assert.That(result, Is.Not.Null);
            Assert.That(ticker.SellOrders.Count == 0);
            Assert.That(ticker.BuyOrders.Count == 1);
            Assert.That(ticker.BuyOrders.First().Quantity, Is.EqualTo(25));
            Assert.That(ticker.BuyOrders.First().Price, Is.EqualTo(80));
            Assert.That(account.Portfolio.Money, Is.EqualTo(9500));
            Assert.That(sellerAccount.Portfolio.Money, Is.EqualTo(-9500));
            Assert.That(account.Portfolio.Stocks.Any(x => x.Symbol == "ABC"));
            Assert.That(account.Portfolio.Stocks.First(x => x.Symbol == "ABC").Orders.Count == 0);
            Assert.That(sellerAccount.Portfolio.Stocks.First(x => x.Symbol == "ABC").Orders.Count == 1);
        }

        [Test]
        public async Task TestAddOrderLimitSell()
        {
            var buyOrderOne = new Order
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OwnerId = "321",
                Quantity = 50,
                Price = 80,
                Symbol = "ABC",
                OrderType = OrderType.LimitBuy
            };

            var buyOrderTwo = new Order
            {
                Id = ObjectId.GenerateNewId().ToString(),
                OwnerId = "321",
                Quantity = 75,
                Price = 100,
                Symbol = "ABC",
                OrderType = OrderType.LimitBuy
            };


            var account = new Account
            {
                Id = "123",
                Portfolio = new Portfolio()
            };

            var sellerAccount = new Account
            {
                Id = "321",
                Portfolio = new Portfolio()
                {
                    Stocks = new List<Stock>
                    {
                        new Stock
                        {
                            Symbol = "ABC",
                            Orders = new List<string>
                            {
                                buyOrderOne.Id,
                                buyOrderTwo.Id
                            }
                        }
                    }
                }
            };

            var order = new Order
            {
                OrderType = OrderType.LimitSell,
                OwnerId = "123",
                Symbol = "ABC",
                Quantity = 100,
                Price = 90,
            };
            var ticker = new Ticker
            {
                Symbol = "ABC",
                BuyOrders = new List<Order>
                {
                    buyOrderOne,
                    buyOrderTwo,
                },
                SellOrders = new List<Order>()
            };

            _accountsViewModel.Setup(x => x.GetAccount("123")).ReturnsAsync(account);
            _accountsViewModel.Setup(x => x.GetAccount("321")).ReturnsAsync(sellerAccount);
            _tickerViewModel.Setup(x => x.GetTicker("ABC")).ReturnsAsync(ticker);

            var result = await _sut.AddOrder(order, "123");

            Assert.That(result, Is.Not.Null);
            Assert.That(ticker.SellOrders.Count == 1);
            Assert.That(ticker.BuyOrders.Count == 1);
            Assert.That(ticker.BuyOrders.First().Quantity, Is.EqualTo(50));
            Assert.That(ticker.BuyOrders.First().Price, Is.EqualTo(80));
            Assert.That(ticker.SellOrders.First().Quantity, Is.EqualTo(25));
            Assert.That(account.Portfolio.Money, Is.EqualTo(7500));
            Assert.That(sellerAccount.Portfolio.Money, Is.EqualTo(-7500));
            Assert.That(account.Portfolio.Stocks.Any(x => x.Symbol == "ABC"));
            Assert.That(account.Portfolio.Stocks.First(x => x.Symbol == "ABC").Orders.Count == 1);
            Assert.That(sellerAccount.Portfolio.Stocks.First(x => x.Symbol == "ABC").Orders.Count == 1);
        }

    }
}
