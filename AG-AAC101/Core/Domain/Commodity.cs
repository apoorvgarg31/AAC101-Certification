using System;

namespace AG_AAC101.Data.Commodity
{
    public class Commodity
    {
        private BlobDbContext blobDbContext;

        public Commodity()
        {
        }

       

        public int ID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public string Unit { get; set; }
        public string EstimatedQuantity { get; set; }
        public string ActualQuantity { get; set; }

      
    }

}
