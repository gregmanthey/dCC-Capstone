﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone
{
    public class SpotifyArtistsSearchJsonResponse
    {
        public class Rootobject
        {
            public Artists artists { get; set; }
        }

        public class Artists
        {
            public string href { get; set; }
            public Item[] items { get; set; }
            public int limit { get; set; }
            public object next { get; set; }
            public int offset { get; set; }
            public object previous { get; set; }
            public int total { get; set; }
        }

        public class Item
        {
            public External_Urls external_urls { get; set; }
            public string[] genres { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public Image[] images { get; set; }
            public string name { get; set; }
            public int popularity { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class External_Urls
        {
            public string spotify { get; set; }
        }

        public class Image
        {
            public int height { get; set; }
            public string url { get; set; }
            public int width { get; set; }
        }

    }
}