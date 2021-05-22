using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ToSoftware.Shop.Carts.Api.Messages
{
    [DataContract]
    public class CartResponseMessage
    {
        [DataMember]
        public Guid Code { get; set; }

        [DataMember]
        public List<ProductRequestMessage> Products { get; set; } = new List<ProductRequestMessage>();

        [DataContract]
        public class ProductRequestMessage
        {
            [DataMember]
            public Guid Code { get; set; }

            [DataMember]
            public int Quantity { get; set; }
        }
    }
}