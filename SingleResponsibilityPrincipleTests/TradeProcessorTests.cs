using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleResponsibilityPrinciple;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.Tests
{
    [TestClass()]
    public class TradeProcessorTests
    {
        /*[TestMethod()]
        public void ProcessTradesTest()
        {
            //Assert.Fail();
        }*/

        private int CountDbRecords()
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Jenni\Documents\tradedatabase.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string myScalarQuery = "SELECT COUNT(*) FROM trade";
                SqlCommand myCommand = new SqlCommand(myScalarQuery, connection);
                //myCommand.Connection.Open();
                int count = (int)myCommand.ExecuteScalar();
                connection.Close();
                return count;
            }
        }

        [TestMethod()]
        public void TestBadTrade_NormalTradeFile()
        {
            // Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.goodTrades.txt");
            var tradeProcessor = new TradeProcessor();

            // Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            // Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore+4, countAfter);
        }

        [TestMethod()]
        public void TestBadTradeFile()
        {
            // Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.badTrades.txt");
            var tradeProcessor = new TradeProcessor();

            // Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            // Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }

        // As a programmer I want to only keep track of positive quantities so that the buyer does not get a refund from buying. 
        [TestMethod()]
        public void TestBadTrade_negQty()
        {
            // Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.badTrades_negQty.txt");
            var tradeProcessor = new TradeProcessor();

            // Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            // Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }

        // As a programmer I want to make sure no transactions with negative prices get logged so that the user doesn't make money from buying.
        [TestMethod()]
        public void TestBadTrade_negPrice()
        {
            // Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.badTrades_negPrice.txt");
            var tradeProcessor = new TradeProcessor();

            // Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            // Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }

        [TestMethod()]
        // User Story: As a programmer I want to make sure that the user only enters a price with, at most, 2 decimal places so that calculations don't get to complex.
        public void TestBadTrade_ExtraDecimalPlace()
        {
            // Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.badTrades_extraDecimalPlace.txt");
            var tradeProcessor = new TradeProcessor();

            // Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            // Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }

        // User Story: As a programmer I want to make sure quantity does not exceed one million so that the consumer cannot buy to much. 
        [TestMethod()]
        public void TestBadTrade_QuantityOneMillion()
        {
            // Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.badTrades_QtyOneMill.txt");
            var tradeProcessor = new TradeProcessor();

            // Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            // Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }

        // User Story: As a programmer I want to make sure that any zero Quantity exchange does not go through so that less memory is used. 
        [TestMethod()]
        public void TestBadTrade_ZeroQty()
        {
            // Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.badTrades_ZeroQty.txt");
            var tradeProcessor = new TradeProcessor();

            // Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            // Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }

        // User Story: As a programmer I want to make sure any transaction going for $0 doesn't get logged so that the program can conserve on memory space. 
        [TestMethod()]
        public void TestBadTrade_ZeroForPrice()
        {
            // Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.badTrades_zeroForPrice.txt");
            var tradeProcessor = new TradeProcessor();

            // Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            // Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }
    }
}