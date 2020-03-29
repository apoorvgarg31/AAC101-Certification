using AG_AAC101;
using AG_AAC101.Data.Commodity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AG_AAC101_Test
{
    public class CommodityTEST
    {
        [Fact]
        public void Diplay_Commodity_bycode_True()
        {
            //Arrange
            var expected = 14;

            //Act
            BlobDbContext db = new BlobDbContext();
            var actual = db.Commodities.Find(14);

            //Assert
            Assert.Equal(expected, actual.ID);
        }

        [Fact]
        public void Diplay_Commodity_bycode_False()
        {
            //Arrange
            var expected = 15;

            //Act
            BlobDbContext db = new BlobDbContext();
            var actual = db.Commodities.Find(14);

            //Assert
            Assert.NotEqual(expected, actual.ID);
        }

        [Fact]
        public void CheckCommodityCount()
        {
            var expected = 9;
            Commodity com = new Commodity();

            //Act
            BlobDbContext db = new BlobDbContext();
            var actual = db.Commodities.CountAsync();

            //Assert
            Assert.NotEqual(expected, actual.Result);
        }

      
    }
}
