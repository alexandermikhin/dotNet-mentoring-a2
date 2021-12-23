using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetailEquity.Model;
using System.Collections.Generic;
using System.Linq;

namespace RetailEquity.Tests
{
    [TestClass]
    public class TradeFilterTests
    {
        private TradeFilter tradeFilter;

        public TradeFilterTests()
        {
            this.tradeFilter = new TradeFilter();
        }

        [TestMethod]
        public void Should_filter_for_Bofa_bank()
        {
            var filteredTrades = this.tradeFilter.FilterForBank(AllTrades, Bank.Bofa);
            var expectedIds = new int[] { 9, 10, 11, 12 };

            filteredTrades.Select(t => t.Id).ToArray().Should().BeEquivalentTo(expectedIds);
        }

        [TestMethod]
        public void Should_filter_for_Connacord_bank()
        {
            var filteredTrades = this.tradeFilter.FilterForBank(AllTrades, Bank.Connacord);
            var expectedIds = new int[] { 5, 6 };

            filteredTrades.Select(t => t.Id).ToArray().Should().BeEquivalentTo(expectedIds);
        }

        [TestMethod]
        public void Should_filter_for_Barclays_bank()
        {
            var filteredTrades = this.tradeFilter.FilterForBank(AllTrades, Bank.Barclays);
            var expectedIds = new int[] { 12 };

            filteredTrades.Select(t => t.Id).ToArray().Should().BeEquivalentTo(expectedIds);
        }

        private IEnumerable<Trade> AllTrades 
        {
            get 
            {
                return new List<Trade>
                {
                    new Trade
                    {
                        Id = 1,
                        Amount = 10,
                        Type = TradeType.Future,
                        SubType = TradeSubType.NewOption
                    },
                    new Trade
                    {
                        Id = 2,
                        Amount = 15,
                        Type = TradeType.Option,
                        SubType = TradeSubType.NewOption
                    },
                    new Trade
                    {
                        Id = 3,
                        Amount = 10,
                        Type = TradeType.Future,
                        SubType = TradeSubType.NyOption
                    },
                    new Trade
                    {
                        Id = 4,
                        Amount = 15,
                        Type = TradeType.Option,
                        SubType = TradeSubType.NyOption
                    },
                    new Trade
                    {
                        Id = 5,
                        Amount = 30,
                        Type = TradeType.Future,
                        SubType = TradeSubType.NewOption
                    },
                    new Trade
                    {
                        Id = 6,
                        Amount = 30,
                        Type = TradeType.Future,
                        SubType = TradeSubType.NyOption
                    },
                    new Trade
                    {
                        Id = 7,
                        Amount = 30,
                        Type = TradeType.Option,
                        SubType = TradeSubType.NewOption
                    },
                    new Trade
                    {
                        Id = 8,
                        Amount = 30,
                        Type = TradeType.Option,
                        SubType = TradeSubType.NyOption
                    },
                    new Trade
                    {
                        Id = 9,
                        Amount = 90,
                        Type = TradeType.Future,
                        SubType = TradeSubType.NewOption
                    },
                    new Trade
                    {
                        Id = 10,
                        Amount = 90,
                        Type = TradeType.Future,
                        SubType = TradeSubType.NyOption
                    },
                    new Trade
                    {
                        Id = 11,
                        Amount = 95,
                        Type = TradeType.Option,
                        SubType = TradeSubType.NewOption
                    },
                    new Trade
                    {
                        Id = 12,
                        Amount = 90,
                        Type = TradeType.Option,
                        SubType = TradeSubType.NyOption
                    },
                };
            }
        }
    }
}
