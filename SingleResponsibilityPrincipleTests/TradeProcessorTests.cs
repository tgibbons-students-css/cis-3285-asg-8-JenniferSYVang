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
        public void TestNormalTradeFile()
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
        public void TestSpacesAfterComma()
        {
            // Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.badTrades_spacesAfterComma.txt");
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