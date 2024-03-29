﻿namespace Web.HttpAggregator.Config
{
    public class UrlsOptions
    {
        public const string Urls = "Urls";

        public string RestaurantsGrpc { get; set; }

        public string OrdersGrpc { get; set; }

        public string OrderQueueGrpc { get; set; }

        public string DishesGrpc { get; set; }

        public string MenuGrpc { get; set; }

        public string TablesGrpc { get; set; }

        public string ResourcesGrpc { get; set; }

        public string UsersGrpc { get; set; }
    }
}
