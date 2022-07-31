using DotNetCoreAngular.Helpers.Pagination;

namespace DotNetCoreAngular.Dtos
{
    public class MessageThreadDto
    {
        public List<MessageDto> Messages { get; set; }
        public PaginationHeader PaginationHeader { get; set; }
    }
}
