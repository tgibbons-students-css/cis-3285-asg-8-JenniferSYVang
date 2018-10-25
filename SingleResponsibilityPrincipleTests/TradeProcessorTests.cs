using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleResponsibilityPrinciple;
using System;
using System.Collections.Generic;
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
        [TestMethod()]
        public void ProcessTradesTest()
        {
            //Assert.Fail();
        }
        private int CountDbRecords()
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\jvang5\source\repos\cis-3285-asg-8-JenniferSYVang\tradedatabase.mdf;Integrated Security=True;Connect Timeout=30;"))
            {
                connection.Open();
                string myScalarQuery = "SELECT COUNT(*) FROM trade";
                SqlCommand myCommand = new SqlCommand(myScalarQuery, connection);
                myCommand.Connection.Open();
                int count = (int)myCommand.ExecuteScalar();
                connection.Close();
                return count;
            }
        }

        [TestMethod()]
        public void TestNormalFile()
        {
            // Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrinciple.goodTrades.txt");
            var tradeProcessor = new TradeProcessor();

            // Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            // Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore+4, countAfter);
        }

    }
}