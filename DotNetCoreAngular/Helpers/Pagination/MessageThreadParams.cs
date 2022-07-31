﻿namespace DotNetCoreAngular.Helpers.Pagination
{
    public class MessageThreadParams : PaginationParams
    {
        public int CurrentUserId { get; set; }
        public int RecipientUserId { get; set; }

        public string RecipientUsername { get; set; }
    }
}
